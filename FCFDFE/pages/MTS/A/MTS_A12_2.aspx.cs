using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.Data.Entity;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A12_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();

        #region 副程式
        private void dataImport_List(string id)
        {
            ViewState["id"] = id;
            string strODT_CREATE_DATE, strOVC_BLD_NO, strOVC_SHIP_NAME, strOVC_VOYAGE, strOVC_TRANSER_DEPT_CDE, strOVC_IS_SECURITY;
            FCommon.getQueryString(this, "ODT_CREATE_DATE", out strODT_CREATE_DATE, true);
            FCommon.getQueryString(this, "OVC_BLD_NO", out strOVC_BLD_NO, true);
            FCommon.getQueryString(this, "OVC_SHIP_NAME", out strOVC_SHIP_NAME, true);
            FCommon.getQueryString(this, "OVC_VOYAGE", out strOVC_VOYAGE, true);
            FCommon.getQueryString(this, "OVC_TRANSER_DEPT_CDE", out strOVC_TRANSER_DEPT_CDE, true);
            FCommon.getQueryString(this, "OVC_IS_SECURITY", out strOVC_IS_SECURITY, true);
            var query =
                from bld in MTSE.TBGMT_BLD.AsEnumerable()
                join arrport in MTSE.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals arrport.OVC_PORT_CDE
                join strport in MTSE.TBGMT_PORTS on bld.OVC_START_PORT equals strport.OVC_PORT_CDE
                where arrport.OVC_IS_ABROAD.Equals("國內")
                orderby bld.OVC_BLD_NO
                select new
                {
                    OVC_BLD_NO = bld.OVC_BLD_NO,
                    OVC_SHIP_NAME = bld.OVC_SHIP_NAME ?? "",
                    OVC_VOYAGE = bld.OVC_VOYAGE ?? "",
                    OVC_START_PORT = strport.OVC_PORT_CHI_NAME,
                    OVC_ARRIVE_PORT = bld.OVC_ARRIVE_PORT ?? "",
                    bld.ODT_MODIFY_DATE,
                    ODT_CREATE_DATE = bld.ODT_CREATE_DATE ?? DateTime.MinValue,
                    OVC_IS_SECURITY_Value = bld.OVC_IS_SECURITY ?? 0
                };

            if (!strODT_CREATE_DATE.Equals(string.Empty))
                query = query.Where(table => FCommon.getTaiwanYear(Convert.ToDateTime(table.ODT_CREATE_DATE)) == Convert.ToInt16(strODT_CREATE_DATE));
            //{
            //    int numyear = Convert.ToInt32(strODT_CREATE_DATE) + 1911;
            //    DateTime creat = Convert.ToDateTime(numyear + "/1/1");
            //    DateTime end = Convert.ToDateTime(numyear + "/12/31");
            //    query = query.Where(table => table.ODT_CREATE_DATE >= creat && table.ODT_CREATE_DATE <= end);
            //}
            if (!strOVC_BLD_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
            if (!strOVC_SHIP_NAME.Equals(string.Empty))
                query = query.Where(table => table.OVC_SHIP_NAME.Contains(strOVC_SHIP_NAME));
            if (!strOVC_VOYAGE.Equals(string.Empty))
                query = query.Where(table => table.OVC_VOYAGE.Contains(strOVC_VOYAGE));
            if (!strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
            {
                var table = GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("TR"))
                    .Where(t => t.OVC_PHR_PARENTS.Equals(strOVC_TRANSER_DEPT_CDE))
                    .Select(t => t.OVC_PHR_ID).ToArray();

                query = query.Where(t => table.Contains(t.OVC_ARRIVE_PORT));
            }
            if (!strOVC_IS_SECURITY.Equals(string.Empty) && decimal.TryParse(strOVC_IS_SECURITY, out decimal decOVC_IS_SECURITY))
                query = query.Where(table => table.OVC_IS_SECURITY_Value == decOVC_IS_SECURITY);
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            int intCount = dt.Rows.Count, nowpage = 0, data = 0;
            if (intCount > 0)
            {
                string[] arrOVC_BLD_NO = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    arrOVC_BLD_NO[i] = dt.Rows[i]["OVC_BLD_NO"].ToString();
                }

                lblall.Text = intCount.ToString();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (arrOVC_BLD_NO[i] .Equals( id))
                    {
                        nowpage = i + 1;
                        lblnow.Text = nowpage.ToString();
                        data = i;
                    }
                }
                if (nowpage != 0)
                {
                    ViewState["sumpage"] = intCount;
                    ViewState["nowpage"] = nowpage;
                    ViewState["data"] = data;
                    ViewState.Add("arrOVC_BLD_NO", new string[dt.Rows.Count]);
                    ViewState["arrOVC_BLD_NO"] = arrOVC_BLD_NO;
                    TBGMT_BLD_dataImport();
                }
                else
                    FCommon.MessageBoxShow(this, "資料錯誤！", $"MTS_A12_1{ getQueryString() }", false);
            }
            else
                FCommon.MessageBoxShow(this, "查詢錯誤！", $"MTS_A12_1{ getQueryString() }", false);
            button();
        }
        private void TBGMT_BLD_dataImport()
        {
            string strid = ViewState["id"].ToString();
            FCommon.Controls_Clear(pnData, "lblnow", "lblall");
            TBGMT_BLD codetable = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(strid)).FirstOrDefault();
            string strDefaultCurrency = VariableMTS.strDefaultCurrency; //預設幣別
            string strONB_CARRIAGE_CURRENCY_I = strDefaultCurrency; //幣別
            string strONB_CARRIAGE_CURRENCY = strDefaultCurrency; //運費幣別
            string strOVC_PAYMENT_TYPE = "到付"; //若無資料，預設為"到付"
            if (codetable != null)
            {
                lblOVC_BLD_NO.Text = codetable.OVC_BLD_NO;
                FCommon.list_setValue(drpOVC_SHIP_COMPANY, codetable.OVC_SHIP_COMPANY);
                FCommon.list_setValue(drpOVC_SEA_OR_AIR, codetable.OVC_SEA_OR_AIR);
                txtOVC_SHIP_NAME.Text = codetable.OVC_SHIP_NAME;
                txtOVC_VOYAGE.Text = codetable.OVC_VOYAGE;
                txtODT_START_DATE.Text = FCommon.getDateTime(codetable.ODT_START_DATE);
                CommonMTS.list_dataImport_PORT(drpOVC_SHIP_COMPANY, drpOVC_SEA_OR_AIR, drpOVC_ARRIVE_PORT); //承運航商，海空運別，啟運/抵達港埠(國內)
                FCommon.list_setValue(drpOVC_ARRIVE_PORT, codetable.OVC_ARRIVE_PORT);
                string strOVC_IS_SECURITY = (codetable.OVC_IS_SECURITY ?? 0).ToString();
                FCommon.list_setValue(drpOVC_IS_SECURITY, strOVC_IS_SECURITY);
                drpOVC_IS_SECURITY.SelectedIndex = codetable.OVC_IS_SECURITY == 1 ? 0 : 1;
                txtODT_PLN_ARRIVE_DATE.Text = FCommon.getDateTime(codetable.ODT_PLN_ARRIVE_DATE);
                txtODT_ACT_ARRIVE_DATE.Text = FCommon.getDateTime(codetable.ODT_ACT_ARRIVE_DATE);
                if (txtODT_ACT_ARRIVE_DATE.Text == "0001-01-01")
                {
                    txtODT_ACT_ARRIVE_DATE.Text = "";
                }
                string strOVC_START_PORT = codetable.OVC_START_PORT;
                txtOVC_PORT_CDE.Text = strOVC_START_PORT;
                TBGMT_PORTS port = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(strOVC_START_PORT)).FirstOrDefault();
                if (port != null)
                    txtOVC_CHI_NAME.Text = port.OVC_PORT_CHI_NAME;
                txtONB_QUANITY.Text = codetable.ONB_QUANITY.ToString();
                string strOVC_QUANITY_UNIT = codetable.OVC_QUANITY_UNIT;
                FCommon.list_setValue(drpOVC_QUANITY_UNIT, strOVC_QUANITY_UNIT);
                bool isOther = !drpOVC_QUANITY_UNIT.SelectedValue.Equals(strOVC_QUANITY_UNIT);
                if (isOther)
                {
                    FCommon.list_setValue(drpOVC_QUANITY_UNIT, "其他");
                    txtOVC_QUANITY_UNIT.Text = strOVC_QUANITY_UNIT;
                }
                txtOVC_QUANITY_UNIT.Visible = isOther;
                txtONB_VOLUME.Text = codetable.ONB_VOLUME.ToString();
                txtONB_ON_SHIP_VOLUME.Text = codetable.ONB_ON_SHIP_VOLUME.ToString();
                FCommon.list_setValue(drpOVC_VOLUME_UNIT, codetable.OVC_VOLUME_UNIT);
                txtONB_WEIGHT.Text = codetable.ONB_WEIGHT.ToString();
                FCommon.list_setValue(drpOVC_WEIGHT_UNIT, codetable.OVC_WEIGHT_UNIT);
                txtONB_ITEM_VALUE.Text = codetable.ONB_ITEM_VALUE.ToString();
                strONB_CARRIAGE_CURRENCY_I = codetable.ONB_CARRIAGE_CURRENCY_I ?? strONB_CARRIAGE_CURRENCY_I;
                txtONB_CARRIAGE.Text = codetable.ONB_CARRIAGE.ToString();
                strONB_CARRIAGE_CURRENCY = codetable.ONB_CARRIAGE_CURRENCY ?? strONB_CARRIAGE_CURRENCY;
                strOVC_PAYMENT_TYPE = codetable.OVC_PAYMENT_TYPE ?? strOVC_PAYMENT_TYPE;
                FCommon.list_setValue(drpOVC_MILITARY_TYPE, codetable.OVC_MILITARY_TYPE);
                lblODT_CREATE_DATE.Text = FCommon.getDateTime(codetable.ODT_CREATE_DATE);
                lblODT_MODIFY_DATE.Text = FCommon.getDateTime(codetable.ODT_MODIFY_DATE);
                //lblOVC_CREATE_LOGIN_ID.Text = codetable.OVC_CREATE_LOGIN_ID;
                lblOVC_CREATE_LOGIN_ID.Text = FCommon.getUserName(codetable.OVC_CREATE_LOGIN_ID);
                if (codetable.ONB_DANGER_PRO == "有")
                    drpONB_DANGER_PRO.SelectedValue = "2";
                else
                    drpONB_DANGER_PRO.SelectedValue = "1";
            }

            FCommon.list_setValue(drpONB_CARRIAGE_CURRENCY_I, strONB_CARRIAGE_CURRENCY_I);
            FCommon.list_setValue(drpONB_CARRIAGE_CURRENCY, strONB_CARRIAGE_CURRENCY);
            FCommon.list_setValue(rdoOVC_PAYMENT_TYPE, strOVC_PAYMENT_TYPE);
        }

        private void button()
        {
            bool isPrev = false, isNext = false;
            if (ViewState["nowpage"] != null || ViewState["sumpage"] != null)
            {
                isPrev = (int)ViewState["nowpage"] != 1;
                isNext = (int)ViewState["nowpage"] < (int)ViewState["sumpage"];
            }
            if (isPrev)
                FCommon.Controls_Attributes("disabled", btnPrev);
            else
                FCommon.Controls_Attributes("disabled", "true", btnPrev);

            if (isNext)
                FCommon.Controls_Attributes("disabled", btnNext);
            else
                FCommon.Controls_Attributes("disabled", "true", btnNext);
        }

        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "ODT_CREATE_DATE", Request.QueryString["ODT_CREATE_DATE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", Request.QueryString["OVC_BLD_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_SHIP_NAME", Request.QueryString["OVC_SHIP_NAME"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_VOYAGE", Request.QueryString["OVC_VOYAGE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", Request.QueryString["OVC_TRANSER_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_IS_SECURITY", Request.QueryString["OVC_IS_SECURITY"], false);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            if (FCommon.getAuth(this))
            {
                if (!IsPostBack)
                {
                    if (FCommon.getQueryString(this, "id", out string id, true))
                    {
                        FCommon.Controls_Attributes("readonly", "true", txtODT_START_DATE, txtOVC_CHI_NAME, txtODT_PLN_ARRIVE_DATE, txtODT_ACT_ARRIVE_DATE);
                        #region 下拉式選單匯入資料
                        CommonMTS.list_dataImport_SHIP_COMPANY(drpOVC_SHIP_COMPANY, true, "未輸入"); //承運航商
                        CommonMTS.list_dataImport_SEA_OR_AIR(drpOVC_SEA_OR_AIR, false); //海空運別
                        CommonMTS.list_dataImport_PORT(drpOVC_SHIP_COMPANY, drpOVC_SEA_OR_AIR, drpOVC_ARRIVE_PORT); //承運航商，海空運別，啟運/抵達港埠(國內)
                        CommonMTS.list_dataImport_QUANITY_UNIT(drpOVC_QUANITY_UNIT, true); //件數單位
                        CommonMTS.list_dataImport_VOLUME_UNIT(drpOVC_VOLUME_UNIT, true); //體積單位
                        CommonMTS.list_dataImport_WEIGHT_UNIT(drpOVC_WEIGHT_UNIT, true); //重量單位
                        CommonMTS.list_dataImport_CURRENCY(drpONB_CARRIAGE_CURRENCY_I, false); //物資價值幣別
                        CommonMTS.list_dataImport_CURRENCY(drpONB_CARRIAGE_CURRENCY, false); //運費幣別
                        CommonMTS.list_dataImport_PAYMENT_TYPE_BLD(rdoOVC_PAYMENT_TYPE); //付款方式
                        CommonMTS.list_dataImport_DEPT_CLASS(drpOVC_MILITARY_TYPE, true); //軍種
                        CommonMTS.list_dataImport_IS_SECURITY(drpOVC_IS_SECURITY, false);//機敏軍品
                        #endregion

                        dataImport_List(id);
                    }
                    else
                        FCommon.MessageBoxShow(this, "提單編號錯誤", $"MTS_A12_1{ getQueryString() }", false);
                }
            }
        }

        #region ~Click
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            int data = (int)ViewState["data"] - 1;
            string id = ((string[])ViewState["arrOVC_BLD_NO"])[data];
            ViewState["nowpage"] = (int)ViewState["nowpage"] - 1;
            ViewState["data"] = (int)ViewState["data"] - 1;
            lblnow.Text = ViewState["nowpage"].ToString();
            ViewState["id"] = id;
            TBGMT_BLD_dataImport();
            button();
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            int data = (int)ViewState["data"] + 1;
            string id = ((string[])ViewState["arrOVC_BLD_NO"])[data];
            ViewState["nowpage"] = (int)ViewState["nowpage"] + 1;
            ViewState["data"] = (int)ViewState["data"] + 1;
            lblnow.Text = ViewState["nowpage"].ToString();
            ViewState["id"] = id;
            TBGMT_BLD_dataImport();
            button();
        }
        protected void btnModify_Click(object sender, EventArgs e)
        {
            if (ViewState["id"] != null)
            {
                string strMessage = "";
                string strUserId = Session["userid"].ToString();
                string strOVC_BLD_NO = ViewState["id"].ToString();
                string strOVC_SHIP_COMPANY = drpOVC_SHIP_COMPANY.SelectedValue;
                string strOVC_SEA_OR_AIR = drpOVC_SEA_OR_AIR.SelectedValue;
                string strOVC_SHIP_NAME = txtOVC_SHIP_NAME.Text;
                string strOVC_VOYAGE = txtOVC_VOYAGE.Text;
                string strODT_START_DATE = txtODT_START_DATE.Text;
                string strOVC_ARRIVE_PORT = drpOVC_ARRIVE_PORT.SelectedValue;
                string strODT_PLN_ARRIVE_DATE = txtODT_PLN_ARRIVE_DATE.Text;
                string strODT_ACT_ARRIVE_DATE = txtODT_ACT_ARRIVE_DATE.Text;
                string strOVC_START_PORT = txtOVC_PORT_CDE.Text;
                string strOVC_QUANITY_UNIT = drpOVC_QUANITY_UNIT.SelectedValue;
                if (strOVC_QUANITY_UNIT.Equals("其他")) strOVC_QUANITY_UNIT = txtOVC_QUANITY_UNIT.Text; //若選擇其他，則取得輸入之資料
                string strOVC_VOLUME_UNIT = drpOVC_VOLUME_UNIT.SelectedValue;
                string strOVC_WEIGHT_UNIT = drpOVC_WEIGHT_UNIT.SelectedValue;
                string strONB_CARRIAGE_CURRENCY_I = drpONB_CARRIAGE_CURRENCY_I.SelectedValue;
                string strONB_CARRIAGE_CURRENCY = drpONB_CARRIAGE_CURRENCY.SelectedValue;
                string strOVC_PAYMENT_TYPE= rdoOVC_PAYMENT_TYPE.SelectedValue;
                string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedValue;

                string strONB_QUANITY = txtONB_QUANITY.Text;
                string strONB_VOLUME = txtONB_VOLUME.Text;
                string strONB_WEIGHT = txtONB_WEIGHT.Text;
                string strONB_ITEM_VALUE = txtONB_ITEM_VALUE.Text;
                string strONB_CARRIAGE = txtONB_CARRIAGE.Text;
                string strONB_ON_SHIP_VOLUME = txtONB_ON_SHIP_VOLUME.Text;
                string strONB_DANGER_PRO = drpONB_DANGER_PRO.SelectedItem.Text;

                DateTime dateNow = DateTime.Now;

                #region 錯誤訊息
                if (strOVC_SHIP_COMPANY.Equals(string.Empty))
                    strMessage += "<P> 請選擇 承運航商 </p>";
                if (strOVC_SHIP_COMPANY .Equals( "非合約航商"))
                    if (strOVC_SEA_OR_AIR.Equals(string.Empty))
                        strMessage += "<P> 請輸入 海空運別 </p>";
                if (strOVC_SHIP_NAME.Equals(string.Empty))
                    strMessage += "<P> 請輸入 船機名稱 </p>";
                if (strOVC_VOYAGE.Equals(string.Empty))
                    strMessage += "<P> 請輸入 船機航次 </p>";
                if (strODT_START_DATE.Equals(string.Empty))
                    strMessage += "<P> 請輸入 啟運日期 </p>";
                if (strOVC_START_PORT.Equals(string.Empty))
                    strMessage += "<P> 請選擇 啟運港埠 </p>";
                if (strODT_PLN_ARRIVE_DATE.Equals(string.Empty))
                    strMessage += "<P> 請輸入 預估抵運日期 </p>";
                if (strOVC_ARRIVE_PORT.Equals(string.Empty))
                    strMessage += "<P> 請選擇 抵達港埠 </p>";
                if (strOVC_QUANITY_UNIT.Equals(string.Empty))
                    strMessage += "<P> 請選擇 件數計量單位 </p>";
                if (strOVC_VOLUME_UNIT.Equals(string.Empty))
                    strMessage += "<P> 請輸入 體積計量單位 </p>";
                if (strOVC_WEIGHT_UNIT.Equals(string.Empty))
                    strMessage += "<P> 請輸入 重量計量單位 </p>";
                if (strONB_CARRIAGE_CURRENCY_I.Equals(string.Empty))
                    strMessage += "<P> 請選擇 物資價值幣別 </p>";
                if (strONB_CARRIAGE_CURRENCY.Equals(string.Empty))
                    strMessage += "<P> 請輸入 運費幣別 </p>";
                if (strOVC_MILITARY_TYPE.Equals(string.Empty))
                    strMessage += "<P> 請輸入 軍種 </p>";
                if (strONB_QUANITY .Equals( string.Empty))
                    strMessage += "<P> 請輸入 件數 </p>";
                if (strONB_VOLUME .Equals( string.Empty))
                    strMessage += "<P> 請輸入 體積 </p>";
                //if (strONB_ON_SHIP_VOLUME .Equals( string.Empty)
                //    strMessage += "<P> 請輸入 佔體體積 </p>";
                if (strONB_WEIGHT .Equals( string.Empty))
                    strMessage += "<P> 請輸入 重量 </p>";
                if (strONB_ITEM_VALUE .Equals( string.Empty))
                    strMessage += "<P> 請輸入 物資價值 </p>";
                if (strONB_CARRIAGE .Equals( string.Empty))
                    strMessage += "<P> 請輸入 運費 </p>";

                bool boolONB_QUANITY = FCommon.checkDecimal(strONB_QUANITY, "件數", ref strMessage, out decimal decONB_QUANITY);
                bool boolONB_VOLUME = FCommon.checkDecimal(strONB_VOLUME, "體積", ref strMessage, out decimal decONB_VOLUME);
                bool boolONB_ON_SHIP_VOLUME = FCommon.checkDecimal(strONB_ON_SHIP_VOLUME, "佔艙體積", ref strMessage, out decimal decONB_ON_SHIP_VOLUME);
                bool boolONB_WEIGHT = FCommon.checkDecimal(strONB_WEIGHT, "重量", ref strMessage, out decimal decONB_WEIGHT);
                bool boolONB_ITEM_VALUE = FCommon.checkDecimal(strONB_ITEM_VALUE, "物資價值", ref strMessage, out decimal decONB_ITEM_VALUE);
                bool boolONB_CARRIAGE = FCommon.checkDecimal(strONB_CARRIAGE, "運費", ref strMessage, out decimal decONB_CARRIAGE);
                bool boolODT_START_DATE = FCommon.checkDateTime(strODT_START_DATE, "啟運日期", ref strMessage, out DateTime dateODT_START_DATE);
                bool boolODT_PLN_ARRIVE_DATE = FCommon.checkDateTime(strODT_PLN_ARRIVE_DATE, "預估抵運日期", ref strMessage, out DateTime dateODT_PLN_ARRIVE_DATE);
                bool boolODT_ACT_ARRIVE_DATE = FCommon.checkDateTime(strODT_ACT_ARRIVE_DATE, "實際抵運日期", ref strMessage, out DateTime dateODT_ACT_ARRIVE_DATE);
                if (boolODT_PLN_ARRIVE_DATE && boolODT_START_DATE)
                    if (DateTime.Compare(dateODT_START_DATE, dateODT_PLN_ARRIVE_DATE) > 0)
                        strMessage += "<P> 預估抵運日期 不正確</p>";
                if (boolODT_ACT_ARRIVE_DATE && boolODT_START_DATE)
                    if (DateTime.Compare(dateODT_START_DATE, dateODT_ACT_ARRIVE_DATE) > 0)
                        strMessage += "<P> 實際抵運日期 不正確</p>";
                #endregion

                if (strMessage.Equals(string.Empty))
                {
                    #region TBGMT_BLD 修改資料
                    TBGMT_BLD codetable = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                    if (codetable != null)
                    {
                        //codetable.OVC_BLD_NO = strOVC_BLD_NO;
                        codetable.OVC_SHIP_COMPANY = strOVC_SHIP_COMPANY;
                        codetable.OVC_SEA_OR_AIR = strOVC_SEA_OR_AIR;
                        codetable.OVC_SHIP_NAME = strOVC_SHIP_NAME;
                        codetable.OVC_VOYAGE = strOVC_VOYAGE;
                        codetable.ODT_START_DATE = dateODT_START_DATE;
                        codetable.OVC_START_PORT = strOVC_START_PORT;
                        codetable.ODT_PLN_ARRIVE_DATE = dateODT_PLN_ARRIVE_DATE;
                        if (boolODT_ACT_ARRIVE_DATE) codetable.ODT_ACT_ARRIVE_DATE = dateODT_ACT_ARRIVE_DATE; else codetable.ODT_ACT_ARRIVE_DATE = null;
                        codetable.OVC_ARRIVE_PORT = strOVC_ARRIVE_PORT;
                        codetable.ONB_QUANITY = decONB_QUANITY;
                        codetable.OVC_QUANITY_UNIT = strOVC_QUANITY_UNIT;
                        codetable.ONB_VOLUME = decONB_VOLUME;
                        if (boolONB_ON_SHIP_VOLUME) codetable.ONB_ON_SHIP_VOLUME = decONB_ON_SHIP_VOLUME; else codetable.ONB_ON_SHIP_VOLUME = null;
                        codetable.OVC_VOLUME_UNIT = strOVC_VOLUME_UNIT;
                        codetable.ONB_WEIGHT = decONB_WEIGHT;
                        codetable.OVC_WEIGHT_UNIT = strOVC_WEIGHT_UNIT;
                        codetable.ONB_ITEM_VALUE = decONB_ITEM_VALUE;
                        codetable.ONB_CARRIAGE_CURRENCY_I = strONB_CARRIAGE_CURRENCY_I;
                        codetable.ONB_CARRIAGE = decONB_CARRIAGE;
                        codetable.ONB_CARRIAGE_CURRENCY = strONB_CARRIAGE_CURRENCY;
                        codetable.OVC_PAYMENT_TYPE = strOVC_PAYMENT_TYPE;
                        codetable.OVC_MILITARY_TYPE = strOVC_MILITARY_TYPE;
                        if(strOVC_PAYMENT_TYPE == "預付")
                        codetable.OVC_STATUS = "B-";
                        else
                            codetable.OVC_STATUS = "B";
                        codetable.ODT_MODIFY_DATE = dateNow;
                        codetable.OVC_MODIFY_LOGIN_ID = strUserId;
                        if (drpOVC_IS_SECURITY.SelectedIndex == 0)
                            codetable.OVC_IS_SECURITY = 1;
                        else
                            codetable.OVC_IS_SECURITY = 0;
                        codetable.ONB_DANGER_PRO = strONB_DANGER_PRO;

                        MTSE.SaveChanges();
                        FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), codetable.GetType().Name.ToString(), this, "修改");
                    }
                    #endregion

                    #region TBGMT_BLD_MRPLOG 新增LOG
                    TBGMT_BLD_MRPLOG Updatelog = new TBGMT_BLD_MRPLOG();
                    Updatelog.LOG_LOGIN_ID = strUserId;
                    Updatelog.LOG_TIME = dateNow;
                    Updatelog.LOG_EVENT = "UPDATE";
                    Updatelog.OVC_BLD_NO = strOVC_BLD_NO;
                    Updatelog.OVC_SHIP_COMPANY = strOVC_SHIP_COMPANY;
                    Updatelog.OVC_SEA_OR_AIR = strOVC_SEA_OR_AIR;
                    Updatelog.OVC_SHIP_NAME = strOVC_SHIP_NAME;
                    Updatelog.OVC_VOYAGE = strOVC_VOYAGE;
                    Updatelog.ODT_START_DATE = dateODT_START_DATE;
                    Updatelog.OVC_START_PORT = strOVC_START_PORT;
                    Updatelog.ODT_PLN_ARRIVE_DATE = dateODT_PLN_ARRIVE_DATE;
                    if (boolODT_ACT_ARRIVE_DATE) Updatelog.ODT_ACT_ARRIVE_DATE = dateODT_ACT_ARRIVE_DATE; else Updatelog.ODT_ACT_ARRIVE_DATE = null;
                    Updatelog.OVC_ARRIVE_PORT = strOVC_ARRIVE_PORT;
                    Updatelog.ONB_QUANITY = decONB_QUANITY;
                    Updatelog.OVC_QUANITY_UNIT = strOVC_QUANITY_UNIT;
                    Updatelog.ONB_VOLUME = decONB_VOLUME;
                    if (boolONB_ON_SHIP_VOLUME) codetable.ONB_ON_SHIP_VOLUME = decONB_ON_SHIP_VOLUME; else codetable.ONB_ON_SHIP_VOLUME = null;
                    Updatelog.OVC_VOLUME_UNIT = strOVC_VOLUME_UNIT;
                    Updatelog.ONB_WEIGHT = decONB_WEIGHT;
                    Updatelog.OVC_WEIGHT_UNIT = strOVC_WEIGHT_UNIT;
                    Updatelog.ONB_ITEM_VALUE = decONB_ITEM_VALUE;
                    Updatelog.ONB_CARRIAGE_CURRENCY_I = strONB_CARRIAGE_CURRENCY_I;
                    Updatelog.ONB_CARRIAGE = decONB_CARRIAGE;
                    Updatelog.ONB_CARRIAGE_CURRENCY = strONB_CARRIAGE_CURRENCY;
                    Updatelog.OVC_PAYMENT_TYPE = strOVC_PAYMENT_TYPE;
                    Updatelog.OVC_MILITARY_TYPE = strOVC_MILITARY_TYPE;
                    if (strOVC_PAYMENT_TYPE == "預付")
                        Updatelog.OVC_STATUS = "B-";
                    else
                        Updatelog.OVC_STATUS = "B";
                    Updatelog.ODT_CREATE_DATE = codetable.ODT_CREATE_DATE;
                    Updatelog.ODT_MODIFY_DATE = dateNow;
                    Updatelog.OVC_CREATE_LOGIN_ID = codetable.OVC_CREATE_LOGIN_ID;
                    Updatelog.OVC_MODIFY_LOGIN_ID = strUserId;
                    Updatelog.MRPLOG_SN = Guid.NewGuid();
                    if (drpOVC_IS_SECURITY.SelectedIndex == 0)
                        Updatelog.OVC_IS_SECURITY = 1;
                    else
                        Updatelog.OVC_IS_SECURITY = 0;
                    Updatelog.ONB_DANGER_PRO = strONB_DANGER_PRO;

                    MTSE.TBGMT_BLD_MRPLOG.Add(Updatelog);
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), Updatelog.GetType().Name.ToString(), this, "新增");
                    #endregion

                    FCommon.AlertShow(PnMessage_Modify, "success", "系統訊息", $"提單編號：{ strOVC_BLD_NO } 修改成功。");
                }
                else
                    FCommon.AlertShow(PnMessage_Modify, "danger", "系統訊息", strMessage);
            }
            else
                FCommon.AlertShow(PnMessage_Modify, "danger", "系統訊息", "提單錯誤。");
        }
        protected void btnDel_Click(object sender, EventArgs e)
        {
            if (ViewState["id"] != null)
            {
                string strOVC_BLD_NO = ViewState["id"].ToString();
                string strUserId = Session["userid"].ToString();
                TBGMT_BLD bld = MTSE.TBGMT_BLD.Where(ot => ot.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                if (bld != null)
                {
                    #region TBGMT_BLD_MRPLOG 刪除LOG
                    TBGMT_BLD_MRPLOG Deletelog = new TBGMT_BLD_MRPLOG();
                    Deletelog.LOG_LOGIN_ID = strUserId;
                    Deletelog.LOG_TIME = DateTime.Now;
                    Deletelog.LOG_EVENT = "DELETE";
                    Deletelog.OVC_BLD_NO = lblOVC_BLD_NO.Text;
                    Deletelog.OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY;
                    Deletelog.OVC_SEA_OR_AIR = bld.OVC_SEA_OR_AIR;
                    Deletelog.OVC_SHIP_NAME = bld.OVC_SHIP_NAME;
                    Deletelog.OVC_VOYAGE = bld.OVC_VOYAGE;
                    Deletelog.ODT_START_DATE = bld.ODT_START_DATE;
                    Deletelog.OVC_START_PORT = bld.OVC_START_PORT;
                    Deletelog.ODT_PLN_ARRIVE_DATE = bld.ODT_PLN_ARRIVE_DATE;
                    Deletelog.ODT_ACT_ARRIVE_DATE = bld.ODT_ACT_ARRIVE_DATE;
                    Deletelog.OVC_ARRIVE_PORT = bld.OVC_ARRIVE_PORT;
                    Deletelog.ONB_QUANITY = bld.ONB_QUANITY;
                    Deletelog.OVC_QUANITY_UNIT = bld.OVC_QUANITY_UNIT;
                    Deletelog.ONB_VOLUME = bld.ONB_VOLUME;
                    Deletelog.ONB_ON_SHIP_VOLUME = bld.ONB_ON_SHIP_VOLUME;
                    Deletelog.OVC_VOLUME_UNIT = bld.OVC_VOLUME_UNIT;
                    Deletelog.ONB_WEIGHT = bld.ONB_WEIGHT;
                    Deletelog.OVC_WEIGHT_UNIT = bld.OVC_WEIGHT_UNIT;
                    Deletelog.ONB_ITEM_VALUE = bld.ONB_ITEM_VALUE;
                    Deletelog.ONB_CARRIAGE_CURRENCY_I = bld.ONB_CARRIAGE_CURRENCY_I;
                    Deletelog.ONB_CARRIAGE = bld.ONB_CARRIAGE;
                    Deletelog.ONB_CARRIAGE_CURRENCY = bld.ONB_CARRIAGE_CURRENCY;
                    Deletelog.OVC_PAYMENT_TYPE = bld.OVC_PAYMENT_TYPE;
                    Deletelog.OVC_MILITARY_TYPE = bld.OVC_MILITARY_TYPE;
                    Deletelog.OVC_STATUS = bld.OVC_STATUS;
                    Deletelog.ODT_CREATE_DATE = bld.ODT_CREATE_DATE;
                    Deletelog.ODT_MODIFY_DATE = bld.ODT_MODIFY_DATE;
                    Deletelog.OVC_CREATE_LOGIN_ID = bld.OVC_CREATE_LOGIN_ID;
                    Deletelog.OVC_MODIFY_LOGIN_ID = bld.OVC_MODIFY_LOGIN_ID;
                    Deletelog.MRPLOG_SN = Guid.NewGuid();
                    MTSE.TBGMT_BLD_MRPLOG.Add(Deletelog);
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), Deletelog.GetType().Name.ToString(), this, "刪除");
                    #endregion

                    MTSE.Entry(bld).State = EntityState.Deleted;
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), bld.GetType().Name.ToString(), this, "刪除");

                    string strMessageSuccess = $"提單編號：{ strOVC_BLD_NO } 刪除成功。";
                    FCommon.AlertShow(PnMessage_Modify, "success", "系統訊息", strMessageSuccess);
                    int sumpage = (int)ViewState["sumpage"]; //取得總共幾筆資料
                    int data = (int)ViewState["data"]; //目前第幾筆資料
                    if (sumpage > 1)
                    {
                        if (data == 0) data++; else data--; //若原本在第0，則下一筆為第1筆，除此之外皆往前1筆。
                        string id = ((string[])ViewState["arrOVC_BLD_NO"])[data];
                        dataImport_List(id); //重新載入清單
                    }
                    else
                        FCommon.MessageBoxShow(this, $"{ strMessageSuccess }\\n已無資料，返回查詢。", $"MTS_A12_1{ getQueryString() }", false);
                }
                else
                    FCommon.AlertShow(PnMessage_Modify, "danger", "系統訊息", $"提單編號：{ strOVC_BLD_NO } 不存在！");
            }
            else
                FCommon.AlertShow(PnMessage_Modify, "danger", "系統訊息", "提單錯誤。");
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_A12_1{ getQueryString() }");
        }
        #endregion

        protected void drpPORT_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonMTS.list_dataImport_PORT(drpOVC_SHIP_COMPANY, drpOVC_SEA_OR_AIR, drpOVC_ARRIVE_PORT); //承運航商，海空運別，啟運/抵達港埠(國內)
        }
        protected void drpOVC_QUANITY_UNIT_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isShow = drpOVC_QUANITY_UNIT.SelectedValue.Equals("其他");
            txtOVC_QUANITY_UNIT.Visible = isShow;
        }
    }
}