using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;

namespace FCFDFE.pages.MPMS.C
{
    public partial class TRANSFORM_C : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        string strPurchNum = "";
        string[] strField = { "OVC_PURCH", "OVC_PUR_AGENCY", "OVC_DATE", "OVC_USER", "OVC_FROM_UNIT_NAME", "OVC_TO_UNIT_NAME", "OVC_REMARK" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["OVC_PURCH"] = Request.QueryString["OVC_PURCH"];
            }


            DataImport();
        }

        protected void GV_OVC_BUDGET_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        private void DataImport()
        {
            string purch = ViewState["OVC_PURCH"].ToString();
            var query1114 =
                from t in mpms.TBM1114
                join t1301 in mpms.TBM1301 on t.OVC_PURCH equals t1301.OVC_PURCH
                where t.OVC_PURCH.Equals(purch)
                select new
                {
                    t.OVC_PURCH,
                    t1301.OVC_PUR_AGENCY,
                    t.OVC_DATE,
                    t.OVC_USER,
                    t.OVC_FROM_UNIT_NAME,
                    t.OVC_TO_UNIT_NAME,
                    t.OVC_REMARK
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query1114);
            ViewState["hasRows"] = FCommon.GridView_dataImport(TBM1114, dt, strField);
        }
    }
}