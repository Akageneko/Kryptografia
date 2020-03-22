using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie_4_schrinking_generator
{
    class Program
    {
        static void Main(string[] args)
        {

            List<bool> input = new List<bool>()     {false, false, true, false, true, false, true, true, false, true};
            List<bool> control = new List<bool>()   {false, false, true, false, true, false, true, true, false, true};

            //ShrinkingGenerator gen = new ShrinkingGenerator(1, 1, 10, 10, input, control);


            Attack attack = new Attack(1, 10, input, control);

            attack.Do_Attack();

            List<bool> input_control = attack.Get_LFSR_control();
            List<int> input_crackerd = attack.Get_Input();
            Console.Write("Cracked Input:");
            foreach (int i in input_crackerd) Console.Write(" {0}",i);
            Console.WriteLine();
            Console.Write("Cracked LFSR cotorl input:");
            foreach (bool i in input_control) Console.Write(" {0}", i);
            Console.WriteLine();

            Console.ReadKey();


        }
    }
}
