using System;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.H
{
    public partial class MTS_H11_2 : Page
    {
        public string strDEPT_SN, strDEPT_Name;
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        string strDateFormat = Variable.strDateFormat, strDateFormatSQL = "'yyyy-mm-dd'"; string strOVC_SECTION;

        #region 副程式
        private void dataImport_GV_WRP_BLD()
        {
            string strMessage = "";
            string strODT_WEEK_DATE1 = txtODT_WEEK_DATE1.Text;
            string strODT_WEEK_DATE2 = txtODT_WEEK_DATE2.Text;
            string strOVC_BLD_NO; FCommon.getQueryString(this, "id", out strOVC_BLD_NO, false);
            DateTime dateODT_WEEK_DATE1, dateODT_WEEK_DATE2;
            if (strOVC_SECTION.Equals(string.Empty))
                strMessage += "<p> 接轉地區不存在！ </p>";
            bool bookODT_WEEK_DATE1 = FCommon.checkDateTime(strODT_WEEK_DATE1, "資料時間－前者", ref strMessage, out dateODT_WEEK_DATE1);
            bool bookODT_WEEK_DATE2 = FCommon.checkDateTime(strODT_WEEK_DATE2, "資料時間－後者", ref strMessage, out dateODT_WEEK_DATE2);
            if (strMessage.Equals(string.Empty))
            {
                string[] strParameterName = { ":start_date", ":end_date", ":section", ":bld_no" };
                ArrayList aryData = new ArrayList();
                aryData.Add(strODT_WEEK_DATE1);
                aryData.Add(strODT_WEEK_DATE2);
                aryData.Add(strOVC_SECTION);
                aryData.Add($"%{ strOVC_BLD_NO }%");
                #region SQL語法
                //string sql = "SELECT a.OVC_BLD_NO, b.OVC_CHI_NAME, a.ONB_QUANITY, " +
                //    "CASE WHEN a.OVC_VOLUME_UNIT <> 'CBM' THEN ROUND(NVL(a.ONB_VOLUME,0) / 35.3142,3) ELSE ROUND(NVL(a.ONB_VOLUME,0) ,3) END ONB_VOLUME, " +
                //    "DEPTCDES_TO_NAMES(OVC_RECEIVE_DEPT_CODE) as OVC_RECEIVE_DEPT, " +
                //    "TO_CHAR(ODT_IMPORT_DATE,'yyyy/mm/dd') AS ODT_IMPORT_DATE, " +
                //    "TO_CHAR(ODT_PASS_CUSTOM_DATE,'yyyy/mm/dd') AS ODT_PASS_DATE, " +
                //    "TO_CHAR(ODT_UNPACKING_DATE,'yyyy/mm/dd') AS ODT_STORED_DATE, " +
                //    "TO_CHAR(ODT_TRANSFER_DATE,'yyyy/mm/dd') AS ODT_TRANSFER_DATE " +
                //    "FROM TBGMT_BLD a, TBGMT_ICR b WHERE (b.ODT_TRANSFER_DATE BETWEEN " + wedDate + " AND " + tueDate + ") AND " +
                //    " a.OVC_BLD_NO = b.OVC_BLD_NO (+) AND " +
                //    " a.OVC_ARRIVE_PORT IN (" + portCond + ") " +
                //    "UNION " +
                //    "SELECT a.OVC_BLD_NO, e.OVC_CHI_NAME, a.ONB_QUANITY, " +
                //    "CASE WHEN a.OVC_VOLUME_UNIT <> 'CBM' THEN ROUND(NVL(a.ONB_VOLUME,0) / 35.3142,3) ELSE ROUND(NVL(a.ONB_VOLUME,0) ,3) END ONB_VOLUME, " +
                //    "'' as OVC_RECEIVE_DEPT, " +
                //    "'' AS ODT_IMPORT_DATE, " +
                //    "TO_CHAR(ODT_PASS_DATE,'yyyy/mm/dd') AS ODT_PASS_DATE, " +
                //    "TO_CHAR(ODT_STORED_DATE,'yyyy/mm/dd') AS ODT_STORED_DATE, " +
                //    "'' AS ODT_TRANSFER_DATE " +
                //    "FROM TBGMT_BLD a, TBGMT_ESO c, TBGMT_ETR d, TBGMT_EDF_DETAIL e WHERE (c.ODT_STORED_DATE BETWEEN " + wedDate + " AND " + tueDate + ") AND " +
                //    "a.OVC_BLD_NO = c.OVC_BLD_NO (+) AND  a.OVC_BLD_NO = d.OVC_BLD_NO (+) AND e.OVC_EDF_NO = c.OVC_EDF_NO AND e.OVC_EDF_ITEM_NO = '1' AND " +
                //    " a.OVC_START_PORT IN (" + portCond + ") ";

                //取得接轉地區之port
                string strSQL_PortList = $@"
                    select OVC_PHR_ID from TBM1407 where OVC_PHR_CATE='TR' and OVC_PHR_PARENTS={ strParameterName[2] }
                ";
                //模糊查詢
                string strSQL_OVC_BLD_NO = $@"
                    and a.OVC_BLD_NO like { strParameterName[3] }
                ";
                //接轉作業週報表，已加入週報表之提單，不需顯示
                string strSQL_WRP_BLD = $@"
                    and a.OVC_BLD_NO not in (
                      select OVC_BLD_NO from TBGMT_WRP_BLD
                      --where OVC_SECTION = '基隆地區'
                      --and ODT_WEEK_DATE = to_date('2006-01-18', 'yyyy-mm-dd')
                    )
                ";

                //進口
                string strSQL_Import = $@"
                    select
                    a.OVC_BLD_NO,
                    b.OVC_CHI_NAME,
                    a.ONB_QUANITY,
                    case when a.OVC_VOLUME_UNIT <> 'CBM' then ROUND(NVL(a.ONB_VOLUME,0) / 35.3142,3) else ROUND(NVL(a.ONB_VOLUME,0) ,3) end ONB_VOLUME, 
                    OVC_RECEIVE_DEPT_CODE as OVC_RECEIVE_DEPT,
                    to_char(ODT_IMPORT_DATE, { strDateFormatSQL }) as ODT_IMPORT_DATE,
                    to_char(ODT_PASS_CUSTOM_DATE, { strDateFormatSQL }) as ODT_PASS_DATE,
                    to_char(ODT_UNPACKING_DATE, { strDateFormatSQL }) as ODT_STORED_DATE,
                    to_char(ODT_TRANSFER_DATE, { strDateFormatSQL }) as ODT_TRANSFER_DATE
                    from TBGMT_BLD a, TBGMT_ICR b
                    where a.OVC_BLD_NO = b.OVC_BLD_NO (+)
                    { strSQL_OVC_BLD_NO }
                    { strSQL_WRP_BLD }
                    and a.OVC_ARRIVE_PORT in ({ strSQL_PortList })
                    and b.ODT_TRANSFER_DATE between to_date({ strParameterName[0] }, { strDateFormatSQL }) and to_date({ strParameterName[1] }, { strDateFormatSQL })
                ";
                //出口
                string strSQL_Export = $@"
                    select
                    a.OVC_BLD_NO,
                    e.OVC_CHI_NAME,
                    a.ONB_QUANITY,
                    case when a.OVC_VOLUME_UNIT <> 'CBM' then ROUND(NVL(a.ONB_VOLUME,0) / 35.3142,3) else ROUND(NVL(a.ONB_VOLUME,0) ,3) end ONB_VOLUME,
                    '' as OVC_RECEIVE_DEPT,
                    '' as ODT_IMPORT_DATE,
                    to_char(ODT_PASS_DATE, { strDateFormatSQL }) as ODT_PASS_DATE,
                    to_char(ODT_STORED_DATE, { strDateFormatSQL }) as ODT_STORED_DATE,
                    '' as ODT_TRANSFER_DATE
                    from TBGMT_BLD a, TBGMT_ESO c, TBGMT_ETR d, TBGMT_EDF_DETAIL e
                    where a.OVC_BLD_NO = c.OVC_BLD_NO (+)
                    and a.OVC_BLD_NO = d.OVC_BLD_NO (+)
                    and e.OVC_EDF_NO = c.OVC_EDF_NO
                    and e.OVC_EDF_ITEM_NO = '1'
                    { strSQL_OVC_BLD_NO }
                    { strSQL_WRP_BLD }
                    and a.OVC_START_PORT in ({ strSQL_PortList })
                    and c.ODT_STORED_DATE between to_date({ strParameterName[0] }, { strDateFormatSQL }) and to_date({ strParameterName[1] }, { strDateFormatSQL })
                ";
                string strSQL_BLD = $@"
                    { strSQL_Import }
                    UNION
                    { strSQL_Export }
                ";
                #endregion
                DataTable dt_BLD = FCommon.getDataTableFromSelect(strSQL_BLD, strParameterName, aryData);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_BLD, dt_BLD);
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        
        private void steDateText(DateTime date1, DateTime date2)
        {
            txtODT_WEEK_DATE1.Text = date1.ToString(strDateFormat);
            txtODT_WEEK_DATE2.Text = date2.ToString(strDateFormat);
            lblODT_MONTH.Text = date1.Month.ToString();
            lblODT_WEEK.Text = MTS_H11.NumOfWeek(date1).ToString();
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getAccountDEPTName(this, out strDEPT_SN, out strDEPT_Name);
                if (FCommon.getQueryString(this, "section", out strOVC_SECTION, false))
                {
                    if (!IsPostBack)
                    {
                        FCommon.Controls_Attributes("readonly", "true", txtODT_WEEK_DATE1, txtODT_WEEK_DATE2);

                        lblDEPT.Text = strDEPT_Name;
                        lblOVC_SECTION.Text = strOVC_SECTION;
                        #region 設定預設日期
                        //網址 或 上週三~這週二
                        DateTime last_wed, this_tue, dateTemp;
                        if (FCommon.getQueryString(this, "date1", out string date1, false) && DateTime.TryParse(date1, out dateTemp))
                        {
                            last_wed = dateTemp;
                            this_tue = dateTemp.AddDays(7);
                        }
                        else if (FCommon.getQueryString(this, "date2", out string date2, false) && DateTime.TryParse(date2, out dateTemp))
                        {
                            last_wed = dateTemp.AddDays(-7);
                            this_tue = dateTemp;
                        }
                        else
                        {
                            DateTime dateNow = DateTime.Now;
                            int today_dayofweek = (int)dateNow.DayOfWeek; //取得今天星期幾
                            last_wed = dateNow.AddDays(-7 + (3 - today_dayofweek)); //取得上週三日期
                            this_tue = dateNow.AddDays(2 - today_dayofweek); //取得這週二日期
                        }
                        steDateText(last_wed, this_tue);
                        #endregion
                        dataImport_GV_WRP_BLD();
                    }
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "接轉地區不存在！");
            }
        }

        #region ~Click
        protected void Button_lastweek_Click(object sender, EventArgs e)
        {
            int addDay = -7;
            string strMessage = "";
            DateTime dateODT_WEEK_DATE1, dateODT_WEEK_DATE2;
            FCommon.checkDateTime(txtODT_WEEK_DATE1.Text, "資料時間－前者", ref strMessage, out dateODT_WEEK_DATE1);
            FCommon.checkDateTime(txtODT_WEEK_DATE2.Text, "資料時間－後者", ref strMessage, out dateODT_WEEK_DATE2);
            if (strMessage.Equals(string.Empty))
            {
                dateODT_WEEK_DATE1 = dateODT_WEEK_DATE1.AddDays(addDay);
                dateODT_WEEK_DATE2 = dateODT_WEEK_DATE2.AddDays(addDay);
                steDateText(dateODT_WEEK_DATE1, dateODT_WEEK_DATE2);
                dataImport_GV_WRP_BLD();
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void Button_nextweek_Click(object sender, EventArgs e)
        {
            int addDay = 7;
            string strMessage = "";
            DateTime dateODT_WEEK_DATE1, dateODT_WEEK_DATE2;
            FCommon.checkDateTime(txtODT_WEEK_DATE1.Text, "資料時間－前者", ref strMessage, out dateODT_WEEK_DATE1);
            FCommon.checkDateTime(txtODT_WEEK_DATE2.Text, "資料時間－後者", ref strMessage, out dateODT_WEEK_DATE2);
            if (strMessage.Equals(string.Empty))
            {
                dateODT_WEEK_DATE1 = dateODT_WEEK_DATE1.AddDays(addDay);
                dateODT_WEEK_DATE2 = dateODT_WEEK_DATE2.AddDays(addDay);
                steDateText(dateODT_WEEK_DATE1, dateODT_WEEK_DATE2);
                dataImport_GV_WRP_BLD();
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        #endregion

        protected void txtODT_WEEK_DATE1_TextChanged(object sender, EventArgs e)
        {
            string strMessage = "";
            DateTime dateODT_WEEK_DATE1, dateODT_WEEK_DATE2;
            bool boolODT_WEEK_DATE1 = FCommon.checkDateTime(txtODT_WEEK_DATE1.Text, "資料時間－前者", ref strMessage, out dateODT_WEEK_DATE1);
            if (boolODT_WEEK_DATE1 && dateODT_WEEK_DATE1.DayOfWeek != DayOfWeek.Wednesday)
                strMessage += "<p> 請選擇 正確日期 </p>";
            if (strMessage.Equals(string.Empty))
            {
                dateODT_WEEK_DATE2 = dateODT_WEEK_DATE1.AddDays(6);
                steDateText(dateODT_WEEK_DATE1, dateODT_WEEK_DATE2);
                dataImport_GV_WRP_BLD();
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void txtODT_WEEK_DATE2_TextChanged(object sender, EventArgs e)
        {
            string strMessage = "";
            DateTime dateODT_WEEK_DATE1, dateODT_WEEK_DATE2;
            bool boolODT_WEEK_DATE2 = FCommon.checkDateTime(txtODT_WEEK_DATE2.Text, "資料時間－後者", ref strMessage, out dateODT_WEEK_DATE2);
            if (boolODT_WEEK_DATE2 && dateODT_WEEK_DATE2.DayOfWeek != DayOfWeek.Tuesday)
                strMessage += "<p> 請選擇 正確日期 </p>";
            if (strMessage.Equals(string.Empty))
            {
                dateODT_WEEK_DATE1 = dateODT_WEEK_DATE2.AddDays(-6);
                steDateText(dateODT_WEEK_DATE1, dateODT_WEEK_DATE2);
                dataImport_GV_WRP_BLD();
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }

        protected void GV_BLD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = theGridView.DataKeys[gvrIndex].Value.ToString();
            //string key = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(edf_SN.ToString()));
            
            switch (e.CommandName)
            {
                case "btnNew":
                    string strMessage = "";
                    string strUserId = Session["userid"].ToString();
                    string strOVC_SECTION = lblOVC_SECTION.Text;
                    string strODT_WEEK_DATE = txtODT_WEEK_DATE1.Text;

                    DateTime dateODT_WEEK_DATE, dateNow = DateTime.Now;
                    if (strOVC_SECTION.Equals(string.Empty))
                        strMessage += "<p> 接轉地區不存在！ </p>";
                    if (strODT_WEEK_DATE.Equals(string.Empty))
                        strMessage += "<p> 請選擇 資料時間－前者！ </p>";
                    bool bookODT_WEEK_DATE = FCommon.checkDateTime(strODT_WEEK_DATE, "資料時間－前者", ref strMessage, out dateODT_WEEK_DATE);

                    if (strMessage.Equals(string.Empty))
                    {
                        TBGMT_WRP_BLD wrp_bld = new TBGMT_WRP_BLD();
                        wrp_bld.OVC_SECTION = strOVC_SECTION;
                        wrp_bld.ODT_WEEK_DATE = dateODT_WEEK_DATE;
                        wrp_bld.OVC_BLD_NO = id;
                        wrp_bld.ODT_CREATE_DATE = dateNow;
                        wrp_bld.OVC_CREATE_LOGIN_ID = strUserId;
                        wrp_bld.ODT_MODIFY_DATE = dateNow;
                        wrp_bld.OVC_MODIFY_LOGIN_ID = strUserId;
                        MTSE.TBGMT_WRP_BLD.Add(wrp_bld);
                        MTSE.SaveChanges();

                        FCommon.AlertShow(PnMessage, "success", "系統訊息", $"提單編號：{ id } 新增成功！");
                        dataImport_GV_WRP_BLD();
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                    break;
                default:
                    break;
            }
        }
        protected void GV_BLD_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}