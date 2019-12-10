using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Entity.GMModel;
using System.Linq;

namespace FCFDFE.pages.MPMS.C
{
    public partial class MPMS_C1B : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        MPMSEntities mpms = new MPMSEntities();
        GMEntities gm = new GMEntities();
        Common FCommon = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    list_dataImport();
                    GVDataImport(drpYEAR.SelectedValue);
                }
            }
            
        }

        protected void GV_CASE_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GV_CASE.UseAccessibleHeader = true;
                GV_CASE.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private string getTaiwanDate(string strDate,string format)
        {
            if (DateTime.TryParse(strDate, out DateTime datetime))
            {
                CultureInfo culture = new CultureInfo("zh-TW");
                culture.DateTimeFormat.Calendar = new TaiwanCalendar();
                return datetime.ToString(format, culture);
            }
            else
            {

                return "";
            }

        }

        private void GVDataImport(string year)
        {
            string[] field = { "OVC_DRESULT", "OVC_PURCH", "OVC_PUR_IPURCH", "OVC_PUR_NSECTION"
                                , "ONB_CHECK_TIMES","OVC_DAUDIT_ASSIGN","OVC_DAUDIT" };
            string userName = "", userID = "";
            string yearSub = year.Substring((year.Length - 2), 2);
            if(Session["userid"] != null && Session["username"]!= null)
            {
                userName = Session["username"].ToString();
                userID = Session["userid"].ToString();
            }
            
            string queryDept = gm.ACCOUNTs.Where(o => o.USER_ID.Equals(userID)).Select(o=>o.DEPT_SN).FirstOrDefault();
            var query =
                from t1202 in mpms.TBM1202
                join t12021 in mpms.TBM1202_1 on new { t1202.OVC_PURCH, t1202.ONB_CHECK_TIMES } equals new { t12021.OVC_PURCH,t12021.ONB_CHECK_TIMES}
                join t1301 in mpms.TBM1301 on t1202.OVC_PURCH equals t1301.OVC_PURCH
                where t1202.OVC_PURCH.Substring(2, 2).Equals(yearSub) 
                    && t1202.OVC_CHECK_UNIT.Equals(queryDept) && t12021.OVC_AUDITOR.Equals(userName)
                select new
                {
                    t1202.OVC_DRESULT,
                    t12021.OVC_PURCH,
                    t1301.OVC_PUR_IPURCH,
                    t1301.OVC_PUR_NSECTION,
                    t12021.ONB_CHECK_TIMES,
                    t12021.OVC_DAUDIT_ASSIGN,
                    t12021.OVC_DAUDIT
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            foreach(DataRow row in dt.Rows)
            {
                row["OVC_DRESULT"] = getTaiwanDate(row["OVC_DRESULT"].ToString(), "yyy年MM月dd日");
                row["OVC_DAUDIT_ASSIGN"] = getTaiwanDate(row["OVC_DAUDIT_ASSIGN"].ToString(), "yyy年MM月dd日 HH:mm");
                row["OVC_DAUDIT"] = getTaiwanDate(row["OVC_DAUDIT"].ToString(), "yyy年MM月dd日 HH:mm");
            }
            hasRows = FCommon.GridView_dataImport(GV_CASE,dt, field);
        }
        
        private void list_dataImport()
        {
            //帶入計畫年度下拉選單的值
            //先將下拉式選單清空
            drpYEAR.Items.Clear();
            //取得台灣年月日
            DateTime datetime = DateTime.Now;
            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            int CalDateYear = Convert.ToInt16(datetime.ToString("yyy", culture));
            int num = CalDateYear;
            for (int i = 0; i < 15; i++)
            {
                drpYEAR.Items.Add(Convert.ToString(num));
                num = num - 1;
            }
        }
        
        protected void btnYearQuery_Click(object sender, EventArgs e)
        {
            GVDataImport(drpYEAR.SelectedValue);
        }

        protected void GV_CASE_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = 0;
            GridViewRow row;
            string strPurchNum = "";
            string strCheckTimes = "";
            GridView grid = sender as GridView;
            if (e.CommandName.Equals("btnSelect"))
            {
                index = Convert.ToInt32(e.CommandArgument);
                row = grid.Rows[index];
                strPurchNum = row.Cells[1].Text;
                strCheckTimes = row.Cells[4].Text;
                string url = "~/pages/MPMS/C/MPMS_C17.aspx?PurchNum=" + strPurchNum + "&numCheckTimes=" + strCheckTimes + "&page=C1B";
                Response.Redirect(url);
            }
        }

        protected void GV_CASE_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbl = (Label)e.Row.FindControl("lblOVC_DRESULT");
                Label lblTitle = (Label)e.Row.FindControl("lblDRESULT_TITELE");
                Button btn = (Button)e.Row.FindControl("btnSelect");
                if (!string.IsNullOrEmpty(lbl.Text))
                {
                    lbl.Visible = true;
                    lblTitle.Visible = true;
                    btn.Visible = false;
                }
            }

        }

    }
}