using System;
using System.Linq;
using System.Web.UI;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System.Data;

namespace FCFDFE.pages.MTS.C
{
    public partial class MTS_C15_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        public string strDEPT_SN, strDEPT_Name;
        DateTime dateNow = DateTime.Now;
        string strOtherText = VariableMTS.strOtherText;
        string strDefault_OVC_CLAIM_REASON_NOTE = "請輸入其他損失原因";
        string id;

        #region 副程式
        private void dataImport()
        {
            if (Guid.TryParse(id, out Guid guidRECLAIM_SN))
            {
                TBGMT_CLAIM_RESERVE reserve = MTSE.TBGMT_CLAIM_RESERVE.Where(table => table.RECLAIM_SN.Equals(guidRECLAIM_SN)).FirstOrDefault();
                if (reserve != null)
                {
                    lblOVC_RECLAIM_NO.Text = reserve.OVC_RECLAIM_NO;
                    lblOVC_CREATE_ID.Text = reserve.OVC_CREATE_ID;
                    string strOVC_MILITARY_TYPE = reserve.OVC_MILITARY_TYPE;
                    txtOVC_DEPT_CDE.Value = strOVC_MILITARY_TYPE;
                    txtOVC_ONNAME.Text = FCommon.getDeptName(strOVC_MILITARY_TYPE);
                    txtOVC_APPLY_DATE.Text = FCommon.getDateTime(reserve.OVC_APPLY_DATE);
                    txtOVC_BLD_NO.Text = reserve.OVC_BLD_NO;
                    txtOVC_IMPORT_DATE.Text = FCommon.getDateTime(reserve.OVC_IMPORT_DATE);
                    txtOVC_INN_NO.Text = reserve.OVC_INN_NO;
                    txtOVC_CLAIM_MSG_NO.Text = reserve.OVC_CLAIM_MSG_NO;
                    txtOVC_CLAIM_DATE.Text = FCommon.getDateTime(reserve.OVC_CLAIM_DATE);
                    txtOVC_PURCHASE_MSG_NO.Text = reserve.OVC_PURCHASE_MSG_NO;
                    txtOVC_PURCHASE_DATE.Text = FCommon.getDateTime(reserve.OVC_PURCHASE_DATE);
                    txtOVC_CLAIM_ITEM.Text = reserve.OVC_CLAIM_ITEM;
                    string strOVC_CLAIM_REASON = reserve.OVC_CLAIM_REASON;
                    FCommon.list_setValue(rdoOVC_CLAIM_REASON, strOVC_CLAIM_REASON);
                    //ListItem item = rdoOVC_CLAIM_REASON.Items.FindByText(strREASON);
                    //if (item != null)
                    //    item.Selected = true;
                    bool boolOVC_CLAIM_REASON = setOVC_CLAIM_REASON(); //設定損失原因－相關提示文字
                    if (boolOVC_CLAIM_REASON) txtOVC_CLAIM_REASON_NOTE.Text = reserve.OVC_CLAIM_REASON_NOTE;
                    txtONB_RECEIVE.Text = reserve.ONB_RECEIVE.ToString();
                    txtONB_ACTUAL_RECEIVE.Text = reserve.ONB_ACTUAL_RECEIVE.ToString();
                    txtONB_CLAIM_BREAK.Text = reserve.ONB_CLAIM_BREAK.ToString();
                    txtONB_CLAIM_NUMBER.Text = reserve.ONB_CLAIM_NUMBER.ToString();
                    txtONB_CLAIM_AMOUNT.Text = reserve.ONB_CLAIM_AMOUNT.ToString();
                    string strOVC_CLAIM_CURRENCY = reserve.OVC_CLAIM_CURRENCY ?? VariableMTS.strDefaultCurrency;
                    FCommon.list_setValue(drpOVC_CLAIM_CURRENCY, strOVC_CLAIM_CURRENCY);
                    txtONB_CLAIM_CURRENCY_RATE.Text = reserve.ONB_CLAIM_CURRENCY_RATE.ToString();
                    //drpONB_CLAIM_CURRENCY_RATE.SelectedValue;
                    txtONB_CLAIM_AMOUNT_NTD.Text = reserve.ONB_CLAIM_AMOUNT_NTD.ToString();

                    txtOVC_CLAIM_COM_MSG.Text = reserve.OVC_CLAIM_COM_MSG;
                    txtOVC_CLAIM_COM_DATE.Text = FCommon.getDateTime(reserve.OVC_CLAIM_COM_DATE);
                    txtOVC_PURCHASE_COM_MSG.Text = reserve.OVC_PURCHASE_COM_MSG;
                    txtOVC_PURCHASE_COM_DATE.Text = FCommon.getDateTime(reserve.OVC_PURCHASE_COM_DATE);
                    txtOVC_CLAIM_REV_MSG.Text = reserve.OVC_CLAIM_REV_MSG;
                    txtOVC_CLAIM_REV_DATE.Text = FCommon.getDateTime(reserve.OVC_CLAIM_REV_DATE);
                    txtOVC_PURCHASE_REV_MSG.Text = reserve.OVC_PURCHASE_REV_MSG;
                    txtOVC_PURCHASE_REV_DATE.Text = FCommon.getDateTime(reserve.OVC_PURCHASE_REV_DATE);
                    txtOVC_INS_COM_MSG.Text = reserve.OVC_INS_COM_MSG;
                    txtONB_COMPENSATION_AMOUNT.Text = reserve.ONB_COMPENSATION_AMOUNT.ToString();
                    string strOVC_COMPENSATION_CURRENCY = reserve.OVC_COMPENSATION_CURRENCY ?? VariableMTS.strDefaultCurrency;
                    FCommon.list_setValue(drpOVC_COMPENSATION_CURRENCY, strOVC_COMPENSATION_CURRENCY);
                    txtONB_COMPENSATION_CURRENCY_RATE.Text = reserve.ONB_COMPENSATION_CURRENCY_RATE.ToString();
                    txtONB_COMPENSATION_AMOUNT_NTD.Text = reserve.ONB_COMPENSATION_AMOUNT_NTD.ToString();
                    txtOVC_CHEQUE_BANK.Text = reserve.OVC_CHEQUE_BANK;
                    txtOVC_CHEQUE_NO.Text = reserve.OVC_CHEQUE_NO;
                    txtOVC_CHEQUE_TITLE.Text = reserve.OVC_CHEQUE_TITLE;
                    txtOVC_CHEQUE_DATE.Text = FCommon.getDateTime(reserve.OVC_CHEQUE_DATE);
                    txtOVC_NOTE.Text = reserve.OVC_NOTE;
                    string strOVC_APPROVE_STATUS = reserve.OVC_APPROVE_STATUS;
                    FCommon.list_setValue(drpOVC_APPROVE_STATUS, strOVC_APPROVE_STATUS);
                    //ListItem item2 = drpOVC_APPROVE_STATUS.Items.FindByText(strSTATUS);
                    //if (item2 != null)
                    //    item2.Selected = true;
                    txtOVC_APPROVE_DATE.Text = FCommon.getDateTime(reserve.OVC_APPROVE_DATE);
                    txtODT_CLAIM_DATE_2.Text = FCommon.getDateTime(reserve.ODT_CLAIM_DATE);
                    txtOVC_CLAIM_NO.Text = strOVC_APPROVE_STATUS.Contains("申請理賠") ? MTSE.TBGMT_CLAIM.Where(t => t.OVC_INN_NO.Equals(reserve.OVC_INN_NO)).Select(t => t.OVC_CLAIM_NO).FirstOrDefault() ?? "" : "";
                }
                else
                    FCommon.AlertShow(pnMessageModify, "danger", "系統訊息", "保留索賠權 不存在！");
            }
            else
                FCommon.AlertShow(pnMessageModify, "danger", "系統訊息", "保留索賠權 系統編號錯誤！");
        }
        private bool setOVC_CLAIM_REASON() //設定損失原因－相關提示文字
        {
            string strOVC_CLAIM_REASON = rdoOVC_CLAIM_REASON.SelectedValue;
            bool isOther = false;
            switch (strOVC_CLAIM_REASON)
            {
                case "短少":
                    labelA.Text = " (A) 請輸入";
                    labelB.Text = " (B) 請輸入";
                    labelC.Text = " (C) = A-B";
                    labelD.Text = " (D) 請輸入";
                    break;
                case "破損":
                    labelA.Text = " (A) (請輸入)";
                    labelB.Text = " (B) = A";
                    labelC.Text = " (C) (請輸入)";
                    labelD.Text = " (D) = C";
                    break;
                case "其他":
                    isOther = true;
                    txtOVC_CLAIM_REASON_NOTE.Text = strDefault_OVC_CLAIM_REASON_NOTE;
                    labelA.Text = " (A) (請輸入)";
                    labelB.Text = " (B) (請輸入)";
                    labelC.Text = " (C) (請輸入)";
                    labelD.Text = " (D) = A-B+C";
                    break;
                default:
                    break;
            }
            txtOVC_CLAIM_REASON_NOTE.Visible = isOther;
            return isOther;
        }
        private void setAutoNumber() //計算件數
        {
            string strMessage = "";
            string strOVC_CLAIM_REASON = rdoOVC_CLAIM_REASON.SelectedValue;
            string strONB_RECEIVE = txtONB_RECEIVE.Text; //應收件數
            string strONB_ACTUAL_RECEIVE = txtONB_ACTUAL_RECEIVE.Text; //實收件數
            string strONB_CLAIM_BREAK = txtONB_CLAIM_BREAK.Text; //破損件數
            string strONB_CLAIM_NUMBER = txtONB_CLAIM_NUMBER.Text; //索賠件數

            bool boolONB_RECEIVE = FCommon.checkDecimal(strONB_RECEIVE, "應收件數", ref strMessage, out decimal decONB_RECEIVE);
            bool boolONB_ACTUAL_RECEIVE = FCommon.checkDecimal(strONB_ACTUAL_RECEIVE, "實收件數", ref strMessage, out decimal decONB_ACTUAL_RECEIVE);
            bool boolONB_CLAIM_BREAK = FCommon.checkDecimal(strONB_CLAIM_BREAK, "破損件數", ref strMessage, out decimal decONB_CLAIM_BREAK);
            bool boolONB_CLAIM_NUMBER = FCommon.checkDecimal(strONB_CLAIM_NUMBER, "索賠件數", ref strMessage, out decimal decONB_CLAIM_NUMBER);
            if (strMessage.Equals(string.Empty))
            {
                switch (strOVC_CLAIM_REASON)
                {
                    case "短少":
                        txtONB_CLAIM_BREAK.Text = (decONB_RECEIVE - decONB_ACTUAL_RECEIVE).ToString();
                        break;
                    case "破損":
                        txtONB_ACTUAL_RECEIVE.Text = decONB_RECEIVE.ToString();
                        txtONB_CLAIM_NUMBER.Text = decONB_CLAIM_BREAK.ToString();
                        break;
                    case "其他":
                        txtONB_CLAIM_NUMBER.Text = (decONB_RECEIVE - decONB_ACTUAL_RECEIVE + decONB_CLAIM_BREAK).ToString();
                        break;
                    default:
                        break;
                }
            }
            else
                FCommon.AlertShow(pnMessageModify, "danger", "系統訊息", strMessage);
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_RECLAIM_NO", Request.QueryString["OVC_RECLAIM_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_DEPT_CDE", Request.QueryString["OVC_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", Request.QueryString["OVC_BLD_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_APPROVE_STATUS", Request.QueryString["OVC_APPROVE_STATUS"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_APPLY_DATE_S", Request.QueryString["OVC_APPLY_DATE_S"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_APPLY_DATE_E", Request.QueryString["OVC_APPLY_DATE_E"], false);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getAccountDEPTName(this, out strDEPT_SN, out strDEPT_Name);
                FCommon.getQueryString(this, "id", out id, true);
                txtOVC_CLAIM_REASON_NOTE.Attributes.Add("onfocus", "if(this.value=='請輸入其他損失原因')this.value='';");
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_ONNAME, txtOVC_APPLY_DATE, txtOVC_IMPORT_DATE, txtOVC_CLAIM_DATE,
                        txtOVC_APPROVE_DATE, txtOVC_CHEQUE_DATE, txtOVC_CLAIM_COM_DATE, txtOVC_PURCHASE_COM_DATE, txtOVC_CLAIM_REV_DATE,
                        txtOVC_PURCHASE_REV_DATE, txtOVC_PURCHASE_DATE, txtODT_CLAIM_DATE_2);
                    #region 匯入下拉式選單
                    CommonMTS.list_dataImport_CURRENCY(drpOVC_CLAIM_CURRENCY, false); //幣別
                    CommonMTS.list_dataImport_CURRENCY(drpOVC_COMPENSATION_CURRENCY, false); //幣別
                    CommonMTS.list_dataImport_CLAIM_REASON(rdoOVC_CLAIM_REASON, false); //損失原因
                    CommonMTS.list_dataImport_APPROVE_STATUS(drpOVC_APPROVE_STATUS, false); //作業進度
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
            string strOVC_APPLY_DATE = txtOVC_APPLY_DATE.Text;
            string strODT_ACT_ARRIVE_DATE = ""; //實際抵運日期

            TBGMT_BLD bld = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
            if (strOVC_BLD_NO.Equals(string.Empty))
                strMessage += "<p> 請輸入 提單編號！ </p>";
            else if (bld == null)
                strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 之提單 不存在！ </p>";
            else
            {
                //若輸入提單的進口日期少於11個月內，則正常顯示該提單的[進口日期]與[投保通知書編號]
                //若輸入提單的進口日期大於11個月，則顯示訊息‘申請日期不可大於進口日期+11個月!’若為管理者或ADM身分不在此
                strODT_ACT_ARRIVE_DATE = FCommon.getDateTime(bld.ODT_ACT_ARRIVE_DATE); //實際抵運日期
                if (strOVC_APPLY_DATE.Equals(string.Empty))
                    strMessage += "<p> 請選取 申請日期！ </p>";
                else if (strODT_ACT_ARRIVE_DATE.Equals(string.Empty))
                    strMessage += "<p> 實際抵運日期 不得為空！ </p>";
                else
                {
                    bool boolOVC_APPLY_DATE = FCommon.checkDateTime(strOVC_APPLY_DATE, "申請日期", ref strMessage, out DateTime dateOVC_APPLY_DATE);
                    bool boolODT_ACT_ARRIVE_DATE = FCommon.checkDateTime(strODT_ACT_ARRIVE_DATE, "實際抵運日期", ref strMessage, out DateTime dateODT_ACT_ARRIVE_DATE);
                    if (DateTime.Compare(dateOVC_APPLY_DATE, dateODT_ACT_ARRIVE_DATE.AddMonths(11)) > 0)
                        strMessage += "<p> 申請日期不可大於進口日期 + 11個月 </p>";
                }
            }
            TBGMT_CLAIM_RESERVE reserve = MTSE.TBGMT_CLAIM_RESERVE.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
            if (reserve != null)
                strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 已申請保留索賠權，不可重複申請！ </p>";

            if (strMessage.Equals(string.Empty))
            {
                TBGMT_PORTS portArr = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(bld.OVC_ARRIVE_PORT)).FirstOrDefault();
                TBGMT_PORTS portStr = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(bld.OVC_START_PORT)).FirstOrDefault();
                if (portArr != null && portArr.OVC_IS_ABROAD.Equals("國內")) //進口提單
                {
                    TBGMT_ICR icr = MTSE.TBGMT_ICR.Where(t => t.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                    TBGMT_IINN iinn = MTSE.TBGMT_IINN.Where(t => t.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                    if (icr == null)
                        strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 之時程管制簿 不存在！ </p>";
                   else if (iinn == null)
                        strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 之投保通知書 不存在！ </p>";
                    if (strMessage.Equals(string.Empty))
                    {
                        var query =
                            from bldT in MTSE.TBGMT_BLD
                            where bldT.OVC_BLD_NO.Equals(strOVC_BLD_NO)
                            join icrT in MTSE.TBGMT_ICR on bldT.OVC_BLD_NO equals icrT.OVC_BLD_NO
                            join iinnT in MTSE.TBGMT_IINN on bldT.OVC_BLD_NO equals iinnT.OVC_BLD_NO
                            select new
                            {
                                bld.ODT_ACT_ARRIVE_DATE,
                                iinnT.OVC_IINN_NO
                            };
                        var table = query.FirstOrDefault();
                        if (table != null)
                        {
                            txtOVC_IMPORT_DATE.Text = strODT_ACT_ARRIVE_DATE;
                            txtOVC_INN_NO.Text = table.OVC_IINN_NO;
                        }
                        else
                            strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 進口資料不完全！ </p>";
                    }
                }
                else if (portStr != null && portStr.OVC_IS_ABROAD.Equals("國內")) //出口提單
                {
                    TBGMT_EDF edf = MTSE.TBGMT_EDF.Where(t => t.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                    if (edf == null)
                        strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 之外運資料表 不存在！ </p>";
                    else
                    {
                        string strOVC_EDF_NO = edf.OVC_EDF_NO;
                        TBGMT_EINN iinn = MTSE.TBGMT_EINN.Where(t => t.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();
                        if (iinn == null)
                            strMessage += $"<p> 外運資料表編號：{ strOVC_EDF_NO } 之投保通知書 不存在！ </p>";
                    }
                    if (strMessage.Equals(string.Empty))
                    {
                        var query =
                            from bldT in MTSE.TBGMT_BLD
                            where bldT.OVC_BLD_NO.Equals(strOVC_BLD_NO)
                            join edfT in MTSE.TBGMT_EDF on bldT.OVC_BLD_NO equals edfT.OVC_BLD_NO
                            join einnT in MTSE.TBGMT_EINN on edfT.OVC_EDF_NO equals einnT.OVC_EDF_NO
                            select new
                            {
                                bld.ODT_ACT_ARRIVE_DATE,
                                einnT.OVC_EINN_NO,
                            };
                        var table = query.FirstOrDefault();
                        if (table != null)
                        {
                            txtOVC_IMPORT_DATE.Text = strODT_ACT_ARRIVE_DATE;
                            txtOVC_INN_NO.Text = table.OVC_EINN_NO;
                        }
                        else
                            strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 出口資料不完全！ </p>";
                    }
                }
                else
                    strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 進出口 判斷錯誤！ </p>";
            }
            if (!strMessage.Equals(string.Empty))
                FCommon.AlertShow(pnMessageModify, "danger", "系統訊息", strMessage);
        }
        //protected void btnDel_Click(object sender, EventArgs e)
        //{
        //    TBGMT_CLAIM_RESERVE temp = new TBGMT_CLAIM_RESERVE();
        //    temp = MTSE.TBGMT_CLAIM_RESERVE.Where(table => table.OVC_RECLAIM_NO.Equals(lblOVC_RECLAIM_NO.Text)).FirstOrDefault();
        //    if (temp != null)
        //    {
        //        //if(temp.OVC_APPROVE_STATUS!=null && temp.OVC_APPROVE_STATUS.Equals("撤銷(資料錯誤)"))
        //        //    FCommon.AlertShow(pnMessageModify, "danger", "系統訊息", "資料已經註銷，無法再註銷");
        //        //else
        //        //{
        //        temp.OVC_APPROVE_STATUS = "撤銷(資料錯誤)";
        //        MTSE.SaveChanges();
        //        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), temp.GetType().Name.ToString(), this, "刪除"); FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), temp.GetType().Name.ToString(), this, "註銷");
        //        FCommon.AlertShow(pnMessageModify, "danger", "系統訊息", "資料已經註銷，無法再註銷");
        //        dataImport();
        //        //}
        //    }
        //}
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            #region 取值
            string strOVC_DEPT_CDE = txtOVC_DEPT_CDE.Value;
            string strOVC_APPLY_DATE = txtOVC_APPLY_DATE.Text;
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_IMPORT_DATE = txtOVC_IMPORT_DATE.Text;
            string strOVC_INN_NO = txtOVC_INN_NO.Text;
            string strOVC_CLAIM_MSG_NO = txtOVC_CLAIM_MSG_NO.Text; //軍種保留索賠權來文文號
            string strOVC_CLAIM_DATE = txtOVC_CLAIM_DATE.Text; //軍種保留索賠權來文日期
            string strOVC_PURCHASE_MSG_NO = txtOVC_PURCHASE_MSG_NO.Text; //國防採購室申請保留索賠權來文文號
            string strOVC_PURCHASE_DATE = txtOVC_PURCHASE_DATE.Text; //國防採購室申請保留索賠權來文日期
            string strOVC_CLAIM_ITEM = txtOVC_CLAIM_ITEM.Text;
            string strOVC_CLAIM_REASON = rdoOVC_CLAIM_REASON.SelectedValue;
            string strOVC_CLAIM_REASON_NOTE = txtOVC_CLAIM_REASON_NOTE.Text;
            bool isOther = strOVC_CLAIM_REASON.Equals(strOtherText);
            string strONB_RECEIVE = txtONB_RECEIVE.Text; //應收件數
            string strONB_ACTUAL_RECEIVE = txtONB_ACTUAL_RECEIVE.Text; //實收件數
            string strONB_CLAIM_BREAK = txtONB_CLAIM_BREAK.Text; //破損件數
            string strONB_CLAIM_NUMBER = txtONB_CLAIM_NUMBER.Text; //索賠件數
            string strONB_CLAIM_AMOUNT = txtONB_CLAIM_AMOUNT.Text; //空值補0
            string strOVC_CLAIM_CURRENCY = drpOVC_CLAIM_CURRENCY.SelectedValue;
            string strONB_CLAIM_CURRENCY_RATE = txtONB_CLAIM_CURRENCY_RATE.Text; //空值補0
            string strONB_CLAIM_AMOUNT_NTD = txtONB_CLAIM_AMOUNT_NTD.Text; //空值補0

            string strOVC_CLAIM_COM_MSG = txtOVC_CLAIM_COM_MSG.Text; //軍種申請理賠來文文號
            string strOVC_CLAIM_COM_DATE = txtOVC_CLAIM_COM_DATE.Text; //軍種申請理賠來文日期
            string strOVC_PURCHASE_COM_MSG = txtOVC_PURCHASE_COM_MSG.Text; //國防採購室申請理賠來文文號
            string strOVC_PURCHASE_COM_DATE = txtOVC_PURCHASE_COM_DATE.Text; //國防採購室申請理賠來文日期
            string strOVC_CLAIM_REV_MSG = txtOVC_CLAIM_REV_MSG.Text; //軍種撤銷保留索賠權來文文號
            string strOVC_CLAIM_REV_DATE = txtOVC_CLAIM_REV_DATE.Text; //軍種撤銷保留索賠權來文日期
            string strOVC_PURCHASE_REV_MSG = txtOVC_PURCHASE_REV_MSG.Text; //國防採購室撤銷保留索賠權來文文號
            string strOVC_PURCHASE_REV_DATE = txtOVC_PURCHASE_REV_DATE.Text; //國防採購室撤銷保留索賠全來文日期
            string strOVC_INS_COM_MSG = txtOVC_INS_COM_MSG.Text; //保險公司理賠文號
            string strONB_COMPENSATION_AMOUNT = txtONB_COMPENSATION_AMOUNT.Text; //空值補0
            string strOVC_COMPENSATION_CURRENCY = drpOVC_COMPENSATION_CURRENCY.Text;
            string strONB_COMPENSATION_CURRENCY_RATE = txtONB_COMPENSATION_CURRENCY_RATE.Text; //空值補0
            string strONB_COMPENSATION_AMOUNT_NTD = txtONB_COMPENSATION_AMOUNT_NTD.Text; //空值補0
            string strOVC_CHEQUE_BANK = txtOVC_CHEQUE_BANK.Text;
            string strOVC_CHEQUE_NO = txtOVC_CHEQUE_NO.Text;
            string strOVC_CHEQUE_TITLE = txtOVC_CHEQUE_TITLE.Text;
            string strOVC_CHEQUE_DATE = txtOVC_CHEQUE_DATE.Text;
            string strOVC_NOTE = txtOVC_NOTE.Text;
            string strOVC_APPROVE_STATUS = drpOVC_APPROVE_STATUS.SelectedValue;
            string strOVC_APPROVE_DATE = txtOVC_APPROVE_DATE.Text;
            string strODT_CLAIM_DATE_2 = txtODT_CLAIM_DATE_2.Text;
            #endregion

            #region 錯誤訊息
            if (!Guid.TryParse(id, out Guid guidRECLAIM_SN))
                strMessage += "<p> 保留索賠權 系統編號錯誤！ </p>";
            TBGMT_CLAIM_RESERVE reserve = MTSE.TBGMT_CLAIM_RESERVE.Where(table => table.RECLAIM_SN.Equals(guidRECLAIM_SN)).FirstOrDefault();
            if (reserve == null)
                strMessage += "<p> 保留索賠權 不存在！ </p>";
            else
            {
                string strOVC_APPROVE_STATUS_Original = reserve.OVC_APPROVE_STATUS ?? "";
                if (strOVC_APPROVE_STATUS_Original.Equals("撤銷(資料錯誤)"))
                    strMessage += "<p> 資料已經註銷，無法更動！ </p>";
            }

            if (strOVC_DEPT_CDE.Equals(string.Empty))
                strMessage += "<p> 請選取 申請單位！ </p>";
            if (strOVC_APPLY_DATE.Equals(string.Empty))
                strMessage += "<p> 請輸入 申請日期！ </p>";
            if (strOVC_BLD_NO.Trim().Equals(string.Empty))
                strMessage += "<p> 請輸入 提單編號！ </p>";
            else if (!strOVC_BLD_NO.Equals(reserve.OVC_BLD_NO)) //不同於原保留索賠權之提單編號
            {
                TBGMT_CLAIM_RESERVE reserve_bld = MTSE.TBGMT_CLAIM_RESERVE.Where(table => !table.RECLAIM_SN.Equals(guidRECLAIM_SN) && table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                if (reserve != null)
                    strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 已申請保留索賠權，不可重複申請！ </p>";
            }
            TBGMT_IINN iinn = MTSE.TBGMT_IINN.Where(table => table.OVC_IINN_NO.Equals(strOVC_INN_NO)).FirstOrDefault();
            TBGMT_EINN einn = MTSE.TBGMT_EINN.Where(table => table.OVC_EINN_NO.Equals(strOVC_INN_NO)).FirstOrDefault();
            if (strOVC_INN_NO.Equals(string.Empty))
                strMessage += "<p> 請輸入 投保通知書編號！ </p>";
            else if (iinn == null && einn == null)
                strMessage += $"<p> 投保通知書編號：{ strOVC_INN_NO } 不存在，請重新輸入！ </p>";
            if (strOVC_CLAIM_MSG_NO.Trim().Equals(string.Empty))
                strMessage += "<p> 請輸入 軍種保留索賠權來文文號！ </p>";
            if (strOVC_CLAIM_DATE.Equals(string.Empty))
                strMessage += "<p> 請輸入 軍種保留索賠權來文日期！ </p>";
            if (!strOVC_PURCHASE_MSG_NO.Trim().Equals(string.Empty) || !strOVC_PURCHASE_DATE.Equals(string.Empty))
            {
                if (strOVC_PURCHASE_MSG_NO.Trim().Equals(string.Empty))
                    strMessage += "<p> 請輸入 國防採購室申請保留索賠權來文文號！ </p>";
                if (strOVC_PURCHASE_DATE.Equals(string.Empty))
                    strMessage += "<p> 請輸入 國防採購室申請保留索賠權來文日期！ </p>";
            }
            if (strOVC_CLAIM_ITEM.Trim().Equals(string.Empty))
                strMessage += "<p> 請輸入 軍品名稱！ </p>";
            if (strOVC_CLAIM_REASON.Equals(string.Empty))
                strMessage += "<p> 請選擇 損失原因！ </p>";
            if (isOther && (strOVC_CLAIM_REASON_NOTE.Equals(string.Empty) || strOVC_CLAIM_REASON_NOTE.Equals(strDefault_OVC_CLAIM_REASON_NOTE)))
                strMessage += "<p> 請輸入 其他-損失原因！ </p>";
            if (strONB_RECEIVE.Trim().Equals(string.Empty))
                strMessage += "<p> 請輸入 應收件數！ </p>";
            if (strONB_ACTUAL_RECEIVE.Trim().Equals(string.Empty))
                strMessage += "<p> 請選擇 實收件數！ </p>";
            if (strOVC_CLAIM_CURRENCY.Equals(string.Empty))
                strMessage += "<p> 請選擇 索賠金額 幣別！ </p>";
            if (!strOVC_CLAIM_COM_MSG.Trim().Equals(string.Empty) || !strOVC_CLAIM_COM_DATE.Equals(string.Empty))
            {
                if (strOVC_CLAIM_COM_MSG.Trim().Equals(string.Empty))
                    strMessage += "<p> 請輸入 軍種申請理賠來文文號！ </p>";
                if (strOVC_CLAIM_COM_DATE.Equals(string.Empty))
                    strMessage += "<p> 請選擇 軍種申請理賠來文日期！ </p>";
            }
            if (!strOVC_PURCHASE_COM_MSG.Trim().Equals(string.Empty) || !strOVC_PURCHASE_COM_DATE.Equals(string.Empty))
            {
                if (strOVC_PURCHASE_COM_MSG.Trim().Equals(string.Empty))
                    strMessage += "<p> 請輸入 國防採購室申請理賠來文文號！ </p>";
                if (strOVC_PURCHASE_COM_DATE.Equals(string.Empty))
                    strMessage += "<p> 請選擇 國防採購室申請理賠來文日期！ </p>";
            }
            if (!strOVC_CLAIM_REV_MSG.Trim().Equals(string.Empty) || !strOVC_CLAIM_REV_DATE.Equals(string.Empty))
            {
                if (strOVC_CLAIM_REV_MSG.Trim().Equals(string.Empty))
                    strMessage += "<p> 請輸入 軍種撤銷保留索賠權來文文號！ </p>";
                if (strOVC_CLAIM_REV_DATE.Equals(string.Empty))
                    strMessage += "<p> 請選擇 軍種撤銷保留索賠權來文日期！ </p>";
            }
            if (!strOVC_PURCHASE_REV_MSG.Trim().Equals(string.Empty) || !strOVC_PURCHASE_REV_DATE.Equals(string.Empty))
            {
                if (strOVC_PURCHASE_REV_MSG.Trim().Equals(string.Empty))
                    strMessage += "<p> 請輸入 國防採購室撤銷保留索賠權來文文號！ </p>";
                if (strOVC_PURCHASE_REV_DATE.Equals(string.Empty))
                    strMessage += "<p> 請選擇 國防採購室撤銷保留索賠全來文日期！ </p>";
            }
            if (strOVC_COMPENSATION_CURRENCY.Equals(string.Empty))
                strMessage += "<p> 請選擇 實際理賠金額 幣別！ </p>";

            bool boolONB_RECEIVE = FCommon.checkDecimal(strONB_RECEIVE, "應收件數", ref strMessage, out decimal decONB_RECEIVE);
            bool boolONB_ACTUAL_RECEIVE = FCommon.checkDecimal(strONB_ACTUAL_RECEIVE, "實收件數", ref strMessage, out decimal decONB_ACTUAL_RECEIVE);
            bool boolONB_CLAIM_BREAK = FCommon.checkDecimal(strONB_CLAIM_BREAK, "破損件數", ref strMessage, out decimal decONB_CLAIM_BREAK);
            bool boolONB_CLAIM_NUMBER = FCommon.checkDecimal(strONB_CLAIM_NUMBER, "索賠件數", ref strMessage, out decimal decONB_CLAIM_NUMBER);
            bool boolONB_CLAIM_AMOUNT = FCommon.checkDecimal(strONB_CLAIM_AMOUNT, "索賠金額", ref strMessage, out decimal decONB_CLAIM_AMOUNT);
            bool boolONB_CLAIM_CURRENCY_RATE = FCommon.checkDecimal(strONB_CLAIM_CURRENCY_RATE, "匯率", ref strMessage, out decimal decONB_CLAIM_CURRENCY_RATE);
            bool boolONB_CLAIM_AMOUNT_NTD = FCommon.checkDecimal(strONB_CLAIM_AMOUNT_NTD, "索賠金額(台幣)", ref strMessage, out decimal decONB_CLAIM_AMOUNT_NTD);
            bool boolONB_COMPENSATION_AMOUNT = FCommon.checkDecimal(strONB_COMPENSATION_AMOUNT, "實際理賠金額", ref strMessage, out decimal decONB_COMPENSATION_AMOUNT);
            bool boolONB_COMPENSATION_CURRENCY_RATE = FCommon.checkDecimal(strONB_COMPENSATION_CURRENCY_RATE, "匯率", ref strMessage, out decimal decONB_COMPENSATION_CURRENCY_RATE);
            bool boolONB_COMPENSATION_AMOUNT_NTD = FCommon.checkDecimal(strONB_COMPENSATION_AMOUNT_NTD, "實際理賠金額(台幣)", ref strMessage, out decimal decONB_COMPENSATION_AMOUNT_NTD);

            bool boolOVC_APPLY_DATE = FCommon.checkDateTime(strOVC_APPLY_DATE, "申請日期", ref strMessage, out DateTime dateOVC_APPLY_DATE);
            bool boolOVC_IMPORT_DATE = FCommon.checkDateTime(strOVC_IMPORT_DATE, "進口日期", ref strMessage, out DateTime dateOVC_IMPORT_DATE);
            bool boolOVC_CLAIM_DATE = FCommon.checkDateTime(strOVC_CLAIM_DATE, "軍種保留索賠權來文日期", ref strMessage, out DateTime dateOVC_CLAIM_DATE);
            bool boolOVC_PURCHASE_DATE = FCommon.checkDateTime(strOVC_PURCHASE_DATE, "國防採購室申請保留索賠權來文日期", ref strMessage, out DateTime dateOVC_PURCHASE_DATE);
            bool boolOVC_CLAIM_COM_DATE = FCommon.checkDateTime(strOVC_CLAIM_COM_DATE, "軍種申請理賠來文日期", ref strMessage, out DateTime dateOVC_CLAIM_COM_DATE);
            bool boolOVC_PURCHASE_COM_DATE = FCommon.checkDateTime(strOVC_PURCHASE_COM_DATE, "國防採購室申請理賠來文日期", ref strMessage, out DateTime dateOVC_PURCHASE_COM_DATE);
            bool boolOVC_CLAIM_REV_DATE = FCommon.checkDateTime(strOVC_CLAIM_REV_DATE, "軍種撤銷保留索賠權來文日期", ref strMessage, out DateTime dateOVC_CLAIM_REV_DATE);
            bool boolOVC_PURCHASE_REV_DATE = FCommon.checkDateTime(strOVC_PURCHASE_REV_DATE, "國防採購室撤銷保留索賠全來文日期", ref strMessage, out DateTime dateOVC_PURCHASE_REV_DATE);
            bool boolOVC_CHEQUE_DATE = FCommon.checkDateTime(strOVC_CHEQUE_DATE, "支票日期", ref strMessage, out DateTime dateOVC_CHEQUE_DATE);
            bool boolOVC_APPROVE_DATE = FCommon.checkDateTime(strOVC_APPROVE_DATE, "結案日期", ref strMessage, out DateTime dateOVC_APPROVE_DATE);
            bool boolODT_CLAIM_DATE_2 = FCommon.checkDateTime(strODT_CLAIM_DATE_2, "理賠日期", ref strMessage, out DateTime dateODT_CLAIM_DATE_2);

            if (boolOVC_APPLY_DATE && boolOVC_IMPORT_DATE && DateTime.Compare(dateOVC_APPLY_DATE, dateOVC_IMPORT_DATE.AddMonths(11)) > 0)
                strMessage += "<p> 申請日期不可大於進口日期 + 11個月 </p>";
            #endregion

            if (boolONB_COMPENSATION_AMOUNT_NTD) reserve.ONB_COMPENSATION_AMOUNT_NTD = decONB_COMPENSATION_AMOUNT_NTD; else reserve.ONB_COMPENSATION_AMOUNT_NTD = null;
            if (strMessage.Equals(string.Empty))
            {
                #region 修改 TBGMT_CLAIM_RESERVE
                reserve.OVC_MILITARY_TYPE = strOVC_DEPT_CDE;
                reserve.OVC_APPLY_DATE = dateOVC_APPLY_DATE;
                reserve.OVC_BLD_NO = strOVC_BLD_NO;
                if (boolOVC_IMPORT_DATE) reserve.OVC_IMPORT_DATE = dateOVC_IMPORT_DATE; else reserve.OVC_IMPORT_DATE = null;
                reserve.OVC_INN_NO = strOVC_INN_NO;
                reserve.OVC_CLAIM_MSG_NO = strOVC_CLAIM_MSG_NO;
                reserve.OVC_CLAIM_DATE = dateOVC_CLAIM_DATE;
                reserve.OVC_PURCHASE_MSG_NO = strOVC_PURCHASE_MSG_NO;
                if (boolOVC_PURCHASE_DATE) reserve.OVC_PURCHASE_DATE = dateOVC_PURCHASE_DATE; else reserve.OVC_PURCHASE_DATE = null;
                reserve.OVC_CLAIM_ITEM = strOVC_CLAIM_ITEM;
                reserve.OVC_CLAIM_REASON = strOVC_CLAIM_REASON;
                if (isOther) reserve.OVC_CLAIM_REASON_NOTE = strOVC_CLAIM_REASON_NOTE; else reserve.OVC_CLAIM_REASON_NOTE = null;
                reserve.ONB_RECEIVE = decONB_RECEIVE;
                reserve.ONB_ACTUAL_RECEIVE = decONB_ACTUAL_RECEIVE;
                if (boolONB_CLAIM_BREAK) reserve.ONB_CLAIM_BREAK = decONB_CLAIM_BREAK; else reserve.ONB_CLAIM_BREAK = null;
                if (boolONB_CLAIM_NUMBER) reserve.ONB_CLAIM_NUMBER = decONB_CLAIM_NUMBER; else reserve.ONB_CLAIM_NUMBER = null;
                reserve.ONB_CLAIM_AMOUNT = decONB_CLAIM_AMOUNT; //空值補0
                reserve.OVC_CLAIM_CURRENCY = strOVC_CLAIM_CURRENCY;
                 reserve.ONB_CLAIM_CURRENCY_RATE = decONB_CLAIM_CURRENCY_RATE; //空值補0
                 reserve.ONB_CLAIM_AMOUNT_NTD = decONB_CLAIM_AMOUNT_NTD; //空值補0

                reserve.OVC_CLAIM_COM_MSG = strOVC_CLAIM_COM_MSG;
                if (boolOVC_CLAIM_COM_DATE) reserve.OVC_CLAIM_COM_DATE = dateOVC_CLAIM_COM_DATE; else reserve.OVC_CLAIM_COM_DATE = null;
                reserve.OVC_PURCHASE_COM_MSG = strOVC_PURCHASE_COM_MSG;
                if (boolOVC_PURCHASE_COM_DATE) reserve.OVC_PURCHASE_COM_DATE = dateOVC_PURCHASE_COM_DATE; else reserve.OVC_PURCHASE_COM_DATE = null;
                reserve.OVC_CLAIM_REV_MSG = strOVC_CLAIM_REV_MSG;
                if (boolOVC_CLAIM_REV_DATE) reserve.OVC_CLAIM_REV_DATE = dateOVC_CLAIM_REV_DATE; else reserve.OVC_CLAIM_REV_DATE = null;
                reserve.OVC_PURCHASE_REV_MSG = strOVC_PURCHASE_REV_MSG;
                if (boolOVC_PURCHASE_REV_DATE) reserve.OVC_PURCHASE_REV_DATE = dateOVC_PURCHASE_REV_DATE; else reserve.OVC_PURCHASE_REV_DATE = null;
                reserve.OVC_INS_COM_MSG = strOVC_INS_COM_MSG;
                reserve.ONB_COMPENSATION_AMOUNT = decONB_COMPENSATION_AMOUNT; //空值補0
                reserve.OVC_COMPENSATION_CURRENCY = strOVC_COMPENSATION_CURRENCY;
                reserve.ONB_COMPENSATION_CURRENCY_RATE = decONB_COMPENSATION_CURRENCY_RATE; //空值補0
                reserve.ONB_COMPENSATION_AMOUNT_NTD = decONB_COMPENSATION_AMOUNT_NTD; //空值補0
                reserve.OVC_CHEQUE_BANK = strOVC_CHEQUE_BANK;
                reserve.OVC_CHEQUE_NO = strOVC_CHEQUE_NO;
                reserve.OVC_CHEQUE_TITLE = strOVC_CHEQUE_TITLE;
                if (boolOVC_CHEQUE_DATE) reserve.OVC_CHEQUE_DATE = dateOVC_CHEQUE_DATE; else reserve.OVC_CHEQUE_DATE = null;
                reserve.OVC_NOTE = strOVC_NOTE;
                reserve.OVC_APPROVE_STATUS = strOVC_APPROVE_STATUS;
                reserve.OVC_APPROVE_DATE = dateOVC_APPROVE_DATE;
                reserve.OVC_MODIFY_LOGIN_ID = strUserId;
                reserve.ODT_MODIFY_DATE = dateNow;
                if (boolODT_CLAIM_DATE_2) reserve.ODT_CLAIM_DATE = dateODT_CLAIM_DATE_2; else reserve.ODT_CLAIM_DATE = null;
                MTSE.SaveChanges();
                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), reserve.GetType().Name.ToString(), this, "修改");

                string strOVC_RECLAIM_NO = reserve.OVC_RECLAIM_NO;
                FCommon.AlertShow(pnMessageModify, "success", "系統訊息", $"保留索賠權更新成功，保留索賠權編號：{ strOVC_RECLAIM_NO }。");
                //drpOVC_APPROVE_STATUS.SelectedValue = strOVC_APPROVE_STATUS;
                #endregion
            }
            else
                FCommon.AlertShow(pnMessageModify, "danger", "系統訊息", strMessage);
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_C15_1{ getQueryString() }");
        }
        #endregion
        
