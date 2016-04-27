using System;
using System.Collections.Generic;

namespace MSOutlookProvider.Model
{
    public sealed class Mail
    {
        public string Id { get; set; }

        public string Subject { get; set; }

        public string Sender { get; set; }

        public DateTime ReceivedAt { get; set; }

        public string Body { get; set; }

        public List<string> ExtractedAttachments { get; } = new List<string>();
    }
}