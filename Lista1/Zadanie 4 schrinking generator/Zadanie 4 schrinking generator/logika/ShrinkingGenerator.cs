using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie_4_schrinking_generator
{
    class ShrinkingGenerator
    {

        LFSR input_generator;
        LFSR control_generator;
        Rand rand;
        public ShrinkingGenerator(int seed_input, int seed_control, int length, int input_modulo, List<bool> lfsr_control_input, List<bool> lfsr_control_control)
        {
            rand = new Rand(seed_input, seed_control, input_modulo);
            input_generator = new LFSR(input_modulo, length, lfsr_control_input);
            control_generator = new LFSR(2, length, lfsr_control_control);

            input_generator.Init_List(rand.Get_Next_Input);
            control_generator.Init_List(rand.Get_Next_Control);
        }

        public int Make_Step()
        {
            int input = input_generator.Step();
            
            if (control_generator.Step() == 1)
                return input;
            else
                return -1;
        }

        public void Stop_Control_Step()
        {
            input_generator.Step();
        }
    }
}
