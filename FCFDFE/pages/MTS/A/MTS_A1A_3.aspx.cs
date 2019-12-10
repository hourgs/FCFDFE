using System;
using System.Linq;
using System.Web.UI;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A1A_3 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();

        #region 副程式
        private void dateImport(string strOVC_IHO_NO)
        {
            TBGMT_IHO IHO = MTSE.TBGMT_IHO.Where(table => table.OVC_IHO_NO.Equals(strOVC_IHO_NO)).FirstOrDefault();
            if (IHO != null)
            {
                lblOVC_IHO_NO.Text = IHO.OVC_IHO_NO;
                lblbOVC_TRANS_TYPE.Text = IHO.OVC_INLAND_TRANS_TYPE;
                lblOVC_START_PLACE.Text = IHO.OVC_START_PLACE;
                lblOVC_ARRIVE_PLACE.Text = IHO.OVC_ARRIVE_PLACE;
                lblODT_START_DATE.Text = FCommon.getDateTime(IHO.ODT_START_DATE);
                lblODT_ARRIVE_DATE.Text = FCommon.getDateTime(IHO.ODT_ARRIVE_DATE);
                lblOVC_RECEIVE_DEPT_CDE.Text = IHO.OVC_RECEIVE_DEPT_CDE;
                lblOVC_TRANSER_DEPT_CDE.Text = IHO.OVC_TRANSER_DEPT_CDE;
                lblONB_TOTAL_QUANITY.Text = IHO.ONB_QUANITY.ToString();
                lblOVC_QUANITY_UNIT.Text = IHO.OVC_QUANITY_UNIT;
                lblONB_TOTAL_VOLUME.Text = IHO.ONB_VOLUME.ToString();
                lblOVC_VOLUME_UNIT.Text = IHO.OVC_VOLUME_UNIT;
                lblONB_TOTAL_WEIGHT.Text = IHO.ONB_WEIGHT.ToString();
                lblOVC_WEIGHT_UNIT.Text = IHO.OVC_WEIGHT_UNIT;
                lblOVC_SHIP_NAME.Text = IHO.OVC_SHIP_NAME;
                lblOVC_VOYAGE.Text = IHO.OVC_VOYAGE;
                lblONB_OVERFLOW.Text = IHO.ONB_OVERFLOW.ToString();
                lblONB_LESS.Text = IHO.ONB_LESS.ToString();
                lblONB_BROKEN.Text = IHO.ONB_BROKEN.ToString();
                lblONB_ACTUAL_RECEIVE.Text = IHO.ONB_ACTUAL_RECEIVE.ToString();
                lblOVC_NOTE.Text = IHO.OVC_NOTE;
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"交接單編號：{ strOVC_IHO_NO } 之運輸交接單 不存在！");

            var query =
                from irdtail in MTSE.TBGMT_IRD_DETAIL.AsEnumerable()
                where irdtail.OVC_IHO_NO != null && irdtail.OVC_IHO_NO.Equals(strOVC_IHO_NO)
                join bld in MTSE.TBGMT_BLD.AsEnumerable() on irdtail.OVC_BLD_NO equals bld.OVC_BLD_NO
                join icr in MTSE.TBGMT_ICR.AsEnumerable() on irdtail.OVC_BLD_NO equals icr.OVC_BLD_NO
                select new
                {
                    OVC_BLD_NO = bld.OVC_BLD_NO,
                    OVC_PURCH_NO = irdtail.OVC_PURCH_NO,
                    //OVC_ITEM_TYPE = bld.OVC_ITEM_TYPE,
                    icr.OVC_CHI_NAME,
                    OVC_BOX_NO = irdtail.OVC_BOX_NO,
                    ONB_ACTUAL_RECEIVE = irdtail.ONB_ACTUAL_RECEIVE
                };

            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_IRD_DETAIL, dt);
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_IHO_NO", Request.QueryString["OVC_IHO_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", Request.QueryString["OVC_BLD_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", Request.QueryString["OVC_TRANSER_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_DEPT_CDE", Request.QueryString["OVC_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_START_DATE_S", Request.QueryString["ODT_START_DATE_S"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_START_DATE_E", Request.QueryString["ODT_START_DATE_E"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_ARRIVE_DATE_S", Request.QueryString["ODT_ARRIVE_DATE_S"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_ARRIVE_DATE_E", Request.QueryString["ODT_ARRIVE_DATE_E"], false);
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
                    //string id = Request.QueryString["OVC_IHO_NO"];
                    if (FCommon.getQueryString(this, "id", out string id, true))
                    {
                        dateImport(id);
                    }
                }
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            string strOVC_IHO_NO = lblOVC_IHO_NO.Text;
            TBGMT_IHO iho = MTSE.TBGMT_IHO.Where(table => table.OVC_IHO_NO.Equals(strOVC_IHO_NO)).FirstOrDefault();
            if (iho != null)
            {
                var query =
                    from tIrdDetail in MTSE.TBGMT_IRD_DETAIL
                    where tIrdDetail.OVC_IHO_NO.Equals(strOVC_IHO_NO)
                    select tIrdDetail;
                foreach (TBGMT_IRD_DETAIL theDetail in query)
                {
                    theDetail.OVC_IHO_NO = null;
                    MTSE.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), theDetail.GetType().Name.ToString(), this, "修改");
                }

                MTSE.Entry(iho).State = EntityState.Deleted;
                MTSE.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), iho.GetType().Name.ToString(), this, "刪除");

                FCommon.AlertShow(PnModify, "success", "系統訊息", $"交接單編號：{ strOVC_IHO_NO } 之運輸交接單 刪除成功。");
            }
            else
                FCommon.AlertShow(PnModify, "danger", "系統訊息", $"交接單編號：{ strOVC_IHO_NO } 之運輸交接單 不存在！");

            //for (int i = 0; 1 < GVTBGMT_IRD_DETAIL.Rows.Count; i++)
            //{
            //    var IRDmodel = new TBGMT_IRD { OVC_BLD_NO = GVTBGMT_IRD_DETAIL.Rows[i].Cells[1].Text };
            //    MTSE.Entry(IRDmodel).State = EntityState.Deleted;
            //}

            //MTSE.Entry(IRDdtailmodel).State = EntityState.Deleted;
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_A1A_1{ getQueryString() }");
        }
        
        protected void GVTBGMT_IRD_DETAIL_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}