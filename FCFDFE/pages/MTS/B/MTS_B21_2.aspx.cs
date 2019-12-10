using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.MTS.B
{
    public partial class MTS_B21_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        string id;

        #region 副程式
        private void dataImport()
        {
            if (Guid.TryParse(id, out Guid guidEDF_SN))
            {
                TBGMT_EDF edf = MTSE.TBGMT_EDF.Where(table => table.EDF_SN.Equals(guidEDF_SN)).FirstOrDefault();
                if (edf != null)
                {
                    string strOVC_EDF_NO = edf.OVC_EDF_NO;
                    string strOVC_EINN_NO = "E" + strOVC_EDF_NO.Substring(3, 12);

                    var queryDetail = MTSE.TBGMT_EDF_DETAIL.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO));
                    TBGMT_EDF_DETAIL edf_detail = queryDetail.FirstOrDefault();
                    TBGMT_ESO eso = MTSE.TBGMT_ESO.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();

                    lblOVC_EINN_NO.Text = strOVC_EINN_NO; //投保通知書編號
                    lblOVC_EDF_NO.Text = strOVC_EDF_NO;
                    lblOVC_PURCH_NO.Text = edf.OVC_PURCH_NO;
                    if (edf_detail != null)
                    {
                        lblOVC_CHI_NAME.Text = edf_detail.OVC_CHI_NAME; //物資名稱
                        lblOVC_ITEM_COUNT.Text = queryDetail.Count().ToString();
                        string strONB_ITEM_VALUE = queryDetail.Sum(table => table.ONB_MONEY).ToString();
                        string strONB_CARRIAGE_CURRENCY = edf_detail.OVC_CURRENCY ?? "0";
                        string strONB_CARRIAGE_CURRENCY_Text = CommonMTS.getCurrencyName(strONB_CARRIAGE_CURRENCY);
                        lblONB_ITEM_VALUE.Text = strONB_ITEM_VALUE;
                        lblONB_CARRIAGE_CURRENCY_Text.Text = strONB_CARRIAGE_CURRENCY_Text;
                        lblONB_CARRIAGE_CURRENCY.Value = strONB_CARRIAGE_CURRENCY;
                        lblONB_INS_AMOUNT.Text = strONB_ITEM_VALUE;
                        lblONB_CARRIAGE_CURRENCY2_Text.Text = strONB_CARRIAGE_CURRENCY_Text;
                        lblONB_CARRIAGE_CURRENCY2.Value = strONB_CARRIAGE_CURRENCY;
                    }
                    lblOVC_START_PORT.Text = CommonMTS.getPortName(edf.OVC_START_PORT);
                    lblOVC_ARRIVE_PORT.Text = CommonMTS.getPortName(edf.OVC_ARRIVE_PORT);
                    if (eso != null)
                        lblOVC_SEA_OR_AIR.Text = eso.OVC_SHIP_COMPANY ?? "";
                }
            }
            setONB_INS_RATE(); //保費費率
            //decimal rate = MTSE.TBGMT_INSRATE.Sum(r => r.OVC_INS_RATE);
            //txtONB_INS_RATE.Text = rate.ToString(); //保費費率
        }
        private void setONB_INS_RATE() //計算保險費率
        {
            #region
            //var query = MTSE.TBGMT_INSRATE;
            //decimal decONB_INS_RATE = 0;
            //foreach (ListItem item in chkOVC_INS_CONDITION.Items)
            //    if (item.Selected)
            //    {
            //        string strOVC_INS_NAME = item.Value;
            //        TBGMT_INSRATE insrate = query.Where(table => table.OVC_INS_NAME.Equals(strOVC_INS_NAME)).FirstOrDefault();
            //        if (insrate != null)
            //            decONB_INS_RATE += insrate.OVC_INS_RATE;
            //    }
            //txtONB_INS_RATE.Text = decONB_INS_RATE.ToString();
            #endregion
            string company = lblCompany.Text;
            if (DateTime.TryParse(txtODT_INS_DATE.Text, out DateTime dt) && !company.Equals(string.Empty))
            {
                dt = DateTime.Parse(txtODT_INS_DATE.Text);
                var query =
                    (from rate in MTSE.TBGMT_INSRATE.AsEnumerable()
                     where rate.OVC_INSCOMPNAY.Equals(company)
                     where rate.ODT_START_DATE != null ? DateTime.Compare(DateTime.Parse(rate.ODT_START_DATE.ToString()), dt) <= 0 : false
                     where rate.ODT_END_DATE != null ? DateTime.Compare(DateTime.Parse(rate.ODT_END_DATE.ToString()), dt) >= 0 : false
                     select rate).FirstOrDefault();
                decimal? decRATE = 0;
                if (query != null)
                {
                    decRATE = chkOVC_INS_CONDITION.Items[0].Selected == true && query.OVC_INS_RATE_1 != null ? decRATE + query.OVC_INS_RATE_1 : decRATE;
                    decRATE = chkOVC_INS_CONDITION.Items[1].Selected == true && query.OVC_INS_RATE_2 != null ? decRATE + query.OVC_INS_RATE_2 : decRATE;
                    decRATE = chkOVC_INS_CONDITION.Items[2].Selected == true && query.OVC_INS_RATE_3 != null ? decRATE + query.OVC_INS_RATE_3 : decRATE;
                    decRATE = chkOVC_INS_CONDITION.Items[3].Selected == true && query.OVC_INS_RATE_4 != null ? decRATE + query.OVC_INS_RATE_4 : decRATE;

                    txtONB_INS_RATE.Text = decRATE.ToString();
                    lblONB_INS_RATE.Text = decRATE.ToString() + "%";
                }
                else
                {
                    txtONB_INS_RATE.Text = "";
                    lblONB_INS_RATE.Text = "";
                }
            }
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_EDF_NO", Request.QueryString["OVC_EDF_NO"], false);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getQueryString(this, "id", out id, true); //EDF_SN
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtODT_INS_DATE);
                    #region 匯入下拉式選單
                    CommonMTS.list_dataImport_INS_CONDITION(chkOVC_INS_CONDITION, false); //保險條件
                    CommonMTS.list_dataImport_DEPT_CLASS(drpOVC_MILITARY_TYPE, true); //軍種
                    #endregion

                    dataImport();
                }
            }
        }

        #region ~Click
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            #region 取值
            string strMessage = "";
            string strUserId= Session["userid"].ToString();
            string strOVC_EINN_NO = lblOVC_EINN_NO.Text; //投保通知書編號
            string strOVC_EDF_NO = lblOVC_EDF_NO.Text; //外運資料表編號
            string strONB_ITEM_VALUE =lblONB_ITEM_VALUE.Text; //物資價值
            string strONB_CARRIAGE_CURRENCY = lblONB_CARRIAGE_CURRENCY.Value;
            string strONB_INS_AMOUNT = lblONB_INS_AMOUNT.Text; //投保金額
            string strONB_CARRIAGE_CURRENCY2 = lblONB_CARRIAGE_CURRENCY2.Value;
            string strODT_INS_DATE = txtODT_INS_DATE.Text; //投保日期
            string strOVC_INS_CONDITION = ""; //保險條件
            foreach (ListItem item in chkOVC_INS_CONDITION.Items)
                if (item.Selected)
                {
                    strOVC_INS_CONDITION += strOVC_INS_CONDITION.Equals(string.Empty) ? "" : ",";
                    strOVC_INS_CONDITION += item.Value;
                }
            string strONB_INS_RATE = txtONB_INS_RATE.Text; //保險費率
            string strOVC_SEA_OR_AIR = lblOVC_SEA_OR_AIR.Text; //航運類別
            string strOVC_PAYMENT_TYPE = txtOVC_PAYMENT_TYPE.Text; //保費支付方法
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedValue.ToString(); //軍種

            DateTime dateNow = DateTime.Now;
            #endregion

            //取得匯率
            //var exchangeRate = MTSE.TBGMT_CURRENCY_RATE.Where(table => table.OVC_CURRENCY_CODE.Equals(strONB_CARRIAGE_CURRENCY))
            //    .Where(table=>table.ODT_DATE.Equals(dateNow)).OrderByDescending(table=>table.ODT_DATE).FirstOrDefault();
            //decimal rate = exchangeRate.ONB_RATE;

            //if (exchangeRate == null)
            //    strMessage += "找不到當天匯率，請先至「F15幣別幣值維護」加入" + dateNow.ToShortDateString() + "之匯率";

            #region 錯誤訊息
            if (strOVC_EINN_NO.Equals(string.Empty))
                strMessage += "<P> 投保通知書編號 不得為空！ </p>";
            else
            {
                TBGMT_EINN einn = MTSE.TBGMT_EINN.Where(table => table.OVC_EINN_NO.Equals(strOVC_EINN_NO)).FirstOrDefault();
                if (einn != null)
                    strMessage += $"<P> 編號：{ strOVC_EINN_NO } 之投保通知書 已存在！ </p>";
            }
            if (strOVC_EDF_NO.Equals(string.Empty))
                strMessage += "<P> 外運資料表編號 不得為空！ </p>";
            else
            {
                TBGMT_EINN einn = MTSE.TBGMT_EINN.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();
                if (einn != null)
                    strMessage += $"<P> 外運資料表編號編號：{ strOVC_EDF_NO } 於投保通知書中 已存在！ </p>";
            }
            if (strONB_ITEM_VALUE.Equals(string.Empty))
                strMessage += "<P> 物資價值 不得為空！ </p>";
            if (strONB_INS_AMOUNT.Equals(string.Empty))
                strMessage += "<P> 投保金額 不得為空！ </p>";
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
                #region 新增 TBGMT_EINN
                TBGMT_EINN einn = new TBGMT_EINN();
                einn.OVC_EINN_NO = strOVC_EINN_NO;
                einn.OVC_EDF_NO = strOVC_EDF_NO;
                einn.OVC_INS_CONDITION = strOVC_INS_CONDITION;
                einn.ONB_INS_RATE = decONB_INS_RATE;
                einn.ONB_ITEM_VALUE = decONB_ITEM_VALUE;
                einn.ONB_CARRIAGE_CURRENCY = strONB_CARRIAGE_CURRENCY;
                einn.ONB_INS_AMOUNT = decONB_INS_AMOUNT;
                einn.ONB_CARRIAGE_CURRENCY2 = strONB_CARRIAGE_CURRENCY2;
                einn.ODT_INS_DATE = dateODT_INS_DATE;
                einn.OVC_PAYMENT_TYPE = strOVC_PAYMENT_TYPE;
                einn.OVC_MILITARY_TYPE = strOVC_MILITARY_TYPE;
                einn.ODT_CREATE_DATE = dateNow;
                einn.OVC_CREATE_LOGIN_ID = strUserId;
                einn.ODT_MODIFY_DATE = dateNow;
                einn.OVC_MODIFY_LOGIN_ID = strUserId;
                einn.EINN_SN = Guid.NewGuid();
                einn.OVC_FINAL_INS_AMOUNT = 0; //dONB_INS_RATE * intONB_INS_AMOUNT * rate;
                MTSE.TBGMT_EINN.Add(einn);
                MTSE.SaveChanges();
                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), einn.GetType().Name.ToString(), this, "新增");
                #endregion

                FCommon.AlertShow(PnMessage, "success", "系統訊息", $"編號：{ strOVC_EINN_NO } 之投保通知書 新增成功。");
                //FCommon.Controls_Clear(txtODT_INS_DATE, txtOVC_PAYMENT_TYPE, drpOVC_MILITARY_TYPE);
                //for (int i = 0; i < chkOVC_INS_CONDITION.Items.Count; i++)
                //{
                //    chkOVC_INS_CONDITION.Items[i].Selected = true;
                //}
                //txtONB_INS_RATE.Text = MTSE.TBGMT_INSRATE.Sum(r => r.OVC_INS_RATE).ToString();
                FCommon.MessageBoxShow(this, "繼續出口投保通知書", $"MTS_B21_1{ getQueryString() }", false);
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
                var query =
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
                lblCompany.Text = query == null ? "" : query.FirstOrDefault().OVC_COMPANY;
                //FCommon.list_dataImport(drpCompany, dt, "OVC_COMPANY", "OVC_COMPANY", true);
            }
            setONB_INS_RATE();
        }

        protected void chkOVC_INS_CONDITION_SelectedIndexChanged(object sender, EventArgs e)
        {
            setONB_INS_RATE();
        }
    }
}