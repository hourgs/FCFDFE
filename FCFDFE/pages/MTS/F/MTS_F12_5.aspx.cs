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
    public partial class MTS_F12_5 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTS = new MTSEntities();
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

            if (strOvcCompany.Equals(string.Empty))
                strMessage += "<p> 請輸入 保險公司！ </p>";
            if (strOnbCoSort.Equals(string.Empty))
                strMessage += "<p> 請輸入 排序！ </p>";
            decimal decOnbCoSort;
            bool boolOnbCoSort = FCommon.checkDecimal(strOnbCoSort, "排序", ref strMessage, out decOnbCoSort);


            var query2 =
                from tbgmt_company in MTS.TBGMT_COMPANY
                where tbgmt_company.OVC_CO_TYPE.Equals( "1")
            select new
                {
                    ONB_CO_SORT = tbgmt_company.ONB_CO_SORT,
                    OVC_COMPANY = tbgmt_company.OVC_COMPANY,
                };

            DataTable dt = new DataTable();
            if (boolOnbCoSort)
            {
                query2 = query2.Where(ot => ot.ONB_CO_SORT == decOnbCoSort);
                dt = CommonStatic.LinqQueryToDataTable(query2);
                if (dt.Rows.Count > 0)
                {
                    strMessage += "<p> 保險公司排序不可重複 </p>";
                }
            }

            if (strMessage.Equals(string.Empty))
            {
                try
                {
                    TBGMT_COMPANY company = new TBGMT_COMPANY();
                    company.CO_SN = Guid.NewGuid();
                    company.OVC_COMPANY = strOvcCompany;
                    company.OVC_CO_TYPE = "1";
                    company.ONB_CO_SORT = decOnbCoSort;
                    MTS.TBGMT_COMPANY.Add(company);
                    MTS.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), company.GetType().Name.ToString(), this, "新增保險公司資料成功");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "更新成功");
                }
                catch (Exception ex)
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                }

            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);

            //DataTable dt2 = new DataTable();
            //if (txtOvcCompany.Text == string.Empty || txtOnbCoSort.Text == string.Empty)
            //{
            //    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請確認填寫每個欄位!");
            //}
            //else
            //{
            //    var query2 =
            //    from tbgmt_company in MTS.TBGMT_COMPANY
            //    select new
            //    {
            //        ONB_CO_SORT = tbgmt_company.ONB_CO_SORT,
            //        OVC_COMPANY = tbgmt_company.OVC_COMPANY,
            //    };
            //    decimal temp2 = Convert.ToDecimal(txtOnbCoSort.Text);
            //    string companyname = txtOvcCompany.Text;
            //    query2 = query2.Where(ot => ot.ONB_CO_SORT == temp2);
            //    query2 = query2.Where(ot => ot.OVC_COMPANY == companyname);
            //    dt2 = CommonStatic.LinqQueryToDataTable(query2);

            //    if (dt2.Rows.Count > 0)
            //    {
            //        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "保險公司排序不可重複!");
            //    }
            //    else
            //    {
            //        try
            //        {
            //            TBGMT_COMPANY company = new TBGMT_COMPANY();
            //            company.CO_SN = Guid.NewGuid();
            //            company.OVC_COMPANY = txtOvcCompany.Text;
            //            company.OVC_CO_TYPE = "1";
            //            company.ONB_CO_SORT = Convert.ToDecimal(txtOnbCoSort.Text);
            //            MTS.TBGMT_COMPANY.Add(company);
            //            MTS.SaveChanges();
            //            FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增保險公司資料成功!");
            //        }
            //        catch (Exception ex)
            //        {
            //            FCommon.AlertShow(PnMessage, "danger", "系統訊息", ex.ToString());
            //        }
            //    }
            //}
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
            }
        }
    }
}