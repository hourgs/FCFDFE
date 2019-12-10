using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using System.Web.UI;

namespace FCFDFE.pages.MTS.C
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

            DateTime today = DateTime.Now;
            CultureInfo TwCultureInfo = new CultureInfo("zh-TW");
            TwCultureInfo.DateTimeFormat.Calendar = TwCultureInfo.OptionalCalendars[1];
            //每頁頁尾
            PdfContentByte cb = writer.DirectContent;
            Rectangle pageSize = document.PageSize;
            cb.SetRGBColorFill(0, 0, 0);

            cb.BeginText();
            cb.SetFontAndSize(chBaseFont, 12);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "中華民國" + today.ToString("yyyy年MM月dd日", TwCultureInfo), pageSize.GetRight(50), pageSize.GetTop(130), 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "字第          號", pageSize.GetRight(50), pageSize.GetTop(145), 0);

            cb.EndText();

            int pageN = (writer.PageNumber);
            string[] text = { "第", "一", "聯", "：", "送", "合", "約", "保", "險", "公", "司" };
            string[] text2 = { "第", "二", "聯", "：", "送", "索", "賠", "單", "位" };
            string[] text3 = { "第", "三", "聯", "：", "送", "採", "購", "室", "檔", "案", "室", "歸", "檔" };
            string[] text4 = { "第", "四", "聯", "：", "送", "採", "購", "室", "物", "資", "接", "轉", "處", "存", "查" };
            cb.BeginText();

            if (pageN == 1)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text[i], pageSize.GetRight(20), pageSize.GetTop(70 + (i * 12)), 0);
                }
            }
            if (pageN == 2)
            {
                for (int i = 0; i < text2.Length; i++)
                {
                    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text2[i], pageSize.GetRight(20), pageSize.GetTop(70 + (i * 12)), 0);
                }
            }
            if (pageN == 3)
            {
                for (int i = 0; i < text3.Length; i++)
                {
                    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text3[i], pageSize.GetRight(20), pageSize.GetTop(70 + (i * 12)), 0);
                }
            }
            if (pageN == 4)
            {
                for (int i = 0; i < text4.Length; i++)
                {
                    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text4[i], pageSize.GetRight(20), pageSize.GetTop(70 + (i * 12)), 0);
                }
            }


            cb.EndText();
        }
    }
    public partial class MTS_C12_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        DateTime dateNow = DateTime.Now;

        #region 副程式
        private void dataImport()
        {
            string strMessage = "";
            string strOVC_CLAIM_NO = txtOVC_CLAIM_NO.Text;
            string strOVC_DEPT_CDE = txtOVC_DEPT_CDE.Value;
            string strODT_CLAIM_DATE = txtODT_CLAIM_DATE.Text;

            ViewState["OVC_CLAIM_NO"] = strOVC_CLAIM_NO;
            ViewState["OVC_DEPT_CDE"] = strOVC_DEPT_CDE;
            ViewState["ODT_CLAIM_DATE"] = strODT_CLAIM_DATE;

            if (strOVC_CLAIM_NO.Equals(string.Empty))
                strMessage += "<p> 請輸入 索賠通知書編號！ </p>";

            if (strMessage.Equals(string.Empty))
            {
                var query =
                    from claim in MTSE.TBGMT_CLAIM.AsEnumerable()
                    join dept in GME.TBMDEPTs on claim.OVC_MILITARY_TYPE equals dept.OVC_DEPT_CDE
                    join account in GME.ACCOUNTs on claim.OVC_CREATE_LOGIN_ID equals account.USER_ID
                    select new
                    {
                        CLAIM_SN = claim.CLAIM_SN,
                        OVC_CLAIM_NO = claim.OVC_CLAIM_NO,
                        OVC_MILITARY_TYPE = claim.OVC_MILITARY_TYPE??"",
                        ODT_CLAIM_DATE = FCommon.getDateTime(claim.ODT_CLAIM_DATE),
                        OVC_ONNAME = dept.OVC_ONNAME,
                        OVC_CLAIM_MSG_NO = claim.OVC_CLAIM_MSG_NO,
                        OVC_INN_NO = claim.OVC_INN_NO,
                        OVC_CLAIM_ITEM = claim.OVC_CLAIM_ITEM,
                        ONB_CLAIM_NUMBER = claim.ONB_CLAIM_NUMBER,
                        ONB_CLAIM_AMOUNT = claim.ONB_CLAIM_AMOUNT,
                        OVC_CREATE_LOGIN_ID = account.USER_NAME,
                        OVC_CLAIM_CONDITION = claim.OVC_CLAIM_CONDITION ?? ""
                    };

                if (!strOVC_CLAIM_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_CLAIM_NO.Contains(strOVC_CLAIM_NO));
                if (!strOVC_DEPT_CDE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_MILITARY_TYPE.Equals(strOVC_DEPT_CDE));
                if (!strODT_CLAIM_DATE.Equals(string.Empty) && DateTime.TryParse(strODT_CLAIM_DATE, out DateTime dateODT_CLAIM_DATEs))
                    query = query.Where(table => DateTime.TryParse(table.ODT_CLAIM_DATE, out DateTime dateODT_CLAIM_DATE) && DateTime.Compare(dateODT_CLAIM_DATE, dateODT_CLAIM_DATEs) == 0);

                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_CLAIM, dt);
            }
            else
                FCommon.AlertShow(pnMessageQuery, "danger", "系統訊息", strMessage);
        }
        protected void btnPrint(string strCLAIM_SN)
        {
            if (Guid.TryParse(strCLAIM_SN, out Guid guidCLAIM_SN))
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
                    from claim in MTSE.TBGMT_CLAIM
                    where claim.CLAIM_SN.Equals(guidCLAIM_SN)
                    join dept in MTSE.TBMDEPTs on claim.OVC_MILITARY_TYPE equals dept.OVC_DEPT_CDE into dept
                    from d in dept.DefaultIfEmpty()
                    join currency in MTSE.TBGMT_CURRENCY on claim.OVC_CLAIM_CURRENCY equals currency.OVC_CURRENCY_CODE into currency
                    from c in currency.DefaultIfEmpty()

                    select new
                    {
                        OVC_CLAIM_NO = claim.OVC_CLAIM_NO,
                        OVC_DEPT_NAME = d.OVC_ONNAME,
                        OVC_PURCH_NO = claim.OVC_PURCH_NO,
                        ODT_CLAIM_DATE = claim.ODT_CLAIM_DATE,
                        OVC_CLAIM_MSG_NO = claim.OVC_CLAIM_MSG_NO,
                        OVC_INN_NO = claim.OVC_INN_NO,
                        OVC_CLAIM_ITEM = claim.OVC_CLAIM_ITEM,
                        ONB_CLAIM_NUMBER = claim.ONB_CLAIM_NUMBER,
                        ONB_CLAIM_AMOUNT = claim.ONB_CLAIM_AMOUNT,
                        OVC_CURRENCY_NAME = c.OVC_CURRENCY_NAME,
                        OVC_CLAIM_REASON = claim.OVC_CLAIM_REASON,
                        OVC_NOTE = claim.OVC_NOTE,
                    };
                if (query.Any())
                {
                    DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                    DataRow dr = dt.Rows[0];
                    string strOVC_CLAIM_NO = dr["OVC_CLAIM_NO"].ToString();

                    //創建table
                    PdfPTable pdftable = new PdfPTable(new float[] { 3, 9 });
                    pdftable.TotalWidth = 500f;
                    pdftable.LockedWidth = true;
                    pdftable.DefaultCell.FixedHeight = 60;

                    PdfPCell title = new PdfPCell(new Phrase("\n\n國防部國防採購室外購案軍品索賠通知書", new Font(bfChinese, 16, Font.BOLD, BaseColor.BLACK)));
                    title.VerticalAlignment = Element.ALIGN_TOP;
                    title.HorizontalAlignment = Element.ALIGN_CENTER;
                    title.Colspan = 2;
                    title.FixedHeight = 120;
                    pdftable.AddCell(title);

                    PdfContentByte cb = writer.DirectContent;
                    Rectangle pageSize = doc.PageSize;
                    cb.SetRGBColorFill(0, 0, 0);
                    cb.BeginText();
                    cb.SetFontAndSize(chBaseFont, 12);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "表單編號：" + strOVC_CLAIM_NO + " ", pageSize.GetRight(50), pageSize.GetTop(110), 0);
                    cb.EndText();



                    PdfPCell cell1_0 = new PdfPCell(new Phrase("申請單位", ChFont));
                    cell1_0.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell1_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell1_0.FixedHeight = 30;
                    pdftable.AddCell(cell1_0);

                    PdfPCell cell1_1 = new PdfPCell(new Phrase(dr["OVC_DEPT_NAME"].ToString(), ChFont));
                    cell1_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell1_1.FixedHeight = 30;
                    pdftable.AddCell(cell1_1);

                    PdfPCell cell2_0 = new PdfPCell(new Phrase("案號", ChFont));
                    cell2_0.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell2_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell2_0.FixedHeight = 30;
                    pdftable.AddCell(cell2_0);


                    PdfPCell cell2_1 = new PdfPCell(new Phrase(dr["OVC_PURCH_NO"].ToString(), ChFont));
                    cell2_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell2_1.FixedHeight = 30;
                    pdftable.AddCell(cell2_1);

                    PdfPCell cell3_0 = new PdfPCell(new Phrase("軍種索賠日期", ChFont));
                    cell3_0.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell3_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell3_0.FixedHeight = 30;
                    pdftable.AddCell(cell3_0);

                    PdfPCell cell3_1 = new PdfPCell();
                    if (!dr["ODT_CLAIM_DATE"].Equals(DBNull.Value))
                    {
                        cell3_1 = new PdfPCell(new Phrase(FCommon.getDateTime(dr["ODT_CLAIM_DATE"]), ChFont));
                    }
                    cell3_1.FixedHeight = 30;
                    cell3_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdftable.AddCell(cell3_1);

                    PdfPCell cell4_0 = new PdfPCell(new Phrase("字號", ChFont));
                    cell4_0.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell4_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4_0.FixedHeight = 30;
                    pdftable.AddCell(cell4_0);

                    PdfPCell cell4_1 = new PdfPCell(new Phrase(dr["OVC_CLAIM_MSG_NO"].ToString(), ChFont));
                    cell4_1.FixedHeight = 30;
                    cell4_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdftable.AddCell(cell4_1);

                    PdfPCell cell5_0 = new PdfPCell(new Phrase("保單號碼", ChFont));
                    cell5_0.FixedHeight = 30;
                    cell5_0.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell5_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdftable.AddCell(cell5_0);

                    PdfPCell cell5_1 = new PdfPCell(new Phrase(dr["OVC_INN_NO"].ToString(), ChFont));
                    cell5_1.FixedHeight = 30;
                    cell5_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdftable.AddCell(cell5_1);



                    PdfPCell cell6_0 = new PdfPCell(new Phrase("索賠軍品名稱", ChFont));
                    cell6_0.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell6_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell6_0.FixedHeight = 30;
                    pdftable.AddCell(cell6_0);

                    PdfPCell cell6_1 = new PdfPCell(new Phrase(dr["OVC_CLAIM_ITEM"].ToString(), ChFont));
                    cell6_1.FixedHeight = 30;
                    cell6_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdftable.AddCell(cell6_1);

                    PdfPCell cell7_0 = new PdfPCell(new Phrase("索賠軍品數量", ChFont));
                    cell7_0.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell7_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell7_0.FixedHeight = 30;
                    pdftable.AddCell(cell7_0);

                    PdfPCell cell7_1 = new PdfPCell(new Phrase(dr["ONB_CLAIM_NUMBER"].ToString() + " EA", ChFont));
                    cell7_1.FixedHeight = 30;
                    cell7_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdftable.AddCell(cell7_1);

                    PdfPCell cell8_0 = new PdfPCell(new Phrase("索賠軍品總額", ChFont));
                    cell8_0.FixedHeight = 30;
                    cell8_0.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell8_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdftable.AddCell(cell8_0);



                    PdfPCell cell8_1 = new PdfPCell(new Phrase(dr["ONB_CLAIM_AMOUNT"].ToString() + " " + dr["OVC_CURRENCY_NAME"].ToString(), ChFont));
                    cell8_1.FixedHeight = 30;
                    cell8_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdftable.AddCell(cell8_1);


                    PdfPCell cell9_0 = new PdfPCell(new Phrase("索賠原因", ChFont));
                    cell9_0.FixedHeight = 50;
                    cell9_0.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell9_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdftable.AddCell(cell9_0);

                    PdfPCell cell9_1 = new PdfPCell(new Phrase(dr["OVC_CLAIM_REASON"].ToString(), ChFont));
                    cell9_1.FixedHeight = 50;
                    cell9_1.VerticalAlignment = Element.ALIGN_TOP;
                    pdftable.AddCell(cell9_1);



                    PdfPCell cell10_0 = new PdfPCell(new Phrase("備考", ChFont));
                    cell10_0.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell10_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell10_0.FixedHeight = 30;
                    pdftable.AddCell(cell10_0);

                    PdfPCell cell10_1 = new PdfPCell(new Phrase(dr["OVC_NOTE"].ToString(), ChFont));
                    cell10_1.FixedHeight = 30;
                    cell10_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdftable.AddCell(cell10_1);



                    doc.Add(pdftable);
                    doc.NewPage();
                    doc.Add(pdftable);
                    cb.BeginText();
                    cb.SetFontAndSize(chBaseFont, 12);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "表單編號：" + strOVC_CLAIM_NO + " ", pageSize.GetRight(50), pageSize.GetTop(110), 0);
                    cb.EndText();
                    doc.NewPage();
                    doc.Add(pdftable);
                    cb.BeginText();
                    cb.SetFontAndSize(chBaseFont, 12);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "表單編號：" + strOVC_CLAIM_NO + " ", pageSize.GetRight(50), pageSize.GetTop(110), 0);
                    cb.EndText();
                    doc.NewPage();
                    doc.Add(pdftable);
                    cb.BeginText();
                    cb.SetFontAndSize(chBaseFont, 12);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "表單編號：" + strOVC_CLAIM_NO + " ", pageSize.GetRight(50), pageSize.GetTop(110), 0);
                    cb.EndText();
                    doc.Close();

                    string strFileName = "國防部國防採購室外購案軍品索賠通知書.pdf";
                    FCommon.DownloadFile(this, strFileName, Memory);
                }
                else
                    FCommon.AlertShow(pnMessageQuery, "danger", "系統訊息", "索賠通知書 不存在！");
            }
            else
                FCommon.AlertShow(pnMessageQuery, "danger", "系統訊息", "索賠通知書 系統編號錯誤！");
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_CLAIM_NO", ViewState["OVC_CLAIM_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_DEPT_CDE", ViewState["OVC_DEPT_CDE"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_CLAIM_DATE", ViewState["ODT_CLAIM_DATE"], true);
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
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_ONNAME, txtODT_CLAIM_DATE);

                    bool boolImport = false;
                    if (FCommon.getQueryString(this, "OVC_CLAIM_NO", out string strOVC_CLAIM_NO, true))
                        txtOVC_CLAIM_NO.Text = strOVC_CLAIM_NO;
                    if (FCommon.getQueryString(this, "OVC_DEPT_CDE", out string strOVC_DEPT_CDE, true))
                    {
                        txtOVC_DEPT_CDE.Value = strOVC_DEPT_CDE;
                        txtOVC_ONNAME.Text = FCommon.getDeptName(strOVC_DEPT_CDE);
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "ODT_CLAIM_DATE", out string strODT_CLAIM_DATE, true))
                        txtODT_CLAIM_DATE.Text = strODT_CLAIM_DATE;
                    if (boolImport)
                        dataImport();
                }
            }
        }

        #region ~Click
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }
        protected void btnClearDept_Click(object sender, EventArgs e)
        {
            FCommon.Controls_Clear(txtOVC_DEPT_CDE, txtOVC_ONNAME);
        }
        #endregion

        #region GridView
        protected void GV_TBGMT_CLAIM_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = theGridView.DataKeys[gvrIndex].Value.ToString();
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);

            switch (e.CommandName)
            {
                case "btnModify":
                    Response.Redirect($"MTS_C12_2{strQueryString}");
                    break;
                case "btnDel":
                    Response.Redirect($"MTS_C12_3{strQueryString}");
                    break;
                case "btnPrint":
                    btnPrint(id);
                    break;
            }
        }
        protected void GV_TBGMT_CLAIM_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        #endregion
    }
}