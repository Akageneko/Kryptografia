using Math.Gmp.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merkle_Hellman_crypto
{
    class Generator
    {

        Random rand;

        public Generator()
        {
            rand = new Random();
        }

        public List<mpz_t> SuperincresingSentence(int len)
        {
            List<mpz_t> list = new List<mpz_t>();

            list.Add((mpz_t)rand.Next().ToString());
            for (int i = 0; i < len-1; i++)
            {
                mpz_t a = new mpz_t();
                gmp_lib.mpz_init(a);
                gmp_lib.mpz_add(a, list[i], list[i]);
                gmp_lib.mpz_add(a, a, (mpz_t)rand.Next().ToString());
                list.Add(a);
            }

            return list;
        }

        public mpz_t SumList(List<mpz_t> list)
        {
            mpz_t wyn = new mpz_t();
            gmp_lib.mpz_init(wyn);
            foreach (mpz_t a in list)
            {
                gmp_lib.mpz_add(wyn, wyn, a);
            }
            
            return wyn;
        }

        public mpz_t GetNextPrime(mpz_t n)
        {
            mpz_t wyn = new mpz_t();
            gmp_lib.mpz_init(wyn);
            gmp_lib.mpz_nextprime(wyn, n);

            return wyn; 
        }

        public mpz_t GetCoprimeToPrime(mpz_t prime)
        {
            mpz_t wyn = new mpz_t();
            gmp_lib.mpz_init(wyn);
            gmp_lib.mpz_sub(wyn, prime, (mpz_t)rand.Next().ToString());

            return wyn;
        }
        
        public List<mpz_t> GetPublicKey(List<mpz_t> list, mpz_t bigger, mpz_t smaller)
        {

            List<mpz_t> wyn = new List<mpz_t>();
            
            foreach(mpz_t l in list)
            {
                mpz_t a = new mpz_t();
                gmp_lib.mpz_init(a);
                gmp_lib.mpz_mul(a, l, smaller);
                gmp_lib.mpz_mod(a, a, bigger);
               
                wyn.Add(a);
            }

            return wyn;
        }
    
    
    
    }
}
