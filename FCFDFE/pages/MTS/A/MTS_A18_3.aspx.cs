using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A18_3 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();

        #region 副程式
        private void dataImport(string strOVC_BLD_NO) //載入本頁基本資料
        {
            //資料匯入
            TBGMT_BLD bldTable = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
            TBGMT_ICR icrTable = MTSE.TBGMT_ICR.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
            TBGMT_IRD irdTable = MTSE.TBGMT_IRD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
            lblOVC_BLD_NO.Text = strOVC_BLD_NO;
            if (bldTable != null)
            {
                lblONB_QUANITY.Text = bldTable.ONB_QUANITY.ToString();
                lblOVC_QUANITY_UNIT.Text = bldTable.OVC_QUANITY_UNIT;
                lblOVC_SHIP_NAME.Text = bldTable.OVC_SHIP_NAME;
                lblONB_WEIGHT.Text = bldTable.ONB_WEIGHT.ToString();
                lblOVC_WEIGHT_UNIT.Text = bldTable.OVC_WEIGHT_UNIT;
                lblOVC_VOYAGE.Text = bldTable.OVC_VOYAGE;
                lblONB_VOLUME.Text = bldTable.ONB_VOLUME.ToString();
                lblOVC_VOLUME_UNIT.Text = bldTable.OVC_VOLUME_UNIT;
            }
            if (icrTable != null)
                lblOVC_PURCH_NO.Text = icrTable.OVC_PURCH_NO;
            if (irdTable != null)
            {
                lblODT_ARRIVE_PORT_DATE.Text = FCommon.getDateTime(irdTable.ODT_ARRIVE_PORT_DATE); //到港日期 => 進口日期
                lblODT_CLEAR_DATE.Text = FCommon.getDateTime(irdTable.ODT_CLEAR_DATE); //清檢日期 => 提單/拆櫃日期

                lblONB_ACTUAL_RECEIVE.Text = irdTable.ONB_ACTUAL_RECEIVE.ToString(); //實收
                lblONB_OVERFLOW.Text = irdTable.ONB_OVERFLOW.ToString(); //溢卸
                lblONB_LESS.Text = irdTable.ONB_LESS.ToString(); //短少
                lblONB_BROKEN.Text = irdTable.ONB_BROKEN.ToString(); //破損
                lblOVC_NOTE.Text = irdTable.OVC_NOTE; //備考
            }
        }
        private void dataImport_CTN() //載入貨櫃資料表GV
        {
            //資料匯入
            string strOVC_BLD_NO = lblOVC_BLD_NO.Text;
            if (!strOVC_BLD_NO.Equals(string.Empty))
            {
                var query =
                from IrdCtnTable in MTSE.TBGMT_IRD_CTN
                where IrdCtnTable.OVC_BLD_NO.Equals(strOVC_BLD_NO)
                select IrdCtnTable;

                DataTable dt = query.ListToDataTable();
                ViewState["hasRows_CTN"] = FCommon.GridView_dataImport(GVTBGMT_CTN, dt);
            }
        }
        private void dataImport_DETAIL() //載入分運明細資料表GV
        {
            //資料匯入
            string strOVC_BLD_NO = lblOVC_BLD_NO.Text;
            if (!strOVC_BLD_NO.Equals(string.Empty))
            {
                var query =
                    from IrdDetailTable in MTSE.TBGMT_IRD_DETAIL
                    join dept in MTSE.TBMDEPTs on IrdDetailTable.OVC_DEPT_CDE equals dept.OVC_DEPT_CDE into deptTemp
                    from dept in deptTemp.DefaultIfEmpty()
                    where IrdDetailTable.OVC_BLD_NO.Equals(strOVC_BLD_NO)
                    select new
                    {
                        IrdDetailTable.OVC_IRDDETAIL_SN,
                        IrdDetailTable.OVC_DEPT_CDE,
                        OVC_ONNAME = dept != null ? dept.OVC_ONNAME : "",
                        IrdDetailTable.OVC_BOX_NO,
                        IrdDetailTable.ONB_ACTUAL_RECEIVE,
                        IrdDetailTable.ONB_OVERFLOW,
                        IrdDetailTable.ONB_LESS,
                        IrdDetailTable.ONB_BROKEN
                    };
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows_DETAIL"] = FCommon.GridView_dataImport(GVTBGMT_DETAIL, dt);
            }
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", Request.QueryString["OVC_BLD_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", Request.QueryString["OVC_TRANSER_DEPT_CDE"], false);
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
                    if (FCommon.getQueryString(this, "id", out string id, true)) //判斷讀取的到提單號碼
                    {
                        dataImport(id);
                        dataImport_CTN();
                        dataImport_DETAIL();
                    }
                    else
                        FCommon.MessageBoxShow(this, "提單編號錯誤！", $"MTS_A18_1{ getQueryString() }", false);
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string strOVC_BLD_NO = lblOVC_BLD_NO.Text;
            if (!strOVC_BLD_NO.Equals(string.Empty))
            {
                string strMessage = "";

                TBGMT_IRD irdTable = MTSE.TBGMT_IRD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                if (irdTable == null) strMessage += "<p> 此筆 接配紀錄表 不存在 </p>";

                if (strMessage.Equals(string.Empty))
                {
                    MTSE.Entry(irdTable).State = EntityState.Deleted;
                    MTSE.SaveChanges(); //儲存
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), irdTable.GetType().Name.ToString(), this, "刪除");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", $"提單編號：{ strOVC_BLD_NO } 刪除接配紀錄表成功");
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_A18_1{ getQueryString() }");
        }

        #region GV事件
        protected void GVTBGMT_CTN_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows_CTN"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        protected void GVTBGMT_DETAIL_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows_DETAIL"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        #endregion
    }
}