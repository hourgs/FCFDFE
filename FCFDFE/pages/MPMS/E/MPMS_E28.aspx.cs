using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data.Entity;
using System.IO;
using Xceed.Words.NET;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E28 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            if (Session["rowtext"] != null && Session["shiptime"] != null)
            {
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtONB_SHIP_TIMES);
                    TB_dataImport();
                }
            }
            else
                FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
        }

        #region Click
        // 回上一頁
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E27.aspx";
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
            if (CheckBoxList8.Items[0].Selected.Equals(true) && !string.IsNullOrEmpty(TextBox_8.Text) && !int.TryParse(TextBox_8.Text, out int n))
                strMessage = "<p>天數 須為數字</p>";
            if (CheckBoxList9.Items[0].Selected.Equals(true) && !string.IsNullOrEmpty(TextBox_9.Text) && !int.TryParse(TextBox_9.Text, out n))
                strMessage = "<p>天數 須為數字</p>";
            if (CheckBoxList10.Items[0].Selected.Equals(true) && !string.IsNullOrEmpty(TextBox_10.Text) && !int.TryParse(TextBox_10.Text, out n))
                strMessage = "<p>天數 須為數字</p>";
            if (CheckBoxList11.Items[0].Selected.Equals(true) && !string.IsNullOrEmpty(TextBox_11.Text) && !int.TryParse(TextBox_11.Text, out n))
                strMessage = "<p>天數 須為數字</p>";

            if (Session["rowtext"] != null)
            {
                string purch_6 = Session["purch_6"].ToString();
                string purch = Session["rowtext"].ToString();
                short shiptime = short.Parse(Session["shiptime"].ToString());
                if (Session["isModify_2"] != null)//修改
                {
                    short INSPECT_TIMES = short.Parse(Session["INSPECT_TIMES"].ToString());
                    short RE_INSPECT_TIMES = short.Parse(Session["RE_INSPECT_TIMES"].ToString());
                    TBMINSPECT_REPORT tbminspect_report = new TBMINSPECT_REPORT();
                    tbminspect_report = mpms.TBMINSPECT_REPORT
                        .Where(table => purch.Contains(table.OVC_PURCH) && table.OVC_PURCH_6.Equals(purch_6))
                        .Where(table => table.ONB_TIMES.Equals(shiptime) && table.ONB_INSPECT_TIMES.Equals(INSPECT_TIMES) && table.ONB_RE_INSPECT_TIMES.Equals(RE_INSPECT_TIMES)).FirstOrDefault();
                    if (tbminspect_report != null)
                    {
                        tbminspect_report.OVC_REPORT_DESC = txtOVC_REPORT_DESC.Text;
                        tbminspect_report.OVC_RESULT_4_DELAY = txtOVC_RESULT_4_DELAY.Text;
                        tbminspect_report.OVC_RESULT_4_PUNISH = txtOVC_RESULT_4_PUNISH.Text;
                        if (rdoOVC_RESULT_1.SelectedIndex == 0)
                            tbminspect_report.OVC_RESULT_1 = "Y";
                        else
                            tbminspect_report.OVC_RESULT_1 = "N";
                        tbminspect_report.OVC_RESULT_2_PERCENT = txtOVC_RESULT_2_PERCENT.Text;
                        if (rdoOVC_RESULT_2.SelectedIndex == 0)
                            tbminspect_report.OVC_RESULT_2 = "Y";
                        else
                            tbminspect_report.OVC_RESULT_2 = "N";
                        if (rdoOVC_RESULT_3.SelectedIndex == 0)
                            tbminspect_report.OVC_RESULT_3 = "Y";
                        else
                            tbminspect_report.OVC_RESULT_3 = "N";
                        if (rdoOVC_RESULT_4.SelectedIndex == 0)
                            tbminspect_report.OVC_RESULT_4 = "Y";
                        else
                            tbminspect_report.OVC_RESULT_4 = "N";
                        tbminspect_report.OVC_RESULT_5 = txtOVC_RESULT_5.Text;
                        if (strMessage == "")
                            mpms.SaveChanges();
                    }
                    for (int i = 1; i < 13; i++)
                    {
                        string chk = "CheckBoxList" + i;
                        string tex = "TextBox_" + i;
                        string strlab = "label_" + i;
                        string strlab2 = "label2_" + i;
                        CheckBoxList checkBoxList = (CheckBoxList)tb.FindControl(chk);
                        TextBox textBox = (TextBox)tb.FindControl(tex);
                        Label lab = (Label)tb.FindControl(strlab);
                        Label lab2 = (Label)tb.FindControl(strlab2);
                        short sh = short.Parse(i.ToString());
                        TBMINSPECT_REPORT_ITEM tbminspect_report_item = new TBMINSPECT_REPORT_ITEM();
                        tbminspect_report_item = mpms.TBMINSPECT_REPORT_ITEM
                            .Where(table => table.OVC_PURCH.Equals(tbminspect_report.OVC_PURCH) && table.OVC_PURCH_6.Equals(tbminspect_report.OVC_PURCH_6) && table.OVC_VEN_CST.Equals(tbminspect_report.OVC_VEN_CST) &&
                                table.ONB_ITEM.Equals(sh) && table.ONB_TIMES.Equals(tbminspect_report.ONB_TIMES) && table.ONB_INSPECT_TIMES.Equals(tbminspect_report.ONB_INSPECT_TIMES) && table.ONB_RE_INSPECT_TIMES.Equals(tbminspect_report.ONB_RE_INSPECT_TIMES)).FirstOrDefault();
                        if (tbminspect_report_item != null)
                        {
                            mpms.Entry(tbminspect_report_item).State = EntityState.Deleted;
                            if (strMessage == "")
                                mpms.SaveChanges();
                        }
                        if (checkBoxList.Items[0].Selected == true)
                        {
                            TBMINSPECT_REPORT_ITEM tbminspect_report_item_new = new TBMINSPECT_REPORT_ITEM();
                            tbminspect_report_item_new.OVC_PURCH = tbminspect_report.OVC_PURCH;
                            tbminspect_report_item_new.OVC_PURCH_6 = tbminspect_report.OVC_PURCH_6;
                            tbminspect_report_item_new.OVC_VEN_CST = tbminspect_report.OVC_VEN_CST;
                            tbminspect_report_item_new.ONB_TIMES = tbminspect_report.ONB_TIMES;
                            tbminspect_report_item_new.ONB_INSPECT_TIMES = tbminspect_report.ONB_INSPECT_TIMES;
                            tbminspect_report_item_new.ONB_RE_INSPECT_TIMES = tbminspect_report.ONB_RE_INSPECT_TIMES;
                            tbminspect_report_item_new.OVC_KIND = tbminspect_report.OVC_KIND;
                            tbminspect_report_item_new.ONB_ITEM = sh;
                            if (i > 0 && i < 8)
                                tbminspect_report_item_new.OVC_ITEM_NAME = lab.Text;
                            else if (i > 7 && i < 12)
                                tbminspect_report_item_new.OVC_ITEM_NAME = lab.Text + textBox.Text + lab2.Text;
                            else
                                tbminspect_report_item_new.OVC_ITEM_NAME = txtOther.Text;
                            if (strMessage == "")
                            {
                                mpms.TBMINSPECT_REPORT_ITEM.Add(tbminspect_report_item_new);
                                mpms.SaveChanges();
                            }
                        }
                    }
                    if (strMessage == "")
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                }
                else//新增
                {
                    short INSPECT_TIMES = short.Parse(drpONB_INSPECT_TIMES.Text);
                    short RE_INSPECT_TIMES = short.Parse(drpONB_RE_INSPECT_TIMES.Text);
                    var queryPurch =
                        (from tbm1301 in mpms.TBM1301
                         join tbm1302 in mpms.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                         where purch.Contains(tbm1301.OVC_PURCH)
                         where tbm1302.OVC_PURCH_6.Equals(purch_6)
                         select new
                         {
                             OVC_PURCH = tbm1301.OVC_PURCH,
                             OVC_PURCH_KIND = tbm1301.OVC_PURCH_KIND,
                             OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                             OVC_VEN_CST = tbm1302.OVC_VEN_CST
                         }).FirstOrDefault();
                    var queryIns = mpms.TBMINSPECT_REPORT
                        .Where(o => o.OVC_PURCH.Equals(queryPurch.OVC_PURCH) && o.OVC_PURCH_6.Equals(queryPurch.OVC_PURCH_6))
                        .Where(o => o.ONB_TIMES.Equals(shiptime) && o.ONB_INSPECT_TIMES.Equals(INSPECT_TIMES) && o.ONB_RE_INSPECT_TIMES.Equals(RE_INSPECT_TIMES)).FirstOrDefault();
                    if (queryIns != null)
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "副驗次數及再驗次數重複");
                    else
                    {
                        TBMINSPECT_REPORT tbminspect_report_new = new TBMINSPECT_REPORT();
                        tbminspect_report_new.OVC_PURCH = queryPurch.OVC_PURCH;
                        tbminspect_report_new.OVC_PURCH_6 = queryPurch.OVC_PURCH_6;
                        tbminspect_report_new.OVC_VEN_CST = queryPurch.OVC_VEN_CST;
                        tbminspect_report_new.ONB_TIMES = shiptime;
                        tbminspect_report_new.OVC_KIND = queryPurch.OVC_PURCH_KIND;
                        tbminspect_report_new.OVC_DINSPECT = DateTime.Now.ToString("yyyy-MM-dd");
                        tbminspect_report_new.ONB_INSPECT_TIMES = short.Parse(drpONB_INSPECT_TIMES.Text);
                        tbminspect_report_new.ONB_RE_INSPECT_TIMES = short.Parse(drpONB_RE_INSPECT_TIMES.Text);
                        tbminspect_report_new.OVC_REPORT_DESC = txtOVC_REPORT_DESC.Text;
                        var queryDoName = mpms.TBMRECEIVE_CONTRACT
                            .Where(o => o.OVC_PURCH.Equals(queryPurch.OVC_PURCH))
                            .Where(o => o.OVC_PURCH_6.Equals(queryPurch.OVC_PURCH_6)).FirstOrDefault();
                        tbminspect_report_new.OVC_DO_NAME = queryDoName.OVC_DO_NAME;
                        if (rdoOVC_RESULT_1.SelectedIndex == 0)
                            tbminspect_report_new.OVC_RESULT_1 = "Y";
                        else
                            tbminspect_report_new.OVC_RESULT_1 = "N";
                        tbminspect_report_new.OVC_RESULT_2_PERCENT = txtOVC_RESULT_2_PERCENT.Text;
                        if (rdoOVC_RESULT_2.SelectedIndex == 0)
                            tbminspect_report_new.OVC_RESULT_2 = "Y";
                        else
                            tbminspect_report_new.OVC_RESULT_2 = "N";
                        if (rdoOVC_RESULT_3.SelectedIndex == 0)
                            tbminspect_report_new.OVC_RESULT_3 = "Y";
                        else
                            tbminspect_report_new.OVC_RESULT_3 = "N";
                        if (rdoOVC_RESULT_4.SelectedIndex == 0)
                            tbminspect_report_new.OVC_RESULT_4 = "Y";
                        else
                            tbminspect_report_new.OVC_RESULT_4 = "N";
                        tbminspect_report_new.OVC_RESULT_5 = txtOVC_RESULT_5.Text;
                        if (strMessage == "")
                        {
                            mpms.TBMINSPECT_REPORT.Add(tbminspect_report_new);
                            mpms.SaveChanges();
                        }
                        for (int i = 1; i < 13; i++)
                        {
                            string chk = "CheckBoxList" + i;
                            string tex = "TextBox_" + i;
                            string strlab = "label_" + i;
                            string strlab2 = "label2_" + i;
                            CheckBoxList checkBoxList = (CheckBoxList)tb.FindControl(chk);
                            TextBox textBox = (TextBox)tb.FindControl(tex);
                            Label lab = (Label)tb.FindControl(strlab);
                            Label lab2 = (Label)tb.FindControl(strlab2);
                            short sh = short.Parse(i.ToString());
                            if (checkBoxList.Items[0].Selected == true)
                            {
                                TBMINSPECT_REPORT_ITEM tbminspect_report_item_new = new TBMINSPECT_REPORT_ITEM();
                                tbminspect_report_item_new.OVC_PURCH = queryPurch.OVC_PURCH;
                                tbminspect_report_item_new.OVC_PURCH_6 = queryPurch.OVC_PURCH_6;
                                tbminspect_report_item_new.OVC_VEN_CST = queryPurch.OVC_VEN_CST;
                                tbminspect_report_item_new.ONB_TIMES = short.Parse(txtONB_SHIP_TIMES.Text);
                                tbminspect_report_item_new.ONB_INSPECT_TIMES = short.Parse(drpONB_INSPECT_TIMES.Text);
                                tbminspect_report_item_new.ONB_RE_INSPECT_TIMES = short.Parse(drpONB_RE_INSPECT_TIMES.Text);
                                tbminspect_report_item_new.OVC_KIND = queryPurch.OVC_PURCH_KIND;
                                tbminspect_report_item_new.ONB_ITEM = sh;
                                if (i > 0 && i < 8)
                                    tbminspect_report_item_new.OVC_ITEM_NAME = lab.Text;
                                else if (i > 7 && i < 12)
                                    tbminspect_report_item_new.OVC_ITEM_NAME = lab.Text + textBox.Text + lab2.Text;
                                else
                                    tbminspect_report_item_new.OVC_ITEM_NAME = txtOther.Text;
                                if (strMessage == "")
                                {
                                    mpms.TBMINSPECT_REPORT_ITEM.Add(tbminspect_report_item_new);
                                    mpms.SaveChanges();
                                }
                            }
                        }
                        if (strMessage == "")
                        {
                            Session["isModify_2"] = "1";
                            Session["INSPECT_TIMES"] = INSPECT_TIMES;
                            Session["RE_INSPECT_TIMES"] = RE_INSPECT_TIMES;
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
                        }
                        else
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                    }
                }
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Acceptance_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "驗收情形報告.docx";
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
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Acceptance_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/Acceptance_Temp.pdf";
            string fileName = purch + "驗收情形報告.pdf";
            FCommon.WordToPDF(this, path_temp, wordfilepath, fileName);
        }
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Acceptance_Temp.docx");
            string wordfilepath = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Acceptance_Temp.odt");
            string fileName = purch + "驗收情形報告.odt";
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
                txtONB_SHIP_TIMES.Text = Session["shiptime"].ToString();
                var purch = Session["rowtext"].ToString().Substring(0, 7);
                if (Session["shiptime"] != null)
                    txtONB_SHIP_TIMES.Text = Session["shiptime"].ToString();
                if (Session["INSPECT_TIMES"] == null)
                {
                    drpONB_INSPECT_TIMES.Text = "0";
                    drpONB_RE_INSPECT_TIMES.Text = "0";
                }
                else
                {
                    drpONB_INSPECT_TIMES.Text = Session["INSPECT_TIMES"].ToString();
                    drpONB_RE_INSPECT_TIMES.Text = Session["RE_INSPECT_TIMES"].ToString();
                }
                if (Session["isModify_2"] != null)
                { 
                    var query =
                        from report in mpms.TBMINSPECT_REPORT
                        where report.OVC_PURCH.Equals(purch)
                        where report.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            ONB_TIMES = report.ONB_TIMES,
                            ONB_INSPECT_TIMES = report.ONB_INSPECT_TIMES,
                            ONB_RE_INSPECT_TIMES = report.ONB_RE_INSPECT_TIMES,
                            OVC_REPORT_DESC = report.OVC_REPORT_DESC,
                            OVC_RESULT_1 = report.OVC_RESULT_1,
                            OVC_RESULT_2 = report.OVC_RESULT_2,
                            OVC_RESULT_2_PERCENT = report.OVC_RESULT_2_PERCENT,
                            OVC_RESULT_3 = report.OVC_RESULT_3,
                            OVC_RESULT_4 = report.OVC_RESULT_4,
                            OVC_RESULT_5 = report.OVC_RESULT_5,
                            OVC_RESULT_4_DELAY = report.OVC_RESULT_4_DELAY,
                            OVC_RESULT_4_PUNISH = report.OVC_RESULT_4_PUNISH
                        };
                    foreach (var q in query)
                    {
                        txtOVC_REPORT_DESC.Text = q.OVC_REPORT_DESC;
                        txtOVC_RESULT_4_DELAY.Text = q.OVC_RESULT_4_DELAY;
                        txtOVC_RESULT_4_PUNISH.Text = q.OVC_RESULT_4_PUNISH;
                        if (q.ONB_TIMES.ToString() == Session["shiptime"].ToString() && q.ONB_INSPECT_TIMES.ToString() == drpONB_INSPECT_TIMES.Text && q.ONB_RE_INSPECT_TIMES.ToString() == drpONB_RE_INSPECT_TIMES.Text)
                        {
                            if (Session["isModify_2"] != null)
                            {
                                if (q.OVC_RESULT_1 == "Y")
                                    rdoOVC_RESULT_1.SelectedIndex = 0;
                                else if (q.OVC_RESULT_1 == "N")
                                    rdoOVC_RESULT_1.SelectedIndex = 1;
                                txtOVC_RESULT_2_PERCENT.Text = q.OVC_RESULT_2_PERCENT;
                                if (q.OVC_RESULT_2 == "Y")
                                    rdoOVC_RESULT_2.SelectedIndex = 0;
                                else if (q.OVC_RESULT_2 == "N")
                                    rdoOVC_RESULT_2.SelectedIndex = 1;
                                if (q.OVC_RESULT_3 == "Y")
                                    rdoOVC_RESULT_3.SelectedIndex = 0;
                                else if (q.OVC_RESULT_3 == "N")
                                    rdoOVC_RESULT_3.SelectedIndex = 1;
                                if (q.OVC_RESULT_4 == "Y")
                                    rdoOVC_RESULT_4.SelectedIndex = 0;
                                else if (q.OVC_RESULT_4 == "N")
                                    rdoOVC_RESULT_4.SelectedIndex = 1;
                                txtOVC_RESULT_5.Text = q.OVC_RESULT_5;

                                for (int i = 1; i < 13; i++)
                                {
                                    string chk = "CheckBoxList" + i;
                                    string tex = "TextBox_" + i;
                                    CheckBoxList checkBoxList = (CheckBoxList)tb.FindControl(chk);
                                    TextBox textBox = (TextBox)tb.FindControl(tex);
                                    var queryItem2 =
                                        from report_item in mpms.TBMINSPECT_REPORT_ITEM
                                        where report_item.OVC_PURCH.Equals(purch)
                                        select new
                                        {
                                            ONB_TIMES = report_item.ONB_TIMES,
                                            ONB_INSPECT_TIMES = report_item.ONB_INSPECT_TIMES,
                                            ONB_RE_INSPECT_TIMES = report_item.ONB_RE_INSPECT_TIMES,
                                            ONB_ITEM = report_item.ONB_ITEM,
                                            OVC_ITEM_NAME = report_item.OVC_ITEM_NAME
                                        };
                                    foreach (var qu in queryItem2)
                                    {
                                        if (qu.ONB_ITEM == i && qu.ONB_TIMES.ToString() == Session["shiptime"].ToString() && qu.ONB_INSPECT_TIMES.ToString() == drpONB_INSPECT_TIMES.Text && qu.ONB_RE_INSPECT_TIMES.ToString() == drpONB_RE_INSPECT_TIMES.Text)
                                        {
                                            checkBoxList.Items[0].Selected = true;
                                            if (i >= 8 && i <= 11)
                                            {
                                                string str = Regex.Replace(qu.OVC_ITEM_NAME.Substring(3), "[^0-9]", "");
                                                textBox.Text = str;
                                            }
                                            if (i == 12)
                                                txtOther.Text = qu.OVC_ITEM_NAME;
                                        }
                                            
                                    }
                                }
                                //var queryItem =
                                //    from report_item in mpms.TBMINSPECT_REPORT_ITEM
                                //    where report_item.OVC_PURCH.Equals(purch)
                                //    select new
                                //    {
                                //        ONB_TIMES = report_item.ONB_TIMES,
                                //        ONB_INSPECT_TIMES = report_item.ONB_INSPECT_TIMES,
                                //        ONB_RE_INSPECT_TIMES = report_item.ONB_RE_INSPECT_TIMES,
                                //        ONB_ITEM = report_item.ONB_ITEM
                                //    };
                                //foreach (var qu in queryItem)
                                //{
                                //    if (qu.ONB_TIMES.ToString() == Session["shiptime"].ToString() && qu.ONB_INSPECT_TIMES.ToString() == Session["INSPECT_TIMES"].ToString() && qu.ONB_RE_INSPECT_TIMES.ToString() == Session["RE_INSPECT_TIMES"].ToString())
                                //        chkONB_ITEM.Items[qu.ONB_ITEM - 1].Selected = true;
                                //}
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 驗收情形報告表
        private void ExportToWord()
        {
            string purch = Session["rowtext"].ToString().Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            string WordPath = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Acceptance.docx");
            string SavePath = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Acceptance_Temp.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(WordPath))
                {
                    doc.ReplaceText("[$OVC_PURCH$]", Session["rowtext"].ToString(), false, RegexOptions.None);
                    var query = mpms.TBMINSPECT_REPORT.Where(t => t.OVC_PURCH.Equals(purch) && t.OVC_PURCH_6.Equals(purch_6)).FirstOrDefault();
                    if (query != null)
                    {
                        doc.ReplaceText("[$OVC_REPORT_DESC$]", query.OVC_REPORT_DESC ?? "", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_1Y$]", query.OVC_RESULT_1 == "Y" ? "■" : "□", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_1N$]", query.OVC_RESULT_1 == "N" ? "■" : "□", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_2_PERCENT$]", query.OVC_RESULT_2_PERCENT ?? "", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_2Y$]", query.OVC_RESULT_2 == "Y" ? "■" : "□", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_2N$]", query.OVC_RESULT_2 == "N" ? "■" : "□", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_3Y$]", query.OVC_RESULT_3 == "Y" ? "■" : "□", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_3N$]", query.OVC_RESULT_3 == "N" ? "■" : "□", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_4Y$]", query.OVC_RESULT_4 == "Y" ? "■" : "□", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_4N$]", query.OVC_RESULT_4 == "N" ? "■" : "□", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_4_DELAY$]", query.OVC_RESULT_4_DELAY ?? "", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_4_PUNISH$]", query.OVC_RESULT_4_PUNISH ?? "", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_5$]", query.OVC_RESULT_5 ?? "", false, RegexOptions.None);
                        doc.ReplaceText("[$ONB_ITEM_1$]", query.OVC_RESULT_5 ?? "", false, RegexOptions.None);
                    }
                    else
                    {
                        doc.ReplaceText("[$OVC_REPORT_DESC$]", "", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_1Y$]", "□", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_1N$]", "□", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_2_PERCENT$]", "", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_2Y$]", "□", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_2N$]", "□", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_3Y$]", "□", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_3N$]", "□", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_4Y$]", "□", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_4N$]", "□", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_4_DELAY$]", "", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_4_PUNISH$]", "", false, RegexOptions.None);
                        doc.ReplaceText("[$OVC_RESULT_5$]", "", false, RegexOptions.None);
                        doc.ReplaceText("[$ONB_ITEM_1$]", "", false, RegexOptions.None);
                    }
                    var queryItem = mpms.TBMINSPECT_REPORT_ITEM.Where(t => t.OVC_PURCH.Equals(purch) && t.OVC_PURCH_6.Equals(purch_6));
                    string strItem = "";
                    foreach (var q in queryItem)
                    {
                        for (int i = 1; i < 19; i++)
                        {
                            if (q.ONB_ITEM == i)
                            {
                                strItem += "，" + i;
                                string strONB_ITEM = "[$ONB_ITEM_" + i + "$]";
                                doc.ReplaceText(strONB_ITEM, "■", false, RegexOptions.None);
                                if (i >= 8 && i <= 11)
                                {
                                    string strONB_ITEM_DAY = "[$ONB_ITEM_" + i + "_DAY$]";
                                    string strDay = Regex.Replace(q.OVC_ITEM_NAME.Substring(3), "[^0-9]", "");
                                    doc.ReplaceText(strONB_ITEM_DAY, strDay, false, RegexOptions.None);
                                }
                                if (i == 12 || i == 18)
                                {
                                    string strONB_ITEM_NAME = "[$ONB_ITEM_" + i + "_NAME$]";
                                    doc.ReplaceText(strONB_ITEM_NAME, q.OVC_ITEM_NAME, false, RegexOptions.None);
                                }
                            }
                        }
                    }
                    doc.ReplaceText("[$ITEM$]", strItem.Substring(1), false, RegexOptions.None);
                    for (int i = 1; i < 19; i++)
                    {
                        string strONB_ITEM = "[$ONB_ITEM_" + i + "$]";
                        doc.ReplaceText(strONB_ITEM, "□", false, RegexOptions.None);
                        if (i >= 8 && i <= 11)
                        {
                            string strONB_ITEM_DAY = "[$ONB_ITEM_" + i + "_DAY$]";
                            doc.ReplaceText(strONB_ITEM_DAY, "", false, RegexOptions.None);
                        }
                        if (i == 12 || i == 18)
                        {
                            string strONB_ITEM_NAME = "[$ONB_ITEM_" + i + "_NAME$]";
                            doc.ReplaceText(strONB_ITEM_NAME, "", false, RegexOptions.None);
                        }
                    }
                    doc.SaveAs(SavePath);
                }
                buffer = ms.ToArray();
            }
        }
        #endregion

        #endregion
    }
}