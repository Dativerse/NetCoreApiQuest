using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

public class EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : IEmployeeService
{
  public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
  {
    var company = repository.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
    var employeesFromDb = repository.Employee.GetEmployees(companyId,trackChanges);

    var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

    return employeesDto;
  }
  public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
  {
    var company = repository.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
    var employeeDb = repository.Employee.GetEmployee(companyId, id, trackChanges) ?? throw new EmployeeNotFoundException(id);

    var employee = mapper.Map<EmployeeDto>(employeeDb);

    return employee;
  }
}