using System.Collections.Generic;

namespace _0_Framework.Application.Sms
{
    public interface ISmsService
    {
        void Send(string number, string message);
        List<RecentSmsDto> GetRecentSmsSent();
    }
}