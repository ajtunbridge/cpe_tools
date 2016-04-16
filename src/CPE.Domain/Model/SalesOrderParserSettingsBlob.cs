using System;
using System.Text.RegularExpressions;

namespace CPE.Domain.Model
{
    [Serializable]
    public class SalesOrderParserSettingsBlob
    {
        public string DrawingNumberExpr { get; set; }
        public RegexOptions DrawingNumberOptions { get; set; }

        public string DeliveryDateExpr { get; set; }
        public RegexOptions DeliveryDateOptions { get; set; }

        public string BuyerExpr { get; set; }
        public RegexOptions BuyerOptions { get; set; }

        public string CustomerIdentifierExpr { get; set; }
        public RegexOptions CustomerIdentifierOptions { get; set; }

        public string OrderNumberIdentifierExpr { get; set; }
        public RegexOptions OrderNumberOptions { get; set; }

        public string MultiLineDrawingNumberAndDeliveryExpr { get; set; }
        public RegexOptions MultiLineDrawingNumberAndDeliveryOptions { get; set; }
    }
}