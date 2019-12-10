using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using Microsoft.International.Formatters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xceed.Words.NET;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E26 : System.Web.UI.Page
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
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_DAUDIT, txtONB_SHIP_TIMES, txtOVC_DNOTICE);
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
            send_url = "~/pages/MPMS/E/MPMS_E25.aspx";
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
            short onbTime;
            short onbNo;
            string purch_6 = Session["purch_6"].ToString();
            if (short.TryParse(txtONB_SHIP_TIMES.Text, out short sh) && short.TryParse(TextBox1.Text, out sh))
            {
                onbTime = short.Parse(txtONB_SHIP_TIMES.Text);
                onbNo = short.Parse(TextBox1.Text);
                if (Session["isModify"] != null)
                {
                    var query =
                            from tbmaudit in mpms.TBMAUDIT
                            where tbmaudit.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                            where tbmaudit.OVC_PURCH_6.Equals(purch_6)
                            select new
                            {
                                ONB_TIMES = tbmaudit.ONB_TIMES,
                                ONB_AUDIT = tbmaudit.ONB_AUDIT,
                                OVC_PERFORMANCE_LIMIT = tbmaudit.OVC_PERFORMANCE_LIMIT,
                                OVC_VEN_ADDRESS = tbmaudit.OVC_VEN_ADDRESS,
                                OVC_DAUDIT = tbmaudit.OVC_DAUDIT,
                                OVC_PERFORMANCE_PLACE = tbmaudit.OVC_PERFORMANCE_PLACE,
                                OVC_DNOTICE = tbmaudit.OVC_DNOTICE,
                                OVC_INOTICE = tbmaudit.OVC_INOTICE,
                                OVC_DESC = tbmaudit.OVC_DESC
                            };
                    foreach (var q in query)
                    {
                        if (txtONB_SHIP_TIMES.Text == q.ONB_TIMES.ToString() || TextBox1.Text == q.OVC_DAUDIT)
                        {
                            TBMAUDIT tBMAUDIT = new TBMAUDIT();
                            tBMAUDIT = mpms.TBMAUDIT
                                .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7)) && table.OVC_PURCH_6.Equals(purch_6) && table.ONB_TIMES.Equals(q.ONB_TIMES) && table.OVC_DAUDIT.Equals(q.OVC_DAUDIT)).FirstOrDefault();
                            if (tBMAUDIT != null)
                            {
                                tBMAUDIT.ONB_TIMES = short.Parse(txtONB_SHIP_TIMES.Text);
                                tBMAUDIT.ONB_AUDIT = short.Parse(TextBox1.Text);
                                tBMAUDIT.OVC_PERFORMANCE_LIMIT = txtOVC_PERFORMANCE_LIMIT.Text;
                                tBMAUDIT.OVC_VEN_ADDRESS = txtOVC_VEN_ADDRESS.Text;
                                tBMAUDIT.OVC_DAUDIT = txtOVC_DAUDIT.Text;
                                tBMAUDIT.OVC_PERFORMANCE_PLACE = txtOVC_PERFORMANCE_PLACE.Text;
                                tBMAUDIT.OVC_DNOTICE = txtOVC_DNOTICE.Text;
                                tBMAUDIT.OVC_INOTICE = txtOVC_INOTICE.Text;
                                tBMAUDIT.OVC_DESC = txtOVC_DESC.Text;
                                mpms.SaveChanges();
                            }
                        }
                    }
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                }
                else
                {
                    var query = mpms.TBMAUDIT
                        .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7)))
                        .Where(o => o.OVC_PURCH_6.Equals(purch_6))
                        .Where(table => table.ONB_TIMES.Equals(onbTime))
                        .Where(table => table.ONB_AUDIT.Equals(onbNo)).FirstOrDefault();

                    if (query != null)
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "交貨批次及履約督導次數重複");
                    else
                    {
                        TBMAUDIT tBMAUDIT_new = new TBMAUDIT();
                        tBMAUDIT_new.OVC_PURCH = lblOVC_PURCH.Text.Substring(0, 7);
                        var queryFirst =
                            from tbm1302 in mpms.TBM1302
                            join tbm1301 in mpms.TBM1301_PLAN on tbm1302.OVC_PURCH equals tbm1301.OVC_PURCH
                            where tbm1302.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                            where tbm1302.OVC_PURCH_6.Equals(purch_6)
                            select new
                            {
                                OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                                OVC_VEN_CST = tbm1302.OVC_VEN_CST,
                                OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE,
                                OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                                OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                                ONB_MONEY = tbm1302.ONB_MONEY,
                                OVC_DCONTRACT = tbm1302.OVC_DCONTRACT,
                                OVC_DBID = tbm1302.OVC_DBID
                            };
                        foreach (var q in queryFirst)
                        {
                            tBMAUDIT_new.OVC_PURCH_6 = q.OVC_PURCH_6;
                            tBMAUDIT_new.OVC_VEN_CST = q.OVC_VEN_CST;
                            tBMAUDIT_new.OVC_VEN_TITLE = q.OVC_VEN_TITLE;
                        }
                        var queryDoName =
                            from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                            where tbmreceive.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                            where tbmreceive.OVC_PURCH_6.Equals(purch_6)
                            select tbmreceive.OVC_DO_NAME;
                        foreach (var qu in queryDoName)
                            tBMAUDIT_new.OVC_DO_NAME = qu;

                        tBMAUDIT_new.ONB_TIMES = short.Parse(txtONB_SHIP_TIMES.Text);
                        tBMAUDIT_new.ONB_AUDIT = short.Parse(TextBox1.Text);
                        tBMAUDIT_new.OVC_PERFORMANCE_LIMIT = txtOVC_PERFORMANCE_LIMIT.Text;
                        tBMAUDIT_new.OVC_VEN_ADDRESS = txtOVC_VEN_ADDRESS.Text;
                        tBMAUDIT_new.OVC_DAUDIT = txtOVC_DAUDIT.Text;
                        tBMAUDIT_new.OVC_PERFORMANCE_PLACE = txtOVC_PERFORMANCE_PLACE.Text;
                        tBMAUDIT_new.OVC_DNOTICE = txtOVC_DNOTICE.Text;
                        tBMAUDIT_new.OVC_INOTICE = txtOVC_INOTICE.Text;
                        tBMAUDIT_new.OVC_DESC = txtOVC_DESC.Text;
                        mpms.TBMAUDIT.Add(tBMAUDIT_new);
                        mpms.SaveChanges();
                        Session["isModify"] = "1";
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
                    }
                }
            }
            else
            {
                if (!short.TryParse(txtONB_SHIP_TIMES.Text, out sh) && short.TryParse(TextBox1.Text, out sh))
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "交貨批次 須為數字");
                else if (!short.TryParse(TextBox1.Text, out sh) && short.TryParse(txtONB_SHIP_TIMES.Text, out sh))
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "履約督導次數 須為數字");
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "<p>交貨批次 須為數字</p><p>履約督導次數 須為數字</p>");
            }
        }

        #region 清除日期btn
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtOVC_DAUDIT.Text = "";
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            txtOVC_DNOTICE.Text = "";
        }
        #endregion

        //print
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            PrinterServlet_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/PS_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "履約督導紀錄表 .docx";
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
            PrinterServlet_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/PS_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/PS_Temp.pdf";
            string fileName = purch + "履約督導紀錄表.pdf";
            FCommon.WordToPDF(this, path_temp, wordfilepath, fileName);
        }
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            PrinterServlet_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/PS_Temp.docx");
            string wordfilepath = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/PS_Temp.odt");
            string fileName = purch + "履約督導紀錄表.odt";
            FCommon.WordToOdt(this, path_temp, wordfilepath, fileName);
        }
        #endregion

        #region 副程式

        #region Table資料帶入
        private void TB_dataImport()
        {
            if (Session["rowtext"] != null)
            {
                lblOVC_PURCH.Text = Session["rowtext"].ToString();
                string purch_6 = Session["purch_6"].ToString();
                var queryFirst =
                    from tbm1302 in mpms.TBM1302
                    join tbm1301 in mpms.TBM1301_PLAN on tbm1302.OVC_PURCH equals tbm1301.OVC_PURCH
                    where tbm1302.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    where tbm1302.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                        OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                        ONB_MONEY = tbm1302.ONB_MONEY,
                        OVC_DCONTRACT = tbm1302.OVC_DCONTRACT,
                        OVC_DBID = tbm1302.OVC_DBID
                    };
                foreach (var q in queryFirst)
                {
                    txtOVC_PUR_AGENCY.Text = q.OVC_PUR_NSECTION;
                    txtOVC_PURCH.Text = q.OVC_PUR_IPURCH;
                    txtONB_MCONTRACT.Text = q.ONB_MONEY.ToString();
                    txtOVC_DCONTRACT.Text = dateTW(q.OVC_DCONTRACT);
                    txtOVC_DBID.Text = dateTW(q.OVC_DBID);
                }
                if (Session["isModify"] != null && Session["shiptime"] != null && Session["no"] != null)
                {
                    txtONB_SHIP_TIMES.Text = Session["shiptime"].ToString();
                    TextBox1.Text = Session["no"].ToString();
                    var query =
                        from tbmaudit in mpms.TBMAUDIT
                        where tbmaudit.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                        where tbmaudit.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            ONB_TIMES = tbmaudit.ONB_TIMES,
                            ONB_AUDIT = tbmaudit.ONB_AUDIT,
                            OVC_PERFORMANCE_LIMIT = tbmaudit.OVC_PERFORMANCE_LIMIT,
                            OVC_VEN_ADDRESS = tbmaudit.OVC_VEN_ADDRESS,
                            OVC_DAUDIT = tbmaudit.OVC_DAUDIT,
                            OVC_PERFORMANCE_PLACE = tbmaudit.OVC_PERFORMANCE_PLACE,
                            OVC_DNOTICE = tbmaudit.OVC_DNOTICE,
                            OVC_INOTICE = tbmaudit.OVC_INOTICE,
                            OVC_DESC = tbmaudit.OVC_DESC
                        };
                    foreach (var q in query)
                    {
                        if (q.ONB_TIMES.ToString() == txtONB_SHIP_TIMES.Text && q.ONB_AUDIT.ToString() == TextBox1.Text)
                        {
                            txtONB_SHIP_TIMES.Text = q.ONB_TIMES.ToString();
                            TextBox1.Text = q.ONB_AUDIT.ToString();
                            txtOVC_PERFORMANCE_LIMIT.Text = q.OVC_PERFORMANCE_LIMIT;
                            txtOVC_VEN_ADDRESS.Text = q.OVC_VEN_ADDRESS;
                            txtOVC_DAUDIT.Text = q.OVC_DAUDIT;
                            txtOVC_PERFORMANCE_PLACE.Text = q.OVC_PERFORMANCE_PLACE;
                            txtOVC_DNOTICE.Text = q.OVC_DNOTICE;
                            txtOVC_INOTICE.Text = q.OVC_INOTICE;
                            txtOVC_DESC.Text = q.OVC_DESC;
                        }
                    }
                }
                else
                {
                    if (Session["shiptime_free"] != null)
                    {
                        txtONB_SHIP_TIMES.Text = Session["shiptime_free"].ToString();
                        var query =
                            from tbmaudit in mpms.TBMAUDIT
                            where tbmaudit.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                            where tbmaudit.OVC_PURCH_6.Equals(purch_6)
                            orderby tbmaudit.ONB_AUDIT
                            select new
                            {
                                ONB_AUDIT = tbmaudit.ONB_AUDIT
                            };
                        foreach (var q in query)
                            if (q.ONB_AUDIT >= short.Parse(TextBox1.Text)) TextBox1.Text = (q.ONB_AUDIT + 1).ToString();
                    }
                }
            }
        }
        #endregion

        #region 列印
        void PrinterServlet_ExportToWord()
        {
            decimal money = 0;
            string Money = "";
            string path = "";
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/PrinterServletE26_1.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    doc.ReplaceText("[$NAME$]", txtOVC_PURCH.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$PURCH$]", lblOVC_PURCH.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$AGENCY$]", txtOVC_PUR_AGENCY.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryVen =
                        from tbm1302 in mpms.TBM1302
                        where tbm1302.OVC_PURCH.Equals(purch)
                        where tbm1302.OVC_PURCH_6.Equals(purch_6)
                        select tbm1302.OVC_VEN_TITLE;
                    foreach (var q in queryVen)
                        if (q != null) doc.ReplaceText("[$VEN$]", q, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$VEN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryNSECTION = mpms.TBM1301.Where(o => o.OVC_PURCH.Equals(purch)).FirstOrDefault();
                    doc.ReplaceText("[$OVC_PUR_NSECTION$]", queryNSECTION != null ? queryNSECTION.OVC_PUR_NSECTION : "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$AGENCY$]", txtOVC_PUR_AGENCY.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    if (txtOVC_DAUDIT.Text != "")
                    {
                        int year = int.Parse(txtOVC_DAUDIT.Text.Substring(0, 4)) - 1911;
                        string date = year.ToString() + "年" + txtOVC_DAUDIT.Text.Substring(5, 2) + "月" + txtOVC_DAUDIT.Text.Substring(8, 2) + "日";
                        doc.ReplaceText("[$OVC_DAUDIT$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_DAUDIT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_PERFORMANCE_LIMIT$]", txtOVC_PERFORMANCE_LIMIT.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_PERFORMANCE_PLACE$]", txtOVC_PERFORMANCE_PLACE.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    if (txtONB_MCONTRACT.Text != "")
                    {
                        money = decimal.Parse(txtONB_MCONTRACT.Text);
                        Money = EastAsiaNumericFormatter.FormatWithCulture("Lc", money, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                        doc.ReplaceText("[$ONB_MCONTRACT$]", Money, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$ONB_MCONTRACT$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$TIME$]", txtONB_SHIP_TIMES.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    if (txtOVC_DNOTICE.Text != "")
                    {
                        int year = int.Parse(txtOVC_DNOTICE.Text.Substring(0, 4)) - 1911;
                        string date = year.ToString() + "年" + txtOVC_DNOTICE.Text.Substring(5, 2) + "月" + txtOVC_DNOTICE.Text.Substring(8, 2) + "日";
                        doc.ReplaceText("[$OVC_DNOTICE$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_DNOTICE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_DESC$]", txtOVC_DESC.Text, false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/PS_Temp.docx");
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