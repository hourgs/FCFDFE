using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FCFDFE.pages.MTS.B
{
    public class Side : PdfPageEventHelper
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

            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "編號：977-3-02-01-03", pageSize.GetLeft(85), pageSize.GetBottom(270), 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "中華民國93年8月31日初版", pageSize.GetRight(85), pageSize.GetBottom(270), 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "中華民國93年11月4日二版", pageSize.GetRight(85), pageSize.GetBottom(255), 0);

            cb.EndText();

            string[] text = { "第", "一", "聯", "：", "送", "合", "約", "保", "險", "公", "司" };
            cb.BeginText();
            for (int i = 0; i < 11; i++)
            {
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text[i], pageSize.GetRight(20), pageSize.GetTop(70 + (i * 10)), 0);
            }
            cb.EndText();
        }
    }
    public partial class MTS_B12_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();

        #region 副程式
        protected void dataimport()
        {
            string strOVC_IINN_NO = txtOVC_IINN_NO.Text;
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_PURCH_NO = txtOVC_PURCH_NO.Text;

            ViewState["OVC_IINN_NO"] = strOVC_IINN_NO;
            ViewState["OVC_BLD_NO"] = strOVC_BLD_NO;
            ViewState["OVC_PURCH_NO"] = strOVC_PURCH_NO;

            var query =
                from iinn in MTSE.TBGMT_IINN
                join dept_class in MTSE.TBGMT_DEPT_CLASS on iinn.OVC_MILITARY_TYPE equals dept_class.OVC_CLASS into classTemp
                from dept_class in classTemp.DefaultIfEmpty()
                select new
                {
                    OVC_IINN_NO = iinn.OVC_IINN_NO,
                    OVC_BLD_NO = iinn.OVC_BLD_NO ?? "",
                    OVC_PURCH_NO = iinn.OVC_PURCH_NO ?? "",
                    ONB_INS_AMOUNT = iinn.ONB_INS_AMOUNT,
                    OVC_FINAL_INS_AMOUNT = iinn.OVC_FINAL_INS_AMOUNT,
                    OVC_MILITARY_TYPE = dept_class != null ? dept_class.OVC_CLASS_NAME : "",
                    OVC_DELIVERY_CONDITION = iinn.OVC_DELIVERY_CONDITION,
                    OVC_PURCHASE_TYPE = iinn.OVC_PURCHASE_TYPE
                };
            if (!strOVC_IINN_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_IINN_NO.Contains(strOVC_IINN_NO));
            if (!strOVC_BLD_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
            if (!strOVC_PURCH_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_PURCH_NO.Contains(strOVC_PURCH_NO));
            if (query.Count() > 1000)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
            query = query.Take(1000);

            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_IINN, dt);
        }
        private void dataPrint(string strOVC_IINN_NO)
        {
            Document doc = new Document(PageSize.A4, 10, 10, 30, 20);
            MemoryStream Memory = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, Memory);
            writer.PageEvent = new Side();
            doc.Open();
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H
              , BaseFont.NOT_EMBEDDED);
            Font ChFont = new Font(bfChinese, 12, Font.NORMAL, BaseColor.BLACK);
            string chFontPath = "c:\\windows\\fonts\\KAIU.TTF";
            BaseFont chBaseFont = BaseFont.CreateFont(chFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            var query =
                from iinn in MTSE.TBGMT_IINN
                where iinn.OVC_IINN_NO.Equals(strOVC_IINN_NO)
                join bld in MTSE.TBGMT_BLD on iinn.OVC_BLD_NO equals bld.OVC_BLD_NO into bldTemp
                from bld in bldTemp.DefaultIfEmpty()
                join icr in MTSE.TBGMT_ICR on iinn.OVC_BLD_NO equals icr.OVC_BLD_NO into icrTemp
                from icr in icrTemp.DefaultIfEmpty()
                join portSta in MTSE.TBGMT_PORTS on bld.OVC_START_PORT equals portSta.OVC_PORT_CDE into portStaTemp
                from portSta in portStaTemp.DefaultIfEmpty()
                join portArr in MTSE.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals portArr.OVC_PORT_CDE into portArrTemp
                from portArr in portArrTemp.DefaultIfEmpty()
                join dept_class in MTSE.TBGMT_DEPT_CLASS on iinn.OVC_MILITARY_TYPE equals dept_class.OVC_CLASS into classTemp
                from dept_class in classTemp.DefaultIfEmpty()
                join currency in MTSE.TBGMT_CURRENCY on iinn.ONB_CARRIAGE_CURRENCY equals currency.OVC_CURRENCY_CODE into currencyTemp
                from currency in currencyTemp.DefaultIfEmpty()
                join currency2 in MTSE.TBGMT_CURRENCY on iinn.ONB_CARRIAGE_CURRENCY2 equals currency2.OVC_CURRENCY_CODE into currencyTemp2
                from currency2 in currencyTemp2.DefaultIfEmpty()
                select new
                {
                    OVC_IINN_NO = iinn.OVC_IINN_NO,
                    OVC_BLD_NO = bld != null ? bld.OVC_BLD_NO : "",
                    OVC_PURCH_NO = iinn.OVC_PURCH_NO,
                    OVC_CHI_NAME = icr != null ? icr.OVC_CHI_NAME : "",
                    ONB_QUANITY = bld != null ? bld.ONB_QUANITY : null,
                    ONB_ITEM_VALUE = iinn.ONB_ITEM_VALUE,
                    ONB_INS_AMOUNT = iinn.ONB_INS_AMOUNT,
                    OVC_DELIVERY_CONDITION = iinn.OVC_DELIVERY_CONDITION,
                    OVC_INS_CONDITION = iinn.OVC_INS_CONDITION,
                    ONB_INS_RATE = iinn.ONB_INS_RATE,
                    OVC_SHIP_COMPANY = bld != null ? bld.OVC_SHIP_COMPANY : "",
                    OVC_START_PORT = portSta != null ? portSta.OVC_PORT_CHI_NAME : "",
                    ODT_START_DATE = bld != null ? bld.ODT_START_DATE : null,
                    OVC_ARRIVE_PORT = portArr != null ? portArr.OVC_PORT_CHI_NAME : "",
                    //ODT_PLN_ARRIVE_DATE = bld != null ? bld.ODT_PLN_ARRIVE_DATE : "",
                    ODT_IMPORT_DATE = icr != null ? icr.ODT_IMPORT_DATE : null,
                    OVC_PURCHASE_TYPE = iinn.OVC_PURCHASE_TYPE,
                    OVC_PAYMENT_TYPE = iinn.OVC_PAYMENT_TYPE,
                    OVC_CLASS_NAME = dept_class != null ? dept_class.OVC_CLASS_NAME : "",
                    ODT_INS_DATE = iinn.ODT_INS_DATE,
                    OVC_NOTE = iinn.OVC_NOTE,
                    OVC_CURRENCY_NAME = currency != null ? currency.OVC_CURRENCY_NAME : "",
                    OVC_CURRENCY_NAME2 = currency2 != null ? currency2.OVC_CURRENCY_NAME : ""
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                //創建table
                PdfPTable pdftable = new PdfPTable(new float[] { 4, 2, 2, 6 });
                pdftable.TotalWidth = 500f;
                pdftable.LockedWidth = true;
                pdftable.DefaultCell.FixedHeight = 60;

                PdfPCell title = new PdfPCell(new Phrase("國防部國防採購室國外物資海空運保險投保通知書", new Font(bfChinese, 14, Font.BOLD, BaseColor.BLACK)));
                title.VerticalAlignment = Element.ALIGN_TOP;
                title.HorizontalAlignment = Element.ALIGN_CENTER;
                title.Colspan = 4;
                title.FixedHeight = 50;
                pdftable.AddCell(title);

                DateTime today = DateTime.Now;
                PdfContentByte cb = writer.DirectContent;//插入標題右/左下角編號
                Rectangle pageSize = doc.PageSize;
                cb.SetRGBColorFill(0, 0, 0);
                cb.BeginText();
                cb.SetFontAndSize(chBaseFont, 10);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, strOVC_IINN_NO, pageSize.GetRight(50), pageSize.GetTop(65), 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, FCommon.getDateTime(today), pageSize.GetRight(50), pageSize.GetTop(75), 0);
                cb.EndText();
                cb.BeginText();
                cb.SetFontAndSize(chBaseFont, 12);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "提單號碼:" + dr["OVC_BLD_NO"].ToString(), pageSize.GetLeft(50), pageSize.GetTop(75), 0);
                // cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, dr["OVC_BLD_NO"].ToString(), pageSize.GetLeft(125), pageSize.GetTop(75), 0);

                cb.EndText();

                PdfPCell cell1_0 = new PdfPCell(new Phrase("案號", ChFont));
                cell1_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell1_0.FixedHeight = 30;
                pdftable.AddCell(cell1_0);

                PdfPCell cell1_1 = new PdfPCell(new Phrase(dr["OVC_PURCH_NO"].ToString(), ChFont));
                cell1_1.Colspan = 3;
                cell1_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell1_1.FixedHeight = 30;
                pdftable.AddCell(cell1_1);

                PdfPCell cell2_0 = new PdfPCell(new Phrase("物品名稱及數量", ChFont));
                cell2_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell2_0.FixedHeight = 30;
                pdftable.AddCell(cell2_0);

                PdfPCell cell2_1 = new PdfPCell(new Phrase(dr["OVC_CHI_NAME"].ToString() + "/" + dr["ONB_QUANITY"].ToString(), ChFont));
                cell2_1.Colspan = 3;
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
                cell3_1.Colspan = 3;
                pdftable.AddCell(cell3_1);

                PdfPCell cell4_0 = new PdfPCell(new Phrase("投保金額", ChFont));
                cell4_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell4_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4_0.FixedHeight = 30;
                pdftable.AddCell(cell4_0);

                PdfPCell cell4_1 = new PdfPCell(new Phrase(dr["ONB_INS_AMOUNT"].ToString() + dr["OVC_CURRENCY_NAME2"], ChFont));
                cell4_1.FixedHeight = 30;
                cell4_1.Colspan = 3;
                cell4_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell4_1);

                PdfPCell cell5_0 = new PdfPCell(new Phrase("交貨條件", ChFont));
                cell5_0.FixedHeight = 30;
                cell5_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell5_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell5_0);

                PdfPCell cell5_1 = new PdfPCell(new Phrase(dr["OVC_DELIVERY_CONDITION"].ToString(), ChFont));
                cell5_1.FixedHeight = 30;
                cell5_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell5_1);

                PdfPCell cell5_2 = new PdfPCell(new Phrase("保險條件", ChFont));
                cell5_2.FixedHeight = 30;
                cell5_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell5_2.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell5_2);

                PdfPCell cell5_3 = new PdfPCell(new Phrase(dr["OVC_INS_CONDITION"].ToString(), ChFont));
                cell5_3.FixedHeight = 30;
                cell5_3.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell5_3);

                PdfPCell cell6_0 = new PdfPCell(new Phrase("保險費率", ChFont));
                cell6_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell6_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell6_0.FixedHeight = 30;
                pdftable.AddCell(cell6_0);

                PdfPCell cell6_1 = new PdfPCell(new Phrase(dr["ONB_INS_RATE"].ToString() + " %", ChFont));
                cell6_1.FixedHeight = 30;
                cell6_1.Colspan = 3;
                cell6_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell6_1);

                PdfPCell cell7_0 = new PdfPCell(new Phrase("承運商", ChFont));
                cell7_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell7_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell7_0.FixedHeight = 30;
                pdftable.AddCell(cell7_0);

                PdfPCell cell7_1 = new PdfPCell(new Phrase(dr["OVC_SHIP_COMPANY"].ToString(), ChFont));
                cell7_1.FixedHeight = 30;
                cell7_1.Colspan = 3;
                cell7_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell7_1);

                PdfPCell cell8_0 = new PdfPCell(new Phrase("起運港口", ChFont));
                cell8_0.FixedHeight = 30;
                cell8_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell8_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell8_0);

                PdfPCell cell8_1 = new PdfPCell(new Phrase(dr["OVC_START_PORT"].ToString(), ChFont));
                cell8_1.FixedHeight = 30;
                cell8_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell8_1);

                PdfPCell cell8_2 = new PdfPCell(new Phrase("起運時間", ChFont));
                cell8_2.FixedHeight = 30;
                cell8_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell8_2.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell8_2);

                PdfPCell cell8_3 = new PdfPCell();
                cell8_3 = new PdfPCell(new Phrase(FCommon.getDateTime(dr["ODT_START_DATE"]), ChFont));
                cell8_3.FixedHeight = 30;
                cell8_3.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell8_3);

                PdfPCell cell9_0 = new PdfPCell(new Phrase("目的港口", ChFont));
                cell9_0.FixedHeight = 30;
                cell9_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell9_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell9_0);

                PdfPCell cell9_1 = new PdfPCell(new Phrase(dr["OVC_ARRIVE_PORT"].ToString(), ChFont));
                cell9_1.FixedHeight = 30;
                cell9_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell9_1);

                PdfPCell cell9_2 = new PdfPCell(new Phrase("進口時間", ChFont));
                cell9_2.FixedHeight = 30;
                cell9_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell9_2.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell9_2);

                PdfPCell cell9_3 = new PdfPCell();
                cell9_3 = new PdfPCell(new Phrase(FCommon.getDateTime(dr["ODT_IMPORT_DATE"]), ChFont));
                cell9_3.FixedHeight = 30;
                cell9_3.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell9_3);

                PdfPCell cell10_0 = new PdfPCell(new Phrase("軍購或商購", ChFont));
                cell10_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell10_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell10_0.FixedHeight = 30;
                pdftable.AddCell(cell10_0);

                PdfPCell cell10_1 = new PdfPCell(new Phrase(dr["OVC_PURCHASE_TYPE"].ToString(), ChFont));
                cell10_1.FixedHeight = 30;
                cell10_1.Colspan = 3;
                cell10_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell10_1);

                PdfPCell cell11_0 = new PdfPCell(new Phrase("保費支付方式", ChFont));
                cell11_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell11_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell11_0.FixedHeight = 30;
                pdftable.AddCell(cell11_0);

                PdfPCell cell11_1 = new PdfPCell(new Phrase(dr["OVC_PAYMENT_TYPE"].ToString(), ChFont));
                cell11_1.FixedHeight = 30;
                cell11_1.Colspan = 3;
                cell11_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell11_1);

                PdfPCell cell12_0 = new PdfPCell(new Phrase("軍種", ChFont));
                cell12_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell12_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell12_0.FixedHeight = 30;
                pdftable.AddCell(cell12_0);

                PdfPCell cell12_1 = new PdfPCell(new Phrase(dr["OVC_CLASS_NAME"].ToString(), ChFont));
                cell12_1.FixedHeight = 30;
                cell12_1.Colspan = 3;
                cell12_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell12_1);

                PdfPCell cell13_0 = new PdfPCell(new Phrase("投保日期", ChFont));
                cell13_0.HorizontalAlignment = Element.ALIGN_CENTER;
                cell13_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell13_0.FixedHeight = 30;
                pdftable.AddCell(cell13_0);

                PdfPCell cell13_1 = new PdfPCell();
                cell13_1 = new PdfPCell(new Phrase(FCommon.getDateTime(dr["ODT_INS_DATE"]), ChFont));

                cell13_1.FixedHeight = 30;
                cell13_1.Colspan = 3;
                cell13_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdftable.AddCell(cell13_1);

                PdfPCell cell14_0 = new PdfPCell(new Phrase(dr["OVC_NOTE"].ToString() + " ", ChFont));
                cell14_0.FixedHeight = 90;
                cell14_0.Colspan = 4;
                pdftable.AddCell(cell14_0);

                doc.Add(pdftable);
                doc.Close();

                string strFileName = "國防部國防採購室國外物資海空運保險投保通知書.pdf";
                FCommon.DownloadFile(this, strFileName, Memory);
            }
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_IINN_NO", ViewState["OVC_IINN_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", ViewState["OVC_BLD_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_PURCH_NO", ViewState["OVC_PURCH_NO"], true);
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
                    bool boolImport = false;
                    if (FCommon.getQueryString(this, "OVC_IINN_NO", out string strOVC_IINN_NO, true))
                    {
                        txtOVC_IINN_NO.Text = strOVC_IINN_NO;
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_BLD_NO", out string strOVC_BLD_NO, true))
                        txtOVC_BLD_NO.Text = strOVC_BLD_NO;
                    if (FCommon.getQueryString(this, "OVC_PURCH_NO", out string strOVC_PURCH_NO, true))
                        txtOVC_PURCH_NO.Text = strOVC_PURCH_NO;
                    if (boolImport) dataimport();
                }
            }
        }

        #region ~Click
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataimport();
        }
        #endregion

        #region GridView
        protected void GVTBGMT_IINN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GVTBGMT_IINN.DataKeys[gvrIndex].Value.ToString();
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);
            
            switch (e.CommandName)
            {
                case "dataModify":
                    Response.Redirect($"MTS_B12_2{ strQueryString }");
                    break;
                case "dataDel":
                    Response.Redirect($"MTS_B12_3{ strQueryString }");
                    break;
                case "dataPrint":
                    dataPrint(id);
                    break;
            }
        }
        protected void GVTBGMT_IINN_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void GVTBGMT_IINN_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
#endregion
    }
}