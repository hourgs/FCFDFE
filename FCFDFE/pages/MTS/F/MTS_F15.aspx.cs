using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F15 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        public int intRowIndex = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (!IsPostBack)
                {
                    DateTime dateNow = DateTime.Now;
                    txtOdtCreateDate1.Text = FCommon.getDateTime(dateNow);
                    txtOdtCreateDate2.Text = FCommon.getDateTime(dateNow);
                    FCommon.Controls_Attributes("readonly", "true", txtOdtCreateDate1, txtOdtCreateDate2);
                }
            }
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataimport();
        }
        private void dataimport()
        {
            string strMessage = "";
            string sD = txtOdtCreateDate1.Text;
            string eD  = txtOdtCreateDate2.Text;
            DateTime datestart, dateend;
            bool booldate1 = FCommon.checkDateTime(sD, "開始日期", ref strMessage, out datestart);
            bool booldate2 = FCommon.checkDateTime(eD, "結束日期", ref strMessage, out dateend);
            datestart = datestart.AddDays(-1);
            dateend = dateend.AddDays(1);
            if (sD.Equals(string.Empty))
            {
                strMessage += "<p> 請輸入 開始日期</p>";
            }
            if (eD.Equals(string.Empty))
            {
                strMessage += "<p> 請輸入 結束日期</p>";
            }
            if (strMessage.Equals(string.Empty))
            {
                var query = from c in MTSE.TBGMT_CURRENCY
                            join cr in MTSE.TBGMT_CURRENCY_RATE on c.OVC_CURRENCY_CODE equals cr.OVC_CURRENCY_CODE
                            where cr.ODT_DATE > datestart && cr.ODT_DATE < dateend
                            where c.OVC_STATUS.Equals("Y")
                            select new
                            {
                                ODT_DATE = cr.ODT_DATE,
                                OVC_CURRENCY_CODE = cr.OVC_CURRENCY_CODE,
                                OVC_CURRENCY_NAME = c.OVC_CURRENCY_NAME,
                                ONB_RATE = cr.ONB_RATE,
                                ONB_SORT = c.ONB_SORT
                            };
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["dt"] = dt;
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_CURRENCY, dt);
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Response.Redirect("MTS_F15_2.aspx");
        }
        protected void GV_TBGMT_CURRENCY_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            DataTable dt = (DataTable)ViewState["dt"];
            int gvrIndex = gvr.RowIndex;
            string id = theGridView.DataKeys[gvrIndex].Value.ToString();

            DateTime date = Convert.ToDateTime(dt.Rows[gvrIndex]["ODT_DATE"]);
            switch (e.CommandName)
            {
                case "DataEdit":
                    intRowIndex = gvrIndex;
                    theGridView.EditIndex = gvrIndex;
                    dataimport();
                    GV_TBGMT_CURRENCY.PageIndex = gvrIndex;
                    break;
                case "DataSave":
                    intRowIndex = gvrIndex;
                    TextBox txtOVC_CURRENCY_NAME = (TextBox)gvr.FindControl("txtOVC_CURRENCY_NAME");
                    TextBox txtONB_RATE = (TextBox)gvr.FindControl("txtONB_RATE");
                    TextBox txtONB_SORT = (TextBox)gvr.FindControl("txtONB_SORT");
                    Label lblOC = (Label)gvr.FindControl("lblOVC_CLASS");
                    Label lblOCN = (Label)gvr.FindControl("lblOVC_CLASS_NAME");
                    Label lblOS = (Label)gvr.FindControl("lblONB_SORT");
                    string strOVC_CURRENCY_NAME = txtOVC_CURRENCY_NAME.Text;
                    string strONB_RATE = txtONB_RATE.Text;
                    string strONB_SORT = txtONB_SORT.Text;
                    Decimal decONB_SORT, decONB_RATE;
                    string strMessage = "";
                    bool boolSort = FCommon.checkDecimal(strONB_SORT, "排序", ref strMessage, out decONB_SORT);
                    bool boolONB_RATE = FCommon.checkDecimal(strONB_RATE, "兌換比率", ref strMessage, out decONB_RATE);
                    int if_exist_sort = MTSE.TBGMT_CURRENCY.Where(table => table.ONB_SORT == decONB_SORT).Count();
                    int chaname = MTSE.TBGMT_CURRENCY.Where(table => table.OVC_CURRENCY_NAME.Equals(strOVC_CURRENCY_NAME)).Count();
                    if (strOVC_CURRENCY_NAME.Equals(string.Empty))
                    {
                        strMessage += "<p>請輸入 幣別名稱！</p>";
                    }
                    if (strONB_RATE.Equals(string.Empty))
                    {
                        strMessage += "<p>請輸入 與新台幣兌換比例！</p>";
                    }
                    else if (decONB_RATE<=0)
                    {
                        strMessage += "<p>與新台幣兌換比例需為正數！</p>";
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
                        TBGMT_CURRENCY_RATE tcr = MTSE.TBGMT_CURRENCY_RATE.Where(table => table.OVC_CURRENCY_CODE.Equals(id)).Where(table => table.ODT_DATE == date).FirstOrDefault();
                        TBGMT_CURRENCY cu = MTSE.TBGMT_CURRENCY.Where(table => table.OVC_CURRENCY_CODE.Equals(id)).FirstOrDefault();

                        var query = from a in MTSE.TBGMT_CURRENCY
                                    where a.OVC_CURRENCY_CODE.Equals(id)
                                    select new
                                    {
                                        OVC_CURRENCY_CODE = a.OVC_CURRENCY_CODE
                                    };
                        DataTable dt2 = CommonStatic.LinqQueryToDataTable(query);
                        
                        if (dt2.Rows.Count > 0)
                        {

                            tcr.ONB_RATE = decONB_RATE;
                            cu.ODT_MODIFY_DATE = DateTime.Now;
                            cu.OVC_MODIFY_LOGIN_ID = Session["userid"].ToString();
                            cu.OVC_CURRENCY_NAME = strOVC_CURRENCY_NAME;
                            cu.ONB_SORT = decONB_SORT;
                            MTSE.SaveChanges();

                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tcr.GetType().Name.ToString(), this, "修改");
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
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                    break;
                case "DataDelete":
                    string name = gvr.Cells[2].Text;
                    TBGMT_CURRENCY_RATE tcrd = MTSE.TBGMT_CURRENCY_RATE.Where(table => table.OVC_CURRENCY_CODE.Equals(id)).Where(table => table.ODT_DATE == date).FirstOrDefault();
                    TBGMT_CURRENCY cud = MTSE.TBGMT_CURRENCY.Where(table => table.OVC_CURRENCY_CODE.Equals(id)).FirstOrDefault();

                    try
                    {
                        MTSE.Entry(tcrd).State = EntityState.Deleted;
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
                    break;
                case "DataCancel":
                    intRowIndex = gvrIndex;
                    GV_TBGMT_CURRENCY.EditIndex = -1;
                    dataimport();
                    break;
            }
        }

        protected void brnAdd_Click(object sender, EventArgs e)
        {
              Response.Redirect("MTS_F15_1.aspx");
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
    }
}