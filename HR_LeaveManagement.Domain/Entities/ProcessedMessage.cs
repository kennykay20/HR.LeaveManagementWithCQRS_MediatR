using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Domain.Entities
{
    public class ProcessedMessage
    {
        public Guid Id { get; set; }
        public string MessageId { get; set; } = string.Empty;
        public DateTime ProcessedAt { get; set; }
    }
}
