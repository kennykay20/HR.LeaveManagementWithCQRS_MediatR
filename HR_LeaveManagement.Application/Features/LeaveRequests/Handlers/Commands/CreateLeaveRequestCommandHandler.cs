using AutoMapper;
using Hangfire;
using HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces;
using HR_LeaveManagement.Application.Contracts.Infrastructure.Templates;
using HR_LeaveManagement.Application.Contracts.Persistence;
using HR_LeaveManagement.Application.DTOs.LeaveRequest;
using HR_LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR_LeaveManagement.Application.Events;
using HR_LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR_LeaveManagement.Application.Models;
using HR_LeaveManagement.Application.Responses;
using HR_LeaveManagement.Domain.Entities;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, BaseCommandResponse<LeaveRequestDto>>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        private readonly ILeaveRequestRepository _leaveRequestRepo;
        private readonly IMapper _mapper;
        private readonly IEmailJobService _emailJobService;
        private readonly ILogger<CreateLeaveRequestCommandHandler> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        public CreateLeaveRequestCommandHandler(
            ILeaveTypeRepository leaveTypeRepo,
            IMapper mapper,
            IEmailJobService emailJobService,
            ILogger<CreateLeaveRequestCommandHandler> logger,
            IPublishEndpoint publishEndpoint,
            ILeaveRequestRepository leaveRequestRepo)
        {
            _leaveTypeRepo = leaveTypeRepo;
            _mapper = mapper;
            _emailJobService = emailJobService;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _leaveRequestRepo = leaveRequestRepo;
        }
        public async Task<BaseCommandResponse<LeaveRequestDto>> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($" inside the create leave request Ids - {request.LeaveRequestDto.LeaveTypeId}, email - {request.LeaveRequestDto.Email}, start date - {request.LeaveRequestDto.StartDate}");

            var response = new BaseCommandResponse<LeaveRequestDto>();
            var validator = new CreateLeaveRequestDtoValidator(_leaveTypeRepo);
            var validationResult = await validator.ValidateAsync(request.LeaveRequestDto);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Creation Failed.";
                response.Errors = validationResult.Errors.Select(er => er.ErrorMessage).ToList();
                response.Data = null;
                return response;
            }

            var leaveRequest = _mapper.Map<LeaveRequest>(request.LeaveRequestDto);
            leaveRequest.Approved = false;
            leaveRequest.DateRequested = DateTime.Now;
            leaveRequest = await _leaveRequestRepo.Add(leaveRequest);

            response.Success = true;
            response.Message = "Creation successful.";
            response.Id = leaveRequest.Id;

            //var email = new Email
            //{
            //    To = request.LeaveRequestDto.Email ?? "kennyoluwadamilare20@gmail.com",
            //    Subject = "Leave Request Submitted",
            //    Body = EmailTemplateGetter.LeaveRequestNotification(request.LeaveRequestDto.StartDate, request.LeaveRequestDto.EndDate)
            //};

            _logger.LogInformation($"request id - {leaveRequest.Id}, email - {leaveRequest.Email}, leaveTypeId - {request.LeaveRequestDto.LeaveTypeId}");
            // Message queue
            await _publishEndpoint.Publish(
                new LeaveRequestCreatedEvent(
                    leaveRequest.Id,
                    leaveRequest.Email,
                    request.LeaveRequestDto.LeaveTypeId,
                    leaveRequest.StartDate,
                    leaveRequest.EndDate,
                    leaveRequest.RequestComments
                ));

            //try
            //{
            //    Console.WriteLine($"Hangfire started sending email notification to {email.To}");
            //    _emailJobService.QueueLeaveRequestEmail(email);
            //}
            //catch(Exception ex)
            //{
            //    // Log or handler error
            //    Console.WriteLine("Error sending email notification " + ex.Message);
            //}
            return response;
        }
    }
}
