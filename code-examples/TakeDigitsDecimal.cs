namespace CodeExamples
{
    public static class TakeDigitsDecimal
    {

        public static void Main(string[] args)
        {
            Console.WriteLine($"{gradePointAverage}");
            Console.WriteLine($"{leadingDigit}.{firstDigit}{secondDigit}");
            //3.3529411764705882352941176471
            //3.35
            int leadingDigit = (int)gradePointAverage;
            int firstDigit = (int)(gradePointAverage * 10) % 10;
            int secondDigit = (int)(gradePointAverage * 100) % 10;
        }

    }
}