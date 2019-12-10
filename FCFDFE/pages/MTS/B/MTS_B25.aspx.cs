using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace FCFDFE.pages.MTS.B
{
    public partial class MTS_B25 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        
        #region 副程式
        private void list_dataImportV(ListControl list, DataTable dt, string textField, string valueField)
        {
            list.DataSource = dt;
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataBind();
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            if (!IsPostBack)
            {
                FCommon.Controls_Attributes("readonly", "true", txtODT_INS_DATE_S,txtODT_INS_DATE_E);
                DateTime dateNow = DateTime.Now;
                #region 匯入下拉式選單
                string strFirstText = "不限定";
                CommonMTS.list_dataImport_DEPT_CLASS(drpOVC_MILITARY_TYPE, true, strFirstText); //軍種
                #endregion
                txtODT_INS_DATE_S.Text = FCommon.getDateTime(dateNow.AddMonths(-3));
                txtODT_INS_DATE_E.Text = FCommon.getDateTime(dateNow);
            }
        }

        #region ~Click
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedValue;
            //string strOVC_MILITARY_TYPE_Text = drpOVC_MILITARY_TYPE.SelectedItem != null ? drpOVC_MILITARY_TYPE.SelectedItem.Text : "";
            string strODT_INS_DATE_S = txtODT_INS_DATE_S.Text;
            string strODT_INS_DATE_E = txtODT_INS_DATE_E.Text;
            string strODT_INS_DATE = rdoODT_INS_DATE.SelectedValue.ToString();

            bool boolODT_INS_DATE = strODT_INS_DATE.Equals("2");
            bool boolODT_INS_DATE_S = DateTime.TryParse(strODT_INS_DATE_S, out DateTime dateODT_INS_DATE_S);
            bool boolODT_INS_DATE_E = DateTime.TryParse(strODT_INS_DATE_E, out DateTime dateODT_INS_DATE_E);

            if (strOVC_MILITARY_TYPE.Equals(string.Empty) && !boolODT_INS_DATE)
                strMessage += "<P> 至少填入一個選項！ </p>";
            else
            {
                if (boolODT_INS_DATE && !(boolODT_INS_DATE_S && boolODT_INS_DATE_E))
                    strMessage += "<P> 投保日期 不完全！ </p>";
            }

            if (strMessage.Equals(string.Empty))
            {
                var query =
                 from einn in MTSE.TBGMT_EINN.AsEnumerable()
                 join einf in MTSE.TBGMT_EINF on einn.OVC_INF_NO equals einf.OVC_INF_NO
                 join edf in MTSE.TBGMT_EDF on einn.OVC_EDF_NO equals edf.OVC_EDF_NO
                 join bld in MTSE.TBGMT_BLD on edf.OVC_BLD_NO equals bld.OVC_BLD_NO
                 join dept_class in MTSE.TBGMT_DEPT_CLASS on einn.OVC_MILITARY_TYPE equals dept_class.OVC_CLASS
                 //join com in MTSE.TBGMT_COMPANY on einf.CO_SN equals com.CO_SN into comT
                 //from com in comT.DefaultIfEmpty()
                 select new
                 {
                     OVC_EINN_NO = einn.OVC_EINN_NO,
                     ODT_APPLY_DATE = FCommon.getDateTime(einf.ODT_MODIFY_DATE),
                     OVC_MILITARY_TYPE = einn.OVC_MILITARY_TYPE ?? "",
                     OVC_CLASS_NAME = dept_class.OVC_CLASS_NAME,
                     //OVC_SHIP_COMPANY = com != null ? com.OVC_COMPANY : "",//"中保公司"
                     OVC_SHIP_COMPANY = "",
                     ONB_INS_AMOUNT = einn.OVC_FINAL_INS_AMOUNT,
                     ODT_INS_DATE = FCommon.getDateTime(einn.ODT_MODIFY_DATE)
                 };
                if (!strOVC_MILITARY_TYPE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_MILITARY_TYPE.Equals(strOVC_MILITARY_TYPE));

                if (boolODT_INS_DATE)
                    query = query.Where(table => DateTime.TryParse(table.ODT_INS_DATE, out DateTime dateODT_INS_DATE) &&
                        DateTime.Compare(dateODT_INS_DATE, dateODT_INS_DATE_S) >= 0 &&
                        DateTime.Compare(dateODT_INS_DATE, dateODT_INS_DATE_E) <= 0);
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);

                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                
                foreach (DataRow dr in dt.Rows)
                {
                    if (DateTime.TryParse(dr["ODT_INS_DATE"].ToString(), out DateTime date))
                    {
                        var queryCom =
                            (from rate in MTSE.TBGMT_INSRATE.AsEnumerable()
                             where rate.ODT_START_DATE != null ? DateTime.Compare(DateTime.Parse(rate.ODT_START_DATE.ToString()), date) <= 0 : false
                             where rate.ODT_END_DATE != null ? DateTime.Compare(DateTime.Parse(rate.ODT_END_DATE.ToString()), date) >= 0 : false
                             select rate).FirstOrDefault();
                        dr["OVC_SHIP_COMPANY"] = queryCom == null ? "" : queryCom.OVC_INSCOMPNAY;
                    }
                    else
                        dr["OVC_SHIP_COMPANY"] = "";
                }

                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_EINN, dt);
                ViewState["dt"] = dt;
                if (query.Any())
                    btnPrint.Visible = true;
                else
                    btnPrint.Visible = false;
            }
            else
                FCommon.AlertShow(PnWarning, "danger", "系統訊息", strMessage);
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Insurance_schedule();
            //string file_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/IS_Temp.pdf");
            //string file_name = "投保明細表.pdf";
            //FCommon.WordToPDF(this, Insurance_schedule(), file_temp, file_name);
        }
        #endregion

        #region GridView
        protected void GV_TBGMT_EINN_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void GV_TBGMT_EINN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //GridViewRow gvr = e.Row;
            //if (gvr.RowType == DataControlRowType.DataRow)
            //{
            //    if (DateTime.TryParse(gvr.Cells[3].Text, out DateTime dt))
            //    {
            //        var query =
            //            (from rate in MTSE.TBGMT_INSRATE.AsEnumerable()
            //             where rate.ODT_START_DATE != null ? DateTime.Compare(DateTime.Parse(rate.ODT_START_DATE.ToString()), dt) <= 0 : false
            //             where rate.ODT_END_DATE != null ? DateTime.Compare(DateTime.Parse(rate.ODT_END_DATE.ToString()), dt) >= 0 : false
            //             select rate).FirstOrDefault();
            //        gvr.Cells[3].Text = query == null ? "" : query.OVC_INSCOMPNAY;
            //    }
            //}
        }

        protected void GV_TBGMT_EINN_RowCreated(object sender, GridViewRowEventArgs e)
        {
            GridView gv = (GridView)sender;
            decimal decTemp, decTitle = 0;
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                GridViewRow Tgvr = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Insert);
                TableCell cell1 = new TableCell();
                TableCell cell2 = new TableCell();
                TableCell cell3 = new TableCell();
                TableCell cell4 = new TableCell();
                TableCell cell5 = new TableCell();
                TableCell cell6 = new TableCell();
                cell1.Text = "共" + gv.Rows.Count + "筆記錄";
                for (int i = 0; i < gv.Rows.Count; i++)
                {
                    decTitle += decimal.TryParse(gv.Rows[i].Cells[4].Text, out decTemp) ? decTemp : 0;
                }
                cell5.Text = decTitle.ToString();
                Tgvr.Controls.Add(cell1);
                Tgvr.Controls.Add(cell2);
                Tgvr.Controls.Add(cell3);
                Tgvr.Controls.Add(cell4);
                Tgvr.Controls.Add(cell5);
                Tgvr.Controls.Add(cell6);
                GV_TBGMT_EINN.Controls[0].Controls.Add(Tgvr);
            }
        }
        #endregion

        #region 投保明細列印
        private void Insurance_schedule()
        {
            #region 權限問題無法使用
            //string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/投保明細表.docx");
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    using (Xceed.Words.NET.DocX doc = Xceed.Words.NET.DocX.Load(path))
            //    {
            //        string today = (int.Parse(DateTime.Now.Year.ToString()) - 1911).ToString() + DateTime.Now.ToString("年MM月dd日");
            //        doc.ReplaceText("[$TODAY$]", today, false, System.Text.RegularExpressions.RegexOptions.None);
            //        var groceryListTable = doc.Tables.FirstOrDefault(table => table.TableCaption == "table");
            //        var rowPattern = groceryListTable.Rows[1];
            //        rowPattern.Remove();
            //        if (groceryListTable != null)
            //        {
            //            if (GV_TBGMT_EINN.Rows.Count > 0)
            //            {
            //                for (var i = 0; i < GV_TBGMT_EINN.Rows.Count; i++)
            //                {
            //                    var newItem = groceryListTable.InsertRow(rowPattern, groceryListTable.RowCount);
            //                    newItem.ReplaceText("[$OVC_EINN_NO$]", GV_TBGMT_EINN.Rows[i].Cells[0].Text.Replace("&nbsp;", ""));
            //                    newItem.ReplaceText("[$ODT_APPLY_DATE$]", GV_TBGMT_EINN.Rows[i].Cells[1].Text.Replace("&nbsp;", ""));
            //                    newItem.ReplaceText("[$OVC_CLASS_NAME$]", GV_TBGMT_EINN.Rows[i].Cells[2].Text.Replace("&nbsp;", ""));
            //                    newItem.ReplaceText("[$OVC_SHIP_COMPANY$]", GV_TBGMT_EINN.Rows[i].Cells[3].Text.Replace("&nbsp;", ""));
            //                    newItem.ReplaceText("[$ONB_INS_AMOUNT$]", GV_TBGMT_EINN.Rows[i].Cells[4].Text.Replace("&nbsp;", ""));
            //                    newItem.ReplaceText("[$ODT_INS_DATE$]", GV_TBGMT_EINN.Rows[i].Cells[5].Text.Replace("&nbsp;", ""));
            //                }
            //            }
            //        }
            //        doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/IS_Temp.docx");
            //    }
            //}
            //string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/IS_Temp.docx");
            //return path_temp;
            #endregion

            if (ViewState["dt"] != null && ViewState["dt"] is DataTable)
            {
                string today = (int.Parse(DateTime.Now.Year.ToString()) - 1911).ToString() + DateTime.Now.ToString("年MM月dd日");
                DataTable dt = (DataTable)ViewState["dt"];
                string path = Request.PhysicalApplicationPath;//取得檔案絕對路徑
                BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
                Font ChTitle = new Font(bfChinese, 12, Font.BOLD);
                Font ChTabTitle = new Font(bfChinese, 8f);
                Font ChFont = new Font(bfChinese, 7f);
                var doc1 = new Document(PageSize.A4, 0, 0, 50, 80);
                MemoryStream Memory = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
                doc1.Open();

                PdfPTable table1 = new PdfPTable(6);
                table1.TotalWidth = 1200F;
                table1.SetWidths(new float[] { 9, 7, 5, 5, 9, 7});
                table1.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table1.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell title = new PdfPCell(new Phrase("投保明細表\n", ChTitle));
                title.VerticalAlignment = Element.ALIGN_MIDDLE;
                title.HorizontalAlignment = Element.ALIGN_CENTER;
                title.Border = Rectangle.NO_BORDER;
                title.Colspan = 6;
                table1.AddCell(title);
                PdfPCell title_time = new PdfPCell(new Phrase("", ChTitle));
                title_time.VerticalAlignment = Element.ALIGN_MIDDLE;
                title_time.HorizontalAlignment = Element.ALIGN_CENTER;
                title_time.Border = Rectangle.NO_BORDER;
                title_time.Colspan = 4;
                table1.AddCell(title_time);
                PdfPCell title_time_2 = new PdfPCell(new Phrase("印表日期：" + today + "\n\n", ChTabTitle));
                title_time_2.VerticalAlignment = Element.ALIGN_MIDDLE;
                title_time_2.HorizontalAlignment = Element.ALIGN_CENTER;
                title_time_2.Border = Rectangle.NO_BORDER;
                title_time_2.Colspan = 2;
                table1.AddCell(title_time_2);
                
                table1.AddCell(new Phrase("投保通知書編號", ChTabTitle));
                table1.AddCell(new Phrase("結報日期", ChTabTitle));
                table1.AddCell(new Phrase("軍種", ChTabTitle));
                table1.AddCell(new Phrase("承保商", ChTabTitle));
                table1.AddCell(new Phrase("保費金額(新台幣)", ChTabTitle));
                table1.AddCell(new Phrase("支付日期", ChTabTitle));

                int i = 0;
                decimal decTitle = 0;
                string strFormat_Money = "#,0";
                foreach (DataRow dr in dt.Rows)
                {
                    string strNo = (i + 1).ToString(); //序號
                    decimal.TryParse(dr["ONB_INS_AMOUNT"].ToString(), out decimal decONB_INS_AMOUNT);

                    table1.AddCell(new Phrase(dr["OVC_EINN_NO"].ToString(), ChFont));
                    table1.AddCell(new Phrase(dr["ODT_APPLY_DATE"].ToString(), ChFont));
                    table1.AddCell(new Phrase(dr["OVC_CLASS_NAME"].ToString(), ChFont));
                    table1.AddCell(new Phrase(dr["OVC_SHIP_COMPANY"].ToString(), ChFont));
                    table1.AddCell(new Phrase(decONB_INS_AMOUNT.ToString(strFormat_Money), ChFont));
                    table1.AddCell(new Phrase(dr["ODT_INS_DATE"].ToString(), ChFont));

                    i++;
                    decTitle += decONB_INS_AMOUNT;
                }
                table1.AddCell(new Phrase("共" + i.ToString() + "筆記錄", ChFont));
                table1.AddCell(new Phrase("", ChFont));
                table1.AddCell(new Phrase("", ChFont));
                table1.AddCell(new Phrase("", ChFont));
                table1.AddCell(new Phrase(decTitle.ToString(strFormat_Money), ChFont));
                table1.AddCell(new Phrase("", ChFont));

                doc1.Add(table1);
                doc1.Close();

                string strFileName = "投保明細表.pdf";
                FCommon.DownloadFile(this, strFileName, Memory);
            }
        }
        #endregion
    }
}