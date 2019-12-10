using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.MTS.C
{
    public partial class MTS_C13_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        DateTime dateNow = DateTime.Now;
        string id;

        #region 副程式
        private void dataImport()
        {
            if (Guid.TryParse(id, out Guid guidCLAIM_SN))
            {
                TBGMT_CLAIM claim = MTSE.TBGMT_CLAIM.Where(table => table.CLAIM_SN.Equals(guidCLAIM_SN)).FirstOrDefault();
                if (claim != null)
                {
                    lblOVC_CLAIM_NO.Text = claim.OVC_CLAIM_NO;
                    txtOVC_COMPENSATION_DATE.Text = FCommon.getDateTime( claim.OVC_COMPENSATION_DATE);
                    txtOVC_COMPENSATION_MSG_NO.Text = claim.OVC_COMPENSATION_MSG_NO;
                    txtONB_COMPENSATION_AMOUNT.Text = claim.ONB_COMPENSATION_AMOUNT.ToString();
                    FCommon.list_setValue(drpOVC_COMPENSATION_CURRENCY, claim.OVC_COMPENSATION_CURRENCY);
                    FCommon.list_setValue(drpOVC_CLAIM_CONDITION, claim.OVC_CLAIM_CONDITION);

                    TBGMT_CLAIM_RESERVE claim_reserve = MTSE.TBGMT_CLAIM_RESERVE.Where(t => t.OVC_INN_NO.Equals(claim.OVC_INN_NO)).FirstOrDefault();
                    if (claim_reserve != null)
                    {
                        txtOVC_INS_COM_MSG.Text = claim_reserve.OVC_INS_COM_MSG;
                        txtONB_COMPENSATION_AMOUNT_2.Text = claim_reserve.ONB_COMPENSATION_AMOUNT.ToString();
                        string strOVC_COMPENSATION_CURRENCY = claim_reserve.OVC_COMPENSATION_CURRENCY ?? VariableMTS.strDefaultCurrency;
                        FCommon.list_setValue(drpOVC_COMPENSATION_CURRENCY_2, strOVC_COMPENSATION_CURRENCY);
                        txtONB_COMPENSATION_CURRENCY_RATE.Text = claim_reserve.ONB_COMPENSATION_CURRENCY_RATE.ToString();
                        txtONB_COMPENSATION_AMOUNT_NTD.Text = claim_reserve.ONB_COMPENSATION_AMOUNT_NTD.ToString();
                        txtOVC_CHEQUE_BANK.Text = claim_reserve.OVC_CHEQUE_BANK;
                        txtOVC_CHEQUE_NO.Text = claim_reserve.OVC_CHEQUE_NO;
                        txtOVC_CHEQUE_TITLE.Text = claim_reserve.OVC_CHEQUE_TITLE;
                        txtOVC_CHEQUE_DATE.Text = FCommon.getDateTime(claim_reserve.OVC_CHEQUE_DATE);
                        txtODT_CLAIM_DATE_2.Text = FCommon.getDateTime(claim_reserve.ODT_CLAIM_DATE);
                    }
                }
                else
                    FCommon.AlertShow(pnMessageModify, "danger", "系統訊息", "索賠通知書 不存在！");
            }
            else
                FCommon.AlertShow(pnMessageModify, "danger", "系統訊息", "索賠通知書 系統編號錯誤！");
        }
        private string getQueryString()
        {
            string strQueryString = "";
            //FCommon.setQueryString(ref strQueryString, "CLAIM_SN", ViewState["CLAIM_SN"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_CLAIM_CONDITION", Request.QueryString["OVC_CLAIM_CONDITION"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_CLAIM_NO", Request.QueryString["OVC_CLAIM_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_DEPT_CDE", Request.QueryString["OVC_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_CLAIM_DATE", Request.QueryString["ODT_CLAIM_DATE"], false);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getQueryString(this, "id", out id, true);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_COMPENSATION_DATE, txtOVC_CHEQUE_DATE, txtODT_CLAIM_DATE_2);
                    #region 匯入下拉式選單
                    CommonMTS.list_dataImport_CURRENCY(drpOVC_COMPENSATION_CURRENCY, false); //幣別
                    CommonMTS.list_dataImport_CLAIM_CONDITION2(drpOVC_CLAIM_CONDITION, true); //索賠情形
                    CommonMTS.list_dataImport_CURRENCY(drpOVC_COMPENSATION_CURRENCY_2, false); //幣別
                    #endregion

                    dataImport();
                }
            }
        }

        #region ~Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strOVC_CLAIM_NO = lblOVC_CLAIM_NO.Text;
            string strOVC_COMPENSATION_DATE = txtOVC_COMPENSATION_DATE.Text;
            string strOVC_COMPENSATION_MSG_NO = txtOVC_COMPENSATION_MSG_NO.Text;
            string strONB_COMPENSATION_AMOUNT = txtONB_COMPENSATION_AMOUNT.Text;
            string strOVC_COMPENSATION_CURRENCY = drpOVC_COMPENSATION_CURRENCY.SelectedItem.Text;
            string strOVC_CLAIM_CONDITION = drpOVC_CLAIM_CONDITION.SelectedItem.Text;

            #region 理賠或撤銷
            string strOVC_INS_COM_MSG = txtOVC_INS_COM_MSG.Text;
            string strONB_COMPENSATION_AMOUNT_2 = txtONB_COMPENSATION_AMOUNT.Text;
            string strOVC_COMPENSATION_CURRENCY_2 = drpOVC_COMPENSATION_CURRENCY_2.Text;
            string strONB_COMPENSATION_CURRENCY_RATE = txtONB_COMPENSATION_CURRENCY_RATE.Text;
            string strONB_COMPENSATION_AMOUNT_NTD = txtONB_COMPENSATION_AMOUNT_NTD.Text;
            string strOVC_CHEQUE_BANK = txtOVC_CHEQUE_BANK.Text;
            string strOVC_CHEQUE_NO = txtOVC_CHEQUE_NO.Text;
            string strOVC_CHEQUE_TITLE = txtOVC_CHEQUE_TITLE.Text;
            string strOVC_CHEQUE_DATE = txtOVC_CHEQUE_DATE.Text;
            string strODT_CLAIM_DATE_2 = txtODT_CLAIM_DATE_2.Text;

            bool boolONB_COMPENSATION_AMOUNT_2 = FCommon.checkDecimal(strONB_COMPENSATION_AMOUNT_2, "實際理賠金額", ref strMessage, out decimal decONB_COMPENSATION_AMOUNT_2);
            bool boolONB_COMPENSATION_CURRENCY_RATE = FCommon.checkDecimal(strONB_COMPENSATION_CURRENCY_RATE, "匯率", ref strMessage, out decimal decONB_COMPENSATION_CURRENCY_RATE);
            bool boolONB_COMPENSATION_AMOUNT_NTD = FCommon.checkDecimal(strONB_COMPENSATION_AMOUNT_NTD, "實際理賠金額(台幣)", ref strMessage, out decimal decONB_COMPENSATION_AMOUNT_NTD);
            bool boolOVC_CHEQUE_DATE = FCommon.checkDateTime(strOVC_CHEQUE_DATE, "支票日期", ref strMessage, out DateTime dateOVC_CHEQUE_DATE);
            bool boolODT_CLAIM_DATE_2 = FCommon.checkDateTime(strODT_CLAIM_DATE_2, "理賠日期", ref strMessage, out DateTime dateODT_CLAIM_DATE_2);
            #endregion

            #region 錯誤訊息
            if (!Guid.TryParse(id, out Guid guidCLAIM_SN))
                strMessage += "<p> 索賠通知書 系統編號錯誤！ </p>";
            TBGMT_CLAIM claim = MTSE.TBGMT_CLAIM.Where(table => table.CLAIM_SN.Equals(guidCLAIM_SN)).FirstOrDefault();
            if (claim == null)
                strMessage += "<p> 索賠通知書 不存在！ </p>";
            if (strOVC_COMPENSATION_DATE.Equals(string.Empty))
                strMessage += "<p> 請選取 理賠日期！ </p>";
            if (strOVC_COMPENSATION_MSG_NO.Equals(string.Empty))
                strMessage += "<p> 請輸入 理賠文號！ </p>";
            if (strONB_COMPENSATION_AMOUNT.Equals(string.Empty))
                strMessage += "<p> 請輸入 理賠金額！ </p>";

            bool boolOVC_COMPENSATION_DATE = FCommon.checkDateTime(strOVC_COMPENSATION_DATE, "理賠日期", ref strMessage, out DateTime dateOVC_COMPENSATION_DATE);
            bool boolONB_COMPENSATION_AMOUNT = FCommon.checkDecimal(strONB_COMPENSATION_AMOUNT, "理賠金額", ref strMessage, out decimal decONB_COMPENSATION_AMOUNT);
            #endregion



            if (strMessage.Equals(string.Empty))
            {
                claim.OVC_COMPENSATION_DATE = dateOVC_COMPENSATION_DATE;
                claim.OVC_COMPENSATION_MSG_NO = strOVC_COMPENSATION_MSG_NO;
                claim.ONB_COMPENSATION_AMOUNT = decONB_COMPENSATION_AMOUNT;
                claim.OVC_COMPENSATION_CURRENCY = strOVC_COMPENSATION_CURRENCY;
                claim.OVC_CLAIM_CONDITION = strOVC_CLAIM_CONDITION;
                claim.ODT_MODIFY_DATE = dateNow;
                claim.OVC_MODIFY_LOGIN_ID = strUserId;
                claim.OVC_IS_COMPENSATED = "已理賠";

                MTSE.SaveChanges();
                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), claim.GetType().Name.ToString(), this, "修改");
                FCommon.AlertShow(pnMessageModify, "success", "系統訊息", $"通知書更新成功，通知書編號：{ strOVC_CLAIM_NO }。");

                #region 理賠或撤銷
                TBGMT_CLAIM_RESERVE claim_reserve = MTSE.TBGMT_CLAIM_RESERVE.Where(t => t.OVC_INN_NO.Equals(claim.OVC_INN_NO)).FirstOrDefault();
                if (claim_reserve != null)
                {
                    claim_reserve.OVC_INS_COM_MSG = strOVC_INS_COM_MSG;
                    claim_reserve.ONB_COMPENSATION_AMOUNT = decONB_COMPENSATION_AMOUNT_2; //空值補0
                    claim_reserve.OVC_COMPENSATION_CURRENCY = strOVC_COMPENSATION_CURRENCY_2;
                    claim_reserve.ONB_COMPENSATION_CURRENCY_RATE = decONB_COMPENSATION_CURRENCY_RATE; //空值補0
                    claim_reserve.ONB_COMPENSATION_AMOUNT_NTD = decONB_COMPENSATION_AMOUNT_NTD; //空值補0
                    claim_reserve.OVC_CHEQUE_BANK = strOVC_CHEQUE_BANK;
                    claim_reserve.OVC_CHEQUE_NO = strOVC_CHEQUE_NO;
                    claim_reserve.OVC_CHEQUE_TITLE = strOVC_CHEQUE_TITLE;
                    if (boolOVC_CHEQUE_DATE) claim_reserve.OVC_CHEQUE_DATE = dateOVC_CHEQUE_DATE; else claim_reserve.OVC_CHEQUE_DATE = null;
                    if (boolODT_CLAIM_DATE_2) claim_reserve.ODT_CLAIM_DATE = dateODT_CLAIM_DATE_2; else claim_reserve.ODT_CLAIM_DATE = null;
                    MTSE.SaveChanges();
                }
                #endregion
            }
            else
                FCommon.AlertShow(pnMessageModify, "danger", "系統訊息", strMessage);
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_C13_1{ getQueryString() }");
        }
        #endregion
    }
}