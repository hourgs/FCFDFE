using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Content;

namespace FCFDFE.pages.CIMS.E
{
    public partial class CIMS_E11_3_1 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        private GMEntities gme = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["OVC_PURCH"] == null)
            {
                Response.Write("<script>alert('系統檢測到您未依照正確方式進入此頁面，將導至登入畫面!'); location.href='../../../logout.aspx'; </script>");
                return;
            }
            if (!IsPostBack)
            {
                data();
                ViewState["retu"] = Request.UrlReferrer.ToString();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(ViewState["retu"].ToString());
        }

        private void data()
        {
            string enkey = Request.QueryString["OVC_PURCH"];
            string id = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(enkey));
            string enkey5 = Request.QueryString["OVC_PURCH_5"];
            string id5 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(enkey5));
            string enkeyd = Request.QueryString["OVC_DOPEN"];
            string dopen = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(enkeyd));

            var query =mpms.TBM1303.Where(pid => pid.OVC_PURCH == id).Where(pid5 => pid5.OVC_PURCH_5 == id5).Where(dop => dop.OVC_DOPEN == dopen).FirstOrDefault();
            var query1301 = mpms.TBM1301.Where(pid => pid.OVC_PURCH == id).FirstOrDefault();

            lblOVC_PURCH.Text = id;
            lblOVC_PURCH_5.Text = id5;
            lblOVC_PUR_IPURCH1301.Text = query1301.OVC_PUR_IPURCH;
            lblOVC_DOPEN.Text = dopen;
            lblONB_TIMES.Text = query.ONB_TIMES.ToString();
            lblOVC_PLACE.Text = query.OVC_PLACE;
            string openmethod = query.OVC_OPEN_METHOD;
            var queryR8 = mpms.TBM1407.Where(table => table.OVC_PHR_CATE == "R8").Where(table => table.OVC_PHR_ID == openmethod).FirstOrDefault();
            if (queryR8 != null)
                lblOVC_OPEN_METHOD.Text = query.OVC_BID_METHOD + ":" + queryR8.OVC_PHR_DESC;
            else
                lblOVC_OPEN_METHOD.Text = query.OVC_BID_METHOD;
            string bidmethod = query.OVC_BID_METHOD;
            var queryR9 = mpms.TBM1407.Where(table => table.OVC_PHR_CATE == "R9").Where(table => table.OVC_PHR_ID == bidmethod).FirstOrDefault();
            if (queryR9 != null)
                lbl_OVC_BID_METHOD.Text = query.OVC_BID_METHOD + ":" + queryR9.OVC_PHR_DESC;
            else
                lbl_OVC_BID_METHOD.Text = query.OVC_BID_METHOD;
            lbl_OVC_METHOD_1.Text = query.OVC_METHOD_1;
            lblONB_BID_VENDORS.Text = query.ONB_BID_VENDORS.ToString();
            lbl_OVC_METHOD_2.Text = query.OVC_METHOD_2;
            lbl_OVC_METHOD_3.Text = query.OVC_METHOD_3;
            lblONB_RESULT_OK_NOTOK.Text = "[合格家數] : " + query.ONB_RESULT_OK.ToString() + "<br />[不合格家數] : " + query.ONB_RESULT_NOTOK.ToString();
            string result = query.OVC_RESULT;
            var queryA8 = mpms.TBM1407.Where(table => table.OVC_PHR_CATE == "A8").Where(table => table.OVC_PHR_ID == result).FirstOrDefault();
            if (queryA8 != null)
                lblOVC_RESULT.Text = query.OVC_RESULT + ":" + queryA8.OVC_PHR_DESC;
            else
                lblOVC_RESULT.Text = query.OVC_RESULT;
            lblONB_BID_BUDGET.Text = query.ONB_BID_BUDGET.ToString();
            lblOVC_RESULT_REASON.Text = query.OVC_RESULT_REASON;
            lblONB_BID_RESULT.Text = query.ONB_BID_RESULT.ToString();
            lblONB_OK_NOTOK.Text = "[合格家數] : " + query.ONB_OK_VENDORS.ToString() + "  [不合格家數] : " + query.ONB_NOTOK_VENDORS.ToString();
            lblOVC_CHAIRMAN.Text = query.OVC_CHAIRMAN;
            lblOVC_NAME.Text = query.OVC_NAME;
            if (query.OVC_DAPPROVE != null)
                lblOVC_DAPPROVE.Text = Convert.ToDateTime(query.OVC_DAPPROVE).ToString(Variable.strDateFormat) ;
            else
                lblOVC_DAPPROVE.Text = "";
        }
    }
}