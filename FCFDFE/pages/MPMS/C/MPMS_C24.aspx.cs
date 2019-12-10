using System;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Entity.GMModel;
using System.Linq;
using FCFDFE.Content;
using System.Globalization;

namespace FCFDFE.pages.MPMS.C
{
    public partial class MPMS_C24 : System.Web.UI.Page
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
                    list_dataImport(drpOVC_BUDGET_YEAR);
                    GetUserInfo();
                }
            }
        }

        protected void GV_OVC_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GV_OVC.UseAccessibleHeader = true;
                GV_OVC.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void GV_OVC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = 0;
            GridViewRow row;
            GridView grid = sender as GridView;
            index = Convert.ToInt32(e.CommandArgument);
            row = grid.Rows[index];
            Label lblOVC_PURCH = row.FindControl("lblOVC_PURCH") as Label;
            string purchNum = lblOVC_PURCH.Text.Substring(0, 7);
            switch (e.CommandName)
            {
                case "btnClear":
                    ClearApprove(purchNum);
                    break;
                case "btnDo":
                    string url = "~/pages/MPMS/C/MPMS_C19.aspx?PurchNum=" + purchNum;
                    Response.Redirect(url);
                    break;
            }

        }

       
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            GV_DataImport("All");
        }

        protected void btnWait_Click(object sender, EventArgs e)
        {
            GV_DataImport("dapprove");
        }

        private void ClearApprove(string purchNum)
        {
            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(purchNum)).FirstOrDefault();
            if(tbm1301 != null)
            {
                tbm1301.OVC_PUR_DAPPROVE = "";
                tbm1301.OVC_PUR_APPROVE = "";
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1301.GetType().Name.ToString(), this, "修改");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "清除完成");
            }
            
        }


        private void GV_DataImport(string isAll)
        {
            string year = drpOVC_BUDGET_YEAR.SelectedValue;
            string yearSub = year.Substring((year.Length - 2), 2);
            string dept = ViewState["DEPT_SN"].ToString();
            string userName = ViewState["USER_NAME"].ToString();
            string[] field = { "OVC_PURCH", "OVC_PUR_AGENCY", "OVC_PUR_IPURCH", "OVC_PUR_NSECTION", "ONB_CHECK_TIMES", "OVC_DRECEIVE", "OVC_CHECK_OK" };
            if (isAll.Equals("All"))
            {
                if (dept.Equals("0A100") || dept.Equals("00N00"))
                {
                    var query =
                    from t1301 in mpms.TBM1301
                    join t1202R in mpms.TBM1202_RESULT on t1301.OVC_PURCH equals t1202R.OVC_PURCH
                    join t1301P in mpms.TBM1301_PLAN on t1301.OVC_PURCH equals t1301P.OVC_PURCH
                    where t1301.OVC_PURCH.Substring(2, 2).Equals(yearSub) && t1202R.OVC_CHECK_UNIT == dept
                        && t1202R.OVC_CHECKER == userName && (t1301P.OVC_AUDIT_UNIT == dept || t1301P.OVC_PUR_APPROVE_DEP == "A")
                        && t1301.OVC_DOING_UNIT == dept && !string.IsNullOrEmpty(t1202R.OVC_DRESULT)
                        && t1202R.OVC_CHECK_OK == "Y"
                    orderby t1202R.OVC_PURCH, t1202R.ONB_CHECK_TIMES
                    select new
                    {
                        t1301.OVC_PUR_AGENCY,
                        t1301.OVC_PUR_IPURCH,
                        t1301.OVC_PUR_NSECTION,
                        t1301.OVC_PUR_USER,
                        t1301.OVC_PUR_IUSER_PHONE_EXT,
                        t1301.OVC_DPROPOSE,
                        t1301.OVC_PROPOSE,
                        t1301.OVC_PUR_APPROVE,
                        t1301.OVC_PUR_DAPPROVE,
                        t1301.OVC_PUR_ALLOW,
                        t1202R.OVC_PURCH,
                        t1202R.OVC_DRECEIVE,
                        t1202R.OVC_CHECK_UNIT,
                        t1202R.ONB_CHECK_TIMES,
                        t1202R.OVC_DRESULT,
                        t1202R.OVC_CHECKER,
                        t1202R.OVC_CHECK_OK
                    };
                    DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                    foreach (DataRow row in dt.Rows)
                    {
                        row["OVC_DRECEIVE"] = GetTaiwanDate(row["OVC_DRECEIVE"].ToString());
                        row["OVC_DRESULT"] = GetTaiwanDate(row["OVC_DRESULT"].ToString());
                    }
                    hasRows = FCommon.GridView_dataImport(GV_OVC, dt, field);
                }
                else
                {
                    var query =
                    from t1301 in mpms.TBM1301
                    join t1202R in mpms.TBM1202_RESULT on t1301.OVC_PURCH equals t1202R.OVC_PURCH
                    join t1301P in mpms.TBM1301_PLAN on t1301.OVC_PURCH equals t1301P.OVC_PURCH
                    where t1301.OVC_PURCH.Substring(2, 2).Equals(yearSub) && t1202R.OVC_CHECK_UNIT == dept
                        && t1202R.OVC_CHECKER == userName
                        && t1301.OVC_DOING_UNIT == dept && !string.IsNullOrEmpty(t1202R.OVC_DRESULT)
                        && t1202R.OVC_CHECK_OK == "Y"
                    orderby t1202R.OVC_PURCH, t1202R.ONB_CHECK_TIMES
                    select new
                    {
                        t1301.OVC_PUR_AGENCY,
                        t1301.OVC_PUR_IPURCH,
                        t1301.OVC_PUR_NSECTION,
                        t1301.OVC_PUR_USER,
                        t1301.OVC_PUR_IUSER_PHONE_EXT,
                        t1301.OVC_DPROPOSE,
                        t1301.OVC_PROPOSE,
                        t1301.OVC_PUR_APPROVE,
                        t1301.OVC_PUR_DAPPROVE,
                        t1301.OVC_PUR_ALLOW,
                        t1202R.OVC_PURCH,
                        t1202R.OVC_DRECEIVE,
                        t1202R.OVC_CHECK_UNIT,
                        t1202R.ONB_CHECK_TIMES,
                        t1202R.OVC_DRESULT,
                        t1202R.OVC_CHECKER,
                        t1202R.OVC_CHECK_OK
                    };
                    DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                    foreach(DataRow row in dt.Rows)
                    {
                        row["OVC_DRECEIVE"] = GetTaiwanDate(row["OVC_DRECEIVE"].ToString());
                        row["OVC_DRESULT"] = GetTaiwanDate(row["OVC_DRESULT"].ToString());
                    }
                    hasRows = FCommon.GridView_dataImport(GV_OVC, dt, field);
                }
            }
            else if(isAll.Equals("dapprove"))
            {
                if (dept.Equals("0A100") || dept.Equals("00N00"))
                {
                    var query =
                    from t1301 in mpms.TBM1301
                    join t1202R in mpms.TBM1202_RESULT on t1301.OVC_PURCH equals t1202R.OVC_PURCH
                    join t1301P in mpms.TBM1301_PLAN on t1301.OVC_PURCH equals t1301P.OVC_PURCH
                    where t1301.OVC_PURCH.Substring(2, 2).Equals(yearSub) && t1202R.OVC_CHECK_UNIT == dept
                        && t1202R.OVC_CHECKER == userName && (t1301P.OVC_AUDIT_UNIT == dept || t1301P.OVC_PUR_APPROVE_DEP == "A")
                        && t1301.OVC_DOING_UNIT == dept && !string.IsNullOrEmpty(t1202R.OVC_DRESULT)
                        && t1202R.OVC_CHECK_OK == "Y" && t1301.OVC_PUR_DAPPROVE == null
                    orderby t1202R.OVC_PURCH, t1202R.ONB_CHECK_TIMES
                    select new
                    {
                        t1301.OVC_PUR_AGENCY,
                        t1301.OVC_PUR_IPURCH,
                        t1301.OVC_PUR_NSECTION,
                        t1301.OVC_PUR_USER,
                        t1301.OVC_PUR_IUSER_PHONE_EXT,
                        t1301.OVC_DPROPOSE,
                        t1301.OVC_PROPOSE,
                        t1301.OVC_PUR_APPROVE,
                        t1301.OVC_PUR_DAPPROVE,
                        t1301.OVC_PUR_ALLOW,
                        t1202R.OVC_PURCH,
                        t1202R.OVC_DRECEIVE,
                        t1202R.OVC_CHECK_UNIT,
                        t1202R.ONB_CHECK_TIMES,
                        t1202R.OVC_DRESULT,
                        t1202R.OVC_CHECKER,
                        t1202R.OVC_CHECK_OK
                    };
                    DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                    foreach (DataRow row in dt.Rows)
                    {
                        row["OVC_DRECEIVE"] = GetTaiwanDate(row["OVC_DRECEIVE"].ToString());
                        row["OVC_DRESULT"] = GetTaiwanDate(row["OVC_DRESULT"].ToString());
                    }
                    hasRows = FCommon.GridView_dataImport(GV_OVC, dt, field);
                }
                else
                {
                    var query =
                    from t1301 in mpms.TBM1301
                    join t1202R in mpms.TBM1202_RESULT on t1301.OVC_PURCH equals t1202R.OVC_PURCH
                    join t1301P in mpms.TBM1301_PLAN on t1301.OVC_PURCH equals t1301P.OVC_PURCH
                    where t1301.OVC_PURCH.Substring(2, 2).Equals(yearSub) && t1202R.OVC_CHECK_UNIT == dept
                        && t1202R.OVC_CHECKER == userName
                        && t1301.OVC_DOING_UNIT == dept && !string.IsNullOrEmpty(t1202R.OVC_DRESULT)
                        && t1202R.OVC_CHECK_OK == "Y" && t1301.OVC_PUR_DAPPROVE == null
                    orderby t1202R.OVC_PURCH, t1202R.ONB_CHECK_TIMES
                    select new
                    {
                        t1301.OVC_PUR_AGENCY,
                        t1301.OVC_PUR_IPURCH,
                        t1301.OVC_PUR_NSECTION,
                        t1301.OVC_PUR_USER,
                        t1301.OVC_PUR_IUSER_PHONE_EXT,
                        t1301.OVC_DPROPOSE,
                        t1301.OVC_PROPOSE,
                        t1301.OVC_PUR_APPROVE,
                        t1301.OVC_PUR_DAPPROVE,
                        t1301.OVC_PUR_ALLOW,
                        t1202R.OVC_PURCH,
                        t1202R.OVC_DRECEIVE,
                        t1202R.OVC_CHECK_UNIT,
                        t1202R.ONB_CHECK_TIMES,
                        t1202R.OVC_DRESULT,
                        t1202R.OVC_CHECKER,
                        t1202R.OVC_CHECK_OK
                    };
                    DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                    foreach (DataRow row in dt.Rows)
                    {
                        row["OVC_DRECEIVE"] = GetTaiwanDate(row["OVC_DRECEIVE"].ToString());
                        row["OVC_DRESULT"] = GetTaiwanDate(row["OVC_DRESULT"].ToString());
                    }
                    hasRows = FCommon.GridView_dataImport(GV_OVC, dt, field);
                }
            }
        }

        private void GetUserInfo()
        {
            if (Session["userid"] != null)
            {
                if (Session["userid"] != null)
                {
                    string userID = Session["userid"].ToString();
                    var userInfo = gm.ACCOUNTs.Where(o => o.USER_ID.Equals(userID)).FirstOrDefault();
                    ViewState["DEPT_SN"] = userInfo.DEPT_SN;
                    ViewState["USER_NAME"] = userInfo.USER_NAME;
                }
            }

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

        

        private string GetTaiwanDate(string strDate)
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
    }
}