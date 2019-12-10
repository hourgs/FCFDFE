using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;
using System.Data;
using FCFDFE.Content;

namespace FCFDFE.Content
{
    public partial class phraseQuery : System.Web.UI.Page
    {
        private GMEntities ue = new GMEntities();
        //public string FDE, NAME;
        Common FCommon = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //FDE = Request.QueryString["FDE"] != null ? Request.QueryString["FDE"].ToString() : "txtFCODE";
                //NAME = Request.QueryString["NAME"] != null ? Request.QueryString["NAME"].ToString() : "txtNAME";

                var query =
                    ue.TBM2133.Where(id=>id.FCODE != null).
                    Select
                    (cus => new
                    {
                        Name = cus.NAME,
                        Fcode = cus.FCODE
                    });
                DataTable dt = new DataTable();
                DataRow dr;

                // Define the columns of the table.
                dt.Columns.Add(new DataColumn("name", typeof(String)));
                dt.Columns.Add(new DataColumn("code", typeof(String)));

                foreach(var aa in query)
                {
                    dr = dt.NewRow();
                    var name = aa.Fcode +""+ aa.Name;
                    dr[0] = name;
                    dr[1] = aa.Fcode;
                    dt.Rows.Add(dr);
                }

                drpQuery.DataSource = dt;
                drpQuery.DataValueField ="code" ;
                drpQuery.DataTextField = "name";
                drpQuery.DataBind();

                drpQueryCode.Text = drpQuery.SelectedValue.ToString();
                var getdata = ue.TBM2133.Where(id => id.FCODE == drpQueryCode.Text).First();
                drpQueryText.Text = getdata.NAME;

            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            var query =
                    ue.TBM2133.Where(id => id.FCODE != null).Where(name=>name.NAME.Contains(txtQuery.Text)).
                    Select
                    (cus => new
                    {
                        Name = cus.NAME,
                        Fcode = cus.FCODE
                    });
            DataTable dt = new DataTable();
            DataRow dr;

            // Define the columns of the table.
            dt.Columns.Add(new DataColumn("name", typeof(String)));
            dt.Columns.Add(new DataColumn("code", typeof(String)));

            foreach (var aa in query)
            {
                dr = dt.NewRow();
                var name = aa.Fcode + "" + aa.Name;
                dr[0] = name;
                dr[1] = aa.Fcode;
                dt.Rows.Add(dr);
            }

            drpQuery.DataSource = dt;
            drpQuery.DataValueField = "code";
            drpQuery.DataTextField = "name";
            drpQuery.DataBind();

            if (query.Count() > 0)
            {
                drpQueryCode.Text = drpQuery.SelectedValue.ToString();
                var getdata = ue.TBM2133.Where(id => id.FCODE == drpQueryCode.Text).FirstOrDefault();
                drpQueryText.Text = getdata.NAME;
            }
        }

        protected void drpQuery_SelectedIndexChanged(object sender, EventArgs e)
        {

            drpQueryCode.Text = drpQuery.SelectedValue.ToString();
            var getdata = ue.TBM2133.Where(id => id.FCODE == drpQueryCode.Text).First();
            drpQueryText.Text = getdata.NAME;
        }

        protected void btnCodeQuery_Click(object sender, EventArgs e)
        {
            //var query = (from tb in ue.TBM2133
            //             where tb.FCODE != null
            //             where tb.FCODE.Contains(txtCodeQuery.Text)
            //             select tb).ToList();
            //DataTable dataTable = CommonStatic.ListToDataTable(query);
            //list_dataImportV(drpQuery, dataTable, "FCODE", "NAME");
            //drpQueryCode.Text = drpQuery.SelectedValue.ToString();
            //var getdata = ue.TBM2133.Where(id => id.FCODE == drpQueryCode.Text).First();
            //drpQueryText.Text = getdata.NAME;
            var query =
                    ue.TBM2133.Where(id => id.FCODE != null).Where(name => name.FCODE.Contains(txtCodeQuery.Text)).
                    Select
                    (cus => new
                    {
                        Name = cus.NAME,
                        Fcode = cus.FCODE
                    });
            DataTable dt = new DataTable();
            DataRow dr;

            // Define the columns of the table.
            dt.Columns.Add(new DataColumn("name", typeof(String)));
            dt.Columns.Add(new DataColumn("code", typeof(String)));

            foreach (var aa in query)
            {
                dr = dt.NewRow();
                var name = aa.Fcode + "" + aa.Name;
                dr[0] = name;
                dr[1] = aa.Fcode;
                dt.Rows.Add(dr);
            }

            drpQuery.DataSource = dt;
            drpQuery.DataValueField = "code";
            drpQuery.DataTextField = "name";
            drpQuery.DataBind();

            if (query.Count() > 0)
            {
                drpQueryCode.Text = drpQuery.SelectedValue.ToString();
                var getdata = ue.TBM2133.Where(id => id.FCODE == drpQueryCode.Text).FirstOrDefault();
                drpQueryText.Text = getdata.NAME;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Session.Remove("phrasequery");
        }
    }
}