namespace Program
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<List<int>> arr = new List<List<int>>()
            {
                new List<int>() { 11, 2, 4 },
                new List<int>() { 4, 5, 6 },
                new List<int>() { 10, 8, -12 }
            };



            int result = Result.diagonalDifference(arr);
        }
    }

    public static class Result
    {
        public static int diagonalDifference(List<List<int>> arr)
        {

            List<int> tmpLeftDiag = new List<int>();
            List<int> tmpRigthDiag = new List<int>();


            //leftDiag
            for (int i = 0; i < arr.Count; i++)
            {
                tmpLeftDiag.Add(arr[i][i]);

            }
            int tmpCount = arr.Count - 1;

            for (int i = 0; i < arr.Count; i++)
            {
                
                
                tmpRigthDiag.Add(arr[i][tmpCount]);
                tmpCount = tmpCount - 1;  
            }

            //1. left diagonal sum
            int SubTotalLeft = 0;
            foreach (int num in tmpLeftDiag)
            {
                SubTotalLeft += num;
            }
            //2. left diagonal sum
            int SubTotalRigth = 0;
            foreach (int num in tmpRigthDiag)
            {
                SubTotalRigth += num;
            }

            int result = SubTotalLeft - SubTotalRigth;

            return Math.Abs(result);
        }
    }
}
