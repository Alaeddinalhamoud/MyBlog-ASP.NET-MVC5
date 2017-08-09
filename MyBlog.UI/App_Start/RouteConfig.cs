using MyBlog.UI.SEO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyBlog.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add("PostDetails", new SeoFriendlyRoute("Post/details/{id}",
            new RouteValueDictionary(new { controller = "Post", action = "Details" }),
            new MvcRouteHandler()));

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(null, "{controller}/{action}/{postId}",
             
              new { postId = @"\d+" }

              );
            routes.MapRoute(null, "{controller}/{action}/{category}/{page}",
               new { controller = "Post", action = "PostByCategory" },
               new { category = @"\d+" , page = @"\d+" }
              
               );

        }
    }
}
