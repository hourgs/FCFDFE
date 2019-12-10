using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;

namespace FCFDFE.pages.GM
{
    public partial class DeptSubUnit : Page
    {
        GMEntities GME = new GMEntities();
        MTSEntities MTSE = new MTSEntities();
        string topDept = "";
        int rowCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["topDept"]))
            {
                topDept = Request.QueryString["topDept"];
                if (!IsPostBack)
                {
                    Query();
                }
            }
        }

        private void Query()
        {
            var queryDept = GME.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(topDept)).FirstOrDefault();
            lblTitleUnit.Text = topDept + "：" + queryDept.OVC_ONNAME;
            var query = GME.TBMDEPTs.Where(table => table.OVC_TOP_DEPT.Equals(topDept));
            var queryF =
                from tDept in query.ToList()
                join t1407 in GME.TBM1407 on tDept.OVC_CLASS equals t1407.OVC_PHR_ID
                join tDEPT_NAME in GME.TBMDEPTs on tDept.OVC_TOP_DEPT equals tDEPT_NAME.OVC_DEPT_CDE into topName
                from tDEPT_NAME in topName.DefaultIfEmpty()
                join tDEPT_PURCHASE_NAME in GME.TBMDEPTs on tDept.OVC_PURCHASE_DEPT equals tDEPT_PURCHASE_NAME.OVC_DEPT_CDE
                join tTOPNAME2 in MTSE.TBGMT_DEPT_CLASS.AsEnumerable() on tDept.OVC_CLASS2 equals tTOPNAME2.OVC_CLASS into ps
                from tTOPNAME2 in ps.DefaultIfEmpty()
                where t1407.OVC_PHR_CATE.Equals("S9")
                select new
                {
                    tDept.OVC_DEPT_CDE,
                    tDept.OVC_ONNAME,
                    tDept.OVC_PURCHASE_OK,
                    tDept.OVC_TOP_DEPT,
                    OVC_TOP_DEPT_NAME = tDEPT_NAME != null ? tDEPT_NAME.OVC_ONNAME : "",
                    tDept.OVC_PURCHASE_DEPT,
                    OVC_PURCHASE_DEPT_NAME = tDEPT_PURCHASE_NAME.OVC_ONNAME,
                    OVC_CLASS_NAME = t1407.OVC_PHR_DESC,
                    OVC_CLASS2_NAME = tTOPNAME2 != null ? tTOPNAME2.OVC_CLASS_NAME : "",
                    tDept.OVC_ENABLE,
                };
            if (queryF.Any())
            {
                DataTable dt = CommonStatic.LinqQueryToDataTable(queryF);
                Querylist.DataSource = dt;
                rowCount = dt.Rows.Count;
                Querylist.DataBind();
            }
            
        }
        protected void Querylist_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                System.Data.DataRowView DRV = (System.Data.DataRowView)e.Item.DataItem;
                Label lblPURCHASE_OK = (Label)e.Item.FindControl("lblPURCHASE_OK");
                Label lblOVC_ENABLE = (Label)e.Item.FindControl("lblOVC_ENABLE");
                if (lblPURCHASE_OK.Text.Equals("Y"))
                {
                    
                    lblPURCHASE_OK.Text = "是";
                }
                else
                {
                    lblPURCHASE_OK.Text = "否";
                }

                switch (lblOVC_ENABLE.Text)
                {
                    case "0":
                        lblOVC_ENABLE.Text = "停用";
                        break;
                    case "1":
                        lblOVC_ENABLE.Text = "現用";
                        break;
                    case "2":
                        lblOVC_ENABLE.Text = "戰時";
                        break;
                }

            }
        }

        protected void Querylist_DataBound(object sender, EventArgs e)
        {
            Label lblSum = (Label)Querylist.FindControl("lblSum");
            lblSum.Text = rowCount.ToString();
        }
    }
}