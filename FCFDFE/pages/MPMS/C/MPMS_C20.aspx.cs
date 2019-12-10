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
    public partial class MPMS_C20 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        MPMSEntities mpms = new MPMSEntities();
        GMEntities gm = new GMEntities();
        Common FCommon = new Common();
        const string C_SN_AUTH = "3203";
        const string C_SN_ROLE = "31";
        const string OVC_PRIV_LEVEL = "7";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (!IsPostBack)
                {
                    checkerListImport(drpOVC_CHECKER);
                    list_dataImport();
                }
            }
        }
        #region 副程式
        private void GVDataImport(string year)
        {
            
            string[] field = { "OVC_PURCH", "OVC_PUR_IPURCH", "OVC_PUR_NSECTION",
                                    "ONB_CHECK_TIMES", "OVC_DRECEIVE", "OVC_DRESULT","OVC_CHECKER" };
            string yearSub = year.Substring((year.Length - 2), 2);
            string userName = Session["username"].ToString();
            string userID = Session["userid"].ToString();
            string queryDept = gm.ACCOUNTs.Where(o => o.USER_ID.Equals(userID)).Select(o => o.DEPT_SN).FirstOrDefault();
            var query =
                from t in mpms.TBM1301
                join t2 in mpms.TBM1202 on t.OVC_PURCH equals t2.OVC_PURCH
                where t2.OVC_PURCH.Substring(2, 2).Equals(yearSub) && t2.OVC_ASSIGNER.Equals(userName)
                        && t2.OVC_CHECK_UNIT.Equals(queryDept) && t2.OVC_CHECKER.Equals(drpOVC_CHECKER.SelectedItem.Text)
                select new
                {
                    t.OVC_PURCH,
                    t.OVC_PUR_IPURCH,
                    t.OVC_PUR_NSECTION,
                    t2.ONB_CHECK_TIMES,
                    t2.OVC_DRECEIVE,
                    t2.OVC_DRESULT,
                    t2.OVC_CHECKER
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            hasRows = FCommon.GridView_dataImport(GV_NOT, dt, field);
        }

        private void checkerListImport(ListControl list)
        {
            string userID = Session["userid"].ToString();
            string queryDept = gm.ACCOUNTs.Where(o => o.USER_ID.Equals(userID)).Select(o => o.DEPT_SN).FirstOrDefault();
            var queryqUnion =
                (from t in mpms.ACCOUNT_AUTH
                 join t2 in mpms.ACCOUNT on t.USER_ID equals t2.USER_ID
                 where t.C_SN_AUTH.Equals(C_SN_AUTH) && t2.DEPT_SN.Equals(queryDept)
                 select t2.USER_NAME)
                .Union
                (from t in mpms.TBM5200_1
                 join t2 in mpms.TBM5200_PPP on t.USER_ID equals t2.USER_ID
                 where t.C_SN_ROLE.Equals(C_SN_ROLE) && t.OVC_PRIV_LEVEL.Equals(OVC_PRIV_LEVEL) && t2.OVC_PUR_SECTION.Equals(queryDept)
                 select t2.USER_NAME);
            list.Items.Clear();
            foreach (var item in queryqUnion)
            {
                list.Items.Add(item);
            }
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

        private void SaveChangeChecker(string checker, string purchNum, string checkTimes)
        {
            string userID = Session["userid"].ToString();
            string queryDept = gm.ACCOUNTs.Where(o => o.USER_ID.Equals(userID)).Select(o => o.DEPT_SN).FirstOrDefault();
            byte numCheckTimes = Convert.ToByte(checkTimes);
            TBM1202 tbm1202 = new TBM1202();
            tbm1202 = mpms.TBM1202.Where(o => o.OVC_PURCH.Equals(purchNum) && o.OVC_CHECK_UNIT.Equals(queryDept) && o.ONB_CHECK_TIMES == numCheckTimes).FirstOrDefault();
            tbm1202.OVC_CHECKER = checker;
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1202.GetType().Name.ToString(), this, "修改");
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "成功指派承辦人");
        }
        #endregion

        protected void btnYearQuery_Click(object sender, EventArgs e)
        {
            GVDataImport(drpYEAR.SelectedValue);

        }

        protected void GV_NOT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbl = (Label)e.Row.FindControl("lblOVC_DRESULT");
                Label lblTitle = (Label)e.Row.FindControl("lblDRESULT_TITELE");
                Label lblCHECKER = (Label)e.Row.FindControl("lblCHECKER");
                Button btn = (Button)e.Row.FindControl("btnSelect");
                DropDownList drpOVC_CHECKER = (DropDownList)e.Row.FindControl("drpOVC_CHECKER");
                if (string.IsNullOrEmpty(lbl.Text))
                {
                    checkerListImport(drpOVC_CHECKER);
                    drpOVC_CHECKER.Items.FindByText(lblCHECKER.Text).Selected = true;
                }
                else
                {
                    lbl.Visible = true;
                    lblTitle.Visible = true;
                    lblCHECKER.Visible = true;
                    btn.Visible = false;
                    drpOVC_CHECKER.Visible = false;
                }
            }
        }

        protected void GV_NOT_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = 0;
            GridViewRow row;
            GridView grid = sender as GridView;
            if (e.CommandName.Equals("btnSelect"))
            {
                index = Convert.ToInt32(e.CommandArgument);
                row = grid.Rows[index];
                DropDownList drpOVC_CHECKER = (DropDownList)row.FindControl("drpOVC_CHECKER");
                SaveChangeChecker(drpOVC_CHECKER.SelectedValue, row.Cells[0].Text, row.Cells[3].Text);
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
    }
}