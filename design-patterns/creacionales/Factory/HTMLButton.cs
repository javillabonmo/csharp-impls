namespace Factory
{
    public class HTMLButton : IButton
    {
        public string OnClick()
        {
            return "WebButton: Handling onClick event.";
        }

        public string Render()
        {
            return "WebButton: Rendering a button in a web page.";
        }
    }
}
