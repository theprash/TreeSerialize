using System;
using TreeSerialize;

namespace Test
{
    [Serializable]
    class A
    {
        private string A1 = "Some string.";
        private string A2 = "Some string.";
    }

    [Serializable]
    class B
    {
        private A B1 = new A();
        private B B2;
        public int B3 { get; }

        public B(bool first = true)
        {
            if (first) B2 = new B(false);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(TreeSerializer.MakeTree(new B()));
        }
    }
}