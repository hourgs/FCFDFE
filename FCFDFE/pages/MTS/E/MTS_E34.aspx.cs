using System;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace FCFDFE.pages.MTS.E
{
    public partial class MTS_E34 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        private MTSEntities mtse = new MTSEntities();
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PanelS.Visible = true;
                PanelD.Visible = false;
                list_dataImport(drpOdtApplyDate);
                list_dataImport(drpOdtApplyDate2);
                btnPrint.Visible = false;
            }

        }
        public void list_dataImport(ListControl list)
        {
            //先將下拉式選單清空
            list.Items.Clear();

            int CalDateYear = Convert.ToInt16(DateTime.Now.ToString("yyyy")) - 1911;
            int num = CalDateYear;
            for (int i = num; i > 93; i--)
            {
                list.Items.Add(Convert.ToString(i));
            }
        }
        protected void GV_TBGMT_TOF_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);

        }
        private void datas()
        {
            string strODT_APPLY_DATE = drpOdtApplyDate.SelectedItem.ToString();
            if (strODT_APPLY_DATE.Length != 3)
            {
                if (strODT_APPLY_DATE.Length == 2)
                    strODT_APPLY_DATE = "0" + strODT_APPLY_DATE;
                else if (strODT_APPLY_DATE.Length == 1)
                    strODT_APPLY_DATE = "00" + strODT_APPLY_DATE;
                else
                    strODT_APPLY_DATE = "000";
            }

            DataTable dtsection = new DataTable();
            dtsection.Columns.Add(new DataColumn("OVC_SECTION", typeof(String)));
            dtsection.Rows.Add("基隆接轉組");
            dtsection.Rows.Add("桃園接轉組");
            dtsection.Rows.Add("高雄接轉組");

            var queryall = from section in dtsection.AsEnumerable()
                           join tof in mtse.TBGMT_TOF.AsEnumerable()
                           on section.Field<string>("OVC_SECTION") equals tof.OVC_SECTION
                           where tof.OVC_TOF_NO.StartsWith(strODT_APPLY_DATE)
                           group tof by section.Field<string>("OVC_SECTION") into g
                           select new
                           {
                               OVC_SECTION = g.Key,
                               ODT_AMOUNT = g.Sum(p => p.ONB_AMOUNT)
                           };
            DataTable dttof = new DataTable();
            DataRow drtof;
            dttof.Columns.Add(new DataColumn("OVC_SECTION", typeof(String)));
            dttof.Columns.Add(new DataColumn("ONB_AMOUNT", typeof(Decimal)));
            foreach (var aa in queryall)
            {
                drtof = dttof.NewRow();
                drtof[0] = aa.OVC_SECTION;
                if (aa.ODT_AMOUNT == null)
                {
                    drtof[1] = 0;
                }
                else
                {
                    drtof[1] = aa.ODT_AMOUNT;
                }
                dttof.Rows.Add(drtof);
            }

            var queryyes = from section in dtsection.AsEnumerable()
                           join tof in mtse.TBGMT_TOF.AsEnumerable()
                           on section.Field<string>("OVC_SECTION") equals tof.OVC_SECTION
                           where tof.OVC_IS_PAID == "已付款"
                           where tof.OVC_TOF_NO.StartsWith(strODT_APPLY_DATE)
                           group tof by section.Field<string>("OVC_SECTION") into g
                           select new
                           {
                               OVC_SECTION = g.Key,
                               ODT_AMOUNT = g.Sum(p => p.ONB_AMOUNT)
                           };
            DataTable dtyes = new DataTable();
            DataRow dryes;
            dtyes.Columns.Add(new DataColumn("OVC_SECTION", typeof(String)));
            dtyes.Columns.Add(new DataColumn("ONB_AMOUNT", typeof(Decimal)));
            foreach (var aa in queryyes)
            {
                dryes = dtyes.NewRow();
                dryes[0] = aa.OVC_SECTION;
                if (aa.ODT_AMOUNT == null)
                {
                    dryes[1] = 0;
                }
                else
                {
                    dryes[1] = aa.ODT_AMOUNT;
                }
                dtyes.Rows.Add(dryes);
            }

            var queryno = from section in dtsection.AsEnumerable()
                          join tof in mtse.TBGMT_TOF.AsEnumerable()
                          on section.Field<string>("OVC_SECTION") equals tof.OVC_SECTION
                          where tof.OVC_IS_PAID == "未付款"
                          where tof.OVC_TOF_NO.StartsWith(strODT_APPLY_DATE)
                          group tof by section.Field<string>("OVC_SECTION") into g
                          select new
                          {
                              OVC_SECTION = g.Key,
                              ODT_AMOUNT = g.Sum(p => p.ONB_AMOUNT)
                          };
            DataTable dtno = new DataTable();
            DataRow drno;
            dtno.Columns.Add(new DataColumn("OVC_SECTION", typeof(String)));
            dtno.Columns.Add(new DataColumn("ONB_AMOUNT", typeof(Decimal)));
            foreach (var aa in queryno)
            {
                drno = dtno.NewRow();
                drno[0] = aa.OVC_SECTION;
                if (aa.ODT_AMOUNT == null)
                {
                    drno[1] = 0;
                }
                else
                {
                    drno[1] = aa.ODT_AMOUNT;
                }
                dtno.Rows.Add(drno);
            }

            var query = from section in dtsection.AsEnumerable()
                        join tof in dttof.AsEnumerable()
                        on section.Field<string>("OVC_SECTION") equals tof.Field<string>("OVC_SECTION")
                        into tmptof
                        from tof in tmptof.DefaultIfEmpty()
                        join tofyes in dtyes.AsEnumerable()
                        on section.Field<string>("OVC_SECTION") equals tofyes.Field<string>("OVC_SECTION")
                        into tmptofyes
                        from tofyes in tmptofyes.DefaultIfEmpty()
                        join tofno in dtno.AsEnumerable()
                        on section.Field<string>("OVC_SECTION") equals tofno.Field<string>("OVC_SECTION")
                        into tmptofno
                        from tofno in tmptofno.DefaultIfEmpty()
                        select new
                        {
                            OVC_SECTION = section.Field<String>("OVC_SECTION"),
                            ONB_AMOUNT = tof?.Field<decimal>("ONB_AMOUNT") ?? Decimal.Zero,
                            ONB_AMOUNT_YES = tofyes?.Field<decimal>("ONB_AMOUNT") ?? Decimal.Zero,
                            ONB_AMOUNT_NO = tofno?.Field<decimal>("ONB_AMOUNT") ?? Decimal.Zero
                        };
            if (query.Count() > 1000)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
            query = query.Take(1000);
            DataTable dt = new DataTable();
            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_TOF, dt);


        }
        private void datad()
        {
            string strODT_APPLY_DATE = drpOdtApplyDate2.SelectedItem.ToString();
            if (strODT_APPLY_DATE.Length != 3)
            {
                if (strODT_APPLY_DATE.Length == 2)
                    strODT_APPLY_DATE = "0" + strODT_APPLY_DATE;
                else if (strODT_APPLY_DATE.Length == 1)
                    strODT_APPLY_DATE = "00" + strODT_APPLY_DATE;
                else
                    strODT_APPLY_DATE = "000";
            }
            string strSection = drpOvcSection.SelectedItem.Text;
            string strOvcPurposeType = drpOvcPurposeType.SelectedValue.ToString();
            string strOvcIsPaid = drpOvcIsPaid.SelectedItem.Text;

            ViewState["strODT_APPLY_DATE"] = strODT_APPLY_DATE;
            ViewState["drpOvcSection"] = drpOvcSection.SelectedItem.Text;
            ViewState["drpOvcPurposeType"] = drpOvcPurposeType.SelectedValue.ToString();
            ViewState["drpOvcIsPaid"] = drpOvcIsPaid.SelectedItem.Text;

            var query = from tof in mtse.TBGMT_TOF
                        where tof.OVC_TOF_NO.StartsWith(strODT_APPLY_DATE)
                        select new
                        {
                            OVC_TOF_NO = tof.OVC_TOF_NO,
                            OVC_SECTION = tof.OVC_SECTION,
                            ODT_APPLY_DATE = tof.ODT_APPLY_DATE,
                            OVC_PURPOSE_TYPE = tof.OVC_PURPOSE_TYPE,
                            OVC_ABSTRACT = tof.OVC_ABSTRACT,
                            ONB_AMOUNT = tof.ONB_AMOUNT,
                            OVC_NOTE = tof.OVC_NOTE,
                            OVC_IS_PAID = tof.OVC_IS_PAID
                        };
            if (strSection == "桃園地區")
                query = query.Where(table => table.OVC_SECTION == "桃園接轉組");
            if (strSection == "基隆地區")
                query = query.Where(table => table.OVC_SECTION == "基隆接轉組");
            if (strSection == "高雄接轉組")
                query = query.Where(table => table.OVC_SECTION == "高雄接轉組");
            if (strOvcPurposeType != "不限定")
                query = query.Where(table => table.OVC_PURPOSE_TYPE == strOvcPurposeType);
            if (strOvcIsPaid != "不限定")
                query = query.Where(table => table.OVC_IS_PAID == strOvcIsPaid);
            if (query.Count() > 1000)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
            query = query.Take(1000);
            query = query.OrderByDescending(t => t.ODT_APPLY_DATE);
            DataTable dt = new DataTable();
            dt = CommonStatic.LinqQueryToDataTable(query);

            ViewState["hasRows2"] = FCommon.GridView_dataImport(GV_TBGMT_DETAIL, dt);
            btnPrint.Visible = true;
        }

        private void print()
        {
            #region
            string strODT_APPLY_DATE = ViewState["strODT_APPLY_DATE"].ToString();
            string strSection = ViewState["drpOvcSection"].ToString();
            string strOvcPurposeType = ViewState["drpOvcPurposeType"].ToString();
            string strOvcIsPaid = ViewState["drpOvcIsPaid"].ToString();

            var query = from tof in mtse.TBGMT_TOF
                        where tof.OVC_TOF_NO.StartsWith(strODT_APPLY_DATE)
                        select new
                        {
                            OVC_TOF_NO = tof.OVC_TOF_NO,
                            OVC_SECTION = tof.OVC_SECTION,
                            ODT_APPLY_DATE = tof.ODT_APPLY_DATE,
                            OVC_PURPOSE_TYPE = tof.OVC_PURPOSE_TYPE,
                            OVC_ABSTRACT = tof.OVC_ABSTRACT,
                            ONB_AMOUNT = tof.ONB_AMOUNT,
                            OVC_NOTE = tof.OVC_NOTE,
                            OVC_IS_PAID = tof.OVC_IS_PAID
                        };
            if (strSection == "桃園地區")
                query = query.Where(table => table.OVC_SECTION == "桃園接轉組");
            if (strSection == "基隆地區")
                query = query.Where(table => table.OVC_SECTION == "基隆接轉組");
            if (strSection == "高雄接轉組")
                query = query.Where(table => table.OVC_SECTION == "高雄接轉組");
            if (strOvcPurposeType != "不限定")
                query = query.Where(table => table.OVC_PURPOSE_TYPE == strOvcPurposeType);
            if (strOvcIsPaid != "不限定")
                query = query.Where(table => table.OVC_IS_PAID == strOvcIsPaid);
            query = query.OrderByDescending(t => t.ODT_APPLY_DATE);
            #endregion


            DateTime currentTime = System.DateTime.Now;
            int y = currentTime.Year;
            int d = currentTime.Day;
            int m = currentTime.Month;
            

            string path = Request.PhysicalApplicationPath;//取得檔案絕對路徑
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChTitle = new Font(bfChinese, 16, Font.BOLD);
            Font ChTop = new Font(bfChinese, 12);
            Font ChTabTitle = new Font(bfChinese, 10);
            Font ChFont = new Font(bfChinese, 9);
            var doc1 = new Document(PageSize.A4, 0, 0, 50, 80);
            MemoryStream Memory = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            doc1.Open();

            PdfPTable table1 = new PdfPTable(8);
            table1.TotalWidth = 1200F;
            table1.SetWidths(new float[] { 4,4,4,2,6,4,4,3});
            table1.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table1.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell title = new PdfPCell(new Phrase("接轉作業費統計表\n\n", ChTitle));
            title.VerticalAlignment = Element.ALIGN_MIDDLE;
            title.HorizontalAlignment = Element.ALIGN_CENTER;
            title.Border = Rectangle.NO_BORDER;
            title.Colspan = 8;
            table1.AddCell(title);
            PdfPCell top = new PdfPCell(new Phrase("製表人:\n\n", ChTop));
            top.Border = Rectangle.NO_BORDER;
            top.VerticalAlignment = Element.ALIGN_MIDDLE;
            top.HorizontalAlignment = Element.ALIGN_LEFT;
            top.Colspan = 2;
            table1.AddCell(top);
            PdfPCell top2 = new PdfPCell(new Phrase("聯絡電話:\n\n", ChTop));
            top2.Border = Rectangle.NO_BORDER;
            top2.VerticalAlignment = Element.ALIGN_MIDDLE;
            top2.HorizontalAlignment = Element.ALIGN_LEFT;
            top2.Colspan = 2;
            table1.AddCell(top2);
            PdfPCell top3 = new PdfPCell(new Phrase("製表日期:"+y.ToString() + "年" + m.ToString() + "月" + d.ToString() + "日\n\n", ChTop));
            top3.Border = Rectangle.NO_BORDER;
            top3.VerticalAlignment = Element.ALIGN_MIDDLE;
            top3.HorizontalAlignment = Element.ALIGN_RIGHT;
            top3.Colspan = 4;
            table1.AddCell(top3);

            table1.AddCell(new Phrase("申請表編號", ChTabTitle));
            table1.AddCell(new Phrase("申請單位", ChTabTitle));
            table1.AddCell(new Phrase("申請日期", ChTabTitle));
            table1.AddCell(new Phrase("用途別", ChTabTitle));
            table1.AddCell(new Phrase("摘要", ChTabTitle));
            table1.AddCell(new Phrase("金額", ChTabTitle));
            table1.AddCell(new Phrase("備考", ChTabTitle));
            table1.AddCell(new Phrase("付款", ChTabTitle));

            foreach (var t in query)
            {
                decimal i ;
                try
                {
                    i = Convert.ToDecimal(t.ONB_AMOUNT);
                }
                catch
                {
                    i = Convert.ToDecimal(null);
                }
                string a = i.ToString("#,0");
                string day = Convert.ToDateTime(t.ODT_APPLY_DATE).ToString(Variable.strDateFormat) ?? null;
                table1.AddCell(new Phrase(t.OVC_TOF_NO, ChTabTitle));
                table1.AddCell(new Phrase(t.OVC_SECTION, ChTabTitle));
                table1.AddCell(new Phrase(day, ChTabTitle));
                table1.AddCell(new Phrase(t.OVC_PURPOSE_TYPE, ChTabTitle));
                PdfPCell tb = new PdfPCell(new Phrase(t.OVC_ABSTRACT, ChTabTitle));
                tb.HorizontalAlignment = Element.ALIGN_LEFT;
                table1.AddCell(tb);
                table1.AddCell(new Phrase(a, ChTabTitle));
                table1.AddCell(new Phrase(t.OVC_NOTE, ChTabTitle));
                table1.AddCell(new Phrase(t.OVC_IS_PAID, ChTabTitle));
            }

            doc1.Add(table1);
            doc1.Close();
            Response.Clear();//瀏覽器上顯示
            Response.AddHeader("Content-disposition", "attachment;filename=接轉作業費統計表.pdf");
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.Flush();
            Response.End();
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            datas();
        }
        protected void btnToD_Click(object sender, EventArgs e)
        {
            PanelD.Visible = true;
            PanelS.Visible = false;
        }
        protected void btnToS_Click(object sender, EventArgs e)
        {
            PanelD.Visible = false;
            PanelS.Visible = true;
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            print();
        }
        protected void btndDetailQuery_Click(object sender, EventArgs e)
        {
            datad();
        }
        protected void GV_TBGMT_DETAIL_PreRender(object sender, EventArgs e)
        {
            bool hasRows2 = false;
            if (ViewState["hasRows2"] != null)
                hasRows2 = Convert.ToBoolean(ViewState["hasRows2"]);
            FCommon.GridView_PreRenderInit(sender, hasRows2);
        }
    }
}