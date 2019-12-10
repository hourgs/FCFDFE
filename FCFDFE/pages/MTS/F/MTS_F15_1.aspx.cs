using System;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;
using System.Data;
using System.Data.Entity;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F15_1 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        bool hasRows = false;
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();

        public void dataimport()
        {
            var query = 
                from c in MTSE.TBGMT_CURRENCY
                orderby c.ONB_SORT
                select new
                {
                    TODAY = "",
                    OVC_CURRENCY_CODE = c.OVC_CURRENCY_CODE,
                    OVC_CURRENCY_NAME = c.OVC_CURRENCY_NAME,
                    ONB_RATE = ""
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
            string strCode = gvr.Cells[1].Text;
            string strMessage = "";
            decimal decONB_RATE;
            DateTime date_1 = DateTime.Now.AddDays(-1);
            DateTime date_2 = DateTime.Now.AddDays(1);

            Label lblONB_RATE = (Label)gvr.FindControl("lblONB_RATE");
            TextBox txtONB_RATE = (TextBox)gvr.FindControl("txtONB_RATE");
            Button btnedit = (Button)gvr.FindControl("btnedit");
            Button btnupdate = (Button)gvr.FindControl("btnupdate");
            Button btnDel = (Button)gvr.FindControl("btnDel");
            Button btncancel = (Button)gvr.FindControl("btncancel");

            bool boolONB_RATE = FCommon.checkDecimal(txtONB_RATE.Text, "與新台幣兌換比例", ref strMessage, out decONB_RATE);
            if (txtONB_RATE.Text.Equals(string.Empty))
            {
                strMessage += "<p>請輸入 與新台幣兌換比例！</p>";
            }

            switch (e.CommandName)
            {
                case "DataEdit":
                    lblONB_RATE.Visible = false;
                    txtONB_RATE.Visible = true;
                    btnedit.Visible = false;
                    btnupdate.Visible = true;
                    btnDel.Visible = false;
                    btncancel.Visible = true;
                    break;
                case "DataSave":
                    if (strMessage.Equals(string.Empty))
                    {
                        TBGMT_CURRENCY_RATE rate = MTSE.TBGMT_CURRENCY_RATE
                            .Where(o => o.OVC_CURRENCY_CODE.Equals(strCode))
                            .Where(o => o.ODT_DATE > date_1 && o.ODT_DATE < date_2).FirstOrDefault();
                        if (rate != null)
                        {
                            rate.ONB_RATE = decONB_RATE;
                            MTSE.SaveChanges();
                        }
                        else
                        {
                            TBGMT_CURRENCY_RATE rate_new = new TBGMT_CURRENCY_RATE();
                            rate_new.OVC_CURRENCY_CODE = strCode;
                            rate_new.ONB_RATE = decONB_RATE;
                            rate_new.ODT_DATE = DateTime.Now;
                            rate_new.OVC_RATE_SN = Guid.NewGuid();
                            MTSE.TBGMT_CURRENCY_RATE.Add(rate_new);
                            MTSE.SaveChanges();
                        }
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "儲存幣別幣值資料成功！");
                        dataimport();
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                    break;
                case "DataDelete":
                    TBGMT_CURRENCY_RATE rate_Del = MTSE.TBGMT_CURRENCY_RATE
                            .Where(o => o.OVC_CURRENCY_CODE.Equals(strCode))
                            .Where(o => o.ODT_DATE > date_1 && o.ODT_DATE < date_2).FirstOrDefault();
                    if (rate_Del != null)
                    {
                        try
                        {
                            MTSE.Entry(rate_Del).State = EntityState.Deleted;
                            MTSE.SaveChanges();
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除幣別幣值資料成功！");
                            dataimport();
                        }
                        catch (Exception ex)
                        {
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "刪除幣別幣值資料失敗！");
                        }
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "刪除幣別幣值資料失敗！");
                    break;
                case "DataCancel":
                    dataimport();
                    break;
            }

        }
        protected void GV_TBGMT_CURRENCY_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblONB_RATE = (Label)e.Row.FindControl("lblONB_RATE");
                Button btnDel = (Button)e.Row.FindControl("btnDel");
                string Year = (int.Parse(DateTime.Now.Year.ToString()) - 1911).ToString();
                e.Row.Cells[0].Text = Year + DateTime.Now.ToString("年MM月dd");
                string strOVC_CURRENCY_CODE = e.Row.Cells[1].Text;
                DateTime date_1 = DateTime.Now.AddDays(-1);
                DateTime date_2 = DateTime.Now.AddDays(1);
                var queryRate =
                    (from rate in MTSE.TBGMT_CURRENCY_RATE
                     where rate.ODT_DATE > date_1 && rate.ODT_DATE < date_2
                     where rate.OVC_CURRENCY_CODE.Equals(strOVC_CURRENCY_CODE)
                     select rate).FirstOrDefault();
                if (queryRate != null)
                    lblONB_RATE.Text = queryRate.ONB_RATE.ToString();
                else
                    btnDel.Enabled = false;
            }
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