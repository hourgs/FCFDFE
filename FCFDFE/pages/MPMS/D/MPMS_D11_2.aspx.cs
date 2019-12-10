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
    public partial class MPMS_D11_2 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strOVC_PURCH, strUserName,strAGENCY = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (FCommon.getQueryString(this, "OVC_PURCH", out strOVC_PURCH, false))
                {
                    TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strAGENCY = tbm1301 == null? "" : tbm1301.OVC_PUR_AGENCY;
                    lblOVC_PURCH_A.Text = strOVC_PURCH + strAGENCY;
                    if (isPURCHASE_UNIT() && !IsPostBack)
                        DataImport();
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('錯誤訊息：未輸入購案編號');", true);
            }
        }


        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Write("<script language='javascript'>window.close();</script>");
        }


        #region 副程式
        private void DataImport()
        {
            DataTable dt = new DataTable();
            TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbmRECEIVE_BID != null)
            {
                var query =
                    from tbmSTATUS in mpms.TBMSTATUS
                    where tbmSTATUS.OVC_PURCH.Equals(strOVC_PURCH)
                    orderby tbmSTATUS.OVC_STATUS == null ? "0".Length : tbmSTATUS.OVC_STATUS == "3" ? "30".Length : tbmSTATUS.OVC_STATUS.Length,tbmSTATUS.OVC_STATUS
                    select new
                    {
                        OVC_PURCH = tbmSTATUS.OVC_PURCH,
                        OVC_PURCH_A_5 = "",
                        ONB_TIMES = tbmSTATUS.ONB_TIMES,
                        OVC_DO_NAME = tbmSTATUS.OVC_DO_NAME,
                        OVC_STATUS = tbmSTATUS.OVC_STATUS,
                        OVC_STATUS_Desc = "",
                        OVC_DBEGIN = tbmSTATUS.OVC_DBEGIN,
                        OVC_DEND = tbmSTATUS.OVC_DEND,
                    };
                dt = CommonStatic.LinqQueryToDataTable(query);

                foreach (DataRow rows in dt.Rows)
                {
                    string strStatus = rows["OVC_STATUS"].ToString();
                    rows["OVC_PURCH_A_5"] = lblOVC_PURCH_A.Text + tbmRECEIVE_BID.OVC_PURCH_5;

                    TBMSTATUS tbmSTATUS = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                && tb.OVC_STATUS.Equals(strStatus)).FirstOrDefault();
                    if (tbmSTATUS != null)
                    {
                        rows["OVC_STATUS_Desc"] = GetTbm1407Desc("Q9", tbmSTATUS.OVC_STATUS);
                        rows["OVC_DBEGIN"] = GetTaiwanDate(tbmSTATUS.OVC_DBEGIN);
                        rows["OVC_DEND"] = GetTaiwanDate(tbmSTATUS.OVC_DEND);
                    }
                }
                ViewState["Status"] = FCommon.GridView_dataImport(gvSTATUS, dt);
            }
        }


        private bool isPURCHASE_UNIT()
        {
            //檢查使用者是否為該購案採購發包的部門
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strDept = "";
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
                        gvSTATUS.Visible = true;
                        return true;
                    }
                }
            }
            gvSTATUS.Visible = false;
            return false;
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


        private String GetTbm1407Desc(string cateID, string codeID)
        {
            //將TBM1407片語ID代碼轉為Desc
            TBM1407 tbm1407 = new TBM1407();
            if (codeID != null && codeID != "")
            {
                tbm1407 = gm.TBM1407.Where(tb => tb.OVC_PHR_CATE.Equals(cateID) && tb.OVC_PHR_ID.Equals(codeID)).OrderBy(tb => tb.OVC_PHR_ID).FirstOrDefault();
                if (tbm1407 != null)
                {
                    return tbm1407.OVC_PHR_DESC.ToString();
                }
            }
            return codeID;
        }



        #endregion


    }
}