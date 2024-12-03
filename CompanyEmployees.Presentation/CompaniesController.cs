using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController(IServiceManager service) : ControllerBase
{
  [HttpGet]
  public IActionResult GetCompanies()
  {
    var companies = service.CompanyService.GetAllCompanies(trackChanges: false);

    return Ok(companies);
  }

  [HttpGet("{id:guid}", Name = "CompanyById")]
  public IActionResult GetCompany(Guid id)
  {
    var company = service.CompanyService.GetCompany(id, trackChanges: false);
    return Ok(company);
  }

  [HttpPost]
  public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
  {
    if (company is null) return BadRequest("CompanyForCreationDto object is null");
    var createdCompany = service.CompanyService.CreateCompany(company);

    return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
  }

  [HttpGet("collection/({ids})", Name = "CompanyCollection")]
  public IActionResult GetCompanyCollection(IEnumerable<Guid> ids)
  {
    var companies = service.CompanyService.GetByIds(ids, trackChanges: false);

    return Ok(companies);
  }
}