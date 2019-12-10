using System.Data;
using System.Data.OleDb;
using System.IO;

namespace FCFDFE.Content
{
    public class Excel
    {
        static int intIMEX;
        
        #region 須安裝Excel 沒用
        public static void ExportExcel(DataTable dataTable, string strFilePath, string strSheetName)
        {
            if (File.Exists(strFilePath))
                File.Delete(strFilePath);

            intIMEX = 0;    //0表示檔案用來做寫入用途
            string connString = getConnectionString(strFilePath, intIMEX);

            int intRows = dataTable.Rows.Count;
            int intColumns = dataTable.Columns.Count;

            //創建欄位
            //string strCreate = "CREATE TABLE TestSheet ([ID] INTEGER,[Username] VarChar,[UserPwd] VarChar)";
            string strCreate = "CREATE TABLE[" + strSheetName + "] (";
            for (int i = 0; i < intColumns; i++)
            {
                if (i < intColumns - 1)
                    strCreate += "[" + dataTable.Columns[i] + "] VarChar,";
                else
                    strCreate += "[" + dataTable.Columns[i] + "] VarChar)";
            }

            using (OleDbConnection cn = new OleDbConnection(connString))
            {
                cn.Open();
                using (OleDbCommand cmd = new OleDbCommand(strCreate, cn))
                {
                    //新增Excel工作表
                    cmd.ExecuteNonQuery();
                    //增加資料
                    for (int i = 0; i < intRows; i++)
                    {
                        //cmd.CommandText = "INSERT INTO [" + strSheetName + "$] VALUES(1,'elmer','password')";
                        string strInsert = "INSERT INTO [" + strSheetName + "$] VALUES(";
                        for (int j = 0; j < intColumns; j++)
                        {
                            if (j < intColumns - 1)
                                strInsert += "'" + dataTable.Rows[i][j] + "',";
                            else
                                strInsert += "'" + dataTable.Rows[i][j] + "')";
                        }
                        cmd.CommandText = strInsert;
                        cmd.ExecuteNonQuery();
                    }
                }
                cn.Close();
            }
        }

        public static bool checkExcelExist(string strFilePath, string strSheetName)
        {
            bool SheetNameExist = false;
            int intIMEX = 0;    //0表示檔案用來做寫入用途
            string cs = getConnectionString(strFilePath, intIMEX);
            using (OleDbConnection cn = new OleDbConnection(cs))
            {
                cn.Open();
                using (DataTable dt = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null))
                {
                    //取得工作表數量，法一
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    Console.WriteLine((String)dr["TABLE_NAME"]);
                    //    Response.Write((String)dr["TABLE_NAME"]);
                    //}
                    //取得工作表數量，法二
                    int TableCount = dt.Rows.Count;
                    for (int i = 0; i < TableCount; i++)
                    {
                        string sn = dt.Rows[i][2].ToString().Trim();
                        if (sn == strSheetName + "$")
                        {
                            //判斷工作表是否存在
                            SheetNameExist = true;
                            break;
                        }
                    }
                }
                cn.Close();
            }
            return SheetNameExist;
        }

        //public static void DataTableToExcel(DataTable dt, string connString)
        // {
        //     //先算出欄位及列數
        //     int rows = dt.Rows.Count;
        //     int cols = dt.Columns.Count;
        //     //用來建立命令 
        //     StringBuilder sb = new StringBuilder();

        //     sb.Append("CREATE TABLE ");
        //     sb.Append(dt.TableName + " ( ");
        //     //用來做開TABLE的欄名資訊
        //     for (int i = 0; i < cols; i++)
        //     {
        //         if (i < cols - 1)
        //             sb.Append(string.Format("{0} varchar,", dt.Columns[i].ColumnName));
        //         else
        //             sb.Append(string.Format("{0} varchar)", dt.Columns[i].ColumnName));
        //     }
        //     //把要開啟的臨時Excel建立起來
        //     using (OleDbConnection objConn = new OleDbConnection(connString))
        //     {
        //         OleDbCommand objCmd = new OleDbCommand();
        //         objCmd.Connection = objConn;

