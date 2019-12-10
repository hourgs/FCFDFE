using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F18_1 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();


        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "ODT_START_DATE", Request.QueryString["ODT_START_DATE"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_END_DATE", Request.QueryString["ODT_END_DATE"], false);
            FCommon.setQueryString(ref strQueryString, "OdtApplyDate", Request.QueryString["OdtApplyDate"], false);
            return strQueryString;
            //在接收頁面加入此副程式
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            }
        }
        
        #region 按鈕部分
        protected void btnNew_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strOVC_WAY = txtOVC_WAY.Text;
            string strONB_SORT = txtONB_SORT.Text;
            decimal decONB_SORT;
            FCommon.checkDecimal(strONB_SORT, "排序", ref strMessage, out decONB_SORT);
            int if_exist_sort = MTSE.TBGMT_CLEARANCE.Where(table => table.ONB_SORT == decONB_SORT).Count();

            if (strOVC_WAY.Equals(string.Empty)) strMessage += "<p>請輸入 清運方式！</p>";
            if (strONB_SORT.Equals(string.Empty)) strMessage += "<p>請輸入 排序！</p>";
            if (if_exist_sort > 0) strMessage += "<p>排序 已重複！</p>";

            if (strMessage.Equals(string.Empty))
            {
                TBGMT_CLEARANCE clean = new TBGMT_CLEARANCE();
                clean.CL_SN = Guid.NewGuid();
                clean.OVC_WAY = strOVC_WAY;
                clean.ONB_SORT = decONB_SORT;
                clean.ODT_CREATE_DATE = DateTime.Now;
                clean.OVC_CREATE_ID = Session["userid"].ToString();
                MTSE.TBGMT_CLEARANCE.Add(clean);
                MTSE.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), clean.GetType().Name.ToString(), this, "新增");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F18{ getQueryString() }");
        }
        #endregion
    }
}