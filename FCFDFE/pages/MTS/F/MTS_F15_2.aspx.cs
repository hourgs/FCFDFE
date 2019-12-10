using System;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;
using System.Data;
using System.Data.Entity;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F15_2 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        bool hasRows = false;
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();

        public void dataimport()
        {
            var query = from c in MTSE.TBGMT_CURRENCY
                        orderby c.ONB_SORT
                        select new
                        {
                            OVC_CURRENCY_CODE = c.OVC_CURRENCY_CODE,
                            OVC_CURRENCY_NAME = c.OVC_CURRENCY_NAME,
                            ONB_SORT = c.ONB_SORT,
                            OVC_TYPE = c.OVC_TYPE,
                            OVC_STATUS = c.OVC_STATUS,
                            ODT_CREATE_DATE = c.ODT_CREATE_DATE,
                            OVC_CREATE_ID = c.OVC_CREATE_ID,
                            ODT_MODIFY_DATE = c.ODT_MODIFY_DATE,
                            OVC_MODIFY_LOGIN_ID = c.OVC_MODIFY_LOGIN_ID
                        };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["dt"] = dt;
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_CURRENCY, dt);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Response.Redirect("MTS_F15_3.aspx");
        }
        protected void GV_TBGMT_CURRENCY_PreRender(object sender, EventArgs e)
        {
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
            if (hasRows)
            {
                GV_TBGMT_CURRENCY.UseAccessibleHeader = true;
                GV_TBGMT_CURRENCY.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        protected void GV_TBGMT_CURRENCY_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            DataTable dt = (DataTable)ViewState["dt"];
            int gvrIndex = gvr.RowIndex;
            string id = theGridView.DataKeys[gvrIndex].Value.ToString();


            switch (e.CommandName)
            {
                case "DataEdit":
                    theGridView.EditIndex = gvrIndex;
                    dataimport();
                    break;
                case "DataSave":
                    //TextBox txtOVC_CURRENCY_CODE = (TextBox)gvr.FindControl("txtOVC_CURRENCY_CODE");
                    TextBox txtOVC_CURRENCY_NAME = (TextBox)gvr.FindControl("txtOVC_CURRENCY_NAME");
                    TextBox txtONB_SORT = (TextBox)gvr.FindControl("txtONB_SORT");
                    DropDownList drpOVC_TYPE = (DropDownList)gvr.FindControl("drpOVC_TYPE");
                    DropDownList drpOVC_STATUS = (DropDownList)gvr.FindControl("drpOVC_STATUS");

                    //Label lblOCC = (Label)gvr.FindControl("lblOVC_CURRENCY_CODE");
                    Label lblOCN = (Label)gvr.FindControl("lblOVC_CURRENCY_NAME");
                    Label lblOS = (Label)gvr.FindControl("lblONB_SORT");
                    Label lblOVC_TYPE = (Label)gvr.FindControl("lblOVC_TYPE");
                    Label lblOVC_STATUS = (Label)gvr.FindControl("lblOVC_STATUS");

                    //string strOVC_CURRENCY_CODE = txtOVC_CURRENCY_CODE.Text;
                    string strOVC_CURRENCY_NAME = txtOVC_CURRENCY_NAME.Text;
                    string strONB_SORT = txtONB_SORT.Text;
                    string strOVC_TYPE = drpOVC_TYPE.Text;
                    string strOVC_STATUS = drpOVC_STATUS.Text;

                    Decimal decONB_SORT;
                    string strMessage = "";
                    bool boolSort = FCommon.checkDecimal(strONB_SORT, "排序", ref strMessage, out decONB_SORT);

                    int if_exist_sort = MTSE.TBGMT_CURRENCY.Where(table => table.ONB_SORT == decONB_SORT).Count();
                    int chaname = MTSE.TBGMT_CURRENCY.Where(table => table.OVC_CURRENCY_NAME.Equals(strOVC_CURRENCY_NAME)).Count();
                    //int chancode = MTSE.TBGMT_CURRENCY.Where(table => table.OVC_CURRENCY_CODE.Equals(strOVC_CURRENCY_CODE)).Count();

                    //if (strOVC_CURRENCY_CODE.Equals(string.Empty))
                    //{
                    //    strMessage += "<p>請輸入 幣別名稱！</p>";
                    //}
                    //else if (chancode>1)
                    //{
                    //    strMessage += "<p>幣別代號已重複！</p>";
                    //}
                    if (strOVC_CURRENCY_NAME.Equals(string.Empty))
                    {
                        strMessage += "<p>請輸入 幣別名稱！</p>";
                    }
                    if (lblOCN.Equals(strOVC_CURRENCY_NAME))
                    {
                        if (chaname > 1)
                            strMessage += "<p>幣別名稱已重複！</p>";
                    }
                    if (!lblOCN.Text.Equals(strOVC_CURRENCY_NAME))
                    {
                        if (chaname > 0)
                            strMessage += "<p>幣別名稱已重複！</p>";
                    }
                    if (strONB_SORT.Equals(string.Empty))
                    {
                        strMessage += "<p>請輸入 排序！</p>";
                    }
                    if (decONB_SORT <= 0)
                    {
                        strMessage += "<p>排序需為正數</p>";
                    }
                    if (lblOS.Equals(strONB_SORT))
                    {
                        if (if_exist_sort > 1)
                            strMessage += "<p>排序 已重複！</p>";
                    }
                    if (lblOS.Text.Equals(strONB_SORT))
                    {if(if_exist_sort>1)
                        strMessage += "<p>排序 已重複！</p>";
                    }
                    if (strMessage.Equals(string.Empty))
                    {
                        TBGMT_CURRENCY cu = MTSE.TBGMT_CURRENCY.Where(table => table.OVC_CURRENCY_CODE.Equals(id)).FirstOrDefault();
                        if (cu != null)
                        {

                            var query = from a in MTSE.TBGMT_CURRENCY
                                        where a.OVC_CURRENCY_CODE.Equals(id)
                                        select new
                                        {
                                            OVC_CURRENCY_CODE = a.OVC_CURRENCY_CODE
                                        };
                            DataTable dt2 = CommonStatic.LinqQueryToDataTable(query);

                            if (dt2.Rows.Count > 0)
                            {
                                cu.ODT_MODIFY_DATE = DateTime.Now;
                                cu.OVC_MODIFY_LOGIN_ID = Session["userid"].ToString();
                                //cu.OVC_CURRENCY_CODE = strOVC_CURRENCY_CODE;
                                cu.OVC_CURRENCY_NAME = strOVC_CURRENCY_NAME;
                                cu.ONB_SORT = decONB_SORT;
                                cu.OVC_TYPE = strOVC_TYPE;
                                cu.OVC_STATUS = strOVC_STATUS;
                                MTSE.SaveChanges();

                                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), cu.GetType().Name.ToString(), this, "修改");
                                FCommon.AlertShow(PnMessage, "success", "系統訊息", "幣別排序已全部被修改了！！");
                                GV_TBGMT_CURRENCY.EditIndex = -1;
                                dataimport();
                            }
                            else
                            {
                                strMessage += "無此幣別名稱！請重新輸入！";
                                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                            }
                        }
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                    break;
                case "DataDelete":
                    Label lblname = (Label)gvr.FindControl("lblOVC_CURRENCY_NAME");
                    string name = lblname.Text;
                    TBGMT_CURRENCY cud = MTSE.TBGMT_CURRENCY.Where(table => table.OVC_CURRENCY_CODE.Equals(id)).FirstOrDefault();
                    if (cud != null)
                    {
                        try
                        {
                            MTSE.Entry(cud).State = EntityState.Deleted;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), cud.GetType().Name.ToString(), this, "刪除");
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除幣值幣別資料:" + name + "成功！");
                            GV_TBGMT_CURRENCY.EditIndex = -1;
                            dataimport();
                        }
                        catch (Exception ex)
                        {
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "刪除幣值幣別資料:" + name + "失敗！");
                        }
                    }
                    else
                    {
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "刪除幣值幣別資料:" + name + "失敗！");
                    }
                    break;
                case "DataCancel":
                    GV_TBGMT_CURRENCY.EditIndex = -1;
                    dataimport();
                    break;
            }

        }
        protected void GV_TBGMT_CURRENCY_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = e.Row;
            int gvrIndex = gvr.RowIndex;
            //foreach (GridViewRow grv in theGridView.Rows)
            //{

            //    if (gvr.RowType == DataControlRowType.DataRow)
            //    {
            //        if (gvr.Cells[1].Text == "台幣")
            //        {
            //            var temp1 = gvr.Cells[9].FindControl("btnedit") as LinkButton;
            //            if (temp1 != null) temp1.Attributes.Add("disabled", "false");
            //            var temp2 = gvr.Cells[10].FindControl("btnDel") as Button;
            //            if (temp2 != null) temp2.Attributes.Add("disabled", "false");
            //        }
            //    }
            //}
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("MTS_F15.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dataimport();
            }
        }
    }
}