using System;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Linq;

namespace FCFDFE.pages.MPMS.B
{
    public partial class MPMS_B15 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strPurchNum = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (Request.QueryString["PurchNum"] != null)
                {
                    strPurchNum = Request.QueryString["PurchNum"];
                    if (!strPurchNum.Equals(string.Empty))
                    {
                        if (!IsPostBack)
                        {
                            FCommon.Controls_Attributes("readonly", "true", txtOVC_DAPPLY, txtOVC_TO_UNIT_NAME, txtOVC_TO_UNIT_NAME_1, txtOVC_AUDIT_UNIT, txtOVC_AUDIT_UNIT_1
                                , txtOVC_PURCHASE_UNIT, txtOVC_PURCHASE_UNIT_1, txtOVC_CONTRACT_UNIT, txtOVC_CONTRACT_UNIT_1);
                            LoginScreen();
                        }
                    }
                }
                else
                {
                    Response.Redirect("MPMS_B11");
                }
            }
        }

        #region onclick
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //確認按鈕功能
            SaveData();
          
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            //清除按鈕功能
            FCommon.Controls_Clear(txtOVC_PURCH, lblOVC_PUR_AGENCY, lblOVC_PUR_IPURCH, lblOVC_PUR_NSECTION, lblBrackets_1, lblOVC_PUR_SECTION, lblBrackets_2, lblConnect, lblOVC_PUR_USER, lblBrackets_3, lblOVC_PUR_IUSER_PHONE_EXT, lblBrackets_4 , lblOVC_BID, lblOVC_BID_METHOD_1 , lblOVC_BID_TIMES , lblOVC_LAB, lblOVC_PUR_ASS_VEN_CODE , lblOVC_BID_MONEY, txtOVC_DAPPLY , lblOVC_PROPOSE , txtOVC_APPLY_CHIEF);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            //查詢按鈕功能
        }
        #endregion

        #region 副程式

        private void LoginScreen() {
            txtOVC_PURCH.Text = strPurchNum;
            TBM1301 table1301 = new TBM1301();
            table1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            if (table1301!=null) {
                //申辦單位(代碼)-申購人(電話)
                lblOVC_PUR_NSECTION.Text = table1301.OVC_PUR_NSECTION;
                lblOVC_PUR_SECTION.Text = table1301.OVC_PUR_SECTION;
                lblOVC_PUR_USER.Text = table1301.OVC_PUR_USER;
                lblOVC_PUR_IUSER_PHONE_EXT.Text = table1301.OVC_PUR_IUSER_PHONE_EXT;
                txtOVC_DAPPLY.Text = table1301.OVC_DPROPOSE;
            }
            TBM1301_PLAN table1301plan = new TBM1301_PLAN();
            table1301plan = gm.TBM1301_PLAN.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            if (table1301plan != null) {
                //購案名稱
                lblOVC_PUR_IPURCH.Text = table1301plan.OVC_PUR_IPURCH;
                //計劃評核(審查)單位

                //最後計劃評核(審查)單位
                txtOVC_AUDIT_UNIT.Text = "";
                txtOVC_AUDIT_UNIT_1.Text = "";
                //採購發包單位
                txtOVC_PURCHASE_UNIT.Text = table1301plan.OVC_PURCHASE_UNIT;
                
                //履約驗結單位單位
                txtOVC_CONTRACT_UNIT.Text = table1301plan.OVC_CONTRACT_UNIT;
                txtOVC_CONTRACT_UNIT_1.Text = "";
            }
            //單位代碼帶出單位中文名稱
            //計劃評核(審查)單位
            //最後計劃評核(審查)單位
            TBMDEPT dept = new TBMDEPT();
            dept = gm.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(txtOVC_PURCHASE_UNIT.Text)).FirstOrDefault();
            if (dept != null)
            {
                txtOVC_PURCHASE_UNIT_1.Text = dept.OVC_ONNAME;
            }
            //採購發包單位
            TBMDEPT deptOVC_PURCHASE_UNIT = new TBMDEPT();
            deptOVC_PURCHASE_UNIT = gm.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(txtOVC_PURCHASE_UNIT.Text)).FirstOrDefault();
            if (deptOVC_PURCHASE_UNIT != null) {
                txtOVC_PURCHASE_UNIT_1.Text = deptOVC_PURCHASE_UNIT.OVC_ONNAME;
            }
            //履約驗結單位單位
            TBMDEPT deptOVC_CONTRACT_UNIT = new TBMDEPT();
            deptOVC_CONTRACT_UNIT = gm.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(txtOVC_PURCHASE_UNIT.Text)).FirstOrDefault();
            if (deptOVC_CONTRACT_UNIT != null) {
                txtOVC_CONTRACT_UNIT_1.Text = deptOVC_CONTRACT_UNIT.OVC_ONNAME;
            }

            //採購單位地區方式(代碼C2)
            TBM1407 tb1407C2 = new TBM1407();
            tb1407C2 = gm.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("C2")).Where(table => table.OVC_PHR_ID.Equals(table1301.OVC_PUR_AGENCY)).FirstOrDefault();
            if (tb1407C2 != null) {
                lblOVC_PUR_AGENCY.Text = tb1407C2.OVC_PHR_ID + ":" + tb1407C2.OVC_PHR_DESC;
            }
            //決標原則(代碼M3)
            TBM1407 tb1407M3 = new TBM1407();
            tb1407M3 = gm.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("M3")).Where(table => table.OVC_PHR_ID.Equals(table1301.OVC_BID)).FirstOrDefault();
            if (tb1407M3 != null)
            {
                lblOVC_BID.Text = tb1407M3.OVC_PHR_ID + ":" + tb1407M3.OVC_PHR_DESC;
            }
            //投標段次(代碼TG)
            TBM1407 tb1407TG = new TBM1407();
            tb1407TG = gm.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("TG")).Where(table => table.OVC_PHR_ID.Equals(table1301.OVC_BID_TIMES)).FirstOrDefault();
            if (tb1407TG != null)
            {
                lblOVC_BID_TIMES.Text = tb1407TG.OVC_PHR_ID + ":" + tb1407TG.OVC_PHR_DESC;
            }
            //採購屬性(財物勞務代碼GN
            TBM1407 tb1407GN = new TBM1407();
            tb1407GN = gm.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("GN")).Where(table => table.OVC_PHR_ID.Equals(table1301.OVC_LAB)).FirstOrDefault();
            if (tb1407GN != null)
            {
                lblOVC_LAB.Text = tb1407GN.OVC_PHR_ID + ":" + tb1407GN.OVC_PHR_DESC;
            }
        }

        private void SaveData()
        {
            //存檔
            string dateNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt");
            var query1118 = mpms.TBM1118.Where(o => o.OVC_PURCH.Equals(strPurchNum));
            foreach (var result in query1118)
            {
                TBM1118_HISTORY tbm1118 = new TBM1118_HISTORY();
                tbm1118.OVC_PURCH = result.OVC_PURCH;
                tbm1118.OVC_ISOURCE = result.OVC_ISOURCE;
                tbm1118.OVC_POI_IBDG = result.OVC_POI_IBDG;
                tbm1118.OVC_IKIND = result.OVC_IKIND;
                tbm1118.OVC_PJNAME = result.OVC_PJNAME;
                tbm1118.OVC_YY = result.OVC_YY;
                tbm1118.OVC_MM = result.OVC_MM;
                tbm1118.ONB_MBUD = result.ONB_MBUD;
                tbm1118.OVC_CURRENT = result.OVC_CURRENT;
                tbm1118.ONB_RATE = result.ONB_RATE;
                tbm1118.OVC_VERSION = dateNow;
                mpms.TBM1118_HISTORY.Add(tbm1118);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , tbm1118.GetType().Name.ToString(), this, "新增");
            }
            var query1119 = mpms.TBM1119.Where(o => o.OVC_PURCH.Equals(strPurchNum));
            foreach (var result in query1119)
            {
                TBM1119_HISTORY tbm1119 = new TBM1119_HISTORY();
                tbm1119.OVC_PURCH = result.OVC_PURCH;
                tbm1119.OVC_IKIND = result.OVC_IKIND;
                tbm1119.OVC_ATTACH_NAME = result.OVC_ATTACH_NAME;
                tbm1119.OVC_FILE_NAME = result.OVC_FILE_NAME;
                tbm1119.ONB_QTY = result.ONB_QTY;
                tbm1119.ONB_PAGES = result.ONB_PAGES;
                tbm1119.OVC_VERSION = dateNow;
                mpms.TBM1119_HISTORY.Add(tbm1119);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , tbm1119.GetType().Name.ToString(), this, "新增");
            }
            var query1201 = mpms.TBM1201.Where(o => o.OVC_PURCH.Equals(strPurchNum));
            foreach (var result in query1201)
            {
                TBM1201_HISTORY tbm1201 = new TBM1201_HISTORY();
                tbm1201.OVC_PURCH = result.OVC_PURCH;
                tbm1201.ONB_POI_ICOUNT = result.ONB_POI_ICOUNT;
                tbm1201.OVC_POI_NSTUFF_CHN = result.OVC_POI_NSTUFF_CHN;
                tbm1201.OVC_POI_NSTUFF_ENG = result.OVC_POI_NSTUFF_ENG;
                tbm1201.NSN_KIND = result.NSN_KIND;
                tbm1201.NSN = result.NSN;
                tbm1201.OVC_BRAND = result.OVC_BRAND;
                tbm1201.OVC_MODEL = result.OVC_MODEL;
                tbm1201.OVC_POI_IREF = result.OVC_POI_IREF;
                tbm1201.OVC_FCODE = result.OVC_FCODE;
                tbm1201.OVC_POI_IUNIT = result.OVC_POI_IUNIT;
                tbm1201.ONB_POI_QORDER_PLAN = result.ONB_POI_QORDER_PLAN;
                tbm1201.ONB_POI_QORDER_CONT = result.ONB_POI_QORDER_CONT;
                tbm1201.ONB_POI_MPRICE_PLAN = result.ONB_POI_MPRICE_PLAN;
                tbm1201.ONB_POI_MPRICE_CONT = result.ONB_POI_MPRICE_CONT;
                tbm1201.OVC_FIRST_BUY = result.OVC_FIRST_BUY;
                tbm1201.OVC_POI_IPURCH_BEF = result.OVC_POI_IPURCH_BEF;
                tbm1201.OVC_CURR_MPRICE_BEF = result.OVC_CURR_MPRICE_BEF;
                tbm1201.ONB_POI_QORDER_BEF = result.ONB_POI_QORDER_BEF;
                tbm1201.ONB_POI_MPRICE_BEF = result.ONB_POI_MPRICE_BEF;
                tbm1201.OVC_POI_NDESC = result.OVC_POI_NDESC;
                tbm1201.OVC_SAME_QUALITY = result.OVC_SAME_QUALITY;
                tbm1201.OVC_VERSION = dateNow;
                mpms.TBM1201_HISTORY.Add(tbm1201);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , tbm1201.GetType().Name.ToString(), this, "新增");
            }

            var query1220_1 = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum));
            foreach (var result in query1220_1)
            {
                TBM1220_1_HISTORY tbm1220_1 = new TBM1220_1_HISTORY();
                tbm1220_1.OVC_PURCH = result.OVC_PURCH;
                tbm1220_1.OVC_IKIND = result.OVC_IKIND;
                tbm1220_1.ONB_NO = result.ONB_NO;
                tbm1220_1.OVC_CHECK = result.OVC_CHECK;
                tbm1220_1.OVC_MEMO = result.OVC_MEMO;
                tbm1220_1.OVC_STANDARD = result.OVC_STANDARD;
                tbm1220_1.OVC_VERSION = dateNow;
                mpms.TBM1220_1_HISTORY.Add(tbm1220_1);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , tbm1220_1.GetType().Name.ToString(), this, "新增");
            }
            TBM1301 tbm1301 = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            tbm1301.OVC_DPROPOSE = txtOVC_DAPPLY.Text;
            tbm1301.OVC_PROPOSE = txtOVC_PROPOSE.Text;
            tbm1301.OVC_PERMISSION_UPDATE = "N";
            tbm1301.OVC_DOING_UNIT = txtOVC_AUDIT_UNIT.Text;
            gm.SaveChanges();

            var query1301 = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();

            TBM1301_HISTORY tbm1301_HISTORY = new TBM1301_HISTORY();
            tbm1301_HISTORY.OVC_PURCH = query1301.OVC_PURCH;
            tbm1301_HISTORY.OVC_PURCH_KIND = query1301.OVC_PURCH_KIND;
            tbm1301_HISTORY.OVC_LAB = query1301.OVC_LAB;
            tbm1301_HISTORY.OVC_PUR_ASS_VEN_CODE = query1301.OVC_PUR_ASS_VEN_CODE;
            tbm1301_HISTORY.OVC_BID_TIMES = query1301.OVC_BID_TIMES;
            tbm1301_HISTORY.OVC_BID = query1301.OVC_BID;
            tbm1301_HISTORY.OVC_PUR_NSECTION = query1301.OVC_PUR_NSECTION;
            tbm1301_HISTORY.OVC_PUR_SECTION = query1301.OVC_PUR_SECTION;
            tbm1301_HISTORY.OVC_PROPOSE = query1301.OVC_PROPOSE;
            tbm1301_HISTORY.OVC_DPROPOSE = query1301.OVC_DPROPOSE;
            tbm1301_HISTORY.OVC_PUR_APPROVE = query1301.OVC_PUR_APPROVE;
            tbm1301_HISTORY.OVC_PUR_DAPPROVE = query1301.OVC_PUR_DAPPROVE;
            tbm1301_HISTORY.OVC_PUR_IPURCH = query1301.OVC_PUR_IPURCH;
            tbm1301_HISTORY.OVC_PUR_IPURCH_ENG = query1301.OVC_PUR_IPURCH_ENG;
            tbm1301_HISTORY.OVC_PUR_USER = query1301.OVC_PUR_USER;
            tbm1301_HISTORY.OVC_KEYIN = query1301.OVC_KEYIN;
            tbm1301_HISTORY.OVC_PUR_IUSER_PHONE = query1301.OVC_PUR_IUSER_PHONE;
            tbm1301_HISTORY.OVC_PUR_IUSER_PHONE_EXT = query1301.OVC_PUR_IUSER_PHONE_EXT;
            tbm1301_HISTORY.OVC_PUR_IUSER_PHONE_EXT1 = query1301.OVC_PUR_IUSER_PHONE_EXT1;
            tbm1301_HISTORY.OVC_USER_CELLPHONE = query1301.OVC_USER_CELLPHONE;
            tbm1301_HISTORY.OVC_PUR_AGENCY = query1301.OVC_PUR_AGENCY;
            tbm1301_HISTORY.OVC_EST_REAR = query1301.OVC_EST_REAR;
            tbm1301_HISTORY.OVC_PUR_NPURCH = query1301.OVC_PUR_NPURCH;
            tbm1301_HISTORY.OVC_TARGET_DO = query1301.OVC_TARGET_DO;
            tbm1301_HISTORY.OVC_PUR_CURRENT = query1301.OVC_PUR_CURRENT;
            tbm1301_HISTORY.ONB_PUR_RATE = query1301.ONB_PUR_RATE;
            tbm1301_HISTORY.ONB_PUR_BUDGET = query1301.ONB_PUR_BUDGET;
            tbm1301_HISTORY.OVC_AGNT_IN = query1301.OVC_AGNT_IN;
            tbm1301_HISTORY.OVC_PUR_FEE_OK = query1301.OVC_PUR_FEE_OK;
            tbm1301_HISTORY.OVC_PUR_GOOD_OK = query1301.OVC_PUR_GOOD_OK;
            tbm1301_HISTORY.OVC_PUR_TAX_OK = query1301.OVC_PUR_TAX_OK;
            tbm1301_HISTORY.OVC_PUR_DCANPO = query1301.OVC_PUR_DCANPO;
            tbm1301_HISTORY.OVC_PUR_DCANRE = query1301.OVC_PUR_DCANRE;
            tbm1301_HISTORY.OVC_EXAMPLE_SUPPORT = query1301.OVC_EXAMPLE_SUPPORT;
            tbm1301_HISTORY.OVC_PUR_ALLOW = query1301.OVC_PUR_ALLOW;
            tbm1301_HISTORY.OVC_PUR_CREAT = query1301.OVC_PUR_CREAT;
            tbm1301_HISTORY.OVC_DOING_UNIT = query1301.OVC_DOING_UNIT;
            tbm1301_HISTORY.OVC_PERMISSION_UPDATE = query1301.OVC_PERMISSION_UPDATE;
            tbm1301_HISTORY.OVC_SUPERIOR_UNIT = query1301.OVC_SUPERIOR_UNIT;
            tbm1301_HISTORY.OVC_SECTION_CHIEF = query1301.OVC_SECTION_CHIEF;
            tbm1301_HISTORY.OVC_SHIP_TIMES = query1301.OVC_SHIP_TIMES;
            tbm1301_HISTORY.OVC_RECEIVE_PLACE = query1301.OVC_RECEIVE_PLACE;
            tbm1301_HISTORY.OVC_POI_IINSPECT = query1301.OVC_POI_IINSPECT;
            tbm1301_HISTORY.OVC_COUNTRY = query1301.OVC_COUNTRY;
            tbm1301_HISTORY.OVC_WAY_TRANS = query1301.OVC_WAY_TRANS;
            tbm1301_HISTORY.OVC_RECEIVE_NSECTION = query1301.OVC_RECEIVE_NSECTION;
            tbm1301_HISTORY.OVC_FROM_TO = query1301.OVC_FROM_TO;
            tbm1301_HISTORY.OVC_TO_PLACE = query1301.OVC_TO_PLACE;
            tbm1301_HISTORY.OVC_VERSION = dateNow;
            tbm1301_HISTORY.ONB_PUR_BUDGET_NT = query1301.ONB_PUR_BUDGET_NT;
            tbm1301_HISTORY.ONB_RESERVE_AMOUNT = query1301.ONB_RESERVE_AMOUNT;
            tbm1301_HISTORY.OVC_SPECIAL = query1301.OVC_SPECIAL;
            tbm1301_HISTORY.OVC_BID_MONEY = query1301.OVC_BID_MONEY;
            tbm1301_HISTORY.OVC_NEGOTIATION = query1301.OVC_NEGOTIATION;
            tbm1301_HISTORY.OVC_QUOTE = query1301.OVC_QUOTE;
            tbm1301_HISTORY.OVC_PLAN_PURCH = query1301.OVC_PLAN_PURCH;
            tbm1301_HISTORY.OVC_COPY_FROM = query1301.OVC_COPY_FROM;
            tbm1301_HISTORY.OVC_USER_TITLE = query1301.OVC_USER_TITLE;
            tbm1301_HISTORY.IS_PLURAL_BASIS = query1301.IS_PLURAL_BASIS;
            tbm1301_HISTORY.IS_OPEN_CONTRACT = query1301.IS_OPEN_CONTRACT;
            tbm1301_HISTORY.IS_JUXTAPOSED_MANUFACTURER = query1301.IS_JUXTAPOSED_MANUFACTURER;
            mpms.TBM1301_HISTORY.Add(tbm1301_HISTORY);
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , tbm1301_HISTORY.GetType().Name.ToString(), this, "新增");
            string id = Session["userid"].ToString();
            string queryName = gm.ACCOUNTs.Where(o => o.USER_ID.Equals(id)).Select(o => o.USER_NAME).FirstOrDefault();
            
            TBM1114 tbm1114 = new TBM1114();
            tbm1114.OVC_PURCH = strPurchNum;
            tbm1114.OVC_DATE = DateTime.Now;
            tbm1114.OVC_USER = queryName;
            tbm1114.OVC_TO_UNIT_NAME = txtOVC_TO_UNIT_NAME_1.Text;
            tbm1114.OVC_REMARK = "申購單位將購案轉呈" + txtOVC_TO_UNIT_NAME_1.Text;
            mpms.TBM1114.Add(tbm1114);

            TBMMESSAGE tbmMESSAGE = new TBMMESSAGE();
            tbmMESSAGE.OVC_PUR_SECTION = query1301.OVC_PUR_SECTION;
            tbmMESSAGE.USER_NAME = "計評分案人";
            tbmMESSAGE.OVC_PURCH = strPurchNum;
            tbmMESSAGE.OVC_TIMESTAMP = dateNow;
            tbmMESSAGE.OVC_STEP = "1";
            
            var query1301plan = gm.TBM1301_PLAN.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            TBMOVERSEE_LOG tbmOVERSEE_LOG = new TBMOVERSEE_LOG();
            tbmOVERSEE_LOG.OVC_PURCH = strPurchNum;
            tbmOVERSEE_LOG.OVC_DATE = dateNow;
            tbmOVERSEE_LOG.OVC_OVERSEE_UNIT = query1301plan.OVC_AUDIT_UNIT;
            tbmOVERSEE_LOG.OVC_APPLY_UNIT = query1301.OVC_PUR_SECTION;
            tbmOVERSEE_LOG.OVC_APPLY_NAME = query1301.OVC_PUR_USER;
            tbmOVERSEE_LOG.OVC_DAPPLY = query1301.OVC_DPROPOSE;
            tbmOVERSEE_LOG.OVC_APPLY_NO = query1301.OVC_PROPOSE;
            tbmOVERSEE_LOG.OVC_APPLY_CHIEF = query1301.OVC_SECTION_CHIEF;
            mpms.TBMOVERSEE_LOG.Add(tbmOVERSEE_LOG);

            TBM1301_PLAN tbm1301_plan = new TBM1301_PLAN();
            tbm1301_plan = query1301plan;
            tbm1301_plan.OVC_AUDIT_UNIT = txtOVC_AUDIT_UNIT.Text;
            tbm1301_plan.OVC_PURCHASE_UNIT = txtOVC_PURCHASE_UNIT.Text;
            tbm1301_plan.OVC_CONTRACT_UNIT = txtOVC_CONTRACT_UNIT.Text;

            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , tbm1301_plan.GetType().Name.ToString(), this, "修改");
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "完成轉呈作業");
        }
        #endregion
    }
}