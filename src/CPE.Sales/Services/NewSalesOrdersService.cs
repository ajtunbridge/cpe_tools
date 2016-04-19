﻿using System;
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
        private readonly IPartService _parts;
        private readonly IPhotoService _photos;

        private readonly MemoryCache _mailCache = new MemoryCache("ParsedMailCache");
        private readonly OpenOrderReportParserService _openOrderParserService;
        private IEnumerable<ICustomer> _parseableCustomers;

        public NewSalesOrdersService(ICustomerService customerService, IPhotoService photoService, IPartService partService,
            OpenOrderReportParserService openOrderParserService)
        {
            _customers = customerService;
            _parts = partService;
            _photos = photoService;

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

                var lines = new List<SalesOrderLine>();

                var settings = await GetCustomerParseSettingsAsync(mail);

                var text = await ExtractTextAsync(mail);

                var drawingNumberRegex = new Regex(settings.DrawingNumberExpr, settings.DrawingNumberOptions);

                var drawingNumberMatches = drawingNumberRegex.Matches(text);

                if (drawingNumberMatches.Count == 1)
                {
                    var regex = new Regex(settings.DeliveryDateExpr, settings.DeliveryDateOptions);

                    var dateString = regex.Match(text).Value;

                    var rescheduleResult = await
                        _openOrderParserService.CheckIfHasBeenRescheduled(drawingNumberMatches[0].Value, orderNumber);

                    var line = new SalesOrderLine
                    {
                        DrawingNumber = await CleanupDrawingNumberAsync(mail, drawingNumberMatches[0].Value),
                        DeliveryDate = rescheduleResult.HasBeenRescheduled ? rescheduleResult.RescheduledDate.Value : DateTime.Parse(dateString),
                        Photo = await GetPhotoByDrawingNumber(drawingNumberMatches[0].Value)
                    };

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
                            DrawingNumber = await CleanupDrawingNumberAsync(mail,match.Groups["drawing"].Value),
                            DeliveryDate = rescheduleResult.HasBeenRescheduled ? rescheduleResult.RescheduledDate.Value : DateTime.Parse(match.Groups["delivery"].Value),
                            Photo = await GetPhotoByDrawingNumber(match.Groups["drawing"].Value)
                        };

                        lines.Add(line);
                    }

                    lines = lines.OrderBy(l => l.DeliveryDate).ToList();
                }
                
                var newOrder = new NewSalesOrder
                {
                    Buyer = buyer,
                    CustomerName = customer.Name,
                    OrderNumber = orderNumber,
                    Lines = lines.ToList(),
                    EarliestDeliveryDate = lines.OrderBy(l => l.DeliveryDate).Select(l => l.DeliveryDate).First()
                };

                newSalesOrders.Add(newOrder);
            }

            return newSalesOrders;
        }

        public async Task<ICustomer> GetCustomerAsync(MSOutlookMailItem mail)
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

        public async Task<bool> IsMultiLineOrder(MSOutlookMailItem mail)
        {
            var settings = await GetCustomerParseSettingsAsync(mail);

            var text = await ExtractTextAsync(mail);

            var regex = new Regex(settings.DrawingNumberExpr, settings.DrawingNumberOptions);

            var matches = regex.Matches(text);

            return matches.Count > 1;
        }

        public async Task<string> GetBuyerAsync(MSOutlookMailItem mail)
        {
            var settings = await GetCustomerParseSettingsAsync(mail);

            var text = await ExtractTextAsync(mail);

            var regex = new Regex(settings.BuyerExpr, settings.BuyerOptions);

            return regex.Match(text).Value;
        }

        public async Task<string> GetOrderNumberAsync(MSOutlookMailItem mail)
        {
            var settings = await GetCustomerParseSettingsAsync(mail);

            var text = await ExtractTextAsync(mail);

            var regex = new Regex(settings.OrderNumberIdentifierExpr, settings.OrderNumberOptions);

            return regex.Match(text).Value;
        }

        public async Task<string> CleanupDrawingNumberAsync(MSOutlookMailItem mail, string drawingNumber)
        {
            var settings = await GetCustomerParseSettingsAsync(mail);

            if (string.IsNullOrEmpty(settings.DrawingNumberReplacementExpr))
            {
                return drawingNumber;
            }

            var regex = new Regex(settings.DrawingNumberReplacementExpr, settings.DrawingNumberReplacementOptions);

            return regex.Replace(drawingNumber, settings.DrawingNumberReplacementValue);
        }

        public async Task<string> GetSingleLineDrawingNumberAsync(MSOutlookMailItem mail)
        {
            var settings = await GetCustomerParseSettingsAsync(mail);

            var text = await ExtractTextAsync(mail);

            var regex = new Regex(settings.DrawingNumberExpr, settings.DrawingNumberOptions);

            var result = regex.Match(text).Value;

            if (string.IsNullOrEmpty(settings.DrawingNumberReplacementExpr))
            {
                return result;
            }

            regex = new Regex(settings.DrawingNumberReplacementExpr, settings.DrawingNumberReplacementOptions);

            return regex.Replace(result, settings.DrawingNumberReplacementValue);
        }

        public async Task<DateTime> GetSingleLineDeliveryDateAsync(MSOutlookMailItem mail)
        {
            var settings = await GetCustomerParseSettingsAsync(mail);

            var text = await ExtractTextAsync(mail);

            var regex = new Regex(settings.DeliveryDateExpr, settings.DeliveryDateOptions);

            var dateString = regex.Match(text).Value;

            return DateTime.Parse(dateString);
        }

        public async Task<List<SalesOrderLine>> GetSalesOrderLinesAsync(MSOutlookMailItem mail)
        {
            var lines = new List<SalesOrderLine>();

            var settings = await GetCustomerParseSettingsAsync(mail);

            var text = await ExtractTextAsync(mail);

            var drwaingNumberRegex = new Regex(settings.DrawingNumberExpr, settings.DrawingNumberOptions);

            var drawingNumberMatches = drwaingNumberRegex.Matches(text);

            if (drawingNumberMatches.Count == 1)
            {
                var regex = new Regex(settings.DeliveryDateExpr, settings.DeliveryDateOptions);

                var dateString = regex.Match(text).Value;

                var photoBytes = await GetPhotoByDrawingNumber(drawingNumberMatches[0].Value);

                var line = new SalesOrderLine
                {
                    DrawingNumber = drawingNumberMatches[0].Value,
                    DeliveryDate = DateTime.Parse(dateString),
                    Photo = photoBytes
                };

                lines.Add(line);
            }
            else
            {
                var regex = new Regex(settings.MultiLineDrawingNumberAndDeliveryExpr,
                    settings.MultiLineDrawingNumberAndDeliveryOptions);

                foreach (Match match in regex.Matches(text))
                {
                    var photoBytes = await GetPhotoByDrawingNumber(match.Groups["drawing"].Value);

                    var line = new SalesOrderLine
                    {
                        DrawingNumber = match.Groups["drawing"].Value,
                        DeliveryDate = DateTime.Parse(match.Groups["delivery"].Value),
                        Photo = photoBytes
                    };

                    lines.Add(line);
                }
            }

            return lines;
        }
        
        private async Task<SalesOrderParserSettingsBlob> GetCustomerParseSettingsAsync(MSOutlookMailItem mail)
        {
            var customer = await GetCustomerAsync(mail);

            return customer.GetSalesOrderParserSettings();
        }

        private async Task<string> ExtractTextAsync(MSOutlookMailItem mail)
        {
            if (_mailCache.Contains(mail.MailId))
            {
                return (string) _mailCache[mail.MailId];
            }

            var parsedText = await Task.Factory.StartNew(() =>
            {
                PdfLoadedDocument ldoc = new PdfLoadedDocument(mail.Attachments.First());

                PdfLoadedPageCollection loadedPages = ldoc.Pages;

                StringBuilder parsedTextBuilder = new StringBuilder();

                foreach (PdfLoadedPage lpage in loadedPages)
                {
                    parsedTextBuilder.Append(lpage.ExtractText());
                }

                return parsedTextBuilder.ToString();
            });

            _mailCache.Add(mail.MailId, parsedText, new CacheItemPolicy {SlidingExpiration = new TimeSpan(0, 0, 30, 0)});

            return parsedText;
        }

        private async Task<byte[]> GetPhotoByDrawingNumber(string drawingNumber)
        {
            return await Task.Factory.StartNew(() =>
            {
                byte[] photoBytes = null;

                var part = _parts.GetWhereDrawingNumberEquals(drawingNumber);

                if (part != null)
                {
                    photoBytes = _photos.GetPhotoByPart(part);
                }

                return photoBytes;
            });
        }
        private class ParsedMail
        {
            public MSOutlookMailItem Mail { get; set; }
            public string ParsedText { get; set; }
        }
    }
}