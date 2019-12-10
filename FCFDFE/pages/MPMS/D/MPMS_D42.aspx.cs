using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.IO;
using System.Globalization;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D42 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            if (!IsPostBack)
            {
                list_dataImport(drpOVC_BUDGET_YEAR);
            }
        }

        protected void GV_Query_PLAN_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
            {
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            }
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            DataImport();
        }

        private void DataImport()
        {
            string year = drpOVC_BUDGET_YEAR.SelectedValue.ToString();
            string yearSub = year.Substring((year.Length - 2), 2);
            string strOVC_PUR_APPROVE = txtOVC_PUR_APPROVE.Text;

            var query =
                from t in mpms.TBM1202
                join t1301 in mpms.TBM1301 on t.OVC_PURCH equals t1301.OVC_PURCH
                where t.OVC_CHECK_OK.Equals("Y") && t.OVC_PURCH.Substring(2, 2).Equals(yearSub)
                select new
                {
                    OVC_PURCH = t.OVC_PURCH,
                    OVC_PUR_AGENCY = t1301.OVC_PUR_AGENCY,
                    OVC_PUR_IPURCH = t1301.OVC_PUR_IPURCH
                };
            if (!strOVC_PUR_APPROVE.Equals(String.Empty))
            {
                query = query.Where(o => o.OVC_PUR_AGENCY.Equals(strOVC_PUR_APPROVE));
            }

            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_Query_PLAN, dt);
            
        }

      

        protected void GV_Query_PLAN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strPurchNum = e.CommandArgument.ToString();
            switch (e.CommandName)
            {
                case "btnToAUDIT":
                    string url = "~/pages/MPMS/D/MPMS_D43.aspx?PurchNum=" + strPurchNum;
                    Response.Redirect(url);
                    break;
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string url = "~/pages/MPMS/D/MPMS_D14";
            Response.Redirect(url);
        }

        private void list_dataImport(ListControl list)
        {
            //帶入計畫年度下拉選單的值
            //先將下拉式選單清空
            list.Items.Clear();

            //取得台灣年月日
            DateTime datetime = DateTime.Now;
            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            int CalDateYear = Convert.ToInt16(datetime.ToString("yyy", culture));
            int num = CalDateYear;
            for (int i = 0; i < 15; i++)
            {
                list.Items.Add(Convert.ToString(num));
                num = num - 1;
            }
        }

    }
}