using System;
namespace SortingAlgorithms
{
    public static class QuickSort
    {
        // no requiere copying and allocating, merge sort es mas estable
        //la complejidad es O(n*log(n)) en promedio y O(n^2) en el peor caso mas que nada en listas ordenadas
        //es mas rapido que O(n^2) y mas lento que O(n)


        //fixing quicksort
        // random aproach: la lista de entrada la mezclamos que es o(n) y esto nos da un O(n*log(n)) en promedio
        // median approach: se elige el pivot como la mediana de 3 elementos, el primero, el ultimo y el del medio, esto nos da un O(n*log(n)) en promedio
        // es mas util que random approach ya que no se mezcla la lista por lo que es mas deterministico

        public static void Main(int[] args)
        {
            if (array == null || array.Length <= 1) return;
            QuickSort(args, 0, args.Length - 1);
            Console.WriteLine("Array ordenado: " + string.Join(", ", args));
        }

        public static void QuickSort(int[] nums, int low, int high)
        {
            if (low >= high) return; //early return

            int middle = Partition(nums, low, high);
            QuickSort(nums, low, middle - 1);
            QuickSort(nums, middle + 1, high);

        }

        // se compara j con el pivot y se intercambia i con j si j es menor o igual al pivot
        // al final se intercambia el pivot con i+1 y se retorna i+1
        // complejidad O(n) en todos los casos
        // median aproach
        public static int Partition(int[] nums, int low, int high)
        {
            int pivot = nums[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (nums[j] <= pivot)
                {
                    i++;
                    (nums[i], nums[j]) = (nums[j], nums[i]);
                }
            }
            (nums[i + 1], nums[high]) = (nums[high], nums[i + 1]);
            return i + 1;
        }



    }
}