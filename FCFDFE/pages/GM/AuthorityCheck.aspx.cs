using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.GM
{
    public partial class AuthorityCheck : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities GME = new GMEntities();

#region 副程式
        private void dataImport_GV()
        {
            string strC_SN_SYS = drpC_SN_SYS.SelectedValue;
            //string strC_SN_ROLE = rdoC_SN_ROLE.ToString();
            string strDEPT_SN = txtOVC_DEPT_CDE.Text;
            string strUSER_NAME = txtUSER_NAME.Text;
            DateTime dateStart = DateTime.Today;

            var query =
                from accountAuth in GME.ACCOUNT_AUTH
                join account in GME.ACCOUNTs on accountAuth.USER_ID equals account.USER_ID
                join Dept in GME.TBMDEPTs on account.DEPT_SN equals Dept.OVC_DEPT_CDE
                join tableSys in GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("SA")) on accountAuth.C_SN_SYS equals tableSys.OVC_PHR_ID
                join tableRole in GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("S5")) on accountAuth.C_SN_ROLE equals tableRole.OVC_PHR_ID
                join tableAuth in GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("S6")) on accountAuth.C_SN_AUTH equals tableAuth.OVC_PHR_ID into tempAuth
                from tableAuth in tempAuth.DefaultIfEmpty()
                join tableSub in GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("K5")) on accountAuth.C_SN_SUB equals tableSub.OVC_PHR_ID into tempSub
                from tableSub in tempSub.DefaultIfEmpty()
                select new
                {
                    AA_SN = accountAuth.AA_SN,
                    DEPT_SN = Dept.OVC_ONNAME,
                    DEPT_SN_Value = account.DEPT_SN,
                    USER_ID = account.USER_ID,
                    USER_NAME = account.USER_NAME,
                    IUSER_PHONE = account.IUSER_PHONE,
                    C_SN_SYS = tableSys.OVC_PHR_DESC,
                    C_SN_SYS_Value = accountAuth.C_SN_SYS,
                    C_SN_ROLE = tableRole.OVC_PHR_DESC,
                    C_SN_AUTH_Value = accountAuth.C_SN_AUTH,
                    C_SN_AUTH = tableAuth != null ? tableAuth.OVC_PHR_DESC : "",
                    //C_SN_SUB = accountAuth.C_SN_SUB,
                    C_SN_SUB = tableSub != null ? tableSub.OVC_PHR_DESC : "",
                    IS_UPLOAD = accountAuth.IS_UPLOAD,
                    IS_ENABLE = accountAuth.IS_ENABLE,
                    accountAuth.IS_PRO
                };
            if (!strC_SN_SYS.Equals(string.Empty))
                query = query.Where(table => table.C_SN_SYS_Value.Equals(strC_SN_SYS));
            if (!strDEPT_SN.Equals(string.Empty))
                query = query.Where(table => table.DEPT_SN_Value.Equals(strDEPT_SN));
            if (!strUSER_NAME.Equals(string.Empty))
                query = query.Where(table => table.USER_NAME.Contains(strUSER_NAME));
            
            //匯出未處理之資料
            var query_Un = query.Where(table => table.IS_PRO == 0);
            DataTable dt_Un = CommonStatic.LinqQueryToDataTable(query_Un);
            ViewState["hasRows_Un"] = FCommon.GridView_dataImport(GV_ACCOUNT_AUTH_Un, dt_Un);
            
            //匯出已處理之資料
            query = query.Where(table => table.IS_PRO == 1);
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_ACCOUNT_AUTH, dt);
        }
#endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    //系統別
                    DataTable dtC_SN_SYS = CommonStatic.ListToDataTable(GME.TBM1407.Where(table => table.OVC_PHR_CATE == "SA").ToList());
                    FCommon.list_dataImport(drpC_SN_SYS, dtC_SN_SYS, "OVC_PHR_DESC", "OVC_PHR_ID", true);
                    //rdoC_SN_ROLE.Visible = drpC_SN_SYS.SelectedValue.Equals("MPMS");//是否要顯示使用者

                    FCommon.Controls_Attributes("readonly", "ture", txtOVC_DEPT_CDE, txtOVC_ONNAME);    //新增唯讀屬性

                    FCommon.GridView_setEmpty(GV_ACCOUNT_AUTH_Un);
                    FCommon.GridView_setEmpty(GV_ACCOUNT_AUTH);
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport_GV();
        }
        
        protected void GV_ACCOUNT_AUTH_Un_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            Guid id = new Guid(GV_ACCOUNT_AUTH_Un.DataKeys[gvr.RowIndex].Value.ToString());

            switch (e.CommandName)
            {
                case "DataSave":
                    RadioButtonList rdoIS_UPLOAD = (RadioButtonList)gvr.FindControl("rdoIS_UPLOAD");    //上傳
                    RadioButtonList rdoIS_ENABLE = (RadioButtonList)gvr.FindControl("rdoIS_ENABLE");    //開放
                    if (FCommon.Controls_isExist(rdoIS_UPLOAD, rdoIS_ENABLE))
                    {
                        string strIS_UPLOAD = rdoIS_UPLOAD.SelectedValue;
                        string strIS_ENABLE = rdoIS_ENABLE.SelectedValue;
                        
                        ACCOUNT_AUTH accountAuth = GME.ACCOUNT_AUTH.Where(table => table.AA_SN.CompareTo(id) == 0).SingleOrDefault();
                        if (accountAuth != null)
                        {
                            accountAuth.IS_UPLOAD = strIS_UPLOAD;
                            accountAuth.IS_ENABLE = strIS_ENABLE;
                            accountAuth.IS_PRO = 1; //已處理
                            GME.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), accountAuth.GetType().Name.ToString(), this, "修改");
                            dataImport_GV();
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功！");
                        }
                        else
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "帳號權限 錯誤！");
                    }
                    break;
                case "DataDel":

                    break;
                default:
                    break;
            }
        }
        protected void GV_ACCOUNT_AUTH_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            Guid id = Guid.Parse(GV_ACCOUNT_AUTH.DataKeys[gvr.RowIndex].Value.ToString());

            switch (e.CommandName)
            {
                case "DataSave":
                    RadioButtonList rdoIS_UPLOAD = (RadioButtonList)gvr.FindControl("rdoIS_UPLOAD");    //上傳
                    RadioButtonList rdoIS_ENABLE = (RadioButtonList)gvr.FindControl("rdoIS_ENABLE");    //開放
                    if(FCommon.Controls_isExist(rdoIS_UPLOAD, rdoIS_ENABLE))
                    {
                        string strIS_UPLOAD = rdoIS_UPLOAD.SelectedValue;
                        string strIS_ENABLE = rdoIS_ENABLE.SelectedValue;
                        
                        ACCOUNT_AUTH accountAuth = GME.ACCOUNT_AUTH.Where(table => table.AA_SN.CompareTo(id) == 0).SingleOrDefault();
                        if (accountAuth != null)
                        {
                            accountAuth.IS_UPLOAD = strIS_UPLOAD;
                            accountAuth.IS_ENABLE = strIS_ENABLE;
                            GME.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), accountAuth.GetType().Name.ToString(), this, "修改");
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功！");
                        }
                        else
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "帳號權限 錯誤！");
                        dataImport_GV();
                    }
                    break;
                default:
                    break;
            }
        }
        protected void GV_ACCOUNT_AUTH_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //string strC_SN_SUB = e.Row.Cells[7].Text;
                //string strIS_UPLOAD = e.Row.Cells[8].Text;
                //string strIS_ENABLE = e.Row.Cells[9].Text;
                //e.Row.Cells[8].Visible = false;
                //e.Row.Cells[9].Visible = false;

                GridView theGridView = (GridView)sender;
                GridViewRow gvr = e.Row;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //if (!strC_SN_SUB.Equals(string.Empty))
                    //{
                    //    var query =
                    //        from tableSub in GME.TBM1407
                    //        where tableSub.OVC_PHR_CATE.Equals("K5")
                    //        where tableSub.OVC_PHR_ID.Equals(strC_SN_SUB)
                    //        select new { tableSub.OVC_PHR_DESC };
                    //    var queryList = query.ToList();
                    //    if (queryList.Count > 0) e.Row.Cells[7].Text = queryList.First().OVC_PHR_DESC.ToString();
                    //}

                    Label lblIS_UPLOAD = (Label)gvr.FindControl("lblIS_UPLOAD"); //上傳
                    RadioButtonList rdoIS_UPLOAD = (RadioButtonList)gvr.FindControl("rdoIS_UPLOAD"); //上傳
                    Label lblIS_ENABLE = (Label)gvr.FindControl("lblIS_ENABLE"); //開放
                    RadioButtonList rdoIS_ENABLE = (RadioButtonList)gvr.FindControl("rdoIS_ENABLE"); //開放
                    if(FCommon.Controls_isExist(lblIS_UPLOAD, rdoIS_UPLOAD, lblIS_ENABLE, rdoIS_ENABLE))
                    {
                        FCommon.list_dataImportYN(rdoIS_UPLOAD, true, false);
                        FCommon.list_dataImportAudit(rdoIS_ENABLE, true, false);
                        FCommon.list_setValue(rdoIS_UPLOAD, lblIS_UPLOAD.Text);
                        FCommon.list_setValue(rdoIS_ENABLE, lblIS_ENABLE.Text);
                    }
                }
            }
            catch { }
        }
        protected void GV_ACCOUNT_AUTH_Un_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows_Un"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows_Un"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        protected void GV_ACCOUNT_AUTH_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}