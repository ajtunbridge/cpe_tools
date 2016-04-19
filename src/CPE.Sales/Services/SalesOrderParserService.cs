using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CPE.Domain.Model;
using CPE.Domain.Services;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;

namespace CPE.Sales.Services
{
    public class SalesOrderParserService
    {
        private readonly ICustomerService _customers;
        private readonly OpenOrderReportParserService _openOrderParserService;
        private IEnumerable<ICustomer> _parseableCustomers;
        private readonly MemoryCache _mailCache = new MemoryCache("ParsedMailCache");

        public SalesOrderParserService(ICustomerService customerService, OpenOrderReportParserService openOrderParserService)
        {
            _customers = customerService;
            _openOrderParserService = openOrderParserService;
        }

        public async Task<ICustomer> ParseCustomerAsync(MSOutlookMailItem mail)
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
            var settings = await ParseCustomerParseSettingsAsync(mail);

            var text = await ExtractTextAsync(mail);

            var regex = new Regex(settings.DrawingNumberExpr, settings.DrawingNumberOptions);

            var matches = regex.Matches(text);

            return matches.Count > 1;
        }

        public async Task<string> ParseBuyerAsync(MSOutlookMailItem mail)
        {
            var settings = await ParseCustomerParseSettingsAsync(mail);

            var text = await ExtractTextAsync(mail);

            var regex = new Regex(settings.BuyerExpr, settings.BuyerOptions);

            return regex.Match(text).Value;
        }

        public async Task<string> ParseOrderNumberAsync(MSOutlookMailItem mail)
        {
            var settings = await ParseCustomerParseSettingsAsync(mail);

            var text = await ExtractTextAsync(mail);

            var regex = new Regex(settings.OrderNumberIdentifierExpr, settings.OrderNumberOptions);

            return regex.Match(text).Value;
        }

        public async Task<string> ParseSingleLineDrawingNumberAsync(MSOutlookMailItem mail)
        {
            var settings = await ParseCustomerParseSettingsAsync(mail);

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

        public async Task<DateTime> ParseSingleLineDeliveryDateAsync(MSOutlookMailItem mail)
        {
            var settings = await ParseCustomerParseSettingsAsync(mail);

            var text = await ExtractTextAsync(mail);

            var regex = new Regex(settings.DeliveryDateExpr, settings.DeliveryDateOptions);

            var dateString = regex.Match(text).Value;

            return DateTime.Parse(dateString);
        }

        public async Task<List<SalesOrderLine>> GetSalesOrderLinesAsync(MSOutlookMailItem mail)
        {
            var lines = new List<SalesOrderLine>();

            var settings = await ParseCustomerParseSettingsAsync(mail);

            var text = await ExtractTextAsync(mail);

            var regex = new Regex(settings.MultiLineDrawingNumberAndDeliveryExpr, settings.MultiLineDrawingNumberAndDeliveryOptions);

            foreach (Match match in regex.Matches(text))
            {
                var line = new SalesOrderLine
                {
                    DrawingNumber = match.Groups["drawing"].Value,
                    DeliveryDate = DateTime.Parse(match.Groups["delivery"].Value)
                };

                lines.Add(line);
            }

            return lines;
        }

        public async Task<DateTime> ParseEarliestDeliveryAsync(MSOutlookMailItem mail)
        {
            DateTime earliestDeliveryDate = DateTime.MaxValue;

            var orderNumber = await ParseOrderNumberAsync(mail);

            if (await IsMultiLineOrder(mail))
            {
                var lines = await GetSalesOrderLinesAsync(mail);

                foreach (var line in lines)
                {
                    var rescheduleResult =
                        await _openOrderParserService.CheckIfHasBeenRescheduled(line.DrawingNumber, orderNumber);

                    if (rescheduleResult.HasBeenRescheduled)
                    {
                        var currentEarliest = rescheduleResult.RescheduledDate < line.DeliveryDate
                            ? rescheduleResult.RescheduledDate
                            : line.DeliveryDate;

                        if (currentEarliest < earliestDeliveryDate)
                        {
                            earliestDeliveryDate = currentEarliest.Value;
                        }
                    }
                    else
                    {
                        if (line.DeliveryDate < earliestDeliveryDate)
                        {
                            earliestDeliveryDate = line.DeliveryDate;
                        }
                    }
                }
            }
            else
            {
                var drawingNumber = await ParseSingleLineDrawingNumberAsync(mail);
                var originalDeliveryDate = await ParseSingleLineDeliveryDateAsync(mail);

                var rescheduleResult =
                    await _openOrderParserService.CheckIfHasBeenRescheduled(drawingNumber, orderNumber);

                if (rescheduleResult.HasBeenRescheduled)
                {
                    earliestDeliveryDate = rescheduleResult.RescheduledDate < originalDeliveryDate
                        ? rescheduleResult.RescheduledDate.Value
                        : originalDeliveryDate;
                }
                else
                {
                    earliestDeliveryDate = originalDeliveryDate;
                }
            }

            return earliestDeliveryDate;
        }

        private async Task<SalesOrderParserSettingsBlob> ParseCustomerParseSettingsAsync(MSOutlookMailItem mail)
        {
            var customer = await ParseCustomerAsync(mail);

            return customer.GetSalesOrderParserSettings();
        }

        private async Task<string> ExtractTextAsync(MSOutlookMailItem mail)
        {
            if (_mailCache.Contains(mail.MailId))
            {
                return (string)_mailCache[mail.MailId];
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

        private class ParsedMail
        {
            public MSOutlookMailItem Mail { get; set; }

            public string ParsedText { get; set; }
        }
    }
}
