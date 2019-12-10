using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MPMS.A
{
    public partial class MPMS_A13 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string[] strField = { "OVC_PURCH", "OVC_PUR_IPURCH", "OVC_AGENT_UNIT", "OVC_PUR_USER", "OVC_DAPPLY", "OVC_DAUDIT", "OVC_PURCH_OK" };

        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            if (!IsPostBack)
            {
                list_dataImport(drpOVC_BUDGET_YEAR);
            }
        }
        

        #region 副程式
        private void dataImport()
        {
            if (Session["userid"] != null)
            {
                string strUSER_ID = Session["userid"].ToString();
                
                DataTable dt = new DataTable();
                if (strUSER_ID.Length > 0)
                {
                    ACCOUNT ac = new ACCOUNT();
                    string userName="";
                    ac = gm.ACCOUNTs.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                    if (ac != null) {
                        userName = ac.USER_NAME.ToString();
                    }
                    string strOVC_BUDGET_YEAR = drpOVC_BUDGET_YEAR.SelectedItem.ToString();
                    string Compare = strOVC_BUDGET_YEAR.Substring((strOVC_BUDGET_YEAR.Length-2), 2);
                    var query =
                        from plan1301 in gm.TBM1301_PLAN.DefaultIfEmpty().AsEnumerable()
                        where plan1301.OVC_PURCH.Substring(2, 2).Equals(Compare)
                        where plan1301.OVC_PUR_USER == userName
                        select new
                        {
                            OVC_PURCH = plan1301.OVC_PURCH,
                            OVC_PUR_IPURCH = plan1301.OVC_PUR_IPURCH,
                            OVC_AGENT_UNIT = plan1301.OVC_AGENT_UNIT,
                            OVC_PUR_USER = plan1301.OVC_PUR_USER,
                            OVC_DAPPLY = plan1301.OVC_DAPPLY,
                            OVC_DAUDIT = plan1301.OVC_DAUDIT,
                            OVC_PURCH_OK = plan1301.OVC_PURCH_OK,
                        };

                    dt = CommonStatic.LinqQueryToDataTable(query);
                }
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_OVC_BUDGET, dt, strField);
            }
        }

        private void list_dataImport(ListControl list)
        {
            //先將下拉式選單清空
            list.Items.Clear();

            int CalDateYear = Convert.ToInt16(DateTime.Now.ToString("yyyy")) - 1911;
            int num = CalDateYear;
            for (int i = 0; i < 15; i++)
            {
                list.Items.Add(Convert.ToString(num));
                num = num - 1;
            }
        }
        #endregion
        protected void btnQuery_OVC_BUDGET_YEAR_Click(object sender, EventArgs e)
        {
            //上方年度查詢按鈕
            dataImport();
        }

        protected void GV_OVC_BUDGET_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_OVC_BUDGET.DataKeys[gvrIndex].Value.ToString(); //OVC_PURCH

            switch (e.CommandName)
            {
                case "DataQuery":
                    string strPurchNum = GV_OVC_BUDGET.Rows[gvrIndex].Cells[0].Text;
                    string send_url;
                    send_url = "~/pages/MPMS/A/MPMS_A12.aspx?PurchNum_13=" + strPurchNum;
                    Response.Redirect(send_url);
                    break;
                default:
                    break;
            }
        }

        protected void GV_OVC_BUDGET_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string strpurch = e.Row.Cells[0].Text;
                var query = gm.TBM1301.Where(t => t.OVC_PURCH.Equals(strpurch) && t.OVC_PUR_DCANPO != null).FirstOrDefault();
                if (query != null)
                    e.Row.Visible = false;
            }
        }

        protected void GV_OVC_BUDGET_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
            {
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            }
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}