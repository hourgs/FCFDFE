using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D21 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string strUserName;

        protected void Page_Load(object sender, EventArgs e)
        {
            //FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_PURCH_5"] == null)
                FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
            else
            {
                if (!IsPostBack)
                {
                    string strOVC_PURCH_key = Request.QueryString["OVC_PURCH"];
                    string strOVC_PURCH_5_key = Request.QueryString["OVC_PURCH_5"];
                    string strONB_GROUP_key = Request.QueryString["ONB_GROUP"] == null ? "" : Request.QueryString["ONB_GROUP"];
                    string strOVC_VEN_TITLE_key = Request.QueryString["OVC_VEN_TITLE"] == null ? "" : Request.QueryString["OVC_VEN_TITLE"];
                    string strOVC_PURCH = System.Text.Encoding.Default.GetString(Convert.FromBase64String(strOVC_PURCH_key));
                    string strOVC_PURCH_5 = System.Text.Encoding.Default.GetString(Convert.FromBase64String(strOVC_PURCH_5_key));
                    string strONB_GROUP = System.Text.Encoding.Default.GetString(Convert.FromBase64String(strONB_GROUP_key));
                    string strOVC_VEN_TITLE = System.Text.Encoding.Default.GetString(Convert.FromBase64String(strOVC_VEN_TITLE_key));
                    ViewState["strOVC_PURCH"] = strOVC_PURCH;
                    ViewState["strOVC_PURCH_5"] = strOVC_PURCH_5;
                    ViewState["strONB_GROUP"] = strONB_GROUP;
                    ViewState["strOVC_VEN_TITLE"] = strOVC_VEN_TITLE;

                    var queryOVC_PUR_AGENCY = mpms.TBM1301_PLAN.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault().OVC_PUR_AGENCY;

                    lblOVC_PURCH.Text = strOVC_PURCH + queryOVC_PUR_AGENCY + strOVC_PURCH_5;
                    lblOVC_VEN_TITLE.Text = strOVC_VEN_TITLE;
                    lblONB_GROUP.Text = strONB_GROUP;

                    dataImport();
                }
            }
        }

        protected void dataImport()
        {
            lblOVC_PURCH_1.Text = lblOVC_PURCH.Text;
        }
    }
}