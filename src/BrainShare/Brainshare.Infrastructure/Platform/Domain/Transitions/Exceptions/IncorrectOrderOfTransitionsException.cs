using System;

namespace Brainshare.Infrastructure.Platform.Domain.Transitions.Exceptions
{
    public class IncorrectOrderOfTransitionsException : Exception
    {
        public IncorrectOrderOfTransitionsException(string message)
            : base(message)
        {
        }

        public IncorrectOrderOfTransitionsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
