using System;
using System.Web;
using System.Web.Optimization;

namespace koperasi.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery")
                    .Include("~/Scripts/jquery-3.1.1.js")
                    .Include("~/Scripts/jquery-ui-1.12.1.min.js"));

            bundles.Add(new StyleBundle("~/content/css")
                    .Include("~/Content/themes/base/theme.css")
                    .Include("~/Content/themes/base/core.css")
                    .Include("~/Content/themes/base/base.css")
                    .Include("~/Content/themes/base/dialog.css")
                    .Include("~/Content/themes/base/jquery-ui.min.css"));
        }
    }
}