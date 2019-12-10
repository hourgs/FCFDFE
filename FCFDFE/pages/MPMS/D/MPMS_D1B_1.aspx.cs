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
    public partial class MPMS_D1B_1 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (FCommon.getQueryString(this, "OVC_PURCH", out string strOVC_PURCH, false))
                {
                    if(IsOVC_DO_NAME())
                        dataimport();
                }
                else
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
            }
                
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

        private bool IsOVC_DO_NAME()
        {
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strUserName, strDept = "";
            string strOVC_PURCH = Request.QueryString["OVC_PURCH"].ToString();
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

        private void dataimport()
        {
            string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
            string strOVC_PURCH_5_url = Request.QueryString["OVC_PURCH_5"] == null ? "" : Request.QueryString["OVC_PURCH_5"];
            string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
            string strOVC_PURCH_5_GROUP = strOVC_PURCH_5_url != "" ? System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_5_url)) : "";
            string strOVC_PURCH_5 = strOVC_PURCH_5_GROUP != "" ? strOVC_PURCH_5_GROUP.Substring(0, 3) : "";
            string strONB_GROUP = strOVC_PURCH_5_GROUP != "" ? strOVC_PURCH_5_GROUP.Substring(3) : "";
            int onbgroup = Convert.ToInt32(strONB_GROUP);
            
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            if (query1301 != null)
            {
                lblOVC_PURCH.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                lblOVC_PUR_IPURCH.Text = query1301.OVC_PUR_IPURCH;
                lblOVC_PUR_NSECTION.Text = query1301.OVC_PUR_NSECTION;
                int onbbudgetbuy = Convert.ToInt32(query1301.ONB_PUR_BUDGET);//採購金額
                if (query1301.ONB_RESERVE_AMOUNT != null)//採購金額計算
                    onbbudgetbuy = Convert.ToInt32(query1301.ONB_PUR_BUDGET) + Convert.ToInt32(query1301.ONB_RESERVE_AMOUNT);
                lblOVC_BUDGET_BUY.Text = onbbudgetbuy.ToString();
                txtONB_PUR_BUDGET.Text = query1301.ONB_PUR_BUDGET.ToString();
                lblOVC_PUR_CURRENT.Text = GetTbm1407Desc("B0", query1301.OVC_PUR_CURRENT == null ? "" : query1301.OVC_PUR_CURRENT);
                string strC7 = GetTbm1407Desc("C7", query1301.OVC_PUR_ASS_VEN_CODE == null ? "" : query1301.OVC_PUR_ASS_VEN_CODE);
            }

            var queryresult =
                (from tbmresult in mpms.TBMBID_RESULT
                 where tbmresult.OVC_PURCH == strOVC_PURCH
                 where tbmresult.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmresult.ONB_GROUP == onbgroup
                 select tbmresult).FirstOrDefault();
            if (queryresult != null)
            {
                lblOVC_VEN_TITLE.Text = queryresult.OVC_VENDORS_NAME;
            }

            var query1303 =
                (from tbm1303 in mpms.TBM1303
                 where tbm1303.OVC_PURCH == strOVC_PURCH
                 where tbm1303.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbm1303.ONB_GROUP == onbgroup
                 select tbm1303).FirstOrDefault();
            if (query1303 != null)
            {
                lblOVC_DOPEN.Text = ToTaiwanCalendar(query1303.OVC_DOPEN);
                lblOVC_OPEN_HOUR.Text = query1303.OVC_OPEN_HOUR + "時";
                lblOVC_OPEN_MIN.Text = query1303.OVC_OPEN_MIN + "分";
                lblONB_BID_RESULT.Text = query1303.ONB_BID_RESULT.ToString();
                lblONB_BID_BUDGET.Text = query1303.ONB_BID_BUDGET.ToString();
                int onbremainbudget = Convert.ToInt32(query1303.ONB_BID_BUDGET) - Convert.ToInt32(query1303.ONB_BID_RESULT);//標餘款
                lblONB_REMAIN_BUDGET.Text = onbremainbudget.ToString();
                lblONB_TIMES.Text = "第" + query1303.ONB_TIMES + "次開標";
                lblOVC_RESULT_CURRENT.Text = GetTbm1407Desc("B0", query1303.OVC_RESULT_CURRENT == null ? "" : query1303.OVC_RESULT_CURRENT);
                lblOVC_BID_CURRENT.Text = GetTbm1407Desc("B0", query1303.OVC_BID_CURRENT == null ? "" : query1303.OVC_BID_CURRENT);
                string strR8 = GetTbm1407Desc("R8", query1303.OVC_OPEN_METHOD == null ? "" : query1303.OVC_OPEN_METHOD);
            }

            var queryRESULT =
                (from result in mpms.TBMBID_RESULT
                 where result.OVC_PURCH == strOVC_PURCH
                 where result.OVC_PURCH_5 == strOVC_PURCH_5
                 where result.ONB_GROUP == onbgroup
                 select result).FirstOrDefault();
            if (queryRESULT != null)
            {
                txtOVC_DESC.Text = queryRESULT.OVC_DESC;
                txtOVC_MEETING.Text = queryRESULT.OVC_MEETING;
                txtOVC_ADVICE.Text = queryRESULT.OVC_DRAFT;
            }

            var query1313 =
                (from tbm1313 in mpms.TBM1313
                 where tbm1313.OVC_PURCH == strOVC_PURCH
                 where tbm1313.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbm1313.ONB_GROUP == onbgroup
                 select tbm1313).FirstOrDefault();

            /*
            
            var query1407R8 =
                (from tbm1407 in mpms.TBM1407
                 where tbm1407.OVC_PHR_CATE == "R8"
                 where tbm1407.OVC_PHR_ID == stropenmthod
                 select tbm1407).FirstOrDefault();
                

            var query1407B0B =
                (from tbm1407 in mpms.TBM1407
                 where tbm1407.OVC_PHR_CATE == "B0"
                 where tbm1407.OVC_PHR_ID == strOVC_BID_CURRENT
                 select tbm1407).FirstOrDefault();
                 
            var query1407B0R =
                (from tbm1407 in mpms.TBM1407
                 where tbm1407.OVC_PHR_CATE == "B0"
                 where tbm1407.OVC_PHR_ID == strOVC_RESULT_CURRENT
                 select tbm1407).FirstOrDefault();                

            var query1407C7 =
                (from tbm1407 in mpms.TBM1407
                 where tbm1407.OVC_PHR_CATE == "C7"
                 where tbm1407.OVC_PHR_ID == strpurass
                 select tbm1407).FirstOrDefault();
            string strC7 = query1407C7.OVC_PHR_DESC;
            
            var query1407B0 =
                (from tbm1407 in mpms.TBM1407
                 where tbm1407.OVC_PHR_CATE == "B0"
                 where tbm1407.OVC_PHR_ID == strOVC_PUR_CURRENT
                 select tbm1407).FirstOrDefault();
            lblOVC_PUR_CURRENT.Text = query1407B0.OVC_PHR_DESC;
            */
        }


        private String GetTbm1407Desc(string cateID, string codeID)
        {
            //將TBM1407片語ID代碼轉為Desc
            TBM1407 tbm1407 = new TBM1407();
            if (codeID != null && codeID != "")
            {
                tbm1407 = gm.TBM1407.Where(table => table.OVC_PHR_CATE.Equals(cateID) && table.OVC_PHR_ID.Equals(codeID)).FirstOrDefault();
                if (tbm1407 != null)
                {
                    return tbm1407.OVC_PHR_DESC.ToString();
                }
            }
            return codeID;
        }

    }
}