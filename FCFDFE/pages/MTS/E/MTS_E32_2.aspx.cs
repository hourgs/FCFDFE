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
    public partial class MTS_E32_2 : System.Web.UI.Page
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            FCommon.getQueryString(this, "id", out id, true);
            string strOVC_TOF_NO = id;

            var query = mtse.TBGMT_TOF.Where(table => table.OVC_TOF_NO == strOVC_TOF_NO).FirstOrDefault();
            string strOVC_IS_PAID = query.OVC_IS_PAID;

            if (strOVC_IS_PAID == "已付款")
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "此接轉作業費結報申請表已付款，無法修改");
            }
            else
            {
                string strOVC_IE_TYPE = drpOvcIeType.SelectedItem.Text;
                string strOVC_BUDGET = drpOvcBudget.SelectedItem.Text;
                string strOVC_PURPOSE_TYPE = drpOvcPurposeType.SelectedValue.ToString();
                string strOVC_ABSTRACT = txtOvcAbstract.Text;
                string strONB_AMOUNT = txtOnbAmount.Text;
                string strOVC_NOTE = txtOvcNote.Text;
                string strOVC_PLN_CONTENT = txtOvcPlnContent.Text;
                string strMessage = "";

                if (strOVC_BUDGET == "未設定")
                    strOVC_BUDGET = "";

                if (strOVC_ABSTRACT.Equals(string.Empty))
                    strMessage += "<P> 摘要欄位不得為空白 </p>";
                if (strONB_AMOUNT.Equals(string.Empty))
                    strMessage += "<P> 金額欄位不得為空白</p>";
                if (!strONB_AMOUNT.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_AMOUNT, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<P> 金額請輸入數字 </p>";
                    }
                }
                if (strMessage.Equals(string.Empty))
                {
                    query.OVC_IE_TYPE = strOVC_IE_TYPE;
                    query.OVC_BUDGET = strOVC_BUDGET;
                    query.OVC_PURPOSE_TYPE = strOVC_PURPOSE_TYPE;
                    query.OVC_ABSTRACT = strOVC_ABSTRACT;
                    query.ONB_AMOUNT = Convert.ToInt32(strONB_AMOUNT);
                    query.OVC_NOTE = strOVC_NOTE;
                    query.OVC_PLN_CONTENT = strOVC_PLN_CONTENT;

                    mtse.SaveChanges();
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改接轉作業費結報申請表成功，申請表編號：" + strOVC_TOF_NO);
                    FCommon.syslog_add(Session["userid"].ToString(),Request.ServerVariables["REMOTE_ADDR"].ToString(), query.GetType().Name.ToString(), this, "修改");
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }

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
                string strOVC_BUDGET = query.OVC_BUDGET;
                lblOvcTofNo.Text = strOVC_TOF_NO;
                lblMilitaryType.Text = query.OVC_MILITARY_TYPE;
                drpOvcIeType.Text = query.OVC_IE_TYPE;
                if (strOVC_BUDGET != null)
                    drpOvcBudget.Text = query.OVC_BUDGET;
                drpOvcPurposeType.Text = query.OVC_PURPOSE_TYPE;
                txtOvcAbstract.Text = query.OVC_ABSTRACT;
                txtOnbAmount.Text = query.ONB_AMOUNT.ToString();
                txtOvcNote.Text = query.OVC_NOTE;
                txtOvcPlnContent.Text = query.OVC_PLN_CONTENT;
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
    }
}