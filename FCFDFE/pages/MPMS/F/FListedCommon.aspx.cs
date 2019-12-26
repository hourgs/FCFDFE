using FCFDFE.Content;
using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MPMS.F
{
    public partial class FListedCommon : Page
    {
        Common FCommon = new Common();
        //GMEntities gm = new GMEntities();
        public string strMenuName = "", strMenuNameItem = "";
        ArrayList aryFieldName = new ArrayList(), aryFieldSql = new ArrayList();//, aryFieldWidth = new ArrayList();
        string strYear, strMethod, strMethodName;
        string strFieldSql_Dept = "OVC_ONNAME", strFieldSql_Sum = "Sum";
        double doubleFieldWidth_Dept = 0, doubleFieldWidth_Sum = 8;  //比例
        int intHierarchy;
        string[] strOVC_CLASS = { "C", "A", "N", "F", "J", "G", "P", "X" }; //單位類別代號
        string[] strOVC_CLASS_NAME = { "中央", "陸軍", "海軍", "空軍", "聯勤", "後備", "憲兵", "其他" }; //單位類別名稱

        //2階層專用
        string strMethodValue, strOvcClass; //方式之值，軍種類別

        #region 副程式
        private int getHierarchy(string strDEPT_SN) //取得目前為幾階層；1:0A100-1，2:0A100-2，9:其他
        {
            int intH = 0;
            string strHierarchy;
            //取得是否有階層，此變數為從源於自己呼叫才會有
            if (FCommon.getQueryString(this, "hierarchy", out strHierarchy, true))
                intH = int.Parse(strHierarchy);
            else
            {
                switch (strMethod)
                {
                    case "OVC_PUR_APPROVE_DEP": //核定權責
                        if (strDEPT_SN.Equals("0A100") || strDEPT_SN.Equals("00N00"))
                            intH = 1;
                        else
                            intH = 9;
                        break;
                    case "OVC_PUR_AGENCY": //採購方式
                    case "OVC_PUR_ASS_VEN_CODE": //招標方式
                    case "OVC_LAB": //採購屬性
                    case "OVC_PLAN_PURCH": //計畫性質
                        if (strDEPT_SN.Equals("0A100"))
                            intH = 1;
                        else
                            intH = 9;
                        break;
                }
            }
            return intH;
        }
        private void getField()
        {
            string[] strFieldName = new string[] { "無欄位" };
            string[] strFieldSql = new string[] { "" };

            string[] strName_OVC_PUR_APPROVE_DEP = { "A(部核)<br>國防部核定權責", "B(非部核)<br>國防部授權單位自行核定權責", "C(非部核)<br>國防部授權單位自行下授核定權責", "X(非部核)<br>其他核定權責" };
            string[] strSql_OVC_PUR_APPROVE_DEP = { "A", "B", "C", "X" };
            string[] strName_OVC_PUR_AGENCY = { "A<br>非軍事機關(外購)", "B<br>非軍事機關(內購)", "C<br>駐美採購組", "E<br>駐歐採購組", "F<br>單位自辦外購", "L<br>採購室(內購)", "M<br>軍售案(美)", "P<br>單位自辦內購", "S<br>軍售案(法)", "W<br>採購室(外購)" };
            string[] strSql_OVC_PUR_AGENCY = { "A", "B", "C", "E", "F", "L", "M", "P", "S", "W" };
            string[] strName_OVC_PUR_ASS_VEN_CODE = { "A<br>公開招標", "B<br>選擇性招標", "C<br>限制性招標", "D<br>公開取得企劃書或抱報價單" };
            string[] strSql_OVC_PUR_ASS_VEN_CODE = { "A", "B", "C", "D" };
            string[] strName_OVC_LAB = { "勞務採購", "財物採購<br>買受,訂製", "財物採購<br>租賃", "財物採購<br>租購" };
            string[] strSql_OVC_LAB = { "1", "2", "3", "4" };
            string[] strName_OVC_PLAN_PURCH = { "計畫性", "非計畫性" };
            string[] strSql_OVC_PLAN_PURCH = { "1", "2" };

            string[] strName_Temp = null, strSql_Temp = null;
            switch (strMethod)
            {
                case "OVC_PUR_APPROVE_DEP": //核定權責
                    doubleFieldWidth_Dept = 18;
                    strName_Temp = strName_OVC_PUR_APPROVE_DEP;
                    strSql_Temp = strSql_OVC_PUR_APPROVE_DEP;
                    break;
                case "OVC_PUR_AGENCY": //採購方式
                    doubleFieldWidth_Dept = 16;
                    strName_Temp = strName_OVC_PUR_AGENCY;
                    strSql_Temp = strSql_OVC_PUR_AGENCY;
                    break;
                case "OVC_PUR_ASS_VEN_CODE": //招標方式
                    doubleFieldWidth_Dept = 35;
                    strName_Temp = strName_OVC_PUR_ASS_VEN_CODE;
                    strSql_Temp = strSql_OVC_PUR_ASS_VEN_CODE;
                    break;
                case "OVC_LAB": //採購屬性
                    doubleFieldWidth_Dept = 35;
                    strName_Temp = strName_OVC_LAB;
                    strSql_Temp = strSql_OVC_LAB;
                    break;
                case "OVC_PLAN_PURCH": //計畫性質
                    doubleFieldWidth_Dept = 40;
                    strName_Temp = strName_OVC_PLAN_PURCH;
                    strSql_Temp = strSql_OVC_PLAN_PURCH;
                    break;
            }

            if (intHierarchy == 2)
            {
                doubleFieldWidth_Dept = 60;
                if (FCommon.getQueryString(this, "methodValue", out strMethodValue, true) && FCommon.getQueryString(this, "ovcClass", out strOvcClass, true))
                {
                    string strMethodText = "";
                    for(int f=0;f< strName_Temp.Length; f++)
                    {
                        if (strSql_Temp[f].Equals(strMethodValue))
                        {
                            strMethodText = strName_Temp[f];
                            break;
                        }
                    }
                    strFieldName = new string[] { strMethodText };
                    strFieldSql = new string[] { strMethodValue };
                }
                else
                    Response.Redirect("FUnitPurchaseST");
            }
            else if (intHierarchy == 1 || intHierarchy == 9)
            {
                if (strName_Temp != null) strFieldName = strName_Temp;
                if (strSql_Temp != null) strFieldSql = strSql_Temp;
            }

            aryFieldName = new ArrayList(strFieldName);
            aryFieldSql = new ArrayList(strFieldSql);
        }
        //動態欄位
        private void setField(GridView theGridView)
        {
            theGridView.Columns.Clear();
            TemplateField oTemplateField;
            DataControlRowType oHeader = DataControlRowType.Header;
            DataControlRowType oDataRow = DataControlRowType.DataRow;

            oTemplateField = new TemplateField();
            oTemplateField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, $"軍種＼{ strMethodName }");
            oTemplateField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, strFieldSql_Dept);
            oTemplateField.HeaderStyle.Width = Unit.Percentage(doubleFieldWidth_Dept);
            theGridView.Columns.Add(oTemplateField);

            int intCount = aryFieldName.Count;
            for (int i = 0; i < intCount; i++)
            {
                //string strFiledSql = aryFieldSql[i].ToString();
                //LinkButton theBtn = new LinkButton();
                //theBtn.ID = $"btn{ strFiledSql }";
                //theBtn.CommandName = strFiledSql;

                oTemplateField = new TemplateField();
                oTemplateField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, aryFieldName[i].ToString());
                oTemplateField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, aryFieldSql[i].ToString());
                //oTemplateField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, strFiledSql, theBtn, btnLinkButton_Click);
                theGridView.Columns.Add(oTemplateField);
            }
           
            if(intHierarchy ==1 || intHierarchy == 9) //僅 1 及 9 時，需要合計
            {
                oTemplateField = new TemplateField();
                oTemplateField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, "合計");
                oTemplateField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, strFieldSql_Sum);
                oTemplateField.HeaderStyle.Width = Unit.Percentage(doubleFieldWidth_Sum);
                theGridView.Columns.Add(oTemplateField);
            }

            //FCommon.GridView_setEmpty(theGridView);
        }
        private void dataImport(string strDEPT_SN)
        {
            #region SQL語法
            ArrayList artParameterName = new ArrayList();
            ArrayList aryData = new ArrayList();
            string strSQL = "";
            artParameterName.Add(":strYear"); aryData.Add(strYear);
            string strField, strTable, strJoin, strCondition, strGroup, strOrder;
            switch (intHierarchy)
            {
                #region 0A100-1
                case 1: //0A100-1
                    strField = ", "; strTable = ""; strJoin = $"where c.OVC_PUR_SECTION = b.OVC_DEPT_CDE and substr(c.OVC_PURCH, 3, 2) = { artParameterName[0] }"; strGroup = ", ";
                    switch (strMethod)
                    {
                        case "OVC_PUR_APPROVE_DEP": //核定權責
                            strField += "a.OVC_PUR_APPROVE_DEP";
                            strTable = " TBM1301_PLAN a,";
                            strJoin = $"where c.OVC_PURCH = a.OVC_PURCH and a.OVC_PUR_SECTION = b.OVC_DEPT_CDE and substr(a.OVC_PURCH, 3, 2) = { artParameterName[0] }";
                            strGroup += "a.OVC_PUR_APPROVE_DEP";
                            break;
                        case "OVC_PUR_AGENCY": //採購方式核定權責
                            strField += "c.OVC_PUR_AGENCY";
                            strGroup += "c.OVC_PUR_AGENCY";
                            break;
                        case "OVC_PUR_ASS_VEN_CODE": //招標方式
                            strField += "NVL(c.OVC_PUR_ASS_VEN_CODE,' ') OVC_PUR_ASS_VEN_CODE";
                            strGroup += "NVL(c.OVC_PUR_ASS_VEN_CODE,' ')";
                            break;
                        case "OVC_LAB": //採購屬性
                            strField += "NVL(c.OVC_LAB,' ') OVC_LAB";
                            strGroup += "NVL(c.OVC_LAB,' ')";
                            break;
                        case "OVC_PLAN_PURCH": //計畫性質
                            strField += "NVL(c.OVC_PLAN_PURCH,' ') OVC_PLAN_PURCH";
                            strGroup += "NVL(c.OVC_PLAN_PURCH,' ')";
                            break;
                    }
                    strSQL += $@"
                        select SUBSTR(NVL(B.OVC_CLASS,'X'),1,1) OVC_CLASS{ strField }, COUNT(1) CNT 
                        from{ strTable } TBMDEPT B, TBM1301 C 
                        { strJoin }
                        group by SUBSTR(NVL(B.OVC_CLASS,'X'),1,1){ strGroup }
                        order by OVC_CLASS
                    ";
                    break;
                #endregion
                #region 0A100-2
                case 2: //0A100-2
                    artParameterName.Add(":Value"); aryData.Add(strMethodValue);
                    artParameterName.Add(":Kind"); aryData.Add(strOvcClass);
                    strField = " c.OVC_PUR_SECTION, b.OVC_ONNAME,"; strTable = "";
                    strJoin = $"where c.OVC_PUR_SECTION = b.OVC_DEPT_CDE and substr(c.OVC_PURCH,3,2) = { artParameterName[0] }";
                    strCondition = "";
                    strGroup = "c.OVC_PUR_SECTION, b.OVC_ONNAME";
                    strOrder = "c.OVC_PUR_SECTION";
                    switch (strMethod)
                    {
                        case "OVC_PUR_APPROVE_DEP": //核定權責
                            strField = " a.OVC_PUR_SECTION, a.OVC_PUR_NSECTION OVC_ONNAME,";
                            strTable = " TBM1301_PLAN a,";
                            strJoin = $"where c.OVC_PURCH = a.OVC_PURCH and a.OVC_PUR_SECTION = b.OVC_DEPT_CDE and substr(a.OVC_PURCH,3,2) = { artParameterName[0] }";
                            strCondition = "a.OVC_PUR_APPROVE_DEP";
                            strGroup = "a.OVC_PUR_SECTION, a.OVC_PUR_NSECTION";
                            strOrder = "a.OVC_PUR_SECTION";
                            break;
                        case "OVC_PUR_AGENCY": //採購方式核定權責 //無第2層?
                            strField = " c.OVC_PUR_SECTION, c.OVC_PUR_NSECTION OVC_ONNAME,";
                            strCondition = "c.OVC_PUR_AGENCY";
                            strGroup = "c.OVC_PUR_SECTION, c.OVC_PUR_NSECTION";
                            break;
                        case "OVC_PUR_ASS_VEN_CODE": //招標方式
                            strCondition = "NVL(c.OVC_PUR_ASS_VEN_CODE,' ')";
                            break;
                        case "OVC_LAB": //採購屬性
                            strCondition = "NVL(c.OVC_LAB,' ')";
                            break;
                        case "OVC_PLAN_PURCH": //計畫性質
                            strCondition = "NVL(c.OVC_PLAN_PURCH,' ')";
                            break;
                    }
                    strSQL += $@"
                        select{ strField } count(1) CNT
                        from{ strTable } TBMDEPT b, TBM1301 c 
                        { strJoin }
                        and { strCondition } = { artParameterName[1] } --//核定權責
                        and substr(nvl(b.OVC_CLASS,'X'), 1, 1) = { artParameterName[2] } --//單位類別(軍種)
                        group by { strGroup }
                        order by { strOrder }
                    ";
                    break;
                #endregion
                #region 其他
                case 9: //其他
                    artParameterName.Add(":USER_DEPT_SN"); aryData.Add(strDEPT_SN);
                    strField = ", "; strTable = "TBM1301 b,"; strJoin = ""; strGroup = ", ";
                    switch (strMethod)
                    {
                        case "OVC_PUR_APPROVE_DEP": //核定權責
                            strField += "a.OVC_PUR_APPROVE_DEP";
                            strTable = $"TBM1301_PLAN a, { strTable }";
                            strJoin = " a.OVC_PURCH = b.OVC_PURCH and";
                            strGroup += "a.OVC_PUR_APPROVE_DEP";
                            break;
                        case "OVC_PUR_AGENCY": //採購方式
                            strField += "b.OVC_PUR_AGENCY";
                            strGroup += "b.OVC_PUR_AGENCY";
                            break;
                        case "OVC_PUR_ASS_VEN_CODE": //招標方式
                            strField += "NVL(b.OVC_PUR_ASS_VEN_CODE,' ') OVC_PUR_ASS_VEN_CODE";
                            strGroup += "NVL(b.OVC_PUR_ASS_VEN_CODE,' ')";
                            break;
                        case "OVC_LAB": //採購屬性
                            strField += "NVL(b.OVC_LAB,' ') OVC_LAB";
                            strGroup += "NVL(b.OVC_LAB,' ')";
                            break;
                        case "OVC_PLAN_PURCH": //計畫性質
                            strField += "NVL(b.OVC_PLAN_PURCH,' ') OVC_PLAN_PURCH";
                            strGroup += "NVL(b.OVC_PLAN_PURCH,' ')";
                            break;
                    }
                    strSQL += $@"
                        select C.OVC_DEPT_CDE, C.{ strFieldSql_Dept }{ strField }, COUNT(1) CNT
                        from { strTable }
                          (SELECT OVC_DEPT_CDE, OVC_ONNAME 
                            from TBMDEPT  
                            start with OVC_DEPT_CDE = { artParameterName[1] }
                            connect by prior OVC_DEPT_CDE = OVC_TOP_DEPT
                            AND OVC_TOP_DEPT <> OVC_DEPT_CDE and OVC_ENABLE = '1' 
                            order by OVC_DEPT_CDE) c
                        where{ strJoin } b.OVC_PUR_SECTION = c.OVC_DEPT_CDE and substr(b.OVC_PURCH, 3, 2) = { artParameterName[0] }
                        group by c.OVC_DEPT_CDE, c.OVC_ONNAME{ strGroup }
                        order by c.OVC_DEPT_CDE
                    ";
                    break;
                    #endregion
            }
            #endregion

            DataTable dt_Source = FCommon.getDataTableFromSelect(strSQL, artParameterName, aryData);
            DataTable dt = getDataTable(dt_Source);

            //取代空值為0
            int intCount_Column = dt.Columns.Count;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                for (int j = 1; j < intCount_Column; j++) //不須第0列(軍種)
                {
                    string strValue = dr[j].ToString();
                    if (strValue.Equals(string.Empty))
                        dr[j] = "0"; //將無資料之值設為0
                }
            }
            #region 計算合計
            //int intCount_Column = dt.Columns.Count, intValue;
            //int[] intTotal = new int[intCount_Column]; //各分類合計
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    DataRow dr = dt.Rows[i];
            //    for (int j = 1; j < intCount_Column; j++) //不須第0列(軍種)
            //    {
            //        //string strFieldName = dt.Columns[j].ColumnName;
            //        string strValue = dr[j].ToString();
            //        if (i == 0) intTotal[j] = 0; //首筆資料歸零
            //        if (strValue.Equals(string.Empty))
            //            dr[j] = "0"; //將無資料之值設為0
            //        else if (int.TryParse(strValue, out intValue))
            //            intTotal[j] += intValue;

            //    }
            //}
            ////新增合計列
            //DataRow dr_Total = dt.NewRow();
            //dr_Total[strFieldSql_Dept] = "合計";
            //for (int i = 1; i < intCount_Column; i++) //不須第0列(軍種)
            //{
            //    //string strFieldName = dt.Columns[i].ColumnName;
            //    dr_Total[i] = intTotal[i];
            //}
            //dt.Rows.Add(dr_Total);
            #endregion

            ViewState["hasRows"] = FCommon.GridView_dataImport(GVListed, dt);
        }
        private DataTable getDataTable(DataTable dt_Source)
        {
            DataTable dt = new DataTable();
            //新增欄位
            dt.Columns.Add(strFieldSql_Dept);
            foreach (string strField in aryFieldSql)
            {
                if (!dt.Columns.Contains(strField))
                    dt.Columns.Add(strField);
            }
            if (intHierarchy == 1 || intHierarchy == 9)
                dt.Columns.Add(strFieldSql_Sum);
            string strDept_Prev = ""; //前部門代碼
            string strClass_Prev = ""; //前軍種代碼
            DataRow dr = null;
            int intSum = 0, intCnt; //單筆部門合計，各筆部門分類之數量
            int intCount_Column = dt.Columns.Count;
            int[] intTotal = new int[intCount_Column]; //各分類合計
            for (int f = 0; f < intCount_Column; f++) //加總設定基礎值
                intTotal[f] = 0;
            string strURL = Request.Path, strURL_Download = ResolveClientUrl("~/pages/MPMS/F/FListedDownload");
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "year", strYear, true);
            FCommon.setQueryString(ref strQueryString, "method", strMethod, true);
            FCommon.setQueryString(ref strQueryString, "methodName", strMethodName, true);
            switch (intHierarchy)
            {
                #region 0A100-1
                case 1: //0A100-1
                    FCommon.setQueryString(ref strQueryString, "hierarchy", "2", true);
                    
                    for (int i = 0; i < strOVC_CLASS.Length; i++)
                    {
                        dr = dt.NewRow(); //新增一筆新軍種資料
                        intSum = 0; //歸零軍種合計(新軍種)
                        string strClass = strOVC_CLASS[i];
                        dr[strFieldSql_Dept] = strOVC_CLASS_NAME[i]; //設定新資料之軍種名稱
                        for (int j = 0; j < dt_Source.Rows.Count; j++)
                        {
                            DataRow dr_Source = dt_Source.Rows[j];
                            string strClass_Curr = dr_Source["OVC_CLASS"].ToString(); //取得軍種代碼
                            if (strClass_Curr.Equals(strClass)) //判斷資料庫之資料為欲新增之軍種
                            { //確認該行為需要列出之軍種，取得該行分類值，並暫時刪除該行
                                string strField_Kind = dt_Source.Columns.Contains(strMethod) ? dr_Source[strMethod].ToString() : ""; //取得分類之值
                                string strCnt = dr_Source["CNT"].ToString(); //取得該分類數量
                                dr_Source.Delete();

                                if (dt.Columns.Contains(strField_Kind) && int.TryParse(strCnt, out intCnt))
                                {
                                    string theQueryString = strQueryString;
                                    FCommon.setQueryString(ref theQueryString, "methodValue", strField_Kind, true);
                                    FCommon.setQueryString(ref theQueryString, "ovcClass", strClass, true);
                                    string strText = $"<a href='{ strURL + theQueryString }'>{ strCnt }</a>";
                                    dr[strField_Kind] = strText; //設定分類欄位之數量
                                    intSum += intCnt; //單一軍種合計
                                    //計算各分類合計
                                    for (int f = 1; f < intCount_Column - 1; f++) //不須第0列(軍種) 及 合計
                                        if (dt.Columns[f].ColumnName.Equals(strField_Kind))
                                        {
                                            intTotal[f] += intCnt;
                                            break;
                                        }
                                }
                            }
                        }
                        dt_Source.AcceptChanges(); //儲存上方所有刪除。

                        dr[strFieldSql_Sum] = intSum;
                        intTotal[intCount_Column - 1] += intSum; //各部門合計之加總
                        dt.Rows.Add(dr); //儲存該列軍種資料
                    }
                    #region 舊查詢顯示方式，會依照資料庫之軍種排序
                    //for (int i = 0; i < dt_Source.Rows.Count; i++)
                    //{
                    //    DataRow dr_Source = dt_Source.Rows[i];
                    //    string strClass_Curr = dr_Source["OVC_CLASS"].ToString(); //取得軍種代碼
                    //    string strField_Kind = dt_Source.Columns.Contains(strMethod) ? dr_Source[strMethod].ToString() : ""; //取得分類之值
                    //    string strCnt = dr_Source["CNT"].ToString(); //取得該分類數量
                    //    if (!strClass_Curr.Equals(strClass_Prev)) //判斷不同部門代碼時
                    //    {
                    //        if (dr != null)
                    //        {
                    //            dr[strFieldSql_Sum] = intSum;
                    //            intTotal[intCount_Column - 1] += intSum; //各部門合計之加總
                    //            dt.Rows.Add(dr); //儲存上個部門資料
                    //        }
                    //        dr = dt.NewRow(); //並新增一筆新部門資料
                    //        intSum = 0; //歸零部門合計(新部門)
                    //        //取得軍種名稱
                    //        string strCalssName = strClass_Curr;
                    //        for (int c = 0; c < strOVC_CLASS.Length; c++)
                    //        {
                    //            if (strOVC_CLASS[c].Equals(strClass_Curr))
                    //            {
                    //                strCalssName = strOVC_CLASS_NAME[c];
                    //                break;
                    //            }
                    //        }
                    //        dr[strFieldSql_Dept] = strCalssName;// + dr_Source[strFieldSql_Dept].ToString(); //設定新資料之部門名稱
                    //    }
                    //    if (dt.Columns.Contains(strField_Kind) && int.TryParse(strCnt, out intCnt))
                    //    {
                    //        string theQueryString = strQueryString;
                    //        FCommon.setQueryString(ref theQueryString, "methodValue", strField_Kind, true);
                    //        FCommon.setQueryString(ref theQueryString, "ovcClass", strClass_Curr, true);
                    //        string strText = $"<a href='{ strURL + theQueryString }'>{ strCnt }</a>";
                    //        dr[strField_Kind] = strText; //設定分類欄位之數量
                    //        intSum += intCnt; //單一軍種合計
                    //        //計算合計
                    //        for (int f = 1; f < intCount_Column - 1; f++) //不須第0列(軍種) 及 合計
                    //            if (dt.Columns[f].ColumnName.Equals(strField_Kind))
                    //            {
                    //                intTotal[f] += intCnt;
                    //                break;
                    //            }
                    //    }
                    //    strClass_Prev = dr_Source["OVC_CLASS"].ToString();
                    //}
                    //if (dr != null)
                    //{
                    //    dr[strFieldSql_Sum] = intSum;
                    //    intTotal[intCount_Column - 1] += intSum; //各部門合計之加總
                    //    dt.Rows.Add(dr); //儲存最後一部門之資料
                    //}
                    #endregion
                    break;
                #endregion
                #region 0A100-2
                case 2: //0A100-2
                    for (int i = 0; i < dt_Source.Rows.Count; i++)
                    {
                        dr = dt.NewRow();
                        DataRow dr_Source = dt_Source.Rows[i];
                        string strDept = dr_Source["OVC_PUR_SECTION"].ToString(); //取得部門代碼
                        string strCnt = dr_Source["CNT"].ToString(); //取得該分類數量
                        if (int.TryParse(strCnt, out intCnt) && aryFieldSql.Count > 0)
                        {
                            string theQueryString = strQueryString;
                            FCommon.setQueryString(ref theQueryString, "methodValue", strMethodValue, true);
                            //FCommon.setQueryString(ref theQueryString, "ovcClass", strOvcClass, true);
                            FCommon.setQueryString(ref theQueryString, "dept", strDept, true);
                            string strText = $"<a href='{ strURL_Download + theQueryString }' target='_blank'>{ strCnt }</a>";

                            dr[strFieldSql_Dept] = dr_Source[strFieldSql_Dept].ToString(); //設定新資料之部門名稱
                            string strFieldName = aryFieldSql[0].ToString();
                            dr[strFieldName] = strText; //設定分類欄位之數量連結
                            if (intCount_Column > 0)
                                intTotal[intCount_Column - 1] += intCnt;
                        }
                        dt.Rows.Add(dr);
                    }
                    break;
                #endregion
                #region 其他
                case 9: //其他
                    for (int i = 0; i < dt_Source.Rows.Count; i++)
                    {
                        DataRow dr_Source = dt_Source.Rows[i];
                        string strDept_Curr = dr_Source["OVC_DEPT_CDE"].ToString(); //取得部門代碼
                        string strField_Kind = dt_Source.Columns.Contains(strMethod) ? dr_Source[strMethod].ToString() : ""; //取得分類之值
                        string strCnt = dr_Source["CNT"].ToString(); //取得該分類數量
                        if (!strDept_Curr.Equals(strDept_Prev)) //判斷不同部門代碼時
                        {
                            if (dr != null)
                            {
                                dr[strFieldSql_Sum] = intSum;
                                intTotal[intCount_Column - 1] += intSum; //各部門合計之加總
                                dt.Rows.Add(dr); //儲存上個部門資料
                            }
                            dr = dt.NewRow(); //並新增一筆新部門資料
                            intSum = 0; //歸零部門合計(新部門)
                            dr[strFieldSql_Dept] = dr_Source[strFieldSql_Dept].ToString(); //設定新資料之部門名稱
                        }
                        if (dt.Columns.Contains(strField_Kind) && int.TryParse(strCnt, out intCnt))
                        {
                            string theQueryString = strQueryString;
                            FCommon.setQueryString(ref theQueryString, "methodValue", strField_Kind, true);
                            FCommon.setQueryString(ref theQueryString, "dept", strDept_Curr, true);
                            string strText = $"<a href='{ strURL_Download + theQueryString }' target='_blank'>{ strCnt }</a>";

                            dr[strField_Kind] = strText; //設定分類欄位之數量
                            intSum += intCnt;
                            //計算合計
                            for (int f = 1; f < intCount_Column - 1; f++) //不須第0列(軍種) 及 合計
                                if (dt.Columns[f].ColumnName.Equals(strField_Kind))
                                {
                                    intTotal[f] += intCnt;
                                    break;
                                }
                        }
                        strDept_Prev = dr_Source["OVC_DEPT_CDE"].ToString();
                    }
                    if (dr != null)
                    {
                        dr[strFieldSql_Sum] = intSum;
                        intTotal[intCount_Column - 1] += intSum; //各部門合計之加總
                        dt.Rows.Add(dr); //儲存最後一部門之資料
                    }
                    break;
                    #endregion
            }
            //FCommon.AlertShow(PnMessage, "danger", "系統訊息", "欄位數量：" + intCount_Column.ToString());
            //新增合計列
            DataRow dr_Total = dt.NewRow();
            for (int f = 1; f < intCount_Column; f++) //不須第0列(軍種)
                dr_Total[f] = intTotal[f];
            dr_Total[strFieldSql_Dept] = "合計";
            dt.Rows.Add(dr_Total);
            return dt;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                string strDEPT_SN = FCommon.getAccountDEPT(this);
                if (FCommon.getQueryString(this, "year", out strYear, true) &&
                    FCommon.getQueryString(this, "method", out strMethod, true) &&
                    FCommon.getQueryString(this, "methodName", out strMethodName, true))
                {
                    //if (!IsPostBack)
                    {
                        lblYear.Text = strYear;
                        intHierarchy = getHierarchy(strDEPT_SN);
                        //設置readonly屬性
                        //FCommon.Controls_Attributes("readonly", "true", txtOVC_DPROPOSE1, txtOVC_DPROPOSE2, txtOVC_PUR_DAPPROVE1, txtOVC_PUR_DAPPROVE2);
                        //載入GridView 欄位
                        getField(); //取得定義之欄位
                        setField(GVListed); //設定動態欄位
                        dataImport(strDEPT_SN); //取得資料庫資料並匯入GridView
                    }
                }
                else
                    Response.Redirect("FUnitPurchaseST");
            }
        }

        //protected void btnLinkButton_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("FPlanAssessmentSA");
        //}

        protected void GVListed_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //string strC_SN_SUB = e.Row.Cells[7].Text;
                //string strIS_UPLOAD = e.Row.Cells[8].Text;
                //string strIS_ENABLE = e.Row.Cells[9].Text;
                //e.Row.Cells[8].Visible = false;
                //e.Row.Cells[9].Visible = false;

                //if (e.Row.RowType == DataControlRowType.DataRow)
                //{
                //    if (!strC_SN_SUB.Equals(string.Empty))
                //    {
                //        var query =
                //            from tableSub in GME.TBM1407
                //            where tableSub.OVC_PHR_CATE.Equals("K5")
                //            where tableSub.OVC_PHR_ID.Equals(strC_SN_SUB)
                //            select new { tableSub.OVC_PHR_DESC };
                //        var queryList = query.ToList();
                //        if (queryList.Count > 0) e.Row.Cells[7].Text = queryList.First().OVC_PHR_DESC.ToString();
                //    }

                //    RadioButtonList rdoIS_UPLOAD = (RadioButtonList)e.Row.Cells[10].FindControl("drpIS_UPLOAD");    //上傳
                //    RadioButtonList rdoIS_ENABLE = (RadioButtonList)e.Row.Cells[11].FindControl("drpIS_ENABLE");    //開放
                //    FCommon.list_dataImportYN(rdoIS_UPLOAD, true, false);
                //    FCommon.list_dataImportAudit(rdoIS_ENABLE, true, false);
                //    rdoIS_UPLOAD.SelectedValue = strIS_UPLOAD;
                //    rdoIS_ENABLE.SelectedValue = strIS_ENABLE;
                //}
            }
            catch (Exception ex) { }
        }
        protected void GVListed_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}