using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using FCFDFE.Content;

namespace FCFDFE.pages.CIMS.H
{
    public partial class CIMS_H11 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";

        protected void btn_Tax_inf1_Click(object sender, EventArgs e)
        {
            string text_long;
            text_long = "CIMS_H11_2.aspx?text_long=200";
            Response.Redirect(text_long);
        }

        protected void btn_Tax_inf2_Click(object sender, EventArgs e)
        {
            string text_long;
            text_long = "CIMS_H11_2.aspx?text_long=92";
            Response.Redirect(text_long);
        }

        protected void btn_CHI_NAME_Click(object sender, EventArgs e)
        {
            Response.Redirect("CIMS_H11_1.aspx");
        }

        protected void btn_ENG_NAME_Click(object sender, EventArgs e)
        {
            Response.Redirect("CIMS_H11_1.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.UrlReferrer.ToString().Contains("H10"))
            {
                Response.Write("<script>alert('系統檢測到您未依照正確方式進入此頁面，將導至登入畫面!'); location.href='../../../logout.aspx'; </script>");
                return;
            }
        }
        
    }
}