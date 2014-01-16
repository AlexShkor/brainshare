using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace BrainShare.Utils.Extensions
{
    public static class HtmlHelperExt
    {
        private const string BaseApplyModelScriptString = @"<script type=""text/javascript"">
    $(function () {{
        var model = new AllMessagesModel({0});
        ko.applyBindings(model);
    }});
</script>";

        public static IHtmlString ApplyModel(this HtmlHelper page, object model)
        {
            return new MvcHtmlString(string.Format(BaseApplyModelScriptString, JsonConvert.SerializeObject(model)));
        }
    }
}