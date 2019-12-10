using System;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;
using System.Web.UI;
using System.IO;
using Xceed.Words.NET;
using Microsoft.International.Formatters;
using System.Globalization;
using System.Web;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E33 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["rowtext"] != null)
            {
                if (!IsPostBack)
                {
                    dataImport();
                }
            }
            else
                FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
        }

        #region Click
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E16.aspx";
            Response.Redirect(send_url);
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            Session["time"] = "0";
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E34.aspx";
            Response.Redirect(send_url);
        }

        protected void btnTakeOver_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            Session["time"] = GV_TakeOver.Cells[3].Text;
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E34.aspx";
            Response.Redirect(send_url);
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            string purch = Session["rowtext"].ToString().Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            short time = short.Parse(GV_TakeOver.Cells[3].Text);
            var tbmCONTRACT_MODIFY = mpms.TBMCONTRACT_MODIFY.Where(o => o.OVC_PURCH.Equals(purch) && o.OVC_PURCH_6.Equals(purch_6) && o.ONB_TIMES.Equals(time)).FirstOrDefault();
            mpms.Entry(tbmCONTRACT_MODIFY).State = EntityState.Deleted;
            mpms.SaveChanges();
            dataImport();
        }

        #region 列印btn
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            PrinterServlet_ExportToWord("WordPDFprint/PrinterServletE33.docx");
            string purch = Session["rowtext"].ToString().Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Signing_Order_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "廠商資料更動簽辦單.docx";
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
        protected void Unnamed_Click(object sender, EventArgs e)
        {
            PrinterServlet_ExportToWord("WordPDFprint/PrinterServletE33.docx");
            var purch = Session["rowtext"].ToString().Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Signing_Order_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/Signing_Order_Temp.pdf";
            string fileName = purch + "廠商資料更動簽辦單.pdf";
            FCommon.WordToPDF(this, path_temp, wordfilepath, fileName);
        }
        protected void Unnamed_Click1(object sender, EventArgs e)
        {
            PrinterServlet_ExportToWord("WordPDFprint/PrinterServletE33.docx");
            var purch = Session["rowtext"].ToString().Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Signing_Order_Temp.docx");
            string wordfilepath = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Signing_Order_Temp.odt");
            string fileName = purch + "廠商資料更動簽辦單.odt";
            FCommon.WordToOdt(this, path_temp, wordfilepath, fileName);
        }
        #endregion
        #endregion

        #region DataImport
        private void dataImport()
        {
            DataTable dt = new DataTable();
            string purch = Session["rowtext"].ToString().Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();

            var query =
                from con in mpms.TBMCONTRACT_MODIFY
                //join conI in mpms.TBMCONTRACT_MODIFY_ITEM on new { con.OVC_PURCH, con.OVC_PURCH_6 } equals new { conI.OVC_PURCH, conI.OVC_PURCH_6 }
                join t1301 in mpms.TBM1301 on con.OVC_PURCH equals t1301.OVC_PURCH
                where con.OVC_PURCH.Equals(purch)
                where con.OVC_PURCH_6.Equals(purch_6)
                select new
                {
                    OVC_PURCH = con.OVC_PURCH,
                    OVC_PURCH_6 = con.OVC_PURCH_6,
                    ONB_TIMES = con.ONB_TIMES,
                    OVC_DMODIFY = con.OVC_DMODIFY,
                    OVC_PUR_IPURCH = t1301.OVC_PUR_IPURCH
                };
            dt = CommonStatic.LinqQueryToDataTable(query);
            FCommon.GridView_dataImport(GV_TBMCONTRACT_MODIFY, dt);
        }
        #endregion

        #region 列印廠商資料更動簽辦單
        void PrinterServlet_ExportToWord(string url)
        {
            string Money = "";
            string path = "";
            string purch = Session["rowtext"].ToString().Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            path = Path.Combine(Request.PhysicalApplicationPath, url);
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    string userid = Session["userid"].ToString();
                    var queryTel = mpms.ACCOUNT.Where(o => o.USER_ID.Equals(userid)).FirstOrDefault();
                    var tbm1301 = mpms.TBM1301.Where(o => o.OVC_PURCH.Equals(purch)).FirstOrDefault();
                    var tbm1302 = mpms.TBM1302.Where(o => o.OVC_PURCH.Equals(purch) && o.OVC_PURCH_6.Equals(purch_6)).FirstOrDefault();
                    var tbmrec = mpms.TBMRECEIVE_CONTRACT.Where(o => o.OVC_PURCH.Equals(purch) && o.OVC_PURCH_6.Equals(purch_6)).FirstOrDefault();
                    ReplaceText("[$TEL$]", queryTel.IUSER_PHONE, doc);
                    ReplaceText("[$OVC_PURCH$]", Session["rowtext"].ToString(), doc);
                    ReplaceText("[$OVC_PUR_IPURCH$]", tbm1301.OVC_PUR_IPURCH, doc);
                    ReplaceText("[$OVC_PUR_NSECTION$]", tbm1301.OVC_PUR_NSECTION, doc);
                    ReplaceText("[$OVC_VEN_TITLE$]", tbm1302.OVC_VEN_TITLE, doc);
                    ReplaceText_TWmoney("[$ONB_MONEY$]", tbm1302.ONB_MONEY, doc);
                    ReplaceText_CheckMoney("[$CH1$]", "[$CH2$]", "[$CH3$]", "[$CH4$]", doc);
                    if (tbm1301.OVC_LAB.Equals("1"))
                    {
                        doc.ReplaceText("[$CH5$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CH6$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    else
                    {
                        doc.ReplaceText("[$CH5$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CH6$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    DateTime date = DateTime.TryParse(tbm1302.OVC_DCONTRACT, out DateTime dt) ? DateTime.Parse(tbm1302.OVC_DCONTRACT) : dt;
                    if (DateTime.TryParse(tbm1302.OVC_DCONTRACT, out dt))
                    {
                        doc.ReplaceText("[$YEAR$]", date.Year.ToString(), false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$MONTH$]", date.Month.ToString(), false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$DAY$]", date.Day.ToString(), false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    else
                    {
                        doc.ReplaceText("[$YEAR$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$MONTH$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$DAY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    ReplaceText("[$ONB_SHIP_TIMES$]", tbmrec.ONB_SHIP_TIMES.ToString(), doc);
                    var tbmsta = mpms.TBMSTATUS
                        .Where(o => o.OVC_PURCH.Equals(purch) && o.OVC_PURCH_6.Equals(purch_6))
                        .Where(o => o.OVC_STATUS.Substring(0, 1).Equals("3"));
                    string status = "3";
                    foreach (var q in tbmsta)
                    {
                        if (int.Parse(status) <= int.Parse(q.OVC_STATUS))
                            status = q.OVC_STATUS;
                    }
                    switch (status)
                    {
                        case "3":
                        case "31":
                        case "32":
                            doc.ReplaceText("[$CH7$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH8$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH9$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH10$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH11$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            break;
                        case "33":
                        case "34":
                            doc.ReplaceText("[$CH7$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH8$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH9$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH10$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH11$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            break;
                        case "35":
                            doc.ReplaceText("[$CH7$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH8$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH9$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH10$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH11$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            break;
                        case "36":
                            doc.ReplaceText("[$CH7$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH8$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH9$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH10$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH11$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            break;
                        case "37":
                            doc.ReplaceText("[$CH7$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH8$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH9$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH10$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH11$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            break;
                        default:
                            doc.ReplaceText("[$CH7$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH8$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH9$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH10$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CH11$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            break;
                    }

                    doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/Signing_Order_Temp.docx");
                }
                buffer = ms.ToArray();
            }
        }
        void ReplaceText(string af, string bf, DocX doc)
        {
            if (bf != null)
                doc.ReplaceText(af, bf, false, System.Text.RegularExpressions.RegexOptions.None);
            doc.ReplaceText(af, "", false, System.Text.RegularExpressions.RegexOptions.None);
        }
        void ReplaceText_TWmoney(string af, object bf, DocX doc)
        {
            if (bf != null)
            {
                string money = EastAsiaNumericFormatter.FormatWithCulture("Lc", decimal.Parse(bf.ToString()), null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                doc.ReplaceText(af, money + "元整", false, System.Text.RegularExpressions.RegexOptions.None);
            }
            doc.ReplaceText(af, "零元整", false, System.Text.RegularExpressions.RegexOptions.None);
        }

        void ReplaceText_CheckMoney(string ch1, string ch2, string ch3, string ch4, DocX doc)
        {
            string purch = Session["rowtext"].ToString().Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
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
                    if (q.ONB_MONEY < 1000000 || q.ONB_MONEY == null)
                    {
                        doc.ReplaceText(ch1, "■", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch2, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch3, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch4, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    else if (q.ONB_MONEY >= 1000000 && q.ONB_MONEY < 10000000)
                    {
                        doc.ReplaceText(ch1, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch2, "■", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch3, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch4, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    else if (q.ONB_MONEY >= 10000000 && q.ONB_MONEY < 20000000)
                    {
                        doc.ReplaceText(ch1, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch2, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch3, "■", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch4, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    else if (q.ONB_MONEY >= 20000000)
                    {
                        doc.ReplaceText(ch1, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch2, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch3, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch4, "■", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                }
                else
                {
                    if (q.ONB_MONEY < 1000000 || q.ONB_MONEY == null)
                    {
                        doc.ReplaceText(ch1, "■", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch2, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch3, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch4, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    else if (q.ONB_MONEY >= 1000000 && q.ONB_MONEY < 50000000)
                    {
                        doc.ReplaceText(ch1, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch2, "■", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch3, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch4, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    else if (q.ONB_MONEY >= 50000000 && q.ONB_MONEY < 100000000)
                    {
                        doc.ReplaceText(ch1, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch2, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch3, "■", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch4, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    else if (q.ONB_MONEY >= 100000000)
                    {
                        doc.ReplaceText(ch1, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch2, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch3, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText(ch4, "■", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                }
            }
        }
        #endregion
    }
}