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
    public partial class MPMS_D36 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.Controls_Attributes("readonly", "true", txtOVC_DEND);
                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_PURCH_5"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    if (!IsPostBack && IsOVC_DO_NAME())
                    {
                        string strOVC_PURCH = Request.QueryString["OVC_PURCH"];
                        string strOVC_PURCH_5 = Request.QueryString["OVC_PURCH_5"];
                        ViewState["strOVC_PURCH"] = strOVC_PURCH;
                        ViewState["strOVC_PURCH_5"] = strOVC_PURCH_5;
                        var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
                        string strOVC_PUR_AGENCY = query1301.OVC_PUR_AGENCY;
                        ViewState["strOVC_PUR_AGENCY"] = strOVC_PUR_AGENCY;
                        data1302(strOVC_PURCH, strOVC_PURCH_5);
                        data1303(strOVC_PURCH, strOVC_PURCH_5);
                        data1301plan(strOVC_PURCH, strOVC_PURCH_5);
                    }
                }
            }
        }

        private bool IsOVC_DO_NAME()
        {
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strUserName, strDept = "";
            string strOVC_PURCH = Request.QueryString["OVC_PURCH"];
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

        #region dataImport

        private void data1302(string strOVC_PURCH , string strOVC_PURCH_5)
        {
            DataTable dt = new DataTable();
            var query =
                from tbm1302 in mpms.TBM1302
                where tbm1302.OVC_PURCH == strOVC_PURCH
                where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
                join tbm1301 in mpms.TBM1301 on tbm1302.OVC_PURCH equals tbm1301.OVC_PURCH
                select new
                {
                    OVC_PURCH = tbm1302.OVC_PURCH,
                    OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                    OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                    OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                    ONB_GROUP = tbm1302.ONB_GROUP,
                    OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE,
                    OVC_VEN_TEL = tbm1302.OVC_VEN_TEL,
                    OVC_DCONTRACT = tbm1302.OVC_DCONTRACT
                };
            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBM1302, dt);
        }


        private void data1303(string strOVC_PURCH, string strOVC_PURCH_5)
        {
            DataTable dt = new DataTable();
            var query =
                from tbm1303 in mpms.TBM1303.AsEnumerable()
                where tbm1303.OVC_PURCH == strOVC_PURCH
                where tbm1303.OVC_PURCH_5 == strOVC_PURCH_5
                join tbm1302 in mpms.TBM1302.AsEnumerable()
                on new {tbm1303.OVC_PURCH_5,tbm1303.ONB_GROUP } equals new{tbm1302.OVC_PURCH_5,tbm1302.ONB_GROUP}
                into tbmm1302
                from tbm1302 in tbmm1302.DefaultIfEmpty()
                where tbm1302.OVC_PURCH == strOVC_PURCH
                join tbm1407 in mpms.TBM1407
                on tbm1303.OVC_RESULT equals tbm1407.OVC_PHR_ID
                where tbm1407.OVC_PHR_CATE == "A8"
                select new
                {
                    OVC_PURCH = tbm1303.OVC_PURCH,
                    OVC_PURCH_5 = tbm1303.OVC_PURCH_5,
                    OVC_DOPEN = tbm1303.OVC_DOPEN,
                    ONB_TIMES = tbm1303.ONB_TIMES,
                    ONB_GROUP = tbm1303.ONB_GROUP,
                    OVC_RESULT = tbm1303.OVC_RESULT,
                    OVC_DBID = tbm1302.OVC_DBID,
                    OVC_DAPPROVE = tbm1303.OVC_DAPPROVE
                };
            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows2"] = FCommon.GridView_dataImport(GV_TBM1303, dt);
        }

        private void data1301plan(string strOVC_PURCH,string strOVC_PURCH_5)
        {
            DataTable dt = new DataTable();

            var query =
                from tbm1302 in mpms.TBM1302
                where tbm1302.OVC_PURCH == strOVC_PURCH
                where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
                join tbm1301 in mpms.TBM1301_PLAN on tbm1302.OVC_PURCH equals tbm1301.OVC_PURCH
                join tbmdept in mpms.TBMDEPTs on tbm1301.OVC_CONTRACT_UNIT equals tbmdept.OVC_DEPT_CDE
                join t1301 in mpms.TBM1301 on tbm1302.OVC_PURCH equals t1301.OVC_PURCH
                select new
                {
                    OVC_DSEND = tbm1302.OVC_DSEND,
                    OVC_DRECEIVE = tbm1302.OVC_DRECEIVE,
                    OVC_CONTRACT_UNIT = tbmdept.OVC_ONNAME,
                    OVC_PURCH = tbm1302.OVC_PURCH,
                    OVC_PUR_AGENCY = t1301.OVC_PUR_AGENCY,
                    OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                    OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                    ONB_GROUP = tbm1302.ONB_GROUP,
                    OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE
                };

            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows3"] = FCommon.GridView_dataImport(GV_agreen, dt);
        }
        #endregion

        protected void GV_TBM1302_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void GV_TBM1302_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBM1302.DataKeys[gvrIndex].Value.ToString();
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH"].ToString()));
            string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH_5"].ToString()));
            string key3 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(id));
            switch (e.CommandName)
            {
                case "btnMove":
                    Response.Redirect("MPMS_D41.aspx?OVC_PURCH=" + key + "&OVC_PURCH_5=" + key2 + "&ONB_GROUP=" + key3);
                    break;
                default:
                    break;
            }
        }

        protected void GV_TBM1303_PreRender(object sender, EventArgs e)
        {
            bool hasRows2 = false;
            if (ViewState["hasRows2"] != null)
                hasRows2 = Convert.ToBoolean(ViewState["hasRows2"]);
            FCommon.GridView_PreRenderInit(sender, hasRows2);
        }

        protected void GV_agreen_PreRender(object sender, EventArgs e)
        {
            bool hasRows3 = false;
            if (ViewState["hasRows3"] != null)
                hasRows3 = Convert.ToBoolean(ViewState["hasRows3"]);
            FCommon.GridView_PreRenderInit(sender, hasRows3);
        }

        protected void btnSAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < GV_agreen.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)GV_agreen.Rows[i].FindControl("CheckBox1");
                cb.Checked = true;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < GV_agreen.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)GV_agreen.Rows[i].FindControl("CheckBox1");
                cb.Checked = false;
            }
        }

        protected void btnMove_Click(object sender, EventArgs e)
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();

            for (int i = 0; i < GV_agreen.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)GV_agreen.Rows[i].FindControl("CheckBox1");
                if (cb.Checked == true)
                {
                    string strOVC_DSEND = DateTime.Now.ToString("yyyy-MM-dd");
                    string strOVC_PURCH_6 = GV_agreen.Rows[i].Cells[4].Text;

                    var query =
                        (from tbm1302 in mpms.TBM1302
                         where tbm1302.OVC_PURCH == strOVC_PURCH
                         where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
                         where tbm1302.OVC_PURCH_6 == strOVC_PURCH_6
                         select tbm1302).FirstOrDefault();
                    query.OVC_DSEND = strOVC_DSEND;
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), query.GetType().Name.ToString(), this, "修改");

                    var queryplan =
                        (from tbmplan in mpms.TBM1301_PLAN
                         where tbmplan.OVC_PURCH == strOVC_PURCH
                         select tbmplan).FirstOrDefault();
                    queryplan.OVC_CONTRACT_UNIT = "00N00";
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), queryplan.GetType().Name.ToString(), this, "修改");
                }
            }
        }

        protected void btnSAVE_Click(object sender, EventArgs e)
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            for (int i = 0; i < GV_agreen.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)GV_agreen.Rows[i].FindControl("CheckBox1");
                if (cb.Checked == true)
                {
                    string strOVC_PURCH_6 = GV_agreen.Rows[i].Cells[4].Text;
                    var querysta =
                        (from tsta in mpms.TBMSTATUS
                         where tsta.OVC_PURCH == strOVC_PURCH
                         where tsta.OVC_PURCH_5 == strOVC_PURCH_5
                         where tsta.OVC_STATUS == "28"
                         select tsta).FirstOrDefault();
                    querysta.OVC_DEND = DateTime.Now.ToString("yyyy-MM-dd");
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), querysta.GetType().Name.ToString(), this, "修改");

                    TBMSTATUS sta = new TBMSTATUS();
                    sta.OVC_PURCH = strOVC_PURCH;
                    sta.ONB_TIMES = 1;
                    sta.OVC_DO_NAME = query1301.OVC_PUR_USER;
                    sta.OVC_STATUS = "3";
                    sta.OVC_DBEGIN = DateTime.Now.ToString("yyyy-MM-dd");
                    sta.OVC_PURCH_5 = strOVC_PURCH_5;
                    sta.OVC_PURCH_6 = strOVC_PURCH_6;
                    sta.OVC_STATUS_SN = Guid.NewGuid();
                    mpms.TBMSTATUS.Add(sta);

                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), sta.GetType().Name.ToString(), this, "新增");
                }
            }
        }

        protected void brnReturn_Click(object sender, EventArgs e)
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strOVC_PUR_AGENCY = ViewState["strOVC_PUR_AGENCY"].ToString();
            string send_url = "MPMS_D14_1.aspx?OVC_PURCH=" + strOVC_PURCH;
            Response.Redirect(send_url);
        }

        protected void GV_TBM1303_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBM1303.DataKeys[gvrIndex].Value.ToString();
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH"].ToString()));
            string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH_5"].ToString()));
            string key3 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(id));
            switch (e.CommandName)
            {
                case "btnNew":
                    Response.Redirect("MPMS_D37.aspx?OVC_PURCH=" + key + "&OVC_PURCH_5=" + key2+"&ONB_GROUP="+key3);
                    break;
                default:
                    break;
            }
        }

       

    }
}