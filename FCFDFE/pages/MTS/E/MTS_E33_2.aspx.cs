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
    public partial class MTS_E33_2 : System.Web.UI.Page
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
                    dataImport(id);
                    FCommon.Controls_Attributes("readonly", "true", txtOdtPaidDate);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            FCommon.getQueryString(this, "id", out id, true);
            string strOVC_TOF_NO = id;

            var query = mtse.TBGMT_TOF.Where(table => table.OVC_TOF_NO == strOVC_TOF_NO).FirstOrDefault();
            string strOVC_IS_PAID = query.OVC_IS_PAID;

            string strOvcNote = txtOvcNote.Text;
            string strOdtPaidDate = txtOdtPaidDate.Text;
            string strOvcIsPaid = rdoOvcIsPaid.SelectedItem.Text;
            string strOvcBudget = drpOvcBudget.SelectedItem.Text;
            string strMessage = "";

            if (strOvcBudget == "未設定")
                strOvcBudget = "";
            if (strOdtPaidDate.Equals(string.Empty))
                strMessage += "<P> 付款日期欄位不得為空白 </p>";
            if (strOvcNote.Equals(string.Empty))
                strMessage += "<P> 備考欄位不得為空白 </p>";

            if (strMessage.Equals(string.Empty))
            {
                DateTime paid = Convert.ToDateTime(strOdtPaidDate);
                query.OVC_NOTE = strOvcNote;
                query.OVC_IS_PAID = strOvcIsPaid;
                query.OVC_BUDGET = strOvcBudget;
                query.ODT_PAID_DATE = paid;

                mtse.SaveChanges();
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "更新接轉作業費結報申請表成功，申請表編號：" + strOVC_TOF_NO);
                FCommon.syslog_add(Session["userid"].ToString(),Request.ServerVariables["REMOTE_ADDR"].ToString(), query.GetType().Name.ToString(), this, "修改");
            }
            else
            FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);

        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_E33_1.aspx{getQueryString()}");
        }

        private void dataImport(string strOVC_TOF_NO)
        {
            var query = mtse.TBGMT_TOF.Where(table => table.OVC_TOF_NO == strOVC_TOF_NO).FirstOrDefault();
            if (query != null)
            {
                string strOVC_BUDGET = query.OVC_BUDGET;
                lblOvcTofNo.Text = strOVC_TOF_NO;
                lblMilitaryType.Text = query.OVC_MILITARY_TYPE;
                lblOvcIeType.Text = query.OVC_IE_TYPE;
                if (strOVC_BUDGET != null)
                    drpOvcBudget.Text = query.OVC_BUDGET;
                lblOvcPurposeType.Text = query.OVC_PURPOSE_TYPE;
                lblOvcAbstract.Text = query.OVC_ABSTRACT;
                lblOnbAmount.Text = query.ONB_AMOUNT.ToString();
                lblOvcPlnContent.Text = query.OVC_PLN_CONTENT;
                lblOvcSection.Text = query.OVC_SECTION;
                if (query.ODT_APPLY_DATE != null)
                    lblOdtApplyDate.Text = Convert.ToDateTime(query.ODT_APPLY_DATE).ToString(Variable.strDateFormat);
                txtOvcNote.Text = query.OVC_NOTE;
                rdoOvcIsPaid.Text = query.OVC_IS_PAID;
                if (query.ODT_PAID_DATE != null)
                    txtOdtPaidDate.Text = Convert.ToDateTime(query.ODT_PAID_DATE).ToString(Variable.strDateFormat);
            }


        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OvcTofNo", Request.QueryString["OvcTofNo"], false);
            FCommon.setQueryString(ref strQueryString, "OvcIsPaid", Request.QueryString["OvcIsPaid"], false);
            FCommon.setQueryString(ref strQueryString, "OvcBudget", Request.QueryString["OvcBudget"], false);
            FCommon.setQueryString(ref strQueryString, "OvcPurposeType", Request.QueryString["OvcPurposeType"], false);
            FCommon.setQueryString(ref strQueryString, "OdtApplyDate2", Request.QueryString["OdtApplyDate2"], false);
            FCommon.setQueryString(ref strQueryString, "OvcSection", Request.QueryString["OvcSection"], false);
            FCommon.setQueryString(ref strQueryString, "chkOdtApplyDate", Request.QueryString["chkOdtApplyDate"], false);
            return strQueryString;
        }
    }
}