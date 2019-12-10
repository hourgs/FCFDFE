using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E1C : System.Web.UI.Page
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
                        Session.Contents.Remove("sn");
                    }
                }
                else
                    FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
            }
        }

        #region Click

        #region 修改btn
        protected void btnTakeOver_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            Label labSN = (Label)GV_TakeOver.FindControl("labSN");
            Session["sn"] = labSN.Text;
            Session["isModify"] = "1";
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E1D.aspx";
            Response.Redirect(send_url);
        }
        protected void btnTakeOver_Click1(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            Label labSN = (Label)GV_TakeOver.FindControl("labSN");
            Session["sn"] = labSN.Text;
            Session["isModify"] = "1";
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E1E.aspx";
            Response.Redirect(send_url);
        }
        protected void btnTakeOver_Click2(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            Label labSN = (Label)GV_TakeOver.FindControl("labSN");
            Session["sn"] = labSN.Text;
            Session["allMoney"] = GV_TakeOver.Cells[3].Text;
            Session["isModify"] = "1";
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E20.aspx";
            Response.Redirect(send_url);
        }
        #endregion

        #region 新增btn
        protected void btnCashNew_Click(object sender, EventArgs e)
        {
            Session.Contents.Remove("isModify");
            Session.Contents.Remove("sn");
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E1D.aspx";
            Response.Redirect(send_url);
        }
        protected void btnPromNew_Click(object sender, EventArgs e)
        {
            Session.Contents.Remove("isModify");
            Session.Contents.Remove("sn");
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E1E.aspx";
            Response.Redirect(send_url);
        }
        protected void btnStockNew_Click(object sender, EventArgs e)
        {
            Session.Contents.Remove("isModify");
            Session.Contents.Remove("sn");
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E20.aspx";
            Response.Redirect(send_url);
        }
        #endregion

        #region 刪除btn
        protected void btnDel_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            Label labSN = (Label)GV_TakeOver.FindControl("labSN");
            Session["sn"] = labSN.Text;
            Guid guid = Guid.Parse(labSN.Text);
            string purch = Session["rowtext"].ToString().Substring(0, 7);
            var query =
                    from cash in mpms.TBMMANAGE_CASH
                    where cash.OVC_PURCH.Equals(purch)
                    where cash.OVC_CASH_SN.Equals(guid)
                    orderby cash.OVC_PURCH_6
                    select new
                    {
                        OVC_PURCH_6 = cash.OVC_PURCH_6,
                        OVC_CASH_SN = cash.OVC_CASH_SN
                    };
            foreach (var q in query)
            {
                TBMMANAGE_CASH tbmmanage_cash = new TBMMANAGE_CASH();
                tbmmanage_cash = mpms.TBMMANAGE_CASH
                    .Where(table => table.OVC_PURCH.Equals(purch) && table.OVC_PURCH_6.Equals(q.OVC_PURCH_6) && table.OVC_CASH_SN.Equals(q.OVC_CASH_SN)).FirstOrDefault();
                if (q.OVC_PURCH_6.ToString() == GV_TakeOver.Cells[2].Text)
                {
                    mpms.Entry(tbmmanage_cash).State = EntityState.Deleted;
                    mpms.SaveChanges();
                }
            }
            GV_dataImport();
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
        }
        protected void btnDel_Click1(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            Label labSN = (Label)GV_TakeOver.FindControl("labSN");
            Session["sn"] = labSN.Text;
            Guid guid = Guid.Parse(labSN.Text);
            string purch = Session["rowtext"].ToString().Substring(0, 7);
            var query =
                    from prom in mpms.TBMMANAGE_PROM
                    where prom.OVC_PURCH.Equals(purch)
                    where prom.OVC_PROM_SN.Equals(guid)
                    orderby prom.OVC_PURCH_6
                    select new
                    {
                        OVC_PURCH_6 = prom.OVC_PURCH_6,
                        OVC_PROM_SN = prom.OVC_PROM_SN
                    };
            foreach (var q in query)
            {
                TBMMANAGE_PROM tbmmanage_prom = new TBMMANAGE_PROM();
                tbmmanage_prom = mpms.TBMMANAGE_PROM
                    .Where(table => table.OVC_PURCH.Equals(purch) && table.OVC_PURCH_6.Equals(q.OVC_PURCH_6) && table.OVC_PROM_SN.Equals(q.OVC_PROM_SN)).FirstOrDefault();
                if (q.OVC_PURCH_6.ToString() == GV_TakeOver.Cells[2].Text)
                {
                    mpms.Entry(tbmmanage_prom).State = EntityState.Deleted;
                    mpms.SaveChanges();
                }
            }
            GV_dataImport();
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
        }

        protected void btnDel_Click2(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            Label labSN = (Label)GV_TakeOver.FindControl("labSN");
            Session["sn"] = labSN.Text;
            Guid guid = Guid.Parse(labSN.Text);
            string purch = Session["rowtext"].ToString().Substring(0, 7);
            var query =
                    from stock in mpms.TBMMANAGE_STOCK
                    where stock.OVC_PURCH.Equals(purch)
                    where stock.OVC_STOCK_SN.Equals(guid)
                    orderby stock.OVC_PURCH_6
                    select new
                    {
                        OVC_PURCH_6 = stock.OVC_PURCH_6,
                        OVC_STOCK_SN = stock.OVC_STOCK_SN
                    };
            foreach (var q in query)
            {
                TBMMANAGE_STOCK tbmmanage_stock = new TBMMANAGE_STOCK();
                tbmmanage_stock = mpms.TBMMANAGE_STOCK
                    .Where(table => table.OVC_PURCH.Equals(purch) && table.OVC_PURCH_6.Equals(q.OVC_PURCH_6) && table.OVC_STOCK_SN.Equals(q.OVC_STOCK_SN)).FirstOrDefault();
                if (q.OVC_PURCH_6.ToString() == GV_TakeOver.Cells[2].Text)
                {
                    mpms.Entry(tbmmanage_stock).State = EntityState.Deleted;
                    mpms.SaveChanges();
                }
            }
            GV_dataImport();
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
        }
        #endregion

        #region 回主流程btn
        protected void btnCashReturnMain_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }
        protected void btnPromReturnMain_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }
        protected void btnStockReturnMain_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }
        #endregion

        #endregion

        #region 副程式

        #region GridView資料帶入
        private void GV_dataImport()
        {
            if (Session["rowtext"] != null)
            {
                DataTable dt = new DataTable();
                var purch = Session["rowtext"].ToString().Substring(0, 7);

                var query =
                    from tbm1301 in mpms.TBM1301_PLAN
                    join tbm1302 in mpms.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                    where tbm1301.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_PURCH = tbm1302.OVC_PURCH,
                        OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                        OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                    };
                foreach (var q in query)
                    lblOVC_PURCH.Text = q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5;

                var queryCash =
                    from tbmmanage_cash in mpms.TBMMANAGE_CASH
                    where tbmmanage_cash.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_KIND = tbmmanage_cash.OVC_KIND,
                        OVC_PURCH_6 = tbmmanage_cash.OVC_PURCH_6,
                        OVC_OWN_NAME = tbmmanage_cash.OVC_OWN_NAME,
                        ONB_ALL_MONEY = tbmmanage_cash.ONB_ALL_MONEY,
                        OVC_RECEIVE_NO = tbmmanage_cash.OVC_RECEIVE_NO,
                        OVC_DRECEIVE = tbmmanage_cash.OVC_DRECEIVE,
                        OVC_BACK_NO = tbmmanage_cash.OVC_BACK_NO,
                        OVC_DBACK = tbmmanage_cash.OVC_DBACK,
                        OVC_SN = tbmmanage_cash.OVC_CASH_SN
                    };
                dt = CommonStatic.LinqQueryToDataTable(queryCash);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBMMANAGE_CASH, dt);

                var queryProm = 
                    from tbmmanage_prom in mpms.TBMMANAGE_PROM
                    where tbmmanage_prom.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_KIND = tbmmanage_prom.OVC_KIND,
                        OVC_PURCH_6 = tbmmanage_prom.OVC_PURCH_6,
                        OVC_OWN_NAME = tbmmanage_prom.OVC_OWN_NAME,
                        ONB_ALL_MONEY = tbmmanage_prom.ONB_ALL_MONEY,
                        OVC_RECEIVE_NO = tbmmanage_prom.OVC_RECEIVE_NO,
                        OVC_DRECEIVE = tbmmanage_prom.OVC_DRECEIVE,
                        OVC_BACK_NO = tbmmanage_prom.OVC_BACK_NO,
                        OVC_DBACK = tbmmanage_prom.OVC_DBACK,
                        OVC_SN = tbmmanage_prom.OVC_PROM_SN
                    };
                dt = CommonStatic.LinqQueryToDataTable(queryProm);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBMMANAGE_PROM, dt);

                var queryStock =
                    from tbmmanage_stock in mpms.TBMMANAGE_STOCK
                    where tbmmanage_stock.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_KIND = tbmmanage_stock.OVC_KIND,
                        OVC_PURCH_6 = tbmmanage_stock.OVC_PURCH_6,
                        OVC_OWN_NAME = tbmmanage_stock.OVC_OWN_NAME,
                        ONB_ALL_MONEY = tbmmanage_stock.ONB_ALL_MONEY,
                        OVC_RECEIVE_NO = tbmmanage_stock.OVC_RECEIVE_NO,
                        OVC_DRECEIVE = tbmmanage_stock.OVC_DRECEIVE,
                        OVC_BACK_NO = tbmmanage_stock.OVC_BACK_NO,
                        OVC_DBACK = tbmmanage_stock.OVC_DBACK,
                        OVC_SN = tbmmanage_stock.OVC_STOCK_SN
                    };
                dt = CommonStatic.LinqQueryToDataTable(queryStock);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBMMANAGE_STOCK, dt);
            }
        }
        #endregion

        #region GridView
        protected void GV_TBMMANAGE_CASH_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GV_TBMMANAGE_CASH.UseAccessibleHeader = true;
                GV_TBMMANAGE_CASH.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        protected void GV_TBMMANAGE_PROM_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GV_TBMMANAGE_PROM.UseAccessibleHeader = true;
                GV_TBMMANAGE_PROM.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        protected void GV_TBMMANAGE_CASH_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[6].Text = dateTW(e.Row.Cells[6].Text);
                e.Row.Cells[8].Text = dateTW(e.Row.Cells[8].Text);
                if (e.Row.Cells[1].Text == "1")
                    e.Row.Cells[1].Text = "履保";
                else if (e.Row.Cells[1].Text == "2")
                    e.Row.Cells[1].Text = "保固";
                var purch = Session["rowtext"].ToString().Substring(0, 7);
                var query =
                    from tbmmanage_cash in mpms.TBMMANAGE_CASH
                    where tbmmanage_cash.OVC_PURCH.Equals(purch)
                    select tbmmanage_cash.ONB_ALL_MONEY;
                foreach (var q in query)
                    e.Row.Cells[4].Text = String.Format("{0:N}", q);
            }
        }
        protected void GV_TBMMANAGE_PROM_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[6].Text = dateTW(e.Row.Cells[6].Text);
                e.Row.Cells[8].Text = dateTW(e.Row.Cells[8].Text);
                if (e.Row.Cells[1].Text == "1")
                    e.Row.Cells[1].Text = "履保";
                else if (e.Row.Cells[1].Text == "2")
                    e.Row.Cells[1].Text = "保固";
                var purch = Session["rowtext"].ToString().Substring(0, 7);
                var query =
                    from tbmmanage_prom in mpms.TBMMANAGE_PROM
                    where tbmmanage_prom.OVC_PURCH.Equals(purch)
                    select tbmmanage_prom.ONB_ALL_MONEY;
                foreach (var q in query)
                    e.Row.Cells[4].Text = String.Format("{0:N}", q);
            }
        }
        protected void GV_TBMMANAGE_STOCK_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[6].Text = dateTW(e.Row.Cells[6].Text);
                e.Row.Cells[8].Text = dateTW(e.Row.Cells[8].Text);
                if (e.Row.Cells[1].Text == "1")
                    e.Row.Cells[1].Text = "履保";
                else if (e.Row.Cells[1].Text == "2")
                    e.Row.Cells[1].Text = "保固";
                var purch = Session["rowtext"].ToString().Substring(0, 7);
                var query =
                    from tbmmanage_stock in mpms.TBMMANAGE_STOCK
                    where tbmmanage_stock.OVC_PURCH.Equals(purch)
                    select tbmmanage_stock.ONB_ALL_MONEY;
                foreach (var q in query)
                    e.Row.Cells[4].Text = String.Format("{0:N}", q);
            }
        }

        protected void GV_TBMMANAGE_STOCK_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GV_TBMMANAGE_STOCK.UseAccessibleHeader = true;
                GV_TBMMANAGE_STOCK.HeaderRow.TableSection = TableRowSection.TableHeader;
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