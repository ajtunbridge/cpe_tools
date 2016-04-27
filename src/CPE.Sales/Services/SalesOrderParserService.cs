using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CPE.Domain.Model;
using CPE.Domain.Services;
using CPE.Sales.Models;
using CPE.Sales.Properties;
using MSOutlookProvider;
using MSOutlookProvider.Model;

namespace CPE.Sales.Services
{
    public sealed class SalesOrderParserService
    {  
        private readonly IPdfParserService _pdfParser;
        private readonly ICustomerService _customers;
        private readonly IPartService _parts;
        private readonly IPhotoService _photos;
        private readonly ITricornService _tricorn;

        public SalesOrderParserService(IPdfParserService pdfParser, ICustomerService customers, ITricornService tricorn,
            IPhotoService photos, IPartService parts)
        {
            _pdfParser = pdfParser;
            _customers = customers;
            _tricorn = tricorn;
            _photos = photos;
            _parts = parts;
        }

        public async Task<List<SalesOrder>> GetSalesOrdersAsync()
        {
            var folderId = Settings.Default.SalesOrderFolderId;

            var allMail = await Task.Factory.StartNew(() => MailProvider.GetMail(folderId));

            var salesOrders = new List<SalesOrder>(allMail.Count);

            foreach (var mail in allMail)
            {
                var salesOrder = await GenerateSalesOrderFromMail(mail);

                salesOrders.Add(salesOrder);
            }

            return salesOrders;
        }

        private async Task<SalesOrder> GenerateSalesOrderFromMail(Mail mail)
        {
            var pdfFiles = mail.ExtractedAttachments.Where(
                    a => Path.GetExtension(a).Equals(".pdf", StringComparison.OrdinalIgnoreCase));

            if (!pdfFiles.Any() || pdfFiles.Count() > 1)
            {
                return new SalesOrder
                {
                    Buyer = "N/A",
                    CustomerName = "N/A",
                    EarliestDeliveryDate = DateTime.MinValue,
                    OrderNumber = "N/A",
                    Mail = mail,
                    TotalValue = 0
                };
                // TODO: handle situation where there is more than on pdf file in the email   
            }

            await _pdfParser.LoadPdfAsync(pdfFiles.First());

            var customer = await GetCustomerAsync();

            if (customer == null)
            {
                return new SalesOrder
                {
                    Buyer = "N/A",
                    CustomerName = "N/A",
                    EarliestDeliveryDate = DateTime.MinValue,
                    OrderNumber = "N/A",
                    Mail = mail,
                    TotalValue = 0
                };
                // TODO: handle non-parseable customer
            }

            var settings = customer.GetSalesOrderParserSettings();

            var orderNumberMatches = _pdfParser.Find(settings.OrderNumberIdentifierExpr, settings.OrderNumberOptions);
            var buyerMatches = _pdfParser.Find(settings.BuyerExpr, settings.BuyerOptions);
            var totalValueMatches = _pdfParser.Find(settings.TotalValueExpr, settings.TotalValueOptions);

            var order = new SalesOrder
            {
                OrderNumber = orderNumberMatches.Count == 0 ? "N/A" : orderNumberMatches[0].Value,
                CustomerName = customer.Name,
                Buyer = buyerMatches.Count == 0 ? "N/A" : buyerMatches[0].Value,
                TotalValue = totalValueMatches.Count == 0 ? 0 : decimal.Parse(totalValueMatches[0].Value),
                Mail = mail,
                EarliestDeliveryDate = DateTime.MaxValue
            };

            var isMultiLine = _pdfParser.CanFind(settings.MultiLineDrawingNumberAndDeliveryExpr,
                settings.MultiLineDrawingNumberAndDeliveryOptions);

            if (isMultiLine)
            {
                var lines = await GetMultipleSalesOrderLines(settings);
                
                lines.ForEach(line => order.Lines.Add(line));
            }
            else
            {
                SalesOrderLine line;

                if (string.IsNullOrEmpty(settings.MultiDropExpr))
                {
                    line = await GetSingleSalesOrderLine(settings);
                }
                else
                {
                    var multiDropDeliveries = _pdfParser.Find(settings.MultiDropExpr, settings.MultiDropOptions);

                    line = multiDropDeliveries.Count > 0
                        ? await GetSingleMultiDropSalesOrderLine(settings)
                        : await GetSingleSalesOrderLine(settings);
                }

                order.Lines.Add(line);
            }

            foreach (var line in order.Lines)
            {
                if (line.OriginalDeliveryDate < order.EarliestDeliveryDate)
                {
                    order.EarliestDeliveryDate = line.OriginalDeliveryDate;
                }   
            }

            if (order.EarliestDeliveryDate == DateTime.MaxValue)
            {
                order.EarliestDeliveryDate = DateTime.MinValue;
            }

            return order;
        }

