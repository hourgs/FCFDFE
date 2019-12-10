using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MPMS.B
{
    public partial class MPMS_B12 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strToPurch = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!string.IsNullOrEmpty(Request.QueryString["strToPurch"]))
                {
                    strToPurch = Request.QueryString["strToPurch"].ToString();
                }
                if (!IsPostBack)
                {
                    txtOVC_AGNT_IN.Visible = false;
                    LoginScreen();
                }
            }

        }

        protected void GV_OVC_BUDGET_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            //上方搜尋之按鈕功能
            string strOVC_LAB = "";
            string strOVC_PUR_ASS_VEN_CODE = "";
            string strOVC_BID_TIMES = "";
            string strOVC_BID = "";
            string strOVC_ITEM = "";
            string strOVC_AGNT_IN = "";
            string strOVC_PURCH = "";
            string strOVC_PUR_IPURCH = "";
            
            var queryTBM1407_GN =
                from table1407 in gm.TBM1407 where table1407.OVC_PHR_CATE.Equals("GN") select table1407;
            var queryTBM1407_C7 =
                from table1407 in gm.TBM1407 where table1407.OVC_PHR_CATE.Equals("C7") select table1407;
            var queryTBM1407_TG =
                from table1407 in gm.TBM1407 where table1407.OVC_PHR_CATE.Equals("TG") select table1407;
            var queryTBM1407_M3 =
               from table1407 in gm.TBM1407 where table1407.OVC_PHR_CATE.Equals("M3") select table1407;
            var queryTBM1407_GP =
               from table1407 in gm.TBM1407 where table1407.OVC_PHR_CATE.Equals("GP") select table1407;
            var queryTBM1407_GK =
               from table1407 in gm.TBM1407 where table1407.OVC_PHR_CATE.Equals("GK") select table1407;
            var query =
                from table1301 in gm.TBM1301
                join plan1301 in gm.TBM1301_PLAN.AsEnumerable() on table1301.OVC_PURCH equals plan1301.OVC_PURCH
                join table1407_GN in queryTBM1407_GN on table1301.OVC_LAB equals table1407_GN.OVC_PHR_ID into tempGN
                from table1407_GN in tempGN.DefaultIfEmpty()
                join table1407_C7 in queryTBM1407_C7 on table1301.OVC_PUR_ASS_VEN_CODE equals table1407_C7.OVC_PHR_ID into tempC7
                from table1407_C7 in tempC7.DefaultIfEmpty()
                join table1407_TG in queryTBM1407_TG on table1301.OVC_BID_TIMES equals table1407_TG.OVC_PHR_ID into tempTG
                from table1407_TG in tempTG.DefaultIfEmpty()
                join table1407_M3 in queryTBM1407_M3 on table1301.OVC_BID equals table1407_M3.OVC_PHR_ID into tempM3
                from table1407_M3 in tempM3.DefaultIfEmpty()
                join table1407_GP in queryTBM1407_GP on table1301.OVC_PURCH_KIND equals table1407_GP.OVC_PHR_ID into tempGP
                from table1407_GP in tempGP.DefaultIfEmpty()
                join table1407_GK in queryTBM1407_GK on table1301.OVC_BID_TIMES equals table1407_GK.OVC_PHR_ID into tempGK
                from table1407_GK in tempGK.DefaultIfEmpty()
                select new
                {
                    OVC_PURCH = table1301.OVC_PURCH,
                    OVC_PUR_IPURCH = table1301.OVC_PUR_IPURCH,
                    OVC_LAB = table1407_GN.OVC_PHR_DESC,
                    OVC_PUR_ASS_VEN_CODE = table1407_C7.OVC_PHR_DESC, 
                    OVC_BID_TIMES = table1407_TG.OVC_PHR_DESC,
                    OVC_BID = table1407_M3.OVC_PHR_DESC,
                    OVC_ITEM = table1407_GP.OVC_PHR_DESC,
                    OVC_AGNT_IN = table1301.OVC_AGNT_IN,

                    CheckAll = table1301.OVC_EXAMPLE_SUPPORT,
                    DeptCode = table1301.OVC_PUR_SECTION,

                    OVC_PHR_ID_GN = table1407_GN.OVC_PHR_ID,
                    OVC_PHR_ID_C7 = table1407_C7.OVC_PHR_ID,
                    OVC_PHR_ID_TG = table1407_TG.OVC_PHR_ID,
                    OVC_PHR_ID_M3 = table1407_M3.OVC_PHR_ID,
                    OVC_PHR_ID_GP = table1407_GP.OVC_PHR_ID,
                    OVC_PHR_ID_GK = table1407_GK.OVC_PHR_ID
                };
            //顯示所有可複製之購案範例
            //跟登入者同單位，值為Y NULL都可以
            if (rdoCheckAll.SelectedValue == "1")
            {
                string userdept = "";
                if (Session["userid"] != null)
                {
                    string strUSER_ID = Session["userid"].ToString();
                    ACCOUNT acCheck = new ACCOUNT();
                    acCheck = gm.ACCOUNTs.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                    if (acCheck != null)
                    {
                        userdept = acCheck.DEPT_SN;
                    }
                }
                query = query.Where(table => table.DeptCode.Equals(userdept)).Where(table => table.CheckAll != "N");
            }
            //採購屬性
            if (drpOVC_LAB.SelectedValue != "0")
            {
                strOVC_LAB = drpOVC_LAB.SelectedValue.ToString();
                query = query.Where(table => table.OVC_PHR_ID_GN.Equals(strOVC_LAB));
            }
            //招標方式
            if (drpOVC_PUR_ASS_VEN_CODE.SelectedValue != "0")
            {
                strOVC_PUR_ASS_VEN_CODE = drpOVC_PUR_ASS_VEN_CODE.SelectedValue.ToString();
                query = query.Where(table => table.OVC_PHR_ID_C7.Equals(strOVC_PUR_ASS_VEN_CODE));
            }
            //投標段次
            if (drpOVC_BID_TIMES.SelectedValue != "0")
            {
                strOVC_BID_TIMES = drpOVC_BID_TIMES.SelectedValue.ToString();
                query = query.Where(table => table.OVC_PHR_ID_TG.Equals(strOVC_BID_TIMES));
            }
            //決標原則
            if (drpOVC_BID.SelectedValue != "0")
            {
                strOVC_BID = drpOVC_BID.SelectedValue.ToString();
                query = query.Where(table => table.OVC_PHR_ID_M3.Equals(strOVC_BID));
            }
            //採購途徑
            if (drpOVC_ITEM.SelectedValue != "0")
            {
                strOVC_ITEM = drpOVC_ITEM.SelectedValue.ToString();
                query = query.Where(table => table.OVC_PHR_ID_GP.Equals(strOVC_ITEM));
            }
            //招標單位
            if (drpOVC_AGNT_IN.SelectedValue != "0")
            {
                if (drpOVC_AGNT_IN.SelectedValue == "01" || drpOVC_AGNT_IN.SelectedValue == "05"){
                    strOVC_AGNT_IN = txtOVC_AGNT_IN.Text;
                    query = query.Where(table => table.OVC_AGNT_IN.Equals(strOVC_AGNT_IN));
                }
                else {
                    strOVC_AGNT_IN = drpOVC_AGNT_IN.SelectedItem.Text;
                    query = query.Where(table => table.OVC_AGNT_IN.Equals(strOVC_AGNT_IN));
                }
            }
            //購案編號
            if (!txtOVC_PURCH.Text.Equals(string.Empty)) {
                strOVC_PURCH = txtOVC_PURCH.Text;
                query = query.Where(table => table.OVC_PURCH.Equals(strOVC_PURCH));
            }
            //購案名稱
            if (!txtOVC_PUR_IPURCH.Text.Equals(string.Empty))
            {
                strOVC_PUR_IPURCH = txtOVC_PUR_IPURCH.Text;
                query = query.Where(table => table.OVC_PUR_IPURCH.Equals(strOVC_PUR_IPURCH));
            }
            //授權開放的其他單位
            if (rdoOtherAuth.SelectedValue == "1"){
                query = query.Where(table => table.CheckAll.Equals("Y"));
            }
            else {
                string userdept = "";
                if (Session["userid"] != null)
                {
                    string strUSER_ID = Session["userid"].ToString();
                    ACCOUNT acCheck = new ACCOUNT();
                    acCheck = gm.ACCOUNTs.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                    if (acCheck != null)
                    {
                        userdept = acCheck.DEPT_SN;
                    }
                }
                query = query.Where(table => table.DeptCode.Equals(userdept)).Where(table => table.CheckAll.Equals("Y"));
            }
            DataTable tablequery = CommonStatic.LinqQueryToDataTable(query);
            string[] strField = { "OVC_PURCH", "OVC_PUR_IPURCH", "OVC_LAB", "OVC_PUR_ASS_VEN_CODE", "OVC_BID_TIMES", "OVC_BID", "OVC_ITEM", "OVC_AGNT_IN" };
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_OVC_BUDGET, tablequery, strField);

        }
        protected void drpOVC_AGNT_IN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpOVC_AGNT_IN.SelectedValue == "01" || drpOVC_AGNT_IN.SelectedValue == "05")
            {
                txtOVC_AGNT_IN.Visible = true;
            }
            else
            {
                txtOVC_AGNT_IN.Visible = false;
            }
        }

        protected void GV_OVC_BUDGET_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_OVC_BUDGET.DataKeys[gvrIndex].Value.ToString(); //OVC_PURCH

            switch (e.CommandName)
            {
                case "DataCopy":
                    string str_url_Copy;
                    str_url_Copy = "MPMS_B12_1.aspx?OVC_PURCH=" + id + "&strToPurch=" + strToPurch;
                    Response.Redirect(str_url_Copy);
                    break;
                default:
                    break;
            }
        }

        #region 副程式
        private void LoginScreen() {
            //匯入各下拉選單資料
            var query = gm.TBM1407;
            string textFirst = "請選擇-可空白";
            string valueFirst = "0";
            //採購屬性
            DataTable dtOVC_LAB = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "GN").ToList());
            FCommon.list_dataImportV(drpOVC_LAB, dtOVC_LAB, "OVC_PHR_DESC", "OVC_PHR_ID", textFirst, valueFirst, ":", true);
            drpOVC_LAB.SelectedValue = "0";
            //招標方式
            DataTable dtOVC_PUR_ASS_VEN_CODE = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "C7").ToList());
            FCommon.list_dataImportV(drpOVC_PUR_ASS_VEN_CODE, dtOVC_PUR_ASS_VEN_CODE, "OVC_PHR_DESC", "OVC_PHR_ID", textFirst, valueFirst, ":", true);
            drpOVC_PUR_ASS_VEN_CODE.SelectedValue = "0";
            //投標段次
            DataTable dtOVC_BID_TIMES = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "TG").ToList());
            FCommon.list_dataImportV(drpOVC_BID_TIMES, dtOVC_BID_TIMES, "OVC_PHR_DESC", "OVC_PHR_ID", textFirst, valueFirst, ":", true);
            drpOVC_BID_TIMES.SelectedValue = "0";
            //決標原則
            DataTable dtOVC_BID = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "M3").ToList());
            FCommon.list_dataImportV(drpOVC_BID, dtOVC_BID, "OVC_PHR_DESC", "OVC_PHR_ID", textFirst, valueFirst, ":", true);
            drpOVC_BID.SelectedValue = "0";
            //採購途徑
            DataTable dtOVC_ITEM = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "GP").ToList());
            FCommon.list_dataImportV(drpOVC_ITEM, dtOVC_ITEM, "OVC_PHR_DESC", "OVC_PHR_ID", textFirst, valueFirst, ":", true);
            drpOVC_ITEM.SelectedValue = "0";
            //招標單位
            DataTable dtOVC_AGNT_IN = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "GK").ToList());
            FCommon.list_dataImportV(drpOVC_AGNT_IN, dtOVC_AGNT_IN, "OVC_PHR_DESC", "OVC_PHR_ID", textFirst, valueFirst, ":", true);
            drpOVC_AGNT_IN.SelectedValue = "0";
            //顯示所有可複制之購案範例
            rdoCheckAll.SelectedValue = "2";//跟登入同單位 且 除了N之外 Y或NULL都可
            //授權開放的其它單位
            rdoOtherAuth.SelectedValue = "2";//所有單位 只要是Y都可以
        }
        #endregion
    }
}