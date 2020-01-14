using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Microsoft.International.Formatters;
using Microsoft.Office.Interop.Word;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;
using Xceed.Words.NET;


namespace FCFDFE.pages.MPMS.E
{
    public class Watermark : PdfPageEventHelper
    {

        PdfContentByte cb;
        GMEntities gm = new GMEntities();
        public Watermark()
        {

        }

        public string strPurchNum { get; set; }

        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {

            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型

            iTextSharp.text.Font ChFont = new iTextSharp.text.Font(bfChinese, 24, iTextSharp.text.Font.NORMAL, new BaseColor(255, 0, 0, 60));
            var query1301 = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            string strMark = query1301.OVC_PUR_NSECTION + strPurchNum + query1301.OVC_PUR_AGENCY;
            ColumnText.ShowTextAligned(
                  writer.DirectContentUnder,
                  Element.ALIGN_CENTER, new Phrase(strMark + "(" + writer.PageNumber + ")", ChFont),
                  300, 400, 55
                );


            cb = writer.DirectContent;


            DateTime PrintTime = DateTime.Now;
            string chFontPath = "c:\\windows\\fonts\\KAIU.TTF";
            BaseFont chBaseFont = BaseFont.CreateFont(chFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            int pageN = writer.PageNumber + 1;
            string text = "第" + pageN + "聯";
            string text2 = text.Replace("1", "一")
            .Replace("2", "二").Replace("3", "三")
            .Replace("4", "四").Replace("5", "五")
            .Replace("6", "六").Replace("7", "七")
            .Replace("8", "八").Replace("9", "九");

            iTextSharp.text.Rectangle pageSize = document.PageSize;
            cb.SetRGBColorFill(0, 0, 0);

            cb.BeginText();
            cb.SetFontAndSize(chBaseFont, 12);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, text2, pageSize.GetLeft(42), pageSize.GetTop(30), 0);
            cb.SetTextMatrix(pageSize.GetLeft(100), pageSize.GetBottom(30));
            Barcode39 code39 = new Barcode39();
            code39.Code = strPurchNum + query1301.OVC_PUR_AGENCY;
            iTextSharp.text.Image img = code39.CreateImageWithBarcode(cb, null, null);
            img.SetAbsolutePosition(pageSize.GetLeft(10), pageSize.GetBottom(20));
            document.Add(img);


            cb.EndText();
            BaseFont bf = null;
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);



