using Web.Services.Jobs;

namespace Web.Features.Orders.CapturePayment
{
    public class CapturePaymentJob : Job
    {
        public CapturePaymentJob(string paymentIntentId)
        {
            PaymentIntentId = paymentIntentId;
        }

        public string PaymentIntentId { get; }
    }
}
