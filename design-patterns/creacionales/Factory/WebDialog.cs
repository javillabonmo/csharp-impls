namespace Factory
{
    public class WebDialog : Dialog
    {

        public override IButton CreateButton()
        {
            return new HTMLButton();
        }
        public override string Render()
        {
            return "the web dialog is render!!!";
        }

    }


}
