using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractFactory
{
    internal class Application
    {

        private IGUIFactory _factory;
        private IButton _button;


        public Application(IGUIFactory factory)
        {
            _factory = factory;
        }

        public void CreateUI()
        {

            _button = _factory.CreateButton();
        }

        public void Paint()
        {
            
            _button.paint();
        }


    }
}
