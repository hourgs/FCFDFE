using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A27_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        public Common FCommon = new Common();

        #region 副程式
        private void dataImport()
        {
            string strMessage = "";
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            ViewState["OVC_BLD_NO"] = strOVC_BLD_NO;

            if (strOVC_BLD_NO.Equals(string.Empty))
                strMessage += "<P> 請輸入 提單號碼 </p>";

            if (strMessage.Equals(string.Empty))
            {
                var b =
                    from bld in MTSE.TBGMT_BLD
                    where bld.OVC_BLD_NO.Contains(strOVC_BLD_NO)
                    join arrport in MTSE.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals arrport.OVC_PORT_CDE
                    join strport in MTSE.TBGMT_PORTS on bld.OVC_START_PORT equals strport.OVC_PORT_CDE
                    where strport.OVC_IS_ABROAD == "國內" //篩選為出口提單
                    select new
                    {
                        OVC_BLD_NO = bld.OVC_BLD_NO,
                        OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY,
                        OVC_SEA_OR_AIR = bld.OVC_SEA_OR_AIR,
                        OVC_SHIP_NAME = bld.OVC_SHIP_NAME,
                        OVC_VOYAGE = bld.OVC_VOYAGE,
                        //OVC_START_PORT = bld.OVC_START_PORT,
                        //OVC_ARRIVE_PORT = bld.OVC_ARRIVE_PORT,
                        OVC_START_PORT = strport.OVC_PORT_CHI_NAME,
                        OVC_ARRIVE_PORT = arrport.OVC_PORT_CHI_NAME,
                        ONB_QUANITY = bld.ONB_QUANITY,
                        ONB_VOLUME = bld.ONB_VOLUME,
                        ONB_WEIGHT = bld.ONB_WEIGHT,
                        ONB_CARRIAGE = bld.ONB_CARRIAGE
                    };
                var c =
                    from bld in b
                    join ecl in MTSE.TBGMT_ECL on bld.OVC_BLD_NO equals ecl.OVC_BLD_NO
                    select bld;
                var query = b.Except(c);
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);

                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
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
                    string strOVC_BLD_NO;
                    if (FCommon.getQueryString(this, "OVC_BLD_NO", out strOVC_BLD_NO, true))
                    {
                        txtOVC_BLD_NO.Text = strOVC_BLD_NO;
                        dataImport();
                    }
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

            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", ViewState["OVC_BLD_NO"], true);
            FCommon.setQueryString(ref strQueryString, "id", id, true);

            switch (e.CommandName)
            {
                case "btnNew":
                    Response.Redirect($"MTS_A27_2{ strQueryString }");
                    break;
                default:
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