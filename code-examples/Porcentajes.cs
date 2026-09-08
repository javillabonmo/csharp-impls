namespace CodeExamples
{
    public static class Porcentajes
    {

        public static void Main(string[] args)
        {
            decimal[] items = { 15.97m, 3.50m, 12.25m, 22.99m, 10.98m };
            decimal[] discounts = { 0.30m, 0.00m, 0.10m, 0.20m, 0.50m };

            for (int i = 0; i < items.Length; i++)
            {
                Console.WriteLine(items[i] * (1 - discounts[i]));
                // itera y aplica sobre cada elemento un descuento donde 1 representa el 100%
            }
        }

    }
}