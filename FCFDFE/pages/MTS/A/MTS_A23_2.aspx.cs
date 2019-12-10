using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A23_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        Common FCommon = new Common();

        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_DEPT_CDE", Request.QueryString["OVC_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_EDF_NO", Request.QueryString["OVC_EDF_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_START_PORT", Request.QueryString["OVC_START_PORT"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_ARRIVE_PORT", Request.QueryString["OVC_ARRIVE_PORT"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", Request.QueryString["OVC_TRANSER_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_IS_STRATEGY", Request.QueryString["OVC_IS_STRATEGY"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_REVIEW_STATUS", Request.QueryString["OVC_REVIEW_STATUS"], false);
            FCommon.setQueryString(ref strQueryString, "EDF_SN", Request.QueryString["EDF_SN"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_APPLY_DATE_S", Request.QueryString["ODT_APPLY_DATE_S"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_APPLY_DATE_E", Request.QueryString["ODT_APPLY_DATE_E"], false);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_CHI_NAME);
                    DateTime dateNow = DateTime.Now;
                    #region 匯入下拉式選單
                    int theYear = FCommon.getTaiwanYear(dateNow);
                    string theMonth = dateNow.Month.ToString().PadLeft(2, '0');
                    string theDay = dateNow.Day.ToString().PadLeft(2, '0');
                    int yearMax = theYear + 10, yearMin = theYear - 15;

                    FCommon.list_dataImportNumber(drpOVC_PURCH_MSG_NO1_Y, 2, yearMax, yearMin);
                    FCommon.list_setValue(drpOVC_PURCH_MSG_NO1_Y, theYear.ToString());
                    FCommon.list_dataImportNumber(drpOVC_PURCH_MSG_NO1_M, 2, 12);
                    FCommon.list_setValue(drpOVC_PURCH_MSG_NO1_M, theMonth);
                    FCommon.list_dataImportDay(drpOVC_PURCH_MSG_NO1_Y, drpOVC_PURCH_MSG_NO1_M, drpOVC_PURCH_MSG_NO1_D);
                    FCommon.list_setValue(drpOVC_PURCH_MSG_NO1_D, theDay);

                    FCommon.list_dataImportNumber(drpOVC_PROCESS_NO1_Y, 2, yearMax, yearMin);
                    FCommon.list_setValue(drpOVC_PROCESS_NO1_Y, theYear.ToString());
                    FCommon.list_dataImportNumber(drpOVC_PROCESS_NO1_M, 2, 12);
                    FCommon.list_setValue(drpOVC_PROCESS_NO1_M, theMonth);
                    FCommon.list_dataImportDay(drpOVC_PROCESS_NO1_Y, drpOVC_PROCESS_NO1_M, drpOVC_PROCESS_NO1_D);
                    FCommon.list_setValue(drpOVC_PROCESS_NO1_D, theDay);

                    CommonMTS.list_dataImport_SHIP_COMPANY(drpOVC_SHIP_COMPANY, true); //承運航商
                    #endregion

                    try
                    {
                        //外運資料表編號編碼
                        string yyy = FCommon.getTaiwanDate(DateTime.Now, "{0}").PadLeft(3, '0');
                        string judge_eso_no = $"EDF{ yyy }99900";
                        int iho_eso_num = 1;
                        TBGMT_ESO eso_no = MTSE.TBGMT_ESO.Where(table => table.OVC_EDF_NO.StartsWith(judge_eso_no)).OrderByDescending(table => table.OVC_EDF_NO).FirstOrDefault();
                        if (eso_no != null)
                            iho_eso_num = Convert.ToInt16(eso_no.OVC_EDF_NO.Substring(11, 4)) + 1;

                        lblOVC_EDF_NO.Text = judge_eso_no + iho_eso_num.ToString("0000");

                        //啟運港(機場)
                        DataTable dtOVC_PORT = CommonStatic.ListToDataTable(MTSE.TBGMT_PORTS.Where(table => table.OVC_IS_ABROAD == "國內").ToList());
                        FCommon.list_dataImport(drpOVC_START_PORT, dtOVC_PORT, "OVC_PORT_CHI_NAME", "OVC_PORT_CDE", true);
                    }
                    catch { }
                }
            }
        }
        
        protected void btnResetPort_Click(object sender, EventArgs e)
        {
            txtOVC_PORT_CDE.Text = string.Empty;
            txtOVC_CHI_NAME.Text = string.Empty;
        }
        
        protected void btnNew_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strOVC_EDF_NO = lblOVC_EDF_NO.Text;
            string strOVC_PURCH_NO = txtOVC_PURCH_NO.Text;
            string strOVC_START_PORT = drpOVC_START_PORT.SelectedValue;
            string strOVC_ARRIVE_PORT = txtOVC_PORT_CDE.Text;
            string strOVC_PURCH_MSG_NO1_Y = drpOVC_PURCH_MSG_NO1_Y.SelectedValue;
            string strOVC_PURCH_MSG_NO1_M = drpOVC_PURCH_MSG_NO1_M.SelectedValue;
            string strOVC_PURCH_MSG_NO1_D = drpOVC_PURCH_MSG_NO1_D.SelectedValue;
            string strOVC_PURCH_MSG_NO1 = strOVC_PURCH_MSG_NO1_Y.PadLeft(3,'0') + strOVC_PURCH_MSG_NO1_M.PadLeft(2, '0') + strOVC_PURCH_MSG_NO1_D.PadLeft(2, '0');
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

            #region 錯誤訊息
            if (strOVC_EDF_NO.Equals(string.Empty))
                strMessage += "<P> 請輸入 外運資料表編號 </p>";
            else
            {
                TBGMT_EDF edf = MTSE.TBGMT_EDF.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();
                TBGMT_ESO eso = MTSE.TBGMT_ESO.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();
                if (edf != null)
                    strMessage += "<P> 編號 於外運資料表中已存在！ </p>";
                if (eso != null)
                    strMessage += "<P> 編號 於訂艙單中已存在！ </p>";
            }
            if (strOVC_PURCH_NO.Equals(string.Empty))
                strMessage += "<P> 請輸入 案號 </p>";
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
                strMessage += "<P> 請輸入 S/O No. </p>";
            #endregion
            
            if (strMessage.Equals(string.Empty))
            {
                try
                {
                    DateTime dateNow = DateTime.Now;
                    Guid edf_sn = Guid.NewGuid();
                    #region TBGMT_eso 新增資料
                    TBGMT_ESO eso = new TBGMT_ESO();
                    eso.OVC_EDF_NO = strOVC_EDF_NO;
                    eso.OVC_PURCH_MSG_NO1 = strOVC_PURCH_MSG_NO1;
                    eso.OVC_PURCH_MSG_NO2 = strOVC_PURCH_MSG_NO2;
                    eso.OVC_PURCH_MSG_NO3 = strOVC_PURCH_MSG_NO3;
                    eso.OVC_PROCESS_NO1 = strOVC_PROCESS_NO1;
                    eso.OVC_PROCESS_NO2 = strOVC_PROCESS_NO2;
                    eso.OVC_PROCESS_NO3 = strOVC_PROCESS_NO3;
                    eso.OVC_SHIP_COMPANY = strOVC_SHIP_COMPANY;
                    eso.OVC_SO_NO = strOVC_SO_NO;
                    eso.ODT_MODIFY_DATE = dateNow;
                    eso.OVC_CREATE_LOGIN_ID = strUserId;
                    eso.ESO_SN = Guid.NewGuid();
                    eso.EDF_SN = edf_sn;

                    MTSE.TBGMT_ESO.Add(eso);
                    #endregion

                    #region TBGMT_EDF 新增資料
                    TBGMT_EDF edf = new TBGMT_EDF();
                    edf.OVC_EDF_NO = strOVC_EDF_NO;
                    edf.OVC_PURCH_NO = strOVC_PURCH_NO;
                    edf.OVC_START_PORT = strOVC_START_PORT;
                    edf.OVC_ARRIVE_PORT = strOVC_ARRIVE_PORT;
                    edf.ODT_MODIFY_DATE = dateNow;
                    edf.OVC_CREATE_LOGIN_ID = strUserId;
                    edf.EDF_SN = edf_sn;

                    MTSE.TBGMT_EDF.Add(edf);
                    #endregion

                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), edf.GetType().Name, this, "新增");
                    strMessage = $"編號：{ strOVC_EDF_NO } 外運資料表、訂艙單 新增成功。";
                    FCommon.AlertShow(PnWarning, "success", "系統訊息", strMessage);
                    btnBack.Visible = true;
                    FCommon.DialogBoxShow(this, $"{ strMessage }繼續建立訂艙單", $"MTS_A23_1{ getQueryString() }", false);
                }
                catch
                {
                    FCommon.AlertShow(PnWarning, "danger", "系統訊息", "新增失敗，請聯絡工程師。");
                }
            }
            else
                FCommon.AlertShow(PnWarning, "danger", "系統訊息", strMessage);
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_A23_1{ getQueryString() }");
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