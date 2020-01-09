using System;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Entity.GMModel;
using System.Linq;
using FCFDFE.Content;
using System.Data.Entity;
using System.Globalization;
using System.Data.Entity.Infrastructure;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.IO;
using TemplateEngine.Docx;
using Xceed.Words.NET;
using Microsoft.International.Formatters;
using System.Web;

namespace FCFDFE.pages.MPMS.C
{
    public class Watermark : PdfPageEventHelper
    {

        PdfContentByte cb;
        GMEntities gm = new GMEntities();
        public Watermark()
        {

        }

        public string strPurchNum { get; set; }

        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {

            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H
               , BaseFont.NOT_EMBEDDED);//設定字型
            iTextSharp.text.Font ChFont = new iTextSharp.text.Font(bfChinese, 24, iTextSharp.text.Font.NORMAL, new BaseColor(255, 0, 0, 60));
            var query1301 = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            string strMark = query1301.OVC_PUR_NSECTION + strPurchNum + query1301.OVC_PUR_AGENCY;
            ColumnText.ShowTextAligned(
                  writer.DirectContentUnder,
                  Element.ALIGN_CENTER, new Phrase(strMark + "(" + writer.PageNumber + ")", ChFont),
                  300, 400, 55
                );


            cb = writer.DirectContent;


            DateTime PrintTime = DateTime.Now;
            string chFontPath = "c:\\windows\\fonts\\KAIU.TTF";
            BaseFont chBaseFont = BaseFont.CreateFont(chFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            int pageN = writer.PageNumber+1;
            string text = "第" + pageN + "聯";
            string text2 = text.Replace("1", "一")
            .Replace("2", "二").Replace("3", "三")
            .Replace("4", "四").Replace("5", "五")
            .Replace("6", "六").Replace("7", "七")
            .Replace("8", "八").Replace("9", "九");

            Rectangle pageSize = document.PageSize;
            cb.SetRGBColorFill(0, 0, 0);

            cb.BeginText();
            cb.SetFontAndSize(chBaseFont, 12);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, text2, pageSize.GetLeft(42), pageSize.GetTop(30), 0);
            cb.SetTextMatrix(pageSize.GetLeft(100), pageSize.GetBottom(30));
            Barcode39 code39 = new Barcode39();
            code39.Code = strPurchNum + query1301.OVC_PUR_AGENCY;
            iTextSharp.text.Image img = code39.CreateImageWithBarcode(cb, null, null);
            img.SetAbsolutePosition(pageSize.GetLeft(10), pageSize.GetBottom(20));
            document.Add(img);


            cb.EndText();
            BaseFont bf = null;
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);



