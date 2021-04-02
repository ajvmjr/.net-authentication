using authentication.Data.Entities;
using authentication.Data.Exceptions;
using authentication.Data.Repositories;
using AutoMapper;
using Signa.Library.Core.Extensions;

namespace authentication.Business
{
  public class UserBL
  {
    private readonly IMapper _mapper;
    private readonly UserDAO _userDAO;

    public UserBL(IMapper mapper, UserDAO userDAO)
    {
      _mapper = mapper;
      _userDAO = userDAO;
    }

    public UserEntity CheckUserExistance(string email, string password)
    {
      if (email.IsNullOrEmpty() || password.IsNullOrEmpty()) throw new AppException("Informe e-mail e senha.");

      var user = _userDAO.CheckUserExistance(email, password);
      if (user == null) throw new AppException("Usuário não existe");
      return user;
    }

    public object RegisterUser(string name, string email, string password)
    {
      if (name.IsNullOrEmpty() || email.IsNullOrEmpty() || password.IsNullOrEmpty()) throw new AppException("Informe nome, e-mail e senha.");

      var response = _userDAO.RegisterUser(name, email, password);

      if (response <= 0) throw new AppException("Erro ao criar usuário.");

      return new { message = "Usuário criado com sucesso." };
    }
  }
}