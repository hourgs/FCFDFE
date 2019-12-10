using System;
using System.Data;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MPMS.F
{
    public partial class FListedKind : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("column1");
            dt.Columns.Add("column2");
            dt.Columns.Add("column3");
            for (int i = 0; i < 2; i++)
            {
                DataRow dr = dt.NewRow();
                dr["column1"] = "資料鏈結";
                dr["column2"] = "資料鏈結";
                dr["column3"] = "資料鏈結";
                dt.Rows.Add(dr);
                hasRows = true;
            }
        }
    }
}