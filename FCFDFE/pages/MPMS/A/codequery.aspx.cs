using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;

namespace FCFDFE.pages.MPMS.A
{
    public partial class codequery : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            Session["codequery"] = "query1";
        }
    }
}