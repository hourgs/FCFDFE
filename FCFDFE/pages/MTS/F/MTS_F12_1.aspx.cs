using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F12_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTS = new MTSEntities();
        TBGMT_COMPANY company = new TBGMT_COMPANY();

        #region  副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_CO_TYPE", ViewState["OVC_CO_TYPE"], true);
            return strQueryString;
        }
        public void dataImport()
        {
            string strOvcCoType = drpOvcCoType.SelectedValue;
            ViewState["OVC_CO_TYPE"] = strOvcCoType;
            var query =
                from company in MTS.TBGMT_COMPANY
                orderby company.ONB_CO_SORT
                select new
                {
                    company.CO_SN,
                    OVC_COMPANY = company.OVC_COMPANY,
                    OVC_CO_TYPE = company.OVC_CO_TYPE != null ? company.OVC_CO_TYPE.Equals("1") ? "保險公司" : company.OVC_CO_TYPE.Equals("2") ? "進艙廠商" : "航運廠商" : "",
                    OVC_CO_TYPE_Value = company.OVC_CO_TYPE,
                    ONB_CO_SORT = company.ONB_CO_SORT,
                    ODT_CREATE_DATE = company.ODT_CREATE_DATE,
                    OVC_CREATE_ID = company.OVC_CREATE_ID,
                    ODT_MODIFY_DATE = company.ODT_MODIFY_DATE,
                    OVC_MODIFY_LOGIN_ID = company.OVC_MODIFY_LOGIN_ID,
                    ODT_START_DATE = company.ODT_START_DATE,
                    ODT_END_DATE = company.ODT_END_DATE,
                };
            query = query.Where(table => table.OVC_CO_TYPE_Value.Equals(strOvcCoType));
            if (query.Count() > 1000)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
            query = query.Take(1000);
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_COMPANY, dt);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    bool boolImport = false;
                    string strOvcCoType;
                    if (FCommon.getQueryString(this, "OVC_CO_TYPE", out strOvcCoType, true))
                    {
                        FCommon.list_setValue(drpOvcCoType, strOvcCoType);
                        boolImport = true;
                    }
                    if (boolImport)
                        dataImport();
                }
                string strbtnSave = "";
                string strFieldName_OVC_COMPANY = "";
                if (drpOvcCoType.SelectedItem.Text == "進艙廠商")
                {
                    btn_1.Visible = true;
                    btn_2.Visible = false;
                    strbtnSave = "新增進艙廠商";
                    strFieldName_OVC_COMPANY = "進艙廠商";
                }
                else if (drpOvcCoType.SelectedItem.Text == "保險公司")
                {
                    btn_1.Visible = true;
                    btn_2.Visible = false;
                    strbtnSave = "新增保險公司";
                    strFieldName_OVC_COMPANY = "保險公司";
                }
                else
                {
                    btn_1.Visible = false;
                    btn_2.Visible = true;
                }
                btnSave.Text = strbtnSave;
                GV_TBGMT_COMPANY.Columns[0] .HeaderText= strFieldName_OVC_COMPANY;
            }
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "type", drpOvcCoType.SelectedValue, true);
            Response.Redirect($"MTS_F12_3{ strQueryString }");
        }
        protected void btnSave_2_Click(object sender, EventArgs e)
        {
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "type", drpOvcCoType.SelectedValue, true);
            Response.Redirect($"MTS_F12_6{ strQueryString }");
        }

        protected void drpOvcCoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strOvcCoType = drpOvcCoType.SelectedValue, strOVC_COMPANY = "";
            switch (strOvcCoType)
            {
                case "1":
                    strOVC_COMPANY = "保險公司";
                    btn_1.Visible = true;
                    btn_2.Visible = false;
                    break;
                case "2":
                    strOVC_COMPANY = "進艙廠商";
                    btn_1.Visible = true;
                    btn_2.Visible = false;
                    break;
                case "3":
                    strOVC_COMPANY = "航運廠商";
                    btn_1.Visible = false;
                    btn_2.Visible = true;
                    break;
            }
            btnSave.Text = $"新增{ strOVC_COMPANY }";
            if (ViewState["hasRows"] != null && strOVC_COMPANY != "航運廠商")
            {
                dataImport();
            }
        }

        protected void GV_TBGMT_COMPANY_RowCommand(object sender, GridViewCommandEventArgs e)
        {
             GridView theGridView = (GridView)sender;
             GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
             int gvrIndex = gvr.RowIndex;
             Guid id = new Guid(theGridView.DataKeys[gvrIndex].Value.ToString());

             string strQueryString = getQueryString();
             FCommon.setQueryString(ref strQueryString, "id", id, true);
             
             switch (e.CommandName)
             {
                 case "btnManagement":
                 Response.Redirect($"MTS_F12_2{ strQueryString }");
                 break;
             }
        }

        protected void GV_TBGMT_COMPANY_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}