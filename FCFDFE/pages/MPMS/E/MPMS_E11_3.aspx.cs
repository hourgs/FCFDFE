using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E11_3 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
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
                from tbmstatus in mpms.TBMSTATUS
                join tbm1301 in mpms.TBM1301_PLAN.AsEnumerable() on tbmstatus.OVC_PURCH equals tbm1301.OVC_PURCH
                join tbm1302 in mpms.TBM1302.AsEnumerable() on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                where Purchase_number != null ? (tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + tbm1302.OVC_PURCH_5 + tbm1302.OVC_PURCH_6).Contains(Purchase_number) : true
                where Purchase_name != null ? tbm1301.OVC_PUR_IPURCH.Contains(Purchase_name) : true
                where Contract_manufacturer != null ? tbm1302.OVC_VEN_TITLE.Contains(Contract_manufacturer) : true
                where tbmstatus.OVC_STATUS.Equals("3") || tbmstatus.OVC_STATUS.Equals("31") || tbmstatus.OVC_STATUS.Equals("32") || tbmstatus.OVC_STATUS.Equals("33") || tbmstatus.OVC_STATUS.Equals("34") || tbmstatus.OVC_STATUS.Equals("35") || tbmstatus.OVC_STATUS.Equals("36") || tbmstatus.OVC_STATUS.Equals("37")
                orderby tbmstatus.OVC_STATUS
                select new
                {
                    OVC_PURCH = tbmstatus.OVC_PURCH,
                    OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                    OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                    OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE,
                    ONB_TIMES = tbmstatus.ONB_TIMES,
                    OVC_DO_NAME = tbmstatus.OVC_DO_NAME,
                    OVC_STATUS = tbmstatus.OVC_STATUS,
                    OVC_DBEGIN = tbmstatus.OVC_DBEGIN,
                    OVC_DEND = tbmstatus.OVC_DEND
                };

            hasRows = true;
            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_Current, dt);
        }
        #endregion

        #region GridView
        protected void GV_Current_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Cells[0]
                Label labOVC_PURCH = (Label)e.Row.FindControl("labOVC_PURCH");
                Label labOVC_PUR_AGENCY = (Label)e.Row.FindControl("labOVC_PUR_AGENCY");
                Label labOVC_PURCH_5 = (Label)e.Row.FindControl("labOVC_PURCH_5");
                e.Row.Cells[0].Text = labOVC_PURCH.Text + labOVC_PUR_AGENCY.Text + labOVC_PURCH_5.Text;

                var status = e.Row.Cells[6].Text;
                var query =
                    from tbm1407 in gm.TBM1407
                    where tbm1407.OVC_PHR_CATE.Equals("Q9")
                    where tbm1407.OVC_PHR_ID.Equals(status)
                    select tbm1407.OVC_PHR_DESC;
                
                foreach (var q in query)
                    e.Row.Cells[6].Text = q.ToString();
            }
        }
        protected void GV_Current_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GV_Current.UseAccessibleHeader = true;
                GV_Current.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        #endregion

        #endregion
    }
}