            cb.BeginText();
            cb.SetFontAndSize(chBaseFont, 16);
            string strcb = "";
            switch (writer.PageNumber)
            {
                case 1:
                    strcb = "申購單位存查";
                    break;
                case 2:
                    strcb = "核定單位存查";
                    break;
                case 3:
                    strcb = "核交招標約定單位";
                    break;
                case 4:
                    strcb = "核覆原申購單位";
                    break;
            }
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, strcb, pageSize.GetRight(40), pageSize.GetBottom(40), 0);
            cb.SetFontAndSize(chBaseFont, 8);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, PrintTime.ToString("yyyy/MM/dd HH:mm:ss") + "(" + text2 + ")", pageSize.GetRight(40), pageSize.GetBottom(25), 0);

            cb.EndText();

        }

    }

    public class WatermarkList : PdfPageEventHelper
    {
        GMEntities gm = new GMEntities();
        PdfTemplate template;
        BaseFont bf = null;
        PdfContentByte cb;
        /** The header text. */
        public string Header { get; set; }
        public string strPurchNum { get; set; }

        public override void OnOpenDocument(PdfWriter writer, iTextSharp.text.Document document)
        {
            template = writer.DirectContent.CreateTemplate(30, 16);
        }
        private string GetTaiwanDate(string strDate)
        {
            if (DateTime.TryParse(strDate, out DateTime datetime))
            {
                CultureInfo culture = new CultureInfo("zh-TW");
                culture.DateTimeFormat.Calendar = new TaiwanCalendar();
                return datetime.ToString("yyy年MM月dd日", culture);
            }
            else
            {

                return "";
            }
        }
        public override void OnEndPage(PdfWriter writer, iTextSharp.text.Document document)
        {
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            iTextSharp.text.Rectangle pageSize = document.PageSize;
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型

            cb = writer.DirectContent;
            cb.SetRGBColorFill(100, 100, 100);

            string text = "Page " + writer.CurrentPageNumber + " of ";

            PdfContentByte under = writer.DirectContentUnder;
            under.BeginText();
            under.SetRGBColorFill(0xFF, 0x88, 0x88);
            under.SetFontAndSize(bfChinese, 32);
            var query1301 = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            string strMark = query1301.OVC_PUR_NSECTION + strPurchNum + query1301.OVC_PUR_AGENCY;
            under.ShowTextAligned(PdfContentByte.ALIGN_CENTER, strMark + "(" + writer.CurrentPageNumber.ToString() + ")", 300, 400, 55);
            under.EndText();

            //BARCODE39
            Barcode39 codeEAN = new Barcode39();
            codeEAN.Code = strPurchNum + query1301.OVC_PUR_AGENCY;
            iTextSharp.text.Image img = codeEAN.CreateImageWithBarcode(cb, null, null);
            img.SetAbsolutePosition(pageSize.GetRight(180), pageSize.GetTop(60));
            cb.BeginText();
            cb.AddImage(img);

            cb.SetFontAndSize(bfChinese, 18f);
            string kind = "";
            if (query1301.OVC_PURCH_KIND.Equals("1"))
                kind = "國內";
            else
                kind = "國外";
            string yearsub = strPurchNum.Substring(2, 2);
            string year = "";
            if (Int32.Parse(yearsub) >= 70)
                year = yearsub;
            else
                year = "1" + yearsub;

            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, year + "年度" + query1301.OVC_PUR_NSECTION + kind + "財務勞務採購計畫清單", pageSize.GetRight(300), pageSize.GetTop(80), 0);

            cb.SetFontAndSize(bfChinese, 12f);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "中華民國" + GetTaiwanDate(query1301.OVC_DPROPOSE.ToString()) + query1301.OVC_PROPOSE, pageSize.GetRight(150), pageSize.GetTop(95), 0);

            cb.SetFontAndSize(bfChinese, 12f);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "第" + writer.CurrentPageNumber + "頁", pageSize.GetRight(30), pageSize.GetTop(110), 0);

            cb.SetFontAndSize(bfChinese, 12f);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "中華民國" + GetTaiwanDate(query1301.OVC_PUR_DAPPROVE.ToString()) + query1301.OVC_PUR_APPROVE, pageSize.GetLeft(30), pageSize.GetBottom(40), 0);
            cb.AddTemplate(template, pageSize.GetRight(30), pageSize.GetBottom(40));

            cb.SetFontAndSize(bf, 11);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pageSize.GetRight(30), pageSize.GetBottom(40), 0);
            cb.EndText();

        }
        public override void OnCloseDocument(PdfWriter writer, iTextSharp.text.Document document)
        {
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            base.OnCloseDocument(writer, document);
            template.BeginText();
            template.SetFontAndSize(bf, 11);
            template.SetTextMatrix(0, 0);
            template.ShowText("" + (writer.PageNumber));
            template.EndText();

        }
    }

    public class PDF : PdfPageEventHelper
    {
        PdfTemplate template;
        BaseFont bf = null;
        PdfContentByte cb;
        /** The header text. */
        public string Header { get; set; }
        public override void OnOpenDocument(PdfWriter writer, iTextSharp.text.Document document)
        {
            template = writer.DirectContent.CreateTemplate(30, 16);
        }

        public override void OnEndPage(PdfWriter writer, iTextSharp.text.Document document)
        {
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            iTextSharp.text.Rectangle pageSize = document.PageSize;
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型

            cb = writer.DirectContent;
            cb.SetRGBColorFill(100, 100, 100);

            string text = "Page " + writer.CurrentPageNumber + " of ";

            cb.BeginText();

            cb.SetFontAndSize(bfChinese, 18f);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "預算年度分配明細表 ", pageSize.GetRight(300), pageSize.GetTop(80), 0);

            cb.SetFontAndSize(bfChinese, 12f);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "列印日期:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), pageSize.GetRight(100), pageSize.GetTop(95), 0);

            cb.SetFontAndSize(bfChinese, 12f);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "第" + writer.CurrentPageNumber + "頁", pageSize.GetRight(30), pageSize.GetTop(110), 0);

            cb.AddTemplate(template, pageSize.GetRight(30), pageSize.GetBottom(40));

            cb.SetFontAndSize(bf, 11);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pageSize.GetRight(30), pageSize.GetBottom(40), 0);
            cb.EndText();

        }
        public override void OnCloseDocument(PdfWriter writer, iTextSharp.text.Document document)
        {
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            base.OnCloseDocument(writer, document);
            template.BeginText();
            template.SetFontAndSize(bf, 11);
            template.SetTextMatrix(0, 0);
            template.ShowText("" + (writer.PageNumber));
            template.EndText();

        }
    }

    public partial class MPMS_E13 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        TBMRECEIVE_CHECK tbmreceive_check = new TBMRECEIVE_CHECK();
        TBMRECEIVE_CHECK_ITEM tbmreceive_check_item = new TBMRECEIVE_CHECK_ITEM();
        TBMRECEIVE_BID tbmreceive_bid = new TBMRECEIVE_BID();
        TBMRECEIVE_CONTRACT tbmreceive_contract = new TBMRECEIVE_CONTRACT();
        public string strMenuName = "", strMenuNameItem = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (!IsPostBack)
                {
                    if ((string)(Session["XSSRequest"]) == "danger")
                    {
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "輸入錯誤，請重新輸入！");
                        Session["XSSRequest"] = null;
                    }

                    if (Session["rowtext"] != null && Session["userunit"] != null && Session["rowven"] != null)
                    {
                        ViewState["rowtext"] = Session["rowtext"].ToString();
                        //Session.Contents.Remove("rowtext");
                        FCommon.Controls_Attributes("readonly", "true", txtOVC_DSEND, txtOVC_DRECEIVE, txtOVC_DO_DAPPROVE);
                        GV_dataImport();
                        DT_DoName_dataImport();
                        CheckItem_dataImport();
                        btnMouseover();
                    }
                    else
                        FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
                }
            }
        }

        #region Click

        #region 清除日期btn
        protected void btnResetOVC_DSEND_Click(object sender, EventArgs e)
        {
            txtOVC_DSEND.Text = "";
        }
        protected void btnResetOVC_DRECEIVE_Click(object sender, EventArgs e)
        {
            txtOVC_DRECEIVE.Text = "";
        }
        protected void btnResetOVC_DO_DAPPROVE_Click(object sender, EventArgs e)
        {
            txtOVC_DO_DAPPROVE.Text = "";
        }
        #endregion

        #region 存檔btn
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtOVC_VEN_TITLE.Text == "" || rdoOVC_DO_NAME_RESULT.SelectedIndex == -1 || txtOVC_DSEND.Text == "" || txtOVC_DRECEIVE.Text == "")
            {
                string strmeg = "";
                if (txtOVC_VEN_TITLE.Text == "")
                    strmeg += "<p>請填選 承包商名稱</p>";
                if (rdoOVC_DO_NAME_RESULT.Text == "")
                    strmeg += "<p>請填選 契約移轉履約驗結處檢查項目表編輯(第三頁)-綜辦意見</p>";
                if (txtOVC_DSEND.Text == "")
                    strmeg += "<p>請填選 契約移轉履約驗結處檢查項目表編輯(第三頁)-採包移送日</p>";
                if (txtOVC_DRECEIVE.Text == "")
                    strmeg += "<p>請填選 契約移轉履約驗結處檢查項目表編輯(第三頁)-收辦日</p>";
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strmeg);
            }
            else
            {
                #region TBM1302
                string purch_6 = Session["purch_6"].ToString();
                var query1302 =
                    from tbm1302 in mpms.TBM1302
                    where tbm1302.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    //where tbm1302.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                        ONB_GROUP = tbm1302.ONB_GROUP,
                        OVC_VEN_CST = tbm1302.OVC_VEN_CST
                    };
                foreach (var q in query1302)
                {
                    TBM1302 tBM1302 = new TBM1302();
                    tBM1302 = mpms.TBM1302
                        .Where(table => table.OVC_PURCH_6.Equals(q.OVC_PURCH_6))
                        .Where(table => table.ONB_GROUP.Equals(q.ONB_GROUP))
                        .Where(table => table.OVC_VEN_CST.Equals(q.OVC_VEN_CST))
                        .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))).FirstOrDefault();
                    if (tBM1302 != null)
                    {
                        if (tBM1302.OVC_DRECEIVE == null || tBM1302.OVC_DRECEIVE == "")
                        {
                            tBM1302.OVC_DRECEIVE = DateTime.Now.ToString("yyyy-MM-dd");
                            mpms.SaveChanges();
                        }
                    }
                }
                #endregion
                #region TBMRECEIVE_CONTRACT
                var query1301_1302 =
                    from tbm1302 in mpms.TBM1302
                    join tbm1301 in mpms.TBM1301_PLAN on tbm1302.OVC_PURCH equals tbm1301.OVC_PURCH
                    where tbm1302.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    //where tbm1302.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        OVC_PURCH = tbm1302.OVC_PURCH,
                        OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                        OVC_VEN_CST = tbm1302.OVC_VEN_CST,
                        ONB_GROUP = tbm1302.ONB_GROUP,
                        OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE,
                        OVC_VEN_TEL = tbm1302.OVC_VEN_TEL,
                        OVC_VEN_ADDRESS = tbm1302.OVC_VEN_ADDRESS,
                        OVC_VEN_FAX = tbm1302.OVC_VEN_FAX,
                        OVC_VEN_EMAIL = tbm1302.OVC_VEN_EMAIL,
                        OVC_VEN_CELLPHONE = tbm1302.OVC_VEN_CELLPHONE,
                        OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                        ONB_MONEY = tbm1302.ONB_MONEY,
                    };
                foreach (var q in query1301_1302)
                {
                    TBMRECEIVE_CONTRACT tbmreceive_contract = new TBMRECEIVE_CONTRACT();
                    tbmreceive_contract = mpms.TBMRECEIVE_CONTRACT
                        .Where(table => table.OVC_PURCH_6.Equals(q.OVC_PURCH_6))
                        .Where(table => table.ONB_GROUP.Equals(q.ONB_GROUP))
                        .Where(table => table.OVC_VEN_CST.Equals(q.OVC_VEN_CST))
                        .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))).FirstOrDefault();
                    if (tbmreceive_contract == null)
                    {
                        TBMRECEIVE_CONTRACT tbmreceive_contract_new = new TBMRECEIVE_CONTRACT();
                        tbmreceive_contract_new.OVC_PURCH = q.OVC_PURCH;
                        tbmreceive_contract_new.OVC_PURCH_6 = q.OVC_PURCH_6;
                        tbmreceive_contract_new.OVC_VEN_CST = q.OVC_VEN_CST;
                        tbmreceive_contract_new.OVC_VEN_TEL = q.OVC_VEN_TEL;
                        tbmreceive_contract_new.OVC_VEN_ADDRESS = q.OVC_VEN_ADDRESS;
                        tbmreceive_contract_new.OVC_VEN_FAX = q.OVC_VEN_FAX;
                        tbmreceive_contract_new.OVC_VEN_EMAIL = q.OVC_VEN_EMAIL;
                        tbmreceive_contract_new.OVC_VEN_CELLPHONE = q.OVC_VEN_CELLPHONE;
                        tbmreceive_contract_new.ONB_GROUP = q.ONB_GROUP;
                        tbmreceive_contract_new.OVC_VEN_TITLE = q.OVC_VEN_TITLE;
                        tbmreceive_contract_new.OVC_DRECEIVE = txtOVC_DRECEIVE.Text;
                        tbmreceive_contract_new.OVC_DO_NAME = drpOVC_DO_NAME.SelectedValue;
                        tbmreceive_contract_new.ONB_MONEY = q.ONB_MONEY;
                        mpms.TBMRECEIVE_CONTRACT.Add(tbmreceive_contract_new);
                        mpms.SaveChanges();
                    }
                    else
                    {
                        tbmreceive_contract.OVC_DRECEIVE = txtOVC_DRECEIVE.Text;
                        tbmreceive_contract.OVC_DO_NAME = drpOVC_DO_NAME.SelectedValue;
                        mpms.SaveChanges();
                    }
                }
                #endregion
                #region TBMRECEIVE_CHECK & TBMRECEIVE_CHECK_ITEM
                var queryCheck =
                    from tbm1302 in mpms.TBM1302
                    where tbm1302.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    //where tbm1302.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        OVC_PURCH = tbm1302.OVC_PURCH,
                        OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                        OVC_VEN_CST = tbm1302.OVC_VEN_CST,
                        ONB_GROUP = tbm1302.ONB_GROUP
                    };
                foreach (var q in queryCheck)
                {
                    TBMRECEIVE_CHECK tbmreceive_check = new TBMRECEIVE_CHECK();
                    tbmreceive_check = mpms.TBMRECEIVE_CHECK
                        .Where(table => table.OVC_PURCH_6.Equals(q.OVC_PURCH_6))
                        .Where(table => table.ONB_GROUP.Equals(q.ONB_GROUP))
                        .Where(table => table.OVC_VEN_CST.Equals(q.OVC_VEN_CST))
                        .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))).FirstOrDefault();
                    if (tbmreceive_check == null)
                    {
                        TBMRECEIVE_CHECK tbmreceive_check_new = new TBMRECEIVE_CHECK();
                        tbmreceive_check_new.OVC_PURCH = q.OVC_PURCH;
                        tbmreceive_check_new.OVC_PURCH_6 = q.OVC_PURCH_6;
                        tbmreceive_check_new.OVC_VEN_CST = q.OVC_VEN_CST;
                        tbmreceive_check_new.ONB_GROUP = q.ONB_GROUP;
                        tbmreceive_check_new.OVC_DSEND = txtOVC_DSEND.Text;
                        tbmreceive_check_new.OVC_DRECEIVE = txtOVC_DRECEIVE.Text;
                        tbmreceive_check_new.OVC_DO_NAME = drpOVC_DO_NAME.SelectedValue;
                        tbmreceive_check_new.OVC_DO_DAPPROVE = txtOVC_DO_DAPPROVE.Text;
                        tbmreceive_check_new.OVC_DO_NAME_RESULT = (rdoOVC_DO_NAME_RESULT.SelectedIndex + 1).ToString();
                        tbmreceive_check_new.OVC_REJECT_REASON_DO = Reason_for_withdrawal_textbox.Text;
                        mpms.TBMRECEIVE_CHECK.Add(tbmreceive_check_new);
                        mpms.SaveChanges();
                    }
                    else
                    {
                        tbmreceive_check.OVC_DSEND = txtOVC_DSEND.Text;
                        tbmreceive_check.OVC_DRECEIVE = txtOVC_DRECEIVE.Text;
                        tbmreceive_check.OVC_DO_NAME = drpOVC_DO_NAME.Text;
                        tbmreceive_check.OVC_DO_DAPPROVE = txtOVC_DO_DAPPROVE.Text;
                        tbmreceive_check.OVC_DO_NAME_RESULT = (rdoOVC_DO_NAME_RESULT.SelectedIndex + 1).ToString();
                        tbmreceive_check.OVC_REJECT_REASON_DO = Reason_for_withdrawal_textbox.Text;
                        mpms.SaveChanges();
                    }

                    TBMRECEIVE_CHECK_ITEM tbmreceive_check_item_new = new TBMRECEIVE_CHECK_ITEM();
                    tbmreceive_check_item_new = mpms.TBMRECEIVE_CHECK_ITEM
                        .Where(table => table.OVC_PURCH_6.Equals(q.OVC_PURCH_6))
                        .Where(table => table.ONB_GROUP.Equals(q.ONB_GROUP))
                        .Where(table => table.OVC_VEN_CST.Equals(q.OVC_VEN_CST))
                        .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))).FirstOrDefault();
                    if (tbmreceive_check_item_new == null)
                    {
                        for (int i = 1; i < 9; i++)
                        {
                            TBMRECEIVE_CHECK_ITEM tbmreceive_check_item = new TBMRECEIVE_CHECK_ITEM();
                            string lab = "lblQ1_" + i;
                            string drp = "drpCheck1_" + i;
                            Label label = (Label)tbItem.FindControl(lab);
                            DropDownList dropDownList = (DropDownList)tbItem.FindControl(drp);
                            string result = dropDownList.SelectedValue;
                            tbmreceive_check_item.OVC_PURCH = q.OVC_PURCH;
                            tbmreceive_check_item.OVC_PURCH_6 = q.OVC_PURCH_6;
                            tbmreceive_check_item.OVC_VEN_CST = q.OVC_VEN_CST;
                            tbmreceive_check_item.ONB_GROUP = q.ONB_GROUP;
                            tbmreceive_check_item.ONB_ITEM = short.Parse("100" + i);
                            tbmreceive_check_item.OVC_ITEM_NAME = label.Text;
                            tbmreceive_check_item.OVC_RESULT = result;
                            mpms.TBMRECEIVE_CHECK_ITEM.Add(tbmreceive_check_item);
                            mpms.SaveChanges();
                        }
                        for (int i = 1; i < 20; i++)
                        {
                            TBMRECEIVE_CHECK_ITEM tbmreceive_check_item = new TBMRECEIVE_CHECK_ITEM();
                            string lab = "lblQ2_" + i;
                            string drp = "drpCheck2_" + i;
                            Label label = (Label)tbItem.FindControl(lab);
                            DropDownList dropDownList = (DropDownList)tbItem.FindControl(drp);
                            string result = dropDownList.SelectedValue;
                            tbmreceive_check_item.OVC_PURCH = q.OVC_PURCH;
                            tbmreceive_check_item.OVC_PURCH_6 = q.OVC_PURCH_6;
                            tbmreceive_check_item.OVC_VEN_CST = q.OVC_VEN_CST;
                            tbmreceive_check_item.ONB_GROUP = q.ONB_GROUP;
                            tbmreceive_check_item.ONB_ITEM = short.Parse("200" + i);
                            if (i > 6)
                                tbmreceive_check_item.ONB_ITEM = short.Parse("300" + (i - 6));
                            if (i > 15)
                                tbmreceive_check_item.ONB_ITEM = short.Parse("30" + (i - 6));
                            tbmreceive_check_item.OVC_ITEM_NAME = label.Text;
                            tbmreceive_check_item.OVC_RESULT = result;
                            mpms.TBMRECEIVE_CHECK_ITEM.Add(tbmreceive_check_item);
                            mpms.SaveChanges();
                        }
                        for (int i = 1; i < 8; i++)
                        {
                            TBMRECEIVE_CHECK_ITEM tbmreceive_check_item = new TBMRECEIVE_CHECK_ITEM();
                            string lab = "lblQ3_" + i;
                            string drp = "drpCheck3_" + i;
                            Label label = (Label)tbItem.FindControl(lab);
                            DropDownList dropDownList = (DropDownList)tbItem.FindControl(drp);
                            string result = dropDownList.SelectedValue;
                            tbmreceive_check_item.OVC_PURCH = q.OVC_PURCH;
                            tbmreceive_check_item.OVC_PURCH_6 = q.OVC_PURCH_6;
                            tbmreceive_check_item.OVC_VEN_CST = q.OVC_VEN_CST;
                            tbmreceive_check_item.ONB_GROUP = q.ONB_GROUP;
                            tbmreceive_check_item.ONB_ITEM = short.Parse("400" + i);
                            tbmreceive_check_item.OVC_ITEM_NAME = label.Text;
                            tbmreceive_check_item.OVC_RESULT = result;
                            mpms.TBMRECEIVE_CHECK_ITEM.Add(tbmreceive_check_item);
                            mpms.SaveChanges();
                        }
                        for (int i = 8; i < 11; i++)
                        {
                            TBMRECEIVE_CHECK_ITEM tbmreceive_check_item = new TBMRECEIVE_CHECK_ITEM();
                            string lab = "lblQ3_" + i;
                            string drp = "drpCheck3_" + i;
                            Label label = (Label)tbItem.FindControl(lab);
                            DropDownList dropDownList = (DropDownList)tbItem.FindControl(drp);
                            string result = dropDownList.SelectedValue;
                            tbmreceive_check_item.OVC_PURCH = q.OVC_PURCH;
                            tbmreceive_check_item.OVC_PURCH_6 = q.OVC_PURCH_6;
                            tbmreceive_check_item.OVC_VEN_CST = q.OVC_VEN_CST;
                            tbmreceive_check_item.ONB_GROUP = q.ONB_GROUP;
                            tbmreceive_check_item.ONB_ITEM = short.Parse("500" + (i - 7));
                            tbmreceive_check_item.OVC_ITEM_NAME = label.Text;
                            tbmreceive_check_item.OVC_RESULT = result;
                            mpms.TBMRECEIVE_CHECK_ITEM.Add(tbmreceive_check_item);
                            mpms.SaveChanges();
                        }
                        tbmreceive_check_item.OVC_PURCH = q.OVC_PURCH;
                        tbmreceive_check_item.OVC_PURCH_6 = q.OVC_PURCH_6;
                        tbmreceive_check_item.OVC_VEN_CST = q.OVC_VEN_CST;
                        tbmreceive_check_item.ONB_GROUP = q.ONB_GROUP;
                        tbmreceive_check_item.ONB_ITEM = short.Parse("6001");
                        tbmreceive_check_item.OVC_ITEM_NAME = lblQ3_11.Text;
                        tbmreceive_check_item.OVC_RESULT = drpCheck3_11.SelectedValue;
                        mpms.TBMRECEIVE_CHECK_ITEM.Add(tbmreceive_check_item);
                        mpms.SaveChanges();
                    }
                    else
                    {
                        for (int i = 1; i < 9; i++)
                        {
                            string lab = "lblQ1_" + i;
                            string drp = "drpCheck1_" + i;
                            Label label = (Label)tbItem.FindControl(lab);
                            DropDownList dropDownList = (DropDownList)tbItem.FindControl(drp);
                            string result = dropDownList.SelectedValue;
                            tbmreceive_check_item = mpms.TBMRECEIVE_CHECK_ITEM
                                .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7)) && table.OVC_PURCH_6.Equals(purch_6) && table.OVC_ITEM_NAME.Equals(label.Text)).FirstOrDefault();
                            if (tbmreceive_check_item != null)
                            {
                                tbmreceive_check_item.OVC_RESULT = dropDownList.Text;
                                mpms.SaveChanges();
                            }
                        }
                        for (int i = 1; i < 20; i++)
                        {
                            string lab = "lblQ2_" + i;
                            string drp = "drpCheck2_" + i;
                            Label label = (Label)tbItem.FindControl(lab);
                            DropDownList dropDownList = (DropDownList)tbItem.FindControl(drp);
                            string result = dropDownList.SelectedValue;
                            tbmreceive_check_item = mpms.TBMRECEIVE_CHECK_ITEM
                                .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7)) && table.OVC_PURCH_6.Equals(purch_6) && table.OVC_ITEM_NAME.Equals(label.Text)).FirstOrDefault();
                            if (tbmreceive_check_item != null)
                            {
                                tbmreceive_check_item.OVC_RESULT = dropDownList.Text;
                                mpms.SaveChanges();
                            }
                        }
                        for (int i = 1; i < 12; i++)
                        {
                            string lab = "lblQ3_" + i;
                            string drp = "drpCheck3_" + i;
                            Label label = (Label)tbItem.FindControl(lab);
                            DropDownList dropDownList = (DropDownList)tbItem.FindControl(drp);
                            string result = dropDownList.SelectedValue;
                            tbmreceive_check_item = mpms.TBMRECEIVE_CHECK_ITEM
                                .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7)) && table.OVC_PURCH_6.Equals(purch_6) && table.OVC_ITEM_NAME.Equals(label.Text)).FirstOrDefault();
                            if (tbmreceive_check_item != null)
                            {
                                tbmreceive_check_item.OVC_RESULT = dropDownList.Text;
                                mpms.SaveChanges();
                            }
                        }
                    }
                }
                #endregion
                #region TBMSTATUS
                foreach (var q in query1302)
                {
                    TBMSTATUS tbmstatus = new TBMSTATUS();
                    tbmstatus = mpms.TBMSTATUS
                        .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7)))
                        .Where(table => table.OVC_PURCH_6.Equals(q.OVC_PURCH_6))
                        .Where(table => table.OVC_STATUS.Equals("3")).FirstOrDefault();
                    if (tbmstatus != null)
                    {
                        if (tbmstatus.OVC_DEND == null)
                        {
                            tbmstatus.OVC_DEND = DateTime.Now.ToString("yyyy-MM-dd");
                            mpms.SaveChanges();
                        }
                        TBMSTATUS tbmstatus31 = new TBMSTATUS();
                        tbmstatus31 = mpms.TBMSTATUS
                            .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7)))
                            .Where(table => table.OVC_PURCH_6.Equals(q.OVC_PURCH_6))
                            .Where(table => table.OVC_STATUS.Equals("31")).FirstOrDefault();
                        if (tbmstatus31 == null)
                        {
                            TBMSTATUS tbmstatus31_new = new TBMSTATUS();
                            tbmstatus31_new.OVC_PURCH = tbmstatus.OVC_PURCH;
                            tbmstatus31_new.OVC_PURCH_5 = tbmstatus.OVC_PURCH_5;
                            tbmstatus31_new.ONB_TIMES = tbmstatus.ONB_TIMES;
                            tbmstatus31_new.OVC_PURCH_6 = tbmstatus.OVC_PURCH_6;
                            tbmstatus31_new.OVC_DO_NAME = drpOVC_DO_NAME.Text;
                            tbmstatus31_new.OVC_STATUS = "31";
                            tbmstatus31_new.OVC_DBEGIN = tbmstatus.OVC_DEND;
                            tbmstatus31_new.OVC_DEND = txtOVC_DO_DAPPROVE.Text;
                            tbmstatus31_new.ONB_GROUP = tbmstatus.ONB_GROUP;
                            tbmstatus31_new.OVC_STATUS_SN = Guid.NewGuid();
                            mpms.TBMSTATUS.Add(tbmstatus31_new);
                            mpms.SaveChanges();
                        }
                        else
                        {
                            tbmstatus31.OVC_DO_NAME = drpOVC_DO_NAME.Text;
                            tbmstatus31.OVC_DEND = txtOVC_DO_DAPPROVE.Text;
                            mpms.SaveChanges();
                        }
                        TBMSTATUS tbmstatus32 = new TBMSTATUS();
                        tbmstatus32 = mpms.TBMSTATUS
                            .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7)))
                            .Where(table => table.OVC_PURCH_6.Equals(q.OVC_PURCH_6))
                            .Where(table => table.OVC_STATUS.Equals("32")).FirstOrDefault();
                        if (tbmstatus32 == null)
                        {
                            if (txtOVC_DO_DAPPROVE.Text != "")
                            {
                                TBMSTATUS tbmstatus32_new = new TBMSTATUS();
                                tbmstatus32_new.OVC_PURCH = tbmstatus.OVC_PURCH;
                                tbmstatus32_new.OVC_PURCH_5 = tbmstatus.OVC_PURCH_5;
                                tbmstatus32_new.ONB_TIMES = tbmstatus.ONB_TIMES;
                                tbmstatus32_new.OVC_PURCH_6 = tbmstatus.OVC_PURCH_6;
                                tbmstatus32_new.OVC_DO_NAME = drpOVC_DO_NAME.Text;
                                tbmstatus32_new.OVC_STATUS = "32";
                                tbmstatus32_new.OVC_DBEGIN = tbmstatus.OVC_DEND;
                                tbmstatus32_new.ONB_GROUP = tbmstatus.ONB_GROUP;
                                tbmstatus32_new.OVC_STATUS_SN = Guid.NewGuid();
                                mpms.TBMSTATUS.Add(tbmstatus32_new);
                                mpms.SaveChanges();
                            }
                        }
                        else
                        {
                            tbmstatus32.OVC_DO_NAME = drpOVC_DO_NAME.Text;
                            mpms.SaveChanges();
                        }
                    }
                }
                #endregion
                #region TBMDELIVERY
                foreach (var qu in query1301_1302)
                if (txtOVC_DO_DAPPROVE.Text != "" && Session["E15"] == null)
                {
                    int count = 1;
                    var ikind = "";
                    if (qu.OVC_PUR_AGENCY == "L" || qu.OVC_PUR_AGENCY == "P" || qu.OVC_PUR_AGENCY == "B")
                        ikind = "D57";
                    else if (qu.OVC_PUR_AGENCY == "W" || qu.OVC_PUR_AGENCY == "C" || qu.OVC_PUR_AGENCY == "E" || qu.OVC_PUR_AGENCY == "A")
                        ikind = "W59";
                    var queryShipTime =
                        from delivery in mpms.TBMDELIVERY
                        where delivery.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                        select delivery.ONB_SHIP_TIMES;
                    TBMDELIVERY tbmdelivery_new = new TBMDELIVERY();
                    tbmdelivery_new.OVC_PURCH = qu.OVC_PURCH;
                    tbmdelivery_new.OVC_PURCH_6 = qu.OVC_PURCH_6;
                    tbmdelivery_new.OVC_VEN_CST = qu.OVC_VEN_CST;
                    tbmdelivery_new.OVC_ACCORDING = "  年  月  日 字第  號";
                    if (queryShipTime.Count() < 1)
                        tbmdelivery_new.ONB_SHIP_TIMES = 1;
                    else
                    {
                        foreach (var q in queryShipTime)
                        {
                            count++;
                        }
                        tbmdelivery_new.ONB_SHIP_TIMES = short.Parse(count.ToString());
                    }
                    var query1220 =
                        from tbm1220 in mpms.TBM1220_1
                        where tbm1220.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                        where tbm1220.OVC_IKIND.Equals(ikind)
                        select tbm1220.OVC_MEMO;
                    foreach (var que in query1220)
                    {
                        tbmdelivery_new.OVC_DELIVERY_PLACE = que;
                        tbmdelivery_new.OVC_ACCORDING_PLACE = que;
                    }
                    var query1301 =
                        from tbm1301 in mpms.TBM1301_PLAN
                        where tbm1301.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                        select tbm1301.OVC_RECEIVE_DAYS;
                    foreach (var que in query1301)
                    {
                        tbmdelivery_new.OVC_DINSPECT_SOP = que;
                    }
                    mpms.TBMDELIVERY.Add(tbmdelivery_new);
                    mpms.SaveChanges();
                }
                #endregion
                if (Session["E15"] == null)
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "接管成功，購案編號：" + ViewState["rowtext"]);
                else
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                Session["rowtext"] = ViewState["rowtext"].ToString();
            }
        }
        #endregion

        #region 列印
        //物資申請書
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            var strPurchNum = lblOVC_PURCH.Text.Substring(0, 7);
            var query = mpms.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.OVC_PURCH_KIND).FirstOrDefault();
            if (query.Equals("1"))
            {
                PrintPDF();
                string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Material.pdf");
                string filepath = strPurchNum + "物資申請書.pdf";
                string fileName = HttpUtility.UrlEncode(filepath);
                WebClient wc = new WebClient(); //宣告並建立WebClient物件
                byte[] b = wc.DownloadData(path_temp); //載入要下載的檔案
                Response.Clear(); //清除Response內的HTML
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName); //設定標頭檔資訊 attachment 是本文章的關鍵字
                Response.BinaryWrite(b); //開始輸出讀取到的檔案
                File.Delete(path_temp);
                Response.End();
            }
            else
            {
                MaterialApplication_ExportToWord();
                string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/MA_Temp.docx");
                string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/MaterialApplication.pdf";
                WordcvDdf(path_temp, wordfilepath);
                File.Delete(path_temp);
                FileInfo file = new FileInfo(wordfilepath);
                string filepath = strPurchNum + "物資申請書.pdf";
                string fileName = HttpUtility.UrlEncode(filepath);
                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(file.FullName);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.Flush();
                Response.End();
            }
        }
        protected void LinkButton5_Click(object sender, EventArgs e)
        {
            var strPurchNum = lblOVC_PURCH.Text.Substring(0, 7);
            MaterialApplication_ExportToWord();
            string path_end = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/MA_Temp.docx");
            string filepath = strPurchNum + "物資申請書.docx";
            string fileName = HttpUtility.UrlEncode(filepath);
            WebClient wc = new WebClient(); //宣告並建立WebClient物件
            byte[] b = wc.DownloadData(path_end); //載入要下載的檔案
            Response.Clear(); //清除Response內的HTML
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName); //設定標頭檔資訊 attachment 是本文章的關鍵字
            Response.BinaryWrite(b); //開始輸出讀取到的檔案
            File.Delete(path_end);
            Response.End();
        }
        protected void LinkButton13_Click(object sender, EventArgs e)
        {
            var strPurchNum = lblOVC_PURCH.Text.Substring(0, 7);
            MaterialApplication_ExportToWord();
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/MA_Temp.docx");
            string path_end = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/MA_Temp.odt");
            string filepath = strPurchNum + "物資申請書.odt";
            string fileName = HttpUtility.UrlEncode(filepath);
            WordToODT(path_temp, path_end, fileName);
        }
        //計畫清單
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            ListOfPlans_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/ListOfPlans.pdf";
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/LOP_Temp.docx");
            WordcvDdf(path_temp, wordfilepath);
            File.Delete(path_temp);
            FileInfo file = new FileInfo(wordfilepath);
            string filepath = purch + "計畫清單.pdf";
            string fileName = HttpUtility.UrlEncode(filepath);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            Response.End();
        }
        protected void LinkButton8_Click(object sender, EventArgs e)
        {
            ListOfPlans_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/LOP_Temp.docx");
            string filepath = purch + "計畫清單.docx";
            string fileName = HttpUtility.UrlEncode(filepath);
            WebClient wc = new WebClient(); //宣告並建立WebClient物件
            byte[] b = wc.DownloadData(path_temp); //載入要下載的檔案
            Response.Clear(); //清除Response內的HTML
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName); //設定標頭檔資訊 attachment 是本文章的關鍵字
            Response.BinaryWrite(b); //開始輸出讀取到的檔案
            File.Delete(path_temp);
            Response.End();
        }
        protected void LinkButton14_Click(object sender, EventArgs e)
        {
            var strPurchNum = lblOVC_PURCH.Text.Substring(0, 7);
            ListOfPlans_ExportToWord();
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/LOP_Temp.docx");
            string path_end = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/LOP_Temp.odt");
            string filepath = strPurchNum + "計畫清單.odt";
            string fileName = HttpUtility.UrlEncode(filepath);
            WordToODT(path_temp, path_end, fileName);
        }
        //列印合約
        protected void LinkButton6_Click(object sender, EventArgs e)
        {
            Contract_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Contract_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "合約.docx";
            string fileName = HttpUtility.UrlEncode(filepath);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            File.Delete(path_temp);
            Response.End();
        }
        protected void LinkButton12_Click(object sender, EventArgs e)
        {
            Contract_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Contract_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/Contract_PDF.pdf";
            WordcvDdf(path_temp, wordfilepath);
            File.Delete(path_temp);
            FileInfo file = new FileInfo(wordfilepath);
            string filepath = purch + "合約.pdf";
            string fileName = HttpUtility.UrlEncode(filepath);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            Response.End();
        }
        protected void LinkButton18_Click(object sender, EventArgs e)
        {
            var strPurchNum = lblOVC_PURCH.Text.Substring(0, 7);
            Contract_ExportToWord();
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Contract_Temp.docx");
            string path_end = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Contract_Temp.odt");
            string filepath = strPurchNum + "合約.odt";
            string fileName = HttpUtility.UrlEncode(filepath);
            WordToODT(path_temp, path_end, fileName);
        }
        //列印時程管制表
        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            PrinterServlet_ExportToWord();
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/PS_Temp.docx");
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "時程管制表.docx";
            string fileName = HttpUtility.UrlEncode(filepath);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            File.Delete(path_temp);
            Response.End();
        }
        protected void LinkButton11_Click(object sender, EventArgs e)
        {
            PrinterServlet_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/PS_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/PS_PDF.pdf";
            WordcvDdf(path_temp, wordfilepath);
            File.Delete(path_temp);
            FileInfo file = new FileInfo(wordfilepath);
            string filepath = purch + "時程管制表.pdf";
            string fileName = HttpUtility.UrlEncode(filepath);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            Response.End();
        }
        protected void LinkButton17_Click(object sender, EventArgs e)
        {
            var strPurchNum = lblOVC_PURCH.Text.Substring(0, 7);
            PrinterServlet_ExportToWord();
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/PS_Temp.docx");
            string path_end = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/PS_Temp.odt");
            string filepath = strPurchNum + "時程管制表.odt";
            string fileName = HttpUtility.UrlEncode(filepath);
            WordToODT(path_temp, path_end, fileName);
        }
        //列印檢查項目表
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            Session["rowtext"] = ViewState["rowtext"].ToString();
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E13_print.aspx";
            Response.Redirect(send_url);
        }
        //列印檢查項目表word
        protected void LinkButton7_Click(object sender, EventArgs e)
        {
            PrinterServlet2_ExportToWord();
        }
        #endregion

        #region 回主流程
        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            string purch_6 = Session["purch_6"].ToString();
            var status = mpms.TBMSTATUS
                .Where(table => table.OVC_STATUS.Equals("31"))
                .Where(o => o.OVC_PURCH_6.Equals(purch_6))
                .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7)))
                .Where(table => lblOVC_PURCH.Text.Contains(table.OVC_PURCH_6)).FirstOrDefault();
            if (status != null)
            {
                Session["rowtext"] = ViewState["rowtext"].ToString();
                string send_url;
                send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
                Response.Redirect(send_url);
            }
            else
            {
                string send_url;
                send_url = "~/pages/MPMS/E/MPMS_E12.aspx";
                Response.Redirect(send_url);
            }
        }
        #endregion
        #endregion

        #region 副程式
        #region TB資料帶入
        private void GV_dataImport()
        {
            if (ViewState["rowtext"] != null && Session["rowgroup"] != null)
            {
                string purch_6 = Session["purch_6"].ToString();
                string strGroup = Session["rowgroup"].ToString();
                short sho = 0;
                short shoGroup = short.TryParse(strGroup, out sho) ? sho : sho;
                if (Session["E15"] == null)
                {
                    var userunit = Session["userunit"].ToString();
                    var purch = ViewState["rowtext"].ToString().Substring(0, 7);
                    var rowven = Session["rowven"].ToString();
                    var queryfirst =
                        (from tbm1302 in mpms.TBM1302
                         join tbm1301 in mpms.TBM1301_PLAN on tbm1302.OVC_PURCH equals tbm1301.OVC_PURCH
                         where tbm1301.OVC_PURCH.Equals(purch)
                         where tbm1302.OVC_PURCH_6.Equals(purch_6)
                         where tbm1302.OVC_DSEND != null && tbm1302.OVC_DSEND != " "
                         where tbm1302.OVC_DRECEIVE.Equals(null) || tbm1302.OVC_DRECEIVE.Equals(" ")
                         where userunit == "0A100" || userunit == "00N00" ? tbm1301.OVC_CONTRACT_UNIT.Equals("0A100") || tbm1301.OVC_CONTRACT_UNIT.Equals("00N00") : tbm1301.OVC_CONTRACT_UNIT.Equals(userunit)
                         where tbm1302.OVC_VEN_TITLE.Equals(rowven)
                         select new
                         {
                             OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                             OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                             OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE
                         }).FirstOrDefault();
                    if (queryfirst != null)
                    {
                        lblOVC_PURCH.Text = ViewState["rowtext"].ToString() + " (" + strGroup + ")";
                        lblOVC_PUR_IPURCH.Text = queryfirst.OVC_PUR_IPURCH;
                        txtOVC_VEN_TITLE.Text = queryfirst.OVC_VEN_TITLE;
                    }
                    var query =
                        (from tbm1302 in mpms.TBM1302
                         join tbm1301 in mpms.TBM1301_PLAN on tbm1302.OVC_PURCH equals tbm1301.OVC_PURCH
                         where tbm1302.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                         where tbm1302.OVC_PURCH_6.Equals(purch_6)
                         where tbm1301.OVC_PUR_IPURCH.Equals(lblOVC_PUR_IPURCH.Text)
                         where tbm1302.OVC_VEN_TITLE.Equals(txtOVC_VEN_TITLE.Text)
                         where tbm1302.OVC_DRECEIVE.Equals(null) || tbm1302.OVC_DRECEIVE.Equals(" ")
                         where tbm1302.ONB_GROUP.Equals(shoGroup)
                         select new
                         {
                             OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                             OVC_PUR_SECTION = tbm1301.OVC_PUR_SECTION,
                             OVC_PUR_USER = tbm1301.OVC_PUR_USER,
                             OVC_PUR_IUSER_PHONE = tbm1301.OVC_PUR_IUSER_PHONE,
                             OVC_PUR_IUSER_PHONE_EXT = tbm1301.OVC_PUR_IUSER_PHONE_EXT,
                             ONB_MONEY = tbm1302.ONB_MONEY,
                             OVC_DBID = tbm1302.OVC_DBID,
                             OVC_DCONTRACT = tbm1302.OVC_DCONTRACT,
                             OVC_PURCH = tbm1302.OVC_PURCH,
                             OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                             ONB_GROUP = tbm1302.ONB_GROUP,
                             OVC_VEN_CST = tbm1302.OVC_VEN_CST,
                             OVC_DSEND = tbm1302.OVC_DSEND,
                             OVC_DRECEIVE = tbm1302.OVC_DRECEIVE
                         }).FirstOrDefault();
                    if (query != null)
                    {
                        lblApplyDept.Text = query.OVC_PUR_NSECTION + "(" + query.OVC_PUR_SECTION + ") - " + query.OVC_PUR_USER + "(" + query.OVC_PUR_IUSER_PHONE + " 軍線：" + query.OVC_PUR_IUSER_PHONE_EXT + ")";
                        lblONB_MCONTRACT.Text = String.Format("{0:N}", query.ONB_MONEY);
                        lblOVC_DBID.Text = query.OVC_DBID;
                        lblOVC_DCONTRACT.Text = query.OVC_DCONTRACT;
                    }
                    var query1302 =
                        (from tbm1302 in mpms.TBM1302
                         where tbm1302.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                         where tbm1302.OVC_PURCH_6.Equals(purch_6)
                         select tbm1302.OVC_DSEND).FirstOrDefault();
                    if (query1302 != null)
                    {
                        txtOVC_DSEND.Text = query1302;
                        txtOVC_DRECEIVE.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                }
                else
                {
                    var userunit = Session["userunit"].ToString();
                    var purch = ViewState["rowtext"].ToString().Substring(0, 7);
                    var rowven = Session["rowven"].ToString();
                    var queryfirst =
                        (from tbm1302 in mpms.TBM1302
                         join tbm1301 in mpms.TBM1301_PLAN on tbm1302.OVC_PURCH equals tbm1301.OVC_PURCH
                         where tbm1301.OVC_PURCH.Equals(purch)
                         where tbm1302.OVC_DSEND != null && tbm1302.OVC_DSEND != " "
                         where !tbm1302.OVC_DRECEIVE.Equals(null) || tbm1302.OVC_DRECEIVE.Equals(" ")
                         //where tbm1301.OVC_CONTRACT_UNIT.Equals(userunit)
                         where userunit == "0A100" || userunit == "00N00" ? tbm1301.OVC_CONTRACT_UNIT.Equals("0A100") || tbm1301.OVC_CONTRACT_UNIT.Equals("00N00") : tbm1301.OVC_CONTRACT_UNIT.Equals(userunit)
                         where tbm1302.OVC_VEN_TITLE.Equals(rowven)
                         select new
                         {
                             OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                             OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                             OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE
                         }).FirstOrDefault();
                    if (queryfirst != null)
                    {
                        lblOVC_PURCH.Text = ViewState["rowtext"].ToString() + " (" + strGroup + ")";
                        lblOVC_PUR_IPURCH.Text = queryfirst.OVC_PUR_IPURCH;
                        txtOVC_VEN_TITLE.Text = queryfirst.OVC_VEN_TITLE;
                    }
                    var query =
                        (from tbm1302 in mpms.TBM1302
                         join tbm1301 in mpms.TBM1301_PLAN on tbm1302.OVC_PURCH equals tbm1301.OVC_PURCH
                         where tbm1302.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                         where tbm1302.OVC_PURCH_6.Equals(purch_6)
                         where tbm1301.OVC_PUR_IPURCH.Equals(lblOVC_PUR_IPURCH.Text)
                         where tbm1302.OVC_VEN_TITLE.Equals(txtOVC_VEN_TITLE.Text)
                         where tbm1302.ONB_GROUP.Equals(shoGroup)
                         select new
                         {
                             OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                             OVC_PUR_SECTION = tbm1301.OVC_PUR_SECTION,
                             OVC_PUR_USER = tbm1301.OVC_PUR_USER,
                             OVC_PUR_IUSER_PHONE = tbm1301.OVC_PUR_IUSER_PHONE,
                             OVC_PUR_IUSER_PHONE_EXT = tbm1301.OVC_PUR_IUSER_PHONE_EXT,
                             ONB_MONEY = tbm1302.ONB_MONEY,
                             OVC_DBID = tbm1302.OVC_DBID,
                             OVC_DCONTRACT = tbm1302.OVC_DCONTRACT,

                             OVC_PURCH = tbm1302.OVC_PURCH,
                             OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                             ONB_GROUP = tbm1302.ONB_GROUP,
                             OVC_VEN_CST = tbm1302.OVC_VEN_CST,
                             OVC_DSEND = tbm1302.OVC_DSEND,
                             OVC_DRECEIVE = tbm1302.OVC_DRECEIVE
                         }).FirstOrDefault();
                    if (query != null)
                    {
                        lblApplyDept.Text = query.OVC_PUR_NSECTION + "(" + query.OVC_PUR_SECTION + ") - " + query.OVC_PUR_USER + "(" + query.OVC_PUR_IUSER_PHONE + " 軍線：" + query.OVC_PUR_IUSER_PHONE_EXT + ")";
                        lblONB_MCONTRACT.Text = query.ONB_MONEY.ToString();
                        lblOVC_DBID.Text = query.OVC_DBID;
                        lblOVC_DCONTRACT.Text = query.OVC_DCONTRACT;
                    }
                    var queryBid =
                        (from tbmreceove_bid in mpms.TBMRECEIVE_BID
                         join tbmreceive_check in mpms.TBMRECEIVE_CHECK on tbmreceove_bid.OVC_PURCH equals tbmreceive_check.OVC_PURCH
                         where tbmreceove_bid.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                         select new
                         {
                             OVC_DO_NAME_RESULT = tbmreceove_bid.OVC_DO_NAME_RESULT,
                             OVC_REJECT_REASON_DO = tbmreceove_bid.OVC_REJECT_REASON_DO,
                             OVC_DSEND = tbmreceive_check.OVC_DSEND,
                             OVC_DRECEIVE = tbmreceive_check.OVC_DRECEIVE,
                             OVC_DO_NAME = tbmreceive_check.OVC_DO_NAME,
                             OVC_DO_DAPPROVE = tbmreceive_check.OVC_DO_DAPPROVE
                         }).FirstOrDefault();
                    if (queryBid != null)
                    {
                        if (queryBid.OVC_DO_NAME_RESULT != null)
                            rdoOVC_DO_NAME_RESULT.SelectedIndex = Convert.ToInt32(queryBid.OVC_DO_NAME_RESULT) - 1;
                        Reason_for_withdrawal_textbox.Text = queryBid.OVC_REJECT_REASON_DO;
                        txtOVC_DO_DAPPROVE.Text = queryBid.OVC_DO_DAPPROVE;
                    }
                    var query1302 =
                        (from tbm1302 in mpms.TBM1302
                         where tbm1302.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                         where tbm1302.OVC_PURCH_6.Equals(purch_6)
                         select tbm1302.OVC_DSEND).FirstOrDefault();
                    if (query1302 != null)
                    {
                        txtOVC_DSEND.Text = query1302;
                    }
                    var queryContract =
                        (from tbmcontract in mpms.TBMRECEIVE_CONTRACT
                         where tbmcontract.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                         where tbmcontract.OVC_PURCH_6.Equals(purch_6)
                         select new
                         {
                             OVC_DRECEIVE = tbmcontract.OVC_DRECEIVE,
                             OVC_DO_NAME = tbmcontract.OVC_DO_NAME
                         }).FirstOrDefault();
                    if (queryContract != null)
                    {
                        txtOVC_DRECEIVE.Text = queryContract.OVC_DRECEIVE;
                        drpOVC_DO_NAME.Text = queryContract.OVC_DO_NAME;
                    }
                }
            }

            #region 決標、簽約日期=>民國日期
            DateTime dt;
            if (DateTime.TryParse(lblOVC_DBID.Text, out dt))
            {
                dt = DateTime.Parse(lblOVC_DBID.Text);
                lblOVC_DBID.Text = (int.Parse(dt.Year.ToString()) - 1911).ToString() + "年" + dt.Month.ToString() + "月" + dt.Day.ToString() + "日";
            }
            if (DateTime.TryParse(lblOVC_DCONTRACT.Text, out dt))
            {
                dt = DateTime.Parse(lblOVC_DCONTRACT.Text);
                lblOVC_DCONTRACT.Text = (int.Parse(dt.Year.ToString()) - 1911).ToString() + "年" + dt.Month.ToString() + "月" + dt.Day.ToString() + "日";
            }
            #endregion
        }
        #endregion

        #region 檢查項目表資料帶入
        private void CheckItem_dataImport()
        {
            if (ViewState["rowtext"] != null)
            {
                var pur = ViewState["rowtext"].ToString().Substring(0, 7);
                string purch_6 = Session["purch_6"].ToString();
                var query =
                    from tbmreceive_check in mpms.TBMRECEIVE_CHECK
                    join tbmreceive_check_item in mpms.TBMRECEIVE_CHECK_ITEM on tbmreceive_check.OVC_PURCH equals tbmreceive_check_item.OVC_PURCH
                    where tbmreceive_check.OVC_PURCH.Equals(pur)
                    where tbmreceive_check.OVC_PURCH_6.Equals(purch_6)
                    where tbmreceive_check.OVC_PURCH_6.Equals(tbmreceive_check_item.OVC_PURCH_6)
                    select new
                    {
                        OVC_ITEM_NAME = tbmreceive_check_item.OVC_ITEM_NAME,
                        OVC_RESULT = tbmreceive_check_item.OVC_RESULT
                    };
                foreach (var q in query)
                {
                    for (int i = 1; i < 9; i++)
                    {
                        string lab = "lblQ1_" + i;
                        string drp = "drpCheck1_" + i;
                        Label label = (Label)tbItem.FindControl(lab);
                        DropDownList dropDownList = (DropDownList)tbItem.FindControl(drp);
                        string result = dropDownList.SelectedValue;
                        if (q.OVC_ITEM_NAME == label.Text)
                            dropDownList.Text = q.OVC_RESULT;
                    }
                    for (int i = 1; i < 20; i++)
                    {
                        string lab = "lblQ2_" + i;
                        string drp = "drpCheck2_" + i;
                        Label label = (Label)tbItem.FindControl(lab);
                        DropDownList dropDownList = (DropDownList)tbItem.FindControl(drp);
                        string result = dropDownList.SelectedValue;
                        if (q.OVC_ITEM_NAME == label.Text)
                            dropDownList.Text = q.OVC_RESULT;
                    }
                    for (int i = 1; i < 12; i++)
                    {
                        string lab = "lblQ3_" + i;
                        string drp = "drpCheck3_" + i;
                        Label label = (Label)tbItem.FindControl(lab);
                        DropDownList dropDownList = (DropDownList)tbItem.FindControl(drp);
                        string result = dropDownList.SelectedValue;
                        if (q.OVC_ITEM_NAME == label.Text)
                            dropDownList.Text = q.OVC_RESULT;
                    }
                }
            }
        }
        #endregion

        #region rdo顯示退案原因TextBox
        protected void rdoOVC_DO_NAME_RESULT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoOVC_DO_NAME_RESULT.Text.Equals("洽採購發包單位澄清後辦理"))
                reason.Visible = true;
            else
                reason.Visible = false;
            Reason_for_withdrawal_textbox.Text = "";
        }
        #endregion

        #region 履驗承辦人drp資料帶入
        private void DT_DoName_dataImport()
        {
            if (Session["userunit"] != null)
            {
                var userunit = Session["userunit"].ToString();
                var query =
                      (from account in mpms.ACCOUNT
                      join tbm5200_1 in mpms.TBM5200_1 on account.USER_ID equals tbm5200_1.USER_ID
                      where userunit == "0A100" || userunit == "00N00" ? account.DEPT_SN.Equals("0A100") || account.DEPT_SN.Equals("00N00") : account.DEPT_SN.Equals(userunit)
                      where tbm5200_1.C_SN_ROLE.Equals("33")
                      where tbm5200_1.OVC_PRIV_LEVEL.Equals("7")
                      group account by account.USER_NAME into grp
                      select grp.Key).Union
                      (from account in mpms.ACCOUNT
                       join account_auth in mpms.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                       where userunit == "0A100" || userunit == "00N00" ? account.DEPT_SN.Equals("0A100") || account.DEPT_SN.Equals("00N00") : account.DEPT_SN.Equals(userunit)
                       where account_auth.C_SN_ROLE.Equals("34")
                       where account_auth.C_SN_AUTH.Equals("3403")
                       group account by account.USER_NAME into grp
                       select grp.Key);
                
                foreach (var e in query)
                {
                    drpOVC_DO_NAME.Items.Add(e);
                }
                var querydoname =
                    from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                    where tbmreceive.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    select new
                    {
                        tbmreceive.OVC_DO_NAME
                    };
                if (querydoname.Count() > 0)
                {
                    foreach (var q in querydoname)
                    {
                        drpOVC_DO_NAME.Text = q.OVC_DO_NAME;
                    }
                }
                else
                    drpOVC_DO_NAME.Text = Session["username"].ToString();

            }
        }
        #endregion

        #region 物資申請書-內購
        private void PrintPDF()
        {
            string angcy = "";
            var strPurchNum = lblOVC_PURCH.Text.Substring(0, 7);
            //PDF列印
            var doc1 = new iTextSharp.text.Document(PageSize.A4, 50, 50, 45, 50);
            MemoryStream Memory = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc1, new FileStream(Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Material.pdf"), FileMode.Create));//檔案 下載
            Watermark wm = new Watermark();
            wm.strPurchNum = strPurchNum;
            writer.PageEvent = wm;
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            iTextSharp.text.Font ChFont = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            iTextSharp.text.Font ChFont_blue = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.NORMAL, new BaseColor(2, 0, 255));
            iTextSharp.text.Font ChFont_msg = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.ITALIC, BaseColor.RED);
            iTextSharp.text.Font ChFont_memo = new iTextSharp.text.Font(bfChinese, 8, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            doc1.Open();

            //page1
            #region Page1
            var t1301 =
                (from t in gm.TBM1301
                 join t1407 in gm.TBM1407 on t.OVC_PUR_CURRENT equals t1407.OVC_PHR_ID
                 where t.OVC_PURCH.Equals(strPurchNum) && t1407.OVC_PHR_CATE.Equals("B0")
                 select new
                 {
                     t.OVC_PURCH_KIND,
                     t.OVC_PUR_AGENCY,
                     t.OVC_PUR_NSECTION,
                     t.OVC_PUR_DAPPROVE,
                     t.OVC_PUR_APPROVE,
                     t.OVC_DPROPOSE,
                     t.OVC_PROPOSE,
                     t.OVC_PUR_IPURCH,
                     OVC_PUR_CURRENT = t1407.OVC_PHR_DESC,
                     t.ONB_PUR_BUDGET,
                     t.OVC_PUR_SECTION,
                     t.OVC_SECTION_CHIEF,
                     t.OVC_AGNT_IN
                 }).FirstOrDefault();
            if (t1301.OVC_PURCH_KIND.Equals("1"))
                angcy = "內購";
            else
                angcy = "外購";
            var tablePurch = mpms.TBMPURCH_EXT.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            //table1
            PdfPTable table = new PdfPTable(new float[] { 1, 3, 1, 1, 1, 3, 2, 4 });
            table.TotalWidth = 500f;
            table.LockedWidth = true;
            PdfPCell header = new PdfPCell();
            Chunk glue = new Chunk(new VerticalPositionMark());
            string nsection = "";
            if (tablePurch == null)
            {
                nsection = t1301.OVC_PUR_NSECTION;
            }
            else
            {
                nsection = tablePurch.OVC_PUR_NSECTION_2;
            }
            string strp1 = nsection + angcy + "物資申請書";
            iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(strp1, ChFont);
            p.Add(new Chunk(glue));//有沒有其他寫法  無法垂直置中
            string strp2 = "中華民國" + GetTaiwanDate(t1301.OVC_PUR_DAPPROVE, "chDate") + "\n";
            p.Add(strp2);//核定日期
            p.Add(new Chunk(glue));
            p.Add(t1301.OVC_PUR_APPROVE);//核定文號
            p.Alignment = Element.ALIGN_MIDDLE;
            header.AddElement(p);
            header.Colspan = 8;
            table.AddCell(header);
            PdfPCell cell2_1 = new PdfPCell(new Phrase("申購單位", ChFont));
            table.AddCell(cell2_1);
            PdfPCell cell2_2 = new PdfPCell(new Phrase(t1301.OVC_PUR_NSECTION, ChFont));
            cell2_2.Colspan = 4;
            cell2_2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell2_2);
            PdfPCell cell2_3 = new PdfPCell(new Phrase("原申購日期及文號", ChFont));
            cell2_3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2_3.HorizontalAlignment = 1;
            table.AddCell(cell2_3);
            string strPropose = "中華民國" + GetTaiwanDate(t1301.OVC_DPROPOSE, "chDate") + "\n" + t1301.OVC_PROPOSE;
            PdfPCell cell2_4 = new PdfPCell(new Phrase(strPropose, ChFont));//申購日期文號
            cell2_4.Colspan = 2;
            cell2_4.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2_4.HorizontalAlignment = 2;
            table.AddCell(cell2_4);
            doc1.Add(table);

            //table2
            PdfPTable table2 = new PdfPTable(new float[] { 0.7f, 3, 1, 1, 1, 2, 1.5f, 4 });
            table2.TotalWidth = 500f;
            table2.LockedWidth = true;

            PdfPCell cell3_1 = new PdfPCell(new Phrase("物品種類及數量", ChFont));
            cell3_1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell3_1.HorizontalAlignment = 1;
            cell3_1.Rowspan = 4;
            table2.AddCell(cell3_1);
            PdfPCell cell3_2 = new PdfPCell(new Phrase("名稱", ChFont));
            cell3_2.HorizontalAlignment = 1;
            cell3_2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell3_2);
            PdfPCell cell3_3 = new PdfPCell(new Phrase("單位", ChFont));
            cell3_3.HorizontalAlignment = 1;
            cell3_3.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell3_3);
            PdfPCell cell3_4 = new PdfPCell(new Phrase("預估數", ChFont));
            cell3_4.HorizontalAlignment = 1;
            cell3_4.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell3_4);
            PdfPCell cell3_5 = new PdfPCell(new Phrase("單價", ChFont));
            cell3_5.HorizontalAlignment = 1;
            cell3_5.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell3_5);
            PdfPCell cell3_6 = new PdfPCell(new Phrase("預估貨款總價", ChFont));
            cell3_6.HorizontalAlignment = 1;
            cell3_6.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell3_6);
            PdfPCell cell3_7 = new PdfPCell(new Phrase("預算來源", ChFont));
            cell3_7.Colspan = 2;
            cell3_7.HorizontalAlignment = 1;
            cell3_7.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell3_7);
            PdfPCell cell4_1 = new PdfPCell(new Phrase(t1301.OVC_PUR_IPURCH + "，詳清單", ChFont));
            cell4_1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell4_1.Colspan = 5;
            cell4_1.Rowspan = 2;
            table2.AddCell(cell4_1);
            PdfPCell cell4_2 = new PdfPCell(new Phrase("款源", ChFont));
            cell4_2.HorizontalAlignment = 1;
            table2.AddCell(cell4_2);
            var t1118_ISOURCE = mpms.TBM1118.Where(o => o.OVC_PURCH.Equals(strPurchNum)).GroupBy(o => o.OVC_ISOURCE).Select(o => o.Key);
            string Isource = String.Join(";", t1118_ISOURCE);
            PdfPCell cell4_3 = new PdfPCell(new Phrase(Isource, ChFont));
            table2.AddCell(cell4_3);
            PdfPCell cell5_2 = new PdfPCell(new Phrase("科目", ChFont));
            cell5_2.HorizontalAlignment = 1;
            table2.AddCell(cell5_2);
            var t1118_PJNAME = mpms.TBM1118.Where(o => o.OVC_PURCH.Equals(strPurchNum)).GroupBy(o => o.OVC_PJNAME).Select(o => o.Key);
            string Pjname = String.Join(";", t1118_PJNAME);
            PdfPCell cell5_3 = new PdfPCell(new Phrase(Pjname, ChFont));
            table2.AddCell(cell5_3);
            PdfPCell cell6_1 = new PdfPCell(new Phrase("貨款總價", ChFont));
            cell6_1.HorizontalAlignment = 1;
            table2.AddCell(cell6_1);
            decimal moneyNT = (decimal)t1301.ONB_PUR_BUDGET;
            string money = FCommon.MoneyToChinese(moneyNT.ToString()) + "(含稅)";
            PdfPCell cell6_2 = new PdfPCell(new Phrase(t1301.OVC_PUR_CURRENT + " " + money, ChFont));
            cell6_2.Colspan = 4;
            table2.AddCell(cell6_2);
            PdfPCell cell6_3 = new PdfPCell(new Phrase("奉准日期及文號", ChFont));//***************
            cell6_3.HorizontalAlignment = 1;
            table2.AddCell(cell6_3);
            var queryDPPR_PLAN =
                from t in mpms.TBM1231
                where t.OVC_PURCH.Equals(strPurchNum)
                select new
                {
                    date = t.OVC_PUR_DAPPR_PLAN ?? "",
                    appr = t.OVC_PUR_APPR_PLAN
                };
            string OVC_PUR_DAPPR_PLAN_WITH = "";
            foreach (var item in queryDPPR_PLAN)
            {
                OVC_PUR_DAPPR_PLAN_WITH += item.date + item.appr + ";\n";
            }

            PdfPCell cell6_4 = new PdfPCell(new Phrase(OVC_PUR_DAPPR_PLAN_WITH, ChFont));
            table2.AddCell(cell6_4);
            PdfPCell cell7_1 = new PdfPCell(new Phrase("請\n\n求\n\n事\n\n項", ChFont));//*************
            cell7_1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell7_1.HorizontalAlignment = 1;
            table2.AddCell(cell7_1);
            PdfPCell cell7_2 = new PdfPCell();
            string ovcIkind = "";
            switch (t1301.OVC_PUR_AGENCY)
            {
                case "B":
                case "L":
                case "P":
                    ovcIkind = "M3";
                    break;
                case "M":
                case "S":
                    ovcIkind = "F3";
                    break;
                default:
                    ovcIkind = "W3";
                    break;
            }
            //請求事項
            var RequestMemo =
                from t in mpms.TBM1220_1
                join t12202 in mpms.TBM1220_2 on t.OVC_IKIND equals t12202.OVC_IKIND
                where t.OVC_IKIND.StartsWith(ovcIkind) && t.OVC_PURCH.Equals(strPurchNum)
                orderby t.OVC_IKIND
                select new
                {
                    title = t12202.OVC_MEMO_NAME,
                    memo = t.OVC_MEMO
                };
            foreach (var item in RequestMemo)
            {
                Phrase c7_2 = new Phrase(item.title + item.memo + "\n", ChFont_memo);
                cell7_2.AddElement(c7_2);
            }
            cell7_2.Colspan = 7;
            table2.AddCell(cell7_2);


            PdfPCell cell8_1 = new PdfPCell(new Phrase("附件", ChFont));
            cell8_1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell8_1.HorizontalAlignment = 1;
            table2.AddCell(cell8_1);

            PdfPCell cell8_2 = new PdfPCell(new Phrase(GetAttached("M"), ChFont));
            cell8_2.Colspan = 7;
            table2.AddCell(cell8_2);


            PdfPCell cell9_1 = new PdfPCell(new Phrase("備考", ChFont));
            cell9_1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell9_1.HorizontalAlignment = 1;
            table2.AddCell(cell9_1);
            PdfPCell cell9_2 = new PdfPCell();
            //備考
            switch (t1301.OVC_PUR_AGENCY)
            {
                case "B":
                case "L":
                case "P":
                    ovcIkind = "M4";
                    break;
                case "M":
                case "S":
                    ovcIkind = "F4";
                    break;
                default:
                    ovcIkind = "W4";
                    break;
            }
            var markMemo =
               from t in mpms.TBM1220_1
               join t12202 in mpms.TBM1220_2 on t.OVC_IKIND equals t12202.OVC_IKIND
               where t.OVC_IKIND.StartsWith(ovcIkind) && t.OVC_PURCH.Equals(strPurchNum)
               orderby t.OVC_IKIND
               select new
               {
                   title = t12202.OVC_MEMO_NAME,
                   memo = t.OVC_MEMO
               };
            foreach (var item in markMemo)
            {
                Phrase c9_2 = new Phrase(item.title + item.memo + "\n", ChFont_memo);
                cell9_2.AddElement(c9_2);
            }
            cell9_2.Colspan = 7;
            table2.AddCell(cell9_2);


            string toPhares = "";
            if (Session["userid"] != null)
            {
                string strUSER_ID = Session["userid"].ToString();
                string userid = Session["userid"].ToString();
                var queryUserDept = gm.ACCOUNTs.Where(o => o.USER_ID.Equals(userid)).FirstOrDefault();
                if (queryUserDept.DEPT_SN.ToString() == t1301.OVC_PUR_SECTION.ToString())
                    toPhares = "此致";
                else
                    toPhares = "謹呈";
            }
            PdfPCell cell10 = new PdfPCell();
            Chunk glue2 = new Chunk(new VerticalPositionMark());
            iTextSharp.text.Paragraph p2 = new iTextSharp.text.Paragraph(toPhares + "\n", ChFont);
            p2.FirstLineIndent = 25;
            p2.Add(t1301.OVC_PUR_NSECTION);
            p2.Add(new Chunk(glue2));
            p2.Add("主官" + t1301.OVC_SECTION_CHIEF);//主官
            cell10.AddElement(p2);
            cell10.Colspan = 8;
            table2.AddCell(cell10);
            PdfPCell cell11_1 = new PdfPCell(new Phrase("審查意見", ChFont));
            cell11_1.Colspan = 6;
            cell11_1.HorizontalAlignment = Element.ALIGN_CENTER;
            table2.AddCell(cell11_1);
            iTextSharp.text.Paragraph p11_2 = new iTextSharp.text.Paragraph();
            Phrase c11_2_1 = new Phrase("承辦人", ChFont);
            p11_2.Leading = 60; //設定行距（每行之間的距離） 
            Phrase c11_2_2 = new Phrase("核稿人\n", ChFont);
            p11_2.Add(c11_2_2);
            Phrase c11_2_3 = new Phrase("核判人\n", ChFont);
            p11_2.Add(c11_2_3);
            p11_2.Add("\n");

            PdfPCell cell11_2 = new PdfPCell();
            cell11_2.AddElement(c11_2_1);
            cell11_2.AddElement(p11_2);
            cell11_2.Colspan = 2;
            cell11_2.Rowspan = 9;
            table2.AddCell(cell11_2);

            PdfPCell cell12_1 = new PdfPCell(new Phrase("會辦單位", ChFont));
            cell12_1.Colspan = 2;
            cell12_1.Rowspan = 1;
            cell12_1.HorizontalAlignment = Element.ALIGN_CENTER;
            table2.AddCell(cell12_1);

            PdfPCell cell12_2 = new PdfPCell(new Phrase("會辦單位", ChFont));
            cell12_2.Colspan = 2;
            cell12_2.Rowspan = 1;
            cell12_2.HorizontalAlignment = Element.ALIGN_CENTER;
            table2.AddCell(cell12_2);

            PdfPCell cell12_3 = new PdfPCell(new Phrase("承辦單位", ChFont));
            cell12_3.Colspan = 2;
            cell12_3.Rowspan = 1;
            cell12_3.HorizontalAlignment = Element.ALIGN_CENTER;
            table2.AddCell(cell12_3);

            PdfPCell cell13_1 = new PdfPCell(new Phrase());
            cell13_1.Colspan = 2;
            cell13_1.Rowspan = 7;
            table2.AddCell(cell13_1);
            PdfPCell cell13_2 = new PdfPCell(new Phrase());
            cell13_2.Colspan = 2;
            cell13_2.Rowspan = 7;
            table2.AddCell(cell13_2);
            PdfPCell cell13_3 = new PdfPCell(new Phrase());
            cell13_3.Colspan = 2;
            cell13_3.Rowspan = 7;
            table2.AddCell(cell13_3);
            doc1.Add(table2);
            #endregion


            //page3
            #region page3
            doc1.NewPage();
            table = new PdfPTable(new float[] { 1, 3, 1, 1, 1, 3, 2, 4 });
            table.TotalWidth = 500f;
            table.LockedWidth = true;
            header = new PdfPCell();
            glue = new Chunk(new VerticalPositionMark());

            p = new iTextSharp.text.Paragraph(t1301.OVC_PUR_NSECTION + angcy + "物資核定書", ChFont);//標題
            p.Add(new Chunk(glue));//有沒有其他寫法  無法垂直置中
            p.Add(strp2);
            p.Add(new Chunk(glue));
            p.Add(t1301.OVC_PUR_APPROVE);
            p.Alignment = Element.ALIGN_MIDDLE;
            header.AddElement(p);
            header.Colspan = 8;
            table.AddCell(header);
            table.AddCell(cell2_1);
            table.AddCell(cell2_2);
            cell2_3 = new PdfPCell(new Phrase("申購日期及文號", ChFont));
            cell2_3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2_3.HorizontalAlignment = 1;
            table.AddCell(cell2_3);
            table.AddCell(cell2_4);
            doc1.Add(table);
            //table2
            table2 = new PdfPTable(new float[] { 0.7f, 3, 1, 1, 1, 2, 1.5f, 4 });
            table2.TotalWidth = 500f;
            table2.LockedWidth = true;

            table2.AddCell(cell3_1);
            table2.AddCell(cell3_2);
            table2.AddCell(cell3_3);
            table2.AddCell(cell3_4);
            table2.AddCell(cell3_5);
            table2.AddCell(cell3_6);
            table2.AddCell(cell3_7);
            table2.AddCell(cell4_1);
            table2.AddCell(cell4_2);
            table2.AddCell(cell4_3);
            table2.AddCell(cell5_2);
            table2.AddCell(cell5_3);
            table2.AddCell(cell6_1);
            table2.AddCell(cell6_2);
            table2.AddCell(cell6_3);
            table2.AddCell(cell6_4);
            table2.AddCell(cell7_1);
            table2.AddCell(cell7_2);
            table2.AddCell(cell8_1);
            table2.AddCell(cell8_2);
            table2.AddCell(cell9_1);
            table2.AddCell(cell9_2);
            PdfPCell cell10_1 = new PdfPCell(new Phrase("審查意見", ChFont));
            table2.AddCell(cell10_1);
            var queryComment = mpms.TBM1202.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_CHECK_OK.Equals("Y")).Select(o => o.OVC_APPROVE_COMMENT).FirstOrDefault();
            PdfPCell cell10_2 = new PdfPCell(new Phrase(queryComment, ChFont_memo));
            cell10_2.Colspan = 7;
            table2.AddCell(cell10_2);
            PdfPCell cell11 = new PdfPCell();
            p2 = new iTextSharp.text.Paragraph(new Phrase("此令\n", ChFont));
            p2.FirstLineIndent = -5;
            p2.Add(t1301.OVC_AGNT_IN + "                  ");//空格是否有更好的方法
            p2.IndentationLeft = 30;
            p2.Add("主官");
            cell11.AddElement(p2);
            cell11.Colspan = 8;
            cell11.PaddingTop = -4;
            cell11.FixedHeight = 80;
            table2.AddCell(cell11);
            doc1.Add(table2);
            #endregion

            //pag4
            #region page4
            doc1.NewPage();
            table = new PdfPTable(new float[] { 1, 3, 1, 1, 1, 3, 2, 4 });
            table.TotalWidth = 500f;
            table.LockedWidth = true;
            header = new PdfPCell();
            glue = new Chunk(new VerticalPositionMark());
            p = new iTextSharp.text.Paragraph(t1301.OVC_PUR_NSECTION + angcy + "物資核定書", ChFont);//標題
            p.Add(new Chunk(glue));//有沒有其他寫法  無法垂直置中
            p.Add(strp2);
            p.Add(new Chunk(glue));
            p.Add(t1301.OVC_PUR_APPROVE);
            p.Alignment = Element.ALIGN_MIDDLE;
            header.AddElement(p);
            header.Colspan = 8;
            table.AddCell(header);
            table.AddCell(cell2_1);
            table.AddCell(cell2_2);
            cell2_3 = new PdfPCell(new Phrase("申購日期及文號", ChFont));
            cell2_3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2_3.HorizontalAlignment = 1;
            table.AddCell(cell2_3);
            table.AddCell(cell2_4);
            doc1.Add(table);
            //table2
            table2 = new PdfPTable(new float[] { 0.7f, 3, 1, 1, 1, 2, 1.5f, 4 });
            table2.TotalWidth = 500f;
            table2.LockedWidth = true;

            table2.AddCell(cell3_1);
            table2.AddCell(cell3_2);
            table2.AddCell(cell3_3);
            table2.AddCell(cell3_4);
            table2.AddCell(cell3_5);
            table2.AddCell(cell3_6);
            table2.AddCell(cell3_7);
            table2.AddCell(cell4_1);
            table2.AddCell(cell4_2);
            table2.AddCell(cell4_3);
            table2.AddCell(cell5_2);
            table2.AddCell(cell5_3);
            table2.AddCell(cell6_1);
            table2.AddCell(cell6_2);
            table2.AddCell(cell6_3);
            table2.AddCell(cell6_4);
            table2.AddCell(cell7_1);
            table2.AddCell(cell7_2);
            table2.AddCell(cell8_1);
            table2.AddCell(cell8_2);
            table2.AddCell(cell9_1);
            table2.AddCell(cell9_2);
            table2.AddCell(cell10_1);
            table2.AddCell(cell10_2);
            PdfPCell cell11_3 = new PdfPCell();
            p2 = new iTextSharp.text.Paragraph(new Phrase("此令\n", ChFont));
            p2.FirstLineIndent = -5;
            p2.Add(t1301.OVC_PUR_NSECTION + "                  ");//空格是否有更好的方法
            p2.IndentationLeft = 30;
            p2.Add("主官");
            cell11_3.AddElement(p2);
            cell11_3.Colspan = 8;
            cell11_3.PaddingTop = -4;
            cell11_3.FixedHeight = 80;
            table2.AddCell(cell11_3);
            doc1.Add(table2);
            #endregion
            
            string filepath = strPurchNum + "物資申請書.pdf";
            string fileName = HttpUtility.UrlEncode(filepath);

            doc1.Close();

            //doc1.Close();
            //Response.Clear();//瀏覽器上顯示
            //Response.AddHeader("Content-disposition", "attachment;filename=" + fileName);
            //Response.ContentType = "application/octet-stream";
            //Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            //Response.OutputStream.Flush();
            //Response.Flush();
            //Response.End();
        }
        #endregion

        #region 物資申請書-外購
        public void MaterialApplication_ExportToWord()
        {
            string ovcIkind = "";
            string ovcIkind_2 = "";
            int year = 0;
            string date = "";
            string wordfilepath = "";
            string path = "";
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            var query =
                from tbm1301 in mpms.TBM1301
                where tbm1301.OVC_PURCH.Equals(purch)
                select new
                {
                    OVC_PURCH_KIND = tbm1301.OVC_PURCH_KIND,
                    OVC_PUR_DAPPROVE = tbm1301.OVC_PUR_DAPPROVE,
                    OVC_PUR_APPROVE = tbm1301.OVC_PUR_APPROVE,
                    OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                    OVC_DPROPOSE = tbm1301.OVC_DPROPOSE,
                    OVC_PROPOSE = tbm1301.OVC_PROPOSE,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    OVC_PUR_IPURCH_ENG = tbm1301.OVC_PUR_IPURCH_ENG,
                    ONB_PUR_BUDGET_NT = tbm1301.ONB_PUR_BUDGET_NT,
                    OVC_SECTION_CHIEF = tbm1301.OVC_SECTION_CHIEF,
                    OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY
                };
            if (query.Count() < 1)
                return;
            foreach (var q in query)
            {
                if (q.OVC_PURCH_KIND.Equals("1"))
                {
                    path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/MaterialApplication_in.docx");
                }
                else
                {
                    if (q.OVC_PUR_AGENCY != "F" && q.OVC_PUR_AGENCY != "W")
                        path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/MaterialApplication_Kind2.docx");
                    else
                        path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/MaterialApplication.docx");
                }
            }
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/MaterialApplication.pdf";
                    foreach (var q in query)
                    {
                        doc.ReplaceText("[$OVC_PUR_NSECTION$]", q.OVC_PUR_NSECTION != null ? q.OVC_PUR_NSECTION : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        var queryPurch =
                            from tbm1301 in mpms.TBM1301_PLAN
                            where tbm1301.OVC_PURCH.Equals(purch)
                            select new
                            {
                                OVC_PURCH = tbm1301.OVC_PURCH,
                                OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY
                            };
                        foreach (var qu in queryPurch)
                        {
                            doc.ReplaceText("[$PURCH$]", qu.OVC_PURCH + qu.OVC_PUR_AGENCY, false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$Barcode$]", qu.OVC_PURCH, false, System.Text.RegularExpressions.RegexOptions.None);//條碼字型IDAutomationHC39M
                        }
                        doc.ReplaceText("[$OVC_PURCH_KIND$]", "外", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_ATTACH_NAME_2$]", GetAttached("M", "9"), false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_ATTACH_NAME_2$]", "採購輸出入計畫清單九份", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_ATTACH_NAME_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_ATTACH_NAME_4$]", "採購計畫清單三份", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_ATTACH_NAME_5$]", "採購輸出入計畫清單一份", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_ATTACH_NAME_6$]", "採購輸出入計畫清單一份", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_ATTACH_NAME_7$]", "採購輸出入計畫清單一份", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_ATTACH_NAME_8$]", "採購輸出入計畫清單二份", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_ATTACH_NAME_9$]", "採購輸出入計畫清單一份", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_ATTACH_NAME_10$]", "採購輸出入計畫清單一份", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_PUR_DAPPROVE != null)
                        {
                            year = int.Parse(q.OVC_PUR_DAPPROVE.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_PUR_DAPPROVE.Substring(5, 2) + "月" + q.OVC_PUR_DAPPROVE.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_PUR_DAPPROVE$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_PUR_DAPPROVE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_APPROVE$]", q.OVC_PUR_APPROVE != null ? q.OVC_PUR_APPROVE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_NSECTION$]", q.OVC_PUR_NSECTION != null ? q.OVC_PUR_NSECTION : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DPROPOSE != null)
                        {
                            year = int.Parse(q.OVC_DPROPOSE.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DPROPOSE.Substring(5, 2) + "月" + q.OVC_DPROPOSE.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DPROPOSE$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DPROPOSE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PROPOSE$]", q.OVC_PROPOSE != null ? q.OVC_PROPOSE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_IPURCH$]", q.OVC_PUR_IPURCH != null ? q.OVC_PUR_IPURCH : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_PUR_IPURCH_ENG$]", q.OVC_PUR_IPURCH_ENG != null ? "(" + q.OVC_PUR_IPURCH_ENG + ")" : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_PUR_BUDGET_NT != null)
                        {
                            string money = EastAsiaNumericFormatter.FormatWithCulture("Lc", q.ONB_PUR_BUDGET_NT, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                            doc.ReplaceText("[$ONB_PUR_BUDGET_NT$]", money, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$ONB_PUR_BUDGET_NT$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_SECTION_CHIEF$]", q.OVC_SECTION_CHIEF != null ? q.OVC_SECTION_CHIEF : "", false, System.Text.RegularExpressions.RegexOptions.None);

                        switch (q.OVC_PUR_AGENCY)
                        {
                            case "B":
                            case "L":
                            case "P":
                                ovcIkind = "M3";
                                ovcIkind_2 = "M4";
                                break;
                            case "M":
                            case "S":
                                ovcIkind = "F3";
                                ovcIkind_2 = "F4";
                                break;
                            default:
                                ovcIkind = "W3";
                                ovcIkind_2 = "W4";
                                break;
                        }
                    }
                    year = int.Parse(DateTime.Now.ToString("yyyyMMdd").Substring(0, 4)) - 1911;
                    date = year.ToString() + "." + DateTime.Now.Month + "." + DateTime.Now.Day;
                    doc.ReplaceText("[$DATE$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$TIME$]", DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString(), false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryCurrent =
                        from tbm1407 in mpms.TBM1407
                        join tbm1301 in mpms.TBM1301
                            on tbm1407.OVC_PHR_ID equals tbm1301.OVC_PUR_CURRENT
                        where tbm1301.OVC_PURCH.Equals(purch)
                        where tbm1407.OVC_PHR_CATE.Equals("B0")
                        select tbm1407.OVC_PHR_DESC;
                    foreach (var qu in queryCurrent)
                    {
                        if (qu != null)
                            doc.ReplaceText("[$OVC_PUR_CURRENT$]", qu, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_PUR_CURRENT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var query1118 =
                        from tbm1118 in mpms.TBM1118
                        where tbm1118.OVC_PURCH.Equals(purch)
                        select new
                        {
                            OVC_ISOURCE = tbm1118.OVC_ISOURCE,
                            OVC_POI_IBDG = tbm1118.OVC_POI_IBDG,
                            OVC_PJNAME = tbm1118.OVC_PJNAME
                        };
                    foreach (var q in query1118)
                    {
                        if (q.OVC_ISOURCE != null)
                            doc.ReplaceText("[$OVC_ISOURCE$]", q.OVC_ISOURCE, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_POI_IBDG != null)
                            doc.ReplaceText("[$OVC_POI_IBDG$]", q.OVC_POI_IBDG, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_PJNAME != null)
                            doc.ReplaceText("[$OVC_PJNAME$]", q.OVC_PJNAME, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_ISOURCE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_POI_IBDG$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_PJNAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var query1231 =
                        from tbm1231 in mpms.TBM1231
                        where tbm1231.OVC_PURCH.Equals(purch)
                        select tbm1231.OVC_PUR_APPR_PLAN;
                    foreach (var q in query1231)
                    {
                        if (q != null)
                            doc.ReplaceText("[$OVC_PUR_APPR_PLAN$]", q, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_PUR_APPR_PLAN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var query1202 =
                        from tbm1202 in mpms.TBM1202
                        where tbm1202.OVC_PURCH.Equals(purch)
                        select tbm1202.OVC_APPROVE_COMMENT;
                    foreach (var q in query1202)
                    {
                        if (q != null)
                            doc.ReplaceText("[$OVC_APPROVE_COMMENT$]", q, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_APPROVE_COMMENT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_INTEGRATED_REASON$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    table_AddRow(doc, "Page1_1", ovcIkind);
                    table_AddRow(doc, "Page2_1", ovcIkind);
                    table_AddRow(doc, "Page3_1", ovcIkind);
                    table_AddRow(doc, "Page4_1", ovcIkind);
                    table_AddRow(doc, "Page5_1", ovcIkind);
                    table_AddRow(doc, "Page6_1", ovcIkind);
                    table_AddRow(doc, "Page7_1", ovcIkind);
                    table_AddRow(doc, "Page8_1", ovcIkind);
                    table_AddRow(doc, "Page9_1", ovcIkind);
                    table_AddRow(doc, "Page10_1", ovcIkind);
                    table_AddRow(doc, "Page11_1", ovcIkind);
                    table2_AddRow(doc, "Page1_2", 6, ovcIkind_2);
                    table2_AddRow(doc, "Page2_2", 4, ovcIkind_2);
                    table2_AddRow(doc, "Page3_2", 4, ovcIkind_2);
                    table2_AddRow(doc, "Page4_2", 4, ovcIkind_2);
                    table2_AddRow(doc, "Page5_2", 4, ovcIkind_2);
                    table2_AddRow(doc, "Page6_2", 4, ovcIkind_2);
                    table2_AddRow(doc, "Page7_2", 4, ovcIkind_2);
                    table2_AddRow(doc, "Page8_2", 4, ovcIkind_2);
                    table2_AddRow(doc, "Page9_2", 4, ovcIkind_2);
                    table2_AddRow(doc, "Page10_2", 4, ovcIkind_2);
                    table2_AddRow(doc, "Page11_2", 4, ovcIkind_2);
                    doc.ReplaceText("[$MEMO_M11_N$][$MEMO_M11_Y$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$MEMO_M21_N$][$MEMO_M21_Y$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_APPROVE_COMMENT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/MA_Temp.docx");
                }
                buffer = ms.ToArray();
            }
            //string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/MA_Temp.docx");
            //WordcvDdf(path_temp, wordfilepath);
            //File.Delete(path_temp);
            //FileInfo file = new FileInfo(wordfilepath);
            //string filepath = purch + "物資申請書.pdf";
            //string fileName = HttpUtility.UrlEncode(filepath);
            //Response.Clear();
            //Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            //Response.ContentType = "application/octet-stream";
            //Response.WriteFile(file.FullName);
            //Response.OutputStream.Flush();
            //Response.OutputStream.Close();
            //Response.Flush();
            //Response.End();
        }
        #region 表單newRow
        private string GetAttached(string kind, string page)
        {
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            //附件內容
            var queryFile =
                mpms.TBM1119.AsEnumerable()
                .Where(o => o.OVC_PURCH.Equals(purch) && o.OVC_IKIND.Equals(kind))
                .OrderByDescending(o => o.OVC_ATTACH_NAME.Split('.')[0].Length)
                .OrderBy(o => o.OVC_ATTACH_NAME.Split('.')[0]);
            string[] arrFileName = new string[queryFile.Count()];
            int counter = 0;
            foreach (var row in queryFile)
            {
                if (row.OVC_ATTACH_NAME == "採購計畫清單")
                {
                    if (row.ONB_PAGES == 0)
                        arrFileName[counter] = row.OVC_ATTACH_NAME + page + "份";
                    else
                        arrFileName[counter] = row.OVC_ATTACH_NAME + page + "份(" + row.ONB_PAGES.ToString() + "頁)";
                }
                else
                {
                    if (row.ONB_PAGES == 0)
                        arrFileName[counter] = row.OVC_ATTACH_NAME + row.ONB_QTY.ToString() + "份";
                    else
                        arrFileName[counter] = row.OVC_ATTACH_NAME + row.ONB_QTY.ToString() + "份(" + row.ONB_PAGES.ToString() + "頁)";
                }
                counter++;
            }
            string strFileName = string.Join("、", arrFileName);
            return strFileName;
        }
        private void table_AddRow(DocX doc, string Page, string ovcIkind)
        {
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            var groceryListTable = doc.Tables.FirstOrDefault(table => table.TableCaption == Page);
            if (groceryListTable != null)
            {
                var rowPattern = groceryListTable.Rows[groceryListTable.Rows.Count - 5];
                var rowPattern_N = groceryListTable.Rows[groceryListTable.Rows.Count - 4];
                var rowPattern_Y = groceryListTable.Rows[groceryListTable.Rows.Count - 3];
                rowPattern_N.Remove();
                rowPattern_Y.Remove();
                var query1220 =
                    from tbm1220_1 in mpms.TBM1220_1
                    where tbm1220_1.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_IKIND = tbm1220_1.OVC_IKIND,
                        OVC_MEMO = tbm1220_1.OVC_MEMO,
                        OVC_STANDARD = tbm1220_1.OVC_STANDARD
                    };
                int count = 0;
                foreach (var q in query1220)
                {
                    if (q.OVC_IKIND.Contains(ovcIkind) == true && q.OVC_MEMO != null)
                    {
                        count++;
                        if (count == 1)
                        {
                            if (q.OVC_STANDARD == "N")
                            {
                                rowPattern.ReplaceText("[$MEMO_W3_FN$]", "(" + count + ")" + q.OVC_MEMO, false, System.Text.RegularExpressions.RegexOptions.None);
                                rowPattern.ReplaceText("[$MEMO_W3_FY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            else
                            {
                                rowPattern.ReplaceText("[$MEMO_W3_FY$]", "(" + count + ")" + q.OVC_MEMO, false, System.Text.RegularExpressions.RegexOptions.None);
                                rowPattern.ReplaceText("[$MEMO_W3_FN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                        }
                        else
                        {
                            if (q.OVC_STANDARD == "N")
                            {
                                var newItem = groceryListTable.InsertRow(rowPattern_N, groceryListTable.RowCount - 2);
                                newItem.ReplaceText("[$MEMO_W3_N$]", "(" + count + ")" + q.OVC_MEMO.ToString());
                            }
                            else
                            {
                                var newItem = groceryListTable.InsertRow(rowPattern_Y, groceryListTable.RowCount - 2);
                                newItem.ReplaceText("[$MEMO_W3_Y$]", "(" + count + ")" + q.OVC_MEMO.ToString());
                            }
                        }
                    }
                }
                rowPattern.ReplaceText("[$MEMO_W3_FN$][$MEMO_W3_FY$]", "    ", false, System.Text.RegularExpressions.RegexOptions.None);
                var query1220_2 =
                    from tbm1220_1 in mpms.TBM1220_1
                    where tbm1220_1.OVC_PURCH.Equals(purch)
                    where tbm1220_1.OVC_IKIND.Equals("M11")
                    select new
                    {
                        OVC_IKIND = tbm1220_1.OVC_IKIND,
                        OVC_MEMO = tbm1220_1.OVC_MEMO,
                        OVC_STANDARD = tbm1220_1.OVC_STANDARD
                    };
                foreach (var q in query1220_2)
                {
                    if (q.OVC_MEMO != null)
                    {
                        if (q.OVC_STANDARD == "N")
                        {
                            doc.ReplaceText("[$MEMO_M11_N$]", q.OVC_MEMO, false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MEMO_M11_Y$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                        {
                            doc.ReplaceText("[$MEMO_M11_Y$]", q.OVC_MEMO, false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MEMO_M11_N$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                }
                doc.ReplaceText("[$MEMO_M11_N$][$MEMO_M11_Y$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                var query1220_3 =
                    from tbm1220_1 in mpms.TBM1220_1
                    where tbm1220_1.OVC_PURCH.Equals(purch)
                    where tbm1220_1.OVC_IKIND.Equals("M21")
                    select new
                    {
                        OVC_IKIND = tbm1220_1.OVC_IKIND,
                        OVC_MEMO = tbm1220_1.OVC_MEMO,
                        OVC_STANDARD = tbm1220_1.OVC_STANDARD
                    };
                foreach (var q in query1220_3)
                {
                    if (q.OVC_STANDARD == "N")
                    {
                        doc.ReplaceText("[$MEMO_M21_N$]", q.OVC_MEMO, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$MEMO_M21_Y$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    else
                    {
                        doc.ReplaceText("[$MEMO_M21_Y$]", q.OVC_MEMO, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$MEMO_M21_N$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                }
                doc.ReplaceText("[$MEMO_M21_N$][$MEMO_M21_Y$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
            }
        }
        private void table2_AddRow(DocX doc, string Page, int rowCount, string ovcIkind)
        {
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            var groceryListTable = doc.Tables.FirstOrDefault(table => table.TableCaption == Page);
            if (groceryListTable != null)
            {
                var rowPattern = groceryListTable.Rows[0];
                var rowPattern_N = groceryListTable.Rows[1];
                var rowPattern_Y = groceryListTable.Rows[2];
                rowPattern_N.Remove();
                rowPattern_Y.Remove();
                var query1220 =
                    from tbm1220_1 in mpms.TBM1220_1
                    where tbm1220_1.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_IKIND = tbm1220_1.OVC_IKIND,
                        OVC_MEMO = tbm1220_1.OVC_MEMO,
                        OVC_STANDARD = tbm1220_1.OVC_STANDARD
                    };
                int count = 0;
                foreach (var q in query1220)
                {
                    if (q.OVC_IKIND.Contains(ovcIkind) == true && q.OVC_MEMO != null)
                    {
                        count++;
                        if (count == 1)
                        {
                            if (q.OVC_STANDARD == "N")
                            {
                                rowPattern.ReplaceText("[$MEMO_W4_FN$]", "(" + count + ")" + q.OVC_MEMO, false, System.Text.RegularExpressions.RegexOptions.None);
                                rowPattern.ReplaceText("[$MEMO_W4_FY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            else
                            {
                                rowPattern.ReplaceText("[$MEMO_W4_FY$]", "(" + count + ")" + q.OVC_MEMO, false, System.Text.RegularExpressions.RegexOptions.None);
                                rowPattern.ReplaceText("[$MEMO_W4_FN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                        }
                        else
                        {
                            if (q.OVC_STANDARD == "N")
                            {
                                var newItem = groceryListTable.InsertRow(rowPattern_N, groceryListTable.RowCount - rowCount);
                                newItem.ReplaceText("[$MEMO_W4_N$]", "(" + count + ")" + q.OVC_MEMO.ToString());
                            }
                            else
                            {
                                var newItem = groceryListTable.InsertRow(rowPattern_Y, groceryListTable.RowCount - rowCount);
                                newItem.ReplaceText("[$MEMO_W4_Y$]", "(" + count + ")" + q.OVC_MEMO.ToString());
                            }
                        }
                    }
                }
                rowPattern.ReplaceText("[$MEMO_W4_FN$][$MEMO_W4_FY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
            }
        }
        #endregion
        #endregion

        #region 計畫清單
        public void ListOfPlans_ExportToWord()
        {
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string ovcIkind = "";
            int year = 0;
            string date = "";
            string wordfilepath = "";
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/ListOfPlans.docx");
            string purch_6 = Session["purch_6"].ToString();
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/ListOfPlans.pdf";
                    var query1301 =
                        from tbm1301 in mpms.TBM1301
                        where tbm1301.OVC_PURCH.Equals(purch)
                        select new
                        {
                            OVC_PURCH = tbm1301.OVC_PURCH,
                            OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                            OVC_PURCH_KIND = tbm1301.OVC_PURCH_KIND,
                            OVC_PUR_DAPPROVE = tbm1301.OVC_PUR_DAPPROVE,
                            OVC_PUR_APPROVE = tbm1301.OVC_PUR_APPROVE,
                            OVC_PUR_NPURCH = tbm1301.OVC_PUR_NPURCH,
                            OVC_TARGET_DO = tbm1301.OVC_TARGET_DO,
                            OVC_COUNTRY = tbm1301.OVC_COUNTRY,
                            OVC_RECEIVE_NSECTION = tbm1301.OVC_RECEIVE_NSECTION,
                            OVC_SHIP_TIMES = tbm1301.OVC_SHIP_TIMES,
                            OVC_WAY_TRANS = tbm1301.OVC_WAY_TRANS,
                            OVC_FROM_TO = tbm1301.OVC_FROM_TO,
                            OVC_TO_PLACE = tbm1301.OVC_TO_PLACE,
                            OVC_POI_IINSPECT = tbm1301.OVC_POI_IINSPECT,
                            OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                            OVC_DPROPOSE = tbm1301.OVC_DPROPOSE,
                            OVC_PROPOSE = tbm1301.OVC_PROPOSE
                        };
                    foreach (var q in query1301)
                    {
                        string yearsub = q.OVC_PURCH.Substring(2, 2);
                        string y = "";
                        if (Int32.Parse(yearsub) >= 70)
                            y = yearsub;
                        else
                            y = "1" + yearsub;
                        doc.ReplaceText("[$YEAR$]", y, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_NSECTION$]", q.OVC_PUR_NSECTION != null ? q.OVC_PUR_NSECTION : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PURCH_KIND$]", q.OVC_PURCH_KIND == "1" ? "內" : "外", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DPROPOSE != null)
                        {
                            year = int.Parse(q.OVC_DPROPOSE.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DPROPOSE.Substring(5, 2) + "月" + q.OVC_DPROPOSE.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DPROPOSE$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DPROPOSE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PROPOSE$]", q.OVC_PROPOSE != null ? q.OVC_PROPOSE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_PUR_DAPPROVE != null)
                        {
                            year = int.Parse(q.OVC_PUR_DAPPROVE.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_PUR_DAPPROVE.Substring(5, 2) + "月" + q.OVC_PUR_DAPPROVE.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_PUR_DAPPROVE$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_PUR_DAPPROVE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_APPROVE$]", q.OVC_PUR_APPROVE != null ? q.OVC_PUR_APPROVE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        var query1407 =
                            from tbm1407 in mpms.TBM1407
                            where tbm1407.OVC_PHR_CATE.Equals("Q8")
                            where tbm1407.OVC_PHR_ID.Equals(q.OVC_PUR_NPURCH)
                            select tbm1407.OVC_PHR_DESC;
                        foreach (var qu in query1407)
                        {
                            if (qu != null)
                                doc.ReplaceText("[$OVC_PUR_NPURCH$]", qu, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_PUR_NPURCH$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_TARGET_DO$]", q.OVC_TARGET_DO != null ? q.OVC_TARGET_DO : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        var query1407_2 =
                            from tbm1407 in mpms.TBM1407
                            where tbm1407.OVC_PHR_CATE.Equals("C8")
                            where tbm1407.OVC_PHR_ID.Equals(q.OVC_COUNTRY)
                            select tbm1407.OVC_PHR_DESC;
                        foreach (var qu in query1407)
                        {
                            if (qu != null)
                                doc.ReplaceText("[$OVC_COUNTRY$]", qu, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_COUNTRY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_RECEIVE_NSECTION$]", q.OVC_RECEIVE_NSECTION != null ? q.OVC_RECEIVE_NSECTION : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_SHIP_TIMES$]", q.OVC_SHIP_TIMES != null ? q.OVC_SHIP_TIMES : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_WAY_TRANS$]", q.OVC_WAY_TRANS != null ? q.OVC_WAY_TRANS : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_FROM_TO$]", q.OVC_FROM_TO != null ? q.OVC_FROM_TO : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_TO_PLACE$]", q.OVC_TO_PLACE != null ? q.OVC_TO_PLACE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_POI_IINSPECT$]", q.OVC_POI_IINSPECT != null ? q.OVC_POI_IINSPECT : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        switch (q.OVC_PUR_AGENCY)
                        {
                            case "B":
                            case "L":
                            case "P":
                                ovcIkind = "D5";
                                break;
                            case "M":
                            case "S":
                                ovcIkind = "F5";
                                break;
                            default:
                                ovcIkind = "W5";
                                break;
                        }
                    }
                    var query1118 =
                    from tbm1118 in mpms.TBM1118
                    where tbm1118.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_ISOURCE = tbm1118.OVC_ISOURCE,
                        OVC_POI_IBDG = tbm1118.OVC_POI_IBDG,
                        OVC_PJNAME = tbm1118.OVC_PJNAME
                    };
                    foreach (var q in query1118)
                    {
                        if (q.OVC_ISOURCE != null)
                            doc.ReplaceText("[$OVC_ISOURCE$]", q.OVC_ISOURCE, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_POI_IBDG != null)
                            doc.ReplaceText("[$OVC_POI_IBDG$]", q.OVC_POI_IBDG, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_PJNAME != null)
                            doc.ReplaceText("[$OVC_PJNAME$]", q.OVC_PJNAME, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_ISOURCE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_POI_IBDG$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_PJNAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryPurch =
                            from tbm1301 in mpms.TBM1301_PLAN
                            where tbm1301.OVC_PURCH.Equals(purch)
                            select new
                            {
                                OVC_PURCH = tbm1301.OVC_PURCH,
                                OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY
                            };
                    foreach (var qu in queryPurch)
                    {
                        var query1302 =
                            (from tbm1302 in mpms.TBM1302
                             where tbm1302.OVC_PURCH.Equals(purch)
                             where tbm1302.OVC_PURCH_6.Equals(purch_6)
                             select tbm1302.OVC_PURCH_5).FirstOrDefault();
                        doc.ReplaceText("[$PURCH$]", qu.OVC_PURCH + qu.OVC_PUR_AGENCY + query1302, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$PURCH_2$]", qu.OVC_PURCH + qu.OVC_PUR_AGENCY, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$Barcode$]", qu.OVC_PURCH + qu.OVC_PUR_AGENCY + query1302, false, System.Text.RegularExpressions.RegexOptions.None);//條碼字型IDAutomationHC39M
                    }
                    var groceryListTable_1 = doc.Tables.FirstOrDefault(table => table.TableCaption == "Table_1");
                    if (groceryListTable_1 != null)
                    {
                        var rowPattern = groceryListTable_1.Rows[1];
                        rowPattern.Remove();
                        var query1201 =
                            from tbm1201 in mpms.TBM1201
                            where tbm1201.OVC_PURCH.Equals(purch)
                            select new
                            {
                                ONB_POI_ICOUNT = tbm1201.ONB_POI_ICOUNT,
                                OVC_POI_NSTUFF_CHN = tbm1201.OVC_POI_NSTUFF_CHN,
                                OVC_POI_NSTUFF_ENG = tbm1201.OVC_POI_NSTUFF_ENG,
                                NSN = tbm1201.NSN,
                                OVC_BRAND = tbm1201.OVC_BRAND,
                                OVC_MODEL = tbm1201.OVC_MODEL,
                                OVC_POI_IREF = tbm1201.OVC_POI_IREF,
                                OVC_FCODE = tbm1201.OVC_FCODE,
                                OVC_POI_IUNIT = tbm1201.OVC_POI_IUNIT,
                                ONB_POI_QORDER_PLAN = tbm1201.ONB_POI_QORDER_PLAN,
                                ONB_POI_MPRICE_PLAN = tbm1201.ONB_POI_MPRICE_PLAN,
                                OVC_POI_NDESC = tbm1201.OVC_POI_NDESC,
                            };
                        foreach (var q in query1201)
                        {
                            var newItem = groceryListTable_1.InsertRow(rowPattern, groceryListTable_1.RowCount);
                            newItem.ReplaceText("[$ONB_POI_ICOUNT$]", q.ONB_POI_ICOUNT != null ? q.ONB_POI_ICOUNT.ToString() : "");
                            newItem.ReplaceText("[$OVC_POI_NSTUFF_CHN$]", q.OVC_POI_NSTUFF_CHN != null ? q.OVC_POI_NSTUFF_CHN : "");
                            newItem.ReplaceText("[$OVC_POI_NSTUFF_ENG$]", q.OVC_POI_NSTUFF_ENG != null ? q.OVC_POI_NSTUFF_ENG : "");
                            newItem.ReplaceText("[$NSN$]", q.NSN != null ? q.NSN : "(空白)");
                            newItem.ReplaceText("[$OVC_BRAND$]", q.OVC_BRAND != null ? q.OVC_BRAND : "(空白)");
                            newItem.ReplaceText("[$OVC_MODEL$]", q.OVC_MODEL != null ? q.OVC_MODEL : "(空白)");
                            newItem.ReplaceText("[$OVC_POI_IREF$]", q.OVC_POI_IREF != null ? q.OVC_POI_IREF : "(空白)");
                            newItem.ReplaceText("[$OVC_FCODE$]", q.OVC_FCODE != null ? q.OVC_FCODE : "(空白)");
                            var query1407 =
                                from tbm1407 in mpms.TBM1407
                                where tbm1407.OVC_PHR_CATE.Equals("J1")
                                where tbm1407.OVC_PHR_ID.Equals(q.OVC_POI_IUNIT)
                                select new
                                {
                                    OVC_PHR_DESC = tbm1407.OVC_PHR_DESC,
                                    OVC_PHR_REMARK = tbm1407.OVC_PHR_REMARK
                                };
                            foreach (var qu in query1407)
                            {
                                newItem.ReplaceText("[$OVC_PHR_REMARK$]", qu.OVC_PHR_REMARK != null ? qu.OVC_PHR_REMARK : "");
                                newItem.ReplaceText("[$OVC_POI_IUNIT$]", qu.OVC_PHR_DESC != null ? qu.OVC_PHR_DESC : "");
                            }
                            newItem.ReplaceText("[$ONB_POI_QORDER_PLAN$]", q.ONB_POI_QORDER_PLAN != null ? String.Format("{0:N}", q.ONB_POI_QORDER_PLAN) : "");
                            newItem.ReplaceText("[$ONB_POI_MPRICE_PLAN$]", q.ONB_POI_MPRICE_PLAN != null ? String.Format("{0:N}", q.ONB_POI_MPRICE_PLAN) : "");
                            newItem.ReplaceText("[$TOTAL$]", (q.ONB_POI_QORDER_PLAN != null && q.ONB_POI_MPRICE_PLAN != null) ? String.Format("{0:N}", String.Format("{0:N}", q.ONB_POI_MPRICE_PLAN * q.ONB_POI_QORDER_PLAN)) : "");
                            newItem.ReplaceText("[$OVC_POI_NDESC$]", q.OVC_POI_NDESC != null ? q.OVC_POI_NDESC : "");
                            var query1233 =
                                from tbm1233 in mpms.TBM1233
                                where tbm1233.OVC_PURCH.Equals(purch)
                                where tbm1233.ONB_POI_ICOUNT.Equals(q.ONB_POI_ICOUNT)
                                orderby tbm1233.ONB_POI_ICOUNT
                                select tbm1233.OVC_POI_NDESC;
                            foreach (var qu in query1233)
                            {
                                newItem.ReplaceText("[$POI_NDESC$]", qu);
                            }
                        }

                    }
                    var groceryListTable = doc.Tables.FirstOrDefault(table => table.TableCaption == "Remarks");
                    if (groceryListTable != null)
                    {
                        var rowPattern = groceryListTable.Rows[1];
                        var rowPattern_N = groceryListTable.Rows[2];
                        var rowPattern_Y = groceryListTable.Rows[3];
                        rowPattern_N.Remove();
                        rowPattern_Y.Remove();
                        var query1220 =
                            from tbm1220_1 in mpms.TBM1220_1
                            join tbm1220_2 in mpms.TBM1220_2
                                on tbm1220_1.OVC_IKIND equals tbm1220_2.OVC_IKIND
                            where tbm1220_1.OVC_PURCH.Equals(purch)
                            where tbm1220_1.OVC_IKIND.Contains(ovcIkind)
                            select new
                            {
                                OVC_IKIND = tbm1220_1.OVC_IKIND,
                                OVC_MEMO = tbm1220_1.OVC_MEMO,
                                OVC_STANDARD = tbm1220_1.OVC_STANDARD,
                                OVC_MEMO_NAME = tbm1220_2.OVC_MEMO_NAME
                            };
                        int count = 0;
                        foreach (var q in query1220)
                        {
                            if (q.OVC_MEMO != null)
                            {
                                count++;
                                if (count == 1)
                                {
                                    rowPattern.ReplaceText("[$MEMO_N$]", q.OVC_STANDARD == "N" ? q.OVC_MEMO_NAME + "：\r" + q.OVC_MEMO : "", false, System.Text.RegularExpressions.RegexOptions.None);
                                    rowPattern.ReplaceText("[$MEMO_Y$]", q.OVC_STANDARD == "N" ? "" : q.OVC_MEMO_NAME + "：\r" + q.OVC_MEMO, false, System.Text.RegularExpressions.RegexOptions.None);
                                    doc.ReplaceText("[$MEMO_Y$][$MEMO_N$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                }
                                else
                                {
                                    var newItem = q.OVC_STANDARD == "N" ? groceryListTable.InsertRow(rowPattern_N, groceryListTable.RowCount) : groceryListTable.InsertRow(rowPattern_Y, groceryListTable.RowCount);
                                    newItem.ReplaceText("[$MEMO_N$]", q.OVC_STANDARD == "N" ? q.OVC_MEMO_NAME + "：\r" + q.OVC_MEMO : "");
                                    newItem.ReplaceText("[$MEMO_Y$]", q.OVC_STANDARD == "N" ? "" : q.OVC_MEMO_NAME + "：\r" + q.OVC_MEMO);
                                }
                            }
                        }
                    }
                    year = int.Parse(DateTime.Now.ToString("yyyyMMdd").Substring(0, 4)) - 1911;
                    date = year.ToString() + "." + DateTime.Now.Month + "." + DateTime.Now.Day;
                    doc.ReplaceText("[$DATE$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$TIME$]", DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString(), false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/LOP_Temp.docx");
                }
                buffer = ms.ToArray();
            }
        }
        #endregion

        #region 時程管制表
        public void PrinterServlet_ExportToWord()
        {
            string date = "";
            string Acceptance = "";
            string path = "";
            string schedule = "";
            int year = 0;
            var userunit = Session["userunit"].ToString();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            var rowven = Session["rowven"].ToString();
            string purch_6 = Session["purch_6"].ToString();
            var querykind =
                from cash in mpms.TBMMANAGE_CASH
                where cash.OVC_PURCH.Equals(purch)
                where cash.OVC_PURCH_6.Equals(purch_6)
                select cash.OVC_KIND;
            foreach (var q in querykind)
            {
                if (q == "1")
                    path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/PrinterServlet.docx");
                else if (q == "2")
                    path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/PrinterServlet_2.docx");
                else
                    path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/PrinterServlet_3.docx");
            }
            if (querykind.Count() < 1)
                path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/PrinterServlet_3.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    #region 基本資料
                    var query =
                        from tbm1302 in mpms.TBM1302
                        join tbm1301 in mpms.TBM1301_PLAN on tbm1302.OVC_PURCH equals tbm1301.OVC_PURCH
                        join tbmreceive in mpms.TBMRECEIVE_CONTRACT on new { tbm1302.OVC_PURCH, tbm1302.OVC_PURCH_6 } equals new { tbmreceive.OVC_PURCH, tbmreceive.OVC_PURCH_6 } into re
                        from tbmreceive in re.DefaultIfEmpty()
                        where userunit == "0A100" || userunit == "00N00" ? tbm1301.OVC_CONTRACT_UNIT.Equals("0A100") || tbm1301.OVC_CONTRACT_UNIT.Equals("00N00") : tbm1301.OVC_CONTRACT_UNIT.Equals(userunit)
                        where tbm1301.OVC_PURCH.Equals(purch)
                        where tbm1302.OVC_VEN_TITLE.Equals(rowven)
                        where tbm1302.OVC_PURCH_6.Equals(purch_6)
                        //where tbm1302.OVC_PURCH_6.Equals(tbmreceive.OVC_PURCH_6)
                        select new
                        {
                            OVC_PURCH = tbm1301.OVC_PURCH,
                            OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                            OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                            OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                            OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                            ONB_MONEY = tbm1302.ONB_MONEY,
                            OVC_DCONTRACT = tbm1302.OVC_DCONTRACT,
                            OVC_DRECEIVE = tbmreceive.OVC_DRECEIVE ?? "",
                            ONB_SHIP_TIMES = tbmreceive.ONB_SHIP_TIMES ?? 0,
                            OVC_RECEIVE_PLACE = tbm1302.OVC_RECEIVE_PLACE,
                            OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                            OVC_PUR_IUSER_PHONE_EXT = tbm1301.OVC_PUR_IUSER_PHONE_EXT,
                            OVC_PUR_USER = tbm1301.OVC_PUR_USER,
                            OVC_PUR_IUSER_PHONE = tbm1301.OVC_PUR_IUSER_PHONE,
                            OVC_USER_CELLPHONE = tbm1301.OVC_USER_CELLPHONE,
                            OVC_PUR_IUSER_FAX = tbm1301.OVC_PUR_IUSER_FAX,
                            OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE,
                            OVC_VEN_TEL = tbm1302.OVC_VEN_TEL,
                            OVC_VEN_CELLPHONE = tbm1302.OVC_VEN_CELLPHONE,
                            OVC_VEN_FAX = tbm1302.OVC_VEN_FAX,
                            OVC_VEN_ADDRESS = tbm1302.OVC_VEN_ADDRESS,
                            OVC_INSPECT = tbmreceive.OVC_INSPECT ?? "",
                            OVC_PUR_GOOD_OK = tbmreceive.OVC_PUR_GOOD_OK ?? "",
                            OVC_PUR_TAX_OK = tbmreceive.OVC_PUR_TAX_OK ?? "",
                            OVC_PUR_FEE_OK = tbmreceive.OVC_PUR_FEE_OK ?? "",

                        };
                    //if (query.Count() < 1)
                    //    return;
                    foreach (var q in query)
                    {
                        string Purchase = q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6;
                        doc.ReplaceText("[$OVC_PURCH$]", Purchase, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_IPURCH$]", q.OVC_PUR_IPURCH != null ? q.OVC_PUR_IPURCH : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_MONEY != null)
                        {
                            string money = EastAsiaNumericFormatter.FormatWithCulture("Lc", q.ONB_MONEY, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                            doc.ReplaceText("[$ONB_MONEY$]", money, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$ONB_MONEY$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DCONTRACT != null)
                        {
                            year = int.Parse(q.OVC_DCONTRACT.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DCONTRACT.Substring(5, 2) + "月" + q.OVC_DCONTRACT.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DCONTRACT$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DCONTRACT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DRECEIVE != null)
                        {
                            year = int.Parse(q.OVC_DRECEIVE.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DRECEIVE.Substring(5, 2) + "月" + q.OVC_DRECEIVE.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DRECEIVE$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DRECEIVE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_SHIP_TIMES != null)
                        {
                            string shiptime = EastAsiaNumericFormatter.FormatWithCulture("Ln", q.ONB_SHIP_TIMES, null, new CultureInfo("zh-TW")); ;
                            doc.ReplaceText("[$ONB_SHIP_TIMES$]", shiptime, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$ONB_SHIP_TIMES$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_RECEIVE_PLACE$]", q.OVC_RECEIVE_PLACE != null ? q.OVC_RECEIVE_PLACE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_NSECTION$]", q.OVC_PUR_NSECTION != null ? q.OVC_PUR_NSECTION : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_IUSER_PHONE_EXT$]", q.OVC_PUR_IUSER_PHONE_EXT != null ? q.OVC_PUR_IUSER_PHONE_EXT : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_USER$]", q.OVC_PUR_USER != null ? q.OVC_PUR_USER : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_IUSER_PHONE$]", q.OVC_PUR_IUSER_PHONE != null ? q.OVC_PUR_IUSER_PHONE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_USER_CELLPHONE$]", q.OVC_USER_CELLPHONE != null ? q.OVC_USER_CELLPHONE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_IUSER_FAX$]", q.OVC_PUR_IUSER_FAX != null ? q.OVC_PUR_IUSER_FAX : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_VEN_TITLE$]", q.OVC_VEN_TITLE != null ? q.OVC_VEN_TITLE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_VEN_TEL$]", q.OVC_VEN_TEL != null ? q.OVC_VEN_TEL : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_VEN_CELLPHONE$]", q.OVC_VEN_CELLPHONE != null ? q.OVC_VEN_CELLPHONE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_VEN_FAX$]", q.OVC_VEN_FAX != null ? q.OVC_VEN_FAX : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_VEN_ADDRESS$]", q.OVC_VEN_ADDRESS != null ? q.OVC_VEN_ADDRESS : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_INSPECT$]", q.OVC_INSPECT == "是" ? "要" : "不要", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$TAX$]", (q.OVC_PUR_GOOD_OK == "Y" && q.OVC_PUR_TAX_OK == "Y" && q.OVC_PUR_FEE_OK == "Y") ? "不含稅" : "含稅", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    var queryCurrency =
                        from tbm1302 in mpms.TBM1302
                        join tbm1407 in mpms.TBM1407
                            on tbm1302.OVC_CURRENT equals tbm1407.OVC_PHR_ID
                        where tbm1302.OVC_PURCH.Equals(purch)
                        where tbm1302.OVC_PURCH_6.Equals(purch_6)
                        where tbm1407.OVC_PHR_CATE.Equals("B0")
                        where tbm1302.OVC_VEN_TITLE.Equals(rowven)
                        select tbm1407.OVC_PHR_DESC;
                    foreach (var q in queryCurrency)
                    {
                        if (query.Count() > 0)
                            doc.ReplaceText("[$OVC_CURRENT$]", q, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_CURRENT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryAcceptance =
                        from tbmreceiveInspect in mpms.TBMRECEIVE_INSPECT
                        join tbm1407 in mpms.TBM1407
                            on tbmreceiveInspect.OVC_INSPECT equals tbm1407.OVC_PHR_ID
                        where tbm1407.OVC_PHR_CATE.Equals("D2")
                        where !tbm1407.OVC_PHR_ID.Equals("A")
                        where tbmreceiveInspect.OVC_PURCH.Equals(purch)
                        where tbmreceiveInspect.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            OVC_PHR_DESC = tbm1407.OVC_PHR_DESC,
                            OVC_INSPECT_REMARK = tbmreceiveInspect.OVC_INSPECT_REMARK
                        };
                    foreach (var q in queryAcceptance)
                    {
                        if (Acceptance != "")
                            Acceptance = Acceptance + "、";
                        Acceptance = Acceptance + q.OVC_PHR_DESC;
                    }
                    doc.ReplaceText("[$Acceptance$]", queryAcceptance.Count() > 0 ? Acceptance : "", false, System.Text.RegularExpressions.RegexOptions.None);

                    #region 履約金/保固金
                    var querycash =
                        from cash in mpms.TBMMANAGE_CASH
                        where cash.OVC_PURCH.Equals(purch)
                        where cash.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            ONB_ALL_MONEY = cash.ONB_ALL_MONEY,
                            OVC_KIND = cash.OVC_KIND,
                            OVC_DGARRENT_END = cash.OVC_DGARRENT_END
                        };
                    foreach (var q in querycash)
                    {
                        if (q.ONB_ALL_MONEY != null)
                            doc.ReplaceText("[$CASH_ALL_MONEY$]", String.Format("{0:N}", q.ONB_ALL_MONEY), false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DGARRENT_END != null)
                        {
                            year = int.Parse(q.OVC_DGARRENT_END.ToString().Substring(0, 4)) - 1911;
                            date = "自" + year.ToString() + "年" + q.OVC_DGARRENT_END.Substring(5, 2) + "月" + q.OVC_DGARRENT_END.Substring(8, 2) + "日至合約履行完成止。";
                            doc.ReplaceText("[$CASH_DGARRENT_END$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                    doc.ReplaceText("[$CASH_ALL_MONEY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$CASH_DGARRENT_END$]", "至XX年XX月XX日至合約履行完成止。", false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryprom =
                        from prom in mpms.TBMMANAGE_PROM
                        where prom.OVC_PURCH.Equals(purch)
                        where prom.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            ONB_ALL_MONEY = prom.ONB_ALL_MONEY,
                            OVC_KIND = prom.OVC_KIND,
                            OVC_DEFFECT_1 = prom.OVC_DEFFECT_1,
                            OVC_DEFFECT_2 = prom.OVC_DEFFECT_2,
                            OVC_DEFFECT_3 = prom.OVC_DEFFECT_3,
                            OVC_DGARRENT_END = prom.OVC_DGARRENT_END
                        };
                    foreach (var q in queryprom)
                    {
                        if (q.ONB_ALL_MONEY != null)
                            doc.ReplaceText("[$PROM_ALL_MONEY$]", String.Format("{0:N}", q.ONB_ALL_MONEY), false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DGARRENT_END != null)
                        {
                            year = int.Parse(q.OVC_DGARRENT_END.ToString().Substring(0, 4)) - 1911;
                            date = "自" + year.ToString() + "年" + q.OVC_DGARRENT_END.Substring(5, 2) + "月" + q.OVC_DGARRENT_END.Substring(8, 2) + "日至合約履行完成止。";
                            doc.ReplaceText("[$PROM_DGARRENT_END$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                    doc.ReplaceText("[$PROM_ALL_MONEY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$PROM_DGARRENT_END$]", "至XX年XX月XX日至合約履行完成止。", false, System.Text.RegularExpressions.RegexOptions.None);
                    var querystock =
                        from stock in mpms.TBMMANAGE_STOCK
                        where stock.OVC_PURCH.Equals(purch)
                        where stock.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            ONB_ALL_MONEY = stock.ONB_ALL_MONEY,
                            OVC_KIND = stock.OVC_KIND,
                            OVC_DGARRENT_END = stock.OVC_DGARRENT_END
                        };
                    foreach (var q in querystock)
                    {
                        if (q.ONB_ALL_MONEY != null)
                            doc.ReplaceText("[$STOCK_ALL_MONEY$]", String.Format("{0:N}", q.ONB_ALL_MONEY), false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DGARRENT_END != null)
                        {
                            year = int.Parse(q.OVC_DGARRENT_END.ToString().Substring(0, 4)) - 1911;
                            date = "自" + year.ToString() + "年" + q.OVC_DGARRENT_END.Substring(5, 2) + "月" + q.OVC_DGARRENT_END.Substring(8, 2) + "日至合約履行完成止。";
                            doc.ReplaceText("[$STOCK_DGARRENT_END$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                    doc.ReplaceText("[$STOCK_ALL_MONEY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$STOCK_DGARRENT_END$]", "至XX年XX月XX日至合約履行完成止。", false, System.Text.RegularExpressions.RegexOptions.None);
                    #endregion

                    var queryGive =
                        from tbm1407 in mpms.TBM1407
                        join tbmreceivc in mpms.TBMRECEIVE_CONTRACT
                            on tbm1407.OVC_PHR_ID equals tbmreceivc.OVC_GRANT_TO
                        where tbmreceivc.OVC_PURCH.Equals(purch)
                        where tbmreceivc.OVC_PURCH_6.Equals(purch_6)
                        where tbm1407.OVC_PHR_CATE.Equals("P9")
                        select new
                        {
                            OVC_PHR_DESC = tbm1407.OVC_PHR_DESC
                        };
                    foreach (var q in queryGive)
                    {
                        if (queryGive.Count() > 0)
                            doc.ReplaceText("[$OVC_GRANT_TO$]", q.OVC_PHR_DESC, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_GRANT_TO$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    #endregion
                    #region 時程管制
                    var queryPrinter =
                        from tbmdelivery in mpms.TBMDELIVERY
                        where tbmdelivery.OVC_PURCH.Equals(purch)
                        where tbmdelivery.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            ONB_SHIP_TIMES = tbmdelivery.ONB_SHIP_TIMES,
                            OVC_DELIVERY_CONTRACT = tbmdelivery.OVC_DELIVERY_CONTRACT,
                            OVC_DELIVERY = tbmdelivery.OVC_DELIVERY,
                            OVC_DAIRSHIP_PLAN = tbmdelivery.OVC_DAIRSHIP_PLAN,
                            OVC_DAIRSHIP = tbmdelivery.OVC_DAIRSHIP,
                            OVC_DINFORM_PLAN = tbmdelivery.OVC_DINFORM_PLAN,
                            OVC_DINFORM = tbmdelivery.OVC_DINFORM,
                            OVC_DJOINCHECK_PLAN = tbmdelivery.OVC_DJOINCHECK_PLAN,
                            OVC_DJOINCHECK = tbmdelivery.OVC_DJOINCHECK,
                            OVC_DSHIPMENT_PLAN = tbmdelivery.OVC_DSHIPMENT_PLAN,
                            OVC_DSHIPMENT = tbmdelivery.OVC_DSHIPMENT,
                            OVC_DINVENTORY_PLAN = tbmdelivery.OVC_DINVENTORY_PLAN,
                            OVC_DINVENTORY = tbmdelivery.OVC_DINVENTORY,
                            OVC_DINSPECT_PLAN = tbmdelivery.OVC_DINSPECT_PLAN,
                            OVC_DINSPECT = tbmdelivery.OVC_DINSPECT,
                            OVC_DPAY_PLAN = tbmdelivery.OVC_DPAY_PLAN,
                            OVC_DPAY = tbmdelivery.OVC_DPAY
                        };

                    #region 無接管
                    if (queryPrinter.Count() < 1)
                    {
                        doc.ReplaceText("[$SHIP_TIMES$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DELIVERY_CONTRACT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DELIVERY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DELIVERY_schedule$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DAIRSHIP_PLAN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DAIRSHIP$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DAIRSHIP_schedule$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DINFORM_PLAN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DINFORM$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DINFORM_schedule$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DJOINCHECK_PLAN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DJOINCHECK$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DJOINCHECK_schedule$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DSHIPMENT_PLAN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DSHIPMENT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DSHIPMENT_schedule$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DINVENTORY_PLAN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DINVENTORY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DINVENTORY_schedule$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DINSPECT_PLAN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DINSPECT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DINSPECT_schedule$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DPAY_PLAN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DPAY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DPAY_schedule$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_DCLOSE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    #endregion

                    foreach (var q in queryPrinter)
                    {
                        #region 預劃/實際
                        if (q.ONB_SHIP_TIMES != null)
                        {
                            string shiptime = EastAsiaNumericFormatter.FormatWithCulture("Ln", q.ONB_SHIP_TIMES, null, new CultureInfo("zh-TW")); ;
                            doc.ReplaceText("[$SHIP_TIMES$]", shiptime, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$SHIP_TIMES$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DELIVERY_CONTRACT != null)
                        {
                            year = int.Parse(q.OVC_DELIVERY_CONTRACT.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DELIVERY_CONTRACT.Substring(5, 2) + "月" + q.OVC_DELIVERY_CONTRACT.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DELIVERY_CONTRACT$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DELIVERY_CONTRACT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DELIVERY != null)
                        {
                            year = int.Parse(q.OVC_DELIVERY.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DELIVERY.Substring(5, 2) + "月" + q.OVC_DELIVERY.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DELIVERY$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DELIVERY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DAIRSHIP_PLAN != null)
                        {
                            year = int.Parse(q.OVC_DAIRSHIP_PLAN.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DAIRSHIP_PLAN.Substring(5, 2) + "月" + q.OVC_DAIRSHIP_PLAN.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DAIRSHIP_PLAN$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DAIRSHIP_PLAN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DAIRSHIP != null)
                        {
                            year = int.Parse(q.OVC_DAIRSHIP.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DAIRSHIP.Substring(5, 2) + "月" + q.OVC_DAIRSHIP.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DAIRSHIP$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DAIRSHIP$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DINFORM_PLAN != null)
                        {
                            year = int.Parse(q.OVC_DINFORM_PLAN.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DINFORM_PLAN.Substring(5, 2) + "月" + q.OVC_DINFORM_PLAN.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DINFORM_PLAN$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DINFORM_PLAN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DINFORM != null)
                        {
                            year = int.Parse(q.OVC_DINFORM.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DINFORM.Substring(5, 2) + "月" + q.OVC_DINFORM.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DINFORM$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DINFORM$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DJOINCHECK_PLAN != null)
                        {
                            year = int.Parse(q.OVC_DJOINCHECK_PLAN.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DJOINCHECK_PLAN.Substring(5, 2) + "月" + q.OVC_DJOINCHECK_PLAN.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DJOINCHECK_PLAN$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DJOINCHECK_PLAN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DJOINCHECK != null)
                        {
                            year = int.Parse(q.OVC_DJOINCHECK.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DJOINCHECK.Substring(5, 2) + "月" + q.OVC_DJOINCHECK.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DJOINCHECK$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DJOINCHECK$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DSHIPMENT_PLAN != null)
                        {
                            year = int.Parse(q.OVC_DSHIPMENT_PLAN.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DSHIPMENT_PLAN.Substring(5, 2) + "月" + q.OVC_DSHIPMENT_PLAN.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DSHIPMENT_PLAN$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DSHIPMENT_PLAN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DSHIPMENT != null)
                        {
                            year = int.Parse(q.OVC_DSHIPMENT.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DSHIPMENT.Substring(5, 2) + "月" + q.OVC_DSHIPMENT.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DSHIPMENT$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DSHIPMENT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DINVENTORY_PLAN != null)
                        {
                            year = int.Parse(q.OVC_DINVENTORY_PLAN.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DINVENTORY_PLAN.Substring(5, 2) + "月" + q.OVC_DINVENTORY_PLAN.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DINVENTORY_PLAN$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DINVENTORY_PLAN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DINVENTORY != null)
                        {
                            year = int.Parse(q.OVC_DINVENTORY.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DINVENTORY.Substring(5, 2) + "月" + q.OVC_DINVENTORY.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DINVENTORY$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DINVENTORY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DINSPECT_PLAN != null)
                        {
                            year = int.Parse(q.OVC_DINSPECT_PLAN.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DINSPECT_PLAN.Substring(5, 2) + "月" + q.OVC_DINSPECT_PLAN.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DINSPECT_PLAN$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DINSPECT_PLAN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DINSPECT != null)
                        {
                            year = int.Parse(q.OVC_DINSPECT.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DINSPECT.Substring(5, 2) + "月" + q.OVC_DINSPECT.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DINSPECT$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DINSPECT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DPAY_PLAN != null)
                        {
                            year = int.Parse(q.OVC_DPAY_PLAN.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DPAY_PLAN.Substring(5, 2) + "月" + q.OVC_DPAY_PLAN.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DPAY_PLAN$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DPAY_PLAN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DPAY != null)
                        {
                            year = int.Parse(q.OVC_DPAY.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DPAY.Substring(5, 2) + "月" + q.OVC_DPAY.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DPAY$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DPAY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        #endregion
                        #region 進度
                        if (q.OVC_DELIVERY_CONTRACT != null && q.OVC_DELIVERY != null && q.OVC_DELIVERY_CONTRACT != "" && q.OVC_DELIVERY != "")
                        {
                            schedule = dtDifference_days(q.OVC_DELIVERY, q.OVC_DELIVERY_CONTRACT, schedule);
                            if (int.Parse(schedule) > 0)
                                schedule = "超前" + schedule + "天";
                            else if (int.Parse(schedule) < 0)
                                schedule = "落後" + schedule + "天";
                            else
                                schedule = "";
                            doc.ReplaceText("[$OVC_DELIVERY_schedule$]", schedule, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DELIVERY_schedule$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DAIRSHIP_PLAN != null && q.OVC_DAIRSHIP != null && q.OVC_DAIRSHIP_PLAN != "" && q.OVC_DAIRSHIP != "")
                        {
                            schedule = dtDifference_days(q.OVC_DAIRSHIP, q.OVC_DAIRSHIP_PLAN, schedule);
                            if (int.Parse(schedule) > 0)
                                schedule = "超前" + schedule + "天";
                            else if (int.Parse(schedule) < 0)
                                schedule = "落後" + schedule + "天";
                            else
                                schedule = "";
                            doc.ReplaceText("[$OVC_DAIRSHIP_schedule$]", schedule, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DAIRSHIP_schedule$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DINFORM_PLAN != null && q.OVC_DINFORM != null && q.OVC_DINFORM_PLAN != "" && q.OVC_DINFORM != "")
                        {
                            schedule = dtDifference_days(q.OVC_DINFORM, q.OVC_DINFORM_PLAN, schedule);
                            if (int.Parse(schedule) > 0)
                                schedule = "超前" + schedule + "天";
                            else if (int.Parse(schedule) < 0)
                                schedule = "落後" + schedule + "天";
                            else
                                schedule = "";
                            doc.ReplaceText("[$OVC_DINFORM_schedule$]", schedule, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DINFORM_schedule$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DJOINCHECK_PLAN != null && q.OVC_DJOINCHECK != null && q.OVC_DJOINCHECK_PLAN != "" && q.OVC_DJOINCHECK != "")
                        {
                            schedule = dtDifference_days(q.OVC_DJOINCHECK, q.OVC_DJOINCHECK_PLAN, schedule);
                            if (int.Parse(schedule) > 0)
                                schedule = "超前" + schedule + "天";
                            else if (int.Parse(schedule) < 0)
                                schedule = "落後" + schedule + "天";
                            else
                                schedule = "";
                            doc.ReplaceText("[$OVC_DJOINCHECK_schedule$]", schedule, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DJOINCHECK_schedule$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DSHIPMENT_PLAN != null && q.OVC_DSHIPMENT != null && q.OVC_DSHIPMENT_PLAN != "" && q.OVC_DSHIPMENT != "")
                        {
                            schedule = dtDifference_days(q.OVC_DSHIPMENT, q.OVC_DSHIPMENT_PLAN, schedule);
                            if (int.Parse(schedule) > 0)
                                schedule = "超前" + schedule + "天";
                            else if (int.Parse(schedule) < 0)
                                schedule = "落後" + schedule + "天";
                            else
                                schedule = "";
                            doc.ReplaceText("[$OVC_DSHIPMENT_schedule$]", schedule, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DSHIPMENT_schedule$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DINVENTORY_PLAN != null && q.OVC_DINVENTORY != null && q.OVC_DINVENTORY_PLAN != "" && q.OVC_DINVENTORY != "")
                        {
                            schedule = dtDifference_days(q.OVC_DINVENTORY, q.OVC_DINVENTORY_PLAN, schedule);
                            if (int.Parse(schedule) > 0)
                                schedule = "超前" + schedule + "天";
                            else if (int.Parse(schedule) < 0)
                                schedule = "落後" + schedule + "天";
                            else
                                schedule = "";
                            doc.ReplaceText("[$OVC_DINVENTORY_schedule$]", schedule, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DINVENTORY_schedule$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DINSPECT_PLAN != null && q.OVC_DINSPECT != null && q.OVC_DINSPECT_PLAN != "" && q.OVC_DINSPECT != "")
                        {
                            schedule = dtDifference_days(q.OVC_DINSPECT, q.OVC_DINSPECT_PLAN, schedule);
                            if (int.Parse(schedule) > 0)
                                schedule = "超前" + schedule + "天";
                            else if (int.Parse(schedule) < 0)
                                schedule = "落後" + schedule + "天";
                            else
                                schedule = "";
                            doc.ReplaceText("[$OVC_DINSPECT_schedule$]", schedule, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DINSPECT_schedule$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DPAY_PLAN != null && q.OVC_DPAY != null && q.OVC_DPAY_PLAN != "" && q.OVC_DPAY != "")
                        {
                            schedule = dtDifference_days(q.OVC_DPAY, q.OVC_DPAY_PLAN, schedule);
                            if (int.Parse(schedule) > 0)
                                schedule = "超前" + schedule + "天";
                            else if (int.Parse(schedule) < 0)
                                schedule = "落後" + schedule + "天";
                            else
                                schedule = "";
                            doc.ReplaceText(" [$OVC_DPAY_schedule$]", schedule, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText(" [$OVC_DPAY_schedule$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        #endregion
                    }
                    #region 結案日期
                    var queryDclose =
                        from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                        where tbmreceive.OVC_PURCH.Equals(purch)
                        where tbmreceive.OVC_PURCH_6.Equals(purch_6)
                        select tbmreceive.OVC_DCLOSE;
                    foreach (var q in queryDclose)
                    {
                        if (q != null)
                        {
                            year = int.Parse(q.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.Substring(5, 2) + "月" + q.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DCLOSE$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                    doc.ReplaceText("[$OVC_DCLOSE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_DCLOSE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    #endregion
                    #endregion
                    var groceryListTable = doc.Tables.FirstOrDefault(table => table.TableCaption == "Amount_Used");
                    if (groceryListTable != null)
                    {
                        var rowPattern = groceryListTable.Rows[groceryListTable.Rows.Count - 2];
                        var rowPatternF = groceryListTable.Rows[groceryListTable.Rows.Count - 1];
                        rowPatternF.Remove();
                        int INSPECT_MONEY = 0;
                        int PAY_MONEY = 0;
                        int DELAY_MONEY = 0;
                        int MINS_MONEY = 0;
                        string money = "";
                        var queryPay =
                            from pay in mpms.TBMPAY_MONEY
                            where pay.OVC_PURCH.Equals(purch)
                            where pay.OVC_PURCH_6.Equals(purch_6)
                            orderby pay.ONB_TIMES
                            select new
                            {
                                ONB_TIMES = pay.ONB_TIMES,
                                OVC_DFINISH = pay.OVC_DFINISH,
                                ONB_INSPECT_MONEY = pay.ONB_INSPECT_MONEY,
                                ONB_PAY_MONEY = pay.ONB_PAY_MONEY,
                                ONB_DELAY_MONEY = pay.ONB_DELAY_MONEY,
                                ONB_MINS_MONEY = pay.ONB_MINS_MONEY
                            };
                        foreach (var q in queryPay)
                        {
                            var newItem = groceryListTable.InsertRow(rowPattern, groceryListTable.RowCount);
                            newItem.ReplaceText("批次", q.ONB_TIMES.ToString() ?? "");
                            newItem.ReplaceText("日期", q.OVC_DFINISH ?? "");
                            if (q.ONB_INSPECT_MONEY != null)
                            {
                                money = String.Format("{0:N}", q.ONB_INSPECT_MONEY);
                                newItem.ReplaceText("驗收金額", money);
                                INSPECT_MONEY += int.Parse(q.ONB_INSPECT_MONEY.ToString());
                            }
                            newItem.ReplaceText("驗收金額", "");
                            if (q.ONB_PAY_MONEY != null)
                            {
                                money = String.Format("{0:N}", q.ONB_PAY_MONEY);
                                newItem.ReplaceText("付款金額", money);
                                PAY_MONEY += int.Parse(q.ONB_PAY_MONEY.ToString());
                            }
                            newItem.ReplaceText("付款金額", "");
                            if (q.ONB_DELAY_MONEY != null)
                            {
                                money = String.Format("{0:N}", q.ONB_DELAY_MONEY);
                                newItem.ReplaceText("逾罰金額", money);
                                DELAY_MONEY += int.Parse(q.ONB_DELAY_MONEY.ToString());
                            }
                            newItem.ReplaceText("逾罰金額", "");
                            if (q.ONB_MINS_MONEY != null)
                            {
                                money = String.Format("{0:N}", q.ONB_MINS_MONEY);
                                newItem.ReplaceText("減價金額", money);
                                MINS_MONEY += int.Parse(q.ONB_MINS_MONEY.ToString());
                            }
                            newItem.ReplaceText("減價金額", "");
                        }
                        var newItemF = groceryListTable.InsertRow(rowPatternF, groceryListTable.RowCount);
                        newItemF.ReplaceText("批次", "合計");
                        newItemF.ReplaceText("日期", "");
                        money = String.Format("{0:N}", INSPECT_MONEY);
                        newItemF.ReplaceText("驗收金額", money);
                        money = String.Format("{0:N}", PAY_MONEY);
                        newItemF.ReplaceText("付款金額", money);
                        money = String.Format("{0:N}", DELAY_MONEY);
                        newItemF.ReplaceText("逾罰金額", money);
                        money = String.Format("{0:N}", MINS_MONEY);
                        newItemF.ReplaceText("減價金額", money);
                    }
                    var groceryListTable_2 = doc.Tables.FirstOrDefault(table => table.TableCaption == "Important");
                    if (groceryListTable_2 != null)
                    {
                        var rowPattern = groceryListTable_2.Rows[1];
                        rowPattern.Remove();
                        var queryRe =
                            from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                            where tbmreceive.OVC_PURCH.Equals(purch)
                            where tbmreceive.OVC_PURCH_6.Equals(purch_6)
                            select tbmreceive.OVC_RECEIVE_COMM;
                        foreach (var q in queryRe)
                        {
                            if (q != null)
                            {
                                var newItem = groceryListTable_2.InsertRow(rowPattern, groceryListTable_2.RowCount);
                                newItem.ReplaceText("內容", q);
                            }
                        }
                    }

                    doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/PS_Temp.docx");
                }
                buffer = ms.ToArray();
            }
        }
        static string dtDifference_days(string actual, string preplanning, string result)
        {
            if (actual != "" && preplanning != "")
            {
                DateTime dtActual = DateTime.ParseExact(actual, "yyyy-MM-dd", null);
                DateTime dtPreplanning = DateTime.ParseExact(preplanning, "yyyy-MM-dd", null);
                TimeSpan ts = dtPreplanning - dtActual;
                result = ts.Days.ToString();
            }
            return result;
        }
        #endregion

        #region 檢查項目表(此頁面不需要)
        void PrinterServlet2_ExportToWord()
        {
            string path = "";
            int YesNo = 0, YesNo_2 = 0, YesNo_3 = 0;
            var userunit = Session["userunit"].ToString();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            var rowven = Session["rowven"].ToString();
            var name = Session["username"].ToString();
            string purch_6 = Session["purch_6"].ToString();
            path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/PrinterServlete939ffe9E16_2.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryAcc =
                        from acc in mpms.ACCOUNT
                        where acc.USER_NAME.Equals(name)
                        select acc.DEPT_SN;
                    foreach (var q in queryAcc)
                    {
                        if (q == "0A100" || q == "00N00")
                            doc.ReplaceText("[$UNIT$]", "處", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$UNIT$]", "單位", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$PURCH$]", ViewState["rowtext"].ToString(), false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$NAME$]", lblOVC_PUR_IPURCH.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryNSECTION = mpms.TBM1301
                        .Where(o => o.OVC_PURCH.Equals(purch)).FirstOrDefault();
                    if (queryNSECTION != null)
                        doc.ReplaceText("[$OVC_PUR_NSECTION$]", queryNSECTION.OVC_PUR_NSECTION, false, System.Text.RegularExpressions.RegexOptions.None);
                    else
                        doc.ReplaceText("[$OVC_PUR_NSECTION$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryItem =
                        from item in mpms.TBMRECEIVE_CHECK_ITEM
                        where item.OVC_PURCH.Equals(purch)
                        where item.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            ONB_ITEM = item.ONB_ITEM,
                            OVC_ITEM_NAME = item.OVC_ITEM_NAME,
                            OVC_RESULT = item.OVC_RESULT
                        };
                    if (queryItem.Count() < 1)
                    {
                        for (int i = 1; i < 40; i++)
                        {
                            for (int j = 1; j < 3; j++)
                            {
                                string chk = "[$CHK" + i + "$]";
                                string tick = "[$TICK" + i + "_" + j + "$]";
                                doc.ReplaceText(chk, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText(tick, "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                        }
                    }
                    foreach (var q in queryItem)
                    {
                        if (q.OVC_RESULT == "免審")
                        {
                            if (q.ONB_ITEM == 1001)
                            {
                                doc.ReplaceText("[$CHK1$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK1_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK1_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 1002)
                            {
                                doc.ReplaceText("[$CHK2$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK2_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK2_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 1003)
                            {
                                doc.ReplaceText("[$CHK3$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK3_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK3_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 1004)
                            {
                                doc.ReplaceText("[$CHK4$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK4_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK4_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 1005)
                            {
                                doc.ReplaceText("[$CHK5$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK4_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK4_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 1006)
                            {
                                doc.ReplaceText("[$CHK6$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK4_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK4_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 1007)
                            {
                                doc.ReplaceText("[$CHK7$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK4_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK4_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 1008)
                            {
                                doc.ReplaceText("[$CHK8$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK5_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK5_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 2001)
                            {
                                doc.ReplaceText("[$CHK9$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK6_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK6_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 2002)
                            {
                                doc.ReplaceText("[$CHK10$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK6_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK6_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 2003)
                            {
                                doc.ReplaceText("[$CHK11$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK6_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK6_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 2004)
                            {
                                doc.ReplaceText("[$CHK12$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK6_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK6_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 2005)
                            {
                                doc.ReplaceText("[$CHK13$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK6_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK6_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 2006)
                            {
                                doc.ReplaceText("[$CHK14$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK6_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK6_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 3001)
                            {
                                doc.ReplaceText("[$CHK15$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 3002)
                            {
                                doc.ReplaceText("[$CHK16$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 3003)
                            {
                                doc.ReplaceText("[$CHK17$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 3004)
                            {
                                doc.ReplaceText("[$CHK18$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 3005)
                            {
                                doc.ReplaceText("[$CHK19$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 3006)
                            {
                                doc.ReplaceText("[$CHK20$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 3007)
                            {
                                doc.ReplaceText("[$CHK21$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 3008)
                            {
                                doc.ReplaceText("[$CHK22$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 3009)
                            {
                                doc.ReplaceText("[$CHK23$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 3010)
                            {
                                doc.ReplaceText("[$CHK24$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 3011)
                            {
                                doc.ReplaceText("[$CHK25$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 3012)
                            {
                                doc.ReplaceText("[$CHK26$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 3013)
                            {
                                doc.ReplaceText("[$CHK27$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK7_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4001)
                            {
                                doc.ReplaceText("[$CHK28$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK8_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK8_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4002)
                            {
                                doc.ReplaceText("[$CHK29$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK9_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK9_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4003)
                            {
                                doc.ReplaceText("[$CHK30$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK10_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK10_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4004)
                            {
                                doc.ReplaceText("[$CHK31$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK11_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK11_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4005)
                            {
                                doc.ReplaceText("[$CHK32$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK12_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK12_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4006)
                            {
                                doc.ReplaceText("[$CHK33$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK13_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK13_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4007)
                            {
                                doc.ReplaceText("[$CHK34$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK14_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK14_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 5001)
                            {
                                doc.ReplaceText("[$CHK35$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK15_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK15_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 5002)
                            {
                                doc.ReplaceText("[$CHK36$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK16_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK16_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 5003)
                            {
                                doc.ReplaceText("[$CHK37$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK17_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK17_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 6001)
                            {
                                doc.ReplaceText("[$CHK38$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK18_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK18_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 7001)
                            {
                                doc.ReplaceText("[$CHK39$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK19_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK19_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                        }
                        else if (q.OVC_RESULT == "是")
                        {
                            if (q.ONB_ITEM == 1001)
                            {
                                doc.ReplaceText("[$CHK1$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK1_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK1_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 1002)
                            {
                                doc.ReplaceText("[$CHK2$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK2_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK2_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 1003)
                            {
                                doc.ReplaceText("[$CHK3$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK3_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK3_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 1004)
                            {
                                doc.ReplaceText("[$CHK4$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo++;
                            }
                            if (q.ONB_ITEM == 1005)
                            {
                                doc.ReplaceText("[$CHK5$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo++;
                            }
                            if (q.ONB_ITEM == 1006)
                            {
                                doc.ReplaceText("[$CHK6$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo++;
                            }
                            if (q.ONB_ITEM == 1007)
                            {
                                doc.ReplaceText("[$CHK7$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo++;
                            }
                            if (q.ONB_ITEM == 1008)
                            {
                                doc.ReplaceText("[$CHK8$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK5_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK5_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 2001)
                            {
                                doc.ReplaceText("[$CHK9$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_2++;
                            }
                            if (q.ONB_ITEM == 2002)
                            {
                                doc.ReplaceText("[$CHK10$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_2++;
                            }
                            if (q.ONB_ITEM == 2003)
                            {
                                doc.ReplaceText("[$CHK11$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_2++;
                            }
                            if (q.ONB_ITEM == 2004)
                            {
                                doc.ReplaceText("[$CHK12$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_2++;
                            }
                            if (q.ONB_ITEM == 2005)
                            {
                                doc.ReplaceText("[$CHK13$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_2++;
                            }
                            if (q.ONB_ITEM == 2006)
                            {
                                doc.ReplaceText("[$CHK14$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_2++;
                            }
                            if (q.ONB_ITEM == 3001)
                            {
                                doc.ReplaceText("[$CHK15$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_3++;
                            }
                            if (q.ONB_ITEM == 3002)
                            {
                                doc.ReplaceText("[$CHK16$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_3++;
                            }
                            if (q.ONB_ITEM == 3003)
                            {
                                doc.ReplaceText("[$CHK17$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_3++;
                            }
                            if (q.ONB_ITEM == 3004)
                            {
                                doc.ReplaceText("[$CHK18$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_3++;
                            }
                            if (q.ONB_ITEM == 3005)
                            {
                                doc.ReplaceText("[$CHK19$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_3++;
                            }
                            if (q.ONB_ITEM == 3006)
                            {
                                doc.ReplaceText("[$CHK20$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_3++;
                            }
                            if (q.ONB_ITEM == 3007)
                            {
                                doc.ReplaceText("[$CHK21$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_3++;
                            }
                            if (q.ONB_ITEM == 3008)
                            {
                                doc.ReplaceText("[$CHK22$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_3++;
                            }
                            if (q.ONB_ITEM == 3009)
                            {
                                doc.ReplaceText("[$CHK23$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_3++;
                            }
                            if (q.ONB_ITEM == 3010)
                            {
                                doc.ReplaceText("[$CHK24$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_3++;
                            }
                            if (q.ONB_ITEM == 3011)
                            {
                                doc.ReplaceText("[$CHK25$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_3++;
                            }
                            if (q.ONB_ITEM == 3012)
                            {
                                doc.ReplaceText("[$CHK26$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_3++;
                            }
                            if (q.ONB_ITEM == 3013)
                            {
                                doc.ReplaceText("[$CHK27$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                YesNo_3++;
                            }
                            if (q.ONB_ITEM == 4001)
                            {
                                doc.ReplaceText("[$CHK28$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK8_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK8_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4002)
                            {
                                doc.ReplaceText("[$CHK29$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK9_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK9_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4003)
                            {
                                doc.ReplaceText("[$CHK30$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK10_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK10_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4004)
                            {
                                doc.ReplaceText("[$CHK31$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK11_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK11_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4005)
                            {
                                doc.ReplaceText("[$CHK32$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK12_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK12_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4006)
                            {
                                doc.ReplaceText("[$CHK33$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK13_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK13_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4007)
                            {
                                doc.ReplaceText("[$CHK34$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK14_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK14_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 5001)
                            {
                                doc.ReplaceText("[$CHK35$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK15_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK15_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 5002)
                            {
                                doc.ReplaceText("[$CHK36$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK16_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK16_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 5003)
                            {
                                doc.ReplaceText("[$CHK37$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK17_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK17_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 6001)
                            {
                                doc.ReplaceText("[$CHK38$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK18_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK18_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 7001)
                            {
                                doc.ReplaceText("[$CHK39$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK19_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK19_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                        }
                        else
                        {
                            if (q.ONB_ITEM == 1001)
                            {
                                doc.ReplaceText("[$CHK1$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK1_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK1_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 1002)
                            {
                                doc.ReplaceText("[$CHK2$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK2_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK2_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 1003)
                            {
                                doc.ReplaceText("[$CHK3$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK3_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK3_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 1004)
                                doc.ReplaceText("[$CHK4$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 1005)
                                doc.ReplaceText("[$CHK5$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 1006)
                                doc.ReplaceText("[$CHK6$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 1007)
                                doc.ReplaceText("[$CHK7$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 1008)
                            {
                                doc.ReplaceText("[$CHK8$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK5_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK5_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 2001)
                                doc.ReplaceText("[$CHK9$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 2002)
                                doc.ReplaceText("[$CHK10$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 2003)
                                doc.ReplaceText("[$CHK11$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 2004)
                                doc.ReplaceText("[$CHK12$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 2005)
                                doc.ReplaceText("[$CHK13$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 2006)
                                doc.ReplaceText("[$CHK14$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 3001)
                                doc.ReplaceText("[$CHK15$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 3002)
                                doc.ReplaceText("[$CHK16$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 3003)
                                doc.ReplaceText("[$CHK17$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 3004)
                                doc.ReplaceText("[$CHK18$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 3005)
                                doc.ReplaceText("[$CHK19$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 3006)
                                doc.ReplaceText("[$CHK20$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 3007)
                                doc.ReplaceText("[$CHK21$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 3008)
                                doc.ReplaceText("[$CHK22$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 3009)
                                doc.ReplaceText("[$CHK23$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 3010)
                                doc.ReplaceText("[$CHK24$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 3011)
                                doc.ReplaceText("[$CHK25$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 3012)
                                doc.ReplaceText("[$CHK26$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 3013)
                                doc.ReplaceText("[$CHK27$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ITEM == 4001)
                            {
                                doc.ReplaceText("[$CHK28$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK8_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK8_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4002)
                            {
                                doc.ReplaceText("[$CHK29$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK9_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK9_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4003)
                            {
                                doc.ReplaceText("[$CHK30$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK10_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK10_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4004)
                            {
                                doc.ReplaceText("[$CHK31$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK11_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK11_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4005)
                            {
                                doc.ReplaceText("[$CHK32$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK12_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK12_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4006)
                            {
                                doc.ReplaceText("[$CHK33$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK13_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK13_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 4007)
                            {
                                doc.ReplaceText("[$CHK34$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK14_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK14_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 5001)
                            {
                                doc.ReplaceText("[$CHK35$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK15_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK15_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 5002)
                            {
                                doc.ReplaceText("[$CHK36$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK16_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK16_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 5003)
                            {
                                doc.ReplaceText("[$CHK37$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK17_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK17_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 6001)
                            {
                                doc.ReplaceText("[$CHK38$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK18_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK18_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            if (q.ONB_ITEM == 7001)
                            {
                                doc.ReplaceText("[$CHK39$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK19_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$TICK19_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                        }
                    }
                    if (YesNo == 4)
                    {
                        doc.ReplaceText("[$TICK4_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$TICK4_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    else
                    {
                        doc.ReplaceText("[$TICK4_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$TICK4_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    if (YesNo_2 == 6)
                    {
                        doc.ReplaceText("[$TICK6_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$TICK6_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    else
                    {
                        doc.ReplaceText("[$TICK6_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$TICK6_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    if (YesNo_3 == 13)
                    {
                        doc.ReplaceText("[$TICK7_1$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$TICK7_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    else
                    {
                        doc.ReplaceText("[$TICK7_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$TICK7_2$]", "ˇ", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    var queryBid =
                        from tbmreceove_bid in mpms.TBMRECEIVE_BID
                        join tbmreceive_check in mpms.TBMRECEIVE_CHECK
                            on tbmreceove_bid.OVC_PURCH equals tbmreceive_check.OVC_PURCH
                        where tbmreceove_bid.OVC_PURCH.Equals(purch)
                        where tbmreceive_check.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            OVC_DO_NAME_RESULT = tbmreceove_bid.OVC_DO_NAME_RESULT,
                            OVC_REJECT_REASON_DO = tbmreceove_bid.OVC_REJECT_REASON_DO,
                            OVC_DSEND = tbmreceive_check.OVC_DSEND,
                            OVC_DRECEIVE = tbmreceive_check.OVC_DRECEIVE,
                            OVC_DO_NAME = tbmreceive_check.OVC_DO_NAME,
                            OVC_DO_DAPPROVE = tbmreceive_check.OVC_DO_DAPPROVE
                        };
                    foreach (var q in queryBid)
                    {
                        if (q.OVC_DO_NAME_RESULT == "1")
                        {
                            doc.ReplaceText("[$CHK40$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK41$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK42$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else if (q.OVC_DO_NAME_RESULT == "2")
                        {
                            doc.ReplaceText("[$CHK40$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK41$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK42$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else if (q.OVC_DO_NAME_RESULT == "3")
                        {
                            doc.ReplaceText("[$CHK40$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK41$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK42$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }

                    doc.ReplaceText("[$CHK40$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$CHK41$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$CHK42$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$CHK39$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$TICK19_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$TICK19_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/b.docx");
                }
                buffer = ms.ToArray();
            }
            string path_d = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/b.docx");
            FileInfo file = new FileInfo(path_d);
            string filepath = purch + "檢查項目表.docx";
            string fileName = HttpUtility.UrlEncode(filepath);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            Response.End();
        }
        #endregion

        #region 合約
        void Contract_ExportToWord()
        {
            int year = 0;
            string date = "";
            var userunit = Session["userunit"].ToString();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            var rowven = Session["rowven"].ToString();
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/PrinterServletE13_6.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryNSECTION = mpms.TBM1301
                        .Where(o => o.OVC_PURCH.Equals(purch)).FirstOrDefault();
                    doc.ReplaceText("[$OVC_PUR_NSECTION$]", queryNSECTION != null ? queryNSECTION.OVC_PUR_NSECTION : "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryPurch =
                        from tbm1301 in mpms.TBM1301_PLAN
                        join tbm1302 in mpms.TBM1302
                            on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                        where tbm1301.OVC_PURCH.Equals(purch)
                        where tbm1302.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            OVC_PURCH = tbm1301.OVC_PURCH,
                            OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                            OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                            OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                            OVC_DBID = tbm1302.OVC_DBID,
                            OVC_DCONTRACT = tbm1302.OVC_DCONTRACT,
                            OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE,
                            OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                            ONB_MONEY = tbm1302.ONB_MONEY,
                            OVC_VEN_BOSS = tbm1302.OVC_VEN_BOSS,
                            OVC_VEN_NAME = tbm1302.OVC_VEN_NAME,
                            OVC_VEN_FAX = tbm1302.OVC_VEN_FAX,
                            OVC_VEN_CELLPHONE = tbm1302.OVC_VEN_CELLPHONE,
                            OVC_VEN_ADDRESS = tbm1302.OVC_VEN_ADDRESS,
                            OVC_PUR_IUSER_PHONE = tbm1301.OVC_PUR_IUSER_PHONE,
                            OVC_PUR_IUSER_FAX = tbm1301.OVC_PUR_IUSER_FAX
                        };
                    foreach (var q in queryPurch)
                    {
                        if (q.OVC_VEN_TITLE.Equals(txtOVC_VEN_TITLE.Text))
                        {
                            doc.ReplaceText("[$PURCH$]", q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5, false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$PURCH_6$]", q.OVC_PURCH_6, false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.OVC_DBID != null)
                            {
                                year = int.Parse(q.OVC_DBID.Substring(0, 4)) - 1911;
                                date = year.ToString() + "年" + q.OVC_DBID.Substring(5, 2) + "月" + q.OVC_DBID.Substring(8, 2) + "日";
                                doc.ReplaceText("[$DBID$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            doc.ReplaceText("[$DBID$]", "   年  月  日", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.OVC_DCONTRACT != null)
                            {
                                year = int.Parse(q.OVC_DCONTRACT.Substring(0, 4)) - 1911;
                                date = year.ToString() + "年" + q.OVC_DCONTRACT.Substring(5, 2) + "月" + q.OVC_DCONTRACT.Substring(8, 2) + "日";
                                doc.ReplaceText("[$DCONTRACT$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            doc.ReplaceText("[$DCONTRACT$]", "   年  月  日", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$VEN_TITLE$]", q.OVC_VEN_TITLE != null ? q.OVC_VEN_TITLE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$NAME$]", q.OVC_PUR_IPURCH != null ? q.OVC_PUR_IPURCH : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY$]", q.ONB_MONEY != null ? String.Format("{0:N}", q.ONB_MONEY) : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_MONEY != null)
                            {
                                string money = EastAsiaNumericFormatter.FormatWithCulture("Lc", q.ONB_MONEY, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                                doc.ReplaceText("[$CH_MONEY$]", money + "元整", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            doc.ReplaceText("[$CH_MONEY$]", "零元整", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$PRINCIPAL$]", q.OVC_VEN_BOSS != null ? q.OVC_VEN_BOSS : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CONTACT$]", q.OVC_VEN_NAME != null ? q.OVC_VEN_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$FAX$]", q.OVC_VEN_FAX != null ? q.OVC_VEN_FAX : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CELLPHONE$]", q.OVC_VEN_CELLPHONE != null ? q.OVC_VEN_CELLPHONE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$ADDRESS$]", q.OVC_VEN_ADDRESS != null ? q.OVC_VEN_ADDRESS : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$IUSER_PHONE$]", q.OVC_PUR_IUSER_PHONE != null ? q.OVC_PUR_IUSER_PHONE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$IUSER_FAX$]", q.OVC_PUR_IUSER_FAX != null ? q.OVC_PUR_IUSER_FAX : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                    var queryCurrency =
                        from tbm1302 in mpms.TBM1302
                        join tbm1407 in mpms.TBM1407
                            on tbm1302.OVC_CURRENT equals tbm1407.OVC_PHR_ID
                        where tbm1302.OVC_PURCH.Equals(purch)
                        where tbm1302.OVC_PURCH_6.Equals(purch)
                        where tbm1407.OVC_PHR_CATE.Equals("B0")
                        select tbm1407.OVC_PHR_DESC;
                    if (queryCurrency.Count() > 0)
                    {
                        foreach (var q in queryCurrency)
                        {
                            doc.ReplaceText("[$Currency$]", q, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                    doc.ReplaceText("[$Currency$]", "新台幣", false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryTime =
                        from tbm1220_1 in mpms.TBM1220_1
                        where tbm1220_1.OVC_PURCH.Equals(purch)
                        where tbm1220_1.OVC_IKIND.Equals("D56")
                        select tbm1220_1.OVC_MEMO;
                    if (queryTime.Count() > 0)
                    {
                        foreach (var q in queryTime)
                        {
                            doc.ReplaceText("[$TIME$]", q, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                    doc.ReplaceText("[$TIME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryPlace =
                        from tbm1220_1 in mpms.TBM1220_1
                        where tbm1220_1.OVC_PURCH.Equals(purch)
                        where tbm1220_1.OVC_IKIND.Equals("D57")
                        select tbm1220_1.OVC_MEMO;
                    if (queryPlace.Count() > 0)
                    {
                        foreach (var q in queryPlace)
                        {
                            doc.ReplaceText("[$PLACE$]", q, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                    doc.ReplaceText("[$PLACE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryWay =
                        from tbm1220_1 in mpms.TBM1220_1
                        where tbm1220_1.OVC_PURCH.Equals(purch)
                        where tbm1220_1.OVC_IKIND.Equals("D5B")
                        select tbm1220_1.OVC_MEMO;
                    if (queryWay.Count() > 0)
                    {
                        foreach (var q in queryWay)
                        {
                            doc.ReplaceText("[$WAY$]", q, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                    doc.ReplaceText("[$WAY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/Contract_Temp.docx");
                }
                buffer = ms.ToArray();
            }
        }
        #endregion

        #region WordToPDF
        static void WordcvDdf(string args, string wordfilepath)
        {
            // word 檔案位置
            string sourcedocx = args;
            // PDF 儲存位置
            // string targetpdf =  @"C:\Users\linon\Downloads\ddd.pdf";

            //建立 word application instance
            Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
            //開啟 word 檔案
            var wordDocument = appWord.Documents.Open(sourcedocx);

            //匯出為 pdf
            wordDocument.ExportAsFixedFormat(wordfilepath, WdExportFormat.wdExportFormatPDF);

            //關閉 word 檔
            wordDocument.Close();
            //結束 word
            appWord.Quit();
        }
        #endregion

        #region getTaiwanDate
        private string getTaiwanDate(string strDate)
        {
            if (DateTime.TryParse(strDate, out DateTime datetime))
            {
                CultureInfo culture = new CultureInfo("zh-TW");
                culture.DateTimeFormat.Calendar = new TaiwanCalendar();
                return datetime.ToString("yyy年MM月dd日", culture);
            }
            else
                return "";
        }
        #endregion

        #region GetTaiwanDate
        private string GetTaiwanDate(string strDate, string cString)
        {
            if (DateTime.TryParse(strDate, out DateTime datetime))
            {
                CultureInfo culture = new CultureInfo("zh-TW");
                culture.DateTimeFormat.Calendar = new TaiwanCalendar();
                if (cString.Equals("chDate"))
                {
                    return datetime.ToString("yyy年MM月dd日", culture);
                }
                else
                    return datetime.ToString("yyyy" + cString + "MM" + cString + "dd", culture);
            }
            else
                return "";

        }
        #endregion

        #region GetAttached
        private string GetAttached(string kind)
        {
            var strPurchNum = lblOVC_PURCH.Text.Substring(0, 7);
            //附件內容
            var queryFile =
                mpms.TBM1119.AsEnumerable()
                .Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_IKIND.Equals(kind))
                .OrderByDescending(o => o.OVC_ATTACH_NAME.Split('.')[0].Length)
                .OrderBy(o => o.OVC_ATTACH_NAME.Split('.')[0]);
            string[] arrFileName = new string[queryFile.Count()];
            int counter = 0;
            foreach (var row in queryFile)
            {
                arrFileName[counter] = row.OVC_ATTACH_NAME;
                counter++;
            }
            string strFileName = string.Join("、", arrFileName);
            return strFileName;
        }
        #endregion

        #region 上方按鈕(需要用onmouseover改變文字)
        void btnMouseover()
        {
            lblONB_MCONTRACTPer1_1.Text = "履約保證金";
            query1220("D55", lblONB_MCONTRACTPer1_2);
            lblONB_MCONTRACTPer2_1.Text = "契約交貨時間";
            query1220("D56", lblONB_MCONTRACTPer2_2);
            lblONB_MCONTRACTPer3_1.Text = "契約交貨地點";
            query1220("D57", lblONB_MCONTRACTPer3_2);
            lblONB_MCONTRACTPer4_1.Text = "檢驗方式";
            query1220("D59", lblONB_MCONTRACTPer4_2);
            lblONB_MCONTRACTPer5_1.Text = "付款方式";
            query1220("D5B", lblONB_MCONTRACTPer5_2);
            lblONB_MCONTRACTPer6_1.Text = "免稅方式";
            query1220("D5E", lblONB_MCONTRACTPer6_2);
            lblONB_MCONTRACTPer7_1.Text = "保固方式";
            query1220("D5G", lblONB_MCONTRACTPer7_2);
        }
        #region Query
        void query1220(string str, Label lab)
        {
            var purch = ViewState["rowtext"].ToString().Substring(0, 7);
            var query =
                from tbm1220_1 in mpms.TBM1220_1
                where tbm1220_1.OVC_PURCH.Equals(purch)
                where tbm1220_1.OVC_IKIND.Equals(str)
                select tbm1220_1.OVC_MEMO;
            if (query.Count() > 0)
            {
                foreach (var q in query)
                {
                    string s = q.Replace("。\r\n", "。<br>");
                    //s = q.Replace("\r\n(", "<br>(");
                    //s = s.Replace("\r\n（", "<br>（");
                    lab.Text = s;
                }
            }
            else
            {
                lab.Text = "";
            }
        }
        #endregion

        #endregion
        
        #endregion

        #region WordToODT
        private void WordToODT(string FromPath, string TargetPath, string fileName)
        {
            var WordApp = new Microsoft.Office.Interop.Word.Application();
            var workbooks = WordApp.Documents;
            var doc = workbooks.Open(FromPath);

            //  Microsoft.Office.Interop.Word.Document doc = WordApp.Documents.Open(FromPath);
            try
            {
                doc.SaveAs2(TargetPath, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatOpenDocumentText);

                doc.Close();
                WordApp.Visible = false;
                WordApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(WordApp);

                doc = null;
                workbooks = null;
                WordApp = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                WordApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(WordApp);

                doc = null;
                workbooks = null;
                WordApp = null;
                
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            byte[] renderedBytes = null;
            var buffer = new byte[16 * 1024];
            using (var stream = new FileStream(TargetPath, FileMode.Open))
            {
                var memoryStream = new MemoryStream();
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memoryStream.Write(buffer, 0, read);
                        renderedBytes = memoryStream.ToArray();
                    }
                }
                Response.Clear();
                Response.ContentType = "Application/application/vnd.oasis.opendocument.text";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.BinaryWrite(renderedBytes);
                Response.Flush();
                Response.Close();
                Response.End();
            }
        }
        #endregion
    }
}