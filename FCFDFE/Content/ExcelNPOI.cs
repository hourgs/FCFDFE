using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Data;
using System;
using NPOI.SS.Util;
using System.Drawing;

namespace FCFDFE.Content
{
    public class ExcelNPOI
    {
        #region 讀取Excel
        public static DataTable RenderStreamToDataTable(Stream FileNameStream, string strFileName, int rowIndex)
        {
            Common FCommon = new Common();
            DataTable dt = new DataTable();

            IWorkbook hssfworkbook;
            using (Stream file = FileNameStream)
            {
                hssfworkbook = WorkbookFactory.Create(file);
            }
            ISheet sheet = hssfworkbook.GetSheetAt(0);

            IRow headRow = sheet.GetRow(0);
            if (headRow != null)
            {
                int colCount = headRow.LastCellNum;
                for (int i = 0; i < colCount; i++)
                {
                    dt.Columns.Add("COL_" + i);
                }
            }

            for (int i = (sheet.FirstRowNum + rowIndex); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                bool emptyRow = true;
                object[] itemArray = null;

                if (row != null)
                {
                    itemArray = new object[row.LastCellNum];

                    for (int j = row.FirstCellNum; j < row.LastCellNum; j++)
                    {

                        if (row.GetCell(j) != null)
                        {

                            switch (row.GetCell(j).CellType)
                            {
                                case CellType.Numeric:
                                    if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))//日期類型
                                    {
                                        itemArray[j] = FCommon.getDateTime(row.GetCell(j).DateCellValue);
                                    }
                                    else//其他數字類型
                                    {
                                        itemArray[j] = row.GetCell(j).NumericCellValue;
                                    }
                                    break;
                                case CellType.Blank:
                                    itemArray[j] = string.Empty;
                                    break;
                                case CellType.Formula:
                                    if (Path.GetExtension(strFileName).ToLower().Trim() == ".xlsx")
                                    {
                                        XSSFFormulaEvaluator eva = new XSSFFormulaEvaluator(hssfworkbook);
                                        if (eva.Evaluate(row.GetCell(j)).CellType == CellType.Numeric)
                                        {
                                            if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))//日期類型
                                            {
                                                itemArray[j] = FCommon.getDateTime(row.GetCell(j).DateCellValue);
                                            }
                                            else//其他數字類型
                                            {
                                                itemArray[j] = row.GetCell(j).NumericCellValue;
                                            }
                                        }
                                        else
                                        {
                                            itemArray[j] = eva.Evaluate(row.GetCell(j)).StringValue;
                                        }
                                    }
                                    else
                                    {
                                        HSSFFormulaEvaluator eva = new HSSFFormulaEvaluator(hssfworkbook);
                                        if (eva.Evaluate(row.GetCell(j)).CellType == CellType.Numeric)
                                        {
                                            if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))//日期類型
                                            {
                                                itemArray[j] = FCommon.getDateTime(row.GetCell(j).DateCellValue);
                                            }
                                            else//其他數字類型
                                            {
                                                itemArray[j] = row.GetCell(j).NumericCellValue;
                                            }
                                        }
                                        else
                                        {
                                            itemArray[j] = eva.Evaluate(row.GetCell(j)).StringValue;
                                        }
                                    }
                                    break;
                                default:
                                    itemArray[j] = row.GetCell(j).StringCellValue;
                                    break;
                            }

