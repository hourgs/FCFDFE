using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;

namespace FCFDFE.pages.MPMS.B
{
    public class PDF : PdfPageEventHelper
    {
        PdfTemplate template;
        BaseFont bf = null;
        PdfContentByte cb;
        /** The header text. */
        public string Header { get; set; }
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
    }
    public partial class MPMS_B13_1 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strPurchNum = "";
        string[] strField_1 = { "OVC_ISOURCE", "ONB_MONEY" , "OVC_ISAPPR" };
        string[] strField_2 = { "OVC_ISOURCE", "OVC_IBDGPJNAME", "OVC_IKIND", "OVC_YY", "OVC_MM", "ONB_MBUD" };
        //GV_LoginBudget.FooterRow.Cells[0].Text; 拿footer合計資料
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (Request.QueryString["PurchNum"] != null)
                {
                    strPurchNum = Request.QueryString["PurchNum"].ToString();
                    //設置readonly屬性
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_PUR_DAPPR_PLAN);
                    btnModify.Visible = false;
                    lblOVC_PURCH.Text = strPurchNum;
                    if (strPurchNum != null)
                    {
                        if (!IsPostBack)
                        {
                            Loginscreen();
                            dataImport();
                            dataImport_YYMM();
                            list_dataImport(drpOVC_YY);
                        }
                    }
                    if (GV_LoginBudget.Rows.Count > 0)
                        SumMONEY(GV_LoginBudget, 2, "預算合計:", 4);
                    if (GV_BudgetAllocation.Rows.Count > 0)
                        SumMONEY(GV_BudgetAllocation, 5, "目前已預劃:", 6);
                }
                else
                {
                    string url = "~/pages/MPMS/B/MPMS_B11.aspx";
                    Response.Redirect(url);
                }
            }
        }
        #region GV_PreRender
        protected void GV_BudgetAllocation_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void GV_LoginBudget_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        #endregion

        #region 副程式
        private void Loginscreen()
        {
            var query = gm.TBM1407;
            //國防預算L8非國防預算L9
            rdoOVC_IKIND.SelectedValue = "1";
            DataTable dtOVC_POI_IBDG = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "L8").ToList());
            FCommon.list_dataImportV(drpOVC_POI_IBDG_2, dtOVC_POI_IBDG, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            //幣別
            DataTable dtOVC_CURRENT = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "B0").ToList());
            FCommon.list_dataImportV(drpOVC_CURRENT, dtOVC_CURRENT, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            drpOVC_CURRENT.SelectedValue = "N";
            //匯率
            TBM1118 tb1118 = new TBM1118();
            tb1118 = mpms.TBM1118.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            if (tb1118 != null)
            {
                txtONB_RATE.Text = tb1118.ONB_RATE.ToString();
            }
            else
            {
                txtONB_RATE.Text = "";
            }
            
        }
        private void dataImport()
        {
            //上方預算科目GV
            DataTable dt = new DataTable();
            var GVContent =
                from table1231 in mpms.TBM1231
                where table1231.OVC_PURCH.Equals(strPurchNum)
                select new
                {
                    OVC_PURCH = table1231.OVC_PURCH,
                    OVC_ISOURCE = table1231.OVC_ISOURCE,
                    OVC_CURRENT = table1231.OVC_CURRENT,
                    ONB_MONEY = table1231.ONB_MONEY,
                    ONB_RATE = table1231.ONB_RATE,
                    table1231.OVC_PUR_DAPPR_PLAN,
                    table1231.OVC_PUR_APPR_PLAN
                };
            var GV2 =
                from t in GVContent.AsEnumerable()
                select new
                {
                    OVC_PURCH = t.OVC_PURCH,
                    OVC_ISOURCE = t.OVC_ISOURCE,
                    OVC_CURRENT = t.OVC_CURRENT,
                    ONB_MONEY = t.ONB_MONEY.ToString(),
                    ONB_RATE = t.ONB_RATE,
                    OVC_ISAPPR = t.OVC_PUR_DAPPR_PLAN + "\t" + t.OVC_PUR_APPR_PLAN
                };
            if (GV2.Any())
            {
                var query = gm.TBM1407;
                DataTable dtOVC_CURRENT = CommonStatic.LinqQueryToDataTable(query.Where(table => table.OVC_PHR_CATE == "B0"));
                dt = CommonStatic.LinqQueryToDataTable(GV2);
                ViewState["ONB_RATE"] = dt.Rows[0]["ONB_RATE"].ToString();
                foreach (DataRow rows in dt.Rows)
                {
                    string strISAPPR = rows["OVC_ISAPPR"].ToString();
                    string strmoney = rows["OVC_CURRENT"].ToString();
                    if (strISAPPR.Equals("\t俟奉核後，再行補註"))
                        rows["OVC_ISAPPR"] = "";

                    foreach (DataRow rowsCurrent in dtOVC_CURRENT.Rows)
                    {
                        if (strmoney.Equals(rowsCurrent["OVC_PHR_ID"].ToString()))
                        {
                            rows["ONB_MONEY"] = rowsCurrent["OVC_PHR_DESC"].ToString() + ":" + rows["ONB_MONEY"].ToString();
                        }
                    }
                }


                DataTable dtIsource = CommonStatic.ListToDataTable(mpms.TBM1231.Where(table => table.OVC_PURCH.Equals(strPurchNum)).ToList());
                FCommon.list_dataImport(drpOVC_ISOURCE, dtIsource, "OVC_ISOURCE", "OVC_ISOURCE", true);
                //下方新增旁的drlist(匯入款源名稱)
                /*
                drpOVC_ISOURCE.Items.Clear();
                drpOVC_ISOURCE.Items.Add( new System.Web.UI.WebControls.ListItem("請選擇", "請選擇"));
                foreach (DataRow rows in dt.Rows)
                {
                    drpOVC_ISOURCE.Items.Add(new System.Web.UI.WebControls.ListItem(rows["OVC_ISOURCE"].ToString(), rows["OVC_ISOURCE"].ToString()));
                }*/
            }
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_LoginBudget, dt, strField_1);
        }

        private void dataImport_modify()
        {
            //按下異動後顯示詳細資料(上方GV)
            DataTable dt = new DataTable();
            var query =
                from table1231 in mpms.TBM1231
                where table1231.OVC_PURCH.Equals(strPurchNum) && table1231.OVC_ISOURCE.Equals(txtOVC_POI_IBDG.Text)
                select table1231;
            if (query.Any())
            {
                dt = CommonStatic.LinqQueryToDataTable(query);

                //款源
                if (dt.Rows[0]["OVC_IKIND"].ToString().Equals("1"))
                    rdoOVC_IKIND.SelectedValue = "1";
                else
                    rdoOVC_IKIND.SelectedValue = "2";

                drpOVC_CURRENT.SelectedValue = dt.Rows[0]["OVC_CURRENT"].ToString();

                txtONB_RATE.Text = dt.Rows[0]["ONB_RATE"].ToString();

                if (dt.Rows[0]["OVC_PUR_APPR_PLAN"].ToString().Equals("俟奉核後，再行補註"))
                    drpApproved.SelectedValue = "2";
                else
                    drpApproved.SelectedValue = "1";

                txtOVC_PUR_DAPPR_PLAN.Text = dt.Rows[0]["OVC_PUR_DAPPR_PLAN"].ToString();
                txtOVC_PUR_APPR_PLAN.Text = dt.Rows[0]["OVC_PUR_APPR_PLAN"].ToString();

            }

        }

        private void dataImport_YYMM()
        {
            //下方年度月份GV
            DataTable dtBudgetAllocation = new DataTable();
            var GVContent =
                from table1118 in mpms.TBM1118
                where table1118.OVC_PURCH.Equals(strPurchNum)
                select new
                {
                    OVC_PURCH = table1118.OVC_PURCH,
                    OVC_ISOURCE = table1118.OVC_ISOURCE,
                    OVC_IKIND = table1118.OVC_IKIND,
                    OVC_POI_IBDG = table1118.OVC_POI_IBDG,
                    OVC_PJNAME = table1118.OVC_PJNAME,
                    OVC_IBDGPJNAME = table1118.OVC_PJNAME,//預留空位給編號 + 科目名稱
                    OVC_YY = table1118.OVC_YY,
                    OVC_MM = table1118.OVC_MM,
                    ONB_MBUD = table1118.ONB_MBUD
                };
            dtBudgetAllocation = CommonStatic.LinqQueryToDataTable(GVContent);
            foreach (DataRow rows in dtBudgetAllocation.Rows)
            {
                string strOVC_IKIND = rows["OVC_IKIND"].ToString();
                if (strOVC_IKIND == "1")
                {
                    rows["OVC_IBDGPJNAME"] =
                        rows["OVC_POI_IBDG"].ToString()
                        + ":" + rows["OVC_PJNAME"].ToString();
                }
            }
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_BudgetAllocation, dtBudgetAllocation, strField_2);
            
        }

        private void list_dataImport(ListControl list)
        {
            //年度下拉選單
            //先將下拉式選單清空
            list.Items.Clear();

            DateTime datetime = DateTime.Now;
            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            int CalDateYear = Convert.ToInt16(datetime.ToString("yyy", culture));
            
            int num = CalDateYear;
            for (int i = 0; i < 15; i++)
            {
                list.Items.Add(Convert.ToString(num));
                num = num - 1;
            }
        }

        //gv合計footer
        private void SumMONEY(GridView gv, int columnNo, string text, int columnCount)
        {
            TableCell footercell = new TableCell();
            decimal sum = 0;
            foreach (GridViewRow rows in gv.Rows)
            {
                string[] money = rows.Cells[columnNo].Text.Split(':');
                int length = money.Length;
                sum += decimal.Parse(money[length-1]);
            }
            footercell.Text = text + sum.ToString();
            footercell.ColumnSpan = columnCount;
            gv.FooterRow.Cells.Clear();
            gv.FooterRow.Cells.Add(footercell);
        }
        
        private string whatIKIND()
        {
            string[] strOVC_IKIND;
            if (drpOVC_ISOURCE.SelectedItem.ToString().Equals("請選擇"))
            {
                strOVC_IKIND = new string[] {"1"};
                return strOVC_IKIND[0];
            }
            else
            {
                string strSource = drpOVC_ISOURCE.SelectedItem.Text;
                var GVContent =
                from table1231 in mpms.TBM1231
                where table1231.OVC_PURCH.Equals(strPurchNum)
                where table1231.OVC_ISOURCE.Equals(strSource)
                select table1231.OVC_IKIND;
                strOVC_IKIND = GVContent.ToArray();

                return strOVC_IKIND[0];
            }
            
        }

        private string[] getIBDG()
        {
            //drpOVC_POI_IBDG_2 獲取預算名稱
            string selectItem = drpOVC_POI_IBDG_2.SelectedItem.ToString();
            Char delimiter = ':';
            string[] item;
            if (selectItem.Equals("請選擇"))
            {
                item =new string[]{"",""};
                return item;
            }
            else
            {
                item = selectItem.Split(delimiter);
                return item;
            }
            
        }
        private string GetTaiwanDate()
        {

            DateTime datetime = DateTime.Now;
            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            return datetime.ToString("yyy-MM-dd hh:mm:ss tt", culture);
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

        private void AddMoneyToMain()
        {
            TBM1301 tbm1301 = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            var queryMoney = mpms.TBM1231.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Sum(o => o.ONB_MONEY) ?? 0;
            var rate = mpms.TBM1231.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.ONB_RATE).FirstOrDefault() ?? 0;
            decimal MoneyNT = queryMoney * rate;
            tbm1301.ONB_PUR_BUDGET = queryMoney;
            tbm1301.ONB_PUR_BUDGET_NT = MoneyNT;
            gm.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , tbm1301.GetType().Name.ToString(), this, "修改");
        }
        #endregion

        #region Selected~Changed
        protected void drpApproved_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpApproved.SelectedValue.ToString().Equals("2"))
                txtOVC_PUR_APPR_PLAN.Text = "俟奉核後，再行補註";
            else
                txtOVC_PUR_APPR_PLAN.Text = "";
        }

        protected void drpOVC_ISOURCE_SelectedIndexChanged(object sender, EventArgs e)
        {
            //連動預算科目
            var query = gm.TBM1407;
            if (whatIKIND().Equals("1"))
            {
                DataTable dtOVC_POI_IBDG = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "L8"));
                FCommon.list_dataImportV(drpOVC_POI_IBDG_2, dtOVC_POI_IBDG, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            }
            else
            {
                DataTable dtOVC_POI_IBDG = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "L9"));
                FCommon.list_dataImportV(drpOVC_POI_IBDG_2, dtOVC_POI_IBDG, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            }
            

        }
        
        
        protected void drpOVC_POI_IBDG2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //連動到名稱
            string[] item = getIBDG();
            txtOVC_PJNAME.Text = item[1];
        }

        #endregion

        #region OnClick
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //新增存檔按鈕功能
            float num1;
            string strMessage = "";
            string strOVC_PUR_DAPPR_PLAN = txtOVC_PUR_DAPPR_PLAN.Text;
            string strOVC_PUR_APPR_PLAN = txtOVC_PUR_APPR_PLAN.Text;
            if (txtOVC_POI_IBDG.Text.Equals(string.Empty))
                strMessage += "<p> 款源不得空白 </p>";

            if (!float.TryParse(txtONB_RATE.Text, out num1))
            {
                if (txtONB_RATE.Text.Equals(string.Empty))
                    strMessage += "<p> 匯率不得空白 </p>";
                else
                    strMessage += "<p> 匯率格式有誤 </p>";
            }

            TBM1231 table1231query = new TBM1231();
            table1231query = mpms.TBM1231.Where(table => table.OVC_PURCH.Equals(strPurchNum) 
                            && table.OVC_ISOURCE.Equals(txtOVC_POI_IBDG.Text)).FirstOrDefault();
            if (strMessage.Equals(string.Empty))
            {
                if (table1231query == null)
                {
                    TBM1231 table1231 = new TBM1231();
                    table1231.OVC_PURCH = strPurchNum;
                    table1231.OVC_ISOURCE = txtOVC_POI_IBDG.Text;
                    table1231.OVC_IKIND = rdoOVC_IKIND.SelectedValue.ToString();
                    table1231.OVC_PUR_DAPPR_PLAN = txtOVC_PUR_DAPPR_PLAN.Text;
                    table1231.OVC_PUR_APPR_PLAN = txtOVC_PUR_APPR_PLAN.Text;
                    table1231.OVC_CURRENT = drpOVC_CURRENT.SelectedValue.ToString();
                    table1231.ONB_RATE = decimal.Parse(txtONB_RATE.Text);
                    table1231.ONB_MONEY = 0;
                    mpms.TBM1231.Add(table1231);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , table1231.GetType().Name.ToString(), this, "新增");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功，請等候審核");
                }
                else
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "該預購來源已存在");
                }
                dataImport();
                if (GV_LoginBudget.Rows.Count > 0)
                    SumMONEY(GV_LoginBudget, 2, "預算合計:", 4);
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
            
        }
        protected void btnModify_Click(object sender, EventArgs e)
        {   
            //異動存檔按鈕
            string strMessage = "";
            string strOVC_PUR_DAPPR_PLAN = txtOVC_PUR_DAPPR_PLAN.Text;
            string strOVC_PUR_APPR_PLAN = txtOVC_PUR_APPR_PLAN.Text;

            if (txtOVC_POI_IBDG.Text.Equals(string.Empty))
                strMessage += "<P> 款源不得空白 </p>";
            
            TBM1231 table1231query = new TBM1231();
            table1231query = mpms.TBM1231
                .Where(table => table.OVC_PURCH.Equals(strPurchNum))
                .Where(table => table.OVC_ISOURCE.Equals(txtOVC_POI_IBDG.Text)).FirstOrDefault();
            if (strMessage.Equals(string.Empty))
            {
                if (table1231query != null)
                {
                    table1231query.OVC_IKIND = rdoOVC_IKIND.SelectedValue.ToString();
                    table1231query.OVC_PUR_DAPPR_PLAN = txtOVC_PUR_DAPPR_PLAN.Text;
                    table1231query.OVC_PUR_APPR_PLAN = txtOVC_PUR_APPR_PLAN.Text;
                    table1231query.OVC_CURRENT = drpOVC_CURRENT.SelectedValue.ToString();
                    table1231query.ONB_RATE = decimal.Parse(txtONB_RATE.Text);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                       , table1231query.GetType().Name.ToString(), this, "修改");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                }
                else
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "該筆款源不存在");
                }
                dataImport();
                if (GV_LoginBudget.Rows.Count > 0)
                    SumMONEY(GV_LoginBudget, 2, "預算合計:", 4);
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }

        }
        protected void btnDetails_Click(object sender, EventArgs e)
        {
            //預算年度分配明細表按鈕功能
            var query = mpms.TBM1231.Where(o => o.OVC_PURCH.Equals(strPurchNum));
            if (query.Any())
            {
                //預算年度分配明細表按鈕功能
                BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
                iTextSharp.text.Font ChFont = new iTextSharp.text.Font(bfChinese, 12f);
                iTextSharp.text.Font smaillChFont = new iTextSharp.text.Font(bfChinese, 10f);
                var doc1 = new Document(PageSize.A4, 40, 50, 140, -10);
                MemoryStream Memory = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
                writer.PageEvent = new PDF();
                doc1.Open();
                DateTime PrintTime = DateTime.Now;

                PdfPTable firsttable = new PdfPTable(2);
                firsttable.SetWidths(new float[] { 1, 1 });
                firsttable.DefaultCell.Border = Rectangle.NO_BORDER;
                firsttable.TotalWidth = 560F;
                firsttable.LockedWidth = true;
                firsttable.DefaultCell.SetLeading(1.2f, 1.2f);
                var query1301 = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();

                firsttable.AddCell(new Phrase("購案編號：" + strPurchNum + query1301.OVC_PUR_AGENCY, ChFont));
                firsttable.AddCell(new Phrase("購案名稱：" + query1301.OVC_PUR_IPURCH, ChFont));
                firsttable.AddCell(new Phrase("申購單位：" + query1301.OVC_PUR_NSECTION, ChFont));
                firsttable.AddCell(new Phrase("承辦人姓名：" + query1301.OVC_PUR_USER, ChFont));
                firsttable.AddCell(new Phrase("電話(自動)：" + query1301.OVC_PUR_IUSER_PHONE, ChFont));
                firsttable.AddCell(new Phrase("電話(軍線)：" + query1301.OVC_PUR_IUSER_PHONE_EXT, ChFont));
                firsttable.AddCell(new Phrase(" ", ChFont));
                firsttable.AddCell(new Phrase(" ", ChFont));
                doc1.Add(firsttable);

                PdfPTable secendtable = new PdfPTable(4);
                secendtable.SetWidths(new float[] { 1, 3, 3, 3 });
                secendtable.TotalWidth = 560F;
                secendtable.LockedWidth = true;
                secendtable.DefaultCell.FixedHeight = 25f;
                secendtable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                secendtable.AddCell(new Phrase("項次", ChFont));
                secendtable.AddCell(new Phrase("款源", ChFont));
                secendtable.AddCell(new Phrase("預算小計", ChFont));
                secendtable.AddCell(new Phrase("預算是否核定 ", ChFont));

                var query1231 =
                   from t in mpms.TBM1231
                   join c in mpms.TBM1407 on t.OVC_CURRENT equals c.OVC_PHR_ID
                   where t.OVC_PURCH.Equals(strPurchNum) && c.OVC_PHR_CATE == "B0"
                   select new
                   {
                       t.OVC_ISOURCE,
                       ONB_MONEY = t.ONB_MONEY ?? 0,
                       t.OVC_PUR_APPR_PLAN,
                       t.OVC_PUR_DAPPR_PLAN,
                       c.OVC_PHR_DESC
                   };

                int counter = 1;
                decimal sumBudget = 0;
                foreach (var item in query1231)
                {
                    string strDAPPR = GetTaiwanDate(item.OVC_PUR_DAPPR_PLAN);
                    string strMONEY = Convert.ToDecimal(item.ONB_MONEY).ToString("#,###.00");
                    sumBudget += (decimal)item.ONB_MONEY;
                    secendtable.AddCell(new Phrase(counter.ToString(), smaillChFont));
                    secendtable.AddCell(new Phrase(item.OVC_ISOURCE, smaillChFont));
                    secendtable.AddCell(new Phrase(item.OVC_PHR_DESC + "  " + strMONEY, smaillChFont));
                    secendtable.AddCell(new Phrase(item.OVC_PUR_APPR_PLAN + strDAPPR, smaillChFont));
                    counter++;

                }

                string strBudget = sumBudget.ToString("#,###.00");

                PdfPCell tail = new PdfPCell(new Phrase("預算總金額：" + strBudget + "(" + query1231.FirstOrDefault().OVC_PHR_DESC + ")", ChFont));
                tail.Colspan = 4;
                tail.Border = Rectangle.NO_BORDER;
                tail.FixedHeight = 30f;
                tail.VerticalAlignment = Element.ALIGN_MIDDLE;
                secendtable.AddCell(tail);

                doc1.Add(secendtable);


                PdfPTable finaltable = new PdfPTable(5);
                finaltable.SetWidths(new float[] { 1, 4, 1, 1, 3 });
                finaltable.TotalWidth = 560F;
                finaltable.LockedWidth = true;
                finaltable.DefaultCell.FixedHeight = 25f;
                finaltable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                finaltable.AddCell(new Phrase("項次", ChFont));
                finaltable.AddCell(new Phrase("預算科目、名稱 ", ChFont));
                finaltable.AddCell(new Phrase("年度", ChFont));
                finaltable.AddCell(new Phrase("月份", ChFont));
                finaltable.AddCell(new Phrase("預劃金額 ", ChFont));

                var query1118 =
                    from t in mpms.TBM1118
                    where t.OVC_PURCH.Equals(strPurchNum)
                    orderby t.OVC_YY, t.OVC_MM
                    select t;

                int counter2 = 1;
                decimal sumDetailBudget = 0;
                foreach (var item in query1118)
                {
                    string strMONEY = Convert.ToDecimal(item.ONB_MBUD).ToString("#,###.00");
                    finaltable.AddCell(new Phrase(counter2.ToString(), smaillChFont));
                    finaltable.AddCell(new Phrase(item.OVC_POI_IBDG + "(" + item.OVC_PJNAME + ")", smaillChFont));
                    finaltable.AddCell(new Phrase(item.OVC_YY, smaillChFont));
                    finaltable.AddCell(new Phrase(item.OVC_MM, smaillChFont));
                    finaltable.AddCell(new Phrase(strMONEY, smaillChFont));
                    sumDetailBudget += (decimal)item.ONB_MBUD;
                    counter2++;
                }
                string strDetailBudget = sumDetailBudget.ToString("#,###.00");
                PdfPCell content1 = new PdfPCell(new Phrase("預劃總金額：" + strDetailBudget, ChFont));
                content1.Colspan = 4;
                content1.Border = Rectangle.NO_BORDER;

                finaltable.AddCell(content1);
                string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt");
                PdfPCell content2 = new PdfPCell(new Phrase(timeStamp, ChFont));
                content2.HorizontalAlignment = Element.ALIGN_RIGHT;
                content2.Border = Rectangle.NO_BORDER;

                finaltable.AddCell(content2);
                doc1.Add(finaltable);
                doc1.Close();
                Response.Clear();//瀏覽器上顯示
                Response.AddHeader("Content-disposition", "attachment;filename=" + strPurchNum + "預算年度分配明細表.pdf");
                Response.ContentType = "application/octet-stream";
                Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.Flush();
                Response.End();
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "尚未新增預算資料");
            }

        }

       
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            //回物資申請書編制作業按鈕功能
            
            //返回物資申請書編制頁面
            string send_urlUse;
            send_urlUse = "~/pages/MPMS/B/MPMS_B13.aspx?PurchNum=" + strPurchNum;
            Response.Redirect(send_urlUse);

        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            //預算預劃新增
            string strMessage = "";
            string strOVC_ISOURCE = "";
            try
            {
                strOVC_ISOURCE = drpOVC_ISOURCE.SelectedItem.ToString();
                string strOVC_POI_IBDG = "";
                string strOVC_IKIND = "";
                string strOVC_PJNAME = drpOVC_POI_IBDG_2.SelectedItem.ToString();
                string[] item;
                string strOVC_YY = drpOVC_YY.SelectedItem.ToString();
                string strOVC_MM = drpOVC_MM.SelectedItem.ToString();
                string strOVC_CURRENT = "";
                decimal decimalONB_RATE = 0;
                decimal decimalONB_MBUD = 0;


                if (strOVC_ISOURCE.Equals("請選擇"))
                    strMessage += "<p> 請先 選擇款源 </p>";
                else
                {
                    strOVC_IKIND = whatIKIND();
                    string drpIsource = drpOVC_ISOURCE.SelectedItem.Text;
                    var query_current_rate =
                        from table in mpms.TBM1231
                        where table.OVC_PURCH.Equals(strPurchNum) && table.OVC_ISOURCE.Equals(drpIsource)
                        select new
                        {
                            table.OVC_CURRENT,
                            table.ONB_RATE
                        };
                    foreach (var q in query_current_rate)
                    {
                        strOVC_CURRENT = q.OVC_CURRENT;
                        decimalONB_RATE = (decimal)q.ONB_RATE;
                    }

                }

                if (strOVC_PJNAME.Equals("請選擇") && !string.IsNullOrEmpty(txtOVC_PJNAME.Text) && strOVC_IKIND.Equals("2"))
                {

                    strOVC_PJNAME = txtOVC_PJNAME.Text;
                    strOVC_POI_IBDG = txtOVC_PJNAME.Text;
                }
                else if (strOVC_PJNAME.Equals("請選擇") && strOVC_IKIND.Equals("1"))
                {
                    strMessage += "<p> 請先 選擇預算科目 </p>";

                }
                else
                {
                    item = getIBDG();
                    strOVC_PJNAME = item[1];
                    if (strOVC_IKIND.Equals("1"))
                        strOVC_POI_IBDG = item[0];
                    else
                        strOVC_POI_IBDG = item[1];
                }



                if (txtONB_MBUD.Text.Equals(string.Empty))
                    strMessage += "<p> 請先 輸入預劃金額 </p>";
                else
                    decimalONB_MBUD = decimal.Parse(txtONB_MBUD.Text);

                //查詢是否已經存在
                var queryexist =
                    from exist in mpms.TBM1118
                    where exist.OVC_PURCH.Equals(strPurchNum)
                    where exist.OVC_ISOURCE.Equals(strOVC_ISOURCE)
                    where exist.OVC_POI_IBDG.Equals(strOVC_POI_IBDG)
                    where exist.OVC_YY.Equals(strOVC_YY)
                    where exist.OVC_MM.Equals(strOVC_MM)
                    select exist;

                if (queryexist.Any())
                {
                    try
                    {
                        if (strMessage.Equals(string.Empty))
                        {
                            TBM1118 tbm1118 = queryexist.FirstOrDefault();
                            var tempONB_MBUD = tbm1118.ONB_MBUD;
                            tbm1118.ONB_MBUD = Convert.ToDecimal(txtONB_MBUD.Text);
                            mpms.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                                , tbm1118.GetType().Name.ToString(), this, "修改");
                            decimal decimalONB_MONEY = 0;
                            foreach (GridViewRow rows in GV_LoginBudget.Rows)
                            {
                                if (rows.Cells[1].Text.Equals(strOVC_ISOURCE))
                                    decimalONB_MONEY = decimal.Parse(rows.Cells[2].Text.Split(':')[1])
                                                     + decimalONB_MBUD - Convert.ToDecimal(tempONB_MBUD);
                            }

                            //加總金額到主檔
                            TBM1231 tbm1231 = new TBM1231();
                            tbm1231 = mpms.TBM1231.Where(table => table.OVC_PURCH.Equals(strPurchNum)
                                                        && table.OVC_ISOURCE.Equals(strOVC_ISOURCE)).FirstOrDefault();
                            tbm1231.ONB_MONEY = decimalONB_MONEY;
                            mpms.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                                    , tbm1231.GetType().Name.ToString(), this, "修改");
                            AddMoneyToMain();
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                        }

                    }
                    catch
                    {
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "欲修改金額以外項目，請刪除後直接新增");
                    }


                }
                else
                {
                    if (strMessage.Equals(string.Empty))
                    {
                        TBM1118 tbm1118 = new TBM1118();
                        tbm1118.OVC_PURCH = strPurchNum;
                        tbm1118.OVC_ISOURCE = strOVC_ISOURCE;
                        tbm1118.OVC_POI_IBDG = strOVC_POI_IBDG;
                        tbm1118.OVC_IKIND = strOVC_IKIND;
                        tbm1118.OVC_PJNAME = strOVC_PJNAME;
                        tbm1118.OVC_YY = strOVC_YY;
                        tbm1118.OVC_MM = strOVC_MM;
                        tbm1118.ONB_MBUD = decimalONB_MBUD;
                        tbm1118.OVC_CURRENT = strOVC_CURRENT;
                        tbm1118.ONB_RATE = decimalONB_RATE;
                        mpms.TBM1118.Add(tbm1118);
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                                , tbm1118.GetType().Name.ToString(), this, "新增");
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功");

                        decimal decimalONB_MONEY = 0;
                        foreach (GridViewRow rows in GV_LoginBudget.Rows)
                        {
                            if (rows.Cells[1].Text.Equals(strOVC_ISOURCE))
                                decimalONB_MONEY = decimal.Parse(rows.Cells[2].Text.Split(':')[1])
                                                 + decimalONB_MBUD;
                        }

                        //加總金額到主檔
                        TBM1231 tbm1231 = new TBM1231();
                        tbm1231 = mpms.TBM1231.Where(table => table.OVC_PURCH.Equals(strPurchNum)
                                                    && table.OVC_ISOURCE.Equals(strOVC_ISOURCE)).FirstOrDefault();
                        tbm1231.ONB_MONEY = decimalONB_MONEY;
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                                , tbm1231.GetType().Name.ToString(), this, "修改");
                        //加總金額到1301
                        AddMoneyToMain();
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                }



                dataImport();
                dataImport_YYMM();
                if (GV_LoginBudget.Rows.Count > 0)
                    SumMONEY(GV_LoginBudget, 2, "預算合計:", 4);
                if (GV_BudgetAllocation.Rows.Count > 0)
                    SumMONEY(GV_BudgetAllocation, 5, "目前已預劃:", 6);
            }
            catch
            {
                strMessage += "<p> 請先 新增款源 </p>";
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }

        }

        protected void btnDelMain_Click(object sender, EventArgs e)
        {
            //刪除款源按鈕
            Button btn = (Button)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            string strOVC_ISOURCE = gvr.Cells[1].Text;

            //刪除細項
            
            var tbm1118 = mpms.TBM1118
                     .Where(table => table.OVC_PURCH.Equals(strPurchNum)
                      && table.OVC_ISOURCE.Equals(strOVC_ISOURCE));

            TBM1231 tbm1231 = new TBM1231();
            tbm1231 = mpms.TBM1231
                      .Where(table => table.OVC_PURCH.Equals(strPurchNum)
                      && table.OVC_ISOURCE.Equals(strOVC_ISOURCE)).FirstOrDefault();

            if (tbm1231 != null)
            {
                //刪除款源
                if (tbm1118.Any())
                {
                    mpms.TBM1118.RemoveRange(tbm1118);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                    , tbm1118.GetType().Name.ToString(), this, "刪除");
                }
                mpms.Entry(tbm1231).State = EntityState.Deleted;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , tbm1231.GetType().Name.ToString(), this, "刪除");
                AddMoneyToMain();
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "刪除失敗");
            }

            dataImport();
            dataImport_YYMM();
            if (GV_LoginBudget.Rows.Count > 0)
                SumMONEY(GV_LoginBudget, 2, "預算合計:", 4);
            if (GV_BudgetAllocation.Rows.Count > 0)
                SumMONEY(GV_BudgetAllocation, 5, "目前已預劃:", 6);
        }

        protected void btnDelDetail_Click(object sender, EventArgs e)
        {
            //刪除預劃項目按鈕 
            Button btn = (Button)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            string strOVC_ISOURCE = gvr.Cells[1].Text;
            string strOVC_POI_IBDG = gvr.Cells[2].Text.Split(':')[0];
            string strOVC_YY = gvr.Cells[3].Text;
            string strOVC_MM = gvr.Cells[4].Text;
            decimal decimalONB_MBUD = decimal.Parse(gvr.Cells[5].Text);
            decimal decimalONB_MONEY = 0;

            //從主檔減掉這筆金額 (修改)
            foreach(GridViewRow rows in GV_LoginBudget.Rows)
            {
                if(rows.Cells[1].Text.Equals(strOVC_ISOURCE))
                {
                    decimalONB_MONEY = decimal.Parse(rows.Cells[2].Text.Split(':')[1]) - decimalONB_MBUD;
                }
            }
            
            TBM1231 tbm1231 = new TBM1231();
            tbm1231 = mpms.TBM1231
                      .Where(table => table.OVC_PURCH.Equals(strPurchNum)
                      && table.OVC_ISOURCE.Equals(strOVC_ISOURCE)).FirstOrDefault();
            if(tbm1231 != null)
            {
                tbm1231.ONB_MONEY = decimalONB_MONEY;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                    , tbm1231.GetType().Name.ToString(), this, "修改");
                //刪除此筆預劃
                TBM1118 tbm1118 = new TBM1118();
                tbm1118 = mpms.TBM1118
                         .Where(table => table.OVC_PURCH.Equals(strPurchNum)
                         && table.OVC_ISOURCE.Equals(strOVC_ISOURCE)
                         && table.OVC_POI_IBDG.Equals(strOVC_POI_IBDG)
                         && table.OVC_YY.Equals(strOVC_YY)
                         && table.OVC_MM.Equals(strOVC_MM)
                         ).FirstOrDefault();
                //刪除款源
                mpms.Entry(tbm1118).State = EntityState.Deleted;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , tbm1118.GetType().Name.ToString(), this, "刪除");
                AddMoneyToMain();
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "刪除失敗");
            }
            
            dataImport();
            dataImport_YYMM();
            if (GV_LoginBudget.Rows.Count > 0)
                SumMONEY(GV_LoginBudget, 2, "預算合計:", 4);
            if (GV_BudgetAllocation.Rows.Count > 0)
                SumMONEY(GV_BudgetAllocation, 5, "目前已預劃:", 6);

        }

        protected void btn_changeDetail_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            HiddenField hid = (HiddenField)gvr.Cells[0].FindControl("hidKind");            
            string strOVC_ISOURCE = gvr.Cells[1].Text;
            string strOVC_POI_IBDG = gvr.Cells[2].Text.Split(':')[0];
            string strOVC_YY = gvr.Cells[3].Text;
            string strOVC_MM = gvr.Cells[4].Text;
            string strMoney = gvr.Cells[5].Text;
            drpOVC_ISOURCE.SelectedValue = strOVC_ISOURCE;
            if (hid.Value.Equals("1"))
            {
                drpOVC_POI_IBDG_2.SelectedValue = strOVC_POI_IBDG;
            }
            else
            {
                txtOVC_PJNAME.Text = strOVC_POI_IBDG;
            }
            drpOVC_YY.SelectedValue = strOVC_YY;
            drpOVC_MM.SelectedValue = strOVC_MM;
            txtONB_MBUD.Text = strMoney;
        }

        protected void btn_change_Click(object sender, EventArgs e)
        {
            //異動按鈕
            btnModify.Visible = true;
            Button btn = (Button)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            txtOVC_POI_IBDG.Text = gvr.Cells[1].Text;
            dataImport_modify();
        }
        #endregion

    }
}