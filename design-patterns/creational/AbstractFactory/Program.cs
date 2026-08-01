// lo importante de este patron es que se enfoca en crear familias de
// objetos relacionadas


using AbstractFactory;

string config = "Windows";
IGUIFactory? factory = null;

if (config == "Windows")
{
    factory = new WinFactory();
}
else if(config == "Mac")
{
    factory = new MacFactory();
}
else
{
    throw new Exception("No se ha encontrado la configuración");
}

Application application = new Application(factory);