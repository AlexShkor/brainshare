using System;

namespace Brainshare.Infrastructure.Platform.Output
{
    public class ConsoleWrapper : IOutputWriter, IInputReader
    {
        public void Init(Action<string> callback)
        {
        }

        public void Write(string format, params object[] args)
        {
            Console.Write(format, args);
        }
        public void WriteLine(string format = "", params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void ReadKey()
        {
            Console.ReadKey();
        }
    }
}