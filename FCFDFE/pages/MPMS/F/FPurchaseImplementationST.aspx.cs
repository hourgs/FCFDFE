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
    public partial class FPurchaseImplementationST : Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        public string strDEPT_SN, strDEPT_Name;


        #region 副程式
        private void list_dataImport()
        {
            string strTestFirst = "請選擇－可空白";

            //年度
            FCommon.list_dataImportYear(drpYear);

            if (strDEPT_SN != null)
            {
                DataTable dt_DEPT = FCommon.getDataTableDEPT_Includesubordinate(strDEPT_SN);
                FCommon.list_dataImportV(drpOVC_PUR_SECTION, dt_DEPT, "OVC_ONNAME", "OVC_DEPT_CDE", strTestFirst, "", ":", true); //申購單位
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "單位代碼錯誤，請重新登入！");

            var query = gm.TBM1407;
            //核定權責
            DataTable dtOVC_PUR_APPROVE_DEP = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "E8").ToList());
            FCommon.list_dataImportV(drpOVC_PUR_APPROVE_DEP, dtOVC_PUR_APPROVE_DEP, "OVC_PHR_DESC", "OVC_PHR_ID", strTestFirst, "", ":", true);
            //採購屬性
            DataTable dtOVC_LAB = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "GN").ToList());
            FCommon.list_dataImportV(drpOVC_LAB, dtOVC_LAB, "OVC_PHR_DESC", "OVC_PHR_ID", strTestFirst, "", ":", true);
            //採購途徑
            DataTable dtOVC_PUR_AGENCY = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "C2").ToList());
            FCommon.list_dataImportV(drpOVC_PUR_AGENCY, dtOVC_PUR_AGENCY, "OVC_PHR_DESC", "OVC_PHR_ID", strTestFirst, "", ":", true);
            //招標方式
            DataTable dtOVC_PUR_ASS_VEN_CODE = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "C7").ToList());
            FCommon.list_dataImportV(drpOVC_PUR_ASS_VEN_CODE, dtOVC_PUR_ASS_VEN_CODE, "OVC_PHR_DESC", "OVC_PHR_ID", strTestFirst, "", ":", true);
            //計畫性質
            DataTable dtOVC_PLAN_PURCH = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "TD").ToList());
            FCommon.list_dataImportV(drpOVC_PLAN_PURCH, dtOVC_PLAN_PURCH, "OVC_PHR_DESC", "OVC_PHR_ID", strTestFirst, "", ":", true);
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
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_DPROPOSE1, txtOVC_DPROPOSE2, txtOVC_PUR_DAPPROVE1, txtOVC_PUR_DAPPROVE2);
                    list_dataImport();
                }
            }
        }

        #region OnClick
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (strDEPT_SN != null)
            {
                string[] strParameterName = { ":qry_YY", ":USER_DEPT_SN" };
                ArrayList aryData = new ArrayList();

                #region 年份篩選
                bool isYear = chkYear.Checked;
                string strYearSQL_where = "", strYearSQL_and = "";
                if (isYear) //篩選單一年份
                {
                    strYearSQL_where = $" where SUBSTR(OVC_PURCH,3,2) = { strParameterName[0] } ";
                    strYearSQL_and = $" and SUBSTR(OVC_PURCH,3,2) = { strParameterName[0] } ";
                    string strYear = drpYear.SelectedValue;
                    if (strYear.Length > 2)
                        strYear = strYear.Substring(strYear.Length - 2, 2);
                    aryData.Add(strYear);
                }
                #endregion
                aryData.Add(strDEPT_SN);

                #region SQL語法
                string strSQL = "";
                #region 欄位
                strSQL += $@"
                    select
                    a.OVC_PURCH||a.OVC_PUR_AGENCY||NVL(d.OVC_PURCH_5,' ')||NVL(d.OVC_PURCH_6,' ') PURCH, --//案號
                    a.OVC_PUR_IPURCH, --//購案名稱
                    t_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --a.OVC_PUR_NSECTION, --//申購單位
                    t_PUR_APPROVE_DEP.OVC_PHR_DESC OVC_PUR_APPROVE_DEP, --b.OVC_PUR_APPROVE_DEP, --//核定權責
                    t_PLAN_PURCH.OVC_PHR_DESC OVC_PLAN_PURCH, --a.OVC_PLAN_PURCH, --//計畫性質代碼
                    case when (a.OVC_PUR_AGENCY<>'M') then T_PUR_AGENCY.OVC_PHR_DESC else (T_PUR_AGENCY.OVC_PHR_DESC) end OVC_PUR_AGENCY, --//採購單位地區代碼(採購途徑)
                    t_LAB.OVC_PHR_DESC OVC_LAB, --a.OVC_LAB, --//採購屬性代碼
                    t_PUR_ASS_VEN_CODE.OVC_PHR_DESC OVC_PUR_ASS_VEN_CODE, --NVL(a.OVC_PUR_ASS_VEN_CODE,' ') OVC_PUR_ASS_VEN_CODE, --//招標方式代碼
                    NVL(b.OVC_OPEN_CHECK,'N') OVC_OPEN_CHECK, --//是否公開閱覽(Y/N)
                    '' OVC_LOWER_CHECK, --//異質最低標
                    case when L.vCase>0 then 'Y' else 'N' end OVC_ADVENTAGED_CHECK, --//準用最有利標
                    --//額外查詢 適用工業合作(ICP)
                    NVL(a.ONB_PUR_BUDGET_NT,0.0) ONB_PUR_BUDGET_NT, --//預算金額(台幣)
                    NVL(a.ONB_PUR_BUDGET,0.0) ONB_PUR_BUDGET, --//預算金額(原幣)
                    b.OVC_DAPPLY, --//預計申購日
                    a.OVC_DPROPOSE, --//申購日期
                    TO_CHAR(TO_DATE(TO_DATE(NVL(b.OVC_DAPPLY,b.OVC_PUR_CREAT),'YYYY-MM-DD') + b.ONB_REVIEW_DAYS),'YYYY-MM-DD') OVC_PLAN_PUR_DAPPROVE, --//原訂核定日期(TBM1301_PLAN  預計呈報日期＋審核天數)
                    NVL(a.OVC_PUR_DAPPROVE,' ') OVC_PUR_DAPPROVE, --//實際核定日期
                    case when e.OVC_RESULT is null or e.OVC_RESULT='0' then '' else NVL(e.OVC_RESULT_REASON,' ') end OVC_RESULT_REASON, --//未決標說明(無法決標理由)
                    NVL(e.OVC_DOPEN,' ') OVC_DOPEN, --//決標日期
                    NVL((e.ONB_BID_RESULT * e.ONB_RESULT_RATE),0.0)  ONB_MONEY_NT, --// 決標金額(台幣)
                    NVL(e.ONB_BID_RESULT,0.0) ONB_BID_RESULT , --// 決標金額(原幣)
                    NVL(d.OVC_DCONTRACT,' ') OVC_DCONTRACT, --//簽約日
                    NVL(d.OVC_VEN_TITLE,' ') OVC_VEN_TITLE, --//得標商
                    NVL(f.OVC_DELIVERY_CONTRACT,' ') OVC_DELIVERY_CONTRACT, --// 契約交貨日期
                    NVL(f.OVC_DELIVERY,' ') OVC_DELIVERY, --//實際交貨日期
                    '' OVC_EYE_DINSPECT, --//目視驗收日期
                    '' OVC_TEST_DINSPECT, --//性能測試日期
                    --//額外查詢 送化驗日期
                    '' OVC_TRAN_DATE, --//教育訓練日期
                    NVL(h.OVC_DPAY,' ') OVC_DPAY, --//最後付款日期 結算日期
                    NVL(h.OVC_DINSPECT_END,' ') OVC_DINSPECT_END, --//驗結日期 驗收完畢
                    case when a.OVC_DPROPOSE is null then '計畫編製階段' else 
                        case when a.OVC_PUR_DCANPO is null then '' else '本案已撤案' end
                    end OVC_STATUS, --//執行現狀與檢討
                    --//需軍種配合事項
                    case when a.OVC_PUR_DCANPO is null then 'N' else 'Y' end OVC_PUR_DCANPO_YN, --//是否撤案

                    a.OVC_PURCH,
                    NVL(d.OVC_PURCH_5,' ') OVC_PURCH_5,
                    NVL(d.OVC_PURCH_6,' ') OVC_PURCH_6,
                    NVL(d.OVC_VEN_CST,' ') OVC_VEN_CST, --//得標商代號
                    a.OVC_PERMISSION_UPDATE --//需軍種配合事項用到
                    ----以下沒用到----
                    --NVL(e.OVC_RESULT,' ') OVC_RESULT, --//開標結果(代碼A8)
                    --a.OVC_PUR_DCANPO,  --//撤案日
                    ";
                #endregion
                #region 資料表
                strSQL += $@"
                    from 
                    TBM1301 a,
                    TBM1301_PLAN b,
                    TBM1302 d,
                    (select k.OVC_PURCH,k.OVC_PURCH_5,NVL(k.ONB_GROUP,0) ONB_GROUP,
                        k.OVC_DOPEN,k.OVC_RESULT, NVL(k.ONB_BID_RESULT,0.0) ONB_BID_RESULT ,
                        NVL(k.ONB_RESULT_RATE,0.0) ONB_RESULT_RATE,k.OVC_RESULT_REASON
                      from TBM1303 k,
                        (select OVC_PURCH,OVC_PURCH_5,NVL(ONB_GROUP,0) ONB_GROUP,max(OVC_DOPEN) OVC_DOPEN 
                          from TBM1303 
                          { strYearSQL_where }
                          group by OVC_PURCH,OVC_PURCH_5,NVL(ONB_GROUP,0)) m 
                      where k.OVC_PURCH = m.OVC_PURCH and k.OVC_PURCH_5=m.OVC_PURCH_5 
                      and k.ONB_GROUP = m.ONB_GROUP and k.OVC_DOPEN = m.OVC_DOPEN ) e,
                    TBMDELIVERY f, 
                    (select n.*
                      from TBMPAY_MONEY n,
                        (select OVC_PURCH,OVC_PURCH_6,OVC_VEN_CST,max(OVC_DPAY) OVC_DPAY 
                          from TBMPAY_MONEY 
                          group by OVC_PURCH,OVC_PURCH_6,OVC_VEN_CST) O 
                      where n.OVC_PURCH = O.OVC_PURCH and n.OVC_PURCH_6=O.OVC_PURCH_6 
                      and n.OVC_VEN_CST = O.OVC_VEN_CST and n.OVC_DPAY = O.OVC_DPAY ) h, 

                    (SELECT OVC_PURCH, count(1) vCase from tbm1220_1 where ovc_ikind = 'A12' and ovc_check = 'ee' { strYearSQL_and } group by OVC_PURCH) l, --//準用最有利標

                    (SELECT OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PUR_SECTION, --//申購單位
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='E8') t_PUR_APPROVE_DEP, --//核定權責
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='TD') t_PLAN_PURCH, --//計畫性質
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='C2') t_PUR_AGENCY, --//採購途徑
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='GN') t_LAB, --//採購屬性
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='C7') t_PUR_ASS_VEN_CODE --//招標方式
                    ";
                #endregion
                #region join 條件
                strSQL += $@"
                    where a.OVC_PURCH = b.OVC_PURCH 
                    and a.OVC_PURCH = d.OVC_PURCH(+)
                    and d.OVC_PURCH = e.OVC_PURCH(+)
                    and d.OVC_PURCH_5 = e.OVC_PURCH_5(+)
                    and d.ONB_GROUP = e.ONB_GROUP(+) 
                    and d.OVC_PURCH = f.OVC_PURCH(+) 
                    and d.OVC_PURCH_6 = f.OVC_PURCH_6(+) 
                    and d.OVC_VEN_CST = f.OVC_VEN_CST(+) 
                    and d.OVC_PURCH = h.OVC_PURCH(+) 
                    and d.OVC_PURCH_6 = h.OVC_PURCH_6(+) 
                    and d.OVC_VEN_CST = h.OVC_VEN_CST(+) 
                    and a.OVC_PURCH = l.OVC_PURCH(+)

                    and a.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    and b.OVC_PUR_APPROVE_DEP = t_PUR_APPROVE_DEP.OVC_PHR_ID(+)
                    and a.OVC_PLAN_PURCH = t_PLAN_PURCH.OVC_PHR_ID(+)
                    and a.OVC_PUR_AGENCY = t_PUR_AGENCY.OVC_PHR_ID(+)
                    and a.OVC_LAB = t_LAB.OVC_PHR_ID(+)
                    and a.OVC_PUR_ASS_VEN_CODE = t_PUR_ASS_VEN_CODE.OVC_PHR_ID(+)

                    ";
                #endregion
                #region 篩選條件
                //--+加欄位篩選(有選擇的條件(下方有說明)+
                //年度
                if (isYear)
                    strSQL += $@"
                        and substr(a.OVC_PURCH, 3, 2) = { strParameterName[0] }
                        and substr(b.OVC_PURCH, 3, 2) = { strParameterName[0] }
                        ";
                //申購單位 drpOVC_PUR_SECTION 測試OK
                string strOVC_PUR_SECTION = drpOVC_PUR_SECTION.SelectedValue;
                if (chkOVC_PUR_SECTION.Checked && !strOVC_PUR_SECTION.Equals(string.Empty))
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
                if (chkOVC_PUR_APPROVE_DEP.Checked && !strOVC_PUR_APPROVE_DEP.Equals(string.Empty))
                    strSQL += $@"
                        and b.OVC_PUR_APPROVE_DEP='{ strOVC_PUR_APPROVE_DEP }'";
                //採購屬性 drpOVC_LAB 測試OK
                string strOVC_LAB = drpOVC_LAB.SelectedValue;
                if (chkOVC_LAB.Checked && !strOVC_LAB.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_LAB='{ strOVC_LAB }'";
                //採購途徑 drpOVC_PUR_AGENCY 測試OK
                string strOVC_PUR_AGENCY = drpOVC_PUR_AGENCY.SelectedValue;
                if (chkOVC_PUR_AGENCY.Checked && !strOVC_PUR_AGENCY.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_PUR_AGENCY='{ strOVC_PUR_AGENCY }'";
                //招標方式 drpOVC_PUR_ASS_VEN_CODE 測試OK
                string strOVC_PUR_ASS_VEN_CODE = drpOVC_PUR_ASS_VEN_CODE.SelectedValue;
                if (chkOVC_PUR_ASS_VEN_CODE.Checked && !strOVC_PUR_ASS_VEN_CODE.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_PUR_ASS_VEN_CODE='{ strOVC_PUR_ASS_VEN_CODE }'";
                //計畫性質 drpOVC_PLAN_PURCH 測試OK
                string strOVC_PLAN_PURCH = drpOVC_PLAN_PURCH.SelectedValue;
                if (chkOVC_PLAN_PURCH.Checked && !strOVC_PLAN_PURCH.Equals(string.Empty))
                    strSQL += $@"
                        and a.OVC_PLAN_PURCH='{ strOVC_PLAN_PURCH }'";
                //申購日期 txtOVC_DPROPOSE 實際申購日期測試OK
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
                //核定日期 txtOVC_PUR_DAPPROVE實際核定日期測試OK
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
                #endregion
                #region 基本條件 & 排序
                strSQL += $@"
                    and ( b.OVC_AUDIT_UNIT = { strParameterName[1] } --//計評單位=登錄者單位  
                      or  b.OVC_PURCHASE_UNIT = { strParameterName[1] } --//採購單位=登錄者單位
                      or  b.OVC_CONTRACT_UNIT = { strParameterName[1] } ) --//履驗單位=登錄者單位
                    and (b.OVC_DCANCEL is null or b.OVC_DCANCEL = ' ') --//預劃沒有撤案
                    order by a.OVC_PURCH,d.OVC_PURCH_5,d.OVC_PURCH_6
                    ";
                #endregion
                #region 額外欄位查詢語法
                string[] strParameterName_Other = { ":vOVC_PURCH" };
                ArrayList aryData_Other = new ArrayList();
                aryData_Other.Add("");
                //適用工業合作(ICP)
                string strSQL_ICP = $@"select
                    case when count(1)>0 then 'Y' else 'N' end ICP
                    from TBM1220_1 where OVC_PURCH={ strParameterName_Other[0] } and OVC_IKIND in('A1F','A2J','A36')
                ";
                //送化驗日期 OVC_DAPPLY_2
                string[] strParameterName_OVC_DAPPLY_2 = { ":vOVC_PURCH", ":vOVC_PURCH_6", ":vOVC_VEN_CST" };
                ArrayList aryData_OVC_DAPPLY_2 = new ArrayList();
                aryData_OVC_DAPPLY_2.Add("");
                aryData_OVC_DAPPLY_2.Add("");
                aryData_OVC_DAPPLY_2.Add("");
                string strSQL_OVC_DAPPLY_2 = $@"
                    select a.*
                    from TBMAPPLY_INSPECT a
                    where a.OVC_PURCH = { strParameterName_OVC_DAPPLY_2[0] }
                    and a.OVC_PURCH_6 = { strParameterName_OVC_DAPPLY_2[1] }
                    and a.OVC_VEN_CST = { strParameterName_OVC_DAPPLY_2[2] }
                    and a.ONB_TIMES = 0
                    and a.ONB_INSPECT_TIMES = 0
                    and a.ONB_RE_INSPECT_TIMES = 0
                ";
                //執行現狀與檢討 OVC_STATUS
                string strSQL_OVC_STATUS = $@"
                    select a.OVC_STATUS, T_STATUS.OVC_PHR_DESC
                    from 
                        (select max(NVL(a.OVC_STATUS,'N')) OVC_STATUS from TBMSTATUS a where a.OVC_PURCH = { strParameterName_Other[0] }) a,
                        (select OVC_PHR_ID, OVC_PHR_DESC from TBM1407 where OVC_PHR_CATE='Q9') T_STATUS
                    where a.OVC_STATUS=T_STATUS.OVC_PHR_ID(+)
                ";
                #endregion
                string[] strFieldNames = { "購案編號", "購案名稱", "申購單位", "核定權責", "計畫性質", "採購途徑", "採購屬性", "招標方式",
                    "需公開閱覽", "異質最低標", "準用最有利標", "適用工業合作(ICP)", "預算(台幣)", "預算(原幣)", "原訂申購日期", "實際申購日期",
                    "原訂核定日期", "實際核定日期", "未決標說明", "決標日期", "決標金額(台幣)", "決標金額(原幣)", "簽約日期", "得標廠商",
                    "合約交貨日期", "實際交貨日期", "目視驗收日期", "性能測試日期", "送化驗日期", "教育訓練日期", "最後付款日期", "驗結日期",
                    "執行現狀與檢討", "需軍種配合事項", "撤案" };
                string[] strFieldSqls = { "PURCH", "OVC_PUR_IPURCH", "OVC_PUR_SECTION", "OVC_PUR_APPROVE_DEP", "OVC_PLAN_PURCH", "OVC_PUR_AGENCY", "OVC_LAB", "OVC_PUR_ASS_VEN_CODE",
                    "OVC_OPEN_CHECK", "OVC_LOWER_CHECK", "OVC_ADVENTAGED_CHECK", "ICP", "ONB_PUR_BUDGET_NT", "ONB_PUR_BUDGET", "OVC_DAPPLY", "OVC_DPROPOSE",
                    "OVC_PLAN_PUR_DAPPROVE", "OVC_PUR_DAPPROVE", "OVC_RESULT_REASON", "OVC_DOPEN", "ONB_MONEY_NT", "ONB_BID_RESULT", "OVC_DCONTRACT", "OVC_VEN_TITLE",
                    "OVC_DELIVERY_CONTRACT", "OVC_DELIVERY", "OVC_EYE_DINSPECT", "OVC_TEST_DINSPECT", "OVC_DAPPLY_2", "OVC_TRAN_DATE", "OVC_DPAY", "OVC_DINSPECT_END",
                    "OVC_STATUS", "", "OVC_PUR_DCANPO_YN" };
                string strFormatInt = Variable.strExcleFormatInt;
                string strFormatMoney2 = Variable.strExcleFormatMoney2;
                string[] strFormat = { null, null, null, null, null, null, null, null,
                    null, null, null, null, strFormatMoney2, strFormatMoney2, null, null,
                    null, null, null, null, strFormatMoney2, strFormatMoney2, null, null,
                    null, null, null, null, null, null, null, null,
                    null, null, null };
                if (!isYear)
                    strParameterName = strParameterName.Where(str => str != strParameterName[0]).ToArray(); //移除年度項目
                #endregion

                //FCommon.AlertShow(PnMessage, "danger", "系統訊息", strSQL);
                //FCommon.AlertShow(PnMessage, "danger", "系統訊息", strSQL_OVC_DAPPLY_2);

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
                    bool isShow = true; //判斷額外篩選條件
                    if (!isShow)
                        dr.Delete();
                    else
                    {
                        string strOVC_PURCH = dr["OVC_PURCH"].ToString();
                        string strOVC_PURCH_6 = dr["OVC_PURCH_6"].ToString();
                        string strOVC_VEN_CST = dr["OVC_VEN_CST"].ToString();
                        aryData_Other[0] = strOVC_PURCH;
                        aryData_OVC_DAPPLY_2[0] = strOVC_PURCH;
                        aryData_OVC_DAPPLY_2[1] = strOVC_PURCH_6;
                        aryData_OVC_DAPPLY_2[2] = strOVC_VEN_CST;

                        DataTable dt_ICP = FCommon.getDataTableFromSelect(strSQL_ICP, strParameterName_Other, aryData_Other);
                        if (dt_ICP.Rows.Count > 0)
                            dr["ICP"] = dt_ICP.Rows[0]["ICP"];
                        DataTable dt_OVC_DAPPLY_2 = FCommon.getDataTableFromSelect(strSQL_OVC_DAPPLY_2, strParameterName_OVC_DAPPLY_2, aryData_OVC_DAPPLY_2);
                        if (dt_OVC_DAPPLY_2.Rows.Count > 0)
                            dr["OVC_DAPPLY_2"] = dt_OVC_DAPPLY_2.Rows[0]["OVC_DAPPLY_2"];
                        //執行現狀與檢討
                        string strOVC_STATUS = dr["OVC_STATUS"].ToString();
                        if (strOVC_STATUS.Equals(string.Empty))
                        {
                            DataTable dt_OVC_STATUS = FCommon.getDataTableFromSelect(strSQL_OVC_STATUS, strParameterName_Other, aryData_Other);
                            if (dt_OVC_STATUS.Rows.Count > 0)
                            {
                                string strOVC_PHR_ID = dt_OVC_STATUS.Rows[0]["OVC_STATUS"].ToString();
                                string strOVC_PHR_DESC = dt_OVC_STATUS.Rows[0]["OVC_PHR_DESC"].ToString();
                                if (strOVC_PHR_ID.Length > 0)
                                {
                                    string strChar = strOVC_PHR_ID.Substring(0, 1);
                                    if (strChar.Equals("2"))
                                        strOVC_STATUS = $"採購發包階段({ strOVC_PHR_DESC })";
                                    else if (strChar.Equals("3"))
                                        strOVC_STATUS = $"履約驗結階段({ strOVC_PHR_DESC })";
                                }
                            }
                            if (strOVC_STATUS.Equals(string.Empty))
                                strOVC_STATUS = "計畫評核階段";
                            string strOVC_PERMISSION_UPDATE = dr["OVC_PERMISSION_UPDATE"].ToString();
                            if (strOVC_PERMISSION_UPDATE.Equals("Y"))
                                strOVC_STATUS = strOVC_STATUS + "--退委辦(申購)單位澄覆或修訂中";
                            if (strOVC_STATUS.Length > dt_Source.Columns["OVC_STATUS"].MaxLength)
                                dt_Source.Columns["OVC_STATUS"].MaxLength = strOVC_STATUS.Length;
                            dr["OVC_STATUS"] = strOVC_STATUS;
                        }
                    }
                }
                dt_Source.AcceptChanges(); //將刪除列儲存，完成刪除動作

                DataTable dt = FCommon.getDataTable_Export(dt_Source, strFieldNames, strFieldSqls, true, out intCount_Data);

                if (intCount_Data > 0)// && false)
                {
                    string strSheetText = "2";
                    string strTitleText = "購案執行進度明細表";
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