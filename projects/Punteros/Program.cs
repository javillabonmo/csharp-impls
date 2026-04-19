namespace Punteros
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int i = 0;

            unsafe
            {
                Incrementar(&i);
            }
        }

        public unsafe static void Incrementar(int* p)
        {
            *p = *p + 1;
        }
    }
}
