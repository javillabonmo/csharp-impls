namespace Factory
{

    public class WindowsButton : IButton
    {
        public string Render()
        {
            return "WindowsButton: Handling onClick event in windows.";
        }
        public string OnClick()
        {
            return "WindowsButton: Rendering a button in the Windows OS.";
        }
    }
}
