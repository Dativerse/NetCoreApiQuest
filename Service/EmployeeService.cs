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
        var employeesFromDb = repository.Employee.GetEmployees(companyId, trackChanges);

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

    public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
    {
        var company = repository.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employeeForCompany = repository.Employee.GetEmployee(companyId, id, trackChanges) ?? throw new EmployeeNotFoundException(id);

        repository.Employee.DeleteEmployee(employeeForCompany);

        repository.Save();
    }

    public void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
    {
        var company = repository.Company.GetCompany(companyId, compTrackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employeeEntity = repository.Employee.GetEmployee(companyId, id, empTrackChanges) ?? throw new EmployeeNotFoundException(id);

        mapper.Map(employeeForUpdate, employeeEntity);

        repository.Save();
    }

    public (EmployeeForUpdateDto employeeToPatch, Employee employeeEntity) GetEmployeeForPatch(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
    {
        var company = repository.Company.GetCompany(companyId, compTrackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employeeEntity = repository.Employee.GetEmployee(companyId, id, empTrackChanges) ?? throw new EmployeeNotFoundException(companyId);
        var employeeToPatch = mapper.Map<EmployeeForUpdateDto>(employeeEntity);

        return (employeeToPatch, employeeEntity);
    }

    public void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
        mapper.Map(employeeToPatch, employeeEntity);

        repository.Save();
    }
}