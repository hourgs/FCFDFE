using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MPMS.F
{
    public partial class FAnnualPurchaseCT : Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        public string strDEPT_SN, strDEPT_Name;

        string[] strFieldNames = { "購案編號", "購案名稱", "申購單位", "核定權責", "預劃申購日期", "申購單位實際申請日期", "計評實際收辦日期", "預劃核定日期", "主官核批日", "核定(發文)日期",
            "核定文號", "計評承辦人", "計畫性質", "採購途徑", "採購屬性", "招標方式", "是否適用GPA", "政府採購協定片語", "採購發包單位", "採包承辦人", "履約驗結單位", "履驗承辦人", "需公開閱覽", "準用最有利標", "預算(台幣)",
            "是否複數決標", "是否開放式契", "是否並列得標廠商", "開標日期及開標結果", "組別(決標組數)", "核定分組數", "決標日期", "決標金額(台幣【含單價、折扣或其他】)", "決標方式片語", "合約金額(台幣)",
            "得標廠商", "交貨批次", "簽約日期", "合約交貨日期", "實際交貨日期", "驗結日期", "結案日期", "是否撤案", "計評總天數", "紙本收文日", "分派日", "主官批核日", "核定發文日" };
        string[] strFieldSqls = { "PURCH", "OVC_PUR_IPURCH", "OVC_PUR_SECTION", "OVC_PUR_APPROVE_DEP", "OVC_DAPPLY", "OVC_DPROPOSE", "OVC_DRECEIVE", "OVC_PLAN_PUR_DAPPROVE", "OVC_PUR_ALLOW", "OVC_PUR_DAPPROVE",
            "OVC_PUR_APPROVE", "OVC_CHECKER", "OVC_PLAN_PURCH", "OVC_PUR_AGENCY", "OVC_LAB", "OVC_PUR_ASS_VEN_CODE", "GPA", "GPA_TYPE", "OVC_PURCHASE_UNIT", "OVC_NAME", "OVC_CONTRACT_UNIT", "OVC_DO_NAME", "OVC_OPEN_CHECK", "OVC_ADVENTAGED_CHECK", "ONB_PUR_BUDGET_NT",
            "IS_PLURAL_BASIS", "IS_OPEN_CONTRACT", "IS_JUXTAPOSED_MANUFACTURER", "OVC_DOPEN_AND_RESULT", "TTL_BID_GROUP", "TTL_PERMI_GROUP", "OVC_DBID", "ONB_MONEY_NT", "OVC_MEMO", "ONB_MONEY",
            "OVC_VEN_TITLE", "ONB_DELIVERY_TIMES", "OVC_DCONTRACT", "OVC_DELIVERY_CONTRACT", "OVC_DELIVERY", "OVC_DINSPECT_END", "OVC_DCLOSE", "OVC_PUR_DCANPO_YN", "AUDITTOTALDAY", "OVC_DRECEIVE_PAPER", "OVC_DRECEIVE", "OVC_PUR_ALLOW", "OVC_PUR_DAPPROVE" };
        string[] strCheckBoxIDs = { "chkOVC_PURCH", "chkOVC_PUR_IPURCH", "chkOVC_PUR_SECTION", "chkOVC_PUR_APPROVE_DEP", "chkOVC_DAPPLY", "chkOVC_DPROPOSE", "chkOVC_DRECEIVE", "chkOVC_PLAN_PUR_DAPPROVE", "chkOVC_PUR_ALLOW", "chkOVC_PUR_DAPPROVE",
            "chkOVC_PUR_APPROVE", "chkOVC_CHECKER", "chkOVC_PLAN_PURCH", "chkOVC_PUR_AGENCY", "chkOVC_LAB", "chkOVC_PUR_ASS_VEN_CODE", "chkGPA", "chkGPA_TYPE", "chkOVC_PURCHASE_UNIT", "chkOVC_NAME", "chkOVC_CONTRACT_UNIT", "chkOVC_DO_NAME", "chkOVC_OPEN_CHECK", "chkOVC_ADVENTAGED_CHECK", "chkONB_PUR_BUDGET_NT",
            "chkIS_PLURAL_BASIS", "chkIS_OPEN_CONTRACT", "chkIS_JUXTAPOSED_MANUFACTURER", "chkOVC_DOPEN_AND_RESULT", "chkTTL_BID_GROUP", "chkTTL_PERMI_GROUP", "chkOVC_DBID", "chkONB_MONEY_NT", "chkOVC_MEMO", "chkONB_MONEY",
            "chkOVC_VEN_TITLE", "chkONB_DELIVERY_TIMES", "chkOVC_DCONTRACT", "chkOVC_DELIVERY_CONTRACT", "chkOVC_DELIVERY", "chkOVC_DINSPECT_END", "chkOVC_DCLOSE", "chkOVC_PUR_DCANPO_YN", "chkAUDITTOTALDAY", "chkOVC_DRECEIVE_PAPER", "chkOVC_DRECEIVE2", "chkOVC_PUR_ALLOW2", "chkOVC_PUR_DAPPROVE2" };
        static string strFormatInt = Variable.strExcleFormatInt;
        static string strFormatMoney2 = Variable.strExcleFormatMoney2;
        string[] strFormat = { null, null, null, null, null, null, null, null, null, null,
                    null, null, null, null, null, null, null, null, null, null, null, null, null, null, strFormatMoney2,
                    null, null, null, null, strFormatInt, strFormatInt, null, strFormatMoney2, null, strFormatMoney2,
                    null, strFormatInt, null, null, null, null, null, null, strFormatInt, null, null, null, null };

        #region 副程式
        private void list_dataImport()
        {
            var query = gm.TBM1407;

            string strTestFirst = "請選擇－可空白";
            //核定權責
            DataTable dtOVC_PUR_APPROVE_DEP = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "E8").ToList());
            FCommon.list_dataImportV(drpOVC_PUR_APPROVE_DEP, dtOVC_PUR_APPROVE_DEP, "OVC_PHR_DESC", "OVC_PHR_ID", strTestFirst, "", ":", true);
            //計畫性質
            DataTable dtOVC_PLAN_PURCH = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "TD").ToList());
            FCommon.list_dataImportV(drpOVC_PLAN_PURCH, dtOVC_PLAN_PURCH, "OVC_PHR_DESC", "OVC_PHR_ID", strTestFirst, "", ":", true);
            //採購途徑
            DataTable dtOVC_PUR_AGENCY = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "C2").ToList());
            FCommon.list_dataImportV(drpOVC_PUR_AGENCY, dtOVC_PUR_AGENCY, "OVC_PHR_DESC", "OVC_PHR_ID", strTestFirst, "", ":", true);
            //採購屬性
            DataTable dtOVC_LAB = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "GN").ToList());
            FCommon.list_dataImportV(drpOVC_LAB, dtOVC_LAB, "OVC_PHR_DESC", "OVC_PHR_ID", strTestFirst, "", ":", true);
            //招標方式
            DataTable dtOVC_PUR_ASS_VEN_CODE = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "C7").ToList());
            FCommon.list_dataImportV(drpOVC_PUR_ASS_VEN_CODE, dtOVC_PUR_ASS_VEN_CODE, "OVC_PHR_DESC", "OVC_PHR_ID", strTestFirst, "", ":", true);

            FCommon.list_dataImportYN(drpGPA, true, strTestFirst, "", true);
            FCommon.list_dataImportYN(drpOVC_OPEN_CHECK, true, strTestFirst, "", true);
            FCommon.list_dataImportYN(drpIS_PLURAL_BASIS, true, strTestFirst, "", true);
            FCommon.list_dataImportYN(drpIS_OPEN_CONTRACT, true, strTestFirst, "", true);
            FCommon.list_dataImportYN(drpIS_JUXTAPOSED_MANUFACTURER, true, strTestFirst, "", true);

            if (strDEPT_SN != null)
            {
                //string strDEPT_SN = "00N00";
                DataTable dt_DEPT = FCommon.getDataTableDEPT_Includesubordinate(strDEPT_SN);
                //FCommon.AlertShow(PnMessage, "success", "系統訊息", dt_DEPT.Rows.Count.ToString());
                FCommon.list_dataImportV(drpOVC_PUR_SECTION, dt_DEPT, "OVC_ONNAME", "OVC_DEPT_CDE", strTestFirst, "", ":", true); //申購單位
                FCommon.list_dataImportV(drpOVC_PURCHASE_UNIT, dt_DEPT, "OVC_ONNAME", "OVC_DEPT_CDE", strTestFirst, "", ":", true); //採購發包單位
                FCommon.list_dataImportV(drpOVC_CONTRACT_UNIT, dt_DEPT, "OVC_ONNAME", "OVC_DEPT_CDE", strTestFirst, "", ":", true); //履約驗結單位
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "單位代碼錯誤，請重新登入！");

            //System.Data.Entity.Core.Objects.ObjectParameter rTN_MSG =
            //    new System.Data.Entity.Core.Objects.ObjectParameter("rTN_MSG", typeof(char));
            //var queryDEPT = gm.PG_TBMDEPT_INCLUDESUBORDINATE("00N00", rTN_MSG);

            //年度
            FCommon.list_dataImportYear(drpYear);
        }
        private string getAllYearText()
        {
            string strYearFirst = drpYear.Items[drpYear.Items.Count - 1].Text;
            if (strYearFirst.Length > 2)
                strYearFirst = strYearFirst.Substring(strYearFirst.Length - 2, 2);
            string strYearLost = drpYear.Items[0].Text;
            if (strYearLost.Length > 2)
                strYearLost = strYearLost.Substring(strYearLost.Length - 2, 2);
            return $"{ strYearFirst }~{ strYearLost }";
        }
        private DataTable getDataTable_Export(DataTable dt_Source, out int intCount_Data)
        {
            DataTable dt = new DataTable();
            intCount_Data = dt_Source.Rows.Count;
            if (intCount_Data > 0)
            {
                string strFieldNo = "項次";
                dt.Columns.Add(strFieldNo);
                int intCount_Field = strFieldNames.Length;
                bool[] isChecked = new bool[intCount_Field];
                for (int i = 0; i < intCount_Data; i++)
                {
                    DataRow dr = dt.NewRow(), dr_Source = dt_Source.Rows[i];
                    dr[strFieldNo] = (i + 1).ToString();
                    for (int j = 0; j < intCount_Field; j++)
                    {
                        string strFieldName = strFieldNames[j];
                        string strFieldSql = strFieldSqls[j];
                        if (i == 0)
                        { //新增欄位
                            string strCheckBoxID = strCheckBoxIDs[j];
                            CheckBox theCheckBox = (CheckBox)pnField.FindControl(strCheckBoxID);
                            isChecked[j] = theCheckBox != null && theCheckBox.Checked; //判斷有選取為顯示
                            if (isChecked[j])
                                dt.Columns.Add(strFieldName);
                        }
                        if (isChecked[j] && dt_Source.Columns.Contains(strFieldSql))
                        {
                            dr[strFieldName] = dr_Source[strFieldSql];
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
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
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_DAPPLY1, txtOVC_DAPPLY2, txtOVC_DPROPOSE1, txtOVC_DPROPOSE2, txtOVC_PLAN_PUR_DAPPROVE1, txtOVC_PLAN_PUR_DAPPROVE2, txtOVC_PUR_ALLOW1, txtOVC_PUR_ALLOW2, txtOVC_PUR_DAPPROVE1, txtOVC_PUR_DAPPROVE2);
                    list_dataImport();
                }
            }
        }

        #region OnClick
        protected void btnQuery_level_Click(object sender, EventArgs e) //計評階段查詢
        {
            if (strDEPT_SN != null)
            {
                //string strYear = "06";
                //string strUnit = "00N00";
                bool isAllYear = chkYear.Checked;
                string strYearText = "";
                //var queryDEPT = from table in gm.TBMDEPTs select table;
                //var queryParent =
                //    from tableP in queryDEPT
                //    where tableP.OVC_DEPT_CDE.Equals("")
                //    select tableP;
                #region Linq搜尋
                #region 資料表Init
                var queryDEPT =
                    from table in mpms.TBMDEPTs
                    select table;
                var queryTBM1118_1 =
                    from table in mpms.TBM1118_1
                        //where table.OVC_PURCH.Substring(2, 2).Equals(strYear)
                    select table;
                var queryTBM1202 =
                    from table in mpms.TBM1202
                        //where table.OVC_PURCH.Substring(2, 2).Equals(strYear)
                    select table;
                var queryTBM1220_1 =
                    from table in mpms.TBM1220_1
                        //where table.OVC_PURCH.Substring(2, 2).Equals(strYear)
                    select table;
                var queryTBM1202_6 =
                    from table in mpms.TBM1202_6
                    where table.OVC_CHECK_UNIT.Equals(strDEPT_SN)
                    //where table.OVC_PURCH.Substring(2, 2).Equals(strYear)
                    select table;
                var queryTBM1301 =
                    from table in mpms.TBM1301
                        //where table.OVC_PURCH.Substring(2, 2).Equals(strYear)
                    select table;
                var queryTBM1301_PLAN =
                    from table in mpms.TBM1301_PLAN
                        //where table.OVC_PURCH.Substring(2, 2).Equals(strYear)
                    select table;
                var queryTBM1302 =
                    from table in mpms.TBM1302
                        //where table.OVC_PURCH.Substring(2, 2).Equals(strYear)
                    select table;
                var queryTBM1303 =
                    from table in mpms.TBM1303
                        //where table.OVC_PURCH.Substring(2, 2).Equals(strYear)
                    select table;
                var queryTBM1407 =
                    from table in mpms.TBM1407
                    select table;
                var queryTBMBID_RESULT =
                    from table in mpms.TBMBID_RESULT
                        //where table.OVC_PURCH.Substring(2, 2).Equals(strYear)
                    select table;
                var queryTBMDELIVERY =
                    from table in mpms.TBMDELIVERY
                        //where table.OVC_PURCH.Substring(2, 2).Equals(strYear)
                    select table;
                var queryTBMRECEIVE_CONTRACT =
                    from table in mpms.TBMRECEIVE_CONTRACT
                        //where table.OVC_PURCH.Substring(2, 2).Equals(strYear)
                    select table;
                #endregion
                #region 年份篩選
                if (isAllYear)
                { //篩選年份選單
                    int intCount_Year = drpYear.Items.Count;
                    string[] strYears = new string[intCount_Year];
                    for (int i = 0; i < intCount_Year; i++)
                    {
                        ListItem theItem = drpYear.Items[i];
                        string strYear = theItem.Value;
                        if (strYear.Length > 2)
                            strYear = strYear.Substring(strYear.Length - 2, 2);
                        strYears[i] = strYear;
                    }
                    queryTBM1301 = queryTBM1301.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                    queryTBM1301_PLAN = queryTBM1301_PLAN.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                    queryTBM1118_1 = queryTBM1118_1.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                    queryTBM1202 = queryTBM1202.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                    queryTBM1220_1 = queryTBM1220_1.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                    queryTBM1202_6 = queryTBM1202_6.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                    queryTBM1302 = queryTBM1302.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                    queryTBM1303 = queryTBM1303.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                    queryTBMBID_RESULT = queryTBMBID_RESULT.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                    queryTBMDELIVERY = queryTBMDELIVERY.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                    queryTBMRECEIVE_CONTRACT = queryTBMRECEIVE_CONTRACT.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                    strYearText = getAllYearText(); //取得年度字串
                }
                else
                { //篩選單一年份
                    string strYear = drpYear.SelectedValue;
                    if (strYear.Length > 2)
                        strYear = strYear.Substring(strYear.Length - 2, 2);

                    queryTBM1301 = queryTBM1301.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                    queryTBM1301_PLAN = queryTBM1301_PLAN.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                    queryTBM1118_1 = queryTBM1118_1.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                    queryTBM1202 = queryTBM1202.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                    queryTBM1220_1 = queryTBM1220_1.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                    queryTBM1202_6 = queryTBM1202_6.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                    queryTBM1302 = queryTBM1302.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                    queryTBM1303 = queryTBM1303.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                    queryTBMBID_RESULT = queryTBMBID_RESULT.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                    queryTBMDELIVERY = queryTBMDELIVERY.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                    queryTBMRECEIVE_CONTRACT = queryTBMRECEIVE_CONTRACT.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                    strYearText = strYear; //取得年度字串
                }
                #endregion

                #region 各個資料表建置
                //採購主檔
                //var queryA = queryTBM1301;
                var queryA =
                    from tableA in queryTBM1301
                        //申購單位
                    join deptSECTION in queryDEPT on tableA.OVC_PUR_SECTION equals deptSECTION.OVC_DEPT_CDE into tempSECTION
                    from deptSECTION in tempSECTION.DefaultIfEmpty()
                        //計畫性質
                    join tablePLAN in queryTBM1407.Where(table => table.OVC_PHR_CATE.Equals("TD")) on tableA.OVC_PLAN_PURCH equals tablePLAN.OVC_PHR_ID into tempPLAN
                    from tablePLAN in tempPLAN.DefaultIfEmpty()
                        //採購途徑
                    join tableAGENCY in queryTBM1407.Where(table => table.OVC_PHR_CATE.Equals("C2")) on tableA.OVC_PUR_AGENCY equals tableAGENCY.OVC_PHR_ID into tempAGENCY
                    from tableAGENCY in tempAGENCY.DefaultIfEmpty()
                        //採購屬性
                    join tableLAB in queryTBM1407.Where(table => table.OVC_PHR_CATE.Equals("GN")) on tableA.OVC_LAB equals tableLAB.OVC_PHR_ID into tempLAB
                    from tableLAB in tempLAB.DefaultIfEmpty()
                        //招標方式
                    join tableASS_VEN_CODE in queryTBM1407.Where(table => table.OVC_PHR_CATE.Equals("C7")) on tableA.OVC_PUR_ASS_VEN_CODE equals tableASS_VEN_CODE.OVC_PHR_ID into tempASS_VEN_CODE
                    from tableASS_VEN_CODE in tempASS_VEN_CODE.DefaultIfEmpty()
                    select new
                    {
                        tableA.OVC_PURCH,
                        tableA.OVC_PUR_ALLOW,
                        tableA.OVC_PUR_AGENCY, //採購途徑
                        OVC_PUR_AGENCY_TEXT = tableAGENCY.OVC_PHR_DESC,
                        tableA.OVC_PUR_SECTION, //申購單位
                        OVC_PUR_SECTION_TEXT = deptSECTION.OVC_ONNAME,
                        tableA.OVC_PUR_IPURCH,
                        tableA.OVC_PLAN_PURCH, //計畫性質
                        OVC_PLAN_PURCH_TEXT = tablePLAN.OVC_PHR_DESC,
                        tableA.OVC_LAB, //採購屬性
                        OVC_LAB_TEXT = tableLAB.OVC_PHR_DESC,
                        tableA.OVC_PUR_ASS_VEN_CODE, //招標方式
                        OVC_PUR_ASS_VEN_CODE_TEXT = tableASS_VEN_CODE.OVC_PHR_DESC,
                        tableA.ONB_PUR_BUDGET_NT,
                        tableA.OVC_DPROPOSE, //申購單位實際申請日期
                        tableA.IS_PLURAL_BASIS, //是否複數決標(Y/N)
                        tableA.IS_OPEN_CONTRACT, //是否開放式契(Y/N)
                        tableA.IS_JUXTAPOSED_MANUFACTURER, //是否並列得標廠商(Y/N)
                        tableA.OVC_PUR_DAPPROVE, //核定發文日期
                        tableA.OVC_PUR_APPROVE, //核定文號
                        tableA.OVC_PUR_DCANPO //撤案日期
                    };
                //預劃主檔
                var queryB =
                    from tableB in queryTBM1301_PLAN
                    where tableB.OVC_AUDIT_UNIT.Equals(strDEPT_SN)
                    where tableB.OVC_DCANCEL == null //預劃沒有撤案
                    //核定權責
                    join tableAPPROVE in queryTBM1407.Where(table => table.OVC_PHR_CATE.Equals("E8")) on tableB.OVC_PUR_APPROVE_DEP equals tableAPPROVE.OVC_PHR_ID into tempAPPROVE
                    from tableAPPROVE in tempAPPROVE.DefaultIfEmpty()
                        //採購發包單位代碼
                    join deptPURCHASE in queryDEPT on tableB.OVC_PURCHASE_UNIT equals deptPURCHASE.OVC_DEPT_CDE into tempPURCHASE
                    from deptPURCHASE in tempPURCHASE.DefaultIfEmpty()
                        //履約驗結單位代碼
                    join deptCONTRACT in queryDEPT on tableB.OVC_CONTRACT_UNIT equals deptCONTRACT.OVC_DEPT_CDE into tempCONTRACT
                    from deptCONTRACT in tempCONTRACT.DefaultIfEmpty()
                    select new
                    {
                        tableB.OVC_PURCH,
                        tableB.OVC_DAPPLY,
                        tableB.OVC_PUR_APPROVE_DEP,
                        OVC_PUR_APPROVE_DEP_TEXT = tableAPPROVE.OVC_PHR_DESC,
                        tableB.OVC_OPEN_CHECK,
                        tableB.ONB_REVIEW_DAYS,
                        tableB.OVC_PURCHASE_UNIT,
                        OVC_PURCHASE_UNIT_TEXT = deptPURCHASE.OVC_ONNAME,
                        tableB.OVC_CONTRACT_UNIT,
                        OVC_CONTRACT_UNIT_TEXT = deptCONTRACT.OVC_ONNAME
                    };
                //開標紀錄檔(取得最近一次開標日期、開標結果)
                var queryC_B =
                    from tableBB in queryTBM1202
                    where tableBB.OVC_CHECK_UNIT.Equals(strDEPT_SN)
                    join tableB in queryB on tableBB.OVC_PURCH equals tableB.OVC_PURCH
                    group tableBB by new { tableBB.OVC_PURCH, tableBB.OVC_CHECK_UNIT, tableBB.OVC_CHECKER } into groupBB
                    select new
                    {
                        groupBB.Key.OVC_PURCH,
                        groupBB.Key.OVC_CHECK_UNIT,
                        groupBB.Key.OVC_CHECKER,
                        OVC_DRECEIVE = groupBB.Min(bb => bb.OVC_DRECEIVE)
                    };
                var queryC =
                    from tableAA in queryTBM1202
                    join tableBB in queryC_B
                    on new { tableAA.OVC_PURCH, tableAA.OVC_CHECK_UNIT, tableAA.OVC_CHECKER, tableAA.OVC_DRECEIVE } equals new { tableBB.OVC_PURCH, tableBB.OVC_CHECK_UNIT, tableBB.OVC_CHECKER, tableBB.OVC_DRECEIVE }
                    select new
                    {
                        tableAA.OVC_PURCH,
                        tableAA.OVC_CHECK_UNIT,
                        tableAA.OVC_CHECKER,
                        tableAA.OVC_DRECEIVE,
                        tableAA.OVC_DRECEIVE_PAPER
                    };

                //因不同組同時開標會出現多筆所以取一筆
                var queryE_M =
                    from tableM in queryTBM1303
                    join tableB in queryB on tableM.OVC_PURCH equals tableB.OVC_PURCH
                    group tableM by new { tableM.OVC_PURCH, tableM.OVC_PURCH_5 } into groupM
                    select new
                    {
                        groupM.Key.OVC_PURCH,
                        groupM.Key.OVC_PURCH_5,
                        OVC_DOPEN = groupM.Max(m => m.OVC_DOPEN)
                    };
                var queryE =
                    from tableK in queryTBM1303
                    join tableM in queryE_M on new { tableK.OVC_PURCH, tableK.OVC_PURCH_5, tableK.OVC_DOPEN } equals new { tableM.OVC_PURCH, tableM.OVC_PURCH_5, tableM.OVC_DOPEN }
                    group tableK by new { tableK.OVC_PURCH, tableK.OVC_PURCH_5, tableK.OVC_DOPEN, tableK.OVC_NAME } into groupK
                    select new
                    {
                        groupK.Key.OVC_PURCH,
                        groupK.Key.OVC_PURCH_5,
                        groupK.Key.OVC_DOPEN,
                        groupK.Key.OVC_NAME,
                        OVC_RESULT = groupK.Min(k => k.OVC_RESULT)
                    };

                //核定組數
                var queryF =
                    from tableF in queryTBM1118_1
                    join tableB in queryB on tableF.OVC_PURCH equals tableB.OVC_PURCH
                    group tableF by tableF.OVC_PURCH into groupF
                    select new
                    {
                        OVC_PURCH = groupF.Key,
                        TTL_PERMI_GROUP = groupF.Count()
                    };

                //開標紀錄檔(決標組數)
                //開標紀錄檔(決標金額)
                var queryG =
                    from tableG in queryTBM1303
                    where tableG.OVC_RESULT.Equals("0")
                    join tableB in queryB on tableG.OVC_PURCH equals tableB.OVC_PURCH
                    group tableG by tableG.OVC_PURCH into groupG
                    select new
                    {
                        OVC_PURCH = groupG.Key,
                        TTL_BID_GROUP = groupG.Count(), //決標組數
                        ONB_MONEY_NT = groupG.Sum(j => (j.ONB_BID_RESULT * (j.ONB_RESULT_RATE ?? 1)) ?? 0) //決標金額
                    };

                //開標紀錄檔(最大分組數)
                var queryI =
                    from tableI in queryTBM1303
                    join tableB in queryB on tableI.OVC_PURCH equals tableB.OVC_PURCH
                    group tableI by tableI.OVC_PURCH into groupI
                    select new
                    {
                        OVC_PURCH = groupI.Key,
                        TTL_GROUP = groupI.Max(g => g.ONB_GROUP)
                    };

                //H
                var queryH_Temp =
                    from tableH in queryTBMRECEIVE_CONTRACT
                    join tableB in queryB on tableH.OVC_PURCH equals tableB.OVC_PURCH
                    select new
                    {
                        tableH.OVC_PURCH,
                        tableH.OVC_DO_NAME
                    };
                var queryH = queryH_Temp.Distinct();

                //GPA
                var queryK =
                    from tableGPA in queryTBM1220_1
                    join tableB in queryB.AsEnumerable() on tableGPA.OVC_PURCH equals tableB.OVC_PURCH
                    where tableGPA.OVC_IKIND.Equals("M47")
                    //group tableGPA by tableGPA.OVC_PURCH into groupGPA
                    select new
                    {
                        tableGPA.OVC_PURCH,
                        tableGPA.OVC_MEMO
                        //groupGPA.Key,
                        //OVC_MEMO = groupGPA.First().OVC_MEMO
                    };

                //準用最有利標
                var queryL =
                    from tableL in queryTBM1220_1
                    where tableL.OVC_IKIND.Equals("A12")
                    where tableL.OVC_CHECK.Equals("ee")
                    join tableB in queryB on tableL.OVC_PURCH equals tableB.OVC_PURCH
                    group tableL by tableL.OVC_PURCH into groupL
                    select new
                    {
                        OVC_PURCH = groupL.Key,
                        Count = groupL.Count()
                    };

                //計評決標方式片語
                var queryM_B =
                    from tableBB in queryTBM1202_6
                    where tableBB.OVC_IKIND.Equals("A14")
                    join tableB in queryB on tableBB.OVC_PURCH equals tableB.OVC_PURCH
                    orderby tableBB.OVC_PURCH
                    select tableBB;
                var queryM_Temp =
                    from tableM in queryTBM1202_6
                    where tableM.OVC_IKIND.Equals("A14")
                    join tableBB in queryM_B on tableM.ONB_CHECK_TIMES equals tableBB.ONB_CHECK_TIMES
                    select tableM;
                var queryM = queryM_Temp.Distinct();

                //合約金額
                //得標商：同一購案編號會有多筆，以","串接為字串
                //交貨批次
                //簽約日期
                var queryN =
                    from tableN in queryTBM1302.AsEnumerable()
                    join tableB in queryB on tableN.OVC_PURCH equals tableB.OVC_PURCH
                    group tableN by tableN.OVC_PURCH into groupN
                    select new
                    {
                        OVC_PURCH = groupN.Key,
                        ONB_MONEY = groupN.Sum(table => table.ONB_RATE * table.ONB_MONEY) ?? 0, //合約金額
                        OVC_VEN_TITLE = string.Join(",", groupN.Select(table => table.OVC_VEN_TITLE)), //得標商
                        ONB_DELIVERY_TIMES = groupN.Max(table => table.ONB_DELIVERY_TIMES) ?? 1, //交貨批次
                        OVC_DCONTRACT = groupN.Min(table => table.OVC_DCONTRACT), //簽約日期
                    };

                //決標日期
                var queryO =
                    from tableO in queryTBMBID_RESULT
                    join tableB in queryB on tableO.OVC_PURCH equals tableB.OVC_PURCH
                    group tableO by tableO.OVC_PURCH into groupO
                    select new
                    {
                        OVC_PURCH = groupO.Key,
                        OVC_DBID = groupO.Max(table => table.OVC_DBID) //決標日期
                    };

                //合約交貨日期(以最後一個為主)
                //實際交貨日期
                var queryP_DB =
                    from tableDB in queryTBM1302
                    join tableB in queryB on tableDB.OVC_PURCH equals tableB.OVC_PURCH
                    group tableDB by tableDB.OVC_PURCH into groupDB
                    select new
                    {
                        OVC_PURCH = groupDB.Key,
                        OVC_DBID = groupDB.Max(table => table.OVC_DBID)
                    };
                var queryP_B =
                    from tableBB in queryTBM1302
                    join tableB in queryB on tableBB.OVC_PURCH equals tableB.OVC_PURCH
                    join tableDB in queryP_DB on new { tableBB.OVC_PURCH, tableBB.OVC_DBID } equals new { tableDB.OVC_PURCH, tableDB.OVC_DBID }
                    select tableBB;
                //queryP_B.OrderBy(table=>table.OVC_PURCH).Dump();
                var queryP_Temp =
                    from tableA in queryTBMDELIVERY
                    join tableBB in queryP_B
                    on new { tableA.OVC_PURCH, tableA.OVC_PURCH_6, tableA.OVC_VEN_CST } equals new { tableBB.OVC_PURCH, tableBB.OVC_PURCH_6, tableBB.OVC_VEN_CST }
                    orderby tableA.OVC_DELIVERY descending
                    select new
                    {
                        tableA.OVC_PURCH,
                        tableA.OVC_DELIVERY_CONTRACT,
                        tableA.OVC_DELIVERY
                    };
                //var queryP = queryP_Temp.Distinct();
                var queryP =
                    from tableP in queryP_Temp.AsEnumerable()
                    group tableP by tableP.OVC_PURCH into groupP
                    select new
                    {
                        OVC_PURCH = groupP.Key,
                        OVC_DELIVERY_CONTRACT = groupP.Last().OVC_DELIVERY_CONTRACT,
                        OVC_DELIVERY = groupP.Last().OVC_DELIVERY
                    };

                //驗結日期
                var queryQ =
                    from tableQ in queryTBMDELIVERY
                    join tableB in queryB on tableQ.OVC_PURCH equals tableB.OVC_PURCH
                    group tableQ by tableQ.OVC_PURCH into groupQ
                    select new
                    {
                        OVC_PURCH = groupQ.Key,
                        OVC_DINSPECT_END = groupQ.Max(table => table.OVC_DPAY)
                    };
                //queryQ.Dump();

                //結案日期
                var queryR =
                    from tableR in queryTBMRECEIVE_CONTRACT
                    join tableB in queryB on tableR.OVC_PURCH equals tableB.OVC_PURCH
                    group tableR by tableR.OVC_PURCH into groupR
                    select new
                    {
                        OVC_PURCH = groupR.Key,
                        OVC_DCLOSE = groupR.Max(table => table.OVC_DCLOSE)
                    };
                #endregion
                #region 部分條件篩選
                //申購單位 a.OVC_PUR_SECTION 測試OK
                string strOVC_PUR_SECTION = drpOVC_PUR_SECTION.SelectedValue;
                if (!strOVC_PUR_SECTION.Equals(string.Empty))
                {
                    //取得單位清單－含下屬單位
                    DataTable dt_DEPT = FCommon.getDataTableDEPT_Includesubordinate(strOVC_PUR_SECTION);
                    int intCount = dt_DEPT.Rows.Count;
                    if (intCount > 0)
                    {
                        string[] strDEPTs = new string[intCount];
                        for (int i = 0; i < intCount; i++)
                        {
                            strDEPTs[i] = dt_DEPT.Rows[i]["OVC_DEPT_CDE"].ToString();
                        }
                        queryA = queryA.Where(table => strDEPTs.Contains(table.OVC_PUR_SECTION));
                    }
                }
                //核定權責 b.OVC_PUR_APPROVE_DEP 測試OK
                string strOVC_PUR_APPROVE_DEP = drpOVC_PUR_APPROVE_DEP.SelectedValue;
                if (!strOVC_PUR_APPROVE_DEP.Equals(string.Empty))
                    queryB = queryB.Where(table => table.OVC_PUR_APPROVE_DEP.Equals(strOVC_PUR_APPROVE_DEP));
                //預劃申購日期 b.OVC_DAPPLY 測試OK
                string strOVC_DAPPLY1 = txtOVC_DAPPLY1.Text;
                if (!strOVC_DAPPLY1.Equals(string.Empty))
                    queryB = queryB.Where(table => table.OVC_DAPPLY.CompareTo(strOVC_DAPPLY1) >= 0);
                string strOVC_DAPPLY2 = txtOVC_DAPPLY2.Text;
                if (!strOVC_DAPPLY2.Equals(string.Empty))
                    queryB = queryB.Where(table => table.OVC_DAPPLY.CompareTo(strOVC_DAPPLY2) <= 0);
                //申購單位實際申請日期 tableA.OVC_DPROPOSE 測試OK
                string strOVC_DPROPOSE1 = txtOVC_DPROPOSE1.Text;
                if (!strOVC_DPROPOSE1.Equals(string.Empty))
                    queryA = queryA.Where(table => table.OVC_DPROPOSE.CompareTo(strOVC_DPROPOSE1) >= 0);
                string strOVC_DPROPOSE2 = txtOVC_DPROPOSE2.Text;
                if (!strOVC_DPROPOSE2.Equals(string.Empty))
                    queryA = queryA.Where(table => table.OVC_DPROPOSE.CompareTo(strOVC_DPROPOSE2) <= 0);
                //主官核批日 tableA.OVC_PUR_ALLOW 測試OK
                string strOVC_PUR_ALLOW1 = txtOVC_PUR_ALLOW1.Text;
                if (!strOVC_PUR_ALLOW1.Equals(string.Empty))
                    queryA = queryA.Where(table => table.OVC_PUR_ALLOW.CompareTo(strOVC_PUR_ALLOW1) >= 0);
                string strOVC_PUR_ALLOW2 = txtOVC_PUR_ALLOW2.Text;
                if (!strOVC_PUR_ALLOW2.Equals(string.Empty))
                    queryA = queryA.Where(table => table.OVC_PUR_ALLOW.CompareTo(strOVC_PUR_ALLOW2) <= 0);
                //核定（發文）日期 tableA.OVC_PUR_DAPPROVE 測試OK
                string strOVC_PUR_DAPPROVE1 = txtOVC_PUR_DAPPROVE1.Text;
                if (!strOVC_PUR_DAPPROVE1.Equals(string.Empty))
                    queryA = queryA.Where(table => table.OVC_PUR_DAPPROVE.CompareTo(strOVC_PUR_DAPPROVE1) >= 0);
                string strOVC_PUR_DAPPROVE2 = txtOVC_PUR_DAPPROVE2.Text;
                if (!strOVC_PUR_DAPPROVE2.Equals(string.Empty))
                    queryA = queryA.Where(table => table.OVC_PUR_DAPPROVE.CompareTo(strOVC_PUR_DAPPROVE2) <= 0);
                //計畫性質 tableA.OVC_PLAN_PURCH 測試OK
                string strOVC_PLAN_PURCH = drpOVC_PLAN_PURCH.SelectedValue;
                if (!strOVC_PLAN_PURCH.Equals(string.Empty))
                    queryA = queryA.Where(table => table.OVC_PLAN_PURCH.Equals(strOVC_PLAN_PURCH));
                //採購途徑 tableA.OVC_PUR_AGENCY 測試OK
                string strOVC_PUR_AGENCY = drpOVC_PUR_AGENCY.SelectedValue;
                if (!strOVC_PUR_AGENCY.Equals(string.Empty))
                    queryA = queryA.Where(table => table.OVC_PUR_AGENCY.Equals(strOVC_PUR_AGENCY));
                //採購屬性 tableA.OVC_LAB 測試OK
                string strOVC_LAB = drpOVC_LAB.SelectedValue;
                if (!strOVC_LAB.Equals(string.Empty))
                    queryA = queryA.Where(table => table.OVC_LAB.Equals(strOVC_LAB));
                //招標方式 tableA.OVC_PUR_ASS_VEN_CODE 測試OK
                string strOVC_PUR_ASS_VEN_CODE = drpOVC_PUR_ASS_VEN_CODE.SelectedValue;
                if (!strOVC_PUR_ASS_VEN_CODE.Equals(string.Empty))
                    queryA = queryA.Where(table => table.OVC_PUR_ASS_VEN_CODE.Equals(strOVC_PUR_ASS_VEN_CODE));
                //採購發包單位 tableB.OVC_PURCHASE_UNIT 測試OK
                string strOVC_PURCHASE_UNIT = drpOVC_PURCHASE_UNIT.SelectedValue;
                if (!strOVC_PURCHASE_UNIT.Equals(string.Empty))
                {
                    //取得單位清單－含下屬單位
                    DataTable dt_DEPT = FCommon.getDataTableDEPT_Includesubordinate(strOVC_PURCHASE_UNIT);
                    int intCount = dt_DEPT.Rows.Count;
                    if (intCount > 0)
                    {
                        string[] strDEPTs = new string[intCount];
                        for (int i = 0; i < intCount; i++)
                        {
                            strDEPTs[i] = dt_DEPT.Rows[i]["OVC_DEPT_CDE"].ToString();
                        }
                        queryB = queryB.Where(table => strDEPTs.Contains(table.OVC_PURCHASE_UNIT));
                    }
                }
                //履約驗結單位 tableB.OVC_CONTRACT_UNIT 測試OK
                string strOVC_CONTRACT_UNIT = drpOVC_CONTRACT_UNIT.SelectedValue;
                if (!strOVC_CONTRACT_UNIT.Equals(string.Empty))
                {
                    //取得單位清單－含下屬單位
                    DataTable dt_DEPT = FCommon.getDataTableDEPT_Includesubordinate(strOVC_CONTRACT_UNIT);
                    int intCount = dt_DEPT.Rows.Count;
                    if (intCount > 0)
                    {
                        string[] strDEPTs = new string[intCount];
                        for (int i = 0; i < intCount; i++)
                        {
                            strDEPTs[i] = dt_DEPT.Rows[i]["OVC_DEPT_CDE"].ToString();
                        }
                        queryB = queryB.Where(table => strDEPTs.Contains(table.OVC_CONTRACT_UNIT));
                    }
                }
                //需公開閱覽 tableB.OVC_OPEN_CHECK 測試OK
                string strOVC_OPEN_CHECK = drpOVC_OPEN_CHECK.SelectedValue;
                if (!strOVC_OPEN_CHECK.Equals(string.Empty))
                    queryB = queryB.Where(table => table.OVC_OPEN_CHECK.Equals(strOVC_OPEN_CHECK) || (strOVC_OPEN_CHECK.Equals("N") && table.OVC_OPEN_CHECK == null));
                //是否複數決標 tableA.IS_PLURAL_BASIS 測試OK
                string strIS_PLURAL_BASIS = drpIS_PLURAL_BASIS.SelectedValue;
                if (!strIS_PLURAL_BASIS.Equals(string.Empty))
                    queryA = queryA.Where(table => table.IS_PLURAL_BASIS.Equals(strIS_PLURAL_BASIS));
                //是否開放式契 tableA.IS_OPEN_CONTRACT 測試OK
                string strIS_OPEN_CONTRACT = drpIS_OPEN_CONTRACT.SelectedValue;
                if (!strIS_OPEN_CONTRACT.Equals(string.Empty))
                    queryA = queryA.Where(table => table.IS_OPEN_CONTRACT.Equals(strIS_OPEN_CONTRACT));
                //是否並列得標廠商 tableA.IS_JUXTAPOSED_MANUFACTURER 測試OK
                string strIS_JUXTAPOSED_MANUFACTURER = drpIS_JUXTAPOSED_MANUFACTURER.SelectedValue;
                if (!strIS_JUXTAPOSED_MANUFACTURER.Equals(string.Empty))
                    queryA = queryA.Where(table => table.IS_JUXTAPOSED_MANUFACTURER.Equals(strIS_JUXTAPOSED_MANUFACTURER));
                #endregion
                #region 資料表Join
                DateTime dateTemp;
                var query =
                    from tableA in queryA.AsEnumerable()
                        //where tableA.OVC_PURCH.Equals("EP06005")
                    let theOVC_PURCH = tableA.OVC_PURCH
                    join tableB in queryB.AsEnumerable() on tableA.OVC_PURCH equals tableB.OVC_PURCH
                    join tableC in queryC.AsEnumerable() on tableA.OVC_PURCH equals tableC.OVC_PURCH into tempC
                    from tableC in tempC.DefaultIfEmpty()
                    join tableE in queryE.AsEnumerable() on tableA.OVC_PURCH equals tableE.OVC_PURCH into tempE
                    from tableE in tempE.DefaultIfEmpty()
                    join tableF in queryF.AsEnumerable() on tableA.OVC_PURCH equals tableF.OVC_PURCH into tempF
                    from tableF in tempF.DefaultIfEmpty()
                    join tableG in queryG.AsEnumerable() on tableA.OVC_PURCH equals tableG.OVC_PURCH into tempG
                    from tableG in tempG.DefaultIfEmpty()
                    join tableI in queryI.AsEnumerable() on tableA.OVC_PURCH equals tableI.OVC_PURCH into tempI
                    from tableI in tempI.DefaultIfEmpty()
                    join tableH in queryH.AsEnumerable() on tableA.OVC_PURCH equals tableH.OVC_PURCH into tempH
                    from tableH in tempH.DefaultIfEmpty()
                    join tableK in queryK.AsEnumerable() on tableA.OVC_PURCH equals tableK.OVC_PURCH into tempK
                    from tableK in tempK.DefaultIfEmpty()
                    join tableL in queryL.AsEnumerable() on tableA.OVC_PURCH equals tableL.OVC_PURCH into tempL
                    from tableL in tempL.DefaultIfEmpty()
                    join tableM in queryM.AsEnumerable() on tableA.OVC_PURCH equals tableM.OVC_PURCH into tempM
                    from tableM in tempM.DefaultIfEmpty()
                    join tableN in queryN.AsEnumerable() on tableA.OVC_PURCH equals tableN.OVC_PURCH into tempN
                    from tableN in tempN.DefaultIfEmpty()
                    join tableO in queryO.AsEnumerable() on tableA.OVC_PURCH equals tableO.OVC_PURCH into tempO
                    from tableO in tempO.DefaultIfEmpty()
                    join tableP in queryP.AsEnumerable() on tableA.OVC_PURCH equals tableP.OVC_PURCH into tempP
                    from tableP in tempP.DefaultIfEmpty()
                    join tableQ in queryQ.AsEnumerable() on tableA.OVC_PURCH equals tableQ.OVC_PURCH into tempQ
                    from tableQ in tempQ.DefaultIfEmpty()
                    join tableR in queryR.AsEnumerable() on tableA.OVC_PURCH equals tableR.OVC_PURCH into tempR
                    from tableR in tempR.DefaultIfEmpty()
                        //確保部分欄位為時間型態
                    where tableB.OVC_DAPPLY == null || DateTime.TryParse(tableB.OVC_DAPPLY, out dateTemp)
                    where tableA.OVC_PUR_ALLOW == null || DateTime.TryParse(tableA.OVC_PUR_ALLOW, out dateTemp)
                    where tableC == null || tableC.OVC_DRECEIVE_PAPER == null || DateTime.TryParse(tableC.OVC_DRECEIVE_PAPER, out dateTemp)

                    select new
                    {
                        tableA.OVC_PURCH,
                        OVC_PURCH_5 = tableE != null ? tableE.OVC_PURCH_5 : null,
                        PURCH = tableA.OVC_PURCH + (tableA.OVC_PUR_AGENCY ?? "") + (tableE != null ? tableE.OVC_PURCH_5 : ""), //a.OVC_PURCH||a.OVC_PUR_AGENCY||NVL(E.OVC_PURCH_5,' ') PURCH, //案號只到第五組
                        OVC_PUR_SECTION = tableA.OVC_PUR_SECTION_TEXT, //tableA.OVC_PUR_SECTION, //申購單位
                        tableA.OVC_PUR_IPURCH, //購案名稱
                        OVC_PUR_APPROVE_DEP = tableB.OVC_PUR_APPROVE_DEP_TEXT, //tableB.OVC_PUR_APPROVE_DEP, //核定權責
                        OVC_PLAN_PURCH = tableA.OVC_PLAN_PURCH_TEXT, //tableA.OVC_PLAN_PURCH, //計畫性質代碼
                        OVC_PUR_AGENCY = tableA.OVC_PUR_AGENCY_TEXT, //tableA.OVC_PUR_AGENCY, //採購單位地區代碼(採購途徑)
                        //多餘tableB.OVC_MILITARY_TYPE, //軍售案類別(1->個別式2->開放式)
                        OVC_LAB = tableA.OVC_LAB_TEXT, //tableA.OVC_LAB, //採購屬性代碼
                        OVC_PUR_ASS_VEN_CODE = tableA.OVC_PUR_ASS_VEN_CODE_TEXT, //OVC_PUR_ASS_VEN_CODE = tableA.OVC_PUR_ASS_VEN_CODE, // ?? " ", //招標方式代碼
                        OVC_OPEN_CHECK = tableB.OVC_OPEN_CHECK ?? "N", //是否公開閱覽(Y/N)
                        ONB_PUR_BUDGET_NT = tableA.ONB_PUR_BUDGET_NT ?? 0, //預算金額(台幣)
                        tableB.OVC_DAPPLY, //預劃申購日期
                        tableA.OVC_DPROPOSE, //申購單位實際申請日期
                        IS_PLURAL_BASIS = tableA.IS_PLURAL_BASIS != null && tableA.IS_PLURAL_BASIS.Equals("Y") ? "是" : "否", //是否複數決標(Y/N)
                        IS_OPEN_CONTRACT = tableA.IS_OPEN_CONTRACT != null && tableA.IS_OPEN_CONTRACT.Equals("Y") ? "是" : "否", //是否開放式契(Y/N)
                        IS_JUXTAPOSED_MANUFACTURER = tableA.IS_JUXTAPOSED_MANUFACTURER != null && tableA.IS_JUXTAPOSED_MANUFACTURER.Equals("Y") ? "是" : "否", //是否並列得標廠商(Y/N)
                        OVC_PLAN_PUR_DAPPROVE = Convert.ToDateTime(tableB.OVC_DAPPLY).AddDays(tableB.ONB_REVIEW_DAYS ?? 0).ToString("yyyy-MM-dd"), //(TO_CHAR((TO_DATE(NVL(B.OVC_DAPPLY,''),'YYYY-MM-DD') + NVL(B.ONB_REVIEW_DAYS,0)),'YYYY-MM-DD')) OVC_PLAN_PUR_DAPPROVE, //預劃核定日期
                        tableA.OVC_PUR_DAPPROVE, // ?? " ", //核定發文日期
                        tableA.OVC_PUR_APPROVE, // ?? " ", //核定文號
                        tableA.OVC_PUR_ALLOW, // ?? " ", //主官核批日
                                              //多餘tableA.OVC_PUR_DCANPO, // ?? " ", //撤案日
                        OVC_PURCHASE_UNIT = tableB.OVC_PURCHASE_UNIT_TEXT, //tableB.OVC_PURCHASE_UNIT, //採購發包單位代碼
                        OVC_CONTRACT_UNIT = tableB.OVC_CONTRACT_UNIT_TEXT, //tableB.OVC_CONTRACT_UNIT, //履約驗結單位代碼
                        OVC_CHECKER = tableC != null ? tableC.OVC_CHECKER : null, //計評承辦人
                        OVC_DRECEIVE = tableC != null ? tableC.OVC_DRECEIVE : null, //計評實際收辦日期(第1次分派日)
                        OVC_DRECEIVE_PAPER = tableC != null ? tableC.OVC_DRECEIVE_PAPER : null, //紙本收文日
                        TTL_BID_GROUP = tableG != null ? tableG.TTL_BID_GROUP : 0, //已決標組數
                                                                                   //多餘TTL_GROUP = tableI != null ? tableI.TTL_GROUP : 0, //總組數
                        TTL_PERMI_GROUP = tableF != null && tableF.TTL_PERMI_GROUP != 0 ? tableF.TTL_PERMI_GROUP : tableI != null ? tableI.TTL_GROUP != 0 ? tableI.TTL_GROUP : 1 : 1, //TTL_PERMI_GROUP = tableF != null ? tableF.TTL_PERMI_GROUP : 0, //核定分組數
                        //OVC_RESULT = tableE != null ? tableE.OVC_RESULT : null, //最近一次開標結果(代碼A8)
                        //OVC_DOPEN = tableE != null ? tableE.OVC_DOPEN : null, //開標日期(如為複數決標最後一組開標日期)
                        OVC_DOPEN_AND_RESULT = tableE != null ? (tableE.OVC_DOPEN ?? "") + (tableE.OVC_RESULT != null && tableE.OVC_RESULT.Equals("0") ? "(決標)" : "") : "", //開標日期及結果
                        OVC_DBID = tableO != null ? tableO.OVC_DBID : null, //決標日期
                        OVC_NAME = tableE != null ? tableE.OVC_NAME : null,  //採包承辦人
                        ONB_MONEY_NT = tableG != null ? tableG.ONB_MONEY_NT : 0, // 決標金額(台幣)(如為複數決標為總決標金額)
                        OVC_DO_NAME = tableH != null ? tableH.OVC_DO_NAME : null, //履驗承辦人
                                                                                  //AUDITTOTALDAY = ( tableA.OVC_PUR_ALLOW == null || tableC == null || tableC.OVC_DRECEIVE_PAPER == null) ? "0" : SqlMethods.DateDiffDay(Convert.ToDateTime(tableC.OVC_DRECEIVE_PAPER), Convert.ToDateTime(tableA.OVC_PUR_ALLOW)).ToString(), //case when NVL(a.OVC_PUR_ALLOW,' ') = ' ' or NVL(C.OVC_DRECEIVE_PAPER,' ') = ' ' then '0' else TO_CHAR(TO_DATE(a.OVC_PUR_ALLOW,'YYYY-MM-DD') - TO_DATE(C.OVC_DRECEIVE_PAPER,'YYYY-MM-DD')) end AUDITTOTALDAY  //評核總天數
                        AUDITTOTALDAY = (tableA.OVC_PUR_ALLOW == null || tableC == null || tableC.OVC_DRECEIVE_PAPER == null) ? "0" : (Convert.ToDateTime(tableA.OVC_PUR_ALLOW) - Convert.ToDateTime(tableC.OVC_DRECEIVE_PAPER)).Days.ToString(), //case when NVL(a.OVC_PUR_ALLOW,' ') = ' ' or NVL(C.OVC_DRECEIVE_PAPER,' ') = ' ' then '0' else TO_CHAR(TO_DATE(a.OVC_PUR_ALLOW,'YYYY-MM-DD') - TO_DATE(C.OVC_DRECEIVE_PAPER,'YYYY-MM-DD')) end AUDITTOTALDAY  //評核總天數

                        GPA = tableK != null ? tableK.OVC_MEMO.Contains("案內品項屬政府採購協定") ? "是" : "否" : "否", //是否適用GPA
                        GPA_TYPE = tableK != null ? tableK.OVC_MEMO : null, //GPA 政府採購協定片語

                        //OVC_BID_METHOD_2 = queryL.Where(table => table.OVC_PURCH.Equals(theOVC_PURCH)).Count(), //OVC_BID_METHOD_2 = tableL != null ? tableL.result>0 ? "Y": "N" : "N", //準用最有利標
                        OVC_ADVENTAGED_CHECK = tableL != null ? tableL.Count > 0 ? "Y" : "N" : "N", //準用最有利標
                        OVC_MEMO = tableM != null ? tableM.OVC_MEMO : "", //計評決標方式片語
                        ONB_MONEY = tableN != null ? tableN.ONB_MONEY : 0, //合約金額 //超慢ONB_MONEY = queryN.Where(table => table.OVC_PURCH.Equals(theOVC_PURCH)).Sum(table => table.ONB_MONEY), //合約金額
                        OVC_VEN_TITLE = tableN != null ? tableN.OVC_VEN_TITLE : "", //得標商
                        ONB_DELIVERY_TIMES = tableN != null ? tableN.ONB_DELIVERY_TIMES : 1, //交貨批次
                        OVC_DCONTRACT = tableN != null ? tableN.OVC_DCONTRACT : null, //簽約日期

                        OVC_DELIVERY = tableP != null ? tableP.OVC_DELIVERY : null, //實際交貨日期
                        OVC_DELIVERY_CONTRACT = tableP != null ? tableP.OVC_DELIVERY_CONTRACT : null, //合約交貨日期(以最後一個為主)
                        OVC_DINSPECT_END = tableQ != null ? tableQ.OVC_DINSPECT_END : null, //驗結日期
                        OVC_DCLOSE = tableR != null ? tableR.OVC_DCLOSE : null, //結案日期
                        OVC_PUR_DCANPO_YN = tableA.OVC_PUR_DCANPO != null ? "是" : "否", //是否撤案
                                                                                       //tableA.OVC_PUR_ALLOW//主官批核
                        List = ""
                    };
                #endregion
                #region 部分條件篩選
                //預劃核定日期 測試OK
                string strOVC_PLAN_PUR_DAPPROVE1 = txtOVC_PLAN_PUR_DAPPROVE1.Text;
                if (!strOVC_PLAN_PUR_DAPPROVE1.Equals(string.Empty))
                    query = query.Where(table => table.OVC_PLAN_PUR_DAPPROVE.CompareTo(strOVC_PLAN_PUR_DAPPROVE1) >= 0);
                string strOVC_PLAN_PUR_DAPPROVE2 = txtOVC_PLAN_PUR_DAPPROVE2.Text;
                if (!strOVC_PLAN_PUR_DAPPROVE2.Equals(string.Empty))
                    query = query.Where(table => table.OVC_PLAN_PUR_DAPPROVE.CompareTo(strOVC_PLAN_PUR_DAPPROVE2) <= 0);
                //計評承辦人 tableC.OVC_CHECKER 測試OK
                string strOVC_CHECKER = txtOVC_CHECKER.Text;
                if (!strOVC_CHECKER.Equals(string.Empty))
                    query = query.Where(table => table.OVC_CHECKER.Equals(strOVC_CHECKER));
                //是否適用GPA 測試OK
                if (!drpGPA.SelectedValue.Equals(string.Empty))
                    query = query.Where(table => table.GPA.Equals(drpGPA.SelectedItem.Text));
                #endregion
                //var queryOrder = query.OrderBy(table => table.OVC_PURCH);
                #endregion

                var queryOrder = query.OrderBy(table => table.OVC_PURCH_5).OrderBy(table => table.OVC_PURCH);
                DataTable dt_Source = CommonStatic.LinqQueryToDataTable(queryOrder), dt = new DataTable();
                //if (false && chkAUDITTOTALDAY.Checked) //判斷是否要印出 評核總天數 
                //{
                //    dt_Source.Columns.Add("AUDITTOTALDAY");
                //    foreach (DataRow dr in dt_Source.Rows)
                //    {
                //        string strOVC_DRECEIVE_PAPER = dr["OVC_DRECEIVE_PAPER"].ToString();
                //        string strOVC_PUR_ALLOW = dr["OVC_PUR_ALLOW"].ToString();
                //        DateTime dateOVC_DRECEIVE_PAPER, dateOVC_PUR_ALLOW;
                //        TimeSpan timeAUDITTOTALDAY;
                //        if (DateTime.TryParse(strOVC_DRECEIVE_PAPER, out dateOVC_DRECEIVE_PAPER) && DateTime.TryParse(strOVC_PUR_ALLOW, out dateOVC_PUR_ALLOW))
                //        {
                //            timeAUDITTOTALDAY = dateOVC_PUR_ALLOW - dateOVC_DRECEIVE_PAPER;
                //            dr["AUDITTOTALDAY"] = timeAUDITTOTALDAY.Days.ToString();
                //        }
                //    }
                //}
                int intCount_Data = dt_Source.Rows.Count;
                if (intCount_Data > 0)
                {
                    string strFieldNo = "項次";
                    dt.Columns.Add(strFieldNo);
                    int intCount_Field = strFieldNames.Length;
                    bool[] isChecked = new bool[intCount_Field];
                    for (int i = 0; i < intCount_Data; i++)
                    {
                        DataRow dr = dt.NewRow(), dr_Source = dt_Source.Rows[i];
                        dr[strFieldNo] = (i + 1).ToString();
                        for (int j = 0; j < intCount_Field; j++)
                        {
                            string strFieldName = strFieldNames[j];
                            string strFieldSql = strFieldSqls[j];
                            if (i == 0)
                            { //新增欄位
                                string strCheckBoxID = strCheckBoxIDs[j];
                                CheckBox theCheckBox = (CheckBox)pnField.FindControl(strCheckBoxID);
                                isChecked[j] = theCheckBox != null && theCheckBox.Checked; //判斷有選取為顯示
                                if (isChecked[j])
                                    dt.Columns.Add(strFieldName);
                            }
                            if (isChecked[j] && dt_Source.Columns.Contains(strFieldSql))
                            {
                                dr[strFieldName] = dr_Source[strFieldSql];
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                    //foreach (Control theControl in pnField.Controls)
                    //{
                    //    if(theControl is CheckBox)
                    //    {
                    //        CheckBox theCheckBox = (CheckBox)theControl;
                    //        if (!theCheckBox.ID.Equals("chkYear") && theCheckBox.Checked)
                    //        {
                    //            string strFieldName = theCheckBox.Text;
                    //            strFieldName = strFieldName.Replace("：", "");
                    //            dt.Columns.Add(strFieldName);
                    //        }
                    //    }
                    //}
                    
                    string strSheetText = "1_計評";
                    string strTitleText = $"{ strYearText }年度{ strDEPT_Name }購案管制表";
                    string strFileName = $"{ strTitleText }.xlsx";

                    MemoryStream Memory = ExcelNPOI.RenderDataTableToStream_Chief(dt, strSheetText, strTitleText, 3, strFormat); //取得Excel資料流
                    FCommon.DownloadFile(this, strFileName, Memory); //直接下載檔案
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查詢無結果，請重新下條件！");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "單位代碼錯誤，請重新登入！");
            #region 原始SQL
            //select a.OVC_PURCH,nvl(e.OVC_PURCH_5, ' ') OVC_PURCH_5,
            //a.OVC_PUR_AGENCY,a.OVC_PURCH || a.OVC_PUR_AGENCY || nvl(e.OVC_PURCH_5, ' ') purch, --//案號只到第五組
            //     a.ovc_pur_nsection, --//申購單位
            //     a.ovc_pur_ipurch, --//購案名稱
            //     b.ovc_pur_approve_dep,--//核定權責
            //     a.ovc_plan_purch,--//計畫性質代碼
            //     a.ovc_pur_agency,--//採購單位地區代碼(採購途徑)
            //     b.ovc_Military_Type, --//軍售案類別(1->個別式2->開放式)
            //     a.ovc_lab,--//採購屬性代碼
            //     nvl(a.ovc_pur_ass_ven_code, ' ') ovc_Pur_Ass_Ven_Code,--//招標方式代碼
            //      nvl(b.ovc_open_check, 'N') OVC_OPEN_CHECK,--//是否公開閱覽(Y/N)
            //       nvl(a.onb_Pur_Budget_Nt, 0.0) onb_Pur_Budget_Nt, --//預算金額(台幣)
            //        b.ovc_Dapply,--//預劃申購日期
            //        a.ovc_Dpropose,--//申購單位實際申請日期
            //        a.is_Plural_basis,--//是否複數決標(Y/N)
            //        a.is_Open_Contract,--//是否開放式契(Y/N)
            //        a.is_Juxtaposed_Manufacturer,--//是否並列得標廠商(Y/N)
            //        (to_char((to_date(nvl(b.ovc_Dapply, ''), 'YYYY-MM-DD') + nvl(b.onb_Review_days, 0)), 'YYYY-MM-DD')) ovc_Plan_Pur_Dapprove,--//預劃核定日期
            //            nvl(a.ovc_Pur_Dapprove, ' ') ovc_Pur_Dapprove,--//核定發文日期
            //             nvl(a.ovc_Pur_Approve, ' ') ovc_Pur_Approve,--//核定文號
            //              nvl(a.ovc_Pur_Allow, ' ') ovc_Pur_Allow,--//主官核批日
            //               nvl(a.ovc_Pur_Dcanpo, ' ') ovc_Pur_Dcanpo, --//撤案日
            //               b.ovc_Purchase_Unit,--//採購發包單位代碼
            //               b.ovc_Contract_Unit, --//履約驗結單位代碼
            //               c.ovc_Checker, --//計評承辦人
            //               c.ovc_dreceive, --//計評實際收辦日期(第1次分派日)
            //               c.ovc_dreceive_paper, --//紙本收文日
            //               nvl(g.TTL_BID_GROUP, 0) TTL_BID_GROUP, --//已決標組數
            //                nvl(i.TTL_GROUP, 0) TTL_GROUP, --//總組數
            //                 nvl(f.ttl_permi_group, 0) ttl_permi_group, --//核定分組數
            //                  nvl(e.OVC_RESULT, ' ') OVC_RESULT, --//最近一次開標結果(代碼A8)
            //                   nvl(e.OVC_DOPEN, ' ') OVC_DOPEN,--//開標日期(如為複數決標最後一組開標日期)
            //                    nvl(e.OVC_NAME, ' ') OVC_NAME,  --//採包承辦人
            //                     j.onb_Money_NT, --// 決標金額(台幣)(如為複數決標為總決標金額)
            //                     H.ovc_do_name,  --//履驗承辦人
            //case when nvl(a.ovc_pur_allow,' ') = ' ' or nvl(c.ovc_dreceive_paper,' ') = ' ' then '0' else to_char(to_date(a.ovc_pur_allow, 'YYYY-MM-DD') - to_date(c.ovc_dreceive_paper, 'YYYY-MM-DD')) end audittotalday  --//評核總天數
            //  from tbm1301 a,  --//--採購主檔
            //       TBM1301_plan b, --// --預劃主檔
            //       (select aa.ovc_purch,aa.ovc_Check_Unit,aa.ovc_Checker,aa.ovc_dreceive,aa.ovc_dreceive_paper--//計評主檔(計評承辦人),第1次分派日
            //        from tbm1202 aa, 
            //       (select ovc_purch, ovc_Check_Unit, ovc_Checker, min(ovc_dreceive) ovc_dreceive
            //             from tbm1202 where ovc_Check_Unit = '00N00'
            //          and substr(ovc_purch,3,2)= '06'
            //          group by ovc_purch,ovc_Check_Unit,ovc_Checker ) bb
            //       where aa.ovc_purch = bb.ovc_purch and aa.ovc_Check_Unit = bb.ovc_Check_Unit
            //          and aa.ovc_Checker = bb.ovc_Checker and aa.ovc_dreceive = bb.ovc_dreceive) c, 
            //   (select ovc_purch, OVC_PURCH_5, OVC_DOPEN, OVC_NAME, min(OVC_RESULT) OVC_RESULT from
            //         (select k.ovc_purch, k.OVC_PURCH_5, k.OVC_DOPEN, k.OVC_RESULT, k.OVC_NAME --//開標紀錄檔(取得最近一次開標日期、開標結果)
            //               from TBM1303 k,  
            //        (select ovc_purch, OVC_PURCH_5, max(OVC_DOPEN) OVC_DOPEN
            //             from tbm1303 group by ovc_purch, OVC_PURCH_5
            //           having substr(ovc_purch,3,2) = '06' ) m
            //         where k.ovc_purch = m.ovc_purch and k.ovc_purch_5 = m.OVC_PURCH_5 and
            //               k.OVC_DOPEN = m.OVC_DOPEN ) 
            //     group by ovc_purch,OVC_PURCH_5,OVC_DOPEN,OVC_NAME) e,--//因不同組同時開標會出現多筆所以取一筆
            //     (SELECT ovc_purch, count(1) ttl_permi_group FROM TBM1118_1 where substr(ovc_purch, 3, 2) = '06' group by ovc_purch ) f,  --//核定組數
            //         (select ovc_purch, count(1) TTL_BID_GROUP--//開標紀錄檔(決標組數)
            //         from tbm1303  where ovc_Result = '0'
            //         group by ovc_purch
            //         having substr(ovc_purch,3,2) = '06' ) g,  
            //     (select ovc_purch, max(onb_group) TTL_GROUP--//開標紀錄檔(最大分組數)
            //         from tbm1303  where substr(ovc_purch, 3, 2) = '06'
            //         group by ovc_purch ) i, 
            //     (select ovc_purch, sum(nvl((ONB_BID_RESULT * nvl(ONB_RESULT_RATE, 1)), 0.0)) onb_Money_NT--//開標紀錄檔(決標金額)
            //         from tbm1303  where ovc_Result = '0'
            //         group by ovc_purch
            //         having substr(ovc_purch,3,2) = '06' ) j, 
            //     (select distinct ovc_purch, ovc_do_name from tbmreceive_contract where substr(ovc_purch, 3, 2) = '06' ) h
            //    where a.ovc_purch = b.ovc_purch and
            //       a.ovc_purch = c.ovc_purch(+) and
            //       a.ovc_purch = e.ovc_purch(+) and
            //       a.ovc_purch = f.ovc_purch(+) and
            //       a.ovc_purch = g.ovc_purch(+) and
            //       a.ovc_purch = i.ovc_purch(+) and
            //       a.ovc_purch = j.ovc_purch(+) and
            //       a.ovc_purch = H.ovc_purch(+) and
            //       substr(a.ovc_purch, 3, 2) = '06';
            #endregion
        }
        protected void btnQuery_ONB_Click(object sender, EventArgs e) //採包階段查詢
        {
            if (strDEPT_SN != null)
            {
                string[] strParameterName = { ":qry_YY", ":USER_DEPT_SN" };
                ArrayList aryData = new ArrayList();

                #region 年份篩選
                //string strYear = "06";
                string strYearSQL, strYear, strYearText = "";
                bool isAllYear = chkYear.Checked;
                if (isAllYear)
                { //篩選年份選單
                    int intCount_Year = drpYear.Items.Count;
                    //string[] strYears = new string[intCount_Year];
                    string strYearList = "";
                    for (int i = 0; i < intCount_Year; i++)
                    {
                        ListItem theItem = drpYear.Items[i];
                        strYear = theItem.Value;
                        if (strYear.Length > 2)
                            strYear = strYear.Substring(strYear.Length - 2, 2);
                        //strYears[i] = strYear;
                        if (i != 0) strYearList += ", ";
                        strYearList += "'" + strYear + "'";
                    }

                    //string strYearList = string.Join(",", strYears);
                    strYearSQL = $" in ({ strYearList }, { strParameterName[0] })";
                    strYearText = getAllYearText(); //取得年度字串
                }
                else
                { //篩選單一年份
                    strYearSQL = $" = { strParameterName[0] }";
                }
                strYear = drpYear.SelectedValue;
                if (strYear.Length > 2)
                    strYear = strYear.Substring(strYear.Length - 2, 2);
                if (!isAllYear) strYearText = strYear; //取得年度字串
                aryData.Add(strYear);
                aryData.Add(strDEPT_SN);
                #endregion

                #region SQL語法
                string strSQL = "";
                #region 欄位
                strSQL += $@"
                    select a.OVC_PURCH,
                        FGETCONTRACTDESC(a.OVC_PURCH,SUBSTR(a.OVC_PURCH,3,2),'OVC_PURCH_5') OVC_PURCH_5,
                        FGETCONTRACTDESC(a.OVC_PURCH,SUBSTR(a.OVC_PURCH,3,2),'OVC_PURCH_6') OVC_PURCH_6,
                        --a.OVC_PUR_AGENCY,
                        a.OVC_PURCH||a.OVC_PUR_AGENCY||FGETCONTRACTDESC(a.OVC_PURCH,SUBSTR(a.OVC_PURCH,3,2),'OVC_PURCH_5') ||FGETCONTRACTDESC(a.OVC_PURCH,SUBSTR(a.OVC_PURCH,3,2),'OVC_PURCH_6') PURCH,--//案號只到第五組
                        t_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --a.OVC_PUR_NSECTION, --//申購單位
                        a.OVC_PUR_IPURCH, --//購案名稱
                        t_PUR_APPROVE_DEP.OVC_PHR_DESC OVC_PUR_APPROVE_DEP, --B.OVC_PUR_APPROVE_DEP,--//核定權責
                        t_PLAN_PURCH.OVC_PHR_DESC OVC_PLAN_PURCH, --a.OVC_PLAN_PURCH,--//計畫性質代碼
                        case when (a.OVC_PUR_AGENCY<>'M') then T_PUR_AGENCY.OVC_PHR_DESC else (T_PUR_AGENCY.OVC_PHR_DESC) end OVC_PUR_AGENCY, --//採購單位地區代碼(採購途徑)
                        case when B.OVC_MILITARY_TYPE='1' then '(個別式)' else (case when B.OVC_MILITARY_TYPE='2' then '(開放式)' else '' end) end OVC_MILITARY_TYPE, --//軍售案類別(1->個別式2->開放式)
                        --t_PUR_AGENCY.OVC_PHR_DESC OVC_PUR_AGENCY, --a.OVC_PUR_AGENCY,--//採購單位地區代碼(採購途徑)
                        --B.OVC_MILITARY_TYPE, --//軍售案類別(1->個別式2->開放式)
                        t_LAB.OVC_PHR_DESC OVC_LAB, --a.OVC_LAB,--//採購屬性代碼
                        t_PUR_ASS_VEN_CODE.OVC_PHR_DESC OVC_PUR_ASS_VEN_CODE, --NVL(a.OVC_PUR_ASS_VEN_CODE,' ') OVC_PUR_ASS_VEN_CODE,--//招標方式代碼
                        NVL(B.OVC_OPEN_CHECK,'N') OVC_OPEN_CHECK,--//是否公開閱覽(Y/N)
                        NVL(a.ONB_PUR_BUDGET_NT,0.0) ONB_PUR_BUDGET_NT, --//總預算金額(台幣)
                        FGETCONTRACTDESC(a.OVC_PURCH,SUBSTR(a.OVC_PURCH,3,2),'onb_Bud_Money')*FGETCONTRACTDESC(a.OVC_PURCH,SUBSTR(a.OVC_PURCH,3,2),'onb_Bud_Rate') ONB_BUD_MONEY_NT,--// 分組預算金額(台幣)
                        B.OVC_DAPPLY,--//預劃申購日期
                        a.OVC_DPROPOSE,--//申購單位實際申請日期
                        case when a.IS_PLURAL_BASIS='Y' then '是' else '否' end IS_PLURAL_BASIS,--//是否複數決標(Y/N)
                        case when a.IS_OPEN_CONTRACT='Y' then '是' else '否' end IS_OPEN_CONTRACT,--//是否開放式契(Y/N)
                        case when a.IS_JUXTAPOSED_MANUFACTURER='Y' then '是' else '否' end IS_JUXTAPOSED_MANUFACTURER,--//是否並列得標廠商(Y/N)
                        --a.IS_OPEN_CONTRACT,--//是否開放式契(Y/N)
                        --a.IS_JUXTAPOSED_MANUFACTURER,--//是否並列得標廠商(Y/N)
                        (TO_CHAR((TO_DATE(NVL(B.OVC_DAPPLY, ''), 'YYYY-MM-DD') + NVL(B.ONB_REVIEW_DAYS, 0)), 'YYYY-MM-DD')) OVC_PLAN_PUR_DAPPROVE,--//預劃核定日期
                        NVL(a.OVC_PUR_DAPPROVE, ' ') OVC_PUR_DAPPROVE,--//核定發文日期
                        NVL(a.OVC_PUR_APPROVE, ' ') OVC_PUR_APPROVE,--//核定文號
                        NVL(a.OVC_PUR_ALLOW, ' ') OVC_PUR_ALLOW,--//主官核批日
                        NVL(a.OVC_PUR_DCANPO, ' ') OVC_PUR_DCANPO, --//撤案日
                        case when a.OVC_PUR_DCANPO is null then '否' else '是' end OVC_PUR_DCANPO_YN, --//是否撤案
                        t_PURCHASE_UNIT.OVC_ONNAME OVC_PURCHASE_UNIT, --B.OVC_PURCHASE_UNIT,--//採購發包單位代碼
                        t_CONTRACT_UNIT.OVC_ONNAME OVC_CONTRACT_UNIT, --B.OVC_CONTRACT_UNIT, --//履約驗結單位代碼
                        C.OVC_CHECKER, --//計評承辦人
                        C.OVC_DRECEIVE, --//計評實際收辦日期(第1次分派日)
                        C.OVC_DRECEIVE_PAPER, --//紙本收文日
                        NVL(G.TTL_BID_GROUP, 0) TTL_BID_GROUP, --//已決標組數
                        FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ovc_result') OVC_RESULT,--//最近一次開標結果(代碼A8)
                        FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ovc_dopen') OVC_DOPEN,--//開標日期
                        FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ONB_GROUP') ONB_GROUP,--//組別
                        NVL(FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ONB_BID_RESULT') * FGETCONTRACTDESC(a.OVC_PURCH, '05', 'ONB_RESULT_RATE'),0) ONB_MONEY_NT, --// 決標金額(台幣)
                        FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ovc_Dbid') OVC_DBID,--//決標日
                        FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ovc_Ven_Title') OVC_VEN_TITLE,--//得標商名稱
                        FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ovc_Dcontract') OVC_DCONTRACT,--//簽約日
                        NVL(FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'onb_Delivery_Times'),0) ONB_DELIVERY_TIMES, --//交貨批次
                        NVL(FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'onb_Money') * FGETCONTRACTDESC(a.OVC_PURCH, '05', 'onb_Rate'),0) ONB_MONEY, --//合約金額
                        FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ovc_Delivery') OVC_DELIVERY,--//實際交貨日期
                        FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ovc_Delivery_Contract') OVC_DELIVERY_CONTRACT, --//契約交貨日期
                        FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ovc_Dpay') OVC_DINSPECT_END, --//結報日期(驗結日期)
                        NVL(I.TTL_GROUP, 0) TTL_GROUP, --//總組數
                        case when NVL(U.TTL_PERMI_GROUP, 0)<>0 then NVL(U.TTL_PERMI_GROUP, 0) else case when NVL(I.TTL_GROUP, 0)=0 then 1 else NVL(I.TTL_GROUP, 0) end end TTL_PERMI_GROUP, --//核定分組數
                        FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'OVC_NAME') OVC_NAME, --//採包承辦人
                        T.OVC_DO_NAME,  --//履驗承辦人
                        case when NVL(a.OVC_PUR_ALLOW,' ') = ' ' or NVL(C.OVC_DRECEIVE_PAPER,' ') = ' ' then '0' else TO_CHAR(TO_DATE(a.OVC_PUR_ALLOW, 'YYYY-MM-DD') - TO_DATE(C.OVC_DRECEIVE_PAPER, 'YYYY-MM-DD')) end AUDITTOTALDAY,  --//評核總天數
                        case when instr(GPA.OVC_MEMO, '案內品項屬政府採購協定')>0 then '是' else '否' end GPA, --//是否適用GPA
                        GPA.OVC_MEMO GPA_TYPE, --//政府採購協定片語
                        case when L.vCase>0 then 'Y' else 'N' end OVC_ADVENTAGED_CHECK --//準用最有利標
                    ";
                #endregion
                #region 資料表
                strSQL += $@"
                    from
                        TBM1301 a,  --//--採購主檔
                        TBM1301_PLAN B, --// --預劃主檔
                        (select AA.OVC_PURCH,AA.OVC_CHECK_UNIT,AA.OVC_CHECKER,AA.OVC_DRECEIVE,AA.OVC_DRECEIVE_PAPER--//計評主檔(計評承辦人),第1次分派日
                            from TBM1202 AA, 
                            (select OVC_PURCH, OVC_CHECK_UNIT, OVC_CHECKER, min(OVC_DRECEIVE) OVC_DRECEIVE
                                from TBM1202 where OVC_CHECK_UNIT = { strParameterName[1] } and SUBSTR(OVC_PURCH,3,2){strYearSQL}
                                group by OVC_PURCH,OVC_CHECK_UNIT,OVC_CHECKER ) BB
                            where AA.OVC_PURCH = BB.OVC_PURCH and AA.OVC_CHECK_UNIT = BB.OVC_CHECK_UNIT
                            and AA.OVC_CHECKER = BB.OVC_CHECKER and AA.OVC_DRECEIVE = BB.OVC_DRECEIVE) C, 
                        (select OVC_PURCH, COUNT(1) TTL_BID_GROUP--//開標紀錄檔(決標組數)
                            from TBM1303  where OVC_RESULT = '0'
                            group by OVC_PURCH
                            having SUBSTR(OVC_PURCH,3,2){strYearSQL} ) G,  
                        (select OVC_PURCH, max(ONB_GROUP) TTL_GROUP--//開標紀錄檔(最大分組數)
                            from TBM1303  where SUBSTR(OVC_PURCH, 3, 2){strYearSQL}
                            group by OVC_PURCH ) I, 
                        (select distinct OVC_PURCH, OVC_DO_NAME 
                            from TBMRECEIVE_CONTRACT where SUBSTR(OVC_PURCH, 3, 2){strYearSQL} ) T, 
                        (select OVC_PURCH, COUNT(1) TTL_PERMI_GROUP 
                            from TBM1118_1 where SUBSTR(OVC_PURCH, 3, 2){strYearSQL} group by OVC_PURCH ) U,--//核定組數
                        (select OVC_PURCH, OVC_MEMO from TBM1220_1 where SUBSTR(OVC_PURCH, 3, 2){strYearSQL} and OVC_IKIND='M47' and ONB_NO=1) GPA, --//政府採購協定片語 GPA
                        (SELECT OVC_PURCH, count(1) vCase from tbm1220_1 where SUBSTR(OVC_PURCH, 3, 2){strYearSQL} and ovc_ikind = 'A12' and ovc_check = 'ee' group by OVC_PURCH) L, --//準用最有利標

                        (SELECT OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PUR_SECTION, --//申購單位
                        (SELECT OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PURCHASE_UNIT, --//採購發包單位
                        (SELECT OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_CONTRACT_UNIT, --//履約驗結單位
                        (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='E8') t_PUR_APPROVE_DEP, --//核定權責
                        (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='TD') t_PLAN_PURCH, --//計畫性質
                        (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='C2') t_PUR_AGENCY, --//採購途徑 需外加 軍售案類別
                        (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='GN') t_LAB, --//採購屬性
                        (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='C7') t_PUR_ASS_VEN_CODE --//招標方式
                    ";
                #endregion
                #region join 條件
                strSQL += $@"
                    where a.OVC_PURCH = B.OVC_PURCH
                    and a.OVC_PURCH = C.OVC_PURCH(+)
                    and a.OVC_PURCH = G.OVC_PURCH(+)
                    and a.OVC_PURCH = T.OVC_PURCH(+)
                    and a.OVC_PURCH = U.OVC_PURCH(+)
                    and a.OVC_PURCH = I.OVC_PURCH(+)
                    and a.OVC_PURCH = GPA.OVC_PURCH(+)
                    and a.OVC_PURCH = L.OVC_PURCH(+)

                    and a.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    and b.OVC_PURCHASE_UNIT = t_PURCHASE_UNIT.OVC_DEPT_CDE(+)
                    and b.OVC_CONTRACT_UNIT = t_CONTRACT_UNIT.OVC_DEPT_CDE(+) 
                    and b.OVC_PUR_APPROVE_DEP = t_PUR_APPROVE_DEP.OVC_PHR_ID(+)
                    and a.OVC_PLAN_PURCH = t_PLAN_PURCH.OVC_PHR_ID(+)
                    and a.OVC_PUR_AGENCY = t_PUR_AGENCY.OVC_PHR_ID(+)
                    and a.OVC_LAB = t_LAB.OVC_PHR_ID(+)
                    and a.OVC_PUR_ASS_VEN_CODE = t_PUR_ASS_VEN_CODE.OVC_PHR_ID(+)
                    and SUBSTR(a.OVC_PURCH, 3, 2){strYearSQL}
                    ";
                #endregion
                #region 篩選條件
                //申購單位 drpOVC_PUR_SECTION 測試OK
                string strOVC_PUR_SECTION = drpOVC_PUR_SECTION.SelectedValue;
                if (!strOVC_PUR_SECTION.Equals(string.Empty))
                {
                    DataTable dt_DEPT = FCommon.getDataTableDEPT_Includesubordinate(strOVC_PUR_SECTION);
                    string strDEPTs = "''";
                    for (int i = 0; i < dt_DEPT.Rows.Count; i++)
                    {
                        strDEPTs = i == 0 ? "" : strDEPTs + ",";
                        string strOVC_DEPT_CDE = dt_DEPT.Rows[i]["OVC_DEPT_CDE"].ToString();
                        strDEPTs += $"'{ strOVC_DEPT_CDE }'";
                    }
                    strSQL += $@"
                        and a.OVC_PUR_SECTION in ({ strDEPTs })";
                }
                //核定權責 drpOVC_PUR_APPROVE_DEP 測試OK
                string strOVC_PUR_APPROVE_DEP = drpOVC_PUR_APPROVE_DEP.SelectedValue;
                if (!strOVC_PUR_APPROVE_DEP.Equals(string.Empty))
                    strSQL += $@"
                        and b.OVC_PUR_APPROVE_DEP='{ strOVC_PUR_APPROVE_DEP }'";
                //預劃申購日期 txtOVC_DAPPLY 測試OK
                string strOVC_DAPPLY1 = txtOVC_DAPPLY1.Text;
                if (!strOVC_DAPPLY1.Equals(string.Empty))
                    strSQL += $@"
                        and b.OVC_DAPPLY >= '{ strOVC_DAPPLY1 }'
                    ";
                string strOVC_DAPPLY2 = txtOVC_DAPPLY2.Text;
                if (!strOVC_DAPPLY2.Equals(string.Empty))
                    strSQL += $@"
                        and b.OVC_DAPPLY <= '{ strOVC_DAPPLY2 }'
                    ";
                //申購單位實際申請日期 txtOVC_DPROPOSE 測試OK
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
                //預劃核定日期 txtOVC_PLAN_PUR_DAPPROVE
                //string strOVC_PLAN_PUR_DAPPROVE1 = txtOVC_PLAN_PUR_DAPPROVE1.Text;
                //if (!strOVC_PLAN_PUR_DAPPROVE1.Equals(string.Empty))
                //    strSQL += $@"
                //        and OVC_PLAN_PUR_DAPPROVE >= '{ strOVC_PLAN_PUR_DAPPROVE1 }'
                //    ";
                //string strOVC_PLAN_PUR_DAPPROVE2 = txtOVC_PLAN_PUR_DAPPROVE2.Text;
                //if (!strOVC_PLAN_PUR_DAPPROVE2.Equals(string.Empty))
                //    strSQL += $@"
                //        and OVC_PLAN_PUR_DAPPROVE <= '{ strOVC_PLAN_PUR_DAPPROVE2 }'
                //    ";
                //主官核批日 txtOVC_PUR_ALLOW 測試OK
                string strOVC_PUR_ALLOW1 = txtOVC_PUR_ALLOW1.Text;
                if (!strOVC_PUR_ALLOW1.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_PUR_ALLOW >= '{ strOVC_PUR_ALLOW1 }'
                    ";
                string strOVC_PUR_ALLOW2 = txtOVC_PUR_ALLOW2.Text;
                if (!strOVC_PUR_ALLOW2.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_PUR_ALLOW <= '{ strOVC_PUR_ALLOW2 }'
                    ";
                //核定（發文）日期 txtOVC_PUR_DAPPROVE 測試OK
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
                //計評承辦人 txtOVC_CHECKER 測試OK
                string strOVC_CHECKER = txtOVC_CHECKER.Text;
                if (!strOVC_CHECKER.Equals(string.Empty))
                {
                    strSQL += $@"
                        and c.OVC_CHECKER='{ strOVC_CHECKER }'
                    ";
                }
                //計畫性質 drpOVC_PLAN_PURCH 測試OK
                string strOVC_PLAN_PURCH = drpOVC_PLAN_PURCH.SelectedValue;
                if (!strOVC_PLAN_PURCH.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_PLAN_PURCH='{ strOVC_PLAN_PURCH }'";
                //採購途徑 drpOVC_PUR_AGENCY 測試OK
                string strOVC_PUR_AGENCY = drpOVC_PUR_AGENCY.SelectedValue;
                if (!strOVC_PUR_AGENCY.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_PUR_AGENCY='{ strOVC_PUR_AGENCY }'";
                //採購屬性 drpOVC_LAB 測試OK
                string strOVC_LAB = drpOVC_LAB.SelectedValue;
                if (!strOVC_LAB.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_LAB='{ strOVC_LAB }'";
                //招標方式 drpOVC_PUR_ASS_VEN_CODE 測試OK
                string strOVC_PUR_ASS_VEN_CODE = drpOVC_PUR_ASS_VEN_CODE.SelectedValue;
                if (!strOVC_PUR_ASS_VEN_CODE.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_PUR_ASS_VEN_CODE='{ strOVC_PUR_ASS_VEN_CODE }'";
                //是否適用GPA drpGPA 測試OK
                string strGPA = drpGPA.SelectedValue;
                if (!strGPA.Equals(string.Empty))
                {
                    if (strGPA.Equals("Y"))
                        strSQL += $@"
                            and instr(GPA.OVC_MEMO, '案內品項屬政府採購協定')>0";
                    else
                        strSQL += $@"
                            and (GPA.OVC_MEMO is null or instr(GPA.OVC_MEMO, '案內品項屬政府採購協定')=0)";
                }
                //採購發包單位 drpOVC_PURCHASE_UNIT 測試OK
                string strOVC_PURCHASE_UNIT = drpOVC_PURCHASE_UNIT.SelectedValue;
                if (!strOVC_PURCHASE_UNIT.Equals(string.Empty))
                {
                    DataTable dt_DEPT = FCommon.getDataTableDEPT_Includesubordinate(strOVC_PURCHASE_UNIT);
                    string strDEPTs = "''";
                    for (int i = 0; i < dt_DEPT.Rows.Count; i++)
                    {
                        strDEPTs = i == 0 ? "" : strDEPTs + ",";
                        string strOVC_DEPT_CDE = dt_DEPT.Rows[i]["OVC_DEPT_CDE"].ToString();
                        strDEPTs += $"'{ strOVC_DEPT_CDE }'";
                    }
                    strSQL += $@"
                        and b.OVC_PURCHASE_UNIT in ({ strDEPTs })";
                }
                //履約驗結單位 drpOVC_CONTRACT_UNIT 測試OK
                string strOVC_CONTRACT_UNIT = drpOVC_CONTRACT_UNIT.SelectedValue;
                if (!strOVC_CONTRACT_UNIT.Equals(string.Empty))
                {
                    DataTable dt_DEPT = FCommon.getDataTableDEPT_Includesubordinate(strOVC_CONTRACT_UNIT);
                    string strDEPTs = "''";
                    for (int i = 0; i < dt_DEPT.Rows.Count; i++)
                    {
                        strDEPTs = i == 0 ? "" : strDEPTs + ",";
                        string strOVC_DEPT_CDE = dt_DEPT.Rows[i]["OVC_DEPT_CDE"].ToString();
                        strDEPTs += $"'{ strOVC_DEPT_CDE }'";
                    }
                    strSQL += $@"
                        and b.OVC_CONTRACT_UNIT in ({ strDEPTs })";
                }
                //需公開閱覽 drpOVC_OPEN_CHECK 測試OK
                string strOVC_OPEN_CHECK = drpOVC_OPEN_CHECK.SelectedValue;
                if (!strOVC_OPEN_CHECK.Equals(string.Empty))
                {
                    if (strOVC_OPEN_CHECK.Equals("Y"))
                        strSQL += $@"
                            and b.OVC_OPEN_CHECK='{ strOVC_OPEN_CHECK }'";
                    else
                        strSQL += $@"
                            and (b.OVC_OPEN_CHECK='{ strOVC_OPEN_CHECK }' or b.OVC_OPEN_CHECK is null)";
                }
                //是否複數決標 drpIS_PLURAL_BASIS 測試OK
                string strIS_PLURAL_BASIS = drpIS_PLURAL_BASIS.SelectedValue;
                if (!strIS_PLURAL_BASIS.Equals(string.Empty))
                {
                    if (strIS_PLURAL_BASIS.Equals("Y"))
                        strSQL += $@"
                            and a.IS_PLURAL_BASIS='{ strIS_PLURAL_BASIS }'";
                    else
                        strSQL += $@"
                            and (a.IS_PLURAL_BASIS='{ strIS_PLURAL_BASIS }' or a.IS_PLURAL_BASIS is null)";
                }
                //是否開放式契 drpIS_OPEN_CONTRACT 測試OK
                string strIS_OPEN_CONTRACT = drpIS_OPEN_CONTRACT.SelectedValue;
                if (!strIS_OPEN_CONTRACT.Equals(string.Empty))
                {
                    if (strIS_OPEN_CONTRACT.Equals("Y"))
                        strSQL += $@"
                            and a.IS_OPEN_CONTRACT='{ strIS_OPEN_CONTRACT }'";
                    else
                        strSQL += $@"
                            and (a.IS_OPEN_CONTRACT='{ strIS_OPEN_CONTRACT }' or a.IS_OPEN_CONTRACT is null)";
                }
                //是否並列得標廠商 drpIS_JUXTAPOSED_MANUFACTURER 測試OK
                string strIS_JUXTAPOSED_MANUFACTURER = drpIS_JUXTAPOSED_MANUFACTURER.SelectedValue;
                if (!strIS_JUXTAPOSED_MANUFACTURER.Equals(string.Empty))
                {
                    if (strIS_JUXTAPOSED_MANUFACTURER.Equals("Y"))
                        strSQL += $@"
                            and a.IS_JUXTAPOSED_MANUFACTURER='{ strIS_JUXTAPOSED_MANUFACTURER }'";
                    else
                        strSQL += $@"
                            and (a.IS_JUXTAPOSED_MANUFACTURER='{ strIS_JUXTAPOSED_MANUFACTURER }' or a.IS_JUXTAPOSED_MANUFACTURER is null)";
                }
                #endregion
                #region 基本條件 & 排序
                strSQL += $@"
                    and (B.OVC_AUDIT_UNIT = { strParameterName[1] })--//計評單位=登錄者單位  
                    and (B.OVC_DCANCEL is null or B.OVC_DCANCEL = ' ')--//預劃沒有撤案
                    order by OVC_PURCH,OVC_PURCH_5,OVC_PURCH_6
                    ";
                #endregion
                #region 額外欄位查詢語法
                ////決標日期、最後決標日 chkOVC_DBID 上方已查詢，範例資料表同樣無資料，確認是否要額外查詢
                //string[] strParameterName_OVC_DBID = { ":vOVC_PURCH" };
                //ArrayList aryData_OVC_DBID = new ArrayList();
                //aryData_OVC_DBID.Add("");
                //string strSQL_OVC_DBID = $"select max(ovc_Dbid) ovc_Dbid from tbmbid_result where ovc_purch={ strParameterName_OVC_DBID[0] }";
                ////得標廠商、所有得標廠商 chkOVC_VEN_TITLE 上方已查詢，範例資料表同樣無資料，確認是否要額外查詢
                //string[] strParameterName_OVC_VEN_TITLE = { ":vOVC_PURCH" };
                //ArrayList aryData_OVC_VEN_TITLE = new ArrayList();
                //aryData_OVC_VEN_TITLE.Add("");
                //string strSQL_OVC_VEN_TITLE = $"select nvl(ovc_Ven_Title,'') ovc_Ven_Title from tbm1302 where ovc_purch={ strParameterName_OVC_DBID[0] }";
                ////簽約日期、第一個分約簽約日期 chkOVC_DCONTRACT 上方已查詢，範例資料表同樣無資料，確認是否要額外查詢
                //string strSQL_OVC_DCONTRACT = "select min(ovc_Dcontract) ovc_Dcontract from tbm1302 where ovc_purch=?";
                ////交貨批次 chkONB_DELIVERY_TIMES 上方已查詢，範例資料表同樣全0，確認是否要額外查詢
                //string strSQL = "select nvl(max(onb_Delivery_Times),1) onb_Delivery_Times from tbm1302 where ovc_purch=?";
                ////合約交貨日期、契約交貨日期 chkOVC_DELIVERY_CONTRACT 上方已查詢，範例資料表同樣無資料，確認是否要額外查詢
                //string strSQL = @"select distinct a.ovc_Delivery_Contract,a.ovc_Delivery from tbmdelivery a, 
                //    (select * from tbm1302 where  ovc_Dbid = (select max(ovc_Dbid) ovc_Dbid 
                //     from tbm1302  where ovc_purch=?) and ovc_purch= ?  ) b 
                //     where a.ovc_purch = b.ovc_purch and a.ovc_purch_6 = b.ovc_purch_6 and 
                //     a.ovc_ven_cst = b.ovc_Ven_Cst and a.ovc_purch= ? order by a.OVC_DELIVERY desc
                //    ";
                ////驗結日期 chkOVC_DINSPECT_END 上方已查詢，範例資料表同樣無資料，確認是否要額外查詢
                //string strSQL = @"select max(ovc_Dpay) ovc_Dinspect_End from TBMDELIVERY where ovc_purch=?";

                //結案日期、最後結案日期 chkOVC_DCLOSE
                string[] strParameterName_OVC_DCLOSE = { ":vOVC_PURCH" };
                ArrayList aryData_OVC_DCLOSE = new ArrayList();
                aryData_OVC_DCLOSE.Add("");
                string strSQL_OVC_DCLOSE = $@"select max(ovc_Dclose) ovc_Dclose from TBMRECEIVE_CONTRACT where ovc_purch={ strParameterName_OVC_DCLOSE[0] }";

                //合約金額、合約金額(全案) chkONB_MONEY 上方已查詢，範例資料表同樣全0，確認是否要額外查詢
                //string strSQL = "select nvl((sum(onb_Rate * onb_Money)),0.0) onb_Money from tbm1302 where ovc_purch=?";

                //決標方式片語 chkOVC_MEMO
                string[] strParameterName_OVC_MEMO = { ":vOVC_PURCH", ":vUNIT", ":vOVC_IKIND" };
                ArrayList aryData_OVC_MEMO = new ArrayList();
                aryData_OVC_MEMO.Add("");
                aryData_OVC_MEMO.Add(strDEPT_SN);
                aryData_OVC_MEMO.Add("A14");
                string strSQL_OVC_MEMO = $@"
                    select distinct a.* 
                    from TBM1202_6 a, 
                    (select max(ONB_CHECK_TIMES) ONB_CHECK_TIMES from TBM1202_6 where OVC_PURCH = { strParameterName_OVC_MEMO[0] } and OVC_IKIND = { strParameterName_OVC_MEMO[2] } and OVC_CHECK_UNIT = { strParameterName_OVC_MEMO[1] }) B
                        where a.OVC_PURCH = { strParameterName_OVC_MEMO[0] }
                        and a.OVC_IKIND = { strParameterName_OVC_MEMO[2] }
                        and a.OVC_CHECK_UNIT = { strParameterName_OVC_MEMO[1] }
                        and a.ONB_CHECK_TIMES = b.ONB_CHECK_TIMES";
                //政府採購協定片語、政府採購協定GPA
                #endregion
                #endregion

                #region Linq搜尋
                //#region 資料表Init
                //var queryTBM1118_1 =
                //    from table in mpms.TBM1118_1
                //    where table.OVC_PURCH.Substring(2, 2).Equals(strYear)
                //    select table;
                //var queryTBM1202 =
                //    from table in mpms.TBM1202
                //    where table.OVC_PURCH.Substring(2, 2).Equals(strYear)
                //    select table;
                //var queryTBM1301 =
                //    from table in mpms.TBM1301
                //    where table.OVC_PURCH.Substring(2, 2).Equals(strYear)
                //    select table;
                //var queryTBM1301_PLAN =
                //    from table in mpms.TBM1301_PLAN
                //    where table.OVC_PURCH.Substring(2, 2).Equals(strYear)
                //    select table;
                //var queryTBM1303 =
                //    from table in mpms.TBM1303
                //    where table.OVC_PURCH.Substring(2, 2).Equals(strYear)
                //    select table;
                //var queryTBMRECEIVE_CONTRACT =
                //    from table in mpms.TBMRECEIVE_CONTRACT
                //    where table.OVC_PURCH.Substring(2, 2).Equals(strYear)
                //    select table;
                //#endregion
                //#region 年份篩選
                ////if (isAllYear)
                ////{ //篩選年份選單
                ////    int intCount_Year = drpYear.Items.Count;
                ////    string[] strYears = new string[intCount_Year];
                ////    for (int i = 0; i < intCount_Year; i++)
                ////    {
                ////        ListItem theItem = drpYear.Items[i];
                ////        string strYear = theItem.Value;
                ////        if (strYear.Length > 2)
                ////            strYear = strYear.Substring(strYear.Length - 2, 2);
                ////        strYears[i] = strYear;
                ////    }
                ////    queryTBM1301 = queryTBM1301.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                ////    queryTBM1301_PLAN = queryTBM1301_PLAN.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                ////    queryTBM1118_1 = queryTBM1118_1.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                ////    queryTBM1202 = queryTBM1202.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                ////    queryTBM1220_1 = queryTBM1220_1.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                ////    queryTBM1202_6 = queryTBM1202_6.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                ////    queryTBM1302 = queryTBM1302.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                ////    queryTBM1303 = queryTBM1303.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                ////    queryTBMBID_RESULT = queryTBMBID_RESULT.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                ////    queryTBMDELIVERY = queryTBMDELIVERY.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                ////    queryTBMRECEIVE_CONTRACT = queryTBMRECEIVE_CONTRACT.Where(table => strYears.Contains(table.OVC_PURCH.Substring(2, 2)));
                ////}
                ////else
                ////{ //篩選單一年份
                ////    string strYear = drpYear.SelectedValue;
                ////    if (strYear.Length > 2)
                ////        strYear = strYear.Substring(strYear.Length - 2, 2);

                ////    queryTBM1301 = queryTBM1301.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                ////    queryTBM1301_PLAN = queryTBM1301_PLAN.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                ////    queryTBM1118_1 = queryTBM1118_1.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                ////    queryTBM1202 = queryTBM1202.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                ////    queryTBM1220_1 = queryTBM1220_1.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                ////    queryTBM1202_6 = queryTBM1202_6.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                ////    queryTBM1302 = queryTBM1302.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                ////    queryTBM1303 = queryTBM1303.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                ////    queryTBMBID_RESULT = queryTBMBID_RESULT.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                ////    queryTBMDELIVERY = queryTBMDELIVERY.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                ////    queryTBMRECEIVE_CONTRACT = queryTBMRECEIVE_CONTRACT.Where(table => table.OVC_PURCH.Substring(2, 2).Equals(strYear));
                ////}
                //#endregion

                //#region 各個資料表建置
                ////採購主檔
                //var queryA = queryTBM1301;

                ////預劃主檔
                //var queryB =
                //    from tableB in queryTBM1301_PLAN
                //    where tableB.OVC_AUDIT_UNIT.Equals(strDEPT_SN)
                //    where tableB.OVC_DCANCEL == null //預劃沒有撤案
                //    select new
                //    {
                //        tableB.OVC_PURCH,
                //        tableB.OVC_PUR_APPROVE_DEP,
                //        tableB.OVC_MILITARY_TYPE,
                //        tableB.OVC_OPEN_CHECK,
                //        tableB.OVC_DAPPLY,
                //        tableB.OVC_PURCHASE_UNIT,
                //        tableB.OVC_CONTRACT_UNIT
                //    };

                ////開標紀錄檔(取得最近一次開標日期、開標結果)
                //var queryC_B =
                //    from tableBB in queryTBM1202
                //    where tableBB.OVC_CHECK_UNIT.Equals(strDEPT_SN)
                //    join tableB in queryB on tableBB.OVC_PURCH equals tableB.OVC_PURCH
                //    group tableBB by new { tableBB.OVC_PURCH, tableBB.OVC_CHECK_UNIT, tableBB.OVC_CHECKER } into groupBB
                //    select new
                //    {
                //        groupBB.Key.OVC_PURCH,
                //        groupBB.Key.OVC_CHECK_UNIT,
                //        groupBB.Key.OVC_CHECKER,
                //        OVC_DRECEIVE = groupBB.Min(bb => bb.OVC_DRECEIVE)
                //    };
                //var queryC =
                //    from tableAA in queryTBM1202
                //    join tableBB in queryC_B
                //    on new { tableAA.OVC_PURCH, tableAA.OVC_CHECK_UNIT, tableAA.OVC_CHECKER, tableAA.OVC_DRECEIVE } equals new { tableBB.OVC_PURCH, tableBB.OVC_CHECK_UNIT, tableBB.OVC_CHECKER, tableBB.OVC_DRECEIVE }
                //    select new
                //    {
                //        tableAA.OVC_PURCH,
                //        tableAA.OVC_CHECK_UNIT,
                //        tableAA.OVC_CHECKER,
                //        tableAA.OVC_DRECEIVE,
                //        tableAA.OVC_DRECEIVE_PAPER
                //    };

                ////開標紀錄檔(決標組數)
                ////--開標紀錄檔(決標金額)
                //var queryG =
                //    from tableG in queryTBM1303
                //    where tableG.OVC_RESULT.Equals("0")
                //    join tableB in queryB on tableG.OVC_PURCH equals tableB.OVC_PURCH
                //    group tableG by tableG.OVC_PURCH into groupG
                //    select new
                //    {
                //        OVC_PURCH = groupG.Key,
                //        TTL_BID_GROUP = groupG.Count(), //決標組數
                //                                        //ONB_MONEY_NT = groupG.Sum(j => (j.ONB_BID_RESULT * (j.ONB_RESULT_RATE ?? 1)) ?? 0) //決標金額
                //    };

                ////開標紀錄檔(最大分組數)
                //var queryI =
                //    from tableI in queryTBM1303
                //    join tableB in queryB on tableI.OVC_PURCH equals tableB.OVC_PURCH
                //    group tableI by tableI.OVC_PURCH into groupI
                //    select new
                //    {
                //        OVC_PURCH = groupI.Key,
                //        TTL_GROUP = groupI.Max(g => g.ONB_GROUP)
                //    };

                //var queryT =
                //    from tableT in queryTBMRECEIVE_CONTRACT
                //    select new
                //    {
                //        tableT.OVC_PURCH,
                //        tableT.OVC_DO_NAME
                //    };
                //queryT = queryT.Distinct();

                ////核定組數
                //var queryU =
                //    from tableU in queryTBM1118_1
                //    join tableB in queryB on tableU.OVC_PURCH equals tableB.OVC_PURCH
                //    group tableU by tableU.OVC_PURCH into groupU
                //    select new
                //    {
                //        OVC_PURCH = groupU.Key,
                //        TTL_PERMI_GROUP = groupU.Count()
                //    };
                //#endregion
                //#region 部分條件篩選
                //////申購單位 a.OVC_PUR_SECTION 測試OK
                ////string strOVC_PUR_SECTION = drpOVC_PUR_SECTION.SelectedValue;
                ////if (!strOVC_PUR_SECTION.Equals(string.Empty))
                ////{
                ////    //取得單位清單－含下屬單位
                ////    DataTable dt_DEPT = FCommon.getDataTableDEPT_Includesubordinate(strOVC_PUR_SECTION);
                ////    int intCount = dt_DEPT.Rows.Count;
                ////    if (intCount > 0)
                ////    {
                ////        string[] strDEPTs = new string[intCount];
                ////        for (int i = 0; i < intCount; i++)
                ////        {
                ////            strDEPTs[i] = dt_DEPT.Rows[i]["OVC_DEPT_CDE"].ToString();
                ////        }
                ////        queryA = queryA.Where(table => strDEPTs.Contains(table.OVC_PUR_SECTION));
                ////    }
                ////}
                //////核定權責 b.OVC_PUR_APPROVE_DEP 測試OK
                ////string strOVC_PUR_APPROVE_DEP = drpOVC_PUR_APPROVE_DEP.SelectedValue;
                ////if (!strOVC_PUR_APPROVE_DEP.Equals(string.Empty))
                ////    queryB = queryB.Where(table => table.OVC_PUR_APPROVE_DEP.Equals(strOVC_PUR_APPROVE_DEP));
                //////預劃申購日期 b.OVC_DAPPLY 測試OK
                ////string strOVC_DAPPLY1 = txtOVC_DAPPLY1.Text;
                ////if (!strOVC_DAPPLY1.Equals(string.Empty))
                ////    queryB = queryB.Where(table => table.OVC_DAPPLY.CompareTo(strOVC_DAPPLY1) >= 0);
                ////string strOVC_DAPPLY2 = txtOVC_DAPPLY2.Text;
                ////if (!strOVC_DAPPLY2.Equals(string.Empty))
                ////    queryB = queryB.Where(table => table.OVC_DAPPLY.CompareTo(strOVC_DAPPLY2) <= 0);
                //////申購單位實際申請日期 tableA.OVC_DPROPOSE 測試OK
                ////string strOVC_DPROPOSE1 = txtOVC_DPROPOSE1.Text;
                ////if (!strOVC_DPROPOSE1.Equals(string.Empty))
                ////    queryA = queryA.Where(table => table.OVC_DPROPOSE.CompareTo(strOVC_DPROPOSE1) >= 0);
                ////string strOVC_DPROPOSE2 = txtOVC_DPROPOSE2.Text;
                ////if (!strOVC_DPROPOSE2.Equals(string.Empty))
                ////    queryA = queryA.Where(table => table.OVC_DPROPOSE.CompareTo(strOVC_DPROPOSE2) <= 0);
                //////主官核批日 tableA.OVC_PUR_ALLOW 測試OK
                ////string strOVC_PUR_ALLOW1 = txtOVC_PUR_ALLOW1.Text;
                ////if (!strOVC_PUR_ALLOW1.Equals(string.Empty))
                ////    queryA = queryA.Where(table => table.OVC_PUR_ALLOW.CompareTo(strOVC_PUR_ALLOW1) >= 0);
                ////string strOVC_PUR_ALLOW2 = txtOVC_PUR_ALLOW2.Text;
                ////if (!strOVC_PUR_ALLOW2.Equals(string.Empty))
                ////    queryA = queryA.Where(table => table.OVC_PUR_ALLOW.CompareTo(strOVC_PUR_ALLOW2) <= 0);
                //////核定（發文）日期 tableA.OVC_PUR_DAPPROVE 測試OK
                ////string strOVC_PUR_DAPPROVE1 = txtOVC_PUR_DAPPROVE1.Text;
                ////if (!strOVC_PUR_DAPPROVE1.Equals(string.Empty))
                ////    queryA = queryA.Where(table => table.OVC_PUR_DAPPROVE.CompareTo(strOVC_PUR_DAPPROVE1) >= 0);
                ////string strOVC_PUR_DAPPROVE2 = txtOVC_PUR_DAPPROVE2.Text;
                ////if (!strOVC_PUR_DAPPROVE2.Equals(string.Empty))
                ////    queryA = queryA.Where(table => table.OVC_PUR_DAPPROVE.CompareTo(strOVC_PUR_DAPPROVE2) <= 0);
                //////計畫性質 tableA.OVC_PLAN_PURCH 測試OK
                ////string strOVC_PLAN_PURCH = drpOVC_PLAN_PURCH.SelectedValue;
                ////if (!strOVC_PLAN_PURCH.Equals(string.Empty))
                ////    queryA = queryA.Where(table => table.OVC_PLAN_PURCH.Equals(strOVC_PLAN_PURCH));
                //////採購途徑 tableA.OVC_PUR_AGENCY 測試OK
                ////string strOVC_PUR_AGENCY = drpOVC_PUR_AGENCY.SelectedValue;
                ////if (!strOVC_PUR_AGENCY.Equals(string.Empty))
                ////    queryA = queryA.Where(table => table.OVC_PUR_AGENCY.Equals(strOVC_PUR_AGENCY));
                //////採購屬性 tableA.OVC_LAB 測試OK
                ////string strOVC_LAB = drpOVC_LAB.SelectedValue;
                ////if (!strOVC_LAB.Equals(string.Empty))
                ////    queryA = queryA.Where(table => table.OVC_LAB.Equals(strOVC_LAB));
                //////招標方式 tableA.OVC_PUR_ASS_VEN_CODE 測試OK
                ////string strOVC_PUR_ASS_VEN_CODE = drpOVC_PUR_ASS_VEN_CODE.SelectedValue;
                ////if (!strOVC_PUR_ASS_VEN_CODE.Equals(string.Empty))
                ////    queryA = queryA.Where(table => table.OVC_PUR_ASS_VEN_CODE.Equals(strOVC_PUR_ASS_VEN_CODE));
                //////採購發包單位 tableB.OVC_PURCHASE_UNIT 測試OK
                ////string strOVC_PURCHASE_UNIT = drpOVC_PURCHASE_UNIT.SelectedValue;
                ////if (!strOVC_PURCHASE_UNIT.Equals(string.Empty))
                ////{
                ////    //取得單位清單－含下屬單位
                ////    DataTable dt_DEPT = FCommon.getDataTableDEPT_Includesubordinate(strOVC_PURCHASE_UNIT);
                ////    int intCount = dt_DEPT.Rows.Count;
                ////    if (intCount > 0)
                ////    {
                ////        string[] strDEPTs = new string[intCount];
                ////        for (int i = 0; i < intCount; i++)
                ////        {
                ////            strDEPTs[i] = dt_DEPT.Rows[i]["OVC_DEPT_CDE"].ToString();
                ////        }
                ////        queryB = queryB.Where(table => strDEPTs.Contains(table.OVC_PURCHASE_UNIT));
                ////    }
                ////}
                //////履約驗結單位 tableB.OVC_CONTRACT_UNIT 測試OK
                ////string strOVC_CONTRACT_UNIT = drpOVC_CONTRACT_UNIT.SelectedValue;
                ////if (!strOVC_CONTRACT_UNIT.Equals(string.Empty))
                ////{
                ////    //取得單位清單－含下屬單位
                ////    DataTable dt_DEPT = FCommon.getDataTableDEPT_Includesubordinate(strOVC_CONTRACT_UNIT);
                ////    int intCount = dt_DEPT.Rows.Count;
                ////    if (intCount > 0)
                ////    {
                ////        string[] strDEPTs = new string[intCount];
                ////        for (int i = 0; i < intCount; i++)
                ////        {
                ////            strDEPTs[i] = dt_DEPT.Rows[i]["OVC_DEPT_CDE"].ToString();
                ////        }
                ////        queryB = queryB.Where(table => strDEPTs.Contains(table.OVC_CONTRACT_UNIT));
                ////    }
                ////}
                //////需公開閱覽 tableB.OVC_OPEN_CHECK 測試OK
                ////string strOVC_OPEN_CHECK = drpOVC_OPEN_CHECK.SelectedValue;
                ////if (!strOVC_OPEN_CHECK.Equals(string.Empty))
                ////    queryB = queryB.Where(table => table.OVC_OPEN_CHECK.Equals(strOVC_OPEN_CHECK) || (strOVC_OPEN_CHECK.Equals("N") && table.OVC_OPEN_CHECK == null));
                //////是否複數決標 tableA.IS_PLURAL_BASIS 測試OK
                ////string strIS_PLURAL_BASIS = drpIS_PLURAL_BASIS.SelectedValue;
                ////if (!strIS_PLURAL_BASIS.Equals(string.Empty))
                ////    queryA = queryA.Where(table => table.IS_PLURAL_BASIS.Equals(strIS_PLURAL_BASIS));
                //////是否開放式契 tableA.IS_OPEN_CONTRACT 測試OK
                ////string strIS_OPEN_CONTRACT = drpIS_OPEN_CONTRACT.SelectedValue;
                ////if (!strIS_OPEN_CONTRACT.Equals(string.Empty))
                ////    queryA = queryA.Where(table => table.IS_OPEN_CONTRACT.Equals(strIS_OPEN_CONTRACT));
                //////是否並列得標廠商 tableA.IS_JUXTAPOSED_MANUFACTURER 測試OK
                ////string strIS_JUXTAPOSED_MANUFACTURER = drpIS_JUXTAPOSED_MANUFACTURER.SelectedValue;
                ////if (!strIS_JUXTAPOSED_MANUFACTURER.Equals(string.Empty))
                ////    queryA = queryA.Where(table => table.IS_JUXTAPOSED_MANUFACTURER.Equals(strIS_JUXTAPOSED_MANUFACTURER));
                //#endregion
                //#region 資料表Join
                //DateTime dateTemp;
                //var query =
                //    from tableA in queryA.AsEnumerable()
                //    join tableB in queryB.AsEnumerable() on tableA.OVC_PURCH equals tableB.OVC_PURCH
                //    join tableC in queryC.AsEnumerable() on tableA.OVC_PURCH equals tableC.OVC_PURCH into tempC
                //    from tableC in tempC.DefaultIfEmpty()
                //    join tableG in queryG.AsEnumerable() on tableA.OVC_PURCH equals tableG.OVC_PURCH into tempG
                //    from tableG in tempG.DefaultIfEmpty()
                //    join tableT in queryT.AsEnumerable() on tableA.OVC_PURCH equals tableT.OVC_PURCH into tempT
                //    from tableT in tempT.DefaultIfEmpty()
                //    join tableU in queryU.AsEnumerable() on tableA.OVC_PURCH equals tableU.OVC_PURCH into tempU
                //    from tableU in tempU.DefaultIfEmpty()
                //    join tableI in queryI.AsEnumerable() on tableA.OVC_PURCH equals tableI.OVC_PURCH into tempI
                //    from tableI in tempI.DefaultIfEmpty()
                //        //where tableA.OVC_PUR_ALLOW == null || DateTime.TryParse(tableA.OVC_PUR_ALLOW, out dateTemp)
                //        //where tableC == null || tableC.OVC_DRECEIVE_PAPER == null || DateTime.TryParse(tableC.OVC_DRECEIVE_PAPER, out dateTemp)

                //    select new
                //    {
                //        tableA.OVC_PURCH,
                //        //FGETCONTRACTDESC(a.OVC_PURCH,'06','OVC_PURCH_5') OVC_PURCH_5,
                //        //FGETCONTRACTDESC(a.OVC_PURCH,'06','OVC_PURCH_6') OVC_PURCH_6,
                //        //tableA.OVC_PUR_AGENCY,

                //        //a.OVC_PURCH||a.OVC_PUR_AGENCY||FGETCONTRACTDESC(a.OVC_PURCH,'06','OVC_PURCH_5') ||FGETCONTRACTDESC(a.OVC_PURCH,'06','OVC_PURCH_6') purch,--//案號只到第五組
                //        tableA.OVC_PUR_NSECTION, //申購單位
                //        tableA.OVC_PUR_IPURCH, //購案名稱
                //        tableB.OVC_PUR_APPROVE_DEP, //核定權責
                //        tableA.OVC_PLAN_PURCH, //計畫性質代碼
                //        tableA.OVC_PUR_AGENCY, //採購單位地區代碼(採購途徑)
                //        tableB.OVC_MILITARY_TYPE, //軍售案類別(1->個別式2->開放式)
                //        tableA.OVC_LAB, //採購屬性代碼
                //        OVC_PUR_ASS_VEN_CODE = tableA.OVC_PUR_ASS_VEN_CODE ?? "", //招標方式代碼
                //        OVC_OPEN_CHECK = tableB.OVC_OPEN_CHECK ?? "N", //是否公開閱覽(Y/N)
                //        ONB_PUR_BUDGET_NT = tableA.ONB_PUR_BUDGET_NT ?? 0, //總預算金額(台幣)
                //                                                           //FGETCONTRACTDESC(a.OVC_PURCH,'06+"','onb_Bud_Money')*FGETCONTRACTDESC(a.OVC_PURCH,'06+"','onb_Bud_Rate') onb_Bud_Money_Nt, // 分組預算金額(台幣)
                //        tableB.OVC_DAPPLY, //預劃申購日期
                //        tableA.OVC_DPROPOSE, //申購單位實際申請日期
                //        tableA.IS_PLURAL_BASIS, //是否複數決標(Y/N)
                //        tableA.IS_OPEN_CONTRACT, //是否開放式契(Y/N)
                //        tableA.IS_JUXTAPOSED_MANUFACTURER, //是否並列得標廠商(Y/N)
                //                                           //(to_char((to_date(nvl(b.ovc_Dapply,''),'YYYY-MM-DD') + nvl(b.onb_Review_days,0)),'YYYY-MM-DD')) ovc_Plan_Pur_Dapprove, //預劃核定日期
                //        OVC_PUR_DAPPROVE = tableA.OVC_PUR_DAPPROVE ?? "", //核定發文日期
                //        OVC_PUR_APPROVE = tableA.OVC_PUR_APPROVE ?? "", //核定文號
                //        OVC_PUR_ALLOW = tableA.OVC_PUR_ALLOW ?? "", //主官核批日
                //        OVC_PUR_DCANPO = tableA.OVC_PUR_DCANPO ?? "", //撤案日
                //        tableB.OVC_PURCHASE_UNIT, //採購發包單位代碼
                //        tableB.OVC_CONTRACT_UNIT, //履約驗結單位代碼
                //        OVC_CHECKER = tableC != null ? tableC.OVC_CHECKER : null, //計評承辦人
                //        OVC_DRECEIVE = tableC != null ? tableC.OVC_DRECEIVE : null, //計評實際收辦日期(第1次分派日)
                //        OVC_DRECEIVE_PAPER = tableC != null ? tableC.OVC_DRECEIVE_PAPER : null, //紙本收文日
                //        TTL_BID_GROUP = tableG != null ? tableG.TTL_BID_GROUP : 0, //已決標組數
                //        ObjectResult = mpms.FGETCONTRACTDESC("AA03014", "03", "OVC_PURCH_5"),
                //                                                                   //FGETCONTRACTDESC(a.OVC_PURCH,'06+"','ovc_result') OVC_RESULT, //最近一次開標結果(代碼A8)
                //                                                                   //FGETCONTRACTDESC(a.OVC_PURCH,'06+"','ovc_dopen') OVC_DOPEN, //開標日期
                //                                                                   //FGETCONTRACTDESC(a.OVC_PURCH,'06+"','ONB_GROUP') ONB_GROUP, //組別
                //                                                                   //FGETCONTRACTDESC(a.OVC_PURCH,'06+"','ONB_BID_RESULT')*FGETCONTRACTDESC(a.OVC_PURCH,'05','ONB_RESULT_RATE') onb_Money_NT, // 決標金額(台幣)
                //                                                                   //FGETCONTRACTDESC(a.OVC_PURCH,'06+"','ovc_Dbid') ovc_Dbid, //決標日
                //                                                                   //FGETCONTRACTDESC(a.OVC_PURCH,'06+"','ovc_Ven_Title') ovc_Ven_Title, //得標商名稱
                //                                                                   //FGETCONTRACTDESC(a.OVC_PURCH,'06+"','ovc_Dcontract') ovc_Dcontract, //簽約日
                //                                                                   //FGETCONTRACTDESC(a.OVC_PURCH,'06+"','onb_Delivery_Times') onb_Delivery_Times, //交貨批次
                //                                                                   //FGETCONTRACTDESC(a.OVC_PURCH,'06+"','onb_Money')*FGETCONTRACTDESC(a.OVC_PURCH,'05','onb_Rate') onb_Money, //合約金額
                //                                                                   //FGETCONTRACTDESC(a.OVC_PURCH,'06+"','ovc_Delivery') ovc_Delivery, //實際交貨日期
                //                                                                   //FGETCONTRACTDESC(a.OVC_PURCH,'06+"','ovc_Delivery_Contract') ovc_Delivery_Contract, //契約交貨日期
                //                                                                   //FGETCONTRACTDESC(a.OVC_PURCH,'06+"','ovc_Dpay') ovc_Dinspect_End, //結報日期(驗結日期)
                //        TTL_GROUP = tableI != null ? tableI.TTL_GROUP : 0, //總組數
                //        TTL_PERMI_GROUP = tableU != null ? tableU.TTL_PERMI_GROUP : 0, //總組數
                //                                                                       //FGETCONTRACTDESC(a.OVC_PURCH,'06+"','OVC_NAME') OVC_NAME, //採包承辦人
                //        OVC_DO_NAME = tableT != null ? tableT.OVC_DO_NAME : null, //履驗承辦人
                //                                                                  //case when nvl(a.ovc_pur_allow,' ') = ' ' or nvl(c.ovc_dreceive_paper,' ') = ' ' then '0' else to_char(to_date(a.ovc_pur_allow,'YYYY-MM-DD') - to_date(c.ovc_dreceive_paper,'YYYY-MM-DD')) end audittotalday //評核總天數
                //    };
                //#endregion
                //#region 部分條件篩選
                //////預劃核定日期 測試OK
                ////string strOVC_PLAN_PUR_DAPPROVE1 = txtOVC_PLAN_PUR_DAPPROVE1.Text;
                ////if (!strOVC_PLAN_PUR_DAPPROVE1.Equals(string.Empty))
                ////    query = query.Where(table => table.OVC_PLAN_PUR_DAPPROVE.CompareTo(strOVC_PLAN_PUR_DAPPROVE1) >= 0);
                ////string strOVC_PLAN_PUR_DAPPROVE2 = txtOVC_PLAN_PUR_DAPPROVE2.Text;
                ////if (!strOVC_PLAN_PUR_DAPPROVE2.Equals(string.Empty))
                ////    query = query.Where(table => table.OVC_PLAN_PUR_DAPPROVE.CompareTo(strOVC_PLAN_PUR_DAPPROVE2) <= 0);
                //////計評承辦人 tableC.OVC_CHECKER 測試OK
                ////string strOVC_CHECKER = txtOVC_CHECKER.Text;
                ////if (!strOVC_CHECKER.Equals(string.Empty))
                ////    query = query.Where(table => table.OVC_CHECKER.Equals(strOVC_CHECKER));
                //////是否適用GPA 測試OK
                ////if (!drpGPA.SelectedValue.Equals(string.Empty))
                ////    query = query.Where(table => table.GPA.Equals(drpGPA.SelectedItem.Text));
                //#endregion
                ////var queryOrder = query.OrderBy(table => table.OVC_PURCH_5).OrderBy(table => table.OVC_PURCH);
                #endregion

                //DataTable dt_Source = CommonStatic.LinqQueryToDataTable(query);
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strSQL);
                DataTable dt_Source = FCommon.getDataTableFromSelect(strSQL, strParameterName, aryData);
                int intCount_Data;
                //將缺少SQL欄位補足
                foreach (string strFieldSql in strFieldSqls)
                {
                    if (!dt_Source.Columns.Contains(strFieldSql))
                        dt_Source.Columns.Add(strFieldSql);
                }
                //int intMaxLength = dt_Source.Columns["OVC_PUR_AGENCY"].MaxLength;
                //FCommon.AlertShow(PnMessage, "danger", "系統訊息", dt_Source.Columns["OVC_PUR_AGENCY"].MaxLength.ToString());
                foreach (DataRow dr in dt_Source.Rows)
                {
                    bool isShow = true; //判斷額外篩選條件
                                        //預劃核定日期 txtOVC_PLAN_PUR_DAPPROVE
                    string strOVC_PLAN_PUR_DAPPROVE = dr["OVC_PLAN_PUR_DAPPROVE"].ToString();
                    string strOVC_PLAN_PUR_DAPPROVE1 = txtOVC_PLAN_PUR_DAPPROVE1.Text;
                    string strOVC_PLAN_PUR_DAPPROVE2 = txtOVC_PLAN_PUR_DAPPROVE2.Text;
                    DateTime dateOVC_PLAN_PUR_DAPPROVE;
                    if (DateTime.TryParse(strOVC_PLAN_PUR_DAPPROVE, out dateOVC_PLAN_PUR_DAPPROVE))
                    {
                        if (!strOVC_PLAN_PUR_DAPPROVE1.Equals(string.Empty))
                        {
                            DateTime date1 = DateTime.Parse(strOVC_PLAN_PUR_DAPPROVE1);
                            isShow = isShow && DateTime.Compare(date1, dateOVC_PLAN_PUR_DAPPROVE) <= 0;
                        }
                        if (!strOVC_PLAN_PUR_DAPPROVE2.Equals(string.Empty))
                        {
                            DateTime date2 = DateTime.Parse(strOVC_PLAN_PUR_DAPPROVE2);
                            isShow = isShow && DateTime.Compare(dateOVC_PLAN_PUR_DAPPROVE, date2) <= 0;
                        }
                    }
                    else if (!strOVC_PLAN_PUR_DAPPROVE1.Equals(string.Empty) || !strOVC_PLAN_PUR_DAPPROVE2.Equals(string.Empty))
                        isShow = false;
                    if (!isShow)
                        dr.Delete();
                    else
                    {
                        string strOVC_PURCH = dr["OVC_PURCH"].ToString();
                        aryData_OVC_DCLOSE[0] = strOVC_PURCH;
                        aryData_OVC_MEMO[0] = strOVC_PURCH;

                        //string strOVC_PUR_AGENCY = dr["OVC_PUR_AGENCY"].ToString() + dr["OVC_MILITARY_TYPE"].ToString();
                        //if (strOVC_PUR_AGENCY.Length > intMaxLength)
                        //    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strOVC_PURCH +"："+ strOVC_PUR_AGENCY);
                        //else
                        //    dr["OVC_PUR_AGENCY"] = strOVC_PUR_AGENCY;
                        dr["OVC_PUR_AGENCY"] = dr["OVC_PUR_AGENCY"].ToString() + dr["OVC_MILITARY_TYPE"].ToString();

                        if (chkOVC_DCLOSE.Checked) //結案日期、最後結案日期 chkOVC_DCLOSE
                        {
                            DataTable dt_OVC_DCLOSE = FCommon.getDataTableFromSelect(strSQL_OVC_DCLOSE, strParameterName_OVC_DCLOSE, aryData_OVC_DCLOSE);
                            if (dt_OVC_DCLOSE.Rows.Count > 0)
                                dr["OVC_DCLOSE"] = dt_OVC_DCLOSE.Rows[0]["OVC_DCLOSE"];
                        }
                        if (chkOVC_MEMO.Checked) //決標方式片語 chkOVC_MEMO
                        {
                            DataTable dt_OVC_MEMO = FCommon.getDataTableFromSelect(strSQL_OVC_MEMO, strParameterName_OVC_MEMO, aryData_OVC_MEMO);
                            if (dt_OVC_MEMO.Rows.Count > 0)
                                dr["OVC_MEMO"] = dt_OVC_MEMO.Rows[0]["OVC_MEMO"];
                        }
                    }
                }
                dt_Source.AcceptChanges(); //將刪除列儲存，完成刪除動作

                DataTable dt = getDataTable_Export(dt_Source, out intCount_Data);

                if (intCount_Data > 0)// && false)
                {
                    string strSheetText = "1_採購";
                    string strTitleText = $"{ strYearText }年度{ strDEPT_Name }購案管制表";
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
        protected void btnQuery_OVC_Click(object sender, EventArgs e)
        {
            if (strDEPT_SN != null)
            {
                string[] strParameterName = { ":qry_YY", ":USER_DEPT_SN" };
                ArrayList aryData = new ArrayList();

                #region 年份篩選
                //string strYear = "06";
                string strYearSQL, strYear, strYearText = "";
                bool isAllYear = chkYear.Checked;
                if (isAllYear)
                { //篩選年份選單
                    int intCount_Year = drpYear.Items.Count;
                    //string[] strYears = new string[intCount_Year];
                    string strYearList = "";
                    for (int i = 0; i < intCount_Year; i++)
                    {
                        ListItem theItem = drpYear.Items[i];
                        strYear = theItem.Value;
                        if (strYear.Length > 2)
                            strYear = strYear.Substring(strYear.Length - 2, 2);
                        //strYears[i] = strYear;
                        if (i != 0) strYearList += ", ";
                        strYearList += "'" + strYear + "'";
                    }

                    //string strYearList = string.Join(",", strYears);
                    strYearSQL = $" in ({ strYearList }, { strParameterName[0] })";
                    strYearText = getAllYearText(); //取得年度字串
                }
                else
                { //篩選單一年份
                    strYearSQL = $" = { strParameterName[0] }";
                }
                strYear = drpYear.SelectedValue;
                if (strYear.Length > 2)
                    strYear = strYear.Substring(strYear.Length - 2, 2);
                if (!isAllYear) strYearText = strYear; //取得年度字串
                aryData.Add(strYear);
                aryData.Add(strDEPT_SN);
                #endregion

                #region SQL語法
                string strSQL = "";
                #region 欄位
                strSQL += $@"
                    select 
                    a.OVC_PURCH,
                    FGETCONTRACTDESC_1(a.OVC_PURCH,'05','OVC_PURCH_5') OVC_PURCH_5,
                    FGETCONTRACTDESC_1(a.OVC_PURCH,'05','OVC_PURCH_6') OVC_PURCH_6,
                    a.OVC_PURCH||a.OVC_PUR_AGENCY||FGETCONTRACTDESC_1(a.OVC_PURCH,'05','OVC_PURCH_5')||FGETCONTRACTDESC_1(a.OVC_PURCH,'05','OVC_PURCH_6') PURCH, --//案號只到第五組
                    t_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --a.OVC_PUR_NSECTION, --//申購單位
                    a.OVC_PUR_IPURCH, --//購案名稱
                    t_PUR_APPROVE_DEP.OVC_PHR_DESC OVC_PUR_APPROVE_DEP, --b.OVC_PUR_APPROVE_DEP, --//核定權責
                    t_PLAN_PURCH.OVC_PHR_DESC OVC_PLAN_PURCH, --a.OVC_PLAN_PURCH, --//計畫性質代碼
                    case when (a.OVC_PUR_AGENCY<>'M') then T_PUR_AGENCY.OVC_PHR_DESC else (T_PUR_AGENCY.OVC_PHR_DESC) end OVC_PUR_AGENCY, --//採購單位地區代碼(採購途徑)
                    case when B.OVC_MILITARY_TYPE='1' then '(個別式)' else (case when B.OVC_MILITARY_TYPE='2' then '(開放式)' else '' end) end OVC_MILITARY_TYPE, --//軍售案類別(1->個別式2->開放式)
                    --t_PUR_AGENCY.OVC_PHR_DESC OVC_PUR_AGENCY, --a.OVC_PUR_AGENCY, --//採購單位地區代碼(採購途徑)
                    --B.OVC_MILITARY_TYPE, --//軍售案類別(1->個別式2->開放式)
                    t_LAB.OVC_PHR_DESC OVC_LAB, --a.OVC_LAB, --//採購屬性代碼
                    t_PUR_ASS_VEN_CODE.OVC_PHR_DESC OVC_PUR_ASS_VEN_CODE, --NVL(a.OVC_PUR_ASS_VEN_CODE,' ') OVC_PUR_ASS_VEN_CODE, --//招標方式代碼
                    NVL(b.OVC_OPEN_CHECK,'N') OVC_OPEN_CHECK, --//是否公開閱覽(Y/N)
                    NVL(a.ONB_PUR_BUDGET_NT,0.0) ONB_PUR_BUDGET_NT, --//總預算金額(台幣)
                    FGETCONTRACTDESC_1(a.OVC_PURCH,'05','onb_BUD_Money') * FGETCONTRACTDESC_1(a.OVC_PURCH,'05','onb_Bud_Rate') ONB_BUD_MONEY_NT, --// 分組預算金額(台幣)
                    b.OVC_DAPPLY, --//預劃申購日期
                    a.OVC_DPROPOSE, --//申購單位實際申請日期
                    case when a.IS_PLURAL_BASIS is null then '' else case when a.IS_PLURAL_BASIS='Y' then '是' else '否' end end IS_PLURAL_BASIS, --//是否複數決標(Y/N)
                    case when a.IS_OPEN_CONTRACT='Y' then '是' else '否' end IS_OPEN_CONTRACT, --//是否開放式契(Y/N)
                    case when a.IS_JUXTAPOSED_MANUFACTURER='Y' then '是' else '否' end IS_JUXTAPOSED_MANUFACTURER, --//是否並列得標廠商(Y/N)
                    (TO_CHAR((TO_DATE(NVL(b.OVC_DAPPLY,''),'YYYY-MM-DD') + NVL(b.ONB_REVIEW_DAYS,0)),'YYYY-MM-DD')) OVC_PLAN_PUR_DAPPROVE, --//預劃核定日期
                    NVL(a.OVC_PUR_DAPPROVE,' ') OVC_PUR_DAPPROVE, --//核定發文日期
                    NVL(a.OVC_PUR_APPROVE,' ') OVC_PUR_APPROVE, --//核定文號
                    NVL(a.OVC_PUR_ALLOW,' ') OVC_PUR_ALLOW, --//主官核批日
                    --NVL(a.OVC_PUR_DCANPO, ' ') OVC_PUR_DCANPO, --//撤案日
                    case when a.OVC_PUR_DCANPO is null then '否' else '是' end OVC_PUR_DCANPO_YN, --//是否撤案
                    t_PURCHASE_UNIT.OVC_ONNAME OVC_PURCHASE_UNIT, --b.OVC_PURCHASE_UNIT, --//採購發包單位代碼
                    t_CONTRACT_UNIT.OVC_ONNAME OVC_CONTRACT_UNIT, --b.OVC_CONTRACT_UNIT, --//履約驗結單位代碼
                    c.OVC_CHECKER, --//計評承辦人
                    c.OVC_DRECEIVE, --//計評實際收辦日期(第1次分派日)
                    c.OVC_DRECEIVE_PAPER,
                    NVL(g.TTL_BID_GROUP,0) TTL_BID_GROUP, --//已決標組數
                    FGETCONTRACTDESC_1(a.OVC_PURCH,'05','OVC_RESULT') OVC_RESULT, --//最近一次開標結果(代碼A8)
                    FGETCONTRACTDESC_1(a.OVC_PURCH,'05','OVC_DOPEN') OVC_DOPEN, --//開標日期
                    FGETCONTRACTDESC_1(a.OVC_PURCH,'05','ONB_GROUP') ONB_GROUP, --//組別
                    NVL(FGETCONTRACTDESC_1(a.OVC_PURCH,'05','ONB_BID_RESULT') * FGETCONTRACTDESC_1(a.OVC_PURCH,'05','ONB_RESULT_RATE'),0) ONB_MONEY_NT, --// 決標金額(台幣)
                    FGETCONTRACTDESC_1(a.OVC_PURCH,'05','ovc_Dbid') OVC_DBID, --//決標日
                    FGETCONTRACTDESC_1(a.OVC_PURCH,'05','ovc_Ven_Title') OVC_VEN_TITLE, --//得標商名稱
                    FGETCONTRACTDESC_1(a.OVC_PURCH,'05','ovc_Dcontract') OVC_DCONTRACT, --//簽約日
                    NVL(FGETCONTRACTDESC_1(a.OVC_PURCH,'05','onb_Money') * FGETCONTRACTDESC_1(a.OVC_PURCH,'05','onb_Rate'),0) ONB_MONEY, --//合約金額
                    FGETCONTRACTDESC_1(a.OVC_PURCH,'05','ovc_Delivery') OVC_DELIVERY, --//實際交貨日期
                    NVL(FGETCONTRACTDESC_1(a.OVC_PURCH,'05','onb_Ship_Times'),0) ONB_DELIVERY_TIMES, --//交貨批次
                    FGETCONTRACTDESC_1(a.OVC_PURCH,'05','ovc_Delivery_Contract') OVC_DELIVERY_CONTRACT, --//契約交貨日期
                    FGETCONTRACTDESC_1(a.OVC_PURCH,'05','ovc_Dpay') OVC_DINSPECT_END, --//結報日期(驗結日期)
                    NVL(i.TTL_GROUP,0) TTL_GROUP, --//總組數
                    case when NVL(U.TTL_PERMI_GROUP, 0)<>0 then NVL(U.TTL_PERMI_GROUP, 0) else case when NVL(I.TTL_GROUP, 0)=0 then 1 else NVL(I.TTL_GROUP, 0) end end TTL_PERMI_GROUP, --//核定分組數
                    FGETCONTRACTDESC_1(a.OVC_PURCH,'05','OVC_NAME') OVC_NAME, --//採包承辦人
                    t.OVC_DO_NAME,  --//履驗承辦人
                    case when NVL(a.OVC_PUR_ALLOW,' ') = ' ' or NVL(c.OVC_DRECEIVE_PAPER,' ') = ' ' then '0' else TO_CHAR(TO_DATE(a.OVC_PUR_ALLOW,'YYYY-MM-DD') - TO_DATE(c.OVC_DRECEIVE_PAPER,'YYYY-MM-DD')) end AUDITTOTALDAY, --//評核總天數
                    case when instr(GPA.OVC_MEMO, '案內品項屬政府採購協定')>0 then '是' else '否' end GPA, --//是否適用GPA
                    GPA.OVC_MEMO GPA_TYPE, --//政府採購協定片語
                    case when L.vCase>0 then 'Y' else 'N' end OVC_ADVENTAGED_CHECK--//準用最有利標
                    ";

                //strSQL += $@"
                //    a.OVC_PURCH,
                //    FGETCONTRACTDESC(a.OVC_PURCH,SUBSTR(a.OVC_PURCH,3,2),'OVC_PURCH_5') OVC_PURCH_5,
                //    FGETCONTRACTDESC(a.OVC_PURCH,SUBSTR(a.OVC_PURCH,3,2),'OVC_PURCH_6') OVC_PURCH_6,
                //    --a.OVC_PUR_AGENCY,
                //    a.OVC_PURCH||a.OVC_PUR_AGENCY||FGETCONTRACTDESC(a.OVC_PURCH,SUBSTR(a.OVC_PURCH,3,2),'OVC_PURCH_5') ||FGETCONTRACTDESC(a.OVC_PURCH,SUBSTR(a.OVC_PURCH,3,2),'OVC_PURCH_6') PURCH,--//案號只到第五組
                //    t_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --a.OVC_PUR_NSECTION, --//申購單位
                //    a.OVC_PUR_IPURCH, --//購案名稱
                //    t_PUR_APPROVE_DEP.OVC_PHR_DESC OVC_PUR_APPROVE_DEP, --B.OVC_PUR_APPROVE_DEP,--//核定權責
                //    t_PLAN_PURCH.OVC_PHR_DESC OVC_PLAN_PURCH, --a.OVC_PLAN_PURCH,--//計畫性質代碼
                //    case when (a.OVC_PUR_AGENCY<>'M') then T_PUR_AGENCY.OVC_PHR_DESC else (T_PUR_AGENCY.OVC_PHR_DESC) end OVC_PUR_AGENCY, --//採購單位地區代碼(採購途徑)
                //    case when B.OVC_MILITARY_TYPE='1' then '(個別式)' else (case when B.OVC_MILITARY_TYPE='2' then '(開放式)' else '' end) end OVC_MILITARY_TYPE, --//軍售案類別(1->個別式2->開放式)
                //    --t_PUR_AGENCY.OVC_PHR_DESC OVC_PUR_AGENCY, --a.OVC_PUR_AGENCY,--//採購單位地區代碼(採購途徑)
                //    --B.OVC_MILITARY_TYPE, --//軍售案類別(1->個別式2->開放式)
                //    t_LAB.OVC_PHR_DESC OVC_LAB, --a.OVC_LAB,--//採購屬性代碼
                //    t_PUR_ASS_VEN_CODE.OVC_PHR_DESC OVC_PUR_ASS_VEN_CODE, --NVL(a.OVC_PUR_ASS_VEN_CODE,' ') OVC_PUR_ASS_VEN_CODE,--//招標方式代碼
                //    NVL(B.OVC_OPEN_CHECK,'N') OVC_OPEN_CHECK,--//是否公開閱覽(Y/N)
                //    NVL(a.ONB_PUR_BUDGET_NT,0.0) ONB_PUR_BUDGET_NT, --//總預算金額(台幣)
                //    FGETCONTRACTDESC(a.OVC_PURCH,SUBSTR(a.OVC_PURCH,3,2),'onb_Bud_Money')*FGETCONTRACTDESC(a.OVC_PURCH,SUBSTR(a.OVC_PURCH,3,2),'onb_Bud_Rate') ONB_BUD_MONEY_NT,--// 分組預算金額(台幣)
                //    B.OVC_DAPPLY,--//預劃申購日期
                //    a.OVC_DPROPOSE,--//申購單位實際申請日期
                //    case when a.IS_PLURAL_BASIS='Y' then '是' else '否' end IS_PLURAL_BASIS,--//是否複數決標(Y/N)
                //    case when a.IS_OPEN_CONTRACT='Y' then '是' else '否' end IS_OPEN_CONTRACT,--//是否開放式契(Y/N)
                //    case when a.IS_JUXTAPOSED_MANUFACTURER='Y' then '是' else '否' end IS_JUXTAPOSED_MANUFACTURER,--//是否並列得標廠商(Y/N)
                //    --a.IS_OPEN_CONTRACT,--//是否開放式契(Y/N)
                //    --a.IS_JUXTAPOSED_MANUFACTURER,--//是否並列得標廠商(Y/N)
                //    (TO_CHAR((TO_DATE(NVL(B.OVC_DAPPLY, ''), 'YYYY-MM-DD') + NVL(B.ONB_REVIEW_DAYS, 0)), 'YYYY-MM-DD')) OVC_PLAN_PUR_DAPPROVE,--//預劃核定日期
                //    NVL(a.OVC_PUR_DAPPROVE, ' ') OVC_PUR_DAPPROVE,--//核定發文日期
                //    NVL(a.OVC_PUR_APPROVE, ' ') OVC_PUR_APPROVE,--//核定文號
                //    NVL(a.OVC_PUR_ALLOW, ' ') OVC_PUR_ALLOW,--//主官核批日
                //    NVL(a.OVC_PUR_DCANPO, ' ') OVC_PUR_DCANPO, --//撤案日
                //    case when a.OVC_PUR_DCANPO is null then '否' else '是' end OVC_PUR_DCANPO_YN, --//是否撤案
                //    t_PURCHASE_UNIT.OVC_ONNAME OVC_PURCHASE_UNIT, --B.OVC_PURCHASE_UNIT,--//採購發包單位代碼
                //    t_CONTRACT_UNIT.OVC_ONNAME OVC_CONTRACT_UNIT, --B.OVC_CONTRACT_UNIT, --//履約驗結單位代碼
                //    C.OVC_CHECKER, --//計評承辦人
                //    C.OVC_DRECEIVE, --//計評實際收辦日期(第1次分派日)
                //    C.OVC_DRECEIVE_PAPER, --//紙本收文日
                //    NVL(G.TTL_BID_GROUP, 0) TTL_BID_GROUP, --//已決標組數
                //    FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ovc_result') OVC_RESULT,--//最近一次開標結果(代碼A8)
                //    FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ovc_dopen') OVC_DOPEN,--//開標日期
                //    FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ONB_GROUP') ONB_GROUP,--//組別
                //    NVL(FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ONB_BID_RESULT') * FGETCONTRACTDESC(a.OVC_PURCH, '05', 'ONB_RESULT_RATE'),0) ONB_MONEY_NT, --// 決標金額(台幣)
                //    FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ovc_Dbid') OVC_DBID,--//決標日
                //    FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ovc_Ven_Title') OVC_VEN_TITLE,--//得標商名稱
                //    FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ovc_Dcontract') OVC_DCONTRACT,--//簽約日
                //    NVL(FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'onb_Delivery_Times'),0) ONB_DELIVERY_TIMES, --//交貨批次
                //    NVL(FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'onb_Money') * FGETCONTRACTDESC(a.OVC_PURCH, '05', 'onb_Rate'),0) ONB_MONEY, --//合約金額
                //    FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ovc_Delivery') OVC_DELIVERY,--//實際交貨日期
                //    FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ovc_Delivery_Contract') OVC_DELIVERY_CONTRACT, --//契約交貨日期
                //    FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'ovc_Dpay') OVC_DINSPECT_END, --//結報日期(驗結日期)
                //    NVL(I.TTL_GROUP, 0) TTL_GROUP, --//總組數
                //    case when NVL(U.TTL_PERMI_GROUP, 0)<>0 then NVL(U.TTL_PERMI_GROUP, 0) else case when NVL(I.TTL_GROUP, 0)=0 then 1 else NVL(I.TTL_GROUP, 0) end end TTL_PERMI_GROUP, --//核定分組數
                //    FGETCONTRACTDESC(a.OVC_PURCH, SUBSTR(a.OVC_PURCH,3,2), 'OVC_NAME') OVC_NAME, --//採包承辦人
                //    T.OVC_DO_NAME,  --//履驗承辦人
                //    case when NVL(a.OVC_PUR_ALLOW,' ') = ' ' or NVL(C.OVC_DRECEIVE_PAPER,' ') = ' ' then '0' else TO_CHAR(TO_DATE(a.OVC_PUR_ALLOW, 'YYYY-MM-DD') - TO_DATE(C.OVC_DRECEIVE_PAPER, 'YYYY-MM-DD')) end AUDITTOTALDAY,  --//評核總天數
                //    case when instr(GPA.OVC_MEMO, '案內品項屬政府採購協定')>0 then '是' else '否' end GPA, --//是否適用GPA
                //    GPA.OVC_MEMO GPA_TYPE, --//政府採購協定片語
                //    case when L.vCase>0 then 'Y' else 'N' end OVC_ADVENTAGED_CHECK--//準用最有利標
                //    ";
                #endregion
                #region 資料表
                strSQL += $@"
                    from
                        TBM1301 a,  --//--採購主檔
                        TBM1301_PLAN B, --// --預劃主檔
                        (select aa.OVC_PURCH,aa.OVC_CHECK_UNIT,aa.OVC_CHECKER,aa.OVC_DRECEIVE,aa.OVC_DRECEIVE_PAPER --//計評主檔(計評承辦人),第1次分派日
                            from TBM1202 aa,
                            (select OVC_PURCH, OVC_CHECK_UNIT, OVC_CHECKER, min(OVC_DRECEIVE) OVC_DRECEIVE
                                from TBM1202 where OVC_CHECK_UNIT = { strParameterName[1] } and SUBSTR(OVC_PURCH,3,2){strYearSQL}
                                group by OVC_PURCH,OVC_CHECK_UNIT,OVC_CHECKER ) bb
                            where aa.OVC_PURCH = bb.OVC_PURCH and aa.OVC_CHECK_UNIT = bb.OVC_CHECK_UNIT
                            and aa.OVC_CHECKER = bb.OVC_CHECKER and aa.OVC_DRECEIVE = bb.OVC_DRECEIVE) c,
                        (select OVC_PURCH, COUNT(1) TTL_BID_GROUP --//開標紀錄檔(決標組數)
                            from TBM1303  where OVC_RESULT = '0'
                            group by OVC_PURCH
                            having SUBSTR(OVC_PURCH,3,2){strYearSQL} ) g,
                        (select OVC_PURCH, max(ONB_GROUP) TTL_GROUP --//開標紀錄檔(最大分組數)
                            from TBM1303 where SUBSTR(OVC_PURCH, 3, 2){strYearSQL}
                            group by OVC_PURCH ) i,
                        (select distinct OVC_PURCH, OVC_DO_NAME 
                            from TBMRECEIVE_CONTRACT where SUBSTR(OVC_PURCH, 3, 2){strYearSQL} ) t,
                        (select OVC_PURCH, COUNT(1) TTL_PERMI_GROUP 
                            from TBM1118_1 where SUBSTR(OVC_PURCH, 3, 2){strYearSQL} group by OVC_PURCH ) U, --//核定組數

                        (select OVC_PURCH, OVC_MEMO from TBM1220_1 where SUBSTR(OVC_PURCH, 3, 2){strYearSQL} and OVC_IKIND='M47' and ONB_NO=1) GPA, --//政府採購協定片語 GPA
                        (select OVC_PURCH, count(1) vCase from tbm1220_1 where SUBSTR(OVC_PURCH, 3, 2){strYearSQL} and ovc_ikind = 'A12' and ovc_check = 'ee' group by OVC_PURCH) L, --//準用最有利標

                        (select OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PUR_SECTION, --//申購單位
                        (select OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PURCHASE_UNIT, --//採購發包單位
                        (select OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_CONTRACT_UNIT, --//履約驗結單位
                        (select OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='E8') t_PUR_APPROVE_DEP, --//核定權責
                        (select OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='TD') t_PLAN_PURCH, --//計畫性質
                        (select OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='C2') t_PUR_AGENCY, --//採購途徑 需外加 軍售案類別
                        (select OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='GN') t_LAB, --//採購屬性
                        (select OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='C7') t_PUR_ASS_VEN_CODE --//招標方式
                    ";
                #endregion
                #region join 條件
                strSQL += $@"
                    where a.OVC_PURCH = b.OVC_PURCH
                    and a.OVC_PURCH = c.OVC_PURCH(+)
                    and a.OVC_PURCH = g.OVC_PURCH(+)
                    and a.OVC_PURCH = t.OVC_PURCH(+)
                    and a.OVC_PURCH = u.OVC_PURCH(+)
                    and a.OVC_PURCH = i.OVC_PURCH(+)
                    and a.OVC_PURCH = GPA.OVC_PURCH(+)
                    and a.OVC_PURCH = l.OVC_PURCH(+)

                    and a.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    and b.OVC_PURCHASE_UNIT = t_PURCHASE_UNIT.OVC_DEPT_CDE(+)
                    and b.OVC_CONTRACT_UNIT = t_CONTRACT_UNIT.OVC_DEPT_CDE(+) 
                    and b.OVC_PUR_APPROVE_DEP = t_PUR_APPROVE_DEP.OVC_PHR_ID(+)
                    and a.OVC_PLAN_PURCH = t_PLAN_PURCH.OVC_PHR_ID(+)
                    and a.OVC_PUR_AGENCY = t_PUR_AGENCY.OVC_PHR_ID(+)
                    and a.OVC_LAB = t_LAB.OVC_PHR_ID(+)
                    and a.OVC_PUR_ASS_VEN_CODE = t_PUR_ASS_VEN_CODE.OVC_PHR_ID(+)

                    and SUBSTR(a.OVC_PURCH, 3, 2){strYearSQL}
                    ";
                #endregion
                #region 篩選條件
                //--+加欄位篩選(有選擇的條件(下方有說明)+
                //申購單位 drpOVC_PUR_SECTION 測試OK
                string strOVC_PUR_SECTION = drpOVC_PUR_SECTION.SelectedValue;
                if (!strOVC_PUR_SECTION.Equals(string.Empty))
                {
                    DataTable dt_DEPT = FCommon.getDataTableDEPT_Includesubordinate(strOVC_PUR_SECTION);
                    string strDEPTs = "''";
                    for (int i = 0; i < dt_DEPT.Rows.Count; i++)
                    {
                        strDEPTs = i == 0 ? "" : strDEPTs + ",";
                        string strOVC_DEPT_CDE = dt_DEPT.Rows[i]["OVC_DEPT_CDE"].ToString();
                        strDEPTs += $"'{ strOVC_DEPT_CDE }'";
                    }
                    strSQL += $@"
                        and a.OVC_PUR_SECTION in ({ strDEPTs })";
                }
                //核定權責 drpOVC_PUR_APPROVE_DEP 測試OK
                string strOVC_PUR_APPROVE_DEP = drpOVC_PUR_APPROVE_DEP.SelectedValue;
                if (!strOVC_PUR_APPROVE_DEP.Equals(string.Empty))
                    strSQL += $@"
                        and b.OVC_PUR_APPROVE_DEP='{ strOVC_PUR_APPROVE_DEP }'";
                //預劃申購日期 txtOVC_DAPPLY 測試OK
                string strOVC_DAPPLY1 = txtOVC_DAPPLY1.Text;
                if (!strOVC_DAPPLY1.Equals(string.Empty))
                    strSQL += $@"
                        and b.OVC_DAPPLY >= '{ strOVC_DAPPLY1 }'
                    ";
                string strOVC_DAPPLY2 = txtOVC_DAPPLY2.Text;
                if (!strOVC_DAPPLY2.Equals(string.Empty))
                    strSQL += $@"
                        and b.OVC_DAPPLY <= '{ strOVC_DAPPLY2 }'
                    ";
                //申購單位實際申請日期 txtOVC_DPROPOSE 測試OK
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
                //預劃核定日期 txtOVC_PLAN_PUR_DAPPROVE 測試OK
                //string strOVC_PLAN_PUR_DAPPROVE1 = txtOVC_PLAN_PUR_DAPPROVE1.Text;
                //if (!strOVC_PLAN_PUR_DAPPROVE1.Equals(string.Empty))
                //    strSQL += $@"
                //        and OVC_PLAN_PUR_DAPPROVE >= '{ strOVC_PLAN_PUR_DAPPROVE1 }'
                //    ";
                //string strOVC_PLAN_PUR_DAPPROVE2 = txtOVC_PLAN_PUR_DAPPROVE2.Text;
                //if (!strOVC_PLAN_PUR_DAPPROVE2.Equals(string.Empty))
                //    strSQL += $@"
                //        and OVC_PLAN_PUR_DAPPROVE <= '{ strOVC_PLAN_PUR_DAPPROVE2 }'
                //    ";
                //主官核批日 txtOVC_PUR_ALLOW 測試OK
                string strOVC_PUR_ALLOW1 = txtOVC_PUR_ALLOW1.Text;
                if (!strOVC_PUR_ALLOW1.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_PUR_ALLOW >= '{ strOVC_PUR_ALLOW1 }'
                    ";
                string strOVC_PUR_ALLOW2 = txtOVC_PUR_ALLOW2.Text;
                if (!strOVC_PUR_ALLOW2.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_PUR_ALLOW <= '{ strOVC_PUR_ALLOW2 }'
                    ";
                //核定（發文）日期 txtOVC_PUR_DAPPROVE 測試OK
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
                //計評承辦人 txtOVC_CHECKER 測試OK
                string strOVC_CHECKER = txtOVC_CHECKER.Text;
                if (!strOVC_CHECKER.Equals(string.Empty))
                {
                    strSQL += $@"
                        and c.OVC_CHECKER='{ strOVC_CHECKER }'
                    ";
                }
                //計畫性質 drpOVC_PLAN_PURCH 測試OK
                string strOVC_PLAN_PURCH = drpOVC_PLAN_PURCH.SelectedValue;
                if (!strOVC_PLAN_PURCH.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_PLAN_PURCH='{ strOVC_PLAN_PURCH }'";
                //採購途徑 drpOVC_PUR_AGENCY 測試OK
                string strOVC_PUR_AGENCY = drpOVC_PUR_AGENCY.SelectedValue;
                if (!strOVC_PUR_AGENCY.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_PUR_AGENCY='{ strOVC_PUR_AGENCY }'";
                //採購屬性 drpOVC_LAB 測試OK
                string strOVC_LAB = drpOVC_LAB.SelectedValue;
                if (!strOVC_LAB.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_LAB='{ strOVC_LAB }'";
                //招標方式 drpOVC_PUR_ASS_VEN_CODE 測試OK
                string strOVC_PUR_ASS_VEN_CODE = drpOVC_PUR_ASS_VEN_CODE.SelectedValue;
                if (!strOVC_PUR_ASS_VEN_CODE.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_PUR_ASS_VEN_CODE='{ strOVC_PUR_ASS_VEN_CODE }'";
                //是否適用GPA drpGPA 測試OK
                string strGPA = drpGPA.SelectedValue;
                if (!strGPA.Equals(string.Empty))
                {
                    if (strGPA.Equals("Y"))
                        strSQL += $@"
                            and instr(GPA.OVC_MEMO, '案內品項屬政府採購協定')>0";
                    else
                        strSQL += $@"
                            and (GPA.OVC_MEMO is null or instr(GPA.OVC_MEMO, '案內品項屬政府採購協定')=0)";
                }
                //採購發包單位 drpOVC_PURCHASE_UNIT 測試OK
                string strOVC_PURCHASE_UNIT = drpOVC_PURCHASE_UNIT.SelectedValue;
                if (!strOVC_PURCHASE_UNIT.Equals(string.Empty))
                {
                    DataTable dt_DEPT = FCommon.getDataTableDEPT_Includesubordinate(strOVC_PURCHASE_UNIT);
                    string strDEPTs = "''";
                    for (int i = 0; i < dt_DEPT.Rows.Count; i++)
                    {
                        strDEPTs = i == 0 ? "" : strDEPTs + ",";
                        string strOVC_DEPT_CDE = dt_DEPT.Rows[i]["OVC_DEPT_CDE"].ToString();
                        strDEPTs += $"'{ strOVC_DEPT_CDE }'";
                    }
                    strSQL += $@"
                        and b.OVC_PURCHASE_UNIT in ({ strDEPTs })";
                }
                //履約驗結單位 drpOVC_CONTRACT_UNIT 測試OK
                string strOVC_CONTRACT_UNIT = drpOVC_CONTRACT_UNIT.SelectedValue;
                if (!strOVC_CONTRACT_UNIT.Equals(string.Empty))
                {
                    DataTable dt_DEPT = FCommon.getDataTableDEPT_Includesubordinate(strOVC_CONTRACT_UNIT);
                    string strDEPTs = "''";
                    for (int i = 0; i < dt_DEPT.Rows.Count; i++)
                    {
                        strDEPTs = i == 0 ? "" : strDEPTs + ",";
                        string strOVC_DEPT_CDE = dt_DEPT.Rows[i]["OVC_DEPT_CDE"].ToString();
                        strDEPTs += $"'{ strOVC_DEPT_CDE }'";
                    }
                    strSQL += $@"
                        and b.OVC_CONTRACT_UNIT in ({ strDEPTs })";
                }
                //需公開閱覽 drpOVC_OPEN_CHECK 測試OK
                string strOVC_OPEN_CHECK = drpOVC_OPEN_CHECK.SelectedValue;
                if (!strOVC_OPEN_CHECK.Equals(string.Empty))
                {
                    if (strOVC_OPEN_CHECK.Equals("Y"))
                        strSQL += $@"
                            and b.OVC_OPEN_CHECK='{ strOVC_OPEN_CHECK }'";
                    else
                        strSQL += $@"
                            and (b.OVC_OPEN_CHECK='{ strOVC_OPEN_CHECK }' or b.OVC_OPEN_CHECK is null)";
                }
                //是否複數決標 drpIS_PLURAL_BASIS 測試OK
                string strIS_PLURAL_BASIS = drpIS_PLURAL_BASIS.SelectedValue;
                if (!strIS_PLURAL_BASIS.Equals(string.Empty))
                {
                    if (strIS_PLURAL_BASIS.Equals("Y"))
                        strSQL += $@"
                            and a.IS_PLURAL_BASIS='{ strIS_PLURAL_BASIS }'";
                    else
                        strSQL += $@"
                            and (a.IS_PLURAL_BASIS='{ strIS_PLURAL_BASIS }' or a.IS_PLURAL_BASIS is null)";
                }
                //是否開放式契 drpIS_OPEN_CONTRACT 測試OK
                string strIS_OPEN_CONTRACT = drpIS_OPEN_CONTRACT.SelectedValue;
                if (!strIS_OPEN_CONTRACT.Equals(string.Empty))
                {
                    if (strIS_OPEN_CONTRACT.Equals("Y"))
                        strSQL += $@"
                            and a.IS_OPEN_CONTRACT='{ strIS_OPEN_CONTRACT }'";
                    else
                        strSQL += $@"
                            and (a.IS_OPEN_CONTRACT='{ strIS_OPEN_CONTRACT }' or a.IS_OPEN_CONTRACT is null)";
                }
                //是否並列得標廠商 drpIS_JUXTAPOSED_MANUFACTURER 測試OK
                string strIS_JUXTAPOSED_MANUFACTURER = drpIS_JUXTAPOSED_MANUFACTURER.SelectedValue;
                if (!strIS_JUXTAPOSED_MANUFACTURER.Equals(string.Empty))
                {
                    if (strIS_JUXTAPOSED_MANUFACTURER.Equals("Y"))
                        strSQL += $@"
                            and a.IS_JUXTAPOSED_MANUFACTURER='{ strIS_JUXTAPOSED_MANUFACTURER }'";
                    else
                        strSQL += $@"
                            and (a.IS_JUXTAPOSED_MANUFACTURER='{ strIS_JUXTAPOSED_MANUFACTURER }' or a.IS_JUXTAPOSED_MANUFACTURER is null)";
                }
                #endregion
                #region 基本條件 & 排序
                strSQL += $@"
                    and (b.OVC_AUDIT_UNIT = { strParameterName[1] })--//計評單位=登錄者單位  
                    and (b.OVC_DCANCEL is null or b.OVC_DCANCEL = ' ')--//預劃沒有撤案
                    order by OVC_PURCH,OVC_PURCH_5,OVC_PURCH_6
                    ";
                #endregion
                #region 額外欄位查詢語法
                ////決標日期、最後決標日 chkOVC_DBID 上方已查詢，範例資料表同樣無資料，確認是否要額外查詢
                //string[] strParameterName_OVC_DBID = { ":vOVC_PURCH" };
                //ArrayList aryData_OVC_DBID = new ArrayList();
                //aryData_OVC_DBID.Add("");
                //string strSQL_OVC_DBID = $"select max(ovc_Dbid) ovc_Dbid from tbmbid_result where ovc_purch={ strParameterName_OVC_DBID[0] }";
                ////得標廠商、所有得標廠商 chkOVC_VEN_TITLE 上方已查詢，範例資料表同樣無資料，確認是否要額外查詢
                //string[] strParameterName_OVC_VEN_TITLE = { ":vOVC_PURCH" };
                //ArrayList aryData_OVC_VEN_TITLE = new ArrayList();
                //aryData_OVC_VEN_TITLE.Add("");
                //string strSQL_OVC_VEN_TITLE = $"select nvl(ovc_Ven_Title,'') ovc_Ven_Title from tbm1302 where ovc_purch={ strParameterName_OVC_DBID[0] }";
                ////簽約日期、第一個分約簽約日期 chkOVC_DCONTRACT 上方已查詢，範例資料表同樣無資料，確認是否要額外查詢
                //string strSQL_OVC_DCONTRACT = "select min(ovc_Dcontract) ovc_Dcontract from tbm1302 where ovc_purch=?";
                ////交貨批次 chkONB_DELIVERY_TIMES 上方已查詢，範例資料表同樣全0，確認是否要額外查詢
                //string strSQL = "select nvl(max(onb_Delivery_Times),1) onb_Delivery_Times from tbm1302 where ovc_purch=?";
                ////合約交貨日期、契約交貨日期 chkOVC_DELIVERY_CONTRACT 上方已查詢，範例資料表同樣無資料，確認是否要額外查詢
                //string strSQL = @"select distinct a.ovc_Delivery_Contract,a.ovc_Delivery from tbmdelivery a, 
                //    (select * from tbm1302 where  ovc_Dbid = (select max(ovc_Dbid) ovc_Dbid 
                //     from tbm1302  where ovc_purch=?) and ovc_purch= ?  ) b 
                //     where a.ovc_purch = b.ovc_purch and a.ovc_purch_6 = b.ovc_purch_6 and 
                //     a.ovc_ven_cst = b.ovc_Ven_Cst and a.ovc_purch= ? order by a.OVC_DELIVERY desc
                //    ";
                ////驗結日期 chkOVC_DINSPECT_END 上方已查詢，範例資料表同樣無資料，確認是否要額外查詢
                //string strSQL = @"select max(ovc_Dpay) ovc_Dinspect_End from TBMDELIVERY where ovc_purch=?";

                //結案日期、最後結案日期 chkOVC_DCLOSE
                string[] strParameterName_OVC_DCLOSE = { ":vOVC_PURCH" };
                ArrayList aryData_OVC_DCLOSE = new ArrayList();
                aryData_OVC_DCLOSE.Add("");
                string strSQL_OVC_DCLOSE = $@"select max(ovc_Dclose) ovc_Dclose from TBMRECEIVE_CONTRACT where ovc_purch={ strParameterName_OVC_DCLOSE[0] }";

                //合約金額、合約金額(全案) chkONB_MONEY 上方已查詢，範例資料表同樣全0，確認是否要額外查詢
                //string strSQL = "select nvl((sum(onb_Rate * onb_Money)),0.0) onb_Money from tbm1302 where ovc_purch=?";

                //決標方式片語 chkOVC_MEMO
                string[] strParameterName_OVC_MEMO = { ":vOVC_PURCH", ":vUNIT", ":vOVC_IKIND" };
                ArrayList aryData_OVC_MEMO = new ArrayList();
                aryData_OVC_MEMO.Add("");
                aryData_OVC_MEMO.Add(strDEPT_SN);
                aryData_OVC_MEMO.Add("A14");
                string strSQL_OVC_MEMO = $@"
                    select distinct a.* 
                    from TBM1202_6 a, 
                    (select max(ONB_CHECK_TIMES) ONB_CHECK_TIMES from TBM1202_6 where OVC_PURCH = { strParameterName_OVC_MEMO[0] } and OVC_IKIND = { strParameterName_OVC_MEMO[2] } and OVC_CHECK_UNIT = { strParameterName_OVC_MEMO[1] }) B
                        where a.OVC_PURCH = { strParameterName_OVC_MEMO[0] }
                        and a.OVC_IKIND = { strParameterName_OVC_MEMO[2] }
                        and a.OVC_CHECK_UNIT = { strParameterName_OVC_MEMO[1] }
                        and a.ONB_CHECK_TIMES = b.ONB_CHECK_TIMES";
                //政府採購協定片語、政府採購協定GPA
                #endregion
                #endregion
                
                //DataTable dt_Source = CommonStatic.LinqQueryToDataTable(query);
                //FCommon.AlertShow(PnMessage, "danger", "系統訊息", strSQL);
                DataTable dt_Source = FCommon.getDataTableFromSelect(strSQL, strParameterName, aryData);
                int intCount_Data;
                //將缺少SQL欄位補足
                foreach (string strFieldSql in strFieldSqls)
                {
                    if (!dt_Source.Columns.Contains(strFieldSql))
                        dt_Source.Columns.Add(strFieldSql);
                }
                //int intMaxLength = dt_Source.Columns["OVC_PUR_AGENCY"].MaxLength;
                //FCommon.AlertShow(PnMessage, "danger", "系統訊息", dt_Source.Columns["OVC_PUR_AGENCY"].MaxLength.ToString());
                foreach (DataColumn dc in dt_Source.Columns)
                {
                    dc.ReadOnly=false;
                }


                foreach (DataRow dr in dt_Source.Rows)
                {
                    bool isShow = true; //判斷額外篩選條件
                                        //預劃核定日期 txtOVC_PLAN_PUR_DAPPROVE
                    string strOVC_PLAN_PUR_DAPPROVE = dr["OVC_PLAN_PUR_DAPPROVE"].ToString();
                    string strOVC_PLAN_PUR_DAPPROVE1 = txtOVC_PLAN_PUR_DAPPROVE1.Text;
                    string strOVC_PLAN_PUR_DAPPROVE2 = txtOVC_PLAN_PUR_DAPPROVE2.Text;
                    DateTime dateOVC_PLAN_PUR_DAPPROVE;
                    if (DateTime.TryParse(strOVC_PLAN_PUR_DAPPROVE, out dateOVC_PLAN_PUR_DAPPROVE))
                    {
                        if (!strOVC_PLAN_PUR_DAPPROVE1.Equals(string.Empty))
                        {
                            DateTime date1 = DateTime.Parse(strOVC_PLAN_PUR_DAPPROVE1);
                            isShow = isShow && DateTime.Compare(date1, dateOVC_PLAN_PUR_DAPPROVE) <= 0;
                        }
                        if (!strOVC_PLAN_PUR_DAPPROVE2.Equals(string.Empty))
                        {
                            DateTime date2 = DateTime.Parse(strOVC_PLAN_PUR_DAPPROVE2);
                            isShow = isShow && DateTime.Compare(dateOVC_PLAN_PUR_DAPPROVE, date2) <= 0;
                        }
                    }
                    else if (!strOVC_PLAN_PUR_DAPPROVE1.Equals(string.Empty) || !strOVC_PLAN_PUR_DAPPROVE2.Equals(string.Empty))
                        isShow = false;
                    if (!isShow)
                        dr.Delete();
                    else
                    {
                        string strOVC_PURCH = dr["OVC_PURCH"].ToString();
                        aryData_OVC_DCLOSE[0] = strOVC_PURCH;
                        aryData_OVC_MEMO[0] = strOVC_PURCH;

                        //string strOVC_PUR_AGENCY = dr["OVC_PUR_AGENCY"].ToString() + dr["OVC_MILITARY_TYPE"].ToString();
                        //if (strOVC_PUR_AGENCY.Length > intMaxLength)
                        //    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strOVC_PURCH +"："+ strOVC_PUR_AGENCY);
                        //else
                        //    dr["OVC_PUR_AGENCY"] = strOVC_PUR_AGENCY;
                        dr["OVC_PUR_AGENCY"] = dr["OVC_PUR_AGENCY"].ToString() + dr["OVC_MILITARY_TYPE"].ToString();

                        if (chkOVC_DCLOSE.Checked) //結案日期、最後結案日期 chkOVC_DCLOSE
                        {
                            DataTable dt_OVC_DCLOSE = FCommon.getDataTableFromSelect(strSQL_OVC_DCLOSE, strParameterName_OVC_DCLOSE, aryData_OVC_DCLOSE);
                            if (dt_OVC_DCLOSE.Rows.Count > 0)
                                dr["OVC_DCLOSE"] = dt_OVC_DCLOSE.Rows[0]["OVC_DCLOSE"];
                        }
                        if (chkOVC_MEMO.Checked) //決標方式片語 chkOVC_MEMO
                        {
                            DataTable dt_OVC_MEMO = FCommon.getDataTableFromSelect(strSQL_OVC_MEMO, strParameterName_OVC_MEMO, aryData_OVC_MEMO);
                            if (dt_OVC_MEMO.Rows.Count > 0)
                                dr["OVC_MEMO"] = dt_OVC_MEMO.Rows[0]["OVC_MEMO"];
                        }
                    }
                }
                dt_Source.AcceptChanges(); //將刪除列儲存，完成刪除動作

                DataTable dt = getDataTable_Export(dt_Source, out intCount_Data);

                if (intCount_Data > 0)// && false)
                {
                    string strSheetText = "1_履驗";
                    string strTitleText = $"{ strYearText }年度{ strDEPT_Name }購案管制表";
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
        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("FPlanAssessmentSA");
        }
        #endregion
    }
}