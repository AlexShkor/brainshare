using FluentValidation;

namespace Brainshare.Infrastructure.Platform.Validation
{
    public static class Ext
    {
         public static IRuleBuilderOptions<T,string> DateTime<T>(this IRuleBuilder<T,string> ruleBuilder)
         {
             return ruleBuilder.SetValidator(new DateTimeValidator());
         } 
    }
}