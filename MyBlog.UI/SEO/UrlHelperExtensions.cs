using System.Web.Mvc;
namespace MyBlog.UI.SEO
{
    public static class UrlHelperExtensions
    {
        public static string QualifiedAction(this UrlHelper url, string actionName, string controllerName, object routeValues = null)
        {
            return url.Action(actionName, controllerName, routeValues, url.RequestContext.HttpContext.Request.Url.Scheme);
        }
    }
}