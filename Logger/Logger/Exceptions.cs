using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    class PasswordProcessException : Exception
    {
        public PasswordProcessException()
        {

        }

        public PasswordProcessException(string message):base(message)
        {
            
        }

    }
}
