using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F12_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTS = new MTSEntities();
        TBGMT_COMPANY company = new TBGMT_COMPANY();
        Guid id;
        string type;
        //存取初始種類變數
        string sort, strOVC_COMPANY = "", cname;

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
                    //判斷是進艙廠商還是保險公司
                    if (FCommon.getQueryString(this, "OVC_CO_TYPE", out string strOVC_CO_TYPE, true))
                    {
                        switch (strOVC_CO_TYPE)
                        {
                            case "1":
                                strOVC_COMPANY = "保險公司";
                                break;
                            case "2":
                                strOVC_COMPANY = "進艙廠商";
                                break;
                            case "3":
                                strOVC_COMPANY = "航運廠商";
                                trContract.Visible = true;
                                trTransport.Visible = true;
                                trStartDate.Visible = false;
                                trEndDate.Visible = false;
                                break;
                        }
                    }
                    if (!IsPostBack)
                    {
                        FCommon.Controls_Attributes("readonly", "true", txtOdtStartDate, txtOdtEndDate);

                        lblOVC_COMPANY_title.Text = strOVC_COMPANY;
                        lblOVC_COMPANY.Text = strOVC_COMPANY;

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
                            tbgmt_company.OVC_REMARK_1,
                            tbgmt_company.OVC_REMARK_2,
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
                            ViewState["company"] = dr["OVC_COMPANY"].ToString();
                            txtOdtStartDate.Text = FCommon.getDateTime(dr["ODT_START_DATE"]);
                            txtOdtEndDate.Text = FCommon.getDateTime(dr["ODT_END_DATE"]);
                            drpContract.SelectedItem.Text = dr["OVC_REMARK_1"].ToString();
                            drpTransport.SelectedItem.Text = dr["OVC_REMARK_2"].ToString();
                        }
                    }
                }
                else
                    FCommon.MessageBoxShow(this, "編號錯誤！", $"MTS_F12_1{ getQueryString() }", false);
                //給予回傳值
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            if (ViewState["type"] != null) type = ViewState["type"].ToString();
            if (ViewState["sort"] != null) sort = ViewState["sort"].ToString();
            if (ViewState["company"] != null) cname = ViewState["company"].ToString();
            string strOvcCompany = txtOvcCompany.Text;
            string strOnbCoSort = txtOnbCoSort.Text;
            string strOdtStartDate = txtOdtStartDate.Text;
            string strOdtEndDate = txtOdtEndDate.Text;
            string strOVC_MODIFY_LOGIN_ID = Session["username"].ToString();
            string strContract = drpContract.SelectedItem.Text;
            string strTransport = drpTransport.SelectedItem.Text;
            decimal decOnbCoSort;
            DateTime dateOdtStartDate, dateOdtEndDate;
            if (strOvcCompany.Equals(string.Empty))
                strMessage += "<p> 請輸入 名稱！ </p>";
            if (strOnbCoSort.Equals(string.Empty))
                strMessage += "<p> 請輸入 排序！ </p>";
            if (strOdtStartDate.Equals(string.Empty) && type != "3")
                strMessage += "<p> 請輸入 開始日期！ </p>";
            if (strOdtEndDate.Equals(string.Empty) && type != "3")
                strMessage += "<p> 請輸入 結束日期！ </p>";
            bool boolOnbCoSort = FCommon.checkDecimal(strOnbCoSort, "排序", ref strMessage, out decOnbCoSort);
            bool boolOdtStartDate = FCommon.checkDateTime(strOdtStartDate, "開始日期", ref strMessage, out dateOdtStartDate);
            bool boolOdtEndDate = FCommon.checkDateTime(strOdtEndDate, "結束日期", ref strMessage, out dateOdtEndDate);
            if (boolOdtStartDate && boolOdtEndDate && DateTime.Compare(dateOdtStartDate, dateOdtEndDate) > 0)
                strMessage += "<p> 結束日期 必須於 開始日期 之後 ！ </p>";

            var quert =
                from dbcompnay in MTS.TBGMT_COMPANY
                select new
                {
                    dbcompnay.ONB_CO_SORT,
                    dbcompnay.OVC_CO_TYPE,
                    dbcompnay.OVC_COMPANY
                };
            if (boolOnbCoSort)
            {
                var querys = quert.Where(ot => ot.ONB_CO_SORT == decOnbCoSort && ot.OVC_CO_TYPE.Equals(type));
                var queryn = quert.Where(ot => ot.OVC_COMPANY.Equals(strOvcCompany) && ot.OVC_CO_TYPE.Equals(type));
                DataTable dt = CommonStatic.LinqQueryToDataTable(querys);
                DataTable dt2 = CommonStatic.LinqQueryToDataTable(queryn);
                if (!strOnbCoSort.Equals(sort) && dt.Rows.Count != 0)
                {
                    strMessage += "<p> 同種類排序不可重複 </p>";
                }
                if (!strOvcCompany.Equals(cname) && dt2.Rows.Count != 0)
                {
                    strMessage += "<p> 名稱不可重複 </p>";
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
                        company.ODT_MODIFY_DATE = DateTime.Now;
                        company.OVC_MODIFY_LOGIN_ID = strOVC_MODIFY_LOGIN_ID;
                        if (company.OVC_CO_TYPE == "3")
                        {
                            company.OVC_REMARK_1 = strContract;
                            company.OVC_REMARK_2 = strTransport;
                        }
                        else
                        {
                            company.ODT_START_DATE = dateOdtStartDate;
                            company.ODT_END_DATE = dateOdtEndDate;
                        }
                        MTS.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), company.GetType().Name.ToString(), this, $"更新{ strOVC_COMPANY }");
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "更新成功");
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"此筆{ strOVC_COMPANY }不存在");
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
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), company.GetType().Name.ToString(), this, $"刪除{ strOVC_COMPANY }");
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"此筆{ strOVC_COMPANY }不存在");
            }
            catch (Exception ex)
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "刪除失敗");
            }
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F12_1{ getQueryString() }");
        }
    }
}