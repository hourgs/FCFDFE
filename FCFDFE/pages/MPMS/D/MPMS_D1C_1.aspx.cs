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
    public partial class MPMS_D1C_1 : System.Web.UI.Page
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
                        dataEdit();
                }
            }
        }

        private bool IsOVC_DO_NAME()
        {
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strUserName, strDept = "";

            string strOVC_PURCH_P_5 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(Request.QueryString["OVC_PURCH"]));
            string strOVC_PURCH = strOVC_PURCH_P_5.Substring(0, 7);
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


        private void dataEdit()
        {
            string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
            string strONB_GROUP_url = Request.QueryString["ONB_GROUP"];
            string strOVC_PURCH_P_5 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
            string strONB_GROUP_6 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strONB_GROUP_url));
            string strOVC_PURCH = strOVC_PURCH_P_5.Substring(0,7);
            string strOVC_PURCH_5 = strOVC_PURCH_P_5.Substring(7);
            string strONB_GROUP = strONB_GROUP_6.Substring(0,1);
            string strOVC_PURCH_6 = strONB_GROUP_6.Substring(1);
            int onbgroup = Convert.ToInt32(strONB_GROUP);

            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            var queryannounce =
                (from tbmannounce in mpms.TBMRESULT_ANNOUNCE
                 where tbmannounce.OVC_PURCH == strOVC_PURCH
                 where tbmannounce.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmannounce.OVC_PURCH_6 == strOVC_PURCH_6
                 select tbmannounce).FirstOrDefault();
            lblDSEND.Text = queryannounce.OVC_DSEND;
            lblOVC_PURCH.Text = query1301.OVC_PURCH + query1301.OVC_PUR_AGENCY + queryannounce.OVC_PURCH_5 + queryannounce.OVC_PURCH_6;
            lblOVC_PUR_IPURCH.Text = query1301.OVC_PUR_IPURCH;
            lblOVC_PART_IPURCH.Text = queryannounce.OVC_PART_IPURCH;//部分決標項目或數量
            string strOVC_PUR_ASS_VEN_CODE = queryannounce.OVC_PUR_ASS_VEN_CODE;//招標方式
            if (strOVC_PUR_ASS_VEN_CODE == "A")
            {
                lblOVC_PUR_ASS_VEN_CODE.Text = "公開招標";
            }
            else if (strOVC_PUR_ASS_VEN_CODE == "B")
            {
                lblOVC_PUR_ASS_VEN_CODE.Text = "選擇性招標";
            }
            else if (strOVC_PUR_ASS_VEN_CODE == "C")
            {
                lblOVC_PUR_ASS_VEN_CODE.Text = "限制性招標公告徵求";
            }
            else if (strOVC_PUR_ASS_VEN_CODE == "D")
            {
                lblOVC_PUR_ASS_VEN_CODE.Text = "限制性招標公開評選";
            }
            else if (strOVC_PUR_ASS_VEN_CODE == "E")
            {
                lblOVC_PUR_ASS_VEN_CODE.Text = "公開取得廠商報價或企畫書";
            }

            string strOVC_STATUS = queryannounce.OVC_STATUS; //執行現況
            if (strOVC_STATUS == "Y")
            {
                lblOVC_STATUS.Text = "已決標";
            }
            else
                lblOVC_STATUS.Text = "部分決標";
            lblOVC_DBID.Text = queryannounce.OVC_DBID;
            lblONB_BUDGET.Text = queryannounce.ONB_PUR_BUDGET.ToString();//預算金額
            lblONB_BID_BUDGET.Text = queryannounce.ONB_BID_BUDGET.ToString();//底價
            lblONB_BID_RESULT.Text = queryannounce.ONB_BID_RESULT.ToString();//總決標金額
            lblONB_BID_VENDORS.Text = queryannounce.ONB_BID_VENDORS.ToString();
            lblONB_RESULT_VENDORS.Text = queryannounce.ONB_RESULT_VENDORS.ToString();
            lblOVC_NONE_VENDORS.Text = queryannounce.OVC_NONE_VENDORS;//未得標

            //得標廠商資料
            var queryvendor =
                (from tbmvendor in mpms.TBMANNOUNCE_VENDOR
                 where tbmvendor.OVC_PURCH == strOVC_PURCH
                 where tbmvendor.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmvendor.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmvendor.ONB_GROUP == onbgroup
                 select tbmvendor).FirstOrDefault();
            lblOVC_VEN_TITLE.Text = queryvendor.OVC_VEN_TITLE;
            lblOVC_VEN_CST.Text = queryvendor.OVC_VEN_CST;
            lblOVC_VEN_TITLE_1.Text = queryvendor.OVC_VEN_TITLE;
            lblOVC_VEN_ADDRESS.Text = queryvendor.OVC_VEN_ADDRESS;
            lblOVC_VEN_TEL.Text = queryvendor.OVC_VEN_TEL;
            string strover = queryvendor.OVC_EMPLOYEE_OVER;
            if (strover == "Y")
            {
                Panel1.Visible = true;
                lblOVER.Text = "是";
                lblONB_EMPLOYEES.Text = queryvendor.ONB_EMPLOYEES.ToString();
                lblONB_EMPLOYEES_SPECIAL.Text = queryvendor.ONB_EMPLOYEES_SPECIAL.ToString();
                lblONB_EMPLOYEES_ABORIGINAL.Text = queryvendor.ONB_EMPLOYEES_ABORIGINAL.ToString();
            }
            if (strover == "N")
            {
                Panel1.Visible = false;
                lblOVER.Text = "否";
            }

            lblVEN_KIND.Text = queryvendor.OVC_VEN_KIND;
            lblONB_BID_RESULT_MERG.Text = queryvendor.ONB_BID_RESULT_MERG.ToString();
            string strOVC_MIDDLE_SMALL = queryvendor.OVC_MIDDLE_SMALL;
            if (strOVC_MIDDLE_SMALL == "Y")
            {
                lblOVC_MIDDLE_SMALL.Text = "是";
            }
            else
            {
                lblOVC_MIDDLE_SMALL.Text = "否";
            }

            lblONB_BID_RESULT_1.Text = queryvendor.ONB_BID_RESULT.ToString();
            lblONB_BID_JOB.Text = queryvendor.ONB_BID_JOB.ToString();
            lblOVC_VEN_103_2.Text = queryvendor.OVC_VEN_103_2;
            lblOVC_DESC.Text = queryannounce.OVC_DESC;

            //原產地國別或得標廠商國別
            lblOVC_VEN_COUNTRY.Text = queryvendor.OVC_VEN_COUNTRY;


        }
    }
}