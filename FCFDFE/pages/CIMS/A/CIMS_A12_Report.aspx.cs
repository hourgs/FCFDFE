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


namespace FCFDFE.pages.CIMS.A
{
    public partial class CIMS_A12_Report : System.Web.UI.Page
    {
        CIMSEntities CIMS = new CIMSEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"]==null)
            {
                Response.Write("<script>alert('系統檢測到您未依照正確方式進入此頁面，將導至登入畫面!'); location.href='../../../logout.aspx'; </script>");
                return;
            }
            if (!IsPostBack)
            {
                string id = "";
                id = Request.QueryString["id"].ToString();
                showdata(id);
            }
        }


        private void showdata(string id)
        {

            var query =
                (from VENAGENT in CIMS.VENAGENT
                 where VENAGENT.REGNO.Equals(id)
                 select VENAGENT).FirstOrDefault();
            if (query != null)
            {
                txtREGNO.Text = query.REGNO;
                txtVEN_NAME.Text = query.VEN_NAME;
                txtVEN_DEPT.Text = query.VEN_DEPT;
                txtVEN_CODE.Text = query.VEN_CODE;
                txtVEN_ADDR.Text = query.VEN_ADDR;
                txtVEN_TEL.Text = query.VEN_TEL;
                txtVEN_FAX.Text = query.VEN_FAX;
                txtVEN_BOSS.Text = query.VEN_BOSS;
                txtVEN_NAME_T.Text = query.VEN_NAME_T;
                txtVEN_ENAME_T.Text = query.VEN_ENAME_T;
                txtVEN_CODE_T.Text = query.VEN_CODE_T;
                txtVEN_ADDR_T.Text = query.VEN_ADDR_T;
                txtVEN_TEL_T.Text = query.VEN_TEL_T;
                txtVEN_FAX_T.Text = query.VEN_FAX_T;
                txtVEN_BOSS_T.Text = query.VEN_BOSS_T;
                txtAUTH_DATE_S.Text = query.AUTH_DATE_S.ToString();
                txtAUTH_DATE_E.Text = query.AUTH_DATE_E;
                txtAUTH_RANGE.Text = query.AUTH_RANGE;
                txtAGENT_ITEM.Text = query.AGENT_ITEM;
                txtAPPR_DATE_S.Text = query.APPR_DATE_S;
                txtAPPR_DATE_E.Text = query.APPR_DATE_E;

            }
        }
    }

    
}