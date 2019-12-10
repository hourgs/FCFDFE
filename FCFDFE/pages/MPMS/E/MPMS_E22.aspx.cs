using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E22 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            if (Session["rowtext"] != null)
            {
                {
                    if (!IsPostBack)
                    {
                        TB_dataImport();
                        GV_dataImport();

                        #region SessionRemove
                        Session.Contents.Remove("isModify");
                        Session.Contents.Remove("shiptime_free");
                        Session.Contents.Remove("shiptime");
                        Session.Contents.Remove("no");
                        #endregion
                    }
                }
            }
            else
                FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
        }

        #region Click

        #region 新增btn
        protected void btnKAA_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtONB_SHIP_TIMES.Text, out int n))
            {
                Session["shiptime_free"] = txtONB_SHIP_TIMES.Text;
                Session.Contents.Remove("isModify");
                string send_url;
                send_url = "~/pages/MPMS/E/MPMS_E23.aspx";
                Response.Redirect(send_url);
            }
            else
            {
                if (txtONB_SHIP_TIMES.Text == "")
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請輸入 交貨批次");
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "交貨批次 須為數字");
            }
        }

        protected void btnKBA_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtONB_SHIP_TIMES.Text, out int n))
            {
                Session["shiptime_free"] = txtONB_SHIP_TIMES.Text;
                Session["shiptime"] = txtONB_SHIP_TIMES.Text;
                Session.Contents.Remove("isModify");
                string send_url;
                send_url = "~/pages/MPMS/E/MPMS_E30.aspx";
                Response.Redirect(send_url);
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請輸入 交貨批次");
        }

        protected void btnKCA_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtONB_SHIP_TIMES.Text, out int n))
            {
                Session["shiptime_free"] = txtONB_SHIP_TIMES.Text;
                Session.Contents.Remove("isModify");
                string send_url;
                send_url = "~/pages/MPMS/E/MPMS_E31.aspx";
                Response.Redirect(send_url);
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請輸入 交貨批次");
        }
        #endregion

        //回上一頁btn
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E21.aspx";
            Response.Redirect(send_url);
        }

        //回主流程btn
        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }

        //異動btn
        protected void btnModify_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            Session["isModify"] = "1";
            Session["shiptime"] = GV_TakeOver.Cells[3].Text;
            Session["no"] = GV_TakeOver.Cells[5].Text;
            if (GV_TakeOver.Cells[4].Text == "進口稅")
            {
                string send_url;
                send_url = "~/pages/MPMS/E/MPMS_E23.aspx";
                Response.Redirect(send_url);
            }
            else if (GV_TakeOver.Cells[4].Text == "營業稅")
            {
                string send_url;
                send_url = "~/pages/MPMS/E/MPMS_E30.aspx";
                Response.Redirect(send_url);
            }
            else if (GV_TakeOver.Cells[4].Text == "貨物稅")
            {
                string send_url;
                send_url = "~/pages/MPMS/E/MPMS_E31.aspx";
                Response.Redirect(send_url);
            }
        }
        #endregion

        #region 副程式

        #region GridView資料帶入
        private void GV_dataImport()
        {
            DataTable dt = new DataTable();
            if (Session["purch_6"] != null)
            {
                string purch_6 = Session["purch_6"].ToString();
                var query =
                    from tbmfree in mpms.TBMFREEDUTY
                    where tbmfree.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    where tbmfree.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        OVC_PURCH = tbmfree.OVC_PURCH,
                        ONB_SHIP_TIMES = tbmfree.ONB_TIMES,
                        OVC_DUTY_KIND = tbmfree.OVC_DUTY_KIND,
                        ONB_NO = tbmfree.ONB_NO,
                        OVC_GOOD_DESC = tbmfree.OVC_GOOD_DESC
                    };
                dt = CommonStatic.LinqQueryToDataTable(query);
                FCommon.GridView_dataImport(GV_FREE_TAX, dt);
            }
        }
        #endregion

        #region Table資料帶入
        private void TB_dataImport()
        {
            if (Session["rowtext"] != null)
            {
                lblOVC_PURCH.Text = Session["rowtext"].ToString();
                string purch_6 = Session["purch_6"].ToString();
                var query =
                    from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                    join tbm1301 in mpms.TBM1301_PLAN on tbmreceive.OVC_PURCH equals tbm1301.OVC_PURCH
                    where tbmreceive.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    where tbmreceive.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        ONB_SHIP_TIMES = tbmreceive.ONB_SHIP_TIMES
                    };
                foreach (var q in query)
                    txtONB_SHIP_TIMES.Text = q.ONB_SHIP_TIMES.ToString();
            }
        }
        #endregion

        #region GridView
        protected void GV_FREE_TAX_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = (e.Row.RowIndex + 1).ToString();
                if (e.Row.Cells[4].Text == "A")
                    e.Row.Cells[4].Text = "進口稅";
                else if (e.Row.Cells[4].Text == "B")
                    e.Row.Cells[4].Text = "營業稅";
                else if (e.Row.Cells[4].Text == "C")
                    e.Row.Cells[4].Text = "貨物稅";
            }
        }
        #endregion

        #endregion
    }
}