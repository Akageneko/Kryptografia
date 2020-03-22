using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie_4_schrinking_generator
{
    public class Rand
    {
        private int seed_input = 0;
        private int seed_control = 0;

        private int input_modulo = 100;

        private Random random_input;
        private Random random_control;

        public Rand(int seed_input, int seed_control, int input_modulo)
        {
            this.seed_input = seed_input;
            this.seed_control = seed_control;

            this.input_modulo = input_modulo;

            this.random_input = new Random(seed_input);
            this.random_control = new Random(seed_control);

        }
        public Rand(int seed, int input_modulo)
        {
            this.seed_input = seed;
            this.seed_control = seed;

            this.input_modulo = input_modulo;


            this.random_input = new Random(seed_input);
            this.random_control = new Random(seed_control);
        }

        public Rand()
        {
            this.random_input = new Random(seed_input);
            this.random_control = new Random(seed_control);
        }

        public int Get_Next_Input()
        {
            return random_input.Next(input_modulo);
        }
        public int Get_Next_Control()
        {
            return random_control.Next(0, 2);
        }
    }
}
