using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.CIMSModel;
using System.Data.Entity;


namespace FCFDFE.pages.CIMS.H
{
    public partial class CIMS_H10 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        CIMSEntities CIMS = new CIMSEntities();

        protected void B13_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/pages/CIMS/B/CIMS_B13.aspx");
        }

        protected void C11_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/pages/CIMS/C/CIMS_C11.aspx");
        }

        protected void F11_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/pages/CIMS/F/CIMS_F11.aspx");
        }

        protected void D11_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/pages/CIMS/D/CIMS_D11.aspx");
        }

        protected void D12_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/pages/CIMS/D/CIMS_D12.aspx");
        }

        protected void H11_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/pages/CIMS/H/CIMS_H11.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                int a = 1;
            }
                
        }

        
    }
}