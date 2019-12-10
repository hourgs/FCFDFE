using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.D
{
    public partial class QueryCompany : System.Web.UI.Page
    {
        public string NAME, CST, ADDRESS;
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string[] strField = { "OVC_PURCH","OVC_PURCH_5","OVC_PURCH_6", "OVC_VEN_CST", "OVC_VEN_TITLE", "OVC_VEN_TEL", "OVC_VEN_ADDRESS"};
        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getQueryString(this, "NAME", out NAME, false);
            FCommon.getQueryString(this, "CST", out CST, false);
            FCommon.getQueryString(this, "ADDRESS", out ADDRESS, false);
            if (!IsPostBack)
            {
                dataImport();
            }
        }
        private void dataImport()
        {
            string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
            string strOVC_PURCH_5_url = Request.QueryString["OVC_PURCH_5"];
            //string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
            //string strOVC_PURCH_5 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_5_url));
 
            DataTable dt = new DataTable();

            var query =
                from tb1302 in mpms.TBM1302
                where tb1302.OVC_PURCH == strOVC_PURCH_url
                where tb1302.OVC_PURCH_5 == strOVC_PURCH_5_url
                select new
                {
                    OVC_PURCH = tb1302.OVC_PURCH,
                    OVC_PURCH_6 = tb1302.OVC_PURCH_6,
                    OVC_PURCH_5 = tb1302.OVC_PURCH_5,
                    OVC_VEN_CST = tb1302.OVC_VEN_CST,
                    OVC_VEN_TITLE = tb1302.OVC_VEN_TITLE,
                    OVC_VEN_TEL = tb1302.OVC_VEN_TEL,
                    OVC_ADDRESS = tb1302.OVC_VEN_ADDRESS

                };
            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_1302, dt, strField);
        }

        protected void GVTBGMT_1302_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnSelect = (Button)e.Row.FindControl("btnSelect");
                string strVenTitle = e.Row.Cells[3].Text.Replace("&nbsp;", "");
                string strVenCst = e.Row.Cells[2].Text.Replace("&nbsp;", "");
                string strAddress = e.Row.Cells[5].Text.Replace("&nbsp;", "");
                btnSelect.Attributes["onclick"] = "javascript: return reval2('" + strVenTitle + "', '" + strVenCst + "', '" + strAddress + "')";
            }
        }

        protected void GVTBGMT_1302_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
       

    }
}