using System;
using System.Linq;
using System.Web.UI;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A24_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        Common FCommon = new Common();
        string strDateFormat = Variable.strDateFormat;

        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_EDF_NO", Request.QueryString["OVC_EDF_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", Request.QueryString["OVC_TRANSER_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_START_PORT", Request.QueryString["OVC_START_PORT"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_ARRIVE_PORT", Request.QueryString["OVC_ARRIVE_PORT"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_SHIP_COMPANY", Request.QueryString["OVC_SHIP_COMPANY"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_SO_NO", Request.QueryString["OVC_SO_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_STORED_COMPANY", Request.QueryString["OVC_STORED_COMPANY"], false);
            return strQueryString;
        }
        private void setChkBld(string strOVC_BLD_NO)
        {
            string strBldText;
            bool isNull = string.IsNullOrEmpty(strOVC_BLD_NO);
            if (isNull)
                strBldText = "建立提單";
            else
            {
                TBGMT_BLD bld = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                if (bld != null)
                {
                    ViewState["OVC_BLD_NO_OLD"] = strOVC_BLD_NO;
                    strBldText = $"修改原提單編號：{ strOVC_BLD_NO } 為新的提單編號";
                }
                else
                    strBldText = $"原提單編號：{ strOVC_BLD_NO } 不存在，建立提單";
            }
            chkbld.Text = strBldText;
            chkbld.Checked = isNull;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtODT_STORED_DATE, txtODT_START_DATE, txtODT_ACT_ARRIVE_DATE, txtODT_CUSTOM_CLR_DATE);
                    DateTime dateNow = DateTime.Now;
                    #region 匯入下拉式選單
                    int theYear = FCommon.getTaiwanYear(dateNow);
                    string theMonth = dateNow.Month.ToString().PadLeft(2, '0');
                    string theDay = dateNow.Day.ToString().PadLeft(2, '0');
                    int yearMax = theYear + 10, yearMin = theYear - 15;
                    //三軍申購文號
                    FCommon.list_dataImportNumber(drpOVC_PURCH_MSG_NO1_Y, 2, yearMax, yearMin);
                    FCommon.list_setValue(drpOVC_PURCH_MSG_NO1_Y, theYear.ToString());
                    FCommon.list_dataImportNumber(drpOVC_PURCH_MSG_NO1_M, 2, 12);
                    FCommon.list_setValue(drpOVC_PURCH_MSG_NO1_M, theMonth);
                    FCommon.list_dataImportDay(drpOVC_PURCH_MSG_NO1_Y, drpOVC_PURCH_MSG_NO1_M, drpOVC_PURCH_MSG_NO1_D);
                    FCommon.list_setValue(drpOVC_PURCH_MSG_NO1_D, theDay);
                    //辦理免稅文號
                    FCommon.list_dataImportNumber(drpOVC_PROCESS_NO1_Y, 2, yearMax, yearMin);
                    FCommon.list_setValue(drpOVC_PROCESS_NO1_Y, theYear.ToString());
                    FCommon.list_dataImportNumber(drpOVC_PROCESS_NO1_M, 2, 12);
                    FCommon.list_setValue(drpOVC_PROCESS_NO1_M, theMonth);
                    FCommon.list_dataImportDay(drpOVC_PROCESS_NO1_Y, drpOVC_PROCESS_NO1_M, drpOVC_PROCESS_NO1_D);
                    FCommon.list_setValue(drpOVC_PROCESS_NO1_D, theDay);
                    //進艙時間
                    FCommon.list_dataImportNumber(drpODT_STORED_DATE_H, 2, 23, 0);
                    FCommon.list_dataImportNumber(drpODT_STORED_DATE_M, 2, 59, 0);

                    CommonMTS.list_dataImport_SHIP_COMPANY(drpOVC_SHIP_COMPANY, true); //承運航商
                    CommonMTS.list_dataImport_CONTAINER_TYPE(drpOVC_CONTAINER_TYPE, false); //裝貨區分
                    CommonMTS.list_dataImport_STORED_COMPANY(drpOVC_STORED_COMPANY, false); //進艙廠商
                    #endregion

                    if (FCommon.getQueryString(this, "id", out string strEDF_SN, true) && Guid.TryParse(strEDF_SN, out Guid guidEDF_SN))
                    {
                        TBGMT_EDF edf = MTSE.TBGMT_EDF.Where(table => table.EDF_SN.Equals(guidEDF_SN)).FirstOrDefault();
                        if (edf != null)
                        {
                            string strOVC_EDF_NO = edf.OVC_EDF_NO;
                            lblOVC_EDF_NO.Text = strOVC_EDF_NO;
                            lblOVC_PURCH_NO.Text = edf.OVC_PURCH_NO;

                            TBGMT_PORTS port_start = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(edf.OVC_START_PORT)).FirstOrDefault();
                            if (port_start != null)
                                lblOVC_START_PORT.Text = port_start.OVC_PORT_CHI_NAME;


                            TBGMT_PORTS port_arr = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(edf.OVC_ARRIVE_PORT)).FirstOrDefault();
                            if (port_arr != null)
                                lblOVC_ARRIVE_PORT.Text = port_arr.OVC_PORT_CHI_NAME;

                            TBGMT_ESO eso = MTSE.TBGMT_ESO.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();
                            if (eso != null)
                            {
                                //txtOVC_PURCH_MSG_NO1.Text =  eso.OVC_PURCH_MSG_NO1;
                                string strOVC_PURCH_MSG_NO1 = eso.OVC_PURCH_MSG_NO1 ?? ""; //文號
                                if (strOVC_PURCH_MSG_NO1.Length == 7)
                                {
                                    int year;
                                    if (int.TryParse(strOVC_PURCH_MSG_NO1.Substring(0, 3), out year))
                                        FCommon.list_setValue(drpOVC_PURCH_MSG_NO1_Y, year.ToString());
                                    string month = strOVC_PURCH_MSG_NO1.Substring(3, 2), day = strOVC_PURCH_MSG_NO1.Substring(5, 2);
                                    FCommon.list_setValue(drpOVC_PURCH_MSG_NO1_M, month);
                                    FCommon.list_setValue(drpOVC_PURCH_MSG_NO1_D, day);
                                }
                                txtOVC_PURCH_MSG_NO2.Text = eso.OVC_PURCH_MSG_NO2;
                                txtOVC_PURCH_MSG_NO3.Text = eso.OVC_PURCH_MSG_NO3;
                                //txtOVC_PROCESS_NO1.Text = eso.OVC_PROCESS_NO1;
                                string strOVC_PROCESS_NO1 = eso.OVC_PROCESS_NO1 ?? ""; //文號
                                if (strOVC_PROCESS_NO1.Length == 7)
                                {
                                    int year;
                                    if (int.TryParse(strOVC_PROCESS_NO1.Substring(0, 3), out year))
                                        FCommon.list_setValue(drpOVC_PROCESS_NO1_Y, year.ToString());
                                    string month = strOVC_PROCESS_NO1.Substring(3, 2), day = strOVC_PROCESS_NO1.Substring(5, 2);
                                    FCommon.list_setValue(drpOVC_PROCESS_NO1_M, month);
                                    FCommon.list_setValue(drpOVC_PROCESS_NO1_D, day);
                                }
                                txtOVC_PROCESS_NO2.Text = eso.OVC_PROCESS_NO2;
                                txtOVC_PROCESS_NO3.Text = eso.OVC_PROCESS_NO3;
                                drpOVC_SHIP_COMPANY.SelectedValue = eso.OVC_SHIP_COMPANY;
                                txtOVC_SO_NO.Text = eso.OVC_SO_NO;
                                string strOVC_BLD_NO = eso.OVC_BLD_NO;
                                txtOVC_BLD_NO.Text = strOVC_BLD_NO;
                                setChkBld(strOVC_BLD_NO);
                                txtOVC_SHIP_NAME.Text = eso.OVC_SHIP_NAME;
                                txtOVC_VOYAGE.Text = eso.OVC_VOYAGE;
                                drpOVC_CONTAINER_TYPE.SelectedValue = eso.OVC_CONTAINER_TYPE;
                                txtONB_20_COUNT.Text = (eso.ONB_20_COUNT ?? 0).ToString(); //預設為顯示0
                                txtONB_40_COUNT.Text = (eso.ONB_40_COUNT ?? 0).ToString(); //預設為顯示0
                                txtONB_45_COUNT.Text = (eso.ONB_45_COUNT ?? 0).ToString(); //預設為顯示0
                                string strODT_STORED_DATE_H = "12", strODT_STORED_DATE_M = "00"; //若無資料，預設為12:00
                                if (eso.ODT_STORED_DATE != null)
                                {
                                    DateTime dateODT_STORED_DATE = Convert.ToDateTime(eso.ODT_STORED_DATE);
                                    txtODT_STORED_DATE.Text = dateODT_STORED_DATE.ToString(strDateFormat);
                                    strODT_STORED_DATE_H = dateODT_STORED_DATE.ToString("HH");
                                    strODT_STORED_DATE_M = dateODT_STORED_DATE.ToString("mm");
                                }
                                FCommon.list_setValue(drpODT_STORED_DATE_H, strODT_STORED_DATE_H);
                                FCommon.list_setValue(drpODT_STORED_DATE_M, strODT_STORED_DATE_M);
                                if (eso.ODT_START_DATE != null)
                                    txtODT_START_DATE.Text = Convert.ToDateTime(eso.ODT_START_DATE).ToString(Variable.strDateFormat);
                                if (eso.ODT_ACT_ARRIVE_DATE != null)
                                    txtODT_ACT_ARRIVE_DATE.Text = Convert.ToDateTime(eso.ODT_ACT_ARRIVE_DATE).ToString(Variable.strDateFormat);
                                txtOVC_CUSTOM_CLR_PLACE.Text = eso.OVC_CUSTOM_CLR_PLACE;
                                if (eso.ODT_CUSTOM_CLR_DATE != null)
                                    txtODT_CUSTOM_CLR_DATE.Text = Convert.ToDateTime(eso.ODT_CUSTOM_CLR_DATE).ToString(Variable.strDateFormat);
                                drpOVC_STORED_COMPANY.SelectedValue = eso.OVC_STORED_COMPANY;
                            }
                        }
                    }
                    else
                        FCommon.MessageBoxShow(this, "外運資料表編號 錯誤！", $"MTS_A24_1{ getQueryString() }", false);
                }
            }
        }

        protected void btnModify_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strOVC_PURCH_MSG_NO1_Y = drpOVC_PURCH_MSG_NO1_Y.SelectedValue;
            string strOVC_PURCH_MSG_NO1_M = drpOVC_PURCH_MSG_NO1_M.SelectedValue;
            string strOVC_PURCH_MSG_NO1_D = drpOVC_PURCH_MSG_NO1_D.SelectedValue;
            string strOVC_PURCH_MSG_NO1 = strOVC_PURCH_MSG_NO1_Y.PadLeft(3, '0') + strOVC_PURCH_MSG_NO1_M.PadLeft(2, '0') + strOVC_PURCH_MSG_NO1_D.PadLeft(2, '0');
            string strOVC_PURCH_MSG_NO2 = txtOVC_PURCH_MSG_NO2.Text;
            string strOVC_PURCH_MSG_NO3 = txtOVC_PURCH_MSG_NO3.Text;
            string strOVC_PROCESS_NO1_Y = drpOVC_PROCESS_NO1_Y.SelectedValue;
            string strOVC_PROCESS_NO1_M = drpOVC_PROCESS_NO1_M.SelectedValue;
            string strOVC_PROCESS_NO1_D = drpOVC_PROCESS_NO1_D.SelectedValue;
            string strOVC_PROCESS_NO1 = strOVC_PROCESS_NO1_Y.PadLeft(3, '0') + strOVC_PROCESS_NO1_M.PadLeft(2, '0') + strOVC_PROCESS_NO1_D.PadLeft(2, '0');
            string strOVC_PROCESS_NO2 = txtOVC_PROCESS_NO2.Text;
            string strOVC_PROCESS_NO3 = txtOVC_PROCESS_NO3.Text;
            string strOVC_SHIP_COMPANY = drpOVC_SHIP_COMPANY.SelectedValue;
            string strOVC_SO_NO = txtOVC_SO_NO.Text;
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_SHIP_NAME = txtOVC_SHIP_NAME.Text;
            string strOVC_VOYAGE = txtOVC_VOYAGE.Text;
            string strOVC_CONTAINER_TYPE = drpOVC_CONTAINER_TYPE.SelectedValue;
            string strONB_20_COUNT = txtONB_20_COUNT.Text;
            string strONB_40_COUNT = txtONB_40_COUNT.Text;
            string strONB_45_COUNT = txtONB_45_COUNT.Text;
            string strODT_STORED_DATE = txtODT_STORED_DATE.Text;
            string strODT_STORED_DATE_H = drpODT_STORED_DATE_H.SelectedValue;
            string strODT_STORED_DATE_M = drpODT_STORED_DATE_M.SelectedValue;
            string strODT_START_DATE = txtODT_START_DATE.Text;
            string strODT_ACT_ARRIVE_DATE = txtODT_ACT_ARRIVE_DATE.Text;
            string strOVC_CUSTOM_CLR_PLACE = txtOVC_CUSTOM_CLR_PLACE.Text;
            string strODT_CUSTOM_CLR_DATE = txtODT_CUSTOM_CLR_DATE.Text;
            string strOVC_STORED_COMPANY = drpOVC_STORED_COMPANY.SelectedValue;

            int intONB_20_COUNT, intONB_40_COUNT, intONB_45_COUNT;
            //取得日期型態
            DateTime dateODT_START_DATE, dateODT_ACT_ARRIVE_DATE, dateODT_CUSTOM_CLR_DATE, dateNow = DateTime.Now;
            bool boolODT_START_DATE = DateTime.TryParse(strODT_START_DATE, out dateODT_START_DATE);
            bool boolODT_ACT_ARRIVE_DATE = DateTime.TryParse(strODT_ACT_ARRIVE_DATE, out dateODT_ACT_ARRIVE_DATE);
            bool boolODT_CUSTOM_CLR_DATE = DateTime.TryParse(strODT_CUSTOM_CLR_DATE, out dateODT_CUSTOM_CLR_DATE);
            #region 錯誤訊息
            if (strOVC_BLD_NO.Equals(string.Empty))
                strMessage += "<P> 請輸入 提單號碼</p>";
            if (strOVC_PURCH_MSG_NO1.Length != 7)
                strMessage += "<P> 三軍申購文號 日期不齊全</p>";
            if (strOVC_PURCH_MSG_NO2.Equals(string.Empty))
                strMessage += "<P> 請輸入 三軍申購文號 字</p>";
            if (strOVC_PURCH_MSG_NO3.Equals(string.Empty))
                strMessage += "<P> 請輸入 三軍申購文號 號</p>";
            int intLenPROCESS = strOVC_PROCESS_NO1.Length;
            if (intLenPROCESS != 0 && intLenPROCESS != 7) //僅能全不選或全選
                strMessage += "<P> 辦理免稅文號 日期不齊全</p>";
            if (strOVC_SO_NO.Equals(string.Empty))
                strMessage += "<P> 請輸入 S/O No.</p>";
            //確認輸入型態
            bool boolONB_20_COUNT = FCommon.checkInt(strONB_20_COUNT, "裝貨區分-20公尺", ref strMessage, out intONB_20_COUNT);
            bool boolONB_40_COUNT = FCommon.checkInt(strONB_40_COUNT, "裝貨區分-40公尺", ref strMessage, out intONB_40_COUNT);
            bool boolONB_45_COUNT = FCommon.checkInt(strONB_45_COUNT, "裝貨區分-45公尺", ref strMessage, out intONB_45_COUNT);
            #endregion

            TBGMT_BLD eso_bld = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
            bool isBld = chkbld.Checked;
            if (eso_bld != null && isBld)
                FCommon.AlertShow(PnUpdate, "danger", "系統訊息", $"提單編號：{ strOVC_BLD_NO } 已存在。請使用尚未輸入系統之編號！");
            //{
            //    string strScript =
            //        $@"<script language='javascript'>
            //            alert('提單編號：{ strOVC_BLD_NO } 已存在。
            //                請使用尚未輸入系統之編號！');
            //        </script>";
            //    ClientScript.RegisterStartupScript(GetType(), "MessageBox", strScript);
            //}
            else
            {
                if (strMessage.Equals(string.Empty))
                {
                    try
                    {
                        string strUserId = Session["userid"].ToString();
                        string strOVC_EDF_NO = lblOVC_EDF_NO.Text;

                        #region TBGMT_EDF 修改
                        TBGMT_EDF edf = MTSE.TBGMT_EDF.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();
                        if (edf != null)
                        {
                            edf.OVC_BLD_NO = strOVC_BLD_NO;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), edf.GetType().Name, this, "修改");
                        }
                        else
                            FCommon.AlertShow(PnUpdate, "danger", "系統訊息", $"編號：{ strOVC_EDF_NO } 之外運資料表 不存在");
                        #endregion

                        #region TBGMT_ESO 修改
                        TBGMT_ESO eso = MTSE.TBGMT_ESO.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();
                        if (eso != null)
                        {
                            eso.OVC_PURCH_MSG_NO1 = strOVC_PURCH_MSG_NO1;
                            eso.OVC_PURCH_MSG_NO2 = strOVC_PURCH_MSG_NO2;
                            eso.OVC_PURCH_MSG_NO3 = strOVC_PURCH_MSG_NO3;
                            eso.OVC_PROCESS_NO1 = strOVC_PROCESS_NO1;
                            eso.OVC_PROCESS_NO2 = strOVC_PROCESS_NO2;
                            eso.OVC_PROCESS_NO3 = strOVC_PROCESS_NO3;
                            eso.OVC_SHIP_COMPANY = strOVC_SHIP_COMPANY;
                            eso.OVC_SO_NO = strOVC_SO_NO;
                            eso.OVC_BLD_NO = strOVC_BLD_NO;
                            eso.OVC_SHIP_NAME = strOVC_SHIP_NAME;
                            eso.OVC_VOYAGE = strOVC_VOYAGE;
                            eso.OVC_CONTAINER_TYPE = strOVC_CONTAINER_TYPE;
                            if (boolONB_20_COUNT) eso.ONB_20_COUNT = intONB_20_COUNT; else eso.ONB_20_COUNT = null;
                            if (boolONB_40_COUNT) eso.ONB_40_COUNT = intONB_40_COUNT; else eso.ONB_40_COUNT = null;
                            if (boolONB_45_COUNT) eso.ONB_45_COUNT = intONB_45_COUNT; else eso.ONB_45_COUNT = null;

                            if (!strODT_STORED_DATE.Equals(string.Empty))
                            {
                                string date = Convert.ToDateTime(strODT_STORED_DATE).ToString("yyyyMMdd") + strODT_STORED_DATE_H + strODT_STORED_DATE_M + "00";
                                DateTime STORED_DATE = DateTime.ParseExact(date, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                                eso.ODT_STORED_DATE = STORED_DATE;
                            }

                            if (boolODT_START_DATE)
                                eso.ODT_START_DATE = dateODT_START_DATE;
                            else eso.ODT_START_DATE = null;
                            if (boolODT_ACT_ARRIVE_DATE) eso.ODT_ACT_ARRIVE_DATE = dateODT_ACT_ARRIVE_DATE; else eso.ODT_ACT_ARRIVE_DATE = null;
                            eso.OVC_CUSTOM_CLR_PLACE = strOVC_CUSTOM_CLR_PLACE;
                            if (boolODT_CUSTOM_CLR_DATE) eso.ODT_CUSTOM_CLR_DATE = dateODT_CUSTOM_CLR_DATE; else eso.ODT_CUSTOM_CLR_DATE = null;
                            eso.OVC_STORED_COMPANY = strOVC_STORED_COMPANY;
                            eso.ODT_MODIFY_DATE = dateNow;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), eso.GetType().Name, this, "修改");
                        }
                        else
                            FCommon.AlertShow(PnUpdate, "danger", "系統訊息", $"編號：{ strOVC_EDF_NO } 之訂艙單 不存在");
                        #endregion

                        #region TBGMT_BLD 新增或修改
                        if (isBld) //修改或建立提單
                        {
                            if (ViewState["OVC_BLD_NO_OLD"] != null)
                            {
                                string strOVC_BLD_NO_OLD = ViewState["OVC_BLD_NO_OLD"].ToString();
                                eso_bld = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO_OLD)).FirstOrDefault();
                                if (eso_bld != null) //修改提單編號
                                {
                                    eso_bld.OVC_BLD_NO = strOVC_BLD_NO;
                                    MTSE.SaveChanges();
                                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), eso_bld.GetType().Name, this, "修改");
                                    ViewState["OVC_BLD_NO_OLD"] = strOVC_BLD_NO;
                                    chkbld.Text = $"修改原提單編號：{ strOVC_BLD_NO } 為新的提單編號";
                                    FCommon.AlertShow(PnUpdate, "success", "系統訊息", $"修改提單編號：{ strOVC_BLD_NO_OLD } 為 { strOVC_BLD_NO } 成功");
                                }
                                else
                                    FCommon.AlertShow(PnUpdate, "danger", "系統訊息", $"提單編號：{ strOVC_BLD_NO_OLD } 不存在");
                            }
                            else //建立提單編號
                            {
                                string strOVC_SEA_OR_AIR = "";
                                if (new string[] { "中華航空", "長榮航空" }.Contains(strOVC_SHIP_COMPANY))
                                    strOVC_SEA_OR_AIR = "空運";
                                else if (new string[] { "長榮海運", "陽明海運" }.Contains(strOVC_SHIP_COMPANY))
                                    strOVC_SEA_OR_AIR = "海運";

                                string strOVC_START_PORT = edf.OVC_START_PORT;
                                string strOVC_ARRIVE_PORT = edf.OVC_ARRIVE_PORT;
                                string strOVC_STATUS = "A"; //新增進暫存區
                                TBGMT_BLD bld = new TBGMT_BLD();
                                #region TBGMT_BLD 新增資料
                                bld.OVC_BLD_NO = strOVC_BLD_NO;
                                bld.OVC_SHIP_COMPANY = strOVC_SHIP_COMPANY;
                                bld.OVC_SEA_OR_AIR = strOVC_SEA_OR_AIR;
                                bld.OVC_SHIP_NAME = strOVC_SHIP_NAME;
                                bld.OVC_VOYAGE = strOVC_VOYAGE;
                                if (boolODT_START_DATE) bld.ODT_START_DATE = dateODT_START_DATE; else bld.ODT_START_DATE = null;
                                bld.OVC_START_PORT = strOVC_START_PORT;
                                if (boolODT_ACT_ARRIVE_DATE) bld.ODT_ACT_ARRIVE_DATE = dateODT_ACT_ARRIVE_DATE; else bld.ODT_ACT_ARRIVE_DATE = null;
                                bld.OVC_ARRIVE_PORT = strOVC_ARRIVE_PORT;
                                bld.OVC_STATUS = strOVC_STATUS;
                                bld.ODT_CREATE_DATE = dateNow;
                                bld.ODT_MODIFY_DATE = dateNow;
                                bld.OVC_CREATE_LOGIN_ID = strUserId;
                                bld.OVC_MODIFY_LOGIN_ID = strUserId;
                                bld.BLD_SN = Guid.NewGuid();

                                MTSE.TBGMT_BLD.Add(bld);
                                MTSE.SaveChanges();
                                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), bld.GetType().Name, this, "新增");
                                #endregion

                                #region TBGMT_BLD_MRPLOG 新增LOG
                                TBGMT_BLD_MRPLOG bld_log = new TBGMT_BLD_MRPLOG();
                                bld_log.LOG_LOGIN_ID = strUserId;
                                bld_log.LOG_TIME = dateNow;
                                bld_log.LOG_EVENT = "INSERT";
                                bld_log.OVC_BLD_NO = strOVC_BLD_NO;
                                bld_log.OVC_SHIP_COMPANY = strOVC_SHIP_COMPANY;
                                bld_log.OVC_SEA_OR_AIR = strOVC_SEA_OR_AIR;
                                bld_log.OVC_SHIP_NAME = strOVC_SHIP_NAME;
                                bld_log.OVC_VOYAGE = strOVC_VOYAGE;
                                if (boolODT_START_DATE) bld_log.ODT_START_DATE = dateODT_START_DATE; else bld_log.ODT_START_DATE = null;
                                bld_log.OVC_START_PORT = strOVC_START_PORT;
                                if (boolODT_ACT_ARRIVE_DATE) bld_log.ODT_ACT_ARRIVE_DATE = dateODT_ACT_ARRIVE_DATE; else bld_log.ODT_ACT_ARRIVE_DATE = null;
                                bld_log.OVC_ARRIVE_PORT = strOVC_ARRIVE_PORT;
                                bld_log.OVC_STATUS = strOVC_STATUS;
                                bld_log.ODT_CREATE_DATE = dateNow;
                                bld_log.ODT_MODIFY_DATE = dateNow;
                                bld_log.OVC_CREATE_LOGIN_ID = strUserId;
                                bld_log.MRPLOG_SN = Guid.NewGuid();

                                MTSE.TBGMT_BLD_MRPLOG.Add(bld_log);
                                MTSE.SaveChanges();
                                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), bld_log.GetType().Name, this, "新增");
                                #endregion
                                FCommon.AlertShow(PnUpdate, "success", "系統訊息", $"提單編號：{ strOVC_BLD_NO } 建立成功");
                            }
                        }
                        #endregion

                        setChkBld(strOVC_BLD_NO);
                        FCommon.AlertShow(PnUpdate, "success", "系統訊息", $"編號：{ strOVC_EDF_NO } 之訂艙單 修改成功");
                    }
                    catch
                    {
                        FCommon.AlertShow(PnUpdate, "danger", "系統訊息", "修改失敗，請聯絡工程師。");
                    }
                }
                else
                    FCommon.AlertShow(PnUpdate, "danger", "系統訊息", strMessage);
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_A24_1{ getQueryString() }");
        }

        protected void drpOVC_PURCH_MSG_NO1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FCommon.list_dataImportDay(drpOVC_PURCH_MSG_NO1_Y, drpOVC_PURCH_MSG_NO1_M, drpOVC_PURCH_MSG_NO1_D);
        }
        
        protected void drpOVC_PROCESS_NO1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FCommon.list_dataImportDay(drpOVC_PROCESS_NO1_Y, drpOVC_PROCESS_NO1_M, drpOVC_PROCESS_NO1_D);
        }
    }
}