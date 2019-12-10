using System;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Linq;
using System.Globalization;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.C
{
    public partial class MPMS_C12 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        string strPurchNum = "";
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        Common FCommon = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (string.IsNullOrEmpty(Request.QueryString["PurchNum"]))
                {
                    Response.Redirect("MPMS_C11");
                }
                else
                {
                    strPurchNum = Request.QueryString["PurchNum"];
                    if (!IsPostBack)
                    {
                        DataImport();
                        GetDept();
                        GVImport();
                        FCommon.Controls_Attributes("readonly", "true", txtOVC_DAUDIT_ASSIGN, txtOVC_DRECEIVE_PAPER);
                    }
                }
            }
        }

        protected void GV_PLAN_CHECK_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {

                GV_PLAN_CHECK.UseAccessibleHeader = true;
                GV_PLAN_CHECK.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }


        #region onclick
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            GVImport();
        }

        protected void GV_PLAN_CHECK_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = 0;
            GridViewRow row;
            GridView grid = sender as GridView;
            string strPurchNum = (lblOVC_PURCH.Text).Substring(0, lblOVC_PURCH.Text.Length - 1);
            string dept = ViewState["DEPT_SN"].ToString();
            
            if (e.CommandName.Equals("btnSelect"))
            {
                index = Convert.ToInt32(e.CommandArgument);
                row = grid.Rows[index];
                HiddenField hidDRecive = (HiddenField)row.FindControl("hidDRecive");
                byte checkTimes = Convert.ToByte(row.Cells[0].Text);
                TBM1202 tbm1202  = mpms.TBM1202.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.ONB_CHECK_TIMES == checkTimes
                                && o.OVC_CHECK_UNIT.Equals(dept) && o.OVC_DRECEIVE.Equals(hidDRecive.Value)).FirstOrDefault();
                if(tbm1202 != null)
                {
                    mpms.Entry(tbm1202).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    FCommon.AlertShow(PnMessage,"success", "系統訊息", "刪除成功");
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , tbm1202.GetType().Name.ToString(), this, "刪除");
                }
            }
            GVImport();
        }
        #endregion

        #region 副程式
        private void DataImport()
        {
            var query =
                (from t1301 in gm.TBM1301
                where t1301.OVC_PURCH.Equals(strPurchNum)
                select new
                {
                    t1301.OVC_PURCH,
                    t1301.OVC_PUR_AGENCY,
                    t1301.OVC_PUR_IPURCH,
                    t1301.OVC_PUR_NSECTION,
                    t1301.OVC_PUR_SECTION,
                    t1301.OVC_PUR_USER,
                    t1301.OVC_PUR_IUSER_PHONE_EXT,
                }).FirstOrDefault();
            lblOVC_PURCH.Text = query.OVC_PURCH + query.OVC_PUR_AGENCY;
            lblOVC_PUR_IPURCH.Text = query.OVC_PUR_IPURCH;
            lblOVC_PUR_USER.Text = query.OVC_PUR_NSECTION + "(" + query.OVC_PUR_SECTION + ")"
                                    + query.OVC_PUR_USER + "(" + query.OVC_PUR_IUSER_PHONE_EXT + ")";

        }

        private void GetDept()
        {
            if (Session["userid"] != null)
            {
                string strUSER_ID = Session["userid"].ToString();
                var query =
                    (from tAccount in gm.ACCOUNTs
                     join tDept in gm.TBMDEPTs on tAccount.DEPT_SN equals tDept.OVC_DEPT_CDE
                     where tAccount.USER_ID.Equals(strUSER_ID)
                     select new
                     {
                         tAccount.DEPT_SN,
                         tDept.OVC_ONNAME
                     }).FirstOrDefault();
                lblOVC_PUR_SECTION.Text = query.OVC_ONNAME;
                ViewState["DEPT_SN"] = query.DEPT_SN;
                var queryqUnion =
                       (from t in mpms.ACCOUNT_AUTH
                        join acc in mpms.ACCOUNT on t.USER_ID equals acc.USER_ID
                        where t.C_SN_AUTH.Equals("3203") && acc.DEPT_SN.Equals(query.DEPT_SN) && t.IS_ENABLE.Equals("Y")
                        select acc.USER_NAME)
                     .Union
                     (from t in mpms.TBM5200_1
                      join t2 in mpms.TBM5200_PPP on t.USER_ID equals t2.USER_ID
                      where t.C_SN_ROLE.Equals("31") && t.OVC_PRIV_LEVEL.Equals("7") && t2.OVC_PUR_SECTION.Equals(query.DEPT_SN) && t.OVC_ENABLE.Equals("Y")
                      select t2.USER_NAME);
                drpOVC_CHECKER.Items.Clear();
                foreach (var item in queryqUnion)
                {
                    drpOVC_CHECKER.Items.Add(item);
                }

            }
        }
        

        private void SaveData()
        {
            string dept = ViewState["DEPT_SN"].ToString();
            if (string.IsNullOrEmpty(txtOVC_DAUDIT_ASSIGN.Text))
            {
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "請先 輸入分派日");
            }
            else
            {
                try
                {
                    TBM1202 tbm1202 = new TBM1202();
                    tbm1202.OVC_PURCH = (lblOVC_PURCH.Text).Substring(0, lblOVC_PURCH.Text.Length - 1);
                    tbm1202.OVC_DRECEIVE = txtOVC_DAUDIT_ASSIGN.Text;
                    tbm1202.OVC_CHECK_UNIT = dept;
                    tbm1202.ONB_CHECK_TIMES = Convert.ToByte(txtONB_CHECK_TIMES.Text);
                    tbm1202.OVC_CHECKER = drpOVC_CHECKER.SelectedItem.Text;
                    tbm1202.OVC_ASSIGNER = lblOVC_ASSIGNER.Text;
                    tbm1202.OVC_DRECEIVE_PAPER = txtOVC_DRECEIVE_PAPER.Text;
                    mpms.TBM1202.Add(tbm1202);
                    mpms.SaveChanges();
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "成功指派承辦人");
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , tbm1202.GetType().Name.ToString(), this, "新增");
                }
                catch
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "新增失敗");
                }
            }
            
            
        }

        private void GVImport()
        {
            string[] field = { "ONB_CHECK_TIMES", "OVC_DRESULT", "OVC_ONNAME", "OVC_ASSIGNER", "OVC_DRECEIVE", "OVC_DRECEIVE_CHINESE", "OVC_CHECKER" };
            string dept = ViewState["DEPT_SN"].ToString();
            string strPurchNum = (lblOVC_PURCH.Text).Substring(0, lblOVC_PURCH.Text.Length - 1);
            byte checkTimes = Convert.ToByte(txtONB_CHECK_TIMES.Text);
            var query =
                from t in mpms.TBM1202
                join tDept in mpms.TBMDEPTs on t.OVC_CHECK_UNIT equals tDept.OVC_DEPT_CDE
                where t.OVC_PURCH.Equals(strPurchNum) && t.OVC_CHECK_UNIT.Equals(dept)
                orderby t.ONB_CHECK_TIMES
                select new
                {
                    t.ONB_CHECK_TIMES,
                    t.OVC_DRESULT,
                    tDept.OVC_ONNAME,
                    t.OVC_ASSIGNER,
                    t.OVC_DRECEIVE,
                    OVC_DRECEIVE_CHINESE = t.OVC_DRECEIVE,
                    t.OVC_CHECKER
                };

            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            foreach(DataRow rows in dt.Rows)
            {
                rows["OVC_DRECEIVE_CHINESE"] = GetTaiwanDate(rows["OVC_DRECEIVE_CHINESE"].ToString());
            }
            hasRows = FCommon.GridView_dataImport(GV_PLAN_CHECK, dt, field);
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
        #endregion
    }
}