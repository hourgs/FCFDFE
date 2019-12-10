using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.MTS.B
{
    public partial class MTS_B11_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        private GMEntities GME = new GMEntities();

        #region 副程式
        protected void dataimport()
        {
            string strMessage = "";
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_SHIP_COMPANY = drpOVC_SHIP_COMPANY.SelectedValue;
            //string strODT_START_DATE = rdoODT_START_DATE.SelectedValue;
            string strODT_START_DATE_S = txtODT_START_DATE_S.Text;
            string strODT_START_DATE_E = txtODT_START_DATE_E.Text;
            string strOVC_STATUS = drpOVC_STATUS.SelectedValue;
            string strOVC_TRANSER_DEPT_CDE = drpOVC_TRANSER_DEPT_CDE.SelectedValue;
            string strOVC_SEA_OR_AIR = drpOVC_SEA_OR_AIR.SelectedValue;
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedValue;

            ViewState["OVC_BLD_NO"] = strOVC_BLD_NO;
            ViewState["OVC_SHIP_COMPANY"] = strOVC_SHIP_COMPANY;
            ViewState["ODT_START_DATE_S"] = strODT_START_DATE_S;
            ViewState["ODT_START_DATE_E"] = strODT_START_DATE_E;
            ViewState["OVC_STATUS"] = strOVC_STATUS;
            ViewState["OVC_TRANSER_DEPT_CDE"] = strOVC_TRANSER_DEPT_CDE;
            ViewState["OVC_SEA_OR_AIR"] = strOVC_SEA_OR_AIR;
            ViewState["OVC_MILITARY_TYPE"] = strOVC_MILITARY_TYPE;

            //bool boolODT_START_DATE = strODT_START_DATE.Equals("2");
            bool boolODT_START_DATE = !strODT_START_DATE_S.Equals(string.Empty) || !strODT_START_DATE_E.Equals(string.Empty);
            bool boolODT_START_DATE_S = DateTime.TryParse(strODT_START_DATE_S, out DateTime dateODT_START_DATE_S);
            bool boolODT_START_DATE_E = DateTime.TryParse(strODT_START_DATE_E, out DateTime dateODT_START_DATE_E);
            if (boolODT_START_DATE && !(boolODT_START_DATE_S && boolODT_START_DATE_E))
                strMessage += "<P> 啟運日期 不完全！ </p>";

            if (strMessage.Equals(string.Empty))
            {
                var querycom_type = MTSE.TBGMT_COMPANY.Where(t => t.OVC_CO_TYPE.Equals("3"));
                var queryBld =
                from bld in MTSE.TBGMT_BLD.AsEnumerable()
                join portStr in MTSE.TBGMT_PORTS on bld.OVC_START_PORT equals portStr.OVC_PORT_CDE
                join portArr in MTSE.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals portArr.OVC_PORT_CDE
                join com in querycom_type on bld.OVC_SHIP_COMPANY equals com.OVC_COMPANY into comT
                from com in comT.DefaultIfEmpty()
                where portArr.OVC_IS_ABROAD.Equals("國內") //進口條件
                select new
                {
                    OVC_BLD_NO = bld.OVC_BLD_NO,
                    OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY ?? "",
                    OVC_SEA_OR_AIR = bld.OVC_SEA_OR_AIR ?? "",
                    OVC_SHIP_NAME = bld.OVC_SHIP_NAME,
                    OVC_VOYAGE = bld.OVC_VOYAGE,
                    ODT_START_DATE = FCommon.getDateTime(bld.ODT_START_DATE),
                    OVC_START_PORT_Value = bld.OVC_START_PORT,
                    OVC_START_PORT = portStr.OVC_PORT_CHI_NAME,
                    OVC_MILITARY_TYPE = bld.OVC_MILITARY_TYPE ?? "",
                    bld.OVC_STATUS,
                    OVC_ARRIVE_PORT_Value = bld.OVC_ARRIVE_PORT ?? "",
                    //OVC_ARRIVE_PORT = portArr.OVC_PORT_CHI_NAME,
                    OVC_REMARK_1 = com != null ? com.OVC_REMARK_1 : ""
                };
                var queryIinn =
                    from bld in queryBld
                    join iinn in MTSE.TBGMT_IINN on bld.OVC_BLD_NO equals iinn.OVC_BLD_NO
                    select bld;
                var query = queryBld.Except(queryIinn);

                if (!strOVC_BLD_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
                if (!strOVC_SHIP_COMPANY.Equals(string.Empty))
                {
                    if (strOVC_SHIP_COMPANY.Equals("合約航商"))
                        query = query.Where(t => t.OVC_REMARK_1.Equals(strOVC_SHIP_COMPANY));
                        //query = query.Where(table => table.OVC_SHIP_COMPANY != strOVC_SHIP_COMPANY && table.OVC_SHIP_COMPANY != "非合約航商");
                    else
                        query = query.Where(table => table.OVC_SHIP_COMPANY.Equals(strOVC_SHIP_COMPANY) || table.OVC_REMARK_1.Equals(strOVC_SHIP_COMPANY));
                }

                if (boolODT_START_DATE)
                    query = query.Where(table => DateTime.TryParse(table.ODT_START_DATE, out DateTime dateODT_START_DATE) &&
                        DateTime.Compare(dateODT_START_DATE, dateODT_START_DATE_S) >= 0 &&
                        DateTime.Compare(dateODT_START_DATE, dateODT_START_DATE_E) <= 0);
                if (!strOVC_STATUS.Equals(string.Empty))
                    query = query.Where(table => table.OVC_STATUS.Equals(strOVC_STATUS));
                if (!strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                {
                    var table = GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("TR"))
                        .Where(t => t.OVC_PHR_PARENTS.Equals(strOVC_TRANSER_DEPT_CDE))
                        .Select(t => t.OVC_PHR_ID).ToArray();

                    query = query.Where(t => table.Contains(t.OVC_ARRIVE_PORT_Value));
                    //var tbmquery = from tbm in GME.TBM1407.AsEnumerable() where tbm.OVC_PHR_CATE.Equals("TR") select tbm;
                    //string[] arrtbmquery = tbmquery.Where(table => table.OVC_PHR_PARENTS.Equals(strOVC_TRANSER_DEPT_CDE)).Select(table => table.OVC_PHR_ID).ToArray();
                    //foreach (string port in arrtbmquery)
                    //{
                    //    query = query.Where(table => table.OVC_START_PORT.Equals(port) || table.OVC_ARRIVE_PORT.Equals(port));
                    //}
                }
                if (!strOVC_SEA_OR_AIR.Equals(string.Empty))
                    query = query.Where(table => table.OVC_SEA_OR_AIR.Equals(strOVC_SEA_OR_AIR));
                if (!strOVC_MILITARY_TYPE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_MILITARY_TYPE.Equals(strOVC_MILITARY_TYPE));
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
            FCommon.setQueryString(ref strQueryString, "OVC_SHIP_COMPANY", ViewState["OVC_SHIP_COMPANY"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_START_DATE_S", ViewState["ODT_START_DATE_S"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_START_DATE_E", ViewState["ODT_START_DATE_E"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_STATUS", ViewState["OVC_STATUS"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", ViewState["OVC_TRANSER_DEPT_CDE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_SEA_OR_AIR", ViewState["OVC_SEA_OR_AIR"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_MILITARY_TYPE", ViewState["OVC_MILITARY_TYPE"], true);
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
                    FCommon.Controls_Attributes("readonly", "true", txtODT_START_DATE_S, txtODT_START_DATE_E);
                    #region 匯入下拉式選單
                    string strFirstText = "不限定";
                    CommonMTS.list_dataImport_SHIP_COMPANY2(drpOVC_SHIP_COMPANY, false); //承運航商
                    CommonMTS.list_dataImport_STATUS(drpOVC_STATUS, false); //需辦投保
                    CommonMTS.list_dataImport_TransferArea(drpOVC_TRANSER_DEPT_CDE, true, strFirstText); //接轉地區
                    CommonMTS.list_dataImport_SEA_OR_AIR(drpOVC_SEA_OR_AIR, true,strFirstText); //海空運別
                    CommonMTS.list_dataImport_DEPT_CLASS(drpOVC_MILITARY_TYPE, true, strFirstText); //軍種
                    #endregion

                    bool boolImport = false;
                    if (FCommon.getQueryString(this, "OVC_BLD_NO", out string strOVC_BLD_NO, true))
                    {
                        txtOVC_BLD_NO.Text = strOVC_BLD_NO;
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_SHIP_COMPANY", out string strOVC_SHIP_COMPANY, false))
                        FCommon.list_setValue(drpOVC_SHIP_COMPANY, strOVC_SHIP_COMPANY);
                    if (FCommon.getQueryString(this, "ODT_START_DATE_S", out string strODT_START_DATE_S, true))
                        txtODT_START_DATE_S.Text = strODT_START_DATE_S;
                    if (FCommon.getQueryString(this, "ODT_START_DATE_E", out string strODT_START_DATE_E, true))
                        txtODT_START_DATE_E.Text = strODT_START_DATE_E;
                    if (FCommon.getQueryString(this, "OVC_STATUS", out string strOVC_STATUS, true))
                        FCommon.list_setValue(drpOVC_STATUS, strOVC_STATUS);
                    if (FCommon.getQueryString(this, "OVC_TRANSER_DEPT_CDE", out string strOVC_TRANSER_DEPT_CDE, true))
                        FCommon.list_setValue(drpOVC_TRANSER_DEPT_CDE, strOVC_TRANSER_DEPT_CDE);
                    if (FCommon.getQueryString(this, "OVC_SEA_OR_AIR", out string strOVC_SEA_OR_AIR, true))
                        FCommon.list_setValue(drpOVC_SEA_OR_AIR, strOVC_SEA_OR_AIR);
                    if (FCommon.getQueryString(this, "OVC_MILITARY_TYPE", out string strOVC_MILITARY_TYPE, true))
                        FCommon.list_setValue(drpOVC_MILITARY_TYPE, strOVC_MILITARY_TYPE);
                    if (boolImport) dataimport();
                }
            }
        }

        #region ~Click
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataimport();
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_B11_3{ getQueryString() }");
        }
        #endregion

        #region GridView
        protected void GVTBGMT_BLD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GVTBGMT_BLD.DataKeys[gvrIndex].Value.ToString();
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);

            switch (e.CommandName)
            {
                case "dataNew":
                    Response.Redirect($"MTS_B11_2{ strQueryString }");
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
        #endregion
    }
}