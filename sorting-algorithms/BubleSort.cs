using System;

namespace SortingAlgorithms
{
    public static class BubbleSort
    {
        //la complejidad es O(n^2) en el peor de los casos
        //la complejidad es O(n) en el mejor de los casos
        public static void Main(string[] args)
        {
            int[] numbers = { 5, 3, 8, 4, 2 };
            Sort(numbers);
            Console.WriteLine("Sorted: " + string.Join(", ", numbers));
        }

        public static void Sort(int[] array)
        {
            if (array == null || array.Length < 2)
            {
                return;
            }

            int end = array.Length;
            bool swapped;

            while (end > 1)
            {
                swapped = false;
                for (int i = 0; i < end - 1; i++)
                {
                    if (array[i] > array[i + 1])
                    {

                        int temp = array[i];
                        array[i] = array[i + 1];
                        array[i + 1] = temp;
                        swapped = true;
                    }
                }
                if (!swapped)
                {
                    break;
                }
                end--;
            }
        }
    }
}
