using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;
using System.Data.Entity;

namespace FCFDFE.pages.MTS.E
{
    public partial class MTS_E32_3 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        private MTSEntities mtse = new MTSEntities();
        Common FCommon = new Common();
        string id;
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

        protected void btnDel_Click(object sender, EventArgs e)
        {
            FCommon.getQueryString(this, "id", out id, true);
            string strOVC_TOF_NO = id;

            TBGMT_TOF tof = new TBGMT_TOF();
            tof = mtse.TBGMT_TOF.Where(table => table.OVC_TOF_NO == strOVC_TOF_NO).FirstOrDefault();
            if(tof != null)
            {
                string strOVC_IS_PAID = tof.OVC_IS_PAID;
                if (strOVC_IS_PAID == "已付款")
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "此接轉作業費結報申請表已付款，無法修改");
                }
                else
                {
                    mtse.Entry(tof).State = EntityState.Deleted;
                    mtse.SaveChanges();

                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除接轉作業費結報申請表成功，申請表編號：" + strOVC_TOF_NO);
                    FCommon.syslog_add(Session["userid"].ToString(),Request.ServerVariables["REMOTE_ADDR"].ToString(), tof.GetType().Name.ToString(), this, "刪除");
                }
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "此接轉作業費結報申請表已刪除，申請表編號：" + strOVC_TOF_NO);
            }
            
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OvcTofNo", Request.QueryString["OvcTofNo"], false);
            FCommon.setQueryString(ref strQueryString, "OvcIsPaid", Request.QueryString["OvcIsPaid"], false);
            FCommon.setQueryString(ref strQueryString, "OvcBudget", Request.QueryString["OvcBudget"], false);
            FCommon.setQueryString(ref strQueryString, "OvcPurposeType", Request.QueryString["OvcPurposeType"], false);
            FCommon.setQueryString(ref strQueryString, "OdtApplyDate1", Request.QueryString["OdtApplyDate1"], false);
            FCommon.setQueryString(ref strQueryString, "OdtApplyDate2", Request.QueryString["OdtApplyDate2"], false);
            FCommon.setQueryString(ref strQueryString, "chkOdtApplyDate", Request.QueryString["chkOdtApplyDate"], false);
            return strQueryString;
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_E32_1.aspx{getQueryString()}");
        }

        private void dataImport()
        {
            FCommon.getQueryString(this, "id", out id, true);
            string strOVC_TOF_NO = id;

            var query = mtse.TBGMT_TOF.Where(table => table.OVC_TOF_NO == strOVC_TOF_NO).FirstOrDefault();
            if (query != null)
            {
                lblOvcTofNo.Text = strOVC_TOF_NO;
                lblMilitaryType.Text = query.OVC_MILITARY_TYPE;
                lblOvcIeType.Text = query.OVC_IE_TYPE;
                lblOvcBudget.Text = query.OVC_BUDGET;
                lblOvcPurposeType.Text = query.OVC_PURPOSE_TYPE;
                lblOvcAbstract.Text = query.OVC_ABSTRACT;
                lblOnbAmount.Text = query.ONB_AMOUNT.ToString();
                lblOvcNote.Text = query.OVC_NOTE;
                lblOvcPlnContent.Text = query.OVC_PLN_CONTENT;
            }


        }
    }
}