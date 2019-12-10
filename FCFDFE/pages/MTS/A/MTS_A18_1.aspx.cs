using System.IO;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Web.UI;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A18_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();

        //public object table { get; private set; }

        #region 副程式
        private void dataImport()
        {
            string strMessage = "";

            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_TRANSER_DEPT_CDE = drpOVC_TRANSER_DEPT_CDE.SelectedValue.ToString();
            ViewState["OVC_BLD_NO"] = strOVC_BLD_NO;
            ViewState["OVC_TRANSER_DEPT_CDE"] = strOVC_TRANSER_DEPT_CDE;

            if (strOVC_BLD_NO.Equals(string.Empty) && strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                strMessage += "<P> 至少填入一個選項 </p>";

            if (strMessage.Equals(string.Empty))
            {
                var query =
                from irdT in MTSE.TBGMT_IRD.AsEnumerable()
                join bldT in MTSE.TBGMT_BLD.AsEnumerable() on irdT.OVC_BLD_NO equals bldT.OVC_BLD_NO
                join ird_ctnT in MTSE.TBGMT_IRD_CTN on irdT.OVC_BLD_NO equals ird_ctnT.OVC_BLD_NO into ctnTemp
                from ird_ctnT in ctnTemp.GroupBy(table => table.OVC_BLD_NO).DefaultIfEmpty()
                join arrport in MTSE.TBGMT_PORTS on bldT.OVC_ARRIVE_PORT equals arrport.OVC_PORT_CDE
                where arrport.OVC_IS_ABROAD.Equals("國內")
                select new
                {
                    irdT.OVC_BLD_NO,
                    ODT_ARRIVE_PORT_DATE =FCommon.getDateTime( irdT.ODT_ARRIVE_PORT_DATE),
                    ODT_CLEAR_DATE = FCommon.getDateTime(irdT.ODT_CLEAR_DATE),
                    OVC_CONTAINER_NO = ird_ctnT != null ? string.Join(", ", ird_ctnT.Select(table => table.OVC_CONTAINER_NO)) : "",
                    irdT.ONB_ACTUAL_RECEIVE,
                    irdT.ONB_OVERFLOW,
                    irdT.ONB_LESS,
                    irdT.ONB_BROKEN,
                    bldT.OVC_ARRIVE_PORT
                };

                if (!strOVC_BLD_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
                if (!strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                {
                    var table = GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("TR"))
                        .Where(t => t.OVC_PHR_PARENTS.Equals(strOVC_TRANSER_DEPT_CDE))
                        .Select(t => t.OVC_PHR_ID).ToArray();

                    query = query.Where(t => t.OVC_ARRIVE_PORT != null && table.Contains(t.OVC_ARRIVE_PORT));
                    //query = query.Where(table => table.OVC_ARRIVE_PORT != null && table.OVC_ARRIVE_PORT.Equals(strOVC_TRANSER_DEPT_CDE));
                }
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                DataTable dtIRD = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_IRD, dtIRD);
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", ViewState["OVC_BLD_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", ViewState["OVC_TRANSER_DEPT_CDE"], true);
            return strQueryString;
        }
        private void btnPrintOverFlow(string BLD_NO, string TRANSER_DEPT_CDE)
        {
            var temp =
                from ird in MTSE.TBGMT_IRD
                join ird_ctn in MTSE.TBGMT_IRD_CTN on ird.OVC_BLD_NO equals ird_ctn.OVC_BLD_NO into p5
                from ird_ctn in p5.DefaultIfEmpty()
                select new
                {
                    ird_ctn.OVC_CONTAINER_NO,
                    ird.OVC_BLD_NO,
                };
            if (!BLD_NO.Equals(string.Empty))
                temp = temp.Where(ot => ot.OVC_BLD_NO.Contains(BLD_NO));
            DataTable dt2 = CommonStatic.LinqQueryToDataTable(temp);
            var query =
               from irdT in MTSE.TBGMT_IRD
               join bldT in MTSE.TBGMT_BLD on irdT.OVC_BLD_NO equals bldT.OVC_BLD_NO into ps1
               from o1 in ps1.DefaultIfEmpty()
               join ird_detallT in MTSE.TBGMT_IRD_DETAIL on irdT.OVC_BLD_NO equals ird_detallT.OVC_BLD_NO into ps2
               from o2 in ps2.DefaultIfEmpty()
               join icrT in MTSE.TBGMT_ICR on irdT.OVC_BLD_NO equals icrT.OVC_BLD_NO into ps3
               from o3 in ps3.DefaultIfEmpty()
               join dept_cdeT in MTSE.TBGMT_DEPT_CDE on o2.OVC_DEPT_CDE equals dept_cdeT.OVC_DEPT_CODE into ps4
               from o4 in ps4.DefaultIfEmpty()
               select new
               {
                    OVC_BLD_NO = irdT.OVC_BLD_NO, //提單編號
                    o2.ONB_OVERFLOW,//溢卸
                    o2.ONB_LESS,//短少
                    o2.ONB_BROKEN,//破損
                    irdT.OVC_CONTAINER_NO,//貨櫃號碼
                    o1.OVC_SHIP_NAME,//船名
                    o1.OVC_VOYAGE,//航次
                    o2.OVC_BOX_NO,//箱號
                    irdT.OVC_NOTE,//備考
                    //o3.OVC_NOTE,//備考
                    o3.OVC_PURCH_NO,//購案號
                    irdT.ODT_ARRIVE_PORT_DATE,//進口日期
                    irdT.ODT_CLEAR_DATE,//拆櫃日期
                    //o3.ODT_IMPORT_DATE,//進口日期
                    //o3.ODT_UNPACKING_DATE,//拆櫃日期
                    o4.OVC_DEPT_NAME,//分運單位
                    OVC_ARRIVE_PORT = o1.OVC_ARRIVE_PORT,
               };

            

            if (!BLD_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_BLD_NO.Contains(BLD_NO));
            if (!TRANSER_DEPT_CDE.Equals(string.Empty))
            {
                var table = GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("TR"))
                    .Where(t => t.OVC_PHR_PARENTS.Equals(TRANSER_DEPT_CDE))
                    .Select(t => t.OVC_PHR_ID).ToArray();
            
                query = query.Where(t => t.OVC_ARRIVE_PORT != null && table.Contains(t.OVC_ARRIVE_PORT));
            }
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);

            // 處理箱號
            string strOVC_CONTAINER_NO = "";
            if (dt2.Rows.Count > 0)
            {
                strOVC_CONTAINER_NO = dt2.Rows[0][0].ToString();
                for (var i = 1; i < dt2.Rows.Count; i++)
                {
                    strOVC_CONTAINER_NO += "," + dt2.Rows[i][0].ToString();
                }
            }


            ViewState["dt"] = dt;
            DataTable print_query = (DataTable)ViewState["dt"];

            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\mingliu.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChFont = new Font(bfChinese, 12f, Font.BOLD);
            Font Title_ChFont = new Font(bfChinese, 18f, Font.BOLD);
            MemoryStream Memory = new MemoryStream();
            var doc1 = new Document(PageSize.A4, 50, 50, 80, 50);
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            doc1.Open();

            PdfPTable Firsttable = new PdfPTable(2);
            Firsttable.SetWidths(new float[] { 1, 6 });
            Firsttable.TotalWidth = 500F;
            Firsttable.LockedWidth = true;
            Firsttable.DefaultCell.FixedHeight = 40f;
            Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell Title_Text = new PdfPCell(new Phrase("國防部國防採購室接收軍品（溢、短、損）登記表", Title_ChFont));
            Title_Text.Colspan = 2;
            Title_Text.FixedHeight = 40f;
            Title_Text.HorizontalAlignment = Element.ALIGN_CENTER;
            Title_Text.VerticalAlignment = Element.ALIGN_MIDDLE;
            Firsttable.AddCell(Title_Text);
            Firsttable.AddCell(new Phrase("提單編號", ChFont));
            Firsttable.AddCell(new Phrase(BLD_NO, ChFont));
            Firsttable.AddCell(new Phrase("船    名", ChFont));
            Firsttable.AddCell(new Phrase(dt.Rows[0][5].ToString(), ChFont));
            Firsttable.AddCell(new Phrase("航    次", ChFont));
            Firsttable.AddCell(new Phrase(dt.Rows[0][6].ToString(), ChFont));
            Firsttable.AddCell(new Phrase("案    號", ChFont));
            Firsttable.AddCell(new Phrase(dt.Rows[0][9].ToString(), ChFont)); //.Replace(" ", "、")
            Firsttable.AddCell(new Phrase("貨櫃號碼", ChFont));
            Firsttable.AddCell(new Phrase(strOVC_CONTAINER_NO, ChFont));
            Firsttable.AddCell(new Phrase("進口日期", ChFont));
            string msg="";
            DateTime dateImporttime , dateGettime;
            bool boolImporttime = FCommon.checkDateTime(dt.Rows[0][10].ToString(), "進口日期", ref msg, out dateImporttime);
            bool boolGettime = FCommon.checkDateTime(dt.Rows[0][11].ToString(), "拆櫃日期", ref msg, out dateGettime);
            if (boolImporttime)
                Firsttable.AddCell(new Phrase(FCommon.getDateTime(dt.Rows[0][10]), ChFont));
            Firsttable.AddCell(new Phrase("拆櫃日期", ChFont));
            if (boolGettime)
                Firsttable.AddCell(new Phrase(FCommon.getDateTime(dt.Rows[0][11]), ChFont));
            Firsttable.DefaultCell.FixedHeight = 80f;
            Firsttable.AddCell(new Phrase("備    考", ChFont));
            Firsttable.AddCell(new Phrase(dt.Rows[0][8].ToString(), ChFont));

            doc1.Add(Firsttable);

            PdfPTable FinalTable = new PdfPTable(5);
            FinalTable.SetWidths(new float[] { 3, 2, 1, 1, 1 });
            FinalTable.TotalWidth = 500F;
            FinalTable.LockedWidth = true;
            FinalTable.DefaultCell.FixedHeight = 40f;
            FinalTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            FinalTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            FinalTable.AddCell(new Phrase("分  運  單  位", ChFont));
            FinalTable.AddCell(new Phrase("箱  號", ChFont));
            FinalTable.AddCell(new Phrase("溢  卸", ChFont));
            FinalTable.AddCell(new Phrase("短  少", ChFont));
            FinalTable.AddCell(new Phrase("破損", ChFont));
            int Unloading_TNumber = 0, Lack__TNumber = 0, Damaged__TNumber = 0 ,TNumber=0;

            
            for (int i = 0; i < print_query.Rows.Count; i++)
            {
                TNumber += 1;
                Unloading_TNumber += Convert.ToInt16(dt.Rows[i][1]);
                Lack__TNumber += Convert.ToInt16(dt.Rows[i][2]);
                Damaged__TNumber += Convert.ToInt16(dt.Rows[i][3]);
                FinalTable.AddCell(new Phrase(string.Format("{0:#,##0}", dt.Rows[i][12]), ChFont));
                FinalTable.AddCell(new Phrase(string.Format("{0:#,##0}", dt.Rows[i][7]), ChFont));
                FinalTable.AddCell(new Phrase(string.Format("{0:#,##0}", dt.Rows[i][1]), ChFont));
                FinalTable.AddCell(new Phrase(string.Format("{0:#,##0}", dt.Rows[i][2]), ChFont));
                FinalTable.AddCell(new Phrase(string.Format("{0:#,##0}", dt.Rows[i][3]), ChFont));
            }

            FinalTable.AddCell(new Phrase("總  計", ChFont));
            FinalTable.AddCell(new Phrase(TNumber.ToString("#,##0", System.Globalization.CultureInfo.InvariantCulture), ChFont));
            FinalTable.AddCell(new Phrase(Unloading_TNumber.ToString("#,##0", System.Globalization.CultureInfo.InvariantCulture), ChFont));
            FinalTable.AddCell(new Phrase(Lack__TNumber.ToString("#,##0", System.Globalization.CultureInfo.InvariantCulture), ChFont));
            FinalTable.AddCell(new Phrase(Damaged__TNumber.ToString("#,##0", System.Globalization.CultureInfo.InvariantCulture), ChFont));
            
            PdfPCell Supervisor = new PdfPCell(new Phrase("主  管：", ChFont));
            Supervisor.Border = Rectangle.NO_BORDER;
            Supervisor.VerticalAlignment = Element.ALIGN_MIDDLE;
            Supervisor.FixedHeight = 40f;
            FinalTable.AddCell(Supervisor);
            PdfPCell Undertaker = new PdfPCell(new Phrase("承 辦 人:", ChFont));
            Undertaker.Colspan = 3;
            Undertaker.Border = Rectangle.NO_BORDER;
            Undertaker.HorizontalAlignment = Element.ALIGN_CENTER;
            Undertaker.VerticalAlignment = Element.ALIGN_MIDDLE;
            Undertaker.FixedHeight = 40f;
            FinalTable.AddCell(Undertaker);
            PdfPCell Space_Text = new PdfPCell(new Phrase(" ", ChFont));
            Space_Text.Border = Rectangle.NO_BORDER;
            FinalTable.AddCell(Space_Text);
            doc1.Add(FinalTable);
            
            doc1.Close();

            string strFileName = $"國防部國防採購室接收軍品（溢、短、損）登記表-{ BLD_NO }.pdf";
            FCommon.DownloadFile(this, strFileName, Memory);
        }
        private void btnPrint(string BLD_NO , string TRANSER_DEPT_CDE )
        {
           //start
            DataTable dt = new DataTable();
            var query =
                from irdT in MTSE.TBGMT_IRD
                join bldT in MTSE.TBGMT_BLD on irdT.OVC_BLD_NO equals bldT.OVC_BLD_NO into ps1
                from o1 in ps1.DefaultIfEmpty()
                join ird_ctn in MTSE.TBGMT_IRD_CTN on irdT.OVC_BLD_NO equals ird_ctn.OVC_BLD_NO into p5
                from ird_ctn in p5.DefaultIfEmpty()
                join icrT in MTSE.TBGMT_ICR on irdT.OVC_BLD_NO equals icrT.OVC_BLD_NO into ps3
                from o3 in ps3.DefaultIfEmpty()
                select new
                {
                    OVC_BLD_NO = irdT.OVC_BLD_NO, //提單編號
                    irdT.ONB_ACTUAL_RECEIVE,//時收件數
                    irdT.ONB_OVERFLOW,//溢卸
                    irdT.ONB_LESS,//短少
                    irdT.ONB_BROKEN,//破損
                    ird_ctn.OVC_CONTAINER_NO,//貨櫃號碼
                    o1.OVC_SHIP_NAME,//船名
                    o1.ONB_VOLUME,//體機
                    o1.ONB_WEIGHT,//重量
                    o1.ONB_QUANITY,//應收數量
                    o1.OVC_VOYAGE,//航次
                    irdT.OVC_NOTE,//備考
                    //o3.OVC_NOTE,//備考
                    o3.OVC_PURCH_NO,//購案號
                    irdT.ODT_ARRIVE_PORT_DATE,//進口日期
                    irdT.ODT_CLEAR_DATE,//拆櫃日期
                    //o3.ODT_IMPORT_DATE,//進口日期
                    //o3.ODT_UNPACKING_DATE,//拆櫃日期
                    OVC_ARRIVE_PORT = o1.OVC_ARRIVE_PORT,//體機單位
                    o1.OVC_VOLUME_UNIT,
                };
            if (!BLD_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_BLD_NO.Equals(BLD_NO));
            if (!TRANSER_DEPT_CDE.Equals(string.Empty))
            {
                var table = GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("TR"))
                    .Where(t => t.OVC_PHR_PARENTS.Equals(TRANSER_DEPT_CDE))
                    .Select(t => t.OVC_PHR_ID).ToArray();

                query = query.Where(t => t.OVC_ARRIVE_PORT != null && table.Contains(t.OVC_ARRIVE_PORT));
            }
            
            dt = CommonStatic.LinqQueryToDataTable(query);
            double unit = 0;
            string depro = "";
            if (dt.Rows[0][16].Equals("CBM"))
            {
                unit = 1000;
            }
            else if (dt.Rows[0][16].Equals("CF"))
            {
                unit = 28.3168;
            }
            else
            {
               depro = "(未輸入單位)";
            }

            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\mingliu.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChFont = new Font(bfChinese, 12f, Font.BOLD );
            Font Title_ChFont = new Font(bfChinese, 18f, Font.BOLD);
            MemoryStream Memory = new MemoryStream();
            var doc1 = new Document(PageSize.A4, 50, 50, 80, 50);
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);
           // PdfWriter pdfWriter = PdfWriter.GetInstance(doc1, new FileStream(@"C:\Users\User\Downloads\國防部軍備局規格鑑測中心進口物資管制接配紀錄表"+ BLD_NO + ".pdf", FileMode.Create)); //指定路徑創造

            doc1.Open();
            PdfPTable Firsttable = new PdfPTable(4);
            Firsttable.SetWidths(new float[] { 1, 9, 2, 6 });
            Firsttable.TotalWidth = 500F;
            Firsttable.LockedWidth = true;
            Firsttable.DefaultCell.FixedHeight = 40f;
            Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell Title_Text = new PdfPCell(new Phrase("國防部軍備局規格鑑測中心進口物資管制接配紀錄表", Title_ChFont));
            Title_Text.Colspan = 4;
            Title_Text.HorizontalAlignment = Element.ALIGN_CENTER;
            Title_Text.Border = Rectangle.NO_BORDER;
            Firsttable.AddCell(Title_Text);
            PdfPCell B_L_Number = new PdfPCell(new Phrase("提單編號：" + dt.Rows[0][0].ToString(), ChFont));
            B_L_Number.Colspan = 4;
            B_L_Number.HorizontalAlignment = Element.ALIGN_RIGHT;
            B_L_Number.Border = Rectangle.NO_BORDER;
            Firsttable.AddCell(B_L_Number);
            Firsttable.AddCell(new Phrase("船\n\n名", ChFont));
            Firsttable.AddCell(new Phrase(dt.Rows[0][6].ToString(), ChFont));
            Firsttable.AddCell(new Phrase("航次", ChFont));
            Firsttable.AddCell(new Phrase(dt.Rows[0][10].ToString(), ChFont));
            PdfPCell Case_Number = new PdfPCell(new Phrase("購\n\n案\n\n號", ChFont));
            Case_Number.Rowspan = 3;
            Case_Number.VerticalAlignment = Element.ALIGN_MIDDLE;
            Case_Number.HorizontalAlignment = Element.ALIGN_CENTER;
            Firsttable.AddCell(Case_Number);
            PdfPCell Case_Number_Value = new PdfPCell(new Phrase(dt.Rows[0][12].ToString(), ChFont));
            Case_Number_Value.Rowspan = 3;
            Case_Number_Value.VerticalAlignment = Element.ALIGN_MIDDLE;
            Case_Number_Value.HorizontalAlignment = Element.ALIGN_CENTER;
            Firsttable.AddCell(Case_Number_Value);
            Firsttable.AddCell(new Phrase("應收數量", ChFont));
            Firsttable.AddCell(new Phrase(string.Format("{0:#,##0}", dt.Rows[0][9]), ChFont));
            Firsttable.AddCell(new Phrase("重量頓", ChFont));
            Firsttable.AddCell(new Phrase(string.Format("{0:#,##0}", dt.Rows[0][8]) , ChFont));
            Firsttable.AddCell(new Phrase("體積頓", ChFont));
            string msg = "";
            string strWeight = dt.Rows[0][7].ToString();
            decimal decWeight;

            bool boolWight = FCommon.checkDecimal(strWeight, "體積頓", ref msg, out decWeight);
            if (boolWight)
                Firsttable.AddCell(new Phrase(string.Format("{0:#,##0.00##}", (Convert.ToDouble(dt.Rows[0][7])*unit)) + depro, ChFont));

            doc1.Add(Firsttable);

            PdfPTable SecondTable = new PdfPTable(2);
            SecondTable.SetWidths(new float[] { 3, 13 });
            SecondTable.TotalWidth = 500F;
            SecondTable.LockedWidth = true;
            SecondTable.DefaultCell.FixedHeight = 40f;
            SecondTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            SecondTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            SecondTable.AddCell(new Phrase("貨櫃號碼", ChFont));
            SecondTable.AddCell(new Phrase(dt.Rows[0][5].ToString(), ChFont));

            string strImporttime = dt.Rows[0][13].ToString();
            string strGettime = dt.Rows[0][14].ToString();
            DateTime dateImportime, dateGettime;

            bool boolImportime = FCommon.checkDateTime(strImporttime, "進口日期", ref msg, out dateImportime);
            bool boolGettime = FCommon.checkDateTime(strImporttime, "進口日期", ref msg, out dateGettime);

            SecondTable.AddCell(new Phrase("進口日期", ChFont));
            if (boolImportime)
                SecondTable.AddCell(new Phrase(FCommon.getDateTime(dt.Rows[0][13]), ChFont));
            SecondTable.AddCell(new Phrase("提領/拆櫃日期", ChFont));
            if (boolGettime)
                SecondTable.AddCell(new Phrase(FCommon.getDateTime(dt.Rows[0][14]), ChFont));

            doc1.Add(SecondTable);

            PdfPTable FinalTable = new PdfPTable(3);
            FinalTable.SetWidths(new float[] { 1, 2, 11 });
            FinalTable.TotalWidth = 500F;
            FinalTable.LockedWidth = true;
            FinalTable.DefaultCell.FixedHeight = 40f;
            FinalTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            FinalTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell Clearance_Status = new PdfPCell(new Phrase("清\n\n檢\n\n狀\n\n況", ChFont));
            Clearance_Status.Rowspan = 4;
            Clearance_Status.VerticalAlignment = Element.ALIGN_MIDDLE;
            Clearance_Status.HorizontalAlignment = Element.ALIGN_CENTER;
            FinalTable.AddCell(Clearance_Status);
            FinalTable.AddCell(new Phrase("實收件數", ChFont));
            FinalTable.AddCell(new Phrase(string.Format("{0:#,##0}", dt.Rows[0][1]), ChFont));
            FinalTable.AddCell(new Phrase("溢卸", ChFont));
            FinalTable.AddCell(new Phrase(string.Format("{0:#,##0}", dt.Rows[0][2]), ChFont));
            FinalTable.AddCell(new Phrase("短少", ChFont));
            FinalTable.AddCell(new Phrase(string.Format("{0:#,##0}", dt.Rows[0][3]), ChFont));
            FinalTable.AddCell(new Phrase("破損", ChFont));
            FinalTable.AddCell(new Phrase(string.Format("{0:#,##0}", dt.Rows[0][4]), ChFont));
            FinalTable.DefaultCell.FixedHeight = 80f;
            FinalTable.AddCell(new Phrase("備\n\n考", ChFont));
            PdfPCell Remarks_Value = new PdfPCell(new Phrase(dt.Rows[0][11].ToString(), ChFont));
            Remarks_Value.VerticalAlignment = Element.ALIGN_MIDDLE;
            Remarks_Value.HorizontalAlignment = Element.ALIGN_CENTER;
            Remarks_Value.Colspan = 2;
            FinalTable.AddCell(Remarks_Value);

            doc1.Add(FinalTable);

            PdfPTable TailTable = new PdfPTable(4);
            TailTable.SetWidths(new float[] { 1, 6, 1, 6 });
            TailTable.TotalWidth = 500F;
            TailTable.LockedWidth = true;
            TailTable.DefaultCell.FixedHeight = 80f;
            TailTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            TailTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            TailTable.AddCell(new Phrase("承\n\n辦\n\n人", ChFont));
            TailTable.AddCell(new Phrase(" ", ChFont));
            TailTable.AddCell(new Phrase("主\n\n\n管", ChFont));
            TailTable.AddCell(new Phrase(" ", ChFont));

            doc1.Add(TailTable);
            doc1.Close();
            string strFileName = $"國防部軍備局規格鑑測中心進口物資管制接配紀錄表-{ BLD_NO }.pdf";
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
                    #region 匯入下拉式選單
                    string strFirstText = "不限定";
                    CommonMTS.list_dataImport_TransferArea(drpOVC_TRANSER_DEPT_CDE, true, strFirstText); //接轉地區
                    #endregion

                    bool boolImport = false;
                    string strOVC_BLD_NO, strOVC_TRANSER_DEPT_CDE;
                    if (FCommon.getQueryString(this, "OVC_BLD_NO", out strOVC_BLD_NO, true))
                    {
                        txtOVC_BLD_NO.Text = strOVC_BLD_NO;
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_TRANSER_DEPT_CDE", out strOVC_TRANSER_DEPT_CDE, true))
                        FCommon.list_setValue(drpOVC_TRANSER_DEPT_CDE, strOVC_TRANSER_DEPT_CDE);
                    if (boolImport) dataImport();
                }
            }
           
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }
        
        protected void GVTBGMT_IRD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = theGridView.DataKeys[gvrIndex].Value.ToString();

            string strOVC_TRANSER_DEPT_CDE = ViewState["OVC_TRANSER_DEPT_CDE"].ToString();
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);

            switch (e.CommandName)
            {
                case "DataEdit":
                    Response.Redirect($"MTS_A18_2{ strQueryString }");
                    break;
                case "DataDelete":
                    Response.Redirect($"MTS_A18_3{ strQueryString }");
                    break;
                case "Print":
                    btnPrint(id, strOVC_TRANSER_DEPT_CDE);
                    break;
                case "PrintOver":
                    btnPrintOverFlow(id, strOVC_TRANSER_DEPT_CDE);
                    break;
            }
        }
        protected void GVTBGMT_IRD_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                GridView theGridView = (GridView)sender;
                int index = gvr.RowIndex;
                HyperLink hlkOVC_BLD_NO = (HyperLink)gvr.FindControl("hlkOVC_BLD_NO");
                if (FCommon.Controls_isExist(hlkOVC_BLD_NO))
                {
                    string strOVC_BLD_NO = hlkOVC_BLD_NO.Text;
                    hlkOVC_BLD_NO.NavigateUrl = $"javascript: OpenWindow_BLDDATA('{ FCommon.getEncryption(strOVC_BLD_NO) }');";
                }
            }
        }
        protected void GVTBGMT_IRD_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}