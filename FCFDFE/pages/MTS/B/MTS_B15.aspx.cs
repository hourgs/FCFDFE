using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace FCFDFE.pages.MTS.B
{
    public partial class MTS_B15 : Page
    {

        private MTSEntities mtse = new MTSEntities();
        Common FCommon = new Common();
        public string strMenuName = "", strMenuNameItem = "";

        #region 副程式

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtODT_INS_DATE_S, txtODT_INS_DATE_E, txtODT_CREATE_DATE_S, txtODT_CREATE_DATE_E);
                    DateTime dateNow = DateTime.Now;
                    #region 匯入下拉式選單
                    string strFirstText = "不限定";
                    CommonMTS.list_dataImport_DEPT_CLASS(drpOVC_MILITARY_TYPE, false); //軍種
                    #endregion
                    string strDate_S = FCommon.getDateTime(dateNow.AddMonths(-3)), 
                           strDate_E = FCommon.getDateTime(dateNow);
                    txtODT_INS_DATE_S.Text = strDate_S;
                    txtODT_INS_DATE_E.Text = strDate_E;
                    txtODT_CREATE_DATE_S.Text = strDate_S;
                    txtODT_CREATE_DATE_E.Text = strDate_E;
                }
            }
        }

        #region ~Click
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedValue;
            string strODT_INS_DATE = rdoODT_INS_DATE.SelectedValue;
            string strODT_INS_DATE_S = txtODT_INS_DATE_S.Text;
            string strODT_INS_DATE_E = txtODT_INS_DATE_E.Text;
            string strODT_CREATE_DATE = rdoODT_CREATE_DATE.SelectedValue;
            string strODT_CREATE_DATE_S = txtODT_CREATE_DATE_S.Text;
            string strODT_CREATE_DATE_E = txtODT_CREATE_DATE_E.Text;
            string strOVC_PURCH_NO = txtOVC_PURCH_NO.Text;

            bool boolODT_INS_DATE = strODT_INS_DATE.Equals("2");
            bool boolODT_INS_DATE_S = DateTime.TryParse(strODT_INS_DATE_S, out DateTime dateODT_INS_DATE_S);
            bool boolODT_INS_DATE_E = DateTime.TryParse(strODT_INS_DATE_E, out DateTime dateODT_INS_DATE_E);
            bool boolODT_CREATE_DATE = strODT_CREATE_DATE.Equals("2");
            bool boolODT_CREATE_DATE_S = DateTime.TryParse(strODT_CREATE_DATE_S, out DateTime dateODT_CREATE_DATE_S);
            bool boolODT_CREATE_DATE_E = DateTime.TryParse(strODT_CREATE_DATE_E, out DateTime dateODT_CREATE_DATE_E);
            if (boolODT_INS_DATE && !(boolODT_INS_DATE_S && boolODT_INS_DATE_E))
                strMessage += "<P> 投保日期 不完全！ </p>";
            if (boolODT_CREATE_DATE && !(boolODT_CREATE_DATE_S && boolODT_CREATE_DATE_E))
                strMessage += "<P> 建檔日期 不完全！ </p>";

            if (strMessage.Equals(string.Empty))
            {
                //var query_com =
                //    from iinf in mtse.TBGMT_IINF
                //    join com in mtse.TBGMT_COMPANY on iinf.CO_SN equals com.CO_SN
                //    where com.OVC_CO_TYPE.Equals("1")
                //    select new
                //    {
                //        OVC_INF_NO = iinf.OVC_INF_NO,
                //        OVC_COMPANY = com.OVC_COMPANY
                //    };
                var query =
                    from iinn in mtse.TBGMT_IINN.AsEnumerable()
                    where iinn.OVC_MILITARY_TYPE.Equals(strOVC_MILITARY_TYPE)
                    join dept_class in mtse.TBGMT_DEPT_CLASS on iinn.OVC_MILITARY_TYPE equals dept_class.OVC_CLASS into classTemp
                    from dept_class in classTemp.DefaultIfEmpty()
                    select new
                    {
                        OVC_IINN_NO = iinn.OVC_IINN_NO,
                        OVC_MILITARY_TYPE = dept_class != null ? dept_class.OVC_CLASS_NAME : "",
                        OVC_PURCH_NO = iinn.OVC_PURCH_NO != null ? iinn.OVC_PURCH_NO : "",
                        OVC_BLD_NO = iinn.OVC_BLD_NO,
                        ONB_INS_AMOUNT = iinn.ONB_INS_AMOUNT,
                        OVC_DELIVERY_CONDITION = iinn.OVC_DELIVERY_CONDITION,

                        ODT_CREATE_DATE = iinn.ODT_CREATE_DATE.ToString(),
                        ODT_INS_DATE = iinn.ODT_INS_DATE.ToString(),
                    };

                if (!strOVC_PURCH_NO.Equals(string.Empty))
                    query = query.Where(t => t.OVC_PURCH_NO.Contains(strOVC_PURCH_NO));
                if (boolODT_INS_DATE)
                    query = query.Where(table => DateTime.TryParse(table.ODT_INS_DATE, out DateTime dateODT_INS_DATE) &&
                        DateTime.Compare(dateODT_INS_DATE, dateODT_INS_DATE_S) >= 0 &&
                        DateTime.Compare(dateODT_INS_DATE, dateODT_INS_DATE_E) <= 0);
                if (boolODT_CREATE_DATE)
                    query = query.Where(table => DateTime.TryParse(table.ODT_CREATE_DATE, out DateTime dateODT_CREATE_DATE) &&
                        DateTime.Compare(dateODT_CREATE_DATE, dateODT_CREATE_DATE_S) >= 0 &&
                        DateTime.Compare(dateODT_CREATE_DATE, dateODT_CREATE_DATE_E) <= 0);
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);

                DataTable dt = CommonStatic.LinqQueryToDataTable(query);

                foreach (DataRow dr in dt.Rows)
                {
                    if (DateTime.TryParse(dr["ODT_INS_DATE"].ToString(), out DateTime date))
                    {
                        var queryIns =
                            (from rate in mtse.TBGMT_INSRATE.AsEnumerable()
                             where rate.ODT_START_DATE != null ? DateTime.Compare(DateTime.Parse(rate.ODT_START_DATE.ToString()), date) <= 0 : false
                             where rate.ODT_END_DATE != null ? DateTime.Compare(DateTime.Parse(rate.ODT_END_DATE.ToString()), date) >= 0 : false
                             select rate).FirstOrDefault();
                        dr["ODT_INS_DATE"] = queryIns == null ? "" : queryIns.OVC_INSCOMPNAY;
                    }
                    else
                        dr["ODT_INS_DATE"] = "";
                }

                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_IINN, dt);
                ViewState["dt"] = dt;
                btnPrint.Visible = dt.Rows.Count > 0;
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\mingliu.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChFont = new Font(bfChinese, 12f, Font.BOLD);
            MemoryStream Memory = new MemoryStream();
            var doc1 = new Document(PageSize.A4, 50, 50, 80, 50);
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            doc1.Open();

            PdfPTable Firsttable = new PdfPTable(6);
            Firsttable.SetWidths(new float[] { 3, 3, 3, 3, 2, 3 });
            Firsttable.TotalWidth = 550F;
            Firsttable.LockedWidth = true;
            Firsttable.DefaultCell.FixedHeight = 40f;
            Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            Firsttable.AddCell(new Paragraph("投保通知書編號", ChFont));
            Firsttable.AddCell(new Paragraph("軍種", ChFont));
            Firsttable.AddCell(new Paragraph("案號", ChFont));
            Firsttable.AddCell(new Paragraph("提單號碼", ChFont));
            Firsttable.AddCell(new Paragraph("保費金額(新台幣)", ChFont));
            Firsttable.AddCell(new Paragraph("交貨條件", ChFont));

            int i = 0;
            decimal decTitle = 0;
            string strFormat_Money = "#,0";
            if (ViewState["dt"] != null)
            {
                DataTable dt = (DataTable)ViewState["dt"];
                foreach(DataRow dr in dt.Rows)
                {
                    string strNo = (i + 1).ToString(); //序號
                    decimal.TryParse(dr["ONB_INS_AMOUNT"].ToString(), out decimal decONB_INS_AMOUNT);

                    Firsttable.AddCell(new Paragraph(dr["OVC_IINN_NO"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_MILITARY_TYPE"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_PURCH_NO"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_BLD_NO"].ToString(), ChFont));
                    Firsttable.AddCell(new Paragraph(decONB_INS_AMOUNT.ToString(strFormat_Money), ChFont));
                    Firsttable.AddCell(new Paragraph(dr["OVC_DELIVERY_CONDITION"].ToString(), ChFont));

                    i++;
                    decTitle += decONB_INS_AMOUNT;
                }
            }

            Firsttable.AddCell(new Phrase("共" + i.ToString() + "筆記錄", ChFont));
            Firsttable.AddCell(new Phrase("", ChFont));
            Firsttable.AddCell(new Phrase("", ChFont));
            Firsttable.AddCell(new Phrase("", ChFont));
            Firsttable.AddCell(new Phrase(decTitle.ToString(strFormat_Money), ChFont));
            Firsttable.AddCell(new Phrase("", ChFont));
            //for (var i = 0; i < GVTBGMT_IINN.Rows.Count; i++)
            //{
            //    Firsttable.AddCell(new Paragraph(GVTBGMT_IINN.Rows[i].Cells[0].Text, ChFont));
            //    Firsttable.AddCell(new Paragraph(GVTBGMT_IINN.Rows[i].Cells[1].Text, ChFont));
            //    Firsttable.AddCell(new Paragraph(GVTBGMT_IINN.Rows[i].Cells[2].Text, ChFont));
            //    Firsttable.AddCell(new Paragraph(GVTBGMT_IINN.Rows[i].Cells[3].Text, ChFont));
            //    Firsttable.AddCell(new Paragraph(GVTBGMT_IINN.Rows[i].Cells[4].Text, ChFont));
            //    Firsttable.AddCell(new Paragraph(GVTBGMT_IINN.Rows[i].Cells[5].Text, ChFont));

            //}
            doc1.Add(Firsttable);
            doc1.Close();

            string strFileName = "進口軍品購案投保明細表.pdf";
            FCommon.DownloadFile(this, strFileName, Memory);
        }
        #endregion

        #region GridView
        protected void GVTBGMT_IINN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                GridView theGridView = (GridView)sender;
                int index = gvr.RowIndex;
                HyperLink hlkOVC_BLD_NO = (HyperLink)gvr.FindControl("hlkOVC_BLD_NO");
                if (FCommon.Controls_isExist(hlkOVC_BLD_NO))
                {
                    string strOVC_BLD_NO = hlkOVC_BLD_NO.Text;
                    hlkOVC_BLD_NO.NavigateUrl = $"javascript: OpenWindow_BLDDATA('{ FCommon.getEncryption(strOVC_BLD_NO) }');";
                }

                //if (DateTime.TryParse(gvr.Cells[5].Text, out DateTime dt))
                //{
                //    var query =
                //        (from rate in mtse.TBGMT_INSRATE.AsEnumerable()
                //         where rate.ODT_START_DATE != null ? DateTime.Compare(DateTime.Parse(rate.ODT_START_DATE.ToString()), dt) <= 0 : false
                //         where rate.ODT_END_DATE != null ? DateTime.Compare(DateTime.Parse(rate.ODT_END_DATE.ToString()), dt) >= 0 : false
                //         select rate).FirstOrDefault();
                //    gvr.Cells[5].Text = query == null ? "" : query.OVC_INSCOMPNAY;
                //}
            }
        }

        protected void GVTBGMT_IINN_RowCreated(object sender, GridViewRowEventArgs e)
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
                TableCell cell7 = new TableCell();
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
                Tgvr.Controls.Add(cell7);
                GVTBGMT_IINN.Controls[0].Controls.Add(Tgvr);
            }
        }

        protected void GVTBGMT_IINN_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        #endregion
    }
}