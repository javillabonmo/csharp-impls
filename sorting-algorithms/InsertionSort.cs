using System;
using System.Globalization;

namespace SortingAlgorithms
{
    public static class InsertionSort
    {
        public static void Main(string[] args)
        {
            //es menos eficiente en datasets largos, por que es O(n^2) en el peor de los casos
            //pero eficiente en datasets cortos, por que es O(n) en el mejor de los casos
            //se puede usar en produccion para datasets de como 10 y consume poca memoria
            //Stable: Does not change the relative order of elements with equal keys
            List<int> nums = new List<int> { 5, 2, 9, 1, 5, 6 };
            Sort(nums);
            foreach (var num in nums)
            {
                Console.Write(num + " ");
            }
        }

        public static List<int> Sort(List<int> nums)
        {
            for (int i = 1; i < nums.Count; i++)
            {
                while (i > 0 && nums[i - 1] > nums[i])
                {
                    (nums[i], nums[i - 1]) = (nums[i - 1], nums[i]);
                    i--;
                }
            }
            return nums;
        }
    }
}