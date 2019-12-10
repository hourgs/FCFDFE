using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FCFDFE.pages.MTS.B
{
    public class Side2 : PdfPageEventHelper
    {
        PdfContentByte cb;

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            //設定字型
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H
          , BaseFont.NOT_EMBEDDED);
            Font ChFont = new Font(bfChinese, 10, Font.NORMAL, BaseColor.BLACK);
            string chFontPath = "c:\\windows\\fonts\\KAIU.TTF";
            BaseFont chBaseFont = BaseFont.CreateFont(chFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            //每頁頁尾
            PdfContentByte cb = writer.DirectContent;
            Rectangle pageSize = document.PageSize;
            cb.SetRGBColorFill(0, 0, 0);
            cb.BeginText();
            cb.SetFontAndSize(chBaseFont, 12);

            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "編號：977-3-02-01-03", pageSize.GetLeft(85), pageSize.GetBottom(290), 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "中華民國93年8月31日初版", pageSize.GetRight(85), pageSize.GetBottom(290), 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "中華民國93年11月4日二版", pageSize.GetRight(85), pageSize.GetBottom(275), 0);
            cb.EndText();

            int pageN = (writer.PageNumber);
            string[] text = { "第", "一", "聯", "：", "送", "合", "約", "保", "險", "公", "司", "", "", "" };
            string[] text2 = { "第", "二", "聯", "：", "送", "物", "資", "申", "購", "單", "位", "", "", "" };
            string[] text3 = { "第", "三", "聯", "：", "送", "採", "購", "室", "檔", "案", "室", "歸", "檔", "" };
            string[] text4 = { "第", "四", "聯", "：", "送", "採", "購", "物", "資", "接", "轉", "處", "存", "查" };
            cb.BeginText();
            for (int i = 0; i < 14; i++)
            {
                //if (pageN == 1)
                //{
                //    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text[i], pageSize.GetRight(20), pageSize.GetTop(70 + (i * 12)), 0);
                //}
                //else
                //{
                //    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text2[i], pageSize.GetRight(20), pageSize.GetTop(70 + (i * 12)), 0);
                //}
                switch (pageN)
                {
                    case 1:
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text[i], pageSize.GetRight(20), pageSize.GetTop(70 + (i * 12)), 0);
                        break;
                    case 2:
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text2[i], pageSize.GetRight(20), pageSize.GetTop(70 + (i * 12)), 0);
                        break;
                    case 3:
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text3[i], pageSize.GetRight(20), pageSize.GetTop(70 + (i * 12)), 0);
                        break;
                    case 4:
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text4[i], pageSize.GetRight(20), pageSize.GetTop(70 + (i * 12)), 0);
                        break;
                }
            }
            cb.EndText();
        }
    }
    public partial class MTS_B22_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();

        #region 副程式
        private void dataImport()
        {
            string strMessage = "";
            string strOVC_EINN_NO = txtOVC_EINN_NO.Text;
            string strOVC_EDF_NO = txtOVC_EDF_NO.Text;
            string strODT_INS_DATE = rdoODT_INS_DATE.SelectedValue;
            string strODT_INS_DATE_S = txtODT_INS_DATE_S.Text;
            string strODT_INS_DATE_E = txtODT_INS_DATE_E.Text;
            string strODT_CREATE_DATE = rdoODT_CREATE_DATE.SelectedValue;
            string strODT_CREATE_DATE_S = txtODT_CREATE_DATE_S.Text;
            string strODT_CREATE_DATE_E = txtODT_CREATE_DATE_E.Text;
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedValue;

            ViewState["OVC_EINN_NO"] = strOVC_EINN_NO;
            ViewState["OVC_EDF_NO"] = strOVC_EDF_NO;
            ViewState["ODT_INS_DATE"] = strODT_INS_DATE;
            ViewState["ODT_INS_DATE_S"] = strODT_INS_DATE_S;
            ViewState["ODT_INS_DATE_E"] = strODT_INS_DATE_E;
            ViewState["ODT_CREATE_DATE"] = strODT_CREATE_DATE;
            ViewState["ODT_CREATE_DATE_S"] = strODT_CREATE_DATE_S;
            ViewState["ODT_CREATE_DATE_E"] = strODT_CREATE_DATE_E;
            ViewState["OVC_MILITARY_TYPE"] = strOVC_MILITARY_TYPE;

            bool boolODT_INS_DATE = strODT_INS_DATE.Equals("2");
            bool boolODT_INS_DATE_S = DateTime.TryParse(strODT_INS_DATE_S, out DateTime dateODT_INS_DATE_S);
            bool boolODT_INS_DATE_E = DateTime.TryParse(strODT_INS_DATE_E, out DateTime dateODT_INS_DATE_E);
            bool boolODT_CREATE_DATE = strODT_CREATE_DATE.Equals("2");
            bool boolODT_CREATE_DATE_S = DateTime.TryParse(strODT_CREATE_DATE_S, out DateTime dateODT_CREATE_DATE_S);
            bool boolODT_CREATE_DATE_E = DateTime.TryParse(strODT_CREATE_DATE_E, out DateTime dateODT_CREATE_DATE_E);

            if (strOVC_EINN_NO.Equals(string.Empty) && strOVC_EDF_NO.Equals(string.Empty) &&!boolODT_INS_DATE
                && !boolODT_CREATE_DATE && strOVC_MILITARY_TYPE.Equals(string.Empty))
                strMessage += "<P> 至少填入一個選項！ </p>";
            else
            {
                if (boolODT_INS_DATE && !(boolODT_INS_DATE_S && boolODT_INS_DATE_E))
                    strMessage += "<P> 投保日期 不完全！ </p>";
                if (boolODT_CREATE_DATE && !(boolODT_CREATE_DATE_S && boolODT_CREATE_DATE_E))
                    strMessage += "<P> 建檔日期 不完全！ </p>";
            }

            if (strMessage.Equals(string.Empty))
            {
                var query =
                    from einn in MTSE.TBGMT_EINN.AsEnumerable()
                    join edf in MTSE.TBGMT_EDF on einn.OVC_EDF_NO equals edf.OVC_EDF_NO
                    join eso in MTSE.TBGMT_ESO on edf.OVC_EDF_NO equals eso.OVC_EDF_NO
                    join portStr in MTSE.TBGMT_PORTS on edf.OVC_START_PORT equals portStr.OVC_PORT_CDE into portStrTemp
                    from portStr in portStrTemp.DefaultIfEmpty()
                    join portArr in MTSE.TBGMT_PORTS on edf.OVC_ARRIVE_PORT equals portArr.OVC_PORT_CDE into portArrTemp
                    from portArr in portArrTemp.DefaultIfEmpty()
                    orderby einn.OVC_EINN_NO
                    select new
                    {
                        einn.EINN_SN,
                        einn.OVC_EINN_NO,
                        edf.EDF_SN,
                        edf.OVC_EDF_NO,
                        edf.OVC_PURCH_NO,
                        einn.ONB_ITEM_VALUE,
                        ODT_INS_DATE = einn.ODT_INS_DATE.ToString(),
                        ODT_CREATE_DATE = FCommon.getDateTime(einn.ODT_CREATE_DATE),
                        einn.ONB_INS_AMOUNT,
                        einn.OVC_FINAL_INS_AMOUNT,
                        OVC_START_PORT = portStr != null ? portStr.OVC_PORT_CHI_NAME : "",
                        OVC_ARRIVE_PORT = portArr != null ? portArr.OVC_PORT_CHI_NAME : "",
                        OVC_MILITARY_TYPE = einn.OVC_MILITARY_TYPE ?? ""
                    };
                if (!strOVC_EINN_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_EINN_NO.Contains(strOVC_EINN_NO));
                if (!strOVC_EDF_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_EDF_NO.Contains(strOVC_EDF_NO));
                if (boolODT_INS_DATE)
                    query = query.Where(table => DateTime.TryParse(table.ODT_INS_DATE, out DateTime dateODT_INS_DATE) &&
                        DateTime.Compare(dateODT_INS_DATE, dateODT_INS_DATE_S) >= 0 &&
                        DateTime.Compare(dateODT_INS_DATE, dateODT_INS_DATE_E) <= 0);
                if (boolODT_CREATE_DATE)
                    query = query.Where(table => DateTime.TryParse(table.ODT_CREATE_DATE, out DateTime dateODT_CREATE_DATE) &&
                        DateTime.Compare(dateODT_CREATE_DATE, dateODT_CREATE_DATE_S) >= 0 &&
                        DateTime.Compare(dateODT_CREATE_DATE, dateODT_CREATE_DATE_E) <= 0);
                if (!strOVC_MILITARY_TYPE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_MILITARY_TYPE.Equals(strOVC_MILITARY_TYPE));
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);

                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_EINN, dt);
                ViewState["dt"] = dt;
                btnPrint2.Visible = dt.Rows.Count > 0;
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void btnPrint(Guid guidEINN_SN)
        {
            Document doc = new Document(PageSize.A4, 10, 10, 30, 20);
            MemoryStream Memory = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, Memory);
            writer.PageEvent = new Side2();
            doc.Open();
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H
              , BaseFont.NOT_EMBEDDED);
            Font ChFont = new Font(bfChinese, 12, Font.NORMAL, BaseColor.BLACK);
            string chFontPath = "c:\\windows\\fonts\\KAIU.TTF";
            BaseFont chBaseFont = BaseFont.CreateFont(chFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            var query =
                from einn in MTSE.TBGMT_EINN
                where einn.EINN_SN.Equals(guidEINN_SN)
                join edf in MTSE.TBGMT_EDF on einn.OVC_EDF_NO equals edf.OVC_EDF_NO into edf
                from edf2 in edf.DefaultIfEmpty()
                join edf_detail in MTSE.TBGMT_EDF_DETAIL on einn.OVC_EDF_NO equals edf_detail.OVC_EDF_NO into edf_detail
                from edf_detail2 in edf_detail.DefaultIfEmpty()
                join eso in MTSE.TBGMT_ESO on einn.OVC_EDF_NO equals eso.OVC_EDF_NO into eso
                from eso2 in eso.DefaultIfEmpty()
                join port in MTSE.TBGMT_PORTS on edf2.OVC_START_PORT equals port.OVC_PORT_CDE into port
                from p in port.DefaultIfEmpty()
                join port2 in MTSE.TBGMT_PORTS on edf2.OVC_ARRIVE_PORT equals port2.OVC_PORT_CDE into port2
                from p2 in port2.DefaultIfEmpty()
                join dept in MTSE.TBGMT_DEPT_CLASS on einn.OVC_MILITARY_TYPE equals dept.OVC_CLASS into dept
                from d in dept.DefaultIfEmpty()
                join currency in MTSE.TBGMT_CURRENCY on einn.ONB_CARRIAGE_CURRENCY equals currency.OVC_CURRENCY_CODE into currency
                from c in currency.DefaultIfEmpty()
                join currency2 in MTSE.TBGMT_CURRENCY on einn.ONB_CARRIAGE_CURRENCY2 equals currency2.OVC_CURRENCY_CODE into currency2
                from c2 in currency2.DefaultIfEmpty()
                select new
                {
                    OVC_EINN_NO = einn.OVC_EINN_NO,
                    OVC_PURCH_NO = edf2.OVC_PURCH_NO,
                    OVC_CHI_NAME = edf_detail2.OVC_CHI_NAME,
                    ONB_ITEM_COUNT = edf_detail2.ONB_ITEM_COUNT ?? 0,
                    ONB_ITEM_VALUE = einn.ONB_ITEM_VALUE ?? 0,
                    ONB_INS_AMOUNT = einn.ONB_INS_AMOUNT ?? 0,
                    OVC_INS_CONDITION = einn.OVC_INS_CONDITION,
                    ONB_INS_RATE = einn.ONB_INS_RATE ?? 0,
                    OVC_SHIP_COMPANY = eso2.OVC_SHIP_COMPANY,
                    OVC_START_PORT = p.OVC_PORT_CHI_NAME,
                    ODT_START_DATE = eso2.ODT_START_DATE,
                    OVC_ARRIVE_PORT = p2.OVC_PORT_CHI_NAME,
                    OVC_PAYMENT_TYPE = einn.OVC_PAYMENT_TYPE,
                    OVC_CLASS_NAME = d.OVC_CLASS_NAME,
                    ODT_INS_DATE = einn.ODT_INS_DATE,
                    OVC_CURRENCY_NAME = c.OVC_CURRENCY_NAME,
                    OVC_CURRENCY_NAME2 = c2.OVC_CURRENCY_NAME
                };
            //if (!id.Equals(string.Empty))
            //{
            //    query = query.Where(table => table.OVC_EINN_NO.Equals(id));
            //}


            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            if (dt.Rows.Count > 0)
            {

                DataRow dr = dt.Rows[0];
                string strOVC_EINN_NO = dr["OVC_EINN_NO"].ToString();
                //創建table
                PdfPTable pdftable = new PdfPTable(new float[] { 3, 9 });
                pdftable.TotalWidth = 500f;
                pdftable.LockedWidth = true;
                pdftable.DefaultCell.FixedHeight = 60;

                PdfPCell title = new PdfPCell(new Phrase("國防部國防採購室國外物資海空運保險投保通知書", new Font(bfChinese, 14, Font.BOLD, BaseColor.BLACK)));
                title.VerticalAlignment = Element.ALIGN_TOP;
                title.HorizontalAlignment = Element.ALIGN_CENTER;
                title.Colspan = 2;
                title.FixedHeight = 50;
                pdftable.AddCell(title);

                DateTime today = DateTime.Now;
                PdfContentByte cb = writer.DirectContent;//插入標題右/左下角編號
                Rectangle pageSize = doc.PageSize;
                cb.SetRGBColorFill(0, 0, 0);
                cb.BeginText();
                cb.SetFontAndSize(chBaseFont, 10);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, strOVC_EINN_NO, pageSize.GetRight(50), pageSize.GetTop(65), 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, FCommon.getDateTime(today), pageSize.GetRight(50), pageSize.GetTop(75), 0);
                cb.EndText();


                PdfPCell cell1_0 = new PdfPCell(new Phrase("案號或採購文號", ChFont));
                cell1_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell1_0.FixedHeight = 30;
                pdftable.AddCell(cell1_0);

                PdfPCell cell1_1 = new PdfPCell(new Phrase(dr["OVC_PURCH_NO"].ToString(), ChFont));
                cell1_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell1_1.FixedHeight = 30;
                pdftable.AddCell(cell1_1);

                PdfPCell cell2_0 = new PdfPCell(new Phrase("物品名稱及數量", ChFont));
                cell2_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell2_0.FixedHeight = 30;
                pdftable.AddCell(cell2_0);

                int rows_count = dt.Rows.Count;
                int item_count = 0;
                for (int i = 0; i < rows_count; i++)
                {
                    item_count += Convert.ToInt32(dt.Rows[i]["ONB_ITEM_COUNT"]);
                }
                PdfPCell cell2_1 = new PdfPCell(new Phrase(dr["OVC_CHI_NAME"].ToString() + "/" + item_count + "件", ChFont));
                cell2_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell2_1.FixedHeight = 30;
                pdftable.AddCell(cell2_1);

                PdfPCell cell3_0 = new PdfPCell(new Phrase("物資價值", ChFont));
                cell3_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell3_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell3_0.FixedHeight = 30;
                pdftable.AddCell(cell3_0);

                PdfPCell cell3_1 = new PdfPCell(new Phrase(dr["ONB_ITEM_VALUE"].ToString() + dr["OVC_CURRENCY_NAME"], ChFont));
                cell3_1.FixedHeight = 30;
                cell3_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell3_1);

                PdfPCell cell4_0 = new PdfPCell(new Phrase("投保金額", ChFont));
                cell4_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell4_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4_0.FixedHeight = 30;
                pdftable.AddCell(cell4_0);

                PdfPCell cell4_1 = new PdfPCell(new Phrase(dr["ONB_INS_AMOUNT"].ToString() + dr["OVC_CURRENCY_NAME2"], ChFont));
                cell4_1.FixedHeight = 30;
                cell4_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell4_1);

                PdfPCell cell5_0 = new PdfPCell(new Phrase("保險條件", ChFont));
                cell5_0.FixedHeight = 30;
                cell5_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell5_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell5_0);

                PdfPCell cell5_1 = new PdfPCell(new Phrase(dr["OVC_INS_CONDITION"].ToString(), ChFont));
                cell5_1.FixedHeight = 30;
                cell5_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell5_1);



                PdfPCell cell6_0 = new PdfPCell(new Phrase("保險費率", ChFont));
                cell6_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell6_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell6_0.FixedHeight = 30;
                pdftable.AddCell(cell6_0);

                PdfPCell cell6_1 = new PdfPCell(new Phrase(dr["ONB_INS_RATE"].ToString() + "%", ChFont));
                cell6_1.FixedHeight = 30;
                cell6_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell6_1);

                PdfPCell cell7_0 = new PdfPCell(new Phrase("運輸工具", ChFont));
                cell7_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell7_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell7_0.FixedHeight = 30;
                pdftable.AddCell(cell7_0);

                PdfPCell cell7_1 = new PdfPCell(new Phrase(dr["OVC_SHIP_COMPANY"].ToString(), ChFont));
                cell7_1.FixedHeight = 30;
                cell7_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell7_1);

                PdfPCell cell8_0 = new PdfPCell(new Phrase("起運港口及時間", ChFont));
                cell8_0.FixedHeight = 30;
                cell8_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell8_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell8_0);



                PdfPCell cell8_1 = new PdfPCell(new Phrase(dr["OVC_START_PORT"].ToString(), ChFont));
                cell8_1.FixedHeight = 30;
                cell8_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell8_1);


                PdfPCell cell9_0 = new PdfPCell(new Phrase("目的港", ChFont));
                cell9_0.FixedHeight = 30;
                cell9_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell9_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell9_0);

                PdfPCell cell9_1 = new PdfPCell(new Phrase(dr["OVC_ARRIVE_PORT"].ToString(), ChFont));
                cell9_1.FixedHeight = 30;
                cell9_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell9_1);



                PdfPCell cell10_0 = new PdfPCell(new Phrase("保費支付方法", ChFont));
                cell10_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell10_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell10_0.FixedHeight = 30;
                pdftable.AddCell(cell10_0);

                PdfPCell cell10_1 = new PdfPCell();
                if (dr["OVC_PAYMENT_TYPE"].ToString().Equals(String.Empty))
                {
                    cell10_1 = new PdfPCell(new Phrase("保費向本室收取", ChFont));
                }
                else
                {
                    cell10_1 = new PdfPCell(new Phrase(dr["OVC_PAYMENT_TYPE"].ToString(), ChFont));
                }
                cell10_1.FixedHeight = 30;
                cell10_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell10_1);

                PdfPCell cell11_0 = new PdfPCell(new Phrase("軍種", ChFont));
                cell11_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell11_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell11_0.FixedHeight = 30;
                pdftable.AddCell(cell11_0);

                PdfPCell cell11_1 = new PdfPCell(new Phrase(dr["OVC_CLASS_NAME"].ToString(), ChFont));
                cell11_1.FixedHeight = 30;
                cell11_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell11_1);

                PdfPCell cell12_0 = new PdfPCell(new Phrase("投保日期", ChFont));
                cell12_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell12_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell12_0.FixedHeight = 30;
                pdftable.AddCell(cell12_0);

                PdfPCell cell12_1 = new PdfPCell();
                if (dr["ODT_INS_DATE"] != DBNull.Value)
                {
                    cell12_1 = new PdfPCell(new Phrase(FCommon.getDateTime(dr["ODT_INS_DATE"]), ChFont));
                }
                else
                {
                    cell12_1 = new PdfPCell(new Phrase(dr["ODT_INS_DATE"].ToString(), ChFont));
                }
                cell12_1.FixedHeight = 30;
                cell12_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell12_1);




                PdfPCell cell14_0 = new PdfPCell(new Phrase("附註：一、本通知書僅供保險費核計之用\n      二、如有錯誤請於文到一週內申復", ChFont));
                cell14_0.FixedHeight = 90;
                cell14_0.Colspan = 2;
                pdftable.AddCell(cell14_0);

                doc.Add(pdftable);
                doc.NewPage();
                doc.Add(pdftable);
                cb.BeginText();
                cb.SetFontAndSize(chBaseFont, 10);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, strOVC_EINN_NO, pageSize.GetRight(50), pageSize.GetTop(65), 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, FCommon.getDateTime(today), pageSize.GetRight(50), pageSize.GetTop(75), 0);
                cb.EndText();

                doc.NewPage();
                doc.Add(pdftable);
                cb.BeginText();
                cb.SetFontAndSize(chBaseFont, 10);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, strOVC_EINN_NO, pageSize.GetRight(50), pageSize.GetTop(65), 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, FCommon.getDateTime(today), pageSize.GetRight(50), pageSize.GetTop(75), 0);
                cb.EndText();

                doc.NewPage();
                doc.Add(pdftable);
                cb.BeginText();
                cb.SetFontAndSize(chBaseFont, 10);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, strOVC_EINN_NO, pageSize.GetRight(50), pageSize.GetTop(65), 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, FCommon.getDateTime(today), pageSize.GetRight(50), pageSize.GetTop(75), 0);
                cb.EndText();
                doc.Close();

                string strFileName = "國防部國防採購室國外物資海空運保險投保通知書.pdf";
                FCommon.DownloadFile(this, strFileName, Memory);
            }
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_EINN_NO", ViewState["OVC_EINN_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_EDF_NO", ViewState["OVC_EDF_NO"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_INS_DATE", ViewState["ODT_INS_DATE"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_INS_DATE_S", ViewState["ODT_INS_DATE_S"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_INS_DATE_E", ViewState["ODT_INS_DATE_E"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_CREATE_DATE", ViewState["ODT_CREATE_DATE"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_CREATE_DATE_S", ViewState["ODT_CREATE_DATE_S"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_CREATE_DATE_E", ViewState["ODT_CREATE_DATE_E"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_MILITARY_TYPE", ViewState["OVC_MILITARY_TYPE"], true);
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
                    FCommon.Controls_Attributes("readonly", "true", txtODT_INS_DATE_S, txtODT_INS_DATE_E, txtODT_CREATE_DATE_S, txtODT_CREATE_DATE_E);
                    DateTime dateNow = DateTime.Now;
                    #region 匯入下拉式選單
                    string strFirstText = "不限定";
                    CommonMTS.list_dataImport_DEPT_CLASS(drpOVC_MILITARY_TYPE, true, strFirstText); //軍種
                    #endregion
                    //當日前後兩個月
                    string strDate_S = FCommon.getDateTime(dateNow.AddMonths(-1)),
                           strDate_E = FCommon.getDateTime(dateNow.AddMonths(1));
                    txtODT_INS_DATE_S.Text = strDate_S;
                    txtODT_INS_DATE_E.Text = strDate_E;
                    txtODT_CREATE_DATE_S.Text = strDate_S;
                    txtODT_CREATE_DATE_E.Text = strDate_E;

                    bool boolImport = false;
                    if (FCommon.getQueryString(this, "OVC_EINN_NO", out string strOVC_EINN_NO, true))
                    {
                        txtOVC_EINN_NO.Text = strOVC_EINN_NO;
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_EDF_NO", out string strOVC_EDF_NO, true))
                        txtOVC_EDF_NO.Text = strOVC_EDF_NO;
                    if (FCommon.getQueryString(this, "ODT_INS_DATE", out string strODT_INS_DATE, true))
                        FCommon.list_setValue(rdoODT_INS_DATE, strODT_INS_DATE);
                    if (FCommon.getQueryString(this, "ODT_INS_DATE_S", out string strODT_INS_DATE_S, true))
                        txtODT_INS_DATE_S.Text = strODT_INS_DATE_S;
                    if (FCommon.getQueryString(this, "ODT_INS_DATE_E", out string strODT_INS_DATE_E, true))
                        txtODT_INS_DATE_E.Text = strODT_INS_DATE_E;
                    if (FCommon.getQueryString(this, "ODT_CREATE_DATE", out string strODT_CREATE_DATE, true))
                        FCommon.list_setValue(rdoODT_CREATE_DATE, strODT_CREATE_DATE);
                    if (FCommon.getQueryString(this, "ODT_CREATE_DATE_S", out string strODT_CREATE_DATE_S, true))
                        txtODT_CREATE_DATE_S.Text = strODT_CREATE_DATE_S;
                    if (FCommon.getQueryString(this, "ODT_CREATE_DATE_E", out string strODT_CREATE_DATE_E, true))
                        txtODT_CREATE_DATE_E.Text = strODT_CREATE_DATE_E;
                    if (FCommon.getQueryString(this, "OVC_MILITARY_TYPE", out string strOVC_MILITARY_TYPE, true))
                        FCommon.list_setValue(drpOVC_MILITARY_TYPE, strOVC_MILITARY_TYPE);
                    if (boolImport) dataImport();
                }
            }
        }

        #region ~Click
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }

        protected void btnPrint2_Click(object sender, EventArgs e)
        {
            Insurance_notice();
            //string file_temp = Request.PhysicalApplicationPath + "WordPDFprint/IN_Temp.pdf";
            //string file_name = "投保通知書.pdf";
            //FCommon.WordToPDF(this, Insurance_notice(), file_temp, file_name);
        }
        #endregion

        #region GridView
        protected void GV_TBGMT_EINN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            Guid einn_SN = (Guid)GV_TBGMT_EINN.DataKeys[gvrIndex].Value;
            //string edf_no = GV_TBGMT_EINN.Rows[gvrIndex].Cells[2].Text;
            //string einn_no = GV_TBGMT_EINN.Rows[gvrIndex].Cells[1].Text;

            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", einn_SN, true);
            //FCommon.setQueryString(ref strQueryString, "id", edf_no, true);
            //FCommon.setQueryString(ref strQueryString, "id2", einn_no, true);

            switch (e.CommandName)
            {
                case "dataModify":
                    FCommon.setQueryString(ref strQueryString, "action", "Modify", true);
                    Response.Redirect($"MTS_B22_2{ strQueryString }");
                    break;
                case "dataDel":
                    FCommon.setQueryString(ref strQueryString, "action", "Delete", true);
                    Response.Redirect($"MTS_B22_2{ strQueryString }");
                    break;
                /*try
                {
                    TBGMT_EINN einnModel = new TBGMT_EINN { EINN_SN = einn_SN };
                    MTSE.Entry(einnModel).State = EntityState.Deleted;
                    MTSE.SaveChanges();
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除投保通知書"+ einn_no + "成功");
                    Dtquery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                break;*/
                case "dataPrint":
                    btnPrint(einn_SN);
                    break;
                default:
                    break;
            }
        }
        protected void GV_TBGMT_EINN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                GridView theGridView = (GridView)sender;
                int index = gvr.RowIndex;
                HyperLink hlkOVC_EDF_NO = (HyperLink)gvr.FindControl("hlkOVC_EDF_NO");
                HiddenField lblEDF_SN = (HiddenField)gvr.FindControl("lblEDF_SN");
                if (FCommon.Controls_isExist(hlkOVC_EDF_NO, lblEDF_SN))
                {
                    string strEDF_SN = lblEDF_SN.Value;
                    hlkOVC_EDF_NO.NavigateUrl = $"javascript: OpenWindow_EDFDATA('{ FCommon.getEncryption(strEDF_SN) }');";
                }
            }
        }

        protected void GV_TBGMT_EINN_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void GV_TBGMT_EINN_RowCreated(object sender, GridViewRowEventArgs e)
        {
            GridView gv = (GridView)sender;
            decimal decTemp, decTitle = 0;
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                GridViewRow Tgvr = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Insert);
                TableCell cell0 = new TableCell();
                TableCell cell1 = new TableCell();
                TableCell cell2 = new TableCell();
                TableCell cell3 = new TableCell();
                TableCell cell4 = new TableCell();
                TableCell cell5 = new TableCell();
                TableCell cell6 = new TableCell();
                TableCell cell7 = new TableCell();
                TableCell cell8 = new TableCell();
                TableCell cell9 = new TableCell();
                TableCell cell10 = new TableCell();
                TableCell cell11 = new TableCell();
                cell1.Text = "共" + gv.Rows.Count + "筆記錄";
                for (int i = 0; i < gv.Rows.Count; i++)
                {
                    decTitle += decimal.TryParse(gv.Rows[i].Cells[6].Text, out decTemp) ? decTemp : 0;
                }
                cell6.Text = decTitle.ToString();
                Tgvr.Controls.Add(cell0);
                Tgvr.Controls.Add(cell1);
                Tgvr.Controls.Add(cell2);
                Tgvr.Controls.Add(cell3);
                Tgvr.Controls.Add(cell4);
                Tgvr.Controls.Add(cell5);
                Tgvr.Controls.Add(cell6);
                Tgvr.Controls.Add(cell7);
                Tgvr.Controls.Add(cell8);
                Tgvr.Controls.Add(cell9);
                Tgvr.Controls.Add(cell10);
                Tgvr.Controls.Add(cell11);
                GV_TBGMT_EINN.Controls[0].Controls.Add(Tgvr);
            }
        }
        #endregion

        #region 投保明細列印
        private void Insurance_notice()
        {
            #region 權限問題無法使用
            //string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/投保通知書.docx");
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    using (Xceed.Words.NET.DocX doc = Xceed.Words.NET.DocX.Load(path))
            //    {
            //        string today = (int.Parse(DateTime.Now.Year.ToString()) - 1911).ToString() + DateTime.Now.ToString("年MM月dd日");
            //        doc.ReplaceText("[$TODAY$]", today, false, System.Text.RegularExpressions.RegexOptions.None);
            //        var groceryListTable = doc.Tables.FirstOrDefault(table => table.TableCaption == "table");
            //        var rowPattern = groceryListTable.Rows[1];
            //        rowPattern.Remove();
            //        if (groceryListTable != null)
            //        {
            //            if (GV_TBGMT_EINN.Rows.Count > 0)
            //            {
            //                for (var i = 0; i < GV_TBGMT_EINN.Rows.Count; i++)
            //                {
            //                    HyperLink hlkOVC_EDF_NO = (HyperLink)GV_TBGMT_EINN.Rows[i].FindControl("hlkOVC_EDF_NO");
            //                    var newItem = groceryListTable.InsertRow(rowPattern, groceryListTable.RowCount);
            //                    newItem.ReplaceText("[$NO$]", (i + 1).ToString());
            //                    newItem.ReplaceText("[$OVC_EINN_NO$]", GV_TBGMT_EINN.Rows[i].Cells[1].Text.Replace("&nbsp;", ""));
            //                    newItem.ReplaceText("[$OVC_EDF_NO$]", hlkOVC_EDF_NO.Text.Replace("&nbsp;", ""));
            //                    newItem.ReplaceText("[$OVC_PURCH_NO$]", GV_TBGMT_EINN.Rows[i].Cells[3].Text.Replace("&nbsp;", ""));
            //                    newItem.ReplaceText("[$ONB_ITEM_VALUE$]", GV_TBGMT_EINN.Rows[i].Cells[4].Text.Replace("&nbsp;", ""));
            //                    newItem.ReplaceText("[$ODT_CREATE_DATE$]", GV_TBGMT_EINN.Rows[i].Cells[5].Text.Replace("&nbsp;", ""));
            //                    newItem.ReplaceText("[$ONB_INS_AMOUNT$]", GV_TBGMT_EINN.Rows[i].Cells[6].Text.Replace("&nbsp;", ""));
            //                    newItem.ReplaceText("[$OVC_FINAL_INS_AMOUNT$]", GV_TBGMT_EINN.Rows[i].Cells[7].Text.Replace("&nbsp;", ""));
            //                    newItem.ReplaceText("[$TRANSPORTATION$]", GV_TBGMT_EINN.Rows[i].Cells[8].Text.Replace("&nbsp;", ""));
            //                    newItem.ReplaceText("[$OVC_START_PORT$]", GV_TBGMT_EINN.Rows[i].Cells[9].Text.Replace("&nbsp;", ""));
            //                    newItem.ReplaceText("[$OVC_ARRIVE_PORT$]", GV_TBGMT_EINN.Rows[i].Cells[10].Text.Replace("&nbsp;", ""));
            //                }
            //            }
            //        }
            //        doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/IN_Temp.docx");
            //    }
            //}
            //string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/IN_Temp.docx");
            //return path_temp;
            #endregion

            if (ViewState["dt"] != null && ViewState["dt"] is DataTable)
            {
                string today = (int.Parse(DateTime.Now.Year.ToString()) - 1911).ToString() + DateTime.Now.ToString("年MM月dd日");
                DataTable dt = (DataTable)ViewState["dt"];
                string path = Request.PhysicalApplicationPath;//取得檔案絕對路徑
                BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
                Font ChTitle = new Font(bfChinese, 12, Font.BOLD);
                Font ChTabTitle = new Font(bfChinese, 8f);
                Font ChFont = new Font(bfChinese, 7f);
                var doc1 = new Document(PageSize.A4, 0, 0, 50, 80);
                MemoryStream Memory = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
                doc1.Open();

                PdfPTable table1 = new PdfPTable(11);
                table1.TotalWidth = 1200F;
                table1.SetWidths(new float[] { 1, 5, 5, 5, 3, 4, 3, 3, 3, 3, 3});
                table1.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table1.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell title = new PdfPCell(new Phrase("投保通知書\n", ChTitle));
                title.VerticalAlignment = Element.ALIGN_MIDDLE;
                title.HorizontalAlignment = Element.ALIGN_CENTER;
                title.Border = Rectangle.NO_BORDER;
                title.Colspan = 11;
                table1.AddCell(title);
                PdfPCell title_time = new PdfPCell(new Phrase("", ChTitle));
                title_time.VerticalAlignment = Element.ALIGN_MIDDLE;
                title_time.HorizontalAlignment = Element.ALIGN_CENTER;
                title_time.Border = Rectangle.NO_BORDER;
                title_time.Colspan = 7;
                table1.AddCell(title_time);
                PdfPCell title_time_2 = new PdfPCell(new Phrase("印表日期：" + today + "\n\n", ChTabTitle));
                title_time_2.VerticalAlignment = Element.ALIGN_MIDDLE;
                title_time_2.HorizontalAlignment = Element.ALIGN_CENTER;
                title_time_2.Border = Rectangle.NO_BORDER;
                title_time_2.Colspan = 4;
                table1.AddCell(title_time_2);

                table1.AddCell(new Phrase("項次", ChTabTitle));
                table1.AddCell(new Phrase("投保通知書\n編號", ChTabTitle));
                table1.AddCell(new Phrase("外運資料表\n編號", ChTabTitle));
                table1.AddCell(new Phrase("案號或採購\n文號", ChTabTitle));
                table1.AddCell(new Phrase("物資\n價值", ChTabTitle));
                table1.AddCell(new Phrase("出口日期", ChTabTitle));
                table1.AddCell(new Phrase("投保\n金額", ChTabTitle));
                table1.AddCell(new Phrase("保費\n(台幣)", ChTabTitle));
                table1.AddCell(new Phrase("運輸\n工具", ChTabTitle));
                table1.AddCell(new Phrase("啟運\n港口", ChTabTitle));
                table1.AddCell(new Phrase("目的\n港口", ChTabTitle));

                int i = 0;
                decimal decTitle = 0;
                string strFormat_Money = "#,0";
                foreach (DataRow dr in dt.Rows)
                {
                    string strNo = (i + 1).ToString(); //序號
                    decimal.TryParse(dr["ONB_ITEM_VALUE"].ToString(), out decimal decONB_ITEM_VALUE);
                    decimal.TryParse(dr["ONB_INS_AMOUNT"].ToString(), out decimal decONB_INS_AMOUNT);
                    decimal.TryParse(dr["OVC_FINAL_INS_AMOUNT"].ToString(), out decimal decOVC_FINAL_INS_AMOUNT);

                    table1.AddCell(new Phrase(strNo, ChFont));
                    table1.AddCell(new Phrase(dr["OVC_EINN_NO"].ToString(), ChFont));
                    table1.AddCell(new Phrase(dr["OVC_EDF_NO"].ToString(), ChFont));
                    table1.AddCell(new Phrase(dr["OVC_PURCH_NO"].ToString(), ChFont));
                    table1.AddCell(new Phrase(decONB_ITEM_VALUE.ToString(strFormat_Money), ChFont));
                    table1.AddCell(new Phrase(dr["ODT_CREATE_DATE"].ToString(), ChFont));
                    table1.AddCell(new Phrase(decONB_INS_AMOUNT.ToString(strFormat_Money), ChFont));
                    table1.AddCell(new Phrase(decOVC_FINAL_INS_AMOUNT.ToString(strFormat_Money), ChFont));
                    table1.AddCell(new Phrase("", ChFont));
                    table1.AddCell(new Phrase(dr["OVC_START_PORT"].ToString(), ChFont));
                    table1.AddCell(new Phrase(dr["OVC_ARRIVE_PORT"].ToString(), ChFont));

                    i++;
                    decTitle += decONB_INS_AMOUNT;
                }
                table1.AddCell(new Phrase("", ChFont));
                table1.AddCell(new Phrase("共" + i.ToString() + "筆記錄", ChFont));
                table1.AddCell(new Phrase("", ChFont));
                table1.AddCell(new Phrase("", ChFont));
                table1.AddCell(new Phrase("", ChFont));
                table1.AddCell(new Phrase("", ChFont));
                table1.AddCell(new Phrase(decTitle.ToString(), ChFont));
                table1.AddCell(new Phrase("", ChFont));
                table1.AddCell(new Phrase("", ChFont));
                table1.AddCell(new Phrase("", ChFont));
                table1.AddCell(new Phrase("", ChFont));

                doc1.Add(table1);
                doc1.Close();

                string strFileName = "投保通知書.pdf";
                FCommon.DownloadFile(this, strFileName, Memory);
            }
        }
        #endregion
    }
}