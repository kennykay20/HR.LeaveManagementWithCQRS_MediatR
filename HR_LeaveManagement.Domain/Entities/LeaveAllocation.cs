using HR_LeaveManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Domain.Entities
{
    public class LeaveAllocation : BaseCommonEntity
    {
        public int NumberOfDays { get; set; }
        public DateTime DateCreated { get; set; }
        public LeaveType LeaveType { get; set; }
        public int LeaveTypeId { get; set; }
        public int Period { get; set; }
    }
}
