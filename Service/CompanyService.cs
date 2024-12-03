using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

internal class CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : ICompanyService
{
  public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
  {
    var companies = repository.Company.GetAllCompanies(trackChanges);

    var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);

    return companiesDto;
  }

  public CompanyDto GetCompany(Guid id, bool trackChanges)
  {
    var company = repository.Company.GetCompany(id, trackChanges) ?? throw new CompanyNotFoundException(id);
    var companyDto = mapper.Map<CompanyDto>(company);

    return companyDto;
  }
}