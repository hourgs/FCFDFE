using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F13_7 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        private MTSEntities MTS = new MTSEntities();
        TBGMT_CARRIAGE_DISCOUNT discount = new TBGMT_CARRIAGE_DISCOUNT();
        Guid id;

        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_INS_TYPE", Request.QueryString["OVC_INS_TYPE"], false);
            return strQueryString;
            //在接收頁面加入此副程式
        }

        private void dataImport()
        {
            var query = MTS.TBGMT_AIR_TRANSPORT.Where(t => t.AT_SN.Equals(id)).FirstOrDefault();
            var query_com = MTS.TBGMT_COMPANY.Where(t => t.CO_SN.Equals(query.CO_SN)).FirstOrDefault();
            var query_port = MTS.TBGMT_PORTS.Where(t => t.OVC_PORT_CDE.Equals(query.OVC_START_PORT)).FirstOrDefault();
            FCommon.list_setValue(drpOVC_SHIP_COMPANY, query_com.OVC_COMPANY);
            txtOVC_PORT_CDE.Text = query.OVC_START_PORT;
            txtOVC_CHI_NAME.Text = query_port.OVC_PORT_CHI_NAME;
            FCommon.list_setValue(drpONB_CARRIAGE_CURRENCY, query.ONB_CARRIAGE_CURRENCY);
            txtOdtStartDate.Text = query.ODT_START_DATE == null ? "" : Convert.ToDateTime(query.ODT_START_DATE).ToString("yyyy-MM-dd");
            txtOdtEndDate.Text = query.ODT_END_DATE == null ? "" : Convert.ToDateTime(query.ODT_END_DATE).ToString("yyyy-MM-dd");
            if (query.OVC_IMPORT_EXPORT_1 == "進口") drpOVC_IMPORT_EXPORT_1.SelectedIndex = 0; else drpOVC_IMPORT_EXPORT_1.SelectedIndex = 1;
            TextboxImport(txtONB_DISCOUNT_1, query.ONB_DISCOUNT_1);
            TextboxImport(txtONB_M_RATE_1, query.ONB_M_RATE_1);
            TextboxImport(txtONB_AS_N_RATE_1, query.ONB_AS_N_RATE_1);
            TextboxImport(txtONB_N_RATE_1, query.ONB_N_RATE_1);
            TextboxImport(txtONB_AS_45_KG_1, query.ONB_AS_45_KG_1);
            TextboxImport(txtONB_PLUS_45_KG_1, query.ONB_PLUS_45_KG_1);
            TextboxImport(txtONB_AS_100_KG_1, query.ONB_AS_100_KG_1);
            TextboxImport(txtONB_PLUS_100_KG_1, query.ONB_PLUS_100_KG_1);
            TextboxImport(txtONB_AS_300_KG_1, query.ONB_AS_300_KG_1);
            TextboxImport(txtONB_PLUS_300_KG_1, query.ONB_PLUS_300_KG_1);
            TextboxImport(txtONB_AS_500_KG_1, query.ONB_AS_500_KG_1);
            TextboxImport(txtONB_PLUS_500_KG_1, query.ONB_PLUS_500_KG_1);
            TextboxImport(txtONB_EUR_BLD_QUANTITY_1, query.ONB_EUR_BLD_QUANTITY_1);
            TextboxImport(txtONB_EUR_DANGER_PRO_COST_1, query.ONB_EUR_DANGER_PRO_COST_1);
            TextboxImport(txtONB_EUR_FIN_DANGER_PRO_COST_1, query.ONB_EUR_FIN_DANGER_PRO_COST_1);
            TextboxImport(txtONB_USA_BLD_QUANTITY_1, query.ONB_USA_BLD_QUANTITY_1);
            TextboxImport(txtONB_USA_DANGER_PRO_COST_1, query.ONB_USA_DANGER_PRO_COST_1);
            TextboxImport(txtONB_USA_FIN_DANGER_PRO_COST_1, query.ONB_USA_FIN_DANGER_PRO_COST_1);
            if (query.OVC_IMPORT_EXPORT_2 == "進口") drpOVC_IMPORT_EXPORT_2.SelectedIndex = 0; else drpOVC_IMPORT_EXPORT_2.SelectedIndex = 1;
            TextboxImport(txtONB_DISCOUNT_2, query.ONB_DISCOUNT_2);
            TextboxImport(txtONB_M_RATE_2, query.ONB_M_RATE_2);
            TextboxImport(txtONB_AS_N_RATE_2, query.ONB_AS_N_RATE_2);
            TextboxImport(txtONB_N_RATE_2, query.ONB_N_RATE_2);
            TextboxImport(txtONB_AS_45_KG_2, query.ONB_AS_45_KG_2);
            TextboxImport(txtONB_PLUS_45_KG_2, query.ONB_PLUS_45_KG_2);
            TextboxImport(txtONB_AS_100_KG_2, query.ONB_AS_100_KG_2);
            TextboxImport(txtONB_PLUS_100_KG_2, query.ONB_PLUS_100_KG_2);
            TextboxImport(txtONB_AS_300_KG_2, query.ONB_AS_300_KG_2);
            TextboxImport(txtONB_PLUS_300_KG_2, query.ONB_PLUS_300_KG_2);
            TextboxImport(txtONB_AS_500_KG_2, query.ONB_AS_500_KG_2);
            TextboxImport(txtONB_PLUS_500_KG_2, query.ONB_PLUS_500_KG_2);
            TextboxImport(txtONB_EUR_BLD_QUANTITY_2, query.ONB_EUR_BLD_QUANTITY_2);
            TextboxImport(txtONB_EUR_DANGER_PRO_COST_2, query.ONB_EUR_DANGER_PRO_COST_2);
            TextboxImport(txtONB_EUR_FIN_DANGER_PRO_COST_2, query.ONB_EUR_FIN_DANGER_PRO_COST_2);
            TextboxImport(txtONB_USA_BLD_QUANTITY_2, query.ONB_USA_BLD_QUANTITY_2);
            TextboxImport(txtONB_USA_DANGER_PRO_COST_2, query.ONB_USA_DANGER_PRO_COST_2);
            TextboxImport(txtONB_USA_FIN_DANGER_PRO_COST_2, query.ONB_USA_FIN_DANGER_PRO_COST_2);
            if (query.OVC_IMPORT_EXPORT_3 == "進口") drpOVC_IMPORT_EXPORT_3.SelectedIndex = 0; else drpOVC_IMPORT_EXPORT_3.SelectedIndex = 1;
            TextboxImport(txtONB_DISCOUNT_3, query.ONB_DISCOUNT_3);
            TextboxImport(txtONB_M_RATE_3, query.ONB_M_RATE_3);
            TextboxImport(txtONB_AS_N_RATE_3, query.ONB_AS_N_RATE_3);
            TextboxImport(txtONB_N_RATE_3, query.ONB_N_RATE_3);
            TextboxImport(txtONB_AS_45_KG_3, query.ONB_AS_45_KG_3);
            TextboxImport(txtONB_PLUS_45_KG_3, query.ONB_PLUS_45_KG_3);
            TextboxImport(txtONB_AS_100_KG_3, query.ONB_AS_100_KG_3);
            TextboxImport(txtONB_PLUS_100_KG_3, query.ONB_PLUS_100_KG_3);
            TextboxImport(txtONB_AS_300_KG_3, query.ONB_AS_300_KG_3);
            TextboxImport(txtONB_PLUS_300_KG_3, query.ONB_PLUS_300_KG_3);
            TextboxImport(txtONB_AS_500_KG_3, query.ONB_AS_500_KG_3);
            TextboxImport(txtONB_PLUS_500_KG_3, query.ONB_PLUS_500_KG_3);
            txtOVC_REMARK.Text = query.OVC_REMARK;
        }

        void TextboxImport(TextBox txt, decimal? dec)
        {
            txt.Text = dec == null ? "" : dec.ToString();
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (FCommon.getQueryString(this, "id", out string strID, true))
                {
                    id = new Guid(strID);
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

                        dataImport();
                    }
                }
                else
                    FCommon.MessageBoxShow(this, "編號錯誤！", $"MTS_F13_1{ getQueryString() }", false);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            #region 取值
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
                    where tbgmt_air.AT_SN != id
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
                    #region 修改TBGMT_AIR_TRANSPORT
                    TBGMT_AIR_TRANSPORT air = MTS.TBGMT_AIR_TRANSPORT.Where(t => t.AT_SN.Equals(id)).FirstOrDefault();
                    try
                    {
                        air.CO_SN = MTS.TBGMT_COMPANY.Where(t => t.OVC_COMPANY.Equals(strOVC_SHIP_COMPANY)).Select(t => t.CO_SN).FirstOrDefault();
                        air.OVC_START_PORT = strOVC_PORT_CDE;
                        air.ONB_CARRIAGE_CURRENCY = strONB_CARRIAGE_CURRENCY;
                        air.ODT_START_DATE = dtOdtStartDate;
                        air.ODT_END_DATE = dtOdtEndDate;
                        air.OVC_IMPORT_EXPORT_1 = strOVC_IMPORT_EXPORT_1;
                        if (strONB_DISCOUNT_1.Equals(string.Empty)) air.ONB_DISCOUNT_1 = null; else air.ONB_DISCOUNT_1 = decONB_DISCOUNT_1;
                        if (strONB_M_RATE_1.Equals(string.Empty)) air.ONB_M_RATE_1 = null; else air.ONB_M_RATE_1 = decONB_M_RATE_1;
                        if (strONB_AS_N_RATE_1.Equals(string.Empty)) air.ONB_AS_N_RATE_1 = null; else air.ONB_AS_N_RATE_1 = decONB_AS_N_RATE_1;
                        if (strONB_N_RATE_1.Equals(string.Empty)) air.ONB_N_RATE_1 = null; else air.ONB_N_RATE_1 = decONB_N_RATE_1;
                        if (strONB_AS_45_KG_1.Equals(string.Empty)) air.ONB_AS_45_KG_1 = null; else air.ONB_AS_45_KG_1 = decONB_AS_45_KG_1;
                        if (strONB_PLUS_45_KG_1.Equals(string.Empty)) air.ONB_PLUS_45_KG_1 = null; else air.ONB_PLUS_45_KG_1 = decONB_PLUS_45_KG_1;
                        if (strONB_AS_100_KG_1.Equals(string.Empty)) air.ONB_AS_100_KG_1 = null; else air.ONB_AS_100_KG_1 = decONB_AS_100_KG_1;
                        if (strONB_PLUS_100_KG_1.Equals(string.Empty)) air.ONB_PLUS_100_KG_1 = null; else air.ONB_PLUS_100_KG_1 = decONB_PLUS_100_KG_1;
                        if (strONB_AS_300_KG_1.Equals(string.Empty)) air.ONB_AS_300_KG_1 = null; else air.ONB_AS_300_KG_1 = decONB_AS_300_KG_1;
                        if (strONB_PLUS_300_KG_1.Equals(string.Empty)) air.ONB_PLUS_300_KG_1 = null; else air.ONB_PLUS_300_KG_1 = decONB_PLUS_300_KG_1;
                        if (strONB_AS_500_KG_1.Equals(string.Empty)) air.ONB_AS_500_KG_1 = null; else air.ONB_AS_500_KG_1 = decONB_AS_500_KG_1;
                        if (strONB_PLUS_500_KG_1.Equals(string.Empty)) air.ONB_PLUS_500_KG_1 = null; else air.ONB_PLUS_500_KG_1 = decONB_PLUS_500_KG_1;
                        if (strONB_EUR_BLD_QUANTITY_1.Equals(string.Empty)) air.ONB_EUR_BLD_QUANTITY_1 = null; else air.ONB_EUR_BLD_QUANTITY_1 = decONB_EUR_BLD_QUANTITY_1;
                        if (strONB_EUR_DANGER_PRO_COST_1.Equals(string.Empty)) air.ONB_EUR_DANGER_PRO_COST_1 = null; else air.ONB_EUR_DANGER_PRO_COST_1 = decONB_EUR_DANGER_PRO_COST_1;
                        if (strONB_EUR_FIN_DANGER_PRO_COST_1.Equals(string.Empty)) air.ONB_EUR_FIN_DANGER_PRO_COST_1 = null; else air.ONB_EUR_FIN_DANGER_PRO_COST_1 = decONB_EUR_FIN_DANGER_PRO_COST_1;
                        if (strONB_USA_BLD_QUANTITY_1.Equals(string.Empty)) air.ONB_USA_BLD_QUANTITY_1 = null; else air.ONB_USA_BLD_QUANTITY_1 = decONB_USA_BLD_QUANTITY_1;
                        if (strONB_USA_DANGER_PRO_COST_1.Equals(string.Empty)) air.ONB_USA_DANGER_PRO_COST_1 = null; else air.ONB_USA_DANGER_PRO_COST_1 = decONB_USA_DANGER_PRO_COST_1;
                        if (strONB_USA_FIN_DANGER_PRO_COST_1.Equals(string.Empty)) air.ONB_USA_FIN_DANGER_PRO_COST_1 = null; else air.ONB_USA_FIN_DANGER_PRO_COST_1 = decONB_USA_FIN_DANGER_PRO_COST_1;
                        if (strOVC_IMPORT_EXPORT_2.Equals(string.Empty)) air.OVC_IMPORT_EXPORT_2 = null; else air.OVC_IMPORT_EXPORT_2 = strOVC_IMPORT_EXPORT_2;
                        air.OVC_IMPORT_EXPORT_2 = strOVC_IMPORT_EXPORT_2;
                        if (strONB_DISCOUNT_2.Equals(string.Empty)) air.ONB_DISCOUNT_2 = null; else air.ONB_DISCOUNT_2 = decONB_DISCOUNT_2;
                        if (strONB_M_RATE_2.Equals(string.Empty)) air.ONB_M_RATE_2 = null; else air.ONB_M_RATE_2 = decONB_M_RATE_2;
                        if (strONB_AS_N_RATE_2.Equals(string.Empty)) air.ONB_AS_N_RATE_2 = null; else air.ONB_AS_N_RATE_2 = decONB_AS_N_RATE_2;
                        if (strONB_N_RATE_2.Equals(string.Empty)) air.ONB_N_RATE_2 = null; else air.ONB_N_RATE_2 = decONB_N_RATE_2;
                        if (strONB_AS_45_KG_2.Equals(string.Empty)) air.ONB_AS_45_KG_2 = null; else air.ONB_AS_45_KG_2 = decONB_AS_45_KG_2;
                        if (strONB_PLUS_45_KG_2.Equals(string.Empty)) air.ONB_PLUS_45_KG_2 = null; else air.ONB_PLUS_45_KG_2 = decONB_PLUS_45_KG_2;
                        if (strONB_AS_100_KG_2.Equals(string.Empty)) air.ONB_AS_100_KG_2 = null; else air.ONB_AS_100_KG_2 = decONB_AS_100_KG_2;
                        if (strONB_PLUS_100_KG_2.Equals(string.Empty)) air.ONB_PLUS_100_KG_2 = null; else air.ONB_PLUS_100_KG_2 = decONB_PLUS_100_KG_2;
                        if (strONB_AS_300_KG_2.Equals(string.Empty)) air.ONB_AS_300_KG_2 = null; else air.ONB_AS_300_KG_2 = decONB_AS_300_KG_2;
                        if (strONB_PLUS_300_KG_2.Equals(string.Empty)) air.ONB_PLUS_300_KG_2 = null; else air.ONB_PLUS_300_KG_2 = decONB_PLUS_300_KG_2;
                        if (strONB_AS_500_KG_2.Equals(string.Empty)) air.ONB_AS_500_KG_2 = null; else air.ONB_AS_500_KG_2 = decONB_AS_500_KG_2;
                        if (strONB_PLUS_500_KG_2.Equals(string.Empty)) air.ONB_PLUS_500_KG_2 = null; else air.ONB_PLUS_500_KG_2 = decONB_PLUS_500_KG_2;
                        if (strONB_EUR_BLD_QUANTITY_2.Equals(string.Empty)) air.ONB_EUR_BLD_QUANTITY_2 = null; else air.ONB_EUR_BLD_QUANTITY_2 = decONB_EUR_BLD_QUANTITY_2;
                        if (strONB_EUR_DANGER_PRO_COST_2.Equals(string.Empty)) air.ONB_EUR_DANGER_PRO_COST_2 = null; else air.ONB_EUR_DANGER_PRO_COST_2 = decONB_EUR_DANGER_PRO_COST_2;
                        if (strONB_EUR_FIN_DANGER_PRO_COST_2.Equals(string.Empty)) air.ONB_EUR_FIN_DANGER_PRO_COST_2 = null; else air.ONB_EUR_FIN_DANGER_PRO_COST_2 = decONB_EUR_FIN_DANGER_PRO_COST_2;
                        if (strONB_USA_BLD_QUANTITY_2.Equals(string.Empty)) air.ONB_USA_BLD_QUANTITY_2 = null; else air.ONB_USA_BLD_QUANTITY_2 = decONB_USA_BLD_QUANTITY_2;
                        if (strONB_USA_DANGER_PRO_COST_2.Equals(string.Empty)) air.ONB_USA_DANGER_PRO_COST_2 = null; else air.ONB_USA_DANGER_PRO_COST_2 = decONB_USA_DANGER_PRO_COST_2;
                        if (strONB_USA_FIN_DANGER_PRO_COST_2.Equals(string.Empty)) air.ONB_USA_FIN_DANGER_PRO_COST_2 = null; else air.ONB_USA_FIN_DANGER_PRO_COST_2 = decONB_USA_FIN_DANGER_PRO_COST_2;
                        if (strOVC_IMPORT_EXPORT_2.Equals(string.Empty)) air.OVC_IMPORT_EXPORT_2 = null; else air.OVC_IMPORT_EXPORT_2 = strOVC_IMPORT_EXPORT_2;
                        air.OVC_IMPORT_EXPORT_3 = strOVC_IMPORT_EXPORT_3;
                        if (strONB_DISCOUNT_3.Equals(string.Empty)) air.ONB_DISCOUNT_3 = null; else air.ONB_DISCOUNT_3 = decONB_DISCOUNT_3;
                        if (strONB_M_RATE_3.Equals(string.Empty)) air.ONB_M_RATE_3 = null; else air.ONB_M_RATE_3 = decONB_M_RATE_3;
                        if (strONB_AS_N_RATE_3.Equals(string.Empty)) air.ONB_AS_N_RATE_3 = null; else air.ONB_AS_N_RATE_3 = decONB_AS_N_RATE_3;
                        if (strONB_N_RATE_3.Equals(string.Empty)) air.ONB_N_RATE_3 = null; else air.ONB_N_RATE_3 = decONB_N_RATE_3;
                        if (strONB_AS_45_KG_3.Equals(string.Empty)) air.ONB_AS_45_KG_3 = null; else air.ONB_AS_45_KG_3 = decONB_AS_45_KG_3;
                        if (strONB_PLUS_45_KG_3.Equals(string.Empty)) air.ONB_PLUS_45_KG_3 = null; else air.ONB_PLUS_45_KG_3 = decONB_PLUS_45_KG_3;
                        if (strONB_AS_100_KG_3.Equals(string.Empty)) air.ONB_AS_100_KG_3 = null; else air.ONB_AS_100_KG_3 = decONB_AS_100_KG_3;
                        if (strONB_PLUS_100_KG_3.Equals(string.Empty)) air.ONB_PLUS_100_KG_3 = null; else air.ONB_PLUS_100_KG_3 = decONB_PLUS_100_KG_3;
                        if (strONB_AS_300_KG_3.Equals(string.Empty)) air.ONB_AS_300_KG_3 = null; else air.ONB_AS_300_KG_3 = decONB_AS_300_KG_3;
                        if (strONB_PLUS_300_KG_3.Equals(string.Empty)) air.ONB_PLUS_300_KG_3 = null; else air.ONB_PLUS_300_KG_3 = decONB_PLUS_300_KG_3;
                        if (strONB_AS_500_KG_3.Equals(string.Empty)) air.ONB_AS_500_KG_3 = null; else air.ONB_AS_500_KG_3 = decONB_AS_500_KG_3;
                        if (strONB_PLUS_500_KG_3.Equals(string.Empty)) air.ONB_PLUS_500_KG_3 = null; else air.ONB_PLUS_500_KG_3 = decONB_PLUS_500_KG_3;
                        if (strOVC_IMPORT_EXPORT_3.Equals(string.Empty)) air.OVC_IMPORT_EXPORT_3 = null; else air.OVC_IMPORT_EXPORT_3 = strOVC_IMPORT_EXPORT_3;
                        air.OVC_REMARK = strOVC_REMARK;
                        MTS.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), air.GetType().Name.ToString(), this, "修改");
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                    }
                    catch
                    {
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查無運案資料");
                    }
                    #endregion
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            TBGMT_AIR_TRANSPORT air = MTS.TBGMT_AIR_TRANSPORT.Where(t => t.AT_SN.Equals(id)).FirstOrDefault();
            try
            {
                MTS.Entry(air).State = EntityState.Deleted;
                MTS.SaveChanges();
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除運費資料成功！");
            }
            catch
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查無運案資料");
            }
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F13_1{ getQueryString() }");
        }
    }
}