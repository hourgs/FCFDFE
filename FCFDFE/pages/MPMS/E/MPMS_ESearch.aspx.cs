using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Entity.MTSModel;
using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_ESearch : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        MPMSEntities MPMS = new MPMSEntities();
        GMEntities GME = new GMEntities();
        string strDateFormat = Variable.strDateFormat;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            {
                if (!IsPostBack)
                {
                    if ((string)(Session["XSSRequest"]) == "danger")
                    {
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "輸入錯誤，請重新輸入！");
                        Session["XSSRequest"] = null;
                    }

                    var query = GME.TBM1407;
                    DataTable dtOVC_PUR_AGENCY = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "C2").ToList());
                    FCommon.list_dataImportV(DropDownList11, dtOVC_PUR_AGENCY, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
                    DataTable dtOVC_LAB = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "GN").ToList());
                    FCommon.list_dataImportV(DropDownList12, dtOVC_LAB, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
                    FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_DEPT_CDE, txtOVC_ONNAME, txt_nondelivery, txtAfter_day, txtShouldhavef, txtClosingD1,
                        txtClosingD2, txtRePurD1, txtRePurD2, txtDJOINCHECK, txtDJOINCHECK2, txtPenaltyfuse, txtPenaltyfuse2, txtMarginExpiration, txtMarginExpiration2);
                    #region 選擇年度下拉式選單
                    int theYear = FCommon.getTaiwanYear(DateTime.Now);
                    int yearMax = theYear + 2, yearMin = theYear - 15;
                    FCommon.list_dataImportNumber(drpTimeControl, 2, yearMax, yearMin);
                    FCommon.list_setValue(drpTimeControl, theYear.ToString());
                    FCommon.list_dataImportNumber(drpStatics, 2, yearMax, yearMin);
                    FCommon.list_setValue(drpStatics, theYear.ToString());
                    FCommon.list_dataImportNumber(drpexceed_standard, 2, yearMax, yearMin);
                    FCommon.list_setValue(drpexceed_standard, theYear.ToString());
                    FCommon.list_dataImportNumber(drpAfter_year, 2, yearMax, yearMin);
                    FCommon.list_setValue(drpAfter_year, theYear.ToString());
                    FCommon.list_dataImportNumber(drpWithoutwarrantymoney, 2, yearMax, yearMin);
                    FCommon.list_setValue(drpWithoutwarrantymoney, theYear.ToString());
                    FCommon.list_dataImportNumber(drpguaranteenotrefund, 2, yearMax, yearMin);
                    FCommon.list_setValue(drpguaranteenotrefund, theYear.ToString());
                    FCommon.list_dataImportNumber(drpWarrantyGold, 2, yearMax, yearMin);
                    FCommon.list_setValue(drpWarrantyGold, theYear.ToString());
                    FCommon.list_dataImportNumber(drpWarrantyGoldnotRe, 2, yearMax, yearMin);
                    FCommon.list_setValue(drpWarrantyGoldnotRe, theYear.ToString());
                    FCommon.list_dataImportNumber(drpAnnualBatchC, 2, yearMax, yearMin);
                    FCommon.list_setValue(drpAnnualBatchC, theYear.ToString());
                    FCommon.list_dataImportNumber(drpNSecuritydeposit, 2, yearMax, yearMin);
                    FCommon.list_setValue(drpNSecuritydeposit, theYear.ToString());
                    FCommon.list_dataImportNumber(drpSixY, 2, yearMax, yearMin);
                    FCommon.list_setValue(drpSixY, theYear.ToString());
                    #endregion
                }
            }
        }

        #region 回上一頁
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E11.aspx";
            Response.Redirect(send_url);
        }
        #endregion

        #region Q1
        //時程管制(O) query1
        protected void btnTC_Click(object sender, EventArgs e)
        {
            ViewState["btn"] = "Q1";
            DataTable dt = new DataTable();
            if (Session["userid"] != null)
            {
                string strUSER_ID = Session["userid"].ToString();
                string Use_LEVEL = "";
                string StrSection = "";
                string User = "";
                string TC = int.Parse(drpTimeControl.SelectedValue) > 99 ? drpTimeControl.SelectedValue.Substring(1) : drpTimeControl.SelectedValue;
                TBM5200_1 ac = new TBM5200_1();
                TBM5200_PPP acc = new TBM5200_PPP();
                DateTime datetime;
                ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                var acc_auth =
                    (from account in MPMS.ACCOUNTs
                     join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                     where account.USER_ID.Equals(strUSER_ID)
                     select new
                     {
                         C_SN_AUTH = account_auth.C_SN_AUTH,
                         DEPT_SN = account.DEPT_SN,
                         USER_NAME = acc.USER_NAME,
                     }).FirstOrDefault();
                if (ac != null && acc != null)
                {
                    Use_LEVEL = ac.OVC_PRIV_LEVEL;
                    StrSection = acc.OVC_PUR_SECTION;
                    User = acc.USER_NAME;
                }
                else if (acc_auth != null)
                {
                    Use_LEVEL = acc_auth.C_SN_AUTH;
                    StrSection = acc_auth.DEPT_SN;
                    User = acc_auth.USER_NAME;
                }

                var queryGN = MPMS.TBM1407
                .Where(o => o.OVC_PHR_CATE.Equals("GN"));

                var queryP9 = MPMS.TBM1407
                    .Where(o => o.OVC_PHR_CATE.Equals("P9"));

                var queryPay = MPMS.TBMPAY_MONEY
                    .Where(o => o.ONB_PAY_MONEY != null);

                var queryRec = MPMS.TBMRECEIVE_CONTRACT
                    .Where(o => o.OVC_RECEIVE_COMM != null);
                
                var query1301_plan = MPMS.TBM1301_PLAN
                    .Where(o => o.OVC_PURCH.Substring(2, 2).Equals(TC))
                    .Where(o => o.OVC_CONTRACT_UNIT.Equals(StrSection));

                var queryVide = MPMS.VIDELIVERY
                    .Where(v => (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? v.OVC_DO_NAME.Equals(User) : true)
                    .Join(query1301_plan, v => v.OVC_PURCH, o => o.OVC_PURCH, (v, o) => new { v = v, o = o });

                var query1118 =
                    from tbm1118 in MPMS.TBM1118.AsEnumerable()
                    select new
                    {
                        tbm1118.OVC_PURCH,
                        tbm1118.OVC_YY,
                        tbm1118.OVC_POI_IBDG,
                        tbm1118.OVC_PJNAME
                    };

                var t1118group = query1118
                    .GroupBy(cc => new { cc.OVC_PURCH, })
                    .Select(dd => new
                    {
                        dd.Key.OVC_PURCH,
                        OVC_YY = string.Join(";", dd.Select(ee => ee.OVC_YY.AsEnumerable()).Distinct()),
                        OVC_POI_IBDG = string.Join(";", dd.Select(ee => ee.OVC_POI_IBDG.AsEnumerable()).Distinct()),
                        OVC_PJNAME = string.Join(";", dd.Select(ee => ee.OVC_PJNAME.AsEnumerable()).Distinct())
                    });

                var querydel =
                    from tbmdel in MPMS.TBMDELIVERY.AsEnumerable()
                    select new
                    {
                        OVC_PURCH = tbmdel.OVC_PURCH,
                        ONB_SHIP_TIMES = tbmdel.ONB_SHIP_TIMES,
                        OVC_DELIVERY_CONTRACT = tbmdel.OVC_DELIVERY_CONTRACT ?? " ",
                        OVC_DELIVERY = tbmdel.OVC_DELIVERY ?? " ",
                        ONB_DAYS_CONTRACT = tbmdel.ONB_DAYS_CONTRACT,
                        OVC_DJOINCHECK = tbmdel.OVC_DJOINCHECK ?? " ",
                    };

                var tbmdel_g = querydel
                    .GroupBy(cc => new { cc.OVC_PURCH })
                    .Select(dd => new
                    {
                        OVC_PURCH = dd.Key.OVC_PURCH,
                        OVC_DELIVERY_CONTRACT = string.Join(";", dd.Select(ee => ee.ONB_SHIP_TIMES + "(" + ee.OVC_DELIVERY_CONTRACT.AsEnumerable() + ")").Distinct()),
                        OVC_DELIVERY = string.Join(";", dd.Select(ee => ee.ONB_SHIP_TIMES + "(" + ee.OVC_DELIVERY.AsEnumerable() + ")").Distinct()),
                        ONB_DAYS_CONTRACT = string.Join(";", dd.Select(ee => ee.ONB_SHIP_TIMES + "(" + ee.ONB_DAYS_CONTRACT + ")").Distinct()),
                        OVC_DJOINCHECK = string.Join(";", dd.Select(ee => ee.ONB_SHIP_TIMES + "(" + ee.OVC_DJOINCHECK.AsEnumerable() + ")").Distinct()),
                    });

                var query =
                    from vide in queryVide.AsEnumerable()
                    join GN in queryGN on vide.v.OVC_LAB equals GN.OVC_PHR_ID into GN
                    from tbmGN in GN.DefaultIfEmpty().AsEnumerable()
                    join P9 in queryP9 on vide.v.OVC_GRANT_TO equals P9.OVC_PHR_ID into P9
                    from tbmP9 in P9.DefaultIfEmpty().AsEnumerable()
                    join tbmpay in queryPay on new { vide.v.OVC_PURCH, vide.v.OVC_PURCH_6 } equals new { tbmpay.OVC_PURCH, tbmpay.OVC_PURCH_6 } into p
                    from pay in p.DefaultIfEmpty().AsEnumerable()
                    join tbmrec in queryRec on new { vide.v.OVC_PURCH, vide.v.OVC_PURCH_6 } equals new { tbmrec.OVC_PURCH, tbmrec.OVC_PURCH_6 } into r
                    from rec in r.DefaultIfEmpty().AsEnumerable()
                    join t1118 in t1118group.AsEnumerable() on vide.v.OVC_PURCH equals t1118.OVC_PURCH into t
                    from tbm1118 in t.DefaultIfEmpty().AsEnumerable()
                    join tdel in tbmdel_g.AsEnumerable() on vide.v.OVC_PURCH equals tdel.OVC_PURCH into d
                    from tbmdel in d.DefaultIfEmpty().AsEnumerable()
                    let sum_1 = (p.Sum(o => o.ONB_DELAY_MONEY ?? 0))
                    let sum_2 = (p.Sum(o => o.ONB_MINS_MONEY_1 ?? 0)) + (p.Sum(o => o.ONB_MINS_MONEY_2 ?? 0))
                    let DAPPLY = DateTime.TryParse(vide.o.OVC_DAPPLY, out datetime) ? DateTime.Parse(vide.o.OVC_DAPPLY).AddDays(double.Parse((vide.o.ONB_REVIEW_DAYS ?? 0).ToString()))
                                                .AddDays(double.Parse((vide.o.ONB_TENDER_DAYS ?? 0).ToString()))
                                                .AddDays(double.Parse((vide.o.ONB_DELIVER_DAYS ?? 0).ToString()))
                                                .AddDays(double.Parse((vide.o.ONB_RECEIVE_DAYS ?? 0).ToString())).ToString("yyyy-MM-dd") : ""
                    orderby vide.v.OVC_PURCH
                    select new
                    {
                        OVC_PURCH = vide.v.PURCH,
                        ONB_GROUP = "0",
                        OVC_PUR_IPURCH = vide.v.OVC_PUR_IPURCH,
                        OVC_PUR_NSECTION = vide.v.OVC_PUR_NSECTION,
                        OVC_YY = tbm1118 != null ? tbm1118.OVC_YY : "",
                        OVC_POI_IBDG = tbm1118 != null ? tbm1118.OVC_POI_IBDG : "",
                        OVC_PJNAME = tbm1118 != null ? tbm1118.OVC_PJNAME : "",
                        OVC_LAB = tbmGN != null ? tbmGN.OVC_PHR_DESC : "",
                        ONB_MONEY = vide.v.ONB_MONEY ?? 0,
                        ONB_PAY_MONEY = pay != null ? pay.ONB_PAY_MONEY ?? 0 : 0,
                        OVC_DCONTRACT = vide.v.OVC_DCONTRACT,
                        OVC_DELIVERY_CONTRACT = tbmdel != null ? tbmdel.OVC_DELIVERY_CONTRACT : "",
                        OVC_DELIVERY = tbmdel != null ? tbmdel.OVC_DELIVERY : "",
                        ONB_DAYS_CONTRACT = tbmdel != null ? tbmdel.ONB_DAYS_CONTRACT : "",
                        OVC_DJOINCHECK = tbmdel != null ? tbmdel.OVC_DJOINCHECK : "",
                        OVC_DCLOSE = vide.v.OVC_DCLOSE,
                        OVC_STATUS = vide.v.OVC_STATUS,
                        OVC_DAPPLY = DAPPLY,
                        OVC_RECEIVE_COMM = rec != null ? rec.OVC_RECEIVE_COMM ?? "" : "",
                        ONB_DELAY_MONEY = pay != null ? sum_1 : 0,
                        ONB_MINS_MONEY = pay != null ? sum_2 : 0,
                        OVC_GRANT_TO = tbmP9 != null ? tbmP9.OVC_PHR_DESC : "",
                        OVC_DO_NAME = vide.v.OVC_DO_NAME,
                        OVC_VEN_TITLE = vide.v.OVC_VEN_TITLE,
                        OVC_CLOSE = vide.v.OVC_CLOSE
                    };

                dt = CommonStatic.LinqQueryToDataTable(query);
                FCommon.GridView_dataImport(GV_Q19, dt);

                print("履驗時程管制表", GV_Q19);
            }
        }
        #endregion
        #region Q2
        //未結案統計表(目前採購天數：ONB_DATS_CONTRACT確認) query2
        protected void btnStatics_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (Session["userid"] != null)
            {
                string strUSER_ID = Session["userid"].ToString();
                string Use_LEVEL = "";
                string StrSection = "";
                string User = "";
                string TC = int.Parse(drpStatics.Text) > 99 ? drpStatics.SelectedValue.Substring(1) : drpStatics.SelectedValue;
                TBM5200_1 ac = new TBM5200_1();
                TBM5200_PPP acc = new TBM5200_PPP();
                DateTime date;
                ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                var acc_auth =
                    (from account in MPMS.ACCOUNTs
                     join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                     where account.USER_ID.Equals(strUSER_ID)
                     select new
                     {
                         C_SN_AUTH = account_auth.C_SN_AUTH,
                         DEPT_SN = account.DEPT_SN,
                         USER_NAME = acc.USER_NAME,
                     }).FirstOrDefault();
                if (ac != null && acc != null)
                {
                    Use_LEVEL = ac.OVC_PRIV_LEVEL;
                    StrSection = acc.OVC_PUR_SECTION;
                    User = acc.USER_NAME;
                }
                else if (acc_auth != null)
                {
                    Use_LEVEL = acc_auth.C_SN_AUTH;
                    StrSection = acc_auth.DEPT_SN;
                    User = acc_auth.USER_NAME;
                }

                var tbmplan = MPMS.TBM1301_PLAN.Where(o => o.OVC_CONTRACT_UNIT.Equals(StrSection)).DefaultIfEmpty();
                var query =
                    from tbm1301 in MPMS.TBM1301.AsEnumerable()
                    join tbm1302 in MPMS.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                    join tbmR in MPMS.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmR.OVC_PURCH
                    join tbmD in MPMS.TBMDELIVERY.AsEnumerable() on tbm1301.OVC_PURCH equals tbmD.OVC_PURCH
                    where tbm1301.OVC_PURCH == tbmR.OVC_PURCH && tbm1302.OVC_PURCH == tbmR.OVC_PURCH && tbm1302.OVC_PURCH_6 == tbmR.OVC_PURCH_6 && tbm1302.ONB_GROUP == tbmR.ONB_GROUP && tbm1302.OVC_VEN_CST == tbmR.OVC_VEN_CST && tbmR.OVC_PURCH == tbmD.OVC_PURCH && tbmR.OVC_PURCH_6 == tbmD.OVC_PURCH_6
                    where tbm1301.OVC_PURCH.Substring(2, 2).Equals(TC)
                    where tbmR.OVC_DCLOSE == null
                    let usedays = DateTime.TryParse(tbmD.OVC_DELIVERY, out date) ? (DateTime.Now - DateTime.Parse(tbmD.OVC_DELIVERY)).Days : 0
                    orderby tbm1301.OVC_PURCH
                    select new
                    {
                        OVC_PURCH = tbm1301.OVC_PURCH,                              //購案編號
                        OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,                    //購案名稱
                        OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE,                      //廠商名稱
                        OVC_DELIVERY = tbmD.OVC_DELIVERY,                           //交貨日期
                        ONB_DINSPECT_SOP = tbmD.ONB_DINSPECT_SOP,                   //標準驗收天數
                        ONB_DAYS_CONTRACT = usedays,                 //目前採購天數 (需確認)
                        OVC_DO_NAME = tbmR.OVC_DO_NAME,                             //承辦人
                    };

                var finquery =
                    from t1301 in query.AsEnumerable()
                    join plan in tbmplan.AsEnumerable() on t1301.OVC_PURCH equals plan.OVC_PURCH
                    where (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? t1301.OVC_DO_NAME.Equals(User) : true
                    select t1301;

                dt = CommonStatic.LinqQueryToDataTable(finquery);
                FCommon.GridView_dataImport(GV_Q2, dt);

                print("未結案統計表", GV_Q2);
            }
        }
        #endregion
        #region Q3
        //應交貨而未交貨個案統計表(O) query3
        protected void btn_nondelivery_Click(object sender, EventArgs e)
        {
            if (txt_nondelivery.Text == "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請輸入 日期");
            else
            {
                DataTable dtq = new DataTable();
                string strUSER_ID = Session["userid"].ToString();
                string Use_LEVEL = "";
                string StrSection = "";
                string User = "";
                string TC = txt_nondelivery.Text;
                TBM5200_1 ac = new TBM5200_1();
                TBM5200_PPP acc = new TBM5200_PPP();
                var c = (char)10;
                ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                var acc_auth =
                    (from account in MPMS.ACCOUNTs
                     join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                     where account.USER_ID.Equals(strUSER_ID)
                     select new
                     {
                         C_SN_AUTH = account_auth.C_SN_AUTH,
                         DEPT_SN = account.DEPT_SN,
                         USER_NAME = acc.USER_NAME,
                     }).FirstOrDefault();
                if (ac != null && acc != null)
                {
                    Use_LEVEL = ac.OVC_PRIV_LEVEL;
                    StrSection = acc.OVC_PUR_SECTION;
                    User = acc.USER_NAME;
                }
                else if (acc_auth != null)
                {
                    Use_LEVEL = acc_auth.C_SN_AUTH;
                    StrSection = acc_auth.DEPT_SN;
                    User = acc_auth.USER_NAME;
                }
                var tbm = GME.TBM1301_PLAN.Where(o => o.OVC_CONTRACT_UNIT.Equals(StrSection)).ToArray();
                var tbmDE = MPMS.TBMDELIVERY.Where(o => o.OVC_DELIVERY_CONTRACT != null);
                var query =
                    from V in MPMS.VIDELIVERY.AsEnumerable()
                    join tbmD in tbmDE on new { V.OVC_PURCH, V.OVC_PURCH_6 } equals new { tbmD.OVC_PURCH, tbmD.OVC_PURCH_6 }
                    join t in tbm on V.OVC_PURCH equals t.OVC_PURCH
                    where Convert.ToDateTime(tbmD.OVC_DELIVERY_CONTRACT) <= Convert.ToDateTime(TC) && (tbmD.OVC_DELIVERY == " " || Convert.ToDateTime(tbmD.OVC_DELIVERY) > Convert.ToDateTime(TC))
                    where (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? V.OVC_DO_NAME.Equals(User) : true
                    orderby t.OVC_PURCH
                    select new
                    {
                        OVC_PURCH = "購案編號：" + V.PURCH,
                        OVC_PUR_IPURCH = "購案名稱：" + t.OVC_PUR_IPURCH,
                        OVC_PUR_NSECTION = "申購單位：" + t.OVC_PUR_NSECTION,
                        ONB_MCONTRACT = "合約金額：" + string.Format("{0:N}", tbmD.ONB_MCONTRACT),
                        OVC_DELIVERY_CONTRACT = "契約交貨日期：" + tbmD.OVC_DELIVERY_CONTRACT,
                        OVC_VEN_TITLE = "承辦商：" + V.OVC_VEN_TITLE,
                        OVC_DO_NAME = "承辦人：" + V.OVC_DO_NAME,
                        OVC_RECEIVE_COMM = V.OVC_RECEIVE_COMM,                      //重要事項
                    };
                dtq = CommonStatic.LinqQueryToDataTable(query);
                FCommon.GridView_dataImport(GV_Q3, dtq);

                print("應交貨未交貨個案統計表", GV_Q3);
            }
        }
        #endregion
        #region Q4
        //應結案而未結案個案統計表 (O) query4
        protected void btnAfter_day_Click(object sender, EventArgs e)
        {
            if (txtAfter_day.Text == "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請輸入 日期");
            else
            {
                DataTable dtq = new DataTable();
                string strUSER_ID = Session["userid"].ToString();
                string Use_LEVEL = "";
                string StrSection = "";
                string User = "";
                string TC = txtAfter_day.Text;
                TBM5200_1 ac = new TBM5200_1();
                TBM5200_PPP acc = new TBM5200_PPP();
                ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                var acc_auth =
                    (from account in MPMS.ACCOUNTs
                     join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                     where account.USER_ID.Equals(strUSER_ID)
                     select new
                     {
                         C_SN_AUTH = account_auth.C_SN_AUTH,
                         DEPT_SN = account.DEPT_SN,
                         USER_NAME = acc.USER_NAME,
                     }).FirstOrDefault();
                if (ac != null && acc != null)
                {
                    Use_LEVEL = ac.OVC_PRIV_LEVEL;
                    StrSection = acc.OVC_PUR_SECTION;
                    User = acc.USER_NAME;
                }
                else if (acc_auth != null)
                {
                    Use_LEVEL = acc_auth.C_SN_AUTH;
                    StrSection = acc_auth.DEPT_SN;
                    User = acc_auth.USER_NAME;
                }
                DateTime dateTemp, dateTC;
                DateTime.TryParse(TC, out dateTC);

                var plan = GME.TBM1301_PLAN.Where(o => o.OVC_CONTRACT_UNIT.Equals(StrSection)).DefaultIfEmpty();

                var delivery = from tbmD in MPMS.TBMDELIVERY.AsEnumerable()
                               where tbmD.OVC_DPAY_PLAN != null && Convert.ToDateTime(tbmD.OVC_DPAY_PLAN) <= Convert.ToDateTime(TC) && (tbmD.OVC_DPAY == null || Convert.ToDateTime(tbmD.OVC_DPAY) > Convert.ToDateTime(TC))
                               select tbmD;

                var query = from V in MPMS.VIDELIVERY.AsEnumerable()
                            join tbmD in delivery.AsEnumerable() on new { V.OVC_PURCH, V.OVC_PURCH_6 } equals new { tbmD.OVC_PURCH, tbmD.OVC_PURCH_6 }
                            join t in plan on V.OVC_PURCH equals t.OVC_PURCH
                            where (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? V.OVC_DO_NAME.Equals(User) : true
                            orderby V.OVC_PURCH
                            select new
                            {
                                OVC_PURCH = "購案編號：" + V.PURCH,
                                OVC_PUR_IPURCH = "購案名稱：" + t.OVC_PUR_IPURCH,
                                OVC_PUR_NSECTION = "申購單位：" + t.OVC_PUR_NSECTION,
                                ONB_MCONTRACT = "合約金額：" + tbmD.ONB_MCONTRACT,
                                OVC_DJOINCHECK = "會驗日期：" + tbmD.OVC_DJOINCHECK,
                                OVC_DPAY_PLAN = "預估結案日:" + tbmD.OVC_DPAY_PLAN,
                                OVC_DELIVERY = "實際交貨日期：" + tbmD.OVC_DELIVERY,
                                OVC_VEN_TITLE = "承辦商：" + V.OVC_VEN_TITLE,
                                OVC_PUR_USER = "承辦人：" + t.OVC_PUR_USER,
                                OVC_RECEIVE_COMM = V.OVC_RECEIVE_COMM,                      //重要事項
                            };
                dtq = CommonStatic.LinqQueryToDataTable(query);
                FCommon.GridView_dataImport(GV_Q4, dtq);

                print("應結案而未結案個案統計表", GV_Q4);
            }
        }
        #endregion
        #region 結案天數超過標準作業天數 (使用天數錯誤) query5
        protected void btnexceed_standard_Click(object sender, EventArgs e)
        {
            DataTable dtq = new DataTable();
            DateTime n;
            string strUSER_ID = Session["userid"].ToString();
            string Use_LEVEL = "";
            string StrSection = "";
            string User = "";
            string TC = int.Parse(drpexceed_standard.SelectedValue) > 99 ? drpexceed_standard.SelectedValue.Substring(1) : drpexceed_standard.SelectedValue;
            TBM5200_1 ac = new TBM5200_1();
            TBM5200_PPP acc = new TBM5200_PPP();
            ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            var acc_auth =
                (from account in MPMS.ACCOUNTs
                 join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                 where account.USER_ID.Equals(strUSER_ID)
                 select new
                 {
                     C_SN_AUTH = account_auth.C_SN_AUTH,
                     DEPT_SN = account.DEPT_SN,
                     USER_NAME = acc.USER_NAME,
                 }).FirstOrDefault();
            if (ac != null && acc != null)
            {
                Use_LEVEL = ac.OVC_PRIV_LEVEL;
                StrSection = acc.OVC_PUR_SECTION;
                User = acc.USER_NAME;
            }
            else if (acc_auth != null)
            {
                Use_LEVEL = acc_auth.C_SN_AUTH;
                StrSection = acc_auth.DEPT_SN;
                User = acc_auth.USER_NAME;
            }

            var tbmDe = MPMS.TBMDELIVERY
                .Where(o => o.OVC_PURCH.Substring(2, 2).Equals(TC));
            var tbmplan = GME.TBM1301_PLAN
                .Where(o => o.OVC_CONTRACT_UNIT.Equals(StrSection)).ToArray().DefaultIfEmpty();
                
            var query =
                    from t in tbmplan
                    join tbm1301 in MPMS.TBM1301 on t.OVC_PURCH equals tbm1301.OVC_PURCH
                    join tbm1302 in MPMS.TBM1302.AsEnumerable() on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                    join tbmD in tbmDe on new { tbm1302.OVC_PURCH, tbm1302.OVC_PURCH_6 } equals new { tbmD.OVC_PURCH, tbmD.OVC_PURCH_6 }
                    join tbmR in MPMS.TBMRECEIVE_CONTRACT on new { tbmD.OVC_PURCH, tbmD.OVC_PURCH_6 } equals new { tbmR.OVC_PURCH, tbmR.OVC_PURCH_6 }
                    where (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? tbmR.OVC_DO_NAME.Equals(User) : true
                    orderby t.OVC_PURCH
                    select new
                    {
                        購案編號 = tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + tbm1302.OVC_PURCH_5 + tbm1302.OVC_PURCH_6,
                        購案名稱 = tbm1301.OVC_PUR_IPURCH,
                        申購單位 = tbm1301.OVC_PUR_NSECTION,
                        合約金額 = tbmD.ONB_MCONTRACT,
                        實際交貨日期 = tbmD.OVC_DELIVERY,
                        會驗日期 = tbmD.OVC_DJOINCHECK,
                        結案日期 = tbmD.OVC_DPAY,
                        使用天數 = (tbmD.OVC_DPAY != null && tbmD.OVC_DPAY != null) ? (Convert.ToDateTime(tbmD.OVC_DPAY) - Convert.ToDateTime(tbmD.OVC_DELIVERY)).ToString() : "",
                        標準天數 = tbmD.ONB_DINSPECT_SOP,
                        延誤天數 = t.ONB_DELIVER_DAYS,
                        重要事項 = tbmR.OVC_RECEIVE_COMM,
                        承辦人 = tbmR.OVC_DO_NAME
                    };
            dtq = CommonStatic.LinqQueryToDataTable(query);
            FCommon.GridView_dataImport(GV_Q5, dtq);

            print("結案天數超過標準作業天數個案統計表", GV_Q5);
        }
        #endregion
        #region 預劃結案日未能於年度結案個案統計表 (O) query6
        protected void btnAfter_year_Click(object sender, EventArgs e)
        {
            DataTable dtq = new DataTable();

            string strUSER_ID = Session["userid"].ToString();
            string Use_LEVEL = "";
            string StrSection = "";
            string User = "";
            string TC = int.Parse(drpAfter_year.SelectedValue) > 99 ? drpAfter_year.SelectedValue.Substring(1) : drpAfter_year.SelectedValue;
            var tt = TC;
            TBM5200_1 ac = new TBM5200_1();
            TBM5200_PPP acc = new TBM5200_PPP();
            ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            var acc_auth =
                (from account in MPMS.ACCOUNTs
                 join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                 where account.USER_ID.Equals(strUSER_ID)
                 select new
                 {
                     C_SN_AUTH = account_auth.C_SN_AUTH,
                     DEPT_SN = account.DEPT_SN,
                     USER_NAME = acc.USER_NAME,
                 }).FirstOrDefault();
            if (ac != null && acc != null)
            {
                Use_LEVEL = ac.OVC_PRIV_LEVEL;
                StrSection = acc.OVC_PUR_SECTION;
                User = acc.USER_NAME;
            }
            else if (acc_auth != null)
            {
                Use_LEVEL = acc_auth.C_SN_AUTH;
                StrSection = acc_auth.DEPT_SN;
                User = acc_auth.USER_NAME;
            }

            var tbmplan = GME.TBM1301_PLAN.Where(o => o.OVC_CONTRACT_UNIT.Equals(StrSection)).ToArray();
            var tbmDe1 =
                from deliver in MPMS.TBMDELIVERY.AsEnumerable()
                where deliver.OVC_DPAY == null && deliver.OVC_DPAY_PLAN != null && int.Parse(deliver.OVC_DPAY_PLAN.Substring(0, 4)) > (int.Parse(TC) + int.Parse("1911"))
                where deliver.OVC_PURCH.Substring(2, 2).Equals(tt)
                select deliver;

            var query = from tbm1301 in MPMS.TBM1301.AsEnumerable()
                        join tbmD in tbmDe1.AsEnumerable() on tbm1301.OVC_PURCH equals tbmD.OVC_PURCH
                        join tbmPlan in tbmplan.AsEnumerable() on tbm1301.OVC_PURCH equals tbmPlan.OVC_PURCH
                        join tbmRRR in MPMS.TBMRECEIVE_CONTRACT on new { tbmD.OVC_PURCH, tbmD.OVC_PURCH_6 } equals new { tbmRRR.OVC_PURCH, tbmRRR.OVC_PURCH_6 } into mm
                        from tbmR in mm.DefaultIfEmpty().AsEnumerable()
                        join tbm118 in MPMS.TBM1118 on tbmR.OVC_PURCH equals tbm118.OVC_PURCH
                        join tb1302 in MPMS.TBM1302 on tbmR.OVC_PURCH equals tb1302.OVC_PURCH into t1302
                        from tbm1302 in t1302.DefaultIfEmpty().AsEnumerable()
                        join tbmPay in MPMS.TBMPAY_MONEY on new { tbmR.OVC_PURCH, tbmR.OVC_PURCH_6 } equals new { tbmPay.OVC_PURCH, tbmPay.OVC_PURCH_6 } into money
                        from tbmM in money.DefaultIfEmpty().AsEnumerable()
                        where tbmR.OVC_DCLOSE == null || tbmR.OVC_DCLOSE == ""
                        where tbm1302.OVC_PURCH_6 == tbmR.OVC_PURCH_6
                        where tbm1301.OVC_PURCH.Substring(2, 2).Equals(tt)
                        where (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? tbmR.OVC_DO_NAME.Equals(User) : true
                        select new
                        {
                            購案編號 = tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + tbm1302.OVC_PURCH_5 + tbm1302.OVC_PURCH_6,                    //購案編號2
                            購案名稱 = tbm1301.OVC_PUR_IPURCH,          //購案名稱3
                            申購單位 = tbm1301.OVC_PUR_NSECTION,      //申購單位4
                            預算年度 = Convert.ToInt32(tbm118.OVC_YY) % 10,                                                     //預算年度5
                            簽約日期 = tbmR.OVC_DCONTRACT,       //簽約日期6
                            收辦日期 = tbmR.OVC_DRECEIVE,        //收辦日期
                            交貨天數 = tbmD.ONB_DAYS_CONTRACT,        //交貨天數 //文件上寫用tbmD 實際上只有tbmPLAN有該欄位
                            驗收天數 = tbmD.ONB_DINSPECT_SOP,            //驗收天數
                            合約金額 = tbmD.ONB_MCONTRACT != null ? String.Format("{0:N}", tbmD.ONB_MCONTRACT) ?? "0" : "0",  //合約金額5
                                                                                                                          //支用金額 = tbmM.ONB_PAY_MONEY != null ? String.Format("{0:N}", tbmM.ONB_PAY_MONEY) ?? "0" : "0",           //支用金額6
                            支用金額 = tbmM != null ? tbmM.ONB_PAY_MONEY ?? 0 : 0,           //支用金額6
                            預劃結案日 = tbmD.OVC_DPAY_PLAN,         //預劃結案日
                            重要事項 = tbmR.OVC_RECEIVE_COMM,   //重要事項
                            承辦人 = tbmR.OVC_DO_NAME             //承辦人*/
                        };

            dtq = CommonStatic.LinqQueryToDataTable(query);
            FCommon.GridView_dataImport(GV_Q6, dtq);

            print("結案天數超過標準作業天數個案統計表", GV_Q6);
        }
        #endregion
        #region  預劃結案日前應結案而未結案個案統計表 query7 (支用金額有誤，須確認)
        protected void btnShouldhavef_Click(object sender, EventArgs e)
        {
            if (txtShouldhavef.Text == "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請輸入 日期");
            else
            {
                DataTable dtq = new DataTable();
                string strUSER_ID = Session["userid"].ToString();
                string Use_LEVEL = "";
                string StrSection = "";
                string User = "";
                string TC = txtShouldhavef.Text;
                TBM5200_1 ac = new TBM5200_1();
                TBM5200_PPP acc = new TBM5200_PPP();
                ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                var acc_auth =
                    (from account in MPMS.ACCOUNTs
                     join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                     where account.USER_ID.Equals(strUSER_ID)
                     select new
                     {
                         C_SN_AUTH = account_auth.C_SN_AUTH,
                         DEPT_SN = account.DEPT_SN,
                         USER_NAME = acc.USER_NAME,
                     }).FirstOrDefault();
                if (ac != null && acc != null)
                {
                    Use_LEVEL = ac.OVC_PRIV_LEVEL;
                    StrSection = acc.OVC_PUR_SECTION;
                    User = acc.USER_NAME;
                }
                else if (acc_auth != null)
                {
                    Use_LEVEL = acc_auth.C_SN_AUTH;
                    StrSection = acc_auth.DEPT_SN;
                    User = acc_auth.USER_NAME;
                }

                var tbmDe1 =
                    from deliver in MPMS.TBMDELIVERY.AsEnumerable()
                    where deliver.OVC_DPAY == null && deliver.OVC_DPAY_PLAN != null && Convert.ToDateTime(deliver.OVC_DPAY_PLAN) < Convert.ToDateTime(TC)
                    select deliver;


                var money = from tbmMoney in MPMS.TBMPAY_MONEY
                            orderby tbmMoney.OVC_PURCH, tbmMoney.OVC_PURCH_6
                            select new
                            {
                                tbmMoney.OVC_PURCH,
                                tbmMoney.OVC_PURCH_6,
                                tbmMoney.ONB_MCONTRACT,
                                tbmMoney.ONB_PAY_MONEY
                            };

                var tbmplan = GME.TBM1301_PLAN.Where(o => o.OVC_CONTRACT_UNIT.Equals(StrSection)).ToArray();
                var tbmRR = MPMS.TBMRECEIVE_CONTRACT.Where(o => o.OVC_DCLOSE == null);

                var query = from tbm1301 in MPMS.TBM1301.AsEnumerable()
                            join tbmD in tbmDe1 on tbm1301.OVC_PURCH equals tbmD.OVC_PURCH
                            join tbmPlan in tbmplan on tbm1301.OVC_PURCH equals tbmPlan.OVC_PURCH
                            join tbmRRR in MPMS.TBMRECEIVE_CONTRACT on new { tbmD.OVC_PURCH, tbmD.OVC_PURCH_6 } equals new { tbmRRR.OVC_PURCH, tbmRRR.OVC_PURCH_6 } into mm
                            from tbmR in mm.DefaultIfEmpty().AsEnumerable()
                            join tb1302 in MPMS.TBM1302 on tbmR.OVC_PURCH equals tb1302.OVC_PURCH into t1302
                            from tbm1302 in t1302.DefaultIfEmpty()
                            join tbmPay in money on new { tbmR.OVC_PURCH, tbmR.OVC_PURCH_6 } equals new { tbmPay.OVC_PURCH, tbmPay.OVC_PURCH_6 } into tbmm
                            from tbmM in tbmm.DefaultIfEmpty()
                            where tbmR.OVC_DCLOSE == null || tbmR.OVC_DCLOSE == ""
                            where tbm1302.OVC_PURCH_6 == tbmR.OVC_PURCH_6
                            where tbm1302.OVC_PURCH == tbmR.OVC_PURCH
                            where (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? tbmR.OVC_DO_NAME.Equals(User) : true
                            orderby tbm1301.OVC_PURCH
                            select new
                            {
                                購案編號 = tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + tbm1302.OVC_PURCH_5 + tbm1302.OVC_PURCH_6,                   //購案編號1
                                購案名稱 = tbm1301.OVC_PUR_IPURCH,          //購案名稱2
                                申購單位 = tbm1301.OVC_PUR_NSECTION,      //申購單位3
                                合約金額 = tbmD.ONB_MCONTRACT != null ? String.Format("{0:N}", tbmD.ONB_MCONTRACT) ?? "0" : "0",  //合約金額5
                                                                                                                              //支用金額 = tbmM.ONB_PAY_MONEY != null ? String.Format("{0:N}", tbmM.ONB_PAY_MONEY) ?? "0" : "0",           //支用金額6
                                支用金額 = tbmM != null ? tbmM.ONB_PAY_MONEY : 0,           //支用金額6
                                預劃結案日 = tbmD.OVC_DPAY_PLAN,         //預劃結案日7
                                重要事項 = tbmR.OVC_RECEIVE_COMM,   //重要事項8
                                承辦人 = tbmR.OVC_DO_NAME             //承辦人9
                            };
                dtq = CommonStatic.LinqQueryToDataTable(query);
                FCommon.GridView_dataImport(GV_Q7, dtq);

                print("應結案而未結案個案統計表", GV_Q7);
            }
        }
        #endregion
        # region 年度保證金不含保固金管制明細總表 Q8 (union處理)
        protected void btnWithoutwarrantymoney_Click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();

            string strUSER_ID = Session["userid"].ToString();
            string Use_LEVEL = "";
            string StrSection = "";
            string User = "";
            string TC = int.Parse(drpWithoutwarrantymoney.SelectedValue) > 99 ? drpWithoutwarrantymoney.SelectedValue.Substring(1) : drpWithoutwarrantymoney.SelectedValue;
            TBM5200_1 ac = new TBM5200_1();
            TBM5200_PPP acc = new TBM5200_PPP();
            ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            var acc_auth =
                (from account in MPMS.ACCOUNTs
                 join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                 where account.USER_ID.Equals(strUSER_ID)
                 select new
                 {
                     C_SN_AUTH = account_auth.C_SN_AUTH,
                     DEPT_SN = account.DEPT_SN,
                     USER_NAME = acc.USER_NAME,
                 }).FirstOrDefault();
            if (ac != null && acc != null)
            {
                Use_LEVEL = ac.OVC_PRIV_LEVEL;
                StrSection = acc.OVC_PUR_SECTION;
                User = acc.USER_NAME;
            }
            else if (acc_auth != null)
            {
                Use_LEVEL = acc_auth.C_SN_AUTH;
                StrSection = acc_auth.DEPT_SN;
                User = acc_auth.USER_NAME;
            }

            var queryCash =
                from tbm1301 in MPMS.TBM1301
                join cash in MPMS.TBMMANAGE_CASH on tbm1301.OVC_PURCH equals cash.OVC_PURCH
                join tbm1301_plan in MPMS.TBM1301_PLAN on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                join tbmreceivc in MPMS.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmreceivc.OVC_PURCH
                where cash.OVC_KIND.Equals("1")
                where tbm1301.OVC_PURCH.Substring(2, 2).Equals(TC)
                where cash.OVC_PURCH_6.Equals(tbmreceivc.OVC_PURCH_6)
                where cash.OVC_OWN_NO.Equals(tbmreceivc.OVC_VEN_CST)
                where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                where (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? tbmreceivc.OVC_DO_NAME.Equals(User) : true
                select new
                {
                    OVC_PURCH = tbm1301.OVC_PURCH,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    ONB_MONEY = tbmreceivc.ONB_MONEY ?? 0,
                    PRO = "現金",
                    ONB_ALL_MONEY = cash.ONB_ALL_MONEY ?? 0,
                    OVC_OWN_NAME = cash.OVC_OWN_NAME,
                    OVC_COMPTROLLER_NO = cash.OVC_COMPTROLLER_NO,
                    OVC_ONNAME = cash.OVC_ONNAME,
                    OVC_DBACK = cash.OVC_DBACK,
                    OVC_DO_NAME = tbmreceivc.OVC_DO_NAME
                };
            var queryStock =
                from tbm1301 in MPMS.TBM1301
                join stock in MPMS.TBMMANAGE_STOCK on tbm1301.OVC_PURCH equals stock.OVC_PURCH
                join tbm1301_plan in MPMS.TBM1301_PLAN on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                join tbmreceivc in MPMS.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmreceivc.OVC_PURCH
                where stock.OVC_KIND.Equals("1")
                where tbm1301.OVC_PURCH.Substring(2, 2).Equals(TC)
                where stock.OVC_PURCH_6.Equals(tbmreceivc.OVC_PURCH_6)
                where stock.OVC_OWN_NO.Equals(tbmreceivc.OVC_VEN_CST)
                where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                where tbmreceivc.OVC_DO_NAME.Equals(User)
                select new
                {
                    OVC_PURCH = tbm1301.OVC_PURCH,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    ONB_MONEY = tbmreceivc.ONB_MONEY ?? 0,
                    PRO = "有價證件",
                    ONB_ALL_MONEY = stock.ONB_ALL_MONEY ?? 0,
                    OVC_OWN_NAME = stock.OVC_OWN_NAME,
                    OVC_COMPTROLLER_NO = stock.OVC_COMPTROLLER_NO,
                    OVC_ONNAME = stock.OVC_ONNAME,
                    OVC_DBACK = stock.OVC_DBACK,
                    OVC_DO_NAME = tbmreceivc.OVC_DO_NAME
                };
            var queryProm =
                from tbm1301 in MPMS.TBM1301
                join prom in MPMS.TBMMANAGE_PROM on tbm1301.OVC_PURCH equals prom.OVC_PURCH
                join tbm1301_plan in MPMS.TBM1301_PLAN on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                join tbmreceivc in MPMS.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmreceivc.OVC_PURCH
                where prom.OVC_KIND.Equals("1")
                where tbm1301.OVC_PURCH.Substring(2, 2).Equals(TC)
                where prom.OVC_PURCH_6.Equals(tbmreceivc.OVC_PURCH_6)
                where prom.OVC_OWN_NO.Equals(tbmreceivc.OVC_VEN_CST)
                where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                where tbmreceivc.OVC_DO_NAME.Equals(User)
                select new
                {
                    OVC_PURCH = tbm1301.OVC_PURCH,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    ONB_MONEY = tbmreceivc.ONB_MONEY ?? 0,
                    PRO = "保証文件",
                    ONB_ALL_MONEY = prom.ONB_ALL_MONEY ?? 0,
                    OVC_OWN_NAME = prom.OVC_OWNED_NAME,
                    OVC_COMPTROLLER_NO = prom.OVC_COMPTROLLER_NO,
                    OVC_ONNAME = prom.OVC_ONNAME,
                    OVC_DBACK = prom.OVC_DBACK,
                    OVC_DO_NAME = tbmreceivc.OVC_DO_NAME
                };
            var fin = queryCash.Concat(queryStock).Concat(queryProm).AsEnumerable().OrderBy(o => o.OVC_PURCH);
            dt = CommonStatic.LinqQueryToDataTable(fin);
            FCommon.GridView_dataImport(GV_Q8, dt);

            print("保固金管制明細總表", GV_Q8);
        }
        #endregion
        # region 年度尚未發還保證金管制明細表 Q9   
        protected void btnpguaranteenotrefund_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string strUSER_ID = Session["userid"].ToString();
            string Use_LEVEL = "";
            string StrSection = "";
            string User = "";
            string TC = int.Parse(drpguaranteenotrefund.SelectedValue) > 99 ? drpguaranteenotrefund.SelectedValue.Substring(1) : drpguaranteenotrefund.SelectedValue;
            TBM5200_1 ac = new TBM5200_1();
            TBM5200_PPP acc = new TBM5200_PPP();
            ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            var acc_auth =
                (from account in MPMS.ACCOUNTs
                 join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                 where account.USER_ID.Equals(strUSER_ID)
                 select new
                 {
                     C_SN_AUTH = account_auth.C_SN_AUTH,
                     DEPT_SN = account.DEPT_SN,
                     USER_NAME = acc.USER_NAME,
                 }).FirstOrDefault();
            if (ac != null && acc != null)
            {
                Use_LEVEL = ac.OVC_PRIV_LEVEL;
                StrSection = acc.OVC_PUR_SECTION;
                User = acc.USER_NAME;
            }
            else if (acc_auth != null)
            {
                Use_LEVEL = acc_auth.C_SN_AUTH;
                StrSection = acc_auth.DEPT_SN;
                User = acc_auth.USER_NAME;
            }

            var queryCash =
                from tbm1301 in MPMS.TBM1301
                join cash in MPMS.TBMMANAGE_CASH on tbm1301.OVC_PURCH equals cash.OVC_PURCH
                join tbm1301_plan in MPMS.TBM1301_PLAN on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                join tbmreceivc in MPMS.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmreceivc.OVC_PURCH
                where cash.OVC_KIND.Equals("1")
                where cash.OVC_DBACK.Equals(null)
                where tbm1301.OVC_PURCH.Substring(2, 2).Equals(TC)
                where cash.OVC_PURCH_6.Equals(tbmreceivc.OVC_PURCH_6)
                where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                where (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? tbmreceivc.OVC_DO_NAME.Equals(User) : true
                select new
                {
                    OVC_PURCH = tbm1301.OVC_PURCH,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    KIND = "履約保證",
                    PRO = "代管現金",
                    ONB_ALL_MONEY = cash.ONB_ALL_MONEY ?? 0,
                    OVC_OWN_NAME = cash.OVC_OWN_NAME,
                    OVC_ONNAME = cash.OVC_ONNAME,
                    OVC_COMPTROLLER_NO = cash.OVC_COMPTROLLER_NO,
                    OVC_DGARRENT_START = cash.OVC_MARK,
                    OVC_DEFFECT_1 = "",
                    OVC_DGARRENT_END = "",
                    OVC_DBACK = cash.OVC_DBACK,
                    OVC_DO_NAME = tbmreceivc.OVC_DO_NAME
                };
            var queryStock =
                from tbm1301 in MPMS.TBM1301
                join stock in MPMS.TBMMANAGE_STOCK on tbm1301.OVC_PURCH equals stock.OVC_PURCH
                join tbm1301_plan in MPMS.TBM1301_PLAN on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                join tbmreceivc in MPMS.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmreceivc.OVC_PURCH
                where stock.OVC_KIND.Equals("1")
                where stock.OVC_DBACK.Equals(null)
                where tbm1301.OVC_PURCH.Substring(2, 2).Equals(TC)
                where stock.OVC_PURCH_6.Equals(tbmreceivc.OVC_PURCH_6)
                where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                where tbmreceivc.OVC_DO_NAME.Equals(User)
                select new
                {
                    OVC_PURCH = tbm1301.OVC_PURCH,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    KIND = "履約保證",
                    PRO = "代管有價證件",
                    ONB_ALL_MONEY = stock.ONB_ALL_MONEY ?? 0,
                    OVC_OWN_NAME = stock.OVC_OWN_NAME,
                    OVC_ONNAME = stock.OVC_ONNAME,
                    OVC_COMPTROLLER_NO = stock.OVC_COMPTROLLER_NO,
                    OVC_DGARRENT_START = stock.OVC_MARK,
                    OVC_DEFFECT_1 = "",
                    OVC_DGARRENT_END = "",
                    OVC_DBACK = stock.OVC_DBACK,
                    OVC_DO_NAME = tbmreceivc.OVC_DO_NAME
                };
            var queryProm =
                from tbm1301 in MPMS.TBM1301
                join prom in MPMS.TBMMANAGE_PROM on tbm1301.OVC_PURCH equals prom.OVC_PURCH
                join tbm1301_plan in MPMS.TBM1301_PLAN on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                join tbmreceivc in MPMS.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmreceivc.OVC_PURCH
                where prom.OVC_KIND.Equals("1")
                where prom.OVC_DBACK.Equals(null)
                where tbm1301.OVC_PURCH.Substring(2, 2).Equals(TC)
                where prom.OVC_PURCH_6.Equals(tbmreceivc.OVC_PURCH_6)
                where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                where tbmreceivc.OVC_DO_NAME.Equals(User)
                select new
                {
                    OVC_PURCH = tbm1301.OVC_PURCH,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    KIND = "履約保證",
                    PRO = "代管保証文件",
                    ONB_ALL_MONEY = prom.ONB_ALL_MONEY ?? 0,
                    OVC_OWN_NAME = prom.OVC_OWNED_NAME,
                    OVC_ONNAME = prom.OVC_ONNAME,
                    OVC_COMPTROLLER_NO = prom.OVC_COMPTROLLER_NO,
                    OVC_DGARRENT_START = prom.OVC_MARK,
                    OVC_DEFFECT_1 = prom.OVC_DEFFECT_1,
                    OVC_DGARRENT_END = "",
                    OVC_DBACK = prom.OVC_DBACK,
                    OVC_DO_NAME = tbmreceivc.OVC_DO_NAME
                };
            var fin = queryCash.Concat(queryStock).Concat(queryProm).AsEnumerable();
            dt = CommonStatic.LinqQueryToDataTable(fin);
            FCommon.GridView_dataImport(GV_Q9, dt);

            print("履保金管制明細表", GV_Q9);
        }
        #endregion
        # region 年度保固金管制明細總表 Q10
        protected void btnWarrantyGold_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string strUSER_ID = Session["userid"].ToString();
            string Use_LEVEL = "";
            string StrSection = "";
            string User = "";
            string TC = int.Parse(drpWarrantyGold.SelectedValue) > 99 ? drpWarrantyGold.SelectedValue.Substring(1) : drpWarrantyGold.SelectedValue;
            TBM5200_1 ac = new TBM5200_1();
            TBM5200_PPP acc = new TBM5200_PPP();
            ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            var acc_auth =
                (from account in MPMS.ACCOUNTs
                 join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                 where account.USER_ID.Equals(strUSER_ID)
                 select new
                 {
                     C_SN_AUTH = account_auth.C_SN_AUTH,
                     DEPT_SN = account.DEPT_SN,
                     USER_NAME = acc.USER_NAME,
                 }).FirstOrDefault();
            if (ac != null && acc != null)
            {
                Use_LEVEL = ac.OVC_PRIV_LEVEL;
                StrSection = acc.OVC_PUR_SECTION;
                User = acc.USER_NAME;
            }
            else if (acc_auth != null)
            {
                Use_LEVEL = acc_auth.C_SN_AUTH;
                StrSection = acc_auth.DEPT_SN;
                User = acc_auth.USER_NAME;
            }

            var queryCash =
                from tbm1301 in MPMS.TBM1301
                join cash in MPMS.TBMMANAGE_CASH on tbm1301.OVC_PURCH equals cash.OVC_PURCH
                join tbm1301_plan in MPMS.TBM1301_PLAN on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                join tbmreceivc in MPMS.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmreceivc.OVC_PURCH
                where cash.OVC_KIND.Equals("2")
                where tbm1301.OVC_PURCH.Substring(2, 2).Equals(TC)
                where cash.OVC_PURCH_6.Equals(tbmreceivc.OVC_PURCH_6)
                where cash.OVC_OWN_NO.Equals(tbmreceivc.OVC_VEN_CST)
                where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                where (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? tbmreceivc.OVC_DO_NAME.Equals(User) : true
                select new
                {
                    OVC_PURCH = tbm1301.OVC_PURCH,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    ONB_MONEY = tbmreceivc.ONB_MONEY ?? 0,
                    PRO = "現金",
                    ONB_ALL_MONEY = cash.ONB_ALL_MONEY ?? 0,
                    OVC_OWN_NAME = cash.OVC_OWN_NAME,
                    OVC_COMPTROLLER_NO = cash.OVC_COMPTROLLER_NO,
                    OVC_ONNAME = cash.OVC_ONNAME,
                    OVC_MARK = cash.OVC_MARK,
                    OVC_DEFFECT_1 = "",
                    OVC_DBACK = cash.OVC_DBACK,
                    OVC_DO_NAME = tbmreceivc.OVC_DO_NAME
                };
            var queryStock =
                from tbm1301 in MPMS.TBM1301
                join stock in MPMS.TBMMANAGE_STOCK on tbm1301.OVC_PURCH equals stock.OVC_PURCH
                join tbm1301_plan in MPMS.TBM1301_PLAN on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                join tbmreceivc in MPMS.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmreceivc.OVC_PURCH
                where stock.OVC_KIND.Equals("2")
                where tbm1301.OVC_PURCH.Substring(2, 2).Equals(TC)
                where stock.OVC_PURCH_6.Equals(tbmreceivc.OVC_PURCH_6)
                where stock.OVC_OWN_NO.Equals(tbmreceivc.OVC_VEN_CST)
                where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                where tbmreceivc.OVC_DO_NAME.Equals(User)
                select new
                {
                    OVC_PURCH = tbm1301.OVC_PURCH,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    ONB_MONEY = tbmreceivc.ONB_MONEY ?? 0,
                    PRO = "有價證件",
                    ONB_ALL_MONEY = stock.ONB_ALL_MONEY ?? 0,
                    OVC_OWN_NAME = stock.OVC_OWN_NAME,
                    OVC_COMPTROLLER_NO = stock.OVC_COMPTROLLER_NO,
                    OVC_ONNAME = stock.OVC_ONNAME,
                    OVC_MARK = stock.OVC_MARK,
                    OVC_DEFFECT_1 = "",
                    OVC_DBACK = stock.OVC_DBACK,
                    OVC_DO_NAME = tbmreceivc.OVC_DO_NAME
                };
            var queryProm =
                from tbm1301 in MPMS.TBM1301
                join prom in MPMS.TBMMANAGE_PROM on tbm1301.OVC_PURCH equals prom.OVC_PURCH
                join tbm1301_plan in MPMS.TBM1301_PLAN on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                join tbmreceivc in MPMS.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmreceivc.OVC_PURCH
                where prom.OVC_KIND.Equals("2")
                where tbm1301.OVC_PURCH.Substring(2, 2).Equals(TC)
                where prom.OVC_PURCH_6.Equals(tbmreceivc.OVC_PURCH_6)
                where prom.OVC_OWN_NO.Equals(tbmreceivc.OVC_VEN_CST)
                where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                where tbmreceivc.OVC_DO_NAME.Equals(User)
                select new
                {
                    OVC_PURCH = tbm1301.OVC_PURCH,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    ONB_MONEY = tbmreceivc.ONB_MONEY ?? 0,
                    PRO = "保証文件",
                    ONB_ALL_MONEY = prom.ONB_ALL_MONEY ?? 0,
                    OVC_OWN_NAME = prom.OVC_OWNED_NAME,
                    OVC_COMPTROLLER_NO = prom.OVC_COMPTROLLER_NO,
                    OVC_ONNAME = prom.OVC_ONNAME,
                    OVC_MARK = prom.OVC_MARK,
                    OVC_DEFFECT_1 = "",
                    OVC_DBACK = prom.OVC_DBACK,
                    OVC_DO_NAME = tbmreceivc.OVC_DO_NAME
                };
            var fin = queryCash.Concat(queryStock).Concat(queryProm).AsEnumerable();
            dt = CommonStatic.LinqQueryToDataTable(fin);
            FCommon.GridView_dataImport(GV_Q10, dt);

            print("保證金不含保固金管制明細總表", GV_Q10);
        }
        #endregion
        # region 年度尚未發還保固金管制明細表 Q11
        protected void btnWarrantyGoldnotRe_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string strUSER_ID = Session["userid"].ToString();
            string Use_LEVEL = "";
            string StrSection = "";
            string User = "";
            string TC = int.Parse(drpWarrantyGoldnotRe.SelectedValue) > 99 ? drpWarrantyGoldnotRe.SelectedValue.Substring(1) : drpWarrantyGoldnotRe.SelectedValue;
            TBM5200_1 ac = new TBM5200_1();
            TBM5200_PPP acc = new TBM5200_PPP();
            ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            var acc_auth =
                (from account in MPMS.ACCOUNTs
                 join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                 where account.USER_ID.Equals(strUSER_ID)
                 select new
                 {
                     C_SN_AUTH = account_auth.C_SN_AUTH,
                     DEPT_SN = account.DEPT_SN,
                     USER_NAME = acc.USER_NAME,
                 }).FirstOrDefault();
            if (ac != null && acc != null)
            {
                Use_LEVEL = ac.OVC_PRIV_LEVEL;
                StrSection = acc.OVC_PUR_SECTION;
                User = acc.USER_NAME;
            }
            else if (acc_auth != null)
            {
                Use_LEVEL = acc_auth.C_SN_AUTH;
                StrSection = acc_auth.DEPT_SN;
                User = acc_auth.USER_NAME;
            }

            var queryCash =
                from tbm1301 in MPMS.TBM1301
                join cash in MPMS.TBMMANAGE_CASH on tbm1301.OVC_PURCH equals cash.OVC_PURCH
                join tbm1301_plan in MPMS.TBM1301_PLAN on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                join tbmreceivc in MPMS.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmreceivc.OVC_PURCH
                where cash.OVC_KIND.Equals("2")
                where cash.OVC_DBACK.Equals(null)
                where tbm1301.OVC_PURCH.Substring(2, 2).Equals(TC)
                where cash.OVC_PURCH_6.Equals(tbmreceivc.OVC_PURCH_6)
                where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                where (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? tbmreceivc.OVC_DO_NAME.Equals(User) : true
                select new
                {
                    OVC_PURCH = tbm1301.OVC_PURCH,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    KIND = "履約保證",
                    PRO = "代管現金",
                    ONB_ALL_MONEY = cash.ONB_ALL_MONEY ?? 0,
                    OVC_OWN_NAME = cash.OVC_OWN_NAME,
                    OVC_ONNAME = cash.OVC_ONNAME,
                    OVC_COMPTROLLER_NO = cash.OVC_COMPTROLLER_NO,
                    OVC_DGARRENT_START = cash.OVC_MARK,
                    OVC_DEFFECT_1 = "",
                    OVC_DGARRENT_END = "",
                    OVC_DBACK = cash.OVC_DBACK,
                    OVC_DO_NAME = tbmreceivc.OVC_DO_NAME
                };
            var queryStock =
                from tbm1301 in MPMS.TBM1301
                join stock in MPMS.TBMMANAGE_STOCK on tbm1301.OVC_PURCH equals stock.OVC_PURCH
                join tbm1301_plan in MPMS.TBM1301_PLAN on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                join tbmreceivc in MPMS.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmreceivc.OVC_PURCH
                where stock.OVC_KIND.Equals("2")
                where stock.OVC_DBACK.Equals(null)
                where tbm1301.OVC_PURCH.Substring(2, 2).Equals(TC)
                where stock.OVC_PURCH_6.Equals(tbmreceivc.OVC_PURCH_6)
                where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                where tbmreceivc.OVC_DO_NAME.Equals(User)
                select new
                {
                    OVC_PURCH = tbm1301.OVC_PURCH,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    KIND = "履約保證",
                    PRO = "代管有價證件",
                    ONB_ALL_MONEY = stock.ONB_ALL_MONEY ?? 0,
                    OVC_OWN_NAME = stock.OVC_OWN_NAME,
                    OVC_ONNAME = stock.OVC_ONNAME,
                    OVC_COMPTROLLER_NO = stock.OVC_COMPTROLLER_NO,
                    OVC_DGARRENT_START = stock.OVC_MARK,
                    OVC_DEFFECT_1 = "",
                    OVC_DGARRENT_END = "",
                    OVC_DBACK = stock.OVC_DBACK,
                    OVC_DO_NAME = tbmreceivc.OVC_DO_NAME
                };
            var queryProm =
                from tbm1301 in MPMS.TBM1301
                join prom in MPMS.TBMMANAGE_PROM on tbm1301.OVC_PURCH equals prom.OVC_PURCH
                join tbm1301_plan in MPMS.TBM1301_PLAN on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                join tbmreceivc in MPMS.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmreceivc.OVC_PURCH
                where prom.OVC_KIND.Equals("2")
                where prom.OVC_DBACK.Equals(null)
                where tbm1301.OVC_PURCH.Substring(2, 2).Equals(TC)
                where prom.OVC_PURCH_6.Equals(tbmreceivc.OVC_PURCH_6)
                where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                where tbmreceivc.OVC_DO_NAME.Equals(User)
                select new
                {
                    OVC_PURCH = tbm1301.OVC_PURCH,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    KIND = "履約保證",
                    PRO = "代管保証文件",
                    ONB_ALL_MONEY = prom.ONB_ALL_MONEY ?? 0,
                    OVC_OWN_NAME = prom.OVC_OWNED_NAME,
                    OVC_ONNAME = prom.OVC_ONNAME,
                    OVC_COMPTROLLER_NO = prom.OVC_COMPTROLLER_NO,
                    OVC_DGARRENT_START = prom.OVC_MARK,
                    OVC_DEFFECT_1 = prom.OVC_DEFFECT_1,
                    OVC_DGARRENT_END = "",
                    OVC_DBACK = prom.OVC_DBACK,
                    OVC_DO_NAME = tbmreceivc.OVC_DO_NAME
                };
            var fin = queryCash.Concat(queryStock).Concat(queryProm).AsEnumerable();
            dt = CommonStatic.LinqQueryToDataTable(fin);
            FCommon.GridView_dataImport(GV_Q11, dt);

            print("保固金管制明細表", GV_Q11);
        }
        #endregion
        # region 收辦購案統計表 Q12 (日期與範例不同，但與SQL結果相同)
        protected void btnReceivePurchase_Click(object sender, EventArgs e)
        {
            if (txtRePurD1.Text == "" || txtRePurD2.Text == "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請輸入 日期");
            else
            {
                DataTable dtq = new DataTable();
                string strUSER_ID = Session["userid"].ToString();
                string Use_LEVEL = "";
                string StrSection = "";
                string User = "";
                string TC = txtRePurD1.Text;
                string TC2 = txtRePurD2.Text;
                TBM5200_1 ac = new TBM5200_1();
                TBM5200_PPP acc = new TBM5200_PPP();
                ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                var acc_auth =
                    (from account in MPMS.ACCOUNTs
                     join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                     where account.USER_ID.Equals(strUSER_ID)
                     select new
                     {
                         C_SN_AUTH = account_auth.C_SN_AUTH,
                         DEPT_SN = account.DEPT_SN,
                         USER_NAME = acc.USER_NAME,
                     }).FirstOrDefault();
                if (ac != null && acc != null)
                {
                    Use_LEVEL = ac.OVC_PRIV_LEVEL;
                    StrSection = acc.OVC_PUR_SECTION;
                    User = acc.USER_NAME;
                }
                else if (acc_auth != null)
                {
                    Use_LEVEL = acc_auth.C_SN_AUTH;
                    StrSection = acc_auth.DEPT_SN;
                    User = acc_auth.USER_NAME;
                }


                var queryP9 = MPMS.TBM1407
                    .Where(o => o.OVC_PHR_CATE.Equals("P9"));
                var query2 =
                    from tbm1301 in MPMS.TBM1301
                    join tbm1302 in MPMS.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                    join tbmrec in MPMS.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmrec.OVC_PURCH
                    join tbmdel in MPMS.TBMDELIVERY on tbm1301.OVC_PURCH equals tbmdel.OVC_PURCH
                    join tbmpay in MPMS.TBMPAY_MONEY on tbm1301.OVC_PURCH equals tbmpay.OVC_PURCH
                    join tbm1301_plan in MPMS.TBM1301_PLAN on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                    join tbm1118 in MPMS.TBM1118 on tbm1301.OVC_PURCH equals tbm1118.OVC_PURCH
                    where tbmrec.OVC_PURCH_6.Equals(tbmdel.OVC_PURCH_6)
                    where tbmrec.OVC_PURCH_6.Equals(tbm1302.OVC_PURCH_6)
                    where tbmrec.OVC_VEN_CST.Equals(tbm1302.OVC_VEN_CST)
                    where tbmrec.ONB_GROUP.Equals(tbm1302.ONB_GROUP)
                    where tbmrec.OVC_VEN_CST.Equals(tbmpay.OVC_VEN_CST)
                    where (tbmrec.OVC_DRECEIVE.CompareTo(TC) >= 0 && tbmrec.OVC_DRECEIVE.CompareTo(TC2) <= 0)
                    where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                    where tbmdel.ONB_SHIP_TIMES.Equals(tbmpay.ONB_TIMES)
                    where (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? tbmrec.OVC_DO_NAME.Equals(User) : true
                    orderby new { tbm1301.OVC_PURCH, tbmrec.ONB_SHIP_TIMES }
                    select new
                    {
                        purch = tbm1301.OVC_PURCH,// + tbm1301.OVC_PUR_AGENCY + tbm1302.OVC_PURCH_5 + tbm1302.OVC_PURCH_6,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                        OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                        OVC_YY = tbm1118.OVC_YY,
                        OVC_POI_IBDG = tbm1118.OVC_POI_IBDG,
                        OVC_PJNAME = tbm1118 != null ? tbm1118.OVC_PJNAME : "",
                        ONB_MCONTRACT = tbmdel.ONB_MCONTRACT ?? 0,
                        ONB_PAY_MONEY = tbmpay.ONB_PAY_MONEY ?? 0,
                        OVC_DCONTRACT = tbmrec.OVC_DCONTRACT,
                        OVC_DRECEIVE = tbmrec.OVC_DRECEIVE,
                        OVC_DELIVERY_CONTRACT = tbmdel.OVC_DELIVERY_CONTRACT,
                        OVC_DELIVERY = tbmdel.OVC_DELIVERY,
                        ONB_DAYS_CONTRACT = tbmdel.ONB_DAYS_CONTRACT,//(tbmdel.OVC_DELIVERY != null && tbmrec.OVC_DRECEIVE != null) ? ((Convert.ToDateTime(tbmdel.OVC_DELIVERY) - Convert.ToDateTime(tbmrec.OVC_DRECEIVE)).TotalDays).ToString() : "",
                    OVC_DJOINCHECK = tbmdel != null ? tbmdel.OVC_DJOINCHECK : "",
                        OVC_DPAY = tbmdel.OVC_DPAY,
                        預估結案日 = tbmdel != null ? tbmdel.OVC_DPAY_PLAN : "",
                        use_day = "",//(tbmdel.OVC_DPAY != null && tbmdel.OVC_DPAY != null) ? (Convert.ToDateTime(tbmdel.OVC_DELIVERY) - Convert.ToDateTime(tbmdel.OVC_DELIVERY)).TotalDays.ToString() : "",
                    ONB_DINSPECT_SOP = tbmdel.ONB_DINSPECT_SOP,
                        ONB_DELAY_DAYS = tbmpay.ONB_DELAY_DAYS,
                        OVC_RECEIVE_COMM = tbmrec.OVC_RECEIVE_COMM,
                        OnB_DELAY_DAYS = tbmpay.ONB_DELAY_DAYS ?? 0,
                        onb_mins_money_1 = tbmpay.ONB_MINS_MONEY ?? 0,
                        OVC_GRANT_TO = tbmrec.OVC_GRANT_TO,
                        OVC_DO_NAME = tbmrec.OVC_DO_NAME,
                        OVC_VEN_TITLE = tbmrec.OVC_VEN_TITLE,
                        OVC_CLOSE = tbmrec.OVC_CLOSE
                    };
                var queryFin =
                    from q in query2
                    join p9 in queryP9 on q.OVC_GRANT_TO equals p9.OVC_PHR_ID
                    select new
                    {
                        purch = q.purch,// + tbm1301.OVC_PUR_AGENCY + tbm1302.OVC_PURCH_5 + tbm1302.OVC_PURCH_6,
                    OVC_PUR_IPURCH = q.OVC_PUR_IPURCH,
                        OVC_PUR_NSECTION = q.OVC_PUR_NSECTION,
                        OVC_YY = q.OVC_YY,
                        OVC_POI_IBDG = q.OVC_POI_IBDG,
                        OVC_PJNAME = q.OVC_PJNAME,
                        ONB_MCONTRACT = q.ONB_MCONTRACT,
                        ONB_PAY_MONEY = q.ONB_PAY_MONEY,
                        OVC_DCONTRACT = q.OVC_DCONTRACT,
                        OVC_DRECEIVE = q.OVC_DRECEIVE,
                        OVC_DELIVERY_CONTRACT = q.OVC_DELIVERY_CONTRACT,
                        OVC_DELIVERY = q.OVC_DELIVERY,
                        ONB_DAYS_CONTRACT = q.ONB_DAYS_CONTRACT,//(tbmdel.OVC_DELIVERY != null && tbmrec.OVC_DRECEIVE != null) ? ((Convert.ToDateTime(tbmdel.OVC_DELIVERY) - Convert.ToDateTime(tbmrec.OVC_DRECEIVE)).TotalDays).ToString() : "",
                    OVC_DJOINCHECK = q.OVC_DJOINCHECK,
                        OVC_DPAY = q.OVC_DPAY,
                        預估結案日 = q.預估結案日,
                        use_day = "",//(tbmdel.OVC_DPAY != null && tbmdel.OVC_DPAY != null) ? (Convert.ToDateTime(tbmdel.OVC_DELIVERY) - Convert.ToDateTime(tbmdel.OVC_DELIVERY)).TotalDays.ToString() : "",
                    ONB_DINSPECT_SOP = q.ONB_DINSPECT_SOP,
                        ONB_DELAY_DAYS = q.ONB_DELAY_DAYS,
                        OVC_RECEIVE_COMM = q.OVC_RECEIVE_COMM,
                        OnB_DELAY_DAYS = q.ONB_DELAY_DAYS,
                        onb_mins_money_1 = q.onb_mins_money_1,
                        OVC_GRANT_TO = p9.OVC_PHR_DESC,
                        OVC_DO_NAME = q.OVC_DO_NAME,
                        OVC_VEN_TITLE = q.OVC_VEN_TITLE,
                        OVC_CLOSE = q.OVC_CLOSE
                    };
                //var fin = query.AsEnumerable().OrderBy(d => d.OVC_POI_IBDG);
                dtq = CommonStatic.LinqQueryToDataTable(queryFin);
                FCommon.GridView_dataImport(GV_Q12, dtq);

                print("保固金管制明細表", GV_Q12);
            }
        }
        #endregion
        # region 結案購案統計表 Q13  (O)
        protected void btnClosing_Click(object sender, EventArgs e)
        {
            if (txtClosingD1.Text == "" || txtClosingD2.Text == "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請輸入 日期");
            else
            {
                DataTable dtq = new DataTable();
                string strUSER_ID = Session["userid"].ToString();
                string Use_LEVEL = "";
                string StrSection = "";
                string User = "";
                string TC = txtClosingD1.Text;
                string TC2 = txtClosingD2.Text;
                TBM5200_1 ac = new TBM5200_1();
                TBM5200_PPP acc = new TBM5200_PPP();
                ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                var acc_auth =
                    (from account in MPMS.ACCOUNTs
                     join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                     where account.USER_ID.Equals(strUSER_ID)
                     select new
                     {
                         C_SN_AUTH = account_auth.C_SN_AUTH,
                         DEPT_SN = account.DEPT_SN,
                         USER_NAME = acc.USER_NAME,
                     }).FirstOrDefault();
                if (ac != null && acc != null)
                {
                    Use_LEVEL = ac.OVC_PRIV_LEVEL;
                    StrSection = acc.OVC_PUR_SECTION;
                    User = acc.USER_NAME;
                }
                else if (acc_auth != null)
                {
                    Use_LEVEL = acc_auth.C_SN_AUTH;
                    StrSection = acc_auth.DEPT_SN;
                    User = acc_auth.USER_NAME;
                }

                var t1 = GME.TBM1407.Where(o => o.OVC_PHR_CATE.Equals("P9"));
                var tbmplan = GME.TBM1301_PLAN.Where(o => o.OVC_CONTRACT_UNIT.Equals(StrSection)).DefaultIfEmpty();
                var query = from tbm1301 in MPMS.TBM1301.AsEnumerable()
                            join tbmR in MPMS.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmR.OVC_PURCH
                            join tbmPlan in tbmplan on tbmR.OVC_PURCH equals tbmPlan.OVC_PURCH
                            join tbm118 in MPMS.TBM1118 on tbmR.OVC_PURCH equals tbm118.OVC_PURCH
                            join tbm1302 in MPMS.TBM1302 on new { tbmR.OVC_PURCH, tbmR.OVC_PURCH_6, tbmR.OVC_VEN_CST, tbmR.ONB_GROUP } equals new { tbm1302.OVC_PURCH, tbm1302.OVC_PURCH_6, tbm1302.OVC_VEN_CST, tbm1302.ONB_GROUP }
                            join tbmD in MPMS.TBMDELIVERY on new { tbmR.OVC_PURCH, tbmR.OVC_PURCH_6 } equals new { tbmD.OVC_PURCH, tbmD.OVC_PURCH_6 }
                            join tbmPP in MPMS.TBMPAY_MONEY on new { tbmR.OVC_PURCH, tbmR.OVC_PURCH_6 } equals new { tbmPP.OVC_PURCH, tbmPP.OVC_PURCH_6 } into mm
                            from tbmP in mm
                            join P9 in t1.AsEnumerable() on tbmR.OVC_GRANT_TO equals P9.OVC_PHR_ID
                            where Convert.ToDateTime(tbmD.OVC_DPAY) >= Convert.ToDateTime(TC) && Convert.ToDateTime(tbmD.OVC_DPAY) <= Convert.ToDateTime(TC2).AddDays(1)
                            where (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? tbmR.OVC_DO_NAME.Equals(User) : true
                            where tbmD.ONB_SHIP_TIMES == tbmP.ONB_TIMES
                            orderby tbm1301.OVC_PURCH, tbmD.ONB_SHIP_TIMES
                            select new
                            {
                                purch = tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + tbm1302.OVC_PURCH_5 + tbm1302.OVC_PURCH_6,
                                OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,                                              //購案名稱3
                                OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,                                          //申購單位4
                                OVC_YY = Convert.ToInt32(tbm118.OVC_YY) % 10,                                                     //預算年度5
                                OVC_POI_IBDG = tbm118.OVC_POI_IBDG,                                                      //預算科目6
                                OVC_PJNAME = tbm118 != null ? tbm118.OVC_PJNAME : "",                           //預算科目名稱7
                                ONB_MCONTRACT = String.Format("{0:N}", tbmP.ONB_MCONTRACT),                                             //合約金額8
                                ONB_PAY_MONEY = String.Format("{0:N}", tbmP.ONB_PAY_MONEY),                              //支用金額9
                                OVC_DCONTRACT = tbmR.OVC_DCONTRACT,                                             //簽約日期10
                                OVC_DRECEIVE = tbmR.OVC_DRECEIVE,                                               //收辦日期11
                                OVC_DELIVERY_CONTRACT = tbmD != null ? tbmD.OVC_DELIVERY_CONTRACT : "",         //契約交貨日期12
                                OVC_DELIVERY = tbmD != null ? tbmD.OVC_DELIVERY : "",                           //實際交貨日期13
                                ONB_DAYS_CONTRACT = (tbmD.OVC_DELIVERY != null && tbmR.OVC_DRECEIVE != null) ? ((Convert.ToDateTime(tbmD.OVC_DELIVERY) - Convert.ToDateTime(tbmR.OVC_DRECEIVE))).TotalDays.ToString() : "", //實際交貨天數
                                OVC_DJOINCHECK = tbmD != null ? tbmD.OVC_DJOINCHECK : "",                       //會驗日期15
                                OVC_DPAY = tbmD.OVC_DPAY,                                                       //結案日期16
                                預估結案日 = tbmD != null ? tbmD.OVC_DPAY_PLAN : "",                             //預估結案日17
                                use_day = (tbmD.OVC_DPAY != null && tbmD.OVC_DPAY != null) ? (Convert.ToDateTime(tbmD.OVC_DELIVERY) - Convert.ToDateTime(tbmD.OVC_DELIVERY)).TotalDays.ToString() : "", //使用天數
                                ONB_DINSPECT_SOP = tbmD.ONB_DINSPECT_SOP,                                       //標準驗收天數19
                                ONB_DELAY_DAYS = tbmP.ONB_DELAY_DAYS,                                           //延誤天數20
                                OVC_RECEIVE_COMM = tbmR.OVC_RECEIVE_COMM,                                       //重要事項21
                                OnB_DELAY_DAYS = tbmP != null ? tbmP.ONB_DELAY_DAYS : 0,                        //逾時計罰
                                onb_mins_money_1 = tbmP.ONB_MINS_MONEY,                                         //減價收受
                                OVC_GRANT_TO = P9.OVC_PHR_DESC,                       //下授
                                OVC_DO_NAME = tbmR.OVC_DO_NAME,             //承辦人
                                OVC_VEN_TITLE = tbmR.OVC_VEN_TITLE,    //得標商名稱
                                OVC_CLOSE = tbmR.OVC_CLOSE     //歸檔(是/否)
                            };
                dtq = CommonStatic.LinqQueryToDataTable(query);
                FCommon.GridView_dataImport(GV_Q13, dtq);

                print("結案購案統計表", GV_Q13);
            }
        }
        #endregion
        # region 會驗購案統計表 Q14 (與SQL相同，與範例不同)
        protected void btnDJOINCHECK_Click(object sender, EventArgs e)
        {
            if (txtDJOINCHECK.Text == "" || txtDJOINCHECK2.Text == "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請輸入 日期");
            else
            {
                DataTable dtq = new DataTable();
                string strUSER_ID = Session["userid"].ToString();
                string Use_LEVEL = "";
                string StrSection = "";
                string User = "";
                string TC = txtDJOINCHECK.Text;
                string TC2 = txtDJOINCHECK2.Text;
                TBM5200_1 ac = new TBM5200_1();
                TBM5200_PPP acc = new TBM5200_PPP();
                ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                var acc_auth =
                    (from account in MPMS.ACCOUNTs
                     join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                     where account.USER_ID.Equals(strUSER_ID)
                     select new
                     {
                         C_SN_AUTH = account_auth.C_SN_AUTH,
                         DEPT_SN = account.DEPT_SN,
                         USER_NAME = acc.USER_NAME,
                     }).FirstOrDefault();
                if (ac != null && acc != null)
                {
                    Use_LEVEL = ac.OVC_PRIV_LEVEL;
                    StrSection = acc.OVC_PUR_SECTION;
                    User = acc.USER_NAME;
                }
                else if (acc_auth != null)
                {
                    Use_LEVEL = acc_auth.C_SN_AUTH;
                    StrSection = acc_auth.DEPT_SN;
                    User = acc_auth.USER_NAME;
                }

                var queryP9 = MPMS.TBM1407
                    .Where(o => o.OVC_PHR_CATE.Equals("P9"));
                var query2 =
                    from tbm1301 in MPMS.TBM1301
                    join tbm1302 in MPMS.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                    join tbmrec in MPMS.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmrec.OVC_PURCH
                    join tbmdel in MPMS.TBMDELIVERY on tbm1301.OVC_PURCH equals tbmdel.OVC_PURCH
                    join tbmpay in MPMS.TBMPAY_MONEY on tbm1301.OVC_PURCH equals tbmpay.OVC_PURCH
                    join tbm1301_plan in MPMS.TBM1301_PLAN on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                    join tbm1118 in MPMS.TBM1118 on tbm1301.OVC_PURCH equals tbm1118.OVC_PURCH
                    where tbmrec.OVC_PURCH_6.Equals(tbmdel.OVC_PURCH_6)
                    where tbmrec.OVC_PURCH_6.Equals(tbm1302.OVC_PURCH_6)
                    where tbmrec.OVC_VEN_CST.Equals(tbm1302.OVC_VEN_CST)
                    where tbmrec.ONB_GROUP.Equals(tbm1302.ONB_GROUP)
                    where tbmrec.OVC_VEN_CST.Equals(tbmpay.OVC_VEN_CST)
                    where (tbmdel.OVC_DJOINCHECK.CompareTo(TC) >= 0 && tbmdel.OVC_DJOINCHECK.CompareTo(TC2) <= 0)
                    where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                    where tbmdel.ONB_SHIP_TIMES.Equals(tbmpay.ONB_TIMES)
                    where (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? tbmrec.OVC_DO_NAME.Equals(User) : true
                    orderby new { tbm1301.OVC_PURCH, tbmrec.ONB_SHIP_TIMES }
                    select new
                    {
                        purch = tbm1301.OVC_PURCH,// + tbm1301.OVC_PUR_AGENCY + tbm1302.OVC_PURCH_5 + tbm1302.OVC_PURCH_6,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                        OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                        OVC_YY = tbm1118.OVC_YY,
                        OVC_POI_IBDG = tbm1118.OVC_POI_IBDG,
                        OVC_PJNAME = tbm1118 != null ? tbm1118.OVC_PJNAME : "",
                        ONB_MCONTRACT = tbmdel.ONB_MCONTRACT ?? 0,
                        ONB_PAY_MONEY = tbmpay.ONB_PAY_MONEY ?? 0,
                        OVC_DCONTRACT = tbmrec.OVC_DCONTRACT,
                        OVC_DRECEIVE = tbmrec.OVC_DRECEIVE,
                        OVC_DELIVERY_CONTRACT = tbmdel.OVC_DELIVERY_CONTRACT,
                        OVC_DELIVERY = tbmdel.OVC_DELIVERY,
                        ONB_DAYS_CONTRACT = tbmdel.ONB_DAYS_CONTRACT,//(tbmdel.OVC_DELIVERY != null && tbmrec.OVC_DRECEIVE != null) ? ((Convert.ToDateTime(tbmdel.OVC_DELIVERY) - Convert.ToDateTime(tbmrec.OVC_DRECEIVE)).TotalDays).ToString() : "",
                    OVC_DJOINCHECK = tbmdel != null ? tbmdel.OVC_DJOINCHECK : "",
                        OVC_DPAY = tbmdel.OVC_DPAY,
                        預估結案日 = tbmdel != null ? tbmdel.OVC_DPAY_PLAN : "",
                        use_day = "",//(tbmdel.OVC_DPAY != null && tbmdel.OVC_DPAY != null) ? (Convert.ToDateTime(tbmdel.OVC_DELIVERY) - Convert.ToDateTime(tbmdel.OVC_DELIVERY)).TotalDays.ToString() : "",
                    ONB_DINSPECT_SOP = tbmdel.ONB_DINSPECT_SOP,
                        ONB_DELAY_DAYS = tbmpay.ONB_DELAY_DAYS,
                        OVC_RECEIVE_COMM = tbmrec.OVC_RECEIVE_COMM,
                        OnB_DELAY_DAYS = tbmpay.ONB_DELAY_DAYS ?? 0,
                        onb_mins_money_1 = tbmpay.ONB_MINS_MONEY ?? 0,
                        OVC_GRANT_TO = tbmrec.OVC_GRANT_TO,
                        OVC_DO_NAME = tbmrec.OVC_DO_NAME,
                        OVC_VEN_TITLE = tbmrec.OVC_VEN_TITLE,
                        OVC_CLOSE = tbmrec.OVC_CLOSE
                    };
                var queryFin =
                    from q in query2
                    join p9 in queryP9 on q.OVC_GRANT_TO equals p9.OVC_PHR_ID
                    select new
                    {
                        purch = q.purch,// + tbm1301.OVC_PUR_AGENCY + tbm1302.OVC_PURCH_5 + tbm1302.OVC_PURCH_6,
                    OVC_PUR_IPURCH = q.OVC_PUR_IPURCH,
                        OVC_PUR_NSECTION = q.OVC_PUR_NSECTION,
                        OVC_YY = q.OVC_YY,
                        OVC_POI_IBDG = q.OVC_POI_IBDG,
                        OVC_PJNAME = q.OVC_PJNAME,
                        ONB_MCONTRACT = q.ONB_MCONTRACT,
                        ONB_PAY_MONEY = q.ONB_PAY_MONEY,
                        OVC_DCONTRACT = q.OVC_DCONTRACT,
                        OVC_DRECEIVE = q.OVC_DRECEIVE,
                        OVC_DELIVERY_CONTRACT = q.OVC_DELIVERY_CONTRACT,
                        OVC_DELIVERY = q.OVC_DELIVERY,
                        ONB_DAYS_CONTRACT = q.ONB_DAYS_CONTRACT,//(tbmdel.OVC_DELIVERY != null && tbmrec.OVC_DRECEIVE != null) ? ((Convert.ToDateTime(tbmdel.OVC_DELIVERY) - Convert.ToDateTime(tbmrec.OVC_DRECEIVE)).TotalDays).ToString() : "",
                    OVC_DJOINCHECK = q.OVC_DJOINCHECK,
                        OVC_DPAY = q.OVC_DPAY,
                        預估結案日 = q.預估結案日,
                        use_day = "",//(tbmdel.OVC_DPAY != null && tbmdel.OVC_DPAY != null) ? (Convert.ToDateTime(tbmdel.OVC_DELIVERY) - Convert.ToDateTime(tbmdel.OVC_DELIVERY)).TotalDays.ToString() : "",
                    ONB_DINSPECT_SOP = q.ONB_DINSPECT_SOP,
                        ONB_DELAY_DAYS = q.ONB_DELAY_DAYS,
                        OVC_RECEIVE_COMM = q.OVC_RECEIVE_COMM,
                        OnB_DELAY_DAYS = q.ONB_DELAY_DAYS,
                        onb_mins_money_1 = q.onb_mins_money_1,
                        OVC_GRANT_TO = p9.OVC_PHR_DESC,
                        OVC_DO_NAME = q.OVC_DO_NAME,
                        OVC_VEN_TITLE = q.OVC_VEN_TITLE,
                        OVC_CLOSE = q.OVC_CLOSE
                    };
                dtq = CommonStatic.LinqQueryToDataTable(queryFin);
                FCommon.GridView_dataImport(GV_Q14, dtq);

                print("會驗購案統計表", GV_Q14);
            }
        }
        #endregion
        #region 支用逾罰購案統計表 Q15 (null 與 0 的替換)
        protected void btnPenaltyfuse_Click(object sender, EventArgs e)
        {
            if (txtPenaltyfuse.Text == "" || txtPenaltyfuse2.Text == "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請輸入 日期");
            else
            {
                DataTable dtq = new DataTable();
                string strUSER_ID = Session["userid"].ToString();
                string Use_LEVEL = "";
                string StrSection = "";
                string User = "";
                string TC = txtPenaltyfuse.Text;
                string TC2 = txtPenaltyfuse2.Text;
                TBM5200_1 ac = new TBM5200_1();
                TBM5200_PPP acc = new TBM5200_PPP();
                ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                var acc_auth =
                    (from account in MPMS.ACCOUNTs
                     join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                     where account.USER_ID.Equals(strUSER_ID)
                     select new
                     {
                         C_SN_AUTH = account_auth.C_SN_AUTH,
                         DEPT_SN = account.DEPT_SN,
                         USER_NAME = acc.USER_NAME,
                     }).FirstOrDefault();
                if (ac != null && acc != null)
                {
                    Use_LEVEL = ac.OVC_PRIV_LEVEL;
                    StrSection = acc.OVC_PUR_SECTION;
                    User = acc.USER_NAME;
                }
                else if (acc_auth != null)
                {
                    Use_LEVEL = acc_auth.C_SN_AUTH;
                    StrSection = acc_auth.DEPT_SN;
                    User = acc_auth.USER_NAME;
                }

                var tbmplan = GME.TBM1301_PLAN.Where(o => o.OVC_CONTRACT_UNIT.Equals(StrSection)).DefaultIfEmpty();
                var query = from tbm1301 in MPMS.TBM1301.AsEnumerable()
                            join tbmR in MPMS.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmR.OVC_PURCH
                            join tbmPlan in tbmplan on tbmR.OVC_PURCH equals tbmPlan.OVC_PURCH
                            join tbm1302 in MPMS.TBM1302 on new { tbmR.OVC_PURCH, tbmR.OVC_PURCH_6, tbmR.OVC_VEN_CST, tbmR.ONB_GROUP } equals new { tbm1302.OVC_PURCH, tbm1302.OVC_PURCH_6, tbm1302.OVC_VEN_CST, tbm1302.ONB_GROUP }
                            join tbmPP in MPMS.TBMPAY_MONEY on new { tbmR.OVC_PURCH, tbmR.OVC_PURCH_6 } equals new { tbmPP.OVC_PURCH, tbmPP.OVC_PURCH_6 } into mm
                            from tbmP in mm
                            where Convert.ToDateTime(tbmP.OVC_DPAY) >= Convert.ToDateTime(TC) && Convert.ToDateTime(tbmP.OVC_DPAY) <= Convert.ToDateTime(TC2).AddDays(1)
                            where (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? tbmR.OVC_DO_NAME.Equals(User) : true
                            where tbmP.ONB_DELAY_MONEY > 0
                            orderby tbm1301.OVC_PURCH, tbmR.ONB_SHIP_TIMES
                            select new
                            {
                                purch = tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + tbm1302.OVC_PURCH_5 + tbm1302.OVC_PURCH_6,
                                OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,                                                    //購案名稱
                                OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,                                    //申購單位
                                ONB_MONEY = tbm1302 != null ? String.Format("{0:N}", tbm1302.ONB_MONEY) ?? "0" : "0",                                                //合約金額
                                ONB_TIMES = tbmP.ONB_TIMES,                                                     //批次
                                OVC_DPAY = tbmP.OVC_DPAY,                                                       //結案日期
                                ONB_INSPECT_MONEY = tbmP.ONB_INSPECT_MONEY != null ? String.Format("{0:N}", tbmP.ONB_INSPECT_MONEY) ?? "0" : "0",               //驗收金額
                                ONB_ONB_PAY_MONEY = tbmP.ONB_PAY_MONEY != null ? String.Format("{0:N}", tbmP.ONB_PAY_MONEY) ?? "0" : "0",//支用金額
                                OnB_DELAY_MONEY = tbmP.ONB_DELAY_MONEY != null ? String.Format("{0:N}", tbmP.ONB_DELAY_MONEY) ?? "0" : "0", //逾期違約金
                                onb_mins_money_1 = tbmP.ONB_MINS_MONEY != null ? String.Format("{0:N}", tbmP.ONB_MINS_MONEY) ?? "0" : "0",  //減價收受
                                OVC_DO_NAME = tbmR.OVC_DO_NAME,                                                 //承辦人
                            };
                dtq = CommonStatic.LinqQueryToDataTable(query);
                FCommon.GridView_dataImport(GV_Q15, dtq);

                print("購案支用逾期罰款總表", GV_Q15);
            }
        }
        #endregion
        #region   履驗分年分批時程管制總表 Q16 (與範例欄位完全不同，但與SQL相同)
        protected void btnAnnualBatchC_Click(object sender, EventArgs e)
        {
            DataTable dtq = new DataTable();

            string strUSER_ID = Session["userid"].ToString();
            string Use_LEVEL = "";
            string StrSection = "";
            string User = "";
            string TC = int.Parse(drpAnnualBatchC.Text) > 99 ? drpAnnualBatchC.Text.Substring(1) : drpAnnualBatchC.Text;
            TBM5200_1 ac = new TBM5200_1();
            TBM5200_PPP acc = new TBM5200_PPP();
            ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            var acc_auth =
                (from account in MPMS.ACCOUNTs
                 join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                 where account.USER_ID.Equals(strUSER_ID)
                 select new
                 {
                     C_SN_AUTH = account_auth.C_SN_AUTH,
                     DEPT_SN = account.DEPT_SN,
                     USER_NAME = acc.USER_NAME,
                 }).FirstOrDefault();
            if (ac != null && acc != null)
            {
                Use_LEVEL = ac.OVC_PRIV_LEVEL;
                StrSection = acc.OVC_PUR_SECTION;
                User = acc.USER_NAME;
            }
            else if (acc_auth != null)
            {
                Use_LEVEL = acc_auth.C_SN_AUTH;
                StrSection = acc_auth.DEPT_SN;
                User = acc_auth.USER_NAME;
            }

            var query =
                 from tbm1301 in MPMS.TBM1301
                 join tbm1302 in MPMS.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                 join tbmrec in MPMS.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmrec.OVC_PURCH
                 join tbmdel in MPMS.TBMDELIVERY on tbm1301.OVC_PURCH equals tbmdel.OVC_PURCH
                 join tbmpay in MPMS.TBMPAY_MONEY on tbm1301.OVC_PURCH equals tbmpay.OVC_PURCH
                 join tbm1301_plan in MPMS.TBM1301_PLAN on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                 join tbm1118 in MPMS.TBM1118 on tbm1301.OVC_PURCH equals tbm1118.OVC_PURCH
                 where tbm1301.OVC_PURCH.Substring(2, 2).Equals(TC)
                 where tbmrec.OVC_PURCH_6.Equals(tbmdel.OVC_PURCH_6)
                 where tbmrec.OVC_PURCH_6.Equals(tbm1302.OVC_PURCH_6)
                 where tbmrec.OVC_VEN_CST.Equals(tbm1302.OVC_VEN_CST)
                 where tbmrec.ONB_GROUP.Equals(tbm1302.ONB_GROUP)
                 where tbmrec.OVC_PURCH_6.Equals(tbmpay.OVC_PURCH_6)
                 where tbmrec.OVC_VEN_CST.Equals(tbmpay.OVC_VEN_CST)
                 where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                 where tbmdel.ONB_SHIP_TIMES.Equals(tbmpay.ONB_TIMES)
                 where (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? tbmrec.OVC_DO_NAME.Equals(User) : true
                 orderby new { tbm1301.OVC_PURCH, tbmrec.ONB_SHIP_TIMES }
                 select new
                 {
                     OVC_PURCH = tbm1301.OVC_PURCH,
                     OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                     OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                     OVC_YY = MPMS.TBM1118.Where(o => o.OVC_PURCH.Equals(tbm1301.OVC_PURCH)).Select(o => o.OVC_YY).FirstOrDefault(),
                     OVC_POI_IBDG = MPMS.TBM1118.Where(o => o.OVC_PURCH.Equals(tbm1301.OVC_PURCH)).OrderBy(o => o.OVC_POI_IBDG).Select(o => o.OVC_POI_IBDG).FirstOrDefault() ?? "",
                     OVC_PJNAME = MPMS.TBM1118.Where(o => o.OVC_PURCH.Equals(tbm1301.OVC_PURCH)).OrderBy(o => o.OVC_POI_IBDG).Select(o => o.OVC_PJNAME).FirstOrDefault() ?? "",
                     OVC_LAB = MPMS.TBM1407.Where(o => o.OVC_PHR_CATE.Equals("GN") && o.OVC_PHR_ID.Equals(tbm1301.OVC_LAB)).Select(o => o.OVC_PHR_DESC).FirstOrDefault() ?? tbm1301.OVC_LAB,
                     ONB_MCONTRACT = tbmdel.ONB_MCONTRACT,
                     ONB_PAY_MONEY = tbmpay.ONB_PAY_MONEY,
                     OVC_DCONTRACT = tbmrec.OVC_DCONTRACT,
                     OVC_DELIVERY_CONTRACT = tbmdel.OVC_DELIVERY_CONTRACT,
                     OVC_DELIVERY = tbmdel.OVC_DELIVERY,
                     OVC_DJOINCHECK = tbmdel.OVC_DJOINCHECK,
                     OVC_DPAY = tbmdel.OVC_DPAY,
                     OVC_DAPPLY = tbm1301_plan.OVC_DAPPLY,
                     OVC_DCLOSE = tbmrec.OVC_DCLOSE,
                     OVC_STATUS = tbmrec.OVC_STATUS,
                     use_day = "0",
                     ONB_DINSPECT_SOP = tbmdel.ONB_DINSPECT_SOP,
                     ONB_DELAY_DAYS = tbmpay.ONB_DELAY_DAYS,
                     OVC_RECEIVE_COMM = tbmrec.OVC_RECEIVE_COMM,
                     ONB_DELAY_MONEY = tbmpay.ONB_DELAY_MONEY ?? 0,
                     ONB_MINS_MONEY = tbmpay.ONB_MINS_MONEY ?? 0,
                     OVC_GRANT_TO = MPMS.TBM1407.Where(o => o.OVC_PHR_CATE.Equals("P9") && o.OVC_PHR_ID.Equals(tbmrec.OVC_GRANT_TO)).Select(o => o.OVC_PHR_DESC).FirstOrDefault() ?? tbmrec.OVC_GRANT_TO,
                     OVC_DO_NAME = tbmrec.OVC_DO_NAME,
                     OVC_VEN_TITLE = tbmrec.OVC_VEN_TITLE,
                     OVC_CLOSE = tbmrec.OVC_CLOSE
                 };
            
            dtq = CommonStatic.LinqQueryToDataTable(query);
            FCommon.GridView_dataImport(GV_Q16, dtq);

            print("履驗分年分批時程管制總表", GV_Q16);
        }
        #endregion
        #region 新式保險金 Q17 (union後欄位錯誤、資料轉換問題)
        protected void btnNSecuritydeposit_Click(object sender, EventArgs e)
        {

            DataTable dtq = new DataTable();


            string strUSER_ID = Session["userid"].ToString();
            string Use_LEVEL = "";
            string StrSection = "";
            string User = "";
            string TC = int.Parse(drpNSecuritydeposit.Text) > 99 ? drpNSecuritydeposit.Text.Substring(1) : drpNSecuritydeposit.Text;
            TBM5200_1 ac = new TBM5200_1();
            TBM5200_PPP acc = new TBM5200_PPP();
            ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            var acc_auth =
                (from account in MPMS.ACCOUNTs
                 join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                 where account.USER_ID.Equals(strUSER_ID)
                 select new
                 {
                     C_SN_AUTH = account_auth.C_SN_AUTH,
                     DEPT_SN = account.DEPT_SN,
                     USER_NAME = acc.USER_NAME,
                 }).FirstOrDefault();
            if (ac != null && acc != null)
            {
                Use_LEVEL = ac.OVC_PRIV_LEVEL;
                StrSection = acc.OVC_PUR_SECTION;
                User = acc.USER_NAME;
            }
            else if (acc_auth != null)
            {
                Use_LEVEL = acc_auth.C_SN_AUTH;
                StrSection = acc_auth.DEPT_SN;
                User = acc_auth.USER_NAME;
            }

            var query1301_p = MPMS.TBM1301_PLAN
                .Where(o => o.OVC_CONTRACT_UNIT.Equals(StrSection));

            var queryrec = MPMS.TBMRECEIVE_CONTRACT
                .Where(o => (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? o.OVC_DO_NAME.Equals(User) : true);

            var querycash = MPMS.TBMMANAGE_CASH
                .Where(o => o.OVC_PURCH.Substring(2, 2).Equals(TC));

            var querystock = MPMS.TBMMANAGE_STOCK
                .Where(o => o.OVC_PURCH.Substring(2, 2).Equals(TC));

            var queryprom = MPMS.TBMMANAGE_PROM
                .Where(o => o.OVC_PURCH.Substring(2, 2).Equals(TC));

            var queryCash =
                from tbm1301 in MPMS.TBM1301
                join cash in querycash on tbm1301.OVC_PURCH equals cash.OVC_PURCH
                join tbm1301_plan in query1301_p on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                join tbmreceivc in queryrec on tbm1301.OVC_PURCH equals tbmreceivc.OVC_PURCH
                //where tbm1301.OVC_PURCH.Substring(2, 2).Equals(TC)
                where cash.OVC_PURCH_6.Equals(tbmreceivc.OVC_PURCH_6)
                where cash.OVC_OWN_NO.Equals(tbmreceivc.OVC_VEN_CST)
                //where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                //where tbmreceivc.OVC_DO_NAME.Equals(User)
                select new
                {
                    OVC_PURCH = tbm1301.OVC_PURCH,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    unit = tbm1301.OVC_PUR_NSECTION,
                    money = tbmreceivc.ONB_MONEY ?? 0,
                    KIND = cash.OVC_KIND,
                    PRO = "現金",
                    OVC_COMPTROLLER_NO = cash.OVC_COMPTROLLER_NO,
                    ONB_ALL_MONEY = cash.ONB_ALL_MONEY ?? 0,
                    cur = cash.OVC_CURRENT_1,
                    OVC_OWN_NAME = cash.OVC_OWN_NAME,
                    OVC_ONNAME = cash.OVC_ONNAME,
                    OVC_DGARRENT_START = cash.OVC_MARK,
                    OVC_DGARRENT_END = "",
                    OVC_DEFFECT_1 = "",
                    OVC_DBACK = cash.OVC_DBACK,
                    OVC_DO_NAME = tbmreceivc.OVC_DO_NAME
                };

            var queryStock =
                from tbm1301 in MPMS.TBM1301
                join stock in querystock on tbm1301.OVC_PURCH equals stock.OVC_PURCH
                join tbm1301_plan in query1301_p on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                join tbmreceivc in queryrec on tbm1301.OVC_PURCH equals tbmreceivc.OVC_PURCH
                //where tbm1301.OVC_PURCH.Substring(2, 2).Equals(TC)
                where stock.OVC_PURCH_6.Equals(tbmreceivc.OVC_PURCH_6)
                where stock.OVC_OWN_NO.Equals(tbmreceivc.OVC_VEN_CST)
                //where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                //where tbmreceivc.OVC_DO_NAME.Equals(User)
                select new
                {
                    OVC_PURCH = tbm1301.OVC_PURCH,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    unit = tbm1301.OVC_PUR_NSECTION,
                    money = tbmreceivc.ONB_MONEY ?? 0,
                    KIND = stock.OVC_KIND,
                    PRO = "有價證券",
                    OVC_COMPTROLLER_NO = stock.OVC_COMPTROLLER_NO,
                    ONB_ALL_MONEY = stock.ONB_ALL_MONEY ?? 0,
                    cur = stock.OVC_CURRENT_1,
                    OVC_OWN_NAME = stock.OVC_OWN_NAME,
                    OVC_ONNAME = stock.OVC_ONNAME,
                    OVC_DGARRENT_START = stock.OVC_MARK,
                    OVC_DGARRENT_END = "",
                    OVC_DEFFECT_1 = "",
                    OVC_DBACK = stock.OVC_DBACK,
                    OVC_DO_NAME = tbmreceivc.OVC_DO_NAME
                };
            var queryProm =
                from tbm1301 in MPMS.TBM1301
                join prom in queryprom on tbm1301.OVC_PURCH equals prom.OVC_PURCH
                join tbm1301_plan in query1301_p on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                join tbmreceivc in queryrec on tbm1301.OVC_PURCH equals tbmreceivc.OVC_PURCH
                //where tbm1301.OVC_PURCH.Substring(2, 2).Equals(TC)
                where prom.OVC_PURCH_6.Equals(tbmreceivc.OVC_PURCH_6)
                where prom.OVC_OWNED_NO.Equals(tbmreceivc.OVC_VEN_CST)
                //where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                //where tbmreceivc.OVC_DO_NAME.Equals(User)
                select new
                {
                    OVC_PURCH = tbm1301.OVC_PURCH,
                    OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                    unit = tbm1301.OVC_PUR_NSECTION,
                    money = tbmreceivc.ONB_MONEY ?? 0,
                    KIND = prom.OVC_KIND,
                    PRO = "保證文件",
                    OVC_COMPTROLLER_NO = prom.OVC_COMPTROLLER_NO,
                    ONB_ALL_MONEY = prom.ONB_ALL_MONEY ?? 0,
                    cur = prom.OVC_CURRENT,
                    OVC_OWN_NAME = prom.OVC_OWNED_NAME,
                    OVC_ONNAME = prom.OVC_ONNAME,
                    OVC_DGARRENT_START = prom.OVC_MARK,
                    OVC_DGARRENT_END = "",
                    OVC_DEFFECT_1 = prom.OVC_DEFFECT_1,
                    OVC_DBACK = prom.OVC_DBACK,
                    OVC_DO_NAME = tbmreceivc.OVC_DO_NAME
                };
            var fin = queryCash.Concat(queryStock).Concat(queryProm).AsEnumerable().OrderBy(o => o.OVC_PURCH);
            dtq = CommonStatic.LinqQueryToDataTable(fin);
            FCommon.GridView_dataImport(GV_Q17, dtq);

            print("保固金管制明細總表", GV_Q17);
        }
        #endregion
        #region 保證金到期管制明細表 Q18(資料轉換問題)
        protected void btnMarginExpiration_Click(object sender, EventArgs e)
        {
            if (txtMarginExpiration.Text == "" || txtMarginExpiration2.Text == "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請輸入 日期");
            else
            {
                DataTable dtq = new DataTable();
                string strUSER_ID = Session["userid"].ToString();
                string Use_LEVEL = "";
                string StrSection = "";
                string User = "";
                string TC = txtMarginExpiration.Text;
                string TC2 = txtMarginExpiration2.Text;
                TBM5200_1 ac = new TBM5200_1();
                TBM5200_PPP acc = new TBM5200_PPP();
                ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                var acc_auth =
                    (from account in MPMS.ACCOUNTs
                     join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                     where account.USER_ID.Equals(strUSER_ID)
                     select new
                     {
                         C_SN_AUTH = account_auth.C_SN_AUTH,
                         DEPT_SN = account.DEPT_SN,
                         USER_NAME = acc.USER_NAME,
                     }).FirstOrDefault();
                if (ac != null && acc != null)
                {
                    Use_LEVEL = ac.OVC_PRIV_LEVEL;
                    StrSection = acc.OVC_PUR_SECTION;
                    User = acc.USER_NAME;
                }
                else if (acc_auth != null)
                {
                    Use_LEVEL = acc_auth.C_SN_AUTH;
                    StrSection = acc_auth.DEPT_SN;
                    User = acc_auth.USER_NAME;
                }

                var tbmplan = MPMS.TBM1301_PLAN.Where(o => o.OVC_CONTRACT_UNIT.Equals(StrSection)).DefaultIfEmpty();

                var query1301_p = MPMS.TBM1301_PLAN
                    .Where(o => o.OVC_CONTRACT_UNIT.Equals(StrSection));

                var queryrec = MPMS.TBMRECEIVE_CONTRACT
                    .Where(o => (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? o.OVC_DO_NAME.Equals(User) : true);

                var querycash = MPMS.TBMMANAGE_CASH
                    .Where(o => o.OVC_DGARRENT_END.CompareTo(TC) >= 0 && o.OVC_DGARRENT_END.CompareTo(TC2) <= 0);

                var querystock = MPMS.TBMMANAGE_STOCK
                    .Where(o => o.OVC_DGARRENT_END.CompareTo(TC) >= 0 && o.OVC_DGARRENT_END.CompareTo(TC2) <= 0);

                var queryprom = MPMS.TBMMANAGE_PROM
                    .Where(o => o.OVC_DGARRENT_END.CompareTo(TC) >= 0 && o.OVC_DGARRENT_END.CompareTo(TC2) <= 0);

                var Cash =
                    from tbm1301 in MPMS.TBM1301
                    join cash in querycash on tbm1301.OVC_PURCH equals cash.OVC_PURCH
                    join tbm1301_plan in query1301_p on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                    join tbmrec in queryrec on tbm1301.OVC_PURCH equals tbmrec.OVC_PURCH
                    where cash.OVC_PURCH_6.Equals(tbmrec.OVC_PURCH_6)
                    where cash.OVC_OWN_NO.Equals(tbmrec.OVC_VEN_CST)
                    //where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                    //where tbmrec.OVC_DO_NAME.Equals(User)
                    //where cash.OVC_DGARRENT_END.CompareTo(TC) >= 0 && cash.OVC_DGARRENT_END.CompareTo(TC2) <= 0
                    //where (cash.OVC_DGARRENT_END.Equals(null) ? cash.OVC_MARK.Substring(cash.OVC_MARK.IndexOf("日至") + 2, cash.OVC_MARK.IndexOf("日止") - cash.OVC_MARK.IndexOf("日至") + 2).CompareTo(TC) >= 0 && cash.OVC_MARK.Substring(cash.OVC_MARK.IndexOf("日至") + 2, cash.OVC_MARK.IndexOf("日止") - cash.OVC_MARK.IndexOf("日至") + 1).CompareTo(TC) <= 0 : true)
                    select new
                    {
                        OVC_PURCH = tbm1301.OVC_PURCH,
                        OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                        OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                        ONB_MONEY = tbmrec.ONB_MONEY ?? 0,
                        KIND = cash.OVC_KIND,
                        PRO = "現金",
                        OVC_COMPTROLLER_NO = cash.OVC_COMPTROLLER_NO,
                        ONB_ALL_MONEY = cash.ONB_ALL_MONEY ?? 0,
                        cur = cash.OVC_CURRENT_1,
                        OVC_OWN_NAME = cash.OVC_OWN_NAME,
                        OVC_ONNAME = cash.OVC_ONNAME,
                        OVC_DGARRENT_START = cash.OVC_DGARRENT_START,
                        OVC_DGARRENT_END = cash.OVC_DGARRENT_END,
                        OVC_DEFFECT_1 = "",
                        OVC_DBACK = cash.OVC_DBACK,
                        OVC_DO_NAME = tbmrec.OVC_DO_NAME
                    };
                var Stock =
                    from tbm1301 in MPMS.TBM1301
                    join stock in querystock on tbm1301.OVC_PURCH equals stock.OVC_PURCH
                    join tbm1301_plan in query1301_p on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                    join tbmrec in queryrec on tbm1301.OVC_PURCH equals tbmrec.OVC_PURCH
                    where stock.OVC_PURCH_6.Equals(tbmrec.OVC_PURCH_6)
                    where stock.OVC_OWN_NO.Equals(tbmrec.OVC_VEN_CST)
                    //where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                    //where tbmrec.OVC_DO_NAME.Equals(User)
                    //where stock.OVC_DGARRENT_END.CompareTo(TC) >= 0 && stock.OVC_DGARRENT_END.CompareTo(TC2) <= 0
                    //where (stock.OVC_DGARRENT_END.Equals(null) ? stock.OVC_MARK.Substring(stock.OVC_MARK.IndexOf("日至") + 2, stock.OVC_MARK.IndexOf("日止") - stock.OVC_MARK.IndexOf("日至") + 2).CompareTo(TC) >= 0 && stock.OVC_MARK.Substring(stock.OVC_MARK.IndexOf("日至") + 2, stock.OVC_MARK.IndexOf("日止") - stock.OVC_MARK.IndexOf("日至") + 1).CompareTo(TC) <= 0 : true)
                    select new
                    {
                        OVC_PURCH = tbm1301.OVC_PURCH,
                        OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                        OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                        ONB_MONEY = tbmrec.ONB_MONEY ?? 0,
                        KIND = stock.OVC_KIND,
                        PRO = "有價證券",
                        OVC_COMPTROLLER_NO = stock.OVC_COMPTROLLER_NO,
                        ONB_ALL_MONEY = stock.ONB_ALL_MONEY ?? 0,
                        cur = stock.OVC_CURRENT_1,
                        OVC_OWN_NAME = stock.OVC_OWN_NAME,
                        OVC_ONNAME = stock.OVC_ONNAME,
                        OVC_DGARRENT_START = stock.OVC_DGARRENT_START,
                        OVC_DGARRENT_END = stock.OVC_DGARRENT_END,
                        OVC_DEFFECT_1 = "",
                        OVC_DBACK = stock.OVC_DBACK,
                        OVC_DO_NAME = tbmrec.OVC_DO_NAME
                    };
                var Prom =
                    from tbm1301 in MPMS.TBM1301
                    join prom in queryprom on tbm1301.OVC_PURCH equals prom.OVC_PURCH
                    join tbm1301_plan in query1301_p on tbm1301.OVC_PURCH equals tbm1301_plan.OVC_PURCH
                    join tbmrec in queryrec on tbm1301.OVC_PURCH equals tbmrec.OVC_PURCH
                    where prom.OVC_PURCH_6.Equals(tbmrec.OVC_PURCH_6)
                    where prom.OVC_OWNED_NO.Equals(tbmrec.OVC_VEN_CST)
                    //where tbm1301_plan.OVC_CONTRACT_UNIT.Equals(StrSection)
                    //where tbmrec.OVC_DO_NAME.Equals(User)
                    //where prom.OVC_DGARRENT_END.CompareTo(TC) >= 0 && prom.OVC_DGARRENT_END.CompareTo(TC2) <= 0
                    //where (prom.OVC_DGARRENT_END.Equals(null) ? prom.OVC_MARK.Substring(prom.OVC_MARK.IndexOf("日至") + 2, prom.OVC_MARK.IndexOf("日止") - prom.OVC_MARK.IndexOf("日至") + 2).CompareTo(TC) >= 0 && prom.OVC_MARK.Substring(prom.OVC_MARK.IndexOf("日至") + 2, prom.OVC_MARK.IndexOf("日止") - prom.OVC_MARK.IndexOf("日至") + 1).CompareTo(TC) <= 0 : true)
                    select new
                    {
                        OVC_PURCH = tbm1301.OVC_PURCH,
                        OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                        OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                        ONB_MONEY = tbmrec.ONB_MONEY ?? 0,
                        KIND = prom.OVC_KIND,
                        PRO = "保證文件",
                        OVC_COMPTROLLER_NO = prom.OVC_COMPTROLLER_NO,
                        ONB_ALL_MONEY = prom.ONB_ALL_MONEY ?? 0,
                        cur = prom.OVC_CURRENT,
                        OVC_OWN_NAME = prom.OVC_OWNED_NAME,
                        OVC_ONNAME = prom.OVC_ONNAME,
                        OVC_DGARRENT_START = prom.OVC_DGARRENT_START,
                        OVC_DGARRENT_END = prom.OVC_DGARRENT_END,
                        OVC_DEFFECT_1 = prom.OVC_DEFFECT_1,
                        OVC_DBACK = prom.OVC_DBACK,
                        OVC_DO_NAME = tbmrec.OVC_DO_NAME
                    };
                var query = Cash.Concat(Stock).Concat(Prom).OrderBy(o => o.OVC_PURCH);
                dtq = CommonStatic.LinqQueryToDataTable(query);
                FCommon.GridView_dataImport(GV_Q18, dtq);

                print("履驗時程管制總表", GV_Q18);
            }
        }
        #endregion
        #region Q19 多重需求
        protected void btnSix_Click(object sender, EventArgs e)
        {
            ViewState["btn"] = "Q19";
            DataTable dt = new DataTable();
            DateTime datetime;
            string strUSER_ID = Session["userid"].ToString();
            string Use_LEVEL = "";
            string StrSection = "";
            string User = "";
            string TC = int.Parse(drpSixY.SelectedValue) > 99 ? drpSixY.SelectedValue.Substring(1) : drpSixY.SelectedValue;
            string UNIT = txtOVC_DEPT_CDE.Value;
            string WAY = DropDownList11.SelectedValue;
            string KIND = TextBox14.Text;
            string CLASS = DropDownList12.SelectedValue;
            string SCHE = DropDownList13.SelectedValue;
            string SCHE_plus = (int.Parse(SCHE) + 1).ToString();
            if (SCHE == "3")
                SCHE_plus = "31";
            TBM5200_1 ac = new TBM5200_1();
            TBM5200_PPP acc = new TBM5200_PPP();
            ac = MPMS.TBM5200_1.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            var acc_auth =
                (from account in MPMS.ACCOUNTs
                 join account_auth in MPMS.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                 where account.USER_ID.Equals(strUSER_ID)
                 select new
                 {
                     C_SN_AUTH = account_auth.C_SN_AUTH,
                     DEPT_SN = account.DEPT_SN,
                     USER_NAME = acc.USER_NAME,
                 }).FirstOrDefault();
            if (ac != null && acc != null)
            {
                Use_LEVEL = ac.OVC_PRIV_LEVEL;
                StrSection = acc.OVC_PUR_SECTION;
                User = acc.USER_NAME;
            }
            else if (acc_auth != null)
            {
                Use_LEVEL = acc_auth.C_SN_AUTH;
                StrSection = acc_auth.DEPT_SN;
                User = acc_auth.USER_NAME;
            }

            var queryGN = MPMS.TBM1407
                .Where(o => o.OVC_PHR_CATE.Equals("GN"));

            var queryP9 = MPMS.TBM1407
                .Where(o => o.OVC_PHR_CATE.Equals("P9"));

            var queryPay = MPMS.TBMPAY_MONEY
                .Where(o => o.ONB_PAY_MONEY != null);

            var queryRec = MPMS.TBMRECEIVE_CONTRACT
                .Where(o => o.OVC_RECEIVE_COMM != null);

            var query1301_plan = MPMS.TBM1301_PLAN
                .Where(o => o.OVC_PURCH.Substring(2, 2).Equals(TC))
                .Where(o => o.OVC_CONTRACT_UNIT.Equals(StrSection))
                .Where(o => (!string.IsNullOrEmpty(chkOVC_PUR_NSECTION.Text) ? o.OVC_PUR_SECTION.Equals(UNIT) : true))
                .Where(o => (!string.IsNullOrEmpty(CheckBoxList2.Text) ? o.OVC_PUR_AGENCY.Equals(WAY) : true))
                .Where(o => (!string.IsNullOrEmpty(CheckBoxList1.Text) ? o.OVC_PUR_PROJE.Equals(KIND) : true));

            var queryVide = MPMS.VIDELIVERY
                .Where(v => (StrSection != "0A100" && StrSection != "00N00" && (Use_LEVEL == "7" || Use_LEVEL == "3403")) ? v.OVC_DO_NAME.Equals(User) : true)
                .Where(v => (!string.IsNullOrEmpty(CheckBoxList3.Text) ? v.OVC_LAB.Equals(CLASS) : true))
                .Join(query1301_plan, v => v.OVC_PURCH, o => o.OVC_PURCH, (v, o) => new { v = v, o = o });

            var query1118 =
                from tbm1118 in MPMS.TBM1118.AsEnumerable()
                select new
                {
                    tbm1118.OVC_PURCH,
                    tbm1118.OVC_YY,
                    tbm1118.OVC_POI_IBDG,
                    tbm1118.OVC_PJNAME
                };

            var t1118group = query1118
                .GroupBy(cc => new { cc.OVC_PURCH, })
                .Select(dd => new
                {
                    dd.Key.OVC_PURCH,
                    OVC_YY = string.Join(";", dd.Select(ee => ee.OVC_YY.AsEnumerable()).Distinct()),
                    OVC_POI_IBDG = string.Join(";", dd.Select(ee => ee.OVC_POI_IBDG.AsEnumerable()).Distinct()),
                    OVC_PJNAME = string.Join(";", dd.Select(ee => ee.OVC_PJNAME.AsEnumerable()).Distinct())
                });

            var querydel =
                from tbmdel in MPMS.TBMDELIVERY.AsEnumerable()
                select new
                {
                    OVC_PURCH = tbmdel.OVC_PURCH,
                    ONB_SHIP_TIMES = tbmdel.ONB_SHIP_TIMES,
                    OVC_DELIVERY_CONTRACT = tbmdel.OVC_DELIVERY_CONTRACT ?? " ",
                    OVC_DELIVERY = tbmdel.OVC_DELIVERY ?? " ",
                    ONB_DAYS_CONTRACT = tbmdel.ONB_DAYS_CONTRACT,
                    OVC_DJOINCHECK = tbmdel.OVC_DJOINCHECK ?? " ",
                };

            var tbmdel_g = querydel
                .GroupBy(cc => new { cc.OVC_PURCH })
                .Select(dd => new
                {
                    OVC_PURCH = dd.Key.OVC_PURCH,
                    OVC_DELIVERY_CONTRACT = string.Join(";", dd.Select(ee => ee.ONB_SHIP_TIMES + "(" + ee.OVC_DELIVERY_CONTRACT.AsEnumerable() + ")").Distinct()),
                    OVC_DELIVERY = string.Join(";", dd.Select(ee => ee.ONB_SHIP_TIMES + "(" + ee.OVC_DELIVERY.AsEnumerable() + ")").Distinct()),
                    ONB_DAYS_CONTRACT = string.Join(";", dd.Select(ee => ee.ONB_SHIP_TIMES + "(" + ee.ONB_DAYS_CONTRACT + ")").Distinct()),
                    OVC_DJOINCHECK = string.Join(";", dd.Select(ee => ee.ONB_SHIP_TIMES + "(" + ee.OVC_DJOINCHECK.AsEnumerable() + ")").Distinct()),
                });

            var query =
                from vide in queryVide.AsEnumerable()
                join GN in queryGN on vide.v.OVC_LAB equals GN.OVC_PHR_ID into GN
                from tbmGN in GN.DefaultIfEmpty().AsEnumerable()
                join P9 in queryP9 on vide.v.OVC_GRANT_TO equals P9.OVC_PHR_ID into P9
                from tbmP9 in P9.DefaultIfEmpty().AsEnumerable()
                join tbmpay in queryPay on new { vide.v.OVC_PURCH, vide.v.OVC_PURCH_6 } equals new { tbmpay.OVC_PURCH, tbmpay.OVC_PURCH_6 } into p
                from pay in p.DefaultIfEmpty().AsEnumerable()
                join tbmrec in queryRec on new { vide.v.OVC_PURCH, vide.v.OVC_PURCH_6 } equals new { tbmrec.OVC_PURCH, tbmrec.OVC_PURCH_6 } into r
                from rec in r.DefaultIfEmpty().AsEnumerable()
                join t1118 in t1118group.AsEnumerable() on vide.v.OVC_PURCH equals t1118.OVC_PURCH into t
                from tbm1118 in t.DefaultIfEmpty().AsEnumerable()
                join tdel in tbmdel_g.AsEnumerable() on vide.v.OVC_PURCH equals tdel.OVC_PURCH into d
                from tbmdel in d.DefaultIfEmpty().AsEnumerable()
                where (!string.IsNullOrEmpty(CheckBoxList4.Text) ? (from tbmsta in MPMS.TBMSTATUS
                                                                    where tbmsta.OVC_PURCH.Equals(vide.v.OVC_PURCH)
                                                                    where tbmsta.OVC_PURCH_6.Equals(vide.v.OVC_PURCH_6)
                                                                    where tbmsta.OVC_STATUS.Equals(SCHE) || tbmsta.OVC_STATUS.Equals(SCHE_plus)
                                                                    select tbmsta.OVC_PURCH).Count().Equals(1) : true)
                let sum_1 = (p.Sum(o => o.ONB_DELAY_MONEY ?? 0))
                let sum_2 = (p.Sum(o => o.ONB_MINS_MONEY_1 ?? 0)) + (p.Sum(o => o.ONB_MINS_MONEY_2 ?? 0))
                let DAPPLY = DateTime.TryParse(vide.o.OVC_DAPPLY, out datetime) ? DateTime.Parse(vide.o.OVC_DAPPLY).AddDays(double.Parse((vide.o.ONB_REVIEW_DAYS ?? 0).ToString()))
                                            .AddDays(double.Parse((vide.o.ONB_TENDER_DAYS ?? 0).ToString()))
                                            .AddDays(double.Parse((vide.o.ONB_DELIVER_DAYS ?? 0).ToString()))
                                            .AddDays(double.Parse((vide.o.ONB_RECEIVE_DAYS ?? 0).ToString())).ToString("yyyy-MM-dd") : ""
                orderby vide.v.OVC_PURCH
                select new
                {
                    OVC_PURCH = vide.v.PURCH,
                    ONB_GROUP = "0",
                    OVC_PUR_IPURCH = vide.v.OVC_PUR_IPURCH,
                    OVC_PUR_NSECTION = vide.v.OVC_PUR_NSECTION,
                    OVC_YY = tbm1118 != null ? tbm1118.OVC_YY : "",
                    OVC_POI_IBDG = tbm1118 != null ? tbm1118.OVC_POI_IBDG : "",
                    OVC_PJNAME = tbm1118 != null ? tbm1118.OVC_PJNAME : "",
                    OVC_LAB = tbmGN != null ? tbmGN.OVC_PHR_DESC : "",
                    ONB_MONEY = vide.v.ONB_MONEY ?? 0,
                    ONB_PAY_MONEY = pay != null ? pay.ONB_PAY_MONEY ?? 0 : 0,
                    OVC_DCONTRACT = vide.v.OVC_DCONTRACT,
                    OVC_DELIVERY_CONTRACT = tbmdel != null ? tbmdel.OVC_DELIVERY_CONTRACT : "",
                    OVC_DELIVERY = tbmdel != null ? tbmdel.OVC_DELIVERY : "",
                    ONB_DAYS_CONTRACT = tbmdel != null ? tbmdel.ONB_DAYS_CONTRACT : "",
                    OVC_DJOINCHECK = tbmdel != null ? tbmdel.OVC_DJOINCHECK : "",
                    OVC_DCLOSE = vide.v.OVC_DCLOSE,
                    OVC_STATUS = vide.v.OVC_STATUS,
                    OVC_DAPPLY = DAPPLY,
                    OVC_RECEIVE_COMM = rec != null ? rec.OVC_RECEIVE_COMM ?? "" : "",
                    ONB_DELAY_MONEY = pay != null ? sum_1 : 0,
                    ONB_MINS_MONEY = pay != null ? sum_2 : 0,
                    OVC_GRANT_TO = tbmP9 != null ? tbmP9.OVC_PHR_DESC : "",
                    OVC_DO_NAME = vide.v.OVC_DO_NAME,
                    OVC_VEN_TITLE = vide.v.OVC_VEN_TITLE,
                    OVC_CLOSE = vide.v.OVC_CLOSE
                };
            
            dt = CommonStatic.LinqQueryToDataTable(query);
            FCommon.GridView_dataImport(GV_Q19, dt);

            print("履驗時程管制總表", GV_Q19);
        }
        #endregion

        #region Gridview_RowCreated_And_RowDataBound
        #region Q2_GV
        protected void GV_Q2_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab = new Label();
                Label lab2 = new Label();
                lab.Text = drpStatics.Text + "年度 未結案統計表";
                //lab.Text = unit() + lab.Text;//標題單位
                lab2.Text = "印表日期：" + todayTW();
                lab.CssClass = "control-label";
                lab2.CssClass = "control-label";
                lab.Font.Size = 14;
                lab2.Font.Size = 14;
                lab.Font.Bold = true;
                lab2.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 4;
                tc2.Controls.Add(lab2);
                tc2.ColumnSpan = 4;
                gvRow.Cells.Add(tc);
                gvRow.Cells.Add(tc2);
                GV_Q2.Controls[0].Controls.Add(gvRow);
            }
        }
        protected void GV_Q2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                string purch = e.Row.Cells[1].Text;
                var queryPurch =
                    from tbm1301 in MPMS.TBM1301
                    join tbm1302 in MPMS.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                    where tbm1301.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_PURCH = tbm1301.OVC_PURCH,
                        OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                        OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                        OVC_PURCH_6 = tbm1302.OVC_PURCH_6
                    };
                foreach (var q in queryPurch)
                {
                    e.Row.Cells[1].Text = q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6;
                }
            }
        }
        #endregion
        #region Q3_GV
        protected void GV_Q3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                string purch = e.Row.Cells[1].Text;
            }
        }
        protected void GV_Q3_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab = new Label();
                Label lab2 = new Label();
                lab.Text = txt_nondelivery.Text + "日前應交貨未交貨個案統計表";
                //lab.Text = unit() + lab.Text;//標題單位
                lab2.Text = "印表日期：" + todayTW();
                lab.CssClass = "control-label";
                lab2.CssClass = "control-label";
                lab.Font.Size = 14;
                lab2.Font.Size = 14;
                lab.Font.Bold = true;
                lab2.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 2;
                tc2.Controls.Add(lab2);
                tc2.ColumnSpan = 1;
                gvRow.Cells.Add(tc);
                gvRow.Cells.Add(tc2);
                GV_Q3.Controls[0].Controls.Add(gvRow);
            }
        }
        #endregion  
        #region Q4_GV
        protected void GV_Q4_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab = new Label();
                Label lab2 = new Label();
                lab.Text = txtAfter_day.Text + "日前應結案而未結案個案統計表";
                //lab.Text = unit() + lab.Text;//標題單位
                lab2.Text = "印表日期：" + todayTW();
                lab.CssClass = "control-label";
                lab2.CssClass = "control-label";
                lab.Font.Size = 14;
                lab2.Font.Size = 14;
                lab.Font.Bold = true;
                lab2.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 2;
                tc2.Controls.Add(lab2);
                tc2.ColumnSpan = 1;
                gvRow.Cells.Add(tc);
                gvRow.Cells.Add(tc2);
                GV_Q4.Controls[0].Controls.Add(gvRow);
            }
        }
        protected void GV_Q4_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                string purch = e.Row.Cells[1].Text;
            }
        }
        #endregion
        #region Q5_GV
        protected void GV_Q5_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab = new Label();
                Label lab2 = new Label();
                lab.Text = drpexceed_standard.Text + "年度結案天數超過標準作業天數";
                //lab.Text = unit() + lab.Text;//標題單位
                lab2.Text = "印表日期：" + todayTW();
                lab.CssClass = "control-label";
                lab2.CssClass = "control-label";
                lab.Font.Size = 14;
                lab2.Font.Size = 14;
                lab.Font.Bold = true;
                lab2.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 11;
                tc2.Controls.Add(lab2);
                tc2.ColumnSpan = 2;
                gvRow.Cells.Add(tc);
                gvRow.Cells.Add(tc2);
                GV_Q5.Controls[0].Controls.Add(gvRow);
            }
        }
        protected void GV_Q5_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                string purch = e.Row.Cells[1].Text;
            }
        }
        #endregion
        #region Q6_GV
        protected void GV_Q6_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab = new Label();
                Label lab2 = new Label();
                lab.Text = drpAfter_year.Text + "年度預劃結案日未能於年度結案個案統計表";
                //lab.Text = unit() + lab.Text;//標題單位
                lab2.Text = "印表日期：" + todayTW();
                lab.CssClass = "control-label";
                lab2.CssClass = "control-label";
                lab.Font.Size = 14;
                lab2.Font.Size = 14;
                lab.Font.Bold = true;
                lab2.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 11;
                tc2.Controls.Add(lab2);
                tc2.ColumnSpan = 3;
                gvRow.Cells.Add(tc);
                gvRow.Cells.Add(tc2);
                GV_Q6.Controls[0].Controls.Add(gvRow);
            }
        }
        protected void GV_Q6_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                string purch = e.Row.Cells[1].Text;
            }
        }
        #endregion
        #region Q7_GV
        protected void GV_Q7_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab = new Label();
                Label lab2 = new Label();
                lab.Text = "預劃結案日前應於" + txtShouldhavef.Text + "結案而未結案個案統計表";
                //lab.Text = unit() + lab.Text;//標題單位
                lab2.Text = "印表日期：" + todayTW();
                lab.CssClass = "control-label";
                lab2.CssClass = "control-label";
                lab.Font.Size = 14;
                lab2.Font.Size = 14;
                lab.Font.Bold = true;
                lab2.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 7;
                tc2.Controls.Add(lab2);
                tc2.ColumnSpan = 2;
                gvRow.Cells.Add(tc);
                gvRow.Cells.Add(tc2);
                GV_Q7.Controls[0].Controls.Add(gvRow);
            }
        }
        protected void GV_Q7_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                string purch = e.Row.Cells[1].Text;
            }
        }
        #endregion
        #region Q8_GV
        protected void GV_Q8_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab = new Label();
                Label lab2 = new Label();
                lab.Text = drpWithoutwarrantymoney.Text + "年度保證金不含保固金管制明細總表";
                lab.Text = unit() + lab.Text;//標題單位
                lab2.Text = "印表日期：" + todayTW();
                lab.CssClass = "control-label";
                lab2.CssClass = "control-label";
                lab.Font.Size = 14;
                lab2.Font.Size = 14;
                lab.Font.Bold = true;
                lab2.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 9;
                tc2.Controls.Add(lab2);
                tc2.ColumnSpan = 2;
                gvRow.Cells.Add(tc);
                gvRow.Cells.Add(tc2);
                GV_Q8.Controls[0].Controls.Add(gvRow);
            }
        }
        protected void GV_Q8_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                string purch = e.Row.Cells[1].Text;
                var queryPurch =
                    from tbm1301 in MPMS.TBM1301
                    join tbm1302 in MPMS.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                    where tbm1301.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_PURCH = tbm1301.OVC_PURCH,
                        OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                        OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                        OVC_PURCH_6 = tbm1302.OVC_PURCH_6
                    };
                foreach (var q in queryPurch)
                {
                    e.Row.Cells[1].Text = q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6;
                }
                if (e.Row.Cells[9].Text.Contains("-") && int.TryParse(e.Row.Cells[9].Text.Substring(0, 4), out int n))
                {
                    int year = int.Parse(e.Row.Cells[9].Text.Substring(0, 4)) - 1911;
                    string date = year.ToString() + "年" + e.Row.Cells[9].Text.Substring(5, 2) + "月" + e.Row.Cells[9].Text.Substring(8, 2) + "日";
                    e.Row.Cells[9].Text = date;
                }
                if (double.TryParse(e.Row.Cells[3].Text, out double d))
                    e.Row.Cells[3].Text = String.Format("{0:N}", double.Parse(e.Row.Cells[3].Text));
                if (double.TryParse(e.Row.Cells[5].Text, out d))
                    e.Row.Cells[5].Text = String.Format("{0:N}", double.Parse(e.Row.Cells[5].Text));
            }
        }
        #endregion
        #region Q9_GV
        protected void GV_Q9_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab = new Label();
                Label lab2 = new Label();
                lab.Text = drpguaranteenotrefund.Text + "年度 履保金管制明細表&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                lab.Text = unit() + lab.Text;//標題單位
                lab2.Text = "印表日期：" + todayTW();
                lab.CssClass = "control-label";
                lab2.CssClass = "control-label";
                lab.Font.Size = 14;
                lab2.Font.Size = 14;
                lab.Font.Bold = true;
                lab2.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 7;
                tc2.Controls.Add(lab2);
                tc2.ColumnSpan = 7;
                gvRow.Cells.Add(tc);
                gvRow.Cells.Add(tc2);
                GV_Q9.Controls[0].Controls.Add(gvRow);
            }
        }
        protected void GV_Q9_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                string purch = e.Row.Cells[1].Text;
                var queryPurch =
                    from tbm1301 in MPMS.TBM1301
                    join tbm1302 in MPMS.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                    where tbm1301.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_PURCH = tbm1301.OVC_PURCH,
                        OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                        OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                        OVC_PURCH_6 = tbm1302.OVC_PURCH_6
                    };
                foreach (var q in queryPurch)
                {
                    e.Row.Cells[1].Text = q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6;
                }
                if (e.Row.Cells[9].Text.Contains("合約履行完成止") == true)
                    e.Row.Cells[11].Text = "合約履行完成止";
                else
                {
                    //if (e.Row.Cells[9].Text.IndexOf("日至") != -1)
                    //    e.Row.Cells[11].Text = e.Row.Cells[9].Text.Substring(e.Row.Cells[9].Text.IndexOf("日至") + 2, e.Row.Cells[9].Text.IndexOf("日止") - (e.Row.Cells[9].Text.IndexOf("日至") + 1));
                    if (e.Row.Cells[9].Text.IndexOf("日至") != -1 && e.Row.Cells[9].Text.IndexOf("日止") != -1)
                        e.Row.Cells[11].Text = e.Row.Cells[9].Text.Substring(e.Row.Cells[9].Text.IndexOf("日至") + 2, e.Row.Cells[9].Text.IndexOf("日止") - (e.Row.Cells[9].Text.IndexOf("日至") + 1));
                    if (e.Row.Cells[9].Text.IndexOf("日至") != -1 && e.Row.Cells[9].Text.IndexOf("日完") != -1)
                        e.Row.Cells[11].Text = e.Row.Cells[9].Text.Substring(e.Row.Cells[9].Text.IndexOf("日至") + 2, e.Row.Cells[9].Text.IndexOf("日完") - (e.Row.Cells[9].Text.IndexOf("日至") + 1));
                }
                if (e.Row.Cells[9].Text.IndexOf("日至") != -1)
                    e.Row.Cells[9].Text = e.Row.Cells[9].Text.Substring(e.Row.Cells[9].Text.IndexOf("日至") - 9, 10);
                if (e.Row.Cells[9].Text.Contains("自"))
                    e.Row.Cells[9].Text = e.Row.Cells[9].Text.Substring(e.Row.Cells[9].Text.IndexOf("自") + 1, e.Row.Cells[9].Text.IndexOf("日") - e.Row.Cells[9].Text.IndexOf("自"));
                if (e.Row.Cells[10].Text.Contains("-") && int.TryParse(e.Row.Cells[10].Text.Substring(0, 4), out int n))
                {
                    int year = int.Parse(e.Row.Cells[10].Text.Substring(0, 4)) - 1911;
                    string date = year.ToString() + "年" + e.Row.Cells[10].Text.Substring(5, 2) + "月" + e.Row.Cells[10].Text.Substring(8, 2) + "日";
                    e.Row.Cells[10].Text = date;
                }
                if (double.TryParse(e.Row.Cells[5].Text, out double d))
                    e.Row.Cells[5].Text = String.Format("{0:N}", double.Parse(e.Row.Cells[5].Text));
            }
        }
        #endregion
        #region Q10_GV
        protected void GV_Q10_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab = new Label();
                Label lab2 = new Label();
                lab.Text = drpWarrantyGold.Text + "年度保固金管制明細總表";
                lab.Text = unit() + lab.Text;//標題單位
                lab2.Text = "印表日期：" + todayTW();
                lab.CssClass = "control-label";
                lab2.CssClass = "control-label";
                lab.Font.Size = 14;
                lab2.Font.Size = 14;
                lab.Font.Bold = true;
                lab2.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 9;
                tc2.Controls.Add(lab2);
                tc2.ColumnSpan = 4;
                gvRow.Cells.Add(tc);
                gvRow.Cells.Add(tc2);
                GV_Q10.Controls[0].Controls.Add(gvRow);
            }
        }
        protected void GV_Q10_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                string purch = e.Row.Cells[1].Text;
                var queryPurch =
                    from tbm1301 in MPMS.TBM1301
                    join tbm1302 in MPMS.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                    where tbm1301.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_PURCH = tbm1301.OVC_PURCH,
                        OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                        OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                        OVC_PURCH_6 = tbm1302.OVC_PURCH_6
                    };
                foreach (var q in queryPurch)
                {
                    e.Row.Cells[1].Text = q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6;
                }
                if (e.Row.Cells[9].Text.Contains("合約履行完成止") == true)
                    e.Row.Cells[9].Text = "合約履行完成止";
                else
                {
                    //if (e.Row.Cells[9].Text.IndexOf("日至") != -1)
                    //    e.Row.Cells[9].Text = e.Row.Cells[9].Text.Substring(e.Row.Cells[9].Text.IndexOf("日至") + 2, e.Row.Cells[9].Text.IndexOf("日止") - (e.Row.Cells[9].Text.IndexOf("日至") + 1));
                    if (e.Row.Cells[9].Text.IndexOf("日至") != -1 && e.Row.Cells[9].Text.IndexOf("日止") != -1)
                        e.Row.Cells[9].Text = e.Row.Cells[9].Text.Substring(e.Row.Cells[9].Text.IndexOf("日至") + 2, e.Row.Cells[9].Text.IndexOf("日止") - (e.Row.Cells[9].Text.IndexOf("日至") + 1));
                    if (e.Row.Cells[9].Text.IndexOf("日至") != -1 && e.Row.Cells[9].Text.IndexOf("日完") != -1)
                        e.Row.Cells[9].Text = e.Row.Cells[9].Text.Substring(e.Row.Cells[9].Text.IndexOf("日至") + 2, e.Row.Cells[9].Text.IndexOf("日完") - (e.Row.Cells[9].Text.IndexOf("日至") + 1));
                }
                if (e.Row.Cells[10].Text.Contains("-") && int.TryParse(e.Row.Cells[10].Text.Substring(0, 4), out int n))
                {
                    int year = int.Parse(e.Row.Cells[10].Text.Substring(0, 4)) - 1911;
                    string date = year.ToString() + "年" + e.Row.Cells[10].Text.Substring(5, 2) + "月" + e.Row.Cells[10].Text.Substring(8, 2) + "日";
                    e.Row.Cells[10].Text = date;
                }
                if (e.Row.Cells[11].Text.Contains("-") && int.TryParse(e.Row.Cells[11].Text.Substring(0, 4), out n))
                {
                    int year = int.Parse(e.Row.Cells[11].Text.Substring(0, 4)) - 1911;
                    string date = year.ToString() + "年" + e.Row.Cells[11].Text.Substring(5, 2) + "月" + e.Row.Cells[11].Text.Substring(8, 2) + "日";
                    e.Row.Cells[11].Text = date;
                }
                if (int.TryParse(e.Row.Cells[3].Text, out n))
                    e.Row.Cells[3].Text = String.Format("{0:N}", int.Parse(e.Row.Cells[3].Text));
                if (int.TryParse(e.Row.Cells[5].Text, out n))
                    e.Row.Cells[5].Text = String.Format("{0:N}", int.Parse(e.Row.Cells[5].Text));
            }
        }
        #endregion
        #region Q11_GV
        protected void GV_Q11_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab = new Label();
                Label lab2 = new Label();
                lab.Text = drpWarrantyGoldnotRe.Text + "年度 保固金管制明細表";
                lab.Text = unit() + lab.Text;//標題單位
                lab2.Text = "印表日期：" + todayTW();
                lab.CssClass = "control-label";
                lab2.CssClass = "control-label";
                lab.Font.Size = 14;
                lab2.Font.Size = 14;
                lab.Font.Bold = true;
                lab2.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 7;
                tc2.Controls.Add(lab2);
                tc2.ColumnSpan = 7;
                gvRow.Cells.Add(tc);
                gvRow.Cells.Add(tc2);
                GV_Q11.Controls[0].Controls.Add(gvRow);
            }
        }
        protected void GV_Q11_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                string purch = e.Row.Cells[1].Text;
                var queryPurch =
                    from tbm1301 in MPMS.TBM1301
                    join tbm1302 in MPMS.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                    where tbm1301.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_PURCH = tbm1301.OVC_PURCH,
                        OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                        OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                        OVC_PURCH_6 = tbm1302.OVC_PURCH_6
                    };
                foreach (var q in queryPurch)
                {
                    e.Row.Cells[1].Text = q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6;
                }
                if (e.Row.Cells[9].Text.Contains("合約履行完成止") == true)
                    e.Row.Cells[11].Text = "合約履行完成止";
                else
                {
                    if (e.Row.Cells[9].Text.IndexOf("日至") != -1 && e.Row.Cells[9].Text.IndexOf("日止") != -1)
                        e.Row.Cells[11].Text = e.Row.Cells[9].Text.Substring(e.Row.Cells[9].Text.IndexOf("日至") + 2, e.Row.Cells[9].Text.IndexOf("日止") - (e.Row.Cells[9].Text.IndexOf("日至") + 1));
                    if (e.Row.Cells[9].Text.IndexOf("日至") != -1 && e.Row.Cells[9].Text.IndexOf("日完") != -1)
                        e.Row.Cells[11].Text = e.Row.Cells[9].Text.Substring(e.Row.Cells[9].Text.IndexOf("日至") + 2, e.Row.Cells[9].Text.IndexOf("日完") - (e.Row.Cells[9].Text.IndexOf("日至") + 1));
                }
                if (e.Row.Cells[9].Text.IndexOf("日至") != -1)
                    e.Row.Cells[9].Text = e.Row.Cells[9].Text.Substring(e.Row.Cells[9].Text.IndexOf("日至") - 9, 10);
                if (e.Row.Cells[9].Text.Contains("自"))
                    e.Row.Cells[9].Text = e.Row.Cells[9].Text.Substring(e.Row.Cells[9].Text.IndexOf("自") + 1, e.Row.Cells[9].Text.IndexOf("日") - e.Row.Cells[9].Text.IndexOf("自"));
                if (e.Row.Cells[10].Text.Contains("-") && int.TryParse(e.Row.Cells[10].Text.Substring(0, 4), out int n))
                {
                    int year = int.Parse(e.Row.Cells[10].Text.Substring(0, 4)) - 1911;
                    string date = year.ToString() + "年" + e.Row.Cells[10].Text.Substring(5, 2) + "月" + e.Row.Cells[10].Text.Substring(8, 2) + "日";
                    e.Row.Cells[10].Text = date;
                }

                if (int.TryParse(e.Row.Cells[5].Text, out n))
                    e.Row.Cells[5].Text = String.Format("{0:N}", int.Parse(e.Row.Cells[5].Text));
            }
        }
        #endregion
        #region Q12_GV
        protected void GV_Q12_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab = new Label();
                Label lab2 = new Label();
                lab.Text = txtRePurD1.Text + "至" + txtRePurD2.Text + "收辦購案統計表";
                //lab.Text = unit() + lab.Text;//標題單位
                lab2.Text = "印表日期：" + todayTW();
                lab.CssClass = "control-label";
                lab2.CssClass = "control-label";
                lab.Font.Size = 14;
                lab2.Font.Size = 14;
                lab.Font.Bold = true;
                lab2.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 25;
                tc2.Controls.Add(lab2);
                tc2.ColumnSpan = 2;
                gvRow.Cells.Add(tc);
                gvRow.Cells.Add(tc2);
                GV_Q12.Controls[0].Controls.Add(gvRow);
            }
        }
        protected void GV_Q12_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                string purch = e.Row.Cells[1].Text;
                string purch_6 = "";
                var queryPurch =
                    from tbm1301 in MPMS.TBM1301
                    join tbm1302 in MPMS.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                    where tbm1301.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_PURCH = tbm1301.OVC_PURCH,
                        OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                        OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                        OVC_PURCH_6 = tbm1302.OVC_PURCH_6
                    };
                foreach (var q in queryPurch)
                {
                    e.Row.Cells[1].Text = q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6;
                    purch_6 = q.OVC_PURCH_6;
                }
                if (int.TryParse(e.Row.Cells[4].Text, out int n))
                    e.Row.Cells[4].Text = (int.Parse(e.Row.Cells[4].Text) % 10).ToString();
                if (double.TryParse(e.Row.Cells[7].Text, out double d))
                    e.Row.Cells[7].Text = String.Format("{0:N}", double.Parse(e.Row.Cells[7].Text));
                if (double.TryParse(e.Row.Cells[8].Text, out d))
                    e.Row.Cells[8].Text = String.Format("{0:N}", double.Parse(e.Row.Cells[8].Text));
                var querydel =
                    from tbmdel in MPMS.TBMDELIVERY
                    where tbmdel.OVC_PURCH.Equals(purch)
                    where tbmdel.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        OVC_DPAY = tbmdel.OVC_DPAY,
                        OVC_DELIVERY = tbmdel.OVC_DELIVERY
                    };
                foreach (var q in querydel)
                {
                    if (q.OVC_DPAY != null && q.OVC_DPAY != null && DateTime.TryParse(q.OVC_DELIVERY, out DateTime dt))
                        e.Row.Cells[17].Text = (DateTime.Parse(q.OVC_DELIVERY) - DateTime.Parse(q.OVC_DELIVERY)).Days.ToString();//不確定
                }
            }
        }
        #endregion
        #region Q13_GV
        protected void GV_Q13_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab = new Label();
                Label lab2 = new Label();
                lab.Text = txtClosingD1.Text + "至" + txtClosingD2.Text + "結案購案統計表";
                //lab.Text = unit() + lab.Text;//標題單位
                lab2.Text = "印表日期：" + todayTW();
                lab.CssClass = "control-label";
                lab2.CssClass = "control-label";
                lab.Font.Size = 14;
                lab2.Font.Size = 14;
                lab.Font.Bold = true;
                lab2.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 25;
                tc2.Controls.Add(lab2);
                tc2.ColumnSpan = 2;
                gvRow.Cells.Add(tc);
                gvRow.Cells.Add(tc2);
                GV_Q13.Controls[0].Controls.Add(gvRow);
            }
        }
        protected void GV_Q13_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                string purch = e.Row.Cells[1].Text;
            }
        }
        #endregion
        #region Q14_GV
        protected void GV_Q14_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab = new Label();
                Label lab2 = new Label();
                lab.Text = txtDJOINCHECK.Text + "至" + txtDJOINCHECK2.Text + "會驗購案統計表";
                //lab.Text = unit() + lab.Text;//標題單位
                lab2.Text = "印表日期：" + todayTW();
                lab.CssClass = "control-label";
                lab2.CssClass = "control-label";
                lab.Font.Size = 14;
                lab2.Font.Size = 14;
                lab.Font.Bold = true;
                lab2.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 25;
                tc2.Controls.Add(lab2);
                tc2.ColumnSpan = 2;
                gvRow.Cells.Add(tc);
                gvRow.Cells.Add(tc2);
                GV_Q14.Controls[0].Controls.Add(gvRow);
            }
        }
        protected void GV_Q14_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                string purch = e.Row.Cells[1].Text;
                string purch_6 = "";
                var queryPurch =
                    from tbm1301 in MPMS.TBM1301
                    join tbm1302 in MPMS.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                    where tbm1301.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_PURCH = tbm1301.OVC_PURCH,
                        OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                        OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                        OVC_PURCH_6 = tbm1302.OVC_PURCH_6
                    };
                foreach (var q in queryPurch)
                {
                    e.Row.Cells[1].Text = q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6;
                    purch_6 = q.OVC_PURCH_6;
                }
                if (int.TryParse(e.Row.Cells[4].Text, out int n))
                    e.Row.Cells[4].Text = (int.Parse(e.Row.Cells[4].Text) % 10).ToString();
                if (double.TryParse(e.Row.Cells[7].Text, out double d))
                e.Row.Cells[7].Text = String.Format("{0:N}", double.Parse(e.Row.Cells[7].Text));
                if (double.TryParse(e.Row.Cells[8].Text, out d))
                    e.Row.Cells[8].Text = String.Format("{0:N}", double.Parse(e.Row.Cells[8].Text));
                var querydel =
                    from tbmdel in MPMS.TBMDELIVERY
                    where tbmdel.OVC_PURCH.Equals(purch)
                    where tbmdel.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        OVC_DPAY = tbmdel.OVC_DPAY,
                        OVC_DELIVERY = tbmdel.OVC_DELIVERY
                    };
                foreach (var q in querydel)
                {
                    if (q.OVC_DPAY != null && q.OVC_DPAY != null && DateTime.TryParse(q.OVC_DELIVERY, out DateTime dt))
                        e.Row.Cells[17].Text = (DateTime.Parse(q.OVC_DELIVERY) - DateTime.Parse(q.OVC_DELIVERY)).Days.ToString();//不確定
                }
            }
        }
        #endregion
        #region Q15_GV
        protected void GV_Q15_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab = new Label();
                Label lab2 = new Label();
                lab.Text = txtPenaltyfuse.Text + "至" + txtPenaltyfuse2.Text + "購案支用逾期罰款總表";
                //lab.Text = unit() + lab.Text;//標題單位
                lab2.Text = "印表日期：" + todayTW();
                lab.CssClass = "control-label";
                lab2.CssClass = "control-label";
                lab.Font.Size = 14;
                lab2.Font.Size = 14;
                lab.Font.Bold = true;
                lab2.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 10;
                tc2.Controls.Add(lab2);
                tc2.ColumnSpan = 2;
                gvRow.Cells.Add(tc);
                gvRow.Cells.Add(tc2);
                GV_Q15.Controls[0].Controls.Add(gvRow);
            }
        }
        protected void GV_Q15_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                string purch = e.Row.Cells[1].Text;
            }
        }
        #endregion
        #region Q16_GV
        protected void GV_Q16_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab = new Label();
                Label lab2 = new Label();
                lab.Text = drpAnnualBatchC.Text + "年度 履驗分年分批時程管制 總表";
                //lab.Text = unit() + lab.Text;//標題單位
                lab2.Text = "印表日期：" + todayTW();
                lab.CssClass = "control-label";
                lab2.CssClass = "control-label";
                lab.Font.Size = 14;
                lab2.Font.Size = 14;
                lab.Font.Bold = true;
                lab2.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 25;
                tc2.Controls.Add(lab2);
                tc2.ColumnSpan = 2;
                gvRow.Cells.Add(tc);
                gvRow.Cells.Add(tc2);
                GV_Q16.Controls[0].Controls.Add(gvRow);
            }
        }
        protected void GV_Q16_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                string purch = e.Row.Cells[1].Text;
                string purch_6 = "";
                var queryPurch =
                    from tbm1301 in MPMS.TBM1301
                    join tbm1302 in MPMS.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                    where tbm1301.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_PURCH = tbm1301.OVC_PURCH,
                        OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                        OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                        OVC_PURCH_6 = tbm1302.OVC_PURCH_6
                    };
                foreach (var q in queryPurch)
                {
                    e.Row.Cells[1].Text = q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6;
                    purch_6 = q.OVC_PURCH_6;
                }
                if (int.TryParse(e.Row.Cells[4].Text, out int nn))
                    e.Row.Cells[4].Text = (int.Parse(e.Row.Cells[4].Text) % 10).ToString();
                var query1301 = MPMS.TBM1301_PLAN
                    .Where(table => purch.Contains(table.OVC_PURCH)).FirstOrDefault();
                if (query1301 != null && DateTime.TryParse(e.Row.Cells[58].Text, out DateTime d))
                {
                    DateTime dt = DateTime.Parse(e.Row.Cells[58].Text);
                    e.Row.Cells[58].Text =
                        dt.AddDays(double.Parse((query1301.ONB_REVIEW_DAYS ?? 0).ToString()))
                        .AddDays(double.Parse((query1301.ONB_TENDER_DAYS ?? 0).ToString()))
                        .AddDays(double.Parse((query1301.ONB_DELIVER_DAYS ?? 0).ToString()))
                        .AddDays(double.Parse((query1301.ONB_RECEIVE_DAYS ?? 0).ToString())).ToString("yyyy-MM-dd");
                }
                var querydel =
                    from tbmdel in MPMS.TBMDELIVERY
                    where tbmdel.OVC_PURCH.Equals(purch)
                    where tbmdel.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        ONB_SHIP_TIMES = tbmdel.ONB_SHIP_TIMES,
                        OVC_DELIVERY_CONTRACT = tbmdel.OVC_DELIVERY_CONTRACT,
                        OVC_DELIVERY = tbmdel.OVC_DELIVERY,
                        OVC_DJOINCHECK = tbmdel.OVC_DJOINCHECK,
                        OVC_DPAY = tbmdel.OVC_DPAY
                    };
                var querypay =
                    from tbmpay in MPMS.TBMPAY_MONEY
                    where tbmpay.OVC_PURCH.Equals(purch)
                    where tbmpay.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        ONB_TIMES = tbmpay.ONB_TIMES,
                        ONB_PAY_MONEY = tbmpay.ONB_PAY_MONEY
                    };

                for (int i = 2; i < 9; i++)
                {
                    int n = 21;
                    switch (i)
                    {
                        case 2:
                            n = 21;
                            break;
                        case 3:
                            n = 25;
                            break;
                        case 4:
                            n = 29;
                            break;
                        case 5:
                            n = 33;
                            break;
                        case 6:
                            n = 37;
                            break;
                        case 7:
                            n = 41;
                            break;
                        case 8:
                            n = 45;
                            break;
                    }
                    foreach (var q in querydel)
                    {
                        if (q.ONB_SHIP_TIMES == i)
                        {
                            e.Row.Cells[i + n].Text = q.OVC_DELIVERY_CONTRACT ?? "";
                            e.Row.Cells[i + n + 1].Text = q.OVC_DELIVERY ?? "";
                            e.Row.Cells[i + n + 2].Text = q.OVC_DJOINCHECK ?? "";
                            e.Row.Cells[i + n + 4].Text = q.OVC_DPAY ?? "";
                        }
                    }
                    e.Row.Cells[i + n + 3].Text = "0";
                    foreach (var q in querypay)
                    {
                        if (q.ONB_TIMES == i)
                        {
                            e.Row.Cells[i + n + 3].Text = q.ONB_PAY_MONEY.ToString() ?? "0";
                        }
                    }
                }
            }
        }
        #endregion
        #region Q17_GV
        protected void GV_Q17_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab = new Label();
                Label lab2 = new Label();
                lab.Text = drpNSecuritydeposit.Text + "年度保固金管制明細總表";
                lab.Text = unit() + lab.Text;//標題單位
                lab2.Text = "印表日期：" + todayTW();
                lab.CssClass = "control-label";
                lab2.CssClass = "control-label";
                lab.Font.Size = 14;
                lab2.Font.Size = 14;
                lab.Font.Bold = true;
                lab2.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 7;
                tc2.Controls.Add(lab2);
                tc2.ColumnSpan = 7;
                gvRow.Cells.Add(tc);
                gvRow.Cells.Add(tc2);
                GV_Q17.Controls[0].Controls.Add(gvRow);
            }
        }
        protected void GV_Q17_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                string purch = e.Row.Cells[1].Text;
                var queryPurch =
                        from tbm1301 in MPMS.TBM1301
                        join tbm1302 in MPMS.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                        where tbm1301.OVC_PURCH.Equals(purch)
                        select new
                        {
                            OVC_PURCH = tbm1301.OVC_PURCH,
                            OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                            OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                            OVC_PURCH_6 = tbm1302.OVC_PURCH_6
                        };
                foreach (var q in queryPurch)
                {
                    e.Row.Cells[1].Text = q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6;
                }
                if (double.TryParse(e.Row.Cells[4].Text, out double n))
                    e.Row.Cells[4].Text = String.Format("{0:N}", double.Parse(e.Row.Cells[4].Text));
                if (double.TryParse(e.Row.Cells[8].Text, out n))
                    e.Row.Cells[8].Text = String.Format("{0:N}", double.Parse(e.Row.Cells[8].Text));
                if (e.Row.Cells[5].Text == "1")
                    e.Row.Cells[5].Text = "履約保證";
                else
                    e.Row.Cells[5].Text = "保固保證";
                string cur = e.Row.Cells[9].Text;
                var queryCur = MPMS.TBM1407
                    .Where(o => o.OVC_PHR_CATE.Equals("B0"))
                    .Where(o => o.OVC_PHR_ID.Equals(cur)).FirstOrDefault();
                if (queryCur != null)
                    e.Row.Cells[9].Text = queryCur.OVC_PHR_DESC;
                if (e.Row.Cells[12].Text.Contains("合約履行完成止") == true)
                    e.Row.Cells[13].Text = "合約履行完成止";
                else
                {
                    //if (e.Row.Cells[12].Text.IndexOf("日至") != -1)
                    //    e.Row.Cells[13].Text = e.Row.Cells[12].Text.Substring(e.Row.Cells[12].Text.IndexOf("日至") + 2, e.Row.Cells[12].Text.IndexOf("日止") - (e.Row.Cells[12].Text.IndexOf("日至") + 1));
                    if (e.Row.Cells[12].Text.IndexOf("日至") != -1 && e.Row.Cells[12].Text.IndexOf("日止") != -1)
                        e.Row.Cells[13].Text = e.Row.Cells[12].Text.Substring(e.Row.Cells[12].Text.IndexOf("日至") + 2, e.Row.Cells[12].Text.IndexOf("日止") - (e.Row.Cells[12].Text.IndexOf("日至") + 1));
                    if (e.Row.Cells[12].Text.IndexOf("日至") != -1 && e.Row.Cells[12].Text.IndexOf("日完") != -1)
                        e.Row.Cells[13].Text = e.Row.Cells[12].Text.Substring(e.Row.Cells[12].Text.IndexOf("日至") + 2, e.Row.Cells[12].Text.IndexOf("日完") - (e.Row.Cells[12].Text.IndexOf("日至") + 1));
                }
                if (e.Row.Cells[12].Text.IndexOf("日至") != -1)
                    e.Row.Cells[12].Text = e.Row.Cells[12].Text.Substring(e.Row.Cells[12].Text.IndexOf("日至") - 9, 10);
                if (e.Row.Cells[12].Text.Contains("自"))
                    e.Row.Cells[12].Text = e.Row.Cells[12].Text.Substring(e.Row.Cells[12].Text.IndexOf("自") + 1, e.Row.Cells[12].Text.IndexOf("日") - e.Row.Cells[12].Text.IndexOf("自"));
                if (e.Row.Cells[14].Text.Contains("-") && int.TryParse(e.Row.Cells[14].Text.Substring(0, 4), out int nn))
                {
                    int year = int.Parse(e.Row.Cells[14].Text.Substring(0, 4)) - 1911;
                    string date = year.ToString() + "年" + e.Row.Cells[14].Text.Substring(5, 2) + "月" + e.Row.Cells[14].Text.Substring(8, 2) + "日";
                    e.Row.Cells[14].Text = date;
                }
                if (e.Row.Cells[15].Text.Contains("-") && int.TryParse(e.Row.Cells[15].Text.Substring(0, 4), out nn))
                {
                    int year = int.Parse(e.Row.Cells[15].Text.Substring(0, 4)) - 1911;
                    string date = year.ToString() + "年" + e.Row.Cells[15].Text.Substring(5, 2) + "月" + e.Row.Cells[15].Text.Substring(8, 2) + "日";
                    e.Row.Cells[15].Text = date;
                }
            }
        }
        #endregion
        #region Q18_GV
        protected void GV_Q18_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab = new Label();
                Label lab2 = new Label();
                lab.Text = txtMarginExpiration.Text + "至" + txtMarginExpiration2.Text + "保證金到期管制明細表";
                //lab.Text = unit() + lab.Text;//標題單位
                lab2.Text = "印表日期：" + todayTW();
                lab.CssClass = "control-label";
                lab2.CssClass = "control-label";
                lab.Font.Size = 14;
                lab2.Font.Size = 14;
                lab.Font.Bold = true;
                lab2.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 14;
                tc2.Controls.Add(lab2);
                tc2.ColumnSpan = 3;
                gvRow.Cells.Add(tc);
                gvRow.Cells.Add(tc2);
                GV_Q18.Controls[0].Controls.Add(gvRow);
            }
        }
        protected void GV_Q18_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string purch_6 = "";
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                string purch = e.Row.Cells[1].Text;
                var queryPurch =
                        from tbm1301 in MPMS.TBM1301
                        join tbm1302 in MPMS.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                        where tbm1301.OVC_PURCH.Equals(purch)
                        select new
                        {
                            OVC_PURCH = tbm1301.OVC_PURCH,
                            OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                            OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                            OVC_PURCH_6 = tbm1302.OVC_PURCH_6
                        };
                foreach (var q in queryPurch)
                {
                    e.Row.Cells[1].Text = q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6;
                    purch_6 = q.OVC_PURCH_6;
                }
                if (e.Row.Cells[5].Text == "1")
                    e.Row.Cells[5].Text = "履約保證";
                else
                    e.Row.Cells[5].Text = "保固保證";
                if (double.TryParse(e.Row.Cells[4].Text, out double n))
                    e.Row.Cells[4].Text = String.Format("{0:N}", double.Parse(e.Row.Cells[4].Text));
                if (double.TryParse(e.Row.Cells[8].Text, out n))
                    e.Row.Cells[8].Text = String.Format("{0:N}", double.Parse(e.Row.Cells[8].Text));
                string cur = e.Row.Cells[9].Text;
                var queryCur = MPMS.TBM1407
                    .Where(o => o.OVC_PHR_CATE.Equals("B0"))
                    .Where(o => o.OVC_PHR_ID.Equals(cur)).FirstOrDefault();
                if (queryCur != null)
                    e.Row.Cells[9].Text = queryCur.OVC_PHR_DESC;
                e.Row.Cells[12].Text = DateCh(e.Row.Cells[12].Text);
                e.Row.Cells[13].Text = DateCh(e.Row.Cells[13].Text);
                e.Row.Cells[14].Text = DateCh(e.Row.Cells[14].Text);
                e.Row.Cells[15].Text = DateCh(e.Row.Cells[15].Text);
            }
        }
        #endregion
        #region Q1_Q19_GV
        protected void GV_Q19_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab = new Label();
                Label lab2 = new Label();
                if (ViewState["btn"].ToString() == "Q1")
                    lab.Text = drpTimeControl.SelectedValue + "年度 履驗時程管制總表";
                else
                    lab.Text = drpSixY.SelectedValue + "年度 履驗時程管制總表";
                //lab.Text = unit() + lab.Text;//標題單位
                lab2.Text = "印表日期：" + todayTW();
                lab.CssClass = "control-label";
                lab2.CssClass = "control-label";
                lab.Font.Size = 14;
                lab2.Font.Size = 14;
                lab.Font.Bold = true;
                lab2.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 22;
                tc2.Controls.Add(lab2);
                tc2.ColumnSpan = 4;
                gvRow.Cells.Add(tc);
                gvRow.Cells.Add(tc2);
                GV_Q19.Controls[0].Controls.Add(gvRow);
            }
        }
        protected void GV_Q19_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                string purch = e.Row.Cells[1].Text;
                string str = e.Row.Cells[8].Text;
                e.Row.Cells[14].Text = e.Row.Cells[14].Text.Replace("(0)", "( )").Replace("()", "( )");
            }
        }
        #endregion
        #endregion

        public override void VerifyRenderingInServerForm(Control control)
        {
            //'XX'型別 必須置於有 runat=server 的表單標記之中
        }

        #region 日期轉換
        string DateCh(string str)
        {
            if (str.Contains("-") || str.Contains("/"))
            {
                string d = str.Replace("-", "").Replace("/", "");
                int year = int.Parse(str.Substring(0, 4)) - 1911;
                string date = year.ToString() + "年" + d.Substring(4, 2) + "月" + d.Substring(6, 2) + "日";
                return date;
            }
            return str;
        }

        private string todayTW()
        {
            string year = (int.Parse(DateTime.Now.Year.ToString()) - 1911).ToString();
            string str = year + "年" + DateTime.Now.ToString("MM月dd日");
            return str;
        }
        #endregion

        #region 列印
        void print(string filename, GridView gv)
        {
            string filepath = filename;
            string fileName = HttpUtility.UrlEncode(filepath);
            StringWriter tw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(tw);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ".xls");
            HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>");

            //先把分頁關掉
            gv.AllowPaging = false;
            gv.DataBind();

            //Get the HTML for the control.
            gv.RenderControl(hw);
            HttpContext.Current.Response.Write(tw.ToString());
            HttpContext.Current.Response.End();

            gv.AllowPaging = true;
            gv.DataBind();
        }
        #endregion

        private string unit()
        {
            string strUSER_ID = Session["userid"].ToString();
            var acc = MPMS.TBM5200_PPP.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
            if (acc != null)
            {
                if (acc.OVC_PUR_SECTION != null)
                {
                    var dept = MPMS.TBMDEPTs.Where(o => o.OVC_DEPT_CDE.Equals(acc.OVC_PUR_SECTION)).FirstOrDefault();
                    if (dept != null)
                        return dept.OVC_ONNAME.Replace("軍備局採購中心", "國防採購室");
                    else
                        return "";
                }
                else
                    return "";
            }
            else
                return "";
        }
    }
}