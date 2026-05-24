using HR_LeaveManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Domain.Entities
{
    public class Role : BaseCommonEntity
    {
        public string Name { get; set; }
    }
}
