namespace Singleton
{
    internal class Program
    {
        static void Main(string[] args)
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
    class Singleton
    {
        private Singleton() {}
        private static Singleton _instance;

        public static Singleton GetInstance(string value)
        {
            if (_instance == null) {
                _instance = new Singleton();
                _instance.Value = value;
                return _instance;
            }
            return _instance;
        }
        public string Value { get; set; }
        
    }
}
