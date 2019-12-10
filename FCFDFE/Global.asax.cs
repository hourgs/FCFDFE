using FCFDFE.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace FCFDFE
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 應用程式啟動時執行的程式碼
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    if (Server.GetLastError().GetType().ToString() == "System.Web.HttpRequestValidationException")
        //    {
        //        Session["XSSRequest"] = "danger";
        //    }
        //    Server.ClearError();
        //    Response.Write("<Script language='JavaScript'>window.history.go(-1);</Script>");
        //}
    }
}