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

namespace FCFDFE.pages.CIMS.E
{
    public partial class CIMS_E11_2 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        private GMEntities gme = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        Common FCommon = new Common();
        string[] strField = { "OVC_PURCH", "OVC_PUR_IPURCH1301", "OVC_PURCH_6", "OVC_PURCH_5",
            "ONB_GROUP", "OVC_DBID", "ONB_MONEY"};
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["OVC_VEN_CST"] == null)
            {
                Response.Write("<script>alert('系統檢測到您未依照正確方式進入此頁面，將導至登入畫面!'); location.href='../../../logout.aspx'; </script>");
                return;
            }
            FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            if (!IsPostBack)
            {

                string enkey = Request.QueryString["OVC_VEN_CST"];
                string id = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(enkey));
                data(enkey);
                data1302(enkey);
            }
                
        }
        private void data(string id)
        {
            var query = mpms.TBM1203.Where(cid => cid.CSIST_VEN_CST == id).FirstOrDefault();
            lblOVC_CST.Text = id;
            if(query != null)
            {
                lblOVC_VEN_TITLE.Text = query.OVC_VEN_TITLE;
                lblOVC_NVEN.Text = query.OVC_NVEN;
                lblOVC_VEN_ADDRESS.Text = query.OVC_VEN_ADDRESS;
                lblOVC_VEN_ADDRESS_1.Text = query.OVC_VEN_ADDRESS_1;
                if (query.OVC_PUR_CREATE != null)
                    lblOVC_PUR_CREATE.Text = Convert.ToDateTime(query.OVC_PUR_CREATE).ToString(Variable.strDateFormat);
                else
                    lblOVC_PUR_CREATE.Text = "";
                lblOVC_VEN_ITEL.Text = query.OVC_VEN_ITEL;
                lblOVC_FAX_NO.Text = query.OVC_FAX_NO;
                lblOVC_BOSS.Text = query.OVC_BOSS;
                lblGINGE_VEN_CST.Text = query.GINGE_VEN_CST;
                lblMIXED_CAGE.Text = query.MIXED_CAGE;
                if (query.CAGE_DATE != null)
                    lblCAGE_DATE.Text = Convert.ToDateTime(query.CAGE_DATE).ToString(Variable.strDateFormat);
                else
                    lblCAGE_DATE.Text = "";
                if (query.OVC_DMANAGE_BEGIN != null)
                    lblOVC_DMANAGE_BEGIN.Text = Convert.ToDateTime(query.OVC_DMANAGE_BEGIN).ToString(Variable.strDateFormat);
                else
                    lblOVC_DMANAGE_BEGIN.Text = "";
                if (query.OVC_DMANAGE_END != null)
                    lblOVC_DMANAGE_END.Text = Convert.ToDateTime(query.OVC_DMANAGE_END).ToString(Variable.strDateFormat);
                else
                    lblOVC_DMANAGE_END.Text = "";
                if (query.OVC_DRECOVERY != null)
                    lblOVC_DRECOVERY.Text = Convert.ToDateTime(query.OVC_DRECOVERY).ToString(Variable.strDateFormat);
                else
                    lblOVC_DRECOVERY.Text = "";
                lblOVC_MAIN_PRODUCT.Text = query.OVC_MAIN_PRODUCT;
            }
            
        }

        private void data1302(string id)
        {
            DataTable dt = new DataTable();

            var query =
                from tb1302 in mpms.TBM1302
                join tb1301 in mpms.TBM1301 on tb1302.OVC_PURCH equals tb1301.OVC_PURCH
                where tb1302.OVC_VEN_CST == id
                select new
                {
                    OVC_PURCH = tb1302.OVC_PURCH,
                    OVC_PUR_IPURCH1301 = tb1301.OVC_PUR_IPURCH,
                    OVC_PURCH_6 = tb1302.OVC_PURCH_6,
                    OVC_PURCH_5 = tb1302.OVC_PURCH_5,
                    ONB_GROUP = tb1302.ONB_GROUP,
                    OVC_DBID = tb1302.OVC_DBID,
                    ONB_MONEY = tb1302.ONB_MONEY
                };
            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_1302, dt, strField);
        }
        protected void GVTBGMT_1302_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string pid = GVTBGMT_1302.DataKeys[gvrIndex].Value.ToString();
            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(pid));
            switch (e.CommandName)
            {
                case "btnPurch6":
                    LinkButton lb = (LinkButton)e.CommandSource;
                    string id6 = e.CommandArgument.ToString();
                    string key6 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(id6));
                    string str_url_Modify;
                    str_url_Modify = "CIMS_E11_3.aspx?OVC_PURCH=" + key + "&OVC_PURCH_6=" + key6;
                    Response.Redirect(str_url_Modify);
                    break;
                case "btnPurch":
                    string str_url_Modify1;
                    str_url_Modify1 = "CIMS_E11_1.aspx?OVC_PURCH=" + key ;
                    Response.Redirect(str_url_Modify1);
                    break;
                default:
                    break;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(ViewState["retu"].ToString());
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