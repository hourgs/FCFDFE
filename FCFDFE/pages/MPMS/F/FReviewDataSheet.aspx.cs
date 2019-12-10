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
    public partial class FReviewDataSheet : Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strDateFormat = Variable.strDateFormat;
        public string strMenuName = "", strMenuNameItem = "";
        public string strDEPT_SN, strDEPT_Name;
        string strUserID, strPurchNum;
        int numCheckTimes;

        #region 副程式
        private void dataImport()
        {
            var query =
                (from t in mpms.TBM1301
                 join t1202 in mpms.TBM1202 on t.OVC_PURCH equals t1202.OVC_PURCH
                 join tDept in mpms.TBMDEPTs on t1202.OVC_CHECK_UNIT equals tDept.OVC_DEPT_CDE
                 where t.OVC_PURCH.Equals(strPurchNum)
                 where t1202.ONB_CHECK_TIMES == numCheckTimes
                 where t1202.OVC_CHECK_UNIT.Equals(strDEPT_SN)
                 //where t1202.OVC_DRECEIVE.Equals(strDRecive)
                 select new
                 {
                     t.OVC_PURCH,
                     t.OVC_PUR_AGENCY,
                     OVC_CHECK_OK = t1202.OVC_CHECK_OK ?? "N",
                     t1202.OVC_DRECEIVE,
                     OVC_DRESULT = t1202.OVC_DRESULT ?? "",
                     tDept.OVC_ONNAME,
                     t1202.OVC_CHECKER
                 }).FirstOrDefault();
            if (query != null)
            {
                string strOVC_DAUDIT_ASSIGN = query.OVC_DRECEIVE;

                lblOVC_PURCH.Text = query.OVC_PURCH + query.OVC_PUR_AGENCY;
                lblONB_CHECK_TIMES.Text = numCheckTimes.ToString();

                string strONB_CHECK_STATUS = "(複審)";
                bool isCHECK_ID = query.OVC_CHECK_OK.Equals("Y"); //是否為確認審
                if (isCHECK_ID)
                {
                    strONB_CHECK_STATUS = "(確認審)";
                    GV_ADVICE_Import(strOVC_DAUDIT_ASSIGN); //載入 擬辦事項
                }
                else if (numCheckTimes == 1)
                    strONB_CHECK_STATUS = "(初審)";
                lblONB_CHECK_STATUS.Text = strONB_CHECK_STATUS;
                pnADVICE.Visible = isCHECK_ID; //顯示 擬辦事項

                lblOVC_DAUDIT_ASSIGN.Text = FCommon.getTaiwanDate(strOVC_DAUDIT_ASSIGN, "{0}年{1}月{2}日");
                lblOVC_ONNAME.Text = query.OVC_ONNAME;
                lblOVC_CHECKER.Text = query.OVC_CHECKER;
                lblOVC_CHECK_OK.Text = query.OVC_CHECK_OK;
                lblOVC_DRESULT.Text = query.OVC_DRESULT;
            }

            dataImport_Opinion_MEMO(); //讀取 綜審意見
            dataImport_Approved_MEMO(); //讀取 核定事項

            GV_C_Alreadyupdate_Import(); //已上傳檔案
            RepeaterHeaderImport(); //讀取意見
        }
        private void dataImport_Opinion_MEMO()
        {
            int count = 1;
            var query =
                mpms.TBM1202_7
                .Where
                (o => o.OVC_PURCH.Equals(strPurchNum)
                    && o.OVC_CHECK_UNIT.Equals(strDEPT_SN)
                    && o.ONB_CHECK_TIMES == numCheckTimes)
                    .OrderBy(o => o.OVC_IKIND).ThenBy(o => o.ONB_NO);
            string strOpinion_MEMO = "";
            if (query.Any())
                foreach (var item in query)
                {
                    strOpinion_MEMO += $"<p>{ count.ToString() }. { item.OVC_MEMO  }</p>";
                    count++;
                }
            lblOpinion_MEMO.Text = strOpinion_MEMO;
        }
        private void dataImport_Approved_MEMO()
        {
            int count = 1;
            var query =
                mpms.TBM1202_6
                .Where
                (o => o.OVC_PURCH.Equals(strPurchNum)
                    && o.OVC_CHECK_UNIT.Equals(strDEPT_SN)
                    && o.ONB_CHECK_TIMES == numCheckTimes)
                    .OrderBy(o => o.OVC_IKIND).ThenBy(o => o.ONB_NO);
            string strApproved_MEMO = "";
            if (query.Any())
                foreach (var item in query)
                {
                    strApproved_MEMO += $"<p>{ count.ToString() }. { item.OVC_MEMO  }</p>";
                    count++;
                }
            lblApproved_MEMO.Text += strApproved_MEMO;
        }
        private void GV_ADVICE_Import(string strDRecive)
        {
            var query1407_Q5 = from t1407 in mpms.TBM1407 where t1407.OVC_PHR_CATE.Equals("Q5") select t1407;
            var query =
                from t in mpms.TBM1202_ADVICE
                join t1407 in query1407_Q5 on t.OVC_ITEM equals t1407.OVC_PHR_ID
                where t.OVC_PURCH.Equals(strPurchNum) 
                where t.OVC_DRECEIVE.Equals(strDRecive)
                select new
                {
                    OVC_ITEM = t1407.OVC_PHR_DESC,
                    t.OVC_ITEM_ADVICE,
                    t.OVC_ITEM_DESC
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows_ADVICE"] = FCommon.GridView_dataImport(GV_ADVICE, dt);
        }

        private void GV_C_Alreadyupdate_Import()
        {
            var query =
                from t in mpms.TBM1119
                where t.OVC_PURCH.Equals(strPurchNum)
                select new
                {
                    OVC_IKIND = t.OVC_IKIND == "D" ? "採購計畫清單" : "物資申請書",
                    t.OVC_ATTACH_NAME,
                    t.OVC_FILE_NAME,
                    t.ONB_QTY,
                    t.ONB_PAGES
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows_Alreadyupdate"] = FCommon.GridView_dataImport(GV_C_Alreadyupdate, dt);
        }

        private void RepeaterHeaderImport()
        {
            //第一層repeater
            var query =
                (from t12021 in mpms.TBM1202_1
                 join t1407 in mpms.TBM1407 on t12021.OVC_AUDIT_UNIT equals t1407.OVC_PHR_ID
                 join tAccount in mpms.ACCOUNTs
                 on new { name = t12021.OVC_AUDITOR, unit = t12021.OVC_CHECK_UNIT }
                 equals new { name = tAccount.USER_NAME, unit = tAccount.DEPT_SN }
                 where t12021.OVC_PURCH.Equals(strPurchNum)
                 where t12021.ONB_CHECK_TIMES == numCheckTimes
                 where t12021.OVC_CHECK_UNIT.Equals(strDEPT_SN)// && t12021.OVC_DRECEIVE.Equals(strDRecive)
                 where t1407.OVC_PHR_CATE.Equals("K5")
                 where tAccount.IUSER_PHONE != null
                 orderby t1407.OVC_PHR_ID
                 select new
                 {
                     t12021.OVC_AUDIT_UNIT,
                     t1407.OVC_USR_ID,
                     t12021.OVC_AUDITOR,
                     tAccount.IUSER_PHONE,
                     t12021.OVC_DAUDIT_ASSIGN,
                     t12021.OVC_DAUDIT
                 }).ToArray();
            DateTime dateTemp;
            var queryFinal =
                from t in query
                select new
                {
                    t.OVC_AUDIT_UNIT,
                    t.OVC_USR_ID,
                    t.OVC_AUDITOR,
                    t.IUSER_PHONE,
                    t.OVC_DAUDIT
                    //OVC_DAUDIT = DateTime.TryParse(t.OVC_DAUDIT, out dateTemp) ? dateTemp.ToString(strDateFormat) : "" //回覆日
                    //PROCESS = DateDiff(t.OVC_DAUDIT_ASSIGN, t.OVC_DAUDIT)
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(queryFinal);
            Repeater_Header.DataSource = dt;
            Repeater_Header.DataBind();
        }
        private DataTable CommentImport(string auditUnit)
        {
            var query =
                from t1202C in mpms.TBM1202_COMMENT
                join tDETAIL in mpms.TBMOPINION_DETAIL
                on new { t1202C.OVC_AUDIT_UNIT, t1202C.OVC_TITLE, t1202C.OVC_TITLE_ITEM, t1202C.OVC_TITLE_DETAIL }
                equals new { tDETAIL.OVC_AUDIT_UNIT, tDETAIL.OVC_TITLE, tDETAIL.OVC_TITLE_ITEM, tDETAIL.OVC_TITLE_DETAIL }
                where t1202C.OVC_PURCH.Equals(strPurchNum)
                where t1202C.ONB_CHECK_TIMES == numCheckTimes
                where t1202C.OVC_CHECK_UNIT.Equals(strDEPT_SN) //&& t1202C.OVC_DRECEIVE.Equals(strDRecive)
                where t1202C.OVC_AUDIT_UNIT.Equals(auditUnit)
                orderby t1202C.ONB_NO, t1202C.OVC_RESPONSE
                select new
                {
                    t1202C.ONB_NO,
                    tDETAIL.OVC_CONTENT,
                    t1202C.OVC_CHECK_REASON,
                    t1202C.OVC_RESPONSE
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            foreach (DataRow rows in dt.Rows)
            {
                rows["OVC_RESPONSE"] = rows["OVC_RESPONSE"].ToString().Replace("<br>", "");
            }
            return dt;
        }

        private string DateDiff(string strdate1, string strdate2)
        {
            if (string.IsNullOrEmpty(strdate1) || string.IsNullOrEmpty(strdate2))
            {
                return "";
            }
            else
            {
                string dateDiff = null;
                DateTime DateTime1 = Convert.ToDateTime(strdate1);
                DateTime DateTime2 = Convert.ToDateTime(strdate2);
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                dateDiff = ts.Days.ToString();
                return dateDiff;
            }

        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getAccountDEPTName(this, out strDEPT_SN, out strDEPT_Name);
                strUserID = Session["userid"].ToString();
                string strCheckTimes;
                if (FCommon.getQueryString(this, "PurNum", out strPurchNum, true) &&
                   (FCommon.getQueryString(this, "CheckTimes", out strCheckTimes, true) && int.TryParse(strCheckTimes, out numCheckTimes)))
                {
                    if (!IsPostBack)
                    {
                        dataImport();
                    }
                }
                else
                    FCommon.MessageBoxShow(this, "購案編號錯誤！", "FPlanAssessmentSA", false);
            }
        }

        protected void Repeater_Header_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hidOVC_AUDIT_UNIT = (HiddenField)e.Item.FindControl("hidOVC_AUDIT_UNIT");
                string strUnit = hidOVC_AUDIT_UNIT.Value;
                Repeater childRepeater = (Repeater)e.Item.FindControl("Repeater_Content");//找到要繫結資料的childRepeater
                if (childRepeater != null)
                {
                    DataTable dt = new DataTable();
                    dt = CommentImport(strUnit);
                    childRepeater.DataSource = dt;
                    childRepeater.DataBind();
                }
            }
        }

        protected void GV_ADVICE_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows_ADVICE"] != null)
            {
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            }
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        protected void GV_C_Alreadyupdate_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows_Alreadyupdate"] != null)
            {
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            }
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}