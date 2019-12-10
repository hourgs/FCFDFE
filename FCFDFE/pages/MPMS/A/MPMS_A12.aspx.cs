using System;
using System.Data;
using FCFDFE.Content;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.A
{
    public partial class MPMS_A12 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string[] strField = { "OVC_BUDGET_YEAR", "OVC_BUDGET_MONTH", "OVC_POI_IBDG", "OVC_PJNAME", "ONB_MONEY" };
        //"OVC_P_SN", "OVC_PURCH", "OVC_BUDGET_YEAR", 
        string strPurchNum;

        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            if (Request.QueryString["PurchNum_11"] != null)
            {

                strPurchNum = Request.QueryString["PurchNum_11"].ToString();
                if (!strPurchNum.Equals(string.Empty))
                {
                    //設置readonly屬性
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_DPROPOSE, txtOVC_DAPPROVE, txtOVC_DCONTRACT, txtOVC_PUR_USER, txtOVC_PJNAME, txtONB_DELIVER_DATE, drpOVC_PLAN_PURCH, txtOVC_DCLOSE);
                    rdoONB_DELIVER_DAYS.Enabled = false;
                    rdoONB_DELIVER_DATE.Enabled = false;
                    if (!IsPostBack)
                    {
                        //初始畫面設定
                        LoginScreen();
                        dataImport_New();
                        TBM1301_PLAN plan1301 = new TBM1301_PLAN();
                        lblPrePurNum.Text = strPurchNum;
                        plan1301 = gm.TBM1301_PLAN.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                        if (plan1301 == null)
                        {
                            divCheck.Visible = false;
                            btnDel.Visible = false;
                        }
                        else
                        {
                            divCheck.Visible = true;
                            btnDel.Visible = false;
                            FCommon.GridView_setEmpty(GV_TBM1231_PLAN, strField);
                            list_dataImport(drpOVC_BUDGET_YEAR);
                        }
                    }
                }
            }
            else if (Request.QueryString["PurchNum_13"] != null)
            {
                strPurchNum = Request.QueryString["PurchNum_13"].ToString();
                if (!IsPostBack) {
                    //設置readonly屬性
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_DPROPOSE, txtOVC_DAPPROVE, txtOVC_DCONTRACT, txtOVC_PUR_USER, txtOVC_PJNAME, txtONB_DELIVER_DATE, drpOVC_PLAN_PURCH, txtOVC_DCLOSE);
                    rdoONB_DELIVER_DAYS.Enabled = false;
                    rdoONB_DELIVER_DATE.Enabled = false;
                    DetailsImport();
                    //將資料輸入GV
                    dataImport_New();
                    divCheck.Visible = true;
                }
                DateCal();
                list_dataImport(drpOVC_BUDGET_YEAR);

                TBM1301_PLAN plan1301 = new TBM1301_PLAN();
                plan1301 = gm.TBM1301_PLAN.Where(table => table.OVC_PURCH_OK == null).Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                if (plan1301 != null)
                {
                    btnDel.Visible = true;
                }
                else
                {
                    btnDel.Visible = false;
                }
            }
            else
                Response.Redirect("MPMS_A11");
        }
        #region Button OnClick
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //採購案預劃編製頁籤 存檔按鈕功能

            TBM1301_PLAN plan1301 = new TBM1301_PLAN();
            ACCOUNT ac = new ACCOUNT();
            //20181108_此處功能修改為:A11進來為新增，A13進來為修改
            if (Request.QueryString["PurchNum_13"] != null)
                plan1301 = gm.TBM1301_PLAN.Where(table => table.OVC_PURCH == strPurchNum).FirstOrDefault();
            //購案編號
            //plan1301.OVC_PURCH = lblPrePurNum.Text.ToString();
            if (Request.QueryString["PurchNum_11"] != null)
                plan1301.OVC_PURCH = lblPrePurNum.Text.ToString();
            //採購單位地區方式(代碼C2
            plan1301.OVC_PUR_AGENCY = drpOVC_PUR_AGENCY.SelectedValue.ToString();
            //購案名稱(中文)
            plan1301.OVC_PUR_IPURCH = txtOVC_PUR_IPURCH.Text.ToString();
            //預劃總金額
            //plan1301.ONB_PUR_BUDGET = ;  
            //預計呈報日期(YYYY-MM-DD)
            plan1301.OVC_DAPPLY = txtOVC_DPROPOSE.Text.ToString();
            //單位代碼
            //20181108_改為抓Session的單位代碼儲存
            //plan1301.OVC_PUR_SECTION = ac.DEPT_SN;
            string userdept= Session["dept"].ToString();
            plan1301.OVC_PUR_SECTION = Session["dept"].ToString();
            //單位全銜
            //plan1301.OVC_PUR_NSECTION = 
            TBMDEPT dept = new TBMDEPT();
            dept = gm.TBMDEPTs.Where(x => x.OVC_DEPT_CDE.Equals(userdept)).FirstOrDefault();
            plan1301.OVC_PUR_NSECTION = dept.OVC_ONNAME;
            //申購人
            plan1301.OVC_PUR_USER = txtOVC_PUR_USER.Text.ToString();
            //鍵入者
            plan1301.OVC_KEYIN = txtOVC_PUR_USER.Text.ToString();
            //聯絡電話(自動)
            plan1301.OVC_PUR_IUSER_PHONE = txtOVC_PUR_IUSER_PHONE.Text.ToString();
            //傳真號碼
            plan1301.OVC_PUR_IUSER_FAX = txtOVC_PUR_IUSER_FAX.Text.ToString();
            //聯絡電話(軍線1)
            plan1301.OVC_PUR_IUSER_PHONE_EXT = txtOVC_PUR_IUSER_PHONE_EXT.Text.ToString();
            //聯絡電話(軍線2)
            plan1301.OVC_PUR_IUSER_PHONE_EXT1 = txtOVC_PUR_IUSER_PHONE_EXT_1.Text.ToString();
            //聯絡電話(手機)
            plan1301.OVC_USER_CELLPHONE = txtOVC_USER_CELLPHONE.Text.ToString();
            //E-MAIL
            plan1301.EMAIL_ACCOUNT = txtEMAIL_ACCOUNT.Text.ToString();
            //招標方式(代碼C7)
            plan1301.OVC_PUR_ASS_VEN_CODE = drpOVC_PUR_ASS_VEN_CODE.SelectedValue.ToString();
            //採購屬性(財物勞務代碼GN)
            plan1301.OVC_LAB = drpOVC_LAB.SelectedValue.ToString();
            //計畫性質(1->計劃2->非計劃)
            plan1301.OVC_PLAN_PURCH = drpOVC_PLAN_PURCH.SelectedValue.ToString();
            //核定權責(代碼E8)
            plan1301.OVC_PUR_APPROVE_DEP = drpOVC_PUR_APPROVE_DEP.SelectedValue.ToString();
            //專案代號
            //plan1301.OVC_PUR_PROJE = 
            //審核天數(沒有值就帶0)
            plan1301.ONB_REVIEW_DAYS = Convert.ToInt16(txtONB_REVIEW_DAYS.Text.ToString());
            //交貨天數
            plan1301.ONB_DELIVER_DAYS = Convert.ToInt16(txtONB_DELIVER_DAYS.Text.ToString());
            //招標天數
            plan1301.ONB_TENDER_DAYS = Convert.ToInt16(drpOVC_TENDER_DAYS.SelectedValue.ToString());
            //驗結天數
            plan1301.ONB_RECEIVE_DAYS = Convert.ToInt16(drpOVC_RECEIVE_DAYS.SelectedValue.ToString());
            //管制日(YYYY-MM-DD)
            //plan1301.OVC_DAUDIT =
            //管制承辦人
            //plan1301.OVC_AUDITOR = 
            //建檔日(YYYY-MM-DD)
            plan1301.OVC_PUR_CREAT = System.DateTime.Now.ToString("yyyy/MM/dd");
            //是否已經建案(Y/N)
            plan1301.OVC_PURCH_OK = null;
            //撤案日(YYYY-MM-DD)
            //plan1301.OVC_DCANCEL =
            //撤案原因
            //plan1301.OVC_CANCEL_REASON =
            //招標天數代碼(代碼M0)
            //plan1301.OVC_TENDER_DAYS = 
            //驗結天數代碼(代碼M1)
            //plan1301.OVC_RECEIVE_DAYS =
            //計畫評核單位代碼
            plan1301.OVC_AUDIT_UNIT = txtOVC_AUDIT_UNIT.Text.ToString();
            //採購發包單位代碼
            plan1301.OVC_PURCHASE_UNIT = txtOVC_PURCHASE_UNIT.Text.ToString();
            //履約驗結單位代碼
            plan1301.OVC_CONTRACT_UNIT = txtOVC_CONTRACT_UNIT.Text.ToString();
            //委託單位代碼
            plan1301.OVC_AGENT_UNIT = txtOVC_AGENT_UNIT.Text.ToString();
            //FAC是否需資審(Y/N)
            //plan1301.OVC_FAC_CHECK =
            //是否公開閱覽(Y/N)
            //plan1301.OVC_OPEN_CHECK
            //是否為1月1日須執行之購案(Y/N)
            plan1301.OVC_ON_SCHEDULE = drpOVC_ON_SCHEDULE.SelectedValue.ToString();
            //美軍案號(前2碼為TW)
            plan1301.OVC_PURCH_MILITARY = lblOVC_PURCH_MILITARY.Text.ToString();
            //軍售案類別(1->個別式2->開放式
            plan1301.OVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedValue.ToString();

            //20181108_此處功能修改為:A11進來為新增，A13進來為修改
            if(Request.QueryString["PurchNum_11"] != null)
                gm.TBM1301_PLAN.Add(plan1301);
            gm.SaveChanges();
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");

            divCheck.Visible = true;
            list_dataImport(drpOVC_BUDGET_YEAR);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            //採購案預劃編製頁籤 清除按鈕功能
            //清除欄位
            FCommon.Controls_Clear(txtOVC_PUR_IPURCH, txtOVC_PUR_IUSER_PHONE_EXT, txtOVC_PUR_IUSER_PHONE_EXT_1, txtOVC_PUR_IUSER_PHONE, txtOVC_USER_CELLPHONE, txtEMAIL_ACCOUNT, txtOVC_PUR_IUSER_FAX, txtOVC_DPROPOSE, txtOVC_DAPPROVE, txtOVC_DCONTRACT, txtONB_DELIVER_DATE, txtOVC_DCLOSE);
            //恢復初始畫面
            LoginScreen();
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            //預算頁籤 新增按鈕功能
            int CalDateYear = Convert.ToInt16(DateTime.Now.ToString("yyyy")) - 1911;
            string strMessage = "";
            string strOVC_PURCH = lblPrePurNum.Text;
            string strOVC_BUDGET_YEAR = drpOVC_BUDGET_YEAR.SelectedItem.Text.ToString();
            string strOVC_BUDGET_MONTH = drpOVC_BUDGET_MONTH.SelectedValue.ToString();
            string strOVC_POI_IBDG = drpOVC_POI_IBDG.SelectedValue.ToString();
            string strOVC_IKIND = rdoBudgetType.SelectedValue.ToString();
            string strOVC_PJNAME = txtOVC_PJNAME.Text;
            int intONB_MONEY = 0;
            if (txtONB_MONEY.Text != "")
                intONB_MONEY = Convert.ToInt32(txtONB_MONEY.Text);


            if (strOVC_PURCH.Equals(string.Empty))
                strMessage += "<P> 請先 確認預劃編號 </p>";
            if (strOVC_BUDGET_MONTH.Equals(string.Empty))
                strMessage += "<P> 請先選擇 月份 </p>";
            if (strOVC_POI_IBDG.Equals(string.Empty))
                strMessage += "<P> 請選擇 預算科目代號 </p>";
            if (strOVC_IKIND.Equals(string.Empty))
                strMessage += "<P> 請先選擇 預算科目類別 </p>";
            if (strOVC_PJNAME.Equals(string.Empty))
                strMessage += "<P> 請輸入 預算科目名稱 </p>";
            if (intONB_MONEY.Equals(0))
                strMessage += "<P> 請輸入 預劃金額 </p>";

            if (strMessage.Equals(string.Empty))
            {
                TBM1231_PLAN plan1231query = new TBM1231_PLAN();
                plan1231query = mpms.TBM1231_PLAN.Where(table => table.OVC_PURCH.Equals(strOVC_PURCH) && table.OVC_BUDGET_YEAR.Equals(strOVC_BUDGET_YEAR) && table.OVC_BUDGET_MONTH.Equals(strOVC_BUDGET_MONTH) && table.OVC_POI_IBDG.Equals(strOVC_POI_IBDG) && table.OVC_PJNAME.Equals(strOVC_PJNAME)).FirstOrDefault();
                if (plan1231query == null)
                {
                    TBM1231_PLAN plan1231 = new TBM1231_PLAN();
                    plan1231.OVC_P_SN = Guid.NewGuid();
                    plan1231.OVC_PURCH = strOVC_PURCH;
                    plan1231.OVC_BUDGET_YEAR = strOVC_BUDGET_YEAR;
                    plan1231.OVC_BUDGET_MONTH = strOVC_BUDGET_MONTH;
                    plan1231.OVC_POI_IBDG = strOVC_POI_IBDG;
                    plan1231.OVC_IKIND = strOVC_IKIND;
                    plan1231.OVC_PJNAME = strOVC_PJNAME;
                    plan1231.ONB_MONEY = intONB_MONEY;

                    mpms.TBM1231_PLAN.Add(plan1231);
                    mpms.SaveChanges();
                    FCommon.AlertShow(PnMessage_AccountAuth, "success", "系統訊息", "新增成功，請等候審核");
                }
                else
                {
                    FCommon.AlertShow(PnMessage_AccountAuth, "danger", "系統訊息", "該預購明細已存在");
                }
                dataImport_New();
            }
            else
                FCommon.AlertShow(PnMessage_AccountAuth, "danger", "系統訊息", strMessage);
        }
        protected void btnDel_Click(object sender, EventArgs e)
        {
            //A13跳轉後,若 OVC_PURCH_OK=null 出現之刪除紐

            //新增
            TBM1301D d1301 = new TBM1301D();
            d1301.OVC_D_SN = Guid.NewGuid();
            d1301.OVC_PURCH = strPurchNum;// lblPrePurNum.Text;
            d1301.OVC_NPURCH = "";
            d1301.OVC_KIND = "D";
            d1301.OVC_USER = txtOVC_PUR_USER.Text;
            d1301.OVC_DDATE = DateTime.Now.ToString();
            mpms.TBM1301D.Add(d1301);
            mpms.SaveChanges();

            //刪除
            TBM1301_PLAN plan1301 = new TBM1301_PLAN();
            plan1301 = gm.TBM1301_PLAN.Where(table => table.OVC_PURCH == strPurchNum).FirstOrDefault();
            gm.Entry(plan1301).State = EntityState.Deleted;
            gm.SaveChanges();
            FCommon.AlertShow(PnMessage_AccountAuth, "success", "系統訊息", "預畫購案刪除成功。");
            Response.Redirect("MPMS_A13");


        }
        protected void btnOVC_PUR_APPROVE_DEP_Click(object sender, EventArgs e)
        {
            //核定權責說明
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('依據軍事機關財務、勞務採購計畫核定權責區分表，\\n\\n案屬『國防部核定』權責者，請點選A，\\n\\n案屬「國防部授權單位自行核定」權責說明權責者，請點選B，\\n\\n案屬「國防部授權單位自行核定」再下授權權責者，請點玄C，\\n\\n非屬上述三種情形者，請點選X。');", true);
            return;
        }

        protected void btnexp_OVC_AGENT_UNIT_Click(object sender, EventArgs e)
        {
            //委託單位說明
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('各軍種委託聯勤司令部採購之零附件採購案，\\n\\n為使各軍種能即時掌握執行進度，\\n\\n請聯勤司令部承辦上述案件「採購計畫」及「物資申請書」之承辦人員，\\n\\n於本欄點選原委託單位之『單位代碼』，以利各軍種查詢。');", true);
            return;
        }

        protected void btnQuery_OVC_AGENT_UNIT_Click(object sender, EventArgs e)
        {
            //委託單位，單位查詢
            Session["unitquery"] = "query5";
        }

        protected void btnAudit_Click(object sender, EventArgs e)
        {
            //最後計畫評核(審查)單位，單位查詢
            Session["unitquery"] = "query2";
        }

        protected void btnQueryOVC_PURCHASE_UNIT_Click(object sender, EventArgs e)
        {
            //採購發包，單位查詢
            Session["unitquery"] = "query3";
        }

        protected void btnQueryOVC_CONTRACT_UNIT_Click(object sender, EventArgs e)
        {
            //履約驗結，單位查詢
            Session["unitquery"] = "query4";
        }

        #endregion

        #region OnPreRender
        protected void GV_TBM1231_PLAN_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        #endregion

        #region OnRowCommand
        protected void GV_TBM1231_PLAN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            Guid id = new Guid(GV_TBM1231_PLAN.DataKeys[gvrIndex].Value.ToString()); //OVC_P_SN

            switch (e.CommandName)
            {
                case "DataDel":
                    //新增
                    TBM1301D d1301 = new TBM1301D();
                    d1301.OVC_D_SN = Guid.NewGuid();
                    d1301.OVC_PURCH = lblPrePurNum.Text;
                    d1301.OVC_NPURCH = "";
                    d1301.OVC_KIND = "D";
                    d1301.OVC_USER = txtOVC_PUR_USER.Text;
                    d1301.OVC_DDATE = DateTime.Now.ToString();
                    mpms.TBM1301D.Add(d1301);
                    mpms.SaveChanges();

                    //刪除
                    TBM1231_PLAN plan1231 = new TBM1231_PLAN();
                    plan1231 = mpms.TBM1231_PLAN.Where(table => table.OVC_PURCH == strPurchNum).FirstOrDefault();
                    mpms.Entry(plan1231).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    dataImport_New();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region On~~~Changed
        protected void drpOVC_PUR_AGENCY_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strOVC_PUR_AGENCY = drpOVC_PUR_AGENCY.SelectedValue.ToString();
            var query = gm.TBM1407;
            DataTable dtOVC_PUR_ASS_VEN_CODE;
            //依照採購單位地及方式連動招標方式
            switch (strOVC_PUR_AGENCY)
            {
                case "F":
                    dtOVC_PUR_ASS_VEN_CODE = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "C7").Where(table => table.OVC_PHR_ID.Equals("F")).ToList());
                    FCommon.list_dataImportV(drpOVC_PUR_ASS_VEN_CODE, dtOVC_PUR_ASS_VEN_CODE, "OVC_PHR_DESC", "OVC_PHR_ID", ":", false);
                    txtOVC_PURCH_MILITARY.Visible = false;
                    lblDescription.Visible = false;
                    break;
                case "M":
                    dtOVC_PUR_ASS_VEN_CODE = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "C7").Where(table => table.OVC_PHR_ID.Equals("C") || table.OVC_PHR_ID.Equals("D") || table.OVC_PHR_ID.Equals("E")).ToList());
                    FCommon.list_dataImportV(drpOVC_PUR_ASS_VEN_CODE, dtOVC_PUR_ASS_VEN_CODE, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
                    txtOVC_PURCH_MILITARY.Visible = true;
                    lblDescription.Visible = true;
                    break;
                case "P":
                    dtOVC_PUR_ASS_VEN_CODE = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "C7").Where(table => table.OVC_PHR_ID.Equals("F")).ToList());
                    FCommon.list_dataImportV(drpOVC_PUR_ASS_VEN_CODE, dtOVC_PUR_ASS_VEN_CODE, "OVC_PHR_DESC", "OVC_PHR_ID", ":", false);
                    txtOVC_PURCH_MILITARY.Visible = false;
                    lblDescription.Visible = false;
                    break;
                case "S":
                    dtOVC_PUR_ASS_VEN_CODE = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "C7").Where(table => table.OVC_PHR_ID.Equals("C") || table.OVC_PHR_ID.Equals("D") || table.OVC_PHR_ID.Equals("E")).ToList());
                    FCommon.list_dataImportV(drpOVC_PUR_ASS_VEN_CODE, dtOVC_PUR_ASS_VEN_CODE, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
                    txtOVC_PURCH_MILITARY.Visible = false;
                    lblDescription.Visible = false;
                    break;
                default:
                    dtOVC_PUR_ASS_VEN_CODE = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "C7").ToList());
                    FCommon.list_dataImportV(drpOVC_PUR_ASS_VEN_CODE, dtOVC_PUR_ASS_VEN_CODE, "OVC_PHR_DESC", "OVC_PHR_ID", ":", false);
                    txtOVC_PURCH_MILITARY.Visible = false;
                    lblDescription.Visible = false;
                    break;
            }
        }

        protected void txtOVC_DPROPOSE_TextChanged(object sender, EventArgs e)
        {
            //預計申辦日期(=預計呈報日期)，內容值改變時
            DateCal();
        }
        protected void txtONB_REVIEW_DAYS_TextChanged(object sender, EventArgs e)
        {
            //審核天數，內容值改變時
            DateCal();
        }
        protected void drpOVC_TENDER_DAYS_SelectedIndexChanged(object sender, EventArgs e)
        {
            //招標天數，內容值改變時
            DateCal();
        }
        protected void txtONB_DELIVER_DAYS_TextChanged(object sender, EventArgs e)
        {
            //交貨天數，內容值改變時
            rdoONB_DELIVER_DAYS.Checked = true;
            rdoONB_DELIVER_DATE.Checked = false;
            DateCal();
        }
        protected void txtONB_DELIVER_DATE_TextChanged(object sender, EventArgs e)
        {
            //限定交貨日期，內容值改變時
            rdoONB_DELIVER_DAYS.Checked = false;
            rdoONB_DELIVER_DATE.Checked = true;
            DateCal();
        }
        protected void drpOVC_RECEIVE_DAYS_SelectedIndexChanged(object sender, EventArgs e)
        {
            //驗結天數，內容值改變時
            DateCal();
        }

        protected void rdoBudgetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //根據預算類別更動 預算科目代號下拉選單選項
            var query = gm.TBM1407;
            if (rdoBudgetType.SelectedValue == "1")
            {
                DataTable dtOVC_POI_IBDG = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "L8").ToList());
                FCommon.list_dataImportV(drpOVC_POI_IBDG, dtOVC_POI_IBDG, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            }
            else
            {
                DataTable dtOVC_POI_IBDG = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "L9").ToList());
                FCommon.list_dataImportV(drpOVC_POI_IBDG, dtOVC_POI_IBDG, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            }
        }
        protected void drpOVC_POI_IBDG_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectItem = drpOVC_POI_IBDG.SelectedItem.ToString();
            Char delimiter = ':';
            string[] item = selectItem.Split(delimiter);
            txtOVC_PJNAME.Text = item[1];
        }
        #endregion

        #region 副程式

        private void LoginScreen()
        {
            //初始畫面設定
            //承辦人姓名，預設帶入登入者姓名
            if (Session["userid"] != null)
            {
                string strUSER_ID = Session["userid"].ToString();
                ACCOUNT ac = new ACCOUNT();
                ac = gm.ACCOUNTs.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                if (ac != null)
                {
                    string userName = ac.USER_NAME;
                    txtOVC_PUR_USER.Text = userName;
                }
            }

            var query = gm.TBM1407;
            //採購單位地區及方式
            DataTable dtOVC_PUR_AGENCY = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "C2").ToList());
            FCommon.list_dataImportV(drpOVC_PUR_AGENCY, dtOVC_PUR_AGENCY, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            drpOVC_PUR_AGENCY.SelectedValue = "L";
            //美軍案號
            txtOVC_PURCH_MILITARY.Visible = false;
            lblDescription.Visible = false;
            //招標方式
            DataTable dtOVC_PUR_ASS_VEN_CODE = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "C7").ToList());
            FCommon.list_dataImportV(drpOVC_PUR_ASS_VEN_CODE, dtOVC_PUR_ASS_VEN_CODE, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            drpOVC_PUR_ASS_VEN_CODE.SelectedValue = "A";
            //計畫性質
            DateTime today = DateTime.Now;
            int thisYear = Convert.ToInt16(DateTime.Now.ToString("yyyy"));
            DateTime standard = new DateTime(thisYear, 1, 31);
            DataTable dtOVC_PLAN_PURCH = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "TD").ToList());
            FCommon.list_dataImportV(drpOVC_PLAN_PURCH, dtOVC_PLAN_PURCH, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            if (DateTime.Compare(today, standard) <= 0)
            {
                drpOVC_PLAN_PURCH.SelectedValue = "1";
            }
            else
            {
                drpOVC_PLAN_PURCH.SelectedValue = "2";
            }
            //專業代號(適用裝備)
            txtOVC_PUR_PROJE.Text = "無";
            //核定權責
            DataTable dtOVC_PUR_APPROVE_DEP = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "E8").ToList());
            FCommon.list_dataImportV(drpOVC_PUR_APPROVE_DEP, dtOVC_PUR_APPROVE_DEP, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            drpOVC_PUR_APPROVE_DEP.SelectedValue = "A";
            //是否1月1日須執行之購案
            FCommon.list_dataImportYN(drpOVC_ON_SCHEDULE, false, true);
            drpOVC_ON_SCHEDULE.SelectedValue = "N";
            //採購屬性
            DataTable dtOVC_LAB = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "GN").ToList());
            FCommon.list_dataImportV(drpOVC_LAB, dtOVC_LAB, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            drpOVC_LAB.SelectedValue = "1";
            //委託單位代號
            txtOVC_AGENT_UNIT.Text = "00N00";
            //委託單位單位名稱
            txtOVC_AGENT_UNIT_exp.Text = "國防部國防採購室";
            //最後計畫評核單位代號
            txtOVC_AUDIT_UNIT.Text = "00N00";
            //最後計畫評核單位單位名稱
            txtOVC_AUDIT_UNIT_1.Text = "國防部國防採購室";
            //採購發包單位代號
            txtOVC_PURCHASE_UNIT.Text = "00N00";
            //採購發包單位單位名稱
            txtOVC_PURCHASE_UNIT_1.Text = "國防部國防採購室";
            //履約驗結單位代號
            txtOVC_CONTRACT_UNIT.Text = "00N00";
            //履約驗結單位名稱
            txtOVC_CONTRACT_UNIT_1.Text = "國防部國防採購室";
            //預計申辦日期
            txtOVC_DPROPOSE.Text = System.DateTime.Now.ToString("yyyy/MM/dd");
            //審核天數
            txtONB_REVIEW_DAYS.Text = "60";
            //招標天數
            DataTable dtOVC_TENDER_DAYS = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "M0").ToList());
            FCommon.list_dataImportV(drpOVC_TENDER_DAYS, dtOVC_TENDER_DAYS, "OVC_PHR_DESC", "OVC_USR_ID", ":", true);
            drpOVC_TENDER_DAYS.SelectedValue = "45";
            //交貨天數
            rdoONB_DELIVER_DAYS.Checked = true;
            txtONB_DELIVER_DAYS.Text = "365";
            //驗結天數
            DataTable dtOVC_RECEIVE_DAYS = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "M1").ToList());
            FCommon.list_dataImportV(drpOVC_RECEIVE_DAYS, dtOVC_RECEIVE_DAYS, "OVC_PHR_DESC", "OVC_USR_ID", ":", true);
            drpOVC_RECEIVE_DAYS.SelectedValue = "0";
            //國防預算L8非國防預算L9
            rdoBudgetType.SelectedValue = "1";
            DataTable dtOVC_POI_IBDG = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "L8").ToList());
            FCommon.list_dataImportV(drpOVC_POI_IBDG, dtOVC_POI_IBDG, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            DateCal();
        }
        private void DateCal()
        {
            //取得預計呈報日期
            string strOVC_DAPPLY = txtOVC_DPROPOSE.Text;
            //取得審核天數
            string strONB_REVIEW_DAYS = txtONB_REVIEW_DAYS.Text;
            //取得招標天數
            string strOVC_TENDER_DAYS = drpOVC_TENDER_DAYS.SelectedValue.ToString();
            //取得交貨天數
            string strONB_DELIVER_DAYS = "0";
            if (rdoONB_DELIVER_DAYS.Checked)
            {
                strONB_DELIVER_DAYS = txtONB_DELIVER_DAYS.Text;
            }
            //取得驗結天數
            string strOVC_RECEIVE_DAYS = drpOVC_RECEIVE_DAYS.SelectedValue.ToString();
            //若三個必要欄位都有值才進入計算
            if (!strOVC_DAPPLY.Equals(string.Empty) && !strONB_REVIEW_DAYS.Equals(string.Empty) && !strOVC_TENDER_DAYS.Equals(string.Empty) && !strONB_DELIVER_DAYS.Equals(string.Empty) && !strOVC_RECEIVE_DAYS.Equals(string.Empty))
            {
                //將審核天數轉換為數字型態
                int intONB_REVIEW_DAYS = Convert.ToInt16(strONB_REVIEW_DAYS);
                //將預計呈報日期轉成日期格式
                DateTime dateOVC_DAPPLY = Convert.ToDateTime(strOVC_DAPPLY);
                //將預計核定日期初始設定為預計呈報日期
                DateTime dateOVC_DAPPROVE = dateOVC_DAPPLY;
                //取得預計核定日=預計呈報日期+審核天數 
                for (int i = 0; i < intONB_REVIEW_DAYS; i++)
                {
                    dateOVC_DAPPROVE = dateOVC_DAPPROVE.AddDays(1);
                    while (dateOVC_DAPPROVE.DayOfWeek == System.DayOfWeek.Saturday || dateOVC_DAPPROVE.DayOfWeek == System.DayOfWeek.Sunday)
                    {
                        dateOVC_DAPPROVE = dateOVC_DAPPROVE.AddDays(1);
                    }
                }
                txtOVC_DAPPROVE.Text = dateOVC_DAPPROVE.ToString("yyyy-MM-dd");

                //將招標天數轉換為數字型態
                int intOVC_TENDER_DAYS = Convert.ToInt16(drpOVC_TENDER_DAYS.SelectedValue.ToString());
                //將預計簽約日期初始設定為預計核定日期
                DateTime dateOVC_DCONTRACT = dateOVC_DAPPROVE;
                //取得預計簽約日=預計核定日期+招標天數
                for (int i = 0; i < intOVC_TENDER_DAYS; i++)
                {
                    dateOVC_DCONTRACT = dateOVC_DCONTRACT.AddDays(1);
                    while (dateOVC_DCONTRACT.DayOfWeek == System.DayOfWeek.Saturday || dateOVC_DCONTRACT.DayOfWeek == System.DayOfWeek.Sunday)
                    {
                        dateOVC_DCONTRACT = dateOVC_DCONTRACT.AddDays(1);
                    }
                }
                txtOVC_DCONTRACT.Text = dateOVC_DCONTRACT.ToString("yyyy-MM-dd");

                //將交貨天數轉換為數字型態
                int intONB_DELIVER_DAYS = Convert.ToInt16(strONB_DELIVER_DAYS);
                //將驗結天數轉換為數字型態
                int intOVC_RECEIVE_DAYS = Convert.ToInt16(drpOVC_RECEIVE_DAYS.SelectedValue.ToString());
                int plus = intONB_DELIVER_DAYS;// + intOVC_RECEIVE_DAYS;

                DateTime dateDELIVE = DateTime.MinValue;
                string hasdelivedate = "Y";
                //取得交貨日期、預計結案日
                if (rdoONB_DELIVER_DAYS.Checked)
                {
                    //將預計結案日期初始設定為預計簽約日期
                    dateDELIVE = dateOVC_DCONTRACT;
                    for (int i = 0; i < plus; i++)
                    {
                        dateDELIVE = dateDELIVE.AddDays(1);
                        while (dateDELIVE.DayOfWeek == System.DayOfWeek.Saturday || dateDELIVE.DayOfWeek == System.DayOfWeek.Sunday)
                        {
                            dateDELIVE = dateDELIVE.AddDays(1);
                        }
                    }

                }
                else if (!txtONB_DELIVER_DATE.Text.Equals(string.Empty))
                {
                    dateDELIVE = Convert.ToDateTime(txtONB_DELIVER_DATE.Text);
                }
                else {
                    hasdelivedate = "N";
                }
                if (hasdelivedate == "Y")
                {
                    for (int i = 0; i < intOVC_RECEIVE_DAYS; i++)
                    {
                        dateDELIVE = dateDELIVE.AddDays(1);
                        while (dateDELIVE.DayOfWeek == System.DayOfWeek.Saturday || dateDELIVE.DayOfWeek == System.DayOfWeek.Sunday)
                        {
                            dateDELIVE = dateDELIVE.AddDays(1);
                        }
                    }
                    txtOVC_DCLOSE.Text = dateDELIVE.ToString("yyyy-MM-dd");
                }
                else
                    txtOVC_DCLOSE.Text = "";
            }
        }
        private void dataImport_New()
        {
            //將資料輸入GV

            DataTable dt = new DataTable();
            string strUSER_ID = Session["userid"].ToString();
            if (strUSER_ID.Length > 0)
            {
                var query =
                    from plan1231 in mpms.TBM1231_PLAN.DefaultIfEmpty().AsEnumerable()
                    where plan1231.OVC_PURCH.Equals(strPurchNum)
                    select new
                    {
                        OVC_P_SN = plan1231.OVC_P_SN,
                        OVC_PURCH = plan1231.OVC_PURCH,
                        OVC_BUDGET_YEAR = plan1231.OVC_BUDGET_YEAR,
                        OVC_BUDGET_MONTH = plan1231.OVC_BUDGET_MONTH,
                        OVC_POI_IBDG = plan1231.OVC_POI_IBDG,
                        OVC_PJNAME = plan1231.OVC_PJNAME,
                        ONB_MONEY = plan1231.ONB_MONEY,
                    };
                dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBM1231_PLAN, dt, strField);
            }
        }
        private void list_dataImport(ListControl list)
        {
            //年度下拉選單
            //先將下拉式選單清空
            list.Items.Clear();

            int CalDateYear = Convert.ToInt16(DateTime.Now.ToString("yyyy")) - 1911;
            int num = CalDateYear;
            for (int i = 0; i < 15; i++)
            {
                list.Items.Add(Convert.ToString(num));
                num = num - 1;
            }
        }

        private void DetailsImport()
        {
            //將資料庫資料帶出至畫面

            TBM1301_PLAN plan1301 = new TBM1301_PLAN();
            strPurchNum = Request.QueryString["PurchNum_13"].ToString();
            plan1301 = gm.TBM1301_PLAN.Where(table => table.OVC_PURCH == strPurchNum).FirstOrDefault();
            var query = gm.TBM1407;
            //購案編號
            lblPrePurNum.Text = plan1301.OVC_PURCH;
            //採購單位地區方式(代碼C2
            DataTable dtOVC_PUR_AGENCY = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "C2").ToList());
            FCommon.list_dataImportV(drpOVC_PUR_AGENCY, dtOVC_PUR_AGENCY, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            drpOVC_PUR_AGENCY.SelectedValue = plan1301.OVC_PUR_AGENCY;
            //購案名稱(中文)
            txtOVC_PUR_IPURCH.Text = plan1301.OVC_PUR_IPURCH;
            //美軍案號
            if (plan1301.OVC_PUR_AGENCY == "M")
            {
                txtOVC_PURCH_MILITARY.Visible = true;
                lblDescription.Visible = true;
            }
            else
            {
                txtOVC_PURCH_MILITARY.Visible = false;
                lblDescription.Visible = false;
            }

            //預劃總金額
            //plan1301.ONB_PUR_BUDGET = ;  
            //預計呈報日期(YYYY-MM-DD)
            txtOVC_DPROPOSE.Text = plan1301.OVC_DAPPLY;
            //單位代碼
            //單位全銜
            //plan1301.OVC_PUR_NSECTION = 
            //申購人
            txtOVC_PUR_USER.Text = plan1301.OVC_PUR_USER;
            //鍵入者
            txtOVC_PUR_USER.Text = plan1301.OVC_KEYIN;
            //聯絡電話(自動)
            txtOVC_PUR_IUSER_PHONE.Text = plan1301.OVC_PUR_IUSER_PHONE;
            //傳真號碼
            txtOVC_PUR_IUSER_FAX.Text = plan1301.OVC_PUR_IUSER_FAX;
            //聯絡電話(軍線1)
            txtOVC_PUR_IUSER_PHONE_EXT.Text = plan1301.OVC_PUR_IUSER_PHONE_EXT;
            //聯絡電話(軍線2)
            txtOVC_PUR_IUSER_PHONE_EXT_1.Text = plan1301.OVC_PUR_IUSER_PHONE_EXT1;
            //聯絡電話(手機
            txtOVC_USER_CELLPHONE.Text = plan1301.OVC_USER_CELLPHONE;
            //E-MAIL
            txtEMAIL_ACCOUNT.Text = plan1301.EMAIL_ACCOUNT;
            //招標方式(代碼C7
            DataTable dtOVC_PUR_ASS_VEN_CODE = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "C7").ToList());
            FCommon.list_dataImportV(drpOVC_PUR_ASS_VEN_CODE, dtOVC_PUR_ASS_VEN_CODE, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            drpOVC_PUR_ASS_VEN_CODE.SelectedValue = plan1301.OVC_PUR_ASS_VEN_CODE;
            //採購屬性(財物勞務代碼GN)
            DataTable dtOVC_LAB = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "GN").ToList());
            FCommon.list_dataImportV(drpOVC_LAB, dtOVC_LAB, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            drpOVC_LAB.SelectedValue = plan1301.OVC_LAB;
            //計畫性質(1->計劃2->非計劃)
            DataTable dtOVC_PLAN_PURCH = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "TD").ToList());
            FCommon.list_dataImportV(drpOVC_PLAN_PURCH, dtOVC_PLAN_PURCH, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            drpOVC_PLAN_PURCH.SelectedValue = plan1301.OVC_PLAN_PURCH;
            //核定權責(代碼E8)
            DataTable dtOVC_PUR_APPROVE_DEP = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "E8").ToList());
            FCommon.list_dataImportV(drpOVC_PUR_APPROVE_DEP, dtOVC_PUR_APPROVE_DEP, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            drpOVC_PUR_APPROVE_DEP.SelectedValue = plan1301.OVC_PUR_APPROVE_DEP;
            //專案代號
            //plan1301.OVC_PUR_PROJE = 
            //審核天數(沒有值就帶0)
            txtONB_REVIEW_DAYS.Text = plan1301.ONB_REVIEW_DAYS.ToString();
            //交貨天數
            txtONB_DELIVER_DAYS.Text = plan1301.ONB_DELIVER_DAYS.ToString();
            //招標天數
            DataTable dtOVC_TENDER_DAYS = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "M0").ToList());
            FCommon.list_dataImportV(drpOVC_TENDER_DAYS, dtOVC_TENDER_DAYS, "OVC_PHR_DESC", "OVC_USR_ID", ":", true);
            drpOVC_TENDER_DAYS.SelectedValue = plan1301.ONB_TENDER_DAYS.ToString();
            //驗結天數
            DataTable dtOVC_RECEIVE_DAYS = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "M1").ToList());
            FCommon.list_dataImportV(drpOVC_RECEIVE_DAYS, dtOVC_RECEIVE_DAYS, "OVC_PHR_DESC", "OVC_USR_ID", ":", true);
            drpOVC_RECEIVE_DAYS.SelectedValue = plan1301.ONB_RECEIVE_DAYS.ToString();
            //管制日(YYYY-MM-DD)
            //plan1301.OVC_DAUDIT =
            //管制承辦人
            //plan1301.OVC_AUDITOR = 
            //建檔日(YYYY-MM-DD)
            //plan1301.OVC_PUR_CREAT = System.DateTime.Now.ToString("yyyy/MM/dd");
            //是否已經建案(Y/N)
            //plan1301.OVC_PURCH_OK = "N";
            //撤案日(YYYY-MM-DD)
            //plan1301.OVC_DCANCEL =
            //撤案原因
            //plan1301.OVC_CANCEL_REASON =
            //招標天數代碼(代碼M0)
            //plan1301.OVC_TENDER_DAYS = 
            //驗結天數代碼(代碼M1)
            //plan1301.OVC_RECEIVE_DAYS =
            //計畫評核單位代碼
            txtOVC_AUDIT_UNIT.Text = plan1301.OVC_AUDIT_UNIT;
            //計畫評核單位名稱
            TBMDEPT deptOVC_AUDIT_UNIT = new TBMDEPT();
            string strOVC_AUDIT_UNIT;
            if (plan1301.OVC_AUDIT_UNIT != null)
            {
                strOVC_AUDIT_UNIT = plan1301.OVC_AUDIT_UNIT;
                deptOVC_AUDIT_UNIT = gm.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(strOVC_AUDIT_UNIT)).FirstOrDefault();
            }
            else
            {
                strOVC_AUDIT_UNIT = "";
                deptOVC_AUDIT_UNIT.OVC_ONNAME = "";
            }
            txtOVC_AUDIT_UNIT_1.Text = deptOVC_AUDIT_UNIT.OVC_ONNAME;
            //採購發包單位代碼
            txtOVC_PURCHASE_UNIT.Text = plan1301.OVC_PURCHASE_UNIT;
            //採購發包單位名稱
            TBMDEPT deptOVC_PURCHASE_UNIT = new TBMDEPT();
            string strOVC_PURCHASE_UNIT;
            if (plan1301.OVC_PURCHASE_UNIT != null)
            {
                strOVC_PURCHASE_UNIT = plan1301.OVC_PURCHASE_UNIT;
                deptOVC_PURCHASE_UNIT = gm.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(strOVC_PURCHASE_UNIT)).FirstOrDefault();
            }
            else
            {
                strOVC_PURCHASE_UNIT = "";
                deptOVC_PURCHASE_UNIT.OVC_ONNAME = "";
            }
            txtOVC_PURCHASE_UNIT_1.Text = deptOVC_PURCHASE_UNIT.OVC_ONNAME;
            //履約驗結單位代碼
            txtOVC_CONTRACT_UNIT.Text = plan1301.OVC_CONTRACT_UNIT;
            //履約驗結單位名稱
            TBMDEPT deptOVC_CONTRACT_UNIT = new TBMDEPT();
            string strOVC_CONTRACT_UNIT;
            if (plan1301.OVC_CONTRACT_UNIT != null)
            {
                strOVC_CONTRACT_UNIT = plan1301.OVC_CONTRACT_UNIT;
                deptOVC_CONTRACT_UNIT = gm.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(strOVC_CONTRACT_UNIT)).FirstOrDefault();
            }
            else
            {
                strOVC_CONTRACT_UNIT = "";
                deptOVC_CONTRACT_UNIT.OVC_ONNAME = "";
            }
            txtOVC_CONTRACT_UNIT_1.Text = deptOVC_CONTRACT_UNIT.OVC_ONNAME;
            //委託單位代碼
            txtOVC_AGENT_UNIT.Text = plan1301.OVC_AGENT_UNIT;
            //委託單位名稱
            TBMDEPT deptOVC_AGENT_UNIT = new TBMDEPT();
            string strOVC_AGENT_UNIT;
            if (plan1301.OVC_AGENT_UNIT != null)
            {
                strOVC_AGENT_UNIT = plan1301.OVC_AGENT_UNIT;
                deptOVC_AGENT_UNIT = gm.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(strOVC_AGENT_UNIT)).FirstOrDefault();
            }
            else
            {
                strOVC_AGENT_UNIT = "";
                deptOVC_AGENT_UNIT.OVC_ONNAME = "";
            }
            txtOVC_AGENT_UNIT_exp.Text = deptOVC_AGENT_UNIT.OVC_ONNAME;
            //FAC是否需資審(Y/N)
            //plan1301.OVC_FAC_CHECK =
            //是否公開閱覽(Y/N)
            //plan1301.OVC_OPEN_CHECK
            //是否為1月1日須執行之購案(Y/N)
            FCommon.list_dataImportYN(drpOVC_ON_SCHEDULE, false, true);
            drpOVC_ON_SCHEDULE.SelectedValue = plan1301.OVC_ON_SCHEDULE;
            //美軍案號(前2碼為TW)
            lblOVC_PURCH_MILITARY.Text = plan1301.OVC_PURCH_MILITARY;
            //軍售案類別(1->個別式2->開放式
            drpOVC_MILITARY_TYPE.SelectedValue = plan1301.OVC_MILITARY_TYPE;


            TBM1231_PLAN plan1231 = new TBM1231_PLAN();
            //根據預算類別更動 預算科目代號下拉選單選項
            rdoBudgetType.SelectedValue = "1";
            DataTable dtOVC_POI_IBDG = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "L8").ToList());
            FCommon.list_dataImportV(drpOVC_POI_IBDG, dtOVC_POI_IBDG, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);

            //計算日期
            DateCal();
        }
        #endregion
    }
}