using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using System.Data.Entity;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Web.UI;

namespace FCFDFE.pages.MTS.B
{
    public partial class MTS_B24_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();

        #region 副程式
        private void dataImport()
        {
            string strMessage = "";
            string strODT_APPLY_DATE = drpODT_APPLY_DATE.SelectedValue;
            string strOVC_INF_NO = txtOVC_INF_NO.Text;
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedValue;
            string strOVC_MILITARY_TYPE_Text = drpOVC_MILITARY_TYPE.SelectedItem != null ? drpOVC_MILITARY_TYPE.SelectedItem.Text : "";
            string strODT_CREATE_DATE = rdoODT_CREATE_DATE.SelectedValue;
            string strODT_CREATE_DATE_S = txtODT_CREATE_DATE_S.Text;
            string strODT_CREATE_DATE_E = txtODT_CREATE_DATE_E.Text;

            ViewState["ODT_APPLY_DATE"] = strODT_APPLY_DATE;
            ViewState["OVC_INF_NO"] = strOVC_INF_NO;
            ViewState["OVC_MILITARY_TYPE"] = strOVC_MILITARY_TYPE;
            ViewState["ODT_CREATE_DATE"] = strODT_CREATE_DATE;
            ViewState["ODT_CREATE_DATE_S"] = strODT_CREATE_DATE_S;
            ViewState["ODT_CREATE_DATE_E"] = strODT_CREATE_DATE_E;

            bool boolODT_CREATE_DATE = strODT_CREATE_DATE.Equals("2");
            bool boolODT_CREATE_DATE_S = DateTime.TryParse(strODT_CREATE_DATE_S, out DateTime dateODT_CREATE_DATE_S);
            bool boolODT_CREATE_DATE_E = DateTime.TryParse(strODT_CREATE_DATE_E, out DateTime dateODT_CREATE_DATE_E);

            if (strODT_APPLY_DATE.Equals(string.Empty) && strOVC_INF_NO.Equals(string.Empty) && strOVC_MILITARY_TYPE.Equals(string.Empty) && !boolODT_CREATE_DATE)
                strMessage += "<P> 至少填入一個選項！ </p>";
            else
            {
                if (boolODT_CREATE_DATE && !(boolODT_CREATE_DATE_S && boolODT_CREATE_DATE_E))
                    strMessage += "<P> 建檔日期 不完全！ </p>";
            }

            if (strMessage.Equals(string.Empty))
            {
                var query =
                    from einf in MTSE.TBGMT_EINF.AsEnumerable()
                    orderby einf.OVC_INF_NO
                    select new
                    {
                        einf.EINF_SN,
                        einf.OVC_INF_NO,
                        OVC_GIST = einf.OVC_GIST??"",
                        einf.OVC_BUDGET,
                        einf.OVC_PURPOSE_TYPE,
                        einf.ONB_AMOUNT,
                        einf.OVC_BUDGET_INF_NO,
                        einf.OVC_NOTE,
                        einf.OVC_PLN_CONTENT,
                        ODT_CREATE_DATE = einf.ODT_CREATE_DATE.ToString(),
                        ODT_APPLY_DATE = FCommon.getDateTime(einf.ODT_APPLY_DATE)
                    };
                if (!strODT_APPLY_DATE.Equals(string.Empty))
                {
                    strODT_APPLY_DATE = "EINF" + strODT_APPLY_DATE.PadLeft(3, '0');
                    query = query.Where(table => table.OVC_INF_NO.StartsWith(strODT_APPLY_DATE));
                }
                if (!strOVC_INF_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_INF_NO.Contains(strOVC_INF_NO));
                if (!strOVC_MILITARY_TYPE.Equals(string.Empty) && !strOVC_MILITARY_TYPE_Text.Equals(string.Empty))
                    query = query.Where(table => table.OVC_GIST.StartsWith(strOVC_MILITARY_TYPE_Text));
                if (boolODT_CREATE_DATE)
                    query = query.Where(table => DateTime.TryParse(table.ODT_CREATE_DATE, out DateTime dateODT_CREATE_DATE) &&
                        DateTime.Compare(dateODT_CREATE_DATE, dateODT_CREATE_DATE_S) >= 0 &&
                        DateTime.Compare(dateODT_CREATE_DATE, dateODT_CREATE_DATE_E) <= 0);

                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["COUNT_OVC_INF_NO"] = dt.Rows.Count;
                ViewState["SUM_ONB_AMOUNT"] = query.Sum(table => table.ONB_AMOUNT);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_EINF, dt);
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "ODT_APPLY_DATE", ViewState["ODT_APPLY_DATE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_INF_NO", ViewState["OVC_INF_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_MILITARY_TYPE", ViewState["OVC_MILITARY_TYPE"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_CREATE_DATE", ViewState["ODT_CREATE_DATE"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_CREATE_DATE_S", ViewState["ODT_CREATE_DATE_S"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_CREATE_DATE_E", ViewState["ODT_CREATE_DATE_E"], true);
            return strQueryString;
        }

        //B24_2回上一頁後的查詢
        //protected void dataImportBack()
        //{
        //    if (Session["strODT_APPLY_DATE"] != null || Session["strOVC_INF_NO"] != null || Session["strOVC_MILITARY_TYPE"] != null || Session["strODT_CREATE_DATE_S"] != null)
        //    {
        //        var query =
        //          from einf in MTSE.TBGMT_EINF
        //          orderby einf.OVC_INF_NO
        //          select new
        //          {
        //              einf.EINF_SN,
        //              einf.OVC_INF_NO,
        //              einf.OVC_GIST,
        //              einf.OVC_BUDGET,
        //              einf.OVC_PURPOSE_TYPE,
        //              einf.ONB_AMOUNT,
        //              einf.OVC_BUDGET_INF_NO,
        //              einf.OVC_NOTE,
        //              einf.OVC_PLN_CONTENT,
        //              einf.ODT_CREATE_DATE,
        //              einf.ODT_APPLY_DATE
        //          };
        //        if (Session["strODT_APPLY_DATE"] != null)
        //        {
        //            string strODT_APPLY_DATE = Session["strODT_APPLY_DATE"].ToString();
        //            query = query.Where(table => table.OVC_INF_NO.Contains(strODT_APPLY_DATE));
        //            drpODT_APPLY_DATE.SelectedValue = strODT_APPLY_DATE.Substring(4).TrimStart('0');
        //        }
        //        if (Session["strOVC_INF_NO"] != null)
        //        {
        //            string strOVC_INF_NO = Session["strOVC_INF_NO"].ToString();
        //            query = query.Where(table => table.OVC_INF_NO.Contains(strOVC_INF_NO));
        //            txtOVC_INF_NO.Text = strOVC_INF_NO;
        //        }
        //        if (Session["strOVC_MILITARY_TYPE"] != null)
        //        {
        //            string strOVC_MILITARY_TYPE = Session["strOVC_MILITARY_TYPE"].ToString();
        //            query = query.Where(table => table.OVC_GIST.Contains(strOVC_MILITARY_TYPE));
        //            System.Web.UI.WebControls.ListItem item = drpOVC_MILITARY_TYPE.Items.FindByText(strOVC_MILITARY_TYPE);
        //            if (item != null)
        //                item.Selected = true;
        //        }
        //        if (Session["strODT_CREATE_DATE_S"] != null && Session["strODT_CREATE_DATE_E"] != null)
        //        {
        //            DateTime strODT_CREATE_DATE_S = Convert.ToDateTime(Session["strODT_CREATE_DATE_S"]);
        //            DateTime strODT_CREATE_DATE_E = Convert.ToDateTime(Session["strODT_CREATE_DATE_E"]);
        //            query = query.Where(table => table.ODT_CREATE_DATE >= strODT_CREATE_DATE_S && table.ODT_CREATE_DATE <= strODT_CREATE_DATE_E);
        //            txtODT_CREATE_DATE_S.Text = strODT_CREATE_DATE_S.ToShortDateString();
        //            txtODT_CREATE_DATE_E.Text = strODT_CREATE_DATE_E.ToShortDateString();
        //        }
        //        DataTable dt = CommonStatic.LinqQueryToDataTable(query);
        //        ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_EINF, dt);
        //    }
        //}
        public void Pdf(string strEINF_SN)
        //public void Pdf(string strEINF_SN, string[] data_list)
        {
            if (Guid.TryParse(strEINF_SN, out Guid guidEINF_SN))
            {
                #region 上半段資料庫
                var query =
                    from einf in MTSE.TBGMT_EINF
                    where einf.EINF_SN.Equals(guidEINF_SN)
                    join einn in MTSE.TBGMT_EINN on einf.OVC_INF_NO equals einn.OVC_INF_NO into tempEinn
                    from einn in tempEinn.DefaultIfEmpty()
                    join edf in MTSE.TBGMT_EDF on einn.OVC_EDF_NO equals edf.OVC_EDF_NO into tempEdf
                    from edf in tempEdf.DefaultIfEmpty()
                    join portSta in MTSE.TBGMT_PORTS on edf.OVC_START_PORT equals portSta.OVC_PORT_CDE into tempPortSta
                    from portSta in tempPortSta.DefaultIfEmpty()
                    join portArr in MTSE.TBGMT_PORTS on edf.OVC_ARRIVE_PORT equals portArr.OVC_PORT_CDE into tempPortArr
                    from portArr in tempPortArr.DefaultIfEmpty()
                    join edf_detall in MTSE.TBGMT_EDF_DETAIL on einn.OVC_EDF_NO equals edf_detall.OVC_EDF_NO into tempEdf_detall
                    from edf_detall in tempEdf_detall.DefaultIfEmpty()
                    join bld in MTSE.TBGMT_BLD on edf.OVC_BLD_NO equals bld.OVC_BLD_NO into tempBld
                    from bld in tempBld.DefaultIfEmpty()
                    join currency in MTSE.TBGMT_CURRENCY on einn.ONB_CARRIAGE_CURRENCY equals currency.OVC_CURRENCY_CODE into tempCurrency
                    from currency in tempCurrency.DefaultIfEmpty()
                    join currency2 in MTSE.TBGMT_CURRENCY on einn.ONB_CARRIAGE_CURRENCY2 equals currency2.OVC_CURRENCY_CODE into tempCurrency2
                    from currency2 in tempCurrency2.DefaultIfEmpty()
                    select new
                    {
                        OVC_EINN_NO = einn != null ? einn.OVC_EINN_NO : "",
                        OVC_PURCH_NO = edf != null ? edf.OVC_PURCH_NO : "",
                        ONB_ITEM_COUNT = edf_detall != null ? edf_detall.ONB_ITEM_COUNT : null,
                        OVC_CHI_NAME = edf_detall != null ? edf_detall.OVC_CHI_NAME : "",
                        ONB_ITEM_VALUE = einn != null ? einn.ONB_ITEM_VALUE : null,
                        OVC_CURRENCY_NAME = currency != null ? currency.OVC_CURRENCY_NAME : "",
                        ONB_INS_AMOUNT = einn != null ? einn.ONB_INS_AMOUNT : null,
                        OVC_CURRENCY_NAME2 = currency2 != null ? currency2.OVC_CURRENCY_NAME : "",
                        OVC_SEA_OR_AIR = bld != null ? bld.OVC_SEA_OR_AIR : "",
                        OVC_START_PORT = portSta != null ? portSta.OVC_PORT_CHI_NAME : "",
                        OVC_ARRIVE_PORT = portArr != null ? portArr.OVC_PORT_CHI_NAME : "",
                        OVC_GIST = einf.OVC_GIST,
                        OVC_INF_NO = einn != null ? einn.OVC_INF_NO : "",
                        einf.ODT_CREATE_DATE
                    };
                #endregion
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);

                BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\mingliu.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
                Font ChFont = new Font(bfChinese, 12f, Font.BOLD);
                MemoryStream Memory = new MemoryStream();
                var doc1 = new Document(PageSize.A4, 50, 50, 80, 50);
                PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
                DateTime PrintTime = DateTime.Now;
                doc1.Open();
                PdfPTable Firsttable = new PdfPTable(8);
                Firsttable.SetWidths(new float[] { 3, 2, 4, 2, 2, 1, 2, 2 });
                Firsttable.TotalWidth = 550F;
                Firsttable.LockedWidth = true;
                Firsttable.DefaultCell.FixedHeight = 40f;
                Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                Firsttable.AddCell(new Paragraph("投保通知書編號", ChFont));
                Firsttable.AddCell(new Paragraph("案號或採購文號", ChFont));
                Firsttable.AddCell(new Paragraph("物質名稱及數量", ChFont));
                Firsttable.AddCell(new Paragraph("物質價值", ChFont));
                Firsttable.AddCell(new Paragraph("投保金額", ChFont));
                Firsttable.AddCell(new Paragraph("運輸工具", ChFont));
                Firsttable.AddCell(new Paragraph("啟運港口", ChFont));
                Firsttable.AddCell(new Paragraph("目的港口", ChFont));

                int intCount = dt.Rows.Count;
                if (intCount > 0)
                {
                    DataRow dr_First = dt.Rows[0];
                    int item_count = 0; //小計 計算
                    int[] total_count = new int[intCount]; //小計
                    int n = 0;
                    string strOVC_INF_NO = dr_First["OVC_INF_NO"].ToString();
                    string strOVC_EINN_NO_Prev = dr_First["OVC_EINN_NO"].ToString();
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        string strOVC_EINN_NO = dr["OVC_EINN_NO"].ToString();
                        if (int.TryParse(dr["ONB_ITEM_COUNT"].ToString(), out int intONB_ITEM_COUNT))
                            if (strOVC_EINN_NO.Equals(strOVC_EINN_NO_Prev))
                                item_count += intONB_ITEM_COUNT;
                            else
                            {
                                total_count[i - 1] = item_count;
                                item_count = intONB_ITEM_COUNT; //重新計算
                                strOVC_EINN_NO_Prev = strOVC_EINN_NO;
                            }
                        if (i == intCount - 1) //最後一筆
                            total_count[i] = item_count;
                    }

                    int o = 0;
                    string temp2 = "";
                    //for (var i = 0; i < dt.Rows.Count; i++)
                    foreach (DataRow dr in dt.Rows)
                    {

                        if (dr[0].ToString() != temp2)
                        {
                            Firsttable.AddCell(new Paragraph(dr[0].ToString(), ChFont));
                            Firsttable.AddCell(new Paragraph(dr[1].ToString(), ChFont));
                            if (total_count[o] > 1)
                            {
                                Firsttable.AddCell(new Paragraph(dr[3].ToString() + "等物品 / " + total_count[o], ChFont));
                            }
                            else
                            {
                                Firsttable.AddCell(new Paragraph(dr[3].ToString() + "/" + total_count[o], ChFont));
                            }
                            Firsttable.AddCell(new Paragraph(String.Format("{0:#,###.00}", dr[4]) + dr[5].ToString(), ChFont));
                            Firsttable.AddCell(new Paragraph(String.Format("{0:#,###.00}", dr[6]) + dr[7].ToString(), ChFont));
                            Firsttable.AddCell(new Paragraph(dr[8].ToString(), ChFont));
                            Firsttable.AddCell(new Paragraph(dr[9].ToString(), ChFont));
                            Firsttable.AddCell(new Paragraph(dr[10].ToString(), ChFont));
                            temp2 = dr[0].ToString();
                            o++;
                        }
                    }
                    doc1.Add(Firsttable);
                    doc1.Close();

                    string strFileName = $"進口保險結報申請表報表-{ strOVC_INF_NO }.pdf";
                    FCommon.DownloadFile(this, strFileName, Memory);
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無資料！");
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtODT_CREATE_DATE_S, txtODT_CREATE_DATE_E);
                    DateTime dateNow = DateTime.Now;
                    #region 匯入下拉式選單
                    string strFirstText = "不限定", strFirstValue = "";
                    int theYear = FCommon.getTaiwanYear(dateNow);
                    int yearMax = theYear + 0, yearMin = theYear - 15;
                    FCommon.list_dataImportNumber(drpODT_APPLY_DATE, 2, yearMax, yearMin, strFirstText, strFirstValue, true);
                    FCommon.list_setValue(drpODT_APPLY_DATE, theYear.ToString());

                    CommonMTS.list_dataImport_DEPT_CLASS(drpOVC_MILITARY_TYPE, true, strFirstText); //軍種
                    //DataTable dtOVC_MILITARY_TYPE = CommonStatic.ListToDataTable(MTSE.TBGMT_DEPT_CLASS.OrderBy(c => c.OVC_CLASS_NAME).ToList());
                    //FCommon.list_dataImport(drpOVC_MILITARY_TYPE, dtOVC_MILITARY_TYPE, "OVC_CLASS_NAME", "OVC_CLASS", true);
                    #endregion
                    txtODT_CREATE_DATE_S.Text = FCommon.getDateTime(dateNow.AddMonths(-3));
                    txtODT_CREATE_DATE_E.Text = FCommon.getDateTime(dateNow.AddDays(-1));

                    bool boolImport = false;
                    if (FCommon.getQueryString(this, "ODT_APPLY_DATE", out string strODT_APPLY_DATE, true))
                    {
                        FCommon.list_setValue(drpODT_APPLY_DATE, strODT_APPLY_DATE);
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_INF_NO", out string strOVC_INF_NO, true))
                        txtOVC_INF_NO.Text = strOVC_INF_NO;
                    if (FCommon.getQueryString(this, "OVC_MILITARY_TYPE", out string strOVC_MILITARY_TYPE, true))
                        FCommon.list_setValue(drpOVC_MILITARY_TYPE, strOVC_MILITARY_TYPE);
                    if (FCommon.getQueryString(this, "ODT_CREATE_DATE", out string strODT_CREATE_DATE, true))
                        FCommon.list_setValue(rdoODT_CREATE_DATE, strODT_CREATE_DATE);
                    if (FCommon.getQueryString(this, "ODT_CREATE_DATE_S", out string strODT_CREATE_DATE_S, true))
                        txtODT_CREATE_DATE_S.Text = strODT_CREATE_DATE_S;
                    if (FCommon.getQueryString(this, "ODT_CREATE_DATE_E", out string strODT_CREATE_DATE_E, true))
                        txtODT_CREATE_DATE_E.Text = strODT_CREATE_DATE_E;
                    if (boolImport)
                        dataImport();
                }
            }
        }

        #region ~Click
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }
        #endregion

        #region GridView
        protected void GV_TBGMT_EINF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            Guid einf_SN = (Guid)GV_TBGMT_EINF.DataKeys[gvrIndex].Value;
            string id = GV_TBGMT_EINF.DataKeys[gvrIndex].Value.ToString();
            //string inf_no = GV_TBGMT_EINF.Rows[gvrIndex].Cells[0].Text;
            //string[] data_list = e.CommandArgument.ToString().Split(',');

            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);

