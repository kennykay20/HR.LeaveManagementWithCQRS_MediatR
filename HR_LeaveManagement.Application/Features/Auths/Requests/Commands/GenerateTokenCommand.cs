using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.Auths.Requests.Commands
{
    public class GenerateTokenCommand : IRequest<string>
    {
        public int userId { get; set; }
    }
}
