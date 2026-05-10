using AutoMapper;
using HR_LeaveManagement.Application.DTOs;
using HR_LeaveManagement.Application.DTOs.LeaveRequest;
using HR_LeaveManagement.Domain;
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
        }
    }
}
