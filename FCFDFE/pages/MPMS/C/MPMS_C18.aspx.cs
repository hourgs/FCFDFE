using System;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Entity.GMModel;
using System.Linq;
using FCFDFE.Content;
using System.Globalization;
using System.Data.Entity;
using TemplateEngine.Docx;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Xceed.Words.NET;
using Microsoft.International.Formatters;

namespace FCFDFE.pages.MPMS.C
{


    public partial class MPMS_C18 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        string strPurchNum = "";
        string strDRecive = "";
        byte numCheckTimes = 1;
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
                    string.IsNullOrEmpty(Request.QueryString["DRecive"]))
                {
                    Response.Redirect("MPMS_C11");
                }
                else
                {
                    strPurchNum = Request.QueryString["PurchNum"];
                    strDRecive = Request.QueryString["DRecive"];
                    numCheckTimes = Convert.ToByte(Request.QueryString["numCheckTimes"]);
                    if (!IsPostBack)
                    {
                        GetUserInfo();
                        DataImport();
                        RepeaterHeaderImport();
                        GV_ADVICE_Import();
                        GV_Attached_Import();
                        Approved_Import();
                        FCommon.Controls_Attributes("readonly", "true", txtOVC_DRECEIVE_PAPER, txtOVC_DRESULT, txtOVC_DREJECTs);

                        var queryStatus = mpms.TBMSTATUS.Where(t => t.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                        if (queryStatus != null)
                        {
                            btnSave.CssClass += " disabled";
                        }
                    }
                }
            }
        }
        protected void GV_ADVICE_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GV_ADVICE.UseAccessibleHeader = true;
                GV_ADVICE.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        #region OnClick
        protected void btnPrint_Command(object sender, CommandEventArgs e)
        {
            string filePath = "";
            string fileType = "";
            switch (e.CommandName)
            {
                case "btnPrintAppMemo":
                    filePath = PrintAppPDF();
                    fileType = "核定事項.docx";
                    break;
                case "btnPrintInfoPDF":
                    filePath = PrintInfoPDF();
                    fileType = "基本資料.docx";
                    break;
                case "btnPrintCheckOKPDF":
                    filePath = PrintCheckOKPDF();
                    fileType = "確認審會辦單.docx";
                    break;
                case "btnPrintMaterial":
                    var query = mpms.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.OVC_PURCH_KIND).FirstOrDefault();
                    if (query.Equals("1"))
                    {
                        PrintPDF();
                    }
                    else
                    {
                        MaterialApplication_ExportToWord();
                    }

                    break;
            }
            if (!string.IsNullOrEmpty(filePath))
            {
                FileInfo file = new FileInfo(filePath);
                //匯出Word檔提供下載
                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + strPurchNum + fileType);
                //Response.AppendHeader("Content-Disposition", "attachment; filename=" + DateTime.Now + "計畫清單.docx");
                Response.ContentType = "application/octet-stream";
                //Response.BinaryWrite(buffer);
                Response.WriteFile(file.FullName);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.Flush();
                File.Delete(filePath);
                Response.End();
            }
           
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string deptSn = ViewState["DEPT_SN"].ToString();
            string strCheckOK = "";
            var deptName = gm.TBMDEPTs.Where(o => o.OVC_DEPT_CDE.Equals(deptSn)).Select(o => o.OVC_ONNAME).FirstOrDefault();
            var query1301 = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            TBM1202 tbm1202 = new TBM1202();
            tbm1202 =
                    mpms.TBM1202.Where(o => o.OVC_PURCH.Equals(strPurchNum)
                                         && o.ONB_CHECK_TIMES == numCheckTimes
                                         && o.OVC_CHECK_UNIT.Equals(deptSn)
                                         && o.OVC_DRECEIVE.Equals(strDRecive))
                                .FirstOrDefault();
            
            //TBM1202_RESULT tbm1202R = new TBM1202_RESULT();
            //tbm1202R =
            //        mpms.TBM1202_RESULT.Where(o => o.OVC_PURCH.Equals(strPurchNum)
            //                             && o.ONB_CHECK_TIMES == numCheckTimes
            //                             && o.OVC_CHECK_UNIT.Equals(deptSn)
            //                             && o.OVC_DRECEIVE.Equals(strDRecive))
            //                    .FirstOrDefault();
            strCheckOK =  drpOVC_CHECK_OK.SelectedValue;
            if (strCheckOK.Equals("N") && !string.IsNullOrEmpty(txtOVC_DREJECTs.Text))
            {
                TBM1114 t1114 = new TBM1114();
                t1114.OVC_PURCH = strPurchNum;
                t1114.OVC_DATE = DateTime.Now;
                t1114.OVC_USER = ViewState["USER_NAME"].ToString();
                t1114.OVC_FROM_UNIT_NAME = deptName;
                t1114.OVC_TO_UNIT_NAME = query1301.OVC_PUR_NSECTION;
                t1114.OVC_REMARK = deptName + "將購案退回申購單位澄覆或修訂";
                mpms.TBM1114.Add(t1114);
                tbm1202.OVC_DREJECT = txtOVC_DREJECTs.Text;
                tbm1202.OVC_CHECK_OK = strCheckOK;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , t1114.GetType().Name.ToString(), this, "新增");
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1202.GetType().Name.ToString(), this, "修改");

                FCommon.AlertShow(PnMessage, "success", "系統訊息", "已將購案退回申購單位澄覆或修訂");
            }
            else if (strCheckOK.Equals("Y"))
            {
                string strMessage = "";
                if (string.IsNullOrEmpty(txtOVC_DRESULT.Text))
                    strMessage += "<p> 綜簽日不得為空 <p>";
                if (string.IsNullOrEmpty(strMessage))
                {
                    tbADVICE.Visible = true;
                    TBMPURCH_EXT tbmPURCH_EXT = new TBMPURCH_EXT();
                    tbm1202.OVC_CHECK_OK = strCheckOK;
                    tbm1202.OVC_DRECEIVE_PAPER = txtOVC_DRECEIVE_PAPER.Text;
                    tbm1202.OVC_DRESULT = txtOVC_DRESULT.Text;
                    //tbm1202R.OVC_CHECK_OK = strCheckOK;
                    //tbm1202R.OVC_DRECEIVE_PAPER = txtOVC_DRECEIVE_PAPER.Text;
                    //tbm1202R.OVC_DRESULT = txtOVC_DRESULT.Text;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1202.GetType().Name.ToString(), this, "修改");
                    tbmPURCH_EXT = mpms.TBMPURCH_EXT.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                    tbmPURCH_EXT.OVC_PURCH_5 = txtOVC_PURCH_NO.Text;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbmPURCH_EXT.GetType().Name.ToString(), this, "修改");
                    var query1301R = mpms.TBM1301_RECORD.Where(o => o.OVC_PURCH.Equals(strPurchNum));
                    string itemCount = "";
                    if (query1301R.Any())
                    {
                        var item = query1301R.OrderByDescending(o=>o.ITEM_COUNT).FirstOrDefault();
                        int count = Convert.ToInt32(item.ITEM_COUNT);
                        count++;
                        itemCount = count.ToString("000");
                    }
                    else
                    {
                        itemCount = "001";
                        
                    }
                    TBM1301_RECORD t1301R = new TBM1301_RECORD();
                    t1301R.OVC_PURCH = query1301.OVC_PURCH;
                    t1301R.OVC_PURCH_KIND = query1301.OVC_PURCH_KIND;
                    t1301R.OVC_LAB = query1301.OVC_LAB;
                    t1301R.OVC_PUR_ASS_VEN_CODE = query1301.OVC_PUR_ASS_VEN_CODE;
                    t1301R.OVC_BID_TIMES = query1301.OVC_BID_TIMES;
                    t1301R.OVC_BID = query1301.OVC_BID;
                    t1301R.OVC_PUR_NSECTION = query1301.OVC_PUR_NSECTION;
                    t1301R.OVC_PUR_SECTION = query1301.OVC_PUR_SECTION;
                    t1301R.OVC_PROPOSE = query1301.OVC_PROPOSE;
                    t1301R.OVC_DPROPOSE = query1301.OVC_DPROPOSE;
                    t1301R.OVC_PUR_APPROVE = query1301.OVC_PUR_APPROVE;
                    t1301R.OVC_PUR_DAPPROVE = query1301.OVC_PUR_DAPPROVE;
                    t1301R.OVC_PUR_IPURCH = query1301.OVC_PUR_IPURCH;
                    t1301R.OVC_PUR_IPURCH_ENG = query1301.OVC_PUR_IPURCH_ENG;
                    t1301R.OVC_PUR_USER = query1301.OVC_PUR_USER;
                    t1301R.OVC_KEYIN = query1301.OVC_KEYIN;
                    t1301R.OVC_PUR_IUSER_PHONE = query1301.OVC_PUR_IUSER_PHONE;
                    t1301R.OVC_PUR_IUSER_PHONE_EXT = query1301.OVC_PUR_IUSER_PHONE_EXT;
                    t1301R.OVC_PUR_IUSER_PHONE_EXT1 = query1301.OVC_PUR_IUSER_PHONE_EXT1;
                    t1301R.OVC_USER_CELLPHONE = query1301.OVC_USER_CELLPHONE;
                    t1301R.OVC_PUR_AGENCY = query1301.OVC_PUR_AGENCY;
                    t1301R.OVC_EST_REAR = query1301.OVC_EST_REAR;
                    t1301R.OVC_PUR_NPURCH = query1301.OVC_PUR_NPURCH;
                    t1301R.OVC_TARGET_DO = query1301.OVC_TARGET_DO;
                    t1301R.OVC_PUR_CURRENT = query1301.OVC_PUR_CURRENT;
                    t1301R.ONB_PUR_RATE = query1301.ONB_PUR_RATE;
                    t1301R.ONB_PUR_BUDGET = query1301.ONB_PUR_BUDGET;
                    t1301R.OVC_AGNT_IN = query1301.OVC_AGNT_IN;
                    t1301R.OVC_PUR_FEE_OK = query1301.OVC_PUR_FEE_OK;
                    t1301R.OVC_PUR_GOOD_OK = query1301.OVC_PUR_GOOD_OK;
                    t1301R.OVC_PUR_TAX_OK = query1301.OVC_PUR_TAX_OK;
                    t1301R.OVC_PUR_DCANPO = query1301.OVC_PUR_DCANPO;
                    t1301R.OVC_PUR_DCANRE = query1301.OVC_PUR_DCANRE;
                    t1301R.OVC_EXAMPLE_SUPPORT = query1301.OVC_EXAMPLE_SUPPORT;
                    t1301R.OVC_PUR_ALLOW = query1301.OVC_PUR_ALLOW;
                    t1301R.OVC_PUR_CREAT = query1301.OVC_PUR_CREAT;
                    t1301R.OVC_DOING_UNIT = query1301.OVC_DOING_UNIT;
                    t1301R.OVC_PERMISSION_UPDATE = query1301.OVC_PERMISSION_UPDATE;
                    t1301R.OVC_SUPERIOR_UNIT = query1301.OVC_SUPERIOR_UNIT;
                    t1301R.OVC_SECTION_CHIEF = query1301.OVC_SECTION_CHIEF;
                    t1301R.OVC_SHIP_TIMES = query1301.OVC_SHIP_TIMES;
                    t1301R.OVC_RECEIVE_PLACE = query1301.OVC_RECEIVE_PLACE;
                    t1301R.OVC_POI_IINSPECT = query1301.OVC_POI_IINSPECT;
                    t1301R.OVC_COUNTRY = query1301.OVC_COUNTRY;
                    t1301R.OVC_WAY_TRANS = query1301.OVC_WAY_TRANS;
                    t1301R.OVC_RECEIVE_NSECTION = query1301.OVC_RECEIVE_NSECTION;
                    t1301R.OVC_FROM_TO = query1301.OVC_FROM_TO;
                    t1301R.OVC_TO_PLACE = query1301.OVC_TO_PLACE;
                    t1301R.ONB_PUR_BUDGET_NT = query1301.ONB_PUR_BUDGET_NT;
                    t1301R.ONB_RESERVE_AMOUNT = query1301.ONB_RESERVE_AMOUNT;
                    t1301R.OVC_SPECIAL = query1301.OVC_SPECIAL;
                    t1301R.OVC_BID_MONEY = query1301.OVC_BID_MONEY;
                    t1301R.OVC_NEGOTIATION = query1301.OVC_NEGOTIATION;
                    t1301R.OVC_QUOTE = query1301.OVC_QUOTE;
                    t1301R.OVC_PLAN_PURCH = query1301.OVC_PLAN_PURCH;
                    t1301R.OVC_COPY_FROM = query1301.OVC_COPY_FROM;
                    t1301R.OVC_USER_TITLE = query1301.OVC_USER_TITLE;
                    t1301R.IS_PLURAL_BASIS = query1301.IS_PLURAL_BASIS;
                    t1301R.IS_OPEN_CONTRACT = query1301.IS_OPEN_CONTRACT;
                    t1301R.IS_JUXTAPOSED_MANUFACTURER = query1301.IS_JUXTAPOSED_MANUFACTURER;
                    t1301R.OVC_MEMO = txtApproved_MEMO.Text;
                    t1301R.ITEM_COUNT = itemCount;
                    t1301R.OVC_CHECK_OK = drpOVC_CHECK_OK.SelectedValue;
                    t1301R.OVC_MEMO = txtOVC_INTEGRATED_REASON.Text;
                    mpms.TBM1301_RECORD.Add(t1301R);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , t1301R.GetType().Name.ToString(), this, "新增");
                    tbADVICE.Visible = true;
                    divPrint.Visible = true;
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
                }
                else
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                }
            }
            else if (strCheckOK.Equals("N"))
            {
                tbm1202.OVC_CHECK_OK = "N";
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1202.GetType().Name.ToString(), this, "修改");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
            }

        }

        protected void btnTo_Command(object sender, CommandEventArgs e)
        {
            string url;
            switch (e.CommandName)
            {
                case "btnToAllMemo":
                    url = "~/pages/MPMS/C/MPMS_C18_2.aspx?PurchNum=" + strPurchNum + "&CheckTimes=" + numCheckTimes + "&DRecive=" +strDRecive;
                    Response.Redirect(url);
                    break;
                case "btnToAppMemo":
                    url = "~/pages/MPMS/C/MPMS_C22.aspx?PurchNum=" + strPurchNum + "&CheckTimes=" + numCheckTimes + "&DRecive=" + strDRecive;
                    Response.Redirect(url);
                    break;
                case "btnToOKmemo":
                    url = "~/pages/MPMS/C/MPMS_C18_3.aspx?PurchNum=" + strPurchNum + "&CheckTimes=" + numCheckTimes + "&DRecive=" + strDRecive;
                    Response.Redirect(url);
                    break;
                case "btnToAttch":
                    url = "~/pages/MPMS/C/MPMS_C23.aspx?PurchNum=" + strPurchNum + "&CheckTimes=" + numCheckTimes + "&DRecive=" + strDRecive;
                    Response.Redirect(url);
                    break;
                case "btnToADVICE":
                    url = "~/pages/MPMS/C/MPMS_C21.aspx?PurchNum=" + strPurchNum + "&CheckTimes=" + numCheckTimes + "&DRecive=" + strDRecive;
                    Response.Redirect(url);
                    break;
            }
            
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string url = "~/pages/MPMS/C/MPMS_C14.aspx?PurchNum=" + strPurchNum;
            Response.Redirect(url);
        }
        #endregion

        #region 副程式
        private void DataImport()
        {
            string deptSn = ViewState["DEPT_SN"].ToString();
            var query =
                (from t in mpms.TBM1301
                 join t1202 in mpms.TBM1202 on t.OVC_PURCH equals t1202.OVC_PURCH
                 join tDept in mpms.TBMDEPTs on t1202.OVC_CHECK_UNIT equals tDept.OVC_DEPT_CDE
                 where t.OVC_PURCH.Equals(strPurchNum) && t1202.ONB_CHECK_TIMES == numCheckTimes
                        && t1202.OVC_CHECK_UNIT.Equals(deptSn) && t1202.OVC_DRECEIVE.Equals(strDRecive)
                 select new
                 {
                     t.OVC_PURCH,
                     t.OVC_PUR_AGENCY,
                     t.OVC_PUR_IPURCH,
                     t.OVC_PUR_NSECTION,
                     t.OVC_PUR_USER,
                     t.OVC_PUR_IUSER_PHONE,
                     t.OVC_PUR_IUSER_PHONE_EXT,
                     OVC_CHECK_OK = t1202.OVC_CHECK_OK ?? "N",
                     t1202.OVC_DRECEIVE,
                     t1202.OVC_DRECEIVE_PAPER,
                     OVC_DRESULT = t1202.OVC_DRESULT ?? "",
                     tDept.OVC_ONNAME,
                     t1202.OVC_CHECKER,
                 }).FirstOrDefault();
            lblOVC_PURCH.Text = query.OVC_PURCH + query.OVC_PUR_AGENCY;
            lblOVC_PUR_IPURCH.Text = query.OVC_PUR_IPURCH;
            lblOVC_AGENT_UNIT.Text = query.OVC_PUR_NSECTION + "-" + query.OVC_PUR_USER + "(" + query.OVC_PUR_IUSER_PHONE + "軍線:" 
                                   + query.OVC_PUR_IUSER_PHONE_EXT + ")";
            if (query.OVC_CHECK_OK.Equals("Y"))
            {
                lblONB_CHECK_STATUS.Text = "(確認審)";
                tbADVICE.Visible = true;
                divPrint.Visible = true;
            }
            else
            {
                if(numCheckTimes == 1)
                {
                    lblONB_CHECK_STATUS.Text = "(初審)";
                }
                else
                {
                    lblONB_CHECK_STATUS.Text = "(複審)";
                }
            }
            lblONB_CHECK_TIMES.Text = numCheckTimes.ToString();
            lblOVC_DAUDIT_ASSIGN.Text = getTaiwanDate(query.OVC_DRECEIVE);
            lblOVC_CHECK_UNIT.Text = query.OVC_ONNAME + "(" + query.OVC_CHECKER + ")";
            drpOVC_CHECK_OK.SelectedValue = query.OVC_CHECK_OK;
            txtOVC_DRECEIVE_PAPER.Text = query.OVC_DRECEIVE_PAPER;
            txtOVC_DRESULT.Text = query.OVC_DRESULT;
        }

        private void GV_ADVICE_Import()
        {
            string deptSn = ViewState["DEPT_SN"].ToString();
            string[] field = { "OVC_ITEM", "OVC_ITEM_ADVICE", "OVC_ITEM_DESC" };
            
            var query = 
                from t in mpms.TBM1202_ADVICE
                join t1407 in mpms.TBM1407 on t.OVC_ITEM equals t1407.OVC_PHR_ID
                where t.OVC_PURCH.Equals(strPurchNum) && t.OVC_DRECEIVE.Equals(strDRecive)
                    && t1407.OVC_PHR_CATE.Equals("Q5")
                select new
                {
                    OVC_ITEM = t1407.OVC_PHR_DESC,
                    t.OVC_ITEM_ADVICE,
                    t.OVC_ITEM_DESC
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            FCommon.GridView_dataImport(GV_ADVICE, dt, field);
        }

        private void GV_Attached_Import()
        {
            string deptSn = ViewState["DEPT_SN"].ToString();
            string[] field = { "TARGET_UNIT", "OVC_ATTACH_NAME" }; 
            var query =
                from t in mpms.TBM1119_1
                join t1407 in mpms.TBM1407 on t.OVC_TARGET_UNIT equals t1407.OVC_PHR_ID
                where t.OVC_PURCH.Equals(strPurchNum) && t.OVC_CHECK_UNIT.Equals(deptSn) 
                        && t.OVC_DRECEIVE.Equals(strDRecive) && t1407.OVC_PHR_CATE.Equals("Q6")
                orderby t.OVC_TARGET_UNIT
                select new
                {
                    TARGET_UNIT = t1407.OVC_PHR_DESC,
                    t.OVC_ATTACH_NAME,
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            hasRows = FCommon.GridView_dataImport(GV_Attached, dt, field);
        }

        private void Approved_Import()
        {
            string deptSn = ViewState["DEPT_SN"].ToString();
            int count = 1;
            var query = 
                mpms.TBM1202_6
                .Where
                (o => o.OVC_PURCH.Equals(strPurchNum)
                    && o.OVC_CHECK_UNIT.Equals(deptSn)
                    && o.ONB_CHECK_TIMES == numCheckTimes)
                    .OrderBy(o=>o.OVC_IKIND).ThenBy(o=>o.ONB_NO);
            if (query.Any())
            {
                foreach (var item in query)
                {
                    txtApproved_MEMO.Text += count.ToString() + ". " + item.OVC_MEMO + "\r\n";
                    count++;
                }
                txtApproved_MEMO.Height = count * 20;
                txtApproved_MEMO.Visible = true;
            }
            else
            {
                txtApproved_MEMO.Visible = false;
            }
            
        }
        private DataTable CommentImport(string auditUnit)
        {
            string deptSn = ViewState["DEPT_SN"].ToString();
            var query =
                from t1202C in mpms.TBM1202_COMMENT
                join tDETAIL in mpms.TBMOPINION_DETAIL
                on new { t1202C.OVC_AUDIT_UNIT, t1202C.OVC_TITLE, t1202C.OVC_TITLE_ITEM, t1202C.OVC_TITLE_DETAIL }
                equals new { tDETAIL.OVC_AUDIT_UNIT,tDETAIL.OVC_TITLE, tDETAIL.OVC_TITLE_ITEM, tDETAIL.OVC_TITLE_DETAIL }
                where t1202C.OVC_PURCH.Equals(strPurchNum) && t1202C.ONB_CHECK_TIMES == numCheckTimes
                        && t1202C.OVC_CHECK_UNIT.Equals(deptSn) && t1202C.OVC_DRECEIVE.Equals(strDRecive)
                        && t1202C.OVC_AUDIT_UNIT.Equals(auditUnit)
                orderby t1202C.ONB_NO
                select new
                {
                    t1202C.ONB_NO,
                    tDETAIL.OVC_CONTENT,
                    t1202C.OVC_CHECK_REASON,
                    t1202C.OVC_RESPONSE
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            foreach (DataRow rows in dt.Rows)
            {
                rows["OVC_RESPONSE"] = rows["OVC_RESPONSE"].ToString().Replace("<br>", "");
            }
            return dt;
        }

        protected void Repeater_Header_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                HiddenField hidOVC_AUDIT_UNIT = (HiddenField)e.Item.FindControl("hidOVC_AUDIT_UNIT");
                string strUnit = hidOVC_AUDIT_UNIT.Value;
                Repeater childRepeater = (Repeater)e.Item.FindControl("Repeater_Content");//找到要繫結資料的childRepeater
                if (childRepeater != null)
                {
                    DataTable dt = new DataTable();
                    dt = CommentImport(strUnit);
                    childRepeater.DataSource = dt;
                    childRepeater.DataBind();
                }
            }
        }

        private void RepeaterHeaderImport()
        {
            string deptSn = ViewState["DEPT_SN"].ToString();
            //第一層repeater
            var query =
                (from t12021 in mpms.TBM1202_1
                 join t1407 in mpms.TBM1407 on t12021.OVC_AUDIT_UNIT equals t1407.OVC_PHR_ID
                join tAccount in mpms.ACCOUNT
                on new { name = t12021.OVC_AUDITOR, unit = t12021.OVC_CHECK_UNIT } 
                equals new { name = tAccount.USER_NAME, unit = tAccount.DEPT_SN }
                where t12021.OVC_PURCH.Equals(strPurchNum) && t12021.ONB_CHECK_TIMES == numCheckTimes
                        && t12021.OVC_CHECK_UNIT.Equals(deptSn) && t12021.OVC_DRECEIVE.Equals(strDRecive)
                        && t1407.OVC_PHR_CATE.Equals("K5") && tAccount.IUSER_PHONE != null
                orderby t1407.OVC_PHR_ID
                select new
                {
                    t12021.OVC_AUDIT_UNIT,
                    t1407.OVC_USR_ID,
                    t12021.OVC_AUDITOR,
                    tAccount.IUSER_PHONE,
                    t12021.OVC_DAUDIT_ASSIGN,
                    t12021.OVC_DAUDIT
                }).ToArray();
            var queryFinal =
                from t in query
                select new
                {
                    t.OVC_AUDIT_UNIT,
                    t.OVC_USR_ID,
                    t.OVC_AUDITOR,
                    t.IUSER_PHONE,
                    PROCESS = DateDiff(t.OVC_DAUDIT_ASSIGN,t.OVC_DAUDIT)
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(queryFinal);
            Repeater_Header.DataSource = dt;
            Repeater_Header.DataBind();
        }

        private string DateDiff(string strdate1,string strdate2 )
        {
            if (string.IsNullOrEmpty(strdate1) || string.IsNullOrEmpty(strdate2))
            {
                return "";
            }
            else
            {
                string dateDiff = null;
                DateTime DateTime1 = Convert.ToDateTime(strdate1);
                DateTime DateTime2 = Convert.ToDateTime(strdate2);
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                dateDiff = ts.Days.ToString();
                return dateDiff;
            }
            
        }
        
        private string getTaiwanDate(string strDate)
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

        private void GetUserInfo()
        {
            if (Session["userid"] != null)
            {
                string userID = Session["userid"].ToString();
                var userInfo = gm.ACCOUNTs.Where(o => o.USER_ID.Equals(userID)).FirstOrDefault();
                ViewState["USER_NAME"] = userInfo.USER_NAME;
                ViewState["DEPT_SN"] = userInfo.DEPT_SN;
            } 
        }

        private string GetTaiwanDate(string strDate, string cString)
        {
            if (DateTime.TryParse(strDate, out DateTime datetime))
            {
                CultureInfo culture = new CultureInfo("zh-TW");
                culture.DateTimeFormat.Calendar = new TaiwanCalendar();
                if (cString.Equals("chDate"))
                {
                    return datetime.ToString("yyy年MM月dd日", culture);
                }
                else
                    return datetime.ToString("yyyy" + cString + "MM" + cString + "dd", culture);
            }
            else
            {

                return "";
            }

        }

        private string GetAttached(string kind)
        {
            //附件內容
            var queryFile =
                mpms.TBM1119.AsEnumerable()
                .Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_IKIND.Equals(kind))
                .OrderByDescending(o => o.OVC_ATTACH_NAME.Split('.')[0].Length)
                .OrderBy(o => o.OVC_ATTACH_NAME.Split('.')[0]);
            string[] arrFileName = new string[queryFile.Count()];
            int counter = 0;
            foreach (var row in queryFile)
            {
                arrFileName[counter] = row.OVC_ATTACH_NAME;
                counter++;
            }
            string strFileName = string.Join("、", arrFileName);
            return strFileName;
        }
        #endregion

        #region 列印核定意見
        private string PrintAppPDF()
        {
            string fileName = "核定事項.docx";
            string TempName = "";
            string filePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + fileName));
            string outPutfilePath = "";
            string subYear = "";
            var valuesToFill = new TemplateEngine.Docx.Content();
            var query1301 =
                (from t in gm.TBM1301
                 where t.OVC_PURCH.Equals(strPurchNum)
                 select new
                 {
                     t.OVC_PURCH,
                     OVC_PUR_IPURCH = t.OVC_PUR_IPURCH ?? " ",
                     t.OVC_PUR_AGENCY
                 }).FirstOrDefault();

            TempName = strPurchNum + query1301.OVC_PUR_AGENCY + DateTime.Now.ToString("yyyyMMddHHmmss") + "核定事項" + ".docx";
            outPutfilePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + TempName));
            File.Copy(filePath, outPutfilePath);
            subYear = query1301.OVC_PURCH.Substring(2, 2);
            if (subYear.StartsWith("0"))
            {
                subYear = "1" + subYear;
            }
            valuesToFill.Fields.Add(new FieldContent("YEAR", subYear));
            valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IPURCH", query1301.OVC_PUR_IPURCH));
            valuesToFill.Fields.Add(new FieldContent("OVC_PURCH", strPurchNum));
            valuesToFill.Fields.Add(new FieldContent("OVC_PUR_AGENCY", query1301.OVC_PUR_AGENCY));
            var query12026 =
                from t in mpms.TBM1202_6
                where t.OVC_PURCH.Equals(strPurchNum) && t.ONB_CHECK_TIMES == numCheckTimes
                orderby t.OVC_IKIND
                select t.OVC_MEMO;
            
            if (query12026.Any())
            {
                var listContent = new ListContent("MemoList");
                foreach (var memo in query12026)
                {
                    listContent.AddItem(new FieldContent("MemoConten", memo));
                }
                valuesToFill.Lists.Add(listContent);
            }
            
            //儲存變更
            using (var outputDocument = new TemplateProcessor(outPutfilePath)
                    .SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }

            return outPutfilePath;

        }
        #endregion

        #region 列印基本資料
        private string PrintInfoPDF()
        {
            string fileName = "基本資料.docx";
            string TempName = "";
            string filePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + fileName));
            string outPutfilePath = "";
            string subYear = "";
            var valuesToFill = new TemplateEngine.Docx.Content();
            var query1301 =
                (from t in gm.TBM1301
                 where t.OVC_PURCH.Equals(strPurchNum)
                 select new
                 {
                     t.OVC_PURCH,
                     OVC_PUR_NSECTION = t.OVC_PUR_NSECTION ?? " ",
                     OVC_PUR_IPURCH = t.OVC_PUR_IPURCH ?? " ",
                     t.OVC_PUR_AGENCY,
                     ONB_PUR_BUDGET_NT = t.ONB_PUR_BUDGET_NT ?? 0
                 }).FirstOrDefault();
            subYear = query1301.OVC_PURCH.Substring(2, 2);
            if (subYear.StartsWith("0"))
            {
                subYear = "1" + subYear;
            }
            TempName = strPurchNum + query1301.OVC_PUR_AGENCY + DateTime.Now.ToString("yyyyMMddHHmmss") + "核定事項" + ".docx";
            outPutfilePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + TempName));
            File.Copy(filePath, outPutfilePath);

            valuesToFill.Fields.Add(new FieldContent("YEAR", subYear));
            valuesToFill.Fields.Add(new FieldContent("OVC_IPURCH", query1301.OVC_PUR_IPURCH));
            valuesToFill.Fields.Add(new FieldContent("OVC_PURCH", strPurchNum));
            valuesToFill.Fields.Add(new FieldContent("OVC_PUR_AGENCY", query1301.OVC_PUR_AGENCY));
            valuesToFill.Fields.Add(new FieldContent("OVC_PUR_NSECTION", query1301.OVC_PUR_NSECTION));
            valuesToFill.Fields.Add(new FieldContent("YEAR2", subYear));
            valuesToFill.Fields.Add(new FieldContent("OVC_IPURCH2", query1301.OVC_PUR_IPURCH));
           
            var query1231 =
              from t in mpms.TBM1231
              where t.OVC_PURCH.Equals("AG06011")
              select new
              {
                  t.OVC_ISOURCE,
                  t.OVC_IKIND,
                  t.OVC_PUR_APPR_PLAN,
                  t.OVC_PUR_DAPPR_PLAN,
              };
            string strISOURCE = " ";
            string strPUR_APPR = " ";
            if (query1231.Any())
            {
                strISOURCE = "";
                foreach (var item in query1231)
                {
                    if (item.OVC_IKIND.Equals("1"))
                    {
                        strISOURCE += item.OVC_ISOURCE + ";" + "國防預算" + "\t";
                    }
                    else
                    {
                        strISOURCE += item.OVC_ISOURCE + ";" + "非國防預算" + "\t";
                    }
                    strPUR_APPR += item.OVC_PUR_APPR_PLAN + "\t";
                }
            }
            valuesToFill.Fields.Add(new FieldContent("OVC_ISOURCE", strISOURCE));
            var query1118 =
                from t in mpms.TBM1118
                where t.OVC_PURCH == "HA96001"
                group t by new { t.OVC_POI_IBDG, t.OVC_PJNAME } into g
                select new
                {
                    g.Key.OVC_POI_IBDG,
                    g.Key.OVC_PJNAME
                };
            string strPJNAME = " ";
            if (query1118.Any())
            {
                foreach (var item in query1118)
                {
                    if (item.OVC_POI_IBDG.Equals(item.OVC_POI_IBDG))
                    {
                        strPJNAME += strPJNAME + item.OVC_PJNAME + ";";
                    }
                    else
                    {
                        strPJNAME = item.OVC_POI_IBDG + ":" + item.OVC_PJNAME ;
                    }
                }
            }
            valuesToFill.Fields.Add(new FieldContent("OVC_ISOURCE", strISOURCE));
            string strBudget = Convert.ToDecimal(query1301.ONB_PUR_BUDGET_NT).ToString("#,##.00");
            valuesToFill.Fields.Add(new FieldContent("ONB_PUR_BUDGET_NT", strISOURCE));
            string ovcIkind = "";

            switch (query1301.OVC_PUR_AGENCY)
            {
                case "B":
                case "L":
                case "P":
                    ovcIkind = "M3";
                    break;
                case "M":
                case "S":
                    ovcIkind = "F3";
                    break;
                default:
                    ovcIkind = "W3";
                    break;
            }

            var query1220_1 =
                from t in mpms.TBM1220_1
                where t.OVC_PURCH.Equals(strPurchNum)
                select t;

            var query =
              from tIKIND in mpms.TBM1220_2.AsEnumerable()
              join tMemo in query1220_1.AsEnumerable() on tIKIND.OVC_IKIND equals tMemo.OVC_IKIND into ps
              from tMemo in ps.DefaultIfEmpty()
              where tIKIND.OVC_IKIND.StartsWith(ovcIkind)
              orderby tIKIND.OVC_IKIND
              select new
              {
                  tIKIND.OVC_IKIND,
                  tIKIND.OVC_MEMO_NAME,
                  OVC_MEMO = tMemo != null ? tMemo.OVC_MEMO : " "
              };
            ListContent listContent = new ListContent("ListNote");
            int count = 1;
            foreach(var item in query)
            {
               
                if (count <= 5)
                {
                    string title = "title" + count.ToString();
                    string content = "Memo" + count.ToString();
                    string titleName = item.OVC_MEMO_NAME;
                    int charStart = titleName.IndexOf("(");
                    int charEnd = titleName.IndexOf(")");
                    string lastName = titleName.Remove(charStart, charEnd + 1);
                    valuesToFill.Fields.Add(new FieldContent(title, lastName));
                    valuesToFill.Fields.Add(new FieldContent(content, item.OVC_MEMO));
                }
                else
                {
                    if(!string.IsNullOrEmpty(item.OVC_MEMO.Trim()))
                        listContent.AddItem(new FieldContent("NoteContent", item.OVC_MEMO));
                }
                count++;
            }
            valuesToFill.Lists.Add(listContent);
            using (var outputDocument = new TemplateProcessor(outPutfilePath)
                   .SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }

            return outPutfilePath;
        }

     
        #endregion

        #region 列印確認審會辦單
        private string PrintCheckOKPDF()
        {
            string fileName = "確認審會辦單.docx";
            string TempName = "";
            string filePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + fileName));
            string outPutfilePath = "";
            string subYear = "";
            string userDept = ViewState["DEPT_SN"].ToString();
            var valuesToFill = new TemplateEngine.Docx.Content();
            var query1301 =
               (from t in gm.TBM1301
                where t.OVC_PURCH.Equals(strPurchNum)
                select new
                {
                    t.OVC_PURCH,
                    OVC_PUR_NSECTION = t.OVC_PUR_NSECTION ?? " ",
                    OVC_PUR_IPURCH = t.OVC_PUR_IPURCH ?? " ",
                    OVC_PUR_DAPPROVE = t.OVC_PUR_DAPPROVE ?? " ",
                    OVC_PUR_APPROVE = t.OVC_PUR_APPROVE ?? " ",
                    t.OVC_PUR_AGENCY
                }).FirstOrDefault();
            subYear = query1301.OVC_PURCH.Substring(2, 2);
            if (subYear.StartsWith("0"))
            {
                subYear = "1" + subYear;
            }
            TempName = strPurchNum + query1301.OVC_PUR_AGENCY + DateTime.Now.ToString("yyyyMMddHHmmss") + "核定事項" + ".docx";
            outPutfilePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + TempName));
            File.Copy(filePath, outPutfilePath);

            valuesToFill.Fields.Add(new FieldContent("YEAR", subYear));
            valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IPURCH", query1301.OVC_PUR_IPURCH));
            valuesToFill.Fields.Add(new FieldContent("OVC_PURCH", strPurchNum));
            valuesToFill.Fields.Add(new FieldContent("OVC_PUR_AGENCY", query1301.OVC_PUR_AGENCY));
            valuesToFill.Fields.Add(new FieldContent("OVC_PUR_NSECTION", query1301.OVC_PUR_NSECTION));
            valuesToFill.Fields.Add(new FieldContent("OVC_DPROPOSE", getTaiwanDate(query1301.OVC_PUR_DAPPROVE)));
            valuesToFill.Fields.Add(new FieldContent("OVC_PROPOSE", query1301.OVC_PUR_APPROVE));

            var queryDept =
                from t in mpms.TBM1202_1
                join t1407 in mpms.TBM1407 on t.OVC_AUDIT_UNIT equals t1407.OVC_PHR_ID
                where t.OVC_PURCH.Equals(strPurchNum) && t.ONB_CHECK_TIMES == numCheckTimes && t1407.OVC_PHR_CATE.Equals("K5")
                select new
                {
                    t1407.OVC_PHR_DESC,
                    t1407.OVC_USR_ID
                };
            if (userDept.Equals("0A100") || userDept.Equals("00N00"))
            {
                int count = 1;
                foreach(var item in queryDept)
                {
                    if (count <= 6)
                    {
                        string deptNum = "dept" + count.ToString();
                        valuesToFill.Fields.Add(new FieldContent(deptNum, item.OVC_PHR_DESC));
                        if (count >= 2)
                        {
                            string memoNum = "memo" + count.ToString();
                            valuesToFill.Fields.Add(new FieldContent(memoNum, "詳第一受會單位送會目的"));
                        }
                        count++;
                    }
                    
                }
            }
            else
            {
                int count = 1;
                foreach (var item in queryDept)
                {
                    if (count <= 6)
                    {
                        string deptNum = "dept" + count.ToString();
                        valuesToFill.Fields.Add(new FieldContent(deptNum, item.OVC_USR_ID));
                        if (count >= 2)
                        {
                            string memoNum = "memo" + count.ToString();
                            valuesToFill.Fields.Add(new FieldContent(memoNum, "詳第一受會單位送會目的"));
                        }
                        count++;
                    }
                }
            }

            using (var outputDocument = new TemplateProcessor(outPutfilePath)
                  .SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }

            return outPutfilePath;
        }
        #endregion

        #region PDF列印-物資申請書 內購
        private void PrintPDF()
        {
            string angcy = "";
            //PDF列印
            var doc1 = new Document(PageSize.A4, 50, 50, 45, 50);
            MemoryStream Memory = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            Watermark wm = new Watermark();
            wm.strPurchNum = strPurchNum;
            writer.PageEvent = wm;
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\msjhbd.ttc,0", BaseFont.IDENTITY_H
               , BaseFont.NOT_EMBEDDED);//設定字型
            iTextSharp.text.Font ChFont = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            iTextSharp.text.Font ChFont_blue = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.NORMAL, new BaseColor(2, 0, 255));
            iTextSharp.text.Font ChFont_msg = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.ITALIC, BaseColor.RED);
            iTextSharp.text.Font ChFont_memo = new iTextSharp.text.Font(bfChinese, 8, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            doc1.Open();

            //page1
            #region Page1
            var t1301 =
                (from t in gm.TBM1301
                 join t1407 in gm.TBM1407 on t.OVC_PUR_CURRENT equals t1407.OVC_PHR_ID
                 where t.OVC_PURCH.Equals(strPurchNum) && t1407.OVC_PHR_CATE.Equals("B0")
                 select new
                 {
                     t.OVC_PUR_AGENCY,
                     t.OVC_PUR_NSECTION,
                     t.OVC_PUR_DAPPROVE,
                     t.OVC_PUR_APPROVE,
                     t.OVC_DPROPOSE,
                     t.OVC_PROPOSE,
                     t.OVC_PUR_IPURCH,
                     OVC_PUR_CURRENT = t1407.OVC_PHR_DESC,
                     t.ONB_PUR_BUDGET,
                     t.OVC_PUR_SECTION,
                     t.OVC_SECTION_CHIEF,
                     t.OVC_AGNT_IN
                 }).FirstOrDefault();
            if (t1301.OVC_PUR_AGENCY.Equals("1"))
                angcy = "內購";
            else
                angcy = "外購";
            var tablePurch = mpms.TBMPURCH_EXT.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            //table1
            PdfPTable table = new PdfPTable(new float[] { 1, 3, 1, 1, 1, 3, 2, 4 });
            table.TotalWidth = 500f;
            table.LockedWidth = true;
            PdfPCell header = new PdfPCell();
            Chunk glue = new Chunk(new VerticalPositionMark());
            string nsection = "";
            if (tablePurch == null)
            {
                nsection = t1301.OVC_PUR_NSECTION;
            }
            else
            {
                nsection = tablePurch.OVC_PUR_NSECTION_2;
            }
            string strp1 = nsection + angcy + "物資申請書";
            iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(strp1, ChFont);
            p.Add(new Chunk(glue));//有沒有其他寫法  無法垂直置中
            string strp2 = "中華民國" + GetTaiwanDate(t1301.OVC_PUR_DAPPROVE, "chDate") + "\n";
            p.Add(strp2);//核定日期
            p.Add(new Chunk(glue));
            p.Add(t1301.OVC_PUR_APPROVE);//核定文號
            p.Alignment = Element.ALIGN_MIDDLE;
            header.AddElement(p);
            header.Colspan = 8;
            table.AddCell(header);
            PdfPCell cell2_1 = new PdfPCell(new Phrase("申購單位", ChFont));
            table.AddCell(cell2_1);
            PdfPCell cell2_2 = new PdfPCell(new Phrase(t1301.OVC_PUR_NSECTION, ChFont));
            cell2_2.Colspan = 4;
            cell2_2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell2_2);
            PdfPCell cell2_3 = new PdfPCell(new Phrase("原申購日期及文號", ChFont));
            cell2_3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2_3.HorizontalAlignment = 1;
            table.AddCell(cell2_3);
            string strPropose = "中華民國" + GetTaiwanDate(t1301.OVC_DPROPOSE, "chDate") + "\n" + t1301.OVC_PROPOSE;
            PdfPCell cell2_4 = new PdfPCell(new Phrase(strPropose, ChFont));//申購日期文號
            cell2_4.Colspan = 2;
            cell2_4.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2_4.HorizontalAlignment = 2;
            table.AddCell(cell2_4);
            doc1.Add(table);

            //table2
            PdfPTable table2 = new PdfPTable(new float[] { 0.7f, 3, 1, 1, 1, 2, 1.5f, 4 });
            table2.TotalWidth = 500f;
            table2.LockedWidth = true;

            PdfPCell cell3_1 = new PdfPCell(new Phrase("物品種類及數量", ChFont));
            cell3_1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell3_1.HorizontalAlignment = 1;
            cell3_1.Rowspan = 4;
            table2.AddCell(cell3_1);
            PdfPCell cell3_2 = new PdfPCell(new Phrase("名稱", ChFont));
            cell3_2.HorizontalAlignment = 1;
            cell3_2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell3_2);
            PdfPCell cell3_3 = new PdfPCell(new Phrase("單位", ChFont));
            cell3_3.HorizontalAlignment = 1;
            cell3_3.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell3_3);
            PdfPCell cell3_4 = new PdfPCell(new Phrase("預估數", ChFont));
            cell3_4.HorizontalAlignment = 1;
            cell3_4.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell3_4);
            PdfPCell cell3_5 = new PdfPCell(new Phrase("單價", ChFont));
            cell3_5.HorizontalAlignment = 1;
            cell3_5.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell3_5);
            PdfPCell cell3_6 = new PdfPCell(new Phrase("預估貨款總價", ChFont));
            cell3_6.HorizontalAlignment = 1;
            cell3_6.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell3_6);
            PdfPCell cell3_7 = new PdfPCell(new Phrase("預算來源", ChFont));
            cell3_7.Colspan = 2;
            cell3_7.HorizontalAlignment = 1;
            cell3_7.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell3_7);
            PdfPCell cell4_1 = new PdfPCell(new Phrase(t1301.OVC_PUR_IPURCH + "，詳清單", ChFont));
            cell4_1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell4_1.Colspan = 5;
            cell4_1.Rowspan = 2;
            table2.AddCell(cell4_1);
            PdfPCell cell4_2 = new PdfPCell(new Phrase("款源", ChFont));
            cell4_2.HorizontalAlignment = 1;
            table2.AddCell(cell4_2);
            var t1118_ISOURCE = mpms.TBM1118.Where(o => o.OVC_PURCH.Equals(strPurchNum)).GroupBy(o => o.OVC_ISOURCE).Select(o => o.Key);
            string Isource = String.Join(";", t1118_ISOURCE);
            PdfPCell cell4_3 = new PdfPCell(new Phrase(Isource, ChFont));
            table2.AddCell(cell4_3);
            PdfPCell cell5_2 = new PdfPCell(new Phrase("科目", ChFont));
            cell5_2.HorizontalAlignment = 1;
            table2.AddCell(cell5_2);
            var t1118_PJNAME = mpms.TBM1118.Where(o => o.OVC_PURCH.Equals(strPurchNum)).GroupBy(o => o.OVC_PJNAME).Select(o => o.Key);
            string Pjname = String.Join(";", t1118_PJNAME);
            PdfPCell cell5_3 = new PdfPCell(new Phrase(Pjname, ChFont));
            table2.AddCell(cell5_3);
            PdfPCell cell6_1 = new PdfPCell(new Phrase("貨款總價", ChFont));
            cell6_1.HorizontalAlignment = 1;
            table2.AddCell(cell6_1);
            decimal moneyNT = (decimal)t1301.ONB_PUR_BUDGET;
            string money = FCommon.MoneyToChinese(moneyNT.ToString()) + "(含稅)";
            PdfPCell cell6_2 = new PdfPCell(new Phrase(t1301.OVC_PUR_CURRENT + " " + money, ChFont));
            cell6_2.Colspan = 4;
            table2.AddCell(cell6_2);
            PdfPCell cell6_3 = new PdfPCell(new Phrase("奉准日期及文號", ChFont));//***************
            cell6_3.HorizontalAlignment = 1;
            table2.AddCell(cell6_3);
            var queryDPPR_PLAN =
                from t in mpms.TBM1231
                where t.OVC_PURCH.Equals(strPurchNum)
                select new
                {
                    date = t.OVC_PUR_DAPPR_PLAN ?? "",
                    appr = t.OVC_PUR_APPR_PLAN
                };
            string OVC_PUR_DAPPR_PLAN_WITH = "";
            foreach (var item in queryDPPR_PLAN)
            {
                OVC_PUR_DAPPR_PLAN_WITH += item.date + item.appr + ";\n";
            }

            PdfPCell cell6_4 = new PdfPCell(new Phrase(OVC_PUR_DAPPR_PLAN_WITH, ChFont));
            table2.AddCell(cell6_4);
            PdfPCell cell7_1 = new PdfPCell(new Phrase("請\n\n求\n\n事\n\n項", ChFont));//*************
            cell7_1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell7_1.HorizontalAlignment = 1;
            table2.AddCell(cell7_1);
            PdfPCell cell7_2 = new PdfPCell();
            string ovcIkind = "";
            switch (t1301.OVC_PUR_AGENCY)
            {
                case "B":
                case "L":
                case "P":
                    ovcIkind = "M3";
                    break;
                case "M":
                case "S":
                    ovcIkind = "F3";
                    break;
                default:
                    ovcIkind = "W3";
                    break;
            }
            //請求事項
            var RequestMemo =
                from t in mpms.TBM1220_1
                join t12202 in mpms.TBM1220_2 on t.OVC_IKIND equals t12202.OVC_IKIND
                where t.OVC_IKIND.StartsWith(ovcIkind) && t.OVC_PURCH.Equals(strPurchNum)
                orderby t.OVC_IKIND
                select new
                {
                    title = t12202.OVC_MEMO_NAME,
                    memo = t.OVC_MEMO
                };
            foreach (var item in RequestMemo)
            {
                Phrase c7_2 = new Phrase(item.title + item.memo + "\n", ChFont_memo);
                cell7_2.AddElement(c7_2);
            }
            cell7_2.Colspan = 7;
            table2.AddCell(cell7_2);


            PdfPCell cell8_1 = new PdfPCell(new Phrase("附件", ChFont));
            cell8_1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell8_1.HorizontalAlignment = 1;
            table2.AddCell(cell8_1);

            PdfPCell cell8_2 = new PdfPCell(new Phrase(GetAttached("M"), ChFont));
            cell8_2.Colspan = 7;
            table2.AddCell(cell8_2);


            PdfPCell cell9_1 = new PdfPCell(new Phrase("備考", ChFont));
            cell9_1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell9_1.HorizontalAlignment = 1;
            table2.AddCell(cell9_1);
            PdfPCell cell9_2 = new PdfPCell();
            //備考
            switch (t1301.OVC_PUR_AGENCY)
            {
                case "B":
                case "L":
                case "P":
                    ovcIkind = "M4";
                    break;
                case "M":
                case "S":
                    ovcIkind = "F4";
                    break;
                default:
                    ovcIkind = "W4";
                    break;
            }
            var markMemo =
               from t in mpms.TBM1220_1
               join t12202 in mpms.TBM1220_2 on t.OVC_IKIND equals t12202.OVC_IKIND
               where t.OVC_IKIND.StartsWith(ovcIkind) && t.OVC_PURCH.Equals(strPurchNum)
               orderby t.OVC_IKIND
               select new
               {
                   title = t12202.OVC_MEMO_NAME,
                   memo = t.OVC_MEMO
               };
            foreach (var item in markMemo)
            {
                Phrase c9_2 = new Phrase(item.title + item.memo + "\n", ChFont_memo);
                cell9_2.AddElement(c9_2);
            }
            cell9_2.Colspan = 7;
            table2.AddCell(cell9_2);


            string toPhares = "";
            if (Session["userid"] != null)
            {
                string strUSER_ID = Session["userid"].ToString();
                string userid = Session["userid"].ToString();
                var queryUserDept = gm.ACCOUNTs.Where(o => o.USER_ID.Equals(userid)).FirstOrDefault();
                if (queryUserDept.DEPT_SN.ToString() == t1301.OVC_PUR_SECTION.ToString())
                    toPhares = "此致";
                else
                    toPhares = "謹呈";
            }
            PdfPCell cell10 = new PdfPCell();
            Chunk glue2 = new Chunk(new VerticalPositionMark());
            iTextSharp.text.Paragraph p2 = new iTextSharp.text.Paragraph(toPhares + "\n", ChFont);
            p2.FirstLineIndent = 25;
            p2.Add(t1301.OVC_PUR_NSECTION);
            p2.Add(new Chunk(glue2));
            p2.Add("主官" + t1301.OVC_SECTION_CHIEF);//主官
            cell10.AddElement(p2);
            cell10.Colspan = 8;
            table2.AddCell(cell10);
            PdfPCell cell11_1 = new PdfPCell(new Phrase("審查意見", ChFont));
            cell11_1.Colspan = 6;
            cell11_1.HorizontalAlignment = Element.ALIGN_CENTER;
            table2.AddCell(cell11_1);
            iTextSharp.text.Paragraph p11_2 = new iTextSharp.text.Paragraph();
            Phrase c11_2_1 = new Phrase("承辦人", ChFont);
            p11_2.Leading = 60; //設定行距（每行之間的距離） 
            Phrase c11_2_2 = new Phrase("核稿人\n", ChFont);
            p11_2.Add(c11_2_2);
            Phrase c11_2_3 = new Phrase("核判人\n", ChFont);
            p11_2.Add(c11_2_3);
            p11_2.Add("\n");

            PdfPCell cell11_2 = new PdfPCell();
            cell11_2.AddElement(c11_2_1);
            cell11_2.AddElement(p11_2);
            cell11_2.Colspan = 2;
            cell11_2.Rowspan = 9;
            table2.AddCell(cell11_2);

            PdfPCell cell12_1 = new PdfPCell(new Phrase("會辦單位", ChFont));
            cell12_1.Colspan = 2;
            cell12_1.Rowspan = 1;
            cell12_1.HorizontalAlignment = Element.ALIGN_CENTER;
            table2.AddCell(cell12_1);

            PdfPCell cell12_2 = new PdfPCell(new Phrase("會辦單位", ChFont));
            cell12_2.Colspan = 2;
            cell12_2.Rowspan = 1;
            cell12_2.HorizontalAlignment = Element.ALIGN_CENTER;
            table2.AddCell(cell12_2);

            PdfPCell cell12_3 = new PdfPCell(new Phrase("承辦單位", ChFont));
            cell12_3.Colspan = 2;
            cell12_3.Rowspan = 1;
            cell12_3.HorizontalAlignment = Element.ALIGN_CENTER;
            table2.AddCell(cell12_3);

            PdfPCell cell13_1 = new PdfPCell(new Phrase());
            cell13_1.Colspan = 2;
            cell13_1.Rowspan = 7;
            table2.AddCell(cell13_1);
            PdfPCell cell13_2 = new PdfPCell(new Phrase());
            cell13_2.Colspan = 2;
            cell13_2.Rowspan = 7;
            table2.AddCell(cell13_2);
            PdfPCell cell13_3 = new PdfPCell(new Phrase());
            cell13_3.Colspan = 2;
            cell13_3.Rowspan = 7;
            table2.AddCell(cell13_3);
            doc1.Add(table2);
            #endregion


            //page3
            #region page3
            doc1.NewPage();
            table = new PdfPTable(new float[] { 1, 3, 1, 1, 1, 3, 2, 4 });
            table.TotalWidth = 500f;
            table.LockedWidth = true;
            header = new PdfPCell();
            glue = new Chunk(new VerticalPositionMark());

            p = new iTextSharp.text.Paragraph(t1301.OVC_PUR_NSECTION + angcy + "物資核定書", ChFont);//標題
            p.Add(new Chunk(glue));//有沒有其他寫法  無法垂直置中
            p.Add(strp2);
            p.Add(new Chunk(glue));
            p.Add(t1301.OVC_PUR_APPROVE);
            p.Alignment = Element.ALIGN_MIDDLE;
            header.AddElement(p);
            header.Colspan = 8;
            table.AddCell(header);
            table.AddCell(cell2_1);
            table.AddCell(cell2_2);
            cell2_3 = new PdfPCell(new Phrase("申購日期及文號", ChFont));
            cell2_3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2_3.HorizontalAlignment = 1;
            table.AddCell(cell2_3);
            table.AddCell(cell2_4);
            doc1.Add(table);
            //table2
            table2 = new PdfPTable(new float[] { 0.7f, 3, 1, 1, 1, 2, 1.5f, 4 });
            table2.TotalWidth = 500f;
            table2.LockedWidth = true;

            table2.AddCell(cell3_1);
            table2.AddCell(cell3_2);
            table2.AddCell(cell3_3);
            table2.AddCell(cell3_4);
            table2.AddCell(cell3_5);
            table2.AddCell(cell3_6);
            table2.AddCell(cell3_7);
            table2.AddCell(cell4_1);
            table2.AddCell(cell4_2);
            table2.AddCell(cell4_3);
            table2.AddCell(cell5_2);
            table2.AddCell(cell5_3);
            table2.AddCell(cell6_1);
            table2.AddCell(cell6_2);
            table2.AddCell(cell6_3);
            table2.AddCell(cell6_4);
            table2.AddCell(cell7_1);
            table2.AddCell(cell7_2);
            table2.AddCell(cell8_1);
            table2.AddCell(cell8_2);
            table2.AddCell(cell9_1);
            table2.AddCell(cell9_2);
            PdfPCell cell10_1 = new PdfPCell(new Phrase("審查意見", ChFont));
            table2.AddCell(cell10_1);
            var queryComment = mpms.TBM1202.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_CHECK_OK.Equals("Y")).Select(o => o.OVC_APPROVE_COMMENT).FirstOrDefault();
            PdfPCell cell10_2 = new PdfPCell(new Phrase(queryComment, ChFont_memo));
            cell10_2.Colspan = 7;
            table2.AddCell(cell10_2);
            PdfPCell cell11 = new PdfPCell();
            p2 = new iTextSharp.text.Paragraph(new Phrase("此令\n", ChFont));
            p2.FirstLineIndent = -5;
            p2.Add(t1301.OVC_AGNT_IN + "                  ");//空格是否有更好的方法
            p2.IndentationLeft = 30;
            p2.Add("主官");
            cell11.AddElement(p2);
            cell11.Colspan = 8;
            cell11.PaddingTop = -4;
            cell11.FixedHeight = 80;
            table2.AddCell(cell11);
            doc1.Add(table2);
            #endregion

            //pag4
            #region page4
            doc1.NewPage();
            table = new PdfPTable(new float[] { 1, 3, 1, 1, 1, 3, 2, 4 });
            table.TotalWidth = 500f;
            table.LockedWidth = true;
            header = new PdfPCell();
            glue = new Chunk(new VerticalPositionMark());
            p = new iTextSharp.text.Paragraph(t1301.OVC_PUR_NSECTION + angcy + "物資核定書", ChFont);//標題
            p.Add(new Chunk(glue));//有沒有其他寫法  無法垂直置中
            p.Add(strp2);
            p.Add(new Chunk(glue));
            p.Add(t1301.OVC_PUR_APPROVE);
            p.Alignment = Element.ALIGN_MIDDLE;
            header.AddElement(p);
            header.Colspan = 8;
            table.AddCell(header);
            table.AddCell(cell2_1);
            table.AddCell(cell2_2);
            cell2_3 = new PdfPCell(new Phrase("申購日期及文號", ChFont));
            cell2_3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2_3.HorizontalAlignment = 1;
            table.AddCell(cell2_3);
            table.AddCell(cell2_4);
            doc1.Add(table);
            //table2
            table2 = new PdfPTable(new float[] { 0.7f, 3, 1, 1, 1, 2, 1.5f, 4 });
            table2.TotalWidth = 500f;
            table2.LockedWidth = true;

            table2.AddCell(cell3_1);
            table2.AddCell(cell3_2);
            table2.AddCell(cell3_3);
            table2.AddCell(cell3_4);
            table2.AddCell(cell3_5);
            table2.AddCell(cell3_6);
            table2.AddCell(cell3_7);
            table2.AddCell(cell4_1);
            table2.AddCell(cell4_2);
            table2.AddCell(cell4_3);
            table2.AddCell(cell5_2);
            table2.AddCell(cell5_3);
            table2.AddCell(cell6_1);
            table2.AddCell(cell6_2);
            table2.AddCell(cell6_3);
            table2.AddCell(cell6_4);
            table2.AddCell(cell7_1);
            table2.AddCell(cell7_2);
            table2.AddCell(cell8_1);
            table2.AddCell(cell8_2);
            table2.AddCell(cell9_1);
            table2.AddCell(cell9_2);
            table2.AddCell(cell10_1);
            table2.AddCell(cell10_2);
            PdfPCell cell11_3 = new PdfPCell();
            p2 = new iTextSharp.text.Paragraph(new Phrase("此令\n", ChFont));
            p2.FirstLineIndent = -5;
            p2.Add(t1301.OVC_PUR_NSECTION + "                  ");//空格是否有更好的方法
            p2.IndentationLeft = 30;
            p2.Add("主官");
            cell11_3.AddElement(p2);
            cell11_3.Colspan = 8;
            cell11_3.PaddingTop = -4;
            cell11_3.FixedHeight = 80;
            table2.AddCell(cell11_3);
            doc1.Add(table2);
            #endregion

            doc1.Close();
            Response.Clear();//瀏覽器上顯示
            Response.AddHeader("Content-disposition", "attachment;filename=" + strPurchNum + "物資申請書.pdf");
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.Flush();
            Response.End();
        }
        #endregion

        #region PDF列印-物資申請書外購
        public void MaterialApplication_ExportToWord()
        {
            string ovcIkind = "";
            string ovcIkind_2 = "";
            int year = 0;
            string date = "";
            string wordfilepath = "";
            string path = "";
            var purch = strPurchNum;
            string purAgency = "";
            var query =
                from tbm1301 in mpms.TBM1301
                where tbm1301.OVC_PURCH.Equals(purch)
                select new
                {
                    OVC_PURCH_KIND = tbm1301.OVC_PURCH_KIND,
                    OVC_PUR_DAPPROVE = tbm1301.OVC_PUR_DAPPROVE,
                    OVC_PUR_APPROVE = tbm1301.OVC_PUR_APPROVE,
                    OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                    OVC_DPROPOSE = tbm1301.OVC_DPROPOSE,
                    OVC_PROPOSE = tbm1301.OVC_PROPOSE,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    OVC_PUR_IPURCH_ENG = tbm1301.OVC_PUR_IPURCH_ENG,
                    ONB_PUR_BUDGET_NT = tbm1301.ONB_PUR_BUDGET_NT,
                    OVC_SECTION_CHIEF = tbm1301.OVC_SECTION_CHIEF,
                    OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY
                };
            foreach (var q in query)
            {
                purAgency = q.OVC_PUR_AGENCY;
                if (q.OVC_PURCH_KIND == "2" && q.OVC_PUR_AGENCY != "F" && q.OVC_PUR_AGENCY != "W")
                    path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/MaterialApplication_Kind2.docx");
                else
                    path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/MaterialApplication.docx");
            }
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/MaterialApplication.pdf";
                    foreach (var q in query)
                    {
                        if (q.OVC_PUR_NSECTION != null)
                            doc.ReplaceText("[$OVC_PUR_NSECTION$]", q.OVC_PUR_NSECTION, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OVC_PUR_NSECTION$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        var queryPurch =
                            from tbm1301 in mpms.TBM1301_PLAN
                            where tbm1301.OVC_PURCH.Equals(purch)
                            select new
                            {
                                OVC_PURCH = tbm1301.OVC_PURCH,
                                OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY
                            };
                        foreach (var qu in queryPurch)
                        {
                            doc.ReplaceText("[$PURCH$]", qu.OVC_PURCH + qu.OVC_PUR_AGENCY, false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$Barcode$]", qu.OVC_PURCH, false, System.Text.RegularExpressions.RegexOptions.None);//條碼字形IDAutomationHC39M
                        }
                        if (q.OVC_PURCH_KIND == "1")
                        {
                            doc.ReplaceText("[$OVC_PURCH_KIND$]", "內", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_2$]", GetAttached("M", "6"), false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_3$]", "採購計畫清單三份", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_4$]", "採購計畫清單一份", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_5$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_6$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_7$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_8$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_9$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_10$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                        {
                            doc.ReplaceText("[$OVC_PURCH_KIND$]", "外", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_2$]", GetAttached("M", "9"), false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_2$]", "採購輸出入計畫清單九份", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_4$]", "採購計畫清單三份", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_5$]", "採購輸出入計畫清單一份", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_6$]", "採購輸出入計畫清單一份", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_7$]", "採購輸出入計畫清單一份", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_8$]", "採購輸出入計畫清單二份", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_9$]", "採購輸出入計畫清單一份", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_ATTACH_NAME_10$]", "採購輸出入計畫清單一份", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        if (q.OVC_PUR_DAPPROVE != null)
                        {
                            year = int.Parse(q.OVC_PUR_DAPPROVE.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_PUR_DAPPROVE.Substring(5, 2) + "月" + q.OVC_PUR_DAPPROVE.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_PUR_DAPPROVE$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$OVC_PUR_DAPPROVE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_PUR_APPROVE != null)
                            doc.ReplaceText("[$OVC_PUR_APPROVE$]", q.OVC_PUR_APPROVE, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OVC_PUR_APPROVE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_PUR_NSECTION != null)
                            doc.ReplaceText("[$OVC_PUR_NSECTION$]", q.OVC_PUR_NSECTION, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OVC_PUR_NSECTION$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DPROPOSE != null)
                        {
                            year = int.Parse(q.OVC_DPROPOSE.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DPROPOSE.Substring(5, 2) + "月" + q.OVC_DPROPOSE.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DPROPOSE$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$OVC_DPROPOSE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_PROPOSE != null)
                            doc.ReplaceText("[$OVC_PROPOSE$]", q.OVC_PROPOSE, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OVC_PROPOSE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_PUR_IPURCH != null)
                            doc.ReplaceText("[$OVC_PUR_IPURCH$]", q.OVC_PUR_IPURCH, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OVC_PUR_IPURCH$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_PUR_IPURCH_ENG != null)
                            doc.ReplaceText("[$OVC_PUR_IPURCH_ENG$]", "(" + q.OVC_PUR_IPURCH_ENG + ")", false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OVC_PUR_IPURCH_ENG$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_PUR_BUDGET_NT != null)
                        {
                            string money = EastAsiaNumericFormatter.FormatWithCulture("Lc", q.ONB_PUR_BUDGET_NT, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                            doc.ReplaceText("[$ONB_PUR_BUDGET_NT$]", money, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$ONB_PUR_BUDGET_NT$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_SECTION_CHIEF != null)
                            doc.ReplaceText("[$OVC_SECTION_CHIEF$]", q.OVC_SECTION_CHIEF, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OVC_SECTION_CHIEF$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                        switch (q.OVC_PUR_AGENCY)
                        {
                            case "B":
                            case "L":
                            case "P":
                                ovcIkind = "M3";
                                ovcIkind_2 = "M4";
                                break;
                            case "M":
                            case "S":
                                ovcIkind = "F3";
                                ovcIkind_2 = "F4";
                                break;
                            default:
                                ovcIkind = "W3";
                                ovcIkind_2 = "W4";
                                break;
                        }
                    }
                    year = int.Parse(DateTime.Now.ToString("yyyyMMdd").Substring(0, 4)) - 1911;
                    date = year.ToString() + "." + DateTime.Now.Month + "." + DateTime.Now.Day;
                    doc.ReplaceText("[$DATE$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$TIME$]", DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString(), false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryCurrent =
                        from tbm1407 in mpms.TBM1407
                        join tbm1301 in mpms.TBM1301
                            on tbm1407.OVC_PHR_ID equals tbm1301.OVC_PUR_CURRENT
                        where tbm1301.OVC_PURCH.Equals(purch)
                        where tbm1407.OVC_PHR_CATE.Equals("B0")
                        select tbm1407.OVC_PHR_DESC;
                    foreach (var qu in queryCurrent)
                    {
                        if (qu != null)
                            doc.ReplaceText("[$OVC_PUR_CURRENT$]", qu, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_PUR_CURRENT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var query1118 =
                        from tbm1118 in mpms.TBM1118
                        where tbm1118.OVC_PURCH.Equals(purch)
                        select new
                        {
                            OVC_ISOURCE = tbm1118.OVC_ISOURCE,
                            OVC_POI_IBDG = tbm1118.OVC_POI_IBDG,
                            OVC_PJNAME = tbm1118.OVC_PJNAME
                        };
                    foreach (var q in query1118)
                    {
                        if (q.OVC_ISOURCE != null)
                            doc.ReplaceText("[$OVC_ISOURCE$]", q.OVC_ISOURCE, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_POI_IBDG != null)
                            doc.ReplaceText("[$OVC_POI_IBDG$]", q.OVC_POI_IBDG, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_PJNAME != null)
                            doc.ReplaceText("[$OVC_PJNAME$]", q.OVC_PJNAME, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_ISOURCE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_POI_IBDG$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_PJNAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var query1231 =
                        from tbm1231 in mpms.TBM1231
                        where tbm1231.OVC_PURCH.Equals(purch)
                        select tbm1231.OVC_PUR_APPR_PLAN;
                    foreach (var q in query1231)
                    {
                        if (q != null)
                            doc.ReplaceText("[$OVC_PUR_APPR_PLAN$]", q, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_PUR_APPR_PLAN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var query1202 =
                        from tbm1202 in mpms.TBM1202
                        where tbm1202.OVC_PURCH.Equals(purch)
                        select tbm1202.OVC_APPROVE_COMMENT;
                    foreach (var q in query1202)
                    {
                        if (q != null)
                            doc.ReplaceText("[$OVC_APPROVE_COMMENT$]", q, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_INTEGRATED_REASON$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    table_AddRow(doc, "Page1_1", ovcIkind);
                    table_AddRow(doc, "Page2_1", ovcIkind);
                    table_AddRow(doc, "Page3_1", ovcIkind);
                    table_AddRow(doc, "Page4_1", ovcIkind);
                    table_AddRow(doc, "Page5_1", ovcIkind);
                    table_AddRow(doc, "Page6_1", ovcIkind);
                    table_AddRow(doc, "Page7_1", ovcIkind);
                    table_AddRow(doc, "Page8_1", ovcIkind);
                    table_AddRow(doc, "Page9_1", ovcIkind);
                    table_AddRow(doc, "Page10_1", ovcIkind);
                    table2_AddRow(doc, "Page1_2", 6, ovcIkind_2);
                    table2_AddRow(doc, "Page2_2", 4, ovcIkind_2);
                    table2_AddRow(doc, "Page3_2", 4, ovcIkind_2);
                    table2_AddRow(doc, "Page4_2", 4, ovcIkind_2);
                    table2_AddRow(doc, "Page5_2", 4, ovcIkind_2);
                    table2_AddRow(doc, "Page6_2", 4, ovcIkind_2);
                    table2_AddRow(doc, "Page7_2", 4, ovcIkind_2);
                    table2_AddRow(doc, "Page8_2", 4, ovcIkind_2);
                    table2_AddRow(doc, "Page9_2", 4, ovcIkind_2);
                    table2_AddRow(doc, "Page10_2", 4, ovcIkind_2);
                    doc.ReplaceText("[$MEMO_M11_N$][$MEMO_M11_Y$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$MEMO_M21_N$][$MEMO_M21_Y$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_APPROVE_COMMENT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "Tempprint/b.docx");
                }
                buffer = ms.ToArray();
            }
            string path_d = Path.Combine(Request.PhysicalApplicationPath, "Tempprint/b.docx");
            WordcvDdf(path_d, wordfilepath);
            FileInfo file = new FileInfo(wordfilepath);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + strPurchNum + purAgency + "物資申請書.pdf");
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            Response.End();
        }
        #endregion

        #region 表單newRow
        private string GetAttached(string kind, string page)
        {
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            //附件內容
            var queryFile =
                mpms.TBM1119.AsEnumerable()
                .Where(o => o.OVC_PURCH.Equals(purch) && o.OVC_IKIND.Equals(kind))
                .OrderByDescending(o => o.OVC_ATTACH_NAME.Split('.')[0].Length)
                .OrderBy(o => o.OVC_ATTACH_NAME.Split('.')[0]);
            string[] arrFileName = new string[queryFile.Count()];
            int counter = 0;
            foreach (var row in queryFile)
            {
                if (row.OVC_ATTACH_NAME == "採購計畫清單")
                {
                    if (row.ONB_PAGES == 0)
                        arrFileName[counter] = row.OVC_ATTACH_NAME + page + "份";
                    else
                        arrFileName[counter] = row.OVC_ATTACH_NAME + page + "份(" + row.ONB_PAGES.ToString() + "頁)";
                }
                else
                {
                    if (row.ONB_PAGES == 0)
                        arrFileName[counter] = row.OVC_ATTACH_NAME + row.ONB_QTY.ToString() + "份";
                    else
                        arrFileName[counter] = row.OVC_ATTACH_NAME + row.ONB_QTY.ToString() + "份(" + row.ONB_PAGES.ToString() + "頁)";
                }
                counter++;
            }
            string strFileName = string.Join("、", arrFileName);
            return strFileName;
        }
        private void table_AddRow(DocX doc, string Page, string ovcIkind)
        {
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            var groceryListTable = doc.Tables.FirstOrDefault(table => table.TableCaption == Page);
            if (groceryListTable != null)
            {
                var rowPattern = groceryListTable.Rows[groceryListTable.Rows.Count - 5];
                var rowPattern_N = groceryListTable.Rows[groceryListTable.Rows.Count - 4];
                var rowPattern_Y = groceryListTable.Rows[groceryListTable.Rows.Count - 3];
                rowPattern_N.Remove();
                rowPattern_Y.Remove();
                var query1220 =
                    from tbm1220_1 in mpms.TBM1220_1
                    where tbm1220_1.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_IKIND = tbm1220_1.OVC_IKIND,
                        OVC_MEMO = tbm1220_1.OVC_MEMO,
                        OVC_STANDARD = tbm1220_1.OVC_STANDARD
                    };
                int count = 0;
                foreach (var q in query1220)
                {
                    if (q.OVC_IKIND.Contains(ovcIkind) == true && q.OVC_MEMO != null)
                    {
                        count++;
                        if (count == 1)
                        {
                            if (q.OVC_STANDARD == "N")
                            {
                                rowPattern.ReplaceText("[$MEMO_W3_FN$]", "(" + count + ")" + q.OVC_MEMO, false, System.Text.RegularExpressions.RegexOptions.None);
                                rowPattern.ReplaceText("[$MEMO_W3_FY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            else
                            {
                                rowPattern.ReplaceText("[$MEMO_W3_FY$]", "(" + count + ")" + q.OVC_MEMO, false, System.Text.RegularExpressions.RegexOptions.None);
                                rowPattern.ReplaceText("[$MEMO_W3_FN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                        }
                        else
                        {
                            if (q.OVC_STANDARD == "N")
                            {
                                var newItem = groceryListTable.InsertRow(rowPattern_N, groceryListTable.RowCount - 2);
                                newItem.ReplaceText("[$MEMO_W3_N$]", "(" + count + ")" + q.OVC_MEMO.ToString());
                            }
                            else
                            {
                                var newItem = groceryListTable.InsertRow(rowPattern_Y, groceryListTable.RowCount - 2);
                                newItem.ReplaceText("[$MEMO_W3_Y$]", "(" + count + ")" + q.OVC_MEMO.ToString());
                            }
                        }
                    }
                }
                rowPattern.ReplaceText("[$MEMO_W3_FN$][$MEMO_W3_FY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                var query1220_2 =
                    from tbm1220_1 in mpms.TBM1220_1
                    where tbm1220_1.OVC_PURCH.Equals(purch)
                    where tbm1220_1.OVC_IKIND.Equals("M11")
                    select new
                    {
                        OVC_IKIND = tbm1220_1.OVC_IKIND,
                        OVC_MEMO = tbm1220_1.OVC_MEMO,
                        OVC_STANDARD = tbm1220_1.OVC_STANDARD
                    };
                foreach (var q in query1220_2)
                {
                    if (q.OVC_MEMO != null)
                    {
                        if (q.OVC_STANDARD == "N")
                        {
                            doc.ReplaceText("[$MEMO_M11_N$]", q.OVC_MEMO, false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MEMO_M11_Y$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                        {
                            doc.ReplaceText("[$MEMO_M11_Y$]", q.OVC_MEMO, false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MEMO_M11_N$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                }
                doc.ReplaceText("[$MEMO_M11_N$][$MEMO_ M11_Y$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                var query1220_3 =
                    from tbm1220_1 in mpms.TBM1220_1
                    where tbm1220_1.OVC_PURCH.Equals(purch)
                    where tbm1220_1.OVC_IKIND.Equals("M21")
                    select new
                    {
                        OVC_IKIND = tbm1220_1.OVC_IKIND,
                        OVC_MEMO = tbm1220_1.OVC_MEMO,
                        OVC_STANDARD = tbm1220_1.OVC_STANDARD
                    };
                foreach (var q in query1220_3)
                {
                    if (q.OVC_STANDARD == "N")
                    {
                        doc.ReplaceText("[$MEMO_M21_N$]", q.OVC_MEMO, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$MEMO_M21_Y$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    else
                    {
                        doc.ReplaceText("[$MEMO_M21_Y$]", q.OVC_MEMO, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$MEMO_M21_N$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                }
                doc.ReplaceText("[$MEMO_M21_N$][$MEMO_ M21_Y$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
            }
        }
        private void table2_AddRow(DocX doc, string Page, int rowCount, string ovcIkind)
        {
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            var groceryListTable = doc.Tables.FirstOrDefault(table => table.TableCaption == Page);
            if (groceryListTable != null)
            {
                var rowPattern = groceryListTable.Rows[0];
                var rowPattern_N = groceryListTable.Rows[1];
                var rowPattern_Y = groceryListTable.Rows[2];
                rowPattern_N.Remove();
                rowPattern_Y.Remove();
                var query1220 =
                    from tbm1220_1 in mpms.TBM1220_1
                    where tbm1220_1.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_IKIND = tbm1220_1.OVC_IKIND,
                        OVC_MEMO = tbm1220_1.OVC_MEMO,
                        OVC_STANDARD = tbm1220_1.OVC_STANDARD
                    };
                int count = 0;
                foreach (var q in query1220)
                {
                    if (q.OVC_IKIND.Contains(ovcIkind) == true && q.OVC_MEMO != null)
                    {
                        count++;
                        if (count == 1)
                        {
                            if (q.OVC_STANDARD == "N")
                            {
                                rowPattern.ReplaceText("[$MEMO_W4_FN$]", "(" + count + ")" + q.OVC_MEMO, false, System.Text.RegularExpressions.RegexOptions.None);
                                rowPattern.ReplaceText("[$MEMO_W4_FY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            else
                            {
                                rowPattern.ReplaceText("[$MEMO_W4_FY$]", "(" + count + ")" + q.OVC_MEMO, false, System.Text.RegularExpressions.RegexOptions.None);
                                rowPattern.ReplaceText("[$MEMO_W4_FN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                        }
                        else
                        {
                            if (q.OVC_STANDARD == "N")
                            {
                                var newItem = groceryListTable.InsertRow(rowPattern_N, groceryListTable.RowCount - rowCount);
                                newItem.ReplaceText("[$MEMO_W4_N$]", "(" + count + ")" + q.OVC_MEMO.ToString());
                            }
                            else
                            {
                                var newItem = groceryListTable.InsertRow(rowPattern_Y, groceryListTable.RowCount - rowCount);
                                newItem.ReplaceText("[$MEMO_W4_Y$]", "(" + count + ")" + q.OVC_MEMO.ToString());
                            }
                        }
                    }
                }
                rowPattern.ReplaceText("[$MEMO_W4_FN$][$MEMO_W4_FY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
            }
        }

       
        #endregion

        #region 轉換至pdf
        static void WordcvDdf(string args, string wordfilepath)
        {
            // word 檔案位置
            string sourcedocx = args;
            // PDF 儲存位置
            // string targetpdf =  @"C:\Users\linon\Downloads\ddd.pdf";

            //建立 word application instance
            Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
            //開啟 word 檔案
            var wordDocument = appWord.Documents.Open(sourcedocx);

            //匯出為 pdf
            wordDocument.ExportAsFixedFormat(wordfilepath, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);

            //關閉 word 檔
            wordDocument.Close();
            //結束 word
            appWord.Quit();
        }


        #endregion
    }
}