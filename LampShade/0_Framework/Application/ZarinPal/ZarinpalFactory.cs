using Microsoft.Extensions.Configuration;
using RestSharp;
using System;

namespace _0_Framework.Application.ZarinPal
{
    public class ZarinPalFactory : IZarinPalFactory
    {
        private readonly IConfiguration _configuration;
        private readonly string _merchantId;
        private readonly string _baseUrl;

        public ZarinPalFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            var paymentSection = _configuration.GetSection("Payment");
            var prefix = paymentSection["method"];      
            _merchantId = paymentSection["merchant"];
            _baseUrl = $"https://{prefix}.zarinpal.com/pg/v4/payment";
        }

        public ZarinPalPaymentResponse CreatePaymentRequest(string amount , string mobile , string email , string description , long orderId)
        {
            if (!long.TryParse(amount.Replace("," , "") , out var finalAmount))
                throw new ArgumentException("Amount is not in correct format." , nameof(amount));

            var siteUrl = _configuration.GetSection("payment")["siteUrl"];

            var client = new RestClient(_baseUrl + "/request.json");
            var request = new RestRequest(Method.POST);

            request.AddHeader("Content-Type" , "application/json");
            request.AddHeader("Accept" , "application/json");

            request.AddJsonBody(new
            {
                merchant_id = _merchantId ,
                amount = finalAmount ,
                callback_url = $"{siteUrl}/Checkout?handler=CallBack&oId={orderId}" ,
                description ,
                email,
                mobile
            });

            var response = client.Execute<ZarinPalPaymentResponse>(request);
            if (!response.IsSuccessful || response.Data == null)
            {
                throw new ApplicationException(
                    $"خطا در ایجاد درخواست پرداخت: {response.StatusDescription}\nContent: {response.Content}"
                );
            }

            return response.Data;
        }

        public VerificationResponse CreateVerificationRequest(string authority , string amount)
        {
            if (!long.TryParse(amount.Replace("," , "") , out var finalAmount))
                throw new ArgumentException("فرمت مبلغ نامعتبر است" , nameof(amount));

            var client = new RestClient(_baseUrl + "/verify.json");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type" , "application/json");
            request.AddHeader("Accept" , "application/json");

            request.AddJsonBody(new
            {
                merchant_id = _merchantId ,
                authority ,
                amount = finalAmount
            });

            var response = client.Execute<VerificationResponse>(request);

            if (!response.IsSuccessful || response.Data == null)
                throw new ApplicationException(
                    $"خطا در بررسی تراکنش: {response.ErrorMessage ?? response.StatusDescription}");

            return response.Data;
        }

        public string GetStartPayUrl(string authority)
        {
            var host = new Uri(_baseUrl).GetLeftPart(UriPartial.Authority); // Example: "https://sandbox.zarinpal.com"
            return $"{host}/pg/StartPay/{authority}";
        }
    }
}
