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
    public partial class FPlanPurchase : Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string strDateFormat = Variable.strDateFormat;
        public string strDEPT_SN, strDEPT_Name;

        #region 副程式
        private void list_dataImport()
        {
            //年度
            FCommon.list_dataImportYear(drpYear_PerPur);
            FCommon.list_dataImportYear(drpYear_Uncheck);
        }
        //3.4資料表
        private void Query_(string strOVC_PUR_AGENCY)
        {
            if (strDEPT_SN != null)
            {
                string[] strParameterName = { ":USER_DEPT_SN", ":OVC_PUR_AGENCY" };
                ArrayList aryData = new ArrayList();

                aryData.Add(strDEPT_SN);
                aryData.Add(strOVC_PUR_AGENCY);
                string strMessage = "";
                TextBox txtPurSta1 = (TextBox)pnQuery.FindControl($"txtPurSta1_{ strOVC_PUR_AGENCY }");
                TextBox txtPurSta2 = (TextBox)pnQuery.FindControl($"txtPurSta2_{ strOVC_PUR_AGENCY }");
                string strPurSta1 = "", strPurSta2 = "";
                if (FCommon.Controls_isExist(txtPurSta1, txtPurSta2))
                {
                    strPurSta1 = txtPurSta1.Text;
                    strPurSta2 = txtPurSta2.Text;
                }
                if (strPurSta1.Equals(string.Empty) && strPurSta2.Equals(string.Empty))
                    strMessage += "<p> 至少選擇一個日期 </p>";

                if (strMessage.Equals(string.Empty))
                {
                    #region SQL語法
                    string strSQL = "";
                    #region 欄位
                    strSQL += $@"
                    select
                    --//額外查詢 預算科目
                    --//額外查詢 預算款源
                    (a.OVC_PURCH||a.OVC_PUR_AGENCY||e.OVC_PURCH_5) PURCH, --//購案編號
                    a.OVC_PUR_IPURCH, --//購案名稱
                    t_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --a.OVC_PUR_NSECTION, --//申購單位
                    case when b.OVC_PUR_APPROVE_DEP='A' then '部核' else '非部核' end OVC_PUR_APPROVE_DEP, --猜的a.OVC_PURCH_KIND, --//購案類別=屬性(核定類別)
                    f.ONB_TIMES, --//開標次數
                    case when f.OVC_RESULT is null then '' else f.OVC_DOPEN end OVC_DOPEN, --//決標日期
                    --f.OVC_DOPEN, --NVL(e.OVC_DOPEN,' ') OVC_DOPEN, --//決標日期
                    --case when f.OVC_RESULT is null or g.OVC_VEN_TITLE is null then '' else decode(g.OVC_VEN_TITLE, null,f.OVC_VENDORS_NAME,g.OVC_VEN_TITLE) end OVC_VENDORS_NAME, --//得標商
                    case when f.OVC_RESULT is null or g.OVC_VEN_TITLE is null then '' else decode(g.OVC_VEN_TITLE, null,f.OVC_VENDORS_NAME,g.OVC_VEN_TITLE)||'('||g.OVC_PURCH_6||'，'||g.ONB_GROUP||')' end OVC_VENDORS_NAME, --//得標商（合約號，組別）
                    --case when f.OVC_RESULT is null or a.ONB_PUR_BUDGET=0 then '' else to_char(a.ONB_PUR_BUDGET) end ONB_PUR_BUDGET, --//預算金額
                    case when f.OVC_RESULT is null or g.ONB_BUD_MONEY=0 then '' else to_char(g.ONB_BUD_MONEY) end ONB_PUR_BUDGET, --//預算金額
                    case when f.OVC_RESULT is null or f.ONB_BID_RESULT=0 then '' else to_char(f.ONB_BID_RESULT) end ONB_BID_RESULT, --//決標金額
                    --round(case when NVL(g.ONB_BUD_MONEY, 0)=0 then 0 else NVL(f.ONB_BID_RESULT, 0) / NVL(g.ONB_BUD_MONEY, 0) end, 2) BUDGET_RATIO, --//決標預算比％
                    case when f.OVC_RESULT is null or NVL(g.ONB_BUD_MONEY, 0)=0 then '' else to_char(round(NVL(f.ONB_BID_RESULT, 0) / g.ONB_BUD_MONEY *100, 4)) end BUDGET_RATIO, --//決標預算比％
                    --(NVL(a.ONB_PUR_BUDGET, 0) - NVL(f.ONB_BID_RESULT, 0)) Balance, --//標餘款
                    (NVL(g.ONB_BUD_MONEY, 0) - NVL(f.ONB_BID_RESULT, 0)) Balance, --//標餘款
                    --//備考

                    a.OVC_PURCH
                    ";
                    string sss = @"
                    ----以下沒用到----
                    --a.OVC_PUR_AGENCY,
                    --a.OVC_PUR_NSECTION,
                    --a.OVC_COUNTRY,
                    --a.OVC_PUR_ASS_VEN_CODE,
                    --D.OVC_DBEGIN,
                    --g.OVC_PURCH_6,
                    --f.ONB_BID_BUDGET,
                    --c.OVC_COMM,
                    --c.OVC_COMM_REASON,
                    --g.OVC_DCONTRACT,
                    --b.ONB_TENDER_DAYS,
                    --g.OVC_CONTRACT_COMM,
                    --d.OVC_DO_NAME,
                    --b.ONB_DELIVER_DAYS,
                    --b.OVC_PUR_APPROVE_DEP,
                    --b.OVC_PLAN_PURCH,
                    --a.OVC_PUR_CURRENT,
                    --a.OVC_DPROPOSE,
                    --a.OVC_PUR_DAPPROVE,
                    --a.OVC_QUOTE,
                    --g.ONB_GROUP,
                    --g.ONB_BUD_MONEY,
                    --f.OVC_RESULT,
                    --g.OVC_VEN_BOSS,
                    --G.OVC_VEN_NAME
                ";
                    #endregion
                    #region 資料表
                    strSQL += $@"
                    from
                    TBM1301 a,
                    TBM1301_PLAN B,
                    TBMRECEIVE_BID_LOG c,
                    VIWORKSTATUS21 D,
                    VIMAXSTATUS E,
                    VILASTTBM1303 F,
                    TBM1302 g,

                    (SELECT OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PUR_SECTION --//申購單位
                    ";
                    #endregion
                    #region join 條件
                    strSQL += $@"
                    where a.OVC_PURCH = b.OVC_PURCH
                    and a.OVC_PURCH = e.OVC_PURCH(+)
                    and e.OVC_PURCH = c.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = c.OVC_PURCH_5(+)
                    and e.OVC_PURCH = d.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = d.OVC_PURCH_5(+)
                    and e.OVC_PURCH = f.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = f.OVC_PURCH_5(+)
                    and e.OVC_PURCH = g.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = g.OVC_PURCH_5(+)

                    and a.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    ";
                    #endregion
                    #region 篩選條件
                    //--+加欄位篩選(有選擇的條件(下方有說明)+
                    if (!strPurSta1.Equals(string.Empty))
                        strSQL += $@"and f.OVC_DOPEN >= '{ txtPurSta1.Text }'";
                    if (!strPurSta2.Equals(string.Empty))
                        strSQL += $@"and f.OVC_DOPEN <= '{ txtPurSta2.Text }'";
                    #endregion
                    #region 基本條件 & 排序
                    if (strDEPT_SN.Equals("0A100"))
                    {
                        strSQL += $@"
                        and (b.OVC_AUDIT_UNIT = { strParameterName[0] } --//計評單位=登錄者單位
                            or b.OVC_PUR_APPROVE_DEP = 'A') --//讀取採購中心權責購案(部核案)
                        ";
                    }
                    else
                        strSQL += $@"
                        and b.OVC_AUDIT_UNIT = { strParameterName[0] }
                        ";
                    strSQL += $@"
                    and a.OVC_PUR_ALLOW is not null and a.OVC_PUR_ALLOW <> ' '
                    and a.OVC_DOING_UNIT = { strParameterName[0] } --//目前承辦單位=登錄者單位
                    and a.OVC_PUR_AGENCY = { strParameterName[1] }
                    order by a.OVC_PURCH, a.OVC_PUR_AGENCY, g.OVC_PURCH_6, f.OVC_DOPEN
                    ";
                    #endregion
                    #region 額外欄位查詢語法
                    string[] strParameterName_Other = { ":vOVC_PURCH" };
                    ArrayList aryData_Other = new ArrayList();
                    aryData_Other.Add("");
                    //預算科目 OVC_POI_IBDG
                    string strSQL_OVC_POI_IBDG = $@"select OVC_POI_IBDG from TBM1118  where OVC_PURCH = { strParameterName_Other[0] } order by OVC_POI_IBDG";
                    //預算款源 OVC_ISOURCE 多筆
                    string strSQL_OVC_ISOURCE = $@"select OVC_ISOURCE from TBM1231  where OVC_PURCH = { strParameterName_Other[0] }";
                    //備考 OVC_FAC_STATUS
                    //string[] strParameterName_OVC_FAC_STATUS = { ":vOVC_PURCH", ":vOVC_CHECK_UNIT" };
                    //ArrayList aryData_OVC_FAC_STATUS = new ArrayList();
                    //aryData_OVC_FAC_STATUS.Add("");
                    //aryData_OVC_FAC_STATUS.Add(strDEPT_SN);
                    //string strSQL_OVC_FAC_STATUS = $@"select OVC_FAC_STATUS from TBMFAC_STATUS where OVC_PURCH = { strParameterName_OVC_FAC_STATUS[0] } and OVC_CHECK_UNIT = { strParameterName_OVC_FAC_STATUS[1] }";
                    #endregion
                    string[] strFieldNames = { "預算科目", "預算款源", "購案編號", "購案名稱", "申購單位", "購案類別", "開標次數", "決標日期",
                    "得標商", "預算金額", "決標金額", "決標預算比(%)", "標餘款", "備考" };
                    string[] strFieldSqls = { "OVC_POI_IBDG", "OVC_ISOURCE", "PURCH", "OVC_PUR_IPURCH", "OVC_PUR_SECTION", "OVC_PUR_APPROVE_DEP", "ONB_TIMES", "OVC_DOPEN",
                    "OVC_VENDORS_NAME", "ONB_PUR_BUDGET", "ONB_BID_RESULT", "BUDGET_RATIO", "Balance", "", };
                    string strFormatInt = Variable.strExcleFormatInt;
                    string strFormatMoney2 = Variable.strExcleFormatMoney2;
                    string[] strFormat = { null, null, null, null, null, null, strFormatInt, null,
                    null, strFormatMoney2, strFormatMoney2, "0.0###", null, null };
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
                        //aryData_OVC_FAC_STATUS[0] = strOVC_PURCH;

                        //預算科目 OVC_POI_IBDG
                        DataTable dt_OVC_POI_IBDG = FCommon.getDataTableFromSelect(strSQL_OVC_POI_IBDG, strParameterName_Other, aryData_Other);
                        FPlanAssessmentSA.setFieldList(dr, dt_OVC_POI_IBDG, "OVC_POI_IBDG");
                        //預算款源 OVC_ISOURCE 多筆
                        DataTable dt_OVC_ISOURCE = FCommon.getDataTableFromSelect(strSQL_OVC_ISOURCE, strParameterName_Other, aryData_Other);
                        FPlanAssessmentSA.setFieldList(dr, dt_OVC_ISOURCE, "OVC_ISOURCE");//多筆
                                                                                          //DataTable dt_OVC_FAC_STATUS = FCommon.getDataTableFromSelect(strSQL_OVC_FAC_STATUS, strParameterName_OVC_FAC_STATUS, aryData_OVC_FAC_STATUS);
                                                                                          //if (dt_OVC_FAC_STATUS.Rows.Count > 0)
                                                                                          //    dr["OVC_FAC_STATUS"] = dt_OVC_FAC_STATUS.Rows[0]["OVC_FAC_STATUS"];
                    }

                    DataTable dt = FPlanAssessmentSA.getDataTable_Export(dt_Source, strFieldNames, strFieldSqls, out intCount_Data);

                    if (intCount_Data > 0)// && false)
                    {
                        string strSheetText = "", strTitleText = "";
                        if (strOVC_PUR_AGENCY.Equals("L"))
                        {
                            strSheetText = "3";
                            strTitleText = "內購案標餘款情形統計表";
                        }
                        else if (strOVC_PUR_AGENCY.Equals("W"))
                        {
                            strSheetText = "4";
                            strTitleText = "外購案標餘款情形統計表";
                        }

                        string strFileName = $"{ strTitleText }.xlsx";
                        MemoryStream Memory = ExcelNPOI.RenderDataTableToStream_Chief(dt, strSheetText, strTitleText, 3, strFormat); //取得Excel資料流
                        FCommon.DownloadFile(this, strFileName, Memory); //直接下載檔案
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查詢無結果，請重新下條件！");
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
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
                    FCommon.Controls_Attributes("readonly", "true", txtPurSta1_L, txtPurSta2_L, txtPurSta1_W, txtPurSta2_W);
                    list_dataImport();
                }
            }
        }

        #region OnClick
        //1
        protected void btnQuery_PerPur_Click(object sender, EventArgs e)
        {
            if (strDEPT_SN != null)
            {
                string[] strParameterName = { ":strYear", ":USER_DEPT_SN" };
                ArrayList aryData = new ArrayList();

                string strYear = drpYear_PerPur.SelectedValue;
                if (strYear.Length > 2)
                    strYear = strYear.Substring(strYear.Length - 2, 2);
                aryData.Add(strYear);
                aryData.Add(strDEPT_SN);
                //aryData.Add("175J5");
                bool isAll = rdoType.SelectedValue.Equals("All");

                #region SQL語法
                string strSQL = "";
                #region 欄位
                strSQL += $@"
                    select
                    a.OVC_PURCH||a.OVC_PUR_AGENCY||e.OVC_PURCH_5 PURCH, --//購案編號
                    g.OVC_PURCH_6, --//合約號
                    g.ONB_GROUP, --//組別
                    f.ONB_TIMES, --//總開標次數
                    a.OVC_PUR_IPURCH, --購案名稱
                    t_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --a.OVC_PUR_NSECTION, --//申購單位
                    --//額外查詢 預算款源
                    --//額外查詢 預算科子目
                    NVL(a.OVC_COUNTRY, '中華民國') OVC_COUNTRY, --//採購國家
                    t_PUR_ASS_VEN_CODE.OVC_PHR_DESC OVC_PUR_ASS_VEN_CODE, --NVL(a.OVC_PUR_ASS_VEN_CODE,' ') OVC_PUR_ASS_VEN_CODE, --//招標方式代碼
                    d.OVC_DBEGIN, --//收辦日期
                    case when f.OVC_RESULT is null then '' else f.OVC_DOPEN end OVC_DOPEN, --//決標日期
                    case when f.OVC_RESULT is null or f.ONB_BID_RESULT=0 then '' else to_char(f.ONB_BID_RESULT) end ONB_BID_RESULT, --//決標金額
                    case when f.OVC_RESULT is null or f.ONB_BID_BUDGET=0 then '' else to_char(f.ONB_BID_BUDGET) end ONB_BID_BUDGET, --//核定底價
                    case when f.OVC_RESULT is null or g.OVC_VEN_TITLE is null then '' else decode(g.OVC_VEN_TITLE, null,f.OVC_VENDORS_NAME,g.OVC_VEN_TITLE)||'('||g.OVC_PURCH_6||'，'||g.ONB_GROUP||')' end OVC_VENDORS_NAME, --//得標商（合約號，組別）
                    g.OVC_VEN_BOSS, --//負責人
                    g.OVC_VEN_NAME, --//連絡人
                    case when f.OVC_RESULT is null then '' else g.OVC_DCONTRACT end OVC_DCONTRACT, --//簽約日期
                    case when f.OVC_RESULT is null then '' else to_char(to_date(f.OVC_DOPEN,'YYYY/MM/DD') - to_date(d.OVC_DBEGIN,'YYYY/MM/DD')) end PURCHASE_DAYS, --//採購天數 =決標日期(YYYY-MM-DD)-收辦日期(YYYY-MM-DD)
                    b.ONB_TENDER_DAYS, --//標準天數
                    case when f.OVC_RESULT is null or g.ONB_BUD_MONEY=0 then '' else to_char(g.ONB_BUD_MONEY) end ONB_BUD_MONEY, --//預算金額
                    a.ONB_PUR_BUDGET, --//全案總預算金額
                    c.OVC_COMM, --//重要事項
                    b.ONB_DELIVER_DAYS, --//交貨天數
                    case when b.OVC_PUR_APPROVE_DEP='A' then '部核' else '非部核' end OVC_PUR_APPROVE_DEP, --//屬性(核定類別)
                    replace(t_PLAN_PURCH.OVC_PHR_DESC, '購案', '') OVC_PLAN_PURCH, --b.OVC_PLAN_PURCH, --//部核屬性(計畫性質)
                    d.OVC_DO_NAME, --//承辦人

                    a.OVC_PURCH,
                    e.OVC_PURCH_5
                    ----沒有用----
                    ";
                #endregion
                #region 資料表
                strSQL += $@"
                    from
                    TBM1301 a,
                    TBM1301_PLAN b,
                    TBMRECEIVE_BID_LOG c,
                    VIWORKSTATUS21 d,
                    VIMAXSTATUS e,
                    VILASTTBM1303 f,
                    TBM1302 g,

                    (SELECT OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PUR_SECTION, --//申購單位
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='C7') t_PUR_ASS_VEN_CODE, --//招標方式
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='TD') t_PLAN_PURCH --//計畫性質
                    ";
                #endregion
                #region join 條件
                strSQL += $@"
                    where a.OVC_PURCH = b.OVC_PURCH
                    and a.OVC_PURCH = e.OVC_PURCH(+)
                    and e.OVC_PURCH = c.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = c.OVC_PURCH_5(+)
                    and e.OVC_PURCH = d.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = d.OVC_PURCH_5(+)
                    and e.OVC_PURCH = f.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = f.OVC_PURCH_5(+)
                    and e.OVC_PURCH = g.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = g.OVC_PURCH_5(+)

                    and a.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    and a.OVC_PUR_ASS_VEN_CODE = t_PUR_ASS_VEN_CODE.OVC_PHR_ID(+)
                    and b.OVC_PLAN_PURCH = t_PLAN_PURCH.OVC_PHR_ID(+)
                    ";
                #endregion
                #region 篩選條件
                //--+加欄位篩選(有選擇的條件(下方有說明)+
                #endregion
                #region 基本條件 & 排序
                if (strDEPT_SN.Equals("0A100"))
                {
                    strSQL += $@"
                        and a.OVC_PUR_AGENCY in ('L','W')
                        and (b.OVC_AUDIT_UNIT = { strParameterName[1] } --//計評單位=登錄者單位
                            or b.OVC_PUR_APPROVE_DEP = 'A') --//讀取採購中心權責購案(部核案)
                        ";
                }
                else
                    strSQL += $@"
                        and b.OVC_AUDIT_UNIT = { strParameterName[1] }
                        ";
                if (isAll)
                    strSQL += $@"
                    and a.OVC_PUR_ALLOW is not null and a.OVC_PUR_ALLOW <> ' '
                    ";
                else
                    strSQL += $@"
                    and f.OVC_RESULT = '3'
                    ";
                strSQL += $@"
                    and a.OVC_DOING_UNIT = { strParameterName[1] }
                    and substr(a.OVC_PURCH,3,2) = { strParameterName[0] }
                    order by a.OVC_PURCH, a.OVC_PUR_AGENCY, g.OVC_PURCH_6, OVC_DBID
                    ";
                #endregion
                #region 額外欄位查詢語法
                string[] strParameterName_Other = { ":vOVC_PURCH" };
                ArrayList aryData_Other = new ArrayList();
                aryData_Other.Add("");
                //預算款源 OVC_ISOURCE 多筆
                string strSQL_OVC_ISOURCE = $@"select OVC_ISOURCE from TBM1231  where OVC_PURCH = { strParameterName_Other[0] }";
                //預算科子目 OVC_POI_IBDG 多筆
                string strSQL_OVC_POI_IBDG = $@"select OVC_POI_IBDG from TBM1118  where OVC_PURCH = { strParameterName_Other[0] } order by OVC_POI_IBDG";
                #endregion
                string[] strFieldNames = { "購案編號", "合約號", "組別", "總開標次數", "購案名稱", "申購單位", "預算款源", "預算科子目",
                    "採購國家", "招標方式", "收辦日期", "決標日期", "決標金額", "核定底價", "得標商（合約號，組別）",
                    "負責人", "連絡人", "簽約日期", "採購天數", "標準天數", "預算金額", "全案總預算金額", "重要事項",
                    "交貨天數", "屬性", "部核屬性", "承辦人" };
                string[] strFieldSqls = { "PURCH", "OVC_PURCH_6", "ONB_GROUP", "ONB_TIMES", "OVC_PUR_IPURCH", "OVC_PUR_SECTION", "OVC_ISOURCE", "OVC_POI_IBDG",
                    "OVC_COUNTRY", "OVC_PUR_ASS_VEN_CODE", "OVC_DBEGIN", "OVC_DOPEN", "ONB_BID_RESULT", "ONB_BID_BUDGET", "OVC_VENDORS_NAME",
                    "OVC_VEN_BOSS", "OVC_VEN_NAME", "OVC_DCONTRACT", "PURCHASE_DAYS", "ONB_TENDER_DAYS", "ONB_BUD_MONEY", "ONB_PUR_BUDGET", "OVC_COMM",
                    "ONB_DELIVER_DAYS", "OVC_PUR_APPROVE_DEP", "OVC_PLAN_PURCH", "OVC_DO_NAME" };
                string strFormatInt = Variable.strExcleFormatInt;
                string strFormatMoney2 = Variable.strExcleFormatMoney2;
                string[] strFormat = { null, null, strFormatInt, strFormatInt, null, null, null, null,
                    null, null, null, null, strFormatMoney2, strFormatMoney2, null,
                    null, null, null, strFormatInt, strFormatInt, strFormatMoney2, strFormatMoney2, null,
                    strFormatInt, null, null, null };
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

                    //預算款源 OVC_ISOURCE 多筆
                    DataTable dt_OVC_ISOURCE = FCommon.getDataTableFromSelect(strSQL_OVC_ISOURCE, strParameterName_Other, aryData_Other);
                    FPlanAssessmentSA.setFieldList(dr, dt_OVC_ISOURCE, "OVC_ISOURCE");//多筆
                    //預算科子目 OVC_POI_IBDG 多筆
                    DataTable dt_OVC_POI_IBDG = FCommon.getDataTableFromSelect(strSQL_OVC_POI_IBDG, strParameterName_Other, aryData_Other);
                    FPlanAssessmentSA.setFieldList(dr, dt_OVC_POI_IBDG, "OVC_POI_IBDG");//多筆
                }

                DataTable dt = FPlanAssessmentSA.getDataTable_Export(dt_Source, strFieldNames, strFieldSqls, out intCount_Data);
                
                if (intCount_Data > 0)// && false)
                {
                    string strSheetText = "1";
                    string strTitleText = "採購時程管制總表";
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
        //2
        protected void btnQuery_Uncheck_Click(object sender, EventArgs e)
        {
            if (strDEPT_SN != null)
            {
                string[] strParameterName = { ":strYear", ":USER_DEPT_SN" };
                ArrayList aryData = new ArrayList();

                string strYear = drpYear_Uncheck.SelectedValue;
                if (strYear.Length > 2)
                    strYear = strYear.Substring(strYear.Length - 2, 2);
                aryData.Add(strYear);
                aryData.Add(strDEPT_SN);
                //aryData.Add("175J5");

                #region SQL語法
                string strSQL = "";
                #region 欄位
                strSQL += $@"
                    select
                    a.OVC_PURCH||a.OVC_PUR_AGENCY||e.OVC_PURCH_5 PURCH, --//購案編號
                    a.OVC_PUR_IPURCH, --購案名稱
                    t_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --a.OVC_PUR_NSECTION, --//申購單位
                    t_PUR_ASS_VEN_CODE.OVC_PHR_DESC OVC_PUR_ASS_VEN_CODE, --NVL(a.OVC_PUR_ASS_VEN_CODE,' ') OVC_PUR_ASS_VEN_CODE, --//招標方式代碼
                    f.ONB_TIMES, --//總開標次數
                    d.OVC_DBEGIN, --//收辦日期
                    b.ONB_TENDER_DAYS, --//標準天數
                    case when f.OVC_RESULT is null then '' else to_char(to_date(f.OVC_DOPEN,'YYYY/MM/DD') - to_date(d.OVC_DBEGIN,'YYYY/MM/DD')) end PURCHASE_DAYS, --//採購天數 =決標日期(YYYY-MM-DD)-收辦日期(YYYY-MM-DD)
                    case when f.OVC_RESULT is null then '' else to_char((to_date(f.OVC_DOPEN,'YYYY/MM/DD') - to_date(d.OVC_DBEGIN,'YYYY/MM/DD')) - NVL(b.ONB_TENDER_DAYS,0)) end EXCEED_DAYS, --//超出天數 =採購天數(work_Days) - 標準天數(招標)(onb_Tender_Days)
                    case when f.OVC_RESULT is null or g.ONB_BUD_MONEY=0 then '' else to_char(g.ONB_BUD_MONEY) end ONB_BUD_MONEY, --//預算金額
                    a.ONB_PUR_BUDGET, --//全案總預算金額
                    case when f.OVC_RESULT is null then '' else f.OVC_DOPEN end OVC_DOPEN, --//決標日期
                    b.ONB_DELIVER_DAYS, --//交貨天數
                    c.OVC_COMM, --//重要事項
                    c.OVC_COMM_REASON, --猜的g.OVC_CONTRACT_COMM, --//檢討與因應措施
                    d.OVC_DO_NAME, --//承辦人

                    a.OVC_PURCH,
                    e.OVC_PURCH_5
                    ";
                #endregion
                #region 資料表
                strSQL += $@"
                    from
                    TBM1301 a,
                    TBM1301_PLAN b,
                    TBMRECEIVE_BID_LOG c,
                    VIWORKSTATUS21 d,
                    VIMAXSTATUS e,
                    VILASTTBM1303 f,
                    VITBM1302 g,

                    (SELECT OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PUR_SECTION, --//申購單位
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='C7') t_PUR_ASS_VEN_CODE --//招標方式
                    ";
                #endregion
                #region join 條件
                strSQL += $@"
                    where a.OVC_PURCH = b.OVC_PURCH
                    and a.OVC_PURCH = e.OVC_PURCH(+)
                    and e.OVC_PURCH = c.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = c.OVC_PURCH_5(+)
                    and e.OVC_PURCH = d.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = d.OVC_PURCH_5(+)
                    and e.OVC_PURCH = f.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = f.OVC_PURCH_5(+)
                    and e.OVC_PURCH = g.OVC_PURCH(+)
                    and e.OVC_PURCH_5 = g.OVC_PURCH_5(+)

                    and a.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    and a.OVC_PUR_ASS_VEN_CODE = t_PUR_ASS_VEN_CODE.OVC_PHR_ID(+)
                    ";
                #endregion
                #region 篩選條件
                //--+加欄位篩選(有選擇的條件(下方有說明)+
                #endregion
                #region 基本條件 & 排序
                if (strDEPT_SN.Equals("0A100"))
                {
                    strSQL += $@"
                        and a.OVC_AGNT_IN like '%軍備局採購中心%'
                        and (b.OVC_AUDIT_UNIT = { strParameterName[1] } --//計評單位=登錄者單位
                            or b.OVC_PUR_APPROVE_DEP = 'A') --//讀取採購中心權責購案(部核案)
                        ";
                }
                else
                    strSQL += $@"
                        and b.OVC_AUDIT_UNIT = { strParameterName[1] }
                        ";
                strSQL += $@"
                    and a.OVC_PUR_ALLOW is not null and a.OVC_PUR_ALLOW <> ' '
                    and a.OVC_DOING_UNIT = { strParameterName[1] } --//目前承辦單位=登錄者單位
                    and substr(a.OVC_PURCH,3,2) = { strParameterName[0] }
                    order by a.OVC_PUR_AGENCY, a.OVC_PURCH, g.OVC_PURCH_6
                    ";
                #endregion
                #region 額外欄位查詢語法
                #endregion
                string[] strFieldNames = { "購案編號", "購案名稱", "申購單位", "招標方式", "總開標次數", "收辦日期",
                    "標準天數", "辦理天數", "超出天數", "預算金額", "全案總預算金額", "決標日期",
                    "交貨天數", "重要事項", "檢討與因應措施", "承辦人" };
                string[] strFieldSqls = { "PURCH", "OVC_PUR_IPURCH", "OVC_PUR_SECTION", "OVC_PUR_ASS_VEN_CODE", "ONB_TIMES", "OVC_DBEGIN",
                    "ONB_TENDER_DAYS", "PURCHASE_DAYS", "EXCEED_DAYS", "ONB_BUD_MONEY", "ONB_PUR_BUDGET", "OVC_DOPEN",
                    "ONB_DELIVER_DAYS", "OVC_COMM", "OVC_CONTRACT_COMM", "OVC_DO_NAME" };
                string strFormatInt = Variable.strExcleFormatInt;
                string strFormatMoney2 = Variable.strExcleFormatMoney2;
                string[] strFormat = { null, null, null, null, strFormatInt, null,
                    strFormatInt, strFormatInt, strFormatInt, strFormatMoney2, strFormatMoney2, null,
                    strFormatInt, null, null, null };
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
                    //超出天數
                    string strEXCEED_DAYS = dr["EXCEED_DAYS"].ToString();
                    int intEXCEED_DAYS;
                    if (int.TryParse(strEXCEED_DAYS, out intEXCEED_DAYS) && intEXCEED_DAYS <= 0)
                        dr["EXCEED_DAYS"] = "";
                }

                DataTable dt = FPlanAssessmentSA.getDataTable_Export(dt_Source, strFieldNames, strFieldSqls, out intCount_Data);

                if (intCount_Data > 0)// && false)
                {
                    string strSheetText = "2";
                    string strTitleText = "採購已收辦未訂約統計表";
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
        //3
        protected void btnQuery_InnerPur_Click(object sender, EventArgs e)
        {
            Query_("L");
        }
        //4
        protected void btnQuery_OuterPur_Click(object sender, EventArgs e)
        {
            Query_("W");
        }
        #endregion
    }
}