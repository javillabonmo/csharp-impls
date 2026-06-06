using System.Collections.Generic;

namespace unidimensional
{
    internal class UnidimensionalArray
    {
        static void Main(string[] args)
        {
            //1. Fundamentos programacion | quinta edicion - luis joyanes aguilar
            //pag 264

            string[] carNames = new string[8];
            carNames[0] = "Alfa Romeo";
            carNames[1] = "Fiat";
            carNames[2] = "Ford";
            carNames[3] = "Lancia";
            carNames[4] = "Renault";
            carNames[5] = "Seat";

            int i = 0;
            foreach (var name in carNames)
            {
                if (name != null)
                    i++;
                else
                {
                    carNames[i] = "null";
                }
            }

            string[] elements = { "Opel", "Citroen" };

            UnidimensionalArray.InsertElement(4, elements[0], carNames, ref i);
            UnidimensionalArray.InsertElement(1, elements[1], carNames, ref i);

            UnidimensionalArray.DeleteElement(3, carNames, ref i);
            UnidimensionalArray.DeleteElement(2, carNames, ref i);


            foreach (string item in carNames)
            {
                Console.WriteLine(item);
            }
        }


        public static void InsertElement(int position, string element, string[] list, ref int i)
        {
            int tmp = i - 1;

            while (tmp >= position)
            {

                list[tmp + 1] = list[tmp];
                tmp--;
            }

            list[position] = element;
            i++;
        }

        public static void DeleteElement(int position, string[] list, ref int i)
        {
            i--;

            while (position <= i)
            {
                if (position == i)
                {
                    list[i] = "null";
                }
                else
                {
                    list[position] = list[position + 1];
                }


                position++;
            }

        }
    }

}
