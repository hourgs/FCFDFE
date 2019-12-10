using System;
using System.Linq;
using System.Web.UI;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A23_3 : Page
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

                    FCommon.getQueryString(this, "id", out string id, true);
                    if (Guid.TryParse(id, out Guid guidEDF_SN))
                    {
                        TBGMT_EDF edf = MTSE.TBGMT_EDF.Where(table => table.EDF_SN.Equals(guidEDF_SN)).FirstOrDefault();
                        if (edf != null)
                        {
                            lblOVC_EDF_NO.Text = edf.OVC_EDF_NO;
                            lblOVC_PURCH_NO.Text = edf.OVC_PURCH_NO;

                            TBGMT_PORTS port_start = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(edf.OVC_START_PORT)).FirstOrDefault();
                            if (port_start != null)
                                lblOVC_START_PORT.Text = port_start.OVC_PORT_CHI_NAME;

                            TBGMT_PORTS port_arr = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(edf.OVC_ARRIVE_PORT)).FirstOrDefault();
                            if (port_arr != null)
                                lblOVC_ARRIVE_PORT.Text = port_arr.OVC_PORT_CHI_NAME;
                        }
                    }
                    else
                        FCommon.MessageBoxShow(this, "編號 錯誤！", $"MTS_A23_1{ getQueryString() }", false);
                }
            }
        }
        
        protected void btnNew_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strOVC_EDF_NO = lblOVC_EDF_NO.Text;
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

            #region 錯誤訊息
            TBGMT_ESO eso = MTSE.TBGMT_ESO.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();
            if (eso != null)
                strMessage += "<P> 此外運資料表之訂艙單 已存在 </p>";
            if (strOVC_PURCH_MSG_NO1.Length != 7)
                strMessage += "<P> 三軍申購文號 日期不齊全 </p>";
            if (strOVC_PURCH_MSG_NO2.Equals(string.Empty))
                strMessage += "<P> 請輸入 三軍申購文號 字 </p>";
            if (strOVC_PURCH_MSG_NO3.Equals(string.Empty))
                strMessage += "<P> 請輸入 三軍申購文號 號 </p>";
            int intLenPROCESS = strOVC_PROCESS_NO1.Length;
            if (intLenPROCESS != 0 && intLenPROCESS != 7) //僅能全不選或全選
                strMessage += "<P> 辦理免稅文號 日期不齊全 </p>";
            if (strOVC_SO_NO.Equals(string.Empty))
                strMessage += "<P> 請輸入 S/O No. </p>";
            #endregion
            
            if (strMessage.Equals(string.Empty))
            {
                try
                {
                    string strUserId = Session["userid"].ToString();
                    DateTime dateNow = DateTime.Now;
                    TBGMT_EDF edf = MTSE.TBGMT_EDF.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();
                    if (edf != null)
                    {
                        Guid edf_sn = edf.EDF_SN;

                        #region TBGMT_eso 新增資料
                        eso = new TBGMT_ESO();
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
                        MTSE.SaveChanges();
                        FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), eso.GetType().Name, this, "新增");

                        strMessage = $"編號：{ strOVC_EDF_NO } 訂艙單 新增成功。";
                        FCommon.AlertShow(PnWarning, "success", "系統訊息", strMessage);
                        btnBack.Visible = true;
                        FCommon.DialogBoxShow(this, $"{ strMessage }繼續建立訂艙單。", $"MTS_A23_1{ getQueryString() }", false);
                    }
                    else
                        FCommon.AlertShow(PnWarning, "danger", "系統訊息", $"外運資料表編號：{ strOVC_EDF_NO }，不存在！");
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