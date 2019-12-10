using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;
using System.Globalization;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D35_1 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_PURCH_5"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    if (!IsPostBack && IsOVC_DO_NAME())
                    {
                        string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
                        string strOVC_PURCH_5_url = Request.QueryString["OVC_PURCH_5"];
                        string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
                        string strOVC_PURCH_5 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_5_url));
                        ViewState["strOVC_PURCH"] = strOVC_PURCH;
                        ViewState["strOVC_PURCH_5"] = strOVC_PURCH_5;

                        var querynone =
                        (from tbmnone in mpms.TBMRESULT_ANNOUNCE_NONE
                         where tbmnone.OVC_PURCH == strOVC_PURCH
                         where tbmnone.OVC_PURCH_5 == strOVC_PURCH_5
                         select tbmnone).FirstOrDefault();

                        dataImport(strOVC_PURCH, strOVC_PURCH_5);
                        dataModify();

                    }
                }
            }
        }

        private bool IsOVC_DO_NAME()
        {
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strUserName, strDept = "";
            string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
            string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
            DataTable dt = new DataTable();
            if (strUSER_ID.Length > 0)
            {
                ACCOUNT ac = new ACCOUNT();
                ac = gm.ACCOUNTs.Where(tb => tb.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                if (ac != null)
                {
                    strUserName = ac.USER_NAME.ToString();
                    strDept = ac.DEPT_SN;
                    TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    if (strOVC_PURCH == "")
                        strErrorMsg = "請選擇購案";
                    else if (tbm1301 == null)
                        strErrorMsg = "查無此購案編號";
                    else
                    {
                        TBM1301_PLAN plan1301 =
                            gm.TBM1301_PLAN.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCHASE_UNIT.Equals(strDept)).FirstOrDefault();

                        TBMRECEIVE_BID tbmRECEIVE_BID =
                            mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_DO_NAME.Equals(strUserName)).FirstOrDefault();
                        if (tbmRECEIVE_BID == null || plan1301 == null)
                            strErrorMsg = "非此購案的承辦人";
                    }

                    if (strErrorMsg != "")
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErrorMsg);
                    else
                    {
                        divForm.Visible = true;
                        return true;
                    }
                }
            }
            divForm.Visible = false;
            return false;
        }

        private void dataImport(string strOVC_PURCH, string strOVC_PURCH_5)
        {
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            lblOVC_PURCH.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
            lblOVC_PUR_IPURCH.Text = query1301.OVC_PUR_IPURCH;
            string strpurass = query1301.OVC_PUR_ASS_VEN_CODE;
            var query1407C7 =
                (from tbm1407 in mpms.TBM1407
                 where tbm1407.OVC_PHR_CATE == "C7"
                 where tbm1407.OVC_PHR_ID == strpurass
                 select tbm1407).FirstOrDefault();
            lblOVC_PUR_ASS_VEN_CODE.Text = query1407C7 == null ? "" : query1407C7.OVC_PHR_DESC;
            var query1303 =
                (from tbm1303 in mpms.TBM1303
                 where tbm1303.OVC_PURCH == strOVC_PURCH
                 where tbm1303.OVC_PURCH_5 == strOVC_PURCH_5
                 select tbm1303).FirstOrDefault();
            if(query1303 != null)
                lblOVC_DOPEN.Text = ToTaiwanCalendar(query1303.OVC_DOPEN) + query1303.OVC_OPEN_HOUR + "時" + query1303.OVC_OPEN_MIN + "分";
        }
        public string ToTaiwanCalendar(string strDate)
        {
            //西元年轉民國年
            if (strDate != null)
            {
                DateTime datetime = Convert.ToDateTime(strDate);
                CultureInfo info = new CultureInfo("zh-TW");
                TaiwanCalendar twC = new TaiwanCalendar();
                info.DateTimeFormat.Calendar = twC;
                return datetime.ToString("yyy年MM月dd日", info);
            }
            return string.Empty;
        }
        private void dataModify()
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();

            var queryannounce =
                (from tbmannounce in mpms.TBMRESULT_ANNOUNCE_NONE
                 where tbmannounce.OVC_PURCH == strOVC_PURCH
                 where tbmannounce.OVC_PURCH_5 == strOVC_PURCH_5
                 select tbmannounce).FirstOrDefault();
            if (queryannounce != null)
            {
                lblONB_BID_VENDORS.Text = queryannounce.ONB_BID_VENDORS.ToString();
                lblDSEND.Text = queryannounce.OVC_DSEND;
                lblOVC_DANNOUNCE_LAST.Text = queryannounce.OVC_DANNOUNCE_LAST;
                lblOVC_RESULT_REASON.Text = queryannounce.OVC_RESULT_REASON;
                lblOVC_CONTINUE.Text = queryannounce.OVC_CONTINUE;
                lblOVC_MEMO.Text = queryannounce.OVC_MEMO;
            }
        }
    }
}