using System;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System.Data;
using System.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Web.UI;

namespace FCFDFE.pages.MTS.C
{
    public partial class MTS_C16 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        public string strDEPT_SN, strDEPT_Name;

        #region 副程式
        private void dataImport()
        {
            txtOVC_DEPT_CDE.Value = strDEPT_SN;
            txtOVC_ONNAME.Text = strDEPT_Name;
            btnPrint.Visible = false;
        }
        private void dataImport_GV_TBGMT_CLAIM_RESERVE()
        {
            string strMessage = "";
            string strOVC_RECLAIM_NO = txtOVC_RECLAIM_NO.Text;
            string strOVC_DEPT_CDE = txtOVC_DEPT_CDE.Value;
            //string strOVC_DEPT_NAME = txtOVC_ONNAME.Text;
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_APPLY_DATE_S = txtOVC_APPLY_DATE_S.Text;
            string strOVC_APPLY_DATE_E = txtOVC_APPLY_DATE_E.Text;
            string[] strOVC_APPROVE_STATUS = chkOVC_APPROVE_STATUS.Items.Cast<System.Web.UI.WebControls.ListItem>().Where(item => item.Selected).Select(x => x.Value).ToArray();
            //int totalcount = chkOVC_APPROVE_STATUS.Items.Cast<System.Web.UI.WebControls.ListItem>().Where(item => item.Selected).Count();

            bool boolOVC_APPLY_DATE = !strOVC_APPLY_DATE_S.Equals(string.Empty) || !strOVC_APPLY_DATE_E.Equals(string.Empty);
            bool boolOVC_APPLY_DATE_S = DateTime.TryParse(strOVC_APPLY_DATE_S, out DateTime dateOVC_APPLY_DATE_S);
            bool boolOVC_APPLY_DATE_E = DateTime.TryParse(strOVC_APPLY_DATE_E, out DateTime dateOVC_APPLY_DATE_E);
            if (boolOVC_APPLY_DATE && !(boolOVC_APPLY_DATE_S && boolOVC_APPLY_DATE_E))
                strMessage += "<P> 申請日期 不完全！ </p>";
            if (strOVC_RECLAIM_NO.Equals(string.Empty) && strOVC_DEPT_CDE.Equals(string.Empty) &&
                strOVC_BLD_NO.Equals(string.Empty) && !boolOVC_APPLY_DATE && strOVC_APPROVE_STATUS.Length == 0)
                strMessage += "<p> 請至少填寫一項條件！ </p>";

            if (strMessage.Equals(string.Empty))
            {
                var query =
                    from reserve in MTSE.TBGMT_CLAIM_RESERVE.AsEnumerable()
                        //join dept in MTSE.TBGMT_DEPT_CDE on reserve.OVC_MILITARY_TYPE equals dept.OVC_DEPT_CODE
                    join claim in MTSE.TBGMT_CLAIM on reserve.OVC_INN_NO equals claim.OVC_INN_NO into temp
                    from claim in temp.DefaultIfEmpty()
                    select new
                    {
                        OVC_RECLAIM_NO = reserve.OVC_RECLAIM_NO,
                        OVC_APPLY_DATE = FCommon.getDateTime(reserve.OVC_APPLY_DATE),
                        OVC_CLAIM_ITEM = reserve.OVC_CLAIM_ITEM,
                        OVC_PURCH_NO = claim == null ? "" : claim.OVC_PURCH_NO,
                        OVC_INN_NO = reserve.OVC_INN_NO,
                        OVC_BLD_NO = reserve.OVC_BLD_NO ?? "",
                        ONB_RECEIVE = reserve.ONB_RECEIVE,
                        ONB_ACTUAL_RECEIVE = reserve.ONB_ACTUAL_RECEIVE,
                        OVC_CLAIM_REASON = reserve.OVC_CLAIM_REASON,
                        ONB_CLAIM_NUMBER = reserve.ONB_CLAIM_NUMBER,
                        OVC_IMPORT_DATE = FCommon.getDateTime(reserve.OVC_IMPORT_DATE),
                        OVC_APPROVE_DATE = FCommon.getDateTime(reserve.OVC_APPROVE_DATE),
                        ONB_CLAIM_AMOUNT = reserve.ONB_CLAIM_AMOUNT,
                        OVC_APPROVE_STATUS = reserve.OVC_APPROVE_STATUS ?? "",
                        OVC_NOTE = reserve.OVC_NOTE,

                        OVC_MILITARY_TYPE = reserve.OVC_MILITARY_TYPE ?? "",
                        //OVC_DEPT_NAME = dept.OVC_DEPT_NAME,
                        ONB_CLAIM_AMOUNT_NTD = reserve.ONB_CLAIM_AMOUNT_NTD,
                        OVC_CLAIM_NO = reserve.OVC_APPROVE_STATUS.Contains("申請理賠") ? (claim == null ? "" : claim.OVC_CLAIM_NO) : "",
                        OVC_CLIAM_CURRENCY = reserve.OVC_CLAIM_CURRENCY
                    };

                if (!strOVC_RECLAIM_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_RECLAIM_NO.Contains(strOVC_RECLAIM_NO));
                if (!strOVC_DEPT_CDE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_MILITARY_TYPE.Equals(strOVC_DEPT_CDE));
                if (!strOVC_BLD_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
                if (boolOVC_APPLY_DATE)
                    query = query.Where(table => DateTime.TryParse(table.OVC_APPLY_DATE, out DateTime dateOVC_APPLY_DATE) &&
                        DateTime.Compare(dateOVC_APPLY_DATE, dateOVC_APPLY_DATE_S) >= 0 &&
                        DateTime.Compare(dateOVC_APPLY_DATE, dateOVC_APPLY_DATE_E) <= 0);
                if (strOVC_APPROVE_STATUS.Length > 0)
                    query = query.Where(table => strOVC_APPROVE_STATUS.Contains(table.OVC_APPROVE_STATUS));

                #region
                //if (totalcount != 0)
                //{
                //    if (totalcount == 1)
                //    {
                //        string ch1 = selected[0].ToString();
                //        query = query.Where(table => table.OVC_APPROVE_STATUS == ch1);
                //    }

                //    if (totalcount == 2)
                //    {
                //        string ch1 = selected[0].ToString();
                //        string ch2 = selected[1].ToString();
                //        query = query.Where(table => table.OVC_APPROVE_STATUS == ch1 || table.OVC_APPROVE_STATUS == ch2);
                //    }
                //    if (totalcount == 3)
                //    {
                //        string ch1 = selected[0].ToString();
                //        string ch2 = selected[1].ToString();
                //        string ch3 = selected[2].ToString();
                //        query = query.Where(table => table.OVC_APPROVE_STATUS == ch1 || table.OVC_APPROVE_STATUS == ch2 || table.OVC_APPROVE_STATUS == ch3);
                //    }
                //    if (totalcount == 4)
                //    {
                //        string ch1 = selected[0].ToString();
                //        string ch2 = selected[1].ToString();
                //        string ch3 = selected[2].ToString();
                //        string ch4 = selected[3].ToString();
                //        query = query.Where(table => table.OVC_APPROVE_STATUS == ch1 || table.OVC_APPROVE_STATUS == ch2 || table.OVC_APPROVE_STATUS == ch3 || table.OVC_APPROVE_STATUS == ch4);
                //    }
                //    if (totalcount == 5)
                //    {
                //        string ch1 = selected[0].ToString();
                //        string ch2 = selected[1].ToString();
                //        string ch3 = selected[2].ToString();
                //        string ch4 = selected[3].ToString();
                //        string ch5 = selected[4].ToString();
                //        query = query.Where(table => table.OVC_APPROVE_STATUS == ch1 || table.OVC_APPROVE_STATUS == ch2 || table.OVC_APPROVE_STATUS == ch3 || table.OVC_APPROVE_STATUS == ch4 || table.OVC_APPROVE_STATUS == ch5);
                //    }
                //    if (totalcount == 6)
                //    {
                //        string ch1 = selected[0].ToString();
                //        string ch2 = selected[1].ToString();
                //        string ch3 = selected[2].ToString();
                //        string ch4 = selected[3].ToString();
                //        string ch5 = selected[4].ToString();
                //        string ch6 = selected[5].ToString();
                //        query = query.Where(table => table.OVC_APPROVE_STATUS == ch1 || table.OVC_APPROVE_STATUS == ch2 || table.OVC_APPROVE_STATUS == ch3 || table.OVC_APPROVE_STATUS == ch4 || table.OVC_APPROVE_STATUS == ch5 || table.OVC_APPROVE_STATUS == ch6);
                //    }
                //    if (totalcount == 7)
                //    {
                //        string ch1 = selected[0].ToString();
                //        string ch2 = selected[1].ToString();
                //        string ch3 = selected[2].ToString();
                //        string ch4 = selected[3].ToString();
                //        string ch5 = selected[4].ToString();
                //        string ch6 = selected[5].ToString();
                //        string ch7 = selected[6].ToString();
                //        query = query.Where(table => table.OVC_APPROVE_STATUS == ch1 || table.OVC_APPROVE_STATUS == ch2 || table.OVC_APPROVE_STATUS == ch3 || table.OVC_APPROVE_STATUS == ch4 || table.OVC_APPROVE_STATUS == ch5 || table.OVC_APPROVE_STATUS == ch6 || table.OVC_APPROVE_STATUS == ch7);
                //    }
                //}
                #endregion

                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                bool hasRows = FCommon.GridView_dataImport(GV_TBGMT_CLAIM_RESERVE, dt);
                ViewState["hasRows"] = hasRows;
                ViewState["dt"] = dt;
                btnPrint.Visible = hasRows;
            }
            else
                FCommon.AlertShow(pnMessageQuery, "danger", "系統訊息", strMessage);
        }
        private void dataprint()
        {
            if (ViewState["dt"] != null && ViewState["dt"] is DataTable)
            {
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

                PdfPTable table1 = new PdfPTable(13);
                table1.TotalWidth = 1200F;
                table1.SetWidths(new float[] { 2, 4, 4, 4, 6, 2, 2, 2, 3, 5, 3, 4, 3 });
                table1.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table1.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell title = new PdfPCell(new Phrase("保留索賠及撤銷清單\n\n", ChTitle));
                title.VerticalAlignment = Element.ALIGN_MIDDLE;
                title.HorizontalAlignment = Element.ALIGN_CENTER;
                title.Border = Rectangle.NO_BORDER;
                title.Colspan = 13;
                table1.AddCell(title);

                table1.AddCell(new Phrase("序", ChTabTitle));
                table1.AddCell(new Phrase("保留索賠權編號", ChTabTitle));
                table1.AddCell(new Phrase("申請日期\n進口日期", ChTabTitle));
                table1.AddCell(new Phrase("軍品名稱", ChTabTitle));
                table1.AddCell(new Phrase("投保通知書編號\n提單編號", ChTabTitle));
                table1.AddCell(new Phrase("應收\n件數", ChTabTitle));
                table1.AddCell(new Phrase("實收\n件數", ChTabTitle));
                table1.AddCell(new Phrase("損失原因", ChTabTitle));
                table1.AddCell(new Phrase("索賠件數", ChTabTitle));
                table1.AddCell(new Phrase("索賠金額(外幣)", ChTabTitle));
                table1.AddCell(new Phrase("索賠金額(台幣)", ChTabTitle));
                table1.AddCell(new Phrase("作業\n進度", ChTabTitle));
                table1.AddCell(new Phrase("備註", ChTabTitle));

                int i = 0;
                string strFormat_Money = "#,0";
                decimal decSum_ONB_CLAIM_NUMBER = 0; //總 索賠件數
                decimal decSum_ONB_CLAIM_AMOUNT_NTD = 0; //總 索賠金額(台幣)
                foreach (DataRow dr in dt.Rows)
                //foreach (var t in query)
                {
                    string strNo = (i + 1).ToString(); //序號

                    decimal.TryParse(dr["ONB_RECEIVE"].ToString(), out decimal decONB_RECEIVE);
                    decimal.TryParse(dr["ONB_ACTUAL_RECEIVE"].ToString(), out decimal decONB_ACTUAL_RECEIVE);
                    decimal.TryParse(dr["ONB_CLAIM_NUMBER"].ToString(), out decimal decONB_CLAIM_NUMBER);
                    decimal.TryParse(dr["ONB_CLAIM_AMOUNT"].ToString(), out decimal decONB_CLAIM_AMOUNT);
                    decimal.TryParse(dr["ONB_CLAIM_AMOUNT_NTD"].ToString(), out decimal decONB_CLAIM_AMOUNT_NTD);

                    table1.AddCell(new Phrase(strNo, ChFont));
                    table1.AddCell(new Phrase(dr["OVC_RECLAIM_NO"].ToString(), ChFont));
                    table1.AddCell(new Phrase($"{ FCommon.getDateTime(dr["OVC_APPLY_DATE"]) }\n{ FCommon.getDateTime(dr["OVC_IMPORT_DATE"]) }", ChFont));
                    table1.AddCell(new Phrase(dr["OVC_CLAIM_ITEM"].ToString(), ChFont));
                    table1.AddCell(new Phrase($"{ dr["OVC_INN_NO"].ToString() }\n{ dr["OVC_BLD_NO"].ToString() }", ChFont));
                    table1.AddCell(new Phrase(decONB_RECEIVE.ToString(strFormat_Money), ChFont));
                    table1.AddCell(new Phrase(decONB_ACTUAL_RECEIVE.ToString(strFormat_Money), ChFont));
                    table1.AddCell(new Phrase(dr["OVC_CLAIM_REASON"].ToString(), ChFont));
                    table1.AddCell(new Phrase(decONB_CLAIM_NUMBER.ToString(strFormat_Money), ChFont));
                    table1.AddCell(new Phrase($"{ decONB_CLAIM_AMOUNT.ToString("N") } ({ dr["OVC_CLIAM_CURRENCY"].ToString() })", ChFont));
                    table1.AddCell(new Phrase(decONB_CLAIM_AMOUNT_NTD.ToString(strFormat_Money), ChFont));
                    table1.AddCell(new Phrase(dr["OVC_APPROVE_STATUS"].ToString(), ChFont));
                    table1.AddCell(new Phrase(dr["OVC_NOTE"].ToString(), ChFont));
                    
                    i++;
                    decSum_ONB_CLAIM_NUMBER += decONB_CLAIM_NUMBER;
                    decSum_ONB_CLAIM_AMOUNT_NTD += decONB_CLAIM_AMOUNT_NTD;
                }

                table1.AddCell(new Phrase("合計", ChTabTitle));
                table1.AddCell(new Phrase("共" + i .ToString() + "筆", ChTabTitle));
                table1.AddCell(new Phrase("", ChTabTitle));
                table1.AddCell(new Phrase("", ChTabTitle));
                table1.AddCell(new Phrase("", ChTabTitle));
                table1.AddCell(new Phrase("", ChTabTitle));
                table1.AddCell(new Phrase("", ChTabTitle));
                table1.AddCell(new Phrase("", ChTabTitle));
                table1.AddCell(new Phrase(decSum_ONB_CLAIM_NUMBER.ToString(strFormat_Money), ChTabTitle));
                table1.AddCell(new Phrase("", ChTabTitle));
                table1.AddCell(new Phrase(decSum_ONB_CLAIM_AMOUNT_NTD.ToString(strFormat_Money), ChTabTitle));
                table1.AddCell(new Phrase("", ChTabTitle));
                table1.AddCell(new Phrase("", ChTabTitle));

                doc1.Add(table1);
                doc1.Close();

                string strFileName = "保留索賠及撤銷清單.pdf";
                FCommon.DownloadFile(this, strFileName, Memory);
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getAccountDEPTName(this, out strDEPT_SN, out strDEPT_Name);
                if (!Page.IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_ONNAME, txtOVC_APPLY_DATE_S, txtOVC_APPLY_DATE_E);
                    #region 匯入下拉式選單
                    CommonMTS.list_dataImport_APPROVE_STATUS(chkOVC_APPROVE_STATUS, false); //作業進度
                    #endregion
                    dataImport();
                }
            }
        }

        #region ~Click
        protected void btnClearDept_Click(object sender, EventArgs e)
        {
            FCommon.Controls_Clear(txtOVC_DEPT_CDE, txtOVC_ONNAME);
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport_GV_TBGMT_CLAIM_RESERVE();
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            dataprint();
        }
        //protected void btnDataCancel_Click(object sender, EventArgs e)
        //{
        //    txtOvcMilitrayType.Text = string.Empty;
        //}
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtOVC_APPLY_DATE_S.Text = string.Empty;
            txtOVC_APPLY_DATE_E.Text = string.Empty;
            txtOVC_BLD_NO.Text = string.Empty;
            txtOVC_RECLAIM_NO.Text = string.Empty;
            chkOVC_APPROVE_STATUS.Text = string.Empty;
        }
        #endregion

        #region GridView
        protected void GV_TBGMT_CLAIM_RESERVE_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        #endregion
    }
}