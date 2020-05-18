using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merkle_Hellman_crypto
{
    class Program
    {
        static void Main(string[] args)
        {
            int MarcleHellman_Sentence = 64;

            

            String str = "Nie wiem co dac!";

            /*          BitArray bit = new BitArray(Encoding.UTF8.GetBytes(str));
                        byte[] bytes = new byte[16];
                        bit.CopyTo(bytes, 0);
                        foreach (byte b in bytes) Console.Write(" {0}",b);

                        Console.WriteLine(Encoding.UTF8.GetString(bytes, 0, bytes.Length));

                        //Encoding.ASCII.GetString(bytes, 0, bytes.Length);
            */
            Console.Write("Podaj długość listy która ma być w koderze (mile widziane wilokrotności 8): ");
            MarcleHellman_Sentence = Int32.Parse(Console.ReadLine());

            Console.Write("Podaj string do zakodowania: ");
            str = Console.ReadLine();

            Console.WriteLine("Kodowany string: {0}",str);
            Console.WriteLine("-------------------------------------------");

            CryptoSystem cryptosys = new CryptoSystem(MarcleHellman_Sentence);

            String[] coded = cryptosys.Encode(str);
            Console.WriteLine("Co należało by przesłać po zakodowaniu:");
            foreach (String s in coded) Console.WriteLine(s);

            Console.WriteLine("-------------------------------------------");

            String decoded = cryptosys.Decode(coded);
            Console.WriteLine("Odkodowant string: {0}",decoded);


            Console.ReadKey();
        }
    }
}
