using System;

namespace _0_Framework.Application.Sms
{
    public class RecentSmsDto
    {
        public string RecipientNumber { get; set; }  
        public string RecipientName { get; set; }  
        public string MessageText { get; set; }   
        public bool? DeliveryStatus { get; set; }  
        public string SentAt { get; set; } 
        public string DeliveryAt { get; set; }

    }
}
