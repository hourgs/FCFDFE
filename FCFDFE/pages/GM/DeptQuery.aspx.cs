using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using System.Data.Entity;

namespace FCFDFE.pages.GM
{
    public partial class DeptQuery : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities GME = new GMEntities();
        MTSEntities MTSE = new MTSEntities();

        #region 副程式
        private void dataImport_GV_TBMDEPT()
        {
            string strOVC_DEPT_CDE = txtOVC_DEPT_CDE.Text;
            string strOVC_ONNAME = txtOVC_ONNAME.Text;
            string strOVC_ENABLE = drpOVC_ENABLE.SelectedValue;
            string strOVC_CLASS = drpOVC_CLASS.SelectedValue;
            string strOVC_PURCHASE_DEPT = drpOVC_PURCHASE_DEPT.SelectedValue;
            string strOVC_CLASS2 = drpOVC_CLASS2.SelectedValue;

            ViewState["OVC_DEPT_CDE"] = strOVC_DEPT_CDE;
            //ViewState["OVC_ONNAME"] = strOVC_ONNAME;
            ViewState["OVC_ENABLE"] = strOVC_ENABLE;
            ViewState["OVC_CLASS"] = strOVC_CLASS;
            ViewState["OVC_PURCHASE_DEPT"] = strOVC_PURCHASE_DEPT;
            ViewState["OVC_CLASS2"] = strOVC_CLASS2;

            var queryDept =from dept in GME.TBMDEPTs select dept;

            if (!string.IsNullOrEmpty(strOVC_DEPT_CDE))
                queryDept = queryDept.Where(table => table.OVC_DEPT_CDE.Equals(strOVC_DEPT_CDE));
            //if (!string.IsNullOrEmpty(strOVC_ONNAME))
            //    queryDept = queryDept.Where(table => table.OVC_ONNAME.Contains(strOVC_ONNAME));
            if (!string.IsNullOrEmpty(strOVC_ENABLE))
                queryDept = queryDept.Where(table => table.OVC_ENABLE.Equals(strOVC_ENABLE));
            if (!string.IsNullOrEmpty(strOVC_CLASS))
                queryDept = queryDept.Where(table => table.OVC_CLASS.Equals(strOVC_CLASS));
            if (!string.IsNullOrEmpty(strOVC_PURCHASE_DEPT))
                queryDept = queryDept.Where(table => table.OVC_PURCHASE_DEPT.Equals(strOVC_PURCHASE_DEPT));
            if (!string.IsNullOrEmpty(strOVC_CLASS2))
                queryDept = queryDept.Where(table => table.OVC_CLASS2.Equals(strOVC_CLASS2));

            var query =
                from dept in queryDept.AsEnumerable()
                join t1407 in GME.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("S9")).AsEnumerable() on dept.OVC_CLASS equals t1407.OVC_PHR_ID into Temp
                from t1407 in Temp.DefaultIfEmpty()
                join top_dept in GME.TBMDEPTs.AsEnumerable() on dept.OVC_TOP_DEPT equals top_dept.OVC_DEPT_CDE into top_deptTemp
                from top_dept in top_deptTemp.DefaultIfEmpty()
                join purchase_dept in GME.TBMDEPTs.AsEnumerable() on dept.OVC_PURCHASE_DEPT equals purchase_dept.OVC_DEPT_CDE into purchase_deptTemp
                from purchase_dept in purchase_deptTemp.DefaultIfEmpty()
                join class_dept2 in MTSE.TBGMT_DEPT_CLASS.AsEnumerable() on dept.OVC_CLASS2 equals class_dept2.OVC_CLASS into class_dept2Temp
                from class_dept2 in class_dept2Temp.DefaultIfEmpty()
                select new
                {
                    dept.OVC_DEPT_CDE,
                    dept.OVC_ONNAME,
                    OVC_PURCHASE_OK_Value = dept.OVC_PURCHASE_OK ?? "N",
                    OVC_PURCHASE_OK = (dept.OVC_PURCHASE_OK ?? "N").Equals("Y") ? "是" : "否",
                    dept.OVC_TOP_DEPT,
                    OVC_TOP_DEPT_NAME = top_dept != null ? top_dept.OVC_ONNAME : "",
                    dept.OVC_PURCHASE_DEPT,
                    OVC_PURCHASE_DEPT_NAME = purchase_dept != null ? purchase_dept.OVC_ONNAME : "",
                    OVC_CLASS_NAME = t1407 != null ? t1407.OVC_PHR_DESC : "" ,
                    OVC_CLASS2_NAME = class_dept2 != null ? class_dept2.OVC_CLASS_NAME : "",
                    OVC_ENABLE = (dept.OVC_ENABLE ?? "").Equals("0") ? "停用" : (dept.OVC_ENABLE ?? "").Equals("1") ? "現用" : (dept.OVC_ENABLE ?? "").Equals("2") ? "戰時" : ""
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["dt"] = dt;
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBMDEPT, dt);
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_DEPT_CDE", ViewState["OVC_DEPT_CDE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_ONNAME", ViewState["OVC_ONNAME"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_ENABLE", ViewState["OVC_ENABLE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_CLASS", ViewState["OVC_CLASS"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_PURCHASE_DEPT", ViewState["OVC_PURCHASE_DEPT"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_CLASS2", ViewState["OVC_CLASS2"], true);
            return strQueryString;
        }

