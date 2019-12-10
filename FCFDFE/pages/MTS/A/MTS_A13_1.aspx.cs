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
    public partial class MTS_A13_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        Common FCommon = new Common();

        #region 副程式
        private void dataImport()
            {
            string strMessage = "";
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_TRANSER_DEPT_CDE = drpOVC_TRANSER_DEPT_CDE.SelectedValue;
            string strOVC_SHIP_NAME = txtOVC_SHIP_NAME.Text;
            string strOVC_VOYAGE = txtOVC_VOYAGE.Text;
            string strOVC_IS_SECURITY = drpOVC_IS_SECURITY.SelectedValue;

            ViewState["OVC_BLD_NO"] = strOVC_BLD_NO;
            ViewState["OVC_TRANSER_DEPT_CDE"] = strOVC_TRANSER_DEPT_CDE;
            ViewState["OVC_SHIP_NAME"] = strOVC_SHIP_NAME;
            ViewState["OVC_VOYAGE"] = strOVC_VOYAGE;
            ViewState["OVC_IS_SECURITY"] = strOVC_IS_SECURITY;

            if (strOVC_BLD_NO.Equals(string.Empty) && strOVC_TRANSER_DEPT_CDE.Equals(string.Empty) && strOVC_SHIP_NAME.Equals(string.Empty)
                && strOVC_VOYAGE.Equals(string.Empty) && strOVC_IS_SECURITY.Equals(string.Empty))
                strMessage += "<P> 至少填入一個選項 </p>";

            if (strMessage.Equals(string.Empty))
            {
                var query =
                    from bld in MTSE.TBGMT_BLD
                    join icr in MTSE.TBGMT_ICR on bld.OVC_BLD_NO equals icr.OVC_BLD_NO into ps
                    from icr in ps.DefaultIfEmpty()
                    join arrport in MTSE.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals arrport.OVC_PORT_CDE
                    join strport in MTSE.TBGMT_PORTS on bld.OVC_START_PORT equals strport.OVC_PORT_CDE
                    where icr.OVC_BLD_NO == null
                    where arrport.OVC_IS_ABROAD.Equals("國內")
                    select new
                    {
                        OVC_BLD_NO = bld.OVC_BLD_NO,
                        OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY,
                        OVC_SEA_OR_AIR = bld.OVC_SEA_OR_AIR,
                        OVC_SHIP_NAME = bld.OVC_SHIP_NAME??"",
                        OVC_VOYAGE = bld.OVC_VOYAGE??"",
                        OVC_START_PORT = strport.OVC_PORT_CHI_NAME,
                        OVC_START_PORT_Value = bld.OVC_START_PORT??"",
                        OVC_ARRIVE_PORT = arrport.OVC_PORT_CHI_NAME,
                        //OVC_ARRIVE_PORT = bld.OVC_ARRIVE_PORT,
                        ONB_QUANITY = bld.ONB_QUANITY,
                        ONB_VOLUME = bld.ONB_VOLUME,
                        ONB_WEIGHT = bld.ONB_WEIGHT,
                        ONB_CARRIAGE = bld.ONB_CARRIAGE,
                        OVC_IS_SECURITY_Value = bld.OVC_IS_SECURITY ?? 0,
                        OVC_IS_SECURITY = (bld.OVC_IS_SECURITY ?? 0) == 0 ? "否" : "是"
                    };
                if (!strOVC_BLD_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
                if (!strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                {
                    var table = GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("TR"))
                        .Where(t => t.OVC_PHR_PARENTS.Equals(strOVC_TRANSER_DEPT_CDE))
                        .Select(t => t.OVC_PHR_ID).ToArray();

                    query = query.Where(t => table.Contains(t.OVC_START_PORT_Value));
                }
                if (!strOVC_SHIP_NAME.Equals(string.Empty))
                    query = query.Where(table => table.OVC_SHIP_NAME.Contains(strOVC_SHIP_NAME));
                if (!strOVC_VOYAGE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_VOYAGE.Contains(strOVC_VOYAGE));
                if (!strOVC_IS_SECURITY.Equals(string.Empty) && decimal.TryParse(strOVC_IS_SECURITY, out decimal decOVC_IS_SECURITY))
                    query = query.Where(table => table.OVC_IS_SECURITY_Value == decOVC_IS_SECURITY);
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_BLD, dt);
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", ViewState["OVC_BLD_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", ViewState["OVC_TRANSER_DEPT_CDE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_SHIP_NAME", ViewState["OVC_SHIP_NAME"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_VOYAGE", ViewState["OVC_VOYAGE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_IS_SECURITY", ViewState["OVC_IS_SECURITY"], true);
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
                    #region 匯入下拉式選單
                    string strFirstText = "不限定";
                    CommonMTS.list_dataImport_TransferArea(drpOVC_TRANSER_DEPT_CDE, true, strFirstText); //接轉地區
                    CommonMTS.list_dataImport_IS_SECURITY(drpOVC_IS_SECURITY, true, strFirstText);//機敏軍品
                    #endregion
                    bool isImport = false;
                    string strOVC_BLD_NO, strOVC_TRANSER_DEPT_CDE, strOVC_SHIP_NAME, strOVC_VOYAGE, strOVC_IS_SECURITY;
                    if (FCommon.getQueryString(this, "OVC_BLD_NO", out strOVC_BLD_NO, true))
                    {
                        txtOVC_BLD_NO.Text = strOVC_BLD_NO;
                        isImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_TRANSER_DEPT_CDE", out strOVC_TRANSER_DEPT_CDE, true))
                        FCommon.list_setValue(drpOVC_TRANSER_DEPT_CDE, strOVC_TRANSER_DEPT_CDE);
                    if (FCommon.getQueryString(this, "OVC_SHIP_NAME", out strOVC_SHIP_NAME, true))
                        txtOVC_SHIP_NAME.Text = strOVC_SHIP_NAME;
                    if (FCommon.getQueryString(this, "OVC_VOYAGE", out strOVC_VOYAGE, true))
                        txtOVC_VOYAGE.Text = strOVC_VOYAGE;
                    if (FCommon.getQueryString(this, "OVC_IS_SECURITY", out strOVC_IS_SECURITY, true))
                        FCommon.list_setValue(drpOVC_IS_SECURITY, strOVC_IS_SECURITY);
                    if (isImport) dataImport();
                }
            }
            
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }
                
        protected void GVTBGMT_BLD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GVTBGMT_BLD.DataKeys[gvrIndex].Value.ToString();
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);
            switch (e.CommandName)
            {
                case "btnNew":
                    Response.Redirect($"MTS_A13_2{ strQueryString }");
                    break;
                default:
                    break;
            }
        }
        protected void GVTBGMT_BLD_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;
            if(gvr.RowType == DataControlRowType.DataRow)
            {
                GridView theGridView = (GridView)sender;
                int index = gvr.RowIndex;
                HyperLink hlkOVC_BLD_NO = (HyperLink)gvr.FindControl("hlkOVC_BLD_NO");
                if (FCommon.Controls_isExist(hlkOVC_BLD_NO))
                {
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