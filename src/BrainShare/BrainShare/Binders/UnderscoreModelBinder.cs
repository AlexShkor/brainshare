using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace BrainShare.Binders
{

    public class UnderscoreModelBinder : DefaultModelBinder
    {
        public UnderscoreModelBinder()
        {

        }

        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            var propertyBinderAttribute = propertyDescriptor
                .Attributes
                .OfType<PropertyBinderAttribute>()
                .FirstOrDefault();

            if (propertyBinderAttribute != null)
            {
                var value = propertyBinderAttribute.BindModel(controllerContext, bindingContext);
                propertyDescriptor.SetValue(bindingContext.Model, value);
            }
            else
            {
                base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
            }
        }
    }

    [AttributeUsageAttribute(AttributeTargets.Property)]
    public abstract class PropertyBinderAttribute : Attribute, IModelBinder
    {
        public abstract object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext);
    }

    public class ParameterNameAttribute : PropertyBinderAttribute
    {
        private readonly string parameterName;
        public ParameterNameAttribute(string parameterName)
        {
            this.parameterName = parameterName;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(this.parameterName);
            if (value != null)
            {
                return value.AttemptedValue;
            }
            return null;
        }
    }
}