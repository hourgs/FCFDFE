using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;

namespace FCFDFE.pages.MTS.A
{   
    public partial class ITEMLIST : System.Web.UI.Page
    {
        private MTSEntities ae = new MTSEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var query =
                    ae.TBGMT_ITEMLIST.Select
                    (cus => new
                    {
                        OVC_CHI_NAME = cus.OVC_CHI_NAME
                    }).ToList();
                drpQuery.DataSource = query;
                drpQuery.DataValueField = "OVC_CHI_NAME";
                drpQuery.DataBind();
                drpQueryText.Text = drpQuery.SelectedItem.ToString();
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                var query =
                    ae.TBGMT_ITEMLIST.Where(table => table.OVC_CHI_NAME.Contains(txtQuery.Text.Trim())).Select
                    (cus => new
                    {
                        OVC_CHI_NAME = cus.OVC_CHI_NAME
                    }).ToList();

                if (query.Count > 0)
                {
                    drpQuery.DataSource = query;
                    drpQuery.DataValueField = "OVC_CHI_NAME";
                    drpQuery.DataBind();
                    drpQueryText.Text = drpQuery.SelectedItem.ToString();
                }
            }
            catch { }
        }

        protected void drpQuery_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpQueryText.Text = drpQuery.SelectedItem.ToString();
        }
    }
}