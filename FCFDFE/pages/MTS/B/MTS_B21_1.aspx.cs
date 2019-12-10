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
    public partial class MTS_B21_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();

        #region 副程式
        private void dataImport()
        {
            string strOVC_EDF_NO = txtOVC_EDF_NO.Text;
            ViewState["OVC_EDF_NO"] = strOVC_EDF_NO;

            if (!string.IsNullOrEmpty(strOVC_EDF_NO))
            {
                var queryEDF =
                    from edf in MTSE.TBGMT_EDF.AsEnumerable()
                    where edf.OVC_EDF_NO.Contains(strOVC_EDF_NO)
                    where edf.OVC_REVIEW_STATUS != null && edf.OVC_REVIEW_STATUS.Equals("通過") //審核通過
                    where (edf.OVC_PAYMENT_TYPE != null ? edf.OVC_PAYMENT_TYPE.Equals("預付") : edf.OVC_PAYMENT_TYPE != null)
                        || (edf.OVC_IS_PAY != null ? edf.OVC_IS_PAY.Equals("1") : edf.OVC_IS_PAY != null) //須為預付 or 到付－投保

                    join eso in MTSE.TBGMT_ESO on edf.OVC_EDF_NO equals eso.OVC_EDF_NO into esoTemp
                    from eso in esoTemp.DefaultIfEmpty()
                    join portStr in MTSE.TBGMT_PORTS on edf.OVC_START_PORT equals portStr.OVC_PORT_CDE into portStrTemp
                    from portStr in portStrTemp.DefaultIfEmpty()
                    join portArr in MTSE.TBGMT_PORTS on edf.OVC_ARRIVE_PORT equals portArr.OVC_PORT_CDE into portArrTemp
                    from portArr in portArrTemp.DefaultIfEmpty()
                    join dept in GME.TBMDEPTs on edf.OVC_DEPT_CDE equals dept.OVC_DEPT_CDE into deptTemp
                    from dept in deptTemp.DefaultIfEmpty()
                    select new
                    {
                        edf.EDF_SN,
                        edf.OVC_EDF_NO,
                        edf.OVC_PURCH_NO,
                        OVC_START_PORT = portStr != null ? portStr.OVC_PORT_CHI_NAME : "",
                        OVC_ARRIVE_PORT = portArr != null ? portArr.OVC_PORT_CHI_NAME : "",
                        OVC_DEPT_CDE = dept != null ? dept.OVC_ONNAME : "",
                        OVC_PAYMENT_TYPE = edf.OVC_PAYMENT_TYPE,
                        OVC_IS_STRATEGY = edf.OVC_IS_STRATEGY,
                        OVC_REVIEW_STATUS = edf.OVC_REVIEW_STATUS
                    };
                var queryEINN =
                    from edf in queryEDF
                    join einn in MTSE.TBGMT_EINN on edf.OVC_EDF_NO equals einn.OVC_EDF_NO
                    select edf;
                //var queryEINN =
                //    from einn in MTSE.TBGMT_EINN
                //    join edf in MTSE.TBGMT_EDF on strOVC_EDF_NOeinn.OVC_EDF_NO equals edf.OVC_EDF_NO
                //    where einn.OVC_EDF_NO.Equals()
                //    select edf;
                var query = queryEDF.Except(queryEINN);
                //var query =
                //    from f in MTSE.TBGMT_EDF
                //    where f.OVC_EDF_NO.Contains(strOVC_EDF_NO)
                //    where f.OVC_REVIEW_STATUS == ("通過")
                //    select f;
                //var query_einn = 
                //    from e in MTSE.TBGMT_EINN
                //    join f in MTSE.TBGMT_EDF on e.OVC_EDF_NO equals f.OVC_EDF_NO
                //    where e.OVC_EDF_NO.Contains(strOVC_EDF_NO)
                //    select f;
                //var query_eso =
                //    from s in MTSE.TBGMT_ESO
                //    join f in MTSE.TBGMT_EDF on s.OVC_EDF_NO equals f.OVC_EDF_NO
                //    where s.OVC_EDF_NO.Contains(strOVC_EDF_NO)
                //    select f;
                //query = query.Except(query_einn).Except(query_eso);
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);

                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_EDF, dt);
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請輸入 外運資料表編號！");
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_EDF_NO", ViewState["OVC_EDF_NO"], true);
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
                    bool boolImport = false;
                    if(FCommon.getQueryString(this, "OVC_EDF_NO", out string strOVC_EDF_NO, true))
                    {
                        txtOVC_EDF_NO.Text = strOVC_EDF_NO;
                        boolImport = true;
                    }
                    if (boolImport) dataImport();
                }
            }
        }

        #region ~Click
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }
        #endregion

        #region GridView
        protected void GV_TBGMT_EDF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBGMT_EDF.DataKeys[gvrIndex].Value.ToString();
            //string key = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(id));

            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);

            switch (e.CommandName)
            {
                case "dataNew":
                    Response.Redirect($"MTS_B21_2{ strQueryString }");
                    break;
            }
        }
        protected void GV_TBGMT_EDF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                GridView theGridView = (GridView)sender;
                int index = gvr.RowIndex;
                HyperLink hlkOVC_EDF_NO = (HyperLink)gvr.FindControl("hlkOVC_EDF_NO");
                if (FCommon.Controls_isExist(hlkOVC_EDF_NO))
                {
                    string strEDF_SN = theGridView.DataKeys[index].Value.ToString();
                    hlkOVC_EDF_NO.NavigateUrl = $"javascript: OpenWindow_EDFDATA('{ FCommon.getEncryption(strEDF_SN) }');";
                }
            }
        }
        protected void GV_TBGMT_EDF_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        #endregion
    }
}