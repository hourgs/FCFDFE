using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;
using System.Data;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F15_3 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string mess_isrepeat = "";
            string mess_isnull = "";
            string strMessage = "";
            string strCurrencyCode = txtCurrencyCode.Text;
            string strCurrencyName = txtCurrencyName.Text;
            string strOvcType = drpOvcType.SelectedValue;
            string strOvcStatus = drpOvcType.SelectedValue;
            string strOnbSort = txtOnbSort.Text;
            decimal decOnbSort;
            bool boolsort = FCommon.checkDecimal(strOnbSort, "排序", ref strMessage, out decOnbSort);
            int ifexist_code = MTSE.TBGMT_CURRENCY.Where(table => table.OVC_CURRENCY_CODE.Equals(strCurrencyCode)).Count();
            int ifexist_name = MTSE.TBGMT_CURRENCY.Where(table => table.OVC_CURRENCY_NAME.Equals(strCurrencyName)).Count();
            int ifexist_sort = MTSE.TBGMT_CURRENCY.Where(table => table.ONB_SORT == decOnbSort).Count();

            if (strCurrencyCode.Equals(string.Empty))
            {
                strMessage += "請輸入幣別代碼！";
            }
            if (strCurrencyName.Equals(string.Empty))
            {
                strMessage += "請輸入幣別名稱！";
            }
            if (strOvcType.Equals(string.Empty))
            {
                strMessage += "請選擇貨幣別！";
            }
            if (strOvcStatus.Equals(string.Empty))
            {
                strMessage += "請選擇幣別狀態！";
            }
            if (strOnbSort.Equals(string.Empty))
            {
                strMessage += "請輸入排序！";
            }
            if (ifexist_code > 0)
            {
                strMessage += "幣別代碼已重複！";
            }
            if (ifexist_name > 0)
            {
                strMessage += "幣別名稱已重複！";
            }
            if (ifexist_sort > 0)
            {
                strMessage += "排序已重複！";
            }
            if (strMessage.Equals(string.Empty))
            {
                TBGMT_CURRENCY c = new TBGMT_CURRENCY();
                c.OVC_CURRENCY_CODE = strCurrencyCode;
                c.OVC_CURRENCY_NAME = strCurrencyName;
                c.ONB_SORT = decOnbSort;
                c.OVC_TYPE = drpOvcType.SelectedValue;
                c.CURRENCY_SN = Guid.NewGuid();
                // 產生新的唯一key
                c.OVC_STATUS = drpOvcStatus.SelectedValue;

                MTSE.TBGMT_CURRENCY.Add(c);
                MTSE.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), c.GetType().Name.ToString(), this, "新增");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增貨幣資料成功！");
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
        }
        

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("MTS_F15.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            }
        }
    }
}