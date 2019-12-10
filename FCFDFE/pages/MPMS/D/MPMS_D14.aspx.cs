using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;
using System.Globalization;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D14 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string strUSER_ID, strUserName = "", strDept;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if ((string)(Session["XSSRequest"]) == "danger")
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "輸入錯誤，請重新輸入！");
                    Session["XSSRequest"] = null;
                }

                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
                if (strUSER_ID.Length > 0)
                {
                    ACCOUNT ac = new ACCOUNT();
                    ac = gm.ACCOUNTs.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                    if (ac != null)
                    {
                        strUserName = ac.USER_NAME.ToString();
                        lblUserName.Text = strUserName;
                        strDept = ac.DEPT_SN;
                        //判斷是否有新接之購案
                        hasTBMMESSAGE();
                        if (!IsPostBack)
                        {
                            List_DataImport(drpOVC_BUDGET_YEAR);
                            if (Request.QueryString["DateYear"] != null)
                            {
                                drpOVC_BUDGET_YEAR.SelectedValue = Request.QueryString["DateYear"].ToString();
                                DataImport();
                            } 
                        }
                    }
                }
            }
        }


        protected void gvSTATUS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int rowsCount = gvSTATUS.Rows.Count;
                e.Row.Cells.Clear();
                TableCell tbCell = new TableCell();
                TableCell tbCell_2 = new TableCell();
                TableCell tbCell_3 = new TableCell();
                tbCell.ColumnSpan = 2;
                tbCell.Text = "合計";
                tbCell_2.ColumnSpan =5;
                tbCell_2.Text= rowsCount.ToString();
                tbCell_3.Text ="";
                e.Row.Cells.Add(tbCell);
                e.Row.Cells.Add(tbCell_2);
                e.Row.Cells.Add(tbCell_3);
            }
        }

        protected void gvSTATUS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string strOVC_PURCH = ((Label)gvSTATUS.Rows[gvrIndex].FindControl("lblOVC_PURCH")).Text;
            string strOVC_PUR_AGENCY = ((Label)gvSTATUS.Rows[gvrIndex].FindControl("lblOVC_PUR_AGENCY")).Text;
            string strOVC_PURCH_5 = ((Label)gvSTATUS.Rows[gvrIndex].FindControl("lblOVC_PURCH_5")).Text;
            string strOVC_DO_NAME = gvSTATUS.Rows[gvrIndex].Cells[5].Text;
            string send_url;
            switch (e.CommandName)
            {
                //點擊 購案編號
                case "DataDetail":
                    send_url = "MPMS_D11_6.aspx?OVC_PURCH=" + strOVC_PURCH;
                    Response.Write("<script>window.open('" + send_url + "','_blank');</script>");
                    break;


                //點擊 輸入
                case "Insert":
                    send_url = "~/pages/MPMS/D/MPMS_D14_2.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_PURCH_5=" + strOVC_PURCH_5;
                    Response.Redirect(send_url);
                    break;

                //點擊 選擇
                case "Select":
                    send_url = "~/pages/MPMS/D/MPMS_D14_1.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_PURCH_5=" + strOVC_PURCH_5;
                    Response.Redirect(send_url);
                    break;

                default:
                    break;
            }
        }


        protected void gvSTATUS_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }



        #region Button OnClick
        
        protected void btnReviewOpinion_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/D/MPMS_D42.aspx";
            Response.Redirect(send_url);
        }

        protected void btnSearchData_Click(object sender, EventArgs e)
        {
            //按下 查詢購案移辦資料
            string send_url = "MPMS_D11_1.aspx?OVC_PURCH=" + txtOVC_PURCH.Text ;
            Response.Write("<script>window.open('" + send_url + "','_blank');</script>");
        }

        protected void btnSearchStatus_Click(object sender, EventArgs e)
        {
            //按下 查詢採購目前狀態
            string send_url = "MPMS_D11_2.aspx?OVC_PURCH=" + txtOVC_PURCH.Text;
            Response.Write("<script>window.open('" + send_url + "','_blank');</script>");
        }

        protected void btnAllRevoked_Click(object sender, EventArgs e)
        {
            //按下 查詢所有撤案資料
            string send_url = "MPMS_D11_3.aspx";
            Response.Write("<script>window.open('" + send_url + "','_blank');</script>");
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            //按下 查詢
            DataImport();
        }

        #endregion



        #region 副程式

        private void hasTBMMESSAGE()
        {
            //判定是否有新接之購案
            string strPurchNum, strPurchNums = "";
            var queryMessage = mpms.TBMMESSAGE.Where(tb => tb.OVC_PUR_SECTION.Equals(strDept) && tb.USER_NAME.Equals(strUserName) && tb.OVC_STEP.Equals("2")).OrderBy(tb => tb.OVC_PURCH).ToList();
            if (queryMessage.Count > 0)
            {
                foreach (var rows in queryMessage)
                {
                    TBMMESSAGE tbmMessage = new TBMMESSAGE();
                    strPurchNum = rows.OVC_PURCH.ToString();
                    strPurchNums = strPurchNums + "　" + strPurchNum;
                    tbmMessage = mpms.TBMMESSAGE.Where(tb => tb.OVC_PUR_SECTION.Equals(strDept) && tb.USER_NAME.Equals(strUserName) && tb.OVC_PURCH.Equals(strPurchNum)
                                && tb.OVC_STEP.Equals("2")).FirstOrDefault();
                    mpms.Entry(tbmMessage).State = EntityState.Deleted;
                    mpms.SaveChanges();
                }
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), queryMessage.GetType().Name.ToString(), this, "刪除");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('您有新案件待處理，案號:" + strPurchNums + "');", true);
            }
        }


        private void List_DataImport(ListControl list)
        {
            //帶入計畫年度下拉選單的值
            //先將下拉式選單清空
            list.Items.Clear();

            int CalDateYear = Convert.ToInt16(DateTime.Now.ToString("yyyy")) - 1911;
            int num = CalDateYear;
            for (int i = 0; i < 15; i++)
            {
                list.Items.Add(Convert.ToString(num));
                num = num - 1;
            }
        }

        private void DataImport()
        {
            //已移履驗結單之購案不顯示 (條件：tbStatus.OVC_STATUS.Substring(0, 1)) < 3 )
            DataTable dt;
            string strOVC_BUDGET_YEAR = drpOVC_BUDGET_YEAR.SelectedItem.ToString();
            string Compare = strOVC_BUDGET_YEAR.Substring((strOVC_BUDGET_YEAR.Length - 2), 2);
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strOVC_PURCH, strOVC_REMARK;
            if (strUSER_ID.Length > 0)
            {
                var queryOvc_Purch =
                    (from tbRECEIVE_BID in mpms.TBMRECEIVE_BID
                    where tbRECEIVE_BID.OVC_PURCH.Substring(2, 2).Equals(Compare)
                          && tbRECEIVE_BID.OVC_DO_NAME.Equals(strUserName)
                    select new
                    {
                        OVC_PURCH = tbRECEIVE_BID.OVC_PURCH,
                        OVC_DO_NAME = tbRECEIVE_BID.OVC_DO_NAME,
                    }).ToArray();

                var queryTBM1301 =
                    (from tbm1301Plan in gm.TBM1301_PLAN
                     where tbm1301Plan.OVC_PURCH.Substring(2, 2).Equals(Compare)
                     join tbm1301 in gm.TBM1301 on tbm1301Plan.OVC_PURCH equals tbm1301.OVC_PURCH
                     select new
                     {
                         OVC_PURCH = tbm1301Plan.OVC_PURCH,
                         OVC_PUR_AGENCY = tbm1301Plan.OVC_PUR_AGENCY,
                         OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                         OVC_PUR_USER = tbm1301.OVC_PUR_USER,
                         OVC_PERMISSION_UPDATE = tbm1301.OVC_PERMISSION_UPDATE,
                     }).ToArray();

                var queryMaxStatus =
                    from tbmStatus in mpms.TBMSTATUS
                     where tbmStatus.OVC_PURCH.Substring(2, 2).Equals(Compare)
                     group tbmStatus by tbmStatus.OVC_PURCH into tbGroup1
                     let maxStatus = tbGroup1.OrderByDescending
                         (tb => tb.OVC_STATUS == null ? "0".Length : tb.OVC_STATUS == "3" ? "30".Length : tb.OVC_STATUS.Length)
                         .ThenByDescending(tb => tb.OVC_STATUS).FirstOrDefault()
                     select new
                     {
                         OVC_PURCH = maxStatus.OVC_PURCH,
                         OVC_STATUS = maxStatus.OVC_STATUS == "22" ? "23" : (maxStatus.OVC_STATUS == "24" ? "25" : maxStatus.OVC_STATUS),
                         OVC_DBEGIN = maxStatus.OVC_DBEGIN,
                     };
                    /* select new
                    {
                        OVC_PURCH = maxStatus.OVC_PURCH,
                        OVC_STATUS = maxStatus.OVC_STATUS,
                        OVC_DBEGIN = maxStatus.OVC_DBEGIN,
                    }); */


                var queryTotal =
                    from tbRECEIVE_BID in queryOvc_Purch
                    join tbm1301 in queryTBM1301 on tbRECEIVE_BID.OVC_PURCH equals tbm1301.OVC_PURCH into tbGroup1
                    from tbm1301 in tbGroup1.DefaultIfEmpty()
                    join tbmSTATUS in queryMaxStatus on tbRECEIVE_BID.OVC_PURCH equals tbmSTATUS.OVC_PURCH into tbGroup2
                    from tbmSTATUS in tbGroup2.DefaultIfEmpty()
                    select new
                    {
                        OVC_PURCH = tbRECEIVE_BID.OVC_PURCH,
                        OVC_PUR_AGENCY = tbm1301 == null ? string.Empty : tbm1301.OVC_PUR_AGENCY,
                        OVC_PURCH_A = "",
                        OVC_PURCH_5 = "",
                        OVC_PUR_IPURCH = tbm1301 == null ? string.Empty : tbm1301.OVC_PUR_IPURCH,
                        OVC_DBEGIN = tbmSTATUS == null ? string.Empty : tbmSTATUS.OVC_DBEGIN,
                        OVC_DO_NAME = tbRECEIVE_BID.OVC_DO_NAME,
                        OVC_STATUS = tbmSTATUS == null ? string.Empty : tbmSTATUS.OVC_STATUS,
                        OVC_STATUS_Desc = "",
                        OVC_REMARK = tbm1301 == null ? string.Empty : tbm1301.OVC_PERMISSION_UPDATE,
                        Date_Flag = "",
                    };

                var queryLessThan3 =
                        from tb in queryTotal
                        where tb.OVC_STATUS == "" ? (tb.OVC_STATUS == "") : (tb.OVC_STATUS.Substring(0, 1).CompareTo("3") < 0)
                        select new
                        {
                            OVC_PURCH = tb.OVC_PURCH,
                            OVC_PUR_AGENCY = tb.OVC_PUR_AGENCY,
                            OVC_PURCH_A = "",
                            OVC_PURCH_5 = "",
                            OVC_PUR_IPURCH = tb.OVC_PUR_IPURCH,
                            OVC_DBEGIN = tb.OVC_DBEGIN,
                            OVC_DO_NAME = tb.OVC_DO_NAME,
                            OVC_STATUS = tb.OVC_STATUS,
                            OVC_STATUS_Desc = "",
                            OVC_REMARK = tb.OVC_REMARK,
                            Date_Flag = "",
                        };

                dt = CommonStatic.LinqQueryToDataTable(queryLessThan3);

                foreach (DataRow rows in dt.Rows)
                {
                    strOVC_PURCH = rows["OVC_PURCH"].ToString();
                    rows["OVC_PURCH_A"] = strOVC_PURCH + rows["OVC_PUR_AGENCY"].ToString();

                    TBMPURCH_EXT tbmPURCH_EXT = mpms.TBMPURCH_EXT.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    if (tbmPURCH_EXT != null)
                        rows["OVC_PURCH_5"] = tbmPURCH_EXT.OVC_PURCH_5;

                    string maxStatus = rows["OVC_STATUS"].ToString();
                    rows["OVC_DBEGIN"] = GetTaiwanDate(rows["OVC_DBEGIN"].ToString());
                    rows["OVC_STATUS_Desc"] = GetTbm1407Desc("Q9", maxStatus);
                    rows["Date_Flag"] = FlagVisible(rows["OVC_DBEGIN"].ToString());

                    strOVC_REMARK = (rows["OVC_STATUS"].ToString() != "" || rows["Date_Flag"].ToString() == "1") ? "<br/>" : "";
                    strOVC_REMARK += rows["OVC_REMARK"].ToString() == "Y" ? "(退委方修訂計畫中)" : "";
                    rows["OVC_REMARK"] = strOVC_REMARK;
                }
                ViewState["hasRows"] = FCommon.GridView_dataImport(gvSTATUS, dt);
            }
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
            return strDate;
        }

        

        public string FlagVisible(string strOVC_DBEGIN)
        {
            //跟現在日期差30天以上 return "1"=>顯示 /  "0"=>隱藏
            if (strOVC_DBEGIN != "")
            {
                if ((Convert.ToDateTime(strOVC_DBEGIN) <= DateTime.Now.AddDays(-30)))
                {
                    return "1";
                }
            }
            return "0";
        }


        private String GetTbm1407Desc(string cateID, string codeID)
        {
            //將TBM1407片語ID代碼轉為Desc
            TBM1407 tbm1407 = new TBM1407();
            if (codeID != null && codeID != "")
            {
                tbm1407 = gm.TBM1407.Where(tb => tb.OVC_PHR_CATE.Equals(cateID) && tb.OVC_PHR_ID.Equals(codeID)).OrderBy(tb => tb.OVC_PHR_ID).FirstOrDefault();
                if (tbm1407 != null)
                {
                    return tbm1407.OVC_PHR_DESC.ToString();
                }
            }
            return codeID;
        }


    }

        #endregion

}