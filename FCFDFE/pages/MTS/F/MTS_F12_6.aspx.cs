using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F12_6 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTS = new MTSEntities();
        string  strOVC_COMPANY = "";

        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_CO_TYPE", Request.QueryString["OVC_CO_TYPE"], false);
            return strQueryString;
            //在接收頁面加入此副程式
        }
        #endregion 
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strOvcCompany = txtOvcCompany.Text;
            string strContract = drpContract.SelectedItem.Text;
            string strTransport = drpTransport.SelectedItem.Text;
            string strOnbCoSort = txtOnbCoSort.Text;
            string strOVC_CREATE_ID = Session["username"].ToString();

            if (strOvcCompany.Equals(string.Empty))
                strMessage += "<p> 請輸入 廠商名稱！ </p>";
            if (strOnbCoSort.Equals(string.Empty))
                strMessage += "<p> 請輸入 排序！ </p>";
            decimal decOnbCoSort;
            bool boolOnbCoSort = FCommon.checkDecimal(strOnbCoSort, "排序", ref strMessage, out decOnbCoSort);

            #region 排序、名稱重複判斷
            var query =
                from com in MTS.TBGMT_COMPANY
                where com.OVC_CO_TYPE.Equals("3")
                select new
                {
                    ONB_CO_SORT = com.ONB_CO_SORT,
                    OVC_COMPANY = com.OVC_COMPANY,
                };
            var q_name = query.Where(ot => ot.OVC_COMPANY.Equals(strOvcCompany));
            var q_sort = query.Where(ot => ot.ONB_CO_SORT == decOnbCoSort);

            if (q_name.Any())
            {
                strMessage += "<p> 廠商名稱不可重複 </p>";
            }
            if (q_sort.Any())
            {
                strMessage += "<p> 廠商種類排序不可重複 </p>";
            }
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                try
                {
                    TBGMT_COMPANY company = new TBGMT_COMPANY();
                    company.CO_SN = Guid.NewGuid();
                    company.OVC_COMPANY = strOvcCompany;
                    company.ODT_CREATE_DATE = DateTime.Now;
                    company.OVC_CREATE_ID = strOVC_CREATE_ID;
                    company.OVC_CO_TYPE = "3";
                    company.ONB_CO_SORT = decOnbCoSort;
                    company.OVC_REMARK_1 = strContract;
                    company.OVC_REMARK_2 = strTransport;
                    MTS.TBGMT_COMPANY.Add(company);
                    MTS.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), company.GetType().Name.ToString(), this, "新增進艙廠商資料成功");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "更新成功");
                }
                catch
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F12_1{ getQueryString() }");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                //判斷是進艙廠商還是保險公司
                if (!IsPostBack)
                {
                    lblOVC_COMPANY_title.Text = "航運廠商";
                }
            }
        }
    }
}