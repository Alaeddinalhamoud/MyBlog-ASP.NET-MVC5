using System.Collections.Generic;
using System.Xml.Linq;

namespace MyBlog.UI.SEO
{
 public   interface ISitemapGenerator
    {
        XDocument GenerateSiteMap(IEnumerable<ISitemapItem> items);
    }
}
    