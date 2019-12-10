using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E11_2 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            if (Session["Purchase_number"] == null && Session["Purchase_name"] == null && Session["Contract_manufacturer"] == null)
                FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
            else
            {
                if (!IsPostBack)
                {
                    GV_dataImport();
                }
            }

        }

        #region 回上一頁
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E11.aspx";
            Response.Redirect(send_url);
        }
        #endregion

        #region 副程式


        #region GridView資料帶入
        private void GV_dataImport()
        {
            DataTable dt = new DataTable();

            var Purchase_number = Session["Purchase_number"] != null ? Session["Purchase_number"].ToString() : null;
            var Purchase_name = Session["Purchase_name"] != null ? Session["Purchase_name"].ToString() : null;
            var Contract_manufacturer = Session["Contract_manufacturer"] != null ? Session["Contract_manufacturer"].ToString() : null;

            var query =
                from tbm1114 in mpms.TBM1114
                join tbm1301 in mpms.TBM1301_PLAN.AsEnumerable() on tbm1114.OVC_PURCH equals tbm1301.OVC_PURCH
                join tbm1302 in mpms.TBM1302.AsEnumerable() on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                where Purchase_number != null ? (tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + tbm1302.OVC_PURCH_5 + tbm1302.OVC_PURCH_6).Contains(Purchase_number) : true
                where Purchase_name != null ? tbm1301.OVC_PUR_IPURCH.Contains(Purchase_name) : true
                where Contract_manufacturer != null ? tbm1302.OVC_VEN_TITLE.Contains(Contract_manufacturer) : true
                select new
                {
                    OVC_PURCH = tbm1114.OVC_PURCH,
                    OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                    OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE,
                    OVC_DATE = tbm1114.OVC_DATE,
                    OVC_USER = tbm1114.OVC_USER,
                    OVC_FROM_UNIT_NAME = tbm1114.OVC_FROM_UNIT_NAME,
                    OVC_TO_UNIT_NAME = tbm1114.OVC_TO_UNIT_NAME,
                    OVC_REMARK = tbm1114.OVC_REMARK
                };

            hasRows = true;
            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_Transferring, dt);
        }
        #endregion

        #region GridView
        protected void GV_Transferring_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Cells[0]
                Label labOVC_PURCH = (Label)e.Row.FindControl("labOVC_PURCH");
                Label labOVC_PUR_AGENCY = (Label)e.Row.FindControl("labOVC_PUR_AGENCY");
                e.Row.Cells[0].Text = labOVC_PURCH.Text + labOVC_PUR_AGENCY.Text;
            }
        }
        //
        protected void GV_Transferring_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GV_Transferring.UseAccessibleHeader = true;
                GV_Transferring.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        #endregion

        #endregion
    }
}