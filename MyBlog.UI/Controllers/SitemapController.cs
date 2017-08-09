using MyBlog.UI.SEO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.UI.Controllers
{
    [Authorize(Roles = "Admin")]    
    public class SitemapController : Controller
    {
        // GET: /Sitemap/

        public ActionResult Index()
        {
            var sitemapItems = new List<SitemapItem>
        {
            new SitemapItem(Url.QualifiedAction("index", "home"), changeFrequency: SitemapChangeFrequency.Always, priority: 1.0),
            new SitemapItem(Url.QualifiedAction("Index", "Post"), lastModified: DateTime.Now),
            new SitemapItem(Url.QualifiedAction("index", "PAGE"), lastModified: DateTime.Now),
            new SitemapItem(PathUtils.CombinePaths(Request.Url.GetLeftPart(UriPartial.Authority), "/home/feed"))
        };

            return new SitemapResult(sitemapItems);
        }
    }
}