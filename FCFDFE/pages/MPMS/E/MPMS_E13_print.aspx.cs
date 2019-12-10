using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.IO;
using Xceed.Words.NET;
using Microsoft.Office.Interop.Word;
using System.Web;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E13_print : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["rowtext"] != null)
                {
                    Label1.Text = Session["rowtext"].ToString();
                    string purch = Label1.Text.Substring(0, 7);
                    var n = mpms.TBM1301.Where(o => o.OVC_PURCH.Equals(purch)).FirstOrDefault();
                    Label2.Text = n.OVC_PUR_IPURCH;
                    dataImport();
                }
                else
                    FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
            }
        }

        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            string send_url;
            if (Session["E15"] != null)
                send_url = "~/pages/MPMS/E/MPMS_E16.aspx";
            else
                send_url = "~/pages/MPMS/E/MPMS_E13.aspx";
            Response.Redirect(send_url);
        }

        #region 列印btn
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            PrinterServlet2_ExportToWord();

            //Print();//無預覽列印

            PreviewPrint();//預覽列印
        }
        #endregion

        #region DataImport
        private void dataImport()
        {
            var pur = Session["rowtext"].ToString().Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            var query =
                from tbmreceive_check in mpms.TBMRECEIVE_CHECK
                join tbmreceive_check_item in mpms.TBMRECEIVE_CHECK_ITEM on tbmreceive_check.OVC_PURCH equals tbmreceive_check_item.OVC_PURCH
                where tbmreceive_check.OVC_PURCH.Equals(pur)
                where tbmreceive_check.OVC_PURCH_6.Equals(purch_6)
                where tbmreceive_check.OVC_PURCH_6.Equals(tbmreceive_check_item.OVC_PURCH_6)
                select new
                {
                    OVC_ITEM_NAME = tbmreceive_check_item.OVC_ITEM_NAME,
                    OVC_RESULT = tbmreceive_check_item.OVC_RESULT
                };
            foreach (var q in query)
            {
                for (int i = 1; i < 9; i++)
                {
                    string lab = "labItem1_" + i;
                    string lab2 = "labCheck1_" + i;
                    Label label = (Label)tbItem_1.FindControl(lab);
                    Label label2 = (Label)tbItem_1.FindControl(lab2);
                    if (q.OVC_ITEM_NAME == label.Text)
                        label2.Text = q.OVC_RESULT;
                }
                for (int i = 1; i < 7; i++)
                {
                    string lab = "labItem2_" + i;
                    string lab2 = "labCheck2_" + i;
                    Label label = (Label)tbItem_1.FindControl(lab);
                    Label label2 = (Label)tbItem_1.FindControl(lab2);
                    if (q.OVC_ITEM_NAME == label.Text)
                        label2.Text = q.OVC_RESULT;
                }
                for (int i = 1; i < 14; i++)
                {
                    string lab = "labItem3_" + i;
                    string lab2 = "labCheck3_" + i;
                    Label label = (Label)tbItem_1.FindControl(lab);
                    Label label2 = (Label)tbItem_1.FindControl(lab2);
                    if (q.OVC_ITEM_NAME == label.Text)
                        label2.Text = q.OVC_RESULT;
                }
                for (int i = 1; i < 8; i++)
                {
                    string lab = "labItem4_" + i;
                    string lab2 = "labCheck4_" + i;
                    Label label = (Label)tbItem_2.FindControl(lab);
                    Label label2 = (Label)tbItem_2.FindControl(lab2);
                    if (q.OVC_ITEM_NAME == label.Text)
                        label2.Text = q.OVC_RESULT;
                }
                for (int i = 1; i < 4; i++)
                {
                    string lab = "labItem5_" + i;
                    string lab2 = "labCheck5_" + i;
                    Label label = (Label)tbItem_2.FindControl(lab);
                    Label label2 = (Label)tbItem_2.FindControl(lab2);
                    if (q.OVC_ITEM_NAME == label.Text)
                        label2.Text = q.OVC_RESULT;
                }
                for (int i = 1; i < 2; i++)
                {
                    string lab = "labItem6_" + i;
                    string lab2 = "labCheck6_" + i;
                    Label label = (Label)tbItem_2.FindControl(lab);
                    Label label2 = (Label)tbItem_2.FindControl(lab2);
                    if (q.OVC_ITEM_NAME == label.Text)
                        label2.Text = q.OVC_RESULT;
                }
                var queryBid =
                        from tbmreceove_bid in mpms.TBMRECEIVE_BID
                        join tbmreceive_check in mpms.TBMRECEIVE_CHECK on tbmreceove_bid.OVC_PURCH equals tbmreceive_check.OVC_PURCH
                        where tbmreceove_bid.OVC_PURCH.Equals(pur)
                        select new
                        {
                            OVC_DO_NAME_RESULT = tbmreceove_bid.OVC_DO_NAME_RESULT,
                            OVC_REJECT_REASON_DO = tbmreceove_bid.OVC_REJECT_REASON_DO,
                            OVC_DSEND = tbmreceive_check.OVC_DSEND,
                            OVC_DRECEIVE = tbmreceive_check.OVC_DRECEIVE,
                            OVC_DO_NAME = tbmreceive_check.OVC_DO_NAME,
                            OVC_DO_DAPPROVE = tbmreceive_check.OVC_DO_DAPPROVE
                        };
                foreach (var qu in queryBid)
                {
                    if (qu.OVC_DO_NAME_RESULT == "1")
                        labOVC_DO_NAME_RESULT.Text = "";
                    else if (qu.OVC_DO_NAME_RESULT == "2")
                        labOVC_DO_NAME_RESULT.Text = qu.OVC_REJECT_REASON_DO;
                    else if (qu.OVC_DO_NAME_RESULT == "3")
                        labOVC_DO_NAME_RESULT.Text = "先行接管補請修正";
                }
            }
        }
        #endregion

        #region 檢查項目表
        void PrinterServlet2_ExportToWord()
        {
            string path = "";
            var userunit = Session["userunit"].ToString();
            var purch = Label1.Text.Substring(0, 7);
            var rowven = Session["rowven"].ToString();
            var name = Session["username"].ToString();
            string purch_6 = Session["purch_6"].ToString();
            path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/ItemCheckE13.docx");
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    doc.ReplaceText("[$OVC_PURCH$]", Session["rowtext"].ToString(), false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$USER_NAME$]", Session["username"].ToString(), false, System.Text.RegularExpressions.RegexOptions.None);

                    var queryNSECTION = mpms.TBM1301.Where(o => o.OVC_PURCH.Equals(purch)).FirstOrDefault();
                    doc.ReplaceText("[$OVC_PUR_IPURCH$]", queryNSECTION != null ? queryNSECTION.OVC_PUR_IPURCH : "", false, System.Text.RegularExpressions.RegexOptions.None);

                    var queryItem =
                        from item in mpms.TBMRECEIVE_CHECK_ITEM
                        where item.OVC_PURCH.Equals(purch)
                        where item.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            ONB_ITEM = item.ONB_ITEM,
                            OVC_ITEM_NAME = item.OVC_ITEM_NAME,
                            OVC_RESULT = item.OVC_RESULT
                        };
                    if (queryItem.Count() < 1)
                        return;
                    foreach (var q in queryItem)
                    {
                        if (q.ONB_ITEM == 1001)
                            doc.ReplaceText("[$C1_1$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 1002)
                            doc.ReplaceText("[$C1_2$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 1003)
                            doc.ReplaceText("[$C1_3$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 1004)
                            doc.ReplaceText("[$C1_4$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 1005)
                            doc.ReplaceText("[$C1_5$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 1006)
                            doc.ReplaceText("[$C1_6$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 1007)
                            doc.ReplaceText("[$C1_7$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 1008)
                            doc.ReplaceText("[$C1_8$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 2001)
                            doc.ReplaceText("[$C2_1$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 2002)
                            doc.ReplaceText("[$C2_2$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 2003)
                            doc.ReplaceText("[$C2_3$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 2004)
                            doc.ReplaceText("[$C2_4$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 2005)
                            doc.ReplaceText("[$C2_5$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 2006)
                            doc.ReplaceText("[$C2_6$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 3001)
                            doc.ReplaceText("[$C3_1$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 3002)  
                            doc.ReplaceText("[$C3_2$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 3003)    
                            doc.ReplaceText("[$C3_3$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 3004)       
                            doc.ReplaceText("[$C3_4$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 3005)   
                            doc.ReplaceText("[$C3_5$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 3006)     
                            doc.ReplaceText("[$C3_6$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 3007)   
                            doc.ReplaceText("[$C3_7$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 3008)    
                            doc.ReplaceText("[$C3_8$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 3009)    
                            doc.ReplaceText("[$C3_9$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 3010)
                            doc.ReplaceText("[$C3_10$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 3011)     
                            doc.ReplaceText("[$C3_11$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 3012)   
                            doc.ReplaceText("[$C3_12$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 3013)       
                            doc.ReplaceText("[$C3_13$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 4001)
                            doc.ReplaceText("[$C4_1$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 4002)     
                            doc.ReplaceText("[$C4_2$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 4003)  
                            doc.ReplaceText("[$C4_3$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 4004)      
                            doc.ReplaceText("[$C4_4$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 4005)      
                            doc.ReplaceText("[$C4_5$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 4006)      
                            doc.ReplaceText("[$C4_6$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 4007)    
                            doc.ReplaceText("[$C4_7$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 5001)
                            doc.ReplaceText("[$C5_1$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 5002)
                            doc.ReplaceText("[$C5_2$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 5003)
                            doc.ReplaceText("[$C5_3$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 6001)
                            doc.ReplaceText("[$C6_1$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ITEM == 7001)
                            doc.ReplaceText("[$C7_1$]", q.OVC_RESULT, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$C7_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    var queryBid =
                        from tbmreceove_bid in mpms.TBMRECEIVE_BID
                        join tbmreceive_check in mpms.TBMRECEIVE_CHECK on tbmreceove_bid.OVC_PURCH equals tbmreceive_check.OVC_PURCH
                        where tbmreceove_bid.OVC_PURCH.Equals(purch)
                        where tbmreceive_check.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            OVC_DO_NAME_RESULT = tbmreceove_bid.OVC_DO_NAME_RESULT,
                            OVC_REJECT_REASON_DO = tbmreceove_bid.OVC_REJECT_REASON_DO,
                            OVC_DSEND = tbmreceive_check.OVC_DSEND,
                            OVC_DRECEIVE = tbmreceive_check.OVC_DRECEIVE,
                            OVC_DO_NAME = tbmreceive_check.OVC_DO_NAME,
                            OVC_DO_DAPPROVE = tbmreceive_check.OVC_DO_DAPPROVE
                        };
                    foreach (var qu in queryBid)
                    {
                        if (qu.OVC_DO_NAME_RESULT == "1")
                            doc.ReplaceText("[$OVC_DO_NAME_RESULT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        else if (qu.OVC_DO_NAME_RESULT == "2")
                            doc.ReplaceText("[$OVC_DO_NAME_RESULT$]", qu.OVC_REJECT_REASON_DO, false, System.Text.RegularExpressions.RegexOptions.None);
                        else if (qu.OVC_DO_NAME_RESULT == "3")
                            doc.ReplaceText("[$OVC_DO_NAME_RESULT$]", "先行接管補請修正", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_DO_NAME_RESULT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$TODAY$]", DateTime.Now.ToString("yyyy/MM/dd"), false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/CheckItem_Temp.docx");
                }
            }
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/CheckItem_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/ItemCheckE13.pdf";
            WordcvDdf(path_temp, wordfilepath);
        }
        #endregion

        #region 列印(無預覽)
        void Print()
        {
            string path_d = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/CheckItem_Temp.docx");
            object oMissing = System.Reflection.Missing.Value;
            Application wa = new Application();
            Document wd = new Document();
            //            wa.Visible=true; 
            //wa.ActivePrinter = @"Microsoft Office Document Image Writer";

            object file = path_d;
            wd = wa.Documents.Add(ref file, ref oMissing, ref oMissing, ref oMissing);

            //print it 
            object xcopies = 1;
            object oFalse = false;
            object oTrue = true;

            wd.PrintOut(ref oFalse, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref xcopies, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing);

            wa.Quit(ref oFalse, ref oMissing, ref oMissing);
        }
        #endregion

        #region 預覽列印(下載PDF)
        void PreviewPrint()
        {
            string path_d = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/ItemCheckE13.pdf");

            //建立新的PDF
            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4);
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(Request.PhysicalApplicationPath + "pages/MPMS/E/檢查項目表.pdf", FileMode.Create));
            doc.Open();

            //打開此PDF就會啟動AdobeReader列印的指令
            iTextSharp.text.pdf.PdfAction jAction = iTextSharp.text.pdf.PdfAction.JavaScript("this.print(true);\r", writer);
            writer.AddJavaScript(jAction);

            var reader = new iTextSharp.text.pdf.PdfReader(path_d);

            //將reader內容寫到writer中
            var imp = writer.GetImportedPage(reader, 1);
            var imp2 = writer.GetImportedPage(reader, 2);
            iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
            iTextSharp.awt.geom.AffineTransform af = new iTextSharp.awt.geom.AffineTransform();
            
            cb.AddTemplate(imp, af);
            doc.NewPage();
            cb.AddTemplate(imp2, af);
            doc.Close();
            reader.Dispose();

            //將iframe的來源帶入此PDF
            frame1.Attributes["src"] = "檢查項目表.pdf";
        }
        #endregion

        #region WordToPDF
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
            wordDocument.ExportAsFixedFormat(wordfilepath, WdExportFormat.wdExportFormatPDF);

            //關閉 word 檔
            wordDocument.Close();
            //結束 word
            appWord.Quit();
        }
        #endregion
    }
}