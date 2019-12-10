using System;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Entity.GMModel;
using System.Linq;
using System.Globalization;


namespace FCFDFE.pages.MPMS.C
{
    

    public partial class MPMS_C13 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        Common FCommon = new Common();
        static string[] field = { "IS_PLURAL_BASIS", "OVC_PURCH", "OVC_PURCH_AGENCY", "OVC_PUR_IPURCH",
                                    "OVC_PUR_NSECTION", "ONB_CHECK_TIMES","MaxDate" ,"MinDate" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    GetUserInfo();
                    list_dataImport(drpOVC_BUDGET_YEAR);
                }
            }
        }

        protected void btnQuery_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "btnQueryallOVC":
                    DataQuery("all");
                    GV_CASE.Visible = false;
                    break;
                case "btnQuerynoONB":
                    DataQuery("NoStatus");
                    GV_CASE.Visible = false;
                    break;
                case "btnQuerymyOVC":
                    GV_CASE.Visible = true;
                    QueryCase();
                    DataQuery("all");
                    break;
            }
        }

        protected void btnToAUDIT_Command(object sender, CommandEventArgs e)
        {
            string strPurchNum = e.CommandArgument.ToString();
            string url = "";
            switch (e.CommandName)
            {
                case "btnToAUDIT":
                    url = "~/pages/MPMS/C/MPMS_C14.aspx?PurchNum=" + strPurchNum;
                    break;
                case "btnCheck":
                    url = "~/pages/MPMS/C/MPMS_C13_1.aspx?PurchNum=" + strPurchNum;
                    break;
            }
            Response.Redirect(url);
        }


        #region 副程式
        protected void GV_Query_PLAN_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GV_Query_PLAN.UseAccessibleHeader = true;
                GV_Query_PLAN.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

       

        protected void GV_Query_PLAN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hidIS_PLURAL_BASIS = (HiddenField)e.Row.FindControl("hidIS_PLURAL_BASIS");
                HiddenField hidOVC_CHECK_OK = (HiddenField)e.Row.FindControl("hidOVC_CHECK_OK");
                HiddenField hidOVC_PERMISSION_UPDATE = (HiddenField)e.Row.FindControl("hidOVC_PERMISSION_UPDATE");
                HiddenField hidOVC_DREJECT = (HiddenField)e.Row.FindControl("hidOVC_DREJECT");
                HiddenField hidMinDate = (HiddenField)e.Row.FindControl("hidMinDate");
                Label lblSignDate = (Label)e.Row.FindControl("lblSignDate");
                Label lblSignMonth = (Label)e.Row.FindControl("lblSignMonth");
                DateTime dateReject, DateRecive;
                Button btnThis = (Button)e.Row.FindControl("btnCheck");
                string checkTimes = e.Row.Cells[3].Text;
                DateTime now = DateTime.Now;
                TimeSpan span;
                Boolean isTimeData_Reject = DateTime.TryParse(hidOVC_DREJECT.Value, out dateReject);
                Boolean isTimeData_Receive = DateTime.TryParse(hidOVC_DREJECT.Value, out DateRecive);
                if (hidIS_PLURAL_BASIS.Value.Equals("Y"))
                {
                    btnThis.Visible = true;
                }
                
                //逾時一個月未核定
                if (isTimeData_Receive)
                {
                    span = now.Subtract(DateRecive);
                    int diff = Convert.ToInt16(span.Days);
                    if (diff >= 30)
                    {
                        lblSignMonth.Visible = true;
                    }
                }

                //逾時五日、七日
                if (isTimeData_Reject)
                {
                    span = now.Subtract(dateReject);
                    int diff = Convert.ToInt16(span.Days);
                    if (hidOVC_CHECK_OK.Value != "Y" && hidOVC_PERMISSION_UPDATE.Value.Equals("Y"))
                    {
                        if (checkTimes.Equals("1") && diff >=7 )
                        {
                            lblSignDate.Visible = true;
                        }
                        else if(checkTimes != "1" && diff >= 5)
                        {
                            lblSignDate.Visible = true;
                        }
                    }
                }
                
            }
        }
        
        private void GetUserInfo()
        {
            if (Session["userid"] != null)
            {
                string strUSER_ID = Session["userid"].ToString();
                var query =
                    (from tAccount in gm.ACCOUNTs
                     where tAccount.USER_ID.Equals(strUSER_ID)
                     select new
                     {
                         tAccount.DEPT_SN,
                         tAccount.USER_NAME
                     }).FirstOrDefault();

                ViewState["DEPT_SN"] = query.DEPT_SN;
                ViewState["USER_NAME"] = query.USER_NAME;
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

        private void QueryCase()
        {
            //顯示所有人的承辦案件數量
            string year = drpOVC_BUDGET_YEAR.SelectedItem.Text;
            string yearSub = year.Substring((year.Length - 2), 2);
            string[] field = { "Name", "Count" }; 
            if (Session["userid"] != null)
            {
                string strUSER_ID = Session["userid"].ToString();
                //查詢本單位底下所有承辦人
                var query =
                    (from tAccount in gm.ACCOUNTs
                     join tDept in gm.TBMDEPTs on tAccount.DEPT_SN equals tDept.OVC_DEPT_CDE
                     where tAccount.USER_ID.Equals(strUSER_ID)
                     select new
                     {
                         tAccount.DEPT_SN,
                         tDept.OVC_ONNAME
                     }).FirstOrDefault();
                var queryqUnion =
                     (from t in mpms.ACCOUNT_AUTH
                      join t2 in mpms.ACCOUNT on t.USER_ID equals t2.USER_ID
                      where t.C_SN_AUTH.Equals("3203") && t2.DEPT_SN.Equals(query.DEPT_SN)
                      select t2.USER_NAME)
                     .Union
                     (from t in mpms.TBM5200_1
                      join t2 in mpms.TBM5200_PPP on t.USER_ID equals t2.USER_ID
                      where t.C_SN_ROLE.Equals("31") && t.OVC_PRIV_LEVEL.Equals("7") && t2.OVC_PUR_SECTION.Equals(query.DEPT_SN)
                      select t2.USER_NAME);

                //查詢今年案數
                var queryCaseGroup =
                from t in mpms.TBM1202
                join t1301P in mpms.TBM1301_PLAN on t.OVC_PURCH equals t1301P.OVC_PURCH
                where t.OVC_CHECK_UNIT.Equals(query.DEPT_SN) && t1301P.OVC_AUDIT_UNIT.Equals(query.DEPT_SN)
                        && t.OVC_PURCH.Substring(2, 2).Equals(yearSub)
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
                        Count = g.Count()
                    };
                //合併
                var queryAllCount =
                    from t in queryqUnion.AsEnumerable()
                    join t2 in queryCaseCount.AsEnumerable() on t equals t2.Checker into ps
                    from t2 in ps.DefaultIfEmpty()
                    select new
                    {
                        Name = t,
                        Count = t2 != null ? t2.Count : 0
                    };
                DataTable dt = new DataTable();
                dt = CommonStatic.LinqQueryToDataTable(queryAllCount);
                hasRows = FCommon.GridView_dataImport(GV_CASE, dt, field);
                FooterCount(yearSub);
            }

        }

       

        private void FooterCount(string yearsub)
        {
           //承辦案件合計
            string dept = ViewState["DEPT_SN"].ToString();
            var query =
                from t in mpms.TBMOVERSEE_LOG
                where t.OVC_OVERSEE_UNIT.Equals(dept) && t.OVC_PURCH.Substring(2, 2).Equals(yearsub)
                group t by t.OVC_PURCH into g
                select new
                {
                    g.Key,
                    max = g.Max(o => o.OVC_DATE)
                };

            var query2 =
            (from t1301 in mpms.TBM1301
             join t1301P in mpms.TBM1301_PLAN on t1301.OVC_PURCH equals t1301P.OVC_PURCH
             join tOversee in query on t1301.OVC_PURCH equals tOversee.Key
             where (t1301P.OVC_DCANCEL == null || t1301P.OVC_DCANCEL == " ") && (t1301.OVC_PUR_DCANPO == null || t1301.OVC_PUR_DCANPO == " ")
                    && t1301.OVC_DPROPOSE != null && t1301.OVC_DPROPOSE != " " && t1301P.OVC_PURCH.Substring(2, 2).Equals(yearsub)
             select new
             {
                 t1301.OVC_PURCH
             }).Count();
            GV_CASE.FooterRow.Cells[0].Text = "合計（不含尚未分辦）";
            GV_CASE.FooterRow.Cells[0].ColumnSpan = 2;
            GV_CASE.FooterRow.Cells[1].Text = query2.ToString();
            GV_CASE.FooterRow.Cells.RemoveAt(2);

        }

       
        private void DataQuery(string queryType)
        {
            string year = drpOVC_BUDGET_YEAR.SelectedItem.Text;
            string yearSub = year.Substring((year.Length - 2), 2);
            string DEPT_SN = ViewState["DEPT_SN"].ToString();
            string USER_NAME = ViewState["USER_NAME"].ToString();

            var query =
                from t in mpms.TBM1202
                where t.OVC_PURCH.Substring(2, 2).Equals(yearSub) && t.OVC_CHECKER.Equals(USER_NAME)
                        && t.OVC_CHECK_UNIT.Equals(DEPT_SN)
                group t by new
                {
                    t.OVC_PURCH,
                    t.OVC_CHECK_UNIT,
                    t.OVC_CHECKER,

                } into g
                select new
                {
                    ONB_CHECK_TIMES = g.Max(o => o.ONB_CHECK_TIMES),
                    MinDate = g.Min(o => o.OVC_DRECEIVE),
                    MaxDate = g.Max(o => o.OVC_DRECEIVE),
                    g.Key.OVC_PURCH,
                    g.Key.OVC_CHECK_UNIT,
                    g.Key.OVC_CHECKER,
                };

            if (DEPT_SN.Equals("0A100") || DEPT_SN.Equals("00N00"))
            {
                var query2 =
                    from t in query
                    join t1202 in mpms.TBM1202
                    on new
                    {
                        t.OVC_PURCH,
                        t.ONB_CHECK_TIMES,
                        t.OVC_CHECK_UNIT
                    }
                    equals new
                    {
                        t1202.OVC_PURCH,
                        t1202.ONB_CHECK_TIMES,
                        t1202.OVC_CHECK_UNIT
                    }
                    join t1301 in mpms.TBM1301 on t.OVC_PURCH equals t1301.OVC_PURCH
                    join t1301P in mpms.TBM1301_PLAN on t.OVC_PURCH equals t1301P.OVC_PURCH
                    where string.IsNullOrEmpty(t1301.OVC_PUR_DCANPO)
                            && (t1301P.OVC_AUDIT_UNIT.Equals(DEPT_SN) || t1301P.OVC_PUR_APPROVE_DEP.Equals("A"))
                    select new
                    {
                        t.ONB_CHECK_TIMES,
                        t.MinDate,
                        t.MaxDate,
                        t.OVC_PURCH,
                        t1301.OVC_PUR_IPURCH,
                        t1301.OVC_PUR_NSECTION,
                        t1301.OVC_PUR_AGENCY,
                        t.OVC_CHECK_UNIT,
                        t.OVC_CHECKER,
                        t1202.OVC_DREJECT,
                        t1202.OVC_CHECK_OK,
                        t1301.IS_PLURAL_BASIS,
                        t1301.OVC_PERMISSION_UPDATE,
                    };
                if (queryType.Equals("all"))
                {
                    DataTable dt = CommonStatic.LinqQueryToDataTable(query2);
                    hasRows = FCommon.GridView_dataImport(GV_Query_PLAN, dt, field);
                }
                else if(queryType.Equals("NoStatus"))
                {
                    var queryLlist =
                        from t in query2
                        select new { t.OVC_PURCH };

                    var qStatus =
                    from t in mpms.TBMSTATUS
                    where t.OVC_STATUS != "19"
                    group t by t.OVC_PURCH into g
                    select new
                    {
                        OVC_PURCH = g.Key,
                    };
                    var finalList = queryLlist.Except(qStatus);

                    var queryFinal =
                    from t in query2
                    join b in finalList on t.OVC_PURCH equals b.OVC_PURCH
                    select new
                    {
                        t.ONB_CHECK_TIMES,
                        t.MinDate,
                        t.MaxDate,
                        t.OVC_PURCH,
                        t.OVC_PUR_IPURCH,
                        t.OVC_PUR_NSECTION,
                        t.OVC_PUR_AGENCY,
                        t.OVC_CHECK_UNIT,
                        t.OVC_CHECKER,
                        t.OVC_DREJECT,
                        t.OVC_CHECK_OK,
                        t.IS_PLURAL_BASIS,
                        t.OVC_PERMISSION_UPDATE,
                    };
                    DataTable dt = CommonStatic.LinqQueryToDataTable(queryFinal);
                    hasRows = FCommon.GridView_dataImport(GV_Query_PLAN, dt, field);
                }
            }
            else
            {
                var query2 =
                    from t in query
                    join t1202 in mpms.TBM1202 
                    on new
                    {
                        t.OVC_PURCH,
                        t.ONB_CHECK_TIMES,
                        t.OVC_CHECK_UNIT
                    } 
                    equals new
                    {
                        t1202.OVC_PURCH,
                        t1202.ONB_CHECK_TIMES,
                        t1202.OVC_CHECK_UNIT
                    }
                    join t1301 in mpms.TBM1301 on t.OVC_PURCH equals t1301.OVC_PURCH
                    where string.IsNullOrEmpty(t1301.OVC_PUR_DCANPO)
                    select new
                    {
                        t.ONB_CHECK_TIMES,
                        t.MinDate,
                        t.MaxDate,
                        t.OVC_PURCH,
                        t1301.OVC_PUR_IPURCH,
                        t1301.OVC_PUR_NSECTION,
                        t1301.OVC_PUR_AGENCY,
                        t.OVC_CHECK_UNIT,
                        t.OVC_CHECKER,
                        t1202.OVC_DREJECT,
                        t1202.OVC_CHECK_OK,
                        t1301.IS_PLURAL_BASIS,
                        t1301.OVC_PERMISSION_UPDATE,
                    };

                if (queryType.Equals("all"))
                {
                    DataTable dt = CommonStatic.LinqQueryToDataTable(query2);
                    hasRows = FCommon.GridView_dataImport(GV_Query_PLAN, dt, field);
                }
                else if (queryType.Equals("NoStatus"))
                {
                    var queryLlist =
                        from t in query2
                        select new { t.OVC_PURCH };

                    var qStatus =
                    from t in mpms.TBMSTATUS
                    where t.OVC_STATUS != "19"
                    group t by t.OVC_PURCH into g
                    select new
                    {
                        OVC_PURCH = g.Key,
                    };
                    var finalList = queryLlist.Except(qStatus);

                    var queryFinal =
                    from t in query2
                    join b in finalList on t.OVC_PURCH equals b.OVC_PURCH
                    select new
                    {
                        t.ONB_CHECK_TIMES,
                        t.MinDate,
                        t.MaxDate,
                        t.OVC_PURCH,
                        t.OVC_PUR_IPURCH,
                        t.OVC_PUR_NSECTION,
                        t.OVC_PUR_AGENCY,
                        t.OVC_CHECK_UNIT,
                        t.OVC_CHECKER,
                        t.OVC_DREJECT,
                        t.OVC_CHECK_OK,
                        t.IS_PLURAL_BASIS,
                        t.OVC_PERMISSION_UPDATE,
                    };
                    DataTable dt = CommonStatic.LinqQueryToDataTable(queryFinal);
                    hasRows = FCommon.GridView_dataImport(GV_Query_PLAN, dt, field);
                }
            }
            
        }
        #endregion
         
       
    }
}