            switch (e.CommandName)
            {
                case "dataModify":
                    Response.Redirect($"MTS_B24_2{ strQueryString }");
                    break;
                case "dataDel":
                    TBGMT_EINF einf = MTSE.TBGMT_EINF.Where(table => table.EINF_SN.Equals(einf_SN)).FirstOrDefault();
                    if (einf != null)
                    {
                        string strUserId = Session["userid"].ToString();
                        string strOVC_INF_NO = einf.OVC_INF_NO;
                        MTSE.Entry(einf).State = EntityState.Deleted;
                        MTSE.SaveChanges();
                        FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), einf.GetType().Name.ToString(), this, "刪除");

                        var queryEinn =
                            from einn in MTSE.TBGMT_EINN
                            join edf in MTSE.TBGMT_EDF on einn.OVC_EDF_NO equals edf.OVC_EDF_NO
                            where einn.OVC_INF_NO.Equals(strOVC_INF_NO)
                            select einn;
                        foreach (var einn in queryEinn)
                        {
                            einn.OVC_INF_NO = string.Empty;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), einn.GetType().Name.ToString(), this, "修改");
                        }
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", $"編號：{ strOVC_INF_NO } 之投保通知書 刪除成功。");
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "投保通知書 不存在！");
                    dataImport();
                    break;
                case "dataPrint":
                    //Pdf(id, data_list);
                    Pdf(id);
                    break;
                default:
                    break;
            }
        }
        protected void GV_TBGMT_EINF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    break;
                //統計footer
                case DataControlRowType.Footer:
                    if (ViewState["COUNT_OVC_INF_NO"] != null)
                        e.Row.Cells[1].Text = $"共 { ViewState["COUNT_OVC_INF_NO"].ToString() } 項";
                    if (ViewState["SUM_ONB_AMOUNT"] != null)
                        e.Row.Cells[4].Text = ViewState["SUM_ONB_AMOUNT"].ToString();
                    break;
                default:
                    break;
            }
        }
        protected void GV_TBGMT_EINF_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_TBGMT_EINF.PageIndex = e.NewPageIndex; //抓取GV分頁頁數
            dataImport();
        }
        protected void GV_TBGMT_EINF_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        #endregion
    }
}