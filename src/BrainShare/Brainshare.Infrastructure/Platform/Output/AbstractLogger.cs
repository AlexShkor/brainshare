using System;

namespace Brainshare.Infrastructure.Platform.Output
{
    public class AbstractLogger : IOutputWriter
    {
        private Action<string> _callback;

        public void Init(Action<string> callback)
        {
            _callback = callback;
        }

        public void Write(string format, params object[] args)
        {
            WriteLine(format, args);
        }

        public void WriteLine(string format = "", params object[] args)
        {
            _callback(string.Format(format, args));
        }
    }
}