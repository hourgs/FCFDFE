using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A28_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        public Common FCommon = new Common();
        string strDateFormat = Variable.strDateFormat;

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
                var query =
                    //from ecl in MTSE.TBGMT_ECL
                    from ecl in MTSE.TBGMT_ECL.AsEnumerable()
                    where ecl.OVC_ECL_NO != null
                    select new
                    {
                        OVC_BLD_NO = ecl.OVC_BLD_NO,
                        OVC_CLASS_CDE = ecl.OVC_CLASS_CDE,
                        OVC_CLASS_NAME = ecl.OVC_CLASS_NAME,
                        OVC_ECL_NO = ecl.OVC_ECL_NO,
                        OVC_EXP_TYPE = ecl.OVC_EXP_TYPE,
                        OVC_SHIP_CDE = ecl.OVC_SHIP_CDE,
                        OVC_PACK_NO = ecl.OVC_PACK_NO,
                        ODT_EXP_DATE = FCommon.getDateTime( ecl.ODT_EXP_DATE),
                        OVC_STORED_PLACE = ecl.OVC_STORED_PLACE
                    };
                if (!strOVC_BLD_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);

                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_ECL, dt);
            }
            else
                FCommon.AlertShow(PnWarning, "danger", "系統訊息", strMessage);
        }

        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", ViewState["OVC_BLD_NO"], true);
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

        protected void GVTBGMT_ECL_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GVTBGMT_ECL.DataKeys[gvrIndex].Value.ToString();

            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);

            switch (e.CommandName)
            {
                case "btnModify":
                    Response.Redirect($"MTS_A28_2{ strQueryString }");
                    break;
                case "btnDel":
                    Response.Redirect($"MTS_A28_3{ strQueryString }");
                    break;
                default:
                    break;
            }
        }
        
        protected void GVTBGMT_ECL_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void GVTBGMT_ECL_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}