using System;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System.Web.UI;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E1A : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (Session["rowtext"] != null)
                {
                    if (!IsPostBack)
                    {
                        GV_dataImport();
                        Session.Contents.Remove("isModify");
                        DataTable dt = new DataTable();
                        for (int i = 0; i < 1; i++)
                        {
                            DataRow dr = dt.NewRow();
                            dt.Rows.Add(dr);
                        }
                        GV_TBMPAY_MONEY_2.DataSource = dt.AsDataView();
                        GV_TBMPAY_MONEY_2.DataBind();
                    }
                }
                else
                    FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
            }
        }

        #region Click
        //修改btn
        protected void btnTakeOver_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            Session["ONB_TIMES"] = GV_TakeOver.Cells[4].Text;
            Session["isModify"] = "1";
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E1B.aspx";
            Response.Redirect(send_url);
        }
        //刪除btn
        protected void btnDel_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            string purch = Session["rowtext"].ToString().Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            var query =
                    from tbmpay_money in mpms.TBMPAY_MONEY
                    where tbmpay_money.OVC_PURCH.Equals(purch)
                    where tbmpay_money.OVC_PURCH_6.Equals(purch_6)
                    orderby tbmpay_money.ONB_TIMES
                    select new
                    {
                        ONB_TIMES = tbmpay_money.ONB_TIMES
                    };
            foreach (var q in query)
            {
                TBMPAY_MONEY tbmpay_money = new TBMPAY_MONEY();
                tbmpay_money = mpms.TBMPAY_MONEY
                    .Where(table => table.OVC_PURCH.Equals(purch) && table.OVC_PURCH_6.Equals(purch_6) && table.ONB_TIMES.Equals(q.ONB_TIMES)).FirstOrDefault();
                if (q.ONB_TIMES.ToString() == GV_TakeOver.Cells[4].Text)
                {
                    mpms.Entry(tbmpay_money).State = EntityState.Deleted;
                    mpms.SaveChanges();
                }
            }
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
            GV_dataImport();
        }
        //新增btn
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            TextBox txtSettlementTimes = (TextBox)GV_TakeOver.FindControl("txtSettlementTimes");
            TextBox txtOVC_POI_IBDG = (TextBox)GV_TakeOver.FindControl("txtOVC_POI_IBDG");
            DropDownList drpOVC_POI_IBDG = (DropDownList)GV_TakeOver.FindControl("drpOVC_POI_IBDG");
            if (int.TryParse(txtSettlementTimes.Text, out int n))
            {
                if (txtSettlementTimes.Text == "" || drpOVC_POI_IBDG.Text == "請選擇")
                {
                    string strmeg = "";
                    if (txtSettlementTimes.Text == "")
                        strmeg += "<p>請填選 結算次數</p>";
                    if (drpOVC_POI_IBDG.Text == "請選擇")
                        strmeg += "<p>請填選 工作分支計畫</p>";
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strmeg);
                }
                else
                {
                    var purch = GV_TakeOver.Cells[0].Text.Substring(0, 7);
                    int c = 0;
                    var query =
                        from pay in mpms.TBMPAY_MONEY
                        where pay.OVC_PURCH.Equals(purch)
                        select pay.ONB_TIMES;
                    foreach (var q in query)
                        if (q == short.Parse(txtSettlementTimes.Text)) c++;
                    if (c > 0)
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "結算次數重複");
                    else
                    {
                        Session["ONB_TIMES"] = txtSettlementTimes.Text;
                        Session["OVC_PJNAME"] = txtOVC_POI_IBDG.Text;
                        Session.Contents.Remove("isModify");
                        string send_url;
                        send_url = "~/pages/MPMS/E/MPMS_E1B.aspx";
                        Response.Redirect(send_url);
                    }
                }
            }
            else
            {
                string strmeg = " < p > 請填選 結算次數 </ p > ";
                if (drpOVC_POI_IBDG.Text == "請選擇")
                    strmeg += "<p>請填選 工作分支計畫</p>";
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strmeg);
            }
        }
        //回上一頁
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }
        #endregion

        #region 副程式

        #region GridView資料帶入
        private void GV_dataImport()
        {
            if (Session["rowtext"] != null)
            {
                DataTable dt = new DataTable();
                var purch = Session["rowtext"].ToString().Substring(0, 7);
                string purch_6 = Session["purch_6"].ToString();
                var query =
                    from tbmpay_money in mpms.TBMPAY_MONEY
                    where tbmpay_money.OVC_PURCH.Equals(purch)
                    where tbmpay_money.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        OVC_PURCH = tbmpay_money.OVC_PURCH,
                        OVC_POI_IBDG = tbmpay_money.OVC_POI_IBDG,
                        OVC_PJNAME = tbmpay_money.OVC_PJNAME,
                        ONB_PAY_MONEY = tbmpay_money.ONB_PAY_MONEY,
                        OVC_DPAY = tbmpay_money.OVC_DPAY,
                        OVC_DO_NAME = tbmpay_money.OVC_DO_NAME,
                        ONB_TIMES = tbmpay_money.ONB_TIMES
                    };
                if (query.Count() > 0)
                    hasRows = true;
                dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBMPAY_MONEY_1, dt);
            }
        }
        #endregion

        #region 工作分支計畫(預算科目)
        private void list_dataImport(ListControl list)
        {
            if (Session["rowtext"] != null)
            {
                var purch = Session["rowtext"].ToString().Substring(0, 7);
                list.Items.Clear();
                list.Items.Add("請選擇");
                var query =
                    from tbm1118 in mpms.TBM1118
                    where tbm1118.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_POI_IBDG = tbm1118.OVC_POI_IBDG,
                        OVC_PJNAME = tbm1118.OVC_PJNAME
                    };
                foreach (var q in query)
                    list.Items.Add(q.OVC_POI_IBDG + q.OVC_PJNAME);
            }
        }
        #endregion

        #region GridView
        protected void GV_TBMPAY_MONEY_1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["rowtext"] != null)
                {
                    e.Row.Cells[6].Text = dateTW(e.Row.Cells[6].Text);
                    e.Row.Cells[1].Text = Session["rowtext"].ToString();
                    var purch = Session["rowtext"].ToString().Substring(0, 7);
                    string purch_6 = Session["purch_6"].ToString();
                    var query =
                        from tbmpay_money in mpms.TBMPAY_MONEY
                        where tbmpay_money.OVC_PURCH.Equals(purch)
                        where tbmpay_money.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            ONB_PAY_MONEY = tbmpay_money.ONB_PAY_MONEY
                        };
                    foreach (var q in query)
                        e.Row.Cells[5].Text = String.Format("{0:N}", q.ONB_PAY_MONEY);
                }
            }
        }
        protected void GV_TBMPAY_MONEY_2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList drpOVC_POI_IBDG = (DropDownList)e.Row.FindControl("drpOVC_POI_IBDG");
                TextBox txtSettlementTimes = (TextBox)e.Row.FindControl("txtSettlementTimes");
                TextBox txtOVC_POI_IBDG = (TextBox)e.Row.FindControl("txtOVC_POI_IBDG");
                if (Session["rowtext"] != null)
                {
                    string purch_6 = Session["purch_6"].ToString();
                    var purch = Session["rowtext"].ToString().Substring(0, 7);
                    var querytime =
                        from pay in mpms.TBMPAY_MONEY
                        where pay.OVC_PURCH.Equals(purch)
                        where pay.OVC_PURCH_6.Equals(purch_6)
                        orderby pay.ONB_TIMES
                        select pay.ONB_TIMES;
                    foreach (var q in querytime)
                        if (q >= short.Parse(txtSettlementTimes.Text)) txtSettlementTimes.Text = (q + 1).ToString();

                    var query =
                        from tbm1302 in mpms.TBM1302
                        join tbm1301 in mpms.TBM1301_PLAN on tbm1302.OVC_PURCH equals tbm1301.OVC_PURCH
                        where tbm1301.OVC_PURCH.Equals(purch)
                        where tbm1302.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            OVC_PURCH = tbm1301.OVC_PURCH,
                            OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                            OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                            OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                            OVC_VEN_CST = tbm1302.OVC_VEN_CST
                        };
                    foreach (var q in query)
                    {
                        e.Row.Cells[0].Text = q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5;
                        e.Row.Cells[1].Text = q.OVC_PURCH_6;
                        e.Row.Cells[3].Text = q.OVC_VEN_CST;
                    }
                    Session["purch6"] = e.Row.Cells[1].Text;
                    Session["cst"] = e.Row.Cells[3].Text;
                }
                list_dataImport(drpOVC_POI_IBDG);
            }
        }
        protected void GV_TBMPAY_MONEY_1_PreRender(object sender, EventArgs e)
        {
            //if (hasRows)
            //{
            //    GV_TBMPAY_MONEY_1.UseAccessibleHeader = true;
            //    GV_TBMPAY_MONEY_1.HeaderRow.TableSection = TableRowSection.TableHeader;
            //}
        }

        protected void GV_TBMPAY_MONEY_2_PreRender(object sender, EventArgs e)
        {
            //if (hasRows)
            //{
            //
            //    GV_TBMPAY_MONEY_2.UseAccessibleHeader = true;
            //    GV_TBMPAY_MONEY_2.HeaderRow.TableSection = TableRowSection.TableHeader;
            //}
        }
        protected void drpOVC_POI_IBDG_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drpTakeOver = (DropDownList)sender;
            GridViewRow GV_TakeOver = (GridViewRow)drpTakeOver.NamingContainer;
            TextBox txtOVC_POI_IBDG = (TextBox)GV_TakeOver.FindControl("txtOVC_POI_IBDG");
            if (Session["rowtext"] != null)
            {
                var purch = Session["rowtext"].ToString().Substring(0, 7);
                var query =
                    from tbm1118 in mpms.TBM1118
                    where tbm1118.OVC_PURCH.Equals(purch)
                    where drpTakeOver.Text.Contains(tbm1118.OVC_POI_IBDG)
                    where drpTakeOver.Text.Contains(tbm1118.OVC_PJNAME)
                    select new
                    {
                        OVC_POI_IBDG = tbm1118.OVC_POI_IBDG,
                        OVC_PJNAME = tbm1118.OVC_PJNAME
                    };
                foreach (var q in query)
                        txtOVC_POI_IBDG.Text = q.OVC_POI_IBDG;
                if (drpTakeOver.Text == "請選擇")
                    txtOVC_POI_IBDG.Text = "";
            }
        }
        #endregion

        #region 民國年
        private string dateTW(string str)
        {
            DateTime dt;
            string strdt = "";
            if (DateTime.TryParse(str, out DateTime d))
            {
                dt = DateTime.Parse(str);
                strdt = (int.Parse(dt.Year.ToString()) - 1911).ToString() + "年" + dt.Month.ToString() + "月" + dt.Day.ToString() + "日";
                return strdt;
            }
            else
                return str;
        }
        #endregion

        #endregion
    }
}