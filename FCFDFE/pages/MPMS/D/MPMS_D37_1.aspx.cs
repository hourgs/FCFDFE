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
    public partial class MPMS_D37_1 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["ONB_GROUP"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    if (!IsPostBack && IsOVC_DO_NAME())
                        dataDefault();
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

        private void dataDefault()
        {
            string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
            string strONB_GROUP_url = Request.QueryString["ONB_GROUP"];
 
            string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
            string sONB_GROUP = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strONB_GROUP_url));
            string strOVC_PURCH_5 = sONB_GROUP.Substring(1);
            string strONB_GROUP = sONB_GROUP.Substring(0, 1);

            int onbgroup = Convert.ToInt32(strONB_GROUP);
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            var query1302 =
                (from tbm1302 in mpms.TBM1302
                 where tbm1302.OVC_PURCH == strOVC_PURCH
                 where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbm1302.ONB_GROUP == onbgroup
                 select tbm1302).FirstOrDefault();
            if (query1302 != null)
            {
                dataImport(strOVC_PURCH, strOVC_PURCH_5, onbgroup);
            }
            lblInfo_PurchNo.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
            //合約商
            lblInfo_Group.Text = strONB_GROUP;
            lblOVC_PURCH.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
            lblOVC_PUR_IPURCH.Text = query1301.OVC_PUR_IPURCH;
            lblONB_GROUP.Text = strONB_GROUP; 
        }
        private void dataImport(string strOVC_PURCH, string strOVC_PURCH_5, int strONB_GROUP)
        {
            string str1302current = "", strcurrent="";
            var query1302 =
                (from tbm1302 in mpms.TBM1302
                 where tbm1302.OVC_PURCH == strOVC_PURCH
                 where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbm1302.ONB_GROUP == strONB_GROUP
                 select tbm1302).FirstOrDefault();
            if (query1302 != null)
            {
                lblInfo_Contractor.Text = query1302.OVC_VEN_TITLE;
                lblOVC_PURCH_6.Text = query1302.OVC_PURCH_6;

                lblONB_GROUP.Text = query1302.ONB_GROUP.ToString();
                str1302current = query1302.OVC_BUD_CURRENT;

                lblONB_PUR_BUDGET.Text = query1302.ONB_BUD_MONEY.ToString();
                lblOVC_DBID.Text = query1302.OVC_DBID;
                lblOVC_DOPEN.Text = query1302.OVC_DOPEN;
                lblOVC_DCONTRACT.Text = query1302.OVC_DCONTRACT;
                strcurrent = query1302.OVC_CURRENT;

                lblONB_MCONTRACT.Text = query1302.ONB_MONEY.ToString();
                lblONB_MONEY_DISCOUNT.Text = query1302.ONB_MONEY_DISCOUNT.ToString();
                lblOVC_RECEIVE_PLACE.Text = query1302.OVC_RECEIVE_PLACE;
                lblOVC_SHIP_TIMES.Text = query1302.OVC_SHIP_TIMES;
                lblOVC_PAYMENT.Text = query1302.OVC_PAYMENT;
                lblONB_DELIVERY_TIMES.Text = query1302.ONB_DELIVERY_TIMES.ToString();
                lblOVC_VEN_CST.Text = query1302.OVC_VEN_CST;
                lblOVC_VEN_TITLE.Text = query1302.OVC_VEN_TITLE;
                lblOVC_VEN_FAX.Text = query1302.OVC_VEN_FAX;
                lblOVC_VEN_EMAIL.Text = query1302.OVC_VEN_EMAIL;
                lblOVC_VEN_BOSS.Text = query1302.OVC_VEN_BOSS;
                lblOVC_VEN_TEL.Text = query1302.OVC_VEN_TEL;
                lblOVC_VEN_NAME.Text = query1302.OVC_VEN_NAME;
                lblOVC_VEN_CELLPHONE.Text = query1302.OVC_VEN_CELLPHONE;
                lblOVC_VEN_ADDRESS.Text = query1302.OVC_VEN_ADDRESS;
                lblOVC_CONTRACT_COMM.Text = query1302.OVC_CONTRACT_COMM;
            }

            var query1407B01 =
                (from tbm1407 in mpms.TBM1407
                 where tbm1407.OVC_PHR_CATE == "B0"
                 where tbm1407.OVC_PHR_ID == str1302current
                 select tbm1407).FirstOrDefault();
            lblONB_GROUP_BUDGET.Text = query1407B01 == null ? "" : query1407B01.OVC_PHR_DESC;
            
            var query1407B012 =
                (from tbm1407 in mpms.TBM1407
                 where tbm1407.OVC_PHR_CATE == "B0"
                 where tbm1407.OVC_PHR_ID == strcurrent
                 select tbm1407).FirstOrDefault();
            lblONB_CURRENT.Text = query1407B012 == null ? "" : query1407B012.OVC_PHR_DESC;
            
        }
    }
}