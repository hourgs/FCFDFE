using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using Microsoft.International.Formatters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xceed.Words.NET;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E31 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            if (Session["rowtext"] != null)
            {
                if (!IsPostBack)
                {
                    TB_dataImport();
                }
            }
            else
                FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
        }

        #region Click

        //回主流程btn
        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }
        //回上一頁btn
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E22.aspx";
            Response.Redirect(send_url);
        }
        // 存檔btn
        protected void btnSave_Click(object sender, EventArgs e)
        {
            short onbTime = short.Parse(lblONB_DELIVERY_TIMES.Text);
            short onbNo;

            string strMessage = "";
            string strTextBox3 = TextBox3.Text;
            bool boolTextBox3 = FCommon.checkDecimal(strTextBox3, "購買數量", ref strMessage, out decimal decTextBox3);

            //if (strMessage.Equals(string.Empty))
            //{
                if (short.TryParse(txtONB_NO.Text, out short sh))
                {
                    onbNo = short.Parse(txtONB_NO.Text);
                    if (Session["isModify"] != null)
                    {
                        var query =
                            from tbmfree in mpms.TBMFREEDUTY
                            where tbmfree.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                            select new
                            {
                                OVC_PURCH = tbmfree.OVC_PURCH,
                                ONB_NO = tbmfree.ONB_NO
                            };
                        foreach (var q in query)
                        {
                            if (q.ONB_NO.ToString() == txtONB_NO.Text)
                            {
                                TBMFREEDUTY tbmfreeduty = new TBMFREEDUTY();
                                tbmfreeduty = mpms.TBMFREEDUTY
                                    .Where(table => table.OVC_PURCH.Equals(q.OVC_PURCH) && table.ONB_NO.Equals(q.ONB_NO) && table.OVC_DUTY_KIND.Equals("C")).FirstOrDefault();
                                if (tbmfreeduty != null)
                                {
                                    tbmfreeduty.OVC_APPLY = txtOVC_APPLY.Text;
                                    tbmfreeduty.OVC_GOOD_NAPPROVE = txtOVC_GOOD_NAPPROVE.Text;
                                    tbmfreeduty.OVC_GOOD_DESC = txtOVC_GOOD_DESC.Text;
                                    tbmfreeduty.OVC_GOOD_QUALITY = TextBox3.Text;
                                    tbmfreeduty.OVC_GOOD_DAPPLY = TextBox4.Text;
                                    tbmfreeduty.OVC_GOOD_BUDGET = txtOVC_GOOD_BUDGET.Text;
                                    tbmfreeduty.OVC_GOOD_AUDIT = txtOVC_GOOD_AUDIT.Text;
                                    tbmfreeduty.OVC_GOOD_USE = txtOVC_GOOD_USE.Text;
                                    tbmfreeduty.OVC_APPROVE_DESC = txtOVC_APPROVE_DESC.Text;
                                    tbmfreeduty.OVC_EXPORT_VENDOR = txtOVC_EXPORT_VENDOR.Text;
                                    tbmfreeduty.OVC_GOOD_NSECTION = txtOVC_GOOD_NSECTION.Text;
                                    tbmfreeduty.OVC_GOOD_IAPPLY = txtOVC_DAPPLY.Text;
                                    tbmfreeduty.OVC_GOOD_IAPPROVE = txtOVC_MEMO.Text;
                                    mpms.SaveChanges();
                                }
                            }
                        }
                        Session["isModify"] = "1";
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                    }
                    else
                    {
                        var query = mpms.TBMFREEDUTY
                               .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7)))
                               .Where(table => table.OVC_DUTY_KIND.Equals("C"))
                               .Where(table => table.ONB_TIMES.Equals(onbTime))
                               .Where(table => table.ONB_NO.Equals(onbNo)).FirstOrDefault();
                        if (query != null)
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "交貨批次及申請次數重複");
                        else
                        {
                            TBMFREEDUTY tbmfreeduty_new = new TBMFREEDUTY();
                            tbmfreeduty_new.OVC_PURCH = lblOVC_PURCH.Text.Substring(0, 7);
                            var query_new =
                                from tbm1302 in mpms.TBM1302
                                join tbmreceive in mpms.TBMRECEIVE_CONTRACT on tbm1302.OVC_PURCH equals tbmreceive.OVC_PURCH
                                where tbm1302.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                                select new
                                {
                                    OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                                    OVC_VEN_CST = tbm1302.OVC_VEN_CST,
                                    OVC_DO_NAME = tbmreceive.OVC_DO_NAME
                                };
                            foreach (var qu in query_new)
                            {
                                tbmfreeduty_new.OVC_PURCH_6 = qu.OVC_PURCH_6;
                                tbmfreeduty_new.OVC_VEN_CST = qu.OVC_VEN_CST;
                                tbmfreeduty_new.OVC_DO_NAME = qu.OVC_DO_NAME;
                            }
                            tbmfreeduty_new.ONB_TIMES = short.Parse(lblONB_DELIVERY_TIMES.Text);
                            tbmfreeduty_new.OVC_DUTY_KIND = "C";
                            tbmfreeduty_new.ONB_NO = short.Parse(txtONB_NO.Text);
                            tbmfreeduty_new.OVC_APPLY = txtOVC_APPLY.Text;
                            tbmfreeduty_new.OVC_GOOD_NAPPROVE = txtOVC_GOOD_NAPPROVE.Text;
                            tbmfreeduty_new.OVC_GOOD_DESC = txtOVC_GOOD_DESC.Text;
                            tbmfreeduty_new.OVC_GOOD_QUALITY = TextBox3.Text;
                            tbmfreeduty_new.OVC_GOOD_DAPPLY = TextBox4.Text;
                            tbmfreeduty_new.OVC_GOOD_BUDGET = txtOVC_GOOD_BUDGET.Text;
                            tbmfreeduty_new.OVC_GOOD_AUDIT = txtOVC_GOOD_AUDIT.Text;
                            tbmfreeduty_new.OVC_GOOD_USE = txtOVC_GOOD_USE.Text;
                            tbmfreeduty_new.OVC_APPROVE_DESC = txtOVC_APPROVE_DESC.Text;
                            tbmfreeduty_new.OVC_EXPORT_VENDOR = txtOVC_EXPORT_VENDOR.Text;
                            tbmfreeduty_new.OVC_GOOD_NSECTION = txtOVC_GOOD_NSECTION.Text;
                            tbmfreeduty_new.OVC_GOOD_IAPPLY = txtOVC_DAPPLY.Text;
                            tbmfreeduty_new.OVC_GOOD_IAPPROVE = txtOVC_MEMO.Text;
                            mpms.TBMFREEDUTY.Add(tbmfreeduty_new);
                            mpms.SaveChanges();
                            Session["isModify"] = "1";
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
                        }
                    }
                    Session["shiptime"] = lblONB_DELIVERY_TIMES.Text;
                    Session["no"] = txtONB_NO.Text;
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "申請次數 須為數字");
            //}
            //else
            //{
            //    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            //}
           
        }
        
        //刪除btn
        protected void btnDel_Click(object sender, EventArgs e)
        {

            if (Session["shiptime"] != null && Session["no"] != null)
            {
                string purch_6 = Session["purch_6"].ToString();
                short onbTime = short.Parse(Session["shiptime"].ToString());
                short onbNo = short.Parse(Session["no"].ToString());
                var queryFree =
                    from tbmfree in mpms.TBMFREEDUTY
                    where tbmfree.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    where tbmfree.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        ONB_TIMES = tbmfree.ONB_TIMES,
                        OVC_DUTY_KIND = tbmfree.OVC_DUTY_KIND,
                        ONB_NO = tbmfree.ONB_NO
                    };
                foreach (var q in queryFree)
                {
                    if (q.ONB_TIMES == onbTime && q.OVC_DUTY_KIND == "C" && q.ONB_NO == onbNo)
                    {
                        TBMFREEDUTY tbmfreeduty = new TBMFREEDUTY();
                        tbmfreeduty = mpms.TBMFREEDUTY
                            .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7)) && table.OVC_PURCH_6.Equals(purch_6) && table.OVC_DUTY_KIND.Equals(q.OVC_DUTY_KIND) && table.ONB_NO.Equals(q.ONB_NO)).FirstOrDefault();
                        if (tbmfreeduty != null)
                        {
                            mpms.Entry(tbmfreeduty).State = EntityState.Deleted;
                            mpms.SaveChanges();
                        }
                    }
                }
                string send_url;
                send_url = "~/pages/MPMS/E/MPMS_E22.aspx";
                Response.Redirect(send_url);
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            tax_form();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Goods_Tax_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "申請免稅購買軍用貨品核定單 .docx";
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
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            tax_form();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Goods_Tax_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/Goods_Tax_Temp.pdf";
            string fileName = purch + "申請免稅購買軍用貨品核定單.pdf";
            FCommon.WordToPDF(this, path_temp, wordfilepath, fileName);
        }
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            tax_form();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Goods_Tax_Temp.docx");
            string wordfilepath = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Goods_Tax_Temp.odt");
            string fileName = purch + "申請免稅購買軍用貨品核定單.odt";
            FCommon.WordToOdt(this, path_temp, wordfilepath, fileName);
        }
        #endregion

        #region 副程式
        private void TB_dataImport()
        {
            if (Session["rowtext"] != null)
            {
                string purch_6 = Session["purch_6"].ToString();
                lblOVC_PURCH.Text = Session["rowtext"].ToString();
                var query =
                    from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                    join tbm1301 in mpms.TBM1301_PLAN on tbmreceive.OVC_PURCH equals tbm1301.OVC_PURCH
                    where tbmreceive.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    where tbmreceive.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        ONB_SHIP_TIMES = tbmreceive.ONB_SHIP_TIMES,
                        OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION
                    };
                var queryFree =
                       from tbmfree in mpms.TBMFREEDUTY
                       where tbmfree.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                       where tbmfree.OVC_PURCH_6.Equals(purch_6)
                       select new
                       {
                           ONB_TIMES = tbmfree.ONB_TIMES,
                           OVC_DUTY_KIND = tbmfree.OVC_DUTY_KIND,
                           ONB_NO = tbmfree.ONB_NO,

                           OVC_APPLY = tbmfree.OVC_APPLY,
                           OVC_GOOD_NAPPROVE = tbmfree.OVC_GOOD_NAPPROVE,
                           OVC_GOOD_DESC = tbmfree.OVC_GOOD_DESC,
                           OVC_GOOD_QUALITY = tbmfree.OVC_GOOD_QUALITY,
                           OVC_GOOD_DAPPLY = tbmfree.OVC_GOOD_DAPPLY,
                           OVC_GOOD_BUDGET = tbmfree.OVC_GOOD_BUDGET,
                           OVC_GOOD_AUDIT = tbmfree.OVC_GOOD_AUDIT,
                           OVC_GOOD_USE = tbmfree.OVC_GOOD_USE,
                           OVC_APPROVE_DESC = tbmfree.OVC_APPROVE_DESC,
                           OVC_EXPORT_VENDOR = tbmfree.OVC_EXPORT_VENDOR,
                           OVC_GOOD_NSECTION = tbmfree.OVC_GOOD_NSECTION,
                           OVC_GOOD_IAPPLY = tbmfree.OVC_GOOD_IAPPLY,
                           OVC_GOOD_IAPPROVE = tbmfree.OVC_GOOD_IAPPROVE
                       };
                if (Session["isModify"] != null && Session["shiptime"] != null && Session["no"] != null)
                {
                    lblONB_DELIVERY_TIMES.Text = Session["shiptime"].ToString();
                    txtONB_NO.Text = Session["no"].ToString();
                    foreach (var q in query)
                    {
                        lblOVC_AGNT_IN.Text = q.OVC_PUR_NSECTION;
                        lblONB_DELIVERY_TIMES.Text = q.ONB_SHIP_TIMES.ToString();
                    }
                    foreach (var q in queryFree)
                    {
                        if (q.ONB_TIMES.ToString() == lblONB_DELIVERY_TIMES.Text && q.OVC_DUTY_KIND == "C" && q.ONB_NO.ToString() == txtONB_NO.Text)
                        {
                            txtOVC_APPLY.Text = q.OVC_APPLY;
                            txtOVC_GOOD_NAPPROVE.Text = q.OVC_GOOD_NAPPROVE;
                            txtOVC_GOOD_DESC.Text = q.OVC_GOOD_DESC;
                            TextBox3.Text = q.OVC_GOOD_QUALITY;
                            TextBox4.Text = q.OVC_GOOD_DAPPLY;
                            txtOVC_GOOD_BUDGET.Text = q.OVC_GOOD_BUDGET;
                            txtOVC_GOOD_AUDIT.Text = q.OVC_GOOD_AUDIT;
                            txtOVC_GOOD_USE.Text = q.OVC_GOOD_USE;
                            txtOVC_APPROVE_DESC.Text = q.OVC_APPROVE_DESC;
                            txtOVC_EXPORT_VENDOR.Text = q.OVC_EXPORT_VENDOR;
                            txtOVC_GOOD_NSECTION.Text = q.OVC_GOOD_NSECTION;
                            txtOVC_DAPPLY.Text = q.OVC_GOOD_IAPPLY;
                            txtOVC_MEMO.Text = q.OVC_GOOD_IAPPROVE;
                        }
                    }
                }
                else
                {
                    foreach (var q in query)
                    {
                        if (Session["shiptime_free"] != null)
                        {
                            lblOVC_AGNT_IN.Text = q.OVC_PUR_NSECTION;
                            lblONB_DELIVERY_TIMES.Text = Session["shiptime_free"].ToString();
                        }
                    }
                    foreach (var q in queryFree)
                        if (q.ONB_TIMES.ToString() == lblONB_DELIVERY_TIMES.Text && q.OVC_DUTY_KIND == "C")
                            if (q.ONB_NO >= short.Parse(txtONB_NO.Text))
                                txtONB_NO.Text = (q.ONB_NO + 1).ToString();
                }
            }
        }
        #endregion

        #region 列印軍品採購免進口稅貨物稅表單
        void tax_form()
        {
            int year = 0;
            string date = "";
            string money = "";
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Goods_TaxE30.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    //申請單位
                    var query =
                    from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                    join tbm1301 in mpms.TBM1301_PLAN
                        on tbmreceive.OVC_PURCH equals tbm1301.OVC_PURCH
                    where tbmreceive.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    where tbmreceive.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        ONB_SHIP_TIMES = tbmreceive.ONB_SHIP_TIMES,
                        OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION
                    };
                    foreach (var q in query)
                    {
                        if (q.OVC_PUR_NSECTION != null)
                            doc.ReplaceText("[$OVC_PUR_AGENCY$]", q.OVC_PUR_NSECTION, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_PUR_AGENCY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    //購案編號
                    doc.ReplaceText("[$PURCH$]", Session["rowtext"] != null ? Session["rowtext"].ToString() : "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryFree =
                       from tbmfree in mpms.TBMFREEDUTY
                       where tbmfree.OVC_PURCH.Equals(purch)
                       where tbmfree.OVC_PURCH_6.Equals(purch_6)
                       select new
                       {
                           ONB_TIMES = tbmfree.ONB_TIMES,
                           OVC_DUTY_KIND = tbmfree.OVC_DUTY_KIND,
                           ONB_NO = tbmfree.ONB_NO,

                           OVC_EXPORT_VENDOR = tbmfree.OVC_EXPORT_VENDOR,
                           OVC_TAX_COUNTRY = tbmfree.OVC_TAX_COUNTRY,
                           OVC_TAX_STUFF = tbmfree.OVC_TAX_STUFF,
                           OVC_TAX_QUALITY = tbmfree.OVC_TAX_QUALITY,
                           OVC_TAX_UNIT = tbmfree.OVC_TAX_UNIT,
                           OVC_TAX_UNIT_SUM = tbmfree.OVC_TAX_UNIT_SUM,
                           OVC_TAX_MODEL = tbmfree.OVC_TAX_MODEL,
                           OVC_TAX_DESC = tbmfree.OVC_TAX_DESC,
                           OVC_UNIT_PRICE = tbmfree.OVC_UNIT_PRICE,
                           OVC_TAX_USE = tbmfree.OVC_TAX_USE,
                           OVC_TAX_DIMPORT = tbmfree.OVC_TAX_DIMPORT,
                           OVC_TAX_PLACE = tbmfree.OVC_TAX_PLACE,
                           OVC_TAX_VENDOR = tbmfree.OVC_TAX_VENDOR,
                           OVC_GOOD_TOTAL = tbmfree.OVC_GOOD_TOTAL,
                           OVC_GOOD_IAPPROVE = tbmfree.OVC_GOOD_IAPPROVE,
                           OVC_GOOD_USE = tbmfree.OVC_GOOD_USE,
                           OVC_APPLY = tbmfree.OVC_APPLY,
                           OVC_GOOD_NAPPROVE = tbmfree.OVC_GOOD_NAPPROVE,
                           OVC_GOOD_DESC = tbmfree.OVC_GOOD_DESC,
                           OVC_GOOD_QUALITY = tbmfree.OVC_GOOD_QUALITY,
                           OVC_GOOD_DAPPLY = tbmfree.OVC_GOOD_DAPPLY,
                           OVC_GOOD_BUDGET = tbmfree.OVC_GOOD_BUDGET,
                           OVC_GOOD_AUDIT = tbmfree.OVC_GOOD_AUDIT,
                           OVC_APPROVE_DESC = tbmfree.OVC_APPROVE_DESC,
                           OVC_GOOD_NSECTION = tbmfree.OVC_GOOD_NSECTION,
                           OVC_GOOD_IAPPLY = tbmfree.OVC_GOOD_IAPPLY
                       };
                    foreach (var q in queryFree)
                    {
                        if (Session["shiptime"] != null && Session["no"] != null)
                        {
                            if (q.ONB_TIMES.ToString() == lblONB_DELIVERY_TIMES.Text && q.OVC_DUTY_KIND == "C" && q.ONB_NO.ToString() == txtONB_NO.Text)
                            {
                                doc.ReplaceText("[$OVC_EXPORT_VENDOR$]", q.OVC_EXPORT_VENDOR != null ? q.OVC_EXPORT_VENDOR : "", false, System.Text.RegularExpressions.RegexOptions.None);//供應廠商
                                doc.ReplaceText("[$OVC_GOOD_USE$]", q.OVC_GOOD_USE != null ? q.OVC_GOOD_USE : "", false, System.Text.RegularExpressions.RegexOptions.None);//貨品用途
                                //購買數量
                                if (q.OVC_GOOD_QUALITY != null)
                                {
                                    if (decimal.TryParse(q.OVC_GOOD_QUALITY, out decimal d))
                                    {
                                        decimal decGood = decimal.Parse(q.OVC_GOOD_QUALITY);
                                        money = EastAsiaNumericFormatter.FormatWithCulture("Lc", decGood, null, new CultureInfo("zh-tw"));
                                        doc.ReplaceText("[$OVC_GOOD_QUALITY$]", money, false, System.Text.RegularExpressions.RegexOptions.None);
                                    }
                                    doc.ReplaceText("[$OVC_GOOD_QUALITY$]", q.OVC_GOOD_QUALITY, false, System.Text.RegularExpressions.RegexOptions.None);
                                }
                                doc.ReplaceText("[$OVC_GOOD_IAPPROVE$]", q.OVC_GOOD_IAPPROVE != null ? q.OVC_GOOD_IAPPROVE : "", false, System.Text.RegularExpressions.RegexOptions.None);//核定意見
                                doc.ReplaceText("[$OVC_APPLY$]", q.OVC_APPLY != null ? q.OVC_APPLY : "", false, System.Text.RegularExpressions.RegexOptions.None);//申請書
                                doc.ReplaceText("[$OVC_GOOD_NAPPROVE$]", q.OVC_GOOD_NAPPROVE != null ? q.OVC_GOOD_NAPPROVE : "", false, System.Text.RegularExpressions.RegexOptions.None);//核定機關
                                doc.ReplaceText("[$OVC_GOOD_DESC$]", q.OVC_GOOD_DESC != null ? q.OVC_GOOD_DESC : "", false, System.Text.RegularExpressions.RegexOptions.None);//貨品名稱及其規格
                                doc.ReplaceText("[$OVC_GOOD_DAPPLY$]", q.OVC_GOOD_DAPPLY != null ? q.OVC_GOOD_DAPPLY　: "", false, System.Text.RegularExpressions.RegexOptions.None);//發文日期文號
                                doc.ReplaceText("[$OVC_GOOD_BUDGET$]", q.OVC_GOOD_BUDGET != null ? q.OVC_GOOD_BUDGET : "", false, System.Text.RegularExpressions.RegexOptions.None);//經費來源
                                doc.ReplaceText("[$OVC_GOOD_AUDIT$]", q.OVC_GOOD_AUDIT != null ? q.OVC_GOOD_AUDIT : "", false, System.Text.RegularExpressions.RegexOptions.None);//稽徵機關 
                                doc.ReplaceText("[$OVC_APPROVE_DESC$]", q.OVC_APPROVE_DESC != null ? q.OVC_APPROVE_DESC : "", false, System.Text.RegularExpressions.RegexOptions.None);//核准意見
                                doc.ReplaceText("[$OVC_GOOD_NSECTION$]", q.OVC_GOOD_NSECTION != null ? q.OVC_GOOD_NSECTION : "", false, System.Text.RegularExpressions.RegexOptions.None);//使用單位
                                doc.ReplaceText("[$OVC_GOOD_IAPPLY$]", q.OVC_GOOD_IAPPLY != null ? q.OVC_GOOD_IAPPLY : "", false, System.Text.RegularExpressions.RegexOptions.None);//發文日期文號
                            }
                        }
                    }
                    doc.ReplaceText("[$OVC_EXPORT_VENDOR$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_GOOD_USE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    year = int.Parse(DateTime.Now.ToString("yyyyMMdd").Substring(0, 4)) - 1911;
                    date = year.ToString() + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day + "日";
                    doc.ReplaceText("[$TODAY$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_GOOD_IAPPROVE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_APPLY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_GOOD_NAPPROVE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_GOOD_DESC$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_GOOD_QUALITY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_GOOD_DAPPLY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_GOOD_BUDGET$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_GOOD_AUDIT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_APPROVE_DESC$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_GOOD_NSECTION$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_GOOD_IAPPLY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/Goods_Tax_Temp.docx");
                }
                buffer = ms.ToArray();
            }
        }
        #endregion
    }
}