using System;
using System.Linq;
using System.Web.UI;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A14_2 : Page
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
                txtOVC_DEPT_CDE.Value = ICR.OVC_RECEIVE_DEPT_CODE;
                TBMDEPT tbmdept = GME.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(ICR.OVC_RECEIVE_DEPT_CODE)).FirstOrDefault();
                if (tbmdept != null) txtOVC_ONNAME.Text = tbmdept.OVC_ONNAME;
                txtOVC_PURCH_NO.Text = ICR.OVC_PURCH_NO;
                txtOVC_CHI_NAME.Text = ICR.OVC_CHI_NAME;
                txtODT_RECEIVE_INF_DATE.Text = FCommon.getDateTime(ICR.ODT_RECEIVE_INF_DATE);
                FCommon.list_setValue(drpOVC_TRANS_TYPE, ICR.OVC_TRANS_TYPE);
                txtODT_IMPORT_DATE.Text = FCommon.getDateTime(ICR.ODT_IMPORT_DATE);
                txtODT_PASS_CUSTOM_DATE.Text = FCommon.getDateTime(ICR.ODT_PASS_CUSTOM_DATE);
                txtODT_ABROAD_CUSTOM_DATE.Text = FCommon.getDateTime(ICR.ODT_ABROAD_CUSTOM_DATE);
                txtODT_UNPACKING_DATE.Text = FCommon.getDateTime(ICR.ODT_UNPACKING_DATE);
                txtODT_CHANGE_BLD_DATE.Text = FCommon.getDateTime(ICR.ODT_CHANGE_BLD_DATE);
                txtODT_TRANSFER_DATE.Text = FCommon.getDateTime(ICR.ODT_TRANSFER_DATE);
                txtODT_CUSTOM_DATE.Text = FCommon.getDateTime(ICR.ODT_CUSTOM_DATE);
                txtODT_RECEIVE_DATE.Text = FCommon.getDateTime(ICR.ODT_RECEIVE_DATE);
                txtOVC_NOTE.Text = ICR.OVC_NOTE;
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
                        FCommon.Controls_Attributes("readonly", "true", txtOVC_ONNAME, txtODT_RECEIVE_INF_DATE, txtODT_IMPORT_DATE, txtODT_PASS_CUSTOM_DATE, txtODT_ABROAD_CUSTOM_DATE,
                           txtODT_UNPACKING_DATE, txtODT_CHANGE_BLD_DATE, txtODT_TRANSFER_DATE, txtODT_CUSTOM_DATE, txtODT_RECEIVE_DATE);
                        #region 匯入下拉式選單
                        CommonMTS.list_dataImport_TRANS_TYPE(drpOVC_TRANS_TYPE, false); //清運方式
                        CommonMTS.list_dataImport_IS_SECURITY(drpOVC_IS_SECURITY, false);//機敏軍品
                        #endregion

                        dataImport();
                    }
                }
                
            }
            
        }

        protected void btnResetOVC_DEPT_CDE_CODE_Click(object sender, EventArgs e)
        {
            txtOVC_DEPT_CDE.Value = string.Empty;
            txtOVC_ONNAME.Text = string.Empty;
        }
        protected void btnModify_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strOVC_PURCH_NO = txtOVC_PURCH_NO.Text;
            string strOVC_CHI_NAME = txtOVC_CHI_NAME.Text;
            string strOVC_DEPT_CDE = txtOVC_DEPT_CDE.Value;
            string strODT_RECEIVE_INF_DATE = txtODT_RECEIVE_INF_DATE.Text;
            string strOVC_TRANS_TYPE = drpOVC_TRANS_TYPE.SelectedValue;
            string strODT_IMPORT_DATE = txtODT_IMPORT_DATE.Text;
            string strODT_PASS_CUSTOM_DATE = txtODT_PASS_CUSTOM_DATE.Text;
            string strODT_ABROAD_CUSTOM_DATE = txtODT_ABROAD_CUSTOM_DATE.Text;
            string strODT_UNPACKING_DATE = txtODT_UNPACKING_DATE.Text;
            string strODT_CHANGE_BLD_DATE = txtODT_CHANGE_BLD_DATE.Text;
            string strODT_TRANSFER_DATE = txtODT_TRANSFER_DATE.Text;
            string strODT_CUSTOM_DATE = txtODT_CUSTOM_DATE.Text;
            string strODT_RECEIVE_DATE = txtODT_RECEIVE_DATE.Text;
            string strOVC_IS_SECURITY = drpOVC_IS_SECURITY.SelectedValue;
            string strOVC_NOTE = txtOVC_NOTE.Text;

            DateTime dateODT_RECEIVE_INF_DATE, dateODT_IMPORT_DATE, dateODT_PASS_CUSTOM_DATE, dateODT_ABROAD_CUSTOM_DATE, dateODT_UNPACKING_DATE,
                dateODT_CHANGE_BLD_DATE, dateODT_TRANSFER_DATE, dateODT_CUSTOM_DATE, dateODT_RECEIVE_DATE, dateNow = DateTime.Now;

            TBGMT_BLD BLD = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
            TBGMT_ICR ICR = MTSE.TBGMT_ICR.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
            #region 錯誤訊息
            if (BLD == null) strMessage += $"<P> 提單編號：{ id } 之提單資料檔 不存在，請先新增！ </p>";
            if (ICR == null) strMessage += $"<P> 提單編號：{ id } 之時程管制簿 不存在，請先新增！ </p>";
            if (strOVC_PURCH_NO.Equals(string.Empty))
                strMessage += "<P> 請輸入 案號 </p>";
            //if (strOVC_CHI_NAME.Equals(string.Empty))
            //    strMessage += "<P> 請輸入 品名 </p>";
            if (strODT_IMPORT_DATE.Equals(string.Empty))
                strMessage += "<P> 請輸入 進口時間 </p>";
            bool boolOVC_IS_SECURITY = FCommon.checkInt(strOVC_IS_SECURITY, "機敏軍品", ref strMessage, out int intOVC_IS_SECURITY);
            bool boolODT_RECEIVE_INF_DATE = FCommon.checkDateTime(strODT_RECEIVE_INF_DATE, "收到貨通知書日期", ref strMessage, out dateODT_RECEIVE_INF_DATE);
            bool booODT_IMPORT_DATE = FCommon.checkDateTime(strODT_IMPORT_DATE, "進口日期", ref strMessage, out dateODT_IMPORT_DATE);
            bool boolODT_PASS_CUSTOM_DATE = FCommon.checkDateTime(strODT_PASS_CUSTOM_DATE, "通關日期", ref strMessage, out dateODT_PASS_CUSTOM_DATE);
            bool boolODT_ABROAD_CUSTOM_DATE = FCommon.checkDateTime(strODT_ABROAD_CUSTOM_DATE, "收國外報關文件日期", ref strMessage, out dateODT_ABROAD_CUSTOM_DATE);
            bool boolODT_UNPACKING_DATE = FCommon.checkDateTime(strODT_UNPACKING_DATE, "拆櫃日期", ref strMessage, out dateODT_UNPACKING_DATE);
            bool boolODT_CHANGE_BLD_DATE = FCommon.checkDateTime(strODT_CHANGE_BLD_DATE, "換小提單日期", ref strMessage, out dateODT_CHANGE_BLD_DATE);
            bool boolODT_TRANSFER_DATE = FCommon.checkDateTime(strODT_TRANSFER_DATE, "清運日期", ref strMessage, out dateODT_TRANSFER_DATE);
            bool boolODT_CUSTOM_DATE = FCommon.checkDateTime(strODT_CUSTOM_DATE, "報關日期", ref strMessage, out dateODT_CUSTOM_DATE);
            bool boolODT_RECEIVE_DATE = FCommon.checkDateTime(strODT_RECEIVE_DATE, "接收日期", ref strMessage, out dateODT_RECEIVE_DATE);
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                try
                {
                    #region TBGMT_BLD 修改資料
                    BLD.OVC_IS_SECURITY = intOVC_IS_SECURITY;
                    BLD.ODT_MODIFY_DATE = dateNow;
                    BLD.OVC_MODIFY_LOGIN_ID = strUserId;
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), BLD.GetType().Name.ToString(), this, "修改");
                    #endregion

                    #region TBGMT_BLD_MRPLOG 修改LOG
                    TBGMT_BLD_MRPLOG BLD_LOG = new TBGMT_BLD_MRPLOG();
                    BLD_LOG.LOG_LOGIN_ID = strUserId;
                    BLD_LOG.LOG_TIME = dateNow;
                    BLD_LOG.LOG_EVENT = "UPDATE";
                    BLD_LOG.OVC_BLD_NO = id;
                    BLD_LOG.OVC_SHIP_COMPANY = BLD.OVC_SHIP_COMPANY;
                    BLD_LOG.OVC_SEA_OR_AIR = BLD.OVC_SEA_OR_AIR;
                    BLD_LOG.OVC_SHIP_NAME = BLD.OVC_SHIP_NAME;
                    BLD_LOG.OVC_VOYAGE = BLD.OVC_VOYAGE;
                    BLD_LOG.ODT_START_DATE = BLD.ODT_START_DATE;
                    BLD_LOG.OVC_START_PORT = BLD.OVC_START_PORT;
                    BLD_LOG.ODT_PLN_ARRIVE_DATE = BLD.ODT_PLN_ARRIVE_DATE;
                    BLD_LOG.ODT_ACT_ARRIVE_DATE = BLD.ODT_ACT_ARRIVE_DATE;
                    BLD_LOG.OVC_ARRIVE_PORT = BLD.OVC_ARRIVE_PORT;
                    BLD_LOG.ONB_QUANITY = BLD.ONB_QUANITY;
                    BLD_LOG.OVC_QUANITY_UNIT = BLD.OVC_QUANITY_UNIT;
                    BLD_LOG.ONB_VOLUME = BLD.ONB_VOLUME;
                    BLD_LOG.ONB_ON_SHIP_VOLUME = BLD.ONB_ON_SHIP_VOLUME;
                    BLD_LOG.OVC_VOLUME_UNIT = BLD.OVC_VOLUME_UNIT;
                    BLD_LOG.ONB_WEIGHT = BLD.ONB_WEIGHT;
                    BLD_LOG.OVC_WEIGHT_UNIT = BLD.OVC_WEIGHT_UNIT;
                    BLD_LOG.ONB_ITEM_VALUE = BLD.ONB_ITEM_VALUE;
                    BLD_LOG.ONB_CARRIAGE_CURRENCY_I = BLD.ONB_CARRIAGE_CURRENCY_I;
                    BLD_LOG.ONB_CARRIAGE = BLD.ONB_CARRIAGE;
                    BLD_LOG.ONB_CARRIAGE_CURRENCY = BLD.ONB_CARRIAGE_CURRENCY;
                    BLD_LOG.OVC_PAYMENT_TYPE = BLD.OVC_PAYMENT_TYPE;
                    BLD_LOG.OVC_MILITARY_TYPE = BLD.OVC_MILITARY_TYPE;
                    BLD_LOG.OVC_STATUS = BLD.OVC_STATUS;
                    BLD_LOG.ODT_CREATE_DATE = BLD.ODT_CREATE_DATE;
                    BLD_LOG.OVC_IS_SECURITY = Convert.ToByte(strOVC_IS_SECURITY);
                    BLD_LOG.ODT_MODIFY_DATE = dateNow;
                    BLD_LOG.OVC_CREATE_LOGIN_ID = BLD.OVC_CREATE_LOGIN_ID;
                   
                    BLD_LOG.MRPLOG_SN = Guid.NewGuid();
                    MTSE.TBGMT_BLD_MRPLOG.Add(BLD_LOG);
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), BLD_LOG.GetType().Name.ToString(), this, "新增");
                    #endregion

                    #region TBGMT_ICR 修改
                    ICR.OVC_RECEIVE_DEPT_CODE = strOVC_DEPT_CDE;
                    ICR.OVC_PURCH_NO = strOVC_PURCH_NO;
                    ICR.OVC_CHI_NAME = strOVC_CHI_NAME;
                    ICR.OVC_RECEIVE_DEPT_CODE = strOVC_DEPT_CDE;
                    if (boolODT_RECEIVE_INF_DATE) ICR.ODT_RECEIVE_INF_DATE = dateODT_RECEIVE_INF_DATE; else ICR.ODT_RECEIVE_INF_DATE = null;
                    ICR.OVC_TRANS_TYPE = strOVC_TRANS_TYPE;
                    if (booODT_IMPORT_DATE)ICR.ODT_IMPORT_DATE = dateODT_IMPORT_DATE; else ICR.ODT_IMPORT_DATE = null;
                    if (boolODT_PASS_CUSTOM_DATE)ICR.ODT_PASS_CUSTOM_DATE = dateODT_PASS_CUSTOM_DATE; else ICR.ODT_PASS_CUSTOM_DATE = null;
                    if (boolODT_ABROAD_CUSTOM_DATE)ICR.ODT_ABROAD_CUSTOM_DATE = dateODT_ABROAD_CUSTOM_DATE; else ICR.ODT_ABROAD_CUSTOM_DATE = null;
                    if (boolODT_UNPACKING_DATE)ICR.ODT_UNPACKING_DATE = dateODT_UNPACKING_DATE; else ICR.ODT_UNPACKING_DATE = null;
                    if (boolODT_CHANGE_BLD_DATE)ICR.ODT_CHANGE_BLD_DATE = dateODT_CHANGE_BLD_DATE; else ICR.ODT_CHANGE_BLD_DATE = null;
                    if (boolODT_TRANSFER_DATE)ICR.ODT_TRANSFER_DATE = dateODT_TRANSFER_DATE; else ICR.ODT_TRANSFER_DATE = null;
                    if (boolODT_CUSTOM_DATE)ICR.ODT_CUSTOM_DATE = dateODT_CUSTOM_DATE; else ICR.ODT_CUSTOM_DATE = null;
                    if (boolODT_RECEIVE_DATE)ICR.ODT_RECEIVE_DATE = dateODT_RECEIVE_DATE; else ICR.ODT_RECEIVE_DATE = null;
                    ICR.OVC_NOTE = strOVC_NOTE;
                    ICR.ODT_MODIFY_DATE = dateNow;
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), ICR.GetType().Name.ToString(), this, "修改");
                    #endregion

                    #region TBGMT_ICR_MRPLOG 新增LOG
                    TBGMT_ICR_MRPLOG ICR_LOG = new TBGMT_ICR_MRPLOG();
                    ICR_LOG.LOG_LOGIN_ID = strUserId;
                    ICR_LOG.LOG_EVENT = "UPDATE";
                    ICR_LOG.LOG_TIME = dateNow;
                    ICR_LOG.OVC_BLD_NO = id;
                    ICR_LOG.OVC_PURCH_NO = strOVC_PURCH_NO;
                    ICR_LOG.OVC_CHI_NAME = txtOVC_CHI_NAME.Text;
                    ICR_LOG.OVC_RECEIVE_DEPT_CODE = strOVC_DEPT_CDE;
                    if (boolODT_RECEIVE_INF_DATE) ICR_LOG.ODT_RECEIVE_INF_DATE = dateODT_RECEIVE_INF_DATE; else ICR_LOG.ODT_RECEIVE_INF_DATE = null;
                    ICR_LOG.OVC_TRANS_TYPE = strOVC_TRANS_TYPE;
                    if (booODT_IMPORT_DATE) ICR_LOG.ODT_IMPORT_DATE = dateODT_IMPORT_DATE; else ICR_LOG.ODT_IMPORT_DATE = null;
                    if (boolODT_PASS_CUSTOM_DATE) ICR_LOG.ODT_PASS_CUSTOM_DATE = dateODT_PASS_CUSTOM_DATE; else ICR_LOG.ODT_PASS_CUSTOM_DATE = null;
                    if (boolODT_ABROAD_CUSTOM_DATE) ICR_LOG.ODT_ABROAD_CUSTOM_DATE = dateODT_ABROAD_CUSTOM_DATE; else ICR_LOG.ODT_ABROAD_CUSTOM_DATE = null;
                    if (boolODT_UNPACKING_DATE) ICR_LOG.ODT_UNPACKING_DATE = dateODT_UNPACKING_DATE; else ICR_LOG.ODT_UNPACKING_DATE = null;
                    if (boolODT_CHANGE_BLD_DATE) ICR_LOG.ODT_CHANGE_BLD_DATE = dateODT_CHANGE_BLD_DATE; else ICR_LOG.ODT_CHANGE_BLD_DATE = null;
                    if (boolODT_TRANSFER_DATE) ICR_LOG.ODT_TRANSFER_DATE = dateODT_TRANSFER_DATE; else ICR_LOG.ODT_TRANSFER_DATE = null;
                    if (boolODT_CUSTOM_DATE) ICR_LOG.ODT_CUSTOM_DATE = dateODT_CUSTOM_DATE; else ICR_LOG.ODT_CUSTOM_DATE = null;
                    if (boolODT_RECEIVE_DATE) ICR_LOG.ODT_RECEIVE_DATE = dateODT_RECEIVE_DATE; else ICR_LOG.ODT_RECEIVE_DATE = null;
                    ICR_LOG.OVC_NOTE = strOVC_NOTE;
                    ICR_LOG.OVC_CREATE_LOGIN_ID = ICR.OVC_CREATE_LOGIN_ID;
                    ICR_LOG.ODT_MODIFY_DATE = dateNow;
                    ICR_LOG.OVC_ICRMRPLOG_SN = Guid.NewGuid();
                    MTSE.TBGMT_ICR_MRPLOG.Add(ICR_LOG);
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), ICR_LOG.GetType().Name.ToString(), this, "新增");
                    #endregion
                    
                    FCommon.AlertShow(PnModify, "success", "系統訊息", $"提單編號：{ id } 之時程管制簿 修改成功！");
                }
                catch
                {
                    FCommon.AlertShow(PnModify, "danger", "系統訊息", "更新失敗，請聯絡工程師。");
                }
            }
            else
                FCommon.AlertShow(PnModify, "danger", "系統訊息", strMessage);
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_A14_1{ getQueryString() }");
        }
    }
}