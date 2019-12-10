using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.Web.UI;

namespace FCFDFE.pages.MTS.C
{
    public partial class MTS_C12_2 : Page
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
                    lblOVC_CREATE_LOGIN_ID.Text = FCommon.getUserName(claim.OVC_CREATE_LOGIN_ID);
                    string strOVC_DEPT_CDE = claim.OVC_MILITARY_TYPE;
                    txtOVC_DEPT_CDE.Value = strOVC_DEPT_CDE;
                    txtOVC_ONNAME.Text = FCommon.getDeptName(strOVC_DEPT_CDE);
                    txtOVC_BLD_NO.Text = claim.OVC_BLD_NO;
                    txtOVC_PURCH_NO.Text = claim.OVC_PURCH_NO;
                    txtODT_CLAIM_DATE.Text = FCommon.getDateTime(claim.ODT_CLAIM_DATE);
                    txtOVC_CLAIM_MSG_NO.Text = claim.OVC_CLAIM_MSG_NO;
                    txtOVC_INN_NO.Text = claim.OVC_INN_NO;
                    txtOVC_CLAIM_ITEM.Text = claim.OVC_CLAIM_ITEM;
                    txtONB_CLAIM_NUMBER.Text = claim.ONB_CLAIM_NUMBER.ToString();
                    txtONB_CLAIM_AMOUNT.Text = claim.ONB_CLAIM_AMOUNT.ToString();
                    FCommon.list_setValue(drpOVC_CLAIM_CURRENCY, claim.OVC_CLAIM_CURRENCY);
                    txtOVC_CLAIM_REASON.Text = claim.OVC_CLAIM_REASON;
                    FCommon.list_setValue(drpOVC_CLAIM_CONDITION, claim.OVC_CLAIM_CONDITION);
                    //string strCONDITION = claim.OVC_CLAIM_CONDITION;
                    //ListItem item = drpOVC_CLAIM_CONDITION.Items.FindByText(strCONDITION);
                    //if (item != null)
                    //    item.Selected = true;
                    string[] strGET_FILE = claim.GET_FILE.Split(',');
                    foreach (string str in strGET_FILE)
                    {
                        ListItem item = chkGET_FILE.Items.FindByText(str);
                        if (item != null) item.Selected = true;
                    }
                    txtOVC_NOTE.Text = claim.OVC_NOTE;

