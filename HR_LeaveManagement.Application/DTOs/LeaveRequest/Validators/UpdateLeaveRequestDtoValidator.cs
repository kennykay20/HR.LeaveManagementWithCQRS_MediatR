using FluentValidation;
using HR_LeaveManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.DTOs.LeaveRequest.Validators
{
    public class UpdateLeaveRequestDtoValidator : AbstractValidator<UpdateLeaveRequestDto>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepo;
        private readonly ILeaveTypeRepository _leaveTypeRepo;

        public UpdateLeaveRequestDtoValidator(
            ILeaveRequestRepository leaveRequestRepo, 
            ILeaveTypeRepository leaveTypeRepo)
        {
            _leaveRequestRepo = leaveRequestRepo;
            _leaveTypeRepo = leaveTypeRepo;

            Include(new ILeaveRequestDtoValidator(_leaveTypeRepo));

            RuleFor(data => data.Id)
                .NotNull()
                .WithMessage("{PropertyName} must be present.");

            RuleFor(data => data.Id)
                .GreaterThan(0)
                .MustAsync(async (id, token) =>
                {
                    Console.WriteLine("id------------------");
                    Console.WriteLine(id);
                    var leaveRequestExist = await _leaveRequestRepo.Exist(id);
                    return leaveRequestExist;
                })
                .WithMessage("{PropertyName} does not exist.");

            RuleFor(data => data.Cancelled)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.");

            
            
        }
    }
}
