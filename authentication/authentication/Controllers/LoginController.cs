using System;
using System.Threading.Tasks;
using authentication.Business;
using authentication.Domain.Models.Request;
using authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication.Controllers
{
  [ApiController]
  [Route("account")]
  public class LoginController : ControllerBase
  {
    private readonly UserBL _userBL;

    public LoginController(UserBL userBL)
    {
      _userBL = userBL;
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public dynamic Authenticate([FromBody] LoginRequest model)
    {
      var user = _userBL.CheckUserExistance(model.email, model.password);
      var token = TokenService.GenerateToken(user);

      user.PasswordUser = "";

      return new
      {
        user = user,
        token = token
      };
    }

    [HttpPost]
    [Route("signup")]
    [AllowAnonymous]
    public ActionResult SignUp([FromBody] CreateUserRequest user) => Ok(_userBL.RegisterUser(user.name, user.email, user.password));

    [HttpGet]
    [Route("anonymous")]
    public string Anonymous() => "Anonymous";

    [HttpGet]
    [Route("authenticated")]
    [Authorize]
    public string Autheticated() => String.Format("Autenticado - {0}", User.Identity.Name);

    [HttpGet]
    [Route("administrator")]
    [Authorize(Roles = "Administrator")]
    public string Adm() => "Administrator";
  }
}