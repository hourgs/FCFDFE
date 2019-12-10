using System;
using System.Linq;
using System.Web.UI;
using System.Data;
using FCFDFE.Content;
using System.Data.Entity;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.B
{
    public partial class MTS_B22_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        //string id, id2, state;
        string strAction;
        Guid guidEINN_SN; //EINN_SN

        #region 副程式
        private void dateImport()
        {
            TBGMT_EINN einn = MTSE.TBGMT_EINN.Where(table => table.EINN_SN.Equals(guidEINN_SN)).FirstOrDefault();
            if (einn != null)
            {
                string strOVC_EDF_NO = einn.OVC_EDF_NO;
                TBGMT_EDF edf = MTSE.TBGMT_EDF.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();
                var queryDetail = MTSE.TBGMT_EDF_DETAIL.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO));
                TBGMT_EDF_DETAIL edf_detail = queryDetail.FirstOrDefault();
                TBGMT_ESO eso = MTSE.TBGMT_ESO.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();

                lblOVC_EINN_NO.Text = einn.OVC_EINN_NO; //投保通知書編號
                lblOVC_EDF_NO.Text = strOVC_EDF_NO;
                if (edf != null)
                {
                    lblOVC_PURCH_NO.Text = edf.OVC_PURCH_NO;
                    lblOVC_START_PORT.Text = CommonMTS.getPortName(edf.OVC_START_PORT);
                    lblOVC_ARRIVE_PORT.Text = CommonMTS.getPortName(edf.OVC_ARRIVE_PORT);
                }
                if (edf_detail != null)
                {
                    lblOVC_CHI_NAME.Text = edf_detail.OVC_CHI_NAME; //物資名稱
                    lblOVC_ITEM_COUNT.Text = queryDetail.Count().ToString();
                    string strONB_ITEM_VALUE = (queryDetail.Sum(table => table.ONB_MONEY) ?? 0).ToString();
                    string strONB_CARRIAGE_CURRENCY = edf_detail.OVC_CURRENCY ?? "";
                    string strONB_CARRIAGE_CURRENCY_Text = CommonMTS.getCurrencyName(strONB_CARRIAGE_CURRENCY);
                    lblONB_ITEM_VALUE.Text = strONB_ITEM_VALUE;
                    lblONB_CARRIAGE_CURRENCY_Text.Text = strONB_CARRIAGE_CURRENCY_Text;
                    lblONB_CARRIAGE_CURRENCY.Value = strONB_CARRIAGE_CURRENCY;
                    lblONB_INS_AMOUNT.Text = strONB_ITEM_VALUE;
                    lblONB_CARRIAGE_CURRENCY2_Text.Text = strONB_CARRIAGE_CURRENCY_Text;
                    lblONB_CARRIAGE_CURRENCY2.Value = strONB_CARRIAGE_CURRENCY;
                }
                if (eso != null)
                    lblOVC_SEA_OR_AIR.Text = eso.OVC_SHIP_COMPANY ?? "";
                txtODT_INS_DATE.Text = FCommon.getDateTime(einn.ODT_INS_DATE);
                string[] strOVC_INS_CONDITIONs = (einn.OVC_INS_CONDITION ?? "").Split(','); //保險條件
                foreach (ListItem item in chkOVC_INS_CONDITION.Items)
                {
                    string strText = item.Text;
                    item.Selected = strOVC_INS_CONDITIONs.Contains(strText);
                }
                txtONB_INS_RATE.Text = (einn.ONB_INS_RATE ?? 0).ToString();
                txtOVC_CESSION_NO.Text = einn.OVC_CESSION_NO;
                txtOVC_PAYMENT_TYPE.Text = einn.OVC_PAYMENT_TYPE;
                FCommon.list_setValue(drpOVC_MILITARY_TYPE, einn.OVC_MILITARY_TYPE);
                decimal decOVC_FINAL_INS_AMOUNT = einn.OVC_FINAL_INS_AMOUNT ?? 0;
                if (decOVC_FINAL_INS_AMOUNT == 0)
                    setOVC_FINAL_INS_AMOUNT();
                else
                    lblOVC_FINAL_INS_AMOUNT.Text = decOVC_FINAL_INS_AMOUNT.ToString();
                //getOVC_FINAL_INS_AMOUNT(); //計算保費
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "找不到該筆 投保通知書！");
        }
        private void setONB_INS_RATE() //計算保險費率
        {
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
                string strCompany = query_com == null ? "" : query_com.FirstOrDefault().OVC_COMPANY;
                //FCommon.list_dataImport(drpCompany, dt, "OVC_COMPANY", "OVC_COMPANY", true);
                if (DateTime.TryParse(txtODT_INS_DATE.Text, out DateTime dt) && !strCompany.Equals(string.Empty))
                {
                    dt = DateTime.Parse(txtODT_INS_DATE.Text);
                    var query =
                        (from rate in MTSE.TBGMT_INSRATE.AsEnumerable()
                         where rate.OVC_INSCOMPNAY.Equals(strCompany)
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
                    }
                    else
                    {
                        txtONB_INS_RATE.Text = "";
                    }
                }
            }
        }
        private void setOVC_FINAL_INS_AMOUNT() //計算保費
        {
            string strOVC_FINAL_INS_AMOUNT = "";
            string strONB_ITEM_VALUE = lblONB_ITEM_VALUE.Text; //物資價值
            decimal.TryParse(strONB_ITEM_VALUE, out decimal decONB_ITEM_VALUE);
            string strONB_INS_RATE = txtONB_INS_RATE.Text; //保險費率
            decimal.TryParse(strONB_INS_RATE, out decimal decONB_INS_RATE);
            //decimal decOVC_FINAL_INS_AMOUNT = einn.OVC_FINAL_INS_AMOUNT ?? 0;
            string strONB_CARRIAGE_CURRENCY = lblONB_CARRIAGE_CURRENCY.Value;  //物資價值－幣別
            decimal decTodayRate = CommonMTS.getRate(strONB_CARRIAGE_CURRENCY);
            decimal decTotal = 0;
            if (decTodayRate != -1)
            {
                strOVC_FINAL_INS_AMOUNT = strONB_ITEM_VALUE + " × " + strONB_INS_RATE + "% × " + decTodayRate.ToString() + " = ";
                decTotal = decONB_ITEM_VALUE * decONB_INS_RATE * decTodayRate / 100;
                if (decTotal > 0 && decTotal < 1)
                {
                    strOVC_FINAL_INS_AMOUNT += "1（保費不足一元，以一元計價）";
                    ViewState["OVC_FINAL_INS_AMOUNT"] = 1;
                }
                else
                {
                    strOVC_FINAL_INS_AMOUNT += decTotal.ToString();
                    ViewState["OVC_FINAL_INS_AMOUNT"] = Math.Round(decTotal);
                }
            }
            else
                strOVC_FINAL_INS_AMOUNT = $"找不到當天匯率，請先至「F15幣別幣值維護」加入 { FCommon.getDateTime(DateTime.Now) } 之匯率";
            lblOVC_FINAL_INS_AMOUNT.Text = strOVC_FINAL_INS_AMOUNT;
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_EINN_NO", Request.QueryString["OVC_EINN_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_EDF_NO", Request.QueryString["OVC_EDF_NO"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_INS_DATE", Request.QueryString["ODT_INS_DATE1"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_INS_DATE_S", Request.QueryString["ODT_INS_DATE1"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_INS_DATE_E", Request.QueryString["ODT_INS_DATE2"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_CREATE_DATE", Request.QueryString["ODT_CREATE_DATE1"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_CREATE_DATE_S", Request.QueryString["ODT_CREATE_DATE1"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_CREATE_DATE_E", Request.QueryString["ODT_CREATE_DATE2"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_MILITARY_TYPE", Request.QueryString["OVC_MILITARY_TYPE"], false);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            { 
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (FCommon.getQueryString(this, "id", out string id, true) && Guid.TryParse(id, out guidEINN_SN))
                {
                    FCommon.getQueryString(this, "action", out strAction, true);
                    //FCommon.getQueryString(this, "id2", out id2, true);
                    if (!IsPostBack)
                    {
                        FCommon.Controls_Attributes("readonly", "true", txtODT_INS_DATE);
                        #region 匯入下拉式選單
                        CommonMTS.list_dataImport_INS_CONDITION(chkOVC_INS_CONDITION, false); //保險條件
                        CommonMTS.list_dataImport_DEPT_CLASS(drpOVC_MILITARY_TYPE, true); //軍種
                        #endregion

                        //string edf_no = "";
                        //string einn_no = "";
                        switch (strAction)
                        {
                            case "Modify":
                                btnSave.Text = "更新投保通知書";
                                btnSave.CssClass = "btn-success";
                                //edf_no = id;// System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(enkey));
                                break;
                            case "Delete":
                                btnSave.Text = "刪除投保通知書";
                                btnSave.CssClass = "btn-danger";
                                btnSave.OnClientClick = "return confirm('您確定要刪除這筆記錄嗎？')";
                                //edf_no = id;//System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(enkey));
                                //einn_no = id2;//System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(enkey2));
                                break;
                        }
                        dateImport();
                    }
                }
                else
                    FCommon.MessageBoxShow(this, "編號錯誤！", $"MTS_B22_1{ getQueryString() }", false);
            }
        }

        #region ~Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            TBGMT_EINN einn = MTSE.TBGMT_EINN.Where(table => table.EINN_SN.Equals(guidEINN_SN)).FirstOrDefault();
            if (einn != null)
            {
                string strOVC_EINN_NO = einn.OVC_EINN_NO;
                switch (strAction)
                {
                    case "Modify":
                        #region 取值
                        //string strOVC_EINN_NO = lblOVC_EINN_NO.Text; //投保通知書編號
                        string strODT_INS_DATE = txtODT_INS_DATE.Text; //投保日期
                        string strOVC_INS_CONDITION = ""; //保險條件
                        foreach (ListItem item in chkOVC_INS_CONDITION.Items)
                            if (item.Selected)
                            {
                                strOVC_INS_CONDITION += strOVC_INS_CONDITION.Equals(string.Empty) ? "" : ",";
                                strOVC_INS_CONDITION += item.Value;
                            }
                        string strONB_INS_RATE = txtONB_INS_RATE.Text; //保險費率
                        string strOVC_CESSION_NO = txtOVC_CESSION_NO.Text;
                        string strOVC_PAYMENT_TYPE = txtOVC_PAYMENT_TYPE.Text; //保費支付方法
                        string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedValue.ToString(); //軍種
                        string strOVC_FINAL_INS_AMOUNT = ViewState["OVC_FINAL_INS_AMOUNT"] != null ? ViewState["OVC_FINAL_INS_AMOUNT"].ToString() : "0"; //保費
                        decimal.TryParse(strOVC_FINAL_INS_AMOUNT, out decimal decOVC_FINAL_INS_AMOUNT);

                        DateTime dateNow = DateTime.Now;
                        #endregion

                        #region 錯誤訊息
                        if (strODT_INS_DATE.Equals(string.Empty))
                            strMessage += "<P> 請選擇 投保日期 </p>";
                        if (strONB_INS_RATE.Equals(string.Empty))
                            strMessage += "<P> 請輸入 保險費率 </p>";
                        if (strOVC_MILITARY_TYPE.Equals(string.Empty))
                            strMessage += "<P> 請選擇 軍種 </p>";

                        bool boolODT_INS_DATE = FCommon.checkDateTime(strODT_INS_DATE, "投保日期", ref strMessage, out DateTime dateODT_INS_DATE);
                        bool boolONB_INS_RATE = FCommon.checkDecimal(strONB_INS_RATE, "保險費率", ref strMessage, out decimal decONB_INS_RATE);
                        #endregion

                        if (strMessage.Equals(string.Empty))
                        {
                            #region 更新 TBGMT_EINN
                            einn.ODT_INS_DATE = dateODT_INS_DATE;
                            einn.OVC_INS_CONDITION = strOVC_INS_CONDITION;
                            einn.ONB_INS_RATE = decONB_INS_RATE;
                            einn.OVC_CESSION_NO = strOVC_CESSION_NO;
                            einn.OVC_PAYMENT_TYPE = strOVC_PAYMENT_TYPE;
                            einn.OVC_MILITARY_TYPE = strOVC_MILITARY_TYPE;
                            einn.ODT_MODIFY_DATE = dateNow;
                            einn.OVC_MODIFY_LOGIN_ID = strUserId;
                            einn.OVC_FINAL_INS_AMOUNT = decOVC_FINAL_INS_AMOUNT;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), einn.GetType().Name.ToString(), this, "修改");
                            #endregion

                            FCommon.AlertShow(PnMessage, "success", "系統訊息", $"編號：{ strOVC_EINN_NO } 之投保通知書 更新成功。");
                            //FCommon.Controls_Clear(txtODT_INS_DATE, txtOVC_PAYMENT_TYPE, drpOVC_MILITARY_TYPE);
                            //for (int i = 0; i < chkOVC_INS_CONDITION.Items.Count; i++)
                            //{
                            //    chkOVC_INS_CONDITION.Items[i].Selected = true;
                            //}
                            //txtONB_INS_RATE.Text = MTSE.TBGMT_INSRATE.Sum(r => r.OVC_INS_RATE).ToString();
                            FCommon.MessageBoxShow(this, "繼續出口投保通知書", $"MTS_B22_1{ getQueryString() }", false);
                        }
                        else
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                        break;
                    case "Delete":
                        try
                        {
                            MTSE.Entry(einn).State = EntityState.Deleted;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), einn.GetType().Name.ToString(), this, "刪除");
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", $"編號：{ strOVC_EINN_NO } 之投保通知書 刪除成功。");
                            //FCommon.Controls_Clear(txtODT_INS_DATE, txtOVC_PAYMENT_TYPE, drpOVC_MILITARY_TYPE);
                            //for (int i = 0; i < chkOVC_INS_CONDITION.Items.Count; i++)
                            //{
                            //    chkOVC_INS_CONDITION.Items[i].Selected = true;
                            //}
                            //txtONB_INS_RATE.Text = MTSE.TBGMT_INSRATE.Sum(r => r.OVC_INS_RATE).ToString();
                            FCommon.MessageBoxShow(this, "繼續出口投保通知書", $"MTS_B22_1{ getQueryString() }", false);
                        }
                        catch
                        {
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "刪除失敗，請聯絡工程師！");
                        }
                        break;
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "投保通知書 不存在！");
        }
        #endregion

        protected void txtONB_INS_RATE_TextChanged(object sender, EventArgs e)
        {
            setOVC_FINAL_INS_AMOUNT();
        }

        protected void chkOVC_INS_CONDITION_SelectedIndexChanged(object sender, EventArgs e)
        {
            setONB_INS_RATE();
        }
    }
}