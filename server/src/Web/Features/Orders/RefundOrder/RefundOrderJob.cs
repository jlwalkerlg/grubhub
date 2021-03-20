using System;
using Web.Services.Jobs;

namespace Web.Features.Orders.RefundOrder
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
