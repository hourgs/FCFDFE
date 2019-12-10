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
    public partial class STATUS : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        string[] strField = { "OVC_PURCH", "OVC_PUR_AGENCY", "ONB_TIMES", "OVC_DO_NAME", "OVC_PHR_DESC", "OVC_DBEGIN", "OVC_DEND"};
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
            //TODO:日期格式
            string purch = ViewState["OVC_PURCH"].ToString();
            var query =
                from t in mpms.TBMSTATUS
                join t1407 in mpms.TBM1407 on t.OVC_STATUS equals t1407.OVC_PHR_ID
                join t1301 in mpms.TBM1301 on t.OVC_PURCH equals t1301.OVC_PURCH
                where t.OVC_PURCH.Equals(purch) && t1407.OVC_PHR_CATE.Equals("Q9")
                select new
                {
                    t.OVC_PURCH,
                    t1301.OVC_PUR_AGENCY,
                    t.ONB_TIMES,
                    t.OVC_DO_NAME,
                    t1407.OVC_PHR_DESC,
                    t.OVC_DBEGIN,
                    t.OVC_DEND
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(TBM1114, dt, strField);
        }
    }
}