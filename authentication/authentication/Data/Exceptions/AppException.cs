using System;

namespace authentication.Data.Exceptions
{
  public class AppException : ApplicationException
  {
    public AppException(string message) : base(message)
    {

    }
  }
}