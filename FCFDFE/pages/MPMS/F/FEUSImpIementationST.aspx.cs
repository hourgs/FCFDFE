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
    public partial class FEUSImpIementationST : Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        public string strDEPT_SN, strDEPT_Name;

        #region 副程式
        private void list_dataImport()
        {
            //年度
            FCommon.list_dataImportYear(drpYear);
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
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_DELIVERY1, txtOVC_DELIVERY2, txtOVC_DJOINCHECK1, txtOVC_DJOINCHECK2, txtOVC_DPAY1, txtOVC_DPAY2, txtOVC_DCLOSE1, txtOVC_DCLOSE2);
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
                if (isYear) //篩選單一年份
                {
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
                    a.OVC_PURCH,
                    NVL(d.OVC_PURCH_5,' ') OVC_PURCH_5,
                    NVL(d.OVC_PURCH_6,' ') OVC_PURCH_6,
                    a.OVC_PURCH||a.OVC_PUR_AGENCY||NVL(d.OVC_PURCH_5,' ')||NVL(d.OVC_PURCH_6,' ') PURCH, --//案號
                    a.OVC_PUR_IPURCH, --//購案名稱
                    t_PUR_SECTION.OVC_ONNAME OVC_PUR_SECTION, --a.OVC_PUR_NSECTION, --//申購單位
                    t_PUR_APPROVE_DEP.OVC_PHR_DESC OVC_PUR_APPROVE_DEP, --b.OVC_PUR_APPROVE_DEP, --//核定權責
                    a.OVC_DPROPOSE, --//申購日期
                    NVL(a.OVC_PUR_DAPPROVE,' ') OVC_PUR_DAPPROVE, --//實際核定日期
                    NVL(e.OVC_DOPEN,' ') OVC_DOPEN, --//決標日期
                    NVL(d.OVC_DCONTRACT,' ') OVC_DCONTRACT, --//簽約日
                    NVL(d.OVC_VEN_TITLE,' ') OVC_VEN_TITLE, --//得標商
                    NVL(f.OVC_DELIVERY,' ') OVC_DELIVERY, --//實際交貨日期
                    NVL(h.OVC_DINSPECT_END,' ') OVC_DINSPECT_END, --//履驗日期 驗收完畢/合格日期
                    --NVL(f.OVC_DJOINCHECK,' ') OVC_DJOINCHECK, --//會驗日期
                    NVL(h.OVC_DPAY,' ') OVC_DPAY, --//結算日期(=最後付款日期?)
                    NVL(e.OVC_RESULT_REASON,' ') OVC_RESULT_REASON, --//未決標說明(無法決標理由)
                    --NVL(e.OVC_RESULT,' ') OVC_RESULT, --//開標結果(代碼A8)
                    NVL(e.ONB_BID_RESULT,0.0) ONB_BID_RESULT, --// 決標金額(原幣)
                    NVL((e.ONB_BID_RESULT * e.ONB_RESULT_RATE),0.0) ONB_MONEY_NT, --// 決標金額(台幣)
                    c.OVC_RECEIVE_COMM --// 重要事項記載
                    ";
                #endregion
                #region 資料表
                strSQL += $@"
                    from 
                    TBM1301 a,
                    TBM1301_PLAN b,
                    TBMRECEIVE_CONTRACT c,
                    TBM1302 d,
                    (select k.OVC_PURCH,k.OVC_PURCH_5,NVL(k.ONB_GROUP,0) ONB_GROUP, k.OVC_DOPEN,k.OVC_RESULT, NVL(k.ONB_BID_RESULT,0.0) ONB_BID_RESULT , NVL(k.ONB_RESULT_RATE,0.0) ONB_RESULT_RATE,k.OVC_RESULT_REASON
                        from TBM1303 K,
                        (select OVC_PURCH,OVC_PURCH_5,NVL(ONB_GROUP,0) ONB_GROUP,max(OVC_DOPEN) OVC_DOPEN from TBM1303 
                            group by OVC_PURCH,OVC_PURCH_5,ONB_GROUP   ) m 
                        where k.OVC_PURCH = m.OVC_PURCH and k.OVC_PURCH_5=m.OVC_PURCH_5 
                        and k.ONB_GROUP = m.ONB_GROUP and k.OVC_DOPEN = m.OVC_DOPEN ) e,
                    TBMDELIVERY f, 
                    (select n.*
                        from TBMPAY_MONEY n,
                        (select OVC_PURCH,OVC_PURCH_6,OVC_VEN_CST,max(OVC_DPAY) OVC_DPAY 
                            from TBMPAY_MONEY 
                            group by OVC_PURCH,OVC_PURCH_6,OVC_VEN_CST) o
                        where n.OVC_PURCH = o.OVC_PURCH and n.OVC_PURCH_6=o.OVC_PURCH_6 
                        and n.OVC_VEN_CST = o.OVC_VEN_CST and n.OVC_DPAY = o.OVC_DPAY ) h, 

                    (SELECT OVC_DEPT_CDE, OVC_ONNAME FROM TBMDEPT) t_PUR_SECTION, --//申購單位
                    (SELECT OVC_PHR_ID, OVC_PHR_DESC FROM TBM1407 WHERE OVC_PHR_CATE='E8') t_PUR_APPROVE_DEP --//核定權責
                    ";
                #endregion
                #region join 條件
                strSQL += $@"
                    where a.OVC_PURCH = b.OVC_PURCH 
                    and a.OVC_PURCH = d.OVC_PURCH(+) 
                    and d.OVC_PURCH = e.OVC_PURCH(+) 
                    and d.OVC_PURCH_5 = e.OVC_PURCH_5(+) 
                    and d.ONB_GROUP = e.ONB_GROUP(+) 
                    and d.OVC_PURCH = c.OVC_PURCH(+) 
                    and d.OVC_PURCH_6 = c.OVC_PURCH_6(+) 
                    and d.OVC_VEN_CST = c.OVC_VEN_CST(+) 
                    and d.OVC_PURCH = f.OVC_PURCH(+) 
                    and d.OVC_PURCH_6 = f.OVC_PURCH_6(+) 
                    and d.OVC_VEN_CST = f.OVC_VEN_CST(+) 
                    and d.OVC_PURCH = h.OVC_PURCH(+) 
                    and d.OVC_PURCH_6 = h.OVC_PURCH_6(+) 
                    and d.OVC_VEN_CST = h.OVC_VEN_CST(+) 

                    and a.OVC_PUR_SECTION = t_PUR_SECTION.OVC_DEPT_CDE(+)
                    and b.OVC_PUR_APPROVE_DEP = t_PUR_APPROVE_DEP.OVC_PHR_ID(+)
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
                //交貨日期 txtOVC_DELIVERY
                if (chkOVC_DELIVERY.Checked)
                {
                    string strOVC_DELIVERY1 = txtOVC_DELIVERY1.Text;
                    if (!strOVC_DELIVERY1.Equals(string.Empty))
                        strSQL += $@"
                        and f.OVC_DELIVERY >= '{ strOVC_DELIVERY1 }'
                    ";
                    string strOVC_DELIVERY2 = txtOVC_DELIVERY2.Text;
                    if (!strOVC_DELIVERY2.Equals(string.Empty))
                        strSQL += $@"
                        and f.OVC_DELIVERY <= '{ strOVC_DELIVERY2 }'
                    ";
                }
                //履驗日期 篩選會驗日期 txtOVC_DJOINCHECK
                if (chkOVC_DJOINCHECK.Checked)
                {
                    string strOVC_DJOINCHECK1 = txtOVC_DJOINCHECK1.Text;
                    if (!strOVC_DJOINCHECK1.Equals(string.Empty))
                        strSQL += $@"
                        and f.OVC_DJOINCHECK >= '{ strOVC_DJOINCHECK1 }'
                    ";
                    string strOVC_DJOINCHECK2 = txtOVC_DJOINCHECK2.Text;
                    if (!strOVC_DJOINCHECK2.Equals(string.Empty))
                        strSQL += $@"
                        and f.OVC_DJOINCHECK <= '{ strOVC_DJOINCHECK2 }'
                    ";
                }
                //最後付款日期 txtOVC_DPAY
                if (chkOVC_DPAY.Checked)
                {
                    string strOVC_DPAY1 = txtOVC_DPAY1.Text;
                    if (!strOVC_DPAY1.Equals(string.Empty))
                        strSQL += $@"
                        and h.OVC_DPAY >= '{ strOVC_DPAY1 }'
                    ";
                    string strOVC_DPAY2 = txtOVC_DPAY2.Text;
                    if (!strOVC_DPAY2.Equals(string.Empty))
                        strSQL += $@"
                        and h.OVC_DPAY <= '{ strOVC_DPAY2 }'
                    ";
                }
                //驗結日期 txtOVC_DCLOSE
                if (chkOVC_DCLOSE.Checked)
                {
                    string strOVC_DCLOSE1 = txtOVC_DCLOSE1.Text;
                    if (!strOVC_DCLOSE1.Equals(string.Empty))
                        strSQL += $@"
                        and c.OVC_DCLOSE >= '{ strOVC_DCLOSE1 }'
                    ";
                    string strOVC_DCLOSE2 = txtOVC_DCLOSE2.Text;
                    if (!strOVC_DCLOSE2.Equals(string.Empty))
                        strSQL += $@"
                        and c.OVC_DCLOSE <= '{ strOVC_DCLOSE2 }'
                    ";
                }
                #endregion
                #region 基本條件 & 排序
                strSQL += $@"
                    and ( b.OVC_AUDIT_UNIT = { strParameterName[1] } --//計評單位=登錄者單位  
                        or b.OVC_PURCHASE_UNIT = { strParameterName[1] } --//採購單位=登錄者單位
                        or b.OVC_CONTRACT_UNIT = { strParameterName[1] } ) --//履驗單位=登錄者單位
                    and (a.OVC_PUR_AGENCY = 'C' or a.OVC_PUR_AGENCY = 'E') --//採購單位地區為駐美、歐組
                    and (b.OVC_DCANCEL is null or b.OVC_DCANCEL = ' ') --//預劃沒有撤案
                    order by a.OVC_PURCH,d.OVC_PURCH_5,d.OVC_PURCH_6
                    ";
                #endregion
                #region 額外欄位查詢語法
                #endregion
                if (!isYear)
                    strParameterName = strParameterName.Where(str => str != strParameterName[0]).ToArray(); //移除年度項目
                string[] strFieldNames = { "購案編號", "購案名稱", "申購單位", "核定權責", "申購日期", "核定日期", "未決標說明", "決標日期",
                    "決標金額(台幣)", "決標金額(原幣)", "簽約日期", "得標廠商", "交貨日期", "最後付款日期", "履驗日期", "備考" };
                string[] strFieldSqls = { "PURCH", "OVC_PUR_IPURCH", "OVC_PUR_SECTION", "OVC_PUR_APPROVE_DEP", "OVC_DPROPOSE", "OVC_PUR_DAPPROVE", "OVC_RESULT_REASON", "OVC_DOPEN",
                    "ONB_MONEY_NT", "ONB_BID_RESULT", "OVC_DCONTRACT", "OVC_VEN_TITLE", "OVC_DELIVERY", "OVC_DPAY", "OVC_DINSPECT_END", "OVC_RECEIVE_COMM" };
                string strFormatInt = Variable.strExcleFormatInt;
                string strFormatMoney2 = Variable.strExcleFormatMoney2;
                string[] strFormat = { null, null, null, null, null, null, null, null,
                    strFormatMoney2, strFormatMoney2, null, null, null, null, null, null };
                #endregion

                //FCommon.AlertShow(PnMessage, "danger", "系統訊息", strSQL);
                DataTable dt_Source = FCommon.getDataTableFromSelect(strSQL, strParameterName, aryData);
                int intCount_Data;
                if (false)
                {
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

                            //dr["OVC_PUR_AGENCY"] = dr["OVC_PUR_AGENCY"].ToString() + dr["OVC_MILITARY_TYPE"].ToString();

                        }
                    }
                    dt_Source.AcceptChanges(); //將刪除列儲存，完成刪除動作
                }

                DataTable dt = FPlanAssessmentSA.getDataTable_Export(dt_Source, strFieldNames, strFieldSqls, out intCount_Data);

                if (intCount_Data > 0)// && false)
                {
                    string strSheetText = "4";
                    string strTitleText = "駐美、歐組購案履驗階段執行現況明細表";
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