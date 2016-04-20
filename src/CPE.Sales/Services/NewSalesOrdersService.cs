using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CPE.Domain.Model;
using CPE.Domain.Services;
using CPE.Sales.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;

namespace CPE.Sales.Services
{
    public class NewSalesOrdersService
    {
        private readonly ICustomerService _customers;
        private readonly MemoryCache _memoryCache = new MemoryCache("NewSalesOrderServiceCache");
        private readonly OpenOrderReportParserService _openOrderParserService;
        private readonly IPartService _parts;
        private readonly IPhotoService _photos;
        private readonly ITricornService _tricorn;
        private IEnumerable<ICustomer> _parseableCustomers;

        public NewSalesOrdersService(ICustomerService customerService, IPhotoService photoService,
            IPartService partService, ITricornService tricornService,
            OpenOrderReportParserService openOrderParserService)
        {
            _customers = customerService;
            _parts = partService;
            _photos = photoService;

            _tricorn = tricornService;

            _openOrderParserService = openOrderParserService;
        }

        public async Task<List<NewSalesOrder>> GetNewSalesOrdersAsync()
        {
            var newSalesOrders = new List<NewSalesOrder>();

            var newMail = await Task.Factory.StartNew(() => MSOutlookService.GetSalesOrderMail());

            foreach (var mail in newMail)
            {
                var orderNumber = await GetOrderNumberAsync(mail);
                var customer = await GetCustomerAsync(mail);
                var buyer = await GetBuyerAsync(mail);
                var totalValue = await GetTotalValueAsync(mail);

                var lines = new List<SalesOrderLine>();

                var settings = await GetCustomerParseSettingsAsync(mail);

                var text = await ExtractTextAsync(mail);

                var multiLineRegex = new Regex(settings.MultiLineDrawingNumberAndDeliveryExpr, settings.MultiLineDrawingNumberAndDeliveryOptions);

                var multiLineMatches = multiLineRegex.Matches(text);

                if (multiLineMatches.Count == 0)
                {
                    var drawingRegex = new Regex(settings.DrawingNumberExpr, settings.DrawingNumberOptions);

                    var drawingNumber = drawingRegex.Match(text).Value;

                    var deliveryRegex = new Regex(settings.DeliveryDateExpr, settings.DeliveryDateOptions);

                    var dateString = deliveryRegex.Match(text).Value;

                    var rescheduleResult = await
                        _openOrderParserService.CheckIfHasBeenRescheduled(drawingNumber, orderNumber);

                    var line = new SalesOrderLine
                    {
                        DrawingNumber = await CleanDrawingNumberAsync(mail, drawingNumber),
                        Name = await _tricorn.GetNameByDrawingNumberAsync(drawingNumber),
                        OriginalDeliveryDate = DateTime.Parse(dateString),
                        Photo = await GetPhotoByDrawingNumber(drawingNumber)
                    };

                    if (rescheduleResult.HasBeenRescheduled)
                    {
                        line.RescheduledDeliveryDate = rescheduleResult.RescheduledDate.Value;
                    }

                    lines.Add(line);
                }
                else
                {
                    var regex = new Regex(settings.MultiLineDrawingNumberAndDeliveryExpr,
                        settings.MultiLineDrawingNumberAndDeliveryOptions);

                    foreach (Match match in regex.Matches(text))
                    {
                        var rescheduleResult = await
                            _openOrderParserService.CheckIfHasBeenRescheduled(match.Groups["drawing"].Value, orderNumber);

                        var line = new SalesOrderLine
                        {
                            DrawingNumber = await CleanDrawingNumberAsync(mail, match.Groups["drawing"].Value),
                            Name = await _tricorn.GetNameByDrawingNumberAsync(match.Groups["drawing"].Value),
                            OriginalDeliveryDate = DateTime.Parse(match.Groups["delivery"].Value),
                            Photo = await GetPhotoByDrawingNumber(match.Groups["drawing"].Value)
                        };

                        if (rescheduleResult.HasBeenRescheduled)
                        {
                            line.RescheduledDeliveryDate = rescheduleResult.RescheduledDate.Value;
                        }

                        lines.Add(line);
                    }

                    lines = lines.OrderBy(l => l.OriginalDeliveryDate).ToList();
                }

                var newOrder = new NewSalesOrder
                {
                    Buyer = buyer,
                    CustomerName = customer.Name,
                    OrderNumber = orderNumber,
                    EarliestDeliveryDate = DateTime.MaxValue,
                    Lines = lines.ToList(),
                    TotalValue = totalValue
                };

                foreach (var line in lines)
                {
                    if (line.OriginalDeliveryDate < newOrder.EarliestDeliveryDate)
                    {
                        newOrder.EarliestDeliveryDate = line.OriginalDeliveryDate;
                    }

                    if (line.RescheduledDeliveryDate.HasValue &&
                        line.RescheduledDeliveryDate.Value < newOrder.EarliestDeliveryDate)
                    {
                        newOrder.EarliestDeliveryDate = line.RescheduledDeliveryDate.Value;
                    }
                }

                newSalesOrders.Add(newOrder);
            }

            return newSalesOrders;
        }

