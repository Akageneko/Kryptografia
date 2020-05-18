using Math.Gmp.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Merkle_Hellman_crypto
{
    class PrivateKey
    {

        private List<mpz_t> Sequence;
        private mpz_t Bigger;
        private mpz_t Smaller;


        public void SetSequence(List<mpz_t> l)
        {
            this.Sequence = l;
        }

        public void SetBigger(mpz_t a)
        {
            this.Bigger = a;
        }

        public void SetSmaller(mpz_t a)
        {
            this.Smaller = a;
        }
        public List<mpz_t> GetSequence()
        {
            return this.Sequence;
        }

        public mpz_t GetBigger()
        {
            return this.Bigger;
        }

        public mpz_t GetSmaller()
        {
            return this.Smaller;
        }

    }
}
