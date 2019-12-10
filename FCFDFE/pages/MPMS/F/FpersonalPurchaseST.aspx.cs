using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Data;
using System.Web.UI;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;
using System.IO;

namespace FCFDFE.pages.MPMS.F
{
    public partial class FpersonalPurchaseST : Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strDateFormat = Variable.strDateFormat;
        public string strMenuName = "", strMenuNameItem = "";
        public string strDEPT_SN, strDEPT_Name;

        #region 副程式
        private void list_dataImport()
        {
            FCommon.list_dataImportYear(drpOVC_BUDGET_YEAR);
            if (!string.IsNullOrEmpty(Request.QueryString["year"]))
            {
                string strYear;
                if (FCommon.getQueryString(this, "year", out strYear, true))
                    FCommon.list_setValue(drpOVC_BUDGET_YEAR, strYear);
            }
        }
        private void dataImport()
        {
            //顯示所有人的承辦案件數量
            string strYear = drpOVC_BUDGET_YEAR.SelectedValue;
            ViewState["year"] = strYear;
            strYear = strYear.Substring((strYear.Length - 2), 2); //取得年度末兩碼
            if (Session["userid"] != null)
            {
                //查詢今年案數
                var queryCaseGroup =
                from t in mpms.TBM1202
                join t1301P in mpms.TBM1301_PLAN on t.OVC_PURCH equals t1301P.OVC_PURCH
                where t.OVC_CHECK_UNIT.Equals(strDEPT_SN) && t1301P.OVC_AUDIT_UNIT.Equals(strDEPT_SN)
                        && t.OVC_PURCH.Substring(2, 2).Equals(strYear)
                group t by new { t.OVC_PURCH, t.OVC_CHECKER } into g
                select new
                {
                    g.Key.OVC_PURCH,
                    g.Key.OVC_CHECKER
                };
                var queryCaseCount =
                    from t in queryCaseGroup
                    group t by t.OVC_CHECKER into g
                    select new
                    {
                        Checker = g.Key,
                        AllCount = g.Count()
                    };
                DataTable dt = CommonStatic.LinqQueryToDataTable(queryCaseCount);
                dt.Columns.Add("No");
                dt.Columns.Add("AllCountText");
                int intAllTotal = 0; //計算合計數量
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    dr["No"] = i + 1;
                    string strOVC_NAME = dr["Checker"].ToString();
                    string strAllCount = dr["AllCount"].ToString();
                    int intAllCount = int.Parse(strAllCount);
                    intAllTotal += intAllCount;
                }
                bool hasRows = FCommon.GridView_dataImport(GV_CASE, dt);
                if (hasRows)
                {
                    string strAllCount = intAllTotal.ToString();
                    //加上超連結
                    string strQueryString = "";
                    FCommon.setQueryString(ref strQueryString, "year", ViewState["year"].ToString(), true);
                    FCommon.setQueryString(ref strQueryString, "ovcName", "All", false);
                    string strAllCountText = $"<a href='FpersonalPurchaselist{ strQueryString }'>{ strAllCount }</a>"; //超連結過去會沒資料
                    Label lblAllCount = new Label();
                    lblAllCount.Text = strAllCountText;

                    GridViewRow grvFooter = GV_CASE.FooterRow;
                    grvFooter.Cells[0].Text = "合計";
                    grvFooter.Cells[0].CssClass += " text-center";
                    grvFooter.Cells[0].ColumnSpan = 2;
                    grvFooter.Cells[1].Controls.Add(lblAllCount);
                    grvFooter.Cells.RemoveAt(2);
                }
                ViewState["hasRows"] = hasRows;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getAccountDEPTName(this, out strDEPT_SN, out strDEPT_Name);
                if (!IsPostBack)
                {
                    list_dataImport();
                    dataImport();
                }
            }
        }

        protected void btnQuery_OVC_BUDGET_Click(object sender, EventArgs e)
        {
            dataImport();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            string strQueryString = "";
            if (ViewState["year"] != null)
            {
                string strYear = ViewState["year"].ToString();
                FCommon.setQueryString(ref strQueryString, "yearPerPur", strYear, true);
            }
            Response.Redirect($"FPlanAssessmentSA{ strQueryString }");
        }

        protected void GV_CASE_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvRow = e.Row;
            string strYear = ViewState["year"].ToString();
            if (gvRow.RowType == DataControlRowType.DataRow)
            {
                //重複之購案顯示不同 審查次數、分派日、回覆日、作業天數
                Label lblChecker = (Label)gvRow.FindControl("lblChecker"); //承辦人姓名
                Label lblAllCount = (Label)gvRow.FindControl("lblAllCount"); //承辦案數
                if (ViewState["year"]!=null && FCommon.Controls_isExist(lblChecker, lblAllCount)) //審查次數
                {
                    string strOVC_NAME = lblChecker.Text;
                    string strAllCount = lblAllCount.Text;
                    //加上超連結
                    string strQueryString = "";
                    FCommon.setQueryString(ref strQueryString, "year", strYear, true);
                    FCommon.setQueryString(ref strQueryString, "ovcName", strOVC_NAME, false);
                    string strAllCountText = $"<a href='FpersonalPurchaselist{ strQueryString }'>{ strAllCount }</a>";

                    lblAllCount.Text = strAllCountText;
                }
            }
        }
        protected void GV_CASE_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
            {
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            }
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}