using System;
using System.Data;
using System.Linq;
using FCFDFE.Content;
using System.Data.Entity;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.Web.UI;

namespace FCFDFE.pages.MTS.C
{
    public partial class MTS_C12_3 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        DateTime dateNow = DateTime.Now;
        string id;

        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_CLAIM_NO", Request.QueryString["OVC_CLAIM_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_DEPT_CDE", Request.QueryString["OVC_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_CLAIM_DATE", Request.QueryString["ODT_CLAIM_DATE"], false);
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
                    if (Guid.TryParse(id, out Guid guidCLAIM_SN))
                    {
                        TBGMT_CLAIM claim = MTSE.TBGMT_CLAIM.Where(table => table.CLAIM_SN.Equals(guidCLAIM_SN)).FirstOrDefault();
                        if (claim != null)
                        {
                            lblOVC_CLAIM_NO.Text = claim.OVC_CLAIM_NO;
                            string strOVC_DEPT_CDE = claim.OVC_MILITARY_TYPE;
                            lblOVC_DEPT_CDE.Text = strOVC_DEPT_CDE;
                            lblOVC_ONNAME.Text = FCommon.getDeptName(strOVC_DEPT_CDE);
                            lblOVC_PURCH_NO.Text = claim.OVC_PURCH_NO;
                            lblODT_CLAIM_DATE.Text = FCommon.getDateTime(claim.ODT_CLAIM_DATE);
                            lblOVC_CLAIM_MSG_NO.Text = claim.OVC_CLAIM_MSG_NO;
                            lblOVC_INN_NO.Text = claim.OVC_INN_NO;
                            lblOVC_CLAIM_ITEM.Text = claim.OVC_CLAIM_ITEM;
                            lblONB_CLAIM_NUMBER.Text = claim.ONB_CLAIM_NUMBER.ToString();
                            lblONB_CLAIM_AMOUNT.Text = claim.ONB_CLAIM_AMOUNT.ToString();
                            lblOVC_CLAIM_REASON.Text = claim.OVC_CLAIM_REASON;
                            lblOVC_NOTE.Text = claim.OVC_NOTE;
                        }
                        else
                            FCommon.AlertShow(pnMessageDelete, "danger", "系統訊息", "索賠通知書 不存在！");
                    }
                    else
                        FCommon.AlertShow(pnMessageDelete, "danger", "系統訊息", "索賠通知書 系統編號錯誤！");
                }
            }
        }

        #region ~Click
        protected void btnDel_Click(object sender, EventArgs e)
        {
            if (Guid.TryParse(id, out Guid guidCLAIM_SN))
            {
                TBGMT_CLAIM claim = MTSE.TBGMT_CLAIM.Where(table => table.CLAIM_SN.Equals(guidCLAIM_SN)).FirstOrDefault();
                if (claim != null)
                {
                    MTSE.Entry(claim).State = EntityState.Deleted;
                    MTSE.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), claim.GetType().Name.ToString(), this, "刪除");
                    FCommon.AlertShow(pnMessageDelete, "success", "系統訊息", "刪除投保通知書 " + lblOVC_CLAIM_NO.Text + " 成功。");
                }
                else
                    FCommon.AlertShow(pnMessageDelete, "danger", "系統訊息", "索賠通知書 不存在！");
            }
            else
                FCommon.AlertShow(pnMessageDelete, "danger", "系統訊息", "索賠通知書 系統編號錯誤！");
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_C12_1{ getQueryString() }");
        }
        #endregion
    }
}