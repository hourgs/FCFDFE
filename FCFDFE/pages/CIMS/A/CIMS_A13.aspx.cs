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
    public partial class CIMS_A13 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        CIMSEntities CIMS = new CIMSEntities();
        string[] strField = { "REGNO", "VEN_NAME", "VEN_NAME_T", "AGENT_ITEM", "AbleTime" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtAUTH_DATE_S_query, txtAUTH_DATE_E_query, txtREGNO, txtAUTH_DATE_S, txtAUTH_DATE_E, txtAPPR_DATE_S, txtAPPR_DATE_E);
                }
            }
        }

        protected void GV_VENAGENT_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_VENAGENT.DataKeys[gvrIndex].Value.ToString();

            switch (e.CommandName)
            {
                case "Detail":
                    condition.Visible = false;
                    showdetail(id);
                    break;
                default:
                    break;


            }

        }

        protected void GV_VENAGENT_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            querytable.Visible = true;
            dataimport();
        }

        private void dataimport()
        {
            DataTable dt = new DataTable();
            var query =
                from VENAGENT in CIMS.VENAGENT.DefaultIfEmpty().AsEnumerable()
                select new
                {
                    REGNO = VENAGENT.REGNO == null ? "" : VENAGENT.REGNO,
                    VEN_NAME = VENAGENT.VEN_NAME == null ? "" : VENAGENT.VEN_NAME,
                    VEN_NAME_T = VENAGENT.VEN_NAME_T == null ? "" : VENAGENT.VEN_NAME_T,
                    AGENT_ITEM = VENAGENT.AGENT_ITEM == null ? "" : VENAGENT.AGENT_ITEM,
                    AUTH_DATE_S=VENAGENT.AUTH_DATE_S==null?"":VENAGENT.AUTH_DATE_S.ToString(),
                    AUTH_DATE_E=VENAGENT.AUTH_DATE_E==null?"": VENAGENT.AUTH_DATE_E,
                    AbleTime = VENAGENT.AUTH_DATE_S==null|| VENAGENT.AUTH_DATE_E == null?"": Convert.ToDateTime(VENAGENT.AUTH_DATE_S).ToString(Variable.strDateFormat) + "\n~\n" + Convert.ToDateTime(VENAGENT.AUTH_DATE_E).ToString(Variable.strDateFormat)
                };

            if (txtREGNO_query.Text != "")
                query = query.Where(table => table.REGNO.Contains(txtREGNO_query.Text));
            if(txtVEN_NAME_query.Text!="")
                query = query.Where(table => table.VEN_NAME.Contains(txtVEN_NAME_query.Text));
            if (txtVEN_NAME_T_query.Text != "")
                query = query.Where(table => table.VEN_NAME_T.Contains(txtVEN_NAME_T_query.Text));
            if (txtAGENT_ITEM_query.Text != "")
                query = query.Where(table => table.AGENT_ITEM.Contains(txtAGENT_ITEM_query.Text));
            if (txtAUTH_DATE_S_query.Text != "")
            {
                query = query.Where(table => !table.AUTH_DATE_S.Equals(""));
                query = query.Where(table => DateTime.Compare(Convert.ToDateTime(table.AUTH_DATE_S), Convert.ToDateTime(txtAUTH_DATE_S_query.Text)) >= 0);
            }
            if (txtAUTH_DATE_E_query.Text != "")
            {
                query = query.Where(table => !table.AUTH_DATE_E.Equals(""));
                query = query.Where(table => DateTime.Compare(Convert.ToDateTime(table.AUTH_DATE_E), Convert.ToDateTime(txtAUTH_DATE_E_query.Text)) <= 0);
            }

            dt = CommonStatic.LinqQueryToDataTable(query);
             ViewState["hasRows"] = FCommon.GridView_dataImport(GV_VENAGENT, dt, strField);
        }


        protected void btnReport_Click(object sender, EventArgs e)
        {
            string REGNO = txtREGNO.Text;
            Response.Write("<script language='javascript'>window.open('CIMS_A12_Report.aspx?id=" + REGNO + "');</script>");

            //Response.Redirect("CIMS_A12_Report.aspx?id=" + REGNO);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            dataimport();
            DataDetail.Visible = false;
            condition.Visible = true;
            querytable.Visible = true;
        }

        private void showdetail(string id)
        {
            condition.Visible = false;
            querytable.Visible = false;
            DataDetail.Visible = true;

            var query=
                (from VENAGENT in CIMS.VENAGENT
                where VENAGENT.REGNO.Equals(id)
                select VENAGENT).FirstOrDefault();
            if (query != null)
            {
                txtREGNO.Text=query.REGNO;
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
                txtOVC_MEMO.Text = query.OVC_MEMO;
            }

        }






    }
}