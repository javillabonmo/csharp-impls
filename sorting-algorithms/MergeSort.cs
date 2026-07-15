using System;

namespace SortingAlgorithms
{
    public static class MergeSort
    {
        //la complejidad es O(n*(log(n))) en todos los casos
        //es mas lento que O(n) pero mas rapido que O(n^2)
        public static void Main(string[] args)
        {

        }

        public static List<int> MergeSort(span<int> array)
        {
            if (array == null || array.Length < 2)
            {
                return new List<int>();
            }

            int mid = array.Length / 2;//casteo implicito, si es impar se redondea hacia abajo

            span<int> first = array.Slice(0, mid);
            span<int> second = array.Slice(mid);

            MergeSort(first);
            MergeSort(second);

            return Merge(first, second);
        }

        public static List<int> Merge(span<int> first, span<int> second)
        {
            List<int> final = new List<int>();
            int i = 0, j = 0;


            //esta es la logica compleja a mi entender T.T

            while (i < first.Length && j < second.Length)
            {
                if (first[i] < second[j])
                {
                    final.Append(first[i]);
                    i++;
                }
                else
                {
                    final.Append(second[j]);
                    j++;
                }
            }

            while (i < first.Length)
            {
                final.Append(first[i]);
                i++;
            }
            while (j < second.Length)
            {
                final.Append(second[j]);
                j++;
            }

            return final;
        }
    }
}
