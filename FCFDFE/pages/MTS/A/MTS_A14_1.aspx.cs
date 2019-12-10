using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FCFDFE.pages.MTS.A
{
    public class Header : PdfPageEventHelper
    {
        PdfContentByte cb;

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            //設定字型
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H
          , BaseFont.NOT_EMBEDDED);
            Font ChFont = new Font(bfChinese, 8, Font.NORMAL, BaseColor.BLACK);
            string chFontPath = "c:\\windows\\fonts\\KAIU.TTF";
            BaseFont chBaseFont = BaseFont.CreateFont(chFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            //每頁標頭
            PdfContentByte cb = writer.DirectContent;
            Rectangle pageSize = document.PageSize;
            DateTime PrintTime = DateTime.Now;
            cb.SetRGBColorFill(0, 0, 0);
            cb.BeginText();
            cb.SetFontAndSize(chBaseFont, 8);

            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "製表人：               聯絡電話：", pageSize.GetLeft(42), pageSize.GetTop(40), 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "製表日期：" + PrintTime.ToString(" yyyy年MM月dd日"), pageSize.GetRight(42), pageSize.GetTop(40), 0);
            cb.EndText();
        }
    }

    public partial class MTS_A14_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();

        #region 副程式
        private void dateImport()
        {
            string strMessage = "";
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_TRANSER_DEPT_CDE = drpOVC_TRANSER_DEPT_CDE.SelectedValue;

            string strOVC_SHIP_NAME = txtOVC_SHIP_NAME.Text;
            string strOVC_VOYAGE = txtOVC_VOYAGE.Text;
            string strOVC_DEPT_CDE = txtOVC_DEPT_CDE.Value;
            string strOVC_IS_SECURITY = drpOVC_IS_SECURITY.SelectedValue;

            ViewState["OVC_BLD_NO"] = strOVC_BLD_NO;
            ViewState["OVC_TRANSER_DEPT_CDE"] = strOVC_TRANSER_DEPT_CDE;
            ViewState["OVC_SHIP_NAME"] = strOVC_SHIP_NAME;
            ViewState["OVC_VOYAGE"] = strOVC_VOYAGE;
            ViewState["OVC_DEPT_CDE"] = strOVC_DEPT_CDE;
            ViewState["OVC_IS_SECURITY"] = strOVC_IS_SECURITY;


            if (strMessage.Equals(string.Empty))
            {
                //var query =
                //    from bld in MTSE.TBGMT_BLD.AsEnumerable()
                //    join icr in MTSE.TBGMT_ICR.AsEnumerable() on bld.OVC_BLD_NO equals icr.OVC_BLD_NO
                //    // join dept_cde in MTSE.TBGMT_DEPT_CDE on icr.OVC_RECEIVE_DEPT_CODE equals dept_cde.OVC_DEPT_CODE
                //    join chn in GME.TBMDEPTs.AsEnumerable() on icr.OVC_RECEIVE_DEPT_CODE equals chn.OVC_DEPT_CDE into ps
                //    from chn in ps.DefaultIfEmpty()
                //    join arrport in MTSE.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals arrport.OVC_PORT_CDE
                //    where arrport.OVC_IS_ABROAD.Equals("國內")
                //    select new
                //    {
                //        OVC_BLD_NO = bld.OVC_BLD_NO,
                //        OVC_PURCH_NO = icr.OVC_PURCH_NO,
                //        OVC_CHI_NAME = icr.OVC_CHI_NAME,
                //        OVC_RECEIVE_DEPT_CODE = chn != null ? chn.OVC_ONNAME : "",
                //        ODT_RECEIVE_INF_DATE = FCommon.getDateTime(icr.ODT_RECEIVE_INF_DATE),
                //        ODT_IMPORT_DATE = FCommon.getDateTime(icr.ODT_IMPORT_DATE),
                //        ODT_ABROAD_CUSTOM_DATE = FCommon.getDateTime(icr.ODT_ABROAD_CUSTOM_DATE),
                //        ODT_CHANGE_BLD_DATE = FCommon.getDateTime(icr.ODT_CHANGE_BLD_DATE),
                //        ODT_CUSTOM_DATE = FCommon.getDateTime(icr.ODT_CUSTOM_DATE),
                //        OVC_IS_SECURITY_Value = bld.OVC_IS_SECURITY ?? 0,
                //        OVC_IS_SECURITY = (bld.OVC_IS_SECURITY ?? 0) == 0 ? "否" : "是",
                //        OVC_START_PORT = bld.OVC_START_PORT ?? "",
                //        OVC_SHIP_NAME = bld.OVC_SHIP_NAME ?? "",
                //        OVC_VOYAGE = bld.OVC_VOYAGE ?? ""
                //    };
                var query =
                    from bld in MTSE.TBGMT_BLD.AsEnumerable()
                    join icr in MTSE.TBGMT_ICR.AsEnumerable() on bld.OVC_BLD_NO equals icr.OVC_BLD_NO
                    // join dept_cde in MTSE.TBGMT_DEPT_CDE on icr.OVC_RECEIVE_DEPT_CODE equals dept_cde.OVC_DEPT_CODE
                    join chn in GME.TBMDEPTs.AsEnumerable() on icr.OVC_RECEIVE_DEPT_CODE equals chn.OVC_DEPT_CDE into ps
                    from chn in ps.DefaultIfEmpty()
                    join arrport in MTSE.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals arrport.OVC_PORT_CDE
                    where arrport.OVC_IS_ABROAD.Equals("國內")
                    select new
                    {
                        OVC_BLD_NO = bld.OVC_BLD_NO,
                        OVC_PURCH_NO = icr.OVC_PURCH_NO,
                        OVC_CHI_NAME = icr.OVC_CHI_NAME,
                        OVC_RECEIVE_DEPT_CODE = "",
                        ODT_RECEIVE_INF_DATE = FCommon.getDateTime(icr.ODT_RECEIVE_INF_DATE),
                        ODT_IMPORT_DATE = FCommon.getDateTime(icr.ODT_IMPORT_DATE),
                        ODT_ABROAD_CUSTOM_DATE = FCommon.getDateTime(icr.ODT_ABROAD_CUSTOM_DATE),
                        ODT_CHANGE_BLD_DATE = FCommon.getDateTime(icr.ODT_CHANGE_BLD_DATE),
                        ODT_CUSTOM_DATE = FCommon.getDateTime(icr.ODT_CUSTOM_DATE),
                        OVC_IS_SECURITY_Value = bld.OVC_IS_SECURITY ?? 0,
                        OVC_IS_SECURITY = (bld.OVC_IS_SECURITY ?? 0) == 0 ? "否" : "是",
                        OVC_START_PORT = bld.OVC_START_PORT ?? "",
                        OVC_SHIP_NAME = bld.OVC_SHIP_NAME ?? "",
                        OVC_VOYAGE = bld.OVC_VOYAGE ?? ""
                    };
                if (!strOVC_BLD_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
                if (!strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                {
                    var table = GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("TR"))
                        .Where(t => t.OVC_PHR_PARENTS.Equals(strOVC_TRANSER_DEPT_CDE))
                        .Select(t => t.OVC_PHR_ID).ToArray();

                    query = query.Where(t => table.Contains(t.OVC_START_PORT));
                }
                if (!strOVC_SHIP_NAME.Equals(string.Empty))
                    query = query.Where(table => table.OVC_SHIP_NAME.Contains(strOVC_SHIP_NAME));
                if (!strOVC_VOYAGE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_VOYAGE.Contains(strOVC_VOYAGE));
                if (!strOVC_DEPT_CDE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_RECEIVE_DEPT_CODE.Contains(strOVC_DEPT_CDE));
                if (!strOVC_IS_SECURITY.Equals(string.Empty) && decimal.TryParse(strOVC_IS_SECURITY, out decimal decOVC_IS_SECURITY))
                    query = query.Where(table => table.OVC_IS_SECURITY_Value == decOVC_IS_SECURITY);
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnWarning, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);

                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_ICR, dt);
                ViewState["dt"] = dt;
                btnPrint.Visible = dt.Rows.Count > 0;
                //if (dt.Rows.Count > 0)
                //    btnPrint.CssClass = btnPrint.CssClass.Replace(" hidden", "");
                //else
                //    btnPrint.CssClass += " hidden";
            }
            else
                FCommon.AlertShow(PnWarning, "danger", "系統訊息", strMessage);
        }
        //public static Control FindControlRecursive(Control Root, string Id)
        //{
        //    if (Root.ID.Equals(Id)) { return Root; }

        //    foreach (Control Ctl in Root.Controls)
        //    {
        //        Control FoundCtl = FindControlRecursive(Ctl, Id);
        //        if (FoundCtl != null) { return FoundCtl; }
        //    }
        //    return null;
        //}
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", ViewState["OVC_BLD_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", ViewState["OVC_TRANSER_DEPT_CDE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_SHIP_NAME", ViewState["OVC_SHIP_NAME"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_VOYAGE", ViewState["OVC_VOYAGE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_DEPT_CDE", ViewState["OVC_DEPT_CDE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_IS_SECURITY", ViewState["OVC_IS_SECURITY"], true);
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
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_DEPT_CDE, txtOVC_ONNAME);
                    #region 匯入下拉式選單
                    string strFirstText = "不限定";
                    CommonMTS.list_dataImport_TransferArea(drpOVC_TRANSER_DEPT_CDE, true, strFirstText); //接轉地區
                    CommonMTS.list_dataImport_IS_SECURITY(drpOVC_IS_SECURITY, true, strFirstText);//機敏軍品
                    #endregion

                    bool isImport = false;
                    string strOVC_BLD_NO, strOVC_TRANSER_DEPT_CDE, strOVC_SHIP_NAME, strOVC_VOYAGE, strOVC_DEPT_CDE, strOVC_IS_SECURITY;
                    if (FCommon.getQueryString(this, "OVC_BLD_NO", out strOVC_BLD_NO, true))
                    {
                        txtOVC_BLD_NO.Text = strOVC_BLD_NO;
                        isImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_TRANSER_DEPT_CDE", out strOVC_TRANSER_DEPT_CDE, true))
                        FCommon.list_setValue(drpOVC_TRANSER_DEPT_CDE, strOVC_TRANSER_DEPT_CDE);

                    if (FCommon.getQueryString(this, "OVC_SHIP_NAME", out strOVC_SHIP_NAME, true))
                        txtOVC_SHIP_NAME.Text = strOVC_SHIP_NAME;
                    if (FCommon.getQueryString(this, "OVC_VOYAGE", out strOVC_VOYAGE, true))
                        txtOVC_VOYAGE.Text = strOVC_VOYAGE;
                    if (FCommon.getQueryString(this, "OVC_DEPT_CDE", out strOVC_DEPT_CDE, true))
                    {
                        txtOVC_DEPT_CDE.Value = strOVC_DEPT_CDE;
                        TBMDEPT detp = GME.TBMDEPTs.Where(t => t.OVC_DEPT_CDE.Equals(strOVC_DEPT_CDE)).FirstOrDefault();
                        if (detp != null) txtOVC_ONNAME.Text = detp.OVC_ONNAME;
                    }
                    if (FCommon.getQueryString(this, "OVC_IS_SECURITY", out strOVC_IS_SECURITY, true))
                    {
                        FCommon.list_setValue(drpOVC_IS_SECURITY, strOVC_IS_SECURITY);
                        //drpOVC_IS_SECURITY.SelectedIndex = 0;
                    }
                    if (isImport)
                        dateImport();
                }
            }
        }

        protected void btnResettxtOVC_DEPT_CDE_Click(object sender, EventArgs e)
        {
            txtOVC_DEPT_CDE.Value = string.Empty;
            txtOVC_ONNAME.Text = string.Empty;
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dateImport();
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (ViewState["hasRows"] != null)
            {
                Document doc = new Document(PageSize.A4, 10, 10, 50, 20);
                MemoryStream Memory = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(doc, Memory);
                // PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(@"C:\Users\Jonathan\Desktop\test.pdf", FileMode.Create)); //指定路徑創造

                writer.PageEvent = new Header();
                doc.Open();

                //設定字型
                BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H
              , BaseFont.NOT_EMBEDDED);
                Font ChFont = new Font(bfChinese, 6, Font.NORMAL, BaseColor.BLACK);
                string chFontPath = "c:\\windows\\fonts\\KAIU.TTF";
                BaseFont chBaseFont = BaseFont.CreateFont(chFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                PdfContentByte cb = writer.DirectContent;
                Rectangle pageSize = doc.PageSize;
                cb.SetRGBColorFill(0, 0, 0);
                cb.BeginText();
                cb.SetFontAndSize(chBaseFont, 10);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "國防部國防採購室進口管理簿", pageSize.GetLeft(300), pageSize.GetTop(25), 0);
                cb.EndText();

                PdfPTable pdftable = new PdfPTable(new float[] { 2, 5, 4, 6, 6, 6, 6, 6, 6, 3, 4 });
                pdftable.TotalWidth = 580f;
                pdftable.LockedWidth = true;
                pdftable.DefaultCell.FixedHeight = 50;


                string[] title = { "項次", "船名", "航次", "提單號碼", "收到貨通知書日期", "進口日期", "收國外報關文件日期", "換小提單日期", "報關日期", "機敏軍品", "備考" };
                for (int i = 0; i < title.Length; i++)
                {
                    PdfPCell cell_title = new PdfPCell(new Phrase(title[i], ChFont));
                    cell_title.FixedHeight = 50;
                    cell_title.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_title.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdftable.AddCell(cell_title);
                }
                if (ViewState["dt"] != null)
                {
                    DataTable print_query = (DataTable)ViewState["dt"];

                    for (int i = 0; i < print_query.Rows.Count; i++)
                    {
                        PdfPCell cell_num = new PdfPCell(new Phrase((i + 1).ToString(), ChFont));
                        cell_num.FixedHeight = 50;
                        cell_num.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell_num.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdftable.AddCell(cell_num);
                        string bld_no = print_query.Rows[i][0].ToString();

                        TBGMT_BLD codetable = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(bld_no)).FirstOrDefault();


                        PdfPCell cell_shipname = new PdfPCell(new Phrase(codetable.OVC_SHIP_NAME, ChFont));
                        cell_shipname.FixedHeight = 50;
                        cell_shipname.VerticalAlignment = Element.ALIGN_MIDDLE;


                        pdftable.AddCell(cell_shipname);
                        PdfPCell cell_voyage = new PdfPCell(new Phrase(codetable.OVC_VOYAGE, ChFont));
                        cell_voyage.FixedHeight = 50;
                        cell_voyage.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdftable.AddCell(cell_voyage);
                        PdfPCell cell_ordernum = new PdfPCell(new Phrase(print_query.Rows[i][0].ToString(), ChFont));
                        cell_ordernum.FixedHeight = 50;
                        cell_ordernum.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdftable.AddCell(cell_ordernum);
                        for (int u = 4; u <= 8; u++)
                        {
                            if (print_query.Rows[i][u].ToString() != "")//設定日期格式
                            {
                                PdfPCell cell_date = new PdfPCell(new Phrase(FCommon.getDateTime(print_query.Rows[i][u]), ChFont));
                                cell_date.FixedHeight = 50;
                                cell_date.HorizontalAlignment = Element.ALIGN_CENTER;
                                cell_date.VerticalAlignment = Element.ALIGN_MIDDLE;
                                pdftable.AddCell(cell_date);
                            }
                            else
                            {
                                PdfPCell cell_date = new PdfPCell(new Phrase("", ChFont));
                                cell_date.FixedHeight = 50;
                                pdftable.AddCell(cell_date);
                            }
                        }
                        string strOVC_IS_SECURITY = (codetable.OVC_IS_SECURITY ?? 0) == 0 ? "否" : "是";
                        PdfPCell cell_security = new PdfPCell(new Phrase(strOVC_IS_SECURITY, ChFont));
                        cell_security.FixedHeight = 50;
                        cell_security.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell_security.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdftable.AddCell(cell_security);

                        //if (codetable2 != null)
                        //{
                        //    PdfPCell cell_note = new PdfPCell(new Phrase(codetable2.OVC_NOTE, ChFont));
                        //    cell_note.FixedHeight = 50;
                        //    cell_note.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //    pdftable.AddCell(cell_note);
                        //}
                        //else
                        //{
                        //    PdfPCell cell_note = new PdfPCell();
                        //    cell_note.FixedHeight = 50;
                        //    pdftable.AddCell(cell_note);
                        //}

                        //備考left join

                        var query =
                        from bld in MTSE.TBGMT_BLD
                        join icr in MTSE.TBGMT_ICR_MRPLOG on bld.OVC_BLD_NO equals icr.OVC_BLD_NO into note
                        from n in note.DefaultIfEmpty()
                        select new
                        {
                            OVC_BLD_NO = bld.OVC_BLD_NO,
                            OVC_NOTE = n.OVC_NOTE
                        };
                        query = query.Where(table => table.OVC_BLD_NO.Equals(bld_no));
                        DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                        PdfPCell cell_note = new PdfPCell(new Phrase(dt.Rows[0][1].ToString(), ChFont));
                        cell_note.FixedHeight = 50;
                        cell_note.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdftable.AddCell(cell_note);
                        //
                    }
                }
                doc.Add(pdftable);
                doc.Close();

                string strFileName = $"國防部國防採購室進口管制簿.pdf";
                FCommon.DownloadFile(this, strFileName, Memory);
            }
        }
        
        protected void GVTBGMT_ICR_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GVTBGMT_ICR.DataKeys[gvrIndex].Value.ToString();

            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);
            switch (e.CommandName)
            {
                case "btnModify":
                    Response.Redirect($"MTS_A14_2{ strQueryString }");
                    break;
                case "btnDel":
                    Response.Redirect($"MTS_A14_3{ strQueryString }");
                    break;
                default:
                    break;
            }
        }
        protected void GVTBGMT_ICR_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void GVTBGMT_ICR_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}