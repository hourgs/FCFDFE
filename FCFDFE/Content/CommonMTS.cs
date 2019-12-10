using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MTSModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace FCFDFE.Content
{
    public class CommonMTS
    {
        #region 匯入下拉式選單
        #region 資料庫
        public static void list_dataImport_PORT(DropDownList drpOVC_SHIP_COMPANY, DropDownList drpOVC_SEA_OR_AIR, DropDownList drpOVC_PORT) //承運航商，海空運別，啟運/抵達港埠(國內)
        {
            Common FCommon = new Common();
            MTSEntities MTSE = new MTSEntities();
            bool isOther = false; //是否非合約航商
            string strOVC_SHIP_COMPANY = drpOVC_SHIP_COMPANY.SelectedValue, strOVC_PORT_TYPE;
            #region 啟運港埠判斷  
            if (strOVC_SHIP_COMPANY.Contains("航空") || strOVC_SHIP_COMPANY.Contains("空運"))
            {
                strOVC_PORT_TYPE = "機場";
                FCommon.list_setValue(drpOVC_SEA_OR_AIR, "空運");
            }
            else if (strOVC_SHIP_COMPANY.Contains("海運"))
            {
                strOVC_PORT_TYPE = "海港";
                FCommon.list_setValue(drpOVC_SEA_OR_AIR, "海運");
            }
            else
            {
                if (drpOVC_SEA_OR_AIR.SelectedValue.Equals("空運"))
                    strOVC_PORT_TYPE = "機場";
                else
                    strOVC_PORT_TYPE = "海港";
                isOther = true;
            }
            var query =
                from port in MTSE.TBGMT_PORTS
                where port.OVC_IS_ABROAD.Equals("國內")
                where port.OVC_PORT_TYPE.Equals(strOVC_PORT_TYPE)
                select port;
            DataTable dtTBGMT_PORTS = CommonStatic.LinqQueryToDataTable(query);
            FCommon.list_dataImport(drpOVC_PORT, dtTBGMT_PORTS, "OVC_PORT_CHI_NAME", "OVC_PORT_CDE", false);
            #endregion
            if (isOther)
            {
                FCommon.Controls_Attributes("disabled", drpOVC_SEA_OR_AIR);
                FCommon.Controls_Attributes("Style", drpOVC_SEA_OR_AIR);
            }
            else
            {
                FCommon.Controls_Attributes("disabled", "true", drpOVC_SEA_OR_AIR);
                FCommon.Controls_Attributes("Style", "background-color:#DDDDDD;", drpOVC_SEA_OR_AIR);
            }
        }
        public static void list_dataImport_PORT(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "") //啟運/抵達港埠(國內)
        {
            Common FCommon = new Common();
            MTSEntities MTSE = new MTSEntities();
            var query =
                from port in MTSE.TBGMT_PORTS
                where port.OVC_IS_ABROAD.Equals("國內")
                select port;
            DataTable dt = CommonStatic.ListToDataTable(query);
            FCommon.list_dataImport(list, dt, "OVC_PORT_CHI_NAME", "OVC_PORT_CDE", textField, valueField, isShowFirst);
        }
        //接轉地區
        public static void list_dataImport_TransferArea(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            GMEntities GME = new GMEntities();
            string strKeyField = "OVC_PHR_PARENTS";
            //var query =
            //    from ports in GME.TBM1407
            //    where ports.OVC_PHR_CATE.Equals("TR")
            //    select ports;
            //DataTable dtSource = CommonStatic.LinqQueryToDataTable(query);
            //DataTable dtDistinct = dtSource.DefaultView.ToTable(true, new string[] { strKeyField });

            var query =
                from ports in GME.TBM1407.AsEnumerable()
                where ports.OVC_PHR_CATE.Equals("TR") || ports.OVC_PHR_CATE != null
                select ports;
            DataTable dtSource = CommonStatic.LinqQueryToDataTable(query);
            DataTable dtDistinct = dtSource.DefaultView.ToTable(true, new string[] { strKeyField });
            FCommon.list_dataImport(list, dtDistinct, strKeyField, strKeyField, textField, valueField, isShowFirst);
        }
        //幣別
        public static void list_dataImport_CURRENCY(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            MTSEntities MTSE = new MTSEntities();
            var query = from ports in MTSE.TBGMT_CURRENCY select ports;
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            FCommon.list_dataImport(list, dt, "OVC_CURRENCY_NAME", "OVC_CURRENCY_CODE", textField, valueField, isShowFirst);
            //預設幣別
            string strDefaultCurrency = VariableMTS.strDefaultCurrency;
            FCommon.list_setValue(list, strDefaultCurrency);
        }
        //幣別
        public static void list_dataImport_CURRENCY(ListControl list, bool isShowFirst, string[] strExcepts, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            MTSEntities MTSE = new MTSEntities();
            var query = from currency in MTSE.TBGMT_CURRENCY select currency;
            if (strExcepts.Length > 0)
                query = query.Where(table => !strExcepts.Contains(table.OVC_CURRENCY_CODE));
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            FCommon.list_dataImport(list, dt, "OVC_CURRENCY_NAME", "OVC_CURRENCY_CODE", textField, valueField, isShowFirst);
            //預設幣別
            string strDefaultCurrency = VariableMTS.strDefaultCurrency;
            FCommon.list_setValue(list, strDefaultCurrency);
        }
        //軍種
        public static void list_dataImport_DEPT_CLASS(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            MTSEntities MTSE = new MTSEntities();
            var query =
                from ports in MTSE.TBGMT_DEPT_CLASS
                orderby ports.ONB_SORT
                select ports;
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            FCommon.list_dataImport(list, dt, "OVC_CLASS_NAME", "OVC_CLASS", textField, valueField, isShowFirst);
            FCommon.list_setValue(list, "A");
        }
        public enum COMPANY_TYPE { InsuranceCompany = 1, EnteringHoldVendor = 2 };
        //保險公司
        public static void list_dataImport_COMPANY(ListControl list, COMPANY_TYPE type, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            MTSEntities MTSE = new MTSEntities();
            string strType = ((int)type).ToString();
            var query =
                from company in MTSE.TBGMT_COMPANY
                    //join insrate in MTSE.TBGMT_INSRATE on company.CO_SN equals insrate.OVC_CO_SN
                    //    join i in MTSE.TBGMT_INSRATE on c.OVC_COMPANY equals i.OVC_INSCOMPNAY
                where company.OVC_CO_TYPE.Equals(strType)
                select company;
            if (type.Equals(COMPANY_TYPE.InsuranceCompany)) //保險公司
                query = from company in query
                        join insrate in MTSE.TBGMT_INSRATE on company.CO_SN equals insrate.OVC_CO_SN
                        select company;
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            FCommon.list_dataImport(list, dt, "OVC_COMPANY", "CO_SN", textField, valueField, isShowFirst);
        }
        //承運航商
        public static void list_dataImport_SHIP_COMPANY(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            //string[] strList = VariableMTS.list_OVC_SHIP_COMPANY;
            //FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);

            MTSEntities MTSE = new MTSEntities();
            var query =
                from company in MTSE.TBGMT_COMPANY
                where company.OVC_CO_TYPE.Equals("3")
                select company;
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            FCommon.list_dataImport(list, dt, "OVC_COMPANY", "OVC_COMPANY", textField, valueField, isShowFirst);
        }
        //清運方式
        public static void list_dataImport_TRANS_TYPE(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            //string[] strList = { "鐵運", "零星", "軍車", "自提", "快捷", "承攬商", "委商" };
            //FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);

            MTSEntities MTSE = new MTSEntities();
            var query =
                from clean in MTSE.TBGMT_CLEARANCE
                select clean;
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            FCommon.list_dataImport(list, dt, "OVC_WAY", "OVC_WAY", textField, valueField, isShowFirst);
        }
        //航線
        public static void list_dataImport_ROUTE(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            MTSEntities MTSE = new MTSEntities();
            var query =
                from port in MTSE.TBGMT_PORTS
                where port.OVC_ROUTE != null
                group port by port.OVC_ROUTE into g
                select new
                {
                    OVC_ROUTE = g.Key
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            FCommon.list_dataImport(list, dt, "OVC_ROUTE", "OVC_ROUTE", textField, valueField, isShowFirst);
        }
        #endregion
        #region 字串型態
        //承運航商 是非合約航商
        public static void list_dataImport_SHIP_COMPANY2(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList = { "合約航商", "非合約航商" };
            FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);
        }
        //海空運別
        public static void list_dataImport_SEA_OR_AIR(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList = { "空運", "海運" };
            FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);
        }
        //戰略性高科技貨品
        public static void list_dataImport_IS_STRATEGY(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList = { "是", "否" };
            FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);
        }
        //機敏軍品
        public static void list_dataImport_IS_SECURITY(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strListText = { "是", "否" };
            string[] strListValue = { "1", "0" };
            FCommon.list_dataImport(list, strListText, strListValue, textField, valueField, isShowFirst);
            if (!isShowFirst)
                FCommon.list_setValue(list, "0"); //預設為 否
        }
        //付款方式(提單 BLD用)
        public static void list_dataImport_PAYMENT_TYPE_BLD(ListControl list, bool isShowFirst = false, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strListText = { "PREPAID(預付) 無須投保", "COLLECT(到付) 辦理投保作業" };
            string[] strListValue = { "預付", "到付" };
            FCommon.list_dataImport(list, strListText, strListValue, textField, valueField, isShowFirst);
            //FCommon.list_setValue(list, "到付"); //預設為 到付
        }
        //付款方式(外運資料表 EDF用)
        public static void list_dataImport_PAYMENT_TYPE_EDF(ListControl list, bool isShowFirst = false, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strListText = { "PREPAID 預付 (軍種年度運保費項下支付)", "COLLECT 到付 (收貨人支付)" };
            string[] strListValue = { "預付", "到付" };
            FCommon.list_dataImport(list, strListText, strListValue, textField, valueField, isShowFirst);
            FCommon.list_setValue(list, "預付"); //預設為 預付
        }
        //付款方式(外運資料表 EDF用)－是否投保
        public static void list_dataImport_IS_PAY(ListControl list, bool isShowFirst = false, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strListText = { "投保", "不投保" };
            string[] strListValue = { "1", "0" };
            FCommon.list_dataImport(list, strListText, strListValue, textField, valueField, isShowFirst);
            FCommon.list_setValue(list, "0"); //預設為 不投保
        }
        //付款方式(外運資料表 EDF用)－其他
        public static void list_dataImport_PAYMENT_TYPE_Other(ListControl list, bool isShowFirst = false, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList = { "契約航商", "代理商", "快遞" }; //Coperator Agents Delivery
            FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);
            FCommon.list_setValue(list, "契約航商"); //預設為 契約航商
        }
        //件數單位
        public static void list_dataImport_QUANITY_UNIT(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList = VariableMTS.list_QUANITY_UNIT;
            FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);
        }
        //體積單位
        public static void list_dataImport_VOLUME_UNIT(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList = { "CF", "CBM" };
            FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);
        }
        //重量單位
        public static void list_dataImport_WEIGHT_UNIT(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList = { "KG" };
            FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);
        }
        //重量單位
        public static void list_dataImport_WEIGHT_UNIT2(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList = { "公斤", "公噸" };
            FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);
        }
        //裝貨區分
        public static void list_dataImport_CONTAINER_TYPE(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList = { "CY", "N/A", "CFS" };
            FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);
        }
        //進艙廠商
        public static void list_dataImport_STORED_COMPANY(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList = { "N/A", "欣隆", "怡聯", "高鳳", "新隆", "好好國際", "中國貨運", "長榮海運", "長榮儲運", "陽明海運", "空運-華儲", "空運-榮儲" };
            FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);
        }
        //交貨和保險條件
        public static void list_dataImport_DELIVERY_CONDITION(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList = { "FOB", "FCA", "EX WORK", "CPT", "CIP", "CIF", "CFR" };
            FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);
            FCommon.list_setValue(list, "EX WORK");
        }
        //軍售或商購
        public static void list_dataImport_PURCHASE_TYPE(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList = { "美軍售", "法軍售", "美商購", "法商購" };
            FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);
        }
        //需辦投保
        public static void list_dataImport_STATUS(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList_Text = { "是", "否" };
            string[] strList_Value = { "B", "B-" };
            FCommon.list_dataImport(list, strList_Text, strList_Value, textField, valueField, isShowFirst);
        }
        //保險條件
        public static void list_dataImport_INS_CONDITION(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList = { "全險", "在台內陸險", "在外內陸險", "兵險" };
            FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);
            if (list is CheckBoxList) //如果為複選清單，則預設為全選
                foreach (ListItem item in list.Items)
                    item.Selected = true;
        }
        //索賠情形
        public static void list_dataImport_CLAIM_CONDITION(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList = { "索賠", "索賠權保留" };
            FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);
        }
        public static void list_dataImport_CLAIM_CONDITION2(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList = { "索賠", "理賠", "撤案", "結案" };
            FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);
        }
        //需備索賠文件
        public static void list_dataImport_GET_FILE(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList = { "索賠函", "短卸/事故證明單或公證報告", "發票或單價明細表", "裝箱單", "提單資料" };
            FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);
            if (list is CheckBoxList) //如果為複選清單，則預設為全選
                foreach (ListItem item in list.Items)
                    item.Selected = true;
        }
        //損失原因
        public static void list_dataImport_CLAIM_REASON(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList = { "短少", "破損", VariableMTS.strOtherText };
            FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);
        }
        //作業進度
        public static void list_dataImport_APPROVE_STATUS(ListControl list, bool isShowFirst, string textField = "未輸入", string valueField = "")
        {
            Common FCommon = new Common();
            string[] strList = { "申請保留索賠權", "已申請保留索賠權", "申請理賠", "已理賠", "撤銷(美軍獲賠)", "已撤銷", "撤銷(資料錯誤)" };
            FCommon.list_dataImport(list, strList, strList, textField, valueField, isShowFirst);
        }
        #endregion
        #endregion

        #region 帶碼轉換
        public static string getPortName(object objOVC_PORT_CDE) //取得港埠名稱
        {
            MTSEntities MTSE = new MTSEntities();
            string strOVC_PORT_CHI_NAME = "";
            if (objOVC_PORT_CDE != null)
            {
                string strOVC_PORT_CDE = objOVC_PORT_CDE.ToString();
                TBGMT_PORTS port = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(strOVC_PORT_CDE)).FirstOrDefault();
                if (port != null) strOVC_PORT_CHI_NAME = port.OVC_PORT_CHI_NAME;
            }
            return strOVC_PORT_CHI_NAME;
        }
        public static string getClassName(object objOVC_CLASS) //取得軍種名稱
        {
            MTSEntities MTSE = new MTSEntities();
            string strOVC_CLASS_NAME = "";
            if (objOVC_CLASS != null)
            {
                string strOVC_CLASS = objOVC_CLASS.ToString();
                TBGMT_DEPT_CLASS dept_class = MTSE.TBGMT_DEPT_CLASS.Where(table => table.OVC_CLASS.Equals(strOVC_CLASS)).FirstOrDefault();
                if (dept_class != null) strOVC_CLASS_NAME = dept_class.OVC_CLASS_NAME;
            }
            return strOVC_CLASS_NAME;
        }
        public static string getCurrencyName(object objOVC_CURRENCY_CODE) //取得貨幣名稱
        {
            MTSEntities MTSE = new MTSEntities();
            string strOVC_CURRENCY_NAME = "";
            if (objOVC_CURRENCY_CODE != null)
            {
                string strOVC_CURRENCY_CODE = objOVC_CURRENCY_CODE.ToString();
                TBGMT_CURRENCY currency = MTSE.TBGMT_CURRENCY.Where(table => table.OVC_CURRENCY_CODE.Equals(strOVC_CURRENCY_CODE)).FirstOrDefault();
                if (currency != null) strOVC_CURRENCY_NAME = currency.OVC_CURRENCY_NAME;
            }
            return strOVC_CURRENCY_NAME;
        }
        #endregion

#region 計算公式
        public static decimal getRate(string strCurrency) //回傳 -1 為無匯率
        {
            MTSEntities MTSE = new MTSEntities();
            if (strCurrency == "NTD")
                return 1;
            else
            {
                DateTime dateNow = DateTime.Now;
                DateTime dateNowM = dateNow.AddMonths(-1);
                var query = MTSE.TBGMT_CURRENCY_RATE
                    .Where(c => c.OVC_CURRENCY_CODE.Equals(strCurrency))
                    .Where(c => c.ODT_DATE <= dateNow && c.ODT_DATE >= dateNowM)
                    .OrderByDescending(c => c.ODT_DATE).FirstOrDefault();
                if (query != null)
                    return query.ONB_RATE;
                else //找不到近一個月內的匯率
                    return -1;
            }
        }
#endregion
    }
}