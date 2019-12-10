using System;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace FCFDFE.pages.MPMS.B
{
    public partial class MPMS_B16 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    ListYearImport(drpOVC_BUDGET_YEAR);
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            GridViewImport();
        }
      
        protected void GV_Content_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GV_Content.UseAccessibleHeader = true;
                GV_Content.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private void ListYearImport(ListControl list)
        {
            //帶入計畫年度下拉選單的值
            //先將下拉式選單清空
            list.Items.Clear();
            TaiwanCalendar twC = new TaiwanCalendar();
            int num = Convert.ToInt32(twC.GetYear(DateTime.Now));
            for (int i = 0; i < 15; i++)
            {
                list.Items.Add(Convert.ToString(num));
                num = num - 1;
            }
        }

       
        private void GridViewImport()
        {
            string strOVC_BUDGET_YEAR = drpOVC_BUDGET_YEAR.SelectedItem.ToString();
            string Compare = strOVC_BUDGET_YEAR.Substring((strOVC_BUDGET_YEAR.Length - 2), 2);

            string[] strField = { "OVC_PURCH", "OVC_PUR_IPURCH", "OVC_PUR_NSECTION", "OVC_PUR_DCANPO"
                                    , "OVC_PUR_DCANRE", "ONB_CHECK_TIMES", "OVC_ASSIGNER","OVC_STATUS_ID","OVC_STATUS" };

            var query1407Q9 = gm.TBM1407.Where(o => o.OVC_PHR_CATE.Equals("Q9"));

            var queryStatus =
                        from tbSTATUS in mpms.TBMSTATUS.AsEnumerable()
                        join tb1407_Q9 in query1407Q9 on tbSTATUS.OVC_STATUS equals tb1407_Q9.OVC_PHR_ID into tbGroup2
                        from tb1407_Q9 in tbGroup2.DefaultIfEmpty()
                        select new
                        {
                            OVC_PURCH = tbSTATUS.OVC_PURCH,
                            OVC_STATUS_ID = tbSTATUS.OVC_STATUS,
                            OVC_STATUS_Name = tb1407_Q9 != null ? tb1407_Q9.OVC_PHR_DESC : tbSTATUS.OVC_STATUS,
                        };
            var query =
                from tb1301 in gm.TBM1301.AsEnumerable()
                join tbSTATUS in queryStatus on tb1301.OVC_PURCH equals tbSTATUS.OVC_PURCH into tbGroup1
                from tbSTATUS in tbGroup1.OrderByDescending(tb => tb.OVC_STATUS_ID.Length).ThenByDescending(tb => tb.OVC_STATUS_ID).Take(1).DefaultIfEmpty()
                join tb1202 in mpms.TBM1202 on tb1301.OVC_PURCH equals tb1202.OVC_PURCH into tbGroup2
                from tb1202 in tbGroup2.OrderByDescending(tb => tb.ONB_CHECK_TIMES).Take(1)
                join tb1202_2 in mpms.TBM1202.Where(tb => tb.OVC_CHECK_OK.Equals("N")) on tb1301.OVC_PURCH equals tb1202_2.OVC_PURCH into tbGroup3
                from tb1202_2 in tbGroup3.OrderByDescending(tb => tb.OVC_DRECEIVE).Take(1)
                where tb1301.OVC_PURCH.Substring(2, 2).Equals(Compare) && tb1301.OVC_PUR_DCANPO != null
                select new
                {
                    OVC_PURCH = tb1301.OVC_PURCH,
                    OVC_PUR_IPURCH = tb1301.OVC_PUR_IPURCH,
                    OVC_PUR_NSECTION = tb1301.OVC_PUR_NSECTION,
                    OVC_PUR_DCANPO = tb1301.OVC_PUR_DCANPO,
                    OVC_PUR_DCANRE = tb1301.OVC_PUR_DCANRE,
                    //ONB_CHECK_TIMES = ONB_CHECK_TIMES 最大值
                    ONB_CHECK_TIMES = tb1202 != null ? tb1202.ONB_CHECK_TIMES : (int?)null,
                    //OVC_ASSIGNER = OVC_CHECK_OK ="N" & OVC_DRECEIVE 最大日期
                    OVC_ASSIGNER = tb1202_2 != null ? tb1202_2.OVC_ASSIGNER : string.Empty,
                    //OVC_STATUS = OVC_STATUS 最大值
                    OVC_STATUS = tbSTATUS != null ? tbSTATUS.OVC_STATUS_Name : string.Empty,
                    //OVC_STATUS_ID = tbSTATUS != null ? tbSTATUS.OVC_STATUS_ID : string.Empty
                };
            DataTable dt = new DataTable();
            dt = CommonStatic.LinqQueryToDataTable(query);
            

            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_Content, dt, strField);
        }
    }
}