using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using TreeSerialize;

namespace Test
{
    [Serializable]
    class A
    {
        private string A1 = "asdfasdf.";
        private string A2 = "asdfasdf.";
    }

    [Serializable]
    class B
    {
        private A B1 = new A();
        private A B2 = new A();
        private A B3 = new A();
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Serialize(new B()).Length);
        }

        static byte[] Serialize(object obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }
    }
}