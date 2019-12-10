using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xceed.Words.NET;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E19 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (Session["rowtext"] != null && Session["unit"] != null && Session["date"] != null)
                {
                    if (!IsPostBack)
                    {
                        FCommon.Controls_Attributes("readonly", "true", txtOVC_DAPPLY);
                        list_dataImport(drpOVC_INSPECT_UNIT);
                        TB_dataImport();
                    }
                }
                else
                    FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
            }
        }

        #region Click
        //存檔
        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool boolONB_QUALITY = txtONB_QUALITY.Text == "" || decimal.TryParse(txtONB_QUALITY.Text, out decimal d);
            bool boolONB_CASE_QUALITY = txtONB_CASE_QUALITY.Text == "" || decimal.TryParse(txtONB_CASE_QUALITY.Text, out d);
            bool boolONB_REPORT_ORG = txtONB_REPORT_ORG.Text == "" || short.TryParse(txtONB_REPORT_ORG.Text, out short s);
            bool boolONB_REPORT_COPY = txtONB_REPORT_COPY.Text == "" || short.TryParse(txtONB_REPORT_COPY.Text, out s);
            bool boolONB_REPORT = txtONB_REPORT.Text == "" || short.TryParse(txtONB_REPORT.Text, out s);
            bool boolONB_REPORT_ENG = txtONB_REPORT_ENG.Text == "" || short.TryParse(txtONB_REPORT_ENG.Text, out s);

            if (boolONB_QUALITY && boolONB_CASE_QUALITY && boolONB_REPORT_ORG && boolONB_REPORT_COPY && boolONB_REPORT && boolONB_REPORT_ENG)
            {
                if (Session["isModify"] != null)
                {
                    string purch_6 = Session["purch_6"].ToString();
                    var query =
                        from tbmapply in mpms.TBMAPPLY_INSPECT
                        where tbmapply.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                        where tbmapply.OVC_PURCH_6.Equals(purch_6)
                        select tbmapply.ONB_TIMES;
                    foreach (var q in query)
                    {
                        if (q.ToString() == lblONB_TIMES.Text)
                        {
                            TBMAPPLY_INSPECT tbmapply_inspect = new TBMAPPLY_INSPECT();
                            tbmapply_inspect = mpms.TBMAPPLY_INSPECT
                                .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7)) && table.OVC_PURCH_6.Equals(purch_6) && table.ONB_TIMES.Equals(q)).FirstOrDefault();
                            if (tbmapply_inspect != null)
                            {
                                tbmapply_inspect.OVC_INSPECT_UNIT = txtOVC_INSPECT_UNIT.Text;
                                tbmapply_inspect.OVC_DAPPLY = txtOVC_DAPPLY.Text;
                                tbmapply_inspect.OVC_TAKER = txtOVC_TAKER.Text;
                                tbmapply_inspect.OVC_INSPECTED_STUFF = txtOVC_INSPECTED_STUFF.Text;
                                if (RadioButtonList1.SelectedIndex == 0)
                                    tbmapply_inspect.OVC_BACK = "Y";
                                else
                                    tbmapply_inspect.OVC_BACK = "N";
                                tbmapply_inspect.OVC_INSPECTED_STUFF_ENG = txtOVC_INSPECTED_STUFF_ENG.Text;
                                if (txtONB_QUALITY.Text != "")
                                    tbmapply_inspect.ONB_QUALITY = decimal.Parse(txtONB_QUALITY.Text);
                                tbmapply_inspect.OVC_INSPECT_DESC = txtOVC_INSPECT_DESC.Text;
                                if (RadioButtonList2.SelectedIndex == 0)
                                    tbmapply_inspect.OVC_ATTACH = "Y";
                                else
                                    tbmapply_inspect.OVC_ATTACH = "N";
                                tbmapply_inspect.OVC_IAPPLY = txtOVC_IAPPLY.Text;
                                tbmapply_inspect.OVC_APPLY_DESC = txtOVC_APPLY_DESC.Text;
                                tbmapply_inspect.OVC_MARK = txtOVC_MARK.Text;
                                if (txtONB_CASE_QUALITY.Text != "")
                                    tbmapply_inspect.ONB_CASE_QUALITY = decimal.Parse(txtONB_CASE_QUALITY.Text);
                                if (txtONB_REPORT_ORG.Text != "")
                                    tbmapply_inspect.ONB_REPORT_ORG = short.Parse(txtONB_REPORT_ORG.Text);
                                if (txtONB_REPORT_COPY.Text != "")
                                    tbmapply_inspect.ONB_REPORT_COPY = short.Parse(txtONB_REPORT_COPY.Text);
                                if (txtONB_REPORT.Text != "")
                                    tbmapply_inspect.ONB_REPORT = short.Parse(txtONB_REPORT.Text);
                                if (txtONB_REPORT_ENG.Text != "")
                                    tbmapply_inspect.ONB_REPORT_ENG = short.Parse(txtONB_REPORT_ENG.Text);
                                mpms.SaveChanges();
                            }
                            TB_dataImport();
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                        }
                    }

                }
                else
                {
                    string purch_6 = Session["purch_6"].ToString();
                    TBMAPPLY_INSPECT tbmapply_inspect_new = new TBMAPPLY_INSPECT();
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
                        tbmapply_inspect_new.OVC_PURCH = q.OVC_PURCH;
                        tbmapply_inspect_new.OVC_PURCH_6 = q.OVC_PURCH_6;
                        tbmapply_inspect_new.OVC_VEN_CST = q.OVC_VEN_CST;
                    }
                    if (lblONB_TIMES.Text != "")
                        tbmapply_inspect_new.ONB_TIMES = short.Parse(lblONB_TIMES.Text);
                    if (lblONB_INSPECT_TIMES.Text != "")
                        tbmapply_inspect_new.ONB_INSPECT_TIMES = short.Parse(lblONB_INSPECT_TIMES.Text);
                    if (lblONB_RE_INSPECT_TIMES.Text != "")
                        tbmapply_inspect_new.ONB_RE_INSPECT_TIMES = short.Parse(lblONB_RE_INSPECT_TIMES.Text);
                    tbmapply_inspect_new.OVC_INSPECT_UNIT = txtOVC_INSPECT_UNIT.Text;
                    tbmapply_inspect_new.OVC_DAPPLY = txtOVC_DAPPLY.Text;
                    tbmapply_inspect_new.OVC_IAPPLY = txtOVC_IAPPLY.Text;
                    tbmapply_inspect_new.OVC_INSPECTED_STUFF = txtOVC_INSPECTED_STUFF.Text;
                    if (txtONB_QUALITY.Text != "")
                        tbmapply_inspect_new.ONB_QUALITY = decimal.Parse(txtONB_QUALITY.Text);
                    tbmapply_inspect_new.OVC_INSPECT_DESC = txtOVC_INSPECT_DESC.Text;
                    if (RadioButtonList2.SelectedIndex == 0)
                        tbmapply_inspect_new.OVC_ATTACH = "Y";
                    else
                        tbmapply_inspect_new.OVC_ATTACH = "N";
                    tbmapply_inspect_new.OVC_TAKER = txtOVC_TAKER.Text;
                    tbmapply_inspect_new.OVC_MARK = txtOVC_MARK.Text;
                    tbmapply_inspect_new.OVC_APPLY_DESC = txtOVC_APPLY_DESC.Text;
                    if (txtONB_CASE_QUALITY.Text != "")
                        tbmapply_inspect_new.ONB_CASE_QUALITY = decimal.Parse(txtONB_CASE_QUALITY.Text);
                    if (txtONB_REPORT_ORG.Text != "")
                        tbmapply_inspect_new.ONB_REPORT_ORG = short.Parse(txtONB_REPORT_ORG.Text);
                    if (txtONB_REPORT_COPY.Text != "")
                        tbmapply_inspect_new.ONB_REPORT_COPY = short.Parse(txtONB_REPORT_COPY.Text);
                    var querydoname =
                        from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                        where tbmreceive.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                        where tbmreceive.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            OVC_DO_NAME = tbmreceive.OVC_DO_NAME
                        };
                    foreach (var q in querydoname)
                    {
                        tbmapply_inspect_new.OVC_DO_NAME = q.OVC_DO_NAME;
                    }
                    if (RadioButtonList1.SelectedIndex == 0)
                        tbmapply_inspect_new.OVC_BACK = "Y";
                    else
                        tbmapply_inspect_new.OVC_BACK = "N";
                    tbmapply_inspect_new.OVC_INSPECTED_STUFF_ENG = txtOVC_INSPECTED_STUFF_ENG.Text;
                    if (txtONB_REPORT.Text != "")
                        tbmapply_inspect_new.ONB_REPORT = short.Parse(txtONB_REPORT.Text);
                    if (txtONB_REPORT_ENG.Text != "")
                        tbmapply_inspect_new.ONB_REPORT_ENG = short.Parse(txtONB_REPORT_ENG.Text);
                    mpms.TBMAPPLY_INSPECT.Add(tbmapply_inspect_new);
                    mpms.SaveChanges();
                    Session["isModify"] = "1";
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
                }
            }
            else
            {
                string strmeg = "";
                if (!txtONB_QUALITY.Text.Equals("") && !decimal.TryParse(txtONB_QUALITY.Text, out d))
                    strmeg += "<p>件數 須為數字</p>";
                if (!txtONB_CASE_QUALITY.Text.Equals("") && !decimal.TryParse(txtONB_CASE_QUALITY.Text, out d))
                    strmeg += "<p>申請檢驗 須為數字</p>";
                if (!txtONB_REPORT_ORG.Text.Equals("") && !short.TryParse(txtONB_REPORT_ORG.Text, out s))
                    strmeg += "<p>需要報告書正本 須為數字</p>";
                if (!txtONB_REPORT_COPY.Text.Equals("") && !short.TryParse(txtONB_REPORT_COPY.Text, out s))
                    strmeg += "<p>副本 須為數字</p>";
                if (!txtONB_REPORT.Text.Equals("") && !short.TryParse(txtONB_REPORT.Text, out s))
                    strmeg += "<p>報告書中文份數 須為數字</p>";
                if (!txtONB_REPORT_ENG.Text.Equals("") && !short.TryParse(txtONB_REPORT_ENG.Text, out s))
                    strmeg += "<p>報告書英文份數 須為數字</p>";
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strmeg);
            }
        }
        //回上一頁
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E18.aspx";
            Response.Redirect(send_url);
        }
        //回主流程
        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }
        //檢驗申請單
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/IA_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "檢驗申請單.docx";
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
            ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/IA_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/IA_Temp.pdf";
            string fileName = purch + "檢驗申請單.pdf";
            FCommon.WordToPDF(this, path_temp, wordfilepath, fileName);
        }
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/IA_Temp.docx");
            string wordfilepath = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/IA_Temp.odt");
            string fileName = purch + "檢驗申請單.odt";
            FCommon.WordToOdt(this, path_temp, wordfilepath, fileName);
        }
        #endregion

        #region 副程式

        #region 受文單位
        private void list_dataImport(ListControl list)
        {
            list.Items.Clear();
            var query =
                from tbm1407 in mpms.TBM1407
                where tbm1407.OVC_PHR_CATE.Equals("S7")
                select tbm1407.OVC_PHR_DESC;
            foreach (var q in query)
                list.Items.Add(q);
        }
        #endregion

        #region Table資料帶入
        private void TB_dataImport()
        {
            if (Session["rowtext"] != null)
            {
                string purch_6 = Session["purch_6"].ToString();
                lblOVC_PURCH.Text = Session["rowtext"].ToString();
                if (Session["isModify"] != null && Session["date"] != null && Session["unit"] != null)
                {
                    var purch = Session["rowtext"].ToString().Substring(0, 7);
                    //var date = Session["date"].ToString();
                    //var unit = Session["unit"].ToString();
                    string strONB_TIMES = Session["txtONB_TIMES"].ToString();
                    string strONB_INSPECT_TIMES = Session["txtONB_INSPECT_TIMES"].ToString();
                    string strONB_RE_INSPECT_TIMES = Session["txtONB_RE_INSPECT_TIMES"].ToString();
                    short ONB_TIMES = short.Parse(strONB_TIMES);
                    short ONB_INSPECT_TIMES = short.Parse(strONB_INSPECT_TIMES);
                    short ONB_RE_INSPECT_TIMES = short.Parse(strONB_RE_INSPECT_TIMES);
                    var query =
                        from tbmapply in mpms.TBMAPPLY_INSPECT
                        where tbmapply.OVC_PURCH.Equals(purch)
                        where tbmapply.OVC_PURCH_6.Equals(purch_6)
                        where tbmapply.ONB_TIMES.Equals(ONB_TIMES)
                        where tbmapply.ONB_INSPECT_TIMES.Equals(ONB_INSPECT_TIMES)
                        where tbmapply.ONB_RE_INSPECT_TIMES.Equals(ONB_RE_INSPECT_TIMES)
                        //where tbmapply.OVC_DAPPLY.Equals(date)
                        //where tbmapply.OVC_INSPECT_UNIT.Equals(unit)
                        select new
                        {
                            ONB_TIMES = tbmapply.ONB_TIMES,
                            ONB_INSPECT_TIMES = tbmapply.ONB_INSPECT_TIMES,
                            ONB_RE_INSPECT_TIMES = tbmapply.ONB_RE_INSPECT_TIMES,
                            OVC_INSPECT_UNIT = tbmapply.OVC_INSPECT_UNIT,
                            OVC_DAPPLY = tbmapply.OVC_DAPPLY,
                            OVC_TAKER = tbmapply.OVC_TAKER,
                            OVC_INSPECTED_STUFF = tbmapply.OVC_INSPECTED_STUFF,
                            OVC_BACK = tbmapply.OVC_BACK,
                            OVC_INSPECTED_STUFF_ENG = tbmapply.OVC_INSPECTED_STUFF_ENG,
                            ONB_QUALITY = tbmapply.ONB_QUALITY,
                            OVC_INSPECT_DESC = tbmapply.OVC_INSPECT_DESC,
                            OVC_ATTACH = tbmapply.OVC_ATTACH,
                            OVC_IAPPLY = tbmapply.OVC_IAPPLY,
                            OVC_APPLY_DESC = tbmapply.OVC_APPLY_DESC,
                            OVC_MARK = tbmapply.OVC_MARK,
                            ONB_CASE_QUALITY = tbmapply.ONB_CASE_QUALITY,
                            ONB_REPORT_ORG = tbmapply.ONB_REPORT_ORG,
                            ONB_REPORT_COPY = tbmapply.ONB_REPORT_COPY,
                            ONB_REPORT = tbmapply.ONB_REPORT,
                            ONB_REPORT_ENG = tbmapply.ONB_REPORT_ENG
                        };
                    foreach (var q in query)
                    {
                        lblONB_TIMES.Text = q.ONB_TIMES.ToString();
                        lblONB_INSPECT_TIMES.Text = q.ONB_INSPECT_TIMES.ToString();
                        lblONB_RE_INSPECT_TIMES.Text = q.ONB_RE_INSPECT_TIMES.ToString();
                        var t1407 = mpms.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("S7") && t.OVC_PHR_DESC.Equals(q.OVC_INSPECT_UNIT)).FirstOrDefault();
                        if (t1407 != null) drpOVC_INSPECT_UNIT.Text = q.OVC_INSPECT_UNIT; else drpOVC_INSPECT_UNIT.Text = "其他(請自行輸入)";
                        txtOVC_INSPECT_UNIT.Text = q.OVC_INSPECT_UNIT;
                        txtOVC_DAPPLY.Text = q.OVC_DAPPLY;
                        txtOVC_TAKER.Text = q.OVC_TAKER;
                        txtOVC_INSPECTED_STUFF.Text = q.OVC_INSPECTED_STUFF;
                        if (q.OVC_BACK.ToString() == "Y")
                            RadioButtonList1.SelectedIndex = 0;
                        else if (q.OVC_BACK.ToString() == "N")
                            RadioButtonList1.SelectedIndex = 1;
                        txtOVC_INSPECTED_STUFF_ENG.Text = q.OVC_INSPECTED_STUFF_ENG;
                        txtONB_QUALITY.Text = q.ONB_QUALITY.ToString();
                        txtOVC_INSPECT_DESC.Text = q.OVC_INSPECT_DESC;
                        if (q.OVC_ATTACH.ToString() == "Y")
                            RadioButtonList2.SelectedIndex = 0;
                        else if (q.OVC_ATTACH.ToString() == "N")
                            RadioButtonList2.SelectedIndex = 1;
                        txtOVC_IAPPLY.Text = q.OVC_IAPPLY;
                        txtOVC_APPLY_DESC.Text = q.OVC_APPLY_DESC;
                        txtOVC_MARK.Text = q.OVC_MARK;
                        txtONB_CASE_QUALITY.Text = q.ONB_CASE_QUALITY.ToString();
                        txtONB_REPORT_ORG.Text = q.ONB_REPORT_ORG.ToString();
                        txtONB_REPORT_COPY.Text = q.ONB_REPORT_COPY.ToString();
                        txtONB_REPORT.Text = q.ONB_REPORT.ToString();
                        txtONB_REPORT_ENG.Text = q.ONB_REPORT_ENG.ToString();
                    }
                }
                else
                {
                    if (Session["txtONB_TIMES"] != null)
                    {
                        lblONB_TIMES.Text = Session["txtONB_TIMES"].ToString();
                        lblONB_INSPECT_TIMES.Text = Session["txtONB_INSPECT_TIMES"].ToString();
                        lblONB_RE_INSPECT_TIMES.Text = Session["txtONB_RE_INSPECT_TIMES"].ToString();
                        txtOVC_INSPECT_UNIT.Text = Session["unit"].ToString();
                        txtOVC_DAPPLY.Text = Session["date"].ToString();
                    }
                }
            }
        }

        protected void drpOVC_INSPECT_UNIT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpOVC_INSPECT_UNIT.Text.Equals("其他(請自行輸入)"))
                txtOVC_INSPECT_UNIT.Text = "";
            else
                txtOVC_INSPECT_UNIT.Text = drpOVC_INSPECT_UNIT.Text;
        }
        #endregion

        #region 檢驗申請單
        void ExportToWord()
        {
            string path = "";
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            var query =
                from inspect in mpms.TBMAPPLY_INSPECT
                where inspect.OVC_PURCH.Equals(purch)
                where inspect.OVC_PURCH_6.Equals(purch_6)
                select new
                {
                    OVC_INSPECT_UNIT = inspect.OVC_INSPECT_UNIT,
                    OVC_DO_NAME = inspect.OVC_DO_NAME,
                    ONB_REPORT = inspect.ONB_REPORT,
                    ONB_REPORT_ENG = inspect.ONB_REPORT_ENG,
                    ONB_REPORT_ORG = inspect.ONB_REPORT_ORG,
                    ONB_REPORT_COPY = inspect.ONB_REPORT_COPY,
                    OVC_BACK = inspect.OVC_BACK,
                    OVC_INSPECTED_STUFF = inspect.OVC_INSPECTED_STUFF,
                    OVC_INSPECTED_STUFF_ENG = inspect.OVC_INSPECTED_STUFF_ENG,
                    ONB_QUALITY = inspect.ONB_QUALITY,
                    OVC_IAPPLY = inspect.OVC_IAPPLY,
                    OVC_INSPECT_DESC = inspect.OVC_INSPECT_DESC,
                    OVC_ATTACH = inspect.OVC_ATTACH,
                    OVC_MARK = inspect.OVC_MARK,
                    ONB_CASE_QUALITY = inspect.ONB_CASE_QUALITY,
                    OVC_APPLY_DESC = inspect.OVC_APPLY_DESC,
                    OVC_TAKER = inspect.OVC_TAKER,
                    OVC_VEN_CST = inspect.OVC_VEN_CST
                };
            foreach (var q in query)
            {
                if (q.OVC_INSPECT_UNIT == "財團法人中國紡織工業研究中心")
                    path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/TextileE19.docx");
                else if (q.OVC_INSPECT_UNIT == "經濟部標準檢驗局(第六組)")
                    path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/EconomicE19.docx");
                else if (q.OVC_INSPECT_UNIT == "財團法人鞋類設計暨技術研究中心")
                    path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/ShoeE19.docx");
                else if (q.OVC_INSPECT_UNIT == "國防部軍備局規格鑑測中心")
                    path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/ArmsE19.docx");
                else if (q.OVC_INSPECT_UNIT == "食品工業發展研究所")
                    path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/FoodE19.docx");
                else
                    return;
            }
            if (query.Count() < 1)
                return;
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    foreach (var q in query)
                    {
                        var queryAcc =
                            from acc in mpms.ACCOUNT
                            where acc.USER_NAME.Equals(q.OVC_DO_NAME)
                            select acc.IUSER_PHONE;
                        foreach (var qu in queryAcc)
                            if (qu != null) doc.ReplaceText("[$PHONE$]", qu, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$PHONE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$DO_NAME$]", q.OVC_DO_NAME != null ? q.OVC_DO_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CHK1$]", (q.ONB_REPORT != null && q.ONB_REPORT != 0) ? "■" : "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CHI$]", (q.ONB_REPORT != null && q.ONB_REPORT != 0) ? q.ONB_REPORT.ToString() : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CHK2$]", (q.ONB_REPORT_ENG != null && q.ONB_REPORT_ENG != 0) ? "■" : "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$ENG$]", (q.ONB_REPORT_ENG != null && q.ONB_REPORT_ENG != 0) ? q.ONB_REPORT_ENG.ToString() : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CHK3$]", (q.ONB_REPORT_ORG != null && q.ONB_REPORT_ORG != 0) ? "■" : "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$ORG$]", (q.ONB_REPORT_ORG != null && q.ONB_REPORT_ORG != 0) ? q.ONB_REPORT_ORG.ToString() : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CHK4$]", (q.ONB_REPORT_COPY != null && q.ONB_REPORT_COPY != 0) ? "■" : "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$COPY$]", (q.ONB_REPORT_COPY != null && q.ONB_REPORT_COPY != 0) ? q.ONB_REPORT_COPY.ToString() : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CHK5$]", (q.OVC_BACK == "Y") ? "■" : "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$BACK_Y$]", (q.OVC_BACK == "Y") ? "■" : "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$BACK_N$]", (q.OVC_BACK == "Y") ? "□" : "■", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$Inspected_Stuff$]", q.OVC_INSPECTED_STUFF != null ? q.OVC_INSPECTED_STUFF : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$Inspected_Stuff_Eng$]", q.OVC_INSPECTED_STUFF_ENG != null ? q.OVC_INSPECTED_STUFF_ENG : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$Quality$]", q.ONB_QUALITY != null ? String.Format("{0:N}", q.ONB_QUALITY) : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        string d = DateTime.Now.ToString("yyyy-MM-dd");
                        int year = int.Parse(d.Substring(0, 4)) - 1911;
                        string date = year.ToString() + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day + "日";
                        doc.ReplaceText("[$TODAY$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_IAPPLY$]", q.OVC_IAPPLY != null ? q.OVC_IAPPLY : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$INSPECT_DESC$]", q.OVC_INSPECT_DESC != null ? q.OVC_INSPECT_DESC : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$ATTACH_Y$]", q.OVC_ATTACH == "Y" ? "■" : "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$ATTACH_N$]", q.OVC_ATTACH == "N" ? "■" : "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$MARK$]", q.OVC_MARK != null ? q.OVC_MARK : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CASE_QUALITY$]", q.ONB_CASE_QUALITY != null ? String.Format("{0:N}", q.ONB_CASE_QUALITY) : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$APPLY_DESC$]", q.OVC_APPLY_DESC != null ? q.OVC_APPLY_DESC : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$TAKER$]", q.OVC_TAKER != null ? q.OVC_TAKER : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CST$]", q.OVC_VEN_CST != null ? q.OVC_VEN_CST : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        var query1302 =
                            from tbm1302 in mpms.TBM1302
                            where tbm1302.OVC_PURCH.Equals(purch)
                            where tbm1302.OVC_PURCH_6.Equals(purch_6)
                            where tbm1302.OVC_VEN_CST.Equals(q.OVC_VEN_CST)
                            select tbm1302.OVC_VEN_TITLE;
                        foreach (var qu in query1302)
                            if (qu != null) doc.ReplaceText("[$VEN$]", qu, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$VEN$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    var queryNSECTION = mpms.TBM1301.Where(o => o.OVC_PURCH.Equals(purch)).FirstOrDefault();
                    doc.ReplaceText("[$OVC_PUR_NSECTION$]", queryNSECTION != null ? queryNSECTION.OVC_PUR_NSECTION : "", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/IA_Temp.docx");
                }
                buffer = ms.ToArray();
            }
        }
        #endregion

        #endregion
    }
}