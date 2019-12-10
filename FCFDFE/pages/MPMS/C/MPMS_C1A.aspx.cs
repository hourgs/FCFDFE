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
    public partial class MPMS_C1A : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    GetUserInfo();
                    list_dataImport(drpOVC_BUDGET_YEAR);
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_DCANCEL);
                }
            }
        }
        protected void BtnQuery_OVC_PURCH_Click(object sender, EventArgs e)
        {
            DataQuery();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            CancelCaseSave();
        }
        protected void btnQuery_OVC_YEAR_Click(object sender, EventArgs e)
        {
            CanceledCaseQuery();
        }
        protected void GV_OVC_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {

                GV_OVC.UseAccessibleHeader = true;
                GV_OVC.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        #region 副程式
        private void DataQuery()
        {
            string purchNum = "";
            purchNum = txtOVC_PURCH.Text;
            var qeury = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(purchNum));
            if (qeury.Any())
            {
                var item1301 = qeury.FirstOrDefault();
                lblOVC_PUR_IPURCH.Text = item1301.OVC_PUR_IPURCH;
                lblOVC_PUR_USER.Text = item1301.OVC_PUR_NSECTION + "(" + item1301.OVC_PUR_SECTION + ")-"
                                        + item1301.OVC_PUR_USER + "(" + item1301.OVC_PUR_IUSER_PHONE + "  軍線："
                                        + item1301.OVC_PUR_IUSER_PHONE_EXT + ")";
                var queryStatus =
                    (from tStatus in mpms.TBMSTATUS
                     join t1407 in mpms.TBM1407 on tStatus.OVC_STATUS equals t1407.OVC_PHR_ID
                     where tStatus.OVC_PURCH.Equals(purchNum) && t1407.OVC_PHR_CATE.Equals("Q9")
                     orderby tStatus.OVC_DBEGIN descending
                     select t1407.OVC_PHR_DESC
                    ).FirstOrDefault();
                lblOVC_STATUS.Text = queryStatus;
                // 購案目前承辦單位 相同 
                if ((item1301.OVC_DOING_UNIT == null) ||
                    (item1301.OVC_DOING_UNIT.Equals("")) ||
                    (item1301.OVC_DOING_UNIT.Equals(item1301.OVC_PUR_SECTION)))
                {
                    if ((item1301.OVC_PERMISSION_UPDATE != null) &&
                        (item1301.OVC_PERMISSION_UPDATE.Equals("Y")))
                    {
                        //與 購案目前承辦單位 但退委辦單位澄覆中   
                        FCommon.AlertShow(PnMessage, "danger", "不可撤案!", "本案目前退委辦單位澄覆中！");
                    }
                    else
                    {
                        trCancelDate.Visible = true;
                        trAPPROVE.Visible = true;
                        lblOVC_PUR_DAPPROVE.Text = item1301.OVC_PUR_DAPPROVE;
                        lblOVC_PUR_APPROVE.Text = item1301.OVC_PUR_APPROVE;
                    }
                }
                else
                {
                    //與 購案目前承辦單位 不相同
                    FCommon.AlertShow(PnMessage, "danger", "不可撤案!", "本案目前階段貴官無權處理！\n(本案尚未移送至貴單位)");
                }
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "本購案編號：" + purchNum + " 不存在，請重新輸入！");
            }

        }

        private void CancelCaseSave()
        {
            if(!string.IsNullOrEmpty(txtOVC_PUR_DCANRE.Text) && !string.IsNullOrEmpty(txtOVC_DCANCEL.Text) 
                && !string.IsNullOrEmpty(txtOVC_PURCH.Text))
            {
                System.Data.Entity.Core.Objects.ObjectParameter rTN_MSG =
                    new System.Data.Entity.Core.Objects.ObjectParameter("rTN_MSG", typeof(char));
                mpms.PG_TBM1301_CANCEL_OLTP(txtOVC_PURCH.Text, txtOVC_DCANCEL.Text, txtOVC_PUR_DCANRE.Text, rTN_MSG);
                
                string myKey = rTN_MSG.Value.ToString();
                if (myKey.Equals("SUCCESS"))
                {
                    TBM1202 t1202 = mpms.TBM1202.Where(t => t.OVC_PURCH.Equals(txtOVC_PURCH.Text)).FirstOrDefault();
                    if (t1202 != null)
                    {
                        t1202.OVC_CHECK_OK = "N";
                        mpms.SaveChanges();
                    }
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "撤案成功");
                }
                else
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "撤案失敗");
                }
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "撤案日期或原因不可空白");
            }
           
        }

        private void CanceledCaseQuery()
        {
            string[] field = { "OVC_PURCH", "OVC_PUR_IPURCH", "ONB_CHECK_TIMES", "OVC_CHECKER", "OVC_DRECEIVE" };
            string dept = ViewState["DEPT_SN"].ToString();
            string year = drpOVC_BUDGET_YEAR.SelectedItem.Text;
            string yearSub = year.Substring((year.Length - 2), 2);
            var group1202 =
                from t in mpms.TBM1202
                where t.OVC_PURCH.Substring(2, 2).Equals(yearSub) && t.OVC_CHECK_UNIT.Equals(dept)
                group t by new
                {
                    t.OVC_PURCH,
                    t.OVC_CHECK_UNIT,
                    t.OVC_CHECKER
                } into g
                select new
                {
                    g.Key.OVC_PURCH,
                    g.Key.OVC_CHECK_UNIT,
                    g.Key.OVC_CHECKER,
                    OVC_CHECK_TIMES = g.Select(o => o.ONB_CHECK_TIMES).Max(),
                    ONB_CHECK_TIMES = g.Max(o => o.ONB_CHECK_TIMES),
                    OVC_DRECEIVE = g.Max(o => o.OVC_DRECEIVE),
                };
            if (dept.Equals("0A100") || dept.Equals("00N00"))
            {
                var query =
               from t1202G in group1202
               join t1301 in mpms.TBM1301 on t1202G.OVC_PURCH equals t1301.OVC_PURCH
               join t1301P in mpms.TBM1301_PLAN on t1301.OVC_PURCH equals t1301P.OVC_PURCH
               where t1301.OVC_PURCH.Substring(2, 2).Equals(yearSub)
                       && !string.IsNullOrEmpty(t1301.OVC_PUR_DCANPO)
                       && (t1301P.OVC_AUDIT_UNIT.Equals(dept) || t1301P.OVC_PUR_APPROVE_DEP.Equals("A"))
               orderby t1202G.OVC_PURCH
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
                   t1202G.OVC_PURCH,
                   t1202G.OVC_CHECKER,
                   t1202G.ONB_CHECK_TIMES,
                   t1202G.OVC_DRECEIVE,
                   t1301.OVC_PUR_DCANPO,
                   t1301.OVC_PUR_DCANRE,
               };
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                FCommon.GridView_dataImport(GV_OVC, dt, field);
            }
            else
            {
                var query =
                from t1202G in group1202
                join t1301 in mpms.TBM1301 on t1202G.OVC_PURCH equals t1301.OVC_PURCH
                join t1301P in mpms.TBM1301_PLAN on t1301.OVC_PURCH equals t1301P.OVC_PURCH
                where t1301.OVC_PURCH.Substring(2, 2).Equals(yearSub)
                        && !string.IsNullOrEmpty(t1301.OVC_PUR_DCANPO)
                        && t1301P.OVC_AUDIT_UNIT.Equals(dept)
                orderby t1202G.OVC_PURCH
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
                    t1202G.OVC_PURCH,
                    t1202G.OVC_CHECKER,
                    t1202G.ONB_CHECK_TIMES,
                    t1202G.OVC_DRECEIVE,
                    t1301.OVC_PUR_DCANPO,
                    t1301.OVC_PUR_DCANRE,
                };
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                FCommon.GridView_dataImport(GV_OVC, dt, field);
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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            FCommon.Controls_Clear(lblOVC_PUR_IPURCH, txtOVC_PURCH, lblOVC_PUR_USER, lblOVC_STATUS, txtOVC_DCANCEL
                    , txtOVC_PUR_DCANRE, lblOVC_PUR_DAPPROVE, lblOVC_PUR_APPROVE);
            trCancelDate.Visible = false;
            trAPPROVE.Visible = false;
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
                         tAccount.DEPT_SN
                     }).FirstOrDefault();
                ViewState["DEPT_SN"] = query.DEPT_SN;
            }
        }
        #endregion
    }


}