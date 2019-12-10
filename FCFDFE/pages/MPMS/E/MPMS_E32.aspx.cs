using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E32 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            if (Session["rowtext"] != null && Session["shiptime"] != null)
            {
                if (!IsPostBack)
                {
                    GV_dataImport();
                }
            }
            else
                FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
        }
        #region Click

        //回上一頁
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E27.aspx";
            Response.Redirect(send_url);
        }
        // 回主流程
        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }
        //選擇本項btn
        protected void btnChoose_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            TextBox txtCount = (TextBox)GV_TakeOver.FindControl("txtCount");
            TextBox txtPrice = (TextBox)GV_TakeOver.FindControl("txtPrice");
            string purch_6 = Session["purch_6"].ToString();
            string ICOUNT = GV_TakeOver.Cells[1].Text;
            string NAME = GV_TakeOver.Cells[2].Text;
            string COUNT = txtCount.Text;
            string PRICE = txtPrice.Text;
            var query =
                from tbm1201 in mpms.TBM1201
                join tbmreceive in mpms.TBMRECEIVE_CONTRACT on tbm1201.OVC_PURCH equals tbmreceive.OVC_PURCH
                where tbm1201.OVC_PURCH.Equals(lblOVC_PURCH.Text)
                where tbmreceive.OVC_PURCH_6.Equals(purch_6)
                where NAME.Contains(tbm1201.OVC_POI_NSTUFF_CHN)
                select new
                {
                    OVC_PURCH = tbm1201.OVC_PURCH,
                    OVC_PURCH_6 = tbmreceive.OVC_PURCH_6,
                    OVC_VEN_CST = tbmreceive.OVC_VEN_CST,
                    ONB_POI_ICOUNT = tbm1201.ONB_POI_ICOUNT,
                    OVC_POI_NSTUFF_CHN = tbm1201.OVC_POI_NSTUFF_CHN,
                    ONB_SHIP_TIMES = tbmreceive.ONB_SHIP_TIMES,
                    ONB_POI_QORDER_CONT = tbm1201.ONB_POI_QORDER_CONT,
                    ONB_POI_MPRICE_CONT = tbm1201.ONB_POI_MPRICE_CONT
                };
            foreach (var q in query)
            {
                if (q.ONB_POI_ICOUNT.ToString() == ICOUNT)
                {
                    TBMDELIVERY_ITEM tbmdelivery_item = new TBMDELIVERY_ITEM();
                    tbmdelivery_item = mpms.TBMDELIVERY_ITEM
                        .Where(table => table.OVC_PURCH.Equals(q.OVC_PURCH) && table.OVC_PURCH_6.Equals(purch_6) && table.ONB_ICOUNT.Equals(q.ONB_POI_ICOUNT)).FirstOrDefault();
                    if (tbmdelivery_item == null)
                    {
                        TBMDELIVERY_ITEM tbmdelivery_item_new = new TBMDELIVERY_ITEM();
                        tbmdelivery_item_new.OVC_PURCH = q.OVC_PURCH;
                        tbmdelivery_item_new.OVC_PURCH_6 = q.OVC_PURCH_6;
                        tbmdelivery_item_new.OVC_VEN_CST = q.OVC_VEN_CST;
                        if (q.ONB_SHIP_TIMES != null && short.TryParse(q.ONB_SHIP_TIMES.ToString(), out short n))
                            tbmdelivery_item_new.ONB_SHIP_TIMES = short.Parse(q.ONB_SHIP_TIMES.ToString());
                        else
                            tbmdelivery_item_new.ONB_SHIP_TIMES = 0;
                        if (short.TryParse(ICOUNT, out short s))
                            tbmdelivery_item_new.ONB_ICOUNT = short.Parse(ICOUNT);
                        if (decimal.TryParse(COUNT, out decimal d))
                            tbmdelivery_item_new.ONB_QDELIVERY = decimal.Parse(COUNT);
                        if (decimal.TryParse(PRICE, out d))
                            tbmdelivery_item_new.ONB_MDELIVERY = decimal.Parse(PRICE);
                        mpms.TBMDELIVERY_ITEM.Add(tbmdelivery_item_new);
                        mpms.SaveChanges();
                    }
                }
            }
            GV_dataImport();
        }
        //刪除本項btn
        protected void btnDel_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            string purch_6 = Session["purch_6"].ToString();
            string ICOUNT = GV_TakeOver.Cells[1].Text;
            string NAME = GV_TakeOver.Cells[2].Text;
            string COUNT = GV_TakeOver.Cells[3].Text;
            string PRICE = GV_TakeOver.Cells[4].Text;
            var query =
                from tbmdelivery in mpms.TBMDELIVERY_ITEM
                where tbmdelivery.OVC_PURCH.Equals(lblOVC_PURCH.Text)
                where tbmdelivery.OVC_PURCH_6.Equals(purch_6)
                select new
                {
                    OVC_PURCH = tbmdelivery.OVC_PURCH,
                    ONB_ICOUNT = tbmdelivery.ONB_ICOUNT,
                    ONB_QDELIVERY = tbmdelivery.ONB_QDELIVERY,
                    ONB_MDELIVERY = tbmdelivery.ONB_MDELIVERY
                };
            foreach (var q in query)
            {
                if (q.ONB_ICOUNT.ToString() == ICOUNT)
                {
                    TBMDELIVERY_ITEM tbmdelivery_item = new TBMDELIVERY_ITEM();
                    tbmdelivery_item = mpms.TBMDELIVERY_ITEM
                        .Where(table => table.OVC_PURCH.Equals(q.OVC_PURCH) && table.OVC_PURCH_6.Equals(purch_6) && table.ONB_ICOUNT.Equals(q.ONB_ICOUNT)).FirstOrDefault();
                    if (tbmdelivery_item != null)
                    {
                        mpms.Entry(tbmdelivery_item).State = EntityState.Deleted;
                        mpms.SaveChanges();
                    }
                }
            }
            GV_dataImport();
        }

        #endregion

        #region 副程式

        #region GridView資料帶入
        private void GV_dataImport()
        {
            if (Session["rowtext"] != null)
            {
                lblOVC_PURCH.Text = Session["rowtext"].ToString().Substring(0, 7);

                string purch_6 = Session["purch_6"].ToString();
                short ship = short.Parse(Session["shiptime"].ToString());
                DataTable dt = new DataTable();
                var query =
                    from tbm1201 in mpms.TBM1201
                    where tbm1201.OVC_PURCH.Equals(lblOVC_PURCH.Text)
                    select new
                    {
                        ONB_POI_ICOUNT = tbm1201.ONB_POI_ICOUNT,
                        OVC_POI_NSTUFF_CHN = tbm1201.OVC_POI_NSTUFF_CHN,
                        OVC_MODEL = tbm1201.OVC_MODEL,
                        ONB_POI_QORDER_CONT = tbm1201.ONB_POI_QORDER_CONT,
                        ONB_POI_MPRICE_CONT = tbm1201.ONB_POI_MPRICE_CONT
                    };
                dt = CommonStatic.LinqQueryToDataTable(query);
                FCommon.GridView_dataImport(GV_CHOOSE_FREE_TAX_DETAIL, dt);

                DataTable dt2 = new DataTable();
                var query2 =
                    from tbmdelivery in mpms.TBMDELIVERY_ITEM
                    where tbmdelivery.OVC_PURCH.Equals(lblOVC_PURCH.Text)
                    where tbmdelivery.OVC_PURCH_6.Equals(purch_6)
                    orderby tbmdelivery.ONB_ICOUNT
                    select new
                    {
                        ONB_ICOUNT = tbmdelivery.ONB_ICOUNT,
                        ONB_QDELIVERY = tbmdelivery.ONB_QDELIVERY,
                        ONB_MDELIVERY = tbmdelivery.ONB_MDELIVERY
                    };
                dt2 = CommonStatic.LinqQueryToDataTable(query2);
                FCommon.GridView_dataImport(GV_FREE_TAX_DETAIL, dt2);
            }
        }
        #endregion

        #region GridView
        protected void GV_CHOOSE_FREE_TAX_DETAIL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtCount = (TextBox)e.Row.FindControl("txtCount");
                TextBox txtPrice = (TextBox)e.Row.FindControl("txtPrice");
                if (decimal.TryParse(txtCount.Text, out decimal d))
                    txtCount.Text = String.Format("{0:N}", decimal.Parse(txtCount.Text));
                if (decimal.TryParse(txtPrice.Text, out d))
                    txtPrice.Text = String.Format("{0:N}", decimal.Parse(txtPrice.Text));
                var query =
                    from tbm1201 in mpms.TBM1201
                    where tbm1201.OVC_PURCH.Equals(lblOVC_PURCH.Text)
                    select new
                    {
                        ONB_POI_ICOUNT = tbm1201.ONB_POI_ICOUNT,
                        OVC_POI_NSTUFF_CHN = tbm1201.OVC_POI_NSTUFF_CHN,
                        OVC_MODEL = tbm1201.OVC_MODEL,
                        ONB_POI_QORDER_CONT = tbm1201.ONB_POI_QORDER_CONT,
                        ONB_POI_MPRICE_CONT = tbm1201.ONB_POI_MPRICE_CONT,
                        OVC_POI_NSTUFF_ENG = tbm1201.OVC_POI_NSTUFF_ENG
                    };
                foreach (var q in query)
                    if (q.ONB_POI_ICOUNT.ToString() == e.Row.Cells[1].Text)
                        e.Row.Cells[2].Text = q.OVC_POI_NSTUFF_CHN + "<br/>" + q.OVC_POI_NSTUFF_ENG;

                if (decimal.TryParse(txtCount.Text, out d) && decimal.TryParse(txtPrice.Text, out d))
                    e.Row.Cells[5].Text = (decimal.Parse(txtCount.Text) * decimal.Parse(txtPrice.Text)).ToString();
                if (decimal.TryParse(e.Row.Cells[5].Text, out d))
                    e.Row.Cells[5].Text = String.Format("{0:N}", decimal.Parse(e.Row.Cells[5].Text));
            }
        }
        protected void GV_FREE_TAX_DETAIL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (decimal.TryParse(e.Row.Cells[3].Text, out decimal d))
                    e.Row.Cells[3].Text = String.Format("{0:N}", decimal.Parse(e.Row.Cells[3].Text));
                if (decimal.TryParse(e.Row.Cells[4].Text, out d))
                    e.Row.Cells[4].Text = String.Format("{0:N}", decimal.Parse(e.Row.Cells[4].Text));
                string purch_6 = Session["purch_6"].ToString();
                var query =
                    from tbm1201 in mpms.TBM1201
                    join tbmdelivery in mpms.TBMDELIVERY_ITEM on tbm1201.OVC_PURCH equals tbmdelivery.OVC_PURCH
                    where tbm1201.OVC_PURCH.Equals(lblOVC_PURCH.Text)
                    where tbmdelivery.OVC_PURCH_6.Equals(purch_6)
                    where tbm1201.ONB_POI_ICOUNT.Equals(tbmdelivery.ONB_ICOUNT)
                    //where tbm1201.ONB_POI_QORDER_CONT.Equals(tbmfree.ONB_QDELIVERY)
                    //where tbm1201.ONB_POI_MPRICE_CONT.Equals(tbmfree.ONB_MDELIVERY)
                    select new
                    {
                        ONB_POI_ICOUNT = tbm1201.ONB_POI_ICOUNT,
                        OVC_POI_NSTUFF_CHN = tbm1201.OVC_POI_NSTUFF_CHN,
                        OVC_MODEL = tbm1201.OVC_MODEL,
                        ONB_POI_QORDER_CONT = tbm1201.ONB_POI_QORDER_CONT,
                        ONB_POI_MPRICE_CONT = tbm1201.ONB_POI_MPRICE_CONT,
                        OVC_POI_NSTUFF_ENG = tbm1201.OVC_POI_NSTUFF_ENG
                    };
                foreach (var q in query)
                    if (q.ONB_POI_ICOUNT.ToString() == e.Row.Cells[1].Text)
                        e.Row.Cells[2].Text = q.OVC_POI_NSTUFF_CHN + "<br/>" + q.OVC_POI_NSTUFF_ENG;

                if (decimal.TryParse(e.Row.Cells[3].Text, out d) && decimal.TryParse(e.Row.Cells[4].Text, out d))
                    e.Row.Cells[5].Text = (decimal.Parse(e.Row.Cells[3].Text) * decimal.Parse(e.Row.Cells[4].Text)).ToString();
                if (decimal.TryParse(e.Row.Cells[5].Text, out d))
                    e.Row.Cells[5].Text = String.Format("{0:N}", decimal.Parse(e.Row.Cells[5].Text));
            }
        }

        protected void GV_CHOOSE_FREE_TAX_DETAIL_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell tc = new TableCell();
                Label lab = new Label();
                lab.Text = "請選擇合約明細";
                lab.CssClass = "control-label";
                lab.Font.Size = 14;
                lab.ForeColor = System.Drawing.Color.Red;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 6;
                tc.BackColor = System.Drawing.Color.Yellow;
                gvRow.Cells.Add(tc);
                GV_CHOOSE_FREE_TAX_DETAIL.Controls[0].Controls.Add(gvRow);
            }
        }
        #endregion

        #endregion
    }
}