using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using FCFDFE.Entity.MTSModel;

namespace FCFDFE.pages.GM
{
    public partial class Deptadd : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities GME = new GMEntities();
        MTSEntities MTSE = new MTSEntities();

        #region 副程式
        private void SaveData()
        {
            string strMessage = "";
            string strOVC_DEPT_CDE = txtOVC_DEPT_CDE.Text;
            string strOVC_ONNAME = txtOVC_ONNAME.Text;
            string strOVC_DEPT_FAX = txtOVC_DEPT_FAX.Text;
            string strOVC_DEPT_MAILADDR = txtOVC_DEPT_MAILADDR.Text;
            string strOVC_DEPT_DOC_NAME = txtOVC_DEPT_DOC_NAME.Text;
            string strOVC_MANAGER = txtOVC_MANAGER.Text;
            string strOVC_PURCHASE_DEPT = drpOVC_PURCHASE_DEPT.SelectedValue;
            string strOVC_MANAGER_TITLE = txtOVC_MANAGER_TITLE.Text;
            string strOVC_EMAILADDRESS = txtOVC_EMAILADDRESS.Text;
            string strOVC_DEPT_SEC_NAME = txtOVC_DEPT_SEC_NAME.Text;
            string strOVC_TOP_DEPT = txtOVC_TOP_DEPT.Text;
            string strOVC_CLASS = drpOVC_CLASS.SelectedValue;
            string strOVC_PURCHASE_OK = drpOVC_PURCHASE_OK.SelectedValue;
            string strOVC_DEPT_PHONE = txtOVC_DEPT_PHONE.Text;
            string strOVC_ENABLE = drpOVC_ENABLE.SelectedValue;
            string strOVC_CLASS2 =  drpOVC_CLASS2.SelectedValue;
            string strOVC_CLASS2_TOP = txtOVC_TOP_DEPT2.Text;

            TBMDEPT dept = GME.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(strOVC_DEPT_CDE)).FirstOrDefault();
            #region 錯誤訊息
            if (string.IsNullOrEmpty(strOVC_DEPT_CDE))
                strMessage += "<p> 請輸入 單位代碼！ </p>";
            else if (dept != null)
                strMessage += "<p> 單位代碼 已存在！ </p>";
            if (string.IsNullOrEmpty(strOVC_ONNAME))
                strMessage += "<p> 請輸入 單位名稱！ </p>";
            #endregion
            if (string.IsNullOrEmpty(strMessage))
            {
                try
                {
                    dept = new TBMDEPT();
                    dept.OVC_DEPT_CDE = strOVC_DEPT_CDE;
                    dept.OVC_ONNAME = strOVC_ONNAME;
                    dept.OVC_DEPT_FAX = strOVC_DEPT_FAX;
                    dept.OVC_DEPT_MAILADDR = strOVC_DEPT_MAILADDR;
                    dept.OVC_DEPT_DOC_NAME = strOVC_DEPT_DOC_NAME;
                    dept.OVC_MANAGER = strOVC_MANAGER;
                    dept.OVC_PURCHASE_DEPT = strOVC_PURCHASE_DEPT;
                    dept.OVC_MANAGER_TITLE = strOVC_MANAGER_TITLE;
                    dept.OVC_EMAILADDRESS = strOVC_EMAILADDRESS;
                    dept.OVC_DEPT_SEC_NAME = strOVC_DEPT_SEC_NAME;
                    dept.OVC_TOP_DEPT = strOVC_TOP_DEPT;
                    dept.OVC_CLASS = strOVC_CLASS;
                    dept.OVC_PURCHASE_OK = strOVC_PURCHASE_OK;
                    dept.OVC_DEPT_PHONE = strOVC_DEPT_PHONE;
                    dept.OVC_ENABLE = strOVC_ENABLE;
                    dept.OVC_CLASS2 = strOVC_CLASS2;
                    dept.OVC_TOP_DEPT2 = strOVC_CLASS2_TOP;
                    GME.TBMDEPTs.Add(dept);
                    GME.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), dept.GetType().Name.ToString(), this, "新增");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", $"單位代碼：{ strOVC_DEPT_CDE } 之單位檔資料 新增成功。");
                }
                catch
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "單位 新增失敗，請聯絡工程師！");
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_DEPT_CDE", Request.QueryString["OVC_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_ONNAME", Request.QueryString["OVC_ONNAME"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_ENABLE", Request.QueryString["OVC_ENABLE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_CLASS", Request.QueryString["OVC_CLASS"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_PURCHASE_DEPT", Request.QueryString["OVC_PURCHASE_DEPT"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_CLASS2", Request.QueryString["OVC_CLASS2"], false);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    #region 匯入下拉式選單
                    var queryPUR_KIND = GME.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("S9"));
                    DataTable dtPUR_KIND = CommonStatic.LinqQueryToDataTable(queryPUR_KIND);
                    FCommon.list_dataImportV(drpOVC_CLASS, dtPUR_KIND, "OVC_PHR_DESC", "OVC_PHR_ID", "全部", "", "：", true);

                    var queryAllDept = GME.TBMDEPTs.Where(table => table.OVC_PURCHASE_OK.Equals("Y"));
                    DataTable dtAllDept = CommonStatic.LinqQueryToDataTable(queryAllDept);
                    FCommon.list_dataImportV(drpOVC_PURCHASE_DEPT, dtAllDept, "OVC_ONNAME", "OVC_DEPT_CDE", "：", true);

                    var queryCLASS = MTSE.TBGMT_DEPT_CLASS.OrderBy(table => table.ONB_SORT);
                    DataTable dtCLASS = CommonStatic.LinqQueryToDataTable(queryCLASS);
                    FCommon.list_dataImportV(drpOVC_CLASS2, dtCLASS, "OVC_CLASS_NAME", "OVC_CLASS", "全部", "", "：", true);
#endregion
                }
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            SaveData();
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"DeptQuery{ getQueryString() }");
        }
    }
}