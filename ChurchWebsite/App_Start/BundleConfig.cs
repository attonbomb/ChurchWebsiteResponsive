using System.Web;
using System.Web.Optimization;

namespace ChurchWebsite
{
  public class BundleConfig
  {
    // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
    public static void RegisterBundles(BundleCollection bundles)
    {
      //prevents compiler ignoring minified files  
      bundles.IgnoreList.Clear();
        
     /* bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
          "~/Scripts/bjqs-1.3.min.js",        
          "~/Scripts/jquery-2.0.3.min.js"));*/

     bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                  "~/Scripts/jquery-ui-{version}.js"));
      
     bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

      bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                  "~/Scripts/knockout-{version}.js",
                  "~/Scripts/knockout.mapping-latest.js",
                  "~/Scripts/perpetuum.knockout.js"));

      bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                  "~/Scripts/jquery.unobtrusive*",
                  "~/Scripts/jquery.validate*"));

      // Use the development version of Modernizr to develop with and learn from. Then, when you're
      // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
      bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                  "~/Scripts/modernizr-*"));

      bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Site.css"));

      bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                  "~/Content/themes/base/jquery.ui.core.css",
                  "~/Content/themes/base/jquery.ui.resizable.css",
                  "~/Content/themes/base/jquery.ui.selectable.css",
                  "~/Content/themes/base/jquery.ui.accordion.css",
                  "~/Content/themes/base/jquery.ui.autocomplete.css",
                  "~/Content/themes/base/jquery.ui.button.css",
                  "~/Content/themes/base/jquery.ui.dialog.css",
                  "~/Content/themes/base/jquery.ui.slider.css",
                  "~/Content/themes/base/jquery.ui.tabs.css",
                  "~/Content/themes/base/jquery.ui.datepicker.css",
                  "~/Content/themes/base/jquery.ui.progressbar.css",
                  "~/Content/themes/base/jquery.ui.theme.css"));

      bundles.Add(new ScriptBundle("~/bundles/MobileJS").Include(
        "~/Scripts/jquery.mobile-1.*", 
        "~/Scripts/jquery-2.*",
        "~/Scripts/jquery.slicknav.js"));

      bundles.Add(new StyleBundle("~/Content/MobileCSS").Include(
        "~/Content/SiteMobile.css",
        "~/Content/jquery-mobile-1.3.1.css", 
        "~/Content/jquery.mobile.structure-1.3.1.min.css",
        "~/Content/slicknav.css"
        ));  
    }
  }
}