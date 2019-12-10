using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System.Data;

namespace FCFDFE.pages.MPMS.B
{
    public partial class MPMS_B13_8_1 : System.Web.UI.Page
    {

        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        string strDocTypeNO = "";
        string strDocNo = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (string.IsNullOrEmpty(Request.QueryString["DocTypeNO"]) ||
                    string.IsNullOrEmpty(Request.QueryString["DocNO"]) ||
                    string.IsNullOrEmpty(Request.QueryString["PurchNum"]))
                {

                }
                else
                {
                    strDocTypeNO = Request.QueryString["DocTypeNO"].ToString();
                    strDocNo = Request.QueryString["DocNO"].ToString();
                    if (!IsPostBack)
                    {
                        LoginScreen();
                        DataImport();
                    }
                }
            }
        }
        
        protected void btn_Modify_Click(object sender, EventArgs e)
        {
            Button btnThis = (Button)sender;
            int gvRowIndex = (btnThis.NamingContainer as GridViewRow).RowIndex;
            string strLawNo = GV_DOCLAW.Rows[gvRowIndex].Cells[0].Text;
            string url = "~/pages/MPMS/B/MPMS_B13_8_2.aspx?PurchNum="+ Request.QueryString["PurchNum"] + "&DocTypeNO=" + Request.QueryString["DocTypeNO"] + "&DocNO=" 
                + Request.QueryString["DocNO"] + "&LawNo=" + strLawNo;
            Response.Redirect(url);
        }

        protected void btn_Return_Click(object sender, EventArgs e)
        {
            string url = "~/pages/MPMS/B/MPMS_B13_8.aspx?PurchNum=" + Request.QueryString["PurchNum"];
            Response.Redirect(url);
        }

        private void LoginScreen()
        {
            string strTypeName = mpms.TBMDOCTYPE.Where(o => o.DOC_TYPENO.Equals(strDocTypeNO)).Select(o => o.DOC_NAME).First();
            lblDOCTYPE.Text = strTypeName;
        }

        private void DataImport()
        {
            string[] strField = { "DOC_LAW_NO", "DOC_LAW_NAME", "DOC_NO" };
            var query =
                from table in mpms.TBMDOCLAW
                where table.DOC_TYPENO.Equals(strDocTypeNO) && table.DOC_NO.Equals(strDocNo)
                orderby table.DOC_LAW_NO.Length, table.DOC_LAW_NO
                select new
                {
                    table.DOC_LAW_NO,
                    table.DOC_LAW_NAME,
                    table.DOC_NO
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            int max = dt.Rows.Count;

            var queryDocContent = mpms.TBMDOCCONTENT.Where(o => o.DOC_TYPENO.Equals(strDocTypeNO) && o.DOC_NO.Equals(strDocNo)).DefaultIfEmpty();
            DataTable dtDocContent = CommonStatic.LinqQueryToDataTable(queryDocContent);
            for (int i = 1; i <= max; i++)
            {
                var queryCommit =
                    from table in dtDocContent.AsEnumerable()
                    where table.Field<string>("DOC_LAW_NO").Equals(i.ToString()) && table.Field<string>("DOC_COMMIT").Equals("0")
                    select table;

                if (queryCommit.Any())
                    dt.Rows[i - 1]["DOC_NO"] = "未完成";
                else
                    dt.Rows[i - 1]["DOC_NO"] = "已完成";
            }
            FCommon.GridView_dataImport(GV_DOCLAW, dt, strField);
        }

    }
}