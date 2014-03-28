using System;
using System.Linq.Expressions;
using Brainshare.Infrastructure.Platform.Extensions;
using FluentValidation.Validators;

namespace Brainshare.Infrastructure.Platform.Validation
{
    public class DateTimeValidator: PropertyValidator
    {
        public DateTimeValidator():this(() => "Can't parse date.")
        {
            
        }

        public DateTimeValidator(string errorMessageResourceName, Type errorMessageResourceType) : base(errorMessageResourceName, errorMessageResourceType)
        {
        }

        public DateTimeValidator(string errorMessage) : base(errorMessage)
        {
        }

        public DateTimeValidator(Expression<Func<string>> errorMessageResourceSelector) : base(errorMessageResourceSelector)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = (string) context.PropertyValue;
            if (value.HasValue())
            {
                return value.ToNullableDateTime().HasValue;
            }
            return true;
        }
    }
}