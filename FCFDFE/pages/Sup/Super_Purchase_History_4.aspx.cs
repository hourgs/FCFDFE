using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;

namespace FCFDFE.pages.Sup
{
    public partial class Super_Purchase_History_4 : System.Web.UI.Page
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
                data1201();
                ViewState["retu"] = Request.UrlReferrer.ToString();
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(ViewState["retu"].ToString());
        }

        private void data1201()
        {
            string enkey = Request.QueryString["OVC_PURCH"];
            string id = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(enkey));
            string poienkey = Request.QueryString["ONB_POI_ICOUNT"];
            string poiid = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(poienkey));
            int p = Convert.ToInt16(poiid);

            var query = mpms.TBM1201.Where(pid => pid.OVC_PURCH == id).Where(poid => poid.ONB_POI_ICOUNT == p).FirstOrDefault();
            var query1301 = mpms.TBM1301.Where(pid => pid.OVC_PURCH == id).FirstOrDefault();

            lblOVC_PURCH.Text = id;
            lblOVC_PUR_IPURCH1301.Text = query1301.OVC_PUR_IPURCH;
            lblONB_POI_ICOUNT.Text = poiid;
            lblOVC_FCODE.Text = query.OVC_FCODE;
            lblOVC_POI_NSTUFF_CHN.Text = query.OVC_POI_NSTUFF_CHN;
            lblOVC_POI_NSTUFF_ENG.Text = query.OVC_POI_NSTUFF_ENG;
            lblNSN_KIND.Text = query.NSN_KIND;
            lblNSN.Text = query.NSN;
            lblOVC_BRAND.Text = query.OVC_BRAND;
            lblOVC_MODEL.Text = query.OVC_MODEL;
            lblOVC_POI_IREF.Text = query.OVC_POI_IREF;
            lblOVC_SAME_QUALITY.Text = query.OVC_SAME_QUALITY;
            lblOVC_POI_IUNIT.Text = query.OVC_POI_IUNIT;
            lblOVC_FIRST_BUY.Text = query.OVC_FIRST_BUY;
            lblOVC_POI_IPURCH_BEF.Text = query.OVC_POI_IPURCH_BEF;
            lblONB_POI_QORDER_PLAN.Text = query.ONB_POI_QORDER_PLAN.ToString();
            lblONB_POI_QORDER_CONT.Text = query.ONB_POI_QORDER_CONT.ToString();
            lblONB_POI_MPRICE_PLAN.Text = query.ONB_POI_MPRICE_PLAN.ToString();
            lblONB_POI_MPRICE_CONT.Text = query.ONB_POI_MPRICE_CONT.ToString();
            lblOVC_POI_NDESC.Text = query.OVC_POI_NDESC;
        }
    }
}