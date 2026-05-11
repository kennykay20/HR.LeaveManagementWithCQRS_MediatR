using FluentValidation;
using HR_LeaveManagement.Application.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.DTOs.LeaveRequest.Validators
{
    public class CreateLeaveRequestDtoValidator : AbstractValidator<CreateLeaveRequestDto>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepo;

        public CreateLeaveRequestDtoValidator(ILeaveRequestRepository leaveRequestRepo)
        {
            _leaveRequestRepo = leaveRequestRepo;

            Include(new ILeaveRequestDtoValidator(_leaveRequestRepo));
            
        }
    }
}
