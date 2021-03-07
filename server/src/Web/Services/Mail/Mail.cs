using System;

namespace Web.Services.Mail
{
    public class Mail
    {
        public Mail(string fromAddress, string toAddress)
        {
            FromAddress = fromAddress ?? throw new ArgumentNullException(nameof(fromAddress));
            ToAddress = toAddress ?? throw new ArgumentNullException(nameof(toAddress));
        }

        public string FromName { get; init; }
        public string FromAddress { get; }
        public string ToName { get; init; }
        public string ToAddress { get; }
        public string Subject { get; init; }
        public string Body { get; init; }
    }
}
