using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Lexico_2
{
    public class Error : Exception
    {
        public Error(string message, StreamWriter log) : base(message)
        {
            log.WriteLine("Error: " + message);
        }

        public Error(String message, StreamWriter log, int line) : base (message + "on line " + line)
        {
            log.WriteLine("Error: " + message + " on line " + line);
        }
    }
}