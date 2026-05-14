using AutoMapper;
using HR_LeaveManagement.Application.DTOs.LeaveAllocation;
using HR_LeaveManagement.Application.DTOs.LeaveRequest;
using HR_LeaveManagement.Application.DTOs.LeaveType;
using HR_LeaveManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LeaveRequest, LeaveRequestDto>().ReverseMap();
            CreateMap<LeaveRequest, LeaveRequestListDto>().ReverseMap();
            CreateMap<LeaveType, LeaveTypeDto>().ReverseMap();
            CreateMap<LeaveAllocation, LeaveAllocationDto>().ReverseMap();
            CreateMap<CreateLeaveTypeDto, LeaveType>().ReverseMap();
            CreateMap<CreateLeaveRequestDto, LeaveRequest>().ReverseMap();
            CreateMap<CreateLeaveAllocationDto, LeaveAllocation>().ReverseMap();
        }
    }
}
