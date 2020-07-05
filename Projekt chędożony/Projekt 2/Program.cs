using Projekt_2.logika;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_2
{
    class Program
    {
        static void Main(string[] args)
        {
            int uczestnikow = 4;
            string sciezka_certyfikaty = @"C:\infa\s8\Krypto\git\Kryptografia\Projekt chędożony\Projekt 2\bin\Debug\";



            Game game = new Game(uczestnikow, sciezka_certyfikaty);




            Console.ReadKey();
        }
    }
}
