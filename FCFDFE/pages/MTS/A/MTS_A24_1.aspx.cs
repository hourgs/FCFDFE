using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;


namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A24_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        Common FCommon = new Common();

        #region 副程式
        private void dataImport()
        {
            string strMessage = "";
            string strOVC_EDF_NO = txtOVC_EDF_NO.Text;
            string strOVC_TRANSER_DEPT_CDE = drpOVC_TRANSER_DEPT_CDE.SelectedValue;
            string strOVC_START_PORT = drpOVC_START_PORT.SelectedValue;
            string strOVC_ARRIVE_PORT = txtOVC_PORT_CDE.Text;
            string strOVC_SHIP_COMPANY = drpOVC_SHIP_COMPANY.SelectedValue;
            string strOVC_SO_NO = txtOVC_SO_NO.Text;
            string strOVC_STORED_COMPANY = drpOVC_STORED_COMPANY.SelectedValue;

            ViewState["OVC_EDF_NO"] = strOVC_EDF_NO;
            ViewState["OVC_TRANSER_DEPT_CDE"] = strOVC_TRANSER_DEPT_CDE;
            ViewState["OVC_START_PORT"] = strOVC_START_PORT;
            ViewState["OVC_ARRIVE_PORT"] = strOVC_ARRIVE_PORT;
            ViewState["OVC_SHIP_COMPANY"] = strOVC_SHIP_COMPANY;
            ViewState["OVC_SO_NO"] = strOVC_SO_NO;
            ViewState["OVC_STORED_COMPANY"] = strOVC_STORED_COMPANY;

            if (strOVC_EDF_NO.Equals(string.Empty) && strOVC_TRANSER_DEPT_CDE.Equals(string.Empty) && strOVC_START_PORT.Equals(string.Empty) && strOVC_ARRIVE_PORT.Equals(string.Empty)
                && strOVC_SHIP_COMPANY.Equals(string.Empty) && strOVC_SO_NO.Equals(string.Empty))
                strMessage += "<P> 至少填入一個選項 </p>";

            if (strMessage.Equals(string.Empty))
            {
                var query =
                    from eso in MTSE.TBGMT_ESO.AsEnumerable()
                    join edf in MTSE.TBGMT_EDF.AsEnumerable() on eso.OVC_EDF_NO equals edf.OVC_EDF_NO
                    join arrport in MTSE.TBGMT_PORTS on edf.OVC_ARRIVE_PORT equals arrport.OVC_PORT_CDE
                    join strport in MTSE.TBGMT_PORTS on edf.OVC_START_PORT equals strport.OVC_PORT_CDE
                    select new
                    {
                        edf.EDF_SN,
                        edf.OVC_EDF_NO,
                        edf.OVC_DEPT_CDE,
                        OVC_START_PORT = strport.OVC_PORT_CHI_NAME,
                        OVC_ARRIVE_PORT = arrport.OVC_PORT_CHI_NAME,
                        OVC_START_PORT_Value = edf.OVC_START_PORT,
                        OVC_ARRIVE_PORT_Value = edf.OVC_ARRIVE_PORT,
                        OVC_SHIP_COMPANY = eso.OVC_SHIP_COMPANY ?? "",
                        eso.OVC_SO_NO,
                        eso.OVC_BLD_NO,
                        OVC_STORED_COMPANY = eso.OVC_STORED_COMPANY ?? ""
                    };
                if (!strOVC_EDF_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_EDF_NO.Contains(strOVC_EDF_NO));
                if (!strOVC_START_PORT.Equals(string.Empty))
                    query = query.Where(table => table.OVC_START_PORT_Value.Equals(strOVC_START_PORT));
                if (!strOVC_ARRIVE_PORT.Equals(string.Empty))
                    query = query.Where(table => table.OVC_ARRIVE_PORT_Value.Equals(strOVC_ARRIVE_PORT));
                if (!strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                {
                    var table = GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("TR"))
                        .Where(t => t.OVC_PHR_PARENTS.Equals(strOVC_TRANSER_DEPT_CDE))
                        .Select(t => t.OVC_PHR_ID).ToArray();

                    query = query.Where(t => table.Contains(t.OVC_START_PORT_Value));
                }
                if (!strOVC_SHIP_COMPANY.Equals(string.Empty))
                    query = query.Where(table => table.OVC_SHIP_COMPANY.Equals(strOVC_SHIP_COMPANY));
                if (!strOVC_SO_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_SO_NO.Equals(strOVC_SO_NO));
                if (!strOVC_STORED_COMPANY.Equals(string.Empty))
                    query = query.Where(table => table.OVC_STORED_COMPANY.Equals(strOVC_STORED_COMPANY));
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnWarning, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_ESO, dt);
            }
            else
                FCommon.AlertShow(PnWarning, "danger", "系統訊息", strMessage);
        }
        public void PDFBooking_orders(string strEDF_SN, string[] list)
        {
            if (Guid.TryParse(strEDF_SN, out Guid guidEDF_SN))
            {
                //string strOVC_EDF_NO = EDF_NO;
                string strOVC_TRANSER_DEPT_CDE = list[0];
                string strOVC_START_PORT = list[1];
                string strOVC_ARRIVE_PORT = list[2];
                string strOVC_SHIP_COMPANY = list[3];
                string strOVC_SO_NO = list[4];

                var query =
                    from eso in MTSE.TBGMT_ESO
                    join edf in MTSE.TBGMT_EDF.AsEnumerable() on eso.OVC_EDF_NO equals edf.OVC_EDF_NO into ps1
                    from edf in ps1.DefaultIfEmpty()
                    where edf.EDF_SN.Equals(guidEDF_SN)
                    join ports in MTSE.TBGMT_PORTS.AsEnumerable() on edf.OVC_ARRIVE_PORT equals ports.OVC_PORT_CDE into ps2
                    from ports in ps2.DefaultIfEmpty()
                    join ports2 in MTSE.TBGMT_PORTS.AsEnumerable() on edf.OVC_START_PORT equals ports2.OVC_PORT_CDE into ps3
                    from ports2 in ps3.DefaultIfEmpty()
                    select new
                    {
                        OVC_EDF_NO = edf != null ? edf.OVC_EDF_NO ?? "" : "", //表單編號
                        OVC_DEPT_CDE = edf != null ? edf.OVC_DEPT_CDE : "",//接轉地區
                        OVC_START_PORT = edf != null ? edf.OVC_START_PORT ?? "" : "",//啟運港
                        OVC_ARRIVE_PORT = edf != null ? edf.OVC_ARRIVE_PORT ?? "" : "",//目的港(機場)
                        OVC_SHIP_COMPANY = eso.OVC_SHIP_COMPANY ?? "",// 航空公司
                        OVC_SO_NO = eso.OVC_SO_NO ?? "", //S/O編號 裝艙單
                        eso.OVC_BLD_NO, //提單編號
                        eso.OVC_SHIP_NAME,//船名
                        eso.OVC_VOYAGE,//航次
                        eso.ODT_START_DATE, //送貨日
                        eso.ODT_ACT_ARRIVE_DATE, //抵達日
                        eso.OVC_CONTAINER_TYPE, //裝貨區分
                        eso.ODT_STORED_DATE,//進倉時間
                        OVC_PURCH_NO = edf != null ? edf.OVC_PURCH_NO : "",//暗號
                        eso.OVC_CUSTOM_CLR_PLACE,//結關地點
                        OVC_PORT_CHI_NAME = ports != null ? ports.OVC_PORT_CHI_NAME : "",
                        text = ports2 != null ? ports2.OVC_PORT_CHI_NAME : "",
                    };
                //if (!strOVC_EDF_NO.Equals(string.Empty))
                //    query = query.Where(table => table.OVC_EDF_NO.Contains(strOVC_EDF_NO));
                if (!strOVC_START_PORT.Equals(string.Empty))
                    query = query.Where(table => table.OVC_START_PORT == strOVC_START_PORT);
                if (!strOVC_ARRIVE_PORT.Equals(string.Empty))
                    query = query.Where(table => table.OVC_ARRIVE_PORT == strOVC_ARRIVE_PORT);
                if (!strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                {
                    var table = GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("TR"))
                        .Where(t => t.OVC_PHR_PARENTS.Equals(strOVC_TRANSER_DEPT_CDE))
                        .Select(t => t.OVC_PHR_ID).ToArray();

                    query = query.Where(t => table.Contains(t.OVC_START_PORT));
                }
                if (!strOVC_SHIP_COMPANY.Equals(string.Empty))
                    query = query.Where(table => table.OVC_SHIP_COMPANY == strOVC_SHIP_COMPANY);
                if (!strOVC_SO_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_SO_NO == strOVC_SO_NO);
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    string strOVC_EDF_NO = dr["OVC_EDF_NO"].ToString();

                    BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
                    Font ChFont = new Font(bfChinese, 12f, Font.BOLD);
                    Font Title_ChFont = new Font(bfChinese, 18f, Font.BOLD);
                    MemoryStream Memory = new MemoryStream();
                    var doc1 = new Document(PageSize.A4, 50, 50, 80, 50);
                    PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
                    DateTime PrintTime = DateTime.Now;
                    doc1.Open();

                    PdfPTable Firsttable = new PdfPTable(2);
                    Firsttable.SetWidths(new float[] { 3, 8 });

                    Firsttable.TotalWidth = 500F;
                    Firsttable.LockedWidth = true;
                    Firsttable.DefaultCell.FixedHeight = 40f;
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    PdfPCell Title_Text = new PdfPCell(new Phrase("訂艙單", Title_ChFont));
                    Title_Text.Colspan = 2;
                    Title_Text.FixedHeight = 40f;
                    Title_Text.HorizontalAlignment = Element.ALIGN_CENTER;
                    Title_Text.VerticalAlignment = Element.ALIGN_MIDDLE;
                    Title_Text.Border = Rectangle.NO_BORDER;
                    Firsttable.AddCell(Title_Text);
                    PdfPCell secTitle_Text = new PdfPCell(new Phrase("表單編號：" + strOVC_EDF_NO + "\n\n列印日期：" + PrintTime.ToString("yyyy/MM/dd"), ChFont));
                    secTitle_Text.Colspan = 2;
                    secTitle_Text.FixedHeight = 50f;
                    secTitle_Text.HorizontalAlignment = Element.ALIGN_RIGHT;
                    secTitle_Text.VerticalAlignment = Element.ALIGN_MIDDLE;
                    secTitle_Text.Border = Rectangle.NO_BORDER;
                    String firstlead = "  ";
                    Firsttable.AddCell(secTitle_Text);
                    Firsttable.AddCell(new Phrase("目的港(機場)", ChFont));
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Firsttable.AddCell(new Paragraph(firstlead + dr[15].ToString(), ChFont));
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Firsttable.AddCell(new Phrase("啟運港(機場)", ChFont));
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Firsttable.AddCell(new Paragraph(firstlead + dr[16].ToString(), ChFont));
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Firsttable.AddCell(new Phrase("船(機)名/航次", ChFont));
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Firsttable.AddCell(new Paragraph(firstlead + dr[7].ToString() + " / " + dr[8].ToString(), ChFont) { FirstLineIndent = 10f });
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Firsttable.AddCell(new Phrase("船(航空)公司", ChFont));
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Firsttable.AddCell(new Paragraph(firstlead + dr[4].ToString(), ChFont) { FirstLineIndent = 10f });
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Firsttable.AddCell(new Phrase(0, "裝艙單", ChFont));
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Firsttable.AddCell(new Paragraph(firstlead + dr[5].ToString(), ChFont) { FirstLineIndent = 10f });
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Firsttable.AddCell(new Phrase("裝貨區分", ChFont));
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Firsttable.AddCell(new Paragraph(firstlead + dr[11].ToString(), ChFont) { FirstLineIndent = 10f });
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Firsttable.AddCell(new Phrase("進艙時間", ChFont));
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Firsttable.AddCell(new Paragraph(firstlead + dr[12].ToString(), ChFont) { FirstLineIndent = 10f });
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Firsttable.AddCell(new Phrase("開航日", ChFont));
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Firsttable.AddCell(new Paragraph(firstlead + dr[9].ToString(), ChFont) { FirstLineIndent = 10f });
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Firsttable.AddCell(new Phrase("預計抵達日", ChFont));
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Firsttable.AddCell(new Paragraph(firstlead + dr[10].ToString(), ChFont) { FirstLineIndent = 10f });
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Firsttable.AddCell(new Phrase("送貨地點", ChFont));
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Firsttable.AddCell(new Paragraph(firstlead + dr[1].ToString(), ChFont) { FirstLineIndent = 10f });
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Firsttable.AddCell(new Phrase("貨物存放處所", ChFont));
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Firsttable.AddCell(new Paragraph(firstlead + dr[1].ToString(), ChFont) { FirstLineIndent = 10f });
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Firsttable.AddCell(new Phrase("領櫃地點", ChFont));
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Firsttable.AddCell(new Phrase("", ChFont));
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Firsttable.AddCell(new Phrase("案號", ChFont));
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Firsttable.AddCell(new Paragraph(firstlead + dr[13].ToString(), ChFont) { FirstLineIndent = 10f });
                    PdfPCell tail_text = new PdfPCell(new Phrase("P.S. 結關資料若有變動請務必予結關當日通知以免延誤，謝謝！", ChFont));
                    tail_text.Colspan = 2;
                    tail_text.Border = Rectangle.NO_BORDER;
                    tail_text.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tail_text.HorizontalAlignment = Element.ALIGN_LEFT;
                    Firsttable.AddCell(tail_text);
                    doc1.Add(Firsttable);
                    
                    doc1.Close();
                    string strFileName = $"訂艙單 - { strOVC_EDF_NO }.pdf";
                    FCommon.DownloadFile(this, strFileName, Memory);
                }
                else
                    FCommon.AlertShow(PnWarning, "danger", "系統訊息", "無此資料！");
            }
            else
                FCommon.AlertShow(PnWarning, "danger", "系統訊息", "編號錯誤！");
        }
        public void PDFLoading_list(string strEDF_SN, string[] list)
        {
            if (Guid.TryParse(strEDF_SN, out Guid guidEDF_SN))
            {
                //string strOVC_EDF_NO = EDF_NO;
                string strOVC_TRANSER_DEPT_CDE = list[0];
                string strOVC_START_PORT = list[1];
                string strOVC_ARRIVE_PORT = list[2];
                string strOVC_SHIP_COMPANY = list[3];
                string strOVC_SO_NO = list[4];
                var query =
                    from eso in MTSE.TBGMT_ESO
                    join edf in MTSE.TBGMT_EDF.AsEnumerable() on eso.OVC_EDF_NO equals edf.OVC_EDF_NO into ps1
                    from edf in ps1.DefaultIfEmpty()
                    where edf.EDF_SN.Equals(guidEDF_SN)
                    join ports in MTSE.TBGMT_PORTS.AsEnumerable() on edf.OVC_ARRIVE_PORT equals ports.OVC_PORT_CDE into ps2
                    from ports in ps2.DefaultIfEmpty()
                    join edf_dtail in MTSE.TBGMT_EDF_DETAIL.AsEnumerable() on eso.OVC_EDF_NO equals edf_dtail.OVC_EDF_NO into ps3
                    from edf_dtail in ps3.DefaultIfEmpty()
                    join ports2 in MTSE.TBGMT_PORTS.AsEnumerable() on edf.OVC_START_PORT equals ports2.OVC_PORT_CDE into ps4
                    from ports2 in ps4.DefaultIfEmpty()
                    select new
                    {
                        OVC_EDF_NO = edf != null ? edf.OVC_EDF_NO ?? "" : "", //表單編號
                    OVC_START_PORT = edf != null ? edf.OVC_START_PORT ?? "" : "",//啟運港
                    OVC_ARRIVE_PORT = edf != null ? edf.OVC_ARRIVE_PORT ?? "" : "",//目的港(機場)
                    OVC_SHIP_COMPANY = eso.OVC_SHIP_COMPANY ?? "",// 航空公司
                    OVC_SO_NO = eso.OVC_SO_NO ?? "", //S/O編號 裝艙單
                    eso.OVC_SHIP_NAME,
                        eso.OVC_VOYAGE,
                        OVC_SHIP_FROM = edf != null ? edf.OVC_SHIP_FROM : "",//ROC
                    OVC_CON_ENG_ADDRESS = edf != null ? edf.OVC_CON_ENG_ADDRESS : "",//CONSIGNEE
                    OVC_CON_TEL = edf != null ? edf.OVC_CON_TEL : "",
                        OVC_CON_FAX = edf != null ? edf.OVC_CON_FAX : "",
                        OVC_NP_ENG_ADDRESS = edf != null ? edf.OVC_NP_ENG_ADDRESS : "",
                        OVC_NP_TEL = edf != null ? edf.OVC_NP_TEL : "",
                        OVC_NP_FAX = edf != null ? edf.OVC_NP_FAX : "",
                        OVC_ANP_ENG_ADDRESS = edf != null ? edf.OVC_ANP_ENG_ADDRESS : "",
                        OVC_ANP_TEL = edf != null ? edf.OVC_ANP_TEL : "",
                        OVC_ANP_FAX = edf != null ? edf.OVC_ANP_FAX : "",
                        OVC_ANP_ENG_ADDRESS2 = edf != null ? edf.OVC_ANP_ENG_ADDRESS2 : "",
                        OVC_ANP_TEL2 = edf != null ? edf.OVC_ANP_TEL2 : "",
                        OVC_ANP_FAX2 = edf != null ? edf.OVC_ANP_FAX2 : "",
                        OVC_PURCH_NO = edf != null ? edf.OVC_PURCH_NO : "",
                        OVC_BOX_NO = edf_dtail != null ? edf_dtail.OVC_BOX_NO : "",
                        OVC_EDF_ITEM_NO = edf_dtail != null ? edf_dtail.OVC_EDF_ITEM_NO : 0,
                        OVC_CHI_NAME = edf_dtail != null ? edf_dtail.OVC_CHI_NAME : "",
                        OVC_ENG_NAME = edf_dtail != null ? edf_dtail.OVC_ENG_NAME : "",
                        OVC_ITEM_NO = edf_dtail != null ? edf_dtail.OVC_ITEM_NO : "",
                        OVC_ITEM_NO2 = edf_dtail != null ? edf_dtail.OVC_ITEM_NO2 : "",
                        OVC_ITEM_NO3 = edf_dtail != null ? edf_dtail.OVC_ITEM_NO3 : "",
                        OVC_NOTE = edf != null ? edf.OVC_NOTE : "",
                        ONB_WEIGHT = edf_dtail != null ? edf_dtail.ONB_WEIGHT : 0,
                        OVC_WEIGHT_UNIT = edf_dtail != null ? edf_dtail.OVC_WEIGHT_UNIT : "",
                        ONB_VOLUME = edf_dtail != null ? edf_dtail.ONB_VOLUME : 0,
                        OVC_VOLUME_UNIT = edf_dtail != null ? edf_dtail.OVC_VOLUME_UNIT : "",
                        OVC_PORT_ENG_NAME = ports != null ? ports.OVC_PORT_ENG_NAME : "",
                        text = ports2 != null ? ports2.OVC_PORT_ENG_NAME : "",
                        OVC_PAYMENT_TYPE = edf != null ? edf.OVC_PAYMENT_TYPE : ""//35
                };
                //if (!strOVC_EDF_NO.Equals(string.Empty))
                //    query = query.Where(table => table.OVC_EDF_NO.Contains(strOVC_EDF_NO));
                if (!strOVC_START_PORT.Equals(string.Empty))
                    query = query.Where(table => table.OVC_START_PORT == strOVC_START_PORT);
                if (!strOVC_ARRIVE_PORT.Equals(string.Empty))
                    query = query.Where(table => table.OVC_ARRIVE_PORT == strOVC_ARRIVE_PORT);
                if (!strOVC_TRANSER_DEPT_CDE.Contains(string.Empty))
                {
                    var table = GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("TR"))
                        .Where(t => t.OVC_PHR_PARENTS.Equals(strOVC_TRANSER_DEPT_CDE))
                        .Select(t => t.OVC_PHR_ID).ToArray();

                    query = query.Where(t => table.Equals(t.OVC_START_PORT));
                }
                if (!strOVC_SHIP_COMPANY.Equals(string.Empty))
                    query = query.Where(table => table.OVC_SHIP_COMPANY == strOVC_SHIP_COMPANY);
                if (!strOVC_SO_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_SO_NO == strOVC_SO_NO);
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    string strOVC_EDF_NO = dr["OVC_EDF_NO"].ToString();

                    double unit = 0;
                    double weight_unit = 0;
                    BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\mingliu.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
                    Font ChFont = new Font(bfChinese, 10f, Font.BOLD);
                    Font Title_ChFont = new Font(bfChinese, 16f, Font.BOLD);
                    Font font = new Font(bfChinese, 8f, Font.BOLD);
                    MemoryStream Memory = new MemoryStream();
                    var doc1 = new Document(PageSize.A4, 50, 50, 80, 50);
                    PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
                    DateTime PrintTime = DateTime.Now;
                    doc1.Open();
                    PdfPTable Firsttable = new PdfPTable(3);
                    Firsttable.SetWidths(new float[] { 1, 1, 1 });
                    Firsttable.TotalWidth = 550F;
                    Firsttable.LockedWidth = true;
                    Firsttable.DefaultCell.FixedHeight = 40f;
                    Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    PdfPCell Title_Text = new PdfPCell(new Phrase("外運資料編號：" + strOVC_EDF_NO, ChFont));
                    Title_Text.Colspan = 2;
                    Title_Text.FixedHeight = 40f;
                    Title_Text.HorizontalAlignment = Element.ALIGN_LEFT;
                    Title_Text.VerticalAlignment = Element.ALIGN_MIDDLE;
                    Firsttable.AddCell(Title_Text);
                    PdfPCell secTitle_Text = new PdfPCell(new Phrase("S/O NO.: " + dr[4] + "\n國 防 部 國 防\n  採 購 室 軍 品", Title_ChFont));
                    secTitle_Text.Rowspan = 4;
                    secTitle_Text.FixedHeight = 50f;
                    secTitle_Text.HorizontalAlignment = Element.ALIGN_CENTER;
                    secTitle_Text.VerticalAlignment = Element.ALIGN_MIDDLE;
                    secTitle_Text.Border = Rectangle.NO_BORDER;
                    Firsttable.AddCell(secTitle_Text);
                    Firsttable.AddCell(new Phrase("VESSEL : " + dr[5], ChFont));
                    Firsttable.AddCell(new Phrase("VOG NO. : " + dr[6], ChFont));
                    Firsttable.AddCell(new Phrase("PLACE OF RECEIPT: " + dr[34].ToString().Replace("Airport", ""), ChFont));
                    Firsttable.AddCell(new Phrase("PORT OF DISCHARGE:" + dr[33], ChFont));
                    Firsttable.AddCell(new Phrase("PORT OF LOADINGL:" + dr[34].ToString().Replace("Airport", ""), ChFont));
                    Firsttable.AddCell(new Phrase("PLACE OF DISCHARGE: " + dr[33], ChFont));
                    PdfPCell Text1 = new PdfPCell(new Phrase(" ", ChFont));
                    Text1.Colspan = 2;
                    Text1.FixedHeight = 80f;
                    Paragraph Text1_title = new Paragraph("SHIPPER:", ChFont);
                    Paragraph Text1_content = new Paragraph(dr[7].ToString(), ChFont) { FirstLineIndent = 10f };
                    Text1.AddElement(Text1_title);
                    Text1.AddElement(Text1_content);
                    Firsttable.AddCell(Text1);
                    PdfPCell sapce = new PdfPCell(new Phrase(" ", ChFont));
                    sapce.Border = Rectangle.NO_BORDER;
                    Firsttable.AddCell(sapce);

                    PdfPCell Text2 = new PdfPCell(new Phrase(" ", ChFont));
                    Text2.Colspan = 2;
                    Text2.FixedHeight = 140f;
                    Paragraph Text2_title = new Paragraph("CONSIGNEE:", ChFont);
                    Paragraph Text2_content = new Paragraph(dr[8].ToString(), ChFont) { FirstLineIndent = 10f };
                    Paragraph Text2_content2 = new Paragraph("電話 ： " + dr[9].ToString(), ChFont) { FirstLineIndent = 10f };
                    Paragraph Text2_content3 = new Paragraph("傳真 ： " + dr[10].ToString(), ChFont) { FirstLineIndent = 10f };
                    Text2.AddElement(Text2_title);
                    Text2.AddElement(Text2_content);
                    Text2.AddElement(Text2_content2);
                    Text2.AddElement(Text2_content3);
                    Firsttable.AddCell(Text2);
                    string square = ((char)0x25A1).ToString();
                    PdfPCell Text_right = new PdfPCell();
                    Text_right.FixedHeight = 50f;
                    Text_right.VerticalAlignment = Element.ALIGN_MIDDLE;
                    Text_right.Border = Rectangle.NO_BORDER;
                    Text_right.AddElement(new Paragraph(square + " LCL-LCL " + square + " FCL-LCL", ChFont));
                    Text_right.AddElement(new Paragraph(square + " FCL-FCL " + square + " LCL-FCL", ChFont));
                    var check1 = "";
                    var check2 = "";
                    var Marks_text = "";
                    if (dr[35].ToString().Equals("預付"))
                    {
                        check2 = "V";
                        Marks_text = "FREIGHT PREPAID";
                    }
                    else
                    {
                        check1 = "V";
                        Marks_text = "FREIGHT COLLECT";
                    }
                    Text_right.AddElement(new Paragraph("\nFREIGHT（" + check1 + ")COLLECT（" + check2 + ")PREPAID", font));
                    Firsttable.AddCell(Text_right);
                    PdfPCell Text3 = new PdfPCell(new Phrase(" ", ChFont));
                    Text3.Colspan = 2;
                    Text3.FixedHeight = 100f;
                    Paragraph Text3_title = new Paragraph("NOTIFY PARTY :", ChFont);
                    Paragraph Text3_content = new Paragraph("ADDRESS : " + dr[11].ToString(), ChFont) { FirstLineIndent = 10f };
                    Paragraph Text3_content3 = new Paragraph("電話： " + dr[12].ToString(), ChFont) { FirstLineIndent = 10f };
                    Paragraph Text3_content4 = new Paragraph("傳真： " + dr[13].ToString(), ChFont) { FirstLineIndent = 10f };
                    Text3.AddElement(Text3_title);
                    Text3.AddElement(Text3_content);
                    Text3.AddElement(Text3_content3);
                    Text3.AddElement(Text3_content4);
                    Firsttable.AddCell(Text3);
                    PdfPCell Text4 = new PdfPCell(new Phrase(" ", ChFont));
                    Text1.FixedHeight = 50f;
                    Paragraph Text4_title = new Paragraph("ALSO NOTIFY PARTY : ", ChFont);
                    Paragraph Text4_content1 = new Paragraph("ADDRESS: " + dr[14].ToString(), ChFont) { FirstLineIndent = 10f };
                    Paragraph Text4_content2 = new Paragraph("電話： " + dr[15].ToString(), ChFont) { FirstLineIndent = 10f };
                    Paragraph Text4_content3 = new Paragraph("傳真： " + dr[16].ToString(), ChFont) { FirstLineIndent = 10f };
                    Paragraph Text4_content4 = new Paragraph("ALSO NOTIFY PARTY : ", ChFont);
                    Paragraph Text4_content5 = new Paragraph("ADDRESS: " + dr[17].ToString(), ChFont) { FirstLineIndent = 10f };
                    Paragraph Text4_content6 = new Paragraph("電話： " + dr[18].ToString(), ChFont) { FirstLineIndent = 10f };
                    Paragraph Text4_content7 = new Paragraph("傳真： " + dr[19].ToString(), ChFont) { FirstLineIndent = 10f };
                    Paragraph paddingtext = new Paragraph("\n");
                    Text4.AddElement(Text4_title);
                    Text4.AddElement(Text4_content1);
                    Text4.AddElement(Text4_content2);
                    Text4.AddElement(Text4_content3);
                    Text4.AddElement(Text4_content4);
                    Text4.AddElement(Text4_content5);
                    Text4.AddElement(Text4_content6);
                    Text4.AddElement(Text4_content7);
                    Text4.AddElement(paddingtext);
                    Firsttable.AddCell(Text4);

                    doc1.Add(Firsttable);
                    doc1.NewPage();

                    PdfPTable Secondtable = new PdfPTable(5);
                    Secondtable.SetWidths(new float[] { 3, 2, 5, 3, 3 });
                    Secondtable.TotalWidth = 550F;
                    Secondtable.LockedWidth = true;
                    Secondtable.DefaultCell.FixedHeight = 25f;
                    Secondtable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    PdfPCell seccond_title = new PdfPCell(new Phrase("PARTICULARS FURNISHED BY SHIPPER\n\n", ChFont));
                    seccond_title.HorizontalAlignment = Element.ALIGN_CENTER;
                    seccond_title.Colspan = 5;
                    seccond_title.Border = Rectangle.NO_BORDER;
                    Secondtable.AddCell(seccond_title);
                    Secondtable.AddCell(new Phrase("Marks & Numbers", ChFont));
                    Secondtable.AddCell(new Phrase("No. of Pkgs or Containers", font));
                    Secondtable.AddCell(new Phrase(" ", ChFont));
                    Secondtable.AddCell(new Phrase("Gross Weight(kgs)", ChFont));
                    Secondtable.AddCell(new Phrase("Measurement", ChFont));
                    Secondtable.DefaultCell.FixedHeight = 40f;
                    Secondtable.DefaultCell.PaddingTop = 10f;
                    double gross_weight = 0;
                    double measurement = 0;
                    var box_nember = 0;
                    for (var i = (dt.Rows.Count - 1); i >= 0; i--)
                    {
                        if (dt.Rows[i][32].Equals("CF"))
                        {
                            unit = 0.03;
                        }
                        else
                        {
                            unit = 1;
                        }
                        if (dt.Rows[i][30].Equals("磅"))
                        {
                            weight_unit = 0.45359237;
                        }
                        else
                        {
                            weight_unit = 1;
                        }
                        double doubleTemp;
                        if (double.TryParse(dt.Rows[i][29].ToString(), out doubleTemp))
                            gross_weight += doubleTemp * weight_unit;
                        if (double.TryParse(dt.Rows[i][31].ToString(), out doubleTemp))
                            measurement += doubleTemp * unit;
                        //box_nember +=Convert.ToInt16(dt.Rows[i][21]);
                        box_nember++; //改計次 箱號數量
                        Secondtable.AddCell(new Paragraph(dt.Rows[i][20].ToString() + "\n\n" + "SEA WAY B/L:\n\n" + Marks_text, ChFont));
                        Secondtable.AddCell(new Paragraph(dt.Rows[i][21].ToString(), ChFont));
                        PdfPCell seccondtable_text = new PdfPCell();
                        seccondtable_text.HorizontalAlignment = Element.ALIGN_CENTER;
                        Paragraph seccondText_title = new Paragraph(dt.Rows[i][22].ToString() + ". " + dt.Rows[i][23].ToString() + "(" + dt.Rows[i][24].ToString() + ")", ChFont) { FirstLineIndent = 10f };
                        Paragraph seccondText_content = new Paragraph("NSN: " + dt.Rows[i][25].ToString(), ChFont) { FirstLineIndent = 10f };
                        Paragraph seccondText_content2 = new Paragraph("DOC-NO: " + dt.Rows[i][26].ToString(), ChFont) { FirstLineIndent = 10f };
                        Paragraph seccondText_content3 = new Paragraph("P/N: " + dt.Rows[i][27].ToString(), ChFont) { FirstLineIndent = 10f };
                        Paragraph seccondText_content4 = new Paragraph(dt.Rows[i][28].ToString(), ChFont);
                        seccondtable_text.AddElement(seccondText_title);
                        seccondtable_text.AddElement(seccondText_content);
                        seccondtable_text.AddElement(seccondText_content2);
                        seccondtable_text.AddElement(seccondText_content3);
                        seccondtable_text.AddElement(seccondText_content4);
                        seccondtable_text.AddElement(paddingtext);
                        Secondtable.AddCell(seccondtable_text);
                        Secondtable.AddCell(new Paragraph(dt.Rows[i][29].ToString() + " " + dr[30].ToString(), ChFont));
                        Secondtable.AddCell(new Paragraph(dt.Rows[i][31].ToString() + " " + dr[32].ToString(), ChFont));
                    }
                    Secondtable.DefaultCell.PaddingTop = 0f;
                    Secondtable.DefaultCell.FixedHeight = 20f;
                    Secondtable.DefaultCell.PaddingTop = 3f;
                    Secondtable.AddCell(new Phrase(" ", ChFont));                 // total
                    Secondtable.AddCell(new Phrase(box_nember.ToString("#,##0", System.Globalization.CultureInfo.InvariantCulture), ChFont));                 // total
                    Secondtable.AddCell(new Phrase(" ", ChFont));                 // total
                    Secondtable.AddCell(new Phrase(gross_weight.ToString("#,##0.0##", System.Globalization.CultureInfo.InvariantCulture) + " 公斤", ChFont));             // total
                    Secondtable.AddCell(new Phrase(measurement.ToString("#,##0.0##", System.Globalization.CultureInfo.InvariantCulture) + " CBM", ChFont));          // total

                    doc1.Add(Secondtable);
                    doc1.Close();
                    string strFileName = $"裝貨單 - { strOVC_EDF_NO }.pdf";
                    FCommon.DownloadFile(this, strFileName, Memory);
                }
                else
                    FCommon.AlertShow(PnWarning, "danger", "系統訊息", "無此資料！");
            }
            else
                FCommon.AlertShow(PnWarning, "danger", "系統訊息", "編號錯誤！");
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_CHI_NAME);
                    #region 匯入下拉式選單
                    string strFirstText = "不限定";
                    CommonMTS.list_dataImport_TransferArea(drpOVC_TRANSER_DEPT_CDE, true, strFirstText); //接轉地區
                    CommonMTS.list_dataImport_PORT(drpOVC_START_PORT, true, strFirstText); //啟運港(機場)
                    CommonMTS.list_dataImport_SHIP_COMPANY(drpOVC_SHIP_COMPANY, true, strFirstText); //承運航商
                    CommonMTS.list_dataImport_STORED_COMPANY(drpOVC_STORED_COMPANY, true, strFirstText); //進艙廠商
                    #endregion

                    bool boolImport = false;
                    string strOVC_EDF_NO, strOVC_TRANSER_DEPT_CDE, strOVC_START_PORT, strOVC_ARRIVE_PORT, strOVC_SHIP_COMPANY, strOVC_SO_NO, strOVC_STORED_COMPANY;
                    if (FCommon.getQueryString(this, "OVC_EDF_NO", out strOVC_EDF_NO, true))
                    {
                        txtOVC_EDF_NO.Text = strOVC_EDF_NO;
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_TRANSER_DEPT_CDE", out strOVC_TRANSER_DEPT_CDE, true))
                        FCommon.list_setValue(drpOVC_TRANSER_DEPT_CDE, strOVC_TRANSER_DEPT_CDE);
                    if (FCommon.getQueryString(this, "OVC_START_PORT", out strOVC_START_PORT, true))
                        FCommon.list_setValue(drpOVC_START_PORT, strOVC_START_PORT);
                    if (FCommon.getQueryString(this, "OVC_ARRIVE_PORT", out strOVC_ARRIVE_PORT, true))
                    {
                        txtOVC_PORT_CDE.Text = strOVC_ARRIVE_PORT;
                        TBGMT_PORTS tPort = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(strOVC_ARRIVE_PORT)).FirstOrDefault();
                        if (tPort != null) txtOVC_CHI_NAME.Text = tPort.OVC_PORT_CHI_NAME;
                    }
                    if (FCommon.getQueryString(this, "OVC_SHIP_COMPANY", out strOVC_SHIP_COMPANY, true))
                        FCommon.list_setValue(drpOVC_SHIP_COMPANY, strOVC_SHIP_COMPANY);
                    if (FCommon.getQueryString(this, "OVC_SO_NO", out strOVC_SO_NO, true))
                        txtOVC_SO_NO.Text = strOVC_SO_NO;
                    if (FCommon.getQueryString(this, "OVC_STORED_COMPANY", out strOVC_STORED_COMPANY, true))
                        FCommon.list_setValue(drpOVC_STORED_COMPANY, strOVC_STORED_COMPANY);
                    if (boolImport) dataImport();
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }
        protected void btnResetPort_Click(object sender, EventArgs e)
        {
            txtOVC_PORT_CDE.Text = string.Empty;
            txtOVC_CHI_NAME.Text = string.Empty;
        }
        
        protected void GVTBGMT_ESO_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GVTBGMT_ESO.DataKeys[gvrIndex].Value.ToString();
            Guid.TryParse(id, out Guid guidEDF_SN);

            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_EDF_NO", ViewState["OVC_EDF_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", ViewState["OVC_TRANSER_DEPT_CDE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_START_PORT", ViewState["OVC_START_PORT"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_ARRIVE_PORT", ViewState["OVC_ARRIVE_PORT"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_SHIP_COMPANY", ViewState["OVC_SHIP_COMPANY"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_SO_NO", ViewState["OVC_SO_NO"], true);
            FCommon.setQueryString(ref strQueryString, "id", id, true);
            FCommon.setQueryString(ref strQueryString, "OVC_STORED_COMPANY", ViewState["OVC_STORED_COMPANY"], true);

            string[] ar = e.CommandArgument.ToString().Split(',');
            switch (e.CommandName)
            {
                case "btnModify":
                    Response.Redirect($"MTS_A24_2{ strQueryString }");
                    break;
                case "btnDel":
                    Response.Redirect($"MTS_A24_3{ strQueryString }");
                    break;
                case "btnPrintB":
                    PDFBooking_orders(id, ar);
                    break;
                case "btnPrintL":
                    PDFLoading_list(id, ar);
                    break;
            }
        }
        protected void GVTBGMT_ESO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                GridView theGridView = (GridView)sender;
                int index = gvr.RowIndex;
                HyperLink hlkOVC_EDF_NO = (HyperLink)gvr.FindControl("hlkOVC_EDF_NO");
                HyperLink hlkOVC_BLD_NO = (HyperLink)gvr.FindControl("hlkOVC_BLD_NO");
                if (FCommon.Controls_isExist(hlkOVC_EDF_NO, hlkOVC_BLD_NO))
                {
                    string strOVC_EDF_NO = theGridView.DataKeys[index].Value.ToString();
                    hlkOVC_EDF_NO.NavigateUrl = $"javascript: OpenWindow_EDFDATA('{ FCommon.getEncryption(strOVC_EDF_NO) }');";

                    string strOVC_BLD_NO = hlkOVC_BLD_NO.Text;
                    hlkOVC_BLD_NO.NavigateUrl = $"javascript: OpenWindow_BLDDATA('{ FCommon.getEncryption(strOVC_BLD_NO) }');";
                }
            }
        }
        protected void GVTBGMT_ESO_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}