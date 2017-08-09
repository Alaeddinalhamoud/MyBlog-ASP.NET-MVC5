using MyBlog.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.UI.HtmlHelpers
{
    public static class PagingHelpers
    {

        public static MvcHtmlString PageLinks(this HtmlHelper html,
                                               PagingInfo pagingInfo,
                                               Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                // Build I tag
                var iTagBuilder = new TagBuilder("li");

                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                 
                tag.AddCssClass("btn btn-default");
                // Render the end tag
            iTagBuilder.ToString(TagRenderMode.EndTag);
                // Add the I tag to the A tag><
                iTagBuilder.InnerHtml += tag.ToString();

                result.Append(iTagBuilder.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}
