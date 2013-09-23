using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BrainShare.Extensions
{
    public static class BootstrapExtensions
    {
        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string additionalCssClass = null, string innerHtml = null)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            
            var innerLink = String.Format("<a href=\"/{0}/{1}\">{2}{3}</a>", controllerName, actionName, linkText, innerHtml);
            
            var li = new TagBuilder("li")
            {
                InnerHtml = innerLink
                //InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName).ToHtmlString()
            };

            
            if (additionalCssClass != null)
                li.AddCssClass(additionalCssClass);

            if (controllerName == currentController && actionName == currentAction)
                li.AddCssClass("active");

            return new MvcHtmlString(li.ToString());
        }

        public static MvcHtmlString MenuDropdown(this HtmlHelper htmlHelper, Dictionary<string, List<string>> actions, string name)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            var dropdownLi = new TagBuilder("li");
            dropdownLi.AddCssClass("dropdown");
            dropdownLi.InnerHtml = "<a href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\">" + name + "<b class=\"caret\"></b></a>";

            var innerUl = new TagBuilder("ul");
            innerUl.AddCssClass("dropdown-menu");

            string innerLi = "";
            foreach (var pair in actions)
            {
                var li = "<li>" + htmlHelper.ActionLink(pair.Key, pair.Value[0], pair.Value[1]).ToHtmlString() + "<li>";
                innerLi += li;
            }

            innerUl.InnerHtml = innerLi;
            dropdownLi.InnerHtml += innerUl;

            var values = actions.Values;

            var isCurrentAction = values.Any(v => v[0] == currentAction);
            var isCurrentController = values.Any(v => v[1] == currentController);

            if (isCurrentAction && isCurrentController)
            {
                dropdownLi.AddCssClass("active");
            }

            return new MvcHtmlString(dropdownLi.ToString());
        }
    }
}



//public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string additionalCssClass = null)
//        {
//            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
//            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

//            var li = new TagBuilder("li")
//            {
//                InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName).ToHtmlString()
//            };

//            if (additionalCssClass != null)
//                li.AddCssClass(additionalCssClass);

//            if (controllerName == currentController && actionName == currentAction)
//                li.AddCssClass("active");

//            return new MvcHtmlString(li.ToString());
//        }