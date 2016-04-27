using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CPE.Domain.Services;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;

namespace SyncfusionPdfParser
{
    public sealed class SfPdfParser : IPdfParserService
    {
        private readonly List<CachedText> _cache = new List<CachedText>();

        private string _extractedText;

        public async Task LoadPdfAsync(string fileName)
        {
            var cached = _cache.FirstOrDefault(c => c.FileName == fileName);

            if (cached != null)
            {
                _extractedText = cached.Text;
                return;
            }

            _extractedText = await Task.Factory.StartNew(() =>
            {
                var ldoc = new PdfLoadedDocument(fileName);

                var loadedPages = ldoc.Pages;

                var parsedTextBuilder = new StringBuilder();

                foreach (PdfLoadedPage lpage in loadedPages)
                {
                    parsedTextBuilder.Append(lpage.ExtractText());
                }
                return parsedTextBuilder.ToString();
            });

            _cache.Add(new CachedText {FileName = fileName, Text = _extractedText});
        }

        public bool CanFind(string expression, RegexOptions options)
        {
            var regex = new Regex(expression, options);

            return regex.IsMatch(_extractedText);
        }

        public MatchCollection Find(string expression, RegexOptions options)
        {
            if (expression == null)
            {
                return null;
            }

            var regex = new Regex(expression, options);

            return regex.Matches(_extractedText);
        }
    }
}