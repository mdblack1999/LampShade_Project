using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using IPE.SmsIrClient;

namespace _0_Framework.Application.Sms
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;

        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiKey = _configuration.GetSection("SmsSecrets")["ApiKey"];
        }

        public void Send(string number , string message)
        {
            if (string.IsNullOrWhiteSpace(_apiKey)) return;

            var smsIr = new SmsIr(_apiKey);

            long[] lines;
            try
            {
                lines = smsIr.GetLinesAsync().GetAwaiter().GetResult().Data;
            }
            catch
            {
                return;
            }

            if (lines == null || lines.Length == 0) return;

            SmsResponse TrySend(long lineNumber)
            {
                try
                {
                    var sendResult = smsIr
                        .BulkSendAsync(lineNumber , message , new[] { number } , null)
                        .GetAwaiter()
                        .GetResult();

                    return sendResult.Status == 1
                        ? new SmsResponse { IsSuccessful = true , Message = string.Empty }
                        : new SmsResponse { IsSuccessful = false , Message = sendResult.Message };
                }
                catch (Exception ex)
                {
                    return new SmsResponse { IsSuccessful = false , Message = ex.Message };
                }
            }

            var primaryLine = lines.LastOrDefault();
            if (primaryLine == 0) return;

            var primaryResponse = TrySend(primaryLine);
            if (primaryResponse.IsSuccessful) return;

            var fallbackLine = lines.FirstOrDefault();
            if (fallbackLine == 0) return;

            var fallbackResponse = TrySend(fallbackLine);
            if (!fallbackResponse.IsSuccessful)
            {
                Console.WriteLine(fallbackResponse.Message);
            }
        }

        public List<RecentSmsDto> GetRecentSmsSent()
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                return new List<RecentSmsDto>();

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("x-api-key" , _apiKey);

            var url = "https://api.sms.ir/v1/send/live?pageNumber=1&pageSize=10";
            HttpResponseMessage response;
            try
            {
                response = httpClient.GetAsync(url).GetAwaiter().GetResult();
            }
            catch
            {
                return new List<RecentSmsDto>();
            }

            if (!response.IsSuccessStatusCode)
                return new List<RecentSmsDto>();

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var liveDto = JsonSerializer.Deserialize<LiveMessagesDto>(json , new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (liveDto == null || liveDto.Data == null)
                return new List<RecentSmsDto>();

            var list = new List<RecentSmsDto>();
            foreach (var item in liveDto.Data)
            {
                list.Add(new RecentSmsDto
                {
                    RecipientNumber = item.Mobile.ToString() ,
                    RecipientName = string.Empty ,
                    MessageText = item.MessageText ,
                    DeliveryStatus = item.DeliveryState == 1 ,
                    SentAt = DateTimeOffset.FromUnixTimeSeconds(item.SendDateTime).DateTime.ToFarsiFull(),
                    DeliveryAt = item.DeliveryDateTime.HasValue
                    ? DateTimeOffset.FromUnixTimeSeconds(item.DeliveryDateTime.Value).DateTime.ToFarsiFull()
                    : "--"
                });
            }
            //foreach (var sms in list)
            //{
            //    sms.RecipientName = _accountApplication.GetAccountBy(sms.RecipientNumber);
            //}

            return list;
        }

        public class SmsResponse
        {
            public bool IsSuccessful { get; init; }
            public string Message { get; init; }
        }

        public class LiveMessagesDto
        {
            [JsonPropertyName("status")]
            public int Status { get; set; }

            [JsonPropertyName("message")]
            public string Message { get; set; }

            [JsonPropertyName("data")]
            public LiveMessageItem[] Data { get; set; }
        }

        public class LiveMessageItem
        {
            [JsonPropertyName("messageId")]
            public long MessageId { get; set; }

            [JsonPropertyName("mobile")]
            public long Mobile { get; set; }

            [JsonPropertyName("messageText")]
            public string MessageText { get; set; }

            [JsonPropertyName("sendDateTime")]
            public long SendDateTime { get; set; }

            [JsonPropertyName("lineNumber")]
            public long LineNumber { get; set; }

            [JsonPropertyName("cost")]
            public double Cost { get; set; }

            [JsonPropertyName("deliveryState")]
            public int DeliveryState { get; set; }

            [JsonPropertyName("deliveryDateTime")]
            public long? DeliveryDateTime { get; set; }
        }
    }
}
