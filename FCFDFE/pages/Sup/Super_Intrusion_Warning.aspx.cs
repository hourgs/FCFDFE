using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.Entity;

namespace FCFDFE.pages.Sup
{
    public partial class Super_Intrusion_Warning : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string[] strField = { "OVC_PURCH", "OVC_PUR_IPURCH", "OVC_AGENT_UNIT", "OVC_PUR_USER", "OVC_DAPPLY", "OVC_DAUDIT", "OVC_PURCH_OK" };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                DataImport();
        }

        protected void GV_USER_LOGIN_ERRTRY_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }


        protected void query_Click(object sender, EventArgs e)
        {
            DataImport();
        }


        private void DataImport()
        {
            string ip = txtIP.Text.ToString();
            string sdate = txt_SDATE.Text.ToString();
            string edate = txt_EDATE.Text.ToString();
            var query =
                from ULE in gm.USER_LOGIN_ERRTRY
                orderby ULE.ULE_CREATEDATE descending
                select new
                {
                    SN = ULE.ULE_SN,
                    IP = ULE.ULE_IP,
                    DATE = ULE.ULE_CREATEDATE,
                    REASON = ULE.ULE_REASON,
                };
            if (ip != "")
                query = query.Where(x => x.IP.Contains(ip));
            if (sdate != "")
            {
                DateTime Date1 = Convert.ToDateTime(sdate);
                query = query.Where(table => table.DATE >= Date1);
            }
            if (edate != "")
            {
                DateTime Date2 = Convert.ToDateTime(edate);
                query = query.Where(table => table.DATE <= Date2);
            }
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_USER_LOGIN_ERRTRY, dt);
            GV_USER_LOGIN_ERRTRY.Visible = true;
        }
    }
}