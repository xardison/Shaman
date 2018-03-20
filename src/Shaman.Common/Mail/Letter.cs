using System;
using System.Collections.Generic;
using System.Net.Mail;
using Shaman.Common.Extension;

namespace Shaman.Common.Mail
{
    public abstract class Letter : IDisposable
    {
        protected Letter()
        {
            To = new List<Receiver>();
            Attachments = new List<Attachment>();
        }

        public Receiver From { get; set; }
        public IList<Receiver> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IList<Attachment> Attachments { get; set; }

        public void Dispose()
        {
            To.Clear();
            Attachments.DoForEach(attachment => attachment.Dispose());
            Attachments.Clear();
        }
    }
}
