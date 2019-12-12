using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.CIMSModel;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;
using System.IO;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace FCFDFE.pages.CIMS.G
{
    public partial class CIMS_G11 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        CIMSEntities CIMS = new CIMSEntities();
        GMEntities GM = new GMEntities();
        MPMSEntities MPMS = new MPMSEntities();

        protected void btnQuery1_Click(object sender, EventArgs e)
        {
			if (txtQuery1_s.Text == "" || txtQuery1_e.Text == "")
				FCommon.AlertShow(PnMessage, "danger", "系統訊息", "核定日期起訖皆為必選欄位!");
			else
			{
				DateTime dateValue;
				DataTable dt = new DataTable();
				var query = from TBM1301 in GM.TBM1301.DefaultIfEmpty().AsEnumerable()
							join TBMRECEIVE_BID in MPMS.TBMRECEIVE_BID.DefaultIfEmpty().AsEnumerable() on TBM1301.OVC_PURCH equals TBMRECEIVE_BID.OVC_PURCH
							join TBMRECEIVE_WORK in MPMS.TBMRECEIVE_WORK.DefaultIfEmpty().AsEnumerable() on TBM1301.OVC_PURCH equals TBMRECEIVE_WORK.OVC_PURCH
							orderby TBM1301.OVC_PURCH ascending
							select new
							{
								OVC_PURCH = TBM1301.OVC_PURCH ?? "",
								OVC_PUR_AGENCY = TBM1301.OVC_PUR_AGENCY ?? "",
								OVC_PURCH_5 = TBMRECEIVE_BID.OVC_PURCH_5 ?? "",
								OVC_PUR_IPURCH = TBM1301.OVC_PUR_IPURCH ?? "",
								OVC_PUR_DAPPROVE = DateTime.TryParse(TBM1301.OVC_PUR_DAPPROVE, out dateValue) == false ? "" : TBM1301.OVC_PUR_DAPPROVE,
								OVC_DANNOUNCE = TBMRECEIVE_WORK.OVC_DANNOUNCE ?? "",
								OVC_PUR_APPROVE = TBM1301.OVC_PUR_APPROVE ?? "",
								OVC_DOPEN = TBMRECEIVE_WORK.OVC_DOPEN ?? "",
								ONB_TIMES = TBMRECEIVE_WORK.ONB_TIMES,
								OVC_PUR_ASS_VEN_CODE = TBMRECEIVE_WORK.OVC_PUR_ASS_VEN_CODE ?? "",
								OVC_DO_NAME = TBMRECEIVE_BID.OVC_DO_NAME ?? ""
							};
				query = query.Where(table => !table.OVC_PUR_DAPPROVE.Equals(""));
				query = query.Where(table => DateTime.Compare(Convert.ToDateTime(table.OVC_PUR_DAPPROVE), Convert.ToDateTime(txtQuery1_s.Text)) >= 0);
				query = query.Where(table => DateTime.Compare(Convert.ToDateTime(table.OVC_PUR_DAPPROVE), Convert.ToDateTime(txtQuery1_e.Text)) <= 0);

				dt = CommonStatic.LinqQueryToDataTable(query);
				DataColumn column = new DataColumn();
				column.ColumnName = "RANK";
				column.DataType = System.Type.GetType("System.Int32");
				dt.Columns.Add(column);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Rank"] = i + 1;
                    }
                    dt.Columns["RANK"].SetOrdinal(0);//定義欄位順序
                    dt.Columns["RANK"].ColumnName = "項次";
                    dt.Columns["OVC_PURCH"].ColumnName = "購案編號";
                    dt.Columns["OVC_PUR_AGENCY"].ColumnName = "第四組";
                    dt.Columns["OVC_PURCH_5"].ColumnName = "第五組";
                    dt.Columns["OVC_PUR_IPURCH"].ColumnName = "購案名稱";
                    dt.Columns["OVC_PUR_DAPPROVE"].ColumnName = "核定日期";
                    dt.Columns["OVC_PUR_APPROVE"].ColumnName = "核定文號";
                    dt.Columns["OVC_DANNOUNCE"].ColumnName = "公告日期";
                    dt.Columns["OVC_DOPEN"].ColumnName = "開標日期";
                    dt.Columns["ONB_TIMES"].ColumnName = "開標次數";
                    dt.Columns["OVC_PUR_ASS_VEN_CODE"].ColumnName = "招標方式";
                    dt.Columns["OVC_DO_NAME"].ColumnName = "購訂承辦人";
                }
                else
                {   //選定範圍沒有資料給預設值                 
                    dt.Columns.Add("OVC_PURCH");
                    dt.Columns.Add("OVC_PUR_AGENCY");
                    dt.Columns.Add("OVC_PURCH_5");
                    dt.Columns.Add("OVC_PUR_IPURCH");
                    dt.Columns.Add("OVC_PUR_DAPPROVE");
                    dt.Columns.Add("OVC_PUR_APPROVE");
                    dt.Columns.Add("OVC_DANNOUNCE");
                    dt.Columns.Add("OVC_DOPEN");
                    dt.Columns.Add("ONB_TIMES");
                    dt.Columns.Add("OVC_PUR_ASS_VEN_CODE");
                    dt.Columns.Add("OVC_DO_NAME");

                    dt.Columns["RANK"].ColumnName = "項次";
                    dt.Columns["OVC_PURCH"].ColumnName = "購案編號";
                    dt.Columns["OVC_PUR_AGENCY"].ColumnName = "第四組";
                    dt.Columns["OVC_PURCH_5"].ColumnName = "第五組";
                    dt.Columns["OVC_PUR_IPURCH"].ColumnName = "購案名稱";
                    dt.Columns["OVC_PUR_DAPPROVE"].ColumnName = "核定日期";
                    dt.Columns["OVC_PUR_APPROVE"].ColumnName = "核定文號";
                    dt.Columns["OVC_DANNOUNCE"].ColumnName = "公告日期";
                    dt.Columns["OVC_DOPEN"].ColumnName = "開標日期";
                    dt.Columns["ONB_TIMES"].ColumnName = "開標次數";
                    dt.Columns["OVC_PUR_ASS_VEN_CODE"].ColumnName = "招標方式";
                    dt.Columns["OVC_DO_NAME"].ColumnName = "購訂承辦人";
                }

                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                string SheetName = "StatisticAction";
                ws = wb.CreateSheet(SheetName);
                MemoryStream ms = new MemoryStream();

                //設定Title樣式
                HSSFCellStyle title =(HSSFCellStyle)wb.CreateCellStyle();
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
                ws.CreateRow(0).CreateCell(0).SetCellValue("核定日期自"+ txtQuery1_s.Text+" 至 "+ txtQuery1_s.Text+"止 排標統計表");
                for (int i = 1; i <= 11; i++)
                {
                    ws.GetRow(0).CreateCell(i);
                }
                ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 11));
                string timenow = "";
                timenow = DateTime.Now.ToString("yyyy-MM-dd");
                //第二欄資料
                ws.CreateRow(1).CreateCell(0).SetCellValue("印表日期:" + timenow);
                for (int i = 1; i <= 11; i++)
                {
                    ws.GetRow(1).CreateCell(i);
                }
                ws.AddMergedRegion(new CellRangeAddress(1, 1, 0, 11));
                for (int i = 0; i <= 1; i++)
                {
                    for (int j = 0; j <= 11; j++)
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
                HSSFCellStyle data  = (HSSFCellStyle)wb.CreateCellStyle();
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
                ws.SetColumnWidth(1, 10 * 256);
                ws.SetColumnWidth(2, 7 * 256);
                ws.SetColumnWidth(3, 7 * 256);
                ws.SetColumnWidth(4, 40 * 256);
                ws.SetColumnWidth(5, 15 * 256);
                ws.SetColumnWidth(6, 15 * 256);
                ws.SetColumnWidth(7, 25 * 256);
                ws.SetColumnWidth(8, 15 * 256);
                ws.SetColumnWidth(9, 5 * 256);
                ws.SetColumnWidth(10, 40 * 256);
                ws.SetColumnWidth(11, 10 * 256);
                wb.Write(ms);
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename=排標統計表.xls"));
                Response.BinaryWrite(ms.ToArray());
                wb = null;
                ms.Close();
                ms.Dispose();
            }
        }

        protected void btnQuery2_Click(object sender, EventArgs e)
        {
            if (txtQuery2_s.Text == "" || txtQuery2_e.Text == "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "公告日期起訖皆為必選欄位!");
            else
            {
                DateTime dateValue;
                DataTable dt = new DataTable();
                var query = from TBM1301 in GM.TBM1301.DefaultIfEmpty().AsEnumerable()
                            join TBMRECEIVE_BID in MPMS.TBMRECEIVE_BID.DefaultIfEmpty().AsEnumerable() on TBM1301.OVC_PURCH equals TBMRECEIVE_BID.OVC_PURCH
                            join TBMRECEIVE_WORK in MPMS.TBMRECEIVE_WORK.DefaultIfEmpty().AsEnumerable() on TBM1301.OVC_PURCH equals TBMRECEIVE_WORK.OVC_PURCH
                            orderby TBM1301.OVC_PURCH ascending
                            select new
                            {
                                OVC_PURCH = TBM1301.OVC_PURCH ?? "",
                                OVC_PUR_AGENCY = TBM1301.OVC_PUR_AGENCY ?? "",
                                OVC_PURCH_5 = TBMRECEIVE_BID.OVC_PURCH_5 ?? "",
                                OVC_PUR_IPURCH = TBM1301.OVC_PUR_IPURCH ?? "",
                                OVC_PUR_DAPPROVE = TBM1301.OVC_PUR_DAPPROVE ?? "",
                                OVC_DANNOUNCE = DateTime.TryParse(TBMRECEIVE_WORK.OVC_DANNOUNCE, out dateValue)==false?"": TBMRECEIVE_WORK.OVC_DANNOUNCE,
                                OVC_PUR_APPROVE = TBM1301.OVC_PUR_APPROVE ?? "",
                                OVC_DOPEN = TBMRECEIVE_WORK.OVC_DOPEN ?? "",
                                ONB_TIMES = TBMRECEIVE_WORK.ONB_TIMES,
                                OVC_PUR_ASS_VEN_CODE = TBMRECEIVE_WORK.OVC_PUR_ASS_VEN_CODE ?? "",
                                OVC_DO_NAME = TBMRECEIVE_BID.OVC_DO_NAME ?? ""
                            };
                query = query.Where(table => !table.OVC_DANNOUNCE.Equals(""));
                query = query.Where(table => DateTime.Compare(Convert.ToDateTime(table.OVC_DANNOUNCE), Convert.ToDateTime(txtQuery2_s.Text)) >= 0);
                query = query.Where(table => DateTime.Compare(Convert.ToDateTime(table.OVC_DANNOUNCE), Convert.ToDateTime(txtQuery2_e.Text)) <= 0);

                dt = CommonStatic.LinqQueryToDataTable(query);
                DataColumn column = new DataColumn();
                column.ColumnName = "RANK";
                column.DataType = System.Type.GetType("System.Int32");
                dt.Columns.Add(column);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Rank"] = i + 1;
                    }
                    dt.Columns["RANK"].SetOrdinal(0);//定義欄位順序
                    dt.Columns["RANK"].ColumnName = "項次";
                    dt.Columns["OVC_PURCH"].ColumnName = "購案編號";
                    dt.Columns["OVC_PUR_AGENCY"].ColumnName = "第四組";
                    dt.Columns["OVC_PURCH_5"].ColumnName = "第五組";
                    dt.Columns["OVC_PUR_IPURCH"].ColumnName = "購案名稱";
                    dt.Columns["OVC_PUR_DAPPROVE"].ColumnName = "核定日期";
                    dt.Columns["OVC_PUR_APPROVE"].ColumnName = "核定文號";
                    dt.Columns["OVC_DANNOUNCE"].ColumnName = "公告日期";
                    dt.Columns["OVC_DOPEN"].ColumnName = "開標日期";
                    dt.Columns["ONB_TIMES"].ColumnName = "開標次數";
                    dt.Columns["OVC_PUR_ASS_VEN_CODE"].ColumnName = "招標方式";
                    dt.Columns["OVC_DO_NAME"].ColumnName = "購訂承辦人";
                }
                else
                {
                    //選定範圍沒有資料給預設值                 
                    dt.Columns.Add("OVC_PURCH");
                    dt.Columns.Add("OVC_PUR_AGENCY");
                    dt.Columns.Add("OVC_PURCH_5");
                    dt.Columns.Add("OVC_PUR_IPURCH");
                    dt.Columns.Add("OVC_PUR_DAPPROVE");
                    dt.Columns.Add("OVC_PUR_APPROVE");
                    dt.Columns.Add("OVC_DANNOUNCE");
                    dt.Columns.Add("OVC_DOPEN");
                    dt.Columns.Add("ONB_TIMES");
                    dt.Columns.Add("OVC_PUR_ASS_VEN_CODE");
                    dt.Columns.Add("OVC_DO_NAME");

                    dt.Columns["RANK"].ColumnName = "項次";
                    dt.Columns["OVC_PURCH"].ColumnName = "購案編號";
                    dt.Columns["OVC_PUR_AGENCY"].ColumnName = "第四組";
                    dt.Columns["OVC_PURCH_5"].ColumnName = "第五組";
                    dt.Columns["OVC_PUR_IPURCH"].ColumnName = "購案名稱";
                    dt.Columns["OVC_PUR_DAPPROVE"].ColumnName = "核定日期";
                    dt.Columns["OVC_PUR_APPROVE"].ColumnName = "核定文號";
                    dt.Columns["OVC_DANNOUNCE"].ColumnName = "公告日期";
                    dt.Columns["OVC_DOPEN"].ColumnName = "開標日期";
                    dt.Columns["ONB_TIMES"].ColumnName = "開標次數";
                    dt.Columns["OVC_PUR_ASS_VEN_CODE"].ColumnName = "招標方式";
                    dt.Columns["OVC_DO_NAME"].ColumnName = "購訂承辦人";
                }

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
                ws.CreateRow(0).CreateCell(0).SetCellValue("核定日期自" + txtQuery1_s.Text + " 至 " + txtQuery1_s.Text + "止 排標統計表");
                for (int i = 1; i <= 11; i++)
                {
                    ws.GetRow(0).CreateCell(i);
                }
                ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 11));
                string timenow = "";
                timenow = DateTime.Now.ToString("yyyy-MM-dd");
                //第二欄資料
                ws.CreateRow(1).CreateCell(0).SetCellValue("印表日期:" + timenow);
                for (int i = 1; i <= 11; i++)
                {
                    ws.GetRow(1).CreateCell(i);
                }
                ws.AddMergedRegion(new CellRangeAddress(1, 1, 0, 11));
                for (int i = 0; i <= 1; i++)
                {
                    for (int j = 0; j <= 11; j++)
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
                ws.SetColumnWidth(1, 10 * 256);
                ws.SetColumnWidth(2, 7 * 256);
                ws.SetColumnWidth(3, 7 * 256);
                ws.SetColumnWidth(4, 40 * 256);
                ws.SetColumnWidth(5, 15 * 256);
                ws.SetColumnWidth(6, 15 * 256);
                ws.SetColumnWidth(7, 25 * 256);
                ws.SetColumnWidth(8, 15 * 256);
                ws.SetColumnWidth(9, 5 * 256);
                ws.SetColumnWidth(10, 40 * 256);
                ws.SetColumnWidth(11, 10 * 256);
                wb.Write(ms);
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename=排標統計表.xls"));
                Response.BinaryWrite(ms.ToArray());
                wb = null;
                ms.Close();
                ms.Dispose();
            }
        }

        protected void btnQuery3_Click(object sender, EventArgs e)
        {
            if (txtQuery3_s.Text == "" || txtQuery3_e.Text == "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "開標日期起訖皆為必選欄位!");
            else
            {
                DateTime dateValue;
                DataTable dt = new DataTable();
                var query = from TBM1301 in GM.TBM1301.DefaultIfEmpty().AsEnumerable()
                            join TBMRECEIVE_BID in MPMS.TBMRECEIVE_BID.DefaultIfEmpty().AsEnumerable() on TBM1301.OVC_PURCH equals TBMRECEIVE_BID.OVC_PURCH
                            join TBMRECEIVE_WORK in MPMS.TBMRECEIVE_WORK.DefaultIfEmpty().AsEnumerable() on TBM1301.OVC_PURCH equals TBMRECEIVE_WORK.OVC_PURCH
                            orderby TBM1301.OVC_PURCH ascending
                            select new
                            {
                                OVC_PURCH = TBM1301.OVC_PURCH ?? "",
                                OVC_PUR_AGENCY = TBM1301.OVC_PUR_AGENCY ?? "",
                                OVC_PURCH_5 = TBMRECEIVE_BID.OVC_PURCH_5 ?? "",
                                OVC_PUR_IPURCH = TBM1301.OVC_PUR_IPURCH ?? "",
                                OVC_PUR_DAPPROVE = TBM1301.OVC_PUR_DAPPROVE ?? "",
                                OVC_DANNOUNCE = TBMRECEIVE_WORK.OVC_DANNOUNCE ?? "",
                                OVC_PUR_APPROVE = TBM1301.OVC_PUR_APPROVE ?? "",
                                OVC_DOPEN = DateTime.TryParse(TBMRECEIVE_WORK.OVC_DOPEN, out dateValue) == false ? "" : TBMRECEIVE_WORK.OVC_DOPEN,
                                ONB_TIMES = TBMRECEIVE_WORK.ONB_TIMES,
                                OVC_PUR_ASS_VEN_CODE = TBMRECEIVE_WORK.OVC_PUR_ASS_VEN_CODE ?? "",
                                OVC_DO_NAME = TBMRECEIVE_BID.OVC_DO_NAME ?? ""
                            };
                query = query.Where(table => !table.OVC_DOPEN.Equals(""));
                query = query.Where(table => DateTime.Compare(Convert.ToDateTime(table.OVC_DOPEN), Convert.ToDateTime(txtQuery3_s.Text)) >= 0);
                query = query.Where(table => DateTime.Compare(Convert.ToDateTime(table.OVC_DOPEN), Convert.ToDateTime(txtQuery3_e.Text)) <= 0);

                dt = CommonStatic.LinqQueryToDataTable(query);
                DataColumn column = new DataColumn();
                column.ColumnName = "RANK";
                column.DataType = System.Type.GetType("System.Int32");
                dt.Columns.Add(column);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Rank"] = i + 1;
                    }
                    dt.Columns["RANK"].SetOrdinal(0);//定義欄位順序
                    dt.Columns["RANK"].ColumnName = "項次";
                    dt.Columns["OVC_PURCH"].ColumnName = "購案編號";
                    dt.Columns["OVC_PUR_AGENCY"].ColumnName = "第四組";
                    dt.Columns["OVC_PURCH_5"].ColumnName = "第五組";
                    dt.Columns["OVC_PUR_IPURCH"].ColumnName = "購案名稱";
                    dt.Columns["OVC_PUR_DAPPROVE"].ColumnName = "核定日期";
                    dt.Columns["OVC_PUR_APPROVE"].ColumnName = "核定文號";
                    dt.Columns["OVC_DANNOUNCE"].ColumnName = "公告日期";
                    dt.Columns["OVC_DOPEN"].ColumnName = "開標日期";
                    dt.Columns["ONB_TIMES"].ColumnName = "開標次數";
                    dt.Columns["OVC_PUR_ASS_VEN_CODE"].ColumnName = "招標方式";
                    dt.Columns["OVC_DO_NAME"].ColumnName = "購訂承辦人";
                }
                else
                {
                    //選定範圍沒有資料給預設值                 
                    dt.Columns.Add("OVC_PURCH");
                    dt.Columns.Add("OVC_PUR_AGENCY");
                    dt.Columns.Add("OVC_PURCH_5");
                    dt.Columns.Add("OVC_PUR_IPURCH");
                    dt.Columns.Add("OVC_PUR_DAPPROVE");
                    dt.Columns.Add("OVC_PUR_APPROVE");
                    dt.Columns.Add("OVC_DANNOUNCE");
                    dt.Columns.Add("OVC_DOPEN");
                    dt.Columns.Add("ONB_TIMES");
                    dt.Columns.Add("OVC_PUR_ASS_VEN_CODE");
                    dt.Columns.Add("OVC_DO_NAME");

                    dt.Columns["RANK"].ColumnName = "項次";
                    dt.Columns["OVC_PURCH"].ColumnName = "購案編號";
                    dt.Columns["OVC_PUR_AGENCY"].ColumnName = "第四組";
                    dt.Columns["OVC_PURCH_5"].ColumnName = "第五組";
                    dt.Columns["OVC_PUR_IPURCH"].ColumnName = "購案名稱";
                    dt.Columns["OVC_PUR_DAPPROVE"].ColumnName = "核定日期";
                    dt.Columns["OVC_PUR_APPROVE"].ColumnName = "核定文號";
                    dt.Columns["OVC_DANNOUNCE"].ColumnName = "公告日期";
                    dt.Columns["OVC_DOPEN"].ColumnName = "開標日期";
                    dt.Columns["ONB_TIMES"].ColumnName = "開標次數";
                    dt.Columns["OVC_PUR_ASS_VEN_CODE"].ColumnName = "招標方式";
                    dt.Columns["OVC_DO_NAME"].ColumnName = "購訂承辦人";
                }

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
                ws.CreateRow(0).CreateCell(0).SetCellValue("核定日期自" + txtQuery1_s.Text + " 至 " + txtQuery1_s.Text + "止 排標統計表");
                for (int i = 1; i <= 11; i++)
                {
                    ws.GetRow(0).CreateCell(i);
                }
                ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 11));
                string timenow = "";
                timenow = DateTime.Now.ToString("yyyy-MM-dd");
                //第二欄資料
                ws.CreateRow(1).CreateCell(0).SetCellValue("印表日期:" + timenow);
                for (int i = 1; i <= 11; i++)
                {
                    ws.GetRow(1).CreateCell(i);
                }
                ws.AddMergedRegion(new CellRangeAddress(1, 1, 0, 11));
                for (int i = 0; i <= 1; i++)
                {
                    for (int j = 0; j <= 11; j++)
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
                ws.SetColumnWidth(1, 10 * 256);
                ws.SetColumnWidth(2, 7 * 256);
                ws.SetColumnWidth(3, 7 * 256);
                ws.SetColumnWidth(4, 40 * 256);
                ws.SetColumnWidth(5, 15 * 256);
                ws.SetColumnWidth(6, 15 * 256);
                ws.SetColumnWidth(7, 25 * 256);
                ws.SetColumnWidth(8, 15 * 256);
                ws.SetColumnWidth(9, 5 * 256);
                ws.SetColumnWidth(10, 40 * 256);
                ws.SetColumnWidth(11, 10 * 256);
                wb.Write(ms);
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename=排標統計表.xls"));
                Response.BinaryWrite(ms.ToArray());
                wb = null;
                ms.Close();
                ms.Dispose();
            }
        }

        protected void btnQuery4_Click(object sender, EventArgs e)
        {
            if (txtQuery4_s.Text == "" || txtQuery4_e.Text == "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "開標日期起訖皆為必選欄位!");
            else
    
            {
                DateTime dateValue;
                DataTable dt = new DataTable();
                var query = from TBM1301 in GM.TBM1301.DefaultIfEmpty().AsEnumerable()
                            join TBMRECEIVE_BID in MPMS.TBMRECEIVE_BID.DefaultIfEmpty().AsEnumerable() on TBM1301.OVC_PURCH equals TBMRECEIVE_BID.OVC_PURCH
                            join TBMRECEIVE_WORK in MPMS.TBMRECEIVE_WORK.DefaultIfEmpty().AsEnumerable() on TBM1301.OVC_PURCH equals TBMRECEIVE_WORK.OVC_PURCH
                            orderby TBMRECEIVE_WORK.OVC_DOPEN ascending
                            select new
                            {
                                OVC_DOPEN = DateTime.TryParse(TBMRECEIVE_WORK.OVC_DOPEN, out dateValue) == false ? "" : Convert.ToDateTime(TBMRECEIVE_WORK.OVC_DOPEN).ToString("yyyy/MM/dd dddd"),
                                OVC_OPEN_HOUR= TBMRECEIVE_WORK.OVC_OPEN_HOUR??"",
                                OVC_OPEN_MIN = TBMRECEIVE_WORK.OVC_OPEN_MIN ?? "",
                                OVC_PURCH=TBM1301.OVC_PURCH==null|| TBM1301.OVC_PUR_AGENCY==null|| TBMRECEIVE_BID.OVC_PURCH_5==null?"": TBM1301.OVC_PURCH+ TBM1301.OVC_PUR_AGENCY+ TBMRECEIVE_BID.OVC_PURCH_5,
                                OVC_PUR_IPURCH = TBM1301.OVC_PUR_IPURCH ?? "",
                                OVC_PUR_ASS_VEN_CODE=TBMRECEIVE_WORK.OVC_PUR_ASS_VEN_CODE??"",
                                dija="",
                                ONB_TIMES = TBMRECEIVE_WORK.ONB_TIMES,
                                remark=""
                                

                            };
                query = query.Where(table => !table.OVC_DOPEN.Equals(""));
                query = query.Where(table => DateTime.Compare(Convert.ToDateTime(table.OVC_DOPEN), Convert.ToDateTime(txtQuery4_s.Text)) >= 0);
                query = query.Where(table => DateTime.Compare(Convert.ToDateTime(table.OVC_DOPEN), Convert.ToDateTime(txtQuery4_e.Text)) <= 0);

                dt = CommonStatic.LinqQueryToDataTable(query);
                DataColumn column = new DataColumn();
                column.ColumnName = "RANK";
                column.DataType = System.Type.GetType("System.Int32");
                dt.Columns.Add(column);

                if(dt.Rows.Count > 0)
                { 
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Rank"] = i + 1;
                    }
                    dt.Columns["RANK"].SetOrdinal(0);//定義欄位順序
                    dt.Columns["RANK"].ColumnName = "項次";
                    dt.Columns["OVC_DOPEN"].ColumnName = "開標日期";
                    dt.Columns["OVC_OPEN_HOUR"].ColumnName = "時";
                    dt.Columns["OVC_OPEN_MIN"].ColumnName = "分";
                    dt.Columns["OVC_PURCH"].ColumnName = "購案編號";
                    dt.Columns["OVC_PUR_IPURCH"].ColumnName = "購案名稱";
                    dt.Columns["OVC_PUR_ASS_VEN_CODE"].ColumnName = "招標方式";
                    dt.Columns["dija"].ColumnName = "底價核定權責";
                    dt.Columns["ONB_TIMES"].ColumnName = "開標次數";
                    dt.Columns["remark"].ColumnName = "備考(鑑價承辦人)";
                }
                else
                {
                    //選定範圍沒有資料給預設值                 
                    dt.Columns.Add("OVC_DOPEN");
                    dt.Columns.Add("OVC_OPEN_HOUR");
                    dt.Columns.Add("OVC_OPEN_MIN");
                    dt.Columns.Add("OVC_PURCH");
                    dt.Columns.Add("OVC_PUR_IPURCH");
                    dt.Columns.Add("OVC_PUR_ASS_VEN_CODE");
                    dt.Columns.Add("dija");
                    dt.Columns.Add("ONB_TIMES");
                    dt.Columns.Add("remark");

                    dt.Columns["RANK"].ColumnName = "項次";
                    dt.Columns["OVC_DOPEN"].ColumnName = "開標日期";
                    dt.Columns["OVC_OPEN_HOUR"].ColumnName = "時";
                    dt.Columns["OVC_OPEN_MIN"].ColumnName = "分";
                    dt.Columns["OVC_PURCH"].ColumnName = "購案編號";
                    dt.Columns["OVC_PUR_IPURCH"].ColumnName = "購案名稱";
                    dt.Columns["OVC_PUR_ASS_VEN_CODE"].ColumnName = "招標方式";
                    dt.Columns["dija"].ColumnName = "底價核定權責";
                    dt.Columns["ONB_TIMES"].ColumnName = "開標次數";
                    dt.Columns["remark"].ColumnName = "備考(鑑價承辦人)";
                }


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
                ws.CreateRow(0).CreateCell(0).SetCellValue("核定日期自" + txtQuery1_s.Text + " 至 " + txtQuery1_s.Text + "止 開標預劃期程表");
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
                ws.SetColumnWidth(1, 20 * 256);
                ws.SetColumnWidth(2, 5 * 256);
                ws.SetColumnWidth(3, 5 * 256);
                ws.SetColumnWidth(4, 15 * 256);
                ws.SetColumnWidth(5, 40 * 256);
                ws.SetColumnWidth(6, 40 * 256);
                ws.SetColumnWidth(7, 10 * 256);
                ws.SetColumnWidth(8, 8 * 256);
                ws.SetColumnWidth(9, 15 * 256);
                wb.Write(ms);
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename=開標預劃期程表.xls"));
                Response.BinaryWrite(ms.ToArray());
                wb = null;
                ms.Close();
                ms.Dispose();
            }
        }

        protected void btnQuery5_Click(object sender, EventArgs e)
        {
            if (txtQuery5_s.Text == "" || txtQuery5_e.Text == "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "決標日期起訖皆為必選欄位!");
            else
            {
                DateTime dateValue;
                DataTable dt = new DataTable();
                string[] strArray1 = new string[] { "0", "1" };//測試只有0
                string[] strArray2 = new string[] { "A","B","C","X" };
                string[] strArray3 = new string[] { "L", "W" };
                var query = from TBM1303 in MPMS.TBM1303.DefaultIfEmpty().AsEnumerable()
                            join TBM1302 in MPMS.TBM1302.DefaultIfEmpty().AsEnumerable() on TBM1303.OVC_PURCH equals TBM1302.OVC_PURCH
                            join TBMRECEIVE_WORK in MPMS.TBMRECEIVE_WORK.DefaultIfEmpty().AsEnumerable() on TBM1303.OVC_PURCH equals TBMRECEIVE_WORK.OVC_PURCH
                            join TBMBID_RESULT in MPMS.TBMBID_RESULT.DefaultIfEmpty().AsEnumerable() on TBM1303.OVC_PURCH equals TBMBID_RESULT.OVC_PURCH
                            join TBM1301_PLAN in GM.TBM1301_PLAN.DefaultIfEmpty().AsEnumerable() on TBM1303.OVC_PURCH equals TBM1301_PLAN.OVC_PURCH
                            where strArray1.Contains(TBM1303.OVC_RESULT)
                            where TBM1301_PLAN.OVC_PURCHASE_UNIT.Equals("0A100") || TBM1301_PLAN.OVC_PURCHASE_UNIT.Equals("00N00")
                            where strArray2.Contains(TBM1301_PLAN.OVC_PUR_APPROVE_DEP)
                            where strArray3.Contains(TBM1301_PLAN.OVC_PUR_AGENCY)
                            where TBMBID_RESULT.OVC_DOPEN_NEXT ==null
                            orderby TBM1303.OVC_DOPEN ascending
                            select new
                            {
                                OVC_DOPEN = DateTime.TryParse(TBM1303.OVC_DOPEN, out dateValue) == false ? "" : TBM1303.OVC_DOPEN,
                                OVC_PUR_NSECTION = TBM1301_PLAN.OVC_PUR_NSECTION ?? "",
                                OVC_PURCH = TBM1301_PLAN.OVC_PURCH == null || TBM1301_PLAN.OVC_PUR_AGENCY == null || TBM1303.OVC_PURCH_5 == null ? "" : TBM1301_PLAN.OVC_PURCH + TBM1301_PLAN.OVC_PUR_AGENCY + TBM1303.OVC_PURCH_5,
                                ONB_GROUP = TBM1302.ONB_GROUP,
                                OVC_PURCH_6 = TBM1302.OVC_PURCH_6 ?? "",
                                OVC_PUR_IPURCH = TBM1301_PLAN.OVC_PUR_IPURCH ?? "",
                                OVC_PUR_ASS_VEN_CODE = TBMRECEIVE_WORK.OVC_PUR_ASS_VEN_CODE ?? "",
                                ONB_TIMES = TBM1303.ONB_TIMES,
                                OVC_PUR_APPROVE_DEP = TBM1301_PLAN.OVC_PUR_APPROVE_DEP ?? "",
                                OVC_CURRENT = TBMBID_RESULT.OVC_CURRENT ?? "",
                                ONB_BUDGET = TBMBID_RESULT.ONB_BUDGET,
                                OVC_BID_CURRENT = TBMBID_RESULT.OVC_BID_CURRENT ?? "",
                                ONB_BID_BUDGET = TBMBID_RESULT.ONB_BID_BUDGET,
                                OVC_RESULT_CURRENT = TBMBID_RESULT.OVC_RESULT_CURRENT ?? "",
                                ONB_BID_RESULT = TBMBID_RESULT.ONB_BID_RESULT,
                                OVC_REMAIN_CURRENT = TBMBID_RESULT.OVC_REMAIN_CURRENT ?? "",
                                ONB_REMAIN_BUDGET = TBMBID_RESULT.ONB_REMAIN_BUDGET,
                                PERSENT_BID = (TBMBID_RESULT.ONB_BID_BUDGET == 0 || TBMBID_RESULT.ONB_BID_BUDGET == null) ? 0 + "%" : Math.Truncate((Convert.ToDecimal(TBMBID_RESULT.ONB_BID_RESULT) / Convert.ToDecimal(TBMBID_RESULT.ONB_BID_BUDGET))*100) + "%",
                                OVC_VEN_TITLE=TBM1302.OVC_VEN_TITLE??"",
                                ONB_BID_VENDORS=TBM1303.ONB_BID_VENDORS

                            };
                query = query.Where(table => !table.OVC_DOPEN.Equals(""));
                query = query.Where(table => DateTime.Compare(Convert.ToDateTime(table.OVC_DOPEN), Convert.ToDateTime(txtQuery5_s.Text)) >= 0);
                query = query.Where(table => DateTime.Compare(Convert.ToDateTime(table.OVC_DOPEN), Convert.ToDateTime(txtQuery5_e.Text)) <= 0);

                dt = CommonStatic.LinqQueryToDataTable(query);
                DataColumn column = new DataColumn();
                column.ColumnName = "RANK";
                column.DataType = System.Type.GetType("System.Int32");
                dt.Columns.Add(column);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Rank"] = i + 1;
                    }
                    dt.Columns["RANK"].SetOrdinal(0);//定義欄位順序

                    dt.Columns["RANK"].ColumnName = "項次";
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
                }
                else
                {
                    //選定範圍沒有資料給預設值                 
                    dt.Columns.Add("OVC_DOPEN");
                    dt.Columns.Add("OVC_PUR_NSECTION");
                    dt.Columns.Add("OVC_PURCH");
                    dt.Columns.Add("ONB_GROUP");
                    dt.Columns.Add("OVC_PURCH_6");
                    dt.Columns.Add("OVC_PUR_IPURCH");
                    dt.Columns.Add("OVC_PUR_ASS_VEN_CODE");
                    dt.Columns.Add("ONB_TIMES");
                    dt.Columns.Add("OVC_PUR_APPROVE_DEP");
                    dt.Columns.Add("OVC_CURRENT");
                    dt.Columns.Add("ONB_BUDGET");
                    dt.Columns.Add("OVC_BID_CURRENT");
                    dt.Columns.Add("ONB_BID_BUDGET");
                    dt.Columns.Add("OVC_RESULT_CURRENT");
                    dt.Columns.Add("ONB_BID_RESULT");
                    dt.Columns.Add("OVC_REMAIN_CURRENT");
                    dt.Columns.Add("ONB_REMAIN_BUDGET");
                    dt.Columns.Add("PERSENT_BID");
                    dt.Columns.Add("OVC_VEN_TITLE");
                    dt.Columns.Add("ONB_BID_VENDORS");

                    dt.Columns["RANK"].ColumnName = "項次";
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
                }





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
                ws.CreateRow(0).CreateCell(0).SetCellValue("核定日期自" + txtQuery1_s.Text + " 至 " + txtQuery1_s.Text + "止 決標購案統計表");
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
}