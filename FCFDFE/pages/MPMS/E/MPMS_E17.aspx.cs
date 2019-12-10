using System;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;
using System.Web.UI;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E17 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        bool hasRows = false;
        int count = 0;
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
                        GV_NewTBMDELIVERY_ITEM.DataSource = dt.AsDataView();
                        GV_NewTBMDELIVERY_ITEM.DataBind();
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
            Session["isModify"] = "1";
            Session["shiptime"] = GV_TakeOver.Cells[1].Text;
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E27.aspx";
            Response.Redirect(send_url);
        }
        //新增btn
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            TextBox TextBox1 = (TextBox)GV_TakeOver.FindControl("TextBox1");
            if (!int.TryParse(TextBox1.Text, out int n))
            {
                if (TextBox1.Text == "")
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請填選 交貨批次 ");
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "交貨批次 須為數字");
            }
            else
            {
                int c = 0;
                if (Session["rowtext"] != null)
                {
                    var purch = Session["rowtext"].ToString().Substring(0, 7);
                    var query =
                        from tbmdelivery in mpms.TBMDELIVERY
                        where tbmdelivery.OVC_PURCH.Equals(purch)
                        select tbmdelivery.ONB_SHIP_TIMES;
                    foreach (var q in query)
                        if (q.ToString() == TextBox1.Text) c++;
                    if (c > 0)
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "交貨批次重複");
                    else
                    {
                        Session.Contents.Remove("isModify");
                        Session["shiptime"] = TextBox1.Text;
                        string send_url;
                        send_url = "~/pages/MPMS/E/MPMS_E27.aspx";
                        Response.Redirect(send_url);
                    }
                }
            }
        }
        //回上一頁
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }
        //檢驗申請
        protected void btnApply_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E18.aspx";
            Response.Redirect(send_url);
        }
        //結算證明
        protected void btnSettlement_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E1A.aspx";
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
                    from Tbmdelivery in mpms.TBMDELIVERY
                    where Tbmdelivery.OVC_PURCH.Equals(purch)
                    where Tbmdelivery.OVC_PURCH_6.Equals(purch_6)
                    orderby Tbmdelivery.ONB_SHIP_TIMES
                    select new
                    {
                        ONB_SHIP_TIMES = Tbmdelivery.ONB_SHIP_TIMES
                    };
            foreach (var q in query)
            {
                if (q.ONB_SHIP_TIMES.ToString() == GV_TakeOver.Cells[1].Text)
                {
                    TBMDELIVERY tbmdelivery = new TBMDELIVERY();
                    tbmdelivery = mpms.TBMDELIVERY
                        .Where(table => table.OVC_PURCH.Equals(purch) && table.OVC_PURCH_6.Equals(purch_6) && table.ONB_SHIP_TIMES.Equals(q.ONB_SHIP_TIMES)).FirstOrDefault();
                    mpms.Entry(tbmdelivery).State = EntityState.Deleted;
                    mpms.SaveChanges();
                }
            }
            GV_dataImport();
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
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
                    from tbmdelivery in mpms.TBMDELIVERY
                    where tbmdelivery.OVC_PURCH.Equals(purch)
                    where tbmdelivery.OVC_PURCH_6.Equals(purch_6)
                    orderby tbmdelivery.ONB_SHIP_TIMES
                    select new
                    {
                        ONB_SHIP_TIMES = tbmdelivery.ONB_SHIP_TIMES,
                        OVC_DELIVERY_CONTRACT = tbmdelivery.OVC_DELIVERY_CONTRACT,
                        OVC_DELIVERY = tbmdelivery.OVC_DELIVERY,
                        OVC_DJOINCHECK = tbmdelivery.OVC_DJOINCHECK,
                        OVC_DPAY = tbmdelivery.OVC_DPAY
                    };
                if (query.Count() > 0)
                    hasRows = true;
                dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBMDELIVERY_ITEM, dt);
            }

        }
        #endregion

        #region GridView
        protected void GV_TBMDELIVERY_ITEM_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                count++;
                lblONB_DELIVERY_TIMES.Text = count.ToString();
                e.Row.Cells[2].Text = dateTW(e.Row.Cells[2].Text);
                e.Row.Cells[3].Text = dateTW(e.Row.Cells[3].Text);
                e.Row.Cells[4].Text = dateTW(e.Row.Cells[4].Text);
                e.Row.Cells[5].Text = dateTW(e.Row.Cells[5].Text);
            }
        }
        protected void GV_TBMDELIVERY_ITEM_PreRender(object sender, EventArgs e)
        {
            //if (hasRows)
            //{
            //    GV_TBMDELIVERY_ITEM.UseAccessibleHeader = true;
            //    GV_TBMDELIVERY_ITEM.HeaderRow.TableSection = TableRowSection.TableHeader;
            //}
        }

        protected void GV_NewTBMDELIVERY_ITEM_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox TextBox1 = (TextBox)e.Row.FindControl("TextBox1");
                if (Session["rowtext"] != null)
                {
                    var purch = Session["rowtext"].ToString().Substring(0, 7);
                    string purch_6 = Session["purch_6"].ToString();
                    var query =
                        from tbmdelivery in mpms.TBMDELIVERY
                        where tbmdelivery.OVC_PURCH.Equals(purch)
                        where tbmdelivery.OVC_PURCH_6.Equals(purch_6)
                        orderby tbmdelivery.ONB_SHIP_TIMES
                        select new
                        {
                            ONB_SHIP_TIMES = tbmdelivery.ONB_SHIP_TIMES,
                            OVC_DELIVERY_CONTRACT = tbmdelivery.OVC_DELIVERY_CONTRACT,
                            OVC_DELIVERY = tbmdelivery.OVC_DELIVERY,
                            OVC_DJOINCHECK = tbmdelivery.OVC_DJOINCHECK,
                            OVC_DPAY = tbmdelivery.OVC_DPAY
                        };
                    foreach (var q in query)
                        if (q.ONB_SHIP_TIMES >= short.Parse(TextBox1.Text)) TextBox1.Text = (q.ONB_SHIP_TIMES + 1).ToString();
                }
            }
        }
        protected void GV_NewTBMDELIVERY_ITEM_PreRender(object sender, EventArgs e)
        {
            //if (hasRows)
            //{
            //    GV_NewTBMDELIVERY_ITEM.UseAccessibleHeader = true;
            //    GV_NewTBMDELIVERY_ITEM.HeaderRow.TableSection = TableRowSection.TableHeader;
            //}
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