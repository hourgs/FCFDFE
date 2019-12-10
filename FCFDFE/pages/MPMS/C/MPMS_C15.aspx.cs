            using System;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MPMSModel;
using System.Linq;
using FCFDFE.Entity.GMModel;
using FCFDFE.Content;
using System.Data.Entity;
using TemplateEngine.Docx;
using System.IO;
using System.Globalization;

namespace FCFDFE.pages.MPMS.C
{
    public partial class MPMS_C15 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        string strPurchNum = "",strDRECIVE="";
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
                    numCheckTimes = byte.Parse(Request.QueryString["numCheckTimes"]);
                    strDRECIVE = Request.QueryString["DRecive"];
                    if (!IsPostBack)
                    {
                        var agency = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault(); ;
                        lblOVC_PURCH.Text = strPurchNum + agency.OVC_PUR_AGENCY;
                        lblONB_CHECK_TIMES.Text = numCheckTimes.ToString();
                        DataImport(numCheckTimes);
                        GV_Import();

                        var queryStatus = mpms.TBMSTATUS.Where(t => t.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                        if (queryStatus != null)
                        {
                            btnSave.CssClass += " disabled";
                        }
                    }

                }
            }
        }
        #region click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            GV_Import();
        }
        protected void btnClear_Command(object sender, CommandEventArgs e)
        {
            string rdoID = e.CommandArgument.ToString();
            RadioButtonList rdo = this.Master.FindControl("MainContent").FindControl(rdoID) as RadioButtonList;
            rdo.ClearSelection();
        }
        protected void btn_FIRST_PRINT_Click(object sender, EventArgs e)
        {
            string filePath = "";
            string fileType = "";
            filePath = PrintCheckOKPDF();
            fileType = "初審會辦單.docx";
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

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string url = "~/pages/MPMS/C/MPMS_C14.aspx?PurchNum=" + strPurchNum;
            Response.Redirect(url);
        }

        #endregion
        protected void GV_alreadych_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {

                GV_alreadych.UseAccessibleHeader = true;
                GV_alreadych.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        #region 副程式
        private void SaveData()
        {
            string strSaveMessage = "";
            string strDelMessage = "";
            string[] idField = { "00","01","02","03","04","05","06" };
            foreach(string id in idField)
            {
                string rdoID = "rdo_K5" + id;
                string lblID = "lblK5" + id;
                RadioButtonList rdo = this.Master.FindControl("MainContent").FindControl(rdoID) as RadioButtonList;
                Label lbl = this.Master.FindControl("MainContent").FindControl(lblID) as Label;
                if (rdo.SelectedValue.Equals("1"))
                {
                    var tbm1202_1Exist = mpms.TBM1202_1.Where(o => o.OVC_PURCH.Equals(strPurchNum)
                                                        && o.ONB_CHECK_TIMES == numCheckTimes
                                                        && o.OVC_DRECEIVE.Equals(strDRECIVE)
                                                        && o.OVC_AUDIT_UNIT.Equals(id));
                    if (!tbm1202_1Exist.Any())
                    {
                        TBM1202_1 tbm1202_1 = new TBM1202_1();
                        tbm1202_1.OVC_PURCH = strPurchNum;
                        tbm1202_1.OVC_DRECEIVE = strDRECIVE;
                        tbm1202_1.OVC_CHECK_UNIT = GetUserDept();
                        tbm1202_1.ONB_CHECK_TIMES = numCheckTimes;
                        tbm1202_1.OVC_AUDIT_UNIT = id;
                        mpms.TBM1202_1.Add(tbm1202_1);
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                             , tbm1202_1.GetType().Name.ToString(), this, "新增");
                        strSaveMessage += "<p> 新增" + lbl.Text + " <p>";

                    }

                }
                else if(rdo.SelectedValue.Equals("2"))
                {
                    TBM1202_1 tbm1202_1 = new TBM1202_1();
                    tbm1202_1 = mpms.TBM1202_1.Where(o => o.OVC_PURCH.Equals(strPurchNum) 
                                                        && o.ONB_CHECK_TIMES == numCheckTimes
                                                        && o.OVC_DRECEIVE.Equals(strDRECIVE)
                                                        && o.OVC_AUDIT_UNIT.Equals(id))
                                              .FirstOrDefault();
                    if (tbm1202_1 != null)
                    {
                        mpms.Entry(tbm1202_1).State = EntityState.Deleted;
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , tbm1202_1.GetType().Name.ToString(), this, "刪除");
                        strDelMessage += "<p> 刪除" + lbl.Text + " <p>";
                    }
                }
                
            }
            if (string.IsNullOrEmpty(strSaveMessage) && string.IsNullOrEmpty(strDelMessage))
            {
                FCommon.AlertShow(PnMessage, "info", "系統訊息", "沒有新增或刪除任何部門！");
            }
            else
            {
                FCommon.AlertShow(PnMessage, "success", "系統訊息", strSaveMessage + "\r\n" + strDelMessage);
            }
        }

        private string GetUserDept()
        {
            if(Session["userid"] != null)
            {
                string userID = Session["userid"].ToString();
                var queryDept = gm.ACCOUNTs.Where(o => o.USER_ID.Equals(userID)).Select(o => o.DEPT_SN).FirstOrDefault();
                return queryDept;
            }
            else
            {
                return string.Empty;
            }
        }

        private void GV_Import()
        {
            string[] field = { "OVC_USR_ID" };
            var query12021 =
                from t12021 in mpms.TBM1202_1
                join t1407 in mpms.TBM1407 on t12021.OVC_AUDIT_UNIT equals t1407.OVC_PHR_ID
                where t12021.OVC_PURCH.Equals(strPurchNum) && t12021.ONB_CHECK_TIMES == numCheckTimes && t1407.OVC_PHR_CATE.Equals("K5")
                select new { t1407.OVC_USR_ID };


            DataTable dt = CommonStatic.LinqQueryToDataTable(query12021);
            hasRows = FCommon.GridView_dataImport(GV_alreadych, dt, field);
        }

        private void DataImport(byte checkTimes)
        {
           
           
            var queryComment =
                from tComment in mpms.TBM1202_COMMENT
                where tComment.OVC_PURCH.Equals(strPurchNum) && tComment.ONB_CHECK_TIMES == checkTimes
                group tComment by tComment.OVC_AUDIT_UNIT into g
                select g.Key;
            
            foreach(var item in queryComment)
            {
                string rdoID = "rdo_K5" + item;
                string btnID = "btnClearK5" + item;
                string lblID = "lblK5" + item;
                RadioButtonList rdo = this.Master.FindControl("MainContent").FindControl(rdoID) as RadioButtonList;
                Button btn = this.Master.FindControl("MainContent").FindControl(btnID) as Button;
                Label lbl = this.Master.FindControl("MainContent").FindControl(lblID) as Label;
                rdo.Visible = false;
                btn.Visible = false;
                lbl.Text += "，已有審查意見。";
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

       
        #endregion

        #region 列印確認審會辦單
        private string PrintCheckOKPDF()
        {
            string fileName = "初審會辦單.docx";
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
            int count = 1;
            foreach (var item in queryDept)
            {
                if (count <= 6)
                {
                    string deptNum = "dept" + count.ToString();
                    valuesToFill.Fields.Add(new FieldContent(deptNum, "聯審小組("+item.OVC_USR_ID+")"));
                    if (count >= 2)
                    {
                        string memoNum = "memo" + count.ToString();
                        valuesToFill.Fields.Add(new FieldContent(memoNum, "詳第一受會單位送會目的"));
                    }
                    count++;
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
    }
}