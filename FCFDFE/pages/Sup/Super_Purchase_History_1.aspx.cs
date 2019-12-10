using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.CIMSModel;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Content;
using System.Data;

namespace FCFDFE.pages.Sup
{
    public partial class Super_Purchase_History_1 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        private GMEntities gme = new GMEntities();
        private CIMSEntities cimse = new CIMSEntities();
        MPMSEntities mpms = new MPMSEntities();
        Common FCommon = new Common();
        string[] strField = { "OVC_PURCH_6", "OVC_PURCH_5", "OVC_VEN_CST", "OVC_VEN_TITLE",
            "OVC_DBID", "OVC_CURRENT", "ONB_MONEY", "ONB_MONEY_NTD" };
        string[] strField2 = { "ONB_POI_ICOUNT","OVC_POI_NSTUFF_CHN", "NSN", "OVC_POI_IREF", "ONB_POI_QORDER_PLAN",
            "ONB_POI_QORDER_CONT", "ONB_POI_MPRICE_PLAN", "ONB_POI_MPRICE_CONT"};
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
                basicdata(id);
                data1302(id);
                data1201(id);

            }
        }

        private void data1302(string ovcpurch)
        {
            DataTable dt = new DataTable();

            var query =
                from tb1302 in mpms.TBM1302
                join tb1407 in mpms.TBM1407 on tb1302.OVC_BUD_CURRENT equals tb1407.OVC_PHR_ID
                where tb1407.OVC_PHR_CATE == "B0"
                where tb1302.OVC_PURCH == ovcpurch
                select new
                {
                    OVC_PURCH_6 = tb1302.OVC_PURCH_6,
                    OVC_PURCH_5 = tb1302.OVC_PURCH_5,
                    OVC_VEN_CST = tb1302.OVC_VEN_CST,
                    OVC_VEN_TITLE = tb1302.OVC_VEN_TITLE,
                    OVC_DBID = tb1302.OVC_DBID,
                    OVC_CURRENT = tb1407.OVC_PHR_DESC,
                    ONB_MONEY = tb1302.ONB_MONEY,
                    ONB_MONEY_NTD = tb1302.ONB_RATE * tb1302.ONB_MONEY
                };

            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_1302, dt, strField);
        }

        private void data1201(string ovcpurch)
        {
            DataTable dt = new DataTable();

            var query =
                from tb1201 in mpms.TBM1201
                where tb1201.OVC_PURCH == ovcpurch
                select new
                {
                    ONB_POI_ICOUNT = tb1201.ONB_POI_ICOUNT,
                    OVC_POI_NSTUFF_CHN = tb1201.OVC_POI_NSTUFF_CHN,
                    NSN = tb1201.NSN,
                    OVC_POI_IREF = tb1201.OVC_POI_IREF,
                    ONB_POI_QORDER_PLAN = tb1201.ONB_POI_QORDER_PLAN,
                    ONB_POI_QORDER_CONT = tb1201.ONB_POI_QORDER_CONT,
                    ONB_POI_MPRICE_PLAN = tb1201.ONB_POI_MPRICE_PLAN,
                    ONB_POI_MPRICE_CONT = tb1201.ONB_POI_MPRICE_CONT
                };

            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows2"] = FCommon.GridView_dataImport(GVTBGMT_1201, dt, strField2);
        }

        private void basicdata(string ovcpurch)
        {
            var query = mpms.TBM1301.Where(id => id.OVC_PURCH.Equals(ovcpurch)).FirstOrDefault();
            string strOVC_PUR_SECTION = query.OVC_PUR_SECTION;
            var queryunit = mpms.TBMDEPTs.Where(id => id.OVC_DEPT_CDE == strOVC_PUR_SECTION).FirstOrDefault();

            lblOVC_PURCH.Text = query.OVC_PURCH;
            lblOVC_AGNT_IN.Text = query.OVC_AGNT_IN;
            lblOVC_PUR_IPURCH.Text = query.OVC_PUR_IPURCH;
            if (queryunit != null)
                lblOVC_PUR_SECTION.Text = query.OVC_PUR_SECTION + ":" + queryunit.OVC_ONNAME;
            else
                lblOVC_PUR_SECTION.Text = query.OVC_PUR_SECTION;
            string kind = query.OVC_PURCH_KIND;
            string pkind = "";
            if (kind == "1")
                pkind = "內購案";
            if (kind == "2")
                pkind = "外購案";
            lblOVC_PURCH_KIND.Text = query.OVC_PURCH_KIND + ":" + pkind;
            lblOVC_PUR_USER.Text = query.OVC_PUR_USER;
            lblOVC_PUR_IUSER_PHONE_EXT.Text = query.OVC_PUR_IUSER_PHONE_EXT;
            lblOVC_PUR_IUSER_PHONE.Text = query.OVC_PUR_IUSER_PHONE;
            string vencode = query.OVC_PUR_ASS_VEN_CODE;
            var queryC7 = mpms.TBM1407.Where(table => table.OVC_PHR_CATE == "C7").Where(table => table.OVC_PHR_ID == vencode).FirstOrDefault();
            if (queryC7 != null)
                lblOVC_PUR_ASS_VEN_CODE.Text = query.OVC_PUR_ASS_VEN_CODE + ":" + queryC7.OVC_PHR_DESC;
            else
                lblOVC_PUR_ASS_VEN_CODE.Text = query.OVC_PUR_ASS_VEN_CODE;
            string current = query.OVC_PUR_CURRENT;
            var queryB0 = mpms.TBM1407.Where(table => table.OVC_PHR_CATE == "B0").Where(table => table.OVC_PHR_ID == current).FirstOrDefault();
            if (queryB0 != null)
                lblOVC_CURRENT.Text = query.OVC_PUR_CURRENT + ":" + queryB0.OVC_PHR_DESC;
            else
                lblOVC_CURRENT.Text = query.OVC_PUR_CURRENT;
            lblONB_RATE.Text = query.ONB_PUR_RATE.ToString();
            string bid = query.OVC_BID;
            var queryM3 = mpms.TBM1407.Where(table => table.OVC_PHR_CATE == "M3").Where(table => table.OVC_PHR_ID == bid).FirstOrDefault();
            if (queryM3 != null)
                lblOVC_BID.Text = query.OVC_BID + ":" + queryM3.OVC_PHR_DESC;
            else
                lblOVC_BID.Text = query.OVC_BID;
            lblONB_PUR_BUDGET.Text = String.Format("{0:N}", query.ONB_PUR_BUDGET);
            lblONB_PUR_BUDGT_NTD.Text = String.Format("{0:N}", query.ONB_PUR_BUDGET_NT);
            var queryfile = cimse.TBM_FILE.Where(table => table.OVC_PURCH == ovcpurch).FirstOrDefault();
            if (queryfile != null)
                lblFile.Text = queryfile.OVC_FILE_NAME;
            else
                lblFile.Text = "無檔案";
        }
        protected void GVTBGMT_1201_PreRender(object sender, EventArgs e)
        {
            bool hasRows2 = false;
            if (ViewState["hasRows2"] != null)
                hasRows2 = Convert.ToBoolean(ViewState["hasRows2"]);
            FCommon.GridView_PreRenderInit(sender, hasRows2);
        }

        protected void GVTBGMT_1302_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string enkey = Request.QueryString["OVC_PURCH"];
            string id = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(enkey));
            string pkey = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(id));
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string pid = GVTBGMT_1302.DataKeys[gvrIndex].Value.ToString();
            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(pid));
            switch (e.CommandName)
            {
                case "btnPurch6":
                    string str_url_Modify;
                    str_url_Modify = "Super_Purchase_History_3.aspx?OVC_PURCH=" + pkey + "&OVC_PURCH_6=" + key;
                    Response.Redirect(str_url_Modify);
                    break;
                default:
                    break;
            }
        }

        protected void GVTBGMT_1201_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string enkey = Request.QueryString["OVC_PURCH"];
            string id = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(enkey));
            string pkey = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(id));
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string pid = GVTBGMT_1201.DataKeys[gvrIndex].Value.ToString();
            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(pid));
            switch (e.CommandName)
            {
                case "btnPoiIcount":
                    string str_url_Modify;
                    str_url_Modify = "Super_Purchase_History_4.aspx?OVC_PURCH=" + pkey + "&ONB_POI_ICOUNT=" + key;
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

        protected void GVTBGMT_1302_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}