using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F16 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        public int intRowIndex = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            if (!IsPostBack)
            {
                bool boolImport = false;
                string txtOvcClass = "";
                if (FCommon.getQueryString(this, "OVC_CLASS", out txtOvcClass, true))
                {
                    FCommon.list_setValue(drpOvcClass, txtOvcClass);
                    boolImport = true;
                }
                if (boolImport)
                    dataimport();
                var query = from c in MTSE.TBGMT_DEPT_CLASS
                            select new
                            {
                                c.OVC_CLASS_NAME
                            };
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                for (int i = 0 ; i < dt.Rows.Count ; i++)
                {
                    drpOvcClass.Items.Add(dt.Rows[i][0].ToString());
                }

            }
        }
        #region 副程式
        private void dataimport()
        {
            string dept_class = drpOvcClass.SelectedValue;
            DataTable dt;
            if (dept_class == "全部")
            {
                var query = from c in MTSE.TBGMT_DEPT_CLASS
                            select new
                            {
                                OVC_CLASS = c.OVC_CLASS,
                                OVC_CLASS_NAME = c.OVC_CLASS_NAME,
                                ONB_SORT = c.ONB_SORT,
                                c.OVC_DEPTCLA_SN,
                            };
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                dt = CommonStatic.LinqQueryToDataTable(query);
            }
            else
            {
                var query = from c in MTSE.TBGMT_DEPT_CLASS
                            where c.OVC_CLASS_NAME == dept_class
                            select new
                            {
                                OVC_CLASS = c.OVC_CLASS,
                                OVC_CLASS_NAME = c.OVC_CLASS_NAME,
                                ONB_SORT = c.ONB_SORT,
                                c.OVC_DEPTCLA_SN
                            };
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                dt = CommonStatic.LinqQueryToDataTable(query);
            }
            ViewState["OVC_CLASS"] = dept_class;
            ViewState["dt"] = dt;
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_DEPT_CLASS, dt);
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_CLASS", ViewState["OVC_CLASS"], true);
            return strQueryString;
        }
        #endregion
        #region 按鈕部分
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataimport();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_CLASS", ViewState["OVC_CLASS"], true);
            Response.Redirect($"MTS_F16_2{ strQueryString }");
        }
        #endregion
        #region GV部分
        protected void GV_TBGMT_DEPT_CLASS_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    intRowIndex = gvrIndex;
                    theGridView.EditIndex = gvrIndex;
                    dataimport();
                        GV_TBGMT_DEPT_CLASS.PageIndex = gvrIndex;
                    break;
                case "DataSave":
                    intRowIndex = gvrIndex;
                    #region 儲存
                    TextBox txtOVC_CLASS = (TextBox)gvr.FindControl("txtOVC_CLASS");
                    TextBox txtOVC_CLASS_NAME = (TextBox)gvr.FindControl("txtOVC_CLASS_NAME");
                    TextBox txtONB_SORT = (TextBox)gvr.FindControl("txtONB_SORT");
                    Label lblOC = (Label)gvr.FindControl("lblOVC_CLASS");
                    Label lblOCN = (Label)gvr.FindControl("OVC_CLASS_NAME");
                    Label lblOS = (Label)gvr.FindControl("lblONB_SORT");
                    string strOVC_CLASS = txtOVC_CLASS.Text;
                    string strOVC_CLASS_NAME = txtOVC_CLASS_NAME.Text;
                    string strONB_SORT = txtONB_SORT.Text;
                    Decimal decONB_SORT;
                    string strMessage = "";
                    bool boolSort = FCommon.checkDecimal(txtONB_SORT.Text, "排序", ref strMessage, out decONB_SORT);
                    int if_exist_class = MTSE.TBGMT_DEPT_CLASS.Where(table => table.OVC_CLASS.Equals(strOVC_CLASS)).Count();
                    int if_exist_class_name = MTSE.TBGMT_DEPT_CLASS.Where(table => table.OVC_CLASS_NAME.Equals(strOVC_CLASS_NAME)).Count();
                    int if_exist_sort = MTSE.TBGMT_DEPT_CLASS.Where(table => table.ONB_SORT == decONB_SORT).Count();
                    if (strOVC_CLASS.Equals(string.Empty))
                    {
                        strMessage += "<p>請輸入 類別代碼！</p>";
                    }
                    else if (if_exist_class>1)
                    {
                        strMessage += "<p>類別代碼 已重複！</p>";
                    }
                    if (strOVC_CLASS_NAME.Equals(string.Empty))
                    {
                        strMessage += "<p>請輸入 類別名稱！</p>";
                    }
                    else if (if_exist_class_name > 1)
                    {
                        strMessage += "<p>類別名稱 已重複！</p>";
                    }
                    if (strONB_SORT.Equals(string.Empty))
                    {
                        strMessage += "<p>請輸入 排序！</p>";
                    }
                    else if (if_exist_sort > 1)
                    {
                        strMessage += "<p>排序 已重複！</p>";
                    }
                    if (strMessage.Equals(string.Empty))
                    {
                        TBGMT_DEPT_CLASS dc = MTSE.TBGMT_DEPT_CLASS.Where(table => table.OVC_DEPTCLA_SN.Equals(guid)).FirstOrDefault();
                        if (dc != null)
                        {
                            dc.OVC_CLASS = strOVC_CLASS;
                            dc.OVC_CLASS_NAME = strOVC_CLASS_NAME;
                            dc.ONB_SORT = decONB_SORT;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), dc.GetType().Name.ToString(), this, "修改");
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功！！");
                            GV_TBGMT_DEPT_CLASS.EditIndex = -1;
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
                        TBGMT_DEPT_CLASS dc = MTSE.TBGMT_DEPT_CLASS.Where(table => table.OVC_DEPTCLA_SN.Equals(guid)).FirstOrDefault();
                        if (dc != null)
                        {
                            MTSE.Entry(dc).State = EntityState.Deleted;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), dc.GetType().Name.ToString(), this, "刪除");
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功！！");
                            GV_TBGMT_DEPT_CLASS.EditIndex = -1;
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
                    intRowIndex = gvrIndex;
                    GV_TBGMT_DEPT_CLASS.EditIndex = -1;
                    dataimport();
                    break;
            }
        }
        protected void GV_TBGMT_DEPT_CLASS_PreRender(object sender, EventArgs e)
        {
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
            if (hasRows)
            {
                GV_TBGMT_DEPT_CLASS.UseAccessibleHeader = true;
                GV_TBGMT_DEPT_CLASS.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        #endregion
    }
}