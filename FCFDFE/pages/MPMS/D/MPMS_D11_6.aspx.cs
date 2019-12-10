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
    public partial class MPMS_D11_6 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strOVC_PURCH;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (FCommon.getQueryString(this, "OVC_PURCH", out strOVC_PURCH, false))
                {
                    if (isPURCHASE_UNIT() && !IsPostBack)
                        DataImport();   //將資料帶入畫面
                }
                else
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D11.aspx", false);
            }
        }


        #region 副程式
        private bool isPURCHASE_UNIT()
        {
            //檢查使用者是否為該購案採購發包的部門
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strUserName, strDept = "";
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
                        strErrorMsg = "請輸入購案編號";
                    else if (tbm1301 == null)
                        strErrorMsg = "查無此購案編號";
                    else
                    {
                        var queryOvcPurch =
                            (from tbm1301Plan in gm.TBM1301_PLAN
                             where tbm1301Plan.OVC_PURCHASE_UNIT.Equals(strDept)
                                 && tbm1301Plan.OVC_PURCH.Equals(strOVC_PURCH)
                             join tbm1301_1 in gm.TBM1301 on tbm1301Plan.OVC_PURCH equals tbm1301_1.OVC_PURCH
                             select tbm1301Plan).ToList();

                        if (queryOvcPurch.Count == 0)
                            strErrorMsg = "非此購案的採購發包部門";
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


        private void DataImport()
        {
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strOVC_PURCH_5="";
            DataTable dt = new DataTable();

            if (strUSER_ID.Length > 0)
            {
                ACCOUNT ac = new ACCOUNT();
                string userName = "";
                ac = gm.ACCOUNTs.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                if (ac != null)
                {
                    userName = ac.USER_NAME.ToString();
                }
                
                TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                TBM1301_PLAN plan1301 = gm.TBM1301_PLAN.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                TBM1231 tbm1231 = mpms.TBM1231.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                TBM1220_1 tbm1220_1 = mpms.TBM1220_1.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) 
                                        && tb.OVC_IKIND.Equals("W52")).FirstOrDefault();
                TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                TBMRECEIVE_BID_LOG tbmRECEIVE_BID_LOG = mpms.TBMRECEIVE_BID_LOG.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                TBMRECEIVE_WORK tbmRECEIVE_WORK = mpms.TBMRECEIVE_WORK.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                                            && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)).FirstOrDefault();
                TBMBID_VENDOR tbmBID_VENDOR = mpms.TBMBID_VENDOR.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                TBM1303 tbm1303_1 = mpms.TBM1303.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                                            && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.ONB_TIMES == 1).FirstOrDefault();
                TBM1303 tbm1303_2 = mpms.TBM1303.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                                            && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.ONB_TIMES == 2).FirstOrDefault();
                VIWORKSTATUS21 viWORKSTATUS21 = mpms.VIWORKSTATUS21.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                                            && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)).FirstOrDefault();
                VITBM1302 viTBM1302 = mpms.VITBM1302.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                                            && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)).FirstOrDefault();

                if (tbm1301 != null)
                {
                    //TBM1301
                    lblOVC_PURCH.Text = tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                    lblOVC_PUR_NSECTION.Text = tbm1301.OVC_PUR_NSECTION;
                    lblOVC_PUR_IPURCH.Text = tbm1301.OVC_PUR_IPURCH;
                    lblOVC_PUR_NSECTION.Text = tbm1301.OVC_PUR_NSECTION;
                    lblOVC_PUR_ASS_VEN_CODE.Text = GetTbm1407Desc("C7", tbm1301.OVC_PUR_ASS_VEN_CODE);
                    lblONB_PUR_BUDGET.Text = tbm1301.ONB_PUR_BUDGET.ToString();
                    lblOVC_PUR_CURRENT.Text = GetTbm1407Desc("B0", tbm1301.OVC_PUR_CURRENT);
                    lblONB_PUR_BUDGET.Text = tbm1301.ONB_PUR_BUDGET.ToString();
                    lblOVC_PUR_DAPPROVE.Text = GetTaiwanDate(tbm1301.OVC_PUR_DAPPROVE);
                    lblOVC_DPROPOSE.Text = GetTaiwanDate(tbm1301.OVC_DPROPOSE);
                    


                    if (plan1301 != null)
                    {
                        string strONB_TENDER_DAYS = plan1301.ONB_TENDER_DAYS.ToString();
                        int numONB_REVIEW_DAYS = plan1301.ONB_REVIEW_DAYS == null ? 0 : int.Parse(plan1301.ONB_REVIEW_DAYS.ToString());
                        int numONB_TENDER_DAYS = plan1301.ONB_TENDER_DAYS == null ? 0 : int.Parse(plan1301.ONB_TENDER_DAYS.ToString());
                        lblONB_DELIVER_DAYS.Text = plan1301.ONB_DELIVER_DAYS.ToString();
                        lblOVC_DAPPLY.Text = GetTaiwanDate(plan1301.OVC_DAPPLY);
                        
                        if (plan1301.OVC_DAPPLY != null)
                        {
                            DateTime dateOVC_DAPPLY = Convert.ToDateTime(plan1301.OVC_DAPPLY);
                            DateTime dateOVC_PRE_DAPPROVE = dateOVC_DAPPLY.AddDays(numONB_REVIEW_DAYS);
                            DateTime dateOVC_PRE_CONTRACR = dateOVC_PRE_DAPPROVE.AddDays(numONB_TENDER_DAYS);
                            lblOVC_PRE_DAPPROVE.Text = GetTaiwanDate(dateOVC_PRE_DAPPROVE.ToString("yyyy-MM-dd"));
                            lblOVC_PRE_CONTRACR.Text = GetTaiwanDate(dateOVC_PRE_CONTRACR.ToString("yyyy-MM-dd"));
                        }
                    }


                    if (tbmRECEIVE_BID_LOG != null)
                    {
                        lblOVC_COMM.Text = tbmRECEIVE_BID_LOG.OVC_COMM;
                        lblOVC_COMM_REASON.Text = tbmRECEIVE_BID_LOG.OVC_COMM_REASON;
                    }


                    if (tbm1231 != null)
                    lblOVC_ISOURCE.Text = tbm1231.OVC_ISOURCE;


                    if (tbm1220_1 != null)
                        lblOVC_MEMO.Text = tbm1220_1.OVC_MEMO;


                    if (tbmRECEIVE_WORK != null)
                    {
                        lblOVC_DWAIT.Text = tbmRECEIVE_WORK.OVC_DWAIT;
                        lblOVC_DWAIT_KIND.Text = tbmRECEIVE_WORK.OVC_DWAIT_KIND == null ? "" :GetTbm1407Desc("R1", tbmRECEIVE_WORK.OVC_DWAIT_KIND);
                    }


                    if (tbmBID_VENDOR != null)
                        lblOVC_DOPEN.Text = tbmBID_VENDOR.OVC_DOPEN == null ? "" : GetTaiwanDate(tbmBID_VENDOR.OVC_DOPEN);


                    if (tbm1303_1 != null)
                        lblOVC_DOPEN_1.Text = tbm1303_1.OVC_DOPEN == null ? "" : GetTaiwanDate(tbm1303_1.OVC_DOPEN);


                    if (tbm1303_2 != null)
                        lblOVC_DOPEN_2.Text = tbm1303_2.OVC_DOPEN == null ? "" : GetTaiwanDate(tbm1303_2.OVC_DOPEN);


                    if (viWORKSTATUS21 != null)
                        lblOVC_DBEGIN.Text = viWORKSTATUS21.OVC_DBEGIN == null ? "" : GetTaiwanDate(viWORKSTATUS21.OVC_DBEGIN);


                    if (viTBM1302 != null)
                        lblOVC_DCONTRACT.Text = viTBM1302.OVC_DCONTRACT;

                }
                
            }
            
        }

        private String GetTbm1407Desc(string cateID, string codeID)
        {
            //將TBM1407片語ID代碼轉為Desc
            TBM1407 tbm1407 = new TBM1407();
            if (codeID != null)
            {
                tbm1407 = gm.TBM1407.Where(table => table.OVC_PHR_CATE.Equals(cateID) && table.OVC_PHR_ID.Equals(codeID)).FirstOrDefault();
                if (tbm1407 != null)
                {
                    return tbm1407.OVC_PHR_DESC.ToString();
                }
            }
            return "";
        }


        public string GetTaiwanDate(string strDate)
        {
            //西元年轉民國年
            if (strDate != null && strDate != "")
            {
                DateTime datetime = Convert.ToDateTime(strDate);
                CultureInfo info = new CultureInfo("zh-TW");
                TaiwanCalendar twC = new TaiwanCalendar();
                info.DateTimeFormat.Calendar = twC;
                return datetime.ToString("yyy年MM月dd日", info);
            }
            else
                return strDate;
        }

        #endregion

    }

}