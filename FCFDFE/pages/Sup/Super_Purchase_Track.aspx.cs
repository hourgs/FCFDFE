using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.Sup
{
    public partial class Super_Purchase_Track : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string[] strField = { "OVC_PURCH", "OVC_PUR_IPURCH", "OVC_AGENT_UNIT", "OVC_PUR_USER", "OVC_DAPPLY", "OVC_DAUDIT", "OVC_PURCH_OK" };

        protected void Page_Load(object sender, EventArgs e)
        {
                
        }

        protected void GV_Purchase_Track_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void query_Click(object sender, EventArgs e)
        {
            importdata();



        }

        private void importdata()
        {
            #region 計評待分案
            //1301有1202沒有
            var query =
                (from t1301 in mpms.TBM1301
                 where t1301.OVC_PERMISSION_UPDATE.Equals("N")
                 && t1301.OVC_DPROPOSE != null
                 select t1301.OVC_PURCH)
                .Except
                (from t1202 in mpms.TBM1202
                 select t1202.OVC_PURCH);
            //資料傳送日
            var queryDateT =
                from t in mpms.TBM1114.Where(o => o.OVC_REMARK.Contains("申購單位將購案轉呈"))
                group t by t.OVC_PURCH into g
                select new
                {
                    g.Key,
                    Date = g.Max(t => t.OVC_DATE)
                };
            var query_3_1 =
                (from t in query
                join t1301 in mpms.TBM1301 on t equals t1301.OVC_PURCH
                join t1114 in queryDateT on t equals t1114.Key
                select new
                {
                    PURCH=t1301.OVC_PURCH,
                    IPURCH=t1301.OVC_PUR_IPURCH,
                    STATE_TYPE="計畫評核",
                    STATE_NOW = "待分案",
                    USER_NOW =""
                }).ToList();

            #endregion
            #region 計評承辦中
            var query1 =
                from t in mpms.TBM1202
                group t by new
                {
                    t.OVC_PURCH,
                    t.OVC_CHECK_UNIT,
                    t.OVC_CHECKER,

                } into g
                select new
                {
                    ONB_CHECK_TIMES = g.Max(o => o.ONB_CHECK_TIMES),
                    MinDate = g.Min(o => o.OVC_DRECEIVE),
                    MaxDate = g.Max(o => o.OVC_DRECEIVE),
                    g.Key.OVC_PURCH,
                    g.Key.OVC_CHECK_UNIT,
                    g.Key.OVC_CHECKER,
                };

            
                var query2 =
                    from t in query1
                    join t1202 in mpms.TBM1202
                    on new
                    {
                        t.OVC_PURCH,
                        t.ONB_CHECK_TIMES,
                        t.OVC_CHECK_UNIT
                    }
                    equals new
                    {
                        t1202.OVC_PURCH,
                        t1202.ONB_CHECK_TIMES,
                        t1202.OVC_CHECK_UNIT
                    }
                    join t1301 in mpms.TBM1301 on t.OVC_PURCH equals t1301.OVC_PURCH
                    where string.IsNullOrEmpty(t1301.OVC_PUR_DCANPO)
                    select new
                    {
                        t.ONB_CHECK_TIMES,
                        t.MinDate,
                        t.MaxDate,
                        t.OVC_PURCH,
                        t1301.OVC_PUR_IPURCH,
                        t1301.OVC_PUR_NSECTION,
                        t1301.OVC_PUR_AGENCY,
                        t.OVC_CHECK_UNIT,
                        t.OVC_CHECKER,
                        t1202.OVC_DREJECT,
                        t1202.OVC_CHECK_OK,
                        t1301.IS_PLURAL_BASIS,
                        t1301.OVC_PERMISSION_UPDATE,
                    };
                var queryLlist =
                    from t in query2
                    select new { t.OVC_PURCH };

                var qStatus =
                from t in mpms.TBMSTATUS
                where t.OVC_STATUS != "19"
                group t by t.OVC_PURCH into g
                select new
                {
                    OVC_PURCH = g.Key,
                };
                var finalList = queryLlist.Except(qStatus);

                var query_3_2 =
                (from t in query2
                join b in finalList on t.OVC_PURCH equals b.OVC_PURCH
                select new
                {
                    PURCH=t.OVC_PURCH,
                    IPURCH=t.OVC_PUR_IPURCH,
                    STATE_TYPE="計畫評核",
                    STATE_NOW="承辦中",
                    USER_NOW=t.OVC_CHECKER??""
                }).ToList();
            #endregion
            #region 採包待分案
            var queryOvcPurch =
                (from tbm1301Plan in mpms.TBM1301_PLAN
                 join tbm1301 in mpms.TBM1301 on tbm1301Plan.OVC_PURCH equals tbm1301.OVC_PURCH
                 where tbm1301.OVC_PUR_ALLOW != null
                 select new
                 {
                     OVC_PURCH = tbm1301Plan.OVC_PURCH,
                     OVC_PUR_AGENCY = tbm1301Plan.OVC_PUR_AGENCY,
                     OVC_PUR_IPURCH = tbm1301Plan.OVC_PUR_IPURCH,
                     OVC_DO_NAME = "",
                     OVC_PERMISSION_UPDATE = tbm1301.OVC_PERMISSION_UPDATE,
                 }).ToArray();

            var queryMaxStatus =
                from tbmStatus in mpms.TBMSTATUS
                group tbmStatus by tbmStatus.OVC_PURCH into tbGroup1
                let maxStatus = tbGroup1.OrderByDescending
                    (tb => tb.OVC_STATUS == null ? "0".Length : tb.OVC_STATUS == "3" ? "30".Length : tb.OVC_STATUS.Length)
                    .ThenByDescending(tb => tb.OVC_STATUS).FirstOrDefault()
                select new
                {
                    OVC_PURCH = maxStatus.OVC_PURCH,
                    OVC_STATUS = maxStatus.OVC_STATUS,
                    OVC_DBEGIN = maxStatus.OVC_DBEGIN,
                };


            var queryTotal =
                from tbm1301 in queryOvcPurch
                join tbmSTATUS in queryMaxStatus on tbm1301.OVC_PURCH equals tbmSTATUS.OVC_PURCH into tbGroup1
                from tbmSTATUS in tbGroup1.DefaultIfEmpty()
                orderby tbm1301.OVC_PURCH
                select new
                {
                    OVC_PURCH = tbm1301.OVC_PURCH,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    OVC_DBEGIN = tbmSTATUS == null ? string.Empty : tbmSTATUS.OVC_DBEGIN,
                    OVC_STATUS = tbmSTATUS == null ? string.Empty : tbmSTATUS.OVC_STATUS,
                    OVC_REMARK = tbm1301.OVC_PERMISSION_UPDATE,
                };
            //已移履驗結單之購案不顯示 (條件：tbStatus.OVC_STATUS.Substring(0, 1) < 3 )
            var query_4_1 =
            (from tb in queryTotal
            where tb.OVC_STATUS == "" ? (tb.OVC_STATUS == "") : (tb.OVC_STATUS.Substring(0, 1).CompareTo("3") < 0)
            select new
            {
                PURCH = tb.OVC_PURCH,
                IPURCH = tb.OVC_PUR_IPURCH,
                STATE_TYPE="採購發包",
                STATE_NOW="待分案",
                USER_NOW = ""
            }).ToList();
            #endregion
            #region 採包承辦中
            var queryOvc_Purch =
                    (from tbRECEIVE_BID in mpms.TBMRECEIVE_BID
                     select new
                     {
                         OVC_PURCH = tbRECEIVE_BID.OVC_PURCH,
                         OVC_DO_NAME = tbRECEIVE_BID.OVC_DO_NAME,
                     }).ToArray();

            var queryTBM1301 =
                (from tbm1301Plan in mpms.TBM1301_PLAN
                 join tbm1301 in mpms.TBM1301 on tbm1301Plan.OVC_PURCH equals tbm1301.OVC_PURCH
                 select new
                 {
                     OVC_PURCH = tbm1301Plan.OVC_PURCH,
                     OVC_PUR_AGENCY = tbm1301Plan.OVC_PUR_AGENCY,
                     OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                     OVC_PUR_USER = tbm1301.OVC_PUR_USER,
                     OVC_PERMISSION_UPDATE = tbm1301.OVC_PERMISSION_UPDATE,
                 }).ToArray();

            var queryMaxStatus_1 =
                from tbmStatus in mpms.TBMSTATUS
                group tbmStatus by tbmStatus.OVC_PURCH into tbGroup1
                let maxStatus = tbGroup1.OrderByDescending
                    (tb => tb.OVC_STATUS == null ? "0".Length : tb.OVC_STATUS == "3" ? "30".Length : tb.OVC_STATUS.Length)
                    .ThenByDescending(tb => tb.OVC_STATUS).FirstOrDefault()
                select new
                {
                    OVC_PURCH = maxStatus.OVC_PURCH,
                    OVC_STATUS = maxStatus.OVC_STATUS == "22" ? "23" : (maxStatus.OVC_STATUS == "24" ? "25" : maxStatus.OVC_STATUS),
                    OVC_DBEGIN = maxStatus.OVC_DBEGIN,
                };


            var queryTotal_1 =
                from tbRECEIVE_BID in queryOvc_Purch
                join tbm1301 in queryTBM1301 on tbRECEIVE_BID.OVC_PURCH equals tbm1301.OVC_PURCH into tbGroup1
                from tbm1301 in tbGroup1.DefaultIfEmpty()
                join tbmSTATUS in queryMaxStatus_1 on tbRECEIVE_BID.OVC_PURCH equals tbmSTATUS.OVC_PURCH into tbGroup2
                from tbmSTATUS in tbGroup2.DefaultIfEmpty()
                select new
                {
                    OVC_PURCH = tbRECEIVE_BID.OVC_PURCH,
                    OVC_PUR_AGENCY = tbm1301 == null ? string.Empty : tbm1301.OVC_PUR_AGENCY,
                    OVC_PURCH_A = "",
                    OVC_PURCH_5 = "",
                    OVC_PUR_IPURCH = tbm1301 == null ? string.Empty : tbm1301.OVC_PUR_IPURCH,
                    OVC_DBEGIN = tbmSTATUS == null ? string.Empty : tbmSTATUS.OVC_DBEGIN,
                    OVC_DO_NAME = tbRECEIVE_BID.OVC_DO_NAME,
                    OVC_STATUS = tbmSTATUS == null ? string.Empty : tbmSTATUS.OVC_STATUS,
                    OVC_STATUS_Desc = "",
                    OVC_REMARK = tbm1301 == null ? string.Empty : tbm1301.OVC_PERMISSION_UPDATE,
                    Date_Flag = "",
                };

            var query_4_2 =
                    (from tb in queryTotal_1
                    where tb.OVC_STATUS == "" ? (tb.OVC_STATUS == "") : (tb.OVC_STATUS.Substring(0, 1).CompareTo("3") < 0)
                    select new
                    {
                        PURCH = tb.OVC_PURCH,
                        IPURCH = tb.OVC_PUR_IPURCH,
                        STATE_TYPE="採購發包",
                        STATE_NOW="承辦中",
                        USER_NOW = tb.OVC_DO_NAME??""
                    }).ToList();
            #endregion
            #region 履約驗結待分案
            var query_5_1 =
            (from tbm1302 in mpms.TBM1302
            join tbm1301 in mpms.TBM1301_PLAN on tbm1302.OVC_PURCH equals tbm1301.OVC_PURCH
            where tbm1302.OVC_DSEND != null && tbm1302.OVC_DSEND != " "
            where tbm1302.OVC_DRECEIVE.Equals(null) || tbm1302.OVC_DRECEIVE.Equals(" ")
            orderby tbm1301.OVC_PURCH
            select new
            {
                PURCH = tbm1301.OVC_PURCH,
                IPURCH = tbm1301.OVC_PUR_IPURCH,
                STATE_TYPE = "履約驗結",
                STATE_NOW = "待分案",
                USER_NOW = ""
            }).ToList();
            #endregion
            #region 屢驗承辦中
            var query_5_2 =
                (from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                join tbm1302 in mpms.TBM1302 on tbmreceive.OVC_PURCH equals tbm1302.OVC_PURCH
                join tbm1301 in mpms.TBM1301_PLAN on tbmreceive.OVC_PURCH equals tbm1301.OVC_PURCH
                where tbmreceive.OVC_PURCH_6.Equals(tbm1302.OVC_PURCH_6)
                where tbmreceive.OVC_VEN_CST.Equals(tbm1302.OVC_VEN_CST)
                where tbmreceive.ONB_GROUP.Equals(tbm1302.ONB_GROUP)
                where tbm1302.OVC_DSEND != null
                where tbm1302.OVC_DRECEIVE != null
                where tbmreceive.OVC_DCLOSE.Equals(null)
                select new
                {
                    PURCH = tbmreceive.OVC_PURCH,
                    IPURCH = tbm1301.OVC_PUR_IPURCH,
                    STATE_TYPE = "履約驗結",
                    STATE_NOW = "承辦中",
                    USER_NOW = tbmreceive.OVC_DO_NAME ?? ""
                }).ToList();
            #endregion
            #region 結案
            var query_6 =
                (from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                join tbm1302 in mpms.TBM1302 on tbmreceive.OVC_PURCH equals tbm1302.OVC_PURCH
                join tbm1301 in mpms.TBM1301_PLAN on tbmreceive.OVC_PURCH equals tbm1301.OVC_PURCH
                where tbmreceive.OVC_PURCH_6.Equals(tbm1302.OVC_PURCH_6)
                where tbmreceive.OVC_VEN_CST.Equals(tbm1302.OVC_VEN_CST)
                where tbmreceive.ONB_GROUP.Equals(tbm1302.ONB_GROUP)
                where tbm1302.OVC_DSEND != null
                where tbm1302.OVC_DRECEIVE != null
                where tbmreceive.OVC_DCLOSE != null
                select new
                {
                    PURCH = tbm1301.OVC_PURCH,
                    IPURCH = tbm1301.OVC_PUR_IPURCH,
                    STATE_TYPE = "結案",
                    STATE_NOW = "結案",
                    USER_NOW = tbmreceive.OVC_DO_NAME ?? ""
                }).ToList();
            #endregion
            var query_final =
                (from q3_1 in query_3_1
                 select q3_1)
                .Union
                (from q3_2 in query_3_2
                 select q3_2)
                 .Union
                (from q4_1 in query_4_1
                 select q4_1)
                 .Union
                (from q4_2 in query_4_2
                 select q4_2)
                 .Union
                (from q5_1 in query_5_1
                 select q5_1)
                 .Union
                (from q5_2 in query_5_2
                 select q5_2)
                 .Union
                (from q6 in query_6
                 select q6);
            bool isQueryEmpty = false;
            string strPURCH = txtIPURCH.Text;
            string strIPURCH = txtIPURCH.Text;
            string strSTATE_TYPE = drpSTATE_TYPE.SelectedValue.ToString();
            string strSTATE_NOW = drpSTATE_NOW.SelectedValue.ToString();
            string strUSER_NOW = txtUSER_NOW.Text;

            if (strPURCH != "")
            {
                query_final = query_final.Where(x => x.PURCH.Contains(strPURCH));
                isQueryEmpty = true;
            }
            if (strIPURCH != "")
            {
                query_final = query_final.Where(x => x.IPURCH.Contains(strIPURCH));
                isQueryEmpty = true;
            }
            if (strSTATE_TYPE != "")
            {
                query_final = query_final.Where(x => x.STATE_TYPE.Equals(strSTATE_TYPE));
                isQueryEmpty = true;
            }
            if (strSTATE_NOW != ""&& strSTATE_TYPE!="結案")
            {
                query_final = query_final.Where(x => x.STATE_NOW.Equals(strSTATE_NOW));
                isQueryEmpty = true;
            }
            if (strUSER_NOW != "")
            {
                query_final = query_final.Where(x => x.USER_NOW!=null);
                query_final = query_final.Where(x => x.USER_NOW.Contains(strUSER_NOW));
                isQueryEmpty = true;
            }
            if (isQueryEmpty == false)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於查詢結果資料筆數過多，請輸入至少一個查詢條件!");
            else
            {
                DataTable dt1 = CommonStatic.LinqQueryToDataTable(query_final);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_Purchase_Track, dt1);
            }           
        }
    }
}