        private async Task<List<SalesOrderLine>> GetMultipleSalesOrderLines(SalesOrderParserSettingsBlob settings)
        {
            var lines = new List<SalesOrderLine>();

            var lineMatches = _pdfParser.Find(settings.MultiLineDrawingNumberAndDeliveryExpr,
                settings.MultiLineDrawingNumberAndDeliveryOptions);

            foreach (Match match in lineMatches)
            {
                var drawingNumber = match.Groups["drawing"].Value;
                var delivery = match.Groups["delivery"].Value;
                
                var line = new SalesOrderLine();

                line.OriginalDeliveryDate = string.IsNullOrEmpty(delivery) ? DateTime.MinValue : DateTime.Parse(delivery);

                if (string.IsNullOrEmpty(settings.DrawingNumberReplacementExpr))
                {
                    line.DrawingNumber = string.IsNullOrEmpty(drawingNumber) ? "N/A" : drawingNumber;
                }
                else
                {
                    if (string.IsNullOrEmpty(drawingNumber))
                    {
                        line.DrawingNumber = "N/A";
                    }
                    else
                    {
                        line.DrawingNumber = RegexReplace(
                            drawingNumber,
                            settings.DrawingNumberReplacementExpr,
                            settings.DrawingNumberReplacementValue,
                            settings.DrawingNumberReplacementOptions);
                    }
                }

                line.Photo = await _photos.GetPhotoByDrawingNumber(line.DrawingNumber);
                line.Name = await _tricorn.GetNameByDrawingNumberAsync(line.DrawingNumber);

                lines.Add(line);
            }

            return lines;
        }

        private async Task<SalesOrderLine> GetSingleSalesOrderLine(SalesOrderParserSettingsBlob settings)
        {
            var line = new SalesOrderLine();

            var deliveryMatches = _pdfParser.Find(settings.DeliveryDateExpr, settings.DeliveryDateOptions);
            var drawingNumberMatches = _pdfParser.Find(settings.DrawingNumberExpr, settings.DrawingNumberOptions);

            if (string.IsNullOrEmpty(settings.DrawingNumberReplacementExpr))
            {
                line.DrawingNumber = drawingNumberMatches.Count == 0 ? "N/A" : drawingNumberMatches[0].Value;
            }
            else
            {
                if (drawingNumberMatches.Count == 0)
                {
                    line.DrawingNumber = "N/A";
                }
                else
                {
                    line.DrawingNumber = RegexReplace(
                        drawingNumberMatches[0].Value,
                        settings.DrawingNumberReplacementExpr,
                        settings.DrawingNumberReplacementValue,
                        settings.DrawingNumberReplacementOptions);
                }
            }

            line.Photo = await _photos.GetPhotoByDrawingNumber(line.DrawingNumber);
            line.Name = await _tricorn.GetNameByDrawingNumberAsync(line.DrawingNumber);
            line.OriginalDeliveryDate = deliveryMatches.Count == 0
                ? DateTime.MinValue
                : DateTime.Parse(deliveryMatches[0].Value);

            return line;
        }

        private async Task<SalesOrderLine> GetSingleMultiDropSalesOrderLine(SalesOrderParserSettingsBlob settings)
        {
            var line = new SalesOrderLine();
            
            var drawingNumberMatches = _pdfParser.Find(settings.DrawingNumberExpr, settings.DrawingNumberOptions);

            if (string.IsNullOrEmpty(settings.DrawingNumberReplacementExpr))
            {
                line.DrawingNumber = drawingNumberMatches.Count == 0 ? "N/A" : drawingNumberMatches[0].Value;
            }
            else
            {
                if (drawingNumberMatches.Count == 0)
                {
                    line.DrawingNumber = "N/A";
                }
                else
                {
                    line.DrawingNumber = RegexReplace(
                        drawingNumberMatches[0].Value,
                        settings.DrawingNumberReplacementExpr,
                        settings.DrawingNumberReplacementValue,
                        settings.DrawingNumberReplacementOptions);
                }
            }

            line.Photo = await _photos.GetPhotoByDrawingNumber(line.DrawingNumber);
            line.Name = await _tricorn.GetNameByDrawingNumberAsync(line.DrawingNumber);

            var deliveryMatches = _pdfParser.Find(settings.MultiDropExpr, settings.MultiDropOptions);

            line.OriginalDeliveryDate = DateTime.MaxValue;

            foreach (Match match in deliveryMatches)
            {
                var quantityString = match.Groups["quantity"].Value;
                var deliveryString = match.Groups["delivery"].Value;

                var batch = new BatchDelivery
                {
                    DeliveryDate = DateTime.Parse(deliveryString),
                    Quantity = int.Parse(quantityString)
                };

                line.BatchDeliveries.Add(batch);

                if (batch.DeliveryDate < line.OriginalDeliveryDate)
                {
                    line.OriginalDeliveryDate = batch.DeliveryDate;
                }
            }

            return line;
        }

        private string RegexReplace(string input, string expression, string replacementExpression, RegexOptions options)
        {
            var regex = new Regex(expression, options);

            return regex.Replace(input, replacementExpression);
        }

        private async Task<ICustomer> GetCustomerAsync()
        {
            foreach (var customer in await _customers.GetAllAsync())
            {
                if (!customer.HasSalesOrderParserSettings)
                {
                    continue;
                }

                var settings = customer.GetSalesOrderParserSettings();
                
                if (_pdfParser.CanFind(settings.CustomerIdentifierExpr, settings.CustomerIdentifierOptions))
                {
                    return customer;
                }
            }

            return null;
        }
    }
}
