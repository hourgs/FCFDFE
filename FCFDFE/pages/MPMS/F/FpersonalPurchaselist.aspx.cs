using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Data;
using System.Web.UI;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;
using System.IO;
using System.Collections;

namespace FCFDFE.pages.MPMS.F
{
    public partial class FpersonalPurchaselist : Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strDateFormat = Variable.strDateFormat;
        public string strMenuName = "", strMenuNameItem = "";
        public string strDEPT_SN, strDEPT_Name;
        string strUserID, strOVC_PURCH, strYear, strOVC_NAME;
        DataTable dt_Source = null;

        #region 副程式
        private void dataImport()
        {
            ArrayList artParameterName, aryData;
            string strSQL = getSQL(strUserID, strOVC_PURCH, strYear, strOVC_NAME, out artParameterName, out aryData);
            #region MPMS_C6查詢
            //var query =
            //    from t1202_1 in mpms.TBM1202_1
            //    where t1202_1.OVC_PURCH.Equals(strOVC_PURCH)
            //    join t1301 in mpms.TBM1301 on t1202_1.OVC_PURCH equals t1301.OVC_PURCH
            //    join t1202 in mpms.TBM1202
            //    on new { t1202_1.OVC_PURCH, t1202_1.OVC_DRECEIVE, t1202_1.OVC_CHECK_UNIT, t1202_1.ONB_CHECK_TIMES }
            //    equals new { t1202.OVC_PURCH, t1202.OVC_DRECEIVE, t1202.OVC_CHECK_UNIT, t1202.ONB_CHECK_TIMES }
            //    //&& t1202_1.OVC_CHECK_UNIT.Equals(deptSN) && t1202_1.OVC_AUDIT_UNIT.Equals(strUnit)
            //    orderby t1202_1.OVC_PURCH, t1202_1.ONB_CHECK_TIMES
            //    select new
            //    {
            //        t1202_1.OVC_PURCH,
            //        t1202_1.ONB_CHECK_TIMES,
            //        t1202_1.OVC_DAUDIT,
            //        t1202_1.OVC_DAUDIT_ASSIGN,
            //        t1301.OVC_PUR_IPURCH,
            //        t1301.OVC_PUR_AGENCY,
            //        t1301.OVC_PUR_NSECTION,
            //        t1202.OVC_DRESULT,
            //        t1202_1.OVC_CHECK_UNIT
            //    };
            //if (strDeptList.Contains(strDEPT_SN))
            //    query = query.Where(t => strDeptList.Contains(t.OVC_CHECK_UNIT));
            //else
            //    query = query.Where(t => t.OVC_CHECK_UNIT.Equals(strDEPT_SN));
            //DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            #endregion
            //FCommon.AlertShow(PnMessage, "danger", "SQL", strSQL);
            dt_Source = FCommon.getDataTableFromSelect(strSQL, artParameterName, aryData);
            DataTable dt = dt_Source.Copy(); //將重複之購案編號先清掉
            //foreach(DataColumn column in dt_Source.Columns)
            //    dt.Columns.Add(column.ColumnName);
            string strPURCH_Prev = null;
            foreach(DataRow dr in dt.Rows)
            {
                string strPURCH = dr["PURCH"].ToString();
                if (strPURCH_Prev != null && strPURCH.Equals(strPURCH_Prev))
                    dr.Delete();
                strPURCH_Prev = strPURCH;
            }
            dt.AcceptChanges();
            ViewState["hasRows"] = FCommon.GridView_dataImport(GVpersonal, dt);
        }
        private string getSQL(string strUserID, string strOVC_PURCH, string strYear, string strOVC_NAME,out ArrayList artParameterName, out ArrayList aryData)
        {
            string strSQL = "";
            artParameterName = new ArrayList();
            aryData = new ArrayList();
            if (!string.IsNullOrEmpty(strYear))
                strYear = strYear.Substring((strYear.Length - 2), 2); //取得年度末兩碼
            string[] strRole, strAuth, strRoleOld, strAuthOld;
            FCommon.getRoleAuthList(this, "MPMS", out strRole, out strAuth); //取得採購系統下，所有角色、權限清單
            FCommon.getOldAuthList(this, out strRoleOld, out strAuthOld); //取得採購系統下，所有角色、權限清單
                                                                          //string[] strDeptList = { "00N00", "0A100" }; //此清單內之單位，可以互查資料
            if (strDEPT_SN != null && strRole != null && strAuth != null)
            {
                #region 判斷角色、權限
                bool isEvaluation = false, isChief = false; //是否計評角色；主官權限
                int intCountRole = strRole.Length;
                for (int i = 0; i < intCountRole; i++)
                {
                    string theRole = strRole[i];
                    //角色為 計評主辦單位
                    isEvaluation = strRole.Contains("32");

                    //判斷是否為 主官(管) 權限 or 承辦首席
                    string theAuth = strAuth[i].Substring(strAuth[i].Length - 2, 2); //取後2碼
                    if (!theRole.Equals("35") && (theAuth.Equals("01") || theAuth.Equals("04")))
                        isChief = true;
                }
                //舊系統權限
                int intCountRoleOld = strRoleOld.Length;
                for (int i = 0; i < intCountRoleOld; i++)
                {
                    string theRole = strRoleOld[i];
                    //角色為 計評主辦單位
                    if (theRole.Equals("31")) isEvaluation = true;

                    //判斷是否為 主官(管) 權限 or 承辦首席
                    string theAuth = strAuthOld[i];
                    if (theAuth.Equals("5") || theAuth.Equals("8"))
                        isChief = true;
                }
                #endregion
                #region SQL語法
                if (isEvaluation)
                {
                    if (!string.IsNullOrEmpty(strOVC_PURCH)) //有輸入購案編號
                    {
                        artParameterName.Add(":OvcPurch"); aryData.Add(strOVC_PURCH + "%");
                        bool isDept = strUserID.Equals("31ASSIGNER") || strDEPT_SN.Equals("00N00") || strDEPT_SN.Equals("0A100"); //取得是否特定部門
                        if (!isDept) { artParameterName.Add(":USER_DEPT_SN"); aryData.Add(strDEPT_SN); }
                        #region 欄位
                        strSQL += $@"
                        select
                        b.OVC_PURCH||a.OVC_PUR_AGENCY PURCH,
                        a.OVC_PUR_AGENCY,
                        a.OVC_PUR_IPURCH,
                        a.OVC_PUR_NSECTION,
                        a.OVC_PUR_USER,
                        a.OVC_PUR_IUSER_PHONE_EXT,
                        a.OVC_DPROPOSE,
                        a.OVC_PROPOSE,
                        a.OVC_PUR_APPROVE,
                        a.OVC_PUR_DAPPROVE,
                        a.ovc_Pur_Dcanpo,
                        b.OVC_PURCH,
                        b.OVC_DRECEIVE,
                        b.OVC_CHECK_UNIT,
                        b.ONB_CHECK_TIMES,
                        b.OVC_DAUDIT_ASSIGN,
                        b.OVC_AUDIT_UNIT,
                        b.OVC_AUDIT_ASSIGNER,
                        b.OVC_DAUDIT,
                        b.OVC_CHECKER OVC_AUDITOR,
                        b.OVC_DRESULT
                    ";
                        #endregion
                        #region 資料表
                        string strTable_Condition;
                        if (isDept)
                            strTable_Condition = " and (a.OVC_CHECK_UNIT='00N00' or a.OVC_CHECK_UNIT='0A100')"; //採購室也可查到採購中心案
                        else
                            strTable_Condition = $" and a.OVC_CHECK_UNIT = { artParameterName[1] }";

                        strSQL += $@"
                        from
                        TBM1301 a,
                        (select a.OVC_PURCH, a.OVC_CHECK_UNIT, a.ONB_CHECK_TIMES, b.OVC_DRECEIVE, b.OVC_DAUDIT_ASSIGN, b.OVC_AUDIT_UNIT, b.OVC_AUDIT_ASSIGNER, b.OVC_DAUDIT, a.OVC_CHECKER, a.OVC_DRESULT
                            from TBM1202 a ,tbm1202_1 b
                            where a.OVC_PURCH = b.OVC_PURCH(+)
                            and a.OVC_DRECEIVE = b.OVC_DRECEIVE(+)
                            and a.OVC_PURCH like { artParameterName[0] } --//限制案號
                            and a.OVC_CHECK_UNIT = b.OVC_CHECK_UNIT(+) 
                            and a.OVC_CHECKER = b.OVC_AUDITOR(+)
                            { strTable_Condition }
                            and a.ONB_CHECK_TIMES = b.ONB_CHECK_TIMES(+)) b,
                        TBM1301_PLAN d
                    ";
                        #endregion
                        #region join 條件
                        strSQL += $@"
                        where a.OVC_PURCH = b.OVC_PURCH
                        and a.OVC_PURCH = d.OVC_PURCH
                    ";
                        #endregion
                        #region 基本條件 & 排序
                        string strCondition;
                        if (isDept)
                            strCondition = @"
                            and (d.OVC_AUDIT_UNIT='0A100' or d.OVC_AUDIT_UNIT='00N00' --//計評單位=登錄者單位
                            or d.OVC_PUR_APPROVE_DEP = 'A') --//讀取採購中心權責購案(部核案)
                        "; //採購室也可查到採購中心案
                        else
                            strCondition = $" and d.OVC_AUDIT_UNIT = { artParameterName[1] }";

                        strSQL += $@"
                        { strCondition }
                        order by b.OVC_PURCH, b.ONB_CHECK_TIMES
                    ";
                        #endregion
                    }
                    else if (!string.IsNullOrEmpty(strYear))
                    {
                        string strParameterOvcPurch = ":OvcPurch", strParameterYear = ":strYear", strParameterDept = ":USER_DEPT_SN", strParameterOvcName = ":ovcName";
                        bool isOvcPurch = !string.IsNullOrEmpty(strOVC_PURCH); //是否篩選購案編號
                        if (isOvcPurch) { artParameterName.Add(strParameterOvcPurch); aryData.Add(strOVC_PURCH + "%"); }
                        bool isYear = !string.IsNullOrEmpty(strYear) && (!strYear.Equals("all")); //是否篩選年度
                        if (isYear) { artParameterName.Add(strParameterYear); aryData.Add(strYear); }
                        bool isDept = strUserID.Equals("31ASSIGNER") || strDEPT_SN.Equals("00N00"); //取得是否特定部門
                        if (!isDept) { artParameterName.Add(strParameterDept); aryData.Add(strDEPT_SN); }
                        bool isName = strOVC_NAME != null && !strOVC_NAME.Equals("none") && !strOVC_NAME.Equals("All");
                        if (isName) { artParameterName.Add(strParameterOvcName); aryData.Add(strOVC_NAME); }
                        #region 欄位
                        strSQL += $@"
                            select
                            b.OVC_PURCH||a.OVC_PUR_AGENCY PURCH,
                            a.OVC_PUR_AGENCY,
                            a.OVC_PUR_IPURCH,
                            a.OVC_PUR_NSECTION,
                            a.OVC_PUR_USER,
                            a.OVC_PUR_IUSER_PHONE_EXT,
                            a.OVC_DPROPOSE,
                            a.OVC_PROPOSE,
                            a.OVC_PUR_APPROVE,
                            a.OVC_PUR_DAPPROVE,
                            a.ovc_Pur_Dcanpo,
                            b.OVC_PURCH,
                            b.OVC_DRECEIVE,
                            b.OVC_CHECK_UNIT,
                            b.ONB_CHECK_TIMES,
                            b.OVC_DAUDIT_ASSIGN,
                            b.OVC_AUDIT_UNIT,
                            b.OVC_AUDIT_ASSIGNER,
                            b.OVC_DAUDIT,
                            b.OVC_CHECKER OVC_AUDITOR,
                            b.OVC_DRESULT
                        ";
                        #endregion
                        #region 資料表
                        string strTable_Condition; //主辦單位與登入者單位相同
                        if (isDept)
                            strTable_Condition = " and (a.OVC_CHECK_UNIT='00N00' or a.OVC_CHECK_UNIT='0A100')"; //採購室也可查到採購中心案
                        else
                            strTable_Condition = $" and a.OVC_CHECK_UNIT = { strParameterDept }";

                        if (isYear)
                            strTable_Condition = $@" and substr(a.OVC_PURCH,3,2)={ strParameterYear }
                            { strTable_Condition }"; //限制年度

                        strSQL += $@"
                            from
                            TBM1301 a,
                            (select a.OVC_PURCH, a.OVC_CHECK_UNIT, a.ONB_CHECK_TIMES, b.OVC_DRECEIVE, b.OVC_DAUDIT_ASSIGN, b.OVC_AUDIT_UNIT, b.OVC_AUDIT_ASSIGNER, b.OVC_DAUDIT, a.OVC_CHECKER, a.OVC_DRESULT
                                from TBM1202 a ,tbm1202_1 b
                                where a.OVC_PURCH = b.OVC_PURCH(+)
                                and a.OVC_DRECEIVE = b.OVC_DRECEIVE(+)
                                { ((strOVC_NAME != null) && (!strOVC_NAME.Equals("none")) ? (strOVC_NAME.Equals("All") ? "" : $"and a.OVC_CHECKER = { strParameterOvcName }") : " and(a.OVC_CHECKER is null or a.OVC_CHECKER = ' ') ") }--//限制某承辦人
                                and a.OVC_CHECK_UNIT = b.OVC_CHECK_UNIT(+) 
                                and a.OVC_CHECKER = b.OVC_AUDITOR(+)
                                { strTable_Condition }
                                and a.ONB_CHECK_TIMES = b.ONB_CHECK_TIMES(+)) b,
                            TBM1301_PLAN d
                        ";
                        #endregion
                        #region join 條件
                        strSQL += $@"
                            where a.OVC_PURCH = b.OVC_PURCH
                            and a.OVC_PURCH = d.OVC_PURCH
                        ";
                        #endregion
                        #region 基本條件 & 排序
                        //承辦人 權限
                        string strCondition;
                        if (isDept)
                            strCondition = @"
                            and (d.OVC_AUDIT_UNIT='00N00' or d.OVC_AUDIT_UNIT='0A100' --//計評單位=登錄者單位
                            or d.OVC_PUR_APPROVE_DEP = 'A') --//讀取採購中心權責購案(部核案)
                        "; //採購室也可查到採購中心案
                        else
                            strCondition = $" and d.OVC_AUDIT_UNIT = { strParameterDept }";
                        if (isOvcPurch)
                            strCondition += $" and a.OVC_PURCH like { strParameterOvcPurch }"; //指定某購案
                        else if (isYear)
                            strCondition += $" and substr(a.OVC_PURCH,3,2) = { strParameterYear }"; //限制年度


                        //if (vcase != null && vcase.equals("notApprove"))
                        //{
                        //    if (user.getOvc_Pur_Section().equals("0A100"))
                        //    {//採購中心核定文號為【備採綜 ...】  
                        //        sql = sql + " and (a.ovc_Pur_Dapprove is null " + //尚未核定
                        //                  " or (a.ovc_Pur_Dapprove is not null and substr(trim(nvl(a.ovc_pur_approve,' ')),1,2) not in ('備採','昇軸') )) " + //有核定日期但非採購中心核定
                        //                  " and ( d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' " + //計評單位=登錄者單位      
                        //            "         or d.OVC_PUR_APPROVE_DEP = 'A') "; //讀取採購中心權責購案(部核案)
                        //    }
                        //    else
                        //    {
                        //        sql = sql + " and a.ovc_Pur_Dapprove is null " + //尚未核定
                        //                  " and d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' ";  //計評單位=登錄者單位 
                        //    }
                        //    sql = sql + " and (a.OVC_PUR_DCANPO is null or a.OVC_PUR_DCANPO = '') " +  //未撤案
                        //              " and (d.OVC_DCANCEL is null or d.OVC_DCANCEL = '') ";
                        //}
                        //else if (vcase != null && vcase.equals("Approve"))
                        //{
                        //    if (user.getOvc_Pur_Section().equals("0A100"))
                        //    { //採購中心核定文號為【備採綜 ...】 
                        //        sql = sql + " and (a.ovc_Pur_Dapprove is not null and substr(trim(nvl(a.ovc_pur_approve,' ')),1,2) in ('備採','昇軸') ) " + //採購中心核定
                        //                  " and ( d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' " + //計評單位=登錄者單位      
                        //                  " or d.OVC_PUR_APPROVE_DEP = 'A') "; //讀取採購中心權責購案(部核案)
                        //    }
                        //    else
                        //    {
                        //        sql = sql + " and a.ovc_Pur_Dapprove is not null " + //已核定
                        //                  "    and d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' ";  //計評單位=登錄者單位 
                        //    }
                        //    sql = sql + " and (a.OVC_PUR_DCANPO is null or a.OVC_PUR_DCANPO = '') " +//未撤案
                        //              " and (d.OVC_DCANCEL is null or d.OVC_DCANCEL = '') ";
                        //}
                        //else if (vcase != null && vcase.equals("Cancel"))
                        //{
                        //    sql = sql + " and ((a.OVC_PUR_DCANPO is not null) " + //已撤案;
                        //              " or (d.OVC_DCANCEL is not null)) ";
                        //}


                        strSQL += $@"
                            { strCondition }
                            order by b.OVC_PURCH, b.ONB_CHECK_TIMES
                        ";


                        #endregion
                    }
                }
                else //角色為 非計評主辦單位
                {
                    string strParameterOvcPurch = ":OvcPurch", strParameterYear = ":strYear", strParameterDept = ":USER_DEPT_SN", strParameterOvcName = ":ovcName";
                    bool isOvcPurch = !string.IsNullOrEmpty(strOVC_PURCH); //是否篩選購案編號
                    if (isOvcPurch && !isChief) { artParameterName.Add(strParameterOvcPurch); aryData.Add(strOVC_PURCH + "%"); } //不是主官才會有可能要篩選購案編號
                    bool isYear = !string.IsNullOrEmpty(strYear) && !strYear.Equals("all"); //是否篩選年度
                    if (isYear && (isChief || !isOvcPurch)) { artParameterName.Add(strParameterYear); aryData.Add(strYear); }
                    bool isDept = strUserID.Equals("31ASSIGNER") || strDEPT_SN.Equals("00N00") || strDEPT_SN.Equals("0A100"); //取得是否特定部門
                    if (!isDept) { artParameterName.Add(strParameterDept); aryData.Add(strDEPT_SN); }
                    bool isName = !string.IsNullOrEmpty(strOVC_NAME); //是否篩選承辦人
                    if (isChief) isName = isName && !strOVC_NAME.Equals("none"); //如果是主官，則條件不一樣
                    if (isName && (isChief || !isOvcPurch)) { artParameterName.Add(strParameterOvcName); aryData.Add(strOVC_NAME); }

                    #region 欄位
                    strSQL += $@"
                        select
                        b.OVC_PURCH||a.OVC_PUR_AGENCY PURCH,
                        a.OVC_PUR_AGENCY,
                        a.OVC_PUR_IPURCH,
                        a.OVC_PUR_NSECTION,
                        a.OVC_PUR_USER,
                        a.OVC_PUR_IUSER_PHONE_EXT,
                        a.OVC_DPROPOSE,
                        a.OVC_PROPOSE,
                        a.OVC_PUR_APPROVE,
                        a.OVC_PUR_DAPPROVE,
                        a.ovc_Pur_Dcanpo,
                        b.OVC_PURCH,
                        b.OVC_DRECEIVE,
                        b.OVC_CHECK_UNIT,
                        b.ONB_CHECK_TIMES,
                        b.OVC_DAUDIT_ASSIGN,
                        b.OVC_AUDIT_UNIT,
                        b.OVC_AUDIT_ASSIGNER,
                        b.OVC_DAUDIT,
                        b.OVC_AUDITOR,
                        c.OVC_DRESULT
                    ";
                    #endregion
                    #region 資料表
                    string strTable_Condition;
                    if (isDept)
                        strTable_Condition = " and (a.OVC_CHECK_UNIT='00N00' or a.OVC_CHECK_UNIT='0A100')"; //採購室也可查到採購中心案
                    else strTable_Condition = $" and a.OVC_CHECK_UNIT = { strParameterDept }";

                    strSQL += $@"
                        from
                        TBM1301 a,
                        TBM1202_1 b,
                        TBM1202 c,
                        TBM1301_PLAN d
                    ";
                    #endregion
                    #region join 條件
                    strSQL += $@"
                        where a.OVC_PURCH = b.OVC_PURCH
                        and a.OVC_PURCH = d.OVC_PURCH
                        and b.OVC_PURCH = c.OVC_PURCH
                        and b.OVC_DRECEIVE = c.OVC_DRECEIVE
                        and b.OVC_CHECK_UNIT = c.OVC_CHECK_UNIT
                        and b.ONB_CHECK_TIMES = c.ONB_CHECK_TIMES
                    ";
                    #endregion
                    #region 基本條件 & 排序
                    string strCondition; //計評單位=登錄者單位
                    if (isDept)
                        strCondition = @"
                            and (d.OVC_AUDIT_UNIT='00N00' or d.OVC_AUDIT_UNIT='0A100' --//計評單位=登錄者單位
                            or d.OVC_PUR_APPROVE_DEP = 'A') --//讀取採購中心權責購案(部核案)
                        "; //採購室也可查到採購中心案
                    else strCondition = $" and d.OVC_AUDIT_UNIT = { strParameterDept }";
                           //主官(管) 權限 or 承辦首席
                    if (isChief)
                    {
                        if (isYear)
                            strCondition += $" and substr(a.OVC_PURCH,3,2) = { strParameterYear }"; //限制年度
                        if (isName)
                            strCondition += $" and b.OVC_AUDITOR = { strParameterOvcName }"; //限制某承辦人
                        else
                            strCondition += " and (b.OVC_AUDITOR is null or b.OVC_AUDITOR = ' ') ";
                    }
                    else
                    {
                        //承辦人 權限
                        if (isOvcPurch)
                            strCondition += $" and a.OVC_PURCH like { strParameterOvcPurch }"; //指定某購案
                        else
                        {
                            if (isYear)
                                strCondition += $" and substr(a.OVC_PURCH,3,2) = { strParameterYear }"; //限制年度
                            if (isName)
                                strCondition += $" and b.OVC_AUDITOR = { strParameterOvcName }"; //指定某承辦人
                        }
                    }

                    //if (vcase != null && vcase.equals("notApprove"))
                    //{
                    //    if (user.getOvc_Pur_Section().equals("0A100") || user.getOvc_Pur_Section().equals("00N00"))
                    //    {//採購中心核定文號為【備採綜 ...】  
                    //        sql = sql + " and (a.ovc_Pur_Dapprove is null " + //尚未核定
                    //                    " or (a.ovc_Pur_Dapprove is not null and substr(trim(nvl(a.ovc_pur_approve,' ')),1,2) not in ('備採','昇軸') )) " + //有核定日期但非採購中心核定
                    //                    " and ( d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' or d.OVC_AUDIT_UNIT = '00N00'   " + //計評單位=登錄者單位      
                    //              "         or d.OVC_PUR_APPROVE_DEP = 'A') "; //讀取採購中心權責購案(部核案)
                    //    }
                    //    else
                    //    {
                    //        sql = sql + " and a.ovc_Pur_Dapprove is null " + //尚未核定
                    //                    " and d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' ";  //計評單位=登錄者單位 
                    //    }
                    //    sql = sql + " and (a.OVC_PUR_DCANPO is null or a.OVC_PUR_DCANPO = '') " +  //未撤案
                    //                " and (d.OVC_DCANCEL is null or d.OVC_DCANCEL = '') ";
                    //}
                    //else if (vcase != null && vcase.equals("Approve"))
                    //{
                    //    if (user.getOvc_Pur_Section().equals("0A100") || user.getOvc_Pur_Section().equals("00N00"))
                    //    { //採購中心核定文號為【備採綜 ...】 
                    //        sql = sql + " and (a.ovc_Pur_Dapprove is not null and substr(trim(nvl(a.ovc_pur_approve,' ')),1,2) in ('備採','昇軸') ) " + //採購中心核定
                    //                    " and ( d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' or d.OVC_AUDIT_UNIT = '00N00'   " + //計評單位=登錄者單位      
                    //                    " or d.OVC_PUR_APPROVE_DEP = 'A') "; //讀取採購中心權責購案(部核案)
                    //    }
                    //    else
                    //    {
                    //        sql = sql + " and a.ovc_Pur_Dapprove is not null " + //已核定
                    //                    "    and d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' ";  //計評單位=登錄者單位 
                    //    }
                    //    sql = sql + " and (a.OVC_PUR_DCANPO is null or a.OVC_PUR_DCANPO = '') " +//未撤案
                    //                " and (d.OVC_DCANCEL is null or d.OVC_DCANCEL = '') ";
                    //}
                    //else if (vcase != null && vcase.equals("Cancel"))
                    //{
                    //    sql = sql + " and ((a.OVC_PUR_DCANPO is not null) " + //已撤案;
                    //                " or (d.OVC_DCANCEL is not null)) ";
                    //}
                    if (strDEPT_SN.Equals("0A100") || strDEPT_SN.Equals("00N00"))
                        strCondition += $" and (b.OVC_CHECK_UNIT='{ strDEPT_SN }' or b.OVC_CHECK_UNIT='00N00')"; //主辦單位與登入者單位相同
                    else
                        strCondition += $" and b.OVC_CHECK_UNIT='{ strDEPT_SN }'"; //主辦單位與登入者單位相同

                    strSQL += $@"
                        and b.OVC_AUDIT_UNIT = (select OVC_AUDIT_UNIT from TBM1206 where USER_ID='{ strUserID }') --//同聯審小組(代碼K5)
                        { strCondition }
                        order by b.OVC_PURCH, b.ONB_CHECK_TIMES
                    ";
                    #endregion
                }
                #endregion
            }
            return strSQL;
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getAccountDEPTName(this, out strDEPT_SN, out strDEPT_Name);
                strUserID = Session["userid"].ToString();
                FCommon.getQueryString(this, "PurNum", out strOVC_PURCH, true);
                FCommon.getQueryString(this, "year", out strYear, true);
                FCommon.getQueryString(this, "ovcName", out strOVC_NAME, false);
                if (!IsPostBack)
                {
                    dataImport();
                }
            }
        }

        #region OnClick
        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            string strQueryString = "", strURL = "FPlanAssessmentSA"; //預設回 統計分析報表
            if (!string.IsNullOrEmpty(strOVC_PURCH))
                FCommon.setQueryString(ref strQueryString, "PurNum", strOVC_PURCH, true); //若有購案編號，則傳回購案編號
            else if (!string.IsNullOrEmpty(strYear))
            {
                FCommon.setQueryString(ref strQueryString, "year", strYear, true); //若有年度，則傳回年度至聯審小組查詢作業
                strURL = "FpersonalPurchaseST";
            }
            Response.Redirect(strURL + strQueryString);
        }
        #endregion

        protected void GVpersonal_Command(object sender, CommandEventArgs e)
        {
            string strPurchNum = e.CommandArgument.ToString();
            FPersonalPurchasePrint pagePrint = new FPersonalPurchasePrint();
            pagePrint.strPurchNum = strPurchNum;
            switch (e.CommandName)
            {
                case "PrintSupPDF":
                    var query = mpms.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.OVC_PURCH_KIND).FirstOrDefault();
                    if (query.Equals("1"))
                    {
                        pagePrint.PrintPDF(this);
                    }
                    else
                    {
                        pagePrint.MaterialApplication_ExportToWord(this);
                    }
                    break;
                case "btnOVCLIST_PRINT":
                    string filePathWORD = pagePrint.printListDoc(this);
                    FileInfo fileWORD = new FileInfo(filePathWORD);
                    string pdfPath = fileWORD.DirectoryName + "\\" + strPurchNum + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                    FPersonalPurchasePrint.WordcvDdf(filePathWORD, pdfPath);
                    File.Delete(filePathWORD);
                    //匯出PDF檔提供下載
                    Response.Clear();
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + strPurchNum + "計畫清單.pdf");
                    Response.ContentType = "application/octet-stream";
                    //Response.BinaryWrite(buffer);
                    Response.WriteFile(pdfPath);
                    Response.OutputStream.Flush();
                    Response.OutputStream.Close();
                    Response.Flush();
                    File.Delete(filePathWORD);
                    File.Delete(pdfPath);
                    Response.End();
                    break;
            }
        }
        protected void GVpersonal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvRow = e.Row;
            if (gvRow.RowType == DataControlRowType.DataRow)
            {
                //重複之購案顯示不同 審查次數、分派日、回覆日、作業天數
                Label lblPURCH = (Label)gvRow.FindControl("lblPURCH"); //購案編號
                Label lblONB_CHECK_TIMES = (Label)gvRow.FindControl("lblONB_CHECK_TIMES"); //購案編號
                Label lblOVC_DRECEIVE = (Label)gvRow.FindControl("lblOVC_DRECEIVE"); //分派日
                Label lblOVC_DAUDIT = (Label)gvRow.FindControl("lblOVC_DAUDIT"); //回覆日
                Label lblWorkDay = (Label)gvRow.FindControl("lblWorkDay"); //作業天數
                if (dt_Source != null && FCommon.Controls_isExist(lblPURCH, lblONB_CHECK_TIMES, lblOVC_DRECEIVE, lblOVC_DAUDIT, lblWorkDay)) //審查次數
                {
                    string strPURCH = lblPURCH.Text;
                    string strONB_CHECK_TIMES = "", strOVC_DRECEIVE = "", strOVC_DAUDIT = "", strWorkDay = "", strEmpty = "&nbsp;";
                    //int index = 0;
                    foreach (DataRow dr in dt_Source.Rows)
                    {
                        string thePURCH = dr["PURCH"].ToString();
                        string theOVC_PURCH = dr["OVC_PURCH"].ToString();
                        if (thePURCH.Equals(strPURCH)) //相同之購案編號
                        {
                            string theONB_CHECK_TIMES = dr["ONB_CHECK_TIMES"].ToString();
                            string theOVC_DRECEIVE = dr["OVC_DRECEIVE"].ToString();
                            string theOVC_DAUDIT = dr["OVC_DAUDIT"].ToString();
                            string theWorkDay;
                            if (theONB_CHECK_TIMES.Equals(string.Empty)) theONB_CHECK_TIMES = strEmpty;
                            else
                            { //加上超連結
                                string strQueryString = "";
                                FCommon.setQueryString(ref strQueryString, "PurNum", theOVC_PURCH, true);
                                FCommon.setQueryString(ref strQueryString, "CheckTimes", theONB_CHECK_TIMES,true);
                                //FCommon.setQueryString(ref strQueryString, "DRecive", theOVC_DRECEIVE, true);
                                theONB_CHECK_TIMES = $"<a href='FReviewDataSheet{ strQueryString }'>{ theONB_CHECK_TIMES }</a>";
                            }
                            strONB_CHECK_TIMES += $"<p>{ theONB_CHECK_TIMES }</p>";
                            //計算作業天數
                            DateTime dateOVC_DRECEIVE, dateOVC_DAUDIT;
                            bool isDate1 = DateTime.TryParse(theOVC_DRECEIVE, out dateOVC_DRECEIVE);
                            bool isDate2 = DateTime.TryParse(theOVC_DAUDIT, out dateOVC_DAUDIT);
                            if (isDate1) theOVC_DRECEIVE = dateOVC_DRECEIVE.ToString(strDateFormat);
                            else theOVC_DRECEIVE = strEmpty;
                            if (isDate2) theOVC_DAUDIT = dateOVC_DAUDIT.ToString(strDateFormat);
                            else theOVC_DAUDIT = strEmpty;
                            if (isDate1 && isDate2)
                            {
                                TimeSpan ts = dateOVC_DAUDIT - dateOVC_DRECEIVE;
                                theWorkDay = ts.Days.ToString();
                            }
                            else theWorkDay = "0";
                            strOVC_DRECEIVE += $"<p>{ theOVC_DRECEIVE }</p>";
                            strOVC_DAUDIT += $"<p>{ theOVC_DAUDIT }</p>";
                            strWorkDay += $"<p>{ theWorkDay }</p>";
                        }
                    }
                    lblONB_CHECK_TIMES.Text = strONB_CHECK_TIMES;
                    lblOVC_DRECEIVE.Text = strOVC_DRECEIVE;
                    lblOVC_DAUDIT.Text = strOVC_DAUDIT;
                    lblWorkDay.Text = strWorkDay;
                }
            }
        }
        protected void GVpersonal_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
            {
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            }
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}