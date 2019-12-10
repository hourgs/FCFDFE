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
using System.IO;
using NPOI.XWPF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;
using System.Web;
using TemplateEngine.Docx;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D32 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (Request.QueryString["TYPE"] == null || Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_PURCH_5"] == null || Request.QueryString["ONB_GROUP"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    if (!IsPostBack && IsOVC_DO_NAME())
                    {
                        string strTYPE_url = Request.QueryString["TYPE"];
                        string strTYPE = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strTYPE_url));
                        if (strTYPE == "awardofbid")
                        {
                            string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
                            string strOVC_PURCH_5_url = Request.QueryString["OVC_PURCH_5"];
                            string strONB_GROUP_url = Request.QueryString["ONB_GROUP"];
                            string strONB_TIMES_url = Request.QueryString["ONB_TIMES"];
                            string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
                            string strOVC_PURCH_5 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_5_url));
                            string strONB_GROUP = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strONB_GROUP_url));
                            string strONB_TIMES = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strONB_TIMES_url));
                            short onbgroup = Convert.ToInt16(strONB_GROUP);

                            ViewState["strOVC_PURCH"] = strOVC_PURCH;
                            ViewState["strOVC_PURCH_5"] = strOVC_PURCH_5;
                            ViewState["strONB_GROUP"] = strONB_GROUP;
                            ViewState["strONB_TIMES"] = strONB_TIMES;
                            dataImport(strOVC_PURCH, strOVC_PURCH_5, onbgroup);
                            dataGV(strOVC_PURCH, strOVC_PURCH_5, onbgroup);
                        }
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

        private void dataImport(string strOVC_PURCH, string strOVC_PURCH_5, short onbgroup)
        {
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            var queryresult =
                (from tbmresult in mpms.TBMBID_RESULT
                 where tbmresult.OVC_PURCH == strOVC_PURCH
                 where tbmresult.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmresult.ONB_GROUP == onbgroup
                 select tbmresult).FirstOrDefault();
            var query1303 =
                (from tbm1303 in mpms.TBM1303
                 where tbm1303.OVC_PURCH == strOVC_PURCH
                 where tbm1303.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbm1303.ONB_GROUP == onbgroup
                 select tbm1303).FirstOrDefault();
            string strpurass = query1301.OVC_PUR_ASS_VEN_CODE;
            var query1407C7 =
                (from tbm1407 in mpms.TBM1407
                 where tbm1407.OVC_PHR_CATE == "C7"
                 where tbm1407.OVC_PHR_ID == strpurass
                 select tbm1407).FirstOrDefault();
            string strbidtimes = query1301.OVC_BID_TIMES;
            var query1407TG =
                (from tbm1407 in mpms.TBM1407
                 where tbm1407.OVC_PHR_CATE == "TG"
                 where tbm1407.OVC_PHR_ID == strbidtimes
                 select tbm1407).FirstOrDefault();
            string strovcbid = query1301.OVC_BID;
            var query1407M3 =
                (from tbm1407 in mpms.TBM1407
                 where tbm1407.OVC_PHR_CATE == "M3"
                 where tbm1407.OVC_PHR_ID == strovcbid
                 select tbm1407).FirstOrDefault();
            string strresult = query1303.OVC_RESULT;
            var query1407A8 =
                (from tbm1407 in mpms.TBM1407
                 where tbm1407.OVC_PHR_CATE == "A8"
                 where tbm1407.OVC_PHR_ID == strresult
                 select tbm1407).FirstOrDefault();
            if (query1301 != null)
            {
                lblOVC_PURCH.Text = query1301.OVC_PURCH;
                lblOVC_PUR_IPURCH.Text = query1301.OVC_PUR_IPURCH;
            }
            if (query1303 != null)
            {
                lblOVC_DOPEN_tw.Text = ToTaiwanCalendar(query1303.OVC_DOPEN) + query1303.OVC_OPEN_HOUR + "時" + query1303.OVC_OPEN_MIN + "分";
                lblOVC_DOPEN.Text = query1303.OVC_DOPEN;
            }
            lblOVC_BID_TIMES.Text = query1407TG == null ? "" : query1407TG.OVC_PHR_DESC;
            lblOVC_BID.Text = query1407M3 == null ? "" : query1407M3.OVC_PHR_DESC;
            lblOVC_PUR_ASS_VEN_CODE.Text = query1407C7 == null ? "" : query1407C7.OVC_PHR_DESC;
            lblOVC_RESULT.Text = query1407A8 == null ? "" : query1407A8.OVC_PHR_DESC;
            lblType.Text = query1407A8 == null ? "" : query1407A8.OVC_PHR_DESC;
            lblOVC_VEN_TITLE.Text = queryresult == null ? "" : queryresult.OVC_VENDORS_NAME;
        }

        private void dataGV(string strOVC_PURCH, string strOVC_PURCH_5, short onbgroup)
        {
            string strOVC_VENDORS_NAME = lblOVC_VEN_TITLE.Text;
            DataTable dt = new DataTable();

            var query =
                (from tbm1313 in mpms.TBM1313
                 where tbm1313.OVC_PURCH == strOVC_PURCH
                 where tbm1313.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbm1313.OVC_VEN_TITLE != strOVC_VENDORS_NAME
                 //where tbm1313.ONB_GROUP == onbgroup
                 join tbmnotice in mpms.TBMRESULT_NOTICE.OrderByDescending(o => o.OVC_NOTICE_TIME)
                 on new { tbm1313.OVC_PURCH, tbm1313.OVC_PURCH_5, tbm1313.OVC_VEN_CST } equals new { tbmnotice.OVC_PURCH, tbmnotice.OVC_PURCH_5, tbmnotice.OVC_VEN_CST }
                 into tb
                 let tbmnotice = tb.FirstOrDefault()
                 //group new { t1313 = tbm1313, tnotice = tbmnotice }
                 //    by tbmnotice.OVC_PURCH into tbmnoticenew
                 //let item = tbmnoticenew.First()
                 //select new
                 //{
                 //    OVC_PURCH = item.t1313.OVC_PURCH,
                 //    OVC_PURCH_5 = item.t1313.OVC_PURCH_5,
                 //    ONB_GROUP = item.t1313.ONB_GROUP,
                 //    OVC_VEN_TITLE = item.t1313.OVC_VEN_TITLE,
                 //    OVC_NOTICE_TIME = item.tnotice.OVC_NOTICE_TIME,
                 //    OVC_NAME = item.tnotice.OVC_NAME
                 //});
                 select new
                 {
                     OVC_PURCH = tbm1313.OVC_PURCH,
                     OVC_PURCH_5 = tbm1313.OVC_PURCH_5,
                     ONB_GROUP = tbm1313.ONB_GROUP,
                     OVC_VEN_TITLE = tbm1313.OVC_VEN_TITLE,
                     OVC_NOTICE_TIME = tbmnotice.OVC_NOTICE_TIME,
                     OVC_VEN_CST = tbm1313.OVC_VEN_CST,
                     OVC_NAME = tbmnotice.OVC_NAME,
                     OVC_VEN_ADDRESS = tbmnotice.OVC_VEN_ADDRESS,
                     OVC_DOPEN = tbm1313.OVC_DOPEN
                 });

            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["Status"] = FCommon.GridView_dataImport(GV_info, dt);
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

        protected void btnSAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < GV_info.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)GV_info.Rows[i].FindControl("CheckBox1");
                cb.Checked = true;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < GV_info.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)GV_info.Rows[i].FindControl("CheckBox1");
                cb.Checked = false;
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string Rcompany = "";
            for (int i = 0; i <= GV_info.Rows.Count-1; i++)
            {
                bool cbx = false;
                cbx = ((CheckBox)GV_info.Rows[i].FindControl("CheckBox1")).Checked;
                if (cbx)
                {
                    string str1 = GV_info.Rows[i].Cells[3].Text.Trim().ToString()??"";
                    string str2 = GV_info.Rows[i].Cells[5].Text.Trim().ToString()??"";
                    if (str2 != "&nbsp;")
                    {
                        if (Rcompany == "")
                            Rcompany = str1 + "(" + str2 + ")";
                        else Rcompany += "," + str1 + "(" + str2 + ")";
                    }
                    else
                    {
                        if (Rcompany == "")
                            Rcompany = str1 + "()";
                        else Rcompany += "," + str1 + "()";
                    }
                }
            }


            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            string userid = Session["userid"].ToString();
            var queryuser = gm.ACCOUNTs.Where(o => o.USER_ID == userid).FirstOrDefault();
            string ovcpurch = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
            string date = ToTaiwanCalendar(DateTime.Now.ToShortDateString());


            string outPutfilePath = "";

            //1.複製至目標路徑

            string fileName = "開標結果通知函(稿)預覽列印.docx";
            string TempName = "";
            string filePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + fileName));


            //2.設定檔案內容

            //FieldContent部分

            TempName = DateTime.Now.ToString("yyyy-mm-dd") + ".docx";
            outPutfilePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + TempName));
            File.Copy(filePath, outPutfilePath);
            var valuesToFill = new TemplateEngine.Docx.Content();
            valuesToFill.Fields.Add(new FieldContent("time", date));
            valuesToFill.Fields.Add(new FieldContent("p_name", query1301.OVC_PUR_IPURCH));
            valuesToFill.Fields.Add(new FieldContent("Pno", ViewState["strOVC_PURCH"].ToString()));
            valuesToFill.Fields.Add(new FieldContent("user", queryuser.USER_NAME));
            valuesToFill.Fields.Add(new FieldContent("tel", queryuser.IUSER_PHONE));
            valuesToFill.Fields.Add(new FieldContent("P_unit", Rcompany));
            //儲存變更 C:\Users\linon\Desktop\FCFDFE0619\FCFDFE\FCFDFE
            using (TemplateProcessor outputDocument = new TemplateProcessor(outPutfilePath).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }


            FileInfo file = new FileInfo(outPutfilePath);
            string pdfPath = file.DirectoryName + ".pdf";
            WordcvDdf(outPutfilePath, pdfPath);
            File.Delete(outPutfilePath);
            //匯出PDF檔供下載
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + "開標結果通知函.pdf");
            Response.ContentType = "application/octet-stream";
            //Response.BinaryWrite(buffer);
            Response.WriteFile(pdfPath);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            File.Delete(outPutfilePath);
            File.Delete(pdfPath);
            Response.End();
        }

        protected void btnPrint_odt_Click(object sender, EventArgs e)
        {
            string Rcompany = "";
            for (int i = 0; i <= GV_info.Rows.Count - 1; i++)
            {
                bool cbx = false;
                cbx = ((CheckBox)GV_info.Rows[i].FindControl("CheckBox1")).Checked;
                if (cbx)
                {
                    string str1 = GV_info.Rows[i].Cells[3].Text.Trim().ToString() ?? "";
                    string str2 = GV_info.Rows[i].Cells[5].Text.Trim().ToString() ?? "";
                    if (str2 != "&nbsp;")
                    {
                        if (Rcompany == "")
                            Rcompany = str1 + "(" + str2 + ")";
                        else Rcompany += "," + str1 + "(" + str2 + ")";
                    }
                    else
                    {
                        if (Rcompany == "")
                            Rcompany = str1 + "()";
                        else Rcompany += "," + str1 + "()";
                    }
                }
            }


            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            string userid = Session["userid"].ToString();
            var queryuser = gm.ACCOUNTs.Where(o => o.USER_ID == userid).FirstOrDefault();
            string ovcpurch = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
            string date = ToTaiwanCalendar(DateTime.Now.ToShortDateString());


            string outPutfilePath = "";

            //1.複製至目標路徑

            string fileName = "開標結果通知函(稿)預覽列印.docx";
            string TempName = "";
            string filePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + fileName));


            //2.設定檔案內容

            //FieldContent部分

            TempName = DateTime.Now.ToString("yyyy-mm-dd") + ".docx";
            outPutfilePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + TempName));
            File.Copy(filePath, outPutfilePath);
            var valuesToFill = new TemplateEngine.Docx.Content();
            valuesToFill.Fields.Add(new FieldContent("time", date));
            valuesToFill.Fields.Add(new FieldContent("p_name", query1301.OVC_PUR_IPURCH));
            valuesToFill.Fields.Add(new FieldContent("Pno", ViewState["strOVC_PURCH"].ToString()));
            valuesToFill.Fields.Add(new FieldContent("user", queryuser.USER_NAME));
            valuesToFill.Fields.Add(new FieldContent("tel", queryuser.IUSER_PHONE));
            valuesToFill.Fields.Add(new FieldContent("P_unit", Rcompany));
            //儲存變更 C:\Users\linon\Desktop\FCFDFE0619\FCFDFE\FCFDFE
            using (TemplateProcessor outputDocument = new TemplateProcessor(outPutfilePath).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }

            string filepath = outPutfilePath;
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/Print_odt.odt";
            string FileName = strOVC_PURCH + "開標結果通知函.odt";
            FCommon.WordToOdt(this, filepath, filetemp, FileName);
        }

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
        protected void btnReturnMod_Click(object sender, EventArgs e)
        {
            //Response.Redirect("MPMS_D1B.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString() + "&OVC_DOPEN=" + lblOVC_DOPEN.Text + "&ONB_GROUP=" + ViewState["strONB_GROUP"].ToString());
            string send_url = "MPMS_D18_7.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_DOPEN=" + lblOVC_DOPEN.Text +
                                    "&ONB_TIMES=" + ViewState["strONB_TIMES"] + "&ONB_GROUP=" + ViewState["strONB_GROUP"];
            Response.Redirect(send_url);
        }

        protected void btnReturnS_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_D1B.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString());
        }

        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_D14_1.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString());
        }

        protected void GV_info_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
            {
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            }
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}