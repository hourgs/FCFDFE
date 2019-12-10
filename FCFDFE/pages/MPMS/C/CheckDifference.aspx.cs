using System;
using System.Linq;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Entity.GMModel;
using System.Data;
using FCFDFE.Content;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace FCFDFE.pages.MPMS.C
{
    public partial class CheckDifference : System.Web.UI.Page
    {
        MPMSEntities mpms = new MPMSEntities();
        GMEntities gm = new GMEntities();
        Common FCommon = new Common();
        string strPurNum = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["OVC_PURCH"]))
            {
                strPurNum = Request.QueryString["OVC_PURCH"];
                
                Import_table1301();
                Import_table1220();
                Import_table1231();
                Import_table1118();
                Imoprt_table1119();
                Import_table1201();
            }
        }

     
        private void Imoprt_table1119()
        {
            var query1119 =
                from t in mpms.TBM1119
                where t.OVC_PURCH.Equals(strPurNum)
                select new
                {
                    t.OVC_IKIND,
                    t.OVC_ATTACH_NAME,
                    OVC_FILE_NAME = t.OVC_FILE_NAME ?? " ",
                    t.ONB_QTY,
                    t.ONB_PAGES
                };

            var query1119OLDMAX =
              (from t in mpms.TBM1119_HISTORY
               where t.OVC_PURCH.Equals(strPurNum)
               group t by t.OVC_VERSION into g
               orderby g.Key descending
               select g).Take(1);

            DataTable dtDiff = new DataTable();
            DataTable dtNew = new DataTable();
            DataTable dtDel = new DataTable();

            foreach (var item in query1119OLDMAX)
            {
                var query1119_OLD =
                from t in item
                select new
                {
                    t.OVC_IKIND,
                    t.OVC_ATTACH_NAME,
                    OVC_FILE_NAME = t.OVC_FILE_NAME ?? " ",
                    t.ONB_QTY,
                    t.ONB_PAGES
                };
                //異動的項目
                var querydiff =
                    from t in query1119_OLD
                    join t2 in query1119 on new { t.OVC_IKIND, t.OVC_ATTACH_NAME } equals new { t2.OVC_IKIND, t2.OVC_ATTACH_NAME }
                    where   !t.OVC_FILE_NAME.Equals(t2.OVC_FILE_NAME) ||
                            (t.ONB_QTY != t2.ONB_QTY) ||
                            (t.ONB_PAGES != t2.ONB_PAGES)
                    select new
                    {
                        OVC_IKIND_OLD = t.OVC_IKIND,
                        OVC_ATTACH_NAME_OLD = t.OVC_ATTACH_NAME,
                        OVC_FILE_NAME_OLD = t.OVC_FILE_NAME,
                        ONB_QTY_OLD = t.ONB_QTY,
                        ONB_PAGES_OLD = t.ONB_PAGES,
                        OVC_IKIND_NEW = t2.OVC_IKIND,
                        OVC_ATTACH_NAME_NEW = t2.OVC_ATTACH_NAME,
                        OVC_FILE_NAME_NEW = t2.OVC_FILE_NAME,
                        ONB_QTY_NEW = t2.ONB_QTY,
                        ONB_PAGES_NEW = t2.ONB_PAGES
                    };
                dtDiff = CommonStatic.LinqQueryToDataTable(querydiff);
                //刪除的項目
                var queryDel =
                     (from tOld in query1119_OLD
                      join tNew in query1119 on new { tOld.OVC_IKIND, tOld.OVC_ATTACH_NAME } equals new { tNew.OVC_IKIND, tNew.OVC_ATTACH_NAME } into ps
                      from tNew in ps.DefaultIfEmpty()
                      select new
                      {
                          OVC_IKIND_OLD = tOld.OVC_IKIND,
                          OVC_ATTACH_NAME_OLD = tOld.OVC_ATTACH_NAME,
                          OVC_FILE_NAME_OLD = tOld.OVC_FILE_NAME,
                          ONB_QTY_OLD = tOld.ONB_QTY,
                          ONB_PAGES_OLD = tOld.ONB_PAGES,
                          OVC_IKIND_NEW = tNew?.OVC_IKIND ?? "",
                          OVC_ATTACH_NAME_NEW = tNew?.OVC_ATTACH_NAME ?? "",
                          OVC_FILE_NAME_NEW = tNew?.OVC_FILE_NAME ?? "",
                          ONB_QTY_NEW = tNew?.ONB_QTY ?? null,
                          ONB_PAGES_NEW = tNew?.ONB_PAGES ?? null,
                      }).Where(o => o.OVC_IKIND_NEW == "");
                dtDel = CommonStatic.LinqQueryToDataTable(queryDel);

                var queryInsert =
                    (from tNew in query1119.ToList()
                     join tOld in query1119_OLD on new { tNew.OVC_IKIND, tNew.OVC_ATTACH_NAME } equals new { tOld.OVC_IKIND, tOld.OVC_ATTACH_NAME } into ps
                     from tOld in ps.DefaultIfEmpty()
                     select new
                     {
                         OVC_IKIND_OLD = tOld?.OVC_IKIND ?? "",
                         OVC_ATTACH_NAME_OLD = tOld?.OVC_ATTACH_NAME ?? "",
                         OVC_FILE_NAME_OLD = tOld?.OVC_FILE_NAME ?? "",
                         ONB_QTY_OLD = tOld?.ONB_QTY ?? null,
                         ONB_PAGES_OLD = tOld?.ONB_PAGES ?? null,
                         OVC_IKIND_NEW = tNew.OVC_IKIND,
                         OVC_ATTACH_NAME_NEW = tNew.OVC_ATTACH_NAME,
                         OVC_FILE_NAME_NEW = tNew.OVC_FILE_NAME,
                         ONB_QTY_NEW = tNew.ONB_QTY,
                         ONB_PAGES_NEW = tNew.ONB_PAGES,
                     }).Where(o => o.OVC_IKIND_OLD == "");
                dtNew = CommonStatic.LinqQueryToDataTable(queryInsert);
                dtDiff.Merge(dtDel);
                dtDiff.Merge(dtNew);
                rptATTACH.DataSource = dtDiff;
                rptATTACH.DataBind();
            }
        }

        private void Import_table1201()
        {
            var query1201 =
               from t in mpms.TBM1201
               where t.OVC_PURCH.Equals(strPurNum)
               select new
               {
                   t.ONB_POI_ICOUNT,
                   OVC_POI_NDESC = t.OVC_POI_NDESC ?? " ",
                   OVC_SAME_QUALITY = t.OVC_SAME_QUALITY ?? " ",
                   OVC_POI_NSTUFF_CHN = t.OVC_POI_NSTUFF_CHN ?? " ",
                   OVC_POI_NSTUFF_ENG = t.OVC_POI_NSTUFF_ENG ?? " ",
                   NSN_KIND = t.NSN_KIND ?? " ",
                   NSN = t.NSN ?? " "
               };

            var query1201OLDMAX =
              (from t in mpms.TBM1201_HISTORY
               where t.OVC_PURCH.Equals(strPurNum)
               group t by t.OVC_VERSION into g
               orderby g.Key descending
               select g).Take(1);

            DataTable dtDiff = new DataTable();
            DataTable dtNew = new DataTable();
            DataTable dtDel = new DataTable();

            foreach (var item in query1201OLDMAX)
            {
                var query1201_OLD =
                from t in item
                select new
                {
                    t.ONB_POI_ICOUNT,
                    OVC_POI_NDESC = t.OVC_POI_NDESC ?? " ",
                    OVC_SAME_QUALITY = t.OVC_SAME_QUALITY ?? " ",
                    OVC_POI_NSTUFF_CHN = t.OVC_POI_NSTUFF_CHN ?? " ",
                    OVC_POI_NSTUFF_ENG = t.OVC_POI_NSTUFF_ENG ?? " ",
                    NSN_KIND = t.NSN_KIND ?? " ",
                    NSN = t.NSN ?? " "
                };
                //異動的項目
                var querydiff =
                    from t in query1201_OLD
                    join t2 in query1201 on  t.ONB_POI_ICOUNT equals t2.ONB_POI_ICOUNT
                    where !t.OVC_POI_NDESC.Equals(t2.OVC_POI_NDESC) ||
                          !t.OVC_SAME_QUALITY.Equals(t2.OVC_SAME_QUALITY) ||
                          !t.OVC_POI_NSTUFF_CHN.Equals(t2.OVC_POI_NSTUFF_CHN) ||
                          !t.OVC_POI_NSTUFF_ENG.Equals(t2.OVC_POI_NSTUFF_ENG) ||
                          !t.NSN_KIND.Equals(t2.NSN_KIND) ||
                          !t.NSN.Equals(t2.NSN)
                    select new
                    {
                        ONB_POI_ICOUNT_OLD = t.ONB_POI_ICOUNT,
                        OVC_POI_NDESC_OLD = t.OVC_POI_NDESC,
                        OVC_SAME_QUALITY_OLD = t.OVC_SAME_QUALITY,
                        OVC_POI_NSTUFF_CHN_OLD = t.OVC_POI_NSTUFF_CHN,
                        OVC_POI_NSTUFF_ENG_OLD = t.OVC_POI_NSTUFF_ENG,
                        NSN_KIND_OLD = t.NSN_KIND,
                        NSN_OLD = t.NSN,
                        ONB_POI_ICOUNT_NEW = t2.ONB_POI_ICOUNT,
                        OVC_POI_NDESC_NEW = t2.OVC_POI_NDESC,
                        OVC_SAME_QUALITY_NEW = t2.OVC_SAME_QUALITY,
                        OVC_POI_NSTUFF_CHN_NEW = t2.OVC_POI_NSTUFF_CHN,
                        OVC_POI_NSTUFF_ENG_NEW = t2.OVC_POI_NSTUFF_ENG,
                        NSN_KIND_NEW = t2.NSN_KIND,
                        NSN_NEW = t2.NSN
                    };
                dtDiff = CommonStatic.LinqQueryToDataTable(querydiff);
                //刪除的項目
                var queryDel =
                     (from tOld in query1201_OLD
                      join tNew in query1201 on tOld.ONB_POI_ICOUNT equals tNew.ONB_POI_ICOUNT into ps
                      from tNew in ps.DefaultIfEmpty()
                      select new
                      {
                          ONB_POI_ICOUNT_OLD = tOld.ONB_POI_ICOUNT,
                          OVC_POI_NDESC_OLD = tOld.OVC_POI_NDESC,
                          OVC_SAME_QUALITY_OLD = tOld.OVC_SAME_QUALITY,
                          OVC_POI_NSTUFF_CHN_OLD = tOld.OVC_POI_NSTUFF_CHN,
                          OVC_POI_NSTUFF_ENG_OLD = tOld.OVC_POI_NSTUFF_ENG,
                          NSN_KIND_OLD = tOld.NSN_KIND,
                          NSN_OLD = tOld.NSN,
                          ONB_POI_ICOUNT_NEW = tNew?.ONB_POI_ICOUNT ?? null,
                          OVC_POI_NDESC_NEW = tNew?.OVC_POI_NDESC ?? " ",
                          OVC_SAME_QUALITY_NEW = tNew?.OVC_SAME_QUALITY ?? " ",
                          OVC_POI_NSTUFF_CHN_NEW = tNew?.OVC_POI_NSTUFF_CHN ?? " ",
                          OVC_POI_NSTUFF_ENG_NEW = tNew?.OVC_POI_NSTUFF_ENG ?? " ",
                          NSN_KIND_NEW = tNew?.NSN_KIND ?? " ",
                          NSN_NEW = tNew?.NSN ?? " "
                      }).Where(o => o.ONB_POI_ICOUNT_NEW == null);
                dtDel = CommonStatic.LinqQueryToDataTable(queryDel);

                var queryInsert =
                    (from tNew in query1201.ToList()
                     join tOld in query1201_OLD on tNew.ONB_POI_ICOUNT equals tOld.ONB_POI_ICOUNT into ps
                     from tOld in ps.DefaultIfEmpty()
                     select new
                     {
                         ONB_POI_ICOUNT_OLD = tOld?.ONB_POI_ICOUNT ?? null,
                         OVC_POI_NDESC_OLD = tOld?.OVC_POI_NDESC ?? " ",
                         OVC_SAME_QUALITY_OLD = tOld?.OVC_SAME_QUALITY ?? " ",
                         OVC_POI_NSTUFF_CHN_OLD = tOld?.OVC_POI_NSTUFF_CHN ?? " ",
                         OVC_POI_NSTUFF_ENG_OLD = tOld?.OVC_POI_NSTUFF_ENG ?? " ",
                         NSN_KIND_OLD = tOld?.NSN_KIND ?? " ",
                         NSN_OLD = tOld?.NSN ?? " ",
                         ONB_POI_ICOUNT_NEW = tNew.ONB_POI_ICOUNT,
                         OVC_POI_NDESC_NEW = tNew.OVC_POI_NDESC,
                         OVC_SAME_QUALITY_NEW = tNew.OVC_SAME_QUALITY,
                         OVC_POI_NSTUFF_CHN_NEW = tNew.OVC_POI_NSTUFF_CHN,
                         OVC_POI_NSTUFF_ENG_NEW = tNew.OVC_POI_NSTUFF_ENG,
                         NSN_KIND_NEW = tNew.NSN_KIND,
                         NSN_NEW = tNew.NSN
                     }).Where(o => o.ONB_POI_ICOUNT_OLD == null);
                dtNew = CommonStatic.LinqQueryToDataTable(queryInsert);
                dtDiff.Merge(dtDel);
                dtDiff.Merge(dtNew);
                rptNSTUFF.DataSource = dtDiff;
                rptNSTUFF.DataBind();
            }
        }

        private void Import_table1118()
        {
            var query1118 =
                from t in mpms.TBM1118
                where t.OVC_PURCH.Equals(strPurNum)
                orderby t.OVC_YY, t.OVC_MM
                select new
                {
                    t.OVC_ISOURCE,
                    t.OVC_IKIND,
                    t.OVC_POI_IBDG,
                    t.OVC_PJNAME,
                    t.OVC_YY,
                    t.OVC_MM,
                    t.ONB_MBUD
                };

            var query1118OLDMAX =
              (from t in mpms.TBM1118_HISTORY
               where t.OVC_PURCH.Equals(strPurNum)
               group t by t.OVC_VERSION into g
               orderby g.Key descending
               select g).Take(1);
           
            DataTable dtDiff = new DataTable();
            DataTable dtNew = new DataTable();
            DataTable dtDel = new DataTable();

            foreach (var item in query1118OLDMAX)
            {
                var query1118_OLD =
                from t in item
                orderby t.OVC_YY, t.OVC_MM
                select new
                {
                    t.OVC_ISOURCE,
                    t.OVC_IKIND,
                    t.OVC_POI_IBDG,
                    t.OVC_PJNAME,
                    t.OVC_YY,
                    t.OVC_MM,
                    t.ONB_MBUD
                };
                //異動的項目
                var querydiff =
                    from t in query1118_OLD
                    join t2 in query1118 on new { t.OVC_ISOURCE, t.OVC_POI_IBDG, t.OVC_YY, t.OVC_MM } equals new { t2.OVC_ISOURCE, t2.OVC_POI_IBDG, t2.OVC_YY, t2.OVC_MM }
                    where   !t.OVC_IKIND.Equals(t2.OVC_IKIND) ||
                            !t.OVC_PJNAME.Equals(t2.OVC_PJNAME) ||
                            (t.ONB_MBUD != t2.ONB_MBUD)
                    select new
                    {
                        OVC_ISOURCE_OLD = t.OVC_ISOURCE,
                        OVC_IKIND_OLD = t.OVC_IKIND,
                        OVC_OVC_POI_IBDG_OLD = t.OVC_POI_IBDG,
                        OVC_OVC_PJNAME_OLD = t.OVC_PJNAME,
                        OVC_OVC_YY_OLD = t.OVC_YY,
                        ONB_OVC_MM_OLD = t.OVC_MM,
                        ONB_MBUD_OLD = t.ONB_MBUD,
                        OVC_ISOURCE_NEW = t2.OVC_ISOURCE,
                        OVC_IKIND_NEW = t2.OVC_IKIND,
                        OVC_OVC_POI_IBDG_NEW = t2.OVC_POI_IBDG,
                        OVC_OVC_PJNAME_NEW = t2.OVC_PJNAME,
                        OVC_OVC_YY_NEW = t2.OVC_YY,
                        ONB_OVC_MM_NEW = t2.OVC_MM,
                        ONB_MBUD_NEW = t2.ONB_MBUD,
                    };
                dtDiff = CommonStatic.LinqQueryToDataTable(querydiff);
                //刪除的項目
                var queryDel =
                     (from tOld in query1118_OLD
                      join tNew in query1118 on new { tOld.OVC_ISOURCE, tOld.OVC_POI_IBDG, tOld.OVC_YY, tOld.OVC_MM } 
                      equals new { tNew.OVC_ISOURCE, tNew.OVC_POI_IBDG, tNew.OVC_YY, tNew.OVC_MM } into ps
                      from tNew in ps.DefaultIfEmpty()
                      select new
                      {
                          OVC_ISOURCE_OLD = tOld.OVC_ISOURCE,
                          OVC_IKIND_OLD = tOld.OVC_IKIND,
                          OVC_OVC_POI_IBDG_OLD = tOld.OVC_POI_IBDG,
                          OVC_OVC_PJNAME_OLD = tOld.OVC_PJNAME,
                          OVC_OVC_YY_OLD = tOld.OVC_YY,
                          ONB_OVC_MM_OLD = tOld.OVC_MM,
                          ONB_MBUD_OLD = tOld.ONB_MBUD,
                          OVC_ISOURCE_NEW = tNew?.OVC_ISOURCE ?? "",
                          OVC_IKIND_NEW = tNew?.OVC_IKIND ?? "",
                          OVC_OVC_POI_IBDG_NEW = tNew?.OVC_POI_IBDG ?? "",
                          OVC_OVC_PJNAME_NEW = tNew?.OVC_PJNAME ?? "",
                          OVC_OVC_YY_NEW = tNew?.OVC_YY ?? "",
                          ONB_OVC_MM_NEW = tNew?.OVC_MM ?? "",
                          ONB_MBUD_NEW = tNew?.ONB_MBUD ?? null,

                      }).Where(o => o.OVC_ISOURCE_NEW == "");
                dtDel = CommonStatic.LinqQueryToDataTable(queryDel);

                var queryInsert =
                    (from tNew in query1118.ToList()
                     join tOld in query1118_OLD on new { tNew.OVC_ISOURCE, tNew.OVC_POI_IBDG, tNew.OVC_YY, tNew.OVC_MM } 
                     equals new { tOld.OVC_ISOURCE, tOld.OVC_POI_IBDG, tOld.OVC_YY, tOld.OVC_MM } into ps
                     from tOld in ps.DefaultIfEmpty()
                     select new
                     {
                         OVC_ISOURCE_OLD = tOld?.OVC_ISOURCE ?? "",
                         OVC_IKIND_OLD = tOld?.OVC_IKIND ?? "",
                         OVC_OVC_POI_IBDG_OLD = tOld?.OVC_POI_IBDG ?? "",
                         OVC_OVC_PJNAME_OLD = tOld?.OVC_PJNAME ?? "",
                         OVC_OVC_YY_OLD = tOld?.OVC_YY ?? "",
                         ONB_OVC_MM_OLD = tOld?.OVC_MM ?? "",
                         ONB_MBUD_OLD = tOld?.ONB_MBUD ?? null,
                         OVC_ISOURCE_NEW = tNew.OVC_ISOURCE,
                         OVC_IKIND_NEW = tNew.OVC_IKIND,
                         OVC_OVC_POI_IBDG_NEW = tNew.OVC_POI_IBDG,
                         OVC_OVC_PJNAME_NEW = tNew.OVC_PJNAME,
                         OVC_OVC_YY_NEW = tNew.OVC_YY,
                         ONB_OVC_MM_NEW = tNew.OVC_MM,
                         ONB_MBUD_NEW = tNew.ONB_MBUD,
                     }).Where(o => o.OVC_ISOURCE_OLD == "");
                dtNew = CommonStatic.LinqQueryToDataTable(queryInsert);
                dtDiff.Merge(dtDel);
                dtDiff.Merge(dtNew);
                rptDeatilBudget.DataSource = dtDiff;
                rptDeatilBudget.DataBind();
            }

        }

        private void Import_table1231()
        {
            var query1231 =
                from t in mpms.TBM1231
                join t1407 in mpms.TBM1407 on t.OVC_CURRENT equals t1407.OVC_PHR_ID
                where t.OVC_PURCH.Equals(strPurNum) && t1407.OVC_PHR_CATE.Equals("")
                select new
                {
                    t.OVC_ISOURCE,
                    t.OVC_IKIND,
                    t.OVC_PUR_DAPPR_PLAN,
                    t.OVC_PUR_APPR_PLAN,
                    t1407.OVC_PHR_DESC,
                    t.ONB_RATE,
                    t.ONB_MONEY
                };

            var query1231OLDMAX =
               (from t in mpms.TBM1231_HISTORY
                where t.OVC_PURCH.Equals(strPurNum)
                group t by t.OVC_VERSION into g
                orderby g.Key descending
                select g).Take(1);

            DataTable dtDiff = new DataTable();
            DataTable dtNew = new DataTable();
            DataTable dtDel = new DataTable();

            foreach (var item in query1231OLDMAX)
            {
                var query1231_1OLD =
                from t in item
                join t1407 in mpms.TBM1407 on t.OVC_CURRENT equals t1407.OVC_PHR_ID
                where t1407.OVC_PHR_CATE.Equals("")
                select new
                {
                    t.OVC_ISOURCE,
                    t.OVC_IKIND,
                    t.OVC_PUR_DAPPR_PLAN,
                    t.OVC_PUR_APPR_PLAN,
                    t1407.OVC_PHR_DESC,
                    t.ONB_RATE,
                    t.ONB_MONEY
                };
                //異動的項目
                var querydiff =
                    from t in query1231_1OLD
                    join t2 in query1231 on t.OVC_ISOURCE equals t2.OVC_ISOURCE
                    where !t.OVC_IKIND.Equals(t2.OVC_IKIND) ||
                            !t.OVC_PUR_DAPPR_PLAN.Equals(t2.OVC_PUR_DAPPR_PLAN) ||
                            !t.OVC_PUR_APPR_PLAN.Equals(t2.OVC_PUR_APPR_PLAN) ||
                            !t.OVC_PHR_DESC.Equals(t2.OVC_PHR_DESC) ||
                            (t.ONB_RATE != t2.ONB_RATE) ||
                            (t.ONB_MONEY != t2.ONB_MONEY)
                    select new
                    {
                        OVC_ISOURCE_OLD = t.OVC_ISOURCE,
                        OVC_IKIND_OLD = t.OVC_IKIND,
                        OVC_PUR_DAPPR_PLAN_OLD = t.OVC_PUR_DAPPR_PLAN,
                        OVC_PUR_APPR_PLAN_OLD = t.OVC_PUR_APPR_PLAN,
                        OVC_PHR_DESC_OLD = t.OVC_PHR_DESC,
                        ONB_RATE_OLD = t.ONB_RATE,
                        ONB_MONEY_OLD = t.ONB_MONEY,
                        OVC_ISOURCE_NEW = t2.OVC_ISOURCE,
                        OVC_IKIND_NEW = t2.OVC_IKIND,
                        OVC_PUR_DAPPR_PLAN_NEW = t2.OVC_PUR_DAPPR_PLAN,
                        OVC_PUR_APPR_PLAN_NEW = t2.OVC_PUR_APPR_PLAN,
                        OVC_PHR_DESC_NEW = t2.OVC_PHR_DESC,
                        ONB_RATE_NEW = t2.ONB_RATE,
                        ONB_MONEY_NEW = t2.ONB_MONEY
                    };
                dtDiff = CommonStatic.LinqQueryToDataTable(querydiff);
                //刪除的項目
                var queryDel =
                     (from tOld in query1231_1OLD
                      join tNew in query1231 on tOld.OVC_ISOURCE equals tNew.OVC_ISOURCE into ps
                      from tNew in ps.DefaultIfEmpty()
                      select new
                      {
                          OVC_ISOURCE_OLD = tOld.OVC_ISOURCE,
                          OVC_IKIND_OLD = tOld.OVC_IKIND,
                          OVC_PUR_DAPPR_PLAN_OLD = tOld.OVC_PUR_DAPPR_PLAN,
                          OVC_PUR_APPR_PLAN_OLD = tOld.OVC_PUR_APPR_PLAN,
                          OVC_PHR_DESC_OLD = tOld.OVC_PHR_DESC,
                          ONB_RATE_OLD = tOld.ONB_RATE,
                          ONB_MONEY_OLD = tOld.ONB_MONEY,
                          OVC_ISOURCE_NEW = tNew?.OVC_ISOURCE ?? "",
                          OVC_IKIND_NEW = tNew?.OVC_IKIND ?? "",
                          OVC_PUR_DAPPR_PLAN_NEW = tNew?.OVC_PUR_DAPPR_PLAN ?? "",
                          OVC_PUR_APPR_PLAN_NEW = tNew?.OVC_PUR_APPR_PLAN ?? "",
                          OVC_PHR_DESC_NEW = tNew?.OVC_PHR_DESC ?? "",
                          ONB_RATE_NEW = tNew?.ONB_RATE ?? null,
                          ONB_MONEY_NEW = tNew?.ONB_MONEY ?? null

                      }).Where(o => o.OVC_ISOURCE_NEW == "");
                dtDel = CommonStatic.LinqQueryToDataTable(queryDel);

                var queryInsert =
                    (from tNew in query1231.ToList()
                     join tOld in query1231_1OLD on tNew.OVC_ISOURCE equals tOld.OVC_ISOURCE into ps
                     from tOld in ps.DefaultIfEmpty()
                     select new
                     {
                         OVC_ISOURCE_OLD = tOld.OVC_ISOURCE ?? "",
                         OVC_IKIND_OLD = tOld.OVC_IKIND ?? "",
                         OVC_PUR_DAPPR_PLAN_OLD = tOld.OVC_PUR_DAPPR_PLAN ?? "",
                         OVC_PUR_APPR_PLAN_OLD = tOld.OVC_PUR_APPR_PLAN ?? "",
                         OVC_PHR_DESC_OLD = tOld.OVC_PHR_DESC ?? "",
                         ONB_RATE_OLD = tOld?.ONB_RATE ?? null,
                         ONB_MONEY_OLD = tOld?.ONB_MONEY ?? null,
                         OVC_ISOURCE_NEW = tNew.OVC_ISOURCE,
                         OVC_IKIND_NEW = tNew.OVC_IKIND,
                         OVC_PUR_DAPPR_PLAN_NEW = tNew.OVC_PUR_DAPPR_PLAN,
                         OVC_PUR_APPR_PLAN_NEW = tNew.OVC_PUR_APPR_PLAN,
                         OVC_PHR_DESC_NEW = tNew.OVC_PHR_DESC,
                         ONB_RATE_NEW = tNew.ONB_RATE,
                         ONB_MONEY_NEW = tNew.ONB_MONEY
                     }).Where(o => o.OVC_ISOURCE_OLD == "");
                dtNew = CommonStatic.LinqQueryToDataTable(queryInsert);
                dtDiff.Merge(dtDel);
                dtDiff.Merge(dtNew);
                rptISOURCE.DataSource = dtDiff;
                rptISOURCE.DataBind();
            }

        }

        private void Import_table1301()
        {
            var query1301_History =
                (from t in mpms.TBM1301_HISTORY
                 where t.OVC_PURCH.Equals(strPurNum)
                 orderby t.OVC_VERSION descending
                 select t).FirstOrDefault();

            var query1301 =
                (from t in mpms.TBM1301
                where t.OVC_PURCH.Equals(strPurNum)
                select t).FirstOrDefault();
            lblOVC_PURCH.Text = query1301.OVC_PURCH + query1301.OVC_PUR_AGENCY;
            DataTable dt = new DataTable();
            DataTable dtCompare = new DataTable();
            dt.Columns.Add("ColumnName");
            dt.Columns.Add("OldData");
            dt.Columns.Add("NewData");
            dtCompare.Columns.Add("ColumnName");
            dtCompare.Columns.Add("OldData");
            dtCompare.Columns.Add("NewData");

            dt.Rows.Add("內外購別", query1301_History.OVC_PURCH_KIND, query1301.OVC_PURCH_KIND);
            dt.Rows.Add("承辦人階級", query1301_History.OVC_USER_TITLE, query1301.OVC_USER_TITLE);
            dt.Rows.Add("核定日期", query1301_History.OVC_PUR_DAPPROVE, query1301.OVC_PUR_DAPPROVE);
            dt.Rows.Add("中文購案名稱", query1301_History.OVC_PUR_IPURCH, query1301.OVC_PUR_IPURCH);
            dt.Rows.Add("英文購案名稱", query1301_History.OVC_PUR_IPURCH_ENG, query1301.OVC_PUR_IPURCH_ENG);
            dt.Rows.Add("承辦人", query1301_History.OVC_PUR_USER, query1301.OVC_PUR_USER);
            dt.Rows.Add("鍵入者", query1301_History.OVC_KEYIN, query1301.OVC_KEYIN);
            dt.Rows.Add("聯絡自動電話", query1301_History.OVC_PUR_IUSER_PHONE, query1301.OVC_PUR_IUSER_PHONE);
            dt.Rows.Add("聯絡軍線電話1", query1301_History.OVC_PUR_IUSER_PHONE_EXT, query1301.OVC_PUR_IUSER_PHONE_EXT);
            dt.Rows.Add("聯絡軍線電話2", query1301_History.OVC_PUR_IUSER_PHONE_EXT1, query1301.OVC_PUR_IUSER_PHONE_EXT1);
            dt.Rows.Add("聯絡手機電話", query1301_History.OVC_USER_CELLPHONE, query1301.OVC_USER_CELLPHONE);
            dt.Rows.Add("採購單位地區方式", query1301_History.OVC_PUR_AGENCY, query1301.OVC_PUR_AGENCY);
            dt.Rows.Add("實際或預估金額", query1301_History.OVC_EST_REAR, query1301.OVC_EST_REAR);
            dt.Rows.Add("軍品類別", query1301_History.OVC_PUR_NPURCH, query1301.OVC_PUR_NPURCH);
            dt.Rows.Add("軍品用途", query1301_History.OVC_TARGET_DO, query1301.OVC_TARGET_DO);
            dt.Rows.Add("幣別", query1301_History.OVC_PUR_CURRENT, query1301.OVC_PUR_CURRENT);
            dt.Rows.Add("匯率", query1301_History.ONB_PUR_RATE, query1301.ONB_PUR_RATE);
            dt.Rows.Add("預算總金額", query1301_History.ONB_PUR_BUDGET, query1301.ONB_PUR_BUDGET);
            dt.Rows.Add("採購單位", query1301_History.OVC_AGNT_IN, query1301.OVC_AGNT_IN);
            dt.Rows.Add("免進口關稅", query1301_History.OVC_PUR_FEE_OK, query1301.OVC_PUR_FEE_OK);
            dt.Rows.Add("免貨物稅", query1301_History.OVC_PUR_GOOD_OK, query1301.OVC_PUR_GOOD_OK);
            dt.Rows.Add("免營業稅", query1301_History.OVC_PUR_TAX_OK, query1301.OVC_PUR_TAX_OK);
            dt.Rows.Add("撤案日", query1301_History.OVC_PUR_DCANPO, query1301.OVC_PUR_DCANPO);
            dt.Rows.Add("撤案原因", query1301_History.OVC_PUR_DCANRE, query1301.OVC_PUR_DCANRE);
            dt.Rows.Add("是否提供為範例", query1301_History.OVC_EXAMPLE_SUPPORT, query1301.OVC_EXAMPLE_SUPPORT);
            dt.Rows.Add("主官核批日", query1301_History.OVC_PUR_ALLOW, query1301.OVC_PUR_ALLOW);
            dt.Rows.Add("建檔日", query1301_History.OVC_PUR_CREAT, query1301.OVC_PUR_CREAT);
            dt.Rows.Add("購案目前承辦單位", query1301_History.OVC_DOING_UNIT, query1301.OVC_DOING_UNIT);
            dt.Rows.Add("謹呈之單位名稱", query1301_History.OVC_SUPERIOR_UNIT, query1301.OVC_SUPERIOR_UNIT);
            dt.Rows.Add("單位主官", query1301_History.OVC_SECTION_CHIEF, query1301.OVC_SECTION_CHIEF);
            dt.Rows.Add("交貨時間", query1301_History.OVC_SHIP_TIMES, query1301.OVC_SHIP_TIMES);
            dt.Rows.Add("交貨地點", query1301_History.OVC_RECEIVE_PLACE, query1301.OVC_RECEIVE_PLACE);
            dt.Rows.Add("檢驗方法", query1301_History.OVC_POI_IINSPECT, query1301.OVC_POI_IINSPECT);
            dt.Rows.Add("採購地區", query1301_History.OVC_COUNTRY, query1301.OVC_COUNTRY);
            dt.Rows.Add("運輸方式", query1301_History.OVC_WAY_TRANS, query1301.OVC_WAY_TRANS);
            dt.Rows.Add("接收單位", query1301_History.OVC_RECEIVE_NSECTION, query1301.OVC_RECEIVE_NSECTION);
            dt.Rows.Add("起運及輸入口案", query1301_History.OVC_FROM_TO, query1301.OVC_FROM_TO);
            dt.Rows.Add("接收地點", query1301_History.OVC_TO_PLACE, query1301.OVC_TO_PLACE);
            dt.Rows.Add("預算總金額_NT", query1301_History.ONB_PUR_BUDGET_NT, query1301.ONB_PUR_BUDGET_NT);
            dt.Rows.Add("保留擴充金額_保留增購", query1301_History.ONB_RESERVE_AMOUNT, query1301.ONB_RESERVE_AMOUNT);
            dt.Rows.Add("特殊採購", query1301_History.OVC_SPECIAL, query1301.OVC_SPECIAL);
            dt.Rows.Add("押標金額度", query1301_History.OVC_BID_MONEY, query1301.OVC_BID_MONEY);
            dt.Rows.Add("協商措施", query1301_History.OVC_NEGOTIATION, query1301.OVC_NEGOTIATION);
            dt.Rows.Add("報價及決標方式", query1301_History.OVC_QUOTE, query1301.OVC_QUOTE);
            dt.Rows.Add("計畫性質", query1301_History.OVC_PLAN_PURCH, query1301.OVC_PLAN_PURCH);
            dt.Rows.Add("複製購案之原購案編號", query1301_History.OVC_COPY_FROM, query1301.OVC_COPY_FROM);
            dt.Rows.Add("採購屬性", query1301_History.OVC_LAB, query1301.OVC_LAB);
            dt.Rows.Add("招標方式", query1301_History.OVC_PUR_ASS_VEN_CODE, query1301.OVC_PUR_ASS_VEN_CODE);
            dt.Rows.Add("投標段次", query1301_History.OVC_BID_TIMES, query1301.OVC_BID_TIMES);
            dt.Rows.Add("決標原則", query1301_History.OVC_BID, query1301.OVC_BID);
            dt.Rows.Add("單位全銜", query1301_History.OVC_PUR_NSECTION, query1301.OVC_PUR_NSECTION);
            dt.Rows.Add("單位代碼", query1301_History.OVC_PUR_SECTION, query1301.OVC_PUR_SECTION);
            dt.Rows.Add("申購文號", query1301_History.OVC_PROPOSE, query1301.OVC_PROPOSE);
            dt.Rows.Add("申購日期", query1301_History.OVC_DPROPOSE, query1301.OVC_DPROPOSE);
            dt.Rows.Add("核定文號", query1301_History.OVC_PUR_APPROVE, query1301.OVC_PUR_APPROVE);
            dt.Rows.Add("版本", query1301_History.OVC_VERSION, query1301.OVC_VERSION);

            foreach(DataRow row in dt.Rows)
            {
                if (!row["OldData"].ToString().Equals(row["NewData"].ToString()))
                {
                    dtCompare.Rows.Add(row["ColumnName"].ToString(), row["OldData"].ToString(),row["NewData"].ToString());
                }
            }
            GV_1301_History.DataSource = dtCompare;
            GV_1301_History.DataBind();
        }

        private void Import_table1220()
        {
            var queryKind = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurNum)).Select(o => o.OVC_PUR_AGENCY).FirstOrDefault();
            string ovcIkind = "";
            string ovcIkinMark = "";
            switch (queryKind)
            {
                case "B":
                case "L":
                case "P":
                    ovcIkind = "M3";
                    break;
                case "M":
                case "S":
                    ovcIkind = "F3";
                    break;
                default:
                    ovcIkind = "W3";
                    break;
            }

            switch (queryKind)
            {
                case "B":
                case "L":
                case "P":
                    ovcIkinMark = "M4";
                    break;
                case "M":
                case "S":
                    ovcIkinMark = "F4";
                    break;
                default:
                    ovcIkinMark = "W4";
                    break;
            }
            DataTable dt = QueryTBM1220("物資申請書請求事項-->", ovcIkind);
            DataTable dt2 = QueryTBM1220("物資申請書備考-->", ovcIkinMark);
            dt.Merge(dt2);
            rptMemo.DataSource = dt;
            rptMemo.DataBind();
            
        }

        private DataTable QueryTBM1220(string markName,string iKind)
        {
            var query1220_1 =
                from t in mpms.TBM1220_1
                join t2 in mpms.TBM1220_2 on t.OVC_IKIND equals t2.OVC_IKIND
                where t.OVC_PURCH.Equals(strPurNum) && t.OVC_IKIND.StartsWith(iKind)
                orderby t.OVC_IKIND, t.ONB_NO
                select new
                {
                    t.OVC_IKIND,
                    t2.OVC_MEMO_NAME,
                    t.ONB_NO,
                    t.OVC_MEMO,
                };
            var query1220_1OLDMAX =
               (from t in mpms.TBM1220_1_HISTORY
                where t.OVC_PURCH.Equals(strPurNum) && t.OVC_IKIND.StartsWith(iKind)
                group t by t.OVC_VERSION into g
                orderby g.Key descending
                select g).Take(1);

            DataTable dtDiff = new DataTable();
            DataTable dtNew = new DataTable();
            DataTable dtDel = new DataTable();
            foreach (var item in query1220_1OLDMAX)
            {
                var query1220_1OLD =
                from t in item
                join t2 in mpms.TBM1220_2 on t.OVC_IKIND equals t2.OVC_IKIND
                where t.OVC_PURCH.Equals(strPurNum) && t.OVC_IKIND.StartsWith(iKind)
                orderby t.OVC_VERSION descending, t.OVC_IKIND, t.ONB_NO
                select new
                {
                    t.OVC_IKIND,
                    t2.OVC_MEMO_NAME,
                    t.ONB_NO,
                    t.OVC_MEMO,
                };
                //異動的項目
                var querydiff =
                    from t in query1220_1OLD
                    join t2 in query1220_1 on new { t.OVC_IKIND, t.ONB_NO } equals new { t2.OVC_IKIND, t2.ONB_NO }
                    where !t.OVC_MEMO.Equals(t2.OVC_MEMO)
                    select new
                    {
                        OVC_MEMO_NAME = t.OVC_MEMO_NAME,
                        MEMO_NAME_NEW = t.OVC_MEMO_NAME,
                        MEMO_ORIGIN = t.OVC_MEMO,
                        MEMO_NEW = t2.OVC_MEMO,
                        ONB_NO_NEW = t.ONB_NO,
                        t.ONB_NO,
                    };
                dtDiff = CommonStatic.LinqQueryToDataTable(querydiff);
                //刪除的項目
                var queryDel =
                     (from tOld in query1220_1OLD
                      join tNew in query1220_1 on new { tOld.OVC_IKIND, tOld.ONB_NO } equals new { tNew.OVC_IKIND, tNew.ONB_NO } into ps
                      from tNew in ps.DefaultIfEmpty()
                      select new
                      {
                          OVC_MEMO_NAME = tOld.OVC_MEMO_NAME,
                          MEMO_NAME_NEW = tNew?.OVC_MEMO ?? "",
                          MEMO_ORIGIN = tOld.OVC_MEMO,
                          MEMO_NEW = tNew?.OVC_MEMO ?? "",
                          ONB_NO_NEW = tNew?.ONB_NO ?? null,
                          tOld.ONB_NO,
                      }).Where(o => o.MEMO_NEW == "");
                dtDel = CommonStatic.LinqQueryToDataTable(queryDel);

                var queryInsert =
                    (from tNew in query1220_1.ToList()
                     join tOld in query1220_1OLD on new { tNew.OVC_IKIND, tNew.ONB_NO } equals new { tOld.OVC_IKIND, tOld.ONB_NO } into ps
                     from tOld in ps.DefaultIfEmpty()
                     select new
                     {
                         OVC_MEMO_NAME = tOld?.OVC_MEMO_NAME ?? "",
                         MEMO_NAME_NEW = tNew.OVC_MEMO_NAME,
                         MEMO_ORIGIN = tOld?.OVC_MEMO_NAME ?? "",
                         MEMO_NEW = tNew.OVC_MEMO,
                         ONB_NO_NEW = tNew.ONB_NO,
                         ONB_NO = tOld?.ONB_NO ?? null,
                     }).Where(o => o.MEMO_ORIGIN == "");
                dtNew = CommonStatic.LinqQueryToDataTable(queryInsert);
                dtDiff.Merge(dtDel);
                dtDiff.Merge(dtNew);
            }

            foreach (DataRow row in dtDiff.Rows)
            {
                if (!string.IsNullOrEmpty(row["MEMO_NAME_NEW"].ToString()))
                {

                    row["MEMO_NAME_NEW"] = markName + row["MEMO_NAME_NEW"].ToString();
                }
                if (!string.IsNullOrEmpty(row["OVC_MEMO_NAME"].ToString()))
                {
                    row["OVC_MEMO_NAME"] = markName + row["OVC_MEMO_NAME"].ToString();
                }
            }
            return dtDiff;
        }

        private void DoingReapter(RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lbl = (Label)e.Item.FindControl("lblMemoOri");
                if (string.IsNullOrEmpty(lbl.Text))
                {
                    HtmlTableRow li = e.Item.FindControl("trBefore") as HtmlTableRow;
                    li.Attributes.Add("style", "display: none;");
                    Label lblChangeType = (Label)e.Item.FindControl("lblChangeType");
                    lblChangeType.Text = "新增此筆";
                }
            }
        }

        protected void rptMemo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DoingReapter(e);
        }

        protected void rptISOURCE_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DoingReapter(e);
        }

        protected void rptDeatilBudget_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DoingReapter(e);
        }

        protected void rptATTACH_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DoingReapter(e);
        }

        protected void rptNSTUFF_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DoingReapter(e);
        }
    }
}