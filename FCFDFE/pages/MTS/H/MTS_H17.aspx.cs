using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.Collections;

namespace FCFDFE.pages.MTS.H
{
    public partial class MTS_H17 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        public string strDEPT_SN, strDEPT_Name;
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        Common FCommon = new Common();
        public int intAuth = 0; //是否為物資接轉處長官(2) or 地區長官(1) or 三軍/其他(0)
        string[] delim = { "<br/>" };
        string[] aryAreaList = { "基隆地區", "桃園地區", "高雄分遣組" };
        string strDateFormatSQL = "'yyyy-mm-dd'";

        #region 副程式
        //public void dataImport(string msg)
        //{
        //    DataTable dt = new DataTable();
        //    DateTime dateQueryDate1 = Convert.ToDateTime(Session["dateQueryDate1"]);
        //    DateTime dateQueryDate2 = Convert.ToDateTime(Session["dateQueryDate2"]);
        //    DateTime dateOdtCreateDate1 = Convert.ToDateTime(Session["dateOdtCreateDate1"]);
        //    DateTime dateOdtCreateDate2 = Convert.ToDateTime(Session["dateOdtCreateDate2"]);
        //    string strovcShipCompany = Session["strovcShipCompany"].ToString();
        //    string strOvcVoyage = Session["strOvcVoyage"].ToString();
        //    string strIdrSn = Session["strIdrSn"].ToString();
        //    string strArea = Session["strArea"].ToString();

        //    var query =
        //        from ics in MTSE.TBGMT_ICS
        //        join bld in MTSE.TBGMT_BLD on ics.OVC_BLD_NO equals bld.OVC_BLD_NO into p3
        //        from bld in p3.DefaultIfEmpty()
        //        join ports in MTSE.TBGMT_PORTS on bld.OVC_START_PORT equals ports.OVC_PORT_CDE into p1
        //        from ports in p1.DefaultIfEmpty()
        //        join scc in MTSE.TBGMT_SCC on bld.OVC_BLD_NO equals scc.OVC_BLD_NO into p2
        //        from scc in p2.DefaultIfEmpty()
        //        join cinf in MTSE.TBGMT_CINF on ics.OVC_INF_NO equals cinf.OVC_INF_NO into p4
        //        from cinf in p4.DefaultIfEmpty()
        //        join ports2 in MTSE.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals ports2.OVC_PORT_CDE into p5
        //        from ports2 in p5.DefaultIfEmpty()
        //        select new
        //        {
        //            column1 = bld.OVC_SHIP_COMPANY,
        //            column1_1 = bld.OVC_BLD_NO,
        //            column2 = bld.OVC_SHIP_NAME,
        //            column2_1 = bld.OVC_VOYAGE,
        //            column3 = ports.OVC_PORT_CHI_NAME,
        //            column4 = bld.ODT_LAST_START_DATE,
        //            column5 = "",
        //            column6 = scc.ODT_ACQUIRE_DATE,
        //            column7 = scc.ODT_RETURN_DATE,
        //            column8 = cinf.ODT_PAID_DATE,
        //            column9 = scc.OVC_NOTE_CENTER,
        //            column9_1 = scc.OVC_NOTE_COMPANY,
        //            column10 = "",
        //            ODT_PLN_ARRIVE_DATE = bld.ODT_PLN_ARRIVE_DATE,
        //            OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY,
        //            OVC_VOYAGE = bld.OVC_VOYAGE,
        //            OVC_ARRIVE_PORT = bld.OVC_ARRIVE_PORT,
        //            ONB_CARRIAGE = bld.ONB_CARRIAGE,
        //            ONB_SHOW = scc.ONB_SHOW,
        //            OVC_INLAND_CARRIAGE = ics.OVC_INLAND_CARRIAGE,
        //            ports2.OVC_PORT_CHI_NAME,
        //        };
        //    if (!strovcShipCompany.Equals("不限定"))
        //        query = query.Where(table => table.OVC_SHIP_COMPANY.Equals(strovcShipCompany));
        //    if (!strOvcVoyage.Equals(string.Empty))
        //        query = query.Where(table => table.OVC_VOYAGE.Contains(strOvcVoyage));
        //    query = query.Where(table => table.ODT_PLN_ARRIVE_DATE >= dateQueryDate1 && table.ODT_PLN_ARRIVE_DATE <= dateQueryDate2);
        //    if (strArea == "桃園地區")
        //    {
        //        query = query.Where(table => table.OVC_ARRIVE_PORT == "TPE" || table.OVC_ARRIVE_PORT == "TXG" || table.OVC_ARRIVE_PORT == "HUN" || table.OVC_ARRIVE_PORT == "TTY");
        //    }
        //    if (strArea == "基隆地區")
        //    {
        //        query = query.Where(table => table.OVC_ARRIVE_PORT == "TWKEL" || table.OVC_ARRIVE_PORT == "TWHUN" || table.OVC_ARRIVE_PORT == "TWSUO" || table.OVC_ARRIVE_PORT == "TWTXG" || table.OVC_ARRIVE_PORT == "TWTPE");

        //    }
        //    if (strArea == "高雄分遣組")
        //    {
        //        query = query.Where(table => table.OVC_ARRIVE_PORT == "TWKHH" || table.OVC_ARRIVE_PORT == "KHH");
        //    }

        //    dt = CommonStatic.LinqQueryToDataTable(query);

        //    //合併dt欄位
        //    if (dt.Rows.Count > 0)
        //    {
        //        for (var i = 0; i < dt.Rows.Count; i++)
        //        {
        //            dt.Rows[i][4] = dt.Rows[i][4] + "<br/>" + dt.Rows[i][20];
        //            dt.Rows[i][6] = dt.Rows[i][17] + "<br/>" + dt.Rows[i][19];
        //            dt.Rows[i][0] = dt.Rows[i][0].ToString() + "<br/>" + dt.Rows[i][1].ToString();
        //            dt.Rows[i][2] = dt.Rows[i][2] + "<br/>" + dt.Rows[i][3];
        //            dt.Rows[i][10] = dt.Rows[i][10] + "<br/>" + dt.Rows[i][11];
        //            dt.Rows[i][12] = dt.Rows[i][18].ToString().Replace("0", "否").Replace("1", "是");
        //        }
        //    }
        //    ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_SCC, dt);
        //    if (msg == "")
        //    {
        //        FCommon.AlertShow(PnMessage, "success", "系統訊息", "更新成功");
        //    }
        //    else
        //    {
        //        FCommon.AlertShow(PnMessage, "danger", "系統訊息", msg);
        //    }
        //}
        private void dataImport_GV_TBGMT_SCC()
        {
            string strMessage = "";
            string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
            string strQueryDate1 = txtQueryDate1.Text;
            string strQueryDate2 = txtQueryDate2.Text;
            string strOVC_SHIP_COMPANY = drpOVC_SHIP_COMPANY.SelectedValue;
            string strOVC_VOYAGE = txtOVC_VOYAGE.Text;
            ViewState["OVC_SECTION"] = strOVC_SECTION;
            ViewState["QueryDate1"] = strQueryDate1;
            ViewState["QueryDate2"] = strQueryDate2;
            ViewState["OVC_SHIP_COMPANY"] = strOVC_SHIP_COMPANY;
            ViewState["OVC_VOYAGE"] = strOVC_VOYAGE;

            #region 錯誤訊息
            if (strOVC_SECTION.Equals(string.Empty))
                strMessage += "<p> 接轉地區不存在！ </p>";
            if (strQueryDate1.Equals(string.Empty))
                strMessage += "<p> 請選擇 查詢日期－開始！ </p>";
            if (strQueryDate2.Equals(string.Empty))
                strMessage += "<p> 請選擇 查詢日期－結束！ </p>";
            bool boolQueryDate1 = FCommon.checkDateTime(strQueryDate1, "查詢日期－開始", ref strMessage, out DateTime dateQueryDate1);
            bool boolQueryDate2 = FCommon.checkDateTime(strQueryDate2, "查詢日期－結束", ref strMessage, out DateTime dateQueryDate2);
            if(boolQueryDate1 && boolQueryDate2 && DateTime.Compare(dateQueryDate1, dateQueryDate2)>0)
                strMessage += "<p> 查詢日期 錯誤！ </p>";
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                DataTable dt = getTBGMT_SCC(strOVC_SECTION, strQueryDate1, strQueryDate2, strOVC_SHIP_COMPANY, strOVC_VOYAGE, true);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_SCC, dt);

