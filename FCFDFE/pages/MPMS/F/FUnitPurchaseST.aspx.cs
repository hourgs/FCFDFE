using FCFDFE.Content;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MPMS.F
{
    public partial class FUnitPurchaseST : Page
    {
        Common FCommon = new Common();
        public string strMenuName = "", strMenuNameItem = "";
        public string strDEPT_SN, strDEPT_Name;

        #region 副程式
        private void list_dataImport()
        {
            //年度
            FCommon.list_dataImportYear(drpYear);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getAccountDEPTName(this, out strDEPT_SN, out strDEPT_Name);

                if (!IsPostBack)
                {
                    //設置readonly屬性
                    //FCommon.Controls_Attributes("readonly", "true", txtOVC_DPROPOSE1, txtOVC_DPROPOSE2, txtOVC_PUR_DAPPROVE1, txtOVC_PUR_DAPPROVE2);
                    list_dataImport();
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string strYear = drpYear.SelectedValue;
            strYear = strYear.Substring(strYear.Length - 2, 2);
            string strMethod = rdoMethod.SelectedValue;
            string strMethodName = rdoMethod.SelectedItem.Text;

            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "year", strYear, true);
            FCommon.setQueryString(ref strQueryString, "method", strMethod, true);
            FCommon.setQueryString(ref strQueryString, "methodName", strMethodName, true);
            if (strMethod.Equals("AllUnit"))
                Response.Redirect($"FListedDownload{ strQueryString }"); //全部單位直接下載Excel
            else
                Response.Redirect($"FListedCommon{ strQueryString }");
        }
        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("FPlanAssessmentSA");
        }
    }
}