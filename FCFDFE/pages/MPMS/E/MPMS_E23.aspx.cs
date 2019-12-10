using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using Microsoft.International.Formatters;
using Microsoft.Office.Interop.Word;
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
    public partial class MPMS_E23 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            if (Session["rowtext"] != null)
            {
                {
                    if (!IsPostBack)
                    {
                        FCommon.Controls_Attributes("readonly", "true", txtOVC_TAX_DIMPORT);
                        TB_dataImport();
                        Session.Contents.Remove("Application_times");
                    }
                }
            }
            else
                FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
        }
        #region Click

        //請選擇合約免稅明細btn
        protected void btnCD_Click(object sender, EventArgs e)
        {
            if (Session["isModify"] != null)
            {
                Session["Application_times"] = txtONB_NO.Text;
                string send_url;
                send_url = "~/pages/MPMS/E/MPMS_E24.aspx";
                Response.Redirect(send_url);
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "轉錄前請先存檔");
        }

        //存檔btn
        protected void btnSave_Click(object sender, EventArgs e)
        {
            short onbTime = short.Parse(lblONB_SHIP_TIMES.Text);
            short onbNo;
            string purch_6 = Session["purch_6"].ToString();

            string strMessage = "";
            string strtxtOVC_GOOD_TOTAL = txtOVC_GOOD_TOTAL.Text;

            bool boolOVC_GOOD_TOTAL = FCommon.checkDecimal(strtxtOVC_GOOD_TOTAL, "貨品總價", ref strMessage, out decimal decOVC_GOOD_TOTAL);

            //if (strMessage.Equals(string.Empty))
            //{
                if (short.TryParse(txtONB_NO.Text, out short s))
                {
                    onbNo = short.Parse(txtONB_NO.Text);
                    if (Session["isModify"] != null)
                    {
                        var query =
                            from tbmfree in mpms.TBMFREEDUTY
                            where tbmfree.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                            where tbmfree.OVC_DUTY_KIND.Equals("A")
                            where tbmfree.ONB_TIMES.Equals(onbTime)
                            where tbmfree.ONB_NO.Equals(onbNo)
                            where tbmfree.OVC_PURCH_6.Equals(purch_6)
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
                                    .Where(table => table.OVC_PURCH.Equals(q.OVC_PURCH) && table.OVC_PURCH_6.Equals(purch_6) && table.ONB_NO.Equals(q.ONB_NO) && table.OVC_DUTY_KIND.Equals("A")).FirstOrDefault();
                                if (tbmfreeduty != null)
                                {
                                    tbmfreeduty.ONB_NO = short.Parse(txtONB_NO.Text);
                                    tbmfreeduty.OVC_EXPORT_VENDOR = txtOVC_EXPORT_VENDOR.Text;
                                    tbmfreeduty.OVC_TAX_COUNTRY = txtOVC_TAX_COUNTRY.Text;
                                    tbmfreeduty.OVC_TAX_STUFF = txtOVC_TAX_STUFF.Text;
                                    tbmfreeduty.OVC_TAX_QUALITY = txtOVC_TAX_QUALITY.Text;
                                    tbmfreeduty.OVC_TAX_UNIT = txtOVC_TAX_UNIT.Text;
                                    tbmfreeduty.OVC_TAX_UNIT_SUM = txtOVC_TAX_UNIT_SUM.Text;
                                    tbmfreeduty.OVC_TAX_MODEL = txtOVC_TAX_MODEL.Text;
                                    tbmfreeduty.OVC_TAX_DESC = txtOVC_TAX_DESC.Text;
                                    tbmfreeduty.OVC_UNIT_PRICE = txtOVC_UNIT_PRICE.Text;
                                    tbmfreeduty.OVC_TAX_USE = txtOVC_USE.Text;
                                    tbmfreeduty.OVC_TAX_DIMPORT = txtOVC_TAX_DIMPORT.Text;
                                    tbmfreeduty.OVC_TAX_PLACE = txtOVC_TAX_PLACE.Text;
                                    tbmfreeduty.OVC_TAX_VENDOR = txtOVC_TAX_VENDOR.Text;
                                    tbmfreeduty.OVC_GOOD_TOTAL = txtOVC_GOOD_TOTAL.Text;
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
                            .Where(table => table.OVC_PURCH_6.Equals(purch_6))
                            .Where(table => table.OVC_DUTY_KIND.Equals("A"))
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
                                where tbm1302.OVC_PURCH_6.Equals(purch_6)
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
                            tbmfreeduty_new.ONB_TIMES = short.Parse(lblONB_SHIP_TIMES.Text);
                            tbmfreeduty_new.OVC_DUTY_KIND = "A";
                            tbmfreeduty_new.ONB_NO = short.Parse(txtONB_NO.Text);
                            tbmfreeduty_new.OVC_EXPORT_VENDOR = txtOVC_EXPORT_VENDOR.Text;
                            tbmfreeduty_new.OVC_TAX_COUNTRY = txtOVC_TAX_COUNTRY.Text;
                            tbmfreeduty_new.OVC_TAX_STUFF = txtOVC_TAX_STUFF.Text;
                            tbmfreeduty_new.OVC_TAX_QUALITY = txtOVC_TAX_QUALITY.Text;
                            tbmfreeduty_new.OVC_TAX_UNIT = txtOVC_TAX_UNIT.Text;
                            tbmfreeduty_new.OVC_TAX_UNIT_SUM = txtOVC_TAX_UNIT_SUM.Text;
                            tbmfreeduty_new.OVC_TAX_MODEL = txtOVC_TAX_MODEL.Text;
                            tbmfreeduty_new.OVC_TAX_DESC = txtOVC_TAX_DESC.Text;
                            tbmfreeduty_new.OVC_UNIT_PRICE = txtOVC_UNIT_PRICE.Text;
                            tbmfreeduty_new.OVC_TAX_USE = txtOVC_USE.Text;
                            tbmfreeduty_new.OVC_TAX_DIMPORT = txtOVC_TAX_DIMPORT.Text;
                            tbmfreeduty_new.OVC_TAX_PLACE = txtOVC_TAX_PLACE.Text;
                            tbmfreeduty_new.OVC_TAX_VENDOR = txtOVC_TAX_VENDOR.Text;
                            tbmfreeduty_new.OVC_GOOD_TOTAL = txtOVC_GOOD_TOTAL.Text;
                            mpms.TBMFREEDUTY.Add(tbmfreeduty_new);
                            mpms.SaveChanges();
                            Session["isModify"] = "1";
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
                        }
                    }
                    Session["shiptime"] = lblONB_SHIP_TIMES.Text;
                    Session["no"] = txtONB_NO.Text;

                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "申請次數 須為數字");
            //}
          
            //     else
            //{
            //    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            //}
        }

        //回上一頁btn
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E22.aspx";
            Response.Redirect(send_url);
        }

        //回主流程btn
        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }

        //清除日期
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtOVC_TAX_DIMPORT.Text = "";
        }

        //刪除btn
        protected void btnDel_Click(object sender, EventArgs e)
        {
            short onbTime = short.Parse(lblONB_SHIP_TIMES.Text);
            short onbNo = short.Parse(txtONB_NO.Text);
            string purch_6 = Session["purch_6"].ToString();
            var queryFree =
                from tbmfree in mpms.TBMFREEDUTY
                where tbmfree.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                where tbmfree.OVC_PURCH_6.Equals(purch_6)
                where tbmfree.OVC_DUTY_KIND.Equals("A")
                where tbmfree.ONB_TIMES.Equals(onbTime)
                where tbmfree.ONB_NO.Equals(onbNo)
                select new
                {
                    ONB_TIMES = tbmfree.ONB_TIMES,
                    OVC_DUTY_KIND = tbmfree.OVC_DUTY_KIND,
                    ONB_NO = tbmfree.ONB_NO
                };
            foreach (var q in queryFree)
            {
                if (q.ONB_TIMES.ToString() == lblONB_SHIP_TIMES.Text && q.OVC_DUTY_KIND == "A" && q.ONB_NO.ToString() == txtONB_NO.Text)
                {
                    TBMFREEDUTY tbmfreeduty = new TBMFREEDUTY();
                    tbmfreeduty = mpms.TBMFREEDUTY
                        .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7)) && table.OVC_DUTY_KIND.Equals(q.OVC_DUTY_KIND) && table.ONB_NO.Equals(q.ONB_NO)).FirstOrDefault();
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

        //列印軍品採購免進口稅貨物稅表單
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            tax_form();
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Income_Tax_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "軍品採購免進口稅貨物稅表單.docx";
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
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            tax_form();
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Income_Tax_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/Income_Tax_Temp.pdf";
            WordcvDdf(path_temp, wordfilepath);
            File.Delete(path_temp);
            FileInfo file = new FileInfo(wordfilepath);
            string filepath = purch + "軍品採購免進口稅貨物稅表單.pdf";
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
        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            tax_form();
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Income_Tax_Temp.docx");
            string path_end = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Income_Tax_Temp.odt");
            string filepath = purch + "軍品採購免進口稅貨物稅表單.odt";
            string fileName = HttpUtility.UrlEncode(filepath);
            WordToODT(path_temp, path_end, fileName);
        }
        #endregion

        #region 副程式

        #region Table資料帶入
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
                           OVC_GOOD_TOTAL = tbmfree.OVC_GOOD_TOTAL
                       };
                if (Session["isModify"] != null && Session["shiptime"] != null && Session["no"] != null)
                {
                    lblONB_SHIP_TIMES.Text = Session["shiptime"].ToString();
                    txtONB_NO.Text = Session["no"].ToString();

                    foreach (var q in query)
                        lblOVC_PUR_AGENCY.Text = q.OVC_PUR_NSECTION;

                    foreach (var q in queryFree)
                    {
                        if (q.ONB_TIMES.ToString() == lblONB_SHIP_TIMES.Text && q.OVC_DUTY_KIND == "A" && q.ONB_NO.ToString() == txtONB_NO.Text)
                        {
                            txtOVC_EXPORT_VENDOR.Text = q.OVC_EXPORT_VENDOR;
                            txtOVC_TAX_COUNTRY.Text = q.OVC_TAX_COUNTRY;
                            txtOVC_TAX_STUFF.Text = q.OVC_TAX_STUFF;
                            txtOVC_TAX_QUALITY.Text = q.OVC_TAX_QUALITY;
                            txtOVC_TAX_UNIT.Text = q.OVC_TAX_UNIT;
                            txtOVC_TAX_UNIT_SUM.Text = q.OVC_TAX_UNIT_SUM;
                            txtOVC_TAX_MODEL.Text = q.OVC_TAX_MODEL;
                            txtOVC_TAX_DESC.Text = q.OVC_TAX_DESC;
                            txtOVC_UNIT_PRICE.Text = q.OVC_UNIT_PRICE;
                            txtOVC_USE.Text = q.OVC_TAX_USE;
                            txtOVC_TAX_DIMPORT.Text = q.OVC_TAX_DIMPORT;
                            txtOVC_TAX_PLACE.Text = q.OVC_TAX_PLACE;
                            txtOVC_TAX_VENDOR.Text = q.OVC_TAX_VENDOR;
                            txtOVC_GOOD_TOTAL.Text = q.OVC_GOOD_TOTAL;
                        }
                    }
                }
                else
                {
                    foreach (var q in query)
                    {
                        if (Session["shiptime_free"] != null)
                        {
                            lblOVC_PUR_AGENCY.Text = q.OVC_PUR_NSECTION;
                            lblONB_SHIP_TIMES.Text = Session["shiptime_free"].ToString();
                        }
                    }
                    foreach (var q in queryFree)
                        if (q.ONB_TIMES.ToString() == lblONB_SHIP_TIMES.Text && q.OVC_DUTY_KIND == "A" && q.ONB_NO >= short.Parse(txtONB_NO.Text)) txtONB_NO.Text = (q.ONB_NO + 1).ToString();
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
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/TaxForm_E23.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
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
                    foreach (var q in query)
                        if (q.OVC_PUR_NSECTION != null) doc.ReplaceText("[$OVC_PUR_AGENCY$]", q.OVC_PUR_NSECTION, false, System.Text.RegularExpressions.RegexOptions.None);
                    
                    doc.ReplaceText("[$OVC_PUR_AGENCY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
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
                           OVC_GOOD_TOTAL = tbmfree.OVC_GOOD_TOTAL
                       };
                    foreach (var q in queryFree)
                    {
                        if (Session["shiptime"] != null && Session["no"] != null)
                        {
                            if (q.ONB_TIMES.ToString() == lblONB_SHIP_TIMES.Text && q.OVC_DUTY_KIND == "A" && q.ONB_NO.ToString() == txtONB_NO.Text)
                            {
                                doc.ReplaceText("[$OVC_EXPORT_VENDOR$]", q.OVC_EXPORT_VENDOR != null ? q.OVC_EXPORT_VENDOR : "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$OVC_TAX_COUNTRY$]", q.OVC_TAX_COUNTRY != null ? q.OVC_TAX_COUNTRY : "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$OVC_TAX_STUFF$]", q.OVC_TAX_STUFF != null ? q.OVC_TAX_STUFF : "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$OVC_TAX_QUALITY$]", q.OVC_TAX_QUALITY != null ? q.OVC_TAX_QUALITY : "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$OVC_TAX_UNIT$]", q.OVC_TAX_UNIT != null ? q.OVC_TAX_UNIT : "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$OVC_TAX_UNIT_SUM$]", q.OVC_TAX_UNIT_SUM != null ? q.OVC_TAX_UNIT_SUM : "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$OVC_TAX_MODEL$]", q.OVC_TAX_MODEL != null ? q.OVC_TAX_MODEL : "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$OVC_TAX_DESC$]", q.OVC_TAX_DESC != null ? q.OVC_TAX_DESC : "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$OVC_UNIT_PRICE$]", q.OVC_UNIT_PRICE != null ? q.OVC_UNIT_PRICE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$OVC_USE$]", q.OVC_TAX_USE != null ? q.OVC_TAX_USE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                                if (q.OVC_GOOD_TOTAL != null)
                                {
                                    if (decimal.TryParse(q.OVC_GOOD_TOTAL, out decimal d))
                                    {
                                        money = EastAsiaNumericFormatter.FormatWithCulture("Lc", decimal.Parse(q.OVC_GOOD_TOTAL), null, new CultureInfo("zh-tw"));
                                        doc.ReplaceText("[$OVC_GOOD_TOTAL$]", money, false, System.Text.RegularExpressions.RegexOptions.None);
                                    }
                                    doc.ReplaceText("[$OVC_GOOD_TOTAL$]", q.OVC_GOOD_TOTAL, false, System.Text.RegularExpressions.RegexOptions.None);
                                }
                                if (q.OVC_TAX_DIMPORT != null)
                                {
                                    year = int.Parse(q.OVC_TAX_DIMPORT.ToString().Substring(0, 4)) - 1911;
                                    date = year.ToString() + "年" + q.OVC_TAX_DIMPORT.Substring(5, 2) + "月" + q.OVC_TAX_DIMPORT.Substring(8, 2) + "日";
                                    doc.ReplaceText("[$OVC_TAX_DIMPORT$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                                }
                                doc.ReplaceText("[$OVC_TAX_DIMPORT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$OVC_TAX_PLACE$]", q.OVC_TAX_PLACE != null ? q.OVC_TAX_PLACE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$OVC_TAX_VENDOR$]", q.OVC_TAX_VENDOR != null ? q.OVC_TAX_VENDOR : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                        }
                    }
                    doc.ReplaceText("[$OVC_EXPORT_VENDOR$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_TAX_COUNTRY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_TAX_STUFF$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_TAX_QUALITY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_TAX_UNIT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_TAX_UNIT_SUM$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_TAX_MODEL$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_TAX_DESC$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_UNIT_PRICE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_USE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_GOOD_TOTAL$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_TAX_DIMPORT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_TAX_PLACE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_TAX_VENDOR$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    year = int.Parse(DateTime.Now.ToString("yyyyMMdd").Substring(0, 4)) - 1911;
                    date = year.ToString() + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day + "日";
                    doc.ReplaceText("[$TODAY$]", date, false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/Income_Tax_Temp.docx");
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
        #endregion
    }
}