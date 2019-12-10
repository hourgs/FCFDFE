using System;
using System.Data;
using System.Linq;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System.Data.Entity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using FCFDFE.Entity.GMModel;
using System.Collections;
using System.Web.UI;

namespace FCFDFE.pages.MTS.H
{
    public partial class MTS_H11 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        public string strDEPT_SN, strDEPT_Name;
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
       public int intAuth = 0; //是否為物資接轉處長官(2)or地區長官(1)or無權限(0)
        string[] aryAreaList = { "基隆地區", "桃園地區", "高雄分遣組" };
        string strDateFormatSQL = "'yyyy-mm-dd'";

        #region 副程式
        //protected void dataImport()
        //{
        //    string strMessage = "";
        //    string strUserId = Session["userid"].ToString();
        //    //string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
        //    string strOVC_NOTE = txtOVC_NOTE.Text;
        //    string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
        //    string strODT_WEEK_DATE1 = txtODT_WEEK_DATE1.Text;
        //    string strODT_WEEK_DATE2 = txtODT_WEEK_DATE2.Text;

        //    DateTime dateODT_WEEK_DATE1, dateODT_WEEK_DATE2, dateNow = DateTime.Now;
        //    //if (strOVC_BLD_NO.Equals(string.Empty))
        //    //    strMessage += "<p> 請輸入 提單編號 </p>";
        //    if (strOVC_SECTION.Equals(string.Empty))
        //        strMessage += "<p> 接轉地區不存在！ </p>";
        //    bool bookODT_WEEK_DATE1 = FCommon.checkDateTime(strODT_WEEK_DATE1, "資料時間－前者", ref strMessage, out dateODT_WEEK_DATE1);
        //    bool bookODT_WEEK_DATE2 = FCommon.checkDateTime(strODT_WEEK_DATE2, "資料時間－後者", ref strMessage, out dateODT_WEEK_DATE2);
        //    if (strMessage.Equals(string.Empty))
        //    {
        //        var query_wrp =
        //            from wrp in MTSE.TBGMT_WRP
        //            where wrp.OVC_SECTION.Equals(strOVC_SECTION)
        //            where DateTime.Compare(wrp.ODT_WEEK_DATE, dateODT_WEEK_DATE1) == 0
        //            //join bld in MTSE.TBGMT_WRP_BLD on new { wrp.OVC_SECTION, wrp.ODT_WEEK_DATE } equals new { bld.OVC_SECTION, bld.ODT_WEEK_DATE }
        //            select new
        //            {
        //                ONB_SHIP_AIR = wrp.ONB_SHIP_AIR,
        //                ONB_SHIP_SEA = wrp.ONB_SHIP_SEA,
        //                ONB_BLD_AIR = wrp.ONB_BLD_AIR,
        //                ONB_BLD_SEA = wrp.ONB_BLD_SEA,
        //                ONB_20_COUNT = wrp.ONB_20_COUNT,
        //                ONB_40_COUNT = wrp.ONB_40_COUNT,
        //                ONB_45_COUNT = wrp.ONB_45_COUNT,
        //                ONB_QUANITY = wrp.ONB_QUANITY,
        //                ONB_WEIGHT = wrp.ONB_WEIGHT,
        //                ONB_VOLUME = wrp.ONB_VOLUME,
        //                ONB_TRANS_DEFAULT = wrp.ONB_TRANS_DEFAULT,
        //                ONB_TRANS_SUPPLIER = wrp.ONB_TRANS_SUPPLIER,
        //                ONB_TRANS_TRAIN = wrp.ONB_TRANS_TRAIN
        //            };
        //        DataTable dt_wrp = CommonStatic.LinqQueryToDataTable(query_wrp);

        //        string[] strParameterName = { ":start_date", ":end_date" };
        //        ArrayList aryData = new ArrayList();
        //        aryData.Add(strODT_WEEK_DATE1);
        //        aryData.Add(strODT_WEEK_DATE2);

        //        string strSQL_unionA = $@"
        //            select
        //            a.OVC_BLD_NO,
        //            b.OVC_CHI_NAME,
        //            a.ONB_QUANITY,
        //            case when a.OVC_VOLUME_UNIT <> 'CBM' then ROUND(NVL(a.ONB_VOLUME,0) / 35.3142,3) else ROUND(NVL(a.ONB_VOLUME,0) ,3) end ONB_WEIGHT, 
        //            OVC_RECEIVE_DEPT_CODE as OVC_DEPT_NAME,
        //            to_char(ODT_IMPORT_DATE, { strDateFormatSQL }) as ODT_IMPORT_DATE,
        //            to_char(ODT_PASS_CUSTOM_DATE, { strDateFormatSQL }) as ODT_PASS_CUSTOM_DATE,
        //            to_char(ODT_UNPACKING_DATE, { strDateFormatSQL }) as ODT_STORED_DATE,
        //            to_char(ODT_TRANSFER_DATE, { strDateFormatSQL }) as ODT_TRANSFER_DATE,
        //            b.OVC_NOTE as OVC_NOTE
        //            from TBGMT_BLD a, TBGMT_ICR b
        //            where a.OVC_BLD_NO = b.OVC_BLD_NO
        //        ";

        //        string strSQL_unionB = $@"
        //            select
        //            a.OVC_BLD_NO,
        //            e.OVC_CHI_NAME,
        //            a.ONB_QUANITY,
        //            case when a.OVC_VOLUME_UNIT <> 'CBM' then ROUND(NVL(a.ONB_VOLUME,0) / 35.3142,3) else ROUND(NVL(a.ONB_VOLUME,0) ,3) end ONB_WEIGHT,
        //            '' as OVC_DEPT_NAME,
        //            '' as ODT_IMPORT_DATE,
        //            to_char(ODT_PASS_DATE, { strDateFormatSQL }) as ODT_PASS_CUSTOM_DATE,
        //            to_char(ODT_STORED_DATE, { strDateFormatSQL }) as ODT_STORED_DATE,
        //            '' as ODT_TRANSFER_DATE,
        //            '' as OVC_NOTE
        //            from TBGMT_BLD a, TBGMT_ESO c, TBGMT_ETR d, TBGMT_EDF_DETAIL e
        //            where a.OVC_BLD_NO = c.OVC_BLD_NO
        //            and a.OVC_BLD_NO = d.OVC_BLD_NO
        //            and e.OVC_EDF_NO = c.OVC_EDF_NO
        //            and e.OVC_EDF_ITEM_NO = '1'
        //        ";
        //        string strSQL_table3 = $@"
        //            { strSQL_unionA }
        //            and b.ODT_TRANSFER_DATE between to_date({ strParameterName[0] }, { strDateFormatSQL }) and to_date({ strParameterName[1] }, { strDateFormatSQL })
        //            UNION
        //            { strSQL_unionB }
        //            and c.ODT_STORED_DATE between to_date({ strParameterName[0] }, { strDateFormatSQL }) and to_date({ strParameterName[1] }, { strDateFormatSQL })
        //        ";
        //        DataTable dt_WRP_BLD = FCommon.getDataTableFromSelect(strSQL_table3, strParameterName, aryData);

        //        if (dt_wrp.Rows.Count > 0)
        //        {
        //            DataRow dr = dt_wrp.Rows[0];
        //            txtONB_SHIP_AIR.Text = dr["ONB_SHIP_AIR"].ToString();
        //            txtONB_SHIP_SEA.Text = dr["ONB_SHIP_SEA"].ToString();
        //            txtONB_BLD_AIR.Text = dr["ONB_BLD_AIR"].ToString();
        //            txtONB_BLD_SEA.Text = dr["ONB_BLD_SEA"].ToString();
        //            txtONB_20_COUNT.Text = dr["ONB_20_COUNT"].ToString();
        //            txtONB_40_COUNT.Text = dr["ONB_40_COUNT"].ToString();
        //            txtONB_45_COUNT.Text = dr["ONB_45_COUNT"].ToString();
        //            txtONB_QUANITY.Text = dr["ONB_QUANITY"].ToString();
        //            txtONB_WEIGHT.Text = dr["ONB_WEIGHT"].ToString();
        //            txtONB_VOLUME.Text = dr["ONB_VOLUME"].ToString();
        //            txtONB_TRANS_DEFAULT.Text = dr["ONB_TRANS_DEFAULT"].ToString();
        //            txtONB_TRANS_SUPPLIER.Text = dr["ONB_TRANS_SUPPLIER"].ToString();
        //            txtONB_TRANS_TRAIN.Text = dr["ONB_TRANS_TRAIN"].ToString();

        //            //gridview查詢
        //            //string temp = weekdate1.ToString("dd") + "-" + weekdate1.Month + "月" + "-" + weekdate1.ToString("yy");
        //            //string temp2 = weekdate2.ToString("dd") + "-" + weekdate2.Month + "月" + "-" + weekdate2.ToString("yy");
        //        }
        //        else
        //        {
        //            //FCommon.Controls_Clear(pnData);
        //            //steDateText(dateODT_WEEK_DATE1, dateODT_WEEK_DATE2);
        //            statistic();
        //        }

        //        string id, date1, date2;
        //        if (FCommon.getQueryString(this, "id", out id, true))
        //        {
        //            //string id = Request.QueryString["bld_no"].ToString();
        //            FCommon.getQueryString(this, "date1", out date1, true);
        //            FCommon.getQueryString(this, "date2", out date2, true);

        //            strParameterName = new string[] { ":bld_no" };
        //            aryData = new ArrayList();
        //            aryData.Add(id);

        //            string strSQL = $@"
        //                { strSQL_unionA }
        //                and a.OVC_BLD_NO = { strParameterName[0] }
        //                UNION
        //                { strSQL_unionB }
        //                and a.OVC_BLD_NO = { strParameterName[0] }
        //            ";

        //            DataTable dt = FCommon.getDataTableFromSelect(strSQL, strParameterName, aryData);
        //            dt_WRP_BLD.Merge(dt);
        //        }
        //        ViewState["hasRows"] = FCommon.GridView_dataImport(GV_WRP_BLD, dt_WRP_BLD);

        //        //btnPrint.Enabled = GV_WRP_BLD.Rows.Count > 0;
        //        if (GV_WRP_BLD.Rows.Count > 0)
        //            FCommon.Controls_Attributes("disabled", btnPrint);
        //        else
        //            FCommon.Controls_Attributes("disabled", "true", btnPrint);
        //    }
        //    else
        //        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        //}
        protected void dataImport()
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            //string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_NOTE = txtOVC_NOTE.Text;
            string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
            string strODT_WEEK_DATE1 = txtODT_WEEK_DATE1.Text;
            string strODT_WEEK_DATE2 = txtODT_WEEK_DATE2.Text;

