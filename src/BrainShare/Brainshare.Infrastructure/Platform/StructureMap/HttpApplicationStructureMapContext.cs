using System;
using System.Web;
using StructureMap;

namespace Brainshare.Infrastructure.Platform.StructureMap
{
    /// <summary>
    /// Holds StructureMap container in the HttpApplicationState collection
    /// </summary>
    public class HttpApplicationStructureMapContext
    {
        /// <summary>
        /// Key in the session
        /// </summary>
        private const String StructureMapContextKey = "StructureMapContext";

        /// <summary>
        /// Synchronization object
        /// </summary>
        private static Object _lock = new Object();

        /// <summary>
        /// Current StructureMap container
        /// </summary>
        private static IContainer _current;

        /// <summary>
        /// Singleton StructureMap container access
        /// </summary>
        public static IContainer Current
        {
            get
            {
                IContainer container;

                if (HttpContext.Current != null)
                    container = HttpContext.Current.Application[StructureMapContextKey] as IContainer;
                else
                    container = _current;

                // Double check
                if (container == null)
                {
                    lock (_lock)
                    {
                        if (container == null)
                        {
                            container = new Container();

                            if (HttpContext.Current != null)
                                HttpContext.Current.Application[StructureMapContextKey] = container;
                            else
                                _current = container;
                        }
                    }
                }

                return container;
            }
        }
    }
}