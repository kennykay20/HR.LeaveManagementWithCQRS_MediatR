using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Infrastructure.Helpers
{
    public class NigeriaTimeEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var nigeriaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
            var nigeriaTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, nigeriaTimeZone);
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("NigeriaTime", nigeriaTime.ToString("yyyy-MM-dd HH:mm:ss")));
        }
    }
}
