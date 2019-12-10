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
    public partial class MPMS_E30 : System.Web.UI.Page
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
        //存檔btn
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            short onbTime = short.Parse(lblONB_DELIVERY_TIMES.Text);
            short onbNo;
            string strOVC_GOOD_TOTAL = txtOVC_GOOD_TOTAL.Text;

            bool boolOVC_GOOD_TOTAL = FCommon.checkDecimal(strOVC_GOOD_TOTAL, "貨物總價", ref strMessage, out decimal decOVC_GOOD_TOTAL);
           
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
                                ONB_NO = tbmfree.ONB_NO,
                                tbmfree.ONB_TIMES
                            };
                        foreach (var q in query)
                        {
                            if (q.ONB_NO == onbNo && q.ONB_TIMES == onbTime)
                            {
                                TBMFREEDUTY tbmfreeduty = new TBMFREEDUTY();
                                tbmfreeduty = mpms.TBMFREEDUTY
                                    .Where(table => table.OVC_PURCH.Equals(q.OVC_PURCH) && table.ONB_NO.Equals(q.ONB_NO) && table.OVC_DUTY_KIND.Equals("B")).FirstOrDefault();
                                if (tbmfreeduty != null)
                                {
                                    if (drpOVC_GOOD_DESC.SelectedIndex != 0 && drpOVC_GOOD_DESC.SelectedIndex != -1)
                                        tbmfreeduty.OVC_TAX_STUFF = drpOVC_GOOD_DESC.Text;
                                    else
                                        tbmfreeduty.OVC_TAX_STUFF = txt.Text;
                                    tbmfreeduty.OVC_GOOD_TOTAL = txtOVC_GOOD_TOTAL.Text;
                                    tbmfreeduty.OVC_GOOD_USE = txtOVC_GOOD_USE.Text;
                                    tbmfreeduty.OVC_GOOD_IAPPROVE = txtOVC_GOOD_IAPPROVE.Text;
                                    tbmfreeduty.ONB_NO = short.Parse(txtONB_NO.Text);
                                }
                            }
                        }
                        mpms.SaveChanges();
                        Session["isModify"] = "1";
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                    }
                    else
                    {
                        var query = mpms.TBMFREEDUTY
                            .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7)))
                            .Where(table => table.OVC_DUTY_KIND.Equals("B"))
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
                            tbmfreeduty_new.OVC_DUTY_KIND = "B";
                            tbmfreeduty_new.ONB_NO = short.Parse(txtONB_NO.Text);
                            if (txtOVC_GOOD_TOTAL != null)

                                if (drpOVC_GOOD_DESC.Text != "請選擇貨品名稱")
                                    tbmfreeduty_new.OVC_TAX_STUFF = drpOVC_GOOD_DESC.Text;



                                else
                                    tbmfreeduty_new.OVC_TAX_STUFF = txt.Text;

                            tbmfreeduty_new.OVC_GOOD_USE = txtOVC_GOOD_USE.Text;
                            tbmfreeduty_new.OVC_GOOD_IAPPROVE = txtOVC_GOOD_IAPPROVE.Text;
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
        // 刪除btn
        protected void btnDel_Click(object sender, EventArgs e)
        {
            if (Session["shiptime"] != null && Session["no"] != null)
            {
                short onbTime = short.Parse(Session["shiptime"].ToString());
                short onbNo = short.Parse(Session["no"].ToString());
                string purch_6 = Session["purch_6"].ToString();
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
                    if (q.ONB_TIMES == onbTime && q.OVC_DUTY_KIND == "B" && q.ONB_NO == onbNo)
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
        }

        //print
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            tax_form();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Business_Tax_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "軍品採購免徵營業稅證明 .docx";
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
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Business_Tax_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/Business_Tax_Temp.pdf";
            string fileName = purch + "軍品採購免徵營業稅證明.pdf";
            FCommon.WordToPDF(this, path_temp, wordfilepath, fileName);
        }
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            tax_form();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Business_Tax_Temp.docx");
            string wordfilepath = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Business_Tax_Temp.odt");
            string fileName = purch + "軍品採購免徵營業稅證明.odt";
            FCommon.WordToOdt(this, path_temp, wordfilepath, fileName);
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
                var queryDrp =
                    from tbm1201 in mpms.TBM1201
                    where tbm1201.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    select tbm1201.OVC_POI_NSTUFF_CHN;
                foreach (var q in queryDrp)
                {
                    drpOVC_GOOD_DESC.Items.Add(q);
                }
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

                           OVC_TAX_STUFF = tbmfree.OVC_TAX_STUFF,
                           OVC_GOOD_DESC = tbmfree.OVC_GOOD_DESC,
                           OVC_GOOD_TOTAL = tbmfree.OVC_GOOD_TOTAL,
                           OVC_GOOD_USE = tbmfree.OVC_GOOD_USE,
                           OVC_GOOD_IAPPROVE = tbmfree.OVC_GOOD_IAPPROVE
                       };
                if (Session["isModify"] != null && Session["shiptime"] != null && Session["no"] != null)
                {
                    lblONB_DELIVERY_TIMES.Text = Session["shiptime"].ToString();
                    txtONB_NO.Text = Session["no"].ToString();
                    foreach (var q in query)
                    {
                        lblOVC_AGNT_IN.Text = q.OVC_PUR_NSECTION;
                        
                    }
                    foreach (var q in queryFree)
                    {
                        if (q.ONB_TIMES.ToString() == lblONB_DELIVERY_TIMES.Text && q.OVC_DUTY_KIND == "B" && q.ONB_NO.ToString() == txtONB_NO.Text)
                        {                             
                            txtOVC_GOOD_TOTAL.Text = q.OVC_GOOD_TOTAL;
                            txtOVC_GOOD_USE.Text = q.OVC_GOOD_USE;
                            txt.Text = q.OVC_TAX_STUFF;
                            txtOVC_GOOD_IAPPROVE.Text = q.OVC_GOOD_IAPPROVE;
                            drpOVC_GOOD_DESC.Text = q.OVC_GOOD_DESC;
                        }
                    }
                }
                else
                {
                    foreach (var q in query)
                    {
                        if (Session["shiptime_free"] != null)
                        lblOVC_AGNT_IN.Text = q.OVC_PUR_NSECTION;
                        lblONB_DELIVERY_TIMES.Text = Session["shiptime_free"].ToString();
                    }
                    foreach (var q in queryFree)
                    {
                        if (q.ONB_TIMES.ToString() == lblONB_DELIVERY_TIMES.Text && q.OVC_DUTY_KIND == "B")
                        {
                            if (q.ONB_NO >= short.Parse(txtONB_NO.Text))
                                txtONB_NO.Text = (q.ONB_NO + 1).ToString();
                        }
                    }
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
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Business_TaxE30.docx");
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
                        if (q.OVC_PUR_NSECTION != null)
                            doc.ReplaceText("[$OVC_PUR_AGENCY$]", q.OVC_PUR_NSECTION, false, System.Text.RegularExpressions.RegexOptions.None);
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
                           OVC_GOOD_TOTAL = tbmfree.OVC_GOOD_TOTAL,
                           OVC_GOOD_IAPPROVE = tbmfree.OVC_GOOD_IAPPROVE,
                           OVC_GOOD_USE = tbmfree.OVC_GOOD_USE
                       };
                    foreach (var q in queryFree)
                    {
                        if (Session["shiptime"] != null && Session["no"] != null)
                        {
                            if (q.ONB_TIMES.ToString() == lblONB_DELIVERY_TIMES.Text && q.OVC_DUTY_KIND == "B" && q.ONB_NO.ToString() == txtONB_NO.Text)
                            {
                                if (q.OVC_EXPORT_VENDOR != null)
                                    doc.ReplaceText("[$OVC_EXPORT_VENDOR$]", q.OVC_EXPORT_VENDOR, false, System.Text.RegularExpressions.RegexOptions.None);
                                if (q.OVC_TAX_COUNTRY != null)
                                    doc.ReplaceText("[$OVC_TAX_COUNTRY$]", q.OVC_TAX_COUNTRY, false, System.Text.RegularExpressions.RegexOptions.None);
                                if (q.OVC_TAX_STUFF != null)
                                    doc.ReplaceText("[$OVC_TAX_STUFF$]", q.OVC_TAX_STUFF, false, System.Text.RegularExpressions.RegexOptions.None);
                                if (q.OVC_TAX_QUALITY != null)
                                    doc.ReplaceText("[$OVC_TAX_QUALITY$]", q.OVC_TAX_QUALITY, false, System.Text.RegularExpressions.RegexOptions.None);
                                if (q.OVC_TAX_UNIT != null)
                                    doc.ReplaceText("[$OVC_TAX_UNIT$]", q.OVC_TAX_UNIT, false, System.Text.RegularExpressions.RegexOptions.None);
                                if (q.OVC_TAX_UNIT_SUM != null)
                                    doc.ReplaceText("[$OVC_TAX_UNIT_SUM$]", q.OVC_TAX_UNIT_SUM, false, System.Text.RegularExpressions.RegexOptions.None);
                                if (q.OVC_TAX_MODEL != null)
                                    doc.ReplaceText("[$OVC_TAX_MODEL$]", q.OVC_TAX_MODEL, false, System.Text.RegularExpressions.RegexOptions.None);
                                if (q.OVC_TAX_DESC != null)
                                    doc.ReplaceText("[$OVC_TAX_DESC$]", q.OVC_TAX_DESC, false, System.Text.RegularExpressions.RegexOptions.None);
                                if (q.OVC_UNIT_PRICE != null)
                                    doc.ReplaceText("[$OVC_UNIT_PRICE$]", q.OVC_UNIT_PRICE, false, System.Text.RegularExpressions.RegexOptions.None);
                                if (q.OVC_GOOD_USE != null)
                                    doc.ReplaceText("[$OVC_USE$]", q.OVC_GOOD_USE, false, System.Text.RegularExpressions.RegexOptions.None);
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
                                if (q.OVC_TAX_PLACE != null)
                                    doc.ReplaceText("[$OVC_TAX_PLACE$]", q.OVC_TAX_PLACE, false, System.Text.RegularExpressions.RegexOptions.None);
                                if (q.OVC_TAX_VENDOR != null)
                                    doc.ReplaceText("[$OVC_TAX_VENDOR$]", q.OVC_TAX_VENDOR, false, System.Text.RegularExpressions.RegexOptions.None);
                                if (q.OVC_GOOD_IAPPROVE != null)
                                    doc.ReplaceText("[$OVC_GOOD_IAPPROVE$]", q.OVC_GOOD_IAPPROVE, false, System.Text.RegularExpressions.RegexOptions.None);

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
                    doc.ReplaceText("[$OVC_GOOD_IAPPROVE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/Business_Tax_Temp.docx");
                }
                buffer = ms.ToArray();
            }
        }
        #endregion

        #endregion
    }
}