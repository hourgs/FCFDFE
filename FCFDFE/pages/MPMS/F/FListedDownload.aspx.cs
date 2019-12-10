using FCFDFE.Content;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;

namespace FCFDFE.pages.MPMS.F
{
    public partial class FListedDownload : Page
    {
        Common FCommon = new Common();
        string strYear, strMethod, strMethodName, strMethodValue, strHierarchy;
        //int intHierarchy;

        #region 副程式
        private void dataImport(string strDEPT_SN)
        {
            ArrayList artParameterName = new ArrayList();
            ArrayList aryData = new ArrayList();
            artParameterName.Add(":strYear"); aryData.Add(strYear);
            #region SQL語法
            string strSQL = "", strOvcClass, strDept;
            bool isAllUnit = strMethod.Equals("AllUnit");
            bool isHasMethodValue = FCommon.getQueryString(this, "methodValue", out strMethodValue, true);
            bool isHasDept = FCommon.getQueryString(this, "dept", out strDept, true);
            if (FCommon.getQueryString(this, "ovcClass", out strOvcClass, true))
            {
                artParameterName.Add(":strOvcClass"); aryData.Add(strOvcClass);
                #region 欄位
                strSQL += $@"
                    select
                    b.OVC_PURCH||b.OVC_PUR_AGENCY||e.OVC_PURCH_5 PURCH, --//購案編號
                    NVL(f.ONB_GROUP,0) ONB_GROUP, --//分組
                    a.OVC_PUR_APPROVE_DEP, --//核定權責(代碼E8)tbm1301_plan
                    b.OVC_PUR_IPURCH, --//購案名稱(中文)tbm1301
                    t_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --b.OVC_PUR_SECTION, --//單位代碼tbm1301
                    b.OVC_AGNT_IN, --//採購單位(名稱，預設值　from 代碼 GK)tbm1301
                    t_LAB.OVC_PHR_DESC OVC_LAB, --NVL(b.OVC_LAB,' ') OVC_LAB, --//採購屬性 -->財物勞務(代碼GN)  tbm1301
                    replace(t_PLAN_PURCH.OVC_PHR_DESC, '性購案', '') OVC_PLAN_PURCH, --NVL(b.OVC_PLAN_PURCH,' ') OVC_PLAN_PURCH, --//計畫性質(1->計劃2->非計劃) 94.12.28 增加 tbm1301
                    b.OVC_PUR_USER, --//委方承辦人 tbm1301
                    NVL(d.OVC_CHECKER,' ') OVC_CHECKER, --//計評主辦單位承辦人(最後的評核單位) tbm1202
                    e.OVC_DO_NAME OVC_BID_DO_NAME, --//採購承辦人  (TBMRECEIVE_BID)
                    h.OVC_DO_NAME, --// 履驗承辦人(tbmreceive_contract)
                    --//額外查詢 預算科子目
                    --//額外查詢 預算年度
                    --//額外查詢 幣別 --t_PUR_CURRENT.OVC_PHR_DESC OVC_PUR_CURRENT, --//幣別
                    --//額外查詢 匯率 --//round(NVL(b.ONB_PUR_RATE, 1), 2) ONB_PUR_RATE, --//匯率
                    NVL(b.ONB_PUR_BUDGET_NT, 0) ONB_PUR_BUDGET_NT, --//預算總金額(新台幣) tbm1301
                    t_PUR_ASS_VEN_CODE.OVC_PHR_DESC OVC_PUR_ASS_VEN_CODE, --NVL(b.OVC_PUR_ASS_VEN_CODE,' ') OVC_PUR_ASS_VEN_CODE, --//招標方式(代碼C7) tbm1301
                    b.OVC_DPROPOSE, --//申購日期 tbm1301
                    --//申購文號
                    b.OVC_PUR_DAPPROVE, --//核定日期 tbm1301
                    --//核定文號
                    NVL(f.OVC_DOPEN,' ') OVC_DOPEN, --// 開標日期(YYYY-MM-DD) tbm1303 ()最後開標日)
                    t_RESULT.OVC_PHR_DESC OVC_RESULT, --NVL(f.OVC_RESULT,' ') OVC_RESULT, --//開標結果(代碼A8) tbm1303
                    NVL(g.OVC_DBID,' ') OVC_DBID, --// 決標日期(YYYY-MM-DD)  TBMBID_RESULT
                    NVL((g.ONB_RESULT_RATE * g.ONB_BID_RESULT),0.0)  ONB_MONEY_NT, --// 決標金額(台幣)
                    NVL((g.ONB_REMAIN_BUDGET * g.ONB_REMAIN_RATE),0.0) ONB_REMAIN_BUDGET_NT, --// 標餘款(台幣) TBMBID_RESULT
                    g.OVC_VENDORS_NAME, --// 得標商名稱 TBMBID_RESULT
                    b.OVC_PUR_DCANRE, --//撤案原因 tbm1301

                    b.OVC_PURCH
                    ----沒有用----
                    --b.OVC_PUR_AGENCY, --//採購單位地區方式代碼C2) tbm1301
                    --b.OVC_PUR_NSECTION, --+ --//單位全銜tbm1301
                    ";
                #endregion
                #region 資料表
                strSQL += $@"
                    from
                    TBM1301_PLAN a,
                    TBM1301 b,
                    TBMDEPT c,
                    TBM1202 d,
                    TBMRECEIVE_BID e,
                    (select K.OVC_PURCH,K.OVC_PURCH_5,NVL(K.ONB_GROUP,0) ONB_GROUP,
                        K.OVC_DOPEN,K.OVC_RESULT, NVL(K.ONB_BID_RESULT,0.0) ONB_BID_RESULT,
                        NVL(K.ONB_RESULT_RATE,0.0) ONB_RESULT_RATE,K.OVC_RESULT_REASON
                        from TBM1303 K,
                        (select OVC_PURCH,OVC_PURCH_5,NVL(ONB_GROUP,0) ONB_GROUP,max(OVC_DOPEN) OVC_DOPEN
                        from TBM1303 where SUBSTR(OVC_PURCH,3,2) = { artParameterName[0] }
                        group by OVC_PURCH,OVC_PURCH_5,NVL(ONB_GROUP,0) ) M,
                        (select OVC_PURCH,OVC_PURCH_5,COUNT(1) CNT from
                        (select OVC_PURCH,OVC_PURCH_5,ONB_GROUP,COUNT(1) CNT
                            from TBM1303 where SUBSTR(OVC_PURCH,3,2) = { artParameterName[0] }
                            group by OVC_PURCH,OVC_PURCH_5,ONB_GROUP)
                        group by OVC_PURCH,OVC_PURCH_5) N
                        where K.OVC_PURCH = M.OVC_PURCH and K.OVC_PURCH_5=M.OVC_PURCH_5
                        and K.ONB_GROUP = M.ONB_GROUP and K.OVC_DOPEN = M.OVC_DOPEN
                        and K.OVC_PURCH = N.OVC_PURCH and K.OVC_PURCH_5=N.OVC_PURCH_5
                        and ( (N.CNT > 1 and K.ONB_GROUP <> 0) or (N.CNT = 1 and K.ONB_GROUP = 0))
                        and SUBSTR(K.OVC_PURCH,3,2) = { artParameterName[0] }) f,
                    TBMBID_RESULT g,
                    (select distinct OVC_PURCH, OVC_PURCH_6, ONB_GROUP, OVC_DO_NAME from TBMRECEIVE_CONTRACT) h,

                    (select OVC_DEPT_CDE, OVC_ONNAME from TBMDEPT) t_PUR_SECTION, --//申購單位
                    (select OVC_PHR_ID, OVC_PHR_DESC from TBM1407 where OVC_PHR_CATE='E8') t_PUR_APPROVE_DEP, --//核定權責
                    (select OVC_PHR_ID, OVC_PHR_DESC from TBM1407 where OVC_PHR_CATE='TD') t_PLAN_PURCH, --//計畫性質
                    --沒用(select OVC_PHR_ID, OVC_PHR_DESC from TBM1407 where OVC_PHR_CATE='C2') t_PUR_AGENCY, --//採購途徑
                    (select OVC_PHR_ID, OVC_PHR_DESC from TBM1407 where OVC_PHR_CATE='GN') t_LAB, --//採購屬性
                    (select OVC_PHR_ID, OVC_PHR_DESC from TBM1407 where OVC_PHR_CATE='C7') t_PUR_ASS_VEN_CODE, --//招標方式
                    (select OVC_PHR_ID, OVC_PHR_DESC from TBM1407 where OVC_PHR_CATE='A8') t_RESULT --//開標結果
                    --沒用(SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='B0') t_PUR_CURRENT --//幣別
                    ";
                #endregion
                #region join 條件
                strSQL += $@"
                    where a.OVC_PURCH = B.OVC_PURCH --//案號
                    and b.OVC_PUR_SECTION = c.OVC_DEPT_CDE --//單位
                    and a.OVC_PURCH = d.OVC_PURCH(+)
                    and d.OVC_CHECK_UNIT(+) = a.OVC_AUDIT_UNIT --//計評主辦單位=預劃計評單位
                    --重複and a.OVC_PURCH = d.OVC_PURCH(+)
                    and e.OVC_PURCH = f.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = f.OVC_PURCH_5(+)
                    and f.OVC_PURCH = g.OVC_PURCH(+)
                    and f.OVC_PURCH_5 = g.OVC_PURCH_5(+)
                    and f.OVC_DOPEN = g.OVC_DOPEN(+)
                    and f.ONB_GROUP = g.ONB_GROUP(+)
                    and g.OVC_PURCH = h.OVC_PURCH(+)
                    and g.ONB_GROUP = h.ONB_GROUP(+)

                    and b.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    and a.OVC_PUR_APPROVE_DEP = t_PUR_APPROVE_DEP.OVC_PHR_ID(+)
                    and b.OVC_PLAN_PURCH = t_PLAN_PURCH.OVC_PHR_ID(+)
                    --沒用and b.OVC_PUR_AGENCY = t_PUR_AGENCY.OVC_PHR_ID(+)
                    and b.OVC_LAB = t_LAB.OVC_PHR_ID(+)
                    and b.OVC_PUR_ASS_VEN_CODE = t_PUR_ASS_VEN_CODE.OVC_PHR_ID(+)
                    and f.OVC_RESULT = t_RESULT.OVC_PHR_ID(+)
                    --沒用and NVL(b.OVC_PUR_CURRENT, 'N') = t_PUR_CURRENT.OVC_PHR_ID(+)
                    ";
                #endregion
                #region 篩選條件
                //+各統計方式的判斷條件 +
                switch (strMethod)
                {
                    case "OVC_PUR_APPROVE_DEP": //核定權責
                        strSQL += $@"
                            and a.OVC_PUR_APPROVE_DEP = { artParameterName[1] }
                        ";
                        break;
                    case "OVC_PUR_AGENCY": //採購方式 //無第2層?
                        strSQL += $@"
                            and b.OVC_PUR_AGENCY = { artParameterName[1] }
                        ";
                        break;
                    case "OVC_PUR_ASS_VEN_CODE": //招標方式
                        strSQL += $@"
                            and b.OVC_PUR_ASS_VEN_CODE = { artParameterName[1] }
                        ";
                        break;
                    case "OVC_LAB": //採購屬性
                        strSQL += $@"
                            and b.OVC_LAB = { artParameterName[1] }
                        ";
                        break;
                    case "OVC_PLAN_PURCH": //計畫性質
                        strSQL += $@"
                            and b.OVC_PLAN_PURCH = { artParameterName[1] }
                        ";
                        break;
                }
                #endregion
                #region 基本條件 & 排序
                strSQL += $@"
                    and SUBSTR(a.OVC_PURCH,3,2) = { artParameterName[0] } --//年度
                    and SUBSTR(NVL(c.OVC_CLASS,'X'),1,1) = { artParameterName[2] } --//單位類別
                    and d.ONB_CHECK_TIMES(+) = 1 --//只找第一次
                    order by a.OVC_PURCH, e.OVC_PURCH_5, f.ONB_GROUP
                    ";
                #endregion
            }
            else if(isAllUnit || (isHasMethodValue && isHasDept))
            {
                if (!isAllUnit)
                {
                    artParameterName.Add(":strMethodValue"); aryData.Add(strMethodValue);
                    artParameterName.Add(":dept"); aryData.Add(strDept);
                }
                #region 欄位
                strSQL += $@"
                    select
                    b.OVC_PURCH||b.OVC_PUR_AGENCY||FGETYEARSTATISTIC(a.OVC_PURCH,{ artParameterName[0] },'OVC_PURCH_5') PURCH, --//購案編號
                    NVL(FGETYEARSTATISTIC(a.OVC_PURCH,{ artParameterName[0] },'ONB_GROUP'), '0' ) ONB_GROUP, --//分組
                    a.OVC_PUR_APPROVE_DEP, --//核定權責(代碼E8)tbm1301_plan
                    b.OVC_PUR_IPURCH, --//購案名稱(中文)tbm1301
                    t_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --b.OVC_PUR_SECTION, --//單位代碼tbm1301
                    b.OVC_AGNT_IN, --//採購單位(名稱，預設值　from 代碼 GK)tbm1301
                    t_LAB.OVC_PHR_DESC OVC_LAB, --NVL(b.OVC_LAB,' ') OVC_LAB, --//採購屬性 -->財物勞務(代碼GN)  tbm1301
                    replace(t_PLAN_PURCH.OVC_PHR_DESC, '性購案', '') OVC_PLAN_PURCH, --NVL(b.OVC_PLAN_PURCH,' ') OVC_PLAN_PURCH, --//計畫性質(1->計劃2->非計劃) 94.12.28 增加 tbm1301
                    b.OVC_PUR_USER, --//委方承辦人
                    NVL(d.OVC_CHECKER, ' ') OVC_CHECKER, --//計評主辦單位承辦人(最後的評核單位) tbm1202
                    FGETYEARSTATISTIC(a.OVC_PURCH,{ artParameterName[0] },'OVC_BID_DO_NAME') OVC_BID_DO_NAME, --//採購承辦人  (TBMRECEIVE_BID)
                    FGETYEARSTATISTIC(a.OVC_PURCH,{ artParameterName[0] },'OVC_DO_NAME') OVC_DO_NAME, --// 履驗承辦人(tbmreceive_contract)
                    --//額外查詢 預算科子目
                    --//額外查詢 預算年度
                    --//額外查詢 幣別 --t_PUR_CURRENT.OVC_PHR_DESC OVC_PUR_CURRENT, --//幣別
                    --//額外查詢 匯率 --//round(NVL(b.ONB_PUR_RATE, 1), 2) ONB_PUR_RATE, --//匯率
                    NVL(b.ONB_PUR_BUDGET_NT, 0) ONB_PUR_BUDGET_NT, --//預算總金額(新台幣) tbm1301
                    t_PUR_ASS_VEN_CODE.OVC_PHR_DESC OVC_PUR_ASS_VEN_CODE, --NVL(b.OVC_PUR_ASS_VEN_CODE,' ') OVC_PUR_ASS_VEN_CODE, --//招標方式(代碼C7) tbm1301
                    b.OVC_DPROPOSE, --//申購日期 tbm1301
                    --//申購文號
                    b.OVC_PUR_DAPPROVE, --//核定日期 tbm1301
                    --//核定文號
                    NVL(FGETYEARSTATISTIC(a.OVC_PURCH,{ artParameterName[0] },'OVC_DOPEN'),' ' ) OVC_DOPEN, --// 開標日期(YYYY-MM-DD) tbm1303 ()最後開標日)
                    t_RESULT.OVC_PHR_DESC OVC_RESULT, --NVL(FGETYEARSTATISTIC(a.OVC_PURCH,{ artParameterName[0] },'OVC_RESULT'),' ' ) OVC_RESULT, --//開標結果(代碼A8) tbm1303
                    NVL(FGETYEARSTATISTIC(a.OVC_PURCH,{ artParameterName[0] },'OVC_DBID'),' ') OVC_DBID, --// 決標日期(YYYY-MM-DD)  TBMBID_RESULT
                    NVL((FGETYEARSTATISTIC(a.OVC_PURCH,{ artParameterName[0] },'ONB_RESULT_RATE') * FGETYEARSTATISTIC(a.OVC_PURCH,{ artParameterName[0] },'ONB_BID_RESULT')), 0.0) ONB_MONEY_NT, --// 決標金額(台幣)
                    NVL((FGETYEARSTATISTIC(a.OVC_PURCH,{ artParameterName[0] },'ONB_REMAIN_BUDGET') * FGETYEARSTATISTIC(a.OVC_PURCH,{ artParameterName[0] },'ONB_REMAIN_RATE')), 0.0) ONB_REMAIN_BUDGET_NT, --// 標餘款(台幣) TBMBID_RESULT
                    FGETYEARSTATISTIC(a.OVC_PURCH,{ artParameterName[0] },'OVC_VENDORS_NAME') OVC_VENDORS_NAME, --// 得標商名稱  TBMBID_RESULT
                    b.OVC_PUR_DCANRE, --//撤案原因 tbm1301

                    b.OVC_PURCH
                    ----沒有用----
                    --b.OVC_PUR_AGENCY, --//採購單位地區方式代碼C2) tbm1301
                    --b.OVC_PUR_NSECTION, --//單位全銜tbm1301
                    --NVL(b.OVC_LAB, ' ') OVC_LAB, --//計畫性質(1->計劃2->非計劃) 94.12.28 增加 tbm1301
                    ";
                #endregion
                #region 資料表
                strSQL += $@"
                    from
                    TBM1301_PLAN a,
                    TBM1301 b,
                    TBM1202 d,

                    (select OVC_DEPT_CDE, OVC_ONNAME from TBMDEPT) t_PUR_SECTION, --//申購單位
                    (select OVC_PHR_ID, OVC_PHR_DESC from TBM1407 where OVC_PHR_CATE='E8') t_PUR_APPROVE_DEP, --//核定權責
                    (select OVC_PHR_ID, OVC_PHR_DESC from TBM1407 where OVC_PHR_CATE='TD') t_PLAN_PURCH, --//計畫性質
                    --沒用(select OVC_PHR_ID, OVC_PHR_DESC from TBM1407 where OVC_PHR_CATE='C2') t_PUR_AGENCY, --//採購途徑
                    (select OVC_PHR_ID, OVC_PHR_DESC from TBM1407 where OVC_PHR_CATE='GN') t_LAB, --//採購屬性
                    (select OVC_PHR_ID, OVC_PHR_DESC from TBM1407 where OVC_PHR_CATE='C7') t_PUR_ASS_VEN_CODE, --//招標方式
                    (select OVC_PHR_ID, OVC_PHR_DESC from TBM1407 where OVC_PHR_CATE='A8') t_RESULT --//開標結果
                    --沒用(select OVC_PHR_ID, OVC_PHR_DESC from TBM1407 where OVC_PHR_CATE='B0') t_PUR_CURRENT --//幣別
                    ";
                #endregion
                #region join 條件
                strSQL += $@"
                    where a.OVC_PURCH = b.OVC_PURCH 
                    and a.OVC_PURCH = d.OVC_PURCH(+)
                    and d.OVC_CHECK_UNIT(+) = a.OVC_AUDIT_UNIT --//計評主辦單位=預劃計評單位

                    and b.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    and a.OVC_PUR_APPROVE_DEP = t_PUR_APPROVE_DEP.OVC_PHR_ID(+)
                    and b.OVC_PLAN_PURCH = t_PLAN_PURCH.OVC_PHR_ID(+)
                    --沒用and b.OVC_PUR_AGENCY = t_PUR_AGENCY.OVC_PHR_ID(+)
                    and b.OVC_LAB = t_LAB.OVC_PHR_ID(+)
                    and b.OVC_PUR_ASS_VEN_CODE = t_PUR_ASS_VEN_CODE.OVC_PHR_ID(+)
                    and FGETYEARSTATISTIC(a.OVC_PURCH,{ artParameterName[0] },'OVC_RESULT') = t_RESULT.OVC_PHR_ID(+)
                    --沒用and NVL(b.OVC_PUR_CURRENT, 'N') = t_PUR_CURRENT.OVC_PHR_ID(+)
                    ";
                #endregion
                #region 篩選條件
                //+各統計方式的判斷條件 +
                if (!isAllUnit)
                    switch (strMethod)
                    {
                        case "OVC_PUR_APPROVE_DEP": //核定權責
                            strSQL += $@"
                            and a.OVC_PUR_APPROVE_DEP = { artParameterName[1] }
                        ";
                            break;
                        case "OVC_PUR_AGENCY": //採購方式 //無第2層?
                            strSQL += $@"
                            and b.OVC_PUR_AGENCY = { artParameterName[1] }
                        ";
                            break;
                        case "OVC_PUR_ASS_VEN_CODE": //招標方式
                            strSQL += $@"
                            and b.OVC_PUR_ASS_VEN_CODE = { artParameterName[1] }
                        ";
                            break;
                        case "OVC_LAB": //採購屬性
                            strSQL += $@"
                            and b.OVC_LAB = { artParameterName[1] }
                        ";
                            break;
                        case "OVC_PLAN_PURCH": //計畫性質
                            strSQL += $@"
                            and b.OVC_PLAN_PURCH = { artParameterName[1] }
                        ";
                            break;
                    }
                #endregion
                #region 基本條件 & 排序
                if (!isAllUnit)
                    strSQL += $@"
                        and b.OVC_PUR_SECTION = { artParameterName[2] } --//單位代碼
                        ";
                strSQL += $@"
                    and SUBSTR(a.OVC_PURCH,3,2) = { artParameterName[0] }
                    and d.ONB_CHECK_TIMES(+) = 1
                    order by a.OVC_PURCH, FGETYEARSTATISTIC(a.OVC_PURCH,{ artParameterName[0] },'OVC_PURCH_5'), NVL(FGETYEARSTATISTIC(a.OVC_PURCH,{ artParameterName[0] },'ONB_GROUP'), '0' )
                    ";
                #endregion
            }
            else
                FCommon.MessageBoxShow_Close(this, "查詢錯誤！");

            #region 額外欄位查詢語法
            string[] strParameterName_Other = { ":vOVC_PURCH" };
            ArrayList aryData_Other = new ArrayList();
            aryData_Other.Add("");
            string strOther = $@"
                select
                a.OVC_PURCH,
                b.OVC_POI_IBDG,
                b.OVC_YY,
                a.OVC_ISOURCE,
                t_PUR_CURRENT.OVC_PHR_DESC OVC_CURRENT, --a.OVC_CURRENT,
                NVL(a.ONB_RATE, 1) ONB_RATE
                from TBM1231 a, TBM1118 b,
                (select OVC_PHR_ID, OVC_PHR_DESC from TBM1407 where OVC_PHR_CATE='B0') t_PUR_CURRENT --//幣別
                where a.OVC_PURCH = { strParameterName_Other[0] }
                and a.OVC_PURCH = b.OVC_PURCH(+)
                and a.OVC_ISOURCE = b.OVC_ISOURCE(+)
                and NVL(a.OVC_CURRENT, 'N') = t_PUR_CURRENT.OVC_PHR_ID(+)
                ";
            ////預算科子目 OVC_POI_IBDG 多筆
            //string strSQL_OVC_POI_IBDG = $@"select OVC_POI_IBDG from TBM1118  where OVC_PURCH = { strParameterName_Other[0] } order by OVC_POI_IBDG";
            ////預算年度 OVC_YY 多筆
            //string strSQL_OVC_YY = $@"select OVC_YY from TBM1118  where OVC_PURCH = { strParameterName_Other[0] } order by OVC_YY";
            #endregion

            string[] strFieldNames = { "購案編號", "分組", "核定權責", "購案名稱", "申購單位", "採購單位", "採購屬性", "採購型態",
                    "委方承辦人", "計評承辦人", "採購承辦人", "履驗承辦人", "預算科子目", "預算年度", "幣別", "匯率", "預算金額(新台幣)",
                    "招標方式", "申購日期", "申購文號", "核定日期", "核定文號", "開標日期", "開標結果", "決標日期",
                    "決標金額(新台幣)", "標餘款(新台幣)", "得標商", "撤案原因" };
            string[] strFieldSqls = { "PURCH", "ONB_GROUP", "OVC_PUR_APPROVE_DEP", "OVC_PUR_IPURCH", "OVC_PUR_SECTION", "OVC_AGNT_IN", "OVC_LAB", "OVC_PLAN_PURCH",
                    "OVC_PUR_USER", "OVC_CHECKER", "OVC_BID_DO_NAME", "OVC_DO_NAME", "OVC_POI_IBDG", "OVC_YY", "OVC_CURRENT", "ONB_RATE", "ONB_PUR_BUDGET_NT",
                    "OVC_PUR_ASS_VEN_CODE", "OVC_DPROPOSE", "", "OVC_PUR_DAPPROVE", "", "OVC_DOPEN", "OVC_RESULT", "OVC_DBID",
                    "ONB_MONEY_NT", "ONB_REMAIN_BUDGET_NT", "OVC_VENDORS_NAME", "OVC_PUR_DCANRE" };
        string strFormatInt = Variable.strExcleFormatInt;
            string strFormatMoney2 = Variable.strExcleFormatMoney2;
            string[] strFormat = { null, strFormatInt, null, null, null, null, null, null,
                    null, null, null, null, null, null, null, strFormatInt, strFormatMoney2,
                    null, null, null, null, null, null, null, null,
                    strFormatMoney2, strFormatMoney2, null, null };
            #endregion

            FCommon.AlertShow(PnMessage, "danger", "系統訊息", strSQL);
            //if (false)
            {
                DataTable dt_Source = FCommon.getDataTableFromSelect(strSQL, artParameterName, aryData);
                int intCount_Data;
                //將缺少SQL欄位補足
                foreach (string strFieldSql in strFieldSqls)
                {
                    if (!dt_Source.Columns.Contains(strFieldSql))
                        dt_Source.Columns.Add(strFieldSql);
                }
                foreach (DataRow dr in dt_Source.Rows)
                {
                    string strOVC_PURCH = dr["OVC_PURCH"].ToString();
                    aryData_Other[0] = strOVC_PURCH;

                    DataTable dt_Other = FCommon.getDataTableFromSelect(strOther, strParameterName_Other, aryData_Other);
                    if (dt_Other.Rows.Count > 0)
                    {
                        DataRow dr_Other = dt_Other.Rows[0];
                        dr["OVC_POI_IBDG"] = dr_Other["OVC_POI_IBDG"];
                        dr["OVC_YY"] = dr_Other["OVC_YY"];
                        dr["OVC_CURRENT"] = dr_Other["OVC_CURRENT"];
                        dr["ONB_RATE"] = dr_Other["ONB_RATE"];
                    }
                    ////預算科子目 OVC_POI_IBDG 多筆
                    //DataTable dt_OVC_POI_IBDG = FCommon.getDataTableFromSelect(strSQL_OVC_POI_IBDG, strParameterName_Other, aryData_Other);
                    //FPlanAssessmentSA.setFieldList(dr, dt_OVC_POI_IBDG, "OVC_POI_IBDG"); //多筆
                    ////年度 OVC_YY 多筆
                    //DataTable dt_OVC_YY = FCommon.getDataTableFromSelect(strSQL_OVC_YY, strParameterName_Other, aryData_Other);
                    //FPlanAssessmentSA.setFieldList(dr, dt_OVC_YY, "OVC_YY"); //多筆
                }

                DataTable dt = FPlanAssessmentSA.getDataTable_Export(dt_Source, strFieldNames, strFieldSqls, out intCount_Data);

                if (intCount_Data > 0)// && false)
                {
                    string strSheetText = $"計評3-{ strMethodName }";
                    string strTitleText = $"{ strYear }年度 各單位購案統計表";
                    string strFileName = $"{ strTitleText }.xlsx";

                    MemoryStream Memory = ExcelNPOI.RenderDataTableToStream_Chief(dt, strSheetText, strTitleText, 3, strFormat); //取得Excel資料流
                    FCommon.DownloadFile(this, strFileName, Memory); //直接下載檔案
                }
                else
                    FCommon.MessageBoxShow_Close(this, "查詢無結果，請重新下條件！");
            }
        }
        private void Close()
        {
            //關閉視窗
            string strScript =
                $@"<script language='javascript'>
                        window.close();
                    </script>";
            ClientScript.RegisterStartupScript(GetType(), "MessageBox", strScript);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this, true))
            {
                //FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                string strDEPT_SN = FCommon.getAccountDEPT(this);
                if (FCommon.getQueryString(this, "year", out strYear, true) &&
                    FCommon.getQueryString(this, "method", out strMethod, true) &&
                    FCommon.getQueryString(this, "methodName", out strMethodName, true))
                {
                    //if(int.TryParse(strHierarchy,out intHierarchy))
                    {
                        dataImport(strDEPT_SN); //取得資料庫資料並匯入GridView
                    }
                    //Close();
                }
                else
                    Response.Redirect("FUnitPurchaseST");
            }
        }
    }
}