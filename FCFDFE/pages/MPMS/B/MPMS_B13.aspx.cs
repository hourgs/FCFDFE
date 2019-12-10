using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.Entity;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.IO;
using System.Globalization;

namespace FCFDFE.pages.MPMS.B
{
    public class Watermark : PdfPageEventHelper
    {

        PdfContentByte cb;
        GMEntities gm = new GMEntities();
        public Watermark()
        {
        }

        public string strPurchNum{get;set;}

        public override void OnEndPage(PdfWriter writer, Document document)
        {
         
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H
               , BaseFont.NOT_EMBEDDED);//設定字型
            Font ChFont = new Font(bfChinese, 24, Font.NORMAL, new BaseColor(255, 0, 0, 60));
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
            int pageN = writer.PageNumber;
            string text = "第" + pageN + "聯";
            string text2 = text.Replace("1", "一")
            .Replace("2", "二").Replace("3", "三")
            .Replace("4", "四").Replace("5", "五")
            .Replace("6", "六").Replace("7", "七")
            .Replace("8", "八").Replace("9", "九");

            Rectangle pageSize = document.PageSize;
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
            string strcb="";
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
        
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            template = writer.DirectContent.CreateTemplate(30, 16);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            Rectangle pageSize = document.PageSize;
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
                                                                                                                                                                                                             
            cb = writer.DirectContent;
            cb.SetRGBColorFill(100, 100, 100);

            string text = "Page " + writer.CurrentPageNumber + " of ";

            PdfContentByte under = writer.DirectContentUnder;
            under.BeginText();
            under.SetRGBColorFill(0xFF, 0x88, 0x88);
            under.SetFontAndSize(bfChinese, 32);
            var query1301 =
                (from t in gm.TBM1301
                where t.OVC_PURCH.Equals(strPurchNum)
                select new
                {
                    OVC_PUR_NSECTION = t.OVC_PUR_NSECTION ?? "",
                    OVC_PUR_AGENCY = t.OVC_PUR_AGENCY ?? "",
                    OVC_DPROPOSE = t.OVC_PUR_AGENCY ?? "",
                    OVC_PROPOSE = t.OVC_PUR_AGENCY ?? "",
                    OVC_PUR_DAPPROVE = t.OVC_PUR_DAPPROVE ?? "",
                    OVC_PUR_APPROVE = t.OVC_PUR_APPROVE ?? "",
                    OVC_PURCH_KIND = t.OVC_PURCH_KIND ?? "",

                }).FirstOrDefault();
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

            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, year + "年度" +query1301.OVC_PUR_NSECTION+kind + "財務勞務採購計畫清單", pageSize.GetRight(300), pageSize.GetTop(80), 0);

