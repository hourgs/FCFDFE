using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;

namespace FCFDFE.pages.MTS.G
{
    public partial class MTS_G15 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        string strImagePagh = Content.Variable.strImagePath;
        private MTSEntities MTS = new MTSEntities();
        Common FCommon = new Common();
        string[] strField = { "START_PORT", "END_PORT", "OVC_ICS_NO", "OVC_BLD_NO", "OVC_INLAND_CARRIAGE" };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (!IsPostBack)
                {
                    FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                    FCommon.Controls_Attributes("readonly", "true", txtODT_APPLY_DATE_S, txtODT_APPLY_DATE_E);
                    //新增軍種dropdownlist item
                    DataTable DEPT_CLASS = CommonStatic.ListToDataTable(MTS.TBGMT_DEPT_CLASS.ToList());
                    FCommon.list_dataImport(drpOVC_MILITARY_TYPE, DEPT_CLASS, "OVC_CLASS_NAME", "OVC_CLASS", true);
                    //新增抵運、啟運港埠dropdownlist item
                    DataTable Port = CommonStatic.ListToDataTable(MTS.TBGMT_PORTS.ToList());
                    FCommon.list_dataImport(drpOVC_START_PORT, Port, "OVC_PORT_CHI_NAME", "OVC_PORT_CDE", true);
                    FCommon.list_dataImport(drpOVC_ARRIVE_PORT, Port, "OVC_PORT_CHI_NAME", "OVC_PORT_CDE", true);
                    txtODT_APPLY_DATE_S.Text = DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd");
                    txtODT_APPLY_DATE_E.Text = DateTime.Now.ToString("yyyy/MM/dd");
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
            PdfPTable Firsttable = new PdfPTable(5);
            Firsttable.SetWidths(new float[] { 3, 3, 4, 3, 3 });
            Firsttable.TotalWidth = 580F;
            Firsttable.LockedWidth = true;
            Firsttable.DefaultCell.FixedHeight = 40f;
            Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell TITLE = new PdfPCell(new Phrase("年度空運運費查詢", Title_ChFont));
            TITLE.Colspan = 5;
            TITLE.Border = Rectangle.NO_BORDER;
            TITLE.VerticalAlignment = Element.ALIGN_MIDDLE;
            TITLE.HorizontalAlignment = Element.ALIGN_CENTER;
            Firsttable.AddCell(TITLE);
            PdfPCell PrintTime = new PdfPCell(new Phrase("列印日期：" + DateTime.Now.ToString("yyyy/MM/dd"), smaillChFont));
            PrintTime.Colspan = 5;
            PrintTime.Border = Rectangle.NO_BORDER;
            PrintTime.VerticalAlignment = Element.ALIGN_MIDDLE;
            PrintTime.HorizontalAlignment = Element.ALIGN_RIGHT;
            Firsttable.AddCell(PrintTime);
            string[] strTitle = { "啟運港埠", "抵運港埠", "案號", "提單編號", "運費" };
            string[] bldList = Session["G15_bld_no"].ToString().Split(',');
            for (var i = 0; i < strTitle.Length; i++)
            {
                Firsttable.AddCell(new Phrase(strTitle[i], ChFont));
            }
            double total = 0;

            if (GVTBGMT_BLD.Rows.Count > 0)
            {
                for(var i = 0; i< GVTBGMT_BLD.Rows.Count; i++)
                {
                    Firsttable.AddCell(new Phrase(GVTBGMT_BLD.Rows[i].Cells[0].Text, ChFont));
                    Firsttable.AddCell(new Phrase(GVTBGMT_BLD.Rows[i].Cells[1].Text, ChFont));
                    Firsttable.AddCell(new Phrase(GVTBGMT_BLD.Rows[i].Cells[2].Text, ChFont));
                    Firsttable.AddCell(new Phrase(bldList[i], ChFont));
                    Firsttable.AddCell(new Phrase(GVTBGMT_BLD.Rows[i].Cells[4].Text, ChFont));
                    if (GVTBGMT_BLD.Rows[i].Cells[4].Text != "&nbsp;")
                        total += Convert.ToDouble(GVTBGMT_BLD.Rows[i].Cells[4].Text);
                }
            }
            PdfPCell foot = new PdfPCell(new Phrase("共 " + GVTBGMT_BLD.Rows.Count.ToString() + " 筆", ChFont));
            foot.Colspan = 3;
            foot.VerticalAlignment = Element.ALIGN_MIDDLE;
            foot.HorizontalAlignment = Element.ALIGN_CENTER;
            Firsttable.AddCell(foot);
            Firsttable.AddCell(new Phrase("運費總計", ChFont));
            Firsttable.AddCell(new Phrase(total.ToString(), ChFont));
            doc1.Add(Firsttable);

            doc1.Close();
            Response.Clear();//瀏覽器上顯示
            Response.AddHeader("Content-disposition", "attachment;filename=年度空運運費查詢.pdf");
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.Flush();
            Response.End();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedItem.Value;
            string strOVC_START_PORT = drpOVC_START_PORT.SelectedItem.Text;
            string strOVC_ARRIVE_PORT = drpOVC_ARRIVE_PORT.SelectedItem.Text;
            string strODT_APPLY_DATE_S = txtODT_APPLY_DATE_S.Text;
            string strODT_APPLY_DATE_E = txtODT_APPLY_DATE_E.Text;
            string strOVC_ICS_NO = txtOVC_ICS_NO.Text;
            DataTable dt = new DataTable();
            var query =
                from ics in MTS.TBGMT_ICS
                join bld in MTS.TBGMT_BLD on ics.OVC_BLD_NO equals bld.OVC_BLD_NO into p1
                from bld in p1.DefaultIfEmpty()
                join ports in MTS.TBGMT_PORTS on bld.OVC_START_PORT equals ports.OVC_PORT_CDE into p2
                from ports in p2.DefaultIfEmpty()
                join porte in MTS.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals porte.OVC_PORT_CDE into p3
                from porte in p3.DefaultIfEmpty()
                select new
                {
                    START_PORT = ports.OVC_PORT_CHI_NAME,
                    END_PORT = porte.OVC_PORT_CHI_NAME,
                    OVC_ICS_NO = ics.OVC_ICS_NO,
                    OVC_BLD_NO = ics.OVC_BLD_NO,
                    OVC_INLAND_CARRIAGE = ics.OVC_INLAND_CARRIAGE,
                    OVC_MILITARY_TYPE = bld.OVC_MILITARY_TYPE,
                    OVC_SEA_OR_AIR = bld.OVC_SEA_OR_AIR,
                    ODT_START_DATE = bld.ODT_START_DATE,
                };

            if (!strOVC_BLD_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));

