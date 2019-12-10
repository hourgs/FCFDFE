using System;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Entity.GMModel;
using System.Linq;

namespace FCFDFE.pages.MPMS.C
{
    public partial class MPMS_C13_1 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        string strPurchNum = "";
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (Request.QueryString["PurchNum"] != null)
                {
                    strPurchNum = Request.QueryString["PurchNum"].ToString();
                    if (!IsPostBack)
                    {
                        var query1118_2 = mpms.TBM1118_2.Where(o => o.OVC_PURCH.Equals(strPurchNum));
                        if (query1118_2.Any())
                        {
                            lblOVC_PURCH.Text = strPurchNum;
                            DrpGroupImport();
                            GroupDataImport(1);
                            lbldrpselect.Text = "1";
                            GroupItem();
                            ChangeText(IsAduit());
                        }
                        else
                        {
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "尚未執行預算分組!");
                        }
                    }
                }
            }
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            string data = IsAduit();
            string userName = "";
            string userdept = "";
            if (Session["userid"] != null)
            {
                string strUSER_ID = Session["userid"].ToString();
                
                var ac = gm.ACCOUNTs.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                if (ac != null)
                {
                    userName = ac.USER_NAME;
                    userdept = ac.DEPT_SN;
                }
            }
            string date = DateTime.Now.ToString("yyyy-MM-dd");

            //NoData表示新增
            if (data.Equals("NoData"))
            {
                TBM1118_3 tbm11183 = new TBM1118_3();
                tbm11183.OVC_PURCH = strPurchNum;
                tbm11183.OVC_CHECK_UNIT = userdept;
                tbm11183.OVC_CHECKER = userName;
                tbm11183.OVC_AUDIT_DCHECK = date;
                mpms.TBM1118_3.Add(tbm11183);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , tbm11183.GetType().Name.ToString(), this, "修改");
            }
            else if (data.Equals("HasDataRow"))//表示修改
            {
                TBM1118_3 tbm11183 = new TBM1118_3();
                tbm11183 = mpms.TBM1118_3.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                tbm11183.OVC_AUDIT_DCHECK = date;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , tbm11183.GetType().Name.ToString(), this, "修改");
            }
            else
            {
                //有資料，要拿掉
                TBM1118_3 tbm11183 = new TBM1118_3();
                tbm11183 = mpms.TBM1118_3.Where(o => o.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                tbm11183.OVC_AUDIT_DCHECK = "";
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , tbm11183.GetType().Name.ToString(), this, "修改");
            }
            ChangeText(IsAduit());
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_C13.aspx");
        }

        protected void btnBackMain_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_C13.aspx");
        }


        protected void drpGroupNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            string item = drpGroupNum.SelectedItem.Text;
            lbldrpselect.Text = item;
            GroupDataImport(short.Parse(item));
        }

        #region 副程式
        private string IsAduit()
        {
            //檢查TBM1118_3是否有資料
            var query11183 = mpms.TBM1118_3.Where(o => o.OVC_PURCH.Equals(strPurchNum));
            if (query11183.Any())
            {
                var item = query11183.FirstOrDefault();
                if (!string.IsNullOrEmpty(item.OVC_AUDIT_DCHECK))
                {
                    return item.OVC_AUDIT_DCHECK;
                }
                else
                {
                    return "HasDataRow";
                }
            }
            else
            {
                return "NoData";
            }
        }

        private void ChangeText(string strDate)
        {
            DateTime date;
            if (DateTime.TryParse(strDate, out date))
            {
                lblICheck.Text = "本案已查核";
                lblCheckDate.Text = "(" + strDate + ")";
                btnCheck.Text = "取消查核";
            }
            else
            {
                lblICheck.Text = "本案尚未查核";
                lblCheckDate.Text = "";
                btnCheck.Text = "確認查核無誤";
            }
        }
        
        private void GroupDataImport(short numGroup)
        {
            DataTable tableGroup = new DataTable();
            DataTable tableLeft = new DataTable();
            DataTable tableRight = new DataTable();

            var query =
                from table in mpms.TBM1118_2
                join table2 in mpms.TBM1201 on new { table.OVC_PURCH, table.ONB_POI_ICOUNT }
                equals new { table2.OVC_PURCH, table2.ONB_POI_ICOUNT }
                where table.OVC_PURCH.Equals(strPurchNum)
                        && table.ONB_GROUP_PRE == numGroup
                orderby table.ONB_POI_ICOUNT
                select new
                {
                    ONB_POI_ICOUNT = table.ONB_POI_ICOUNT,
                    OVC_POI_NSTUFF_CHN = table2.OVC_POI_NSTUFF_CHN,
                    OVC_BRAND = table2.OVC_BRAND
                };
            string[] strField = { "ONB_POI_ICOUNT", "OVC_POI_NSTUFF_CHN", "OVC_BRAND" };
            tableGroup = CommonStatic.LinqQueryToDataTable(query);
            tableLeft = tableGroup.Clone();
            tableRight = tableGroup.Clone();
            int flag = 1;
            foreach (DataRow rows in tableGroup.Rows)
            {
                if (flag == 1)
                {
                    tableLeft.ImportRow(rows);
                    flag = 2;
                }
                else
                {
                    tableRight.ImportRow(rows);
                    flag = 1;
                }

            }
           
            FCommon.GridView_dataImport(gvGroupLeft, tableLeft, strField);
            FCommon.GridView_dataImport(gvGroupRight, tableRight, strField);

        }

        private void GroupItem()
        {
            DataTable dtMain = new DataTable();
            DataTable dtLeft = new DataTable();
            DataTable dtRight = new DataTable();
            string[] strField = { "ONB_POI_ICOUNT", "ONB_GROUP_PRE", "OVC_POI_NSTUFF_CHN", "OVC_BRAND" };
            var query =
                from c in mpms.TBM1201
                join o in mpms.TBM1118_2 on new { c.OVC_PURCH, c.ONB_POI_ICOUNT } equals new { o.OVC_PURCH, o.ONB_POI_ICOUNT }
                into g
                where c.OVC_PURCH.Equals(strPurchNum)
                from o in g.DefaultIfEmpty()
                orderby c.ONB_POI_ICOUNT
                select new
                {
                    ONB_POI_ICOUNT = c.ONB_POI_ICOUNT,
                    ONB_GROUP_PRE = (int?)o.ONB_GROUP_PRE,
                    OVC_POI_NSTUFF_CHN = c.OVC_POI_NSTUFF_CHN,
                    OVC_BRAND = c.OVC_BRAND
                };
            dtMain = CommonStatic.LinqQueryToDataTable(query);
            dtLeft = dtMain.Clone();
            dtRight = dtMain.Clone();
            int flag = 1;
            foreach (DataRow rows in dtMain.Rows)
            {
                if (flag == 1)
                {
                    dtLeft.ImportRow(rows);
                    flag = 2;
                }
                else
                {
                    dtRight.ImportRow(rows);
                    flag = 1;
                }
            }
            FCommon.GridView_dataImport(gvONB_POI_ICOUNT_LEFT, dtLeft, strField);
            FCommon.GridView_dataImport(gvONB_POI_ICOUNT_Right, dtRight, strField);

        }

       
        private void DrpGroupImport()
        {
            //下拉式選單
            drpGroupNum.Items.Clear();
            var max = mpms.TBM1118_2.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Max(o => o.ONB_GROUP_PRE);
            lblGroupNum.Text = max.ToString();
            for (int i = 1; i <= max; i++)
            {
                drpGroupNum.Items.Add(i.ToString());
            }
        }
        #endregion
    }
}