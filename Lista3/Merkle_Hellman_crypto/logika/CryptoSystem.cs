using Math.Gmp.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Merkle_Hellman_crypto
{
    class CryptoSystem
    {

        public PrivateKey privateKey;
        public PublicKey publicKey;

        public CryptoSystem(int sequenceLength)
        {
            privateKey = new PrivateKey();
            publicKey = new PublicKey();
            Generator gen = new Generator();

            privateKey.SetSequence(gen.SuperincresingSentence(sequenceLength));
            privateKey.SetBigger( gen.GetNextPrime( gen.SumList(privateKey.GetSequence())));
            privateKey.SetSmaller(gen.GetCoprimeToPrime(privateKey.GetBigger()));

            //Console.WriteLine(privateKey.GetBigger());
            //Console.WriteLine(privateKey.GetSmaller());

            publicKey.SetSequence(gen.GetPublicKey(privateKey.GetSequence(), privateKey.GetBigger(), privateKey.GetSmaller()));
           
        }

        public String[] Encode(String input)
        {
            BitArray bits = new BitArray(Encoding.UTF8.GetBytes(input));
            List<mpz_t> key = publicKey.GetSequence();
            int n = 0;
            mpz_t temp = new mpz_t();
            gmp_lib.mpz_init(temp);
            List<mpz_t> wyn = new List<mpz_t>();

            

            foreach (Boolean b in bits)
            {
                if(n == key.Count())
                {
                    wyn.Add(temp);
                    temp = new mpz_t();
                    gmp_lib.mpz_init(temp);
                    
                    n = 0;
                }
                if(b) gmp_lib.mpz_add(temp, temp , key[n]);

                n++;
            }
            wyn.Add(temp);

            List<String> strWyn = new List<string>();
            foreach(mpz_t m in wyn)
            {
                strWyn.Add(m.ToString());
               // Console.WriteLine(m);
            }
            return strWyn.ToArray();
        }

        public String Decode(String[] input)
        {
            mpz_t invers = new mpz_t();
            gmp_lib.mpz_init(invers);
            gmp_lib.mpz_invert(invers, privateKey.GetSmaller(), privateKey.GetBigger());
            mpz_t temp = new mpz_t();
            mpz_t zero = new mpz_t();
            gmp_lib.mpz_init(zero);
            gmp_lib.mpz_init(temp);

            List<List<byte>> list = new List<List<byte>>();
            
            int licz = 7;

            byte byte_temp = 0;

            foreach (String s in input)
            {
                //gmp_lib.mpz_sub(temp, temp, temp);
                list.Add(new List<byte>());
                gmp_lib.mpz_mul(temp, invers,  (mpz_t)s);
                gmp_lib.mpz_mod(temp, temp, privateKey.GetBigger());
                for(int i = privateKey.GetSequence().Count()-1; i >= 0; i--)
                {
                    //if (gmp_lib.mpz_cmp(temp, zero) == 0) break;
                    if (gmp_lib.mpz_cmp(temp , privateKey.GetSequence()[i]) >= 0)
                    {
                        gmp_lib.mpz_sub(temp, temp, privateKey.GetSequence()[i]);
                        byte_temp += (byte)Pow(2, licz);
                    }
                    
                    licz--;
                    if (licz == -1)
                    {
                        licz = 7;
                        list[list.Count-1].Add(byte_temp);
                        byte_temp = 0;
                    }
                }
            }

            
            List<byte> lfin = new List<byte>();
            foreach (List<byte> l in list)
            {
                l.Reverse();
                lfin.AddRange(l);    
            }

            byte[] bytes = lfin.ToArray();
           // foreach (byte b in bytes) Console.Write(" {0}",b);
            String str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            return str;
        }

        private int Pow(int a, int b)
        {
            int x = 1;
            for(int i = b; i>0; i--)
            {
                x *= a;
            }
            return x;
        }

    }
}
