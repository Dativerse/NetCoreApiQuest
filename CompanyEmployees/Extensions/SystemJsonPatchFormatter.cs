using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CompanyEmployees.Extensions
{
  public static class SystemJsonPatchFormatter
  {
    public static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
    {
      var builder = new ServiceCollection()
          .AddLogging()
          .AddMvc()
          .AddNewtonsoftJson()
          .Services.BuildServiceProvider();

      return builder
          .GetRequiredService<IOptions<MvcOptions>>()
          .Value
          .InputFormatters
          .OfType<NewtonsoftJsonPatchInputFormatter>()
          .First();
    }
  }
}
