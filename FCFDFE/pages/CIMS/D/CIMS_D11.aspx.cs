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
    public partial class CIMS_D11 : System.Web.UI.Page
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

        }

        protected void GV_VENAGENT_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {

        }

        protected void btnNew_Click(object sender, EventArgs e)
        {

        }

        //private void dataimport()
        //{
        //    DataTable dt = new DataTable();
        //    //var query =
        //    //    from INDEXDESC in CIMS.INDEXDESC.DefaultIfEmpty().AsEnumerable()
        //    //    orderby INDEXDESC.INDEX_CODE ascending
        //    //    select new
        //    //    {
        //    //        INDEX_CODE = INDEXDESC.INDEX_CODE,
        //    //        UNIT = INDEXDESC.UNIT,
        //    //        START_TIME = INDEXDESC.START_TIME,
        //    //        NEW_TIME = INDEXDESC.NEW_TIME,
        //    //        PERIOD = INDEXDESC.PERIOD,
        //    //        INDEX_DESC = INDEXDESC.INDEX_DESC
        //    //    };

        //    dt = CommonStatic.LinqQueryToDataTable(query);
        //    ViewState["hasRows"] = FCommon.GridView_dataImport(GV_INDEXDESC, dt, strField);
        //}
    }
}