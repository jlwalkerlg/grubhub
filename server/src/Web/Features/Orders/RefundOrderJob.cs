using System;

namespace Web.Features.Orders
{
    public class RefundOrderJob : Job
    {
        public RefundOrderJob(string paymentIntentId)
        {
            PaymentIntentId = paymentIntentId ?? throw new ArgumentNullException(nameof(paymentIntentId));
        }

        public string PaymentIntentId { get; }
    }
}
