namespace WrapAroundImpl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int tipoOrigen = 300;
            try
            {
                byte tipoDestino = (byte)tipoOrigen;//detecta el desbordamiento y lanza una excepción
                Console.WriteLine($"The value {tipoOrigen} was successfully converted to byte: {tipoDestino}");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Overflow occurred!");

            }
        }
    }
}
