using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using FCFDFE.Content;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A22_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        public string strDEPT_SN, strDEPT_Name;
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        string strEDF_SN;

        #region 副程式
        public void dataImport()
        {
            Guid guidEDF_SN = new Guid(strEDF_SN);
            var query = MTSE.TBGMT_EDF.Where(table => table.EDF_SN.Equals(guidEDF_SN)).FirstOrDefault();
            string strOVC_EDF_NO = query.OVC_EDF_NO;
            if (query != null)
            {
                lblOVC_EDF_NO.Text = strOVC_EDF_NO;
                txtOVC_PURCH_NO.Text = query.OVC_PURCH_NO;
                FCommon.list_setValue(drpOVC_START_PORT, query.OVC_START_PORT);
                string strOVC_ARRIVE_PORT = query.OVC_ARRIVE_PORT ?? "";
                txtOVC_PORT_CDE.Text = strOVC_ARRIVE_PORT;
                TBGMT_PORTS tPort = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(strOVC_ARRIVE_PORT)).FirstOrDefault();
                if (tPort != null) txtOVC_CHI_NAME.Text = tPort.OVC_PORT_CHI_NAME;
                txtOVC_SHIP_FROM.Text = query.OVC_SHIP_FROM;
                txtOVC_CON_ENG_ADDRESS.Text = query.OVC_CON_ENG_ADDRESS;
                txtOVC_CON_TEL.Text = query.OVC_CON_TEL;
                txtOVC_CON_FAX.Text = query.OVC_CON_FAX;
                txtOVC_NP_ENG_ADDRESS.Text = query.OVC_NP_ENG_ADDRESS;
                txtOVC_NP_TEL.Text = query.OVC_NP_TEL;
                txtOVC_NP_FAX.Text = query.OVC_NP_FAX;
                txtOVC_ANP_ENG_ADDRESS.Text = query.OVC_ANP_ENG_ADDRESS;
                txtOVC_ANP_TEL.Text = query.OVC_ANP_TEL;
                txtOVC_ANP_FAX.Text = query.OVC_ANP_FAX;
                txtOVC_ANP_ENG_ADDRESS2.Text = query.OVC_ANP_ENG_ADDRESS2;
                txtOVC_ANP_TEL2.Text = query.OVC_ANP_TEL2;
                txtOVC_ANP_FAX2.Text = query.OVC_ANP_FAX2;
                txtOVC_DELIVER_NAME.Text = query.OVC_DELIVER_NAME;
                txtOVC_DELIVER_MOBILE.Text = query.OVC_DELIVER_MOBILE;
                txtOVC_DELIVER_MILITARY_LINE.Text = query.OVC_DELIVER_MILITARY_LINE;
                string strOVC_PAYMENT_TYPE = query.OVC_PAYMENT_TYPE != null ? query.OVC_PAYMENT_TYPE : "預付"; //預設為預付
                string strOVC_IS_PAY = query.OVC_IS_PAY;
                bool isPREPAID = strOVC_PAYMENT_TYPE.Equals("預付");
                if (isPREPAID)
                    FCommon.list_setValue(rdoOVC_PAYMENT_TYPE, strOVC_PAYMENT_TYPE);
                else
                {
                    FCommon.list_setValue(rdoOVC_PAYMENT_TYPE, "到付");
                    FCommon.list_setValue(rdoOVC_IS_PAY, strOVC_IS_PAY);
                    FCommon.list_setValue(rdoOVC_PAYMENT_TYPE_Other, strOVC_PAYMENT_TYPE);
                }
                rdoOVC_IS_PAY.Visible = !isPREPAID;
                rdoOVC_PAYMENT_TYPE_Other.Visible = !isPREPAID;
                txtOVC_NOTE.Text = query.OVC_NOTE;
                bool boolOVC_IS_STRATEGY = (query.OVC_IS_STRATEGY ?? "").Equals("是");
                bool boolOVC_IS_RISK = (query.OVC_IS_RISK ?? "").Equals("是");
                bool boolOVC_IS_ALERTNESS = (query.OVC_IS_ALERTNESS ?? "").Equals("是");
                if (boolOVC_IS_STRATEGY)
                {
                    txtODT_VALIDITY_DATE.Text = FCommon.getDateTime(query.ODT_VALIDITY_DATE);
                    txtOVC_LICENSE_NO.Text = query.OVC_LICENSE_NO;
                }
                chkOVC_IS_STRATEGY.Checked = boolOVC_IS_STRATEGY;
                chkOVC_IS_RISK.Checked = boolOVC_IS_RISK;
                chkOVC_IS_ALERTNESS.Checked = boolOVC_IS_ALERTNESS;
                pnStrategy.Visible = boolOVC_IS_STRATEGY;

                dataImport_GV_TBGMT_EDF_DETAIL();
            }
        }
        public void dataImport_GV_TBGMT_EDF_DETAIL()
        {
            string strOVC_EDF_NO = lblOVC_EDF_NO.Text;
            var query =
                from edf_detail in MTSE.TBGMT_EDF_DETAIL
                join currency in MTSE.TBGMT_CURRENCY on edf_detail.OVC_CURRENCY equals currency.OVC_CURRENCY_CODE into currencyTemp
                from currency in currencyTemp.DefaultIfEmpty()
                where edf_detail.OVC_EDF_NO.Equals(strOVC_EDF_NO)
                orderby edf_detail.OVC_EDF_ITEM_NO
                select new
                {
                    edf_detail.EDF_DET_SN,
                    OVC_BOX_NO = edf_detail.OVC_BOX_NO,
                    OVC_ENG_NAME = edf_detail.OVC_ENG_NAME,
                    OVC_CHI_NAME = edf_detail.OVC_CHI_NAME,
                    OVC_ITEM_NO = edf_detail.OVC_ITEM_NO,
                    OVC_ITEM_NO2 = edf_detail.OVC_ITEM_NO2,
                    OVC_ITEM_NO3 = edf_detail.OVC_ITEM_NO3,
                    ONB_ITEM_COUNT = edf_detail.ONB_ITEM_COUNT,
                    OVC_ITEM_COUNT_UNIT = edf_detail.OVC_ITEM_COUNT_UNIT,
                    ONB_WEIGHT = edf_detail.ONB_WEIGHT,
                    OVC_WEIGHT_UNIT = edf_detail.OVC_WEIGHT_UNIT,
                    ONB_VOLUME = edf_detail.ONB_VOLUME,
                    OVC_VOLUME_UNIT = edf_detail.OVC_VOLUME_UNIT,
                    ONB_LENGTH = edf_detail.ONB_LENGTH,
                    ONB_WIDTH = edf_detail.ONB_WIDTH,
                    ONB_HEIGHT = edf_detail.ONB_HEIGHT,
                    ONB_MONEY = edf_detail.ONB_MONEY,
                    OVC_CURRENCY = currency != null ? currency.OVC_CURRENCY_NAME : "",
                    OVC_CURRENCY_Value = edf_detail.OVC_CURRENCY
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_EDF_DETAIL, dt);

            string strDefaultCurrency = VariableMTS.strDefaultCurrency; //預設幣別
            string strOVC_CURRENCY = strDefaultCurrency; //幣別
            var queryDisabled = query.FirstOrDefault();
            bool isDisabled = queryDisabled != null;
            if (isDisabled)
            {
                //單位下拉選單必須與第一筆資料同單位，如果有資料將鎖定不給選
                string strOVC_ITEM_COUNT_UNIT = queryDisabled.OVC_ITEM_COUNT_UNIT;
                FCommon.list_setValue(drpOVC_ITEM_COUNT_UNIT, strOVC_ITEM_COUNT_UNIT);
                bool isOther = !drpOVC_ITEM_COUNT_UNIT.SelectedValue.Equals(strOVC_ITEM_COUNT_UNIT); //選取值不等於應選取值，表示為其他
                if (isOther)
                {
                    txtOVC_ITEM_COUNT_UNIT.Text = strOVC_ITEM_COUNT_UNIT; //設定其他資料內容
                    FCommon.list_setValue(drpOVC_ITEM_COUNT_UNIT, "其他"); //選取其他項目
                }
                txtOVC_ITEM_COUNT_UNIT.Visible = isOther; //其他文字方塊顯示與否
                FCommon.list_setValue(drpOVC_VOLUME_UNIT, queryDisabled.OVC_VOLUME_UNIT);
                strOVC_CURRENCY = queryDisabled.OVC_CURRENCY_Value ?? strOVC_CURRENCY;

                FCommon.Controls_Attributes("readonly", "true", txtOVC_ITEM_COUNT_UNIT);
            }
            else
            {
                FCommon.Controls_Clear(drpOVC_ITEM_COUNT_UNIT, txtOVC_ITEM_COUNT_UNIT, drpOVC_VOLUME_UNIT);
                FCommon.Controls_Attributes("readonly", txtOVC_ITEM_COUNT_UNIT);
                txtOVC_ITEM_COUNT_UNIT.Visible = false;
            }
            FCommon.list_setValue(drpOVC_CURRENCY, strOVC_CURRENCY);
            drpOVC_ITEM_COUNT_UNIT.Enabled = !isDisabled;
            drpOVC_VOLUME_UNIT.Enabled = !isDisabled;
            drpOVC_CURRENCY.Enabled = !isDisabled;
        }
        private void Create_DETAIL_MRPLOG(Guid edf_det_SN, string strLOG_EVENT)
        {
            TBGMT_EDF_DETAIL DETAIL = MTSE.TBGMT_EDF_DETAIL.Where(table => table.EDF_DET_SN.Equals(edf_det_SN)).FirstOrDefault();
            if (DETAIL != null)
            {
                string strUserId = Session["userid"].ToString(); //資料建立人員
                DateTime dateNow = DateTime.Now;

                #region TBGMT_EDF_DETAIL_MRPLOG 新增
                TBGMT_EDF_DETAIL_MRPLOG DETAIL_MRPLOG = new TBGMT_EDF_DETAIL_MRPLOG();
                DETAIL_MRPLOG.LOG_LOGIN_ID = strUserId;
                DETAIL_MRPLOG.LOG_TIME = dateNow;
                DETAIL_MRPLOG.LOG_EVENT = strLOG_EVENT;
                DETAIL_MRPLOG.OVC_EDF_NO = DETAIL.OVC_EDF_NO; //外運資料表編號
                DETAIL_MRPLOG.OVC_EDF_ITEM_NO = DETAIL.OVC_EDF_ITEM_NO; //項次
                DETAIL_MRPLOG.OVC_BOX_NO = DETAIL.OVC_BOX_NO; //箱號
                DETAIL_MRPLOG.OVC_ENG_NAME = DETAIL.OVC_ENG_NAME; //英文品名
                DETAIL_MRPLOG.OVC_CHI_NAME = DETAIL.OVC_CHI_NAME; //中文品名
                DETAIL_MRPLOG.OVC_ITEM_NO = DETAIL.OVC_ITEM_NO; //料號
                DETAIL_MRPLOG.OVC_ITEM_NO2 = DETAIL.OVC_ITEM_NO2; //單號
                DETAIL_MRPLOG.OVC_ITEM_NO3 = DETAIL.OVC_ITEM_NO3; //件號
                DETAIL_MRPLOG.ONB_ITEM_COUNT = DETAIL.ONB_ITEM_COUNT; //數量
                DETAIL_MRPLOG.OVC_ITEM_COUNT_UNIT = DETAIL.OVC_ITEM_COUNT_UNIT; //數量單位
                DETAIL_MRPLOG.ONB_WEIGHT = DETAIL.ONB_WEIGHT; //重量
                DETAIL_MRPLOG.OVC_WEIGHT_UNIT = DETAIL.OVC_WEIGHT_UNIT; //重量單位
                DETAIL_MRPLOG.ONB_VOLUME = DETAIL.ONB_VOLUME; //容積
                DETAIL_MRPLOG.OVC_VOLUME_UNIT = DETAIL.OVC_VOLUME_UNIT; //容積單位
                DETAIL_MRPLOG.ONB_LENGTH = DETAIL.ONB_LENGTH; //長
                DETAIL_MRPLOG.ONB_WIDTH = DETAIL.ONB_WIDTH;  //寬
                DETAIL_MRPLOG.ONB_HEIGHT = DETAIL.ONB_HEIGHT; //高
                DETAIL_MRPLOG.ONB_MONEY = DETAIL.ONB_MONEY; //金額
                DETAIL_MRPLOG.OVC_CURRENCY = DETAIL.OVC_CURRENCY; //幣別
                DETAIL_MRPLOG.OVC_CREATE_LOGIN_ID = DETAIL.OVC_CREATE_LOGIN_ID; //資料建立人員
                DETAIL_MRPLOG.ODT_MODIFY_DATE = DETAIL.ODT_MODIFY_DATE; //資料修改日期
                DETAIL_MRPLOG.EDF_DET_MRP_SN = Guid.NewGuid();
                MTSE.TBGMT_EDF_DETAIL_MRPLOG.Add(DETAIL_MRPLOG);
                MTSE.SaveChanges(); //儲存
                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), DETAIL_MRPLOG.GetType().Name, this, "新增");
                #endregion
            }
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_REQ_DEPT_CDE", Request.QueryString["OVC_REQ_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_EDF_NO", Request.QueryString["OVC_EDF_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_START_PORT", Request.QueryString["OVC_START_PORT"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_ARRIVE_PORT", Request.QueryString["OVC_ARRIVE_PORT"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", Request.QueryString["OVC_TRANSER_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_IS_STRATEGY", Request.QueryString["OVC_IS_STRATEGY"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_REVIEW_STATUS", Request.QueryString["OVC_REVIEW_STATUS"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_RECEIVE_DATE", Request.QueryString["ODT_RECEIVE_DATE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_PURCH_NO", Request.QueryString["OVC_PURCH_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_ITEM_NO2", Request.QueryString["OVC_ITEM_NO2"], false);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getQueryString(this, "id", out strEDF_SN, true);
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getAccountDEPTName(this, out strDEPT_SN, out strDEPT_Name);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_CHI_NAME, txtODT_VALIDITY_DATE);
                    #region 匯入下拉式選單
                    CommonMTS.list_dataImport_PORT(drpOVC_START_PORT, true); //啟運港(機場)
                    CommonMTS.list_dataImport_PAYMENT_TYPE_EDF(rdoOVC_PAYMENT_TYPE, false); //付款方式
                    CommonMTS.list_dataImport_IS_PAY(rdoOVC_IS_PAY, false); //付款方式－是否投保
                    CommonMTS.list_dataImport_PAYMENT_TYPE_Other(rdoOVC_PAYMENT_TYPE_Other, false); //付款方式－其他
                    CommonMTS.list_dataImport_QUANITY_UNIT(drpOVC_ITEM_COUNT_UNIT, true); //數量單位
                    CommonMTS.list_dataImport_VOLUME_UNIT(drpOVC_VOLUME_UNIT, true); //容積單位
                    CommonMTS.list_dataImport_CURRENCY(drpOVC_CURRENCY, false, new string[] { "CHF" }); //幣值 瑞郎除外
                    #endregion

                    HFdeptCode.Value = strDEPT_SN; //隱藏欄位儲存單位代碼(前端JS抓值用)
                    depLabel.Text = strDEPT_Name;

                    dataImport();
                    pnNewItem.Visible = false;
                }
            }
        }
                
        #region 料件
        //由體積換算
        protected void convertToVolume_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strONB_LENGTH = txtONB_LENGTH.Text;
            string strONB_WIDTH = txtONB_WIDTH.Text;
            string strONB_HEIGHT = txtONB_HEIGHT.Text;
            decimal decONB_LENGTH, decONB_WIDTH, decONB_HEIGHT;

            #region 必填項目
            if (strONB_LENGTH.Equals(string.Empty))
                strMessage += "<P> 請輸入 體積-長 </p>";
            if (strONB_WIDTH.Equals(string.Empty))
                strMessage += "<P> 請輸入 體積-寬 </p>";
            if (strONB_HEIGHT.Equals(string.Empty))
                strMessage += "<P> 請輸入 體積-高 </p>";
            //確認輸入型態
            bool boolONB_LENGTH = FCommon.checkDecimal(strONB_LENGTH, "體積-長", ref strMessage, out decONB_LENGTH);
            bool boolONB_WIDTH = FCommon.checkDecimal(strONB_WIDTH, "體積-寬", ref strMessage, out decONB_WIDTH);
            bool boolONB_HEIGHT = FCommon.checkDecimal(strONB_HEIGHT, "體積-高", ref strMessage, out decONB_HEIGHT);
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                string strONB_VOLUME = "";
                double doubleONB_VOLUME = Convert.ToDouble(decONB_LENGTH * decONB_WIDTH * decONB_HEIGHT);
                if (drpOVC_VOLUME_UNIT.SelectedValue.Equals("CBM"))
                {
                    strONB_VOLUME = Convert.ToString(Math.Round(Math.Pow(0.01, 3) * doubleONB_VOLUME, 3));
                    //2015.03.25 換算小數點3位都為0時,就秀成0.001
                    if (strONB_VOLUME == "0")
                        strONB_VOLUME = "0.001";
                }
                else if (drpOVC_VOLUME_UNIT.SelectedValue.Equals("CF"))
                    strONB_VOLUME = Convert.ToString(Math.Round(Math.Pow(0.0328, 3) * doubleONB_VOLUME, 3));
                txtONB_VOLUME.Text = strONB_VOLUME;
            }
            else
                FCommon.AlertShow(pnVolume, "danger", "系統訊息", strMessage);
        }
        protected void btnNewMP_Click(object sender, EventArgs e)
        {
            pnNewItem.Visible = true;
            btnDel.Visible = false;
            ViewState["isModify"] = false;
            ViewState["EDF_DET_SN"] = null;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            #region 取值
            string strMessage = "";
            string strUserId = Session["userid"].ToString(); //資料建立人員
            string strOVC_EDF_NO = lblOVC_EDF_NO.Text;
            string strOVC_BOX_NO = txtOVC_BOX_NO.Text; //箱號
            string strOVC_ENG_NAME = txtOVC_ENG_NAME.Text; //英文品名
            string strOVC_CHI_NAME = txtDETAIL_OVC_CHI_NAME.Text; //中文品名
            string strOVC_ITEM_NO = txtOVC_ITEM_NO.Text; //料號
            string strOVC_ITEM_NO2 = txtOVC_ITEM_NO2.Text; //單號
            string strOVC_ITEM_NO3 = txtOVC_ITEM_NO3.Text; //件號

            string strONB_ITEM_COUNT = txtONB_ITEM_COUNT.Text; //數量
            string strOVC_ITEM_COUNT_UNIT = drpOVC_ITEM_COUNT_UNIT.SelectedValue; //數量單位
            if (strOVC_ITEM_COUNT_UNIT.Equals("其他")) strOVC_ITEM_COUNT_UNIT = txtOVC_ITEM_COUNT_UNIT.Text; //若選擇其他，則取得輸入之資料
            string strONB_WEIGHT = txtONB_WEIGHT.Text; //重量
            string strOVC_WEIGHT_UNIT = lbOVC_WEIGHT_UNIT.Text; //重量單位
            string strONB_VOLUME = txtONB_VOLUME.Text; //容積
            string strOVC_VOLUME_UNIT = drpOVC_VOLUME_UNIT.SelectedValue; //容積單位
            string strONB_LENGTH = txtONB_LENGTH.Text; //長
            string strONB_WIDTH = txtONB_WIDTH.Text; //寬
            string strONB_HEIGHT = txtONB_HEIGHT.Text; //高
            string strONB_MONEY = txtONB_MONEY.Text; //金額
            string strOVC_CURRENCY = drpOVC_CURRENCY.SelectedValue; //幣別

            decimal decONB_ITEM_COUNT, decONB_WEIGHT, decONB_VOLUME, decONB_LENGTH, decONB_WIDTH, decONB_HEIGHT, decONB_MONEY;
            DateTime dateNow = DateTime.Now;
            #endregion

            #region 必填項目
            if (strOVC_BOX_NO.Equals(string.Empty))
                strMessage += "<P> 請輸入 箱號 </p>";
            if (strOVC_ENG_NAME.Equals(string.Empty))
                strMessage += "<P> 請輸入 英文品名 </p>";
            if (strOVC_CHI_NAME.Equals(string.Empty))
                strMessage += "<P> 請輸入 中文品名 </p>";
            if (strONB_ITEM_COUNT.Equals(string.Empty))
                strMessage += "<P> 請輸入 數量 </p>";
            if (strOVC_ITEM_COUNT_UNIT.Equals(string.Empty))
                strMessage += "<P> 請選取 數量單位 </p>";
            if (strONB_MONEY.Equals(string.Empty))
                strMessage += "<P> 請輸入 金額 </p>";
            if (strOVC_CURRENCY.Equals(string.Empty))
                strMessage += "<P> 請選取 幣別 </p>";

            //確認輸入型態
            bool boolONB_ITEM_COUNT = FCommon.checkDecimal(strONB_ITEM_COUNT, "數量", ref strMessage, out decONB_ITEM_COUNT);
            bool boolONB_WEIGHT = FCommon.checkDecimal(strONB_WEIGHT, "重量", ref strMessage, out decONB_WEIGHT);
            bool boolONB_VOLUME = FCommon.checkDecimal(strONB_VOLUME, "容積", ref strMessage, out decONB_VOLUME);
            bool boolONB_LENGTH = FCommon.checkDecimal(strONB_LENGTH, "體積-長", ref strMessage, out decONB_LENGTH);
            bool boolONB_WIDTH = FCommon.checkDecimal(strONB_WIDTH, "體積-寬", ref strMessage, out decONB_WIDTH);
            bool boolONB_HEIGHT = FCommon.checkDecimal(strONB_HEIGHT, "體積-高", ref strMessage, out decONB_HEIGHT);
            bool boolONB_MONEY = FCommon.checkDecimal(strONB_MONEY, "金額", ref strMessage, out decONB_MONEY);
            #endregion

            #region 更新or新增料件
            if (strMessage.Equals(string.Empty))
            {
                bool isModify = ViewState["isModify"] != null ? Convert.ToBoolean(ViewState["isModify"]) : false;
                if (isModify && ViewState["EDF_DET_SN"] != null)
                {
                    Guid edf_det_SN = new Guid(ViewState["EDF_DET_SN"].ToString());
                    TBGMT_EDF_DETAIL EDF_DETAIL = MTSE.TBGMT_EDF_DETAIL.Where(table => table.EDF_DET_SN.Equals(edf_det_SN)).FirstOrDefault();
                    if (EDF_DETAIL != null)
                    {
                        string strOVC_EDF_ITEM_NO = EDF_DETAIL.OVC_EDF_ITEM_NO.ToString();
                        #region TBGMT_EDF_DETAIL 修改
                        EDF_DETAIL.OVC_EDF_NO = strOVC_EDF_NO; //外運資料表編號
                        //EDF_DETAIL.OVC_EDF_ITEM_NO = intOVC_EDF_ITEM_NO; //項次
                        EDF_DETAIL.OVC_BOX_NO = strOVC_BOX_NO; //箱號
                        EDF_DETAIL.OVC_ENG_NAME = strOVC_ENG_NAME; //英文品名
                        EDF_DETAIL.OVC_CHI_NAME = strOVC_CHI_NAME; //中文品名
                        EDF_DETAIL.OVC_ITEM_NO = strOVC_ITEM_NO; //料號
                        EDF_DETAIL.OVC_ITEM_NO2 = strOVC_ITEM_NO2; //單號
                        EDF_DETAIL.OVC_ITEM_NO3 = strOVC_ITEM_NO3; //件號
                        EDF_DETAIL.ONB_ITEM_COUNT = decONB_ITEM_COUNT; //數量
                        EDF_DETAIL.OVC_ITEM_COUNT_UNIT = strOVC_ITEM_COUNT_UNIT; //數量單位
                        if (boolONB_WEIGHT) EDF_DETAIL.ONB_WEIGHT = decONB_WEIGHT; else EDF_DETAIL.ONB_WEIGHT = null; //重量
                        EDF_DETAIL.OVC_WEIGHT_UNIT = strOVC_WEIGHT_UNIT; //重量單位
                        if (boolONB_VOLUME) EDF_DETAIL.ONB_VOLUME = decONB_VOLUME; else EDF_DETAIL.ONB_VOLUME = null; //容積
                        EDF_DETAIL.OVC_VOLUME_UNIT = strOVC_VOLUME_UNIT; //容積單位
                        if (boolONB_LENGTH) EDF_DETAIL.ONB_LENGTH = decONB_LENGTH; else EDF_DETAIL.ONB_LENGTH = null; //長
                        if (boolONB_WIDTH) EDF_DETAIL.ONB_WIDTH = decONB_WIDTH; else EDF_DETAIL.ONB_WIDTH = null; //寬
                        if (boolONB_HEIGHT) EDF_DETAIL.ONB_HEIGHT = decONB_HEIGHT; else EDF_DETAIL.ONB_HEIGHT = null; //高
                        EDF_DETAIL.ONB_MONEY = decONB_MONEY; //金額
                        EDF_DETAIL.OVC_CURRENCY = strOVC_CURRENCY; //幣別
                        EDF_DETAIL.ODT_MODIFY_DATE = dateNow; //資料修改日期
                        MTSE.SaveChanges(); //儲存
                        FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), EDF_DETAIL.GetType().Name, this, "修改");
                        #endregion

                        Create_DETAIL_MRPLOG(edf_det_SN, "UPDATE");

                        FCommon.AlertShow(PnMessage_Item, "success", "系統訊息", $"項次：{ strOVC_EDF_ITEM_NO } 之料件，更新成功");
                        pnNewItem.Visible = false;
                        ViewState["EDF_DET_SN"] = null;
                    }
                    else
                        FCommon.AlertShow(PnMessage_Item, "danger", "系統訊息", "選取之料件不存在，請重新選取。");
                }
                else
                {
                    //新增－取得新的項次
                    var query = MTSE.TBGMT_EDF_DETAIL.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).OrderByDescending(d => d.OVC_EDF_ITEM_NO).FirstOrDefault();
                    decimal decOVC_EDF_ITEM_NO = query != null ? query.OVC_EDF_ITEM_NO + 1 : 1;

                    Guid edf_det_SN = Guid.NewGuid();
                    #region TBGMT_EDF_DETAIL 新增
                    TBGMT_EDF_DETAIL EDF_DETAIL = new TBGMT_EDF_DETAIL();
                    EDF_DETAIL.OVC_EDF_NO = strOVC_EDF_NO; //外運資料表編號
                    EDF_DETAIL.OVC_EDF_ITEM_NO = decOVC_EDF_ITEM_NO; //項次
                    EDF_DETAIL.OVC_BOX_NO = strOVC_BOX_NO; //箱號
                    EDF_DETAIL.OVC_ENG_NAME = strOVC_ENG_NAME; //英文品名
                    EDF_DETAIL.OVC_CHI_NAME = strOVC_CHI_NAME; //中文品名
                    EDF_DETAIL.OVC_ITEM_NO = strOVC_ITEM_NO; //料號
                    EDF_DETAIL.OVC_ITEM_NO2 = strOVC_ITEM_NO2; //單號
                    EDF_DETAIL.OVC_ITEM_NO3 = strOVC_ITEM_NO3; //件號
                    EDF_DETAIL.ONB_ITEM_COUNT = decONB_ITEM_COUNT; //數量
                    EDF_DETAIL.OVC_ITEM_COUNT_UNIT = strOVC_ITEM_COUNT_UNIT; //數量單位
                    if (boolONB_WEIGHT) EDF_DETAIL.ONB_WEIGHT = decONB_WEIGHT; else EDF_DETAIL.ONB_WEIGHT = null; //重量
                    EDF_DETAIL.OVC_WEIGHT_UNIT = strOVC_WEIGHT_UNIT; //重量單位
                    if (boolONB_VOLUME) EDF_DETAIL.ONB_VOLUME = decONB_VOLUME; else EDF_DETAIL.ONB_VOLUME = null; //容積
                    EDF_DETAIL.OVC_VOLUME_UNIT = strOVC_VOLUME_UNIT; //容積單位
                    if (boolONB_LENGTH) EDF_DETAIL.ONB_LENGTH = decONB_LENGTH; else EDF_DETAIL.ONB_LENGTH = null; //長
                    if (boolONB_WIDTH) EDF_DETAIL.ONB_WIDTH = decONB_WIDTH; else EDF_DETAIL.ONB_WIDTH = null; //寬
                    if (boolONB_HEIGHT) EDF_DETAIL.ONB_HEIGHT = decONB_HEIGHT; else EDF_DETAIL.ONB_HEIGHT = null; //高
                    EDF_DETAIL.ONB_MONEY = decONB_MONEY; //金額
                    EDF_DETAIL.OVC_CURRENCY = strOVC_CURRENCY; //幣別
                    EDF_DETAIL.OVC_CREATE_LOGIN_ID = strUserId; //資料建立人員
                    EDF_DETAIL.ODT_MODIFY_DATE = dateNow; //資料修改日期
                    EDF_DETAIL.EDF_DET_SN = edf_det_SN;
                    MTSE.TBGMT_EDF_DETAIL.Add(EDF_DETAIL);
                    MTSE.SaveChanges(); //儲存
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), EDF_DETAIL.GetType().Name, this, "新增");
                    #endregion
                    Create_DETAIL_MRPLOG(edf_det_SN,"INSERT");

                    FCommon.AlertShow(PnMessage_Item, "success", "系統訊息", $"項次：{ decOVC_EDF_ITEM_NO } 之料件，新增成功");
                    pnNewItem.Visible = false;
                }

                ViewState["isModify"] = null;
                //清空欄位
                FCommon.Controls_Clear(pnNewItem, lbOVC_WEIGHT_UNIT);
                dataImport_GV_TBGMT_EDF_DETAIL();
            }
            else
                FCommon.AlertShow(PnMessage_Item, "danger", "系統訊息", strMessage);
            #endregion
        }
        protected void btnDel_Click(object sender, EventArgs e)
        {
            if (ViewState["EDF_DET_SN"] != null)
            {
                Guid id = new Guid(ViewState["EDF_DET_SN"].ToString());
                string strUserId = Session["userid"].ToString(); //資料建立人員

                TBGMT_EDF_DETAIL DETAIL = MTSE.TBGMT_EDF_DETAIL.Where(table => table.EDF_DET_SN.Equals(id)).FirstOrDefault();
                if (DETAIL != null)
                {
                    Create_DETAIL_MRPLOG(id, "DELETE"); //新增刪除LOG

                    string strOVC_EDF_ITEM_NO = DETAIL.OVC_EDF_ITEM_NO.ToString();
                    MTSE.Entry(DETAIL).State = EntityState.Deleted;
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), DETAIL.GetType().Name, this, "刪除");
                    FCommon.Controls_Clear(pnNewItem, lbOVC_WEIGHT_UNIT);
                    //FCommon.Controls_Clear(txtOVC_BOX_NO, txtOVC_ENG_NAME, txtOVC_ITEM_CHI_NAME, txtOVC_ITEM_NO, txtOVC_ITEM_NO2, txtOVC_ITEM_NO3, txtONB_ITEM_COUNT, txtONB_WEIGHT, txtONB_VOLUME, txtONB_LENGTH, txtONB_WIDTH, txtONB_HEIGHT, txtONB_MONEY);
                    dataImport_GV_TBGMT_EDF_DETAIL();
                    pnNewItem.Visible = false;
                    ViewState["EDF_DET_SN"] = null;
                    FCommon.AlertShow(PnMessage_Item, "success", "系統訊息", $"項次：{ strOVC_EDF_ITEM_NO } 之料件，刪除成功");
                }
                else
                    FCommon.AlertShow(PnMessage_Item, "danger", "系統訊息", "選取之料件已被刪除，請重新選取。");
            }
            else
                FCommon.AlertShow(PnMessage_Item, "danger", "系統訊息", "請先選取料件");
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            FCommon.Controls_Clear(pnNewItem, lbOVC_WEIGHT_UNIT);
            pnNewItem.Visible = false;
            ViewState["isModify"] = null;
            ViewState["EDF_DET_SN"] = null;
        }

        //選取料件
        protected void GV_TBGMT_EDF_DETAIL_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            Guid id = new Guid(theGridView.DataKeys[gvrIndex].Value.ToString());

            ViewState["EDF_DET_SN"] = id;
            switch (e.CommandName)
            {
                case "btnSelect":
                    var detail = MTSE.TBGMT_EDF_DETAIL.Where(table => table.EDF_DET_SN.Equals(id)).FirstOrDefault();
                    if (detail != null)
                    {
                        txtOVC_BOX_NO.Text = detail.OVC_BOX_NO;
                        txtOVC_ENG_NAME.Text = detail.OVC_ENG_NAME;
                        txtDETAIL_OVC_CHI_NAME.Text = detail.OVC_CHI_NAME;
                        txtOVC_ITEM_NO.Text = detail.OVC_ITEM_NO;
                        txtOVC_ITEM_NO2.Text = detail.OVC_ITEM_NO2;
                        txtOVC_ITEM_NO3.Text = detail.OVC_ITEM_NO3;
                        txtONB_ITEM_COUNT.Text = detail.ONB_ITEM_COUNT.ToString();
                        string strOVC_ITEM_COUNT_UNIT = detail.OVC_ITEM_COUNT_UNIT;
                        FCommon.list_setValue(drpOVC_ITEM_COUNT_UNIT, strOVC_ITEM_COUNT_UNIT);
                        bool isOther = !drpOVC_ITEM_COUNT_UNIT.SelectedValue.Equals(strOVC_ITEM_COUNT_UNIT); //選取值不等於應選取值，表示為其他
                        if (isOther)
                        {
                            txtOVC_ITEM_COUNT_UNIT.Text = strOVC_ITEM_COUNT_UNIT; //設定其他資料內容
                            FCommon.list_setValue(drpOVC_ITEM_COUNT_UNIT, "其他"); //選取其他項目
                        }

                        txtONB_LENGTH.Text = detail.ONB_LENGTH.ToString();
                        txtONB_WIDTH.Text = detail.ONB_WIDTH.ToString();
                        txtONB_HEIGHT.Text = detail.ONB_HEIGHT.ToString();
                        txtONB_VOLUME.Text = detail.ONB_VOLUME.ToString();
                        FCommon.list_setValue(drpOVC_VOLUME_UNIT, detail.OVC_VOLUME_UNIT);
                        txtONB_WEIGHT.Text = detail.ONB_WEIGHT.ToString();
                        lbOVC_WEIGHT_UNIT.Text = detail.OVC_WEIGHT_UNIT;
                        txtONB_MONEY.Text = detail.ONB_MONEY.ToString();
                        FCommon.list_setValue(drpOVC_CURRENCY, detail.OVC_CURRENCY);

                        pnNewItem.Visible = true;
                        btnDel.Visible = true;
                        ViewState["isModify"] = true;
                    }
                    break;
                default:
                    break;
            }
        }
        protected void GV_TBGMT_EDF_DETAIL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = e.Row;
            switch (gvr.RowType)
            {
                case DataControlRowType.DataRow:
                    break;
                case DataControlRowType.Footer:
                    gvr.Cells[0].Text = "總計";
                    gvr.Cells[1].Text = "共" + theGridView.Rows.Count + "項";
                    int sumONB_ITEM_COUNT = 0;
                    decimal sumONB_WEIGHT = 0, sumONB_VOLUME = 0, sumONB_MONEY = 0;
                    if (theGridView.Rows.Count != 0)
                    {
                        for (int i = 0; i < theGridView.Rows.Count; i++)
                        {
                            GridViewRow theRow = theGridView.Rows[i];
                            string strONB_ITEM_COUNT = theRow.Cells[6].Text;
                            int intTemp;
                            if (int.TryParse(strONB_ITEM_COUNT, out intTemp))
                                sumONB_ITEM_COUNT += intTemp;
                            string strONB_WEIGHT = theRow.Cells[8].Text;
                            string strONB_VOLUME = theRow.Cells[10].Text;
                            string strONB_MONEY = theRow.Cells[13].Text;
                            decimal decTemp;
                            if (decimal.TryParse(strONB_WEIGHT, out decTemp))
                                sumONB_WEIGHT += decTemp;
                            if (decimal.TryParse(strONB_VOLUME, out decTemp))
                                sumONB_VOLUME += decTemp;
                            if (decimal.TryParse(strONB_MONEY, out decTemp))
                                sumONB_MONEY += decTemp;
                        }
                    }
                    gvr.Cells[6].Text = sumONB_ITEM_COUNT.ToString();
                    gvr.Cells[8].Text = sumONB_WEIGHT.ToString();
                    gvr.Cells[10].Text = sumONB_VOLUME.ToString();
                    gvr.Cells[13].Text = sumONB_MONEY.ToString();

                    GridViewRow firstRow = theGridView.Rows[0];
                    gvr.Cells[7].Text = firstRow.Cells[7].Text;
                    gvr.Cells[9].Text = firstRow.Cells[9].Text;
                    gvr.Cells[11].Text = firstRow.Cells[11].Text;
                    gvr.Cells[14].Text = firstRow.Cells[14].Text;
                    break;
                default:
                    break;
            }
        }
        protected void GV_TBGMT_EDF_DETAIL_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        #endregion

        //更新外運資料表
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Guid guidEDF_SN = new Guid(strEDF_SN);
            TBGMT_EDF EDF = MTSE.TBGMT_EDF.Where(table => table.EDF_SN.Equals(guidEDF_SN)).FirstOrDefault();
            if (EDF != null)
            {
                string strMessage = "";
                #region 取值
                string strUserId = Session["userid"].ToString(); //資料建立人員
                string strOVC_EDF_NO = EDF.OVC_EDF_NO;
                string strOVC_PURCH_NO = txtOVC_PURCH_NO.Text; //案號
                string strOVC_START_PORT = drpOVC_START_PORT.SelectedValue; //啟運港(機場)
                string strOVC_ARRIVE_PORT = txtOVC_PORT_CDE.Text; //目的港(機場)
                TBM1407 PORT = GME.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("TR") && table.OVC_PHR_ID.Equals(strOVC_START_PORT)).FirstOrDefault();
                string strOVC_DEPT_CDE = PORT != null ? PORT.OVC_PHR_PARENTS : "";
                strOVC_DEPT_CDE = strOVC_DEPT_CDE.Replace("地區", "").Replace("分遣組", "");
                string strOVC_SHIP_FROM = txtOVC_SHIP_FROM.Text;
                string strOVC_REQ_DEPT_CDE = strDEPT_SN; //申請單位代碼
                string strOVC_CON_ENG_ADDRESS = txtOVC_CON_ENG_ADDRESS.Text;
                string strOVC_CON_TEL = txtOVC_CON_TEL.Text;
                string strOVC_CON_FAX = txtOVC_CON_FAX.Text;
                string strOVC_NP_ENG_ADDRESS = txtOVC_NP_ENG_ADDRESS.Text;
                string strOVC_NP_TEL = txtOVC_NP_TEL.Text;
                string strOVC_NP_FAX = txtOVC_NP_FAX.Text;
                string strOVC_ANP_ENG_ADDRESS = txtOVC_ANP_ENG_ADDRESS.Text;
                string strOVC_ANP_TEL = txtOVC_ANP_TEL.Text;
                string strOVC_ANP_FAX = txtOVC_ANP_FAX.Text;
                string strOVC_ANP_ENG_ADDRESS2 = txtOVC_ANP_ENG_ADDRESS2.Text;
                string strOVC_ANP_TEL2 = txtOVC_ANP_TEL2.Text;
                string strOVC_ANP_FAX2 = txtOVC_ANP_FAX2.Text;
                string strOVC_DELIVER_NAME = txtOVC_DELIVER_NAME.Text;
                string strOVC_DELIVER_MOBILE = txtOVC_DELIVER_MOBILE.Text;
                string strOVC_DELIVER_MILITARY_LINE = txtOVC_DELIVER_MILITARY_LINE.Text;
                string strOVC_PAYMENT_TYPE = rdoOVC_PAYMENT_TYPE.SelectedValue; //付款方式
                string strOVC_IS_PAY = "";
                if (strOVC_PAYMENT_TYPE.Equals("預付"))
                    strOVC_IS_PAY = "1"; //預付為投保
                else
                {
                    strOVC_PAYMENT_TYPE = rdoOVC_PAYMENT_TYPE_Other.SelectedValue;
                    strOVC_IS_PAY = rdoOVC_IS_PAY.SelectedValue;
                }
                string strOVC_NOTE = txtOVC_NOTE.Text; //備考
                bool boolOVC_IS_STRATEGY = chkOVC_IS_STRATEGY.Checked;
                string strOVC_IS_STRATEGY = boolOVC_IS_STRATEGY ? "是" : "否";
                string strODT_VALIDITY_DATE = txtODT_VALIDITY_DATE.Text;
                string strOVC_LICENSE_NO = txtOVC_LICENSE_NO.Text;
                bool boolOVC_IS_RISK = chkOVC_IS_RISK.Checked;
                string strOVC_IS_RISK = boolOVC_IS_RISK ? "是" : "否";
                bool boolOVC_IS_ALERTNESS = chkOVC_IS_ALERTNESS.Checked;
                string strOVC_IS_ALERTNESS = boolOVC_IS_ALERTNESS ? "是" : "否";

                DateTime dateODT_VALIDITY_DATE = DateTime.MinValue, dateNow = DateTime.Now;
                #endregion

                TBGMT_EDF_DETAIL edf_detail = MTSE.TBGMT_EDF_DETAIL.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();
                #region 必填項目
                if (strOVC_PURCH_NO.Equals(string.Empty))
                    strMessage += "<P> 請輸入 案號 </p>";
                if (strOVC_START_PORT.Equals(string.Empty))
                        strMessage += "<P> 請選擇 啟運港(機場) </p>";
                if (strOVC_ARRIVE_PORT.Equals(string.Empty))
                    strMessage += "<P> 請選擇 目的港(機場) </p>";
                if (strOVC_CON_ENG_ADDRESS.Equals(string.Empty))
                    strMessage += "<P> 請輸入 CONSIGNEE 地址 </p>";
                if (strOVC_CON_TEL.Equals(string.Empty))
                    strMessage += "<P> 請輸入 CONSIGNEE 電話 </p>";
                if (strOVC_CON_FAX.Equals(string.Empty))
                    strMessage += "<P> 請輸入 CONSIGNEE 傳真 </p>";
                if (strOVC_DELIVER_NAME.Equals(string.Empty))
                    strMessage += "<P> 請輸入 發貨人資訊 名字 </p>";
                if (strOVC_DELIVER_MOBILE.Equals(string.Empty))
                    strMessage += "<P> 請輸入 發貨人資訊 手機 </p>";
                if (strOVC_DELIVER_MILITARY_LINE.Equals(string.Empty))
                    strMessage += "<P> 請輸入 發貨人資訊 軍線 </p>";
                if (strOVC_PAYMENT_TYPE.Equals(string.Empty))
                    strMessage += "<P> 請選擇 付款方式 </p>";
                if (boolOVC_IS_STRATEGY)
                {
                    if (strODT_VALIDITY_DATE.Equals(string.Empty))
                        strMessage += "<P> 請選擇 有效期限 </p>";
                    if (strOVC_LICENSE_NO.Equals(string.Empty))
                        strMessage += "<P> 請輸入 輸出許可證號碼 </p>";
                    FCommon.checkDateTime(strODT_VALIDITY_DATE, "有效期限", ref strMessage, out dateODT_VALIDITY_DATE);
                }
                if (edf_detail == null)
                    strMessage += "<P> 請先新增 料件 </p>";
                #endregion

                if (strMessage.Equals(string.Empty))
                {
                    #region 更新 TBGMT_EDF
                    EDF.OVC_PURCH_NO = strOVC_PURCH_NO;
                    EDF.OVC_START_PORT = strOVC_START_PORT;
                    EDF.OVC_ARRIVE_PORT = strOVC_ARRIVE_PORT;
                    EDF.OVC_SHIP_FROM = strOVC_SHIP_FROM;
                    EDF.OVC_DEPT_CDE = strOVC_DEPT_CDE;
                    EDF.OVC_REQ_DEPT_CDE = strOVC_REQ_DEPT_CDE;
                    EDF.OVC_CON_ENG_ADDRESS = strOVC_CON_ENG_ADDRESS;
                    EDF.OVC_CON_TEL = strOVC_CON_TEL;
                    EDF.OVC_CON_FAX = strOVC_CON_FAX;
                    EDF.OVC_NP_ENG_ADDRESS = strOVC_NP_ENG_ADDRESS;
                    EDF.OVC_NP_TEL = strOVC_NP_TEL;
                    EDF.OVC_NP_FAX = strOVC_NP_FAX;
                    EDF.OVC_ANP_ENG_ADDRESS = strOVC_ANP_ENG_ADDRESS;
                    EDF.OVC_ANP_TEL = strOVC_ANP_TEL;
                    EDF.OVC_ANP_FAX = strOVC_ANP_FAX;
                    EDF.OVC_ANP_ENG_ADDRESS2 = strOVC_ANP_ENG_ADDRESS2;
                    EDF.OVC_ANP_TEL2 = strOVC_ANP_TEL2;
                    EDF.OVC_ANP_FAX2 = strOVC_ANP_FAX2;
                    EDF.OVC_DELIVER_NAME = strOVC_DELIVER_NAME;
                    EDF.OVC_DELIVER_MOBILE = strOVC_DELIVER_MOBILE;
                    EDF.OVC_DELIVER_MILITARY_LINE = strOVC_DELIVER_MILITARY_LINE;
                    EDF.OVC_PAYMENT_TYPE = strOVC_PAYMENT_TYPE;
                    EDF.OVC_IS_PAY = strOVC_IS_PAY;
                    EDF.OVC_NOTE = strOVC_NOTE;
                    EDF.OVC_IS_STRATEGY = strOVC_IS_STRATEGY;
                    EDF.OVC_IS_RISK = strOVC_IS_RISK;
                    EDF.OVC_IS_ALERTNESS = strOVC_IS_ALERTNESS;
                    if (boolOVC_IS_STRATEGY)
                    {
                        EDF.ODT_VALIDITY_DATE = dateODT_VALIDITY_DATE; //有效期限
                        EDF.OVC_LICENSE_NO = strOVC_LICENSE_NO; //輸出許可證號碼
                    }
                    EDF.ODT_MODIFY_DATE = dateNow;
                    EDF.OVC_MODIFY_LOGIN_ID = strUserId;
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), EDF.GetType().Name, this, "修改");
                    #endregion
                    #region 新增 TBGMT_EDF_MRPLOG
                    TBGMT_EDF_MRPLOG EDF_MRPLOG = new TBGMT_EDF_MRPLOG();
                    if (boolOVC_IS_STRATEGY)
                    {
                        EDF_MRPLOG.ODT_VALIDITY_DATE = dateODT_VALIDITY_DATE; //有效期限
                        EDF_MRPLOG.OVC_LICENSE_NO = strOVC_LICENSE_NO; //輸出許可證號碼
                    }

                    EDF_MRPLOG.LOG_LOGIN_ID = strUserId;
                    EDF_MRPLOG.LOG_TIME = dateNow;
                    EDF_MRPLOG.LOG_EVENT = "UPDATE";
                    EDF_MRPLOG.OVC_EDF_NO = strOVC_EDF_NO;
                    EDF_MRPLOG.OVC_PURCH_NO = strOVC_PURCH_NO;
                    EDF_MRPLOG.OVC_START_PORT = strOVC_START_PORT;
                    EDF_MRPLOG.OVC_ARRIVE_PORT = strOVC_ARRIVE_PORT;
                    EDF_MRPLOG.OVC_SHIP_FROM = strOVC_SHIP_FROM;
                    EDF_MRPLOG.OVC_DEPT_CDE = strOVC_DEPT_CDE;
                    EDF_MRPLOG.OVC_REQ_DEPT_CDE = strOVC_REQ_DEPT_CDE;
                    EDF_MRPLOG.OVC_CON_ENG_ADDRESS = strOVC_CON_ENG_ADDRESS;
                    EDF_MRPLOG.OVC_CON_TEL = strOVC_CON_TEL;
                    EDF_MRPLOG.OVC_CON_FAX = strOVC_CON_FAX;
                    EDF_MRPLOG.OVC_NP_ENG_ADDRESS = strOVC_NP_ENG_ADDRESS;
                    EDF_MRPLOG.OVC_NP_TEL = strOVC_NP_TEL;
                    EDF_MRPLOG.OVC_NP_FAX = strOVC_NP_FAX;
                    EDF_MRPLOG.OVC_ANP_ENG_ADDRESS = strOVC_ANP_ENG_ADDRESS;
                    EDF_MRPLOG.OVC_ANP_TEL = strOVC_ANP_TEL;
                    EDF_MRPLOG.OVC_ANP_FAX = strOVC_ANP_FAX;
                    EDF_MRPLOG.OVC_ANP_ENG_ADDRESS2 = strOVC_ANP_ENG_ADDRESS2;
                    EDF_MRPLOG.OVC_ANP_TEL2 = strOVC_ANP_TEL2;
                    EDF_MRPLOG.OVC_ANP_FAX2 = strOVC_ANP_FAX2;
                    EDF_MRPLOG.OVC_DELIVER_NAME = strOVC_DELIVER_NAME;
                    EDF_MRPLOG.OVC_DELIVER_MOBILE = strOVC_DELIVER_MOBILE;
                    EDF_MRPLOG.OVC_DELIVER_MILITARY_LINE = strOVC_DELIVER_MILITARY_LINE;
                    EDF_MRPLOG.OVC_PAYMENT_TYPE = strOVC_PAYMENT_TYPE;
                    EDF_MRPLOG.OVC_NOTE = strOVC_NOTE;
                    EDF_MRPLOG.OVC_IS_STRATEGY = strOVC_IS_STRATEGY;
                    EDF_MRPLOG.OVC_BLD_NO = EDF.OVC_BLD_NO;
                    EDF_MRPLOG.ODT_CREATE_DATE = EDF.ODT_CREATE_DATE;
                    EDF_MRPLOG.OVC_CREATE_LOGIN_ID = EDF.OVC_CREATE_LOGIN_ID;
                    EDF_MRPLOG.ODT_MODIFY_DATE = dateNow;
                    EDF_MRPLOG.OVC_MODIFY_LOGIN_ID = EDF.OVC_MODIFY_LOGIN_ID;
                    EDF_MRPLOG.EDF_MRP_SN = Guid.NewGuid();
                    MTSE.TBGMT_EDF_MRPLOG.Add(EDF_MRPLOG);
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), EDF_MRPLOG.GetType().Name, this, "新增");
                    #endregion

                    strMessage = $"編號：{ strOVC_EDF_NO } 之外運資料表 更新成功。";
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", strMessage);
                    FCommon.DialogBoxShow(this, strMessage + "繼續管理外運資料表？", $"MTS_A22_1{ getQueryString() }", false);
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "外運資料表 不存在！");
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_A22_1{ getQueryString() }");
        }

        protected void drpOVC_ITEM_COUNT_UNIT_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isShow = drpOVC_ITEM_COUNT_UNIT.SelectedValue.Equals("其他");
            txtOVC_ITEM_COUNT_UNIT.Visible = isShow;
        }

        protected void txtPRICE_TextChanged(object sender, EventArgs e)
        {
            bool boolONB_ITEM_COUNT = decimal.TryParse(txtONB_ITEM_COUNT.Text, out decimal decONB_ITEM_COUNT);
            bool boolPRICE = decimal.TryParse(txtPRICE.Text, out decimal decPRICE);
            if (boolONB_ITEM_COUNT && boolPRICE)
            {
                txtONB_MONEY.Text = (decONB_ITEM_COUNT * decPRICE).ToString();
            }
        }

        protected void txtONB_ITEM_COUNT_TextChanged(object sender, EventArgs e)
        {
            bool boolONB_ITEM_COUNT = decimal.TryParse(txtONB_ITEM_COUNT.Text, out decimal decONB_ITEM_COUNT);
            bool boolPRICE = decimal.TryParse(txtPRICE.Text, out decimal decPRICE);
            if (boolONB_ITEM_COUNT && boolPRICE)
            {
                txtONB_MONEY.Text = (decONB_ITEM_COUNT * decPRICE).ToString();
            }
        }

        protected void rdoOVC_PAYMENT_TYPE_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isShow = !rdoOVC_PAYMENT_TYPE.SelectedValue.Equals("預付");
            rdoOVC_IS_PAY.Visible = isShow;
            rdoOVC_PAYMENT_TYPE_Other.Visible = isShow;
        }
        //protected void rdoOVC_PAYMENT_TYPE_PREPAID_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (rdoOVC_PAYMENT_TYPE_PREPAID.Checked == true)
        //    {
        //        rdoOVC_IS_PAY.Visible = false;
        //        rdoOVC_PAYMENT_TYPE.Visible = false;
        //    }
        //}
        //protected void rdoOVC_PAYMENT_TYPE_COLLECT_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (rdoOVC_PAYMENT_TYPE_COLLECT.Checked == true)
        //    {
        //        rdoOVC_IS_PAY.Visible = true;
        //        rdoOVC_PAYMENT_TYPE.Visible = true;
        //    }
        //}
        protected void chkOVC_IS_STRATEGY_CheckedChanged(object sender, EventArgs e)
        {
            pnStrategy.Visible = chkOVC_IS_STRATEGY.Checked;
        }
    }
}