using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Content;
using System.Data;

namespace FCFDFE.pages.Sup
{
    public partial class Super_Purchase_History_3 : System.Web.UI.Page
    {

            public string strMenuName = "", strMenuNameItem = "";
            private GMEntities gme = new GMEntities();
            MPMSEntities mpms = new MPMSEntities();
            Common FCommon = new Common();
            string[] strField = { "OVC_PURCH", "OVC_PURCH_5", "ONB_TIMES", "OVC_DOPEN",
            "OVC_OPEN_METHOD", "OVC_RESULT", "OVC_BID_METHOD", "ONB_BID_VENDORS", "ONB_BID_BUDGET", "ONB_BID_RESULT" };
            protected void Page_Load(object sender, EventArgs e)
            {
                if (Request.QueryString["OVC_PURCH"] == null)
                {
                    Response.Write("<script>alert('系統檢測到您未依照正確方式進入此頁面，將導至登入畫面!'); location.href='../../../logout.aspx'; </script>");
                    return;
                }
                if (!IsPostBack)
                {
                    string enkey = Request.QueryString["OVC_PURCH"];
                    string id = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(enkey));
                    string enkey6 = Request.QueryString["OVC_PURCH_6"];
                    string id6 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(enkey6));
                    data(id, id6);
                    data1303(id);
                }

            }
            private void data(string id, string id6)
            {
                var query = mpms.TBM1302.Where(pid => pid.OVC_PURCH == id).Where(pid6 => pid6.OVC_PURCH_6 == id6).FirstOrDefault();
                var query1301 = mpms.TBM1301.Where(pid => pid.OVC_PURCH == id).FirstOrDefault();
                lblOVC_PURCH.Text = id;
                lblOVC_PURCH_6.Text = id6;
                lblOVC_PUR_IPURCH1301.Text = query1301.OVC_PUR_IPURCH;
                if (query.OVC_DBID != null)
                    lblOVC_DBID.Text = Convert.ToDateTime(query.OVC_DBID).ToString(Variable.strDateFormat);
                else
                    lblOVC_DBID.Text = "";
                lblOVC_PURCH_5.Text = query.OVC_PURCH_5;
                lblONB_GROUP.Text = query.ONB_GROUP.ToString();
                lblOVC_VEN_CST.Text = query.OVC_VEN_CST;
                lblOVC_VEN_TITLE.Text = query.OVC_VEN_TITLE;
                lblOVC_VEN_TEL.Text = query.OVC_VEN_TEL;
                lblONB_MONEY.Text = query.ONB_MONEY.ToString();
                lblONB_RATE.Text = query.ONB_RATE.ToString();
                lblONB_MONEY_NTD.Text = (query.ONB_MONEY * query.ONB_RATE).ToString();
                string current = query.OVC_CURRENT;
                var queryB0 = mpms.TBM1407.Where(table => table.OVC_PHR_CATE == "B0").Where(table => table.OVC_PHR_ID == current).FirstOrDefault();
                if (queryB0 != null)
                    lblOVC_CURRENT.Text = query.OVC_CURRENT + ":" + queryB0.OVC_PHR_DESC;
                else
                    lblOVC_CURRENT.Text = query.OVC_CURRENT;
                lblOVC_CONTRACT_COMM.Text = query.OVC_CONTRACT_COMM;

            }

            private void data1303(string id)
            {
                DataTable dt = new DataTable();
                var query =
                     from tb1303 in mpms.TBM1303
                     join tb1407 in mpms.TBM1407 on tb1303.OVC_RESULT equals tb1407.OVC_PHR_ID
                     where tb1407.OVC_PHR_CATE == "A8"
                     join tb14072 in mpms.TBM1407 on tb1303.OVC_BID_METHOD equals tb14072.OVC_PHR_ID
                     where tb14072.OVC_PHR_CATE == "R9"
                     where tb1303.OVC_PURCH == id
                     select new
                     {
                         OVC_PURCH = tb1303.OVC_PURCH,
                         OVC_PURCH_5 = tb1303.OVC_PURCH_5,
                         ONB_TIMES = tb1303.ONB_TIMES,
                         OVC_DOPEN = tb1303.OVC_DOPEN,
                         OVC_OPEN_METHOD = tb1303.OVC_OPEN_METHOD,
                         OVC_RESULT = tb1407.OVC_PHR_DESC,
                         OVC_BID_METHOD = tb14072.OVC_PHR_DESC,
                         ONB_BID_VENDORS = tb1303.ONB_BID_VENDORS,
                         ONB_BID_BUDGET = tb1303.ONB_BID_BUDGET,
                         ONB_BID_RESULT = tb1303.ONB_BID_RESULT
                     };
                dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_1303, dt, strField);
            }


            protected void GVTBGMT_1303_RowCommand(object sender, GridViewCommandEventArgs e)
            {

                string enkey = Request.QueryString["OVC_PURCH"];
                string id = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(enkey));
                string pkey = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(id));

                GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
                int gvrIndex = gvr.RowIndex;
                string pid = GVTBGMT_1303.DataKeys[gvrIndex].Value.ToString();
                string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(pid));
                switch (e.CommandName)
                {
                    case "btnDopen":
                        LinkButton lb = (LinkButton)e.CommandSource;
                        string id5 = e.CommandArgument.ToString();
                        string pkey5 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(id5));
                        string str_url_Modify;
                        str_url_Modify = "Super_Purchase_History_3_1.aspx?OVC_PURCH=" + pkey + "&OVC_PURCH_5=" + pkey5 + "&OVC_DOPEN=" + key;
                        Response.Redirect(str_url_Modify);
                        break;
                    default:
                        break;
                }
            }

            protected void btnBack_Click(object sender, EventArgs e)
            {
                Response.Redirect("Super_Purchase_History.aspx");
            }

            protected void GVTBGMT_1303_PreRender(object sender, EventArgs e)
            {
                bool hasRows = false;
                if (ViewState["hasRows"] != null)
                    hasRows = Convert.ToBoolean(ViewState["hasRows"]);
                FCommon.GridView_PreRenderInit(sender, hasRows);
            }
        }
}