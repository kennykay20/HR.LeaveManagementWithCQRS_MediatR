using HR_LeaveManagement.Application.DTOs.Audit;
using HR_LeaveManagement.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.AuditTrail.Requests.Queries
{
    public class GetAuditPageListRequest : IRequest<ApiListPageResponse<List<AuditDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
