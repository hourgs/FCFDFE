using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E21 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            if (Session["rowtext"] != null)
            {
                if (!IsPostBack)
                {
                    Judgment();
                }
            }
            else
                FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
        }

        #region Click
        //申辦免稅btn
        protected void btnBid_Click(object sender, EventArgs e)
        {                                   
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E22.aspx";
            Response.Redirect(send_url);
        }
        //履約督導
        protected void btnSup_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E25.aspx";
            Response.Redirect(send_url);
        }
        //回上一頁
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }
        #endregion

        #region 副程式
        private void Judgment()
        {
            if (Session["rowtext"] != null)
            {
                var purch = Session["rowtext"].ToString().Substring(0, 7);
                var query =
                    from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                    where tbmreceive.OVC_PURCH.Equals(purch)
                    select new
                    {
                        //OVC_PUR_GOOD_OK = tbmreceive.OVC_PUR_GOOD_OK,
                        //OVC_PUR_TAX_OK = tbmreceive.OVC_PUR_TAX_OK,
                        OVC_PUR_FEE_OK = tbmreceive.OVC_PUR_FEE_OK
                    };
                foreach (var q in query)
                    if (!(q.OVC_PUR_FEE_OK != "N" && q.OVC_PUR_FEE_OK != "0")) btnBid.CssClass = "btn-success btn-lg disabled";

                var query_2 =
                    from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                    where tbmreceive.OVC_PURCH.Equals(purch)
                    select tbmreceive.OVC_INSPECT;
                foreach (var q in query_2)
                    if (q != "是") btnSup.CssClass = "btn-success btn-lg disabled";
            }
        }
        #endregion
    }
}