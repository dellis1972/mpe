using System;

namespace ProjectMercury.TestBench
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (TestApp game = new TestApp())
            {
                game.Run();
            }
        }
    }
}

