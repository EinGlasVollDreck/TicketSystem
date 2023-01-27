using System;

namespace _TicketSystem
{
  public class MultiUserAccessException : Exception
  {
    public MultiUserAccessException(String msg) : base(msg)
    {
    }
    public MultiUserAccessException()
    {
    }
  }
}
