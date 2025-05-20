using System.Collections.Generic;
using Newtonsoft.Json;

namespace _0_Framework.Application.ZarinPal
{
    public class VerificationResponse
    {
        public VerificationData Data { get; set; }
        public List<ZarinPalError> Errors { get; set; }
        public int Status => Data?.Code ?? 0;
        public long RefID => Data?.RefId ?? 0;
    }
    public class VerificationData
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("ref_id")]
        public long RefId { get; set; }
        [JsonProperty("fee")]
        public int Fee { get; set; }
    }
   
}
