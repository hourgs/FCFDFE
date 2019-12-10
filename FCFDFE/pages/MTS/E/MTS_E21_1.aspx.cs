using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.E
{
    public partial class MTS_E21_1 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            for (int i = 0; i < 2; i++)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
                hasRows = true;
            }
            GV_TBGMT_BLD.DataSource = dt.AsDataView();
            GV_TBGMT_BLD.DataBind();
        }
        protected void GV_TBGMT_BLD_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {

                GV_TBGMT_BLD.UseAccessibleHeader = true;
                GV_TBGMT_BLD.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
    }
}