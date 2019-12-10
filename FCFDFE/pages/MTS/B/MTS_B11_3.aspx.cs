using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;

namespace FCFDFE.pages.MTS.B
{
    public partial class MTS_B11_3 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();

        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", Request.QueryString["OVC_BLD_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_SHIP_COMPANY", Request.QueryString["OVC_SHIP_COMPANY"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_START_DATE_S", Request.QueryString["ODT_START_DATE_S"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_START_DATE_E", Request.QueryString["ODT_START_DATE_E"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_STATUS", Request.QueryString["OVC_STATUS"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", Request.QueryString["OVC_TRANSER_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_SEA_OR_AIR", Request.QueryString["OVC_SEA_OR_AIR"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_MILITARY_TYPE", Request.QueryString["OVC_MILITARY_TYPE"], false);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtODT_INS_DATE);
                    #region 匯入下拉式選單
                    CommonMTS.list_dataImport_CURRENCY(drpONB_CARRIAGE_CURRENCY, false); //物資價值幣別
                    CommonMTS.list_dataImport_CURRENCY(drpONB_CARRIAGE_CURRENCY2, false); //投保金額幣別
                    CommonMTS.list_dataImport_DELIVERY_CONDITION(drpOVC_DELIVERY_CONDITION, true); //交貨和保險條件
                    CommonMTS.list_dataImport_PURCHASE_TYPE(drpOVC_PURCHASE_TYPE, false); //軍售或商購
                    CommonMTS.list_dataImport_DEPT_CLASS(drpOVC_MILITARY_TYPE, true); //軍種
                    //CommonMTS.list_dataImport_COMPANY(drpCompany, CommonMTS.COMPANY_TYPE.InsuranceCompany, true); //保險公司
                    //var query =
                    //    from dbcompany in MTSE.TBGMT_COMPANY
                    //    where dbcompany.OVC_CO_TYPE.Equals("1")
                    //    select new
                    //    {
                    //        dbcompany.CO_SN,
                    //        OVC_COMPANY = dbcompany.OVC_COMPANY,
                    //    };
                    //DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                    //FCommon.list_dataImport(drpCompany, dt, "OVC_COMPANY", "OVC_COMPANY", true);
                    #endregion
                }
            }
        }

        #region ~Click
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_B11_1{ getQueryString() }");
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_PURCH_NO = txtPURCH_NO.Text;
            string strONB_ITEM_VALUE = txtONB_ITEM_VALUE.Text;
            string strONB_CARRIAGE_CURRENCY = drpONB_CARRIAGE_CURRENCY.SelectedValue;
            string strONB_INS_AMOUNT = txtONB_INS_AMOUNT.Text;
            string strONB_CARRIAGE_CURRENCY2 = drpONB_CARRIAGE_CURRENCY2.SelectedValue;
            string strODT_INS_DATE = txtODT_INS_DATE.Text;
            string strOVC_DELIVERY_CONDITION = drpOVC_DELIVERY_CONDITION.SelectedValue;
            string strOVC_PURCHASE_TYPE = drpOVC_PURCHASE_TYPE.SelectedValue;
            string strONB_INS_RATE = txtONB_INS_RATE.Text;
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedValue;
            DateTime dateNow = DateTime.TryParse(txtODT_INS_DATE.Text, out DateTime dt) ? DateTime.Parse(txtODT_INS_DATE.Text) : DateTime.Now;

            #region 取得投保通知書編號
            int iho_eso_num = 0;
            //投保通知書編號編碼
            int year = FCommon.getTaiwanYear(dateNow);
            string judge_iinn_no = "I" + year.ToString("000") + strOVC_MILITARY_TYPE;
            TBGMT_IINN iinn_no = MTSE.TBGMT_IINN.Where(table => table.OVC_IINN_NO.StartsWith(judge_iinn_no)).OrderByDescending(table => table.OVC_IINN_NO).FirstOrDefault();
            if (iinn_no != null)
            {
                string strOVC_IINN_NO_Ori = iinn_no.OVC_IINN_NO;
                int intLength_Ori = strOVC_IINN_NO_Ori.Length;
                string strNum = strOVC_IINN_NO_Ori.Substring(intLength_Ori - 7); //取末7碼
                int.TryParse(strNum, out iho_eso_num);
            }
            iho_eso_num++;
            string strOVC_IINN_NO = judge_iinn_no + iho_eso_num.ToString("0000000");
            #endregion

            #region 錯誤訊息
            TBGMT_IINN iinn = MTSE.TBGMT_IINN.Where(table => table.OVC_IINN_NO.Equals(strOVC_IINN_NO)).FirstOrDefault();
            if (iinn != null)
                strMessage += "<P> 投保通知書編號 產生錯誤！ </p>";
            if (strOVC_BLD_NO.Equals(string.Empty))
                strMessage += "<P> 請輸入 提單編號 </p>";
            else
            {
                iinn = MTSE.TBGMT_IINN.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                if (iinn != null)
                    strMessage += $"<P> 提單編號：{ strOVC_BLD_NO } 於投保通知書中 已存在！ </p>";
            }
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
                    iinn = new TBGMT_IINN();
                    iinn.OVC_IINN_NO = strOVC_IINN_NO;
                    iinn.OVC_BLD_NO = strOVC_BLD_NO;
                    iinn.OVC_PURCH_NO = strOVC_PURCH_NO;
                    iinn.ONB_ITEM_VALUE = decONB_ITEM_VALUE;
                    iinn.ONB_CARRIAGE_CURRENCY = strONB_CARRIAGE_CURRENCY;
                    iinn.ONB_INS_AMOUNT = decONB_INS_AMOUNT;
                    iinn.ONB_CARRIAGE_CURRENCY2 = strONB_CARRIAGE_CURRENCY2;
                    iinn.ODT_INS_DATE = dateODT_INS_DATE;
                    iinn.OVC_DELIVERY_CONDITION = strOVC_DELIVERY_CONDITION;
                    iinn.OVC_PURCHASE_TYPE = strOVC_PURCHASE_TYPE;
                    iinn.ONB_INS_RATE = decONB_INS_RATE;
                    iinn.OVC_MILITARY_TYPE = strOVC_MILITARY_TYPE;
                    iinn.ODT_CREATE_DATE = dateNow;
                    iinn.OVC_CREATE_LOGIN_ID = strUserId;
                    iinn.ODT_MODIFY_DATE = dateNow;
                    iinn.OVC_MODIFY_LOGIN_ID = strUserId;
                    iinn.IINN_SN = Guid.NewGuid();

                    MTSE.TBGMT_IINN.Add(iinn);
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), iinn.GetType().Name.ToString(), this, "新增");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", $"編號：{ strOVC_IINN_NO } 之投保通知書 新增成功。");
                }
                catch
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "新增失敗，請聯絡工程師！");
                }
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
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
                lblCompany.Text = query_com == null ? "" : query_com.FirstOrDefault().OVC_COMPANY;
                //FCommon.list_dataImport(drpCompany, dt, "OVC_COMPANY", "OVC_COMPANY", true);

                string company = lblCompany.Text;
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
            #region
            //if (drpOVC_DELIVERY_CONDITION.SelectedValue == "EX WORK")
            //{
            #region
            //	InsRateTextBox.Text="0.059";
            //					InsRateTextBox.Text="0.0169";//0206
            //20170103_Rate Changed
            //InsRateTextBox.Text="0.014786";
            #endregion
            //    if (txtONB_INS_RATE.Text != "0.014786")
            //    {
            //        txtONB_INS_RATE.Text = "0.0133";
            //    }
            //    lblOVC_INS_CONDITION.Text = "全險,在台內陸險,在外內陸險,兵險";
            //}
            //else
            //{
            #region
            //InsRateTextBox.Text="0.05015";
            //					InsRateTextBox.Text="0.014365";//0206
            //20170103_Rate Changed
            //InsRateTextBox.Text="0.0125681";
            #endregion
            //    if (txtONB_INS_RATE.Text != "0.0125681")
            //    {
            //        txtONB_INS_RATE.Text = "0.011305";
            //    }
            //    lblOVC_INS_CONDITION.Text = "全險,在台內陸險,兵險";
            //
            //}
            #endregion
            string company = lblCompany.Text;
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