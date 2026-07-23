using System;

namespace SortingAlgorithms
{
    public static class BubbleSort
    {
        //la complejidad es O(n^2) en el peor de los casos
        //la complejidad es O(n) en el mejor de los casos
        public static void Main(string[] args)
        {
            int[] numbers = { 5, 3, 8, 4, 2 }; //lengh = 5, en iteraciones usar -1 para no obtener un index out of range exception
            Sort(numbers);
            Console.WriteLine("Sorted: " + string.Join(", ", numbers));
        }

        public static void Sort(int[] nums)
        {
            if (nums == null || nums.Length < 2)
            {
                return;
            }

            int end = nums.Length;
            bool swapped;

            while (end > 1)
            {
                swapped = false;
                for (int i = 0; i < end - 1; i++)
                {
                    if (nums[i] > nums[i + 1])
                    {

                        (nums[i], nums[i + 1]) = (nums[i + 1], nums[i]);
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
