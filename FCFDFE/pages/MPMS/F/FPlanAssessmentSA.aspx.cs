using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;

namespace FCFDFE.pages.MPMS.F
{
    public partial class FPlanAssessmentSA : Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string strFormatInt = Variable.strExcleFormatInt;
        string strFormatMoney2 = Variable.strExcleFormatMoney2;
        public string strDEPT_SN, strDEPT_Name;

        #region 副程式
        private void list_dataImport()
        {
            var query = gm.TBM1407;

            string strTestFirst = "請選擇－可空白";
            //單位屬性 自行資料庫搜尋
            DataTable dtUnit = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "S9").ToList());
            FCommon.list_dataImportV(drp_Unit, dtUnit, "OVC_PHR_DESC", "OVC_PHR_ID", ":", false);
            FCommon.list_setValue(drp_Unit, "C");

            //以下皆從MPMS_A12參考而來
            //採購權責
            DataTable dtOVC_PUR_APPROVE_DEP = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "E8").ToList());
            FCommon.list_dataImportV(drpOVC_PUR_APPROVE_DEP, dtOVC_PUR_APPROVE_DEP, "OVC_PHR_DESC", "OVC_PHR_ID", strTestFirst, "", ":", false);
            //採購屬性
            DataTable dtOVC_LAB = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "GN").ToList());
            FCommon.list_dataImportV(drpOVC_LAB, dtOVC_LAB, "OVC_PHR_DESC", "OVC_PHR_ID", strTestFirst, "", ":", false);
            //採購途徑
            DataTable dtOVC_PUR_AGENCY = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "C2").ToList());
            FCommon.list_dataImportV(drpOVC_PUR_AGENCY, dtOVC_PUR_AGENCY, "OVC_PHR_DESC", "OVC_PHR_ID", strTestFirst, "", ":", false);
            //招標方式
            DataTable dtOVC_PUR_ASS_VEN_CODE = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "C7").ToList());
            FCommon.list_dataImportV(drpOVC_PUR_ASS_VEN_CODE, dtOVC_PUR_ASS_VEN_CODE, "OVC_PHR_DESC", "OVC_PHR_ID", strTestFirst, "", ":", false);
            //計畫性質
            DataTable dtOVC_PLAN_PURCH = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "TD").ToList());
            FCommon.list_dataImportV(drpOVC_PLAN_PURCH, dtOVC_PLAN_PURCH, "OVC_PHR_DESC", "OVC_PHR_ID", strTestFirst, "", ":", false);
            //採購級距 drpOVC_PUR_LEVEL
            FCommon.list_dataImportV(drpOVC_PUR_LEVEL, Variable.listPurchLevelDistance_Text, Variable.listPurchLevelDistance_Value, strTestFirst, "", ":", false);

            //年度
            FCommon.list_dataImportYear(drpYear_PerPur);
            FCommon.list_dataImportYear(drpYear_PurSum);
            FCommon.list_dataImportYear(drpYear_OpenRead);
            FCommon.list_dataImportYear(drpYear_UndonePurSta);
            FCommon.list_dataImportYear(drpYear_MostFavS);
            FCommon.list_dataImportYear(drpYear_ApprovedPur);
            FCommon.list_dataImportYear(drpYear_CasePur);
            FCommon.list_dataImportYear(drpYear_UnitPur);
            FCommon.list_dataImportYear(drpYear_MNDPur);
            FCommon.list_dataImportYear(drpYear);
            FCommon.list_dataImportYear(drpYear1);
            FCommon.list_dataImportYear(drpYear2);
            string strYear_PerPur;
            if (FCommon.getQueryString(this, "yearPerPur", out strYear_PerPur, true))
                FCommon.list_setValue(drpYear_PerPur, strYear_PerPur);
        }
        public static DataTable getDataTable_Export(DataTable dt_Source, string[] strFieldNames, string[] strFieldSqls, out int intCount_Data)
        {
            DataTable dt = new DataTable();
            intCount_Data = dt_Source.Rows.Count;
            if (intCount_Data > 0)
            {
                string strFieldNo = "項次";
                dt.Columns.Add(strFieldNo);
                int intCount_Field = strFieldNames.Length;
                for (int i = 0; i < intCount_Data; i++)
                {
                    DataRow dr = dt.NewRow(), dr_Source = dt_Source.Rows[i];
                    dr[strFieldNo] = (i + 1).ToString();
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
        public static void setFieldList(DataRow dr_Source, DataTable dt_Data, string strFieleName)
        {
            if (dt_Data.Rows.Count > 0)
            {
                ArrayList aryData = new ArrayList();
                foreach (DataRow dr_Data in dt_Data.Rows)
                {
                    string strData = dr_Data[strFieleName].ToString();
                    bool isAdd = true;
                    foreach (string str in aryData)
                    {
                        if (strData.Equals(str))
                        {
                            isAdd = false;
                            break;
                        }
                    }
                    if (isAdd) aryData.Add(strData);
                }
                dr_Source[strFieleName] = string.Join(";", (string[])aryData.ToArray(typeof(string)));
            }
        }
        public static ArrayList getList(DataRow dr_Source, DataTable dt_Data, string strFieleName)
        {
            ArrayList aryData = new ArrayList();
            if (dt_Data.Rows.Count > 0)
            {
                foreach (DataRow dr_Data in dt_Data.Rows)
                {
                    string strData = dr_Data[strFieleName].ToString();
                    bool isAdd = true;
                    foreach (string str in aryData)
                    {
                        if (strData.Equals(str))
                        {
                            isAdd = false;
                            break;
                        }
                    }
                    if (isAdd) aryData.Add(strData);
                }
            }
            return aryData;
        }
        #endregion

        #region 查詢副程式
        //8.14欄位相同； 16.17欄位名稱相同
        //8.部核定購案統計總表
        private void Query_PurSum()
        {
            if (strDEPT_SN != null)
            {
                string[] strParameterName = { ":strYear", ":USER_DEPT_SN" };
                ArrayList aryData = new ArrayList();

                string strYear = drpYear_PurSum.SelectedValue;
                if (strYear.Length > 2)
                    strYear = strYear.Substring(strYear.Length - 2, 2);
                aryData.Add(strYear);
                aryData.Add(strDEPT_SN);

                #region SQL語法
                string strSQL = "";
                #region 欄位
                strSQL += $@"
                    select 
                    t_CLASS.OVC_PHR_DESC OVC_CLASS, --h.OVC_CLASS, --//單位屬性
                    a.OVC_PUR_SECTION OVC_PUR_SECTION_, 
                    T_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --a.OVC_PUR_NSECTION, --//申購單位
                    a.OVC_PUR_NSECTION, --//委購單位(-申購單位?)
                    a.OVC_PURCH||a.OVC_PUR_AGENCY||E.OVC_PURCH_5 PURCH, --a.OVC_PURCH, --//購案編號
                    a.OVC_PUR_AGENCY, --//採購方式代碼
                    a.OVC_PUR_IPURCH, --//購案名稱
                    t_PUR_ASS_VEN_CODE.OVC_PHR_DESC OVC_PUR_ASS_VEN_CODE, --NVL(a.OVC_PUR_ASS_VEN_CODE,' ') OVC_PUR_ASS_VEN_CODE, --//招標方式代碼
                    t_LAB.OVC_PHR_DESC OVC_LAB, --a.OVC_LAB, --//採購屬性代碼
                    t_PLAN_PURCH.OVC_PHR_DESC OVC_PLAN_PURCH, --a.OVC_PLAN_PURCH, --//計畫性質代碼
                    --//額外查詢 預算科子目
                    NVL((a.ONB_PUR_BUDGET-f.ONB_BID_RESULT), 0) ONB_PUR_BUDGET, --//預劃金額
                    a.ONB_PUR_BUDGET_NT, --//核定預算金額 預劃金額(台幣)
                    NVL((f.ONB_BID_RESULT * g.ONB_RATE), 0) ONB_BID_RESULT_NT, --//決標金額
                    --//額外查詢 公開閱覽日期
                    --//額外查詢 FAC
                    --//額外查詢 評核承辦人
                    --//採購承辦人
                    a.OVC_PUR_USER, --//委方承辦人
                    a.OVC_PUR_IUSER_PHONE_EXT, a.OVC_PUR_IUSER_PHONE_EXT1, --//委方聯絡電話
                    b.OVC_DAPPLY, --//預計呈報日期(=預計申購日期?)
                    a.OVC_DPROPOSE, --//呈報日期(=申購日期?)
                    --//額外查詢 紙本送達日期
                    --a.OVC_PUR_DAPPROVE, --//核定日期
                    --a.OVC_PUR_ALLOW, --//主官核批日
                    (a.OVC_PUR_DAPPROVE|| case when a.OVC_PUR_ALLOW is null then '' else '('||a.OVC_PUR_ALLOW||')' end) OVC_PUR_DAPPROVE_ALLOW, --//核定日期(主官核批日)
                    --//額外查詢 審查逾60天原因

                    a.OVC_PURCH
                    ----以下沒用到----
                    --B.OVC_DCANCEL, --//預劃撤案
                    --b.OVC_CANCEL_REASON, 
                    --a.OVC_AGNT_IN,
                    --a.OVC_PUR_DCANPO, --//撤案日期
                    --a.OVC_PUR_CURRENT, 
                    --a.ONB_PUR_RATE,
                    --a.ONB_PUR_BUDGET, --//預劃金額
                    --F.ONB_BID_RESULT, 
                    --G.ONB_RATE,
                    --decode(g.OVC_VEN_TITLE,null,f.OVC_VENDORS_NAME,g.OVC_VEN_TITLE) OVC_VENDORS_NAME, 
                    --a.OVC_PUR_DCANRE,
                    --B.OVC_PUR_APPROVE_DEP, --//核定權責
                    --B.OVC_AUDIT_UNIT
                    --E.OVC_PURCH_5

                    --case when a.OVC_PUR_DCANPO is null then 'N' else 'Y' end OVC_PUR_DCANPO_YN, --//是否撤案
                    --case when L.vCase>0 then 'Y' else 'N' end OVC_ADVENTAGED_CHECK --//準用最有利標
                    ";
                #endregion
                #region 資料表
                strSQL += $@"
                    from 
                    TBM1301 a,
                    TBM1301_PLAN b,
                    VIMAXSTATUS e,
                    VIBID_RESULT f,
                    VITBM1302 g,
                    TBMDEPT h,

                    (SELECT OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PUR_SECTION, --//申購單位
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='S9') t_CLASS, --//單位屬性
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='TD') t_PLAN_PURCH, --//計畫性質
                    --(SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='C2') t_PUR_AGENCY, --//採購途徑 
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='GN') t_LAB, --//採購屬性
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='C7') t_PUR_ASS_VEN_CODE --//招標方式
                    ";
                #endregion
                #region join 條件
                strSQL += $@"
                    where a.OVC_PURCH = b.OVC_PURCH
                    and a.OVC_PURCH = e.OVC_PURCH(+)
                    and e.OVC_PURCH = f.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = f.OVC_PURCH_5(+)
                    and e.OVC_PURCH = g.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = g.OVC_PURCH_5(+)
                    and a.OVC_PUR_SECTION = h.OVC_DEPT_CDE

                    and a.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    and h.OVC_CLASS = t_CLASS.OVC_PHR_ID(+)
                    and a.OVC_PLAN_PURCH = t_PLAN_PURCH.OVC_PHR_ID(+)
                    --and a.OVC_PUR_AGENCY = t_PUR_AGENCY.OVC_PHR_ID(+)
                    and a.OVC_LAB = t_LAB.OVC_PHR_ID(+)
                    and a.OVC_PUR_ASS_VEN_CODE = t_PUR_ASS_VEN_CODE.OVC_PHR_ID(+)
                    ";
                #endregion
                #region 篩選條件
                //--+加欄位篩選(有選擇的條件(下方有說明)+
                string strOVC_DELIVERY1 = txt_PurSum1.Text;
                if (!strOVC_DELIVERY1.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_DPROPOSE >= '{ strOVC_DELIVERY1 }'
                    ";
                string strOVC_DELIVERY2 = txt_PurSum2.Text;
                if (!strOVC_DELIVERY2.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_DPROPOSE <= '{ strOVC_DELIVERY2 }'
                    ";
                #endregion
                #region 基本條件 & 排序
                if (strDEPT_SN.Equals("0A100"))
                {
                    strSQL += $@"
                        and(b.OVC_AUDIT_UNIT = { strParameterName[1] } --//計評單位=登錄者單位
                        or b.OVC_PUR_APPROVE_DEP = 'A') --//讀取採購中心權責購案(部核案)
                        ";
                }
                else
                    strSQL += $@"
                        and b.OVC_AUDIT_UNIT = { strParameterName[1] }
                        ";
                strSQL += $@"
                    and substr(a.ovc_purch,3,2) = { strParameterName[0] }
                    order by a.OVC_PURCH
                    ";
                #endregion
                #region 額外欄位查詢語法
                string[] strParameterName_Other = { ":vOVC_PURCH" };
                ArrayList aryData_Other = new ArrayList();
                aryData_Other.Add("");
                //預算科子目 OVC_POI_IBDG
                string strSQL_OVC_POI_IBDG = $@"select OVC_POI_IBDG from TBM1118  where OVC_PURCH = { strParameterName_Other[0] } order by OVC_POI_IBDG";
                //公開閱覽日期 OVC_OPEN_PERIOD
                string strSQL_OVC_OPEN_PERIOD = $@"select OVC_OPEN_PERIOD from TBMANNOUNCE_OPEN where OVC_PURCH={ strParameterName_Other[0] }";
                //FAC OVC_FAC_STATUS
                string[] strParameterName_OVC_FAC_STATUS = { ":vOVC_PURCH", ":vOVC_CHECK_UNIT" };
                ArrayList aryData_OVC_FAC_STATUS = new ArrayList();
                aryData_OVC_FAC_STATUS.Add("");
                aryData_OVC_FAC_STATUS.Add(strDEPT_SN);
                string strSQL_OVC_FAC_STATUS = $@"select OVC_FAC_STATUS from TBMFAC_STATUS where OVC_PURCH = { strParameterName_OVC_FAC_STATUS[0] } and OVC_CHECK_UNIT = { strParameterName_OVC_FAC_STATUS[1] }";
                //評核承辦人 OVC_CHECKER
                string[] strParameterName_OVC_CHECKER = { ":vOVC_PURCH", ":USER_DEPT_SN" };
                ArrayList aryData_OVC_CHECKER = new ArrayList();
                aryData_OVC_CHECKER.Add("");
                aryData_OVC_CHECKER.Add(strDEPT_SN);
                string strSQL_OVC_CHECKER = $@"
                    select OVC_CHECKER
                    from TBM1202 a,
                    (select OVC_PURCH, max(ONB_CHECK_TIMES) TIMES
                        from TBM1202 where OVC_PURCH = { strParameterName_OVC_CHECKER[0] } and OVC_CHECK_UNIT = { strParameterName_OVC_CHECKER[1] }
                        group by OVC_PURCH) B
                    where a.OVC_PURCH = B.OVC_PURCH and a.ONB_CHECK_TIMES = B.TIMES and a.OVC_CHECK_UNIT = { strParameterName_OVC_CHECKER[1] }
                ";
                //紙本送達日期 OVC_DRECEIVE_PAPER
                string strSQL_OVC_DRECEIVE_PAPER = $@"select max(OVC_DRECEIVE_PAPER) OVC_DRECEIVE_PAPER from TBM1202 where OVC_PURCH = { strParameterName_Other[0] }";
                //審查逾60天原因 OVC_REASON
                string strSQL_OVC_REASON = $@"
                    select '第'||ONB_NO||'次('||OVC_REASON||')' OVC_REASON from TBMAUDIT_LOG where OVC_PURCH = { strParameterName_Other[0] }
                ";
                #endregion
                string[] strFieldNames = { "單位屬性", "委購單位", "購案編號", "採購方式", "購案名稱", "招標方式", "採購屬性", "計畫/非計畫",
                    "預算科子目", "預劃金額", "核定預算金額", "決標金額", "公開閱覽日期", "FAC", "評核承辦人", "採購承辦人",
                    "委方承辦人", "委方聯絡電話", "預計呈報日期", "呈報日期", "紙本送達日期", "核定日期(主官核批日)", "審查逾60天原因" };
                string[] strFieldSqls = { "OVC_CLASS", "OVC_PUR_SECTION", "PURCH", "OVC_PUR_AGENCY", "OVC_PUR_IPURCH", "OVC_PUR_ASS_VEN_CODE", "OVC_LAB", "OVC_PLAN_PURCH",
                    "OVC_POI_IBDG", "ONB_PUR_BUDGET", "ONB_PUR_BUDGET_NT", "ONB_BID_RESULT_NT", "OVC_OPEN_PERIOD", "OVC_FAC_STATUS", "OVC_CHECKER", "",
                    "OVC_PUR_USER", "OVC_PUR_IUSER_PHONE_EXT", "OVC_DAPPLY", "OVC_DPROPOSE", "OVC_DRECEIVE_PAPER", "OVC_PUR_DAPPROVE_ALLOW", "OVC_REASON" };
                string[] strFormat = { null, null, null, null, null, null, null, null,
                    null, strFormatMoney2, strFormatMoney2, strFormatMoney2, null, null, null, null,
                    null, null, null, null, null, null, null };
                #endregion

                //FCommon.AlertShow(PnMessage, "danger", "系統訊息", strSQL);
                DataTable dt_Source = FCommon.getDataTableFromSelect(strSQL, strParameterName, aryData);
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
                    aryData_OVC_FAC_STATUS[0] = strOVC_PURCH;
                    aryData_OVC_CHECKER[0] = strOVC_PURCH;

                    //委方聯絡電話
                    string strOVC_PUR_IUSER_PHONE_EXT = dr["OVC_PUR_IUSER_PHONE_EXT"].ToString();
                    string strOVC_PUR_IUSER_PHONE_EXT1 = dr["OVC_PUR_IUSER_PHONE_EXT1"].ToString();
                    if (!strOVC_PUR_IUSER_PHONE_EXT1.Equals(string.Empty))
                    {
                        if (!strOVC_PUR_IUSER_PHONE_EXT.Equals(string.Empty))
                            strOVC_PUR_IUSER_PHONE_EXT = $"{ strOVC_PUR_IUSER_PHONE_EXT };{ strOVC_PUR_IUSER_PHONE_EXT1 }";
                        else
                            strOVC_PUR_IUSER_PHONE_EXT = strOVC_PUR_IUSER_PHONE_EXT1;
                        if (strOVC_PUR_IUSER_PHONE_EXT.Length > dt_Source.Columns["OVC_PUR_IUSER_PHONE_EXT"].MaxLength)
                            dt_Source.Columns["OVC_PUR_IUSER_PHONE_EXT"].MaxLength = strOVC_PUR_IUSER_PHONE_EXT.Length;
                        dr["OVC_PUR_IUSER_PHONE_EXT"] = strOVC_PUR_IUSER_PHONE_EXT;
                    }

                    DataTable dt_OVC_POI_IBDG = FCommon.getDataTableFromSelect(strSQL_OVC_POI_IBDG, strParameterName_Other, aryData_Other);
                    if (dt_OVC_POI_IBDG.Rows.Count > 0)
                    {
                        ArrayList aryOVC_POI_IBDG = new ArrayList();
                        foreach (DataRow dr_OVC_POI_IBDG in dt_OVC_POI_IBDG.Rows)
                        {
                            string strOVC_POI_IBDG = dr_OVC_POI_IBDG["OVC_POI_IBDG"].ToString();
                            bool isAdd = true;
                            foreach (string str in aryOVC_POI_IBDG)
                            {
                                if (strOVC_POI_IBDG.Equals(str))
                                {
                                    isAdd = false;
                                    break;
                                }
                            }
                            if (isAdd) aryOVC_POI_IBDG.Add(strOVC_POI_IBDG);
                        }
                        dr["OVC_POI_IBDG"] = string.Join(";", (string[])aryOVC_POI_IBDG.ToArray(typeof(string)));
                    }
                    DataTable dt_OVC_OPEN_PERIOD = FCommon.getDataTableFromSelect(strSQL_OVC_OPEN_PERIOD, strParameterName_Other, aryData_Other);
                    if (dt_OVC_OPEN_PERIOD.Rows.Count > 0)
                        dr["OVC_OPEN_PERIOD"] = dt_OVC_OPEN_PERIOD.Rows[0]["OVC_OPEN_PERIOD"];
                    DataTable dt_OVC_FAC_STATUS = FCommon.getDataTableFromSelect(strSQL_OVC_FAC_STATUS, strParameterName_OVC_FAC_STATUS, aryData_OVC_FAC_STATUS);
                    if (dt_OVC_FAC_STATUS.Rows.Count > 0)
                        dr["OVC_FAC_STATUS"] = dt_OVC_FAC_STATUS.Rows[0]["OVC_FAC_STATUS"];
                    DataTable dt_OVC_CHECKER = FCommon.getDataTableFromSelect(strSQL_OVC_CHECKER, strParameterName_OVC_CHECKER, aryData_OVC_CHECKER);
                    if (dt_OVC_CHECKER.Rows.Count > 0)
                        dr["OVC_CHECKER"] = dt_OVC_CHECKER.Rows[0]["OVC_CHECKER"];
                    DataTable dt_OVC_DRECEIVE_PAPER = FCommon.getDataTableFromSelect(strSQL_OVC_DRECEIVE_PAPER, strParameterName_Other, aryData_Other);
                    if (dt_OVC_DRECEIVE_PAPER.Rows.Count > 0)
                        dr["OVC_DRECEIVE_PAPER"] = dt_OVC_DRECEIVE_PAPER.Rows[0]["OVC_DRECEIVE_PAPER"];
                    DataTable dt_OVC_REASON = FCommon.getDataTableFromSelect(strSQL_OVC_REASON, strParameterName_Other, aryData_Other);
                    if (dt_OVC_REASON.Rows.Count > 0)
                        dr["OVC_REASON"] = dt_OVC_REASON.Rows[0]["OVC_REASON"];
                }

                DataTable dt = getDataTable_Export(dt_Source, strFieldNames, strFieldSqls, out intCount_Data);

                if (intCount_Data > 0)// && false)
                {
                    string strSheetText = "8";
                    string strTitleText = "部核購案統計表";
                    string strFileName = $"{ strTitleText }.xlsx";

                    MemoryStream Memory = ExcelNPOI.RenderDataTableToStream_Chief(dt, strSheetText, strTitleText, 3, strFormat); //取得Excel資料流
                    FCommon.DownloadFile(this, strFileName, Memory); //直接下載檔案
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查詢無結果，請重新下條件！");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "單位代碼錯誤，請重新登入！");
        }
        //9.辦理公開閱覽購案統計表
        private void Query_OpenRead()
        {
            if (strDEPT_SN != null)
            {
                string[] strParameterName = { ":strYear", ":USER_DEPT_SN" };
                ArrayList aryData = new ArrayList();

                string strYear = drpYear_OpenRead.SelectedValue;
                if (strYear.Length > 2)
                    strYear = strYear.Substring(strYear.Length - 2, 2);
                aryData.Add(strYear);
                aryData.Add(strDEPT_SN);
                
                #region SQL語法
                string strSQL = "";
                #region 欄位
                strSQL += $@"
                    select 
                    t_CLASS.OVC_PHR_DESC OVC_CLASS, --h.OVC_CLASS, --//單位屬性
                    t_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --a.OVC_PUR_NSECTION, --//委購單位(=申購單位?)
                    a.OVC_PURCH||a.OVC_PUR_AGENCY||E.OVC_PURCH_5 PURCH, --a.OVC_PURCH, --//購案編號
                    a.OVC_PUR_IPURCH, --//購案名稱
                    t_PUR_ASS_VEN_CODE.OVC_PHR_DESC OVC_PUR_ASS_VEN_CODE, --NVL(a.OVC_PUR_ASS_VEN_CODE,' ') OVC_PUR_ASS_VEN_CODE, --//招標方式代碼
                    t_LAB.OVC_PHR_DESC OVC_LAB, --a.OVC_LAB, --//採購屬性代碼
                    NVL((a.ONB_PUR_BUDGET-f.ONB_BID_RESULT), 0) ONB_PUR_BUDGET, --//預劃金額
                    --//額外查詢 公開閱覽日期
                    --//額外查詢 評核承辦人

                    a.OVC_PURCH

                    ----以下沒用到----
                    --a.OVC_PUR_AGENCY,
                    --a.OVC_PUR_SECTION,
                    --B.OVC_DCANCEL,
                    --b.OVC_CANCEL_REASON,
                    --a.OVC_AGNT_IN,
                    --a.OVC_PUR_DCANPO,
                    --a.OVC_PLAN_PURCH,
                    --a.OVC_PUR_USER,
                    --a.OVC_PUR_CURRENT,
                    --a.ONB_PUR_RATE,
                    --a.ONB_PUR_BUDGET_NT,
                    --B.OVC_DAPPLY,
                    --a.OVC_DPROPOSE,
                    --a.OVC_PUR_DAPPROVE,
                    --g.ONB_RATE,
                    --decode(g.OVC_VEN_TITLE,null,f.OVC_VENDORS_NAME,g.OVC_VEN_TITLE) OVC_VENDORS_NAME,
                    --a.OVC_PUR_DCANRE,
                    --B.OVC_PUR_APPROVE_DEP,
                    --B.OVC_AUDIT_UNIT
                    ";
                #endregion
                #region 資料表
                strSQL += $@"
                    from
                    TBM1301 a,
                    TBM1301_PLAN b,
                    VIMAXSTATUS e,
                    VIBID_RESULT f,
                    VITBM1302 g,
                    TBMDEPT h,

                    (SELECT OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PUR_SECTION, --//申購單位
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='S9') t_CLASS, --//單位屬性
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='GN') t_LAB, --//採購屬性
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='C7') t_PUR_ASS_VEN_CODE --//招標方式
                    ";
                #endregion
                #region join 條件
                strSQL += $@"
                    where a.OVC_PURCH = b.OVC_PURCH
                    and a.OVC_PURCH = e.OVC_PURCH(+)
                    and e.OVC_PURCH = f.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = f.OVC_PURCH_5(+)
                    and e.OVC_PURCH = g.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = g.OVC_PURCH_5(+)
                    and a.OVC_PUR_SECTION = h.OVC_DEPT_CDE

                    and a.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    and h.OVC_CLASS = t_CLASS.OVC_PHR_ID(+)
                    and a.OVC_LAB = t_LAB.OVC_PHR_ID(+)
                    and a.OVC_PUR_ASS_VEN_CODE = t_PUR_ASS_VEN_CODE.OVC_PHR_ID(+)
                    ";
                #endregion
                #region 篩選條件
                //--+加欄位篩選(有選擇的條件(下方有說明)+
                string strOVC_DELIVERY1 = txt_OpenRead1.Text;
                if (!strOVC_DELIVERY1.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_DPROPOSE >= '{ strOVC_DELIVERY1 }'
                    ";
                string strOVC_DELIVERY2 = txt_OpenRead2.Text;
                if (!strOVC_DELIVERY2.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_DPROPOSE <= '{ strOVC_DELIVERY2 }'
                    ";
                #endregion
                #region 基本條件 & 排序
                if (strDEPT_SN.Equals("0A100"))
                {
                    strSQL += $@"
                        and(b.OVC_AUDIT_UNIT = { strParameterName[1] } --//計評單位=登錄者單位
                        or b.OVC_PUR_APPROVE_DEP = 'A') --//讀取採購中心權責購案(部核案)
                        ";
                }
                else
                    strSQL += $@"
                        and b.OVC_AUDIT_UNIT = { strParameterName[1] }
                        ";
                strSQL += $@"
                    and substr(a.OVC_PURCH,3,2) = { strParameterName[0] }
                    and b.OVC_OPEN_CHECK = 'Y'
                    order by a.OVC_PURCH
                    ";
                #endregion
                #region 額外欄位查詢語法
                string[] strParameterName_Other = { ":vOVC_PURCH" };
                ArrayList aryData_Other = new ArrayList();
                aryData_Other.Add("");
                //公開閱覽日期 OVC_OPEN_PERIOD
                string strSQL_OVC_OPEN_PERIOD = $@"select OVC_OPEN_PERIOD from TBMANNOUNCE_OPEN where OVC_PURCH={ strParameterName_Other[0] }";
                //評核承辦人 OVC_CHECKER
                string[] strParameterName_OVC_CHECKER = { ":vOVC_PURCH", ":USER_DEPT_SN" };
                ArrayList aryData_OVC_CHECKER = new ArrayList();
                aryData_OVC_CHECKER.Add("");
                aryData_OVC_CHECKER.Add(strDEPT_SN);
                string strSQL_OVC_CHECKER = $@"
                    select OVC_CHECKER
                    from TBM1202 a,
                    (select OVC_PURCH, max(ONB_CHECK_TIMES) TIMES
                        from TBM1202 where OVC_PURCH = { strParameterName_OVC_CHECKER[0] } and OVC_CHECK_UNIT = { strParameterName_OVC_CHECKER[1] }
                        group by OVC_PURCH) B
                    where a.OVC_PURCH = B.OVC_PURCH and a.ONB_CHECK_TIMES = B.TIMES and a.OVC_CHECK_UNIT = { strParameterName_OVC_CHECKER[1] }
                ";
                #endregion
                string[] strFieldNames = { "單位屬性", "委購單位", "購案編號", "購案名稱", "招標方式", "採購屬性",
                    "預劃金額", "公開閱覽日期", "評核承辦人" };
                string[] strFieldSqls = { "OVC_CLASS", "OVC_PUR_SECTION", "PURCH", "OVC_PUR_IPURCH", "OVC_PUR_ASS_VEN_CODE", "OVC_LAB",
                    "ONB_PUR_BUDGET", "OVC_OPEN_PERIOD", "OVC_CHECKER" };
                string[] strFormat = { null, null, null, null, null, null,
                    strFormatMoney2, null, null };
                #endregion

                //FCommon.AlertShow(PnMessage, "danger", "系統訊息", strSQL);
                DataTable dt_Source = FCommon.getDataTableFromSelect(strSQL, strParameterName, aryData);
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
                    aryData_OVC_CHECKER[0] = strOVC_PURCH;

                    DataTable dt_OVC_OPEN_PERIOD = FCommon.getDataTableFromSelect(strSQL_OVC_OPEN_PERIOD, strParameterName_Other, aryData_Other);
                    if (dt_OVC_OPEN_PERIOD.Rows.Count > 0)
                        dr["OVC_OPEN_PERIOD"] = dt_OVC_OPEN_PERIOD.Rows[0]["OVC_OPEN_PERIOD"];
                    DataTable dt_OVC_CHECKER = FCommon.getDataTableFromSelect(strSQL_OVC_CHECKER, strParameterName_OVC_CHECKER, aryData_OVC_CHECKER);
                    if (dt_OVC_CHECKER.Rows.Count > 0)
                        dr["OVC_CHECKER"] = dt_OVC_CHECKER.Rows[0]["OVC_CHECKER"];
                }

                DataTable dt = getDataTable_Export(dt_Source, strFieldNames, strFieldSqls, out intCount_Data);

                if (intCount_Data > 0)// && false)
                {
                    string strSheetText = "9";
                    string strTitleText = "辦理公開閱覽購案統計表";
                    string strFileName = $"{ strTitleText }.xlsx";

                    MemoryStream Memory = ExcelNPOI.RenderDataTableToStream_Chief(dt, strSheetText, strTitleText, 3, strFormat); //取得Excel資料流
                    FCommon.DownloadFile(this, strFileName, Memory); //直接下載檔案
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查詢無結果，請重新下條件！");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "單位代碼錯誤，請重新登入！");
        }
        //10.尚未完成資訊審查作業購案統計表
        private void Query_UndonePurSta()
        {
            if (strDEPT_SN != null)
            {
                string[] strParameterName = { ":strYear", ":USER_DEPT_SN" };
                ArrayList aryData = new ArrayList();

                string strYear = drpYear_UndonePurSta.SelectedValue;
                if (strYear.Length > 2)
                    strYear = strYear.Substring(strYear.Length - 2, 2);
                aryData.Add(strYear);
                aryData.Add(strDEPT_SN);

                #region SQL語法
                string strSQL = "";
                #region 欄位
                strSQL += $@"
                    select 
                    t_CLASS.OVC_PHR_DESC OVC_CLASS, --h.OVC_CLASS, --//單位屬性
                    t_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --a.OVC_PUR_NSECTION, --//委購單位(=申購單位?)
                    a.OVC_PURCH||a.OVC_PUR_AGENCY||E.OVC_PURCH_5 PURCH, --a.OVC_PURCH, --//購案編號
                    a.OVC_PUR_IPURCH, --//購案名稱
                    a.OVC_PUR_USER, --//委方承辦人
                    a.OVC_PUR_IUSER_PHONE_EXT, a.OVC_PUR_IUSER_PHONE_EXT1, --//委方聯絡電話
                    --//額外查詢 備註

                    a.OVC_PURCH
                    ";
                #endregion
                #region 資料表
                strSQL += $@"
                    from
                    TBM1301 a,
                    TBM1301_PLAN b,
                    VIMAXSTATUS e,
                    VIBID_RESULT f,
                    VITBM1302 g,
                    TBMDEPT h,

                    (SELECT OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PUR_SECTION, --//申購單位
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='S9') t_CLASS --//單位屬性
                    ";
                #endregion
                #region join 條件
                strSQL += $@"
                    where a.OVC_PURCH = b.OVC_PURCH
                    and a.OVC_PURCH = e.OVC_PURCH(+)
                    and e.OVC_PURCH = f.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = f.OVC_PURCH_5(+)
                    --and e.OVC_PURCH = g.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = g.OVC_PURCH_5(+)
                    and a.OVC_PUR_SECTION = h.OVC_DEPT_CDE

                    and a.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    and h.OVC_CLASS = t_CLASS.OVC_PHR_ID(+)
                    ";
                #endregion
                #region 篩選條件
                //--+加欄位篩選(有選擇的條件(下方有說明)+
                string strOVC_DELIVERY1 = txt_UndonePurSta1.Text;
                if (!strOVC_DELIVERY1.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_DPROPOSE >= '{ strOVC_DELIVERY1 }'
                    ";
                string strOVC_DELIVERY2 = txt_UndonePurSta2.Text;
                if (!strOVC_DELIVERY2.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_DPROPOSE <= '{ strOVC_DELIVERY2 }'
                    ";
                #endregion
                #region 基本條件 & 排序
                if (strDEPT_SN.Equals("0A100"))
                {
                    strSQL += $@"
                        and(b.OVC_AUDIT_UNIT = { strParameterName[1] } --//計評單位=登錄者單位
                        or b.OVC_PUR_APPROVE_DEP = 'A') --//讀取採購中心權責購案(部核案)
                        ";
                }
                else
                    strSQL += $@"
                        and b.OVC_AUDIT_UNIT = { strParameterName[1] }
                        ";
                strSQL += $@"
                    and substr(a.OVC_PURCH,3,2) = { strParameterName[0] }
                    and b.OVC_FAC_CHECK = 'Y'
                    order by a.OVC_PURCH
                    ";
                #endregion
                #region 額外欄位查詢語法
                //備註 OVC_FAC_STATUS
                string[] strParameterName_OVC_FAC_STATUS = { ":vOVC_PURCH", ":vOVC_CHECK_UNIT" };
                ArrayList aryData_OVC_FAC_STATUS = new ArrayList();
                aryData_OVC_FAC_STATUS.Add("");
                aryData_OVC_FAC_STATUS.Add(strDEPT_SN);
                string strSQL_OVC_FAC_STATUS = $@"select OVC_FAC_STATUS from TBMFAC_STATUS where OVC_PURCH = { strParameterName_OVC_FAC_STATUS[0] } and OVC_CHECK_UNIT = { strParameterName_OVC_FAC_STATUS[1] }";
                #endregion
                string[] strFieldNames = { "單位屬性", "委購單位", "購案編號", "購案名稱", "委方承辦人", "委方聯絡電話", "備註" };
                string[] strFieldSqls = { "OVC_CLASS", "OVC_PUR_SECTION", "PURCH", "OVC_PUR_IPURCH", "OVC_PUR_USER", "OVC_PUR_IUSER_PHONE_EXT", "OVC_FAC_STATUS" };
                string[] strFormat = { null, null, null, null, null, null, null };
                #endregion

                //FCommon.AlertShow(PnMessage, "danger", "系統訊息", strSQL);
                DataTable dt_Source = FCommon.getDataTableFromSelect(strSQL, strParameterName, aryData);
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
                    aryData_OVC_FAC_STATUS[0] = strOVC_PURCH;

                    //委方聯絡電話
                    string strOVC_PUR_IUSER_PHONE_EXT = dr["OVC_PUR_IUSER_PHONE_EXT"].ToString();
                    string strOVC_PUR_IUSER_PHONE_EXT1 = dr["OVC_PUR_IUSER_PHONE_EXT1"].ToString();
                    if (!strOVC_PUR_IUSER_PHONE_EXT1.Equals(string.Empty))
                    {
                        if (!strOVC_PUR_IUSER_PHONE_EXT.Equals(string.Empty))
                            strOVC_PUR_IUSER_PHONE_EXT = $"{ strOVC_PUR_IUSER_PHONE_EXT };{ strOVC_PUR_IUSER_PHONE_EXT1 }";
                        else
                            strOVC_PUR_IUSER_PHONE_EXT = strOVC_PUR_IUSER_PHONE_EXT1;
                        if (strOVC_PUR_IUSER_PHONE_EXT.Length > dt_Source.Columns["OVC_PUR_IUSER_PHONE_EXT"].MaxLength)
                            dt_Source.Columns["OVC_PUR_IUSER_PHONE_EXT"].MaxLength = strOVC_PUR_IUSER_PHONE_EXT.Length;
                        dr["OVC_PUR_IUSER_PHONE_EXT"] = strOVC_PUR_IUSER_PHONE_EXT;
                    }

                    DataTable dt_OVC_FAC_STATUS = FCommon.getDataTableFromSelect(strSQL_OVC_FAC_STATUS, strParameterName_OVC_FAC_STATUS, aryData_OVC_FAC_STATUS);
                    if (dt_OVC_FAC_STATUS.Rows.Count > 0)
                        dr["OVC_FAC_STATUS"] = dt_OVC_FAC_STATUS.Rows[0]["OVC_FAC_STATUS"];
                }

                DataTable dt = getDataTable_Export(dt_Source, strFieldNames, strFieldSqls, out intCount_Data);

                if (intCount_Data > 0)// && false)
                {
                    string strSheetText = "10";
                    string strTitleText = "尚未完成資訊審查作業購案統計表";
                    string strFileName = $"{ strTitleText }.xlsx";

                    MemoryStream Memory = ExcelNPOI.RenderDataTableToStream_Chief(dt, strSheetText, strTitleText, 3, strFormat); //取得Excel資料流
                    FCommon.DownloadFile(this, strFileName, Memory); //直接下載檔案
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查詢無結果，請重新下條件！");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "單位代碼錯誤，請重新登入！");
        }
        //11.採用最有利標購案統計表
        private void Query_MostFavS()
        {
            if (strDEPT_SN != null)
            {
                string[] strParameterName = { ":strYear", ":USER_DEPT_SN" };
                ArrayList aryData = new ArrayList();

                string strYear = drpYear_MostFavS.SelectedValue;
                if (strYear.Length > 2)
                    strYear = strYear.Substring(strYear.Length - 2, 2);
                aryData.Add(strYear);
                aryData.Add(strDEPT_SN);

                #region SQL語法
                string strSQL = "";
                #region 欄位
                strSQL += $@"
                    select 
                    t_CLASS.OVC_PHR_DESC OVC_CLASS, --h.OVC_CLASS, --//單位屬性
                    t_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --a.OVC_PUR_NSECTION, --//委購單位(=申購單位?)
                    a.OVC_PURCH||a.OVC_PUR_AGENCY PURCH, --a.OVC_PURCH, --//購案編號
                    a.OVC_PUR_IPURCH, --//購案名稱
                    t_PUR_ASS_VEN_CODE.OVC_PHR_DESC OVC_PUR_ASS_VEN_CODE, --NVL(a.OVC_PUR_ASS_VEN_CODE,' ') OVC_PUR_ASS_VEN_CODE, --//招標方式代碼
                    t_LAB.OVC_PHR_DESC OVC_LAB, --a.OVC_LAB --//採購屬性代碼
                    t_PLAN_PURCH.OVC_PHR_DESC OVC_PLAN_PURCH, --a.OVC_PLAN_PURCH, --//計畫性質代碼 計畫/非計畫
                    NVL((a.ONB_PUR_BUDGET-f.ONB_BID_RESULT), 0) ONB_PUR_BUDGET, --//預劃金額
                    --//額外查詢 評核承辦人
                    --//額外查詢 備註

                    a.OVC_PURCH
                    ";
                #endregion
                #region 資料表
                strSQL += $@"
                    from
                    TBM1301 a,
                    TBM1301_PLAN b,
                    VIMAXSTATUS e,
                    VIBID_RESULT f,
                    VITBM1302 g,
                    TBMDEPT h,

                    (SELECT OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PUR_SECTION, --//申購單位
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='S9') t_CLASS, --//單位屬性
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='TD') t_PLAN_PURCH, --//計畫性質
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='GN') t_LAB, --//採購屬性
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='C7') t_PUR_ASS_VEN_CODE --//招標方式
                    ";
                #endregion
                #region join 條件
                strSQL += $@"
                    where a.OVC_PURCH = b.OVC_PURCH
                    and a.OVC_PURCH = e.OVC_PURCH(+)
                    and e.OVC_PURCH = f.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = f.OVC_PURCH_5(+)
                    and e.OVC_PURCH = g.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = g.OVC_PURCH_5(+)
                    and a.OVC_PUR_SECTION = h.OVC_DEPT_CDE

                    and a.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    and h.OVC_CLASS = t_CLASS.OVC_PHR_ID(+)
                    and a.OVC_PLAN_PURCH = t_PLAN_PURCH.OVC_PHR_ID(+)
                    and a.OVC_LAB = t_LAB.OVC_PHR_ID(+)
                    and a.OVC_PUR_ASS_VEN_CODE = t_PUR_ASS_VEN_CODE.OVC_PHR_ID(+)
                    ";
                #endregion
                #region 篩選條件
                //--+加欄位篩選(有選擇的條件(下方有說明)+
                string strOVC_DELIVERY1 = txt_MostFavS1.Text;
                if (!strOVC_DELIVERY1.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_DPROPOSE >= '{ strOVC_DELIVERY1 }'
                    ";
                string strOVC_DELIVERY2 = txt_MostFavS2.Text;
                if (!strOVC_DELIVERY2.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_DPROPOSE <= '{ strOVC_DELIVERY2 }'
                    ";
                #endregion
                #region 基本條件 & 排序
                if (strDEPT_SN.Equals("0A100"))
                {
                    strSQL += $@"
                        and(b.OVC_AUDIT_UNIT = { strParameterName[1] } --//計評單位=登錄者單位
                        or b.OVC_PUR_APPROVE_DEP = 'A') --//讀取採購中心權責購案(部核案)
                        ";
                }
                else
                    strSQL += $@"
                        and b.OVC_AUDIT_UNIT = { strParameterName[1] }
                        ";
                strSQL += $@"
                    and substr(a.OVC_PURCH,3,2) = { strParameterName[0] }
                    and a.OVC_BID = '3'
                    order by a.OVC_PURCH
                    ";
                #endregion
                #region 額外欄位查詢語法
                string[] strParameterName_Other = { ":vOVC_PURCH" };
                ArrayList aryData_Other = new ArrayList();
                aryData_Other.Add("");
                //評核承辦人 OVC_CHECKER
                string[] strParameterName_OVC_CHECKER = { ":vOVC_PURCH", ":USER_DEPT_SN" };
                ArrayList aryData_OVC_CHECKER = new ArrayList();
                aryData_OVC_CHECKER.Add("");
                aryData_OVC_CHECKER.Add(strDEPT_SN);
                string strSQL_OVC_CHECKER = $@"
                    select OVC_CHECKER
                    from TBM1202 a,
                    (select OVC_PURCH, max(ONB_CHECK_TIMES) TIMES
                        from TBM1202 where OVC_PURCH = { strParameterName_OVC_CHECKER[0] } and OVC_CHECK_UNIT = { strParameterName_OVC_CHECKER[1] }
                        group by OVC_PURCH) B
                    where a.OVC_PURCH = B.OVC_PURCH and a.ONB_CHECK_TIMES = B.TIMES and a.OVC_CHECK_UNIT = { strParameterName_OVC_CHECKER[1] }
                ";
                //備註 OVC_FAC_STATUS
                string[] strParameterName_OVC_FAC_STATUS = { ":vOVC_PURCH", ":vOVC_CHECK_UNIT" };
                ArrayList aryData_OVC_FAC_STATUS = new ArrayList();
                aryData_OVC_FAC_STATUS.Add("");
                aryData_OVC_FAC_STATUS.Add(strDEPT_SN);
                string strSQL_OVC_FAC_STATUS = $@"select OVC_FAC_STATUS from TBMFAC_STATUS where OVC_PURCH = { strParameterName_OVC_FAC_STATUS[0] } and OVC_CHECK_UNIT = { strParameterName_OVC_FAC_STATUS[1] }";
                #endregion
                string[] strFieldNames = { "單位屬性", "委購單位", "購案編號", "購案名稱", "招標方式", "採購屬性", "計畫/非計畫",
                    "預劃金額", "評核承辦人", "備註" };
                string[] strFieldSqls = { "OVC_CLASS", "OVC_PUR_SECTION", "PURCH", "OVC_PUR_IPURCH", "OVC_PUR_ASS_VEN_CODE", "OVC_LAB", "OVC_PLAN_PURCH",
                    "ONB_PUR_BUDGET", "OVC_CHECKER", "OVC_FAC_STATUS" };
                string[] strFormat = { null, null, null, null, null, null, null,
                    strFormatMoney2, null, null };
                #endregion
                //FCommon.AlertShow(PnMessage, "danger", "系統訊息", strSQL);
                DataTable dt_Source = FCommon.getDataTableFromSelect(strSQL, strParameterName, aryData);
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
                    aryData_OVC_CHECKER[0] = strOVC_PURCH;
                    aryData_OVC_FAC_STATUS[0] = strOVC_PURCH;

                    DataTable dt_OVC_CHECKER = FCommon.getDataTableFromSelect(strSQL_OVC_CHECKER, strParameterName_OVC_CHECKER, aryData_OVC_CHECKER);
                    if (dt_OVC_CHECKER.Rows.Count > 0)
                        dr["OVC_CHECKER"] = dt_OVC_CHECKER.Rows[0]["OVC_CHECKER"];
                    DataTable dt_OVC_FAC_STATUS = FCommon.getDataTableFromSelect(strSQL_OVC_FAC_STATUS, strParameterName_OVC_FAC_STATUS, aryData_OVC_FAC_STATUS);
                    if (dt_OVC_FAC_STATUS.Rows.Count > 0)
                        dr["OVC_FAC_STATUS"] = dt_OVC_FAC_STATUS.Rows[0]["OVC_FAC_STATUS"];
                }

                DataTable dt = getDataTable_Export(dt_Source, strFieldNames, strFieldSqls, out intCount_Data);

                if (intCount_Data > 0)// && false)
                {
                    string strSheetText = "11";
                    string strTitleText = "採用最有利標購案統計表";
                    string strFileName = $"{ strTitleText }.xlsx";

                    MemoryStream Memory = ExcelNPOI.RenderDataTableToStream_Chief(dt, strSheetText, strTitleText, 3, strFormat); //取得Excel資料流
                    FCommon.DownloadFile(this, strFileName, Memory); //直接下載檔案
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查詢無結果，請重新下條件！");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "單位代碼錯誤，請重新登入！");
        }
        //12.已核定購案統計表
        private void Query_ApprovedPur()
        {
            if (strDEPT_SN != null)
            {
                string[] strParameterName = { ":strYear", ":USER_DEPT_SN" };
                ArrayList aryData = new ArrayList();

                string strYear = drpYear_ApprovedPur.SelectedValue;
                if (strYear.Length > 2)
                    strYear = strYear.Substring(strYear.Length - 2, 2);
                aryData.Add(strYear);
                aryData.Add(strDEPT_SN);

                #region SQL語法
                string strSQL = "";
                #region 欄位
                strSQL += $@"
                    select 
                    t_CLASS.OVC_PHR_DESC OVC_CLASS, --h.OVC_CLASS, --//單位屬性
                    t_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --a.OVC_PUR_NSECTION, --//委購單位(=申購單位?)
                    a.OVC_PURCH||a.OVC_PUR_AGENCY PURCH, --a.OVC_PURCH, --//購案編號
                    a.OVC_PUR_IPURCH, --//購案名稱
                    t_PUR_ASS_VEN_CODE.OVC_PHR_DESC OVC_PUR_ASS_VEN_CODE, --NVL(a.OVC_PUR_ASS_VEN_CODE,' ') OVC_PUR_ASS_VEN_CODE, --//招標方式代碼
                    t_LAB.OVC_PHR_DESC OVC_LAB, --a.OVC_LAB, --//採購屬性代碼
                    t_PLAN_PURCH.OVC_PHR_DESC OVC_PLAN_PURCH, --a.OVC_PLAN_PURCH, --//計畫性質代碼 計畫/非計畫
                    NVL((a.ONB_PUR_BUDGET-f.ONB_BID_RESULT), 0) ONB_PUR_BUDGET, --//預劃金額
                    a.ONB_PUR_BUDGET_NT, --//核定預算金額 預劃金額(台幣)
                    NVL((f.ONB_BID_RESULT * g.ONB_RATE), 0) ONB_BID_RESULT_NT, --//決標金額
                    (a.OVC_PUR_DAPPROVE|| case when a.OVC_PUR_ALLOW is null then '' else '('||a.OVC_PUR_ALLOW||')' end) OVC_PUR_DAPPROVE_ALLOW, --//核定日期(主官核批日)
                    --//額外查詢 備註

                    a.OVC_PURCH
                    ";
                #endregion
                #region 資料表
                strSQL += $@"
                    from
                    TBM1301 a,
                    TBM1301_PLAN b,
                    VIMAXSTATUS e,
                    VIBID_RESULT f,
                    VITBM1302 g,
                    TBMDEPT h,

                    (SELECT OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PUR_SECTION, --//申購單位
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='S9') t_CLASS, --//單位屬性
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='TD') t_PLAN_PURCH, --//計畫性質
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='GN') t_LAB, --//採購屬性
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='C7') t_PUR_ASS_VEN_CODE --//招標方式
                    ";
                #endregion
                #region join 條件
                strSQL += $@"
                    where a.OVC_PURCH = b.OVC_PURCH
                    and a.OVC_PURCH = e.OVC_PURCH(+)
                    and e.OVC_PURCH = f.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = f.OVC_PURCH_5(+)
                    and e.OVC_PURCH = g.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = g.OVC_PURCH_5(+)
                    and a.OVC_PUR_SECTION = h.OVC_DEPT_CDE

                    and a.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    and h.OVC_CLASS = t_CLASS.OVC_PHR_ID(+)
                    and a.OVC_PLAN_PURCH = t_PLAN_PURCH.OVC_PHR_ID(+)
                    and a.OVC_LAB = t_LAB.OVC_PHR_ID(+)
                    and a.OVC_PUR_ASS_VEN_CODE = t_PUR_ASS_VEN_CODE.OVC_PHR_ID(+)
                    ";
                #endregion
                #region 篩選條件
                //--+加欄位篩選(有選擇的條件(下方有說明)+
                string strOVC_DELIVERY1 = txt_ApprovedPur1.Text;
                if (!strOVC_DELIVERY1.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_DPROPOSE >= '{ strOVC_DELIVERY1 }'
                    ";
                string strOVC_DELIVERY2 = txt_ApprovedPur2.Text;
                if (!strOVC_DELIVERY2.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_DPROPOSE <= '{ strOVC_DELIVERY2 }'
                    ";
                #endregion
                #region 基本條件 & 排序
                if (strDEPT_SN.Equals("0A100"))
                {
                    strSQL += $@"
                        and(b.OVC_AUDIT_UNIT = { strParameterName[1] } --//計評單位=登錄者單位
                        or b.OVC_PUR_APPROVE_DEP = 'A') --//讀取採購中心權責購案(部核案)
                        ";
                }
                else
                    strSQL += $@"
                        and b.OVC_AUDIT_UNIT = { strParameterName[1] }
                        ";
                strSQL += $@"
                    and substr(a.OVC_PURCH,3,2) = { strParameterName[0] }
                    order by a.OVC_PURCH
                    ";
                #endregion
                #region 額外欄位查詢語法
                //備註 OVC_FAC_STATUS
                string[] strParameterName_OVC_FAC_STATUS = { ":vOVC_PURCH", ":vOVC_CHECK_UNIT" };
                ArrayList aryData_OVC_FAC_STATUS = new ArrayList();
                aryData_OVC_FAC_STATUS.Add("");
                aryData_OVC_FAC_STATUS.Add(strDEPT_SN);
                string strSQL_OVC_FAC_STATUS = $@"select OVC_FAC_STATUS from TBMFAC_STATUS where OVC_PURCH = { strParameterName_OVC_FAC_STATUS[0] } and OVC_CHECK_UNIT = { strParameterName_OVC_FAC_STATUS[1] }";
                #endregion
                string[] strFieldNames = { "單位屬性", "委購單位", "購案編號", "購案名稱", "招標方式", "採購屬性", "計畫/非計畫",
                    "預劃金額", "核定預算金額", "決標金額", "核定日期(主官核批日)", "備註" };
                string[] strFieldSqls = { "OVC_CLASS", "OVC_PUR_SECTION", "PURCH", "OVC_PUR_IPURCH", "OVC_PUR_ASS_VEN_CODE", "OVC_LAB", "OVC_PLAN_PURCH",
                    "ONB_PUR_BUDGET", "ONB_PUR_BUDGET_NT", "ONB_BID_RESULT_NT", "OVC_PUR_DAPPROVE_ALLOW", "OVC_FAC_STATUS" };
                string[] strFormat = { null, null, null, null, null, null, null,
                    strFormatMoney2, strFormatMoney2, strFormatMoney2, null, null };
                #endregion

                //FCommon.AlertShow(PnMessage, "danger", "系統訊息", strSQL);
                DataTable dt_Source = FCommon.getDataTableFromSelect(strSQL, strParameterName, aryData);
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
                    aryData_OVC_FAC_STATUS[0] = strOVC_PURCH;
                    
                    DataTable dt_OVC_FAC_STATUS = FCommon.getDataTableFromSelect(strSQL_OVC_FAC_STATUS, strParameterName_OVC_FAC_STATUS, aryData_OVC_FAC_STATUS);
                    if (dt_OVC_FAC_STATUS.Rows.Count > 0)
                        dr["OVC_FAC_STATUS"] = dt_OVC_FAC_STATUS.Rows[0]["OVC_FAC_STATUS"];
                }

                DataTable dt = getDataTable_Export(dt_Source, strFieldNames, strFieldSqls, out intCount_Data);

                if (intCount_Data > 0)// && false)
                {
                    string strSheetText = "12";
                    string strTitleText = "已核定購案統計表";
                    string strFileName = $"{ strTitleText }.xlsx";

                    MemoryStream Memory = ExcelNPOI.RenderDataTableToStream_Chief(dt, strSheetText, strTitleText, 3, strFormat); //取得Excel資料流
                    FCommon.DownloadFile(this, strFileName, Memory); //直接下載檔案
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查詢無結果，請重新下條件！");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "單位代碼錯誤，請重新登入！");
        }
        //13.辦理撤案購案統計表
        private void Query_CasePur()
        {
            if (strDEPT_SN != null)
            {
                string[] strParameterName = { ":strYear", ":USER_DEPT_SN" };
                ArrayList aryData = new ArrayList();

                string strYear = drpYear_CasePur.SelectedValue;
                if (strYear.Length > 2)
                    strYear = strYear.Substring(strYear.Length - 2, 2);
                aryData.Add(strYear);
                aryData.Add(strDEPT_SN);

                #region SQL語法
                string strSQL = "";
                #region 欄位
                strSQL += $@"
                    select 
                    t_CLASS.OVC_PHR_DESC OVC_CLASS, --h.OVC_CLASS, --//單位屬性
                    t_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --a.OVC_PUR_NSECTION, --//委購單位(=申購單位?)
                    a.OVC_PURCH||a.OVC_PUR_AGENCY||E.OVC_PURCH_5 PURCH, --a.OVC_PURCH, --//購案編號
                    a.OVC_PUR_IPURCH, --//購案名稱
                    t_PUR_ASS_VEN_CODE.OVC_PHR_DESC OVC_PUR_ASS_VEN_CODE, --NVL(a.OVC_PUR_ASS_VEN_CODE,' ') OVC_PUR_ASS_VEN_CODE, --//招標方式代碼
                    t_LAB.OVC_PHR_DESC OVC_LAB, --a.OVC_LAB, --//採購屬性代碼
                    t_PLAN_PURCH.OVC_PHR_DESC OVC_PLAN_PURCH, --a.OVC_PLAN_PURCH, --//計畫性質代碼 計畫/非計畫
                    NVL((a.ONB_PUR_BUDGET-f.ONB_BID_RESULT), 0) ONB_PUR_BUDGET, --//預劃金額
                    a.OVC_PUR_DCANRE, --//原因
                    a.OVC_PUR_USER, --//委方承辦人
                    a.OVC_PUR_IUSER_PHONE_EXT, a.OVC_PUR_IUSER_PHONE_EXT1, --//委方聯絡電話
                    (a.OVC_PUR_DAPPROVE|| case when a.OVC_PUR_ALLOW is null then '' else '('||a.OVC_PUR_ALLOW||')' end) OVC_PUR_DAPPROVE_ALLOW, --//核定日期(主官核批日)
                    --//額外查詢 備註

                    a.OVC_PURCH
                    ";
                #endregion
                #region 資料表
                strSQL += $@"
                    from
                    TBM1301 a,
                    TBM1301_PLAN b,
                    VIMAXSTATUS e,
                    VIBID_RESULT f,
                    VITBM1302 g,
                    TBMDEPT h,

                    (SELECT OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PUR_SECTION, --//申購單位
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='S9') t_CLASS, --//單位屬性
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='TD') t_PLAN_PURCH, --//計畫性質
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='GN') t_LAB, --//採購屬性
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='C7') t_PUR_ASS_VEN_CODE --//招標方式
                    ";
                #endregion
                #region join 條件
                strSQL += $@"
                    where a.OVC_PURCH = b.OVC_PURCH
                    and a.OVC_PURCH = e.OVC_PURCH(+)
                    and e.OVC_PURCH = f.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = f.OVC_PURCH_5(+)
                    and e.OVC_PURCH = g.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = g.OVC_PURCH_5(+)
                    and a.OVC_PUR_SECTION = h.OVC_DEPT_CDE

                    and a.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    and h.OVC_CLASS = t_CLASS.OVC_PHR_ID(+)
                    and a.OVC_PLAN_PURCH = t_PLAN_PURCH.OVC_PHR_ID(+)
                    and a.OVC_LAB = t_LAB.OVC_PHR_ID(+)
                    and a.OVC_PUR_ASS_VEN_CODE = t_PUR_ASS_VEN_CODE.OVC_PHR_ID(+)
                    ";
                #endregion
                #region 篩選條件
                //--+加欄位篩選(有選擇的條件(下方有說明)+
                string strOVC_DELIVERY1 = txt_CasePur1.Text;
                if (!strOVC_DELIVERY1.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_DPROPOSE >= '{ strOVC_DELIVERY1 }'
                    ";
                string strOVC_DELIVERY2 = txt_CasePur2.Text;
                if (!strOVC_DELIVERY2.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_DPROPOSE <= '{ strOVC_DELIVERY2 }'
                    ";
                #endregion
                #region 基本條件 & 排序
                if (strDEPT_SN.Equals("0A100"))
                {
                    strSQL += $@"
                        and(b.OVC_AUDIT_UNIT = { strParameterName[1] } --//計評單位=登錄者單位
                        or b.OVC_PUR_APPROVE_DEP = 'A') --//讀取採購中心權責購案(部核案)
                        ";
                }
                else
                    strSQL += $@"
                        and b.OVC_AUDIT_UNIT = { strParameterName[1] }
                        ";
                strSQL += $@"
                    and substr(a.OVC_PURCH,3,2) = { strParameterName[0] }
                    order by a.OVC_PURCH
                    ";
                #endregion
                #region 額外欄位查詢語法
                //備註 OVC_FAC_STATUS
                string[] strParameterName_OVC_FAC_STATUS = { ":vOVC_PURCH", ":vOVC_CHECK_UNIT" };
                ArrayList aryData_OVC_FAC_STATUS = new ArrayList();
                aryData_OVC_FAC_STATUS.Add("");
                aryData_OVC_FAC_STATUS.Add(strDEPT_SN);
                string strSQL_OVC_FAC_STATUS = $@"select OVC_FAC_STATUS from TBMFAC_STATUS where OVC_PURCH = { strParameterName_OVC_FAC_STATUS[0] } and OVC_CHECK_UNIT = { strParameterName_OVC_FAC_STATUS[1] }";
                #endregion
                string[] strFieldNames = { "單位屬性", "委購單位", "購案編號", "購案名稱", "招標方式", "採購屬性", "計畫/非計畫",
                    "預劃金額", "原因", "委方承辦人", "委方聯絡電話", "核定日期(主官核批日)", "備註" };
                string[] strFieldSqls = { "OVC_CLASS", "OVC_PUR_SECTION", "PURCH", "OVC_PUR_IPURCH", "OVC_PUR_ASS_VEN_CODE", "OVC_LAB", "OVC_PLAN_PURCH",
                    "ONB_PUR_BUDGET", "OVC_PUR_DCANRE", "OVC_PUR_USER", "OVC_PUR_IUSER_PHONE_EXT", "OVC_PUR_DAPPROVE_ALLOW", "OVC_FAC_STATUS" };
                string[] strFormat = { null, null, null, null, null, null, null,
                    strFormatMoney2, null, null, null, null, null };
                #endregion

                //FCommon.AlertShow(PnMessage, "danger", "系統訊息", strSQL);
                DataTable dt_Source = FCommon.getDataTableFromSelect(strSQL, strParameterName, aryData);
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
                    aryData_OVC_FAC_STATUS[0] = strOVC_PURCH;

                    //委方聯絡電話
                    string strOVC_PUR_IUSER_PHONE_EXT = dr["OVC_PUR_IUSER_PHONE_EXT"].ToString();
                    string strOVC_PUR_IUSER_PHONE_EXT1 = dr["OVC_PUR_IUSER_PHONE_EXT1"].ToString();
                    if (!strOVC_PUR_IUSER_PHONE_EXT1.Equals(string.Empty))
                    {
                        if (!strOVC_PUR_IUSER_PHONE_EXT.Equals(string.Empty))
                            strOVC_PUR_IUSER_PHONE_EXT = $"{ strOVC_PUR_IUSER_PHONE_EXT };{ strOVC_PUR_IUSER_PHONE_EXT1 }";
                        else
                            strOVC_PUR_IUSER_PHONE_EXT = strOVC_PUR_IUSER_PHONE_EXT1;
                        if (strOVC_PUR_IUSER_PHONE_EXT.Length > dt_Source.Columns["OVC_PUR_IUSER_PHONE_EXT"].MaxLength)
                            dt_Source.Columns["OVC_PUR_IUSER_PHONE_EXT"].MaxLength = strOVC_PUR_IUSER_PHONE_EXT.Length;
                        dr["OVC_PUR_IUSER_PHONE_EXT"] = strOVC_PUR_IUSER_PHONE_EXT;
                    }

                    DataTable dt_OVC_FAC_STATUS = FCommon.getDataTableFromSelect(strSQL_OVC_FAC_STATUS, strParameterName_OVC_FAC_STATUS, aryData_OVC_FAC_STATUS);
                    if (dt_OVC_FAC_STATUS.Rows.Count > 0)
                        dr["OVC_FAC_STATUS"] = dt_OVC_FAC_STATUS.Rows[0]["OVC_FAC_STATUS"];
                }

                DataTable dt = getDataTable_Export(dt_Source, strFieldNames, strFieldSqls, out intCount_Data);

                if (intCount_Data > 0)// && false)
                {
                    string strSheetText = "13";
                    string strTitleText = "辦理撤案購案統計表";
                    string strFileName = $"{ strTitleText }.xlsx";

                    MemoryStream Memory = ExcelNPOI.RenderDataTableToStream_Chief(dt, strSheetText, strTitleText, 3, strFormat); //取得Excel資料流
                    FCommon.DownloadFile(this, strFileName, Memory); //直接下載檔案
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查詢無結果，請重新下條件！");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "單位代碼錯誤，請重新登入！");
        }
        //14.購案統計表 未篩選 0A100
        private void Query_UnitPur()
        {
            if (strDEPT_SN != null)
            {
                string[] strParameterName = { ":strYear", ":USER_DEPT_SN" };
                ArrayList aryData = new ArrayList();

                string strYear = drpYear_UnitPur.SelectedValue;
                if (strYear.Length > 2)
                    strYear = strYear.Substring(strYear.Length - 2, 2);
                aryData.Add(strYear);
                aryData.Add(strDEPT_SN);

                #region SQL語法
                string strSQL = "";
                #region 欄位
                strSQL += $@"
                    select 
                    t_CLASS.OVC_PHR_DESC OVC_CLASS, --h.OVC_CLASS, --//單位屬性
                    T_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --a.OVC_PUR_NSECTION, --//委購單位(=申購單位?)
                    a.OVC_PURCH||a.OVC_PUR_AGENCY||E.OVC_PURCH_5 PURCH, --a.OVC_PURCH, --//購案編號
                    a.OVC_PUR_AGENCY, --//採購方式代碼
                    a.OVC_PUR_IPURCH, --//購案名稱
                    t_PUR_ASS_VEN_CODE.OVC_PHR_DESC OVC_PUR_ASS_VEN_CODE, --NVL(a.OVC_PUR_ASS_VEN_CODE,' ') OVC_PUR_ASS_VEN_CODE, --//招標方式代碼
                    t_LAB.OVC_PHR_DESC OVC_LAB, --a.OVC_LAB, --//採購屬性代碼
                    t_PLAN_PURCH.OVC_PHR_DESC OVC_PLAN_PURCH, --a.OVC_PLAN_PURCH, --//計畫性質代碼
                    --//額外查詢 預算科子目
                    NVL((a.ONB_PUR_BUDGET-f.ONB_BID_RESULT), 0) ONB_PUR_BUDGET, --//預劃金額
                    a.ONB_PUR_BUDGET_NT, --//核定預算金額 預劃金額(台幣)
                    NVL((f.ONB_BID_RESULT * g.ONB_RATE), 0) ONB_BID_RESULT_NT, --//決標金額
                    --//額外查詢 公開閱覽日期
                    --//額外查詢 FAC
                    --//額外查詢 評核承辦人
                    --//採購承辦人
                    a.OVC_PUR_USER, --//委方承辦人
                    a.OVC_PUR_IUSER_PHONE_EXT, a.OVC_PUR_IUSER_PHONE_EXT1, --//委方聯絡電話
                    b.OVC_DAPPLY, --//預計呈報日期(=預計申購日期?)
                    a.OVC_DPROPOSE, --//呈報日期(=申購日期?)
                    --//額外查詢 紙本送達日期
                    (a.OVC_PUR_DAPPROVE|| case when a.OVC_PUR_ALLOW is null then '' else '('||a.OVC_PUR_ALLOW||')' end) OVC_PUR_DAPPROVE_ALLOW, --//核定日期(主官核批日)
                    --//額外查詢 審查逾60天原因

                    a.OVC_PURCH
                    ";
                #endregion
                #region 資料表
                strSQL += $@"
                    from
                    TBM1301 a,
                    TBM1301_PLAN b,
                    VIMAXSTATUS e,
                    VIBID_RESULT f,
                    VITBM1302 g,
                    TBMDEPT h,

                    (SELECT OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PUR_SECTION, --//申購單位
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='S9') t_CLASS, --//單位屬性
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='TD') t_PLAN_PURCH, --//計畫性質
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='GN') t_LAB, --//採購屬性
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='C7') t_PUR_ASS_VEN_CODE --//招標方式
                    ";
                #endregion
                #region join 條件
                strSQL += $@"
                    where a.OVC_PURCH = b.OVC_PURCH
                    and a.OVC_PURCH = e.OVC_PURCH(+)
                    and e.OVC_PURCH = f.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = f.OVC_PURCH_5(+)
                    and e.OVC_PURCH = g.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = g.OVC_PURCH_5(+)
                    and a.OVC_PUR_SECTION = h.OVC_DEPT_CDE

                    and a.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    and h.OVC_CLASS = t_CLASS.OVC_PHR_ID(+)
                    and a.OVC_PLAN_PURCH = t_PLAN_PURCH.OVC_PHR_ID(+)
                    and a.OVC_LAB = t_LAB.OVC_PHR_ID(+)
                    and a.OVC_PUR_ASS_VEN_CODE = t_PUR_ASS_VEN_CODE.OVC_PHR_ID(+)
                    ";
                #endregion
                #region 篩選條件
                //--+加欄位篩選(有選擇的條件(下方有說明)+
                string strOVC_CLASS = drp_Unit.SelectedValue;
                if (!strOVC_CLASS.Equals(string.Empty))
                    strSQL += $@"
                        and h.OVC_CLASS = '{ strOVC_CLASS }' --//單位類別(代碼S9)
                    ";
                string strOVC_DELIVERY1 = txt_UnitPur1.Text;
                if (!strOVC_DELIVERY1.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_DPROPOSE >= '{ strOVC_DELIVERY1 }'
                    ";
                string strOVC_DELIVERY2 = txt_UnitPur2.Text;
                if (!strOVC_DELIVERY2.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_DPROPOSE <= '{ strOVC_DELIVERY2 }'
                    ";
                #endregion
                #region 基本條件 & 排序
                strSQL += $@"
                    and b.OVC_AUDIT_UNIT = { strParameterName[1] } --//計評單位=登錄者單位
                    and substr(a.OVC_PURCH,3,2) = { strParameterName[0] }
                    order by a.OVC_PURCH
                    ";
                #endregion
                #region 額外欄位查詢語法
                string[] strParameterName_Other = { ":vOVC_PURCH" };
                ArrayList aryData_Other = new ArrayList();
                aryData_Other.Add("");
                //預算科子目 OVC_POI_IBDG
                string strSQL_OVC_POI_IBDG = $@"select OVC_POI_IBDG from TBM1118  where OVC_PURCH = { strParameterName_Other[0] } order by OVC_POI_IBDG";
                //公開閱覽日期 OVC_OPEN_PERIOD
                string strSQL_OVC_OPEN_PERIOD = $@"select OVC_OPEN_PERIOD from TBMANNOUNCE_OPEN where OVC_PURCH={ strParameterName_Other[0] }";
                //FAC OVC_FAC_STATUS
                string[] strParameterName_OVC_FAC_STATUS = { ":vOVC_PURCH", ":vOVC_CHECK_UNIT" };
                ArrayList aryData_OVC_FAC_STATUS = new ArrayList();
                aryData_OVC_FAC_STATUS.Add("");
                aryData_OVC_FAC_STATUS.Add(strDEPT_SN);
                string strSQL_OVC_FAC_STATUS = $@"select OVC_FAC_STATUS from TBMFAC_STATUS where OVC_PURCH = { strParameterName_OVC_FAC_STATUS[0] } and OVC_CHECK_UNIT = { strParameterName_OVC_FAC_STATUS[1] }";
                //評核承辦人 OVC_CHECKER
                string[] strParameterName_OVC_CHECKER = { ":vOVC_PURCH", ":USER_DEPT_SN" };
                ArrayList aryData_OVC_CHECKER = new ArrayList();
                aryData_OVC_CHECKER.Add("");
                aryData_OVC_CHECKER.Add(strDEPT_SN);
                string strSQL_OVC_CHECKER = $@"
                    select OVC_CHECKER
                    from TBM1202 a,
                    (select OVC_PURCH, max(ONB_CHECK_TIMES) TIMES
                        from TBM1202 where OVC_PURCH = { strParameterName_OVC_CHECKER[0] } and OVC_CHECK_UNIT = { strParameterName_OVC_CHECKER[1] }
                        group by OVC_PURCH) B
                    where a.OVC_PURCH = B.OVC_PURCH and a.ONB_CHECK_TIMES = B.TIMES and a.OVC_CHECK_UNIT = { strParameterName_OVC_CHECKER[1] }
                ";
                //紙本送達日期 OVC_DRECEIVE_PAPER
                string strSQL_OVC_DRECEIVE_PAPER = $@"select max(OVC_DRECEIVE_PAPER) OVC_DRECEIVE_PAPER from TBM1202 where OVC_PURCH = { strParameterName_Other[0] }";
                //審查逾60天原因 OVC_REASON
                string strSQL_OVC_REASON = $@"
                    select '第'||ONB_NO||'次('||OVC_REASON||')' OVC_REASON from TBMAUDIT_LOG where OVC_PURCH = { strParameterName_Other[0] }
                ";
                #endregion
                string[] strFieldNames = { "單位屬性", "委購單位", "購案編號", "採購方式", "購案名稱", "招標方式", "採購屬性", "計畫/非計畫",
                    "預算科子目", "預劃金額", "核定預算金額", "決標金額", "公開閱覽日期", "FAC", "評核承辦人", "採購承辦人",
                    "委方承辦人", "委方聯絡電話", "預計呈報日期", "呈報日期", "紙本送達日期", "核定日期(主官核批日)", "審查逾60天原因" };
                string[] strFieldSqls = { "OVC_CLASS", "OVC_PUR_SECTION", "PURCH", "OVC_PUR_AGENCY", "OVC_PUR_IPURCH", "OVC_PUR_ASS_VEN_CODE", "OVC_LAB", "OVC_PLAN_PURCH",
                    "OVC_POI_IBDG", "ONB_PUR_BUDGET", "ONB_PUR_BUDGET_NT", "ONB_BID_RESULT_NT", "OVC_OPEN_PERIOD", "OVC_FAC_STATUS", "OVC_CHECKER", "",
                    "OVC_PUR_USER", "OVC_PUR_IUSER_PHONE_EXT", "OVC_DAPPLY", "OVC_DPROPOSE", "OVC_DRECEIVE_PAPER", "OVC_PUR_DAPPROVE_ALLOW", "OVC_REASON" };
                string[] strFormat = { null, null, null, null, null, null, null, null,
                    null, strFormatMoney2, strFormatMoney2, strFormatMoney2, null, null, null, null,
                    null, null, null, null, null, null, null };
                #endregion

                //FCommon.AlertShow(PnMessage, "danger", "系統訊息", strSQL);
                DataTable dt_Source = FCommon.getDataTableFromSelect(strSQL, strParameterName, aryData);
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
                    aryData_OVC_FAC_STATUS[0] = strOVC_PURCH;
                    aryData_OVC_CHECKER[0] = strOVC_PURCH;

                    //委方聯絡電話
                    string strOVC_PUR_IUSER_PHONE_EXT = dr["OVC_PUR_IUSER_PHONE_EXT"].ToString();
                    string strOVC_PUR_IUSER_PHONE_EXT1 = dr["OVC_PUR_IUSER_PHONE_EXT1"].ToString();
                    if (!strOVC_PUR_IUSER_PHONE_EXT1.Equals(string.Empty))
                    {
                        if (!strOVC_PUR_IUSER_PHONE_EXT.Equals(string.Empty))
                            strOVC_PUR_IUSER_PHONE_EXT = $"{ strOVC_PUR_IUSER_PHONE_EXT };{ strOVC_PUR_IUSER_PHONE_EXT1 }";
                        else
                            strOVC_PUR_IUSER_PHONE_EXT = strOVC_PUR_IUSER_PHONE_EXT1;
                        if (strOVC_PUR_IUSER_PHONE_EXT.Length > dt_Source.Columns["OVC_PUR_IUSER_PHONE_EXT"].MaxLength)
                            dt_Source.Columns["OVC_PUR_IUSER_PHONE_EXT"].MaxLength = strOVC_PUR_IUSER_PHONE_EXT.Length;
                        dr["OVC_PUR_IUSER_PHONE_EXT"] = strOVC_PUR_IUSER_PHONE_EXT;
                    }

                    DataTable dt_OVC_POI_IBDG = FCommon.getDataTableFromSelect(strSQL_OVC_POI_IBDG, strParameterName_Other, aryData_Other);
                    if (dt_OVC_POI_IBDG.Rows.Count > 0)
                    {
                        ArrayList aryOVC_POI_IBDG = new ArrayList();
                        foreach (DataRow dr_OVC_POI_IBDG in dt_OVC_POI_IBDG.Rows)
                        {
                            string strOVC_POI_IBDG = dr_OVC_POI_IBDG["OVC_POI_IBDG"].ToString();
                            bool isAdd = true;
                            foreach (string str in aryOVC_POI_IBDG)
                            {
                                if (strOVC_POI_IBDG.Equals(str))
                                {
                                    isAdd = false;
                                    break;
                                }
                            }
                            if (isAdd) aryOVC_POI_IBDG.Add(strOVC_POI_IBDG);
                        }
                        dr["OVC_POI_IBDG"] = string.Join(";", (string[])aryOVC_POI_IBDG.ToArray(typeof(string)));
                    }
                    DataTable dt_OVC_OPEN_PERIOD = FCommon.getDataTableFromSelect(strSQL_OVC_OPEN_PERIOD, strParameterName_Other, aryData_Other);
                    if (dt_OVC_OPEN_PERIOD.Rows.Count > 0)
                        dr["OVC_OPEN_PERIOD"] = dt_OVC_OPEN_PERIOD.Rows[0]["OVC_OPEN_PERIOD"];
                    DataTable dt_OVC_FAC_STATUS = FCommon.getDataTableFromSelect(strSQL_OVC_FAC_STATUS, strParameterName_OVC_FAC_STATUS, aryData_OVC_FAC_STATUS);
                    if (dt_OVC_FAC_STATUS.Rows.Count > 0)
                        dr["OVC_FAC_STATUS"] = dt_OVC_FAC_STATUS.Rows[0]["OVC_FAC_STATUS"];
                    DataTable dt_OVC_CHECKER = FCommon.getDataTableFromSelect(strSQL_OVC_CHECKER, strParameterName_OVC_CHECKER, aryData_OVC_CHECKER);
                    if (dt_OVC_CHECKER.Rows.Count > 0)
                        dr["OVC_CHECKER"] = dt_OVC_CHECKER.Rows[0]["OVC_CHECKER"];
                    DataTable dt_OVC_DRECEIVE_PAPER = FCommon.getDataTableFromSelect(strSQL_OVC_DRECEIVE_PAPER, strParameterName_Other, aryData_Other);
                    if (dt_OVC_DRECEIVE_PAPER.Rows.Count > 0)
                        dr["OVC_DRECEIVE_PAPER"] = dt_OVC_DRECEIVE_PAPER.Rows[0]["OVC_DRECEIVE_PAPER"];
                    DataTable dt_OVC_REASON = FCommon.getDataTableFromSelect(strSQL_OVC_REASON, strParameterName_Other, aryData_Other);
                    if (dt_OVC_REASON.Rows.Count > 0)
                        dr["OVC_REASON"] = dt_OVC_REASON.Rows[0]["OVC_REASON"];
                }

                DataTable dt = getDataTable_Export(dt_Source, strFieldNames, strFieldSqls, out intCount_Data);

                if (intCount_Data > 0)// && false)
                {
                    string strSheetText = "14";
                    string strTitleText = $"{ drp_Unit.SelectedItem.Text } 統計表";
                    string strFileName = $"{ strTitleText }.xlsx";

                    MemoryStream Memory = ExcelNPOI.RenderDataTableToStream_Chief(dt, strSheetText, strTitleText, 3, strFormat); //取得Excel資料流
                    FCommon.DownloadFile(this, strFileName, Memory); //直接下載檔案
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查詢無結果，請重新下條件！");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "單位代碼錯誤，請重新登入！");
        }
        //15.國防部所屬單位委託採購中心辦理購案暨委製案件採購作業節點管制表
        private void Query_MNDPur()
        {
            if (strDEPT_SN != null)
            {
                string[] strParameterName = { ":strYear", ":USER_DEPT_SN" };
                ArrayList aryData = new ArrayList();
                
                string strYearValue = drpYear_MNDPur.SelectedValue;
                string strYear = strYearValue;
                if (strYear.Length > 2)
                    strYear = strYear.Substring(strYear.Length - 2, 2);
                aryData.Add(strYear);
                aryData.Add(strDEPT_SN);

                #region SQL語法
                string strSQL = "";
                #region 欄位
                strSQL += $@"
                    select 
                    t_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --a.OVC_PUR_NSECTION, --//委託單位(=申購單位?)
                    --//額外查詢 年度
                    a.OVC_PURCH_KIND, --//類別
                    --//額外查詢 區分
                    (a.OVC_PURCH||a.OVC_PUR_AGENCY||h.OVC_PURCH_5||g.OVC_PURCH_6) PURCH, --//購案編號
                    a.OVC_PUR_IPURCH, --//購案名稱
                    --//額外查詢 計畫送達日期
                    a.OVC_PUR_DAPPROVE, --//核定日期
                    f.OVC_DBID, --//決標日期
                    NVL((f.ONB_BID_RESULT * f.ONB_RESULT_RATE), 0) ONB_PUR_BUDGET_NT, --a.ONB_PUR_BUDGET_NT, --//全案預算金額
                    --//額外查詢 當年度預算金額
                    NVL((f.ONB_BID_RESULT * f.ONB_RESULT_RATE), 0) BMONEY, --//全案決標金額
                    --//驗收及付款單位
                    --//額外查詢 履約期限
                    --//額外查詢 當年度履約期限
                    0 Field1, --//當年度可付款金額
                    0 Field2, --//預判保留金額
                    --//額外查詢 執行狀況
                    --//額外查詢 預算科目
                    g.OVC_NAME, --//承辦人 部分

                    a.OVC_PURCH

                    ----以下沒用到----
                    --e.OVC_PURCH_5,
                    --g.OVC_PURCH_6,
                    --a.OVC_PUR_AGENCY,
                    ";
                #endregion
                #region 資料表
                strSQL += $@"
                    from
                    TBM1301 a,
                    TBM1301_PLAN b,
                    VIMAXSTATUS e,
                    VIBID_RESULT f,
                    VITBM1302 g,
                    TBMRECEIVE_BID h,

                    (SELECT OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PUR_SECTION --//申購單位
                    ";
                #endregion
                #region join 條件
                strSQL += $@"
                    where a.OVC_PURCH = b.OVC_PURCH
                    and a.OVC_PURCH = e.OVC_PURCH(+)
                    and e.OVC_PURCH = f.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = f.OVC_PURCH_5(+)
                    and e.OVC_PURCH = g.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = g.OVC_PURCH_5(+)
                    and e.OVC_PURCH = h.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = h.OVC_PURCH_5(+)

                    and a.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    ";
                #endregion
                #region 篩選條件
                //--+加欄位篩選(有選擇的條件(下方有說明)+
                #endregion
                #region 基本條件 & 排序
                if (strDEPT_SN.Equals("0A100"))
                {
                    strSQL += $@"
                        and(b.OVC_AUDIT_UNIT = { strParameterName[1] } --//計評單位=登錄者單位
                        or b.OVC_PUR_APPROVE_DEP = 'A') --//讀取採購中心權責購案(部核案)
                        ";
                }
                else
                    strSQL += $@"
                        and b.OVC_AUDIT_UNIT = { strParameterName[1] }
                        ";
                strSQL += $@"
                    and (a.OVC_PUR_DCANPO is null or a.OVC_PUR_DCANPO = ' ')
                    and (b.OVC_DCANCEL is null or b.OVC_DCANCEL = ' ')
                    and substr(a.OVC_PURCH,3,2) = { strParameterName[0] }
                    order by a.OVC_PURCH
                    ";
                #endregion
                #region 額外欄位查詢語法
                string[] strParameterName_Other = { ":vOVC_PURCH" };
                ArrayList aryData_Other = new ArrayList();
                aryData_Other.Add("");
                //年度 OVC_YY 多筆
                string strSQL_OVC_YY = $@"select OVC_YY from TBM1118  where OVC_PURCH = { strParameterName_Other[0] } order by OVC_YY";
                //計畫送達日期 OVC_DRECEIVE_PAPER
                string strSQL_OVC_DRECEIVE_PAPER = $@"select max(OVC_DRECEIVE_PAPER) OVC_DRECEIVE_PAPER from TBM1202 where OVC_PURCH = { strParameterName_Other[0] }";
                //當年度預算金額 MONEY
                string[] strParameterName_MONEY = { ":vOVC_PURCH", ":vOVC_YY" };
                ArrayList aryData_MONEY = new ArrayList();
                aryData_MONEY.Add("");
                aryData_MONEY.Add("");
                string strSQL_MONEY = $@"select NVL(sum(ONB_RATE*ONB_MBUD), 0) MONEY from TBM1118 where OVC_PURCH = { strParameterName_MONEY[0] } and OVC_YY = { strParameterName_MONEY[1] }";
                //履約期限 OVC_DELIVERY_CONTRACT 多筆
                string strSQL_OVC_DELIVERY_CONTRACT = $@"
                    select '第'||ONB_SHIP_TIMES||'次('||NVL(OVC_DELIVERY_CONTRACT, 'null')||')' OVC_DELIVERY_CONTRACT
                    from TBMDELIVERY
                    where OVC_PURCH = { strParameterName_Other[0] }
                    order by ONB_SHIP_TIMES
                ";
                //當年度履約期限 OVC_DELIVERY_CONTRACT_THEN 多筆
                string strTemp = $"'{ (int.Parse(strYearValue) + 1911).ToString() }%'";
                string strSQL_OVC_DELIVERY_CONTRACT_THEN = $@"
                    select '第'||ONB_SHIP_TIMES||'次('||NVL(OVC_DELIVERY_CONTRACT, 'null')||')' OVC_DELIVERY_CONTRACT_THEN
                    from TBMDELIVERY
                    where OVC_PURCH = { strParameterName_Other[0] } and OVC_DELIVERY_CONTRACT like { strTemp }
                    order by ONB_SHIP_TIMES
                ";
                //執行狀況 OVC_APPROVE_COMMENT + OVC_COMM + OVC_RECEIVE_COMM
                string[] strParameterName_OVC_APPROVE_COMMENT = { ":vOVC_PURCH", ":vOVC_CHECK_UNIT" };
                ArrayList aryData_OVC_APPROVE_COMMENT = new ArrayList();
                aryData_OVC_APPROVE_COMMENT.Add("");
                aryData_OVC_APPROVE_COMMENT.Add(strDEPT_SN);
                string strSQL_OVC_APPROVE_COMMENT = $@"
                    select a.OVC_APPROVE_COMMENT from TBM1202 a,
                        (select OVC_PURCH,max(ONB_CHECK_TIMES) TIMES from TBM1202
                            where OVC_PURCH = { strParameterName_OVC_APPROVE_COMMENT[0] } and OVC_CHECK_UNIT = { strParameterName_OVC_APPROVE_COMMENT[1] } and OVC_CHECK_OK = 'Y' 
                            group by OVC_PURCH) B 
                        where a.OVC_PURCH = B.OVC_PURCH and a.ONB_CHECK_TIMES = B.TIMES
                ";
                string[] strParameterName_OVC_COMM = { ":vOVC_PURCH", ":vOVC_PURCH_5" };
                ArrayList aryData_OVC_COMM = new ArrayList();
                aryData_OVC_COMM.Add("");
                aryData_OVC_COMM.Add("");
                string strSQL_OVC_COMM = $@"
                    select OVC_COMM from TBMRECEIVE_BID_LOG
                    where OVC_PURCH = { strParameterName_OVC_COMM[0] } and OVC_PURCH_5 = { strParameterName_OVC_COMM[1] }
                ";
                //多筆
                string[] strParameterName_OVC_RECEIVE_COMM = { ":vOVC_PURCH", ":vOVC_PURCH_6" };
                ArrayList aryData_OVC_RECEIVE_COMM = new ArrayList();
                aryData_OVC_RECEIVE_COMM.Add("");
                aryData_OVC_RECEIVE_COMM.Add("");
                string strSQL_OVC_RECEIVE_COMM = $@"
                    select OVC_RECEIVE_COMM
                    from TBMRECEIVE_CONTRACT where OVC_PURCH = { strParameterName_OVC_RECEIVE_COMM[0] } and OVC_PURCH_6 = { strParameterName_OVC_RECEIVE_COMM[1] }
                ";
                //預算科目 OVC_POI_IBDG
                string strSQL_OVC_POI_IBDG = $@"select OVC_POI_IBDG from TBM1118  where OVC_PURCH = { strParameterName_Other[0] } order by OVC_POI_IBDG";
                //承辦人 OVC_NAME + OVC_CHECKER + OVC_DO_NAME
                //g.OVC_NAME + 
                string[] strParameterName_OVC_CHECKER = { ":vOVC_PURCH", ":vOVC_CHECK_UNIT" };
                ArrayList aryData_OVC_CHECKER = new ArrayList();
                aryData_OVC_CHECKER.Add("");
                aryData_OVC_CHECKER.Add(strDEPT_SN);
                string strSQL_OVC_CHECKER = $@"
                    select OVC_CHECKER
                    from TBM1202 a,
                    (select OVC_PURCH, max(ONB_CHECK_TIMES) TIMES
                        from TBM1202 where OVC_PURCH = { strParameterName_OVC_CHECKER[0] } and OVC_CHECK_UNIT = { strParameterName_OVC_CHECKER[1] }
                        group by OVC_PURCH) B
                    where a.OVC_PURCH = B.OVC_PURCH and a.ONB_CHECK_TIMES = B.TIMES and a.OVC_CHECK_UNIT = { strParameterName_OVC_CHECKER[1] }
                ";
                string strSQL_OVC_DO_NAME = $@"
                    select a.OVC_DO_NAME
                    from TBMRECEIVE_CONTRACT a,
                    (select OVC_PURCH, max(OVC_DO_NAME) OVC_DO_NAME from TBMRECEIVE_CONTRACT
                        where OVC_PURCH = { strParameterName_Other[0] }
                        group by OVC_PURCH) B
                    where a.OVC_PURCH = B.OVC_PURCH and a.OVC_DO_NAME = B.OVC_DO_NAME
                ";
                #endregion
                string[] strFieldNames = { "委託單位", "年度", "類別", "區分", "購案編號", "購案名稱",
                    "計畫送達日期", "核定日期", "決標日期", "全案預算金額", "當年度預算金額", "全案決標金額", "驗收及付款單位",
                    "履約期限", "當年度履約期限", "當年度可付款金額", "預判保留金額", "執行狀況", "預算科目", "承辦人" };
                string[] strFieldSqls = { "OVC_PUR_SECTION", "OVC_YY", "OVC_PURCH_KIND", "BUDGET_YEAR", "PURCH", "OVC_PUR_IPURCH",
                    "OVC_DRECEIVE_PAPER", "OVC_PUR_DAPPROVE", "OVC_DBID", "ONB_PUR_BUDGET_NT", "MONEY", "BMONEY", "",
                    "OVC_DELIVERY_CONTRACT", "OVC_DELIVERY_CONTRACT_THEN", "Field1", "Field2", "OVC_APPROVE_COMMENT", "OVC_POI_IBDG", "OVC_NAME" };
                string[] strFormat = { null, null, null, null, null, null,
                    null, null, null, strFormatMoney2, strFormatMoney2, strFormatMoney2, null,
                    null, null, strFormatMoney2, strFormatMoney2, null, null, null };
                #endregion

                //FCommon.AlertShow(PnMessage, "danger", "系統訊息", strSQL);
                DataTable dt_Source = FCommon.getDataTableFromSelect(strSQL, strParameterName, aryData);
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
                    aryData_MONEY[0] = strOVC_PURCH;
                    aryData_MONEY[1] = strOVC_PURCH; //vOVC_YY
                    aryData_OVC_APPROVE_COMMENT[0] = strOVC_PURCH;
                    //aryData_OVC_APPROVE_COMMENT[1] = strOVC_PURCH; //vOVC_CHECK_UNIT
                    aryData_OVC_COMM[0] = strOVC_PURCH;
                    aryData_OVC_COMM[1] = strOVC_PURCH; //vOVC_PURCH_5
                    aryData_OVC_RECEIVE_COMM[0] = strOVC_PURCH;
                    aryData_OVC_RECEIVE_COMM[1] = strOVC_PURCH; //vOVC_PURCH_6
                    aryData_OVC_CHECKER[0] = strOVC_PURCH;
                    //aryData_OVC_CHECKER[1] = strOVC_PURCH; //vOVC_CHECK_UNIT
                    
                    //年度 OVC_YY 多筆
                    DataTable dt_OVC_YY = FCommon.getDataTableFromSelect(strSQL_OVC_YY, strParameterName_Other, aryData_Other);
                    ArrayList aryOVC_YY = getList(dr, dt_OVC_YY, "OVC_YY");
                    dr["OVC_YY"] = string.Join(";", (string[])aryOVC_YY.ToArray(typeof(string)));
                    //區分
                    int intCount_OVC_YY = aryOVC_YY.Count;
                    string strBUDGET_YEAR = "";
                    if (intCount_OVC_YY > 1)
                        strBUDGET_YEAR = "分年";
                    else if (intCount_OVC_YY == 1 && aryOVC_YY[0].Equals(strYearValue))
                        strBUDGET_YEAR = "當年";
                    dr["BUDGET_YEAR"] = strBUDGET_YEAR;
                    //計畫送達日期 OVC_DRECEIVE_PAPER
                    DataTable dt_OVC_DRECEIVE_PAPER = FCommon.getDataTableFromSelect(strSQL_OVC_DRECEIVE_PAPER, strParameterName_Other, aryData_Other);
                    if (dt_OVC_DRECEIVE_PAPER.Rows.Count > 0)
                        dr["OVC_DRECEIVE_PAPER"] = dt_OVC_DRECEIVE_PAPER.Rows[0]["OVC_DRECEIVE_PAPER"];
                    //當年度預算金額 MONEY
                    DataTable dt_MONEY = FCommon.getDataTableFromSelect(strSQL_MONEY, strParameterName_MONEY, aryData_MONEY);
                    if (dt_MONEY.Rows.Count > 0)
                        dr["MONEY"] = dt_MONEY.Rows[0]["MONEY"];
                    //履約期限 OVC_DELIVERY_CONTRACT 多筆
                    DataTable dt_OVC_DELIVERY_CONTRACT = FCommon.getDataTableFromSelect(strSQL_OVC_DELIVERY_CONTRACT, strParameterName_Other, aryData_Other);
                    if (dt_OVC_DELIVERY_CONTRACT.Rows.Count > 0)
                        dr["OVC_DELIVERY_CONTRACT"] = dt_OVC_DELIVERY_CONTRACT.Rows[0]["OVC_DELIVERY_CONTRACT"];
                    //當年度履約期限 OVC_DELIVERY_CONTRACT_THEN 多筆
                    DataTable dt_OVC_DELIVERY_CONTRACT_THEN = FCommon.getDataTableFromSelect(strSQL_OVC_DELIVERY_CONTRACT_THEN, strParameterName_Other, aryData_Other);
                    if (dt_OVC_DELIVERY_CONTRACT_THEN.Rows.Count > 0)
                        dr["OVC_DELIVERY_CONTRACT_THEN"] = dt_OVC_DELIVERY_CONTRACT_THEN.Rows[0]["OVC_DELIVERY_CONTRACT_THEN"];
                    //執行狀況 OVC_APPROVE_COMMENT + OVC_COMM + OVC_RECEIVE_COMM
                    string strOVC_APPROVE_COMMENT = "";
                    DataTable dt_OVC_APPROVE_COMMENT = FCommon.getDataTableFromSelect(strSQL_OVC_APPROVE_COMMENT, strParameterName_OVC_APPROVE_COMMENT, aryData_OVC_APPROVE_COMMENT);
                    if (dt_OVC_APPROVE_COMMENT.Rows.Count > 0)
                        strOVC_APPROVE_COMMENT = dt_OVC_APPROVE_COMMENT.Rows[0]["OVC_APPROVE_COMMENT"].ToString();
                    DataTable dt_OVC_COMM = FCommon.getDataTableFromSelect(strSQL_OVC_COMM, strParameterName_OVC_COMM, aryData_OVC_COMM);
                    if (dt_OVC_COMM.Rows.Count > 0)
                        strOVC_APPROVE_COMMENT += dt_OVC_COMM.Rows[0]["OVC_COMM"].ToString();
                    DataTable dt_OVC_RECEIVE_COMM = FCommon.getDataTableFromSelect(strSQL_OVC_RECEIVE_COMM, strParameterName_OVC_RECEIVE_COMM, aryData_OVC_RECEIVE_COMM); //多筆
                    if (dt_OVC_RECEIVE_COMM.Rows.Count > 0)
                        strOVC_APPROVE_COMMENT += dt_OVC_RECEIVE_COMM.Rows[0]["OVC_RECEIVE_COMM"].ToString();
                    dr["OVC_APPROVE_COMMENT"] = strOVC_APPROVE_COMMENT;
                    //預算科目 OVC_POI_IBDG 多筆
                    DataTable dt_OVC_POI_IBDG = FCommon.getDataTableFromSelect(strSQL_OVC_POI_IBDG, strParameterName_Other, aryData_Other);
                    setFieldList(dr, dt_OVC_POI_IBDG, "OVC_POI_IBDG");
                    //承辦人 OVC_NAME + OVC_CHECKER + OVC_DO_NAME
                    ArrayList aryOVC_NAME = new ArrayList();
                    string strOVC_NAME = dr["OVC_NAME"].ToString();
                    aryOVC_NAME.Add(strOVC_NAME);
                    DataTable dt_OVC_CHECKER = FCommon.getDataTableFromSelect(strSQL_OVC_CHECKER, strParameterName_OVC_CHECKER, aryData_OVC_CHECKER);
                    foreach(DataRow dr_OVC_CHECKER in dt_OVC_CHECKER.Rows)
                        aryOVC_NAME.Add(dr_OVC_CHECKER["OVC_CHECKER"].ToString());
                    DataTable dt_OVC_DO_NAME = FCommon.getDataTableFromSelect(strSQL_OVC_DO_NAME, strParameterName_Other, aryData_Other);
                    foreach (DataRow dr_OVC_DO_NAME in dt_OVC_DO_NAME.Rows)
                        aryOVC_NAME.Add(dr_OVC_DO_NAME["OVC_DO_NAME"].ToString());
                    strOVC_NAME = "";
                    foreach (string strDate in aryOVC_NAME)
                    {
                        if (!strDate.Equals(string.Empty))
                            strOVC_NAME += $"「{ strDate }」";
                    }
                    if (strOVC_NAME.Length > dt_Source.Columns["OVC_NAME"].MaxLength)
                        dt_Source.Columns["OVC_NAME"].MaxLength = strOVC_NAME.Length;
                    dr["OVC_NAME"] = strOVC_NAME;
                }

                DataTable dt = getDataTable_Export(dt_Source, strFieldNames, strFieldSqls, out intCount_Data);

                if (intCount_Data > 0)// && false)
                {
                    string strSheetText = "15";
                    string strTitleText = "國防部所屬單位委託採購中心辦理購案暨委製案件採購作業節點管制表";
                    string strFileName = $"{ strTitleText }.xlsx";

                    MemoryStream Memory = ExcelNPOI.RenderDataTableToStream_Chief(dt, strSheetText, strTitleText, 3, strFormat); //取得Excel資料流
                    FCommon.DownloadFile(this, strFileName, Memory); //直接下載檔案
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查詢無結果，請重新下條件！");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "單位代碼錯誤，請重新登入！");
        }
        //16
        private void Query16()
        {
            if (strDEPT_SN != null)
            {
                ArrayList aryParameterName = new ArrayList();
                ArrayList aryData = new ArrayList();
                string strParameterName_DEPT = ":USER_DEPT_SN";
                aryParameterName.Add(strParameterName_DEPT);
                aryData.Add(strDEPT_SN);

                #region SQL語法
                string strSQL = "";
                #region 欄位
                strSQL += $@"
                    select
                    (a.OVC_PURCH||a.OVC_PUR_AGENCY||e.OVC_PURCH_5) PURCH, --//購案編號
                    a.OVC_PUR_IPURCH, --//購案名稱
                    t_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --a.OVC_PUR_NSECTION, --//申購單位
                    a.OVC_AGNT_IN, --//採購單位
                    t_LAB.OVC_PHR_DESC OVC_LAB, --a.OVC_LAB,--//採購屬性
                    case when (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) < 1000000 then '未達公告金額' else 
                        case when ( a.OVC_LAB = '1' --//勞務
                                    and (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) < 10000000)
                                  or
                                  ( a.OVC_LAB <> '1' --//財物
                                    and (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) < 50000000) then '公告金額以上未達查核金額' else
                            case when ( a.Ovc_Lab = '1' --//勞務
                                        and (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) < 20000000)
                                      or
                                      ( a.Ovc_Lab <> '1' --//財物
                                        and (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) < 100000000) then '查核金額以上未達巨額' else
                                case when ( a.Ovc_Lab = '1' --//勞務
                                            and(nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) >= 20000000)
                                          or
                                          ( a.Ovc_Lab <> '1' --//財物
                                            and(nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) >= 100000000) then '巨額' else '' 
                                end
                            end
                        end
                    end OVC_PUR_LEVEL, --//採購級距
                    t_PLAN_PURCH.OVC_PHR_DESC OVC_PLAN_PURCH, --a.OVC_PLAN_PURCH,--//計畫性質
                    a.OVC_PUR_USER, --//委方承辦人
                    --//額外查詢 評核承辦人
                    --//採購承辦人
                    --//額外查詢 履驗承辦人
                    --//額外查詢 預算科子目
                    --//額外查詢 預算年度
                    t_PUR_CURRENT.OVC_PHR_DESC OVC_PUR_CURRENT, --a.OVC_PUR_CURRENT,--//幣別
                    round(a.ONB_PUR_RATE, 2) ONB_PUR_RATE, --//匯率
                    a.ONB_PUR_BUDGET, --//預算金額(原幣)
                    NVL(a.ONB_PUR_BUDGET_NT,0.0) ONB_PUR_BUDGET_NT, --//預算金額(台幣)
                    --//同上 當年預算金額(台幣)
                    --//額外查詢 核定事項
                    t_PUR_ASS_VEN_CODE.OVC_PHR_DESC OVC_PUR_ASS_VEN_CODE, --NVL(a.OVC_PUR_ASS_VEN_CODE,' ') OVC_PUR_ASS_VEN_CODE,--//招標方式代碼
                    b.OVC_DAPPLY, --//預計申購日
                    a.OVC_DPROPOSE, --//申購日期
                    a.OVC_PUR_DAPPROVE, --//核定日期
                    --//額外查詢 開標日期及開標結果
                    --//決標日期
                    f.OVC_DESC, --//決標重要說明
                    NVL(f.ONB_BID_RESULT, 0), --//決標金額(原幣)
                    0 ONB_BID_RESULT_NT, --//決標金額(台幣)
                    round(case when NVL(a.ONB_PUR_BUDGET_NT, 0)=0 then 0 else ((NVL(f.ONB_BID_RESULT, 0) * NVL(g.ONB_RATE, 0)) / NVL(a.ONB_PUR_BUDGET_NT, 0)) end, 2) BUDGET_RATIO, --//決標預算比％
                    (NVL(a.ONB_PUR_BUDGET, 0) - NVL(f.ONB_BID_RESULT, 0)) Balance, --//標餘款
                    ((NVL(a.ONB_PUR_BUDGET, 0) - NVL(f.ONB_BID_RESULT, 0)) * NVL(a.ONB_PUR_RATE, 0)) Balance_NT, --//標餘款(台幣)
                    --f.OVC_VENDORS_NAME, --
                    decode(g.OVC_VEN_TITLE,null,f.OVC_VENDORS_NAME,g.OVC_VEN_TITLE) OVC_VENDORS_NAME, --//得標商
                    t_PUR_APPROVE_DEP.OVC_PHR_DESC OVC_PUR_APPROVE_DEP, --b.OVC_PUR_APPROVE_DEP,--//採購權責(=核定權責?)
                    --//額外查詢 採購重要事項記載
                    b.OVC_DCANCEL, --//預劃撤案日
                    b.OVC_CANCEL_REASON, --//預劃撤案原因
                    a.OVC_PUR_DCANPO, --//編製撤案日
                    a.OVC_PUR_DCANRE, --//編製撤案原因

                    a.OVC_PURCH,
                    e.OVC_PURCH_5

                    ----以下沒用到----
                    --a.OVC_PURCH||a.OVC_PUR_AGENCY purch,
                    --a.ovc_pur_section,
                    --nvl(a.Onb_Reserve_Amount,0.0) Onb_Reserve_Amount,
                    --g.ONB_RATE,
                    --B.OVC_AUDIT_UNIT,
                    ";
                #endregion
                #region 資料表
                strSQL += $@"
                    from
                    TBM1301 a,
                    TBM1301_PLAN b,
                    VIMAXSTATUS e,
                    VIBID_RESULT f,
                    VITBM1302 g,

                    (SELECT OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PUR_SECTION, --//申購單位
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='E8') t_PUR_APPROVE_DEP, --//核定權責
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='TD') t_PLAN_PURCH, --//計畫性質
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='GN') t_LAB, --//採購屬性
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='C7') t_PUR_ASS_VEN_CODE, --//招標方式
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='B0') t_PUR_CURRENT --//幣別
                    ";
                #endregion
                #region join 條件
                strSQL += $@"
                    where a.OVC_PURCH = b.OVC_PURCH
                    and a.OVC_PURCH = e.OVC_PURCH(+)
                    and e.OVC_PURCH = f.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = f.OVC_PURCH_5(+)
                    and e.OVC_PURCH = g.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = g.OVC_PURCH_5(+)

                    and a.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    and b.OVC_PUR_APPROVE_DEP = t_PUR_APPROVE_DEP.OVC_PHR_ID(+)
                    and a.OVC_PLAN_PURCH = t_PLAN_PURCH.OVC_PHR_ID(+)
                    and a.OVC_LAB = t_LAB.OVC_PHR_ID(+)
                    and a.OVC_PUR_ASS_VEN_CODE = t_PUR_ASS_VEN_CODE.OVC_PHR_ID(+)
                    and a.OVC_PUR_CURRENT = t_PUR_CURRENT.OVC_PHR_ID(+)
                    ";
                #endregion
                #region 篩選條件
                //--+加欄位篩選(有選擇的條件(下方有說明)+
                //年度
                if (chkYear16.Checked)
                {
                    string theParameterName = ":strYear";
                    string strYear = drpYear.SelectedValue;
                    if (strYear.Length > 2)
                        strYear = strYear.Substring(strYear.Length - 2, 2);
                    aryParameterName.Add(theParameterName);
                    aryData.Add(strYear);

                    strSQL += $@"
                        and substr(a.OVC_PURCH,3,2) = { theParameterName }
                    ";
                }
                //採購權責
                if (chkOVC_PUR_APPROVE_DEP.Checked)
                {
                    string theParameterName = ":strOVC_PUR_APPROVE_DEP";
                    string strOVC_PUR_APPROVE_DEP = drpOVC_PUR_APPROVE_DEP.SelectedValue;
                    aryParameterName.Add(theParameterName);
                    aryData.Add(strOVC_PUR_APPROVE_DEP);

                    strSQL += $@"
                        and b.OVC_PUR_APPROVE_DEP = { theParameterName }
                    ";
                }
                //採購屬性
                if (chkOVC_LAB.Checked)
                {
                    string theParameterName = ":strOVC_LAB";
                    string strOVC_LAB = drpOVC_LAB.SelectedValue;
                    aryParameterName.Add(theParameterName);
                    aryData.Add(strOVC_LAB);

                    strSQL += $@"
                        and a.OVC_LAB = { theParameterName }
                    ";
                }
                //採購途徑
                if (chkOVC_PUR_AGENCY.Checked)
                {
                    string theParameterName = ":strOVC_PUR_AGENCY";
                    string strOVC_PUR_AGENCY = drpOVC_PUR_AGENCY.SelectedValue;
                    aryParameterName.Add(theParameterName);
                    aryData.Add(strOVC_PUR_AGENCY);

                    strSQL += $@"
                        and a.OVC_PUR_AGENCY = { theParameterName }
                    ";
                }
                //招標方式
                if (chkOVC_PUR_ASS_VEN_CODE.Checked)
                {
                    string theParameterName = ":strOVC_PUR_ASS_VEN_CODE";
                    string strOVC_PUR_ASS_VEN_CODE = drpOVC_PUR_ASS_VEN_CODE.SelectedValue;
                    aryParameterName.Add(theParameterName);
                    aryData.Add(strOVC_PUR_ASS_VEN_CODE);

                    strSQL += $@"
                        and a.OVC_PUR_ASS_VEN_CODE = { theParameterName }
                    ";
                }
                //計畫性質
                if (chkOVC_PLAN_PURCH.Checked)
                {
                    string theParameterName = ":strOVC_PLAN_PURCH";
                    string strOVC_PLAN_PURCH = drpOVC_PLAN_PURCH.SelectedValue;
                    aryParameterName.Add(theParameterName);
                    aryData.Add(strOVC_PLAN_PURCH);

                    strSQL += $@"
                        and a.OVC_PLAN_PURCH = { theParameterName }
                    ";
                }
                //購案名稱
                if (chkOVC_PUR_IPURCH.Checked)
                {
                    string theParameterName = ":strOVC_PUR_IPURCH";
                    string strOVC_PUR_IPURCH = $"%{ txtOVC_PUR_IPURCH.Text }%";
                    aryParameterName.Add(theParameterName);
                    aryData.Add(strOVC_PUR_IPURCH);

                    strSQL += $@"
                        and a.OVC_PUR_IPURCH like { theParameterName }
                    ";
                }
                //申購日期
                if (chkOVC_DPROPOSE.Checked)
                {
                    string strOVC_DPROPOSE1 = txtOVC_DPROPOSE1.Text;
                    if (!strOVC_DPROPOSE1.Equals(string.Empty))
                        strSQL += $@"
                            and a.OVC_DPROPOSE >= '{ strOVC_DPROPOSE1 }'
                        ";
                    string strOVC_DPROPOSE2 = txtOVC_DPROPOSE2.Text;
                    if (!strOVC_DPROPOSE2.Equals(string.Empty))
                        strSQL += $@"
                            and a.OVC_DPROPOSE <= '{ strOVC_DPROPOSE2 }'
                        ";
                }
                //核定日期
                if (chkOVC_PUR_DAPPROVE.Checked)
                {
                    string strOVC_PUR_DAPPROVE1 = txtOVC_PUR_DAPPROVE1.Text;
                    if (!strOVC_PUR_DAPPROVE1.Equals(string.Empty))
                        strSQL += $@"
                            and a.OVC_PUR_DAPPROVE >= '{ strOVC_PUR_DAPPROVE1 }'
                        ";
                    string strOVC_PUR_DAPPROVE2 = txtOVC_PUR_DAPPROVE2.Text;
                    if (!strOVC_PUR_DAPPROVE2.Equals(string.Empty))
                        strSQL += $@"
                            and a.OVC_PUR_DAPPROVE <= '{ strOVC_PUR_DAPPROVE2 }'
                        ";
                }
                //採購級距
                if (chkOVC_PUR_LEVEL.Checked)
                {
                    //string theParameterName = ":strOVC_PUR_LEVEL";
                    string strOVC_PUR_LEVEL = drpOVC_PUR_LEVEL.SelectedValue;
                    //aryParameterName.Add(theParameterName);
                    //aryData.Add(strOVC_PUR_LEVEL);

                    switch (strOVC_PUR_LEVEL)
                    {
                        case "1": //未達公告金額
                            strSQL += $@"
                                and (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) < 1000000
                            ";
                            break;
                        case "2": //公告金額以上未達查核金額
                            strSQL += $@"
                                and (
                                    (
                                        a.OVC_LAB = '1'
                                        and (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) >= 1000000
                                        and (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) < 10000000
                                    ) --//勞務
                                    or
                                    (
                                        a.OVC_LAB <> '1'
                                        and (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) >= 1000000
                                        and (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) < 50000000
                                    ) --//財物
                                )
                            ";
                            break;
                        case "3": //查核金額以上未達巨額
                            strSQL += $@"
                                and(
                                    (
                                        a.Ovc_Lab = '1'
                                        and (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) >= 10000000
                                        and (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) < 20000000
                                    ) --//勞務
                                    or
                                    (
                                        a.Ovc_Lab <> '1'
                                        and (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) >= 50000000
                                        and (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) < 100000000
                                    ) --//財物
                                ) 
                            ";
                            break;
                        case "4": //巨額
                            strSQL += $@"
                                and(
                                    (
                                        a.Ovc_Lab = '1'
                                        and(nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) >= 20000000
                                    ) --//勞務
                                    or
                                    (
                                        a.Ovc_Lab <> '1'
                                        and(nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) >= 100000000
                                    ) --//財物
                                )
                            ";
                            break;
                    }
                }
                #endregion
                #region 基本條件 & 排序
                if (strDEPT_SN.Equals("0A100"))
                {
                    strSQL += $@"
                        and(b.OVC_AUDIT_UNIT = { strParameterName_DEPT } --//計評單位=登錄者單位
                        or b.OVC_PUR_APPROVE_DEP = 'A') --//讀取採購中心權責購案(部核案)
                        ";
                }
                else
                    strSQL += $@"
                        and b.OVC_AUDIT_UNIT = { strParameterName_DEPT }
                        ";
                strSQL += $@"
                        and (a.OVC_PUR_DCANPO is null or a.OVC_PUR_DCANPO = ' ')
                        and (b.OVC_DCANCEL is null or b.OVC_DCANCEL = ' ')
                        order by a.OVC_PURCH
                    ";
                #endregion
                #region 額外欄位查詢語法
                string[] strParameterName_Other = { ":vOVC_PURCH" };
                ArrayList aryData_Other = new ArrayList();
                aryData_Other.Add("");
                //評核承辦人 OVC_CHECKER
                string[] strParameterName_OVC_CHECKER = { ":vOVC_PURCH", strParameterName_DEPT };
                ArrayList aryData_OVC_CHECKER = new ArrayList();
                aryData_OVC_CHECKER.Add("");
                aryData_OVC_CHECKER.Add(strDEPT_SN);
                string strSQL_OVC_CHECKER = $@"
                    select OVC_CHECKER
                    from TBM1202 a,
                    (select OVC_PURCH, max(ONB_CHECK_TIMES) TIMES
                        from TBM1202 where OVC_PURCH = { strParameterName_OVC_CHECKER[0] } and OVC_CHECK_UNIT = { strParameterName_OVC_CHECKER[1] }
                        group by OVC_PURCH) B
                    where a.OVC_PURCH = B.OVC_PURCH and a.ONB_CHECK_TIMES = B.TIMES and a.OVC_CHECK_UNIT = { strParameterName_OVC_CHECKER[1] }
                ";
                //履驗承辦人 OVC_DO_NAME
                string strSQL_OVC_DO_NAME = $@"
                    select a.OVC_DO_NAME
                    from TBMRECEIVE_CONTRACT a,
                    (select OVC_PURCH, max(OVC_DO_NAME) OVC_DO_NAME from TBMRECEIVE_CONTRACT
                        where OVC_PURCH = { strParameterName_Other[0] }
                        group by OVC_PURCH) B
                    where a.OVC_PURCH = B.OVC_PURCH and a.OVC_DO_NAME = B.OVC_DO_NAME
                ";
                //預算科子目 OVC_POI_IBDG 多筆
                string strSQL_OVC_POI_IBDG = $@"select OVC_POI_IBDG from TBM1118  where OVC_PURCH = { strParameterName_Other[0] } order by OVC_POI_IBDG";
                //預算年度 OVC_YY 多筆
                string strSQL_OVC_YY = $@"select OVC_YY from TBM1118  where OVC_PURCH = { strParameterName_Other[0] } order by OVC_YY";
                //核定事項 OVC_APPROVE_COMMENT
                string[] strParameterName_OVC_APPROVE_COMMENT = { ":vOVC_PURCH", strParameterName_DEPT };
                ArrayList aryData_OVC_APPROVE_COMMENT = new ArrayList();
                aryData_OVC_APPROVE_COMMENT.Add("");
                aryData_OVC_APPROVE_COMMENT.Add(strDEPT_SN);
                string strSQL_OVC_APPROVE_COMMENT = $@"
                    select a.OVC_APPROVE_COMMENT from TBM1202 a,
                        (select OVC_PURCH,max(ONB_CHECK_TIMES) TIMES from TBM1202
                            where OVC_PURCH = { strParameterName_OVC_APPROVE_COMMENT[0] } and OVC_CHECK_UNIT = { strParameterName_OVC_APPROVE_COMMENT[1] } and OVC_CHECK_OK = 'Y' 
                            group by OVC_PURCH) B 
                        where a.OVC_PURCH = B.OVC_PURCH and a.ONB_CHECK_TIMES = B.TIMES
                ";
                //開標日期及開標結果 OVC_DOPEN OVC_RESULT 多筆
                string[] strParameterName_OVC_RESULT = { ":vOVC_PURCH", ":vOVC_PURCH_5" };
                ArrayList aryData_OVC_RESULT = new ArrayList();
                aryData_OVC_RESULT.Add("");
                aryData_OVC_RESULT.Add("");
                string strSQL_OVC_RESULT = $@"
                    select case when a.OVC_RESULT is null then '' else '開標日：'||a.OVC_DOPEN||' 結果：「'||b.OVC_PHR_DESC||'」' end OVC_RESULT
                    from TBM1303 a,
                    (select OVC_PHR_ID, OVC_PHR_DESC from TBM1407 where OVC_PHR_CATE='A8') b
                    where a.OVC_PURCH = { strParameterName_OVC_RESULT[0] } and a.OVC_PURCH_5 = { strParameterName_OVC_RESULT[1] }
                    and a.OVC_RESULT = b.OVC_PHR_ID(+)
                ";
                //採購重要事項記載 OVC_COMM
                string[] strParameterName_OVC_COMM = { ":vOVC_PURCH", ":vOVC_PURCH_5" };
                ArrayList aryData_OVC_COMM = new ArrayList();
                aryData_OVC_COMM.Add("");
                aryData_OVC_COMM.Add("");
                string strSQL_OVC_COMM = $@"
                    select OVC_COMM from TBMRECEIVE_BID_LOG
                    where OVC_PURCH = { strParameterName_OVC_COMM[0] } and OVC_PURCH_5 = { strParameterName_OVC_COMM[1] }
                ";
                #endregion
                string[] strFieldNames = { "購案編號", "購案名稱", "申購單位", "採購單位", "採購屬性", "採購級距", "計畫性質",
                    "委方承辦人", "評核承辦人", "採購承辦人", "履驗承辦人", "預算科子目", "預算年度", "幣別", "匯率",
                    "預算金額(原幣)", "預算金額(台幣)", "當年預算金額(台幣)", "核定事項", "招標方式", "預計申購日", "申購日期", "核定日期",
                    "開標日期及開標結果", "決標日期", "決標重要說明", "決標金額(原幣)", "決標金額(台幣)", "決標預算比％", "標餘款", "標餘款(台幣)",
                    "得標商", "採購權責", "採購重要事項記載", "預劃撤案日", "預劃撤案原因", "編製撤案日", "編製撤案原因" };
                string[] strFieldSqls = { "PURCH", "OVC_PUR_IPURCH", "OVC_PUR_SECTION", "OVC_AGNT_IN", "OVC_LAB", "OVC_PUR_LEVEL", "OVC_PLAN_PURCH",
                    "OVC_PUR_USER", "OVC_CHECKER", "", "OVC_DO_NAME", "OVC_POI_IBDG", "OVC_YY", "OVC_PUR_CURRENT", "ONB_PUR_RATE",
                    "ONB_PUR_BUDGET", "ONB_PUR_BUDGET_NT", "ONB_PUR_BUDGET_NT", "OVC_APPROVE_COMMENT", "OVC_PUR_ASS_VEN_CODE", "OVC_DAPPLY", "OVC_DPROPOSE", "OVC_PUR_DAPPROVE",
                    "OVC_RESULT", "", "OVC_DESC", "ONB_BID_RESULT", "ONB_BID_RESULT_NT", "BUDGET_RATIO", "Balance", "Balance_NT",
                    "OVC_VENDORS_NAME", "OVC_PUR_APPROVE_DEP", "OVC_COMM", "OVC_DCANCEL", "OVC_CANCEL_REASON", "OVC_PUR_DCANPO", "OVC_PUR_DCANRE" };
                string[] strFormat = { null, null, null, null, null, null, null,
                    null, null, null, null, null, null, null, null,
                    strFormatMoney2, strFormatMoney2, strFormatMoney2, null, null, null, null, null,
                    null, null, null, strFormatMoney2, strFormatMoney2, null, strFormatMoney2, strFormatMoney2,
                    null, null, null, null, null, null, null };
                #endregion

                //FCommon.AlertShow(PnMessage, "danger", "系統訊息", strSQL);
                if (true)
                {
                    DataTable dt_Source = FCommon.getDataTableFromSelect(strSQL, aryParameterName, aryData);
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
                        string strOVC_PURCH_5 = dr["OVC_PURCH_5"].ToString();
                        aryData_Other[0] = strOVC_PURCH;
                        aryData_OVC_CHECKER[0] = strOVC_PURCH;
                        aryData_OVC_APPROVE_COMMENT[0] = strOVC_PURCH;
                        aryData_OVC_RESULT[0] = strOVC_PURCH;
                        aryData_OVC_RESULT[1] = strOVC_PURCH_5; //vOVC_PURCH_5
                        aryData_OVC_COMM[0] = strOVC_PURCH;
                        aryData_OVC_COMM[1] = strOVC_PURCH_5; //vOVC_PURCH_5
                        
                        //評核承辦人 OVC_CHECKER
                        DataTable dt_OVC_CHECKER = FCommon.getDataTableFromSelect(strSQL_OVC_CHECKER, strParameterName_OVC_CHECKER, aryData_OVC_CHECKER);
                        if (dt_OVC_CHECKER.Rows.Count > 0)
                            dr["OVC_CHECKER"] = dt_OVC_CHECKER.Rows[0]["OVC_CHECKER"];
                        //履驗承辦人 OVC_DO_NAME
                        DataTable dt_OVC_DO_NAME = FCommon.getDataTableFromSelect(strSQL_OVC_DO_NAME, strParameterName_Other, aryData_Other);
                        if (dt_OVC_DO_NAME.Rows.Count > 0)
                            dr["OVC_DO_NAME"] = dt_OVC_DO_NAME.Rows[0]["OVC_DO_NAME"];
                        //預算科子目 OVC_POI_IBDG 多筆
                        DataTable dt_OVC_POI_IBDG = FCommon.getDataTableFromSelect(strSQL_OVC_POI_IBDG, strParameterName_Other, aryData_Other);
                        setFieldList(dr, dt_OVC_POI_IBDG, "OVC_POI_IBDG");//多筆
                                                                          //年度 OVC_YY 多筆
                        DataTable dt_OVC_YY = FCommon.getDataTableFromSelect(strSQL_OVC_YY, strParameterName_Other, aryData_Other);
                        setFieldList(dr, dt_OVC_YY, "OVC_YY");//多筆
                                                              //核定事項 OVC_APPROVE_COMMENT
                        DataTable dt_OVC_APPROVE_COMMENT = FCommon.getDataTableFromSelect(strSQL_OVC_APPROVE_COMMENT, strParameterName_OVC_APPROVE_COMMENT, aryData_OVC_APPROVE_COMMENT);
                        if (dt_OVC_APPROVE_COMMENT.Rows.Count > 0)
                            dr["OVC_APPROVE_COMMENT"] = dt_OVC_APPROVE_COMMENT.Rows[0]["OVC_APPROVE_COMMENT"];
                        //開標日期及開標結果 OVC_DOPEN OVC_RESULT
                        DataTable dt_OVC_RESULT = FCommon.getDataTableFromSelect(strSQL_OVC_RESULT, strParameterName_OVC_RESULT, aryData_OVC_RESULT);
                        string strOVC_RESULT = "";
                        foreach (DataRow dr_OVC_RESULT in dt_OVC_RESULT.Rows)
                        {
                            if (!strOVC_RESULT.Equals(string.Empty)) strOVC_RESULT += ";";
                             strOVC_RESULT += dr_OVC_RESULT["OVC_RESULT"].ToString();
                        }
                        dr["OVC_RESULT"] = strOVC_RESULT;
                        //採購重要事項記載 OVC_COMM
                        DataTable dt_OVC_COMM = FCommon.getDataTableFromSelect(strSQL_OVC_COMM, strParameterName_OVC_COMM, aryData_OVC_COMM);
                        if (dt_OVC_COMM.Rows.Count > 0)
                            dr["OVC_COMM"] = dt_OVC_COMM.Rows[0]["OVC_COMM"];
                    }

                    DataTable dt = getDataTable_Export(dt_Source, strFieldNames, strFieldSqls, out intCount_Data);

                    if (intCount_Data > 0)// && false)
                    {
                        string strSheetText = "16";
                        string strTitleText = "計畫評核查詢資料明細表";
                        string strFileName = $"{ strTitleText }.xlsx";

                        MemoryStream Memory = ExcelNPOI.RenderDataTableToStream_Chief(dt, strSheetText, strTitleText, 3, strFormat); //取得Excel資料流
                        FCommon.DownloadFile(this, strFileName, Memory); //直接下載檔案
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查詢無結果，請重新下條件！");
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "單位代碼錯誤，請重新登入！");
        }
        //17
        private void Query17()
        {
            if (strDEPT_SN != null)
            {
                ArrayList aryParameterName = new ArrayList();
                ArrayList aryData = new ArrayList();
                string strParameterName_DEPT = ":USER_DEPT_SN";
                aryParameterName.Add(strParameterName_DEPT);
                aryData.Add(strDEPT_SN);

                #region SQL語法
                string strSQL = "";
                #region 欄位
                strSQL += $@"
                    select
                    (a.OVC_PURCH||a.OVC_PUR_AGENCY||FGETCROSSYEARKEYWORDQRY(a.OVC_PURCH,'OVC_PURCH_5')) PURCH, --//購案編號
                    a.OVC_PUR_IPURCH, --//購案名稱
                    t_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --a.OVC_PUR_NSECTION, --//申購單位
                    a.OVC_AGNT_IN, --//採購單位
                    t_LAB.OVC_PHR_DESC OVC_LAB, --a.OVC_LAB,--//採購屬性
                    case when (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) < 1000000 then '未達公告金額' else 
                        case when ( a.OVC_LAB = '1' --//勞務
                                    and (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) < 10000000)
                                  or
                                  ( a.OVC_LAB <> '1' --//財物
                                    and (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) < 50000000) then '公告金額以上未達查核金額' else
                            case when ( a.Ovc_Lab = '1' --//勞務
                                        and (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) < 20000000)
                                      or
                                      ( a.Ovc_Lab <> '1' --//財物
                                        and (nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) < 100000000) then '查核金額以上未達巨額' else
                                case when ( a.Ovc_Lab = '1' --//勞務
                                            and(nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) >= 20000000)
                                          or
                                          ( a.Ovc_Lab <> '1' --//財物
                                            and(nvl(a.ONB_PUR_BUDGET_NT, 0) + nvl(a.ONB_RESERVE_AMOUNT, 0)) >= 100000000) then '巨額' else '' 
                                end
                            end
                        end
                    end OVC_PUR_LEVEL, --//採購級距
                    t_PLAN_PURCH.OVC_PHR_DESC OVC_PLAN_PURCH, --a.OVC_PLAN_PURCH,--//計畫性質
                    a.OVC_PUR_USER, --//委方承辦人
                    --//額外查詢 評核承辦人
                    --//採購承辦人
                    --//額外查詢 履驗承辦人
                    --//額外查詢 預算科子目
                    --//額外查詢 預算年度
                    t_PUR_CURRENT.OVC_PHR_DESC OVC_PUR_CURRENT, --a.OVC_PUR_CURRENT,--//幣別
                    round(a.ONB_PUR_RATE, 2) ONB_PUR_RATE, --//匯率
                    a.ONB_PUR_BUDGET, --//預算金額(原幣)
                    NVL(a.ONB_PUR_BUDGET_NT,0.0) ONB_PUR_BUDGET_NT, --//預算金額(台幣)
                    --//同上 當年預算金額(台幣)
                    --//額外查詢 核定事項
                    t_PUR_ASS_VEN_CODE.OVC_PHR_DESC OVC_PUR_ASS_VEN_CODE, --NVL(a.OVC_PUR_ASS_VEN_CODE,' ') OVC_PUR_ASS_VEN_CODE,--//招標方式代碼
                    b.OVC_DAPPLY, --//預計申購日
                    a.OVC_DPROPOSE, --//申購日期
                    a.OVC_PUR_DAPPROVE, --//核定日期
                    --//額外查詢 開標日期及開標結果
                    --//決標日期
                    --f.OVC_DESC, --//額外查詢 決標重要說明
                    NVL(FGETCROSSYEARKEYWORDQRY(a.OVC_PURCH, 'ONB_BID_RESULT'), 0) ONB_BID_RESULT, --//決標金額(原幣)
                    0 ONB_BID_RESULT_NT, --//決標金額(台幣)
                    round(case when NVL(a.ONB_PUR_BUDGET_NT, 0)=0 then 0 else ((NVL(FGETCROSSYEARKEYWORDQRY(a.OVC_PURCH, 'ONB_BID_RESULT'), 0) * NVL(FGETCROSSYEARKEYWORDQRY(a.OVC_PURCH, 'ONB_RATE'), 0)) / NVL(a.ONB_PUR_BUDGET_NT, 0)) end, 2) BUDGET_RATIO, --//決標預算比％
                    (NVL(a.ONB_PUR_BUDGET, 0) - NVL(FGETCROSSYEARKEYWORDQRY(a.OVC_PURCH, 'ONB_BID_RESULT'), 0)) Balance, --//標餘款
                    ((NVL(a.ONB_PUR_BUDGET, 0) - NVL(FGETCROSSYEARKEYWORDQRY(a.OVC_PURCH, 'ONB_BID_RESULT'), 0)) * NVL(a.ONB_PUR_RATE, 0)) Balance_NT, --//標餘款(台幣)
                    --FGETCROSSYEARKEYWORDQRY(a.OVC_PURCH,'OVC_VENDORS_NAME') OVC_VENDORS_NAME, --
                    decode(FGETCROSSYEARKEYWORDQRY(a.OVC_PURCH,'OVC_VEN_TITLE'), null, FGETCROSSYEARKEYWORDQRY(a.OVC_PURCH,'OVC_VENDORS_NAME'), FGETCROSSYEARKEYWORDQRY(a.OVC_PURCH,'OVC_VEN_TITLE')) OVC_VENDORS_NAME, --//得標商
                    t_PUR_APPROVE_DEP.OVC_PHR_DESC OVC_PUR_APPROVE_DEP, --b.OVC_PUR_APPROVE_DEP,--//採購權責(=核定權責?)
                    --//額外查詢 採購重要事項記載
                    b.OVC_DCANCEL, --//預劃撤案日
                    b.OVC_CANCEL_REASON, --//預劃撤案原因
                    a.OVC_PUR_DCANPO, --//編製撤案日
                    a.OVC_PUR_DCANRE, --//編製撤案原因

                    a.OVC_PURCH,
                    FGETCROSSYEARKEYWORDQRY(a.OVC_PURCH,'OVC_PURCH_5') OVC_PURCH_5

                    ----以下沒用到----
                    --NVL(A.ONB_RESERVE_AMOUNT,0.0) ONB_RESERVE_AMOUNT,
                    --FGETCROSSYEARKEYWORDQRY(a.OVC_PURCH, 'ONB_RATE') ONB_RATE,
                    --B.OVC_AUDIT_UNIT
                    ";
                #endregion
                #region 資料表
                string strOVC_PUR_TYPE = txtOVC_PUR_TYPE.Text;
                bool isOVC_PUR_TYPE = chkOVC_PUR_TYPE.Checked && !strOVC_PUR_TYPE.Equals(string.Empty);
                strSQL += $@"
                    from
                    TBM1301 a,
                    TBM1301_PLAN b,

                    (SELECT OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PUR_SECTION, --//申購單位
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='E8') t_PUR_APPROVE_DEP, --//核定權責
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='TD') t_PLAN_PURCH, --//計畫性質
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='GN') t_LAB, --//採購屬性
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='C7') t_PUR_ASS_VEN_CODE, --//招標方式
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='B0') t_PUR_CURRENT --//幣別
                    ";
                //採購品項
                if (isOVC_PUR_TYPE)
                {
                    string theParameterName = ":strOVC_PUR_TYPE";
                    strOVC_PUR_TYPE = $"%{ strOVC_PUR_TYPE }%";
                    aryParameterName.Add(theParameterName);
                    aryData.Add(strOVC_PUR_TYPE);
                    strSQL += $@"
                        ,(SELECT OVC_PURCH FROM TBM1201 WHERE OVC_POI_NSTUFF_CHN like { theParameterName } group by OVC_PURCH) h
                    ";
                }
                #endregion
                #region join 條件
                strSQL += $@"
                    where a.OVC_PURCH = b.OVC_PURCH

                    and a.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    and b.OVC_PUR_APPROVE_DEP = t_PUR_APPROVE_DEP.OVC_PHR_ID(+)
                    and a.OVC_PLAN_PURCH = t_PLAN_PURCH.OVC_PHR_ID(+)
                    and a.OVC_LAB = t_LAB.OVC_PHR_ID(+)
                    and a.OVC_PUR_ASS_VEN_CODE = t_PUR_ASS_VEN_CODE.OVC_PHR_ID(+)
                    and a.OVC_PUR_CURRENT = t_PUR_CURRENT.OVC_PHR_ID(+)
                    ";
                //採購品項
                if (isOVC_PUR_TYPE)
                    strSQL += @"
                        and a.OVC_PURCH = h.OVC_PURCH
                    ";
                #endregion
                #region 篩選條件
                //--+加欄位篩選(有選擇的條件(下方有說明)+
                //年度
                if (chkYear17.Checked)
                {
                    string theParameterName = ":strYear";
                    string strYear1 = drpYear1.SelectedValue;
                    string strYear2 = drpYear2.SelectedValue;
                    if (strYear1 .Equals(strYear2))
                    {
                        if (strYear1.Length > 2)
                            strYear1 = strYear1.Substring(strYear1.Length - 2, 2);
                        aryParameterName.Add(theParameterName);
                        aryData.Add(strYear1);

                        strSQL += $@"
                            and substr(a.OVC_PURCH,3,2) = { theParameterName }
                        ";
                    }
                    else
                    {
                        int intYear1 = int.Parse(strYear1);
                        int intYear2 = int.Parse(strYear2);
                        string strYearList = "";
                        //若年度1較大，則與年度2做交換動作，確保年度1<年度2
                        if (intYear1 > intYear2)
                        {
                            int intTemp = intYear1; intYear1 = intYear2; intYear2 = intTemp;
                        }
                        int theYear = intYear1;
                        do
                        {
                            if (theYear != intYear1) strYearList = $"{ strYearList }, ";
                            string strYear = theYear++.ToString();
                            if (strYear.Length > 2)
                                strYear = strYear.Substring(strYear.Length - 2, 2);
                            strYearList = $"{ strYearList }'{ strYear }'";
                        } while (theYear <= intYear2);

                        strSQL += $@"
                            and substr(a.OVC_PURCH,3,2) in ({ strYearList })
                        ";
                    }
                }
                //購案名稱
                if (chk_OVC_PUR_IPURCH17.Checked)
                {
                    string theParameterName = ":strOVC_PUR_IPURCH";
                    string strOVC_PUR_IPURCH = $"%{ txtOVC_PUR_IPURCH17.Text }%";
                    aryParameterName.Add(theParameterName);
                    aryData.Add(strOVC_PUR_IPURCH);

                    strSQL += $@"
                        and a.OVC_PUR_IPURCH like { theParameterName }
                    ";
                }
                //採購品項－於join時做篩選
                #endregion
                #region 基本條件 & 排序
                if (strDEPT_SN.Equals("0A100"))
                {
                    strSQL += $@"
                        and(b.OVC_AUDIT_UNIT = { strParameterName_DEPT } --//計評單位=登錄者單位
                        or b.OVC_PUR_APPROVE_DEP = 'A') --//讀取採購中心權責購案(部核案)
                        ";
                }
                else
                    strSQL += $@"
                        and b.OVC_AUDIT_UNIT = { strParameterName_DEPT }
                        ";
                strSQL += $@"
                        and (a.OVC_PUR_DCANPO is null or a.OVC_PUR_DCANPO = ' ')
                        and (b.OVC_DCANCEL is null or b.OVC_DCANCEL = ' ')
                        order by a.OVC_PURCH
                    ";
                #endregion
                #region 額外欄位查詢語法
                string[] strParameterName_Other = { ":vOVC_PURCH" };
                ArrayList aryData_Other = new ArrayList();
                aryData_Other.Add("");
                //評核承辦人 OVC_CHECKER
                string[] strParameterName_OVC_CHECKER = { ":vOVC_PURCH", strParameterName_DEPT };
                ArrayList aryData_OVC_CHECKER = new ArrayList();
                aryData_OVC_CHECKER.Add("");
                aryData_OVC_CHECKER.Add(strDEPT_SN);
                string strSQL_OVC_CHECKER = $@"
                    select OVC_CHECKER
                    from TBM1202 a,
                    (select OVC_PURCH, max(ONB_CHECK_TIMES) TIMES
                        from TBM1202 where OVC_PURCH = { strParameterName_OVC_CHECKER[0] } and OVC_CHECK_UNIT = { strParameterName_OVC_CHECKER[1] }
                        group by OVC_PURCH) B
                    where a.OVC_PURCH = B.OVC_PURCH and a.ONB_CHECK_TIMES = B.TIMES and a.OVC_CHECK_UNIT = { strParameterName_OVC_CHECKER[1] }
                ";
                //履驗承辦人 OVC_DO_NAME
                string strSQL_OVC_DO_NAME = $@"
                    select a.OVC_DO_NAME
                    from TBMRECEIVE_CONTRACT a,
                    (select OVC_PURCH, max(OVC_DO_NAME) OVC_DO_NAME from TBMRECEIVE_CONTRACT
                        where OVC_PURCH = { strParameterName_Other[0] }
                        group by OVC_PURCH) B
                    where a.OVC_PURCH = B.OVC_PURCH and a.OVC_DO_NAME = B.OVC_DO_NAME
                ";
                //預算科子目 OVC_POI_IBDG 多筆
                string strSQL_OVC_POI_IBDG = $@"select OVC_POI_IBDG from TBM1118  where OVC_PURCH = { strParameterName_Other[0] } order by OVC_POI_IBDG";
                //預算年度 OVC_YY 多筆
                string strSQL_OVC_YY = $@"select OVC_YY from TBM1118  where OVC_PURCH = { strParameterName_Other[0] } order by OVC_YY";
                //核定事項 OVC_APPROVE_COMMENT
                string[] strParameterName_OVC_APPROVE_COMMENT = { ":vOVC_PURCH", strParameterName_DEPT };
                ArrayList aryData_OVC_APPROVE_COMMENT = new ArrayList();
                aryData_OVC_APPROVE_COMMENT.Add("");
                aryData_OVC_APPROVE_COMMENT.Add(strDEPT_SN);
                string strSQL_OVC_APPROVE_COMMENT = $@"
                    select a.OVC_APPROVE_COMMENT from TBM1202 a,
                        (select OVC_PURCH,max(ONB_CHECK_TIMES) TIMES from TBM1202
                            where OVC_PURCH = { strParameterName_OVC_APPROVE_COMMENT[0] } and OVC_CHECK_UNIT = { strParameterName_OVC_APPROVE_COMMENT[1] } and OVC_CHECK_OK = 'Y' 
                            group by OVC_PURCH) B 
                        where a.OVC_PURCH = B.OVC_PURCH and a.ONB_CHECK_TIMES = B.TIMES
                ";
                //開標日期及開標結果 OVC_DOPEN OVC_RESULT
                string[] strParameterName_OVC_RESULT = { ":vOVC_PURCH", ":vOVC_PURCH_5" };
                ArrayList aryData_OVC_RESULT = new ArrayList();
                aryData_OVC_RESULT.Add("");
                aryData_OVC_RESULT.Add("");
                string strSQL_OVC_RESULT = $@"
                    select case when a.OVC_RESULT is null then '' else '開標日：'||a.OVC_DOPEN||' 結果：「'||b.OVC_PHR_DESC||'」' end OVC_RESULT
                    from TBM1303 a,
                    (select OVC_PHR_ID, OVC_PHR_DESC from TBM1407 where OVC_PHR_CATE='A8') b
                    where a.OVC_PURCH = { strParameterName_OVC_RESULT[0] } and a.OVC_PURCH_5 = { strParameterName_OVC_RESULT[1] }
                    and a.OVC_RESULT = b.OVC_PHR_ID(+)
                ";
                //決標重要說明 OVC_DESC
                string strSQL_OVC_DESC = $@"select OVC_DESC from VIBID_RESULT where OVC_PURCH = { strParameterName_Other[0] }";
                //採購重要事項記載 OVC_COMM
                string[] strParameterName_OVC_COMM = { ":vOVC_PURCH", ":vOVC_PURCH_5" };
                ArrayList aryData_OVC_COMM = new ArrayList();
                aryData_OVC_COMM.Add("");
                aryData_OVC_COMM.Add("");
                string strSQL_OVC_COMM = $@"
                    select OVC_COMM from TBMRECEIVE_BID_LOG
                    where OVC_PURCH = { strParameterName_OVC_COMM[0] } and OVC_PURCH_5 = { strParameterName_OVC_COMM[1] }
                ";
                #endregion
                string[] strFieldNames = { "購案編號", "購案名稱", "申購單位", "採購單位", "採購屬性", "採購級距", "計畫性質",
                    "委方承辦人", "評核承辦人", "採購承辦人", "履驗承辦人", "預算科子目", "預算年度", "幣別", "匯率",
                    "預算金額(原幣)", "預算金額(台幣)", "當年預算金額(台幣)", "核定事項", "招標方式", "預計申購日", "申購日期", "核定日期",
                    "開標日期及開標結果", "決標日期", "決標重要說明", "決標金額(原幣)", "決標金額(台幣)", "決標預算比％", "標餘款", "標餘款(台幣)",
                    "得標商", "採購權責", "採購重要事項記載", "預劃撤案日", "預劃撤案原因", "編製撤案日", "編製撤案原因" };
                string[] strFieldSqls = { "PURCH", "OVC_PUR_IPURCH", "OVC_PUR_SECTION", "OVC_AGNT_IN", "OVC_LAB", "OVC_PUR_LEVEL", "OVC_PLAN_PURCH",
                    "OVC_PUR_USER", "OVC_CHECKER", "", "OVC_DO_NAME", "OVC_POI_IBDG", "OVC_YY", "OVC_PUR_CURRENT", "ONB_PUR_RATE",
                    "ONB_PUR_BUDGET", "ONB_PUR_BUDGET_NT", "ONB_PUR_BUDGET_NT", "OVC_APPROVE_COMMENT", "OVC_PUR_ASS_VEN_CODE", "OVC_DAPPLY", "OVC_DPROPOSE", "OVC_PUR_DAPPROVE",
                    "OVC_RESULT", "", "OVC_DESC", "ONB_BID_RESULT", "ONB_BID_RESULT_NT", "BUDGET_RATIO", "Balance", "Balance_NT",
                    "OVC_VENDORS_NAME", "OVC_PUR_APPROVE_DEP", "OVC_COMM", "OVC_DCANCEL", "OVC_CANCEL_REASON", "OVC_PUR_DCANPO", "OVC_PUR_DCANRE" };
                string[] strFormat = { null, null, null, null, null, null, null,
                    null, null, null, null, null, null, null, null,
                    strFormatMoney2, strFormatMoney2, strFormatMoney2, null, null, null, null, null,
                    null, null, null, strFormatMoney2, strFormatMoney2, null, strFormatMoney2, strFormatMoney2,
                    null, null, null, null, null, null, null };
                #endregion

                //FCommon.AlertShow(PnMessage, "danger", "系統訊息", strSQL);
                DataTable dt_Source = FCommon.getDataTableFromSelect(strSQL, aryParameterName, aryData);
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
                    string strOVC_PURCH_5 = dr["OVC_PURCH_5"].ToString();
                    aryData_Other[0] = strOVC_PURCH;
                    aryData_OVC_CHECKER[0] = strOVC_PURCH;
                    aryData_OVC_APPROVE_COMMENT[0] = strOVC_PURCH;
                    aryData_OVC_RESULT[0] = strOVC_PURCH;
                    aryData_OVC_RESULT[1] = strOVC_PURCH_5; //vOVC_PURCH_5
                    aryData_OVC_COMM[0] = strOVC_PURCH;
                    aryData_OVC_COMM[1] = strOVC_PURCH_5; //vOVC_PURCH_5
                    
                    //評核承辦人 OVC_CHECKER
                    DataTable dt_OVC_CHECKER = FCommon.getDataTableFromSelect(strSQL_OVC_CHECKER, strParameterName_OVC_CHECKER, aryData_OVC_CHECKER);
                    if (dt_OVC_CHECKER.Rows.Count > 0)
                        dr["OVC_CHECKER"] = dt_OVC_CHECKER.Rows[0]["OVC_CHECKER"];
                    //履驗承辦人 OVC_DO_NAME
                    DataTable dt_OVC_DO_NAME = FCommon.getDataTableFromSelect(strSQL_OVC_DO_NAME, strParameterName_Other, aryData_Other);
                    if (dt_OVC_DO_NAME.Rows.Count > 0)
                        dr["OVC_DO_NAME"] = dt_OVC_DO_NAME.Rows[0]["OVC_DO_NAME"];
                    //預算科子目 OVC_POI_IBDG 多筆
                    DataTable dt_OVC_POI_IBDG = FCommon.getDataTableFromSelect(strSQL_OVC_POI_IBDG, strParameterName_Other, aryData_Other);
                    setFieldList(dr, dt_OVC_POI_IBDG, "OVC_POI_IBDG");//多筆
                    //年度 OVC_YY 多筆
                    DataTable dt_OVC_YY = FCommon.getDataTableFromSelect(strSQL_OVC_YY, strParameterName_Other, aryData_Other);
                    setFieldList(dr, dt_OVC_YY, "OVC_YY");//多筆
                    //核定事項 OVC_APPROVE_COMMENT
                    DataTable dt_OVC_APPROVE_COMMENT = FCommon.getDataTableFromSelect(strSQL_OVC_APPROVE_COMMENT, strParameterName_OVC_APPROVE_COMMENT, aryData_OVC_APPROVE_COMMENT);
                    if (dt_OVC_APPROVE_COMMENT.Rows.Count > 0)
                        dr["OVC_APPROVE_COMMENT"] = dt_OVC_APPROVE_COMMENT.Rows[0]["OVC_APPROVE_COMMENT"];
                    //開標日期及開標結果 OVC_DOPEN OVC_RESULT
                    DataTable dt_OVC_RESULT = FCommon.getDataTableFromSelect(strSQL_OVC_RESULT, strParameterName_OVC_RESULT, aryData_OVC_RESULT);
                    if (dt_OVC_RESULT.Rows.Count > 0)
                        dr["OVC_RESULT"] = dt_OVC_RESULT.Rows[0]["OVC_RESULT"];
                    //決標重要說明 OVC_DESC
                    DataTable dt_OVC_DESC = FCommon.getDataTableFromSelect(strSQL_OVC_DESC, strParameterName_Other, aryData_Other);
                    if (dt_OVC_DESC.Rows.Count > 0)
                        dr["OVC_DESC"] = dt_OVC_DESC.Rows[0]["OVC_DESC"];
                    //採購重要事項記載 OVC_COMM
                    DataTable dt_OVC_COMM = FCommon.getDataTableFromSelect(strSQL_OVC_COMM, strParameterName_OVC_COMM, aryData_OVC_COMM);
                    if (dt_OVC_COMM.Rows.Count > 0)
                        dr["OVC_COMM"] = dt_OVC_COMM.Rows[0]["OVC_COMM"];
                }
                DataTable dt = getDataTable_Export(dt_Source, strFieldNames, strFieldSqls, out intCount_Data);

                if (intCount_Data > 0)// && false)
                {
                    string strSheetText = "17";
                    string strTitleText = "計畫評核查詢資料明細表";
                    string strFileName = $"{ strTitleText }.xlsx";

                    MemoryStream Memory = ExcelNPOI.RenderDataTableToStream_Chief(dt, strSheetText, strTitleText, 3, strFormat); //取得Excel資料流
                    FCommon.DownloadFile(this, strFileName, Memory); //直接下載檔案
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查詢無結果，請重新下條件！");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "單位代碼錯誤，請重新登入！");
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getAccountDEPTName(this, out strDEPT_SN, out strDEPT_Name);

                if (!IsPostBack)
                {
                    //設置readonly屬性
                    FCommon.Controls_Attributes("readonly", "true", txt_PurSum1, txt_PurSum2, txt_OpenRead1, txt_OpenRead2, txt_UndonePurSta1, txt_UndonePurSta2, 
                        txt_MostFavS1, txt_MostFavS2, txt_ApprovedPur1, txt_ApprovedPur2, txt_CasePur1, txt_CasePur2, txt_UnitPur1, txt_UnitPur2, 
                        txtOVC_DPROPOSE1, txtOVC_DPROPOSE2, txtOVC_PUR_DAPPROVE1, txtOVC_PUR_DAPPROVE2);
                    list_dataImport();

                    //若是從 FpersonalPurchaselist 回來
                    string strOVC_PURCH;
                    if (FCommon.getQueryString(this, "PurNum", out strOVC_PURCH, true))
                        txtOVC_PURCH.Text = strOVC_PURCH;
                }
            }
        }
        
        #region OnClick
        //1.年度購案管制表
        protected void btnQuery_AnnualPur_Click(object sender, EventArgs e)
        {
            Response.Redirect("FAnnualPurchaseCT");
        }
        //2.購案執行進度明細表
        protected void btnQuery_PurImple_Click(object sender, EventArgs e)
        {
            Response.Redirect("FPurchaseImplementationST");
        }
        //3.各單位購案統計表
        protected void btnQuery_AllUnitPur_Click(object sender, EventArgs e)
        {
            Response.Redirect("FUnitPurchaseST");
        }
        //4.駐美、歐組購案履驗階段執行現況明細表
        protected void btnQuery_EUSImple_Click(object sender, EventArgs e)
        {
            Response.Redirect("FEUSImpIementationST");
        }
        //5.澄覆比較表
        protected void btnQuery_ClosedCompar_Click(object sender, EventArgs e)
        {
            Response.Redirect("FClosedComparisonT");
        }
        //6.依照購案編號查詢
        protected void btnQuery_PurchaseNum_Click(object sender, EventArgs e)
        {
            string strOVC_PURCH = txtOVC_PURCH.Text, strQueryString = "";
            if (!string.IsNullOrEmpty(strOVC_PURCH))
            {
                FCommon.setQueryString(ref strQueryString, "PurNum", strOVC_PURCH, true);
                Response.Redirect($"FpersonalPurchaselist{ strQueryString }");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請輸入購案編號！");
        }
        //7.個人承辦案資料明細表
        protected void btnQuery_PerPur_Click(object sender, EventArgs e)
        {
            string strYear = drpYear_PerPur.SelectedValue, strQueryString = "";
            if (!string.IsNullOrEmpty(strYear))
            {
                FCommon.setQueryString(ref strQueryString, "year", strYear, true);
                Response.Redirect($"FpersonalPurchaseST{ strQueryString }");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請輸入購案編號！");
        }
        //8.部核定購案統計總表
        protected void btnQuery_PurSum_Click(object sender, EventArgs e)
        {
            Query_PurSum();
        }
        //9.辦理公開閱覽購案統計表
        protected void btnQuery_OpenRead_Click(object sender, EventArgs e)
        {
            Query_OpenRead();
        }
        //10.尚未完成資訊審查作業購案統計表
        protected void btnQuery_UndonePurSta_Click(object sender, EventArgs e)
        {
            Query_UndonePurSta();
        }
        //11.採用最有利標購案統計表
        protected void btnQuery_MostFavS_Click(object sender, EventArgs e)
        {
            Query_MostFavS();
        }
        //12.已核定購案統計表
        protected void btnQuery_ApprovedPur_Click(object sender, EventArgs e)
        {
            Query_ApprovedPur();
        }
        //13.辦理撤案購案統計表
        protected void btnQuery_CasePur_Click(object sender, EventArgs e)
        {
            Query_CasePur();
        }
        //14.購案統計表
        protected void btnQuery_UnitPur_Click(object sender, EventArgs e)
        {
            Query_UnitPur();
        }
                //15.國防部所屬單位委託採購中心辦理購案暨委製案件採購作業節點管制表
        protected void btnQuery_MNDPur_Click(object sender, EventArgs e)
        {
            Query_MNDPur();
        }
        //16
        protected void btnQuery16_Click(object sender, EventArgs e)
        {
            Query16();
        }
        //17
        protected void btnQuery17_Click(object sender, EventArgs e)
        {
            Query17();
        }
        #endregion
    }
}