        //添加click屬性
        //private void AddWindowOpen(LinkButton btn, string topDept)
        //{
        //    string strURL = "";
        //    string strWinTitle = "";
        //    string strWinProperty = "";

        //    strURL = "\\DeptSubUnit.aspx?topDept=" + topDept;
        //    strWinTitle = "null";
        //    strWinProperty = "toolbar=0,location=0,status=0,menubar=0,width=850,height=500,left=200,top=80";

        //    btn.Attributes.Add("onClick", "javascript:window.open('" + strURL + "','" + strWinTitle + "','" + strWinProperty + "');return false;");
        //}
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "ture", txtOVC_DEPT_CDE, txtOVC_ONNAME);    //新增唯讀屬性
                    #region 下拉式選單
                    var queryPUR_KIND = GME.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("S9"));
                    DataTable dtPUR_KIND = CommonStatic.LinqQueryToDataTable(queryPUR_KIND);
                    FCommon.list_dataImportV(drpOVC_CLASS, dtPUR_KIND, "OVC_PHR_DESC", "OVC_PHR_ID", "全部", "", "：", true);

                    var queryAllDept = GME.TBMDEPTs.Where(table => table.OVC_PURCHASE_OK.Equals("Y"));
                    DataTable dtAllDept = CommonStatic.LinqQueryToDataTable(queryAllDept);
                    FCommon.list_dataImportV(drpOVC_PURCHASE_DEPT, dtAllDept, "OVC_ONNAME", "OVC_DEPT_CDE", "：", true);

                    var queryCLASS = MTSE.TBGMT_DEPT_CLASS.OrderBy(table => table.ONB_SORT);
                    DataTable dtCLASS = CommonStatic.LinqQueryToDataTable(queryCLASS);
                    FCommon.list_dataImportV(drpOVC_CLASS2, dtCLASS, "OVC_CLASS_NAME", "OVC_CLASS", "全部", "", "：", true);
                    #endregion

                    //系統別
                    //DataTable dtC_SN_SYS = CommonStatic.ListToDataTable(GME.TBM1407.Where(table => table.OVC_PHR_CATE == "SA").ToList());
                    FCommon.list_setValue(drpOVC_ENABLE, "1"); //預設為現用

                    bool boolImport = false;
                    if (FCommon.getQueryString(this, "OVC_DEPT_CDE", out string strOVC_DEPT_CDE, true))
                    {
                        txtOVC_DEPT_CDE.Text = strOVC_DEPT_CDE;
                        TBMDEPT detp = GME.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(strOVC_DEPT_CDE)).FirstOrDefault();
                        if (detp != null) txtOVC_ONNAME.Text = detp.OVC_ONNAME;
                        boolImport = true;
                    }
                    //if (FCommon.getQueryString(this, "OVC_ONNAME", out string strOVC_ONNAME, true))
                    if (FCommon.getQueryString(this, "OVC_ENABLE", out string strOVC_ENABLE, true))
                        FCommon.list_setValue(drpOVC_ENABLE, strOVC_ENABLE);
                    if (FCommon.getQueryString(this, "OVC_CLASS", out string strOVC_CLASS, true))
                        FCommon.list_setValue(drpOVC_CLASS, strOVC_CLASS);
                    if (FCommon.getQueryString(this, "OVC_PURCHASE_DEPT", out string strOVC_PURCHASE_DEPT, true))
                        FCommon.list_setValue(drpOVC_PURCHASE_DEPT, strOVC_PURCHASE_DEPT);
                    if (FCommon.getQueryString(this, "OVC_CLASS2", out string strOVC_CLASS2, true))
                        FCommon.list_setValue(drpOVC_CLASS2, strOVC_CLASS2);
                    if (boolImport) dataImport_GV_TBMDEPT();
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport_GV_TBMDEPT();
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect($"Deptadd{ getQueryString() }");
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Response.Redirect($"DeptImport{ getQueryString() }");
        }

        protected void GV_TBMDEPT_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = theGridView.DataKeys[gvrIndex].Value.ToString();
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);
            switch (e.CommandName)
            {
                case "dataModify":
                    Response.Redirect($"DeptModify{ strQueryString }");
                    break;
                case "dataDel":
                    TBMDEPT dept = GME.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(id)).FirstOrDefault();
                    if (dept != null)
                    {
                        string strOVC_ONNAME = dept.OVC_ONNAME;
                        GME.Entry(dept).State = EntityState.Deleted;
                        GME.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), dept.GetType().Name.ToString(), this, "刪除");
                        dataImport_GV_TBMDEPT();
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", $"單位代碼：{ id } 名稱：{ strOVC_ONNAME } 刪除成功。");
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"單位代碼：{ id } 不存在！");
                    break;
            }
        }

        protected void GV_TBMDEPT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;
            if(gvr.RowType == DataControlRowType.DataRow)
            {
                Label lblOVC_PURCHASE_OK = (Label)gvr.FindControl("lblOVC_PURCHASE_OK");
                LinkButton btnOVC_PURCHASE_OK = (LinkButton)gvr.FindControl("btnOVC_PURCHASE_OK");
                if (FCommon.Controls_isExist(lblOVC_PURCHASE_OK, btnOVC_PURCHASE_OK))
                {
                    string strOVC_PURCHASE_OK = lblOVC_PURCHASE_OK.Text;
                    btnOVC_PURCHASE_OK.Visible = strOVC_PURCHASE_OK.Equals("Y");
                }
            }
        }

        protected void GV_TBMDEPT_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        #region 舊 Querylist
        //protected void Page_Init(object sender, EventArgs e)
        //{
        //    Querylist.PagePropertiesChanging +=
        //        new EventHandler<PagePropertiesChangingEventArgs>(Querylist_PagePropertiesChanging);
        //}

        //protected void Querylist_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        //{
        //    this.ContactsDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
        //    DataTable dt = (DataTable)ViewState["dt"];
        //    Querylist.DataSource = dt;
        //    Querylist.DataBind();

        //}
        //protected void Querylist_ItemCommand(object sender, ListViewCommandEventArgs e)
        //{
        //    switch (e.CommandName)
        //    {
        //        case "dataDel":
        //            string deptCode = e.CommandArgument.ToString();
        //            string deptName;
        //            TBMDEPT tdept_D = GME.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(deptCode)).FirstOrDefault();
        //            deptName = tdept_D.OVC_ONNAME;
        //            GME.Entry(tdept_D).State = EntityState.Deleted;
        //            GME.SaveChanges();
        //            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tdept_D.GetType().Name.ToString(), this, "刪除");
        //            Querylist.Items.Clear();
        //            Query();
        //            FCommon.AlertShow(PnMessage, "success", "系統訊息", deptCode + deptName + "刪除成功");
        //            break;
        //        case "dataModify":
        //            string url = "~/pages/GM/DeptModify.aspx?deptCode=" + e.CommandArgument.ToString();
        //            Response.Redirect(url);
        //            break;

        //    }
        //}
        //protected void Querylist_ItemDataBound(object sender, ListViewItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListViewItemType.DataItem)
        //    {
        //        System.Data.DataRowView DRV = (System.Data.DataRowView)e.Item.DataItem;
        //        Label lblPURCHASE_OK = (Label)e.Item.FindControl("lblPURCHASE_OK");
        //        Label lblOVC_ENABLE = (Label)e.Item.FindControl("lblOVC_ENABLE");
        //        LinkButton btnToSubUnit = (LinkButton)e.Item.FindControl("btnToSubUnit");
        //        AddWindowOpen(btnToSubUnit, btnToSubUnit.CommandArgument);
        //        if (lblPURCHASE_OK.Text.Equals("Y"))
        //        {
        //            LinkButton linkBtnPURCHASE_OK = (LinkButton)e.Item.FindControl("linkBtnPURCHASE_OK");
        //            lblPURCHASE_OK.Text = "是";
        //            linkBtnPURCHASE_OK.Visible = true;
        //            AddWindowOpen(linkBtnPURCHASE_OK, linkBtnPURCHASE_OK.CommandArgument);
        //        }
        //        else
        //        {
        //            lblPURCHASE_OK.Text = "否";
        //        }

        //        switch (lblOVC_ENABLE.Text)
        //        {
        //            case "0":
        //                lblOVC_ENABLE.Text = "停用";
        //                break;
        //            case "1":
        //                lblOVC_ENABLE.Text = "現用";
        //                break;
        //            case "2":
        //                lblOVC_ENABLE.Text = "戰時";
        //                break;
        //        }

        //    }
        //}
        #endregion
    }
}