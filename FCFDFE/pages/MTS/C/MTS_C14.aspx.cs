using System;
using System.Linq;
using System.Web.UI;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.MTS.C
{
    public partial class MTS_C14 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        public string strDEPT_SN, strDEPT_Name;
        DateTime dateNow = DateTime.Now;
        string strOtherText = VariableMTS.strOtherText;
        string strDefault_OVC_CLAIM_REASON_NOTE = "請輸入其他損失原因";

        #region 副程式
        private void dataImport()
        {
            setAutoNo(); //保留索賠權編號
            lblOVC_CREATE_LOGIN_ID.Text = FCommon.getUserName(Session["userid"]);
            txtOVC_DEPT_CDE.Value = strDEPT_SN;
            txtOVC_ONNAME.Text = strDEPT_Name;
            txtOVC_APPLY_DATE.Text = FCommon.getDateTime(dateNow);

            bool isOther = rdoOVC_CLAIM_REASON.SelectedValue.Equals(strOtherText);
            txtOVC_CLAIM_REASON_NOTE.Visible = isOther;
        }
        private void setAutoNo() //保留索賠權編號
        {
            string yyy = FCommon.getTaiwanYear(dateNow).ToString("000");
            string reclaim_no = "CRF" + yyy;
            int num = 0;
            var query = MTSE.TBGMT_CLAIM_RESERVE.Where(table => table.OVC_RECLAIM_NO.StartsWith(reclaim_no)).OrderByDescending(table => table.OVC_RECLAIM_NO).FirstOrDefault();
            if (query != null)
                int.TryParse(query.OVC_RECLAIM_NO.Substring(6, 4), out num);
            num += 1;
            reclaim_no += num.ToString("0000");
            lblOVC_RECLAIM_NO.Text = reclaim_no;
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
                FCommon.AlertShow(pnMessageNew, "danger", "系統訊息", strMessage);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getAccountDEPTName(this, out strDEPT_SN, out strDEPT_Name);
                //若點選其他時，顯示textbox預設文字'請輸入其他損失原因'，點選則消失
                txtOVC_CLAIM_REASON_NOTE.Attributes.Add("onfocus", "if(this.value=='請輸入其他損失原因')this.value='';");

                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_ONNAME, txtOVC_APPLY_DATE, txtOVC_IMPORT_DATE, txtOVC_CLAIM_DATE);
                    #region 匯入下拉式選單
                    CommonMTS.list_dataImport_CURRENCY(drpOVC_CLAIM_CURRENCY, false); //幣別
                    //CommonMTS.list_dataImport_CURRENCY(drpOVC_COMPENSATION_CURRENCY, false); //幣別
                    CommonMTS.list_dataImport_CLAIM_REASON(rdoOVC_CLAIM_REASON, false); //損失原因
                    CommonMTS.list_dataImport_APPROVE_STATUS(drpOVC_APPROVE_STATUS, true); //作業進度
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
                FCommon.AlertShow(pnMessageNew, "danger", "系統訊息", strMessage);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            #region 取值
            string strOVC_RECLAIM_NO = lblOVC_RECLAIM_NO.Text;
            string strOVC_DEPT_CDE = txtOVC_DEPT_CDE.Value;
            string strOVC_APPLY_DATE = txtOVC_APPLY_DATE.Text;
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_IMPORT_DATE = txtOVC_IMPORT_DATE.Text;
            string strOVC_INN_NO = txtOVC_INN_NO.Text;
            string strOVC_CLAIM_MSG_NO = txtOVC_CLAIM_MSG_NO.Text;
            string strOVC_CLAIM_DATE = txtOVC_CLAIM_DATE.Text;
            string strOVC_CLAIM_ITEM = txtOVC_CLAIM_ITEM.Text;
            string strOVC_CLAIM_REASON = rdoOVC_CLAIM_REASON.SelectedValue;
            string strOVC_CLAIM_REASON_NOTE = txtOVC_CLAIM_REASON_NOTE.Text;
            bool isOther = strOVC_CLAIM_REASON.Equals(strOtherText);
            string strONB_RECEIVE = txtONB_RECEIVE.Text; //應收件數
            string strONB_ACTUAL_RECEIVE = txtONB_ACTUAL_RECEIVE.Text; //實收件數
            string strONB_CLAIM_BREAK = txtONB_CLAIM_BREAK.Text; //破損件數
            string strONB_CLAIM_NUMBER = txtONB_CLAIM_NUMBER.Text; //索賠件數
            string strONB_CLAIM_AMOUNT = txtONB_CLAIM_AMOUNT.Text;
            string strOVC_CLAIM_CURRENCY = drpOVC_CLAIM_CURRENCY.SelectedValue;
            string strOVC_APPROVE_STATUS = drpOVC_APPROVE_STATUS.SelectedValue; //作業進度
            //string strONB_COMPENSATION_AMOUNT = txtONB_COMPENSATION_AMOUNT.Text;
            //string strOVC_COMPENSATION_CURRENCY = drpOVC_COMPENSATION_CURRENCY.SelectedValue;
            #endregion

            #region 錯誤訊息
            TBGMT_CLAIM_RESERVE reserve = MTSE.TBGMT_CLAIM_RESERVE.Where(table => table.OVC_RECLAIM_NO.Equals(strOVC_RECLAIM_NO)).FirstOrDefault();
            if (reserve != null)
            {
                strMessage += $"<p> 保留索賠權編號：{ strOVC_RECLAIM_NO } 已存在！ </p><p> 已重新取得編號，請再試一次。 </p>";
                setAutoNo(); //保留索賠權編號
            }
            if (strOVC_DEPT_CDE.Equals(string.Empty))
                strMessage += "<p> 請選取 申請單位！ </p>";
            if (strOVC_APPLY_DATE.Equals(string.Empty))
                strMessage += "<p> 請選取 申請日期！ </p>";
            if (strOVC_BLD_NO.Equals(string.Empty))
                strMessage += "<p> 請輸入 提單編號！ </p>";
            else
            {
                TBGMT_CLAIM_RESERVE reserve_bld = MTSE.TBGMT_CLAIM_RESERVE.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                if (reserve != null)
                    strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 已申請保留索賠權，不可重複申請！ </p>";
            }
            TBGMT_IINN iinn = MTSE.TBGMT_IINN.Where(table => table.OVC_IINN_NO.Equals(strOVC_INN_NO)).FirstOrDefault();
            TBGMT_EINN einn = MTSE.TBGMT_EINN.Where(table => table.OVC_EINN_NO.Equals(strOVC_INN_NO)).FirstOrDefault();
            if (strOVC_INN_NO.Equals(string.Empty))
                strMessage += "<p> 請輸入 投保通知書編號！ </p>";
            else if (iinn == null && einn == null)
                strMessage += $"<p> 投保通知書編號：{ strOVC_INN_NO } 不存在，請重新輸入！ </p>";
            if (strOVC_CLAIM_MSG_NO.Equals(string.Empty))
                strMessage += "<p> 請輸入 軍種保留索賠權來文文號！ </p>";
            if (strOVC_CLAIM_DATE.Equals(string.Empty))
                strMessage += "<p> 請選取 軍種保留索賠權來文日期！ </p>";
            if (strOVC_CLAIM_ITEM.Equals(string.Empty))
                strMessage += "<p> 請輸入 軍品名稱！ </p>";
            if (strOVC_CLAIM_REASON.Equals(string.Empty))
                strMessage += "<p> 請選擇 損失原因！ </p>";
            if (isOther && (strOVC_CLAIM_REASON_NOTE.Equals(string.Empty) || strOVC_CLAIM_REASON_NOTE.Equals(strDefault_OVC_CLAIM_REASON_NOTE)))
                strMessage += "<p> 請輸入 其他-損失原因！ </p>";

            bool boolOVC_APPLY_DATE = FCommon.checkDateTime(strOVC_APPLY_DATE, "申請日期", ref strMessage, out DateTime dateOVC_APPLY_DATE);
            bool boolOVC_IMPORT_DATE = FCommon.checkDateTime(strOVC_IMPORT_DATE, "進口日期", ref strMessage, out DateTime dateOVC_IMPORT_DATE);
            bool boolOVC_CLAIM_DATE = FCommon.checkDateTime(strOVC_CLAIM_DATE, "軍種保留索賠權來文日期", ref strMessage, out DateTime dateOVC_CLAIM_DATE);
            bool boolONB_RECEIVE = FCommon.checkDecimal(strONB_RECEIVE, "應收件數", ref strMessage, out decimal decONB_RECEIVE);
            bool boolONB_ACTUAL_RECEIVE = FCommon.checkDecimal(strONB_ACTUAL_RECEIVE, "實收件數", ref strMessage, out decimal decONB_ACTUAL_RECEIVE);
            bool boolONB_CLAIM_BREAK = FCommon.checkDecimal(strONB_CLAIM_BREAK, "破損件數", ref strMessage, out decimal decONB_CLAIM_BREAK);
            bool boolONB_CLAIM_NUMBER = FCommon.checkDecimal(strONB_CLAIM_NUMBER, "索賠件數", ref strMessage, out decimal decONB_CLAIM_NUMBER);
            bool boolONB_CLAIM_AMOUNT = FCommon.checkDecimal(strONB_CLAIM_AMOUNT, "索賠金額", ref strMessage, out decimal decONB_CLAIM_AMOUNT);
            //bool boolONB_COMPENSATION_AMOUNT = FCommon.checkDecimal(strONB_COMPENSATION_AMOUNT, "實際理賠金額", ref strMessage, out decimal decONB_COMPENSATION_AMOUNT);

            if (boolOVC_APPLY_DATE && boolOVC_IMPORT_DATE && DateTime.Compare(dateOVC_APPLY_DATE, dateOVC_IMPORT_DATE.AddMonths(11)) > 0)
                strMessage += "<p> 申請日期不可大於進口日期 + 11個月 </p>";
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                #region 新增TBGMT_CLAIM_RESERVE
                reserve = new TBGMT_CLAIM_RESERVE();
                reserve.RECLAIM_SN = Guid.NewGuid();
                reserve.OVC_RECLAIM_NO = strOVC_RECLAIM_NO;
                reserve.OVC_MILITARY_TYPE = strOVC_DEPT_CDE;
                reserve.OVC_IMPORT_DATE = dateOVC_APPLY_DATE;
                reserve.OVC_BLD_NO = strOVC_BLD_NO;
                if (boolOVC_APPLY_DATE) reserve.OVC_IMPORT_DATE = dateOVC_APPLY_DATE; else reserve.OVC_IMPORT_DATE = null;
                reserve.OVC_INN_NO = strOVC_INN_NO;
                reserve.OVC_CLAIM_MSG_NO = strOVC_CLAIM_MSG_NO;
                reserve.OVC_CLAIM_DATE = dateOVC_CLAIM_DATE;
                reserve.OVC_CLAIM_ITEM = strOVC_CLAIM_ITEM;
                reserve.OVC_CLAIM_REASON = strOVC_CLAIM_REASON;
                if (isOther) reserve.OVC_CLAIM_REASON_NOTE = strOVC_CLAIM_REASON_NOTE; else reserve.OVC_CLAIM_REASON_NOTE = null;
                if (boolONB_RECEIVE) reserve.ONB_RECEIVE = decONB_RECEIVE; else reserve.ONB_RECEIVE = null;
                if (boolONB_ACTUAL_RECEIVE) reserve.ONB_ACTUAL_RECEIVE = decONB_ACTUAL_RECEIVE; else reserve.ONB_ACTUAL_RECEIVE = null;
                if (boolONB_CLAIM_BREAK) reserve.ONB_CLAIM_BREAK = decONB_CLAIM_BREAK; else reserve.ONB_CLAIM_BREAK = null;
                if (boolONB_CLAIM_NUMBER) reserve.ONB_CLAIM_NUMBER = decONB_CLAIM_NUMBER; else reserve.ONB_CLAIM_NUMBER = null;
                if (boolONB_CLAIM_AMOUNT) reserve.ONB_CLAIM_AMOUNT = decONB_CLAIM_AMOUNT; else reserve.ONB_CLAIM_AMOUNT = null;
                reserve.OVC_CLAIM_CURRENCY = strOVC_CLAIM_CURRENCY;
                //reserve.ONB_COMPENSATION_AMOUNT = decONB_COMPENSATION_AMOUNT;
                //reserve.OVC_COMPENSATION_CURRENCY = strOVC_COMPENSATION_CURRENCY;
                reserve.OVC_APPROVE_STATUS = strOVC_APPROVE_STATUS;
                //reserve.OVC_CHEQUE_BANK = strOVC_CHEQUE_BANK;
                //reserve.OVC_CHEQUE_NO = strOVC_CHEQUE_NO;
                //reserve.OVC_CHEQUE_TITLE = strOVC_CHEQUE_TITLE;
                //if (strOVC_CHEQUE_DATE != "")
                //    reserve.OVC_CHEQUE_DATE = Convert.ToDateTime(strOVC_CHEQUE_DATE);
                //reserve.OVC_NOTE = strOVC_NOTE;
                reserve.OVC_CREATE_ID = strUserId;
                reserve.ODT_CREATE_DATE = dateNow;
                reserve.OVC_MODIFY_LOGIN_ID = strUserId;
                reserve.ODT_MODIFY_DATE = dateNow;
                MTSE.TBGMT_CLAIM_RESERVE.Add(reserve);
                MTSE.SaveChanges();
                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), reserve.GetType().Name.ToString(), this, "新增");

                FCommon.AlertShow(pnMessageNew, "success", "系統訊息", $"保留索賠權建立成功，保留索賠權編號：{ strOVC_RECLAIM_NO }。");
                FCommon.MessageBoxShow(this, "繼續新增保留索賠權", "MTS_C14", false);
                dataImport();
                #endregion
            }
            else
                FCommon.AlertShow(pnMessageNew, "danger", "系統訊息", strMessage);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            FCommon.Controls_Clear(txtOVC_DEPT_CDE, txtOVC_ONNAME);
        }
        #endregion

        protected void rdoOVC_CLAIM_REASON_SelectedIndexChanged(object sender, EventArgs e)
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
            setAutoNumber();
        }

        protected void txtONB_RECEIVE_TextChanged(object sender, EventArgs e)
        {
            setAutoNumber();
        }
    }
}