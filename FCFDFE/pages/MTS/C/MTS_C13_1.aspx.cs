using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.MTS.C
{
    public partial class MTS_C13_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();

        #region 副程式
        private void dataImport()
        {
            string strMessage = "";
            string strOVC_CLAIM_CONDITION = drpOVC_CLAIM_CONDITION.SelectedValue;
            string strOVC_CLAIM_NO = txtOVC_CLAIM_NO.Text;
            string strOVC_DEPT_CDE = txtOVC_DEPT_CDE.Value;
            string strODT_CLAIM_DATE = txtODT_CLAIM_DATE.Text;

            ViewState["OVC_CLAIM_CONDITION"] = strOVC_CLAIM_CONDITION;
            ViewState["OVC_CLAIM_NO"] = strOVC_CLAIM_NO;
            ViewState["OVC_DEPT_CDE"] = strOVC_DEPT_CDE;
            ViewState["ODT_CLAIM_DATE"] = strODT_CLAIM_DATE;

            if (strOVC_CLAIM_CONDITION.Equals(string.Empty) && strOVC_CLAIM_NO.Equals(string.Empty) && strOVC_DEPT_CDE.Equals(string.Empty) && strODT_CLAIM_DATE.Equals(string.Empty))
                strMessage += "<p> 至少填入一個查詢條件！ </p>";

            if (strMessage.Equals(string.Empty))
            {
                var query =
                   from claim in MTSE.TBGMT_CLAIM.AsEnumerable()
                   join dept in GME.TBMDEPTs on claim.OVC_MILITARY_TYPE equals dept.OVC_DEPT_CDE
                   join account in GME.ACCOUNTs on claim.OVC_CREATE_LOGIN_ID equals account.USER_ID
                   select new
                   {
                       CLAIM_SN = claim.CLAIM_SN,
                       OVC_CLAIM_NO = claim.OVC_CLAIM_NO,
                       OVC_MILITARY_TYPE = claim.OVC_MILITARY_TYPE??"",
                       OVC_ONNAME = dept.OVC_ONNAME,
                       ODT_CLAIM_DATE = FCommon.getDateTime(claim.ODT_CLAIM_DATE),
                       OVC_CLAIM_MSG_NO = claim.OVC_CLAIM_MSG_NO,
                       OVC_INN_NO = claim.OVC_INN_NO,
                       OVC_CLAIM_ITEM = claim.OVC_CLAIM_ITEM,
                       ONB_CLAIM_NUMBER = claim.ONB_CLAIM_NUMBER,
                       ONB_CLAIM_AMOUNT = claim.ONB_CLAIM_AMOUNT,
                       OVC_CREATE_LOGIN_ID = account.USER_NAME,

                       OVC_CLAIM_CONDITION = claim.OVC_CLAIM_CONDITION ?? ""
                   };
                if (!strOVC_CLAIM_CONDITION.Equals(string.Empty))
                    query = query.Where(table => table.OVC_CLAIM_CONDITION.Equals(strOVC_CLAIM_CONDITION));
                if (!strOVC_DEPT_CDE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_MILITARY_TYPE.Equals(strOVC_DEPT_CDE));
                if (!strOVC_CLAIM_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_CLAIM_NO.Contains(strOVC_CLAIM_NO));
                if (!strODT_CLAIM_DATE.Equals(string.Empty) && DateTime.TryParse(strODT_CLAIM_DATE, out DateTime dateODT_CLAIM_DATEs))
                    query = query.Where(table => DateTime.TryParse(table.ODT_CLAIM_DATE, out DateTime dateODT_CLAIM_DATE) && DateTime.Compare(dateODT_CLAIM_DATE, dateODT_CLAIM_DATEs) == 0);

                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_CLAIM, dt);
            }
            else
                FCommon.AlertShow(pnMessageQuery, "danger", "系統訊息", strMessage);
        }
        private string getQueryString()
        {
            string strQueryString = "";
            //FCommon.setQueryString(ref strQueryString, "CLAIM_SN", ViewState["CLAIM_SN"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_CLAIM_CONDITION", ViewState["OVC_CLAIM_CONDITION"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_CLAIM_NO", ViewState["OVC_CLAIM_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_DEPT_CDE", ViewState["OVC_DEPT_CDE"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_CLAIM_DATE", ViewState["ODT_CLAIM_DATE"], true);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_ONNAME, txtODT_CLAIM_DATE);
                    #region 匯入下拉式選單
                    string strFirstText = "不限定";
                    CommonMTS.list_dataImport_CLAIM_CONDITION2(drpOVC_CLAIM_CONDITION, true, strFirstText); //索賠情形
                    #endregion

                    bool boolImport = false;
                    if (FCommon.getQueryString(this, "OVC_CLAIM_CONDITION", out string strOVC_CLAIM_CONDITION, true))
                        FCommon.list_setValue(drpOVC_CLAIM_CONDITION, strOVC_CLAIM_CONDITION);
                    if (FCommon.getQueryString(this, "OVC_DEPT_CDE", out string strOVC_DEPT_CDE, true))
                    {
                        txtOVC_DEPT_CDE.Value = strOVC_DEPT_CDE;
                        txtOVC_ONNAME.Text = FCommon.getDeptName(strOVC_DEPT_CDE);
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_CLAIM_NO", out string strOVC_CLAIM_NO, true))
                        txtOVC_CLAIM_NO.Text = strOVC_CLAIM_NO;
                    if (FCommon.getQueryString(this, "ODT_CLAIM_DATE", out string strODT_CLAIM_DATE, true))
                        txtODT_CLAIM_DATE.Text = strODT_CLAIM_DATE;

                    if (boolImport)
                        dataImport();
                }
            }
        }

        #region ~Click
        protected void btnClearDept_Click(object sender, EventArgs e)
        {
            FCommon.Controls_Clear(txtOVC_DEPT_CDE, txtOVC_ONNAME);
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }
        #endregion

        #region GridView
        protected void GV_TBGMT_CLAIM_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = theGridView.DataKeys[gvrIndex].Value.ToString();
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);

            switch (e.CommandName)
            {
                case "btnManage":
                    Response.Redirect($"MTS_C13_2{ strQueryString }");
                    break;
                default:
                    break;
            }
        }
        protected void GV_TBGMT_CLAIM_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //int sum = GV_TBGMT_CLAIM.Rows.Count;
            //if(e.Row.RowType==DataControlRowType.Footer)
            //    e.Row.Cells[1].Text = "共" + sum + "筆";
        }
        protected void GV_TBGMT_CLAIM_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        #endregion
    }
}