using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D1D_1 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
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
                        string strOVC_PURCH = Request.QueryString["OVC_PURCH"];
                        string strOVC_PURCH_5_6 = Request.QueryString["OVC_PURCH_5"];
                        string strOVC_PURCH_5 = strOVC_PURCH_5_6.Substring(0, 3);
                        string strOVC_PURCH_6 = strOVC_PURCH_5_6.Substring(3);
                        data(strOVC_PURCH, strOVC_PURCH_5, strOVC_PURCH_6);
                    }
                }
            }
        }

        private bool IsOVC_DO_NAME()
        {
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strUserName, strDept = "";
            string strOVC_PURCH = Request.QueryString["OVC_PURCH"];
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


        private void data(string strOVC_PURCH, string strOVC_PURCH_5, string strOVC_PURCH_6)
        {
            var query1302 =
                (from tbm1302 in mpms.TBM1302
                 where tbm1302.OVC_PURCH == strOVC_PURCH
                 where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbm1302.OVC_PURCH_6 == strOVC_PURCH_6
                 select tbm1302).FirstOrDefault();
            if (query1302 != null)
            {
                lblOVC_PURCH_6.Text = strOVC_PURCH_6;
                lblOVC_VEN_CST.Text = query1302.OVC_VEN_CST;
                lblOVC_VEN_TITLE.Text = query1302.OVC_VEN_TITLE;
                lblOVC_VEN_TEL.Text = query1302.OVC_VEN_TEL;
                lblOVC_VEN_ADDRESS.Text = query1302.OVC_VEN_ADDRESS;
            }
        }
    }
}