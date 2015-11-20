using System;

namespace loghash_testgen
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TestGen generator = new TestGen();
                generator.GenerateFromFile(args[0], args[1]);
            }
            catch (Exception ex)
            {

                Console.Write(ex.ToString());
            }

        }
    }
}
