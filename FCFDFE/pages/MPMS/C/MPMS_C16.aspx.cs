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
    public partial class MPMS_C16 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        MPMSEntities mpms = new MPMSEntities();
        GMEntities gm = new GMEntities();
        Common FCommon = new Common();
        string[] arrAuth = { "3502" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    GetUserInfo();
                    list_dataImport(drpYEAR);
                    GVDataImport(drpYEAR.SelectedValue);
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

        protected void GV_NOT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList drp = (DropDownList)e.Row.FindControl("drpOVC_CHECKER");
                Button btnSave = (Button)e.Row.FindControl("btnSave");
                Label lblDate = (Label)e.Row.FindControl("lblDate");
                Label lblvDate = (Label)e.Row.FindControl("lblvDate");
                HiddenField hidOVC_DRESULT = (HiddenField)e.Row.FindControl("hidOVC_DRESULT");
                HiddenField hidOVC_DAUDIT = (HiddenField)e.Row.FindControl("hidOVC_DAUDIT");
                HiddenField hidOVC_DAUDIT_ASSIGN = (HiddenField)e.Row.FindControl("hidOVC_DAUDIT_ASSIGN");
                ListAuditorImport(drp);
                //判斷是否顯示分派按鈕
                if (string.IsNullOrEmpty(hidOVC_DRESULT.Value))
                {
                    if (string.IsNullOrEmpty(hidOVC_DAUDIT.Value))
                    {
                        btnSave.Visible = true;
                    }
                    else
                    {
                        lblDate.Text = "審查日<br/>" + hidOVC_DAUDIT.Value;
                        lblDate.Visible = true;
                        drp.Visible = false;
                    }
                }
                else
                {
                    lblDate.Text = "審查綜簽日<br/>" + hidOVC_DRESULT.Value;
                    lblDate.Visible = true;
                    drp.Visible = false;
                }

                string vDate = "";
                if (hidOVC_DAUDIT_ASSIGN.Value.Length >= 10)
                {
                    vDate = hidOVC_DAUDIT_ASSIGN.Value.Substring(0, 10) + "  " + hidOVC_DAUDIT_ASSIGN.Value.Substring(10);//審查單位分派日
                    if (hidOVC_DAUDIT.Value.Length >= 10)
                    {
                        vDate = vDate + "<br/>" + hidOVC_DAUDIT.Value.Substring(0, 10)
                      +"  " + hidOVC_DAUDIT.Value.Substring(10);//審查單位回覆日
                    }
                    else
                    {
                        vDate = vDate + "<br/>" + hidOVC_DAUDIT.Value;
                    }
                }
                else
                {
                    vDate = hidOVC_DAUDIT_ASSIGN.Value;//審查單位分派日
                    if (hidOVC_DAUDIT.Value.Length >= 10)
                    {
                        vDate = vDate + "<br/>" + hidOVC_DAUDIT.Value.Substring(0, 10)+
                                 "  " + hidOVC_DAUDIT.Value.Substring(10);//審查單位回覆日
                    }
                    else
                    {
                        vDate = vDate + "<br/>" + hidOVC_DAUDIT.Value;
                    }
                }
                lblvDate.Text = vDate;
            }
        }

        #region 副程式
        private void GVDataImport(string year)
        {
            string[] field = { "OVC_PURCH", "OVC_PUR_IPURCH", "OVC_PUR_NSECTION", "ONB_CHECK_TIMES" };
            string yearSub = year.Substring((year.Length - 2), 2);
            string deptSN = ViewState["DEPT_SN"].ToString();
            string strUnit = ViewState["OVC_AUDIT_UNIT"].ToString();
            var query =
                from t1202_1 in mpms.TBM1202_1
                join t1301 in mpms.TBM1301 on t1202_1.OVC_PURCH equals t1301.OVC_PURCH
                join t1202 in mpms.TBM1202 
                on new
                {
                    t1202_1.OVC_PURCH,
                    t1202_1.OVC_DRECEIVE,
                    t1202_1.OVC_CHECK_UNIT,
                    t1202_1.ONB_CHECK_TIMES
                }
                equals new
                {
                    t1202.OVC_PURCH,
                    t1202.OVC_DRECEIVE,
                    t1202.OVC_CHECK_UNIT,
                    t1202.ONB_CHECK_TIMES
                }
                where t1202_1.OVC_PURCH.Substring(2, 2).Equals(yearSub) 
                    && (string.IsNullOrEmpty(t1202_1.OVC_AUDITOR))
                    && t1202_1.OVC_CHECK_UNIT.Equals(deptSN) && t1202_1.OVC_AUDIT_UNIT.Equals(strUnit)
                orderby t1202_1.OVC_PURCH, t1202_1.ONB_CHECK_TIMES
                select new
                {
                    t1202_1.OVC_PURCH,
                    t1202_1.ONB_CHECK_TIMES,
                    t1202_1.OVC_DAUDIT,
                    t1202_1.OVC_DAUDIT_ASSIGN,
                    t1301.OVC_PUR_IPURCH,
                    t1301.OVC_PUR_AGENCY,
                    t1301.OVC_PUR_NSECTION,
                    t1202.OVC_DRESULT
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            hasRows = FCommon.GridView_dataImport(GV_NOT, dt, field);
        }

        private void GetUserInfo()
        {
            if (Session["userid"] != null)
            {
                string strUSER_ID = Session["userid"].ToString();
                var query_Auth =
                     (from tAccount in mpms.ACCOUNT
                      join tAuth in mpms.ACCOUNT_AUTH on tAccount.USER_ID equals tAuth.USER_ID
                      where tAccount.USER_ID.Equals(strUSER_ID)
                      where tAuth.C_SN_AUTH.Equals("3501")
                      select new
                      {
                          tAccount.DEPT_SN,
                          tAuth.C_SN_SUB
                      }).FirstOrDefault();
                if (query_Auth == null)
                {
                    var query =
                   (from tAccount in mpms.ACCOUNT
                    join t52001 in mpms.TBM5200_1 on tAccount.USER_ID equals t52001.USER_ID
                    where tAccount.USER_ID.Equals(strUSER_ID)
                    select new
                    {
                        tAccount.DEPT_SN,
                        t52001.OVC_AUDIT_UNIT
                    }).FirstOrDefault();

                    ViewState["DEPT_SN"] = query.DEPT_SN;
                    ViewState["OVC_AUDIT_UNIT"] = query.OVC_AUDIT_UNIT;
                }
                else
                {
                    ViewState["DEPT_SN"] = query_Auth.DEPT_SN;
                    ViewState["OVC_AUDIT_UNIT"] = query_Auth.C_SN_SUB;
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

        private void ListAuditorImport(ListControl list)
        {

            list.Items.Clear();
            string deptSN = ViewState["DEPT_SN"].ToString();
            string deptAUDIT = ViewState["OVC_AUDIT_UNIT"].ToString();
            var query =
                from t in mpms.ACCOUNT
                join t2 in mpms.TBM5200_1 on t.USER_ID equals t2.USER_ID
                where t2.C_SN_ROLE.Equals("3A") && t2.OVC_PRIV_LEVEL.Equals("7")
                        && t.DEPT_SN.Equals(deptSN) && t2.OVC_AUDIT_UNIT.Equals(deptAUDIT) && t2.OVC_ENABLE.Equals("Y")
                select t.USER_NAME;
            var queryNewRule =
                from t in gm.ACCOUNTs
                join tAuth in gm.ACCOUNT_AUTH on t.USER_ID equals tAuth.USER_ID
                where tAuth.C_SN_SUB.Equals(deptAUDIT) && tAuth.IS_ENABLE.Equals("Y")
                where arrAuth.Contains(tAuth.C_SN_AUTH)
                select t.USER_NAME;
            var queryUnion = query.ToList().Union(queryNewRule.ToList());
            foreach (var item in queryUnion)
            {
                list.Items.Add(item);
            }
        }

        #endregion

        #region onClick
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Button btnThis = (Button)sender;
            int gvRowIndex = (btnThis.NamingContainer as GridViewRow).RowIndex;
            Label lblOVC_PURCH = (Label)GV_NOT.Rows[gvRowIndex].Cells[1].FindControl("lblOVC_PURCH");
            DropDownList drp = (DropDownList)GV_NOT.Rows[gvRowIndex].Cells[6].FindControl("drpOVC_CHECKER");
            string strPurchNum = lblOVC_PURCH.Text.Substring(0,7);
            byte numCheckTimes  =Convert.ToByte(GV_NOT.Rows[gvRowIndex].Cells[4].Text);
            string strAUDIOR = drp.SelectedItem.Text;
            string AUDIT_UNIT = ViewState["OVC_AUDIT_UNIT"].ToString();

            if (!string.IsNullOrEmpty(strAUDIOR))
            {
                TBM1202_1 tbm1202_1 = new TBM1202_1();
                tbm1202_1 =
                    mpms.TBM1202_1
                    .Where(o => o.OVC_PURCH.Equals(strPurchNum)
                        && o.ONB_CHECK_TIMES == numCheckTimes
                        && o.OVC_AUDIT_UNIT.Equals(AUDIT_UNIT))
                    .FirstOrDefault();
                tbm1202_1.OVC_DAUDIT_ASSIGN = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                tbm1202_1.OVC_AUDIT_ASSIGNER = Session["username"].ToString();
                tbm1202_1.OVC_AUDITOR = strAUDIOR;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1202_1.GetType().Name.ToString(), this, "修改");
                GVDataImport(drpYEAR.SelectedValue);
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "完成指派承辦人");
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "沒有承辦人可供指派");
            }

        }

       
        protected void btnYearQuery_Click(object sender, EventArgs e)
        {
            GVDataImport(drpYEAR.SelectedValue);
        }
        #endregion
    }
}