using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.Z
{
    public partial class MTS_Z21_3 : System.Web.UI.Page
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
            GV_ACCOUNT.DataSource = dt.AsDataView();
            GV_ACCOUNT.DataBind();
        }
        protected void GV_ACCOUNT_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {

                GV_ACCOUNT.UseAccessibleHeader = true;
                GV_ACCOUNT.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
    }
}