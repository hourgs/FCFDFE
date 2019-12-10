using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace FCFDFE.pages.MTS.G
{
    public class Header : PdfPageEventHelper
    {
        PdfContentByte cb;
        PdfTemplate template;
        BaseFont bf = null;
        DateTime PrintTime = DateTime.Now.AddYears(-1911);
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                template = cb.CreateTemplate(80, 80);
            }
            catch
            {

            }
        }
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            int pageN = writer.PageNumber;
            String text = "第 " + pageN + " 頁/ ";
            float len = bf.GetWidthPoint(text, 8);
            Rectangle pageSize = document.PageSize;
            cb.BeginText();
            cb.SetFontAndSize(bf, 12);
            cb.SetTextMatrix(pageSize.GetRight(110) , pageSize.GetTop(70));
            cb.ShowText(text);
            cb.EndText();
            cb.AddTemplate(template, pageSize.GetRight(100) +len , pageSize.GetTop(70));

            cb.BeginText();
            cb.SetFontAndSize(bf, 14);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "國防部國防採購室進口運案表", pageSize.GetLeft(290), pageSize.GetTop(60), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 11);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "製表日期：" + PrintTime.ToString("yy/MM/dd"), pageSize.GetRight(10), pageSize.GetTop(50), 0);
            cb.EndText();
            //cb.BeginText();
            //cb.SetFontAndSize(bf, 12);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "製表日期：" + PrintTime.ToString("yy/MM/dd"), pageSize.GetRight(10), pageSize.GetTop(65), 0);
            //cb.EndText();
        }
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            template.BeginText();
            template.SetFontAndSize(bf, 12);
            template.SetTextMatrix(0, 0);
            template.ShowText("共 " + (writer.PageNumber) + " 頁");
            template.EndText();
        }
    }       

    public partial class MTS_G11 : System.Web.UI.Page
    {
 
        public string strMenuName = "", strMenuNameItem = "";
        private MTSEntities MTS = new MTSEntities();
        Common FCommon = new Common();
        string[] strField = { "OVC_CLASS_NAME", "OVC_SEA_OR_AIR", "OVC_SHIP_COMPANY", "OVC_BLD_NO", "OVC_PURCH_NO", "OVC_START_PORT", "OVC_ARRIVE_PORT", "ODT_CUSTOM_DATE", "ODT_IMPORT_DATE", "ODT_PASS_CUSTOM_DATE", "ODT_TRANSFER_DATE", "ODT_RECEIVE_DATE" };
        string[] strFieldExport = { "OVC_CLASS_NAME", "OVC_SEA_OR_AIR", "OVC_SHIP_COMPANY", "OVC_EDF_NO", "OVC_BLD_NO", "OVC_PURCH_NO", "OVC_START_PORT", "OVC_ARRIVE_PORT", "ODT_REQUIRE_DATE", "ODT_PROCESS_DATE", "ODT_STORED_DATE", "ODT_EXP_DATE", "ODT_START_DATE" };
        string strImagePagh = Content.Variable.strImagePath;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (rdoOVC_IMP_OR_EXP.SelectedItem.Text.Equals("進口"))
                {
                    lbdate.Text = "進口日期";
                    GV_TBGMT_BLD.Visible = true;
                    GV_TBGMT_BLD_export.Visible = false;
                    export_row.Visible = false;
                }

                if (rdoOVC_IMP_OR_EXP.SelectedItem.Text.Equals("出口"))
                {
                    lbdate.Text = "委運單位函文日期";
                    GV_TBGMT_BLD.Visible = false;
                    GV_TBGMT_BLD_export.Visible = true;
                    export_row.Visible = true;
                }

                if (!IsPostBack)
                {
                    FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                    FCommon.Controls_Attributes("readonly", "true", txtODT_IMPORT_DATE_S, txtODT_IMPORT_DATE_E);
                    DataTable DEPT_CLASS = CommonStatic.ListToDataTable(MTS.TBGMT_DEPT_CLASS.DefaultIfEmpty().ToList());
                    FCommon.list_dataImport(drpOVC_MILITARY_TYPE, DEPT_CLASS, "OVC_CLASS_NAME", "OVC_CLASS", true);
                    txtODT_IMPORT_DATE_S.Text = DateTime.Now.ToString("yyyy/MM/dd");
                    txtODT_IMPORT_DATE_E.Text = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");

                }
            }
        }


        protected void GV_TBGMT_BLD_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        protected void GV_TBGMT_BLD2_PreRender(object sender, EventArgs e)
        {
            bool hasRows2 = false;
            if (ViewState["hasRows2"] != null)
                hasRows2 = Convert.ToBoolean(ViewState["hasRows2"]);
            FCommon.GridView_PreRenderInit(sender, hasRows2);
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (rdoOVC_IMP_OR_EXP.SelectedItem.Text.Equals("進口"))
            {
                import_dataimport();
            }
            if (rdoOVC_IMP_OR_EXP.SelectedItem.Text.Equals("出口"))
            {
                export_dataimport();
            }
        }

        protected void import_dataimport()
        {



            DataTable dt = new DataTable();

            string strODT_IMPORT_DATE_S = txtODT_IMPORT_DATE_S.Text;
            string strODT_IMPORT_DATE_E = txtODT_IMPORT_DATE_E.Text;
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedItem.Value;
            string strOVC_SEA_OR_AIR = drpOVC_SEA_OR_AIR.SelectedItem.Text;
            string strOVC_SHIP_COMPANY = drpOVC_SHIP_COMPANY.SelectedItem.Text;
            string strOVC_DEPT_NAME = drpOVC_TRANSER_DEPT_CDE.SelectedItem.Text;
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_PURCH_NO = txtOVC_PURCH_NO.Text;
            int strOVC_IS_SECURITY = Convert.ToInt16(DropSmartUint.SelectedItem.Value);

            var query =
                from bld in MTS.TBGMT_BLD
                join icr in MTS.TBGMT_ICR on bld.OVC_BLD_NO equals icr.OVC_BLD_NO into p1
                from icr in p1.DefaultIfEmpty()
                join port in MTS.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals port.OVC_PORT_CDE into p2
                from port in p2.DefaultIfEmpty()
                join port2 in MTS.TBGMT_PORTS on bld.OVC_START_PORT equals port2.OVC_PORT_CDE into p3
                from port2 in p3.DefaultIfEmpty()
                join dept in MTS.TBGMT_DEPT_CLASS on bld.OVC_MILITARY_TYPE equals dept.OVC_CLASS into p4
                from dept in p4.DefaultIfEmpty()
                select new
                {
                    OVC_CLASS_NAME = dept.OVC_CLASS_NAME,
                    OVC_MILITARY_TYPE = bld.OVC_MILITARY_TYPE, //軍種
                    OVC_SEA_OR_AIR = bld.OVC_SEA_OR_AIR,       //海空運
                    OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY,   //承運航商
                    OVC_BLD_NO = bld.OVC_BLD_NO,               //提單編號
                    OVC_PURCH_NO = icr.OVC_PURCH_NO,           //案號
                    OVC_START_PORT = port2.OVC_PORT_CHI_NAME,
                    OVC_ARRIVE_PORT = port.OVC_PORT_CHI_NAME,
                    ODT_CUSTOM_DATE = icr.ODT_CUSTOM_DATE,
                    ODT_IMPORT_DATE = icr.ODT_IMPORT_DATE,
                    ODT_PASS_CUSTOM_DATE = icr.ODT_PASS_CUSTOM_DATE,
                    ODT_TRANSFER_DATE = icr.ODT_TRANSFER_DATE,
                    ODT_RECEIVE_DATE = icr.ODT_RECEIVE_DATE,
                    OVC_IS_SECURITY = bld.OVC_IS_SECURITY,
                    OVC_IS_ABROAD = port.OVC_IS_ABROAD,
                    OVC_PORT_CDE=port2.OVC_PORT_CDE,
                };
            if (!rdoODT_IMPORT_DATE.SelectedItem.Text.Equals("當日作業"))
            {
                DateTime start = Convert.ToDateTime(strODT_IMPORT_DATE_S);
                DateTime end = Convert.ToDateTime(strODT_IMPORT_DATE_E);
                query = query.Where(table => table.ODT_IMPORT_DATE >= start && table.ODT_IMPORT_DATE < end);
            }
            else
            {
                DateTime now = DateTime.Now;
                query = query.Where(table => table.ODT_IMPORT_DATE == now);
            }

            if (!strOVC_MILITARY_TYPE.Equals(string.Empty))
                query = query.Where(table => table.OVC_MILITARY_TYPE.Equals(strOVC_MILITARY_TYPE));
            if (!strOVC_SEA_OR_AIR.Equals("不限定"))
                query = query.Where(table => table.OVC_SEA_OR_AIR.Equals(strOVC_SEA_OR_AIR));
            if (!strOVC_SHIP_COMPANY.Equals("不限定"))
                query = query.Where(table => table.OVC_SHIP_COMPANY.Equals(strOVC_SHIP_COMPANY));
            if (!strOVC_DEPT_NAME.Equals("不限定"))
            {
                switch (strOVC_DEPT_NAME)
                {
                    case "基隆地區":
                        query = query.Where(table => table.OVC_PORT_CDE.Equals("TWKEL"));
                        break;
                    case "桃園地區":
                        query = query.Where(table => table.OVC_PORT_CDE.Equals("TPE") || table.OVC_PORT_CDE.Equals("TTY"));
                        break;
                    case "高雄分遣組":
                        query = query.Where(table => table.OVC_PORT_CDE.Equals("TWKHH") || table.OVC_PORT_CDE.Equals("KHH"));
                        break;
                    default:
                        break;
                }
            }
            if (!strOVC_BLD_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
            if (!strOVC_PURCH_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_PURCH_NO.Contains(strOVC_PURCH_NO));
            if (strOVC_IS_SECURITY.Equals(1))
                query = query.Where(table => table.OVC_IS_SECURITY == strOVC_IS_SECURITY);
            else
                query = query.Where(table => table.OVC_IS_SECURITY == strOVC_IS_SECURITY || table.OVC_IS_SECURITY == null);
            //query = query.Where(table => !table.OVC_IS_ABROAD.Equals("國內"));
            if (query.Count() > 1000)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
            query = query.Take(1000);
            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_BLD, dt, strField);

            //儲存BLD
            if (dt.Rows.Count > 0)
            {
                string bld_no_list = dt.Rows[0][4].ToString();
                for (var a = 1; a < dt.Rows.Count; a++)
                {
                    bld_no_list += "," + dt.Rows[a][4].ToString();
                  
                }
                Session["bld_no_list"] = bld_no_list;
            }
            if (GV_TBGMT_BLD.Rows.Count > 0)
            {
                btnPrint.Visible = true;
                btnPrintExport.Visible = false;
            }
            else
            {
                btnPrint.Visible = false;
                btnPrintExport.Visible = false;
            }
        }
        protected void export_dataimport()
        {


            DataTable dt = new DataTable();

            string strODT_IMPORT_DATE_S = txtODT_IMPORT_DATE_S.Text;
            string strODT_IMPORT_DATE_E = txtODT_IMPORT_DATE_E.Text;
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedItem.Value;
            string strOVC_SEA_OR_AIR = drpOVC_SEA_OR_AIR.SelectedItem.Text;
            string strOVC_SHIP_COMPANY = drpOVC_SHIP_COMPANY.SelectedItem.Text;
            string strOVC_DEPT_NAME = drpOVC_TRANSER_DEPT_CDE.SelectedItem.Text;
            string strOVC_EDF_NO = txtOVC_EDF_NO.Text;
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_PURCH_NO = txtOVC_PURCH_NO.Text;
            int strOVC_IS_SECURITY = Convert.ToInt16(DropSmartUint.SelectedItem.Value);

            var query =
                from bld in MTS.TBGMT_BLD
                join ecl in MTS.TBGMT_ECL on bld.OVC_BLD_NO equals ecl.OVC_BLD_NO into p1
                from ecl in p1.DefaultIfEmpty()
                join port in MTS.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals port.OVC_PORT_CDE into p2
                from port in p2.DefaultIfEmpty()
                join port2 in MTS.TBGMT_PORTS on bld.OVC_START_PORT equals port2.OVC_PORT_CDE into p3
                from port2 in p3.DefaultIfEmpty()
                join dept in MTS.TBGMT_DEPT_CLASS on bld.OVC_MILITARY_TYPE equals dept.OVC_CLASS into p4
                from dept in p4.DefaultIfEmpty()
                join eso in MTS.TBGMT_ESO on bld.OVC_BLD_NO equals eso.OVC_BLD_NO into p5
                from eso in p5.DefaultIfEmpty()
                join edf in MTS.TBGMT_EDF on eso.OVC_EDF_NO equals edf.OVC_EDF_NO into p6
                from edf in p6.DefaultIfEmpty()
                join etr in MTS.TBGMT_ETR on bld.OVC_BLD_NO equals etr.OVC_BLD_NO into p7
                from etr in p7.DefaultIfEmpty()
                select new
                {
                    OVC_CLASS_NAME = dept.OVC_CLASS_NAME,
                    OVC_MILITARY_TYPE = bld.OVC_MILITARY_TYPE, //軍種
                    OVC_SEA_OR_AIR = bld.OVC_SEA_OR_AIR,       //海空運
                    OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY,   //承運航商
                    OVC_EDF_NO = edf.OVC_EDF_NO,               //外運資料表編號
                    OVC_BLD_NO = bld.OVC_BLD_NO,               //提單編號
                    OVC_PURCH_NO = edf.OVC_PURCH_NO,             //案號
                    OVC_START_PORT = port2.OVC_PORT_CHI_NAME,  //啟運
                    OVC_ARRIVE_PORT = port.OVC_PORT_CHI_NAME,  //抵運
                    ODT_REQUIRE_DATE = etr.ODT_REQUIRE_DATE,   //委運日期
                    ODT_PROCESS_DATE = etr.ODT_PROCESS_DATE,   //中文函文免稅日期
                    ODT_STORED_DATE = eso.ODT_STORED_DATE,     //進艙日期
                    ODT_EXP_DATE = ecl.ODT_EXP_DATE,           //報關日期
                    ODT_START_DATE = bld.ODT_START_DATE,       //啟運日期
                    OVC_IS_SECURITY = bld.OVC_IS_SECURITY,
                    OVC_IS_ABROAD = port.OVC_IS_ABROAD,
                    OVC_PORT_CDE= port2.OVC_PORT_CDE,
                };
            if (!rdoODT_IMPORT_DATE.SelectedItem.Text.Equals("當日作業"))
            {
                DateTime start = Convert.ToDateTime(strODT_IMPORT_DATE_S);
                DateTime end = Convert.ToDateTime(strODT_IMPORT_DATE_E);
                query = query.Where(table => table.ODT_REQUIRE_DATE >= start && table.ODT_REQUIRE_DATE < end);
            }
            else
            {
                DateTime now = DateTime.Now;
                query = query.Where(table => table.ODT_REQUIRE_DATE == now);
            }
            
            if (!strOVC_MILITARY_TYPE.Equals(string.Empty))
                query = query.Where(table => table.OVC_MILITARY_TYPE.Equals(strOVC_MILITARY_TYPE));
            if (!strOVC_SEA_OR_AIR.Equals("不限定"))
                query = query.Where(table => table.OVC_SEA_OR_AIR.Equals(strOVC_SEA_OR_AIR));
            if (!strOVC_SHIP_COMPANY.Equals("不限定"))
                query = query.Where(table => table.OVC_SHIP_COMPANY.Equals(strOVC_SHIP_COMPANY));
            if (!strOVC_DEPT_NAME.Equals("不限定"))
            {
                switch (strOVC_DEPT_NAME)
                {
                    case "基隆地區":
                        query = query.Where(table => table.OVC_PORT_CDE.Equals("TWKEL"));
                        break;
                    case "桃園地區":
                        query = query.Where(table => table.OVC_PORT_CDE.Equals("TPE") || table.OVC_PORT_CDE.Equals("TTY"));
                        break;
                    case "高雄分遣組":
                        query = query.Where(table => table.OVC_PORT_CDE.Equals("TWKHH") || table.OVC_PORT_CDE.Equals("KHH"));
                        break;
                    default:
                        break;
                }
            }
            if (!strOVC_BLD_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
            if (!strOVC_PURCH_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_PURCH_NO.Contains(strOVC_PURCH_NO));
            if (!strOVC_EDF_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_EDF_NO.Contains(strOVC_EDF_NO));
            if (strOVC_IS_SECURITY.Equals(1))
                query = query.Where(table => table.OVC_IS_SECURITY == strOVC_IS_SECURITY);
            else
                query = query.Where(table => table.OVC_IS_SECURITY == strOVC_IS_SECURITY || table.OVC_IS_SECURITY == null);
            //query = query.Where(table => table.OVC_IS_ABROAD.Equals("國內"));
            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows2"] = FCommon.GridView_dataImport(GV_TBGMT_BLD_export, dt, strFieldExport);
            //儲存BLD
            if (dt.Rows.Count > 0)
            {
                string bld_no_list = dt.Rows[0][5].ToString();
                for (var a = 1; a < dt.Rows.Count; a++)
                {
                    bld_no_list += "," + dt.Rows[a][5].ToString();
                   
                }
                Session["bld_no_list"] = bld_no_list;
            }
            if (GV_TBGMT_BLD_export.Rows.Count > 0)
            {
                btnPrintExport.Visible = true;
                btnPrint.Visible = false;
            }
            else
            {
                btnPrintExport.Visible = false;
                btnPrint.Visible = false;
            }
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {

            #region PDF
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChFont = new Font(bfChinese, 10f, Font.BOLD);
            Font Title_ChFont = new Font(bfChinese, 18f, Font.BOLD);
            MemoryStream Memory = new MemoryStream();
            var doc1 = new Document(PageSize.A4, 50, 50, 80, 50);  //la long
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            DateTime PrintTime = DateTime.Now;
            writer.PageEvent = new Header();
            doc1.Open();
            PdfPTable Firsttable = new PdfPTable(12);
            Firsttable.SetWidths(new float[] { 2, 3, 3, 6, 4, 3, 3, 3, 3, 3, 3, 3 });
            Firsttable.TotalWidth = 580F;
            Firsttable.LockedWidth = true;
            Firsttable.DefaultCell.FixedHeight = 40f;
            Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            string[] list = new string[1];
            string[] strTitle = { "軍種", "海空運", "承運航商", "提單編號", "案號", "啟運港埠", "抵運港埠", "報關日期", "進口日期", "通關日期", "清運日期", "接收日期" };
            for (var i = 0; i < strTitle.Length; i++)
            {
                Firsttable.AddCell(new Phrase(strTitle[i], ChFont));
            }
            if (Session["bld_no_list"].ToString().Contains(","))
            {
                list = Session["bld_no_list"].ToString().Split(',');
            }
            else
            {
                list[0]= Session["bld_no_list"].ToString();
            }
            
            if (GV_TBGMT_BLD.Rows.Count > 0)
            {
                for (var i = 0; i < GV_TBGMT_BLD.Rows.Count; i++)
                {
                    for (var x = 0; x < strTitle.Length; x++)
                    {
                        if (x == 3)
                        {
                            Firsttable.AddCell(new Phrase(list[i], ChFont));
                        }
                        else
                        {
                            Firsttable.AddCell(new Phrase(GV_TBGMT_BLD.Rows[i].Cells[x].Text.Replace("&nbsp;", ""), ChFont));
                        }
                    }
                }
            }
            PdfPCell foot = new PdfPCell(new Phrase("總共" + GV_TBGMT_BLD.Rows.Count + "筆資料", ChFont));
            foot.Colspan = 12;
            foot.Border = Rectangle.NO_BORDER;
            Firsttable.AddCell(foot);
            doc1.Add(Firsttable);
            doc1.Close();
            Response.Clear();//瀏覽器上顯示
            Response.AddHeader("Content-disposition", "attachment;filename=運案查詢.pdf");
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.Flush();
            Response.End();
            #endregion
        }
        protected void btnPrintExport_Click(object sender, EventArgs e)
        {
            #region PDF
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChFont = new Font(bfChinese, 10f, Font.BOLD);
            Font Title_ChFont = new Font(bfChinese, 18f, Font.BOLD);
            MemoryStream Memory = new MemoryStream();
            var doc1 = new Document(PageSize.A4, 50, 50, 80, 50);  //la long
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            DateTime PrintTime = DateTime.Now;
            writer.PageEvent = new Header();
            doc1.Open();
            string[] list = new string[11];
            PdfPTable Firsttable = new PdfPTable(13);
            Firsttable.SetWidths(new float[] { 2, 3, 3, 5, 3, 2, 3, 3, 3, 3, 3, 3, 3 });
            Firsttable.TotalWidth = 580F;
            Firsttable.LockedWidth = true;
            Firsttable.DefaultCell.FixedHeight = 40f;
            Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            string[] strTitle = { "軍種", "海空運", "承運航商", "外運資料表編號", "提單編號", "案號", "啟運港埠", "抵運港埠", "委運單位函文日期", "中心函文免稅日期", "進艙日期", "報關日期", "啟運日期" };
            for (var i = 0; i < strTitle.Length; i++)
            {
                Firsttable.AddCell(new Phrase(strTitle[i], ChFont));
            }
            if (Session["bld_no_list"].ToString().Contains(","))
            {
                list = Session["bld_no_list"].ToString().Split(',');
            }
            else
            {
                list[0] = Session["bld_no_list"].ToString();
            }

            if (GV_TBGMT_BLD_export.Rows.Count > 0)
            {
                for (var i = 0; i < GV_TBGMT_BLD_export.Rows.Count; i++)
                {
                    for (var x = 0; x < strTitle.Length; x++)
                    {
                        if (x == 4)
                        {
                            Firsttable.AddCell(new Phrase(list[i], ChFont));
                        }
                        else
                        {
                            Firsttable.AddCell(new Phrase(GV_TBGMT_BLD_export.Rows[i].Cells[x].Text.Replace("&nbsp;", ""), ChFont));
                        }
                    }
                }
            }
            PdfPCell foot = new PdfPCell(new Phrase("總共" + GV_TBGMT_BLD_export.Rows.Count + "筆資料", ChFont));
            foot.Colspan = 12;
            foot.Border = Rectangle.NO_BORDER;
            Firsttable.AddCell(foot);
            doc1.Add(Firsttable);
            doc1.Close();
            Response.Clear();//瀏覽器上顯示
            Response.AddHeader("Content-disposition", "attachment;filename=運案查詢.pdf");
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.Flush();
            Response.End();
            #endregion
        }

        protected void txtOvcBldNo_TextChanged(object sender, EventArgs e)
        {
            txtOVC_BLD_NO.Text = txtOVC_BLD_NO.Text.ToUpper();   //轉大寫
        }
        protected void txtOvcEdfNo_TextChanged(object sender, EventArgs e)
        {
            txtOVC_EDF_NO.Text = txtOVC_EDF_NO.Text.ToUpper();   //轉大寫
        }
    }
}