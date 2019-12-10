using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using System.Data.Entity;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.Web.UI;
using System.IO;
using OfficeOpenXml;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A21 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        public string strDEPT_SN, strDEPT_Name;
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        bool isUpload;

        #region 副程式
        private void dataImport_GV_TBGMT_EDF_DETAIL()
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
        private static string getOVC_EDF_NO(string strDEPT_SN)
        {
            Common FCommon = new Common();
            MTSEntities MTSE = new MTSEntities();
            string yyy = FCommon.getTaiwanDate(DateTime.Now, "{0}").PadLeft(3, '0');
            string strOVC_EDF_NO = "EDF" + yyy + strDEPT_SN;

            TBGMT_EDF edf = MTSE.TBGMT_EDF.Where(table => table.OVC_EDF_NO.StartsWith(strOVC_EDF_NO))
                .OrderByDescending(table => table.OVC_EDF_NO).FirstOrDefault();

            int edf_no_num = 1;
            if (edf != null)
            {
                if (edf.OVC_EDF_NO.Length == 15)
                    edf_no_num = (Convert.ToInt16(edf.OVC_EDF_NO.Substring(11, 4)) + 1);
            }
            strOVC_EDF_NO = strOVC_EDF_NO + edf_no_num.ToString("0000");
            return strOVC_EDF_NO;
        }
        private void Create_EDF()
        {
            #region 取值
            string strMessage = "";
            string strUserId = Session["userid"].ToString(); //資料建立人員
            string strOVC_EDF_NO = getOVC_EDF_NO(strDEPT_SN); //外運資料表編號
            Guid guidEDF_SN = Guid.NewGuid(); //新的Guid，用於儲存時使用

            DateTime dateNow = DateTime.Now;
            #endregion

            TBGMT_EDF edf = MTSE.TBGMT_EDF.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();
            #region 錯誤訊息
            if (strOVC_EDF_NO.Equals(string.Empty))
                strMessage += "<p> 外運資料表編號 錯誤！ </p>";
            else if (edf != null)
            {
                strMessage += $"<p> 編號：{ strOVC_EDF_NO } 之外運資料表 已存在！ </p>";
                FCommon.MessageBoxShow(this, $"編號：{ strOVC_EDF_NO } 之外運資料表 已存在！", "MTS_A21", false);
                return;
            }
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                #region 新增 TBGMT_EDF
                edf = new TBGMT_EDF();
                edf.OVC_EDF_NO = strOVC_EDF_NO;
                edf.EDF_SN = guidEDF_SN;
                edf.ODT_RECEIVE_DATE = dateNow; //申請日期
                edf.ODT_CREATE_DATE = dateNow;
                edf.OVC_CREATE_LOGIN_ID = strUserId;
                edf.ODT_MODIFY_DATE = dateNow;
                edf.OVC_MODIFY_LOGIN_ID = strUserId;
                MTSE.TBGMT_EDF.Add(edf);
                MTSE.SaveChanges();
                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), edf.GetType().Name, this, "新增");
                #endregion

                #region 新增 TBGMT_EDF_MRPLOG
                TBGMT_EDF_MRPLOG EDF_MRPLOG = new TBGMT_EDF_MRPLOG();
                EDF_MRPLOG.LOG_LOGIN_ID = strUserId;
                EDF_MRPLOG.LOG_TIME = dateNow;
                EDF_MRPLOG.LOG_EVENT = "INSERT";
                EDF_MRPLOG.OVC_EDF_NO = strOVC_EDF_NO;
                EDF_MRPLOG.EDF_MRP_SN = Guid.NewGuid();
                EDF_MRPLOG.ODT_RECEIVE_DATE = dateNow; //申請日期
                EDF_MRPLOG.ODT_CREATE_DATE = dateNow;
                EDF_MRPLOG.OVC_CREATE_LOGIN_ID = strUserId;
                EDF_MRPLOG.ODT_MODIFY_DATE = dateNow;
                EDF_MRPLOG.OVC_MODIFY_LOGIN_ID = strUserId;
                MTSE.TBGMT_EDF_MRPLOG.Add(EDF_MRPLOG);
                MTSE.SaveChanges();
                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), EDF_MRPLOG.GetType().Name, this, "新增");
                #endregion
                lblOVC_EDF_NO.Text = strOVC_EDF_NO; //料件使用
                ViewState["EDF_SN"] = guidEDF_SN; //儲存 / 刪除 時使用
                //FCommon.AlertShow(PnMessage, "success", "系統訊息", $"編號：{ strOVC_EDF_NO } 之外運資料表 建立成功。");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this, out isUpload))
            {
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
                    
                    //Create_EDF(); //先新增一筆外運資料，占用自動產生之編號
                    dataImport_GV_TBGMT_EDF_DETAIL();

                    //FCommon.GridView_setEmpty(GV_TBGMT_EDF_DETAIL, strFieldDetail);
                    //pnNewItem.Visible = false;
                    //rdoOVC_PAYMENT_TYPE_COLLECT.Checked = true;
                    //rdoOVC_IS_PAY.Visible = true;
                    //rdoOVC_PAYMENT_TYPE.Visible = true;
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
            //pnNewItem.Visible = true;
        }
        //確定-新增料件
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            #region 取值
            string strUserId = Session["userid"].ToString(); //資料建立人員
            string strOVC_EDF_NO = lblOVC_EDF_NO.Text; //外運資料表編號
            decimal decOVC_EDF_ITEM_NO = 1; //序號
            var query = MTSE.TBGMT_EDF_DETAIL.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).OrderByDescending(d => d.OVC_EDF_ITEM_NO).FirstOrDefault();
            if (query != null) decOVC_EDF_ITEM_NO = query.OVC_EDF_ITEM_NO + 1;
            string strOVC_BOX_NO = txtOVC_BOX_NO.Text; //箱號
            string strOVC_ENG_NAME = txtOVC_ENG_NAME.Text; //英文品名
            string strOVC_CHI_NAME = txtOVC_ITEM_CHI_NAME.Text; //中文品名
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
            if (strOVC_EDF_NO.Equals(string.Empty))
                strMessage += "<P> 外運資料表編號 錯誤！ </p>";
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

            #region 新增料件
            if (strMessage.Equals(string.Empty))
            {
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

                Create_DETAIL_MRPLOG(edf_det_SN, "INSERT"); //新增料件LOG
                                                            //清空欄位
                FCommon.Controls_Clear(pnNewItem, lbOVC_WEIGHT_UNIT);
                //FCommon.Controls_Clear(txtOVC_BOX_NO, txtOVC_ENG_NAME, txtOVC_ITEM_CHI_NAME, txtOVC_ITEM_NO, txtOVC_ITEM_NO2, txtOVC_ITEM_NO3, txtONB_ITEM_COUNT, txtONB_WEIGHT, txtONB_VOLUME, txtONB_LENGTH, txtONB_WIDTH, txtONB_HEIGHT, txtONB_MONEY);
                dataImport_GV_TBGMT_EDF_DETAIL();
                //pnNewItem.Visible = false;
                FCommon.AlertShow(PnMessage, "success", "系統訊息", $"項次：{ decOVC_EDF_ITEM_NO } 之料件，新增成功");
            }
            else
                FCommon.AlertShow(PnMessage_Item, "danger", "系統訊息", strMessage);
            #endregion
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            //pnNewItem.Visible = false;
        }

        protected void GV_TBGMT_EDF_DETAIL_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            Guid id = new Guid(theGridView.DataKeys[gvrIndex].Values["EDF_DET_SN"].ToString()); //EDF_DET_SN

            switch (e.CommandName)
            {
                case "Del":
                    string strUserId = Session["userid"].ToString(); //資料建立人員
                    DateTime dateNow = DateTime.Now;
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
                        //pnNewItem.Visible = false;
                        FCommon.AlertShow(PnMessage_Item, "success", "系統訊息", $"項次：{ strOVC_EDF_ITEM_NO } 之料件，刪除成功");
                    }
                    else
                        FCommon.AlertShow(PnMessage_Item, "danger", "系統訊息", "選取之料件已被刪除，請重新選取。");
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

        #region 舊有新增
        ////新增外運資料表
        //protected void btnNew_Click(object sender, EventArgs e)
        //{
        //    #region 取值
        //    string strMessage = "";
        //    string strUserId = Session["userid"].ToString(); //資料建立人員
        //    string strOVC_EDF_NO = lblOVC_EDF_NO"].ToString(); //外運資料表編號
        //    string strOVC_PURCH_NO = txtOVC_PURCH_NO.Text; //案號
        //    string strOVC_START_PORT = drpOVC_START_PORT.SelectedValue; //啟運港(機場)
        //    string strOVC_ARRIVE_PORT = txtOVC_PORT_CDE.Text; //目的港(機場)
        //    TBM1407 PORT = GME.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("TR") && table.OVC_PHR_ID.Equals(strOVC_START_PORT)).FirstOrDefault();
        //    string strOVC_DEPT_CDE = PORT != null ? PORT.OVC_PHR_PARENTS : "";
        //    strOVC_DEPT_CDE = strOVC_DEPT_CDE.Replace("地區", "").Replace("分遣組", "");
        //    string strOVC_SHIP_FROM = txtOVC_SHIP_FROM.Text;
        //    string strOVC_REQ_DEPT_CDE = strDEPT_SN; //申請單位代碼
        //    string strOVC_CON_ENG_ADDRESS = txtOVC_CON_ENG_ADDRESS.Text;
        //    string strOVC_CON_TEL = txtOVC_CON_TEL.Text;
        //    string strOVC_CON_FAX = txtOVC_CON_FAX.Text;
        //    string strOVC_NP_ENG_ADDRESS = txtOVC_NP_ENG_ADDRESS.Text;
        //    string strOVC_NP_TEL = txtOVC_NP_TEL.Text;
        //    string strOVC_NP_FAX = txtOVC_NP_FAX.Text;
        //    string strOVC_ANP_ENG_ADDRESS = txtOVC_ANP_ENG_ADDRESS.Text;
        //    string strOVC_ANP_TEL = txtOVC_ANP_TEL.Text;
        //    string strOVC_ANP_FAX = txtOVC_ANP_FAX.Text;
        //    string strOVC_ANP_ENG_ADDRESS2 = txtOVC_ANP_ENG_ADDRESS2.Text;
        //    string strOVC_ANP_TEL2 = txtOVC_ANP_TEL2.Text;
        //    string strOVC_ANP_FAX2 = txtOVC_ANP_FAX2.Text;
        //    string strOVC_DELIVER_NAME = txtOVC_DELIVER_NAME.Text; //發貨人名字 
        //    string strOVC_DELIVER_MOBILE = txtOVC_DELIVER_MOBILE.Text; //發貨人手機 
        //    string strOVC_DELIVER_MILITARY_LINE = txtOVC_DELIVER_MILITARY_LINE.Text; //發貨人軍線 
        //    string strOVC_PAYMENT_TYPE = rdoOVC_PAYMENT_TYPE.SelectedValue; //付款方式
        //    string strOVC_IS_PAY = "";
        //    if (strOVC_PAYMENT_TYPE.Equals("預付"))
        //        strOVC_IS_PAY = "1"; //預付為投保
        //    else
        //    {
        //        strOVC_PAYMENT_TYPE = rdoOVC_PAYMENT_TYPE_Other.SelectedValue;
        //        strOVC_IS_PAY = rdoOVC_IS_PAY.SelectedValue;
        //    }
        //    string strOVC_NOTE = txtOVC_NOTE.Text; //備考
        //    bool boolOVC_IS_STRATEGY = chkOVC_IS_STRATEGY.Checked;
        //    string strOVC_IS_STRATEGY = boolOVC_IS_STRATEGY ? "是" : "否";
        //    string strODT_VALIDITY_DATE = txtODT_VALIDITY_DATE.Text;

        //    DateTime dateODT_VALIDITY_DATE, dateNow = DateTime.Now;
        //    #endregion

        //    TBGMT_EDF edf = MTSE.TBGMT_EDF.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();
        //    #region 錯誤訊息
        //    if (edf != null)
        //    {
        //        strMessage += $"<P> 編號：{ strOVC_EDF_NO } 之外運資料表 已存在！ </p>";
        //        FCommon.MessageBoxShow(this, $"編號：{ strOVC_EDF_NO } 之外運資料表 已存在！", "MTS_A21", false);
        //        return;
        //    }
        //    if (strOVC_PURCH_NO.Equals(string.Empty))
        //        strMessage += "<P> 請輸入 案號 </p>";
        //    if (strOVC_START_PORT.Equals(string.Empty))
        //        strMessage += "<P> 請選擇 啟運港(機場) </p>";
        //    if (strOVC_ARRIVE_PORT.Equals(string.Empty))
        //        strMessage += "<P> 請選擇 目的港(機場) </p>";
        //    if (strOVC_CON_ENG_ADDRESS.Equals(string.Empty))
        //        strMessage += "<P> 請輸入 CONSIGNEE 地址 </p>";
        //    if (strOVC_CON_TEL.Equals(string.Empty))
        //        strMessage += "<P> 請輸入 CONSIGNEE 電話 </p>";
        //    if (strOVC_CON_FAX.Equals(string.Empty))
        //        strMessage += "<P> 請輸入 CONSIGNEE 傳真 </p>";
        //    if (strOVC_DELIVER_NAME.Equals(string.Empty))
        //        strMessage += "<P> 請輸入 發貨人資訊 名字 </p>";
        //    if (strOVC_DELIVER_MOBILE.Equals(string.Empty))
        //        strMessage += "<P> 請輸入 發貨人資訊 手機 </p>";
        //    if (strOVC_DELIVER_MILITARY_LINE.Equals(string.Empty))
        //        strMessage += "<P> 請輸入 發貨人資訊 軍線 </p>";
        //    if (strOVC_PAYMENT_TYPE.Equals(string.Empty))
        //        strMessage += "<P> 請選擇 付款方式 </p>";
        //    if (chkOVC_IS_STRATEGY.Checked)
        //    {
        //        if (txtODT_VALIDITY_DATE.Text.Equals(string.Empty))
        //            strMessage += "<P> 請選擇 戰略性高科技貨品 有效期限 </p>";
        //        if (txtOVC_LICENSE_NO.Text.Equals(string.Empty))
        //            strMessage += "<P> 請輸入 戰略性高科技貨品 輸出許可證號碼 </p>";
        //    }
        //    if (GV_TBGMT_EDF_DETAIL.Rows.Count == 0)
        //        strMessage += "<P> 請輸入 料件 </p>";

        //    //確認輸入型態
        //    bool boolODT_VALIDITY_DATE = FCommon.checkDateTime(strODT_VALIDITY_DATE, "有效期限", ref strMessage, out dateODT_VALIDITY_DATE);
        //    #endregion

        //    if (strMessage.Equals(string.Empty))
        //    {
        //        #region 新增 TBGMT_EDF
        //        edf = new TBGMT_EDF();
        //        edf.OVC_EDF_NO = strOVC_EDF_NO;
        //        edf.OVC_PURCH_NO = strOVC_PURCH_NO;
        //        edf.OVC_START_PORT = strOVC_START_PORT;
        //        edf.OVC_ARRIVE_PORT = strOVC_ARRIVE_PORT;
        //        edf.OVC_DEPT_CDE = strOVC_DEPT_CDE;
        //        edf.ODT_RECEIVE_DATE = dateNow; //申請日期
        //        edf.OVC_REQ_DEPT_CDE = strOVC_REQ_DEPT_CDE;
        //        edf.OVC_CON_ENG_ADDRESS = strOVC_CON_ENG_ADDRESS;
        //        edf.OVC_CON_TEL = strOVC_CON_TEL;
        //        edf.OVC_CON_FAX = strOVC_CON_FAX;
        //        edf.OVC_NP_ENG_ADDRESS = strOVC_NP_ENG_ADDRESS;
        //        edf.OVC_NP_TEL = strOVC_NP_TEL;
        //        edf.OVC_NP_FAX = strOVC_NP_FAX;
        //        edf.OVC_ANP_ENG_ADDRESS = strOVC_ANP_ENG_ADDRESS;
        //        edf.OVC_ANP_TEL = strOVC_ANP_TEL;
        //        edf.OVC_ANP_FAX = strOVC_ANP_FAX;
        //        edf.OVC_PAYMENT_TYPE = strOVC_PAYMENT_TYPE;
        //        edf.OVC_NOTE = strOVC_NOTE;
        //        edf.OVC_IS_STRATEGY = strOVC_IS_STRATEGY;
        //        if (boolOVC_IS_STRATEGY)
        //        {
        //            edf.ODT_VALIDITY_DATE = dateODT_VALIDITY_DATE; //有效期限
        //            edf.OVC_LICENSE_NO = txtOVC_LICENSE_NO.Text; //輸出許可證號碼
        //        }
        //        edf.OVC_SHIP_FROM = strOVC_SHIP_FROM;
        //        //edf.OVC_BLD_NO
        //        edf.OVC_IS_PAY = strOVC_IS_PAY;
        //        //edf.OVC_PURCH_NO_OLD = strOVC_PURCH_NO;
        //        edf.EDF_SN = Guid.NewGuid();
        //        edf.OVC_DELIVER_NAME = strOVC_DELIVER_NAME;
        //        edf.OVC_DELIVER_MILITARY_LINE = strOVC_DELIVER_MILITARY_LINE;
        //        edf.OVC_DELIVER_MOBILE = strOVC_DELIVER_MOBILE;
        //        edf.ODT_CREATE_DATE = dateNow;
        //        edf.OVC_CREATE_LOGIN_ID = strUserId;
        //        edf.ODT_MODIFY_DATE = dateNow;
        //        edf.OVC_MODIFY_LOGIN_ID = strUserId;
        //        MTSE.TBGMT_EDF.Add(edf);
        //        MTSE.SaveChanges();
        //        FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), edf.GetType().Name, this, "新增");
        //        #endregion

        //        #region 新增 TBGMT_EDF_MRPLOG
        //        TBGMT_EDF_MRPLOG EDF_MRPLOG = new TBGMT_EDF_MRPLOG();
        //        EDF_MRPLOG.LOG_LOGIN_ID = strUserId;
        //        EDF_MRPLOG.LOG_TIME = dateNow;
        //        EDF_MRPLOG.LOG_EVENT = "INSERT";
        //        EDF_MRPLOG.OVC_EDF_NO = strOVC_EDF_NO;
        //        EDF_MRPLOG.OVC_PURCH_NO = strOVC_PURCH_NO;
        //        EDF_MRPLOG.OVC_START_PORT = strOVC_START_PORT;
        //        EDF_MRPLOG.OVC_ARRIVE_PORT = strOVC_ARRIVE_PORT;
        //        EDF_MRPLOG.ODT_RECEIVE_DATE = dateNow;
        //        EDF_MRPLOG.OVC_DEPT_CDE = strOVC_DEPT_CDE;
        //        EDF_MRPLOG.OVC_REQ_DEPT_CDE = strOVC_REQ_DEPT_CDE;
        //        EDF_MRPLOG.OVC_CON_ENG_ADDRESS = strOVC_CON_ENG_ADDRESS;
        //        EDF_MRPLOG.OVC_CON_TEL = strOVC_CON_TEL;
        //        EDF_MRPLOG.OVC_CON_FAX = strOVC_CON_FAX;
        //        EDF_MRPLOG.OVC_NP_ENG_ADDRESS = strOVC_NP_ENG_ADDRESS;
        //        EDF_MRPLOG.OVC_NP_TEL = strOVC_NP_TEL;
        //        EDF_MRPLOG.OVC_NP_FAX = strOVC_NP_FAX;
        //        EDF_MRPLOG.OVC_ANP_ENG_ADDRESS = strOVC_ANP_ENG_ADDRESS;
        //        EDF_MRPLOG.OVC_ANP_TEL = strOVC_ANP_TEL;
        //        EDF_MRPLOG.OVC_ANP_FAX = strOVC_ANP_FAX;
        //        EDF_MRPLOG.OVC_PAYMENT_TYPE = strOVC_PAYMENT_TYPE;
        //        EDF_MRPLOG.OVC_NOTE = strOVC_NOTE;
        //        EDF_MRPLOG.OVC_IS_STRATEGY = strOVC_IS_STRATEGY;
        //        if (boolOVC_IS_STRATEGY)
        //        {
        //            EDF_MRPLOG.ODT_VALIDITY_DATE = dateODT_VALIDITY_DATE; //有效期限
        //            EDF_MRPLOG.OVC_LICENSE_NO = txtOVC_LICENSE_NO.Text; //輸出許可證號碼
        //        }
        //        EDF_MRPLOG.OVC_SHIP_FROM = strOVC_SHIP_FROM;
        //        //EDF_MRPLOG.OVC_BLD_NO
        //        //EDF_MRPLOG.OVC_IS_PAY = strOVC_IS_PAY;
        //        EDF_MRPLOG.OVC_PURCH_NO_OLD = strOVC_PURCH_NO;
        //        EDF_MRPLOG.EDF_MRP_SN = Guid.NewGuid();
        //        //EDF_MRPLOG.ODT_CREATE_DATE = dateNow;
        //        EDF_MRPLOG.OVC_CREATE_LOGIN_ID = strUserId;
        //        EDF_MRPLOG.ODT_MODIFY_DATE = dateNow;
        //        //EDF_MRPLOG.OVC_MODIFY_LOGIN_ID = strUserId;
        //        MTSE.TBGMT_EDF_MRPLOG.Add(EDF_MRPLOG);
        //        MTSE.SaveChanges();
        //        FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), EDF_MRPLOG.GetType().Name, this, "新增");
        //        #endregion

        //        strMessage = $"編號：{ strOVC_EDF_NO } 之外運資料表 新增成功。";
        //        FCommon.AlertShow(PnMessage, "success", "系統訊息", strMessage);

        //        string strScript =
        //            $@"<script language='javascript'>
        //                if(confirm('{ strMessage }繼續新增外運資料表？'))
        //                    location.href='MTS_A21';
        //                else
        //                    location.href='MTS_A22_1';
        //            </script>";
        //        ClientScript.RegisterStartupScript(GetType(), "MessageBox", strScript);
        //    }
        //    else
        //        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        //}
        #endregion

        protected void btnUpdate_Click(object sender, EventArgs e) //更新外運資料表
        {
            string strMessage = "";
            #region 取值
            string strUserId = Session["userid"].ToString(); //資料建立人員
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

            #region 必填項目
            //if (strOVC_PURCH_NO.Equals(string.Empty))
            //    strMessage += "<P> 請輸入 案號 </p>";
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
            //if (edf_detail == null)
            //    strMessage += "<P> 請先新增 料件 </p>";
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                Create_EDF();
                if (ViewState["EDF_SN"] != null)
                {
                    Guid guidEDF_SN = new Guid(ViewState["EDF_SN"].ToString());
                    TBGMT_EDF EDF = MTSE.TBGMT_EDF.Where(table => table.EDF_SN.Equals(guidEDF_SN)).FirstOrDefault();
                    string strOVC_EDF_NO = EDF.OVC_EDF_NO;
                    if (EDF != null)
                    {

                        //TBGMT_EDF_DETAIL edf_detail = MTSE.TBGMT_EDF_DETAIL.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();

                        if (strMessage.Equals(string.Empty))
                        {
                            #region 更新 TBGMT_EDF
                            EDF.OVC_PURCH_NO = strOVC_PURCH_NO;
                            EDF.OVC_START_PORT = strOVC_START_PORT;
                            EDF.OVC_ARRIVE_PORT = strOVC_ARRIVE_PORT;
                            EDF.ODT_RECEIVE_DATE = dateNow; //申請日期
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
                            EDF_MRPLOG.ODT_RECEIVE_DATE = dateNow; //申請日期
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

                            ViewState["isEffective"] = true;
                            btnClear.Visible = false;
                            strMessage = $"編號：{ strOVC_EDF_NO } 之外運資料表 儲存成功。";
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", strMessage);

                            string strQueryString = "";
                            FCommon.setQueryString(ref strQueryString, "OVC_EDF_NO", strOVC_EDF_NO, true);

                            tbDETAIL.Visible = true;
                            btnNew.Visible = true;

                            //string strScript =
                            //    $@"<script language='javascript'>
                            //            if(confirm('{ strMessage }繼續新增外運資料表？'))
                            //                location.href='MTS_A21';
                            //            else
                            //                location.href='MTS_A22_1{ strQueryString }';
                            //        </script>";
                            //ClientScript.RegisterStartupScript(GetType(), "MessageBox", strScript);
                        }
                        else
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "外運資料表 不存在！");
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "儲存錯誤！");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);

        }
        protected void btnClear_Click(object sender, EventArgs e) //刪除外運資料表
        {
            if (ViewState["isEffective"] != null && bool.TryParse(ViewState["isEffective"].ToString(), out bool isEffective) && isEffective)
            { //資料有效，不須動作

            }
            else
            { //資料無效，刪除資料
                if (ViewState["EDF_SN"] != null)
                {
                    Guid guidEDF_SN = new Guid(ViewState["EDF_SN"].ToString());
                    TBGMT_EDF EDF = MTSE.TBGMT_EDF.Where(table => table.EDF_SN.Equals(guidEDF_SN)).FirstOrDefault();
                    if (EDF != null)
                    {
                        string strOVC_EDF_NO = EDF.OVC_EDF_NO;
                        MTSE.Entry(EDF).State = EntityState.Deleted;
                        MTSE.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), EDF.GetType().Name, this, "刪除");

                        #region 刪除 TBGMT_EDF_DETAIL
                        var queryDetail =
                            from detail in MTSE.TBGMT_EDF_DETAIL
                            where detail.OVC_EDF_NO.Equals(strOVC_EDF_NO)
                            select detail;
                        foreach(TBGMT_EDF_DETAIL detail in queryDetail)
                        {
                            MTSE.Entry(detail).State = EntityState.Deleted;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), detail.GetType().Name, this, "刪除");
                        }
                        #endregion
                    }
                }
                FCommon.Controls_Clear(pnData);
                pnStrategy.Visible = chkOVC_IS_STRATEGY.Checked;
                dataImport_GV_TBGMT_EDF_DETAIL();
            }
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (isUpload)
                if (browse.HasFile && Path.GetExtension(browse.FileName) == ".xlsx")
                {
                    using (ExcelPackage excel = new ExcelPackage(browse.PostedFile.InputStream))
                    {
                        DataTable tbl = new DataTable();
                        var ws = excel.Workbook.Worksheets.First();
                        var hasHeader = true;  // adjust accordingly
                                               // add DataColumns to DataTable
                        foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                            tbl.Columns.Add(hasHeader ? firstRowCell.Text
                                : String.Format("Column {0}", firstRowCell.Start.Column));

                        // add DataRows to DataTable
                        int startRow = hasHeader ? 2 : 1;
                        for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                        {
                            var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                            DataRow row = tbl.NewRow();
                            foreach (var cell in wsRow)
                                row[cell.Start.Column - 1] = cell.Text;
                            tbl.Rows.Add(row);
                        }
                        try
                        {
                            DataRow dr = tbl.Rows[0];
                            #region 判斷是否有重複EDF_NO
                            var query =
                            from tbgmt_edf in MTSE.TBGMT_EDF
                            orderby tbgmt_edf.OVC_EDF_NO
                            select new
                            {
                                OVC_EDF_NO = tbgmt_edf.OVC_EDF_NO,
                            };
                            string temp = "EDF" + DateTime.Now.AddYears(-1911).ToString("yyy") + tbl.Rows[0][4];
                            query = query.Where(table => table.OVC_EDF_NO.Contains(temp));
                            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                            #endregion
                            #region 匯入至TextBox
                            txtOVC_PURCH_NO.Text = dr[0].ToString();


                            //strOVC_S_PORT_CDEE查詢TBGMT_PORTS調出中文港口名稱(啟運港埠)
                            string strOVC_S_PORT_CDE = dr[1].ToString();
                            TBGMT_PORTS S_PORT = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(strOVC_S_PORT_CDE)).FirstOrDefault();
                            string strOVC_S_CHI_NAME = S_PORT != null ? S_PORT.OVC_PORT_CHI_NAME : "";
                            int i = 0;
                            foreach (var strOVC_PORT in drpOVC_START_PORT.Items)
                            {
                                if (strOVC_PORT.ToString() == strOVC_S_CHI_NAME)
                                {
                                    drpOVC_START_PORT.SelectedIndex = i;
                                    break;
                                }
                                i++;
                            }
                            //drpOVC_START_PORT.SelectedItem.Text == (dr[1].ToString());

                            //strOVC_PORT_CDE查詢TBGMT_PORTS調出中文港口名稱  (抵運港埠)
                            string strOVC_PORT_CDE = dr[2].ToString();
                            txtOVC_PORT_CDE.Text = strOVC_PORT_CDE;
                            TBGMT_PORTS A_PORT = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(strOVC_PORT_CDE)).FirstOrDefault();
                            txtOVC_CHI_NAME.Text = A_PORT != null ? A_PORT.OVC_PORT_CHI_NAME : "";
                           // txtOVC_CHI_NAME.Text = "巴黎";

                            txtOVC_CON_ENG_ADDRESS.Text = dr[6].ToString();
                            txtOVC_CON_TEL.Text = dr[7].ToString();
                            txtOVC_CON_FAX.Text = dr[8].ToString();
                            txtOVC_NP_ENG_ADDRESS.Text = dr[10].ToString();
                            txtOVC_NP_TEL.Text = dr[11].ToString();
                            txtOVC_NP_FAX.Text = dr[12].ToString();
                            txtOVC_ANP_ENG_ADDRESS.Text = dr[14].ToString();
                            txtOVC_ANP_TEL.Text = dr[15].ToString();
                            txtOVC_ANP_FAX.Text = dr[16].ToString();
                            string strOVC_PAYMENT_TYPE = dr[17].ToString();
                            string strOVC_IS_PAY = dr[24].ToString();
                            bool isPREPAID = strOVC_PAYMENT_TYPE.Equals("預付") && strOVC_IS_PAY.Equals("1");
                            if (isPREPAID)
                                FCommon.list_setValue(rdoOVC_PAYMENT_TYPE, strOVC_PAYMENT_TYPE);
                            //rdoOVC_PAYMENT_TYPE_PREPAID.Checked = true;
                            else
                            {
                                FCommon.list_setValue(rdoOVC_PAYMENT_TYPE, "到付");
                                FCommon.list_setValue(rdoOVC_IS_PAY, strOVC_IS_PAY);
                                FCommon.list_setValue(rdoOVC_PAYMENT_TYPE_Other, strOVC_PAYMENT_TYPE);
                            }
                            rdoOVC_IS_PAY.Visible = !isPREPAID;
                            rdoOVC_PAYMENT_TYPE_Other.Visible = !isPREPAID;
                            txtOVC_NOTE.Text = dr[18].ToString();
                            txtOVC_SHIP_FROM.Text = dr[20].ToString();
                            txtOVC_ANP_ENG_ADDRESS2.Text = dr[21].ToString();
                            txtOVC_ANP_TEL2.Text = dr[22].ToString();
                            txtOVC_ANP_FAX2.Text = dr[23].ToString();
                            txtOVC_DELIVER_NAME.Text = dr[25].ToString();
                            txtOVC_DELIVER_MILITARY_LINE.Text = dr[26].ToString();
                            txtOVC_DELIVER_MOBILE.Text = dr[27].ToString();
                            {
                                //TBGMT_EDF edf = new TBGMT_EDF();
                                //if (dt.Rows.Count > 0)
                                //{
                                //    string number = dt.Rows[dt.Rows.Count - 1][0].ToString().Substring(11, 4);
                                //    edf.OVC_EDF_NO = "EDF" + DateTime.Now.AddYears(-1911).ToString("yyy") + tbl.Rows[0][4] + (Convert.ToInt16(number) + 1).ToString("0000");
                                //}
                                //else
                                //{
                                //    edf.OVC_EDF_NO = "EDF" + DateTime.Now.AddYears(-1911).ToString("yyy") + tbl.Rows[0][4] + "0001";
                                //}
                                //edf.OVC_DEPT_CDE = dr[3].ToString();
                                //edf.OVC_REQ_DEPT_CDE = dr[4].ToString();
                                //edf.OVC_CON_CHI_ADDRESS = dr[5].ToString();
                                //edf.OVC_NP_CHI_ADDRESS = dr[9].ToString();
                                //edf.OVC_ANP_CHI_ADDRESS = dr[13].ToString();
                                //edf.OVC_PAYMENT_TYPE = dr[17].ToString();
                                //edf.OVC_IS_STRATEGY = dr[19].ToString();
                                //edf.ODT_MODIFY_DATE = DateTime.Now;
                                //edf.OVC_CREATE_LOGIN_ID = Session["userid"].ToString();
                                //edf.OVC_IS_PAY = dr[24].ToString();
                                //edf.EDF_SN = Guid.NewGuid();
                                //edf.OVC_MODIFY_LOGIN_ID = Session["userid"].ToString();
                                //MTSE.TBGMT_EDF.Add(edf);
                                //MTSE.SaveChanges();
                            }
                            #endregion
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "帶入成功。");
                        }
                        catch(Exception ex)
                        {
                            msg = "Excel 帶入失敗，請檢察資料是否正確！\n\n" + ex.ToString() ;
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", msg);
                        }
                    }
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "上傳失敗，副檔名不正確!!");
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無上傳權限！");
        }
        protected void btnUpload2_Click(object sender, EventArgs e)
        {
            if (isUpload)
                if (browse2.HasFile && Path.GetExtension(browse2.FileName) == ".xlsx")
                {
                    using (ExcelPackage excel = new ExcelPackage(browse2.PostedFile.InputStream))
                    {
                        string strSuccess = ""; //成功筆數
                        string strUserId = Session["userid"].ToString();
                        DateTime dateNow = DateTime.Now;
                        DataTable tbl = new DataTable();
                        var ws = excel.Workbook.Worksheets.First();
                        var hasHeader = true;  // adjust accordingly
                                               // add DataColumns to DataTable
                        foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                            tbl.Columns.Add(hasHeader ? firstRowCell.Text
                                : String.Format("Column {0}", firstRowCell.Start.Column));

                        // add DataRows to DataTable
                        int startRow = hasHeader ? 2 : 1;
                        for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                        {
                            var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                            DataRow row = tbl.NewRow();
                            foreach (var cell in wsRow)
                                row[cell.Start.Column - 1] = cell.Text;
                            tbl.Rows.Add(row);
                        }
                        //DataTable dtb = new DataTable();
                        //dtb.Columns.Add("OVC_BOX_NO");
                        //dtb.Columns.Add("OVC_ENG_NAME");
                        //dtb.Columns.Add("OVC_CHI_NAME");
                        //dtb.Columns.Add("OVC_ITEM_NO");
                        //dtb.Columns.Add("OVC_ITEM_NO2");
                        //dtb.Columns.Add("OVC_ITEM_NO3");
                        //dtb.Columns.Add("ONB_ITEM_COUNT");
                        //dtb.Columns.Add("OVC_ITEM_COUNT_UNIT");
                        //dtb.Columns.Add("ONB_WEIGHT");
                        //dtb.Columns.Add("OVC_WEIGHT_UNIT");
                        //dtb.Columns.Add("ONB_VOLUME");
                        //dtb.Columns.Add("OVC_VOLUME_UNIT");
                        //dtb.Columns.Add("ONB_LENGTH");
                        //dtb.Columns.Add("ONB_WIDTH");
                        //dtb.Columns.Add("ONB_HEIGHT");
                        //dtb.Columns.Add("ONB_MONEY");
                        //dtb.Columns.Add("OVC_CURRENCY_NAME");
                        //dtb.Columns.Add("EDF_DET_SN");
                        string strOVC_EDF_NO = lblOVC_EDF_NO.Text;
                        if (!strOVC_EDF_NO.Equals(string.Empty))
                        {
                            for (var i = 1; i < tbl.Rows.Count; i++)
                            {
                                string strNo = (i + 1).ToString();
                                try
                                {
                                    DataRow drl = tbl.Rows[i];
                                    string strMessage = "";
                                    #region 取值
                                    string strOVC_EDF_ITEM_NO = drl[0].ToString();
                                    string strOVC_CHI_NAME = drl[1].ToString();
                                    string strOVC_ENG_NAME = drl[2].ToString();
                                    string strOVC_BOX_NO = drl[3].ToString();
                                    string strOVC_ITEM_NO = drl[4].ToString();
                                    string strOVC_ITEM_NO2 = drl[5].ToString();
                                    string strOVC_ITEM_NO3 = drl[6].ToString();
                                    string strONB_ITEM_COUNT = drl[7].ToString();
                                    string strOVC_ITEM_COUNT_UNIT = drl[8].ToString();
                                    string strONB_WEIGHT = drl[9].ToString();
                                    string strOVC_WEIGHT_UNIT = drl[10].ToString();
                                    string strONB_VOLUME = drl[11].ToString();
                                    string strOVC_VOLUME_UNIT = drl[12].ToString();
                                    string strONB_LENGTH = drl[13].ToString();
                                    string strONB_WIDTH = drl[14].ToString();
                                    string strONB_HEIGHT = drl[15].ToString();
                                    string strONB_MONEY = drl[16].ToString();
                                    string strOVC_CURRENCY = drl[17].ToString();
                                    #endregion
                                    var query =
                                        from detail in MTSE.TBGMT_EDF_DETAIL
                                        where detail.OVC_EDF_NO.Equals(strOVC_EDF_NO)
                                        select detail;
                                    TBGMT_EDF_DETAIL detailFirst = query.FirstOrDefault();
                                    #region 必填項目
                                    if (strOVC_BOX_NO.Equals(string.Empty))
                                        strMessage += "<P> 請輸入 箱號！ </p>";
                                    if (strOVC_ENG_NAME.Equals(string.Empty))
                                        strMessage += "<P> 請輸入 英文品名！ </p>";
                                    if (strOVC_CHI_NAME.Equals(string.Empty))
                                        strMessage += "<P> 請輸入 中文品名！ </p>";
                                    if (strONB_ITEM_COUNT.Equals(string.Empty))
                                        strMessage += "<P> 請輸入 數量！ </p>";
                                    if (strOVC_ITEM_COUNT_UNIT.Equals(string.Empty))
                                        strMessage += "<P> 請輸入 數量單位！ </p>";
                                    else if (detailFirst != null && !strOVC_ITEM_COUNT_UNIT.Equals(detailFirst.OVC_ITEM_COUNT_UNIT))
                                        strMessage += "<P> 數量單位 須與其他單位一致！ </p>";
                                    if (detailFirst != null && !strOVC_VOLUME_UNIT.Equals(detailFirst.OVC_VOLUME_UNIT))
                                        strMessage += "<P> 容積單位 須與其他單位一致！ </p>";
                                    if (strONB_MONEY.Equals(string.Empty))
                                        strMessage += "<P> 請輸入 金額！ </p>";
                                    if (strOVC_CURRENCY.Equals(string.Empty))
                                        strMessage += "<P> 請輸入 幣別！ </p>";
                                    else if (detailFirst != null && !strOVC_CURRENCY.Equals(detailFirst.OVC_CURRENCY))
                                        strMessage += "<P> 幣別 須與其他幣別一致！ </p>";

                                    //確認輸入型態
                                    bool boolOVC_EDF_ITEM_NO = FCommon.checkDecimal(strOVC_EDF_ITEM_NO, "項次", ref strMessage, out decimal decOVC_EDF_ITEM_NO);
                                    bool boolONB_ITEM_COUNT = FCommon.checkDecimal(strONB_ITEM_COUNT, "數量", ref strMessage, out decimal decONB_ITEM_COUNT);
                                    bool boolONB_WEIGHT = FCommon.checkDecimal(strONB_WEIGHT, "重量", ref strMessage, out decimal decONB_WEIGHT);
                                    bool boolONB_VOLUME = FCommon.checkDecimal(strONB_VOLUME, "容積", ref strMessage, out decimal decONB_VOLUME);
                                    bool boolONB_LENGTH = FCommon.checkDecimal(strONB_LENGTH, "體積-長", ref strMessage, out decimal decONB_LENGTH);
                                    bool boolONB_WIDTH = FCommon.checkDecimal(strONB_WIDTH, "體積-寬", ref strMessage, out decimal decONB_WIDTH);
                                    bool boolONB_HEIGHT = FCommon.checkDecimal(strONB_HEIGHT, "體積-高", ref strMessage, out decimal decONB_HEIGHT);
                                    bool boolONB_MONEY = FCommon.checkDecimal(strONB_MONEY, "金額", ref strMessage, out decimal decONB_MONEY);

                                    TBGMT_EDF_DETAIL EDF_DETAIL = query.Where(table => table.OVC_EDF_ITEM_NO == decOVC_EDF_ITEM_NO).FirstOrDefault();
                                    if (EDF_DETAIL != null)
                                        strMessage += "<P> 資料已經存在！ </p>";
                                    #endregion
                                    if (strMessage.Equals(string.Empty))
                                    {
                                        Guid guidEDF_DET_SN = Guid.NewGuid();
                                        #region 匯入TBGMT_EDF_DETAIL
                                        EDF_DETAIL = new TBGMT_EDF_DETAIL();
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
                                        EDF_DETAIL.ONB_WEIGHT = decONB_WEIGHT; //重量
                                        EDF_DETAIL.OVC_WEIGHT_UNIT = strOVC_WEIGHT_UNIT; //重量單位
                                        EDF_DETAIL.ONB_VOLUME = decONB_VOLUME; //容積
                                        EDF_DETAIL.OVC_VOLUME_UNIT = strOVC_VOLUME_UNIT; //容積單位
                                        EDF_DETAIL.ONB_LENGTH = decONB_LENGTH; //長
                                        EDF_DETAIL.ONB_WIDTH = decONB_WIDTH; //寬
                                        EDF_DETAIL.ONB_HEIGHT = decONB_HEIGHT; //高
                                        EDF_DETAIL.ONB_MONEY = decONB_MONEY; //金額
                                        EDF_DETAIL.OVC_CURRENCY = strOVC_CURRENCY; //幣別
                                        EDF_DETAIL.OVC_CREATE_LOGIN_ID = strUserId; //資料建立人員
                                        EDF_DETAIL.ODT_MODIFY_DATE = dateNow; //資料修改日期
                                        EDF_DETAIL.EDF_DET_SN = guidEDF_DET_SN;
                                        MTSE.TBGMT_EDF_DETAIL.Add(EDF_DETAIL);
                                        MTSE.SaveChanges(); //儲存
                                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), EDF_DETAIL.GetType().Name, this, "新增");
                                        #endregion
                                        Create_DETAIL_MRPLOG(guidEDF_DET_SN, "INSERT"); //新增明細LOG

                                        #region 匯入GridViewx
                                        //DataRow dr = dtb.NewRow();
                                        //dr["OVC_BOX_NO"] = strOVC_BOX_NO;
                                        //dr["OVC_ENG_NAME"] = drl[3].ToString();
                                        //dr["OVC_CHI_NAME"] = drl[2].ToString();
                                        //dr["OVC_ITEM_NO"] = drl[5].ToString();
                                        //dr["OVC_ITEM_NO2"] = drl[6].ToString();
                                        //dr["OVC_ITEM_NO3"] = drl[7].ToString();
                                        //dr["ONB_ITEM_COUNT"] = drl[8];
                                        //dr["OVC_ITEM_COUNT_UNIT"] = drl[9].ToString();
                                        //dr["ONB_WEIGHT"] = drl[10];
                                        //dr["OVC_WEIGHT_UNIT"] = drl[11].ToString();
                                        //dr["ONB_VOLUME"] = drl[12].ToString();
                                        //dr["OVC_VOLUME_UNIT"] = drl[13].ToString();
                                        //dr["ONB_LENGTH"] = drl[14].ToString();
                                        //dr["ONB_WIDTH"] = drl[15].ToString();
                                        //dr["ONB_HEIGHT"] = drl[16].ToString();
                                        //dr["ONB_MONEY"] = drl[17].ToString();
                                        //dr["OVC_CURRENCY_NAME"] = drl[18].ToString();
                                        //dr["EDF_DET_SN"] = EDF_DETAIL.EDF_DET_SN;
                                        //dtb.Rows.Add(dr);
                                        #endregion
                                        strSuccess += strSuccess.Equals(string.Empty) ? "" : ", ";
                                        strSuccess += strNo;
                                    }
                                    else
                                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"<p> Excel第 { strNo } 行上傳失敗！ </p>" + strMessage);
                                }
                                catch
                                {
                                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"<p> Excel第 { strNo } 行上傳失敗，請檢察資料是否正確！ </p>");
                                }

                            }
                            if (!strSuccess.Equals(string.Empty))
                            {
                                //FCommon.GridView_dataImport(GV_TBGMT_EDF_DETAIL, dtb);
                                dataImport_GV_TBGMT_EDF_DETAIL();
                                FCommon.AlertShow(PnMessage, "success", "系統訊息", $"Excel第 { strSuccess } 行上傳成功。");
                            }
                        }
                        else
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "外運資料表編號 錯誤！");
                    }
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "副檔名不正確！");
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無上傳權限！");
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            string strScript =
                $@"<script language='javascript'>
                        if(confirm('繼續新增外運資料表？'))
                            location.href='MTS_A21';
                    </script>";
            ClientScript.RegisterStartupScript(GetType(), "MessageBox", strScript);
        }

        protected void drpOVC_ITEM_COUNT_UNIT_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isShow = drpOVC_ITEM_COUNT_UNIT.SelectedValue.Equals("其他");
            txtOVC_ITEM_COUNT_UNIT.Visible = isShow;
        }

        protected void txtONB_ITEM_COUNT_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtONB_ITEM_COUNT.Text, out decimal decONB_ITEM_COUNT))
                decONB_ITEM_COUNT = decimal.Parse(txtONB_ITEM_COUNT.Text);
            if (decimal.TryParse(txtPrice.Text, out decimal decPrice))
                decPrice = decimal.Parse(txtPrice.Text);

            if (!decONB_ITEM_COUNT.Equals(string.Empty) && !decPrice.Equals(string.Empty))
                txtONB_MONEY.Text = (decONB_ITEM_COUNT * decPrice).ToString();
        }

        protected void txtONB_MONEY_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtONB_ITEM_COUNT.Text, out decimal decONB_ITEM_COUNT))
                decONB_ITEM_COUNT = decimal.Parse(txtONB_ITEM_COUNT.Text);
            if (decimal.TryParse(txtPrice.Text, out decimal decPrice))
                decPrice = decimal.Parse(txtPrice.Text);

            if (!decONB_ITEM_COUNT.Equals(string.Empty) && !decPrice.Equals(string.Empty))
                txtONB_MONEY.Text = (decONB_ITEM_COUNT * decPrice).ToString();
        }

        protected void rdoOVC_PAYMENT_TYPE_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isShow = !rdoOVC_PAYMENT_TYPE.SelectedValue.Equals("預付");
            rdoOVC_IS_PAY.Visible = isShow;
            rdoOVC_PAYMENT_TYPE_Other.Visible = isShow;
        }
        //protected void rdoOVC_PAYMENT_TYPE_COLLECT_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (rdoOVC_PAYMENT_TYPE_COLLECT.Checked == true)
        //    {
        //        rdoOVC_IS_PAY.Visible = true;
        //        rdoOVC_PAYMENT_TYPE.Visible = true;
        //    }
        //}
        //protected void rdoOVC_PAYMENT_TYPE_PREPAID_CheckedChanged(object sender, EventArgs e)
        //{
        //    if(rdoOVC_PAYMENT_TYPE_PREPAID.Checked == true)
        //    {
        //        rdoOVC_IS_PAY.Visible = false;
        //        rdoOVC_PAYMENT_TYPE.Visible = false;
        //    }
        //}
        protected void chkOVC_IS_STRATEGY_CheckedChanged(object sender, EventArgs e)
        {
            pnStrategy.Visible = chkOVC_IS_STRATEGY.Checked;
        }
    }
}