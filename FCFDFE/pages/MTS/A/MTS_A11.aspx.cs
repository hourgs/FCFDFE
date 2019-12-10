using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A11 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();

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
                    CommonMTS.list_dataImport_PORT(drpOVC_SHIP_COMPANY, drpOVC_SEA_OR_AIR, drpOVC_ARRIVE_PORT); //承運航商，海空運別，啟運/抵達港埠(國內)
                    CommonMTS.list_dataImport_QUANITY_UNIT(drpOVC_QUANITY_UNIT, true); //件數單位
                    CommonMTS.list_dataImport_VOLUME_UNIT(drpOVC_VOLUME_UNIT, true); //體積單位
                    CommonMTS.list_dataImport_WEIGHT_UNIT(drpOVC_WEIGHT_UNIT, true); //重量單位
                    CommonMTS.list_dataImport_CURRENCY(drpONB_CARRIAGE_CURRENCY_I, false); //物資價值幣別
                    CommonMTS.list_dataImport_CURRENCY(drpONB_CARRIAGE_CURRENCY, false); //運費幣別
                    CommonMTS.list_dataImport_DEPT_CLASS(drpOVC_MILITARY_TYPE, true); //軍種
                    CommonMTS.list_dataImport_IS_SECURITY(drpOVC_IS_SECURITY, false);//機敏軍品
                    #endregion
                }
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            //string strOVC_BLD_NO = txtOVC_BLD_NO.Text.ToUpper();
            //txtOVC_BLD_NO.Text = strOVC_BLD_NO;

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
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedValue;
            string strOVC_IS_SECURITY = drpOVC_IS_SECURITY.SelectedValue;

            string strONB_QUANITY = txtONB_QUANITY.Text;
            string strONB_VOLUME = txtONB_VOLUME.Text;
            string strONB_ON_SHIP_VOLUME = txtONB_ON_SHIP_VOLUME.Text;
            string strONB_WEIGHT = txtONB_WEIGHT.Text;
            string strONB_ITEM_VALUE = txtONB_ITEM_VALUE.Text;
            string strONB_CARRIAGE = txtONB_CARRIAGE.Text;
            string strONB_DANGER_PRO = drpONB_DANGER_PRO.SelectedItem.Text;

            DateTime dateNow = DateTime.Now;
            
            #region 錯誤訊息
            if (strOVC_BLD_NO.Equals(string.Empty))
                strMessage += "<P> 請輸入 提單編號 </p>";
            else
            {
                //var query =
                //   from bld in MTSE.TBGMT_BLD.AsEnumerable()                  
                //   where bld.OVC_BLD_NO.Equals(strOVC_BLD_NO)                   
                //   select new
                //   {
                //       bld.OVC_BLD_NO
                //   };

                //新增判斷提單有無重複
                TBGMT_BLD bld = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();                

                if (bld != null )
                    strMessage += "<P> 提單編號重複 請更改提單編號 </p>";
            }
            bool boolOVC_IS_SECURITY = FCommon.checkInt(strOVC_IS_SECURITY, "機敏軍品", ref strMessage, out int intOVC_IS_SECURITY);
            if (strOVC_SHIP_COMPANY.Equals(string.Empty))
                strMessage += "<P> 請選擇 承運航商 </p>";
            if (strOVC_SHIP_COMPANY.Equals("非合約航商"))
            {
                if (strOVC_SEA_OR_AIR.Equals(string.Empty))
                    strMessage += "<P> 請輸入 海空運別 </p>";
            }
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
            if (strONB_QUANITY.Equals(string.Empty))
                strMessage += "<P> 請輸入 件數 </p>";
            if (strONB_VOLUME.Equals(string.Empty))
                strMessage += "<P> 請輸入 體積 </p>";
            //if (strONB_ON_SHIP_VOLUME.Equals(string.Empty))
            //    strMessage += "<P> 請輸入 佔艙體積 </p>";
            if (strONB_WEIGHT.Equals(string.Empty))
                strMessage += "<P> 請輸入 重量 </p>";
            if (strONB_ITEM_VALUE.Equals(string.Empty))
                strMessage += "<P> 請輸入 物資價值 </p>";
            if (strONB_CARRIAGE.Equals(string.Empty))
                strMessage += "<P> 請輸入 運費 </p>";

            bool boolODT_START_DATE = FCommon.checkDateTime(strODT_START_DATE, "啟運日期", ref strMessage, out DateTime dateODT_START_DATE);
            bool boolODT_PLN_ARRIVE_DATE = FCommon.checkDateTime(strODT_PLN_ARRIVE_DATE, "預估抵運日期", ref strMessage, out DateTime dateODT_PLN_ARRIVE_DATE);
            bool boolODT_ACT_ARRIVE_DATE = FCommon.checkDateTime(strODT_ACT_ARRIVE_DATE, "實際抵運日期", ref strMessage, out DateTime dateODT_ACT_ARRIVE_DATE);
            bool boolONB_QUANITY = FCommon.checkDecimal(strONB_QUANITY, "件數", ref strMessage, out decimal decONB_QUANITY);
            bool boolONB_VOLUME = FCommon.checkDecimal(strONB_VOLUME, "體積", ref strMessage, out decimal decONB_VOLUME);
            bool boolONB_ON_SHIP_VOLUME = FCommon.checkDecimal(strONB_ON_SHIP_VOLUME, "佔艙體積", ref strMessage, out decimal decONB_ON_SHIP_VOLUME);
            bool boolONB_WEIGHT = FCommon.checkDecimal(strONB_WEIGHT, "重量", ref strMessage, out decimal decONB_WEIGHT);
            bool boolONB_ITEM_VALUE = FCommon.checkDecimal(strONB_ITEM_VALUE, "物資價值", ref strMessage, out decimal decONB_ITEM_VALUE);
            bool boolONB_CARRIAGE = FCommon.checkDecimal(strONB_CARRIAGE, "運費", ref strMessage, out decimal decONB_CARRIAGE);
            if (!strODT_START_DATE.Equals(string.Empty) && !strODT_PLN_ARRIVE_DATE.Equals(string.Empty) & boolODT_START_DATE & boolODT_PLN_ARRIVE_DATE)
                if (DateTime.Compare(dateODT_START_DATE, dateODT_PLN_ARRIVE_DATE) > 0)
                    strMessage += "<P> 預估抵運日期 不正確</p>";
            if (!strODT_START_DATE.Equals(string.Empty) && !strODT_ACT_ARRIVE_DATE.Equals(string.Empty) & boolODT_START_DATE & boolODT_ACT_ARRIVE_DATE)
                if (DateTime.Compare(dateODT_START_DATE, dateODT_ACT_ARRIVE_DATE) > 0)
                    strMessage += "<P> 實際抵運日期 不正確</p>";
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                try
                {
                    #region TBGMT_BLD 新增資料
                    TBGMT_BLD codetable = new TBGMT_BLD();
                    codetable.OVC_BLD_NO = strOVC_BLD_NO;
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
                    codetable.OVC_MILITARY_TYPE = strOVC_MILITARY_TYPE;
                    codetable.OVC_IS_SECURITY = intOVC_IS_SECURITY;
                    codetable.OVC_STATUS = "A";
                    codetable.ODT_CREATE_DATE = dateNow;
                    codetable.ODT_MODIFY_DATE = dateNow;
                    codetable.OVC_CREATE_LOGIN_ID = strUserId;
                    codetable.OVC_MODIFY_LOGIN_ID = strUserId;
                    codetable.BLD_SN = Guid.NewGuid();
                    codetable.ONB_DANGER_PRO = strONB_DANGER_PRO;

                    MTSE.TBGMT_BLD.Add(codetable);
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), codetable.GetType().Name.ToString(), this, "新增");
                    #endregion

                    #region TBGMT_BLD_MRPLOG 新增LOG
                    TBGMT_BLD_MRPLOG codetablelog = new TBGMT_BLD_MRPLOG();
                    codetablelog.LOG_LOGIN_ID = strUserId;
                    codetablelog.LOG_TIME = dateNow;
                    codetablelog.LOG_EVENT = "INSERT";
                    codetablelog.OVC_BLD_NO = strOVC_BLD_NO;
                    codetablelog.OVC_SHIP_COMPANY = strOVC_SHIP_COMPANY;
                    codetablelog.OVC_SEA_OR_AIR = strOVC_SEA_OR_AIR;
                    codetablelog.OVC_SHIP_NAME = strOVC_SHIP_NAME;
                    codetablelog.OVC_VOYAGE = strOVC_VOYAGE;
                    codetablelog.ODT_START_DATE = dateODT_START_DATE;
                    codetablelog.OVC_START_PORT = strOVC_START_PORT;
                    codetablelog.ODT_PLN_ARRIVE_DATE = dateODT_PLN_ARRIVE_DATE;
                    if (boolODT_ACT_ARRIVE_DATE) codetablelog.ODT_ACT_ARRIVE_DATE = dateODT_ACT_ARRIVE_DATE; else codetablelog.ODT_ACT_ARRIVE_DATE = null;
                    codetablelog.OVC_ARRIVE_PORT = strOVC_ARRIVE_PORT;
                    codetablelog.ONB_QUANITY = decONB_QUANITY;
                    codetablelog.OVC_QUANITY_UNIT = strOVC_QUANITY_UNIT;
                    codetablelog.ONB_VOLUME = decONB_VOLUME;
                    if (boolONB_ON_SHIP_VOLUME) codetablelog.ONB_ON_SHIP_VOLUME = decONB_ON_SHIP_VOLUME; else codetablelog.ONB_ON_SHIP_VOLUME = null;
                    codetablelog.OVC_VOLUME_UNIT = strOVC_VOLUME_UNIT;
                    codetablelog.ONB_WEIGHT = decONB_WEIGHT;
                    codetablelog.OVC_WEIGHT_UNIT = strOVC_WEIGHT_UNIT;
                    codetablelog.ONB_ITEM_VALUE = decONB_ITEM_VALUE;
                    codetablelog.ONB_CARRIAGE_CURRENCY_I = strONB_CARRIAGE_CURRENCY_I;
                    codetablelog.ONB_CARRIAGE = decONB_CARRIAGE;
                    codetablelog.ONB_CARRIAGE_CURRENCY = strONB_CARRIAGE_CURRENCY;
                    codetablelog.OVC_MILITARY_TYPE = strOVC_MILITARY_TYPE;
                    codetablelog.OVC_IS_SECURITY = Convert.ToByte(strOVC_IS_SECURITY);
                    codetablelog.OVC_STATUS = "A";
                    codetablelog.ODT_CREATE_DATE = dateNow;
                    codetablelog.ODT_MODIFY_DATE = dateNow;
                    codetablelog.OVC_CREATE_LOGIN_ID = strUserId;
                    codetablelog.OVC_MODIFY_LOGIN_ID = strUserId;
                    codetablelog.MRPLOG_SN = Guid.NewGuid();
                    codetablelog.ONB_DANGER_PRO = strONB_DANGER_PRO;

                    MTSE.TBGMT_BLD_MRPLOG.Add(codetablelog);
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), codetablelog.GetType().Name.ToString(), this, "新增");
                    #endregion
                    FCommon.AlertShow(pnwarning, "success", "系統訊息", "新增成功");
                }
                catch(Exception ex)
                {
                    // FCommon.AlertShow(pnwarning, "danger", "系統訊息", ex.ToString());
                    FCommon.AlertShow(pnwarning, "danger", "系統訊息", "新增失敗，請聯絡資訊人員");
                }
            }
            else
                FCommon.AlertShow(pnwarning, "danger", "系統訊息", strMessage);
        }
        
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