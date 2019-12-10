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
    public partial class MTS_F13_5 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        private MTSEntities MTS = new MTSEntities();
        TBGMT_COMPANY company = new TBGMT_COMPANY();
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
                    FCommon.Controls_Attributes("readonly", "true", txtOdtStartDate, txtOdtEndDate);
                    FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strOvcCompany = txtOvcCompany.Text;
            string strOvcCoType = radOvcCoType.SelectedValue;
            string strSD = txtOdtStartDate.Text;
            string strED = txtOdtEndDate.Text;
            string strSort = txtOnbIrSort.Text;


            Decimal Sort;
            DateTime SD, ED;
            bool boolSD = FCommon.checkDateTime(strSD, "開始日期", ref strMessage, out SD);
            bool boolED = FCommon.checkDateTime(strED, "結束日期", ref strMessage, out ED);
            bool boolSort = FCommon.checkDecimal(strSort, "結束日期", ref strMessage, out Sort);

            int if_Cn = MTS.TBGMT_COMPANY.Where(table => table.OVC_COMPANY.Equals(strOvcCompany) && table.OVC_CO_TYPE.Equals(strOvcCoType)).Count();
            int if_Sort = MTS.TBGMT_COMPANY.Where(table => table.ONB_CO_SORT == Sort&&table.OVC_CO_TYPE.Equals(strOvcCoType)).Count();

            if (strOvcCompany.Equals(string.Empty)) strMessage += "<p>請輸入 公司名稱！</p>";
            if (strOvcCoType.Equals(string.Empty)) strMessage += "<p>請選擇 公司/廠商種類！</p>";
            if (strSD.Equals(string.Empty)) strMessage += "<p>請輸入 開始日期！</p>";
            if (strED.Equals(string.Empty)) strMessage += "<p>請輸入 結束日期！</p>";
            if (strSort.Equals(string.Empty)) strMessage += "<p>請輸入 排序！</p>";
            if (SD > ED) strMessage += "<p>開始日期 需早於 結束日期！</p>";
            if (Sort <= 0) strMessage += "<p>排序 須為正數！</p>";
            if (if_Sort > 0) strMessage += "<p>排序 已重複！</p>";
            if (if_Cn > 0) strMessage += "<p>名稱 已重複！</p>";

            if (strMessage.Equals(string.Empty))
            {
                if (company != null)
                {
                    company.CO_SN = Guid.NewGuid();
                    company.OVC_COMPANY = strOvcCompany;
                    company.OVC_CO_TYPE = strOvcCoType;
                    company.ODT_START_DATE = SD;
                    company.ODT_END_DATE = ED;
                    company.ONB_CO_SORT = Sort;
                    company.ODT_CREATE_DATE = DateTime.Now;
                    company.OVC_CREATE_ID = Session["userid"].ToString();

                    MTS.TBGMT_COMPANY.Add(company);
                    MTS.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), company.GetType().Name.ToString(), this, "新增");
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
    }
}