                            if (itemArray[j] != null && !string.IsNullOrEmpty(itemArray[j].ToString().Trim()))
                            {
                                emptyRow = false;
                            }
                        }
                    }
                }

                //非空數據行數據添加到DataTable
                if (!emptyRow)
                {
                    dt.Rows.Add(itemArray);
                }
            }
            return dt;
        }
        #endregion

        #region 儲存Excel
        #region 範例一，簡單產生Excel檔案的方法
        //private void CreateExcelFile()
        //{
        //    //建立Excel 2003檔案
        //    //IWorkbook wb = new HSSFWorkbook();
        //    //ISheet ws = wb.CreateSheet("Class");

        //    ////建立Excel 2007檔案
        //    IWorkbook wb = new XSSFWorkbook();
        //    ISheet ws = wb.CreateSheet("Class");

        //    ws.CreateRow(0);//第一行為欄位名稱
        //    ws.GetRow(0).CreateCell(0).SetCellValue("name");
        //    ws.GetRow(0).CreateCell(1).SetCellValue("score");
        //    ws.CreateRow(1);//第二行之後為資料
        //    ws.GetRow(1).CreateCell(0).SetCellValue("abey");
        //    ws.GetRow(1).CreateCell(1).SetCellValue(85);
        //    ws.CreateRow(2);
        //    ws.GetRow(2).CreateCell(0).SetCellValue("tina");
        //    ws.GetRow(2).CreateCell(1).SetCellValue(82);
        //    ws.CreateRow(3);
        //    ws.GetRow(3).CreateCell(0).SetCellValue("boi");
        //    ws.GetRow(3).CreateCell(1).SetCellValue(84);
        //    ws.CreateRow(4);
        //    ws.GetRow(4).CreateCell(0).SetCellValue("hebe");
        //    ws.GetRow(4).CreateCell(1).SetCellValue(86);
        //    ws.CreateRow(5);
        //    ws.GetRow(5).CreateCell(0).SetCellValue("paul");
        //    ws.GetRow(5).CreateCell(1).SetCellValue(82);
        //    FileStream file = new FileStream(@"d:\tmp\npoi.xls", FileMode.Create);//產生檔案
        //    wb.Write(file);
        //    file.Close();
        //}
        #endregion

        #region 範例二，DataTable轉成Excel檔案的方法
        //private void DataTableToExcelFile(DataTable dt, string strPath, string strFileName, string SheetName)
        //{
        //    //建立Excel 2003檔案
        //    //IWorkbook wb = new HSSFWorkbook();
        //    //ISheet ws;

        //    ////建立Excel 2007檔案
        //    IWorkbook wb = new XSSFWorkbook();
        //    ISheet ws;

        //    ws = wb.CreateSheet(SheetName);

        //    ws.CreateRow(0);//第一行為欄位名稱
        //    for (int i = 0; i < dt.Columns.Count; i++)
        //    {
        //        ws.GetRow(0).CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
        //    }

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        ws.CreateRow(i + 1);
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            ws.GetRow(i + 1).CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
        //        }
        //    }

        //    FileStream file = new FileStream(@"d:\tmp\npoi.xls", FileMode.Create);//產生檔案
        //    wb.Write(file);
        //    file.Close();
        //}
        #endregion

        //將DataTable產生Excel檔案之資料流
        public static MemoryStream RenderDataTableToStream(DataTable SourceTable, string strSheet)
        {
            MemoryStream ms = new MemoryStream();
            XSSFWorkbook workbook = new XSSFWorkbook();
            XSSFSheet sheet = (XSSFSheet)workbook.CreateSheet(strSheet);
            XSSFRow headerRow = (XSSFRow)sheet.CreateRow(0);

            // handling header.
            foreach (DataColumn column in SourceTable.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            }

            // handling value.
            int rowIndex = 1;
            foreach (DataRow row in SourceTable.Rows)
            {
                XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
                foreach (DataColumn column in SourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }
                rowIndex += 1;
            }
            workbook.Write(ms);
            //ms.Position = 0
            ms.Flush();

            sheet = null;
            headerRow = null;
            workbook = null;
            return ms;
        }
        //將DataTable產生之資料流儲存成Excel檔案
        public static void RenderDataTableToExcel(DataTable SourceTable, string strFileName, string strSheet, ref string strMessageError)
        {
            try
            {
                MemoryStream ms = RenderDataTableToStream(SourceTable, strSheet);
                FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write);
                byte[] data = ms.ToArray();

                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();

                data = null;
                ms = null;
                fs = null;
            }
            catch (Exception ex)
            {
                strMessageError = ex.Message;
            }
        }

        //將DataTable產生Excel檔案之資料流－多工作表
        public static MemoryStream RenderDataTableToStream(DataTable[] Tables, string[] Sheets)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                int tableCount = Tables.Length;
                int sheetCount = Sheets.Length;
                if (tableCount == sheetCount)
                {//資料表數量與工作表名稱數量相等
                    XSSFWorkbook workbook = new XSSFWorkbook();
                    XSSFSheet[] sheet = new XSSFSheet[sheetCount];
                    //XSSFCellStyle style = (XSSFCellStyle)workbook.CreateCellStyle();
                    for (int index = 0; index < sheetCount; index++)
                    {
                        string strSheet = Sheets[index];
                        DataTable SourceTable = Tables[index];

                        sheet[index] = (XSSFSheet)workbook.CreateSheet(strSheet);
                        XSSFRow headerRow = (XSSFRow)sheet[index].CreateRow(0);

                        // handling header.
                        foreach (DataColumn column in SourceTable.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                        }

                        // handling value.
                        int rowIndex = 1;
                        foreach (DataRow row in SourceTable.Rows)
                        {
                            XSSFRow dataRow = (XSSFRow)sheet[index].CreateRow(rowIndex);
                            foreach (DataColumn column in SourceTable.Columns)
                            {
                                dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                            }
                            rowIndex += 1;
                        }
                        headerRow = null;
                    }
                    workbook.Write(ms);
                    //ms.Position = 0
                    ms.Flush();
                    sheet = null;
                    workbook = null;
                }
            }
            catch
            { }
            return ms;
        }
        //將DataTable產生之資料流儲存成Excel檔案－多工作表
        public static void RenderDataTableToExcel(DataTable[] SourceTable, string strFileName, string[] strSheet)
        {
            try
            {
                MemoryStream ms = RenderDataTableToStream(SourceTable, strSheet);
                FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write);
                byte[] data = ms.ToArray();

                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();

                data = null;
                ms = null;
                fs = null;
            }
            catch
            { }
        }

        //設定儲存格內容，判斷是否為bool或數字，否則傳入文字。
        private static void setCellContent(XSSFCell cell, string strContent)
        {
            bool boolContent;
            //DateTime dateContent;
            double doubleContent;
            if (bool.TryParse(strContent, out boolContent))
                cell.SetCellValue(boolContent); //設定儲存格文字
            //else if (DateTime.TryParse(strContent, out dateContent)) //日期以文字呈現
            //    cell.SetCellValue(dateContent); //設定儲存格文字
            else if (double.TryParse(strContent, out doubleContent))
                cell.SetCellValue(doubleContent); //設定儲存格文字
            else
                cell.SetCellValue(strContent); //設定儲存格文字
        }
        //設定儲存格框線，上下左右皆相同，顏色及樣式。
        private static void setCellBorder(XSSFCellStyle cellStyle, XSSFColor color, BorderStyle style)
        {
            cellStyle.BorderLeft = style;
            cellStyle.BorderRight = style;
            cellStyle.BorderTop = style;
            cellStyle.BorderBottom = style;
            cellStyle.SetLeftBorderColor(color);
            cellStyle.SetRightBorderColor(color);
            cellStyle.SetTopBorderColor(color);
            cellStyle.SetBottomBorderColor(color);
        }

        #region 自訂格式匯出
        //主官查詢應用－匯出原有資料流外，在上放呈現合併儲存格之標題 及 印表日期
        public static MemoryStream RenderDataTableToStream_Chief(DataTable SourceTable, string strSheet, string strTitleText, int intCountDate, string[] strCellFormat)
        {
            int intRowTitle = 0, intRowSpace = 3; //標題行數空間
            int intCountColumn = SourceTable.Columns.Count, columnIndex; //表格欄數量

            MemoryStream ms = new MemoryStream();
            XSSFWorkbook workbook = new XSSFWorkbook();
            XSSFSheet sheet = (XSSFSheet)workbook.CreateSheet(strSheet);
            XSSFColor colorWhite = new XSSFColor(Color.White);

            #region 標題列 Title
            XSSFCellStyle csTitle = (XSSFCellStyle)workbook.CreateCellStyle(); //儲存格樣式
            csTitle.VerticalAlignment = VerticalAlignment.Center; //垂直置中
            csTitle.Alignment = HorizontalAlignment.Center; //水平置中
            XSSFFont fTitle = (XSSFFont)workbook.CreateFont(); //文字樣式
            fTitle.FontHeightInPoints = 20; //文字尺寸
            csTitle.SetFont(fTitle); ;
            XSSFCellStyle csDate = (XSSFCellStyle)workbook.CreateCellStyle(); //儲存格樣式
            csDate.VerticalAlignment = VerticalAlignment.Center; //垂直置中
            csDate.Alignment = HorizontalAlignment.Right; //水平靠右
            XSSFFont fDate = (XSSFFont)workbook.CreateFont(); //文字樣式
            fDate.FontHeightInPoints = 14;
            csDate.SetFont(fDate); ;

            int intCountTitle = intCountColumn - intCountDate;
            XSSFRow titleRow = (XSSFRow)sheet.CreateRow(0); //保留標題行數空間
            titleRow.HeightInPoints = 30; //設定列高
            //產生第一個要用CreateRow
            XSSFCell cellTitle = (XSSFCell)titleRow.CreateCell(0);
            cellTitle.SetCellValue(strTitleText);
            cellTitle.CellStyle = csTitle;
            sheet.AddMergedRegion(new CellRangeAddress(0, intRowTitle, 0, intCountTitle - 1)); //(0, 5, 0, 3) = 跨越五列(共六列 1~6)，跨越三欄(共四欄 A~D)
            //印表日期
            string strDateText = DateTime.Now.ToString(Variable.strDateFormat);
            XSSFCell cellDate = (XSSFCell)titleRow.CreateCell(intCountTitle);
            cellDate.SetCellValue($"印表日期：{ strDateText }");
            cellDate.CellStyle = csDate;
            sheet.AddMergedRegion(new CellRangeAddress(0, intRowTitle, intCountTitle, intCountColumn - 1));
            #endregion

            #region 資料欄位 handling
            XSSFColor colorCell = new XSSFColor(new byte[] { 223, 228, 232 });
            XSSFColor colorBorder = new XSSFColor(new byte[] { 192, 192, 192 });
            XSSFFont fDataRow = (XSSFFont)workbook.CreateFont(); //文字樣式
            fDataRow.FontHeightInPoints = 10;
            //short[] shorFormat = new short[intCountColumn];
            XSSFCellStyle[] csCellOdd = new XSSFCellStyle[intCountColumn], csCellEven = new XSSFCellStyle[intCountColumn];
            string strFormatString = Variable.strExcleFormatString, strFormatMoney2 = Variable.strExcleFormatMoney2;

            //XSSFRow testRow = (XSSFRow)sheet.CreateRow(2); //測試
            //取得儲存格格式
            for (int i = 0; i < intCountColumn; i++)
            {
                csCellOdd[i] = (XSSFCellStyle)workbook.CreateCellStyle(); //儲存格樣式
                csCellOdd[i].VerticalAlignment = VerticalAlignment.Center; //垂直置中
                setCellBorder(csCellOdd[i], colorBorder, BorderStyle.Medium); //設定框線顏色及樣式
                csCellOdd[i].SetFont(fDataRow); //設定樣式之字形

                csCellEven[i] = (XSSFCellStyle)workbook.CreateCellStyle(); //儲存格樣式
                csCellEven[i].VerticalAlignment = VerticalAlignment.Center; //垂直置中
                csCellEven[i].FillForegroundColorColor = colorCell;
                csCellEven[i].FillPattern = FillPattern.SolidForeground; //設定填滿使用前景顏色
                setCellBorder(csCellEven[i], colorBorder, BorderStyle.Medium); //設定框線顏色及樣式
                csCellEven[i].SetFont(fDataRow); //設定樣式之字形

                string strFormat; int index = i - 1;
                XSSFDataFormat dataFormat = (XSSFDataFormat)workbook.CreateDataFormat();
                //IDataFormat dataForamt = workbook.CreateDataFormat();
                if (i == 0) //序號
                    strFormat = Variable.strExcleFormatInt; //設定序號儲存格格式
                else if (strCellFormat.Length > index && strCellFormat[index] != null) //格式陣列不包含序號欄位，因此-1
                    strFormat = strCellFormat[index];
                else
                    strFormat = strFormatString;
                short format = dataFormat.GetFormat(strFormat); //若未設定，則使用文字格式
                //testRow.CreateCell(i).SetCellValue(strFormat + ":" + format.ToString());
                csCellOdd[i].DataFormat = format;
                csCellEven[i].DataFormat = format;
            }
            //csCell.DataFormat = shorFormat[columnIndex]; //設定樣式之格式

            // handling value.
            int rowIndex = 1 + intRowSpace; //保留標題行數空間
            foreach (DataRow row in SourceTable.Rows)
            {
                XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
                dataRow.HeightInPoints = 18; //設定列高
                bool isOdd = rowIndex % 2 == 0; //取得該行為是否為奇數行
                columnIndex = 0;
                foreach (DataColumn column in SourceTable.Columns)
                {
                    XSSFCell cell = (XSSFCell)dataRow.CreateCell(column.Ordinal);
                    setCellContent(cell, row[column].ToString()); //設定儲存格內容
                    XSSFCellStyle csCell = isOdd ? csCellOdd[columnIndex] : csCellEven[columnIndex]; //儲存格樣式
                    cell.CellStyle = csCell; //設定儲存格樣式
                }
                rowIndex += 1;
            }
            #endregion

            #region 標題欄位 Title
            XSSFCellStyle csHeader = (XSSFCellStyle)workbook.CreateCellStyle(); //儲存格樣式
            csHeader.VerticalAlignment = VerticalAlignment.Center; //垂直置中
            XSSFColor colorTitleBack = new XSSFColor(new byte[] { 48, 141, 187 });
            csHeader.FillForegroundColorColor = colorTitleBack;
            csHeader.FillPattern = FillPattern.SolidForeground; //設定填滿使用前景顏色
            setCellBorder(csHeader, colorBorder, BorderStyle.Medium); //設定框線顏色及樣式
            XSSFFont fHeader = (XSSFFont)workbook.CreateFont(); //文字樣式
            fHeader.Boldweight = (short)FontBoldWeight.Bold; //粗體
            fHeader.FontHeightInPoints = 12; //文字尺寸
            fHeader.SetColor(colorWhite);
            csHeader.SetFont(fHeader);

            //標題欄位 handling header.
            columnIndex = 0;
            XSSFRow headerRow = (XSSFRow)sheet.CreateRow(0 + intRowSpace); //保留標題行數空間
            headerRow.HeightInPoints = 20; //設定列高
            foreach (DataColumn column in SourceTable.Columns)
            {
                XSSFCell cellHeader = (XSSFCell)headerRow.CreateCell(column.Ordinal);
                cellHeader.SetCellValue(column.ColumnName);
                cellHeader.CellStyle = csHeader;

                sheet.AutoSizeColumn(columnIndex++); //自動調整欄寬
            }
            #endregion

            workbook.Write(ms);
            //ms.Position = 0
            ms.Flush();

            sheet = null;
            headerRow = null;
            workbook = null;
            return ms;
        }
        //原有寫法，不過太久了
        //主官查詢應用－匯出原有資料流外，在上放呈現合併儲存格之標題 及 印表日期
        //public static MemoryStream RenderDataTableToStream_Chief(DataTable SourceTable, string strSheet, string strTitleText, int intCountDate, string[] strCellFormat)
        //{
        //    int intRowTitle = 0, intRowSpace = 3; //標題行數空間
        //    int intCountColumn = SourceTable.Columns.Count, columnIndex; //表格欄數量

        //    MemoryStream ms = new MemoryStream();
        //    XSSFWorkbook workbook = new XSSFWorkbook();
        //    XSSFSheet sheet = (XSSFSheet)workbook.CreateSheet(strSheet);
        //    XSSFColor colorWhite = new XSSFColor(Color.White);

        //    #region 標題列 Title
        //    XSSFCellStyle csTitle = (XSSFCellStyle)workbook.CreateCellStyle(); //儲存格樣式
        //    csTitle.VerticalAlignment = VerticalAlignment.Center; //垂直置中
        //    csTitle.Alignment = HorizontalAlignment.Center; //水平置中
        //    XSSFFont fTitle = (XSSFFont)workbook.CreateFont(); //文字樣式
        //    fTitle.FontHeightInPoints = 20; //文字尺寸
        //    csTitle.SetFont(fTitle); ;
        //    XSSFCellStyle csDate = (XSSFCellStyle)workbook.CreateCellStyle(); //儲存格樣式
        //    csDate.VerticalAlignment = VerticalAlignment.Center; //垂直置中
        //    csDate.Alignment = HorizontalAlignment.Right; //水平靠右
        //    XSSFFont fDate = (XSSFFont)workbook.CreateFont(); //文字樣式
        //    fDate.FontHeightInPoints = 14;
        //    csDate.SetFont(fDate); ;

        //    int intCountTitle = intCountColumn - intCountDate;
        //    XSSFRow titleRow = (XSSFRow)sheet.CreateRow(0); //保留標題行數空間
        //    titleRow.HeightInPoints = 30; //設定列高
        //    //產生第一個要用CreateRow
        //    XSSFCell cellTitle = (XSSFCell)titleRow.CreateCell(0);
        //    cellTitle.SetCellValue(strTitleText);
        //    cellTitle.CellStyle = csTitle;
        //    sheet.AddMergedRegion(new CellRangeAddress(0, intRowTitle, 0, intCountTitle - 1)); //(0, 5, 0, 3) = 跨越五列(共六列 1~6)，跨越三欄(共四欄 A~D)
        //    //印表日期
        //    string strDateText = DateTime.Now.ToString(Variable.strDateFormat);
        //    XSSFCell cellDate = (XSSFCell)titleRow.CreateCell(intCountTitle);
        //    cellDate.SetCellValue($"印表日期：{ strDateText }");
        //    cellDate.CellStyle = csDate;
        //    sheet.AddMergedRegion(new CellRangeAddress(0, intRowTitle, intCountTitle, intCountColumn - 1));
        //    #endregion

        //    #region 資料欄位 handling
        //    XSSFColor colorCell = new XSSFColor(new byte[] { 223, 228, 232 });
        //    XSSFColor colorBorder = new XSSFColor(new byte[] { 192, 192, 192 });
        //    XSSFFont fDataRow = (XSSFFont)workbook.CreateFont(); //文字樣式
        //    fDataRow.FontHeightInPoints = 10;
        //    short[] shorFormat = new short[intCountColumn];
        //    //取得儲存格格式
        //    for (int i = 0; i < intCountColumn; i++)
        //    {
        //        short format; int index = i - 1;
        //        IDataFormat dataForamt = workbook.CreateDataFormat();
        //        if (i == 0) //序號
        //            format = dataForamt.GetFormat(Variable.strExcleFormatInt); //設定序號儲存格格式
        //        else if (strCellFormat.Length > index && strCellFormat[index] != null) //格式陣列不包含序號欄位，因此-1
        //            format = dataForamt.GetFormat(strCellFormat[index]);
        //        else
        //            format = dataForamt.GetFormat(Variable.strExcleFormatString); //若未設定，則使用文字格式
        //        shorFormat[i] = format;
        //    }

        //    // handling value.
        //    int rowIndex = 1 + intRowSpace; //保留標題行數空間
        //    foreach (DataRow row in SourceTable.Rows)
        //    {
        //        XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
        //        dataRow.HeightInPoints = 18; //設定列高
        //        bool isEven = rowIndex % 2 == 0; //取得該行為是否為偶數行
        //        columnIndex = 0;
        //        foreach (DataColumn column in SourceTable.Columns)
        //        {
        //            XSSFCell cell = (XSSFCell)dataRow.CreateCell(column.Ordinal);
        //            setCellContent(cell, row[column].ToString()); //設定儲存格內容
        //            XSSFCellStyle csCell = (XSSFCellStyle)workbook.CreateCellStyle(); //儲存格樣式
        //            csCell.VerticalAlignment = VerticalAlignment.Center; //垂直置中
        //            if (isEven)
        //            {
        //                csCell.FillForegroundColorColor = colorCell;
        //                csCell.FillPattern = FillPattern.SolidForeground; //設定填滿使用前景顏色
        //            }
        //            setCellBorder(csCell, colorBorder, BorderStyle.Medium); //設定框線顏色及樣式
        //            csCell.SetFont(fDataRow); //設定樣式之字形
        //            csCell.DataFormat = shorFormat[columnIndex]; //設定樣式之格式
        //            cell.CellStyle = csCell; //設定儲存格樣式
        //        }
        //        rowIndex += 1;
        //    }
        //    #endregion

        //    #region 標題欄位 Title
        //    XSSFCellStyle csHeader = (XSSFCellStyle)workbook.CreateCellStyle(); //儲存格樣式
        //    csHeader.VerticalAlignment = VerticalAlignment.Center; //垂直置中
        //    XSSFColor colorTitleBack = new XSSFColor(new byte[] { 48, 141, 187 });
        //    csHeader.FillForegroundColorColor = colorTitleBack;
        //    csHeader.FillPattern = FillPattern.SolidForeground; //設定填滿使用前景顏色
        //    setCellBorder(csHeader, colorBorder, BorderStyle.Medium); //設定框線顏色及樣式
        //    XSSFFont fHeader = (XSSFFont)workbook.CreateFont(); //文字樣式
        //    fHeader.Boldweight = (short)FontBoldWeight.Bold; //粗體
        //    fHeader.FontHeightInPoints = 12; //文字尺寸
        //    fHeader.SetColor(colorWhite);
        //    csHeader.SetFont(fHeader);

        //    //標題欄位 handling header.
        //    columnIndex = 0;
        //    XSSFRow headerRow = (XSSFRow)sheet.CreateRow(0 + intRowSpace); //保留標題行數空間
        //    headerRow.HeightInPoints = 20; //設定列高
        //    foreach (DataColumn column in SourceTable.Columns)
        //    {
        //        XSSFCell cellHeader = (XSSFCell)headerRow.CreateCell(column.Ordinal);
        //        cellHeader.SetCellValue(column.ColumnName);
        //        cellHeader.CellStyle = csHeader;

        //        sheet.AutoSizeColumn(columnIndex++); //自動調整欄寬
        //    }
        //    #endregion

        //    workbook.Write(ms);
        //    //ms.Position = 0
        //    ms.Flush();

        //    sheet = null;
        //    headerRow = null;
        //    workbook = null;
        //    return ms;
        //}
        #endregion
        #endregion
    }
}