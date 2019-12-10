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
    public partial class MTS_F13_8 : System.Web.UI.Page
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
            var query = MTS.TBGMT_SEA_TRANSPORT.Where(t => t.ST_SN.Equals(id)).FirstOrDefault();
            var query_com = MTS.TBGMT_COMPANY.Where(t => t.CO_SN.Equals(query.CO_SN)).FirstOrDefault();
            var query_port = MTS.TBGMT_PORTS.Where(t => t.OVC_PORT_CDE.Equals(query.OVC_START_PORT)).FirstOrDefault();
            FCommon.list_setValue(drpOVC_SHIP_COMPANY, query_com.OVC_COMPANY);
            FCommon.list_setValue(drpONB_CARRIAGE_CURRENCY, query.ONB_CARRIAGE_CURRENCY);
            FCommon.list_setValue(drpOVC_ROUTE, query.OVC_ROUTE);
            txtOVC_PORT_CDE.Text = query.OVC_START_PORT;
            txtOVC_CHI_NAME.Text = query_port.OVC_PORT_CHI_NAME;
            txtOdtStartDate.Text = query.ODT_START_DATE == null ? "" : Convert.ToDateTime(query.ODT_START_DATE).ToString("yyyy-MM-dd");
            txtOdtEndDate.Text = query.ODT_END_DATE == null ? "" : Convert.ToDateTime(query.ODT_END_DATE).ToString("yyyy-MM-dd");
            if (query.OVC_IMPORT_EXPORT_1 == "進口") drpOVC_IMPORT_EXPORT_1.SelectedIndex = 0; else drpOVC_IMPORT_EXPORT_1.SelectedIndex = 1;
            TextboxImport(txtONB_DISCOUNT_1, query.ONB_DISCOUNT_1);
            txtOVC_ITEM_CATEGORY_1.Text = query.OVC_ITEM_CATEGORY_1;
            TextboxImport(txtONB_LOWEST_FREIGHT_1, query.ONB_LOWEST_FREIGHT_1);
            txtOVC_ITEM_CHI_NAME_1.Text = query.OVC_ITEM_CHI_NAME_1;
            txtOVC_ITEM_ENG_NAME_1.Text = query.OVC_ITEM_ENG_NAME_1;
            TextboxImport(txtONB_WEIGHT_PRICE_1, query.ONB_WEIGHT_PRICE_1);
            TextboxImport(txtONB_VOLUME_PRICE_1, query.ONB_VOLUME_PRICE_1);
            if (query.OVC_IMPORT_EXPORT_2 == "進口") drpOVC_IMPORT_EXPORT_2.SelectedIndex = 0; else drpOVC_IMPORT_EXPORT_2.SelectedIndex = 1;
            TextboxImport(txtONB_DISCOUNT_2, query.ONB_DISCOUNT_2);
            txtOVC_ITEM_CATEGORY_2.Text = query.OVC_ITEM_CATEGORY_2;
            TextboxImport(txtONB_LOWEST_FREIGHT_2, query.ONB_LOWEST_FREIGHT_2);
            txtOVC_ITEM_CHI_NAME_2.Text = query.OVC_ITEM_CHI_NAME_2;
            txtOVC_ITEM_ENG_NAME_2.Text = query.OVC_ITEM_ENG_NAME_2;
            TextboxImport(txtONB_WEIGHT_PRICE_2, query.ONB_WEIGHT_PRICE_2);
            TextboxImport(txtONB_VOLUME_PRICE_2, query.ONB_VOLUME_PRICE_2);
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
                        lblTITLE.Text = "海運運費資料維護-修改/刪除";
                        divNew.Visible = false;
                        divMod.Visible = true;
                        FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                        FCommon.Controls_Attributes("readonly", "true", txtOVC_CHI_NAME, txtOdtStartDate, txtOdtEndDate);

                        //承運航商
                        var query =
                            from company in MTS.TBGMT_COMPANY
                            where company.OVC_CO_TYPE.Equals("3")
                            where company.OVC_REMARK_2.Equals("海運")
                            select company;
                        DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                        FCommon.list_dataImport(drpOVC_SHIP_COMPANY, dt, "OVC_COMPANY", "OVC_COMPANY", "未輸入", "", false);
                        CommonMTS.list_dataImport_CURRENCY(drpONB_CARRIAGE_CURRENCY, false); //運費幣別
                        CommonMTS.list_dataImport_ROUTE(drpOVC_ROUTE, false); //航線

                        dataImport();
                    }
                }
                else
                {
                    if (!IsPostBack)
                    {
                        lblTITLE.Text = "海運運費資料維護-新增";
                        divNew.Visible = true;
                        divMod.Visible = false;
                        FCommon.Controls_Attributes("readonly", "true", txtOVC_CHI_NAME, txtOdtStartDate, txtOdtEndDate);

                        //承運航商
                        var query =
                            from company in MTS.TBGMT_COMPANY
                            where company.OVC_CO_TYPE.Equals("3")
                            where company.OVC_REMARK_2.Equals("海運")
                            select company;
                        DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                        FCommon.list_dataImport(drpOVC_SHIP_COMPANY, dt, "OVC_COMPANY", "OVC_COMPANY", "未輸入", "", false);
                        CommonMTS.list_dataImport_CURRENCY(drpONB_CARRIAGE_CURRENCY, false); //運費幣別
                        CommonMTS.list_dataImport_ROUTE(drpOVC_ROUTE, false); //航線
                    }
                }
                    //FCommon.MessageBoxShow(this, "編號錯誤！", $"MTS_F13_1{ getQueryString() }", false);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            #region 取值
            string strMessage = "";
            string strOVC_SHIP_COMPANY = drpOVC_SHIP_COMPANY.SelectedValue;//承運航商
            string strONB_CARRIAGE_CURRENCY = drpONB_CARRIAGE_CURRENCY.SelectedValue;//幣別
            string strOVC_PORT_CDE = txtOVC_PORT_CDE.Text;//啟運機場
            string strOVC_ROUTE = drpOVC_ROUTE.SelectedValue;//航線
            string strOdtStartDate = txtOdtStartDate.Text;//保險開始日
            string strOdtEndDate = txtOdtEndDate.Text;//保險結束日

            string strOVC_IMPORT_EXPORT_1 = drpOVC_IMPORT_EXPORT_1.SelectedItem.Text;
            string strONB_DISCOUNT_1 = txtONB_DISCOUNT_1.Text;
            string strOVC_ITEM_CATEGORY_1 = txtOVC_ITEM_CATEGORY_1.Text;
            string strONB_LOWEST_FREIGHT_1 = txtONB_LOWEST_FREIGHT_1.Text;
            string strOVC_ITEM_CHI_NAME_1 = txtOVC_ITEM_CHI_NAME_1.Text;
            string strOVC_ITEM_ENG_NAME_1 = txtOVC_ITEM_ENG_NAME_1.Text;
            string strONB_WEIGHT_PRICE_1 = txtONB_WEIGHT_PRICE_1.Text;
            string strONB_VOLUME_PRICE_1 = txtONB_VOLUME_PRICE_1.Text;

            string strOVC_IMPORT_EXPORT_2 = drpOVC_IMPORT_EXPORT_2.SelectedItem.Text;
            string strONB_DISCOUNT_2 = txtONB_DISCOUNT_2.Text;
            string strOVC_ITEM_CATEGORY_2 = txtOVC_ITEM_CATEGORY_2.Text;
            string strONB_LOWEST_FREIGHT_2 = txtONB_LOWEST_FREIGHT_2.Text;
            string strOVC_ITEM_CHI_NAME_2 = txtOVC_ITEM_CHI_NAME_2.Text;
            string strOVC_ITEM_ENG_NAME_2 = txtOVC_ITEM_ENG_NAME_2.Text;
            string strONB_WEIGHT_PRICE_2 = txtONB_WEIGHT_PRICE_2.Text;
            string strONB_VOLUME_PRICE_2 = txtONB_VOLUME_PRICE_2.Text;
            DateTime dtOdtStartDate, dtOdtEndDate;
            decimal decONB_DISCOUNT_1, decONB_DISCOUNT_2;
            decimal decONB_LOWEST_FREIGHT_1, decONB_WEIGHT_PRICE_1, decONB_VOLUME_PRICE_1;
            decimal decONB_LOWEST_FREIGHT_2, decONB_WEIGHT_PRICE_2, decONB_VOLUME_PRICE_2;
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
            FCommon.checkDecimal(strONB_DISCOUNT_2, "折扣數", ref strMessage, out decONB_DISCOUNT_2);
            FCommon.checkDecimal(strONB_LOWEST_FREIGHT_1, "最低運費", ref strMessage, out decONB_LOWEST_FREIGHT_1);
            FCommon.checkDecimal(strONB_WEIGHT_PRICE_1, "重量(W)價格", ref strMessage, out decONB_WEIGHT_PRICE_1);
            FCommon.checkDecimal(strONB_VOLUME_PRICE_1, "容積(M)價格", ref strMessage, out decONB_VOLUME_PRICE_1);
            FCommon.checkDecimal(strONB_LOWEST_FREIGHT_2, "最低運費", ref strMessage, out decONB_LOWEST_FREIGHT_2);
            FCommon.checkDecimal(strONB_WEIGHT_PRICE_2, "重量(W)價格", ref strMessage, out decONB_WEIGHT_PRICE_2);
            FCommon.checkDecimal(strONB_VOLUME_PRICE_2, "容積(M)價格", ref strMessage, out decONB_VOLUME_PRICE_2);
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                #region 合約日期重複判斷
                var query =
                    from tbgmt_sea in MTS.TBGMT_SEA_TRANSPORT.AsEnumerable()
                    join tbgmt_com in MTS.TBGMT_COMPANY on tbgmt_sea.CO_SN equals tbgmt_com.CO_SN
                    where tbgmt_com.OVC_COMPANY.Equals(strOVC_SHIP_COMPANY)
                    where tbgmt_sea.OVC_START_PORT.Equals(strOVC_PORT_CDE)
                    where tbgmt_sea.ST_SN != id
                    select tbgmt_sea;
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
                    TBGMT_SEA_TRANSPORT sea = MTS.TBGMT_SEA_TRANSPORT.Where(t => t.ST_SN.Equals(id)).FirstOrDefault();
                    try
                    {
                        sea.CO_SN = MTS.TBGMT_COMPANY.Where(t => t.OVC_COMPANY.Equals(strOVC_SHIP_COMPANY)).Select(t => t.CO_SN).FirstOrDefault();
                        sea.OVC_START_PORT = strOVC_PORT_CDE;
                        sea.ONB_CARRIAGE_CURRENCY = strONB_CARRIAGE_CURRENCY;
                        sea.OVC_ROUTE = strOVC_ROUTE;
                        sea.ODT_START_DATE = dtOdtStartDate;
                        sea.ODT_END_DATE = dtOdtEndDate;

                        sea.OVC_IMPORT_EXPORT_1 = strOVC_IMPORT_EXPORT_1;
                        if (strONB_DISCOUNT_1.Equals(string.Empty)) sea.ONB_DISCOUNT_1 = null; else sea.ONB_DISCOUNT_1 = decONB_DISCOUNT_1;
                        sea.OVC_ITEM_CATEGORY_1 = strOVC_ITEM_CATEGORY_1;
                        if (strONB_LOWEST_FREIGHT_1.Equals(string.Empty)) sea.ONB_LOWEST_FREIGHT_1 = null; else sea.ONB_LOWEST_FREIGHT_1 = decONB_LOWEST_FREIGHT_1;
                        sea.OVC_ITEM_CHI_NAME_1 = strOVC_ITEM_CHI_NAME_1;
                        sea.OVC_ITEM_ENG_NAME_1 = strOVC_ITEM_ENG_NAME_1;
                        if (strONB_WEIGHT_PRICE_1.Equals(string.Empty)) sea.ONB_WEIGHT_PRICE_1 = null; else sea.ONB_WEIGHT_PRICE_1 = decONB_WEIGHT_PRICE_1;
                        if (strONB_VOLUME_PRICE_1.Equals(string.Empty)) sea.ONB_VOLUME_PRICE_1 = null; else sea.ONB_VOLUME_PRICE_1 = decONB_VOLUME_PRICE_1;

                        sea.OVC_IMPORT_EXPORT_2 = strOVC_IMPORT_EXPORT_2;
                        if (strONB_DISCOUNT_2.Equals(string.Empty)) sea.ONB_DISCOUNT_2 = null; else sea.ONB_DISCOUNT_2 = decONB_DISCOUNT_2;
                        sea.OVC_ITEM_CATEGORY_2 = strOVC_ITEM_CATEGORY_2;
                        if (strONB_LOWEST_FREIGHT_2.Equals(string.Empty)) sea.ONB_LOWEST_FREIGHT_2 = null; else sea.ONB_LOWEST_FREIGHT_2 = decONB_LOWEST_FREIGHT_2;
                        sea.OVC_ITEM_CHI_NAME_2 = strOVC_ITEM_CHI_NAME_2;
                        sea.OVC_ITEM_ENG_NAME_2 = strOVC_ITEM_ENG_NAME_2;
                        if (strONB_WEIGHT_PRICE_2.Equals(string.Empty)) sea.ONB_WEIGHT_PRICE_2 = null; else sea.ONB_WEIGHT_PRICE_2 = decONB_WEIGHT_PRICE_2;
                        if (strONB_VOLUME_PRICE_2.Equals(string.Empty)) sea.ONB_VOLUME_PRICE_2 = null; else sea.ONB_VOLUME_PRICE_2 = decONB_VOLUME_PRICE_2;

                        MTS.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), sea.GetType().Name.ToString(), this, "修改");
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
            TBGMT_SEA_TRANSPORT sea = MTS.TBGMT_SEA_TRANSPORT.Where(t => t.ST_SN.Equals(id)).FirstOrDefault();
            try
            {
                MTS.Entry(sea).State = EntityState.Deleted;
                MTS.SaveChanges();
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除運費資料成功！");
            }
            catch
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查無運案資料");
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            #region 取值
            string strMessage = "";
            string strOVC_SHIP_COMPANY = drpOVC_SHIP_COMPANY.SelectedValue;//承運航商
            string strONB_CARRIAGE_CURRENCY = drpONB_CARRIAGE_CURRENCY.SelectedValue;//幣別
            string strOVC_PORT_CDE = txtOVC_PORT_CDE.Text;//啟運機場
            string strOVC_ROUTE = drpOVC_ROUTE.SelectedValue;//航線
            string strOdtStartDate = txtOdtStartDate.Text;//保險開始日
            string strOdtEndDate = txtOdtEndDate.Text;//保險結束日

            string strOVC_IMPORT_EXPORT_1 = drpOVC_IMPORT_EXPORT_1.SelectedItem.Text;
            string strONB_DISCOUNT_1 = txtONB_DISCOUNT_1.Text;
            string strOVC_ITEM_CATEGORY_1 = txtOVC_ITEM_CATEGORY_1.Text;
            string strONB_LOWEST_FREIGHT_1 = txtONB_LOWEST_FREIGHT_1.Text;
            string strOVC_ITEM_CHI_NAME_1 = txtOVC_ITEM_CHI_NAME_1.Text;
            string strOVC_ITEM_ENG_NAME_1 = txtOVC_ITEM_ENG_NAME_1.Text;
            string strONB_WEIGHT_PRICE_1 = txtONB_WEIGHT_PRICE_1.Text;
            string strONB_VOLUME_PRICE_1 = txtONB_VOLUME_PRICE_1.Text;

            string strOVC_IMPORT_EXPORT_2 = drpOVC_IMPORT_EXPORT_2.SelectedItem.Text;
            string strONB_DISCOUNT_2 = txtONB_DISCOUNT_2.Text;
            string strOVC_ITEM_CATEGORY_2 = txtOVC_ITEM_CATEGORY_2.Text;
            string strONB_LOWEST_FREIGHT_2 = txtONB_LOWEST_FREIGHT_2.Text;
            string strOVC_ITEM_CHI_NAME_2 = txtOVC_ITEM_CHI_NAME_2.Text;
            string strOVC_ITEM_ENG_NAME_2 = txtOVC_ITEM_ENG_NAME_2.Text;
            string strONB_WEIGHT_PRICE_2 = txtONB_WEIGHT_PRICE_2.Text;
            string strONB_VOLUME_PRICE_2 = txtONB_VOLUME_PRICE_2.Text;
            DateTime dtOdtStartDate, dtOdtEndDate;
            decimal decONB_DISCOUNT_1, decONB_DISCOUNT_2;
            decimal decONB_LOWEST_FREIGHT_1, decONB_WEIGHT_PRICE_1, decONB_VOLUME_PRICE_1;
            decimal decONB_LOWEST_FREIGHT_2, decONB_WEIGHT_PRICE_2, decONB_VOLUME_PRICE_2;
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
            FCommon.checkDecimal(strONB_DISCOUNT_2, "折扣數", ref strMessage, out decONB_DISCOUNT_2);
            FCommon.checkDecimal(strONB_LOWEST_FREIGHT_1, "最低運費", ref strMessage, out decONB_LOWEST_FREIGHT_1);
            FCommon.checkDecimal(strONB_WEIGHT_PRICE_1, "重量(W)價格", ref strMessage, out decONB_WEIGHT_PRICE_1);
            FCommon.checkDecimal(strONB_VOLUME_PRICE_1, "容積(M)價格", ref strMessage, out decONB_VOLUME_PRICE_1);
            FCommon.checkDecimal(strONB_LOWEST_FREIGHT_2, "最低運費", ref strMessage, out decONB_LOWEST_FREIGHT_2);
            FCommon.checkDecimal(strONB_WEIGHT_PRICE_2, "重量(W)價格", ref strMessage, out decONB_WEIGHT_PRICE_2);
            FCommon.checkDecimal(strONB_VOLUME_PRICE_2, "容積(M)價格", ref strMessage, out decONB_VOLUME_PRICE_2);
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                #region 合約日期重複判斷
                var query =
                    from tbgmt_sea in MTS.TBGMT_SEA_TRANSPORT.AsEnumerable()
                    join tbgmt_com in MTS.TBGMT_COMPANY on tbgmt_sea.CO_SN equals tbgmt_com.CO_SN
                    where tbgmt_com.OVC_COMPANY.Equals(strOVC_SHIP_COMPANY)
                    where tbgmt_sea.OVC_START_PORT.Equals(strOVC_PORT_CDE)
                    where tbgmt_sea.ST_SN != id
                    select tbgmt_sea;
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
                    TBGMT_SEA_TRANSPORT sea = new TBGMT_SEA_TRANSPORT();
                    try
                    {
                        sea.ST_SN = Guid.NewGuid();
                        sea.CO_SN = MTS.TBGMT_COMPANY.Where(t => t.OVC_COMPANY.Equals(strOVC_SHIP_COMPANY)).Select(t => t.CO_SN).FirstOrDefault();
                        sea.OVC_START_PORT = strOVC_PORT_CDE;
                        sea.ONB_CARRIAGE_CURRENCY = strONB_CARRIAGE_CURRENCY;
                        sea.OVC_ROUTE = strOVC_ROUTE;
                        sea.ODT_START_DATE = dtOdtStartDate;
                        sea.ODT_END_DATE = dtOdtEndDate;

                        sea.OVC_IMPORT_EXPORT_1 = strOVC_IMPORT_EXPORT_1;
                        if (strONB_DISCOUNT_1.Equals(string.Empty)) sea.ONB_DISCOUNT_1 = null; else sea.ONB_DISCOUNT_1 = decONB_DISCOUNT_1;
                        sea.OVC_ITEM_CATEGORY_1 = strOVC_ITEM_CATEGORY_1;
                        if (strONB_LOWEST_FREIGHT_1.Equals(string.Empty)) sea.ONB_LOWEST_FREIGHT_1 = null; else sea.ONB_LOWEST_FREIGHT_1 = decONB_LOWEST_FREIGHT_1;
                        sea.OVC_ITEM_CHI_NAME_1 = strOVC_ITEM_CHI_NAME_1;
                        sea.OVC_ITEM_ENG_NAME_1 = strOVC_ITEM_ENG_NAME_1;
                        if (strONB_WEIGHT_PRICE_1.Equals(string.Empty)) sea.ONB_WEIGHT_PRICE_1 = null; else sea.ONB_WEIGHT_PRICE_1 = decONB_WEIGHT_PRICE_1;
                        if (strONB_VOLUME_PRICE_1.Equals(string.Empty)) sea.ONB_VOLUME_PRICE_1 = null; else sea.ONB_VOLUME_PRICE_1 = decONB_VOLUME_PRICE_1;

                        sea.OVC_IMPORT_EXPORT_2 = strOVC_IMPORT_EXPORT_2;
                        if (strONB_DISCOUNT_2.Equals(string.Empty)) sea.ONB_DISCOUNT_2 = null; else sea.ONB_DISCOUNT_2 = decONB_DISCOUNT_2;
                        sea.OVC_ITEM_CATEGORY_2 = strOVC_ITEM_CATEGORY_2;
                        if (strONB_LOWEST_FREIGHT_2.Equals(string.Empty)) sea.ONB_LOWEST_FREIGHT_2 = null; else sea.ONB_LOWEST_FREIGHT_2 = decONB_LOWEST_FREIGHT_2;
                        sea.OVC_ITEM_CHI_NAME_2 = strOVC_ITEM_CHI_NAME_2;
                        sea.OVC_ITEM_ENG_NAME_2 = strOVC_ITEM_ENG_NAME_2;
                        if (strONB_WEIGHT_PRICE_2.Equals(string.Empty)) sea.ONB_WEIGHT_PRICE_2 = null; else sea.ONB_WEIGHT_PRICE_2 = decONB_WEIGHT_PRICE_2;
                        if (strONB_VOLUME_PRICE_2.Equals(string.Empty)) sea.ONB_VOLUME_PRICE_2 = null; else sea.ONB_VOLUME_PRICE_2 = decONB_VOLUME_PRICE_2;

                        MTS.TBGMT_SEA_TRANSPORT.Add(sea);
                        MTS.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), sea.GetType().Name.ToString(), this, "新增");
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增運費資料成功！");
                    }
                    catch
                    {
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "新增運費資料失敗！");
                    }
                    #endregion
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F13_1{ getQueryString() }");
        }
    }
}