            cb.BeginText();
            cb.SetFontAndSize(chBaseFont, 16);
            string strcb = "";
            switch (writer.PageNumber)
            {
                case 1:
                    strcb = "申購單位存查";
                    break;
                case 2:
                    strcb = "核定單位存查";
                    break;
                case 3:
                    strcb = "核交招標約定單位";
                    break;
                case 4:
                    strcb = "核覆原申購單位";
                    break;
            }
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, strcb, pageSize.GetRight(40), pageSize.GetBottom(40), 0);
            cb.SetFontAndSize(chBaseFont, 8);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, PrintTime.ToString("yyyy/MM/dd HH:mm:ss") + "(" + text2 + ")", pageSize.GetRight(40), pageSize.GetBottom(25), 0);

            cb.EndText();

        }

    }

    public class WatermarkList : PdfPageEventHelper
    {
        GMEntities gm = new GMEntities();
        PdfTemplate template;
        BaseFont bf = null;
        PdfContentByte cb;
        /** The header text. */
        public string Header { get; set; }
        public string strPurchNum { get; set; }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            template = writer.DirectContent.CreateTemplate(30, 16);
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
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            Rectangle pageSize = document.PageSize;
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型

            cb = writer.DirectContent;
            cb.SetRGBColorFill(100, 100, 100);

            string text = "Page " + writer.CurrentPageNumber + " of ";

            PdfContentByte under = writer.DirectContentUnder;
            under.BeginText();
            under.SetRGBColorFill(0xFF, 0x88, 0x88);
            under.SetFontAndSize(bfChinese, 32);
            var query1301 = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            string strMark = query1301.OVC_PUR_NSECTION + strPurchNum + query1301.OVC_PUR_AGENCY;
            under.ShowTextAligned(PdfContentByte.ALIGN_CENTER, strMark + "(" + writer.CurrentPageNumber.ToString() + ")", 300, 400, 55);
            under.EndText();

            //BARCODE39
            Barcode39 codeEAN = new Barcode39();
            codeEAN.Code = strPurchNum + query1301.OVC_PUR_AGENCY;
            iTextSharp.text.Image img = codeEAN.CreateImageWithBarcode(cb, null, null);
            img.SetAbsolutePosition(pageSize.GetRight(180), pageSize.GetTop(60));
            cb.BeginText();
            cb.AddImage(img);

            cb.SetFontAndSize(bfChinese, 18f);
            string kind = "";
            if (query1301.OVC_PURCH_KIND.Equals("1"))
                kind = "國內";
            else
                kind = "國外";
            string yearsub = strPurchNum.Substring(2, 2);
            string year = "";
            if (Int32.Parse(yearsub) >= 70)
                year = yearsub;
            else
                year = "1" + yearsub;

            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, year + "年度" + query1301.OVC_PUR_NSECTION + kind + "財務勞務採購計畫清單", pageSize.GetRight(300), pageSize.GetTop(80), 0);

            cb.SetFontAndSize(bfChinese, 12f);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "中華民國" + GetTaiwanDate(query1301.OVC_DPROPOSE.ToString()) + query1301.OVC_PROPOSE, pageSize.GetRight(150), pageSize.GetTop(95), 0);

            cb.SetFontAndSize(bfChinese, 12f);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "第" + writer.CurrentPageNumber + "頁", pageSize.GetRight(30), pageSize.GetTop(110), 0);

            cb.SetFontAndSize(bfChinese, 12f);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "中華民國" + GetTaiwanDate(query1301.OVC_PUR_DAPPROVE.ToString()) + query1301.OVC_PUR_APPROVE, pageSize.GetLeft(30), pageSize.GetBottom(40), 0);
            cb.AddTemplate(template, pageSize.GetRight(30), pageSize.GetBottom(40));

            cb.SetFontAndSize(bf, 11);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pageSize.GetRight(30), pageSize.GetBottom(40), 0);
            cb.EndText();

        }
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            base.OnCloseDocument(writer, document);
            template.BeginText();
            template.SetFontAndSize(bf, 11);
            template.SetTextMatrix(0, 0);
            template.ShowText("" + (writer.PageNumber));
            template.EndText();

        }
    }

    public class PDF : PdfPageEventHelper
    {
        PdfTemplate template;
        BaseFont bf = null;
        PdfContentByte cb;
        /** The header text. */
        public string Header { get; set; }
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            template = writer.DirectContent.CreateTemplate(30, 16);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            Rectangle pageSize = document.PageSize;
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型

            cb = writer.DirectContent;
            cb.SetRGBColorFill(100, 100, 100);

            string text = "Page " + writer.CurrentPageNumber + " of ";

            cb.BeginText();

            cb.SetFontAndSize(bfChinese, 18f);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "預算年度分配明細表 ", pageSize.GetRight(300), pageSize.GetTop(80), 0);

            cb.SetFontAndSize(bfChinese, 12f);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "列印日期:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), pageSize.GetRight(100), pageSize.GetTop(95), 0);

            cb.SetFontAndSize(bfChinese, 12f);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "第" + writer.CurrentPageNumber + "頁", pageSize.GetRight(30), pageSize.GetTop(110), 0);

            cb.AddTemplate(template, pageSize.GetRight(30), pageSize.GetBottom(40));

            cb.SetFontAndSize(bf, 11);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pageSize.GetRight(30), pageSize.GetBottom(40), 0);
            cb.EndText();

        }
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            base.OnCloseDocument(writer, document);
            template.BeginText();
            template.SetFontAndSize(bf, 11);
            template.SetTextMatrix(0, 0);
            template.ShowText("" + (writer.PageNumber));
            template.EndText();

        }
    }

    public partial class MPMS_C14 : System.Web.UI.Page
    {
        bool hasRows = false,isUpload;
        public string strMenuName = "", strMenuNameItem = "";
        string strPurchNum = "";
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this, out isUpload))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!string.IsNullOrEmpty(Request.QueryString["PurchNum"]))
                {
                    strPurchNum = Request.QueryString["PurchNum"];
                    if (!IsPostBack)
                    {
                        GetUserInfo();
                        AttchedDataImport();
                        DataImport();
                        QueryPlanDataImport();
                        GetFileInfo();
                        AddWindowOpen();
                        FCommon.Controls_Attributes("readonly", "true", txtOVC_DRECEIVE);
                        FCommon.Controls_Attributes("readonly", "true", txtOVC_DANNOUNCE);

                        var queryStatus = mpms.TBMSTATUS.Where(t => t.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                        if (queryStatus != null)
                        {
                            btnSave.CssClass += " disabled";
                            btnNew.CssClass += " disabled";
                            btnUpload.CssClass += " disabled";
                        }
                    }
                }
                else
                {
                    Response.Redirect("MPMS_C13.aspx");
                }
            }
        }
        
        #region PreRender
        protected void GV_OVC_Alreadyupdate_PreRender(object sender, EventArgs e)
        {
            //if (hasRows)
            //{

            //    GV_OVC_Alreadyupdate.UseAccessibleHeader = true;
            //    GV_OVC_Alreadyupdate.HeaderRow.TableSection = TableRowSection.TableHeader;
            //}
        }
        protected void GV_C_Alreadyupdate_PreRender(object sender, EventArgs e)
        {
            //if (hasRows)
            //{

            //    GV_C_Alreadyupdate.UseAccessibleHeader = true;
            //    GV_C_Alreadyupdate.HeaderRow.TableSection = TableRowSection.TableHeader;
            //}
        }
        protected void GV_History_PreRender(object sender, EventArgs e)
        {
            //if (hasRows)
            //{

            //    GV_History.UseAccessibleHeader = true;
            //    GV_History.HeaderRow.TableSection = TableRowSection.TableHeader;
            //}
        }
        protected void GV_Query_PLAN_PreRender(object sender, EventArgs e)
        {
            
            //if (hasRows)
            //{

            //    GV_Query_PLAN.UseAccessibleHeader = true;
            //    GV_Query_PLAN.HeaderRow.TableSection = TableRowSection.TableHeader;
            //}
        }
        #endregion

        #region onClick
        protected void rdo_OVC_READ_BID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = rdo_OVC_READ_BID.SelectedValue;
            if (selectedValue.Equals("1"))
            {
                divDateTime.Visible = true;
            }
            else
            {
                divDateTime.Visible = false;
            }
        }
        
        protected void btnNew_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            if (string.IsNullOrEmpty(txtONB_CHECK_TIMES.Text))
                strMessage += "<p> 審查次數不得為空! <p>";
            if (string.IsNullOrEmpty(txtOVC_DRECEIVE.Text))
                strMessage += "<p> 日期不得為空! <p>";
            if (string.IsNullOrEmpty(strMessage))
            {
                string date = txtOVC_DRECEIVE.Text;
                string userName, detpSN;
                GetUserInfo();
                TBM1202 tbm1202 = new TBM1202();
                userName = ViewState["USER_NAME"].ToString();
                detpSN = ViewState["DEPT_SN"].ToString();
                tbm1202.OVC_PURCH = strPurchNum;
                tbm1202.ONB_CHECK_TIMES = Convert.ToByte(txtONB_CHECK_TIMES.Text);
                tbm1202.OVC_DRECEIVE = date;
                tbm1202.OVC_CHECK_UNIT = detpSN;
                tbm1202.OVC_CHECKER = userName;
                tbm1202.OVC_ASSIGNER = userName;
                try
                {
                    mpms.TBM1202.Add(tbm1202);
                    mpms.SaveChanges();
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功");
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , tbm1202.GetType().Name.ToString(), this, "新增");
                }
                catch (DbUpdateException ex)
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "新增失敗");
                }
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
            QueryPlanDataImport();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/C/PurchCheck/" + strPurchNum));
            if (Directory.Exists(serverDir))
            {

            }
            else
            {
                Directory.CreateDirectory(serverDir);
            }
            FileUpload(serverDir);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strFAC = "";
            string strOPEN = "";
            string dept = ViewState["DEPT_SN"].ToString();
            string userName = ViewState["USER_NAME"].ToString();
            if (rdoFAC.SelectedIndex != -1 && rdoFAC.SelectedItem.Text.Equals("是"))
            {
                strFAC = "Y";
            }
            else if(rdoFAC.SelectedIndex != -1 && rdoFAC.SelectedItem.Text.Equals("否"))
            {
                strFAC = "N";
            }

            if (rdo_OVC_READ_BID.SelectedIndex != -1  && rdo_OVC_READ_BID.SelectedItem.Text.Equals("是"))
            {
                var queryANN = mpms.TBMANNOUNCE_OPEN.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.ONB_TIMES == 1
                                            && o.OVC_CHECK_UNIT.Equals(dept));
                if (queryANN.Any())
                {
                    TBMANNOUNCE_OPEN tbmANNOUNCE_OPEN = queryANN.FirstOrDefault();
                    tbmANNOUNCE_OPEN.OVC_OPEN_PERIOD = txtOVC_OPEN_PERIOD.Text;
                    tbmANNOUNCE_OPEN.OVC_DANNOUNCE = txtOVC_DANNOUNCE.Text;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , tbmANNOUNCE_OPEN.GetType().Name.ToString(), this, "新增");
                }
                else
                {
                    TBMANNOUNCE_OPEN tbmANNOUNCE_OPEN = new TBMANNOUNCE_OPEN();
                    tbmANNOUNCE_OPEN.OVC_PURCH = strPurchNum;
                    tbmANNOUNCE_OPEN.OVC_NAME = userName;
                    tbmANNOUNCE_OPEN.OVC_CHECK_UNIT = dept;
                    tbmANNOUNCE_OPEN.OVC_DANNOUNCE = txtOVC_DANNOUNCE.Text;
                    tbmANNOUNCE_OPEN.OVC_OPEN_PERIOD = txtOVC_OPEN_PERIOD.Text;
                    tbmANNOUNCE_OPEN.ONB_TIMES = 1;
                    mpms.TBMANNOUNCE_OPEN.Add(tbmANNOUNCE_OPEN);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , tbmANNOUNCE_OPEN.GetType().Name.ToString(), this, "修改");
                }
                strOPEN = "Y";
            }
            else if(rdo_OVC_READ_BID.SelectedIndex != -1 && rdo_OVC_READ_BID.SelectedItem.Text.Equals("否"))
            {
                strOPEN = "N";
            }

            if (rdo_OVC_READ_BID.SelectedIndex == -1 || rdoFAC.SelectedIndex == -1)
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "尚未選擇");
            }
            else
            {
                TBM1301_PLAN tbm1301P = gm.TBM1301_PLAN.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                if (tbm1301P != null)
                {
                    tbm1301P.OVC_FAC_CHECK = strFAC;
                    tbm1301P.OVC_OPEN_CHECK = strOPEN;
                    gm.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , tbm1301P.GetType().Name.ToString(), this, "修改");
                }
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "儲存成功");
            }
                
        }
        
        protected void btn_PRINT_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "PrintSupPDF":
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
                case "PrintNewPDF":
                    PrintNewPDF();
                    break;
                case "btnOVCLIST_PRINT_WORD":
                    string filePath  =  printListDoc();
                    FileInfo file = new FileInfo(filePath);
                    //匯出Word檔提供下載
                    Response.Clear();
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + strPurchNum + "計畫清單.docx");
                    //Response.AppendHeader("Content-Disposition", "attachment; filename=" + DateTime.Now + "計畫清單.docx");
                    Response.ContentType = "application/octet-stream";
                    //Response.BinaryWrite(buffer);
                    Response.WriteFile(file.FullName);
                    Response.OutputStream.Flush();
                    Response.OutputStream.Close();
                    Response.Flush();
                    File.Delete(filePath);
                    Response.End();
                    break;
                case "btnOVCLIST_PRINT_ODT":
                    string filePathOdt = printListDoc();
                    string filePathTemp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/LOP_Temp.odt");
                    string fileName = strPurchNum + "計畫清單.odt";
                    FCommon.WordToOdt(this, filePathOdt, filePathTemp, fileName);
                    break;
                case "btnOVCLIST_PRINT":
                    string filePathWORD = printListDoc();
                    FileInfo fileWORD = new FileInfo(filePathWORD);
                    string pdfPath = fileWORD.DirectoryName + "\\" + strPurchNum + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                    WordcvDdf(filePathWORD, pdfPath);
                    File.Delete(filePathWORD);
                    //匯出PDF檔提供下載
                    Response.Clear();
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + strPurchNum + "計畫清單.pdf");
                    Response.ContentType = "application/octet-stream";
                    //Response.BinaryWrite(buffer);
                    Response.WriteFile(pdfPath);
                    Response.OutputStream.Flush();
                    Response.OutputStream.Close();
                    Response.Flush();
                    File.Delete(filePathWORD);
                    File.Delete(pdfPath);
                    Response.End();
                    break;
                case "btnBUDGET_DETAIL":
                    PrintBudgetPDF();
                    break;
            }
        }
        #endregion

        #region 副程式
        private void GetFileInfo()
        {
            string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/C/PurchCheck/" + strPurchNum));
            if (Directory.Exists(serverDir))
            {
                DataTable FileTable = new DataTable();
                FileTable.Columns.Add("FileName", typeof(String));
                FileTable.Columns["FileName"].MaxLength = 50;
                FileTable.Columns.Add("Time", typeof(String));
                FileTable.Columns["Time"].MaxLength = 50;
                FileTable.Columns.Add("Size", typeof(String));
                FileTable.Columns["Size"].MaxLength = 50;
                foreach (string fname in Directory.GetFiles(serverDir))
                {
                    FileInfo info = new FileInfo(fname);
                    DataRow row = FileTable.NewRow();
                    row["FileName"] = info.Name;
                    row["Time"] = info.CreationTime;
                    decimal size = Math.Round((decimal)info.Length / 1024, 5);//K byte
                    row["Size"] = size.ToString()+"  KB";
                    FileTable.Rows.Add(row);
                }
                GV_OVC_Alreadyupdate.DataSource = FileTable;
                GV_OVC_Alreadyupdate.DataBind();
            }
            
        }

        private void FileUpload(string path)
        {
            //檔案上傳
            if (isUpload)
            {
                if (ful.HasFile)
                {
                    string fileName = ful.FileName;
                    string serverFilePath = Path.Combine(path, fileName);
                    try
                    {
                        ful.SaveAs(serverFilePath);
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "檔案上傳成功");
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", error);
                    }
                    GetFileInfo();
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先 選擇檔案");
            }
        }

        private void DataImport()
        {
            var query1301 = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            lblOVC_PURCH.Text = query1301.OVC_PURCH + query1301.OVC_PUR_AGENCY;
            lblOVC_PUR_IPURCH.Text = query1301.OVC_PUR_IPURCH;
            lblOVC_PUR_USER.Text = query1301.OVC_PUR_NSECTION + "(" + query1301.OVC_PUR_SECTION + ")-"
                                    + query1301.OVC_PUR_USER + "(" + query1301.OVC_PUR_IUSER_PHONE + ", 軍線："
                                    + query1301.OVC_PUR_IUSER_PHONE_EXT + ", 手機" + query1301.OVC_USER_CELLPHONE + ")";
            var quert1301P = gm.TBM1301_PLAN.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();

            if (quert1301P.OVC_FAC_CHECK != null)
            {
                if (quert1301P.OVC_FAC_CHECK.Equals("Y"))
                {
                    rdoFAC.SelectedValue = "1";
                }

                else if (quert1301P.OVC_FAC_CHECK.Equals("N"))
                {
                    rdoFAC.SelectedValue = "2";
                }

            }

            if (quert1301P.OVC_OPEN_CHECK != null)
            {
                if (quert1301P.OVC_OPEN_CHECK.Equals("Y"))
                {
                    rdoFAC.SelectedValue = "1";
                    divDateTime.Visible = true;
                    var queryANNOUNCE = mpms.TBMANNOUNCE_OPEN.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                    if (queryANNOUNCE != null)
                    {
                        txtOVC_DANNOUNCE.Text = queryANNOUNCE.OVC_DANNOUNCE;
                        txtOVC_OPEN_PERIOD.Text = queryANNOUNCE.OVC_OPEN_PERIOD;
                    }
                }
                else if (quert1301P.OVC_OPEN_CHECK.Equals("N"))
                {
                    rdoFAC.SelectedValue = "2";
                }
            }

            

            var query1301H = mpms.TBM1301_HISTORY.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o=>o.OVC_VERSION).FirstOrDefault();
            lblOVC_VERSION.Text = "上一版本：" + query1301H;
            lblOVC_VERSION_NOW.Text = "目前版本：" + query1301.OVC_VERSION;
            
        }

        private void QueryPlanDataImport()
        {
            string[] field = { "OVC_PURCH","OVC_DRESULT","ONB_CHECK_TIMES","OVC_DRECEIVE",
                                "OVC_CHECK_OK","OVC_DREJECT","OVC_PERMISSION_UPDATE","ISAUDIT", };
            string checker = ViewState["USER_NAME"].ToString();
            string dept = ViewState["DEPT_SN"].ToString();
            var detp = mpms.TBM1407.Where(o => o.OVC_PHR_CATE.Equals("K5"));
            var query =
                from t in mpms.TBM1202
                join t1202_1 in mpms.TBM1202_1 on new { t.OVC_PURCH, t.ONB_CHECK_TIMES } equals new { t1202_1.OVC_PURCH, t1202_1.ONB_CHECK_TIMES } into ps
                from t1202_1 in ps.DefaultIfEmpty()
                join t1407 in detp on t1202_1.OVC_AUDIT_UNIT equals t1407.OVC_PHR_ID into gp
                from t1407 in gp.DefaultIfEmpty()
                 join t1301 in mpms.TBM1301 on t.OVC_PURCH equals t1301.OVC_PURCH
                 where t.OVC_PURCH.Equals(strPurchNum) && t.OVC_CHECK_UNIT.Equals(dept) && t.OVC_CHECKER.Equals(checker)
                 select new
                 {
                     t.OVC_PURCH,
                     t.ONB_CHECK_TIMES,
                     t.OVC_CHECK_OK,
                     t.OVC_DRECEIVE,
                     t.OVC_DRESULT,
                     t.OVC_DREJECT,
                     OVC_DAUDIT = t1202_1 != null ? t1202_1.OVC_DAUDIT : "",
                     OVC_USR_ID = t1407 != null ? t1407.OVC_USR_ID : "",
                     t1301.OVC_PERMISSION_UPDATE
                 };
            var queryCombine =
                from t in query.AsEnumerable()
                select new
                {
                    t.OVC_PURCH,
                    t.ONB_CHECK_TIMES,
                    t.OVC_CHECK_OK,
                    t.OVC_DRECEIVE,
                    t.OVC_DRESULT,
                    t.OVC_DREJECT,
                    t.OVC_DAUDIT,
                    t.OVC_PERMISSION_UPDATE,
                    ISAUDIT = t.OVC_USR_ID + (t.OVC_DAUDIT == null ? "【尚未回覆】" : "【於" + GetTaiwanDate(t.OVC_DAUDIT,".") + "回覆】")
                };
            var groupAll =
                queryCombine.AsEnumerable()
                .OrderBy(o=>o.ONB_CHECK_TIMES)
                .GroupBy(o =>
                new
                {
                    o.OVC_PURCH,
                    o.OVC_DRESULT,
                    o.ONB_CHECK_TIMES,
                    o.OVC_DRECEIVE,
                    o.OVC_CHECK_OK,
                    o.OVC_DREJECT,
                    o.OVC_PERMISSION_UPDATE,
                }).Select(g=>
                new
                {
                    g.Key.OVC_PURCH,
                    g.Key.OVC_DRESULT,
                    g.Key.ONB_CHECK_TIMES,
                    g.Key.OVC_DRECEIVE,
                    g.Key.OVC_CHECK_OK,
                    g.Key.OVC_DREJECT,
                    g.Key.OVC_PERMISSION_UPDATE,
                    ISAUDIT = string.Join("；", g.Select(ee => ee.ISAUDIT).ToList())
                });
            
           
            DataTable dt = CommonStatic.LinqQueryToDataTable(groupAll);
            hasRows = FCommon.GridView_dataImport(GV_Query_PLAN, dt, field);
            txtONB_CHECK_TIMES.Text = (dt.Rows.Count + 1).ToString();
        }
        
        private void AttchedDataImport()
        {
            string[] field = { "OVC_IKIND", "OVC_ATTACH_NAME", "OVC_FILE_NAME", "ONB_QTY", "ONB_PAGES"};
            var query =
                from t in mpms.TBM1119
                where t.OVC_PURCH.Equals(strPurchNum)
                select new
                {
                    OVC_IKIND = t.OVC_IKIND == "D" ? "採購計畫清單" : "物資申請書",
                    t.OVC_ATTACH_NAME,
                    t.OVC_FILE_NAME,
                    t.ONB_QTY,
                    t.ONB_PAGES
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            hasRows = FCommon.GridView_dataImport(GV_C_Alreadyupdate, dt, field);
        }

        protected void GV_Query_PLAN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hidOVC_CHECK_OK = (HiddenField)e.Row.FindControl("hidOVC_CHECK_OK");
                HiddenField hidOVC_PERMISSION_UPDATE = (HiddenField)e.Row.FindControl("hidOVC_PERMISSION_UPDATE");
                HiddenField hidOVC_DREJECT = (HiddenField)e.Row.FindControl("hidOVC_DREJECT");
                Label lblONB_CHECK_TIMES = (Label)e.Row.FindControl("lblONB_CHECK_TIMES");
                Label lblAUDITSTATUS = (Label)e.Row.FindControl("lblAUDITSTATUS");
                Label lblOVC_DRESULT = (Label)e.Row.FindControl("lblOVC_DRESULT");
                Label lblSignDate = (Label)e.Row.FindControl("lblSignDate");
                Label lblUnit = (Label)e.Row.FindControl("lblUnit");
                Button btn_Signature = (Button)e.Row.FindControl("btn_Signature");
                Button btn_Review = (Button)e.Row.FindControl("btn_Review");
                LinkButton btn_PrintComment = (LinkButton)e.Row.FindControl("btn_PrintComment");
                LinkButton btn_CommentWithResponse = (LinkButton)e.Row.FindControl("btn_CommentWithResponse");
                LinkButton btn_PrintREASON  = (LinkButton)e.Row.FindControl("btn_PrintREASON");
                DateTime dateReject;
                DateTime now = DateTime.Now;
                TimeSpan span;
                Boolean isTimeData_Reject = DateTime.TryParse(hidOVC_DREJECT.Value, out dateReject);

                if (lblAUDITSTATUS.Text.Equals("Y"))
                {
                    lblAUDITSTATUS.Text = "(確認審)";
                    btn_Review.Visible = false;
                    btn_PrintREASON.Visible = true;
                    AddWindowOpenCOMMENT(lblONB_CHECK_TIMES.Text, btn_PrintREASON);
                }
                else
                {
                    if (lblONB_CHECK_TIMES.Text.Equals("1"))
                    {
                        lblAUDITSTATUS.Text = "(初審)";
                    }
                    else
                        lblAUDITSTATUS.Text = "(複審)";
                }

                if (string.IsNullOrEmpty(lblOVC_DRESULT.Text))
                {
                    btn_Signature.Visible = true;
                    lblOVC_DRESULT.Visible = false;
                }

                if(lblUnit.Text.Equals("【尚未回覆】"))
                {
                   lblUnit.Text = "";
                }
                else
                {
                    btn_PrintComment.Visible = true;
                    btn_CommentWithResponse.Visible = true;
                    AddWindowOpen(lblONB_CHECK_TIMES.Text, "false", btn_PrintComment);
                    AddWindowOpen(lblONB_CHECK_TIMES.Text, "true", btn_CommentWithResponse);
                }
                if(e.Row.RowIndex == GV_Query_PLAN.Rows.Count)
                {
                    //逾時五日、七日
                    if (isTimeData_Reject)
                    {
                        span = now.Subtract(dateReject);
                        int diff = Convert.ToInt16(span.Days);
                        if (hidOVC_CHECK_OK.Value != "Y" && hidOVC_PERMISSION_UPDATE.Value.Equals("Y"))
                        {
                            if (lblONB_CHECK_TIMES.Text.Equals("1") && diff >= 7)
                            {
                                lblSignDate.Visible = true;
                            }
                            else if (lblONB_CHECK_TIMES.Text != "1" && diff >= 5)
                            {
                                lblSignDate.Visible = true;
                            }
                        }
                    }
                }
            }
        }

        private void AddWindowOpen(string numCheckTimes, string IS_RESPONSE,LinkButton btn)
        {
            string strURL = "";
            string strWinTitle = "";
            string strWinProperty = "";

            strURL = "\\ReviewCOMMENT.aspx?OVC_PURCH=" + strPurchNum + "&ONB_CHECK_TIMES=" + numCheckTimes + "&IS_RESPONSE=" + IS_RESPONSE;
            strWinTitle = "null";
            strWinProperty = "toolbar=0,location=0,status=0,menubar=0,scrollbars=yes,width=700,height=500,left=200,top=80";
            btn.Attributes.Add("onClick", "javascript:window.open('" + strURL + "','" + strWinTitle + "','" + strWinProperty + "');return false;");
        }
        private void AddWindowOpenCOMMENT(string numCheckTimes, LinkButton btn)
        {
            string strURL = "";
            string strWinTitle = "";
            string strWinProperty = "";

            strURL = "\\COMMENT.aspx?OVC_PURCH=" + strPurchNum + "&ONB_CHECK_TIMES=" + numCheckTimes + "&Unit=03";
            strWinTitle = "null";
            strWinProperty = "toolbar=0,location=0,status=0,menubar=0,scrollbars=yes,width=700,height=500,left=200,top=80";
            btn.Attributes.Add("onClick", "javascript:window.open('" + strURL + "','" + strWinTitle + "','" + strWinProperty + "');return false;");
        }

        protected void GV_Query_PLAN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            Label lblCheckTimes = (Label)GV_Query_PLAN.Rows[gvrIndex].FindControl("lblONB_CHECK_TIMES");
            string url = "";
            string DRecive = GV_Query_PLAN.Rows[gvrIndex].Cells[1].Text;
            switch (e.CommandName)
            {
                case "btn_Change":
                    url = "~/pages/MPMS/C/MPMS_C15.aspx?PurchNum=" + strPurchNum + "&numCheckTimes=" + lblCheckTimes.Text + "&DRecive=" + DRecive;
                    break;
                case "btn_Signature":
                    url = "~/pages/MPMS/C/MPMS_C18.aspx?PurchNum=" + strPurchNum + "&numCheckTimes=" + lblCheckTimes.Text + "&DRecive=" + DRecive;
                    break;
                case "btn_Review":
                    url = "~/pages/MPMS/C/MPMS_C17.aspx?PurchNum=" + strPurchNum + "&numCheckTimes=" + lblCheckTimes.Text + "&page=C14";
                    break;
            }
            Response.Redirect(url);

        }

        protected void GV_OVC_Alreadyupdate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string DRecive = gvr.Cells[1].Text;
            string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/C/PurchCheck/" + strPurchNum + "/" + DRecive));
            switch (e.CommandName)
            {
                case "btn_Download":
                    FCommon.DownloadFile(this, serverDir);
                    break;
            }
        }

        private string GetTaiwanDate(string strDate,string cString)
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

        private void GetUserInfo()
        {
            if(Session["userid"] != null)
            {
                if (Session["userid"] != null)
                {
                    string userID = Session["userid"].ToString();
                    var userInfo = gm.ACCOUNTs.Where(o => o.USER_ID.Equals(userID)).FirstOrDefault();
                    ViewState["USER_NAME"] = userInfo.USER_NAME;
                    ViewState["DEPT_SN"] = userInfo.DEPT_SN;
                }
            }
            
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
            string strURL = "";
            string strWinTitle = "";
            string strWinProperty = "";
            strURL = "\\CheckDifference.aspx?OVC_PURCH=" + strPurchNum;
            strWinTitle = "null";
            strWinProperty = "toolbar=0,location=0,status=0,menubar=0,width=700,height=500,left=200,top=80";

            btnOVC_RESPONSER.Attributes.Add("onClick", "javascript:window.open('" + strURL + "','" + strWinTitle + "','" + strWinProperty + "');return false;");
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
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H
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
                 join t1407 in gm.TBM1407.Where(tt => tt.OVC_PHR_CATE.Equals("B0")) on t.OVC_PUR_CURRENT equals t1407.OVC_PHR_ID into temp1407
                 from t1407 in temp1407.DefaultIfEmpty()
                 where t.OVC_PURCH.Equals(strPurchNum)
                 select new
                 {
                     t.OVC_PUR_AGENCY,
                     t.OVC_PUR_NSECTION,
                     t.OVC_PUR_DAPPROVE,
                     t.OVC_PUR_APPROVE,
                     t.OVC_DPROPOSE,
                     t.OVC_PROPOSE,
                     t.OVC_PUR_IPURCH,
                     OVC_PUR_CURRENT = t1407 != null ? t1407.OVC_PHR_DESC : "",
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
            string strp2 = "中華民國" +GetTaiwanDate(t1301.OVC_PUR_DAPPROVE,"chDate") + "\n";
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
            PdfPCell cell6_2 = new PdfPCell(new Phrase(t1301.OVC_PUR_CURRENT+" " + money, ChFont));
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
            foreach(var item in queryDPPR_PLAN)
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
            //不然IE的下載會有亂碼
            string fileName=HttpUtility.UrlEncode(strPurchNum+ "物資申請書.pdf");
            Response.Clear();//瀏覽器上顯示
            //Response.AddHeader("Content-disposition", "attachment;filename=" + strPurchNum + "物資申請書.pdf");
            Response.AddHeader("Content-disposition", "attachment;filename=" + fileName);
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
            string fileName=HttpUtility.HtmlEncode(strPurchNum+purAgency+"物資申請書.pdf");
            Response.Clear();
            //Response.AppendHeader("Content-Disposition", "attachment; filename=" + strPurchNum + purAgency + "物資申請書.pdf");
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
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

        #region PDF列印-新物資申請書
        private void PrintNewPDF()
        {
            //PDF列印
            var doc1 = new Document(PageSize.A4, 50, 50, 45, 50);
            MemoryStream Memory = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            Watermark wm = new Watermark();
            wm.strPurchNum = strPurchNum;
            writer.PageEvent = wm;
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H
               , BaseFont.NOT_EMBEDDED);//設定字型
            iTextSharp.text.Font ChFont = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            iTextSharp.text.Font ChFont_blue = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.NORMAL, new BaseColor(2, 0, 255));
            iTextSharp.text.Font ChFont_msg = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.ITALIC, BaseColor.RED);
            iTextSharp.text.Font ChFont_memo = new iTextSharp.text.Font(bfChinese, 8, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            doc1.Open();

            //page1
            #region Page1
            string angcy = "";
            var query1407 = gm.TBM1407.Where(o => o.OVC_PHR_CATE.Equals("B0"));
            var t1301 =
                (from t in gm.TBM1301
                 join t1407 in query1407 on t.OVC_PUR_CURRENT equals t1407.OVC_PHR_ID into ps
                 from t1407 in ps.DefaultIfEmpty()
                 where t.OVC_PURCH.Equals(strPurchNum)
                 select new
                 {
                     t.OVC_PURCH_KIND,
                     t.OVC_PUR_AGENCY,
                     t.OVC_PUR_NSECTION,
                     t.OVC_PUR_DAPPROVE,
                     t.OVC_PUR_APPROVE,
                     t.OVC_DPROPOSE,
                     t.OVC_PROPOSE,
                     t.OVC_PUR_IPURCH,
                     OVC_PUR_CURRENT = t1407 != null ? t1407.OVC_PHR_DESC : "",
                     ONB_PUR_BUDGET = t.ONB_PUR_BUDGET ?? 0,
                     t.OVC_PUR_SECTION,
                     t.OVC_SECTION_CHIEF,
                     t.OVC_AGNT_IN,
                     t.OVC_SUPERIOR_UNIT
                 }).FirstOrDefault();
            if (t1301.OVC_PURCH_KIND.Equals("1"))
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
            string strp1 = t1301.OVC_PUR_NSECTION + angcy + "物資申請書存根";
            iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(strp1, ChFont);
            p.Add(new Chunk(glue));//有沒有其他寫法  無法垂直置中
            string strp2 = "中華民國" + GetTaiwanDate(t1301.OVC_PUR_DAPPROVE,"chDate") + "\n";
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
            PdfPCell cell2_3 = new PdfPCell(new Phrase("申購日期及文號", ChFont));
            cell2_3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2_3.HorizontalAlignment = 1;
            table.AddCell(cell2_3);
            string strPropose = "中華民國" + GetTaiwanDate(t1301.OVC_DPROPOSE,"chDate") + "\n" + t1301.OVC_PROPOSE;
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
            PdfPCell cell3_4 = new PdfPCell(new Phrase("數量", ChFont));
            cell3_4.HorizontalAlignment = 1;
            cell3_4.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell3_4);
            PdfPCell cell3_5 = new PdfPCell(new Phrase("單價", ChFont));
            cell3_5.HorizontalAlignment = 1;
            cell3_5.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell3_5);
            PdfPCell cell3_6 = new PdfPCell(new Phrase("總價", ChFont));
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
            PdfPCell cell6_2 = new PdfPCell(new Phrase(t1301.OVC_PUR_CURRENT+ " " + money, ChFont));
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

            PdfPCell cell10 = new PdfPCell();
            Chunk glue2 = new Chunk(new VerticalPositionMark());
            iTextSharp.text.Paragraph p2 = new iTextSharp.text.Paragraph("謹呈\n", ChFont);
            p2.FirstLineIndent = 25;
            p2.Add(t1301.OVC_SUPERIOR_UNIT);
            p2.Add(new Chunk(glue2));
            p2.Add("主官 " + t1301.OVC_SECTION_CHIEF);//主官
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

            //page2
            #region Page2
            doc1.NewPage();
            table = new PdfPTable(new float[] { 1, 3, 1, 1, 1, 3, 2, 4 });

            table.TotalWidth = 500f;
            table.LockedWidth = true;

            header = new PdfPCell();
            glue = new Chunk(new VerticalPositionMark());
            string strUnit;
            var querynsection2 = mpms.TBMPURCH_EXT.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.OVC_PUR_NSECTION_2).FirstOrDefault();
            if (querynsection2 != null)
            {
                strUnit = querynsection2;
            }
            else
            {
                var query1202Dept =
                    (from t in mpms.TBM1202.AsEnumerable()
                     join t2 in gm.TBMDEPTs on t.OVC_CHECK_UNIT equals t2.OVC_DEPT_CDE
                     where t.OVC_PURCH.Equals(strPurchNum)
                     orderby t.OVC_DRECEIVE
                     select t2.OVC_ONNAME).FirstOrDefault();
                strUnit = query1202Dept;
            }

            p = new iTextSharp.text.Paragraph(strUnit + angcy + "物資申請書", ChFont);//標題
            p.Add(new Chunk(glue));//有沒有其他寫法  無法垂直置中
            //日期及文號-TBMOVERSEE_LOG
            string logApply;
            var querylogApply =
                mpms.TBMOVERSEE_LOG.Where(o => o.OVC_PURCH.Equals(strPurchNum))
                                   .OrderBy(o => o.OVC_DATE)
                                   .Select(o => new { o.OVC_DAPPLY, o.OVC_APPLY_NO, o.OVC_APPLY_CHIEF })
                                   .FirstOrDefault();
            logApply = "中華民國" + GetTaiwanDate(querylogApply.OVC_DAPPLY,"chDate") + "\n";
            p.Add(logApply);
            p.Add(new Chunk(glue));
            p.Add(querylogApply.OVC_APPLY_NO);
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

            PdfPCell cell10_2 = new PdfPCell();
            Chunk glue2_2 = new Chunk(new VerticalPositionMark());
            iTextSharp.text.Paragraph p2_2 = new iTextSharp.text.Paragraph("謹呈\n", ChFont);
            p2_2.FirstLineIndent = 25;
            p2_2.Add(t1301.OVC_SUPERIOR_UNIT);
            p2_2.Add(new Chunk(glue2_2));
            p2_2.Add("主官 " + querylogApply.OVC_APPLY_CHIEF);//主官
            cell10_2.AddElement(p2);
            cell10_2.Colspan = 8;
            table2.AddCell(cell10_2);
            table2.AddCell(cell11_1);
            table2.AddCell(cell11_2);
            table2.AddCell(cell12_1);
            table2.AddCell(cell12_2);
            table2.AddCell(cell12_3);
            table2.AddCell(cell13_1);
            table2.AddCell(cell13_2);
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
            p = new iTextSharp.text.Paragraph(t1301.OVC_SUPERIOR_UNIT + angcy + "物資核定書", ChFont);//標題
            p.Add(new Chunk(glue));//有沒有其他寫法  無法垂直置中
            p.Add(strp2);
            p.Add(new Chunk(glue));
            p.Add(t1301.OVC_PUR_APPROVE);
            p.Alignment = Element.ALIGN_MIDDLE;
            header.AddElement(p);
            header.Colspan = 8;
            table.AddCell(header);
            table.AddCell(cell2_1);
            var queryAgntIn = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.OVC_AGNT_IN).FirstOrDefault();
            PdfPCell cell2_2_2 = new PdfPCell(new Phrase(queryAgntIn, ChFont));
            cell2_2_2.Colspan = 4;
            cell2_2_2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell2_2_2);
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

            PdfPCell cell10_1 = new PdfPCell(new Phrase("審查意見", ChFont));
            table2.AddCell(cell10_1);
            var queryComment = mpms.TBM1202.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_CHECK_OK.Equals("Y")).Select(o => o.OVC_APPROVE_COMMENT).FirstOrDefault();
            PdfPCell cell10_3 = new PdfPCell(new Phrase(queryComment, ChFont_memo));
            cell10_3.Colspan = 7;
            table2.AddCell(cell10_3);
            PdfPCell cell11 = new PdfPCell();

            p2 = new iTextSharp.text.Paragraph(new Phrase("此令\n", ChFont));
            p2.FirstLineIndent = -5;
            p2.Add(queryAgntIn + "                  ");//空格是否有更好的方法
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
            p = new iTextSharp.text.Paragraph(t1301.OVC_SUPERIOR_UNIT + angcy + "物資核定書", ChFont);//標題
            p.Add(new Chunk(glue));//有沒有其他寫法  無法垂直置中
            p.Add(strp2);
            p.Add(new Chunk(glue));
            p.Add(t1301.OVC_PUR_APPROVE);
            p.Alignment = Element.ALIGN_MIDDLE;
            header.AddElement(p);
            header.Colspan = 8;
            table.AddCell(header);
            table.AddCell(cell2_1);
            table.AddCell(cell2_2_2);
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
            table2.AddCell(cell10_1);
            table2.AddCell(cell10_3);
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
            string fileName = HttpUtility.UrlEncode(strPurchNum + "新物資申請書.pdf");
            //Response.AddHeader("Content-disposition", "attachment;filename=" + strPurchNum + "新物資申請書.pdf");
            Response.AddHeader("Content-disposition", "attachment;filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.Flush();
            Response.End();
        }
        #endregion

        #region 列印清單
        private string printListDoc()
        {
            string fileName = "計畫清單計評.docx";
            string TempName = "";
            string filePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + fileName));
            string outPutfilePath = "";
            var query1301 =
                (from t in gm.TBM1301
                where t.OVC_PURCH.Equals(strPurchNum)
                select new
                {
                    t.OVC_PURCH_KIND,
                    OVC_PUR_NSECTION = t.OVC_PUR_NSECTION ?? " ",
                    OVC_DPROPOSE = t.OVC_DPROPOSE ?? " ",
                    OVC_PROPOSE = t.OVC_PROPOSE ?? " ",
                    OVC_PUR_NPURCH = t.OVC_PUR_NPURCH ?? " ",
                    OVC_PUR_NSECTION2 = t.OVC_PUR_NSECTION ?? " ",
                    OVC_AGNT_IN = t.OVC_AGNT_IN ?? " ",
                    OVC_RECEIVE_NSECTION = t.OVC_RECEIVE_NSECTION ?? " ",
                    OVC_SHIP_TIMES = t.OVC_SHIP_TIMES ?? " ",
                    OVC_RECEIVE_PLACE = t.OVC_RECEIVE_PLACE ?? "  ",
                    OVC_POI_IINSPECT = t.OVC_POI_IINSPECT ?? " ",
                    t.OVC_PURCH,
                    t.OVC_PUR_AGENCY
                }).FirstOrDefault();
            TempName = strPurchNum + query1301.OVC_PUR_AGENCY + DateTime.Now.ToString("yyyyMMddHHmmss") + "計畫清單" + ".docx";
            outPutfilePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + TempName));
            File.Copy(filePath, outPutfilePath);
            var query1231 = mpms.TBM1231.Where(o => o.OVC_PURCH.Equals(strPurchNum)).ToList();
            var source1231 =
                from t in query1231
                group t by t.OVC_PURCH into g
                select new
                {
                    g.Key,
                    ISOURCE = string.Join(";", g.Select(o => o.OVC_ISOURCE))
                };
            var query1231APPROVE = mpms.TBM1231.Where(o => o.OVC_PURCH.Equals(strPurchNum));
            string AppDate = "";
            foreach (var item in query1231APPROVE)
                AppDate += GetTaiwanDate(item.OVC_PUR_DAPPR_PLAN) + "\t" + item.OVC_PUR_APPR_PLAN +"\r\n";
            var valuesToFill = new TemplateEngine.Docx.Content();
            string strKind = "";
            if (query1301.OVC_PURCH_KIND.Equals("1"))
            {
                strKind = "內";
            }
            else
            {
                strKind = "外";
            }
            valuesToFill.Fields.Add(new FieldContent("year", query1301.OVC_PURCH.Substring(2,2)));
            valuesToFill.Fields.Add(new FieldContent("OVC_PUR_NSECTION", query1301.OVC_PUR_NSECTION));
            valuesToFill.Fields.Add(new FieldContent("OVC_DPROPOSE", GetTaiwanDate(query1301.OVC_DPROPOSE)));
            valuesToFill.Fields.Add(new FieldContent("OVC_PROPOSE", query1301.OVC_PROPOSE));
            valuesToFill.Fields.Add(new FieldContent("OVC_PUR_NPURCH", query1301.OVC_PUR_NPURCH));
            valuesToFill.Fields.Add(new FieldContent("OVC_ISOURCE", source1231.FirstOrDefault().ISOURCE));
            valuesToFill.Fields.Add(new FieldContent("OVC_PUR_DAPPR_PLAN", AppDate));
            valuesToFill.Fields.Add(new FieldContent("OVC_PUR_NSECTION2", query1301.OVC_PUR_NSECTION));
            valuesToFill.Fields.Add(new FieldContent("OVC_AGNT_IN", query1301.OVC_PUR_NSECTION));
            valuesToFill.Fields.Add(new FieldContent("OVC_PURCH", query1301.OVC_PURCH + query1301.OVC_PUR_AGENCY));
            valuesToFill.Fields.Add(new FieldContent("OVC_RECEIVE_NSECTION", query1301.OVC_RECEIVE_NSECTION));
            valuesToFill.Fields.Add(new FieldContent("OVC_SHIP_TIMES", query1301.OVC_SHIP_TIMES));
            valuesToFill.Fields.Add(new FieldContent("OVC_RECEIVE_PLACE", query1301.OVC_RECEIVE_PLACE));
            valuesToFill.Fields.Add(new FieldContent("OVC_POI_IINSPECT", query1301.OVC_POI_IINSPECT));
            valuesToFill.Fields.Add(new FieldContent("kind", strKind));

            var query1201 =
                from t in mpms.TBM1201
                where t.OVC_PURCH.Equals(strPurchNum)
                orderby t.ONB_POI_ICOUNT
                select new
                {
                    t.ONB_POI_ICOUNT,
                    t.ONB_POI_QORDER_PLAN,
                    t.ONB_POI_MPRICE_PLAN,
                    OVC_POI_NSTUFF_CHN = t.OVC_POI_NSTUFF_CHN ?? " ",
                    OVC_POI_IUNIT = t.OVC_POI_IUNIT ?? " ",
                    NSN = t.NSN ?? "(空白)",
                    OVC_BRAND = t.OVC_BRAND ?? "(空白)",
                    OVC_MODEL = t.OVC_MODEL ?? "(空白)",
                    OVC_POI_IREF = t.OVC_POI_IREF ?? "(空白)",
                    OVC_FCODE = t.OVC_FCODE ?? "(空白)",
                    OVC_POI_NDESC = t.OVC_POI_NDESC ?? " ",
                    OVC_POI_IPURCH_BEF = t.OVC_POI_IPURCH_BEF ?? "",
                    OVC_CURR_MPRICE_BEF = t.OVC_CURR_MPRICE_BEF ?? " ",
                    ONB_POI_MPRICE_BEF = t.ONB_POI_MPRICE_BEF ?? null,
                    ONB_POI_QORDER_BEF = t.ONB_POI_QORDER_BEF ?? null
                };
            var tableContent = new TableContent("table");
            foreach (var item in query1201)
            {
                decimal qty = Convert.ToDecimal(item.ONB_POI_QORDER_PLAN);
                decimal price = Convert.ToDecimal(item.ONB_POI_QORDER_PLAN);
                decimal total = qty * price;
                string strIPURCH_BEF = " ";
                string strCURR_MPRICE_BEF = " ";
                string strPOI_MPRICE_BEF = " ";
                string strQORDER_BEF = " ";
                if (!string.IsNullOrEmpty(item.OVC_POI_IPURCH_BEF))
                {
                    strIPURCH_BEF = "案號：" + item.OVC_POI_IPURCH_BEF;
                    strCURR_MPRICE_BEF = item.OVC_CURR_MPRICE_BEF;
                    strPOI_MPRICE_BEF = "價格：" + item.ONB_POI_MPRICE_BEF.ToString();
                    strQORDER_BEF = "數量：" + item.ONB_POI_QORDER_BEF.ToString();
                }

                tableContent.AddRow(
                        new FieldContent("head_count", "(11)\r\n項次"),
                        new FieldContent("head_name", "(12)\r\n品名料號及規格"),
                        new FieldContent("head_iunit", "(13)\r\n單位"),
                        new FieldContent("head_qty", "(14)\r\n數量"),
                        new FieldContent("head_money", "(15)\r\n單價"),
                        new FieldContent("head_total", "(16)\r\n總價"), 
                        new FieldContent("head_remark", "(17)\r\n備考\r\n(以往購價)"),
                        new FieldContent("ONB_POI_ICOUNT", item.ONB_POI_ICOUNT.ToString()),
                        new FieldContent("OVC_POI_NSTUFF_CHN", item.OVC_POI_NSTUFF_CHN),
                        new FieldContent("OVC_POI_IUNIT", item.OVC_POI_IUNIT),
                        new FieldContent("ONB_POI_QORDER_PLAN", qty.ToString("#,##0.00")),
                        new FieldContent("ONB_POI_MPRICE_PLAN", price.ToString("#,##0.00")),
                        new FieldContent("ONB_TOTAL", total.ToString("#,##0.00")),
                        new FieldContent("NSN", item.NSN),
                        new FieldContent("OVC_BRAND", item.OVC_BRAND),
                        new FieldContent("OVC_MODEL", item.OVC_MODEL),
                        new FieldContent("OVC_POI_IREF", item.OVC_POI_IREF),
                        new FieldContent("OVC_FCODE", item.OVC_FCODE),
                        new FieldContent("OVC_POI_NDESC", item.OVC_POI_NDESC),
                        new FieldContent("OVC_POI_IPURCH_BEF", strIPURCH_BEF),
                        new FieldContent("OVC_CURR_MPRICE_BEF", strCURR_MPRICE_BEF),
                        new FieldContent("ONB_POI_MPRICE_BEF", strPOI_MPRICE_BEF),
                        new FieldContent("ONB_POI_QORDER_BEF", strQORDER_BEF)
                    );
            }
            valuesToFill.Tables.Add(tableContent);
            string ovcIkind = "";
            switch (query1301.OVC_PUR_AGENCY)
            {
                case "B":
                case "L":
                case "P":
                    ovcIkind = "D5";
                    break;
                case "M":
                case "S":
                    ovcIkind = "F5";
                    break;
                default:
                    ovcIkind = "W5";
                    break;
            }
            string strMemo = "";
            var query12201 = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum));
            var query =
                from t in gm.TBM1220_2.AsEnumerable()
                join t2 in query12201 on t.OVC_IKIND equals t2.OVC_IKIND into g
                from t2 in g.DefaultIfEmpty()
                where t.OVC_IKIND.StartsWith(ovcIkind)
                orderby t.OVC_IKIND
                select new
                {
                    t.OVC_IKIND,
                    t.OVC_MEMO_NAME,
                    OVC_MEMO = t2 != null ? t2.OVC_MEMO : "(空白)",
                    OVC_STANDARD = t2 != null ? t2.OVC_STANDARD : "(空白)"
                };

            var queryGroup =
                from t in query
                orderby t.OVC_IKIND
                group t by t.OVC_MEMO_NAME into g
                select g;
            
            foreach (var row in queryGroup)
            {
                strMemo += row.Key + ":\r\n　　";
                foreach (var item in row)
                {
                    string memo = item.OVC_MEMO.Replace("<br>", "").Replace(" ","");
                    strMemo += memo + "\r\n";
                }
            }
            valuesToFill.Fields.Add(new FieldContent("remark", strMemo));
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

        #region 列印預算分配明細表pdf
        private void PrintBudgetPDF()
        {
            //預算年度分配明細表按鈕功能
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            iTextSharp.text.Font ChFont = new iTextSharp.text.Font(bfChinese, 12f);
            iTextSharp.text.Font smaillChFont = new iTextSharp.text.Font(bfChinese, 10f);
            var doc1 = new Document(PageSize.A4, 40, 50, 140, -10);
            MemoryStream Memory = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            writer.PageEvent = new PDF();
            doc1.Open();
            DateTime PrintTime = DateTime.Now;
            
            PdfPTable firsttable = new PdfPTable(2);
            firsttable.SetWidths(new float[] { 1, 1 });
            firsttable.DefaultCell.Border = Rectangle.NO_BORDER;
            firsttable.TotalWidth = 560F;
            firsttable.LockedWidth = true;
            firsttable.DefaultCell.SetLeading(1.2f, 1.2f);
            var query1301 = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();

            firsttable.AddCell(new Phrase("購案編號：" + strPurchNum + query1301.OVC_PUR_AGENCY, ChFont));
            firsttable.AddCell(new Phrase("購案名稱：" + query1301.OVC_PUR_IPURCH, ChFont));
            firsttable.AddCell(new Phrase("申購單位：" + query1301.OVC_PUR_NSECTION, ChFont));
            firsttable.AddCell(new Phrase("承辦人姓名：" + query1301.OVC_PUR_USER, ChFont));
            firsttable.AddCell(new Phrase("電話(自動)：" + query1301.OVC_PUR_IUSER_PHONE, ChFont));
            firsttable.AddCell(new Phrase("電話(軍線)：" + query1301.OVC_PUR_IUSER_PHONE_EXT, ChFont));
            firsttable.AddCell(new Phrase(" ", ChFont));
            firsttable.AddCell(new Phrase(" ", ChFont));
            doc1.Add(firsttable);

            PdfPTable secendtable = new PdfPTable(4);
            secendtable.SetWidths(new float[] { 1, 3, 3, 3 });
            secendtable.TotalWidth = 560F;
            secendtable.LockedWidth = true;
            secendtable.DefaultCell.FixedHeight = 25f;
            secendtable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            secendtable.AddCell(new Phrase("項次", ChFont));
            secendtable.AddCell(new Phrase("款源", ChFont));
            secendtable.AddCell(new Phrase("預算小計", ChFont));
            secendtable.AddCell(new Phrase("預算是否核定 ", ChFont));

            var query1231 =
               from t in mpms.TBM1231
               join c in mpms.TBM1407 on t.OVC_CURRENT equals c.OVC_PHR_ID
               where t.OVC_PURCH.Equals(strPurchNum) && c.OVC_PHR_CATE == "B0"
               select new
               {
                   t.OVC_ISOURCE,
                   t.ONB_MONEY,
                   t.OVC_PUR_APPR_PLAN,
                   t.OVC_PUR_DAPPR_PLAN,
                   c.OVC_PHR_DESC
               };

            int counter = 1;
            decimal sumBudget = 0;
            foreach (var item in query1231)
            {
                string strDAPPR = GetTaiwanDate(item.OVC_PUR_DAPPR_PLAN);
                string strMONEY = Convert.ToDecimal(item.ONB_MONEY).ToString("#,###.00");
                sumBudget += (decimal)item.ONB_MONEY;
                secendtable.AddCell(new Phrase(counter.ToString(), smaillChFont));
                secendtable.AddCell(new Phrase(item.OVC_ISOURCE, smaillChFont));
                secendtable.AddCell(new Phrase(item.OVC_PHR_DESC + "  "+ strMONEY, smaillChFont));
                secendtable.AddCell(new Phrase(item.OVC_PUR_APPR_PLAN + strDAPPR, smaillChFont));
                counter++;
                
            }

            string strBudget = sumBudget.ToString("#,###.00");

            PdfPCell tail = new PdfPCell(new Phrase("預算總金額：" + strBudget + "("+ query1231.FirstOrDefault().OVC_PHR_DESC + ")", ChFont));
            tail.Colspan = 4;
            tail.Border = Rectangle.NO_BORDER;
            tail.FixedHeight = 30f;
            tail.VerticalAlignment = Element.ALIGN_MIDDLE;
            secendtable.AddCell(tail);

            doc1.Add(secendtable);


            PdfPTable finaltable = new PdfPTable(5);
            finaltable.SetWidths(new float[] { 1, 4, 1, 1, 3 });
            finaltable.TotalWidth = 560F;
            finaltable.LockedWidth = true;
            finaltable.DefaultCell.FixedHeight = 25f;
            finaltable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            finaltable.AddCell(new Phrase("項次", ChFont));
            finaltable.AddCell(new Phrase("預算科目、名稱 ", ChFont));
            finaltable.AddCell(new Phrase("年度", ChFont));
            finaltable.AddCell(new Phrase("月份", ChFont));
            finaltable.AddCell(new Phrase("預劃金額 ", ChFont));

            var query1118 =
                from t in mpms.TBM1118
                where t.OVC_PURCH.Equals(strPurchNum)
                orderby t.OVC_YY, t.OVC_MM
                select t;

            int counter2 = 1;
            decimal sumDetailBudget = 0;
            foreach (var item in query1118)
            {
                string strMONEY = Convert.ToDecimal(item.ONB_MBUD).ToString("#,###.00");
                finaltable.AddCell(new Phrase(counter2.ToString(), smaillChFont));
                finaltable.AddCell(new Phrase(item.OVC_POI_IBDG + "(" + item.OVC_PJNAME +")", smaillChFont));
                finaltable.AddCell(new Phrase(item.OVC_YY, smaillChFont));
                finaltable.AddCell(new Phrase(item.OVC_MM, smaillChFont));
                finaltable.AddCell(new Phrase(strMONEY, smaillChFont));
                sumDetailBudget += (decimal)item.ONB_MBUD;
                counter2++;
            }
            string strDetailBudget = sumDetailBudget.ToString("#,###.00");
            PdfPCell content1 = new PdfPCell(new Phrase("預劃總金額：" + strDetailBudget, ChFont));
            content1.Colspan = 4;
            content1.Border = Rectangle.NO_BORDER;

            finaltable.AddCell(content1);
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt");
            PdfPCell content2 = new PdfPCell(new Phrase(timeStamp, ChFont));
            content2.HorizontalAlignment = Element.ALIGN_RIGHT;
            content2.Border = Rectangle.NO_BORDER;

            finaltable.AddCell(content2);
            doc1.Add(finaltable);
            doc1.Close();
            Response.Clear();//瀏覽器上顯示
            Response.AddHeader("Content-disposition", "attachment;filename=" + strPurchNum + "預算年度分配明細表.pdf");
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.Flush();
            Response.End();
        }
        #endregion

    }
}