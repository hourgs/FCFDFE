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
    public partial class MTS_F12_4 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTS = new MTSEntities();
        TBGMT_COMPANY company = new TBGMT_COMPANY();
        Guid id;
        string sort;
        string type;

        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_CO_TYPE", Request.QueryString["OVC_CO_TYPE"], false);
            return strQueryString;
            //在接收頁面加入此副程式
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
                        FCommon.Controls_Attributes("readonly", "true", txtOdtStartDate, txtOdtEndDate);
                        var query =
                            from tbgmt_company in MTS.TBGMT_COMPANY
                            where tbgmt_company.CO_SN == id
                            select new
                            {
                                tbgmt_company.OVC_COMPANY,
                                tbgmt_company.ONB_CO_SORT,
                                tbgmt_company.ODT_START_DATE,
                                tbgmt_company.ODT_END_DATE,
                                tbgmt_company.OVC_CO_TYPE,
                            };
                        DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            string strONB_CO_SORT = dr["ONB_CO_SORT"].ToString();
                            txtOvcCompany.Text = dr["OVC_COMPANY"].ToString();
                            txtOnbCoSort.Text = strONB_CO_SORT;
                            //存取初始值
                            ViewState["type"] = dr["OVC_CO_TYPE"].ToString();
                            ViewState["sort"] = strONB_CO_SORT;
                            txtOdtStartDate.Text = FCommon.getDateTime(dr["ODT_START_DATE"]);
                            txtOdtEndDate.Text = FCommon.getDateTime(dr["ODT_END_DATE"]);
                        }
                    }
                }
                else
                    FCommon.MessageBoxShow(this, "編號錯誤！", "MTS_F12_1", false);
            }
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F12_1{ getQueryString() }");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            if (ViewState["type"] != null) type = ViewState["type"].ToString();
            if (ViewState["sort"] != null) sort = ViewState["sort"].ToString();
            string strOvcCompany = txtOvcCompany.Text;
            string strOnbCoSort = txtOnbCoSort.Text;
            string strOdtStartDate = txtOdtStartDate.Text;
            string strOdtEndDate = txtOdtEndDate.Text;
            decimal decOnbCoSort;
            DateTime dateOdtStartDate, dateOdtEndDate;
            if (strOvcCompany.Equals(string.Empty))
                strMessage += "<p> 請輸入 保險公司！ </p>";
            if (strOnbCoSort.Equals(string.Empty))
                strMessage += "<p> 請輸入 排序！ </p>";
            if (strOdtStartDate.Equals(string.Empty))
                strMessage += "<p> 請輸入 開始日期！ </p>";
            if (strOdtEndDate.Equals(string.Empty))
                strMessage += "<p> 請輸入 結束日期！ </p>";
            bool boolOnbCoSort = FCommon.checkDecimal(strOnbCoSort, "排序", ref strMessage, out decOnbCoSort);
            bool boolOdtStartDate = FCommon.checkDateTime(strOdtStartDate, "開始日期", ref strMessage, out dateOdtStartDate);
            bool boolOdtEndDate = FCommon.checkDateTime(strOdtEndDate, "結束日期", ref strMessage, out dateOdtEndDate);
            if(boolOdtStartDate && boolOdtEndDate && DateTime.Compare(dateOdtStartDate, dateOdtEndDate)>0)
                strMessage += "<p> 結束日期 必須於 開始日期 之後 ！ </p>";

            var quert =
                from dbcompnay in MTS.TBGMT_COMPANY
                select new
                {
                    dbcompnay.ONB_CO_SORT,
                    dbcompnay.OVC_CO_TYPE,
                };
            DataTable dt =new DataTable();
            if (boolOnbCoSort)
            {
                quert = quert.Where(ot => ot.ONB_CO_SORT == decOnbCoSort && ot.OVC_CO_TYPE.Equals(type));
                dt = CommonStatic.LinqQueryToDataTable(quert);
                if (!strOnbCoSort.Equals(sort) && dt.Rows.Count != 0)
                {
                    strMessage += "<p> 同種類排序不可重複 </p>";
                }
            }
            if (strMessage.Equals(string.Empty))
            {
                try
                {
                    company = MTS.TBGMT_COMPANY.Where(ot => ot.CO_SN.Equals(id)).FirstOrDefault();
                    if (company != null)
                    {
                        company.OVC_COMPANY = strOvcCompany;
                    company.ONB_CO_SORT = decOnbCoSort;
                    company.ODT_START_DATE = dateOdtStartDate;
                    company.ODT_END_DATE = dateOdtEndDate;
                    MTS.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), company.GetType().Name.ToString(), this, "更新保險公司資料");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "更新成功");
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "此筆保險公司資料不存在");
                }
                catch (Exception ex)
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "更新失敗");
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                company = MTS.TBGMT_COMPANY.Where(ot => ot.CO_SN.Equals(id)).FirstOrDefault(); //所有FirstOrDefault 的，全都要判斷是否為null
                if (company != null)
                {
                    MTS.Entry(company).State = EntityState.Deleted;
                    MTS.SaveChanges();
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), company.GetType().Name.ToString(), this, "刪除保險公司資料");
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "此筆保險公司資料不存在");
            }
            catch (Exception ex)
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "刪除失敗");
            }}
    }
}