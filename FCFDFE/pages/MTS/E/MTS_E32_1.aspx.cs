using System;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace FCFDFE.pages.MTS.E
{
    public partial class MTS_E32_1 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities mtse = new MTSEntities();
        GMEntities gme = new GMEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    if (Session["userid"] != null)
                    {
                        string strUser = Session["userid"].ToString();
                        var query = gme.ACCOUNTs.Where(id => id.USER_ID == strUser).FirstOrDefault();
                        if (query.DEPT_SN != null)
                        {
                            string strdept = query.DEPT_SN;
                            var querydept = gme.TBMDEPTs.Where(id => id.OVC_DEPT_CDE == strdept).FirstOrDefault();
                            lblDept.Text = "國防部國防採購室";//querydept.OVC_ONNAME;
                            FCommon.Controls_Attributes("readonly", "true", txtOvcApplyDate1, txtOvcApplyDate2);
                            txtOvcApplyDate2.Text = DateTime.Now.ToString("yyyy-MM-dd");
                            txtOvcApplyDate1.Text = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(-7).ToString("yyyy-MM-dd");
                        }
                    }

                    bool boolimport = false;

                    string strOvcTofNo, strOvcIsPaid, strOvcBudget, strOvcPurposeType, strOdtApplyDate1, strOdtApplyDate2, strchkOdtApplyDate;

                    if(FCommon.getQueryString(this, "OvcTofNo",out strOvcTofNo, true))
                    {
                        txtOvcTofNo.Text = strOvcTofNo;
                        boolimport = true;
                    }
                    if (FCommon.getQueryString(this, "OvcIsPaid", out strOvcIsPaid, true))
                        FCommon.list_setValue(drpOvcIsPaid, strOvcIsPaid);
                    if (FCommon.getQueryString(this, "OvcBudget", out strOvcBudget, true))
                        FCommon.list_setValue(drpOvcBudget, strOvcBudget);
                    if (FCommon.getQueryString(this, "OvcPurposeType", out strOvcPurposeType, true))
                        FCommon.list_setValue(drpOvcPurposeType, strOvcPurposeType);
                    if (FCommon.getQueryString(this, "OdtApplyDate1", out strOdtApplyDate1, true))
                        txtOvcApplyDate1.Text = strOdtApplyDate1;
                    if (FCommon.getQueryString(this, "OdtApplyDate2", out strOdtApplyDate2, true))
                        txtOvcApplyDate2.Text = strOdtApplyDate2;
                    if (FCommon.getQueryString(this, "chkOdtApplyDate", out strchkOdtApplyDate, true))
                        FCommon.list_setValue(chkOdtApplyDate, strchkOdtApplyDate);
                    if (boolimport)
                        dataImport();
                }
            }
        }

        private void dataImport()
        {
            string strOvcTofNo = txtOvcTofNo.Text;
            string strOvcIsPaid = drpOvcIsPaid.SelectedItem.ToString();
            string strOvcBudget = drpOvcBudget.SelectedItem.ToString();
            string strOvcPurposeType = drpOvcPurposeType.SelectedValue.ToString();
            string strOdtApplyDate1 = txtOvcApplyDate1.Text;
            string strOdtApplyDate2 = txtOvcApplyDate2.Text;
            string strchkOdtApplyDate="";
            try
            {
               strchkOdtApplyDate = chkOdtApplyDate.SelectedItem.Text;
            }
            catch{  }


            ViewState["OvcTofNo"] = strOvcTofNo;
            ViewState["OvcIsPaid"] = strOvcIsPaid;
            ViewState["OvcBudget"] = strOvcBudget;
            ViewState["OvcPurposeType"] = strOvcPurposeType;
            ViewState["OdtApplyDate1"] = strOdtApplyDate1;
            ViewState["OdtApplyDate2"] = strOdtApplyDate2;
            ViewState["chkOdtApplyDate"] = strchkOdtApplyDate;


            var totalcount = chkOdtApplyDate.Items.Cast<System.Web.UI.WebControls.ListItem>().Where(item => item.Selected).Count();

            if (strOvcTofNo.Equals(string.Empty) && strOvcIsPaid.Equals("不限定") &&
                strOvcBudget.Equals("不限定") && strOvcPurposeType.Equals("不限定") &&
                strOdtApplyDate1.Equals(string.Empty) && strOdtApplyDate2.Equals(string.Empty) &&
                totalcount == 0)
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請至少填寫一項條件");
            }
            else
            {
                var query =
                                from tof in mtse.TBGMT_TOF
                                select new
                                {
                                    OVC_TOF_NO = tof.OVC_TOF_NO,
                                    OVC_BUDGET = tof.OVC_BUDGET,
                                    OVC_PURPOSE_TYPE = tof.OVC_PURPOSE_TYPE,
                                    OVC_ABSTRACT = tof.OVC_ABSTRACT,
                                    ONB_AMOUNT = tof.ONB_AMOUNT,
                                    OVC_NOTE = tof.OVC_NOTE,
                                    OVC_SECTION = tof.OVC_SECTION,
                                    ODT_APPLY_DATE = tof.ODT_APPLY_DATE,
                                    OVC_APPLY_ID = tof.OVC_APPLY_ID,
                                    OVC_IS_PAID = tof.OVC_IS_PAID
                                };
                if (!strOvcTofNo.Equals(string.Empty))
                    query = query.Where(table => table.OVC_TOF_NO.Contains(strOvcTofNo));
                if (!strOvcIsPaid.Equals("不限定"))
                    query = query.Where(table => table.OVC_IS_PAID == strOvcIsPaid);
                if (!strOvcBudget.Equals("不限定"))
                    query = query.Where(table => table.OVC_BUDGET == strOvcBudget);
                if (!strOvcPurposeType.Equals("不限定"))
                    query = query.Where(table => table.OVC_PURPOSE_TYPE == strOvcPurposeType);
                if (totalcount == 0)
                {
                    if (!strOdtApplyDate1.Equals(string.Empty))
                    {
                        DateTime d1 = Convert.ToDateTime(strOdtApplyDate1);
                        query = query.Where(table => table.ODT_APPLY_DATE >= d1);
                    }
                    if (!strOdtApplyDate2.Equals(string.Empty))
                    {
                        DateTime d2 = Convert.ToDateTime(strOdtApplyDate2);
                        query = query.Where(table => table.ODT_APPLY_DATE >= d2);
                    }
                }
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                DataTable dt = new DataTable();
                dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_TOF, dt);
            }

        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OvcTofNo", ViewState["OvcTofNo"], true);
            FCommon.setQueryString(ref strQueryString, "OvcIsPaid", ViewState["OvcIsPaid"], true);
            FCommon.setQueryString(ref strQueryString, "OvcBudget", ViewState["OvcBudget"], true);
            FCommon.setQueryString(ref strQueryString, "OvcPurposeType", ViewState["OvcPurposeType"], true);
            FCommon.setQueryString(ref strQueryString, "OdtApplyDate1", ViewState["OdtApplyDate1"], true);
            FCommon.setQueryString(ref strQueryString, "OdtApplyDate2", ViewState["OdtApplyDate2"], true);
            FCommon.setQueryString(ref strQueryString, "chkOdtApplyDate", ViewState["chkOdtApplyDate"], true);
            return strQueryString;
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }

        protected void GV_TBGMT_TOF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBGMT_TOF.DataKeys[gvrIndex].Value.ToString();
            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(id));
            string strQueryString = "";
            strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);
            switch (e.CommandName)
            {
                case "btnModify":
                    string str_url_Modify;
                    str_url_Modify = $"MTS_E32_2.aspx{strQueryString}";
                    Response.Redirect(str_url_Modify);
                    break;
                case "btnDel":
                    string str_url_Del;
                    str_url_Del = $"MTS_E32_3.aspx{strQueryString}";
                    Response.Redirect(str_url_Del);
                    break;
                case "btnPrint":
                    pdfprint(id);
                    break;
                default:
                    break;
            }

        }
        private void pdfprint(string strOVC_TOF_NO)
        {
            var query = mtse.TBGMT_TOF.Where(table => table.OVC_TOF_NO == strOVC_TOF_NO).FirstOrDefault();
            string doctitle = "國防部國防採購室" + query.OVC_SECTION + "預算支用結報申請表";
            int onbamount = Convert.ToInt32(query.ONB_AMOUNT);
            string onb = onbamount.ToString("#,0");
            DateTime currentTime = System.DateTime.Now;
            int y = currentTime.Year;
            int d = currentTime.Day;
            int m = currentTime.Month;

            string path = Request.PhysicalApplicationPath;//取得檔案絕對路徑
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChTitle = new Font(bfChinese, 14, Font.BOLD);
            Font ChFont = new Font(bfChinese, 12);
            var doc1 = new Document(PageSize.A4, 50, 50, 50, 50);
            MemoryStream Memory = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            doc1.Open();
            PdfPTable table1 = new PdfPTable(2);
            table1.TotalWidth = 700F;
            table1.SetWidths(new float[] { 1,1});
            table1.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table1.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            table1.DefaultCell.SetLeading(1f, 1f);//行距
            table1.DefaultCell.Padding = 5; 
            table1.DefaultCell.PaddingTop = 15;
            table1.DefaultCell.PaddingBottom = 15;
            
            PdfPCell title = new PdfPCell(new Phrase(doctitle, ChTitle));
            title.BorderWidthBottom = 0;
            title.VerticalAlignment = Element.ALIGN_MIDDLE;
            title.HorizontalAlignment = Element.ALIGN_CENTER;
            title.PaddingTop = 5;
            title.Colspan = 2;
            table1.AddCell(title);
            PdfPCell title2 = new PdfPCell(new Phrase(y.ToString()+"年"+m.ToString()+"月"+d.ToString()+"日   ", ChFont));
            title2.BorderWidthBottom = 0;
            title2.BorderWidthTop = 0;
            title2.VerticalAlignment = Element.ALIGN_MIDDLE;
            title2.HorizontalAlignment = Element.ALIGN_RIGHT;
            title2.PaddingRight = 10;
            title2.PaddingTop = 5;
            title2.PaddingBottom = 5;
            title2.Colspan = 2;
            table1.AddCell(title2);
            PdfPCell title3 = new PdfPCell(new Phrase("字第"+query.OVC_TOF_NO+"號   ", ChFont));
            title3.BorderWidthBottom = 0;
            title3.BorderWidthTop = 0;
            title3.VerticalAlignment = Element.ALIGN_MIDDLE;
            title3.HorizontalAlignment = Element.ALIGN_RIGHT;
            title3.PaddingRight = 10;
            title3.PaddingTop = 0;
            title3.PaddingBottom = 5;
            title3.Colspan = 2;
            table1.AddCell(title3);

            table1.AddCell(new Phrase("預算支用工作計畫及編號", ChFont));
            table1.AddCell(new Phrase("採購及外購軍品作業費", ChFont));
            table1.AddCell(new Phrase("用         途        別", ChFont));
            table1.AddCell(new Phrase(query.OVC_PURPOSE_TYPE, ChFont));
            table1.AddCell(new Phrase("摘                   要", ChFont));
            table1.AddCell(new Phrase(query.OVC_ABSTRACT, ChFont));
            table1.AddCell(new Phrase("金                   額", ChFont));
            table1.AddCell(new Phrase("$"+onb+ "元", ChFont));
            table1.AddCell(new Phrase("備                   考\n( 支用月份、通知單編號 )", ChFont));
            table1.AddCell(new Phrase(query.OVC_NOTE, ChFont));
            table1.AddCell(new Phrase("擬                   辦", ChFont));
            table1.AddCell(new Phrase("核                   示", ChFont));
            table1.AddCell(new Phrase(query.OVC_PLN_CONTENT, ChFont));
            table1.AddCell(new Phrase("\n\n\n", ChFont));
            
            string fileName = System.Web.HttpUtility.UrlEncode("保留索賠及撤銷清單.pdf");

            doc1.Add(table1);
            doc1.Close();
            Response.Clear();//瀏覽器上顯示
            Response.AddHeader("Content-disposition", "attachment;filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.Flush();
            Response.End();

        }
        protected void GV_TBGMT_TOF_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}