        protected void rdoOVC_CLAIM_REASON_SelectedIndexChanged(object sender, EventArgs e)
        {
            setOVC_CLAIM_REASON();
            setAutoNumber();
        }

        protected void txtOVC_NO_TextChanged(object sender, EventArgs e)
        {
            string strOVC_APPROVE_STATUS = "";
            if (!txtOVC_PURCHASE_MSG_NO.Text.Equals(string.Empty))
                strOVC_APPROVE_STATUS = "申請保留索賠權";
            if (!txtOVC_CLAIM_COM_MSG.Text.Equals(string.Empty))
                strOVC_APPROVE_STATUS = "申請理賠";
            if (!txtOVC_PURCHASE_COM_MSG.Text.Equals(string.Empty))
                strOVC_APPROVE_STATUS = "已理賠";
            if (!txtOVC_CLAIM_REV_MSG.Text.Equals(string.Empty))
                strOVC_APPROVE_STATUS = "撤銷(美軍獲賠)";
            if (!txtOVC_PURCHASE_REV_MSG.Text.Equals(string.Empty))
                strOVC_APPROVE_STATUS = "已撤銷";

            FCommon.list_setValue(drpOVC_APPROVE_STATUS, strOVC_APPROVE_STATUS);
        }
        protected void txtONB_RECEIVE_TextChanged(object sender, EventArgs e)
        {
            setAutoNumber();
        }
    }
}