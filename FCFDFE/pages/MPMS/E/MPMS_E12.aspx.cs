using System;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E12 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (!IsPostBack)
                {
                    if ((string)(Session["XSSRequest"]) == "danger")
                    {
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "輸入錯誤，請重新輸入！");
                        Session["XSSRequest"] = null;
                    }

                    GV_dataImport();

                    #region SessionRemove
                    Session.Contents.Remove("rowtext");
                    Session.Contents.Remove("rowgroup");
                    Session.Contents.Remove("rowven");
                    Session.Contents.Remove("purch_6");
                    #endregion
                }
            }
        }

        #region Click
        //接管btn
        protected void BtnTakeOver_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            Label labOVC_PURCH = (Label)GV_TakeOver.FindControl("labOVC_PURCH");
            Label labOVC_PURCH_6 = (Label)GV_TakeOver.FindControl("labOVC_PURCH_6");
            Session["rowtext"] = labOVC_PURCH.Text;
            Session["rowgroup"] = GV_TakeOver.Cells[1].Text;
            Session["rowven"] = GV_TakeOver.Cells[4].Text;
            Session["purch_6"] = labOVC_PURCH_6.Text;
            Session.Contents.Remove("E15");
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E13.aspx";
            Response.Redirect(send_url);
        }
        //回上一頁btn
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
            if (Session["userid"] != null)
            {
                var id = Session["userid"].ToString();
                var queryunit = mpms.ACCOUNT.Where(o => o.USER_ID.Equals(id)).FirstOrDefault();
                Session["userunit"] = queryunit.DEPT_SN;
                var userunit = Session["userunit"].ToString();
                var query =
                from tbm1302 in mpms.TBM1302
                join tbm1301 in mpms.TBM1301_PLAN on tbm1302.OVC_PURCH equals tbm1301.OVC_PURCH
                where tbm1302.OVC_DSEND != null && tbm1302.OVC_DSEND != " "
                where tbm1302.OVC_DRECEIVE.Equals(null) || tbm1302.OVC_DRECEIVE.Equals(" ")
                where userunit == "0A100" || userunit == "00N00" ? tbm1301.OVC_CONTRACT_UNIT.Equals("0A100") || tbm1301.OVC_CONTRACT_UNIT.Equals("00N00") : tbm1301.OVC_CONTRACT_UNIT.Equals(userunit)
                orderby tbm1301.OVC_PURCH
                select new
                {
                    OVC_PURCH = tbm1301.OVC_PURCH,
                    OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                    OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                    OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                    ONB_GROUP = tbm1302.ONB_GROUP,
                    OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE
                };
                hasRows = true;
                dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TakeOver, dt);
            }
        }
        #endregion

        #region GridView
        protected void GV_TakeOver_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Cells[0]
                Label labOVC_PURCH = (Label)e.Row.FindControl("labOVC_PURCH");
                Label labOVC_PUR_AGENCY = (Label)e.Row.FindControl("labOVC_PUR_AGENCY");
                Label labOVC_PURCH_5 = (Label)e.Row.FindControl("labOVC_PURCH_5");
                Label labOVC_PURCH_6 = (Label)e.Row.FindControl("labOVC_PURCH_6");

                labOVC_PURCH.Text = labOVC_PURCH.Text + labOVC_PUR_AGENCY.Text + labOVC_PURCH_5.Text + labOVC_PURCH_6.Text;
            }
        }
        protected void GV_TakeOver_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GV_TakeOver.UseAccessibleHeader = true;
                GV_TakeOver.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        #endregion

        #endregion
    }
}