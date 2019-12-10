using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A17_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        #region 副程式
        private void dataImport()
        {
            string strMessage = "";

            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_TRANSER_DEPT_CDE = drpOVC_TRANSER_DEPT_CDE.SelectedValue.ToString();

            ViewState["OVC_BLD_NO"] = strOVC_BLD_NO;
            ViewState["OVC_TRANSER_DEPT_CDE"] = strOVC_TRANSER_DEPT_CDE;


            if (strOVC_BLD_NO.Equals(string.Empty) && strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                strMessage += "<P> 至少填入一個選項 </p>";

            if (strMessage.Equals(string.Empty))
            {
                //差集寫法
                var query_ird =
                from ird in MTSE.TBGMT_IRD
                join bld in MTSE.TBGMT_BLD on ird.OVC_BLD_NO equals bld.OVC_BLD_NO
                select bld;
                
                var query_icr =
                    from icr in MTSE.TBGMT_ICR
                    join bld in MTSE.TBGMT_BLD on icr.OVC_BLD_NO equals bld.OVC_BLD_NO
                    select bld;
                var queryTemp = query_icr.Except(query_ird); // 有 ICR時程管制簿 尚未建立 IRD接配紀錄表

                if (!strOVC_BLD_NO.Equals(string.Empty))
                    queryTemp = queryTemp.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
                if (!strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                {
                    var table = GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("TR"))
                               .Where(t => t.OVC_PHR_PARENTS.Equals(strOVC_TRANSER_DEPT_CDE))
                               .Select(t => t.OVC_PHR_ID).ToArray();

                    queryTemp = queryTemp.Where(t => table.Contains(t.OVC_ARRIVE_PORT));
                    //query = query.Where(table => table.OVC_ARRIVE_PORT.Equals(strOVC_TRANSER_DEPT_CDE));
                }
                //DataTable dtBLD = CommonStatic.ListToDataTable(query.ToList());
                var query =
                    from bld in queryTemp
                    join arrport in MTSE.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals arrport.OVC_PORT_CDE
                    join strport in MTSE.TBGMT_PORTS on bld.OVC_START_PORT equals strport.OVC_PORT_CDE
                    where arrport.OVC_IS_ABROAD.Equals("國內")
                    orderby bld.OVC_BLD_NO
                    select new
                    {
                        bld.OVC_BLD_NO,
                        bld.OVC_SHIP_COMPANY,
                        bld.OVC_SHIP_NAME,
                        bld.OVC_VOYAGE,
                        OVC_START_PORT = strport.OVC_PORT_CHI_NAME,
                        OVC_ARRIVE_PORT = arrport.OVC_PORT_CHI_NAME,
                        bld.ONB_QUANITY,
                        bld.ONB_VOLUME,
                        bld.ONB_WEIGHT,
                        bld.ONB_CARRIAGE
                    };
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                DataTable dtBLD = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_BLD, dtBLD);
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", ViewState["OVC_BLD_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", ViewState["OVC_TRANSER_DEPT_CDE"], true);
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
                    #endregion
                    bool isImport = false;
                    string strOVC_BLD_NO, strOVC_TRANSER_DEPT_CDE;
                    if (FCommon.getQueryString(this, "OVC_BLD_NO", out strOVC_BLD_NO, true))
                    {
                        txtOVC_BLD_NO.Text = strOVC_BLD_NO;
                        isImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_TRANSER_DEPT_CDE", out strOVC_TRANSER_DEPT_CDE, true))
                        FCommon.list_setValue(drpOVC_TRANSER_DEPT_CDE, strOVC_TRANSER_DEPT_CDE);
                    if (isImport) dataImport();
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }
        
        protected void GV_TBGMT_BLD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = theGridView.DataKeys[gvrIndex].Value.ToString();
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);
            switch (e.CommandName)
            {
                case "DataCreate":
                    Response.Redirect($"MTS_A17_2{ strQueryString }");
                    break;
                default:
                    break;
            }
        }
        protected void GV_TBGMT_BLD_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void GV_TBGMT_BLD_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}