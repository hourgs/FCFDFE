using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D1C : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        public string sOVC_PURCH, sONB_GROUP;
        
        protected void GV_TBMRESULT_ANNOUNCE_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void GV_TBMRESULT_ANNOUNCE_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBMRESULT_ANNOUNCE.DataKeys[gvrIndex].Value.ToString();
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strOVC_PURCH_6 = ViewState["strOVC_PURCH_6"].ToString();
            string strONB_GROUP = ViewState["strONB_GROUP"].ToString();
            int onbgroup = Convert.ToInt32(strONB_GROUP);
            switch (e.CommandName)
            {
                case "btnWork":
                    Panel1.Visible = false;
                    Panel2.Visible = true;
                    dataEdit();
                    string strkey = strOVC_PURCH + strOVC_PURCH_5;
                    string strkey3 = strONB_GROUP + strOVC_PURCH_6;
                    string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strkey));
                    string key3 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strkey3));

                    sOVC_PURCH = key;
                    sONB_GROUP = key3;
                    break;
                case "btnDel":
                    var queryannounce =
                     (from tbmannounce in mpms.TBMRESULT_ANNOUNCE
                      where tbmannounce.OVC_PURCH == strOVC_PURCH
                      where tbmannounce.OVC_PURCH_5 == strOVC_PURCH_5
                      where tbmannounce.OVC_PURCH_6 == strOVC_PURCH_6
                      select tbmannounce).FirstOrDefault();
                    mpms.Entry(queryannounce).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), queryannounce.GetType().Name.ToString(), this, "刪除");
                    FCommon.AlertShow(Panel3, "success", "系統訊息", "刪除成功");
                    dataImport();
                    break;
                default:
                    break;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_PURCH_5"] == null || Request.QueryString["OVC_PURCH_6"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    if (!IsPostBack && IsOVC_DO_NAME())
                    {
                        string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
                        string strOVC_PURCH_5_url = Request.QueryString["OVC_PURCH_5"];
                        string strONB_GROUP_url = Request.QueryString["ONB_GROUP"];
                        string strOVC_DOPEN_url = Request.QueryString["OVC_DOPEN"];
                        string strONB_TIMES_url = Request.QueryString["ONB_TIMES"];
                        string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
                        string strOVC_PURCH_5 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_5_url));
                        string strOVC_DOPEN = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_DOPEN_url));
                        string strONB_GROUP = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strONB_GROUP_url));
                        string strONB_TIMES = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strONB_TIMES_url));
                        short onbgroup = Convert.ToInt16(strONB_GROUP);

                        ViewState["strOVC_PURCH"] = strOVC_PURCH;
                        ViewState["strOVC_PURCH_5"] = strOVC_PURCH_5;
                        ViewState["strONB_GROUP"] = strONB_GROUP;
                        ViewState["strOVC_DOPEN"] = strOVC_DOPEN;
                        ViewState["strONB_TIMES"] = strONB_TIMES;

                        Panel1.Visible = true;
                        Panel2.Visible = false;

                        dataImport();
                        FCommon.Controls_Attributes("readonly", "true", txtDSEND, txtOVC_DAPPROVE);
                        var query1407 =
                            (from tbm1407 in mpms.TBM1407
                             where tbm1407.OVC_PHR_CATE == "U0"
                             select tbm1407).ToList();
                        DataTable dt = CommonStatic.ListToDataTable(query1407);
                        FCommon.list_dataImport(drpOVC_VEN_COUNTRY, dt, "OVC_PHR_DESC", "OVC_PHR_ID", true);
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

        private void dataImport()
        {
            string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
            string strOVC_PURCH_5_url = Request.QueryString["OVC_PURCH_5"];
            string strOVC_PURCH_6_url = Request.QueryString["OVC_PURCH_6"];
            string strONB_GROUP_url = Request.QueryString["ONB_GROUP"] == null ? "" : Request.QueryString["ONB_GROUP"];
            string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
            string strOVC_PURCH_5 = strOVC_PURCH_5_url == "" ? "" : System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_5_url));
            string strOVC_PURCH_6 = strOVC_PURCH_6_url == "" ? "" : System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_6_url));
            string strONB_GROUP = strONB_GROUP_url == "" ? "" : System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strONB_GROUP_url));
            ViewState["strOVC_PURCH"] = strOVC_PURCH;
            ViewState["strOVC_PURCH_5"] = strOVC_PURCH_5;
            ViewState["strOVC_PURCH_6"] = strOVC_PURCH_6;
            ViewState["strONB_GROUP"] = strONB_GROUP;
            int onbgroup = Convert.ToInt32(strONB_GROUP);

            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            lblPURCH.Text = query1301.OVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
            DataTable dt = new DataTable();
            var query =
                from tbmannounce in mpms.TBMRESULT_ANNOUNCE
                where tbmannounce.OVC_PURCH == strOVC_PURCH
                where tbmannounce.OVC_PURCH_5 == strOVC_PURCH_5
                where tbmannounce.OVC_PURCH_6 == strOVC_PURCH_6
                join tbmresult in mpms.TBMBID_RESULT on tbmannounce.OVC_PURCH equals tbmresult.OVC_PURCH
                where tbmresult.OVC_PURCH_5 == strOVC_PURCH_5
                where tbmresult.ONB_GROUP == onbgroup
                join tbm1301 in mpms.TBM1301 on tbmannounce.OVC_PURCH equals tbm1301.OVC_PURCH
                select new
                {
                    OVC_PURCH = tbmannounce.OVC_PURCH,
                    OVC_PURCH_5 = tbmannounce.OVC_PURCH_5,
                    OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                    OVC_PURCH_6 = tbmannounce.OVC_PURCH_6,
                    ONB_GROUP = tbmresult.ONB_GROUP,
                    OVC_DBID = tbmannounce.OVC_DBID,
                    OVC_DSEND = tbmannounce.OVC_DSEND,
                    OVC_NAME = tbmannounce.OVC_NAME
                };
            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["Status"] = FCommon.GridView_dataImport(GV_TBMRESULT_ANNOUNCE, dt);
        }

        protected void rdoOVC_EMPLOYEE_OVER_Y_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOVC_EMPLOYEE_OVER_Y.Checked == true)
                rdoOVC_EMPLOYEE_OVER_N.Checked = false;
            if (rdoOVC_EMPLOYEE_OVER_Y.Checked == false)
                rdoOVC_EMPLOYEE_OVER_N.Checked = true;

        }

        protected void rdoOVC_EMPLOYEE_OVER_N_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOVC_EMPLOYEE_OVER_N.Checked == true)
                rdoOVC_EMPLOYEE_OVER_Y.Checked = false;
            if (rdoOVC_EMPLOYEE_OVER_N.Checked == false)
                rdoOVC_EMPLOYEE_OVER_Y.Checked = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strOVC_PURCH_6 = ViewState["strOVC_PURCH_6"].ToString();
            string strONB_GROUP = ViewState["strONB_GROUP"].ToString();
            int onbgroup = Convert.ToInt32(strONB_GROUP);

            string strDSEND = txtDSEND.Text;
            string strOVC_PART_IPURCH = txtOVC_PART_IPURCH.Text;
            string strOVC_PUR_ASS_VEN_CODE = rdoOVC_PUR_ASS_VEN_CODE.SelectedValue.ToString();
            string strOVC_STATUS = rOVC_STATUS.SelectedValue.ToString();
            string strONB_PUR_BUDGET = txtONB_BUDGET.Text;
            string strONB_BID_BUDGET = txtONB_BID_BUDGET.Text;
            string strONB_BID_RESULT = txtONB_BID_RESULT.Text;
            string strOVC_NONE_VENDORS = txtOVC_NONE_VENDORS.Text;

            string strOVC_VEN_ADDRESS = txtOVC_VEN_ADDRESS.Text;
            string strOVC_VEN_TEL = txtOVC_VEN_TEL.Text;
            string strOVER = "";
            if (rdoOVC_EMPLOYEE_OVER_Y.Checked == true)
            {
                strOVER = "Y";
            }
            else
            {
                strOVER = "N";
            }
            string strONB_EMPLOYEES = ONB_EMPLOYEES.Text;
            string strONB_EMPLOYEES_SPECIAL = txtONB_EMPLOYEES_SPECIAL.Text;
            string strONB_EMPLOYEES_ABORIGINAL = txtONB_EMPLOYEES_ABORIGINAL.Text;
            string strVEN_KIND = rdoVEN_KIND.SelectedValue.ToString();
            string strOthers = txtOthers.Text;
            string strONB_BID_RESULT_MERG = txtONB_BID_RESULT_MERG.Text;
            string strOVC_MIDDLE_SMALL = rdoOVC_MIDDLE_SMALL.SelectedValue.ToString();
            string strONB_BID_RESULT_1 = txtONB_BID_RESULT_1.Text;
            string strONB_BID_JOB = txtONB_BID_JOB.Text;
            string strOVC_VEN_103_2 = txtOVC_VEN_103_2.Text;
            string strOVC_DESC = txtOVC_DESC.Text;
            string strOVC_DAPPROVE = txtOVC_DAPPROVE.Text;
            string strOVC_VEN_COUNTRY = drpOVC_VEN_COUNTRY.SelectedItem.ToString();
            string strMessage = "";

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
            else
                strMessage += "<P> 預算金額欄位不得為空白 </p>";
            if (!strONB_BID_BUDGET.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_BID_BUDGET, out n))
                {

                }
                else
                {
                    strMessage += "<P> 底價金額或評審委員會建議金額請輸入數字 </p>";
                }
            }
            else
                strMessage += "<P> 底價金額或評審委員會建議金額欄位不得為空白 </p>";
            if (!strONB_BID_RESULT.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_BID_RESULT, out n))
                {

                }
                else
                {
                    strMessage += "<P> 總決標金額請輸入數字 </p>";
                }
            }
            else
                strMessage += "<P> 總決標金額欄位不得為空白 </p>";

            if (strOVC_NONE_VENDORS.Equals(string.Empty))
                strMessage += "<P> 未得標廠商代碼及名稱欄位不得為空白 </p>";

            if (strOVC_VEN_ADDRESS.Equals(string.Empty))
                strMessage += "<P> 廠商地址欄位不得為空白 </p>";
            if (strOVER == "Y")
            {
                if (!strONB_EMPLOYEES.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_EMPLOYEES, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<P> 雇用員工總人數請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<P> 雇用員工總人數欄位不得為空白 </p>";

                if (!strONB_EMPLOYEES_SPECIAL.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_EMPLOYEES_SPECIAL, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<P> 已僱用身心障礙人士人數請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<P> 已僱用身心障礙人士人數欄位不得為空白 </p>";

                if (!strONB_EMPLOYEES_ABORIGINAL.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_EMPLOYEES_ABORIGINAL, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<P> 已僱用原住民人士人數請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<P> 已僱用原住民人士人數欄位不得為空白 </p>";
            }

            if (strOVC_VEN_ADDRESS.Equals(string.Empty))
                strMessage += "<P> 廠商地址欄位不得為空白 </p>";

            if (strOVC_VEN_COUNTRY == "請選擇")
                strMessage += "<P> 請選擇原產地國別或得標廠商國別 </p>";

            if (!strONB_BID_RESULT_MERG.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_BID_RESULT_MERG, out n))
                {
                }
                else
                {
                    strMessage += "<P> 併列共同供應商個別廠商之決標金額請輸入數字 </p>";
                }
            }
            else
                strMessage += "<P> 併列共同供應商個別廠商之決標金額欄位不得為空白 </p>";

            if (strVEN_KIND == "其他")
            {
                if (strOVC_NONE_VENDORS.Equals(string.Empty))
                    strMessage += "<P> 得標商類別欄位不得為空白 </p>";
            }

            if (!strONB_BID_RESULT_1.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_BID_RESULT_1, out n))
                {
                }
                else
                {
                    strMessage += "<P> 決標金額請輸入數字 </p>";
                }
            }
            else
                strMessage += "<P> 決標金額欄位不得為空白 </p>";

            if (!strONB_BID_JOB.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_BID_JOB, out n))
                {
                }
                else
                {
                    strMessage += "<P> 預估分包予中小企業之金額請輸入數字 </p>";
                }
            }
            else
                strMessage += "<P> 預估分包予中小企業之金額欄位不得為空白 </p>";

            if (strMessage.Equals(string.Empty))
            {
                var queryannounce =
                (from tbmannounce in mpms.TBMRESULT_ANNOUNCE
                 where tbmannounce.OVC_PURCH == strOVC_PURCH
                 where tbmannounce.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmannounce.OVC_PURCH_6 == strOVC_PURCH_6
                 select tbmannounce).FirstOrDefault();
                if (queryannounce != null)
                {
                    queryannounce.OVC_DSEND = strDSEND;
                    queryannounce.OVC_PART_IPURCH = strOVC_PART_IPURCH;
                    queryannounce.OVC_PUR_ASS_VEN_CODE = strOVC_PUR_ASS_VEN_CODE;
                    queryannounce.OVC_STATUS = strOVC_STATUS;
                    int onbpurbudget = Convert.ToInt32(strONB_PUR_BUDGET);
                    queryannounce.ONB_PUR_BUDGET = onbpurbudget;
                    int onbbidbudget = Convert.ToInt32(strONB_BID_BUDGET);
                    queryannounce.ONB_BID_BUDGET = onbbidbudget;
                    int onbbidresult = Convert.ToInt32(strONB_BID_RESULT);
                    queryannounce.ONB_BID_RESULT = onbbidresult;
                    queryannounce.OVC_NONE_VENDORS = strOVC_NONE_VENDORS;

                    var queryvendor =
                                    (from tbmvendor in mpms.TBMANNOUNCE_VENDOR
                                     where tbmvendor.OVC_PURCH == strOVC_PURCH
                                     where tbmvendor.OVC_PURCH_5 == strOVC_PURCH_5
                                     where tbmvendor.OVC_PURCH_6 == strOVC_PURCH_6
                                     where tbmvendor.ONB_GROUP == onbgroup
                                     select tbmvendor).FirstOrDefault();

                    queryvendor.OVC_VEN_ADDRESS = strOVC_VEN_ADDRESS;
                    queryvendor.OVC_VEN_TEL = strOVC_VEN_TEL;
                    queryvendor.OVC_EMPLOYEE_OVER = strOVER;
                    if (strOVER == "Y")
                    {
                        int onbemployees = Convert.ToInt32(strONB_EMPLOYEES);
                        queryvendor.ONB_EMPLOYEES = onbemployees;
                        int onbemployeesspecial = Convert.ToInt32(strONB_EMPLOYEES_SPECIAL);
                        queryvendor.ONB_EMPLOYEES_SPECIAL = onbemployeesspecial;
                        int onbemployeesaboriginal = Convert.ToInt32(strONB_EMPLOYEES_ABORIGINAL);
                        queryvendor.ONB_EMPLOYEES_ABORIGINAL = onbemployeesaboriginal;
                    }
                    if (strVEN_KIND == "其他")
                    {
                        queryvendor.OVC_VEN_KIND = strOthers;
                    }
                    else
                        queryvendor.OVC_VEN_KIND = strVEN_KIND;
                    int onbbidresultmerg = Convert.ToInt32(strONB_BID_RESULT_MERG);
                    queryvendor.ONB_BID_RESULT_MERG = onbbidresultmerg;
                    queryvendor.OVC_MIDDLE_SMALL = strOVC_MIDDLE_SMALL;
                    int onbbidresult1 = Convert.ToInt32(strONB_BID_RESULT_1);
                    queryvendor.ONB_BID_RESULT = onbbidresult1;
                    int onbbidjob = Convert.ToInt32(strONB_BID_JOB);
                    queryvendor.ONB_BID_JOB = onbbidjob;
                    queryvendor.OVC_VEN_103_2 = strOVC_VEN_103_2;
                    queryvendor.OVC_VEN_COUNTRY = strOVC_VEN_COUNTRY;

                    queryannounce.OVC_DESC = strOVC_DESC;
                    queryannounce.OVC_DAPPROVE = strOVC_DAPPROVE;

                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), queryannounce.GetType().Name.ToString(), this, "修改");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "契約編號" + strOVC_PURCH + strOVC_PURCH_5 + strOVC_PURCH_6 + "修改成功!!");
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);


        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            Panel2.Visible = false;
            Panel1.Visible = true;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //Response.Redirect("MPMS_D1B.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString());
            string send_url = "MPMS_D18_7.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_DOPEN=" + ViewState["strOVC_DOPEN"].ToString() +
                                    "&ONB_TIMES=" + ViewState["strONB_TIMES"] + "&ONB_GROUP=" + ViewState["strONB_GROUP"];
            Response.Redirect(send_url);
        }

        protected void btnToMain_Click(object sender, EventArgs e)
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            Response.Redirect("MPMS_D14_1.aspx?OVC_PURCH=" + strOVC_PURCH);
        }

        protected void btnBefore_Click(object sender, EventArgs e)
        {
            //Response.Redirect("MPMS_D1B.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString());
            string send_url = "MPMS_D18_7.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_DOPEN=" + ViewState["strOVC_DOPEN"].ToString() +
                                    "&ONB_TIMES=" + ViewState["strONB_TIMES"] + "&ONB_GROUP=" + ViewState["strONB_GROUP"];
            Response.Redirect(send_url);
        }

        protected void btnMain_Click(object sender, EventArgs e)
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            Response.Redirect("MPMS_D14_1.aspx?OVC_PURCH=" + strOVC_PURCH);
        }

        private void dataEdit()
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strOVC_PURCH_6 = ViewState["strOVC_PURCH_6"].ToString();
            string strONB_GROUP = ViewState["strONB_GROUP"].ToString();
            int onbgroup = Convert.ToInt32(strONB_GROUP);

            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            var queryannounce =
                (from tbmannounce in mpms.TBMRESULT_ANNOUNCE
                 where tbmannounce.OVC_PURCH == strOVC_PURCH
                 where tbmannounce.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmannounce.OVC_PURCH_6 == strOVC_PURCH_6
                 select tbmannounce).FirstOrDefault();
            txtDSEND.Text = DateTime.Now.ToShortDateString();
            lblDSEND.Text = queryannounce.OVC_DSEND + ")";
            lblOVC_PURCH.Text = query1301.OVC_PURCH +query1301.OVC_PUR_AGENCY+ queryannounce.OVC_PURCH_5 + queryannounce.OVC_PURCH_6;
            lblOVC_PUR_IPURCH.Text = query1301.OVC_PUR_IPURCH;
            txtOVC_PART_IPURCH.Text = queryannounce.OVC_PART_IPURCH;//部分決標項目或數量
            rdoOVC_PUR_ASS_VEN_CODE.SelectedValue = queryannounce.OVC_PUR_ASS_VEN_CODE;//招標方式
            rOVC_STATUS.SelectedValue = queryannounce.OVC_STATUS; //執行現況
            lblOVC_DBID.Text = queryannounce.OVC_DBID;
            txtONB_BUDGET.Text = queryannounce.ONB_PUR_BUDGET.ToString();//預算金額
            txtONB_BID_BUDGET.Text = queryannounce.ONB_BID_BUDGET.ToString();//底價
            txtONB_BID_RESULT.Text = queryannounce.ONB_BID_RESULT.ToString();//總決標金額
            lblONB_BID_VENDORS.Text = queryannounce.ONB_BID_VENDORS.ToString();
            lblONB_RESULT_VENDORS.Text = queryannounce.ONB_RESULT_VENDORS.ToString();
            txtOVC_NONE_VENDORS.Text = queryannounce.OVC_NONE_VENDORS;//未得標

            //得標廠商資料
            var queryvendor =
                (from tbmvendor in mpms.TBMANNOUNCE_VENDOR
                 where tbmvendor.OVC_PURCH == strOVC_PURCH
                 where tbmvendor.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmvendor.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmvendor.ONB_GROUP == onbgroup
                 select tbmvendor).FirstOrDefault();
            lblOVC_VEN_TITLE.Text = queryvendor.OVC_VEN_TITLE;
            lblOVC_VEN_CST.Text = queryvendor.OVC_VEN_CST;
            lblOVC_VEN_TITLE_1.Text = queryvendor.OVC_VEN_TITLE;
            txtOVC_VEN_ADDRESS.Text = queryvendor.OVC_VEN_ADDRESS;
            txtOVC_VEN_TEL.Text = queryvendor.OVC_VEN_TEL;
            string strover = queryvendor.OVC_EMPLOYEE_OVER;
            if (strover == "Y")
            {
                rdoOVC_EMPLOYEE_OVER_Y.Checked = true;
                ONB_EMPLOYEES.Text = queryvendor.ONB_EMPLOYEES.ToString();
                txtONB_EMPLOYEES_SPECIAL.Text = queryvendor.ONB_EMPLOYEES_SPECIAL.ToString();
                txtONB_EMPLOYEES_ABORIGINAL.Text = queryvendor.ONB_EMPLOYEES_ABORIGINAL.ToString();
            }
            if (strover == "N")
                rdoOVC_EMPLOYEE_OVER_N.Checked = true;
            string strVEN_KIND = queryvendor.OVC_VEN_KIND;
            if (strVEN_KIND == "營造業" || strVEN_KIND == "技師事務所" || strVEN_KIND == "技師顧問機構" || strVEN_KIND == "建築事務所")
            {
                rdoVEN_KIND.SelectedValue = strVEN_KIND;
            }
            else
            {
                rdoVEN_KIND.SelectedValue = "其他";
                txtOthers.Text = strVEN_KIND;
            }
            txtONB_BID_RESULT_MERG.Text = queryvendor.ONB_BID_RESULT_MERG.ToString();
            rdoOVC_MIDDLE_SMALL.SelectedValue = queryvendor.OVC_MIDDLE_SMALL;
            txtONB_BID_RESULT_1.Text = queryvendor.ONB_BID_RESULT.ToString();
            txtONB_BID_JOB.Text = queryvendor.ONB_BID_JOB.ToString();
            txtOVC_VEN_103_2.Text = queryvendor.OVC_VEN_103_2;
            txtOVC_DESC.Text = queryannounce.OVC_DESC;
            txtOVC_DAPPROVE.Text = queryannounce.OVC_DAPPROVE;

            //原產地國別或得標廠商國別
            string strU0 = queryvendor.OVC_VEN_COUNTRY;
            if (strU0 != null)
                drpOVC_VEN_COUNTRY.SelectedItem.Text = strU0;

        }
    }
}