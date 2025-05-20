namespace _0_Framework.Application.ZarinPal
{
    public class PaymentResult
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public string IssueTrackingNo { get; set; }

        public PaymentResult Succeeded(string message , string issueTrackingNumber)
        {
            IsSuccessful = true;
            Message = message;
            IssueTrackingNo = issueTrackingNumber;
            return this;
        }

        public PaymentResult Failed(string message)
        {
            IsSuccessful = false;
            Message = message;
            return this;
        }
    }
}