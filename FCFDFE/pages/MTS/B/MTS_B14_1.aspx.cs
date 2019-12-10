using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;
using System.Data.Entity;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace FCFDFE.pages.MTS.B
{
    public partial class MTS_B14_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();

        #region 副程式
        private void dataImport()
        {
            string strMessage = "";
            string strODT_APPLY_DATE = drpODT_APPLY_DATE.SelectedValue;
            //int selectyear = Convert.ToInt32(strODT_APPLY_DATE) + 1911;
            //string strODT_APPLY_DATE2 = selectyear.ToString();
            string strISSU_NO = txtISSU_NO.Text;
            string strOVC_INF_NO = txtOVC_INF_NO.Text;
            string strOVC_INV_NO = txtOVC_INV_NO.Text;
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedValue;
            string strOVC_MILITARY_TYPE_Text = drpOVC_MILITARY_TYPE.SelectedItem != null ? drpOVC_MILITARY_TYPE.SelectedItem.Text : "";
            string strODT_CREATE_DATE = rdoODT_CREATE_DATE.SelectedValue;
            string strODT_CREATE_DATE_S = txtODT_CREATE_DATE_S.Text;
            string strODT_CREATE_DATE_E = txtODT_CREATE_DATE_E.Text;
            string strOVC_PURCH_NO = txtOVC_PURCH_NO.Text;

            ViewState["ODT_APPLY_DATE"] = strODT_APPLY_DATE;
            ViewState["ISSU_NO"] = strISSU_NO;
            ViewState["OVC_INF_NO"] = strOVC_INF_NO;
            ViewState["OVC_INV_NO"] = strOVC_INV_NO;
            ViewState["OVC_MILITARY_TYPE"] = strOVC_MILITARY_TYPE;
            ViewState["ODT_CREATE_DATE"] = strODT_CREATE_DATE;
            ViewState["ODT_CREATE_DATE_S"] = strODT_CREATE_DATE_S;
            ViewState["ODT_CREATE_DATE_E"] = strODT_CREATE_DATE_E;
            ViewState["OVC_PURCH_NO"] = strOVC_PURCH_NO;

            bool boolODT_CREATE_DATE = strODT_CREATE_DATE.Equals("2");
            bool boolODT_CREATE_DATE_S = DateTime.TryParse(strODT_CREATE_DATE_S, out DateTime dateODT_CREATE_DATE_S);
            bool boolODT_CREATE_DATE_E = DateTime.TryParse(strODT_CREATE_DATE_E, out DateTime dateODT_CREATE_DATE_E);
            if (boolODT_CREATE_DATE && !(boolODT_CREATE_DATE_S && boolODT_CREATE_DATE_E))
                strMessage += "<P> 建檔日期 不完全！ </p>";

            if (strMessage.Equals(string.Empty))
            {
                var query =
                from iinf in MTSE.TBGMT_IINF.AsEnumerable()
                select new
                {
                    iinf.IINF_SN,
                    iinf.OVC_INF_NO,
                    OVC_GIST = iinf.OVC_GIST ?? "",
                    iinf.OVC_BUDGET,
                    iinf.OVC_PURPOSE_TYPE,
                    iinf.ONB_AMOUNT,
                    ODT_APPLY_DATE = FCommon.getDateTime(iinf.ODT_APPLY_DATE),

                    ISSU_NO = iinf.ISSU_NO ?? "",
                    OVC_INV_NO = iinf.OVC_INV_NO ?? "",
                    ODT_CREATE_DATE = iinf.ODT_CREATE_DATE.ToString()
                };
                if (!strODT_APPLY_DATE.Equals(string.Empty))
                {
                    strODT_APPLY_DATE = "IINF" + strODT_APPLY_DATE.PadLeft(3, '0');
                    query = query.Where(table => table.OVC_INF_NO.StartsWith(strODT_APPLY_DATE));
                }
                if (!strISSU_NO.Equals(string.Empty))
                    query = query.Where(table => table.ISSU_NO.Contains(strISSU_NO));
                if (!strOVC_INF_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_INF_NO.Contains(strOVC_INF_NO));
                if (!strOVC_INV_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_INV_NO.Contains(strOVC_INV_NO));
                if (!strOVC_MILITARY_TYPE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_GIST.StartsWith(strOVC_MILITARY_TYPE_Text));
                if (!strOVC_PURCH_NO.Equals(string.Empty))
                    query = query.Where(t => t.OVC_GIST.Contains(strOVC_PURCH_NO));
                if (boolODT_CREATE_DATE)
                    query = query.Where(table => DateTime.TryParse(table.ODT_CREATE_DATE, out DateTime dateODT_CREATE_DATE) &&
                        DateTime.Compare(dateODT_CREATE_DATE, dateODT_CREATE_DATE_S) >= 0 &&
                        DateTime.Compare(dateODT_CREATE_DATE, dateODT_CREATE_DATE_E) <= 0);
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);

                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_IINF, dt);
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        public void PDF(string key, string[] data_list)
        {
            DataTable dt = new DataTable();
            //string strMessage = "";
            string strODT_APPLY_DATE = data_list[0].ToString();
            int selectyear = Convert.ToInt32(strODT_APPLY_DATE) + 1911;
            string select = selectyear.ToString();
            string strISSU_NO = data_list[1];
            string strOVC_INF_NO = key;
            string strOVC_INV_NO = data_list[3];
            string strOVC_MILITARY_TYPE = data_list[4].ToString();
            string strODT_CREATE_DATE_S = data_list[5];
            string strODT_CREATE_DATE_E = data_list[6];
            string rdtype = data_list[7].ToString();

            if (strODT_APPLY_DATE.Length != 3)
            {
                if (strODT_APPLY_DATE.Length == 2)
                    strODT_APPLY_DATE = "0" + strODT_APPLY_DATE;
                else if (strODT_APPLY_DATE.Length == 1)
                    strODT_APPLY_DATE = "00" + strODT_APPLY_DATE;
                else
                    strODT_APPLY_DATE = "000";
            }

            var query =
                from einf in MTSE.TBGMT_IINF
                join eiinn in MTSE.TBGMT_IINN on einf.OVC_INF_NO equals eiinn.OVC_INF_NO into ps
                from eiinn in ps.DefaultIfEmpty()
                join dept_class in MTSE.TBGMT_DEPT_CLASS on eiinn.OVC_MILITARY_TYPE equals dept_class.OVC_CLASS into ps2
                from dept_class in ps2.DefaultIfEmpty()
                join bld in MTSE.TBGMT_BLD on eiinn.OVC_BLD_NO equals bld.OVC_BLD_NO into ps6
                from bld in ps6.DefaultIfEmpty()
                join icr in MTSE.TBGMT_ICR on eiinn.OVC_BLD_NO equals icr.OVC_BLD_NO into ps7
                from icr in ps7.DefaultIfEmpty()
                join ports in MTSE.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals ports.OVC_PORT_CDE into ps4
                from ports in ps4.DefaultIfEmpty()
                join ports2 in MTSE.TBGMT_PORTS on bld.OVC_START_PORT equals ports2.OVC_PORT_CDE into ps5
                from ports2 in ps5.DefaultIfEmpty()
                select new
                {
                    eiinn.OVC_IINN_NO,
                    eiinn.OVC_PURCH_NO,
                    icr.OVC_CHI_NAME,
                    eiinn.ONB_ITEM_VALUE,
                    eiinn.ONB_INS_AMOUNT,
                    bld.OVC_SEA_OR_AIR,
                    text = ports2.OVC_PORT_CHI_NAME,
                    ports.OVC_PORT_CHI_NAME,
                    OVC_INF_NO = einf.OVC_INF_NO,
                    ISSU_NO = einf.ISSU_NO,
                    OVC_INV_NO = einf.OVC_INV_NO,
                    OVC_GIST = einf.OVC_GIST,
                    ODT_CREATE_DATE = einf.ODT_CREATE_DATE,
                    bld.ONB_QUANITY,
                };
            if (!strODT_APPLY_DATE.Equals(string.Empty))
            {
                query = query.Where(table => table.OVC_INF_NO.StartsWith("IINF" + strODT_APPLY_DATE));

            }
            if (!strISSU_NO.Equals(string.Empty))
                query = query.Where(table => table.ISSU_NO.Contains(strISSU_NO));
            if (!strOVC_INF_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_INF_NO.Contains(strOVC_INF_NO));
            if (!strOVC_INV_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_INV_NO.Contains(strOVC_INV_NO));
            if (!strOVC_MILITARY_TYPE.Equals("請選擇"))
                query = query.Where(table => table.OVC_GIST.Contains(strOVC_MILITARY_TYPE));
            if (!strOVC_INV_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_INV_NO.Contains(strOVC_INV_NO));
            if (rdtype == "2")
            {
                string date1 = txtODT_CREATE_DATE_S.Text;
                string date2 = txtODT_CREATE_DATE_E.Text;
                DateTime d1 = Convert.ToDateTime(date1);
                DateTime d2 = Convert.ToDateTime(date2);
                query = query.Where(table => table.ODT_CREATE_DATE >= d1)
                             .Where(table => table.ODT_CREATE_DATE <= d2);
            }

            dt = CommonStatic.LinqQueryToDataTable(query);


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
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                Firsttable.AddCell(new Paragraph(dt.Rows[i][0].ToString(), ChFont));
                Firsttable.AddCell(new Paragraph(dt.Rows[i][1].ToString(), ChFont));
                Firsttable.AddCell(new Paragraph(dt.Rows[i][2].ToString() + " / " + dt.Rows[i][13].ToString(), ChFont));
                Firsttable.AddCell(new Paragraph(String.Format("{0:#,###.00}", dt.Rows[i][3]), ChFont));
                Firsttable.AddCell(new Paragraph(String.Format("{0:#,###.00}", dt.Rows[i][4]), ChFont));
                Firsttable.AddCell(new Paragraph(dt.Rows[i][5].ToString(), ChFont));
                Firsttable.AddCell(new Paragraph(dt.Rows[i][6].ToString(), ChFont));
                Firsttable.AddCell(new Paragraph(dt.Rows[i][7].ToString(), ChFont));
            }
            doc1.Add(Firsttable);
            doc1.Close();

            string strFileName = $"進口保險結報申請表報表 - { key }.pdf";
            FCommon.DownloadFile(this, strFileName, Memory);
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "ODT_APPLY_DATE", ViewState["ODT_APPLY_DATE"], true);
            FCommon.setQueryString(ref strQueryString, "ISSU_NO", ViewState["ISSU_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_INF_NO", ViewState["OVC_INF_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_INV_NO", ViewState["OVC_INV_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_MILITARY_TYPE", ViewState["OVC_MILITARY_TYPE"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_CREATE_DATE", ViewState["ODT_CREATE_DATE"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_CREATE_DATE_S", ViewState["ODT_CREATE_DATE_S"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_CREATE_DATE_E", ViewState["ODT_CREATE_DATE_E"], true);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtODT_CREATE_DATE_E, txtODT_CREATE_DATE_S);
                    DateTime dateNow = DateTime.Now;
                    #region 匯入下拉式選單
                    int theYear = FCommon.getTaiwanYear(dateNow);
                    int yearMax = theYear + 0, yearMin = theYear - 15;
                    FCommon.list_dataImportNumber(drpODT_APPLY_DATE, 2, yearMax, yearMin);
                    FCommon.list_setValue(drpODT_APPLY_DATE, theYear.ToString());

                    string strFirstText = "不限定";
                    CommonMTS.list_dataImport_DEPT_CLASS(drpOVC_MILITARY_TYPE, true, strFirstText); //軍種
                    #endregion
                    txtODT_CREATE_DATE_S.Text = FCommon.getDateTime(dateNow.AddMonths(-2));
                    txtODT_CREATE_DATE_E.Text = FCommon.getDateTime(dateNow);

                    bool boolImport = false;
                    if (FCommon.getQueryString(this, "ODT_APPLY_DATE", out string strODT_APPLY_DATE, true))
                    {
                        FCommon.list_setValue(drpODT_APPLY_DATE, strODT_APPLY_DATE);
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "ISSU_NO", out string strISSU_NO, false))
                        txtISSU_NO.Text = strISSU_NO;
                    if (FCommon.getQueryString(this, "OVC_INF_NO", out string strOVC_INF_NO, true))
                        txtOVC_INF_NO.Text = strOVC_INF_NO;
                    if (FCommon.getQueryString(this, "OVC_INV_NO", out string strOVC_INV_NO, true))
                        txtOVC_INV_NO.Text = strOVC_INV_NO;
                    if (FCommon.getQueryString(this, "OVC_MILITARY_TYPE", out string strOVC_MILITARY_TYPE, true))
                        FCommon.list_setValue(drpOVC_MILITARY_TYPE, strOVC_MILITARY_TYPE);
                    if (FCommon.getQueryString(this, "ODT_CREATE_DATE", out string ODT_CREATE_DATE, true))
                        FCommon.list_setValue(rdoODT_CREATE_DATE, ODT_CREATE_DATE);
                    if (FCommon.getQueryString(this, "ODT_CREATE_DATE_S", out string strODT_CREATE_DATE_S, true))
                        txtODT_CREATE_DATE_S.Text = strODT_CREATE_DATE_S;
                    if (FCommon.getQueryString(this, "ODT_CREATE_DATE_E", out string strODT_CREATE_DATE_E, true))
                        txtODT_CREATE_DATE_E.Text = strODT_CREATE_DATE_E;
                    if (boolImport) dataImport();
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
        protected void GVTBGMT_IINF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GVTBGMT_IINF.DataKeys[gvrIndex].Value.ToString();
            string[] data_list = e.CommandArgument.ToString().Split(',');

            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);
            switch (e.CommandName)
            {
                case "dataModify":
                    Response.Redirect($"MTS_B14_2{ strQueryString }");
                    break;
                case "dataDel":
                    TBGMT_IINF iinf = MTSE.TBGMT_IINF.Where(table => table.IINF_SN.Equals(Guid.Parse(id))).FirstOrDefault();
                    if (iinf != null)
                    {
                        string strUserId = Session["userid"].ToString();
                        string strOVC_INF_NO = iinf.OVC_INF_NO;
                        MTSE.Entry(iinf).State = EntityState.Deleted;
                        MTSE.SaveChanges();
                        FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), iinf.GetType().Name.ToString(), this, "刪除");

                        var queryIinn =
                            from iinn in MTSE.TBGMT_IINN
                            where iinn.OVC_INF_NO.Equals(strOVC_INF_NO)
                            select iinn;
                        foreach (var iinn in queryIinn)
                        {
                            iinn.OVC_INF_NO = string.Empty;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), iinn.GetType().Name.ToString(), this, "修改");
                        }

                        FCommon.AlertShow(PnMessage, "success", "系統訊息", $"編號：{ strOVC_INF_NO } 之結報申請表 刪除成功。");
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "結報申請表 不存在！");
                    dataImport();
                    break;
                case "dataPrint":
                    PDF(id, data_list);
                    break;
            }
        }

        protected void GVTBGMT_IINF_RowCreated(object sender, GridViewRowEventArgs e)
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
                TableCell cell8 = new TableCell();
                TableCell cell9 = new TableCell();
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
                Tgvr.Controls.Add(cell8);
                Tgvr.Controls.Add(cell9);
                GVTBGMT_IINF.Controls[0].Controls.Add(Tgvr);
            }
        }

        protected void GVTBGMT_IINF_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        #endregion
    }
}