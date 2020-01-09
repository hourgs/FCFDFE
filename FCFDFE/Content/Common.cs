using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;
using System.IO;
using System.Configuration;
//using System.Data.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Globalization;
using FCFDFE.Entity.MPMSModel;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace FCFDFE.Content
{
    public class Common
    {
        GMEntities GME = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string[] strSuperUser = { "ADMINABMS", "123" }; //暫時供測試使用，此帳號為超級使用者

        #region 權限
        //取得權限-20180430 新增 Zoe
        //public DataTable getAuthData(string strUSER_ID, string strSystem, string strRole, string strAuth)
        //{
        //    DataTable dt = new DataTable();
        //    DateTime dateStart = DateTime.Today;
        //    var query =
        //        from accountAuth in GME.ACCOUNT_AUTH.AsEnumerable()
        //        where accountAuth.USER_ID.Equals(strUSER_ID)
        //        where accountAuth.END_DATE == null || accountAuth.END_DATE >= dateStart
        //        where accountAuth.IS_ENABLE.Equals("Y")
        //        select new
        //        {
        //            AA_SN = accountAuth.AA_SN,
        //            accountAuth.C_SN_SYS,
        //            accountAuth.C_SN_ROLE,
        //            accountAuth.C_SN_AUTH,
        //            C_SN_SUB = accountAuth.C_SN_SUB,
        //            accountAuth.IS_UPLOAD,
        //            accountAuth.IS_ENABLE,
        //            accountAuth.IS_PRO //1：已處理
        //            };
        //    if (strSystem != null)
        //        query = query.Where(t => t.C_SN_SYS.Equals(strSystem));
        //    if (strRole != null)
        //        query = query.Where(t => t.C_SN_ROLE.Equals(strRole));
        //    if (strAuth != null)
        //        query = query.Where(t => t.C_SN_AUTH.Equals(strAuth));
        //    dt = CommonStatic.LinqQueryToDataTable(query);
        //    return dt;
        //}
        //取得權限-20180430 新增 Zoe
        //public void getAuthData(string strUSER_ID, string strSystem, string strRole, string strAuth, out bool isEnable)
        //{
        //    DataTable dt = getAuthData(strUSER_ID, strSystem, strRole, strAuth);
        //    isEnable = false;
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        string strEnable = dr["IS_ENABLE"].ToString();
        //        if (strEnable.Equals("Y"))
        //            isEnable = true;
        //    }
        //}
        //取得權限-20180430 新增 Zoe
        //public void getAuthData(string strUSER_ID, string strSystem, string strRole, string strAuth, out bool isEnable, out bool isUpload)
        //{
        //    DataTable dt = getAuthData(strUSER_ID, strSystem, strRole, strAuth);
        //    isEnable = false;
        //    isUpload = false;
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        string strEnable = dr["IS_ENABLE"].ToString();
        //        if (strEnable.Equals("Y"))
        //            isEnable = true;
        //        string strUpload = dr["IS_UPLOAD"].ToString();
        //        if (strUpload.Equals("Y"))
        //            isUpload = false;
        //    }
        //}

        //取得權限-20180430 新增 Zoe
        //取得該頁面是否有權限可以閱讀
        public bool getAuth(Page thePage, bool isClose = false)
        {
            bool isEnable = false;
            if (thePage.Session["userid"] != null)
            {
                isEnable = true;
                //string strUSER_ID = thePage.Session["userid"].ToString();
                //if (strSuperUser.Contains(strUSER_ID)) //暫時供測試使用，此帳號為超級使用者
                //    isEnable = true;
                //else
                //{
                //    string strPageUrl = Path.GetFileName(thePage.Request.Path);
                //    DateTime dateStart = DateTime.Today;
                //    //搜尋該帳號所有可以閱讀該頁面之權限角色，若無資料則表示沒權限閱讀
                //    //限制條件：無日期限制或日期範圍內；啟用狀態為Y
                //    var query =
                //        from accountAuth in GME.ACCOUNT_AUTH
                //        where accountAuth.USER_ID.Equals(strUSER_ID)
                //        where accountAuth.END_DATE == null || accountAuth.END_DATE >= dateStart
                //        where accountAuth.IS_ENABLE.Equals("Y")
                //        join authMenu in GME.AUTH_MENU on accountAuth.C_SN_AUTH equals authMenu.GROUP_SN
                //        join menuPages in GME.MENU_PAGES on authMenu.MENU_SN equals menuPages.M_SN
                //        where menuPages.M_URL.Equals(strPageUrl)
                //        select new
                //        {
                //            accountAuth.C_SN_AUTH,
                //            menuPages.M_NAME,
                //            menuPages.M_URL,
                //            accountAuth.IS_UPLOAD
                //        };
                //    isEnable = query.Count() > 0;
                //    if (isEnable)
                //    {
                //    }
                //    else
                //        showMessageAuth(thePage, strPageUrl, isClose);
                //}
            }
            else
                MessageBoxShow(thePage, "尚未登入，請先登入！", "login", true);
            return isEnable;
        }
        //取得該頁面是否有權限可以閱讀，回傳上傳權限（多權限角色中，只要其中一個允許，則回傳true）
        public bool getAuth(Page thePage, out bool isUpload, bool isClose = false)
        {
            bool isEnable = false; isUpload = false;
            if (thePage.Session["userid"] != null)
            {
                isEnable = true; isUpload = true;
                //string strUSER_ID = thePage.Session["userid"].ToString();
                //if (strSuperUser.Contains(strUSER_ID)) //暫時供測試使用，此帳號為超級使用者
                //{
                //    isEnable = true;
                //    isUpload = true;
                //}
                //else
                //{
                //    string strPageUrl = Path.GetFileName(thePage.Request.Path);
                //    DateTime dateStart = DateTime.Today;
                //var query =
                //    from accountAuth in GME.ACCOUNT_AUTH
                //    where accountAuth.USER_ID.Equals(strUSER_ID)
                //    where accountAuth.END_DATE == null || accountAuth.END_DATE >= dateStart
                //    where accountAuth.IS_ENABLE.Equals("Y")
                //    join authMenu in GME.AUTH_MENU on accountAuth.C_SN_AUTH equals authMenu.GROUP_SN
                //    join menuPages in GME.MENU_PAGES on authMenu.MENU_SN equals menuPages.M_SN
                //    where menuPages.M_URL.Equals(strPageUrl)
                //    select new
                //    {
                //        accountAuth.C_SN_AUTH,
                //        menuPages.M_NAME,
                //        menuPages.M_URL,
                //        accountAuth.IS_UPLOAD
                //    };
                //    isEnable = query.Count() > 0;
                //    if (isEnable)
                //        foreach (var table in query)
                //        {
                //            bool isTemp = table.IS_UPLOAD.Equals("Y");
                //            if (isTemp)
                //                isUpload = isTemp;
                //        }
                //    else
                //        showMessageAuth(thePage, strPageUrl, isClose);
                //}
            }
            else
                MessageBoxShow(thePage, "尚未登入，請先登入！", "login", true);
            return isEnable;
        }
        public void showMessageAuth(Page thePage, string strPageUrl, bool isClose)
        {
            //string strURL = "~/pages/GM/AccountModify";
            //MessageBoxShow(thePage, "無權限閱讀 " + strPageName + " 頁面，請先申請並等待核定後再行修改", strURL, true);
            string strPageName = "此";
            MENU_PAGES menuPage = GME.MENU_PAGES.Where(table => table.M_URL.Equals(strPageUrl)).FirstOrDefault();
            if(menuPage!=null)
                strPageName = menuPage.M_NAME;
            string strMessage = "無權限閱讀 " + strPageName + " 頁面，請先申請並等待核定後再行修改";
            string strScript = 
                $@"<script language='javascript'>
                    alert('{ strMessage }');";
            if (isClose)
                strScript += "window.close();";
            else
                strScript += "history.back();";
            strScript += "</script>";
            thePage.ClientScript.RegisterStartupScript(thePage.GetType(), "MessageBox", strScript);
        }
        //取得目前使用者有的角色及權限清單
        public void getRoleAuthList(Page thePage, string strSystem, out string[] strRole, out string[] strAuth)
        {
            strRole = null;
            strAuth = null;
            if (thePage.Session["userid"] != null)
            {
                string strUSER_ID = thePage.Session["userid"].ToString();
                DateTime dateStart = DateTime.Today;
                //搜尋該帳號所有可以閱讀該頁面之權限角色，若無資料則表示沒權限閱讀
                //限制條件：無日期限制或日期範圍內；啟用狀態為Y
                var query =
                    from accountAuth in GME.ACCOUNT_AUTH
                    where accountAuth.USER_ID.Equals(strUSER_ID)
                    where accountAuth.END_DATE == null || accountAuth.END_DATE >= dateStart
                    where accountAuth.IS_ENABLE.Equals("Y")
                    select new
                    {
                        accountAuth.C_SN_SYS,
                        accountAuth.C_SN_ROLE,
                        accountAuth.C_SN_AUTH,
                        accountAuth.C_SN_SUB
                    };
                if (!string.IsNullOrEmpty(strSystem))
                    query = query.Where(t => t.C_SN_SYS.Equals(strSystem));
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                int intCount = dt.Rows.Count;
                strRole = new string[intCount];
                strAuth = new string[intCount];
                for (int i = 0;i<intCount;i++)
                {
                    DataRow dr = dt.Rows[i];
                    strRole[i] = dr["C_SN_ROLE"].ToString();
                    strAuth[i] = dr["C_SN_AUTH"].ToString();
                }
            }
            else
                MessageBoxShow(thePage, "尚未登入，請先登入！", "login", true);
        }
        public void getOldAuthList(Page thePage, out string[] strRole, out string[] strAuth)
        {
            strRole = null;
            strAuth = null;
            if (thePage.Session["userid"] != null)
            {
                string strUSER_ID = thePage.Session["userid"].ToString();
                //搜尋該帳號所有可以閱讀該頁面之權限角色，若無資料則表示沒權限閱讀
                //限制條件：無日期限制或日期範圍內；啟用狀態為Y
                var query =
                    from t5200_1 in mpms.TBM5200_1
                    where t5200_1.USER_ID.Equals(strUSER_ID)
                    where t5200_1.OVC_ENABLE.Equals("Y")
                    select new
                    {
                        t5200_1.C_SN_ROLE,
                        t5200_1.OVC_PRIV_LEVEL,
                        t5200_1.OVC_UPLOAD
                    };
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                int intCount = dt.Rows.Count;
                strRole = new string[intCount];
                strAuth = new string[intCount];
                for (int i = 0; i < intCount; i++)
                {
                    DataRow dr = dt.Rows[i];
                    strRole[i] = dr["C_SN_ROLE"].ToString();
                    strAuth[i] = dr["OVC_PRIV_LEVEL"].ToString();
                }
            }
            else
                MessageBoxShow(thePage, "尚未登入，請先登入！", "login", true);
        }
        //取得目前登入帳號之單位
        public string getAccountDEPT(Page thePage)
        {
            string strDEPT_SN = null;
            if (thePage.Session["userid"] != null)
            {
                string strUSER_ID = thePage.Session["userid"].ToString();
                ACCOUNT ac = GME.ACCOUNTs.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                if (ac != null)
                {
                    strDEPT_SN = ac.DEPT_SN;
                }
            }
            return strDEPT_SN;
        }
        //取得目前登入帳號之單位及單位名稱
        public void getAccountDEPTName(Page thePage, out string strDEPT_SN, out string strOVC_ONNAME)
        {
            strDEPT_SN = null;
            strOVC_ONNAME = ""; //名稱直接使用，預設為空字串
            if (thePage.Session["userid"] != null)
            {
                string strUSER_ID = thePage.Session["userid"].ToString();
                var query =
                    from account in GME.ACCOUNTs
                    where account.USER_ID.Equals(strUSER_ID)
                    join dept in GME.TBMDEPTs on account.DEPT_SN equals dept.OVC_DEPT_CDE into tempDept
                    from dept in tempDept.DefaultIfEmpty()
                    select new
                    {
                        account.DEPT_SN,
                        OVC_ONNAME = dept != null ? dept.OVC_ONNAME : ""
                    };
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                if (dt.Rows.Count>0)
                {
                    DataRow dr = dt.Rows[0];
                    strDEPT_SN = dr["DEPT_SN"].ToString();
                    strOVC_ONNAME = dr["OVC_ONNAME"].ToString();
                }
            }
        }
        #endregion

        #region 下拉式選單
        //下拉式選單初始化
        public void list_Init(ListControl list, bool isPleaseChoose)
        {//－下拉式物件、資料表、顯示名稱欄位、值欄位、排序、是否出現"請選擇"
            //先將下拉式選單清空
            list.Items.Clear();
            if (isPleaseChoose)
            {
                list.AppendDataBoundItems = true;
                list.Items.Add(new ListItem("請選擇", ""));
            }
        }
        //下拉式選單資料匯入－下拉式物件、"是"是否在前面、是否出現"請選擇"
        public void list_dataImportYN(ListControl list, bool isTure, bool isPleaseChoose)
        {
            //先將下拉式選單清空
            list.Items.Clear();
            if (isPleaseChoose)
            {
                list.AppendDataBoundItems = true;
                list.Items.Add(new ListItem("請選擇", ""));
            }
            if (isTure)
            {
                list.Items.Add(new ListItem("是", "Y"));
                list.Items.Add(new ListItem("否", "N"));
            }
            else
            {
                list.Items.Add(new ListItem("否", "N"));
                list.Items.Add(new ListItem("是", "Y"));
            }
        }
        //下拉式選單資料匯入－下拉式物件、"是"是否在前面、是否出現"請選擇"
        public void list_dataImportYN(ListControl list, bool isTure, string textFirst, string valueFirst, bool isShowFirst)
        {
            //先將下拉式選單清空
            list.Items.Clear();
            if (isShowFirst)
            {
                list.AppendDataBoundItems = true;
                list.Items.Add(new ListItem(textFirst, valueFirst));
            }
            if (isTure)
            {
                list.Items.Add(new ListItem("是", "Y"));
                list.Items.Add(new ListItem("否", "N"));
            }
            else
            {
                list.Items.Add(new ListItem("否", "N"));
                list.Items.Add(new ListItem("是", "Y"));
            }
        }
        //下拉式選單資料匯入－下拉式物件、"核准"是否在前面、是否出現"請選擇"
        public void list_dataImportAudit(ListControl list, bool isTure, bool isPleaseChoose)
        {
            //先將下拉式選單清空
            list.Items.Clear();
            if (isPleaseChoose)
            {
                list.AppendDataBoundItems = true;
                list.Items.Add(new ListItem("請選擇", ""));
            }
            if (isTure)
            {
                list.Items.Add(new ListItem("核准", "Y"));
                list.Items.Add(new ListItem("駁回", "N"));
            }
            else
            {
                list.Items.Add(new ListItem("核准", "N"));
                list.Items.Add(new ListItem("駁回", "Y"));
            }
        }
        //下拉式選單資料匯入－下拉式物件、資料表、顯示名稱欄位、值欄位、是否出現"請選擇"
        public void list_dataImport(ListControl list, DataTable dt, string textField, string valueField, bool isPleaseChoose)
        {
            //先將下拉式選單清空
            list.Items.Clear();
            if (isPleaseChoose)
            {
                list.AppendDataBoundItems = true;
                list.Items.Add(new ListItem("請選擇", ""));
            }

            list.DataSource = dt;
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataBind();
        }
        //下拉式選單資料匯入－下拉式物件、資料表、顯示名稱欄位、值欄位、第一項名稱、第一項值、是否顯示第一項
        public void list_dataImport(ListControl list, DataTable dt, string textField, string valueField, string textFirst, string valueFirst, bool isShowFirst)
        {
            //先將下拉式選單清空
            list.Items.Clear();
            if (isShowFirst)
            {
                list.AppendDataBoundItems = true;
                list.Items.Add(new ListItem(textFirst, valueFirst));
            }

            list.DataSource = dt;
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataBind();
        }
        //下拉式選單資料匯入－下拉式物件、顯示名稱陣列、值陣列、第一項名稱、第一項值、是否顯示第一項
        public void list_dataImport(ListControl list, string[] textFields, string[] valueFields, string textFirst, string valueFirst, bool isShowFirst)
        {   //下拉式物件顯示名稱陣列、值欄位陣列、"請選擇"字串名稱"請選擇"內容值、、是否出現"請選擇"
            list.Items.Clear(); //先將下拉式選單清空
            if (isShowFirst)
            {
                list.AppendDataBoundItems = true;
                list.Items.Add(new ListItem(textFirst, valueFirst));
            }

            DataTable dataTable = new DataTable();
            string textField = "textFidle", valueField = "valueField";
            int intRow = textFields.Length;
            dataTable.Columns.Add(textField);
            dataTable.Columns.Add(valueField);
            for (int i = 0; i < intRow; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr[textField] = textFields[i];
                dr[valueField] = valueFields[i];
                dataTable.Rows.Add(dr);
            }

            list.DataSource = dataTable;
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataBind();
        }
        //下拉式選單資料匯入－下拉式物件、資料表、顯示名稱欄位、值欄位、第一項名稱、第一項值、value及text連接字串、是否顯示第一項
        public void list_dataImportV(ListControl list, DataTable dt, string textField, string valueField, string textFirst, string valueFirst, string strConj, bool isShowFirst)
        {
            //先將下拉式選單清空
            list.Items.Clear();
            if (isShowFirst)
            {
                list.AppendDataBoundItems = true;
                list.Items.Add(new ListItem(textFirst, valueFirst));
            }
            string strFieldName = valueField + textField;
            if (!dt.Columns.Contains(strFieldName))
            {
                dt.Columns.Add(strFieldName);
                foreach (DataRow dr in dt.Rows)
                {
                    dr[strFieldName] = dr[valueField].ToString() + strConj + dr[textField].ToString();
                }
            }

            list.DataSource = dt;
            list.DataTextField = strFieldName;
            list.DataValueField = valueField;
            list.DataBind();
        }
        //下拉式選單資料匯入－下拉式物件、資料表、顯示名稱欄位、值欄位、是否出現"請選擇"
        public void list_dataImportV(ListControl list, DataTable dt, string textField, string valueField, bool isPleaseChoose)
        {
            //先將下拉式選單清空
            list.Items.Clear();
            if (isPleaseChoose)
            {
                list.AppendDataBoundItems = true;
                list.Items.Add(new ListItem("請選擇", ""));
            }
            string strFieldName = valueField + textField;
            if (!dt.Columns.Contains(strFieldName))
            {
                dt.Columns.Add(strFieldName);
                foreach (DataRow dr in dt.Rows)
                {
                    dr[strFieldName] = dr[valueField].ToString() + " " + dr[textField].ToString();
                }
            }
            isPleaseChoose = false;

            list.DataSource = dt;
            list.DataTextField = strFieldName;
            list.DataValueField = valueField;
            list.DataBind();
        }
        //下拉式選單資料匯入－下拉式物件、資料表、顯示名稱欄位、值欄位、value及text連接字串、是否出現"請選擇"
        public void list_dataImportV(ListControl list, DataTable dt, string textField, string valueField, string strConj, bool isPleaseChoose)
        {
            //先將下拉式選單清空
            list.Items.Clear();
            if (isPleaseChoose)
            {
                list.AppendDataBoundItems = true;
                list.Items.Add(new ListItem("請選擇", ""));
            }
            string strFieldName = valueField + textField;
            if (!dt.Columns.Contains(strFieldName))
            {
                dt.Columns.Add(strFieldName);
                foreach (DataRow dr in dt.Rows)
                {
                    dr[strFieldName] = dr[valueField].ToString() + strConj + dr[textField].ToString();
                }
            }

            list.DataSource = dt;
            list.DataTextField = strFieldName;
            list.DataValueField = valueField;
            list.DataBind();
        }
        //下拉式選單資料匯入－下拉式物件、顯示名稱陣列、值陣列、value及text連接字串、第一項名稱、第一項值、是否顯示第一項
        public void list_dataImportV(ListControl list, string[] textFields, string[] valueFields, string textFirst, string valueFirst, string strConj, bool isShowFirst)
        {
            list.Items.Clear(); //先將下拉式選單清空
            if (isShowFirst)
            {
                list.AppendDataBoundItems = true;
                list.Items.Add(new ListItem(textFirst, valueFirst));
            }

            DataTable dataTable = new DataTable();
            string textField = "textFidle", valueField = "valueField";
            int intRow = textFields.Length;
            dataTable.Columns.Add(textField);
            dataTable.Columns.Add(valueField);
            for (int i = 0; i < intRow; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr[textField] = valueFields[i] + strConj + textFields[i];
                dr[valueField] = valueFields[i];
                dataTable.Rows.Add(dr);
            }

            list.DataSource = dataTable;
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataBind();
        }
        //下拉式選單資料匯入－下拉式物件、資料表、顯示名稱欄位、值欄位、是否出現"請選擇"
        public void list_dataImport(ListControl list, object query, string textField, string valueField, bool isPleaseChoose)
        {
            //先將下拉式選單清空
            list.Items.Clear();
            if (isPleaseChoose)
            {
                list.AppendDataBoundItems = true;
                list.Items.Add(new ListItem("請選擇", ""));
            }

            list.DataSource = query;
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataBind();
        }
        //下拉式選單資料匯入年分預設15年－下拉式物件
        public void list_dataImportYear(ListControl list)
        {
            int intCount_Year = 15;
            //年度下拉選單
            //先將下拉式選單清空
            list.Items.Clear();

            int CalDateYear = Convert.ToInt16(DateTime.Now.ToString("yyyy")) - 1911;
            int num = CalDateYear;
            for (int i = 0; i < intCount_Year; i++)
            {
                list.Items.Add(Convert.ToString(num));
                num = num - 1;
            }
        }
        //匯入下拉式選單－日，以年、月下拉式選單為依據，是否為民國年
        public void list_dataImportDay(ListControl listYear, ListControl listMonth, ListControl listDay, bool isTaiwanYear = true)
        {
            string strDaySelected = listDay.SelectedValue; //紀錄原先選擇日
            int year, month, maxDay = 0;
            if (int.TryParse(listYear.SelectedValue, out year) && int.TryParse(listMonth.SelectedValue, out month))
            {
                if (isTaiwanYear) year += 1911; //轉換為西元年
                maxDay = DateTime.DaysInMonth(year, month); //取得該年月之最大日
            }
            list_dataImportNumber(listDay, 2, maxDay); //匯入下拉式選單－日，以計算出之該年月之最大日
            list_setValue(listDay, strDaySelected); //試著設定被選日，若無原項目，則保持選擇第一項
        }
        //匯入下拉式選單－數字型態，下拉式選單，位數(前方補0)，最大數字，最小數字
        public void list_dataImportNumber(ListControl list, int digit, int max, int min = 1, string textFirst = "請選擇", string valueFirst = "", bool isShowFirst = false)
        {
            list.Items.Clear();
            if (isShowFirst)
            {
                list.AppendDataBoundItems = true;
                list.Items.Add(new ListItem(textFirst, valueFirst));
            }
            for (int i = min; i <= max; i++)
            {
                string strText = i.ToString().PadLeft(digit, '0');
                list.Items.Add(strText);
            }
        }
        #endregion

        #region GridView
        // 20180430 新增 Zoe
        public bool GridView_dataImport(GridView gridView, DataTable dt)
        {
            bool hasRows = dt.Rows.Count > 0;
            if (hasRows)
            {
                gridView.DataSource = dt;
                gridView.DataBind();
            }
            else
                GridView_setEmpty(gridView, dt);
            return hasRows;
        }
        public bool GridView_dataImport(GridView gridView, DataTable dt, string[] strField)
        {
            bool hasRows = dt.Rows.Count > 0;
            if (hasRows)
            {
                gridView.DataSource = dt;
                gridView.DataBind();
            }
            else
                GridView_setEmpty(gridView, strField);
            return hasRows;
        }
        //Gridview新增空白列 20180430 新增 Zoe
        public bool GridView_setEmpty(GridView gridView, DataTable dt)
        {
            gridView.ShowHeaderWhenEmpty = true;
            gridView.EmptyDataText = "無資料";
            gridView.EmptyDataRowStyle.HorizontalAlign = HorizontalAlign.Center;
            gridView.EmptyDataRowStyle.VerticalAlign = VerticalAlign.Middle;
            gridView.DataSource = dt;
            gridView.DataBind();
            return false;
        }
        //Gridview新增空白列
        public bool GridView_setEmpty(GridView gridView, string[] field)
        {
            DataTable dt = new DataTable();
            foreach (string value in field)
            {
                dt.Columns.Add(value);
            }
            gridView.ShowHeaderWhenEmpty = true;
            gridView.EmptyDataText = "無資料";
            gridView.EmptyDataRowStyle.HorizontalAlign = HorizontalAlign.Center;
            gridView.EmptyDataRowStyle.VerticalAlign = VerticalAlign.Middle;
            gridView.DataSource = dt;
            gridView.DataBind();
            return false;

            //int intColumn = gridView.Rows[0].Cells.Count;
            //gridView.Rows[0].Cells.Clear();
            //gridView.Rows[0].Cells.Add(new TableCell());
            //gridView.Rows[0].Cells[0].ColumnSpan = intColumn;
            //gridView.Rows[0].Cells[0].Text = "無資料";
            //gridView.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            //gridView.Rows[0].VerticalAlign = VerticalAlign.Middle;
        }
        //public void GridView_
        //Gridview新增空白列
        public bool GridView_setEmpty(GridView gridView)
        {//－GridView
            DataTable dt = new DataTable();
            //20180430 修改 Zoe
            //foreach (DataControlField item in gridView.Columns)
            //{
            //    string strName = item.SortExpression;
            //    dt.Columns.Add(strName);
            //}

            //gridView.AutoGenerateColumns = true;
            //gridView.EmptyDataText = "無資料";
            //gridView.DataSource = dt;
            //gridView.DataBind();

            gridView.ShowHeaderWhenEmpty = true;
            gridView.EmptyDataText = "無資料";
            gridView.EmptyDataRowStyle.HorizontalAlign = HorizontalAlign.Center;
            gridView.EmptyDataRowStyle.VerticalAlign = VerticalAlign.Middle;
            gridView.DataSource = dt;
            gridView.DataBind();
            return false;
        }
        public void GridView_PreRenderInit(object sender, bool hasRows)
        {
            GridView theGridView = (GridView)sender;
            theGridView.EmptyDataRowStyle.HorizontalAlign = HorizontalAlign.Center;
            theGridView.EmptyDataRowStyle.VerticalAlign = VerticalAlign.Middle;

            if (hasRows)
            {
                theGridView.UseAccessibleHeader = true;
                theGridView.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        #endregion

        #region 資料庫
        //以SQL語法取得DataTable
        public DataTable getDataTableFromSelect(string strSQL)
        {
            DataTable dt = new DataTable();
            string connnn = ConfigurationManager.ConnectionStrings["SQLConnetString"].ConnectionString;
            using (OracleConnection conn = new OracleConnection(connnn))
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(strSQL, conn);
                cmd.BindByName = true;
                OracleDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                //while (dr.Read())
                //{
                //    this.TextBox1.Text = dr["user_name"].ToString();
                //}
                dr.Dispose();
                conn.Close();
            }
            
            return dt;
        }
        //以SQL語法取得DataTable
        public DataTable getDataTableFromSelect(string strSQL, string[] strParameterName, ArrayList aryValue)
        {
            DataTable dt = new DataTable();
            string connnn = ConfigurationManager.ConnectionStrings["SQLConnetString"].ConnectionString;
            using (OracleConnection conn = new OracleConnection(connnn))
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(strSQL, conn);
                cmd.BindByName = true;
                for (int i=0;i< strParameterName.Length; i++)
                {
                    string parameterName = strParameterName[i];
                    object value = aryValue[i];
                    cmd.Parameters.Add(parameterName, value);
                }
                OracleDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                //while (dr.Read())
                //{
                //    this.TextBox1.Text = dr["user_name"].ToString();
                //}
                dr.Dispose();
                conn.Close();
            }

            return dt;
        }
        //以SQL語法取得DataTable
        public DataTable getDataTableFromSelect(string strSQL, ArrayList aryParameterName, ArrayList aryValue)
        {
            DataTable dt = new DataTable();
            string connnn = ConfigurationManager.ConnectionStrings["SQLConnetString"].ConnectionString;
            using (OracleConnection conn = new OracleConnection(connnn))
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(strSQL, conn);
                cmd.BindByName = true;
                for (int i = 0; i < aryParameterName.Count; i++)
                {
                    string parameterName = aryParameterName[i].ToString();
                    object value = aryValue[i];
                    cmd.Parameters.Add(parameterName, value);
                }
                OracleDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                //while (dr.Read())
                //{
                //    this.TextBox1.Text = dr["user_name"].ToString();
                //}
                dr.Dispose();
                conn.Close();
            }

            return dt;
        }
        //取得單位資料表－含下屬單位
        public DataTable getDataTableDEPT_Includesubordinate(string strDEPT)
        {
            DataTable dt_DEPT = new DataTable();
            string[] strParameterName = { ":vOVC_DEPT_CDE" };
            ArrayList aryData = new ArrayList();
            aryData.Add(strDEPT);
            string strSQL = $@"
                select OVC_DEPT_CDE, OVC_ONNAME 
                from TBMDEPT 
                start with OVC_DEPT_CDE = { strParameterName[0] }
                connect by prior OVC_DEPT_CDE = OVC_TOP_DEPT and OVC_TOP_DEPT <> OVC_DEPT_CDE and OVC_ENABLE = '1' 
                order by OVC_DEPT_CDE";
            dt_DEPT = getDataTableFromSelect(strSQL, strParameterName, aryData);
            return dt_DEPT;
        }
        //取得匯出資料表，從資料庫來源，固定欄位數量
        public DataTable getDataTable_Export(DataTable dt_Source, string[] strFieldNames, string[] strFieldSqls, bool isShowNo, out int intCount_Data)
        {//dt_Source：資料庫來源資料表；strFieldNames：匯出欄位名稱陣列；strFieldSqls：資料庫來源欄位陣列；isShowNo：是否顯示項次欄位；intCount_Data：回傳筆數
            DataTable dt = new DataTable();
            intCount_Data = dt_Source.Rows.Count;
            if (intCount_Data > 0)
            {
                string strFieldNo = "項次";
                if (isShowNo) dt.Columns.Add(strFieldNo);
                int intCount_Field = strFieldNames.Length;
                for (int i = 0; i < intCount_Data; i++)
                {
                    DataRow dr = dt.NewRow(), dr_Source = dt_Source.Rows[i];
                    if (isShowNo) dr[strFieldNo] = (i + 1).ToString();
                    for (int j = 0; j < intCount_Field; j++)
                    {
                        string strFieldName = strFieldNames[j];
                        string strFieldSql = strFieldSqls[j];
                        if (i == 0) dt.Columns.Add(strFieldName); //新增欄位
                        if (dt_Source.Columns.Contains(strFieldSql))
                            dr[strFieldName] = dr_Source[strFieldSql];
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        #endregion

        #region 控制項
        // 清空控制項內容 (不限數量之Control[TextBox,Label,HiddenField..待新增])
        public void Controls_Clear(params Control[] controls)
        {
            foreach (Control theControl in controls)
            {
                if (theControl is TextBox)
                {
                    (theControl as TextBox).Text = "";
                }
                else if (theControl is Label)
                {
                    (theControl as Label).Text = "";
                }
                else if (theControl is HiddenField)
                {
                    (theControl as HiddenField).Value = "";
                }
            }
        }
        //清除Panel裡所有附ID之控制項
        public void Controls_Clear(Panel thePanel) // 20180502 新增 Zoe
        {
            foreach (Control theControl in thePanel.Controls)
            {
                if (theControl.ID != null)
                {
                    if (theControl is TextBox)
                        (theControl as TextBox).Text = "";
                    else if (theControl is Label)
                        (theControl as Label).Text = "";
                    else if (theControl is HiddenField)
                        (theControl as HiddenField).Value = "";
                    else if (theControl is DropDownList)
                        (theControl as DropDownList).SelectedValue = null;
                    else if (theControl is RadioButtonList)
                        (theControl as RadioButtonList).SelectedValue = null;
                    else if (theControl is CheckBoxList)
                        //CheckBoxList theCheckBoxList = (CheckBoxList)theControl;
                        foreach (ListItem theCheckBox in (theControl as CheckBoxList).Items)
                        {
                            theCheckBox.Selected = false;
                        }
                    else if (theControl is CheckBox)
                        (theControl as CheckBox).Checked = false;
                    else if (theControl is Panel)
                        Controls_Clear((theControl as Panel));
                }
            }
        }
        //清除Panel裡所有附ID之控制項－除了ID於字串陣列為例外
        public void Controls_Clear(Panel thePanel, params string[] strExceptID) // 20180502 新增 Zoe
        {
            foreach (Control theControl in thePanel.Controls)
            {
                string strControlID = theControl.ID;
                if (strControlID != null && !strExceptID.Contains(strControlID))
                {
                    if (theControl is TextBox)
                        (theControl as TextBox).Text = "";
                    else if (theControl is Label)
                        (theControl as Label).Text = "";
                    else if (theControl is HiddenField)
                        (theControl as HiddenField).Value = "";
                    else if (theControl is DropDownList)
                        (theControl as DropDownList).SelectedValue = null;
                    else if (theControl is RadioButtonList)
                        (theControl as RadioButtonList).SelectedValue = null;
                    else if (theControl is CheckBoxList)
                        //CheckBoxList theCheckBoxList = (CheckBoxList)theControl;
                        foreach (ListItem theCheckBox in (theControl as CheckBoxList).Items)
                        {
                            theCheckBox.Selected = false;
                        }
                    else if (theControl is CheckBox)
                        (theControl as CheckBox).Checked = false;
                    else if (theControl is Panel)
                        Controls_Clear((theControl as Panel));
                }
            }
        }
        // 判斷控制項不為null (不限數量之Control[TextBox,Label,HiddenField..待新增])
        public bool Controls_isExist(params Control[] controls)
        {
            bool isExist = true;
            foreach (Control theControl in controls)
            {
                if (theControl == null) isExist = false;
            }
            return isExist;
        }
        // 新增控制項屬性 (屬性名稱、屬性內容值、不限數量之Control[TextBox,Label..待新增])
        public void Controls_Attributes(string strName, string strValue, params Control[] controls)
        {
            foreach (Control theControl in controls)
            {
                if (theControl is TextBox)
                {
                    (theControl as TextBox).Attributes.Add(strName, strValue);
                }
                else if (theControl is Label)
                {
                    (theControl as Label).Attributes.Add(strName, strValue);
                }
                else if (theControl is Button)
                {
                    (theControl as Button).Attributes.Add(strName, strValue);
                }
                else if (theControl is ListControl)
                {
                    (theControl as ListControl).Attributes.Add(strName, strValue);
                }
            }
        }
        // 移除控制項屬性 (屬性名稱、不限數量之Control[TextBox,Label..待新增])
        public void Controls_Attributes(string strName, params Control[] controls)
        {
            foreach (Control theControl in controls)
            {
                if (theControl is TextBox)
                {
                    (theControl as TextBox).Attributes.Remove(strName);
                }
                else if (theControl is Label)
                {
                    (theControl as Label).Attributes.Remove(strName);
                }
                else if (theControl is Button)
                {
                    (theControl as Button).Attributes.Remove(strName);
                }
                else if (theControl is ListControl)
                {
                    (theControl as ListControl).Attributes.Remove(strName);
                }
            }
        }
        #endregion
        #region 控制項內容 20180503 新增 Zoe
        //取得物件內容－bool，如果不為布琳則回傳預設值
        public bool getValue_Boolean(string strValue, bool boolDefault)
        {
            bool boolValue = boolDefault;
            try
            {
                if (!strValue.Equals(string.Empty))
                    boolValue = Convert.ToBoolean(strValue);
            }
            catch { }
            return boolValue;
        }
        //取得物件內容－bool
        public string getValue_Boolean(string strSourceValue)
        {
            string strValue = null;
            try
            {
                if (!strSourceValue.Equals(string.Empty)) strValue = strSourceValue;
            }
            catch { }
            return strValue;
        }
        //取得物件內容－bool從單選鈕
        public string getValue_BooleanFromRadio(RadioButtonList theRadio)
        {
            string strValue = null;
            try
            {
                string strTemp = theRadio.SelectedValue;
                if (!strTemp.Equals(string.Empty)) strValue = strTemp;
            }
            catch { }
            return strValue;
        }
        //取得物件內容－字串，如果為空字串則回傳預設值
        public string getValue_String(string strSourceValue, string strDefault)
        {
            string strValue = strDefault;
            try
            {
                if (!strSourceValue.Equals(string.Empty)) strValue = strSourceValue;
            }
            catch { }
            return strValue;
        }
        //取得物件內容－字串，如果為True才要取得內容
        public string getValue_IfTrue(string strBool, string strSourceValue)
        {
            string strValue = null;
            try
            {
                if (strBool != null && Convert.ToBoolean(strBool) && !strSourceValue.Equals(string.Empty))
                    strValue = getValue_NotEmpty(strSourceValue);
            }
            catch { }
            return strValue;
        }
        //取得物件內容－字串，如果為空字串則回傳null
        public string getValue_NotEmpty(string strSourceValue)
        {
            string strValue = null;
            try
            {
                if (!strSourceValue.Equals(string.Empty)) strValue = strSourceValue;
            }
            catch { }
            return strValue;
        }
        //設定清單內容
        public void list_setValue(ListControl theList, string strValue)
        {
            try
            {
                theList.SelectedValue = strValue;
            }
            catch
            {
                theList.SelectedValue = null;
            }
        }
        #endregion

        #region 檢查判斷 20180501 新增 Zoe
        public void checkEmpty(string strValue, string strText, int intWay, ref string strMessage)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                string strWay;
                switch (intWay)
                {
                    case 1:
                    default:
                        strWay = "輸入";
                        break;
                    case 2:
                        strWay = "選擇";
                        break;
                }
                strMessage += "<p> 請" + strWay + " " + strText + " ！ </p>";
            }
        }
        public void checkLength(string strValue, string strText, int intLength, ref string strMessage)
        {
            if (strValue.Length != intLength)
                strMessage += "<p> " + strText + " 長度限制為" + intLength.ToString() + "！ </p>";
        }
        public void checkLengthMax(string strValue, string strText, int intLength, ref string strMessage)
        {
            if (strValue.Length > intLength)
                strMessage += "<p> " + strText + " 長度上限為" + intLength.ToString() + "！ </p>";
        }

        public void checkInt(string strValue, string strText, ref string strMessage)
        {
            int intTemp = 0;
            if (!string.IsNullOrEmpty(strValue) && !int.TryParse(strValue, out intTemp))
                strMessage += "<p> " + strText + " 須為數字！ </p>";
        }
        public bool checkInt(string strValue, string strText, ref string strMessage, out int intTemp) //回傳是否數字。若字串為null，不會新增錯誤描述
        {
            bool isInt = int.TryParse(strValue, out intTemp);
            if (!string.IsNullOrEmpty(strValue) && !isInt)
                strMessage += "<p> " + strText + " 須為數字！ </p>";
            return isInt;
        }

        public void checkDecimal(string strValue, string strText, ref string strMessage)
        {
            decimal decTemp;
            if (!string.IsNullOrEmpty(strValue) && !decimal.TryParse(strValue, out decTemp))
                strMessage += "<p> " + strText + " 須為數字！ </p>";
        }
        public bool checkDecimal(string strValue, string strText, ref string strMessage, out decimal decTemp) //回傳是否數字。若字串為null，不會新增錯誤描述
        {
            bool isDecimal = decimal.TryParse(strValue, out decTemp);
            if (!string.IsNullOrEmpty(strValue) && !isDecimal)
                strMessage += "<p> " + strText + " 須為數字！ </p>";
            return isDecimal;
        }

        public bool checkDateTime(string strValue, string strText, ref string strMessage, out DateTime date) //回傳是否日期。若字串為null，不會新增錯誤描述
        {
            bool isDateTime = DateTime.TryParse(strValue, out date);
            if (!string.IsNullOrEmpty(strValue) && !isDateTime)
                strMessage += "<p> " + strText + " 日期型態錯誤！ </p>";
            return isDateTime;
        }

        public void checkDirectory(params string[] strPath)
        {
            foreach (string thePath in strPath)
            {
                if (!Directory.Exists(thePath))
                    Directory.CreateDirectory(thePath);
            }
        }
        #endregion

        #region 訊息
        // Alert 平面訊息 (Pnael容器，訊息種類[info,danger,success,warning],標題,內文)
        public void AlertShow(Panel panelContent, string strKind, string strHeader, string strText)
        {
            string strIcon = "";
            switch (strKind)
            {   //info,error,success,warning
                case "info":
                    strIcon = "icon-info-sign";
                    break;
                case "danger":
                    strIcon = "icon-remove-sign";
                    break;
                case "success":
                    strIcon = "icon-ok-sign";
                    break;
                case "warning":
                    strIcon = "icon-exclamation-sign";
                    break;
            }


            string str_show = "";
            str_show += "<div class='alert alert-" + strKind + " alert-block fade in'>";
            str_show += "   <a href = '#' class='close close-sm' data-dismiss='alert'>&times;</a>";
            if (strHeader == "")
                str_show += "   <i class='" + strIcon + "'></i>";
            else
            {
                str_show += "   <h4>";
                str_show += "       <i class='" + strIcon + "'></i>";
                str_show += "       " + strHeader + "";
                str_show += "   </h4>";
            }
            str_show += "   " + strText + "";
            str_show += "</div>";
            str_show += "<div name='floatingmenu' class='floating-menu-" + strKind + "'><h3>" + strHeader + "</h3>" + strText + "</div>";
            panelContent.Controls.Add(new LiteralControl(str_show));
        }
        // 彈跳出視窗並跳轉頁面 Url使用與MasterPage之相對位置，且不需要.aspx
        public void MessageBoxShow(Page thePage, string strMessage, string strUrl, bool isRootDirectory)
        {
            if (isRootDirectory)
                strUrl = thePage.ResolveClientUrl("~/" + strUrl);
            string strScript =
                $@"<script language='javascript'>
                    alert('{ strMessage }');
                    location.href='{ strUrl }';
                </script>";
            thePage.ClientScript.RegisterStartupScript(thePage.GetType(), "MessageBox", strScript);
        }
        // 彈跳出視窗並關閉頁面
        public void MessageBoxShow_Close(Page thePage, string strMessage)
        {
            string strScript =
                $@"<script language='javascript'>
                    alert('{ strMessage }');
                    window.close();
                </script>";
            thePage.ClientScript.RegisterStartupScript(thePage.GetType(), "MessageBox", strScript);
        }
        // 彈跳出選擇視窗，如是則跳轉頁面 Url使用與MasterPage之相對位置，且不需要.aspx
        public void DialogBoxShow(Page thePage, string strMessage, string strUrl, bool isRootDirectory)
        {
            if (isRootDirectory)
                strUrl = thePage.ResolveClientUrl("~/" + strUrl);
            string strScript =
                $@"<script language='javascript'>
                    if(confirm('{ strMessage }'))
                        location.href='{ strUrl }';
                </script>";
            thePage.ClientScript.RegisterStartupScript(thePage.GetType(), "MessageBox", strScript);
        }
        #endregion

        #region 計算公式
        //原有日期加上工作日
        public DateTime WorkingDayPlus(DateTime dateOrigin, int intWordingDay)
        {
            DateTime dateNew = dateOrigin;
            while (intWordingDay-- > 0)
            {
                dateNew = dateNew.AddDays(1);
                while (getIsWeekDay(dateNew))
                    dateNew = dateNew.AddDays(1);
            }
            return dateNew;
        }
        //計算兩個日期中有幾個工作日
        public int WorkingDayCalculation(DateTime dateFirst, DateTime dateSecond)
        {
            int intDay = 0;
            DateTime dateTemp = dateFirst;
            while (dateTemp != dateSecond)
            {
                if(dateTemp < dateSecond)
                {
                    dateTemp = dateTemp.AddDays(1);
                    intDay++;
                    while (dateTemp != dateSecond && getIsWeekDay(dateTemp))
                        dateTemp = dateTemp.AddDays(1);
                }
               else if (dateTemp > dateSecond)
                {
                    dateTemp = dateTemp.AddDays(-1);
                    intDay--;
                    while (dateTemp != dateSecond && getIsWeekDay(dateTemp))
                        dateTemp = dateTemp.AddDays(-1);
                }
            }
            return intDay;
        }
        private bool getIsWeekDay(DateTime date)
        {
            bool isWeekDay = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
            return isWeekDay;
        }

        //原有日期日曆天日
        public DateTime CalendarDayPlus(DateTime dateOrigin, int intCalendarDay)
        {
            DateTime dateNew = dateOrigin.AddDays(intCalendarDay);
            return dateNew;
        }
        //計算兩個日期中有幾個日曆天
        public int CalendarDayCalculation(DateTime dateFirst, DateTime dateSecond)
        {
            TimeSpan timeSpan = dateSecond.Subtract(dateFirst);
            int intDay = timeSpan.Days;
            return intDay;
        }

        //取得民國日期－西元年、轉出格式(空字串則為{0}{1}{2})
        public string getTaiwanDate(DateTime sourceDateTime, string strFormat = "{0}{1}{2}")
        {
            string strTaiwanDate = "";
            if (DateTime.Compare(sourceDateTime, Convert.ToDateTime("1912/01/01")) > 0) //判斷須為民國存在年間
            {
                TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
                if (strFormat.Length == 0) strFormat = "{0}{1}{2}";
                string strYear = taiwanCalendar.GetYear(sourceDateTime).ToString().PadLeft(3, '0');
                string strMonth = sourceDateTime.Month.ToString().PadLeft(2, '0');
                string strDay = sourceDateTime.Day.ToString().PadLeft(2, '0');
                strTaiwanDate = string.Format(strFormat, strYear, strMonth, strDay);
            }
            return strTaiwanDate;
        }
        //取得民國日期－西元年、轉出格式(空字串則為{0}{1}{2})
        public string getTaiwanDate(string sourceDateTime, string strFormat)
        {
            string strTaiwanDate = "";
            DateTime dateSource;
            if (DateTime.TryParse(sourceDateTime, out dateSource))
                if (DateTime.Compare(dateSource, Convert.ToDateTime("1912/01/01")) > 0) //判斷須為民國存在年間
                {
                    TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
                    if (strFormat.Length == 0) strFormat = "{0}{1}{2}";
                    string strYear = taiwanCalendar.GetYear(dateSource).ToString().PadLeft(3, '0');
                    string strMonth = dateSource.Month.ToString().PadLeft(2, '0');
                    string strDay = dateSource.Day.ToString().PadLeft(2, '0');
                    strTaiwanDate = string.Format(strFormat, strYear, strMonth, strDay);
                }
            return strTaiwanDate;
        }
        //取得民國日期－西元年、轉出格式(空字串則為{0}{1}{2})，分別指定年月日位數，不足補0，超過取尾數
        public string getTaiwanDate_Assign(string sourceDateTime, string strFormat, int yearDigit = 3, int monthDigit = 2, int dayDigit = 2)
        {
            string strTaiwanDate = "";
            DateTime dateSource;
            if (DateTime.TryParse(sourceDateTime, out dateSource))
                if (DateTime.Compare(dateSource, Convert.ToDateTime("1912/01/01")) > 0) //判斷須為民國存在年間
                {
                    TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
                    if (strFormat.Length == 0) strFormat = "{0}{1}{2}";

                    string strYear = taiwanCalendar.GetYear(dateSource).ToString().PadLeft(yearDigit, '0');
                    if (strYear.Length > yearDigit)
                        strYear = strYear.Substring(strYear.Length - yearDigit, yearDigit);

                    string strMonth = dateSource.Month.ToString().PadLeft(monthDigit, '0');
                    if (strMonth.Length > monthDigit)
                        strMonth = strMonth.Substring(strMonth.Length - monthDigit, monthDigit);

                    string strDay = dateSource.Day.ToString().PadLeft(dayDigit, '0');
                    if (strDay.Length > dayDigit)
                        strDay = strDay.Substring(strDay.Length - dayDigit, dayDigit);

                    strTaiwanDate = string.Format(strFormat, strYear, strMonth, strDay);
                }
            return strTaiwanDate;
        }

        //取得民國年－西元年、轉出格式(空字串則為{0}{1}{2})
        public int getTaiwanYear(DateTime sourceDateTime)
        {
            int intYear = 0;
            if (DateTime.Compare(sourceDateTime, Convert.ToDateTime("1912/01/01")) > 0) //判斷須為民國存在年間
            {
                TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
                intYear = taiwanCalendar.GetYear(sourceDateTime);
            }
            return intYear;
        }
        //取得日期
        public string getDateTime(object sourceDateTime, string strDateFormat = null)
        {
            string date = "";
            if (strDateFormat == null) strDateFormat = Variable.strDateFormat;
            if (sourceDateTime != null && DateTime.TryParse(sourceDateTime.ToString(), out DateTime dateTemp))
                date = dateTemp.ToString(strDateFormat);
            return date;
        }
        public string getUserName(object userId)
        {
            string strUserName = "-";
            if (userId != null)
            {
                string strUserId = userId.ToString();
                ACCOUNT account = GME.ACCOUNTs.Where(table => table.USER_ID.Equals(strUserId)).FirstOrDefault();
                strUserName = account != null ? account.USER_NAME : "無此帳號";
            }
            return strUserName;
        }
        public string getDeptName(object deptCde)
        {
            string strOVC_ONNAME = "-";
            if (deptCde != null)
            {
                string strOVC_DEPT_CDE = deptCde.ToString();
                TBMDEPT dept = GME.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(strOVC_DEPT_CDE)).FirstOrDefault();
                strOVC_ONNAME = dept != null ? dept.OVC_ONNAME : "無此單位";
            }
            return strOVC_ONNAME;
        }

        //網址參數加密
        public string getEncryption(string strOriginal)
        {
            string strResult = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOriginal));
            return strResult;
        }
        //設定整段網址參數(加密後)
        public void setQueryString(ref string strQuerySring, string strVariable, string strValue, bool isEncryption)
        {
            if (strValue != null)
            {
                if (strQuerySring.Equals(string.Empty)) strQuerySring += "?"; else strQuerySring += "&";
                if (isEncryption) strValue = getEncryption(strValue);
                strQuerySring += $"{ strVariable }={ strValue }";
            }
        }
        //設定整段網址參數(加密後)
        public void setQueryString(ref string strQuerySring, string strVariable, object objValue, bool isEncryption)
        {
            if (objValue != null)
            {
                string strValue = objValue.ToString();
                if (strQuerySring.Equals(string.Empty)) strQuerySring += "?"; else strQuerySring += "&";
                if (isEncryption) strValue = getEncryption(strValue);
                strQuerySring += $"{ strVariable }={ strValue }";
            }
        }
        //取得網址參數
        //public bool getQueryString(Page thePage, string strVariable, out string strValue)
        //{
        //    bool isExist = false;
        //    strValue = "";
        //    if (thePage.Request.QueryString[strVariable] != null)
        //    {
        //        strValue = thePage.Request.QueryString[strVariable].ToString();
        //        isExist = true;
        //    }
        //    return isExist;
        //}
        //取得網址參數，是否直接解密
        public bool getQueryString(Page thePage, string strVariable, out string strValue, bool isDecryption = false)
        {
            bool isExist = false;
            strValue = "";
            if (thePage.Request.QueryString[strVariable] != null)
            {
                strValue = thePage.Request.QueryString[strVariable].ToString();
                if (isDecryption)
                {
                    strValue = getDecryption(strValue.Replace(" ", "+"));
                }
                isExist = true;
            }
            return isExist;
        }
        //網址參數解密
        public string getDecryption(string strOriginal)
        {
            string strResult = System.Text.Encoding.Default.GetString(Convert.FromBase64String(strOriginal));
            return strResult;
        }
        //public bool getDecryption(string strOriginal, out string strResult)
        //{
        //    bool isSuccess = false;
        //    strResult = null;
        //    if (!string.IsNullOrEmpty(strOriginal))
        //    {
        //        try
        //        {
        //            strResult = System.Text.Encoding.Default.GetString(Convert.FromBase64String(strOriginal));
        //            isSuccess = true;
        //        }
        //        catch { }
        //    }
        //    return isSuccess;
        //}
        #endregion
        public void syslog_add(string userid, string ip, string command, Page page, string act)
        {
            ALLSYS_LOG syslog = new ALLSYS_LOG();
            syslog.AS_USER_ID = userid;
            syslog.AS_DATE = DateTime.Now;
            syslog.AS_IP = ip;
            syslog.AS_COMMAND = command;
            syslog.AS_PAGE = Path.GetFileName(page.Request.Path);
            syslog.AS_ACT = act;
            syslog.AS_SN = Guid.NewGuid();

            GME.ALLSYS_LOG.Add(syslog);
            GME.SaveChanges();
        }

        public void getPageNode(Page thePage, ref string modelName, ref string pageName)
        {
            int intLength = 0;
            pageName = Path.GetFileName(thePage.Request.Path);
            if (pageName.Contains("MTS"))
                intLength = pageName.IndexOf("_") + 3;
            else
                intLength = pageName.IndexOf("_") + 2;
            if (intLength > 1)
                modelName = pageName.Substring(0, intLength);
            else
                modelName = pageName;
            modelName = "#li_" + modelName;
            pageName = "#li_" + pageName;
        }

        public void getDataTable(ref DataTable dt, string[] strText, string[] strValue)
        {
            dt = new DataTable();
            dt.Columns.Add("Text");
            dt.Columns.Add("Value");
            for (int i = 0; i < strText.Length; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Text"] = strText[i];
                dr["Value"] = strValue[i];
                dt.Rows.Add(dr);
            }
        }
        #region 身分檢驗
        // 身份證檢驗
        public bool CheckPersonalID(string strPersonalID)
        {
            if (string.IsNullOrEmpty(strPersonalID))
                return false;   //沒有輸入，回傳 ID 錯誤             
            strPersonalID = strPersonalID.ToUpper(); //英文字母轉大寫            
            var regIDFormat = new Regex("^[A-Z]{1}[0-9]{9}$");
            if (!regIDFormat.IsMatch(strPersonalID))   //先確認格式是否符合              
                return false;   //Regular Expression 驗證失敗，回傳 ID 錯誤               
            int[] intSeed = new int[10];       //除了檢查碼外每個數字的存放空間             
            //A=10 B=11 C=12 D=13 E=14 F=15 G=16 H=17 J=18 K=19 L=20 M=21 N=22             
            //P=23 Q=24 R=25 S=26 T=27 U=28 V=29 X=30 Y=31 W=32  Z=33 I=34 O=35             
            string[] charMapping = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N"
                                    , "P", "Q", "R", "S", "T", "U", "V", "X", "Y", "W", "Z", "I", "O" };

            string strFirst = strPersonalID.Substring(0, 1);
            for (int intMapIndex = 0; intMapIndex < charMapping.Length; intMapIndex++)
            {
                if (charMapping[intMapIndex] == strFirst)
                {
                    intMapIndex += 10;
                    intSeed[0] = intMapIndex / 10;       //10進制的高位元放入存放空間                     
                    intSeed[1] = (intMapIndex % 10) * 9; //10進制的低位元*9後放入存放空間                     
                    break;
                }
            }
            for (int intMapIndex = 2; intMapIndex < 10; intMapIndex++)
            {   //將剩餘數字乘上權數後放入存放空間                
                intSeed[intMapIndex] = Convert.ToInt32(strPersonalID.Substring(intMapIndex - 1, 1)) * (10 - intMapIndex);
            }             //檢查是否符合檢查規則，10減存放空間所有數字和除以10的餘數的個位數字是否等於檢查碼             
            //(10 - ((seed[0] + .... + seed[9]) % 10)) % 10 == 身分證字號的最後一碼             
            return (10 - (intSeed.Sum() % 10)) % 10 == Convert.ToInt32(strPersonalID.Substring(9, 1));
        }
        // 關閉頁面 可判斷是否出現彈跳視窗
        #endregion
        public void ClosePage(Panel panelContent, string strMessage, bool isShowMessage)
        {
            string strScript = "<script language='javascript'>";
            if (isShowMessage)
                strScript += "alert('" + strMessage + "');";
            strScript += "window.close();" +
                        "</script>";
            panelContent.Controls.Add(new LiteralControl(strScript));
        }
        #region 小寫金額轉大寫(國字)金額
        //小寫金額轉大寫(國字)金額(string in string out)
        public string MoneyToChinese(string LowerMoney)
        {
            string functionReturnValue = null;
            bool IsNegative = false; // 判斷是否負數
            if (LowerMoney.Trim().Substring(0, 1) == "-")
            {
                // 負數轉正
                LowerMoney = LowerMoney.Trim().Remove(0, 1);
                IsNegative = true;
            }
            string strLower = null;
            string strUpart = null;
            string strUpper = null;
            int iTemp = 0;
            // 保留兩位小數 123.489→123.49　　123.4→123.4
            LowerMoney = Math.Round(double.Parse(LowerMoney), 2).ToString();
            if (LowerMoney.IndexOf(".") > 0)
            {
                if (LowerMoney.IndexOf(".") == LowerMoney.Length - 2)
                {
                    LowerMoney = LowerMoney + "0";
                }
            }
            else
            {
                LowerMoney = LowerMoney + ".00";
            }
            strLower = LowerMoney;
            iTemp = 1;
            strUpper = "";
            while (iTemp <= strLower.Length)
            {
                switch (strLower.Substring(strLower.Length - iTemp, 1))
                {
                    case ".":
                        strUpart = "元";
                        break;
                    case "0":
                        strUpart = "零";
                        break;
                    case "1":
                        strUpart = "壹";
                        break;
                    case "2":
                        strUpart = "貳";
                        break;
                    case "3":
                        strUpart = "參";
                        break;
                    case "4":
                        strUpart = "肆";
                        break;
                    case "5":
                        strUpart = "伍";
                        break;
                    case "6":
                        strUpart = "陸";
                        break;
                    case "7":
                        strUpart = "柒";
                        break;
                    case "8":
                        strUpart = "捌";
                        break;
                    case "9":
                        strUpart = "玖";
                        break;
                }

                switch (iTemp)
                {
                    case 1:
                        strUpart = strUpart + "分";
                        break;
                    case 2:
                        strUpart = strUpart + "角";
                        break;
                    case 3:
                        strUpart = strUpart + "";
                        break;
                    case 4:
                        strUpart = strUpart + "";
                        break;
                    case 5:
                        strUpart = strUpart + "拾";
                        break;
                    case 6:
                        strUpart = strUpart + "佰";
                        break;
                    case 7:
                        strUpart = strUpart + "仟";
                        break;
                    case 8:
                        strUpart = strUpart + "萬";
                        break;
                    case 9:
                        strUpart = strUpart + "拾";
                        break;
                    case 10:
                        strUpart = strUpart + "佰";
                        break;
                    case 11:
                        strUpart = strUpart + "仟";
                        break;
                    case 12:
                        strUpart = strUpart + "億";
                        break;
                    case 13:
                        strUpart = strUpart + "拾";
                        break;
                    case 14:
                        strUpart = strUpart + "佰";
                        break;
                    case 15:
                        strUpart = strUpart + "仟";
                        break;
                    case 16:
                        strUpart = strUpart + "兆";
                        break;
                    default:
                        strUpart = strUpart + "";
                        break;
                }

                strUpper = strUpart + strUpper;
                iTemp = iTemp + 1;
            }

            strUpper = strUpper.Replace("零拾", "零");
            strUpper = strUpper.Replace("零佰", "零");
            strUpper = strUpper.Replace("零仟", "零");
            strUpper = strUpper.Replace("零零零", "零");
            strUpper = strUpper.Replace("零零", "零");
            strUpper = strUpper.Replace("零角零分", "整");
            strUpper = strUpper.Replace("零分", "整");
            strUpper = strUpper.Replace("零角", "零");
            strUpper = strUpper.Replace("零兆零億零萬零元", "兆元");
            strUpper = strUpper.Replace("零億零萬零元", "億元");
            strUpper = strUpper.Replace("億零萬零元", "億元");
            strUpper = strUpper.Replace("零億零萬", "億");
            strUpper = strUpper.Replace("零萬零元", "萬元");
            strUpper = strUpper.Replace("零億", "億");
            strUpper = strUpper.Replace("零萬", "萬");
            strUpper = strUpper.Replace("零元", "元");
            strUpper = strUpper.Replace("零零", "零");

            // 壹元以下金額處理
            if (strUpper.Substring(0, 1) == "元")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "零")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "角")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "分")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "整")
            {
                strUpper = "零元整";
            }
            functionReturnValue = strUpper;

            if (IsNegative == true)
            {
                return "負" + functionReturnValue;
            }
            else
            {
                return functionReturnValue;
            }
        }
        #endregion
        #region 直接下載
        //直接下載檔案
        public void DownloadFile(Page thePage, string strFileName, MemoryStream Memory)
        {
            thePage.Response.Clear();//瀏覽器上顯示
            strFileName = HttpUtility.UrlEncode(strFileName);
            thePage.Response.AddHeader("Content-disposition", "attachment; filename=" + strFileName);
            thePage.Response.ContentType = "application/octet-stream";
            thePage.Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            thePage.Response.OutputStream.Flush();
            thePage.Response.OutputStream.Close();
            thePage.Response.Flush();
            thePage.Response.End();
        }
        public void DownloadFile(Page thePage, string strFilePath)
        {
            FileInfo file = new FileInfo(strFilePath);
            string strFileName = HttpUtility.UrlEncode(file.Name);
            thePage.Response.Clear();
            thePage.Response.AppendHeader("Content-Disposition", "attachment; filename=" + strFileName);
            thePage.Response.ContentType = "application/octet-stream";
            thePage.Response.WriteFile(file.FullName);
            thePage.Response.OutputStream.Flush();
            thePage.Response.OutputStream.Close();
            thePage.Response.Flush();
            thePage.Response.End();
        }
        #endregion
        #region 轉檔+下載
        //word轉pdf
        public void WordToPDF(Page thisPage, string FilePath, string FileTemp, string FileName)
        {
            File.Delete(FileTemp);
            // word 檔案位置
            string sourcedocx = FilePath;
            // PDF 儲存位置
            // string targetpdf =  @"C:\Users\linon\Downloads\ddd.pdf";

            //建立 word application instance
            Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
            //開啟 word 檔案
            var wordDocument = appWord.Documents.Open(sourcedocx);

            //匯出為 pdf
            wordDocument.ExportAsFixedFormat(FileTemp, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);

            //關閉 word 檔
            wordDocument.Close();
            //結束 word
            appWord.Quit();

            FileInfo file = new FileInfo(FileTemp);
            string filepath = FileName;
            string fileName = HttpUtility.UrlEncode(filepath);
            thisPage.Response.Clear();
            thisPage.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            thisPage.Response.ContentType = "application/octet-stream";
            thisPage.Response.WriteFile(file.FullName);
            thisPage.Response.OutputStream.Flush();
            thisPage.Response.OutputStream.Close();
            thisPage.Response.Flush();
            File.Delete(FilePath);
            thisPage.Response.End();
        }

        //doc副檔名轉odt副檔名
        public void WordToOdt(Page thisPage, string FromPath, string TargetPath, string FileName)
        {
            File.Delete(TargetPath);
            var WordApp = new Microsoft.Office.Interop.Word.Application();
            var workbooks = WordApp.Documents;
            var doc = workbooks.Open(FromPath);

            //  Microsoft.Office.Interop.Word.Document doc = WordApp.Documents.Open(FromPath);
            try
            {
                doc.SaveAs2(TargetPath, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatOpenDocumentText);

                doc.Close();
                WordApp.Visible = false;
                WordApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(WordApp);

                doc = null;
                workbooks = null;
                WordApp = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                WordApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(WordApp);

                doc = null;
                workbooks = null;
                WordApp = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            byte[] renderedBytes = null;
            var buffer = new byte[16 * 1024];
            using (var stream = new FileStream(TargetPath, FileMode.Open))
            {
                var memoryStream = new MemoryStream();
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memoryStream.Write(buffer, 0, read);
                        renderedBytes = memoryStream.ToArray();
                    }
                }

                string filepath = FileName;
                string fileName = HttpUtility.UrlEncode(filepath);
                thisPage.Response.Clear();
                thisPage.Response.ContentType = "Application/application/vnd.oasis.opendocument.text";
                thisPage.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                thisPage.Response.BinaryWrite(renderedBytes);
                thisPage.Response.Flush();
                thisPage.Response.Close();
                File.Delete(FromPath);
                thisPage.Response.End();
            }
        }

        public void WordToOdt(HttpContext context, string FromPath, string TargetPath, string FileName)
        {
            File.Delete(TargetPath);
            var WordApp = new Microsoft.Office.Interop.Word.Application();
            var workbooks = WordApp.Documents;
            var doc = workbooks.Open(FromPath);

            //  Microsoft.Office.Interop.Word.Document doc = WordApp.Documents.Open(FromPath);
            try
            {
                doc.SaveAs2(TargetPath, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatOpenDocumentText);

                doc.Close();
                WordApp.Visible = false;
                WordApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(WordApp);

                doc = null;
                workbooks = null;
                WordApp = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                WordApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(WordApp);

                doc = null;
                workbooks = null;
                WordApp = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            byte[] renderedBytes = null;
            var buffer = new byte[16 * 1024];
            using (var stream = new FileStream(TargetPath, FileMode.Open))
            {
                var memoryStream = new MemoryStream();
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memoryStream.Write(buffer, 0, read);
                        renderedBytes = memoryStream.ToArray();
                    }
                }

                string filepath = FileName;
                string fileName = HttpUtility.UrlEncode(filepath);
                context.Response.Clear();
                context.Response.ContentType = "Application/application/vnd.oasis.opendocument.text";
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                context.Response.BinaryWrite(renderedBytes);
                context.Response.Flush();
                context.Response.Close();
                File.Delete(FromPath);
                context.Response.End();
            }
        }
        #endregion

        #region 寄信
        public void SendOtherMail(string Content, string pRecipient)
        {

            try
            {
                string SenderPW = "";
                var query = GME.TBM1407.Where(x => x.OVC_PHR_CATE == "SP" && x.OVC_PHR_ID== "02").FirstOrDefault();
                if (query != null)
                    SenderPW = query.OVC_PHR_DESC.ToString();

                string SenderGmail = "mpcmts@webmail.com.tw";



                MailMessage Message = new MailMessage();//MailMessage(寄信者, 收信者) 
                Message.From = new MailAddress(SenderGmail); 
                // 新增收件人
                Message.To.Add(pRecipient);
                Message.IsBodyHtml = true;
                Message.BodyEncoding = System.Text.Encoding.UTF8;//E-mail編碼 

                Message.Subject = DateTime.Now.ToString();//E-mail主旨 
                Message.Body = Content;//E-mail內容 

                SmtpClient client = new SmtpClient("webmail.mil.tw", 25);//使用指定的 SMTP 伺服器和連接埠傳送電子郵件
                client.Credentials = new NetworkCredential(SenderGmail, SenderPW);//寄信者信箱帳密
                client.EnableSsl = true;//EnableSsl 屬性會指定是否使用 SSL 來存取指定的 SMTP 郵件伺服器。

                client.Send(Message);
            }
            catch
            {

                //  Response.Write("<script>alert('登入錯誤過多次，請聯絡資訊人員')</script>");
            }
        }

        public void contactMail(string xFrom, string xName, string xSubject, string xMessage)
        {
            MailMessage xmail = new MailMessage();  //信件本體宣告
            //xFrom = Server.HtmlEncode(xFrom);  // 對使用者輸入都先做 HtmlEncode，避免隱碼攻擊
            //xName = Server.HtmlEncode(xName);
            //xSubject = Server.HtmlEncode(xSubject);

            string SenderPW = "";
            var query = GME.TBM1407.Where(x => x.OVC_PHR_CATE == "SP" && x.OVC_PHR_ID == "02").FirstOrDefault();
            if (query != null)
                SenderPW = query.OVC_PHR_DESC.ToString();

            string Sendermail = "mpcmts@webmail.com.tw";

            xmail.From = new MailAddress(Sendermail, "採購管理系統自動回復郵件");
            xmail.To.Add(new MailAddress(xFrom, xName));
            //xmail.CC.Add(new MailAddress("", ""));  //其它收件人
            //xmail.Bcc.Add(new MailAddress("", "")); //密本收件人
            xmail.Priority = MailPriority.Normal;  //優先等級
            xmail.Subject = xSubject;  //主旨
            //xmail.Body = Server.HtmlEncode(xMessage);  //Email 內容
            xmail.Body =xMessage;  //Email 內容
            xmail.IsBodyHtml = true;  // 設定Email 內容為HTML格式
            //xmail.BodyEncoding = System.Text.Encoding.GetEncoding(950);     
            //設定編碼為BIG 5
            xmail.BodyEncoding = System.Text.Encoding.UTF8;
            //xmail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");   
            //設定編碼為 utf-8
            // 設定SMTP伺服器
            SmtpClient smtpServer = new SmtpClient();
            smtpServer.Credentials = new System.Net.NetworkCredential(Sendermail, SenderPW);
            smtpServer.Port = 25;  // smtpServer port = 25
            smtpServer.Host = "smtp.gmail.com";
            smtpServer.EnableSsl = true;  // SSL 要看你的 mail server，像 gmail 就必須啟用
            smtpServer.Send(xmail);
            
        }
        #endregion
    }
}