        //         objCmd.CommandText = sb.ToString();


        //         objConn.Open();
        //         //先執行CreateTable的任務
        //         objCmd.ExecuteNonQuery();


        //         //開始處理資料內容的新增
        //         #region 開始處理資料內容的新增
        //         //把之前 CreateTable 清空
        //         sb.Remove(0, sb.Length);
        //         sb.Append("INSERT INTO ");
        //         sb.Append(dt.TableName + " ( ");
        //         //這邊開始組該Excel欄位順序
        //         for (int i = 0; i < cols; i++)
        //         {
        //             if (i < cols - 1)
        //                 sb.Append(dt.Columns[i].ColumnName + ",");
        //             else
        //                 sb.Append(dt.Columns[i].ColumnName + ") values (");
        //         }
        //         //這邊組 DataTable裡面的值要給到Excel欄位的
        //         for (int i = 0; i < cols; i++)
        //         {
        //             if (i < cols - 1)
        //                 sb.Append("@" + dt.Columns[i].ColumnName + ",");
        //             else
        //                 sb.Append("@" + dt.Columns[i].ColumnName + ")");
        //         }
        //         #endregion


        //         //建立插入動作的Command
        //         objCmd.CommandText = sb.ToString();
        //         OleDbParameterCollection param = objCmd.Parameters;

        //         for (int i = 0; i < cols; i++)
        //         {
        //             param.Add(new OleDbParameter("@" + dt.Columns[i].ColumnName, OleDbType.VarChar));
        //         }

        //         //使用參數化的方式來給予值
        //         foreach (DataRow row in dt.Rows)
        //         {
        //             for (int i = 0; i < param.Count; i++)
        //             {
        //                 param[i].Value = row[i];
        //             }
        //             //執行這一筆的給值
        //             objCmd.ExecuteNonQuery();
        //         }


        //     }//end using
        // }
        //建立Excel空檔案
        //DataTable Dt, string MainTableName, string SheetName, string filename, string path, bool  readColName = false
        //void CreateExcel(DataTable dataTable, string strFilePath, string strSheetName)
        //{
        //    intIMEX = 0;    //0表示檔案用來做寫入用途
        //    string conString = getConnectionString(strFilePath, intIMEX);
        //    using (OleDbConnection conn = new OleDbConnection(conString))
        //    {
        //        conn.Open();
        //        string strCreate = String.Empty;
        //        int int_dtColumn = dataTable.Columns.Count;
        //        for(int i = 0; i < int_dtColumn; i++)
        //        {
        //            string strColumnName = dataTable.Columns[i].ColumnName;
        //        }

        //        using (OleDbCommand command = new OleDbCommand(strCreate, conn))
        //        {
        //            command.ExecuteNonQuery();
        //        }

        //    }
        //}

        public static string getConnectionString(string strPath, int IMEX)
        {
            //  ========EXECL讀取顯示======
            //  Extended Properties=Excel 12.0
            //  HDR=Yes，代表Excel檔中的工作表第一列是欄位名稱
            //  IMEX=0 時為「匯出模式」，這個模式開啟的 Excel 檔案只能用來做「寫入」用途。
            //  IMEX=1 時為「匯入模式」，這個模式開啟的 Excel 檔案只能用來做「讀取」用途。
            //  IMEX=2 時為「連結模式」，這個模式開啟的 Excel 檔案可同時支援「讀取」與「寫入」用途。
            //  需安裝 Microsoft Access Database Engine 2010 - 32位元
            string conncetionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                    "Data Source='" + strPath + "';" +
                    "Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=" + IMEX.ToString() + ";Persist Security Info=False'";
            return conncetionString;
        }
        #endregion
    }
}