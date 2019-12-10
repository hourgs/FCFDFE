using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;
using System.Collections;
using FCFDFE.Content;

namespace FCFDFE
{
    
    public partial class SuperMaster : MasterPage
    {
        //public string theSystem;
        Common FCommon = new Common();
        private GMEntities GME = new GMEntities();
        public string strSYSTEM = "";

        public void MessageBoxShow(string strMessage, string strUrl, bool isRootDirectory)
        {
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Message Box", "<script language = 'javascript'>alert('" + msg + "');location.href='../../../../login.aspx';</script>");

            if (isRootDirectory)
                strUrl = Page.ResolveClientUrl("~/" + strUrl);
            string strScript =
                "<script language='javascript'>" +
                    "alert('" + strMessage + "');" +
                    "location.href='" + strUrl + "';" +
                "</script>";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "MessageBox", strScript);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Superadmin"] == null)
                Response.Redirect("~/logout.aspx");
        }



        protected void Timer1_Tick(object sender, EventArgs e)
        {
            if(Session["logid"]!= null)
            {
                string ID = Session["logid"].ToString();
                Guid g = new Guid(ID);
                var getlog = GME.USER_LOGIN.Where(sn => sn.AC_SN == g).FirstOrDefault();

                getlog.LOGOUT_TIME = DateTime.Now;
                GME.SaveChanges();
            }

            Session.Clear();
            MessageBoxShow("登入逾時，請重新登入", "login", true);
        }
    }
}