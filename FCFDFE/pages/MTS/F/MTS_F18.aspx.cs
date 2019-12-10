using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F18 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtOdtStartDate, txtOdtEndDate);

                    string strOdtStartDate, strOdtEndDate, strOdtApplyDate;
                    bool boolImport = false;
                    if (FCommon.getQueryString(this, "ODT_START_DATE", out strOdtStartDate, true))
                    {
                        txtOdtStartDate.Text = strOdtStartDate;
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "ODT_END_DATE", out strOdtEndDate, true))
                    {
                        txtOdtEndDate.Text = strOdtEndDate;
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OdtApplyDate", out strOdtApplyDate, true))
                    {
                        if (strOdtApplyDate.Equals("不限定日期"))
                            chkOdtApplyDate.Items[0].Selected = true;
                        boolImport = true;
                    }
                    if (boolImport == true) dataimport();
                }
            }
        }
        #region 副程式
        private void dataimport()
        {
            string strOdtStartDate = txtOdtStartDate.Text;
            string strOdtEndDate = txtOdtEndDate.Text;
            string strOdtApplyDate = chkOdtApplyDate.SelectedValue;
            ViewState["ODT_START_DATE"] = strOdtStartDate;
            ViewState["ODT_END_DATE"] = strOdtEndDate;
            ViewState["OdtApplyDate"] = strOdtApplyDate;

            var query =
                from clean in MTSE.TBGMT_CLEARANCE
                select clean;

            if (!chkOdtApplyDate.SelectedValue.Equals("不限定日期"))
            {
                if (strOdtStartDate != string.Empty)
                {
                    DateTime start = Convert.ToDateTime(strOdtStartDate);
                    query = query.Where(t => t.ODT_CREATE_DATE >= start);
                }
                if (strOdtEndDate != string.Empty)
                {
                    DateTime end = Convert.ToDateTime(strOdtEndDate);
                    query = query.Where(t => t.ODT_CREATE_DATE <= end);
                }
                if (strOdtStartDate != string.Empty && strOdtEndDate.Equals(string.Empty))
                {
                    DateTime end = DateTime.Now;
                    query = query.Where(t => t.ODT_CREATE_DATE <= end);
                }
            }
            if (chkOdtApplyDate.SelectedValue.Equals("不限定日期"))
            {
                //選取不限定時清空原本資料
                txtOdtStartDate.Text = "";
                txtOdtEndDate.Text = "";
            }
            if (query.Count() > 1000)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
            query = query.Take(1000);
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_CLEAN, dt);
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "ODT_START_DATE", ViewState["ODT_START_DATE"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_END_DATE", ViewState["ODT_END_DATE"], true);
            FCommon.setQueryString(ref strQueryString, "OdtApplyDate", ViewState["OdtApplyDate"], true);
            return strQueryString;
        }
        #endregion
        #region 按鈕部分
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataimport();
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F18_1{ getQueryString() }");
        }
        #endregion
        #region GV部分
        protected void GV_TBGMT_CLEAN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            DataTable dt = (DataTable)ViewState["dt"];
            int gvrIndex = gvr.RowIndex;
            string id = theGridView.DataKeys[gvrIndex].Value.ToString();
            Guid guid = Guid.Parse(id);

            switch (e.CommandName)
            {
                case "DataEdit":
                    theGridView.EditIndex = gvrIndex;
                    dataimport();
                    break;
                case "DataSave":
                    #region 儲存
                    TextBox txtOVC_CLASS_NAME = (TextBox)gvr.FindControl("txtOVC_CLASS_NAME");
                    string strMessage = "";
                    string strOVC_CLASS_NAME = txtOVC_CLASS_NAME.Text;
                    if (strOVC_CLASS_NAME.Equals(string.Empty))
                    {
                        strMessage += "<p>請輸入 清運方式！</p>";
                    }
                    if (strMessage.Equals(string.Empty))
                    {
                        TBGMT_CLEARANCE clean = MTSE.TBGMT_CLEARANCE.Where(t => t.CL_SN.Equals(guid)).FirstOrDefault();
                        if (clean != null)
                        {
                            clean.OVC_WAY = strOVC_CLASS_NAME;
                            clean.ODT_MODIFY_DATE = DateTime.Now;
                            clean.OVC_MODIFY_LOGIN_ID = Session["username"].ToString();
                            MTSE.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), clean.GetType().Name.ToString(), this, "修改");
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功！！");
                            GV_TBGMT_CLEAN.EditIndex = -1;
                            dataimport();
                        }
                        else
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                    #endregion
                    break;
                case "DataDelete":
                    #region 刪除
                    if (!id.Equals(string.Empty))
                    {
                        TBGMT_CLEARANCE clean = MTSE.TBGMT_CLEARANCE.Where(t => t.CL_SN.Equals(guid)).FirstOrDefault();
                        if (clean != null)
                        {
                            MTSE.Entry(clean).State = EntityState.Deleted;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), clean.GetType().Name.ToString(), this, "刪除");
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功！！");
                            GV_TBGMT_CLEAN.EditIndex = -1;
                            dataimport();
                        }
                        else
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "刪除失敗");
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查無此資料");
                    #endregion
                    break;
                case "DataCancel":
                    GV_TBGMT_CLEAN.EditIndex = -1;
                    dataimport();
                    break;
            }
        }

        protected void GV_TBGMT_CLEAN_PreRender(object sender, EventArgs e)
        {
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
            if (hasRows)
            {
                GV_TBGMT_CLEAN.UseAccessibleHeader = true;
                GV_TBGMT_CLEAN.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        #endregion
    }
}