                    TBGMT_CLAIM_RESERVE claim_reserve = MTSE.TBGMT_CLAIM_RESERVE.Where(t => t.OVC_INN_NO.Equals(claim.OVC_INN_NO)).FirstOrDefault();
                    if (claim_reserve != null)
                    {
                        txtOVC_INS_COM_MSG.Text = claim_reserve.OVC_INS_COM_MSG;
                        txtONB_COMPENSATION_AMOUNT.Text = claim_reserve.ONB_COMPENSATION_AMOUNT.ToString();
                        string strOVC_COMPENSATION_CURRENCY = claim_reserve.OVC_COMPENSATION_CURRENCY ?? VariableMTS.strDefaultCurrency;
                        FCommon.list_setValue(drpOVC_COMPENSATION_CURRENCY, strOVC_COMPENSATION_CURRENCY);
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
                    FCommon.Controls_Attributes("readonly", "true", txtODT_CLAIM_DATE, txtOVC_ONNAME, txtOVC_CHEQUE_DATE, txtODT_CLAIM_DATE_2);
                    #region 匯入下拉式選單
                    CommonMTS.list_dataImport_CURRENCY(drpOVC_CLAIM_CURRENCY, false); //幣別
                    CommonMTS.list_dataImport_CURRENCY(drpOVC_COMPENSATION_CURRENCY, false); //幣別
                    //CommonMTS.list_dataImport_CLAIM_CONDITION(drpOVC_CLAIM_CONDITION, true); //索賠情形
                    CommonMTS.list_dataImport_CLAIM_CONDITION2(drpOVC_CLAIM_CONDITION, true); //索賠情形
                    CommonMTS.list_dataImport_GET_FILE(chkGET_FILE, false); //需備索賠文件
                    #endregion

                    dataImport();
                }
            }
        }

        #region ~Click
        protected void btnClearDept_Click(object sender, EventArgs e)
        {
            FCommon.Controls_Clear(txtOVC_DEPT_CDE, txtOVC_ONNAME);
        }
        protected void btnLoad_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;

            TBGMT_BLD bld = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
            if (strOVC_BLD_NO.Equals(string.Empty))
                strMessage += "<p> 請輸入 提單編號！ </p>";
            else if (bld == null)
                strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 不存在！ </p>";

            if (strMessage.Equals(string.Empty))
            {
                TBGMT_PORTS portArr = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(bld.OVC_ARRIVE_PORT)).FirstOrDefault();
                TBGMT_PORTS portStr = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(bld.OVC_START_PORT)).FirstOrDefault();
                if (portArr != null && portArr.OVC_IS_ABROAD.Equals("國內")) //進口提單
                {
                    var query =
                      from bldT in MTSE.TBGMT_BLD
                      where bldT.OVC_BLD_NO.Equals(strOVC_BLD_NO)
                      join icr in MTSE.TBGMT_ICR on bldT.OVC_BLD_NO equals icr.OVC_BLD_NO
                      join iinn in MTSE.TBGMT_IINN on bldT.OVC_BLD_NO equals iinn.OVC_BLD_NO
                      select icr;
                    TBGMT_ICR ICR = query.FirstOrDefault(); //時程管制簿
                    if (ICR != null)
                    {
                        txtOVC_PURCH_NO.Text = ICR.OVC_PURCH_NO;
                    }
                    else
                        strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 進口資料不完全！ </p>";
                }
                else if (portStr != null && portStr.OVC_IS_ABROAD.Equals("國內")) //出口提單
                {
                    var query =
                      from bldT in MTSE.TBGMT_BLD
                      where bldT.OVC_BLD_NO.Equals(strOVC_BLD_NO)
                      join edf in MTSE.TBGMT_EDF on bldT.OVC_BLD_NO equals edf.OVC_BLD_NO
                      join einn in MTSE.TBGMT_EINN on edf.OVC_EDF_NO equals einn.OVC_EDF_NO
                      select edf;
                    TBGMT_EDF EDF = query.FirstOrDefault(); //外運資料表
                    if (EDF != null)
                    {
                        txtOVC_PURCH_NO.Text = EDF.OVC_PURCH_NO;
                    }
                    else
                        strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 出口資料不完全！ </p>";
                }
                else
                    strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 進出口 判斷錯誤！ </p>";
            }
            if (!strMessage.Equals(string.Empty))
                FCommon.AlertShow(pnMessageModify, "danger", "系統訊息", strMessage);
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strOVC_CLAIM_NO = lblOVC_CLAIM_NO.Text;
            string strOVC_DEPT_CDE = txtOVC_DEPT_CDE.Value;
            //string strOVC_ONNAME = txtOVC_ONNAME.Text;
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_PURCH_NO = txtOVC_PURCH_NO.Text;
            string strODT_CLAIM_DATE = txtODT_CLAIM_DATE.Text;
            string strOVC_CLAIM_MSG_NO = txtOVC_CLAIM_MSG_NO.Text;
            string strOVC_INN_NO = txtOVC_INN_NO.Text;
            string strOVC_CLAIM_ITEM = txtOVC_CLAIM_ITEM.Text;
            string strONB_CLAIM_NUMBER = txtONB_CLAIM_NUMBER.Text;
            string strONB_CLAIM_AMOUNT = txtONB_CLAIM_AMOUNT.Text;
            string strOVC_CLAIM_CURRENCY = drpOVC_CLAIM_CURRENCY.SelectedValue;
            string strOVC_CLAIM_REASON = txtOVC_CLAIM_REASON.Text;
            string strOVC_CLAIM_CONDITION = drpOVC_CLAIM_CONDITION.SelectedValue;
            string strGET_FILE = ""; //需備索賠文件
            string strOVC_NOTE = txtOVC_NOTE.Text;

            #region 理賠或撤銷
            string strOVC_INS_COM_MSG = txtOVC_INS_COM_MSG.Text;
            string strONB_COMPENSATION_AMOUNT = txtONB_COMPENSATION_AMOUNT.Text;
            string strOVC_COMPENSATION_CURRENCY = drpOVC_COMPENSATION_CURRENCY.Text;
            string strONB_COMPENSATION_CURRENCY_RATE = txtONB_COMPENSATION_CURRENCY_RATE.Text;
            string strONB_COMPENSATION_AMOUNT_NTD = txtONB_COMPENSATION_AMOUNT_NTD.Text;
            string strOVC_CHEQUE_BANK = txtOVC_CHEQUE_BANK.Text;
            string strOVC_CHEQUE_NO = txtOVC_CHEQUE_NO.Text;
            string strOVC_CHEQUE_TITLE = txtOVC_CHEQUE_TITLE.Text;
            string strOVC_CHEQUE_DATE = txtOVC_CHEQUE_DATE.Text;
            string strODT_CLAIM_DATE_2 = txtODT_CLAIM_DATE_2.Text;
            
            bool boolONB_COMPENSATION_AMOUNT = FCommon.checkDecimal(strONB_COMPENSATION_AMOUNT, "實際理賠金額", ref strMessage, out decimal decONB_COMPENSATION_AMOUNT);
            bool boolONB_COMPENSATION_CURRENCY_RATE = FCommon.checkDecimal(strONB_COMPENSATION_CURRENCY_RATE, "匯率", ref strMessage, out decimal decONB_COMPENSATION_CURRENCY_RATE);
            bool boolONB_COMPENSATION_AMOUNT_NTD = FCommon.checkDecimal(strONB_COMPENSATION_AMOUNT_NTD, "實際理賠金額(台幣)", ref strMessage, out decimal decONB_COMPENSATION_AMOUNT_NTD);
            bool boolOVC_CHEQUE_DATE = FCommon.checkDateTime(strOVC_CHEQUE_DATE, "支票日期", ref strMessage, out DateTime dateOVC_CHEQUE_DATE);
            bool boolODT_CLAIM_DATE_2 = FCommon.checkDateTime(strODT_CLAIM_DATE_2, "理賠日期", ref strMessage, out DateTime dateODT_CLAIM_DATE_2);
            #endregion

            for (int i = 0; i < chkGET_FILE.Items.Count; i++)
            {
                if (chkGET_FILE.Items[i].Selected)
                    strGET_FILE += chkGET_FILE.Items[i].Text + ",";
            }
            if (!strGET_FILE.Equals(string.Empty))
                strGET_FILE = strGET_FILE.Substring(0, strGET_FILE.Length - 1);

            #region 錯誤訊息
            if (!Guid.TryParse(id, out Guid guidCLAIM_SN))
                strMessage += "<p> 索賠通知書 系統編號錯誤！ </p>";
            TBGMT_CLAIM claim = MTSE.TBGMT_CLAIM.Where(table => table.CLAIM_SN.Equals(guidCLAIM_SN)).FirstOrDefault();
            if (claim == null)
                strMessage += "<p> 索賠通知書 不存在！ </p>";
            if (strOVC_DEPT_CDE.Equals(string.Empty))
                strMessage += "<p> 請選取 申請單位！ </p>";
            if (strODT_CLAIM_DATE.Equals(string.Empty))
                strMessage += "<p> 請選取 索賠日期！ </p>";
            if (strONB_CLAIM_AMOUNT.Equals(string.Empty))
                strMessage += "<p> 請輸入 索賠軍品總額！ </p>";
            if (strOVC_CLAIM_CURRENCY.Equals(string.Empty))
                strMessage += "<p> 請選取 索賠軍品總額 幣別！ </p>";
            if (strOVC_CLAIM_CONDITION.Equals(string.Empty))
                strMessage += "<p> 請選取 索賠情形！ </p>";

            bool boolODT_CLAIM_DATE = FCommon.checkDateTime(strODT_CLAIM_DATE, "索賠日期", ref strMessage, out DateTime dateODT_CLAIM_DATE);
            bool boolONB_CLAIM_NUMBER = FCommon.checkDecimal(strONB_CLAIM_NUMBER, "索賠軍品數量", ref strMessage, out decimal decONB_CLAIM_NUMBER);
            bool boolONB_CLAIM_AMOUNT = FCommon.checkDecimal(strONB_CLAIM_AMOUNT, "索賠軍品總額", ref strMessage, out decimal decONB_CLAIM_AMOUNT);
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                claim.OVC_MILITARY_TYPE = strOVC_DEPT_CDE;
                claim.OVC_BLD_NO = strOVC_BLD_NO;
                claim.OVC_PURCH_NO = strOVC_PURCH_NO;
                claim.ODT_CLAIM_DATE = dateODT_CLAIM_DATE;
                claim.OVC_CLAIM_MSG_NO = strOVC_CLAIM_MSG_NO;
                claim.OVC_INN_NO = strOVC_INN_NO;
                claim.OVC_CLAIM_ITEM = strOVC_CLAIM_ITEM;
                if (boolONB_CLAIM_NUMBER) claim.ONB_CLAIM_NUMBER = decONB_CLAIM_NUMBER; else claim.ONB_CLAIM_NUMBER = null;
                claim.ONB_CLAIM_AMOUNT = decONB_CLAIM_AMOUNT;
                claim.OVC_CLAIM_CURRENCY = strOVC_CLAIM_CURRENCY;
                claim.OVC_CLAIM_REASON = strOVC_CLAIM_REASON;
                claim.OVC_CLAIM_CONDITION = strOVC_CLAIM_CONDITION;
                claim.GET_FILE = strGET_FILE;
                claim.OVC_NOTE = strOVC_NOTE;
                claim.OVC_IS_COMPENSATED = "未理賠";
                claim.ODT_MODIFY_DATE = dateNow;
                claim.OVC_MODIFY_LOGIN_ID = strUserId;
                MTSE.SaveChanges();
                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), claim.GetType().Name.ToString(), this, "修改");

                #region 理賠或撤銷
                TBGMT_CLAIM_RESERVE claim_reserve = MTSE.TBGMT_CLAIM_RESERVE.Where(t => t.OVC_INN_NO.Equals(claim.OVC_INN_NO)).FirstOrDefault();
                if (claim_reserve != null)
                {
                    claim_reserve.OVC_INS_COM_MSG = strOVC_INS_COM_MSG;
                    claim_reserve.ONB_COMPENSATION_AMOUNT = decONB_COMPENSATION_AMOUNT; //空值補0
                    claim_reserve.OVC_COMPENSATION_CURRENCY = strOVC_COMPENSATION_CURRENCY;
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

                FCommon.AlertShow(pnMessageModify, "success", "系統訊息", $"索賠通知書更新成功，索賠通知書編號：{ strOVC_CLAIM_NO }。");
            }
            else
                FCommon.AlertShow(pnMessageModify, "danger", "系統訊息", strMessage);
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_C12_1{ getQueryString() }");
        }
        #endregion
    }
}