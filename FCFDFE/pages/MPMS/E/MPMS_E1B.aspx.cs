using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using Microsoft.International.Formatters;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using Xceed.Words.NET;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E1B : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (Session["rowtext"] != null && Session["cst"] != null)
                {
                    if (!IsPostBack)
                    {
                        FCommon.Controls_Attributes("readonly", "true", txtOVC_DRECEIVE, txtOVC_DINSPECT_BEGINT, txtOVC_DINSPECT_END);
                        TB_dataImport();
                    }
                }
                else
                    FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
            }
        }

        #region Click

        #region 清除日期btn
        protected void btnResetOVC_DRECEIVE_Click(object sender, EventArgs e)
        {
            txtOVC_DRECEIVE.Text = "";
        }
        protected void btnResetOVC_DINSPECT_BEGIN_Click(object sender, EventArgs e)
        {
            txtOVC_DINSPECT_BEGINT.Text = "";
        }
        protected void btnResetOVC_DINSPECT_END_Click(object sender, EventArgs e)
        {
            txtOVC_DINSPECT_END.Text = "";
        }
        #endregion

        //回上一頁
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E1A.aspx";
            Response.Redirect(send_url);
        }

        //回主流程
        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }

        //存檔btn
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            decimal decONB_MONEY;
            bool boolONB_MONEY = FCommon.checkDecimal(txtONB_MONEY.Text, "契約金額", ref strMessage, out decONB_MONEY);
            bool boolONB_DELAY_DAYSY = FCommon.checkDecimal(txtONB_DELAY_DAYSY.Text, "逾期總天數", ref strMessage, out decimal d);
            bool boolONB_PUNISH_DAYS = FCommon.checkInt(txtONB_PUNISH_DAYS.Text, "預期應計違約全天", ref strMessage, out int n);
            bool boolONB_NO_PUNISH_DAYS = FCommon.checkInt(txtONB_NO_PUNISH_DAYS.Text, "准延天數", ref strMessage, out n);
            bool boolONB_DELAY_MONEY = FCommon.checkDecimal(txtONB_DELAY_MONEY.Text, "逾期違約金額", ref strMessage, out d);
            bool boolONB_OTHER_MONEY = FCommon.checkDecimal(txtONB_OTHER_MONEY.Text, "其他違約金", ref strMessage, out d);
            bool boolONB_ADD_MONEY_1 = FCommon.checkDecimal(txtONB_ADD_MONEY_1.Text, "增加金額", ref strMessage, out d);
            bool boolONB_ADD_MONEY_2 = FCommon.checkDecimal(txtONB_ADD_MONEY_2.Text, "增加金額", ref strMessage, out d);
            bool boolONB_MINS_MONEY_1 = FCommon.checkDecimal(txtONB_MINS_MONEY_1.Text, "減少金額", ref strMessage, out d);
            bool boolONB_MINS_MONEY_2 = FCommon.checkDecimal(txtONB_MINS_MONEY_2.Text, "減少金額", ref strMessage, out d);
            bool boolOVC_DPAY = FCommon.checkDecimal(txtOVC_DPAY.Text, "驗收扣款", ref strMessage, out d);
            bool boolONB_INSPECT_MONEY = FCommon.checkDecimal(txtONB_INSPECT_MONEY.Text, "驗收金額", ref strMessage, out d);
            bool boolONB_PAY_MONEY = FCommon.checkDecimal(txtONB_PAY_MONEY.Text, "結算總價", ref strMessage, out d);

            if (Session["isModify"] != null)
            {
                string purch_6 = Session["purch_6"].ToString();
                var query =
                    from pay_money in mpms.TBMPAY_MONEY
                    where pay_money.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    where pay_money.OVC_PURCH_6.Equals(purch_6)
                    select pay_money.ONB_TIMES;
                foreach (var q in query)
                {
                    if (q.ToString() == lblSettlementTimes.Text)
                    {
                        TBMPAY_MONEY tbmpay_money = new TBMPAY_MONEY();
                        tbmpay_money = mpms.TBMPAY_MONEY
                            .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7)) && table.OVC_PURCH_6.Equals(purch_6) && table.ONB_TIMES.Equals(q)).FirstOrDefault();
                        if (tbmpay_money != null)
                        {
                            tbmpay_money.OVC_PJNAME = txtOVC_PJNAME.Text;
                            tbmpay_money.OVC_POI_IBDG = txtOVC_POI_IBDG.Text;
                            tbmpay_money.OVC_IBDG_USE_NAME = txtOVC_IBDG_USE_NAME.Text;
                            tbmpay_money.OVC_IBDG_USE = txtOVC_IBDG_USE.Text;
                            tbmpay_money.OVC_PAY_REASON = txtOVC_PAY_REASON.Text;
                            tbmpay_money.OVC_PERFORMANCE_LIMIT = txtOVC_PERFORMANCE_LIMIT.Text;
                            tbmpay_money.OVC_DELIVERY_PLACE = txtOVC_PERFORMANCE_PLACE.Text;
                            tbmpay_money.OVC_DFINISH = txtOVC_DRECEIVE.Text;
                            tbmpay_money.OVC_DINSPECT_BEGIN = txtOVC_DINSPECT_BEGINT.Text;
                            tbmpay_money.OVC_DINSPECT_END = txtOVC_DINSPECT_END.Text;
                            if (boolONB_MONEY) tbmpay_money.ONB_MCONTRACT = decONB_MONEY; else tbmpay_money.ONB_MCONTRACT = null;
                            if (boolONB_DELAY_DAYSY) tbmpay_money.ONB_DELAY_DAYS = short.Parse(txtONB_DELAY_DAYSY.Text); else tbmpay_money.ONB_DELAY_DAYS = null;
                            if (boolONB_PUNISH_DAYS) tbmpay_money.ONB_PUNISH_DAYS = int.Parse(txtONB_PUNISH_DAYS.Text); else tbmpay_money.ONB_PUNISH_DAYS = null;
                            if (boolONB_NO_PUNISH_DAYS) tbmpay_money.ONB_NO_PUNISH_DAYS = short.Parse(txtONB_NO_PUNISH_DAYS.Text); else tbmpay_money.ONB_NO_PUNISH_DAYS = null;
                            tbmpay_money.OVC_NO_PUNISH_DAYS = txtOVC_NO_PUNISH_DAYS.Text;
                            if (boolONB_DELAY_MONEY) tbmpay_money.ONB_DELAY_MONEY = decimal.Parse(txtONB_DELAY_MONEY.Text); else tbmpay_money.ONB_DELAY_MONEY = null;
                            if (boolONB_OTHER_MONEY) tbmpay_money.ONB_OTHER_MONEY = decimal.Parse(txtONB_OTHER_MONEY.Text); else tbmpay_money.ONB_OTHER_MONEY = null;
                            if (boolONB_ADD_MONEY_1) tbmpay_money.ONB_ADD_MONEY_1 = decimal.Parse(txtONB_ADD_MONEY_1.Text); else tbmpay_money.ONB_ADD_MONEY_1 = null;
                            tbmpay_money.OVC_ADD_ACCORDING_1 = txtOVC_ADD_ACCORDING_1.Text;
                            if (boolONB_ADD_MONEY_2) tbmpay_money.ONB_ADD_MONEY_2 = decimal.Parse(txtONB_ADD_MONEY_2.Text); else tbmpay_money.ONB_ADD_MONEY_2 = null;
                            tbmpay_money.OVC_ADD_ACCORDING_2 = txtOVC_ADD_ACCORDING_2.Text;
                            if (boolONB_MINS_MONEY_1) tbmpay_money.ONB_MINS_MONEY_1 = decimal.Parse(txtONB_MINS_MONEY_1.Text); else tbmpay_money.ONB_MINS_MONEY_1 = null;
                            tbmpay_money.OVC_MINS_ACCORDING_1 = txtOVC_MINS_ACCORDING_1.Text;
                            if (boolONB_MINS_MONEY_2) tbmpay_money.ONB_MINS_MONEY_2 = decimal.Parse(txtONB_MINS_MONEY_2.Text); else tbmpay_money.ONB_MINS_MONEY_2 = null;
                            tbmpay_money.OVC_MINS_ACCORDING_2 = txtOVC_MINS_ACCORDING_2.Text;
                            if (boolOVC_DPAY) tbmpay_money.ONB_MINS_MONEY = decimal.Parse(txtOVC_DPAY.Text); else tbmpay_money.ONB_MINS_MONEY = null;
                            if (boolONB_INSPECT_MONEY) tbmpay_money.ONB_INSPECT_MONEY = decimal.Parse(txtONB_INSPECT_MONEY.Text); else tbmpay_money.ONB_INSPECT_MONEY = null;
                            if (boolONB_PAY_MONEY) tbmpay_money.ONB_PAY_MONEY = decimal.Parse(txtONB_PAY_MONEY.Text); else tbmpay_money.ONB_PAY_MONEY = null;
                            tbmpay_money.OVC_ADVICE = txtOVC_ADVICE.Text;
                            tbmpay_money.OVC_SUMMARY = txtOVC_SUMMARY.Text;
                        }
                        if (strMessage == "")
                        {
                            mpms.SaveChanges();
                            TB_dataImport();
                            Session["isModify"] = "1";
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                        }
                        else
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                    }
                }
            }
            else
            {
                string purch_6 = Session["purch_6"].ToString();
                TBMPAY_MONEY tbmpay_money_new = new TBMPAY_MONEY();
                var queryNew =
                    from tbm1302 in mpms.TBM1302
                    where tbm1302.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    where tbm1302.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        OVC_PURCH = tbm1302.OVC_PURCH,
                        OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                        OVC_VEN_CST = tbm1302.OVC_VEN_CST
                    };
                foreach (var q in queryNew)
                {
                    tbmpay_money_new.OVC_PURCH = q.OVC_PURCH;
                    tbmpay_money_new.OVC_PURCH_6 = q.OVC_PURCH_6;
                    tbmpay_money_new.OVC_VEN_CST = q.OVC_VEN_CST;
                }
                tbmpay_money_new.ONB_TIMES = short.Parse(lblSettlementTimes.Text);
                tbmpay_money_new.OVC_PJNAME = txtOVC_PJNAME.Text;
                tbmpay_money_new.OVC_POI_IBDG = txtOVC_POI_IBDG.Text;
                tbmpay_money_new.OVC_IBDG_USE_NAME = txtOVC_IBDG_USE_NAME.Text;
                tbmpay_money_new.OVC_IBDG_USE = txtOVC_IBDG_USE.Text;
                tbmpay_money_new.OVC_PAY_REASON = txtOVC_PAY_REASON.Text;
                if (boolONB_MONEY) tbmpay_money_new.ONB_MCONTRACT = decONB_MONEY; else tbmpay_money_new.ONB_MCONTRACT = null;
                tbmpay_money_new.OVC_PERFORMANCE_LIMIT = txtOVC_PERFORMANCE_LIMIT.Text;
                tbmpay_money_new.OVC_DELIVERY_PLACE = txtOVC_PERFORMANCE_PLACE.Text;
                tbmpay_money_new.OVC_DFINISH = txtOVC_DRECEIVE.Text;
                tbmpay_money_new.OVC_DINSPECT_BEGIN = txtOVC_DINSPECT_BEGINT.Text;
                tbmpay_money_new.OVC_DINSPECT_END = txtOVC_DINSPECT_END.Text;
                if (boolONB_DELAY_DAYSY) tbmpay_money_new.ONB_DELAY_DAYS = short.Parse(txtONB_DELAY_DAYSY.Text); else tbmpay_money_new.ONB_DELAY_DAYS = null;
                if (boolONB_PUNISH_DAYS) tbmpay_money_new.ONB_PUNISH_DAYS = int.Parse(txtONB_PUNISH_DAYS.Text); else tbmpay_money_new.ONB_PUNISH_DAYS = null;
                if (boolONB_NO_PUNISH_DAYS) tbmpay_money_new.ONB_NO_PUNISH_DAYS = short.Parse(txtONB_NO_PUNISH_DAYS.Text); else tbmpay_money_new.ONB_NO_PUNISH_DAYS = null;
                tbmpay_money_new.OVC_NO_PUNISH_DAYS = txtOVC_NO_PUNISH_DAYS.Text;
                if (boolONB_DELAY_MONEY) tbmpay_money_new.ONB_DELAY_MONEY = decimal.Parse(txtONB_DELAY_MONEY.Text); else tbmpay_money_new.ONB_DELAY_MONEY = null;
                if (boolONB_OTHER_MONEY) tbmpay_money_new.ONB_OTHER_MONEY = decimal.Parse(txtONB_OTHER_MONEY.Text); else tbmpay_money_new.ONB_OTHER_MONEY = null;
                if (boolONB_ADD_MONEY_1) tbmpay_money_new.ONB_ADD_MONEY_1 = decimal.Parse(txtONB_ADD_MONEY_1.Text); else tbmpay_money_new.ONB_ADD_MONEY_1 = null;
                tbmpay_money_new.OVC_ADD_ACCORDING_1 = txtOVC_ADD_ACCORDING_1.Text;
                if (boolONB_ADD_MONEY_2) tbmpay_money_new.ONB_ADD_MONEY_2 = decimal.Parse(txtONB_ADD_MONEY_2.Text); else tbmpay_money_new.ONB_ADD_MONEY_2 = null;
                tbmpay_money_new.OVC_ADD_ACCORDING_2 = txtOVC_ADD_ACCORDING_2.Text;
                if (boolONB_MINS_MONEY_1) tbmpay_money_new.ONB_MINS_MONEY_1 = decimal.Parse(txtONB_MINS_MONEY_1.Text); else tbmpay_money_new.ONB_MINS_MONEY_1 = null;
                tbmpay_money_new.OVC_MINS_ACCORDING_1 = txtOVC_MINS_ACCORDING_1.Text;
                if (boolONB_MINS_MONEY_2) tbmpay_money_new.ONB_MINS_MONEY_2 = decimal.Parse(txtONB_MINS_MONEY_2.Text); else tbmpay_money_new.ONB_MINS_MONEY_2 = null;
                tbmpay_money_new.OVC_MINS_ACCORDING_2 = txtOVC_MINS_ACCORDING_2.Text;
                if (boolOVC_DPAY) tbmpay_money_new.ONB_MINS_MONEY = decimal.Parse(txtOVC_DPAY.Text); else tbmpay_money_new.ONB_MINS_MONEY = null;
                if (boolONB_INSPECT_MONEY) tbmpay_money_new.ONB_INSPECT_MONEY = decimal.Parse(txtONB_INSPECT_MONEY.Text); else tbmpay_money_new.ONB_INSPECT_MONEY = null;
                if (boolONB_PAY_MONEY) tbmpay_money_new.ONB_PAY_MONEY = decimal.Parse(txtONB_PAY_MONEY.Text); else tbmpay_money_new.ONB_PAY_MONEY = null;
                tbmpay_money_new.OVC_ADVICE = txtOVC_ADVICE.Text;
                tbmpay_money_new.OVC_SUMMARY = txtOVC_SUMMARY.Text;
                if (strMessage == "")
                {
                    mpms.TBMPAY_MONEY.Add(tbmpay_money_new);
                    mpms.SaveChanges();
                    Session["isModify"] = "1";
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
        }

        #region 報表
        //結算驗收證明書預覽列印(簽)
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            PaymentPurchaseServlet_ExportToWord("WordPDFprint/結算驗收證明書E1B.docx");
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/SA_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "結算驗收證明書.docx";
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
        protected void LinkButton5_Click(object sender, EventArgs e)
        {
            PaymentPurchaseServlet_ExportToWord("WordPDFprint/結算驗收證明書E1B.docx");
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/SA_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/SA_Temp.pdf";
            string fileName = purch + "結算驗收證明書.pdf";
            FCommon.WordToPDF(this, path_temp, wordfilepath, fileName);
        }
        protected void LinkButton9_Click(object sender, EventArgs e)
        {
            PaymentPurchaseServlet_ExportToWord("WordPDFprint/結算驗收證明書E1B.docx");
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/SA_Temp.docx");
            string wordfilepath = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/SA_Temp.odt");
            string fileName = purch + "結算驗收證明書.odt";
            FCommon.WordToOdt(this, path_temp, wordfilepath, fileName);
        }
        // 結算驗收證明書預覽列印
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            PaymentPurchaseServlet_ExportToWord("WordPDFprint/PaymentPurchaseServletE1B_2.docx");
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/SA_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "結算驗收證明書.docx";
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
        protected void LinkButton6_Click(object sender, EventArgs e)
        {
            PaymentPurchaseServlet_ExportToWord("WordPDFprint/PaymentPurchaseServletE1B_2.docx");
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/SA_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/SA_Temp.pdf";
            string fileName = purch + "結算驗收證明書.pdf";
            FCommon.WordToPDF(this, path_temp, wordfilepath, fileName);
        }
        protected void LinkButton10_Click(object sender, EventArgs e)
        {
            PaymentPurchaseServlet_ExportToWord("WordPDFprint/PaymentPurchaseServletE1B_2.docx");
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/SA_Temp.docx");
            string wordfilepath = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/SA_Temp.odt");
            string fileName = purch + "結算驗收證明書.odt";
            FCommon.WordToOdt(this, path_temp, wordfilepath, fileName);
        }
        //單據黏貼預覽列印
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            ExpenseCertificate_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/DP_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "單據黏貼.docx";
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
        protected void LinkButton7_Click(object sender, EventArgs e)
        {
            ExpenseCertificate_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/DP_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/DP_Temp.pdf";
            string fileName = purch + "單據黏貼.pdf";
            FCommon.WordToPDF(this, path_temp, wordfilepath, fileName);
        }
        protected void LinkButton11_Click(object sender, EventArgs e)
        {
            ExpenseCertificate_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/DP_Temp.docx");
            string wordfilepath = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/DP_Temp.odt");
            string fileName = purch + "單據黏貼.odt";
            FCommon.WordToOdt(this, path_temp, wordfilepath, fileName);
        }
        //減價收簽辦單預覽列印
        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            ExpenseCertificate2_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/RPR_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "減價收簽辦單.docx";
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
        protected void LinkButton8_Click(object sender, EventArgs e)
        {
            ExpenseCertificate2_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/RPR_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/RPR_Temp.pdf";
            string fileName = purch + "減價收簽辦單.pdf";
            FCommon.WordToPDF(this, path_temp, wordfilepath, fileName);
        }
        protected void LinkButton12_Click(object sender, EventArgs e)
        {
            ExpenseCertificate2_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/RPR_Temp.docx");
            string wordfilepath = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/RPR_Temp.odt");
            string fileName = purch + "減價收簽辦單.odt";
            FCommon.WordToOdt(this, path_temp, wordfilepath, fileName);
        }
        #endregion

        #endregion

        #region 副程式

        #region Table資料帶入
        private void TB_dataImport()
        {
            if (Session["rowtext"] != null && Session["purch6"] != null && Session["cst"] != null)
            {
                var purch = Session["rowtext"].ToString().Substring(0, 7);
                string purch_6 = Session["purch_6"].ToString();
                var cst = Session["cst"].ToString();
                var queryMid =
                    from tbm1301 in mpms.TBM1301_PLAN
                    join tbm1302 in mpms.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                    where tbm1301.OVC_PURCH.Equals(purch)
                    where tbm1302.OVC_PURCH_6.Equals(purch_6)
                    where tbm1302.OVC_VEN_CST.Equals(cst)
                    select new
                    {
                        OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                        OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE,
                        OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                        OVC_DCONTRACT = tbm1302.OVC_DCONTRACT,
                        OVC_DBID = tbm1302.OVC_DBID
                    };
                foreach (var q in queryMid)
                {
                    lblOVC_PUR_IPURCH_ENG.Text = q.OVC_PUR_IPURCH;
                    lblOVC_PURCH.Text = Session["rowtext"].ToString();
                    lblOVC_VEN_TITLE.Text = q.OVC_VEN_TITLE;
                    lblOVC_USER_FAX.Text = q.OVC_PUR_NSECTION;
                    lblSignedDate.Text = q.OVC_DCONTRACT;
                    lblOVC_DBID.Text = q.OVC_DBID;
                }
                if (Session["isModify"] != null)
                {
                    var query =
                        from tbmpay in mpms.TBMPAY_MONEY
                        where tbmpay.OVC_PURCH.Equals(purch)
                        where tbmpay.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            OVC_DPAY = tbmpay.OVC_DPAY,
                            ONB_TIMES = tbmpay.ONB_TIMES,
                            OVC_POI_IBDG = tbmpay.OVC_POI_IBDG,
                            OVC_PJNAME = tbmpay.OVC_PJNAME,
                            OVC_IBDG_USE_NAME = tbmpay.OVC_IBDG_USE_NAME,
                            OVC_IBDG_USE = tbmpay.OVC_IBDG_USE,
                            OVC_PAY_REASON = tbmpay.OVC_PAY_REASON,
                            ONB_MCONTRACT = tbmpay.ONB_MCONTRACT,
                            OVC_PERFORMANCE_LIMIT = tbmpay.OVC_PERFORMANCE_LIMIT,
                            OVC_DELIVERY_PLACE = tbmpay.OVC_DELIVERY_PLACE,
                            OVC_DFINISH = tbmpay.OVC_DFINISH,
                            OVC_DINSPECT_BEGIN = tbmpay.OVC_DINSPECT_BEGIN,
                            OVC_DINSPECT_END = tbmpay.OVC_DINSPECT_END,
                            ONB_DELAY_DAYS = tbmpay.ONB_DELAY_DAYS,
                            ONB_PUNISH_DAYS = tbmpay.ONB_PUNISH_DAYS,
                            ONB_NO_PUNISH_DAYS = tbmpay.ONB_NO_PUNISH_DAYS,
                            OVC_NO_PUNISH_DAYS = tbmpay.OVC_NO_PUNISH_DAYS,
                            ONB_DELAY_MONEY = tbmpay.ONB_DELAY_MONEY,
                            ONB_OTHER_MONEY = tbmpay.ONB_OTHER_MONEY,
                            ONB_ADD_MONEY_1 = tbmpay.ONB_ADD_MONEY_1,
                            OVC_ADD_ACCORDING_1 = tbmpay.OVC_ADD_ACCORDING_1,
                            ONB_ADD_MONEY_2 = tbmpay.ONB_ADD_MONEY_2,
                            OVC_ADD_ACCORDING_2 = tbmpay.OVC_ADD_ACCORDING_2,
                            ONB_MINS_MONEY_1 = tbmpay.ONB_MINS_MONEY_1,
                            OVC_MINS_ACCORDING_1 = tbmpay.OVC_MINS_ACCORDING_1,
                            ONB_MINS_MONEY_2 = tbmpay.ONB_MINS_MONEY_2,
                            OVC_MINS_ACCORDING_2 = tbmpay.OVC_MINS_ACCORDING_2,
                            ONB_MINS_MONEY = tbmpay.ONB_MINS_MONEY,
                            ONB_INSPECT_MONEY = tbmpay.ONB_INSPECT_MONEY,
                            ONB_PAY_MONEY = tbmpay.ONB_PAY_MONEY,
                            OVC_ADVICE = tbmpay.OVC_ADVICE,
                            OVC_SUMMARY = tbmpay.OVC_SUMMARY
                        };
                    foreach (var q in query)
                    {
                        if (Session["ONB_TIMES"] != null && (Session["ONB_TIMES"].ToString() == q.ONB_TIMES.ToString()))
                        {
                            lblOVC_DPAY.Text = q.OVC_DPAY;
                            lblSettlementTimes.Text = q.ONB_TIMES.ToString();
                            txtOVC_PJNAME.Text = q.OVC_PJNAME;
                            txtOVC_POI_IBDG.Text = q.OVC_POI_IBDG;
                            txtOVC_IBDG_USE_NAME.Text = q.OVC_IBDG_USE_NAME;
                            txtOVC_IBDG_USE.Text = q.OVC_IBDG_USE;
                            txtOVC_PAY_REASON.Text = q.OVC_PAY_REASON;
                            txtONB_MONEY.Text = String.Format("{0:N}", q.ONB_MCONTRACT ?? 0);
                            txtOVC_PERFORMANCE_LIMIT.Text = q.OVC_PERFORMANCE_LIMIT;
                            txtOVC_PERFORMANCE_PLACE.Text = q.OVC_DELIVERY_PLACE;
                            txtOVC_DRECEIVE.Text = q.OVC_DFINISH;
                            txtOVC_DINSPECT_BEGINT.Text = q.OVC_DINSPECT_BEGIN;
                            txtOVC_DINSPECT_END.Text = q.OVC_DINSPECT_END;
                            txtONB_DELAY_DAYSY.Text = q.ONB_DELAY_DAYS.ToString();
                            txtONB_PUNISH_DAYS.Text = q.ONB_PUNISH_DAYS.ToString();
                            txtONB_NO_PUNISH_DAYS.Text = q.ONB_NO_PUNISH_DAYS.ToString();
                            txtOVC_NO_PUNISH_DAYS.Text = q.OVC_NO_PUNISH_DAYS;
                            txtONB_DELAY_MONEY.Text = String.Format("{0:N}", q.ONB_DELAY_MONEY ?? 0);
                            txtONB_OTHER_MONEY.Text = String.Format("{0:N}", q.ONB_OTHER_MONEY ?? 0);
                            txtONB_ADD_MONEY_1.Text = String.Format("{0:N}", q.ONB_ADD_MONEY_1 ?? 0);
                            txtOVC_ADD_ACCORDING_1.Text = q.OVC_ADD_ACCORDING_1;
                            txtONB_ADD_MONEY_2.Text = String.Format("{0:N}", q.ONB_ADD_MONEY_2 ?? 0);
                            txtOVC_ADD_ACCORDING_2.Text = q.OVC_ADD_ACCORDING_2;
                            txtONB_MINS_MONEY_1.Text = String.Format("{0:N}", q.ONB_MINS_MONEY_1 ?? 0);
                            txtOVC_MINS_ACCORDING_1.Text = q.OVC_MINS_ACCORDING_1;
                            txtONB_MINS_MONEY_2.Text = String.Format("{0:N}", q.ONB_MINS_MONEY_2 ?? 0);
                            txtOVC_MINS_ACCORDING_2.Text = q.OVC_MINS_ACCORDING_2;
                            txtOVC_DPAY.Text = q.ONB_MINS_MONEY.ToString();
                            txtONB_INSPECT_MONEY.Text = String.Format("{0:N}", q.ONB_INSPECT_MONEY ?? 0);
                            txtONB_PAY_MONEY.Text = String.Format("{0:N}", q.ONB_PAY_MONEY ?? 0);
                            txtOVC_ADVICE.Text = q.OVC_ADVICE;
                            txtOVC_SUMMARY.Text = q.OVC_SUMMARY;
                            Total1.Text = String.Format("{0:N}", (q.ONB_ADD_MONEY_1 + q.ONB_ADD_MONEY_2) ?? 0);
                            Total2.Text = String.Format("{0:N}", (q.ONB_MINS_MONEY_1 + q.ONB_MINS_MONEY_2) ?? 0);
                        }
                    }
                    var queryLab =
                       from tbm1301 in mpms.TBM1301
                       join tbmreceive in mpms.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmreceive.OVC_PURCH
                       where tbm1301.OVC_PURCH.Equals(purch)
                       where tbmreceive.OVC_PURCH_6.Equals(purch_6)
                       select new
                       {
                           OVC_LAB = tbm1301.OVC_LAB,
                           ONB_MONEY = tbmreceive.ONB_MONEY
                       };
                    foreach (var q in queryLab)
                    {
                        if (q.OVC_LAB == "1")
                        {
                            if (q.ONB_MONEY < 1000000)
                            {
                                chkOVC_BUDGET_BUY.Items[0].Selected = true;
                                chkOVC_BUDGET_BUY.Items[1].Selected = false;
                                chkOVC_BUDGET_BUY.Items[2].Selected = false;
                                chkOVC_BUDGET_BUY.Items[3].Selected = false;
                            }
                            else if (q.ONB_MONEY >= 1000000 && q.ONB_MONEY < 10000000)
                            {
                                chkOVC_BUDGET_BUY.Items[0].Selected = false;
                                chkOVC_BUDGET_BUY.Items[1].Selected = true;
                                chkOVC_BUDGET_BUY.Items[2].Selected = false;
                                chkOVC_BUDGET_BUY.Items[3].Selected = false;
                            }
                            else if (q.ONB_MONEY >= 10000000 && q.ONB_MONEY < 20000000)
                            {
                                chkOVC_BUDGET_BUY.Items[0].Selected = false;
                                chkOVC_BUDGET_BUY.Items[1].Selected = false;
                                chkOVC_BUDGET_BUY.Items[2].Selected = true;
                                chkOVC_BUDGET_BUY.Items[3].Selected = false;
                            }
                            else if (q.ONB_MONEY >= 20000000)
                            {
                                chkOVC_BUDGET_BUY.Items[0].Selected = false;
                                chkOVC_BUDGET_BUY.Items[1].Selected = false;
                                chkOVC_BUDGET_BUY.Items[2].Selected = false;
                                chkOVC_BUDGET_BUY.Items[3].Selected = true;
                            }
                        }
                        else
                        {
                            if (q.ONB_MONEY < 1000000)
                            {
                                chkOVC_BUDGET_BUY.Items[0].Selected = true;
                                chkOVC_BUDGET_BUY.Items[1].Selected = false;
                                chkOVC_BUDGET_BUY.Items[2].Selected = false;
                                chkOVC_BUDGET_BUY.Items[3].Selected = false;
                            }
                            else if (q.ONB_MONEY >= 1000000 && q.ONB_MONEY < 50000000)
                            {
                                chkOVC_BUDGET_BUY.Items[0].Selected = false;
                                chkOVC_BUDGET_BUY.Items[1].Selected = true;
                                chkOVC_BUDGET_BUY.Items[2].Selected = false;
                                chkOVC_BUDGET_BUY.Items[3].Selected = false;
                            }
                            else if (q.ONB_MONEY >= 50000000 && q.ONB_MONEY < 100000000)
                            {
                                chkOVC_BUDGET_BUY.Items[0].Selected = false;
                                chkOVC_BUDGET_BUY.Items[1].Selected = false;
                                chkOVC_BUDGET_BUY.Items[2].Selected = true;
                                chkOVC_BUDGET_BUY.Items[3].Selected = false;
                            }
                            else if (q.ONB_MONEY >= 100000000)
                            {
                                chkOVC_BUDGET_BUY.Items[0].Selected = false;
                                chkOVC_BUDGET_BUY.Items[1].Selected = false;
                                chkOVC_BUDGET_BUY.Items[2].Selected = false;
                                chkOVC_BUDGET_BUY.Items[3].Selected = true;
                            }
                        }
                    }
                }
                else
                {
                    if (Session["ONB_TIMES"] != null)
                    {
                        lblOVC_DPAY.Text = DateTime.Now.ToString("yyyy-MM-dd");
                        lblSettlementTimes.Text = Session["ONB_TIMES"].ToString();
                        if (Session["OVC_PJNAME"] != null)
                        {
                            txtOVC_POI_IBDG.Text = Session["OVC_PJNAME"].ToString();
                            var queryPJName =
                                from tbm1118 in mpms.TBM1118
                                where tbm1118.OVC_PURCH.Equals(purch)
                                where tbm1118.OVC_POI_IBDG.Equals(txtOVC_POI_IBDG.Text)
                                select new
                                {
                                    OVC_POI_IBDG = tbm1118.OVC_POI_IBDG,
                                    OVC_PJNAME = tbm1118.OVC_PJNAME
                                };
                            foreach (var q in queryPJName)
                                if (q.OVC_POI_IBDG == txtOVC_POI_IBDG.Text) txtOVC_PJNAME.Text = q.OVC_PJNAME;
                        }
                    }
                }
                var queryCurrency =
                    from tbm1302 in mpms.TBM1302
                    join tbm1407 in mpms.TBM1407 on tbm1302.OVC_CURRENT equals tbm1407.OVC_PHR_ID
                    where tbm1302.OVC_PURCH.Equals(purch)
                    where tbm1302.OVC_PURCH_6.Equals(purch_6)
                    where tbm1302.OVC_VEN_CST.Equals(cst)
                    where tbm1407.OVC_PHR_CATE.Equals("B0")
                    select tbm1407.OVC_PHR_DESC;
                foreach (var q in queryCurrency)
                    lblOVC_CURRENT.Text = q;
            }
            lblSignedDate.Text = dateTW(lblSignedDate.Text);
            lblOVC_DBID.Text = dateTW(lblOVC_DBID.Text);
        }
        #endregion

        #region 結算驗收證明書預覽列印
        void PaymentPurchaseServlet_ExportToWord(string url)
        {
            string Money = "";
            string path = "";
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            path = Path.Combine(Request.PhysicalApplicationPath, url);
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryNSECTION = mpms.TBM1301
                        .Where(o => o.OVC_PURCH.Equals(purch)).FirstOrDefault();
                    doc.ReplaceText("[$OVC_PUR_NSECTION$]", queryNSECTION != null ? queryNSECTION.OVC_PUR_NSECTION : "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$NAME$]", lblOVC_PUR_IPURCH_ENG.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$PURCH$]", lblOVC_PURCH.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$VEN$]", lblOVC_VEN_TITLE.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$Currency$]", lblOVC_CURRENT.Text != "" ? lblOVC_CURRENT.Text : "新臺幣", false, System.Text.RegularExpressions.RegexOptions.None);
                    if (decimal.TryParse(txtONB_MONEY.Text, out decimal n))
                    {
                        decimal money = decimal.Parse(txtONB_MONEY.Text);
                        if (txtONB_MONEY.Text != "")
                            doc.ReplaceText("[$Money$]", String.Format("{0:N}", money), false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$Money$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$CHK1$]", chkOVC_BUDGET_BUY.Items[0].Selected == true ? "■" : "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$CHK2$]", chkOVC_BUDGET_BUY.Items[1].Selected == true ? "■" : "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$CHK3$]", chkOVC_BUDGET_BUY.Items[2].Selected == true ? "■" : "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$CHK4$]", chkOVC_BUDGET_BUY.Items[3].Selected == true ? "■" : "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$Deadline$]", dateTW(txtOVC_PERFORMANCE_LIMIT.Text), false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$Place$]", txtOVC_PERFORMANCE_PLACE.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$Complete$]", dateTW(txtOVC_DRECEIVE.Text), false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$Start$]", dateTW(txtOVC_DINSPECT_BEGINT.Text), false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$End$]", dateTW(txtOVC_DINSPECT_END.Text), false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$DELAY_DAYSY$]", txtONB_DELAY_DAYSY.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$NO_PUNISH_DAYS$]", txtONB_NO_PUNISH_DAYS.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_NO_PUNISH_DAYS$]", txtOVC_NO_PUNISH_DAYS.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$PUNISH_DAYS$]", txtONB_PUNISH_DAYS.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$DELAY_MONEY$]", txtONB_DELAY_MONEY.Text != "" ? txtONB_DELAY_MONEY.Text : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OTHER_MONEY$]", txtONB_OTHER_MONEY.Text != "" ? txtONB_OTHER_MONEY.Text : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$ADD_MONEY_1$]", txtONB_ADD_MONEY_1.Text != "" ? txtONB_ADD_MONEY_1.Text : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$ADD_ACCORDING_1$]", txtOVC_ADD_ACCORDING_1.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$ADD_MONEY_2$]", txtONB_ADD_MONEY_2.Text != "" ? txtONB_ADD_MONEY_2.Text : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$ADD_ACCORDING_2$]", txtOVC_ADD_ACCORDING_2.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$MINS_MONEY_1$]", txtONB_MINS_MONEY_1.Text != "" ? txtONB_MINS_MONEY_1.Text : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$MINS_ACCORDING_1$]", txtOVC_MINS_ACCORDING_1.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$MINS_MONEY_2$]", txtONB_MINS_MONEY_2.Text != "" ? txtONB_MINS_MONEY_2.Text : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$MINS_ACCORDING_2$]", txtOVC_MINS_ACCORDING_2.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$Total_1$]", Total1.Text != "" ? Total1.Text : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$Total_2$]", Total2.Text != "" ? Total2.Text : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_DPAY$]", txtOVC_DPAY.Text != "" ? txtOVC_DPAY.Text : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                    if (decimal.TryParse(txtONB_PAY_MONEY.Text, out decimal d))
                    {
                        decimal money = decimal.Parse(txtONB_PAY_MONEY.Text);
                        Money = EastAsiaNumericFormatter.FormatWithCulture("Lc", money, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                        doc.ReplaceText("[$Total_Money$]", Money, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$Total_Money$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_ADVICE$]", txtOVC_ADVICE.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_SUMMARY$]", txtOVC_SUMMARY.Text, false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/SA_Temp.docx");
                }
                buffer = ms.ToArray();
            }
        }
        #endregion

        #region 單據黏貼預覽列印
        void ExpenseCertificate_ExportToWord()
        {
            string Money = "";
            string path = "";
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/PaymentPurchaseServletE1B_3.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryNSECTION = mpms.TBM1301.Where(o => o.OVC_PURCH.Equals(purch)).FirstOrDefault();
                    doc.ReplaceText("[$OVC_PUR_NSECTION$]", queryNSECTION != null ? queryNSECTION.OVC_PUR_NSECTION : "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$POI_IBDG$]", txtOVC_POI_IBDG.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$PJNAME$]", txtOVC_PJNAME.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$POI_IBDG$]", txtOVC_IBDG_USE.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$IBDG_USE_NAME$]", txtOVC_IBDG_USE_NAME.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    if (decimal.TryParse(txtONB_MONEY.Text, out decimal n))
                    {
                        decimal money = decimal.Parse(txtONB_MONEY.Text);
                        if (txtONB_MONEY.Text != "")
                            doc.ReplaceText("[$MONEY$]", String.Format("{0:N}", money), false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$MONEY$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                    if (decimal.TryParse(txtONB_PAY_MONEY.Text, out n))
                    {
                        decimal money = decimal.Parse(txtONB_PAY_MONEY.Text);
                        Money = EastAsiaNumericFormatter.FormatWithCulture("Lc", money, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                        doc.ReplaceText("[$MONEY_2$]", Money, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$MONEY_2$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$PAY_REASON$]", txtOVC_PAY_REASON.Text, false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/DP_Temp.docx");
                }
                buffer = ms.ToArray();
            }
        }
        #endregion

        #region  減價收簽辦單預覽列印
        void ExpenseCertificate2_ExportToWord()
        {
            string Money = "";
            string path = "";
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/PaymentPurchaseServletE1B_4.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryNSECTION = mpms.TBM1301.Where(o => o.OVC_PURCH.Equals(purch)).FirstOrDefault();
                    doc.ReplaceText("[$OVC_PUR_NSECTION$]", queryNSECTION != null ? queryNSECTION.OVC_PUR_NSECTION : "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$PURCH$]", lblOVC_PURCH.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$NAME$]", lblOVC_PUR_IPURCH_ENG.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$UNIT$]", lblOVC_USER_FAX.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    if (int.TryParse(txtONB_MONEY.Text, out int i))
                    {
                        int m = int.Parse(txtONB_MONEY.Text);
                        string money = EastAsiaNumericFormatter.FormatWithCulture("Lc", m, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                        doc.ReplaceText("[$Total_Money$]", money, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$Total_Money$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$CHK1$]", chkOVC_BUDGET_BUY.Items[0].Selected == true ? "■" : "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$CHK2$]", chkOVC_BUDGET_BUY.Items[1].Selected == true ? "■" : "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$CHK3$]", chkOVC_BUDGET_BUY.Items[2].Selected == true ? "■" : "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$CHK4$]", chkOVC_BUDGET_BUY.Items[3].Selected == true ? "■" : "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    var query =
                        from tbm1301 in mpms.TBM1301
                        join tbm1407 in mpms.TBM1407 on tbm1301.OVC_LAB equals tbm1407.OVC_PHR_ID
                        where tbm1301.OVC_PURCH.Equals(purch)
                        where tbm1407.OVC_PHR_CATE.Equals("GN")
                        select tbm1407.OVC_PHR_ID;
                    foreach (var q in query)
                    {
                        doc.ReplaceText("[$CHK5$]", q == "1" ? "■" : "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CHK6$]", q == "1" ? "□" : "■", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$CHK5$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$CHK6$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$VEN$]", lblOVC_VEN_TITLE.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryShip =
                        from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                        where tbmreceive.OVC_PURCH.Equals(purch)
                        where tbmreceive.OVC_PURCH_6.Equals(purch_6)
                        select  new
                        {
                            ONB_SHIP_TIMES = tbmreceive.ONB_SHIP_TIMES,
                            OVC_STATUS = tbmreceive.OVC_STATUS
                        };
                    foreach (var q in queryShip)
                    {
                        doc.ReplaceText("[$SHIP_TIME$]", q.ONB_SHIP_TIMES.ToString(), false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_STATUS == "已結案")
                        {
                            doc.ReplaceText("[$CHK7$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK8$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK9$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK10$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK11$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else if (q.OVC_STATUS == "待結報")
                        {
                            doc.ReplaceText("[$CHK7$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK8$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK9$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK10$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK11$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else if (q.OVC_STATUS == "待驗收")
                        {
                            doc.ReplaceText("[$CHK7$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK8$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK9$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK10$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK11$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else if (q.OVC_STATUS == "待交貨")
                        {
                            doc.ReplaceText("[$CHK7$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK8$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK9$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK10$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK11$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                        {
                            doc.ReplaceText("[$CHK7$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK8$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK9$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK10$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CHK11$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                    {
                        doc.ReplaceText("[$CHK7$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CHK8$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CHK9$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CHK10$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CHK11$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    if (Session["username"] != null)
                    {
                        var name = Session["username"].ToString();
                        var queryTel =
                            from account in mpms.ACCOUNT
                            where account.USER_NAME.Equals(name)
                            select account.IUSER_PHONE??"";
                        foreach (var qu in queryTel)
                            doc.ReplaceText("[$TEL$]", qu, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$TEL$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/RPR_Temp.docx");
                }
                buffer = ms.ToArray();
            }
        }
        #endregion

        #region 民國年
        private string dateTW(string str)
        {
            DateTime dt;
            string strdt = "";
            if (DateTime.TryParse(str, out DateTime d))
            {
                dt = DateTime.Parse(str);
                strdt = (int.Parse(dt.Year.ToString()) - 1911).ToString() + "年" + dt.Month.ToString() + "月" + dt.Day.ToString() + "日";
                return strdt;
            }
            else
                return str;
        }
        #endregion

        #endregion
    }
}