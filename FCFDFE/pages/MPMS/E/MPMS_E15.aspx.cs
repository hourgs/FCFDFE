using System;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System.Web.UI;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E15 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        string rowtext, purch_6;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            if (Session["rowtext"] != null)
            {
                if (!IsPostBack)
                {
                    GV_dataImport();

                    #region 購案編號
                    purch_6 = Session["purch_6"].ToString();
                    var purch = Session["rowtext"].ToString().Substring(0, 7);
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
                            };
                    foreach (var q in query)
                    {
                        Label1.Text = q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5;
                    }
                    #endregion
                }
            }
            else
                FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
        }

        #region Click
        //異動btn
        protected void btnChange_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            int rowindex = GV_TakeOver.RowIndex;
            string send_url;
            Session["E15"] = "1";
            switch (rowindex)
            {
                case 1:
                    send_url = "~/pages/MPMS/E/MPMS_E13.aspx";
                    Response.Redirect(send_url);
                    break;
                case 2:
                    send_url = "~/pages/MPMS/E/MPMS_E16.aspx";
                    Response.Redirect(send_url);
                    break;
                case 3:
                    send_url = "~/pages/MPMS/E/MPMS_E21.aspx";
                    Response.Redirect(send_url);
                    break;
                case 4:
                    send_url = "~/pages/MPMS/E/MPMS_E17.aspx";
                    Response.Redirect(send_url);
                    break;
                case 5:
                    send_url = "~/pages/MPMS/E/MPMS_E18.aspx";
                    Response.Redirect(send_url);
                    break;
                case 6:
                    send_url = "~/pages/MPMS/E/MPMS_E1A.aspx";
                    Response.Redirect(send_url);
                    break;
                case 7:
                    send_url = "~/pages/MPMS/E/MPMS_E1C.aspx";
                    Response.Redirect(send_url);
                    break;
            }
        }
        //結束日btn
        protected void btnSaveOVC_CONTRACT_END_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            var purch = Session["rowtext"].ToString().Substring(0, 7);
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            TextBox txtOVC_CONTRACT_END = (TextBox)GV_TakeOver.FindControl("txtOVC_CONTRACT_END");
            int rowindex = GV_TakeOver.RowIndex;
            string status = "";
            string status_plus = "";
            string ex = "";
            purch_6 = Session["purch_6"].ToString();
            //if (GV_TakeOver.Cells[1].Text.CompareTo(txtOVC_CONTRACT_END.Text) > 0)
            //    strMessage += "<p>結束日 不可早於 收辦日</p>";
            switch (rowindex)
            {
                case 2:
                    status = "32";
                    status_plus = "33";
                    break;
                case 3:
                    status = "33";
                    status_plus = "34";
                    ex = "契約管理";
                    break;
                case 4:
                    status = "34";
                    status_plus = "35";
                    ex = "待交貨";
                    break;
                case 5:
                    status = "35";
                    status_plus = "36";
                    ex = "待會驗";
                    break;
                case 6:
                    status = "36";
                    status_plus = "37";
                    ex = "待驗收";
                    break;
                case 7:
                    status = "37";
                    ex = "待結報";
                    break;
            }
            TBMSTATUS tbmstatus = new TBMSTATUS();
            tbmstatus = mpms.TBMSTATUS
                .Where(table => table.OVC_PURCH.Equals(purch) && table.OVC_PURCH_6.Equals(purch_6) && table.OVC_STATUS.Equals(status)).FirstOrDefault();
            if (tbmstatus != null)
            {
                if (status != "37" && strMessage == "")
                {
                    TBMSTATUS tbmstatus_test = new TBMSTATUS();
                    tbmstatus_test = mpms.TBMSTATUS
                        .Where(table => table.OVC_PURCH.Equals(purch) && table.OVC_PURCH_6.Equals(purch_6) && table.OVC_STATUS.Equals(status_plus)).FirstOrDefault();
                    if (tbmstatus_test != null)
                    {
                        tbmstatus_test.OVC_DBEGIN = txtOVC_CONTRACT_END.Text;
                        mpms.SaveChanges();
                    }
                    else
                    {
                        TBMSTATUS tbmstatus_new = new TBMSTATUS();
                        tbmstatus_new.OVC_PURCH = tbmstatus.OVC_PURCH;
                        tbmstatus_new.OVC_PURCH_5 = tbmstatus.OVC_PURCH_5;
                        tbmstatus_new.OVC_PURCH_6 = tbmstatus.OVC_PURCH_6;
                        tbmstatus_new.ONB_TIMES = tbmstatus.ONB_TIMES;
                        tbmstatus_new.OVC_DO_NAME = Session["username"].ToString();
                        tbmstatus_new.OVC_STATUS = status_plus;
                        tbmstatus_new.OVC_DBEGIN = txtOVC_CONTRACT_END.Text;
                        tbmstatus_new.ONB_DAYS_STD = tbmstatus.ONB_DAYS_STD;
                        tbmstatus_new.ONB_GROUP = tbmstatus.ONB_GROUP;
                        tbmstatus_new.OVC_STATUS_SN = Guid.NewGuid();
                        mpms.TBMSTATUS.Add(tbmstatus_new);
                        mpms.SaveChanges();
                    }
                }
                if (strMessage == "")
                {
                    tbmstatus.OVC_DEND = txtOVC_CONTRACT_END.Text;
                    mpms.SaveChanges();
                }
            }
            else
            {
                strMessage += "<p>請先完成 " + ex + " 作業</p>";
            }
            if (strMessage == "")
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            GV_dataImport();
        }
        //回上一頁
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            if (Session["status"] != null)
            {
                string send_url;
                send_url = "~/pages/MPMS/E/MPMS_E14.aspx";
                Response.Redirect(send_url);
            }
            else
            {
                Session["name"] = Session["username"].ToString();
                Session["listYear"] = Session["rowtext"].ToString().Substring(2, 2);
                Session["status"] = "total";
                string send_url;
                send_url = "~/pages/MPMS/E/MPMS_E14.aspx";
                Response.Redirect(send_url);
            }
        }
        #endregion

        #region 副程式

        #region GridView新增固定列
        private void GV_dataImport()
        {
            DataTable dt = new DataTable();

            for (int i = 0; i < 8; i++)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            GV_TBMANNOUNCE_OPEN.DataSource = dt.AsDataView();
            GV_TBMANNOUNCE_OPEN.DataBind();
        }
        #endregion

        #region GridView
        protected void GV_TBMANNOUNCE_OPEN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnChange = (Button)e.Row.FindControl("btnChange");
                Button btnSaveOVC_CONTRACT_END = (Button)e.Row.FindControl("btnSaveOVC_CONTRACT_END");
                Panel panDate = (Panel)e.Row.FindControl("panDate");
                TextBox txtOVC_CONTRACT_END = (TextBox)e.Row.FindControl("txtOVC_CONTRACT_END");
                Label labOVC_CONTRACT_END = (Label)e.Row.FindControl("labOVC_CONTRACT_END");
                FCommon.Controls_Attributes("readonly", "true", txtOVC_CONTRACT_END);
                if (Session["rowtext"] != null)
                {
                    DataTable dt = new DataTable();
                    rowtext = Session["rowtext"].ToString().Substring(0, 7);
                    purch_6 = Session["purch_6"].ToString();
                    var queryFirst =
                        from tbmstatus in mpms.TBMSTATUS
                        join tbm1407 in mpms.TBM1407 on tbmstatus.OVC_STATUS equals tbm1407.OVC_PHR_ID
                        where tbm1407.OVC_PHR_CATE.Equals("Q9")
                        where tbmstatus.OVC_PURCH.Equals(rowtext)
                        where tbmstatus.OVC_PURCH_6.Equals(purch_6)
                        orderby tbmstatus.OVC_STATUS
                        select new
                        {
                            OVC_DBEGIN = tbmstatus.OVC_DBEGIN,
                            OVC_DEND = tbmstatus.OVC_DEND,
                            OVC_DO_NAME = tbmstatus.OVC_DO_NAME,
                            OVC_STATUS = tbmstatus.OVC_STATUS,
                            OVC_PHR_DESC = tbm1407.OVC_PHR_DESC
                        };
                    foreach (var q in queryFirst)
                    {
                        //資料帶入
                        switch (GV_TBMANNOUNCE_OPEN.Rows.Count)
                        {
                            case 0:
                                e.Row.Cells[4].Text = "履驗單位待收辦";
                                if (q.OVC_PHR_DESC == "履驗單位待收辦")
                                {
                                    e.Row.Cells[1].Text = q.OVC_DBEGIN;
                                    txtOVC_CONTRACT_END.Text = q.OVC_DEND;
                                    labOVC_CONTRACT_END.Text = q.OVC_DEND;
                                    e.Row.Cells[3].Text = q.OVC_DO_NAME;
                                }
                                break;
                            case 1:
                                e.Row.Cells[4].Text = "契約接管";
                                if (q.OVC_PHR_DESC == "契約接管")
                                {
                                    e.Row.Cells[1].Text = q.OVC_DBEGIN;
                                    txtOVC_CONTRACT_END.Text = q.OVC_DEND;
                                    labOVC_CONTRACT_END.Text = q.OVC_DEND;
                                    e.Row.Cells[3].Text = q.OVC_DO_NAME;
                                }
                                break;
                            case 2:
                                e.Row.Cells[4].Text = "契約管理";
                                if (q.OVC_PHR_DESC == "契約管理")
                                {
                                    e.Row.Cells[1].Text = q.OVC_DBEGIN;
                                    txtOVC_CONTRACT_END.Text = q.OVC_DEND;
                                    labOVC_CONTRACT_END.Text = q.OVC_DEND;
                                    e.Row.Cells[3].Text = q.OVC_DO_NAME;
                                }
                                break;
                            case 3:
                                e.Row.Cells[4].Text = "待交貨";
                                if (q.OVC_PHR_DESC == "待交貨")
                                {
                                    e.Row.Cells[1].Text = q.OVC_DBEGIN;
                                    txtOVC_CONTRACT_END.Text = q.OVC_DEND;
                                    labOVC_CONTRACT_END.Text = q.OVC_DEND;
                                    e.Row.Cells[3].Text = q.OVC_DO_NAME;
                                }
                                break;
                            case 4:
                                e.Row.Cells[4].Text = "待會驗";
                                if (q.OVC_PHR_DESC == "待會驗")
                                {
                                    e.Row.Cells[1].Text = q.OVC_DBEGIN;
                                    txtOVC_CONTRACT_END.Text = q.OVC_DEND;
                                    labOVC_CONTRACT_END.Text = q.OVC_DEND;
                                    e.Row.Cells[3].Text = q.OVC_DO_NAME;
                                }
                                break;
                            case 5:
                                e.Row.Cells[4].Text = "待驗收";
                                if (q.OVC_PHR_DESC == "待驗收")
                                {
                                    e.Row.Cells[1].Text = q.OVC_DBEGIN;
                                    txtOVC_CONTRACT_END.Text = q.OVC_DEND;
                                    labOVC_CONTRACT_END.Text = q.OVC_DEND;
                                    e.Row.Cells[3].Text = q.OVC_DO_NAME;
                                }
                                break;
                            case 6:
                                e.Row.Cells[4].Text = "待結報";
                                if (q.OVC_PHR_DESC == "待結報")
                                {
                                    e.Row.Cells[1].Text = q.OVC_DBEGIN;
                                    txtOVC_CONTRACT_END.Text = q.OVC_DEND;
                                    labOVC_CONTRACT_END.Text = q.OVC_DEND;
                                    e.Row.Cells[3].Text = q.OVC_DO_NAME;
                                }
                                break;
                            case 7:
                                e.Row.Cells[4].Text = "保證金管制";
                                if (q.OVC_PHR_DESC == "保證金管制")
                                {
                                    e.Row.Cells[1].Text = q.OVC_DBEGIN;
                                    txtOVC_CONTRACT_END.Text = q.OVC_DEND;
                                    labOVC_CONTRACT_END.Text = q.OVC_DEND;
                                    e.Row.Cells[3].Text = q.OVC_DO_NAME;
                                }
                                break;
                        }
                    }
                }
                if (Session["userid"] != null)
                {
                    var userid = Session["userid"].ToString();
                    var queryAcc =
                        from tbm5200_1 in mpms.TBM5200_1
                        where tbm5200_1.USER_ID.Equals(userid)
                        select tbm5200_1.OVC_PRIV_LEVEL;
                    foreach (var q in queryAcc)
                    {
                        if (e.Row.Cells[4].Text == "契約接管")
                        {
                            if (q == "7" || q == "8")
                                btnChange.Visible = true;
                        }
                        if (e.Row.Cells[4].Text == "契約管理" || e.Row.Cells[4].Text == "待交貨" || e.Row.Cells[4].Text == "待會驗" || e.Row.Cells[4].Text == "待驗收" || e.Row.Cells[4].Text == "待結報")
                        {
                            if (q == "7" || q == "8")
                                btnChange.Visible = true;
                            if (q == "7")
                                btnSaveOVC_CONTRACT_END.Visible = true;
                        }
                        if (e.Row.Cells[4].Text == "保證金管制")
                        {
                            if (q == "3" || q == "7" || q == "8")
                                btnChange.Visible = true;
                            if (q == "3" || q == "7")
                                btnSaveOVC_CONTRACT_END.Visible = true;
                        }
                    }
                    var queryAcc_auth =
                        from acc_auth in mpms.ACCOUNT_AUTH
                        where acc_auth.USER_ID.Equals(userid)
                        select acc_auth.C_SN_AUTH;
                    foreach (var q in queryAcc_auth)
                    {
                        if (e.Row.Cells[4].Text == "契約接管")
                        {
                            if (q == "3403" || q == "3404")
                                btnChange.Visible = true;
                        }
                        if (e.Row.Cells[4].Text == "契約管理" || e.Row.Cells[4].Text == "待交貨" || e.Row.Cells[4].Text == "待會驗" || e.Row.Cells[4].Text == "待驗收" || e.Row.Cells[4].Text == "待結報")
                        {
                            if (q == "3403" || q == "3404")
                                btnChange.Visible = true;
                            if (q == "3403")
                                btnSaveOVC_CONTRACT_END.Visible = true;
                        }
                        if (e.Row.Cells[4].Text == "保證金管制")
                        {
                            if (q == "3403" || q == "3404")
                                btnChange.Visible = true;
                            if (q == "3403")
                                btnSaveOVC_CONTRACT_END.Visible = true;
                        }
                    }
                }
                //顯示日期
                var queryDate =
                    from tbmstatus in mpms.TBMSTATUS
                    join tbm1407 in mpms.TBM1407 on tbmstatus.OVC_STATUS equals tbm1407.OVC_PHR_ID
                    where tbm1407.OVC_PHR_CATE.Equals("Q9")
                    where tbmstatus.OVC_PURCH.Equals(rowtext)
                    where tbmstatus.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        OVC_PHR_DESC = tbm1407.OVC_PHR_DESC,
                        ONB_DAYS_STD = tbmstatus.ONB_DAYS_STD
                    };
                foreach (var q in queryDate)
                {
                    if (q.OVC_PHR_DESC == e.Row.Cells[4].Text)
                    {
                        if (e.Row.Cells[4].Text == "履驗單位待收辦" || e.Row.Cells[4].Text == "契約接管")
                        {
                            panDate.Visible = false;
                            labOVC_CONTRACT_END.Visible = true;
                        }
                        //作業天數
                        if (e.Row.Cells[1].Text != "" && labOVC_CONTRACT_END.Text != "")
                        {
                            DateTime dtOVC_DBEGIN = DateTime.ParseExact(e.Row.Cells[1].Text, "yyyy-MM-dd", null);
                            DateTime dtOVC_CONTRACT_END = DateTime.ParseExact(labOVC_CONTRACT_END.Text, "yyyy-MM-dd", null);
                            TimeSpan ts = dtOVC_CONTRACT_END - dtOVC_DBEGIN;
                            e.Row.Cells[5].Text = ts.Days.ToString();
                        }
                        if (e.Row.Cells[1].Text != "" && labOVC_CONTRACT_END.Text == "")
                        {
                            DateTime dtOVC_DBEGIN = DateTime.ParseExact(e.Row.Cells[1].Text, "yyyy-MM-dd", null);
                            TimeSpan ts = DateTime.Today - dtOVC_DBEGIN;
                            e.Row.Cells[5].Text = ts.Days.ToString();
                        }
                        if (q.ONB_DAYS_STD != 0 && q.ONB_DAYS_STD != null)
                        {
                            if (Convert.ToInt32(e.Row.Cells[5].Text) > q.ONB_DAYS_STD)
                                e.Row.Cells[5].Text += " 超過" + q.ONB_DAYS_STD;
                        }
                    }
                }
            }
        }
        protected void GV_TBMANNOUNCE_OPEN_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GV_TBMANNOUNCE_OPEN.UseAccessibleHeader = true;
                GV_TBMANNOUNCE_OPEN.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        #endregion

        #endregion
    }
}