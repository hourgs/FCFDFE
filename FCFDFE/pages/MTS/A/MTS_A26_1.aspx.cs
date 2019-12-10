using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A26_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        Common FCommon = new Common();

        #region 副程式
        private void dateImport()
        {
            DataTable dt = new DataTable();
            string strMessage = "";
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_EDF_NO = txtOVC_EDF_NO.Text;
            string strOVC_SHIP_NAME = txtOVC_SHIP_NAME.Text;
            string strOVC_VOYAGE = txtOVC_VOYAGE.Text;
            string strOVC_TRANSER_DEPT_CDE = drpOVC_TRANSER_DEPT_CDE.SelectedValue;
            string strOVC_IS_SECURITY = rdoOVC_IS_SECURITY.SelectedValue;

            ViewState["OVC_BLD_NO"] = strOVC_BLD_NO;
            ViewState["OVC_EDF_NO"] = strOVC_EDF_NO;
            ViewState["OVC_SHIP_NAME"] = strOVC_SHIP_NAME;
            ViewState["OVC_VOYAGE"] = strOVC_VOYAGE;
            ViewState["OVC_TRANSER_DEPT_CDE"] = strOVC_TRANSER_DEPT_CDE;
            ViewState["OVC_IS_SECURITY"] = strOVC_IS_SECURITY;

            if (strOVC_BLD_NO.Equals(string.Empty) && strOVC_EDF_NO.Equals(string.Empty) && strOVC_SHIP_NAME.Equals(string.Empty) && strOVC_VOYAGE.Equals(string.Empty)
                && strOVC_TRANSER_DEPT_CDE.Equals(string.Empty) && strOVC_IS_SECURITY.Equals(string.Empty))
                strMessage += "<P> 至少填入一個選項 </p>";

            if (strMessage.Equals(string.Empty))
            {
                var query =
                    from bld in MTSE.TBGMT_BLD.AsEnumerable()
                    where bld.OVC_STATUS.Equals("B")
                    join edf in MTSE.TBGMT_EDF.AsEnumerable() on bld.OVC_BLD_NO equals edf.OVC_BLD_NO into edfTemp
                    from edf in edfTemp.DefaultIfEmpty()
                    join arrport in MTSE.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals arrport.OVC_PORT_CDE
                    join strport in MTSE.TBGMT_PORTS on bld.OVC_START_PORT equals strport.OVC_PORT_CDE
                    where strport.OVC_IS_ABROAD == "國內" //篩選為出口提單
                    orderby bld.OVC_BLD_NO
                    select new
                    {
                        EDF_SN = edf != null ? edf.EDF_SN.ToString() : "",
                        OVC_EDF_NO = edf != null ? edf.OVC_EDF_NO : "",
                        OVC_BLD_NO = bld.OVC_BLD_NO,
                        OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY,
                        OVC_SEA_OR_AIR = bld.OVC_SEA_OR_AIR,
                        OVC_SHIP_NAME = bld.OVC_SHIP_NAME,
                        OVC_VOYAGE = bld.OVC_VOYAGE,
                        ODT_START_DATE = FCommon.getDateTime(bld.ODT_START_DATE),
                        OVC_START_PORT = strport.OVC_PORT_CHI_NAME,
                        OVC_ARRIVE_PORT = arrport.OVC_PORT_CHI_NAME,
                        OVC_START_PORT_Value = bld.OVC_START_PORT,
                        ODT_PLN_ARRIVE_DATE = FCommon.getDateTime(bld.ODT_PLN_ARRIVE_DATE),
                        ODT_ACT_ARRIVE_DATE = FCommon.getDateTime(bld.ODT_ACT_ARRIVE_DATE),
                        OVC_IS_SECURITY = (bld.OVC_IS_SECURITY ?? 0) == 0 ? "否" : "是",
                        OVC_IS_SECURITY_Value = bld.OVC_IS_SECURITY ?? 0
                    };
                if (!strOVC_BLD_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
                if (!strOVC_EDF_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_EDF_NO.Contains(strOVC_EDF_NO));
                if (!strOVC_SHIP_NAME.Equals(string.Empty))
                    query = query.Where(table => table.OVC_SHIP_NAME == strOVC_SHIP_NAME);
                if (!strOVC_VOYAGE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_VOYAGE == strOVC_VOYAGE);
                if (!strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                {
                    var table = GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("TR"))
                    .Where(t => t.OVC_PHR_PARENTS.Equals(strOVC_TRANSER_DEPT_CDE))
                    .Select(t => t.OVC_PHR_ID).ToArray();

                    query = query.Where(t => table.Contains(t.OVC_START_PORT_Value));
                }
                if (!strOVC_IS_SECURITY.Equals(string.Empty))
                {
                    int issecurity;
                    if (int.TryParse(strOVC_IS_SECURITY, out issecurity))
                        query = query.Where(table => table.OVC_IS_SECURITY_Value == issecurity);
                }
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);

                dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_BLD, dt);
            }
            else
                FCommon.AlertShow(PnWarning, "danger", "系統訊息", strMessage);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    #region 匯入下拉式選單
                    string strFirstText = "不限定";
                    CommonMTS.list_dataImport_TransferArea(drpOVC_TRANSER_DEPT_CDE, true, strFirstText); //接轉地區
                    CommonMTS.list_dataImport_IS_SECURITY(rdoOVC_IS_SECURITY, false);//機敏軍品
                    #endregion

                    bool boolImport = false;
                    string strOVC_BLD_NO, strOVC_EDF_NO, strOVC_SHIP_NAME, strOVC_VOYAGE, strOVC_TRANSER_DEPT_CDE, strOVC_IS_SECURITY;
                    if (FCommon.getQueryString(this, "OVC_BLD_NO", out strOVC_BLD_NO, true))
                    {
                        txtOVC_BLD_NO.Text = strOVC_BLD_NO;
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_EDF_NO", out strOVC_EDF_NO, true))
                        txtOVC_EDF_NO.Text = strOVC_EDF_NO;
                    if (FCommon.getQueryString(this, "OVC_SHIP_NAME", out strOVC_SHIP_NAME, true))
                        txtOVC_SHIP_NAME.Text = strOVC_SHIP_NAME;
                    if (FCommon.getQueryString(this, "OVC_VOYAGE", out strOVC_VOYAGE, true))
                        txtOVC_VOYAGE.Text = strOVC_VOYAGE;
                    if (FCommon.getQueryString(this, "OVC_TRANSER_DEPT_CDE", out strOVC_TRANSER_DEPT_CDE, true))
                        FCommon.list_setValue(drpOVC_TRANSER_DEPT_CDE, strOVC_TRANSER_DEPT_CDE);
                    if (FCommon.getQueryString(this, "OVC_IS_SECURITY", out strOVC_IS_SECURITY, true))
                        FCommon.list_setValue(rdoOVC_IS_SECURITY, strOVC_IS_SECURITY);
                    if (boolImport)
                        dateImport();
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dateImport();
        }
        
        protected void GVTBGMT_BLD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GVTBGMT_BLD.DataKeys[gvrIndex].Value.ToString();

            switch (e.CommandName)
            {
                case "btnModify":
                    string strQueryString = "";
                    FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", ViewState["OVC_BLD_NO"], true);
                    FCommon.setQueryString(ref strQueryString, "OVC_EDF_NO", ViewState["OVC_EDF_NO"], true);
                    FCommon.setQueryString(ref strQueryString, "OVC_SHIP_NAME", ViewState["OVC_SHIP_NAME"], true);
                    FCommon.setQueryString(ref strQueryString, "OVC_VOYAGE", ViewState["OVC_VOYAGE"], true);
                    FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", ViewState["OVC_TRANSER_DEPT_CDE"], true);
                    FCommon.setQueryString(ref strQueryString, "OVC_IS_SECURITY", ViewState["OVC_IS_SECURITY"], true);
                    FCommon.setQueryString(ref strQueryString, "id", id, true);
                    Response.Redirect($"MTS_A26_2{ strQueryString }");
                    break;
            }
        }
        protected void GVTBGMT_BLD_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                GridView theGridView = (GridView)sender;
                int index = gvr.RowIndex;
                HiddenField lblEDF_SN = (HiddenField)gvr.FindControl("lblEDF_SN");
                HyperLink hlkOVC_EDF_NO = (HyperLink)gvr.FindControl("hlkOVC_EDF_NO");
                HyperLink hlkOVC_BLD_NO = (HyperLink)gvr.FindControl("hlkOVC_BLD_NO");
                if (FCommon.Controls_isExist(lblEDF_SN, hlkOVC_EDF_NO, hlkOVC_BLD_NO))
                {
                    string strOVC_EDF_NO = lblEDF_SN.Value;
                    hlkOVC_EDF_NO.NavigateUrl = $"javascript: OpenWindow_EDFDATA('{ FCommon.getEncryption(strOVC_EDF_NO) }');";
                    
                    string strOVC_BLD_NO = hlkOVC_BLD_NO.Text;
                    hlkOVC_BLD_NO.NavigateUrl = $"javascript: OpenWindow_BLDDATA('{ FCommon.getEncryption(strOVC_BLD_NO) }');";
                }
            }
        }
        protected void GVTBGMT_BLD_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}