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
    public partial class MTS_A25_3 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();

        string strSysId;
        string id;
        int nowpage = 0;
        int data;

        #region 副程式
        private void TBGMT_BLD_dataImport(string strid)
        {
            FCommon.Controls_Clear(pnDate, "lblnow", "lblall");
            TBGMT_BLD codetable = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO == strid).FirstOrDefault();
            string strDefaultCurrency = VariableMTS.strDefaultCurrency; //預設幣別
            string strONB_CARRIAGE_CURRENCY_I = strDefaultCurrency; //幣別
            string strONB_CARRIAGE_CURRENCY = strDefaultCurrency; //運費幣別
            string strOVC_PAYMENT_TYPE = "預付"; //若無資料，預設為"預付"
            if (codetable != null)
            {
                lblOVC_BLD_NO.Text = codetable.OVC_BLD_NO;
                FCommon.list_setValue(drpOVC_SHIP_COMPANY, codetable.OVC_SHIP_COMPANY);
                FCommon.list_setValue(drpOVC_SEA_OR_AIR, codetable.OVC_SEA_OR_AIR);
                txtOVC_SHIP_NAME.Text = codetable.OVC_SHIP_NAME ?? "";
                txtOVC_VOYAGE.Text = codetable.OVC_VOYAGE ?? "";
                txtODT_START_DATE.Text = FCommon.getDateTime(codetable.ODT_START_DATE);
                CommonMTS.list_dataImport_PORT(drpOVC_SHIP_COMPANY, drpOVC_SEA_OR_AIR, drpOVC_START_PORT); //承運航商，海空運別，啟運/抵達港埠(國內)
                FCommon.list_setValue(drpOVC_START_PORT, codetable.OVC_START_PORT);
                txtODT_PLN_ARRIVE_DATE.Text = FCommon.getDateTime(codetable.ODT_PLN_ARRIVE_DATE);
                txtODT_ACT_ARRIVE_DATE.Text = FCommon.getDateTime(codetable.ODT_ACT_ARRIVE_DATE);
                string strOVC_ARRIVE_PORT = codetable.OVC_ARRIVE_PORT ?? "";
                txtOVC_PORT_CDE.Text = strOVC_ARRIVE_PORT;
                TBGMT_PORTS port = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(strOVC_ARRIVE_PORT)).FirstOrDefault();
                if (port != null) txtOVC_CHI_NAME.Text = port.OVC_PORT_CHI_NAME;
                txtONB_QUANITY.Text = codetable.ONB_QUANITY.ToString();
                //FCommon.list_setValue(drpOVC_QUANITY_UNIT,codetable.OVC_QUANITY_UNIT);
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
                lblOVC_CREATE_LOGIN_ID.Text = FCommon.getUserName(codetable.OVC_CREATE_LOGIN_ID);
            }
            FCommon.list_setValue(drpONB_CARRIAGE_CURRENCY_I, strONB_CARRIAGE_CURRENCY_I);
            FCommon.list_setValue(drpONB_CARRIAGE_CURRENCY, strONB_CARRIAGE_CURRENCY);
            FCommon.list_setValue(rdoOVC_PAYMENT_TYPE, strOVC_PAYMENT_TYPE);
        }

        private void button()
        {
            if (ViewState["nowpage"] != null)
            {
                int nowpage = (int)ViewState["nowpage"];
                if (nowpage == 1)
                    FCommon.Controls_Attributes("disabled", "true", btnPrev);
                else
                    FCommon.Controls_Attributes("disabled", btnPrev);

                if (nowpage == (int)ViewState["sumpage"])
                    FCommon.Controls_Attributes("disabled", "true", btnNext);
                else
                    FCommon.Controls_Attributes("disabled", btnNext);
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);

                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtODT_START_DATE, txtODT_PLN_ARRIVE_DATE, txtODT_ACT_ARRIVE_DATE, txtOVC_CHI_NAME);
                    #region 匯入下拉式選單
                    CommonMTS.list_dataImport_SHIP_COMPANY(drpOVC_SHIP_COMPANY, true); //承運航商
                    CommonMTS.list_dataImport_SEA_OR_AIR(drpOVC_SEA_OR_AIR, false); //海空運別
                    CommonMTS.list_dataImport_PORT(drpOVC_SHIP_COMPANY, drpOVC_SEA_OR_AIR, drpOVC_START_PORT); //承運航商，海空運別，啟運/抵達港埠(國內)
                    CommonMTS.list_dataImport_QUANITY_UNIT(drpOVC_QUANITY_UNIT, true); //件數單位
                    CommonMTS.list_dataImport_VOLUME_UNIT(drpOVC_VOLUME_UNIT, true); //體積單位
                    CommonMTS.list_dataImport_WEIGHT_UNIT(drpOVC_WEIGHT_UNIT, true); //重量單位
                    CommonMTS.list_dataImport_CURRENCY(drpONB_CARRIAGE_CURRENCY_I, false); //物資價值幣別
                    CommonMTS.list_dataImport_CURRENCY(drpONB_CARRIAGE_CURRENCY, false); //運費幣別
                    CommonMTS.list_dataImport_PAYMENT_TYPE_BLD(rdoOVC_PAYMENT_TYPE); //付款方式
                    CommonMTS.list_dataImport_DEPT_CLASS(drpOVC_MILITARY_TYPE, true); //軍種
                    #endregion

                    if (FCommon.getQueryString(this, "id", out id, true))
                        ViewState["id"] = id;
                    else
                        FCommon.MessageBoxShow(this, "提單編號錯誤！", "MTS_A25_1", false);
                    FCommon.getQueryString(this, "OVC_BLD_NO", out string strOVC_BLD_NO, true);
                    FCommon.getQueryString(this, "OVC_SHIP_NAME", out string strOVC_SHIP_NAME, true);
                    FCommon.getQueryString(this, "OVC_VOYAGE", out string strOVC_VOYAGE, true);
                    FCommon.getQueryString(this, "OVC_TRANSER_DEPT_CDE", out string strOVC_TRANSER_DEPT_CDE, true);
                    FCommon.getQueryString(this, "OVC_IS_SECURITY", out string strOVC_IS_SECURITY, true);

                    //var tbmquery = from tbm in GME.TBM1407.AsEnumerable() where tbm.OVC_PHR_CATE == "TR" select tbm;
                    var query =
                        from bld in MTSE.TBGMT_BLD.AsEnumerable()
                        where bld.OVC_STATUS == null || bld.OVC_STATUS.Equals("A")
                        join arrport in MTSE.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals arrport.OVC_PORT_CDE
                        join strport in MTSE.TBGMT_PORTS on bld.OVC_START_PORT equals strport.OVC_PORT_CDE
                        where strport.OVC_IS_ABROAD == "國內"
                        orderby bld.OVC_BLD_NO
                        select new
                        {
                            OVC_BLD_NO = bld.OVC_BLD_NO,
                            OVC_SHIP_NAME = bld.OVC_SHIP_NAME,
                            OVC_VOYAGE = bld.OVC_VOYAGE,
                            OVC_START_PORT = bld.OVC_START_PORT,
                            OVC_IS_SECURITY = bld.OVC_IS_SECURITY ?? 0
                        };
                    if (!strOVC_BLD_NO.Equals(string.Empty))
                        query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
                    if (!strOVC_SHIP_NAME.Equals(string.Empty))
                        query = query.Where(table => table.OVC_SHIP_NAME.Equals(strOVC_SHIP_NAME));
                    if (!strOVC_VOYAGE.Equals(string.Empty))
                        query = query.Where(table => table.OVC_VOYAGE != null && table.OVC_VOYAGE.Equals(strOVC_VOYAGE));
                    if (!strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                    {
                        var table = GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("TR"))
                        .Where(t => t.OVC_PHR_PARENTS.Equals(strOVC_TRANSER_DEPT_CDE))
                        .Select(t => t.OVC_PHR_ID).ToArray();

                        query = query.Where(t => table.Contains(t.OVC_START_PORT));
                    }
                    if (!strOVC_IS_SECURITY.Equals(string.Empty))
                    {
                        int issecurity;
                        if (int.TryParse(strOVC_IS_SECURITY, out issecurity))
                            query = query.Where(table => table.OVC_IS_SECURITY == issecurity);
                    }
                    #region 舊查詢
                    //var query =
                    //from bld in MTSE.TBGMT_BLD.AsEnumerable()
                    //orderby bld.OVC_BLD_NO
                    //select new
                    //{
                    //    Value = bld.OVC_BLD_NO,
                    //    bld.OVC_SHIP_NAME,
                    //    bld.OVC_VOYAGE,
                    //    bld.OVC_ARRIVE_PORT,
                    //    bld.OVC_IS_SECURITY
                    //};
                    //if (!strOVC_BLD_NO.Equals(string.Empty))
                    //    query = query.Where(table => table.Value == strOVC_BLD_NO);
                    //if (!strOVC_SHIP_NAME.Equals(string.Empty))
                    //    query = query.Where(table => table.OVC_SHIP_NAME == strOVC_SHIP_NAME);
                    //if (!strOVC_VOYAGE.Equals(string.Empty))
                    //    query = query.Where(table => table.OVC_VOYAGE == strOVC_VOYAGE);
                    //if (!strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                    //{
                    //    string[] arrtbmquery = tbmquery.Where(table => table.OVC_PHR_PARENTS == strOVC_TRANSER_DEPT_CDE).Select(table => table.OVC_PHR_ID).ToArray();
                    //    foreach (string port in arrtbmquery)
                    //    {
                    //        query = query.Where(table => table.OVC_ARRIVE_PORT == port);
                    //    }
                    //}
                    //if (!strOVC_IS_SECURITY.Equals(string.Empty))
                    //    query = query.Where(table => table.OVC_IS_SECURITY == Convert.ToDecimal(strOVC_IS_SECURITY));
                    #endregion

                    DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                    int intCount = dt.Rows.Count;
                    if (intCount > 0)
                    {
                        string[] arrOVC_BLD_NO = new string[intCount];
                        for (int i = 0; i < intCount; i++)
                        {
                            strSysId = dt.Rows[i]["OVC_BLD_NO"].ToString();
                            arrOVC_BLD_NO[i] = strSysId;
                        }

                        lblall.Text = intCount.ToString();
                        for (int i = 0; i < intCount; i++)
                        {
                            if (arrOVC_BLD_NO[i] == id)
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
                            TBGMT_BLD_dataImport(ViewState["id"].ToString());
                        }
                        else
                            FCommon.MessageBoxShow(this, "資料錯誤！", "MTS_A25_1", false);
                    }
                    else
                        FCommon.MessageBoxShow(this, "查詢錯誤！", "MTS_A25_1", false);
                }
                button();
            }
        }

        #region ~Click
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            data = (int)ViewState["data"] - 1;
            id = ((string[])this.ViewState["arrOVC_BLD_NO"])[data];
            ViewState["nowpage"] = (int)ViewState["nowpage"] - 1;
            ViewState["data"] = (int)ViewState["data"] - 1;
            TBGMT_BLD_dataImport(id);
            lblnow.Text = ViewState["nowpage"].ToString();
            ViewState["id"] = id;
            button();
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            data = (int)ViewState["data"] + 1;
            id = ((string[])this.ViewState["arrOVC_BLD_NO"])[data];
            ViewState["nowpage"] = (int)ViewState["nowpage"] + 1;
            ViewState["data"] = (int)ViewState["data"] + 1;
            TBGMT_BLD_dataImport(id);
            lblnow.Text = ViewState["nowpage"].ToString();
            ViewState["id"] = id;
            button();
        }
        protected void btnModify_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string nowid = ViewState["id"].ToString();
            string strUserId = Session["userid"].ToString();
            string strOVC_SHIP_COMPANY = drpOVC_SHIP_COMPANY.SelectedValue;
            string strOVC_SEA_OR_AIR = drpOVC_SEA_OR_AIR.SelectedValue;
            string strOVC_SHIP_NAME = txtOVC_SHIP_NAME.Text;
            string strOVC_VOYAGE = txtOVC_VOYAGE.Text;
            string strODT_START_DATE = txtODT_START_DATE.Text;
            string txtOVC_START_PORT = drpOVC_START_PORT.SelectedValue;
            string strODT_PLN_ARRIVE_DATE = txtODT_PLN_ARRIVE_DATE.Text;
            string strODT_ACT_ARRIVE_DATE = txtODT_ACT_ARRIVE_DATE.Text;
            string strOVC_ARRIVE_PORT = txtOVC_PORT_CDE.Text;
            string strOVC_QUANITY_UNIT = drpOVC_QUANITY_UNIT.SelectedValue;
            if (strOVC_QUANITY_UNIT.Equals("其他")) strOVC_QUANITY_UNIT = txtOVC_QUANITY_UNIT.Text; //若選擇其他，則取得輸入之資料
            string strOVC_VOLUME_UNIT = drpOVC_VOLUME_UNIT.SelectedValue;
            string strOVC_WEIGHT_UNIT = drpOVC_WEIGHT_UNIT.SelectedValue;
            string strONB_CARRIAGE_CURRENCY_I = drpONB_CARRIAGE_CURRENCY_I.SelectedValue;
            string strONB_CARRIAGE_CURRENCY = drpONB_CARRIAGE_CURRENCY.SelectedValue;
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedValue;
            string strOVC_STATUS = "B"; //直接新增為正式區
            string strONB_QUANITY = txtONB_QUANITY.Text;
            string strONB_VOLUME = txtONB_VOLUME.Text;
            string strONB_ON_SHIP_VOLUME = txtONB_ON_SHIP_VOLUME.Text;
            string strONB_WEIGHT = txtONB_WEIGHT.Text;
            string strONB_ITEM_VALUE = txtONB_ITEM_VALUE.Text;
            string strONB_CARRIAGE = txtONB_CARRIAGE.Text;

            decimal decONB_QUANITY, decONB_VOLUME, decONB_ON_SHIP_VOLUME, decONB_WEIGHT, decONB_ITEM_VALUE, decONB_CARRIAGE;
            DateTime dateODT_START_DATE, dateODT_PLN_ARRIVE_DATE, dateODT_ACT_ARRIVE_DATE, dateNow = DateTime.Now;

            #region 錯誤訊息
            if (strOVC_SHIP_COMPANY.Equals(string.Empty))
                strMessage += "<p> 請選擇 承運航商 </p>";
            else if (strOVC_SHIP_COMPANY.Equals("非合約航商"))
                if (strOVC_SEA_OR_AIR.Equals(string.Empty))
                    strMessage += "<p> 請輸入 海空運別 </p>";
            if (strOVC_SHIP_NAME.Equals(string.Empty))
                strMessage += "<P> 請輸入 船機名稱 </p>";
            if (strOVC_VOYAGE.Equals(string.Empty))
                strMessage += "<P> 請輸入 船機航次 </p>";
            if (strODT_START_DATE.Equals(string.Empty))
                strMessage += "<P> 請輸入 啟運日期 </p>";
            if (txtOVC_START_PORT.Equals(string.Empty))
                strMessage += "<P> 請選擇 啟運港埠 </p>";
            if (strODT_PLN_ARRIVE_DATE.Equals(string.Empty))
                strMessage += "<P> 請輸入 預估抵運日期 </p>";
            //if (strODT_ACT_ARRIVE_DATE.Equals(string.Empty))
            //    strMessage += "<P> 請輸入 實際抵運日期 </p>";
            if (strOVC_ARRIVE_PORT.Equals(string.Empty))
                strMessage += "<P> 請選擇 抵達港埠 </p>";
            if (strOVC_QUANITY_UNIT.Equals(string.Empty))
                strMessage += "<P> 請選擇 件數計量單位 </p>";
            //if (strOVC_VOLUME_UNIT.Equals(string.Empty))
            //    strMessage += "<P> 請輸入 體積計量單位 </p>";
            if (strOVC_WEIGHT_UNIT.Equals(string.Empty))
                strMessage += "<P> 請輸入 重量計量單位 </p>";
            if (strONB_CARRIAGE_CURRENCY_I.Equals(string.Empty))
                strMessage += "<P> 請選擇 物資價值幣別 </p>";
            if (strONB_CARRIAGE_CURRENCY.Equals(string.Empty))
                strMessage += "<P> 請輸入 運費幣別 </p>";
            if (strOVC_MILITARY_TYPE.Equals(string.Empty))
                strMessage += "<P> 請輸入 軍種 </p>";

            if (strONB_QUANITY.Equals(string.Empty))
                strMessage += "<p> 請輸入 件數 </p>";
            //if (strONB_VOLUME.Equals(string.Empty))
            //    strMessage += "<p> 請輸入 體積 </p>";
            //if (strONB_ON_SHIP_VOLUME.Equals(string.Empty))
            //    strMessage += "<p> 請輸入 佔艙體積 </p>";
            if (strONB_WEIGHT.Equals(string.Empty))
                strMessage += "<p> 請輸入 重量 </p>";
            if (strONB_ITEM_VALUE.Equals(string.Empty))
                strMessage += "<p> 請輸入 物資價值 </p>";
            if (strONB_CARRIAGE.Equals(string.Empty))
                strMessage += "<p> 請輸入 運費 </p>";

            //確認輸入型態
            bool boolONB_QUANITY = FCommon.checkDecimal(strONB_QUANITY, "件數", ref strMessage, out decONB_QUANITY);
            bool boolONB_VOLUME = FCommon.checkDecimal(strONB_VOLUME, "體積", ref strMessage, out decONB_VOLUME);
            bool boolONB_ON_SHIP_VOLUME = FCommon.checkDecimal(strONB_ON_SHIP_VOLUME, "體積", ref strMessage, out decONB_ON_SHIP_VOLUME);
            bool boolONB_WEIGHT = FCommon.checkDecimal(strONB_WEIGHT, "重量", ref strMessage, out decONB_WEIGHT);
            bool boolONB_ITEM_VALUE = FCommon.checkDecimal(strONB_ITEM_VALUE, "物資價值", ref strMessage, out decONB_ITEM_VALUE);
            bool boolONB_CARRIAGE = FCommon.checkDecimal(strONB_CARRIAGE, "運費", ref strMessage, out decONB_CARRIAGE);
            bool boolODT_START_DATE = FCommon.checkDateTime(strODT_START_DATE, "啟運日期", ref strMessage, out dateODT_START_DATE);
            bool boolODT_PLN_ARRIVE_DATE = FCommon.checkDateTime(strODT_PLN_ARRIVE_DATE, "預估抵運日期", ref strMessage, out dateODT_PLN_ARRIVE_DATE);
            //bool boolODT_ACT_ARRIVE_DATE = FCommon.checkDateTime(strODT_ACT_ARRIVE_DATE, "實際抵運日期", ref strMessage, out dateODT_ACT_ARRIVE_DATE);
            bool boolODT_ACT_ARRIVE_DATE = DateTime.TryParse(strODT_ACT_ARRIVE_DATE, out dateODT_ACT_ARRIVE_DATE);
            //確認日期前後
            if (boolODT_START_DATE && boolODT_PLN_ARRIVE_DATE && DateTime.Compare(dateODT_START_DATE, dateODT_PLN_ARRIVE_DATE) > 0)
                strMessage += "<p> 預估抵運日期 不正確</p>";
            //if (boolODT_START_DATE && boolODT_ACT_ARRIVE_DATE && DateTime.Compare(dateODT_START_DATE, dateODT_ACT_ARRIVE_DATE) > 0)
            //    strMessage += "<p> 實際抵運日期 不正確</p>";
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                try
                {
                    TBGMT_BLD bld = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO == nowid).FirstOrDefault();
                    #region TBGMT_BLD 修改資料
                    if (bld != null)
                    {
                        bld.OVC_BLD_NO = nowid;
                        bld.OVC_SHIP_COMPANY = strOVC_SHIP_COMPANY;
                        bld.OVC_SEA_OR_AIR = strOVC_SEA_OR_AIR;
                        bld.OVC_SHIP_NAME = strOVC_SHIP_NAME;
                        bld.OVC_VOYAGE = strOVC_VOYAGE;
                        bld.ODT_START_DATE = dateODT_START_DATE;
                        bld.OVC_START_PORT = txtOVC_START_PORT;
                        bld.ODT_PLN_ARRIVE_DATE = dateODT_PLN_ARRIVE_DATE;
                        if (boolODT_ACT_ARRIVE_DATE) bld.ODT_ACT_ARRIVE_DATE = dateODT_ACT_ARRIVE_DATE; else bld.ODT_ACT_ARRIVE_DATE = null;
                        bld.OVC_ARRIVE_PORT = strOVC_ARRIVE_PORT;
                        bld.ONB_QUANITY = decONB_QUANITY;
                        bld.OVC_QUANITY_UNIT = strOVC_QUANITY_UNIT;
                        if (boolONB_VOLUME) bld.ONB_VOLUME = decONB_VOLUME; else bld.ONB_VOLUME = null;
                        if (boolONB_ON_SHIP_VOLUME) bld.ONB_ON_SHIP_VOLUME = decONB_ON_SHIP_VOLUME; else bld.ONB_ON_SHIP_VOLUME = null;
                        if (strOVC_VOLUME_UNIT.Equals(string.Empty)) bld.OVC_VOLUME_UNIT = null; else bld.OVC_VOLUME_UNIT = strOVC_VOLUME_UNIT;
                        bld.ONB_WEIGHT = decONB_WEIGHT;
                        bld.OVC_WEIGHT_UNIT = strOVC_WEIGHT_UNIT;
                        bld.ONB_ITEM_VALUE = decONB_ITEM_VALUE;
                        bld.ONB_CARRIAGE_CURRENCY_I = strONB_CARRIAGE_CURRENCY_I;
                        bld.ONB_CARRIAGE = decONB_CARRIAGE;
                        bld.ONB_CARRIAGE_CURRENCY = strONB_CARRIAGE_CURRENCY;
                        bld.OVC_PAYMENT_TYPE = rdoOVC_PAYMENT_TYPE.SelectedValue;
                        bld.OVC_MILITARY_TYPE = strOVC_MILITARY_TYPE;
                        bld.OVC_STATUS = strOVC_STATUS;
                        bld.ODT_MODIFY_DATE = dateNow;
                        bld.OVC_MODIFY_LOGIN_ID = strUserId;

                        MTSE.SaveChanges();
                        FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), bld.GetType().Name, this, "修改");
                    }
                    #endregion

                    #region TBGMT_BLD_MRPLOG 修改LOG
                    TBGMT_BLD_MRPLOG bld_log = new TBGMT_BLD_MRPLOG();
                    bld_log.LOG_LOGIN_ID = strUserId;
                    bld_log.LOG_TIME = dateNow;
                    bld_log.LOG_EVENT = "UPDATE";
                    bld_log.OVC_BLD_NO = nowid;
                    bld_log.OVC_SHIP_COMPANY = strOVC_SHIP_COMPANY;
                    bld_log.OVC_SEA_OR_AIR = strOVC_SEA_OR_AIR;
                    bld_log.OVC_SHIP_NAME = strOVC_SHIP_NAME;
                    bld_log.OVC_VOYAGE = strOVC_VOYAGE;
                    bld_log.ODT_START_DATE = dateODT_START_DATE;
                    bld_log.OVC_START_PORT = txtOVC_START_PORT;
                    bld_log.ODT_PLN_ARRIVE_DATE = dateODT_PLN_ARRIVE_DATE;
                    if (boolODT_ACT_ARRIVE_DATE) bld_log.ODT_ACT_ARRIVE_DATE = dateODT_ACT_ARRIVE_DATE; else bld_log.ODT_ACT_ARRIVE_DATE = null;
                    bld_log.OVC_ARRIVE_PORT = strOVC_ARRIVE_PORT;
                    bld_log.ONB_QUANITY = decONB_QUANITY;
                    bld_log.OVC_QUANITY_UNIT = strOVC_QUANITY_UNIT;
                    if (boolONB_VOLUME) bld_log.ONB_VOLUME = decONB_VOLUME; else bld_log.ONB_VOLUME = null;
                    if (boolONB_ON_SHIP_VOLUME) bld_log.ONB_ON_SHIP_VOLUME = decONB_ON_SHIP_VOLUME; else bld_log.ONB_ON_SHIP_VOLUME = null;
                    if (strOVC_VOLUME_UNIT.Equals(string.Empty)) bld_log.OVC_VOLUME_UNIT = null; else bld_log.OVC_VOLUME_UNIT = strOVC_VOLUME_UNIT;
                    bld_log.ONB_WEIGHT = decONB_WEIGHT;
                    bld_log.OVC_WEIGHT_UNIT = strOVC_WEIGHT_UNIT;
                    bld_log.ONB_ITEM_VALUE = decONB_ITEM_VALUE;
                    bld_log.ONB_CARRIAGE_CURRENCY_I = strONB_CARRIAGE_CURRENCY_I;
                    bld_log.ONB_CARRIAGE = decONB_CARRIAGE;
                    bld_log.ONB_CARRIAGE_CURRENCY = strONB_CARRIAGE_CURRENCY;
                    bld_log.OVC_PAYMENT_TYPE = rdoOVC_PAYMENT_TYPE.SelectedValue;
                    bld_log.OVC_MILITARY_TYPE = strOVC_MILITARY_TYPE;
                    bld_log.OVC_STATUS = strOVC_STATUS;
                    bld_log.ODT_CREATE_DATE = bld.ODT_CREATE_DATE;
                    bld_log.ODT_MODIFY_DATE = bld.ODT_MODIFY_DATE;
                    bld_log.OVC_CREATE_LOGIN_ID = bld.OVC_CREATE_LOGIN_ID;
                    bld_log.MRPLOG_SN = Guid.NewGuid();

                    MTSE.TBGMT_BLD_MRPLOG.Add(bld_log);
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), bld_log.GetType().Name, this, "新增");
                    #endregion

                    FCommon.AlertShow(PnMessage_Modify, "success", "系統訊息", $"提單編號：{ nowid } 修改成功，並加入正式區！");
                }
                catch
                {
                    FCommon.AlertShow(PnMessage_Modify, "danger", "系統訊息", "修改失敗，請聯絡工程師。");
                }
            }
            else
                FCommon.AlertShow(PnMessage_Modify, "danger", "系統訊息", strMessage);
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", Request.QueryString["OVC_BLD_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_SHIP_NAME", Request.QueryString["OVC_SHIP_NAME"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_VOYAGE", Request.QueryString["OVC_VOYAGE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", Request.QueryString["OVC_TRANSER_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_IS_SECURITY", Request.QueryString["OVC_IS_SECURITY"], false);
            Response.Redirect($"MTS_A25_1{ strQueryString }");
        }
        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                string nowid = ViewState["id"].ToString();
                string strUserId = Session["userid"].ToString();
                TBGMT_BLD bld = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO == nowid).FirstOrDefault();
                if (bld != null)
                {
                    #region TBGMT_BLD_MRPLOG 刪除LOG
                    TBGMT_BLD_MRPLOG bld_log = new TBGMT_BLD_MRPLOG();
                    bld_log.LOG_LOGIN_ID = strUserId;
                    bld_log.LOG_TIME = DateTime.Now;
                    bld_log.LOG_EVENT = "DELETE";
                    bld_log.OVC_BLD_NO = nowid;
                    bld_log.OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY;
                    bld_log.OVC_SEA_OR_AIR = bld.OVC_SEA_OR_AIR;
                    bld_log.OVC_SHIP_NAME = bld.OVC_SHIP_NAME;
                    bld_log.OVC_VOYAGE = bld.OVC_VOYAGE;
                    bld_log.ODT_START_DATE = bld.ODT_START_DATE;
                    bld_log.OVC_START_PORT = bld.OVC_START_PORT;
                    bld_log.ODT_PLN_ARRIVE_DATE = bld.ODT_PLN_ARRIVE_DATE;
                    bld_log.ODT_ACT_ARRIVE_DATE = bld.ODT_ACT_ARRIVE_DATE;
                    bld_log.OVC_ARRIVE_PORT = bld.OVC_ARRIVE_PORT;
                    bld_log.ONB_QUANITY = bld.ONB_QUANITY;
                    bld_log.OVC_QUANITY_UNIT = bld.OVC_QUANITY_UNIT;
                    bld_log.ONB_VOLUME = bld.ONB_VOLUME;
                    bld_log.ONB_ON_SHIP_VOLUME = bld.ONB_ON_SHIP_VOLUME;
                    bld_log.OVC_VOLUME_UNIT = bld.OVC_VOLUME_UNIT;
                    bld_log.ONB_WEIGHT = bld.ONB_WEIGHT;
                    bld_log.OVC_WEIGHT_UNIT = bld.OVC_WEIGHT_UNIT;
                    bld_log.ONB_ITEM_VALUE = bld.ONB_ITEM_VALUE;
                    bld_log.ONB_CARRIAGE_CURRENCY_I = bld.ONB_CARRIAGE_CURRENCY_I;
                    bld_log.ONB_CARRIAGE = bld.ONB_CARRIAGE;
                    bld_log.ONB_CARRIAGE_CURRENCY = bld.ONB_CARRIAGE_CURRENCY;
                    bld_log.OVC_PAYMENT_TYPE = bld.OVC_PAYMENT_TYPE;
                    bld_log.OVC_MILITARY_TYPE = bld.OVC_MILITARY_TYPE;
                    bld_log.OVC_STATUS = bld.OVC_STATUS;
                    bld_log.ODT_CREATE_DATE = bld.ODT_CREATE_DATE;
                    bld_log.ODT_MODIFY_DATE = bld.ODT_MODIFY_DATE;
                    bld_log.OVC_CREATE_LOGIN_ID = bld.OVC_CREATE_LOGIN_ID;
                    bld_log.MRPLOG_SN = Guid.NewGuid();

                    MTSE.TBGMT_BLD_MRPLOG.Add(bld_log);
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), bld_log.GetType().Name, this, "新增");
                    #endregion

                    //先新增log再刪除
                    MTSE.Entry(bld).State = EntityState.Deleted;
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), bld.GetType().Name, this, "刪除");

                    FCommon.AlertShow(PnMessage_Modify, "success", "系統訊息", $"提單編號：{ nowid } 刪除成功！");
                }
                else
                    FCommon.AlertShow(PnMessage_Modify, "danger", "系統訊息", $"提單編號：{ nowid } 已被刪除，不存在");
            }
            catch
            {
                FCommon.AlertShow(PnMessage_Modify, "danger", "系統訊息", "刪除失敗，請聯絡工程師。");
            }
        }
        #endregion

        protected void drpPORT_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonMTS.list_dataImport_PORT(drpOVC_SHIP_COMPANY, drpOVC_SEA_OR_AIR, drpOVC_START_PORT); //承運航商，海空運別，啟運/抵達港埠(國內)
        }
        protected void drpOVC_QUANITY_UNIT_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isShow = drpOVC_QUANITY_UNIT.SelectedValue.Equals("其他");
            txtOVC_QUANITY_UNIT.Visible = isShow;
        }
    }
}