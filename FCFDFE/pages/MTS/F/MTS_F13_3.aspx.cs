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
    public partial class MTS_F13_3 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        private MTSEntities MTS = new MTSEntities();
        TBGMT_INSRATE insrate = new TBGMT_INSRATE();
        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "ODT_START_DATE", Request.QueryString["ODT_START_DATE"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_END_DATE", Request.QueryString["ODT_END_DATE"], false);
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
                    FCommon.Controls_Attributes("readonly", "true", txtOdtStartDate, txtOdtEndDate);
                    var query =
                        from dbcompany in MTS.TBGMT_COMPANY
                        where dbcompany.OVC_CO_TYPE.Equals("1")
                        select new
                        {
                            dbcompany.CO_SN,
                            OVC_COMPANY = dbcompany.OVC_COMPANY,
                        };
                    DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                    FCommon.list_dataImport(drpOvcCompany, dt, "OVC_COMPANY", "OVC_COMPANY", true);
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strINS_NAME = "", strMessage = "";
            string strOvcCompany = drpOvcCompany.SelectedValue;
            //string strOvcInsType = txtOvcInsType.Text;
            //string strOvcInsRate = txtOvcInsRate.Text;
            string strOdtStartdate = txtOdtStartDate.Text;
            string strOdtEnddate = txtOdtEndDate.Text;
            string strOnbIrSort = txtOnbIrSort.Text;
            string strFULL_INSURANCE = txtFULL_INSURANCE.Text;
            string strINSIDE_LAND_INSURANCE = txtINSIDE_LAND_INSURANCE.Text;
            string strOUTSIDE_LAND_INSURANCE = txtOUTSIDE_LAND_INSURANCE.Text;
            string strMILITARY_INSURANCE = txtMILITARY_INSURANCE.Text;
            DateTime Start, End;
            decimal DecRate, Sort, Rate, decFULL_INSURANCE = 0, decINSIDE_LAND_INSURANCE = 0, decOUTSIDE_LAND_INSURANCE = 0, decMILITARY_INSURANCE = 0;
            FCommon.checkDateTime(strOdtStartdate, "保險開始日", ref strMessage, out Start);
            FCommon.checkDateTime(strOdtEnddate, "保險結束日", ref strMessage, out End);
            FCommon.checkDecimal(strOnbIrSort, "排序", ref strMessage, out Sort);
            //FCommon.checkDecimal(strOvcInsRate, "保險費率%", ref strMessage, out Rate);
            int if_exist_sort = MTS.TBGMT_INSRATE.Where(table => table.ONB_SORT == Sort).Count();
            int if_Cn = MTS.TBGMT_INSRATE.Where(table => table.OVC_INSCOMPNAY.Equals(strOvcCompany)).Count();
            //int if_IT = MTS.TBGMT_INSRATE.Where(table => table.OVC_INS_NAME.Equals(strOvcInsType)).Count();
            bool boolFULL_INSURANCE = strFULL_INSURANCE.Equals(string.Empty) ? true : FCommon.checkDecimal(strFULL_INSURANCE, "全險", ref strMessage, out decFULL_INSURANCE);
            bool boolINSIDE_LAND_INSURANCE = strINSIDE_LAND_INSURANCE.Equals(string.Empty) ? true : FCommon.checkDecimal(strINSIDE_LAND_INSURANCE, "在台內陸險", ref strMessage, out decINSIDE_LAND_INSURANCE);
            bool boolOUTSIDE_LAND_INSURANCE = strOUTSIDE_LAND_INSURANCE.Equals(string.Empty) ? true : FCommon.checkDecimal(strOUTSIDE_LAND_INSURANCE, "在外內陸險", ref strMessage, out decOUTSIDE_LAND_INSURANCE);
            bool boolMILITARY_INSURANCE = strMILITARY_INSURANCE.Equals(string.Empty) ? true : FCommon.checkDecimal(strMILITARY_INSURANCE, "兵險及罷工險", ref strMessage, out decMILITARY_INSURANCE);
            DecRate = decFULL_INSURANCE + decINSIDE_LAND_INSURANCE + decOUTSIDE_LAND_INSURANCE + decMILITARY_INSURANCE; //費率加總

            if (strOvcCompany.Equals(string.Empty)) strMessage += "<p>請輸入 保險公司！</p>";
            //if (strOvcInsType.Equals(string.Empty)) strMessage += "<p>請輸入 保險費種類！</p>";
            //if (strOvcInsRate.Equals(string.Empty)) strMessage += "<p>請輸入 保險費率%！</p>";
            if (strOdtStartdate.Equals(string.Empty)) strMessage += "<p>請輸入 保險開始日！</p>";
            if (strOdtEnddate.Equals(string.Empty)) strMessage += "<p>請輸入 保險結束日！</p>";
            if (strOnbIrSort.Equals(string.Empty)) strMessage += "<p>請輸入 排序！</p>";
            if (Start > End) strMessage += "<p>保險開始日 需早於 保險結束日！</p>";
            if (Sort <= 0) strMessage += "<p>排序 須為正數！</p>";
            //if (Rate <= 0) strMessage += "<p>保險費率% 需大於0！</p>";
            if (if_exist_sort > 0) strMessage += "<p>排序 已重複！</p>";
            //if (if_Cn > 0) strMessage += "<p>保險公司名稱 已重複！</p>";
            //if (if_IT > 0) strMessage += "<p>保險費種類 已重複！</p>";
            if (strFULL_INSURANCE.Equals(string.Empty) && strINSIDE_LAND_INSURANCE.Equals(string.Empty) && strOUTSIDE_LAND_INSURANCE.Equals(string.Empty) && strMILITARY_INSURANCE.Equals(string.Empty)
                && chkFULL_INSURANCE.Checked == false && chkINSIDE_LAND_INSURANCE.Checked == false && chkOUTSIDE_LAND_INSURANCE.Checked == false && chkMILITARY_INSURANCE.Checked == false)
            {
                strMessage += "<p>請輸入 保險種類！</p>";
            }
            if (decFULL_INSURANCE < 0 || decINSIDE_LAND_INSURANCE < 0 || decOUTSIDE_LAND_INSURANCE < 0 || decMILITARY_INSURANCE < 0)
            {
                strMessage += "<p>保險費率百分比請輸入正數！</p>";
            }
            var query_sort = strMessage == "" ? MTS.TBGMT_INSRATE
                .Where(t => t.ONB_SORT != null ? t.ONB_SORT == Sort : false).FirstOrDefault() : null;
            if (query_sort != null)
            {
                strMessage += "<p>排序不可重複！</p>";
            }
            var query_start_day = strMessage == "" ? MTS.TBGMT_INSRATE.AsEnumerable()
                //.Where(t => t.OVC_INSCOMPNAY.Equals(strOvcCompany))
                .Where(t => DateTime.Compare(Convert.ToDateTime(t.ODT_START_DATE), Start) <= 0)
                .Where(t => DateTime.Compare(Convert.ToDateTime(t.ODT_END_DATE), Start) >= 0).FirstOrDefault() : null;
            var query_end_day = strMessage == "" ? MTS.TBGMT_INSRATE.AsEnumerable()
                //.Where(t => t.OVC_INSCOMPNAY.Equals(strOvcCompany))
                .Where(t => DateTime.Compare(Convert.ToDateTime(t.ODT_START_DATE), End) <= 0)
                .Where(t => DateTime.Compare(Convert.ToDateTime(t.ODT_END_DATE), End) >= 0).FirstOrDefault() : null;
            if (query_start_day != null || query_end_day != null)
            {
                strMessage += "<p>保險公司的保險期間不能重疊</p>";
            }
            if (chkFULL_INSURANCE.Checked == true)
            {
                strINS_NAME += strINS_NAME.Equals(string.Empty) ? "全險" : "、全險";
            }
            if (chkINSIDE_LAND_INSURANCE.Checked == true)
            {
                strINS_NAME += strINS_NAME.Equals(string.Empty) ? "在台內陸險" : "、在台內陸險";
            }
            if (chkOUTSIDE_LAND_INSURANCE.Checked == true)
            {
                strINS_NAME += strINS_NAME.Equals(string.Empty) ? "在外內陸險" : "、在外內陸險";
            }
            if (chkMILITARY_INSURANCE.Checked == true)
            {
                strINS_NAME += strINS_NAME.Equals(string.Empty) ? "兵險及罷工險" : "、兵險及罷工險";
            }

            var query_company = MTS.TBGMT_COMPANY.Where(t => t.OVC_COMPANY.Equals(strOvcCompany)).FirstOrDefault();
            if (strMessage.Equals(string.Empty))
            {
                if (insrate != null)
                {
                    insrate.OVC_INS_NAME = strINS_NAME;
                    insrate.INSRATE_SN = Guid.NewGuid();
                    insrate.OVC_INSCOMPNAY = strOvcCompany;
                    insrate.OVC_INS_RATE = DecRate;
                    insrate.ODT_START_DATE = Start;
                    insrate.ODT_END_DATE = End;
                    insrate.ONB_SORT = Sort;
                    insrate.ODT_CREATE_DATE = DateTime.Now;
                    insrate.OVC_CREATE_ID = Session["userid"].ToString();
                    insrate.OVC_CO_SN = query_company.CO_SN;
                    insrate.OVC_INS_NAME_1 = chkFULL_INSURANCE.Checked == true ? "Y" : "N";
                    insrate.OVC_INS_NAME_2 = chkINSIDE_LAND_INSURANCE.Checked == true ? "Y" : "N";
                    insrate.OVC_INS_NAME_3 = chkOUTSIDE_LAND_INSURANCE.Checked == true ? "Y" : "N";
                    insrate.OVC_INS_NAME_4 = chkMILITARY_INSURANCE.Checked == true ? "Y" : "N";
                    insrate.OVC_INS_RATE_1 = decFULL_INSURANCE;
                    insrate.OVC_INS_RATE_2 = decINSIDE_LAND_INSURANCE;
                    insrate.OVC_INS_RATE_3 = decOUTSIDE_LAND_INSURANCE;
                    insrate.OVC_INS_RATE_4 = decMILITARY_INSURANCE;
                    MTS.TBGMT_INSRATE.Add(insrate);
                    MTS.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), insrate.GetType().Name.ToString(), this, "新增");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功");
                }
                else FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
            else FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F13_1{ getQueryString() }");
        }
        #region 保險費率
        protected void INSURANCE_CheckedChanged(object sender, EventArgs e)
        {
            Insurance();
        }

        private void Insurance()
        {
            decimal dec, decEX_WORK = 0, decFBO_FCA_CPT_CFR = 0, decOTHER = 0;
            if (decimal.TryParse(txtFULL_INSURANCE.Text, out dec))
            {
                decEX_WORK = decEX_WORK + dec;
                decFBO_FCA_CPT_CFR = decFBO_FCA_CPT_CFR + dec;
                if (chkFULL_INSURANCE.Checked == true)
                    decOTHER = decOTHER + dec;
            }
            if (decimal.TryParse(txtINSIDE_LAND_INSURANCE.Text, out dec))
            {
                decEX_WORK = decEX_WORK + dec;
                decFBO_FCA_CPT_CFR = decFBO_FCA_CPT_CFR + dec;
                if (chkINSIDE_LAND_INSURANCE.Checked == true)
                    decOTHER = decOTHER + dec;
            }
            if (decimal.TryParse(txtOUTSIDE_LAND_INSURANCE.Text, out dec))
            {
                decEX_WORK = decEX_WORK + dec;
                if (chkOUTSIDE_LAND_INSURANCE.Checked == true)
                    decOTHER = decOTHER + dec;
            }
            if (decimal.TryParse(txtMILITARY_INSURANCE.Text, out dec))
            {
                decEX_WORK = decEX_WORK + dec;
                decFBO_FCA_CPT_CFR = decFBO_FCA_CPT_CFR + dec;
                if (chkMILITARY_INSURANCE.Checked == true)
                    decOTHER = decOTHER + dec;
            }
            txtEX_WORK.Text = decEX_WORK.ToString();
            txtFBO_FCA_CPT_CFR.Text = decFBO_FCA_CPT_CFR.ToString();
            txtOTHER.Text = decOTHER.ToString();
        }
        #endregion
    }
}