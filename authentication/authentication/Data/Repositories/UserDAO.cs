using authentication.Data.Entities;
using Dapper;
using Signa.Library.Core.Data.Repository;

namespace authentication.Data.Repositories
{
  public class UserDAO : RepositoryBase
  {
    public UserEntity CheckUserExistance(string userEmail, string userPassword)
    {
      var sql = @"Select *
                    From   Users
                    Where  Email_User = @userEmail And Password_User = @userPassword";
      using (var db = Connection)
      {
        return db.QueryFirstOrDefault<UserEntity>(sql, new { userEmail, userPassword });
      }
    }

    public int RegisterUser(string userName, string userEmail, string userPassword)
    {
      var sql = @"Insert Into Users
                    (Name_User, 
                    Email_User, 
                    Password_User,
                    Role_User
                    )
                    Values
                    (@userName, 
                    @userEmail, 
                    @userPassword,
                    @defaultRole
                    );
                    Select Scope_Identity()";

      string defaultRole = "Common";

      using (var db = Connection)
      {
        return db.ExecuteScalar<int>(sql, new
        {
          userName,
          userEmail,
          userPassword,
          defaultRole
        });
      }
    }
  }
}