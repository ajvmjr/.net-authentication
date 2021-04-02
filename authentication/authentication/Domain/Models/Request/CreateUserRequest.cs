namespace authentication.Domain.Models.Request
{
  public class CreateUserRequest
  {
    public string name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
  }
}