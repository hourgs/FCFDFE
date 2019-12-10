using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.Sup
{
    public partial class Super_User_Track : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string[] strField = { "OVC_PURCH", "OVC_PUR_IPURCH", "OVC_AGENT_UNIT", "OVC_PUR_USER", "OVC_DAPPLY", "OVC_DAUDIT", "OVC_PURCH_OK" };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                GV_UserTrack.Visible = false;
        }

        protected void GV_UserTrack_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void query_Click(object sender, EventArgs e)
        {
            string ip = txtUSER_IP.Text.ToString();
            string id = txtUSER_ID.Text.ToString();
            string sdate=txt_SDATE.Text.ToString();
            string edate=txt_EDATE.Text.ToString();
            var query =
                from LOG in gm.ALLSYS_LOG
                join AC in gm.ACCOUNTs on LOG.AS_USER_ID equals AC.USER_ID
                orderby LOG.AS_DATE descending
                select new
                {
                    USER_ID=LOG.AS_USER_ID,
                    USER_NAME=AC.USER_NAME,
                    DATE=LOG.AS_DATE,
                    IP=LOG.AS_IP,
                    PAGE=LOG.AS_PAGE,
                    TB=LOG.AS_COMMAND,
                    ACT=LOG.AS_ACT,
                    SN=LOG.AS_SN
            };
            if (ip != "")
                query = query.Where(x => x.IP.Contains(ip));
            if (id != "")
                query = query.Where(x => x.USER_ID.Contains(id));
            if (sdate != "")
            {
                DateTime Date1 = Convert.ToDateTime(sdate);
                query = query.Where(table =>table.DATE>= Date1);
            }
            if (edate != "")
            {
                DateTime Date2 = Convert.ToDateTime(edate);
                query = query.Where(table => table.DATE<= Date2);
            }


            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_UserTrack, dt);
            GV_UserTrack.Visible = true;

        }



        #region 副程式
        //private void dataImport()
        //{
        //    if (Session["userid"] != null)
        //    {
        //        string strUSER_ID = Session["userid"].ToString();

        //        DataTable dt = new DataTable();
        //        if (strUSER_ID.Length > 0)
        //        {
        //            ACCOUNT ac = new ACCOUNT();
        //            string userName="";
        //            ac = gm.ACCOUNTs.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
        //            if (ac != null) {
        //                userName = ac.USER_NAME.ToString();
        //            }
        //            string Compare = strOVC_BUDGET_YEAR.Substring((strOVC_BUDGET_YEAR.Length-2), 2);
        //            var query =
        //                from plan1301 in gm.TBM1301_PLAN.DefaultIfEmpty().AsEnumerable()
        //                where plan1301.OVC_PURCH.Substring(2, 2).Equals(Compare)
        //                where plan1301.OVC_PUR_USER == userName
        //                select new
        //                {
        //                    OVC_PURCH = plan1301.OVC_PURCH,
        //                    OVC_PUR_IPURCH = plan1301.OVC_PUR_IPURCH,
        //                    OVC_AGENT_UNIT = plan1301.OVC_AGENT_UNIT,
        //                    OVC_PUR_USER = plan1301.OVC_PUR_USER,
        //                    OVC_DAPPLY = plan1301.OVC_DAPPLY,
        //                    OVC_DAUDIT = plan1301.OVC_DAUDIT,
        //                    OVC_PURCH_OK = plan1301.OVC_PURCH_OK,
        //                };

        //            dt = CommonStatic.LinqQueryToDataTable(query);
        //        }
        //        ViewState["hasRows"] = FCommon.GridView_dataImport(GV_OVC_BUDGET, dt, strField);
        //    }
        //}
        #endregion

    }
}