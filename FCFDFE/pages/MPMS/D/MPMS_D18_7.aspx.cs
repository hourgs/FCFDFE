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
using TemplateEngine.Docx;
using System.IO;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D18_7 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strOVC_PURCH, strOVC_PURCH_5 = "", strOVC_DOPEN;
        short numONB_TIMES, numONB_GROUP;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            {
                if (Request.QueryString["OVC_PURCH"] != null & Request.QueryString["OVC_DOPEN"] !=null && Request.QueryString["ONB_TIMES"] != null && Request.QueryString["ONB_GROUP"] != null)
                {
                    strOVC_PURCH = Request.QueryString["OVC_PURCH"].ToString();
                    strOVC_DOPEN = Request.QueryString["OVC_DOPEN"].ToString();
                    short.TryParse(Request.QueryString["ONB_TIMES"].ToString(), out numONB_TIMES);
                    short.TryParse(Request.QueryString["ONB_GROUP"].ToString(), out numONB_GROUP);

                    TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    if (tbmRECEIVE_BID != null)
                    {
                        ViewState["OVC_DO_NAME"] = tbmRECEIVE_BID.OVC_DO_NAME;
                        strOVC_PURCH_5 = tbmRECEIVE_BID.OVC_PURCH_5;
                    }

                    ViewState["strOVC_PURCH_5"] = strOVC_PURCH_5;
                    ViewState["OVC_DOPEN"] = strOVC_DOPEN;
                    if (IsOVC_DO_NAME() && !IsPostBack)
                    {
                        PanelDBID.Visible = false;
                        DataImport();
                        fotterdata(numONB_GROUP.ToString());
                        ViewState["group"] = numONB_GROUP.ToString();

                        FCommon.Controls_Attributes("readonly", "true", txtOVC_DBID, txtOVC_DAPPROVE, txtOVC_VEN_TITLE);
                    }
                }
                else
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
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
                        FCommon.AlertShow(PnMessage_Top, "danger", "系統訊息", strErrorMsg);
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
            int onbgroup = numONB_GROUP;
            string strOVC_PURCH_5_GROUP = strOVC_PURCH_5 + onbgroup.ToString();
            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH));
            string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH_5_GROUP));

            lblONB_GROUP.Text = onbgroup.ToString();
            //得標商下拉選單
            var querytbm1313 =
                (from tbm1313 in mpms.TBM1313
                 where tbm1313.OVC_PURCH == strOVC_PURCH
                 where tbm1313.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbm1313.ONB_GROUP == onbgroup
                 select tbm1313).ToList();
            DataTable dt = CommonStatic.ListToDataTable(querytbm1313);
            FCommon.list_dataImport(drpOVC_VEN_TITLE, dt, "OVC_VEN_TITLE", "OVC_VEN_CST", true);

            string strpurass = "";
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            if (query1301 != null)
            {
                strpurass = query1301.OVC_PUR_ASS_VEN_CODE;
                lblOVC_PURCH2.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                lblOVC_PUR_IPURCH.Text = query1301.OVC_PUR_IPURCH;
                lblOVC_PUR_NSECTION.Text = query1301.OVC_PUR_NSECTION;

                int onbbudgetbuy = Convert.ToInt32(query1301.ONB_PUR_BUDGET);//採購金額
                if (query1301.ONB_RESERVE_AMOUNT != null)//採購金額計算
                    onbbudgetbuy = Convert.ToInt32(query1301.ONB_PUR_BUDGET) + Convert.ToInt32(query1301.ONB_RESERVE_AMOUNT);
                lblOVC_PUR_CURRENT_1.Text = query1301.OVC_PUR_CURRENT == null ? "" : GetTbm1407Desc("B0", query1301.OVC_PUR_CURRENT);
                lblOVC_PUR_CURRENT_2.Text = query1301.OVC_PUR_CURRENT == null ? "" : GetTbm1407Desc("B0", query1301.OVC_PUR_CURRENT);
                lblOVC_BUDGET_BUY.Text = onbbudgetbuy.ToString();
                txtONB_PUR_BUDGET.Text = query1301.ONB_PUR_BUDGET.ToString();
            }

            string stropenmthod = "";
            var query1303 =
                (from tbm1303 in mpms.TBM1303
                 where tbm1303.OVC_PURCH.Equals(strOVC_PURCH) && tbm1303.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                 && tbm1303.OVC_DOPEN.Equals(strOVC_DOPEN) && tbm1303.ONB_GROUP == onbgroup && tbm1303.ONB_TIMES == numONB_TIMES
                 select tbm1303).FirstOrDefault();
            if (query1303 != null)
            {
                lblOVC_RESULT_CURRENT.Text = query1303.OVC_RESULT_CURRENT == null ? "" : GetTbm1407Desc("B0", query1303.OVC_RESULT_CURRENT);
                lblOVC_CURRENT.Text = query1303.OVC_CURRENT == null ? "" : GetTbm1407Desc("B0", query1303.OVC_CURRENT);
                stropenmthod = query1303.OVC_OPEN_METHOD;
                lblOVC_DOPEN.Text = query1303.OVC_DOPEN;
                lblOVC_DOPEN_tw.Text = GetTaiwanDate(query1303.OVC_DOPEN);
                lblOVC_OPEN_HOUR.Text = query1303.OVC_OPEN_HOUR + "時";
                lblOVC_OPEN_MIN.Text = query1303.OVC_OPEN_MIN + "分";
                txtONB_BID_RESULT.Text = query1303.ONB_BID_RESULT.ToString();
                txtONB_BID_BUDGET.Text = query1303.ONB_BID_BUDGET.ToString();
                int onbremainbudget = Convert.ToInt32(query1303.ONB_BID_BUDGET) - Convert.ToInt32(query1303.ONB_BID_RESULT);//標餘款
                txtONB_REMAIN_BUDGET.Text = onbremainbudget.ToString();
                lblONB_TIMES.Text = numONB_TIMES.ToString();
                txtOVC_DAPPROVE.Text = query1303.OVC_DAPPROVE;
                if (query1303.OVC_RESULT == "0" || query1303.OVC_RESULT == "3")//決標日顯示條件
                    PanelDBID.Visible = true;
            }

            var queryRESULT =
                (from result in mpms.TBMBID_RESULT
                 where result.OVC_PURCH == strOVC_PURCH && result.OVC_PURCH_5 == strOVC_PURCH_5
                 && result.OVC_DOPEN.Equals(strOVC_DOPEN) && result.ONB_GROUP == onbgroup
                 select result).FirstOrDefault();
            if (queryRESULT != null)
            {
                txtOVC_DESC.Text = queryRESULT.OVC_DESC;
                txtOVC_MEETING.Text = queryRESULT.OVC_MEETING;
                txtOVC_DRAFT.Text = queryRESULT.OVC_DRAFT;
                SetChecked(queryRESULT.OVC_DRAFT);
                //txtOVC_VEN_TITLE.Text = queryRESULT.OVC_VENDORS_NAME;
                if (query1303 != null)
                {
                    if (query1303.OVC_RESULT == "0" || query1303.OVC_RESULT == "3")//決標日顯示條件
                    {
                        PanelDBID.Visible = true;
                        txtOVC_DBID.Text = queryRESULT.OVC_DBID;
                    }
                }
            }


            //  ---- 自動帶入得標商 -----
            var query1302 =
                from tbm1302 in mpms.TBM1302
                where tbm1302.OVC_PURCH.Equals(strOVC_PURCH) && tbm1302.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                        && tbm1302.OVC_DOPEN.Equals(strOVC_DOPEN) && tbm1302.ONB_GROUP == onbgroup
                select new
                {
                    OVC_PURCH = tbm1302.OVC_PURCH,
                    OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                    OVC_DOPEN = tbm1302.OVC_DOPEN,
                    ONB_GROUP = tbm1302.ONB_GROUP,
                    OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE,
                };

            DataTable dt1302 = CommonStatic.LinqQueryToDataTable(query1302);
            if (dt1302.Rows.Count != 0)
            {
                string strOVC_VEN_TITLE = "";
                foreach (DataRow rows in dt1302.Rows)
                {
                    strOVC_VEN_TITLE += strOVC_VEN_TITLE == "" ? rows["OVC_VEN_TITLE"].ToString() : "," + rows["OVC_VEN_TITLE"].ToString();
                }
                txtOVC_VEN_TITLE.Text = strOVC_VEN_TITLE;
            }

            var query1407C7 =
                (from tbm1407 in mpms.TBM1407
                 where tbm1407.OVC_PHR_CATE == "C7"
                 where tbm1407.OVC_PHR_ID == strpurass
                 select tbm1407).FirstOrDefault();
            if (query1407C7 != null)
            {
                string strC7 = query1407C7.OVC_PHR_DESC;
            }

            string strR8 = "";
            var query1407R8 =
                (from tbm1407 in mpms.TBM1407
                 where tbm1407.OVC_PHR_CATE == "R8"
                 where tbm1407.OVC_PHR_ID == stropenmthod
                 select tbm1407).FirstOrDefault();
            if (query1407R8 != null)
            {
                strR8 = query1407R8.OVC_PHR_DESC;
            }
        }




        private void fotterdata(string strONB_GROUP)
        {
            int onbgroup = Convert.ToInt32(strONB_GROUP);
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();

            var query1303 =
                (from tbm1303 in mpms.TBM1303
                 where tbm1303.OVC_PURCH == strOVC_PURCH
                 where tbm1303.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbm1303.ONB_GROUP == onbgroup
                 select tbm1303).FirstOrDefault();
            var query1302 =
                (from tbm1302 in mpms.TBM1302
                 where tbm1302.OVC_PURCH == strOVC_PURCH
                 where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbm1302.ONB_GROUP == onbgroup
                 select tbm1302).FirstOrDefault();

            lblOVC_DOPEN_1.Text = GetTaiwanDate(query1303.OVC_DOPEN);
            lblOVC_OPEN_HOUR_1.Text = query1303.OVC_OPEN_HOUR + "時";
            lblOVC_OPEN_MIN_1.Text = query1303.OVC_OPEN_MIN + "分";
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            lblOVC_PURCH.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5 + "(" + strONB_GROUP + ")";
            if (query1302 != null)
            {
                lblOVC_PURCH_6.Text = query1302.OVC_PURCH_6;
                lblOVC_DOPEN_2.Text = GetTaiwanDate(query1302.OVC_DOPEN);
                lblOVC_DBID.Text = query1302.OVC_DBID;
                lblOVC_VEN_TITLE.Text = query1302.OVC_VEN_TITLE;
                lblOVC_VEN_CST.Text = query1302.OVC_VEN_CST;
                if (query1302.ONB_MONEY != 0 || query1302.ONB_MONEY != null)
                {
                    string strCURRENT = query1302.OVC_CURRENT;
                    var query1407B0 =
                        (from tbm1407 in mpms.TBM1407
                         where tbm1407.OVC_PHR_CATE == "B0"
                         where tbm1407.OVC_PHR_ID == strCURRENT
                         select tbm1407).FirstOrDefault();
                    lblONB_BID_RESULT.Text = (query1407B0 == null ? "" : query1407B0.OVC_PHR_DESC) + " " + query1302.ONB_MONEY.ToString();
                }
                else
                    lblONB_BID_RESULT.Text = "尚未於契約製作輸入合約金額";

            }

        }

        public string GetTaiwanDate(string strDate)
        {
            //西元年轉民國年
            if (strDate != "")
            {
                DateTime datetime = Convert.ToDateTime(strDate);
                CultureInfo info = new CultureInfo("zh-TW");
                TaiwanCalendar twC = new TaiwanCalendar();
                info.DateTimeFormat.Calendar = twC;
                return datetime.ToString("yyy年MM月dd日", info);
            }
            return strDate;
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

        private String GetChecked()
        {
            string strOVC_DRAFT = "";
            for (int i = 0; i < chkOVC_DRAFT.Items.Count; i++)
            {
                if (chkOVC_DRAFT.Items[i].Selected == true)
                {
                    strOVC_DRAFT += strOVC_DRAFT == "" ? chkOVC_DRAFT.Items[i].Value : "," + chkOVC_DRAFT.Items[i].Value;
                }
            }
            return strOVC_DRAFT;
        }


        private void SetChecked(string strOVC_DRAFT)
        {
            if (strOVC_DRAFT.ToString().Trim().Length != 0)
            {
                string[] selectValue = strOVC_DRAFT.Split(',');
                for (int intItem = 0; intItem <= selectValue.Length - 1; intItem++)
                {
                    for (int i = 0; i < chkOVC_DRAFT.Items.Count; i++)
                    {
                        if (selectValue[intItem] == chkOVC_DRAFT.Items[i].Value)
                        {
                            chkOVC_DRAFT.Items[i].Selected = true;
                            break;
                        }
                    }
                }
            }
        }


        #endregion






        #region click
        protected void btnToDefault_Click(object sender, EventArgs e)
        {
            //點擊 回開標紀錄選擇畫面
            string send_url = "~/pages/MPMS/D/MPMS_D18.aspx?OVC_PURCH=" + strOVC_PURCH;
            Response.Redirect(send_url);
        }

        protected void drpOVC_VEN_TITLE_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOVC_VEN_TITLE.Text = drpOVC_VEN_TITLE.SelectedItem.ToString();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            #region strmessage
            string strMessage = "";
            string strONB_PUR_BUDGET = txtONB_PUR_BUDGET.Text;
            string strONB_BID_RESULT = txtONB_BID_RESULT.Text;
            string strONB_BID_BUDGET = txtONB_BID_BUDGET.Text;
            string strOVC_VEN_TITLE = txtOVC_VEN_TITLE.Text;

            if (PanelDBID.Visible == true)
            {
                if (txtOVC_DBID.Text.Equals(string.Empty))
                    strMessage += "<P> 決標日欄位不得為空白 </p>";
            }
            if (strONB_PUR_BUDGET.Equals(string.Empty))
                strMessage += "<P> 預算金額欄位不得為空白 </p>";
            if (strONB_BID_RESULT.Equals(string.Empty))
                strMessage += "<P> 決標金額欄位不得為空白 </p>";
            if (strONB_BID_BUDGET.Equals(string.Empty))
                strMessage += "<P> 底價金額欄位不得為空白 </p>";
            if (strOVC_VEN_TITLE.Equals(string.Empty))
                strMessage += "<P> 請選擇 得標商 </p>";

            if (!strONB_PUR_BUDGET.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_PUR_BUDGET, out n))
                {

                }
                else
                {
                    strMessage += "<P> 預算金額請輸入數字 </p>";
                }
            }
            if (!strONB_BID_RESULT.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_BID_RESULT, out n))
                {

                }
                else
                {
                    strMessage += "<P> 底價金額請輸入數字 </p>";
                }
            }
            if (!strONB_BID_BUDGET.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_BID_BUDGET, out n))
                {

                }
                else
                {
                    strMessage += "<P> 底價金額請輸入數字 </p>";
                }
            }
            #endregion


            if (strMessage.Equals(string.Empty))
            {
                string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
                string strOVC_DOPEN = ViewState["OVC_DOPEN"].ToString();
                string strgroup = ViewState["group"].ToString();
                short onbgroup = Convert.ToInt16(strgroup);

                var queryRESULT =
                        (from result in mpms.TBMBID_RESULT
                         where result.OVC_PURCH.Equals(strOVC_PURCH) && result.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                         && result.OVC_DOPEN.Equals(strOVC_DOPEN) && result.ONB_GROUP == onbgroup
                         select result).FirstOrDefault();
                if (queryRESULT != null)
                {
                    if (PanelDBID.Visible == true)
                    {
                        queryRESULT.OVC_DBID = txtOVC_DBID.Text;
                    }
                    queryRESULT.OVC_DESC = txtOVC_DESC.Text;
                    queryRESULT.OVC_MEETING = txtOVC_MEETING.Text;
                    //queryRESULT.OVC_DRAFT = txtOVC_DRAFT.Text;
                    queryRESULT.OVC_DRAFT = GetChecked();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), queryRESULT.GetType().Name.ToString(), this, "修改");
                }
                else
                {
                    TBMBID_RESULT tbmBID_RESULT = new TBMBID_RESULT();
                    tbmBID_RESULT.OVC_PURCH = strOVC_PURCH;
                    tbmBID_RESULT.OVC_PURCH_5 = strOVC_PURCH_5;
                    tbmBID_RESULT.OVC_DOPEN = strOVC_DOPEN;
                    tbmBID_RESULT.ONB_GROUP = onbgroup;
                    if (PanelDBID.Visible == true)
                    {
                        tbmBID_RESULT.OVC_DBID = txtOVC_DBID.Text;
                    }
                    tbmBID_RESULT.OVC_DESC = txtOVC_DESC.Text;
                    tbmBID_RESULT.OVC_MEETING = txtOVC_MEETING.Text;
                    //tbmBID_RESULT.OVC_DRAFT = txtOVC_DRAFT.Text;
                    tbmBID_RESULT.OVC_DRAFT = GetChecked();
                    mpms.TBMBID_RESULT.Add(tbmBID_RESULT);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmBID_RESULT.GetType().Name.ToString(), this, "新增");
                }

                int onbpurbudget = Convert.ToInt32(strONB_PUR_BUDGET);
                int onbbidresult = Convert.ToInt32(strONB_BID_RESULT);
                int onbbidbudget = Convert.ToInt32(strONB_BID_BUDGET);

                var query1301 = mpms.TBM1301.Where(tb => tb.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
                if (query1301 != null)
                {
                    query1301.ONB_PUR_BUDGET = onbpurbudget;
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), query1301.GetType().Name.ToString(), this, "修改");
                }

                var query1303 =
                    (from tbm1303 in mpms.TBM1303
                     where tbm1303.OVC_PURCH.Equals(strOVC_PURCH) && tbm1303.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                     && tbm1303.OVC_DOPEN.Equals(strOVC_DOPEN) && tbm1303.ONB_GROUP == onbgroup
                     select tbm1303).FirstOrDefault();
                if (query1303 != null)
                {
                    query1303.ONB_BID_RESULT = onbbidresult;
                    query1303.ONB_BID_BUDGET = onbbidbudget;
                    query1303.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), query1303.GetType().Name.ToString(), this, "修改");
                }

                var query1302 =
                    (from tbm1302 in mpms.TBM1302
                     where tbm1302.OVC_PURCH == strOVC_PURCH
                     where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
                     where tbm1302.ONB_GROUP == onbgroup
                     select tbm1302).FirstOrDefault();
                if (query1302 != null)
                {
                    query1302.OVC_VEN_TITLE = txtOVC_VEN_TITLE.Text;
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), query1302.GetType().Name.ToString(), this, "修改");
                }


                var querystaa =
                        (from tsta in mpms.TBMSTATUS
                         where tsta.OVC_PURCH == strOVC_PURCH
                         where tsta.OVC_PURCH_5 == strOVC_PURCH_5
                         where tsta.OVC_STATUS == "27"
                         select tsta).FirstOrDefault();
                if (querystaa == null && !txtOVC_DAPPROVE.Text.Equals(string.Empty))
                {
                    TBMSTATUS sta = new TBMSTATUS();
                    sta.OVC_PURCH = strOVC_PURCH;
                    sta.OVC_PURCH_5 = strOVC_PURCH_5;
                    sta.ONB_TIMES = 1;
                    sta.OVC_DO_NAME = ViewState["OVC_DO_NAME"].ToString();
                    sta.OVC_STATUS = "27";
                    sta.OVC_DBEGIN = DateTime.Now.ToString("yyyy-MM-dd");
                    sta.ONB_GROUP = onbgroup;
                    sta.OVC_STATUS_SN = Guid.NewGuid();
                    mpms.TBMSTATUS.Add(sta);
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), sta.GetType().Name.ToString(), this, "新增");

                    var querysta =
                        (from tsta in mpms.TBMSTATUS
                         where tsta.OVC_PURCH == strOVC_PURCH
                         where tsta.OVC_PURCH_5 == strOVC_PURCH_5
                         where tsta.OVC_STATUS == "26"
                         select tsta).FirstOrDefault();
                    querysta.OVC_DEND = DateTime.Now.ToString("yyyy-MM-dd");
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), querysta.GetType().Name.ToString(), this, "修改");
                }
                mpms.SaveChanges();
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "購案編號" + strOVC_PURCH + "更新成功!");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }

        protected void btnOVC_RESULT_REASON_Click(object sender, EventArgs e)
        {
            string strONB_GROUP = ViewState["group"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strType = "awardofbid";

            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH));
            string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH_5));
            string key3 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strONB_GROUP));
            string key4 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strType));
            string key5 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(numONB_TIMES.ToString()));
            string send_url = "MPMS_D32.aspx?OVC_PURCH=" + key + "&OVC_PURCH_5=" + key2 + "&ONB_GROUP=" + key3 + "&TYPE=" + key4 + "&ONB_TIMES=" + key5;

            Response.Redirect(send_url);
        }

        protected void btnTBMRESULT_ANNOUNCE_NONE_Click(object sender, EventArgs e)
        {
            string strONB_GROUP = ViewState["group"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strOVC_PURCH_6 = lblOVC_PURCH_6.Text;

            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH));
            string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH_5));
            string key3 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strONB_GROUP));
            string key4 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH_6));
            string key5 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_DOPEN));
            string key6 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(numONB_TIMES.ToString()));

            string send_url = "MPMS_D34.aspx?OVC_PURCH=" + key + "&OVC_PURCH_5=" + key2 + "&ONB_GROUP=" + key3 + "&OVC_PURCH_6=" + key4
                + "&OVC_DOPEN=" + key5 + "&ONB_TIMES=" + key6;

            Response.Redirect(send_url);
        }

        protected void btnTBMANNOUNCE_VENDOR_Click(object sender, EventArgs e)
        {
            string strONB_GROUP = ViewState["group"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strOVC_PURCH_6 = lblOVC_PURCH_6.Text;

            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH));
            string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH_5));
            string key3 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strONB_GROUP));
            string key4 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH_6));
            string key5 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_DOPEN));
            string key6 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(numONB_TIMES.ToString()));
            string send_url = "MPMS_D1C.aspx?OVC_PURCH=" + key + "&OVC_PURCH_5=" + key2 + "&ONB_GROUP=" + key3 + "&OVC_PURCH_6=" + key4 
                + "&OVC_DOPEN=" + key5 + "&ONB_TIMES=" + key6;

            Response.Redirect(send_url);
        }

        protected void btnReRead_Click(object sender, EventArgs e)
        {
            DataImport();
        }

        protected void lbtnToWord_D1B_Click(object sender, EventArgs e)
        {
            //點擊LinkButton 開標結果報告表預覽列印
            string filepath = OutputWord_D1B();
            string fileName = "16." + strOVC_PURCH + "開標結果報告表.docx";
            FileInfo fileInfo = new FileInfo(filepath);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            System.IO.File.Delete(filepath);
            Response.End();
        }
        protected void lbtnToWord_D1B_odt_Click(object sender, EventArgs e)
        {
            string filepath = OutputWord_D1B();
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/Word_D1B_odt.odt";
            string FileName = "16." + strOVC_PURCH + "開標結果報告表.odt";
            FCommon.WordToOdt(this, filepath, filetemp, FileName);
        }

        #endregion


        #region 輸出Word檔

        private string OutputWord_D1B()
        {
            string strONB_TIMES = lblONB_TIMES.Text;
            var targetPath = Server.MapPath("~/WordPDFprint/");
            string fileName = "16." + strOVC_PURCH + "開標結果報告表.docx";
            File.Copy(targetPath + "D1B-開標結果報告表.docx", targetPath + fileName);
            var valuesToFill = new TemplateEngine.Docx.Content();

            valuesToFill.Fields.Add(new FieldContent("ONB_TIMES", strONB_TIMES));
            valuesToFill.Fields.Add(new FieldContent("OVC_DOPEN", strOVC_DOPEN == "" ? "" : GetTaiwanDate(strOVC_DOPEN)));

            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_NSECTION", tbm1301.OVC_PUR_NSECTION == null ? "" : tbm1301.OVC_PUR_NSECTION));
                valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", strOVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IPURCH", tbm1301.OVC_PUR_IPURCH == null ? "" : tbm1301.OVC_PUR_IPURCH));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_CURRENT", tbm1301.OVC_PUR_CURRENT == null ? "" : GetTbm1407Desc("B0", tbm1301.OVC_PUR_CURRENT)));
                int onbbudgetbuy = 0;
                if (tbm1301.ONB_PUR_BUDGET != null)
                    onbbudgetbuy = Convert.ToInt32(tbm1301.ONB_PUR_BUDGET);//採購金額
                if (tbm1301.ONB_RESERVE_AMOUNT != null && tbm1301.ONB_PUR_BUDGET != null)//採購金額計算
                    onbbudgetbuy = Convert.ToInt32(tbm1301.ONB_PUR_BUDGET) + Convert.ToInt32(tbm1301.ONB_RESERVE_AMOUNT);
                valuesToFill.Fields.Add(new FieldContent("ONB_PUR_BUDGET", onbbudgetbuy.ToString()));
            }
            TBM1303 tbm1303 = mpms.TBM1303.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5 == strOVC_PURCH_5
                && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP).FirstOrDefault();
            if (tbm1303 != null)
            {
                string strOVC_REMAIN_CURRENT = "";
                valuesToFill.Fields.Add(new FieldContent("OVC_OPEN_HOUR", tbm1303.OVC_OPEN_HOUR == null ? "" : tbm1303.OVC_OPEN_HOUR));
                valuesToFill.Fields.Add(new FieldContent("ONB_BID_RESULT", tbm1303.ONB_BID_RESULT == null ? "" : tbm1303.ONB_BID_RESULT.ToString()));
                valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_CURRENT", tbm1303.ONB_BID_RESULT == null ? "" : GetTbm1407Desc("B0", tbm1303.OVC_RESULT_CURRENT)));
                valuesToFill.Fields.Add(new FieldContent("ONB_BID_RESULT", tbm1303.ONB_BID_RESULT == null ? "" : tbm1303.ONB_BID_RESULT.ToString()));
                valuesToFill.Fields.Add(new FieldContent("OVC_CURRENT", tbm1303.OVC_CURRENT == null ? "" : GetTbm1407Desc("B0", tbm1303.OVC_CURRENT)));
                valuesToFill.Fields.Add(new FieldContent("ONB_BID_BUDGET", tbm1303.ONB_BID_BUDGET == null ? "" : tbm1303.ONB_BID_BUDGET.ToString()));
                if (tbm1303.OVC_BID_CURRENT != null)
                    strOVC_REMAIN_CURRENT = GetTbm1407Desc("B0", tbm1303.OVC_BID_CURRENT);
                else if (tbm1303.OVC_RESULT_CURRENT != null)
                    strOVC_REMAIN_CURRENT = GetTbm1407Desc("B0", tbm1303.OVC_RESULT_CURRENT);
                valuesToFill.Fields.Add(new FieldContent("OVC_REMAIN_CURRENT", strOVC_REMAIN_CURRENT));
                if (tbm1303.ONB_BID_BUDGET != null && tbm1303.ONB_BID_RESULT != null)
                {
                    int onbremainbudget = Convert.ToInt32(tbm1303.ONB_BID_BUDGET) - Convert.ToInt32(tbm1303.ONB_BID_RESULT);    //標餘款
                    valuesToFill.Fields.Add(new FieldContent("ONB_REMAIN_BUDGET", onbremainbudget.ToString()));
                }
                if (tbm1303.OVC_RESULT != null)
                {
                    switch (tbm1303.OVC_RESULT)
                    {
                        case "0":   //一、決標
                            valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_0", "■"));
                            break;
                        case "1":   //流標
                            valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_1or2", "■"));
                            if (tbm1303.ONB_BID_VENDOR_LAW != null)
                                valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_REASON_1", "■"));
                            break;
                        case "2":   //廢標
                            valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_1or2", "■"));
                            if (tbm1303.OVC_RESULT_REASON == "最低報價超底價")
                                valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_REASON_2", "■"));
                            break;
                        case "3":   //保留開標結果
                            valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_3", "■"));
                            break;

                    }
                }
            }

            TBMBID_RESULT tbmBID_RESULT = mpms.TBMBID_RESULT.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                && tb.ONB_GROUP == numONB_GROUP).FirstOrDefault();
            if (tbmBID_RESULT != null)
            {
                if (tbmBID_RESULT.OVC_DRAFT != null)
                {
                    string[] selectValue = tbmBID_RESULT.OVC_DRAFT.Split(',');
                    string strOVC_DRAFT = "";
                    for (int intItem = 1; intItem <= selectValue.Length; intItem++)
                    {
                        strOVC_DRAFT += strOVC_DRAFT == "" ? intItem + ". " + selectValue[intItem - 1] : "\r\n" + intItem + ". " + selectValue[intItem - 1];
                    }
                    valuesToFill.Fields.Add(new FieldContent("OVC_DRAFT", strOVC_DRAFT));
                }
            }

            var query1302 =
                from tbm1302 in mpms.TBM1302
                where tbm1302.OVC_PURCH.Equals(strOVC_PURCH) && tbm1302.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                        && tbm1302.OVC_DOPEN.Equals(strOVC_DOPEN) && tbm1302.ONB_GROUP == numONB_GROUP
                select new
                {
                    OVC_PURCH = tbm1302.OVC_PURCH,
                    OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                    OVC_DOPEN = tbm1302.OVC_DOPEN,
                    ONB_GROUP = tbm1302.ONB_GROUP,
                    OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE,
                };

            DataTable dt1302 = CommonStatic.LinqQueryToDataTable(query1302);
            if (dt1302.Rows.Count != 0)
            {
                string strOVC_VEN_TITLE = "";
                foreach (DataRow rows in dt1302.Rows)
                {
                    strOVC_VEN_TITLE += strOVC_VEN_TITLE == "" ? rows["OVC_VEN_TITLE"].ToString() : "," + rows["OVC_VEN_TITLE"].ToString();
                }
                valuesToFill.Fields.Add(new FieldContent("OVC_VENDORS_NAME", strOVC_VEN_TITLE));
            }


            using (var outputDocument = new TemplateProcessor(targetPath + fileName).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }

            string filepath = targetPath + fileName;
            return filepath;
        }

        #endregion

        
    }
}