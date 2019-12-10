using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;

namespace FCFDFE.pages.MTS.B
{
    public partial class MTS_B12_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        string id;

        #region 副程式
        private void dataImport()
        {
            TBGMT_IINN iinn = MTSE.TBGMT_IINN.Where(table => table.OVC_IINN_NO.Equals(id)).FirstOrDefault();
            if (iinn != null)
            {
                string strOVC_BLD_NO = iinn.OVC_BLD_NO;

                lblOVC_IINN_NO.Text = iinn.OVC_IINN_NO;
                lblOVC_BLD_NO.Text = strOVC_BLD_NO;
                txtOVC_PURCH_NO.Text = iinn.OVC_PURCH_NO;
                txtISSU_NO.Text = iinn.ISSU_NO;
                txtONB_ITEM_VALUE.Text = iinn.ONB_ITEM_VALUE.ToString();
                FCommon.list_setValue(drpONB_CARRIAGE_CURRENCY, iinn.ONB_CARRIAGE_CURRENCY);
                txtONB_INS_AMOUNT.Text = iinn.ONB_INS_AMOUNT.ToString();
                FCommon.list_setValue(drpONB_CARRIAGE_CURRENCY2, iinn.ONB_CARRIAGE_CURRENCY2);
                txtODT_INS_DATE.Text = FCommon.getDateTime(iinn.ODT_INS_DATE);
                FCommon.list_setValue(drpOVC_DELIVERY_CONDITION, iinn.OVC_DELIVERY_CONDITION);
                lblOVC_INS_CONDITION.Text = iinn.OVC_INS_CONDITION;
                FCommon.list_setValue(drpOVC_PURCHASE_TYPE, iinn.OVC_PURCHASE_TYPE);
                txtONB_INS_RATE.Text = iinn.ONB_INS_RATE.ToString();
                txtPOLICY_NO.Text = iinn.POLICY_NO;
                txtOVC_PAYMENT_TYPE.Text = iinn.OVC_PAYMENT_TYPE;
                FCommon.list_setValue(drpOVC_MILITARY_TYPE, iinn.OVC_MILITARY_TYPE);
                lblOVC_FINAL_INS_AMOUNT.Text = iinn.OVC_FINAL_INS_AMOUNT.ToString();
                txtOVC_NOTE.Text = iinn.OVC_NOTE;

                TBGMT_BLD bld = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                if (bld != null)
                {
                    lblONB_QUANITY.Text = bld.ONB_QUANITY.ToString();
                    lblOVC_SHIP_COMPANY.Text = bld.OVC_SHIP_COMPANY;
                    lblOVC_START_PORT.Text = CommonMTS.getPortName(bld.OVC_START_PORT);
                    //lblODT_START_DATE.Text = FCommon.getDateTime(bld.REAL_START_DATE); //實際啟運日期
                    lblODT_START_DATE.Text = FCommon.getDateTime(bld.ODT_START_DATE);
                    lblOVC_ARRIVE_PORT.Text = CommonMTS.getPortName(bld.OVC_ARRIVE_PORT);
                }

                TBGMT_ICR icr = MTSE.TBGMT_ICR.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                if (icr != null)
                {
                    lblOVC_CHI_NAME.Text = icr.OVC_CHI_NAME;
                    lblODT_IMPORT_DATE.Text = FCommon.getDateTime(icr.ODT_IMPORT_DATE);
                }
            }
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_IINN_NO", Request.QueryString["OVC_IINN_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", Request.QueryString["OVC_BLD_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_PURCH_NO", Request.QueryString["OVC_PURCH_NO"], false);
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
                    FCommon.Controls_Attributes("readonly", "true", txtODT_INS_DATE);
                    #region 匯入下拉式選單
                    CommonMTS.list_dataImport_CURRENCY(drpONB_CARRIAGE_CURRENCY, false); //物資價值幣別
                    CommonMTS.list_dataImport_CURRENCY(drpONB_CARRIAGE_CURRENCY2, false); //投保金額幣別
                    CommonMTS.list_dataImport_DELIVERY_CONDITION(drpOVC_DELIVERY_CONDITION, true); //交貨和保險條件
                    CommonMTS.list_dataImport_PURCHASE_TYPE(drpOVC_PURCHASE_TYPE, false); //軍售或商購
                    CommonMTS.list_dataImport_DEPT_CLASS(drpOVC_MILITARY_TYPE, true); //軍種
                    #endregion

                    dataImport();
                }
            }
        }

        #region ~Click
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_B12_1{ getQueryString() }");
        }
        protected void btnModify_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strOVC_PURCH_NO = txtOVC_PURCH_NO.Text;
            string strISSU_NO = txtISSU_NO.Text;
            string strONB_ITEM_VALUE = txtONB_ITEM_VALUE.Text;
            string strONB_CARRIAGE_CURRENCY = drpONB_CARRIAGE_CURRENCY.SelectedValue;
            string strONB_INS_AMOUNT = txtONB_INS_AMOUNT.Text;
            string strONB_CARRIAGE_CURRENCY2 = drpONB_CARRIAGE_CURRENCY2.SelectedValue;
            string strODT_INS_DATE = txtODT_INS_DATE.Text;
            string strOVC_DELIVERY_CONDITION = drpOVC_DELIVERY_CONDITION.SelectedValue;
            string strOVC_INS_CONDITION = lblOVC_INS_CONDITION.Text;
            string strOVC_PURCHASE_TYPE = drpOVC_PURCHASE_TYPE.SelectedValue;
            string strONB_INS_RATE = txtONB_INS_RATE.Text;
            string strPOLICY_NO = txtPOLICY_NO.Text;
            string strOVC_PAYMENT_TYPE = txtOVC_PAYMENT_TYPE.Text;
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedValue;
            string strOVC_NOTE = txtOVC_NOTE.Text;
            DateTime dateNow = DateTime.Now;

            #region 錯誤訊息
            TBGMT_IINN iinn = MTSE.TBGMT_IINN.Where(table => table.OVC_IINN_NO.Equals(id)).FirstOrDefault();
            if (iinn == null)
                strMessage += $"<P> 編號：{ id } 之投保通知書編號 不存在！ </p>";
            if (strONB_ITEM_VALUE.Equals(string.Empty))
                strMessage += "<P> 請輸入 物資價值 </p>";
            if (strONB_INS_AMOUNT.Equals(string.Empty))
                strMessage += "<P> 請輸入 投保金額 </p>";
            if (strODT_INS_DATE.Equals(string.Empty))
                strMessage += "<P> 請選擇 投保日期 </p>";
            if (strONB_INS_RATE.Equals(string.Empty))
                strMessage += "<P> 請輸入 保險費率 </p>";
            if (strOVC_MILITARY_TYPE.Equals(string.Empty))
                strMessage += "<P> 請選擇 軍種 </p>";
            bool boolONB_ITEM_VALUE = FCommon.checkDecimal(strONB_ITEM_VALUE, "物資價值", ref strMessage, out decimal decONB_ITEM_VALUE);
            bool boolONB_INS_AMOUNT = FCommon.checkDecimal(strONB_INS_AMOUNT, "投保金額", ref strMessage, out decimal decONB_INS_AMOUNT);
            bool boolODT_INS_DATE = FCommon.checkDateTime(strODT_INS_DATE, "投保日期", ref strMessage, out DateTime dateODT_INS_DATE);
            bool boolONB_INS_RATE = FCommon.checkDecimal(strONB_INS_RATE, "保險費率", ref strMessage, out decimal decONB_INS_RATE);
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                try
                {
                    iinn.OVC_PURCH_NO = strOVC_PURCH_NO;
                    iinn.ISSU_NO = strISSU_NO;
                    iinn.ONB_ITEM_VALUE = decONB_ITEM_VALUE;
                    iinn.ONB_CARRIAGE_CURRENCY = strONB_CARRIAGE_CURRENCY;
                    iinn.ONB_INS_AMOUNT = decONB_INS_AMOUNT;
                    iinn.ONB_CARRIAGE_CURRENCY2 = strONB_CARRIAGE_CURRENCY2;
                    iinn.ODT_INS_DATE = dateODT_INS_DATE;
                    iinn.OVC_DELIVERY_CONDITION = strOVC_DELIVERY_CONDITION;
                    iinn.OVC_INS_CONDITION = strOVC_INS_CONDITION;
                    iinn.OVC_PURCHASE_TYPE = strOVC_PURCHASE_TYPE;
                    iinn.ONB_INS_RATE = decONB_INS_RATE;
                    iinn.POLICY_NO = strPOLICY_NO;
                    iinn.OVC_PAYMENT_TYPE = strOVC_PAYMENT_TYPE;
                    iinn.OVC_MILITARY_TYPE = strOVC_MILITARY_TYPE;
                    iinn.OVC_NOTE = strOVC_NOTE;
                    iinn.ODT_MODIFY_DATE = dateNow;
                    iinn.OVC_MODIFY_LOGIN_ID = strUserId;
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), iinn.GetType().Name.ToString(), this, "修改");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", $"編號：{ id } 之投保通知書 修改成功。");
                }
                catch
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "修改失敗，請聯絡工程師！");
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        #endregion

        protected void txtODT_INS_DATE_TextChanged(object sender, EventArgs e)
        {
            if (DateTime.TryParse(txtODT_INS_DATE.Text, out DateTime date))
            {
                date = DateTime.Parse(txtODT_INS_DATE.Text);
                var query_com =
                    from dbcompany in MTSE.TBGMT_COMPANY.AsEnumerable()
                    join rate in MTSE.TBGMT_INSRATE.AsEnumerable() on dbcompany.CO_SN equals rate.OVC_CO_SN
                    where dbcompany.OVC_CO_TYPE.Equals("1")
                    where rate.ODT_START_DATE != null ? DateTime.Compare(DateTime.Parse(rate.ODT_START_DATE.ToString()), date) <= 0 : false
                    where rate.ODT_END_DATE != null ? DateTime.Compare(DateTime.Parse(rate.ODT_END_DATE.ToString()), date) >= 0 : false
                    select new
                    {
                        dbcompany.CO_SN,
                        OVC_COMPANY = dbcompany.OVC_COMPANY,
                    };
                //DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                //FCommon.list_dataImport(drpCompany, dt, "OVC_COMPANY", "OVC_COMPANY", true);

                string company = query_com == null ? "" : query_com.FirstOrDefault().OVC_COMPANY;
                if (DateTime.TryParse(txtODT_INS_DATE.Text, out DateTime dt))
                {
                    dt = DateTime.Parse(txtODT_INS_DATE.Text);
                    var query =
                        (from rate in MTSE.TBGMT_INSRATE.AsEnumerable()
                         where rate.OVC_INSCOMPNAY.Equals(company)
                         where rate.ODT_START_DATE != null ? DateTime.Compare(DateTime.Parse(rate.ODT_START_DATE.ToString()), dt) <= 0 : false
                         where rate.ODT_END_DATE != null ? DateTime.Compare(DateTime.Parse(rate.ODT_END_DATE.ToString()), dt) >= 0 : false
                         select rate).FirstOrDefault();
                    decimal? decEX_WORK = 0, decELSE = 0;
                    if (query != null)
                    {
                        decEX_WORK = query.OVC_INS_NAME_1 == "Y" && query.OVC_INS_RATE_1 != null ? decEX_WORK + query.OVC_INS_RATE_1 : decEX_WORK;
                        decELSE = query.OVC_INS_NAME_1 == "Y" && query.OVC_INS_RATE_1 != null ? decELSE + query.OVC_INS_RATE_1 : decELSE;
                        decEX_WORK = query.OVC_INS_NAME_2 == "Y" && query.OVC_INS_RATE_2 != null ? decEX_WORK + query.OVC_INS_RATE_2 : decEX_WORK;
                        decELSE = query.OVC_INS_NAME_2 == "Y" && query.OVC_INS_RATE_2 != null ? decELSE + query.OVC_INS_RATE_2 : decELSE;
                        decEX_WORK = query.OVC_INS_NAME_3 == "Y" && query.OVC_INS_RATE_3 != null ? decEX_WORK + query.OVC_INS_RATE_3 : decEX_WORK;
                        decEX_WORK = query.OVC_INS_NAME_4 == "Y" && query.OVC_INS_RATE_4 != null ? decEX_WORK + query.OVC_INS_RATE_4 : decEX_WORK;
                        decELSE = query.OVC_INS_NAME_4 == "Y" && query.OVC_INS_RATE_4 != null ? decELSE + query.OVC_INS_RATE_4 : decELSE;

                        if (drpOVC_DELIVERY_CONDITION.SelectedValue == "未輸入")
                            txtONB_INS_RATE.Text = query.OVC_INS_RATE.ToString();
                        else if (drpOVC_DELIVERY_CONDITION.SelectedValue == "EX WORK")
                            txtONB_INS_RATE.Text = decEX_WORK.ToString();
                        else
                            txtONB_INS_RATE.Text = decELSE.ToString();
                    }
                    else
                        txtONB_INS_RATE.Text = "";
                }
            }
        }

        protected void drpOVC_DELIVERY_CONDITION_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (drpOVC_DELIVERY_CONDITION.SelectedValue.Equals("EX WORK"))
            //{
            //    //	InsRateTextBox.Text="0.059";
            //    //					InsRateTextBox.Text="0.0169";//0206
            //    //20170103_Rate Changed
            //    //InsRateTextBox.Text="0.014786";
            //    if (txtONB_INS_RATE.Text != "0.014786")
            //    {
            //        txtONB_INS_RATE.Text = "0.0133";
            //    }
            //    lblOVC_INS_CONDITION.Text = "全險,在台內陸險,在外內陸險,兵險";
            //}
            //else
            //{
            //    //InsRateTextBox.Text="0.05015";
            //    //					InsRateTextBox.Text="0.014365";//0206
            //    //20170103_Rate Changed
            //    //InsRateTextBox.Text="0.0125681";
            //    if (txtONB_INS_RATE.Text != "0.0125681")
            //    {
            //        txtONB_INS_RATE.Text = "0.011305";
            //    }
            //    lblOVC_INS_CONDITION.Text = "全險,在台內陸險,兵險";
            //
            //}
            if (DateTime.TryParse(txtODT_INS_DATE.Text, out DateTime date))
            {
                date = DateTime.Parse(txtODT_INS_DATE.Text);
                var query_com =
                from dbcompany in MTSE.TBGMT_COMPANY.AsEnumerable()
                join rate in MTSE.TBGMT_INSRATE.AsEnumerable() on dbcompany.CO_SN equals rate.OVC_CO_SN
                where dbcompany.OVC_CO_TYPE.Equals("1")
                where rate.ODT_START_DATE != null ? DateTime.Compare(DateTime.Parse(rate.ODT_START_DATE.ToString()), date) <= 0 : false
                where rate.ODT_END_DATE != null ? DateTime.Compare(DateTime.Parse(rate.ODT_END_DATE.ToString()), date) >= 0 : false
                select new
                {
                    dbcompany.CO_SN,
                    OVC_COMPANY = dbcompany.OVC_COMPANY,
                };

                string company = query_com == null ? "" : query_com.FirstOrDefault().OVC_COMPANY;
                if (DateTime.TryParse(txtODT_INS_DATE.Text, out DateTime dt))
                {
                    dt = DateTime.Parse(txtODT_INS_DATE.Text);
                    var query =
                        (from rate in MTSE.TBGMT_INSRATE.AsEnumerable()
                         where rate.OVC_INSCOMPNAY.Equals(company)
                         where rate.ODT_START_DATE != null ? DateTime.Compare(DateTime.Parse(rate.ODT_START_DATE.ToString()), dt) <= 0 : false
                         where rate.ODT_END_DATE != null ? DateTime.Compare(DateTime.Parse(rate.ODT_END_DATE.ToString()), dt) >= 0 : false
                         select rate).FirstOrDefault();
                    decimal? decEX_WORK = 0, decELSE = 0;
                    if (query != null)
                    {
                        decEX_WORK = query.OVC_INS_NAME_1 == "Y" && query.OVC_INS_RATE_1 != null ? decEX_WORK + query.OVC_INS_RATE_1 : decEX_WORK;
                        decELSE = query.OVC_INS_NAME_1 == "Y" && query.OVC_INS_RATE_1 != null ? decELSE + query.OVC_INS_RATE_1 : decELSE;
                        decEX_WORK = query.OVC_INS_NAME_2 == "Y" && query.OVC_INS_RATE_2 != null ? decEX_WORK + query.OVC_INS_RATE_2 : decEX_WORK;
                        decELSE = query.OVC_INS_NAME_2 == "Y" && query.OVC_INS_RATE_2 != null ? decELSE + query.OVC_INS_RATE_2 : decELSE;
                        decEX_WORK = query.OVC_INS_NAME_3 == "Y" && query.OVC_INS_RATE_3 != null ? decEX_WORK + query.OVC_INS_RATE_3 : decEX_WORK;
                        decEX_WORK = query.OVC_INS_NAME_4 == "Y" && query.OVC_INS_RATE_4 != null ? decEX_WORK + query.OVC_INS_RATE_4 : decEX_WORK;
                        decELSE = query.OVC_INS_NAME_4 == "Y" && query.OVC_INS_RATE_4 != null ? decELSE + query.OVC_INS_RATE_4 : decELSE;

                        if (drpOVC_DELIVERY_CONDITION.SelectedValue == "未輸入")
                            txtONB_INS_RATE.Text = query.OVC_INS_RATE.ToString();
                        else if (drpOVC_DELIVERY_CONDITION.SelectedValue == "EX WORK")
                            txtONB_INS_RATE.Text = decEX_WORK.ToString();
                        else
                            txtONB_INS_RATE.Text = decELSE.ToString();
                    }
                    else
                        txtONB_INS_RATE.Text = "";
                }
            }
        }
    }
}