using System;
using System.Linq;
using System.Web.UI;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MTS.B
{
    public partial class MTS_B12_3 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        string id;

        #region 副程式
        private void dataImport()
        {
            TBGMT_IINN iinn = MTSE.TBGMT_IINN.Where(table => table.OVC_IINN_NO.Equals(id)).FirstOrDefault();
            if (iinn != null)
            {
                string strOVC_BLD_NO = iinn.OVC_BLD_NO;

                lblOVC_IINN_NO.Text = iinn.OVC_IINN_NO;
                lblOVC_BLD_NO.Text = strOVC_BLD_NO;
                lblOVC_PURCH_NO.Text = iinn.OVC_PURCH_NO;
                lblISSU_NO.Text = iinn.ISSU_NO;
                lblONB_ITEM_VALUE.Text = iinn.ONB_ITEM_VALUE.ToString();
                lblONB_CARRIAGE_CURRENCY.Text = iinn.ONB_CARRIAGE_CURRENCY;
                lblONB_INS_AMOUNT.Text = iinn.ONB_INS_AMOUNT.ToString();
                lblONB_CARRIAGE_CURRENCY2.Text = iinn.ONB_CARRIAGE_CURRENCY2;
                lblODT_INS_DATE.Text = FCommon.getDateTime(iinn.ODT_INS_DATE);
                lblOVC_DELIVERY_CONDITION.Text = iinn.OVC_DELIVERY_CONDITION;
                lblOVC_INS_CONDITION.Text = iinn.OVC_INS_CONDITION;
                lblOVC_PURCHASE_TYPE.Text = iinn.OVC_PURCHASE_TYPE;
                lblONB_INS_RATE.Text = iinn.ONB_INS_RATE.ToString();
                lblPOLICY_NO.Text = iinn.POLICY_NO;
                lblOVC_PAYMENT_TYPE.Text = iinn.OVC_PAYMENT_TYPE;
                lblOVC_MILITARY_TYPE.Text = CommonMTS.getClassName(iinn.OVC_MILITARY_TYPE); //取得軍種名稱
                lblOVC_FINAL_INS_AMOUNT.Text = iinn.OVC_FINAL_INS_AMOUNT.ToString();
                lblOVC_NOTE.Text = iinn.OVC_NOTE;

                TBGMT_BLD bld = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                if (bld != null)
                {
                    lblONB_QUANITY.Text = bld.ONB_QUANITY.ToString();
                    lblOVC_SHIP_COMPANY.Text = bld.OVC_SHIP_COMPANY;
                    lblOVC_START_PORT.Text = CommonMTS.getPortName(bld.OVC_START_PORT);
                    lblODT_START_DATE.Text = FCommon.getDateTime(bld.REAL_START_DATE);
                    lblOVC_ARRIVE_PORT.Text = CommonMTS.getPortName(bld.OVC_ARRIVE_PORT);
                }

                TBGMT_ICR icr = MTSE.TBGMT_ICR.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                if (icr != null)
                {
                    lblOVC_CHI_NAME.Text = icr.OVC_CHI_NAME;
                    lblODT_IMPORT_DATE.Text = FCommon.getDateTime(icr.ODT_IMPORT_DATE);
                }
            }
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_IINN_NO", Request.QueryString["OVC_IINN_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", Request.QueryString["OVC_BLD_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_PURCH_NO", Request.QueryString["OVC_PURCH_NO"], false);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getQueryString(this, "id", out id, true);
                if (!IsPostBack)
                {
                    dataImport();
                }
            }
        }

        #region ~Click
        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                TBGMT_IINN iinn = MTSE.TBGMT_IINN.Where(table => table.OVC_IINN_NO.Equals(id)).FirstOrDefault();
                if (iinn != null)
                {
                    MTSE.Entry(iinn).State = EntityState.Deleted;
                    MTSE.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), iinn.GetType().Name.ToString(), this, "刪除");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", $"編號：{ id } 之投保通知書 刪除成功。");
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"編號：{ id } 之投保通知書 不存在！");
            }
            catch
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "刪除失敗，請聯絡工程師！");
            }
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_B12_1{ getQueryString() }");
        }
        #endregion
    }
}