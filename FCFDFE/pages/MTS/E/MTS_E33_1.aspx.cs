using System;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.IO;

namespace FCFDFE.pages.MTS.E
{
    public partial class MTS_E33_1 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities mtse = new MTSEntities();
        GMEntities gme = new GMEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    if (Session["userid"] != null)
                    {
                        string strUser = Session["userid"].ToString();
                        var query = gme.ACCOUNTs.Where(id => id.USER_ID == strUser).FirstOrDefault();
                        if (query.DEPT_SN != null)
                        {
                            string strdept = query.DEPT_SN;
                            var querydept = gme.TBMDEPTs.Where(id => id.OVC_DEPT_CDE == strdept).FirstOrDefault();
                            lblDept.Text = querydept.OVC_ONNAME;
                            FCommon.Controls_Attributes("readonly", "true", txtOvcApplyDate1, txtOvcApplyDate2);
                            txtOvcApplyDate1.Text = DateTime.Now.ToString("yyyy-MM-dd");
                            txtOvcApplyDate2.Text = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(7).ToString("yyyy-MM-dd");

                            bool boolimport = false;
                            string strOvcTofNo, strOvcIsPaid, strOvcBudget, strOvcPurposeType, strOdtApplyDate1, strOdtApplyDate2, strOvcSection, strchkOdtApplyDate;
                            if (FCommon.getQueryString(this, "OvcTofNo", out strOvcTofNo, true))
                            {
                                txtOvcTofNo.Text = strOvcTofNo;
                                boolimport = true;
                            }
                            if (FCommon.getQueryString(this, "OvcIsPaid", out strOvcIsPaid, true))
                                FCommon.list_setValue(drpOvcIsPaid, strOvcIsPaid);
                            if (FCommon.getQueryString(this, "OdtApplyDate1", out strOdtApplyDate1, true))
                                txtOvcApplyDate1.Text = strOdtApplyDate1;
                            if (FCommon.getQueryString(this, "OdtApplyDate2", out strOdtApplyDate2, true))
                                txtOvcApplyDate1.Text = strOdtApplyDate2;
                            if (FCommon.getQueryString(this, "OvcBudget", out strOvcBudget, true))
                                FCommon.list_setValue(drpOvcBudget, strOvcBudget);
                            if (FCommon.getQueryString(this, "OvcPurposeType", out strOvcPurposeType, true))
                                FCommon.list_setValue(drpOvcPurposeType, strOvcPurposeType);
                            if (FCommon.getQueryString(this, "OvcSection", out strOvcSection, true))
                                FCommon.list_setValue(drpOvcSection, strOvcSection);
                            if (FCommon.getQueryString(this, "chkOdtApplyDate", out strchkOdtApplyDate, true))
                                FCommon.list_setValue(chkOdtApplyDate, strchkOdtApplyDate);
                            if (boolimport)
                                dataImport();
                        }
                    }
                }
            }

        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }

        private void dataImport()
        {
            string strOvcTofNo = txtOvcTofNo.Text;
            string strOvcIsPaid = drpOvcIsPaid.SelectedItem.ToString();
            string strOvcBudget = drpOvcBudget.SelectedItem.ToString();
            string strOvcPurposeType = drpOvcPurposeType.SelectedValue.ToString();
            string strOdtApplyDate1 = txtOvcApplyDate1.Text;
            string strOdtApplyDate2 = txtOvcApplyDate2.Text;
            string strOvcSection = drpOvcSection.SelectedItem.ToString();
            string strchkOdtApplyDate = "";
            try
            {
                strchkOdtApplyDate = chkOdtApplyDate.SelectedItem.Text;
            }
            catch { };

            ViewState["OvcTofNo"] = strOvcTofNo;
            ViewState["OvcIsPaid"] = strOvcIsPaid;
            ViewState["OvcBudget"] = strOvcBudget;
            ViewState["OvcPurposeType"] = strOvcPurposeType;
            ViewState["OdtApplyDate1"] = strOdtApplyDate1;
            ViewState["OdtApplyDate2"] = strOdtApplyDate2;
            ViewState["OvcSection"] = strOvcSection;
            ViewState["chkOdtApplyDate"] = strchkOdtApplyDate;

            var totalcount = chkOdtApplyDate.Items.Cast<System.Web.UI.WebControls.ListItem>().Where(item => item.Selected).Count();

            if (strOvcTofNo.Equals(string.Empty) && strOvcIsPaid.Equals("不限定") &&
                strOvcBudget.Equals("不限定") && strOvcPurposeType.Equals("不限定") &&
                strOdtApplyDate1.Equals(string.Empty) && strOdtApplyDate2.Equals(string.Empty) &&
                totalcount == 0 && strOvcSection.Equals("不限定"))
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請至少填寫一項條件");
            }
            else
            {
                var query =
                    from tof in mtse.TBGMT_TOF
                    select new
                    {
                        OVC_TOF_NO = tof.OVC_TOF_NO,
                        OVC_BUDGET = tof.OVC_BUDGET,
                        OVC_PURPOSE_TYPE = tof.OVC_PURPOSE_TYPE,
                        OVC_ABSTRACT = tof.OVC_ABSTRACT,
                        ONB_AMOUNT = tof.ONB_AMOUNT,
                        OVC_NOTE = tof.OVC_NOTE,
                        OVC_SECTION = tof.OVC_SECTION,
                        ODT_APPLY_DATE = tof.ODT_APPLY_DATE,
                        OVC_APPLY_ID = tof.OVC_APPLY_ID,
                        OVC_IS_PAID = tof.OVC_IS_PAID,
                        ODT_PAID_DATE = tof.ODT_PAID_DATE
                    };
                if (!strOvcTofNo.Equals(string.Empty))
                    query = query.Where(table => table.OVC_TOF_NO.Contains(strOvcTofNo));
                if (!strOvcIsPaid.Equals("不限定"))
                    query = query.Where(table => table.OVC_IS_PAID == strOvcIsPaid);
                if (!strOvcBudget.Equals("不限定"))
                    query = query.Where(table => table.OVC_BUDGET == strOvcBudget);
                if (!strOvcPurposeType.Equals("不限定"))
                    query = query.Where(table => table.OVC_PURPOSE_TYPE == strOvcPurposeType);
                if (strOvcSection.Equals("高雄接轉組"))
                    query = query.Where(table => table.OVC_SECTION == strOvcSection);
                if (strOvcSection.Equals("桃園地區"))
                    query = query.Where(table => table.OVC_SECTION == "桃園接轉組");
                if (strOvcSection.Equals("基隆地區"))
                    query = query.Where(table => table.OVC_SECTION == "基隆接轉組");
                if (totalcount == 0)
                {
                    if (!strOdtApplyDate1.Equals(string.Empty))
                    {
                        DateTime d1 = Convert.ToDateTime(strOdtApplyDate1);
                        query = query.Where(table => table.ODT_APPLY_DATE >= d1);
                    }
                    if (!strOdtApplyDate2.Equals(string.Empty))
                    {
                        DateTime d2 = Convert.ToDateTime(strOdtApplyDate2);
                        query = query.Where(table => table.ODT_APPLY_DATE >= d2);
                    }
                }
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                DataTable dt = new DataTable();
                dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_TOF, dt);
            }

        }

        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OvcTofNo", ViewState["OvcTofNo"], true);
            FCommon.setQueryString(ref strQueryString, "OvcIsPaid", ViewState["OvcIsPaid"], true);
            FCommon.setQueryString(ref strQueryString, "OvcBudget", ViewState["OvcBudget"], true);
            FCommon.setQueryString(ref strQueryString, "OvcPurposeType", ViewState["OvcPurposeType"], true);
            FCommon.setQueryString(ref strQueryString, "OdtApplyDate2", ViewState["OdtApplyDate2"], true);
            FCommon.setQueryString(ref strQueryString, "OvcSection", ViewState["OvcSection"], true);
            FCommon.setQueryString(ref strQueryString, "chkOdtApplyDate", ViewState["chkOdtApplyDate"], true);
            return strQueryString;
        }
        protected void GV_TBGMT_TOF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBGMT_TOF.DataKeys[gvrIndex].Value.ToString();
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);
            switch (e.CommandName)
            {
                case "btnModify":
                    string str_url_Modify;
                    str_url_Modify = $"MTS_E33_2.aspx{strQueryString}";
                    Response.Redirect(str_url_Modify);
                    break;
                default:
                    break;
            }
        }

        protected void GV_TBGMT_TOF_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}