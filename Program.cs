using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lexico_1;

namespace Lexico_2
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (Lexico l = new Lexico())
                {
                    while (!l.finArchivo())
                    {
                        l.nexToken();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}