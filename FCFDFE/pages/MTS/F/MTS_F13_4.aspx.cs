using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F13_4 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        private MTSEntities MTS = new MTSEntities();
        TBGMT_CARRIAGE_DISCOUNT discount = new TBGMT_CARRIAGE_DISCOUNT();

        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_INS_TYPE", Request.QueryString["OVC_INS_TYPE"], false);
            return strQueryString;
            //在接收頁面加入此副程式
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (!IsPostBack)
                {
                    FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_CHI_NAME, txtOdtStartDate, txtOdtEndDate);

                    //承運航商
                    var query =
                        from company in MTS.TBGMT_COMPANY
                        where company.OVC_CO_TYPE.Equals("3")
                        where company.OVC_REMARK_2.Equals("空運")
                        select company;
                    DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                    FCommon.list_dataImport(drpOVC_SHIP_COMPANY, dt, "OVC_COMPANY", "OVC_COMPANY", "未輸入", "", false);
                    CommonMTS.list_dataImport_CURRENCY(drpONB_CARRIAGE_CURRENCY, false); //運費幣別
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            #region 宣告取值
            string strMessage = "";
            string strOVC_SHIP_COMPANY = drpOVC_SHIP_COMPANY.SelectedValue;//承運航商
            string strOVC_PORT_CDE = txtOVC_PORT_CDE.Text;//啟運機場
            string strONB_CARRIAGE_CURRENCY = drpONB_CARRIAGE_CURRENCY.SelectedValue;//幣別
            string strOdtStartDate = txtOdtStartDate.Text;//合約開始日
            string strOdtEndDate = txtOdtEndDate.Text;//合約結束日
            //1
            string strOVC_IMPORT_EXPORT_1 = drpOVC_IMPORT_EXPORT_1.SelectedItem.Text;
            string strONB_DISCOUNT_1 = txtONB_DISCOUNT_1.Text;
            string strONB_M_RATE_1 = txtONB_M_RATE_1.Text;
            string strONB_AS_N_RATE_1 = txtONB_AS_N_RATE_1.Text;
            string strONB_N_RATE_1 = txtONB_N_RATE_1.Text;
            string strONB_AS_45_KG_1 = txtONB_AS_45_KG_1.Text;
            string strONB_PLUS_45_KG_1 = txtONB_PLUS_45_KG_1.Text;
            string strONB_AS_100_KG_1 = txtONB_AS_100_KG_1.Text;
            string strONB_PLUS_100_KG_1 = txtONB_PLUS_100_KG_1.Text;
            string strONB_AS_300_KG_1 = txtONB_AS_300_KG_1.Text;
            string strONB_PLUS_300_KG_1 = txtONB_PLUS_300_KG_1.Text;
            string strONB_AS_500_KG_1 = txtONB_AS_500_KG_1.Text;
            string strONB_PLUS_500_KG_1 = txtONB_PLUS_500_KG_1.Text;
            string strONB_EUR_BLD_QUANTITY_1 = txtONB_EUR_BLD_QUANTITY_1.Text;
            string strONB_EUR_DANGER_PRO_COST_1 = txtONB_EUR_DANGER_PRO_COST_1.Text;
            string strONB_EUR_FIN_DANGER_PRO_COST_1 = txtONB_EUR_FIN_DANGER_PRO_COST_1.Text;
            string strONB_USA_BLD_QUANTITY_1 = txtONB_USA_BLD_QUANTITY_1.Text;
            string strONB_USA_DANGER_PRO_COST_1 = txtONB_USA_DANGER_PRO_COST_1.Text;
            string strONB_USA_FIN_DANGER_PRO_COST_1 = txtONB_USA_FIN_DANGER_PRO_COST_1.Text;
            //2
            string strOVC_IMPORT_EXPORT_2 = drpOVC_IMPORT_EXPORT_2.SelectedItem.Text;
            string strONB_DISCOUNT_2 = txtONB_DISCOUNT_2.Text;
            string strONB_M_RATE_2 = txtONB_M_RATE_2.Text;
            string strONB_AS_N_RATE_2 = txtONB_AS_N_RATE_2.Text;
            string strONB_N_RATE_2 = txtONB_N_RATE_2.Text;
            string strONB_AS_45_KG_2 = txtONB_AS_45_KG_2.Text;
            string strONB_PLUS_45_KG_2 = txtONB_PLUS_45_KG_2.Text;
            string strONB_AS_100_KG_2 = txtONB_AS_100_KG_2.Text;
            string strONB_PLUS_100_KG_2 = txtONB_PLUS_100_KG_2.Text;
            string strONB_AS_300_KG_2 = txtONB_AS_300_KG_2.Text;
            string strONB_PLUS_300_KG_2 = txtONB_PLUS_300_KG_2.Text;
            string strONB_AS_500_KG_2 = txtONB_AS_500_KG_2.Text;
            string strONB_PLUS_500_KG_2 = txtONB_PLUS_500_KG_2.Text;
            string strONB_EUR_BLD_QUANTITY_2 = txtONB_EUR_BLD_QUANTITY_2.Text;
            string strONB_EUR_DANGER_PRO_COST_2 = txtONB_EUR_DANGER_PRO_COST_2.Text;
            string strONB_EUR_FIN_DANGER_PRO_COST_2 = txtONB_EUR_FIN_DANGER_PRO_COST_2.Text;
            string strONB_USA_BLD_QUANTITY_2 = txtONB_USA_BLD_QUANTITY_2.Text;
            string strONB_USA_DANGER_PRO_COST_2 = txtONB_USA_DANGER_PRO_COST_2.Text;
            string strONB_USA_FIN_DANGER_PRO_COST_2 = txtONB_USA_FIN_DANGER_PRO_COST_2.Text;
            //3
            string strOVC_IMPORT_EXPORT_3 = drpOVC_IMPORT_EXPORT_3.SelectedItem.Text;
            string strONB_DISCOUNT_3 = txtONB_DISCOUNT_3.Text;
            string strONB_M_RATE_3 = txtONB_M_RATE_3.Text;
            string strONB_AS_N_RATE_3 = txtONB_AS_N_RATE_3.Text;
            string strONB_N_RATE_3 = txtONB_N_RATE_3.Text;
            string strONB_AS_45_KG_3 = txtONB_AS_45_KG_3.Text;
            string strONB_PLUS_45_KG_3 = txtONB_PLUS_45_KG_3.Text;
            string strONB_AS_100_KG_3 = txtONB_AS_100_KG_3.Text;
            string strONB_PLUS_100_KG_3 = txtONB_PLUS_100_KG_3.Text;
            string strONB_AS_300_KG_3 = txtONB_AS_300_KG_3.Text;
            string strONB_PLUS_300_KG_3 = txtONB_PLUS_300_KG_3.Text;
            string strONB_AS_500_KG_3 = txtONB_AS_500_KG_3.Text;
            string strONB_PLUS_500_KG_3 = txtONB_PLUS_500_KG_3.Text;
            string strOVC_REMARK = txtOVC_REMARK.Text;//備考
            DateTime dtOdtStartDate, dtOdtEndDate;
            decimal decONB_DISCOUNT_1, decONB_M_RATE_1, decONB_AS_N_RATE_1, decONB_N_RATE_1, decONB_AS_45_KG_1, decONB_PLUS_45_KG_1, decONB_AS_100_KG_1, decONB_PLUS_100_KG_1,
                decONB_AS_300_KG_1, decONB_PLUS_300_KG_1, decONB_AS_500_KG_1, decONB_PLUS_500_KG_1, decONB_EUR_BLD_QUANTITY_1, decONB_EUR_DANGER_PRO_COST_1,
                decONB_EUR_FIN_DANGER_PRO_COST_1, decONB_USA_BLD_QUANTITY_1, decONB_USA_DANGER_PRO_COST_1, decONB_USA_FIN_DANGER_PRO_COST_1;
            decimal decONB_DISCOUNT_2, decONB_M_RATE_2, decONB_AS_N_RATE_2, decONB_N_RATE_2, decONB_AS_45_KG_2, decONB_PLUS_45_KG_2, decONB_AS_100_KG_2, decONB_PLUS_100_KG_2,
                decONB_AS_300_KG_2, decONB_PLUS_300_KG_2, decONB_AS_500_KG_2, decONB_PLUS_500_KG_2, decONB_EUR_BLD_QUANTITY_2, decONB_EUR_DANGER_PRO_COST_2,
                decONB_EUR_FIN_DANGER_PRO_COST_2, decONB_USA_BLD_QUANTITY_2, decONB_USA_DANGER_PRO_COST_2, decONB_USA_FIN_DANGER_PRO_COST_2;
            decimal decONB_DISCOUNT_3, decONB_M_RATE_3, decONB_AS_N_RATE_3, decONB_N_RATE_3, decONB_AS_45_KG_3, decONB_PLUS_45_KG_3, decONB_AS_100_KG_3, decONB_PLUS_100_KG_3,
                decONB_AS_300_KG_3, decONB_PLUS_300_KG_3, decONB_AS_500_KG_3, decONB_PLUS_500_KG_3;
            #endregion
            #region 檢查格式
            if (strOVC_PORT_CDE.Equals(string.Empty))
            {
                strMessage += "<P> 請選擇 啟運機場 </p>";
            }
            if (strOdtStartDate.Equals(string.Empty))
            {
                strMessage += "<P> 請選擇 合約開始日 </p>";
            }
            if (strOdtEndDate.Equals(string.Empty))
            {
                strMessage += "<P> 請選擇 合約結束日 </p>";
            }
            FCommon.checkDateTime(strOdtStartDate, "合約開始日", ref strMessage, out dtOdtStartDate);
            FCommon.checkDateTime(strOdtEndDate, "合約結束日", ref strMessage, out dtOdtEndDate);
            FCommon.checkDecimal(strONB_DISCOUNT_1, "折扣數", ref strMessage, out decONB_DISCOUNT_1);
            FCommon.checkDecimal(strONB_M_RATE_1, "M-Rate", ref strMessage, out decONB_M_RATE_1);
            FCommon.checkDecimal(strONB_AS_N_RATE_1, "As N-Rate", ref strMessage, out decONB_AS_N_RATE_1);
            FCommon.checkDecimal(strONB_N_RATE_1, "N-RATE", ref strMessage, out decONB_N_RATE_1);
            FCommon.checkDecimal(strONB_AS_45_KG_1, "AS 45 KG", ref strMessage, out decONB_AS_45_KG_1);
            FCommon.checkDecimal(strONB_PLUS_45_KG_1, "+45 KG", ref strMessage, out decONB_PLUS_45_KG_1);
            FCommon.checkDecimal(strONB_AS_100_KG_1, "As 100 KG", ref strMessage, out decONB_AS_100_KG_1);
            FCommon.checkDecimal(strONB_PLUS_100_KG_1, "+100 KG", ref strMessage, out decONB_PLUS_100_KG_1);
            FCommon.checkDecimal(strONB_AS_300_KG_1, "As 300 KG", ref strMessage, out decONB_AS_300_KG_1);
            FCommon.checkDecimal(strONB_PLUS_300_KG_1, "+300 KG", ref strMessage, out decONB_PLUS_300_KG_1);
            FCommon.checkDecimal(strONB_AS_500_KG_1, "As 500 KG", ref strMessage, out decONB_AS_500_KG_1);
            FCommon.checkDecimal(strONB_PLUS_500_KG_1, "+500 KG", ref strMessage, out decONB_PLUS_500_KG_1);
            FCommon.checkDecimal(strONB_EUR_BLD_QUANTITY_1, "提單", ref strMessage, out decONB_EUR_BLD_QUANTITY_1);
            FCommon.checkDecimal(strONB_EUR_DANGER_PRO_COST_1, "危險品收費", ref strMessage, out decONB_EUR_DANGER_PRO_COST_1);
            FCommon.checkDecimal(strONB_EUR_FIN_DANGER_PRO_COST_1, "歐洲線危險品收費", ref strMessage, out decONB_EUR_FIN_DANGER_PRO_COST_1);
            FCommon.checkDecimal(strONB_USA_BLD_QUANTITY_1, "提單", ref strMessage, out decONB_USA_BLD_QUANTITY_1);
            FCommon.checkDecimal(strONB_USA_DANGER_PRO_COST_1, "危險品收費", ref strMessage, out decONB_USA_DANGER_PRO_COST_1);
            FCommon.checkDecimal(strONB_USA_FIN_DANGER_PRO_COST_1, "美國線危險品收費", ref strMessage, out decONB_USA_FIN_DANGER_PRO_COST_1);
            FCommon.checkDecimal(strONB_DISCOUNT_2, "折扣數", ref strMessage, out decONB_DISCOUNT_2);
            FCommon.checkDecimal(strONB_M_RATE_2, "M-Rate", ref strMessage, out decONB_M_RATE_2);
            FCommon.checkDecimal(strONB_AS_N_RATE_2, "As N-Rate", ref strMessage, out decONB_AS_N_RATE_2);
            FCommon.checkDecimal(strONB_N_RATE_2, "N-RATE", ref strMessage, out decONB_N_RATE_2);
            FCommon.checkDecimal(strONB_AS_45_KG_2, "AS 45 KG", ref strMessage, out decONB_AS_45_KG_2);
            FCommon.checkDecimal(strONB_PLUS_45_KG_2, "+45 KG", ref strMessage, out decONB_PLUS_45_KG_2);
            FCommon.checkDecimal(strONB_AS_100_KG_2, "As 100 KG", ref strMessage, out decONB_AS_100_KG_2);
            FCommon.checkDecimal(strONB_PLUS_100_KG_2, "+100 KG", ref strMessage, out decONB_PLUS_100_KG_2);
            FCommon.checkDecimal(strONB_AS_300_KG_2, "As 300 KG", ref strMessage, out decONB_AS_300_KG_2);
            FCommon.checkDecimal(strONB_PLUS_300_KG_2, "+300 KG", ref strMessage, out decONB_PLUS_300_KG_2);
            FCommon.checkDecimal(strONB_AS_500_KG_2, "As 500 KG", ref strMessage, out decONB_AS_500_KG_2);
            FCommon.checkDecimal(strONB_PLUS_500_KG_2, "+500 KG", ref strMessage, out decONB_PLUS_500_KG_2);
            FCommon.checkDecimal(strONB_EUR_BLD_QUANTITY_2, "提單", ref strMessage, out decONB_EUR_BLD_QUANTITY_2);
            FCommon.checkDecimal(strONB_EUR_DANGER_PRO_COST_2, "危險品收費", ref strMessage, out decONB_EUR_DANGER_PRO_COST_2);
            FCommon.checkDecimal(strONB_EUR_FIN_DANGER_PRO_COST_2, "歐洲線危險品收費", ref strMessage, out decONB_EUR_FIN_DANGER_PRO_COST_2);
            FCommon.checkDecimal(strONB_USA_BLD_QUANTITY_2, "提單", ref strMessage, out decONB_USA_BLD_QUANTITY_2);
            FCommon.checkDecimal(strONB_USA_DANGER_PRO_COST_2, "危險品收費", ref strMessage, out decONB_USA_DANGER_PRO_COST_2);
            FCommon.checkDecimal(strONB_USA_FIN_DANGER_PRO_COST_2, "美國線危險品收費", ref strMessage, out decONB_USA_FIN_DANGER_PRO_COST_2);
            FCommon.checkDecimal(strONB_DISCOUNT_3, "折扣數", ref strMessage, out decONB_DISCOUNT_3);
            FCommon.checkDecimal(strONB_M_RATE_3, "M-Rate", ref strMessage, out decONB_M_RATE_3);
            FCommon.checkDecimal(strONB_AS_N_RATE_3, "As N-Rate", ref strMessage, out decONB_AS_N_RATE_3);
            FCommon.checkDecimal(strONB_N_RATE_3, "N-RATE", ref strMessage, out decONB_N_RATE_3);
            FCommon.checkDecimal(strONB_AS_45_KG_3, "AS 45 KG", ref strMessage, out decONB_AS_45_KG_3);
            FCommon.checkDecimal(strONB_PLUS_45_KG_3, "+45 KG", ref strMessage, out decONB_PLUS_45_KG_3);
            FCommon.checkDecimal(strONB_AS_100_KG_3, "As 100 KG", ref strMessage, out decONB_AS_100_KG_3);
            FCommon.checkDecimal(strONB_PLUS_100_KG_3, "+100 KG", ref strMessage, out decONB_PLUS_100_KG_3);
            FCommon.checkDecimal(strONB_AS_300_KG_3, "As 300 KG", ref strMessage, out decONB_AS_300_KG_3);
            FCommon.checkDecimal(strONB_PLUS_300_KG_3, "+300 KG", ref strMessage, out decONB_PLUS_300_KG_3);
            FCommon.checkDecimal(strONB_AS_500_KG_3, "As 500 KG", ref strMessage, out decONB_AS_500_KG_3);
            FCommon.checkDecimal(strONB_PLUS_500_KG_3, "+500 KG", ref strMessage, out decONB_PLUS_500_KG_3);
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                #region 合約日期重複判斷
                var query =
                    from tbgmt_air in MTS.TBGMT_AIR_TRANSPORT.AsEnumerable()
                    join tbgmt_com in MTS.TBGMT_COMPANY on tbgmt_air.CO_SN equals tbgmt_com.CO_SN
                    where tbgmt_com.OVC_COMPANY.Equals(strOVC_SHIP_COMPANY)
                    where tbgmt_air.OVC_START_PORT.Equals(strOVC_PORT_CDE)
                    select tbgmt_air;
                var query_start_date = query
                    .Where(t => DateTime.Compare(Convert.ToDateTime(t.ODT_START_DATE), dtOdtStartDate) <= 0)
                    .Where(t => DateTime.Compare(Convert.ToDateTime(t.ODT_END_DATE), dtOdtStartDate) >= 0);
                var query_end_date = query
                    .Where(t => DateTime.Compare(Convert.ToDateTime(t.ODT_START_DATE), dtOdtEndDate) <= 0)
                    .Where(t => DateTime.Compare(Convert.ToDateTime(t.ODT_END_DATE), dtOdtEndDate) >= 0);
                #endregion
                if (query_start_date.Any() || query_end_date.Any())
                {
                    strMessage += "<p> 合約日期重疊 </p>";
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                }
                else
                {
                    #region 新增TBGMT_AIR_TRANSPORT
                    TBGMT_AIR_TRANSPORT air_new = new TBGMT_AIR_TRANSPORT();
                    air_new.AT_SN = Guid.NewGuid();
                    air_new.CO_SN = MTS.TBGMT_COMPANY.Where(t => t.OVC_COMPANY.Equals(strOVC_SHIP_COMPANY)).Select(t => t.CO_SN).FirstOrDefault();
                    air_new.OVC_START_PORT = strOVC_PORT_CDE;
                    air_new.ONB_CARRIAGE_CURRENCY = strONB_CARRIAGE_CURRENCY;
                    air_new.ODT_START_DATE = dtOdtStartDate;
                    air_new.ODT_END_DATE = dtOdtEndDate;
                    air_new.OVC_IMPORT_EXPORT_1 = strOVC_IMPORT_EXPORT_1;
                    if (strONB_DISCOUNT_1.Equals(string.Empty)) air_new.ONB_DISCOUNT_1 = null; else air_new.ONB_DISCOUNT_1 = decONB_DISCOUNT_1;
                    if (strONB_M_RATE_1.Equals(string.Empty)) air_new.ONB_M_RATE_1 = null; else air_new.ONB_M_RATE_1 = decONB_M_RATE_1;
                    if (strONB_AS_N_RATE_1.Equals(string.Empty)) air_new.ONB_AS_N_RATE_1 = null; else air_new.ONB_AS_N_RATE_1 = decONB_AS_N_RATE_1;
                    if (strONB_N_RATE_1.Equals(string.Empty)) air_new.ONB_N_RATE_1 = null; else air_new.ONB_N_RATE_1 = decONB_N_RATE_1;
                    if (strONB_AS_45_KG_1.Equals(string.Empty)) air_new.ONB_AS_45_KG_1 = null; else air_new.ONB_AS_45_KG_1 = decONB_AS_45_KG_1;
                    if (strONB_PLUS_45_KG_1.Equals(string.Empty)) air_new.ONB_PLUS_45_KG_1 = null; else air_new.ONB_PLUS_45_KG_1 = decONB_PLUS_45_KG_1;
                    if (strONB_AS_100_KG_1.Equals(string.Empty)) air_new.ONB_AS_100_KG_1 = null; else air_new.ONB_AS_100_KG_1 = decONB_AS_100_KG_1;
                    if (strONB_PLUS_100_KG_1.Equals(string.Empty)) air_new.ONB_PLUS_100_KG_1 = null; else air_new.ONB_PLUS_100_KG_1 = decONB_PLUS_100_KG_1;
                    if (strONB_AS_300_KG_1.Equals(string.Empty)) air_new.ONB_AS_300_KG_1 = null; else air_new.ONB_AS_300_KG_1 = decONB_AS_300_KG_1;
                    if (strONB_PLUS_300_KG_1.Equals(string.Empty)) air_new.ONB_PLUS_300_KG_1 = null; else air_new.ONB_PLUS_300_KG_1 = decONB_PLUS_300_KG_1;
                    if (strONB_AS_500_KG_1.Equals(string.Empty)) air_new.ONB_AS_500_KG_1 = null; else air_new.ONB_AS_500_KG_1 = decONB_AS_500_KG_1;
                    if (strONB_PLUS_500_KG_1.Equals(string.Empty)) air_new.ONB_PLUS_500_KG_1 = null; else air_new.ONB_PLUS_500_KG_1 = decONB_PLUS_500_KG_1;
                    if (strONB_EUR_BLD_QUANTITY_1.Equals(string.Empty)) air_new.ONB_EUR_BLD_QUANTITY_1 = null; else air_new.ONB_EUR_BLD_QUANTITY_1 = decONB_EUR_BLD_QUANTITY_1;
                    if (strONB_EUR_DANGER_PRO_COST_1.Equals(string.Empty)) air_new.ONB_EUR_DANGER_PRO_COST_1 = null; else air_new.ONB_EUR_DANGER_PRO_COST_1 = decONB_EUR_DANGER_PRO_COST_1;
                    if (strONB_EUR_FIN_DANGER_PRO_COST_1.Equals(string.Empty)) air_new.ONB_EUR_FIN_DANGER_PRO_COST_1 = null; else air_new.ONB_EUR_FIN_DANGER_PRO_COST_1 = decONB_EUR_FIN_DANGER_PRO_COST_1;
                    if (strONB_USA_BLD_QUANTITY_1.Equals(string.Empty)) air_new.ONB_USA_BLD_QUANTITY_1 = null; else air_new.ONB_USA_BLD_QUANTITY_1 = decONB_USA_BLD_QUANTITY_1;
                    if (strONB_USA_DANGER_PRO_COST_1.Equals(string.Empty)) air_new.ONB_USA_DANGER_PRO_COST_1 = null; else air_new.ONB_USA_DANGER_PRO_COST_1 = decONB_USA_DANGER_PRO_COST_1;
                    if (strONB_USA_FIN_DANGER_PRO_COST_1.Equals(string.Empty)) air_new.ONB_USA_FIN_DANGER_PRO_COST_1 = null; else air_new.ONB_USA_FIN_DANGER_PRO_COST_1 = decONB_USA_FIN_DANGER_PRO_COST_1;
                    if (strOVC_IMPORT_EXPORT_2.Equals(string.Empty)) air_new.OVC_IMPORT_EXPORT_2 = null; else air_new.OVC_IMPORT_EXPORT_2 = strOVC_IMPORT_EXPORT_2;
                    air_new.OVC_IMPORT_EXPORT_2 = strOVC_IMPORT_EXPORT_2;
                    if (strONB_DISCOUNT_2.Equals(string.Empty)) air_new.ONB_DISCOUNT_2 = null; else air_new.ONB_DISCOUNT_2 = decONB_DISCOUNT_2;
                    if (strONB_M_RATE_2.Equals(string.Empty)) air_new.ONB_M_RATE_2 = null; else air_new.ONB_M_RATE_2 = decONB_M_RATE_2;
                    if (strONB_AS_N_RATE_2.Equals(string.Empty)) air_new.ONB_AS_N_RATE_2 = null; else air_new.ONB_AS_N_RATE_2 = decONB_AS_N_RATE_2;
                    if (strONB_N_RATE_2.Equals(string.Empty)) air_new.ONB_N_RATE_2 = null; else air_new.ONB_N_RATE_2 = decONB_N_RATE_2;
                    if (strONB_AS_45_KG_2.Equals(string.Empty)) air_new.ONB_AS_45_KG_2 = null; else air_new.ONB_AS_45_KG_2 = decONB_AS_45_KG_2;
                    if (strONB_PLUS_45_KG_2.Equals(string.Empty)) air_new.ONB_PLUS_45_KG_2 = null; else air_new.ONB_PLUS_45_KG_2 = decONB_PLUS_45_KG_2;
                    if (strONB_AS_100_KG_2.Equals(string.Empty)) air_new.ONB_AS_100_KG_2 = null; else air_new.ONB_AS_100_KG_2 = decONB_AS_100_KG_2;
                    if (strONB_PLUS_100_KG_2.Equals(string.Empty)) air_new.ONB_PLUS_100_KG_2 = null; else air_new.ONB_PLUS_100_KG_2 = decONB_PLUS_100_KG_2;
                    if (strONB_AS_300_KG_2.Equals(string.Empty)) air_new.ONB_AS_300_KG_2 = null; else air_new.ONB_AS_300_KG_2 = decONB_AS_300_KG_2;
                    if (strONB_PLUS_300_KG_2.Equals(string.Empty)) air_new.ONB_PLUS_300_KG_2 = null; else air_new.ONB_PLUS_300_KG_2 = decONB_PLUS_300_KG_2;
                    if (strONB_AS_500_KG_2.Equals(string.Empty)) air_new.ONB_AS_500_KG_2 = null; else air_new.ONB_AS_500_KG_2 = decONB_AS_500_KG_2;
                    if (strONB_PLUS_500_KG_2.Equals(string.Empty)) air_new.ONB_PLUS_500_KG_2 = null; else air_new.ONB_PLUS_500_KG_2 = decONB_PLUS_500_KG_2;
                    if (strONB_EUR_BLD_QUANTITY_2.Equals(string.Empty)) air_new.ONB_EUR_BLD_QUANTITY_2 = null; else air_new.ONB_EUR_BLD_QUANTITY_2 = decONB_EUR_BLD_QUANTITY_2;
                    if (strONB_EUR_DANGER_PRO_COST_2.Equals(string.Empty)) air_new.ONB_EUR_DANGER_PRO_COST_2 = null; else air_new.ONB_EUR_DANGER_PRO_COST_2 = decONB_EUR_DANGER_PRO_COST_2;
                    if (strONB_EUR_FIN_DANGER_PRO_COST_2.Equals(string.Empty)) air_new.ONB_EUR_FIN_DANGER_PRO_COST_2 = null; else air_new.ONB_EUR_FIN_DANGER_PRO_COST_2 = decONB_EUR_FIN_DANGER_PRO_COST_2;
                    if (strONB_USA_BLD_QUANTITY_2.Equals(string.Empty)) air_new.ONB_USA_BLD_QUANTITY_2 = null; else air_new.ONB_USA_BLD_QUANTITY_2 = decONB_USA_BLD_QUANTITY_2;
                    if (strONB_USA_DANGER_PRO_COST_2.Equals(string.Empty)) air_new.ONB_USA_DANGER_PRO_COST_2 = null; else air_new.ONB_USA_DANGER_PRO_COST_2 = decONB_USA_DANGER_PRO_COST_2;
                    if (strONB_USA_FIN_DANGER_PRO_COST_2.Equals(string.Empty)) air_new.ONB_USA_FIN_DANGER_PRO_COST_2 = null; else air_new.ONB_USA_FIN_DANGER_PRO_COST_2 = decONB_USA_FIN_DANGER_PRO_COST_2;
                    if (strOVC_IMPORT_EXPORT_2.Equals(string.Empty)) air_new.OVC_IMPORT_EXPORT_2 = null; else air_new.OVC_IMPORT_EXPORT_2 = strOVC_IMPORT_EXPORT_2;
                    air_new.OVC_IMPORT_EXPORT_3 = strOVC_IMPORT_EXPORT_3;
                    if (strONB_DISCOUNT_3.Equals(string.Empty)) air_new.ONB_DISCOUNT_3 = null; else air_new.ONB_DISCOUNT_3 = decONB_DISCOUNT_3;
                    if (strONB_M_RATE_3.Equals(string.Empty)) air_new.ONB_M_RATE_3 = null; else air_new.ONB_M_RATE_3 = decONB_M_RATE_3;
                    if (strONB_AS_N_RATE_3.Equals(string.Empty)) air_new.ONB_AS_N_RATE_3 = null; else air_new.ONB_AS_N_RATE_3 = decONB_AS_N_RATE_3;
                    if (strONB_N_RATE_3.Equals(string.Empty)) air_new.ONB_N_RATE_3 = null; else air_new.ONB_N_RATE_3 = decONB_N_RATE_3;
                    if (strONB_AS_45_KG_3.Equals(string.Empty)) air_new.ONB_AS_45_KG_3 = null; else air_new.ONB_AS_45_KG_3 = decONB_AS_45_KG_3;
                    if (strONB_PLUS_45_KG_3.Equals(string.Empty)) air_new.ONB_PLUS_45_KG_3 = null; else air_new.ONB_PLUS_45_KG_3 = decONB_PLUS_45_KG_3;
                    if (strONB_AS_100_KG_3.Equals(string.Empty)) air_new.ONB_AS_100_KG_3 = null; else air_new.ONB_AS_100_KG_3 = decONB_AS_100_KG_3;
                    if (strONB_PLUS_100_KG_3.Equals(string.Empty)) air_new.ONB_PLUS_100_KG_3 = null; else air_new.ONB_PLUS_100_KG_3 = decONB_PLUS_100_KG_3;
                    if (strONB_AS_300_KG_3.Equals(string.Empty)) air_new.ONB_AS_300_KG_3 = null; else air_new.ONB_AS_300_KG_3 = decONB_AS_300_KG_3;
                    if (strONB_PLUS_300_KG_3.Equals(string.Empty)) air_new.ONB_PLUS_300_KG_3 = null; else air_new.ONB_PLUS_300_KG_3 = decONB_PLUS_300_KG_3;
                    if (strONB_AS_500_KG_3.Equals(string.Empty)) air_new.ONB_AS_500_KG_3 = null; else air_new.ONB_AS_500_KG_3 = decONB_AS_500_KG_3;
                    if (strONB_PLUS_500_KG_3.Equals(string.Empty)) air_new.ONB_PLUS_500_KG_3 = null; else air_new.ONB_PLUS_500_KG_3 = decONB_PLUS_500_KG_3;
                    if (strOVC_IMPORT_EXPORT_3.Equals(string.Empty)) air_new.OVC_IMPORT_EXPORT_3 = null; else air_new.OVC_IMPORT_EXPORT_3 = strOVC_IMPORT_EXPORT_3;
                    air_new.OVC_REMARK = strOVC_REMARK;
                    MTS.TBGMT_AIR_TRANSPORT.Add(air_new);
                    MTS.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), air_new.GetType().Name.ToString(), this, "新增");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功");
                    #endregion
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        
        protected void txtONB_DANGER_PRO_COST_TextChanged(object sender, EventArgs e)
        {
            for (int i = 1; i < 3; i++)
            {
                string strONB_EUR_BLD_QUANTITY = "txtONB_EUR_BLD_QUANTITY_" + i;
                string strONB_EUR_DANGER_PRO_COST = "txtONB_EUR_DANGER_PRO_COST_" + i;
                string strONB_EUR_FIN_DANGER_PRO_COST = "txtONB_EUR_FIN_DANGER_PRO_COST_" + i;
                TextBox txtONB_EUR_BLD_QUANTITY = (TextBox)tbAIR.FindControl(strONB_EUR_BLD_QUANTITY);
                TextBox txtONB_EUR_DANGER_PRO_COST = (TextBox)tbAIR.FindControl(strONB_EUR_DANGER_PRO_COST);
                TextBox txtONB_EUR_FIN_DANGER_PRO_COST = (TextBox)tbAIR.FindControl(strONB_EUR_FIN_DANGER_PRO_COST);
                bool boolONB_EUR_BLD_QUANTITY = decimal.TryParse(txtONB_EUR_BLD_QUANTITY.Text, out decimal decONB_EUR_BLD_QUANTITY);
                bool boolONB_EUR_DANGER_PRO_COST = decimal.TryParse(txtONB_EUR_DANGER_PRO_COST.Text, out decimal decONB_EUR_DANGER_PRO_COST);
                if (boolONB_EUR_BLD_QUANTITY && boolONB_EUR_DANGER_PRO_COST)
                {
                    txtONB_EUR_FIN_DANGER_PRO_COST.Text = (decONB_EUR_BLD_QUANTITY * decONB_EUR_DANGER_PRO_COST / 100).ToString();
                }
                string strONB_USA_BLD_QUANTITY = "txtONB_USA_BLD_QUANTITY_" + i;
                string strONB_USA_DANGER_PRO_COST = "txtONB_USA_DANGER_PRO_COST_" + i;
                string strONB_USA_FIN_DANGER_PRO_COST = "txtONB_USA_FIN_DANGER_PRO_COST_" + i;
                TextBox txtONB_USA_BLD_QUANTITY = (TextBox)tbAIR.FindControl(strONB_USA_BLD_QUANTITY);
                TextBox txtONB_USA_DANGER_PRO_COST = (TextBox)tbAIR.FindControl(strONB_USA_DANGER_PRO_COST);
                TextBox txtONB_USA_FIN_DANGER_PRO_COST = (TextBox)tbAIR.FindControl(strONB_USA_FIN_DANGER_PRO_COST);
                bool boolONB_USA_BLD_QUANTITY = decimal.TryParse(txtONB_USA_BLD_QUANTITY.Text, out decimal decONB_USA_BLD_QUANTITY);
                bool boolONB_USA_DANGER_PRO_COST = decimal.TryParse(txtONB_USA_DANGER_PRO_COST.Text, out decimal decONB_USA_DANGER_PRO_COST);
                if (boolONB_USA_BLD_QUANTITY && boolONB_USA_DANGER_PRO_COST)
                {
                    txtONB_USA_FIN_DANGER_PRO_COST.Text = (decONB_USA_BLD_QUANTITY * decONB_USA_DANGER_PRO_COST / 100).ToString();
                }
            }
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F13_1{ getQueryString() }");
        }
    }
}