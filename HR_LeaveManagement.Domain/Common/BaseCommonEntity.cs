using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Domain.Common
{
    public abstract class BaseCommonEntity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
