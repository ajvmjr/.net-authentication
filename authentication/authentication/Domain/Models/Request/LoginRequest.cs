namespace authentication.Domain.Models.Request
{
  public class LoginRequest
  {
    public string email { get; set; }
    public string password { get; set; }
  }
}