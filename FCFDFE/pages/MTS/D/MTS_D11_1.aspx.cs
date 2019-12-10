using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;


namespace FCFDFE.pages.MTS.D
{
    public partial class MTS_D11_1 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        private MTSEntities mtse = new MTSEntities();

        #region 副程式
        public void list_dataImport(ListControl list)
        {
            //先將下拉式選單清空
            list.Items.Clear();

            int CalDateYear = Convert.ToInt16(DateTime.Now.ToString("yyyy")) - 1911;
            int num = CalDateYear;
            for (int i = num; i > 92; i--)
            {
                list.Items.Add(Convert.ToString(i));
            }
        }
        public void list_dataImport2(ListControl list)
        {
            //先將下拉式選單清空
            list.Items.Clear();
            list.Items.Add("未限定");
            list.Items.Add("未付款");
            list.Items.Add("已付款");
        }
        public void list_dataImport3(ListControl list, DataTable dt, string textField, string valueField)
        {
            //先將下拉式選單清空
            list.Items.Clear();
            list.AppendDataBoundItems = true;
            list.Items.Add("未限定");
            list.DataSource = dt;
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataBind();
        }

        private void dateImport()
        {
            //這邊是查詢按鈕執行的function
            DataTable dt = new DataTable();
            string strODT_APPLY_DATE = drpOdtApplyDate.SelectedItem.ToString();
            string strOVC_INF_NO = txtOvcInfNo.Text;
            string strOVC_IS_PAID = drpOvcIsPaid.SelectedItem.ToString();
            string strOVC_CLASS_NAME = drpOvcClassName.SelectedItem.ToString();
            //判斷上方你有哪些存取字串的變數
            // ViewState["變數名稱"] = 變數字串; 在下方以此類推，全部都要照這樣做
            ViewState["ODT_APPLY_DATE"] = strODT_APPLY_DATE;
            ViewState["OVC_INF_NO"] = strOVC_INF_NO;
            ViewState["OVC_IS_PAID"] = strOVC_IS_PAID;
            ViewState["OVC_CLASS_NAME"] = strOVC_CLASS_NAME;
            //紀錄查詢數值


            if (strODT_APPLY_DATE.Length != 3)
            {
                if (strODT_APPLY_DATE.Length == 2)
                    strODT_APPLY_DATE = "0" + strODT_APPLY_DATE;
                else if (strODT_APPLY_DATE.Length == 1)
                    strODT_APPLY_DATE = "00" + strODT_APPLY_DATE;
                else
                    strODT_APPLY_DATE = "000";
            }

            var query = from iinf in mtse.TBGMT_IINF
                        select iinf;

            if (!strODT_APPLY_DATE.Equals(string.Empty))
                query = query.Where(table => table.OVC_INF_NO.StartsWith("IINF" + strODT_APPLY_DATE));
            if (!strOVC_INF_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_INF_NO.Contains(strOVC_INF_NO));
            if (!strOVC_IS_PAID.Equals("未限定"))
                query = query.Where(table => table.OVC_IS_PAID.Equals(strOVC_IS_PAID));
            if (!strOVC_CLASS_NAME.Equals("未限定"))
                query = query.Where(table => table.OVC_GIST.Contains(strOVC_CLASS_NAME));

            if (query.Count() > 1000)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
            query = query.Take(1000);
            dt = CommonStatic.LinqQueryToDataTable(query);
            //ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_IINF, dt, strField);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_IINF, dt);
        }
        //在下方副程式的位置新增函數 getQueryString() 請統一以此命名
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "ODT_APPLY_DATE", ViewState["ODT_APPLY_DATE"], true);
            //public void setQueryString(ref string strQuerySring, string strVariable, object objValue, bool isEncryption)
            //最後一個true 代表的是是否加密
            FCommon.setQueryString(ref strQueryString, "OVC_INF_NO", ViewState["OVC_INF_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_IS_PAID", ViewState["OVC_IS_PAID"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_CLASS_NAME", ViewState["OVC_CLASS_NAME"], true);
            return strQueryString;
            //紀錄查詢數值
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    list_dataImport(drpOdtApplyDate);
                    list_dataImport2(drpOvcIsPaid);
                    DataTable dt = CommonStatic.ListToDataTable(mtse.TBGMT_DEPT_CLASS.Select(x => x).ToList());
                    list_dataImport3(drpOvcClassName, dt, "OVC_CLASS_NAME", "OVC_CLASS");
                    
                    //以下在帶入變數至控制項，並做查詢動作，要在下拉式選單 選項資料載入後面
                    bool boolImport = false;
                    string strODT_APPLY_DATE, strOVC_INF_NO, strOVC_IS_PAID, strOVC_CLASS_NAME;
                    if (FCommon.getQueryString(this, "ODT_APPLY_DATE", out strODT_APPLY_DATE, true))
                    {
                        FCommon.list_setValue(drpOdtApplyDate, strODT_APPLY_DATE);
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_INF_NO", out strOVC_INF_NO, true))
                        txtOvcInfNo.Text = strOVC_INF_NO;
                    if (FCommon.getQueryString(this, "OVC_IS_PAID", out strOVC_IS_PAID, true))
                        FCommon.list_setValue(drpOvcIsPaid, strOVC_IS_PAID);
                    if (FCommon.getQueryString(this, "OVC_CLASS_NAME", out strOVC_CLASS_NAME, true))
                        FCommon.list_setValue(drpOvcClassName, strOVC_CLASS_NAME);
                    if (boolImport)
                        dateImport();
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dateImport();
        }
        
        protected void GV_TBGMT_IINF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBGMT_IINF.DataKeys[gvrIndex].Value.ToString();

            //新增欲編輯之資料編號ID，在getQueryString()之外的資料
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);

            switch (e.CommandName)
            {
                case "btnModify":
                    Response.Redirect($"MTS_D11_2{ strQueryString }");
                    //跳轉頁面修改，請先判斷你那頁面是怎麼跳轉
                    break;
            }
        }
        protected void GV_TBGMT_IINF_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}