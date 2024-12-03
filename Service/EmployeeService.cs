﻿using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
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

  public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
  {
    var company = repository.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
    var employeeEntity = mapper.Map<Employee>(employeeForCreation);

    repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
    repository.Save();

    var employeeToReturn = mapper.Map<EmployeeDto>(employeeEntity);

    return employeeToReturn;
  }
}