                //取得是否可編輯，權限 1,2。
                bool isHasData = dt.Rows.Count > 0; //若是否有資料資料
                bool isCanEdit = isHasData && !drpOVC_SECTION.Visible; //有資料，且不可選擇接轉地區，才可以修改多項資料
                pnEdit.Visible = isHasData; //isCanEdit; //判斷權限及資料筆數
                btnPrintCenter.Visible = isHasData; //有資料才須顯示列印
                btnPrintCompany.Visible = isHasData; //有資料才須顯示列印
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        
        private DataTable getTBGMT_SCC(string strOVC_SECTION, string strQueryDate1, string strQueryDate2, string strOVC_SHIP_COMPANY, string strOVC_VOYAGE, bool isSpace)
        {
            string[] strParameterName = { ":section", ":start_date", ":end_date" };
            ArrayList aryData = new ArrayList();
            aryData.Add(strOVC_SECTION);
            aryData.Add(FCommon.getDateTime(strQueryDate1));
            aryData.Add(FCommon.getDateTime(strQueryDate2));

            string strSQL_PortList = $@"
                    select OVC_PHR_ID from TBM1407 where OVC_PHR_CATE='TR' and OVC_PHR_PARENTS={ strParameterName[0] }
                ";
            string strSQL_DateRange = $@"
                    between to_date({ strParameterName[1] }, { strDateFormatSQL }) and to_date({ strParameterName[2] }, { strDateFormatSQL })
                ";

            //string[] colName = {"航商","船名","航次","提單號碼","裝貨港","離港日","最後港口",
            //                       "結關日期前一日","軍種","運費USD","匯率","運費TWD",
            //                        "帳單收獲日期","簽證退商日期","中心支付日期","中心備考","航商備考"};
            //進口 
            string strSQL_Import = $@"
                select OVC_SHIP_COMPANY, OVC_SHIP_NAME, OVC_VOYAGE, a.OVC_BLD_NO,
                PORTCDE_TO_CHINAME(OVC_START_PORT) as OVC_START_PORT,
                ODT_LAST_START_DATE as ODT_START_DATE,
                PORTCDE_TO_CHINAME(OVC_LAST_START_PORT) as OVC_LAST_START_PORT,
                TO_DATE('','') as ODT_LAST_START_DATE,
                CLASSCDE_TO_CHINAME(OVC_MILITARY_TYPE) as OVC_MILITARY,
                a.ONB_CARRIAGE as USD_CARRIAGE,
                GET_RATE('USD', ODT_LAST_START_DATE) as RATE,
                ROUND(e.OVC_INLAND_CARRIAGE) as TWD_CARRIAGE,
                ODT_ACQUIRE_DATE, ODT_RETURN_DATE, f.ODT_PAID_DATE, OVC_NOTE_CENTER, OVC_NOTE_COMPANY,
                0 as IESORT, ODT_ACT_ARRIVE_DATE as DTSORT, ONB_SHOW

                from TBGMT_BLD a, TBGMT_ICS e, TBGMT_CINF f, TBGMT_SCC g
                where a.OVC_BLD_NO = g.OVC_BLD_NO (+)
                and a.OVC_BLD_NO = e.OVC_BLD_NO (+)
                and e.OVC_INF_NO = f.OVC_INF_NO (+)

                and (a.OVC_IS_CHARGE is null or a.OVC_IS_CHARGE <> '否')
                and a.OVC_ARRIVE_PORT in ({ strSQL_PortList })
                and ODT_PLN_ARRIVE_DATE { strSQL_DateRange }
            ";
            
            //出口          

            string strSQL_Export = $@"
                select a.OVC_SHIP_COMPANY, a.OVC_SHIP_NAME, a.OVC_VOYAGE, a.OVC_BLD_NO,
                PORTCDE_TO_CHINAME(OVC_START_PORT) as OVC_START_PORT,
                TO_DATE('','') as ODT_START_DATE,
                PORTCDE_TO_CHINAME(OVC_LAST_START_PORT) as OVC_LAST_START_PORT,
                ODT_LAST_START_DATE,
                CLASSCDE_TO_CHINAME(OVC_MILITARY_TYPE) as OVC_MILITARY,
                a.ONB_CARRIAGE as USD_CARRIAGE,
                GET_RATE('USD', ODT_LAST_START_DATE) as RATE,
                ROUND(e.OVC_INLAND_CARRIAGE) as TWD_CARRIAGE,
                ODT_ACQUIRE_DATE, ODT_RETURN_DATE, f.ODT_PAID_DATE, OVC_NOTE_CENTER, OVC_NOTE_COMPANY,
                1 as IESORT, ODT_LAST_START_DATE as DTSORT, ONB_SHOW

                from TBGMT_BLD a, TBGMT_ECL b, TBGMT_ICS e, TBGMT_CINF f, TBGMT_SCC g
                where a.OVC_BLD_NO = b.OVC_BLD_NO
                and a.OVC_BLD_NO = g.OVC_BLD_NO (+)
                and a.OVC_BLD_NO = e.OVC_BLD_NO (+)
                and e.OVC_INF_NO = f.OVC_INF_NO (+)

                and (a.OVC_IS_CHARGE is null or a.OVC_IS_CHARGE <> '否')
                and a.OVC_START_PORT in ({ strSQL_PortList })
                and ODT_EXP_DATE { strSQL_DateRange }
            ";          
            if (!strOVC_SHIP_COMPANY.Equals(string.Empty))
            {
                strSQL_Import += $@"
                        and OVC_SHIP_COMPANY = '{ strOVC_SHIP_COMPANY }'
                    ";
                strSQL_Export += $@"
                        and OVC_SHIP_COMPANY = '{ strOVC_SHIP_COMPANY }'
                    ";
            }
            if (!strOVC_VOYAGE.Equals(string.Empty))
            {
                strSQL_Import += $@"
                        and UPPER(OVC_VOYAGE) like '%' || UPPER('{ strOVC_VOYAGE }') || '%'
                    ";
                strSQL_Export += $@"
                        and UPPER(OVC_VOYAGE) like '%' || UPPER('{ strOVC_VOYAGE }') || '%'
                    ";
            }
            string strSQL = $@"
                    { strSQL_Import }
                    UNION
                    { strSQL_Export }
                    order by IESORT, DTSORT, OVC_SHIP_COMPANY desc, OVC_SHIP_NAME, OVC_VOYAGE, OVC_BLD_NO
                ";
            DataTable dt = FCommon.getDataTableFromSelect(strSQL, strParameterName, aryData);
            dt.Columns.Add("ODT_START_DATE_Text");
            dt.Columns.Add("ODT_LAST_START_DATE_Text");
            dt.Columns.Add("USD_CARRIAGE_Text");
            dt.Columns.Add("TWD_CARRIAGE_Text");
            dt.Columns.Add("ODT_ACQUIRE_DATE_Text");
            dt.Columns.Add("ODT_RETURN_DATE_Text");
            dt.Columns.Add("ODT_PAID_DATE_Text");
            dt.Columns.Add("ONB_SHOW_Text");
            foreach (DataRow dr in dt.Rows)
            {
                string strDateFormat = "MM/dd";
                dr["ODT_START_DATE_Text"] = FCommon.getDateTime(dr["ODT_START_DATE"], strDateFormat);
                dr["ODT_LAST_START_DATE_Text"] = FCommon.getDateTime(dr["ODT_LAST_START_DATE"], strDateFormat);
                dr["ODT_ACQUIRE_DATE_Text"] = FCommon.getDateTime(dr["ODT_ACQUIRE_DATE"], strDateFormat);
                dr["ODT_RETURN_DATE_Text"] = FCommon.getDateTime(dr["ODT_RETURN_DATE"], strDateFormat);
                dr["ODT_PAID_DATE_Text"] = FCommon.getDateTime(dr["ODT_PAID_DATE"], strDateFormat);
                //貨幣
                dr["USD_CARRIAGE_Text"] = dr["USD_CARRIAGE"].ToString();
                dr["TWD_CARRIAGE_Text"] = dr["TWD_CARRIAGE"].ToString();
                //報表顯示
                if (!int.TryParse(dr["ONB_SHOW"].ToString(), out int intONB_SHOW))
                    intONB_SHOW = 1; //null 預設為 是
                string strONB_SHOW_Text = intONB_SHOW == 1 ? "是" : "否"; //null 預設為 是
                dr["ONB_SHOW"] = intONB_SHOW;
                dr["ONB_SHOW_Text"] = strONB_SHOW_Text;

                if (isSpace)
                {
                    //若無資料 補空格
                    string[] strFields_Space = { "OVC_SHIP_COMPANY", "OVC_BLD_NO", "OVC_SHIP_NAME", "OVC_VOYAGE", "OVC_START_PORT", "OVC_LAST_START_PORT",
                    "ODT_START_DATE_Text", "ODT_LAST_START_DATE_Text", "USD_CARRIAGE_Text","TWD_CARRIAGE_Text","OVC_NOTE_CENTER","OVC_NOTE_COMPANY" };
                    foreach (string strFieldName in strFields_Space)
                    {
                        string strValue = dr[strFieldName].ToString();
                        if (string.IsNullOrEmpty(strValue))
                        {
                            try
                            {
                                dr[strFieldName] = null;
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }
            return dt;
        }
        //protected void Rank()
        //{
        //    //var userID = Session["userid"].ToString();
        //    //string[] query
        //    //    = GM.ACCOUNT_AUTH.Where(id => id.USER_ID.Equals(userID)).Select(auth => auth.C_SN_AUTH).ToArray();
        //    //for (var i = 0; i < query.Length; i++)
        //    //{
        //    //    string temp = query[i];
        //    //    string[] query2 = GM.TBM1407.Where(id => id.OVC_PHR_ID.Equals(temp)).Select(desc => desc.OVC_PHR_DESC).ToArray();
        //    //    if (query2.Any(array => array.Contains("物資接轉處")))
        //    //    {
        //    //        btnQuery.Visible = true;
        //    //        GV_TBGMT_SCC.Visible = true;
        //    //        //預設值
        //    //        drpArea.SelectedItem.Text = "基隆地區";
        //    //        break;
        //    //    }
        //    //    else if (query2.Any(array => array.Contains("地區接轉")))
        //    //    {
        //    //        Label1.Text = "地區接轉";
        //    //        btnQuery.Visible = true;
        //    //        GV_TBGMT_SCC.Visible = true;
        //    //        string[] query3 = GM.ACCOUNTs.Where(id => id.USER_ID.Equals(userID)).Select(dept_sn => dept_sn.DEPT_SN).ToArray();

        //    //        if (query3.Any(array => array.Contains("00N10")))
        //    //        {
        //    //            Label_Area.Text = "基隆地區";
        //    //        }
        //    //        if (query3.Any(array => array.Contains("00N20")))
        //    //        {
        //    //            Label_Area.Text = "桃園地區";
        //    //        }
        //    //        if (query3.Any(array => array.Contains("00N30")))
        //    //        {
        //    //            Label_Area.Text = "高雄分遣組";
        //    //        }
        //    //        drpArea.Visible = false;
        //    //        Label_Area.Visible = true;
        //    //        break;
        //    //    }
        //    //    else if (query2.Any(array => array.Contains("三軍")))
        //    //    {
        //    //        btnQuery_narmal.Visible = true;
        //    //        GV_TBGMT_SCC_normal.Visible = true;
        //    //        //預設值
        //    //        drpArea.SelectedItem.Text = "基隆地區";
        //    //        break;
        //    //    }
        //    //    else
        //    //    {
        //    //        Response.Write("<script>alert('您無此權限訪問!');location.href='../../GM/BulletinBoard'; </script>");
        //    //    }
        //    //}
        //}
        private void Print(string strType)
        {

            if (ViewState["OVC_SECTION"] != null && ViewState["QueryDate1"] != null && ViewState["QueryDate2"] != null && ViewState["OVC_SHIP_COMPANY"] != null && ViewState["OVC_VOYAGE"] != null)
            {
                //string strOVC_SECTION = intAuth == 2 ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
                string strOVC_SECTION = ViewState["OVC_SECTION"].ToString();
                string strQueryDate1 = ViewState["QueryDate1"].ToString();
                string strQueryDate2 = ViewState["QueryDate2"].ToString();
                string strOVC_SHIP_COMPANY = ViewState["OVC_SHIP_COMPANY"].ToString();
                string strOVC_VOYAGE = ViewState["OVC_VOYAGE"].ToString();

                string strDateFormatTaiwan = "{0}年{1}月{2}日";
                string strTaiwanDate1 = FCommon.getTaiwanDate(strQueryDate1, strDateFormatTaiwan);
                string strTaiwanDate2 = FCommon.getTaiwanDate(strQueryDate2, strDateFormatTaiwan);

                string strTitel = $"海空運進(出)口軍品運費管制表－{ strOVC_SECTION }";
                string[] strTitle = { "編\n號", "航商", "船名", "航次", "提單編號", "裝貨港", "離港日", "最後港口", "結關日期前一日", "品名", "軍種", "重量\n(體積)頓", "單價", "運費\n(USD)", "匯率", "運費\n(TWD)", "帳單收穫\n日期", "簽證退商\n日期", "中心支付\n日期", "備考" };
                BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
                Font ChFont = new Font(bfChinese, 8f, Font.BOLD);
                Font SmallChFont = new Font(bfChinese, 10f, Font.BOLD);
                Font Title_ChFont = new Font(bfChinese, 18f, Font.BOLD);
                MemoryStream Memory = new MemoryStream();
                var doc1 = new Document(PageSize.A4, 80, 80, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
                doc1.Open();

                PdfPTable Firsttable = new PdfPTable(20);
                Firsttable.SetWidths(new float[] { 1, 2, 3, 2, 5, 2, 2, 3, 2, 4, 2, 3, 2, 3, 2, 3, 2, 2, 2, 3 });
                Firsttable.TotalWidth = 580F;
                Firsttable.LockedWidth = true;
                Firsttable.DefaultCell.FixedHeight = 40f;
                Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell Title = new PdfPCell(new Phrase(strTitel, Title_ChFont));
                Title.HorizontalAlignment = Element.ALIGN_CENTER;
                Title.VerticalAlignment = Element.ALIGN_MIDDLE;
                Title.Colspan = 15;
                Title.Border = Rectangle.NO_BORDER;
                Firsttable.AddCell(Title);
                PdfPCell secTitle = new PdfPCell(new Phrase("資料期程　：\n" + strTaiwanDate1 + " - " + strTaiwanDate2, ChFont));
                secTitle.VerticalAlignment = Element.ALIGN_MIDDLE;
                secTitle.Colspan = 5;
                secTitle.Border = Rectangle.NO_BORDER;
                Firsttable.AddCell(secTitle);

                for (var i = 0; i < strTitle.Length; i++)
                {
                    Firsttable.AddCell(new Phrase(strTitle[i], ChFont));
                }

                DataTable dt = getTBGMT_SCC(strOVC_SECTION, strQueryDate1, strQueryDate2, strOVC_SHIP_COMPANY, strOVC_VOYAGE, false);
                //DataTable dt = (DataTable)ViewState["dataTable"];
                int Nonumber = 1;
                string strTypeName = ""; //中心備考 OVC_NOTE_CENTER / 航商備考 OVC_NOTE_COMPANY
                switch (strType)
                {
                    case "OVC_NOTE_CENTER":
                        strTypeName = "中心備考";
                        break;
                    case "OVC_NOTE_COMPANY":
                        strTypeName = "航商備考";
                        break;
                }
                foreach (DataRow dr in dt.Rows)
                {
                    if (!int.TryParse(dr["ONB_SHOW"].ToString(), out int intONB_SHOW))
                        intONB_SHOW = 1; //null 預設為 是
                    if (intONB_SHOW == 1) //報表顯示為是，則需要印出
                    {
                        string strOVC_BLD_NO = dr["OVC_BLD_NO"].ToString();
                        string strIESORT = dr["IESORT"].ToString(); //0:進口 1:出口
                        string strOVC_CHI_NAME = "", strOVC_CLASS_NAME = "", strONB_VOLUME = "", strONB_VOLUME_CARRIAGE = "", strONB_CARRIAGE_USD = "", strOVC_IS_ABROAD = "", strONB_CARRIAGE_TWD = "";
                        //    switch (strIESORT)
                        //    {
                        //        case "0": //進口
                        //            var query =
                        //                from bld in MTSE.TBGMT_BLD
                        //                where bld.OVC_BLD_NO.Equals(strOVC_BLD_NO)
                        //                join icr in MTSE.TBGMT_ICR on bld.OVC_BLD_NO equals icr.OVC_BLD_NO into icrTemp
                        //                from icr in icrTemp.DefaultIfEmpty()
                        //                join dept_class in MTSE.TBGMT_DEPT_CLASS on bld.OVC_MILITARY_TYPE equals dept_class.OVC_CLASS into dept_classTemp
                        //                from dept_class in dept_classTemp.DefaultIfEmpty()
                        //                select new
                        //                {
                        //                    OVC_CHI_NAME = icr != null ? icr.OVC_CHI_NAME: "", //品名
                        //                    OVC_CLASS_NAME = dept_class != null ? dept_class.OVC_CLASS_NAME : "", //軍種
                        //                    ONB_VOLUME = bld.ONB_VOLUME,
                        //                    bld.ONB_CARRIAGE
                        //                };
                        //            break;
                        //        case "1": //出口
                        //            break;
                        //    }
                        #region 查詢語法
                        var queryBld =
                            from bld in MTSE.TBGMT_BLD
                            where bld.OVC_BLD_NO.Equals(strOVC_BLD_NO)
                            join dept_class in MTSE.TBGMT_DEPT_CLASS on bld.OVC_MILITARY_TYPE equals dept_class.OVC_CLASS into dept_classTemp
                            from dept_class in dept_classTemp.DefaultIfEmpty()
                            select new
                            {
                                OVC_CLASS_NAME = dept_class != null ? dept_class.OVC_CLASS_NAME : "", //軍種
                                bld.ONB_VOLUME, //重量(體積)頓
                            };
                        var dataBld = queryBld.FirstOrDefault();
                        if (dataBld != null)
                        {
                            strOVC_CLASS_NAME = dataBld.OVC_CLASS_NAME;
                            strONB_VOLUME = dataBld.ONB_VOLUME.ToString();
                        }
                        var queryOther =
                            from bld in MTSE.TBGMT_BLD
                            where bld.OVC_BLD_NO.Equals(strOVC_BLD_NO)
                            join port in MTSE.TBGMT_PORTS on bld.OVC_START_PORT equals port.OVC_PORT_CDE into p2
                            from port in p2.DefaultIfEmpty()
                            join bld_carriage in MTSE.TBGMT_BLD_CARRIAGE on bld.OVC_BLD_NO equals bld_carriage.OVC_BLD_NO into bld_carriageTemp
                            from bld_carriage in bld_carriageTemp.DefaultIfEmpty()
                            join transfer_sea_price in MTSE.TBGMT_TRANSFER_SEA_PRICE on bld_carriage.OVC_ITEM_TYPE equals transfer_sea_price.OVC_INDEX_NO into priceTemp
                            from transfer_sea_price in priceTemp.DefaultIfEmpty()
                            where transfer_sea_price.OVC_IS_ABROAD.Equals(port != null ? port.OVC_IS_ABROAD : "")
                            select new
                            {
                                OVC_CHI_NAME = transfer_sea_price != null ? transfer_sea_price.OVC_CHI_NAME : "", //品名
                                ONB_VOLUME_CARRIAGE = transfer_sea_price != null ? transfer_sea_price.ONB_VOLUME_CARRIAGE : 0, //單價
                                                                                                                               //ONB_CARRIAGE_USD = bld.ONB_CARRIAGE, //運費(USD)
                                OVC_IS_ABROAD = transfer_sea_price != null ? transfer_sea_price.OVC_IS_ABROAD : "", //匯率
                                                                                                                    //ONB_CARRIAGE_TWD = bld_carriage != null ? bld_carriage.ONB_CARRIAGE : null //運費(TWD)
                            };
                        var dataOther = queryOther.FirstOrDefault();
                        if (dataOther != null)
                        {
                            strOVC_CHI_NAME = dataOther.OVC_CHI_NAME;
                            strONB_VOLUME_CARRIAGE = dataOther.ONB_VOLUME_CARRIAGE.ToString();
                            //strONB_CARRIAGE_USD = dataOther.ONB_CARRIAGE_USD.ToString();
                            strOVC_IS_ABROAD = dataOther.OVC_IS_ABROAD;
                            //strONB_CARRIAGE_TWD = dataOther.ONB_CARRIAGE_TWD.ToString();
                        }
                        #endregion
                        //資料庫資料表
                        Firsttable.AddCell(new Phrase((Nonumber++).ToString(), ChFont)); //編號
                        Firsttable.AddCell(new Phrase(dr["OVC_SHIP_COMPANY"].ToString(), ChFont)); //航商
                        Firsttable.AddCell(new Phrase(dr["OVC_SHIP_NAME"].ToString(), ChFont)); //船名
                        Firsttable.AddCell(new Phrase(dr["OVC_VOYAGE"].ToString(), ChFont)); //航次
                        Firsttable.AddCell(new Phrase(strOVC_BLD_NO, ChFont)); //提單編號
                        Firsttable.AddCell(new Phrase(dr["OVC_START_PORT"].ToString(), ChFont)); //裝貨港
                        Firsttable.AddCell(new Phrase(dr["ODT_START_DATE_Text"].ToString(), ChFont)); //離港日
                        Firsttable.AddCell(new Phrase(dr["OVC_LAST_START_PORT"].ToString(), ChFont)); //最後港口
                        Firsttable.AddCell(new Phrase(dr["ODT_LAST_START_DATE_Text"].ToString(), ChFont)); //結關日期前一日
                        Firsttable.AddCell(new Phrase(strOVC_CHI_NAME, ChFont)); //品名
                        Firsttable.AddCell(new Phrase(strOVC_CLASS_NAME, ChFont)); //軍種
                        Firsttable.AddCell(new Phrase(strONB_VOLUME, ChFont)); //重量(體積)頓
                        Firsttable.AddCell(new Phrase(strONB_VOLUME_CARRIAGE, ChFont)); //單價
                        Firsttable.AddCell(new Phrase(dr["USD_CARRIAGE_Text"].ToString().ToString(), ChFont)); //運費(USD) USD_CARRIAGE
                        Firsttable.AddCell(new Phrase(strOVC_IS_ABROAD, ChFont)); //匯率
                        Firsttable.AddCell(new Phrase(dr["TWD_CARRIAGE_Text"].ToString(), ChFont)); //運費(TWD) TWD_CARRIAGE
                        Firsttable.AddCell(new Phrase(dr["ODT_ACQUIRE_DATE_Text"].ToString(), ChFont)); //帳單收穫日期
                        Firsttable.AddCell(new Phrase(dr["ODT_RETURN_DATE_Text"].ToString(), ChFont)); //簽證退商日期
                        Firsttable.AddCell(new Phrase(dr["ODT_PAID_DATE_Text"].ToString(), ChFont)); //中心支付日期
                        string strOVC_NOTE = dr[strType].ToString();
                        if (!string.IsNullOrEmpty(strOVC_NOTE)) strOVC_NOTE += "\n" + strTypeName;
                        Firsttable.AddCell(new Phrase(strOVC_NOTE, ChFont)); //中心備考 OVC_NOTE_CENTER / 航商備考 OVC_NOTE_COMPANY
                    }
                }
                doc1.Add(Firsttable);
                doc1.Close();

                string strFileName = $"{ strTitel }.pdf";
                FCommon.DownloadFile(this, strFileName, Memory);
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (GV_TBGMT_SCC.Rows.Count > 0)
            //{
            //    for (var i = 0; i < GV_TBGMT_SCC.Rows.Count; i++)
            //    {
            //        Label lb1 = new Label();
            //        lb1.ID = "text1";
            //        Label lb2 = new Label();
            //        lb2.ID = "text2";
            //        Label lb3 = new Label();
            //        lb3.ID = "text3";
            //        lb3.Style.Add("display", "block");
            //        Label lb4 = new Label();
            //        lb4.ID = "text4";
            //        lb4.Style.Add("display", "block");
            //        Label lb5 = new Label();
            //        lb5.ID = "text5";
            //        TextBox txtChange = new TextBox();
            //        txtChange.ID = "txtChange";
            //        TextBox txtChange2 = new TextBox();
            //        txtChange2.ID = "txtChange2";
            //        TextBox txtChange3 = new TextBox();
            //        txtChange3.ID = "txtChange3";
            //        TextBox txtChange4 = new TextBox();
            //        txtChange4.ID = "txtChange4";
            //        CheckBox txtCheck = new CheckBox();
            //        txtCheck.ID = "txtCheck";
            //        txtCheck.Text = "是";
            //        txtCheck.Checked = true;
            //        string[] TxtChange = GV_TBGMT_SCC.Rows[i].Cells[8].Text.Split(delim, StringSplitOptions.None);
            //        txtChange.Visible = false;
            //        txtChange2.Visible = false;
            //        txtChange3.Visible = false;
            //        txtChange4.Visible = false;
            //        txtCheck.Visible = false;
            //        txtChange.Text = GV_TBGMT_SCC.Rows[i].Cells[5].Text;
            //        lb1.Text = GV_TBGMT_SCC.Rows[i].Cells[5].Text;
            //        txtChange2.Text = GV_TBGMT_SCC.Rows[i].Cells[6].Text;
            //        lb2.Text = GV_TBGMT_SCC.Rows[i].Cells[6].Text;
            //        txtChange3.Text = TxtChange[0];
            //        lb3.Text = TxtChange[0];
            //        txtChange4.Text = TxtChange[1];
            //        lb4.Text = TxtChange[1];
            //        lb5.Text = GV_TBGMT_SCC.Rows[i].Cells[9].Text;
            //        GV_TBGMT_SCC.Rows[i].Cells[5].Controls.Add(txtChange);
            //        GV_TBGMT_SCC.Rows[i].Cells[5].Controls.Add(lb1);
            //        GV_TBGMT_SCC.Rows[i].Cells[6].Controls.Add(txtChange2);
            //        GV_TBGMT_SCC.Rows[i].Cells[6].Controls.Add(lb2);
            //        GV_TBGMT_SCC.Rows[i].Cells[8].Controls.Add(txtChange3);
            //        GV_TBGMT_SCC.Rows[i].Cells[8].Controls.Add(txtChange4);
            //        GV_TBGMT_SCC.Rows[i].Cells[8].Controls.Add(lb3);
            //        GV_TBGMT_SCC.Rows[i].Cells[8].Controls.Add(lb4);
            //        GV_TBGMT_SCC.Rows[i].Cells[9].Controls.Add(txtCheck);
            //        GV_TBGMT_SCC.Rows[i].Cells[9].Controls.Add(lb5);
            //    }
            //}
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getAccountDEPTName(this, out strDEPT_SN, out strDEPT_Name);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtQueryDate1, txtQueryDate2, txtODT_ACQUIRE_DATE, txtODT_RETURN_DATE);
                    lblDEPT.Text = strDEPT_Name;
                    #region 下拉式選單
                    FCommon.list_dataImport(drpOVC_SECTION, aryAreaList, aryAreaList, "", "", false);
                    FCommon.list_dataImport(drpOVC_SHIP_COMPANY, VariableMTS.list_OVC_SHIP_COMPANY, VariableMTS.list_OVC_SHIP_COMPANY, "不限定", "", true);
                    #endregion
                    #region 設定日期
                    DateTime dateNow = DateTime.Now, dateTwoWeek = dateNow.AddDays(-15);
                    string strDateNow = FCommon.getDateTime(dateNow), strDateTwoWeek = FCommon.getDateTime(dateTwoWeek);
                    txtQueryDate1.Text = strDateTwoWeek;
                    txtQueryDate2.Text = strDateNow;
                    //txtOdtCreateDate1.Text = strDateTwoWeek;
                    //txtOdtCreateDate2.Text = strDateNow;
                    //txtOdtAcquireDate.Text = strDateNow;
                    //txtReturnDate.Text = strDateNow;
                    #endregion

                    //GV_TBGMT_SCC.Visible = false;
                    //btnUpdate.Enabled = false;

                    //if (Session["userid"] != null)
                    //{
                    #region 舊方法 intAuth取得權限
                    //string strUSER_ID = Session["userid"].ToString();
                    //搜尋該帳號所有在MTS系統中之權限角色
                    //限制條件：無日期限制或日期範圍內；啟用狀態為Y
                    //var queryAuth =
                    //    from accountAuth in GME.ACCOUNT_AUTH
                    //    where accountAuth.USER_ID.Equals(strUSER_ID)
                    //    where accountAuth.C_SN_SYS.Equals("MTS")
                    //    where accountAuth.END_DATE == null || accountAuth.END_DATE >= dateNow
                    //    where accountAuth.IS_ENABLE.Equals("Y")
                    //    join tAuth in GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("S6")) on accountAuth.C_SN_AUTH equals tAuth.OVC_PHR_ID
                    //    select new
                    //    {
                    //        accountAuth.C_SN_AUTH,
                    //        tAuth.OVC_PHR_DESC
                    //    };
                    //DataTable dt = CommonStatic.LinqQueryToDataTable(queryAuth);
                    //int intCount = dt.Rows.Count;
                    //for (int i = 0; i < intCount; i++)
                    //{
                    //    DataRow dr = dt.Rows[i];
                    //    string strOVC_PHR_DESC = dr["OVC_PHR_DESC"].ToString();
                    //    if (strOVC_PHR_DESC.Contains("物資接轉處")) //最大權限使用者
                    //    {
                    //        intAuth = 2;
                    //        break; //已經為最大權限使用者，不需再判斷其他權限
                    //    }
                    //    else if (strOVC_PHR_DESC.Contains("地區接轉") && intAuth < 1) //若權限小於此權限，才需定義為此權限
                    //        intAuth = 1;
                    //}
                    //if (intAuth == 1 && strDEPT_SN.Equals("00N00")) //地區長官 且 單位是00N00 等同物資接轉處長官
                    //    intAuth = 2;
                    #endregion
                    #region 取得權限、設定接轉地區
                    bool isCanEdit = false, isSelectSECTION=false;
                    string strOVC_SECTION = "";
                    if (strDEPT_SN.Equals("00N00"))
                    {
                        intAuth = 2;
                        FCommon.list_setValue(drpOVC_SECTION, "基隆地區");
                        isCanEdit = true;
                        isSelectSECTION = true;
                    }
                    else
                        foreach (string strArea in aryAreaList)
                        {
                            if (strDEPT_Name.Contains(strArea))
                            {
                                intAuth = 1;
                                strOVC_SECTION = strArea;
                                isCanEdit = true;
                                break;
                            }
                        }
                    if (intAuth == 0)
                        isSelectSECTION = true;

                    ViewState["auth"] = intAuth; //儲存權限
                    lblOVC_SECTION.Text = strOVC_SECTION;
                    lblOVC_SECTION.Visible = false;
                    drpOVC_SECTION.Visible = isSelectSECTION;

                    pnEdit.Visible = false; //一開始先隱藏
                    pnEdit.Enabled = isCanEdit;
                    btnUpdate.Enabled = isCanEdit;
                    int intCount_GV = GV_TBGMT_SCC.Columns.Count;
                    GV_TBGMT_SCC.Columns[intCount_GV - 2].Visible = isCanEdit; //是否顯示編輯按鈕
                    GV_TBGMT_SCC.Columns[intCount_GV - 1].Visible = isCanEdit && !isSelectSECTION; //是否顯示核取方塊
                    #endregion
                }
                //else
                //    FCommon.MessageBoxShow(this, "尚未登入，請先登入！", "login", true);
                //}
                intAuth = ViewState["auth"] != null ? (int)ViewState["auth"] : 0; //取得權限
                //if (intAuth == 0)
                //    FCommon.showMessageAuth(this, Path.GetFileName(Request.Path), false); //顯示無權限之訊息，並返回上一頁
            }
        }

        #region ~Click
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            GV_TBGMT_SCC.EditIndex = -1;
            dataImport_GV_TBGMT_SCC();
            #region 舊查詢
            //DataTable dt = new DataTable();
            //DateTime dateQueryDate1 = Convert.ToDateTime(txtQueryDate1.Text);
            //DateTime dateQueryDate2 = Convert.ToDateTime(txtQueryDate2.Text);
            //DateTime dateOdtCreateDate1 = Convert.ToDateTime(txtOdtCreateDate1.Text);
            //DateTime dateOdtCreateDate2 = Convert.ToDateTime(txtOdtCreateDate2.Text);
            //string strovcShipCompany = drpOVC_SHIP_COMPANY.SelectedValue;
            //string strOvcVoyage = txtOvcVoyage.Text;
            //string strIdrSn = txtIdrSn.Text;
            //string strArea = drpOVC_SECTION.SelectedItem.Text;
            ////將搜尋條件紀錄
            //Session["dateQueryDate1"] = dateQueryDate1;
            //Session["dateQueryDate2"] = dateQueryDate2;
            //Session["dateOdtCreateDate1"] = dateOdtCreateDate1;
            //Session["dateOdtCreateDate2"] = dateOdtCreateDate2;
            //Session["strovcShipCompany"] = strovcShipCompany;
            //Session["strOvcVoyage"] = strOvcVoyage;
            //Session["strIdrSn"] = strIdrSn;
            //Session["strArea"] = strArea;

            //var query =
            //    from ics in MTSE.TBGMT_ICS
            //    join bld in MTSE.TBGMT_BLD on ics.OVC_BLD_NO equals bld.OVC_BLD_NO into p3
            //    from bld in p3.DefaultIfEmpty()
            //    join ports in MTSE.TBGMT_PORTS on bld.OVC_START_PORT equals ports.OVC_PORT_CDE into p1
            //    from ports in p1.DefaultIfEmpty()
            //    join scc in MTSE.TBGMT_SCC on bld.OVC_BLD_NO equals scc.OVC_BLD_NO into p2
            //    from scc in p2.DefaultIfEmpty()
            //    join cinf in MTSE.TBGMT_CINF on ics.OVC_INF_NO equals cinf.OVC_INF_NO into p4
            //    from cinf in p4.DefaultIfEmpty()
            //    join ports2 in MTSE.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals ports2.OVC_PORT_CDE into p5
            //    from ports2 in p5.DefaultIfEmpty()
            //    select new
            //    {
            //        column1 = bld.OVC_SHIP_COMPANY,
            //        column1_1 = bld.OVC_BLD_NO,
            //        column2 = bld.OVC_SHIP_NAME,
            //        column2_1 = bld.OVC_VOYAGE,
            //        column3 = ports.OVC_PORT_CHI_NAME,
            //        column4 = bld.ODT_LAST_START_DATE,
            //        column5 = "",
            //        column6 = scc.ODT_ACQUIRE_DATE,
            //        column7 = scc.ODT_RETURN_DATE,
            //        column8 = cinf.ODT_PAID_DATE,
            //        column9 = scc.OVC_NOTE_CENTER,
            //        column9_1 = scc.OVC_NOTE_COMPANY,
            //        column10 = "",
            //        ODT_PLN_ARRIVE_DATE = bld.ODT_PLN_ARRIVE_DATE,
            //        OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY,
            //        OVC_VOYAGE = bld.OVC_VOYAGE,
            //        OVC_ARRIVE_PORT = bld.OVC_ARRIVE_PORT,
            //        ONB_CARRIAGE = bld.ONB_CARRIAGE,
            //        ONB_SHOW = scc.ONB_SHOW,
            //        OVC_INLAND_CARRIAGE = ics.OVC_INLAND_CARRIAGE,
            //        ports2.OVC_PORT_CHI_NAME,
            //    };
            //if (!strovcShipCompany.Equals("不限定"))
            //    query = query.Where(table => table.OVC_SHIP_COMPANY.Equals(strovcShipCompany));
            //if (!strOvcVoyage.Equals(string.Empty))
            //    query = query.Where(table => table.OVC_VOYAGE.Contains(strOvcVoyage));
            //query = query.Where(table => table.ODT_PLN_ARRIVE_DATE >= dateQueryDate1 && table.ODT_PLN_ARRIVE_DATE <= dateQueryDate2);
            //if (strArea == "桃園地區")
            //{
            //    query = query.Where(table => table.OVC_ARRIVE_PORT == "TPE" || table.OVC_ARRIVE_PORT == "TXG" || table.OVC_ARRIVE_PORT == "HUN" || table.OVC_ARRIVE_PORT == "TTY");
            //}
            //if (strArea == "基隆地區")
            //{
            //    query = query.Where(table => table.OVC_ARRIVE_PORT == "TWKEL" || table.OVC_ARRIVE_PORT == "TWHUN" || table.OVC_ARRIVE_PORT == "TWSUO" || table.OVC_ARRIVE_PORT == "TWTXG" || table.OVC_ARRIVE_PORT == "TWTPE");

            //}
            //if (strArea == "高雄分遣組")
            //{
            //    query = query.Where(table => table.OVC_ARRIVE_PORT == "TWKHH" || table.OVC_ARRIVE_PORT == "KHH");
            //}
            //dt = CommonStatic.LinqQueryToDataTable(query);


            //if (dt.Rows.Count > 0)
            //{
            //    //合併dt欄位
            //    for (var i = 0; i < dt.Rows.Count; i++)
            //    {
            //        dt.Rows[i][4] = dt.Rows[i][4] + "<br/>" + dt.Rows[i][20];
            //        dt.Rows[i][6] = dt.Rows[i][17] + "<br/>" + dt.Rows[i][19];
            //        dt.Rows[i][0] = dt.Rows[i][0].ToString() + "<br/>" + dt.Rows[i][1].ToString();
            //        dt.Rows[i][2] = dt.Rows[i][2] + "<br/>" + dt.Rows[i][3];
            //        dt.Rows[i][10] = dt.Rows[i][10] + "<br/>" + dt.Rows[i][11];
            //        dt.Rows[i][12] = dt.Rows[i][18].ToString().Replace("0", "否").Replace("1", "是");
            //    }
            //}
            //ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_SCC, dt);
            #endregion
        }
        //protected void btnQuery_narmal_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = new DataTable();
        //    DateTime dateQueryDate1 = Convert.ToDateTime(txtQueryDate1.Text);
        //    DateTime dateQueryDate2 = Convert.ToDateTime(txtQueryDate2.Text);
        //    DateTime dateOdtCreateDate1 = Convert.ToDateTime(txtOdtCreateDate1.Text);
        //    DateTime dateOdtCreateDate2 = Convert.ToDateTime(txtOdtCreateDate2.Text);
        //    string strovcShipCompany = drpOVC_SHIP_COMPANY.SelectedValue;
        //    string strOVC_VOYAGE = txtOVC_VOYAGE.Text;
        //    string strIdrSn = txtIdrSn.Text;
        //    string strArea = drpOVC_SECTION.SelectedItem.Text;
        //    //將搜尋條件紀錄
        //    Session["dateQueryDate1"] = dateQueryDate1;
        //    Session["dateQueryDate2"] = dateQueryDate2;
        //    Session["dateOdtCreateDate1"] = dateOdtCreateDate1;
        //    Session["dateOdtCreateDate2"] = dateOdtCreateDate2;
        //    Session["strovcShipCompany"] = strovcShipCompany;
        //    Session["strOvcVoyage"] = strOVC_VOYAGE;
        //    Session["strIdrSn"] = strIdrSn;
        //    Session["strArea"] = strArea;
        //    var query =
        //        from ics in MTSE.TBGMT_ICS
        //        join bld in MTSE.TBGMT_BLD on ics.OVC_BLD_NO equals bld.OVC_BLD_NO into p3
        //        from bld in p3.DefaultIfEmpty()
        //        join ports in MTSE.TBGMT_PORTS on bld.OVC_START_PORT equals ports.OVC_PORT_CDE into p1
        //        from ports in p1.DefaultIfEmpty()
        //        join scc in MTSE.TBGMT_SCC on bld.OVC_BLD_NO equals scc.OVC_BLD_NO into p2
        //        from scc in p2.DefaultIfEmpty()
        //        join cinf in MTSE.TBGMT_CINF on ics.OVC_INF_NO equals cinf.OVC_INF_NO into p4
        //        from cinf in p4.DefaultIfEmpty()
        //        join ports2 in MTSE.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals ports2.OVC_PORT_CDE into p5
        //        from ports2 in p5.DefaultIfEmpty()
        //        select new
        //        {
        //            column1 = bld.OVC_SHIP_COMPANY,
        //            column1_1 = bld.OVC_BLD_NO,
        //            column2 = bld.OVC_SHIP_NAME,
        //            column2_1 = bld.OVC_VOYAGE,
        //            column3 = ports.OVC_PORT_CHI_NAME,
        //            column4 = bld.ODT_LAST_START_DATE,
        //            column5 = "",
        //            column6 = scc.ODT_ACQUIRE_DATE,
        //            column7 = scc.ODT_RETURN_DATE,
        //            column8 = cinf.ODT_PAID_DATE,
        //            column9 = scc.OVC_NOTE_CENTER,
        //            column9_1 = scc.OVC_NOTE_COMPANY,
        //            column10 = "",
        //            ODT_PLN_ARRIVE_DATE = bld.ODT_PLN_ARRIVE_DATE,
        //            OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY,
        //            OVC_VOYAGE = bld.OVC_VOYAGE,
        //            OVC_ARRIVE_PORT = bld.OVC_ARRIVE_PORT,
        //            ONB_CARRIAGE = bld.ONB_CARRIAGE,
        //            ONB_SHOW = scc.ONB_SHOW,
        //            OVC_INLAND_CARRIAGE = ics.OVC_INLAND_CARRIAGE,
        //            ports2.OVC_PORT_CHI_NAME,
        //        };
        //    if (!strovcShipCompany.Equals("不限定"))
        //        query = query.Where(table => table.OVC_SHIP_COMPANY.Equals(strovcShipCompany));
        //    if (!strOVC_VOYAGE.Equals(string.Empty))
        //        query = query.Where(table => table.OVC_VOYAGE.Contains(strOVC_VOYAGE));
        //    query = query.Where(table => table.ODT_PLN_ARRIVE_DATE >= dateQueryDate1 && table.ODT_PLN_ARRIVE_DATE <= dateQueryDate2);
        //    if (strArea == "桃園地區")
        //    {
        //        query = query.Where(table => table.OVC_ARRIVE_PORT == "TPE" || table.OVC_ARRIVE_PORT == "TXG" || table.OVC_ARRIVE_PORT == "HUN" || table.OVC_ARRIVE_PORT == "TTY");
        //    }
        //    if (strArea == "基隆地區")
        //    {
        //        query = query.Where(table => table.OVC_ARRIVE_PORT == "TWKEL" || table.OVC_ARRIVE_PORT == "TWHUN" || table.OVC_ARRIVE_PORT == "TWSUO" || table.OVC_ARRIVE_PORT == "TWTXG" || table.OVC_ARRIVE_PORT == "TWTPE");

        //    }
        //    if (strArea == "高雄分遣組")
        //    {
        //        query = query.Where(table => table.OVC_ARRIVE_PORT == "TWKHH" || table.OVC_ARRIVE_PORT == "KHH");
        //    }
        //    dt = CommonStatic.LinqQueryToDataTable(query);


        //    if (dt.Rows.Count > 0)
        //    {
        //        //合併dt欄位
        //        for (var i = 0; i < dt.Rows.Count; i++)
        //        {
        //            dt.Rows[i][4] = dt.Rows[i][4] + "<br/>" + dt.Rows[i][20];
        //            dt.Rows[i][6] = dt.Rows[i][17] + "<br/>" + dt.Rows[i][19];
        //            dt.Rows[i][0] = dt.Rows[i][0].ToString() + "<br/>" + dt.Rows[i][1].ToString();
        //            dt.Rows[i][2] = dt.Rows[i][2] + "<br/>" + dt.Rows[i][3];
        //            dt.Rows[i][10] = dt.Rows[i][10] + "<br/>" + dt.Rows[i][11];
        //            dt.Rows[i][12] = dt.Rows[i][18].ToString().Replace("0", "否").Replace("1", "是");
        //        }
        //    }
        //    ViewState["hasRows2"] = FCommon.GridView_dataImport(GV_TBGMT_SCC_normal, dt);
        //}
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            #region 舊寫法
            //if (GV_TBGMT_SCC.Rows.Count > 0)
            //{
            //    for (var i = 0; i < GV_TBGMT_SCC.Rows.Count; i++)
            //    {
            //        CheckBox temp = (CheckBox)GV_TBGMT_SCC.Rows[i].FindControl("chkSelect");
            //        if (temp.Checked)
            //        {
            //            string strOVC_BLD_NO = GV_TBGMT_SCC.DataKeys[i].Value.ToString();

            //            var query =
            //                from bld in MTSE.TBGMT_BLD
            //                where bld.OVC_BLD_NO.Equals(strOVC_BLD_NO)
            //                join icr in MTSE.TBGMT_ICR on bld.OVC_BLD_NO equals icr.OVC_BLD_NO into p1
            //                from icr in p1.DefaultIfEmpty()
            //                join port in MTSE.TBGMT_PORTS on bld.OVC_START_PORT equals port.OVC_PORT_CDE into p2
            //                from port in p2.DefaultIfEmpty()
            //                join port2 in MTSE.TBGMT_PORTS on bld.OVC_LAST_START_PORT equals port2.OVC_PORT_CDE into p3
            //                from port2 in p3.DefaultIfEmpty()
            //                join scc in MTSE.TBGMT_SCC on bld.OVC_BLD_NO equals scc.OVC_BLD_NO into p4
            //                from scc in p4.DefaultIfEmpty()
            //                join dept_class in MTSE.TBGMT_DEPT_CLASS on bld.OVC_MILITARY_TYPE equals dept_class.OVC_CLASS into p5
            //                from dept_class in p5.DefaultIfEmpty()
            //                join bld_carriage in MTSE.TBGMT_BLD_CARRIAGE on bld.OVC_BLD_NO equals bld_carriage.OVC_BLD_NO into p6
            //                from bld_carriage in p6.DefaultIfEmpty()
            //                join transfer_sea_price in MTSE.TBGMT_TRANSFER_SEA_PRICE on bld_carriage.OVC_ITEM_TYPE equals transfer_sea_price.OVC_INDEX_NO
            //                where transfer_sea_price.OVC_IS_ABROAD == port.OVC_IS_ABROAD
            //                join ics in MTSE.TBGMT_ICS on bld.OVC_BLD_NO equals ics.OVC_BLD_NO
            //                join cinf in MTSE.TBGMT_CINF on ics.OVC_INF_NO equals cinf.OVC_INF_NO into p7
            //                from cinf in p7.DefaultIfEmpty()
            //                select new
            //                {
            //                    bld.OVC_SHIP_COMPANY,
            //                    bld.OVC_SHIP_NAME,
            //                    bld.OVC_VOYAGE,
            //                    OVC_BLD_NO = bld.OVC_BLD_NO,
            //                    OVC_PORT_CHI_NAME = port != null ? port.OVC_PORT_CHI_NAME : "",
            //                    bld.ODT_LAST_START_DATE,
            //                    temp = port2 != null ? port2.OVC_PORT_CHI_NAME : "",
            //                    bld.ODT_ACT_ARRIVE_DATE,
            //                    transfer_sea_price.OVC_CHI_NAME,
            //                    OVC_CLASS_NAME = dept_class != null ? dept_class.OVC_CLASS_NAME : "",
            //                    bld.ONB_OLD_VOLUME,
            //                    transfer_sea_price.ONB_VOLUME_CARRIAGE, //單價
            //                    bld.ONB_CARRIAGE,
            //                    str2 = transfer_sea_price.OVC_IS_ABROAD,///匯率
            //                    tmep2 = bld_carriage != null ? bld_carriage.ONB_CARRIAGE : null,
            //                    ODT_ACQUIRE_DATE = scc != null ? scc.ODT_ACQUIRE_DATE : null,
            //                    ODT_RETURN_DATE = scc != null ? scc.ODT_RETURN_DATE : null,
            //                    ODT_PAID_DATE = cinf != null ? cinf.ODT_PAID_DATE : null,
            //                    OVC_NOTE_COMPANY = scc != null ? scc.OVC_NOTE_COMPANY : null
            //                };
            //            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            //            if (dt.Rows.Count > 0)
            //            {
            //                DataRow dr = dt.Rows[0];
            //                //資料庫資料表
            //                Firsttable.AddCell(new Phrase((Nonumber).ToString(), ChFont));
            //                Firsttable.AddCell(new Phrase(dr[0].ToString(), ChFont));
            //                Firsttable.AddCell(new Phrase(dr[1].ToString(), ChFont));
            //                Firsttable.AddCell(new Phrase(dr[2].ToString(), ChFont));
            //                Firsttable.AddCell(new Phrase(dr[3].ToString(), ChFont));
            //                Firsttable.AddCell(new Phrase(dr[4].ToString(), ChFont));
            //                Firsttable.AddCell(new Phrase(FCommon.getDateTime(dr[5], "MM/dd"), ChFont));
            //                Firsttable.AddCell(new Phrase(dr[6].ToString(), ChFont));
            //                Firsttable.AddCell(new Phrase(dr[7].ToString(), ChFont));
            //                Firsttable.AddCell(new Phrase(dr[8].ToString(), ChFont));
            //                Firsttable.AddCell(new Phrase(dr[9].ToString(), ChFont));
            //                Firsttable.AddCell(new Phrase(dr[10].ToString(), ChFont));
            //                Firsttable.AddCell(new Phrase(dr[11].ToString(), ChFont));
            //                Firsttable.AddCell(new Phrase(dr[12].ToString(), ChFont));
            //                Firsttable.AddCell(new Phrase(dr[13].ToString(), ChFont));
            //                Firsttable.AddCell(new Phrase(dr[14].ToString(), ChFont));
            //                Firsttable.AddCell(new Phrase(FCommon.getDateTime(dr[15], "MM/dd"), ChFont));
            //                Firsttable.AddCell(new Phrase(FCommon.getDateTime(dr[16], "MM/dd"), ChFont));
            //                Firsttable.AddCell(new Phrase(FCommon.getDateTime(dr[17], "MM/dd"), ChFont));
            //                Firsttable.AddCell(new Phrase(dr[18].ToString(), ChFont));
            //                Nonumber++;
            //            }
            //        }
            //    }
            //}
            #endregion
            Button theButton = (Button)sender;
            string strType = theButton.CommandArgument;
            Print(strType);
        }
        protected void btnPrintCompany_Click(object sender, EventArgs e)
        {
            string strOVC_SECTION = intAuth == 2 ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
            string strTitel = $"海空運進(出)口軍品運費管制表－{ strOVC_SECTION }";
            int Nonumber = 1;
            string[] strTitle = { "編\n號", "商行", "船名", "航次", "提單編號", "裝貨港", "離港日", "最後港口", "結關日期前一日", "品名", "軍種", "重量\n(體積)頓", "單價", "運費\n(USD)", "匯率", "運費\n(TWD)", "帳單收\n穫日期", "簽證退\n商日期", "中心支\n付日期", "備考" };
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChFont = new Font(bfChinese, 8f, Font.BOLD);
            Font SmallChFont = new Font(bfChinese, 10f, Font.BOLD);
            Font Title_ChFont = new Font(bfChinese, 18f, Font.BOLD);
            MemoryStream Memory = new MemoryStream();
            var doc1 = new Document(PageSize.A4, 80, 80, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            doc1.Open();

            PdfPTable Firsttable = new PdfPTable(20);
            Firsttable.SetWidths(new float[] { 1, 2, 3, 2, 5, 2, 2, 3, 2, 4, 2, 3, 2, 3, 2, 3, 2, 2, 2, 3 });
            Firsttable.TotalWidth = 580F;
            Firsttable.LockedWidth = true;
            Firsttable.DefaultCell.FixedHeight = 40f;
            Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell Title = new PdfPCell(new Phrase(strTitel, Title_ChFont));
            Title.HorizontalAlignment = Element.ALIGN_CENTER;
            Title.VerticalAlignment = Element.ALIGN_MIDDLE;
            Title.Colspan = 16;
            Title.Border = Rectangle.NO_BORDER;
            Firsttable.AddCell(Title);
            PdfPCell secTitle = new PdfPCell(new Phrase("", ChFont));
            secTitle.VerticalAlignment = Element.ALIGN_MIDDLE;
            secTitle.Colspan = 4;
            secTitle.Border = Rectangle.NO_BORDER;
            Firsttable.AddCell(secTitle);

            for (var i = 0; i < strTitle.Length; i++)
            {
                Firsttable.AddCell(new Phrase(strTitle[i], ChFont));
            }
            if (GV_TBGMT_SCC.Rows.Count > 0)
            {
                for (var i = 0; i < GV_TBGMT_SCC.Rows.Count; i++)
                {
                    GridViewRow gvr = GV_TBGMT_SCC.Rows[i];
                    CheckBox temp = (CheckBox)GV_TBGMT_SCC.Rows[i].FindControl("chkSelect");
                    if (temp.Checked)
                    {
                        string strOVC_BLD_NO = GV_TBGMT_SCC.DataKeys[i].Value.ToString();

                        var query =
                            from bld in MTSE.TBGMT_BLD
                            where bld.OVC_BLD_NO.Equals(strOVC_BLD_NO)
                            join icr in MTSE.TBGMT_ICR on bld.OVC_BLD_NO equals icr.OVC_BLD_NO into p1
                            from icr in p1.DefaultIfEmpty()
                            join port in MTSE.TBGMT_PORTS on bld.OVC_START_PORT equals port.OVC_PORT_CDE into p2
                            from port in p2.DefaultIfEmpty()
                            join port2 in MTSE.TBGMT_PORTS on bld.OVC_LAST_START_PORT equals port2.OVC_PORT_CDE into p3
                            from port2 in p3.DefaultIfEmpty()
                            join scc in MTSE.TBGMT_SCC on bld.OVC_BLD_NO equals scc.OVC_BLD_NO into p4
                            from scc in p4.DefaultIfEmpty()
                            join dept_class in MTSE.TBGMT_DEPT_CLASS on bld.OVC_MILITARY_TYPE equals dept_class.OVC_CLASS into p5
                            from dept_class in p5.DefaultIfEmpty()
                            join bld_carriage in MTSE.TBGMT_BLD_CARRIAGE on bld.OVC_BLD_NO equals bld_carriage.OVC_BLD_NO into p6
                            from bld_carriage in p6.DefaultIfEmpty()
                            join transfer_sea_price in MTSE.TBGMT_TRANSFER_SEA_PRICE on bld_carriage.OVC_ITEM_TYPE equals transfer_sea_price.OVC_INDEX_NO
                            where transfer_sea_price.OVC_IS_ABROAD == port.OVC_IS_ABROAD
                            join ics in MTSE.TBGMT_ICS on bld.OVC_BLD_NO equals ics.OVC_BLD_NO
                            join cinf in MTSE.TBGMT_CINF on ics.OVC_INF_NO equals cinf.OVC_INF_NO into p7
                            from cinf in p7.DefaultIfEmpty()
                            select new
                            {
                                bld.OVC_SHIP_COMPANY,
                                bld.OVC_SHIP_NAME,
                                bld.OVC_VOYAGE,
                                OVC_BLD_NO = bld.OVC_BLD_NO,
                                OVC_PORT_CHI_NAME = port != null ? port.OVC_PORT_CHI_NAME : null,
                                bld.ODT_LAST_START_DATE,
                                temp = port2 != null ? port2.OVC_PORT_CHI_NAME : "",
                                bld.ODT_ACT_ARRIVE_DATE,
                                transfer_sea_price.OVC_CHI_NAME,
                                OVC_CLASS_NAME = dept_class != null ? dept_class.OVC_CLASS_NAME : "",
                                bld.ONB_OLD_VOLUME,
                                transfer_sea_price.ONB_VOLUME_CARRIAGE, //單價
                                bld.ONB_CARRIAGE,
                                str2 = transfer_sea_price.OVC_IS_ABROAD,///匯率
                                tmep2 = bld_carriage != null ? bld_carriage.ONB_CARRIAGE : null,
                                ODT_ACQUIRE_DATE = scc != null ? scc.ODT_ACQUIRE_DATE : null,
                                ODT_RETURN_DATE = scc != null ? scc.ODT_RETURN_DATE : null,
                                ODT_PAID_DATE = cinf != null ? cinf.ODT_PAID_DATE : null,
                                OVC_NOTE_COMPANY = scc != null ? scc.OVC_NOTE_COMPANY : null
                            };
                   DataTable     dt = CommonStatic.LinqQueryToDataTable(query);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            //資料庫資料表
                            Firsttable.AddCell(new Phrase((Nonumber).ToString(), ChFont));
                            Firsttable.AddCell(new Phrase(dr[0].ToString(), ChFont));
                            Firsttable.AddCell(new Phrase(dr[1].ToString(), ChFont));
                            Firsttable.AddCell(new Phrase(dr[2].ToString(), ChFont));
                            Firsttable.AddCell(new Phrase(dr[3].ToString(), ChFont));
                            Firsttable.AddCell(new Phrase(dr[4].ToString(), ChFont));
                            Firsttable.AddCell(new Phrase(FCommon.getDateTime(dr[5], "MM/dd"), ChFont));
                            Firsttable.AddCell(new Phrase(dr[6].ToString(), ChFont));
                            Firsttable.AddCell(new Phrase(dr[7].ToString(), ChFont));
                            Firsttable.AddCell(new Phrase(dr[8].ToString(), ChFont));
                            Firsttable.AddCell(new Phrase(dr[9].ToString(), ChFont));
                            Firsttable.AddCell(new Phrase(dr[10].ToString(), ChFont));
                            Firsttable.AddCell(new Phrase(dr[11].ToString(), ChFont));
                            Firsttable.AddCell(new Phrase(dr[12].ToString(), ChFont));
                           bool boolcurrencytime =  DateTime.TryParse(dr[5].ToString(), out DateTime currencytime);
                            var currency = from currency_rate in MTSE.TBGMT_CURRENCY_RATE
                                           select new
                                           {
                                               currency_rate.ONB_RATE,
                                               ODT_DATE = currency_rate.ODT_DATE,
                                               OVC_CURRENCY_CODE = currency_rate.OVC_CURRENCY_CODE,
                                           };
                            if (boolcurrencytime) currency = currency.Where(table => table.ODT_DATE >= currencytime);
                            currency = currency.Where(table => table.OVC_CURRENCY_CODE == "USD");
                            DataTable dt2 = CommonStatic.LinqQueryToDataTable(currency);


                            Firsttable.AddCell(new Phrase(dt2.Rows[dt.Rows.Count - 1][0].ToString(), ChFont));//
                            Firsttable.AddCell(new Phrase(dr[14].ToString(), ChFont));
                            Firsttable.AddCell(new Phrase(FCommon.getDateTime(dr[15], "MM/dd"), ChFont));
                            Firsttable.AddCell(new Phrase(FCommon.getDateTime(dr[16], "MM/dd"), ChFont));
                            Firsttable.AddCell(new Phrase(FCommon.getDateTime(dr[17], "MM/dd"), ChFont));
                            Firsttable.AddCell(new Phrase(dr[18].ToString(), ChFont));
                            Nonumber++;
                        }
                    }
                }
            }

            doc1.Add(Firsttable);
            doc1.Close();

            string strFileName = $"{ strTitel }.pdf";
            FCommon.DownloadFile(this, strFileName, Memory);

            //Response.Clear();//瀏覽器上顯示
            //Response.AddHeader("Content-disposition", $"attachment;filename={ strTitel }.pdf");
            //Response.ContentType = "application/octet-stream";
            //Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            //Response.OutputStream.Flush();
            //Response.Flush();
            //Response.End();
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            bool isODT_ACQUIRE_DATE = chkODT_ACQUIRE_DATE.Checked;
            bool isODT_RETURN_DATE = chkODT_RETURN_DATE.Checked;
            bool isOVC_NOTE_CENTER = chkOVC_NOTE_CENTER.Checked;
            bool isOVC_NOTE_COMPANY = chkOVC_NOTE_COMPANY.Checked;
            bool isONB_SHOW = chkONB_SHOW.Checked;
            string strODT_ACQUIRE_DATE = txtODT_ACQUIRE_DATE.Text;
            string strODT_RETURN_DATE = txtODT_RETURN_DATE.Text;
            string strOVC_NOTE_CENTER = txtOVC_NOTE_CENTER.Text;
            string strOVC_NOTE_COMPANY = txtOVC_NOTE_COMPANY.Text;
            string strONB_SHOW = drpONB_SHOW.SelectedValue;

            int intCount = GV_TBGMT_SCC.Rows.Count;
            #region 錯誤訊息
            //if (!(isODT_ACQUIRE_DATE || isODT_RETURN_DATE || isOVC_NOTE_CENTER || isOVC_NOTE_COMPANY || isONB_SHOW))
            //    strMessage += "<p> 未做任何異動！ </p>";
            if (intCount == 0)
                strMessage += "<p> 查詢無資料！ </p>";
            bool boolODT_ACQUIRE_DATE = FCommon.checkDateTime(strODT_ACQUIRE_DATE, "帳單收穫日期", ref strMessage, out DateTime dateODT_ACQUIRE_DATE);
            bool boolODT_RETURN_DATE = FCommon.checkDateTime(strODT_RETURN_DATE, "簽證退商日期", ref strMessage, out DateTime dateODT_RETURN_DATE);
            bool boolONB_SHOW = decimal.TryParse(strONB_SHOW, out decimal decONB_SHOW); //報表顯示
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                bool hasSuccess = false;
                string strDataNew = "", strDataEdit = "";
                for (int i = 0; i < intCount; i++)
                {
                    GridViewRow gvr = GV_TBGMT_SCC.Rows[i];
                    CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSelect");
                    if (FCommon.Controls_isExist(chkSelect) && chkSelect.Checked) //確認有選取
                    {
                        string id = GV_TBGMT_SCC.DataKeys[gvr.RowIndex].Value.ToString();
                        TBGMT_SCC scc = MTSE.TBGMT_SCC.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
                        if (scc == null) // 新增
                        {
                            scc = new TBGMT_SCC();
                            scc.OVC_BLD_NO = id;
                            if (isODT_ACQUIRE_DATE) if (boolODT_ACQUIRE_DATE) scc.ODT_ACQUIRE_DATE = dateODT_ACQUIRE_DATE; else scc.ODT_ACQUIRE_DATE = null;
                            if (isODT_RETURN_DATE) if (boolODT_RETURN_DATE) scc.ODT_RETURN_DATE = dateODT_RETURN_DATE; else scc.ODT_RETURN_DATE = null;
                            if (isOVC_NOTE_CENTER) scc.OVC_NOTE_CENTER = strOVC_NOTE_CENTER;
                            if (isOVC_NOTE_COMPANY) scc.OVC_NOTE_COMPANY = strOVC_NOTE_COMPANY;
                            if (isONB_SHOW) if (boolONB_SHOW) scc.ONB_SHOW = decONB_SHOW; else scc.ONB_SHOW = null;
                            MTSE.TBGMT_SCC.Add(scc);
                            MTSE.SaveChanges();
                            FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), scc.GetType().Name.ToString(), this, "新增");
                            strDataNew += strDataNew.Equals(string.Empty) ? "" : ", ";
                            strDataNew += id;
                        }
                        else // 修改
                        {
                            if (isODT_ACQUIRE_DATE) if (boolODT_ACQUIRE_DATE) scc.ODT_ACQUIRE_DATE = dateODT_ACQUIRE_DATE; else scc.ODT_ACQUIRE_DATE = null;
                            if (isODT_RETURN_DATE) if (boolODT_RETURN_DATE) scc.ODT_RETURN_DATE = dateODT_RETURN_DATE; else scc.ODT_RETURN_DATE = null;
                            if (isOVC_NOTE_CENTER) scc.OVC_NOTE_CENTER = strOVC_NOTE_CENTER;
                            if (isOVC_NOTE_COMPANY) scc.OVC_NOTE_COMPANY = strOVC_NOTE_COMPANY;
                            if (isONB_SHOW) if (boolONB_SHOW) scc.ONB_SHOW = decONB_SHOW; else scc.ONB_SHOW = null;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), scc.GetType().Name.ToString(), this, "修改");
                            strDataEdit += strDataEdit.Equals(string.Empty) ? "" : ", ";
                            strDataEdit += id;
                        }
                        hasSuccess = true;
                        //SELECT COUNT(*) FROM TBGMT_SCC WHERE OVC_BLD_NO =
                    }
                }
                if (hasSuccess)
                {
                    string strMessageSuccess = "<p> 更新資料列成功。 </p>";
                    if (!strDataNew.Equals(string.Empty)) strMessageSuccess += $"<p> 新增提單編號：{ strDataNew }。 </p>";
                    if (!strDataEdit.Equals(string.Empty)) strMessageSuccess += $"<p> 修改提單編號：{ strDataEdit }。 </p>";
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", strMessageSuccess);
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無做任何異動！");
                GV_TBGMT_SCC.EditIndex = -1;
                dataImport_GV_TBGMT_SCC();
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            #region 舊寫法
            //DateTime datetimeOdtAcquireDate = Convert.ToDateTime(txtOdtAcquireDate.Text);
            //string strOdtAcquireDate = datetimeOdtAcquireDate.ToString("yy/MM/dd");
            //DateTime datetimeReturnDate = Convert.ToDateTime(txtReturnDate.Text);
            //string strReturnDate = datetimeReturnDate.ToString("yy/MM/dd");
            //string strOvcNoteCenter = txtOvcNoteCenter.Text;
            //string strNoteCompany = txtNoteCompany.Text;
            //string strOnbShow = drpOnbShow.SelectedItem.Text;
            //// 初始化為不勾選
            //for (var i = 0; i < GV_TBGMT_SCC.Rows.Count; i++)
            //{
            //    CheckBox temp = (CheckBox)GV_TBGMT_SCC.Rows[i].FindControl("CheckBox1");
            //    temp.Checked = false;
            //}
            //// 符合條件勾選
            //for (var i = 0; i < GV_TBGMT_SCC.Rows.Count; i++)
            //{
            //    Label la1 = (Label)GV_TBGMT_SCC.Rows[i].FindControl("text1");
            //    Label la2 = (Label)GV_TBGMT_SCC.Rows[i].FindControl("text2");
            //    Label la3 = (Label)GV_TBGMT_SCC.Rows[i].FindControl("text3");
            //    Label la4 = (Label)GV_TBGMT_SCC.Rows[i].FindControl("text4");
            //    Label la5 = (Label)GV_TBGMT_SCC.Rows[i].FindControl("text5");
            //    CheckBox temp = (CheckBox)GV_TBGMT_SCC.Rows[i].FindControl("CheckBox1");
            //    if (chkOdtAcquireDate.Checked && la1.Text.Equals(strOdtAcquireDate))
            //    {
            //        temp.Checked = true;
            //    }
            //    if (chkOdtReturnDate.Checked && la2.Text.Equals(strReturnDate))
            //    {
            //        temp.Checked = true;
            //    }
            //    if (chkOvcNoteCenter.Checked && la3.Text.Contains(strOvcNoteCenter))
            //    {
            //        temp.Checked = true;
            //    }
            //    if (chkNoteCompany.Checked && la4.Text.Contains(strNoteCompany))
            //    {
            //        temp.Checked = true;
            //    }
            //    if (chkOnbShow.Checked && la5.Text.Equals(strOnbShow))
            //    {
            //        temp.Checked = true;
            //    }
            //}
            #endregion
        }
        #endregion

        #region GridView
        protected void GV_TBGMT_SCC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = theGridView.DataKeys[gvrIndex].Value.ToString();

            switch (e.CommandName)
            {
                case "DataEdit": //編輯
                    theGridView.EditIndex = gvrIndex;
                    dataImport_GV_TBGMT_SCC();
                    break;
                case "DataSave": //儲存
                    TextBox txtODT_ACQUIRE_DATE = (TextBox)gvr.FindControl("txtODT_ACQUIRE_DATE");
                    TextBox txtODT_RETURN_DATE = (TextBox)gvr.FindControl("txtODT_RETURN_DATE");
                    TextBox txtOVC_NOTE_CENTER = (TextBox)gvr.FindControl("txtOVC_NOTE_CENTER");
                    TextBox txtOVC_NOTE_COMPANY = (TextBox)gvr.FindControl("txtOVC_NOTE_COMPANY");
                    //DropDownList drpONB_SHOW = (DropDownList)gvr.FindControl("drpONB_SHOW");
                    CheckBox chkONB_SHOW = (CheckBox)gvr.FindControl("chkONB_SHOW");
                    if (FCommon.Controls_isExist(txtODT_ACQUIRE_DATE, txtODT_RETURN_DATE, txtOVC_NOTE_CENTER, txtOVC_NOTE_COMPANY, chkONB_SHOW))
                    {
                        string strMessage = "";
                        string strUserId = Session["userid"].ToString();
                        string strODT_ACQUIRE_DATE = txtODT_ACQUIRE_DATE.Text;
                        string strODT_RETURN_DATE = txtODT_RETURN_DATE.Text;
                        string strOVC_NOTE_CENTER = txtOVC_NOTE_CENTER.Text;
                        string strOVC_NOTE_COMPANY = txtOVC_NOTE_COMPANY.Text;
                        //string strONB_SHOW = drpONB_SHOW.SelectedValue;
                        bool boolONB_SHOW = chkONB_SHOW.Checked;
                        int intONB_SHOW = boolONB_SHOW ? 1 : 0;

                        TBGMT_SCC scc = MTSE.TBGMT_SCC.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
                        #region 錯誤訊息
                        bool boolODT_ACQUIRE_DATE = FCommon.checkDateTime(strODT_ACQUIRE_DATE, "帳單收穫日期", ref strMessage, out DateTime dateODT_ACQUIRE_DATE);
                        bool boolODT_RETURN_DATE = FCommon.checkDateTime(strODT_RETURN_DATE, "簽證退商日期", ref strMessage, out DateTime dateODT_RETURN_DATE);
                        //bool boolONB_SHOW = decimal.TryParse(strONB_SHOW, out decimal decONB_SHOW); //報表顯示
                        #endregion

                        if (strMessage.Equals(string.Empty))
                        {
                            if (scc == null) // 新增
                            {
                                scc = new TBGMT_SCC();
                                scc.OVC_BLD_NO = id;
                                if (boolODT_ACQUIRE_DATE) scc.ODT_ACQUIRE_DATE = dateODT_ACQUIRE_DATE; else scc.ODT_ACQUIRE_DATE = null;
                                if (boolODT_RETURN_DATE) scc.ODT_RETURN_DATE = dateODT_RETURN_DATE; else scc.ODT_RETURN_DATE = null;
                                scc.OVC_NOTE_CENTER = strOVC_NOTE_CENTER;
                                scc.OVC_NOTE_COMPANY = strOVC_NOTE_COMPANY;
                                //if (boolONB_SHOW) scc.ONB_SHOW = decONB_SHOW; else scc.ONB_SHOW = null;
                                scc.ONB_SHOW = intONB_SHOW;
                                MTSE.TBGMT_SCC.Add(scc);
                                MTSE.SaveChanges();
                                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), scc.GetType().Name.ToString(), this, "新增");
                                FCommon.AlertShow(PnMessage, "success", "系統訊息", $"編號：{ id } 新增成功。");
                            }
                            else // 修改
                            {
                                if (boolODT_ACQUIRE_DATE) scc.ODT_ACQUIRE_DATE = dateODT_ACQUIRE_DATE; else scc.ODT_ACQUIRE_DATE = null;
                                if (boolODT_RETURN_DATE) scc.ODT_RETURN_DATE = dateODT_RETURN_DATE; else scc.ODT_RETURN_DATE = null;
                                scc.OVC_NOTE_CENTER = strOVC_NOTE_CENTER;
                                scc.OVC_NOTE_COMPANY = strOVC_NOTE_COMPANY;
                                //if (boolONB_SHOW) scc.ONB_SHOW = decONB_SHOW; else scc.ONB_SHOW = null;
                                scc.ONB_SHOW = intONB_SHOW;
                                MTSE.SaveChanges();
                                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), scc.GetType().Name.ToString(), this, "修改");
                                FCommon.AlertShow(PnMessage, "success", "系統訊息", $"編號：{ id } 修改成功。");
                            }

                            theGridView.EditIndex = -1;
                            dataImport_GV_TBGMT_SCC();
                        }
                        else
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                    }
                    break;
                case "DataCancel": //取消
                    theGridView.EditIndex = -1;
                    dataImport_GV_TBGMT_SCC();
                    break;
            }
            #region 舊寫法
            ////透過ID尋找控制項
            //Button edit = (Button)GV_TBGMT_SCC.Rows[gvrIndex].FindControl("btnEdit");
            //Button update = (Button)GV_TBGMT_SCC.Rows[gvrIndex].FindControl("btnUpdate");
            //Button cancel = (Button)GV_TBGMT_SCC.Rows[gvrIndex].FindControl("btnCancel");
            //var txtChange = GV_TBGMT_SCC.Rows[gvrIndex].Cells[5].FindControl("txtChange") as TextBox;
            //var txtChange2 = GV_TBGMT_SCC.Rows[gvrIndex].Cells[5].FindControl("txtChange2") as TextBox;
            //var txtChange3 = GV_TBGMT_SCC.Rows[gvrIndex].Cells[5].FindControl("txtChange3") as TextBox;
            //var txtChange4 = GV_TBGMT_SCC.Rows[gvrIndex].Cells[5].FindControl("txtChange4") as TextBox;
            //var txtCheck = GV_TBGMT_SCC.Rows[gvrIndex].Cells[5].FindControl("txtCheck") as CheckBox;
            //Label la1 = (Label)GV_TBGMT_SCC.Rows[gvrIndex].FindControl("text1");
            //Label la2 = (Label)GV_TBGMT_SCC.Rows[gvrIndex].FindControl("text2");
            //Label la3 = (Label)GV_TBGMT_SCC.Rows[gvrIndex].FindControl("text3");
            //Label la4 = (Label)GV_TBGMT_SCC.Rows[gvrIndex].FindControl("text4");
            //Label la5 = (Label)GV_TBGMT_SCC.Rows[gvrIndex].FindControl("text5");
            //int temp = 1;
            //switch (e.CommandName)
            //{
            //    case "btnEdit":
            //        //新增控制項CSS 並顯示
            //        txtChange.CssClass = "tb tb-s position-left";
            //        txtChange2.CssClass = "tb tb-s position-left";
            //        txtChange3.CssClass = "tb tb-s position-left";
            //        txtChange4.CssClass = "tb tb-s position-left";
            //        txtChange.Visible = true;
            //        txtChange2.Visible = true;
            //        txtChange3.Visible = true;
            //        txtChange4.Visible = true;
            //        txtCheck.Visible = true;
            //        la1.Visible = false;
            //        la2.Visible = false;
            //        la3.Visible = false;
            //        la4.Visible = false;
            //        la5.Visible = false;
            //        //顯示、隱藏按鈕物件
            //        edit.Visible = false;
            //        update.Visible = true;
            //        cancel.Visible = true;
            //        break;
            //    case "btnUpdate":
            //        //透過<BR/>標籤分割文字
            //        string[] txtcolumn1 = GV_TBGMT_SCC.Rows[gvrIndex].Cells[0].Text.Split(delim, StringSplitOptions.None);
            //        string bld_no = txtcolumn1[1];
            //        //宣告錯誤訊息
            //        string msg = "";

            //        //報表顯示 是 => 1 否=>0
            //        if (!txtCheck.Checked)
            //            temp = 0;
            //        //更新資料庫資料
            //        try
            //        {
            //            TBGMT_SCC scc = MTSE.TBGMT_SCC.Where(table => table.OVC_BLD_NO.Equals(bld_no)).FirstOrDefault();
            //            scc.ODT_ACQUIRE_DATE = Convert.ToDateTime(txtChange.Text);
            //            scc.ODT_RETURN_DATE = Convert.ToDateTime(txtChange2.Text);
            //            scc.OVC_NOTE_CENTER = txtChange3.Text;
            //            scc.OVC_NOTE_COMPANY = txtChange4.Text;
            //            scc.ONB_SHOW = temp;
            //            MTSE.SaveChanges();
            //        }
            //        catch
            //        {
            //            msg += "請檢查欄位是否填寫正確、或是無填寫!!";
            //FCommon.AlertShow(PnMessage, "danger", "系統訊息", "儲存失敗，請聯絡工程師！");
            //        }
            //        //隱藏、顯示控制項
            //        edit.Visible = true;
            //        update.Visible = false;
            //        cancel.Visible = false;
            //        la1.Visible = true;
            //        la2.Visible = true;
            //        la3.Visible = true;
            //        la4.Visible = true;
            //        la5.Visible = true;
            //        //隱藏控制項
            //        txtChange.Visible = false;
            //        txtChange2.Visible = false;
            //        txtChange3.Visible = false;
            //        txtChange4.Visible = false;
            //        txtCheck.Visible = false;
            //        dataImport(msg);
            //        break;
            //    case "btnCancel":
            //        //隱藏、顯示控制項
            //        edit.Visible = true;
            //        update.Visible = false;
            //        cancel.Visible = false;
            //        //隱藏控制項
            //        la1.Visible = true;
            //        la2.Visible = true;
            //        la3.Visible = true;
            //        la4.Visible = true;
            //        la5.Visible = true;
            //        txtChange.Visible = false;
            //        txtChange2.Visible = false;
            //        txtChange3.Visible = false;
            //        txtChange4.Visible = false;
            //        txtCheck.Visible = false;
            //        break;
            //    default:
            //        break;
            //}
            #endregion
        }
        protected void GV_TBGMT_SCC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                int editIndex = theGridView.EditIndex;
                if(gvr.RowIndex == editIndex)
                {
                    //報表顯示 下拉式選單 帶值
                    HiddenField lblONB_SHOW = (HiddenField)gvr.FindControl("lblONB_SHOW");
                    //DropDownList drpONB_SHOW = (DropDownList)gvr.FindControl("drpONB_SHOW");
                    CheckBox chkONB_SHOW = (CheckBox)gvr.FindControl("chkONB_SHOW");
                    if (FCommon.Controls_isExist(lblONB_SHOW, chkONB_SHOW))
                    {
                        chkONB_SHOW.Checked = lblONB_SHOW.Value.Equals("1");
                        //FCommon.list_setValue(drpONB_SHOW, lblONB_SHOW.Value);
                    }
                    //日期
                    TextBox txtODT_RETURN_DATE = (TextBox)gvr.FindControl("txtODT_RETURN_DATE");
                    TextBox txtODT_ACQUIRE_DATE = (TextBox)gvr.FindControl("txtODT_ACQUIRE_DATE");
                    if (FCommon.Controls_isExist(txtODT_RETURN_DATE, txtODT_ACQUIRE_DATE))
                    {
                        FCommon.Controls_Attributes("readonly", "true", txtODT_RETURN_DATE, txtODT_ACQUIRE_DATE);
                    }
                }
                //讓<br/> 標籤 working
                //e.Row.Cells[0].Text = HttpUtility.HtmlDecode(e.Row.Cells[0].Text);
                //e.Row.Cells[1].Text = HttpUtility.HtmlDecode(e.Row.Cells[1].Text);
                //e.Row.Cells[2].Text = HttpUtility.HtmlDecode(e.Row.Cells[2].Text);
                //e.Row.Cells[4].Text = HttpUtility.HtmlDecode(e.Row.Cells[4].Text);
                //e.Row.Cells[8].Text = HttpUtility.HtmlDecode(e.Row.Cells[8].Text);
            }
        }
        protected void GV_TBGMT_SCC_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        //protected void GV_TBGMT_SCC_normal_PreRender(object sender, EventArgs e)
        //{
        //    bool hasRows = false;
        //    if (ViewState["hasRows2"] != null)
        //        hasRows = Convert.ToBoolean(ViewState["hasRows2"]);
        //    FCommon.GridView_PreRenderInit(sender, hasRows);
        //}
        #endregion
    }
}