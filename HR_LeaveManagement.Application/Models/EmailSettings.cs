using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.Models
{
    public class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string AUTH { get; set; }
        public bool UseSSL { get; set; }
    }
}
