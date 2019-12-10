using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.E
{
    public partial class MTS_E24_2 : System.Web.UI.Page
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
            GridViewTBGMT_CINF.DataSource = dt.AsDataView();
            GridViewTBGMT_CINF.DataBind();
        }
        protected void GridViewTBGMT_CINF_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {

                GridViewTBGMT_CINF.UseAccessibleHeader = true;
                GridViewTBGMT_CINF.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
    }
}