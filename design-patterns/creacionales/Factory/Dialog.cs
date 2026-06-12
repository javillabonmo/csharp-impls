namespace Factory
{


    public abstract class Dialog
    {
        public abstract IButton CreateButton();
        public abstract string Render();

    }
}
