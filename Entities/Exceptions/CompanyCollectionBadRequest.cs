namespace Entities.Exceptions
{
  public sealed class CompanyCollectionBadRequest : BadRequestException
  {
    public CompanyCollectionBadRequest()
     : base("Collection count mismatch comparing to ids.")
    {
    }
  }
}
