namespace CodeExamples
{
    public static class SumTarget
    {

        public static void Main(string[] args)
        {
            int[] coins = [5, 5, 50, 25, 25, 10, 5];

            //buscar en una matriz dos elementos cuyos valores sumados sean target 
            int[] TwoCoins(int[] coins, int target)
            {
                int[] sum = new int[2];

                for (int i = 0; i < coins.Length; i++)
                {
                    for (int j = 0; j < coins.Length; j++)
                    {
                        if (j == 0)
                        {
                            continue;
                        }
                        if (coins[i] + coins[j] == target)
                        {
                            return [i, j];
                        }
                    }
                }

                return [];
            }

            TwoCoins(coins, 15);
        }

    }
}