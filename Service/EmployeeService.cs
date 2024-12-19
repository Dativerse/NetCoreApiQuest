using System.Dynamic;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Service.DataShaping;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service
{
  public class EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IDataShaper<EmployeeDto> dataShaper) : IEmployeeService
  {
    public async Task<(IEnumerable<ExpandoObject> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {
      if (!employeeParameters.ValidAgeRange) throw new MaxAgeRangeBadRequestException();
      await CheckIfCompanyExists(companyId, trackChanges);

      var employeesWithMetaData = await repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges);

      var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);
      var shapedData = dataShaper.ShapeData(employeesDto, employeeParameters.Fields);

      return (employees: shapedData, metaData: employeesWithMetaData.MetaData);
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
      await CheckIfCompanyExists(companyId, trackChanges);

      var employeesFromDb = await repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);

      var employee = mapper.Map<EmployeeDto>(employeesFromDb);

      return employee;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
    {
      await CheckIfCompanyExists(companyId, trackChanges);
      var employeeEntity = mapper.Map<Employee>(employeeForCreation);

      repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
      await repository.SaveAsync();

      var employeeToReturn = mapper.Map<EmployeeDto>(employeeEntity);

      return employeeToReturn;
    }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
    {
      await CheckIfCompanyExists(companyId, trackChanges);

      var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);
      repository.Employee.DeleteEmployee(employeeDb);

      await repository.SaveAsync();
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
    {
      await CheckIfCompanyExists(companyId, compTrackChanges);
      var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);
      mapper.Map(employeeForUpdate, employeeDb);

      await repository.SaveAsync();
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
    {
      await CheckIfCompanyExists(companyId, compTrackChanges);
      var employeeEntity = await repository.Employee.GetEmployeeAsync(companyId, id, empTrackChanges) ?? throw new EmployeeNotFoundException(companyId);
      var employeeToPatch = mapper.Map<EmployeeForUpdateDto>(employeeEntity);

      return (employeeToPatch, employeeEntity);
    }

    public void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
      mapper.Map(employeeToPatch, employeeEntity);

      repository.SaveAsync();
    }

    private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
    {
      var company = await repository.Company.GetCompanyAsync(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
    }

    private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(Guid companyId, Guid id, bool trackChanges)
    {
      var employeeDb = await repository.Employee.GetEmployeeAsync(companyId, id, trackChanges) ?? throw new EmployeeNotFoundException(id);
      return employeeDb;
    }
  }
}