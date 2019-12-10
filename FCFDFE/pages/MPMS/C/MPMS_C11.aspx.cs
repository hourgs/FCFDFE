using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System.Linq;

namespace FCFDFE.pages.MPMS.C
{
    public partial class MPMS_C11 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        MPMSEntities mpms = new MPMSEntities();
        Common FCommon = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {  
                    list_dataImport(drpYEAR);
                    QueryUndo(drpYEAR.SelectedItem.Text);
                }
            }
        }

        protected void GV_NOT_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {

                GV_NOT.UseAccessibleHeader = true;
                GV_NOT.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        #region onClick
        protected void btnYearQuery_Click(object sender, EventArgs e)
        {
            QueryUndo(drpYEAR.SelectedItem.Text);
        }

        protected void btnModify_Click(object sender, EventArgs e)
        {
            Button btnThis = (Button)sender;
            int gvRowIndex = (btnThis.NamingContainer as GridViewRow).RowIndex;
            HiddenField hidOVC_PURCH = (HiddenField)GV_NOT.Rows[gvRowIndex].Cells[6].FindControl("hidOVC_PURCH");
            string strPurchNum = hidOVC_PURCH.Value;
            string url = "~/pages/MPMS/C/MPMS_C12.aspx?PurchNum=" + strPurchNum;
            Response.Redirect(url);
        }
        #endregion

        #region 副程式

        private void QueryUndo(string year)
        {
            string[] field = { "OVC_PURCH", "OVC_PUR_IPURCH", "OVC_PUR_NSECTION", "OVC_PUR_USER",
                                "OVC_DPROPOSE", "OVC_PROPOSE","OVC_PURCH_ORG" };
            string yearSub = year.Substring((year.Length - 2), 2);
            string userid = Session["userid"].ToString();
            var queryDept = mpms.ACCOUNT.Where(t => t.USER_ID.Equals(userid)).Select(t => t.DEPT_SN).FirstOrDefault();
            //1301有1202沒有
            var query =
                (from t1301 in mpms.TBM1301
                where t1301.OVC_PURCH.Substring(2, 2).Equals(yearSub) 
                && t1301.OVC_PERMISSION_UPDATE.Equals("N")
                && t1301.OVC_DPROPOSE != null
                where queryDept == "0A100" || queryDept == "00N00" ? t1301.OVC_DOING_UNIT.Equals("0A100") || t1301.OVC_DOING_UNIT.Equals("00N00") : t1301.OVC_DOING_UNIT.Equals(queryDept)
                 select t1301.OVC_PURCH)
                .Except
                (from t1202 in mpms.TBM1202
                 where t1202.OVC_PURCH.Substring(2, 2).Equals(yearSub)
                 select t1202.OVC_PURCH);
            //資料傳送日
            var queryDateT =
                from t in mpms.TBM1114.Where(o => o.OVC_PURCH.Substring(2, 2).Equals(yearSub) && o.OVC_REMARK.Contains("申購單位將購案轉呈"))
                group t by t.OVC_PURCH into g
                select new
                {
                    g.Key,
                    Date = g.Max(t => t.OVC_DATE)
                };
            var queryList =
                from t in query
                join t1301 in mpms.TBM1301 on t equals t1301.OVC_PURCH
                join t1114 in queryDateT on t equals t1114.Key
                select new
                {
                    OVC_PURCH_ORG = t1301.OVC_PURCH,
                    t1301.OVC_PURCH,
                    t1301.OVC_PUR_AGENCY,
                    t1301.OVC_PUR_IPURCH,
                    t1301.OVC_PUR_NSECTION,
                    t1301.OVC_PUR_USER,
                    t1301.OVC_PUR_IUSER_PHONE_EXT,
                    t1301.OVC_DPROPOSE,
                    DateT = t1114.Date,
                    t1301.OVC_PROPOSE,
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(queryList);
            

            foreach(DataRow row in dt.Rows)
            {
                row["OVC_PURCH"] = row["OVC_PURCH"].ToString() + row["OVC_PUR_AGENCY"].ToString();
                row["OVC_PUR_USER"] = row["OVC_PUR_USER"].ToString() +"("+ row["OVC_PUR_IUSER_PHONE_EXT"].ToString()+")";
                row["OVC_DPROPOSE"] = getTaiwanDate(row["OVC_DPROPOSE"].ToString()) + "<br/>" + row["DateT"].ToString();
            }
            
            hasRows = FCommon.GridView_dataImport(GV_NOT, dt, field);
        }

        private void list_dataImport(ListControl list)
        {
            //帶入計畫年度下拉選單的值
            //先將下拉式選單清空
            list.Items.Clear();

            //取得台灣年月日
            DateTime datetime = DateTime.Now;
            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            int CalDateYear = Convert.ToInt16(datetime.ToString("yyy", culture));
            int num = CalDateYear;
            for (int i = 0; i < 15; i++)
            {
                list.Items.Add(Convert.ToString(num));
                num = num - 1;
            }
        }
        

        private string getTaiwanDate(string strDate)
        {
            if (DateTime.TryParse(strDate, out DateTime datetime))
            {
                CultureInfo culture = new CultureInfo("zh-TW");
                culture.DateTimeFormat.Calendar = new TaiwanCalendar();
                return datetime.ToString("yyy年MM月dd日", culture);
            }
            else
            {
                return "";
            }
        }
        #endregion
    }
}