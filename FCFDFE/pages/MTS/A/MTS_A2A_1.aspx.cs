using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System.IO;
using FCFDFE.Entity.GMModel;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A2A_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        GMEntities GM = new GMEntities();
        MTSEntities MTSE = new MTSEntities();
        Common FCommon = new Common();

        #region 副程式
        private void dataImport()
        {
            string strMessage = "";
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strODT_REQUIRE_DATE = txtODT_REQUIRE_DATE.Text;
            ViewState["OVC_BLD_NO"] = strOVC_BLD_NO;
            ViewState["ODT_REQUIRE_DATE"] = strODT_REQUIRE_DATE;

            if (strOVC_BLD_NO.Equals(string.Empty))
                strMessage += "<P> 請填入 提單編號 </p>";

            if (strMessage.Equals(string.Empty))
            {
                var query =
                    from etr in MTSE.TBGMT_ETR.AsEnumerable()
                    select new
                    {
                        OVC_BLD_NO = etr.OVC_BLD_NO,
                        ODT_REQUIRE_DATE = FCommon.getDateTime(etr.ODT_REQUIRE_DATE),
                        ODT_REQUIRE_DATE_Value = etr.ODT_REQUIRE_DATE,
                        ODT_RECEIVE_DATE = FCommon.getDateTime(etr.ODT_RECEIVE_DATE),
                        ODT_PROCESS_DATE = FCommon.getDateTime(etr.ODT_PROCESS_DATE),
                        ODT_STRATEGY_PROCESS_DATE = FCommon.getDateTime(etr.ODT_STRATEGY_PROCESS_DATE),
                        ODT_TEL_DATE = FCommon.getDateTime(etr.ODT_TEL_DATE),
                        ODT_PASS_DATE = FCommon.getDateTime(etr.ODT_PASS_DATE),
                        ODT_SHIP_START_DATE = FCommon.getDateTime(etr.ODT_SHIP_START_DATE),
                        ODT_RETURN_DATE = FCommon.getDateTime(etr.ODT_RETURN_DATE)
                    };
                if (!strOVC_BLD_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
                if (!strODT_REQUIRE_DATE.Equals(string.Empty))
                    query = query.Where(table => table.ODT_REQUIRE_DATE_Value != null && table.ODT_REQUIRE_DATE_Value == Convert.ToDateTime(strODT_REQUIRE_DATE));
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);

                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_ETR, dt);
            }
            else
                FCommon.AlertShow(PnWarning, "danger", "系統訊息", strMessage);
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", ViewState["OVC_BLD_NO"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_REQUIRE_DATE", ViewState["ODT_REQUIRE_DATE"], true);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);

                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtODT_REQUIRE_DATE);

                    bool boolImport = false;
                    string strOVC_BLD_NO, strODT_REQUIRE_DATE;
                    if (FCommon.getQueryString(this, "OVC_BLD_NO", out strOVC_BLD_NO, true))
                    {
                        txtOVC_BLD_NO.Text = strOVC_BLD_NO;
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "ODT_REQUIRE_DATE", out strODT_REQUIRE_DATE, true))
                        txtODT_REQUIRE_DATE.Text = strODT_REQUIRE_DATE;
                    if (boolImport) dataImport();
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }
        
        protected void GVTBGMT_ETR_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GVTBGMT_ETR.DataKeys[gvrIndex].Value.ToString();

            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);

            switch (e.CommandName)
            {
                case "btnModify":
                    Response.Redirect($"MTS_A2A_2{ strQueryString }");
                    break;
                case "btnDel":
                    Response.Redirect($"MTS_A2A_3{ strQueryString }");
                    break;
                case "btnPrint":
                    word(id);

                    //FileInfo file = new FileInfo(filePath);
                    //string pdfPath = file.DirectoryName + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                    //string filename = "國防部國防採購室軍品出口作業時程管制表.pdf";
                    //FCommon.WordToPDF(this, filePath, pdfPath, filename);

                    //WordcvDdf(filePath, pdfPath);
                    //File.Delete(filePath);
                    ////匯出PDF檔供下載
                    //Response.Clear();
                    //Response.AppendHeader("Content-Disposition", "attachment; filename=" + "國防部國防採購室軍品出口作業時程管制表.pdf");
                    //Response.ContentType = "application/octet-stream";
                    ////Response.BinaryWrite(buffer);
                    //Response.WriteFile(pdfPath);
                    //Response.OutputStream.Flush();
                    //Response.OutputStream.Close();
                    //Response.Flush();
                    //File.Delete(filePath);
                    //File.Delete(pdfPath);
                    //Response.End();
                    break;
                case "btnPrint_2":
                    string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MTS/A/" + id));
                    if (Directory.Exists(serverDir))
                    {
                        string[] files = Directory.GetFiles(serverDir);
                        foreach (string f in files)
                        {
                            string fileName = f.Replace(serverDir + "\\", "");
                            string path = f;
                            FileInfo filepath = new FileInfo(path);
                            Response.Clear();
                            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                            Response.ContentType = "application/octet-stream";
                            Response.WriteFile(filepath.FullName);
                            Response.OutputStream.Flush();
                            Response.OutputStream.Close();
                            Response.Flush();
                            Response.End();
                        }
                    }
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無提單下載！");
                    break;
                default:
                    break;
            }
        }
        protected void GVTBGMT_ETR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                GridView theGridView = (GridView)sender;
                int index = gvr.RowIndex;
                HyperLink hlkOVC_BLD_NO = (HyperLink)gvr.FindControl("hlkOVC_BLD_NO");
                Button btnPrint = (Button)gvr.FindControl("btnPrint");
                Button btnPrint_2 = (Button)gvr.FindControl("btnPrint_2");
                if (FCommon.Controls_isExist(hlkOVC_BLD_NO))
                {
                    string strOVC_BLD_NO = hlkOVC_BLD_NO.Text;
                    hlkOVC_BLD_NO.NavigateUrl = $"javascript: OpenWindow_BLDDATA('{ FCommon.getEncryption(strOVC_BLD_NO) }');";
                }
                #region 單位
                string userid = Session["userid"].ToString();
                string dept = GM.ACCOUNTs.Where(o => o.USER_ID.Equals(userid)).FirstOrDefault().DEPT_SN;
                if (dept != "0A100" && dept != "00N00")
                    btnPrint.Visible = false;
                else
                    btnPrint_2.Visible = false;
                #endregion
            }
        }
        protected void GVTBGMT_ETR_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        #region word文件
        private void word(string BLD_NO)
        {

            #region 資料載入
            DataTable dt = new DataTable();
            var query =
                from etr in MTSE.TBGMT_ETR.AsEnumerable()
                join EDF in MTSE.TBGMT_EDF.AsEnumerable() on etr.OVC_BLD_NO equals EDF.OVC_BLD_NO
                join ESO in MTSE.TBGMT_ESO.AsEnumerable() on etr.OVC_BLD_NO equals ESO.OVC_BLD_NO
                join ECL in MTSE.TBGMT_ECL.AsEnumerable() on etr.OVC_BLD_NO equals ECL.OVC_BLD_NO
                where BLD_NO.Equals(etr.OVC_BLD_NO)
                select new
                {
                    etr.OVC_BLD_NO,
                    etr.ODT_REQUIRE_DATE,
                    etr.OVC_REQUIRE_MSG_NO,
                    etr.ODT_RECEIVE_DATE,
                    etr.OVC_RECEIVE_MSG_NO,
                    etr.ODT_PROCESS_DATE,
                    etr.OVC_PROCESS_MSG_NO,
                    etr.ODT_STRATEGY_PROCESS_DATE,
                    etr.ODT_TEL_DATE,
                    etr.ODT_PASS_DATE,
                    etr.ODT_SHIP_START_DATE,
                    etr.ODT_RETURN_DATE,
                    etr.OVC_DELAY_DESCRIPTION,
                    EDF.OVC_PURCH_NO,
                    EDF.ODT_VALIDITY_DATE,
                    ESO.ODT_STORED_DATE,
                    EDF.OVC_IS_STRATEGY,
                    ECL.ODT_EXP_DATE
                    };

            dt = CommonStatic.LinqQueryToDataTable(query);
            #endregion
            #region 因權限無法使用
            //1.複製至目標路徑

            //string fileName = "A2A_Template.docx";
            //string TempName = "";
            //string filePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + fileName));
            //string outPutfilePath = "";


            //2.設定檔案內容

            //FieldContent部分


            //TempName = DateTime.Now.ToString("yyyyMMddHHmmss") + "國防部國防採購室軍品出口作業時程管制表" + ".docx";
            //outPutfilePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + TempName));
            //File.Copy(filePath, outPutfilePath);
            //string strDateFormatTaiwan = "{0} ／ {1} ／ {2}";
            //DataRow dr = dt.Rows[0];
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    using (Xceed.Words.NET.DocX doc = Xceed.Words.NET.DocX.Load(filePath))
            //    {
            //        doc.ReplaceText("[$top_num$]", Convert.ToString(dt.Rows[0][0]), false, System.Text.RegularExpressions.RegexOptions.None);
            //        doc.ReplaceText("[$Export_num$]", Convert.ToString(dr["OVC_PURCH_NO"].ToString()), false, System.Text.RegularExpressions.RegexOptions.None);
            //        doc.ReplaceText("[$Cexport_Time$]", FCommon.getTaiwanDate_Assign((Convert.ToString(dt.Rows[0][1])), strDateFormatTaiwan, 2), false, System.Text.RegularExpressions.RegexOptions.None);
            //        doc.ReplaceText("[$OVC_REQUIRE_MSG_NO$]", Convert.ToString(dt.Rows[0][2]), false, System.Text.RegularExpressions.RegexOptions.None);
            //        doc.ReplaceText("[$R_Time$]", FCommon.getTaiwanDate_Assign((Convert.ToString(dt.Rows[0][3])), strDateFormatTaiwan, 2), false, System.Text.RegularExpressions.RegexOptions.None);
            //        doc.ReplaceText("[$OVC_RECEIVE_MSG_NO$]", Convert.ToString(dt.Rows[0][4]), false, System.Text.RegularExpressions.RegexOptions.None);
            //        doc.ReplaceText("[$FF_Time$]", FCommon.getTaiwanDate_Assign((Convert.ToString(dt.Rows[0][5])), strDateFormatTaiwan, 2), false, System.Text.RegularExpressions.RegexOptions.None);
            //        doc.ReplaceText("[$OVC_PROCESS_MSG_NO$]", Convert.ToString(dt.Rows[0][6]), false, System.Text.RegularExpressions.RegexOptions.None);
            //        string strODT_STRATEGY_PROCESS_DATE = FCommon.getTaiwanDate_Assign(dr["ODT_STRATEGY_PROCESS_DATE"].ToString(), strDateFormatTaiwan, 2);
            //        doc.ReplaceText("[$RS_Time$]", strODT_STRATEGY_PROCESS_DATE, false, System.Text.RegularExpressions.RegexOptions.None);
            //        if (dr["OVC_IS_STRATEGY"].ToString() == "是")
            //        {
            //            doc.ReplaceText("[$SVP_Time$]", FCommon.getTaiwanDate_Assign(Convert.ToString(dr["ODT_VALIDITY_DATE"]), strDateFormatTaiwan, 2), false, System.Text.RegularExpressions.RegexOptions.None);
            //        }
            //        else
            //        {
            //            doc.ReplaceText("[$SVP_Time$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
            //        }
            //        doc.ReplaceText("[$TIntake_Time$]", FCommon.getTaiwanDate_Assign((Convert.ToString(dt.Rows[0][8])), strDateFormatTaiwan, 2), false, System.Text.RegularExpressions.RegexOptions.None);
            //        string strODT_STORED_DATE = FCommon.getTaiwanDate_Assign(dr["ODT_STORED_DATE"].ToString(), strDateFormatTaiwan, 2);
            //        doc.ReplaceText("[$Intake_Time$]", strODT_STORED_DATE, false, System.Text.RegularExpressions.RegexOptions.None);
            //        string strODT_EXP_DATE = FCommon.getTaiwanDate_Assign(dr["ODT_EXP_DATE"].ToString(), strDateFormatTaiwan, 2);
            //        doc.ReplaceText("[$T_Time$]", strODT_EXP_DATE, false, System.Text.RegularExpressions.RegexOptions.None);
            //        doc.ReplaceText("[$P_Time$]", FCommon.getTaiwanDate_Assign((Convert.ToString(dt.Rows[0][9])), strDateFormatTaiwan, 2), false, System.Text.RegularExpressions.RegexOptions.None);
            //        doc.ReplaceText("[$D_Time$]", FCommon.getTaiwanDate_Assign((Convert.ToString(dt.Rows[0][10])), strDateFormatTaiwan, 2), false, System.Text.RegularExpressions.RegexOptions.None);
            //        doc.ReplaceText("[$L_Time$]", FCommon.getTaiwanDate_Assign((Convert.ToString(dt.Rows[0][11])), strDateFormatTaiwan, 2), false, System.Text.RegularExpressions.RegexOptions.None);
            //        doc.ReplaceText("[$Note$]", Convert.ToString(dt.Rows[0][12]), false, System.Text.RegularExpressions.RegexOptions.None);
            //        doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/PS_Temp.docx");
            //    }
            //}
            //string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/PS_Temp.docx";
            //return filetemp;
            //var valuesToFill = new TemplateEngine.Docx.Content();
            //valuesToFill.Fields.Add(new FieldContent("top_num", Convert.ToString(dt.Rows[0][0])));
            //valuesToFill.Fields.Add(new FieldContent("Export_num", Convert.ToString(dr["OVC_PURCH_NO"].ToString())));
            //valuesToFill.Fields.Add(new FieldContent("Cexport_Time", FCommon.getTaiwanDate_Assign((Convert.ToString(dt.Rows[0][1])), strDateFormatTaiwan, 2)));
            //valuesToFill.Fields.Add(new FieldContent("OVC_REQUIRE_MSG_NO", Convert.ToString(dt.Rows[0][2])));
            //valuesToFill.Fields.Add(new FieldContent("R_Time", FCommon.getTaiwanDate_Assign((Convert.ToString(dt.Rows[0][3])), strDateFormatTaiwan, 2)));
            //valuesToFill.Fields.Add(new FieldContent("OVC_RECEIVE_MSG_NO", Convert.ToString(dt.Rows[0][4])));
            //valuesToFill.Fields.Add(new FieldContent("FF_Time", FCommon.getTaiwanDate_Assign((Convert.ToString(dt.Rows[0][5])), strDateFormatTaiwan, 2)));
            //valuesToFill.Fields.Add(new FieldContent("OVC_PROCESS_MSG_NO", Convert.ToString(dt.Rows[0][6])));
            //string strODT_STRATEGY_PROCESS_DATE = FCommon.getTaiwanDate_Assign(dr["ODT_STRATEGY_PROCESS_DATE"].ToString(), strDateFormatTaiwan, 2);
            //valuesToFill.Fields.Add(new FieldContent("RS_Time", strODT_STRATEGY_PROCESS_DATE));
            //if (dr["OVC_IS_STRATEGY"].ToString() == "是")
            //{
            //    valuesToFill.Fields.Add(new FieldContent("SVP_Time", FCommon.getTaiwanDate_Assign(Convert.ToString(dr["ODT_VALIDITY_DATE"]), strDateFormatTaiwan, 2)));
            //}
            //else
            //{
            //    valuesToFill.Fields.Add(new FieldContent("SVP_Time", ""));
            //}
            //valuesToFill.Fields.Add(new FieldContent("TIntake_Time", FCommon.getTaiwanDate_Assign((Convert.ToString(dt.Rows[0][8])), strDateFormatTaiwan, 2)));
            //string strODT_STORED_DATE = FCommon.getTaiwanDate_Assign(dr["ODT_STORED_DATE"].ToString(), strDateFormatTaiwan,2);
            //valuesToFill.Fields.Add(new FieldContent("Intake_Time", strODT_STORED_DATE));
            //string strODT_EXP_DATE = FCommon.getTaiwanDate_Assign(dr["ODT_EXP_DATE"].ToString(), strDateFormatTaiwan,2);
            //valuesToFill.Fields.Add(new FieldContent("T_Time", strODT_EXP_DATE));
            //valuesToFill.Fields.Add(new FieldContent("P_Time", FCommon.getTaiwanDate_Assign((Convert.ToString(dt.Rows[0][9])), strDateFormatTaiwan, 2)));
            //valuesToFill.Fields.Add(new FieldContent("D_Time", FCommon.getTaiwanDate_Assign((Convert.ToString(dt.Rows[0][10])), strDateFormatTaiwan, 2)));
            //valuesToFill.Fields.Add(new FieldContent("L_Time", FCommon.getTaiwanDate_Assign((Convert.ToString(dt.Rows[0][11])), strDateFormatTaiwan, 2)));
            //valuesToFill.Fields.Add(new FieldContent("Note", Convert.ToString(dt.Rows[0][12])));
            //儲存變更 C:\Users\linon\Desktop\FCFDFE0619\FCFDFE\FCFDFE
            //using (TemplateProcessor outputDocument = new TemplateProcessor(outPutfilePath).SetRemoveContentControls(true))
            //{
            //    outputDocument.FillContent(valuesToFill);
            //    outputDocument.SaveChanges();
            //}
            //return outPutfilePath;
            #endregion
            
            string today = (int.Parse(DateTime.Now.Year.ToString()) - 1911).ToString() + DateTime.Now.ToString("年MM月dd日");
            string path = Request.PhysicalApplicationPath;//取得檔案絕對路徑
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChTitle = new Font(bfChinese, 14, Font.BOLD);
            Font ChTabTitle = new Font(bfChinese, 8f);
            Font ChFont = new Font(bfChinese, 7f);
            var doc1 = new Document(PageSize.A4, 0, 0, 50, 80);
            MemoryStream Memory = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            doc1.Open();

            PdfPTable table1 = new PdfPTable(4);
            table1.TotalWidth = 1200F;
            table1.SetWidths(new float[] { 2, 1, 1, 2 });
            table1.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table1.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell title = new PdfPCell(new Phrase("國防部國防採購室軍品出口作業時程管制表\n", ChTitle));
            title.VerticalAlignment = Element.ALIGN_MIDDLE;
            title.HorizontalAlignment = Element.ALIGN_CENTER;
            //title.Border = Rectangle.NO_BORDER;
            title.Colspan = 4;
            table1.AddCell(title);
            //PdfPCell title_time = new PdfPCell(new Phrase("", ChTitle));
            //title_time.VerticalAlignment = Element.ALIGN_MIDDLE;
            //title_time.HorizontalAlignment = Element.ALIGN_CENTER;
            //title_time.Border = Rectangle.NO_BORDER;
            //title_time.Colspan = 4;
            //table1.AddCell(title_time);
            //PdfPCell title_time_2 = new PdfPCell(new Phrase("印表日期：" + today + "\n\n", ChTabTitle));
            //title_time_2.VerticalAlignment = Element.ALIGN_MIDDLE;
            //title_time_2.HorizontalAlignment = Element.ALIGN_CENTER;
            //title_time_2.Border = Rectangle.NO_BORDER;
            //title_time_2.Colspan = 2;
            //table1.AddCell(title_time_2);

            //table1.AddCell(new Phrase("投保通知書編號", ChTabTitle));
            //table1.AddCell(new Phrase("結報日期", ChTabTitle));
            //table1.AddCell(new Phrase("軍種", ChTabTitle));
            //table1.AddCell(new Phrase("承保商", ChTabTitle));
            //table1.AddCell(new Phrase("保費金額(新台幣)", ChTabTitle));
            //table1.AddCell(new Phrase("支付日期", ChTabTitle));

            //int i = 0;
            //string strFormat_Money = "#,0";
            //foreach (DataRow dr in dt.Rows)
            //{
            //    string strNo = (i + 1).ToString(); //序號
            //    decimal.TryParse(dr["ONB_INS_AMOUNT"].ToString(), out decimal decONB_INS_AMOUNT);
            //
            //    table1.AddCell(new Phrase(dr["OVC_EINN_NO"].ToString(), ChFont));
            //    table1.AddCell(new Phrase(dr["ODT_APPLY_DATE"].ToString(), ChFont));
            //    table1.AddCell(new Phrase(dr["OVC_CLASS_NAME"].ToString(), ChFont));
            //    table1.AddCell(new Phrase(dr["OVC_SHIP_COMPANY"].ToString(), ChFont));
            //    table1.AddCell(new Phrase(decONB_INS_AMOUNT.ToString(strFormat_Money), ChFont));
            //    table1.AddCell(new Phrase(dr["ODT_INS_DATE"].ToString(), ChFont));
            //
            //    i++;
            //}

            doc1.Add(table1);
            doc1.Close();

            string strFileName = "國防部國防採購室軍品出口作業時程管制表.pdf";
            FCommon.DownloadFile(this, strFileName, Memory);
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