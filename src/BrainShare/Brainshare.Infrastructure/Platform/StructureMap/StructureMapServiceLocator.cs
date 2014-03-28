using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using StructureMap;

namespace Brainshare.Infrastructure.Platform.StructureMap
{
    public class StructureMapServiceLocator : ServiceLocatorImplBase
    {
        private readonly IContainer _container;

        public StructureMapServiceLocator(IContainer container)
        {
            _container = container;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return string.IsNullOrEmpty(key)
                ? _container.GetInstance(serviceType)
                : _container.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            foreach (object obj in _container.GetAllInstances(serviceType))
            {
                yield return obj;
            }
        }
    }
}