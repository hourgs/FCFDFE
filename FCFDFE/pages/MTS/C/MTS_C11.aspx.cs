using System;
using System.Data;
using System.Linq;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.Web.UI;

namespace FCFDFE.pages.MTS.C
{
    public partial class MTS_C11 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        public string strDEPT_SN, strDEPT_Name;
        DateTime dateNow = DateTime.Now;

        #region 副程式
        private void dataImport()
        {
            setAutoNo(); //索賠通知書編號
            lblOVC_CREATE_LOGIN_ID.Text = FCommon.getUserName(Session["userid"]);
            txtOVC_DEPT_CDE.Value = strDEPT_SN;
            txtOVC_ONNAME.Text = strDEPT_Name;
        }
        private void setAutoNo() //索賠通知書編號
        {
            string yyy = FCommon.getTaiwanYear(dateNow).ToString("000");
            string claim_no = "CNF" + yyy;
            int num = 0;
            var query = MTSE.TBGMT_CLAIM.Where(table => table.OVC_CLAIM_NO.StartsWith(claim_no)).OrderByDescending(table => table.OVC_CLAIM_NO).FirstOrDefault();
            if (query != null)
                int.TryParse(query.OVC_CLAIM_NO.Substring(6, 4), out num);
            num += 1;
            claim_no += num.ToString("0000");
            lblOVC_CLAIM_NO.Text = claim_no;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getAccountDEPTName(this, out strDEPT_SN, out strDEPT_Name);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtODT_CLAIM_DATE, txtOVC_ONNAME);
                    #region 匯入下拉式選單
                    CommonMTS.list_dataImport_CURRENCY(drpOVC_CLAIM_CURRENCY, false); //幣別
                    CommonMTS.list_dataImport_CLAIM_CONDITION(drpOVC_CLAIM_CONDITION, true); //索賠情形
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
            else            if (bld == null)
                strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 不存在！ </p>";
            TBGMT_CLAIM claim = MTSE.TBGMT_CLAIM.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
            if (claim != null)
                strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 已申請索賠通知書，不可重複申請！ </p>";

            if (strMessage.Equals(string.Empty))
            {
                TBGMT_PORTS portArr = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(bld.OVC_ARRIVE_PORT)).FirstOrDefault();
                TBGMT_PORTS portStr = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(bld.OVC_START_PORT)).FirstOrDefault();
                if (portArr!=null && portArr.OVC_IS_ABROAD.Equals("國內")) //進口提單
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
                else if (portStr!=null && portStr.OVC_IS_ABROAD.Equals("國內")) //出口提單
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
                FCommon.AlertShow(pnMessageNew, "danger", "系統訊息", strMessage);
        }
        protected void btnSave_Click(object sender, EventArgs e)
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

            for (int i = 0; i < chkGET_FILE.Items.Count; i++)
            {
                if (chkGET_FILE.Items[i].Selected)
                    strGET_FILE += chkGET_FILE.Items[i].Text + ",";
            }
            if (!strGET_FILE.Equals(string.Empty))
                strGET_FILE = strGET_FILE.Substring(0, strGET_FILE.Length - 1);

            #region 錯誤訊息
            TBGMT_CLAIM claim = MTSE. TBGMT_CLAIM.Where(table=>table.OVC_CLAIM_NO.Equals(strOVC_CLAIM_NO)).FirstOrDefault();
            if (claim != null)
            {
                strMessage += $"<p> 索賠通知書編號：{ strOVC_CLAIM_NO } 已存在！ </p><p> 已重新取得編號，請再試一次。 </p>";
                setAutoNo(); //索賠通知書編號
            }
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
                claim = new TBGMT_CLAIM();
                claim.CLAIM_SN = Guid.NewGuid();
                claim.OVC_CLAIM_NO = strOVC_CLAIM_NO;
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
                claim.ODT_CREATE_DATE = dateNow;
                claim.OVC_CREATE_LOGIN_ID = strUserId;
                claim.ODT_MODIFY_DATE = dateNow;
                claim.OVC_MODIFY_LOGIN_ID = strUserId;
                MTSE.TBGMT_CLAIM.Add(claim);
                MTSE.SaveChanges();
                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), claim.GetType().Name.ToString(), this, "新增");

                FCommon.AlertShow(pnMessageNew, "success", "系統訊息", $"索賠通知書建立成功，索賠通知書編號：{ strOVC_CLAIM_NO }。");
                FCommon.MessageBoxShow(this, "繼續新增索賠通知書", "MTS_C11", false);
                dataImport();
            }
            else
                FCommon.AlertShow(pnMessageNew, "danger", "系統訊息", strMessage);
        }
        #endregion
    }
}