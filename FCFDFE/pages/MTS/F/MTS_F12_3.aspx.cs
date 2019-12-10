using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F12_3 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTS = new MTSEntities();
        string  strOVC_COMPANY = "";

        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_CO_TYPE", Request.QueryString["OVC_CO_TYPE"], false);
            return strQueryString;
            //在接收頁面加入此副程式
        }
        #endregion 
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strOvcCompany = txtOvcCompany.Text;
            string strOnbCoSort = txtOnbCoSort.Text;
            string strOVC_CREATE_ID = Session["username"].ToString();

            if (strOvcCompany.Equals(string.Empty))
                strMessage += "<p> 請輸入 進艙廠商！ </p>";
            if (strOnbCoSort.Equals(string.Empty))
                strMessage += "<p> 請輸入 排序！ </p>";
            decimal decOnbCoSort;
            bool boolOnbCoSort = FCommon.checkDecimal(strOnbCoSort, "排序", ref strMessage, out decOnbCoSort);
            #region 保險公司/進銷廠商查詢
            var c_query =
                from tbgmt_company in MTS.TBGMT_COMPANY
                where tbgmt_company.OVC_CO_TYPE.Equals("1")
                select new
                {
                    ONB_CO_SORT = tbgmt_company.ONB_CO_SORT,
                    OVC_COMPANY=tbgmt_company.OVC_COMPANY,
                };
            var p_query =
                from tbgmt_company in MTS.TBGMT_COMPANY
                where tbgmt_company.OVC_CO_TYPE.Equals("2")
                select new
                {
                    ONB_CO_SORT = tbgmt_company.ONB_CO_SORT,
                    OVC_COMPANY = tbgmt_company.OVC_COMPANY,
                };
            #endregion
            if (boolOnbCoSort)
            {
                #region 排序、名稱查詢重複
                var cq_name = c_query.Where(ot => ot.OVC_COMPANY.Equals(strOvcCompany));
                var cq_sort = c_query.Where(ot => ot.ONB_CO_SORT == decOnbCoSort);
                var pq_name = p_query.Where(ot => ot.OVC_COMPANY.Equals(strOvcCompany));
                var pq_sort = p_query.Where(ot => ot.ONB_CO_SORT == decOnbCoSort);
                DataTable dtcn = CommonStatic.LinqQueryToDataTable(cq_name);
                DataTable dtcs = CommonStatic.LinqQueryToDataTable(cq_sort);
                DataTable dtpn = CommonStatic.LinqQueryToDataTable(pq_name);
                DataTable dtps = CommonStatic.LinqQueryToDataTable(pq_sort);
                bool boolcn = dtcn.Rows.Count > 0;
                bool boolcs = dtcs.Rows.Count > 0;
                bool boolpn = dtpn.Rows.Count > 0;
                bool boolps = dtps.Rows.Count > 0;
                switch (strOVC_COMPANY)
                {
                    case "進艙廠商":
                        if (boolpn) strMessage += "<p> 廠商名稱不可重複 </p>";
                        if (boolps) strMessage += "<p> 廠商種類排序不可重複 </p>";
                        break;
                    case "保險公司":
                        if (boolcn) strMessage += "<p> 保險公司名稱不可重複 </p>";
                        if (boolcs) strMessage += "<p> 保險公司排序不可重複 </p>";
                        break;
                }
                #endregion
            }

            if (strMessage.Equals(string.Empty))
            {
                try
                {
                    if (strOVC_COMPANY.Equals("進艙廠商"))
                    {
                        TBGMT_COMPANY company = new TBGMT_COMPANY();
                        company.CO_SN = Guid.NewGuid();
                        company.OVC_COMPANY = strOvcCompany;
                        company.ODT_CREATE_DATE = DateTime.Now;
                        company.OVC_CREATE_ID = strOVC_CREATE_ID;
                        company.OVC_CO_TYPE = "2";
                        company.ONB_CO_SORT = decOnbCoSort;
                        MTS.TBGMT_COMPANY.Add(company);
                        MTS.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), company.GetType().Name.ToString(), this, "新增進艙廠商資料成功");
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "更新成功");
                    }
                    else
                    {
                        TBGMT_COMPANY company = new TBGMT_COMPANY();
                        company.CO_SN = Guid.NewGuid();
                        company.OVC_COMPANY = strOvcCompany;
                        company.ODT_CREATE_DATE = DateTime.Now;
                        company.OVC_CREATE_ID = strOVC_CREATE_ID;
                        company.OVC_CO_TYPE = "1";
                        company.ONB_CO_SORT = decOnbCoSort;
                        MTS.TBGMT_COMPANY.Add(company);
                        MTS.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), company.GetType().Name.ToString(), this, "新增進艙廠商資料成功");
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "更新成功");
                    }
                }
                catch (Exception ex)
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                }
                
            }
            else
            FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F12_1{ getQueryString() }");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                //判斷是進艙廠商還是保險公司
                if (FCommon.getQueryString(this, "type", out string strOVC_CO_TYPE, true))
                {
                    switch (strOVC_CO_TYPE)
                    {
                        case "1":
                            strOVC_COMPANY = "保險公司";
                            break;
                        case "2":
                            strOVC_COMPANY = "進艙廠商";
                            break;
                    }
                }
                if (!IsPostBack)
                {
                    lblOVC_COMPANY_title.Text = strOVC_COMPANY;
                    lblOVC_COMPANY.Text = strOVC_COMPANY;
                }
            }
        }
    }
}