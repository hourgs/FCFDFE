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
    public partial class MTS_A23_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        Common FCommon = new Common();

        #region 副程式
        private void dataImport()
        {
            string strMessage = "";
            string strOVC_DEPT_CDE = txtOVC_DEPT_CDE.Value;
            string strOVC_EDF_NO = txtOVC_EDF_NO.Text;
            string strOVC_START_PORT = drpOVC_START_PORT.SelectedValue;
            string strOVC_ARRIVE_PORT = txtOVC_PORT_CDE.Text;
            string strOVC_TRANSER_DEPT_CDE = drpOVC_TRANSER_DEPT_CDE.SelectedValue;
            string strOVC_IS_STRATEGY = drpOVC_IS_STRATEGY.SelectedValue;
            string strOVC_REVIEW_STATUS = drpOVC_REVIEW_STATUS.SelectedValue;
            string strEDF_SN = drpEDF_SN.SelectedValue;
            string strODT_APPLY_DATE_S = txtODT_APPLY_DATE_S.Text;
            string strODT_APPLY_DATE_E = txtODT_APPLY_DATE_E.Text;

            ViewState["OVC_DEPT_CDE"] = strOVC_DEPT_CDE;
            ViewState["OVC_EDF_NO"] = strOVC_EDF_NO;
            ViewState["OVC_START_PORT"] = strOVC_START_PORT;
            ViewState["OVC_ARRIVE_PORT"] = strOVC_ARRIVE_PORT;
            ViewState["OVC_TRANSER_DEPT_CDE"] = strOVC_TRANSER_DEPT_CDE;
            ViewState["OVC_IS_STRATEGY"] = strOVC_IS_STRATEGY;
            ViewState["OVC_REVIEW_STATUS"] = strOVC_REVIEW_STATUS;
            ViewState["EDF_SN"] = strEDF_SN;
            ViewState["ODT_APPLY_DATE_S"] = strODT_APPLY_DATE_S;
            ViewState["ODT_APPLY_DATE_E"] = strODT_APPLY_DATE_E;

            if (strOVC_DEPT_CDE.Equals(string.Empty) && strOVC_EDF_NO.Equals(string.Empty) && strOVC_START_PORT.Equals(string.Empty) && strOVC_ARRIVE_PORT.Equals(string.Empty)
                && strOVC_TRANSER_DEPT_CDE.Equals(string.Empty) && strOVC_IS_STRATEGY.Equals(string.Empty) && strOVC_REVIEW_STATUS.Equals(string.Empty) && strEDF_SN.Equals(string.Empty)
                && (strODT_APPLY_DATE_S.Equals(string.Empty) && strODT_APPLY_DATE_E.Equals(string.Empty)))
                strMessage += "<P> 至少填入一個選項 </p>";

            if (strMessage.Equals(string.Empty))
            {
                //var esoquery = from eso in MTSE.TBGMT_ESO.AsEnumerable() select eso.OVC_EDF_NO;
                //string[] list = esoquery.ToArray();

                var query =
                    from edf in MTSE.TBGMT_EDF.AsEnumerable()
                    join arrport in MTSE.TBGMT_PORTS on edf.OVC_ARRIVE_PORT equals arrport.OVC_PORT_CDE
                    join strport in MTSE.TBGMT_PORTS on edf.OVC_START_PORT equals strport.OVC_PORT_CDE
                    join dept in MTSE.TBMDEPTs on edf.OVC_REQ_DEPT_CDE equals dept.OVC_DEPT_CDE into tempD
                    from dept in tempD.DefaultIfEmpty()
                        //where list.All(a => a != edf.OVC_EDF_NO) //篩選尚未建立訂艙單之外運資料表
                    select new
                    {
                        edf.EDF_SN,
                        OVC_EDF_NO = edf.OVC_EDF_NO,
                        OVC_PURCH_NO = edf.OVC_PURCH_NO,
                        OVC_START_PORT = strport.OVC_PORT_CHI_NAME,
                        OVC_ARRIVE_PORT = arrport.OVC_PORT_CHI_NAME,
                        OVC_START_PORT_Value = edf.OVC_START_PORT,
                        OVC_ARRIVE_PORT_Value = edf.OVC_ARRIVE_PORT,
                        OVC_DEPT_CDE = edf.OVC_DEPT_CDE != null ? edf.OVC_DEPT_CDE : "",
                        OVC_REQ_DEPT_CDE = dept != null ? dept.OVC_ONNAME : "",
                        //OVC_REQ_DEPT_CDE_Value = edf.OVC_REQ_DEPT_CDE ?? "",
                        OVC_CREATE_ID = edf.OVC_CREATE_LOGIN_ID,
                        ODT_RECEIVE_DATE = FCommon.getDateTime(edf.ODT_RECEIVE_DATE),
                        OVC_IS_STRATEGY = edf.OVC_IS_STRATEGY ?? "",
                        OVC_REVIEW_STATUS = edf.OVC_REVIEW_STATUS ?? "",
                    };
                if (!strOVC_DEPT_CDE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_DEPT_CDE .Equals(strOVC_DEPT_CDE));
                if (!strOVC_EDF_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_EDF_NO.Contains(strOVC_EDF_NO));
                if (!strOVC_START_PORT.Equals(string.Empty))
                    query = query.Where(table => table.OVC_START_PORT_Value.Equals(strOVC_START_PORT));
                if (!strOVC_ARRIVE_PORT.Equals(string.Empty))
                    query = query.Where(table => table.OVC_ARRIVE_PORT_Value.Equals(strOVC_ARRIVE_PORT));
                if (!strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                {
                    var table = GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("TR"))
                                .Where(t => t.OVC_PHR_PARENTS.Equals(strOVC_TRANSER_DEPT_CDE))
                                .Select(t => t.OVC_PHR_ID).ToArray();

                    query = query.Where(t => table.Contains(t.OVC_START_PORT_Value));
                }
                if (!strOVC_IS_STRATEGY.Equals(string.Empty))
                    query = query.Where(table => table.OVC_IS_STRATEGY.Equals(strOVC_IS_STRATEGY));
                if (!strOVC_REVIEW_STATUS.Equals(string.Empty))
                    query = query.Where(table => table.OVC_REVIEW_STATUS.Equals(strOVC_REVIEW_STATUS));
                //if (!strEDF_SN.Equals(string.Empty))
                //    query = query.Where(table => table.EDF_SN.Equals(Guid.Empty));
                if (!strODT_APPLY_DATE_S.Equals(string.Empty))
                    query = query.Where(table => DateTime.TryParse(table.ODT_RECEIVE_DATE, out DateTime dateODT_RECEIVE_DATE) && DateTime.Compare(dateODT_RECEIVE_DATE, Convert.ToDateTime(strODT_APPLY_DATE_S)) >= 0);
                if(!strODT_APPLY_DATE_E.Equals(string.Empty))
                    query = query.Where(table => DateTime.TryParse(table.ODT_RECEIVE_DATE, out DateTime dateODT_RECEIVE_DATE) && DateTime.Compare(dateODT_RECEIVE_DATE, Convert.ToDateTime(strODT_APPLY_DATE_E)) <= 0);
                
                var c =
                    from edf in query
                    join eso in MTSE.TBGMT_ESO on edf.OVC_EDF_NO equals eso.OVC_EDF_NO
                    select edf;
                query = query.Except(c);
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);

                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_EDF, dt);
            }
            else
                FCommon.AlertShow(PnWarning, "danger", "系統訊息", strMessage);
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_DEPT_CDE", ViewState["OVC_DEPT_CDE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_EDF_NO", ViewState["OVC_EDF_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_START_PORT", ViewState["OVC_START_PORT"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_ARRIVE_PORT", ViewState["OVC_ARRIVE_PORT"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", ViewState["OVC_TRANSER_DEPT_CDE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_IS_STRATEGY", ViewState["OVC_IS_STRATEGY"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_REVIEW_STATUS", ViewState["OVC_REVIEW_STATUS"], true);
            FCommon.setQueryString(ref strQueryString, "EDF_SN", ViewState["EDF_SN"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_APPLY_DATE_S", ViewState["ODT_APPLY_DATE_S"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_APPLY_DATE_E", ViewState["ODT_APPLY_DATE_E"], true);
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
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_ONNAME, txtOVC_CHI_NAME, txtODT_APPLY_DATE_S, txtODT_APPLY_DATE_E);
                    #region 匯入下拉式選單
                    string strFirstText = "不限定";
                    CommonMTS.list_dataImport_PORT(drpOVC_START_PORT, true, strFirstText); //啟運港(機場)
                    CommonMTS.list_dataImport_TransferArea(drpOVC_TRANSER_DEPT_CDE, true, strFirstText); //接轉地區
                    CommonMTS.list_dataImport_IS_STRATEGY(drpOVC_IS_STRATEGY, true, strFirstText); //戰略性高科技貨品
                    string[] strREVIEW_STATUS_List = { "通過", "剔退" };
                    FCommon.list_dataImport(drpOVC_REVIEW_STATUS, strREVIEW_STATUS_List, strREVIEW_STATUS_List, strFirstText, "", true); //審核狀況
                    #endregion

                    bool boolImport = false;
                    string strOVC_DEPT_CDE, strOVC_EDF_NO, strOVC_START_PORT, strOVC_ARRIVE_PORT, strOVC_TRANSER_DEPT_CDE, strOVC_IS_STRATEGY, strOVC_REVIEW_STATUS, strEDF_SN, strODT_APPLY_DATE_S, strODT_APPLY_DATE_E;
                    if (FCommon.getQueryString(this, "OVC_DEPT_CDE", out strOVC_DEPT_CDE, true))
                    {
                        txtOVC_DEPT_CDE.Value = strOVC_DEPT_CDE;
                        TBMDEPT dept = GME.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(strOVC_DEPT_CDE)).FirstOrDefault();
                        if (dept != null) txtOVC_ONNAME.Text = dept.OVC_ONNAME;
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_EDF_NO", out strOVC_EDF_NO, true))
                        txtOVC_EDF_NO.Text = strOVC_EDF_NO;
                    if (FCommon.getQueryString(this, "OVC_START_PORT", out strOVC_START_PORT, true))
                        FCommon.list_setValue(drpOVC_START_PORT, strOVC_START_PORT);
                    if (FCommon.getQueryString(this, "OVC_ARRIVE_PORT", out strOVC_ARRIVE_PORT, true))
                    {
                        txtOVC_PORT_CDE.Text = strOVC_ARRIVE_PORT;
                        TBGMT_PORTS tPort = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(strOVC_ARRIVE_PORT)).FirstOrDefault();
                        if (tPort != null) txtOVC_CHI_NAME.Text = tPort.OVC_PORT_CHI_NAME;
                    }
                    if (FCommon.getQueryString(this, "OVC_TRANSER_DEPT_CDE", out strOVC_TRANSER_DEPT_CDE, true))
                        FCommon.list_setValue(drpOVC_TRANSER_DEPT_CDE, strOVC_TRANSER_DEPT_CDE);
                    if (FCommon.getQueryString(this, "OVC_IS_STRATEGY", out strOVC_IS_STRATEGY, true))
                        FCommon.list_setValue(drpOVC_IS_STRATEGY, strOVC_IS_STRATEGY);
                    if (FCommon.getQueryString(this, "OVC_REVIEW_STATUS", out strOVC_REVIEW_STATUS, true))
                        FCommon.list_setValue(drpOVC_REVIEW_STATUS, strOVC_REVIEW_STATUS);
                    if (FCommon.getQueryString(this, "EDF_SN", out strEDF_SN, true))
                        FCommon.list_setValue(drpEDF_SN, strEDF_SN);
                    if (FCommon.getQueryString(this, "ODT_APPLY_DATE_S", out strODT_APPLY_DATE_S, true))
                        txtODT_APPLY_DATE_S.Text = strODT_APPLY_DATE_S;
                    if (FCommon.getQueryString(this, "ODT_APPLY_DATE_E", out strODT_APPLY_DATE_E, true))
                        txtODT_APPLY_DATE_E.Text = strODT_APPLY_DATE_E;
                    if (boolImport) dataImport();
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }
        
        protected void btnResettxtOVC_DEPT_CDE_Click(object sender, EventArgs e)
        {
            txtOVC_DEPT_CDE.Value = string.Empty;
            txtOVC_ONNAME.Text = string.Empty;
        }
        protected void btnResetPort_Click(object sender, EventArgs e)
        {
            txtOVC_PORT_CDE.Text = string.Empty;
            txtOVC_CHI_NAME.Text = string.Empty;
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_A23_2{ getQueryString() }");
        }
        
        protected void GVTBGMT_EDF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GVTBGMT_EDF.DataKeys[gvrIndex].Value.ToString();
            Guid.TryParse(id, out Guid guidEDF_SN);

            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);

            switch (e.CommandName)
            {
                case "btnNew":
                    TBGMT_EDF edf = MTSE.TBGMT_EDF.Where(table => table.EDF_SN.Equals(guidEDF_SN)).FirstOrDefault();
                    if (edf != null)
                    {
                        string strOVC_EDF_NO = edf.OVC_EDF_NO;
                        string strOVC_REVIEW_STATUS = edf.OVC_REVIEW_STATUS ?? "";
                        if (strOVC_REVIEW_STATUS.Equals("通過"))
                            Response.Redirect($"MTS_A23_3{ strQueryString }");
                        else
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"編號：{ strOVC_EDF_NO } 之外運資料表 尚未審核通過，無法登錄訂艙單");
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "外運資料表 不存在！");
                    break;
            }
        }
        protected void GVTBGMT_EDF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                GridView theGridView = (GridView)sender;
                int index = gvr.RowIndex;
                HyperLink hlkOVC_EDF_NO = (HyperLink)gvr.FindControl("hlkOVC_EDF_NO");
                if (FCommon.Controls_isExist(hlkOVC_EDF_NO))
                {
                    string strOVC_EDF_NO = theGridView.DataKeys[index].Value.ToString();
                    hlkOVC_EDF_NO.NavigateUrl = $"javascript: OpenWindow_EDFDATA('{ FCommon.getEncryption(strOVC_EDF_NO) }');";
                }
            }
        }
        protected void GVTBGMT_EDF_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}