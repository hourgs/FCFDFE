using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;
using System.Data.Entity;

namespace FCFDFE.pages.MTS.D
{
    public partial class MTS_D13 : System.Web.UI.Page
    {

        public string strMenuName = "", strMenuNameItem = "";
        private MTSEntities mtse = new MTSEntities();
        Common FCommon = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtOdtInvDate1, txtOdtInvDate2);
                    list_dataImport(drpOdtApplyDate);
                    DataTable dt = CommonStatic.ListToDataTable(mtse.TBGMT_DEPT_CLASS.Select(x => x).ToList());
                    list_dataImport2(drpOvcMilitaryType, dt, "OVC_CLASS_NAME", "OVC_CLASS");
                    PanelStatistics.Visible = true;
                    PanelDetails.Visible = false;
                }
            }
           
        }
        public void list_dataImport(ListControl list)
        {
            //先將下拉式選單清空
            list.Items.Clear();

            int CalDateYear = Convert.ToInt16(DateTime.Now.ToString("yyyy")) - 1911;
            int num = CalDateYear;
            for (int i = num; i > 93; i--)
            {
                list.Items.Add(Convert.ToString(i));
            }
        }

        public void list_dataImport2(ListControl list, DataTable dt, string textField, string valueField)
        {
            //先將下拉式選單清空
            list.Items.Clear();
            list.AppendDataBoundItems = true;
            list.Items.Add("不限定");
            list.DataSource = dt;
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataBind();
        }

        private void datastatistics()
        {
            string strODT_APPLY_DATE = drpOdtApplyDate.SelectedItem.ToString();
            if (strODT_APPLY_DATE.Length != 3)
            {
                if (strODT_APPLY_DATE.Length == 2)
                    strODT_APPLY_DATE = "0" + strODT_APPLY_DATE;
                else if (strODT_APPLY_DATE.Length == 1)
                    strODT_APPLY_DATE = "00" + strODT_APPLY_DATE;
                else
                    strODT_APPLY_DATE = "000";
            }
            #region
            var queryeinf =
                from dept in mtse.TBGMT_DEPT_CLASS
                join einn in mtse.TBGMT_EINN on dept.OVC_CLASS equals einn.OVC_MILITARY_TYPE
                join einf in mtse.TBGMT_EINF on einn.OVC_INF_NO equals einf.OVC_INF_NO
                where einf.OVC_INF_NO.StartsWith("EINF" + strODT_APPLY_DATE)
                group einf by dept.OVC_CLASS into g
                select new
                 {
                     OVC_CLASS = g.Key,
                     ODT_SHOULD = g.Sum(p => p.ONB_AMOUNT)
                 };
            DataTable dteinf = new DataTable();
            DataRow drinf;
            dteinf.Columns.Add(new DataColumn("OVC_CLASS", typeof(String)));
            dteinf.Columns.Add(new DataColumn("OVC_SHOULD", typeof(Decimal)));
            foreach (var aa in queryeinf)
            {
                drinf = dteinf.NewRow();
                drinf[0] = aa.OVC_CLASS;
                if (aa.ODT_SHOULD == null)
                {
                    drinf[1] = 0;
                }
                else
                {
                    drinf[1] = aa.ODT_SHOULD;
                }
                
                dteinf.Rows.Add(drinf);
            }
            var queryeinfyes =
                from dept in mtse.TBGMT_DEPT_CLASS
                join einn in mtse.TBGMT_EINN on dept.OVC_CLASS equals einn.OVC_MILITARY_TYPE
                join einf in mtse.TBGMT_EINF on einn.OVC_INF_NO equals einf.OVC_INF_NO
                where einf.OVC_IS_PAID == "已付款"
                where einn.OVC_INF_NO.Contains("EINF" + strODT_APPLY_DATE)
                group einf by dept.OVC_CLASS into g
                select new
                {
                    OVC_CLASS = g.Key,
                    ODT_SHOULD = g.Sum(p => p.ONB_AMOUNT),
                };
            DataTable dteinfyes = new DataTable();
            DataRow drinfyes;
            dteinfyes.Columns.Add(new DataColumn("OVC_CLASS", typeof(String)));
            dteinfyes.Columns.Add(new DataColumn("OVC_SHOULD", typeof(Decimal)));
            foreach (var aa in queryeinfyes)
            {
                drinfyes = dteinfyes.NewRow();
                drinfyes[0] = aa.OVC_CLASS;
                if (aa.ODT_SHOULD == null)
                {
                    drinfyes[1] = 0;
                }
                else
                {
                    drinfyes[1] = aa.ODT_SHOULD;
                }
                
                dteinfyes.Rows.Add(drinfyes);
            }
            var queryeinfno =
                from dept in mtse.TBGMT_DEPT_CLASS
                join einn in mtse.TBGMT_EINN on dept.OVC_CLASS equals einn.OVC_MILITARY_TYPE
                join einf in mtse.TBGMT_EINF on einn.OVC_INF_NO equals einf.OVC_INF_NO
                where einf.OVC_IS_PAID == "未付款"
                where einn.OVC_INF_NO.Contains("EINF" + strODT_APPLY_DATE)
                group einf by dept.OVC_CLASS into g
                select new
                {
                    OVC_CLASS = g.Key,
                    ODT_SHOULD = g.Sum(p => p.ONB_AMOUNT),
                };
            DataTable dteinfno = new DataTable();
            DataRow drinfno;
            dteinfno.Columns.Add(new DataColumn("OVC_CLASS", typeof(String)));
            dteinfno.Columns.Add(new DataColumn("OVC_SHOULD", typeof(Decimal)));
            foreach (var aa in queryeinfno)
            {
                drinfno = dteinfno.NewRow();
                drinfno[0] = aa.OVC_CLASS;
                if (aa.ODT_SHOULD == null)
                {
                    drinfno[1] = 0;
                }
                else
                {
                    drinfno[1] = aa.ODT_SHOULD;
                }

                dteinfno.Rows.Add(drinfno);
            }
            var queryiinf =
                from dept in mtse.TBGMT_DEPT_CLASS
                join iinn in mtse.TBGMT_IINN on dept.OVC_CLASS equals iinn.OVC_MILITARY_TYPE
                join iinf in mtse.TBGMT_IINF on iinn.OVC_INF_NO equals iinf.OVC_INF_NO
                where iinn.OVC_INF_NO.Contains("IINF" + strODT_APPLY_DATE)
                group iinf by dept.OVC_CLASS into g
                select new
                {
                    OVC_CLASS = g.Key,
                    ODT_SHOULD = g.Sum(p => p.ONB_AMOUNT),
                };
            DataTable dtiinf = new DataTable();
            DataRow driinf;
            dtiinf.Columns.Add(new DataColumn("OVC_CLASS", typeof(String)));
            dtiinf.Columns.Add(new DataColumn("OVC_SHOULD", typeof(Decimal)));
            foreach (var aa in queryiinf)
            {
                driinf = dtiinf.NewRow();
                driinf[0] = aa.OVC_CLASS;
                if (aa.ODT_SHOULD == null)
                    driinf[1] = 0;
                else
                {
                    driinf[1] = aa.ODT_SHOULD;
                }
                    
                dtiinf.Rows.Add(driinf);
            }
            var queryiinfyes =
                from dept in mtse.TBGMT_DEPT_CLASS
                join iinn in mtse.TBGMT_IINN on dept.OVC_CLASS equals iinn.OVC_MILITARY_TYPE
                join iinf in mtse.TBGMT_IINF on iinn.OVC_INF_NO equals iinf.OVC_INF_NO
                where iinn.OVC_INF_NO.Contains("IINF" + strODT_APPLY_DATE)
                where iinf.OVC_IS_PAID == "已付款"
                group iinf by dept.OVC_CLASS into g
                select new
                {
                    OVC_CLASS = g.Key,
                    ODT_SHOULD = g.Sum(p => p.ONB_AMOUNT),
                };
            DataTable dtiinfyes = new DataTable();
            DataRow driinfyes;
            dtiinfyes.Columns.Add(new DataColumn("OVC_CLASS", typeof(String)));
            dtiinfyes.Columns.Add(new DataColumn("OVC_SHOULD", typeof(Decimal)));
            foreach (var aa in queryiinfyes)
            {
                driinfyes = dtiinfyes.NewRow();
                driinfyes[0] = aa.OVC_CLASS;
                if(aa.ODT_SHOULD == null)
                {
                    driinfyes[1] = 0;
                }
                else
                {
                   driinfyes[1] = aa.ODT_SHOULD;
                }

                dtiinfyes.Rows.Add(driinfyes);
            }
            var queryiinfno =
                from dept in mtse.TBGMT_DEPT_CLASS
                join iinn in mtse.TBGMT_IINN on dept.OVC_CLASS equals iinn.OVC_MILITARY_TYPE
                join iinf in mtse.TBGMT_IINF on iinn.OVC_INF_NO equals iinf.OVC_INF_NO
                where iinn.OVC_INF_NO.Contains("IINF" + strODT_APPLY_DATE)
                where iinf.OVC_IS_PAID == "未付款"
                group iinf by dept.OVC_CLASS into g
                select new
                {
                    OVC_CLASS = g.Key,
                    ODT_SHOULD = g.Sum(p => p.ONB_AMOUNT),
                };
            DataTable dtiinfno = new DataTable();
            DataRow driinfno;
            dtiinfno.Columns.Add(new DataColumn("OVC_CLASS", typeof(String)));
            dtiinfno.Columns.Add(new DataColumn("OVC_SHOULD", typeof(Decimal)));
            foreach (var aa in queryiinfno)
            {
                driinfno = dtiinfno.NewRow();
                driinfno[0] = aa.OVC_CLASS;
                if(aa.ODT_SHOULD == null)
                {
                    driinfno[1] = 0;
                }
                else
                {
                    driinfno[1] = aa.ODT_SHOULD;
                }
                
                dtiinfno.Rows.Add(driinfno);
            }
            var query = from dept in mtse.TBGMT_DEPT_CLASS.AsEnumerable()
                        join einf in dteinf.AsEnumerable()
                        on dept.OVC_CLASS equals einf.Field<string>("OVC_CLASS")
                        into tmpeinf
                        from einf in tmpeinf.DefaultIfEmpty()
                        join einfyes in dteinfyes.AsEnumerable()
                        on dept.OVC_CLASS equals einfyes.Field<string>("OVC_CLASS")
                        into tmpeinfyes
                        from einfyes in tmpeinfyes.DefaultIfEmpty()
                        join einfno in dteinfno.AsEnumerable()
                        on dept.OVC_CLASS equals einfno.Field<string>("OVC_CLASS")
                        into tmpeinfno
                        from einfno in tmpeinfno.DefaultIfEmpty()
                        join iinf in dtiinf.AsEnumerable()
                        on dept.OVC_CLASS equals iinf.Field<string>("OVC_CLASS")
                        into tmpiinf
                        from iinf in tmpiinf.DefaultIfEmpty()
                        join iinfyes in dtiinfyes.AsEnumerable()
                        on dept.OVC_CLASS equals iinfyes.Field<string>("OVC_CLASS")
                        into tmpiinfyes
                        from iinfyes in tmpiinfyes.DefaultIfEmpty()
                        join iinfno in dtiinfno.AsEnumerable()
                        on dept.OVC_CLASS equals iinfno.Field<string>("OVC_CLASS")
                        into tmpiinfno
                        from iinfno in tmpiinfno.DefaultIfEmpty()
                        select new
                        {
                            OVC_CLASS_NAME = dept.OVC_CLASS_NAME,
                            ODT_SHOULD = einf?.Field<decimal>("OVC_SHOULD")??Decimal.Zero + iinf?.Field<decimal>("OVC_SHOULD")??Decimal.Zero,
                            ODT_YES = einfyes?.Field<decimal>("OVC_SHOULD") ?? Decimal.Zero + iinfyes?.Field<decimal>("OVC_SHOULD") ?? Decimal.Zero,
                            ODT_NO = einfno?.Field<decimal>("OVC_SHOULD") ?? Decimal.Zero + iinfno?.Field<decimal>("OVC_SHOULD") ?? Decimal.Zero
                        };
            if (query.Count() > 1000)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
            query = query.Take(1000);



            DataTable dt = new DataTable();
            dt = CommonStatic.LinqQueryToDataTable(query);



           // ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_INSRATE, dt, strField);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_INSRATE, dt);
            #endregion

        }

        private void datadetailsapplyed()
        {
            string strMessage= "";
            string strOvcIsPaid = drpIsPaid.SelectedItem.ToString();
            string strOvcClassName = drpOvcMilitaryType.SelectedItem.ToString();
            string strImportOrExport = drpImportOrExport.SelectedItem.ToString();
            string strOdtInvDate1 = txtOdtInvDate1.Text;
            string strOdtInvDate2 = txtOdtInvDate2.Text;
            string strOvcPurchNo = txtOvcPurchNo.Text;
            if(strOdtInvDate1.Equals(string.Empty) && txtOdtInvDate2.Equals(string.Empty) && strOvcPurchNo.Equals(string.Empty))
            {
                strMessage += "日期與案號至少擇一輸入";
            }
            if (strMessage.Equals(string.Empty))
            {

                DataTable dt = new DataTable();
                var query =
                    (from einn in mtse.TBGMT_EINN
                     join edf in mtse.TBGMT_EDF on einn.OVC_EDF_NO equals edf.OVC_EDF_NO into tbedf
                     from edf in tbedf.DefaultIfEmpty()
                     join einf in mtse.TBGMT_EINF on einn.OVC_INF_NO equals einf.OVC_INF_NO into tbeinf
                     from einf in tbeinf.DefaultIfEmpty()
                     join dept in mtse.TBGMT_DEPT_CLASS on einn.OVC_MILITARY_TYPE equals dept.OVC_CLASS into tbdept
                     from dept in tbdept.DefaultIfEmpty()
                     join company in mtse.TBGMT_COMPANY on einf.CO_SN equals company.CO_SN into tbcompany
                     from company in tbcompany.DefaultIfEmpty()
                     select new
                     {
                         OVC_INN_NO = einn.OVC_EINN_NO,
                         OVC_EDF_NO = einn.OVC_EDF_NO,
                         OVC_INF_NO = einn.OVC_INF_NO,
                         ONB_INS_AMOUNT = einn.ONB_INS_AMOUNT,
                         OVC_COMPANY = company.OVC_COMPANY,
                         OVC_CLASS_NAME = dept.OVC_CLASS_NAME,
                         OVC_PURCH_NO = edf.OVC_PURCH_NO,
                         OVC_IS_PAID = einf.OVC_IS_PAID,
                         ODT_INS_DATE = einn.ODT_INS_DATE
                     }).Union
                     (from iinn in mtse.TBGMT_IINN
                      join bld in mtse.TBGMT_BLD on iinn.OVC_BLD_NO equals bld.OVC_BLD_NO into tbbld
                      from bld in tbbld.DefaultIfEmpty()
                      join iinf in mtse.TBGMT_IINF on iinn.OVC_INF_NO equals iinf.OVC_INF_NO into tbiinf
                      from iinf in tbiinf.DefaultIfEmpty()
                      join dept in mtse.TBGMT_DEPT_CLASS on iinn.OVC_MILITARY_TYPE equals dept.OVC_CLASS into tbdept
                      from dept in tbdept.DefaultIfEmpty()
                      join company in mtse.TBGMT_COMPANY on iinf.CO_SN equals company.CO_SN into tbcompany
                      from company in tbcompany.DefaultIfEmpty()
                      select new
                      {
                          OVC_INN_NO = iinn.OVC_IINN_NO,
                          OVC_EDF_NO = iinn.OVC_BLD_NO,
                          OVC_INF_NO = iinn.OVC_INF_NO,
                          ONB_INS_AMOUNT = iinn.ONB_INS_AMOUNT,
                          OVC_COMPANY = company.OVC_COMPANY,
                          OVC_CLASS_NAME = dept.OVC_CLASS_NAME,
                          OVC_PURCH_NO = iinn.OVC_PURCH_NO,
                          OVC_IS_PAID = iinf.OVC_IS_PAID,
                          ODT_INS_DATE = iinn.ODT_INS_DATE
                      }
                        );
                if (strOvcIsPaid == "未申請")
                    query = query.Where(table => table.OVC_INF_NO == null);
                if (strOvcIsPaid == "未付款")
                    query = query.Where(table => table.OVC_IS_PAID == "未付款");
                if (strOvcIsPaid == "已付款")
                    query = query.Where(table => table.OVC_IS_PAID == "已付款");
                if (strOvcClassName != "不限定")
                    query = query.Where(table => table.OVC_CLASS_NAME == strOvcClassName);
                if (strImportOrExport == "進口")
                    query = query.Where(table => table.OVC_INN_NO.StartsWith("I"));
                if (strImportOrExport == "出口")
                    query = query.Where(table => table.OVC_INN_NO.StartsWith("E"));
                if (!strOvcPurchNo.Equals(string.Empty))
                    query = query.Where(table => table.OVC_PURCH_NO.Contains(strOvcPurchNo));
                if (!strOdtInvDate1.Equals(string.Empty))
                {
                    DateTime d1 = Convert.ToDateTime(strOdtInvDate1);
                    query = query.Where(table => table.ODT_INS_DATE >= d1);
                }
                if (!strOdtInvDate2.Equals(string.Empty))
                {
                    DateTime d2 = Convert.ToDateTime(strOdtInvDate2);
                    query = query.Where(table => table.ODT_INS_DATE <= d2);
                }
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                dt = CommonStatic.LinqQueryToDataTable(query);
                //ViewState["hasRows2"] = FCommon.GridView_dataImport(GV_TBGMT_INSRATE_DETAIL, dt, strFieldDetail);
                ViewState["hasRows2"] = FCommon.GridView_dataImport(GV_TBGMT_INSRATE_DETAIL, dt);
            }
            else FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void GV_TBGMT_INSRATE_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void btnChangeToStatistics_Click(object sender, EventArgs e)
        {
            PanelStatistics.Visible = true;
            PanelDetails.Visible = false;
        }

        protected void btnChangeToDetails_Click(object sender, EventArgs e)
        {
            PanelStatistics.Visible = false;
            PanelDetails.Visible = true;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            datastatistics();
        }

        protected void btnQuery1_Click(object sender, EventArgs e)
        {
            datadetailsapplyed();
        }

        protected void GV_TBGMT_INSRATE_DETAIL_PreRender(object sender, EventArgs e)
        {
            bool hasRows2 = false;
            if (ViewState["hasRows2"] != null)
                hasRows2 = Convert.ToBoolean(ViewState["hasRows2"]);
            FCommon.GridView_PreRenderInit(sender, hasRows2);
        }
    }
}