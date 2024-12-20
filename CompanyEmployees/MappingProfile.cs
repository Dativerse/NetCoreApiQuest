using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace CompanyEmployees
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      CreateMap<Company, CompanyDto>().ForMember(c => c.FullAddress, opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
      CreateMap<Employee, EmployeeDto>();
      CreateMap<EmployeeForCreationDto, Employee>();
      CreateMap<CompanyForCreationDto, Company>();
      CreateMap<EmployeeForUpdateDto, Employee>();
      CreateMap<CompanyForUpdateDto, Company>();
      CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();

    }
  }
}
