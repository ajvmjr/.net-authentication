namespace authentication.Data.Entities
{
  public class UserEntity
  {
    public int IdUser { get; set; }
    public string NameUser { get; set; }
    public string EmailUser { get; set; }
    public string PasswordUser { get; set; }
    public string RoleUser { get; set; }
  }
}