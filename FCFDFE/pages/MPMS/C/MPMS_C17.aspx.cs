using System;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Entity.GMModel;
using System.Linq;
using FCFDFE.Content;
using System.Globalization;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.C
{
    public partial class MPMS_C17 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        string strPurchNum = "";
        byte numCheckTimes = 0;
        string page = "";
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (string.IsNullOrEmpty(Request.QueryString["PurchNum"]) ||
                    string.IsNullOrEmpty(Request.QueryString["numCheckTimes"]) ||
                    string.IsNullOrEmpty(Request.QueryString["page"]))
                {
                    Response.Redirect("MPMS_C11");
                }
                else
                {
                    strPurchNum = Request.QueryString["PurchNum"];
                    numCheckTimes = Convert.ToByte(Request.QueryString["numCheckTimes"]);
                    page = Request.QueryString["page"];
                    if (!IsPostBack)
                    {
                        DataImport();
                        UserAuditUnit();
                        DrpOpinionImport();
                        GVImport();
                        AddWindowOpen();
                        AddWindowOpenTOMemo();
                    }
                }
            }
        }
        #region onClick
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            GVImport();
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string url = "";
            if (page.Equals("C14"))
            {
                url = "~/pages/MPMS/C/MPMS_C14.aspx?PurchNum=" + strPurchNum;
            }
            else
            {
                url = "MPMS_C1B.aspx";
            }
            Response.Redirect(url);
        }

        protected void btnInGV_Command(object sender, CommandEventArgs e)
        {
            if(ViewState["OVC_AUDIT_UNIT"] != null)
            {
                string strUnit = ViewState["OVC_AUDIT_UNIT"].ToString();
                Button btnThis = (Button)sender;
                int gvRowIndex = (btnThis.NamingContainer as GridViewRow).RowIndex;
                TextBox txtGV_MEMO = (TextBox)GV_Comment.Rows[gvRowIndex].Cells[4].FindControl("txtGV_MEMO");
                HiddenField hidONB_NO = (HiddenField)GV_Comment.Rows[gvRowIndex].Cells[4].FindControl("hidONB_NO");
                HiddenField hidTITLE = (HiddenField)GV_Comment.Rows[gvRowIndex].Cells[4].FindControl("hidTITLE");
                HiddenField hidTITLE_ITEM = (HiddenField)GV_Comment.Rows[gvRowIndex].Cells[4].FindControl("hidTITLE_ITEM");
                HiddenField hidTITLE_DETAIL = (HiddenField)GV_Comment.Rows[gvRowIndex].Cells[4].FindControl("hidTITLE_DETAIL");

                TBM1202_COMMENT tbm1202C = new TBM1202_COMMENT();
                byte onbNO = Convert.ToByte(hidONB_NO.Value);
                tbm1202C =
                    mpms.TBM1202_COMMENT
                    .Where(o => o.OVC_PURCH.Equals(strPurchNum)
                            && o.OVC_AUDIT_UNIT.Equals(strUnit)
                            && o.ONB_CHECK_TIMES == numCheckTimes
                            && o.ONB_NO == onbNO
                            && o.OVC_TITLE.Equals(hidTITLE.Value)
                            && o.OVC_TITLE_ITEM.Equals(hidTITLE_ITEM.Value)
                            && o.OVC_TITLE_DETAIL.Equals(hidTITLE_DETAIL.Value))
                    .FirstOrDefault();

                if (e.CommandName.Equals("ModifySave"))
                {
                    tbm1202C.OVC_CHECK_REASON = txtGV_MEMO.Text;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1202C.GetType().Name.ToString(), this, "修改");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                }
                else if (e.CommandName.Equals("Del"))
                {

                    mpms.Entry(tbm1202C).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1202C.GetType().Name.ToString(), this, "刪除");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
                }
                GVImport();
            }
            
        }

        
        #endregion

        #region 副程式
        private void UserAuditUnit()
        {
            string auditUnitNow = "";
   
            if (Session["userid"] != null)
            {
                string userID = Session["userid"].ToString();
                var qName = gm.ACCOUNTs.Where(o => o.USER_ID.Equals(userID)).Select(o => o.USER_NAME).FirstOrDefault();

                var queryUserUnit =
                    from t in mpms.TBM1202_1
                    where t.OVC_PURCH.Equals(strPurchNum) && t.ONB_CHECK_TIMES == numCheckTimes && t.OVC_AUDITOR.Equals(qName)
                    select t.OVC_AUDIT_UNIT;
                if (queryUserUnit.Any())
                {
                    var qUnitName =
                        from t in mpms.TBM1407
                        join tAuditUnit in queryUserUnit on t.OVC_PHR_ID equals tAuditUnit
                        where t.OVC_PHR_CATE.Equals("K5")
                        select new { t.OVC_PHR_ID, t.OVC_USR_ID };

                    DataTable dt = CommonStatic.LinqQueryToDataTable(qUnitName);
                    FCommon.list_dataImport(drpAuditUnit, dt, "OVC_USR_ID", "OVC_PHR_ID", false);

                    auditUnitNow = queryUserUnit.FirstOrDefault();
                    drpAuditUnit.Items.FindByValue(auditUnitNow).Selected = true;
                }
                else
                    auditUnitNow = "03";
            }
            UserInfoImport(auditUnitNow);

        }

        private void DrpOpinionImport()
        {
            string strUnit = ViewState["OVC_AUDIT_UNIT"].ToString();
            var queryOpinion = mpms.TBMOPINION.Where(o => o.OVC_AUDIT_UNIT.Equals(strUnit));
            DataTable dt = CommonStatic.LinqQueryToDataTable(queryOpinion);
            FCommon.list_dataImport(drpOVC_TITLE, dt, "OVC_CONTENT", "OVC_TITLE", true);
        }

        private void DataImport()
        {
            var query1301 = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            lblOVC_PURCH.Text = query1301.OVC_PURCH + query1301.OVC_PUR_AGENCY;
            lblOVC_PUR_IPURCH.Text = query1301.OVC_PUR_IPURCH;
            lblOVC_AGENT_UNIT.Text = query1301.OVC_PUR_NSECTION + "-" + query1301.OVC_PUR_USER + "(" 
                                   + query1301.OVC_PUR_IUSER_PHONE + " 軍線:" + query1301.OVC_PUR_IUSER_PHONE_EXT + ")";

            var query1202 =
                from t1202 in mpms.TBM1202
                 join tDetp in mpms.TBMDEPTs on t1202.OVC_CHECK_UNIT equals tDetp.OVC_DEPT_CDE
                 where t1202.OVC_PURCH.Equals(strPurchNum) && t1202.ONB_CHECK_TIMES == numCheckTimes
                 select new
                {
                    t1202.OVC_DRECEIVE,
                    OVC_DRECEIVE_PAPER = t1202.OVC_DRECEIVE_PAPER != null ? t1202.OVC_DRECEIVE_PAPER : "",
                    t1202.OVC_CHECKER,
                    tDetp.OVC_ONNAME
                };
            foreach(var item in query1202)
            {
                lblOVC_DAUDIT_ASSIGN.Text = GetTaiwanDate(item.OVC_DRECEIVE);
                lblOVC_DRECEIVE_PAPER.Text = GetTaiwanDate(item.OVC_DRECEIVE_PAPER);
                lblOVC_CHECK_UNIT.Text = item.OVC_ONNAME + "(" + item.OVC_CHECKER + ")";
                lblONB_CHECK_TIMES.Text = numCheckTimes.ToString();
            }
        }



        private void UserInfoImport(string auditUnitNow)
        {
            if (Session["userid"] != null)
            {

                string userID = Session["userid"].ToString();
                //先找Auth(新)再找5200_1(舊)
                var userInfo_AUTH =
                   (from tAccount in mpms.ACCOUNT
                    join tAuth in mpms.ACCOUNT_AUTH on tAccount.USER_ID equals tAuth.USER_ID
                    join t1407 in mpms.TBM1407 on tAuth.C_SN_SUB equals t1407.OVC_PHR_ID
                    where tAccount.USER_ID.Equals(userID) && t1407.OVC_PHR_CATE.Equals("K5") && tAuth.C_SN_SUB.Equals(auditUnitNow)
                    select new
                    {
                        tAccount.USER_ID,
                        tAccount.USER_NAME,
                        tAccount.DEPT_SN,
                        OVC_AUDIT_UNIT = tAuth.C_SN_SUB,
                        UNIT_USR_ID = t1407.OVC_USR_ID,
                        UNIT_NAME = t1407.OVC_PHR_DESC,
                    }).FirstOrDefault();
                
                if(userInfo_AUTH == null)
                {
                    var userInfo =
                    (from tAccount in mpms.ACCOUNT
                     join t52001 in mpms.TBM5200_1 on tAccount.USER_ID equals t52001.USER_ID
                     join t1407 in mpms.TBM1407 on t52001.OVC_AUDIT_UNIT equals t1407.OVC_PHR_ID
                     where tAccount.USER_ID.Equals(userID) && t1407.OVC_PHR_CATE.Equals("K5") && t52001.OVC_AUDIT_UNIT.Equals(auditUnitNow)
                     select new
                     {
                         tAccount.USER_ID,
                         tAccount.USER_NAME,
                         tAccount.DEPT_SN,
                         OVC_AUDIT_UNIT = t52001.OVC_AUDIT_UNIT,
                         UNIT_USR_ID = t1407.OVC_USR_ID,
                         UNIT_NAME = t1407.OVC_PHR_DESC,
                     }).FirstOrDefault();

                    lblTitleUnit.Text = userInfo.UNIT_USR_ID;
                    lblOVC_AUDIT_UNIT.Text = userInfo.UNIT_NAME;
                    lblUSER_ID.Text = userInfo.USER_NAME;
                    lblOVC_DAUDIT.Text = DateTime.Now.ToString();
                    ViewState["OVC_AUDIT_UNIT"] = userInfo.OVC_AUDIT_UNIT;
                    ViewState["DEPT_SN"] = userInfo.DEPT_SN;
                }
                else
                {
                    lblTitleUnit.Text = userInfo_AUTH.UNIT_USR_ID;
                    lblOVC_AUDIT_UNIT.Text = userInfo_AUTH.UNIT_NAME;
                    lblUSER_ID.Text = userInfo_AUTH.USER_NAME;
                    lblOVC_DAUDIT.Text = DateTime.Now.ToString();
                    ViewState["OVC_AUDIT_UNIT"] = userInfo_AUTH.OVC_AUDIT_UNIT;
                    ViewState["DEPT_SN"] = userInfo_AUTH.DEPT_SN;
                }
            }
        }

        private void SaveData()
        {
            //要先查項次ONB_NO
            TBM1202_COMMENT t1202C = new TBM1202_COMMENT();
            TBM1202_1 t1202_1 = new TBM1202_1();
            string strUnit = ViewState["OVC_AUDIT_UNIT"].ToString();

            var queryNO =
                mpms.TBM1202_COMMENT
                .Where(o => o.OVC_PURCH.Equals(strPurchNum)
                        && o.ONB_CHECK_TIMES == numCheckTimes
                        && o.OVC_AUDIT_UNIT.Equals(strUnit)
                        && o.OVC_TITLE.Equals(drpOVC_TITLE.SelectedValue)
                        && o.OVC_TITLE_ITEM.Equals(drpOVC_ITEM.SelectedValue)
                        && o.OVC_TITLE_DETAIL.Equals(drpOVC_Detail.SelectedValue))
                .Select(o => o.ONB_NO);
            byte ONB_NO = 0;
            if (queryNO.Any())
                ONB_NO = Convert.ToByte(queryNO.Max() + 1);
            else
                ONB_NO++;
            DateTime date = DateTime.Parse(lblOVC_DAUDIT_ASSIGN.Text);

            t1202C.OVC_PURCH = strPurchNum;
            t1202C.OVC_DRECEIVE = date.ToString("yyyy-MM-dd");
            t1202C.OVC_CHECK_UNIT = ViewState["DEPT_SN"].ToString();
            t1202C.ONB_CHECK_TIMES = numCheckTimes;
            t1202C.OVC_AUDIT_UNIT = strUnit;
            t1202C.OVC_TITLE = drpOVC_TITLE.SelectedValue;
            t1202C.OVC_TITLE_ITEM = drpOVC_ITEM.SelectedValue;
            t1202C.OVC_TITLE_DETAIL = drpOVC_Detail.SelectedValue;
            t1202C.ONB_NO = ONB_NO;
            t1202C.OVC_CHECK_REASON = txtOVC_MEMO.Text;
            mpms.TBM1202_COMMENT.Add(t1202C);
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , t1202C.GetType().Name.ToString(), this, "新增");

            t1202_1 =
                mpms.TBM1202_1
                    .Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.ONB_CHECK_TIMES == numCheckTimes
                        && o.OVC_AUDIT_UNIT.Equals(strUnit))
                    .FirstOrDefault();
            t1202_1.OVC_DAUDIT = DateTime.Now.ToString();
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , t1202_1.GetType().Name.ToString(), this, "修改");
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功");
        }

        private void GVImport()
        {
            string strUnit = ViewState["OVC_AUDIT_UNIT"].ToString();
            string[] field = { "ONB_NO", "OVC_TITLE_NAME", "OVC_TITLE_ITEM_NAME", "OVC_TITLE_DETAIL_NAME", "OVC_CHECK_REASON" };
            var query =
                from t1 in mpms.TBM1202_COMMENT
                join t2 in mpms.TBMOPINION on new { t1.OVC_AUDIT_UNIT, t1.OVC_TITLE } equals new { t2.OVC_AUDIT_UNIT, t2.OVC_TITLE }
                join t3 in mpms.TBMOPINION_ITEM
                on new { t1.OVC_AUDIT_UNIT, t1.OVC_TITLE, t1.OVC_TITLE_ITEM }
                equals new { t3.OVC_AUDIT_UNIT, t3.OVC_TITLE, t3.OVC_TITLE_ITEM }
                join t4 in mpms.TBMOPINION_DETAIL
                on new { t1.OVC_AUDIT_UNIT, t1.OVC_TITLE, t1.OVC_TITLE_ITEM, t1.OVC_TITLE_DETAIL }
                equals new { t4.OVC_AUDIT_UNIT, t4.OVC_TITLE, t4.OVC_TITLE_ITEM, t4.OVC_TITLE_DETAIL }
                where t1.OVC_PURCH.Equals(strPurchNum) && t1.ONB_CHECK_TIMES == numCheckTimes && t1.OVC_AUDIT_UNIT.Equals(strUnit)
                orderby t1.ONB_NO, t1.OVC_TITLE
                select new
                {
                    t1.ONB_NO,
                    OVC_TITLE = t1.OVC_TITLE,
                    OVC_TITLE_ITEM = t1.OVC_TITLE_ITEM,
                    OVC_TITLE_DETAIL = t1.OVC_TITLE_DETAIL,
                    OVC_TITLE_NAME = t2.OVC_CONTENT,
                    OVC_TITLE_ITEM_NAME = t3.OVC_CONTENT,
                    OVC_TITLE_DETAIL_NAME = t4.OVC_CONTENT,
                    t1.OVC_CHECK_REASON
                };
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            //要先把<br> 拿掉
                foreach (DataRow rows in dt.Rows)
                {
                    rows["OVC_CHECK_REASON"] = rows["OVC_CHECK_REASON"].ToString().Replace("<br>", "");
                }
            hasRows = FCommon.GridView_dataImport(GV_Comment, dt, field);

        }
        
        private string GetTaiwanDate(string strDate)
        {
            if (DateTime.TryParse(strDate, out DateTime datetime))
            {
                CultureInfo culture = new CultureInfo("zh-TW");
                culture.DateTimeFormat.Calendar = new TaiwanCalendar();
                return datetime.ToString("yyy年MM月dd日", culture);
            }
            else
            {
                return "";
            }

        }
      
        private void AddWindowOpen()
        {
            btn_TOCOMMENT.Attributes.Remove("onClick");

            string strURL = "";
            string strWinTitle = "";
            string strWinProperty = "";
            string strUnit = ViewState["OVC_AUDIT_UNIT"].ToString();

            strURL = "\\COMMENT.aspx?OVC_PURCH=" + strPurchNum + "&numCheckTimes=" + numCheckTimes + "&Unit=" + strUnit;
            strWinTitle = "null";
            strWinProperty = "toolbar=0,location=0,status=0,menubar=0,width=700,height=500,left=200,top=80";

            btn_TOCOMMENT.Attributes.Add("onClick", "javascript:window.open('" + strURL + "','" + strWinTitle + "','" + strWinProperty + "');return false;");
        }
        private void AddWindowOpenTOMemo()
        {
            btnToMemo.Attributes.Remove("onClick");
            string strURL = "";
            string strWinTitle = "";
            string strWinProperty = "";
            strURL = "\\RequestMemo.aspx?OVC_PURCH=" + strPurchNum;
            strWinTitle = "null";
            strWinProperty = "toolbar=0,location=0,status=0,menubar=0,width=700,height=500,left=200,top=80";

            btnToMemo.Attributes.Add("onClick", "javascript:window.open('" + strURL + "','" + strWinTitle + "','" + strWinProperty + "');return false;");
        }
        #endregion

        #region SelectedChange
        protected void drpOVC_TITLE_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strUnit = ViewState["OVC_AUDIT_UNIT"].ToString();
            if (!drpOVC_TITLE.SelectedItem.Text.Equals("請選擇"))
            {
               var queryItem =
               mpms.TBMOPINION_ITEM
               .Where(o => o.OVC_AUDIT_UNIT.Equals(strUnit)
                       && o.OVC_TITLE.Equals(drpOVC_TITLE.SelectedValue));
                DataTable dt = CommonStatic.LinqQueryToDataTable(queryItem);
                FCommon.list_dataImport(drpOVC_ITEM, dt, "OVC_CONTENT", "OVC_TITLE_ITEM", true);
                trItem.Visible = true;
            }
            else
            {
                trItem.Visible = false;
                trDetail.Visible = false;
                trMemo.Visible = false;
                trSave.Visible = false;
            }
        }
        protected void drpAuditUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInfoImport(drpAuditUnit.SelectedValue);
            GVImport();
            GVImport();
            AddWindowOpen();
            AddWindowOpenTOMemo();
        }

        protected void drpOVC_ITEM_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strUnit = ViewState["OVC_AUDIT_UNIT"].ToString();
            if (!drpOVC_ITEM.SelectedItem.Text.Equals("請選擇"))
            {
                var queryDetail =
                mpms.TBMOPINION_DETAIL
                .Where(o => o.OVC_AUDIT_UNIT.Equals(strUnit)
                        && o.OVC_TITLE.Equals(drpOVC_TITLE.SelectedValue)
                        && o.OVC_TITLE_ITEM.Equals(drpOVC_ITEM.SelectedValue));
                DataTable dt = CommonStatic.LinqQueryToDataTable(queryDetail);
                FCommon.list_dataImport(drpOVC_Detail, dt, "OVC_CONTENT", "OVC_TITLE_DETAIL", true);
                trDetail.Visible = true;
            }
            else
            {
                trDetail.Visible = false;
                trMemo.Visible = false;
                trSave.Visible = false;
            }
        }

        protected void GV_Comment_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GV_Comment.UseAccessibleHeader = true;
                GV_Comment.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void GV_Comment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button btnModifySave = (Button)e.Row.FindControl("btnModifySave");
            Button btnDel = (Button)e.Row.FindControl("btnDel");
            var queryStatus = mpms.TBMSTATUS.Where(t => t.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            if (queryStatus != null)
            {
                btnModifySave.CssClass += " disabled";
                btnDel.CssClass += " disabled";
            }
        }

        protected void drpOVC_Detail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!drpOVC_Detail.SelectedItem.Text.Equals("請選擇"))
            {
                trMemo.Visible = true;
                trSave.Visible = true;
            }
            else
            {
                trMemo.Visible = false;
                trSave.Visible = false;
            }
        }
        #endregion
    }
}