using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Newtonsoft.Json;

namespace Brainshare.Infrastructure.Platform.Mvc
{
    public static class HtmlHelperExt
    {
         public static IHtmlString Errors(this HtmlHelper source, string modelPrefix = "model.")
         {
             return source.Partial("KnockoutValidationSummary",modelPrefix);
         }
         public static IHtmlString JsonModel<TModel>(this HtmlHelper<TModel> source, TModel model)
         {
             return source.Raw(JsonConvert.SerializeObject(model));
         }

         public static bool IsAuthenticated(this HtmlHelper source)
         {
             return source.ViewContext.ViewData["UserName"] != null;
         } 


         public static string UserName(this HtmlHelper source)
         {
             return (string) source.ViewContext.ViewData["UserName"];
         } 
    }
}