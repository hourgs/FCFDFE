using System;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Entity.GMModel;
using System.Globalization;
using System.Data;

namespace FCFDFE.pages.MPMS.C
{
    public partial class MPMS_C25 : System.Web.UI.Page
    {

        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        MPMSEntities mpms = new MPMSEntities();
        GMEntities gm = new GMEntities();
        Common FCommon = new Common();
        string[] field = { "Name", "DoingCase", "DoneCase", "AllCase" };
        protected void Page_Load(object sender, EventArgs e)
        {

            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    list_dataImport(drpYEAR);
                    GetUserInfo();
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

        protected void btnYearQuery_Click(object sender, EventArgs e)
        {
            
            string DEPT_SN = ViewState["DEPT_SN"].ToString();
            string year = drpYEAR.SelectedValue;
            string yearSub = year.Substring((year.Length - 2), 2);
            if (DEPT_SN.Equals("0A100") || DEPT_SN.Equals("00N00"))
            {
                GV_DataImportDEPTs(DEPT_SN, yearSub);
            }
            else
            {
                GV_DataImport(DEPT_SN, yearSub);
            }
        }

        private void GV_DataImportDEPTs(string dept,string yearSub)
        {
            string C_SN_ROLE = ViewState["C_SN_ROLE"].ToString();
            string OVC_AUDIT_UNIT = ViewState["OVC_AUDIT_UNIT"].ToString();
            //承辦人清單
            var users =
                (from t in mpms.ACCOUNT
                join t5200_1 in mpms.TBM5200_1 on t.USER_ID equals t5200_1.USER_ID
                where t5200_1.OVC_PRIV_LEVEL.Equals("7")
                        && t5200_1.C_SN_ROLE.Equals(C_SN_ROLE) && t5200_1.OVC_ENABLE.Equals("Y")
                        && t.DEPT_SN.Equals(dept)
                select t.USER_NAME)
                .Union
                (from t2 in mpms.ACCOUNT
                 join tAuth in mpms.ACCOUNT_AUTH on t2.USER_ID equals tAuth.USER_ID
                 where tAuth.C_SN_AUTH.Equals("3502") && tAuth.IS_ENABLE.Equals("Y") && tAuth.C_SN_SUB.Equals(OVC_AUDIT_UNIT)
                 select t2.USER_NAME); ;
            //承辦案數
            var handleCase =
                from t1202_1 in mpms.TBM1202_1
                join t1301 in mpms.TBM1301 on t1202_1.OVC_PURCH equals t1301.OVC_PURCH
                join t1301P in mpms.TBM1301_PLAN on t1202_1.OVC_PURCH equals t1301P.OVC_PURCH
                join user in users on t1202_1.OVC_AUDITOR equals user
                where t1202_1.OVC_AUDIT_UNIT.Equals(OVC_AUDIT_UNIT) && t1202_1.OVC_CHECK_UNIT.Equals(dept)
                        && string.IsNullOrEmpty(t1301.OVC_PUR_DCANPO) && string.IsNullOrEmpty(t1301P.OVC_DCANCEL)
                        && t1202_1.OVC_PURCH.Substring(2, 2).Equals(yearSub)
                group t1202_1 by new { t1202_1.OVC_AUDITOR, t1202_1.OVC_PURCH } into g
                select new
                {
                    g.Key.OVC_AUDITOR,
                    Count = g.Count()
                };
            var a = handleCase.GroupBy(o => o.OVC_AUDITOR).Select(o => new { o.Key, count = o.Count() }).ToList();
            //審查中案數
            
            string[] arrApprove = { "備採", "昇軸" };
            var onApproveCase =
                from t1202_1 in mpms.TBM1202_1
                join t1301 in mpms.TBM1301 on t1202_1.OVC_PURCH equals t1301.OVC_PURCH
                join t1301P in mpms.TBM1301_PLAN on t1202_1.OVC_PURCH equals t1301P.OVC_PURCH
                where t1202_1.OVC_AUDIT_UNIT.Equals(OVC_AUDIT_UNIT) && t1202_1.OVC_CHECK_UNIT.Equals(dept)
                        && string.IsNullOrEmpty(t1301.OVC_PUR_DCANPO) && string.IsNullOrEmpty(t1301P.OVC_DCANCEL)
                        && t1202_1.OVC_PURCH.Substring(2, 2).Equals(yearSub)
                        && (t1301.OVC_PUR_DAPPROVE == null
                            || (t1301.OVC_PUR_DAPPROVE != null && !arrApprove.Contains(t1301.OVC_PUR_APPROVE.Trim().Substring(0, 2))))
                        && users.Contains(t1202_1.OVC_AUDITOR)
                group t1202_1 by new { t1202_1.OVC_AUDITOR, t1202_1.OVC_PURCH } into g
                select new
                {
                    g.Key.OVC_AUDITOR,
                };
            var b = onApproveCase.GroupBy(o => o.OVC_AUDITOR).Select(o => new { o.Key, count = o.Count() }).ToList();
            //已核定
            var queryDone =
                from t1202_1 in mpms.TBM1202_1
                join t1301 in mpms.TBM1301 on t1202_1.OVC_PURCH equals t1301.OVC_PURCH
                join t1301P in mpms.TBM1301_PLAN on t1202_1.OVC_PURCH equals t1301P.OVC_PURCH
                where t1202_1.OVC_AUDIT_UNIT.Equals(OVC_AUDIT_UNIT) && t1202_1.OVC_CHECK_UNIT.Equals(dept)
                        && string.IsNullOrEmpty(t1301.OVC_PUR_DCANPO) && string.IsNullOrEmpty(t1301P.OVC_DCANCEL)
                        && t1202_1.OVC_PURCH.Substring(2, 2).Equals(yearSub)
                        && (t1301.OVC_PUR_DAPPROVE != null && arrApprove.Contains(t1301.OVC_PUR_APPROVE.Trim().Substring(0, 2)))
                        && users.Contains(t1202_1.OVC_AUDITOR)
                group t1202_1 by new { t1202_1.OVC_AUDITOR, t1202_1.OVC_PURCH } into g
                select new
                {
                    g.Key.OVC_AUDITOR,
                };
            var c = queryDone.GroupBy(o => o.OVC_AUDITOR).Select(o => new { o.Key, count = o.Count()}).ToList();
            var d =
                from queryA in a
                join queryB in b on queryA.Key equals queryB.Key into ps
                from queryB in ps.DefaultIfEmpty()
                join queryC in c on queryA.Key equals queryC.Key into ps2
                from queryC in ps2.DefaultIfEmpty()
                select new
                {
                    Name = queryA.Key,
                    DoingCase = queryB?.count ?? 0,
                    DoneCase = queryC?.count ?? 0,
                    AllCase = queryA.count
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(d);
            FCommon.GridView_dataImport(GV_NOT, dt, field);
        }

        private void GV_DataImport(string dept, string yearSub)
        {
            string C_SN_ROLE = ViewState["C_SN_ROLE"].ToString();
            string OVC_AUDIT_UNIT = ViewState["OVC_AUDIT_UNIT"].ToString();
            //承辦人清單
            var users =
                (from t in mpms.ACCOUNT
                join t5200_1 in mpms.TBM5200_1 on t.USER_ID equals t5200_1.USER_ID
                where t5200_1.OVC_PRIV_LEVEL.Equals("7")
                        && t5200_1.C_SN_ROLE.Equals(C_SN_ROLE) && t5200_1.OVC_ENABLE.Equals("Y")
                        && t.DEPT_SN.Equals(dept)
                select t.USER_NAME)
                .Union
                (from t2 in mpms.ACCOUNT
                 join tAuth in mpms.ACCOUNT_AUTH on t2.USER_ID equals tAuth.USER_ID
                 where tAuth.C_SN_AUTH.Equals("3502") && tAuth.IS_ENABLE.Equals("Y") && tAuth.C_SN_SUB.Equals(OVC_AUDIT_UNIT)
                 select t2.USER_NAME);
            //總承辦案數
            var handleCase =
                from t1202_1 in mpms.TBM1202_1
                join t1301 in mpms.TBM1301 on t1202_1.OVC_PURCH equals t1301.OVC_PURCH
                join t1301P in mpms.TBM1301_PLAN on t1202_1.OVC_PURCH equals t1301P.OVC_PURCH
                join user in users on t1202_1.OVC_AUDITOR equals user
                where t1202_1.OVC_AUDIT_UNIT.Equals(OVC_AUDIT_UNIT) && t1202_1.OVC_CHECK_UNIT.Equals(dept)
                        && string.IsNullOrEmpty(t1301.OVC_PUR_DCANPO) && string.IsNullOrEmpty(t1301P.OVC_DCANCEL)
                        && t1202_1.OVC_PURCH.Substring(2, 2).Equals(yearSub)
                group t1202_1 by new { t1202_1.OVC_AUDITOR, t1202_1.OVC_PURCH } into g
                select new
                {
                    g.Key.OVC_AUDITOR,
                    Count = g.Count()
                };
            var a = handleCase.GroupBy(o => o.OVC_AUDITOR).Select(o => new { o.Key, count = o.Count() }).ToList();
            //審查中案數
            string[] arrApprove = { "備採", "昇軸" };
            var onApproveCase =
                from t1202_1 in mpms.TBM1202_1
                join t1301 in mpms.TBM1301 on t1202_1.OVC_PURCH equals t1301.OVC_PURCH
                join t1301P in mpms.TBM1301_PLAN on t1202_1.OVC_PURCH equals t1301P.OVC_PURCH
                where t1202_1.OVC_AUDIT_UNIT.Equals(OVC_AUDIT_UNIT) && t1202_1.OVC_CHECK_UNIT.Equals(dept)
                        && string.IsNullOrEmpty(t1301.OVC_PUR_DCANPO) && string.IsNullOrEmpty(t1301P.OVC_DCANCEL)
                        && t1202_1.OVC_PURCH.Substring(2, 2).Equals(yearSub)
                        && t1301.OVC_PUR_DAPPROVE == null
                        && users.Contains(t1202_1.OVC_AUDITOR)
                group t1202_1 by new { t1202_1.OVC_AUDITOR, t1202_1.OVC_PURCH } into g
                select new
                {
                    g.Key.OVC_AUDITOR,
                };
            var b = onApproveCase.GroupBy(o => o.OVC_AUDITOR).Select(o => new { o.Key, count = o.Count() }).ToList();
            //已核定案數
            var queryDone =
                from t1202_1 in mpms.TBM1202_1
                join t1301 in mpms.TBM1301 on t1202_1.OVC_PURCH equals t1301.OVC_PURCH
                join t1301P in mpms.TBM1301_PLAN on t1202_1.OVC_PURCH equals t1301P.OVC_PURCH
                where t1202_1.OVC_AUDIT_UNIT.Equals(OVC_AUDIT_UNIT) && t1202_1.OVC_CHECK_UNIT.Equals(dept)
                        && string.IsNullOrEmpty(t1301.OVC_PUR_DCANPO) && string.IsNullOrEmpty(t1301P.OVC_DCANCEL)
                        && t1202_1.OVC_PURCH.Substring(2, 2).Equals(yearSub)
                        && t1301.OVC_PUR_DAPPROVE != null
                        && users.Contains(t1202_1.OVC_AUDITOR)
                group t1202_1 by new { t1202_1.OVC_AUDITOR, t1202_1.OVC_PURCH } into g
                select new
                {
                    g.Key.OVC_AUDITOR,
                };

            var c = queryDone.GroupBy(o => o.OVC_AUDITOR).Select(o => new { o.Key, count = o.Count() }).ToList();
            
            var d =
                from queryA in a
                join queryB in b on queryA.Key equals queryB.Key into ps
                from queryB in ps.DefaultIfEmpty()
                join queryC in c on queryA.Key equals queryC.Key into ps2
                from queryC in ps2.DefaultIfEmpty()
                select new
                {
                    Name = queryA.Key,
                    DoingCase = queryB?.count ?? 0,
                    DoneCase = queryC?.count ?? 0,
                    AllCase = queryA.count
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(d);
            FCommon.GridView_dataImport(GV_NOT, dt, field);
            
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

        protected void GV_NOT_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Button BTN = (Button)e.CommandSource;
            GridViewRow myRow = (GridViewRow)BTN.NamingContainer;
            Label hidONB_NO = (Label)GV_NOT.Rows[myRow.DataItemIndex].FindControl("Label");
            string year = "";
            if (drpYEAR.SelectedItem.Text.Length == 2)
            {
                year = drpYEAR.SelectedItem.Text;
            }
            else
            {
                year = drpYEAR.SelectedItem.Text.Substring(1);
            }
            if (e.CommandName.Equals("btnSave"))
             {
                string name = GV_NOT.Rows[myRow.DataItemIndex].Cells[1].Text;
                string url = "~/pages/MPMS/C/MPMS_C16_1.aspx?Auditor=" + name + "&YearSub=" + year;
                Response.Redirect(url);
            }
        }

        private void GetUserInfo()
        {
            if (Session["userid"] != null)
            {
                string strUSER_ID = Session["userid"].ToString();

                var query =
                    (from tAccount in gm.ACCOUNTs.AsEnumerable()
                     join t52001 in mpms.TBM5200_1 on tAccount.USER_ID equals t52001.USER_ID
                     where tAccount.USER_ID.Equals(strUSER_ID)
                     select new
                     {
                         tAccount.DEPT_SN,
                         t52001.OVC_AUDIT_UNIT,
                         t52001.C_SN_ROLE
                     }).FirstOrDefault();

                if(query != null)
                {
                    ViewState["C_SN_ROLE"] = query.C_SN_ROLE;
                    ViewState["DEPT_SN"] = query.DEPT_SN;
                    ViewState["OVC_AUDIT_UNIT"] = query.OVC_AUDIT_UNIT;
                }
                else
                {
                     var query_Auth =
                       (from tAccount in gm.ACCOUNTs
                        join tAuth in gm.ACCOUNT_AUTH on tAccount.USER_ID equals tAuth.USER_ID
                        where tAccount.USER_ID.Equals(strUSER_ID) && tAuth.C_SN_AUTH.Equals("3502")
                        select new
                        {
                            tAccount.DEPT_SN,
                            tAuth.C_SN_SUB,
                        }).FirstOrDefault();
                    ViewState["C_SN_ROLE"] = "";
                    ViewState["DEPT_SN"] = query_Auth.DEPT_SN;
                    ViewState["OVC_AUDIT_UNIT"] = query_Auth.C_SN_SUB;
                }
               
            }
        }
    }
}