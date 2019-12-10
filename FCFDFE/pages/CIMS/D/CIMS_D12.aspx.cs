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
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace FCFDFE.pages.CIMS.D
{
    public partial class CIMS_D12 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        CIMSEntities CIMS = new CIMSEntities();
        string[] strField = { "INDEX_CODE", "UNIT", "STAR_TIME", "NEW_TIME", "PERIOD", "INDEX_DESC" };
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            //DataTable dt = new DataTable();
            //for (int i = 0; i < 1; i++)
            //{
            //    DataRow dr = dt.NewRow();
            //    dt.Rows.Add(dr);
            //}
            //GridView1.DataSource = dt.AsDataView();
            //GridView1.DataBind();
        }

        protected void GV_VENAGENT_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}