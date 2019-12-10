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
    public partial class MPMS_E18 : System.Web.UI.Page
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

                        #region SessionRemove
                        Session.Contents.Remove("txtONB_TIMES");
                        Session.Contents.Remove("txtONB_INSPECT_TIMES");
                        Session.Contents.Remove("txtONB_RE_INSPECT_TIMES");
                        Session.Contents.Remove("date");
                        Session.Contents.Remove("unit");
                        Session.Contents.Remove("isModify");
                        #endregion

                        DataTable dt = new DataTable();
                        for (int i = 0; i < 1; i++)
                        {
                            DataRow dr = dt.NewRow();
                            dt.Rows.Add(dr);
                        }
                        GV_TBMAPPLY_INSPECT.DataSource = dt.AsDataView();
                        GV_TBMAPPLY_INSPECT.DataBind();
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
            Session["txtONB_TIMES"] = GV_TakeOver.Cells[2].Text;
            Session["txtONB_INSPECT_TIMES"] = GV_TakeOver.Cells[3].Text;
            Session["txtONB_RE_INSPECT_TIMES"] = GV_TakeOver.Cells[4].Text;
            Session["date"] = GV_TakeOver.Cells[5].Text;
            Session["unit"] = GV_TakeOver.Cells[6].Text;
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E19.aspx";
            Response.Redirect(send_url);
        }
        //新增btn
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Session.Contents.Remove("isModify");
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            TextBox txtONB_TIMES = (TextBox)GV_TakeOver.FindControl("txtONB_TIMES");
            TextBox txtONB_INSPECT_TIMES = (TextBox)GV_TakeOver.FindControl("txtONB_INSPECT_TIMES");
            TextBox txtONB_RE_INSPECT_TIMES = (TextBox)GV_TakeOver.FindControl("txtONB_RE_INSPECT_TIMES");
            TextBox txtOVC_DAPPLY = (TextBox)GV_TakeOver.FindControl("txtOVC_DAPPLY");
            DropDownList drpOVC_INSPECT_UNIT = (DropDownList)GV_TakeOver.FindControl("drpOVC_INSPECT_UNIT");
            if (txtONB_TIMES.Text == "" || txtONB_INSPECT_TIMES.Text == "" || txtONB_RE_INSPECT_TIMES.Text == "" || txtOVC_DAPPLY.Text == "")
            {
                string strmeg = "";
                if (txtONB_TIMES.Text == "")
                    strmeg += "<p>請填選 批次</p>";
                if (txtONB_INSPECT_TIMES.Text == "")
                    strmeg += "<p>請填選 複驗次數</p>";
                if (txtONB_RE_INSPECT_TIMES.Text == "")
                    strmeg += "<p>請填選 再驗次數</p>";
                if (txtOVC_DAPPLY.Text == "")
                    strmeg += "<p>請填選 申請日期</p>";
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strmeg);
            }
            else if (short.TryParse(txtONB_TIMES.Text, out short s) && short.TryParse(txtONB_INSPECT_TIMES.Text, out s) && short.TryParse(txtONB_RE_INSPECT_TIMES.Text, out s))
            {
                string purch = GV_TakeOver.Cells[1].Text.Substring(0, 7);
                short onbtime = short.Parse(txtONB_TIMES.Text);
                short INSPECT_TIMES = short.Parse(txtONB_INSPECT_TIMES.Text);
                short RE_INSPECT_TIMES = short.Parse(txtONB_RE_INSPECT_TIMES.Text);
                int count = 0;
                var query =
                    from apply in mpms.TBMAPPLY_INSPECT
                    where apply.OVC_PURCH.Equals(purch)
                    select new
                    {
                        ONB_TIMES = apply.ONB_TIMES,
                        ONB_INSPECT_TIMES = apply.ONB_INSPECT_TIMES,
                        ONB_RE_INSPECT_TIMES = apply.ONB_RE_INSPECT_TIMES
                    };
                foreach (var q in query)
                    if (q.ONB_TIMES == onbtime && q.ONB_INSPECT_TIMES == INSPECT_TIMES && q.ONB_RE_INSPECT_TIMES == RE_INSPECT_TIMES) count++;

                if (count > 0)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "批次、複驗次數及再驗次數重複");
                else
                {
                    Session["txtONB_TIMES"] = txtONB_TIMES.Text;
                    Session["txtONB_INSPECT_TIMES"] = txtONB_INSPECT_TIMES.Text;
                    Session["txtONB_RE_INSPECT_TIMES"] = txtONB_RE_INSPECT_TIMES.Text;
                    Session["date"] = txtOVC_DAPPLY.Text;
                    Session["unit"] = drpOVC_INSPECT_UNIT.Text;
                    string send_url;
                    send_url = "~/pages/MPMS/E/MPMS_E19.aspx";
                    Response.Redirect(send_url);
                }
            }
            else
            {
                string strmeg = "";
                if (!short.TryParse(txtONB_TIMES.Text, out s))
                    strmeg += "<p>批次 須為數字</p>";
                if (!short.TryParse(txtONB_INSPECT_TIMES.Text, out s))
                    strmeg += "<p>複驗次數 須為數字</p>";
                if (!short.TryParse(txtONB_RE_INSPECT_TIMES.Text, out s))
                    strmeg += "<p>再驗次數 須為數字</p>";
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strmeg);
            }
        }
        //刪除btn
        protected void btnDel_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            string purch = Session["rowtext"].ToString().Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            var query =
                from tbmapply in mpms.TBMAPPLY_INSPECT
                where tbmapply.OVC_PURCH.Equals(purch)
                where tbmapply.OVC_PURCH_6.Equals(purch_6)
                orderby tbmapply.ONB_TIMES
                select new
                {
                    ONB_TIMES = tbmapply.ONB_TIMES,
                    ONB_INSPECT_TIMES = tbmapply.ONB_INSPECT_TIMES,
                    ONB_RE_INSPECT_TIMES = tbmapply.ONB_RE_INSPECT_TIMES
                };
            foreach (var q in query)
            {
                TBMAPPLY_INSPECT tbmapply_inspect = new TBMAPPLY_INSPECT();
                tbmapply_inspect = mpms.TBMAPPLY_INSPECT
                    .Where(table => table.OVC_PURCH.Equals(purch) && table.ONB_TIMES.Equals(q.ONB_TIMES) && table.ONB_INSPECT_TIMES.Equals(q.ONB_INSPECT_TIMES) && table.ONB_RE_INSPECT_TIMES.Equals(q.ONB_RE_INSPECT_TIMES)).FirstOrDefault();
                if (q.ONB_TIMES.ToString() == GV_TakeOver.Cells[2].Text && q.ONB_INSPECT_TIMES.ToString() == GV_TakeOver.Cells[3].Text && q.ONB_RE_INSPECT_TIMES.ToString() == GV_TakeOver.Cells[4].Text)
                {
                    mpms.Entry(tbmapply_inspect).State = EntityState.Deleted;
                    mpms.SaveChanges();
                }
            }
            GV_dataImport();
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
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

        #region 檢驗單位
        private void list_dataImport(ListControl list)
        {
            list.Items.Clear();
            var query =
                from tbm1407 in mpms.TBM1407
                where tbm1407.OVC_PHR_CATE.Equals("S7")
                select tbm1407.OVC_PHR_DESC;
            foreach (var q in query)
                list.Items.Add(q);
        }
        #endregion

        #region GridView資料帶入
        private void GV_dataImport()
        {
            if (Session["rowtext"] != null)
            {
                DataTable dt = new DataTable();
                var purch = Session["rowtext"].ToString().Substring(0, 7);
                string purch_6 = Session["purch_6"].ToString();
                var query =
                    from tbmapply in mpms.TBMAPPLY_INSPECT
                    where tbmapply.OVC_PURCH.Equals(purch)
                    where tbmapply.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        OVC_PURCH = tbmapply.OVC_PURCH,
                        ONB_TIMES = tbmapply.ONB_TIMES,
                        ONB_INSPECT_TIMES = tbmapply.ONB_INSPECT_TIMES,
                        ONB_RE_INSPECT_TIMES = tbmapply.ONB_RE_INSPECT_TIMES,
                        OVC_DAPPLY = tbmapply.OVC_DAPPLY,
                        OVC_INSPECT_UNIT = tbmapply.OVC_INSPECT_UNIT
                    };
                if (query.Count() > 0)
                    hasRows = true;
                dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBMAPPLY_INSPECT_NEW, dt);
            }
        }
        #endregion

        #region GridView
        protected void GV_TBMAPPLY_INSPECT_NEW_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["rowtext"] != null)
                {
                    e.Row.Cells[1].Text = Session["rowtext"].ToString();
                    e.Row.Cells[5].Text = dateTW(e.Row.Cells[5].Text);
                }
            }
        }
        protected void GV_TBMAPPLY_INSPECT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList drpOVC_INSPECT_UNIT = (DropDownList)e.Row.FindControl("drpOVC_INSPECT_UNIT");
                TextBox txtOVC_DAPPLY = (TextBox)e.Row.FindControl("txtOVC_DAPPLY");
                FCommon.Controls_Attributes("readonly", "true", txtOVC_DAPPLY);
                if (Session["rowtext"] != null)
                    e.Row.Cells[1].Text = Session["rowtext"].ToString();
                list_dataImport(drpOVC_INSPECT_UNIT);
            }
        }
        protected void GV_TBMAPPLY_INSPECT_NEW_PreRender(object sender, EventArgs e)
        {
            //if (hasRows)
            //{
            //    GV_TBMAPPLY_INSPECT_NEW.UseAccessibleHeader = true;
            //    GV_TBMAPPLY_INSPECT_NEW.HeaderRow.TableSection = TableRowSection.TableHeader;
            //}
        }
        protected void GV_TBMAPPLY_INSPECT_PreRender(object sender, EventArgs e)
        {
            //if (hasRows)
            //{
            //    GV_TBMAPPLY_INSPECT.UseAccessibleHeader = true;
            //    GV_TBMAPPLY_INSPECT.HeaderRow.TableSection = TableRowSection.TableHeader;
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