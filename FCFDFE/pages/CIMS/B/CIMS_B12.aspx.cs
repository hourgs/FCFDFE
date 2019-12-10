using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.CIMSModel;
using FCFDFE.Entity.GMModel;
using System.Data.Entity;
using System.IO;

namespace FCFDFE.pages.CIMS.B
{
    public partial class CIMS_B12 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        CIMSEntities CIMS = new CIMSEntities();
        GMEntities GM = new GMEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string[] strField = { "OVC_PURCH", "OVC_PUR_IPURCH", "OVC_PUR_USER", "OVC_DPROPOSE", "OVC_PUR_SECTION", "OVC_PUR_NSECTION" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txt_OVC_DPROPOSE_SDATE, txt_OVC_DPROPOSE_EDATE);
                }
            }
        }

        protected void GV_TBM1301_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void GV_TBM1301_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBM1301.DataKeys[gvrIndex].Value.ToString();

            switch (e.CommandName)
            {
                case "Check":
                    Response.Write("<script language='javascript'>window.open('CIMS_B11_1.aspx?id=" + id + "');</script>");
                    break;
                default:
                    break;


            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            showData();
            Detail.Visible = true;
        }

        protected void showData()
        {
            DataTable dt = new DataTable();
            DateTime dateValue;
            var query =
                from TBM1301 in GM.TBM1301.DefaultIfEmpty().AsEnumerable()
                select new
                {
                    OVC_PURCH = TBM1301.OVC_PURCH,
                    OVC_PUR_IPURCH=TBM1301.OVC_PUR_IPURCH==null?"": TBM1301.OVC_PUR_IPURCH,
                    OVC_PUR_USER=TBM1301.OVC_PUR_USER==null?"":TBM1301.OVC_PUR_USER,
                    OVC_DPROPOSE = TBM1301.OVC_DPROPOSE == null ? "" : TBM1301.OVC_DPROPOSE,
                    OVC_PUR_SECTION = TBM1301.OVC_PUR_SECTION == null ? "" : TBM1301.OVC_PUR_SECTION,
                    OVC_PUR_NSECTION = TBM1301.OVC_PUR_NSECTION == null ? "" : TBM1301.OVC_PUR_NSECTION
                };

            if (txtOVC_PURCH.Text != "")
                query = query.Where(table => table.OVC_PURCH.Contains(txtOVC_PURCH.Text));
            if (txtOVC_PUR_IPURCH.Text != "")
                query = query.Where(table => table.OVC_PUR_IPURCH.Contains(txtOVC_PUR_IPURCH.Text));
            if (txtOVC_PUR_USER.Text !="")
                query = query.Where(table => table.OVC_PUR_USER.Contains(txtOVC_PUR_USER.Text));
            if (txtOVC_PUR_NSECTION.Text !="")
                query = query.Where(table => table.OVC_PUR_NSECTION.Contains(txtOVC_PUR_NSECTION.Text));
            if (txt_OVC_DPROPOSE_SDATE.Text != "")
            {
                query = query.Where(table => DateTime.TryParse(table.OVC_DPROPOSE, out dateValue) == true);
                query = query.Where(table => DateTime.Compare(Convert.ToDateTime(table.OVC_DPROPOSE), Convert.ToDateTime(txt_OVC_DPROPOSE_SDATE.Text)) >= 0);
            }
            if (txt_OVC_DPROPOSE_EDATE.Text != "")
            {
                query = query.Where(table => DateTime.TryParse(table.OVC_DPROPOSE, out dateValue) == true);
                query = query.Where(table => DateTime.Compare(Convert.ToDateTime(table.OVC_DPROPOSE), Convert.ToDateTime(txt_OVC_DPROPOSE_EDATE.Text)) <= 0);
            }



            dt = CommonStatic.LinqQueryToDataTable(query);

            DataColumn column = new DataColumn();
            column.ColumnName = "RANK";
            column.DataType = System.Type.GetType("System.Int32");
            dt.Columns.Add(column);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Rank"] = i + 1;
            }
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBM1301, dt, strField);

        }
    }
}