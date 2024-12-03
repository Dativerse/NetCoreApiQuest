using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

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

  [HttpGet("{id:guid}")]
  public IActionResult GetCompany(Guid id)
  {
    var company = service.CompanyService.GetCompany(id, trackChanges: false);
    return Ok(company);
  }
}