            DateTime dateODT_WEEK_DATE1, dateODT_WEEK_DATE2, dateNow = DateTime.Now;
            //if (strOVC_BLD_NO.Equals(string.Empty))
            //    strMessage += "<p> 請輸入 提單編號 </p>";
            if (strOVC_SECTION.Equals(string.Empty))
                strMessage += "<p> 接轉地區不存在！ </p>";
            if (strODT_WEEK_DATE1.Equals(string.Empty))
                strMessage += "<p> 請選擇 資料時間－前者！ </p>";
            bool bookODT_WEEK_DATE1 = FCommon.checkDateTime(strODT_WEEK_DATE1, "資料時間－前者", ref strMessage, out dateODT_WEEK_DATE1);
            bool bookODT_WEEK_DATE2 = FCommon.checkDateTime(strODT_WEEK_DATE2, "資料時間－後者", ref strMessage, out dateODT_WEEK_DATE2);
            if (strMessage.Equals(string.Empty))
            {
                var query_wrp =
                    from wrp in MTSE.TBGMT_WRP
                    where wrp.OVC_SECTION.Equals(strOVC_SECTION)
                    where DateTime.Compare(wrp.ODT_WEEK_DATE, dateODT_WEEK_DATE1) == 0
                    //join bld in MTSE.TBGMT_WRP_BLD on new { wrp.OVC_SECTION, wrp.ODT_WEEK_DATE } equals new { bld.OVC_SECTION, bld.ODT_WEEK_DATE }
                    select new
                    {
                        ONB_SHIP_AIR = wrp.ONB_SHIP_AIR,
                        ONB_SHIP_SEA = wrp.ONB_SHIP_SEA,
                        ONB_BLD_AIR = wrp.ONB_BLD_AIR,
                        ONB_BLD_SEA = wrp.ONB_BLD_SEA,
                        ONB_20_COUNT = wrp.ONB_20_COUNT,
                        ONB_40_COUNT = wrp.ONB_40_COUNT,
                        ONB_45_COUNT = wrp.ONB_45_COUNT,
                        ONB_QUANITY = wrp.ONB_QUANITY,
                        ONB_WEIGHT = wrp.ONB_WEIGHT,
                        ONB_VOLUME = wrp.ONB_VOLUME,
                        ONB_TRANS_DEFAULT = wrp.ONB_TRANS_DEFAULT,
                        ONB_TRANS_SUPPLIER = wrp.ONB_TRANS_SUPPLIER,
                        ONB_TRANS_TRAIN = wrp.ONB_TRANS_TRAIN
                    };
                DataTable dt_wrp = CommonStatic.LinqQueryToDataTable(query_wrp);

                if (dt_wrp.Rows.Count > 0)
                {
                    DataRow dr = dt_wrp.Rows[0];
                    txtONB_SHIP_AIR.Text = dr["ONB_SHIP_AIR"].ToString();
                    txtONB_SHIP_SEA.Text = dr["ONB_SHIP_SEA"].ToString();
                    txtONB_BLD_AIR.Text = dr["ONB_BLD_AIR"].ToString();
                    txtONB_BLD_SEA.Text = dr["ONB_BLD_SEA"].ToString();
                    txtONB_20_COUNT.Text = dr["ONB_20_COUNT"].ToString();
                    txtONB_40_COUNT.Text = dr["ONB_40_COUNT"].ToString();
                    txtONB_45_COUNT.Text = dr["ONB_45_COUNT"].ToString();
                    txtONB_QUANITY.Text = dr["ONB_QUANITY"].ToString();
                    txtONB_WEIGHT.Text = dr["ONB_WEIGHT"].ToString();
                    txtONB_VOLUME.Text = dr["ONB_VOLUME"].ToString();
                    txtONB_TRANS_DEFAULT.Text = dr["ONB_TRANS_DEFAULT"].ToString();
                    txtONB_TRANS_SUPPLIER.Text = dr["ONB_TRANS_SUPPLIER"].ToString();
                    txtONB_TRANS_TRAIN.Text = dr["ONB_TRANS_TRAIN"].ToString();

                    //gridview查詢
                    //string temp = weekdate1.ToString("dd") + "-" + weekdate1.Month + "月" + "-" + weekdate1.ToString("yy");
                    //string temp2 = weekdate2.ToString("dd") + "-" + weekdate2.Month + "月" + "-" + weekdate2.ToString("yy");
                }
                else
                {
                    //FCommon.Controls_Clear(pnData);
                    //steDateText(dateODT_WEEK_DATE1, dateODT_WEEK_DATE2);
                    statistic();
                }

                dataImport_GV_WRP_BLD();
                //btnPrint.Enabled = GV_WRP_BLD.Rows.Count > 0;
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        public void dataImport_GV_WRP_BLD()
        {
            string strMessage = "";
            string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
            string strODT_WEEK_DATE1 = txtODT_WEEK_DATE1.Text;
            string strODT_WEEK_DATE2 = txtODT_WEEK_DATE2.Text;

            DateTime dateODT_WEEK_DATE1, dateODT_WEEK_DATE2;
            if (strOVC_SECTION.Equals(string.Empty))
                strMessage += "<p> 接轉地區不存在！ </p>";
            bool bookODT_WEEK_DATE1 = FCommon.checkDateTime(strODT_WEEK_DATE1, "資料時間－前者", ref strMessage, out dateODT_WEEK_DATE1);
            bool bookODT_WEEK_DATE2 = FCommon.checkDateTime(strODT_WEEK_DATE2, "資料時間－後者", ref strMessage, out dateODT_WEEK_DATE2);
            if (strMessage.Equals(string.Empty))
            {
                DataTable dt_WRP_BLD = getWRP_BLD(strOVC_SECTION, strODT_WEEK_DATE1);
                //string id, date1, date2;
                //if (FCommon.getQueryString(this, "id", out id, true))
                //{
                //    //string id = Request.QueryString["bld_no"].ToString();
                //    FCommon.getQueryString(this, "date1", out date1, true);
                //    FCommon.getQueryString(this, "date2", out date2, true);

                //    strParameterName = new string[] { ":bld_no" };
                //    aryData = new ArrayList();
                //    aryData.Add(id);

                //    string strSQL = $@"
                //        { strSQL_unionA }
                //        and a.OVC_BLD_NO = { strParameterName[0] }
                //        UNION
                //        { strSQL_unionB }
                //        and a.OVC_BLD_NO = { strParameterName[0] }
                //    ";

                //    DataTable dt = FCommon.getDataTableFromSelect(strSQL, strParameterName, aryData);
                //    dt_WRP_BLD.Merge(dt);
                //}
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_WRP_BLD, dt_WRP_BLD);

                if (GV_WRP_BLD.Rows.Count > 0)
                    FCommon.Controls_Attributes("disabled", btnPrint);
                else
                    FCommon.Controls_Attributes("disabled", "true", btnPrint);
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        private DataTable getWRP_BLD(string strOVC_SECTION, string strODT_WEEK_DATE1)
        {
            string[] strParameterName = { ":start_date", ":section" };
            ArrayList aryData = new ArrayList();
            aryData.Add(strODT_WEEK_DATE1);
            aryData.Add(strOVC_SECTION);
            #region SQL語法
            //string sql = "SELECT a.OVC_BLD_NO, b.OVC_CHI_NAME, a.ONB_QUANITY, " +
            //    "CASE WHEN a.OVC_VOLUME_UNIT <> 'CBM' THEN ROUND(NVL(a.ONB_VOLUME,0) / 35.3142,3) ELSE ROUND(NVL(a.ONB_VOLUME,0) ,3) END ONB_VOLUME, " +
            //    "DEPTCDES_TO_NAMES(OVC_RECEIVE_DEPT_CODE) as OVC_RECEIVE_DEPT, " +
            //    "TO_CHAR(ODT_IMPORT_DATE,'yyyy/mm/dd') AS ODT_IMPORT_DATE, " +
            //    "TO_CHAR(ODT_PASS_CUSTOM_DATE,'yyyy/mm/dd') AS ODT_PASS_DATE, " +
            //    "TO_CHAR(ODT_UNPACKING_DATE,'yyyy/mm/dd') AS ODT_STORED_DATE, " +
            //    "TO_CHAR(ODT_TRANSFER_DATE,'yyyy/mm/dd') AS ODT_TRANSFER_DATE, " +
            //    "'進口' AS IE, x.OVC_SECTION, x.OVC_NOTE, 'javascript:setBld(''' || a.OVC_BLD_NO || ''',''' || x.OVC_NOTE || ''')' AS SCRIPT " +
            //    "FROM TBGMT_WRP_BLD x, TBGMT_BLD a, TBGMT_ICR b WHERE OVC_SECTION = '" + Section + "' AND " +
            //    "ODT_WEEK_DATE = TO_DATE('" + WeekDate.ToShortDateString() + "','yyyy/mm/dd') AND " +
            //    "x.OVC_BLD_NO = a.OVC_BLD_NO AND " +
            //    " a.OVC_BLD_NO = b.OVC_BLD_NO (+) " +
            //    "UNION " +
            //    "SELECT a.OVC_BLD_NO, e.OVC_CHI_NAME, a.ONB_QUANITY, " +
            //    "CASE WHEN a.OVC_VOLUME_UNIT <> 'CBM' THEN ROUND(NVL(a.ONB_VOLUME,0) / 35.3142,3) ELSE ROUND(NVL(a.ONB_VOLUME,0) ,3) END ONB_VOLUME, " +
            //    "'' as OVC_RECEIVE_DEPT, " +
            //    "'' AS ODT_IMPORT_DATE, " +
            //    "TO_CHAR(ODT_PASS_DATE,'yyyy/mm/dd') AS ODT_PASS_DATE, " +
            //    "TO_CHAR(ODT_STORED_DATE,'yyyy/mm/dd') AS ODT_STORED_DATE, " +
            //    "'' AS ODT_TRANSFER_DATE, " +
            //    "'出口' AS IE, x.OVC_SECTION, x.OVC_NOTE , 'javascript:setBld(''' || a.OVC_BLD_NO || ''',''' || x.OVC_NOTE || ''')' AS SCRIPT " +
            //    "FROM TBGMT_WRP_BLD x, TBGMT_BLD a, TBGMT_ESO c, TBGMT_ETR d, TBGMT_EDF_DETAIL e WHERE OVC_SECTION = '" + Section + "' AND " +
            //    "ODT_WEEK_DATE = TO_DATE('" + WeekDate.ToShortDateString() + "','yyyy/mm/dd') AND " +
            //    "x.OVC_BLD_NO = a.OVC_BLD_NO AND " +
            //    "a.OVC_BLD_NO = c.OVC_BLD_NO (+) AND  a.OVC_BLD_NO = d.OVC_BLD_NO (+) AND e.OVC_EDF_NO = c.OVC_EDF_NO AND e.OVC_EDF_ITEM_NO = '1' ";

            string strSQL_unionA = $@"
                    select
                    a.OVC_BLD_NO,
                    b.OVC_CHI_NAME,
                    a.ONB_QUANITY,
                    case when a.OVC_VOLUME_UNIT <> 'CBM' then ROUND(NVL(a.ONB_VOLUME,0) / 35.3142,3) else ROUND(NVL(a.ONB_VOLUME,0) ,3) end ONB_VOLUME, 
                    dept.OVC_ONNAME as OVC_RECEIVE_DEPT,
                    to_char(ODT_IMPORT_DATE, { strDateFormatSQL }) as ODT_IMPORT_DATE,
                    to_char(ODT_PASS_CUSTOM_DATE, { strDateFormatSQL }) as ODT_PASS_DATE,
                    to_char(ODT_UNPACKING_DATE, { strDateFormatSQL }) as ODT_STORED_DATE,
                    to_char(ODT_TRANSFER_DATE, { strDateFormatSQL }) as ODT_TRANSFER_DATE,
                    '進口' AS IE, x.OVC_SECTION, x.OVC_NOTE
                    from TBGMT_WRP_BLD x, TBGMT_BLD a, TBGMT_ICR b, TBMDEPT dept
                    where x.OVC_BLD_NO = a.OVC_BLD_NO
                    and a.OVC_BLD_NO = b.OVC_BLD_NO (+)
                    and b.OVC_RECEIVE_DEPT_CODE = dept.OVC_DEPT_CDE (+)
                    and OVC_SECTION={ strParameterName[1] }
                    and ODT_WEEK_DATE = to_date({ strParameterName[0] }, { strDateFormatSQL })
                ";
            string strSQL_unionB = $@"
                    select
                    a.OVC_BLD_NO,
                    e.OVC_CHI_NAME,
                    a.ONB_QUANITY,
                    case when a.OVC_VOLUME_UNIT <> 'CBM' then ROUND(NVL(a.ONB_VOLUME,0) / 35.3142,3) else ROUND(NVL(a.ONB_VOLUME,0) ,3) end ONB_VOLUME,
                    '' as OVC_RECEIVE_DEPT,
                    '' as ODT_IMPORT_DATE,
                    to_char(ODT_PASS_DATE, { strDateFormatSQL }) as ODT_PASS_DATE,
                    to_char(ODT_STORED_DATE, { strDateFormatSQL }) as ODT_STORED_DATE,
                    '' as ODT_TRANSFER_DATE,
                    '出口' AS IE, x.OVC_SECTION, x.OVC_NOTE
                    from TBGMT_WRP_BLD x, TBGMT_BLD a, TBGMT_ESO c, TBGMT_ETR d, TBGMT_EDF_DETAIL e
                    where x.OVC_BLD_NO = a.OVC_BLD_NO
                    and a.OVC_BLD_NO = c.OVC_BLD_NO (+)
                    and a.OVC_BLD_NO = d.OVC_BLD_NO (+)
                    and e.OVC_EDF_NO = c.OVC_EDF_NO
                    and e.OVC_EDF_ITEM_NO = '1'
                    and OVC_SECTION={ strParameterName[1] }
                    and ODT_WEEK_DATE = to_date({ strParameterName[0] }, { strDateFormatSQL })
                ";
            string strSQL_WRP_BLD = $@"
                    { strSQL_unionA }
                    UNION
                    { strSQL_unionB }
                ";
            #endregion
            DataTable dt_WRP_BLD = FCommon.getDataTableFromSelect(strSQL_WRP_BLD, strParameterName, aryData);
            return dt_WRP_BLD;
        }
        private void statistic() //統計並儲存
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
            string strODT_WEEK_DATE1 = txtODT_WEEK_DATE1.Text;
            string strODT_WEEK_DATE2 = txtODT_WEEK_DATE2.Text;

            DateTime dateODT_WEEK_DATE1, dateODT_WEEK_DATE2, dateNow = DateTime.Now;
            //if (strOVC_BLD_NO.Equals(string.Empty))
            //    strMessage += "<p> 請輸入 提單編號 </p>";
            if (strOVC_SECTION.Equals(string.Empty))
                strMessage += "<p> 接轉地區不存在！ </p>";
            bool bookODT_WEEK_DATE1 = FCommon.checkDateTime(strODT_WEEK_DATE1, "資料時間－前者", ref strMessage, out dateODT_WEEK_DATE1);
            bool bookODT_WEEK_DATE2 = FCommon.checkDateTime(strODT_WEEK_DATE2, "資料時間－後者", ref strMessage, out dateODT_WEEK_DATE2);

            if (strMessage.Equals(string.Empty))
            {
                string[] strParameterName = { ":start_date", ":end_date", ":section" };
                ArrayList aryData = new ArrayList();
                aryData.Add(strODT_WEEK_DATE1);
                aryData.Add(strODT_WEEK_DATE2);
                aryData.Add(strOVC_SECTION);

                //取得接轉地區之port
                //var queryPort =
                //    from port in GME.TBM1407
                //    where port.OVC_PHR_CATE.Equals("TR")
                //    where port.OVC_PHR_PARENTS.Equals(strOVC_SECTION)
                //    select port;
                string strSQL_PortList = $@"
                    select OVC_PHR_ID from TBM1407 where OVC_PHR_CATE='TR' and OVC_PHR_PARENTS={ strParameterName[2] }
                ";
                string strSQL_Port = $@"
                    and ( OVC_ARRIVE_PORT in ({ strSQL_PortList }) or OVC_START_PORT in ({ strSQL_PortList }))
                ";

                #region 取得航架次、報關數
                decimal decONB_SHIP_AIR = 0, decONB_SHIP_SEA = 0, decONB_BLD_AIR = 0, decONB_BLD_SEA = 0;

                string[] strParameterName_SeaOrAir = { ":start_date", ":end_date", ":sea_or_air", strParameterName[2] };
                ArrayList aryData_SeaOrAir = new ArrayList();
                aryData_SeaOrAir.Add(strODT_WEEK_DATE1);
                aryData_SeaOrAir.Add(strODT_WEEK_DATE2);
                aryData_SeaOrAir.Add("");
                aryData_SeaOrAir.Add(strOVC_SECTION);

                //航架次
                string strSQL_ONB_SHIP = $@"
                        select COUNT(*) from(
                            select a.OVC_SHIP_COMPANY, a.OVC_VOYAGE, a.ODT_START_DATE 
                            from TBGMT_BLD a, TBGMT_ICR b, TBGMT_ESO c
                            where a.OVC_BLD_NO = b.OVC_BLD_NO (+)
                            and a.OVC_BLD_NO = c.OVC_BLD_NO (+)
                            and a.OVC_SEA_OR_AIR = { strParameterName_SeaOrAir[2] }
                            { strSQL_Port }
                            and( b.ODT_TRANSFER_DATE between to_date({ strParameterName_SeaOrAir[0] }, { strDateFormatSQL }) and to_date({ strParameterName_SeaOrAir[1] }, { strDateFormatSQL })
                                or c.ODT_STORED_DATE between to_date({ strParameterName_SeaOrAir[0] }, { strDateFormatSQL }) and to_date({ strParameterName_SeaOrAir[1] }, { strDateFormatSQL })
                            )
                            GROUP BY a.OVC_SHIP_COMPANY, a.OVC_VOYAGE, a.ODT_START_DATE
                        )";
                //航架次
                //string sql1 = "SELECT COUNT(*) FROM (SELECT a.OVC_SHIP_COMPANY, a.OVC_VOYAGE, a.ODT_START_DATE " +
                //    "FROM TBGMT_BLD a, TBGMT_ICR b, TBGMT_ESO c WHERE a.OVC_BLD_NO = b.OVC_BLD_NO (+) AND a.OVC_BLD_NO = c.OVC_BLD_NO (+) AND " +
                //    "(b.ODT_TRANSFER_DATE BETWEEN " + wedDate + " AND " + tueDate + " OR c.ODT_STORED_DATE BETWEEN " + wedDate + " AND " + tueDate + ") AND " +
                //    " a.OVC_SEA_OR_AIR = :sea_or_air AND " + portCond + "GROUP BY a.OVC_SHIP_COMPANY, a.OVC_VOYAGE, a.ODT_START_DATE) ";

                //報關數
                string strSQL_ONB_BLD = $@"
                        select COUNT(*) from (
                            select a.OVC_BLD_NO
                            from TBGMT_BLD a, TBGMT_ICR b, TBGMT_ESO c
                            where a.OVC_BLD_NO = b.OVC_BLD_NO (+)
                            and a.OVC_BLD_NO = c.OVC_BLD_NO (+)
                            and a.OVC_SEA_OR_AIR ={ strParameterName_SeaOrAir[2] }
                            { strSQL_Port }
                            and( b.ODT_TRANSFER_DATE between to_date({ strParameterName_SeaOrAir[0] }, { strDateFormatSQL }) and to_date({ strParameterName_SeaOrAir[1] }, { strDateFormatSQL })
                                or c.ODT_STORED_DATE between to_date({ strParameterName_SeaOrAir[0] }, { strDateFormatSQL }) and to_date({ strParameterName_SeaOrAir[1] }, { strDateFormatSQL })
                            )
                        )";
                ////報關數
                //string sql2 = "SELECT COUNT(*) FROM (SELECT a.OVC_BLD_NO " +
                //    "FROM TBGMT_BLD a, TBGMT_ICR b, TBGMT_ESO c WHERE a.OVC_BLD_NO = b.OVC_BLD_NO (+) AND a.OVC_BLD_NO = c.OVC_BLD_NO (+) AND " +
                //    "(b.ODT_TRANSFER_DATE BETWEEN " + wedDate + " AND " + tueDate + " OR c.ODT_STORED_DATE BETWEEN " + wedDate + " AND " + tueDate + ") AND " +
                //    " a.OVC_SEA_OR_AIR = :sea_or_air AND " + portCond + " ) ";

                aryData_SeaOrAir[2] = "海運";
                //FCommon.AlertShow(PnMessage, "danger", "系統訊息", strSQL_ONB_SHIP);
                //throw new ArgumentException();

                DataTable dt_ONB_SHIP_SEA = FCommon.getDataTableFromSelect(strSQL_ONB_SHIP, strParameterName_SeaOrAir, aryData_SeaOrAir); //海運航次
                if (dt_ONB_SHIP_SEA.Rows.Count > 0) decimal.TryParse(dt_ONB_SHIP_SEA.Rows[0][0].ToString(), out decONB_SHIP_SEA);
                DataTable dt_ONB_BLD_SEA = FCommon.getDataTableFromSelect(strSQL_ONB_BLD, strParameterName_SeaOrAir, aryData_SeaOrAir); //海運報關數
                if (dt_ONB_BLD_SEA.Rows.Count > 0) decimal.TryParse(dt_ONB_BLD_SEA.Rows[0][0].ToString(), out decONB_BLD_SEA);

                aryData_SeaOrAir[2] = "空運";
                DataTable dt_ONB_SHIP_AIR = FCommon.getDataTableFromSelect(strSQL_ONB_SHIP, strParameterName_SeaOrAir, aryData_SeaOrAir); //空運架次
                if (dt_ONB_SHIP_AIR.Rows.Count > 0) decimal.TryParse(dt_ONB_SHIP_AIR.Rows[0][0].ToString(), out decONB_SHIP_AIR);
                DataTable dt_ONB_BLD_AIR = FCommon.getDataTableFromSelect(strSQL_ONB_BLD, strParameterName_SeaOrAir, aryData_SeaOrAir); //空運報關數
                if (dt_ONB_BLD_AIR.Rows.Count > 0) decimal.TryParse(dt_ONB_BLD_AIR.Rows[0][0].ToString(), out decONB_BLD_AIR);
                #endregion

                #region 取得 件數、重量、體積
                decimal decONB_QUANITY = 0, decONB_WEIGHT = 0, decONB_VOLUME = 0;
                //件數 重量KG 體積CBM
                string strSQL_QWV = $@"
                        select NVL(SUM(ONB_QUANITY),0) as ONB_QUANITY, NVL(SUM(ONB_WEIGHT),0) as ONB_WEIGHT, NVL(SUM(ONB_VOLUME),0) as ONB_VOLUME
                        from (
                            select
                            ONB_QUANITY,
                            case when OVC_WEIGHT_UNIT <> 'KG' then ROUND(NVL(ONB_WEIGHT, 0) * 0.45359,3) else ROUND(NVL(ONB_WEIGHT, 0), 3) end ONB_WEIGHT,
                            case when OVC_VOLUME_UNIT <> 'CBM' then ROUND(NVL(ONB_VOLUME, 0) / 35.3142,3) else ROUND(NVL(ONB_VOLUME, 0), 3) end ONB_VOLUME
                            from TBGMT_BLD a, TBGMT_ICR b
                            where a.OVC_BLD_NO = b.OVC_BLD_NO (+)
                            { strSQL_Port }
                            and b.ODT_TRANSFER_DATE between to_date({ strParameterName[0] }, { strDateFormatSQL }) and to_date({ strParameterName[1] }, { strDateFormatSQL })
                            UNION 
                            select d.ONB_QUANITY,  
                                case when OVC_WEIGHT_UNIT <> 'KG' then ROUND(NVL(ONB_WEIGHT, 0) * 0.45359,3) else ROUND(NVL(ONB_WEIGHT, 0), 3) end ONB_WEIGHT, 
                                case when OVC_VOLUME_UNIT <> 'CBM' then ROUND(NVL(ONB_VOLUME, 0) / 35.3142,3) else ROUND(NVL(ONB_VOLUME, 0), 3) end ONB_VOLUME 
                            from TBGMT_BLD a, TBGMT_ICR b, TBGMT_ESO c ,
                            (select OVC_EDF_NO, COUNT(OVC_BOX_NO) as ONB_QUANITY 
                                from(select DISTINCT OVC_EDF_NO, OVC_BOX_NO from TBGMT_EDF_DETAIL) 
                                GROUP BY OVC_EDF_NO) d
                            where a.OVC_BLD_NO = c.OVC_BLD_NO (+)
                            and c.OVC_EDF_NO = d.OVC_EDF_NO
                            { strSQL_Port }
                            and c.ODT_STORED_DATE between to_date({ strParameterName[0] }, { strDateFormatSQL }) and to_date({ strParameterName[1] }, { strDateFormatSQL })
                        )
                    ";
                //件數 重量KG 體積CBM
                //string sql3 = "SELECT NVL(SUM(ONB_QUANITY),0),NVL(SUM(ONB_WEIGHT),0),NVL(SUM(ONB_VOLUME),0) FROM (SELECT ONB_QUANITY, " +
                //    "CASE WHEN OVC_WEIGHT_UNIT <> 'KG' THEN ROUND(NVL(ONB_WEIGHT,0) * 0.45359,3) ELSE ROUND(NVL(ONB_WEIGHT,0) ,3) END ONB_WEIGHT, " +
                //    "CASE WHEN OVC_VOLUME_UNIT <> 'CBM' THEN ROUND(NVL(ONB_VOLUME,0) / 35.3142,3) ELSE ROUND(NVL(ONB_VOLUME,0) ,3) END ONB_VOLUME " +
                //    "FROM TBGMT_BLD a, TBGMT_ICR b WHERE a.OVC_BLD_NO = b.OVC_BLD_NO (+) AND " +
                //    "(b.ODT_TRANSFER_DATE BETWEEN " + wedDate + " AND " + tueDate + ") AND " +
                //    portCond +
                //    "UNION " +
                //    "SELECT d.ONB_QUANITY, " +
                //    "CASE WHEN OVC_WEIGHT_UNIT <> 'KG' THEN ROUND(NVL(ONB_WEIGHT,0) * 0.45359,3) ELSE ROUND(NVL(ONB_WEIGHT,0) ,3) END ONB_WEIGHT, " +
                //    "CASE WHEN OVC_VOLUME_UNIT <> 'CBM' THEN ROUND(NVL(ONB_VOLUME,0) / 35.3142,3) ELSE ROUND(NVL(ONB_VOLUME,0) ,3) END ONB_VOLUME " +
                //    "FROM TBGMT_BLD a, TBGMT_ICR b,TBGMT_ESO c , " +
                //    "(SELECT OVC_EDF_NO, COUNT(OVC_BOX_NO) AS ONB_QUANITY FROM (SELECT DISTINCT OVC_EDF_NO, OVC_BOX_NO FROM TBGMT_EDF_DETAIL) GROUP BY OVC_EDF_NO) d " +
                //    "WHERE a.OVC_BLD_NO = c.OVC_BLD_NO (+) AND c.OVC_EDF_NO = d.OVC_EDF_NO AND " +
                //    "(c.ODT_STORED_DATE BETWEEN " + wedDate + " AND " + tueDate + ") AND " +
                //    portCond +
                //    " ) ";
                DataTable dt_QWV = FCommon.getDataTableFromSelect(strSQL_QWV, strParameterName, aryData);
                foreach (DataRow dr_QWV in dt_QWV.Rows)
                {
                    decimal decTemp;
                    if (decimal.TryParse(dr_QWV["ONB_QUANITY"].ToString(), out decTemp)) decONB_QUANITY += decTemp;
                    if (decimal.TryParse(dr_QWV["ONB_WEIGHT"].ToString(), out decTemp)) decONB_WEIGHT += decTemp;
                    if (decimal.TryParse(dr_QWV["ONB_VOLUME"].ToString(), out decTemp)) decONB_VOLUME += decTemp;
                }
                #endregion

                #region 取得貨櫃
                decimal decONB_20_COUNT = 0, decONB_40_COUNT = 0, decONB_45_COUNT = 0;
                //出口貨櫃
                string strSQL_ONB_COUNT = $@"
                        select NVL(SUM(ONB_20_COUNT),0) as ONB_20_COUNT, NVL(SUM(ONB_40_COUNT),0) as ONB_40_COUNT, NVL(SUM(ONB_45_COUNT),0) as ONB_45_COUNT
                        from TBGMT_ESO 
                        where OVC_BLD_NO IN( 
                            select a.OVC_BLD_NO
                            from TBGMT_BLD a, TBGMT_ICR b, TBGMT_ESO c 
                            where a.OVC_BLD_NO = b.OVC_BLD_NO (+)
                            and a.OVC_BLD_NO = c.OVC_BLD_NO (+)
                            { strSQL_Port }
                            and( b.ODT_TRANSFER_DATE between to_date({ strParameterName[0] }, { strDateFormatSQL }) and to_date({ strParameterName[1] }, { strDateFormatSQL })
                                or c.ODT_STORED_DATE between to_date({ strParameterName[0] }, { strDateFormatSQL }) and to_date({ strParameterName[1] }, { strDateFormatSQL })
                            )
                        )
                    ";
                //出口貨櫃
                //string sql4 = "SELECT NVL(SUM(ONB_20_COUNT),0), NVL(SUM(ONB_40_COUNT),0), NVL(SUM(ONB_45_COUNT),0) FROM TBGMT_ESO " +
                //    "WHERE OVC_BLD_NO IN ( SELECT a.OVC_BLD_NO FROM TBGMT_BLD a, TBGMT_ICR b, TBGMT_ESO c " +
                //    "WHERE a.OVC_BLD_NO = b.OVC_BLD_NO (+) AND a.OVC_BLD_NO = c.OVC_BLD_NO (+) AND " +
                //    "(b.ODT_TRANSFER_DATE BETWEEN " + wedDate + " AND " + tueDate + " OR c.ODT_STORED_DATE BETWEEN " + wedDate + " AND " + tueDate + ") AND " +
                //    portCond + " ) ";

                DataTable dt_ONB_COUNT = FCommon.getDataTableFromSelect(strSQL_ONB_COUNT, strParameterName, aryData);
                foreach (DataRow dr_ONB_COUNT in dt_ONB_COUNT.Rows)
                {
                    decimal decTemp;
                    if (decimal.TryParse(dr_ONB_COUNT["ONB_20_COUNT"].ToString(), out decTemp)) decONB_20_COUNT += decTemp;
                    if (decimal.TryParse(dr_ONB_COUNT["ONB_40_COUNT"].ToString(), out decTemp)) decONB_40_COUNT += decTemp;
                    if (decimal.TryParse(dr_ONB_COUNT["ONB_45_COUNT"].ToString(), out decTemp)) decONB_45_COUNT += decTemp;
                }

                //進口貨櫃
                string strSQL_ONB_COUNT2 = $@"
                        select ONB_SIZE, COUNT(OVC_CONTAINER_NO) COUNT
                        from(
                            select ONB_SIZE, OVC_CONTAINER_NO
                            from TBGMT_IRD_CTN
                            where OVC_BLD_NO IN(
                                select a.OVC_BLD_NO
                                from TBGMT_BLD a, TBGMT_ICR b, TBGMT_ESO c
                                where a.OVC_BLD_NO = b.OVC_BLD_NO (+)
                                and a.OVC_BLD_NO = c.OVC_BLD_NO (+)
                                { strSQL_Port }
                                and( b.ODT_TRANSFER_DATE between to_date({ strParameterName[0] }, { strDateFormatSQL }) and to_date({ strParameterName[1] }, { strDateFormatSQL })
                                    or c.ODT_STORED_DATE between to_date({ strParameterName[0] }, { strDateFormatSQL }) and to_date({ strParameterName[1] }, { strDateFormatSQL })
                                )
                            )
                            GROUP BY OVC_CONTAINER_NO, ONB_SIZE
                        )
                        GROUP BY ONB_SIZE";
                //進口貨櫃
                //string sql5 = "SELECT ONB_SIZE, COUNT(OVC_CONTAINER_NO) FROM (SELECT ONB_SIZE,OVC_CONTAINER_NO FROM TBGMT_IRD_CTN " +
                //    "WHERE OVC_BLD_NO IN ( SELECT a.OVC_BLD_NO FROM TBGMT_BLD a, TBGMT_ICR b, TBGMT_ESO c " +
                //    "WHERE a.OVC_BLD_NO = b.OVC_BLD_NO (+) AND a.OVC_BLD_NO = c.OVC_BLD_NO (+) AND " +
                //    "(b.ODT_TRANSFER_DATE BETWEEN " + wedDate + " AND " + tueDate + " OR c.ODT_STORED_DATE BETWEEN " + wedDate + " AND " + tueDate + ") AND " +
                //    portCond + " ) GROUP BY OVC_CONTAINER_NO, ONB_SIZE ) GROUP BY ONB_SIZE ";

                DataTable dt_ONB_COUNT2 = FCommon.getDataTableFromSelect(strSQL_ONB_COUNT2, strParameterName, aryData);
                foreach (DataRow dr_ONB_COUNT in dt_ONB_COUNT2.Rows)
                {
                    string strONB_SIZE = dr_ONB_COUNT["ONB_SIZE"].ToString();
                    if (decimal.TryParse(dr_ONB_COUNT["COUNT"].ToString(), out decimal decTemp))
                    {
                        switch (strONB_SIZE)
                        {
                            case "20":
                                decONB_20_COUNT += decTemp;
                                break;
                            case "40":
                                decONB_40_COUNT += decTemp;
                                break;
                            case "45":
                                decONB_45_COUNT += decTemp;
                                break;
                        }
                    }
                }
                #endregion

                decimal decONB_TRANS_DEFAULT = 0, decONB_TRANS_SUPPLIER = 0, decONB_TRANS_TRAIN = 0;
                try
                {
                    //TBGMT_WRP_BLD wrp_bld = new TBGMT_WRP_BLD();
                    TBGMT_WRP wrp = MTSE.TBGMT_WRP.Where(table => table.OVC_SECTION.Equals(strOVC_SECTION) && DateTime.Compare(table.ODT_WEEK_DATE, dateODT_WEEK_DATE1) == 0).FirstOrDefault();
                    bool isCreate = wrp == null;
                    if (isCreate) wrp = new TBGMT_WRP();
                    wrp.OVC_SECTION = strOVC_SECTION;
                    wrp.ODT_WEEK_DATE = dateODT_WEEK_DATE1;
                    //wrp.ONB_QUANITY = Convert.ToDecimal(dt_WRP_BLD.Rows[0][2]);
                    wrp.ONB_QUANITY = decONB_QUANITY;
                    wrp.ONB_SHIP_AIR = decONB_SHIP_AIR;
                    wrp.ONB_SHIP_SEA = decONB_SHIP_SEA;
                    wrp.ONB_BLD_AIR = decONB_BLD_AIR;
                    wrp.ONB_BLD_SEA = decONB_BLD_SEA;
                    wrp.ONB_WEIGHT = decONB_WEIGHT;
                    wrp.ONB_VOLUME = decONB_VOLUME;
                    wrp.ONB_20_COUNT = decONB_20_COUNT;
                    wrp.ONB_40_COUNT = decONB_40_COUNT;
                    wrp.ONB_45_COUNT = decONB_45_COUNT;
                    wrp.ONB_TRANS_DEFAULT = decONB_TRANS_DEFAULT;
                    wrp.ONB_TRANS_SUPPLIER = decONB_TRANS_SUPPLIER;
                    wrp.ONB_TRANS_TRAIN = decONB_TRANS_TRAIN;
                    if (isCreate) wrp.OVC_LOGIN_ID = strUserId;
                    if (isCreate) wrp.ODT_CREATE_DATE = dateNow;
                    wrp.OVC_MODIFY_LOGIN_ID = strUserId;
                    wrp.ODT_MODIFY_DATE = dateNow;
                    if (isCreate) MTSE.TBGMT_WRP.Add(wrp);

                    //wrp_bld.OVC_SECTION = strOVC_SECTION;
                    //wrp_bld.OVC_BLD_NO = dt_WRP_BLD.Rows[0][0].ToString();
                    //wrp_bld.ODT_WEEK_DATE = dateODT_WEEK_DATE1;

                    //MTSE.TBGMT_WRP_BLD.Add(wrp_bld);
                    MTSE.SaveChanges();


                    txtONB_SHIP_AIR.Text = decONB_SHIP_AIR.ToString();
                    txtONB_SHIP_SEA.Text = decONB_SHIP_SEA.ToString();
                    txtONB_BLD_AIR.Text = decONB_BLD_AIR.ToString();
                    txtONB_BLD_SEA.Text = decONB_BLD_SEA.ToString();
                    txtONB_20_COUNT.Text = decONB_20_COUNT.ToString();
                    txtONB_40_COUNT.Text = decONB_40_COUNT.ToString();
                    txtONB_45_COUNT.Text = decONB_45_COUNT.ToString();
                    txtONB_QUANITY.Text = decONB_QUANITY.ToString();
                    txtONB_WEIGHT.Text = decONB_WEIGHT.ToString();
                    txtONB_VOLUME.Text = decONB_VOLUME.ToString();
                    txtONB_TRANS_DEFAULT.Text = decONB_TRANS_DEFAULT.ToString();
                    txtONB_TRANS_SUPPLIER.Text = decONB_TRANS_SUPPLIER.ToString();
                    txtONB_TRANS_TRAIN.Text = decONB_TRANS_TRAIN.ToString();
                }
                catch
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "新增 接轉作業週報表 失敗，請聯絡工程師！");
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }

        public static int NumOfWeek(DateTime dt)
        {
            int nDay = dt.Day;
            //int nDayOfWeek = dt.DayOfWeek - DayOfWeek.Sunday;
            int nDayOfWeek = dt.DayOfWeek - DayOfWeek.Wednesday;
            int nRemainder = (nDayOfWeek - nDay+1) % 7;
            if (nRemainder < 0) { nRemainder = 7 + nRemainder; }
            return (6 + nDay + nRemainder) / 7;
        }
        private void steDateText(DateTime date1, DateTime date2)
        {
            txtODT_WEEK_DATE1.Text =FCommon.getDateTime( date1);
            txtODT_WEEK_DATE2.Text = FCommon.getDateTime(date2);
            lblODT_MONTH.Text = date1.Month.ToString();
            //lblODT_WEEK.Text = NumOfWeek(DateTime.Parse("2018-03-07")).ToString();
            lblODT_WEEK.Text = NumOfWeek(date1).ToString();
        }
        private string getQueryString()
        {
            string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
            string strODT_WEEK_DATE1 = txtODT_WEEK_DATE1.Text;
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strOVC_NOTE = txtOVC_NOTE.Text;
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "section", strOVC_SECTION, true);
            FCommon.setQueryString(ref strQueryString, "date1", strODT_WEEK_DATE1, true);
            FCommon.setQueryString(ref strQueryString, "id", strOVC_BLD_NO, true);
            FCommon.setQueryString(ref strQueryString, "note", strOVC_NOTE, true);
            return strQueryString;
        }
        private void onWeekClick(int addDay)
        {
            string strMessage = "";
            DateTime dateODT_WEEK_DATE1, dateODT_WEEK_DATE2;
            FCommon.checkDateTime(txtODT_WEEK_DATE1.Text, "資料時間－前者", ref strMessage, out dateODT_WEEK_DATE1);
            FCommon.checkDateTime(txtODT_WEEK_DATE2.Text, "資料時間－後者", ref strMessage, out dateODT_WEEK_DATE2);
            if (strMessage.Equals(string.Empty))
            {
                //txtODT_WEEK_DATE1.Text = dateODT_WEEK_DATE1.AddDays(addDay).ToString(strDateFormat);
                //txtODT_WEEK_DATE2.Text = dateODT_WEEK_DATE2.AddDays(addDay).ToString(strDateFormat);
                dateODT_WEEK_DATE1 = dateODT_WEEK_DATE1.AddDays(addDay);
                dateODT_WEEK_DATE2 = dateODT_WEEK_DATE2.AddDays(addDay);
                steDateText(dateODT_WEEK_DATE1, dateODT_WEEK_DATE2);
                dataImport();
                //string strSQL = Path.GetFileName(Request.Path);
                //Response.Redirect(strSQL + getQueryString());
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
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
                    FCommon.Controls_Attributes("readonly", "true", txtODT_WEEK_DATE1, txtODT_WEEK_DATE2);
                    lblDEPT.Text = strDEPT_Name;

                    //if (Session["userid"] != null)
                    //{
                    #region 取得權限、設定接轉地區
                    bool isSelectSECTION = false;
                    string strOVC_SECTION = "";
                    if (strDEPT_SN.Equals("00N00"))
                    {
                        intAuth = 2;
                        if (!FCommon.getQueryString(this, "section", out strOVC_SECTION, true))
                            strOVC_SECTION = "基隆地區";
                        FCommon.list_setValue(drpOVC_SECTION, strOVC_SECTION);
                        isSelectSECTION = true;
                    }
                    else
                        foreach (string strArea in aryAreaList)
                        {
                            if (strDEPT_Name.Contains(strArea))
                            {
                                intAuth = 1;
                                strOVC_SECTION = strArea;
                                break;
                            }
                        }
                    //switch (strDEPT_SN)
                    //{
                    //    case "00N00":
                    //        intAuth = 2;
                    //        if (!FCommon.getQueryString(this, "section", out strOVC_SECTION, true))
                    //            strOVC_SECTION = "基隆地區";
                    //        FCommon.list_setValue(drpOVC_SECTION, strOVC_SECTION);
                    //        isSelectSECTION = true;
                    //        break;
                    //    case "00N10":
                    //        intAuth = 1;
                    //        strOVC_SECTION = "基隆地區";
                    //        break;
                    //    case "00N20":
                    //        intAuth = 1;
                    //        strOVC_SECTION = "桃園地區";
                    //        break;
                    //    case "00N30":
                    //        intAuth = 1;
                    //        strOVC_SECTION = "高雄分遣組";
                    //        break;
                    //    default:
                    //        intAuth = 0;
                    //        break;
                    //}
                    ViewState["auth"] = intAuth; //儲存權限
                    lblOVC_SECTION.Text = strOVC_SECTION;
                    lblOVC_SECTION.Visible = !isSelectSECTION;
                    drpOVC_SECTION.Visible = isSelectSECTION;

                    if (intAuth != 0)
                    {
                        #region 設定預設日期
                        //網址 或 上週三~這週二
                        DateTime last_wed, this_tue, dateTemp;
                        if (FCommon.getQueryString(this, "date1", out string date1, true) && DateTime.TryParse(date1, out dateTemp))
                        {
                            last_wed = dateTemp;
                            this_tue = dateTemp.AddDays(7);
                        }
                        else if (FCommon.getQueryString(this, "date2", out string date2, true) && DateTime.TryParse(date2, out dateTemp))
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
                        if (FCommon.getQueryString(this, "id", out string strOVC_BLD_NO, true))
                            txtOVC_BLD_NO.Text = strOVC_BLD_NO;
                        if (FCommon.getQueryString(this, "note", out string strOVC_NOTE, true))
                            txtOVC_NOTE.Text = strOVC_NOTE;
                        dataImport();
                    }
                    #endregion
                    #region 舊方法 intAuth取得權限
                    //string strUSER_ID = Session["userid"].ToString();
                    //DateTime dateStart = DateTime.Today;
                    ////搜尋該帳號所有在MTS系統中之權限角色
                    ////限制條件：無日期限制或日期範圍內；啟用狀態為Y
                    //var queryAuth =
                    //    from accountAuth in GME.ACCOUNT_AUTH
                    //    where accountAuth.USER_ID.Equals(strUSER_ID)
                    //    where accountAuth.C_SN_SYS.Equals("MTS")
                    //    where accountAuth.END_DATE == null || accountAuth.END_DATE >= dateStart
                    //    where accountAuth.IS_ENABLE.Equals("Y")
                    //    join tAuth in GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("S6")) on accountAuth.C_SN_AUTH equals tAuth.OVC_PHR_ID
                    //    select new
                    //    {
                    //        accountAuth.C_SN_AUTH,
                    //        tAuth.OVC_PHR_DESC
                    //    };
                    //DataTable dt = CommonStatic.LinqQueryToDataTable(queryAuth);
                    //int intCount = dt.Rows.Count;
                    //for (int i = 0; i < intCount; i++)
                    //{
                    //    DataRow dr = dt.Rows[i];
                    //    string strOVC_PHR_DESC = dr["OVC_PHR_DESC"].ToString();
                    //    if (strOVC_PHR_DESC.Contains("物資接轉處")) //最大權限使用者
                    //    {
                    //        intAuth = 2;
                    //        break; //已經為最大權限使用者，不需再判斷其他權限
                    //    }
                    //    else if (strOVC_PHR_DESC.Contains("地區接轉") && intAuth < 1) //若權限小於此權限，才需定義為此權限
                    //        intAuth = 1;
                    //}
                    //if (intAuth == 1 && strDEPT_SN.Equals("00N00")) //地區長官 且 單位是00N00 等同物資接轉處長官
                    //    intAuth = 2;
                    //ViewState["auth"] = intAuth; //儲存權限
                    //if (intAuth != 0)
                    //{
                    //    #region 設定接轉地區
                    //    string strOVC_SECTION = "";
                    //    switch (intAuth)
                    //    {
                    //        case 1:
                    //            if (strDEPT_SN.Equals("00N10"))
                    //                strOVC_SECTION = "基隆地區";
                    //            else if (strDEPT_SN.Equals("00N20"))
                    //                strOVC_SECTION = "桃園地區";
                    //            else if (strDEPT_SN.Equals("00N30"))
                    //                strOVC_SECTION = "高雄分遣組";
                    //            lblOVC_SECTION.Text = strOVC_SECTION;
                    //            lblOVC_SECTION.Visible = true;
                    //            drpOVC_SECTION.Visible = false;
                    //            break;
                    //        case 2:
                    //            if (FCommon.getQueryString(this, "section", out strOVC_SECTION, true))
                    //                FCommon.list_setValue(drpOVC_SECTION, strOVC_SECTION);
                    //            lblOVC_SECTION.Visible = false;
                    //            drpOVC_SECTION.Visible = true;
                    //            break;
                    //    }
                    //    #endregion
                    //    #region 設定預設日期
                    //    //網址 或 上週三~這週二
                    //    DateTime last_wed, this_tue, dateTemp;
                    //    if (FCommon.getQueryString(this, "date1", out string date1, true) && DateTime.TryParse(date1, out dateTemp))
                    //    {
                    //        last_wed = dateTemp;
                    //        this_tue = dateTemp.AddDays(7);
                    //    }
                    //    else if (FCommon.getQueryString(this, "date2", out string date2, true) && DateTime.TryParse(date2, out dateTemp))
                    //    {
                    //        last_wed = dateTemp.AddDays(-7);
                    //        this_tue = dateTemp;
                    //    }
                    //    else
                    //    {
                    //        DateTime dateNow = DateTime.Now;
                    //        int today_dayofweek = (int)dateNow.DayOfWeek; //取得今天星期幾
                    //        last_wed = dateNow.AddDays(-7 + (3 - today_dayofweek)); //取得上週三日期
                    //        this_tue = dateNow.AddDays(2 - today_dayofweek); //取得這週二日期
                    //    }
                    //    steDateText(last_wed, this_tue);
                    //    #endregion
                    //    if (FCommon.getQueryString(this, "id", out string strOVC_BLD_NO, true))
                    //        txtOVC_BLD_NO.Text = strOVC_BLD_NO;
                    //    if (FCommon.getQueryString(this, "note", out string strOVC_NOTE, true))
                    //        txtOVC_NOTE.Text = strOVC_NOTE;
                    //    dataImport();
                    //}
                    #endregion
                    //}
                    //else
                    //    FCommon.MessageBoxShow(this, "尚未登入，請先登入！", "login", true);
                }

                intAuth = ViewState["auth"] != null ? (int)ViewState["auth"] : 0; //取得權限
                if (intAuth == 0)
                    FCommon.showMessageAuth(this, Path.GetFileName(Request.Path), false); //顯示無權限之訊息，並返回上一頁
            }
        }

        #region ~Click
        protected void Button_lastweek_Click(object sender, EventArgs e)
        {
            int addDay = -7;
            onWeekClick(addDay);
        }
        protected void Button_nextweek_Click(object sender, EventArgs e)
        {
            int addDay = 7;
            onWeekClick(addDay);
        }
        protected void btnStatistic_Click(object sender, EventArgs e)
        {
            statistic();
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport_GV_WRP_BLD();
            //string strMessage = "";
            //string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            //string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
            //string strODT_WEEK_DATE1 = txtODT_WEEK_DATE1.Text;
            //string strODT_WEEK_DATE2 = txtODT_WEEK_DATE2.Text;
            //DateTime dateODT_WEEK_DATE1, dateODT_WEEK_DATE2;
            //if (strOVC_BLD_NO.Equals(string.Empty))
            //    strMessage += "<p> 請輸入 提單編號 </p>";
            //bool bookODT_WEEK_DATE1 = FCommon.checkDateTime(strODT_WEEK_DATE1, "資料時間－前者", ref strMessage, out dateODT_WEEK_DATE1);
            //bool bookODT_WEEK_DATE2 = FCommon.checkDateTime(strODT_WEEK_DATE2, "資料時間－後者", ref strMessage, out dateODT_WEEK_DATE2);
            //if (strMessage.Equals(string.Empty))
            //{
            //    string strQueryString = getQueryString();
            //    string strScript = $@"
            //        <script language='javascript'>
            //            var win = 
            //                window.open('MTS_H11_2{ strQueryString }', null, 'width=1200,height=700,left=0,top=0');
            //        </script>
            //    ";
            //    ClientScript.RegisterStartupScript(GetType(), "Open", strScript);
            //    //string strSQL = Path.GetFileName(Request.Path);
            //    //Response.Redirect(strSQL + strQueryString);
            //}
            //else
            //    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                string strMessage = "";
                string strUserId = Session["userid"].ToString();
                string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
                string strODT_WEEK_DATE1 = txtODT_WEEK_DATE1.Text;
                string strODT_WEEK_DATE2 = txtODT_WEEK_DATE2.Text;
                string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
                string strOVC_NOTE = txtOVC_NOTE.Text;

                DateTime dateODT_WEEK_DATE1, dateODT_WEEK_DATE2, dateNow = DateTime.Now;
                if (strOVC_SECTION.Equals(string.Empty))
                    strMessage += "<p> 接轉地區不存在 </p>";
                if (strODT_WEEK_DATE1.Equals(string.Empty))
                    strMessage += "<p> 請輸入 資料時間－前者 </p>";
                bool boolODT_WEEK_DATE1 = FCommon.checkDateTime(strODT_WEEK_DATE1, "資料時間－前者", ref strMessage, out dateODT_WEEK_DATE1);
                bool boolODT_WEEK_DATE2 = FCommon.checkDateTime(strODT_WEEK_DATE2, "資料時間－後者", ref strMessage, out dateODT_WEEK_DATE2);
                if (strOVC_BLD_NO.Equals(string.Empty))
                    strMessage += "<p> 請輸入 提單編號 </p>";
                else
                {
                    //TBGMT_WRP_BLD wrp_bld = MTSE.TBGMT_WRP_BLD.Where(table =>
                    //    table.OVC_SECTION.Equals(strOVC_SECTION) && DateTime.Compare(table.ODT_WEEK_DATE, dateODT_WEEK_DATE1) == 0 && table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                    TBGMT_WRP_BLD wrp_bld = MTSE.TBGMT_WRP_BLD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                    if (wrp_bld != null)
                        strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 已存在於 接轉作業週報表：{ wrp_bld.OVC_SECTION }-{ FCommon.getDateTime(wrp_bld.ODT_WEEK_DATE) }，請直接更新！ </p>";
                    else
                    {
                        TBGMT_BLD bld = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                        if (bld != null)
                        {
                            string strOVC_START_PORT = bld.OVC_START_PORT;
                            string strOVC_ARRIVE_PORT = bld.OVC_ARRIVE_PORT;
                            var queryPort = from port in MTSE.TBGMT_PORTS select port;
                            var queryPort_str = queryPort.Where(table => table.OVC_PORT_CDE.Equals(strOVC_START_PORT)).FirstOrDefault();
                            var queryPort_arr = queryPort.Where(table => table.OVC_PORT_CDE.Equals(strOVC_ARRIVE_PORT)).FirstOrDefault();

                            var querySection = from tr in GME.TBM1407 where tr.OVC_PHR_CATE.Equals("TR") select tr;

                            if (queryPort_str != null && queryPort_str.OVC_IS_ABROAD.Equals("國內")) //出口
                            {
                                bool isRangeDate = false; //確認日期範圍
                                #region 日期範圍
                                ArrayList aryDate = new ArrayList();
                                var queryESO =
                                    from eso in MTSE.TBGMT_ESO
                                    where eso.OVC_BLD_NO.Equals(strOVC_BLD_NO)
                                    where eso.ODT_STORED_DATE != null
                                    select eso;
                                foreach (TBGMT_ESO eso in queryESO)
                                {
                                    DateTime theDate = Convert.ToDateTime(eso.ODT_STORED_DATE);
                                    if (DateTime.Compare(theDate, dateODT_WEEK_DATE1) >= 0 && DateTime.Compare(theDate, dateODT_WEEK_DATE2) <= 0)
                                        isRangeDate = true;
                                    else
                                        aryDate.Add(FCommon.getDateTime(theDate));
                                }
                                if (!isRangeDate)
                                {
                                    string strDate = string.Join(",", (string[])aryDate.ToArray(typeof(string)));
                                    strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 為出口提單，有效日期位於 { strDate }！ </p>";
                                }
                                #endregion
                                #region 接轉地區
                                string strSection = "";
                                TBM1407 tr_str = querySection.Where(table => table.OVC_PHR_ID.Equals(strOVC_START_PORT)).FirstOrDefault();
                                if (tr_str != null && tr_str.OVC_PHR_PARENTS != null)
                                {
                                    strSection = tr_str.OVC_PHR_PARENTS;
                                    if (!strSection.Equals(strOVC_SECTION))
                                        strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 接轉地區為 { strSection }！ </p>";
                                }
                                else
                                    strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 接轉地區不存在！ </p>";
                                #endregion
                            }
                            else if (queryPort_arr != null && queryPort_arr.OVC_IS_ABROAD.Equals("國內")) //進口
                            {
                                bool isRangeDate = false; //確認日期範圍
                                #region 日期範圍
                                ArrayList aryDate = new ArrayList();
                                var queryICR =
                                    from icr in MTSE.TBGMT_ICR
                                    where icr.OVC_BLD_NO.Equals(strOVC_BLD_NO)
                                    where icr.ODT_TRANSFER_DATE != null
                                    select icr;
                                foreach (TBGMT_ICR icr in queryICR)
                                {
                                    DateTime theDate = Convert.ToDateTime(icr.ODT_TRANSFER_DATE);
                                    if (DateTime.Compare(theDate, dateODT_WEEK_DATE1) >= 0 && DateTime.Compare(theDate, dateODT_WEEK_DATE2) <= 0)
                                        isRangeDate = true;
                                    else
                                        aryDate.Add(FCommon.getDateTime(theDate));
                                }
                                if (!isRangeDate)
                                {
                                    string strDate = string.Join(",", (string[])aryDate.ToArray(typeof(string)));
                                    strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 為進口提單，有效日期位於 { strDate }！ </p>";
                                }
                                #endregion
                                #region 接轉地區
                                string strSection = "";
                                TBM1407 tr_arr = querySection.Where(table => table.OVC_PHR_ID.Equals(strOVC_ARRIVE_PORT)).FirstOrDefault();
                                if (tr_arr != null && tr_arr.OVC_PHR_PARENTS != null)
                                {
                                    strSection = tr_arr.OVC_PHR_PARENTS;
                                    if (!strSection.Equals(strOVC_SECTION))
                                        strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 接轉地區為 { strSection }！ </p>";
                                }
                                else
                                    strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 接轉地區不存在！ </p>";
                                #endregion
                            }
                            else
                                strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 定位錯誤！ </p>";
                        }
                        else
                            strMessage += $"<p> 提單編號：{ strOVC_BLD_NO } 不存在！ </p>";
                    }
                    if (strMessage.Equals(string.Empty))
                    {
                        wrp_bld = new TBGMT_WRP_BLD();
                        wrp_bld.OVC_SECTION = strOVC_SECTION;
                        wrp_bld.ODT_WEEK_DATE = dateODT_WEEK_DATE1;
                        wrp_bld.OVC_BLD_NO = strOVC_BLD_NO;
                        wrp_bld.OVC_NOTE = strOVC_NOTE;
                        wrp_bld.ODT_CREATE_DATE = dateNow;
                        wrp_bld.OVC_CREATE_LOGIN_ID = strUserId;
                        wrp_bld.ODT_MODIFY_DATE = dateNow;
                        wrp_bld.OVC_MODIFY_LOGIN_ID = strUserId;

                        MTSE.TBGMT_WRP_BLD.Add(wrp_bld);
                        MTSE.SaveChanges();
                        dataImport_GV_WRP_BLD();
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", $"提單編號：{ strOVC_BLD_NO }，新增成功！");
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                }
            }
            catch
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "新增失敗");
            }
        }
        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                string strMessage = "";
                string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
                string strODT_WEEK_DATE = txtODT_WEEK_DATE1.Text;
                string strOVC_BLD_NO = txtOVC_BLD_NO.Text;

                DateTime dateODT_WEEK_DATE;
                if (strOVC_SECTION.Equals(string.Empty))
                    strMessage += "<p> 接轉地區不存在 </p>";
                if (strODT_WEEK_DATE.Equals(string.Empty))
                    strMessage += "<p> 請輸入 資料時間－前者 </p>";
                if (strOVC_BLD_NO.Equals(string.Empty))
                    strMessage += "<p> 請輸入 提單編號 </p>";
                bool boolODT_WEEK_DATE = FCommon.checkDateTime(strODT_WEEK_DATE, "資料時間－前者", ref strMessage, out dateODT_WEEK_DATE);
                if (strMessage.Equals(string.Empty))
                {
                    TBGMT_WRP_BLD wrp_bld = MTSE.TBGMT_WRP_BLD.Where(table =>
                        table.OVC_SECTION.Equals(strOVC_SECTION) && DateTime.Compare(table.ODT_WEEK_DATE, dateODT_WEEK_DATE) == 0 && table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                    if (wrp_bld != null)
                    {
                        MTSE.Entry(wrp_bld).State = EntityState.Deleted;
                        MTSE.SaveChanges();
                        dataImport_GV_WRP_BLD();
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", $"提單編號：{ strOVC_BLD_NO }，刪除成功！");
                    }
                    else
                    {
                        wrp_bld = MTSE.TBGMT_WRP_BLD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                        if (wrp_bld != null)
                            strMessage = $"提單編號：{ strOVC_BLD_NO } 存在於 接轉作業週報表：{ wrp_bld.OVC_SECTION }-{ FCommon.getDateTime(wrp_bld.ODT_WEEK_DATE) }！";
                        else
                            strMessage = $"提單編號：{ strOVC_BLD_NO } 不存在於 接轉作業週報表，已被刪除！";
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                    }
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
            catch
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "刪除失敗");
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string strMessage = "";
                string strUserId = Session["userid"].ToString();
                string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
                string strODT_WEEK_DATE = txtODT_WEEK_DATE1.Text;
                string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
                string strOVC_NOTE = txtOVC_NOTE.Text;

                DateTime dateODT_WEEK_DATE, dateNow = DateTime.Now;
                if (strOVC_SECTION.Equals(string.Empty))
                    strMessage += "<p> 接轉地區不存在 </p>";
                if (strODT_WEEK_DATE.Equals(string.Empty))
                    strMessage += "<p> 請輸入 資料時間－前者 </p>";
                if (strOVC_BLD_NO.Equals(string.Empty))
                    strMessage += "<p> 請輸入 提單編號 </p>";
                bool boolODT_WEEK_DATE = FCommon.checkDateTime(strODT_WEEK_DATE, "資料時間－前者", ref strMessage, out dateODT_WEEK_DATE);
                if (strMessage.Equals(string.Empty))
                {
                    TBGMT_WRP_BLD wrp_bld = MTSE.TBGMT_WRP_BLD.Where(table =>
                        table.OVC_SECTION.Equals(strOVC_SECTION) && DateTime.Compare(table.ODT_WEEK_DATE, dateODT_WEEK_DATE) == 0 && table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                    if (wrp_bld != null)
                    {
                        wrp_bld.OVC_NOTE = strOVC_NOTE;
                        wrp_bld.ODT_MODIFY_DATE = dateNow;
                        wrp_bld.OVC_MODIFY_LOGIN_ID = strUserId;

                        MTSE.SaveChanges();
                        dataImport_GV_WRP_BLD();
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", $"提單編號：{ strOVC_BLD_NO }，更新成功！");
                    }
                    else
                    {
                        wrp_bld = MTSE.TBGMT_WRP_BLD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                        if (wrp_bld != null)
                            strMessage = $"提單編號：{ strOVC_BLD_NO } 存在於 接轉作業週報表：{ wrp_bld.OVC_SECTION }-{ FCommon.getDateTime(wrp_bld.ODT_WEEK_DATE) }！";
                        else
                            strMessage = $"提單編號：{ strOVC_BLD_NO } 不存在於 接轉作業週報表，請先新增！";
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                    }
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
            catch
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "更新失敗");
            }
        }
        protected void btnUpdateWeek_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            string strONB_SHIP_AIR = txtONB_SHIP_AIR.Text;
            string strONB_SHIP_SEA = txtONB_SHIP_SEA.Text;
            string strONB_BLD_AIR = txtONB_BLD_AIR.Text;
            string strONB_BLD_SEA = txtONB_BLD_SEA.Text;
            string strONB_20_COUNT = txtONB_20_COUNT.Text;
            string strONB_40_COUNT = txtONB_40_COUNT.Text;
            string strONB_45_COUNT = txtONB_45_COUNT.Text;
            string strONB_QUANITY = txtONB_QUANITY.Text;
            string strONB_WEIGHT = txtONB_WEIGHT.Text;
            string strONB_VOLUME = txtONB_VOLUME.Text;
            string strONB_TRANS_DEFAULT = txtONB_TRANS_DEFAULT.Text;
            string strONB_TRANS_SUPPLIER = txtONB_TRANS_SUPPLIER.Text;
            string strONB_TRANS_TRAIN = txtONB_TRANS_TRAIN.Text;

            //string strOVC_SECTION = drpOVC_SECTION.SelectedValue;
            string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
            string strODT_WEEK_DATE1 = txtODT_WEEK_DATE1.Text;
            //string strODT_WEEK_DATE2 = txtODT_WEEK_DATE2.Text;

            decimal decONB_SHIP_AIR, decONB_SHIP_SEA, decONB_BLD_AIR, decONB_BLD_SEA, decONB_20_COUNT, decONB_40_COUNT, decONB_45_COUNT,
                decONB_QUANITY, decONB_WEIGHT, decONB_VOLUME, decONB_TRANS_DEFAULT, decONB_TRANS_SUPPLIER, decONB_TRANS_TRAIN;
            DateTime dateODT_WEEK_DATE1, dateODT_WEEK_DATE2;

            #region 錯誤訊息
            //if (strOVC_BLD_NO.Equals(string.Empty))
            //    strMessage += "<p> 請輸入 提單編號 </p>";
            if (strOVC_SECTION.Equals(string.Empty))
                strMessage += "<p> 接轉地區不存在！ </p>";
            if (strODT_WEEK_DATE1.Equals(string.Empty))
                strMessage += "<p> 請選擇 資料時間－前者 </p>";
            bool bookODT_WEEK_DATE1 = FCommon.checkDateTime(strODT_WEEK_DATE1, "資料時間－前者", ref strMessage, out dateODT_WEEK_DATE1);
            //bool bookODT_WEEK_DATE2 = FCommon.checkDateTime(strODT_WEEK_DATE2, "資料時間－後者", ref strMessage, out dateODT_WEEK_DATE2);
            bool boolONB_SHIP_AIR = FCommon.checkDecimal(strONB_SHIP_AIR, "空運架次", ref strMessage, out decONB_SHIP_AIR);
            bool boolONB_SHIP_SEA = FCommon.checkDecimal(strONB_SHIP_SEA, "海運航次", ref strMessage, out decONB_SHIP_SEA);
            bool boolONB_BLD_AIR = FCommon.checkDecimal(strONB_BLD_AIR, "空運報關數", ref strMessage, out decONB_BLD_AIR);
            bool boolONB_BLD_SEA = FCommon.checkDecimal(strONB_BLD_SEA, "海運報關數", ref strMessage, out decONB_BLD_SEA);
            bool boolONB_20_COUNT = FCommon.checkDecimal(strONB_20_COUNT, "貨櫃20呎", ref strMessage, out decONB_20_COUNT);
            bool boolONB_40_COUNT = FCommon.checkDecimal(strONB_40_COUNT, "貨櫃40呎", ref strMessage, out decONB_40_COUNT);
            bool boolONB_45_COUNT = FCommon.checkDecimal(strONB_45_COUNT, "貨櫃45呎", ref strMessage, out decONB_45_COUNT);
            bool boolONB_QUANITY = FCommon.checkDecimal(strONB_QUANITY, "接轉件數", ref strMessage, out decONB_QUANITY);
            bool boolONB_WEIGHT = FCommon.checkDecimal(strONB_WEIGHT, "接轉重量KG", ref strMessage, out decONB_WEIGHT);
            bool boolONB_VOLUME = FCommon.checkDecimal(strONB_VOLUME, "接轉體積CBM", ref strMessage, out decONB_VOLUME);
            bool boolONB_TRANS_DEFAULT = FCommon.checkDecimal(strONB_TRANS_DEFAULT, "建制輸具車次", ref strMessage, out decONB_TRANS_DEFAULT);
            bool boolONB_TRANS_SUPPLIER = FCommon.checkDecimal(strONB_TRANS_SUPPLIER, "委商輸具車次", ref strMessage, out decONB_TRANS_SUPPLIER);
            bool boolONB_TRANS_TRAIN = FCommon.checkDecimal(strONB_TRANS_TRAIN, "鐵運車次", ref strMessage, out decONB_TRANS_TRAIN);
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                try
                {
                    TBGMT_WRP wrp = MTSE.TBGMT_WRP.Where(table => table.OVC_SECTION.Equals(strOVC_SECTION)).Where(table => DateTime.Compare(table.ODT_WEEK_DATE, dateODT_WEEK_DATE1) == 0).FirstOrDefault();
                    if (wrp != null)
                    {
                        if (boolONB_SHIP_AIR) wrp.ONB_SHIP_AIR = decONB_SHIP_AIR; else wrp.ONB_SHIP_AIR = null;
                        if (boolONB_SHIP_SEA) wrp.ONB_SHIP_SEA = decONB_SHIP_SEA; else wrp.ONB_SHIP_SEA = null;
                        if (boolONB_BLD_AIR) wrp.ONB_BLD_AIR = decONB_BLD_AIR; else wrp.ONB_BLD_AIR = null;
                        if (boolONB_BLD_SEA) wrp.ONB_BLD_SEA = decONB_BLD_SEA; else wrp.ONB_BLD_SEA = null;
                        if (boolONB_20_COUNT) wrp.ONB_20_COUNT = decONB_20_COUNT; else wrp.ONB_20_COUNT = null;
                        if (boolONB_40_COUNT) wrp.ONB_40_COUNT = decONB_40_COUNT; else wrp.ONB_40_COUNT = null;
                        if (boolONB_45_COUNT) wrp.ONB_45_COUNT = decONB_45_COUNT; else wrp.ONB_45_COUNT = null;
                        if (boolONB_QUANITY) wrp.ONB_QUANITY = decONB_QUANITY; else wrp.ONB_QUANITY = null;
                        if (boolONB_WEIGHT) wrp.ONB_WEIGHT = decONB_WEIGHT; else wrp.ONB_WEIGHT = null;
                        if (boolONB_VOLUME) wrp.ONB_VOLUME = decONB_VOLUME; else wrp.ONB_VOLUME = null;
                        if (boolONB_TRANS_DEFAULT) wrp.ONB_TRANS_DEFAULT = decONB_TRANS_DEFAULT; else wrp.ONB_TRANS_DEFAULT = null;
                        if (boolONB_TRANS_SUPPLIER) wrp.ONB_TRANS_SUPPLIER = decONB_TRANS_SUPPLIER; else wrp.ONB_TRANS_SUPPLIER = null;
                        if (boolONB_TRANS_TRAIN) wrp.ONB_TRANS_TRAIN = decONB_TRANS_TRAIN; else wrp.ONB_TRANS_TRAIN = null;
                        MTSE.SaveChanges();

                        dataImport();
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", $"接轉作業週報表：{ strOVC_SECTION }-{ strODT_WEEK_DATE1 } 更新成功！");
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"接轉作業週報表：{ strOVC_SECTION }-{ strODT_WEEK_DATE1 } 不存在！");
                    //TBGMT_WRP_BLD wrp_bld = MTSE.TBGMT_WRP_BLD.Where(table => table.OVC_SECTION.Equals(strOVC_SECTION)).Where(table => DateTime.Compare(table.ODT_WEEK_DATE, dateODT_WEEK_DATE1) == 0).FirstOrDefault();
                    //if (wrp_bld == null)
                    //{
                    //    //wrp_bld.OVC_SECTION = ovc_section;
                    //    //wrp_bld.ODT_WEEK_DATE = weekdate1;
                    //    MTSE.SaveChanges();
                    //}
                    //else
                    //    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "TBGMT_WRP_BLD 不存在");
                }
                catch
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "更新失敗");
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            //string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            //string strONB_SHIP_AIR = txtONB_SHIP_AIR.Text;
            //string strONB_SHIP_SEA = txtONB_SHIP_SEA.Text;
            //string strONB_BLD_AIR = txtONB_BLD_AIR.Text;
            //string strONB_BLD_SEA = txtONB_BLD_SEA.Text;
            //string strONB_20_COUNT = txtONB_20_COUNT.Text;
            //string strONB_40_COUNT = txtONB_40_COUNT.Text;
            //string strONB_45_COUNT = txtONB_45_COUNT.Text;
            //string strONB_QUANITY = txtONB_QUANITY.Text;
            //string strONB_WEIGHT = txtONB_WEIGHT.Text;
            //string strONB_VOLUME = txtONB_VOLUME.Text;
            //string strONB_TRANS_DEFAULT = txtONB_TRANS_DEFAULT.Text;
            //string strONB_TRANS_SUPPLIER = txtONB_TRANS_SUPPLIER.Text;
            //string strONB_TRANS_TRAIN = txtONB_TRANS_TRAIN.Text;

            ////string strOVC_SECTION = drpOVC_SECTION.SelectedValue;
            string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
            string strODT_WEEK_DATE1 = txtODT_WEEK_DATE1.Text;
            //string strODT_WEEK_DATE2 = txtODT_WEEK_DATE2.Text;

            //decimal decONB_SHIP_AIR, decONB_SHIP_SEA, decONB_BLD_AIR, decONB_BLD_SEA, decONB_20_COUNT, decONB_40_COUNT, decONB_45_COUNT,
            //    decONB_QUANITY, decONB_WEIGHT, decONB_VOLUME, decONB_TRANS_DEFAULT, decONB_TRANS_SUPPLIER, decONB_TRANS_TRAIN;

            #region 錯誤訊息
            if (strOVC_SECTION.Equals(string.Empty))
                strMessage += "<p> 接轉地區不存在！ </p>";
            if (strODT_WEEK_DATE1.Equals(string.Empty))
                strMessage += "<p> 請選擇 資料時間－前者！ </p>";
            bool bookODT_WEEK_DATE1 = FCommon.checkDateTime(strODT_WEEK_DATE1, "資料時間－前者", ref strMessage, out DateTime dateODT_WEEK_DATE1);
            //bool bookODT_WEEK_DATE2 = FCommon.checkDateTime(strODT_WEEK_DATE2, "資料時間－後者", ref strMessage, out dateODT_WEEK_DATE2);
            //bool boolONB_SHIP_AIR = FCommon.checkDecimal(strONB_SHIP_AIR, "空運架次", ref strMessage, out decONB_SHIP_AIR);
            //bool boolONB_SHIP_SEA = FCommon.checkDecimal(strONB_SHIP_SEA, "海運航次", ref strMessage, out decONB_SHIP_SEA);
            //bool boolONB_BLD_AIR = FCommon.checkDecimal(strONB_BLD_AIR, "空運報關數", ref strMessage, out decONB_BLD_AIR);
            //bool boolONB_BLD_SEA = FCommon.checkDecimal(strONB_BLD_SEA, "海運報關數", ref strMessage, out decONB_BLD_SEA);
            //bool boolONB_20_COUNT = FCommon.checkDecimal(strONB_20_COUNT, "貨櫃20呎", ref strMessage, out decONB_20_COUNT);
            //bool boolONB_40_COUNT = FCommon.checkDecimal(strONB_40_COUNT, "貨櫃40呎", ref strMessage, out decONB_40_COUNT);
            //bool boolONB_45_COUNT = FCommon.checkDecimal(strONB_45_COUNT, "貨櫃45呎", ref strMessage, out decONB_45_COUNT);
            //bool boolONB_QUANITY = FCommon.checkDecimal(strONB_QUANITY, "接轉件數", ref strMessage, out decONB_QUANITY);
            //bool boolONB_WEIGHT = FCommon.checkDecimal(strONB_WEIGHT, "接轉重量KG", ref strMessage, out decONB_WEIGHT);
            //bool boolONB_VOLUME = FCommon.checkDecimal(strONB_VOLUME, "接轉體積CBM", ref strMessage, out decONB_VOLUME);
            //bool boolONB_TRANS_DEFAULT = FCommon.checkDecimal(strONB_TRANS_DEFAULT, "建制輸具車次", ref strMessage, out decONB_TRANS_DEFAULT);
            //bool boolONB_TRANS_SUPPLIER = FCommon.checkDecimal(strONB_TRANS_SUPPLIER, "委商輸具車次", ref strMessage, out decONB_TRANS_SUPPLIER);
            //bool boolONB_TRANS_TRAIN = FCommon.checkDecimal(strONB_TRANS_TRAIN, "鐵運車次", ref strMessage, out decONB_TRANS_TRAIN);
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                TBGMT_WRP wrp = MTSE.TBGMT_WRP.Where(table => table.OVC_SECTION.Equals(strOVC_SECTION) && DateTime.Compare(table.ODT_WEEK_DATE, dateODT_WEEK_DATE1) == 0).FirstOrDefault();
                if (wrp != null)
                {
                    string strONB_SHIP_AIR = wrp.ONB_SHIP_AIR.ToString();
                    string strONB_SHIP_SEA = wrp.ONB_SHIP_SEA.ToString();
                    string strONB_BLD_AIR = wrp.ONB_BLD_AIR.ToString();
                    string strONB_BLD_SEA = wrp.ONB_BLD_SEA.ToString();
                    string strONB_20_COUNT = wrp.ONB_20_COUNT.ToString();
                    string strONB_40_COUNT = wrp.ONB_40_COUNT.ToString();
                    string strONB_45_COUNT = wrp.ONB_45_COUNT.ToString();
                    string strONB_QUANITY = wrp.ONB_QUANITY.ToString();
                    string strONB_WEIGHT = wrp.ONB_WEIGHT.ToString();
                    string strONB_VOLUME = wrp.ONB_VOLUME.ToString();
                    string strONB_TRANS_DEFAULT = wrp.ONB_TRANS_DEFAULT.ToString();
                    string strONB_TRANS_SUPPLIER = wrp.ONB_TRANS_SUPPLIER.ToString();
                    string strONB_TRANS_TRAIN = wrp.ONB_TRANS_TRAIN.ToString();

                    decimal decONB_BLD_AIR, decONB_BLD_SEA, decONB_20_COUNT, decONB_40_COUNT, decONB_45_COUNT;
                    decimal.TryParse(strONB_BLD_AIR, out decONB_BLD_AIR);
                    decimal.TryParse(strONB_BLD_SEA, out decONB_BLD_SEA);
                    decimal.TryParse(strONB_20_COUNT, out decONB_20_COUNT);
                    decimal.TryParse(strONB_40_COUNT, out decONB_40_COUNT);
                    decimal.TryParse(strONB_45_COUNT, out decONB_45_COUNT);

                    Document doc = new Document(PageSize.A4, 10, 10, 50, 20);
                    MemoryStream Memory = new MemoryStream();
                    PdfWriter writer = PdfWriter.GetInstance(doc, Memory);
                    doc.Open();

                    //設定字型
                    BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H
                  , BaseFont.NOT_EMBEDDED);
                    Font ChFont = new Font(bfChinese, 12, Font.NORMAL, BaseColor.BLACK);
                    string chFontPath = "c:\\windows\\fonts\\KAIU.TTF";
                    BaseFont chBaseFont = BaseFont.CreateFont(chFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                    PdfContentByte cb = writer.DirectContent;
                    Rectangle pageSize = doc.PageSize;
                    cb.SetRGBColorFill(0, 0, 0);
                    cb.BeginText();
                    cb.SetFontAndSize(chBaseFont, 12);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, $"{ strDEPT_Name } { strOVC_SECTION } 接轉作業週報表", pageSize.GetLeft(300), pageSize.GetTop(25), 0);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "資料時間：" + txtODT_WEEK_DATE1.Text + "至" + txtODT_WEEK_DATE2.Text, pageSize.GetLeft(300), pageSize.GetTop(40), 0);
                    cb.EndText();

                    PdfPTable pdftable = new PdfPTable(new float[] { 2, 2, 2, 2, 2, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2 });
                    pdftable.TotalWidth = 580f;
                    pdftable.LockedWidth = true;
                    pdftable.DefaultCell.FixedHeight = 25;


                    PdfPCell cell1_1 = new PdfPCell(new Phrase("接轉狀況", ChFont));
                    cell1_1.Colspan = 2;
                    cell1_1.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdftable.AddCell(cell1_1);

                    PdfPCell cell1_2 = new PdfPCell(new Phrase("報關案數", ChFont));
                    cell1_2.Colspan = 3;
                    cell1_2.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdftable.AddCell(cell1_2);

                    PdfPCell cell1_3 = new PdfPCell(new Phrase("接轉貨櫃", ChFont));
                    cell1_3.Colspan = 4;
                    cell1_3.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdftable.AddCell(cell1_3);

                    PdfPCell cell1_4 = new PdfPCell(new Phrase("接轉件數", ChFont));
                    cell1_4.Rowspan = 2;
                    cell1_4.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdftable.AddCell(cell1_4);

                    PdfPCell cell1_5 = new PdfPCell(new Phrase("接轉重量(KG)", ChFont));
                    cell1_5.Rowspan = 2;
                    cell1_5.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdftable.AddCell(cell1_5);

                    PdfPCell cell1_6 = new PdfPCell(new Phrase("接轉體積(CBM)", ChFont));
                    cell1_6.Rowspan = 2;
                    cell1_6.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdftable.AddCell(cell1_6);

                    PdfPCell cell1_7 = new PdfPCell(new Phrase("運輸車次", ChFont));
                    cell1_7.Colspan = 3;
                    cell1_7.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdftable.AddCell(cell1_7);

                    string[] cell2_name = { "空運(架次)", "航運(架次)", "空運", "海運", "小計", "20呎", "40呎", "45呎", "小計", "建制輸具", "委商輸具", "鐵運" };
                    for (int i = 0; i < cell2_name.Length; i++)
                    {
                        PdfPCell cell2 = new PdfPCell(new Phrase(cell2_name[i], ChFont));
                        cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdftable.AddCell(cell2);
                    }
                    string onbbld_total2 = (decONB_BLD_AIR + decONB_BLD_SEA).ToString();
                    string onb_total2 = (decONB_20_COUNT + decONB_40_COUNT + decONB_45_COUNT).ToString();
                    string[] cell3_name = { strONB_SHIP_AIR, strONB_SHIP_SEA, strONB_BLD_AIR, strONB_BLD_SEA, onbbld_total2,
                    strONB_20_COUNT, strONB_40_COUNT, strONB_45_COUNT, onb_total2, strONB_QUANITY, strONB_WEIGHT, strONB_VOLUME, strONB_TRANS_DEFAULT, strONB_TRANS_SUPPLIER, strONB_TRANS_TRAIN };
                    for (int i = 0; i < cell3_name.Length; i++)
                    {
                        PdfPCell cell3 = new PdfPCell(new Phrase(cell3_name[i], ChFont));
                        cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdftable.AddCell(cell3);
                    }
                    doc.Add(pdftable);
                    doc.Add(new Phrase("\r\n", ChFont));
                    PdfPTable pdftable2 = new PdfPTable(new float[] { 3, 1, 2, 3, 3, 3, 3, 3, 3, 4 });
                    pdftable2.TotalWidth = 580f;
                    pdftable2.LockedWidth = true;
                    pdftable2.DefaultCell.FixedHeight = 30;

                    string[] cell4_name = { "品名", "箱件數量", "噸位", "接收單位", "進口日期", "通關日期", "進倉日期", "接轉日期", "作業單位", "備考" };
                    string[] cell4_sql = { "OVC_CHI_NAME", "ONB_QUANITY", "ONB_VOLUME", "OVC_RECEIVE_DEPT", "ODT_IMPORT_DATE", "ODT_PASS_DATE", "ODT_STORED_DATE", "ODT_TRANSFER_DATE", "OVC_SECTION", "OVC_NOTE" };
                    
                    for (int i = 0; i < cell4_name.Length; i++)
                    {
                        PdfPCell cell4 = new PdfPCell(new Phrase(cell4_name[i], ChFont));
                        cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell4.VerticalAlignment = Element.ALIGN_CENTER;
                        pdftable2.AddCell(cell4);
                    }

                    //var query = from wrp_bld in MTSE.TBGMT_WRP_BLD
                    //            join eso in MTSE.TBGMT_ESO on wrp_bld.OVC_BLD_NO equals eso.OVC_BLD_NO into edf2
                    //            from eso in edf2.DefaultIfEmpty()
                    //            join edf_detail in MTSE.TBGMT_EDF_DETAIL on eso.OVC_EDF_NO equals edf_detail.OVC_EDF_NO into edf_detail2
                    //            from edf_detail in edf_detail2.DefaultIfEmpty()
                    //            join icr in MTSE.TBGMT_ICR on wrp_bld.OVC_BLD_NO equals icr.OVC_BLD_NO into icr2
                    //            from icr in icr2.DefaultIfEmpty()
                    //            join dept in MTSE.TBGMT_DEPT_CDE on icr.OVC_RECEIVE_DEPT_CODE equals dept.OVC_DEPT_CODE into dept2
                    //            from dept in dept2.DefaultIfEmpty()
                    //            select new
                    //            {
                    //                OVC_CHI_NAME = edf_detail.OVC_CHI_NAME,
                    //                ONB_ITEM_COUNT = edf_detail.ONB_ITEM_COUNT,
                    //                ONB_WEIGHTONB_VOLUME = edf_detail.ONB_VOLUME,
                    //                OVC_DEPT_NAME = dept.OVC_DEPT_NAME,
                    //                ODT_IMPORT_DATE = icr.ODT_IMPORT_DATE,
                    //                ODT_PASS_CUSTOM_DATE = icr.ODT_PASS_CUSTOM_DATE,
                    //                ODT_UNPACKING_DATE = icr.ODT_UNPACKING_DATE,
                    //                ODT_TRANSFER_DATE = icr.ODT_TRANSFER_DATE,
                    //                drp_area = strOVC_SECTION,
                    //                OVC_NOTE = wrp_bld.OVC_NOTE,
                    //                ODT_WEEK_DATE = wrp_bld.ODT_WEEK_DATE,
                    //                OVC_SECTION = wrp_bld.OVC_SECTION,
                    //                OVC_VOLUME_UNIT = edf_detail.OVC_VOLUME_UNIT
                    //            };

                    //query = query.Where(table => DateTime.Compare(table.ODT_WEEK_DATE, dateODT_WEEK_DATE1) == 0);
                    //query = query.Where(table => table.OVC_SECTION.Equals(strOVC_SECTION));

                    //DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                    DataTable dt = getWRP_BLD(strOVC_SECTION, strODT_WEEK_DATE1);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            //if (dt.Rows[i][12].ToString() == "CF")
                            //{
                            //    dt.Rows[i][2] = Convert.ToDouble(dt.Rows[i][2]) / 35.315;
                            //}
                            for (int j = 0; j < cell4_sql.Length; j++)
                            {
                                string strField = cell4_sql[j];
                                PdfPCell cell5 = new PdfPCell(new Phrase(dr[strField].ToString(), ChFont));
                                cell5.HorizontalAlignment = Element.ALIGN_CENTER;
                                pdftable2.AddCell(cell5);
                            }
                        }
                    }
                    doc.Add(pdftable2);
                    doc.Close();

                    string strFileName = $"{ strDEPT_Name }接轉作業週報表.pdf";
                    FCommon.DownloadFile(this, strFileName, Memory);

                    //Response.Clear();
                    //Response.ContentType = "application/octet-stream";
                    //Response.AddHeader("Content-Disposition", $"attachment; filename={ strDEPT_Name }接轉作業週報表.pdf");
                    //Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
                    //Response.OutputStream.Flush(); ;
                    //Response.OutputStream.Close();
                    //Response.Flush();
                    //Response.End();
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"<p> 接轉作業週報表：{ strOVC_SECTION }-{ strODT_WEEK_DATE1 } 不存在！ </p>");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        #endregion

        protected void drpOVC_SECTION_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string strSQL = Path.GetFileName(Request.Path);
            //Response.Redirect(strSQL + getQueryString());
            dataImport();
        }

        protected void txtODT_WEEK_DATE1_TextChanged(object sender, EventArgs e)
        {
            string strMessage = "";
            string strODT_WEEK_DATE1 = txtODT_WEEK_DATE1.Text;
            DateTime dateODT_WEEK_DATE1, dateODT_WEEK_DATE2;
            if (strODT_WEEK_DATE1.Equals(string.Empty))
                strMessage += "<p> 請選擇 資料時間－前者 </p>";
            bool boolODT_WEEK_DATE1 = FCommon.checkDateTime(strODT_WEEK_DATE1, "資料時間－前者", ref strMessage, out dateODT_WEEK_DATE1);
            if (boolODT_WEEK_DATE1 && dateODT_WEEK_DATE1.DayOfWeek != DayOfWeek.Wednesday)
                strMessage += "<p> 請選擇 正確日期 </p>";
            if (strMessage.Equals(string.Empty))
            {
                dateODT_WEEK_DATE2 = dateODT_WEEK_DATE1.AddDays(6);
                steDateText(dateODT_WEEK_DATE1, dateODT_WEEK_DATE2);
                dataImport();
                //txtODT_WEEK_DATE2.Text = dateODT_WEEK_DATE1.AddDays(6).ToString(strDateFormat);
                //string strSQL = Path.GetFileName(Request.Path);
                //Response.Redirect(strSQL + getQueryString());
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void txtODT_WEEK_DATE2_TextChanged(object sender, EventArgs e)
        {
            string strMessage = "";
            string strODT_WEEK_DATE2 = txtODT_WEEK_DATE2.Text;
            DateTime dateODT_WEEK_DATE1, dateODT_WEEK_DATE2;
            if (strODT_WEEK_DATE2.Equals(string.Empty))
                strMessage += "<p> 請選擇 資料時間－後者 </p>";
            bool boolODT_WEEK_DATE2 = FCommon.checkDateTime(txtODT_WEEK_DATE2.Text, "資料時間－後者", ref strMessage, out dateODT_WEEK_DATE2);
            if (boolODT_WEEK_DATE2 && dateODT_WEEK_DATE2.DayOfWeek != DayOfWeek.Tuesday)
                strMessage += "<p> 請選擇 正確日期 </p>";
            if (strMessage.Equals(string.Empty))
            {
                dateODT_WEEK_DATE1 = dateODT_WEEK_DATE2.AddDays(-6);
                steDateText(dateODT_WEEK_DATE1, dateODT_WEEK_DATE2);
                dataImport();
                //txtODT_WEEK_DATE1.Text = dateODT_WEEK_DATE2.AddDays(-6).ToString(strDateFormat);
                //string strSQL = Path.GetFileName(Request.Path);
                //Response.Redirect(strSQL + getQueryString());
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void txtOVC_BLD_NO_TextChanged(object sender, EventArgs e)
        {
            txtOVC_BLD_NO.Text = txtOVC_BLD_NO.Text.ToUpper();   //轉大寫
            //string strSQL = Path.GetFileName(Request.Path);
            //Response.Redirect(strSQL + getQueryString());
        }

        protected void GV_WRP_BLD_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}