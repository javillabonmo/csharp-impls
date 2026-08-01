namespace Factory
{
    internal class Program
    {
        public static void Main(String[] args)
        {

            Application a = new();
            a.Initialize();
            a.dialog.Render();
        }
    }

    public class Application
    {
        public Dialog dialog;
        public void Initialize()
        {
            string config = "Windows";
            dialog = config switch
            {
                "Windows" => new WindowsDialog(),
                "Web" => new WebDialog(),
                _ => throw new Exception($"Error: Unknown configuration {config}.")
            };

        }
    }

}


