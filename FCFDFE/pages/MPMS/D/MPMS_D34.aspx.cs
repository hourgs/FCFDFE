using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D34 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
                string strOVC_PURCH_5_url = Request.QueryString["OVC_PURCH_5"];
                string strOVC_PURCH_6_url = Request.QueryString["OVC_PURCH_6"];
                string strONB_GROUP_url = Request.QueryString["ONB_GROUP"];
                string strOVC_DOPEN_url = Request.QueryString["OVC_DOPEN"];
                string strONB_TIMES_url = Request.QueryString["ONB_TIMES"];
                string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
                string strOVC_PURCH_5 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_5_url));
                string strOVC_PURCH_6 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_6_url));
                string strOVC_DOPEN = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_DOPEN_url));
                string strONB_GROUP = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strONB_GROUP_url));
                string strONB_TIMES = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strONB_TIMES_url));
                short onbgroup = Convert.ToInt16(strONB_GROUP);

                ViewState["strOVC_PURCH"] = strOVC_PURCH;
                ViewState["strOVC_PURCH_5"] = strOVC_PURCH_5;
                ViewState["strOVC_PURCH_6"] = strOVC_PURCH_6;
                ViewState["strONB_GROUP"] = strONB_GROUP;
                ViewState["strOVC_DOPEN"] = strOVC_DOPEN;
                ViewState["strONB_TIMES"] = strONB_TIMES;

                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_PURCH_5"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    if (!IsPostBack && IsOVC_DO_NAME())
                        dataImport();
                }
            }
        }

        private bool IsOVC_DO_NAME()
        {
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strUserName, strDept = "";
            string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
            string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
            DataTable dt = new DataTable();
            if (strUSER_ID.Length > 0)
            {
                ACCOUNT ac = new ACCOUNT();
                ac = gm.ACCOUNTs.Where(tb => tb.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                if (ac != null)
                {
                    strUserName = ac.USER_NAME.ToString();
                    strDept = ac.DEPT_SN;
                    TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    if (strOVC_PURCH == "")
                        strErrorMsg = "請選擇購案";
                    else if (tbm1301 == null)
                        strErrorMsg = "查無此購案編號";
                    else
                    {
                        TBM1301_PLAN plan1301 =
                            gm.TBM1301_PLAN.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCHASE_UNIT.Equals(strDept)).FirstOrDefault();

                        TBMRECEIVE_BID tbmRECEIVE_BID =
                            mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_DO_NAME.Equals(strUserName)).FirstOrDefault();
                        if (tbmRECEIVE_BID == null || plan1301 == null)
                            strErrorMsg = "非此購案的承辦人";
                    }

                    if (strErrorMsg != "")
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErrorMsg);
                    else
                    {
                        divForm.Visible = true;
                        return true;
                    }
                }
            }
            divForm.Visible = false;
            return false;
        }

        private void dataImport()
        {
            string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
            string strOVC_PURCH_5_url = Request.QueryString["OVC_PURCH_5"];
            string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
            string strOVC_PURCH_5 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_5_url));
            ViewState["strOVC_PURCH"] = strOVC_PURCH;
            ViewState["strOVC_PURCH_5"] = strOVC_PURCH_5;
            DataTable dt = new DataTable();

            var querynone =
                from tbmnone in mpms.TBMRESULT_ANNOUNCE_NONE
                 where tbmnone.OVC_PURCH == strOVC_PURCH
                 where tbmnone.OVC_PURCH_5 == strOVC_PURCH_5
                 join tbm1301 in mpms.TBM1301 on tbmnone.OVC_PURCH equals tbm1301.OVC_PURCH
                 select new
                 {
                     OVC_PURCH = tbmnone.OVC_PURCH,
                     OVC_PURCH_5 = tbmnone.OVC_PURCH_5,
                     OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                     ONB_TIMES = tbmnone.ONB_TIMES,
                     OVC_DOPEN = tbmnone.OVC_DOPEN,
                     OVC_DSEND = tbmnone.OVC_DSEND,
                     OVC_NAME = tbmnone.OVC_NAME
                 };
            dt = CommonStatic.LinqQueryToDataTable(querynone);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_info, dt);
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            lblOVC_PURCH.Text = query1301.OVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5; ;
        }
        protected void GV_info_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_info.DataKeys[gvrIndex].Value.ToString(); //OVC_PURCH
            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH"].ToString()));
            string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH_5"].ToString()));
            string key3 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strONB_GROUP"].ToString()));
            string key4 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH_6"].ToString()));
            string key5 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_DOPEN"].ToString()));
            string key6 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strONB_TIMES"].ToString()));
            switch (e.CommandName)
            {
                case "btnWork":
                    Response.Redirect("MPMS_D35.aspx?OVC_PURCH=" + key + "&OVC_PURCH_5=" + key2 + "&ONB_GROUP=" + key3 + "&OVC_PURCH_6=" + key4
                + "&OVC_DOPEN=" + key5 + "&ONB_TIMES=" + key6 + "&TYPE=Modify");
                    break;
                case "btnDel":
                    string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
                    string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
                    
                    var queryannouncenone=
                     (from tbmannounce in mpms.TBMRESULT_ANNOUNCE_NONE
                      where tbmannounce.OVC_PURCH == strOVC_PURCH
                      where tbmannounce.OVC_PURCH_5 == strOVC_PURCH_5
                      select tbmannounce).FirstOrDefault();
                    mpms.Entry(queryannouncenone).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), queryannouncenone.GetType().Name.ToString(), this, "刪除");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
                    dataImport();
                    break;
                default:
                    break;
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH"].ToString()));
            string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH_5"].ToString()));
            string key3 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strONB_GROUP"].ToString()));
            string key4 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH_6"].ToString()));
            string key5 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_DOPEN"].ToString()));
            string key6 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strONB_TIMES"].ToString()));

            Response.Redirect("MPMS_D35.aspx?OVC_PURCH=" + key + "&OVC_PURCH_5=" + key2 + "&ONB_GROUP=" + key3 + "&OVC_PURCH_6=" + key4
                + "&OVC_DOPEN=" + key5 + "&ONB_TIMES=" + key6 + "&TYPE=New");
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            //Response.Redirect("MPMS_D1B.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString());
            string send_url = "MPMS_D18_7.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_DOPEN=" + ViewState["strOVC_DOPEN"].ToString() +
                                    "&ONB_TIMES=" + ViewState["strONB_TIMES"] + "&ONB_GROUP=" + ViewState["strONB_GROUP"];
            Response.Redirect(send_url);
        }

        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_D14.aspx");
        }

        protected void GV_info_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}