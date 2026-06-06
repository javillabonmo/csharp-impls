namespace Program
{
    internal class StairCase
    {
        static void Main(string[] args)
        {
            staircase(6);
        }
        public static void staircase(int n)
        {
            for (int j = 1; j <= n; j++)
            {
                for (int i = 1; i <= n; i++)
                {
                    int diff = n - j;
                    if (i > diff)
                    {
                        Console.Write("#");

                    }
                    else
                    {
                        Console.Write(" ");
                    }
                    if (i == n)
                    {
                        Console.WriteLine("");
                    }
                }
            }



        }
    }
}
