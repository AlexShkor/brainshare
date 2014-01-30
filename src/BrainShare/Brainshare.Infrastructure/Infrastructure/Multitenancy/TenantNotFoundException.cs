using System;

namespace Brainshare.Infrastructure.Infrastructure.Multitenancy
{
    public class TenantNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the TenantNotFoundException class
        /// </summary>
        /// <param name="message">Message that describes the error</param>
        public TenantNotFoundException(string message = "A tenant was not found.")
            : base(message ?? string.Empty)
        {
        }
    }
}
