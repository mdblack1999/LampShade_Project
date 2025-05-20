using System.Collections.Generic;
using Newtonsoft.Json;

namespace _0_Framework.Application.ZarinPal
{
    public class ZarinPalPaymentResponse
    {
        public ZarinPalPaymentData Data { get; set; }
        public List<ZarinPalError> Errors { get; set; } 
    }

    public class ZarinPalPaymentData
    {
        [JsonProperty("authority")]
        public string Authority { get; set; }
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("fee")]
        public int Fee { get; set; }
        [JsonProperty("fee_type")]
        public string FeeType { get; set; }
    }

    public class ZarinPalError
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }


}
