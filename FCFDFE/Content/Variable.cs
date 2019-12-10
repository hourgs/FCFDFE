namespace FCFDFE.Content
{
    public class Variable
    {
        public static string strImagePath = "~/images/";
        public static string strDateFormat = "yyyy-MM-dd";

        #region 清單項目
        public static string[] listPurchLevelDistance_Text = { "未達公告金額", "公告金額以上未達查核金額", "查核金額以上未達巨額", "巨額" };
        public static string[] listPurchLevelDistance_Value = { "1", "2", "3", "4" };
        #endregion

        #region Excel格式
        public static string strExcleFormatString = "@";
        public static string strExcleFormatInt = "0_";
        public static string strExcleFormatMoney = "#,##0";
        public static string strExcleFormatMoney2 = "#,##0.00";
        #endregion

        //Excel暫時
        public static string OLEDBConnStr(string DataSource, int IMEX)
        {
            string str = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                    "Data Source='" + DataSource + "';" +
                    "Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=" + IMEX + ";Persist Security Info=False'";
            return str;
        }
    }
}