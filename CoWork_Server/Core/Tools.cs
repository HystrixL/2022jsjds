using System.Net;

namespace Co_Work.Core
{
    public static class Tools
    {
        public static string GetAddressIP()
        {
            var addressIp = string.Empty;
            foreach (var ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ipAddress.AddressFamily.ToString() != "InterNetwork") continue;
                addressIp = ipAddress.ToString();
                break;
            }

            return addressIp;
        }
    }
}