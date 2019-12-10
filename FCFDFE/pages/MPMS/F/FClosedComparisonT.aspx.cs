using FCFDFE.Content;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web.UI;

namespace FCFDFE.pages.MPMS.F
{
    public partial class FClosedComparisonT : Page
    {
        Common FCommon = new Common();
        public string strMenuName = "", strMenuNameItem = "";
        public string strDEPT_SN, strDEPT_Name;

        #region 副程式
        private void list_dataImport()
        {
            string strTestFirst = "請選擇－可空白";
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
                    //FCommon.Controls_Attributes("readonly", "true", textVersionTime1, textVersionTime2);
                    list_dataImport();
                }
            }
        }

        #region OnClick
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string strQueryString = "";
            string strOVC_PURCH = txtOVC_PURCH.Text;
            FCommon.setQueryString(ref strQueryString, "PurNum", strOVC_PURCH, true);
            string strSQL = $"FAfterClosedComparisonT{ strQueryString }";
            //Response.Redirect($"FAfterClosedComparisonT{ strQueryString }");
            string strScript = $@"
                <script language='javascript'>
                    window.open('{ strSQL }');
                </script>";
            ClientScript.RegisterStartupScript(GetType(), "Open", strScript);
        }
        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("FPlanAssessmentSA");
        }
        #endregion
    }
}