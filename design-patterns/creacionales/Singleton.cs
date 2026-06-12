namespace Singleton
{
    internal class Program
    {
        static void Main()
        {
            TestSingleton("val1");
            TestSingleton("val2");

        }

        public static void TestSingleton(string value)
        {
            Singleton singleton = Singleton.GetInstance(value);
            Console.WriteLine(singleton.Value);
        }

    }
    internal sealed class Singleton
    {
        private Singleton() { }
        private static Singleton? _instance;

        public static Singleton GetInstance(string value)
        {
            //si la instancia es null asigna el objeto de la derecha
            _instance ??= new Singleton()
            {
                Value = value
            };


            return _instance;
        }
        public string? Value { get; set; }

    }
}
