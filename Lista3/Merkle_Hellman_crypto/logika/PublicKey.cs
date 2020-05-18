using Math.Gmp.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merkle_Hellman_crypto
{
    class PublicKey
    {

        private List<mpz_t> Sequence;

        public void SetSequence(List<mpz_t> l)
        {
            this.Sequence = l;
        }

        public List<mpz_t> GetSequence()
        {
            return this.Sequence;
        }


    }
}
