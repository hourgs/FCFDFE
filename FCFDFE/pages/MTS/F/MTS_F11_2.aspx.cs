using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F11_2 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTS = new MTSEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {

            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string strOvcPortCde = txtOvcPortCde.Text;
            string strOvcPortType = drpOvcPortType.SelectedItem.Text;

            DataTable dt = new DataTable();
            var query =
                from port in MTS.TBGMT_PORTS
                select new
                {
                    port.OVC_PORT_CDE,
                    port.OVC_PORT_CHI_NAME,
                    port.OVC_PORT_ENG_NAME,
                    port.OVC_PORT_TYPE,
                    port.OVC_IS_ABROAD,
                };
            if(strOvcPortCde!=string.Empty)
                    query = query.Where(ot => ot.OVC_PORT_CDE.Equals(strOvcPortCde));
            query = query.Where(ot => ot.OVC_PORT_TYPE.Equals(strOvcPortType));

            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_PORT, dt );
        }

        protected void GV_TBGMT_PORT_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}