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
using System.Data.Entity.Infrastructure;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.IO;
using TemplateEngine.Docx;
using Xceed.Words.NET;
using Microsoft.International.Formatters;
using System.Web;
using System.Net;

namespace FCFDFE.pages.MPMS.D
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
            int pageN = writer.PageNumber + 1;
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


    public partial class MPMS_D15 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strOVC_PURCH, strOVC_PURCH_5;
        short numONB_TIMES;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this, out isUpload))
            {
                FCommon.Controls_Attributes("readonly", "true", txtOVC_DO_DAPPROVE);
                strOVC_PURCH = Request.QueryString["OVC_PURCH"] == null ? "" : Request.QueryString["OVC_PURCH"].ToString();
                TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                numONB_TIMES = Request.QueryString["ONB_TIMES"] == null ? short.Parse("0") : short.Parse(Request.QueryString["ONB_TIMES"].ToString());

                if (IsOVC_DO_NAME() && !IsPostBack)
                {
                    #region 下拉是選單
                    FCommon.list_dataImportYN(drpOVC_RESULT_1, true, true);
                    FCommon.list_dataImportYN(drpOVC_RESULT_2, true, true);
                    FCommon.list_dataImportYN(drpOVC_RESULT_3, true, true);
                    FCommon.list_dataImportYN(drpOVC_RESULT_4, true, true);
                    #endregion

                    //隱藏不需要的CONTROL
                    lblOVC_RESULT_1.Visible = false;
                    lblOVC_RESULT_2.Visible = false;
                    lblOVC_RESULT_3.Visible = false;
                    lblOVC_RESULT_4.Visible = false;
                    lblOVC_ITEM_NAME_4.Visible = false;

                    //顯示委案單位已上傳檔案
                    AttchedDataImport();

                    //資料帶入頁面
                    DataImport();
                    GvFilesImport();
                }
            }
        }

        protected void drpOVC_ITEM_NAME_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOVC_ITEM_NAME.Text += txtOVC_ITEM_NAME.Text == "" ? "" : "\r\n";
            txtOVC_ITEM_NAME.Text += drpOVC_ITEM_NAME.SelectedValue;
        }



        #region Button OnClick

        protected void btnSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                TBMRECEIVE_BID_ITEM tbmBldItem = mpms.TBMRECEIVE_BID_ITEM
                    .Where(t => t.OVC_PURCH.Equals(strOVC_PURCH) && t.OVC_PURCH_5.Equals(strOVC_PURCH_5))
                    .Where(t => t.OVC_KIND.Equals("1") && t.ONB_ITEM == (i + 1)).FirstOrDefault();
                if (tbmBldItem != null)
                {
                    string strItem = (i + 1).ToString();
                    DropDownList drpOVC_RESULT = (DropDownList)this.Master.FindControl("MainContent").FindControl("drpOVC_RESULT_" + strItem);
                    tbmBldItem.OVC_RESULT = drpOVC_RESULT.SelectedValue == "Y" ? "是" : drpOVC_RESULT.SelectedValue == "N" ? "否" : null;

                    if (strItem.Equals("4"))
                    {
                        tbmBldItem.OVC_ITEM_NAME = txtOVC_ITEM_NAME_4.Text;
                    }

                    mpms.SaveChanges();
                }
            }

            //點擊 存檔
            TBMRECEIVE_BID tbmRECEIVE_BID = new TBMRECEIVE_BID();
            tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                               && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)).FirstOrDefault();
            if (tbmRECEIVE_BID == null)
            {
                //新增 採購收辦主檔
                TBMRECEIVE_BID tbmRECEIVE_BID_New = new TBMRECEIVE_BID();
                if (rdoOVC_DO_NAME_RESULT.SelectedValue != "")
                {
                    tbmRECEIVE_BID_New.OVC_DO_NAME_RESULT = rdoOVC_DO_NAME_RESULT.SelectedValue;
                    tbmRECEIVE_BID_New.OVC_DO_DAPPROVE = txtOVC_DO_DAPPROVE.Text;
                }
                mpms.TBMRECEIVE_BID.Add(tbmRECEIVE_BID_New);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmRECEIVE_BID_New.GetType().Name.ToString(), this, "新增");
            }
            else
            {
                //修改 採購收辦主檔
                if (rdoOVC_DO_NAME_RESULT.SelectedValue != "")
                {
                    tbmRECEIVE_BID.OVC_DO_NAME_RESULT = rdoOVC_DO_NAME_RESULT.SelectedValue;
                    tbmRECEIVE_BID.OVC_DO_DAPPROVE = txtOVC_DO_DAPPROVE.Text;
                }
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmRECEIVE_BID.GetType().Name.ToString(), this, "修改");
            }

            TBMRECEIVE_BID_ITEM tbmRECEIVE_BID_ITEM = mpms.TBMRECEIVE_BID_ITEM.
                Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                && tb.ONB_ITEM == 1 && tb.OVC_KIND.Equals("2")).FirstOrDefault();
            if (tbmRECEIVE_BID_ITEM != null)
            {
                //修改 承辦人-登管人員檢查項目

                tbmRECEIVE_BID_ITEM.OVC_ITEM_NAME = txtOVC_ITEM_NAME.Text;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmRECEIVE_BID_ITEM.GetType().Name.ToString(), this, "新增");
            }
            else
            {
                //新增 承辦人-登管人員檢查項目
                TBMRECEIVE_BID_ITEM tbmRECEIVE_BID_ITEM_New = new TBMRECEIVE_BID_ITEM
                {
                    OVC_PURCH = strOVC_PURCH,
                    OVC_PURCH_5 = strOVC_PURCH_5,
                    OVC_KIND = "2",
                    ONB_ITEM = 1,
                    OVC_ITEM_NAME = txtOVC_ITEM_NAME.Text,
                    OVC_RESULT = "否",
                };
                mpms.TBMRECEIVE_BID_ITEM.Add(tbmRECEIVE_BID_ITEM_New);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmRECEIVE_BID_ITEM_New.GetType().Name.ToString(), this, "修改");
            }

            if (numONB_TIMES != 0 && txtOVC_DO_DAPPROVE.Text != "")
            {
                //修改 OVC_STATUS="21" 核定 = 收辦(採購)的 階段結束日
                TBMSTATUS tbmSTATUS_21 = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                            && tb.ONB_TIMES == numONB_TIMES && tb.OVC_STATUS.Equals("21")).FirstOrDefault();
                if (tbmSTATUS_21 != null)
                {
                    tbmSTATUS_21.OVC_DEND = txtOVC_DO_DAPPROVE.Text;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmSTATUS_21.GetType().Name.ToString(), this, "修改");
                }

                string strOVC_DO_NAME = "";
                TBMRECEIVE_BID tbmRECEIVE_BID_DoName = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (tbmRECEIVE_BID != null)
                    strOVC_DO_NAME = tbmRECEIVE_BID.OVC_DO_NAME;

                //新增 OVC_STATUS="23" 招標 = 開標通知的 階段開始日
                TBMSTATUS tbmSTATUS_23 = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                            && tb.ONB_TIMES == numONB_TIMES && tb.OVC_STATUS.Equals("23")).FirstOrDefault();
                if (tbmSTATUS_23 == null)
                {
                    //新增 TBMSTATUS (購案階段紀錄檔)
                    TBMSTATUS tbmSTATUS_New = new TBMSTATUS
                    {
                        OVC_STATUS_SN = Guid.NewGuid(),
                        OVC_PURCH = strOVC_PURCH,
                        OVC_PURCH_5 = strOVC_PURCH_5,
                        ONB_TIMES = numONB_TIMES,
                        OVC_DO_NAME = strOVC_DO_NAME,
                        OVC_STATUS = "23",
                        OVC_DBEGIN = DateTime.Now.ToString("yyyy-MM-dd"),
                    };
                    mpms.TBMSTATUS.Add(tbmSTATUS_New);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmSTATUS_New.GetType().Name.ToString(), this, "新增");
                }
            
            }

            FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功！");
            //btnWork.Visible = true;
        }


        protected void btnBack_Click(object sender, EventArgs e)
        {
            //點擊 回上一頁
            string send_url = "~/pages/MPMS/D/MPMS_D14_1.aspx?OVC_PURCH=" + strOVC_PURCH ;
            Response.Redirect(send_url);
        }


        
        protected void btnWork_Click(object sender, EventArgs e)
        {
            //點擊Button 公告作業
            string send_url;
            TBMRECEIVE_ANNOUNCE tbmMRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
            tbmMRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                    && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)).FirstOrDefault();
            //TBMRECEIVE_ANNOUNCE 有資料(異動/新增) => 前往D16
            if (tbmMRECEIVE_ANNOUNCE != null)
                send_url = "~/pages/MPMS/D/MPMS_D16.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_PURCH_5=" + strOVC_PURCH_5;

            //TBMRECEIVE_ANNOUNCE 沒資料 => 前往D16_1
            else
            {
                send_url = "~/pages/MPMS/D/MPMS_D16_1.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_PURCH_5=" + strOVC_PURCH_5;    
            }
            Response.Redirect(send_url);

        }
        

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //按下上傳 => 檔案上傳
            if (isUpload)
            {
                if (FileUpload.HasFile)
                {
                    string serverPath = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D12"));
                    string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D12/" + strOVC_PURCH));
                    string fileName = FileUpload.FileName;
                    string serverFilePath = Path.Combine(serverDir, fileName);
                    //string strOVC_ATTACH_NAME = ViewState["OVC_ATTACH_NAME_UPLOAD"].ToString();
                    if (!Directory.Exists(serverPath))
                        Directory.CreateDirectory(serverPath);   //新增D12資料夾
                    if (!Directory.Exists(serverDir))
                        Directory.CreateDirectory(serverDir);   //新增購案編號資料夾
                    try
                    {
                        FileUpload.SaveAs(serverFilePath);
                        GvFilesImport();
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "檔案上傳成功");
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", error);
                    }
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先 選擇檔案");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無上傳權限");
        }


        #region 1.物資核定書
        protected void btnBook_Click(object sender, EventArgs e)
        {
            MaterialApplication_ExportToWord();
            string path_end = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/MA_Temp.docx");
            string filepath = strOVC_PURCH + "物資申請書.docx";
            string fileName = HttpUtility.UrlEncode(filepath);
            WebClient wc = new WebClient(); //宣告並建立WebClient物件
            byte[] b = wc.DownloadData(path_end); //載入要下載的檔案
            Response.Clear(); //清除Response內的HTML
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName); //設定標頭檔資訊 attachment 是本文章的關鍵字
            Response.BinaryWrite(b); //開始輸出讀取到的檔案
            File.Delete(path_end);
            Response.End();
        }
        protected void lbtnBook_pdf_Click(object sender, EventArgs e)
        {
            var query = mpms.TBM1301.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH)).Select(o => o.OVC_PURCH_KIND).FirstOrDefault();
            if (query.Equals("1"))
                PrintPDF();
            else
            {
                MaterialApplication_ExportToWord();
                string path_d = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/MA_Temp.docx");
                string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/MaterialApplication.pdf";
                WordcvDdf(path_d, wordfilepath);
                FileInfo file = new FileInfo(wordfilepath);
                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + strOVC_PURCH + "-物資申請書.pdf");
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(file.FullName);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.Flush();
                Response.End();
            }
        }
        protected void lbtnBook_odt_Click(object sender, EventArgs e)
        {
            MaterialApplication_ExportToWord();
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/MA_Temp.docx");
            string path_end = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/MA_Temp.odt");
            string filepath = strOVC_PURCH + "物資申請書.odt";
            FCommon.WordToOdt(this, path_temp, path_end, filepath);
        }
        #endregion

        #region 標單(有價款)
        protected void btnWordP_Click(object sender, EventArgs e)
        {
            string filepath = ListOfPlans_ExportToWord(true);
            FileInfo file = new FileInfo(filepath);
            //匯出Word檔提供下載
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + strOVC_PURCH + "標單(有價款).docx");
            Response.ContentType = "application/octet-stream";
            //Response.BinaryWrite(buffer);
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            File.Delete(filepath);
            Response.End();
        }
        protected void lbtnWordP_pdf_Click(object sender, EventArgs e)
        {
            string filepath = ListOfPlans_ExportToWord(true);
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/WordP_pdf.pdf";
            string FileName = strOVC_PURCH + "標單(有價款).pdf";
            FCommon.WordToPDF(this, filepath, filetemp, FileName);
        }
        protected void lbtnWordP_odt_Click(object sender, EventArgs e)
        {
            string filepath = ListOfPlans_ExportToWord(true);
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/WordP_odt.odt";
            string FileName = strOVC_PURCH + "標單(有價款).odt";
            FCommon.WordToOdt(this, filepath, filetemp, FileName);
        }
        #endregion

        protected void btnPdfP_Click(object sender, EventArgs e)
        {
            //標單(pdf有價款)
            ListOfPlans_ExportToPDF(true);
        }

        #region 標單(無價款)
        protected void btnWord_Click(object sender, EventArgs e)
        {
            //標單(WORD價款)
            string filepath = ListOfPlans_ExportToWord(false);
            FileInfo file = new FileInfo(filepath);
            //匯出Word檔提供下載
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + strOVC_PURCH + "標單(WORD無價款).docx");
            Response.ContentType = "application/octet-stream";
            //Response.BinaryWrite(buffer);
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            File.Delete(filepath);
            Response.End();
        }
        protected void lbtnWord_pdf_Click(object sender, EventArgs e)
        {
            string filepath = ListOfPlans_ExportToWord(false);
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/Word_pdf.pdf";
            string FileName = strOVC_PURCH + "標單(無價款).pdf";
            FCommon.WordToPDF(this, filepath, filetemp, FileName);
        }
        protected void lbtnWord_odt_Click(object sender, EventArgs e)
        {
            string filepath = ListOfPlans_ExportToWord(false);
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/Word_odt.odt";
            string FileName = strOVC_PURCH + "標單(無價款).odt";
            FCommon.WordToOdt(this, filepath, filetemp, FileName);
        }
        #endregion

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            //標單(pdf無價款)
            ListOfPlans_ExportToPDF(false);
        }

        #region 檢核表
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            //檢核表
            string filepath = OutputWordD15_1();
            string fileName = strOVC_PURCH + "-檢核表.docx";
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
        protected void lbtnCheck_pdf_Click(object sender, EventArgs e)
        {
            string filepath = OutputWordD15_1();
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/Check_pdf.pdf";
            string fileName = strOVC_PURCH + "-檢核表.pdf";
            FCommon.WordToPDF(this, filepath, filetemp, fileName);
        }
        protected void lbtnCheck_odt_Click(object sender, EventArgs e)
        {
            string filepath = OutputWordD15_1();
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/Word_odt.odt";
            string fileName = strOVC_PURCH + "-檢核表.odt";
            FCommon.WordToOdt(this, filepath, filetemp, fileName);
        }
        #endregion

        #endregion



        #region 副程式
        private bool IsOVC_DO_NAME()
        {
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strUserName, strDept = "";
            System.Data.DataTable dt = new System.Data.DataTable();
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



        private void DataImport()
        {
            //將資料庫資料帶出至畫面
            var query = gm.TBM1407;
            TBM1301 tbm1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();

            //設定 綜辦意見 RadioButtonList選項
            RdoImport(rdoOVC_DO_NAME_RESULT, "RA");

            if (tbm1301 != null)
            {
                //購案編號 + 採購單位地區 + 採購號碼
                lblOVC_PURCH_A_5.Text = tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                //購案名稱(中文)
                lblOVC_PUR_IPURCH.Text = tbm1301.OVC_PUR_IPURCH;
                //計畫申購單位
                lblOVC_PUR_NSECTION.Text = tbm1301.OVC_PUR_NSECTION;
                //計畫申購單位代碼
                lblOVC_PUR_SECTION.Text = tbm1301.OVC_PUR_SECTION;
                //申購人
                lblOVC_PUR_USER.Text = tbm1301.OVC_PUR_USER;
                //電話
                lblOVC_PUR_IUSER_PHONE.Text = tbm1301.OVC_PUR_IUSER_PHONE;
                //軍線
                lblOVC_PUR_IUSER_PHONE_EXT.Text = tbm1301.OVC_PUR_IUSER_PHONE_EXT;
                //採購屬性(代碼GN)
                lblOVC_LAB.Text = GetTbm1407Desc("GN", tbm1301.OVC_LAB);
                //招標方式(代碼C7)
                lblOVC_PUR_ASS_VEN_CODE.Text = GetTbm1407Desc("C7", tbm1301.OVC_PUR_ASS_VEN_CODE);
                //投標段次(代碼TG)
                lblOVC_BID_TIMES.Text = GetTbm1407Desc("TG", tbm1301.OVC_BID_TIMES);
                //決標原則(代碼M3)
                lblOVC_BID.Text = GetTbm1407Desc("M3", tbm1301.OVC_BID);


                TBMRECEIVE_BID tbmRECEIVE_BID = new TBMRECEIVE_BID();
                tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
                if (tbmRECEIVE_BID != null)
                {
                    //承辦人
                    lblOVC_DO_NAME.Text = tbmRECEIVE_BID.OVC_DO_NAME;
                    //收辦日
                    lblOVC_DRECEIVE.Text = GetTaiwanDate(tbmRECEIVE_BID.OVC_DRECEIVE);
                    //(登管人員)主官核批日
                    lblOVC_DAPPROVE.Text = tbmRECEIVE_BID.OVC_DAPPROVE == null? "" : GetTaiwanDate(tbmRECEIVE_BID.OVC_DAPPROVE);
                    //(登管人員)綜辦意見
                    lblOVC_NAME_RESULT.Text = GetTbm1407Desc("RA",tbmRECEIVE_BID.OVC_NAME_RESULT);
                    //(承辦人)綜辦意見
                    if (tbmRECEIVE_BID.OVC_DO_NAME_RESULT != null)
                        rdoOVC_DO_NAME_RESULT.SelectedValue = tbmRECEIVE_BID.OVC_DO_NAME_RESULT;
                    //(承辦人)收辦主官核批日
                    txtOVC_DO_DAPPROVE.Text = tbmRECEIVE_BID.OVC_DO_DAPPROVE;
                }


                TBMSTATUS tbmSTATUS = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) 
                    && tb.ONB_TIMES == numONB_TIMES && tb.OVC_STATUS=="21")
                    .OrderByDescending(tb => tb.OVC_STATUS=="" ? "0".Length : tb.OVC_STATUS == "3" ? "30".Length : tb.OVC_STATUS.Length)
                    .ThenByDescending(tb => tb.OVC_STATUS).FirstOrDefault();
                if (tbmSTATUS != null)
                {
                    //核評移案日 = 階段開始日
                    lblOVC_DBEGIN.Text = GetTaiwanDate(tbmSTATUS.OVC_DBEGIN);
                }


                //分案人-登管人員檢查項目
                var queryRECEIVE_BID_ITEM =
                    from tbmRECEIVE_BID_ITEM in mpms.TBMRECEIVE_BID_ITEM
                    where tbmRECEIVE_BID_ITEM.OVC_PURCH.Equals(strOVC_PURCH) && tbmRECEIVE_BID_ITEM.OVC_KIND.Equals("1")
                    select tbmRECEIVE_BID_ITEM;
                System.Data.DataTable dt = CommonStatic.LinqQueryToDataTable(queryRECEIVE_BID_ITEM);
                foreach (DataRow rows in dt.Rows)
                {
                    int numONB_ITEM = rows["ONB_ITEM"] != null ? int.Parse(rows["ONB_ITEM"].ToString()) : 0;
                    if (numONB_ITEM != 0 && numONB_ITEM < 5)
                    {
                        Label lblOVC_RESULT = (Label)this.Master.FindControl("MainContent").FindControl("lblOVC_RESULT_" + numONB_ITEM);
                        DropDownList drpOVC_RESULT = (DropDownList)this.Master.FindControl("MainContent").FindControl("drpOVC_RESULT_" + numONB_ITEM);
                        lblOVC_RESULT.Text = rows["OVC_RESULT"].ToString();
                        string strYN = rows["OVC_RESULT"].ToString() == "是" ? "Y" : rows["OVC_RESULT"].ToString() == "否" ? "N" : "";
                        FCommon.list_setValue(drpOVC_RESULT, strYN);
                    }
                    if(numONB_ITEM == 4)
                        lblOVC_ITEM_NAME_4.Text= rows["OVC_ITEM_NAME"].ToString();
                }


                //承辦人-登管人員檢查項目
                TBMRECEIVE_BID_ITEM tbmRECEIVE_BID_ITEM_2 =
                    mpms.TBMRECEIVE_BID_ITEM.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                    && tb.OVC_KIND.Equals("2") && tb.ONB_ITEM == 1).FirstOrDefault();
                if (tbmRECEIVE_BID_ITEM_2 != null)
                {
                    //btnWork.Visible = true;
                    txtOVC_ITEM_NAME.Text = tbmRECEIVE_BID_ITEM_2.OVC_ITEM_NAME;
                }
                else
                    btnWork.Visible = false;
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無此購案編號");
        }



        private void RdoImport(RadioButtonList rdo, string cateID)
        {
            //設定 綜辦意見 RadioButtonList選項
            rdo.Items.Clear();
            System.Data.DataTable dt;
            var query1407 = from tb1407 in gm.TBM1407 where tb1407.OVC_PHR_CATE.Equals(cateID) select tb1407;
            dt = CommonStatic.LinqQueryToDataTable(query1407);
            if (dt != null && dt.Rows.Count > 0)
            {
                rdo.DataSource = dt;
                rdo.DataValueField = "OVC_PHR_ID";
                rdo.DataTextField = "OVC_PHR_DESC";
                rdo.DataBind();
            }
        }


        public void GvFilesImport()
        {
            //資料夾檔案內容寫入GV
            System.Data.DataTable dtFile = new System.Data.DataTable();
            dtFile.Columns.Add("FileName", typeof(System.String));
            //dtFile.Columns.Add("Time", typeof(System.DateTime));
            dtFile.Columns.Add("Time", typeof(System.String));
            dtFile.Columns.Add("FileSize", typeof(System.Int32));
            string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D12/" + strOVC_PURCH));
            if (!Directory.Exists(serverDir))
            {
                Directory.CreateDirectory(serverDir);  //新增資料夾
            }
            DirectoryInfo filePath = new DirectoryInfo(serverDir);
            FileInfo[] fileList = filePath.GetFiles();  //擷取目錄下所有檔案內容，並存到 FileInfo Array
            if (fileList.Length != 0)
            {
                foreach (FileInfo file in fileList)
                {
                    string strFilePath = String.Format("<a href='/Uploadfile/MPMS/D/D12/" + strOVC_PURCH + "/" + file.Name + "' target='_blank'>" + file.Name + "</a>");
                    dtFile.Rows.Add(strFilePath, GetTaiwanDate(file.CreationTime.ToString()) + " " + file.CreationTime.Hour + ":" + file.CreationTime.Minute
                                    , file.Length / 1024);  //file.Length/1024 = KB
                }
                gvFiles.DataSource = dtFile;
                gvFiles.DataBind();
            }

            if (dtFile == null || dtFile.Rows.Count <= 0)
            {
                string[] fileField = { "FileName", "Time", "FileSize" };
                FCommon.GridView_setEmpty(gvFiles, fileField);
            }
        }


        private String GetTbm1407Desc(string cateID, string codeID)
        {
            //將TBM1407片語ID代碼轉為Desc
            TBM1407 tbm1407 = new TBM1407();
            if (codeID != null && codeID != "")
            {
                tbm1407 = gm.TBM1407.Where(table => table.OVC_PHR_CATE.Equals(cateID) && table.OVC_PHR_ID.Equals(codeID)).FirstOrDefault();
                if (tbm1407 != null)
                {
                    return tbm1407.OVC_PHR_DESC.ToString();
                }
            }
            return "";
        }

        

        public string GetTaiwanDate(string strDate)
        {
            //西元年轉民國年
            if (DateTime.TryParse(strDate, out DateTime datetime))
            {
                CultureInfo info = new CultureInfo("zh-TW");
                TaiwanCalendar twC = new TaiwanCalendar();
                info.DateTimeFormat.Calendar = twC;
                return datetime.ToString("yyy年MM月dd日", info);
            }
            else
                return string.Empty;
        }

        #endregion

        protected void GV_C_Alreadyupdate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            LinkButton lbtnDownloadFile = (LinkButton)gvr.FindControl("lbtnDownloadFile");
            switch (e.CommandName)
            {
                case "DownloadFile":
                    string FileName = lbtnDownloadFile.Text;
                    string FilePath = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/B/" + strOVC_PURCH + "/" + FileName));
                    if (File.Exists(FilePath))
                        FCommon.DownloadFile(this, FilePath);
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "找不到對應的檔案！");
                    break;
            }
        }


        #region 報表輸出

        #region PDF列印-物資申請書 內購
        private void PrintPDF()
        {
            string angcy = "";
            //PDF列印
            var doc1 = new iTextSharp.text.Document(PageSize.A4, 50, 50, 45, 50);
            MemoryStream Memory = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            Watermark wm = new Watermark();
            wm.strPurchNum = strOVC_PURCH;
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
                 where t.OVC_PURCH.Equals(strOVC_PURCH)
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
                     t.ONB_PUR_BUDGET,
                     t.OVC_PUR_SECTION,
                     t.OVC_SECTION_CHIEF,
                     t.OVC_AGNT_IN
                 }).FirstOrDefault();
            if (t1301.OVC_PURCH_KIND.Equals("1"))
                angcy = "內購";
            else
                angcy = "外購";
            var tablePurch = mpms.TBMPURCH_EXT.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
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
            string strp2 = t1301.OVC_PUR_DAPPROVE == null ? "" : "中華民國" + GetTaiwanDate(t1301.OVC_PUR_DAPPROVE) + "\n";
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
            string strPropose = t1301.OVC_DPROPOSE == null ? "" : "中華民國" + GetTaiwanDate(t1301.OVC_DPROPOSE) + "\n" + t1301.OVC_PROPOSE;
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
            var t1118_ISOURCE = mpms.TBM1118.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH)).GroupBy(o => o.OVC_ISOURCE).Select(o => o.Key);
            string Isource = String.Join(";", t1118_ISOURCE);
            PdfPCell cell4_3 = new PdfPCell(new Phrase(Isource, ChFont));
            table2.AddCell(cell4_3);
            PdfPCell cell5_2 = new PdfPCell(new Phrase("科目", ChFont));
            cell5_2.HorizontalAlignment = 1;
            table2.AddCell(cell5_2);
            var t1118_PJNAME = mpms.TBM1118.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH)).GroupBy(o => o.OVC_PJNAME).Select(o => o.Key);
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
                where t.OVC_PURCH.Equals(strOVC_PURCH)
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
                where t.OVC_IKIND.StartsWith(ovcIkind) && t.OVC_PURCH.Equals(strOVC_PURCH)
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
               where t.OVC_IKIND.StartsWith(ovcIkind) && t.OVC_PURCH.Equals(strOVC_PURCH)
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
            var queryComment = mpms.TBM1202.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH) && o.OVC_CHECK_OK.Equals("Y")).Select(o => o.OVC_APPROVE_COMMENT).FirstOrDefault();
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
            Response.AddHeader("Content-disposition", "attachment;filename=" + strOVC_PURCH + "-物資申請書.pdf");
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
            var purch = strOVC_PURCH;
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
                if (q.OVC_PURCH_KIND.Equals("1"))
                {
                    path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/MaterialApplication_in.docx");
                }
                else
                {
                    if (q.OVC_PUR_AGENCY != "F" && q.OVC_PUR_AGENCY != "W")
                        path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/MaterialApplication_Kind2.docx");
                    else
                        path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/MaterialApplication.docx");
                }
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

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/MA_Temp.docx");
                    //doc.SaveAs(Request.PhysicalApplicationPath + "Tempprint/b.docx");
                }
                buffer = ms.ToArray();
            }
        }
        
        #endregion


        #region 表單newRow
        private string GetAttached(string kind, string page)
        {
            var purch = lblOVC_PURCH_A_5.Text.Substring(0, 7);
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
            var purch = lblOVC_PURCH_A_5.Text.Substring(0, 7);
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
            var purch = lblOVC_PURCH_A_5.Text.Substring(0, 7);
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


        #region 輸出標單(WORD有價款)
        private string printListDoc()
        {
            string fileName = "計畫清單計評.docx";
            string TempName = "";
            string filePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + fileName));
            string outPutfilePath = "";
            var query1301 =
                (from t in gm.TBM1301
                 where t.OVC_PURCH.Equals(strOVC_PURCH)
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
            TempName = strOVC_PURCH + query1301.OVC_PUR_AGENCY + DateTime.Now.ToString("yyyyMMddHHmmss") + "計畫清單" + ".docx";
            outPutfilePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + TempName));
            File.Delete(outPutfilePath);
            File.Copy(filePath, outPutfilePath);
            var query1231 = mpms.TBM1231.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH)).ToList();
            var source1231 =
                from t in query1231
                group t by t.OVC_PURCH into g
                select new
                {
                    g.Key,
                    ISOURCE = string.Join(";", g.Select(o => o.OVC_ISOURCE))
                };
            var query1231APPROVE = mpms.TBM1231.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH));
            string AppDate = "";
            foreach (var item in query1231APPROVE)
                AppDate += GetTaiwanDate(item.OVC_PUR_DAPPR_PLAN) + "\t" + item.OVC_PUR_APPR_PLAN + "\r\n";
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
            valuesToFill.Fields.Add(new FieldContent("year", query1301.OVC_PURCH.Substring(2, 2)));
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
                where t.OVC_PURCH.Equals(strOVC_PURCH)
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
            var query12201 = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH));
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
                    string memo = item.OVC_MEMO.Replace("<br>", "").Replace(" ", "");
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


        #region 輸出標單(WORD無價款)
        private string printListDoc_NoPrice()
        {
            string fileName = "計畫清單計評.docx";
            string TempName = "";
            string filePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + fileName));
            string outPutfilePath = "";
            var query1301 =
                (from t in gm.TBM1301
                 where t.OVC_PURCH.Equals(strOVC_PURCH)
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
            TempName = strOVC_PURCH + query1301.OVC_PUR_AGENCY + DateTime.Now.ToString("yyyyMMddHHmmss") + "計畫清單" + ".docx";
            outPutfilePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + TempName));
            File.Delete(outPutfilePath);
            File.Copy(filePath, outPutfilePath);
            var query1231 = mpms.TBM1231.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH)).ToList();
            var source1231 =
                from t in query1231
                group t by t.OVC_PURCH into g
                select new
                {
                    g.Key,
                    ISOURCE = string.Join(";", g.Select(o => o.OVC_ISOURCE))
                };
            var query1231APPROVE = mpms.TBM1231.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH));
            string AppDate = "";
            foreach (var item in query1231APPROVE)
                AppDate += GetTaiwanDate(item.OVC_PUR_DAPPR_PLAN) + "\t" + item.OVC_PUR_APPR_PLAN + "\r\n";
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
            valuesToFill.Fields.Add(new FieldContent("year", query1301.OVC_PURCH.Substring(2, 2)));
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
                where t.OVC_PURCH.Equals(strOVC_PURCH)
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
                string strQORDER_BEF = " ";
                if (!string.IsNullOrEmpty(item.OVC_POI_IPURCH_BEF))
                {
                    strIPURCH_BEF = "案號：" + item.OVC_POI_IPURCH_BEF;
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
                    new FieldContent("ONB_POI_MPRICE_PLAN", ""),
                    new FieldContent("ONB_TOTAL", ""),
                    new FieldContent("NSN", item.NSN),
                    new FieldContent("OVC_BRAND", item.OVC_BRAND),
                    new FieldContent("OVC_MODEL", item.OVC_MODEL),
                    new FieldContent("OVC_POI_IREF", item.OVC_POI_IREF),
                    new FieldContent("OVC_FCODE", item.OVC_FCODE),
                    new FieldContent("OVC_POI_NDESC", item.OVC_POI_NDESC),
                    new FieldContent("OVC_POI_IPURCH_BEF", strIPURCH_BEF),
                    new FieldContent("OVC_CURR_MPRICE_BEF", ""),
                    new FieldContent("ONB_POI_MPRICE_BEF", ""),
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
            var query12201 = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH));
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
                    string memo = item.OVC_MEMO.Replace("<br>", "").Replace(" ", "");
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


        #region 附件內容
        private string GetAttached(string kind)
        {
            //附件內容
            var queryFile =
                mpms.TBM1119.AsEnumerable()
                .Where(o => o.OVC_PURCH.Equals(strOVC_PURCH) && o.OVC_IKIND.Equals(kind))
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


        #region 輸出標單
        public string ListOfPlans_ExportToWord(bool hasPrice)
        {
            string filePath = "", strWordame = "";
            if (hasPrice)
            {
                filePath = printListDoc();
                strWordame = "標單(WORD有價款).docx";
            }
            else
            {
                filePath = printListDoc_NoPrice();
                strWordame = "標單(WORD無價款).docx";
            }

            return filePath;
        }
        

        public void ListOfPlans_ExportToPDF(bool hasPrice)
        {
            string filePathWORD = "",strWordame = ""; ;
            if (hasPrice)
            {
                filePathWORD = printListDoc();
                strWordame = "標單(pdf有價款).pdf";
            }
            else
            {
                filePathWORD = printListDoc_NoPrice();
                strWordame = "標單(pdf無價款).pdf";
            }
            FileInfo fileWORD = new FileInfo(filePathWORD);
            string pdfPath = fileWORD.DirectoryName + "\\" + strOVC_PURCH + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
            WordcvDdf(filePathWORD, pdfPath);
            File.Delete(filePathWORD);
            //匯出PDF檔提供下載
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + strOVC_PURCH + strWordame);
            Response.ContentType = "application/octet-stream";
            //Response.BinaryWrite(buffer);
            Response.WriteFile(pdfPath);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            File.Delete(filePathWORD);
            File.Delete(pdfPath);
            Response.End();
        }

        #endregion


        #region 檢核表

        private string OutputWordD15_1()
        {
            //輸出檢核表

            var targetPath = Server.MapPath("~/WordPDFprint/");
            string fileName = strOVC_PURCH + "-檢核表.docx";
            File.Delete(targetPath + fileName);
            File.Copy(targetPath + "D15_1-檢核表.docx", targetPath + fileName);
            var valuesToFill = new TemplateEngine.Docx.Content();
            var tableContent = new TableContent("table");

            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5));

                //分案人-登管人員檢查項目
                var queryRECEIVE_BID_ITEM =
                    from tbmRECEIVE_BID_ITEM in mpms.TBMRECEIVE_BID_ITEM
                    where tbmRECEIVE_BID_ITEM.OVC_PURCH.Equals(strOVC_PURCH) && tbmRECEIVE_BID_ITEM.OVC_KIND.Equals("1")
                    select tbmRECEIVE_BID_ITEM;

                System.Data.DataTable dt = CommonStatic.LinqQueryToDataTable(queryRECEIVE_BID_ITEM);
                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow rows in dt.Rows)
                    {
                        string strColumnName = "";
                        int numONB_ITEM = rows["ONB_ITEM"] != null ? int.Parse(rows["ONB_ITEM"].ToString()) : 0;
                        if (numONB_ITEM != 0 && numONB_ITEM < 5)
                        {
                            strColumnName = "OVC_RESULT_" + numONB_ITEM;
                            valuesToFill.Fields.Add(new FieldContent("ONB_ITEM", numONB_ITEM.ToString()));
                            valuesToFill.Fields.Add(new FieldContent(strColumnName, rows["OVC_RESULT"] == null ? "" : rows["OVC_RESULT"].ToString()));
                        }
                        if (numONB_ITEM == 4)
                            valuesToFill.Fields.Add(new FieldContent("OVC_ITEM_NAME_4", rows["OVC_ITEM_NAME"] == null ? "" : rows["OVC_ITEM_NAME"].ToString()));
                    }
                }

                //綜辦意見
                TBMRECEIVE_BID tbmRECEIVE_BID = new TBMRECEIVE_BID();
                tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
                if (tbmRECEIVE_BID != null)
                {
                    //(登管人員)綜辦意見
                    valuesToFill.Fields.Add(new FieldContent("OVC_NAME_RESULT", tbmRECEIVE_BID.OVC_NAME_RESULT == null ? "" : GetTbm1407Desc("RA", tbmRECEIVE_BID.OVC_NAME_RESULT)));
                    //(承辦人)綜辦意見
                    valuesToFill.Fields.Add(new FieldContent("OVC_DO_NAME_RESULT", tbmRECEIVE_BID.OVC_DO_NAME_RESULT == null ? "" : GetTbm1407Desc("RA", tbmRECEIVE_BID.OVC_DO_NAME_RESULT)));
                }

                //承辦人-登管人員檢查項目
                TBMRECEIVE_BID_ITEM tbmRECEIVE_BID_ITEM_2 =
                    mpms.TBMRECEIVE_BID_ITEM.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                    && tb.OVC_KIND.Equals("2") && tb.ONB_ITEM == 1).FirstOrDefault();
                if (tbmRECEIVE_BID_ITEM_2 != null)
                {
                    if (tbmRECEIVE_BID_ITEM_2.OVC_ITEM_NAME != null)
                    {
                        string strOVC_ITEM_NAME = tbmRECEIVE_BID_ITEM_2.OVC_ITEM_NAME.ToString();
                        string[] strOVC_ITEM_NAMEs = strOVC_ITEM_NAME.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        int numONB_ITEM = 1;
                        foreach (string OVC_ITEM_NAME in strOVC_ITEM_NAMEs)
                        {
                            tableContent.AddRow(
                                new FieldContent("ONB_ITEM", numONB_ITEM.ToString()),
                                new FieldContent("OVC_ITEM_NAME", OVC_ITEM_NAME)
                            );
                            numONB_ITEM++;
                        }
                        valuesToFill.Tables.Add(tableContent);
                    }
                    else
                    {
                        tableContent.AddRow(
                                new FieldContent("ONB_ITEM", ""),
                                new FieldContent("OVC_ITEM_NAME", "")
                            );
                        valuesToFill.Tables.Add(tableContent);
                    }
                }
                else
                {
                    tableContent.AddRow(
                                new FieldContent("ONB_ITEM", ""),
                                new FieldContent("OVC_ITEM_NAME", "")
                            );
                    valuesToFill.Tables.Add(tableContent);
                }
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


        #endregion



        #region 顯示檔案資料 //顯示委案單位已上傳檔案
        bool hasRows = false, isUpload;

        private void AttchedDataImport()
        {
            string[] field = { "OVC_IKIND", "OVC_ATTACH_NAME", "OVC_FILE_NAME", "ONB_QTY", "ONB_PAGES" };
            var query =
                from t in mpms.TBM1119
                where t.OVC_PURCH.Equals(strOVC_PURCH)
                select new
                {
                    OVC_IKIND = t.OVC_IKIND == "D" ? "採購計畫清單" : "物資申請書",
                    t.OVC_ATTACH_NAME,
                    t.OVC_FILE_NAME,
                    t.ONB_QTY,
                    t.ONB_PAGES
                };
            System.Data.DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            hasRows = FCommon.GridView_dataImport(GV_C_Alreadyupdate, dt, field);
        }

        #endregion

    }
}