            if (!strOVC_ICS_NO.Equals(string.Empty))
                query = query.Where(t => t.OVC_ICS_NO.Contains(strOVC_ICS_NO));

            if (!strOVC_MILITARY_TYPE.Equals(string.Empty))
                query = query.Where(table => table.OVC_MILITARY_TYPE.Equals(strOVC_MILITARY_TYPE));

            if (!strOVC_START_PORT.Equals("請選擇"))
                query = query.Where(table => table.START_PORT.Equals(strOVC_START_PORT));

            if (!strOVC_ARRIVE_PORT.Equals("請選擇"))
                query = query.Where(table => table.END_PORT.Equals(strOVC_ARRIVE_PORT));

            if (chkODT_ACT_ARRIVE_DATE.Checked != true)
            {
                DateTime start = Convert.ToDateTime(strODT_APPLY_DATE_S);
                DateTime end = Convert.ToDateTime(strODT_APPLY_DATE_E);
                query = query.Where(table => table.ODT_START_DATE >= start && table.ODT_START_DATE <= end);
            }
            else
            {
                txtODT_APPLY_DATE_S.Text = "";
                txtODT_APPLY_DATE_E.Text = "";
            }
            query = query.Where(table => table.OVC_SEA_OR_AIR.Equals("空運"));
            if (query.Count() > 1000)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
            query = query.Take(1000);
            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_BLD, dt, strField);
            if (dt.Rows.Count > 0)
            {
                string bld_no = dt.Rows[0][3].ToString();
                for(var i=0; i < dt.Rows.Count; i++)
                {
                    bld_no+=","+ dt.Rows[i][3].ToString();
                }
                Session["G15_bld_no"] = bld_no;
            }
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
        protected void txtOVC_BLD_NO_TextChanged(object sender, EventArgs e)
        {
            txtOVC_BLD_NO.Text = txtOVC_BLD_NO.Text.ToUpper();   //轉大寫
        }
    }
}