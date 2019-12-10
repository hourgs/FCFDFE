using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;

namespace FCFDFE
{
    public partial class logout : System.Web.UI.Page
    {
        //private static readonly ILog TxtLog = LogManager.GetLogger(typeof(_Default));//取得logger物件
        private GMEntities gm = new GMEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["logid"] != null)
            {
                string ID = Session["logid"].ToString();
            Guid g = new Guid(ID);
            var getlog = gm.USER_LOGIN.Where(sn => sn.AC_SN == g).FirstOrDefault();

            getlog.LOGOUT_TIME = DateTime.Now;
            gm.SaveChanges();
            //XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("log4net.config")));
            //TxtLog.Info(id + "登出");
            Session.Clear();
            Response.Redirect("~/login.aspx");
            }
            else
                Response.Redirect("~/login.aspx");
        }
    }
}