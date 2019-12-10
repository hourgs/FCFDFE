using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace FCFDFE.pages.MTS.C
{
    public partial class MTS_C15_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        DateTime dateNow = DateTime.Now;

        #region 副程式
        private void dataImport()
        {
            string strMessage = "";
            string strOVC_RECLAIM_NO = txtOVC_RECLAIM_NO.Text;
            string strOVC_DEPT_CDE = txtOVC_DEPT_CDE.Value;
            //string strOVC_ONNAME = txtOVC_ONNAME.Text;
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_APPROVE_STATUS = drpOVC_APPROVE_STATUS.SelectedValue;
            string strOVC_APPLY_DATE_S = txtOVC_APPLY_DATE_S.Text;
            string strOVC_APPLY_DATE_E = txtOVC_APPLY_DATE_E.Text;

            ViewState["OVC_RECLAIM_NO"] = strOVC_RECLAIM_NO;
            ViewState["OVC_DEPT_CDE"] = strOVC_DEPT_CDE;
            //ViewState["OVC_ONNAME"] = strOVC_ONNAME;
            ViewState["OVC_BLD_NO"] = strOVC_BLD_NO;
            ViewState["OVC_APPROVE_STATUS"] = strOVC_APPROVE_STATUS;
            ViewState["OVC_APPLY_DATE_S"] = strOVC_APPLY_DATE_S;
            ViewState["OVC_APPLY_DATE_E"] = strOVC_APPLY_DATE_E;

            if (strOVC_RECLAIM_NO.Equals(string.Empty))
                strMessage += "<p> 請輸入 保留索賠權編號！ </p>";
            //if (strOVC_APPROVE_STATUS.Equals(string.Empty))
            //    strMessage += "<p> 請選取 作業進度！ </p>";
            bool boolOVC_APPLY_DATE = !strOVC_APPLY_DATE_S.Equals(string.Empty) || !strOVC_APPLY_DATE_E.Equals(string.Empty);
            bool boolOVC_APPLY_DATE_S = DateTime.TryParse(strOVC_APPLY_DATE_S, out DateTime dateOVC_APPLY_DATE_S);
            bool boolOVC_APPLY_DATE_E = DateTime.TryParse(strOVC_APPLY_DATE_E, out DateTime dateOVC_APPLY_DATE_E);
            if (boolOVC_APPLY_DATE && !(boolOVC_APPLY_DATE_S && boolOVC_APPLY_DATE_E))
                strMessage += "<P> 申請日期 不完全！ </p>";

            if (strMessage.Equals(string.Empty))
            {
                var query =
                    from reserve in MTSE.TBGMT_CLAIM_RESERVE.AsEnumerable()
                    where reserve.OVC_RECLAIM_NO.Contains(strOVC_RECLAIM_NO)
                    join dept in MTSE.TBMDEPTs on reserve.OVC_MILITARY_TYPE equals dept.OVC_DEPT_CDE
                    join claim in MTSE.TBGMT_CLAIM on reserve.OVC_INN_NO equals claim.OVC_INN_NO into temp
                    from claim in temp.DefaultIfEmpty()
                    select new
                    {
                        RECLAIM_SN = reserve.RECLAIM_SN,
                        OVC_APPROVE_STATUS = reserve.OVC_APPROVE_STATUS ?? "",
                        OVC_APPLY_DATE = reserve.OVC_APPLY_DATE.ToString(),
                        OVC_BLD_NO = reserve.OVC_BLD_NO ?? "",
                        OVC_ONNAME = dept.OVC_ONNAME,
                        OVC_RECLAIM_NO = reserve.OVC_RECLAIM_NO,
                        OVC_CLAIM_MSG_NO = reserve.OVC_CLAIM_MSG_NO,
                        OVC_CLAIM_DATE = FCommon.getDateTime(reserve.OVC_CLAIM_DATE),
                        OVC_CLAIM_ITEM = reserve.OVC_CLAIM_ITEM,
                        OVC_CLAIM_REASON = reserve.OVC_CLAIM_REASON,
                        ONB_CLAIM_NUMBER = reserve.ONB_CLAIM_NUMBER,
                        ONB_CLAIM_AMOUNT = reserve.ONB_CLAIM_AMOUNT,
                        OVC_NOTE = reserve.OVC_NOTE,
                        OVC_MILITARY_TYPE = reserve.OVC_MILITARY_TYPE ?? "",
                        OVC_PURCH_NO = claim == null ? "" : claim.OVC_PURCH_NO,
                    };
                if (!strOVC_DEPT_CDE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_MILITARY_TYPE.Equals(strOVC_DEPT_CDE));
                if (!strOVC_BLD_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
                if (!strOVC_APPROVE_STATUS.Equals(string.Empty))
                    query = query.Where(table => table.OVC_APPROVE_STATUS.Equals(strOVC_APPROVE_STATUS));
                if (boolOVC_APPLY_DATE)
                    query = query.Where(table => DateTime.TryParse(table.OVC_APPLY_DATE, out DateTime dateOVC_APPLY_DATE) &&
                        DateTime.Compare(dateOVC_APPLY_DATE, dateOVC_APPLY_DATE_S) >= 0 &&
                        DateTime.Compare(dateOVC_APPLY_DATE, dateOVC_APPLY_DATE_E) <= 0);
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_CLAIM_RESERVE, dt);
            }
            else
                FCommon.AlertShow(pnMessageQuery, "danger", "系統訊息", strMessage);
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_RECLAIM_NO", ViewState["OVC_RECLAIM_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_DEPT_CDE", ViewState["OVC_DEPT_CDE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", ViewState["OVC_BLD_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_APPROVE_STATUS", ViewState["OVC_APPROVE_STATUS"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_APPLY_DATE_S", ViewState["OVC_APPLY_DATE_S"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_APPLY_DATE_E", ViewState["OVC_APPLY_DATE_E"], true);
            return strQueryString;
        }
        #endregion
        #region PDF 副程式
        public void Pdf_Details(string strRECLAIM_SN)
        {
            if (Guid.TryParse(strRECLAIM_SN, out Guid guidRECLAIM_SN))
            {
                var query =
                    from claim_reserve in MTSE.TBGMT_CLAIM_RESERVE
                    where claim_reserve.RECLAIM_SN.Equals(guidRECLAIM_SN)
                    join dept in MTSE.TBGMT_DEPT_CDE on claim_reserve.OVC_MILITARY_TYPE equals dept.OVC_DEPT_CODE into tempDept
                    from dept in tempDept.DefaultIfEmpty()
                    join currency in MTSE.TBGMT_CURRENCY on claim_reserve.OVC_CLAIM_CURRENCY equals currency.OVC_CURRENCY_CODE into tempCurrency
                    from currency in tempCurrency.DefaultIfEmpty()

                    select new
                    {
                        OVC_RECLAIM_NO = claim_reserve.OVC_RECLAIM_NO,
                        OVC_DEPT_NAME = dept!=null?dept.OVC_DEPT_NAME:"",
                        OVC_APPLY_DATE = claim_reserve.OVC_APPLY_DATE,
                        OVC_BLD_NO = claim_reserve.OVC_BLD_NO,
                        OVC_IMPORT_DATE = claim_reserve.OVC_IMPORT_DATE,
                        OVC_INN_NO = claim_reserve.OVC_INN_NO,
                        OVC_CLAIM_DATE = claim_reserve.OVC_CLAIM_DATE,
                        OVC_CLAIM_MSG_NO = claim_reserve.OVC_CLAIM_MSG_NO,
                        OVC_CLAIM_REV_DATE = claim_reserve.OVC_CLAIM_REV_DATE,
                        OVC_CLAIM_REV_MSG = claim_reserve.OVC_CLAIM_REV_MSG,
                        OVC_CLAIM_ITEM = claim_reserve.OVC_CLAIM_ITEM,
                        OVC_CLAIM_REASON = claim_reserve.OVC_CLAIM_REASON,
                        ONB_RECEIVE = claim_reserve.ONB_RECEIVE,
                        ONB_ACTUAL_RECEIVE = claim_reserve.ONB_ACTUAL_RECEIVE,
                        ONB_CLAIM_NUMBER = claim_reserve.ONB_CLAIM_NUMBER,
                        ONB_CLAIM_BREAK = claim_reserve.ONB_CLAIM_BREAK,
                        ONB_CLAIM_AMOUNT = claim_reserve.ONB_CLAIM_AMOUNT,
                        OVC_CURRENCY_NAME = currency!=null? currency.OVC_CURRENCY_NAME:"",
                        OVC_NOTE = claim_reserve.OVC_NOTE,

                    };
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    string strOVC_RECLAIM_NO = dr["OVC_RECLAIM_NO"].ToString();

                    #region PDF
                    BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\mingliu.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
                    Font title_ChFont = new Font(bfChinese, 14f, Font.BOLD);
                    Font ChFont = new Font(bfChinese, 12f, Font.BOLD);
                    Font font = new Font(bfChinese, 10f, Font.BOLD);
                    MemoryStream Memory = new MemoryStream();
                    var doc1 = new Document(PageSize.A4, 50, 50, 80, 50);
                    PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
                    DateTime PrintTime = DateTime.Now;

                    doc1.Open();
                    PdfPTable Firsttable = new PdfPTable(4);
                    Firsttable.SetWidths(new float[] { 3, 4, 3, 4 });
                    Firsttable.TotalWidth = 550F;
                    Firsttable.LockedWidth = true;
                    Firsttable.DefaultCell.FixedHeight = 40f;
                    Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    Firsttable.DefaultCell.PaddingLeft = 5f;
                    PdfPCell colspan2 = new PdfPCell();
                    colspan2.Colspan = 2;
                    PdfPCell title = new PdfPCell(new Paragraph("保險索賠權明細表", title_ChFont));
                    title.VerticalAlignment = Element.ALIGN_MIDDLE;
                    title.HorizontalAlignment = Element.ALIGN_CENTER;
                    title.Border = Rectangle.NO_BORDER;
                    title.Colspan = 4;
                    Firsttable.AddCell(title);
                    PdfPCell title_time = new PdfPCell(new Paragraph("列印日期 ： " + FCommon.getDateTime(PrintTime), ChFont));
                    title_time.HorizontalAlignment = Element.ALIGN_RIGHT;
                    title_time.VerticalAlignment = Element.ALIGN_BOTTOM;
                    title_time.Border = Rectangle.NO_BORDER;
                    title_time.PaddingBottom = 5f;
                    title_time.Colspan = 4;
                    Firsttable.AddCell(title_time);
                    Firsttable.AddCell(new Paragraph("保留索賠權編號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_RECLAIM_NO"].ToString(), ChFont));
                    Firsttable.AddCell(colspan2);
                    Firsttable.AddCell(new Paragraph("申  請  單  位", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_DEPT_NAME"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("申  請  日  期", ChFont));
                    Firsttable.AddCell(new Paragraph(FCommon.getDateTime(dr["OVC_APPLY_DATE"]), ChFont));
                    Firsttable.AddCell(new Paragraph("提  單  編  號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_BLD_NO"].ToString(), ChFont));
                    Firsttable.AddCell(colspan2);
                    Firsttable.AddCell(new Paragraph("進  口  日  期", ChFont));
                    Firsttable.AddCell(new Paragraph(FCommon.getDateTime(dr["OVC_IMPORT_DATE"]), ChFont));
                    Firsttable.AddCell(new Paragraph("投保通知書編號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_INN_NO"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("軍種保留索賠權日期", ChFont));
                    Firsttable.AddCell(new Paragraph(FCommon.getDateTime(dr["OVC_CLAIM_DATE"]), ChFont));
                    Firsttable.AddCell(new Paragraph("軍種保留索賠權文號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CLAIM_MSG_NO"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("國防採購室保留索賠權日期", ChFont));
                    Firsttable.AddCell(new Paragraph(FCommon.getDateTime(dr["OVC_CLAIM_REV_DATE"]), ChFont));
                    Firsttable.AddCell(new Paragraph("國防採購室保留索賠權文號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CLAIM_REV_MSG"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("軍  品  名  稱", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CLAIM_ITEM"].ToString(), ChFont));
                    Firsttable.AddCell(colspan2);
                    Firsttable.AddCell(new Paragraph("損  失  原  因", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CLAIM_REASON"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph(" ", ChFont));
                    Firsttable.AddCell(new Paragraph("", ChFont));
                    Firsttable.AddCell(new Paragraph("應  收  件  數", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["ONB_RECEIVE"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("實  收  件  數", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["ONB_ACTUAL_RECEIVE"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("索  賠  件  數", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["ONB_CLAIM_NUMBER"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("破  損  件  數", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["ONB_CLAIM_BREAK"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("索  賠  金  額", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["ONB_CLAIM_AMOUNT"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("幣          別", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CURRENCY_NAME"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("備          註", ChFont));
                    PdfPCell tail_text = new PdfPCell(new Paragraph(dr["OVC_NOTE"].ToString(), ChFont));
                    tail_text.Colspan = 3;
                    tail_text.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tail_text.PaddingLeft = 5f;
                    Firsttable.AddCell(tail_text);
                    doc1.Add(Firsttable);
                    doc1.Close();
                    #endregion

                    string strFileName = $"保險索賠權明細表-{ strOVC_RECLAIM_NO }.pdf";
                    FCommon.DownloadFile(this, strFileName, Memory);
                }
            }
            else
                FCommon.AlertShow(pnMessageQuery, "danger", "系統訊息", "保留索賠權 系統編號錯誤！");
        }
        public void Pdf_Claims(string strRECLAIM_SN)
        {
            if (Guid.TryParse(strRECLAIM_SN, out Guid guidRECLAIM_SN))
            {
                var query =
                    from claim_reserve in MTSE.TBGMT_CLAIM_RESERVE
                        //where claim_reserve.OVC_RECLAIM_NO.Equals(no)
                where claim_reserve.RECLAIM_SN.Equals(guidRECLAIM_SN)
                    join dept in MTSE.TBGMT_DEPT_CDE on claim_reserve.OVC_MILITARY_TYPE equals dept.OVC_DEPT_CODE into ps1
                    from dept in ps1.DefaultIfEmpty()
                    select new
                    {
                        claim_reserve.OVC_RECLAIM_NO,
                        OVC_DEPT_NAME = dept!=null?  dept.OVC_DEPT_NAME:"",//轉國字
                    claim_reserve.OVC_APPLY_DATE,
                        claim_reserve.OVC_BLD_NO,
                        claim_reserve.OVC_IMPORT_DATE,
                        claim_reserve.OVC_INN_NO,
                        claim_reserve.OVC_CLAIM_DATE,
                        claim_reserve.OVC_CLAIM_MSG_NO,
                        claim_reserve.OVC_CLAIM_REV_DATE,
                        claim_reserve.OVC_CLAIM_REV_MSG,
                        claim_reserve.OVC_CLAIM_ITEM,
                        claim_reserve.OVC_CLAIM_REASON,
                        claim_reserve.ONB_RECEIVE,
                        claim_reserve.ONB_ACTUAL_RECEIVE,
                        claim_reserve.ONB_CLAIM_NUMBER,
                        claim_reserve.ONB_CLAIM_BREAK,
                        claim_reserve.ONB_CLAIM_AMOUNT,
                        claim_reserve.OVC_CLAIM_CURRENCY,
                        claim_reserve.ONB_COMPENSATION_AMOUNT,
                        claim_reserve.OVC_COMPENSATION_CURRENCY,
                        claim_reserve.ONB_COMPENSATION_AMOUNT_NTD,
                        claim_reserve.ONB_COMPENSATION_CURRENCY_RATE,
                        claim_reserve.OVC_CLAIM_COM_DATE,
                        claim_reserve.OVC_CLAIM_COM_MSG,
                        claim_reserve.OVC_PURCHASE_DATE,
                        claim_reserve.OVC_PURCHASE_MSG_NO,
                        claim_reserve.OVC_INS_COM_MSG,
                        claim_reserve.OVC_CHEQUE_BANK,
                        claim_reserve.OVC_CHEQUE_NO,
                        claim_reserve.OVC_CHEQUE_TITLE,
                        claim_reserve.OVC_CHEQUE_DATE,
                        claim_reserve.OVC_NOTE,
                        OVC_APPROVE_STATUS = claim_reserve.OVC_APPROVE_STATUS,
                    };

                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    string strOVC_RECLAIM_NO = dr["OVC_RECLAIM_NO"].ToString();

                    BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\mingliu.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
                    Font title_ChFont = new Font(bfChinese, 14f, Font.BOLD);
                    Font ChFont = new Font(bfChinese, 12f, Font.BOLD);
                    Font font = new Font(bfChinese, 10f, Font.BOLD);
                    MemoryStream Memory = new MemoryStream();
                    var doc1 = new Document(PageSize.A4, 50, 50, 40, 50);
                    PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
                    DateTime PrintTime = DateTime.Now;
                    doc1.Open();
                    PdfPTable Firsttable = new PdfPTable(4);
                    Firsttable.SetWidths(new float[] { 3, 4, 3, 4 });
                    Firsttable.TotalWidth = 550F;
                    Firsttable.LockedWidth = true;
                    Firsttable.DefaultCell.FixedHeight = 35f;
                    Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    Firsttable.DefaultCell.PaddingLeft = 5f;
                    PdfPCell colspan2 = new PdfPCell();
                    colspan2.Colspan = 2;
                    PdfPCell title = new PdfPCell(new Paragraph("理賠明細表", title_ChFont));
                    title.VerticalAlignment = Element.ALIGN_MIDDLE;
                    title.HorizontalAlignment = Element.ALIGN_CENTER;
                    title.Border = Rectangle.NO_BORDER;
                    title.Colspan = 4;
                    Firsttable.AddCell(title);
                    PdfPCell title_time = new PdfPCell(new Paragraph("列印日期 ： " + FCommon.getDateTime(PrintTime), ChFont));
                    title_time.HorizontalAlignment = Element.ALIGN_RIGHT;
                    title_time.VerticalAlignment = Element.ALIGN_BOTTOM;
                    title_time.Border = Rectangle.NO_BORDER;
                    title_time.PaddingBottom = 5f;
                    title_time.Colspan = 4;
                    Firsttable.AddCell(title_time);
                    Firsttable.AddCell(new Paragraph("保留索賠權編號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_RECLAIM_NO"].ToString(), ChFont));
                    Firsttable.AddCell(colspan2);
                    Firsttable.AddCell(new Paragraph("申  請  單  位", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_DEPT_NAME"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("申  請  日  期", ChFont));
                    Firsttable.AddCell(new Paragraph(FCommon.getDateTime(dr["OVC_APPLY_DATE"]), ChFont));
                    Firsttable.AddCell(new Paragraph("提  單  編  號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_BLD_NO"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph(" ", ChFont));
                    Firsttable.AddCell(new Paragraph("", ChFont));
                    Firsttable.AddCell(new Paragraph("進  口  日  期", ChFont));
                    Firsttable.AddCell(new Paragraph(FCommon.getDateTime(dr["OVC_IMPORT_DATE"]), ChFont));
                    Firsttable.AddCell(new Paragraph("投保通知書編號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_INN_NO"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("軍種保留索賠權日期", ChFont));
                    Firsttable.AddCell(new Paragraph(FCommon.getDateTime(dr["OVC_CLAIM_DATE"]), ChFont));
                    Firsttable.AddCell(new Paragraph("軍種保留索賠權文號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CLAIM_MSG_NO"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("國防採購室保留索賠權日期", ChFont));
                    Firsttable.AddCell(new Paragraph(FCommon.getDateTime(dr["OVC_CLAIM_REV_DATE"]), ChFont));
                    Firsttable.AddCell(new Paragraph("國防採購室保留索賠權文號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CLAIM_REV_MSG"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("軍  品  名  稱", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CLAIM_ITEM"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("損  失  原  因", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CLAIM_REASON"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("因  收  件  數", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["ONB_RECEIVE"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("實  收  件  數", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["ONB_ACTUAL_RECEIVE"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("索  賠  件  數", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["ONB_CLAIM_NUMBER"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("破  損  件  數", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["ONB_CLAIM_BREAK"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("索  賠  金  額", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["ONB_CLAIM_AMOUNT"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("幣          別", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CLAIM_CURRENCY"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("實際理賠金額", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["ONB_COMPENSATION_AMOUNT"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("幣          別", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_COMPENSATION_CURRENCY"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("實際理賠金額(台幣)", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["ONB_COMPENSATION_AMOUNT_NTD"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("匯          率", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["ONB_COMPENSATION_CURRENCY_RATE"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("軍種申請理賠日期", ChFont));
                    Firsttable.AddCell(new Paragraph(FCommon.getDateTime(dr["OVC_CLAIM_COM_DATE"]), ChFont));
                    Firsttable.AddCell(new Paragraph("軍種申請理賠文號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CLAIM_COM_MSG"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("國防採購室申請理賠日期", ChFont));
                    Firsttable.AddCell(new Paragraph(FCommon.getDateTime(dr["OVC_PURCHASE_DATE"]), ChFont));
                    Firsttable.AddCell(new Paragraph("國防採購室申請理賠文號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_PURCHASE_MSG_NO"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph(" ", ChFont));
                    Firsttable.AddCell(new Paragraph("", ChFont));
                    Firsttable.AddCell(new Paragraph("保險公司理賠文號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_INS_COM_MSG"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("支  票  銀  行", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CHEQUE_BANK"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("支  票  號  碼", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CHEQUE_NO"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("支  票  抬  頭", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CHEQUE_TITLE"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("支  票  日  期", ChFont));
                    Firsttable.AddCell(new Paragraph(FCommon.getDateTime(dr["OVC_CHEQUE_DATE"]), ChFont));
                    Firsttable.AddCell(new Paragraph("備          註", ChFont));
                    PdfPCell colspan3 = new PdfPCell(new Paragraph(dr["OVC_NOTE"].ToString(), ChFont));
                    colspan3.PaddingLeft = 5f;
                    colspan3.VerticalAlignment = Element.ALIGN_MIDDLE;
                    colspan3.Colspan = 3;
                    Firsttable.AddCell(colspan3);
                    doc1.Add(Firsttable);
                    doc1.Close();

                    string strFileName = $"理賠明細表-{ strOVC_RECLAIM_NO }.pdf";
                    FCommon.DownloadFile(this, strFileName, Memory);
                }
            }
            else
                FCommon.AlertShow(pnMessageQuery, "danger", "系統訊息", "保留索賠權 系統編號錯誤！");
        }
        public void Pdf_Reserve_Details(string strRECLAIM_SN)
        {
            if (Guid.TryParse(strRECLAIM_SN, out Guid guidRECLAIM_SN))
            {
                var query =
                    from claim_reserve in MTSE.TBGMT_CLAIM_RESERVE
                    where claim_reserve.RECLAIM_SN.Equals(guidRECLAIM_SN)
                    join dept in MTSE.TBGMT_DEPT_CDE on claim_reserve.OVC_MILITARY_TYPE equals dept.OVC_DEPT_CODE into tepmDept
                    from dept in tepmDept.DefaultIfEmpty()
                    join currency in MTSE.TBGMT_CURRENCY on claim_reserve.OVC_CLAIM_CURRENCY equals currency.OVC_CURRENCY_CODE into tempCurrency
                    from currency in tempCurrency.DefaultIfEmpty()
                    select new
                    {
                        OVC_RECLAIM_NO = claim_reserve.OVC_RECLAIM_NO,
                        OVC_DEPT_NAME = dept != null ? dept.OVC_DEPT_NAME : "",
                        OVC_APPLY_DATE = claim_reserve.OVC_APPLY_DATE,
                        OVC_BLD_NO = claim_reserve.OVC_BLD_NO,
                        OVC_IMPORT_DATE = claim_reserve.OVC_IMPORT_DATE,
                        OVC_INN_NO = claim_reserve.OVC_INN_NO,
                        OVC_CLAIM_DATE = claim_reserve.OVC_CLAIM_DATE,
                        OVC_CLAIM_MSG_NO = claim_reserve.OVC_CLAIM_MSG_NO,
                        OVC_CLAIM_REV_DATE = claim_reserve.OVC_CLAIM_REV_DATE,
                        OVC_CLAIM_REV_MSG = claim_reserve.OVC_CLAIM_REV_MSG,
                        OVC_CLAIM_ITEM = claim_reserve.OVC_CLAIM_ITEM,
                        OVC_CLAIM_REASON = claim_reserve.OVC_CLAIM_REASON,
                        ONB_RECEIVE = claim_reserve.ONB_RECEIVE,
                        ONB_ACTUAL_RECEIVE = claim_reserve.ONB_ACTUAL_RECEIVE,
                        ONB_CLAIM_NUMBER = claim_reserve.ONB_CLAIM_NUMBER,
                        ONB_CLAIM_BREAK = claim_reserve.ONB_CLAIM_BREAK,
                        ONB_CLAIM_AMOUNT = claim_reserve.ONB_CLAIM_AMOUNT,
                        OVC_CURRENCY_NAME = currency != null ? currency.OVC_CURRENCY_NAME : "",
                        OVC_NOTE = claim_reserve.OVC_NOTE,
                    };
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    string strOVC_RECLAIM_NO = dr["OVC_RECLAIM_NO"].ToString();

                    BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\mingliu.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
                    Font title_ChFont = new Font(bfChinese, 14f, Font.BOLD);
                    Font ChFont = new Font(bfChinese, 12f, Font.BOLD);
                    Font font = new Font(bfChinese, 10f, Font.BOLD);
                    MemoryStream Memory = new MemoryStream();
                    var doc1 = new Document(PageSize.A4, 50, 50, 80, 50);
                    PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
                    DateTime PrintTime = DateTime.Now;
                    doc1.Open();
                    PdfPTable Firsttable = new PdfPTable(4);
                    Firsttable.SetWidths(new float[] { 3, 4, 3, 4 });
                    Firsttable.TotalWidth = 550F;
                    Firsttable.LockedWidth = true;
                    Firsttable.DefaultCell.FixedHeight = 40f;
                    Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    Firsttable.DefaultCell.PaddingLeft = 5f;
                    PdfPCell colspan2 = new PdfPCell();
                    colspan2.Colspan = 2;
                    PdfPCell colspan3 = new PdfPCell(new Paragraph("", ChFont));
                    colspan3.PaddingLeft = 5f;
                    colspan3.VerticalAlignment = Element.ALIGN_MIDDLE;
                    colspan3.Colspan = 3;
                    PdfPCell title = new PdfPCell(new Paragraph("保留索賠權撤銷明細表", title_ChFont));
                    title.VerticalAlignment = Element.ALIGN_MIDDLE;
                    title.HorizontalAlignment = Element.ALIGN_CENTER;
                    title.Border = Rectangle.NO_BORDER;
                    title.Colspan = 4;
                    Firsttable.AddCell(title);
                    PdfPCell title_time = new PdfPCell(new Paragraph("列印日期 ： " + FCommon.getDateTime(PrintTime), ChFont));
                    title_time.HorizontalAlignment = Element.ALIGN_RIGHT;
                    title_time.VerticalAlignment = Element.ALIGN_BOTTOM;
                    title_time.Border = Rectangle.NO_BORDER;
                    title_time.PaddingBottom = 5f;
                    title_time.Colspan = 4;
                    Firsttable.AddCell(title_time);
                    Firsttable.AddCell(new Paragraph("保留索賠權編號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_RECLAIM_NO"].ToString(), ChFont));
                    Firsttable.AddCell(colspan2);
                    Firsttable.AddCell(new Paragraph("申  請  單  位", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_DEPT_NAME"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("申  請  日  期", ChFont));
                    Firsttable.AddCell(new Paragraph(FCommon.getDateTime(dr["OVC_APPLY_DATE"]), ChFont));
                    Firsttable.AddCell(new Paragraph("提  單  編  號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_BLD_NO"].ToString(), ChFont));
                    Firsttable.AddCell(colspan2);
                    Firsttable.AddCell(new Paragraph("進  口  日  期", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_IMPORT_DATE"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("投保通知書編號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_INN_NO"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("軍種保留索賠權日期", ChFont));
                    Firsttable.AddCell(new Paragraph(FCommon.getDateTime(dr["OVC_CLAIM_DATE"]), ChFont));
                    Firsttable.AddCell(new Paragraph("軍種保留索賠權文號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CLAIM_MSG_NO"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("國防採購室保留索賠權日期", ChFont));
                    Firsttable.AddCell(new Paragraph(FCommon.getDateTime(dr["OVC_CLAIM_REV_DATE"]), ChFont));
                    Firsttable.AddCell(new Paragraph("國防採購室保留索賠權文號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CLAIM_REV_MSG"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("軍  品  名  稱", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CLAIM_ITEM"].ToString(), ChFont));
                    Firsttable.AddCell(colspan2);
                    Firsttable.AddCell(new Paragraph("損  失  原  因", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CLAIM_REASON"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph(" ", ChFont));
                    Firsttable.AddCell(new Paragraph("", ChFont));
                    Firsttable.AddCell(new Paragraph("應  收  件  數", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["ONB_RECEIVE"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("實  收  件  數", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["ONB_ACTUAL_RECEIVE"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("索  賠  件  數", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["ONB_CLAIM_NUMBER"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("破  損  件  數", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["ONB_CLAIM_BREAK"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("索  賠  金  額", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["ONB_CLAIM_AMOUNT"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("幣          別", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CURRENCY_NAME"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("軍種撤銷保留索賠權日期", ChFont));
                    Firsttable.AddCell(new Paragraph(FCommon.getDateTime(dr["OVC_CLAIM_REV_DATE"].ToString()), ChFont));
                    Firsttable.AddCell(new Paragraph("軍種撤銷保留索賠權文號", ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_CLAIM_REV_MSG"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph("國防採購室撤銷保留索賠權日期", ChFont));
                    Firsttable.AddCell(new Paragraph("", ChFont));
                    Firsttable.AddCell(new Paragraph("國防採購室撤銷保留索賠權文號", ChFont));
                    Firsttable.AddCell(new Paragraph("", ChFont));
                    Firsttable.AddCell(new Paragraph("備註", ChFont));
                    PdfPCell tail_text = new PdfPCell(new Paragraph(dr["OVC_NOTE"].ToString(), ChFont));
                    tail_text.Colspan = 3;
                    tail_text.VerticalAlignment = Element.ALIGN_TOP;
                    Firsttable.AddCell(tail_text);
                    doc1.Add(Firsttable);
                    doc1.Close();

                    string strFileName = $"保留索賠權撤銷明細表-{ strOVC_RECLAIM_NO }.pdf";
                    FCommon.DownloadFile(this, strFileName, Memory);
                }
            }
            else
                FCommon.AlertShow(pnMessageQuery, "danger", "系統訊息", "保留索賠權 系統編號錯誤！");
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_ONNAME, txtOVC_APPLY_DATE_S, txtOVC_APPLY_DATE_E);
                    #region 匯入下拉式選單
                    string strFirstText = "查詢全部進度";
                    CommonMTS.list_dataImport_APPROVE_STATUS(drpOVC_APPROVE_STATUS, true, strFirstText); //作業進度
                    #endregion

                    bool boolImport = false;
                    if (FCommon.getQueryString(this, "OVC_RECLAIM_NO", out string strOVC_RECLAIM_NO, true))
                        txtOVC_RECLAIM_NO.Text = strOVC_RECLAIM_NO;
                    if (FCommon.getQueryString(this, "OVC_DEPT_CDE", out string strOVC_DEPT_CDE, false))
                    {
                        txtOVC_DEPT_CDE.Value = strOVC_DEPT_CDE;
                        txtOVC_ONNAME.Text = FCommon.getDeptName(strOVC_DEPT_CDE);
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_BLD_NO", out string strOVC_BLD_NO, true))
                        txtOVC_BLD_NO.Text = strOVC_BLD_NO;
                    if (FCommon.getQueryString(this, "OVC_APPROVE_STATUS", out string strOVC_APPROVE_STATUS, true))
                        FCommon.list_setValue(drpOVC_APPROVE_STATUS, strOVC_APPROVE_STATUS);
                    if (FCommon.getQueryString(this, "OVC_APPLY_DATE_S", out string strOVC_APPLY_DATE_S, true))
                        txtOVC_APPLY_DATE_S.Text = strOVC_APPLY_DATE_S;
                    if (FCommon.getQueryString(this, "OVC_APPLY_DATE_E", out string strOVC_APPLY_DATE_E, true))
                        txtOVC_APPLY_DATE_E.Text = strOVC_APPLY_DATE_E;
                    if (boolImport) dataImport();
                }
            }
        }

        #region ~Click
        protected void btnClearDept_Click(object sender, EventArgs e)
        {
            FCommon.Controls_Clear(txtOVC_DEPT_CDE, txtOVC_ONNAME);
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }
        #endregion

        #region GridView
        protected void GV_TBGMT_CLAIM_RESERVE_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = theGridView.DataKeys[gvrIndex].Value.ToString();
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);

            string no = gvr.Cells[0].Text;
            string gvrN = gvr.Cells[1].Text;

            switch (e.CommandName)
            {
                case "btnModify":
                    Response.Redirect($"MTS_C15_2{ strQueryString }");
                    break;
                case "btnPrint":
                    DropDownList drpAction = (DropDownList)gvr.FindControl("drpAction");
                    if (FCommon.Controls_isExist(drpAction))
                    {
                        string strAction = drpAction.SelectedValue;
                        if (strAction.Equals("申請"))
                            Pdf_Details(id);
                        if (strAction.Equals("理賠"))
                            Pdf_Claims(id);
                        if (strAction.Equals("撤銷"))
                            Pdf_Reserve_Details(id);
                    }
                    break;
                default:
                    break;
            }
        }
        protected void GV_TBGMT_CLAIM_RESERVE_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                GridView theGridView = (GridView)sender;
                int index = gvr.RowIndex;
                DropDownList drpAction = (DropDownList)gvr.FindControl("drpAction");
                if (FCommon.Controls_isExist(drpAction))
                {
                    string[] strList = { "申請", "理賠", "撤銷" };
                    FCommon.list_dataImport(drpAction, strList, strList, "", "", false);
                }
            }
        }
        protected void GV_TBGMT_CLAIM_RESERVE_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        #endregion
    }
}