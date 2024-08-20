using NLog;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;

namespace UzmanCrm.CrmService.Infrastructure.NLogConfigService
{

    public static class NLogConfigService
    {
        private static string GetMacAddress()
        {
            String sMacAddress = string.Empty;
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                        var list = Enumerable.Range(0, sMacAddress.Length / 2).Select(i => sMacAddress.Substring(i * 2, 2));
                        sMacAddress = string.Join("-", list);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return sMacAddress;
        }

        public static LogEventInfo Logger(LogLevel logLevel, string instanceId, string loggerName, string methodname, string organization, string type, string message)
        {
            LogEventInfo logEvent = new LogEventInfo(logLevel, loggerName, message);
            string ipAddress = HttpContext.Current == null ? "" : HttpContext.Current.Request.UserHostAddress.ToString();
            logEvent.Properties["Organization"] = organization;
            logEvent.Properties["Type"] = type;
            logEvent.Properties["MacAddress"] = NLogConfigService.GetMacAddress();
            logEvent.Properties["Methodname"] = methodname;
            logEvent.Properties["InstanceId"] = instanceId;
            logEvent.Properties["ipaddress"] = ipAddress;

            return logEvent;

        }
    }
}
