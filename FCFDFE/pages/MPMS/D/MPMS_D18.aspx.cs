using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Globalization;


namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D18 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string strOVC_PURCH, strOVC_PUR_AGENCY, strOVC_PURCH_5;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (FCommon.getQueryString(this, "OVC_PURCH", out strOVC_PURCH, false))
                {
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_DAPPROVE);
                    TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PUR_AGENCY = tbm1301 == null ? "" : tbm1301.OVC_PUR_AGENCY;

                    TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                    if (IsOVC_DO_NAME() && !IsPostBack)
                        DataImport();
                }
                else
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
            }
        }


        protected void gvTbm1303_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string strOVC_DOPEN = ((Label)gvTbm1303.Rows[gvrIndex].FindControl("lblOVC_DOPEN")).Text;
            string strONB_TIMES = ((Label)gvTbm1303.Rows[gvrIndex].FindControl("lblONB_TIMES")).Text;
            string strONB_GROUP = ((Label)gvTbm1303.Rows[gvrIndex].FindControl("lblONB_GROUP")).Text;

            if (e.CommandName == "Enter")
            {
                //點擊 作業
                string send_url = "~/pages/MPMS/D/MPMS_D18_1.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_DOPEN=" + strOVC_DOPEN +
                                    "&ONB_TIMES=" + strONB_TIMES + "&ONB_GROUP=" + strONB_GROUP;
                Response.Redirect(send_url);
            }
            else if (e.CommandName == "Copy")
            {
                //點擊 複製資料
                string strNUM = ((TextBox)gvTbm1303.Rows[gvrIndex].FindControl("txtNUM")).Text;
                if (strNUM == "")
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請填入編號數字");
                else if ((int.Parse(strNUM) < 0) || (int.Parse(strNUM) > gvTbm1303.Rows.Count))
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無此編號");
                else
                {
                    int copyNUM = int.Parse(strNUM) - 1;
                    string copyOVC_DOPEN = ((Label)gvTbm1303.Rows[copyNUM].FindControl("lblOVC_DOPEN")).Text;
                    string copyONB_TIMES = ((Label)gvTbm1303.Rows[copyNUM].FindControl("lblONB_TIMES")).Text;
                    string copyONB_GROUP = ((Label)gvTbm1303.Rows[copyNUM].FindControl("lblONB_GROUP")).Text;
                    string send_url = "~/pages/MPMS/D/MPMS_D18_1.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_DOPEN=" + strOVC_DOPEN +
                                        "&ONB_TIMES=" + strONB_TIMES + "&ONB_GROUP=" + strONB_GROUP
                                        + "&copyDOPEN=" + copyOVC_DOPEN + "&copyTIMES=" + copyONB_TIMES + "&copyGROUP=" + copyONB_GROUP;
                    Response.Redirect(send_url);
                }
            }
            else if (e.CommandName == "ToD18_3")
            {
                //點擊 廠商編輯
                string send_url = "~/pages/MPMS/D/MPMS_D18_3.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_DOPEN=" + strOVC_DOPEN +
                                    "&ONB_TIMES=" + strONB_TIMES + "&ONB_GROUP=" + strONB_GROUP;
                Response.Redirect(send_url);
            }
            else if (e.CommandName == "ToD18_7")
            {
                string send_url = "~/pages/MPMS/D/MPMS_D18_7.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_DOPEN=" + strOVC_DOPEN +
                                    "&ONB_TIMES=" + strONB_TIMES + "&ONB_GROUP=" + strONB_GROUP;
                Response.Redirect(send_url);
            }
        }


        protected void gvTBMRECEIVE_WORK_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Button btn = (Button)e.CommandSource;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string strErrorMsg = "";
            string strONB_GROUP = ((TextBox)gvTBMRECEIVE_WORK.Rows[gvrIndex].FindControl("txtONB_GROUP")).Text;
            //點擊 新增
            if (e.CommandName == "New")
            {
                if (strONB_GROUP == "")
                    strErrorMsg = "<p>請填入組別</p>";
                else
                {
                    string strOVC_PURCH_5 = ((Label)gvTBMRECEIVE_WORK.Rows[gvrIndex].FindControl("lblOVC_PURCH_5")).Text;
                    short numONB_TIMES = short.Parse(((Label)gvTBMRECEIVE_WORK.Rows[gvrIndex].FindControl("lblONB_TIMES")).Text);
                    string strOVC_DOPEN = ((Label)gvTBMRECEIVE_WORK.Rows[gvrIndex].FindControl("lblOVC_DOPEN")).Text;
                    string strOVC_OPEN_HOUR = ((Label)gvTBMRECEIVE_WORK.Rows[gvrIndex].FindControl("lblOVC_OPEN_HOUR")).Text;
                    string strOVC_OPEN_MIN = ((Label)gvTBMRECEIVE_WORK.Rows[gvrIndex].FindControl("lblOVC_OPEN_MIN")).Text;
                    short numONB_GROUP = short.Parse(strONB_GROUP);
                    var isExist = mpms.TBM1303.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                                        && tb.ONB_TIMES == numONB_TIMES && tb.ONB_GROUP == numONB_GROUP
                                                        && tb.OVC_DOPEN.Equals(strOVC_DOPEN)).ToList();
                    if (isExist.Count == 0)
                    {
                        TBM1303 tbm1303_New = new TBM1303
                        {
                            OVC_PURCH = strOVC_PURCH,
                            OVC_PURCH_5 = strOVC_PURCH_5,
                            ONB_TIMES = numONB_TIMES,
                            OVC_DOPEN = strOVC_DOPEN,
                            OVC_OPEN_HOUR = strOVC_OPEN_HOUR,
                            OVC_OPEN_MIN = strOVC_OPEN_MIN,
                            ONB_GROUP = numONB_GROUP,
                        };
                        mpms.TBM1303.Add(tbm1303_New);
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1303_New.GetType().Name.ToString(), this, "新增");
                    }
                    else
                        strErrorMsg += "<p>此筆資料已存在，請輸入其他組別數字</p>";
                }
                if (strErrorMsg == "")
                {
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功！");
                    DataImport();
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErrorMsg);
            }
        }


        protected void btnReturn_Click(object sender, EventArgs e)
        {
            //點擊 回上一頁
            string send_url = "~/pages/MPMS/D/MPMS_D14_1.aspx?OVC_PURCH=" + strOVC_PURCH;
            Response.Redirect(send_url);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtOVC_DAPPROVE.Text != "")
            {
                //修改 OVC_STATUS="25" 開標通知的 階段結束日
                TBMSTATUS tbmSTATUS_25 = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                            && tb.ONB_TIMES == 1 && tb.OVC_STATUS.Equals("25")).FirstOrDefault();
                if (tbmSTATUS_25 != null)
                {
                    tbmSTATUS_25.OVC_DEND = txtOVC_DAPPROVE.Text;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmSTATUS_25.GetType().Name.ToString(), this, "修改");
                }

                //新增 OVC_STATUS="27" 保證金(函)收繳的 階段開始日
                string strOVC_DO_NAME = "";
                TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (tbmRECEIVE_BID != null)
                    strOVC_DO_NAME = tbmRECEIVE_BID.OVC_DO_NAME;

                TBMSTATUS tbmSTATUS_27 = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                            && tb.ONB_TIMES == 1 && tb.OVC_STATUS.Equals("27")).FirstOrDefault();
                if (tbmSTATUS_27 == null)
                {
                    TBMSTATUS tbmSTATUS_New = new TBMSTATUS
                    {
                        OVC_STATUS_SN = Guid.NewGuid(),
                        OVC_PURCH = strOVC_PURCH,
                        OVC_PURCH_5 = strOVC_PURCH_5,
                        ONB_TIMES = 1,
                        OVC_DO_NAME = strOVC_DO_NAME,
                        OVC_STATUS = "27",
                        OVC_DBEGIN = DateTime.Now.ToString("yyyy-MM-dd"),
                    };
                    mpms.TBMSTATUS.Add(tbmSTATUS_New);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmSTATUS_New.GetType().Name.ToString(), this, "新增");
                }
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功！");
            }
        }


        #region 副程式
        private bool IsOVC_DO_NAME()
        {
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strUserName, strDept = "";
            DataTable dt = new DataTable();
            if (strUSER_ID.Length > 0)
            {
                ACCOUNT ac = new ACCOUNT();
                ac = gm.ACCOUNTs.Where(tb => tb.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                if (ac != null)
                {
                    strUserName = ac.USER_NAME.ToString();
                    strDept = ac.DEPT_SN;
                    TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    if (strOVC_PURCH == "")
                        strErrorMsg = "請選擇購案";
                    else if (tbm1301 == null)
                        strErrorMsg = "查無此購案編號";
                    else
                    {
                        TBM1301_PLAN plan1301 =
                            gm.TBM1301_PLAN.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCHASE_UNIT.Equals(strDept)).FirstOrDefault();

                        TBMRECEIVE_BID tbmRECEIVE_BID =
                            mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_DO_NAME.Equals(strUserName)).FirstOrDefault();
                        if (tbmRECEIVE_BID == null || plan1301 == null)
                            strErrorMsg = "非此購案的承辦人";
                    }

                    if (strErrorMsg != "")
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErrorMsg);
                    else
                    {
                        divForm.Visible = true;
                        return true;
                    }
                }
            }
            divForm.Visible = false;
            return false;
        }

        private void DataImport()
        {
            DataTable dt = new DataTable();

            var queryTBM1303 =
                from tb1303 in mpms.TBM1303
                where tb1303.OVC_PURCH.Equals(strOVC_PURCH) && tb1303.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                orderby tb1303.ONB_TIMES, tb1303.ONB_GROUP, tb1303.OVC_DOPEN
                select new
                {
                    OVC_PURCH_A_5 = tb1303.OVC_PURCH + strOVC_PUR_AGENCY + strOVC_PURCH_5,
                    OVC_PURCH = tb1303.OVC_PURCH,
                    OVC_PURCH_5 = tb1303.OVC_PURCH_5,
                    ONB_TIMES = tb1303.ONB_TIMES,
                    ONB_GROUP = tb1303.ONB_GROUP,
                    OVC_DOPEN = tb1303.OVC_DOPEN,
                    OVC_DOPEN_tw = "",
                    OVC_OPEN_HOUR = tb1303.OVC_OPEN_HOUR,
                    OVC_OPEN_MIN = tb1303.OVC_OPEN_MIN,
                    OVC_CHAIRMAN = tb1303.OVC_CHAIRMAN,
                    OVC_RESULT = tb1303.OVC_RESULT,
                    OVC_DAPPROVE = tb1303.OVC_DAPPROVE,
                    OVC_DAPPROVE_tw = "",
                };
            dt = CommonStatic.LinqQueryToDataTable(queryTBM1303);
            foreach (DataRow rows in dt.Rows)
            {
                string rowOVC_DOPEN = rows["OVC_DOPEN"].ToString();
                int rowONB_TIMES = int.Parse(rows["ONB_TIMES"].ToString());
                int rowONB_GROUP = int.Parse(rows["ONB_GROUP"].ToString());
                rows["OVC_DOPEN_tw"] = GetTaiwanDate(rowOVC_DOPEN);
                rows["OVC_RESULT"] = GetTbm1407Desc("A8", rows["OVC_RESULT"].ToString());
                rows["OVC_DAPPROVE_tw"] = GetTaiwanDate(rows["OVC_DAPPROVE"].ToString());
            }
            FCommon.GridView_dataImport(gvTbm1303, dt);


            var queryRECEIVE_WORK =
                from tbmRECEIVE_WORK in mpms.TBMRECEIVE_WORK
                join modify in mpms.TBMANNOUNCE_MODIFY on new { tbmRECEIVE_WORK.OVC_PURCH, tbmRECEIVE_WORK.OVC_PURCH_5, tbmRECEIVE_WORK.OVC_DOPEN, tbmRECEIVE_WORK.OVC_OPEN_HOUR, tbmRECEIVE_WORK.OVC_OPEN_MIN } equals new { modify.OVC_PURCH, modify.OVC_PURCH_5, modify.OVC_DOPEN, modify.OVC_OPEN_HOUR, modify.OVC_OPEN_MIN } into mod
                from modify in mod.DefaultIfEmpty()
                where tbmRECEIVE_WORK.OVC_PURCH.Equals(strOVC_PURCH) && tbmRECEIVE_WORK.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                orderby tbmRECEIVE_WORK.ONB_TIMES
                select new
                {
                    OVC_PURCH_A_5 = tbmRECEIVE_WORK.OVC_PURCH + strOVC_PUR_AGENCY + strOVC_PURCH_5,
                    OVC_PURCH = tbmRECEIVE_WORK.OVC_PURCH,
                    OVC_PURCH_5 = tbmRECEIVE_WORK.OVC_PURCH_5,
                    ONB_TIMES = tbmRECEIVE_WORK.ONB_TIMES,
                    OVC_DOPEN_H_M = "",
                    OVC_DOPEN = !string.IsNullOrEmpty(modify.OVC_DOPEN_N) ? modify.OVC_DOPEN_N : tbmRECEIVE_WORK.OVC_DOPEN,
                    OVC_OPEN_HOUR = !string.IsNullOrEmpty(modify.OVC_OPEN_HOUR_N) ? modify.OVC_OPEN_HOUR_N : tbmRECEIVE_WORK.OVC_OPEN_HOUR,
                    OVC_OPEN_MIN = !string.IsNullOrEmpty(modify.OVC_OPEN_MIN_N) ? modify.OVC_OPEN_MIN_N : tbmRECEIVE_WORK.OVC_OPEN_MIN,
                };
            DataTable dt_New = CommonStatic.LinqQueryToDataTable(queryRECEIVE_WORK);
            foreach (DataRow rows in dt_New.Rows)
            {
                rows["OVC_DOPEN_H_M"] = GetTaiwanDate(rows["OVC_DOPEN"].ToString()) + " " + rows["OVC_OPEN_HOUR"].ToString()
                                                + "時 " + rows["OVC_OPEN_MIN"].ToString() + "分";
            }
            FCommon.GridView_dataImport(gvTBMRECEIVE_WORK, dt_New);

            var tbmstatus = mpms.TBMSTATUS
                .Where(o => o.OVC_PURCH.Equals(strOVC_PURCH) && o.OVC_PURCH_5.Equals(strOVC_PURCH_5))
                .Where(o => o.OVC_STATUS.Equals("25")).FirstOrDefault();
            if (tbmstatus != null)
                txtOVC_DAPPROVE.Text = tbmstatus.OVC_DEND != null ? tbmstatus.OVC_DEND : "";
        }
        

        public string GetTaiwanDate(string strDate)
        {
            //西元年轉民國年
            if (strDate != null && strDate != "")
            {
                DateTime datetime = Convert.ToDateTime(strDate);
                CultureInfo info = new CultureInfo("zh-TW");
                TaiwanCalendar twC = new TaiwanCalendar();
                info.DateTimeFormat.Calendar = twC;
                return datetime.ToString("yyy年MM月dd日", info);
            }
            return string.Empty;
        }


        private String GetTbm1407Desc(string cateID, string codeID)
        {
            //將TBM1407片語ID代碼轉為Desc
            TBM1407 tbm1407 = new TBM1407();
            if (codeID != null && codeID != "")
            {
                tbm1407 = gm.TBM1407.Where(table => table.OVC_PHR_CATE.Equals(cateID) && table.OVC_PHR_ID.Equals(codeID)).FirstOrDefault();
                if (tbm1407 != null)
                {
                    return tbm1407.OVC_PHR_DESC.ToString();
                }
            }
            return codeID;
        }


        #endregion

    }
}

