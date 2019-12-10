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
    public partial class MPMS_D35 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        public string sOVC_PURCH, sOVC_PURCH_5;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_PURCH_5"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    if (!IsPostBack && IsOVC_DO_NAME())
                    {
                        string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
                        string strOVC_PURCH_5_url = Request.QueryString["OVC_PURCH_5"];
                        string strOVC_PURCH_6_url = Request.QueryString["OVC_PURCH_6"];
                        string strONB_GROUP_url = Request.QueryString["ONB_GROUP"];
                        string strOVC_DOPEN_url = Request.QueryString["OVC_DOPEN"];
                        string strONB_TIMES_url = Request.QueryString["ONB_TIMES"];
                        string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
                        string strOVC_PURCH_5 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_5_url));
                        string strOVC_PURCH_6 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_6_url));
                        string strOVC_DOPEN = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_DOPEN_url));
                        string strONB_GROUP = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strONB_GROUP_url));
                        string strONB_TIMES = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strONB_TIMES_url));
                        string strTYPE = Request.QueryString["TYPE"];
                        short onbgroup = Convert.ToInt16(strONB_GROUP);

                        ViewState["strOVC_PURCH"] = strOVC_PURCH;
                        ViewState["strOVC_PURCH_5"] = strOVC_PURCH_5;
                        ViewState["strOVC_PURCH_6"] = strOVC_PURCH_6;
                        ViewState["strONB_GROUP"] = strONB_GROUP;
                        ViewState["strOVC_DOPEN"] = strOVC_DOPEN;
                        ViewState["strONB_TIMES"] = strONB_TIMES;
                        ViewState["TYPE"] = strTYPE;

                        string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH));
                        string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH_5));
                        sOVC_PURCH = key;
                        sOVC_PURCH_5 = key2;
                        var querynone =
                        (from tbmnone in mpms.TBMRESULT_ANNOUNCE_NONE
                         where tbmnone.OVC_PURCH == strOVC_PURCH
                         where tbmnone.OVC_PURCH_5 == strOVC_PURCH_5
                         select tbmnone).FirstOrDefault();
                        LinkButton1.Visible = false;
                        dataImport(strOVC_PURCH, strOVC_PURCH_5);

                        if (querynone != null && strTYPE == "Modify")
                        {
                            dataModify();
                            LinkButton1.Visible = true;
                        }

                        FCommon.Controls_Attributes("readonly", "true", txtDSEND, txtOVC_DAPPROVE, txtOVC_DANNOUNCE_LAST);
                    }
                }
            }
        }

        private bool IsOVC_DO_NAME()
        {
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strUserName, strDept = "";
            string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
            string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
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

        private void dataImport(string strOVC_PURCH, string strOVC_PURCH_5)
        {
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            lblOVC_PURCH.Text = strOVC_PURCH +query1301.OVC_PUR_AGENCY+strOVC_PURCH_5;
            lblOVC_PUR_IPURCH.Text = query1301.OVC_PUR_IPURCH;
            string strpurass = query1301.OVC_PUR_ASS_VEN_CODE;
            var query1407C7 =
                (from tbm1407 in mpms.TBM1407
                 where tbm1407.OVC_PHR_CATE == "C7"
                 where tbm1407.OVC_PHR_ID == strpurass
                 select tbm1407).FirstOrDefault();
            lblOVC_PUR_ASS_VEN_CODE.Text = query1407C7 == null? "" : query1407C7.OVC_PHR_DESC;
            var query1303 =
                (from tbm1303 in mpms.TBM1303
                 where tbm1303.OVC_PURCH == strOVC_PURCH
                 where tbm1303.OVC_PURCH_5 == strOVC_PURCH_5
                 select tbm1303).FirstOrDefault();
            if(query1303 != null)
                lblOVC_DOPEN.Text = ToTaiwanCalendar(query1303.OVC_DOPEN) + query1303.OVC_OPEN_HOUR + "時" + query1303.OVC_OPEN_MIN + "分";
        }
        public string ToTaiwanCalendar(string strDate)
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strType = ViewState["TYPE"].ToString();
            if (strType == "Modify")
            {
                Modify();
            }
            else
                Newdata();
        }
        private void Newdata()
        {
            string strDSEND = txtDSEND.Text;
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strONB_BID_VDNDORS = txtONB_BID_VENDORS.Text;
            string strOVC_DANNOUNCE_LAST = txtOVC_DANNOUNCE_LAST.Text;
            string strOVC_RESULT_REASON = rdoOVC_RESULT_REASON.SelectedItem.Text;
            string strOVC_CONTINUE = rdoOVC_CONTINUE.SelectedValue.ToString();
            string strOVC_MEMO = txtOVC_MEMO.Text;
            string strOVC_DAPPROVE = txtOVC_DAPPROVE.Text;
            string strMessage = "";

            if (!strONB_BID_VDNDORS.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_BID_VDNDORS, out n))
                {
                }
                else
                {
                    strMessage += "<P> 投標廠商家數請輸入數字 </p>";
                }
            }
            else
                strMessage += "<P> 投標廠商家數欄位不得為空白 </p>";


            if (strMessage.Equals(string.Empty))
            {
                var query1303 =
                 (from tbm1303 in mpms.TBM1303
                  where tbm1303.OVC_PURCH == strOVC_PURCH
                  where tbm1303.OVC_PURCH_5 == strOVC_PURCH_5
                  select tbm1303).FirstOrDefault();

                TBMRESULT_ANNOUNCE_NONE announce = new TBMRESULT_ANNOUNCE_NONE();

                announce.OVC_PURCH = strOVC_PURCH;
                announce.OVC_PURCH_5 = strOVC_PURCH_5;
                announce.OVC_DOPEN = query1303.OVC_DOPEN;
                announce.OVC_OPEN_HOUR = query1303.OVC_OPEN_HOUR;
                announce.OVC_OPEN_MIN = query1303.OVC_OPEN_MIN;
                short onbtime = Convert.ToInt16(query1303.ONB_TIMES);
                announce.ONB_TIMES = onbtime;
                short onbbidvendors = Convert.ToInt16(strONB_BID_VDNDORS);
                announce.ONB_BID_VENDORS = onbbidvendors;
                announce.OVC_DANNOUNCE_LAST = strOVC_DANNOUNCE_LAST;
                announce.OVC_RESULT_REASON = strOVC_RESULT_REASON;
                announce.OVC_CONTINUE = strOVC_CONTINUE;
                announce.OVC_MEMO = strOVC_MEMO;
                announce.OVC_DAPPROVE = strOVC_DAPPROVE;
                announce.OVC_DSEND = strDSEND;

                mpms.TBMRESULT_ANNOUNCE_NONE.Add(announce);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), announce.GetType().Name.ToString(), this, "新增");
                LinkButton1.Visible = true;
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "契約編號" + strOVC_PURCH + strOVC_PURCH_5 + "新增成功!!");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        private void dataModify()
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
           
            var queryannounce =
                (from tbmannounce in mpms.TBMRESULT_ANNOUNCE_NONE
                 where tbmannounce.OVC_PURCH == strOVC_PURCH
                 where tbmannounce.OVC_PURCH_5 == strOVC_PURCH_5
                 select tbmannounce).FirstOrDefault();
            if (queryannounce != null)
            {
                txtDSEND.Text = queryannounce.OVC_DSEND;
                lblDSEND.Text = queryannounce.OVC_DSEND;
                txtONB_BID_VENDORS.Text = queryannounce.ONB_BID_VENDORS.ToString();
                txtOVC_DANNOUNCE_LAST.Text = queryannounce.OVC_DANNOUNCE_LAST;
                rdoOVC_RESULT_REASON.SelectedItem.Text = queryannounce.OVC_RESULT_REASON;
                rdoOVC_CONTINUE.SelectedValue = queryannounce.OVC_CONTINUE;
                txtOVC_MEMO.Text = queryannounce.OVC_MEMO;
                txtOVC_DAPPROVE.Text = queryannounce.OVC_DAPPROVE;
            }
            ViewState["TYPE"] = "Modify";
        }

        protected void btnReturnE_Click(object sender, EventArgs e)
        {
            //Response.Redirect("MPMS_D1B.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString());
            string send_url = "MPMS_D18_7.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_DOPEN=" + ViewState["strOVC_DOPEN"].ToString() +
                                    "&ONB_TIMES=" + ViewState["strONB_TIMES"] + "&ONB_GROUP=" + ViewState["strONB_GROUP"];
            Response.Redirect(send_url);
        }

        protected void btnReturnS_Click(object sender, EventArgs e)
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();

            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH"].ToString()));
            string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH_5"].ToString()));
            string key3 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strONB_GROUP"].ToString()));
            string key4 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH_6"].ToString()));
            string key5 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_DOPEN"].ToString()));
            string key6 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strONB_TIMES"].ToString()));

            string send_url = "MPMS_D34.aspx?OVC_PURCH=" + key + "&OVC_PURCH_5=" + key2 + "&ONB_GROUP=" + key3 + "&OVC_PURCH_6=" + key4
                + "&OVC_DOPEN=" + key5 + "&ONB_TIMES=" + key6;
            Response.Redirect(send_url);
        }

        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_D14.aspx");
        }

        private void Modify()
        {
            string strDSEND = txtDSEND.Text;
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strONB_BID_VDNDORS = txtONB_BID_VENDORS.Text;
            string strOVC_DANNOUNCE_LAST = txtOVC_DANNOUNCE_LAST.Text;
            string strOVC_RESULT_REASON = rdoOVC_RESULT_REASON.SelectedItem.Text;
            string strOVC_CONTINUE = rdoOVC_CONTINUE.SelectedValue.ToString();
            string strOVC_MEMO = txtOVC_MEMO.Text;
            string strOVC_DAPPROVE = txtOVC_DAPPROVE.Text;
            string strMessage = "";

            if (!strONB_BID_VDNDORS.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_BID_VDNDORS, out n))
                {
                }
                else
                {
                    strMessage += "<P> 投標廠商家數請輸入數字 </p>";
                }
            }
            else
                strMessage += "<P> 投標廠商家數欄位不得為空白 </p>";

            if (strOVC_DANNOUNCE_LAST.Equals(string.Empty))
                strMessage += "<P> 最近一次公告日期欄位不得為空白 </p>";

            if (strMessage.Equals(string.Empty))
            {
                var queryannounce =
                (from tbmannounce in mpms.TBMRESULT_ANNOUNCE_NONE
                 where tbmannounce.OVC_PURCH == strOVC_PURCH
                 where tbmannounce.OVC_PURCH_5 == strOVC_PURCH_5
                 select tbmannounce).FirstOrDefault();

                short onbbidvendors = Convert.ToInt16(strONB_BID_VDNDORS);
                queryannounce.ONB_BID_VENDORS = onbbidvendors;
                queryannounce.OVC_DANNOUNCE_LAST = strOVC_DANNOUNCE_LAST;
                queryannounce.OVC_RESULT_REASON = strOVC_RESULT_REASON;
                queryannounce.OVC_CONTINUE = strOVC_CONTINUE;
                queryannounce.OVC_MEMO = strOVC_MEMO;
                queryannounce.OVC_DAPPROVE = strOVC_DAPPROVE;
                queryannounce.OVC_DSEND = strDSEND;

                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), queryannounce.GetType().Name.ToString(), this, "修改");

                FCommon.AlertShow(PnMessage, "success", "系統訊息", "契約編號" + strOVC_PURCH + strOVC_PURCH_5 + "修改成功!!");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);

        }
    }
}