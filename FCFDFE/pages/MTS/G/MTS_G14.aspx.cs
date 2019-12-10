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
    public partial class MTS_G14 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        string strImagePagh = Content.Variable.strImagePath;
        private MTSEntities MTS = new MTSEntities();
        Common FCommon = new Common();
        string[] strField = { "OVC_BLD_NO", "OVC_IINN_NO", "OVC_PURCH_NO", "ONB_ITEM_VALUE", "OVC_CLASS_NAME", "OVC_DELIVERY_CONDITION", "OVC_PURCHASE_TYPE", "ODT_IMPORT_DATE" };
        string[] strField2 = { "OVC_BLD_NO", "OVC_IINN_NO", "OVC_PURCH_NO", "ONB_ITEM_VALUE", "OVC_CLASS_NAME", "OVC_DELIVERY_CONDITION", "OVC_PURCHASE_TYPE", "ODT_IMPORT_DATE" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (!IsPostBack)
                {
                    FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                    FCommon.Controls_Attributes("readonly", "true", txtODT_START_DATE_S, txtODT_START_DATE_E);

                    //新增軍種dropdownlist item
                    DataTable DEPT_CLASS = CommonStatic.ListToDataTable(MTS.TBGMT_DEPT_CLASS.ToList());
                    FCommon.list_dataImport(drpOVC_MILITARY_TYPE, DEPT_CLASS, "OVC_CLASS_NAME", "OVC_CLASS", true);

                    txtODT_START_DATE_S.Text = DateTime.Now.ToString("yyyy/MM/dd");
                    txtODT_START_DATE_E.Text = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");
                    btnPrint2.Visible = false;
                    btnPrint.Visible = false;
                }
            }
        }
        protected void btnPrint2_Click(object sender, EventArgs e)
        {
            string[] strTitle = { "提單編號", "案號或採購文案", "軍種", "進口日期" };
            string[] bldlist = Session["G14_bld_no"].ToString().Split(',');


            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChFont = new Font(bfChinese, 12f, Font.BOLD);
            Font SmallChFont = new Font(bfChinese, 10f, Font.BOLD);
            Font Title_ChFont = new Font(bfChinese, 18f, Font.BOLD);
            MemoryStream Memory = new MemoryStream();
            var doc1 = new Document(PageSize.A4, 80, 80, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            doc1.Open();
            #region 進口軍售商購案未投保統計表 table

            PdfPTable Firsttable = new PdfPTable(4);
            Firsttable.SetWidths(new float[] {  4, 5, 3, 4 });
            Firsttable.TotalWidth = 580F;
            Firsttable.LockedWidth = true;
            Firsttable.DefaultCell.FixedHeight = 40f;
            Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell Title = new PdfPCell(new Phrase("進口軍售商購案未投保統計表", Title_ChFont));
            Title.HorizontalAlignment = Element.ALIGN_CENTER;
            Title.VerticalAlignment = Element.ALIGN_MIDDLE;
            Title.Colspan = 5;
            Title.Border = Rectangle.NO_BORDER;
            Firsttable.AddCell(Title);
            PdfPCell PrintTime = new PdfPCell(new Phrase("列印日期：" + DateTime.Now.AddYears(-1911).ToString("yy/MM/dd"), SmallChFont));
            PrintTime.HorizontalAlignment = Element.ALIGN_RIGHT;
            PrintTime.VerticalAlignment = Element.ALIGN_MIDDLE;
            PrintTime.Colspan = 5;
            PrintTime.Border = Rectangle.NO_BORDER;
            Firsttable.AddCell(PrintTime);

            #endregion
            for (var i = 0; i < strTitle.Length; i++)
            {
                Firsttable.AddCell(new Phrase(strTitle[i], ChFont));
            }
            for(var i =0; i < GVTBGMT_IINN.Rows.Count; i++)
            {
                Firsttable.AddCell(new Phrase(bldlist[i], ChFont));
                Firsttable.AddCell(new Phrase(GVTBGMT_IINN.Rows[i].Cells[1].Text.Replace("&nbsp;", ""), ChFont));
                Firsttable.AddCell(new Phrase(GVTBGMT_IINN.Rows[i].Cells[2].Text.Replace("&nbsp;", ""), ChFont));
                Firsttable.AddCell(new Phrase(GVTBGMT_IINN.Rows[i].Cells[3].Text.Replace("&nbsp;", ""), ChFont));
            }
            doc1.Add(Firsttable);
            doc1.Close();

            string fileName = HttpUtility.UrlEncode("進口軍售商購案未投保統計表.pdf");
            Response.Clear();//瀏覽器上顯示
            Response.AddHeader("Content-disposition", "attachment;filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.Flush();
            Response.End();
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChFont = new Font(bfChinese, 12f, Font.BOLD);
            Font SmallChFont = new Font(bfChinese, 10f, Font.BOLD);
            Font Title_ChFont = new Font(bfChinese, 18f, Font.BOLD);
            MemoryStream Memory = new MemoryStream();
            var doc1 = new Document(PageSize.A4, 80, 80, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            doc1.Open();
            #region 進口軍售商購物已投保統計表 table

            PdfPTable Secondtable = new PdfPTable(8);
            Secondtable.SetWidths(new float[] { 2, 2, 2, 2, 1, 2, 2, 2 });
            Secondtable.TotalWidth = 580F;
            Secondtable.LockedWidth = true;
            Secondtable.DefaultCell.FixedHeight = 40f;
            Secondtable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Secondtable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell secTitle = new PdfPCell(new Phrase("進口軍售商購案已投保統計表", Title_ChFont));
            secTitle.HorizontalAlignment = Element.ALIGN_CENTER;
            secTitle.VerticalAlignment = Element.ALIGN_MIDDLE;
            secTitle.Colspan = 8;
            secTitle.Border = Rectangle.NO_BORDER;
            Secondtable.AddCell(secTitle);
            PdfPCell secPrintTime = new PdfPCell(new Phrase("列印日期：" + DateTime.Now.AddYears(-1911).ToString("yy/MM/DD"), SmallChFont));
            secPrintTime.HorizontalAlignment = Element.ALIGN_RIGHT;
            secPrintTime.VerticalAlignment = Element.ALIGN_MIDDLE;
            secPrintTime.Colspan = 8;
            secPrintTime.Border = Rectangle.NO_BORDER;
            Secondtable.AddCell(secPrintTime);

            #endregion
            string[] strTitle2 = { "提單編號", "投保通知書編號", "案號或採購文案", "投保金額", "軍種", "交付條件", "軍售或商購", "進口日期" };
            string[] bldlist = Session["G14_bld_no"].ToString().Split(',');
            for (var i = 0; i < strTitle2.Length; i++)
            {
                Secondtable.AddCell(new Phrase(strTitle2[i], ChFont));
            }
            for(var i =0; i< GVTBGMT_IINN.Rows.Count; i++)
            {
                Secondtable.AddCell(new Phrase(bldlist[i], ChFont));
                Secondtable.AddCell(new Phrase(GVTBGMT_IINN.Rows[i].Cells[1].Text.Replace("&nbsp;", ""),ChFont));
                Secondtable.AddCell(new Phrase(GVTBGMT_IINN.Rows[i].Cells[2].Text.Replace("&nbsp;", ""),ChFont));
                Secondtable.AddCell(new Phrase(GVTBGMT_IINN.Rows[i].Cells[3].Text.Replace("&nbsp;", ""),ChFont));
                Secondtable.AddCell(new Phrase(GVTBGMT_IINN.Rows[i].Cells[4].Text.Replace("&nbsp;", ""),ChFont));
                Secondtable.AddCell(new Phrase(GVTBGMT_IINN.Rows[i].Cells[5].Text.Replace("&nbsp;", ""),ChFont));
                Secondtable.AddCell(new Phrase(GVTBGMT_IINN.Rows[i].Cells[6].Text.Replace("&nbsp;", ""),ChFont));
                Secondtable.AddCell(new Phrase(GVTBGMT_IINN.Rows[i].Cells[7].Text.Replace("&nbsp;", ""), ChFont));
            }
            doc1.Add(Secondtable);
            doc1.Close();

            string fileName = HttpUtility.UrlEncode("進口軍售商購案已投保統計表.pdf");
            Response.Clear();//瀏覽器上顯示
            Response.AddHeader("Content-disposition", "attachment;filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.Flush();
            Response.End();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string strODT_START_DATE_S = txtODT_START_DATE_S.Text;
            string strODT_START_DATE_E = txtODT_START_DATE_E.Text;
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedItem.Text;
            string strOVC_PURCHASE_TYPE = drpOVC_PURCHASE_TYPE.SelectedItem.Text;
            string strOVC_IS_PAY = drpOVC_IS_PAY.SelectedItem.Text;

            if(strOVC_IS_PAY == "已投保")
            {
                GVTBGMT_IINN.Columns.Clear();
                string[] HeaderText = { "提單編號", "投保通知書編號", "案號或採購文號", "投保金額", "軍種", "交貨條件", "軍售或商購", "進口日期" };
                string[] DataField = { "OVC_BLD_NO", "OVC_IINN_NO", "OVC_PURCH_NO", "ONB_ITEM_VALUE", "OVC_CLASS_NAME", "OVC_DELIVERY_CONDITION", "OVC_PURCHASE_TYPE", "ODT_IMPORT_DATE" };

                //新增標題列
                BoundField column1 = new BoundField();
                column1.HeaderText = HeaderText[0];
                GVTBGMT_IINN.Columns.Add(column1);

                for (var i =1; i < HeaderText.Length; i++)
                {
                    BoundField column = new BoundField();
                    column.HeaderText = HeaderText[i];
                    column.DataField = DataField[i];
                    
                    if (i == HeaderText.Length - 1)
                        column.DataFormatString = "{0:yyyy/MM/dd}";
                    GVTBGMT_IINN.Columns.Add(column);
                }

                DataTable dt = new DataTable();
                var query =
                    from iinn in MTS.TBGMT_IINN
                    join icr in MTS.TBGMT_ICR on iinn.OVC_BLD_NO equals icr.OVC_BLD_NO into p1
                    from icr in p1.DefaultIfEmpty()
                    join bld in MTS.TBGMT_BLD on iinn.OVC_BLD_NO equals bld.OVC_BLD_NO into p2
                    from bld in p2.DefaultIfEmpty()
                    join dept_class in MTS.TBGMT_DEPT_CLASS on iinn.OVC_MILITARY_TYPE equals dept_class.OVC_CLASS into p3
                    from dept_class in p3.DefaultIfEmpty()
                    select new
                    {
                        OVC_BLD_NO = iinn.OVC_BLD_NO,
                        OVC_IINN_NO = iinn.OVC_IINN_NO,
                        OVC_PURCH_NO = iinn.OVC_PURCH_NO,
                        ONB_ITEM_VALUE = iinn.ONB_ITEM_VALUE,
                        OVC_CLASS_NAME = dept_class.OVC_CLASS_NAME,
                        OVC_DELIVERY_CONDITION = iinn.OVC_DELIVERY_CONDITION,
                        OVC_PURCHASE_TYPE = iinn.OVC_PURCHASE_TYPE,
                        ODT_IMPORT_DATE = icr.ODT_IMPORT_DATE,
                    };

                DateTime start = Convert.ToDateTime(strODT_START_DATE_S);
                DateTime end = Convert.ToDateTime(strODT_START_DATE_E);

                if (!strOVC_MILITARY_TYPE.Equals("請選擇"))
                    query = query.Where(table => table.OVC_CLASS_NAME.Equals(strOVC_MILITARY_TYPE));

                if (!strOVC_PURCHASE_TYPE.Equals("不限定"))
                    query = query.Where(table => table.OVC_PURCHASE_TYPE.Contains(strOVC_PURCHASE_TYPE));

                query = query.Where(table => table.ODT_IMPORT_DATE >= start && table.ODT_IMPORT_DATE < end);
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_IINN, dt, strField);

                //新增linkbutton 
                if (dt.Rows.Count > 0)
                {
                    string bld_no = dt.Rows[0][0].ToString();
                    for(var i =0; i < dt.Rows.Count; i++)
                    {
                        LinkButton bld_link = new LinkButton();
                        bld_link.Text = dt.Rows[i][0].ToString();
                        bld_link.Attributes.Add("href", "javascript:var win=window.open('BLDDATA.aspx?OVC_BLD_NO="+ dt.Rows[i][0].ToString() + "',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=600,height=700,left=0,top=0');");
                        GVTBGMT_IINN.Rows[i].Cells[0].Controls.Add(bld_link);
                        if (i > 0)
                            bld_no += ","+dt.Rows[i][0].ToString();
                    }
                    Session["G14_bld_no"] = bld_no;
                }
               
                if (GVTBGMT_IINN.Rows.Count > 0)
                {
                    btnPrint2.Visible = false;
                    btnPrint.Visible = true;
                }
                else
                {
                    btnPrint.Visible = false;
                    btnPrint2.Visible = false;
                }
            }
            if (strOVC_IS_PAY == "未投保")
            {
                GVTBGMT_IINN.Columns.Clear();
                string[] HeaderText = { "提單編號", "案號或採購文號", "軍種", "進口日期" };
                string[] DataField = { "OVC_BLD_NO", "OVC_PURCH_NO", "OVC_CLASS_NAME",  "ODT_IMPORT_DATE" };

                //新增標題列
                BoundField column1 = new BoundField();
                column1.HeaderText = HeaderText[0];
                GVTBGMT_IINN.Columns.Add(column1);

                for (var i = 1; i < HeaderText.Length; i++)
                {
                    BoundField column = new BoundField();
                    column.HeaderText = HeaderText[i];
                    column.DataField = DataField[i];
                    if (i == HeaderText.Length - 1)
                        column.DataFormatString = "{0:yyyy/MM/dd}";
                    GVTBGMT_IINN.Columns.Add(column);

                    DataTable dt2 = new DataTable();

                    var query2 =
                        from icr in MTS.TBGMT_ICR
                        join bld in MTS.TBGMT_BLD on icr.OVC_BLD_NO equals bld.OVC_BLD_NO into p1
                        from bld in p1.DefaultIfEmpty()
                        join dept_class in MTS.TBGMT_DEPT_CLASS on bld.OVC_MILITARY_TYPE equals dept_class.OVC_CLASS into p2
                        from dept_class in p2.DefaultIfEmpty()
                        join iinn in MTS.TBGMT_IINN on bld.OVC_BLD_NO equals iinn.OVC_BLD_NO into p3
                        from iinn in p3.DefaultIfEmpty()
                        select new
                        {
                            OVC_BLD_NO = icr.OVC_BLD_NO,
                            OVC_PURCH_NO = icr.OVC_PURCH_NO,
                            OVC_CLASS_NAME = dept_class.OVC_CLASS_NAME,
                            ODT_IMPORT_DATE = icr.ODT_IMPORT_DATE,
                            OVC_PURCHASE_TYPE = iinn.OVC_PURCHASE_TYPE,
                            OVC_IINN_NO = iinn.OVC_IINN_NO,
                        };
                    if (!strOVC_MILITARY_TYPE.Equals("請選擇"))
                        query2 = query2.Where(table => table.OVC_CLASS_NAME.Equals(strOVC_MILITARY_TYPE));

                    if (!strOVC_PURCHASE_TYPE.Equals("不限定"))
                        query2 = query2.Where(table => table.OVC_PURCHASE_TYPE.Contains(strOVC_PURCHASE_TYPE));

                    DateTime start = Convert.ToDateTime(strODT_START_DATE_S);
                    DateTime end = Convert.ToDateTime(strODT_START_DATE_E);

                    query2 = query2.Where(table => table.ODT_IMPORT_DATE >= start && table.ODT_IMPORT_DATE < end);
                    query2 = query2.Where(table => table.OVC_IINN_NO.Equals(null));
                    if (query2.Count() > 1000)
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                    query2 = query2.Take(1000);
                    dt2 = CommonStatic.LinqQueryToDataTable(query2);
                    ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_IINN, dt2, strField2);

                    if (dt2.Rows.Count > 0)
                    {
                        string bld_no = dt2.Rows[0][0].ToString();
                        for (var temp = 0; temp < dt2.Rows.Count; temp++)
                        {
                            LinkButton bld_link = new LinkButton();
                            bld_link.Text = dt2.Rows[temp][0].ToString();
                            bld_link.Attributes.Add("href", "javascript:var win=window.open('BLDDATA.aspx?OVC_BLD_NO=" + dt2.Rows[temp][0].ToString() + "',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=600,height=700,left=0,top=0');");
                            GVTBGMT_IINN.Rows[temp].Cells[0].Controls.Add(bld_link);
                            if(temp>0)
                                bld_no += ","+dt2.Rows[temp][0].ToString();
                        }
                        Session["G14_bld_no"] = bld_no;
                    }

                    if (GVTBGMT_IINN.Rows.Count > 0)
                    {
                        btnPrint.Visible = false;
                        btnPrint2.Visible = true;
                    }
                    else
                    {
                        btnPrint.Visible = false;
                        btnPrint2.Visible = false;
                    }
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
    }
}