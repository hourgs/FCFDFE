using System;
using System.Linq;
using System.Web.UI;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A28_3 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        Common FCommon = new Common();
        string id;

        #region 副程式
        private void dataImport()
        {
            TBGMT_ECL ECL = MTSE.TBGMT_ECL.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
            if (ECL != null)
            {
                lblOVC_CLASS_CDE.Text = ECL.OVC_CLASS_CDE;
                lblOVC_CLASS_NAME.Text = ECL.OVC_CLASS_NAME;
                lblOVC_ECL_NO.Text = ECL.OVC_ECL_NO;
                lblOVC_EXP_TYPE.Text = ECL.OVC_EXP_TYPE;
                lblOVC_SHIP_CDE.Text = ECL.OVC_SHIP_CDE;
                lblOVC_PACK_NO.Text = ECL.OVC_PACK_NO;
                lblODT_EXP_DATE.Text = FCommon.getDateTime(ECL.ODT_EXP_DATE);
                lblOVC_STORED_PLACE.Text = ECL.OVC_STORED_PLACE;
            }

            var query =
                from edf in MTSE.TBGMT_EDF
                join edf_detail in MTSE.TBGMT_EDF_DETAIL on edf.OVC_EDF_NO equals edf_detail.OVC_EDF_NO
                //from edf in MTSE.TBGMT_EDF.AsEnumerable()
                //join edf_detail in MTSE.TBGMT_EDF_DETAIL.AsEnumerable() on edf.OVC_EDF_NO equals edf_detail.OVC_EDF_NO
                where edf.OVC_BLD_NO.Equals(id)
                select new
                {
                    OVC_ENG_NAME = edf_detail.OVC_ENG_NAME,
                    OVC_CHI_NAME = edf_detail.OVC_CHI_NAME,
                    OVC_ITEM_NO = edf_detail.OVC_ITEM_NO,
                    OVC_ITEM_NO2 = edf_detail.OVC_ITEM_NO2,
                    OVC_ITEM_NO3 = edf_detail.OVC_ITEM_NO3,
                    ONB_ITEM_COUNT = edf_detail.ONB_ITEM_COUNT,
                    OVC_ITEM_COUNT_UNIT = edf_detail.OVC_ITEM_COUNT_UNIT,
                    ONB_WEIGHT = edf_detail.ONB_WEIGHT,
                    OVC_WEIGHT_UNIT = edf_detail.OVC_WEIGHT_UNIT,
                    ONB_VOLUME = edf_detail.ONB_VOLUME,
                    OVC_VOLUME_UNIT = edf_detail.OVC_VOLUME_UNIT,
                    ONB_BULK = (edf_detail.ONB_LENGTH) * (edf_detail.ONB_WIDTH) * (edf_detail.ONB_HEIGHT),
                    ONB_MONEY = edf_detail.ONB_MONEY,
                    OVC_CURRENCY = edf_detail.OVC_CURRENCY
                };

            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_EDF_DETAIL, dt);
        }

        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", Request.QueryString["OVC_BLD_NO"], false);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getQueryString(this, "id", out id, true);
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);

                if (!IsPostBack)
                {
                    lblOVC_BLD_NO.Text = id;
                    dataImport();
                }
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                string strUserId = Session["userid"].ToString();
                TBGMT_ECL ecl = MTSE.TBGMT_ECL.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
                if (ecl != null)
                {
                    MTSE.Entry(ecl).State = EntityState.Deleted;
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), ecl.GetType().Name, this, "刪除");

                    FCommon.AlertShow(PnDelete, "success", "系統訊息", $"提單編號：{ id } 之出口報單 刪除成功。");
                }
                else
                    FCommon.AlertShow(PnDelete, "danger", "系統訊息", $"提單編號：{ id } 之出口報單 已被刪除，不存在！");
            }
            catch
            {
                FCommon.AlertShow(PnDelete, "danger", "系統訊息", "刪除失敗，請聯絡工程師。");
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_A28_1{ getQueryString() }");
        }

        protected void GVTBGMT_EDF_DETAIL_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}