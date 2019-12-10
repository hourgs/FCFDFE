using System;
using System.Linq;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.Data.Entity;
using System.Web.UI;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A14_3 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        string id;

        #region 副程式
        private void dataImport()
        {
            TBGMT_BLD BLD = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
            if (BLD != null)
            {
                lblOVC_BLD_NO.Text = BLD.OVC_BLD_NO;
                lblOVC_SHIP_NAME.Text = BLD.OVC_SHIP_NAME;
                lblOVC_VOYAGE.Text = BLD.OVC_VOYAGE;
                FCommon.list_setValue(drpOVC_IS_SECURITY, (BLD.OVC_IS_SECURITY ?? 0).ToString());
            }
            TBGMT_ICR ICR = MTSE.TBGMT_ICR.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
            if (ICR != null)
            {
                TBMDEPT tbmdept = GME.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(ICR.OVC_RECEIVE_DEPT_CODE)).FirstOrDefault();
                if (tbmdept != null) lblOVC_RECEIVE_DEPT_CODE.Text = tbmdept.OVC_ONNAME;
                lblOVC_PURCH_NO.Text = ICR.OVC_PURCH_NO;
                lblOVC_CHI_NAME.Text = ICR.OVC_CHI_NAME;
                lblODT_RECEIVE_INF_DATE.Text = FCommon.getDateTime(ICR.ODT_RECEIVE_INF_DATE);
                lblOVC_TRANS_TYPE.Text = ICR.OVC_TRANS_TYPE;
                lblODT_IMPORT_DATE.Text = FCommon.getDateTime(ICR.ODT_IMPORT_DATE);
                lblODT_PASS_CUSTOM_DATE.Text = FCommon.getDateTime(ICR.ODT_PASS_CUSTOM_DATE);
                lblODT_ABROAD_CUSTOM_DATE.Text = FCommon.getDateTime(ICR.ODT_ABROAD_CUSTOM_DATE);
                lblODT_UNPACKING_DATE.Text = FCommon.getDateTime(ICR.ODT_UNPACKING_DATE);
                lblODT_CHANGE_BLD_DATE.Text = FCommon.getDateTime(ICR.ODT_CHANGE_BLD_DATE);
                lblODT_TRANSFER_DATE.Text = FCommon.getDateTime(ICR.ODT_TRANSFER_DATE); 
                lblODT_CUSTOM_DATE.Text = FCommon.getDateTime(ICR.ODT_CUSTOM_DATE);
                lblODT_RECEIVE_DATE.Text = FCommon.getDateTime(ICR.ODT_RECEIVE_DATE);
                lblOVC_NOTE.Text = ICR.OVC_NOTE;
            }
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", Request.QueryString["OVC_BLD_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", Request.QueryString["OVC_TRANSER_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_SHIP_NAME", Request.QueryString["OVC_SHIP_NAME"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_VOYAGE", Request.QueryString["OVC_VOYAGE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_DEPT_CDE", Request.QueryString["OVC_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_IS_SECURITY", Request.QueryString["OVC_IS_SECURITY"], false);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if(FCommon.getQueryString(this, "id", out id, true))
                {
                    if (!IsPostBack)
                    {
                        #region 匯入下拉式選單
                        CommonMTS.list_dataImport_IS_SECURITY(drpOVC_IS_SECURITY, false);//機敏軍品
                        #endregion

                        dataImport();
                    }
                }
            }
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_A14_1{ getQueryString() }");
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            DateTime dateNow = DateTime.Now;
            TBGMT_BLD BLD = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
            TBGMT_ICR ICR = MTSE.TBGMT_ICR.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
            #region 錯誤訊息
            if (BLD == null) strMessage += $"<P> 提單編號：{ id } 之提單資料檔 不存在！ </p>";
            if (ICR == null) strMessage += $"<P> 提單編號：{ id } 之時程管制簿 不存在！ </p>";
            #endregion
            if (strMessage.Equals(string.Empty))
            {
                try
                {
                    #region TBGMT_ICR_MRPLOG 刪除LOG
                    TBGMT_ICR_MRPLOG ICR_LOG = new TBGMT_ICR_MRPLOG();
                    ICR_LOG.LOG_LOGIN_ID = strUserId;
                    ICR_LOG.LOG_EVENT = "DELETE";
                    ICR_LOG.LOG_TIME = dateNow;
                    ICR_LOG.OVC_BLD_NO = ICR.OVC_BLD_NO;
                    ICR_LOG.OVC_PURCH_NO = ICR.OVC_PURCH_NO;
                    ICR_LOG.OVC_CHI_NAME = ICR.OVC_CHI_NAME;
                    ICR_LOG.OVC_RECEIVE_DEPT_CODE = ICR.OVC_RECEIVE_DEPT_CODE;
                    ICR_LOG.ODT_RECEIVE_INF_DATE = ICR.ODT_RECEIVE_INF_DATE;
                    ICR_LOG.OVC_TRANS_TYPE = ICR.OVC_TRANS_TYPE;
                    ICR_LOG.ODT_IMPORT_DATE = ICR.ODT_IMPORT_DATE;
                    ICR_LOG.ODT_PASS_CUSTOM_DATE = ICR.ODT_PASS_CUSTOM_DATE;
                    ICR_LOG.ODT_ABROAD_CUSTOM_DATE = ICR.ODT_ABROAD_CUSTOM_DATE;
                    ICR_LOG.ODT_UNPACKING_DATE = ICR.ODT_UNPACKING_DATE;
                    ICR_LOG.ODT_CHANGE_BLD_DATE = ICR.ODT_CHANGE_BLD_DATE;
                    ICR_LOG.ODT_TRANSFER_DATE = ICR.ODT_TRANSFER_DATE;
                    ICR_LOG.ODT_CUSTOM_DATE = ICR.ODT_CUSTOM_DATE;
                    ICR_LOG.ODT_RECEIVE_DATE = ICR.ODT_RECEIVE_DATE;
                    ICR_LOG.OVC_NOTE = ICR.OVC_NOTE;
                    ICR_LOG.ODT_MODIFY_DATE = ICR.ODT_MODIFY_DATE;
                    ICR_LOG.OVC_CREATE_LOGIN_ID = ICR.OVC_CREATE_LOGIN_ID;
                    ICR_LOG.OVC_ICRMRPLOG_SN = Guid.NewGuid();
                    MTSE.TBGMT_ICR_MRPLOG.Add(ICR_LOG);
                    MTSE.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), ICR_LOG.GetType().Name.ToString(), this, "新增");
                    #endregion

                    #region TBGMT_BLD_MRPLOG 刪除LOG 沒有刪除TBGMT_BLD 不需要產生LOG
                    //BLD_LOG.LOG_LOGIN_ID = Session["userid"].ToString();
                    //BLD_LOG.LOG_TIME = System.DateTime.Now;
                    //BLD_LOG.LOG_EVENT = "DELETE";
                    //BLD_LOG.OVC_BLD_NO = lblOVC_BLD_NO.Text;
                    //BLD_LOG.OVC_SHIP_COMPANY = codetable.OVC_SHIP_COMPANY;
                    //BLD_LOG.OVC_SEA_OR_AIR = codetable.OVC_SEA_OR_AIR;
                    //BLD_LOG.OVC_SHIP_NAME = codetable.OVC_SHIP_NAME;
                    //BLD_LOG.OVC_VOYAGE = codetable.OVC_VOYAGE;
                    //BLD_LOG.ODT_START_DATE = codetable.ODT_START_DATE;
                    //BLD_LOG.OVC_START_PORT = codetable.OVC_START_PORT;
                    //BLD_LOG.ODT_PLN_ARRIVE_DATE = codetable.ODT_PLN_ARRIVE_DATE;
                    //BLD_LOG.ODT_ACT_ARRIVE_DATE = codetable.ODT_ACT_ARRIVE_DATE;
                    //BLD_LOG.OVC_ARRIVE_PORT = codetable.OVC_ARRIVE_PORT;
                    //BLD_LOG.ONB_QUANITY = codetable.ONB_QUANITY;
                    //BLD_LOG.OVC_QUANITY_UNIT = codetable.OVC_QUANITY_UNIT;
                    //BLD_LOG.ONB_VOLUME = codetable.ONB_VOLUME;
                    //BLD_LOG.ONB_ON_SHIP_VOLUME = codetable.ONB_ON_SHIP_VOLUME;
                    //BLD_LOG.OVC_VOLUME_UNIT = codetable.OVC_VOLUME_UNIT;
                    //BLD_LOG.ONB_WEIGHT = codetable.ONB_WEIGHT;
                    //BLD_LOG.OVC_WEIGHT_UNIT = codetable.OVC_WEIGHT_UNIT;
                    //BLD_LOG.ONB_ITEM_VALUE = codetable.ONB_ITEM_VALUE;
                    //BLD_LOG.ONB_CARRIAGE_CURRENCY_I = codetable.ONB_CARRIAGE_CURRENCY_I;
                    //BLD_LOG.ONB_CARRIAGE = codetable.ONB_CARRIAGE;
                    //BLD_LOG.ONB_CARRIAGE_CURRENCY = codetable.ONB_CARRIAGE_CURRENCY;
                    //BLD_LOG.OVC_PAYMENT_TYPE = codetable.OVC_PAYMENT_TYPE;
                    //BLD_LOG.OVC_MILITARY_TYPE = codetable.OVC_MILITARY_TYPE;
                    //BLD_LOG.OVC_STATUS = codetable.OVC_STATUS;
                    //BLD_LOG.ODT_CREATE_DATE = codetable.ODT_CREATE_DATE;
                    //BLD_LOG.ODT_MODIFY_DATE = DateTime.Now;
                    //BLD_LOG.OVC_CREATE_LOGIN_ID = codetable.OVC_CREATE_LOGIN_ID;
                    //BLD_LOG.MRPLOG_SN = Guid.NewGuid();
                    //BLD_LOG.OVC_IS_SECURITY = Convert.ToByte(drpOVC_IS_SECURITY.SelectedValue);

                    //MTSE.TBGMT_BLD_MRPLOG.Add(BLD_LOG);
                    //MTSE.SaveChanges();
                    #endregion

                    MTSE.Entry(ICR).State = EntityState.Deleted;
                    MTSE.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), ICR.GetType().Name.ToString(), this, "刪除");

                    FCommon.AlertShow(PnDelete, "success", "系統訊息", $"提單編號：{ id } 之時程管制簿 刪除成功！");
                }
                catch
                {
                    FCommon.AlertShow(PnDelete, "danger", "系統訊息", "刪除失敗，請聯絡工程師。");
                }
            }
            else
                FCommon.AlertShow(PnDelete, "danger", "系統訊息", strMessage);
        }
    }
}