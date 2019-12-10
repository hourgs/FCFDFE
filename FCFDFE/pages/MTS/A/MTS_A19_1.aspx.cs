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
    public partial class MTS_A19_1 : Page
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
            string strOVC_DEPT_CDE = txtOVC_DEPT_CDE.Value;
            string strOVC_TRANSER_DEPT_CDE = drpOVC_TRANSER_DEPT_CDE.SelectedValue.ToString();
            ViewState["OVC_BLD_NO"] = strOVC_BLD_NO;
            ViewState["OVC_DEPT_CDE"] = strOVC_DEPT_CDE;
            ViewState["OVC_TRANSER_DEPT_CDE"] = strOVC_TRANSER_DEPT_CDE;

            if (strOVC_BLD_NO.Equals(string.Empty) && strOVC_DEPT_CDE.Equals(string.Empty) && strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                strMessage += "<P> 至少填入一個選項 </p>";

            if (strMessage.Equals(string.Empty))
            {
                var query =
                    from bld in MTSE.TBGMT_BLD.AsEnumerable().Distinct()
                    join icr in MTSE.TBGMT_ICR.AsEnumerable() on bld.OVC_BLD_NO equals icr.OVC_BLD_NO
                    join irdtail in MTSE.TBGMT_IRD_DETAIL.AsEnumerable() on bld.OVC_BLD_NO equals irdtail.OVC_BLD_NO
                    where irdtail.OVC_IHO_NO == null
                    join dept in MTSE.TBMDEPTs on irdtail.OVC_DEPT_CDE equals dept.OVC_DEPT_CDE into deptTemp
                    from dept in deptTemp.DefaultIfEmpty()
                    join arrport in MTSE.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals arrport.OVC_PORT_CDE
                    where arrport.OVC_IS_ABROAD.Equals("國內")
                    select new
                    {
                        OVC_IRDDETAIL_SN = irdtail.OVC_IRDDETAIL_SN,
                        OVC_BLD_NO = bld.OVC_BLD_NO,
                        OVC_DEPT_CDE = irdtail.OVC_DEPT_CDE,
                        bld.OVC_ARRIVE_PORT,

                        OVC_PURCH_NO = irdtail.OVC_PURCH_NO,
                        //OVC_ITEM_TYPE = bld.OVC_ITEM_TYPE,
                        icr.OVC_CHI_NAME,
                        OVC_ONNAME = dept != null ? dept.OVC_ONNAME : "",
                        OVC_BOX_NO = irdtail.OVC_BOX_NO,
                        ONB_ACTUAL_RECEIVE = irdtail.ONB_ACTUAL_RECEIVE
                    };
                if (!strOVC_BLD_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
                if (!strOVC_DEPT_CDE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_DEPT_CDE.Contains(strOVC_DEPT_CDE));
                if (!strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                {
                    var table = GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("TR"))
                               .Where(t => t.OVC_PHR_PARENTS.Equals(strOVC_TRANSER_DEPT_CDE))
                               .Select(t => t.OVC_PHR_ID).ToArray();

                    query = query.Where(t => table.Contains(t.OVC_ARRIVE_PORT));
                }
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);

                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_IHO, dt);
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", ViewState["OVC_BLD_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_DEPT_CDE", ViewState["OVC_DEPT_CDE"], true);
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
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_DEPT_CDE, txtOVC_ONNAME);
                    #region 匯入下拉式選單
                    string strFirstText = "不限定";
                    CommonMTS.list_dataImport_TransferArea(drpOVC_TRANSER_DEPT_CDE, true, strFirstText); //接轉地區
                    #endregion

                    bool isImport = false;
                    if (FCommon.getQueryString(this, "OVC_BLD_NO", out string strOVC_BLD_NO, true))
                    {
                        txtOVC_BLD_NO.Text = strOVC_BLD_NO;
                        isImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_DEPT_CDE", out string strOVC_DEPT_CDE, true))
                    {
                        txtOVC_DEPT_CDE.Value = strOVC_DEPT_CDE;
                        TBMDEPT detp = GME.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(strOVC_DEPT_CDE)).FirstOrDefault();
                        if (detp != null) txtOVC_ONNAME.Text = detp.OVC_ONNAME;
                    }
                    if (FCommon.getQueryString(this, "OVC_TRANSER_DEPT_CDE", out string strOVC_TRANSER_DEPT_CDE, true))
                        FCommon.list_setValue(drpOVC_TRANSER_DEPT_CDE, strOVC_TRANSER_DEPT_CDE);
                    if (isImport) dataImport();
                }
            }
        }

#region ~Click
        protected void btnResetOVC_DEPT_CDE_CODE_Click(object sender, EventArgs e)
        {
            txtOVC_DEPT_CDE.Value = string.Empty;
            txtOVC_ONNAME.Text = string.Empty;
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            string checkID = "";
            for (int i = 0; i < GVTBGMT_IHO.Rows.Count; i++)
            {
                GridViewRow gvr = GVTBGMT_IHO.Rows[i];
                CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSelect");
                if (FCommon.Controls_isExist(chkSelect) && chkSelect.Checked)
                {
                    checkID += checkID.Equals(string.Empty) ? "" : " ";
                    string id = GVTBGMT_IHO.DataKeys[gvr.RowIndex].Value.ToString();
                    checkID += id;
                }
            }
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", checkID, true);
            if (!checkID.Equals(string.Empty))
                Response.Redirect($"MTS_A19_2{ strQueryString }");
            else
               FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先選取");
        }
#endregion
        
        protected void GVTBGMT_IHO_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void GVTBGMT_IHO_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}