using System.Web;
using System.Web.Optimization;


namespace MyBlog.UI.App_Start
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                         "~/Scripts/jquery-ui-1.12.1.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/AJAX").Include(
                  "~/Scripts/jquery.unobtrusive-ajax.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/LoadMeAJAX").Include(
                 "~/Scripts/LoadMeAJAX.js"));

            bundles.Add(new ScriptBundle("~/bundles/ckeditor").Include(
                        "~/Scripts/ckeditor/ckeditor.js"));
           
            bundles.Add(new ScriptBundle("~/bundles/FooterJs").Include(
                      "~/Footer/js/css3-animate-it.js"));
            bundles.Add(new ScriptBundle("~/bundles/MaterializeJS").Include(
                     "~/Scripts/materialize/materialize.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/MyFile").Include(
                      "~/Scripts/MyFile.js"));
            bundles.Add(new ScriptBundle("~/bundles/ImgUploder").Include(
                      "~/Scripts/ImgUploder.js"));





            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new StyleBundle("~/Content/Loadme").Include(
                    "~/Content/loadme.css"));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/scripts/bootstrap.min.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new StyleBundle("~/Content/Site").Include("~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/BootStrap").Include(
                "~/Content/bootstrap.min.css"));
           
            bundles.Add(new StyleBundle("~/Content/Dashbord").Include("~/Content/Dashbord.css"));

            bundles.Add(new StyleBundle("~/Content/font-awesome").Include(
                     "~/Content/font-awesome.min.css"));
        

            bundles.Add(new StyleBundle("~/Content/CSS-Comments").Include(
                     "~/Content/Comments.css"));
            bundles.Add(new StyleBundle("~/Content/CSS-Footer").Include(
                     "~/Footer/css/animations.css",
                      "~/Footer/css/footer-1.css"));
            bundles.Add(new StyleBundle("~/Content/CSS-Materialize").Include(
                     "~/Content/materialize/css/materialize.min.css"));
            bundles.Add(new StyleBundle("~/Content/CK-school_book").Include(
                     "~/scripts/ckeditor/plugins/codesnippet/lib/highlight/styles/school_book.css"));

        }
    }
}
