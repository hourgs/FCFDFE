using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.Content
{
    public static class CommonStatic
    {
        private static GMEntities accountE = new GMEntities();

        #region dataTable
        //資料庫清單轉換dataTable
        public static DataTable ListToDataTable<TResult>(this IEnumerable<TResult> ListValue) where TResult : class, new()
        {
            //建立一個回傳用的 DataTable
            DataTable dt = new DataTable();

            //取得映射型別
            Type type = typeof(TResult);

            //宣告一個 PropertyInfo 陣列，來接取 Type 所有的共用屬性
            PropertyInfo[] PI_List = null;

            foreach (var item in ListValue)
            {
                //判斷 DataTable 是否已經定義欄位名稱與型態
                if (dt.Columns.Count == 0)
                {
                    //取得 Type 所有的共用屬性
                    PI_List = item.GetType().GetProperties();

                    //將 List 中的 名稱 與 型別，定義 DataTable 中的欄位 名稱 與 型別
                    foreach (var item1 in PI_List)
                    {
                        dt.Columns.Add(item1.Name);
                    }
                }

                //在 DataTable 中建立一個新的列
                DataRow dr = dt.NewRow();

                //將資料足筆新增到 DataTable 中
                foreach (var item2 in PI_List)
                {
                    dr[item2.Name] = item2.GetValue(item, null);
                }

                dt.Rows.Add(dr);
            }

            dt.AcceptChanges();

            return dt;
        }

        public static DataTable LinqQueryToDataTable2<T>(IQueryable<T> query)
        {
            DataTable dataTable = new DataTable();

            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(T)))
            {
                dataTable.Columns.Add(pd.Name, pd.PropertyType);
            }
            foreach (T item in query)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(T)))
                {
                    Row[pd.Name] = pd.GetValue(item);
                }
                dataTable.Rows.Add(Row);
            }

            return dataTable;
        }

        public static DataTable LinqQueryToDataTable<T>(IEnumerable<T> query)
        {
            DataTable tbl = new DataTable();
            PropertyInfo[] props = null;
            foreach (T item in query)
            {
                if (props == null) //尚未初始化
                {
                    Type t = item.GetType();
                    props = t.GetProperties();
                    foreach (PropertyInfo pi in props)
                    {
                        Type colType = pi.PropertyType;
                        //針對Nullable<>特別處理
                        if (colType.IsGenericType
                            && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            colType = colType.GetGenericArguments()[0];
                        //建立欄位
                        tbl.Columns.Add(pi.Name, colType);
                    }
                }
                DataRow row = tbl.NewRow();
                foreach (PropertyInfo pi in props)
                    row[pi.Name] = pi.GetValue(item, null) ?? DBNull.Value;
                tbl.Rows.Add(row);
            }
            return tbl;
        }
        public static DataTable LinqQueryToDataTable<T>(IEnumerable<T> query, string strKeyField, string[] strValue)
        {
            DataTable tbl = new DataTable();
            PropertyInfo[] props = null;
            foreach (T item in query)
            {
                if (props == null) //尚未初始化
                {
                    Type t = item.GetType();
                    props = t.GetProperties();
                    foreach (PropertyInfo pi in props)
                    {
                        Type colType = pi.PropertyType;
                        //針對Nullable<>特別處理
                        if (colType.IsGenericType
                            && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            colType = colType.GetGenericArguments()[0];
                        //建立欄位
                        tbl.Columns.Add(pi.Name, colType);
                    }
                }
                DataRow row = tbl.NewRow();
                foreach (PropertyInfo pi in props)
                    row[pi.Name] = pi.GetValue(item, null) ?? DBNull.Value;

                //判斷是否為指定的資料
                foreach (string value in strValue)
                {
                    if (row[strKeyField].Equals(value))
                    {
                        tbl.Rows.Add(row);
                        break;
                    }
                }
            }
            return tbl;
        }
        #endregion
    }
}