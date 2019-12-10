using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;
using System.Data;
using FCFDFE.Content;

namespace FCFDFE.pages.MPMS.A
{
    public partial class unitQuery : Page
    {
        //public string status;
        //public string CDE,NAME;
        public string CDE, NAME;
        Common FCommon = new Common();
        GMEntities GME = new GMEntities();
        #region 副程式
        private void dataImport_list()
        {
            string strOVC_DEPT_CDE = txtOVC_DEPT_CDE.Text;
            string strOVC_ONNAME = txtOVC_ONNAME.Text;
            var query = from depts in GME.TBMDEPTs select depts;
            if (!string.IsNullOrEmpty(strOVC_DEPT_CDE))
                query = query.Where(table => table.OVC_DEPT_CDE.Contains(strOVC_DEPT_CDE));
            if (!string.IsNullOrEmpty(strOVC_ONNAME))
                query = query.Where(table => table.OVC_ONNAME.Contains(strOVC_ONNAME));
            DataTable dt = CommonStatic.ListToDataTable(query);
            FCommon.list_dataImportV(drpQuery, dt, "OVC_ONNAME", "OVC_DEPT_CDE", false);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtValue.Text = dr["OVC_DEPT_CDE"].ToString();
                txtText.Text = dr["OVC_ONNAME"].ToString();
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查無資料");
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getQueryString(this, "CDE", out CDE, true);
            FCommon.getQueryString(this, "NAME", out NAME, true);
            if (!IsPostBack)
            {
                dataImport_list();
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport_list();
        }

        protected void drpQuery_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strValue = drpQuery.SelectedValue, strText = "";
            if (!strValue.Equals(string.Empty))
            {
                TBMDEPT dept = GME.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(strValue)).FirstOrDefault();
                if (dept != null) strText = dept.OVC_ONNAME;
            }
            txtValue.Text = strValue;
            txtText.Text = strText;
        }
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    Session.Remove("unitquery");
        //}
    }
}