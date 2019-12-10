using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using System.Data.Entity;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace FCFDFE.pages.MTS.A
{
    public class PDF : PdfPageEventHelper
    {

        PdfTemplate template;
        BaseFont bf = null;
        PdfContentByte cb;
        private MTSEntities mtse = new MTSEntities();
        private string strOVC_EDF_NO;
        private string path;

        public PDF(string strOVC_EDF_NO, string path) //傳EDF_NO以及檔案絕對路徑
        {
            this.strOVC_EDF_NO = strOVC_EDF_NO;
            this.path = path;
        }


        /** The header text. */
        public string Header { get; set; }
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            template = writer.DirectContent.CreateTemplate(30, 16);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            var query = mtse.TBGMT_EDF.Where(edf => edf.OVC_EDF_NO == strOVC_EDF_NO).FirstOrDefault();
            string imgpath = path + "images/MTS/stamp.png"; //圖片路徑

            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imgpath);
            img.ScaleToFit(53f, 53f);
            img.SetAbsolutePosition(467f, 25f);
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            Rectangle pageSize = document.PageSize;
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型

            cb = writer.DirectContent;
            cb.BeginText();
            cb.SetFontAndSize(bfChinese, 11);
            //string text = "Page " + writer.CurrentPageNumber + " of ";
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "編號 : 997-3-02-01-01", pageSize.GetLeft(67), pageSize.GetBottom(60), 0);
            System.Globalization.TaiwanCalendar taiwanCalendar = new System.Globalization.TaiwanCalendar();
            //if (query.ODT_CREATE_DATE != null)
            //{
            //    string d1 = query.ODT_CREATE_DATE.ToString();
            //    DateTime create = Convert.ToDateTime(d1);
            //    var datetime = string.Format("中華民國{0}年{1}月{2}日", taiwanCalendar.GetYear(create), create.Month, create.Day);
            //    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, datetime + " 初版 ", pageSize.GetRight(140), pageSize.GetBottom(60), 0);
            //}
            //else
            //{
            //    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "中華民國  年 月 日 初版 ", pageSize.GetRight(140), pageSize.GetBottom(60), 0);
            //}
            //if(query.ODT_MODIFY_DATE != null)
            //{
            //    string d2 = query.ODT_MODIFY_DATE.ToString();
            //    DateTime moidfy = Convert.ToDateTime(d2);
            //    var datetime2 = string.Format("中華民國{0}年{1}月{2}日", taiwanCalendar.GetYear(moidfy), moidfy.Month, moidfy.Day);
            //    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, datetime2 + " 二版 ", pageSize.GetRight(140), pageSize.GetBottom(40), 0);
            //}
            //else
            //{
            //    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "中華民國  年 月 日 二版 ", pageSize.GetRight(140), pageSize.GetBottom(40), 0);
            //}
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "中華民國93年8月31日 初版 ", pageSize.GetRight(140), pageSize.GetBottom(60), 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "中華民國93年11月4日 二版 ", pageSize.GetRight(140), pageSize.GetBottom(40), 0);
            cb.AddImage(img);


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
    public partial class MTS_A22_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        public string strDEPT_SN, strDEPT_Name;
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        Common FCommon = new Common();

        #region 副程式
        public void dataImport()
        {
            string strMessage = "";
            string strOVC_REQ_DEPT_CDE = txtOVC_DEPT_CDE.Value;
            string strOVC_EDF_NO = txtOVC_EDF_NO.Text;
            string strOVC_START_PORT = drpOVC_START_PORT.SelectedValue;
            //string strOVC_ARRIVE_PORT = drpOVC_ARRIVE_PORT.SelectedValue;
            string strOVC_ARRIVE_PORT = txtOVC_PORT_CDE.Text;
            string strOVC_TRANSER_DEPT_CDE = drpOVC_TRANSER_DEPT_CDE.SelectedValue;
            string strOVC_IS_STRATEGY = drpOVC_IS_STRATEGY.SelectedValue;
            string strOVC_REVIEW_STATUS = drpOVC_REVIEW_STATUS.SelectedValue;
            string strODT_RECEIVE_DATE = txtODT_RECEIVE_DATE.Text;
            string strOVC_PURCH_NO = txtOVC_PURCH_NO.Text;
            string strOVC_ITEM_NO2 = txtOVC_ITEM_NO2.Text;

            ViewState["OVC_REQ_DEPT_CDE"] = strOVC_REQ_DEPT_CDE;
            ViewState["OVC_EDF_NO"] = strOVC_EDF_NO;
            ViewState["OVC_START_PORT"] = strOVC_START_PORT;
            ViewState["OVC_ARRIVE_PORT"] = strOVC_ARRIVE_PORT;
            ViewState["OVC_TRANSER_DEPT_CDE"] = strOVC_TRANSER_DEPT_CDE;
            ViewState["OVC_IS_STRATEGY"] = strOVC_IS_STRATEGY;
            ViewState["OVC_REVIEW_STATUS"] = strOVC_REVIEW_STATUS;
            ViewState["ODT_RECEIVE_DATE"] = strODT_RECEIVE_DATE;
            ViewState["OVC_PURCH_NO"] = strOVC_PURCH_NO;
            ViewState["OVC_ITEM_NO2"] = strOVC_ITEM_NO2;

            if (strOVC_REQ_DEPT_CDE.Equals(string.Empty) && strOVC_EDF_NO.Equals(string.Empty) && strOVC_START_PORT.Equals(string.Empty)
                && strOVC_ARRIVE_PORT.Equals(string.Empty) && strOVC_TRANSER_DEPT_CDE.Equals(string.Empty) && strOVC_IS_STRATEGY.Equals(string.Empty)
                && strOVC_REVIEW_STATUS.Equals(string.Empty) && strODT_RECEIVE_DATE.Equals(string.Empty) && strOVC_PURCH_NO.Equals(string.Empty) 
                && strOVC_ITEM_NO2.Equals(string.Empty))
                strMessage += "<P> 至少填入一個選項 </p>";

            if (strMessage.Equals(string.Empty))
            {
                var query =
                    from edf in MTSE.TBGMT_EDF.AsEnumerable()
                    join edf_detail in MTSE.TBGMT_EDF_DETAIL.AsEnumerable() on edf.OVC_EDF_NO equals edf_detail.OVC_EDF_NO into tempEDF
                    from edf_detail in tempEDF.DefaultIfEmpty()
                    join portStart in MTSE.TBGMT_PORTS on edf.OVC_START_PORT equals portStart.OVC_PORT_CDE into tempPS
                    from portStart in tempPS.DefaultIfEmpty()
                    join portArrive in MTSE.TBGMT_PORTS on edf.OVC_ARRIVE_PORT equals portArrive.OVC_PORT_CDE into tempPA
                    from portArrive in tempPA.DefaultIfEmpty()
                    join dept in MTSE.TBMDEPTs on edf.OVC_REQ_DEPT_CDE equals dept.OVC_DEPT_CDE into tempD
                    from dept in tempD.DefaultIfEmpty()
                    orderby edf.OVC_EDF_NO
                    select new
                    {
                        edf.EDF_SN,
                        OVC_EDF_NO = edf.OVC_EDF_NO,
                        OVC_PURCH_NO = edf.OVC_PURCH_NO ?? "",
                        OVC_START_PORT = portStart != null ? portStart.OVC_PORT_CHI_NAME : "",
                        OVC_ARRIVE_PORT = portArrive != null ? portArrive.OVC_PORT_CHI_NAME : "",
                        OVC_START_PORT_Value = edf.OVC_START_PORT ?? "",
                        OVC_ARRIVE_PORT_Value = edf.OVC_ARRIVE_PORT ?? "",
                        OVC_DEPT_CDE = edf.OVC_DEPT_CDE,
                        OVC_REQ_DEPT_CDE = dept != null ? dept.OVC_ONNAME : "",
                        OVC_REQ_DEPT_CDE_Value = edf.OVC_REQ_DEPT_CDE ?? "",
                        OVC_CREATE_LOGIN_ID = edf.OVC_CREATE_LOGIN_ID,
                        ODT_RECEIVE_DATE = FCommon.getDateTime(edf.ODT_RECEIVE_DATE),
                        OVC_IS_STRATEGY = edf.OVC_IS_STRATEGY ?? "",
                        OVC_REVIEW_STATUS = edf.OVC_REVIEW_STATUS ?? "",
                        OVC_ITEM_NO2 = edf_detail != null ? edf_detail.OVC_ITEM_NO2 ?? "" : ""
                    };
                if (!strOVC_REQ_DEPT_CDE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_REQ_DEPT_CDE_Value.Equals(strOVC_REQ_DEPT_CDE));
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
                if (!strOVC_IS_STRATEGY.Equals(string.Empty))
                    query = query.Where(table => table.OVC_IS_STRATEGY.Equals(strOVC_IS_STRATEGY));
                if (!strOVC_REVIEW_STATUS.Equals(string.Empty))
                {
                    if (strOVC_REVIEW_STATUS.Equals("未審核"))
                        query = query.Where(table => table.OVC_REVIEW_STATUS.Equals(string.Empty));
                    else
                        query = query.Where(table => table.OVC_REVIEW_STATUS.Equals(strOVC_REVIEW_STATUS));
                }
                if (!strODT_RECEIVE_DATE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_EDF_NO.Substring(3, 3).Equals(strODT_RECEIVE_DATE.PadLeft(3, '0')));
                if (!strOVC_PURCH_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_PURCH_NO.Contains(strOVC_PURCH_NO));
                if (!strOVC_ITEM_NO2.Equals(string.Empty))
                    query = query.Where(table => table.OVC_ITEM_NO2.Contains(strOVC_ITEM_NO2));
                ////一般使用者可查詢該單位的外運資料，但國防部採購室承辦人不在此限
                //if (strDEPT_Name.IndexOf("國防採購室") == -1)
                //    query = query.Where(table => table.OVC_REQ_DEPT_CDE_Value.Equals(strOVC_REQ_DEPT_CDE));
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);

                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_EDF, dt);
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }

        private void print(string strOVC_EDF_NO)
        {
            string path = Request.PhysicalApplicationPath;//取得檔案絕對路徑
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChTitle = new Font(bfChinese, 16, Font.BOLD);
            Font ChTabTitle = new Font(bfChinese, 12f);
            Font ChFont = new Font(bfChinese, 11f);
            Font smaillChFont = new Font(bfChinese, 10f);
            var doc1 = new Document(PageSize.A4, 60, 60, 50, 80);
            MemoryStream Memory = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            writer.PageEvent = new PDF(strOVC_EDF_NO, path);
            doc1.Open();
            DateTime PrintTime = DateTime.Now;

            #region 外運表資料

            var query = MTSE.TBGMT_EDF.Where(edf => edf.OVC_EDF_NO == strOVC_EDF_NO).FirstOrDefault();
            string strOVC_PURCH_NO = query.OVC_PURCH_NO;
            string strOVC_START_PORT_CHNAME = "";
            string strOVC_START_PORT_ENNAME = "";
            string strOVC_ARRIVE_PORT_CHNAME = "";
            string strOVC_ARRIVE_PORT_ENNAME = "";
            if (query.OVC_START_PORT != null)
            {
                string strOVC_START_PORT_CDE = query.OVC_START_PORT;
                var querystart = MTSE.TBGMT_PORTS.Where(cde => cde.OVC_PORT_CDE == strOVC_START_PORT_CDE).FirstOrDefault();
                strOVC_START_PORT_CHNAME = querystart.OVC_PORT_CHI_NAME;
                strOVC_START_PORT_ENNAME = "(" + querystart.OVC_PORT_ENG_NAME + ")";
            }
            if (query.OVC_ARRIVE_PORT != null)
            {
                string strOVC_ARRIVE_PORT_CDE = query.OVC_ARRIVE_PORT;
                var queryarrive = MTSE.TBGMT_PORTS.Where(cde => cde.OVC_PORT_CDE == strOVC_ARRIVE_PORT_CDE).FirstOrDefault();
                strOVC_ARRIVE_PORT_CHNAME = queryarrive.OVC_PORT_CHI_NAME;
                strOVC_ARRIVE_PORT_ENNAME = "(" + queryarrive.OVC_PORT_ENG_NAME + ")";
            }
            string strOVC_DEPT_CDE = query.OVC_REQ_DEPT_CDE;
            var querydept = MTSE.TBGMT_DEPT_CDE.Where(cde => cde.OVC_DEPT_CODE == strOVC_DEPT_CDE).FirstOrDefault();
            string strOVC_ONNAME = "";
            if (querydept != null)
            {
                strOVC_ONNAME = querydept.OVC_DEPT_NAME;
            }
            string strOVC_SHIP_FROM = query.OVC_SHIP_FROM;
            string strOVC_DELIVER_NAME = query.OVC_DELIVER_NAME;
            string strOVC_DELIVER_MILITARY_LINE = query.OVC_DELIVER_MILITARY_LINE;
            string strOVC_DELIVER_MOBILE = query.OVC_DELIVER_MOBILE;
            int address_number = 1;
            string strOVC_CON_ENG_ADDRESS = query.OVC_CON_ENG_ADDRESS;
            string strOVC_CON_TEL = query.OVC_CON_TEL;
            string strOVC_CON_FAX = query.OVC_CON_FAX;
            if (query.OVC_NP_ENG_ADDRESS != null)
                address_number = 2;
            if (query.OVC_ANP_ENG_ADDRESS != null)
                address_number = 3;
            if (query.OVC_ANP_ENG_ADDRESS2 != null)
                address_number = 3;
            string strOVC_PAYMENT_TYPE = query.OVC_PAYMENT_TYPE;
            string strOVC_PAYMENT = "";
            string strOVC_PAYMENT_DETAIL = "";
            if (strOVC_PAYMENT_TYPE == "預付")
            {
                strOVC_PAYMENT = "PREPAID 預付 (軍種年度運保費項下支付)";
            }
            if (strOVC_PAYMENT_TYPE == "契約航商" || strOVC_PAYMENT_TYPE == "代理商" || strOVC_PAYMENT_TYPE == "快遞")
            {
                strOVC_PAYMENT = "COLLECT 到付 (收貨人支付)";
                strOVC_PAYMENT_DETAIL = strOVC_PAYMENT_TYPE;
            }
            string strOVC_NOTE = query.OVC_NOTE;

            #endregion

            string datatime = FCommon.getTaiwanDate(DateTime.Now, "{0}/{1}/{2}");
            #region firsttable
            PdfPTable firsttable = new PdfPTable(2);
            firsttable.SetWidths(new float[] { 2, 5 });
            //firsttable.DefaultCell.Border = Rectangle.NO_BORDER;
            firsttable.TotalWidth = 470F;
            firsttable.LockedWidth = true;
            firsttable.DefaultCell.SetLeading(1f, 1f);//行距
            firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            firsttable.DefaultCell.PaddingLeft = 5;
            PdfPCell title = new PdfPCell(new Phrase("國防部" + strOVC_ONNAME + "  外運資料表", ChTitle));
            title.Colspan = 2;
            title.HorizontalAlignment = Element.ALIGN_CENTER;
            title.VerticalAlignment = Element.ALIGN_MIDDLE;
            title.Border = Rectangle.NO_BORDER;
            firsttable.AddCell(title);
            PdfPCell title2 = new PdfPCell(new Phrase("LIST OF MILITARY MATERIAL EXPORTATION" + "  資料時間" + datatime, ChTitle));
            title2.Colspan = 2;
            title2.HorizontalAlignment = Element.ALIGN_CENTER;
            title2.VerticalAlignment = Element.ALIGN_MIDDLE;
            title2.Border = Rectangle.NO_BORDER;
            firsttable.AddCell(title2);
            PdfPCell title3 = new PdfPCell(new Phrase(" \n ", smaillChFont));
            title3.Colspan = 2;
            title3.Border = Rectangle.NO_BORDER;
            firsttable.AddCell(title3);
            PdfPCell tab1 = new PdfPCell(new Phrase("表單編號 \n FORM NO.", ChTabTitle));
            tab1.HorizontalAlignment = Element.ALIGN_CENTER;
            tab1.VerticalAlignment = Element.ALIGN_MIDDLE;
            tab1.PaddingBottom = 5;
            firsttable.AddCell(tab1);
            PdfPCell tabr1 = new PdfPCell(new Phrase(strOVC_EDF_NO, ChFont));
            tabr1.VerticalAlignment = Element.ALIGN_MIDDLE;
            tabr1.PaddingLeft = 5;
            firsttable.AddCell(tabr1);

            PdfPCell tab2 = new PdfPCell(new Phrase("案號 \n CASE NO.", ChTabTitle));
            tab2.HorizontalAlignment = Element.ALIGN_CENTER;
            tab2.PaddingBottom = 5;
            firsttable.AddCell(tab2);
            PdfPCell tabr2 = new PdfPCell(new Phrase(strOVC_PURCH_NO, ChFont));
            tabr2.VerticalAlignment = Element.ALIGN_MIDDLE;
            tabr2.PaddingLeft = 5;
            firsttable.AddCell(tabr2);

            PdfPCell tab3 = new PdfPCell(new Phrase("啓運港(機場) \n PORT OF EXIT", ChTabTitle));
            tab3.HorizontalAlignment = Element.ALIGN_CENTER;
            tab3.PaddingBottom = 5;
            firsttable.AddCell(tab3);
            PdfPCell tabr3 = new PdfPCell(new Phrase(strOVC_START_PORT_CHNAME + "  " + strOVC_START_PORT_ENNAME, ChFont));
            tabr3.VerticalAlignment = Element.ALIGN_MIDDLE;
            tabr3.PaddingLeft = 5;
            firsttable.AddCell(tabr3);

            PdfPCell tab4 = new PdfPCell(new Phrase("目的港(機場) \n PORT OF DESTINATION", ChTabTitle));
            tab4.HorizontalAlignment = Element.ALIGN_CENTER;
            tab4.PaddingBottom = 5;
            firsttable.AddCell(tab4);
            PdfPCell tabr4 = new PdfPCell(new Phrase(strOVC_ARRIVE_PORT_CHNAME + "  " + strOVC_ARRIVE_PORT_ENNAME, ChFont));
            tabr4.VerticalAlignment = Element.ALIGN_MIDDLE;
            tabr4.PaddingLeft = 5;
            firsttable.AddCell(tabr4);

            PdfPCell tab5 = new PdfPCell(new Phrase("發貨單位 \n SHIP FROM", ChTabTitle));
            tab5.HorizontalAlignment = Element.ALIGN_CENTER;
            tab5.VerticalAlignment = Element.ALIGN_MIDDLE;
            firsttable.AddCell(tab5);
            firsttable.AddCell(new Phrase(strOVC_SHIP_FROM + "\n" + "發貨人: \n" + "名字 :" + strOVC_DELIVER_NAME + "\n" + "軍線 : " + strOVC_DELIVER_MILITARY_LINE + "                    手機 : " + strOVC_DELIVER_MOBILE + " \n \n", ChFont));

            PdfPCell tab6 = new PdfPCell(new Phrase("收貨單位地址、電話、傳真 \n CONSIGNEE ADDRESS,TEL,FAX", ChTabTitle));
            tab6.Rowspan = address_number;
            tab6.HorizontalAlignment = Element.ALIGN_CENTER;
            tab6.VerticalAlignment = Element.ALIGN_MIDDLE;
            firsttable.AddCell(tab6);
            firsttable.AddCell(new Phrase("● CONSIGNEE :\n" + "ADDRESS : " + strOVC_CON_ENG_ADDRESS + "\n" + "TEL : " + strOVC_CON_TEL + "        FAX : " + strOVC_CON_FAX + " \n\n", ChFont));
            if (address_number == 2)
            {
                string strOVC_NP_ENG_ADDRESS = query.OVC_NP_ENG_ADDRESS;
                string strOVC_NP_TEL = query.OVC_NP_TEL;
                string strOVC_NP_FAX = query.OVC_NP_FAX;
                firsttable.AddCell(new Phrase("● NOTIFY PARTY :\n" + "ADDRESS : " + strOVC_NP_ENG_ADDRESS + "\n" + "TEL : " + strOVC_NP_TEL + "        FAX : " + strOVC_NP_FAX + " \n\n", ChFont));
            }
            if (address_number == 3)
            {
                string strOVC_NP_ENG_ADDRESS = query.OVC_NP_ENG_ADDRESS;
                string strOVC_NP_TEL = query.OVC_NP_TEL;
                string strOVC_NP_FAX = query.OVC_NP_FAX;
                firsttable.AddCell(new Phrase("● NOTIFY PARTY :\n" + "ADDRESS : " + strOVC_NP_ENG_ADDRESS + "\n" + "TEL : " + strOVC_NP_TEL + "        FAX : " + strOVC_NP_FAX + " \n\n", ChFont));
                string strOVC_ANP_ENG_ADDRESS = query.OVC_ANP_ENG_ADDRESS;
                string strOVC_ANP_TEL = query.OVC_ANP_TEL;
                string strOVC_ANP_FAX = query.OVC_ANP_FAX;
                firsttable.AddCell(new Phrase("● ALSO NOTIFY PARTY 1 :\n" + "ADDRESS : " + strOVC_ANP_ENG_ADDRESS + "\n" + "TEL : " + strOVC_ANP_TEL + "        FAX : " + strOVC_ANP_FAX + " \n\n", ChFont));
            }
            if (address_number == 4)
            {
                string strOVC_NP_ENG_ADDRESS = query.OVC_NP_ENG_ADDRESS;
                string strOVC_NP_TEL = query.OVC_NP_TEL;
                string strOVC_NP_FAX = query.OVC_NP_FAX;
                firsttable.AddCell(new Phrase("● NOTIFY PARTY :\n" + "ADDRESS : " + strOVC_NP_ENG_ADDRESS + "\n" + "TEL : " + strOVC_NP_TEL + "        FAX : " + strOVC_NP_FAX + " \n\n", ChFont));
                string strOVC_ANP_ENG_ADDRESS = query.OVC_ANP_ENG_ADDRESS;
                string strOVC_ANP_TEL = query.OVC_ANP_TEL;
                string strOVC_ANP_FAX = query.OVC_ANP_FAX;
                firsttable.AddCell(new Phrase("● ALSO NOTIFY PARTY 1 :\n" + "ADDRESS : " + strOVC_ANP_ENG_ADDRESS + "\n" + "TEL : " + strOVC_ANP_TEL + "        FAX : " + strOVC_ANP_FAX + " \n\n", ChFont));
                string strOVC_ANP_ENG_ADDRESS2 = query.OVC_ANP_ENG_ADDRESS2;
                string strOVC_ANP_TEL2 = query.OVC_ANP_TEL2;
                string strOVC_ANP_FAX2 = query.OVC_ANP_FAX2;
                firsttable.AddCell(new Phrase("● ALSO NOTIFY PARTY 2 :\n" + "ADDRESS : " + strOVC_ANP_ENG_ADDRESS2 + "\n" + "TEL : " + strOVC_ANP_TEL2 + "        FAX : " + strOVC_ANP_FAX2 + " \n\n", ChFont));
            }
            PdfPCell tab7 = new PdfPCell(new Phrase("付款方式 \n PAYMENT", ChTabTitle));
            tab7.HorizontalAlignment = Element.ALIGN_CENTER;
            tab7.PaddingBottom = 5;
            firsttable.AddCell(tab7);
            PdfPCell tabr7 = new PdfPCell(new Phrase(strOVC_PAYMENT + "  " + strOVC_PAYMENT_DETAIL, ChFont));
            tabr7.VerticalAlignment = Element.ALIGN_MIDDLE;
            tabr7.PaddingLeft = 5;
            firsttable.AddCell(tabr7);

            PdfPCell tab8 = new PdfPCell(new Phrase("備考 \n REMARK", ChTabTitle));
            tab8.HorizontalAlignment = Element.ALIGN_CENTER;
            tab8.PaddingBottom = 5;
            firsttable.AddCell(tab8);
            PdfPCell tabr8 = new PdfPCell(new Phrase(strOVC_NOTE, ChFont));
            tabr8.VerticalAlignment = Element.ALIGN_MIDDLE;
            tabr8.PaddingLeft = 5;
            firsttable.AddCell(tabr8);

            doc1.Add(firsttable);
            #endregion
            Paragraph middle = new Paragraph();
            middle.Add("\n");
            doc1.Add(middle);

            #region second
            PdfPTable secondtable = new PdfPTable(6);
            secondtable.SetWidths(new float[] { 4, 1, 2, 2, 2, 2 });
            secondtable.TotalWidth = 470F;
            secondtable.LockedWidth = true;
            secondtable.DefaultCell.SetLeading(1f, 1f);//行距

            PdfPCell tit1 = new PdfPCell(new Phrase("品名(料、單、件號)\n" + "ITEM NOMENCLATURE \n" + "(NSN, DOC-NO, P/N)", ChTabTitle));
            tit1.VerticalAlignment = Element.ALIGN_MIDDLE;
            tit1.HorizontalAlignment = Element.ALIGN_CENTER;
            tit1.PaddingBottom = 5;
            secondtable.AddCell(tit1);

            PdfPCell tit2 = new PdfPCell(new Phrase("數量\n" + "EA", ChTabTitle));
            tit2.VerticalAlignment = Element.ALIGN_MIDDLE;
            tit2.HorizontalAlignment = Element.ALIGN_CENTER;
            tit2.PaddingBottom = 5;
            secondtable.AddCell(tit2);

            PdfPCell tit3 = new PdfPCell(new Phrase("箱號\n" + "PKG", ChTabTitle));
            tit3.VerticalAlignment = Element.ALIGN_MIDDLE;
            tit3.HorizontalAlignment = Element.ALIGN_CENTER;
            tit3.PaddingBottom = 5;
            secondtable.AddCell(tit3);

            PdfPCell tit4 = new PdfPCell(new Phrase("重量\n" + "WT", ChTabTitle));
            tit4.VerticalAlignment = Element.ALIGN_MIDDLE;
            tit4.HorizontalAlignment = Element.ALIGN_CENTER;
            tit4.PaddingBottom = 5;
            secondtable.AddCell(tit4);

            PdfPCell tit5 = new PdfPCell(new Phrase("容積\n" + "CUB FT", ChTabTitle));
            tit5.VerticalAlignment = Element.ALIGN_MIDDLE;
            tit5.HorizontalAlignment = Element.ALIGN_CENTER;
            tit5.PaddingBottom = 5;
            secondtable.AddCell(tit5);

            PdfPCell tit6 = new PdfPCell(new Phrase("金額\n" + "VALUE", ChTabTitle));
            tit6.VerticalAlignment = Element.ALIGN_MIDDLE;
            tit6.HorizontalAlignment = Element.ALIGN_CENTER;
            tit6.PaddingBottom = 5;
            secondtable.AddCell(tit6);



            decimal weight = 0;
            decimal volume = 0;
            decimal money = 0;
            int num = 0;
            var querydetail = MTSE.TBGMT_EDF_DETAIL.Where(no => no.OVC_EDF_NO == strOVC_EDF_NO);
            var querydetail2 = MTSE.TBGMT_EDF_DETAIL.Where(no => no.OVC_EDF_NO == strOVC_EDF_NO).FirstOrDefault();
            int i = 1;
            foreach (var d in querydetail)
            {
                string strItem = i.ToString() + ". " + d.OVC_CHI_NAME + "\n" + d.OVC_ENG_NAME + "\n" + "MSN : " + d.OVC_ITEM_NO + "\n" + "DOC-NO : " + d.OVC_ITEM_NO2 + "\n" + "P/N : " + d.OVC_ITEM_NO3;
                int onbnum = Convert.ToInt32(d.ONB_ITEM_COUNT);
                decimal onbweight = Convert.ToDecimal(d.ONB_WEIGHT);
                decimal onbvolume = Convert.ToDecimal(d.ONB_VOLUME);
                decimal onbmoney = Convert.ToDecimal(d.ONB_MONEY);

                PdfPCell t1 = new PdfPCell(new Phrase(strItem, ChFont));
                t1.BorderWidthTop = 0;
                t1.BorderWidthBottom = 0;
                t1.PaddingLeft = 5;
                secondtable.AddCell(t1);

                PdfPCell t2 = new PdfPCell(new Phrase(String.Format("{0:N0}", d.ONB_ITEM_COUNT.ToString()), ChFont));
                t2.BorderWidthTop = 0;
                t2.BorderWidthBottom = 0;
                t2.PaddingLeft = 5;
                t2.HorizontalAlignment = Element.ALIGN_CENTER;
                secondtable.AddCell(t2);

                PdfPCell t3 = new PdfPCell(new Phrase(d.OVC_BOX_NO, ChFont));
                t3.BorderWidthTop = 0;
                t3.BorderWidthBottom = 0;
                t3.PaddingLeft = 5;
                t3.HorizontalAlignment = Element.ALIGN_CENTER;
                secondtable.AddCell(t3);

                PdfPCell t4 = new PdfPCell(new Phrase(String.Format("{0:N02}", d.ONB_WEIGHT) + "\n" + d.OVC_WEIGHT_UNIT, ChFont));
                t4.BorderWidthTop = 0;
                t4.BorderWidthBottom = 0;
                t4.PaddingLeft = 5;
                t4.HorizontalAlignment = Element.ALIGN_RIGHT;
                secondtable.AddCell(t4);

                PdfPCell t5 = new PdfPCell(new Phrase(String.Format("{0:N04}", d.ONB_VOLUME) + "\n" + d.OVC_VOLUME_UNIT, ChFont));
                t5.BorderWidthTop = 0;
                t5.BorderWidthBottom = 0;
                t5.PaddingLeft = 5;
                t5.HorizontalAlignment = Element.ALIGN_RIGHT;
                secondtable.AddCell(t5);

                PdfPCell t6 = new PdfPCell(new Phrase(String.Format("{0:N02}", d.ONB_MONEY) + "\n" + d.OVC_CURRENCY, ChFont));
                t6.BorderWidthTop = 0;
                t6.BorderWidthBottom = 0;
                t6.PaddingLeft = 5;
                t6.HorizontalAlignment = Element.ALIGN_RIGHT;
                secondtable.AddCell(t6);

                i++;
                num = num + onbnum;
                weight = weight + onbweight;
                volume = volume + onbvolume;
                money = money + onbmoney;
            }

            var querybox = (from p in MTSE.TBGMT_EDF_DETAIL
                            where p.OVC_EDF_NO == strOVC_EDF_NO
                            select p.OVC_BOX_NO).Distinct();
            DataTable dt = new DataTable();
            dt.Columns.Add("box");
            foreach (var item in querybox)
            {
                DataRow dr = dt.NewRow();
                dr["box"] = item.ToString();
                dt.Rows.Add(dr);
            }
            int boxnum = dt.Rows.Count;

            PdfPCell footer1 = new PdfPCell(new Phrase("合計\n" + "TOTAL", ChTabTitle));
            footer1.VerticalAlignment = Element.ALIGN_MIDDLE;
            footer1.HorizontalAlignment = Element.ALIGN_CENTER;
            footer1.PaddingBottom = 5;
            secondtable.AddCell(footer1);

            PdfPCell footer2 = new PdfPCell(new Phrase(String.Format("{0:N0}", num.ToString()), ChTabTitle));
            footer2.VerticalAlignment = Element.ALIGN_MIDDLE;
            footer2.HorizontalAlignment = Element.ALIGN_CENTER;
            footer2.PaddingBottom = 5;
            secondtable.AddCell(footer2);

            PdfPCell footer3 = new PdfPCell(new Phrase(boxnum.ToString(), ChTabTitle));
            footer3.VerticalAlignment = Element.ALIGN_MIDDLE;
            footer3.HorizontalAlignment = Element.ALIGN_CENTER;
            footer3.PaddingBottom = 5;
            secondtable.AddCell(footer3);

            string strOVC_WEIGHT_UNIT = querydetail2 != null ? querydetail2.OVC_WEIGHT_UNIT ?? "" : "";
            PdfPCell footer4 = new PdfPCell(new Phrase(String.Format("{0:N02}", weight) + "\n" + strOVC_WEIGHT_UNIT, ChTabTitle));
            footer4.VerticalAlignment = Element.ALIGN_MIDDLE;
            footer4.HorizontalAlignment = Element.ALIGN_RIGHT;
            footer4.PaddingBottom = 5;
            secondtable.AddCell(footer4);

            string strOVC_VOLUME_UNIT = querydetail2 != null ? querydetail2.OVC_VOLUME_UNIT ?? "" : "";
            PdfPCell footer5 = new PdfPCell(new Phrase(String.Format("{0:N04}", volume) + "\n" + strOVC_VOLUME_UNIT, ChTabTitle));
            footer5.VerticalAlignment = Element.ALIGN_MIDDLE;
            footer5.HorizontalAlignment = Element.ALIGN_RIGHT;
            footer5.PaddingBottom = 5;
            secondtable.AddCell(footer5);

            string strOVC_CURRENCY = querydetail2 != null ? querydetail2.OVC_CURRENCY ?? "" : "";
            PdfPCell footer6 = new PdfPCell(new Phrase(String.Format("{0:N02}", money) + "\n" + strOVC_CURRENCY, ChTabTitle));
            footer6.VerticalAlignment = Element.ALIGN_MIDDLE;
            footer6.HorizontalAlignment = Element.ALIGN_RIGHT;
            footer6.PaddingBottom = 5;
            secondtable.AddCell(footer6);

            doc1.Add(secondtable);
            #endregion
            Phrase simplePhr1 = new Phrase(" 主管 :                              製表人 :", ChTabTitle);

            Paragraph fotter = new Paragraph();
            fotter.Add(simplePhr1);
            doc1.Add(fotter);

            doc1.Close();
            string strFileName = $"外運資料表 - { strOVC_EDF_NO }.pdf";
            FCommon.DownloadFile(this, strFileName, Memory);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getAccountDEPTName(this, out strDEPT_SN, out strDEPT_Name);
                if (!IsPostBack)
                {
                    //設置readonly屬性
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_ONNAME, txtOVC_CHI_NAME);
                    //申請單位
                    //唯有國防部採購室承辦人可使用審核和單位查詢
                    bool isCanVerify = strDEPT_Name.Contains("國防採購室");
                    GVTBGMT_EDF.Columns[13].Visible = isCanVerify; //審核
                    btnQueryOVC_REQ_DEPT_CDE.Visible = isCanVerify;
                    btnReset.Visible = isCanVerify;
                    txtOVC_ONNAME.Text = isCanVerify ? "" : strDEPT_Name;
                    txtOVC_DEPT_CDE.Value = isCanVerify ? "" : strDEPT_SN;

                    //申請年度(民國年)
                    txtODT_RECEIVE_DATE.Text = FCommon.getTaiwanYear(DateTime.Now).ToString().PadLeft(3, '0');

                    #region 匯入下拉式選單
                    string strFirstText = "不限定";
                    //啟運港(機場)
                    CommonMTS.list_dataImport_PORT(drpOVC_START_PORT, true, strFirstText);
                    CommonMTS.list_dataImport_TransferArea(drpOVC_TRANSER_DEPT_CDE, true, strFirstText); //接轉地區
                    CommonMTS.list_dataImport_IS_STRATEGY(drpOVC_IS_STRATEGY, true, strFirstText); //戰略性高科技貨品
                    string[] strREVIEW_STATUS_List = { "未審核", "通過", "剔退" };
                    FCommon.list_dataImport(drpOVC_REVIEW_STATUS, strREVIEW_STATUS_List, strREVIEW_STATUS_List, strFirstText, "", true); //審核狀況
                    #endregion

                    bool boolImport = false;
                    string strOVC_REQ_DEPT_CDE, strOVC_EDF_NO, strOVC_START_PORT, strOVC_ARRIVE_PORT, strOVC_TRANSER_DEPT_CDE, strOVC_IS_STRATEGY
                        , strOVC_REVIEW_STATUS, strODT_RECEIVE_DATE, strOVC_PURCH_NO, strOVC_ITEM_NO2;
                    if (FCommon.getQueryString(this, "OVC_REQ_DEPT_CDE", out strOVC_REQ_DEPT_CDE, true))
                    {
                        txtOVC_DEPT_CDE.Value = strOVC_REQ_DEPT_CDE;
                        TBMDEPT dept = GME.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(strOVC_REQ_DEPT_CDE)).FirstOrDefault();
                        if (dept != null) txtOVC_ONNAME.Text = dept.OVC_ONNAME;
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_EDF_NO", out strOVC_EDF_NO, true))
                    {
                        txtOVC_EDF_NO.Text = strOVC_EDF_NO;
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_START_PORT", out strOVC_START_PORT, true))
                        FCommon.list_setValue(drpOVC_START_PORT, strOVC_START_PORT);
                    if (FCommon.getQueryString(this, "OVC_ARRIVE_PORT", out strOVC_ARRIVE_PORT, true))
                    {
                        txtOVC_PORT_CDE.Text = strOVC_ARRIVE_PORT;
                        TBGMT_PORTS tPort = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(strOVC_ARRIVE_PORT)).FirstOrDefault();
                        if (tPort != null) txtOVC_CHI_NAME.Text = tPort.OVC_PORT_CHI_NAME;
                    }
                        //FCommon.list_setValue(drpOVC_ARRIVE_PORT, strOVC_ARRIVE_PORT);
                    if (FCommon.getQueryString(this, "OVC_TRANSER_DEPT_CDE", out strOVC_TRANSER_DEPT_CDE, true))
                        FCommon.list_setValue(drpOVC_TRANSER_DEPT_CDE, strOVC_TRANSER_DEPT_CDE);
                    if (FCommon.getQueryString(this, "OVC_IS_STRATEGY", out strOVC_IS_STRATEGY, true))
                        FCommon.list_setValue(drpOVC_IS_STRATEGY, strOVC_IS_STRATEGY);
                    if (FCommon.getQueryString(this, "OVC_REVIEW_STATUS", out strOVC_REVIEW_STATUS, true))
                        FCommon.list_setValue(drpOVC_REVIEW_STATUS, strOVC_REVIEW_STATUS);
                    if (FCommon.getQueryString(this, "ODT_RECEIVE_DATE", out strODT_RECEIVE_DATE, true))
                        txtODT_RECEIVE_DATE.Text = strODT_RECEIVE_DATE;
                    if (FCommon.getQueryString(this, "OVC_PURCH_NO", out strOVC_PURCH_NO, true))
                        txtOVC_PURCH_NO.Text = strOVC_PURCH_NO;
                    if (FCommon.getQueryString(this, "OVC_ITEM_NO2", out strOVC_ITEM_NO2, true))
                        txtOVC_ITEM_NO2.Text = strOVC_ITEM_NO2;
                    if (boolImport)
                        dataImport();
                }
            }
        }

        #region btn_Click
        //查詢
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtOVC_ONNAME.Text = string.Empty;
            txtOVC_DEPT_CDE.Value = string.Empty;
        }
        protected void btnResetPort_Click(object sender, EventArgs e)
        {
            txtOVC_PORT_CDE.Text = string.Empty;
            txtOVC_CHI_NAME.Text = string.Empty;
        }
        #endregion
        
        #region GridView
        protected void GVTBGMT_EDF_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            //Guid guidEDF_SN = (Guid)GVTBGMT_EDF.DataKeys[gvrIndex].Value;
            string id = GVTBGMT_EDF.DataKeys[gvrIndex].Value.ToString();
            Guid.TryParse(id, out Guid guidEDF_SN);

            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_REQ_DEPT_CDE", ViewState["OVC_REQ_DEPT_CDE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_EDF_NO", ViewState["OVC_EDF_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_START_PORT", ViewState["OVC_START_PORT"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_ARRIVE_PORT", ViewState["OVC_ARRIVE_PORT"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", ViewState["OVC_TRANSER_DEPT_CDE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_IS_STRATEGY", ViewState["OVC_IS_STRATEGY"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_REVIEW_STATUS", ViewState["OVC_REVIEW_STATUS"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_RECEIVE_DATE", ViewState["ODT_RECEIVE_DATE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_PURCH_NO", ViewState["OVC_PURCH_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_ITEM_NO2", ViewState["OVC_ITEM_NO2"], true);
            FCommon.setQueryString(ref strQueryString, "id", guidEDF_SN, true);

            switch (e.CommandName)
            {
                case "btnModify":
                    TBGMT_EDF edf = MTSE.TBGMT_EDF.Where(table => table.EDF_SN.Equals(guidEDF_SN)).FirstOrDefault();
                    if (edf != null)
                    {
                        string strOVC_EDF_NO = edf.OVC_EDF_NO;
                        string strOVC_REVIEW_STATUS = edf.OVC_REVIEW_STATUS ?? "";
                        if (strOVC_REVIEW_STATUS.Equals("通過"))
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"編號：{ strOVC_EDF_NO } 之外運資料表 已審核通過，不可修改！");
                        else
                            Response.Redirect($"MTS_A22_2{ strQueryString }");
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "外運資料表 不存在！");
                    break;
                case "btnDel":
                    edf = MTSE.TBGMT_EDF.Where(table => table.EDF_SN.Equals(guidEDF_SN)).FirstOrDefault();
                    if (edf != null)
                    {
                        string strMessage = "";
                        string strUserId = Session["userid"].ToString();
                        string strOVC_EDF_NO = edf.OVC_EDF_NO;
                        string strOVC_REVIEW_STATUS = edf.OVC_REVIEW_STATUS ?? "";
                        DateTime dateNow = DateTime.Now;
                        #region 錯誤訊息
                        if (strOVC_EDF_NO.Equals(string.Empty))
                            strMessage += $"<p> 外運資料表編號 錯誤！ </p>";
                        else
                        {
                            if (strOVC_REVIEW_STATUS.Equals("通過"))
                                strMessage += $"<p> 編號：{ strOVC_EDF_NO } 之外運資料表 已審核通過，不可刪除！ </p>";
                            TBGMT_ESO eso = MTSE.TBGMT_ESO.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();
                            if (eso != null)
                                strMessage += $"<p> 編號：{ strOVC_EDF_NO } 之外運資料表 已登入訂艙單，請先刪除訂艙單！ </p>";
                            TBGMT_EINN einn = MTSE.TBGMT_EINN.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();
                            if (einn != null)
                                strMessage += $"<p> 編號：{ strOVC_EDF_NO } 之外運資料表 已投保，請先刪除投保通知書：{ einn.OVC_EINN_NO } </p>";
                        }
                        #endregion

                        if (strMessage.Equals(string.Empty))
                        {
                            #region 新增 TBGMT_EDF_MRPLOG
                            TBGMT_EDF_MRPLOG edf_log = new TBGMT_EDF_MRPLOG();
                            edf_log.LOG_LOGIN_ID = strUserId;
                            edf_log.LOG_TIME = dateNow;
                            edf_log.LOG_EVENT = "DELETE";
                            edf_log.OVC_EDF_NO = edf.OVC_EDF_NO;
                            edf_log.OVC_PURCH_NO = edf.OVC_PURCH_NO;
                            edf_log.OVC_START_PORT = edf.OVC_START_PORT;
                            edf_log.OVC_ARRIVE_PORT = edf.OVC_ARRIVE_PORT;
                            edf_log.OVC_DEPT_CDE = edf.OVC_DEPT_CDE;
                            edf_log.ODT_RECEIVE_DATE = edf.ODT_RECEIVE_DATE;
                            edf_log.OVC_REQ_DEPT_CDE = edf.OVC_REQ_DEPT_CDE;
                            edf_log.OVC_CON_CHI_ADDRESS = edf.OVC_CON_CHI_ADDRESS;
                            edf_log.OVC_CON_ENG_ADDRESS = edf.OVC_CON_ENG_ADDRESS;
                            edf_log.OVC_CON_TEL = edf.OVC_CON_TEL;
                            edf_log.OVC_CON_FAX = edf.OVC_CON_FAX;
                            edf_log.OVC_NP_CHI_ADDRESS = edf.OVC_NP_CHI_ADDRESS;
                            edf_log.OVC_NP_ENG_ADDRESS = edf.OVC_NP_ENG_ADDRESS;
                            edf_log.OVC_NP_TEL = edf.OVC_NP_TEL;
                            edf_log.OVC_NP_FAX = edf.OVC_NP_FAX;
                            edf_log.OVC_ANP_CHI_ADDRESS = edf.OVC_ANP_CHI_ADDRESS;
                            edf_log.OVC_ANP_ENG_ADDRESS = edf.OVC_ANP_ENG_ADDRESS;
                            edf_log.OVC_ANP_TEL = edf.OVC_ANP_TEL;
                            edf_log.OVC_ANP_FAX = edf.OVC_ANP_FAX;
                            edf_log.OVC_PAYMENT_TYPE = edf.OVC_PAYMENT_TYPE;
                            edf_log.OVC_NOTE = edf.OVC_NOTE;
                            edf_log.OVC_IS_STRATEGY = edf.OVC_IS_STRATEGY;
                            edf_log.ODT_VALIDITY_DATE = edf.ODT_VALIDITY_DATE;
                            edf_log.OVC_LICENSE_NO = edf.OVC_LICENSE_NO;
                            edf_log.OVC_REVIEW_STATUS = edf.OVC_REVIEW_STATUS;
                            edf_log.OVC_REVIEW_LOGIN_ID = edf.OVC_REVIEW_LOGIN_ID;
                            edf_log.OVC_SHIP_FROM = edf.OVC_SHIP_FROM;
                            edf_log.ODT_REVIEW_DATE = edf.ODT_REVIEW_DATE;
                            edf_log.OVC_BLD_NO = edf.OVC_BLD_NO;
                            edf_log.ODT_MODIFY_DATE = edf.ODT_MODIFY_DATE;
                            edf_log.OVC_CREATE_LOGIN_ID = edf.OVC_CREATE_LOGIN_ID;
                            edf_log.OVC_ANP_ENG_ADDRESS2 = edf.OVC_ANP_ENG_ADDRESS2;
                            edf_log.OVC_ANP_TEL2 = edf.OVC_ANP_TEL2;
                            edf_log.OVC_ANP_FAX2 = edf.OVC_ANP_FAX2;
                            //edf_log.OVC_PURCH_NO_OLD = edf.OVC_PURCH_NO_OLD;
                            edf_log.EDF_MRP_SN = Guid.NewGuid();
                            MTSE.TBGMT_EDF_MRPLOG.Add(edf_log);
                            MTSE.SaveChanges();
                            FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), edf_log.GetType().Name, this, "新增");
                            #endregion

                            #region 新增 TBGMT_EDF_DETAIL_MRPLOG & 刪除 TBGMT_EDF_DETAIL
                            var queryDetail = MTSE.TBGMT_EDF_DETAIL.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO));
                            foreach (TBGMT_EDF_DETAIL edf_detail in queryDetail)
                            {
                                TBGMT_EDF_DETAIL_MRPLOG edf_detail_log = new TBGMT_EDF_DETAIL_MRPLOG();
                                edf_detail_log.LOG_LOGIN_ID = strUserId;
                                edf_detail_log.LOG_TIME = dateNow;
                                edf_detail_log.LOG_EVENT = "DELETE";
                                edf_detail_log.OVC_EDF_NO = edf_detail.OVC_EDF_NO;
                                edf_detail_log.OVC_EDF_ITEM_NO = edf_detail.OVC_EDF_ITEM_NO;
                                edf_detail_log.OVC_CHI_NAME = edf_detail.OVC_CHI_NAME;
                                edf_detail_log.OVC_ENG_NAME = edf_detail.OVC_ENG_NAME;
                                edf_detail_log.OVC_BOX_NO = edf_detail.OVC_BOX_NO;
                                edf_detail_log.OVC_ITEM_NO = edf_detail.OVC_ITEM_NO;
                                edf_detail_log.OVC_ITEM_NO2 = edf_detail.OVC_ITEM_NO2;
                                edf_detail_log.OVC_ITEM_NO3 = edf_detail.OVC_ITEM_NO3;
                                edf_detail_log.ONB_ITEM_COUNT = edf_detail.ONB_ITEM_COUNT;
                                edf_detail_log.OVC_ITEM_COUNT_UNIT = edf_detail.OVC_ITEM_COUNT_UNIT;
                                edf_detail_log.ONB_WEIGHT = edf_detail.ONB_WEIGHT;
                                edf_detail_log.OVC_WEIGHT_UNIT = edf_detail.OVC_WEIGHT_UNIT;
                                edf_detail_log.ONB_VOLUME = edf_detail.ONB_VOLUME;
                                edf_detail_log.OVC_VOLUME_UNIT = edf_detail.OVC_VOLUME_UNIT;
                                edf_detail_log.ONB_LENGTH = edf_detail.ONB_LENGTH;
                                edf_detail_log.ONB_WIDTH = edf_detail.ONB_WIDTH;
                                edf_detail_log.ONB_HEIGHT = edf_detail.ONB_HEIGHT;
                                edf_detail_log.ONB_MONEY = edf_detail.ONB_MONEY;
                                edf_detail_log.OVC_CURRENCY = edf_detail.OVC_CURRENCY;
                                edf_detail_log.ODT_MODIFY_DATE = edf_detail.ODT_MODIFY_DATE;
                                edf_detail_log.OVC_CREATE_LOGIN_ID = edf_detail.OVC_CREATE_LOGIN_ID;
                                edf_detail_log.EDF_DET_MRP_SN = Guid.NewGuid();
                                MTSE.TBGMT_EDF_DETAIL_MRPLOG.Add(edf_detail_log);
                                MTSE.SaveChanges();
                                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), edf_detail_log.GetType().Name, this, "新增");

                                MTSE.Entry(edf_detail).State = EntityState.Deleted;
                                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), edf_detail.GetType().Name, this, "刪除");
                                MTSE.SaveChanges();
                            }
                            #endregion

                            #region 刪除 TBGMT_EDF
                            MTSE.Entry(edf).State = EntityState.Deleted;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), edf.GetType().Name, this, "刪除");
                            #endregion

                            FCommon.AlertShow(PnMessage, "success", "系統訊息", $"編號：{ strOVC_EDF_NO } 之外運資料表 刪除成功。");
                            dataImport();
                        }
                        else
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "外運資料表 不存在！");
                    break;
                case "btnCheck":
                    Response.Redirect($"MTS_A22_3{ strQueryString }");
                    break;
                case "btnPrint":
                    edf = MTSE.TBGMT_EDF.Where(table => table.EDF_SN.Equals(guidEDF_SN)).FirstOrDefault();
                    if (edf != null)
                    {
                        string strOVC_EDF_NO = edf.OVC_EDF_NO;
                        print(strOVC_EDF_NO);
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "外運資料表 不存在！");
                    break;
                default:
                    break;
            }
        }
        protected void GVTBGMT_EDF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                GridView theGridView = (GridView)sender;
                int index = gvr.RowIndex;
                HyperLink hlkOVC_EDF_NO = (HyperLink)gvr.FindControl("hlkOVC_EDF_NO");
                if (FCommon.Controls_isExist(hlkOVC_EDF_NO))
                {
                    string strOVC_EDF_NO = theGridView.DataKeys[index].Value.ToString();
                    hlkOVC_EDF_NO.NavigateUrl = $"javascript: OpenWindow_EDFDATA('{ FCommon.getEncryption(strOVC_EDF_NO) }');";
                }
            }
        }
        protected void GVTBGMT_EDF_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        #endregion
    }
}