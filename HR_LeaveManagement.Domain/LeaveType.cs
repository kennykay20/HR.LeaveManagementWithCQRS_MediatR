using HR_LeaveManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Domain
{
    public class LeaveType : BaseCommonEntity
    {
        public string Name { get; set; }
        public int DefaultDays { get; set; }
    }
}
