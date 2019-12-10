using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A25_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        Common FCommon = new Common();

        #region 副程式
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
                    CommonMTS.list_dataImport_DEPT_CLASS(drpOVC_MILITARY_TYPE, true); //軍種
                    #endregion
                    string strDefaultCurrency = VariableMTS.strDefaultCurrency;
                    FCommon.list_setValue(drpONB_CARRIAGE_CURRENCY_I, strDefaultCurrency);
                    FCommon.list_setValue(drpONB_CARRIAGE_CURRENCY, strDefaultCurrency);
                }
            }
        }
        
        protected void btnNew_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_SHIP_COMPANY = drpOVC_SHIP_COMPANY.SelectedValue;
            string strOVC_SEA_OR_AIR = drpOVC_SEA_OR_AIR.SelectedValue;
            string strOVC_SHIP_NAME = txtOVC_SHIP_NAME.Text;
            string strOVC_VOYAGE = txtOVC_VOYAGE.Text;
            string  strODT_START_DATE = txtODT_START_DATE.Text;
            string strOVC_START_PORT = drpOVC_START_PORT.SelectedValue;
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
            string strONB_WEIGHT = txtONB_WEIGHT.Text;
            string strONB_ITEM_VALUE = txtONB_ITEM_VALUE.Text;
            string strONB_CARRIAGE = txtONB_CARRIAGE.Text;

            decimal decONB_QUANITY, decONB_VOLUME, decONB_WEIGHT, decONB_ITEM_VALUE, decONB_CARRIAGE;
            DateTime dateODT_START_DATE, dateODT_PLN_ARRIVE_DATE, dateODT_ACT_ARRIVE_DATE, dateNow = DateTime.Now;

            #region 錯誤訊息
            if (strOVC_BLD_NO.Equals(string.Empty))
                strMessage += "<p> 請輸入 提單編號 </p>";
            if (strOVC_SHIP_COMPANY.Equals(string.Empty))
                strMessage += "<p> 請選擇 承運航商 </p>";
            else if (strOVC_SHIP_COMPANY.Equals("非合約航商"))
                if (strOVC_SEA_OR_AIR.Equals(string.Empty))
                    strMessage += "<p> 請輸入 海空運別 </p>";
            if (strOVC_SHIP_NAME.Equals(string.Empty))
                strMessage += "<p> 請輸入 船機名稱 </p>";
            if (strOVC_VOYAGE.Equals(string.Empty))
                strMessage += "<p> 請輸入 船機航次 </p>";
            if (strODT_START_DATE.Equals(string.Empty))
                strMessage += "<p> 請輸入 啟運日期 </p>";
            if (strOVC_START_PORT.Equals(string.Empty))
                strMessage += "<p> 請選擇 啟運港埠 </p>";
            if (strODT_PLN_ARRIVE_DATE.Equals(string.Empty))
                strMessage += "<p> 請輸入 預估抵運日期 </p>";
            //if (strODT_ACT_ARRIVE_DATE.Equals(string.Empty))
            //    strMessage += "<p> 請輸入 實際抵運日期 </p>";
            if (strOVC_ARRIVE_PORT.Equals(string.Empty))
                strMessage += "<p> 請選擇 抵達港埠 </p>";
            if (strOVC_QUANITY_UNIT.Equals(string.Empty))
                strMessage += "<p> 請選擇 件數計量單位 </p>";
            if (strOVC_VOLUME_UNIT.Equals(string.Empty))
                strMessage += "<p> 請輸入 體積計量單位 </p>";
            if (strOVC_WEIGHT_UNIT.Equals(string.Empty))
                strMessage += "<p> 請輸入 重量計量單位 </p>";
            if (strONB_CARRIAGE_CURRENCY_I.Equals(string.Empty))
                strMessage += "<p> 請選擇 物資價值幣別 </p>";
            if (strONB_CARRIAGE_CURRENCY.Equals(string.Empty))
                strMessage += "<p> 請輸入 運費幣別 </p>";
            if (strOVC_MILITARY_TYPE.Equals(string.Empty))
                strMessage += "<p> 請輸入 軍種 </p>";
            if (strONB_QUANITY.Equals(string.Empty))
                strMessage += "<p> 請輸入 件數 </p>";
            //if (strONB_VOLUME.Equals(string.Empty))
            //    strMessage += "<p> 請輸入 體積 </p>";
            if (strONB_WEIGHT.Equals(string.Empty))
                strMessage += "<p> 請輸入 重量 </p>";
            if (strONB_ITEM_VALUE.Equals(string.Empty))
                strMessage += "<p> 請輸入 物資價值 </p>";
            if (strONB_CARRIAGE.Equals(string.Empty))
                strMessage += "<p> 請輸入 運費 </p>";

            //確認輸入型態
            bool boolONB_QUANITY = FCommon.checkDecimal(strONB_QUANITY, "件數", ref strMessage, out decONB_QUANITY);
            bool boolONB_VOLUME = FCommon.checkDecimal(strONB_VOLUME, "體積", ref strMessage, out decONB_VOLUME);
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
                    TBGMT_BLD bld = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO == strOVC_BLD_NO).FirstOrDefault();
                    if (bld == null)
                    {
                        #region TBGMT_BLD 新增資料
                        bld = new TBGMT_BLD();
                        bld.OVC_BLD_NO = strOVC_BLD_NO;
                        bld.OVC_SHIP_COMPANY = strOVC_SHIP_COMPANY;
                        bld.OVC_SEA_OR_AIR = strOVC_SEA_OR_AIR;
                        bld.OVC_SHIP_NAME = strOVC_SHIP_NAME;
                        bld.OVC_VOYAGE = strOVC_VOYAGE;
                        bld.ODT_START_DATE = dateODT_START_DATE;
                        bld.OVC_START_PORT = strOVC_START_PORT;
                        bld.ODT_PLN_ARRIVE_DATE = dateODT_PLN_ARRIVE_DATE;
                        if (boolODT_ACT_ARRIVE_DATE) bld.ODT_ACT_ARRIVE_DATE = dateODT_ACT_ARRIVE_DATE; else bld.ODT_ACT_ARRIVE_DATE = null;
                        bld.OVC_ARRIVE_PORT = strOVC_ARRIVE_PORT;
                        bld.ONB_QUANITY = decONB_QUANITY;
                        bld.OVC_QUANITY_UNIT = strOVC_QUANITY_UNIT;
                        if (boolONB_VOLUME) bld.ONB_VOLUME = decONB_VOLUME; else bld.ONB_VOLUME = null;
                        bld.OVC_VOLUME_UNIT = strOVC_VOLUME_UNIT;
                        bld.ONB_WEIGHT = decONB_WEIGHT;
                        bld.OVC_WEIGHT_UNIT = strOVC_WEIGHT_UNIT;
                        bld.ONB_ITEM_VALUE = decONB_ITEM_VALUE;
                        bld.ONB_CARRIAGE_CURRENCY_I = strONB_CARRIAGE_CURRENCY_I;
                        bld.ONB_CARRIAGE = decONB_CARRIAGE;
                        bld.ONB_CARRIAGE_CURRENCY = strONB_CARRIAGE_CURRENCY;
                        bld.OVC_MILITARY_TYPE = strOVC_MILITARY_TYPE;
                        bld.OVC_STATUS = strOVC_STATUS;
                        bld.ODT_CREATE_DATE = dateNow;
                        bld.ODT_MODIFY_DATE = dateNow;
                        bld.OVC_CREATE_LOGIN_ID = strUserId;
                        bld.OVC_MODIFY_LOGIN_ID = strUserId;
                        bld.BLD_SN = Guid.NewGuid();

                        MTSE.TBGMT_BLD.Add(bld);
                        MTSE.SaveChanges();
                        FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), bld.GetType().Name, this, "新增");
                        #endregion

                        #region TBGMT_BLD_MRPLOG 新增LOG
                        TBGMT_BLD_MRPLOG bld_log = new TBGMT_BLD_MRPLOG();
                        bld_log.LOG_LOGIN_ID = strUserId;
                        bld_log.LOG_TIME = dateNow;
                        bld_log.LOG_EVENT = "INSERT";
                        bld_log.OVC_BLD_NO = strOVC_BLD_NO;
                        bld_log.OVC_SHIP_COMPANY = strOVC_SHIP_COMPANY;
                        bld_log.OVC_SEA_OR_AIR = strOVC_SEA_OR_AIR;
                        bld_log.OVC_SHIP_NAME = strOVC_SHIP_NAME;
                        bld_log.OVC_VOYAGE = strOVC_VOYAGE;
                        bld_log.ODT_START_DATE = dateODT_START_DATE;
                        bld_log.OVC_START_PORT = strOVC_START_PORT;
                        bld_log.ODT_PLN_ARRIVE_DATE = dateODT_PLN_ARRIVE_DATE;
                        if (boolODT_ACT_ARRIVE_DATE) bld_log.ODT_ACT_ARRIVE_DATE = dateODT_ACT_ARRIVE_DATE; else bld_log.ODT_ACT_ARRIVE_DATE = null;
                        bld_log.OVC_ARRIVE_PORT = strOVC_ARRIVE_PORT;
                        bld_log.ONB_QUANITY = decONB_QUANITY;
                        bld_log.OVC_QUANITY_UNIT = strOVC_QUANITY_UNIT;
                        if (boolONB_VOLUME) bld_log.ONB_VOLUME = decONB_VOLUME; else bld_log.ONB_VOLUME = null;
                        bld_log.OVC_VOLUME_UNIT = strOVC_VOLUME_UNIT;
                        bld_log.ONB_WEIGHT = decONB_WEIGHT;
                        bld_log.OVC_WEIGHT_UNIT = strOVC_WEIGHT_UNIT;
                        bld_log.ONB_ITEM_VALUE = decONB_ITEM_VALUE;
                        bld_log.ONB_CARRIAGE_CURRENCY_I = strONB_CARRIAGE_CURRENCY_I;
                        bld_log.ONB_CARRIAGE = decONB_CARRIAGE;
                        bld_log.ONB_CARRIAGE_CURRENCY = strONB_CARRIAGE_CURRENCY;
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
                        FCommon.AlertShow(PnWarning, "success", "系統訊息", "新增成功");
                    }
                    else
                        FCommon.AlertShow(PnWarning, "danger", "系統訊息", $"提單編號：{ strOVC_BLD_NO } 已存在");
                }
                catch
                {
                    FCommon.AlertShow(PnWarning, "danger", "系統訊息", "新增失敗，請聯絡工程師。");
                }
            }
            else
                FCommon.AlertShow(PnWarning, "danger", "系統訊息", strMessage);
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