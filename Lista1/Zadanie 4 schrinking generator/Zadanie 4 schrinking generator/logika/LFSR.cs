using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie_4_schrinking_generator
{ 
    class LFSR
    {
        int modulo;
        int length;

        List<int> list = new List<int>();
        List<bool> combine;
        List<int> all = new List<int>();

        Rand rand;

        public LFSR(int modulo, int length, List<bool> comb)
        {
            this.modulo = modulo;
            this.length = length;
            this.combine = comb;
        }

        public void Init_List(Func< int> Next)
        {
            int temp = 0;
            for(int i = 0; i<length; i++)
            {
                temp = Next();
                list.Add(temp);
                all.Add(temp);
            }
        }

        public int Step()
        {
            int temp = 0;
            for(int i=0; i<list.Count; i++)
            {
                if (combine[i])
                {
                    temp += list[i];
                }
            }
            temp = temp % modulo;

            all.Add(temp);
            list.Insert(0,temp);
            temp = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return temp;
        }

        public List<int> Get_All()
        {
            return all;
        }

    }
}
