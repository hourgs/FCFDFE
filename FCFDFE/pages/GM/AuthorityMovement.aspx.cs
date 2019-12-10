using FCFDFE.Content;
using System;
using System.Data;
using System.Web.UI;
using System.Linq;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.GM
{
    public partial class AuthorityMovement : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities GME = new GMEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FCommon.Controls_Attributes("readonly", "true", txtOVC_DEPT_CDE, txtOVC_ONNAME, txtEND_DATE, txtACCOUNT_Name_ORI, txtACCOUNT_Name); //新增唯讀屬性
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = string.Empty;
            string strUserId = Session["userid"].ToString();
            string strACCOUNT_ID = txtACCOUNT_ID.Text;
            string strACCOUNT_ID_ORI = txtACCOUNT_ID_ORI.Text;
            DateTime dateStart = DateTime.Today;//, dateEnd = DateTime.Today;
            string strDEPT_ID = txtOVC_DEPT_CDE.Text;
            string strEND_DATE = txtEND_DATE.Text;

            #region 錯誤訊息
            if (strACCOUNT_ID_ORI.Equals(string.Empty))
                strMessage += "<P> 請選擇 原有權限人員 </p>";
            if (strACCOUNT_ID.Equals(string.Empty))
                strMessage += "<P> 請選擇 新設權限人員 </p>";
            if (!strACCOUNT_ID_ORI.Equals(string.Empty) && !strACCOUNT_ID.Equals(string.Empty) && strACCOUNT_ID_ORI.Equals(strACCOUNT_ID))
                strMessage += "<P> 原有權限人員 與 新設權限人員 不可重複 </p>";
            bool boolEND_DATE = FCommon.checkDateTime(strEND_DATE, "結束日期", ref strMessage, out DateTime dateEnd);
            if (boolEND_DATE && DateTime.Compare(dateStart, dateEnd) > 0)
                strMessage += "<P> 結束日期 不正確 </p>";
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                var query = GME.ACCOUNT_AUTH
                    .Where(table => table.USER_ID.Equals(strACCOUNT_ID_ORI))    //判斷使用者
                    .Where(table => table.END_DATE == null || table.END_DATE >= dateStart);    //判斷使用者
                if (query.Any())
                {
                    foreach (ACCOUNT_AUTH accountAuth in query)
                    {
                        if (boolEND_DATE) accountAuth.END_DATE = dateEnd; else accountAuth.END_DATE = null;
                        accountAuth.USER_ID = strACCOUNT_ID;
                        GME.SaveChanges();
                        FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), accountAuth.GetType().Name.ToString(), this, "修改");
                    }

                    ACCOUNT_SHIFT accountShift = new ACCOUNT_SHIFT();
                    accountShift.AS_SN = Guid.NewGuid();
                    accountShift.AACOUNT_ID = strACCOUNT_ID;
                    accountShift.AACOUNT_ID_ORI = strACCOUNT_ID_ORI;
                    accountShift.DEPT_ID = strDEPT_ID;
                    accountShift.START_SATE = dateStart;
                    if (boolEND_DATE) accountShift.END_DATE = dateEnd;
                    GME.ACCOUNT_SHIFT.Add(accountShift);
                    GME.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), accountShift.GetType().Name.ToString(), this, "新增");

                    FCommon.AlertShow(PnMessage_Shift, "success", "系統訊息", "轉移與複製成功");
                }
                else
                    FCommon.AlertShow(PnMessage_Shift, "danger", "系統訊息", "原有權限人員 無有效權限");
            }
            else
                FCommon.AlertShow(PnMessage_Shift, "danger", "系統訊息", strMessage);
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string strDEPT_SN = txtOVC_DEPT_CDE.Text;
            var queryUser = from account in GME.ACCOUNTs select account;
            if (!strDEPT_SN.Equals(string.Empty))
                queryUser = queryUser.Where(table => table.DEPT_SN.Equals(strDEPT_SN));
            DataTable dt = CommonStatic.LinqQueryToDataTable(queryUser);
            FCommon.list_dataImport(lstACCOUNT_ID_ORI, dt, "USER_NAME", "USER_ID", false);
            FCommon.list_dataImport(lstACCOUNT_ID, dt, "USER_NAME", "USER_ID", false);
        }
        //protected void btnClear_Click(object sender, EventArgs e)
        //{
        //    txtEND_DATE.Text = "";  //清空日期
        //}

        protected void lstACCOUNT_ID_ORI_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtACCOUNT_ID_ORI.Text = lstACCOUNT_ID_ORI.SelectedValue;
            txtACCOUNT_Name_ORI.Text = lstACCOUNT_ID_ORI.SelectedItem != null ? lstACCOUNT_ID_ORI.SelectedItem.Text : "";
        }
        protected void lstACCOUNT_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtACCOUNT_ID.Text = lstACCOUNT_ID.SelectedValue;
            txtACCOUNT_Name.Text = lstACCOUNT_ID.SelectedItem != null ? lstACCOUNT_ID.SelectedItem.Text : "";
        }
    }
}