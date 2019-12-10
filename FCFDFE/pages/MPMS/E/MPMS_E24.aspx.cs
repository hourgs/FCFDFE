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
    public partial class MPMS_E24 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            if (Session["rowtext"] != null)
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
        //回編輯畫面
        protected void btnRE_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E23.aspx";
            Response.Redirect(send_url);
        }

        //選擇本項btn
        protected void btnChoose_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            TextBox txtCount = (TextBox)GV_TakeOver.FindControl("txtCount");
            TextBox txtPrice = (TextBox)GV_TakeOver.FindControl("txtPrice");
            TextBox txtPlace = (TextBox)GV_TakeOver.FindControl("txtPlace");
            string ICOUNT = GV_TakeOver.Cells[1].Text;
            string NAME = GV_TakeOver.Cells[2].Text;
            string COUNT = txtCount.Text;
            string PRICE = txtPrice.Text;
            string PLACE = txtPlace.Text;
            if (int.TryParse(COUNT, out int n) && int.TryParse(PRICE, out n))
            {
                var query =
                    from tbm1201 in mpms.TBM1201
                    join tbmreceive in mpms.TBMRECEIVE_CONTRACT on tbm1201.OVC_PURCH equals tbmreceive.OVC_PURCH
                    where tbm1201.OVC_PURCH.Equals(lblOVC_PURCH.Text)
                    where NAME.Contains(tbm1201.OVC_POI_NSTUFF_CHN)
                    select new
                    {
                        OVC_PURCH = tbm1201.OVC_PURCH,
                        OVC_PURCH_6 = tbmreceive.OVC_PURCH_6,
                        OVC_VEN_CST = tbmreceive.OVC_VEN_CST,
                        ONB_POI_ICOUNT = tbm1201.ONB_POI_ICOUNT,
                        OVC_POI_NSTUFF_CHN = tbm1201.OVC_POI_NSTUFF_CHN,
                        ONB_SHIP_TIMES = tbmreceive.ONB_SHIP_TIMES ?? 1,
                        ONB_POI_QORDER_CONT = tbm1201.ONB_POI_QORDER_CONT ?? 0,
                        ONB_POI_MPRICE_CONT = tbm1201.ONB_POI_MPRICE_CONT ?? 0
                    };
                foreach (var q in query)
                {
                    if (q.ONB_POI_ICOUNT.ToString() == ICOUNT)
                    {
                        TBMFREEDUTY_ITEM tbmfreeduty_item = new TBMFREEDUTY_ITEM();
                        tbmfreeduty_item = mpms.TBMFREEDUTY_ITEM
                            .Where(table => table.OVC_PURCH.Equals(q.OVC_PURCH) && table.ONB_ICOUNT.Equals(q.ONB_POI_ICOUNT)).FirstOrDefault();
                        if (tbmfreeduty_item == null)
                        {
                            TBMFREEDUTY_ITEM tbmfreeduty_item_new = new TBMFREEDUTY_ITEM();
                            tbmfreeduty_item_new.OVC_PURCH = q.OVC_PURCH;
                            tbmfreeduty_item_new.OVC_PURCH_6 = q.OVC_PURCH_6;
                            tbmfreeduty_item_new.OVC_VEN_CST = q.OVC_VEN_CST;
                            tbmfreeduty_item_new.ONB_TIMES = q.ONB_SHIP_TIMES;
                            tbmfreeduty_item_new.OVC_DUTY_KIND = "A";
                            tbmfreeduty_item_new.ONB_NO = short.Parse(Session["Application_times"].ToString());
                            tbmfreeduty_item_new.ONB_ICOUNT = short.Parse(ICOUNT);
                            tbmfreeduty_item_new.ONB_QDELIVERY = decimal.Parse(COUNT);
                            tbmfreeduty_item_new.ONB_MDELIVERY = decimal.Parse(PRICE);
                            tbmfreeduty_item_new.OVC_SHIP_PLACE = PLACE;
                            mpms.TBMFREEDUTY_ITEM.Add(tbmfreeduty_item_new);
                            mpms.SaveChanges();
                        }
                    }
                }
                GV_dataImport();
            }
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('填寫數量及單價');", true);
        }

        //刪除本項btn
        protected void btnDel_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            string ICOUNT = GV_TakeOver.Cells[1].Text;
            string NAME = GV_TakeOver.Cells[2].Text;
            string COUNT = GV_TakeOver.Cells[3].Text;
            string PRICE = GV_TakeOver.Cells[4].Text;
            string PLACE = GV_TakeOver.Cells[5].Text;
            var query =
                from tbmfree in mpms.TBMFREEDUTY_ITEM
                where tbmfree.OVC_PURCH.Equals(lblOVC_PURCH.Text)
                select new
                {
                    OVC_PURCH = tbmfree.OVC_PURCH,
                    ONB_ICOUNT = tbmfree.ONB_ICOUNT,
                    ONB_QDELIVERY = tbmfree.ONB_QDELIVERY,
                    ONB_MDELIVERY = tbmfree.ONB_MDELIVERY
                };
            foreach (var q in query)
            {
                if (q.ONB_ICOUNT.ToString() == ICOUNT && q.ONB_QDELIVERY.ToString() == COUNT && q.ONB_MDELIVERY.ToString() == PRICE)
                {
                    TBMFREEDUTY_ITEM tbmfreeduty_item = new TBMFREEDUTY_ITEM();
                    tbmfreeduty_item = mpms.TBMFREEDUTY_ITEM
                        .Where(table => table.OVC_PURCH.Equals(q.OVC_PURCH) && table.ONB_ICOUNT.Equals(q.ONB_ICOUNT)).FirstOrDefault();
                    if (tbmfreeduty_item != null)
                    {
                        mpms.Entry(tbmfreeduty_item).State = EntityState.Deleted;
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
                lblOVC_PURCH.Text = Session["rowtext"].ToString().Substring(0, 7);
            DataTable dt = new DataTable();
            var query =
                from tbm1201 in mpms.TBM1201
                where tbm1201.OVC_PURCH.Equals(lblOVC_PURCH.Text)
                select new
                {
                    ONB_POI_ICOUNT = tbm1201.ONB_POI_ICOUNT,
                    OVC_POI_NSTUFF_CHN = tbm1201.OVC_POI_NSTUFF_CHN,
                    OVC_MODEL = tbm1201.OVC_MODEL,
                    ONB_POI_QORDER_CONT = tbm1201.ONB_POI_QORDER_CONT ?? 0,
                    ONB_POI_MPRICE_CONT = tbm1201.ONB_POI_MPRICE_CONT ?? 0
                };
            dt = CommonStatic.LinqQueryToDataTable(query);
            FCommon.GridView_dataImport(GV_CHOOSE_FREE_TAX_DETAIL, dt);

            DataTable dt2 = new DataTable();
            var query2 =
                from tbmfree in mpms.TBMFREEDUTY_ITEM
                where tbmfree.OVC_PURCH.Equals(lblOVC_PURCH.Text)
                orderby tbmfree.ONB_ICOUNT
                select new
                {
                    ONB_ICOUNT = tbmfree.ONB_ICOUNT,
                    ONB_QDELIVERY = tbmfree.ONB_QDELIVERY,
                    ONB_MDELIVERY = tbmfree.ONB_MDELIVERY,
                    OVC_SHIP_PLACE = tbmfree.OVC_SHIP_PLACE
                };
            dt2 = CommonStatic.LinqQueryToDataTable(query2);
            FCommon.GridView_dataImport(GV_FREE_TAX_DETAIL, dt2);
        }
        #endregion

        #region GridView
        protected void GV_CHOOSE_FREE_TAX_DETAIL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
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
                foreach (var q in query)
                    if (q.ONB_POI_ICOUNT.ToString() == e.Row.Cells[1].Text) e.Row.Cells[2].Text = q.OVC_POI_NSTUFF_CHN + "<br/>" + q.OVC_MODEL;
            }
        }

        protected void GV_FREE_TAX_DETAIL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var query =
                from tbm1201 in mpms.TBM1201
                join tbmfree in mpms.TBMFREEDUTY_ITEM on tbm1201.OVC_PURCH equals tbmfree.OVC_PURCH
                where tbm1201.OVC_PURCH.Equals(lblOVC_PURCH.Text)
                where tbm1201.ONB_POI_ICOUNT.Equals(tbmfree.ONB_ICOUNT)
                //where tbm1201.ONB_POI_QORDER_CONT.Equals(tbmfree.ONB_QDELIVERY)
                //where tbm1201.ONB_POI_MPRICE_CONT.Equals(tbmfree.ONB_MDELIVERY)
                select new
                {
                    ONB_POI_ICOUNT = tbm1201.ONB_POI_ICOUNT,
                    OVC_POI_NSTUFF_CHN = tbm1201.OVC_POI_NSTUFF_CHN,
                    OVC_MODEL = tbm1201.OVC_MODEL,
                    ONB_POI_QORDER_CONT = tbm1201.ONB_POI_QORDER_CONT,
                    ONB_POI_MPRICE_CONT = tbm1201.ONB_POI_MPRICE_CONT
                };
                foreach (var q in query)
                    if (q.ONB_POI_ICOUNT.ToString() == e.Row.Cells[1].Text) e.Row.Cells[2].Text = q.OVC_POI_NSTUFF_CHN + "<br/>" + q.OVC_MODEL;
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