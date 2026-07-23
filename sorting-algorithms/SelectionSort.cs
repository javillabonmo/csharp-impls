using System;

namespace SortingAlgorithms
{
    public static class SelectionSort
    {
        //la complejidad es O(n^2) en todos los casos
        //es mas lento que O(n) y O(n*(log(n)))
        public static void Main(string[] args)
        {

        }

        public static List<int> SelectionSort(span<int> nums)
        {
            if (nums == null || nums.Length < 2)
            {
                return new List<int>();
            }

            for (int i = 0; i < nums.Length - 1; i++)
            {
                int minIndex = i;

                for (int j = i + 1; j < nums.Length; j++)
                {
                    if (nums[j] < nums[minIndex])
                    {
                        minIndex = j;
                    }
                }
                if (minIndex != i)
                {
                    (nums[i], nums[minIndex]) = (nums[minIndex], nums[i]);
                }
            }

            return new List<int>(nums.ToArray());
        }
    }
}