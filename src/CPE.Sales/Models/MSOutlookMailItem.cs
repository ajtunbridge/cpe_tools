using System;
using System.Collections.Generic;

namespace CPE.Sales.Models
{
    public sealed class MSOutlookMailItem
    {
        public string MailId { get; set; }
        public string Subject { get; set; }
        public string Sender { get; set; }
        public DateTime ReceivedAt { get; set; }
        public string Body { get; set; }
        public List<string> Attachments { get; set; } = new List<string>();
    }
}