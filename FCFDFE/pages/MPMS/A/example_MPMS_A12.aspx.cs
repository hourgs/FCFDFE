using System;
using System.Web.UI;

namespace FCFDFE.pages.MPMS.A
{
    public partial class example_MPMS_A12 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";

        protected void btnMerge_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(txtString1.Text).Append(txtString2.Text);
            lblString.Text = sb.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}