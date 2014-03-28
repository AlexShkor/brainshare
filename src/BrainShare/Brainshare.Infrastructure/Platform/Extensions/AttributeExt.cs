using System;
using System.ComponentModel;
using System.Linq;

namespace Brainshare.Infrastructure.Platform.Extensions
{
    public static class AttributeExt
    {
        public static T GetAttribute<T>(this object value) where T : Attribute
        {
            if (value == null)
                return null;

            var fi = value.GetType().GetField(value.ToString());
            if (fi == null) return null;
            var attributes = (T[])fi.GetCustomAttributes(typeof(T), false);

            return attributes.SingleOrDefault(x => x.GetType() == typeof(T));
        }

        public static string GetDescription(this object value)
        {
            if (value == null)
                return null;

            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T GetValueFromDescription<T>(this string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description.Equals(description, StringComparison.InvariantCultureIgnoreCase))
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name.Equals(description, StringComparison.InvariantCultureIgnoreCase))
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }
    }
}