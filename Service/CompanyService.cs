using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
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

  public CompanyDto CreateCompany(CompanyForCreationDto company)
  {
    var companyEntity = mapper.Map<Company>(company);
    repository.Company.CreateCompany(companyEntity);
    repository.Save();

    var companyToReturn = mapper.Map<CompanyDto>(companyEntity);

    return companyToReturn;
  }

  public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
  {
    if (ids is null) throw new IdParametersBadRequestException();
    var companyEntities = repository.Company.GetByIds(ids, trackChanges);
    if (ids.Count() != companyEntities.Count()) throw new CollectionByIdsBadRequestException();
    var companiesToReturn = mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
    return companiesToReturn;
  }
}