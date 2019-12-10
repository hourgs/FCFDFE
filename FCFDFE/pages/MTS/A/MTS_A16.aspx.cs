using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using TemplateEngine.Docx;
using System.IO;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A16 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();

#region 副程式
        private void dataImport()
        {
            string strMessage = "";
            string strODT_IDR_DATE = txtODT_IDR_DATE.Text;
            string strOVC_TRANSER_DEPT_CDE = drpOVC_TRANSER_DEPT_CDE.SelectedValue;
            //ViewState["ODT_IDR_DATE"] = strODT_IDR_DATE;
            //ViewState["OVC_TRANSER_DEPT_CDE"] = strOVC_TRANSER_DEPT_CDE;
            
            bool boolODT_IDR_DATE = FCommon.checkDateTime(strODT_IDR_DATE, "日期", ref strMessage, out DateTime dateODT_IDR_DATE);
            if (strODT_IDR_DATE.Equals(string.Empty) && strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                strMessage += "<P> 至少填入一個選項 </p>";
            
            if (strMessage.Equals(string.Empty))
            {
                var tbm1407 = GME.TBM1407
                    .Where(t => t.OVC_PHR_CATE.Equals("TR"))
                    .Where(t => (!string.IsNullOrEmpty(drpOVC_TRANSER_DEPT_CDE.SelectedValue) ? t.OVC_PHR_PARENTS.Equals(strOVC_TRANSER_DEPT_CDE) : true));
                //政賢測試
                var query =
                from ICR in MTSE.TBGMT_ICR.AsEnumerable()
                join BLD in MTSE.TBGMT_BLD.AsEnumerable() on ICR.OVC_BLD_NO equals BLD.OVC_BLD_NO
                join IDR in MTSE.TBGMT_IDR.AsEnumerable() on BLD.OVC_BLD_NO equals IDR.OVC_BLD_NO
                join chn in GME.TBMDEPTs.AsEnumerable() on ICR.OVC_RECEIVE_DEPT_CODE equals chn.OVC_DEPT_CDE into ps
                from chn in ps.DefaultIfEmpty()
                join tb1407 in tbm1407 on BLD.OVC_ARRIVE_PORT equals tb1407.OVC_PHR_ID into t
                from tb1407 in t.DefaultIfEmpty()
                join arrport in MTSE.TBGMT_PORTS on BLD.OVC_ARRIVE_PORT equals arrport.OVC_PORT_CDE
                where arrport.OVC_IS_ABROAD.Equals("國內")
                select new
                {
                    OVC_BLD_NO = BLD.OVC_BLD_NO,
                    OVC_PURCH_NO = ICR.OVC_PURCH_NO,
                    OVC_CHI_NAME = ICR.OVC_CHI_NAME,
                    ODT_IMPORT_DATE = FCommon.getDateTime(ICR.ODT_IMPORT_DATE),
                    ODT_CUSTOM_DATE = FCommon.getDateTime(ICR.ODT_CUSTOM_DATE),
                    ODT_PASS_CUSTOM_DATE = FCommon.getDateTime(ICR.ODT_PASS_CUSTOM_DATE),
                    ODT_UNPACKING_DATE = FCommon.getDateTime(ICR.ODT_UNPACKING_DATE),
                    ODT_TRANSFER_DATE = FCommon.getDateTime(ICR.ODT_TRANSFER_DATE),
                    ODT_RECEIVE_DATE = FCommon.getDateTime(ICR.ODT_RECEIVE_DATE),
                    OVC_TRANS_TYPE = ICR.OVC_TRANS_TYPE,
                    OVC_RECEIVE_DEPT_CODE = chn != null ? chn.OVC_ONNAME : "", //接收單位
                    OVC_RECEIVE_DEPT_CODE_Value = ICR.OVC_RECEIVE_DEPT_CODE, //接收單位代碼
                    ONB_QUANITY = BLD.ONB_QUANITY,
                    ONB_WEIGHT = BLD.ONB_WEIGHT,
                    ONB_VOLUME = BLD.ONB_VOLUME,
                    OVC_NOTE = ICR.OVC_NOTE,
                    //ODT_IDR_DATE = IDR.ODT_IDR_DATE,
                    ODT_RECEIVE_INF_DATE = ICR.ODT_RECEIVE_INF_DATE, //收單日期
                };

                if (boolODT_IDR_DATE)
                {
                    query = query.Where(table => (DateTime.TryParse(table.ODT_RECEIVE_DATE, out DateTime dateODT_RECEIVE_DATE) && DateTime.Compare(dateODT_RECEIVE_DATE, dateODT_IDR_DATE) >= 0)); //接收日期
                }
                if (!strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                {
                    var table = GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("TR"))
                        .Where(t => t.OVC_PHR_PARENTS.Equals(strOVC_TRANSER_DEPT_CDE))
                        .Select(t => t.OVC_PHR_ID).ToArray();
                    query = query.Where(t => table.Contains(t.OVC_RECEIVE_DEPT_CODE_Value));
                }
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_ICR, dt);
                ViewState["dt"] = dt;
                btnPrint.Visible = dt.Rows.Count > 0;
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        //private string getQueryString()
        //{
        //    string strQueryString = "";
        //    FCommon.setQueryString(ref strQueryString, "ODT_IDR_DATE", ViewState["ODT_IDR_DATE"], true);
        //    FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", ViewState["OVC_TRANSER_DEPT_CDE"], true);
        //    return strQueryString;
        //}
#endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    txtODT_IDR_DATE.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    FCommon.Controls_Attributes("readonly", "true", txtODT_IDR_DATE);
                    #region 匯入下拉式選單
                    string strFirstText = "不限定";
                    CommonMTS.list_dataImport_TransferArea(drpOVC_TRANSER_DEPT_CDE, true, strFirstText); //接轉地區
                    #endregion
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            #region 權限問題無法使用
            //string outPutfilePath = "";
            //DateTime dateNow = DateTime.Now;
            //if (ViewState["dt"] != null)
            //{
            //    #region 資料載入
            //    DataTable dt = (DataTable)ViewState["dt"];
            //
            //    #endregion
            //    string userName = "";
            //    string userTel = "";
            //    if (Session["userid"] != null)
            //    {
            //        string userID = Session["userid"].ToString();
            //        var queryUser = GME.ACCOUNTs.Where(o => o.USER_ID.Equals(userID)).Select(o => new { o.USER_NAME, o.IUSER_PHONE }).FirstOrDefault();
            //        userName = queryUser.USER_NAME;
            //        userTel = queryUser.IUSER_PHONE;
            //    }
            //
            //    //1.複製至目標路徑
            //
            //    string fileName = "A16_Template.docx";
            //    string TempName = "";
            //    string filePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + fileName));
            //
            //
            //    //2.設定檔案內容
            //
            //    //FieldContent部分
            //
            //    TempName = dateNow.ToString("yyyyMMddHHmmss") + "國防部國防採購室軍品出口作業時程管制表" + ".docx";
            //    outPutfilePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + TempName));
            //    File.Copy(filePath, outPutfilePath);
            //
            //    var valuesToFill = new TemplateEngine.Docx.Content();
            //    string strDateFormatTaiwan = "{0}/{1}/{2}";
            //    string strNow = FCommon.getDateTime(dateNow);
            //    var tableContent = new TableContent("table");
            //    int i = 1;
            //    int iCount = 0;
            //    double iWeight = 0;
            //    double iVolume = 0;
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        string strONB_QUANITY= String.Format("{0:N}", dr["ONB_QUANITY"]);
            //        string strONB_WEIGHT = String.Format("{0:N}", dr["ONB_WEIGHT"]);
            //        string strONB_VOLUME = String.Format("{0:N}", dr["ONB_VOLUME"]);
            //        int.TryParse(strONB_QUANITY, out int pCount);
            //        double.TryParse(strONB_WEIGHT, out double pWeight);
            //        double.TryParse(strONB_VOLUME, out double pVolume);
            //        //DateTime datetemp;
            //        //DateTime.TryParse(dr["OVC_TRANS_TYPE"].ToString(), out datetemp);
            //        string ODT_IMPORT_DATE =   FCommon.getDateTime((dr["ODT_IMPORT_DATE"]));
            //        string ODT_RECEIVE_INF_DATE =  FCommon.getDateTime((dr["ODT_RECEIVE_INF_DATE"]));
            //        string ODT_CUSTOM_DATE = FCommon.getDateTime((dr["ODT_CUSTOM_DATE"]));
            //        string ODT_PASS_CUSTOM_DATE = FCommon.getDateTime((dr["ODT_PASS_CUSTOM_DATE"]));
            //        string ODT_UNPACKING_DATE = FCommon.getDateTime((dr["ODT_UNPACKING_DATE"]));
            //        string ODT_TRANSFER_DATE = FCommon.getDateTime((dr["ODT_TRANSFER_DATE"]));
            //        string ODT_RECEIVE_DATE = FCommon.getDateTime((dr["ODT_RECEIVE_DATE"]));
            //
            //        iCount += pCount;
            //        iWeight += pWeight;
            //        iVolume += pVolume;
            //        tableContent.AddRow(
            //        new FieldContent("C0", (i).ToString()),//提單號
            //        new FieldContent("C1", dr["OVC_BLD_NO"].ToString()),//提單號
            //        new FieldContent("C2", dr["OVC_PURCH_NO"].ToString()),//暗號
            //        new FieldContent("C3", dr["OVC_CHI_NAME"].ToString()),//品名
            //        new FieldContent("C4", ODT_IMPORT_DATE),//進口日期
            //        new FieldContent("C5", ODT_RECEIVE_INF_DATE),//收單日期
            //        new FieldContent("C6", ODT_CUSTOM_DATE),//報關日期
            //        new FieldContent("C7", ODT_PASS_CUSTOM_DATE),//通關日期
            //        new FieldContent("C8", ODT_UNPACKING_DATE),//拆櫃
            //        new FieldContent("C9", ODT_TRANSFER_DATE),//清運
            //        new FieldContent("C10", ODT_RECEIVE_DATE),//接收
            //        new FieldContent("C11", dr["OVC_TRANS_TYPE"].ToString()),//清運方式
            //        new FieldContent("C12", dr["OVC_RECEIVE_DEPT_CODE"].ToString()),//接收單位
            //        new FieldContent("C13", strONB_QUANITY),//件數
            //        new FieldContent("C14", strONB_WEIGHT),//重量
            //        new FieldContent("C15", strONB_VOLUME),//體積
            //        new FieldContent("C16", dr["OVC_NOTE"].ToString())//備註
            //        );
            //        i++;
            //    }
            //    //頁首
            //    //valuesToFill.Fields.Add(new FieldContent("top_num", Convert.ToString(dt.Rows[0][0])));
            //    valuesToFill.Fields.Add(new FieldContent("T1", (userName)));
            //    valuesToFill.Fields.Add(new FieldContent("T2", (userTel)));
            //    valuesToFill.Fields.Add(new FieldContent("T4", (strNow)));
            //    valuesToFill.Fields.Add(new FieldContent("T5", (strNow)));
            //    ////表格尾
            //    tableContent.AddRow(
            //        new FieldContent("C0", ""),//提單號
            //        new FieldContent("C1", "共" + dt.Rows.Count + "筆"),//提單號
            //        new FieldContent("C2", ""),//暗號
            //        new FieldContent("C3", ""),//品名
            //        new FieldContent("C4", ""),//進口日期
            //        new FieldContent("C5", ""),//收單日期
            //        new FieldContent("C6", ""),//報關日期
            //        new FieldContent("C7", ""),//通關日期
            //        new FieldContent("C8", ""),//拆櫃
            //        new FieldContent("C9", ""),//清運
            //        new FieldContent("C10", ""),//接收
            //        new FieldContent("C11", ""),//清運方式
            //        new FieldContent("C12", ""),//接收單位
            //        new FieldContent("C13", String.Format("{0:N}", iCount) + "件"),//件數
            //        new FieldContent("C14", String.Format("{0:N}", iWeight) + "噸"),//重量
            //        new FieldContent("C15", String.Format("{0:N}", iVolume) + ""),//備註
            //        new FieldContent("C16", "")//備註
            //        );
            //    valuesToFill.Tables.Add(tableContent);
            //    //儲存變更 C:\Users\linon\Desktop\FCFDFE0619\FCFDFE\FCFDFE
            //    using (TemplateProcessor outputDocument = new TemplateProcessor(outPutfilePath).SetRemoveContentControls(true))
            //    {
            //        outputDocument.FillContent(valuesToFill);
            //        outputDocument.SaveChanges();
            //    }
            //
            //}
            //FileInfo file = new FileInfo(outPutfilePath);
            //string pdfPath = file.DirectoryName + "\\" + dateNow.ToString("yyyyMMddHHmmss") + ".pdf";
            //WordcvDdf(outPutfilePath, pdfPath);
            //File.Delete(outPutfilePath);
            //string filename = System.Web.HttpUtility.UrlEncode("國防部國防採購室運況管制日報表");
            ////匯出PDF檔供下載
            //Response.Clear();
            //Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + ".pdf");
            //Response.ContentType = "application/octet-stream";
            ////Response.BinaryWrite(buffer);
            //Response.WriteFile(pdfPath);
            //Response.OutputStream.Flush();
            //Response.OutputStream.Close();
            //Response.Flush();
            //File.Delete(outPutfilePath);
            //File.Delete(pdfPath);
            //Response.End();
            #endregion
            if (ViewState["hasRows"] != null)
            {
                Document doc = new Document(PageSize.A4.Rotate(), 10, 10, 50, 20);
                MemoryStream Memory = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(doc, Memory);
                // PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(@"C:\Users\Jonathan\Desktop\test.pdf", FileMode.Create)); //指定路徑創造

                writer.PageEvent = new Header();
                doc.Open();

                //設定字型
                BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H
              , BaseFont.NOT_EMBEDDED);
                Font ChFont = new Font(bfChinese, 6, Font.NORMAL, BaseColor.BLACK);
                string chFontPath = "c:\\windows\\fonts\\KAIU.TTF";
                BaseFont chBaseFont = BaseFont.CreateFont(chFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                PdfContentByte cb = writer.DirectContent;
                Rectangle pageSize = doc.PageSize;
                cb.SetRGBColorFill(0, 0, 0);
                cb.BeginText();
                cb.SetFontAndSize(chBaseFont, 10);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "國防部國防採購室運況管制日報表", pageSize.GetLeft(400), pageSize.GetTop(25), 0);
                cb.EndText();

                PdfPTable pdftable = new PdfPTable(new float[] { 1, 5, 5, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2 });
                pdftable.TotalWidth = 780f;
                pdftable.LockedWidth = true;
                pdftable.DefaultCell.FixedHeight = 30;


                string[] title = { "項次", "提單號", "案號", "品名", "進口日期", "收單日期", "報關日期", "通關日期", "拆櫃日期", "清運日期", "接收日期", "清運方式", "接收單位", "件數", "重量噸", "體積噸", "備考" };
                for (int i = 0; i < title.Length; i++)
                {
                    PdfPCell cell_title = new PdfPCell(new Phrase(title[i], ChFont));
                    cell_title.FixedHeight = 30;
                    cell_title.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_title.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdftable.AddCell(cell_title);
                }
                if (ViewState["dt"] != null)
                {
                    DataTable print_query = (DataTable)ViewState["dt"];
                    int i = 0;
                    string strFormat_Money = "#,0";
                    foreach (DataRow dr in print_query.Rows)
                    {
                        string strNo = (i + 1).ToString(); //序號
                        decimal.TryParse(dr["ONB_WEIGHT"].ToString(), out decimal decONB_WEIGHT);
                        decimal.TryParse(dr["ONB_VOLUME"].ToString(), out decimal decONB_VOLUME);

                        pdftable.AddCell(new Phrase(strNo, ChFont));
                        pdftable.AddCell(new Phrase(dr["OVC_BLD_NO"].ToString(), ChFont));
                        pdftable.AddCell(new Phrase(dr["OVC_PURCH_NO"].ToString(), ChFont));
                        pdftable.AddCell(new Phrase(dr["OVC_CHI_NAME"].ToString(), ChFont));
                        pdftable.AddCell(new Phrase(dr["ODT_IMPORT_DATE"].ToString(), ChFont));
                        pdftable.AddCell(new Phrase(FCommon.getDateTime(dr["ODT_RECEIVE_INF_DATE"].ToString(), "yyyy-MM-dd"), ChFont));
                        pdftable.AddCell(new Phrase(dr["ODT_CUSTOM_DATE"].ToString(), ChFont));
                        pdftable.AddCell(new Phrase(dr["ODT_PASS_CUSTOM_DATE"].ToString(), ChFont));
                        pdftable.AddCell(new Phrase(dr["ODT_UNPACKING_DATE"].ToString(), ChFont));
                        pdftable.AddCell(new Phrase(dr["ODT_TRANSFER_DATE"].ToString(), ChFont));
                        pdftable.AddCell(new Phrase(dr["ODT_RECEIVE_DATE"].ToString(), ChFont));
                        pdftable.AddCell(new Phrase(dr["OVC_TRANS_TYPE"].ToString(), ChFont));
                        pdftable.AddCell(new Phrase(dr["OVC_RECEIVE_DEPT_CODE"].ToString(), ChFont));
                        pdftable.AddCell(new Phrase(dr["ONB_QUANITY"].ToString(), ChFont));
                        pdftable.AddCell(new Phrase(decONB_WEIGHT.ToString(strFormat_Money), ChFont));
                        pdftable.AddCell(new Phrase(decONB_VOLUME.ToString(strFormat_Money), ChFont));
                        pdftable.AddCell(new Phrase(dr["OVC_NOTE"].ToString(), ChFont));

                        i++;
                    }
                }
                doc.Add(pdftable);
                doc.Close();

                string strFileName = $"國防部國防採購室運況管制日報表.pdf";
                FCommon.DownloadFile(this, strFileName, Memory);
            }
        }

        #region gridview
        //有資料時顯示統計用footer
        protected void GVTBGMT_ICR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = e.Row;
            int sum1 = theGridView.Rows.Count, sumONB_QUANITY = 0;
            decimal sumONB_WEIGHT = 0, sumONB_VOLUME = 0;
            switch (gvr.RowType)
            {
                case DataControlRowType.DataRow:
                    int index = gvr.RowIndex;
                    HyperLink hlkOVC_BLD_NO = (HyperLink)gvr.FindControl("hlkOVC_BLD_NO");
                    if (FCommon.Controls_isExist(hlkOVC_BLD_NO))
                    {
                        string strOVC_BLD_NO = hlkOVC_BLD_NO.Text;
                        hlkOVC_BLD_NO.NavigateUrl = $"javascript: OpenWindow_BLDDATA('{ FCommon.getEncryption(strOVC_BLD_NO) }');";
                    }
                    break;
                //統計footer
                case DataControlRowType.Footer:
                    gvr.Cells[1].Text = "共" + sum1 + "項";
                    for (int i = 1; i < sum1 - 1; i++)
                    {
                        try
                        {
                            int.TryParse(theGridView.Rows[i].Cells[12].Text, out int pCount);
                            decimal.TryParse(theGridView.Rows[i].Cells[13].Text, out decimal pWeight);
                            decimal.TryParse(theGridView.Rows[i].Cells[14].Text, out decimal pVolume);
                            sumONB_QUANITY += pCount;
                            sumONB_WEIGHT += pWeight;
                            sumONB_VOLUME += pVolume;
                        }
                        catch { }
                    };
                    gvr.Cells[12].Text = sumONB_QUANITY.ToString();
                    gvr.Cells[13].Text = sumONB_WEIGHT.ToString();
                    gvr.Cells[14].Text = sumONB_VOLUME.ToString();
                    break;
                default:
                    break;
            }
        }
        protected void GVTBGMT_ICR_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
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