using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Domain.Entities
{
    public class AuditTrail
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string Action { get; set; } = default!;
        public string Method { get; set; } = default!;
        public string Path { get; set; } = default!;
        public int StatusCode { get; set; }
        public string? IpAddress { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
