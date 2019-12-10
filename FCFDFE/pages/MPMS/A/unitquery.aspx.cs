using FCFDFE.Content;
using System;

namespace FCFDFE.pages.MPMS.A
{
    public partial class unitquery : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                FCommon.Controls_Attributes("readonly", "ture", txtOVC_DEPT_CDE, txtOVC_ONNAME);    //新增唯讀屬性
            }
        }

        protected void btnAugent_Click(object sender, EventArgs e)
        {
            Session["unitquery"] = "query5";
        }

        protected void btnPurchase_Click(object sender, EventArgs e)
        {
            Session["unitquery"] = "query3";
        }

        protected void btnContract_Click(object sender, EventArgs e)
        {
            Session["unitquery"] = "query4";
        }

        protected void btnAudit_Click(object sender, EventArgs e)
        {
            Session["unitquery"] = "query2";
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            Session["unitquery"] = "query1";
        }
    }
}