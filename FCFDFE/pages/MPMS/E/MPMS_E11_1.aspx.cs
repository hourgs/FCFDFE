using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E11_1 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            if (Session["CaseFrom"] != null && Session["CaseTo"] != null)
            {
                if (!IsPostBack)
                {
                    GV_dataImport();
                }
            }
            else
                FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
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
            if (Session["CaseFrom"] != null && Session["CaseTo"] != null)
            {
                DataTable dt = new DataTable();
                var dtCaseFrom = Session["CaseFrom"].ToString();
                var dtCaseTo = Session["CaseTo"].ToString();

                #region 當日期dtCaseFrom比dtCaseTo晚時，兩邊交換值
                if (dtCaseFrom.CompareTo(dtCaseTo) > 0)
                {
                    dtCaseFrom = Session["CaseTo"].ToString();
                    dtCaseTo = Session["CaseFrom"].ToString();
                }
                #endregion

                var query =
                    from tbm1301 in mpms.TBM1301
                    join tbmreceive in mpms.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmreceive.OVC_PURCH
                    where tbmreceive.OVC_STATUS.Equals("已結案")
                    where !tbmreceive.OVC_DCLOSE.Equals(null)
                    where tbmreceive.OVC_DCLOSE.CompareTo(dtCaseFrom) >=0 && tbmreceive.OVC_DCLOSE.CompareTo(dtCaseTo) <= 0
                    orderby tbmreceive.OVC_DCLOSE descending
                    select new
                    {
                        OVC_PURCH = tbm1301.OVC_PURCH,
                        OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                        OVC_PUR_USER = tbm1301.OVC_PUR_USER,
                        OVC_PUR_APPROVE = tbm1301.OVC_PUR_APPROVE,
                        OVC_PUR_DAPPROVE = tbm1301.OVC_PUR_DAPPROVE,
                        OVC_DO_NAME = tbmreceive.OVC_DO_NAME,
                        OVC_STATUS = tbmreceive.OVC_STATUS,
                        OVC_DCLOSE = tbmreceive.OVC_DCLOSE
                    };
                hasRows = true;
                dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_Reports, dt);
            }
        }
        #endregion

        #region GridView
        protected void GV_Reports_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GV_Reports.UseAccessibleHeader = true;
                GV_Reports.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        #endregion

        #endregion
    }
}