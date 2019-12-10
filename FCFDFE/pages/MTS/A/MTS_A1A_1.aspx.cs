using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;

namespace FCFDFE.pages.MTS.A
{
    public class pdf : PdfPageEventHelper
    {
        PdfTemplate template;

        BaseFont bf = null;
        PdfContentByte cb;
        private string iHO_NO;

        public pdf(string iHO_NO)
        {
            this.iHO_NO = iHO_NO;
        }

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
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\\windows\\fonts\\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型

            cb = writer.DirectContent;
            cb.SetRGBColorFill(100, 100, 100);

            cb.BeginText();
            if (writer.CurrentPageNumber == 1)
            {
                cb.SetFontAndSize(bfChinese, 22f);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "國防部國防採購室軍品運輸交接單", pageSize.GetRight(280), pageSize.GetTop(75), 0);
                cb.SetFontAndSize(bfChinese, 12f);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "交接單編號：" + this.iHO_NO, pageSize.GetRight(30), pageSize.GetTop(90), 0);
            }
            cb.SetFontAndSize(bfChinese, 11f);
            String text = "第 " + writer.CurrentPageNumber + " 頁，共   頁";
            cb.SetTextMatrix(pageSize.GetRight(115), pageSize.GetTop(104));
            cb.ShowText(text);

            //cb.AddTemplate(template, pageSize.GetRight(30), pageSize.GetTop(110));
            //cb.AddTemplate(template, pageSize.GetRight(30), pageSize.GetBottom(30));
            cb.EndText();

            cb.AddTemplate(template, pageSize.GetRight(49), pageSize.GetTop(104));

        }
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\\windows\\fonts\\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型            Rectangle pageSize = document.PageSize;
            base.OnCloseDocument(writer, document);

            template.BeginText();
            template.Height = 50;
            template.SetFontAndSize(bfChinese, 12f);
            template.SetTextMatrix(0, 0);
            template.ShowText((writer.PageNumber).ToString());
            template.EndText();

        }
    }

    public partial class MTS_A1A_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();

        #region 副程式
        private void dataImport()
        {
            string strMessage = "";
            string strOVC_IHO_NO = txtOVC_IHO_NO.Text;
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_TRANSER_DEPT_CDE = drpOVC_TRANSER_DEPT_CDE.SelectedValue;
            string strOVC_DEPT_CDE = txtOVC_DEPT_CDE.Value;
            string strODT_START_DATE_S = txtODT_START_DATE_S.Text;
            string strODT_START_DATE_E = txtODT_START_DATE_E.Text;
            string strODT_ARRIVE_DATE_S = txtODT_ARRIVE_DATE_S.Text;
            string strODT_ARRIVE_DATE_E = txtODT_ARRIVE_DATE_E.Text;
            ViewState["OVC_IHO_NO"] = strOVC_IHO_NO;
            ViewState["OVC_BLD_NO"] = strOVC_BLD_NO;
            ViewState["OVC_TRANSER_DEPT_CDE"] = strOVC_TRANSER_DEPT_CDE;
            ViewState["OVC_DEPT_CDE"] = strOVC_DEPT_CDE;
            ViewState["ODT_START_DATE_S"] = strODT_START_DATE_S;
            ViewState["ODT_START_DATE_E"] = strODT_START_DATE_E;
            ViewState["ODT_ARRIVE_DATE_S"] = strODT_ARRIVE_DATE_S;
            ViewState["ODT_ARRIVE_DATE_E"] = strODT_ARRIVE_DATE_E;

            if (strOVC_IHO_NO.Equals(string.Empty) && strOVC_BLD_NO.Equals(string.Empty) && strOVC_TRANSER_DEPT_CDE.Equals(string.Empty) && strOVC_DEPT_CDE.Equals(string.Empty)
                && (strODT_START_DATE_S.Equals(string.Empty) || strODT_START_DATE_E.Equals(string.Empty)) && (strODT_ARRIVE_DATE_S.Equals(string.Empty) || strODT_ARRIVE_DATE_E.Equals(string.Empty)))
                strMessage += "<P> 至少填入一個選項 </p>";
            bool boolODT_START_DATE_S = FCommon.checkDateTime(strODT_START_DATE_S, "起運時間－開始", ref strMessage, out DateTime dateODT_START_DATE_S);
            bool boolODT_START_DATE_E = FCommon.checkDateTime(strODT_START_DATE_E, "起運時間－結束", ref strMessage, out DateTime dateODT_START_DATE_E);
            bool boolODT_ARRIVE_DATE_S = FCommon.checkDateTime(strODT_ARRIVE_DATE_S, "抵運時間－開始", ref strMessage, out DateTime dateODT_ARRIVE_DATE_S);
            bool boolODT_ARRIVE_DATE_E = FCommon.checkDateTime(strODT_ARRIVE_DATE_E, "抵運時間－結束", ref strMessage, out DateTime dateODT_ARRIVE_DATE_E);

            if (strMessage.Equals(string.Empty))
            {
                var query =
                    from iho in MTSE.TBGMT_IHO.AsEnumerable()
                    join ird_detail in MTSE.TBGMT_IRD_DETAIL.AsEnumerable() on iho.OVC_IHO_NO equals ird_detail.OVC_IHO_NO into ird_detailGrpoup
                    //from ird_detail in detailTemp.GroupBy(table => table.OVC_IHO_NO).DefaultIfEmpty()
                    join dept in GME.TBMDEPTs on iho.OVC_RECEIVE_DEPT_CDE equals dept.OVC_DEPT_CDE into tempDept
                    from dept in tempDept.DefaultIfEmpty()
                    select new
                    {
                        OVC_IHO_NO = iho.OVC_IHO_NO,
                        //OVC_BLD_NO = string.Join(",\n", (from ird_detail in subGrp group ird_detail by ird_detail.OVC_BLD_NO into groupDetail select groupDetail.Key).ToArray()),
                        OVC_BLD_NO = string.Join(", \n", ird_detailGrpoup.GroupBy(table => table.OVC_BLD_NO).Select(table => table.Key)) ?? "",
                        OVC_INLAND_TRANS_TYPE = iho.OVC_INLAND_TRANS_TYPE,
                        OVC_START_PLACE = iho.OVC_START_PLACE,
                        iho.OVC_ARRIVE_PLACE,
                        OVC_RECEIVE_DEPT_CDE_Value = iho.OVC_RECEIVE_DEPT_CDE ?? "", //ee
                        OVC_RECEIVE_DEPT_CDE = dept != null ? dept.OVC_ONNAME : "",
                        //OVC_TRANSER_DEPT_CDE_Value = iho.OVC_TRANSER_DEPT_CDE ?? "",
                        OVC_TRANSER_DEPT_CDE = iho.OVC_TRANSER_DEPT_CDE ?? "",
                        ODT_START_DATE = FCommon.getDateTime(iho.ODT_START_DATE),
                        ODT_ARRIVE_DATE = FCommon.getDateTime(iho.ODT_ARRIVE_DATE),
                    };
                if (!strOVC_IHO_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_IHO_NO.Contains(strOVC_IHO_NO));
                if (!strOVC_BLD_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
                if (!strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_TRANSER_DEPT_CDE.Equals(strOVC_TRANSER_DEPT_CDE));
                //{
                //    var table = GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("TR"))
                //        .Where(t => t.OVC_PHR_PARENTS.Equals(strOVC_TRANSER_DEPT_CDE))
                //        .Select(t => t.OVC_PHR_ID).ToArray();

                //    query = query.Where(t => table.Contains(t.OVC_TRANSER_DEPT_CDE));
                //}
                if (!strOVC_DEPT_CDE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_RECEIVE_DEPT_CDE_Value.Equals(strOVC_DEPT_CDE));
                if (!strODT_START_DATE_S.Equals(string.Empty) && !strODT_START_DATE_E.Equals(string.Empty))
                    query = query.Where(table => DateTime.TryParse(table.ODT_START_DATE, out DateTime dateODT_START_DATE) &&
                        DateTime.Compare(dateODT_START_DATE, dateODT_START_DATE_S) >= 0 &&
                        DateTime.Compare(dateODT_START_DATE, dateODT_START_DATE_E) <= 0);
                if (!strODT_ARRIVE_DATE_S.Equals(string.Empty) && !strODT_ARRIVE_DATE_E.Equals(string.Empty))
                    query = query.Where(table => DateTime.TryParse(table.ODT_ARRIVE_DATE, out DateTime dateODT_ARRIVE_DATE) &&
                        DateTime.Compare(dateODT_ARRIVE_DATE, dateODT_ARRIVE_DATE_S) >= 0 &&
                        DateTime.Compare(dateODT_ARRIVE_DATE, dateODT_ARRIVE_DATE_E) <= 0);
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);

                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_IHO, dt);
            }
            else
                FCommon.AlertShow(PnWarning, "danger", "系統訊息", strMessage);
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_IHO_NO", ViewState["OVC_IHO_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", ViewState["OVC_BLD_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", ViewState["OVC_TRANSER_DEPT_CDE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_DEPT_CDE", ViewState["OVC_DEPT_CDE"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_START_DATE_S", ViewState["ODT_START_DATE_S"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_START_DATE_E", ViewState["ODT_START_DATE_E"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_ARRIVE_DATE_S", ViewState["ODT_ARRIVE_DATE_S"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_ARRIVE_DATE_E", ViewState["ODT_ARRIVE_DATE_E"], true);
            return strQueryString;
        }
        private void btnPrint(string IHO_NO, string[] list)
        {
            String OVC_IHO_NO = IHO_NO;
            String OVC_BLD_NO = list[0];
            String drpOVC_TRANSER_DEPT_CDE = list[1];
            String OVC_ONNAME = list[2];

            var query =
                from ihoT in MTSE.TBGMT_IHO.AsEnumerable().Where(table => table.OVC_IHO_NO == OVC_IHO_NO)
                join irddT in MTSE.TBGMT_IRD_DETAIL.AsEnumerable() on ihoT.OVC_IHO_NO equals irddT.OVC_IHO_NO into irddT_temp
                //join dept in MTSE.TBGMT_DEPT_CDE.AsEnumerable() on ihoT.OVC_RECEIVE_DEPT_CDE equals dept.OVC_DEPT_CODE into dept
                //from dept_name in dept.DefaultIfEmpty()
                join dept in GME.TBMDEPTs on ihoT.OVC_RECEIVE_DEPT_CDE equals dept.OVC_DEPT_CDE into tempDept
                from dept in tempDept.DefaultIfEmpty()
                join dept2 in MTSE.TBGMT_DEPT_CDE.AsEnumerable() on ihoT.OVC_TRANSER_DEPT_CDE equals dept2.OVC_DEPT_CODE into dept2
                from dept_name2 in dept2.DefaultIfEmpty()


                    //from irddT in irddT_temp.DefaultIfEmpty()
                    //from irddT in MTSE.TBGMT_IRD_DETAIL.DefaultIfEmpty().AsEnumerable().Where(Table => Table.OVC_IHO_NO == OVC_IHO_NO)

                select new
                {
                    ihoT.OVC_IHO_NO,            //交接單編號
                    ihoT.OVC_INLAND_TRANS_TYPE, //運輸方法
                    ihoT.OVC_START_PLACE,       //起運地點
                    ihoT.OVC_ARRIVE_PLACE,      //運達地點
                    ihoT.ODT_START_DATE,        //起運時間
                    ihoT.ODT_ARRIVE_DATE,       //抵運時間
                                                // OVC_RECEIVE_DEPT_CDE = dept_name != null ? dept_name.OVC_DEPT_NAME : "",  //接收單位
                                                //OVC_RECEIVE_DEPT_CDE =txtOVC_DEPT_CDE,  //接收單位                    
                                                //OVC_TRANSER_DEPT_CDE = strOVC_TRANSER_DEPT_CDE,  //接轉單位

                    OVC_RECEIVE_DEPT_CDE = dept != null ? dept.OVC_ONNAME : "",
                    OVC_TRANSER_DEPT_CDE = "國防部國防採購室" + ihoT.OVC_TRANSER_DEPT_CDE ?? "",
                    ihoT.ONB_OVERFLOW,          //超出件數
                    ihoT.ONB_LESS,              //短少件數
                    ihoT.ONB_ACTUAL_RECEIVE,    //實收件數
                    ihoT.ONB_BROKEN,            //破損件數
                    ihoT.OVC_NOTE,              //備考
                    ihoT.ONB_QUANITY,           //件數
                    ihoT.OVC_QUANITY_UNIT,      //件數計量單位
                    ihoT.ONB_VOLUME,            //體積
                    ihoT.OVC_VOLUME_UNIT,       //體積計量單位
                    ihoT.ONB_WEIGHT,            //重量
                    ihoT.OVC_WEIGHT_UNIT,       //重量計量單位
                    ihoT.OVC_SHIP_NAME,         //船機名稱
                    ihoT.OVC_VOYAGE,            //船機航次
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);

            var query_bld =
                from bldT in MTSE.TBGMT_BLD.AsEnumerable()
                where bldT.OVC_BLD_NO.Equals(OVC_BLD_NO)
                select new
                {
                    bldT.ODT_ACT_ARRIVE_DATE    //進口日期
                };
            DataTable dta = CommonStatic.LinqQueryToDataTable(query_bld);

            var query_detail =
                from irddT in MTSE.TBGMT_IRD_DETAIL.DefaultIfEmpty().AsEnumerable().Where(Table => Table.OVC_IHO_NO == OVC_IHO_NO)
                join icrT in MTSE.TBGMT_ICR.DefaultIfEmpty().AsEnumerable() on irddT.OVC_BLD_NO equals icrT.OVC_BLD_NO
                //into icrT_temp
                //from icrT in icrT_temp.DefaultIfEmpty()
                //where irddT.OVC_IHO_NO.Equals(OVC_IHO_NO)
                select new
                {
                    irddT.OVC_BLD_NO,   //提單號
                    irddT.OVC_PURCH_NO, //購案號碼
                    icrT.OVC_CHI_NAME,  //中文品名
                    irddT.OVC_BOX_NO,   //箱號
                    irddT.ONB_OVERFLOW,          //超出件數
                    irddT.ONB_LESS,              //短少件數
                    irddT.ONB_ACTUAL_RECEIVE,    //實收件數
                    irddT.ONB_BROKEN,            //破損件數
                };
            DataTable dtd = CommonStatic.LinqQueryToDataTable(query_detail);


            BaseFont bfChinese = BaseFont.CreateFont(@"C:\\windows\\fonts\\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChFont = new Font(bfChinese, 12f);
            Font smaillChFont = new Font(bfChinese, 10f);
            Font Title_ChFont = new Font(bfChinese, 22f, Font.BOLD);
            Font STitle_ChFont = new Font(bfChinese, 14f);
            MemoryStream Memory = new MemoryStream();
            Document doc1 = new Document(PageSize.A4, 50, 50, 110, -10);
            //PdfWriter pdfWriter = PdfWriter.GetInstance(doc1, new FileStream(@"C:\Users\linon\Downloads\國防部國防採購室軍品運輸交接單.pdf", FileMode.Create)); //指定路徑創造
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc1, Memory);
            pdfWriter.PageEvent = new pdf(IHO_NO);
            doc1.Open();


            PdfPTable firsttable = new PdfPTable(4);
            firsttable.SetWidths(new float[] { 2.5f, 4.5f, 2.5f, 4.5f });
            firsttable.DefaultCell.BorderWidth = 1;
            firsttable.TotalWidth = 560F;
            firsttable.LockedWidth = true;
            firsttable.DefaultCell.SetLeading(1.2f, 1.2f);
            firsttable.DefaultCell.FixedHeight = 25f;
            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            firsttable.AddCell(new Phrase("清運方式", ChFont));
            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            firsttable.AddCell(new Phrase(dt.Rows[0][1].ToString(), ChFont));
            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            firsttable.AddCell(new Phrase("接收單位 ", ChFont));
            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            firsttable.AddCell(new Phrase(dt.Rows[0][6].ToString(), ChFont));
            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            firsttable.AddCell(new Phrase("接轉單位", ChFont));
            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            firsttable.AddCell(new Phrase(dt.Rows[0][7].ToString(), ChFont));
            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            firsttable.AddCell(new Phrase("進口日期", ChFont));
            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            try
            {
                if (dta.Rows[0][0] != DBNull.Value)
                    firsttable.AddCell(new Phrase(Convert.ToDateTime(dta.Rows[0][0]).ToString("yyyy/MM/dd"), ChFont));
                else
                    firsttable.AddCell(new Phrase("", ChFont));
            }
            catch (Exception)
            {
                firsttable.AddCell(new Phrase("", ChFont));
            }

            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            firsttable.AddCell(new Phrase("船(機)名", ChFont));
            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            firsttable.AddCell(new Phrase(dt.Rows[0][19].ToString(), ChFont));
            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            firsttable.AddCell(new Phrase("航次", ChFont));
            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            firsttable.AddCell(new Phrase(dt.Rows[0][20].ToString(), ChFont));
            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            firsttable.AddCell(new Phrase("起運地點", ChFont));
            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            firsttable.AddCell(new Phrase(dt.Rows[0][2].ToString(), ChFont));
            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            firsttable.AddCell(new Phrase("運達地點", ChFont));
            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            firsttable.AddCell(new Phrase(dt.Rows[0][3].ToString(), ChFont));
            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            firsttable.AddCell(new Phrase("起運時間", ChFont));
            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (dt.Rows[0][4] != DBNull.Value)
                firsttable.AddCell(new Phrase(Convert.ToDateTime(dt.Rows[0][4]).ToString("yyyy/MM/dd"), ChFont));
            else
                firsttable.AddCell(new Phrase("", ChFont));

            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            firsttable.AddCell(new Phrase("抵達時間", ChFont));
            firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (dt.Rows[0][5] != DBNull.Value)
                firsttable.AddCell(new Phrase(Convert.ToDateTime(dt.Rows[0][5]).ToString("yyyy/MM/dd"), ChFont));
            else
                firsttable.AddCell(new Phrase("", ChFont));


            doc1.Add(firsttable);

            PdfPTable secendtable = new PdfPTable(4);
            secendtable.SetWidths(new float[] { 2.5f, 3.83f, 3.83f, 3.83f });
            secendtable.DefaultCell.BorderWidth = 1;
            secendtable.TotalWidth = 560F;
            secendtable.LockedWidth = true;
            secendtable.DefaultCell.FixedHeight = 25f;
            secendtable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell tail = new PdfPCell(new Phrase("軍品情形", ChFont));
            tail.Rowspan = 2;
            tail.BorderWidth = 1;
            tail.FixedHeight = 50f;
            tail.HorizontalAlignment = Element.ALIGN_CENTER;
            tail.VerticalAlignment = Element.ALIGN_MIDDLE;
            secendtable.AddCell(tail);
            secendtable.AddCell(new Phrase("總件數", ChFont));
            secendtable.AddCell(new Phrase("總體積", ChFont));
            secendtable.AddCell(new Phrase("總重量", ChFont));
            //string num = float.Parse(dt.Rows[0][13].ToString()).ToString("f3");
            //string volume = float.Parse(dt.Rows[0][15].ToString()).ToString("f3");
            //string weight = float.Parse(dt.Rows[0][17].ToString()).ToString("f3");
            decimal.TryParse(dt.Rows[0]["ONB_QUANITY"].ToString(), out decimal decONB_QUANITY);
            decimal.TryParse(dt.Rows[0]["ONB_VOLUME"].ToString(), out decimal decONB_VOLUME);
            decimal.TryParse(dt.Rows[0]["ONB_WEIGHT"].ToString(), out decimal decONB_WEIGHT);
            secendtable.AddCell(new Phrase(decONB_QUANITY.ToString("N3") + " " + dt.Rows[0]["OVC_QUANITY_UNIT"].ToString(), ChFont));
            secendtable.AddCell(new Phrase(decONB_VOLUME.ToString("N3") + " " + dt.Rows[0]["OVC_VOLUME_UNIT"].ToString(), ChFont));
            secendtable.AddCell(new Phrase(decONB_WEIGHT.ToString("N3") + " " + dt.Rows[0]["OVC_WEIGHT_UNIT"].ToString(), ChFont));
            doc1.Add(secendtable);


            PdfPTable thirdtable = new PdfPTable(5);
            thirdtable.SetWidths(new float[] { 2.5f, 2.875f, 2.875f, 2.875f, 2.875f });
            thirdtable.TotalWidth = 560F;
            thirdtable.DefaultCell.BorderWidth = 1;
            thirdtable.LockedWidth = true;
            thirdtable.DefaultCell.FixedHeight = 25f;
            thirdtable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell ordercell = new PdfPCell(new Phrase("點收情形", ChFont));
            ordercell.Rowspan = 2;
            ordercell.FixedHeight = 50f;
            ordercell.BorderWidth = 1;
            ordercell.HorizontalAlignment = Element.ALIGN_CENTER;
            ordercell.VerticalAlignment = Element.ALIGN_MIDDLE;
            thirdtable.AddCell(ordercell);

            thirdtable.AddCell(new Phrase("超出", ChFont));
            thirdtable.AddCell(new Phrase("短少", ChFont));
            thirdtable.AddCell(new Phrase("破損", ChFont));
            thirdtable.AddCell(new Phrase("實收", ChFont));
            if (dt.Rows[0][8].ToString().Equals("0"))
                thirdtable.AddCell(new Phrase("", ChFont));
            else
                thirdtable.AddCell(new Phrase(dt.Rows[0][8].ToString(), ChFont));
            if (dt.Rows[0][8].ToString().Equals("0"))
                thirdtable.AddCell(new Phrase("", ChFont));
            else
                thirdtable.AddCell(new Phrase(dt.Rows[0][9].ToString(), ChFont));
            if (dt.Rows[0][8].ToString().Equals("0"))
                thirdtable.AddCell(new Phrase("", ChFont));
            else
                thirdtable.AddCell(new Phrase(dt.Rows[0][11].ToString(), ChFont));
            if (dt.Rows[0][8].ToString().Equals("0"))
                thirdtable.AddCell(new Phrase("", ChFont));
            else
                thirdtable.AddCell(new Phrase(dt.Rows[0][10].ToString(), ChFont));

            PdfPCell Remarks = new PdfPCell(new Phrase("備考", ChFont));
            Remarks.FixedHeight = 50f;
            Remarks.BorderWidth = 1;
            Remarks.HorizontalAlignment = Element.ALIGN_CENTER;
            Remarks.VerticalAlignment = Element.ALIGN_MIDDLE;
            thirdtable.AddCell(Remarks);
            PdfPCell Remarkscell = new PdfPCell(new Phrase("", ChFont));
            Remarkscell.Colspan = 4;
            Remarkscell.BorderWidth = 1;
            Remarkscell.VerticalAlignment = Element.ALIGN_MIDDLE;
            thirdtable.AddCell(Remarkscell);
            Font ChBFont = new Font(bfChinese, 14f, Font.BOLD, BaseColor.BLUE);
            PdfPCell BlueRemarkscell = new PdfPCell(new Phrase("接收單位請於完成接收點交後，於「單位接收人」欄位簽章並註明時間(11碼，另於「點收情形」欄位寫實際點收件數，如有貨品異常請於「備考」欄位註明該箱號及異常狀況。", ChBFont));
            BlueRemarkscell.Colspan = 5;
            BlueRemarkscell.FixedHeight = 50f;
            BlueRemarkscell.BorderWidth = 1;
            BlueRemarkscell.VerticalAlignment = Element.ALIGN_MIDDLE;
            thirdtable.AddCell(BlueRemarkscell);
            doc1.Add(thirdtable);

            PdfPTable fourthtable = new PdfPTable(6);
            fourthtable.SetWidths(new float[] { 2.5f, 3.2f, 1.4f, 2.8f, 1.2f, 3f });
            fourthtable.TotalWidth = 560F;
            fourthtable.LockedWidth = true;
            fourthtable.DefaultCell.BorderWidth = 1;
            fourthtable.DefaultCell.FixedHeight = 80f;
            fourthtable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            fourthtable.AddCell(new Phrase("物流公司\n接收人", ChFont));
            fourthtable.AddCell(new Phrase("", ChFont));
            fourthtable.AddCell(new Phrase("單位\n接收人", ChFont));
            fourthtable.AddCell(new Phrase("", ChFont));
            fourthtable.AddCell(new Phrase("發貨人", ChFont));
            fourthtable.AddCell(new Phrase("", ChFont));
            fourthtable.AddCell(new Phrase("承辦人", ChFont));
            fourthtable.AddCell(new Phrase("", ChFont));
            fourthtable.AddCell(new Phrase("審核", ChFont));
            fourthtable.AddCell(new Phrase("", ChFont));
            fourthtable.AddCell(new Phrase("主管", ChFont));
            fourthtable.AddCell(new Phrase("", ChFont));
            doc1.Add(fourthtable);

            PdfPTable ntable = new PdfPTable(1);
            ntable.SetWidths(new float[] { 1 });
            ntable.DefaultCell.Border = Rectangle.NO_BORDER;
            ntable.TotalWidth = 560F;
            ntable.LockedWidth = true;
            ntable.DefaultCell.FixedHeight = 10f;
            ntable.AddCell(new Phrase("", ChFont));
            ntable.AddCell(new Phrase("", ChFont));
            doc1.Add(ntable);


            PdfPTable fifthtable = new PdfPTable(6);
            fifthtable.SetWidths(new float[] { 1, 2, 2, 3, 2, 2 });
            fifthtable.TotalWidth = 560F;
            fifthtable.DefaultCell.BorderWidth = 1;
            fifthtable.LockedWidth = true;
            fifthtable.DefaultCell.FixedHeight = 40f;
            fifthtable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            fifthtable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            fifthtable.HeaderRows = 1;
            fifthtable.AddCell(new Phrase("項次", ChFont));
            fifthtable.AddCell(new Phrase("提單號碼", ChFont));
            fifthtable.AddCell(new Phrase("購案號", ChFont));
            fifthtable.AddCell(new Phrase("品名", ChFont));
            fifthtable.AddCell(new Phrase("箱號", ChFont));
            fifthtable.AddCell(new Phrase("應收件數", ChFont));

            var count = 0;

            for (int i = 0 ; i <= dtd.Rows.Count - 1 ; i++)
            {
                fifthtable.AddCell(new Phrase((i + 1).ToString(), ChFont));
                fifthtable.AddCell(new Phrase(dtd.Rows[i][0].ToString(), ChFont));
                fifthtable.AddCell(new Phrase(dtd.Rows[i][1].ToString(), ChFont));
                fifthtable.AddCell(new Phrase(dtd.Rows[i][2].ToString(), ChFont));
                fifthtable.AddCell(new Phrase(dtd.Rows[i][3].ToString(), ChFont));
                fifthtable.AddCell(new Phrase(dtd.Rows[i][6].ToString(), ChFont));
                try
                {
                    count += Convert.ToInt32(dtd.Rows[i][6].ToString());
                }
                catch { }
            }
            fifthtable.AddCell(new Phrase(dtd.Rows.Count.ToString() + "項", ChFont));
            fifthtable.AddCell(new Phrase("", ChFont));
            fifthtable.AddCell(new Phrase("", ChFont));
            fifthtable.AddCell(new Phrase("", ChFont));
            fifthtable.AddCell(new Phrase("", ChFont));
            fifthtable.AddCell(new Phrase(count + "件", ChFont));
            doc1.Add(fifthtable);
            doc1.Add(new Paragraph() { SpacingBefore = 5 });

            string pageName = Server.MapPath("/");
            iTextSharp.text.Image PNG = iTextSharp.text.Image.GetInstance(Server.MapPath("/") +"/images/MTS/img.png");
            PNG.ScalePercent(25f);
            PNG.Alignment = Element.ALIGN_RIGHT;
            doc1.Add(PNG);
            doc1.Close();

            string strFileName = $"國防部國防採購室軍品運輸交接單{ IHO_NO }.pdf";
            FCommon.DownloadFile(this, strFileName, Memory);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_ONNAME, txtODT_START_DATE_S, txtODT_START_DATE_E, txtODT_ARRIVE_DATE_S, txtODT_ARRIVE_DATE_E);
                    #region 匯入下拉式選單
                    string strFirstText = "不限定";
                    CommonMTS.list_dataImport_TransferArea(drpOVC_TRANSER_DEPT_CDE, true, strFirstText); //接轉地區
                    #endregion

                    bool isImport = false;
                    if (FCommon.getQueryString(this, "OVC_IHO_NO", out string strOVC_IHO_NO, true))
                    {
                        txtOVC_IHO_NO.Text = strOVC_IHO_NO;
                        isImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_BLD_NO", out string strOVC_BLD_NO, true))
                        txtOVC_BLD_NO.Text = strOVC_BLD_NO;
                    if (FCommon.getQueryString(this, "OVC_TRANSER_DEPT_CDE", out string strOVC_TRANSER_DEPT_CDE, true))
                        FCommon.list_setValue(drpOVC_TRANSER_DEPT_CDE, strOVC_TRANSER_DEPT_CDE);
                    if (FCommon.getQueryString(this, "OVC_DEPT_CDE", out string strOVC_DEPT_CDE, true))
                    {
                        txtOVC_DEPT_CDE.Value = strOVC_DEPT_CDE;
                        TBMDEPT detp = GME.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(strOVC_DEPT_CDE)).FirstOrDefault();
                        if (detp != null) txtOVC_ONNAME.Text = detp.OVC_ONNAME;
                    }
                    if (FCommon.getQueryString(this, "ODT_START_DATE_S", out string strODT_START_DATE_S, true))
                        txtODT_START_DATE_S.Text = strODT_START_DATE_S;
                    if (FCommon.getQueryString(this, "ODT_START_DATE_E", out string strODT_START_DATE_E, true))
                        txtODT_START_DATE_E.Text = strODT_START_DATE_E;
                    if (FCommon.getQueryString(this, "ODT_ARRIVE_DATE_S", out string strODT_ARRIVE_DATE_S, true))
                        txtODT_ARRIVE_DATE_S.Text = strODT_ARRIVE_DATE_S;
                    if (FCommon.getQueryString(this, "ODT_ARRIVE_DATE_E", out string strODT_ARRIVE_DATE_E, true))
                        txtODT_ARRIVE_DATE_E.Text = strODT_ARRIVE_DATE_E;
                    if (isImport) dataImport();
                }
            }

        }

        protected void btnResetOVC_DEPT_CDE_CODE_Click(object sender, EventArgs e)
        {
            txtOVC_DEPT_CDE.Value = string.Empty;
            txtOVC_ONNAME.Text = string.Empty;
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }

        protected void GVTBGMT_IHO_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GVTBGMT_IHO.DataKeys[gvrIndex].Value.ToString();
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);

            switch (e.CommandName)
            {
                case "btnModify":
                    Response.Redirect($"MTS_A1A_2{ strQueryString }");
                    break;
                case "btnDel":
                    Response.Redirect($"MTS_A1A_3{ strQueryString }");
                    break;
                case "PrintOver":
                    string[] ary = e.CommandArgument.ToString().Split(',');
                    btnPrint(id, ary);
                    break;
            }
        }
        protected void GVTBGMT_IHO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                GridView theGridView = (GridView)sender;
                int index = gvr.RowIndex;
                //HyperLink hlkOVC_BLD_NO = (HyperLink)gvr.FindControl("hlkOVC_BLD_NO");
                HiddenField lblOVC_BLD_NO = (HiddenField)gvr.FindControl("lblOVC_BLD_NO");
                Panel pnOVC_BLD_NO = (Panel)gvr.FindControl("pnOVC_BLD_NO");
                if (FCommon.Controls_isExist(lblOVC_BLD_NO, pnOVC_BLD_NO))
                {
                    string[] strOVC_BLD_NOs = lblOVC_BLD_NO.Value.Split(',');
                    foreach (string strValue in strOVC_BLD_NOs)
                    {
                        string strOVC_BLD_NO = strValue.Trim();
                        HyperLink hlkOVC_BLD_NO = new HyperLink();
                        hlkOVC_BLD_NO.Text = strOVC_BLD_NO;
                        hlkOVC_BLD_NO.NavigateUrl = $"javascript: OpenWindow_BLDDATA('{ FCommon.getEncryption(strOVC_BLD_NO) }');";

                        if (pnOVC_BLD_NO.Controls.Count > 0) pnOVC_BLD_NO.Controls.Add(new LiteralControl(", "));
                        pnOVC_BLD_NO.Controls.Add(hlkOVC_BLD_NO);
                    }

                    //string strOVC_BLD_NO = hlkOVC_BLD_NO.Text;
                }
            }
        }
        protected void GVTBGMT_IHO_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

    }
}