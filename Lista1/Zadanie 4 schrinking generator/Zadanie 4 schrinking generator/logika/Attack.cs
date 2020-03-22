using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie_4_schrinking_generator
{
    class Attack
    {
        List<ShrinkingGenerator> generators = new List<ShrinkingGenerator>();

        List<List<int>> output = new List<List<int>>();
        List<List<List<int>>> input_combinations = new List<List<List<int>>>();
        List<List<List<int>>> control_combinations = new List<List<List<int>>>();

        List<List<List<bool>>> posibilities = new List<List<List<bool>>>();

        List<int> control = new List<int>();
        List<int> input = new List<int>();
        List<bool> lfsr_control;

        int modulo = 100;
        int length;

        public Attack(int seed, int length, List<bool> lfsr_control_input, List<bool> lfsr_control_control)
        {

            this.length = length;
            for(int i = 0; i<length*2; i++)
            {
                generators.Add(new ShrinkingGenerator(seed, seed, length, modulo, lfsr_control_input, lfsr_control_control));
                output.Add(new List<int>());
                input_combinations.Add(new List<List<int>>());
                control_combinations.Add(new List<List<int>>());

            }
            
        }

        public void Do_Attack()
        {
            int k = 0;
            int j = 0;
            foreach (ShrinkingGenerator g in generators)
            {
                for (j = 0; j < k; j++)
                {
                    g.Stop_Control_Step();
                }
                k++;
            }

            k = 0; 
            foreach(ShrinkingGenerator g in generators)
            {
                for (int i = 0; i < length*2; i++)
                {
                    output[k].Add(g.Make_Step());
                }
                k++;
            }
           
            //Write_Output();

            k = 0;
            foreach(int i in output[0])
            {
                if (i == -1)
                    control.Add(0);
                else
                    control.Add(1);                
            }

            //nadol
            k = 0;
            j = 0;
            for(int i = 0; i< output.Count(); i++)
            {
                for(int z =0; z< output[i].Count(); z++)
                {
                    if(output[i][z] == -1)
                    {
                        k = z-1;
                        j = i+1;
                        while (k >= 0 && j < output.Count() )
                        {
                            if(output[j][k] != -1)
                            {
                                output[i][z] = output[j][k];
                                break;
                            }
                            k--;
                            j++;
                        }
                    }
                }

            }
            //w gore
            k = 0;
            j = 0;
            for (int i = 0; i < output.Count(); i++)
            {
                for (int z = 0; z < output[i].Count(); z++)
                {
                    if (output[i][z] == -1)
                    {
                        k = z + 1;
                        j = i - 1;
                        while (j >= 0 && k < output.Count())
                        {
                            if (output[j][k] != -1)
                            {
                                output[i][z] = output[j][k];
                                break;
                            }
                            k++;
                            j--;
                        }
                    }
                }

            }

            //obracamy pierwsze lenght elementow
            foreach(List<int> l in output)
            {
                int temp;
                for(int i = 0 ; i < length/2; i++)
                {
                    temp = l[i];
                    l[i] = l[length - 1 - i];
                    l[length - 1 - i] = temp;
                }
            }


            //odnajuje wektor lfsr-a
            for (int i = 0; i < output.Count(); i++)
            {
                for (int z = length; z < output[i].Count(); z++)
                {

                }
            }

            Make_All_Posibilities();

            foreach (List<int> l in output){
                All_Modulo_Finding(l);
                int temp = 0;
                foreach (List<List<bool>> ll in posibilities)
                    temp += ll.Count();

                if (temp == 1) break;
            }

            input = output[0];

            
            foreach (List<List<bool>> ll in posibilities)
                if (ll.Count() == 1)
                {
                    lfsr_control = new List<bool>(ll[0]);
                    break;
                }

            
            

        }

        private List<List<bool>> Init_Propabilities(int number, int start, int stop, List<List<bool>> list)
        {
            int numb_of_zer = list.Count - 1;
            number--;
            for (int i = start; i < stop - number; i++)
            {
                list.Add(new List<bool>(list[numb_of_zer]));
                if (number > 0)
                {
                    list.Last()[i] = true;
                    list = Init_Propabilities(number, i+1, stop, list);
                }
                else
                {
                    list.Last()[i] = true;
                }
            }
            list.RemoveAt(numb_of_zer);
            return list;
        }

        private void Make_All_Posibilities()
        {
            for (int i = 1; i <= length; i++)
            {
                List<List<bool>> list = new List<List<bool>>();
                list.Add(new List<bool>());
                for (int j = 0; j < length; j++)
                {
                    list.Last().Add(false);
                }
                posibilities.Add(Init_Propabilities(i, 0, length, list));
            }
        }

        private bool Check_Option(List<int> regi, List<bool> opt, int wyn, int leng)
        {
            
            int temp = 0;
            List<int> registr = new List<int>(regi);
            for (int j = 0; j < leng; j++)
            {
                temp = 0;
                for (int i = 0; i < registr.Count(); i++)
                {
                    if (opt[i])
                    {
                        temp += registr[i];
                    }
                }
                temp = temp % modulo;

                registr.Insert(0, temp);
                registr.RemoveAt(registr.Count - 1);
            }
            
            temp = 0;
            for (int i = 0; i < registr.Count(); i++)
            {
                if (opt[i])
                {
                    temp += registr[i];
                }
            }
            temp = temp % modulo;
            if (temp == wyn)
                return true;
            else
                return false;
        }

        private void All_Modulo_Finding(List<int> inp)
        {

            List<int> regist = new List<int>();
            for(int i = 0; i < length; i++)
            {
                regist.Add(inp[i]);
            }

            for (int i = length; i<inp.Count(); i++)
            {
                foreach (List<List<bool>> ll in posibilities)
                {
                    for(int j = 0; j < ll.Count(); j++)
                    {
                        if (!Check_Option(regist, ll[j], inp[i], i - length))
                        {
                            ll.RemoveAt(j);
                            j--;
                        }
                    }
                }

                int temp = 0;
                foreach(List<List<bool>> ll in posibilities)
                    temp += ll.Count();
                
                if (temp == 1) break;

            }
 
        }


        public void Write_Output()
        {
 
            foreach(List<int> l in output)
            {
                foreach(int i in l)
                {
                    Console.Write(" : {0}",i);
                }
                Console.WriteLine();
            }
        }

        public List<int> Get_Control()
        {
            return control;
        }

        public List<int> Get_Input()
        {
            return input;
        }

        public List<bool> Get_LFSR_control()
        {
            return lfsr_control;
        }




    }
}
