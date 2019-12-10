using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.CIMSModel;
using System.Data.Entity;

namespace FCFDFE.pages.CIMS.D
{
    public partial class CIMS_D13 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        CIMSEntities CIMS = new CIMSEntities();

        protected void button1_Click(object sender, EventArgs e)
        {
            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查無資料");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
        }
    }
}