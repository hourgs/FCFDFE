using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;
using System.Data.Entity;

namespace FCFDFE.pages.MTS.H
{
    public partial class MTS_H12 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        string strDEPT_SN, strDEPT_Name;
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        string[] aryAreaList = { "基隆地區", "桃園地區", "高雄分遣組" };
        int intAuth = 0; //是否為物資接轉處長官(2)or地區長官(1)or無權限(0)
        string strDateFormatSQL = "'yyyy-mm-dd'";

        #region 副程式
        protected void dataImport()
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            //string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
            string strOVC_SECTION = lblOVC_SECTION.Text;
            DateTime dateNow = DateTime.Now;
            #region 錯誤訊息
            if (strOVC_SECTION.Equals(string.Empty))
                strMessage += "<p> 接轉地區不存在！ </p>";
            getDate(out DateTime dateStart, out DateTime dateEnd, out DateTime dateLastMonth, ref strMessage);
            #endregion
            string strDateStart = FCommon.getDateTime(dateStart);
            string strDateEnd = FCommon.getDateTime(dateEnd);
            string strDateLastMonth = FCommon.getDateTime(dateLastMonth);

            if (strMessage.Equals(string.Empty))
            {
                bool isNotClose = false;
                string[] strParameterName_start = { ":section", ":start_date" };
                ArrayList aryData_start = new ArrayList();
                aryData_start.Add(strOVC_SECTION);
                aryData_start.Add(strDateStart);

                #region 確認是否未關帳
                if (intAuth == 1)
                {
                    if (!getIsPrevMonthClosedtExist(strOVC_SECTION, strDateLastMonth))
                    {
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "前月未關帳！");
                        isNotClose = true;
                    }
                }
                else
                {
                    string strMessage_Section;
                    if (!getIsAllClosedtExist(strDateStart, out strMessage_Section))
                    {
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"本月尚有{ strMessage_Section }接轉組未關帳！");
                        isNotClose = true;
                    }
                    else if (!getIsLogItemExist(strOVC_SECTION, strDateStart))
                    {
                        try
                        {
                            //新增採購中心之 TBGMT_MRP_ITEM_LOG
                            //執行預存程序 mrp_Item_Log_summarize 帶 mrp_date=strDateStart
                            System.Data.Entity.Core.Objects.ObjectParameter rTN_MSG =
                                new System.Data.Entity.Core.Objects.ObjectParameter("rTN_MSG", typeof(string));
                            MTSE.MRP_ITEM_LOG_SUMMARIZE(strDateStart, rTN_MSG);
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "採購室 細項Log 產生成功。");
                        }
                        catch
                        {
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "採購室 細項Log 產生失敗！");
                        }
                    }
                }
                #endregion

                if (!isNotClose) //沒有未關帳之資料
                {
                    pnData.Visible = true;

                    //預先將列印做到預警
                    if (intAuth == 1 && !getIsClosedtExist(strOVC_SECTION, strDateStart))
                        FCommon.Controls_Attributes("onClick", "if (window.confirm('本月第一次列印,會即刻關帳!是否關帳後列印？')) return true; else  return false;", btnPrint);
                    else
                        FCommon.Controls_Attributes("onClick", btnPrint);

                    string strWhere_start = $@"
                        where OVC_SECTION = { strParameterName_start[0] }
                        and ODT_MONTH_DATE = to_date({ strParameterName_start[1] }, { strDateFormatSQL })
                    ";

                    #region 月報表 TBGMT_MRP
                    string strONB_SHIP_AIR = "", strONB_SHIP_SEA = "", strODT_OPEN_DATE = "", strODT_DOC_DATE = "", strOVC_DOC_NO = "", strOVC_NOTE = "";//, CLOSE_DT; 
                    if (getIsExist(strOVC_SECTION, strDateStart, strWhere_start, out DataTable dt)) //當月報表存在，直接顯示
                    {
                        DataRow dr = dt.Rows[0];
                        strONB_SHIP_AIR = dr["ONB_SHIP_AIR"].ToString();
                        strONB_SHIP_SEA = dr["ONB_SHIP_SEA"].ToString();
                        strODT_OPEN_DATE = FCommon.getDateTime(dr["ODT_OPEN_DATE"]);
                        strODT_DOC_DATE = FCommon.getDateTime(dr["ODT_DOC_DATE"]);
                        strOVC_DOC_NO = dr["OVC_DOC_NO"].ToString();
                        strOVC_NOTE = dr["OVC_NOTE"].ToString();

                        //顯示新統計之航架次
                        decimal decONB_SHIP_AIR, decONB_SHIP_SEA;
                        getSHIP(strOVC_SECTION, strDateStart, strDateEnd, out strONB_SHIP_AIR, out strONB_SHIP_SEA);
                        if (decimal.TryParse(strONB_SHIP_AIR, out decONB_SHIP_AIR)) strONB_SHIP_AIR = decONB_SHIP_AIR.ToString();
                        if (decimal.TryParse(strONB_SHIP_SEA, out decONB_SHIP_SEA)) strONB_SHIP_SEA = decONB_SHIP_SEA.ToString();
                    }
                    else
                    {
                        TBGMT_MRP mrp = statistic();
                        if (mrp != null)
                        {
                            strONB_SHIP_AIR = mrp.ONB_SHIP_AIR.ToString();
                            strONB_SHIP_SEA = mrp.ONB_SHIP_SEA.ToString();
                            strODT_OPEN_DATE = FCommon.getDateTime(mrp.ODT_OPEN_DATE);
                            strODT_DOC_DATE = FCommon.getDateTime(mrp.ODT_DOC_DATE);
                            strOVC_DOC_NO = mrp.OVC_DOC_NO;
                            strOVC_NOTE = mrp.OVC_NOTE;
                        }
                    }
                    txtONB_SHIP_AIR.Text = strONB_SHIP_AIR;
                    txtONB_SHIP_SEA.Text = strONB_SHIP_SEA;
                    txtODT_OPEN_DATE.Text = strODT_OPEN_DATE;
                    txtODT_DOC_DATE.Text = strODT_DOC_DATE;
                    txtOVC_DOC_NO.Text = strOVC_DOC_NO;
                    txtOVC_NOTE.Text = strOVC_NOTE;
                    #endregion

                    #region 細項 MRP_ITEM
                    setDeleteAllItem(strOVC_SECTION, dateStart); //先刪除目前之細項

                    if (strOVC_SECTION.Equals("採購室"))
                    {
                        try
                        {
                            //新增採購中心之 TBGMT_MRP_ITEM_LOG
                            //執行預存程序 mrp_Item_Log_summarize 帶 mrp_date=strDateStart
                            System.Data.Entity.Core.Objects.ObjectParameter rTN_MSG =
                                new System.Data.Entity.Core.Objects.ObjectParameter("rTN_MSG", typeof(string));
                            MTSE.MRP_ITEM_LOG_SUMMARIZE(strDateStart, rTN_MSG);
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "採購室 細項Log 產生成功。");
                        }
                        catch
                        {
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "採購室 細項Log 產生失敗！");
                        }
                    }
                    else
                    {
                        if (!getIsAnyItemExist(strOVC_SECTION, strDateStart, strWhere_start)) //沒有ITEM 資料
                        {
                            setGetDummyItems_CreateItems(strOVC_SECTION, dateStart, dateEnd); //給予虛擬資料
                        }
                        if (!getIsClosedtExist(strOVC_SECTION, strDateStart)) //當期尚未關帳
                        {
                            try
                            {
                                if (getIsPrevMonthClosedtExist(strOVC_SECTION, strDateLastMonth)) //Zoe新增條件，阻擋因前期未關帳，產生之預存錯誤訊息
                                {
                                    //MakeDiffData 產生差異資料
                                    //執行預存程序 Mrp_Item_Difference2 帶 section=strOVC_SECTION, mrp_date=strDateStart
                                    System.Data.Entity.Core.Objects.ObjectParameter rTN_MSG =
                                    new System.Data.Entity.Core.Objects.ObjectParameter("rTN_MSG", typeof(string));
                                    MTSE.MRP_ITEM_DIFFERENCE2(strOVC_SECTION, strDateStart, rTN_MSG);
                                    string myKey = rTN_MSG.Value.ToString();
                                    if (myKey.Equals("SUCCESS"))
                                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "產生差異資料 成功。");
                                    else
                                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "<p> 產生差異資料 失敗！ </p>" + myKey);
                                }
                                else
                                    FCommon.AlertShow(PnMessage, "warning", "系統訊息", "前月未關帳，未產生差異資料。");
                            }
                            catch
                            {
                                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "產生差異資料 錯誤！");
                            }
                        }
                    }
                    #endregion

                    string strTable1, strTable2;
                    if (intAuth == 1)
                    {
                        if (!getIsClosedtExist(strOVC_SECTION, strDateStart))
                        {
                            strTable1 = "TBGMT_MRP_ITEM";
                            strTable2 = "TBGMT_MRP_ITEM_TEMP_DIFF";
                        }
                        else
                        {
                            strTable1 = "TBGMT_MRP_ITEM_LOG";
                            strTable2 = "TBGMT_MRP_ITEM_DIFF";
                        }
                    }
                    else
                    {
                        strTable1 = "TBGMT_MRP_ITEM_LOG";
                        strTable2 = "TBGMT_MRP_ITEM_DIFF";
                    }

                    //顯示詳細項目
                    DataTable dt_MRP_ITEM = dataImport_GV_TBGMT_MRP_ITEM(strTable1, strTable2);
                    ViewState["GV_TBGMT_MRP_ITEM"] = dt_MRP_ITEM;
                    drpOdtYear.Enabled = false;
                    drpOdtMonth.Enabled = false;
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "查詢成功");
                }

                #region 原始查詢
                //if (intAuth == 1)
                //{
                //    var mrpTemp_other =
                //        from mrp in MTSE.TBGMT_MRP
                //        where mrp.OVC_SECTION == "基隆地區" || mrp.OVC_SECTION == "桃園地區" || mrp.OVC_SECTION == "高雄分遣組"
                //        select mrp;
                //    var mrpTemp_center =
                //        from mrp in MTSE.TBGMT_MRP
                //        where mrp.OVC_SECTION == "採購中心"
                //        select mrp;
                //    //檢測TBGMT_MRP是否有該年月與前一個月的基隆地區、桃園地區、高雄分遣組地區月報表檔資料
                //    var mrp_query = mrpTemp_other.Where(table => table.ODT_MONTH_DATE == dateStart).Count();
                //    if (mrp_query > 0)
                //    {
                //        var mrp_query_last = mrpTemp_other.Where(table => table.ODT_MONTH_DATE == dateLastMonth).Count();
                //        if (mrp_query_last > 0)
                //        {
                //            var query =
                //                from mrp in mrpTemp_center
                //                where mrp.ODT_MONTH_DATE >= dateStart && mrp.ODT_MONTH_DATE <= dateEnd
                //                orderby mrp.ODT_MONTH_DATE
                //                select new
                //                {
                //                    ONB_SHIP_AIR = mrp.ONB_SHIP_AIR,
                //                    ONB_SHIP_SEA = mrp.ONB_SHIP_SEA,
                //                    ODT_OPEN_DATE = mrp.ODT_OPEN_DATE,
                //                    ODT_DOC_DATE = mrp.ODT_DOC_DATE,
                //                    OVC_DOC_NO = mrp.OVC_DOC_NO,
                //                    OVC_NOTE = mrp.OVC_NOTE
                //                };
                //            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                //            if (dt.Rows.Count > 0)
                //            {
                //                DataRow dr = dt.Rows[0];
                //                txtONB_SHIP_AIR.Text = dr["ONB_SHIP_AIR"].ToString();
                //                txtONB_SHIP_SEA.Text = dr["ONB_SHIP_SEA"].ToString();
                //                txtODT_OPEN_DATE.Text = dr["ODT_OPEN_DATE"].ToString();
                //                txtODT_DOC_DATE.Text = dr["ODT_DOC_DATE"].ToString();
                //                txtOVC_DOC_NO.Text = dr["OVC_DOC_NO"].ToString();
                //                lstOVC_NOTE.Items.Add(dr["OVC_NOTE"].ToString());
                //            }

                //            var query2 =
                //                from a in
                //                    (from mrp_item in MTSE.TBGMT_MRP_ITEM
                //                     where mrp_item.ODT_MONTH_DATE == dateStart
                //                     select mrp_item
                //                     )
                //                group a by a into m2
                //                select new
                //                {
                //                    OVC_SECTION = m2.Key.OVC_SECTION,
                //                    OVC_MILITARY = m2.Key.OVC_MILITARY,
                //                    OVC_IE = m2.Key.OVC_IE,
                //                    ONB_QUANITY_M = m2.Sum(m => m.ONB_QUANITY_M),
                //                    ONB_WEIGHT_M = m2.Sum(m => m.ONB_WEIGHT_M),
                //                    ONB_VOLUME_M = m2.Sum(m => m.ONB_VOLUME_M),
                //                    ONB_BLD_M = m2.Sum(m => m.ONB_BLD_M),
                //                    ONB_QUANITY_C = m2.Sum(m => m.ONB_QUANITY_C),
                //                    ONB_WEIGHT_C = m2.Sum(m => m.ONB_WEIGHT_C),
                //                    ONB_VOLUME_C = m2.Sum(m => m.ONB_VOLUME_C),
                //                    ONB_BLD_C = m2.Sum(m => m.ONB_BLD_C),
                //                    ONB_QUANITY_O = m2.Sum(m => m.ONB_QUANITY_O),
                //                    ONB_WEIGHT_O = m2.Sum(m => m.ONB_WEIGHT_O),
                //                    ONB_VOLUME_O = m2.Sum(m => m.ONB_VOLUME_O),
                //                    ONB_BLD_O = m2.Sum(m => m.ONB_BLD_O)
                //                };

                //            var query3 = from temp_diff in MTSE.TBGMT_MRP_ITEM_TEMP_DIFF
                //                         where temp_diff.ODT_MONTH_DATE == dateStart
                //                         select new
                //                         {
                //                             OVC_SECTION = temp_diff.OVC_SECTION,
                //                             OVC_MILITARY = temp_diff.OVC_MILITARY,
                //                             OVC_IE = temp_diff.OVC_IE,
                //                             ONB_QUANITY_M = temp_diff.ONB_QUANITY_M,
                //                             ONB_WEIGHT_M = temp_diff.ONB_WEIGHT_M,
                //                             ONB_VOLUME_M = temp_diff.ONB_VOLUME_M,
                //                             ONB_BLD_M = temp_diff.ONB_BLD_M,
                //                             ONB_QUANITY_C = temp_diff.ONB_QUANITY_C,
                //                             ONB_WEIGHT_C = temp_diff.ONB_WEIGHT_C,
                //                             ONB_VOLUME_C = temp_diff.ONB_VOLUME_C,
                //                             ONB_BLD_C = temp_diff.ONB_BLD_C,
                //                             ONB_QUANITY_O = temp_diff.ONB_QUANITY_O,
                //                             ONB_WEIGHT_O = temp_diff.ONB_WEIGHT_O,
                //                             ONB_VOLUME_O = temp_diff.ONB_VOLUME_O,
                //                             ONB_BLD_O = temp_diff.ONB_BLD_O
                //                         };

                //            var un = query2.Concat(query3).ToList();
                //            var q = un.GroupBy(cc =>
                //                new
                //                {
                //                    cc.OVC_SECTION,
                //                    cc.OVC_MILITARY,
                //                    cc.OVC_IE,
                //                }
                //                ).Select(dd =>
                //                new
                //                {
                //                    OVC_SECTION = dd.Key.OVC_SECTION,
                //                    OVC_MILITARY = dd.Key.OVC_MILITARY,
                //                    OVC_IE = dd.Key.OVC_IE,
                //                    ONB_QUANITY_M = string.Join("", dd.Select(ee => ee.ONB_QUANITY_M.ToString()).ToList()),
                //                    ONB_WEIGHT_M = string.Join("", dd.Select(ee => ee.ONB_WEIGHT_M.ToString()).ToList()),
                //                    ONB_VOLUME_M = string.Join("", dd.Select(ee => ee.ONB_VOLUME_M.ToString()).ToList()),
                //                    ONB_BLD_M = string.Join("", dd.Select(ee => ee.ONB_BLD_M.ToString()).ToList()),
                //                    ONB_QUANITY_C = string.Join("", dd.Select(ee => ee.ONB_QUANITY_C.ToString()).ToList()),
                //                    ONB_WEIGHT_C = string.Join("", dd.Select(ee => ee.ONB_WEIGHT_C.ToString()).ToList()),
                //                    ONB_VOLUME_C = string.Join("", dd.Select(ee => ee.ONB_VOLUME_C.ToString()).ToList()),
                //                    ONB_BLD_C = string.Join("", dd.Select(ee => ee.ONB_BLD_C.ToString()).ToList()),
                //                    ONB_QUANITY_O = string.Join("", dd.Select(ee => ee.ONB_QUANITY_O.ToString()).ToList()),
                //                    ONB_WEIGHT_O = string.Join("", dd.Select(ee => ee.ONB_WEIGHT_O.ToString()).ToList()),
                //                    ONB_VOLUME_O = string.Join("", dd.Select(ee => ee.ONB_VOLUME_O.ToString()).ToList()),
                //                    ONB_BLD_O = string.Join("", dd.Select(ee => ee.ONB_BLD_O.ToString()).ToList()),
                //                }).OrderByDescending(z => z.OVC_IE).OrderBy(u => u.OVC_MILITARY);
                //            DataTable dt2 = CommonStatic.LinqQueryToDataTable(q);
                //            ViewState["GV_TBGMT_MRP_ITEM"] = dt2;
                //            ViewState["hasRows"] = FCommon.GridView_dataImport(GV__TBGMT_MRP_ITEM, dt2);
                //            lblImessage.Text = "查詢成功";
                //        }
                //    }
                //    else
                //    {
                //        //string[] strParameterName = { ":start_date", ":end_date", "section" };
                //        //ArrayList aryData = new ArrayList();
                //        //aryData.Add(strDateStart);
                //        //aryData.Add(strDateEnd);
                //        //aryData.Add(strOVC_SECTION);
                //        string sql = $@"select a.OVC_CLASS_NAME AS OVC_MILITARY,  '進口' AS OVC_IE,  
                //                  ONB_BLD_M, ONB_QUANITY_M, ONB_WEIGHT_M, ONB_VOLUME_M,                
                //                  ONB_BLD_C, ONB_QUANITY_C, ONB_WEIGHT_C, ONB_VOLUME_C,                
                //                  ONB_BLD_O, ONB_QUANITY_O, ONB_WEIGHT_O, ONB_VOLUME_O                
                //                   from                                                             
                //                  (select OVC_CLASS_NAME from TBGMT_DEPT_CLASS) a,            
                //                  (                                                          
                //                  -- '進口'
                //                  select OVC_MILITARY_TYPE, count(OVC_BLD_NO) as ONB_BLD_M, sum(ONB_QUANITY) as ONB_QUANITY_M, sum(ONB_WEIGHT) as ONB_WEIGHT_M, 
                //                  sum(ONB_VOLUME) as ONB_VOLUME_M 
                //                  from ( 
                //                  select (OVC_MILITARY_TYPE) as OVC_MILITARY_TYPE, a.OVC_BLD_NO, OVC_PURCH_NO, c.ONB_QUANITY,     
                //                  CASE WHEN OVC_WEIGHT_UNIT <> 'KG' THEN ROUND(NVL(ONB_WEIGHT,0) * 0.45359,3) ELSE ROUND(NVL(ONB_WEIGHT,0) ,3) END ONB_WEIGHT,  
                //                  CASE WHEN OVC_VOLUME_UNIT <> 'CBM' THEN ROUND(NVL(ONB_VOLUME,0) / 35.3142,3) ELSE ROUND(NVL(ONB_VOLUME,0) ,3) END ONB_VOLUME  
                //                  from TBGMT_BLD a, TBGMT_ICR b,  
                //                  (SELECT OVC_BLD_NO,ONB_ACTUAL_RECEIVE AS ONB_QUANITY FROM (
                //                  SELECT a.OVC_BLD_NO as OVC_BLD_NO,ONB_ACTUAL_RECEIVE,ONB_OVERFLOW,ONB_LESS, ONB_BROKEN 
                //                  FROM TBGMT_IRD a, TBGMT_BLD b WHERE a.OVC_BLD_NO = b.OVC_BLD_NO)) c 
                //                  where a.OVC_BLD_NO = b.OVC_BLD_NO AND a.OVC_BLD_NO = c.OVC_BLD_NO 

                //                  AND ODT_TRANSFER_DATE BETWEEN { strParameterName[0] } AND { strParameterName[1] }
                //                  AND(OVC_PURCH_NO like '___' or OVC_PURCH_NO like '___ %')
                //                  ) group by OVC_MILITARY_TYPE
                //                  ) x,        
                //                  (
                //                  --'進口'
                //                  select OVC_MILITARY_TYPE, count(OVC_BLD_NO) as ONB_BLD_C, sum(ONB_QUANITY) as ONB_QUANITY_C, 
                //                  sum(ONB_WEIGHT) as ONB_WEIGHT_C, sum(ONB_VOLUME) as ONB_VOLUME_C from(
                //                  select(OVC_MILITARY_TYPE) as OVC_MILITARY_TYPE, a.OVC_BLD_NO, OVC_PURCH_NO, c.ONB_QUANITY,
                //                  CASE WHEN OVC_WEIGHT_UNIT <> 'KG' THEN ROUND(NVL(ONB_WEIGHT, 0) * 0.45359, 3) ELSE ROUND(NVL(ONB_WEIGHT, 0), 3) END ONB_WEIGHT,
                //                  CASE WHEN OVC_VOLUME_UNIT <> 'CBM' THEN ROUND(NVL(ONB_VOLUME, 0) / 35.3142, 3) ELSE ROUND(NVL(ONB_VOLUME, 0), 3) END ONB_VOLUME
                //                  from TBGMT_BLD a, TBGMT_ICR b,
                //                  (SELECT OVC_BLD_NO, ONB_ACTUAL_RECEIVE AS ONB_QUANITY FROM(SELECT a.OVC_BLD_NO as OVC_BLD_NO, ONB_ACTUAL_RECEIVE,
                //                  ONB_OVERFLOW, ONB_LESS, ONB_BROKEN FROM TBGMT_IRD a, TBGMT_BLD b WHERE a.OVC_BLD_NO = b.OVC_BLD_NO)) c
                //                   where a.OVC_BLD_NO = b.OVC_BLD_NO AND a.OVC_BLD_NO = c.OVC_BLD_NO
                //                  AND ODT_TRANSFER_DATE BETWEEN { strParameterName[0] } AND { strParameterName[1] }
                //                  AND not(OVC_PURCH_NO like '___' or OVC_PURCH_NO like '___ %' or upper(OVC_PURCH_NO) = 'NONE' or OVC_PURCH_NO like '新光%')  
                //                  ) group by OVC_MILITARY_TYPE
                //                  ) y,      
                //                  (
                //                  --'進口'
                //                  select OVC_MILITARY_TYPE, count(OVC_BLD_NO) as ONB_BLD_O, sum(ONB_QUANITY) as ONB_QUANITY_O, sum(ONB_WEIGHT) as ONB_WEIGHT_O, 
                //                  sum(ONB_VOLUME) as ONB_VOLUME_O
                //                  from(select(OVC_MILITARY_TYPE) as OVC_MILITARY_TYPE, a.OVC_BLD_NO, OVC_PURCH_NO, c.ONB_QUANITY,
                //                  CASE WHEN OVC_WEIGHT_UNIT <> 'KG' THEN ROUND(NVL(ONB_WEIGHT, 0) * 0.45359, 3) ELSE ROUND(NVL(ONB_WEIGHT, 0), 3) END ONB_WEIGHT,
                //                  CASE WHEN OVC_VOLUME_UNIT <> 'CBM' THEN ROUND(NVL(ONB_VOLUME, 0) / 35.3142, 3) ELSE ROUND(NVL(ONB_VOLUME, 0), 3) END ONB_VOLUME
                //                  from TBGMT_BLD a, TBGMT_ICR b,
                //                  (SELECT OVC_BLD_NO, ONB_ACTUAL_RECEIVE AS ONB_QUANITY FROM(SELECT a.OVC_BLD_NO as OVC_BLD_NO, ONB_ACTUAL_RECEIVE, ONB_OVERFLOW,
                //                  ONB_LESS, ONB_BROKEN FROM TBGMT_IRD a, TBGMT_BLD b WHERE a.OVC_BLD_NO = b.OVC_BLD_NO)) c
                //                  where a.OVC_BLD_NO = b.OVC_BLD_NO
                //                  AND a.OVC_BLD_NO = c.OVC_BLD_NO
                //                  AND ODT_TRANSFER_DATE BETWEEN { strParameterName[0] } AND { strParameterName[1] }
                //                  AND(upper(OVC_PURCH_NO) = 'NONE' or OVC_PURCH_NO like '新光%')
                //                  ) group by OVC_MILITARY_TYPE
                //                  ) z
                //                  where a.OVC_CLASS_NAME = x.OVC_MILITARY_TYPE
                //                  and a.OVC_CLASS_NAME = y.OVC_MILITARY_TYPE
                //                  and a.OVC_CLASS_NAME = z.OVC_MILITARY_TYPE
                //                  --'出口'
                //                  UNION
                //                  --select '{ strParameterName[2] }' AS OVC_SECTION, { strParameterName[0] } AS ODT_MONTH_DATE,
                //                  select a.OVC_CLASS_NAME AS OVC_MILITARY, '出口' AS OVC_IE,
                //                  ONB_BLD_M, ONB_QUANITY_M, ONB_WEIGHT_M, ONB_VOLUME_M,
                //                  ONB_BLD_C, ONB_QUANITY_C, ONB_WEIGHT_C, ONB_VOLUME_C,
                //                  ONB_BLD_O, ONB_QUANITY_O, ONB_WEIGHT_O, ONB_VOLUME_O
                //                  from
                //                  (select OVC_CLASS_NAME from TBGMT_DEPT_CLASS) a,             
                //                  (
                //                  select OVC_MILITARY_TYPE, count(OVC_BLD_NO) as ONB_BLD_M, sum(ONB_QUANITY) as ONB_QUANITY_M, sum(ONB_WEIGHT) as ONB_WEIGHT_M,
                //                  sum(ONB_VOLUME) as ONB_VOLUME_M
                //                  from(
                //                  select(OVC_MILITARY_TYPE) as OVC_MILITARY_TYPE, a.OVC_BLD_NO, OVC_PURCH_NO, d.ONB_QUANITY,
                //                  CASE WHEN OVC_WEIGHT_UNIT <> 'KG' THEN ROUND(NVL(REAL_WEIGHT, 0) * 0.45359, 3) ELSE ROUND(NVL(REAL_WEIGHT, 0), 3) END ONB_WEIGHT,
                //                  CASE WHEN OVC_VOLUME_UNIT <> 'CBM' THEN ROUND(NVL(REAL_VOLUME, 0) / 35.3142, 3) ELSE ROUND(NVL(REAL_VOLUME, 0), 3) END ONB_VOLUME
                //                  from TBGMT_BLD a, TBGMT_ESO b, TBGMT_EDF c,
                //                  (SELECT OVC_EDF_NO, COUNT(OVC_BOX_NO) AS ONB_QUANITY FROM(SELECT DISTINCT OVC_EDF_NO, OVC_BOX_NO FROM TBGMT_EDF_DETAIL)
                //                  GROUP BY OVC_EDF_NO) d
                //                  where a.OVC_BLD_NO = b.OVC_BLD_NO
                //                  AND c.OVC_EDF_NO = b.OVC_EDF_NO
                //                  AND c.OVC_EDF_NO = d.OVC_EDF_NO
                //                  AND a.REAL_START_DATE BETWEEN { strParameterName[0] } AND { strParameterName[1] }
                //                  --AND
                //                  --(OVC_PURCH_NO like '___' or OVC_PURCH_NO like '___ %')
                //                  ) group by OVC_MILITARY_TYPE
                //                  ) x,            
                //                  (select OVC_MILITARY_TYPE, count(OVC_BLD_NO) as ONB_BLD_C, sum(ONB_QUANITY) as ONB_QUANITY_C, sum(ONB_WEIGHT) as ONB_WEIGHT_C,
                //                  sum(ONB_VOLUME) as ONB_VOLUME_C
                //                  from(
                //                  select(OVC_MILITARY_TYPE) as OVC_MILITARY_TYPE, a.OVC_BLD_NO, OVC_PURCH_NO, d.ONB_QUANITY,
                //                  CASE WHEN OVC_WEIGHT_UNIT <> 'KG' THEN ROUND(NVL(REAL_WEIGHT, 0) * 0.45359, 3) ELSE ROUND(NVL(REAL_WEIGHT, 0), 3) END ONB_WEIGHT,
                //                  CASE WHEN OVC_VOLUME_UNIT <> 'CBM' THEN ROUND(NVL(REAL_VOLUME, 0) / 35.3142, 3) ELSE ROUND(NVL(REAL_VOLUME, 0), 3) END ONB_VOLUME
                //                  from TBGMT_BLD a, TBGMT_ESO b, TBGMT_EDF c,
                //                  (SELECT OVC_EDF_NO, COUNT(OVC_BOX_NO) AS ONB_QUANITY
                //                  FROM(SELECT DISTINCT OVC_EDF_NO, OVC_BOX_NO
                //                  FROM TBGMT_EDF_DETAIL) GROUP BY OVC_EDF_NO) d
                //                  where a.OVC_BLD_NO = b.OVC_BLD_NO
                //                  AND c.OVC_EDF_NO = b.OVC_EDF_NO
                //                  AND c.OVC_EDF_NO = d.OVC_EDF_NO
                //                  AND a.REAL_START_DATE BETWEEN { strParameterName[0] } AND { strParameterName[1] }
                //                  --AND not(OVC_PURCH_NO  like '___' or OVC_PURCH_NO like '___ %'
                //                  --or upper(OVC_PURCH_NO) = 'NONE' or OVC_PURCH_NO like '新光%')
                //                  ) group by OVC_MILITARY_TYPE
                //                  ) y,                                     
                //                  (select OVC_MILITARY_TYPE, count(OVC_BLD_NO) as ONB_BLD_O, sum(ONB_QUANITY) as ONB_QUANITY_O, sum(ONB_WEIGHT) as ONB_WEIGHT_O,
                //                  sum(ONB_VOLUME) as ONB_VOLUME_O
                //                  from(
                //                  select(OVC_MILITARY_TYPE) as OVC_MILITARY_TYPE, a.OVC_BLD_NO, OVC_PURCH_NO, d.ONB_QUANITY,
                //                  CASE WHEN OVC_WEIGHT_UNIT <> 'KG' THEN ROUND(NVL(REAL_WEIGHT, 0) * 0.45359, 3) ELSE ROUND(NVL(REAL_WEIGHT, 0), 3) END ONB_WEIGHT,
                //                  CASE WHEN OVC_VOLUME_UNIT <> 'CBM' THEN ROUND(NVL(REAL_VOLUME, 0) / 35.3142, 3) ELSE ROUND(NVL(REAL_VOLUME, 0), 3) END ONB_VOLUME
                //                  from TBGMT_BLD a, TBGMT_ESO b, TBGMT_EDF c,
                //                  (SELECT OVC_EDF_NO, COUNT(OVC_BOX_NO) AS ONB_QUANITY
                //                  FROM(SELECT DISTINCT OVC_EDF_NO, OVC_BOX_NO
                //                  FROM TBGMT_EDF_DETAIL) GROUP BY OVC_EDF_NO) d
                //                  where a.OVC_BLD_NO = b.OVC_BLD_NO
                //                  AND c.OVC_EDF_NO = b.OVC_EDF_NO
                //                  AND c.OVC_EDF_NO = d.OVC_EDF_NO
                //                  AND a.REAL_START_DATE BETWEEN { strParameterName[0] } AND { strParameterName[1] }
                //                  --AND upper(OVC_PURCH_NO) = 'NONE')  
                //                  ) group by OVC_MILITARY_TYPE
                //                  UNION
                //                  select OVC_MILITARY_TYPE, count(OVC_BLD_NO) as ONB_BLD_O, sum(ONB_QUANITY) as ONB_QUANITY_O, sum(ONB_WEIGHT) as ONB_WEIGHT_O, sum(ONB_VOLUME) as ONB_VOLUME_O
                //                  from(
                //                  select(OVC_MILITARY_TYPE) as OVC_MILITARY_TYPE, a.OVC_BLD_NO, OVC_PURCH_NO, a.ONB_QUANITY,
                //                  CASE WHEN OVC_WEIGHT_UNIT <> 'KG' THEN ROUND(NVL(REAL_WEIGHT, 0) * 0.45359, 3) ELSE ROUND(NVL(REAL_WEIGHT, 0), 3) END ONB_WEIGHT,
                //                  CASE WHEN OVC_VOLUME_UNIT <> 'CBM' THEN ROUND(NVL(REAL_VOLUME, 0) / 35.3142, 3) ELSE ROUND(NVL(REAL_VOLUME, 0), 3) END ONB_VOLUME
                //                  from TBGMT_BLD a, TBGMT_ESO b, TBGMT_EDF c
                //                  --(SELECT OVC_EDF_NO, COUNT(OVC_BOX_NO) AS ONB_QUANITY FROM(SELECT DISTINCT OVC_EDF_NO, OVC_BOX_NO FROM TBGMT_EDF_DETAIL) GROUP BY OVC_EDF_NO) d
                //                  where a.OVC_BLD_NO = b.OVC_BLD_NO
                //                  AND c.OVC_EDF_NO = b.OVC_EDF_NO
                //                  AND  a.REAL_START_DATE BETWEEN { strParameterName[0] } AND { strParameterName[1] }
                //                  --AND(OVC_PURCH_NO like '新光%')
                //                  ) group by OVC_MILITARY_TYPE
                //                  ) z
                //                  where a.OVC_CLASS_NAME = x.OVC_MILITARY_TYPE
                //                  and a.OVC_CLASS_NAME = y.OVC_MILITARY_TYPE
                //                  and a.OVC_CLASS_NAME = z.OVC_MILITARY_TYPE ";

                //        DataTable dt_Source = FCommon.getDataTableFromSelect(sql, strParameterName, aryData);

                //        try
                //        {
                //            DataRow dr_Source = dt_Source.Rows[0];
                //            TBGMT_MRP_ITEM mrp_item = new TBGMT_MRP_ITEM();
                //            TBGMT_MRP_ITEM_LOG mrp_item_log = new TBGMT_MRP_ITEM_LOG();
                //            TBGMT_MRP_ITEM_DIFF mrp_item_diff = new TBGMT_MRP_ITEM_DIFF();

                //            mrp_item.OVC_SECTION = strOVC_SECTION;
                //            mrp_item_log.OVC_SECTION = strOVC_SECTION;
                //            mrp_item_diff.OVC_SECTION = strOVC_SECTION;

                //            mrp_item.ODT_MONTH_DATE = dateStart;
                //            mrp_item_log.ODT_MONTH_DATE = dateStart;
                //            mrp_item_diff.ODT_MONTH_DATE = dateStart;

                //            mrp_item.OVC_MILITARY = dr_Source[0].ToString();
                //            mrp_item_log.OVC_MILITARY = dr_Source[0].ToString();
                //            mrp_item_diff.OVC_MILITARY = dr_Source[0].ToString();

                //            mrp_item.OVC_IE = dr_Source[1].ToString();
                //            mrp_item_log.OVC_IE = dr_Source[1].ToString();
                //            mrp_item_diff.OVC_IE = dr_Source[1].ToString();

                //            mrp_item.ONB_BLD_M = Convert.ToDecimal(dr_Source[2]);
                //            mrp_item_log.ONB_BLD_M = Convert.ToDecimal(dr_Source[2]);
                //            mrp_item_diff.ONB_BLD_M = Convert.ToDecimal(dr_Source[2]);

                //            mrp_item.ONB_QUANITY_M = Convert.ToDecimal(dr_Source[3]);
                //            mrp_item_log.ONB_QUANITY_M = Convert.ToDecimal(dr_Source[3]);
                //            mrp_item_diff.ONB_QUANITY_M = Convert.ToDecimal(dr_Source[3]);

                //            mrp_item.ONB_WEIGHT_M = Convert.ToDecimal(dr_Source[4]);
                //            mrp_item_log.ONB_WEIGHT_M = Convert.ToDecimal(dr_Source[4]);
                //            mrp_item_diff.ONB_WEIGHT_M = Convert.ToDecimal(dr_Source[4]);

                //            mrp_item.ONB_VOLUME_M = Convert.ToDecimal(dr_Source[5]);
                //            mrp_item_log.ONB_VOLUME_M = Convert.ToDecimal(dr_Source[5]);
                //            mrp_item_diff.ONB_VOLUME_M = Convert.ToDecimal(dr_Source[5]);

                //            mrp_item.ONB_BLD_C = Convert.ToDecimal(dr_Source[6]);
                //            mrp_item_log.ONB_BLD_C = Convert.ToDecimal(dr_Source[6]);
                //            mrp_item_diff.ONB_BLD_C = Convert.ToDecimal(dr_Source[6]);

                //            mrp_item.ONB_QUANITY_C = Convert.ToDecimal(dr_Source[7]);
                //            mrp_item_log.ONB_QUANITY_C = Convert.ToDecimal(dr_Source[7]);
                //            mrp_item_diff.ONB_QUANITY_C = Convert.ToDecimal(dr_Source[7]);

                //            mrp_item.ONB_WEIGHT_C = Convert.ToDecimal(dr_Source[8]);
                //            mrp_item_log.ONB_WEIGHT_C = Convert.ToDecimal(dr_Source[8]);
                //            mrp_item_diff.ONB_WEIGHT_C = Convert.ToDecimal(dr_Source[8]);

                //            mrp_item.ONB_VOLUME_C = Convert.ToDecimal(dr_Source[9]);
                //            mrp_item_log.ONB_VOLUME_C = Convert.ToDecimal(dr_Source[9]);
                //            mrp_item_diff.ONB_VOLUME_C = Convert.ToDecimal(dr_Source[9]);

                //            mrp_item.ONB_BLD_O = Convert.ToDecimal(dr_Source[10]);
                //            mrp_item_log.ONB_BLD_O = Convert.ToDecimal(dr_Source[10]);
                //            mrp_item_diff.ONB_BLD_O = Convert.ToDecimal(dr_Source[10]);

                //            mrp_item.ONB_QUANITY_O = Convert.ToDecimal(dr_Source[11]);
                //            mrp_item_log.ONB_QUANITY_O = Convert.ToDecimal(dr_Source[11]);
                //            mrp_item_diff.ONB_QUANITY_O = Convert.ToDecimal(dr_Source[11]);

                //            mrp_item.ONB_WEIGHT_O = Convert.ToDecimal(dr_Source[12]);
                //            mrp_item_log.ONB_WEIGHT_O = Convert.ToDecimal(dr_Source[12]);
                //            mrp_item_diff.ONB_WEIGHT_O = Convert.ToDecimal(dr_Source[12]);

                //            mrp_item.ONB_VOLUME_O = Convert.ToDecimal(dr_Source[13]);
                //            mrp_item_log.ONB_VOLUME_O = Convert.ToDecimal(dr_Source[13]);
                //            mrp_item_diff.ONB_VOLUME_O = Convert.ToDecimal(dr_Source[13]);

                //            MTSE.TBGMT_MRP_ITEM.Add(mrp_item);
                //            MTSE.TBGMT_MRP_ITEM_LOG.Add(mrp_item_log);
                //            MTSE.TBGMT_MRP_ITEM_DIFF.Add(mrp_item_diff);
                //            MTSE.SaveChanges();
                //        }
                //        catch
                //        {
                //            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "新增 接轉作業月報表項目 錯誤");
                //        }
                //    }
                //}
                //else if (intAuth == 2)
                //{
                //    var mrp_query_keelung = (from mrp in MTSE.TBGMT_MRP
                //                             where mrp.OVC_SECTION == "基隆地區"
                //                             where mrp.ODT_MONTH_DATE == dateStart
                //                             select mrp).Count();
                //    var mrp_query_taoyuan = (from mrp in MTSE.TBGMT_MRP
                //                             where mrp.OVC_SECTION == "桃園地區"
                //                             where mrp.ODT_MONTH_DATE == dateStart
                //                             select mrp).Count();
                //    var mrp_query_kaohsiung = (from mrp in MTSE.TBGMT_MRP
                //                               where mrp.OVC_SECTION == "桃園地區"
                //                               where mrp.ODT_MONTH_DATE == dateStart
                //                               select mrp).Count();
                //    if (mrp_query_keelung > 0 && mrp_query_taoyuan > 0 && mrp_query_kaohsiung > 0)
                //    {
                //        var query = from mrp in MTSE.TBGMT_MRP
                //                    where mrp.ODT_MONTH_DATE >= dateStart && mrp.ODT_MONTH_DATE <= dateEnd
                //                    where mrp.OVC_SECTION == "採購中心"
                //                    orderby mrp.ODT_MONTH_DATE
                //                    select new
                //                    {
                //                        ONB_SHIP_AIR = mrp.ONB_SHIP_AIR,
                //                        ONB_SHIP_SEA = mrp.ONB_SHIP_SEA,
                //                        ODT_OPEN_DATE = mrp.ODT_OPEN_DATE,
                //                        ODT_DOC_DATE = mrp.ODT_DOC_DATE,
                //                        OVC_DOC_NO = mrp.OVC_DOC_NO,
                //                        OVC_NOTE = mrp.OVC_NOTE
                //                    };
                //        DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                //        if (dt.Rows.Count > 0)
                //        {
                //            DataRow dr = dt.Rows[0];
                //            txtONB_SHIP_AIR.Text = dr["ONB_SHIP_AIR"].ToString();
                //            txtONB_SHIP_SEA.Text = dr["ONB_SHIP_SEA"].ToString();
                //            txtODT_OPEN_DATE.Text = dr["ODT_OPEN_DATE"].ToString();
                //            txtODT_DOC_DATE.Text = dr["ODT_DOC_DATE"].ToString();
                //            txtOVC_DOC_NO.Text = dr["OVC_DOC_NO"].ToString();
                //            lstOVC_NOTE.Items.Add(dr["OVC_NOTE"].ToString());
                //        }

                //        var query2 = from a in
                //             (from mrp_item in MTSE.TBGMT_MRP_ITEM
                //              where mrp_item.ODT_MONTH_DATE == dateStart
                //              select mrp_item
                //                      )
                //                     group a by a into m2
                //                     select new
                //                     {
                //                         OVC_SECTION = m2.Key.OVC_SECTION,
                //                         OVC_MILITARY = m2.Key.OVC_MILITARY,
                //                         OVC_IE = m2.Key.OVC_IE,
                //                         ONB_QUANITY_M = m2.Sum(m => m.ONB_QUANITY_M),
                //                         ONB_WEIGHT_M = m2.Sum(m => m.ONB_WEIGHT_M),
                //                         ONB_VOLUME_M = m2.Sum(m => m.ONB_VOLUME_M),
                //                         ONB_BLD_M = m2.Sum(m => m.ONB_BLD_M),
                //                         ONB_QUANITY_C = m2.Sum(m => m.ONB_QUANITY_C),
                //                         ONB_WEIGHT_C = m2.Sum(m => m.ONB_WEIGHT_C),
                //                         ONB_VOLUME_C = m2.Sum(m => m.ONB_VOLUME_C),
                //                         ONB_BLD_C = m2.Sum(m => m.ONB_BLD_C),
                //                         ONB_QUANITY_O = m2.Sum(m => m.ONB_QUANITY_O),
                //                         ONB_WEIGHT_O = m2.Sum(m => m.ONB_WEIGHT_O),
                //                         ONB_VOLUME_O = m2.Sum(m => m.ONB_VOLUME_O),
                //                         ONB_BLD_O = m2.Sum(m => m.ONB_BLD_O)
                //                     };

                //        var query3 = from temp_diff in MTSE.TBGMT_MRP_ITEM_TEMP_DIFF
                //                     where temp_diff.ODT_MONTH_DATE == dateStart
                //                     select new
                //                     {
                //                         OVC_SECTION = temp_diff.OVC_SECTION,
                //                         OVC_MILITARY = temp_diff.OVC_MILITARY,
                //                         OVC_IE = temp_diff.OVC_IE,
                //                         ONB_QUANITY_M = temp_diff.ONB_QUANITY_M,
                //                         ONB_WEIGHT_M = temp_diff.ONB_WEIGHT_M,
                //                         ONB_VOLUME_M = temp_diff.ONB_VOLUME_M,
                //                         ONB_BLD_M = temp_diff.ONB_BLD_M,
                //                         ONB_QUANITY_C = temp_diff.ONB_QUANITY_C,
                //                         ONB_WEIGHT_C = temp_diff.ONB_WEIGHT_C,
                //                         ONB_VOLUME_C = temp_diff.ONB_VOLUME_C,
                //                         ONB_BLD_C = temp_diff.ONB_BLD_C,
                //                         ONB_QUANITY_O = temp_diff.ONB_QUANITY_O,
                //                         ONB_WEIGHT_O = temp_diff.ONB_WEIGHT_O,
                //                         ONB_VOLUME_O = temp_diff.ONB_VOLUME_O,
                //                         ONB_BLD_O = temp_diff.ONB_BLD_O
                //                     };

                //        var un = query2.Concat(query3).ToList();
                //        var q = un.GroupBy(cc =>
                //            new
                //            {
                //                cc.OVC_SECTION,
                //                cc.OVC_MILITARY,
                //                cc.OVC_IE
                //            }
                //            ).Select(dd =>
                //            new
                //            {
                //                OVC_SECTION = dd.Key.OVC_SECTION,
                //                OVC_MILITARY = dd.Key.OVC_MILITARY,
                //                OVC_IE = dd.Key.OVC_IE,
                //                ONB_QUANITY_M = string.Join("", dd.Select(ee => ee.ONB_QUANITY_M.ToString()).ToList()),
                //                ONB_WEIGHT_M = string.Join("", dd.Select(ee => ee.ONB_WEIGHT_M.ToString()).ToList()),
                //                ONB_VOLUME_M = string.Join("", dd.Select(ee => ee.ONB_VOLUME_M.ToString()).ToList()),
                //                ONB_BLD_M = string.Join("", dd.Select(ee => ee.ONB_BLD_M.ToString()).ToList()),
                //                ONB_QUANITY_C = string.Join("", dd.Select(ee => ee.ONB_QUANITY_C.ToString()).ToList()),
                //                ONB_WEIGHT_C = string.Join("", dd.Select(ee => ee.ONB_WEIGHT_C.ToString()).ToList()),
                //                ONB_VOLUME_C = string.Join("", dd.Select(ee => ee.ONB_VOLUME_C.ToString()).ToList()),
                //                ONB_BLD_C = string.Join("", dd.Select(ee => ee.ONB_BLD_C.ToString()).ToList()),
                //                ONB_QUANITY_O = string.Join("", dd.Select(ee => ee.ONB_QUANITY_O.ToString()).ToList()),
                //                ONB_WEIGHT_O = string.Join("", dd.Select(ee => ee.ONB_WEIGHT_O.ToString()).ToList()),
                //                ONB_VOLUME_O = string.Join("", dd.Select(ee => ee.ONB_VOLUME_O.ToString()).ToList()),
                //                ONB_BLD_O = string.Join("", dd.Select(ee => ee.ONB_BLD_O.ToString()).ToList()),
                //            }).OrderByDescending(z => z.OVC_IE).OrderBy(u => u.OVC_MILITARY);
                //        DataTable dt2 = CommonStatic.LinqQueryToDataTable(q);
                //        ViewState["GV_TBGMT_MRP_ITEM"] = dt2;
                //        ViewState["hasRows"] = FCommon.GridView_dataImport(GV__TBGMT_MRP_ITEM, dt2);
                //        lblImessage.Text = "查詢成功";
                //    }
                //    else
                //    {
                //        string fail = "";
                //        if (mrp_query_keelung < 0)
                //        {
                //            if (fail != "")
                //            {
                //                fail += "、基隆";
                //            }
                //            else
                //            {
                //                fail += "基隆";
                //            }
                //        }
                //        if (mrp_query_taoyuan < 0)
                //        {
                //            if (fail != "")
                //            {
                //                fail += "、桃園";
                //            }
                //            else
                //            {
                //                fail += "桃園";
                //            }

                //        }
                //        if (mrp_query_kaohsiung < 0)
                //        {
                //            if (fail != "")
                //            {
                //                fail += "、高雄";
                //            }
                //            else
                //            {
                //                fail += "高雄";
                //            }
                //        }
                //        lblImessage.Text = "尚有" + fail + "地區接轉組未關帳!!";
                //    }

                //}
                #endregion
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        private DataTable dataImport_GV_TBGMT_MRP_ITEM(string strTable1, string strTable2)
        {
            //舊系統 MRP
            //GetItems()    為 TBGMT_MRP_ITEM 及 TBGMT_MRP_ITEM_TEMP_DIFF
            //GetLogItems() 為 TBGMT_MRP_ITEM_LOG 及 TBGMT_MRP_ITEM_DIFF
            //若Table 帶 null，則取上次載入之Table，用於按鈕。
            string strMessage = "";
            string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
            DateTime dateStart, dateEnd, dateLastMonth;
            #region 錯誤訊息
            if (strOVC_SECTION.Equals(string.Empty))
                strMessage += "<p> 接轉地區不存在！ </p>";
            if (strTable1 == null)
                if (ViewState["Table1"] == null)
                    strMessage += "<p> 細項資料表錯誤1！ </p>";
                else
                    strTable1 = ViewState["Table1"].ToString();
            if (strTable2 == null)
                if (ViewState["Table2"] == null)
                    strMessage += "<p> 細項資料表錯誤2！ </p>";
                else
                    strTable2 = ViewState["Table2"].ToString();
            getDate(out dateStart, out dateEnd, out dateLastMonth, ref strMessage);
            #endregion
            string strDateStart = FCommon.getDateTime(dateStart);

            if (strMessage.Equals(string.Empty))
            {
                try
                {
                    //string strTable1 = "TBGMT_MRP_ITEM", strTable2 = "TBGMT_MRP_ITEM_TEMP_DIFF";
                    DataTable dt_MRP_ITEM = getGetItems(strOVC_SECTION, strDateStart, strTable1, strTable2);
                    getLeast_MRP_ITEM(dt_MRP_ITEM, strOVC_SECTION, strDateStart);
                    bool hasRows = FCommon.GridView_dataImport(GV_TBGMT_MRP_ITEM, dt_MRP_ITEM);
                    ViewState["hasRows"] = hasRows;
                    btnStatisticItem.Visible = hasRows;

                    //紀錄舊有Table
                    ViewState["Table1"] = strTable1;
                    ViewState["Table2"] = strTable2;
                    return dt_MRP_ITEM;
                }
                catch
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查詢細項錯誤");
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            btnStatisticItem.Visible = false;
            return null;
        }
        private TBGMT_MRP statistic() //統計並儲存
        {
            TBGMT_MRP mrp = null;
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;

            DateTime dateStart, dateEnd, dateLastMonth, dateNow = DateTime.Now;
            #region 錯誤訊息
            if (strOVC_SECTION.Equals(string.Empty))
                strMessage += "<p> 接轉地區不存在！ </p>";
            getDate(out dateStart, out dateEnd, out dateLastMonth, ref strMessage);
            #endregion
            string strDateStart = FCommon.getDateTime(dateStart);
            string strDateEnd = FCommon.getDateTime(dateEnd);
            string strDateLastMonth = FCommon.getDateTime(dateLastMonth);

            if (strMessage.Equals(string.Empty))
            {
                //取得航架次
                string strONB_SHIP_AIR, strONB_SHIP_SEA;
                decimal decONB_SHIP_AIR, decONB_SHIP_SEA;
                getSHIP(strOVC_SECTION, strDateStart, strDateEnd, out strONB_SHIP_AIR, out strONB_SHIP_SEA);
                decimal.TryParse(strONB_SHIP_AIR, out decONB_SHIP_AIR);
                decimal.TryParse(strONB_SHIP_SEA, out decONB_SHIP_SEA);
                
                DateTime dateODT_OPEN_DATE = dateNow.AddYears(3), dateODT_DOC_DATE = DateTime.MinValue; //預設日期
                string strOVC_DOC_NO = "", strOVC_NOTE = "";
                try
                {
                    mrp = MTSE.TBGMT_MRP.Where(table => table.OVC_SECTION.Equals(strOVC_SECTION) && DateTime.Compare(table.ODT_MONTH_DATE, dateStart) == 0).FirstOrDefault();
                    bool isCreate = mrp == null;
                    if (isCreate) mrp = new TBGMT_MRP();
                    mrp.OVC_SECTION = strOVC_SECTION;
                    mrp.ODT_MONTH_DATE = dateStart;
                    mrp.ONB_SHIP_AIR = decONB_SHIP_AIR;
                    mrp.ONB_SHIP_SEA = decONB_SHIP_SEA;
                    //mrp.ODT_OPEN_DATE = dateODT_OPEN_DATE;
                    //mrp.ODT_DOC_DATE = dateODT_DOC_DATE;
                    mrp.ODT_OPEN_DATE = dateODT_OPEN_DATE;
                    mrp.ODT_DOC_DATE = null;
                    mrp.OVC_DOC_NO = strOVC_DOC_NO;
                    mrp.OVC_NOTE = strOVC_NOTE;
                    if (isCreate) mrp.OVC_CREATE_LOGIN_ID = strUserId;
                    if (isCreate) mrp.ODT_CREATE_DATE = dateNow;
                    mrp.OVC_MODIFY_LOGIN_ID = strUserId;
                    mrp.ODT_MODIFY_DATE = dateNow;
                    if (isCreate) MTSE.TBGMT_MRP.Add(mrp);
                    MTSE.SaveChanges();

                    txtONB_SHIP_AIR.Text = decONB_SHIP_AIR.ToString();
                    txtONB_SHIP_SEA.Text = decONB_SHIP_SEA.ToString();
                    txtODT_OPEN_DATE.Text = FCommon.getDateTime(dateODT_OPEN_DATE);
                    txtODT_DOC_DATE.Text = FCommon.getDateTime(null);
                    txtOVC_DOC_NO.Text = strOVC_DOC_NO;
                    txtOVC_NOTE.Text = strOVC_NOTE;
                }
                catch
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "統計 接轉作業月報表 錯誤，請聯絡工程師！");
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            return mrp;
        }
        private void getSHIP(string strOVC_SECTION, string strDateStart, string strDateEnd, out string strONB_SHIP_AIR, out string strONB_SHIP_SEA) //取得統計之航架次
        {
            string[] strParameterName_SeaOrAir = { ":start_date", ":end_date", ":sea_or_air", ":section" };
            ArrayList aryData_SeaOrAir = new ArrayList();
            aryData_SeaOrAir.Add(strDateStart);
            aryData_SeaOrAir.Add(strDateEnd);
            aryData_SeaOrAir.Add("");
            aryData_SeaOrAir.Add(strOVC_SECTION);

            string strSQL_PortList = $@"
                    select OVC_PHR_ID from TBM1407 where OVC_PHR_CATE='TR' and OVC_PHR_PARENTS={ strParameterName_SeaOrAir[3] }
                ";
            string strSQL_Port = $@"
                    and ( OVC_ARRIVE_PORT in ({ strSQL_PortList }) or OVC_START_PORT in ({ strSQL_PortList }))
                ";

            #region 取得航架次
            //decONB_SHIP_AIR = 0; decONB_SHIP_SEA = 0;
            strONB_SHIP_AIR = ""; strONB_SHIP_SEA = "";

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
                            GROUP BY a.OVC_SHIP_COMPANY, a.OVC_VOYAGE, a.ODT_START_DATE, a.OVC_SHIP_NAME, a.OVC_ARRIVE_PORT
                        )";

            //航架次
            //string sql1 = "SELECT COUNT(*) FROM (SELECT a.OVC_SHIP_COMPANY, a.OVC_VOYAGE, a.ODT_START_DATE " +
            //    "FROM TBGMT_BLD a, TBGMT_ICR b, TBGMT_ESO c WHERE a.OVC_BLD_NO = b.OVC_BLD_NO (+) AND a.OVC_BLD_NO = c.OVC_BLD_NO (+) AND " +
            //    "(b.ODT_TRANSFER_DATE BETWEEN " + startDate + " AND " + endDate + " or c.ODT_STORED_DATE BETWEEN " + startDate + " AND " + endDate + ") AND " +
            //    " a.OVC_SEA_OR_AIR = :sea_or_air AND " + portCond + "GROUP BY a.OVC_SHIP_COMPANY, a.OVC_VOYAGE, a.ODT_START_DATE,a.OVC_SHIP_NAME,a.OVC_ARRIVE_PORT) ";

            aryData_SeaOrAir[2] = "海運";
            DataTable dt_ONB_SHIP_SEA = FCommon.getDataTableFromSelect(strSQL_ONB_SHIP, strParameterName_SeaOrAir, aryData_SeaOrAir); //海運航次
            //if (dt_ONB_SHIP_SEA.Rows.Count > 0) decimal.TryParse(dt_ONB_SHIP_SEA.Rows[0][0].ToString(), out decONB_SHIP_SEA);
            if (dt_ONB_SHIP_SEA.Rows.Count > 0) strONB_SHIP_AIR = dt_ONB_SHIP_SEA.Rows[0][0].ToString();

            aryData_SeaOrAir[2] = "空運";
            DataTable dt_ONB_SHIP_AIR = FCommon.getDataTableFromSelect(strSQL_ONB_SHIP, strParameterName_SeaOrAir, aryData_SeaOrAir); //空運架次
            //if (dt_ONB_SHIP_AIR.Rows.Count > 0) decimal.TryParse(dt_ONB_SHIP_AIR.Rows[0][0].ToString(), out decONB_SHIP_AIR);
            if (dt_ONB_SHIP_AIR.Rows.Count > 0) strONB_SHIP_SEA = dt_ONB_SHIP_AIR.Rows[0][0].ToString();
            #endregion
        }
        private string getSQL_ImportKind(string strSQL_PortList, string strSQL_DateBetween, string strWhere) //取得SQL-特定種類之進口Item，依照Where
        {
            string strSQL = $@"
                from (select CLASSCDE_TO_CHINAME(OVC_MILITARY_TYPE) as OVC_MILITARY_TYPE, a.OVC_BLD_NO, OVC_PURCH_NO, c.ONB_QUANITY,
                    CASE WHEN OVC_WEIGHT_UNIT <> 'KG' THEN ROUND(NVL(ONB_WEIGHT,0) * 0.45359,3) ELSE ROUND(NVL(ONB_WEIGHT,0) ,3) END ONB_WEIGHT,
                    CASE WHEN OVC_VOLUME_UNIT <> 'CBM' THEN ROUND(NVL(ONB_VOLUME,0) / 35.3142,3) ELSE ROUND(NVL(ONB_VOLUME,0) ,3) END ONB_VOLUME
                    from TBGMT_BLD a, TBGMT_ICR b,
                    (select OVC_BLD_NO,ONB_ACTUAL_RECEIVE AS ONB_QUANITY
                        from (select a.OVC_BLD_NO as OVC_BLD_NO,ONB_ACTUAL_RECEIVE,ONB_OVERFLOW,ONB_LESS, ONB_BROKEN from TBGMT_IRD a, TBGMT_BLD b where a.OVC_BLD_NO = b.OVC_BLD_NO)
                    ) c
                    where a.OVC_BLD_NO = b.OVC_BLD_NO
                    and a.OVC_BLD_NO = c.OVC_BLD_NO
                    and OVC_ARRIVE_PORT in ({ strSQL_PortList })
                    and ODT_TRANSFER_DATE { strSQL_DateBetween }
                    and { strWhere }
                )
                group by OVC_MILITARY_TYPE
            ";
            return strSQL;
        }
        private string getSQL_ExportKine(string strSQL_PortList, string strSQL_DateBetween, string strWhere) //取得SQL-特定種類之出口Item，依照Where
        {
            string strSQL = $@"
                from (select CLASSCDE_TO_CHINAME(OVC_MILITARY_TYPE) as OVC_MILITARY_TYPE, a.OVC_BLD_NO, OVC_PURCH_NO, d.ONB_QUANITY,
                    CASE WHEN OVC_WEIGHT_UNIT <> 'KG' THEN ROUND(NVL(REAL_WEIGHT,0) * 0.45359,3) ELSE ROUND(NVL(REAL_WEIGHT,0) ,3) END ONB_WEIGHT,
                    CASE WHEN OVC_VOLUME_UNIT <> 'CBM' THEN ROUND(NVL(REAL_VOLUME,0) / 35.3142,3) ELSE ROUND(NVL(REAL_VOLUME,0) ,3) END ONB_VOLUME
                    from TBGMT_BLD a, TBGMT_ESO b, TBGMT_EDF c,
                    (select OVC_EDF_NO, COUNT(OVC_BOX_NO) AS ONB_QUANITY from (select DISTINCT OVC_EDF_NO, OVC_BOX_NO from TBGMT_EDF_DETAIL) GROUP BY OVC_EDF_NO) d
                    where a.OVC_BLD_NO = b.OVC_BLD_NO and c.OVC_EDF_NO = b.OVC_EDF_NO and c.OVC_EDF_NO = d.OVC_EDF_NO
                    and a.OVC_START_PORT in ({ strSQL_PortList })
                    and a.REAL_START_DATE { strSQL_DateBetween }
                    and { strWhere }
                )
                group by OVC_MILITARY_TYPE
            ";
            return strSQL;
        }
        private bool getIsExist(string strOVC_SECTION, string strDateStart, string strWhere_start, out DataTable dt_Exist) //取得該筆月報表是否存在
        {
            string[] strParameterName = { ":section", ":start_date" };
            ArrayList aryData = new ArrayList();
            aryData.Add(strOVC_SECTION);
            aryData.Add(strDateStart);

            string strSQL_Exist = $@"
                        select * from TBGMT_MRP
                        { strWhere_start }
                    ";
            //string sql = "SELECT COUNT(*) FROM TBGMT_MRP WHERE ";
            //sql += "OVC_SECTION = '" + section + "' AND";
            //sql += " ODT_MONTH_DATE = TO_DATE('" + firstday.ToShortDateString() + "','yyyy/mm/dd') ";

            dt_Exist = FCommon.getDataTableFromSelect(strSQL_Exist, strParameterName, aryData);
            bool isExist = dt_Exist.Rows.Count > 0;
            return isExist;
        }
        private bool getIsAnyItemExist(string strOVC_SECTION, string strDateStart, string strWhere_start) //取得
        {
            string[] strParameterName = { ":section", ":start_date" };
            ArrayList aryData = new ArrayList();
            aryData.Add(strOVC_SECTION);
            aryData.Add(strDateStart);

            string strSQL_MRP_ITEM_check = $@"
                            select * from TBGMT_MRP_ITEM
                            { strWhere_start }
                        ";
            //string sql = "SELECT COUNT(*) FROM TBGMT_MRP_ITEM WHERE ";
            //sql += " OVC_SECTION = '" + Section + "' AND ";
            //sql += " ODT_MONTH_DATE = TO_DATE('" + MonthDate.ToShortDateString() + "','yyyy/mm/dd') ";
            DataTable dt_MRP_ITEM_check = FCommon.getDataTableFromSelect(strSQL_MRP_ITEM_check, strParameterName, aryData);
            bool isExist = dt_MRP_ITEM_check.Rows.Count > 0;
            return isExist;
        }
        private bool getIsPrevMonthClosedtExist(string strOVC_SECTION, string strDateLastMonth) //取得前月關帳，是否存在
        {
            string[] strParameterName_last = { ":section", ":last_month" };
            ArrayList aryData_last = new ArrayList();
            aryData_last.Add(strOVC_SECTION);
            aryData_last.Add(strDateLastMonth);

            //判斷該區前月未關帳
            string strSQL_Closed = $@"
                select NVL(b.CNT, 0) from
                (select distinct OVC_SECTION from TBGMT_MRP where OVC_SECTION={ strParameterName_last[0] }) a,
                (select OVC_SECTION, NVL(count(1), 0) as CNT from TBGMT_MRP
                    where OVC_SECTION = { strParameterName_last[0] }
                    and CLOSE_DT is not null
                    and ODT_MONTH_DATE = to_date({ strParameterName_last[1] }, {strDateFormatSQL})
                    group by OVC_SECTION, ODT_MONTH_DATE) b
                where a.OVC_SECTION = b.OVC_SECTION (+)
            ";
            //sql += " select nvl(B.cnt,0) from ";
            //sql += " (select distinct OVC_SECTION  from TBGMT_MRP WHERE";
            //sql += " OVC_SECTION = '" + xSection + "' ) A,";
            //sql += " (SELECT OVC_SECTION,nvl(COUNT(1),0) as cnt FROM TBGMT_MRP WHERE";
            //sql += " OVC_SECTION = '" + xSection + "' and";
            //sql += " close_dt is not null AND";
            //sql += " ODT_MONTH_DATE = trunc(add_months(TO_DATE('" + MonthDate.ToShortDateString() + "','yyyy/mm/dd'),-1),'mm')  group by OVC_SECTION,ODT_MONTH_DATE )B where";
            //sql += " A.OVC_SECTION=B.OVC_SECTION(+)  ";
            DataTable dt_Closed = FCommon.getDataTableFromSelect(strSQL_Closed, strParameterName_last, aryData_last);
            bool isExist = false;
            if (dt_Closed.Rows.Count > 0 && int.TryParse(dt_Closed.Rows[0][0].ToString(), out int intTemp) && intTemp > 0)
                isExist = true;
            return isExist;
        }
        private bool getIsClosedtExist(string strOVC_SECTION, string strDateStart) // 取得該月關帳是否存在
        {
            string[] strParameterName_start = { ":section", ":start_date" };
            ArrayList aryData_start = new ArrayList();
            aryData_start.Add(strOVC_SECTION);
            aryData_start.Add(strDateStart);

            //及時差異數tempdiff
            string strSQL_MRP_ITEM_DIFF = $@"
                            select NVL(b.CNT, 0)
                            from
                            (select DISTINCT OVC_SECTION from TBGMT_MRP where OVC_SECTION = { strParameterName_start[0] }) a,
                            (select OVC_SECTION, NVL(count(1), 0) as CNT from TBGMT_MRP
                                where OVC_SECTION = { strParameterName_start[0] }
                                and ODT_MONTH_DATE = to_date({ strParameterName_start[1] }, { strDateFormatSQL })
                                and CLOSE_DT is not null
                                group by OVC_SECTION, ODT_MONTH_DATE) b
                            where a.OVC_SECTION = b.OVC_SECTION (+)
                        ";
            //sql += " select nvl(B.cnt,0) from";
            //sql += " (select distinct OVC_SECTION  from TBGMT_MRP WHERE";
            //sql += " OVC_SECTION = '" + xSection + "' ) A,";
            //sql += " (SELECT OVC_SECTION,nvl(COUNT(1),0) as cnt FROM TBGMT_MRP WHERE";
            //sql += " OVC_SECTION = '" + xSection + "' and";
            //sql += " close_dt is not null AND";
            //sql += " ODT_MONTH_DATE = TO_DATE('" + MonthDate.ToShortDateString() + "','yyyy/mm/dd')  group by OVC_SECTION,ODT_MONTH_DATE )B where";
            //sql += " A.OVC_SECTION=B.OVC_SECTION(+)  ";
            DataTable dt_MRP_ITEM_DIFF = FCommon.getDataTableFromSelect(strSQL_MRP_ITEM_DIFF, strParameterName_start, aryData_start);
            bool isExist = false;
            if (dt_MRP_ITEM_DIFF.Rows.Count > 0 && int.TryParse(dt_MRP_ITEM_DIFF.Rows[0][0].ToString(), out int intTemp) && intTemp > 0)
                isExist = true;
            return isExist;
        }
        //private bool getIsAllClosedtExist(string strOVC_SECTION, string strDateStart, out string strMessage_Section) //判斷三區當月是否全都關帳，採購中心使用
        private bool getIsAllClosedtExist(string strDateStart, out string strMessage_Section) //判斷三區當月是否全都關帳，採購中心使用
        {
            string[] strParameterName_start = { ":section", ":start_date" };
            ArrayList aryData_start = new ArrayList();
            aryData_start.Add(""); //strOVC_SECTION
            aryData_start.Add(strDateStart);

            string strSQL_Closed = $@"
                select count(*) from TBGMT_MRP
                where OVC_SECTION = { strParameterName_start[0] }
                and CLOSE_DT is not null
                and ODT_MONTH_DATE = to_date({ strParameterName_start[1] }, {strDateFormatSQL})
            ";
            //string sql = " select count(*) from tbgmt_mrp where ";
            //sql += " ovc_section in  ('基隆地區','桃園地區','高雄分遣組') and ";
            //sql += " close_dt is not  null AND ";
            //sql += " ODT_MONTH_DATE = TO_date('" + firstdate.ToShortDateString() + "','yyyy/mm/dd') ";

            //int intCountClose = dt_Close.Rows.Count;
            string[] strSectionList = { "基隆地區", "桃園地區", "高雄分遣組" }; //, strSectionListClose = new string[intCountClose];
            //for(int i= 0;i< intCountClose;i++)
            //{
            //    DataRow dr = dt_Close.Rows[i];
            //    strSectionListClose[i] = dr["OVC_SECTION"].ToString();
            //}

            strMessage_Section = "";
            for (int i = 0; i < strSectionList.Length; i++)
            {
                string theSection = strSectionList[i];
                aryData_start[0] = theSection;
                DataTable dt_Close = FCommon.getDataTableFromSelect(strSQL_Closed, strParameterName_start, aryData_start);
                if (dt_Close.Rows.Count == 0 || dt_Close.Rows[0][0].ToString().Equals("0"))
                {
                    if (!strMessage_Section.Equals(string.Empty)) strMessage_Section += "、";
                    strMessage_Section += theSection;
                }
            }
            return strMessage_Section.Equals(string.Empty);
        }
        private bool getIsDiffItemExist(string strOVC_SECTION, string strDateStart) //取得是否有差異Item是否存在
        {
            string[] strParameterName_start = { ":section", ":start_date" };
            ArrayList aryData_start = new ArrayList();
            aryData_start.Add(strOVC_SECTION);
            aryData_start.Add(strDateStart);

            string strSQL_MRP_ITEM_DIFF = $@"
                select count(*)
                from TBGMT_MRP_ITEM_DIFF
                where OVC_SECTION = { strParameterName_start[0] }
                and ODT_MONTH_DATE = to_date({ strParameterName_start[1] }, { strDateFormatSQL })
            ";
            //string sql = "SELECT COUNT(*) FROM TBGMT_MRP_ITEM_DIFF WHERE OVC_SECTION = '" + Section +
            //    "' AND ODT_MONTH_DATE = TO_DATE('" + MonthDate.ToShortDateString() + "','yyyy/mm/dd') ";
            DataTable dt_MRP_ITEM_DIFF = FCommon.getDataTableFromSelect(strSQL_MRP_ITEM_DIFF, strParameterName_start, aryData_start);
            bool isExist = false;
            if (dt_MRP_ITEM_DIFF.Rows.Count > 0 && int.TryParse(dt_MRP_ITEM_DIFF.Rows[0][0].ToString(), out int intTemp) && intTemp > 0)
                isExist = true;
            return isExist;
        }
        private bool getIsLogItemExist(string strOVC_SECTION, string strDateStart) //取得是否有Log Item是否存在
        {
            string[] strParameterName_start = { ":section", ":start_date" };
            ArrayList aryData_start = new ArrayList();
            aryData_start.Add(strOVC_SECTION);
            aryData_start.Add(strDateStart);

            string strSQL_MRP_ITEM_LOG = $@"
                select count(*)
                from TBGMT_MRP_ITEM_LOG
                where OVC_SECTION = { strParameterName_start[0] }
                and ODT_MONTH_DATE = to_date({ strParameterName_start[1] }, { strDateFormatSQL })
            ";
            //string sql = "SELECT COUNT(*) FROM TBGMT_MRP_ITEM_LOG WHERE OVC_SECTION = '" + Section +
            //    "' AND ODT_MONTH_DATE = TO_DATE('" + MonthDate.ToShortDateString() + "','yyyy/mm/dd') ";
            DataTable dt_MRP_ITEM_LOG = FCommon.getDataTableFromSelect(strSQL_MRP_ITEM_LOG, strParameterName_start, aryData_start);
            bool isExist = false;
            if (dt_MRP_ITEM_LOG.Rows.Count > 0 && int.TryParse(dt_MRP_ITEM_LOG.Rows[0][0].ToString(), out int intTemp) && intTemp > 0)
                isExist = true;
            return isExist;
        }
        private void getLeast_MRP_ITEM(DataTable dt_MRP_ITEM, string strOVC_SECTION, string strDateStart) //至少有一筆
        {
            if (dt_MRP_ITEM.Rows.Count == 0)
            {
                DataRow dr_MRP_ITEM = dt_MRP_ITEM.NewRow();
                dr_MRP_ITEM["OVC_SECTION"] = strOVC_SECTION;
                dr_MRP_ITEM["ODT_MONTH_DATE"] = strDateStart;
                dr_MRP_ITEM["OVC_MILITARY"] = "其他";
                dr_MRP_ITEM["OVC_IE"] = "進口";
                dr_MRP_ITEM["ONB_BLD_M"] = 0;
                dr_MRP_ITEM["ONB_QUANITY_M"] = 0;
                dr_MRP_ITEM["ONB_WEIGHT_M"] = 0;
                dr_MRP_ITEM["ONB_VOLUME_M"] = 0;
                dr_MRP_ITEM["ONB_BLD_C"] = 0;
                dr_MRP_ITEM["ONB_QUANITY_C"] = 0;
                dr_MRP_ITEM["ONB_WEIGHT_C"] = 0;
                dr_MRP_ITEM["ONB_VOLUME_C"] = 0;
                dr_MRP_ITEM["ONB_BLD_O"] = 0;
                dr_MRP_ITEM["ONB_QUANITY_O"] = 0;
                dr_MRP_ITEM["ONB_WEIGHT_O"] = 0;
                dr_MRP_ITEM["ONB_VOLUME_O"] = 0;
                dt_MRP_ITEM.Rows.Add(dr_MRP_ITEM);
            }
        }
        private void setDeleteAllItem(string strOVC_SECTION, DateTime dateStart) //刪掉所有Item
        {
            var mrp_itemDel =
                from item in MTSE.TBGMT_MRP_ITEM
                where item.OVC_SECTION.Equals(strOVC_SECTION)
                where DateTime.Compare(item.ODT_MONTH_DATE, dateStart) == 0
                select item;
            //string where = " OVC_SECTION = '" + Section +
            //    "' AND ODT_MONTH_DATE = TO_DATE('" + MonthDate.ToShortDateString() + "','yyyy/mm/dd') ";
            //int i = onlineDB.Delete("TBGMT_MRP_ITEM", where);
            foreach (TBGMT_MRP_ITEM item in mrp_itemDel)
            {
                MTSE.Entry(item).State = EntityState.Deleted;
                MTSE.SaveChanges();
            }
        }
        private void setGetDummyItems_CreateItems(string strOVC_SECTION, DateTime dateStart, DateTime dateEnd) //統計Item 並存進 TBGMT_MRP_ITEM 暫存區
        {
            string strUserId = Session["userid"].ToString();
            DateTime dateNow = DateTime.Now;
            string strDateStart = FCommon.getDateTime(dateStart);
            string strDateEnd = FCommon.getDateTime(dateEnd);

            string[] strParameterName = { ":section", ":start_date", ":end_date" };
            ArrayList aryData = new ArrayList();
            aryData.Add(strOVC_SECTION);
            aryData.Add(strDateStart);
            aryData.Add(strDateEnd);
            string strSQL_PortList = $@"
                    select OVC_PHR_ID from TBM1407 where OVC_PHR_CATE='TR' and OVC_PHR_PARENTS={ strParameterName[0] }
                ";
            string strSQL_DateBetween = $@"
                    between to_date({ strParameterName[1] }, { strDateFormatSQL }) and to_date({ strParameterName[2] }, { strDateFormatSQL })
                ";

            #region SQL
            string strSQL_MRP_ITEM = $@"
                select
                { strParameterName[0] } as OVC_SECTION,
                { strParameterName[1] } as ODT_MONTH_DATE,
                a.OVC_CLASS_NAME AS OVC_MILITARY,
                '進口' AS OVC_IE,
                ONB_BLD_M, ONB_QUANITY_M, ONB_WEIGHT_M, ONB_VOLUME_M,
                ONB_BLD_C, ONB_QUANITY_C, ONB_WEIGHT_C, ONB_VOLUME_C,
                ONB_BLD_O, ONB_QUANITY_O, ONB_WEIGHT_O, ONB_VOLUME_O
                from
                (select OVC_CLASS_NAME from TBGMT_DEPT_CLASS) a,
                (select OVC_MILITARY_TYPE, count(OVC_BLD_NO) as ONB_BLD_M, sum(ONB_QUANITY) as ONB_QUANITY_M, sum(ONB_WEIGHT) as ONB_WEIGHT_M, sum(ONB_VOLUME) as ONB_VOLUME_M
                    { getSQL_ImportKind(strSQL_PortList, strSQL_DateBetween, "(OVC_PURCH_NO like '___' or OVC_PURCH_NO like '___ %')") }
                ) x,
                (select OVC_MILITARY_TYPE, count(OVC_BLD_NO) as ONB_BLD_C, sum(ONB_QUANITY) as ONB_QUANITY_C, sum(ONB_WEIGHT) as ONB_WEIGHT_C, sum(ONB_VOLUME) as ONB_VOLUME_C
                    { getSQL_ImportKind(strSQL_PortList, strSQL_DateBetween, "not (OVC_PURCH_NO like '___' or OVC_PURCH_NO like '___ %' or upper(OVC_PURCH_NO) = 'NONE' or OVC_PURCH_NO like '新光%')") }
                ) y,
                (select OVC_MILITARY_TYPE, count(OVC_BLD_NO) as ONB_BLD_O, sum(ONB_QUANITY) as ONB_QUANITY_O, sum(ONB_WEIGHT) as ONB_WEIGHT_O, sum(ONB_VOLUME) as ONB_VOLUME_O
                    { getSQL_ImportKind(strSQL_PortList, strSQL_DateBetween, "(upper(OVC_PURCH_NO) = 'NONE' or OVC_PURCH_NO like '新光%')") }
                ) z
                where a.OVC_CLASS_NAME  = x.OVC_MILITARY_TYPE (+)
                and a.OVC_CLASS_NAME = y.OVC_MILITARY_TYPE (+)
                and a.OVC_CLASS_NAME = z.OVC_MILITARY_TYPE (+)

                UNION

                select
                { strParameterName[0] } AS OVC_SECTION,
                { strParameterName[1] } AS ODT_MONTH_DATE,
                a.OVC_CLASS_NAME AS OVC_MILITARY,
                '出口' AS OVC_IE,
                ONB_BLD_M, ONB_QUANITY_M, ONB_WEIGHT_M, ONB_VOLUME_M,
                ONB_BLD_C, ONB_QUANITY_C, ONB_WEIGHT_C, ONB_VOLUME_C,
                ONB_BLD_O, ONB_QUANITY_O, ONB_WEIGHT_O, ONB_VOLUME_O
                from
                (select OVC_CLASS_NAME from TBGMT_DEPT_CLASS) a,
                (select OVC_MILITARY_TYPE, count(OVC_BLD_NO) as ONB_BLD_M, sum(ONB_QUANITY) as ONB_QUANITY_M, sum(ONB_WEIGHT) as ONB_WEIGHT_M, sum(ONB_VOLUME) as ONB_VOLUME_M
                    { getSQL_ExportKine(strSQL_PortList, strSQL_DateBetween, "(OVC_PURCH_NO like '___' or OVC_PURCH_NO like '___ %')") }
                ) x,
                (select OVC_MILITARY_TYPE, count(OVC_BLD_NO) as ONB_BLD_C, sum(ONB_QUANITY) as ONB_QUANITY_C, sum(ONB_WEIGHT) as ONB_WEIGHT_C, sum(ONB_VOLUME) as ONB_VOLUME_C
                    { getSQL_ExportKine(strSQL_PortList, strSQL_DateBetween, "not (OVC_PURCH_NO  like '___' or OVC_PURCH_NO like '___ %' or upper(OVC_PURCH_NO) = 'NONE' or OVC_PURCH_NO like '新光%')") }
                ) y,
                (
                    select OVC_MILITARY_TYPE, count(OVC_BLD_NO) as ONB_BLD_O, sum(ONB_QUANITY) as ONB_QUANITY_O, sum(ONB_WEIGHT) as ONB_WEIGHT_O, sum(ONB_VOLUME) as ONB_VOLUME_O
                    { getSQL_ExportKine(strSQL_PortList, strSQL_DateBetween, "(upper(OVC_PURCH_NO) = 'NONE')") }
                UNION
                    select OVC_MILITARY_TYPE, count(OVC_BLD_NO) as ONB_BLD_O, sum(ONB_QUANITY) as ONB_QUANITY_O, sum(ONB_WEIGHT) as ONB_WEIGHT_O, sum(ONB_VOLUME) as ONB_VOLUME_O
                    from (select CLASSCDE_TO_CHINAME(OVC_MILITARY_TYPE) as OVC_MILITARY_TYPE, a.OVC_BLD_NO, OVC_PURCH_NO, a.ONB_QUANITY,
                        CASE WHEN OVC_WEIGHT_UNIT <> 'KG' THEN ROUND(NVL(REAL_WEIGHT,0) * 0.45359,3) ELSE ROUND(NVL(REAL_WEIGHT,0) ,3) END ONB_WEIGHT,
                        CASE WHEN OVC_VOLUME_UNIT <> 'CBM' THEN ROUND(NVL(REAL_VOLUME,0) / 35.3142,3) ELSE ROUND(NVL(REAL_VOLUME,0) ,3) END ONB_VOLUME
                        from TBGMT_BLD a, TBGMT_ESO b, TBGMT_EDF c
                        where a.OVC_BLD_NO = b.OVC_BLD_NO and c.OVC_EDF_NO = b.OVC_EDF_NO
                        and a.OVC_START_PORT in ({ strSQL_PortList })
                        and a.REAL_START_DATE { strSQL_DateBetween }
                        and (OVC_PURCH_NO like '新光%')
                    )
                    group by OVC_MILITARY_TYPE
                ) z
                where a.OVC_CLASS_NAME  = x.OVC_MILITARY_TYPE (+)
                and a.OVC_CLASS_NAME = y.OVC_MILITARY_TYPE (+)
                and a.OVC_CLASS_NAME = z.OVC_MILITARY_TYPE (+)
            ";
            #endregion
            DataTable dt_MRP_ITEM = FCommon.getDataTableFromSelect(strSQL_MRP_ITEM, strParameterName, aryData);
            getLeast_MRP_ITEM(dt_MRP_ITEM, strOVC_SECTION, strDateStart);
            foreach (DataRow dr_MRP_ITEM in dt_MRP_ITEM.Rows)
            {
                string strOVC_MILITARY = dr_MRP_ITEM["OVC_MILITARY"].ToString();
                string strOVC_IE = dr_MRP_ITEM["OVC_IE"].ToString();
                decimal decONB_BLD_M, decONB_QUANITY_M, decONB_WEIGHT_M, decONB_VOLUME_M;
                decimal decONB_BLD_C, decONB_QUANITY_C, decONB_WEIGHT_C, decONB_VOLUME_C;
                decimal decONB_BLD_O, decONB_QUANITY_O, decONB_WEIGHT_O, decONB_VOLUME_O;
                decimal.TryParse(dr_MRP_ITEM["ONB_BLD_M"].ToString(), out decONB_BLD_M);
                decimal.TryParse(dr_MRP_ITEM["ONB_QUANITY_M"].ToString(), out decONB_QUANITY_M);
                decimal.TryParse(dr_MRP_ITEM["ONB_WEIGHT_M"].ToString(), out decONB_WEIGHT_M);
                decimal.TryParse(dr_MRP_ITEM["ONB_VOLUME_M"].ToString(), out decONB_VOLUME_M);
                decimal.TryParse(dr_MRP_ITEM["ONB_BLD_C"].ToString(), out decONB_BLD_C);
                decimal.TryParse(dr_MRP_ITEM["ONB_QUANITY_C"].ToString(), out decONB_QUANITY_C);
                decimal.TryParse(dr_MRP_ITEM["ONB_WEIGHT_C"].ToString(), out decONB_WEIGHT_C);
                decimal.TryParse(dr_MRP_ITEM["ONB_VOLUME_C"].ToString(), out decONB_VOLUME_C);
                decimal.TryParse(dr_MRP_ITEM["ONB_BLD_O"].ToString(), out decONB_BLD_O);
                decimal.TryParse(dr_MRP_ITEM["ONB_QUANITY_O"].ToString(), out decONB_QUANITY_O);
                decimal.TryParse(dr_MRP_ITEM["ONB_WEIGHT_O"].ToString(), out decONB_WEIGHT_O);
                decimal.TryParse(dr_MRP_ITEM["ONB_VOLUME_O"].ToString(), out decONB_VOLUME_O);

                TBGMT_MRP_ITEM mrp_item = new TBGMT_MRP_ITEM();
                mrp_item.MRP_ITEM_SN = Guid.NewGuid();
                mrp_item.OVC_SECTION = strOVC_SECTION;
                mrp_item.ODT_MONTH_DATE = dateStart;
                mrp_item.OVC_MILITARY = strOVC_MILITARY;
                mrp_item.OVC_IE = strOVC_IE;
                mrp_item.ONB_BLD_M = decONB_BLD_M;
                mrp_item.ONB_QUANITY_M = decONB_QUANITY_M;
                mrp_item.ONB_WEIGHT_M = decONB_WEIGHT_M;
                mrp_item.ONB_VOLUME_M = decONB_VOLUME_M;
                mrp_item.ONB_BLD_C = decONB_BLD_C;
                mrp_item.ONB_QUANITY_C = decONB_QUANITY_C;
                mrp_item.ONB_WEIGHT_C = decONB_WEIGHT_C;
                mrp_item.ONB_VOLUME_C = decONB_VOLUME_C;
                mrp_item.ONB_BLD_O = decONB_BLD_O;
                mrp_item.ONB_QUANITY_O = decONB_QUANITY_O;
                mrp_item.ONB_WEIGHT_O = decONB_WEIGHT_O;
                mrp_item.ONB_VOLUME_O = decONB_VOLUME_O;
                mrp_item.OVC_CREATE_LOGIN_ID = strUserId;
                mrp_item.ODT_CREATE_DATE = dateNow;
                mrp_item.OVC_MODIFY_LOGIN_ID = strUserId;
                mrp_item.ODT_MODIFY_DATE = dateNow;
                MTSE.TBGMT_MRP_ITEM.Add(mrp_item);

                //TBGMT_MRP_ITEM_LOG mrp_item_log = new TBGMT_MRP_ITEM_LOG();
                //mrp_item_log.OVC_SECTION = strOVC_SECTION;
                //mrp_item_log.ODT_MONTH_DATE = dateStart;
                //mrp_item_log.OVC_MILITARY = strOVC_MILITARY;
                //mrp_item_log.OVC_IE = strOVC_IE;
                //mrp_item_log.ONB_BLD_M = decONB_BLD_M;
                //mrp_item_log.ONB_QUANITY_M = decONB_QUANITY_M;
                //mrp_item_log.ONB_WEIGHT_M = decONB_WEIGHT_M;
                //mrp_item_log.ONB_VOLUME_M = decONB_VOLUME_M;
                //mrp_item_log.ONB_BLD_C = decONB_BLD_C;
                //mrp_item_log.ONB_QUANITY_C = decONB_QUANITY_C;
                //mrp_item_log.ONB_WEIGHT_C = decONB_WEIGHT_C;
                //mrp_item_log.ONB_VOLUME_C = decONB_VOLUME_C;
                //mrp_item_log.ONB_BLD_O = decONB_BLD_O;
                //mrp_item_log.ONB_QUANITY_O = decONB_QUANITY_O;
                //mrp_item_log.ONB_WEIGHT_O = decONB_WEIGHT_O;
                //mrp_item_log.ONB_VOLUME_O = decONB_VOLUME_O;
                //mrp_item_log.OVC_CREATE_LOGIN_ID = strUserId;
                //mrp_item_log.ODT_CREATE_DATE = dateNow;
                //mrp_item_log.OVC_MODIFY_LOGIN_ID = strUserId;
                //mrp_item_log.ODT_MODIFY_DATE = dateNow;
                //MTSE.TBGMT_MRP_ITEM_LOG.Add(mrp_item_log);

                //TBGMT_MRP_ITEM_DIFF mrp_item_diff = new TBGMT_MRP_ITEM_DIFF();
                //mrp_item_diff.OVC_SECTION = strOVC_SECTION;
                //mrp_item_diff.ODT_MONTH_DATE = dateStart;
                //mrp_item_diff.OVC_MILITARY = strOVC_MILITARY;
                //mrp_item_diff.OVC_IE = strOVC_IE;
                //mrp_item_diff.ONB_BLD_M = decONB_BLD_M;
                //mrp_item_diff.ONB_QUANITY_M = decONB_QUANITY_M;
                //mrp_item_diff.ONB_WEIGHT_M = decONB_WEIGHT_M;
                //mrp_item_diff.ONB_VOLUME_M = decONB_VOLUME_M;
                //mrp_item_diff.ONB_BLD_C = decONB_BLD_C;
                //mrp_item_diff.ONB_QUANITY_C = decONB_QUANITY_C;
                //mrp_item_diff.ONB_WEIGHT_C = decONB_WEIGHT_C;
                //mrp_item_diff.ONB_VOLUME_C = decONB_VOLUME_C;
                //mrp_item_diff.ONB_BLD_O = decONB_BLD_O;
                //mrp_item_diff.ONB_QUANITY_O = decONB_QUANITY_O;
                //mrp_item_diff.ONB_WEIGHT_O = decONB_WEIGHT_O;
                //mrp_item_diff.ONB_VOLUME_O = decONB_VOLUME_O;
                //mrp_item_diff.OVC_CREATE_LOGIN_ID = strUserId;
                //mrp_item_diff.ODT_CREATE_DATE = dateNow;
                //mrp_item_diff.OVC_MODIFY_LOGIN_ID = strUserId;
                //mrp_item_diff.ODT_MODIFY_DATE = dateNow;
                //MTSE.TBGMT_MRP_ITEM_DIFF.Add(mrp_item_diff);

                MTSE.SaveChanges();
            }
        }
        private DataTable getGetItems(string strOVC_SECTION, string strDateStart, string strTable1, string strTable2)
        {
            string[] strParameterName_start = { ":section", ":start_date" };
            ArrayList aryData_start = new ArrayList();
            aryData_start.Add(strOVC_SECTION);
            aryData_start.Add(strDateStart);

            string strWhere_start = $@"
                where OVC_SECTION = { strParameterName_start[0] }
                and ODT_MONTH_DATE = to_date({ strParameterName_start[1] }, { strDateFormatSQL })
            ";
            string strSQL_MRP_ITEM = $@"
                        select 
                        OVC_SECTION, max(ODT_MONTH_DATE) as ODT_MONTH_DATE ,OVC_MILITARY AS OVC_MILITARY, OVC_IE AS OVC_IE,
                        SUM(ONB_BLD_M) AS ONB_BLD_M,
                        SUM(ONB_QUANITY_M) AS ONB_QUANITY_M,
                        SUM(ONB_WEIGHT_M) AS ONB_WEIGHT_M,
                        SUM(ONB_VOLUME_M) AS ONB_VOLUME_M,
                        SUM(ONB_BLD_C) AS ONB_BLD_C,
                        SUM(ONB_QUANITY_C) AS ONB_QUANITY_C,
                        SUM(ONB_WEIGHT_C) AS ONB_WEIGHT_C,
                        SUM(ONB_VOLUME_C) AS ONB_VOLUME_C,
                        SUM(ONB_BLD_O) AS ONB_BLD_O,
                        SUM(ONB_QUANITY_O) AS ONB_QUANITY_O,
                        SUM(ONB_WEIGHT_O) AS ONB_WEIGHT_O,
                        SUM(ONB_VOLUME_O) AS ONB_VOLUME_O
                        from
                        (
                            select
                            OVC_SECTION, ODT_MONTH_DATE, OVC_MILITARY, OVC_IE,
                            ONB_BLD_M, ONB_QUANITY_M, ONB_WEIGHT_M, ONB_VOLUME_M,
                            ONB_BLD_C, ONB_QUANITY_C, ONB_WEIGHT_C, ONB_VOLUME_C,
                            ONB_BLD_O, ONB_QUANITY_O, ONB_WEIGHT_O, ONB_VOLUME_O,
                            YEAR, ODT_MODIFY_DATE
                            from { strTable1 }
                            { strWhere_start }
                        UNION
                            select
                            OVC_SECTION, ODT_MONTH_DATE, OVC_MILITARY, OVC_IE,
                            ONB_BLD_M, ONB_QUANITY_M, ONB_WEIGHT_M, ONB_VOLUME_M,
                            ONB_BLD_C, ONB_QUANITY_C, ONB_WEIGHT_C, ONB_VOLUME_C,
                            ONB_BLD_O, ONB_QUANITY_O, ONB_WEIGHT_O, ONB_VOLUME_O,
                            YEAR, ODT_MODIFY_DATE
                            from { strTable2 }
                            { strWhere_start }
                        )
                        group by OVC_SECTION, OVC_MILITARY, OVC_IE
                        order by OVC_IE desc, OVC_MILITARY
                    ";

            DataTable dt_MRP_ITEM = FCommon.getDataTableFromSelect(strSQL_MRP_ITEM, strParameterName_start, aryData_start);
            return dt_MRP_ITEM;
        }

        private bool getDate(out DateTime dateStart, out DateTime dateEnd, out DateTime dateLastMonth, ref string outMessage) //從下拉式選單取得該月之相關日期
        {
            string strMessage = "";
            string strYear = drpOdtYear.SelectedValue;
            string strMonth = drpOdtMonth.SelectedValue;
            int intYear, intMonth;
            #region 錯誤訊息
            if (strYear.Equals(string.Empty))
                strMessage += "<p> 請選擇 年！ </p>";
            if (strMonth.Equals(string.Empty))
                strMessage += "<p> 請選擇 月！ </p>";
            bool boolYear = FCommon.checkInt(strYear, "年", ref strMessage, out intYear);
            bool boolMonth = FCommon.checkInt(strMonth, "月", ref strMessage, out intMonth);
            #endregion
            bool isPass = strMessage.Equals(string.Empty);
            if (isPass)
            {
                dateStart = new DateTime(intYear + 1911, intMonth, 1); //取得該月第一天之日期
                dateEnd = dateStart.AddMonths(1).AddDays(-1); //取得該月最後一天之日期
                dateLastMonth = dateStart.AddMonths(-1);
            }
            else
            {
                outMessage += strMessage;
                dateStart = DateTime.MinValue; //取得該月第一天之日期
                dateEnd = DateTime.MinValue; //取得該月最後一天之日期
                dateLastMonth = DateTime.MinValue;
            }

            return isPass;
        }
        #endregion

        #region 列印
        private void OriginPrint(string strDateStart, string strDateEnd, string strODT_OPEN_DATE, string strODT_DOC_DATE, string strOVC_DOC_NO, string strDateNow, string strDateNowTaiwan)
        {
            string[] strParameterName = { ":date_start", ":date_end" };
            ArrayList aryData = new ArrayList();
            aryData.Add(strDateStart);
            aryData.Add(strDateEnd);
            string sql = $@"select OVC_MILITARY AS 軍種, OVC_IE AS OVC_IE, SUM(ONB_QUANITY_M) AS 軍售件數, SUM(ONB_WEIGHT_M) AS 軍售重量,
                            SUM(ONB_VOLUME_M) AS 軍售體積, SUM(ONB_BLD_M) AS 軍售報關, SUM(ONB_QUANITY_C) AS 商購件數, SUM(ONB_WEIGHT_C) AS 商購重量,     
                            SUM(ONB_VOLUME_C) AS 商購體積,  SUM(ONB_BLD_C) AS 商購報關, SUM(ONB_QUANITY_O) AS 其他件數, SUM(ONB_WEIGHT_O) AS 其他重量,    
                            SUM(ONB_VOLUME_O) AS 其他體積, SUM(ONB_BLD_O) AS 其他報關 FROM (select * from TBGMT_MRP_ITEM 
                            where ODT_MONTH_DATE BETWEEN to_date({ strParameterName[0] }, { strDateFormatSQL }) AND to_date({ strParameterName[1] }, { strDateFormatSQL }) union 
                            select * from  TBGMT_MRP_ITEM_TEMP_DIFF where ODT_MONTH_DATE BETWEEN to_date({ strParameterName[0] }, { strDateFormatSQL }) and to_date({ strParameterName[1] }, { strDateFormatSQL }))    
                            group by OVC_SECTION,OVC_MILITARY ,OVC_IE 
                            order by OVC_IE DESC , OVC_MILITARY";
            DataTable dtyear = FCommon.getDataTableFromSelect(sql, strParameterName, aryData);

            Document doc = new Document(PageSize.A4, 10, 10, 100, 50);
            MemoryStream Memory = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, Memory);
            doc.Open();

            //設定字型
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H
          , BaseFont.NOT_EMBEDDED);
            Font ChFont = new Font(bfChinese, 8, Font.NORMAL, BaseColor.BLACK);
            Font ChFont2 = new Font(bfChinese, 8, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK);
            Font ChFont3 = new Font(bfChinese, 8, Font.BOLD, BaseColor.BLACK);
            string chFontPath = "c:\\windows\\fonts\\KAIU.TTF";
            BaseFont chBaseFont = BaseFont.CreateFont(chFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);


            PdfPTable pdftable = new PdfPTable(17);
            pdftable.SetWidths(new float[] { 6, 2, 3, 4, 3, 2, 3, 4, 3, 2, 3, 4, 3, 2, 3, 4, 3 });

            pdftable.TotalWidth = 580F;
            pdftable.LockedWidth = true;
            pdftable.DefaultCell.FixedHeight = 25f;
            pdftable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdftable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;


            PdfContentByte cb = writer.DirectContent;
            Rectangle pageSize = doc.PageSize;
            cb.SetRGBColorFill(0, 0, 0);
            cb.BeginText();
            cb.SetFontAndSize(chBaseFont, 12);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $"保密區分：密(本件屬一般公務機密，保密至 { strODT_OPEN_DATE }解除密等) ", pageSize.GetLeft(10), pageSize.GetTop(60), 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $"發文日期：中華民國 { strODT_DOC_DATE }", pageSize.GetLeft(10), pageSize.GetTop(75), 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $"發文字號：{ strOVC_DOC_NO }", pageSize.GetLeft(10), pageSize.GetTop(90), 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $"月報表統計日期：{ strDateNow }", pageSize.GetRight(220), pageSize.GetTop(90), 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "資料來源：國防採購室", pageSize.GetLeft(10), pageSize.GetBottom(60), 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "填表說明：國防採購室於次月五日前彙編本表，傳送主計局。", pageSize.GetLeft(10), pageSize.GetBottom(45), 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $"中華民國 { strDateNowTaiwan }編制", pageSize.GetRight(180), pageSize.GetBottom(60), 0);
            cb.EndText();

            pdftable.AddCell(new Phrase("秘密類", ChFont));

            PdfPCell cell0_1 = new PdfPCell(new Phrase("次月五日前編報", ChFont));
            cell0_1.Colspan = 10;
            cell0_1.Rowspan = 2;
            pdftable.AddCell(cell0_1);

            PdfPCell cell0_2 = new PdfPCell(new Phrase("編制單位", ChFont));
            cell0_2.Colspan = 2;
            pdftable.AddCell(cell0_2);

            PdfPCell cell0_3 = new PdfPCell(new Phrase("國防部國防採購室", ChFont));
            cell0_3.Colspan = 4;
            pdftable.AddCell(cell0_3);

            pdftable.AddCell(new Phrase("月報", ChFont));

            PdfPCell cell1_1 = new PdfPCell(new Phrase("表單編號", ChFont));
            cell1_1.Colspan = 2;
            pdftable.AddCell(cell1_1);

            PdfPCell cell1_2 = new PdfPCell(new Phrase("3324-25-03", ChFont));
            cell1_2.Colspan = 4;
            pdftable.AddCell(cell1_2);

            PdfPCell cell2_0 = new PdfPCell(new Phrase("國軍國外物資國防採購室接轉月報表\n中華民國" + drpOdtYear.SelectedValue + "年" + drpOdtMonth.SelectedValue + "月", ChFont));
            cell2_0.Colspan = 13;
            cell2_0.Rowspan = 2;
            pdftable.AddCell(cell2_0);

            PdfPCell cell2_1 = new PdfPCell(new Phrase("單位：件、KG、CBM、次數", ChFont));
            cell2_1.Colspan = 4;
            cell2_1.Rowspan = 2;
            pdftable.AddCell(cell2_1);

            pdftable.AddCell(new Phrase("區分", ChFont));

            PdfPCell cell3_1 = new PdfPCell(new Phrase("駐美採購組軍購案", ChFont));
            cell3_1.Colspan = 4;
            pdftable.AddCell(cell3_1);

            PdfPCell cell3_2 = new PdfPCell(new Phrase("駐美(歐)採購組商購案", ChFont));
            cell3_2.Colspan = 4;
            pdftable.AddCell(cell3_2);

            PdfPCell cell3_3 = new PdfPCell(new Phrase("其他", ChFont));
            cell3_3.Colspan = 4;
            pdftable.AddCell(cell3_3);

            PdfPCell cell3_4 = new PdfPCell(new Phrase("合計", ChFont));
            cell3_4.Colspan = 4;
            pdftable.AddCell(cell3_4);

            pdftable.AddCell(new Phrase("進口單位", ChFont2));
            string[] unit = { "件", "重量(KG)", "體積(CBM)", "報關數" };
            for (int a = 0; a < 4; a++)
            {
                for (int b = 0; b < 4; b++)
                {
                    pdftable.AddCell(new Phrase(unit[b], ChFont));
                }
            }
            DataTable gv = ((DataTable)ViewState["GV_TBGMT_MRP_ITEM"]) ?? new DataTable();
            string[] unit2 = { "中央", "陸軍", "海軍", "空軍", "資通電軍指揮部", "憲兵", "後備", "星光指揮部", "進口合計" };
            string[] unit3 = { "中央", "陸軍", "海軍", "空軍", "資通電軍指揮部", "憲兵", "後備", "星光指揮部", "出口合計", "本月合計", "年度合計" };

            double[] total_count = new double[4];
            for (int a = 0; a < unit2.Length; a++)
            {
                double count1 = 0, count2 = 0, count3 = 0, count4 = 0;
                for (int b = 0; b < 17; b++)
                {
                    if (b == 0)
                    {
                        if (a == unit2.Length - 1)
                        {
                            pdftable.AddCell(new Phrase(unit2[a], ChFont3));
                        }
                        else
                        {
                            pdftable.AddCell(new Phrase(unit2[a], ChFont));
                        }
                    }
                    else if (b < 13)
                    {
                        if (a == unit2.Length - 1)
                        {
                            double count = 0;
                            for (int i = 0; i < gv.Rows.Count; i++)
                            {
                                if (gv.Rows[i]["OVC_IE"].ToString() == "進口")
                                {
                                    //count += Convert.ToDouble(gv.Rows[i][b + 2].ToString());
                                    string strCount = gv.Rows[i][b + 3].ToString();
                                    if (double.TryParse(strCount, out double intTemp))
                                        count += intTemp;
                                }
                            }
                            pdftable.AddCell(new Phrase(Math.Round(count).ToString(), ChFont));
                            if (b == 1 || b == 5 || b == 9)
                            {
                                count1 += count;
                                total_count[0] = count1;
                            }
                            if (b == 2 || b == 6 || b == 10)
                            {
                                count2 += count;
                                total_count[1] = count2;
                            }
                            if (b == 3 || b == 7 || b == 11)
                            {
                                count3 += count;
                                total_count[2] = count3;
                            }
                            if (b == 4 || b == 8 || b == 12)
                            {
                                count4 += count;
                                total_count[3] = count4;
                            }
                        }
                        else
                        {
                            Double count = 0;
                            for (int i = 0; i < gv.Rows.Count; i++)
                            {
                                if (gv.Rows[i]["OVC_MILITARY"].ToString() == unit2[a] && gv.Rows[i]["OVC_IE"].ToString() == "進口")
                                {
                                    //count += Convert.ToDouble(gv.Rows[i][b + 2].ToString());
                                    string strCount = gv.Rows[i][b + 3].ToString();
                                    if (double.TryParse(strCount, out double intTemp))
                                        count += intTemp;
                                }
                            }
                            pdftable.AddCell(new Phrase(Math.Round(count).ToString(), ChFont));
                            if (b == 1 || b == 5 || b == 9)
                            {
                                count1 += count;
                                total_count[0] = count1;
                            }
                            if (b == 2 || b == 6 || b == 10)
                            {
                                count2 += count;
                                total_count[1] = count2;
                            }
                            if (b == 3 || b == 7 || b == 11)
                            {
                                count3 += count;
                                total_count[2] = count3;
                            }
                            if (b == 4 || b == 8 || b == 12)
                            {
                                count4 += count;
                                total_count[3] = count4;
                            }
                        }
                    }
                    else
                    {
                        pdftable.AddCell(new Phrase(Math.Round(total_count[b - 13]).ToString(), ChFont));

                    }
                }
            }
            pdftable.AddCell(new Phrase("出口單位", ChFont2));
            for (int a = 0; a < 4; a++)
            {
                for (int b = 0; b < 4; b++)
                {
                    pdftable.AddCell(new Phrase(unit[b], ChFont));
                }
            }

            for (int a = 0; a < unit3.Length; a++)
            {
                double count1 = 0, count2 = 0, count3 = 0, count4 = 0;
                for (int b = 0; b < 17; b++)
                {
                    if (b == 0)
                    {
                        if (a == unit3.Length - 3 || a == unit3.Length - 2 || a == unit3.Length - 1)
                        {
                            pdftable.AddCell(new Phrase(unit3[a], ChFont3));
                        }
                        else
                        {
                            pdftable.AddCell(new Phrase(unit3[a], ChFont));
                        }

                    }
                    else if (b < 13)
                    {
                        if (a == unit3.Length - 3)
                        {
                            Double count = 0;
                            for (int i = 0; i < gv.Rows.Count; i++)
                            {
                                if (gv.Rows[i]["OVC_IE"].ToString() == "出口")
                                {
                                    //count += Convert.ToDouble(gv.Rows[i][b + 2].ToString());
                                    string strCount = gv.Rows[i][b + 3].ToString();
                                    if (double.TryParse(strCount, out double intTemp))
                                        count += intTemp;
                                }
                            }
                            pdftable.AddCell(new Phrase(Math.Round(count).ToString(), ChFont));
                            if (b == 1 || b == 5 || b == 9)
                            {
                                count1 += count;
                                total_count[0] = count1;
                            }
                            if (b == 2 || b == 6 || b == 10)
                            {
                                count2 += count;
                                total_count[1] = count2;
                            }
                            if (b == 3 || b == 7 || b == 11)
                            {
                                count3 += count;
                                total_count[2] = count3;
                            }
                            if (b == 4 || b == 8 || b == 12)
                            {
                                count4 += count;
                                total_count[3] = count4;
                            }
                        }
                        else if (a == unit3.Length - 2)
                        {
                            Double count = 0;
                            for (int i = 0; i < gv.Rows.Count; i++)
                            {
                                if (gv.Rows[i]["OVC_IE"].ToString() == "出口" || gv.Rows[i]["OVC_IE"].ToString() == "進口")
                                {
                                    //count += Convert.ToDouble(gv.Rows[i][b + 2].ToString());
                                    string strCount = gv.Rows[i][b + 3].ToString();
                                    if (double.TryParse(strCount, out double intTemp))
                                        count += intTemp;
                                }
                            }
                            pdftable.AddCell(new Phrase(Math.Round(count).ToString(), ChFont));
                            if (b == 1 || b == 5 || b == 9)
                            {
                                count1 += count;
                                total_count[0] = count1;
                            }
                            if (b == 2 || b == 6 || b == 10)
                            {
                                count2 += count;
                                total_count[1] = count2;
                            }
                            if (b == 3 || b == 7 || b == 11)
                            {
                                count3 += count;
                                total_count[2] = count3;
                            }
                            if (b == 4 || b == 8 || b == 12)
                            {
                                count4 += count;
                                total_count[3] = count4;
                            }
                        }
                        else if (a == 10) //有問題 年度合計
                        {
                            Double count = 0;
                            for (int i = 0; i < dtyear.Rows.Count; i++)
                            {
                                if (dtyear.Rows[i]["OVC_IE"].ToString() == "出口" || dtyear.Rows[i]["OVC_IE"].ToString() == "進口")
                                {
                                    count += Convert.ToDouble(dtyear.Rows[i][b + 1].ToString());
                                }
                            }
                            pdftable.AddCell(new Phrase(Math.Round(count).ToString(), ChFont));
                            if (b == 1 || b == 5 || b == 9)
                            {
                                count1 += count;
                                total_count[0] = count1;
                            }
                            if (b == 2 || b == 6 || b == 10)
                            {
                                count2 += count;
                                total_count[1] = count2;
                            }
                            if (b == 3 || b == 7 || b == 11)
                            {
                                count3 += count;
                                total_count[2] = count3;
                            }
                            if (b == 4 || b == 8 || b == 12)
                            {
                                count4 += count;
                                total_count[3] = count4;
                            }

                        }
                        else
                        {
                            Double count = 0;
                            for (int i = 0; i < gv.Rows.Count; i++)
                            {
                                if (gv.Rows[i]["OVC_MILITARY"].ToString() == unit3[a] && gv.Rows[i]["OVC_IE"].ToString() == "出口" && b < 13)
                                {
                                    //count += Convert.ToDouble(gv.Rows[i][b + 2].ToString());
                                    string strCount = gv.Rows[i][b + 3].ToString();
                                    if (double.TryParse(strCount, out double intTemp))
                                        count += intTemp;
                                }
                            }
                            pdftable.AddCell(new Phrase(Math.Round(count).ToString(), ChFont));
                            if (b == 1 || b == 5 || b == 9)
                            {
                                count1 += count;
                                total_count[0] = count1;
                            }
                            if (b == 2 || b == 6 || b == 10)
                            {
                                count2 += count;
                                total_count[1] = count2;
                            }
                            if (b == 3 || b == 7 || b == 11)
                            {
                                count3 += count;
                                total_count[2] = count3;
                            }
                            if (b == 4 || b == 8 || b == 12)
                            {
                                count4 += count;
                                total_count[3] = count4;
                            }
                        }

                    }
                    else
                    {
                        pdftable.AddCell(new Phrase(Math.Round(total_count[b - 13]).ToString(), ChFont));
                    }
                }
            }

            pdftable.AddCell(new Phrase("備考", ChFont));

            PdfPCell cell4_1 = new PdfPCell(new Phrase("", ChFont));
            cell4_1.Colspan = 16;
            pdftable.AddCell(cell4_1);


            doc.Add(pdftable);
            doc.Close();

            string strFileName = $"{ strDEPT_Name }國軍國外物資接轉月報表.pdf";
            FCommon.DownloadFile(this, strFileName, Memory);

            //Response.Clear();
            //Response.ContentType = "application/octet-stream";
            //Response.AddHeader("Content-Disposition", "attachment; filename=國軍國外物資國防採購室接轉月報表.pdf");
            //Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            //Response.OutputStream.Flush(); ;
            //Response.OutputStream.Close();
            //Response.Flush();
            //Response.End();
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
                    FCommon.Controls_Attributes("readonly", "true", txtODT_OPEN_DATE, txtODT_DOC_DATE);
                    lblDEPT.Text = strDEPT_Name;
                    DateTime dateNow = DateTime.Now;
                    #region 匯入下拉式選單
                    int theYear = FCommon.getTaiwanYear(dateNow);
                    int yearMax = theYear + 2, yearMin = theYear - 15;
                    FCommon.list_dataImportNumber(drpOdtYear, 2, yearMax, yearMin);
                    FCommon.list_setValue(drpOdtYear, theYear.ToString());
                    FCommon.list_dataImportNumber(drpOdtMonth, 2, 12);
                    FCommon.list_setValue(drpOdtMonth, dateNow.Month.ToString().PadLeft(2, '0'));
                    #endregion
                    pnData.Visible = false;

                    #region 取得權限、設定接轉地區
                    bool isSelectSECTION = false;
                    string strOVC_SECTION = "";
                    if (strDEPT_SN.Equals("00N00"))
                    {
                        intAuth = 2;
                        if (!FCommon.getQueryString(this, "section", out strOVC_SECTION, true))
                            strOVC_SECTION = "採購室"; //預設為採購中心
                        FCommon.list_setValue(drpOVC_SECTION, strOVC_SECTION);
                        btnStatisticItem.Enabled = false;
                        isSelectSECTION = true;
                    }
                    else
                        foreach (string strArea in aryAreaList)
                        {
                            if (strDEPT_Name.Contains(strArea))
                            {
                                intAuth = 1;
                                strOVC_SECTION = strArea;
                                btnStatisticItem.Enabled = true;
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
                    //lblOVC_SECTION.Visible = !isSelectSECTION;
                    drpOVC_SECTION.Visible = isSelectSECTION;

                    if (intAuth != 0)
                    {
                        //if (FCommon.getQueryString(this, "id", out string strOVC_BLD_NO, true))
                        //    txtOVC_BLD_NO.Text = strOVC_BLD_NO;
                        //if (FCommon.getQueryString(this, "note", out string strOVC_NOTE, true))
                        //    txtOVC_NOTE.Text = strOVC_NOTE;
                        //dataImport();
                    }
                    #endregion
                    #region 舊方法 intAuth取得權限
                    //if (Session["userid"] != null)
                    //{
                    //    string strUSER_ID = Session["userid"].ToString();
                    //    //搜尋該帳號所有在MTS系統中之權限角色
                    //    //限制條件：無日期限制或日期範圍內；啟用狀態為Y
                    //    var queryAuth =
                    //        from accountAuth in GME.ACCOUNT_AUTH
                    //        where accountAuth.USER_ID.Equals(strUSER_ID)
                    //        where accountAuth.C_SN_SYS.Equals("MTS")
                    //        where accountAuth.END_DATE == null || accountAuth.END_DATE >= dateNow
                    //        where accountAuth.IS_ENABLE.Equals("Y")
                    //        join tAuth in GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("S6")) on accountAuth.C_SN_AUTH equals tAuth.OVC_PHR_ID
                    //        select new
                    //        {
                    //            accountAuth.C_SN_AUTH,
                    //            tAuth.OVC_PHR_DESC
                    //        };
                    //    DataTable dt = CommonStatic.LinqQueryToDataTable(queryAuth);
                    //    int intCount = dt.Rows.Count;
                    //    for (int i = 0; i < intCount; i++)
                    //    {
                    //        DataRow dr = dt.Rows[i];
                    //        string strOVC_PHR_DESC = dr["OVC_PHR_DESC"].ToString();
                    //        if (strOVC_PHR_DESC.Contains("物資接轉處")) //最大權限使用者
                    //        {
                    //            intAuth = 2;
                    //            FCommon.Controls_Attributes("disabled", "true", btnStatistic);
                    //            //btnStatistic.Enabled = false;
                    //            break; //已經為最大權限使用者，不需再判斷其他權限
                    //        }
                    //        else if (strOVC_PHR_DESC.Contains("地區接轉") && intAuth < 1) //若權限小於此權限，才需定義為此權限
                    //            intAuth = 1;
                    //    }
                    //    if (intAuth == 1 && strDEPT_SN.Equals("00N00")) //地區長官 且 單位是00N00 等同物資接轉處長官
                    //        intAuth = 2;
                    //    ViewState["auth"] = intAuth; //儲存權限
                    //    if (intAuth != 0)
                    //    {
                    //        #region 設定接轉地區
                    //        string strOVC_SECTION = "";
                    //        switch (intAuth)
                    //        {
                    //            case 1:
                    //                if (strDEPT_SN.Equals("00N10"))
                    //                    strOVC_SECTION = "基隆地區";
                    //                else if (strDEPT_SN.Equals("00N20"))
                    //                    strOVC_SECTION = "桃園地區";
                    //                else if (strDEPT_SN.Equals("00N30"))
                    //                    strOVC_SECTION = "高雄分遣組";

                    //                lblOVC_SECTION.Text = strOVC_SECTION;
                    //                lblOVC_SECTION.Visible = true;
                    //                drpOVC_SECTION.Visible = false;
                    //                btnStatisticItem.Enabled = true;
                    //                break;
                    //            case 2:
                    //                if (FCommon.getQueryString(this, "section", out strOVC_SECTION, true))
                    //                    FCommon.list_setValue(drpOVC_SECTION, strOVC_SECTION);
                    //                lblOVC_SECTION.Visible = false;
                    //                drpOVC_SECTION.Visible = true;
                    //                btnStatisticItem.Enabled = false;
                    //                break;
                    //        }
                    //        #endregion
                    //        //if (FCommon.getQueryString(this, "id", out string strOVC_BLD_NO, true))
                    //        //    txtOVC_BLD_NO.Text = strOVC_BLD_NO;
                    //        //if (FCommon.getQueryString(this, "note", out string strOVC_NOTE, true))
                    //        //    txtOVC_NOTE.Text = strOVC_NOTE;
                    //        //dataImport();
                    //    }
                    //}
                    //else
                    //    FCommon.MessageBoxShow(this, "尚未登入，請先登入！", "login", true);
                    #endregion
                }

                intAuth = ViewState["auth"] != null ? (int)ViewState["auth"] : 0; //取得權限
                if (intAuth == 0)
                    FCommon.showMessageAuth(this, Path.GetFileName(Request.Path), false); //顯示無權限之訊息，並返回上一頁
            }
        }

        #region ~Click
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }
        protected void btnStatistic_Click(object sender, EventArgs e)
        {
            statistic();
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "月報表重新統計 成功。");
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            //string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
            string strOVC_SECTION = lblOVC_SECTION.Text;
            string strONB_SHIP_AIR = txtONB_SHIP_AIR.Text;
            string strONB_SHIP_SEA = txtONB_SHIP_SEA.Text;
            string strODT_OPEN_DATE = txtODT_OPEN_DATE.Text;
            string strODT_DOC_DATE = txtODT_DOC_DATE.Text;
            string strOVC_DOC_NO = txtOVC_DOC_NO.Text;
            string strOVC_NOTE = txtOVC_NOTE.Text;

            decimal decONB_SHIP_AIR, decONB_SHIP_SEA;
            DateTime dateStart, dateEnd, dateLastMonth, dateODT_OPEN_DATE, dateODT_DOC_DATE;
            getDate(out dateStart, out dateEnd, out dateLastMonth, ref strMessage);
            string strDateStart = FCommon.getDateTime(dateStart);
            #region 錯誤訊息
            if (strOVC_SECTION.Equals(string.Empty))
                strMessage += "<p> 接轉地區不存在！ </p>";
            bool boolONB_SHIP_AIR = FCommon.checkDecimal(strONB_SHIP_AIR, "空運架次", ref strMessage, out decONB_SHIP_AIR);
            bool boolONB_SHIP_SEA = FCommon.checkDecimal(strONB_SHIP_SEA, "海運航次", ref strMessage, out decONB_SHIP_SEA);
            bool boolODT_OPEN_DATE = FCommon.checkDateTime(strODT_OPEN_DATE, "解密日期", ref strMessage, out dateODT_OPEN_DATE);
            bool boolODT_DOC_DATE = FCommon.checkDateTime(strODT_DOC_DATE, "發文日期", ref strMessage, out dateODT_DOC_DATE);
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                try
                {
                    TBGMT_MRP mrp = MTSE.TBGMT_MRP.Where(table => table.OVC_SECTION == strOVC_SECTION).Where(table => DateTime.Compare(table.ODT_MONTH_DATE, dateStart) == 0).FirstOrDefault();
                    if (mrp != null)
                    {
                        if (boolONB_SHIP_AIR) mrp.ONB_SHIP_AIR = decONB_SHIP_AIR;
                        if (boolONB_SHIP_SEA) mrp.ONB_SHIP_SEA = decONB_SHIP_SEA;
                        if (boolODT_OPEN_DATE) mrp.ODT_OPEN_DATE = dateODT_OPEN_DATE;
                        if (boolODT_DOC_DATE) mrp.ODT_DOC_DATE = dateODT_DOC_DATE;
                        mrp.OVC_DOC_NO = strOVC_DOC_NO;
                        mrp.OVC_NOTE = strOVC_NOTE;
                        MTSE.SaveChanges();

                        FCommon.AlertShow(PnMessage, "success", "系統訊息", $"接轉作業月報表：{ strOVC_SECTION }-{ strDateStart } 更新成功！");
                        FCommon.Controls_Attributes("readonly", "true", txtONB_SHIP_AIR, txtONB_SHIP_SEA);
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"接轉作業月報表：{ strOVC_SECTION }-{ strDateStart } 不存在！");
                }
                catch
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "更新月報表失敗，請聯絡工程師！");
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
            DateTime dateStart, dateEnd, dateLastMonth, dateNow = DateTime.Now;
            string strDateFormatTaiwan = "{0}年 {1}月 {2}日";
            string strDateFormatAll = "yyyy-MM-dd HH:mm:ss";
            string strDateNow = FCommon.getDateTime(dateNow, "yyyy-MM-dd HH:mm");
            string strDateNowTaiwan = FCommon.getTaiwanDate(dateNow, strDateFormatTaiwan);
            #region 錯誤訊息
            if (strOVC_SECTION.Equals(string.Empty))
                strMessage += "<p> 接轉地區不存在！ </p>";
            getDate(out dateStart, out dateEnd, out dateLastMonth, ref strMessage);
            #endregion
            string strDateStart = FCommon.getDateTime(dateStart);
            string strDateEnd = FCommon.getDateTime(dateEnd);
            string strDateLastMonth = FCommon.getDateTime(dateLastMonth);

            if (strMessage.Equals(string.Empty))
            {
                string[] strParameterName_start = { ":section", ":start_date" };
                ArrayList aryData_start = new ArrayList();
                aryData_start.Add(strOVC_SECTION);
                aryData_start.Add(strDateStart);
                string[] strParameterName_last = { ":section", ":last_month" };
                ArrayList aryData_last = new ArrayList();
                aryData_last.Add(strOVC_SECTION);
                aryData_last.Add(strDateLastMonth);

                string strONB_SHIP_AIR = "", strONB_SHIP_SEA = "", strODT_OPEN_DATE = "", strODT_DOC_DATE = "", strOVC_DOC_NO = "", strOVC_NOTE = "", strCLOSE_DT = "";
                //判斷當月是否存在
                var query_MRP =
                    from mrp in MTSE.TBGMT_MRP
                    where mrp.OVC_SECTION.Equals(strOVC_SECTION)
                    where mrp.ODT_MONTH_DATE == dateStart
                    select mrp;
                TBGMT_MRP tableMRP = query_MRP.FirstOrDefault();
                //string strSQL_MRP = $@"
                //        select * from TBGMT_MRP
                //        where OVC_SECTION = { strParameterName_start[0] }
                //        and ODT_MONTH_DATE = to_date({ strParameterName_start[1] }, { strDateFormatSQL })
                //    ";
                //DataTable dt_MRP = FCommon.getDataTableFromSelect(strSQL_MRP, strParameterName_start, aryData_start);
                //bool isHasMRP = dt_MRP.Rows.Count > 0;
                bool isHasMRP = tableMRP != null;
                if (isHasMRP)
                {
                    //DataRow dr_MRP = dt_MRP.Rows[0];
                    //strODT_OPEN_DATE = FCommon.getTaiwanDate(dr_MRP["ODT_OPEN_DATE"].ToString(), strDateFormatTaiwan);
                    //strODT_DOC_DATE = FCommon.getTaiwanDate(dr_MRP["ODT_DOC_DATE"].ToString(), strDateFormatTaiwan);
                    //strOVC_DOC_NO = dr_MRP["OVC_DOC_NO"].ToString();
                    //strOVC_NOTE = dr_MRP["OVC_NOTE"].ToString();
                    //strCLOSE_DT = dr_MRP["CLOSE_DT"].ToString();

                    strODT_OPEN_DATE = FCommon.getTaiwanDate(tableMRP.ODT_OPEN_DATE.ToString(), strDateFormatTaiwan);
                    strODT_DOC_DATE = FCommon.getTaiwanDate(tableMRP.ODT_DOC_DATE.ToString(), strDateFormatTaiwan);
                    strOVC_DOC_NO = tableMRP.OVC_DOC_NO;
                    strOVC_NOTE = tableMRP.OVC_NOTE;
                    strCLOSE_DT = FCommon.getDateTime(tableMRP.CLOSE_DT);
                }

                #region 關帳
                if (intAuth == 1 && isHasMRP && string.IsNullOrEmpty(strCLOSE_DT)) //判斷權限及尚未關帳，才需做關帳
                {
                    tableMRP.CLOSE_DT = dateNow;
                    strCLOSE_DT = FCommon.getDateTime(dateNow, strDateFormatAll); //strDateNow
                    MTSE.SaveChanges();
                    //TBGMT_MRP_ITEM_DIFF 不存在
                    if (!getIsDiffItemExist(strOVC_SECTION, strDateStart))
                    { //將最後那次差異試算備份
                        TBGMT_MRP_ITEM_TEMP_DIFF item_temp_diff =
                            MTSE.TBGMT_MRP_ITEM_TEMP_DIFF.Where(table => table.OVC_SECTION.Equals(strOVC_SECTION) && DateTime.Compare(table.ODT_MONTH_DATE, dateStart) == 0).FirstOrDefault();
                        if (item_temp_diff != null)
                        {
                            TBGMT_MRP_ITEM_DIFF item_diff = new TBGMT_MRP_ITEM_DIFF();
                            item_diff.MRP_ITEM_SN = Guid.NewGuid();
                            item_diff.OVC_SECTION = item_temp_diff.OVC_SECTION;
                            item_diff.ODT_MONTH_DATE = item_temp_diff.ODT_MONTH_DATE;
                            item_diff.OVC_MILITARY = item_temp_diff.OVC_MILITARY;
                            item_diff.OVC_IE = item_temp_diff.OVC_IE;
                            item_diff.ONB_BLD_M = item_temp_diff.ONB_BLD_M;
                            item_diff.ONB_QUANITY_M = item_temp_diff.ONB_QUANITY_M;
                            item_diff.ONB_WEIGHT_M = item_temp_diff.ONB_WEIGHT_M;
                            item_diff.ONB_VOLUME_M = item_temp_diff.ONB_VOLUME_M;
                            item_diff.ONB_BLD_C = item_temp_diff.ONB_BLD_C;
                            item_diff.ONB_QUANITY_C = item_temp_diff.ONB_QUANITY_C;
                            item_diff.ONB_WEIGHT_C = item_temp_diff.ONB_WEIGHT_C;
                            item_diff.ONB_VOLUME_C = item_temp_diff.ONB_VOLUME_C;
                            item_diff.ONB_BLD_O = item_temp_diff.ONB_BLD_O;
                            item_diff.ONB_QUANITY_O = item_temp_diff.ONB_QUANITY_O;
                            item_diff.ONB_WEIGHT_O = item_temp_diff.ONB_WEIGHT_O;
                            item_diff.ONB_VOLUME_O = item_temp_diff.ONB_VOLUME_O;
                            item_diff.YEAR = item_temp_diff.YEAR;
                            item_diff.ODT_MODIFY_DATE = item_temp_diff.ODT_MODIFY_DATE;
                            item_diff.ODT_CREATE_DATE = item_temp_diff.ODT_CREATE_DATE;
                            item_diff.OVC_CREATE_LOGIN_ID = item_temp_diff.OVC_CREATE_LOGIN_ID;
                            item_diff.OVC_MODIFY_LOGIN_ID = item_temp_diff.OVC_MODIFY_LOGIN_ID;
                            MTSE.TBGMT_MRP_ITEM_DIFF.Add(item_diff);
                            MTSE.SaveChanges();
                        }
                        //string sql = @"insert into TBGMT_MRP_ITEM_DIFF select * from TBGMT_MRP_ITEM_TEMP_DIFF where OVC_SECTION ='"
                        //    + Section + "' AND ODT_MONTH_DATE = TO_DATE('" + MonthDate.ToShortDateString() + "','yyyy/mm/dd') ";
                    }
                    //TBGMT_MRP_ITEM_LOG 不存在
                    if (!getIsLogItemExist(strOVC_SECTION, strDateStart))
                    {
                        TBGMT_MRP_ITEM item =
                            MTSE.TBGMT_MRP_ITEM.Where(table => table.OVC_SECTION.Equals(strOVC_SECTION) && DateTime.Compare(table.ODT_MONTH_DATE, dateStart) == 0).FirstOrDefault();
                        if (item != null)
                        {
                            TBGMT_MRP_ITEM_LOG item_log = new TBGMT_MRP_ITEM_LOG();
                            item_log.MRP_ITEM_SN = Guid.NewGuid();
                            item_log.OVC_SECTION = item.OVC_SECTION;
                            item_log.ODT_MONTH_DATE = item.ODT_MONTH_DATE;
                            item_log.OVC_MILITARY = item.OVC_MILITARY;
                            item_log.OVC_IE = item.OVC_IE;
                            item_log.ONB_BLD_M = item.ONB_BLD_M;
                            item_log.ONB_QUANITY_M = item.ONB_QUANITY_M;
                            item_log.ONB_WEIGHT_M = item.ONB_WEIGHT_M;
                            item_log.ONB_VOLUME_M = item.ONB_VOLUME_M;
                            item_log.ONB_BLD_C = item.ONB_BLD_C;
                            item_log.ONB_QUANITY_C = item.ONB_QUANITY_C;
                            item_log.ONB_WEIGHT_C = item.ONB_WEIGHT_C;
                            item_log.ONB_VOLUME_C = item.ONB_VOLUME_C;
                            item_log.ONB_BLD_O = item.ONB_BLD_O;
                            item_log.ONB_QUANITY_O = item.ONB_QUANITY_O;
                            item_log.ONB_WEIGHT_O = item.ONB_WEIGHT_O;
                            item_log.ONB_VOLUME_O = item.ONB_VOLUME_O;
                            item_log.YEAR = item.YEAR;
                            item_log.ODT_MODIFY_DATE = item.ODT_MODIFY_DATE;
                            item_log.ODT_CREATE_DATE = item.ODT_CREATE_DATE;
                            item_log.OVC_CREATE_LOGIN_ID = item.OVC_CREATE_LOGIN_ID;
                            item_log.OVC_MODIFY_LOGIN_ID = item.OVC_MODIFY_LOGIN_ID;
                            MTSE.TBGMT_MRP_ITEM_LOG.Add(item_log);
                            MTSE.SaveChanges();
                        }
                        //string sql = @"insert into TBGMT_MRP_ITEM_LOG select * from TBGMT_MRP_ITEM where OVC_SECTION ='"
                        //    + Section + "' AND ODT_MONTH_DATE = TO_DATE('" + MonthDate.ToShortDateString() + "','yyyy/mm/dd') ";
                    }

                    //取得上月關帳日期
                    string strSQL_PrevClosed = $@"
                        select b.CLOSE_DT from
                        (select distinct OVC_SECTION from TBGMT_MRP where OVC_SECTION={ strParameterName_last[0] }) a,
                        (select OVC_SECTION, CLOSE_DT from TBGMT_MRP
                            where OVC_SECTION = { strParameterName_last[0] }
                            and CLOSE_DT is not null
                            and ODT_MONTH_DATE = to_date({ strParameterName_last[1] }, {strDateFormatSQL})) b
                        where a.OVC_SECTION = b.OVC_SECTION (+)
                    ";
                    DataTable dt_PrevClosed = FCommon.getDataTableFromSelect(strSQL_PrevClosed, strParameterName_last, aryData_last);
                    if (dt_PrevClosed.Rows.Count > 0)
                    {
                        string strCLOSE_DT_Prev = FCommon.getDateTime(dt_PrevClosed.Rows[0]["CLOSE_DT"], strDateFormatAll);

                        try
                        {
                            //
                            //執行預存程序 Mrp_Log_Upd_Used_Flag 帶 section=strOVC_SECTION, mrp_date=strDateStart, pri_dt=strCLOSE_DT_Prev, this_dt=strCLOSE_DT
                            System.Data.Entity.Core.Objects.ObjectParameter rTN_MSG =
                                new System.Data.Entity.Core.Objects.ObjectParameter("rTN_MSG", typeof(string));
                            MTSE.MRP_LOG_UPD_USED_FLAG(strOVC_SECTION, strDateStart, strCLOSE_DT_Prev, strCLOSE_DT, rTN_MSG);
                            string myKey = rTN_MSG.Value.ToString();
                            if (myKey.Equals("SUCCESS"))
                                FCommon.AlertShow(PnMessage, "success", "系統訊息", "關帳相關Log 產生成功。");
                            else
                                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "<p> 關帳相關Log 產生成功！ </p>" + myKey);
                        }
                        catch
                        {
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "關帳相關Log 產生錯誤！");
                        }
                    }
                    FCommon.Controls_Attributes("onClick", btnPrint);
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "關帳完成。");
                }
                #endregion
                string strMessage_Section = "";
                if (strOVC_SECTION.Equals("採購室"))
                    if (getIsAllClosedtExist(strDateStart, out strMessage_Section))
                    {
                        try
                        {
                            //
                            //執行預存程序 mrp_Item_Diff_summarize 帶 mrp_date=strDateStart
                            System.Data.Entity.Core.Objects.ObjectParameter rTN_MSG =
                                new System.Data.Entity.Core.Objects.ObjectParameter("rTN_MSG", typeof(string));
                            MTSE.MRP_ITEM_DIFF_SUMMARIZE(strDateStart, rTN_MSG);
                            string myKey = rTN_MSG.Value.ToString();

                            FCommon.AlertShow(PnMessage, "success", "系統訊息", strOVC_SECTION + "差異數彙總 成功。");
                        }
                        catch
                        {
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "差異數彙總 失敗！");
                        }
                    }
                if (strMessage_Section.Equals(string.Empty))
                {
                    OriginPrint(strDateStart, strDateEnd, strODT_OPEN_DATE, strODT_DOC_DATE, strOVC_DOC_NO, strDateNow, strDateNowTaiwan); //原有列印
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"本月尚有{ strMessage_Section }接轉組未關帳！");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void btnQueryDiff_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            //string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
            string strOVC_SECTION = lblOVC_SECTION.Text;
            DateTime dateNow = DateTime.Now;
            #region 錯誤訊息
            if (strOVC_SECTION.Equals(string.Empty))
                strMessage += "<p> 接轉地區不存在！ </p>";
            getDate(out DateTime dateStart, out DateTime dateEnd, out DateTime dateLastMonth, ref strMessage);
            #endregion
            string strDateStart = FCommon.getDateTime(dateStart);
            string strDateEnd = FCommon.getDateTime(dateEnd);
            string strDateLastMonth = FCommon.getDateTime(dateLastMonth);

            if (strMessage.Equals(string.Empty))
            {
                string[] strParameterName = { ":section", ":start_date", ":end_date" };
                ArrayList aryData = new ArrayList();
                aryData.Add(strOVC_SECTION);
                aryData.Add(strDateStart);
                aryData.Add(strDateEnd);
                string[] strParameterName_start = { strParameterName[0], strParameterName[1] };
                ArrayList aryData_start = new ArrayList();
                aryData_start.Add(strOVC_SECTION);
                aryData_start.Add(strDateStart);
                string strSQL_PortList = $@"
                    select OVC_PHR_ID from TBM1407 where OVC_PHR_CATE='TR' and OVC_PHR_PARENTS={ strParameterName[0] }
                ";
                string strSQL_DateBetween = $@"
                    between to_date({ strParameterName[1] }, { strDateFormatSQL }) and to_date({ strParameterName[2] }, { strDateFormatSQL })
                ";
                string strWhere_start = $@"
                    where OVC_SECTION = { strParameterName[0] }
                    and ODT_MONTH_DATE = to_date({ strParameterName_start[1] }, { strDateFormatSQL })
                ";

                string strTableName = "";
                bool isLeastNoe = true; //是否至少有一筆資料

                if (strOVC_SECTION.Equals("採購室"))
                {
                    string strMessage_Section;
                    if (!getIsAllClosedtExist(strDateStart, out strMessage_Section))
                    {
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"本月尚有{ strMessage_Section }接轉組未關帳！");
                        return;
                    }
                    else
                    {
                        try
                        {
                            //
                            //執行預存程序 mrp_Item_Diff_summarize 帶 mrp_date=strDateStart
                            System.Data.Entity.Core.Objects.ObjectParameter rTN_MSG =
                                new System.Data.Entity.Core.Objects.ObjectParameter("rTN_MSG", typeof(string));
                            MTSE.MRP_ITEM_DIFF_SUMMARIZE(strDateStart, rTN_MSG);
                            string myKey = rTN_MSG.Value.ToString();

                            FCommon.AlertShow(PnMessage, "success", "系統訊息", strOVC_SECTION + "差異數彙總 成功。");
                        }
                        catch
                        {
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "差異數彙總 失敗！");
                        }
                    }
                    strTableName = "TBGMT_MRP_ITEM_DIFF";
                }
                else
                {
                    if (!getIsClosedtExist(strOVC_SECTION, strDateStart)) //本月尚未關帳
                    {
                        strTableName = "TBGMT_MRP_ITEM_TEMP_DIFF";
                        isLeastNoe = false;
                    }
                    else
                        strTableName = "TBGMT_MRP_ITEM_DIFF";
                }
                string strSQL_MRP_ITEM_DIFF = $@"
                    select
                    OVC_SECTION, ODT_MONTH_DATE, OVC_MILITARY, OVC_IE,
                    ONB_BLD_M, ONB_QUANITY_M, ONB_WEIGHT_M, ONB_VOLUME_M,
                    ONB_BLD_C, ONB_QUANITY_C, ONB_WEIGHT_C, ONB_VOLUME_C,
                    ONB_BLD_O, ONB_QUANITY_O, ONB_WEIGHT_O, ONB_VOLUME_O
                    from { strTableName }
                    { strWhere_start }
                    order by OVC_IE desc, OVC_MILITARY
                ";
                //string sql = "SELECT OVC_SECTION, ODT_MONTH_DATE, OVC_MILITARY, OVC_IE, ONB_BLD_M, ONB_QUANITY_M, " +
                //    "ONB_WEIGHT_M, ONB_VOLUME_M, ONB_BLD_C, ONB_QUANITY_C, ONB_WEIGHT_C, ONB_VOLUME_C, ONB_BLD_O, " +
                //    "ONB_QUANITY_O, ONB_WEIGHT_O, ONB_VOLUME_O FROM TBGMT_MRP_ITEM_TEMP_DIFF WHERE";
                //sql += " OVC_SECTION = '" + Section + "' AND ";
                //sql += " ODT_MONTH_DATE = TO_DATE('" + MonthDate.ToShortDateString() + "','yyyy/mm/dd') ORDER BY OVC_IE DESC , OVC_MILITARY";

                DataTable dt_MRP_ITEM_DIFF = FCommon.getDataTableFromSelect(strSQL_MRP_ITEM_DIFF, strParameterName_start, aryData_start);
                if (isLeastNoe) getLeast_MRP_ITEM(dt_MRP_ITEM_DIFF, strOVC_SECTION, strDateStart); //取得至少一筆資料
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_MRP_ITEM, dt_MRP_ITEM_DIFF);
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "差異數查詢 成功。");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);

            #region 原始查詢
            //DateTime dateStart = Convert.ToDateTime((Convert.ToInt16(drpOdtYear.SelectedValue) + 1911).ToString() + "-" + drpOdtMonth.SelectedValue + "-01");
            //var query = (from temp_diff in MTSE.TBGMT_MRP_ITEM_TEMP_DIFF
            //             where temp_diff.ODT_MONTH_DATE == dateStart
            //             select new
            //             {
            //                 OVC_SECTION = temp_diff.OVC_SECTION,
            //                 OVC_MILITARY = temp_diff.OVC_MILITARY,
            //                 OVC_IE = temp_diff.OVC_IE,
            //                 ONB_QUANITY_M = temp_diff.ONB_QUANITY_M,
            //                 ONB_WEIGHT_M = temp_diff.ONB_WEIGHT_M,
            //                 ONB_VOLUME_M = temp_diff.ONB_VOLUME_M,
            //                 ONB_BLD_M = temp_diff.ONB_BLD_M,
            //                 ONB_QUANITY_C = temp_diff.ONB_QUANITY_C,
            //                 ONB_WEIGHT_C = temp_diff.ONB_WEIGHT_C,
            //                 ONB_VOLUME_C = temp_diff.ONB_VOLUME_C,
            //                 ONB_BLD_C = temp_diff.ONB_BLD_C,
            //                 ONB_QUANITY_O = temp_diff.ONB_QUANITY_O,
            //                 ONB_WEIGHT_O = temp_diff.ONB_WEIGHT_O,
            //                 ONB_VOLUME_O = temp_diff.ONB_VOLUME_O,
            //                 ONB_BLD_O = temp_diff.ONB_BLD_O
            //             }).ToList();
            //var q = query.GroupBy(cc =>
            //    new
            //    {
            //        cc.OVC_SECTION,
            //        cc.OVC_MILITARY,
            //        cc.OVC_IE
            //    }
            //    ).Select(dd =>
            //    new
            //    {
            //        OVC_SECTION = dd.Key.OVC_SECTION,
            //        OVC_MILITARY = dd.Key.OVC_MILITARY,
            //        OVC_IE = dd.Key.OVC_IE,
            //        ONB_QUANITY_M = string.Join("", dd.Select(ee => ee.ONB_QUANITY_M.ToString()).ToList()),
            //        ONB_WEIGHT_M = string.Join("", dd.Select(ee => ee.ONB_WEIGHT_M.ToString()).ToList()),
            //        ONB_VOLUME_M = string.Join("", dd.Select(ee => ee.ONB_VOLUME_M.ToString()).ToList()),
            //        ONB_BLD_M = string.Join("", dd.Select(ee => ee.ONB_BLD_M.ToString()).ToList()),
            //        ONB_QUANITY_C = string.Join("", dd.Select(ee => ee.ONB_QUANITY_C.ToString()).ToList()),
            //        ONB_WEIGHT_C = string.Join("", dd.Select(ee => ee.ONB_WEIGHT_C.ToString()).ToList()),
            //        ONB_VOLUME_C = string.Join("", dd.Select(ee => ee.ONB_VOLUME_C.ToString()).ToList()),
            //        ONB_BLD_C = string.Join("", dd.Select(ee => ee.ONB_BLD_C.ToString()).ToList()),
            //        ONB_QUANITY_O = string.Join("", dd.Select(ee => ee.ONB_QUANITY_O.ToString()).ToList()),
            //        ONB_WEIGHT_O = string.Join("", dd.Select(ee => ee.ONB_WEIGHT_O.ToString()).ToList()),
            //        ONB_VOLUME_O = string.Join("", dd.Select(ee => ee.ONB_VOLUME_O.ToString()).ToList()),
            //        ONB_BLD_O = string.Join("", dd.Select(ee => ee.ONB_BLD_O.ToString()).ToList()),
            //    }).OrderByDescending(z => z.OVC_IE).OrderBy(u => u.OVC_MILITARY);

            //DataTable dt = CommonStatic.LinqQueryToDataTable(q);
            //ViewState["hasRows"] = FCommon.GridView_dataImport(GV__TBGMT_MRP_ITEM, dt);
            #endregion
        }
        protected void btnStatisticItem_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            //string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
            string strOVC_SECTION = lblOVC_SECTION.Text;
            #region 錯誤訊息
            if (strOVC_SECTION.Equals(string.Empty))
                strMessage += "<p> 接轉地區不存在！ </p>";
            getDate(out DateTime dateStart, out DateTime dateEnd, out DateTime dateLastMonth, ref strMessage);
            #endregion
            string strDateStart = FCommon.getDateTime(dateStart);
            //string strDateEnd = FCommon.getDateTime(dateEnd);

            if (strMessage.Equals(string.Empty))
            {
                setDeleteAllItem(strOVC_SECTION, dateStart);
                setGetDummyItems_CreateItems(strOVC_SECTION, dateStart, dateEnd);

                string strTable1 = "TBGMT_MRP_ITEM", strTable2 = "TBGMT_MRP_ITEM_TEMP_DIFF";
                dataImport_GV_TBGMT_MRP_ITEM(strTable1, strTable2);
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "重新統計細項資料 成功。");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void btnSummaryItem_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strOVC_SECTION = drpOVC_SECTION.Visible ? drpOVC_SECTION.SelectedValue : lblOVC_SECTION.Text;
            DateTime dateStart, dateEnd, dateLastMonth, dateNow = DateTime.Now;
            #region 錯誤訊息
            if (strOVC_SECTION.Equals(string.Empty))
                strMessage += "<p> 接轉地區不存在！ </p>";
            getDate(out dateStart, out dateEnd, out dateLastMonth, ref strMessage);
            #endregion
            string strDateStart = FCommon.getDateTime(dateStart);
            //string strDateEnd = FCommon.getDateTime(dateEnd);

            if (strMessage.Equals(string.Empty))
            {
                try
                {
                    //
                    //執行預存程序 mrp_sta_summarize 帶 mrp_date=strDateStart
                    System.Data.Entity.Core.Objects.ObjectParameter rTN_MSG =
                        new System.Data.Entity.Core.Objects.ObjectParameter("rTN_MSG", typeof(string));
                    MTSE.MRP_STA_SUMMARIZE(strDateStart, rTN_MSG);
                    string myKey = rTN_MSG.Value.ToString();
                    //if (myKey.Equals("SUCCESS"))
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "彙總 成功。");
                }
                catch
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "彙總 失敗！");
                }

                string strTable1 = "TBGMT_MRP_ITEM", strTable2 = "TBGMT_MRP_ITEM_TEMP_DIFF";
                dataImport_GV_TBGMT_MRP_ITEM(strTable1, strTable2);
                //FCommon.AlertShow(PnMessage, "success", "系統訊息", "採購中心資料彙總 成功。");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        #endregion

        #region ~GV_TBGMT_MRP_ITEM
        //protected void GV_TBGMT_MRP_ITEM_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    GridView theGridView = (GridView)sender;
        //    GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
        //    int gvrIndex = gvr.RowIndex;
        //    //string id = theGridView.DataKeys[gvrIndex].Value.ToString();

        //    switch (e.CommandName)
        //    {
        //        case "DataEdit": //編輯
        //            theGridView.EditIndex = gvrIndex;
        //            dataImport_GV_TBGMT_MRP_ITEM(null, null);
        //            break;
        //        case "DataSave": //儲存
        //            theGridView.EditIndex = -1;
        //            dataImport_GV_TBGMT_MRP_ITEM(null, null);
        //            break;
        //        case "DataDelete": //刪除
        //            theGridView.EditIndex = -1;
        //            dataImport_GV_TBGMT_MRP_ITEM(null, null);
        //            break;
        //        case "DataCancel": //取消
        //            theGridView.EditIndex = -1;
        //            dataImport_GV_TBGMT_MRP_ITEM(null, null);
        //            break;
        //    }
        //}

        protected void GV_TBGMT_MRP_ITEM_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        #endregion
    }
}