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
    public partial class MTS_G16 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        string strImagePagh = Content.Variable.strImagePath;
        private MTSEntities MTS = new MTSEntities();
        Common FCommon = new Common();
        string[] strField = { "OVC_CLASS_NAME", "OVC_CHI_NAME", "OVC_BLD_NO", "OVC_PORT_CHI_NAME", "ODT_ACT_ARRIVE_DATE", "ONB_CARRIAGE" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (!IsPostBack)
                {
                    FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                    FCommon.Controls_Attributes("readonly", "true", txtODT_LAST_START_DATE_S, txtODT_LAST_START_DATE_E);
                    //新增軍種dropdownlist item
                    DataTable DEPT_CLASS = CommonStatic.ListToDataTable(MTS.TBGMT_DEPT_CLASS.ToList());
                    FCommon.list_dataImport(drpOVC_MILITARY_TYPE, DEPT_CLASS, "OVC_CLASS_NAME", "OVC_CLASS", true);
                    CommonMTS.list_dataImport_SHIP_COMPANY(drpOVC_SHIP_COMPANY, true); //承運航商
                    //新增品名下拉是
                    DataTable dt = new DataTable();
                    var query =
                        from sea_price in MTS.TBGMT_TRANSFER_SEA_PRICE
                        select new
                        {
                            sea_price.OVC_CHI_NAME,
                            sea_price.OVC_INDEX_NO,
                        };
                    dt = CommonStatic.LinqQueryToDataTable(query.Distinct());
                    FCommon.list_dataImport(drpOVC_ITEM_TYPE, dt, "OVC_CHI_NAME", "OVC_INDEX_NO", true);

                    txtODT_LAST_START_DATE_S.Text = DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd");
                    txtODT_LAST_START_DATE_E.Text = DateTime.Now.ToString("yyyy/MM/dd");
                }
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChFont = new Font(bfChinese, 12f, Font.BOLD);
            Font smaillChFont = new Font(bfChinese, 10f, Font.BOLD);
            Font Title_ChFont = new Font(bfChinese, 18f, Font.BOLD);
            MemoryStream Memory = new MemoryStream();
            var doc1 = new Document(PageSize.A4, 80, 80, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            doc1.Open();
            PdfPTable Firsttable = new PdfPTable(6);
            Firsttable.SetWidths(new float[] { 2, 2, 4, 4, 3, 2 });
            Firsttable.TotalWidth = 580F;
            Firsttable.LockedWidth = true;
            Firsttable.DefaultCell.FixedHeight = 40f;
            Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell TITLE = new PdfPCell(new Phrase("年度海運運費查詢", Title_ChFont));
            TITLE.Colspan = 6;
            TITLE.Border = Rectangle.NO_BORDER;
            TITLE.VerticalAlignment = Element.ALIGN_MIDDLE;
            TITLE.HorizontalAlignment = Element.ALIGN_CENTER;
            Firsttable.AddCell(TITLE);
            PdfPCell PrintTime = new PdfPCell(new Phrase("列印日期：" + DateTime.Now.ToString("yyyy/MM/dd"), smaillChFont));
            PrintTime.Colspan = 6;
            PrintTime.Border = Rectangle.NO_BORDER;
            PrintTime.VerticalAlignment = Element.ALIGN_MIDDLE;
            PrintTime.HorizontalAlignment = Element.ALIGN_RIGHT;
            Firsttable.AddCell(PrintTime);
            string[] strTitle = {"軍種" , "品名分類", "提單編號","最後離境/\n抵運港口", "最後離境日期",  "運費" };
            for (var i = 0; i < strTitle.Length; i++)
            {
                Firsttable.AddCell(new Phrase(strTitle[i], ChFont));
            }
            double total = 0;

            if (GVTBGMT_BLD.Rows.Count > 0)
            {
                for (var i = 0; i < GVTBGMT_BLD.Rows.Count; i++)
                {
                    Firsttable.AddCell(new Phrase(GVTBGMT_BLD.Rows[i].Cells[0].Text.Replace("&nbsp;", ""), ChFont));
                    Firsttable.AddCell(new Phrase(GVTBGMT_BLD.Rows[i].Cells[1].Text.Replace("&nbsp;", ""), ChFont));
                    Firsttable.AddCell(new Phrase(GVTBGMT_BLD.Rows[i].Cells[2].Text.Replace("&nbsp;", ""), ChFont));
                    Firsttable.AddCell(new Phrase(GVTBGMT_BLD.Rows[i].Cells[3].Text.Replace("&nbsp;", ""), ChFont));
                    Firsttable.AddCell(new Phrase(GVTBGMT_BLD.Rows[i].Cells[4].Text.Replace("&nbsp;", ""), ChFont));
                    Firsttable.AddCell(new Phrase(GVTBGMT_BLD.Rows[i].Cells[5].Text.Replace("&nbsp;", ""), ChFont));
                    if (GVTBGMT_BLD.Rows[i].Cells[5].Text != "&nbsp;")
                        total += Convert.ToDouble(GVTBGMT_BLD.Rows[i].Cells[5].Text);
                }
            }
            PdfPCell foot = new PdfPCell(new Phrase("共 " + GVTBGMT_BLD.Rows.Count.ToString() + " 筆", ChFont));
            foot.Colspan = 4;
            foot.VerticalAlignment = Element.ALIGN_MIDDLE;
            foot.HorizontalAlignment = Element.ALIGN_CENTER;
            Firsttable.AddCell(foot);
            Firsttable.AddCell(new Phrase("海費總計", ChFont));
            Firsttable.AddCell(new Phrase(total.ToString(), ChFont));
            doc1.Add(Firsttable);

            doc1.Close();
            Response.Clear();//瀏覽器上顯示
            Response.AddHeader("Content-disposition", "attachment;filename=年度海運運費查詢.pdf");
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.Flush();
            Response.End();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_SHIP_COMPANY = drpOVC_SHIP_COMPANY.SelectedValue;
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedItem.Text;
            string strOVC_ITEM_TYPE = drpOVC_ITEM_TYPE.SelectedItem.Text;
            string strODT_LAST_START_DATE_S = txtODT_LAST_START_DATE_S.Text;
            string strODT_LAST_START_DATE_E = txtODT_LAST_START_DATE_E.Text;

            var query =
                from bld_carriage in MTS.TBGMT_BLD_CARRIAGE
                join bld in MTS.TBGMT_BLD on bld_carriage.OVC_BLD_NO equals bld.OVC_BLD_NO into p1
                from bld in p1.DefaultIfEmpty()
                join ics in MTS.TBGMT_ICS on bld_carriage.OVC_BLD_NO equals ics.OVC_BLD_NO into p2
                from ics in p2.DefaultIfEmpty()
                join dept_class in MTS.TBGMT_DEPT_CLASS on bld.OVC_MILITARY_TYPE equals dept_class.OVC_CLASS into p3
                from dept_class in p3.DefaultIfEmpty()
                join ports in MTS.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals ports.OVC_PORT_CDE into p4
                from ports in p4.DefaultIfEmpty()
                join sea_price in MTS.TBGMT_TRANSFER_SEA_PRICE on bld_carriage.OVC_ITEM_TYPE equals sea_price.OVC_INDEX_NO into p5
                from sea_price in p5.DefaultIfEmpty()
                select new
                {
                    OVC_CLASS_NAME = dept_class.OVC_CLASS_NAME,
                    OVC_CHI_NAME = sea_price.OVC_CHI_NAME,
                    OVC_BLD_NO = bld.OVC_BLD_NO,
                    OVC_PORT_CHI_NAME = ports.OVC_PORT_CHI_NAME,
                    ODT_ACT_ARRIVE_DATE = bld.ODT_ACT_ARRIVE_DATE,
                    ONB_CARRIAGE = bld_carriage.ONB_CARRIAGE,
                    OVC_SEA_OR_AIR = bld.OVC_SEA_OR_AIR,
                    OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY,
                    ODT_LAST_START_DATE = bld.ODT_LAST_START_DATE,
                };


            if (!strOVC_BLD_NO.Equals(string.Empty))
                query = query.Where(t => t.OVC_BLD_NO.Contains(strOVC_BLD_NO));

            if (!strOVC_SHIP_COMPANY.Equals(string.Empty))
                query = query.Where(t => t.OVC_SHIP_COMPANY.Contains(strOVC_SHIP_COMPANY));

            if (!strOVC_MILITARY_TYPE.Equals("請選擇"))
                query = query.Where(table => table.OVC_CLASS_NAME.Equals(strOVC_MILITARY_TYPE));

            if (!strOVC_ITEM_TYPE.Equals("請選擇"))
                query = query.Where(table => table.OVC_CHI_NAME.Equals(strOVC_ITEM_TYPE));

            if (chkODT_ACT_ARRIVE_DATE.Checked != true)
            {
                DateTime start = Convert.ToDateTime(strODT_LAST_START_DATE_S);
                DateTime end = Convert.ToDateTime(strODT_LAST_START_DATE_E);

                query = query.Where(table => table.ODT_LAST_START_DATE >= start && table.ODT_LAST_START_DATE <= end);
            }
            else
            {
                txtODT_LAST_START_DATE_S.Text = "";
                txtODT_LAST_START_DATE_E.Text = "";
            }
            query = query.Where(table => table.OVC_SEA_OR_AIR.Equals("海運"));
            query = query.Take(1000);
            dt = CommonStatic.LinqQueryToDataTable(query.Distinct());
            ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_BLD, dt, strField);
            if (GVTBGMT_BLD.Rows.Count > 0)
            {
                btnPrint.Visible = true;
            }
            else
            {
                btnPrint.Visible = false;
            }
        }

        protected void GVTBGMT_BLD_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

    }
}