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
    public partial class MPMS_D24 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string[] strField = { "OVC_PURCH", "OVC_PURCH_5", "OVC_PUR_IPURCH", "OVC_DBEGIN", "OVC_DO_NAME", " OVC_STATUS", "OVC_REMARK" };

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}