            cb.SetFontAndSize(bfChinese, 12f);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "中華民國" + getTaiwanDate(query1301.OVC_DPROPOSE) + query1301.OVC_PROPOSE, pageSize.GetRight(150), pageSize.GetTop(95), 0);

            cb.SetFontAndSize(bfChinese, 12f);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "第" + writer.CurrentPageNumber + "頁", pageSize.GetRight(30), pageSize.GetTop(110), 0);

            cb.SetFontAndSize(bfChinese, 12f);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "中華民國" + getTaiwanDate(query1301.OVC_PUR_DAPPROVE) + query1301.OVC_PUR_APPROVE, pageSize.GetLeft(30), pageSize.GetBottom(40), 0);
            cb.AddTemplate(template, pageSize.GetRight(30), pageSize.GetBottom(40));

            cb.SetFontAndSize(bf, 11);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pageSize.GetRight(30), pageSize.GetBottom(40), 0);
            cb.EndText();

        }
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            base.OnCloseDocument(writer, document);
            template.BeginText();
            template.SetFontAndSize(bf, 11);
            template.SetTextMatrix(0, 0);
            template.ShowText("" + (writer.PageNumber));
            template.EndText();

        }
        
        private string getTaiwanDate(string strDate)
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
    }

    class TopBottomTableBorderMaker : IPdfPTableEvent
    {
        private BaseColor _borderColor;
        private float _borderWidth;

        /// <summary>
        /// Add a top and bottom border to the table.
        /// </summary>
        /// <param name="borderColor">The color of the border.</param>
        /// <param name="borderWidth">The width of the border</param>
        public TopBottomTableBorderMaker(BaseColor borderColor, float borderWidth)
        {
            this._borderColor = borderColor;
            this._borderWidth = borderWidth;
        }
        public void TableLayout(PdfPTable table, float[][] widths, float[] heights, int headerRows, int rowStart, PdfContentByte[] canvases)
        {
            //widths (should be thought of as x's) is an array of arrays, first index is for each row, second index is for each column
            //The below uses first and last to calculate where each X should start and end
            var firstRowWidths = widths[0];
            var lastRowWidths = widths[widths.Length - 1];

            var firstRowXStart = firstRowWidths[0];
            var firstRowXEnd = firstRowWidths[firstRowWidths.Length - 1] - firstRowXStart+12;

            var lastRowXStart = lastRowWidths[0];
            var lastRowXEnd = lastRowWidths[lastRowWidths.Length - 1] - lastRowXStart+12;

            //heights (should be thought of as y's) is the y for each row's top plus one extra for the last row's bottom
            //The below uses first and last to calculate where each Y should start and end
            var firstRowYStart = heights[0];
            var firstRowYEnd = heights[1] - firstRowYStart;
            
            var lastRowYStart = heights[heights.Length - 1];
            var lastRowYEnd = heights[heights.Length - 2] - lastRowYStart;

            //Where we're going to draw our lines
            PdfContentByte canvas = canvases[PdfPTable.LINECANVAS];

            //I always try to save the previous state before changinge anything
            canvas.SaveState();

            //Set our line properties
            canvas.SetLineWidth(this._borderWidth);
            canvas.SetColorStroke(this._borderColor);
            
            //Draw Upline
            canvas.MoveTo(firstRowXStart, firstRowYStart);
            canvas.LineTo(firstRowXEnd, firstRowYStart);
            //They aren't actually drawn until you stroke them!
            canvas.Stroke();
            //Draw Underline
            canvas.MoveTo(lastRowXStart, lastRowYStart);
            canvas.LineTo(lastRowXEnd, lastRowYStart);
            canvas.Stroke();

            //Restore any previous settings
            canvas.RestoreState();
        }
    }

    public partial class MPMS_B13 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        //新增測試登入角色  鄭博心 F129278006 19760212 HK96301
        //修改測試登入角色  林哲仁 A121555832 95708888 JC96001
        
        string strPurchOK="";
        string strOVC_PUR_AGENCY = "";
        string strPurchNum;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (Request.QueryString["PurchNum"] != null)
                {
                    strPurchNum = Request.QueryString["PurchNum"];
                    if (!IsPostBack)
                    {
                        FCommon.Controls_Attributes("readonly", "true", txtOVC_AGNT_IN_SHOW_IN, txtOVC_AGNT_IN_SHOW);
                        TBM1301 table1301 = new TBM1301();
                        table1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();

                        //判斷內外購
                        TBM1301_PLAN table1301plan = new TBM1301_PLAN();
                        table1301plan = gm.TBM1301_PLAN.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                        strOVC_PUR_AGENCY = table1301plan.OVC_PUR_AGENCY;
                        ViewState["OVC_PUR_AGENCY"] = strOVC_PUR_AGENCY;
                        if (table1301plan.OVC_PUR_AGENCY == "B" || table1301plan.OVC_PUR_AGENCY == "L" || table1301plan.OVC_PUR_AGENCY == "P")
                        {
                            lblStubIn.Visible = true;
                            divSecContentIN.Visible = true;
                            lblStubOut.Visible = false;
                            divSecContentOUT.Visible = false;
                            OutPart.Visible = false;
                            ViewState["strOVC_PURCH_KIND"] = "1";
                        }
                        else
                        {
                            lblStubIn.Visible = false;
                            divSecContentIN.Visible = false;
                            lblStubOut.Visible = true;
                            divSecContentOUT.Visible = true;
                            OutPart.Visible = true;
                            ViewState["strOVC_PURCH_KIND"] = "2";
                        }
                        LoginScreen();
                        if (table1301 == null)
                        {
                            //判斷由甚麼條件開啟-新增
                            btnSave_New.Visible = true;
                            btnSave_Modify.Visible = false;
                            //預算編輯按鈕
                            btnModify_BUDGET.Visible = false;
                            //請求事項編輯按鈕
                            btnModify.Visible = false;
                            //備考編輯按鈕
                            btnModify_MEMO.Visible = false;
                            //用途編輯按鈕
                            btnUseEditing.Visible = false;
                            //外購理由編輯按鈕
                            btnOutPurEditing.Visible = false;
                            //頁籤2頭
                            pageSecond.Visible = false;
                            //頁籤3頭+內容
                            NewLoginScreen();
                            //列印物資申請書按鈕
                            btnGOOD_APPLICATION_PRINT.Visible = false;
                            //列印新物資申請書按鈕
                            btnOLD_GOOD_APPLICATION_PRINT.Visible = false;
                        }
                        else
                        {
                            //判斷由甚麼條件開啟-修改
                            btnSave_New.Visible = false;
                            btnSave_Modify.Visible = true;
                            //預算編輯按鈕
                            btnModify_BUDGET.Visible = true;
                            //請求事項編輯按鈕
                            btnModify.Visible = true;
                            //備考編輯按鈕
                            btnModify_MEMO.Visible = true;
                            //用途編輯按鈕
                            btnUseEditing.Visible = true;
                            //外購理由編輯按鈕
                            btnOutPurEditing.Visible = true;
                            //頁籤3頭+內容 如果有就要開
                            ModifyLoginScreen();
                            //列印物資申請書按鈕
                            btnGOOD_APPLICATION_PRINT.Visible = true;
                            //列印新物資申請書按鈕
                            btnOLD_GOOD_APPLICATION_PRINT.Visible = true;
                        }
                    }
                    if (GridView1.Rows.Count > 0)
                        FooterSum(GridView1);
                    if (GridView1_IN.Rows.Count > 0)
                        FooterSum(GridView1_IN);

                }
                else
                {
                    Response.Redirect("MPMS_B11");
                }
            }
            
        }

        #region 副程式
        private void LoginScreen()
        {
            //抓取登錄者姓名
            if (Session["userid"] != null)
            {
                string strUSER_ID = Session["userid"].ToString();
                ACCOUNT ac = new ACCOUNT();
                ac = gm.ACCOUNTs.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                if (ac != null)
                {
                    ViewState["UserName"]  = ac.USER_NAME;
                }
            }

            TBM1301_PLAN plan1301 = new TBM1301_PLAN();
            plan1301 = gm.TBM1301_PLAN.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();


            if (plan1301 != null)
            {
                //左上
                if (string.IsNullOrEmpty(plan1301.OVC_PUR_NSECTION))
                    txtOVC_KIND_APPLY.Text = "國防部政務辦公室";
                else
                    txtOVC_KIND_APPLY.Text = plan1301.OVC_PUR_NSECTION;
                //申購單位
                if(string.IsNullOrEmpty(plan1301.OVC_PUR_NSECTION))
                    txtOVC_PUR_NSECTION.Text = "國防部政務辦公室";
                else
                    txtOVC_PUR_NSECTION.Text = plan1301.OVC_PUR_NSECTION;

                //購案中文名稱
                txtOVC_PUR_IPURCH.Text = plan1301.OVC_PUR_IPURCH;
                //購案英文名稱(只有1301有 1301PLAN沒有)
                var query1301 = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                if(query1301!=null)
                    txtOVC_PUR_IPURCH_ENG.Text = query1301.OVC_PUR_IPURCH_ENG;
                //計畫性質
                var query = gm.TBM1407;
                string textFirst = "不可空白";
                string valueFirst = "0";
                DataTable dtOVC_PLAN_PURCH = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "TD").ToList());
                FCommon.list_dataImportV(drpOVC_PLAN_PURCH, dtOVC_PLAN_PURCH, "OVC_PHR_DESC", "OVC_PHR_ID", textFirst, valueFirst, ":", true);
                drpOVC_PLAN_PURCH.SelectedValue = plan1301.OVC_PLAN_PURCH;
                //承辦人資料
                txtOVC_USER.Text = plan1301.OVC_PUR_USER;
                txtOVC_PUR_IUSER_PHONE.Text = plan1301.OVC_PUR_IUSER_PHONE;
                txtOVC_PUR_IUSER_PHONE_EXT.Text = plan1301.OVC_PUR_IUSER_PHONE_EXT;
                txtOVC_USER_CELLPHONE.Text = plan1301.OVC_USER_CELLPHONE;
                txtOVC_PUR_IUSER_PHONE_EXT1.Text = plan1301.OVC_PUR_IUSER_PHONE_EXT1;
                
                //按鈕
                //預算編輯
                btnModify_BUDGET.Visible = false;
                //請求事項編輯按鈕
                btnModify.Visible = false;
                //備考編輯按鈕
                btnModify_MEMO.Visible = false;
                //標的分類勞務RD財務RC
                rdoOVC_LAB.SelectedValue = plan1301.OVC_LAB;
                if (rdoOVC_LAB.SelectedValue.Equals("1"))
                {
                    DataTable dtStandardType = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "RD").ToList());
                    FCommon.list_dataImportV(drpStandardType, dtStandardType, "OVC_PHR_DESC", "OVC_PHR_ID", textFirst, valueFirst, ":", true);
                }
                else
                {
                    DataTable dtStandardType = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "RC").ToList());
                    FCommon.list_dataImportV(drpStandardType, dtStandardType, "OVC_PHR_DESC", "OVC_PHR_ID", textFirst, valueFirst, ":", true);
                }
                //履約地點
                DataTable dtOVC_RECEIVE_PLACE = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "R4").ToList());
                FCommon.list_dataImportV(drpOVC_RECEIVE_PLACE, dtOVC_RECEIVE_PLACE, "OVC_PHR_DESC", "OVC_PHR_ID", textFirst, valueFirst, ":", true);
                drpOVC_RECEIVE_PLACE.SelectedValue = plan1301.OVC_PLAN_PURCH;
                //本案為複數決標
                FCommon.list_dataImportYN(drpIS_PLURAL_BASIS, false, true);
                //本案為開放式契約
                FCommon.list_dataImportYN(drpIS_OPEN_CONTRACT, false, true);
                //本案為並列得標廠商
                FCommon.list_dataImportYN(drpIS_JUXTAPOSED_MANUFACTURER, false, true);
                //軍品類別-內
                DataTable dtOVC_PUR_NPURCH_IN = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "Q8").ToList());
                FCommon.list_dataImportV(drpOVC_PUR_NPURCH_IN, dtOVC_PUR_NPURCH_IN, "OVC_PHR_DESC", "OVC_PHR_ID", "請選擇-可空白", valueFirst, ":", true);
                //軍品類別-外
                DataTable dtOVC_PUR_NPURCH = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "Q8").ToList());
                FCommon.list_dataImportV(drpOVC_PUR_NPURCH, dtOVC_PUR_NPURCH, "OVC_PHR_DESC", "OVC_PHR_ID", textFirst, valueFirst, ":", true);
                //採購單位
                DataTable dtOVC_AGNT_IN_IN = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "GK").ToList());
                FCommon.list_dataImport(drpOVC_AGNT_IN_IN, dtOVC_AGNT_IN_IN, "OVC_PHR_DESC", "OVC_PHR_ID", textFirst, valueFirst, true);
                //採購單位-外
                DataTable dtOVC_AGNT_IN_OUT = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "GK").ToList());
                FCommon.list_dataImport(drpOVC_AGNT_IN_OUT, dtOVC_AGNT_IN_OUT, "OVC_PHR_DESC", "OVC_PHR_ID", textFirst, valueFirst, true);
                //採購地區-外購清單編制
                DataTable dtOVC_COUNTRY = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "C8").ToList());
                FCommon.list_dataImportV(drpOVC_COUNTRY, dtOVC_COUNTRY, "OVC_PHR_DESC", "OVC_PHR_ID", ":", false);
                //料號種類(代碼GU)-外購清單編制
                DataTable dtNSN_KIND = CommonStatic.LinqQueryToDataTable(query.Where(table => table.OVC_PHR_CATE == "GU").ToList());
                FCommon.list_dataImportV(drpNSN_KIND, dtNSN_KIND, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
                //料號種類(代碼GU)-內購清單編制
                DataTable dtNSN_KIND_IN = CommonStatic.LinqQueryToDataTable(query.Where(table => table.OVC_PHR_CATE == "GU").ToList());
                FCommon.list_dataImportV(drpNSN_KIND_IN, dtNSN_KIND_IN, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
                //單位-外購清單編制
                DataTable dtOVC_POI_IUNIT = CommonStatic.LinqQueryToDataTable(query.Where(table => table.OVC_PHR_CATE == "J1").ToList());
                FCommon.list_dataImportV(drpOVC_POI_IUNIT, dtOVC_POI_IUNIT, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
                //單位-內購清單編制
                DataTable dtOVC_POI_IUNIT_IN = CommonStatic.LinqQueryToDataTable(query.Where(table => table.OVC_PHR_CATE == "J1").ToList());
                FCommon.list_dataImportV(drpOVC_POI_IUNIT_IN, dtOVC_POI_IUNIT_IN, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
                //以往採購裡的幣別-外
                DataTable dtOVC_CURRENT = CommonStatic.LinqQueryToDataTable(query.Where(table => table.OVC_PHR_CATE == "B0").ToList());
                FCommon.list_dataImport(drpOVC_CURR_MPRICE_BEF, dtOVC_CURRENT, "OVC_PHR_DESC", "OVC_PHR_DESC", true);
                //以往採購裡的幣別-內
                DataTable dtOVC_CURRENT_IN = CommonStatic.LinqQueryToDataTable(query.Where(table => table.OVC_PHR_CATE == "B0").ToList());
                FCommon.list_dataImport(drpOVC_CURR_MPRICE_BEF_IN, dtOVC_CURRENT_IN, "OVC_PHR_DESC", "OVC_PHR_DESC", true);
            }
        }

        private void ModifyLoginScreen()
        {
            //修改的資料載入 
            DataTable Tbm12201 = new DataTable();
            TBM1301 table1301 = new TBM1301();
            TBM1231 table1231 = new TBM1231();
            TBMPURCH_EXT tableEXT = new TBMPURCH_EXT();
            string ovcIkind = "";
            int intONB_POI_ICOUNT = 0;
            DataTable tbm1118 = new DataTable();
            List<string> isourceList = new List<string>();
            List<string> pjnameList = new List<string>();
            table1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            var query1201Count = mpms.TBM1201.Where(table => table.OVC_PURCH.Equals(strPurchNum)).ToList();
            foreach (var row in query1201Count)
                intONB_POI_ICOUNT++;
            if (table1301.OVC_DPROPOSE != null)
            {
                //申購日期及文號
                lblOVC_DPROPOSE.Text = getTaiwanDate(table1301.OVC_DPROPOSE,"chDate");
                lblOVC_PROPOSE.Text = table1301.OVC_PROPOSE;
            }
            if(table1301.OVC_PUR_APPROVE != null)
            {
                //核定日期及文號 右上角
                lblOVC_PUR_DAPPROVE.Text = getTaiwanDate(table1301.OVC_PUR_DAPPROVE, "chDate");
                lblOVC_PUR_APPROVE.Text = table1301.OVC_PUR_APPROVE;
            }
            //金額
            if (table1301.ONB_PUR_BUDGET_NT != null)
            {
                decimal moneyNT = (decimal)table1301.ONB_PUR_BUDGET_NT;
                lblONB_MONEY_NT_CHI.Text = FCommon.MoneyToChinese(moneyNT.ToString()) + "(含稅)";
                lblONB_MONEY_NT_NUM.Text = moneyNT.ToString("#,0.00");
            }
            //工程會必要資料
            tableEXT = mpms.TBMPURCH_EXT.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            if (tableEXT != null)
            {
                try
                {
                    drpStandardType.SelectedValue = tableEXT.OVC_TARGET_KIND;
                }
                catch { }
                txtOVC_TARGET_KIND.Text = tableEXT.OVC_TARGET_KIND;
                drpOVC_RECEIVE_PLACE.SelectedValue = tableEXT.OVC_PERFORMANCE_PLACE;
                txtOVC_SHIP_TIMES.Text = tableEXT.OVC_PERFORMANCE_LIMIT;
                txtOVC_VENDOR_DESC.Text = tableEXT.OVC_VENDOR_DESC;
            }
            //相關稅賦
            rdoOVC_PUR_FEE_OK.SelectedValue = table1301.OVC_PUR_FEE_OK;
            rdoOVC_PUR_TAX_OK.SelectedValue = table1301.OVC_PUR_TAX_OK;
            rdoOVC_PUR_GOOD_OK.SelectedValue = table1301.OVC_PUR_GOOD_OK;
            //款源
            tbm1118 = mpms.TBM1118.Where(o => o.OVC_PURCH.Equals(strPurchNum)).ListToDataTable();
            var querySource =
                from tbm in tbm1118.AsEnumerable()
                group tbm by tbm["OVC_ISOURCE"] into g
                select g.Key;
            foreach (var group in querySource)
                isourceList.Add(group.ToString());

            lblOVC_ISOURCE.Text = string.Join(";", isourceList.ToArray());
            //計畫清單編制裡面的款源
            lblONB_PUR_BUDGET_FORM_IN.Text = lblOVC_ISOURCE.Text;
            lblONB_PUR_BUDGET_FORM_OUT.Text = lblOVC_ISOURCE.Text;
            //項目
            var queryPjname =
               from tbm in tbm1118.AsEnumerable()
               group tbm by tbm["OVC_PJNAME"] into g
               select g.Key;
            foreach (var pjname in queryPjname)
                pjnameList.Add(pjname.ToString());

            lblOVC_PJNAME.Text = string.Join(";", pjnameList.ToArray());

            //複數決標 
            drpIS_PLURAL_BASIS.SelectedValue = table1301.IS_PLURAL_BASIS;
            //複數決標頁籤頁籤
            if (drpIS_PLURAL_BASIS.SelectedValue.Equals("Y"))
            {
                pageThird.Visible = true;
                divThird.Visible = true;
                pageThirdImport(1);
                pageThirdImport_ICout();
                DrpGroupImport();
            }
            else
            {
                pageThird.Visible = false;
                divThird.Visible = false;
            }
                
            //開放式契約
            drpIS_OPEN_CONTRACT.SelectedValue = table1301.IS_OPEN_CONTRACT;
            //並列得標廠商
            drpIS_JUXTAPOSED_MANUFACTURER.SelectedValue = table1301.IS_JUXTAPOSED_MANUFACTURER;
            //計劃性質
            drpOVC_PLAN_PURCH.SelectedValue = table1301.OVC_PLAN_PURCH;
            //先查各項請求事項與清單備註
            var query =
                from tbm in mpms.TBM1220_1
                where tbm.OVC_PURCH.Equals(strPurchNum)
                select new
                {
                    tbm.OVC_IKIND,
                    tbm.OVC_MEMO
                };
            Tbm12201 = CommonStatic.LinqQueryToDataTable(query);

            //請求事項
            switch (strOVC_PUR_AGENCY)
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

            txtBoxInIt(Tbm12201, txtOVC_MEMO_REQUEST, ovcIkind);
            //備考
            switch (strOVC_PUR_AGENCY)
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
            txtBoxInIt(Tbm12201, txtOVC_MEMO, ovcIkind);
            
           
            if (ViewState["strOVC_PURCH_KIND"].ToString().Equals("2"))
            {    
                //外購
                //用途
                txtUSEBoxInIt(Tbm12201, txtUse, "M11");
                //外購理由
                txtUSEBoxInIt(Tbm12201, txtOutPur, "M21");
                //軍品類別---以下都在第二頁
                if (table1301.OVC_PUR_NPURCH != null)
                    drpOVC_PUR_NPURCH.SelectedValue = table1301.OVC_PUR_NPURCH;
                //軍品用途
                txtOVC_TARGET_DO.Text = table1301.OVC_TARGET_DO;
                //接收單位
                txtOVC_RECEIVE_NSECTION.Text = table1301.OVC_RECEIVE_NSECTION;
                //編號
                lblOVC_PURCH.Text = strPurchNum;
                //交貨時間
                txtOVC_SHIP_TIMES_OUT.Text = table1301.OVC_SHIP_TIMES;
                //運輸方式
                txtOVC_WAY_TRANS.Text = table1301.OVC_WAY_TRANS;
                //起運及輸入口岸
                txtOVC_FROM_TO.Text = table1301.OVC_FROM_TO;
                //接收地點
                txtOVC_TO_PLACE.Text = table1301.OVC_TO_PLACE;
                //檢驗方法
                txtOVC_POI_IINSPECT_OUT.Text = table1301.OVC_POI_IINSPECT;
                //採購地區
                drpOVC_COUNTRY.SelectedValue = table1301.OVC_COUNTRY;
                //採購單位
                txtOVC_AGNT_IN_SHOW.Text = table1301.OVC_AGNT_IN;
                //放項次
                lblONB_POI_ICOUNT.Text = (intONB_POI_ICOUNT + 1).ToString();
                dataImport_Detail(GridView1);
                
                //貨款總價
                decimal budgetNT = 0;
                if(table1301.ONB_PUR_BUDGET_NT!=null)
                    budgetNT = (decimal)table1301.ONB_PUR_BUDGET_NT;

                lblOVC_GOOD_TOTAL.Text = "新臺幣" + budgetNT.ToString("#,0.00") + "(含稅)";
                txtOVC_USER_TITLE.Text = table1301.OVC_USER_TITLE;

            }
            else
            {
                //內購
                //軍品類別---以下都在第二頁
                if (table1301.OVC_PUR_NPURCH != null)
                    drpOVC_PUR_NPURCH_IN.SelectedValue = table1301.OVC_PUR_NPURCH;
                //計畫清單編制-奉准日期.文號
                table1231 = mpms.TBM1231.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                if(table1231 != null)
                    lblOVC_PUR_DAPPR_PLAN_IN.Text = table1231.OVC_PUR_DAPPR_PLAN + "\t" + table1231.OVC_PUR_APPR_PLAN;
                //計畫申購單位
                lblOVC_PUR_NSECTION_IN.Text = table1301.OVC_PUR_NSECTION;
                //採購單位
                txtOVC_AGNT_IN_SHOW_IN.Text = table1301.OVC_AGNT_IN;
                //編號
                lblOVC_PURCH_IN.Text = strPurchNum;
                //接收單位
                txtOVC_RECEIVE_NSECTION_IN.Text = table1301.OVC_RECEIVE_NSECTION;
                //交貨時間
                txtOVC_SHIP_TIMES_IN.Text = table1301.OVC_SHIP_TIMES;
                //交貨地點
                txtOVC_RECEIVE_PLACE_IN.Text = table1301.OVC_RECEIVE_PLACE;
                //檢驗方法
                txtOVC_POI_IINSPECT.Text = table1301.OVC_POI_IINSPECT;
                //放項次
                lblONB_POI_ICOUNT_IN.Text = (intONB_POI_ICOUNT + 1).ToString();
                dataImport_Detail(GridView1_IN);
                //隨案檢附
                var query12201D20 = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_IKIND.Equals("D20")).Select(o => o.OVC_MEMO).FirstOrDefault();
                lbl_WITH_IN.Text = query12201D20;
                //貨款總價
                decimal budgetNT = 0;
                if (table1301.ONB_PUR_BUDGET_NT != null)
                {
                    budgetNT = (decimal)table1301.ONB_PUR_BUDGET_NT;
                    lblOVC_GOOD_TOTAL_IN.Text = "新臺幣" + budgetNT.ToString("#,0.00") + "(含稅)";
                }
                
            }
            txtOVC_SECTION_CHIEF.Text = table1301.OVC_SECTION_CHIEF;
            txtOVC_SUPERIOR_UNIT.Text = table1301.OVC_SUPERIOR_UNIT;
        }

        private void pageThirdImport(short numGroup)
        {
            //第三頁籤資料載入
            DataTable tableGroup = new DataTable();
            DataTable tableLeft = new DataTable();
            DataTable tableRight = new DataTable();

            var query =
                from table in mpms.TBM1118_2
                join table2 in mpms.TBM1201 on new { table.OVC_PURCH, table.ONB_POI_ICOUNT }
                equals new { table2.OVC_PURCH, table2.ONB_POI_ICOUNT }
                where table.OVC_PURCH.Equals(strPurchNum)
                        && table.ONB_GROUP_PRE == numGroup
                orderby table.ONB_POI_ICOUNT
                select new
                {
                    ONB_POI_ICOUNT = table.ONB_POI_ICOUNT,
                    OVC_POI_NSTUFF_CHN = table2.OVC_POI_NSTUFF_CHN,
                    OVC_BRAND = table2.OVC_BRAND
                };
            string[] strField = { "ONB_POI_ICOUNT", "OVC_POI_NSTUFF_CHN", "OVC_BRAND" };
            tableGroup = CommonStatic.LinqQueryToDataTable(query);
            tableLeft = tableGroup.Clone();
            tableRight = tableGroup.Clone();
            int flag = 1;
            //FCommon.GridView_dataImport(gvGroupLeft, tableGroup, poi);
            foreach (DataRow rows in tableGroup.Rows)
            {
                if (flag == 1)
                {
                    tableLeft.ImportRow(rows);
                    flag = 2;
                }
                else
                {
                    tableRight.ImportRow(rows);
                    flag = 1;
                }
                    
            }
            var queryBudge = mpms.TBM1118_1.Where(o => o.OVC_PURCH.Equals(strPurchNum)
                                                         && o.ONB_GROUP_PRE == numGroup).ToList();
            if (queryBudge.Count > 0)
            {
                foreach (var rows in queryBudge)
                {
                    lblGRUOP_BUDGE.Text = rows.ONB_GROUP_BUDGET.ToString();
                }
            }
            else
                lblGRUOP_BUDGE.Text = "0";



            ViewState["hasRowsL"] = FCommon.GridView_dataImport(gvGroupLeft, tableLeft, strField);
            ViewState["hasRowsR"] = FCommon.GridView_dataImport(gvGroupRight, tableRight, strField);
            
        }

        private void pageThirdImport_ICout()
        {
            DataTable dtMain = new DataTable();
            DataTable dtLeft = new DataTable();
            DataTable dtRight = new DataTable();
            string[] strField = { "ONB_POI_ICOUNT", "ONB_GROUP_PRE", "OVC_POI_NSTUFF_CHN", "OVC_BRAND" };
            var query =
                from c in mpms.TBM1201
                join o in mpms.TBM1118_2 on new { c.OVC_PURCH, c.ONB_POI_ICOUNT } equals new { o.OVC_PURCH, o.ONB_POI_ICOUNT }
                into g
                where c.OVC_PURCH.Equals(strPurchNum)
                from o in g.DefaultIfEmpty()
                orderby c.ONB_POI_ICOUNT
                select new
                {
                    ONB_POI_ICOUNT = c.ONB_POI_ICOUNT,
                    ONB_GROUP_PRE =(int?) o.ONB_GROUP_PRE,
                    OVC_POI_NSTUFF_CHN = c.OVC_POI_NSTUFF_CHN,
                    OVC_BRAND = c.OVC_BRAND
                };
            dtMain = CommonStatic.LinqQueryToDataTable(query);
            dtLeft = dtMain.Clone();
            dtRight = dtMain.Clone();
            int flag = 1;
            foreach(DataRow rows in dtMain.Rows)
            {
                if (flag ==1)
                {
                    dtLeft.ImportRow(rows);
                    flag = 2;
                }
                else
                {
                    dtRight.ImportRow(rows);
                    flag = 1;
                }
            }
            FCommon.GridView_dataImport(gvONB_POI_ICOUNT_LEFT, dtLeft, strField);
            FCommon.GridView_dataImport(gvONB_POI_ICOUNT_Right, dtRight, strField);

        }

        private void DrpGroupImport()
        {
            //下拉式選單
            drpGROUP.Items.Clear();
            var querymax = mpms.TBM1118_2.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Any();
            if (querymax)
            {
                var max = mpms.TBM1118_2.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Max(o => o.ONB_GROUP_PRE);
                for (int i = 1; i <= max + 1; i++)
                {
                    drpGROUP.Items.Add(i.ToString());
                }
            }
            else
            {
                drpGROUP.Items.Add("1");
            }
            
        }
        
        private void NewLoginScreen()
        {
            //計畫清單頁籤
            pageSecond.Visible = false;
            divSecContentIN.Visible = false;
            divSecContentOUT.Visible = false;
            //複數決標案頁籤
            pageThird.Visible = false;
            divThird.Visible = false;
            

        }

        private void dataImport_Detail(GridView gv)
        {
            DataTable dt = new DataTable();
            string[] strField_1 = { "ONB_POI_ICOUNT", "NSN", "OVC_POI_NSTUFF_CHN", "ONB_POI_QORDER_PLAN", "ONB_POI_MPRICE_PLAN" };
            var GVContent =
                from table in mpms.TBM1201
                where table.OVC_PURCH.Equals(strPurchNum)
                orderby table.ONB_POI_ICOUNT
                select new
                {
                    table.ONB_POI_ICOUNT,
                    table.NSN,
                    table.OVC_POI_NSTUFF_CHN,
                    table.ONB_POI_QORDER_PLAN,
                    table.ONB_POI_MPRICE_PLAN
                };
            dt = CommonStatic.LinqQueryToDataTable(GVContent);
            ViewState[gv.ID] = FCommon.GridView_dataImport(gv, dt, strField_1);
        }

        private void txtUSEBoxInIt(DataTable dt, TextBox textBox, string ovcIkind)
        {
            //放用途跟外購理由
            var query =
                from tbm in dt.AsEnumerable()
                where tbm.Field<string>("OVC_IKIND").Equals(ovcIkind)
                select tbm.Field<string>("OVC_MEMO");
            foreach (var datarow in query)
            {
                textBox.Text += datarow.ToString();
            }

        }

        private void txtBoxInIt(DataTable dt, TextBox textBox, string ikind)
        {
            //放請求事項跟備考
            DataTable Tbm12201 = new DataTable();
            DataTable Tbm12202 = new DataTable();
            List<string> HasMemo = new List<string>();
            List<string> NotMemo = new List<string>();

            var query =
                from tbm in dt.AsEnumerable()
                where tbm.Field<string>("OVC_IKIND").StartsWith(ikind)
                select new
                {
                    OVC_IKIND = tbm.Field<string>("OVC_IKIND"),
                    OVC_MEMO = tbm.Field<string>("OVC_MEMO")
                };

            Tbm12201 = CommonStatic.LinqQueryToDataTable(query);

            var queryKind =
                from tbm in gm.TBM1220_2
                where tbm.OVC_IKIND.StartsWith(ikind)
                select tbm;
            Tbm12202 = CommonStatic.LinqQueryToDataTable(queryKind);

            
            foreach (DataRow row in Tbm12202.Rows)
            {
                string strtext = "";
               
                foreach (DataRow row2 in Tbm12201.Rows)
                {
                    string strTemp = row2["OVC_MEMO"].ToString();
                    strTemp = strTemp.Replace("<br>", "");
                    row2["OVC_MEMO"] = strTemp;
                    if (row["OVC_IKIND"].ToString().Equals(row2["OVC_IKIND"].ToString()))
                    {
                        strtext = row["OVC_MEMO_NAME"].ToString() + " : " + row2["OVC_MEMO"].ToString();
                        HasMemo.Add(strtext);
                    }
                }
                if (strtext.Equals(string.Empty))
                {
                    strtext = row["OVC_MEMO_NAME"].ToString() + " : (空白)";
                    NotMemo.Add(strtext);
                }
            }
            ViewState[textBox.ID] = HasMemo;
            foreach (string has in HasMemo)
                textBox.Text += has + "\r\n\r\n";
            if (!textBox.Text.Equals(string.Empty))
                textBox.Text += "===============================================\r\n\r\n";
            foreach (string not in NotMemo)
                textBox.Text += not + "\r\n\r\n";

        }

        private void FooterSum(GridView gv)
        {
            int count = 0;
            decimal sum = 0, budget = 0;
            var query = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            if (query.ONB_PUR_BUDGET!=null)
            {
                budget = Convert.ToDecimal(query.ONB_PUR_BUDGET);
            }
            foreach (GridViewRow row in gv.Rows)
            {
                decimal mount = 0, money = 0;
                if (decimal.TryParse(row.Cells[3].Text, out mount) && decimal.TryParse(row.Cells[4].Text, out money))
                {
                    sum += mount * money;
                }
                count++;
            }
            gv.FooterRow.Cells[0].Text = "總項數:" + count;
            gv.FooterRow.Cells[1].ColumnSpan = 2;
            gv.FooterRow.Cells[1].Text = "總預算 : " + budget.ToString("#,0.00");
            gv.FooterRow.Cells[2].Text = "合計總價";
            gv.FooterRow.Cells[3].Text = sum.ToString("#,0.00");
            int cellCount = gv.FooterRow.Cells.Count;
            gv.FooterRow.Cells.RemoveAt(cellCount - 1);
        }

        private void dataImport_1201(string iCount)
        {
            //明細異動按鈕的資料帶入-外購的
            decimal x = decimal.Parse(iCount);
            var query = mpms.TBM1201.Where(table => table.OVC_PURCH.Equals(strPurchNum) & table.ONB_POI_ICOUNT == x).FirstOrDefault();
            //性能和規格在tbm1233
            var query2 = mpms.TBM1233.Where(table => table.OVC_PURCH.Equals(strPurchNum) & table.ONB_POI_ICOUNT == x).FirstOrDefault();
            
            txtOVC_POI_NSTUFF_CHN.Text = query.OVC_POI_NSTUFF_CHN;
            txtOVC_POI_NSTUFF_ENG.Text = query.OVC_POI_NSTUFF_ENG;
            drpNSN_KIND.SelectedValue = query.NSN_KIND;
            txtNSN.Text = query.NSN;
            //注意資料庫內的或同等品格式花樣 
            //ex:1.Karl Storz(或同等品)2.Zenoah（或同等品）3.BioTek﹝或同等品﹞4.或同等品5.(或同等品)6.VIGILEO「或同等品」
            CheckSameQ(query.OVC_BRAND, txtOVC_BRAND, chkOVC_SAME_QUALITY_BRAND);
            CheckSameQ(query.OVC_MODEL, txtOVC_MODEL, chkOVC_SAME_QUALITY_MODEL);
            CheckSameQ(query.OVC_MODEL, txtOVC_POI_IREF, chkOVC_SAME_QUALITY_POI_IREF);
            txtOVC_FCODE.Text = query.OVC_FCODE;
            drpOVC_POI_IUNIT.SelectedValue = query.OVC_POI_IUNIT;
            txtONB_POI_QORDER_PLAN.Text = query.ONB_POI_QORDER_PLAN.ToString();
            txtONB_POI_MPRICE_PLAN.Text = query.ONB_POI_MPRICE_PLAN.ToString();
            drpOVC_FIRST_BUY.SelectedValue = query.OVC_FIRST_BUY;
            if (drpOVC_FIRST_BUY.SelectedValue.Equals("N"))
            {
                txtOVC_POI_IPURCH_BEF.Text = query.OVC_POI_IPURCH_BEF;
                txtONB_POI_QORDER_BEF.Text = query.ONB_POI_QORDER_BEF.ToString();
                //新台幣 vs 新臺幣
                string current = query.OVC_CURR_MPRICE_BEF.Replace("台", "臺");
                drpOVC_CURR_MPRICE_BEF.SelectedItem.Text = current;
                txtONB_POI_MPRICE_BEF.Text = query.ONB_POI_MPRICE_BEF.ToString();
            }
            txtOVC_POI_NDESC.Text = query.OVC_POI_NDESC;
            if(query2 != null)
            {
                txtOVC_INSPECT.Text = query2.OVC_POI_NDESC;
            }
        }

        private void dataImport_1201IN(string iCount)
        {
            //明細異動按鈕的資料帶入-內購的
            decimal x = decimal.Parse(iCount);
            var query = mpms.TBM1201.Where(table => table.OVC_PURCH.Equals(strPurchNum) & table.ONB_POI_ICOUNT == x).FirstOrDefault();
            //性能和規格在tbm1233
            var query2 = mpms.TBM1233.Where(table => table.OVC_PURCH.Equals(strPurchNum) & table.ONB_POI_ICOUNT == x).FirstOrDefault();
            txtOVC_POI_NSTUFF_CHN_IN.Text = query.OVC_POI_NSTUFF_CHN;
            txtOVC_POI_NSTUFF_ENG_IN.Text = query.OVC_POI_NSTUFF_ENG;
            drpNSN_KIND_IN.SelectedValue = query.NSN_KIND;
            txtNSN_IN.Text = query.NSN;
            //注意資料庫內的或同等品格式花樣 
            //ex:1.Karl Storz(或同等品)2.Zenoah（或同等品）3.BioTek﹝或同等品﹞4.或同等品5.(或同等品)6.VIGILEO「或同等品」
            CheckSameQ(query.OVC_BRAND, txtOVC_BRAND_IN, chkOVC_SAME_QUALITY_BRAND_IN);
            CheckSameQ(query.OVC_MODEL, txtOVC_MODEL_IN, chkOVC_SAME_QUALITY_MODEL_IN);
            CheckSameQ(query.OVC_MODEL, txtOVC_POI_IREF_IN, chkOVC_SAME_QUALITY_POI_IREF_IN);

            txtOVC_FCODE_IN.Text = query.OVC_FCODE;

            drpOVC_POI_IUNIT_IN.SelectedValue = query.OVC_POI_IUNIT;
            txtONB_POI_QORDER_PLAN_IN.Text = query.ONB_POI_QORDER_PLAN.ToString();
            txtONB_POI_MPRICE_PLAN_IN.Text = query.ONB_POI_MPRICE_PLAN.ToString();
            drpOVC_FIRST_BUY_IN.SelectedValue = query.OVC_FIRST_BUY.ToString();
            if (query.OVC_FIRST_BUY.ToString().Equals("N"))
            {
                txtOVC_POI_IPURCH_BEF_IN.Text = query.OVC_POI_IPURCH_BEF;
                txtONB_POI_QORDER_BEF_IN.Text = (query.ONB_POI_QORDER_BEF ?? 0).ToString();
                //新台幣 vs 新臺幣
                if (query.OVC_CURR_MPRICE_BEF != null)
                    drpOVC_CURR_MPRICE_BEF_IN.SelectedValue = query.OVC_CURR_MPRICE_BEF.Replace("新台幣", "新臺幣");
                txtONB_POI_MPRICE_BEF_IN.Text = query.ONB_POI_MPRICE_BEF.ToString();
            }
            txtOVC_POI_NDESC_IN.Text = query.OVC_POI_NDESC;
           
            if(query2 != null)
                if (query2.OVC_POI_NDESC != null)
                    txtOVC_INSPECT_IN.Text = query2.OVC_POI_NDESC;

        }

        private void dataSaveGroup(GridView gv, string id)
        {
            //存分組資料到資料庫
            foreach (GridViewRow rows in gv.Rows)
            {
                CheckBox cb = (CheckBox)rows.FindControl(id);
                if (cb.Checked)
                {
                    TBM1118_2 tbm1118_2 = new TBM1118_2();
                    tbm1118_2.OVC_PURCH = strPurchNum;
                    tbm1118_2.ONB_GROUP_PRE = short.Parse(lblONB_GROUP_PRE.Text);
                    tbm1118_2.ONB_POI_ICOUNT = short.Parse(rows.Cells[1].Text);
                    mpms.TBM1118_2.Add(tbm1118_2);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , tbm1118_2.GetType().Name.ToString(), this, "新增");
                }
            }
        }

        private void dataGroupBudget()
        {
            //計算並儲存(刪除)分組預算 tbm1118_1
            short numGroup = short.Parse(lblONB_GROUP_PRE.Text);
            decimal sum = 0;
            var query =
                from table in mpms.TBM1118_2
                join table2 in mpms.TBM1201
                on new { table.OVC_PURCH, table.ONB_POI_ICOUNT } equals new { table2.OVC_PURCH, table2.ONB_POI_ICOUNT }
                where table.OVC_PURCH.Equals(strPurchNum) && table.ONB_GROUP_PRE == numGroup
                select new
                {
                    ONB_POI_MPRICE_PLAN = table2.ONB_POI_MPRICE_PLAN,
                    ONB_POI_QORDER_PLAN = table2.ONB_POI_QORDER_PLAN
                };
            foreach (var rows in query)
            {
                decimal money = (decimal)rows.ONB_POI_MPRICE_PLAN;
                decimal q = (decimal)rows.ONB_POI_QORDER_PLAN;
                sum += money * q;
            }
            if (sum == 0)
            {
                //合計等於零表示要刪除
                TBM1118_1 tbm1118_1 = new TBM1118_1();
                tbm1118_1 = mpms.TBM1118_1.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.ONB_GROUP_PRE == numGroup).FirstOrDefault();
                mpms.Entry(tbm1118_1).State = EntityState.Deleted;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , tbm1118_1.GetType().Name.ToString(), this, "刪除");
            }
            else
            {
                //合計不等於零表示要新增或修改

                var isExist = mpms.TBM1118_1.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.ONB_GROUP_PRE == numGroup).ToList();

                if (isExist.Count == 0)
                {
                    //新增
                    TBM1118_1 tbm1118_1 = new TBM1118_1();
                    tbm1118_1.OVC_PURCH = strPurchNum;
                    tbm1118_1.ONB_GROUP_PRE = numGroup;
                    tbm1118_1.ONB_GROUP_BUDGET = sum;
                    mpms.TBM1118_1.Add(tbm1118_1);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                                , tbm1118_1.GetType().Name.ToString(), this, "新增");
                }
                else
                {
                    //修改
                    TBM1118_1 tbm1118_1 = new TBM1118_1();
                    tbm1118_1 = mpms.TBM1118_1.Where(o => o.OVC_PURCH.Equals(strPurchNum)
                                                       && o.ONB_GROUP_PRE == numGroup).FirstOrDefault();
                    tbm1118_1.OVC_PURCH = strPurchNum;
                    tbm1118_1.ONB_GROUP_PRE = numGroup;
                    tbm1118_1.ONB_GROUP_BUDGET = sum;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                                , tbm1118_1.GetType().Name.ToString(), this, "修改");
                }
            }

        }

        private void SelectAll(GridView gv, string id)
        {
            foreach (GridViewRow rows in gv.Rows)
            {
                if (rows.Cells[2].Text.Equals("&nbsp;"))
                {
                    CheckBox cb = (CheckBox)rows.FindControl(id);
                    cb.Checked = true;
                }
            }
        }

        private void ResetAll(GridView gv, string id)
        {
            foreach (GridViewRow rows in gv.Rows)
            {
                CheckBox cb = (CheckBox)rows.FindControl(id);
                cb.Checked = false;
            }
        }

        private void CheckSameQ(string check,TextBox textBox, CheckBox checkBox)
        {
            if (!string.IsNullOrEmpty(check))
            {
                //顯示的時候拿掉或同等品 並將checkbox打勾 注意純"或同等品"必須在最下面
                if (check.Contains("或同等品"))
                {
                    string convert = check.Replace("(或同等品)", string.Empty);
                    convert = convert.Replace("（或同等品）", string.Empty);
                    convert = convert.Replace("﹝或同等品﹞", string.Empty);
                    convert = convert.Replace("「或同等品」", string.Empty);
                    convert = convert.Replace("或同等品", string.Empty);
                    textBox.Text = convert;
                    checkBox.Checked = true;
                }
                else
                {
                    textBox.Text = check;
                }
            }
        }

        #region PDF列印-物資申請書
        private void PrintPDF(string angcy)
        {
            var query1407 = gm.TBM1407.Where(o => o.OVC_PHR_CATE.Equals("B0"));
            var t1301 =
                (from t in gm.TBM1301
                 join t1407 in query1407 on t.OVC_PUR_CURRENT equals t1407.OVC_PHR_ID into ps
                 from t1407 in ps.DefaultIfEmpty()
                 where t.OVC_PURCH.Equals(strPurchNum)
                 select new
                 {
                     t.OVC_PUR_AGENCY,
                     t.OVC_PUR_NSECTION,
                     t.OVC_PUR_DAPPROVE,
                     t.OVC_PUR_APPROVE,
                     t.OVC_DPROPOSE,
                     t.OVC_PROPOSE,
                     t.OVC_PUR_IPURCH,
                     OVC_PUR_CURRENT = t1407 != null ? t1407.OVC_PHR_DESC : "",
                     ONB_PUR_BUDGET = t.ONB_PUR_BUDGET ?? 0,
                     t.OVC_PUR_SECTION,
                     t.OVC_SECTION_CHIEF,
                     t.OVC_AGNT_IN
                 }).FirstOrDefault();
            if(t1301 != null)
            {
                var tablePurch = mpms.TBMPURCH_EXT.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                //PDF列印
                var doc1 = new Document(PageSize.A4, 50, 50, 45, 50);
                MemoryStream Memory = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
                Watermark wm = new Watermark();
                wm.strPurchNum = strPurchNum;
                writer.PageEvent = wm;
                BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H
                   , BaseFont.NOT_EMBEDDED);//設定字型
                Font ChFont = new Font(bfChinese, 12, Font.NORMAL, new BaseColor(0, 0, 0));
                Font ChFont_blue = new Font(bfChinese, 12, Font.NORMAL, new BaseColor(2, 0, 255));
                Font ChFont_msg = new Font(bfChinese, 12, Font.ITALIC, BaseColor.RED);
                Font ChFont_memo = new Font(bfChinese, 8, Font.NORMAL, new BaseColor(0, 0, 0));
                doc1.Open();

                //page1
                #region Page1
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
                string strp1 = nsection + angcy + "物資申請書存根";
                Paragraph p = new Paragraph(strp1, ChFont);
                p.Add(new Chunk(glue));//有沒有其他寫法  無法垂直置中
                string strp2 = "中華民國" + getTaiwanDate(t1301.OVC_PUR_DAPPROVE, "chDate") + "\n";
                p.Add(strp2);//核定日期
                p.Add(new Chunk(glue));
                p.Add(lblOVC_PUR_APPROVE.Text);//核定文號
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
                PdfPCell cell2_3 = new PdfPCell(new Phrase("申購日期及文號", ChFont));
                cell2_3.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell2_3.HorizontalAlignment = 1;
                table.AddCell(cell2_3);
                string strPropose = "中華民國" + getTaiwanDate(t1301.OVC_DPROPOSE, "chDate") + "\n" + t1301.OVC_PROPOSE;
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
                PdfPCell cell3_6 = new PdfPCell(new Phrase(drp.SelectedItem.ToString(), ChFont));
                cell3_6.HorizontalAlignment = 1;
                cell3_6.VerticalAlignment = Element.ALIGN_MIDDLE;
                table2.AddCell(cell3_6);
                PdfPCell cell3_7 = new PdfPCell(new Phrase("預算來源", ChFont));
                cell3_7.Colspan = 2;
                cell3_7.HorizontalAlignment = 1;
                cell3_7.VerticalAlignment = Element.ALIGN_MIDDLE;
                table2.AddCell(cell3_7);
                PdfPCell cell4_1 = new PdfPCell(new Phrase(txtOVC_PUR_IPURCH.Text + "，詳清單", ChFont));
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
                PdfPCell cell6_1 = new PdfPCell(new Phrase(drp.SelectedItem.Text, ChFont));
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
                    int postion = item.title.IndexOf(')');
                    string strNum = item.title.Substring(0, postion + 1);
                    Phrase c7_2 = new Phrase(strNum + item.memo + "\n", ChFont_memo);
                    cell7_2.AddElement(c7_2);
                }
                cell7_2.Colspan = 7;
                table2.AddCell(cell7_2);

                var query1301 = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                string toPhares = "";
                if (Session["userid"] != null)
                {
                    string strUSER_ID = Session["userid"].ToString();
                    string userid = Session["userid"].ToString();
                    var queryUserDept = gm.ACCOUNTs.Where(o => o.USER_ID.Equals(userid)).FirstOrDefault();
                    if (queryUserDept.DEPT_SN.ToString() == query1301.OVC_PUR_SECTION.ToString())
                        toPhares = "此致";
                    else
                        toPhares = "謹呈";
                }
                PdfPCell cell10 = new PdfPCell();
                Chunk glue2 = new Chunk(new VerticalPositionMark());
                Paragraph p2 = new Paragraph(toPhares + "\n", ChFont);
                p2.FirstLineIndent = 25;
                p2.Add(query1301.OVC_PUR_NSECTION);
                p2.Add(new Chunk(glue2));
                p2.Add("主官" + txtOVC_SECTION_CHIEF.Text);//主官
                cell10.AddElement(p2);
                cell10.Colspan = 8;
                table2.AddCell(cell10);
                PdfPCell cell11_1 = new PdfPCell(new Phrase("審查意見", ChFont));
                cell11_1.Colspan = 6;
                cell11_1.HorizontalAlignment = Element.ALIGN_CENTER;
                table2.AddCell(cell11_1);
                Paragraph p11_2 = new Paragraph();
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

                //page2
                #region Page2
                doc1.NewPage();
                table = new PdfPTable(new float[] { 1, 3, 1, 1, 1, 3, 2, 4 });

                table.TotalWidth = 500f;
                table.LockedWidth = true;

                header = new PdfPCell();
                glue = new Chunk(new VerticalPositionMark());
                p = new Paragraph(nsection + angcy + "物資申請書", ChFont);//標題
                p.Add(new Chunk(glue));//有沒有其他寫法  無法垂直置中
                p.Add(strp2);
                p.Add(new Chunk(glue));
                p.Add(lblOVC_PUR_APPROVE.Text);
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
                    int postion = item.title.IndexOf(')');
                    string strNum = item.title.Substring(0, postion + 1);
                    Phrase c9_2 = new Phrase(strNum + item.memo + "\n", ChFont_memo);
                    cell9_2.AddElement(c9_2);
                }
                cell9_2.Colspan = 7;
                table2.AddCell(cell9_2);

                table2.AddCell(cell10);
                table2.AddCell(cell11_1);
                table2.AddCell(cell11_2);
                table2.AddCell(cell12_1);
                table2.AddCell(cell12_2);
                table2.AddCell(cell12_3);
                table2.AddCell(cell13_1);
                table2.AddCell(cell13_2);
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
                p = new Paragraph(nsection + angcy + "物資核定書", ChFont);//標題
                p.Add(new Chunk(glue));//有沒有其他寫法  無法垂直置中
                p.Add(strp2);
                p.Add(new Chunk(glue));
                p.Add(lblOVC_PUR_APPROVE.Text);
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
                var queryAgntIn = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.OVC_AGNT_IN).FirstOrDefault();
                p2 = new Paragraph(new Phrase("此令\n", ChFont));
                p2.FirstLineIndent = -5;
                p2.Add(queryAgntIn + "                  ");//空格是否有更好的方法
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
                p = new Paragraph(nsection + angcy + "物資核定書", ChFont);//標題
                p.Add(new Chunk(glue));//有沒有其他寫法  無法垂直置中
                p.Add(strp2);
                p.Add(new Chunk(glue));
                p.Add(lblOVC_PUR_APPROVE.Text);
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
                p2 = new Paragraph(new Phrase("此令\n", ChFont));
                p2.FirstLineIndent = -5;
                p2.Add(txtOVC_PUR_NSECTION.Text + "                  ");//空格是否有更好的方法
                p2.IndentationLeft = 30;
                p2.Add("主官");
                cell11_3.AddElement(p2);
                cell11_3.Colspan = 8;
                cell11_3.PaddingTop = -4;
                cell11_3.FixedHeight = 80;
                table2.AddCell(cell11_3);
                doc1.Add(table2);
                #endregion

                doc1.Close();
                Response.Clear();//瀏覽器上顯示
                Response.AddHeader("Content-disposition", "attachment;filename=" + strPurchNum + "物資申請書.pdf");
                Response.ContentType = "application/octet-stream";
                Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.Flush();
                Response.End();
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無此筆購案資料");
            }
        }

        #endregion

        #region PDF列印-新物資申請書
        private void PrintNewPDF()
        {
            //PDF列印
            var doc1 = new Document(PageSize.A4, 50, 50, 45, 50);
            MemoryStream Memory = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            Watermark wm = new Watermark();
            wm.strPurchNum = strPurchNum;
            writer.PageEvent = wm;
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H
               , BaseFont.NOT_EMBEDDED);//設定字型
            iTextSharp.text.Font ChFont = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            iTextSharp.text.Font ChFont_blue = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.NORMAL, new BaseColor(2, 0, 255));
            iTextSharp.text.Font ChFont_msg = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.ITALIC, BaseColor.RED);
            iTextSharp.text.Font ChFont_memo = new iTextSharp.text.Font(bfChinese, 8, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            doc1.Open();

            //page1
            #region Page1
            string angcy = "";
            var query1407 = gm.TBM1407.Where(o => o.OVC_PHR_CATE.Equals("B0"));
            var t1301 =
                (from t in gm.TBM1301
                 join t1407 in query1407 on t.OVC_PUR_CURRENT equals t1407.OVC_PHR_ID into ps
                 from t1407 in ps.DefaultIfEmpty()
                 where t.OVC_PURCH.Equals(strPurchNum)
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
                     OVC_PUR_CURRENT = t1407 != null ? t1407.OVC_PHR_DESC:"",
                     ONB_PUR_BUDGET  = t.ONB_PUR_BUDGET ?? 0,
                     t.OVC_PUR_SECTION,
                     t.OVC_SECTION_CHIEF,
                     t.OVC_AGNT_IN,
                     t.OVC_SUPERIOR_UNIT
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
            string strp1 = t1301.OVC_PUR_NSECTION + angcy + "物資申請書存根";
            iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(strp1, ChFont);
            p.Add(new Chunk(glue));//有沒有其他寫法  無法垂直置中
            string strp2 = "中華民國" + getTaiwanDate(t1301.OVC_PUR_DAPPROVE, "chDate") + "\n";
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
            PdfPCell cell2_3 = new PdfPCell(new Phrase("申購日期及文號", ChFont));
            cell2_3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2_3.HorizontalAlignment = 1;
            table.AddCell(cell2_3);
            string strPropose = "中華民國" + getTaiwanDate(t1301.OVC_DPROPOSE, "chDate") + "\n" + t1301.OVC_PROPOSE;
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
            PdfPCell cell3_4 = new PdfPCell(new Phrase("數量", ChFont));
            cell3_4.HorizontalAlignment = 1;
            cell3_4.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell3_4);
            PdfPCell cell3_5 = new PdfPCell(new Phrase("單價", ChFont));
            cell3_5.HorizontalAlignment = 1;
            cell3_5.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell3_5);
            PdfPCell cell3_6 = new PdfPCell(new Phrase("總價", ChFont));
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
            PdfPCell cell6_1 = new PdfPCell(new Phrase(drp.SelectedItem.Text, ChFont));
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

            PdfPCell cell10 = new PdfPCell();
            Chunk glue2 = new Chunk(new VerticalPositionMark());
            iTextSharp.text.Paragraph p2 = new iTextSharp.text.Paragraph("謹呈\n", ChFont);
            p2.FirstLineIndent = 25;
            p2.Add(t1301.OVC_SUPERIOR_UNIT);
            p2.Add(new Chunk(glue2));
            p2.Add("主官 " + t1301.OVC_SECTION_CHIEF);//主官
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

            //page2
            #region Page2
            doc1.NewPage();
            table = new PdfPTable(new float[] { 1, 3, 1, 1, 1, 3, 2, 4 });

            table.TotalWidth = 500f;
            table.LockedWidth = true;

            header = new PdfPCell();
            glue = new Chunk(new VerticalPositionMark());
            string strUnit;
            var querynsection2 = mpms.TBMPURCH_EXT.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.OVC_PUR_NSECTION_2).FirstOrDefault();
            if (querynsection2 != null)
            {
                strUnit = querynsection2;
            }
            else
            {
                var query1202Dept =
                    (from t in mpms.TBM1202.AsEnumerable()
                     join t2 in gm.TBMDEPTs on t.OVC_CHECK_UNIT equals t2.OVC_DEPT_CDE
                     where t.OVC_PURCH.Equals(strPurchNum)
                     orderby t.OVC_DRECEIVE
                     select t2.OVC_ONNAME).FirstOrDefault();
                strUnit = query1202Dept;
            }

            p = new iTextSharp.text.Paragraph(strUnit + angcy + "物資申請書", ChFont);//標題
            p.Add(new Chunk(glue));//有沒有其他寫法  無法垂直置中
            //日期及文號-TBMOVERSEE_LOG
            string logApply;
            var querylogApply =
                mpms.TBMOVERSEE_LOG.Where(o => o.OVC_PURCH.Equals(strPurchNum))
                                   .OrderBy(o => o.OVC_DATE)
                                   .Select(o => new { o.OVC_DAPPLY, o.OVC_APPLY_NO, o.OVC_APPLY_CHIEF })
                                   .FirstOrDefault();
            logApply = querylogApply == null ? "中華民國" : "中華民國" + getTaiwanDate(querylogApply.OVC_DAPPLY, "chDate") + "\n";
            p.Add(logApply);
            p.Add(new Chunk(glue));
            if (querylogApply != null) p.Add(querylogApply.OVC_APPLY_NO);
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

            PdfPCell cell10_2 = new PdfPCell();
            Chunk glue2_2 = new Chunk(new VerticalPositionMark());
            iTextSharp.text.Paragraph p2_2 = new iTextSharp.text.Paragraph("謹呈\n", ChFont);
            p2_2.FirstLineIndent = 25;
            p2_2.Add(t1301.OVC_SUPERIOR_UNIT);
            p2_2.Add(new Chunk(glue2_2));
            if (querylogApply != null) p2_2.Add("主官 " + querylogApply.OVC_APPLY_CHIEF);//主官
            cell10_2.AddElement(p2);
            cell10_2.Colspan = 8;
            table2.AddCell(cell10_2);
            table2.AddCell(cell11_1);
            table2.AddCell(cell11_2);
            table2.AddCell(cell12_1);
            table2.AddCell(cell12_2);
            table2.AddCell(cell12_3);
            table2.AddCell(cell13_1);
            table2.AddCell(cell13_2);
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
            p = new iTextSharp.text.Paragraph(t1301.OVC_SUPERIOR_UNIT + angcy + "物資核定書", ChFont);//標題
            p.Add(new Chunk(glue));//有沒有其他寫法  無法垂直置中
            p.Add(strp2);
            p.Add(new Chunk(glue));
            p.Add(t1301.OVC_PUR_APPROVE);
            p.Alignment = Element.ALIGN_MIDDLE;
            header.AddElement(p);
            header.Colspan = 8;
            table.AddCell(header);
            table.AddCell(cell2_1);
            var queryAgntIn = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.OVC_AGNT_IN).FirstOrDefault();
            PdfPCell cell2_2_2 = new PdfPCell(new Phrase(queryAgntIn, ChFont));
            cell2_2_2.Colspan = 4;
            cell2_2_2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell2_2_2);
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

            PdfPCell cell10_1 = new PdfPCell(new Phrase("審查意見", ChFont));
            table2.AddCell(cell10_1);
            var queryComment = mpms.TBM1202.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_CHECK_OK.Equals("Y")).Select(o => o.OVC_APPROVE_COMMENT).FirstOrDefault();
            PdfPCell cell10_3 = new PdfPCell(new Phrase(queryComment, ChFont_memo));
            cell10_3.Colspan = 7;
            table2.AddCell(cell10_3);
            PdfPCell cell11 = new PdfPCell();

            p2 = new iTextSharp.text.Paragraph(new Phrase("此令\n", ChFont));
            p2.FirstLineIndent = -5;
            p2.Add(queryAgntIn + "                  ");//空格是否有更好的方法
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
            p = new iTextSharp.text.Paragraph(t1301.OVC_SUPERIOR_UNIT + angcy + "物資核定書", ChFont);//標題
            p.Add(new Chunk(glue));//有沒有其他寫法  無法垂直置中
            p.Add(strp2);
            p.Add(new Chunk(glue));
            p.Add(t1301.OVC_PUR_APPROVE);
            p.Alignment = Element.ALIGN_MIDDLE;
            header.AddElement(p);
            header.Colspan = 8;
            table.AddCell(header);
            table.AddCell(cell2_1);
            table.AddCell(cell2_2_2);
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
            table2.AddCell(cell10_1);
            table2.AddCell(cell10_3);
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

            doc1.Close();
            Response.Clear();//瀏覽器上顯示
            Response.AddHeader("Content-disposition", "attachment;filename=" + strPurchNum + "新物資申請書.pdf");
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.Flush();
            Response.End();
        }
        #endregion

        #region PDF列印-內購計畫清單
        public void PrintListINPDF()
        {
            var q1407 = gm.TBM1407.Where(o => o.OVC_PHR_CATE.Equals("Q8"));
            var query1301 =
               (from t in gm.TBM1301
                where t.OVC_PURCH.Equals(strPurchNum)
                join t1407 in q1407 on t.OVC_PUR_NPURCH equals t1407.OVC_PHR_ID into ps
                from t1407 in ps.DefaultIfEmpty()
                select new
                {
                    t.OVC_PURCH_KIND,
                    OVC_PUR_NSECTION = t.OVC_PUR_NSECTION ?? "",
                    OVC_DPROPOSE = t.OVC_DPROPOSE ?? "",
                    OVC_PROPOSE = t.OVC_PROPOSE ?? "",
                    OVC_PUR_NPURCH = t1407 != null ? t1407.OVC_PHR_DESC : "",
                    OVC_AGNT_IN = t.OVC_AGNT_IN ?? "",
                    OVC_RECEIVE_NSECTION = t.OVC_RECEIVE_NSECTION ?? "",
                    OVC_SHIP_TIMES = t.OVC_SHIP_TIMES ?? "",
                    OVC_RECEIVE_PLACE = t.OVC_RECEIVE_PLACE ?? "",
                    OVC_POI_IINSPECT = t.OVC_POI_IINSPECT ?? "",
                    t.OVC_PURCH,
                    t.OVC_PUR_AGENCY
                }).FirstOrDefault();

            var query1231 = mpms.TBM1231.Where(o => o.OVC_PURCH.Equals(strPurchNum)).ToList();
            var source1231 =
                from t in query1231
                group t by t.OVC_PURCH into g
                select new
                {
                    g.Key,
                    ISOURCE = string.Join(";", g.Select(o => o.OVC_ISOURCE))
                };
            string isource = "";
            if (source1231.Any())
            {
                isource = source1231.FirstOrDefault().ISOURCE;
            }
            var query1231APPROVE = mpms.TBM1231.Where(o => o.OVC_PURCH.Equals(strPurchNum));
            string AppDate = "";
            foreach (var item in query1231APPROVE)
                AppDate += getTaiwanDate(item.OVC_PUR_DAPPR_PLAN,"chDate") + "\t" + item.OVC_PUR_APPR_PLAN + "\r\n";

            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChFont = new Font(bfChinese, 12f);
            Font SmailChFont = new Font(bfChinese, 10f);
            Font SmailblueChFont = new Font(bfChinese, 10f, Font.NORMAL, new BaseColor(00, 0, 255));
            Font Underline = new Font(bfChinese, 12f, Font.UNDERLINE);
            Font titleFont = new Font(bfChinese, 18f);
            var doc1 = new Document(PageSize.A4, 40, 50, 140, 55);
            MemoryStream Memory = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            WatermarkList watermarkList = new WatermarkList();
            watermarkList.strPurchNum = strPurchNum;
            writer.PageEvent = watermarkList;
            doc1.Open();
            DateTime PrintTime = DateTime.Now;

            PdfPTable firsttable = new PdfPTable(4);
            firsttable.SetWidths(new float[] { 3, 4, 2, 3 });
            firsttable.DefaultCell.Border = Rectangle.NO_BORDER;
            firsttable.TotalWidth = 560F;
            firsttable.LockedWidth = true;
            firsttable.DefaultCell.SetLeading(1.2f, 1.2f);
            
            firsttable.AddCell(new Phrase("(一)軍品類別:", ChFont));
            firsttable.AddCell(new Phrase(query1301.OVC_PUR_NPURCH, Underline));
            firsttable.AddCell(new Phrase("(六)購案編號:", ChFont));
            var angcy = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.OVC_PUR_AGENCY).FirstOrDefault();
            firsttable.AddCell(new Phrase(strPurchNum + angcy, Underline));

            firsttable.AddCell(new Phrase("(二)預算來源:", ChFont));
            firsttable.AddCell(new Phrase(isource, Underline));
            firsttable.AddCell(new Phrase("(七)接收單位:", ChFont));
            firsttable.AddCell(new Phrase(query1301.OVC_RECEIVE_NSECTION, Underline));

            firsttable.AddCell(new Phrase("(三)預算奉准文號及日期:", SmailChFont));
            firsttable.AddCell(new Phrase(AppDate, Underline));
            firsttable.AddCell(new Phrase("(八)交貨期間:", ChFont));
            firsttable.AddCell(new Phrase(query1301.OVC_SHIP_TIMES, Underline));

            firsttable.AddCell(new Phrase("(四)原計畫申購單位:", ChFont));
            firsttable.AddCell(new Phrase(query1301.OVC_PUR_NSECTION, Underline));
            firsttable.AddCell(new Phrase("(九)交貨地點:", ChFont));
            firsttable.AddCell(new Phrase(query1301.OVC_RECEIVE_PLACE, Underline));

            firsttable.AddCell(new Phrase("(五)採購單位:", ChFont));
            firsttable.AddCell(new Phrase(query1301.OVC_AGNT_IN, Underline));
            firsttable.AddCell(new Phrase("(十)檢驗方法:", ChFont));
            firsttable.AddCell(new Phrase(query1301.OVC_POI_IINSPECT, Underline));
            firsttable.DefaultCell.FixedHeight = 20f;
            firsttable.AddCell(new Phrase(" ", Underline));
            firsttable.AddCell(new Phrase(" ", Underline));
            firsttable.AddCell(new Phrase(" ", Underline));
            firsttable.AddCell(new Phrase(" ", Underline));
            doc1.Add(firsttable);


            PdfPTable secondtable = new PdfPTable(7);
            secondtable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            secondtable.SetWidths(new float[] { 1, 5, 1, 2, 2, 2, 2 });
            secondtable.TotalWidth = 560F;
            secondtable.LockedWidth = true;
            secondtable.AddCell(new Phrase("(十一) 項次", SmailChFont));
            secondtable.AddCell(new Phrase("(十二)品名料號及規格", SmailChFont));
            secondtable.AddCell(new Phrase("(十三) 單位", SmailChFont));
            secondtable.AddCell(new Phrase("(十四)數量", SmailChFont));
            secondtable.AddCell(new Phrase("(十五)單價", SmailChFont));
            secondtable.AddCell(new Phrase("(十六)總價", SmailChFont));
            secondtable.AddCell(new Phrase("(十七)備考      (以往購價)", SmailChFont));

            var query1201 = mpms.TBM1201.Where(o => o.OVC_PURCH.Equals(strPurchNum)).ToArray();
            var queryitem =
                from t in query1201
                join t2 in gm.TBM1407 on t.OVC_POI_IUNIT equals t2.OVC_PHR_ID
                where t2.OVC_PHR_CATE.Equals("J1")
                orderby t.ONB_POI_ICOUNT
                select new
                {
                    t2.OVC_PHR_ID,
                    t.ONB_POI_ICOUNT,
                    t.OVC_POI_NSTUFF_CHN,
                    NSN = t.NSN ?? "(空白)",
                    OVC_BRAND = t.OVC_BRAND ?? "(空白)",
                    OVC_MODEL = t.OVC_MODEL ?? "(空白)",
                    OVC_POI_IREF = t.OVC_POI_IREF ?? "(空白)",
                    OVC_FCODE = t.OVC_FCODE ?? "(空白)",
                    OVC_POI_IUNIT = t2.OVC_PHR_DESC,
                    t.ONB_POI_QORDER_PLAN,
                    t.ONB_POI_MPRICE_PLAN,
                    ONB_PRICE = t.ONB_POI_QORDER_PLAN * t.ONB_POI_MPRICE_PLAN,
                    t.OVC_POI_IPURCH_BEF,
                    t.OVC_CURR_MPRICE_BEF,
                    t.ONB_POI_QORDER_BEF,
                    t.ONB_POI_MPRICE_BEF,
                };
            decimal money = 0;
            var q1231 =
                from t in mpms.TBM1231
                join t1407 in mpms.TBM1407 on t.OVC_CURRENT equals t1407.OVC_PHR_ID
                where t.OVC_PURCH.Equals(strPurchNum) && t1407.OVC_PHR_CATE.Equals("B0")
                select t1407.OVC_PHR_DESC;
            string strCurrent = "";
            if (q1231.Any())
            {
                strCurrent = q1231.FirstOrDefault();
            }
            foreach (var row in queryitem)
            {
                money += (decimal)row.ONB_POI_QORDER_PLAN * (decimal)row.ONB_POI_MPRICE_PLAN;
                secondtable.AddCell(new Phrase(row.ONB_POI_ICOUNT.ToString(), ChFont));
                PdfPCell content = new PdfPCell(new Phrase("  ", ChFont));
                Phrase p1 = new Phrase(row.OVC_POI_NSTUFF_CHN, ChFont);
                Phrase p2 = new Phrase("料號：" + row.NSN, ChFont);
                Phrase p3 = new Phrase("廠牌：" + row.OVC_BRAND, ChFont);
                Phrase p4 = new Phrase("型號：" + row.OVC_MODEL, ChFont);
                Phrase p5 = new Phrase("件號：" + row.OVC_POI_IREF, ChFont);
                Phrase p6 = new Phrase("行政院財產分類編號：" + row.OVC_FCODE, ChFont);
                content.AddElement(p1);
                content.AddElement(p2);
                content.AddElement(p3);
                content.AddElement(p4);
                content.AddElement(p5);
                content.AddElement(p6);
                secondtable.AddCell(content);
                secondtable.AddCell(new Phrase(row.OVC_PHR_ID +"(" + row.OVC_POI_IUNIT+")", ChFont));
                secondtable.AddCell(new Phrase(row.ONB_POI_QORDER_PLAN.ToString(), ChFont));
                secondtable.AddCell(new Phrase(((decimal)row.ONB_POI_MPRICE_PLAN).ToString("#,0.00"), ChFont));
                secondtable.AddCell(new Phrase(((decimal)row.ONB_PRICE).ToString("#,0.00"), ChFont));
                string reMark ="";
                if (row.OVC_POI_IPURCH_BEF != null)
                    reMark += "案號：" + row.OVC_POI_IPURCH_BEF;
                if(row.OVC_CURR_MPRICE_BEF != null && row.ONB_POI_MPRICE_BEF != null && row.ONB_POI_QORDER_BEF != null)
                {
                    if (row.ONB_POI_MPRICE_BEF != 0 && row.ONB_POI_QORDER_BEF != 0)
                    {
                        reMark += " " + row.OVC_CURR_MPRICE_BEF;
                        reMark += " 價格：" + ((decimal)row.ONB_POI_MPRICE_BEF).ToString("#,0.00");
                        reMark += " 數量：" + row.ONB_POI_QORDER_BEF.ToString();
                    }
                }
                   

                secondtable.AddCell(new Phrase(reMark, ChFont));
            }
            doc1.Add(secondtable);

            string ovcIkind = "";
            switch (ViewState["OVC_PUR_AGENCY"].ToString())
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

            var query12201 = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum));
            var query =
                from t in gm.TBM1220_2.AsEnumerable()
                join t2 in query12201 on t.OVC_IKIND equals t2.OVC_IKIND into g
                from t2 in g.DefaultIfEmpty()
                where t.OVC_IKIND.StartsWith(ovcIkind)
                orderby t.OVC_IKIND
                select new
                {
                    t.OVC_IKIND,
                    t.OVC_MEMO_NAME,
                    OVC_MEMO = t2 != null ? t2.OVC_MEMO : "(空白)",
                    OVC_STANDARD = t2 != null ? t2.OVC_STANDARD : "(空白)"
                };

            var queryGroup =
                from t in query
                orderby t.OVC_IKIND
                group t by t.OVC_MEMO_NAME into g
                select g;

            PdfPTable finaltable = new PdfPTable(1);
            finaltable.SetWidths(new float[] { 1 });
            finaltable.TotalWidth = 560F;
            finaltable.LockedWidth = true;
            finaltable.SplitLate = false;
            finaltable.TableEvent = new TopBottomTableBorderMaker(BaseColor.BLACK, 0.5f);
            PdfPCell content3 = new PdfPCell(new Phrase("  ", ChFont));
            content3.Border = 0;
            content3.BorderWidthLeft = .5f;
            content3.BorderWidthRight = .5f;
            content3.AddElement(new Phrase("(十八)備註", ChFont));
            finaltable.AddCell(content3);

            foreach (var row in queryGroup)
            {
                PdfPCell contentMemo = new PdfPCell(new Phrase("  ", ChFont));
                contentMemo.AddElement(new Phrase(row.Key + "：", SmailChFont));

                foreach (var item in row)
                {
                    string memo = item.OVC_MEMO.Replace("<br>", "");
                    if (item.OVC_STANDARD.Equals("Y"))
                        contentMemo.AddElement(new Phrase(memo, SmailChFont));
                    else
                        contentMemo.AddElement(new Phrase(memo, SmailblueChFont));
                }
                contentMemo.Border = 0;
                contentMemo.BorderWidthLeft = .5f;
                contentMemo.BorderWidthRight = .5f;
                finaltable.AddCell(contentMemo);
            }
            
            PdfPCell content4 = new PdfPCell(new Phrase("  ", ChFont));
            content4.AddElement(new Phrase(" ", ChFont));
            content4.AddElement(new Phrase("(十九)" + drp.SelectedItem.ToString() + ":" + q1231.FirstOrDefault() + money.ToString("#,0.00") + "元整（含稅）", ChFont));

            var query12201D20 = mpms.TBM1220_1.Where(o => o.OVC_IKIND.Equals("D20")).Select(o => o.OVC_MEMO).FirstOrDefault();

            content4.AddElement(new Phrase("(二十)" + query12201D20, ChFont));
            content4.AddElement(new Phrase("(二十一)附件：" + GetAttached("M"), ChFont));
            content4.AddElement(new Phrase(" ", ChFont));
            content4.Border = 0;
            content4.BorderWidthLeft = .5f;
            content4.BorderWidthRight = .5f;
            finaltable.AddCell(content4);
            doc1.Add(finaltable);

            PdfPTable timetable = new PdfPTable(1);
            timetable.SetWidths(new float[] { 1 });
            timetable.DefaultCell.Border = Rectangle.NO_BORDER;
            timetable.TotalWidth = 560F;
            timetable.LockedWidth = true;
            timetable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            timetable.AddCell(new Phrase(PrintTime.ToString("yyyy/MM/dd HH:mm:ss"), ChFont));
            doc1.Add(timetable);


            doc1.Close();
            Response.Clear();//瀏覽器上顯示
            Response.AddHeader("Content-disposition", "attachment;filename=" + strPurchNum+ "計畫清單.pdf");
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.Flush();
            Response.End();

        }

        #endregion

        #region PDF列印-外購計畫清單
        public void PrintListOutPDF()
        {
            var q1407 = gm.TBM1407.Where(o => o.OVC_PHR_CATE.Equals("Q8"));
            var query1301 =
               (from t in gm.TBM1301
                where t.OVC_PURCH.Equals(strPurchNum)
                join t1407 in q1407 on t.OVC_PUR_NPURCH equals t1407.OVC_PHR_ID into ps
                from t1407 in ps.DefaultIfEmpty()
                select new
                {
                    t.OVC_PURCH_KIND,
                    OVC_PUR_NSECTION = t.OVC_PUR_NSECTION ?? "",
                    OVC_DPROPOSE = t.OVC_DPROPOSE ?? "",
                    OVC_PROPOSE = t.OVC_PROPOSE ?? "",
                    OVC_PUR_NPURCH = t1407 != null ? t1407.OVC_PHR_DESC : "",
                    OVC_AGNT_IN = t.OVC_AGNT_IN ?? "",
                    OVC_RECEIVE_NSECTION = t.OVC_RECEIVE_NSECTION ?? "",
                    OVC_SHIP_TIMES = t.OVC_SHIP_TIMES ?? "",
                    OVC_RECEIVE_PLACE = t.OVC_RECEIVE_PLACE ?? "",
                    OVC_POI_IINSPECT = t.OVC_POI_IINSPECT ?? "",
                    OVC_TARGET_DO = t.OVC_TARGET_DO ?? "",
                    OVC_WAY_TRANS = t.OVC_WAY_TRANS ?? "",
                    OVC_FROM_TO = t.OVC_FROM_TO ?? "",
                    OVC_TO_PLACE = t.OVC_TO_PLACE ?? "",
                    t.OVC_PURCH,
                    t.OVC_PUR_AGENCY
                }).FirstOrDefault();

            var query1231 = mpms.TBM1231.Where(o => o.OVC_PURCH.Equals(strPurchNum)).ToList();
            var source1231 =
                from t in query1231
                group t by t.OVC_PURCH into g
                select new
                {
                    g.Key,
                    ISOURCE = string.Join(";", g.Select(o => o.OVC_ISOURCE))
                };
            string isource = "";
            if (source1231.Any())
            {
                isource = source1231.FirstOrDefault().ISOURCE;
            }
            var query1231APPROVE = mpms.TBM1231.Where(o => o.OVC_PURCH.Equals(strPurchNum));
            string AppDate = "";
            foreach (var item in query1231APPROVE)
                AppDate += getTaiwanDate(item.OVC_PUR_DAPPR_PLAN, "chDate") + "\t" + item.OVC_PUR_APPR_PLAN + "\r\n";
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChFont = new Font(bfChinese, 12f);
            Font SmailChFont = new Font(bfChinese, 10f);
            Font SmailblueChFont = new Font(bfChinese, 10f, Font.NORMAL, new BaseColor(00, 0, 255));
            Font Underline = new Font(bfChinese, 12f, Font.UNDERLINE);
            Font titleFont = new Font(bfChinese, 18f);
            var doc1 = new Document(PageSize.A4, 40, 50, 140, 55);
            MemoryStream Memory = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            WatermarkList watermarkList = new WatermarkList();
            watermarkList.strPurchNum = strPurchNum;
            writer.PageEvent = watermarkList;
            doc1.Open();
            DateTime PrintTime = DateTime.Now;
            
            PdfPTable firsttable = new PdfPTable(4);
            firsttable.SetWidths(new float[] { 3, 4, 2, 3 });
            firsttable.DefaultCell.Border = Rectangle.NO_BORDER;
            firsttable.TotalWidth = 560F;
            firsttable.LockedWidth = true;
            firsttable.DefaultCell.SetLeading(1.2f, 1.2f);

            firsttable.AddCell(new Phrase("(一)軍品類別:", ChFont));
            firsttable.AddCell(new Phrase(query1301.OVC_PUR_NPURCH, Underline));
            firsttable.AddCell(new Phrase("(七)購案編號:", ChFont));
            var angcy = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.OVC_PUR_AGENCY).FirstOrDefault();
            firsttable.AddCell(new Phrase(strPurchNum + angcy, Underline));

            firsttable.AddCell(new Phrase("(二)軍品用途:", ChFont));
            firsttable.AddCell(new Phrase(query1301.OVC_TARGET_DO, Underline));
            firsttable.AddCell(new Phrase("(八)交貨時間:", ChFont));
            firsttable.AddCell(new Phrase(query1301.OVC_SHIP_TIMES, Underline));

            firsttable.AddCell(new Phrase("(三)預算來源:", SmailChFont));
            firsttable.AddCell(new Phrase(isource, Underline));
            firsttable.AddCell(new Phrase("(九)運輸方式:", ChFont));
            firsttable.AddCell(new Phrase(query1301.OVC_WAY_TRANS, Underline));

            firsttable.AddCell(new Phrase("(四)採購地區:", ChFont));
            firsttable.AddCell(new Phrase(drpOVC_COUNTRY.SelectedItem.Text.Split(':')[1], Underline));
            firsttable.AddCell(new Phrase("(十)起運及輸入口岸:", ChFont));
            firsttable.AddCell(new Phrase(query1301.OVC_FROM_TO, Underline));

            firsttable.AddCell(new Phrase("(五)採購單位:", ChFont));
            firsttable.AddCell(new Phrase(query1301.OVC_PUR_AGENCY, Underline));
            firsttable.AddCell(new Phrase("(十一)接收地點:", ChFont));
            firsttable.AddCell(new Phrase(query1301.OVC_TO_PLACE, Underline));
            firsttable.AddCell(new Phrase("(六)接收單位:", Underline));
            firsttable.AddCell(new Phrase(query1301.OVC_RECEIVE_NSECTION, Underline));
            firsttable.AddCell(new Phrase("(十二)檢驗方法", Underline));
            firsttable.AddCell(new Phrase(query1301.OVC_POI_IINSPECT, Underline));
            firsttable.DefaultCell.FixedHeight = 20f;
            firsttable.AddCell(new Phrase(" ", Underline));
            firsttable.AddCell(new Phrase(" ", Underline));
            firsttable.AddCell(new Phrase(" ", Underline));
            firsttable.AddCell(new Phrase(" ", Underline));
            
            doc1.Add(firsttable);


            PdfPTable secondtable = new PdfPTable(7);
            secondtable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            secondtable.SetWidths(new float[] { 1, 5, 1, 2, 2, 2, 2 });
            secondtable.TotalWidth = 560F;
            secondtable.LockedWidth = true;
            secondtable.AddCell(new Phrase("(十三) 項次", SmailChFont));
            secondtable.AddCell(new Phrase("(十四)品名料號及規格", SmailChFont));
            secondtable.AddCell(new Phrase("(十五) 單位", SmailChFont));
            secondtable.AddCell(new Phrase("(十六)數量", SmailChFont));
            secondtable.AddCell(new Phrase("(十七)單價", SmailChFont));
            secondtable.AddCell(new Phrase("(十八)總價", SmailChFont));
            secondtable.AddCell(new Phrase("(十九)備考      (以往購價)", SmailChFont));

            var query1201 = mpms.TBM1201.Where(o => o.OVC_PURCH.Equals(strPurchNum)).ToArray();
            var queryitem =
                from t in query1201
                join t2 in gm.TBM1407 on t.OVC_POI_IUNIT equals t2.OVC_PHR_ID
                where t2.OVC_PHR_CATE.Equals("J1")
                orderby t.ONB_POI_ICOUNT
                select new
                {
                    t2.OVC_PHR_ID,
                    t.ONB_POI_ICOUNT,
                    t.OVC_POI_NSTUFF_CHN,
                    NSN = t.NSN ?? "(空白)",
                    OVC_BRAND = t.OVC_BRAND ?? "(空白)",
                    OVC_MODEL = t.OVC_MODEL ?? "(空白)",
                    OVC_POI_IREF = t.OVC_POI_IREF ?? "(空白)",
                    OVC_FCODE = t.OVC_FCODE ?? "(空白)",
                    OVC_POI_IUNIT = t2.OVC_PHR_DESC,
                    t.ONB_POI_QORDER_PLAN,
                    t.ONB_POI_MPRICE_PLAN,
                    ONB_PRICE = t.ONB_POI_QORDER_PLAN * t.ONB_POI_MPRICE_PLAN,
                    t.OVC_POI_IPURCH_BEF,
                    t.OVC_CURR_MPRICE_BEF,
                    t.ONB_POI_QORDER_BEF,
                    t.ONB_POI_MPRICE_BEF,
                };
            decimal money = 0;
            var q1231 =
                from t in mpms.TBM1231
                join t1407 in mpms.TBM1407 on t.OVC_CURRENT equals t1407.OVC_PHR_ID
                where t.OVC_PURCH.Equals(strPurchNum) && t1407.OVC_PHR_CATE.Equals("B0")
                select t1407.OVC_PHR_DESC;
            
            foreach (var row in queryitem)
            {
                money += (decimal)row.ONB_POI_QORDER_PLAN * (decimal)row.ONB_POI_MPRICE_PLAN;
                secondtable.AddCell(new Phrase(row.ONB_POI_ICOUNT.ToString(), ChFont));
                PdfPCell content = new PdfPCell(new Phrase("  ", ChFont));
                Phrase p1 = new Phrase(row.OVC_POI_NSTUFF_CHN, ChFont);
                Phrase p2 = new Phrase("料號：" + row.NSN, ChFont);
                Phrase p3 = new Phrase("廠牌：" + row.OVC_BRAND, ChFont);
                Phrase p4 = new Phrase("型號：" + row.OVC_MODEL, ChFont);
                Phrase p5 = new Phrase("件號：" + row.OVC_POI_IREF, ChFont);
                Phrase p6 = new Phrase("行政院財產分類編號：" + row.OVC_FCODE, ChFont);
                content.AddElement(p1);
                content.AddElement(p2);
                content.AddElement(p3);
                content.AddElement(p4);
                content.AddElement(p5);
                content.AddElement(p6);
                secondtable.AddCell(content);
                secondtable.AddCell(new Phrase(row.OVC_PHR_ID + "(" +row.OVC_POI_IUNIT +")", ChFont));
                secondtable.AddCell(new Phrase(row.ONB_POI_QORDER_PLAN.ToString(), ChFont));
                secondtable.AddCell(new Phrase(((decimal)row.ONB_POI_MPRICE_PLAN).ToString("#,0.00"), ChFont));
                secondtable.AddCell(new Phrase(((decimal)row.ONB_PRICE).ToString("#,0.00"), ChFont));
                string reMark = "";
                if (row.OVC_POI_IPURCH_BEF != null)
                    reMark += "案號：" + row.OVC_POI_IPURCH_BEF;
                if (row.OVC_CURR_MPRICE_BEF != null && row.ONB_POI_MPRICE_BEF != null && row.ONB_POI_QORDER_BEF != null)
                {
                    if (row.ONB_POI_MPRICE_BEF != 0 && row.ONB_POI_QORDER_BEF != 0)
                    {
                        reMark += " " + row.OVC_CURR_MPRICE_BEF;
                        reMark += " 價格：" + row.ONB_POI_MPRICE_BEF.ToString();
                        reMark += " 數量：" + row.ONB_POI_QORDER_BEF.ToString();
                    }
                }
                secondtable.AddCell(new Phrase(reMark, ChFont));
            }
            doc1.Add(secondtable);

            string ovcIkind = "";
            switch (ViewState["OVC_PUR_AGENCY"].ToString())
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
            var query12201 = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum));
            var query =
                from t in gm.TBM1220_2.AsEnumerable()
                join t2 in query12201 on t.OVC_IKIND equals t2.OVC_IKIND into g
                from t2 in g.DefaultIfEmpty()
                where t.OVC_IKIND.StartsWith(ovcIkind)
                orderby t.OVC_IKIND
                select new
                {
                    t.OVC_IKIND,
                    t.OVC_MEMO_NAME,
                    OVC_MEMO = t2 != null ? t2.OVC_MEMO : "(空白)",
                    OVC_STANDARD = t2 != null ? t2.OVC_STANDARD : "(空白)"
                };

            var queryGroup =
                from t in query
                orderby t.OVC_IKIND
                group t by t.OVC_MEMO_NAME into g
                select g;

            PdfPTable finaltable = new PdfPTable(1);
            finaltable.SetWidths(new float[] { 1 });
            finaltable.TotalWidth = 560F;
            finaltable.LockedWidth = true;
            finaltable.SplitLate = false;
            finaltable.TableEvent = new TopBottomTableBorderMaker(BaseColor.BLACK, 0.5f);
            PdfPCell content3 = new PdfPCell(new Phrase("  ", ChFont));
            content3.Border = 0;
            content3.BorderWidthLeft = .5f;
            content3.BorderWidthRight = .5f;
            content3.AddElement(new Phrase("(二十)備註：", ChFont));
            finaltable.AddCell(content3);
            foreach (var row in queryGroup)
            {
                PdfPCell contentMemo = new PdfPCell (new Phrase("  ", ChFont));
                contentMemo.AddElement(new Phrase(row.Key + "：", SmailChFont));
                
                foreach (var item in row)
                {
                    string memo = item.OVC_MEMO.Replace("<br>", "");
                    if (item.OVC_STANDARD.Equals("Y"))
                       contentMemo.AddElement(new Phrase(memo, SmailChFont));
                    else
                       contentMemo.AddElement(new Phrase(memo, SmailblueChFont));
                    
                }
                contentMemo.Border = 0;
                contentMemo.BorderWidthLeft = .5f;
                contentMemo.BorderWidthRight = .5f;
                finaltable.AddCell(contentMemo);
            }
            PdfPCell content4 = new PdfPCell(new Phrase("  ", ChFont));
            content4.AddElement(new Phrase(" ", ChFont));
            content4.AddElement(new Phrase("(二十一)" + drp.SelectedItem.ToString() + "：" + q1231.FirstOrDefault() + money.ToString("#,0.00") + "元整（含稅）", ChFont));

            var query12201D20 = mpms.TBM1220_1.Where(o => o.OVC_IKIND.Equals("D20")).Select(o => o.OVC_MEMO).FirstOrDefault();

            content4.AddElement(new Phrase("(二十二)" + query12201D20, ChFont));
            content4.AddElement(new Phrase("(二十三)附件：" + GetAttached("M"), ChFont));
            content4.AddElement(new Phrase(" ", ChFont));
            content4.Border = 0;
            content4.BorderWidthLeft = .5f;
            content4.BorderWidthRight = .5f;
            finaltable.AddCell(content4);

            doc1.Add(finaltable);

            PdfPTable timetable = new PdfPTable(1);
            timetable.SetWidths(new float[] { 1 });
            timetable.DefaultCell.Border = Rectangle.NO_BORDER;
            timetable.TotalWidth = 560F;
            timetable.LockedWidth = true;
            timetable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            timetable.AddCell(new Phrase(PrintTime.ToString("yyyy/MM/dd HH:mm:ss"), ChFont));
            doc1.Add(timetable);


            doc1.Close();
            Response.Clear();//瀏覽器上顯示
            Response.AddHeader("Content-disposition", "attachment;filename=" + strPurchNum + "計畫清單.pdf");
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.Flush();
            Response.End();

        }
        #endregion

        private string GetAttached(string kind)
        {
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

        private string getTaiwanDate(string strDate,string cString)
        {
            //取得台灣年月日
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
            {

                return "";
            }
           
        }

        #endregion

        #region PreRender
        protected void GridView1_IN_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState[GridView1_IN.ID] != null)
                hasRows = Convert.ToBoolean(ViewState[GridView1_IN.ID]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState[GridView1.ID] != null)
                hasRows = Convert.ToBoolean(ViewState[GridView1.ID]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        
        protected void gvGroupLeft_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRowsL"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRowsL"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        protected void gvGroupRight_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRowsR"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRowsR"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        #endregion

        #region onclick

        protected void rdoOVC_LAB_SelectedIndexChanged(object sender, EventArgs e)
        {
            var query = gm.TBM1407;
            string textFirst = "不可空白";
            string valueFirst = "0";
            //標的分類觸發更改下拉選單
            if (rdoOVC_LAB.SelectedValue.Equals("1"))
            {
                DataTable dtStandardType = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "RD").ToList());
                FCommon.list_dataImportV(drpStandardType, dtStandardType, "OVC_PHR_DESC", "OVC_PHR_ID", textFirst, valueFirst, ":", true);
            }
            else {
                DataTable dtStandardType = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "RC").ToList());
                FCommon.list_dataImportV(drpStandardType, dtStandardType, "OVC_PHR_DESC", "OVC_PHR_ID", textFirst, valueFirst, ":", true);
            }
            
        }

        protected void btnSave_Modify_Click(object sender, EventArgs e)
        {
            //修改-存檔按鈕功能
            //資料儲存 TBM1301 TBM1301_PLAN TBMPURCH_EXT
            TBM1301_PLAN plan1301 = new TBM1301_PLAN();
            TBM1301 table1301 = new TBM1301();
            TBMPURCH_EXT tablePurch = new TBMPURCH_EXT();
            tablePurch = mpms.TBMPURCH_EXT.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            plan1301 = gm.TBM1301_PLAN.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            table1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            string strMessage = "";
            if (txtOVC_TARGET_KIND.Text.Equals(string.Empty))
                 strMessage += "<p> 請選擇  標的分類 </p>";
            if (drpOVC_RECEIVE_PLACE.SelectedValue.Equals("0"))
                 strMessage += "<p> 請選擇  履約地點 </p>";
            if (txtOVC_SHIP_TIMES.Text.Equals(string.Empty))
                 strMessage += "<p> 請輸入  履約期限 </p>";

            if (strMessage.Equals(string.Empty))
            {
                if (plan1301 != null)
                {
                    if (table1301 != null)
                    {
                        if (tablePurch != null)
                        {
                            plan1301.OVC_PUR_IPURCH = txtOVC_PUR_IPURCH.Text;
                            plan1301.OVC_PUR_NSECTION = txtOVC_PUR_NSECTION.Text;
                            plan1301.OVC_KEYIN = ViewState["UserName"].ToString();
                            plan1301.OVC_PUR_IUSER_PHONE = txtOVC_PUR_IUSER_PHONE.Text;
                            plan1301.OVC_PUR_IUSER_PHONE_EXT = txtOVC_PUR_IUSER_PHONE_EXT.Text;
                            plan1301.OVC_PUR_IUSER_PHONE_EXT1 = txtOVC_PUR_IUSER_PHONE_EXT1.Text;
                            plan1301.OVC_USER_CELLPHONE = txtOVC_USER_CELLPHONE.Text;
                            plan1301.OVC_LAB = rdoOVC_LAB.SelectedValue;
                            plan1301.OVC_PLAN_PURCH = drpOVC_PLAN_PURCH.SelectedValue;
                            plan1301.OVC_PURCH_OK = "Y";
                            gm.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                                , plan1301.GetType().Name.ToString(), this, "修改");
                            table1301.OVC_LAB = rdoOVC_LAB.SelectedValue;
                            table1301.OVC_PUR_NSECTION = plan1301.OVC_PUR_NSECTION;
                            table1301.OVC_PUR_SECTION = plan1301.OVC_PUR_SECTION;
                            table1301.OVC_PROPOSE = lblOVC_PROPOSE.Text;
                            table1301.OVC_DPROPOSE = lblOVC_DPROPOSE.Text;
                            table1301.OVC_PUR_IPURCH = plan1301.OVC_PUR_IPURCH;
                            table1301.OVC_PUR_IPURCH_ENG = txtOVC_PUR_IPURCH_ENG.Text;
                            table1301.OVC_PUR_USER = plan1301.OVC_PUR_USER;
                            table1301.OVC_USER_TITLE = txtOVC_USER_TITLE.Text;
                            table1301.OVC_KEYIN = ViewState["UserName"].ToString();
                            table1301.OVC_PUR_IUSER_PHONE = plan1301.OVC_PUR_IUSER_PHONE;
                            table1301.OVC_PUR_IUSER_PHONE_EXT = plan1301.OVC_PUR_IUSER_PHONE_EXT;
                            table1301.OVC_PUR_IUSER_PHONE_EXT1 = plan1301.OVC_PUR_IUSER_PHONE_EXT1;
                            table1301.OVC_USER_CELLPHONE = plan1301.OVC_USER_CELLPHONE;
                            table1301.OVC_PUR_AGENCY = plan1301.OVC_PUR_AGENCY;
                            table1301.OVC_PUR_FEE_OK = rdoOVC_PUR_FEE_OK.SelectedValue;
                            table1301.OVC_PUR_GOOD_OK = rdoOVC_PUR_GOOD_OK.SelectedValue;
                            table1301.OVC_PUR_TAX_OK = rdoOVC_PUR_TAX_OK.SelectedValue;
                            table1301.OVC_DOING_UNIT = lblOVC_DOING_UNIT.Text;
                            table1301.OVC_SUPERIOR_UNIT = txtOVC_SUPERIOR_UNIT.Text;
                            table1301.OVC_PLAN_PURCH = plan1301.OVC_PLAN_PURCH;
                            table1301.IS_PLURAL_BASIS = drpIS_PLURAL_BASIS.SelectedValue;
                            table1301.IS_OPEN_CONTRACT = drpIS_OPEN_CONTRACT.SelectedValue;
                            table1301.IS_JUXTAPOSED_MANUFACTURER = drpIS_JUXTAPOSED_MANUFACTURER.SelectedValue;
                            //DateTime dateTime = DateTime.Now;
                            //table1301.OVC_VERSION = dateTime.ToString("yyyy-MM-dd hh:mm:ss tt");

                            gm.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                                , table1301.GetType().Name.ToString(), this, "修改");
                            tablePurch.OVC_PUR_NSECTION_2 = txtOVC_KIND_APPLY.Text;
                            tablePurch.OVC_TARGET_KIND = txtOVC_TARGET_KIND.Text;
                            tablePurch.OVC_PERFORMANCE_PLACE = drpOVC_RECEIVE_PLACE.SelectedValue;
                            tablePurch.OVC_PERFORMANCE_LIMIT = txtOVC_SHIP_TIMES.Text;
                            tablePurch.OVC_VENDOR_DESC = txtOVC_VENDOR_DESC.Text;
                            
                            mpms.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                                , tablePurch.GetType().Name.ToString(), this, "修改");
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "儲存成功");
                        }
                    }
                }
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }

            //頁籤顯示條件
            string strStatus = drpIS_PLURAL_BASIS.SelectedValue.ToString();
            if (strStatus.Equals("Y"))
            {
                pageThird.Visible = true;
                divThird.Visible = true;
            }
            else
            {
                pageThird.Visible = false;
                divThird.Visible = false;
            }
            
        }

        protected void btnModify_BUDGET_Click(object sender, EventArgs e)
        {
            //預算編輯按鈕功能
            var query1231P = mpms.TBM1231_PLAN.Where(o => o.OVC_PURCH.Equals(strPurchNum));
            var query1231 = mpms.TBM1231.Where(o => o.OVC_PURCH.Equals(strPurchNum));
            var query1118 = mpms.TBM1118.Where(o => o.OVC_PURCH.Equals(strPurchNum));
            decimal? decBudget = 0;
            if (query1231P.Any() && !query1231.Any() && !query1118.Any())
            {
                var query =
                    from t in mpms.TBM1231_PLAN
                    where t.OVC_PURCH.Equals(strPurchNum)
                    group t by new { t.OVC_BUDGET_YEAR, t.OVC_IKIND } into g
                    select new
                    {
                        g.Key.OVC_BUDGET_YEAR,
                        g.Key.OVC_IKIND,
                        Money = g.Sum(o => o.ONB_MONEY)
                    };
                foreach(var item in query)
                {
                    TBM1231 tbm1231 = new TBM1231();
                    tbm1231.OVC_CURRENT = "N";
                    tbm1231.ONB_RATE = 1;
                    tbm1231.OVC_IKIND = item.OVC_IKIND;
                    if (item.OVC_IKIND.Equals("1"))
                        tbm1231.OVC_ISOURCE = item.OVC_BUDGET_YEAR + "年度國防預算";
                    else
                    {
                        var queryName = 
                            mpms.TBM1231_PLAN
                            .Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_IKIND.Equals("2"))
                            .Select(o => o.OVC_PJNAME).FirstOrDefault();
                        tbm1231.OVC_ISOURCE = item.OVC_BUDGET_YEAR + queryName;
                    }
                    tbm1231.OVC_PURCH = strPurchNum;
                    tbm1231.ONB_MONEY = item.Money;
                    decBudget += item.Money;
                    mpms.TBM1231.Add(tbm1231);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , tbm1231.GetType().Name.ToString(), this, "新增");
                }

                foreach (var item in query1231P)
                {
                    TBM1118 tbm1118 = new TBM1118();
                    tbm1118.OVC_CURRENT = "N";
                    tbm1118.ONB_RATE = 1;
                    tbm1118.OVC_IKIND = item.OVC_IKIND;
                    tbm1118.OVC_ISOURCE = item.OVC_BUDGET_YEAR + "年度國防預算";
                    tbm1118.ONB_MBUD = item.ONB_MONEY;
                    tbm1118.OVC_YY = item.OVC_BUDGET_YEAR;
                    tbm1118.OVC_MM = item.OVC_BUDGET_MONTH;
                    tbm1118.OVC_PJNAME = item.OVC_PJNAME;
                    tbm1118.OVC_POI_IBDG = item.OVC_POI_IBDG;
                    tbm1118.OVC_PURCH = item.OVC_PURCH;
                    mpms.TBM1118.Add(tbm1118);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , tbm1118.GetType().Name.ToString(), this, "新增");
                }
            }

            #region tbm1301
            TBM1301_PLAN t1301_p = mpms.TBM1301_PLAN.Where(t => t.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            if (t1301_p != null)
            {
                t1301_p.ONB_PUR_BUDGET = decBudget;
                mpms.SaveChanges();
            }
            TBM1301 t1301 = mpms.TBM1301.Where(t => t.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            if (t1301 != null)
            {
                t1301.OVC_PUR_CURRENT = "N";
                t1301.ONB_PUR_BUDGET = decBudget;
                t1301.ONB_PUR_BUDGET_NT = decBudget;
                mpms.SaveChanges();
            }
            #endregion

            string url;
            url = "MPMS_B13_1.aspx?PurchNum=" + strPurchNum;
            Response.Redirect(url);
        }

        protected void btnSave_New_Click(object sender, EventArgs e)
        {
            //新增-儲存按鈕功能
            //資料儲存 TBM1301 TBM1301_PLAN TBMPURCH_EXT TBM1231_PLAN -> TBM1231
            TBM1301_PLAN tb1301plan = new TBM1301_PLAN();
            var query1301PLAN = gm.TBM1301_PLAN.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            tb1301plan = query1301PLAN;
            string strMessage = "";
            if (txtOVC_TARGET_KIND.Text.Equals(string.Empty))
                strMessage += "<p> 請選擇  標的分類 </p>";
            if (drpOVC_RECEIVE_PLACE.SelectedValue.Equals("0"))
                strMessage += "<p> 請選擇  履約地點 </p>";
            if (txtOVC_SHIP_TIMES.Text.Equals(string.Empty))
                strMessage += "<p> 請輸入  履約期限 </p>";


            if (strMessage.Equals(string.Empty))
            {
                if (tb1301plan != null)
                {
                     tb1301plan.OVC_PUR_IPURCH = txtOVC_PUR_IPURCH.Text;
                     tb1301plan.OVC_PUR_NSECTION = txtOVC_PUR_NSECTION.Text;
                     tb1301plan.OVC_KEYIN = ViewState["UserName"].ToString();
                     tb1301plan.OVC_PUR_IUSER_PHONE = txtOVC_PUR_IUSER_PHONE.Text;
                     tb1301plan.OVC_PUR_IUSER_PHONE_EXT = txtOVC_PUR_IUSER_PHONE_EXT.Text;
                     tb1301plan.OVC_PUR_IUSER_PHONE_EXT1 = txtOVC_PUR_IUSER_PHONE_EXT1.Text;
                     tb1301plan.OVC_USER_CELLPHONE = txtOVC_USER_CELLPHONE.Text;
                     tb1301plan.OVC_LAB = rdoOVC_LAB.SelectedValue;
                     tb1301plan.OVC_PLAN_PURCH = drpOVC_PLAN_PURCH.SelectedValue;
                     tb1301plan.OVC_PURCH_OK = "Y";
                     gm.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , tb1301plan.GetType().Name.ToString(), this, "修改");
                }

                TBM1301 tb1301 = new TBM1301();
                tb1301.OVC_PURCH = strPurchNum;
                if (tb1301plan.OVC_PUR_AGENCY == "B" || tb1301plan.OVC_PUR_AGENCY == "L" || tb1301plan.OVC_PUR_AGENCY == "P")
                {
                    tb1301.OVC_PURCH_KIND = "1";
                    divSecContentIN.Visible = true;
                }
                else
                {
                    tb1301.OVC_PURCH_KIND = "2";
                    divSecContentOUT.Visible = true;
                }
                var query1301P = gm.TBM1301_PLAN.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                tb1301.OVC_LAB = rdoOVC_LAB.SelectedValue;
                tb1301.OVC_PUR_NSECTION = query1301P.OVC_PUR_NSECTION;
                tb1301.OVC_PUR_SECTION = query1301P.OVC_PUR_SECTION;
                tb1301.OVC_PUR_ASS_VEN_CODE = query1301P.OVC_PUR_ASS_VEN_CODE;
                tb1301.OVC_PUR_IPURCH = query1301P.OVC_PUR_IPURCH;
                tb1301.OVC_PUR_IPURCH_ENG = txtOVC_PUR_IPURCH_ENG.Text;
                tb1301.OVC_PUR_USER = query1301P.OVC_PUR_USER;
                tb1301.OVC_USER_TITLE = txtOVC_USER_TITLE.Text;
                tb1301.OVC_KEYIN = ViewState["UserName"].ToString();
                tb1301.OVC_PUR_IUSER_PHONE = query1301P.OVC_PUR_IUSER_PHONE;
                tb1301.OVC_PUR_IUSER_PHONE_EXT = query1301P.OVC_PUR_IUSER_PHONE_EXT;
                tb1301.OVC_PUR_IUSER_PHONE_EXT1 = query1301P.OVC_PUR_IUSER_PHONE_EXT1;
                tb1301.OVC_USER_CELLPHONE = query1301P.OVC_USER_CELLPHONE;
                tb1301.OVC_PUR_AGENCY = query1301P.OVC_PUR_AGENCY;
                tb1301.OVC_PUR_FEE_OK = rdoOVC_PUR_FEE_OK.SelectedValue;
                tb1301.OVC_PUR_GOOD_OK = rdoOVC_PUR_GOOD_OK.SelectedValue;
                tb1301.OVC_PUR_TAX_OK = rdoOVC_PUR_TAX_OK.SelectedValue;
                DateTime dateTime = DateTime.Now;
                tb1301.OVC_PUR_CREAT = dateTime.ToString("yyyy-MM-dd");
                tb1301.OVC_SUPERIOR_UNIT = txtOVC_SUPERIOR_UNIT.Text;
                tb1301.OVC_PLAN_PURCH = query1301P.OVC_PLAN_PURCH;
                tb1301.IS_PLURAL_BASIS = drpIS_PLURAL_BASIS.SelectedValue;
                tb1301.IS_OPEN_CONTRACT = drpIS_OPEN_CONTRACT.SelectedValue;
                tb1301.IS_JUXTAPOSED_MANUFACTURER = drpIS_JUXTAPOSED_MANUFACTURER.SelectedValue;
                tb1301.OVC_VERSION = dateTime.ToString("yyyy-MM-dd hh:mm:ss tt");
                gm.TBM1301.Add(tb1301);
                gm.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                    , tb1301.GetType().Name.ToString(), this, "新增");
                TBMPURCH_EXT tablePurch = new TBMPURCH_EXT();
                tablePurch.OVC_PURCH = strPurchNum;
                tablePurch.OVC_PUR_NSECTION_2 = txtOVC_KIND_APPLY.Text;
                tablePurch.OVC_TARGET_KIND = txtOVC_TARGET_KIND.Text;
                tablePurch.OVC_PERFORMANCE_PLACE = drpOVC_RECEIVE_PLACE.SelectedValue;
                tablePurch.OVC_PERFORMANCE_LIMIT = txtOVC_SHIP_TIMES.Text;
                tablePurch.OVC_VENDOR_DESC = txtOVC_VENDOR_DESC.Text;
                mpms.TBMPURCH_EXT.Add(tablePurch);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                    , tablePurch.GetType().Name.ToString(), this, "新增");

                //預算編輯按鈕
                btnModify_BUDGET.Visible = true;
                //請求事項編輯按鈕
                btnModify.Visible = true;
                //備考編輯按鈕
                btnModify_MEMO.Visible = true;
                //用途編輯按鈕
                btnUseEditing.Visible = true;
                //外購理由編輯按鈕
                btnOutPurEditing.Visible = true;
                //二三頁籤
                pageSecond.Visible = true;
                if (drpIS_PLURAL_BASIS.SelectedValue == "Y")
                    pageThird.Visible = true;
                //隱藏新增按鈕顯示新增按鈕
                btnSave_New.Visible = false;
                btnSave_Modify.Visible = true;

                FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功！");

            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }

            


        }

        protected void btnUseEditing_Click(object sender, EventArgs e)
        {
            //用途編輯按鈕內容
            string send_urlUse;
            send_urlUse = "~/pages/MPMS/B/MPMS_B13_5.aspx?PurchNum=" + Request.QueryString["PurchNum"];
            Response.Redirect(send_urlUse);
        }

        protected void btnOutPurEditing_Click(object sender, EventArgs e)
        {
            //外購理由編輯按鈕內容
            string send_urlUse;
            send_urlUse = "~/pages/MPMS/B/MPMS_B13_6.aspx?PurchNum=" + Request.QueryString["PurchNum"];
            Response.Redirect(send_urlUse);
        }

        protected void btn_change_IN_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            lblONB_POI_ICOUNT_IN.Text = gvr.Cells[0].Text;
            dataImport_1201IN(gvr.Cells[0].Text);
        }

        protected void btn_change_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            lblONB_POI_ICOUNT.Text = gvr.Cells[0].Text;
            dataImport_1201(gvr.Cells[0].Text);

        }

        protected void btnSave_plan_IN_Click(object sender, EventArgs e)
        {
            //內購的計畫清單儲存
            string strMessage = "";
            if (string.IsNullOrEmpty(txtOVC_AGNT_IN_SHOW_IN.Text))
            {
                strMessage += "請輸入 採購單位";
            }
            if (string.IsNullOrEmpty(strMessage))
            {
                TBM1301 table1301 = new TBM1301();
                table1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                if (table1301 != null)
                {
                    string strOVC_CURR_MPRICE_BEF = drpOVC_CURR_MPRICE_BEF_IN.SelectedValue;
                    if (strOVC_CURR_MPRICE_BEF.Equals("0"))
                        strOVC_CURR_MPRICE_BEF = "";
                    else
                        table1301.OVC_PUR_CURRENT = strOVC_CURR_MPRICE_BEF;

                    table1301.OVC_AGNT_IN = txtOVC_AGNT_IN_SHOW_IN.Text;
                    table1301.OVC_RECEIVE_NSECTION = txtOVC_RECEIVE_NSECTION_IN.Text;
                    table1301.OVC_SHIP_TIMES = txtOVC_SHIP_TIMES_IN.Text;
                    table1301.OVC_RECEIVE_PLACE = txtOVC_RECEIVE_PLACE_IN.Text;
                    table1301.OVC_POI_IINSPECT = txtOVC_POI_IINSPECT.Text;
                    gm.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , table1301.GetType().Name.ToString(), this, "修改");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
                }
                else
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "存檔失敗");
                }
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }

        }

        protected void btnSave_plan_Click(object sender, EventArgs e)
        {
            //外購的計畫清單儲存
            string strMessage = "";
            if (string.IsNullOrEmpty(txtOVC_AGNT_IN_SHOW.Text))
            {
                strMessage += "請輸入 採購單位";
            }

            if (string.IsNullOrEmpty(strMessage))
            {
                TBM1301 table1301 = new TBM1301();
                table1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                if (table1301 != null)
                {
                    string strOVC_CURR_MPRICE_BEF = drpOVC_CURR_MPRICE_BEF.SelectedValue;
                    if (strOVC_CURR_MPRICE_BEF.Equals("0"))
                        strOVC_CURR_MPRICE_BEF = "";
                    else
                        table1301.OVC_PUR_CURRENT = strOVC_CURR_MPRICE_BEF;

                    table1301.OVC_TARGET_DO = txtOVC_TARGET_DO.Text;
                    table1301.OVC_COUNTRY = drpOVC_COUNTRY.SelectedValue;
                    table1301.OVC_AGNT_IN = txtOVC_AGNT_IN_SHOW.Text;
                    table1301.OVC_RECEIVE_NSECTION = txtOVC_RECEIVE_NSECTION.Text;
                    table1301.OVC_SHIP_TIMES = txtOVC_SHIP_TIMES.Text;
                    table1301.OVC_WAY_TRANS = txtOVC_WAY_TRANS.Text;
                    table1301.OVC_FROM_TO = txtOVC_FROM_TO.Text;
                    table1301.OVC_TO_PLACE = txtOVC_TO_PLACE.Text;
                    table1301.OVC_POI_IINSPECT = txtOVC_POI_IINSPECT_OUT.Text;
                    gm.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , table1301.GetType().Name.ToString(), this, "修改");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
                }
                else
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "存檔失敗");
                }
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
        }

        protected void btnSave_DETIAL_Click(object sender, EventArgs e)
        {
            //外購
            //明細存檔 TBM1201 TBM1233
            //先查是不是異動
            decimal deONB_POI_ICOUNT = decimal.Parse(lblONB_POI_ICOUNT.Text);
            DataTable dt = new DataTable();

            var query =
                from table in mpms.TBM1201
                where table.OVC_PURCH.Equals(strPurchNum) && table.ONB_POI_ICOUNT == deONB_POI_ICOUNT
                select table;
            dt = CommonStatic.LinqQueryToDataTable(query);
            if (dt.Rows.Count > 0)
            {
                //異動
                TBM1201 tbm1201 = new TBM1201();
                TBM1233 tbm1233 = new TBM1233();
                tbm1201 = mpms.TBM1201.Where(table => table.OVC_PURCH.Equals(strPurchNum) && table.ONB_POI_ICOUNT == deONB_POI_ICOUNT).FirstOrDefault();
                tbm1233 = mpms.TBM1233.Where(table => table.OVC_PURCH.Equals(strPurchNum) && table.ONB_POI_ICOUNT == deONB_POI_ICOUNT).FirstOrDefault();
                if (tbm1201 != null)
                {
                    tbm1201.OVC_POI_NSTUFF_CHN = txtOVC_POI_NSTUFF_CHN.Text;
                    tbm1201.OVC_POI_NSTUFF_ENG = txtOVC_POI_NSTUFF_ENG.Text;
                    tbm1201.NSN_KIND = drpNSN_KIND.SelectedValue;
                    tbm1201.NSN = txtNSN.Text;
                    if (chkOVC_SAME_QUALITY_BRAND.Checked)
                        tbm1201.OVC_BRAND = txtOVC_BRAND.Text + "(或同等品)";
                    else
                        tbm1201.OVC_BRAND = txtOVC_BRAND.Text;

                    if (chkOVC_SAME_QUALITY_MODEL.Checked)
                        tbm1201.OVC_MODEL = txtOVC_MODEL.Text + "(或同等品)";
                    else
                        tbm1201.OVC_MODEL = txtOVC_MODEL.Text;

                    if (chkOVC_SAME_QUALITY_POI_IREF.Checked)
                        tbm1201.OVC_POI_IREF = txtOVC_POI_IREF.Text + "(或同等品)";
                    else
                        tbm1201.OVC_POI_IREF = txtOVC_POI_IREF.Text;

                    tbm1201.OVC_FCODE = txtOVC_FCODE.Text;
                    tbm1201.OVC_POI_IUNIT = drpOVC_POI_IUNIT.SelectedValue;
                    tbm1201.ONB_POI_QORDER_PLAN = decimal.Parse(txtONB_POI_QORDER_PLAN.Text);
                    tbm1201.ONB_POI_MPRICE_PLAN = decimal.Parse(txtONB_POI_MPRICE_PLAN.Text);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), 
                        tbm1201.GetType().Name.ToString(), this, "修改");
                    if (tbm1233 != null)
                    {
                        tbm1233.OVC_POI_NDESC = txtOVC_INSPECT.Text;
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , tbm1233.GetType().Name.ToString(), this, "修改");
                    }
                       
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "存檔失敗");
            }
            else
            {   //新增
                decimal numONB_POI_QORDER_PLAN = 0;
                decimal numONB_POI_MPRICE_PLAN = 0;
                string strMessage = "";
                if (!decimal.TryParse(txtONB_POI_QORDER_PLAN.Text, out numONB_POI_QORDER_PLAN))
                    strMessage += "<p> 請先 輸入數量 <p>";
                if (!decimal.TryParse(txtONB_POI_MPRICE_PLAN.Text, out numONB_POI_MPRICE_PLAN))
                    strMessage += "<p> 請先 輸入價格 <p>";
                if (string.IsNullOrEmpty(strMessage))
                {
                    TBM1201 tbm1201 = new TBM1201();
                    TBM1233 tbm1233 = new TBM1233();
                    tbm1201.OVC_PURCH = strPurchNum;
                    tbm1201.ONB_POI_ICOUNT = short.Parse(lblONB_POI_ICOUNT.Text);
                    tbm1201.OVC_POI_NSTUFF_CHN = txtOVC_POI_NSTUFF_CHN.Text;
                    tbm1201.OVC_POI_NSTUFF_ENG = txtOVC_POI_NSTUFF_ENG.Text;
                    tbm1201.NSN_KIND = drpNSN_KIND.SelectedValue;
                    tbm1201.NSN = txtNSN.Text;
                    if (chkOVC_SAME_QUALITY_BRAND.Checked)
                        tbm1201.OVC_BRAND = txtOVC_BRAND.Text + "(或同等品)";
                    else
                        tbm1201.OVC_BRAND = txtOVC_BRAND.Text;

                    if (chkOVC_SAME_QUALITY_MODEL.Checked)
                        tbm1201.OVC_MODEL = txtOVC_MODEL.Text + "(或同等品)";
                    else
                        tbm1201.OVC_MODEL = txtOVC_MODEL.Text;

                    if (chkOVC_SAME_QUALITY_POI_IREF.Checked)
                        tbm1201.OVC_POI_IREF = txtOVC_POI_IREF.Text + "(或同等品)";
                    else
                        tbm1201.OVC_POI_IREF = txtOVC_POI_IREF.Text;

                    tbm1201.OVC_FCODE = txtOVC_FCODE.Text;
                    tbm1201.OVC_POI_IUNIT = drpOVC_POI_IUNIT.SelectedValue;
                    tbm1201.ONB_POI_QORDER_PLAN = numONB_POI_QORDER_PLAN;
                    tbm1201.ONB_POI_MPRICE_PLAN = numONB_POI_MPRICE_PLAN;
                    if (drpOVC_FIRST_BUY.SelectedValue.Equals("N"))
                    {
                        if (chk_OVC_NUM.Checked)
                        {
                            tbm1201.OVC_POI_IPURCH_BEF = txtOVC_POI_IPURCH_BEF.Text;
                            tbm1201.OVC_CURR_MPRICE_BEF = drpOVC_CURR_MPRICE_BEF.SelectedValue;
                            tbm1201.ONB_POI_QORDER_BEF = decimal.Parse(txtONB_POI_QORDER_BEF.Text);
                            tbm1201.ONB_POI_MPRICE_BEF = decimal.Parse(txtONB_POI_MPRICE_BEF.Text);
                        }
                        else if (chk_OVC_SAME.Checked)
                        {
                            tbm1201.OVC_POI_IPURCH_BEF = "999999";
                            tbm1201.OVC_CURR_MPRICE_BEF = "";
                        }
                    }
                    else
                        tbm1201.OVC_FIRST_BUY = drpOVC_FIRST_BUY.SelectedValue;

                    tbm1201.OVC_POI_NDESC = txtOVC_POI_NDESC.Text;
                    mpms.TBM1201.Add(tbm1201);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , tbm1201.GetType().Name.ToString(), this, "新增");
                    if (!string.IsNullOrEmpty(txtOVC_INSPECT.Text))
                    {
                        tbm1233.OVC_PURCH = strPurchNum;
                        tbm1233.ONB_POI_ICOUNT = short.Parse(lblONB_POI_ICOUNT.Text);
                        tbm1233.OVC_POI_NDESC = txtOVC_INSPECT.Text;
                        mpms.TBM1233.Add(tbm1233);
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , tbm1233.GetType().Name.ToString(), this, "新增");
                    }
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "儲存成功");
                    int count = Int32.Parse(lblONB_POI_ICOUNT.Text);
                    lblONB_POI_ICOUNT.Text = (count + 1).ToString();
                    FCommon.Controls_Clear(txtOVC_POI_NSTUFF_CHN, txtOVC_POI_NSTUFF_ENG, txtNSN, txtOVC_FCODE, 
                        txtONB_POI_QORDER_PLAN, txtONB_POI_MPRICE_PLAN, txtOVC_POI_NDESC);
                }
                else
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                }

            }
            if (drpIS_PLURAL_BASIS.SelectedValue == "Y")
            {
                pageThirdImport(1);
                pageThirdImport_ICout();
            }
            dataImport_Detail(GridView1);
            if (GridView1.Rows.Count > 0)
                FooterSum(GridView1);
        }

        protected void btnSave_DETIAL_IN_Click(object sender, EventArgs e)
        {
            //內購

            //明細存檔 TBM1201 TMBPUCHER_EXT
            //先查是不是異動
            decimal deONB_POI_ICOUNT = 0;
            decimal.TryParse(lblONB_POI_ICOUNT_IN.Text,out deONB_POI_ICOUNT);
            DataTable dt = new DataTable();

            var query =
                from table in mpms.TBM1201
                where table.OVC_PURCH.Equals(strPurchNum) && table.ONB_POI_ICOUNT == deONB_POI_ICOUNT
                select table;
            dt = CommonStatic.LinqQueryToDataTable(query);
            if (dt.Rows.Count > 0)
            {
                //異動
                TBM1201 tbm1201 = new TBM1201();
                TBM1233 tbm1233 = new TBM1233();
                tbm1201 = mpms.TBM1201.Where(table => table.OVC_PURCH.Equals(strPurchNum) && table.ONB_POI_ICOUNT == deONB_POI_ICOUNT).FirstOrDefault();
                tbm1233 = mpms.TBM1233.Where(table => table.OVC_PURCH.Equals(strPurchNum) && table.ONB_POI_ICOUNT == deONB_POI_ICOUNT).FirstOrDefault();
                if (tbm1201 != null)
                {
                    tbm1201.OVC_POI_NSTUFF_CHN = txtOVC_POI_NSTUFF_CHN_IN.Text;
                    tbm1201.OVC_POI_NSTUFF_ENG = txtOVC_POI_NSTUFF_ENG_IN.Text;
                    tbm1201.NSN_KIND = drpNSN_KIND_IN.SelectedValue;
                    tbm1201.NSN = txtNSN_IN.Text;
                    if (chkOVC_SAME_QUALITY_BRAND_IN.Checked)
                        tbm1201.OVC_BRAND = txtOVC_BRAND_IN.Text + "(或同等品)";
                    else
                        tbm1201.OVC_BRAND = txtOVC_BRAND_IN.Text;

                    if (chkOVC_SAME_QUALITY_MODEL_IN.Checked)
                        tbm1201.OVC_MODEL = txtOVC_MODEL_IN.Text + "(或同等品)";
                    else
                        tbm1201.OVC_MODEL = txtOVC_MODEL_IN.Text;

                    if (chkOVC_SAME_QUALITY_POI_IREF_IN.Checked)
                        tbm1201.OVC_POI_IREF = txtOVC_POI_IREF_IN.Text + "(或同等品)";
                    else
                        tbm1201.OVC_POI_IREF = txtOVC_POI_IREF_IN.Text;
                    tbm1201.OVC_FCODE = txtOVC_FCODE_IN.Text;
                    tbm1201.OVC_POI_IUNIT = drpOVC_POI_IUNIT_IN.SelectedValue;
                    tbm1201.ONB_POI_QORDER_PLAN = decimal.Parse(txtONB_POI_QORDER_PLAN_IN.Text);
                    tbm1201.ONB_POI_MPRICE_PLAN = decimal.Parse(txtONB_POI_MPRICE_PLAN_IN.Text);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , tbm1201.GetType().Name.ToString(), this, "修改");
                    if (tbm1233 != null)
                    {
                        tbm1233.OVC_POI_NDESC = txtOVC_INSPECT.Text;
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , tbm1233.GetType().Name.ToString(), this, "修改");
                    }
                        
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
                    
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "存檔失敗");
            }
            else
            {
                if(string.IsNullOrEmpty(txtONB_POI_QORDER_PLAN_IN.Text)||
                    string.IsNullOrEmpty(txtONB_POI_MPRICE_PLAN_IN.Text))
                {
                    FCommon.AlertShow(PnMessage,"danger","系統訊息","數量及單價不得為空!");
                }
                else
                {
                    //新增
                    TBM1201 tbm1201 = new TBM1201();
                    TBM1233 tbm1233 = new TBM1233();
                    tbm1201.OVC_PURCH = strPurchNum;
                    tbm1201.ONB_POI_ICOUNT = short.Parse(lblONB_POI_ICOUNT_IN.Text);
                    tbm1201.OVC_POI_NSTUFF_CHN = txtOVC_POI_NSTUFF_CHN_IN.Text;
                    tbm1201.OVC_POI_NSTUFF_ENG = txtOVC_POI_NSTUFF_ENG_IN.Text;
                    tbm1201.NSN_KIND = drpNSN_KIND_IN.SelectedValue;
                    tbm1201.NSN = txtNSN_IN.Text;
                    if (chkOVC_SAME_QUALITY_BRAND.Checked)
                        tbm1201.OVC_BRAND = txtOVC_BRAND_IN.Text + "(或同等品)";
                    else
                        tbm1201.OVC_BRAND = txtOVC_BRAND_IN.Text;

                    if (chkOVC_SAME_QUALITY_MODEL_IN.Checked)
                        tbm1201.OVC_MODEL = txtOVC_MODEL_IN.Text + "(或同等品)";
                    else
                        tbm1201.OVC_MODEL = txtOVC_MODEL_IN.Text;

                    if (chkOVC_SAME_QUALITY_POI_IREF.Checked)
                        tbm1201.OVC_POI_IREF = txtOVC_POI_IREF_IN.Text + "(或同等品)";
                    else
                        tbm1201.OVC_POI_IREF = txtOVC_POI_IREF_IN.Text;

                    tbm1201.OVC_FCODE = txtOVC_FCODE_IN.Text;
                    tbm1201.OVC_POI_IUNIT = drpOVC_POI_IUNIT_IN.SelectedValue;
                    tbm1201.ONB_POI_QORDER_PLAN = decimal.Parse(txtONB_POI_QORDER_PLAN_IN.Text);
                    tbm1201.ONB_POI_MPRICE_PLAN = decimal.Parse(txtONB_POI_MPRICE_PLAN_IN.Text);

                    if (drpOVC_FIRST_BUY_IN.SelectedValue.Equals("N"))
                    {
                        if (chk_OVC_NUM_IN.Checked)
                        {
                            tbm1201.OVC_POI_IPURCH_BEF = txtOVC_POI_IPURCH_BEF_IN.Text;
                            tbm1201.OVC_CURR_MPRICE_BEF = drpOVC_CURR_MPRICE_BEF_IN.SelectedValue;
                            tbm1201.ONB_POI_QORDER_BEF = decimal.Parse(txtONB_POI_QORDER_BEF_IN.Text);
                            tbm1201.ONB_POI_MPRICE_BEF = decimal.Parse(txtONB_POI_MPRICE_BEF_IN.Text);
                        }
                        else if (chk_OVC_SAME_IN.Checked)
                        {
                            tbm1201.OVC_POI_IPURCH_BEF = "999999";
                            tbm1201.OVC_CURR_MPRICE_BEF = "";
                        }
                    }
                    else
                        tbm1201.OVC_FIRST_BUY = drpOVC_FIRST_BUY_IN.SelectedValue;

                    tbm1201.OVC_POI_NDESC = txtOVC_POI_NDESC_IN.Text;
                    mpms.TBM1201.Add(tbm1201);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1201.GetType().Name.ToString(), this, "新增");
                    if (!txtOVC_INSPECT_IN.Text.Equals(string.Empty))
                    {
                        tbm1233.OVC_PURCH = strPurchNum;
                        tbm1233.ONB_POI_ICOUNT = short.Parse(lblONB_POI_ICOUNT_IN.Text);
                        tbm1233.OVC_POI_NDESC = txtOVC_INSPECT_IN.Text;
                        mpms.TBM1233.Add(tbm1233);
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1233.GetType().Name.ToString(), this, "新增");
                    }
                    int count = Int32.Parse(lblONB_POI_ICOUNT_IN.Text);
                    lblONB_POI_ICOUNT_IN.Text = (count + 1).ToString();
                    FCommon.Controls_Clear(txtOVC_POI_NSTUFF_CHN_IN, txtOVC_POI_NSTUFF_ENG_IN, txtNSN_IN, txtOVC_FCODE_IN,
                        txtONB_POI_QORDER_PLAN_IN, txtONB_POI_MPRICE_PLAN_IN, txtOVC_POI_NDESC_IN);
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "儲存成功");
                }
                

            }
            if (drpIS_PLURAL_BASIS.SelectedValue == "Y")
            {
                pageThirdImport(1);
                pageThirdImport_ICout();
            }
            dataImport_Detail(GridView1_IN);
            if (GridView1_IN.Rows.Count > 0)
                FooterSum(GridView1_IN);
        }

        protected void btnDel_IN_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            short deONB_POI_COUNT = short.Parse(gvr.Cells[0].Text);
            TBM1201 tbm1201 = new TBM1201();
            TBM1233 tbm1233 = new TBM1233();
            tbm1201 = mpms.TBM1201.Where(o => o.OVC_PURCH.Equals(strPurchNum)
                                      && o.ONB_POI_ICOUNT == deONB_POI_COUNT).FirstOrDefault();
            if (tbm1201 != null)
            {
                mpms.Entry(tbm1201).State = EntityState.Deleted;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                    , tbm1201.GetType().Name.ToString(), this, "刪除");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
            }
            tbm1233 = mpms.TBM1233.Where(o => o.OVC_PURCH.Equals(strPurchNum)
                                      && o.ONB_POI_ICOUNT == deONB_POI_COUNT).FirstOrDefault();
            if (tbm1233 != null)
            {
                mpms.Entry(tbm1233).State = EntityState.Deleted;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                    , tbm1233.GetType().Name.ToString(), this, "刪除");
            }

            dataImport_Detail(GridView1_IN);
            FooterSum(GridView1_IN);
        }

        protected void btnCancel_LEFT_Click(object sender, EventArgs e)
        {
            //分組刪除按鈕 左邊
            Button btn = (Button)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            short numONB_POI_COUNT = short.Parse(gvr.Cells[1].Text);
            short numONB_GROUP_PRE = short.Parse(lblONB_GROUP_PRE.Text);
            TBM1118_2 tbm1118_2 = new TBM1118_2();
            tbm1118_2 = mpms.TBM1118_2.Where(o => o.OVC_PURCH.Equals(strPurchNum)
                                                && o.ONB_GROUP_PRE == numONB_GROUP_PRE && o.ONB_POI_ICOUNT == numONB_POI_COUNT).FirstOrDefault();
            mpms.Entry(tbm1118_2).State = EntityState.Deleted;
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                , tbm1118_2.GetType().Name.ToString(), this, "刪除");
            dataGroupBudget();
            pageThirdImport(numONB_GROUP_PRE);
            pageThirdImport_ICout();
            DrpGroupImport();
            drpGROUP.SelectedValue = lblONB_GROUP_PRE.Text;
        }

        protected void btnCancel_Right_Click(object sender, EventArgs e)
        {
            //分組刪除按鈕 右邊
            Button btn = (Button)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            short numONB_POI_COUNT = short.Parse(gvr.Cells[1].Text);
            short numONB_GROUP_PRE = short.Parse(lblONB_GROUP_PRE.Text);
            TBM1118_2 tbm1118_2 = new TBM1118_2();
            tbm1118_2 = mpms.TBM1118_2.Where(o => o.OVC_PURCH.Equals(strPurchNum)
                                                && o.ONB_GROUP_PRE == numONB_GROUP_PRE && o.ONB_POI_ICOUNT == numONB_POI_COUNT).FirstOrDefault();
            mpms.Entry(tbm1118_2).State = EntityState.Deleted;
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                , tbm1118_2.GetType().Name.ToString(), this, "刪除");
            dataGroupBudget();
            pageThirdImport(numONB_GROUP_PRE);
            pageThirdImport_ICout();
            DrpGroupImport();

            drpGROUP.SelectedValue = lblONB_GROUP_PRE.Text;
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            //分類存檔案按鈕
            dataSaveGroup(gvONB_POI_ICOUNT_LEFT, "cbIsGroupLeft");
            dataSaveGroup(gvONB_POI_ICOUNT_Right, "cbIsGroupRight");
            //計算分組預算
            dataGroupBudget();
            pageThirdImport(short.Parse(lblONB_GROUP_PRE.Text));
            pageThirdImport_ICout();
            DrpGroupImport();
            drpGROUP.SelectedValue = lblONB_GROUP_PRE.Text;
        }
        
        protected void btnSelectAll_Click(object sender, EventArgs e)
        {
            //全選按鈕
            SelectAll(gvONB_POI_ICOUNT_LEFT, "cbIsGroupLeft");
            SelectAll(gvONB_POI_ICOUNT_Right, "cbIsGroupRight");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            //取消全選按鈕
            ResetAll(gvONB_POI_ICOUNT_LEFT, "cbIsGroupLeft");
            ResetAll(gvONB_POI_ICOUNT_Right, "cbIsGroupRight");
        }

        protected void btnSave_WITH_IN_Click(object sender, EventArgs e)
        {
            string send_urlUse;
            send_urlUse = "~/pages/MPMS/B/MPMS_B13_9.aspx?PurchNum=" + Request.QueryString["PurchNum"];
            Response.Redirect(send_urlUse);
        }

        protected void btnSave_WITH_Click(object sender, EventArgs e)
        {
            string send_urlUse;
            send_urlUse = "~/pages/MPMS/B/MPMS_B13_9.aspx?PurchNum=" + Request.QueryString["PurchNum"];
            Response.Redirect(send_urlUse);
        }

        protected void btnToDoc_Click(object sender, EventArgs e)
        {
            string url = "~/pages/MPMS/B/MPMS_B13_8.aspx?PurchNum=" + Request.QueryString["PurchNum"];
            Response.Redirect(url);
        }

        protected void btnFileUpload_Click(object sender, EventArgs e)
        {
            string url = "~/pages/MPMS/B/MPMS_B13_3.aspx?PurchNum=" + Request.QueryString["PurchNum"] + "&IKIND=M";
            Response.Redirect(url);
        }

        protected void btnFileUpload_Page2_Click(object sender, EventArgs e)
        {
            string url = "~/pages/MPMS/B/MPMS_B13_3.aspx?PurchNum=" + Request.QueryString["PurchNum"] + "&IKIND=D";
            Response.Redirect(url);
        }

        protected void btnModify_Click(object sender, EventArgs e)
        {
            string url = "~/pages/MPMS/B/MPMS_B13_2.aspx?PurchNum=" + Request.QueryString["PurchNum"];
            Response.Redirect(url);
        }

        protected void btnModify_MEMO_Click(object sender, EventArgs e)
        {
            string url = "~/pages/MPMS/B/MPMS_B13_4.aspx?PurchNum=" + Request.QueryString["PurchNum"];
            Response.Redirect(url);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            Session["unitquery"] = "query6";
        }

        protected void btnQuery_NUM_IN_Click(object sender, EventArgs e)
        {
            //Session["phrasequery"] = "query1";
        }

        protected void btnQuery_NUM_Click(object sender, EventArgs e)
        {
            Session["phrasequery"] = "queryOther";
        }

        protected void btnSave_OVC_MEMO_IN_Click(object sender, EventArgs e)
        {
            string url = "~/pages/MPMS/B/MPMS_B13_7.aspx?PurchNum=" + Request.QueryString["PurchNum"];
            Response.Redirect(url);
        }

        protected void btnSave_OVC_MEMO_Click(object sender, EventArgs e)
        {
            string url = "~/pages/MPMS/B/MPMS_B13_7.aspx?PurchNum=" + Request.QueryString["PurchNum"];
            Response.Redirect(url);
        }

        protected void btnQuery_IN_Click(object sender, EventArgs e)
        {
           // Session["unitquery"] = "query7";
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            short deONB_POI_COUNT = short.Parse(gvr.Cells[0].Text);
            TBM1201 tbm1201 = new TBM1201();
            TBM1233 tbm1233 = new TBM1233();
            tbm1201 = mpms.TBM1201.Where(o => o.OVC_PURCH.Equals(strPurchNum)
                                      && o.ONB_POI_ICOUNT == deONB_POI_COUNT).FirstOrDefault();
            if (tbm1201 != null)
            {
                mpms.Entry(tbm1201).State = EntityState.Deleted;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                , tbm1201.GetType().Name.ToString(), this, "刪除");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
            }
            tbm1233 = mpms.TBM1233.Where(o => o.OVC_PURCH.Equals(strPurchNum)
                                      && o.ONB_POI_ICOUNT == deONB_POI_COUNT).FirstOrDefault();
            if (tbm1233 != null)
            {
                mpms.Entry(tbm1233).State = EntityState.Deleted;
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , tbm1233.GetType().Name.ToString(), this, "刪除");
                mpms.SaveChanges();
            }

            dataImport_Detail(GridView1);
            FooterSum(GridView1);
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_B11");
        }


        #endregion

        #region SelectedChanged

        protected void drpStandardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //標的分類代碼觸發
            txtOVC_TARGET_KIND.Text = drpStandardType.SelectedValue.ToString();
        }

        protected void drpOVC_AGNT_IN_IN_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOVC_AGNT_IN_SHOW_IN.Text = drpOVC_AGNT_IN_IN.SelectedItem.Text;
        }

        protected void drpOVC_AGNT_IN_OUT_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOVC_AGNT_IN_SHOW.Text = drpOVC_AGNT_IN_OUT.SelectedItem.Text;
        }
        protected void drpGROUP_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageThirdImport(short.Parse(drpGROUP.SelectedValue));
            lblONB_GROUP_PRE.Text = drpGROUP.SelectedValue;
            lblONB_GROUP_PRE_2.Text = drpGROUP.SelectedValue;
        }

       
        protected void btnPrint_Command(object sender, CommandEventArgs e)
        {
            //列印按鈕
            string strAngcy;
            if (ViewState["strOVC_PURCH_KIND"].ToString().Equals("1"))
            {
                strAngcy = "內購";
            }
            else
            {
                strAngcy = "外購";
            }
                
            switch (e.CommandName)
            {
                case "PrintSupPDF":
                    PrintPDF(strAngcy);
                    break;
                case "PrintNewSupPDF":
                    PrintNewPDF();
                    break;
                case "PrintListInPDF":
                    PrintListINPDF();
                    break;
                case "PrintListOutPDF":
                    PrintListOutPDF();
                    break;
            }
        }

        #endregion
    }
}