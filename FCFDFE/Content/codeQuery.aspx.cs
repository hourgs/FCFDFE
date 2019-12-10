using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;
using System.Data;
using FCFDFE.Content;
using System.Data.Entity;

namespace FCFDFE.Content
{
    public partial class codeQuery : System.Web.UI.Page
    {
        private GMEntities ue = new GMEntities();
        Common FCommon = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dataTable = CommonStatic.ListToDataTable(ue.TBMDEPTs.Select(x => x).ToList());
                list_dataImportV(drpQuery, dataTable, "OVC_DEPT_CDE", "OVC_ONNAME");
                drpCodeQueryText.Text = drpQuery.SelectedValue.ToString();
                var getdata = ue.TBMDEPTs.Where(id => id.OVC_DEPT_CDE == drpCodeQueryText.Text).FirstOrDefault();
                drpNameQueryText.Text = getdata.OVC_ONNAME;
            }
        }

        protected void btnCodeQuery_Click(object sender, EventArgs e)
        {

            DataTable dataTable = CommonStatic.ListToDataTable(ue.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Contains(txtCodeQuery.Text)).ToList());
            list_dataImportV(drpQuery, dataTable, "OVC_DEPT_CDE", "OVC_ONNAME");
        }
        private void list_dataImportV(ListControl list, DataTable dt, string textField, string valueField)
        {

            string strFieldName = valueField + textField;
            dt.Columns.Add(strFieldName);
            foreach (DataRow dr in dt.Rows)
            {
                dr[strFieldName] = dr[textField].ToString() + "" + dr[valueField].ToString();
            }

            list.DataSource = dt;
            list.DataTextField = strFieldName;
            list.DataValueField = textField;
            list.DataBind();
        }
        protected void btnNameQuery_Click(object sender, EventArgs e)
        {
            DataTable dataTable = CommonStatic.ListToDataTable(ue.TBMDEPTs.Where(table => table.OVC_ONNAME.Contains(txtNameQuery.Text)).ToList());
            list_dataImportV(drpQuery, dataTable, "OVC_DEPT_CDE", "OVC_ONNAME");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        protected void drpQuery_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpCodeQueryText.Text = drpQuery.SelectedValue.ToString();
            var getdata = ue.TBMDEPTs.Where(id => id.OVC_DEPT_CDE == drpCodeQueryText.Text).FirstOrDefault();
            drpNameQueryText.Text = getdata.OVC_ONNAME;
        }
    }
}