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
    public partial class MPMS_E25 : System.Web.UI.Page
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
                    TB_dataImport();
                    GV_dataImport();

                    Session.Contents.Remove("isModify");
                    Session.Contents.Remove("shiptime_free");
                    Session.Contents.Remove("shiptime");
                    Session.Contents.Remove("no");
                }
            }
            else
                FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
        }

        #region Click
        //回上一頁btn
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E21.aspx";
            Response.Redirect(send_url);
        }

        //回主流程btn
        protected void btnRM_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }

        //新增btn
        protected void btnNew_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtONB_TIMES.Text, out int n))
            {
                Session["shiptime_free"] = txtONB_TIMES.Text;
                string send_url;
                send_url = "~/pages/MPMS/E/MPMS_E26.aspx";
                Response.Redirect(send_url);
            }
            else
            {
                if (txtONB_TIMES.Text == "")
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請輸入 交貨批次");
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "交貨批次 須為數字");
            }
        }

        //修改btn
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            Session["isModify"] = "1";
            Session["shiptime"] = GV_TakeOver.Cells[3].Text;
            Session["no"] = GV_TakeOver.Cells[4].Text;
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E26.aspx";
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
                from tbmaudit in mpms.TBMAUDIT
                where tbmaudit.OVC_PURCH.Equals(purch)
                where tbmaudit.OVC_PURCH_6.Equals(purch_6)
                select new
                {
                    ONB_TIMES = tbmaudit.ONB_TIMES,
                    ONB_AUDIT = tbmaudit.ONB_AUDIT,
                    OVC_PERFORMANCE_LIMIT = tbmaudit.OVC_PERFORMANCE_LIMIT,
                    OVC_VEN_ADDRESS = tbmaudit.OVC_VEN_ADDRESS,
                    OVC_DAUDIT = tbmaudit.OVC_DAUDIT,
                    OVC_PERFORMANCE_PLACE = tbmaudit.OVC_PERFORMANCE_PLACE,
                    OVC_DNOTICE = tbmaudit.OVC_DNOTICE,
                    OVC_INOTICE = tbmaudit.OVC_INOTICE,
                    OVC_DESC = tbmaudit.OVC_DESC
                };
            foreach (var q in query)
            {
                if (GV_TakeOver.Cells[3].Text == q.ONB_TIMES.ToString() && GV_TakeOver.Cells[4].Text == q.ONB_AUDIT.ToString())
                {
                    TBMAUDIT tBMAUDIT = new TBMAUDIT();
                    tBMAUDIT = mpms.TBMAUDIT
                        .Where(table => table.OVC_PURCH.Equals(purch) && table.OVC_PURCH_6.Equals(purch_6) && table.ONB_TIMES.Equals(q.ONB_TIMES) && table.OVC_DAUDIT.Equals(q.OVC_DAUDIT)).FirstOrDefault();
                    if (tBMAUDIT != null)
                    {
                        mpms.Entry(tBMAUDIT).State = EntityState.Deleted;
                        mpms.SaveChanges();
                    }
                }
            }
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
            GV_dataImport();
        }

        #endregion

        #region 副程式

        #region Table資料帶入
        private void TB_dataImport()
        {
            if (Session["rowtext"] != null)
            {
                var purch = (Session["rowtext"].ToString().Substring(0, 7));
                string purch_6 = Session["purch_6"].ToString();
                var queryPurch =
                    from tbm1301 in mpms.TBM1301_PLAN
                    join tbm1302 in mpms.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                    where tbm1301.OVC_PURCH.Equals(purch)
                    where tbm1302.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        OVC_PURCH = tbm1302.OVC_PURCH,
                        OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                        OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                        OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                        OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH
                    };
                foreach (var q in queryPurch)
                {
                    lblOVC_PURCH.Text = q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5;
                    lblOVC_PURCH_6.Text = q.OVC_PURCH_6;
                    lblOVC_TAX_STUFF.Text = q.OVC_PUR_IPURCH;
                    var queryDoName =
                        from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                        where tbmreceive.OVC_PURCH.Equals(q.OVC_PURCH)
                        select tbmreceive.OVC_DO_NAME;
                    foreach (var qu in queryDoName)
                        lblOVC_DO_NAME.Text = qu;
                }
                //var queryAudio =
                //    from tbmaudit in mpms.TBMAUDIT
                //    where tbmaudit.OVC_PURCH.Equals(purch)
                //    orderby tbmaudit.ONB_AUDIT
                //    select tbmaudit.ONB_AUDIT;
                //foreach (var q in queryAudio)
                //{
                //    if (q >= short.Parse(txtONB_AUDIT.Text))
                //    {
                //        txtONB_AUDIT.Text = (q + 1).ToString();
                //    }
                //}
            }
        }
        #endregion

        #region GridView資料帶入
        private void GV_dataImport()
        {
            DataTable dt = new DataTable();
            if (Session["purch_6"] != null)
            {
                string purch_6 = Session["purch_6"].ToString();
                var query =
                    from tbmaudit in mpms.TBMAUDIT
                    where tbmaudit.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    where tbmaudit.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        OVC_PURCH = tbmaudit.OVC_PURCH,
                        ONB_TIMES = tbmaudit.ONB_TIMES,
                        ONB_AUDIT = tbmaudit.ONB_AUDIT,
                        OVC_DO_NAME = tbmaudit.OVC_DO_NAME
                    };
                dt = CommonStatic.LinqQueryToDataTable(query);
                FCommon.GridView_dataImport(GV_FREE_TAX_DETAIL, dt);
            }
        }
        #endregion

        #region GridView
        protected void GV_FREE_TAX_DETAIL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var purch = (Session["rowtext"].ToString().Substring(0, 7));
                string purch_6 = Session["purch_6"].ToString();
                var query =
                    from tbm1301 in mpms.TBM1301_PLAN
                    join tbm1302 in mpms.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                    where tbm1301.OVC_PURCH.Equals(purch)
                    where tbm1302.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        OVC_PURCH = tbm1302.OVC_PURCH,
                        OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                        OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                        OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                        OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH
                    };
                foreach (var q in query)
                {
                    e.Row.Cells[1].Text = q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5;
                    e.Row.Cells[2].Text = q.OVC_PURCH_6;
                    e.Row.Cells[6].Text = q.OVC_PUR_IPURCH;
                }
            }
        }
        #endregion

        #endregion
    }
}