        private async Task<ICustomer> GetCustomerAsync(MSOutlookMailItem mail)
        {
            if (_parseableCustomers == null)
            {
                _parseableCustomers = await _customers.GetSalesOrderParseableAsync();
            }

            var parsedText = await ExtractTextAsync(mail);

            foreach (var customer in _parseableCustomers)
            {
                var parseSettings = customer.GetSalesOrderParserSettings();

                var regex = new Regex(parseSettings.CustomerIdentifierExpr, parseSettings.CustomerIdentifierOptions);

                if (regex.IsMatch(parsedText))
                {
                    return customer;
                }
            }

            return null;
        }

        private async Task<string> GetBuyerAsync(MSOutlookMailItem mail)
        {
            var settings = await GetCustomerParseSettingsAsync(mail);

            var text = await ExtractTextAsync(mail);

            var regex = new Regex(settings.BuyerExpr, settings.BuyerOptions);

            return regex.Match(text).Value;
        }

        private async Task<string> GetOrderNumberAsync(MSOutlookMailItem mail)
        {
            var settings = await GetCustomerParseSettingsAsync(mail);

            var text = await ExtractTextAsync(mail);

            var regex = new Regex(settings.OrderNumberIdentifierExpr, settings.OrderNumberOptions);

            return regex.Match(text).Value;
        }

        private async Task<decimal> GetTotalValueAsync(MSOutlookMailItem mail)
        {
            var settings = await GetCustomerParseSettingsAsync(mail);

            var text = await ExtractTextAsync(mail);

            var regex = new Regex(settings.TotalValueExpr, settings.TotalValueOptions);

            var stringAmount = regex.Match(text).Value;

            return decimal.Parse(stringAmount);
        }

        private async Task<string> CleanDrawingNumberAsync(MSOutlookMailItem mail, string drawingNumber)
        {
            var settings = await GetCustomerParseSettingsAsync(mail);

            if (string.IsNullOrEmpty(settings.DrawingNumberReplacementExpr))
            {
                return drawingNumber;
            }

            var regex = new Regex(settings.DrawingNumberReplacementExpr, settings.DrawingNumberReplacementOptions);

            return regex.Replace(drawingNumber, settings.DrawingNumberReplacementValue);
        }

        private async Task<SalesOrderParserSettingsBlob> GetCustomerParseSettingsAsync(MSOutlookMailItem mail)
        {
            if (_memoryCache.Contains("PARSESETTINGS:" + mail.MailId))
            {
                return (SalesOrderParserSettingsBlob) _memoryCache["PARSESETTINGS:" + mail.MailId];
            }

            var customer = await GetCustomerAsync(mail);

            var settings = customer.GetSalesOrderParserSettings();

            _memoryCache.Add("PARSESETTINGS:" + mail.MailId, settings,
                new CacheItemPolicy { SlidingExpiration = new TimeSpan(0, 0, 5, 0) });

            return customer.GetSalesOrderParserSettings();
        }

        private async Task<string> ExtractTextAsync(MSOutlookMailItem mail)
        {
            if (_memoryCache.Contains(mail.MailId))
            {
                return (string) _memoryCache[mail.MailId];
            }

            var parsedText = await Task.Factory.StartNew(() =>
            {
                var ldoc = new PdfLoadedDocument(mail.Attachments.First());

                var loadedPages = ldoc.Pages;

                var parsedTextBuilder = new StringBuilder();

                foreach (PdfLoadedPage lpage in loadedPages)
                {
                    parsedTextBuilder.Append(lpage.ExtractText());
                }

                return parsedTextBuilder.ToString();
            });

            _memoryCache.Add(mail.MailId, parsedText,
                new CacheItemPolicy {SlidingExpiration = new TimeSpan(0, 0, 5, 0)});

            return parsedText;
        }

        private async Task<byte[]> GetPhotoByDrawingNumber(string drawingNumber)
        {
            if (_memoryCache.Contains(drawingNumber))
            {
                return (byte[]) _memoryCache[drawingNumber];
            }

            return await Task.Factory.StartNew(() =>
            {
                byte[] photoBytes = null;

                var part = _parts.GetWhereDrawingNumberEquals(drawingNumber);

                if (part != null)
                {
                    photoBytes = _photos.GetPhotoByPart(part);

                    if (photoBytes != null)
                    {
                        _memoryCache.Add(drawingNumber, photoBytes,
                            new CacheItemPolicy {SlidingExpiration = new TimeSpan(0, 0, 5, 0)});
                    }
                }

                return photoBytes;
            });
        }
    }
}