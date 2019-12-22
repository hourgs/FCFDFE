using FCFDFE.Content;
using FCFDFE.Entity.CIMSModel;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace FCFDFE.pages.CIMS.G
{
    public partial class CIMS_G12 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        CIMSEntities CIMS = new CIMSEntities();
        GMEntities GM = new GMEntities();
        MPMSEntities MPMS = new MPMSEntities();
        protected void btnQuery1_Click(object sender, EventArgs e)
        {
            int num = 0;
            string[] strArray3 = new string[] { "L", "W" };
            if (txtOVC_DRECEIVE.Text=="")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "[年度]欄位必填(2碼)");
            else if (int.TryParse(txtOVC_DRECEIVE.Text, out num)==false)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "[年度]請填入2碼數字");
            else
            {
                string year = txtOVC_DRECEIVE.Text;
                DataTable dt = new DataTable();
                var query =
                    (from TBM1202 in MPMS.TBM1202.DefaultIfEmpty().AsEnumerable()
                     where TBM1202.OVC_CHECK_UNIT.Equals("0A100") || TBM1202.OVC_CHECK_UNIT.Equals("00N00")
                     where TBM1202.OVC_PURCH.Substring(2,2).Equals(year)
                    select new
                    {
                        OVC_PURCH = TBM1202.OVC_PURCH,
                    }).Distinct();
                if (query.ToList().Count==0)
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", txtOVC_DRECEIVE.Text+"年度查無資料!");
                    return;
                }
                
                    var query1 =
                    from TBM1202_query in query.DefaultIfEmpty().AsEnumerable()
                    join TBM1301_PLAN in GM.TBM1301_PLAN.DefaultIfEmpty().AsEnumerable() on TBM1202_query.OVC_PURCH equals TBM1301_PLAN.OVC_PURCH
                    where TBM1301_PLAN.OVC_PUR_APPROVE_DEP.Equals("A")
                    where strArray3.Contains(TBM1301_PLAN.OVC_PUR_AGENCY)
                    where TBM1301_PLAN.OVC_PUR_IPURCH != null
                    orderby TBM1301_PLAN.OVC_PURCH ascending
                    select new
                    {
                        OVC_PURCH_1 = TBM1301_PLAN.OVC_PURCH.Substring(0, 2),
                        OVC_PURCH = TBM1301_PLAN.OVC_PURCH,
                        OVC_PUR_AGENCY = TBM1301_PLAN.OVC_PUR_AGENCY,
                        OVC_PUR_APPROVE_DEP = "A",
                        OVC_PLAN_PURCH = TBM1301_PLAN.OVC_PLAN_PURCH == "1" ? "計畫性購案" : "非計畫性購案",
                        OVC_PUR_IPURCH = TBM1301_PLAN.OVC_PUR_IPURCH
                    };
                    string[] strArray4 = new string[]
                    {
                    "TA","TB","TC","TD","TE","TF","TG","TH","TI","TJ","TK","TL","TM","TN","TO","TP","TQ","TR","TS","TT","TU","TV","TW","TX","TY","TZ",
                    "PA","PB","PC","PD","PE","PF","PG","PH","PI","PJ","PK","PL","PM","PN","PO","PP","PQ","PR","PS","PT","PU","PV","PW","PX","PY","PZ",
                    "EA","EB","EC","ED","EE","EF","EG","EH","EI","EJ","EK","EL","EM","EN","EO","EP","EQ","ER","ES","ET","EU","EV","EW","EX","EY","EZ",
                    "FA","FB","FC","FD","FE","FF","FG","FH","FI","FJ","FK","FL","FM","FN","FO","FP","FQ","FR","FS","FT","FU","FV","FW","FX","FY","FZ",
                    "GA","GB","GC","GD","GE","GF","GG","GH","GI","GJ","GK","GL","GM","GN","GO","GP","GQ","GR","GS","GT","GU","GV","GW","GX","GY","GZ",
                    "HA",
                    "HB",
                    "AG",
                    "JA","JB","JC","JD","JE","JF","JG","JH","JI","JJ","JK","JL","JM","JN","JO","JP","JQ","JR","JS","JT","JU","JV","JW","JX","JY","JZ",
                    "LA","LB","LC","LD","LE","LF","LG","LH","LI","LJ","LK","LL","LM","LN","LO","LP","LQ","LR","LS","LT","LU","LV","LW","LX","LY","LZ",
                    "XA","XB","XC","XD","XE","XF","XG","XH","XI","XJ","XK","XL","XM","XN","XO","XP","XQ","XR","XS","XT","XU","XV","XW","XX","XY","XZ",
                    "YA","YB","YC","YD","YE","YF","YG","YH","YI","YJ","YK","YL","YM","YN","YO","YP","YQ","YR","YS","YT","YU","YV","YW","YX","YY","YZ",
                    "BA","BB","BC","BD","BE","BF","BG","BH","BI","BJ","BK","BL","BM","BN","BO","BP","BQ","BR","BS","BT","BU","BV","BW","BX","BY","BZ",
                    "HI",
                    "HP",
                    "HK",
                    "AA","AB","AC","AD","AE","AI","AK","AM","AO","AP","AQ","AR",
                    "CA","CB","CC","CD","CE","CF","CG","CH","CJ","NA"
                    };
                    query1 = query1.Where(table => strArray4.Contains(table.OVC_PURCH_1));
                    dt = CommonStatic.LinqQueryToDataTable(query1);
                if (dt.Rows.Count == 0)
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", txtOVC_DRECEIVE.Text + "年度查無資料!");
                    return;
                }
                DataColumn column = new DataColumn();
                    column.ColumnName = "RANK";
                    column.DataType = System.Type.GetType("System.Int32");
                    dt.Columns.Add(column);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Rank"] = i + 1;
                    }
                    dt.Columns["RANK"].SetOrdinal(0);//定義欄位順序

                    dt.Columns["RANK"].ColumnName = "項次";
                    dt.Columns["OVC_PURCH_1"].ColumnName = "代字";
                    dt.Columns["OVC_PURCH"].ColumnName = "購案編號";
                    dt.Columns["OVC_PUR_AGENCY"].ColumnName = "區分";
                    dt.Columns["OVC_PUR_APPROVE_DEP"].ColumnName = "核定權責";
                    dt.Columns["OVC_PLAN_PURCH"].ColumnName = "計畫性質";
                    dt.Columns["OVC_PUR_IPURCH"].ColumnName = "購案名稱";

                    IWorkbook wb = new HSSFWorkbook();
                    ISheet ws;
                    string SheetName = "StatisticAction";
                    ws = wb.CreateSheet(SheetName);
                    MemoryStream ms = new MemoryStream();

                    //設定Title樣式
                    HSSFCellStyle title = (HSSFCellStyle)wb.CreateCellStyle();
                    //邊框全細線
                    title.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    title.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    title.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    title.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;

                    HSSFFont titlefont = (HSSFFont)wb.CreateFont();
                    //粗體
                    titlefont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    title.SetFont(titlefont);
                    //第一欄標題
                    ws.CreateRow(0).CreateCell(0).SetCellValue(year + "年度採購中心收辦購案執行現況表(收辦案數之購案明細)");
                    for (int i = 1; i <= 6; i++)
                    {
                        ws.GetRow(0).CreateCell(i);
                    }
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6));
                    string timenow = "";
                    timenow = DateTime.Now.ToString("yyyy-MM-dd");
                    //第二欄資料
                    ws.CreateRow(1).CreateCell(0).SetCellValue("印表日期:" + timenow);
                    for (int i = 1; i <= 6; i++)
                    {
                        ws.GetRow(1).CreateCell(i);
                    }
                    ws.AddMergedRegion(new CellRangeAddress(1, 1, 0, 6));
                    for (int i = 0; i <= 1; i++)
                    {
                        for (int j = 0; j <= 6; j++)
                        {
                            ws.GetRow(i).GetCell(j).CellStyle = title;
                        }
                    }
                    //第三行為欄位名稱
                    ws.CreateRow(2);
                    HSSFCellStyle header = (HSSFCellStyle)wb.CreateCellStyle();
                    header.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                    header.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    header.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    header.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    header.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    header.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        ws.GetRow(2).CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                        ws.GetRow(2).GetCell(i).CellStyle = header;
                    }

                    //第四行開始為資料列
                    HSSFCellStyle data = (HSSFCellStyle)wb.CreateCellStyle();
                    data.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    data.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    data.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    data.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ws.CreateRow(i + 3);
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            ws.GetRow(i + 3).CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                            ws.GetRow(i + 3).GetCell(j).CellStyle = data;
                        }

                    }
                    //設定欄位寬度
                    ws.SetColumnWidth(0, 5 * 256);
                    ws.SetColumnWidth(1, 7 * 256);
                    ws.SetColumnWidth(2, 10 * 256);
                    ws.SetColumnWidth(3, 5 * 256);
                    ws.SetColumnWidth(4, 5 * 256);
                    ws.SetColumnWidth(5, 20 * 256);
                    ws.SetColumnWidth(6, 40 * 256);
                    wb.Write(ms);
                    string filename = year + "年度採購中心收辦購案執行現況表(收辦案數之購案明細).xls";
                    Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + filename));
                    Response.BinaryWrite(ms.ToArray());
                    wb = null;
                    ms.Close();
                    ms.Dispose();
                


            }
        }

        protected void btnQuery2_Click(object sender, EventArgs e)
        {
            int num = 0;
            string[] strArray3 = new string[] { "L", "W" };
            if (txtOVC_DBID1.Text == "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "[年度]欄位必填(2碼)");
            else if (int.TryParse(txtOVC_DBID1.Text, out num) == false)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "[年度]請填入2碼數字");
            else
            {
                string year = txtOVC_DBID1.Text;
                DataTable dt = new DataTable();
                var query =
                    (from TBM1202 in MPMS.TBM1202.DefaultIfEmpty().AsEnumerable()
                     where TBM1202.OVC_CHECK_UNIT.Equals("0A100") || TBM1202.OVC_CHECK_UNIT.Equals("00N00")
                     where TBM1202.OVC_PURCH.Substring(2, 2).Equals(year)
                     select new
                     {
                         OVC_PURCH = TBM1202.OVC_PURCH,
                     }).Distinct();
                if (query.ToList().Count == 0)
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", txtOVC_DBID1.Text + "年度查無資料!");
                    return;
                }
                var query3 =
                    (from TBM1303 in MPMS.TBM1303.DefaultIfEmpty().AsEnumerable()
                     where TBM1303.OVC_RESULT != null
                     where TBM1303.ONB_BID_RESULT != null
                     select new
                     {
                         OVC_PURCH = TBM1303.OVC_PURCH,
                         OVC_RESULT = TBM1303.OVC_RESULT,
                         ONB_BID_RESULT=TBM1303.ONB_BID_RESULT
                     }).Distinct();
                query3 = query3.Where(table => table.OVC_RESULT.Equals("0"));
                query3 = query3.Where(table => table.ONB_BID_RESULT > 0);
                var query1 =
                    from TBM1202_query in query.DefaultIfEmpty().AsEnumerable()
                    join TBM1301_PLAN in GM.TBM1301_PLAN.DefaultIfEmpty().AsEnumerable() on TBM1202_query.OVC_PURCH equals TBM1301_PLAN.OVC_PURCH
                    join TBM1303_query in query3.DefaultIfEmpty().AsEnumerable() on TBM1202_query.OVC_PURCH equals TBM1303_query.OVC_PURCH
                    where TBM1301_PLAN.OVC_PUR_APPROVE_DEP.Equals("A")
                    where strArray3.Contains(TBM1301_PLAN.OVC_PUR_AGENCY)
                    where TBM1301_PLAN.OVC_PUR_IPURCH != null
                    orderby TBM1301_PLAN.OVC_PURCH ascending
                    select new
                    {
                        OVC_PURCH_1 = TBM1301_PLAN.OVC_PURCH.Substring(0, 2),
                        OVC_PURCH = TBM1301_PLAN.OVC_PURCH,
                        OVC_PUR_AGENCY = TBM1301_PLAN.OVC_PUR_AGENCY,
                        OVC_PUR_APPROVE_DEP = "A",
                        OVC_PLAN_PURCH = TBM1301_PLAN.OVC_PLAN_PURCH == "1" ? "計畫性購案" : "非計畫性購案",
                        OVC_PUR_IPURCH = TBM1301_PLAN.OVC_PUR_IPURCH
                    };
                string[] strArray4 = new string[]
                {
                    "TA","TB","TC","TD","TE","TF","TG","TH","TI","TJ","TK","TL","TM","TN","TO","TP","TQ","TR","TS","TT","TU","TV","TW","TX","TY","TZ",
                    "PA","PB","PC","PD","PE","PF","PG","PH","PI","PJ","PK","PL","PM","PN","PO","PP","PQ","PR","PS","PT","PU","PV","PW","PX","PY","PZ",
                    "EA","EB","EC","ED","EE","EF","EG","EH","EI","EJ","EK","EL","EM","EN","EO","EP","EQ","ER","ES","ET","EU","EV","EW","EX","EY","EZ",
                    "FA","FB","FC","FD","FE","FF","FG","FH","FI","FJ","FK","FL","FM","FN","FO","FP","FQ","FR","FS","FT","FU","FV","FW","FX","FY","FZ",
                    "GA","GB","GC","GD","GE","GF","GG","GH","GI","GJ","GK","GL","GM","GN","GO","GP","GQ","GR","GS","GT","GU","GV","GW","GX","GY","GZ",
                    "HA",
                    "HB",
                    "AG",
                    "JA","JB","JC","JD","JE","JF","JG","JH","JI","JJ","JK","JL","JM","JN","JO","JP","JQ","JR","JS","JT","JU","JV","JW","JX","JY","JZ",
                    "LA","LB","LC","LD","LE","LF","LG","LH","LI","LJ","LK","LL","LM","LN","LO","LP","LQ","LR","LS","LT","LU","LV","LW","LX","LY","LZ",
                    "XA","XB","XC","XD","XE","XF","XG","XH","XI","XJ","XK","XL","XM","XN","XO","XP","XQ","XR","XS","XT","XU","XV","XW","XX","XY","XZ",
                    "YA","YB","YC","YD","YE","YF","YG","YH","YI","YJ","YK","YL","YM","YN","YO","YP","YQ","YR","YS","YT","YU","YV","YW","YX","YY","YZ",
                    "BA","BB","BC","BD","BE","BF","BG","BH","BI","BJ","BK","BL","BM","BN","BO","BP","BQ","BR","BS","BT","BU","BV","BW","BX","BY","BZ",
                    "HI",
                    "HP",
                    "HK",
                    "AA","AB","AC","AD","AE","AI","AK","AM","AO","AP","AQ","AR",
                    "CA","CB","CC","CD","CE","CF","CG","CH","CJ","NA"
                };
                query1 = query1.Where(table => strArray4.Contains(table.OVC_PURCH_1));

                dt = CommonStatic.LinqQueryToDataTable(query1);
                if (dt.Rows.Count == 0)
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", txtOVC_DBID1.Text + "年度查無資料!");
                    return;
                }
                DataColumn column = new DataColumn();
                column.ColumnName = "RANK";
                column.DataType = System.Type.GetType("System.Int32");
                dt.Columns.Add(column);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Rank"] = i + 1;
                }
                dt.Columns["RANK"].SetOrdinal(0);//定義欄位順序

                dt.Columns["RANK"].ColumnName = "項次";
                dt.Columns["OVC_PURCH_1"].ColumnName = "代字";
                dt.Columns["OVC_PURCH"].ColumnName = "購案編號";
                dt.Columns["OVC_PUR_AGENCY"].ColumnName = "區分";
                dt.Columns["OVC_PUR_APPROVE_DEP"].ColumnName = "核定權責";
                dt.Columns["OVC_PLAN_PURCH"].ColumnName = "計畫性質";
                dt.Columns["OVC_PUR_IPURCH"].ColumnName = "購案名稱";

                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                string SheetName = "StatisticAction";
                ws = wb.CreateSheet(SheetName);
                MemoryStream ms = new MemoryStream();

                //設定Title樣式
                HSSFCellStyle title = (HSSFCellStyle)wb.CreateCellStyle();
                //邊框全細線
                title.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                title.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                title.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                title.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;

                HSSFFont titlefont = (HSSFFont)wb.CreateFont();
                //粗體
                titlefont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                title.SetFont(titlefont);
                //第一欄標題
                ws.CreateRow(0).CreateCell(0).SetCellValue(year+ "年度採購中心收辦購案執行現況表(決標案數之購案明細)");
                for (int i = 1; i <= 6; i++)
                {
                    ws.GetRow(0).CreateCell(i);
                }
                ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6));
                string timenow = "";
                timenow = DateTime.Now.ToString("yyyy-MM-dd");
                //第二欄資料
                ws.CreateRow(1).CreateCell(0).SetCellValue("印表日期:" + timenow);
                for (int i = 1; i <= 6; i++)
                {
                    ws.GetRow(1).CreateCell(i);
                }
                ws.AddMergedRegion(new CellRangeAddress(1, 1, 0, 6));
                for (int i = 0; i <= 1; i++)
                {
                    for (int j = 0; j <= 6; j++)
                    {
                        ws.GetRow(i).GetCell(j).CellStyle = title;
                    }
                }
                //第三行為欄位名稱
                ws.CreateRow(2);
                HSSFCellStyle header = (HSSFCellStyle)wb.CreateCellStyle();
                header.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                header.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                header.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                header.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                header.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                header.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ws.GetRow(2).CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                    ws.GetRow(2).GetCell(i).CellStyle = header;
                }

                //第四行開始為資料列
                HSSFCellStyle data = (HSSFCellStyle)wb.CreateCellStyle();
                data.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                data.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                data.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                data.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ws.CreateRow(i + 3);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ws.GetRow(i + 3).CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                        ws.GetRow(i + 3).GetCell(j).CellStyle = data;
                    }

                }
                //設定欄位寬度
                ws.SetColumnWidth(0, 5 * 256);
                ws.SetColumnWidth(1, 7 * 256);
                ws.SetColumnWidth(2, 10 * 256);
                ws.SetColumnWidth(3, 5 * 256);
                ws.SetColumnWidth(4, 5 * 256);
                ws.SetColumnWidth(5, 20 * 256);
                ws.SetColumnWidth(6, 40 * 256);
                wb.Write(ms);
                string filename = year + "年度採購中心收辦購案執行現況表(決標案數之購案明細).xls";
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + filename));
                Response.BinaryWrite(ms.ToArray());
                wb = null;
                ms.Close();
                ms.Dispose();

            }
        }

        protected void btnQuery3_Click(object sender, EventArgs e)
        {
            int num = 0;
            string[] strArray3 = new string[] { "L", "W" };
            if (txtOVC_CONTRACT_START.Text == "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "[年度]欄位必填(2碼)");
            else if (int.TryParse(txtOVC_CONTRACT_START.Text, out num) == false)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "[年度]請填入2碼數字");
            else
            {
                string year = txtOVC_CONTRACT_START.Text;
                DataTable dt = new DataTable();
                var query =
                    (from TBM1202 in MPMS.TBM1202.DefaultIfEmpty().AsEnumerable()
                     where TBM1202.OVC_CHECK_UNIT.Equals("0A100") || TBM1202.OVC_CHECK_UNIT.Equals("00N00")
                     where TBM1202.OVC_PURCH.Substring(2, 2).Equals(year)
                     select new
                     {
                         OVC_PURCH = TBM1202.OVC_PURCH,
                     }).Distinct();
                if (query.ToList().Count == 0)
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", txtOVC_CONTRACT_START.Text + "年度查無資料!");
                    return;
                }
                var query2 =
                    from TBMRECEIVE_CONTRACT in MPMS.TBMRECEIVE_CONTRACT.DefaultIfEmpty().AsEnumerable()
                    where TBMRECEIVE_CONTRACT.OVC_DO_NAME != null
                    select new
                    {
                        OVC_PURCH = TBMRECEIVE_CONTRACT.OVC_PURCH,
                        OVC_PURCH_6 = TBMRECEIVE_CONTRACT.OVC_PURCH_6
                    };
                var query1 =
                    from TBM1202_query in query.DefaultIfEmpty().AsEnumerable()
                    join TBM1301_PLAN in GM.TBM1301_PLAN.DefaultIfEmpty().AsEnumerable() on TBM1202_query.OVC_PURCH equals TBM1301_PLAN.OVC_PURCH
                    join TBMRECEIVE_CONTRACT_query in query2.DefaultIfEmpty().AsEnumerable() on TBM1202_query.OVC_PURCH equals TBMRECEIVE_CONTRACT_query.OVC_PURCH
                    where TBM1301_PLAN.OVC_PUR_APPROVE_DEP.Equals("A")
                    where strArray3.Contains(TBM1301_PLAN.OVC_PUR_AGENCY)
                    where TBM1301_PLAN.OVC_PUR_IPURCH != null
                    orderby TBM1301_PLAN.OVC_PURCH ascending
                    select new
                    {
                        OVC_PURCH_1 = TBM1301_PLAN.OVC_PURCH.Substring(0, 2),
                        OVC_PURCH = TBM1301_PLAN.OVC_PURCH,
                        OVC_PUR_AGENCY = TBM1301_PLAN.OVC_PUR_AGENCY,
                        blank1 = "",
                        blank2 = "",
                        blank3 = "",
                        blank4 = "",
                        blank5 = "",
                        OVC_PLAN_PURCH = TBM1301_PLAN.OVC_PLAN_PURCH == "1" ? "計畫性購案" : "非計畫性購案",
                        OVC_PUR_IPURCH = TBM1301_PLAN.OVC_PUR_IPURCH
                    };
                string[] strArray4 = new string[]
                {
                    "TA","TB","TC","TD","TE","TF","TG","TH","TI","TJ","TK","TL","TM","TN","TO","TP","TQ","TR","TS","TT","TU","TV","TW","TX","TY","TZ",
                    "PA","PB","PC","PD","PE","PF","PG","PH","PI","PJ","PK","PL","PM","PN","PO","PP","PQ","PR","PS","PT","PU","PV","PW","PX","PY","PZ",
                    "EA","EB","EC","ED","EE","EF","EG","EH","EI","EJ","EK","EL","EM","EN","EO","EP","EQ","ER","ES","ET","EU","EV","EW","EX","EY","EZ",
                    "FA","FB","FC","FD","FE","FF","FG","FH","FI","FJ","FK","FL","FM","FN","FO","FP","FQ","FR","FS","FT","FU","FV","FW","FX","FY","FZ",
                    "GA","GB","GC","GD","GE","GF","GG","GH","GI","GJ","GK","GL","GM","GN","GO","GP","GQ","GR","GS","GT","GU","GV","GW","GX","GY","GZ",
                    "HA",
                    "HB",
                    "AG",
                    "JA","JB","JC","JD","JE","JF","JG","JH","JI","JJ","JK","JL","JM","JN","JO","JP","JQ","JR","JS","JT","JU","JV","JW","JX","JY","JZ",
                    "LA","LB","LC","LD","LE","LF","LG","LH","LI","LJ","LK","LL","LM","LN","LO","LP","LQ","LR","LS","LT","LU","LV","LW","LX","LY","LZ",
                    "XA","XB","XC","XD","XE","XF","XG","XH","XI","XJ","XK","XL","XM","XN","XO","XP","XQ","XR","XS","XT","XU","XV","XW","XX","XY","XZ",
                    "YA","YB","YC","YD","YE","YF","YG","YH","YI","YJ","YK","YL","YM","YN","YO","YP","YQ","YR","YS","YT","YU","YV","YW","YX","YY","YZ",
                    "BA","BB","BC","BD","BE","BF","BG","BH","BI","BJ","BK","BL","BM","BN","BO","BP","BQ","BR","BS","BT","BU","BV","BW","BX","BY","BZ",
                    "HI",
                    "HP",
                    "HK",
                    "AA","AB","AC","AD","AE","AI","AK","AM","AO","AP","AQ","AR",
                    "CA","CB","CC","CD","CE","CF","CG","CH","CJ","NA"
                };
                query1 = query1.Where(table => strArray4.Contains(table.OVC_PURCH_1));
                dt = CommonStatic.LinqQueryToDataTable(query1);
                if (dt.Rows.Count == 0)
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", txtOVC_CONTRACT_START.Text + "年度查無資料!");
                    return;
                }
                DataColumn column = new DataColumn();
                column.ColumnName = "RANK";
                column.DataType = System.Type.GetType("System.Int32");
                dt.Columns.Add(column);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Rank"] = i + 1;
                }
                dt.Columns["RANK"].SetOrdinal(0);//定義欄位順序

                dt.Columns["RANK"].ColumnName = "項次";
                dt.Columns["OVC_PURCH_1"].ColumnName = "代字";
                dt.Columns["OVC_PURCH"].ColumnName = "購案編號";
                dt.Columns["OVC_PUR_AGENCY"].ColumnName = "區分";
                dt.Columns["blank1"].ColumnName = "購辦號碼";
                dt.Columns["blank2"].ColumnName = "契約尾號";
                dt.Columns["blank3"].ColumnName = "組別";
                dt.Columns["blank4"].ColumnName = "得標廠商統編";
                dt.Columns["blank5"].ColumnName = "得標商";
                dt.Columns["OVC_PLAN_PURCH"].ColumnName = "計畫性質";
                dt.Columns["OVC_PUR_IPURCH"].ColumnName = "購案名稱";

                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                string SheetName = "StatisticAction";
                ws = wb.CreateSheet(SheetName);
                MemoryStream ms = new MemoryStream();

                //設定Title樣式
                HSSFCellStyle title = (HSSFCellStyle)wb.CreateCellStyle();
                //邊框全細線
                title.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                title.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                title.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                title.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;

                HSSFFont titlefont = (HSSFFont)wb.CreateFont();
                //粗體
                titlefont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                title.SetFont(titlefont);
                //第一欄標題
                ws.CreateRow(0).CreateCell(0).SetCellValue(year + "年度採購中心收辦購案執行現況表(收辦案數之購案明細)");
                for (int i = 1; i <= 10; i++)
                {
                    ws.GetRow(0).CreateCell(i);
                }
                ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));
                string timenow = "";
                timenow = DateTime.Now.ToString("yyyy-MM-dd");
                //第二欄資料
                ws.CreateRow(1).CreateCell(0).SetCellValue("印表日期:" + timenow);
                for (int i = 1; i <= 10; i++)
                {
                    ws.GetRow(1).CreateCell(i);
                }
                ws.AddMergedRegion(new CellRangeAddress(1, 1, 0, 10));
                for (int i = 0; i <= 1; i++)
                {
                    for (int j = 0; j <= 10; j++)
                    {
                        ws.GetRow(i).GetCell(j).CellStyle = title;
                    }
                }
                //第三行為欄位名稱
                ws.CreateRow(2);
                HSSFCellStyle header = (HSSFCellStyle)wb.CreateCellStyle();
                header.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                header.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                header.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                header.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                header.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                header.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ws.GetRow(2).CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                    ws.GetRow(2).GetCell(i).CellStyle = header;
                }

                //第四行開始為資料列
                HSSFCellStyle data = (HSSFCellStyle)wb.CreateCellStyle();
                data.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                data.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                data.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                data.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ws.CreateRow(i + 3);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ws.GetRow(i + 3).CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                        ws.GetRow(i + 3).GetCell(j).CellStyle = data;
                    }

                }
                //設定欄位寬度
                ws.SetColumnWidth(0, 5 * 256);
                ws.SetColumnWidth(1, 7 * 256);
                ws.SetColumnWidth(2, 10 * 256);
                ws.SetColumnWidth(3, 5 * 256);
                ws.SetColumnWidth(4, 5 * 256);
                ws.SetColumnWidth(5, 5 * 256);
                ws.SetColumnWidth(6, 5 * 256);
                ws.SetColumnWidth(7, 5 * 256);
                ws.SetColumnWidth(8, 5 * 256);
                ws.SetColumnWidth(9, 20 * 256);
                ws.SetColumnWidth(10, 40 * 256);
                wb.Write(ms);
                string filename = year + "年度採購中心收辦購案執行現況表(收辦案數之購案明細).xls";
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + filename));
                Response.BinaryWrite(ms.ToArray());
                wb = null;
                ms.Close();
                ms.Dispose();
            }
        }

        protected void btnQuery4_Click(object sender, EventArgs e)
        {
            int num = 0;
            string[] strArray3 = new string[] { "L", "W" };
            if (txtOVC_DOPEN.Text == "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "[年度]欄位必填(2碼)");
            else if (int.TryParse(txtOVC_DOPEN.Text, out num) == false)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "[年度]請填入2碼數字");
            else
            {
                string year = txtOVC_DOPEN.Text;
                DataTable dt = new DataTable();
                var query =
                    (from TBM1202 in MPMS.TBM1202.DefaultIfEmpty().AsEnumerable()
                     where TBM1202.OVC_CHECK_UNIT.Equals("0A100") || TBM1202.OVC_CHECK_UNIT.Equals("00N00")
                     where TBM1202.OVC_PURCH.Substring(2, 2).Equals(year)
                     select new
                     {
                         OVC_PURCH = TBM1202.OVC_PURCH,
                     }).Distinct();
                if (query.ToList().Count == 0)
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", txtOVC_DOPEN.Text + "年度查無資料!");
                    return;
                }
                var query3 =
                    from TBM1303 in MPMS.TBM1303.DefaultIfEmpty().AsEnumerable()
                    select new
                     {
                         OVC_PURCH = TBM1303.OVC_PURCH,
                         OVC_RESULT = TBM1303.OVC_RESULT==null?"": TBM1303.OVC_RESULT=="0"?"決標":TBM1303.OVC_RESULT=="1"?"流標": TBM1303.OVC_RESULT=="2"?"廢標":"",
                         ONB_BID_RESULT = TBM1303.ONB_BID_RESULT??-1,
                         OVC_PURCH_5=TBM1303.OVC_PURCH_5,
                         ONB_TIMES= TBM1303.ONB_TIMES==null?0:TBM1303.ONB_TIMES,
                         OVC_DOPEN=TBM1303.OVC_DOPEN??"",
                         ONB_GROUP=TBM1303.ONB_GROUP
                    };
                query3 = query3.Where(table=>!table.ONB_TIMES.Equals(0));
                query3 = query3.Where(table =>!table.ONB_GROUP.Equals(-1));
                var query1 =
                    from TBM1202_query in query.DefaultIfEmpty().AsEnumerable()
                    join TBM1301_PLAN in GM.TBM1301_PLAN.DefaultIfEmpty().AsEnumerable() on TBM1202_query.OVC_PURCH equals TBM1301_PLAN.OVC_PURCH
                    join TBM1303_query in query3.DefaultIfEmpty().AsEnumerable() on TBM1202_query.OVC_PURCH equals TBM1303_query.OVC_PURCH
                    where TBM1301_PLAN.OVC_PUR_APPROVE_DEP.Equals("A")
                    where strArray3.Contains(TBM1301_PLAN.OVC_PUR_AGENCY)
                    where TBM1301_PLAN.OVC_PUR_IPURCH != null
                    where (TBM1303_query.OVC_RESULT=="決標"&& TBM1303_query.ONB_BID_RESULT==-1)|| TBM1303_query.OVC_RESULT!="決標"
                    orderby TBM1301_PLAN.OVC_PURCH ascending
                    select new
                    {
                        OVC_PURCH = TBM1301_PLAN.OVC_PURCH,
                        OVC_PUR_AGENCY = TBM1301_PLAN.OVC_PUR_AGENCY,
                        OVC_PURCH_5=TBM1303_query.OVC_PURCH_5,
                        ONB_GROUP=TBM1303_query.ONB_GROUP,
                        ONB_TIMES= TBM1303_query.ONB_TIMES,
                        OVC_DOPEN=TBM1303_query.OVC_DOPEN,
                        OVC_RESULT=TBM1303_query.OVC_RESULT,
                        ONB_BID_RESULT= TBM1303_query.ONB_BID_RESULT.ToString()=="-1"?"": TBM1303_query.ONB_BID_RESULT.ToString()=="0"?"": TBM1303_query.ONB_BID_RESULT.ToString(),
                        OVC_PUR_IPURCH = TBM1301_PLAN.OVC_PUR_IPURCH
                    };
                string[] strArray4 = new string[]
                {
                    "TA","TB","TC","TD","TE","TF","TG","TH","TI","TJ","TK","TL","TM","TN","TO","TP","TQ","TR","TS","TT","TU","TV","TW","TX","TY","TZ",
                    "PA","PB","PC","PD","PE","PF","PG","PH","PI","PJ","PK","PL","PM","PN","PO","PP","PQ","PR","PS","PT","PU","PV","PW","PX","PY","PZ",
                    "EA","EB","EC","ED","EE","EF","EG","EH","EI","EJ","EK","EL","EM","EN","EO","EP","EQ","ER","ES","ET","EU","EV","EW","EX","EY","EZ",
                    "FA","FB","FC","FD","FE","FF","FG","FH","FI","FJ","FK","FL","FM","FN","FO","FP","FQ","FR","FS","FT","FU","FV","FW","FX","FY","FZ",
                    "GA","GB","GC","GD","GE","GF","GG","GH","GI","GJ","GK","GL","GM","GN","GO","GP","GQ","GR","GS","GT","GU","GV","GW","GX","GY","GZ",
                    "HA",
                    "HB",
                    "AG",
                    "JA","JB","JC","JD","JE","JF","JG","JH","JI","JJ","JK","JL","JM","JN","JO","JP","JQ","JR","JS","JT","JU","JV","JW","JX","JY","JZ",
                    "LA","LB","LC","LD","LE","LF","LG","LH","LI","LJ","LK","LL","LM","LN","LO","LP","LQ","LR","LS","LT","LU","LV","LW","LX","LY","LZ",
                    "XA","XB","XC","XD","XE","XF","XG","XH","XI","XJ","XK","XL","XM","XN","XO","XP","XQ","XR","XS","XT","XU","XV","XW","XX","XY","XZ",
                    "YA","YB","YC","YD","YE","YF","YG","YH","YI","YJ","YK","YL","YM","YN","YO","YP","YQ","YR","YS","YT","YU","YV","YW","YX","YY","YZ",
                    "BA","BB","BC","BD","BE","BF","BG","BH","BI","BJ","BK","BL","BM","BN","BO","BP","BQ","BR","BS","BT","BU","BV","BW","BX","BY","BZ",
                    "HI",
                    "HP",
                    "HK",
                    "AA","AB","AC","AD","AE","AI","AK","AM","AO","AP","AQ","AR",
                    "CA","CB","CC","CD","CE","CF","CG","CH","CJ","NA"
                };
                query1 = query1.Where(table => strArray4.Contains(table.OVC_PURCH.Substring(0, 2)));

                dt = CommonStatic.LinqQueryToDataTable(query1);
                if (dt.Rows.Count == 0)
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", txtOVC_DOPEN.Text + "年度查無資料!");
                    return;
                }
                DataColumn column = new DataColumn();
                column.ColumnName = "RANK";
                column.DataType = System.Type.GetType("System.Int32");
                dt.Columns.Add(column);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Rank"] = i + 1;
                }
                dt.Columns["RANK"].SetOrdinal(0);//定義欄位順序

                dt.Columns["RANK"].ColumnName = "項次";
                dt.Columns["OVC_PURCH"].ColumnName = "購案編號";
                dt.Columns["OVC_PUR_AGENCY"].ColumnName = "區分";
                dt.Columns["OVC_PURCH_5"].ColumnName = "購辦號碼";
                dt.Columns["ONB_GROUP"].ColumnName = "組別";
                dt.Columns["ONB_TIMES"].ColumnName = "開標次數";
                dt.Columns["OVC_DOPEN"].ColumnName = "開標日期";
                dt.Columns["OVC_RESULT"].ColumnName = "開標結果";
                dt.Columns["ONB_BID_RESULT"].ColumnName = "決標金額";
                dt.Columns["OVC_PUR_IPURCH"].ColumnName = "購案名稱";

                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                string SheetName = "StatisticAction";
                ws = wb.CreateSheet(SheetName);
                MemoryStream ms = new MemoryStream();

                //設定Title樣式
                HSSFCellStyle title = (HSSFCellStyle)wb.CreateCellStyle();
                //邊框全細線
                title.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                title.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                title.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                title.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;

                HSSFFont titlefont = (HSSFFont)wb.CreateFont();
                //粗體
                titlefont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                title.SetFont(titlefont);
                //第一欄標題
                ws.CreateRow(0).CreateCell(0).SetCellValue(year + "年度未完成決標簽約購案統計表");
                for (int i = 1; i <= 9; i++)
                {
                    ws.GetRow(0).CreateCell(i);
                }
                ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 9));
                string timenow = "";
                timenow = DateTime.Now.ToString("yyyy-MM-dd");
                //第二欄資料
                ws.CreateRow(1).CreateCell(0).SetCellValue("印表日期:" + timenow);
                for (int i = 1; i <= 9; i++)
                {
                    ws.GetRow(1).CreateCell(i);
                }
                ws.AddMergedRegion(new CellRangeAddress(1, 1, 0, 9));
                for (int i = 0; i <= 1; i++)
                {
                    for (int j = 0; j <= 9; j++)
                    {
                        ws.GetRow(i).GetCell(j).CellStyle = title;
                    }
                }
                //第三行為欄位名稱
                ws.CreateRow(2);
                HSSFCellStyle header = (HSSFCellStyle)wb.CreateCellStyle();
                header.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                header.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                header.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                header.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                header.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                header.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ws.GetRow(2).CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                    ws.GetRow(2).GetCell(i).CellStyle = header;
                }

                //第四行開始為資料列
                HSSFCellStyle data = (HSSFCellStyle)wb.CreateCellStyle();
                data.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                data.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                data.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                data.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ws.CreateRow(i + 3);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ws.GetRow(i + 3).CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                        ws.GetRow(i + 3).GetCell(j).CellStyle = data;
                    }

                }
                //設定欄位寬度
                ws.SetColumnWidth(0, 5 * 256);
                ws.SetColumnWidth(1, 7 * 256);
                ws.SetColumnWidth(2, 10 * 256);
                ws.SetColumnWidth(3, 5 * 256);
                ws.SetColumnWidth(4, 5 * 256);
                ws.SetColumnWidth(5, 20 * 256);
                ws.SetColumnWidth(6, 40 * 256);
                wb.Write(ms);
                string filename = year + "年度未完成決標簽約購案統計表.xls";
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + filename));
                Response.BinaryWrite(ms.ToArray());
                wb = null;
                ms.Close();
                ms.Dispose();

            }
        }

        protected void btnQuery5_Click(object sender, EventArgs e)
        {
            int num = 0;
            if (txtOVC_DBID2.Text == "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "[年度]欄位必填(2碼)");
            else if (int.TryParse(txtOVC_DBID2.Text, out num) == false)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "[年度]請填入2碼數字");
            else
            {
                string year = txtOVC_DBID2.Text;
                DataTable dt = new DataTable();
                string[] strArray1 = new string[] { "0", "1" };
                string[] strArray2 = new string[] { "A", "B", "C", "X" };
                string[] strArray3 = new string[] { "L", "W" };
                var query1 =
                    from TBM1303 in MPMS.TBM1303.DefaultIfEmpty().AsEnumerable()
                    where TBM1303.ONB_GROUP != null
                    select new
                    {
                        OVC_PURCH = TBM1303.OVC_PURCH,
                        OVC_PURCH_5 = TBM1303.OVC_PURCH_5,
                        ONB_GROUP = TBM1303.ONB_GROUP.ToString(),
                        ONB_TIMES = TBM1303.ONB_TIMES,
                        OVC_DOPEN = TBM1303.OVC_DOPEN ?? "",
                        OVC_RESULT = TBM1303.OVC_RESULT =="0"? "決標" : "",
                        ONB_BID_VENDORS = TBM1303.ONB_BID_VENDORS,
                    };
                query1 = query1.Where(table => table.OVC_RESULT.Equals("決標"));
                var query2 =
                    from TBMBID_RESULT in MPMS.TBMBID_RESULT.DefaultIfEmpty().AsEnumerable()
                    where TBMBID_RESULT.ONB_GROUP != null
                    select new
                    {
                        OVC_PURCH = TBMBID_RESULT.OVC_PURCH,
                        OVC_PURCH_5 = TBMBID_RESULT.OVC_PURCH_5,
                        ONB_GROUP = TBMBID_RESULT.ONB_GROUP.ToString(),
                        OVC_CURRENT = TBMBID_RESULT.OVC_CURRENT ?? "",
                        ONB_BUDGET = TBMBID_RESULT.ONB_BUDGET,
                        OVC_BID_CURRENT = TBMBID_RESULT.OVC_BID_CURRENT ?? "",
                        ONB_BID_BUDGET = TBMBID_RESULT.ONB_BID_BUDGET,
                        OVC_RESULT_CURRENT = TBMBID_RESULT.OVC_RESULT_CURRENT ?? "",
                        ONB_BID_RESULT = TBMBID_RESULT.ONB_BID_RESULT,
                        OVC_REMAIN_CURRENT = TBMBID_RESULT.OVC_REMAIN_CURRENT ?? "",
                        ONB_REMAIN_BUDGET = TBMBID_RESULT.ONB_REMAIN_BUDGET,
                        PERSENT_BID = (TBMBID_RESULT.ONB_BID_BUDGET == 0 || TBMBID_RESULT.ONB_BID_BUDGET == null) ? 0 + "%" : Math.Truncate((Convert.ToDecimal(TBMBID_RESULT.ONB_BID_RESULT) / Convert.ToDecimal(TBMBID_RESULT.ONB_BID_BUDGET)) * 100) + "%",
                        OVC_DBID = TBMBID_RESULT.OVC_DBID ?? "",
                    };
                var query3 =
                    from TBM1302 in MPMS.TBM1302.DefaultIfEmpty().AsEnumerable()
                    select new
                    {
                        OVC_PURCH = TBM1302.OVC_PURCH,
                        OVC_PURCH_5 = TBM1302.OVC_PURCH_5,
                        ONB_GROUP = TBM1302.ONB_GROUP.ToString(),
                        OVC_PURCH_6 = TBM1302.OVC_PURCH_6 ?? "",
                        OVC_VEN_TITLE = TBM1302.OVC_VEN_TITLE ?? "",
                    };

                var query4 = from TBM1303 in query1.DefaultIfEmpty().AsEnumerable()
                            join TBM1302 in query3.DefaultIfEmpty().AsEnumerable() on new { A = TBM1303.OVC_PURCH, B = TBM1303.OVC_PURCH_5, C = TBM1303.ONB_GROUP } equals new { A = TBM1302.OVC_PURCH, B = TBM1302.OVC_PURCH_5, C = TBM1302.ONB_GROUP }
                            join TBMBID_RESULT in query2.DefaultIfEmpty().AsEnumerable() on new { D=TBM1302.OVC_PURCH, E=TBM1302.OVC_PURCH_5, F=TBM1302.ONB_GROUP } equals new { D=TBMBID_RESULT.OVC_PURCH, E=TBMBID_RESULT.OVC_PURCH_5, F=TBMBID_RESULT.ONB_GROUP }
                            join TBM1301_PLAN in GM.TBM1301_PLAN.DefaultIfEmpty().AsEnumerable() on TBMBID_RESULT.OVC_PURCH equals TBM1301_PLAN.OVC_PURCH
                             //where strArray1.Contains(TBM1303.OVC_RESULT)
                             //where TBM1301_PLAN.OVC_PURCHASE_UNIT.Equals("0A100")
                             //where strArray2.Contains(TBM1301_PLAN.OVC_PUR_APPROVE_DEP)
                             where strArray3.Contains(TBM1301_PLAN.OVC_PUR_AGENCY)
                             where TBM1303.OVC_PURCH.Substring(2, 2).Equals(year)
                             orderby TBM1303.OVC_PURCH ascending
                            select new
                            {
                                JIANJA = "",
                                OVC_PURCH = TBM1301_PLAN.OVC_PURCH == null || TBM1301_PLAN.OVC_PUR_AGENCY == null || TBM1303.OVC_PURCH_5 == null ? "" : TBM1301_PLAN.OVC_PURCH + TBM1301_PLAN.OVC_PUR_AGENCY + TBM1303.OVC_PURCH_5,
                                OVC_PUR_IPURCH = TBM1301_PLAN.OVC_PUR_IPURCH ?? "",
                                ONB_GROUP = TBM1302.ONB_GROUP,
                                OVC_PURCH_6 = TBM1302.OVC_PURCH_6,
                                OVC_PUR_NSECTION = TBM1301_PLAN.OVC_PUR_NSECTION ?? "",
                                OVC_PUR_ASS_VEN_CODE = TBM1301_PLAN.OVC_PUR_ASS_VEN_CODE ?? "",
                                ONB_TIMES = TBM1303.ONB_TIMES,
                                OVC_PUR_APPROVE_DEP = TBM1301_PLAN.OVC_PUR_APPROVE_DEP == null ? "" : TBM1301_PLAN.OVC_PUR_APPROVE_DEP == "A" ? "國防部權責" : TBM1301_PLAN.OVC_PUR_APPROVE_DEP == "B" ? "國防部授權單位自行核定權責" : TBM1301_PLAN.OVC_PUR_APPROVE_DEP == "C" ? "國防部授權單位自行下授何定權責" : "其他",
                                OVC_CURRENT = TBMBID_RESULT.OVC_CURRENT,
                                ONB_BUDGET = TBMBID_RESULT.ONB_BUDGET,
                                OVC_BID_CURRENT = TBMBID_RESULT.OVC_BID_CURRENT,
                                ONB_BID_BUDGET = TBMBID_RESULT.ONB_BID_BUDGET,
                                OVC_RESULT_CURRENT = TBMBID_RESULT.OVC_RESULT_CURRENT,
                                ONB_BID_RESULT = TBMBID_RESULT.ONB_BID_RESULT,
                                OVC_REMAIN_CURRENT = TBMBID_RESULT.OVC_REMAIN_CURRENT,
                                ONB_REMAIN_BUDGET = TBMBID_RESULT.ONB_REMAIN_BUDGET,
                                PERSENT_BID = TBMBID_RESULT.PERSENT_BID,
                                OVC_VEN_TITLE = TBM1302.OVC_VEN_TITLE,
                                OVC_DBID = TBMBID_RESULT.OVC_DBID,
                                OVC_DOPEN = TBM1303.OVC_DOPEN,
                                OVC_RESULT = TBM1303.OVC_RESULT,
                                ONB_BID_VENDORS = TBM1303.ONB_BID_VENDORS,
                                NULL1 = "",
                                NULL2 = "",
                                NULL3 = "",
                                NULL4 = ""
                            };
                query4 = query4.Where(table => table.ONB_BID_RESULT > 0);

                //var query = from TBM1303 in MPMS.TBM1303.DefaultIfEmpty().AsEnumerable()
                //            join TBM1302 in MPMS.TBM1302.DefaultIfEmpty().AsEnumerable() on new { A = TBM1303.OVC_PURCH, B = TBM1303.OVC_PURCH_5, C = TBM1303.ONB_GROUP } equals new { A = TBM1302.OVC_PURCH, B = TBM1302.OVC_PURCH_5, C = TBM1302.ONB_GROUP } into tempTBM1302
                //            from TBM1302 in tempTBM1302.DefaultIfEmpty().AsEnumerable()
                //            join TBMBID_RESULT in MPMS.TBMBID_RESULT.DefaultIfEmpty().AsEnumerable() on new { TBM1302.OVC_PURCH, TBM1302.OVC_PURCH_5, TBM1302.ONB_GROUP } equals new { TBMBID_RESULT.OVC_PURCH, TBMBID_RESULT.OVC_PURCH_5, TBMBID_RESULT.ONB_GROUP } into tempTBMBID_RESULT
                //            from TBMBID_RESULT in tempTBMBID_RESULT.DefaultIfEmpty().AsEnumerable()
                //            join TBM1301_PLAN in GM.TBM1301_PLAN.DefaultIfEmpty().AsEnumerable() on TBMBID_RESULT.OVC_PURCH equals TBM1301_PLAN.OVC_PURCH
                //            where strArray1.Contains(TBM1303.OVC_RESULT)
                //            where TBM1301_PLAN.OVC_PURCHASE_UNIT.Equals("0A100")
                //            where strArray2.Contains(TBM1301_PLAN.OVC_PUR_APPROVE_DEP)
                //            where strArray3.Contains(TBM1301_PLAN.OVC_PUR_AGENCY)
                //            where TBM1303.OVC_PURCH.Substring(2, 2).Equals(year)
                //            orderby TBM1303.OVC_DOPEN ascending
                //            select new
                //            {
                //                JIANJA = "",
                //                OVC_PURCH = TBM1301_PLAN.OVC_PURCH == null || TBM1301_PLAN.OVC_PUR_AGENCY == null || TBM1303.OVC_PURCH_5 == null ? "" : TBM1301_PLAN.OVC_PURCH + TBM1301_PLAN.OVC_PUR_AGENCY + TBM1303.OVC_PURCH_5,
                //                OVC_PUR_IPURCH = TBM1301_PLAN.OVC_PUR_IPURCH ?? "",
                //                ONB_GROUP = TBM1302.ONB_GROUP,
                //                OVC_PURCH_6 = TBM1302.OVC_PURCH_6 ?? "",
                //                OVC_PUR_NSECTION = TBM1301_PLAN.OVC_PUR_NSECTION ?? "",
                //                OVC_PUR_ASS_VEN_CODE = TBM1301_PLAN.OVC_PUR_ASS_VEN_CODE ?? "",
                //                ONB_TIMES = TBM1303.ONB_TIMES,
                //                OVC_PUR_APPROVE_DEP = TBM1301_PLAN.OVC_PUR_APPROVE_DEP == null ? "" : TBM1301_PLAN.OVC_PUR_APPROVE_DEP == "A" ? "國防部權責" : TBM1301_PLAN.OVC_PUR_APPROVE_DEP == "B" ? "國防部授權單位自行核定權責" : TBM1301_PLAN.OVC_PUR_APPROVE_DEP == "C" ? "國防部授權單位自行下授何定權責" : "其他",
                //                OVC_CURRENT = TBMBID_RESULT.OVC_CURRENT ?? "",
                //                ONB_BUDGET = TBMBID_RESULT.ONB_BUDGET,
                //                OVC_BID_CURRENT = TBMBID_RESULT.OVC_BID_CURRENT ?? "",
                //                ONB_BID_BUDGET = TBMBID_RESULT.ONB_BID_BUDGET,
                //                OVC_RESULT_CURRENT = TBMBID_RESULT.OVC_RESULT_CURRENT ?? "",
                //                ONB_BID_RESULT = TBMBID_RESULT.ONB_BID_RESULT,
                //                OVC_REMAIN_CURRENT = TBMBID_RESULT.OVC_REMAIN_CURRENT ?? "",
                //                ONB_REMAIN_BUDGET = TBMBID_RESULT.ONB_REMAIN_BUDGET,
                //                PERSENT_BID = TBMBID_RESULT.ONB_BID_BUDGET == 0 ? 0 + "%" : Math.Truncate((Convert.ToDecimal(TBMBID_RESULT.ONB_BID_RESULT) / Convert.ToDecimal(TBMBID_RESULT.ONB_BID_BUDGET)) * 100) + "%",
                //                OVC_VEN_TITLE = TBM1302.OVC_VEN_TITLE ?? "",
                //                OVC_DBID = TBMBID_RESULT.OVC_DBID ?? "",
                //                OVC_DOPEN = TBM1303.OVC_DOPEN ?? "",
                //                OVC_RESULT = TBM1303.OVC_RESULT == null ? "" : TBM1303.OVC_RESULT == "0" ? "決標" : TBM1303.OVC_RESULT == "1" ? "流標" : TBM1303.OVC_RESULT == "2" ? "廢標" : "",
                //                ONB_BID_VENDORS = TBM1303.ONB_BID_VENDORS,
                //                NULL1 = "",
                //                NULL2 = "",
                //                NULL3 = "",
                //                NULL4 = ""
                //            };


                dt = CommonStatic.LinqQueryToDataTable(query4);
                DataColumn column = new DataColumn();
                column.ColumnName = "RANK";
                column.DataType = System.Type.GetType("System.Int32");
                dt.Columns.Add(column);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Rank"] = i + 1;
                }
                dt.Columns["RANK"].SetOrdinal(0);//定義欄位順序

                dt.Columns["RANK"].ColumnName = "項次";
                dt.Columns["JIANJA"].ColumnName = "鑑價承辦人";
                dt.Columns["OVC_DOPEN"].ColumnName = "開標日期";
                dt.Columns["OVC_PUR_NSECTION"].ColumnName = "申購單位";
                dt.Columns["OVC_PURCH"].ColumnName = "購案案號";
                dt.Columns["ONB_GROUP"].ColumnName = "組別";
                dt.Columns["OVC_PURCH_6"].ColumnName = "契約尾號";
                dt.Columns["OVC_PUR_IPURCH"].ColumnName = "購案名稱";
                dt.Columns["OVC_PUR_ASS_VEN_CODE"].ColumnName = "招標方式";
                dt.Columns["ONB_TIMES"].ColumnName = "開標次數";
                dt.Columns["OVC_PUR_APPROVE_DEP"].ColumnName = "核定權責";
                dt.Columns["OVC_CURRENT"].ColumnName = "預算幣別";
                dt.Columns["ONB_BUDGET"].ColumnName = "預算金額";
                dt.Columns["OVC_BID_CURRENT"].ColumnName = "底價幣別";
                dt.Columns["ONB_BID_BUDGET"].ColumnName = "核定底價";
                dt.Columns["OVC_RESULT_CURRENT"].ColumnName = "決標幣別";
                dt.Columns["ONB_BID_RESULT"].ColumnName = "決標金額";
                dt.Columns["OVC_REMAIN_CURRENT"].ColumnName = "標餘款幣別";
                dt.Columns["ONB_REMAIN_BUDGET"].ColumnName = "標餘款";
                dt.Columns["PERSENT_BID"].ColumnName = "底價決標價";
                dt.Columns["OVC_VEN_TITLE"].ColumnName = "得標商名稱";
                dt.Columns["ONB_BID_VENDORS"].ColumnName = "投標廠商家數";






                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                string SheetName = "StatisticAction";
                ws = wb.CreateSheet(SheetName);
                MemoryStream ms = new MemoryStream();

                //設定Title樣式
                HSSFCellStyle title = (HSSFCellStyle)wb.CreateCellStyle();
                //邊框全細線
                title.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                title.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                title.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                title.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;

                HSSFFont titlefont = (HSSFFont)wb.CreateFont();
                //粗體
                titlefont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                title.SetFont(titlefont);
                //第一欄標題
                ws.CreateRow(0).CreateCell(0).SetCellValue(year + "年度未完成決標簽約購案統計表");
                for (int i = 1; i <= 20; i++)
                {
                    ws.GetRow(0).CreateCell(i);
                }
                ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 20));
                string timenow = "";
                timenow = DateTime.Now.ToString("yyyy-MM-dd");
                //第二欄資料
                ws.CreateRow(1).CreateCell(0).SetCellValue("印表日期:" + timenow);
                for (int i = 1; i <= 20; i++)
                {
                    ws.GetRow(1).CreateCell(i);
                }
                ws.AddMergedRegion(new CellRangeAddress(1, 1, 0, 20));
                for (int i = 0; i <= 1; i++)
                {
                    for (int j = 0; j <= 20; j++)
                    {
                        ws.GetRow(i).GetCell(j).CellStyle = title;
                    }
                }
                //第三行為欄位名稱
                ws.CreateRow(2);
                HSSFCellStyle header = (HSSFCellStyle)wb.CreateCellStyle();
                header.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                header.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                header.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                header.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                header.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                header.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ws.GetRow(2).CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                    ws.GetRow(2).GetCell(i).CellStyle = header;
                }

                //第四行開始為資料列
                HSSFCellStyle data = (HSSFCellStyle)wb.CreateCellStyle();
                data.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                data.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                data.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                data.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ws.CreateRow(i + 3);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ws.GetRow(i + 3).CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                        ws.GetRow(i + 3).GetCell(j).CellStyle = data;
                    }

                }
                //設定欄位寬度
                ws.SetColumnWidth(0, 5 * 256);
                ws.SetColumnWidth(1, 12 * 256);
                ws.SetColumnWidth(2, 30 * 256);
                ws.SetColumnWidth(3, 10 * 256);
                ws.SetColumnWidth(4, 5 * 256);
                ws.SetColumnWidth(5, 5 * 256);
                ws.SetColumnWidth(6, 40 * 256);
                ws.SetColumnWidth(7, 10 * 256);
                ws.SetColumnWidth(8, 5 * 256);
                ws.SetColumnWidth(9, 5 * 256);
                ws.SetColumnWidth(10, 5 * 256);
                ws.SetColumnWidth(11, 10 * 256);
                ws.SetColumnWidth(12, 5 * 256);
                ws.SetColumnWidth(13, 10 * 256);
                ws.SetColumnWidth(14, 5 * 256);
                ws.SetColumnWidth(15, 10 * 256);
                ws.SetColumnWidth(16, 5 * 256);
                ws.SetColumnWidth(17, 10 * 256);
                ws.SetColumnWidth(18, 5 * 256);
                ws.SetColumnWidth(19, 30 * 256);
                ws.SetColumnWidth(20, 5 * 256);
                wb.Write(ms);
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename=開標預劃期程表.xls"));
                Response.BinaryWrite(ms.ToArray());
                wb = null;
                ms.Close();
                ms.Dispose();

            }
        }

        protected void btnQuery6_Click(object sender, EventArgs e)
        {
            int num = 0;
            string[] strArray3 = new string[] { "L", "W" };
            if (txtNow.Text == "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "[年度]欄位必填(2碼)");
            else if (int.TryParse(txtNow.Text, out num) == false)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "[年度]請填入2碼數字");
            else
            {
                int TAtoTZ_1 = 0, PAtoPZ_1 = 0, EAtoEZ_1 = 0, FAtoFZandGAtoGZ_1 = 0, HA_1 = 0, HB_1 = 0, AG_1 = 0, JAtoJZ_1 = 0, LAtoLZ_1 = 0, XAtoXZandYAtoYZandBAtoBZ_1 = 0, HI_1 = 0, HP_1 = 0, HK_1 = 0, other_1 = 0,
                    TAtoTZ_2 = 0, PAtoPZ_2 = 0, EAtoEZ_2 = 0, FAtoFZandGAtoGZ_2 = 0, HA_2 = 0, HB_2 = 0, AG_2 = 0, JAtoJZ_2 = 0, LAtoLZ_2 = 0, XAtoXZandYAtoYZandBAtoBZ_2 = 0, HI_2 = 0, HP_2 = 0, HK_2 = 0, other_2 = 0,
                    TAtoTZ_3 = 0, PAtoPZ_3 = 0, EAtoEZ_3 = 0, FAtoFZandGAtoGZ_3 = 0, HA_3 = 0, HB_3 = 0, AG_3 = 0, JAtoJZ_3 = 0, LAtoLZ_3 = 0, XAtoXZandYAtoYZandBAtoBZ_3 = 0, HI_3 = 0, HP_3 = 0, HK_3 = 0, other_3 = 0,
                    total_1 = 0, total_2 = 0, total_3 = 0, total_4 = 0;
                string TAtoTZ_4 = "", PAtoPZ_4 = "", EAtoEZ_4 = "", FAtoFZandGAtoGZ_4 = "", HA_4 = "", HB_4 = "", AG_4 = "", JAtoJZ_4 = "", LAtoLZ_4 = "", XAtoXZandYAtoYZandBAtoBZ_4 = "", HI_4 = "", HP_4 = "", HK_4 = "", other_4 = "";

                string year = txtNow.Text;
                DataTable dt = new DataTable();
                var query =
                    (from TBM1202 in MPMS.TBM1202.DefaultIfEmpty().AsEnumerable()
                     where TBM1202.OVC_CHECK_UNIT.Equals("0A100") || TBM1202.OVC_CHECK_UNIT.Equals("00N00")
                     where TBM1202.OVC_PURCH.Substring(2, 2).Equals(year)
                     select new
                     {
                         OVC_PURCH = TBM1202.OVC_PURCH,
                     }).Distinct();
                if (query.ToList().Count == 0)
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "尚無資料，故無法產生統計表!");
                    return;
                }
                var query1 =
                    from TBM1202_query in query.DefaultIfEmpty().AsEnumerable()
                    join TBM1301_PLAN in GM.TBM1301_PLAN.DefaultIfEmpty().AsEnumerable() on TBM1202_query.OVC_PURCH equals TBM1301_PLAN.OVC_PURCH
                    where TBM1301_PLAN.OVC_PUR_APPROVE_DEP.Equals("A")
                    where strArray3.Contains(TBM1301_PLAN.OVC_PUR_AGENCY)
                    where TBM1301_PLAN.OVC_PUR_IPURCH != null
                    orderby TBM1301_PLAN.OVC_PURCH ascending
                    select new
                    {
                        OVC_PURCH_1 = TBM1301_PLAN.OVC_PURCH.Substring(0, 2)
                    };
                string[] strArray4 = new string[]
                {
                    "TA","TB","TC","TD","TE","TF","TG","TH","TI","TJ","TK","TL","TM","TN","TO","TP","TQ","TR","TS","TT","TU","TV","TW","TX","TY","TZ",
                    "PA","PB","PC","PD","PE","PF","PG","PH","PI","PJ","PK","PL","PM","PN","PO","PP","PQ","PR","PS","PT","PU","PV","PW","PX","PY","PZ",
                    "EA","EB","EC","ED","EE","EF","EG","EH","EI","EJ","EK","EL","EM","EN","EO","EP","EQ","ER","ES","ET","EU","EV","EW","EX","EY","EZ",
                    "FA","FB","FC","FD","FE","FF","FG","FH","FI","FJ","FK","FL","FM","FN","FO","FP","FQ","FR","FS","FT","FU","FV","FW","FX","FY","FZ",
                    "GA","GB","GC","GD","GE","GF","GG","GH","GI","GJ","GK","GL","GM","GN","GO","GP","GQ","GR","GS","GT","GU","GV","GW","GX","GY","GZ",
                    "HA",
                    "HB",
                    "AG",
                    "JA","JB","JC","JD","JE","JF","JG","JH","JI","JJ","JK","JL","JM","JN","JO","JP","JQ","JR","JS","JT","JU","JV","JW","JX","JY","JZ",
                    "LA","LB","LC","LD","LE","LF","LG","LH","LI","LJ","LK","LL","LM","LN","LO","LP","LQ","LR","LS","LT","LU","LV","LW","LX","LY","LZ",
                    "XA","XB","XC","XD","XE","XF","XG","XH","XI","XJ","XK","XL","XM","XN","XO","XP","XQ","XR","XS","XT","XU","XV","XW","XX","XY","XZ",
                    "YA","YB","YC","YD","YE","YF","YG","YH","YI","YJ","YK","YL","YM","YN","YO","YP","YQ","YR","YS","YT","YU","YV","YW","YX","YY","YZ",
                    "BA","BB","BC","BD","BE","BF","BG","BH","BI","BJ","BK","BL","BM","BN","BO","BP","BQ","BR","BS","BT","BU","BV","BW","BX","BY","BZ",
                    "HI",
                    "HP",
                    "HK",
                    "AA","AB","AC","AD","AE","AI","AK","AM","AO","AP","AQ","AR",
                    "CA","CB","CC","CD","CE","CF","CG","CH","CJ","NA"
                };
                query1 = query1.Where(table => strArray4.Contains(table.OVC_PURCH_1));
                dt = CommonStatic.LinqQueryToDataTable(query1);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    switch (dt.Rows[i][0])
                    {
                        case "TA":
                        case "TB":
                        case "TC":
                        case "TD":
                        case "TE":
                        case "TF":
                        case "TG":
                        case "TH":
                        case "TI":
                        case "TJ":
                        case "TK":
                        case "TL":
                        case "TM":
                        case "TN":
                        case "TO":
                        case "TP":
                        case "TQ":
                        case "TR":
                        case "TS":
                        case "TT":
                        case "TU":
                        case "TV":
                        case "TW":
                        case "TX":
                        case "TY":
                        case "TZ":
                            TAtoTZ_1 += 1;
                            total_1 += 1;
                            break;
                        case "PA":
                        case "PB":
                        case "PC":
                        case "PD":
                        case "PE":
                        case "PF":
                        case "PG":
                        case "PH":
                        case "PI":
                        case "PJ":
                        case "PK":
                        case "PL":
                        case "PM":
                        case "PN":
                        case "PO":
                        case "PP":
                        case "PQ":
                        case "PR":
                        case "PS":
                        case "PT":
                        case "PU":
                        case "PV":
                        case "PW":
                        case "PX":
                        case "PY":
                        case "PZ":
                            PAtoPZ_1 += 1;
                            total_1 += 1;
                            break;
                        case "EA":
                        case "EB":
                        case "EC":
                        case "ED":
                        case "EE":
                        case "EF":
                        case "EG":
                        case "EH":
                        case "EI":
                        case "EJ":
                        case "EK":
                        case "EL":
                        case "EM":
                        case "EN":
                        case "EO":
                        case "EP":
                        case "EQ":
                        case "ER":
                        case "ES":
                        case "ET":
                        case "EU":
                        case "EV":
                        case "EW":
                        case "EX":
                        case "EY":
                        case "EZ":
                            EAtoEZ_1 += 1;
                            total_1 += 1;
                            break;
                        case "FA":
                        case "FB":
                        case "FC":
                        case "FD":
                        case "FE":
                        case "FF":
                        case "FG":
                        case "FH":
                        case "FI":
                        case "FJ":
                        case "FK":
                        case "FL":
                        case "FM":
                        case "FN":
                        case "FO":
                        case "FP":
                        case "FQ":
                        case "FR":
                        case "FS":
                        case "FT":
                        case "FU":
                        case "FV":
                        case "FW":
                        case "FX":
                        case "FY":
                        case "FZ":
                        case "GA":
                        case "GB":
                        case "GC":
                        case "GD":
                        case "GE":
                        case "GF":
                        case "GG":
                        case "GH":
                        case "GI":
                        case "GJ":
                        case "GK":
                        case "GL":
                        case "GM":
                        case "GN":
                        case "GO":
                        case "GP":
                        case "GQ":
                        case "GR":
                        case "GS":
                        case "GT":
                        case "GU":
                        case "GV":
                        case "GW":
                        case "GX":
                        case "GY":
                        case "GZ":
                            FAtoFZandGAtoGZ_1 += 1;
                            total_1 += 1;
                            break;
                        case "HA":
                            HA_1 +=1;
                            total_1 += 1;
                            break;
                        case "HB":
                            HB_1 += 1;
                            total_1 += 1;
                            break;
                        case "AG":
                            AG_1 += 1;
                            total_1 += 1;
                            break;
                        case "JA":
                        case "JB":
                        case "JC":
                        case "JD":
                        case "JE":
                        case "JF":
                        case "JG":
                        case "JH":
                        case "JI":
                        case "JJ":
                        case "JK":
                        case "JL":
                        case "JM":
                        case "JN":
                        case "JO":
                        case "JP":
                        case "JQ":
                        case "JR":
                        case "JS":
                        case "JT":
                        case "JU":
                        case "JV":
                        case "JW":
                        case "JX":
                        case "JY":
                        case "JZ":
                            JAtoJZ_1 += 1;
                            total_1 += 1;
                            break;
                        case "LA":
                        case "LB":
                        case "LC":
                        case "LD":
                        case "LE":
                        case "LF":
                        case "LG":
                        case "LH":
                        case "LI":
                        case "LJ":
                        case "LK":
                        case "LL":
                        case "LM":
                        case "LN":
                        case "LO":
                        case "LP":
                        case "LQ":
                        case "LR":
                        case "LS":
                        case "LT":
                        case "LU":
                        case "LV":
                        case "LW":
                        case "LX":
                        case "LY":
                        case "LZ":
                            LAtoLZ_1 += 1;
                            total_1 += 1;
                            break;
                        case "XA":
                        case "XB":
                        case "XC":
                        case "XD":
                        case "XE":
                        case "XF":
                        case "XG":
                        case "XH":
                        case "XI":
                        case "XJ":
                        case "XK":
                        case "XL":
                        case "XM":
                        case "XN":
                        case "XO":
                        case "XP":
                        case "XQ":
                        case "XR":
                        case "XS":
                        case "XT":
                        case "XU":
                        case "XV":
                        case "XW":
                        case "XX":
                        case "XY":
                        case "XZ":
                        case "YA":
                        case "YB":
                        case "YC":
                        case "YD":
                        case "YE":
                        case "YF":
                        case "YG":
                        case "YH":
                        case "YI":
                        case "YJ":
                        case "YK":
                        case "YL":
                        case "YM":
                        case "YN":
                        case "YO":
                        case "YP":
                        case "YQ":
                        case "YR":
                        case "YS":
                        case "YT":
                        case "YU":
                        case "YV":
                        case "YW":
                        case "YX":
                        case "YY":
                        case "YZ":
                        case "BA":
                        case "BB":
                        case "BC":
                        case "BD":
                        case "BE":
                        case "BF":
                        case "BG":
                        case "BH":
                        case "BI":
                        case "BJ":
                        case "BK":
                        case "BL":
                        case "BM":
                        case "BN":
                        case "BO":
                        case "BP":
                        case "BQ":
                        case "BR":
                        case "BS":
                        case "BT":
                        case "BU":
                        case "BV":
                        case "BW":
                        case "BX":
                        case "BY":
                        case "BZ":
                            XAtoXZandYAtoYZandBAtoBZ_1 += 1;
                            total_1 += 1;
                            break;
                        case "HI":
                            HI_1 += 1;
                            total_1 += 1;
                            break;
                        case "HP":
                            HP_1 += 1;
                            total_1 += 1;
                            break;
                        case "HK":
                            HK_1 += 1;
                            total_1 += 1;
                            break;
                        default:
                            other_1 += 1;
                            total_1 += 1;
                            break;
                    }
                }
                DataTable dt1= new DataTable();
                var query2 =
                    (from TBM1202 in MPMS.TBM1202.DefaultIfEmpty().AsEnumerable()
                     where TBM1202.OVC_CHECK_UNIT.Equals("0A100") || TBM1202.OVC_CHECK_UNIT.Equals("00N00")
                     where TBM1202.OVC_PURCH.Substring(2, 2).Equals(year)
                     select new
                     {
                         OVC_PURCH = TBM1202.OVC_PURCH,
                     }).Distinct();
                var query3 =
                    (from TBM1303 in MPMS.TBM1303.DefaultIfEmpty().AsEnumerable()
                     where TBM1303.OVC_RESULT != null
                     where TBM1303.ONB_BID_RESULT != null
                     select new
                     {
                         OVC_PURCH = TBM1303.OVC_PURCH,
                         OVC_RESULT = TBM1303.OVC_RESULT,
                         ONB_BID_RESULT = TBM1303.ONB_BID_RESULT
                     }).Distinct();
                query3 = query3.Where(table => table.OVC_RESULT.Equals("0"));
                query3 = query3.Where(table => table.ONB_BID_RESULT > 0);
                var query4 =
                    from TBM1202_query in query2.DefaultIfEmpty().AsEnumerable()
                    join TBM1301_PLAN in GM.TBM1301_PLAN.DefaultIfEmpty().AsEnumerable() on TBM1202_query.OVC_PURCH equals TBM1301_PLAN.OVC_PURCH
                    join TBM1303_query in query3.DefaultIfEmpty().AsEnumerable() on TBM1202_query.OVC_PURCH equals TBM1303_query.OVC_PURCH
                    where TBM1301_PLAN.OVC_PUR_APPROVE_DEP.Equals("A")
                    where strArray3.Contains(TBM1301_PLAN.OVC_PUR_AGENCY)
                    where TBM1301_PLAN.OVC_PUR_IPURCH != null
                    orderby TBM1301_PLAN.OVC_PURCH ascending
                    select new
                    {
                        OVC_PURCH_1 = TBM1301_PLAN.OVC_PURCH.Substring(0, 2),
                    };
                query4 = query4.Where(table => strArray4.Contains(table.OVC_PURCH_1));

                dt1 = CommonStatic.LinqQueryToDataTable(query4);

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    switch (dt1.Rows[i][0])
                    {
                        case "TA":
                        case "TB":
                        case "TC":
                        case "TD":
                        case "TE":
                        case "TF":
                        case "TG":
                        case "TH":
                        case "TI":
                        case "TJ":
                        case "TK":
                        case "TL":
                        case "TM":
                        case "TN":
                        case "TO":
                        case "TP":
                        case "TQ":
                        case "TR":
                        case "TS":
                        case "TT":
                        case "TU":
                        case "TV":
                        case "TW":
                        case "TX":
                        case "TY":
                        case "TZ":
                            TAtoTZ_2 += 1;
                            total_2 += 1;
                            break;
                        case "PA":
                        case "PB":
                        case "PC":
                        case "PD":
                        case "PE":
                        case "PF":
                        case "PG":
                        case "PH":
                        case "PI":
                        case "PJ":
                        case "PK":
                        case "PL":
                        case "PM":
                        case "PN":
                        case "PO":
                        case "PP":
                        case "PQ":
                        case "PR":
                        case "PS":
                        case "PT":
                        case "PU":
                        case "PV":
                        case "PW":
                        case "PX":
                        case "PY":
                        case "PZ":
                            PAtoPZ_2 += 1;
                            total_2 += 1;
                            break;
                        case "EA":
                        case "EB":
                        case "EC":
                        case "ED":
                        case "EE":
                        case "EF":
                        case "EG":
                        case "EH":
                        case "EI":
                        case "EJ":
                        case "EK":
                        case "EL":
                        case "EM":
                        case "EN":
                        case "EO":
                        case "EP":
                        case "EQ":
                        case "ER":
                        case "ES":
                        case "ET":
                        case "EU":
                        case "EV":
                        case "EW":
                        case "EX":
                        case "EY":
                        case "EZ":
                            EAtoEZ_2 += 1;
                            total_2 += 1;
                            break;
                        case "FA":
                        case "FB":
                        case "FC":
                        case "FD":
                        case "FE":
                        case "FF":
                        case "FG":
                        case "FH":
                        case "FI":
                        case "FJ":
                        case "FK":
                        case "FL":
                        case "FM":
                        case "FN":
                        case "FO":
                        case "FP":
                        case "FQ":
                        case "FR":
                        case "FS":
                        case "FT":
                        case "FU":
                        case "FV":
                        case "FW":
                        case "FX":
                        case "FY":
                        case "FZ":
                        case "GA":
                        case "GB":
                        case "GC":
                        case "GD":
                        case "GE":
                        case "GF":
                        case "GG":
                        case "GH":
                        case "GI":
                        case "GJ":
                        case "GK":
                        case "GL":
                        case "GM":
                        case "GN":
                        case "GO":
                        case "GP":
                        case "GQ":
                        case "GR":
                        case "GS":
                        case "GT":
                        case "GU":
                        case "GV":
                        case "GW":
                        case "GX":
                        case "GY":
                        case "GZ":
                            FAtoFZandGAtoGZ_2 += 1;
                            total_2 += 1;
                            break;
                        case "HA":
                            HA_2 += 1;
                            total_2 += 1;
                            break;
                        case "HB":
                            HB_2 += 1;
                            total_2 += 1;
                            break;
                        case "AG":
                            AG_2 += 1;
                            total_2 += 1;
                            break;
                        case "JA":
                        case "JB":
                        case "JC":
                        case "JD":
                        case "JE":
                        case "JF":
                        case "JG":
                        case "JH":
                        case "JI":
                        case "JJ":
                        case "JK":
                        case "JL":
                        case "JM":
                        case "JN":
                        case "JO":
                        case "JP":
                        case "JQ":
                        case "JR":
                        case "JS":
                        case "JT":
                        case "JU":
                        case "JV":
                        case "JW":
                        case "JX":
                        case "JY":
                        case "JZ":
                            JAtoJZ_2 += 1;
                            total_2 += 1;
                            break;
                        case "LA":
                        case "LB":
                        case "LC":
                        case "LD":
                        case "LE":
                        case "LF":
                        case "LG":
                        case "LH":
                        case "LI":
                        case "LJ":
                        case "LK":
                        case "LL":
                        case "LM":
                        case "LN":
                        case "LO":
                        case "LP":
                        case "LQ":
                        case "LR":
                        case "LS":
                        case "LT":
                        case "LU":
                        case "LV":
                        case "LW":
                        case "LX":
                        case "LY":
                        case "LZ":
                            LAtoLZ_2 += 1;
                            total_2 += 1;
                            break;
                        case "XA":
                        case "XB":
                        case "XC":
                        case "XD":
                        case "XE":
                        case "XF":
                        case "XG":
                        case "XH":
                        case "XI":
                        case "XJ":
                        case "XK":
                        case "XL":
                        case "XM":
                        case "XN":
                        case "XO":
                        case "XP":
                        case "XQ":
                        case "XR":
                        case "XS":
                        case "XT":
                        case "XU":
                        case "XV":
                        case "XW":
                        case "XX":
                        case "XY":
                        case "XZ":
                        case "YA":
                        case "YB":
                        case "YC":
                        case "YD":
                        case "YE":
                        case "YF":
                        case "YG":
                        case "YH":
                        case "YI":
                        case "YJ":
                        case "YK":
                        case "YL":
                        case "YM":
                        case "YN":
                        case "YO":
                        case "YP":
                        case "YQ":
                        case "YR":
                        case "YS":
                        case "YT":
                        case "YU":
                        case "YV":
                        case "YW":
                        case "YX":
                        case "YY":
                        case "YZ":
                        case "BA":
                        case "BB":
                        case "BC":
                        case "BD":
                        case "BE":
                        case "BF":
                        case "BG":
                        case "BH":
                        case "BI":
                        case "BJ":
                        case "BK":
                        case "BL":
                        case "BM":
                        case "BN":
                        case "BO":
                        case "BP":
                        case "BQ":
                        case "BR":
                        case "BS":
                        case "BT":
                        case "BU":
                        case "BV":
                        case "BW":
                        case "BX":
                        case "BY":
                        case "BZ":
                            XAtoXZandYAtoYZandBAtoBZ_2 += 1;
                            total_2 += 1;
                            break;
                        case "HI":
                            HI_2 += 1;
                            total_2 += 1;
                            break;
                        case "HP":
                            HP_2 += 1;
                            total_2 += 1;
                            break;
                        case "HK":
                            HK_2 += 1;
                            total_2 += 1;
                            break;
                        default:
                            other_2 += 1;
                            total_2 += 1;
                            break;
                    }
                }

                DataTable dt2 = new DataTable();
                var query5 =
                    (from TBM1202 in MPMS.TBM1202.DefaultIfEmpty().AsEnumerable()
                     where TBM1202.OVC_CHECK_UNIT.Equals("0A100") || TBM1202.OVC_CHECK_UNIT.Equals("00N00")
                     where TBM1202.OVC_PURCH.Substring(2, 2).Equals(year)
                     select new
                     {
                         OVC_PURCH = TBM1202.OVC_PURCH,
                     }).Distinct();
                var query6 =
                    from TBMRECEIVE_CONTRACT in MPMS.TBMRECEIVE_CONTRACT.DefaultIfEmpty().AsEnumerable()
                    where TBMRECEIVE_CONTRACT.OVC_DO_NAME != null
                    select new
                    {
                        OVC_PURCH = TBMRECEIVE_CONTRACT.OVC_PURCH,
                        OVC_PURCH_6 = TBMRECEIVE_CONTRACT.OVC_PURCH_6
                    };
                var query7 =
                    from TBM1202_query in query5.DefaultIfEmpty().AsEnumerable()
                    join TBM1301_PLAN in GM.TBM1301_PLAN.DefaultIfEmpty().AsEnumerable() on TBM1202_query.OVC_PURCH equals TBM1301_PLAN.OVC_PURCH
                    join TBMRECEIVE_CONTRACT_query in query6.DefaultIfEmpty().AsEnumerable() on TBM1202_query.OVC_PURCH equals TBMRECEIVE_CONTRACT_query.OVC_PURCH
                    where TBM1301_PLAN.OVC_PUR_APPROVE_DEP.Equals("A")
                    where strArray3.Contains(TBM1301_PLAN.OVC_PUR_AGENCY)
                    where TBM1301_PLAN.OVC_PUR_IPURCH != null
                    orderby TBM1301_PLAN.OVC_PURCH ascending
                    select new
                    {
                        OVC_PURCH_1 = TBM1301_PLAN.OVC_PURCH.Substring(0, 2),
                    };
                query7 = query7.Where(table => strArray4.Contains(table.OVC_PURCH_1));
                dt2 = CommonStatic.LinqQueryToDataTable(query7);

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    switch (dt2.Rows[i][0])
                    {
                        case "TA":
                        case "TB":
                        case "TC":
                        case "TD":
                        case "TE":
                        case "TF":
                        case "TG":
                        case "TH":
                        case "TI":
                        case "TJ":
                        case "TK":
                        case "TL":
                        case "TM":
                        case "TN":
                        case "TO":
                        case "TP":
                        case "TQ":
                        case "TR":
                        case "TS":
                        case "TT":
                        case "TU":
                        case "TV":
                        case "TW":
                        case "TX":
                        case "TY":
                        case "TZ":
                            TAtoTZ_3 += 1;
                            total_3 += 1;
                            break;
                        case "PA":
                        case "PB":
                        case "PC":
                        case "PD":
                        case "PE":
                        case "PF":
                        case "PG":
                        case "PH":
                        case "PI":
                        case "PJ":
                        case "PK":
                        case "PL":
                        case "PM":
                        case "PN":
                        case "PO":
                        case "PP":
                        case "PQ":
                        case "PR":
                        case "PS":
                        case "PT":
                        case "PU":
                        case "PV":
                        case "PW":
                        case "PX":
                        case "PY":
                        case "PZ":
                            PAtoPZ_3 += 1;
                            total_3 += 1;
                            break;
                        case "EA":
                        case "EB":
                        case "EC":
                        case "ED":
                        case "EE":
                        case "EF":
                        case "EG":
                        case "EH":
                        case "EI":
                        case "EJ":
                        case "EK":
                        case "EL":
                        case "EM":
                        case "EN":
                        case "EO":
                        case "EP":
                        case "EQ":
                        case "ER":
                        case "ES":
                        case "ET":
                        case "EU":
                        case "EV":
                        case "EW":
                        case "EX":
                        case "EY":
                        case "EZ":
                            EAtoEZ_3 += 1;
                            total_3 += 1;
                            break;
                        case "FA":
                        case "FB":
                        case "FC":
                        case "FD":
                        case "FE":
                        case "FF":
                        case "FG":
                        case "FH":
                        case "FI":
                        case "FJ":
                        case "FK":
                        case "FL":
                        case "FM":
                        case "FN":
                        case "FO":
                        case "FP":
                        case "FQ":
                        case "FR":
                        case "FS":
                        case "FT":
                        case "FU":
                        case "FV":
                        case "FW":
                        case "FX":
                        case "FY":
                        case "FZ":
                        case "GA":
                        case "GB":
                        case "GC":
                        case "GD":
                        case "GE":
                        case "GF":
                        case "GG":
                        case "GH":
                        case "GI":
                        case "GJ":
                        case "GK":
                        case "GL":
                        case "GM":
                        case "GN":
                        case "GO":
                        case "GP":
                        case "GQ":
                        case "GR":
                        case "GS":
                        case "GT":
                        case "GU":
                        case "GV":
                        case "GW":
                        case "GX":
                        case "GY":
                        case "GZ":
                            FAtoFZandGAtoGZ_3 += 1;
                            total_3 += 1;
                            break;
                        case "HA":
                            HA_3 += 1;
                            total_3 += 1;
                            break;
                        case "HB":
                            HB_3 += 1;
                            total_3 += 1;
                            break;
                        case "AG":
                            AG_3 += 1;
                            total_3 += 1;
                            break;
                        case "JA":
                        case "JB":
                        case "JC":
                        case "JD":
                        case "JE":
                        case "JF":
                        case "JG":
                        case "JH":
                        case "JI":
                        case "JJ":
                        case "JK":
                        case "JL":
                        case "JM":
                        case "JN":
                        case "JO":
                        case "JP":
                        case "JQ":
                        case "JR":
                        case "JS":
                        case "JT":
                        case "JU":
                        case "JV":
                        case "JW":
                        case "JX":
                        case "JY":
                        case "JZ":
                            JAtoJZ_3 += 1;
                            total_3 += 1;
                            break;
                        case "LA":
                        case "LB":
                        case "LC":
                        case "LD":
                        case "LE":
                        case "LF":
                        case "LG":
                        case "LH":
                        case "LI":
                        case "LJ":
                        case "LK":
                        case "LL":
                        case "LM":
                        case "LN":
                        case "LO":
                        case "LP":
                        case "LQ":
                        case "LR":
                        case "LS":
                        case "LT":
                        case "LU":
                        case "LV":
                        case "LW":
                        case "LX":
                        case "LY":
                        case "LZ":
                            LAtoLZ_3 += 1;
                            total_3 += 1;
                            break;
                        case "XA":
                        case "XB":
                        case "XC":
                        case "XD":
                        case "XE":
                        case "XF":
                        case "XG":
                        case "XH":
                        case "XI":
                        case "XJ":
                        case "XK":
                        case "XL":
                        case "XM":
                        case "XN":
                        case "XO":
                        case "XP":
                        case "XQ":
                        case "XR":
                        case "XS":
                        case "XT":
                        case "XU":
                        case "XV":
                        case "XW":
                        case "XX":
                        case "XY":
                        case "XZ":
                        case "YA":
                        case "YB":
                        case "YC":
                        case "YD":
                        case "YE":
                        case "YF":
                        case "YG":
                        case "YH":
                        case "YI":
                        case "YJ":
                        case "YK":
                        case "YL":
                        case "YM":
                        case "YN":
                        case "YO":
                        case "YP":
                        case "YQ":
                        case "YR":
                        case "YS":
                        case "YT":
                        case "YU":
                        case "YV":
                        case "YW":
                        case "YX":
                        case "YY":
                        case "YZ":
                        case "BA":
                        case "BB":
                        case "BC":
                        case "BD":
                        case "BE":
                        case "BF":
                        case "BG":
                        case "BH":
                        case "BI":
                        case "BJ":
                        case "BK":
                        case "BL":
                        case "BM":
                        case "BN":
                        case "BO":
                        case "BP":
                        case "BQ":
                        case "BR":
                        case "BS":
                        case "BT":
                        case "BU":
                        case "BV":
                        case "BW":
                        case "BX":
                        case "BY":
                        case "BZ":
                            XAtoXZandYAtoYZandBAtoBZ_3 += 1;
                            total_3 += 1;
                            break;
                        case "HI":
                            HI_3 += 1;
                            total_3 += 1;
                            break;
                        case "HP":
                            HP_3 += 1;
                            total_3 += 1;
                            break;
                        case "HK":
                            HK_3 += 1;
                            total_3 += 1;
                            break;
                        default:
                            other_3 += 1;
                            total_3 += 1;
                            break;
                    }
                }
                TAtoTZ_4 = TAtoTZ_1 == 0 ? "0" + "%" : Math.Truncate((Convert.ToDecimal(TAtoTZ_2) / Convert.ToDecimal(TAtoTZ_1)) * 100) + "%";
                PAtoPZ_4 = PAtoPZ_1 == 0 ? "0" + "%" : Math.Truncate((Convert.ToDecimal(PAtoPZ_2) / Convert.ToDecimal(PAtoPZ_1)) * 100) + "%";
                EAtoEZ_4 = EAtoEZ_1 == 0 ? "0" + "%" : Math.Truncate((Convert.ToDecimal(EAtoEZ_2) / Convert.ToDecimal(EAtoEZ_1)) * 100) + "%";
                FAtoFZandGAtoGZ_4 = FAtoFZandGAtoGZ_1 == 0 ? "0" + "%" : Math.Truncate((Convert.ToDecimal(FAtoFZandGAtoGZ_2) / Convert.ToDecimal(FAtoFZandGAtoGZ_1)) * 100) + "%";
                HA_4 = HA_1 == 0 ? "0" + "%" : Math.Truncate((Convert.ToDecimal(HA_2) / Convert.ToDecimal(HA_1)) * 100) + "%";
                HB_4 = HB_1 == 0 ? "0" + "%" : Math.Truncate((Convert.ToDecimal(HB_2) / Convert.ToDecimal(HB_1)) * 100) + "%";
                AG_4 = AG_1 == 0 ? "0" + "%" : Math.Truncate((Convert.ToDecimal(AG_2) / Convert.ToDecimal(AG_1)) * 100) + "%";
                JAtoJZ_4 = JAtoJZ_1 == 0 ? "0" + "%" : Math.Truncate((Convert.ToDecimal(JAtoJZ_2) / Convert.ToDecimal(JAtoJZ_1)) * 100) + "%";
                LAtoLZ_4 = LAtoLZ_1 == 0 ? "0" + "%" : Math.Truncate((Convert.ToDecimal(LAtoLZ_2) / Convert.ToDecimal(LAtoLZ_1)) * 100) + "%";
                XAtoXZandYAtoYZandBAtoBZ_4 = XAtoXZandYAtoYZandBAtoBZ_1 == 0 ? "0" + "%" : Math.Truncate((Convert.ToDecimal(XAtoXZandYAtoYZandBAtoBZ_2) / Convert.ToDecimal(XAtoXZandYAtoYZandBAtoBZ_1)) * 100) + "%";
                HI_4 = HI_1 == 0 ? "0" + "%" : Math.Truncate((Convert.ToDecimal(HI_2) / Convert.ToDecimal(HI_1)) * 100) + "%";
                HP_4 = HP_1 == 0 ? "0" + "%" : Math.Truncate((Convert.ToDecimal(HP_2) / Convert.ToDecimal(HP_1)) * 100) + "%";
                HK_4 = HK_1 == 0 ? "0" + "%" : Math.Truncate((Convert.ToDecimal(HK_2) / Convert.ToDecimal(HK_1)) * 100) + "%";
                other_4 = other_1 == 0 ? "0" + "%" : Math.Truncate((Convert.ToDecimal(other_2) / Convert.ToDecimal(other_1)) * 100) + "%";
                txt_TAtoTZ_1.Text = TAtoTZ_1.ToString();
                txt_TAtoTZ_2.Text = TAtoTZ_2.ToString();
                txt_TAtoTZ_3.Text = TAtoTZ_3.ToString();
                txt_TAtoTZ_4.Text = TAtoTZ_4.ToString();
                txt_PAtoPZ_1.Text = PAtoPZ_1.ToString();
                txt_PAtoPZ_2.Text = PAtoPZ_2.ToString();
                txt_PAtoPZ_3.Text = PAtoPZ_3.ToString();
                txt_PAtoPZ_4.Text = PAtoPZ_4.ToString();
                txt_EAtoEZ_1.Text = EAtoEZ_1.ToString();
                txt_EAtoEZ_2.Text = EAtoEZ_2.ToString();
                txt_EAtoEZ_3.Text = EAtoEZ_3.ToString();
                txt_EAtoEZ_4.Text = EAtoEZ_4.ToString();
                txt_FAtoFZandGAtoGZ_1.Text = FAtoFZandGAtoGZ_1.ToString();
                txt_FAtoFZandGAtoGZ_2.Text = FAtoFZandGAtoGZ_2.ToString();
                txt_FAtoFZandGAtoGZ_3.Text = FAtoFZandGAtoGZ_3.ToString();
                txt_FAtoFZandGAtoGZ_4.Text = FAtoFZandGAtoGZ_4.ToString();
                txt_HA_1.Text = HA_1.ToString();
                txt_HA_2.Text = HA_2.ToString();
                txt_HA_3.Text = HA_3.ToString();
                txt_HA_4.Text = HA_4.ToString();
                txt_HB_1.Text = HB_1.ToString();
                txt_HB_2.Text = HB_2.ToString();
                txt_HB_3.Text = HB_3.ToString();
                txt_HB_4.Text = HB_4.ToString();
                txt_AG_1.Text = AG_1.ToString();
                txt_AG_2.Text = AG_2.ToString();
                txt_AG_3.Text = AG_3.ToString();
                txt_AG_4.Text = AG_4.ToString();
                txt_JAtoJZ_1.Text = JAtoJZ_1.ToString();
                txt_JAtoJZ_2.Text = JAtoJZ_2.ToString();
                txt_JAtoJZ_3.Text = JAtoJZ_3.ToString();
                txt_JAtoJZ_4.Text = JAtoJZ_4.ToString();
                txt_LAtoLZ_1.Text = LAtoLZ_1.ToString();
                txt_LAtoLZ_2.Text = LAtoLZ_2.ToString();
                txt_LAtoLZ_3.Text = LAtoLZ_3.ToString();
                txt_LAtoLZ_4.Text = LAtoLZ_4.ToString();
                txt_XAtoXZandYAtoYZandBAtoBZ_1.Text = XAtoXZandYAtoYZandBAtoBZ_1.ToString();
                txt_XAtoXZandYAtoYZandBAtoBZ_2.Text = XAtoXZandYAtoYZandBAtoBZ_2.ToString();
                txt_XAtoXZandYAtoYZandBAtoBZ_3.Text = XAtoXZandYAtoYZandBAtoBZ_3.ToString();
                txt_XAtoXZandYAtoYZandBAtoBZ_4.Text = XAtoXZandYAtoYZandBAtoBZ_4.ToString();
                txt_HI_1.Text = HI_1.ToString();
                txt_HI_2.Text = HI_2.ToString();
                txt_HI_3.Text = HI_3.ToString();
                txt_HI_4.Text = HI_4.ToString();
                txt_HP_1.Text = HP_1.ToString();
                txt_HP_2.Text = HP_2.ToString();
                txt_HP_3.Text = HP_3.ToString();
                txt_HP_4.Text = HP_4.ToString();
                txt_HK_1.Text = HK_1.ToString();
                txt_HK_2.Text = HK_2.ToString();
                txt_HK_3.Text = HK_3.ToString();
                txt_HK_4.Text = HK_4.ToString();
                txt_other_1.Text = other_1.ToString();
                txt_other_2.Text = other_2.ToString();
                txt_other_3.Text = other_3.ToString();
                txt_other_4.Text = other_4.ToString();
                txttotal_1.Text = total_1.ToString();
                txttotal_2.Text = total_2.ToString();
                txttotal_3.Text = total_3.ToString();
                txttotal_4.Text = total_1 == 0 ? "0" + "%" : Math.Truncate((Convert.ToDecimal(total_2) / Convert.ToDecimal(total_1)) * 100) + "%";
                Title1.Text = year + "年度採購中心收辦購案執行現況";

                Panel1.Visible = false;
                Panel2.Visible = true;
            }
        }

        protected void btnRETURN_Click(object sender, EventArgs e)
        {
            Panel1.Visible = true;
            Panel2.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Request.UrlReferrer.ToString().Contains("G10"))
            //{
            //    Response.Write("<script>alert('系統檢測到您未依照正確方式進入此頁面，將導至登入畫面!'); location.href='../../../logout.aspx'; </script>");
            //    return;
            //}
            FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
        }
    }
}