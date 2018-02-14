using System;
using System.Collections.Generic;
using System.Text;

namespace DMC_NET.Exceptions
{
    public class UnexpectedErrorException : Exception
    {
        public UnexpectedErrorException(string message) : base(message)
        {

        }

        public UnexpectedErrorException(string message, Exception internalException) : base(message, internalException)
        {

        }
    }
}
