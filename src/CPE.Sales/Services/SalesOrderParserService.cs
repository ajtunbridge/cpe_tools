using System;
using System.Collections.Generic;
using System.Linq;
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
        private IEnumerable<ICustomer> _parseableCustomers;
        private readonly HashSet<ParsedMail> _parsedMailCache = new HashSet<ParsedMail>();
          
        public SalesOrderParserService(ICustomerService customerService)
        {
            _customers = customerService;
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

        public async Task<string> ParseBuyerAsync(MSOutlookMailItem mail)
        {
            var customer = await ParseCustomerAsync(mail);

            var settings = customer.GetSalesOrderParserSettings();

            var regex = new Regex(settings.BuyerExpr, settings.BuyerOptions);

            var text = await ExtractTextAsync(mail);

            return regex.Match(text).Value;
        }

        public async Task<string> ParseOrderNumberAsync(MSOutlookMailItem mail)
        {
            var customer = await ParseCustomerAsync(mail);

            var settings = customer.GetSalesOrderParserSettings();

            var regex = new Regex(settings.OrderNumberIdentifierExpr, settings.OrderNumberOptions);

            var text = await ExtractTextAsync(mail);

            return regex.Match(text).Value;
        }

        public async Task<DateTime> ParseEarliestDelivery(MSOutlookMailItem mail)
        {
            throw new NotImplementedException();
        }

        private async Task<string> ExtractTextAsync(MSOutlookMailItem mail)
        {
            var cached = _parsedMailCache.SingleOrDefault(m => m.Mail.MailId == mail.MailId);

            if (cached != null)
            {
                return cached.ParsedText;
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
            
            _parsedMailCache.Add(new ParsedMail {Mail = mail, ParsedText = parsedText});

            return parsedText;
        }

        private class ParsedMail
        {
            public MSOutlookMailItem Mail { get; set; }

            public string ParsedText { get; set; }
        }
    }
}
