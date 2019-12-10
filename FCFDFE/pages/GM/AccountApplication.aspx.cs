using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using System.Web.UI;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.GM
{
    public partial class AccountApplication : Page
    {
        Common FCommon = new Common();
        GMEntities GME = new GMEntities();

        #region 副程式
        private void dataImport_Auth()
        {
            string strUSER_ID = ViewState["USER_ID"] != null ? ViewState["USER_ID"].ToString() : "";
            if (strUSER_ID.Length > 0)
            {
                DateTime dateStart = DateTime.Today;
                var query =
                    from accountAuth in GME.ACCOUNT_AUTH.DefaultIfEmpty().AsEnumerable()
                    where accountAuth.USER_ID.Equals(strUSER_ID)
                    where accountAuth.END_DATE == null || accountAuth.END_DATE >= dateStart

                    join tableSys in GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("SA")) on accountAuth.C_SN_SYS equals tableSys.OVC_PHR_ID
                    join tableRole in GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("S5")) on accountAuth.C_SN_ROLE equals tableRole.OVC_PHR_ID
                    join tableAuth in GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("S6")) on accountAuth.C_SN_AUTH equals tableAuth.OVC_PHR_ID into tempAuth
                    from tableAuth in tempAuth.DefaultIfEmpty()
                    join tableSub in GME.TBM1407.Where(t => t.OVC_PHR_CATE.Equals("K5")) on accountAuth.C_SN_SUB equals tableSub.OVC_PHR_ID into tempSub
                    from tableSub in tempSub.DefaultIfEmpty()
                    select new
                    {
                        AA_SN = accountAuth.AA_SN,
                        C_SN_SYS = tableSys.OVC_PHR_DESC,
                        C_SN_ROLE = tableRole.OVC_PHR_DESC,
                        C_SN_AUTH = tableAuth != null ? tableAuth.OVC_PHR_DESC : "",
                        C_SN_SUB = tableSub != null ? tableSub.OVC_PHR_DESC : "",
                        IS_UPLOAD = accountAuth.IS_UPLOAD.Equals("Y") ? "是" : "否",
                        IS_ENABLE = accountAuth.IS_ENABLE.Equals("Y") ? "是" : "否",
                        IS_PRO = accountAuth.IS_PRO == 1 ? "已處理" : "尚未處理"
                    };

                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_ACCOUNT_AUTH, dt);
            }
            else
                FCommon.AlertShow(PnMessage_Account, "danger", "系統訊息", "請先新增使用者！");
        }

        private void roleChange()
        {
            string strC_SN_ROLE = drpC_SN_ROLE.SelectedValue;
            strC_SN_ROLE = strC_SN_ROLE.Equals(string.Empty) ? "無" : strC_SN_ROLE;
            //使用者權限
            var queryC_SN_AUTH = GME.TBM1407.Where(table => table.OVC_PHR_CATE == "S6").Where(table => table.OVC_PHR_PARENTS.Equals(strC_SN_ROLE));
            DataTable dtC_SN_AUTH = CommonStatic.LinqQueryToDataTable(queryC_SN_AUTH);
            FCommon.list_dataImport(drpC_SN_AUTH, dtC_SN_AUTH, "OVC_PHR_DESC", "OVC_PHR_ID", true);
        }
        private void subChange()
        {
            string strC_SN_AUTH = drpC_SN_AUTH.SelectedValue;
            strC_SN_AUTH = strC_SN_AUTH.Equals(string.Empty) ? "無" : strC_SN_AUTH;
            //隸屬單位
            bool isShow = strC_SN_AUTH.IndexOf("35") > -1;
            if (isShow)
            {
                var queryC_SN_SUB = GME.TBM1407.Where(table => table.OVC_PHR_CATE == "K5").Where(table => table.OVC_USR_ID != null);
                DataTable dtC_SN_SUB = CommonStatic.LinqQueryToDataTable(queryC_SN_SUB);
                FCommon.list_dataImport(drpC_SN_SUB, dtC_SN_SUB, "OVC_PHR_DESC", "OVC_PHR_ID", true);
            }
            else
                FCommon.list_Init(drpC_SN_SUB, true);
            tdTitleC_SN_SUB.Visible = isShow;
            tdC_SN_SUB.Visible = isShow;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //頁面第一次加載時要執行的事件，當不是第一次加載時不執行此事件
            if (!IsPostBack)
            {
                FCommon.Controls_Attributes("readonly", "true", MainContent_txtOVC_DEPT_CDE, MainContent_txtOVC_ONNAME);
                #region 下拉式選單匯入資料
                //系統別
                DataTable dtC_SN_SYS = CommonStatic.LinqQueryToDataTable(GME.TBM1407.Where(table => table.OVC_PHR_CATE == "SA"));
                FCommon.list_dataImport(drpC_SN_SYS, dtC_SN_SYS, "OVC_PHR_DESC", "OVC_PHR_ID", true);
                //使用者角色
                FCommon.list_Init(drpC_SN_ROLE, true);
                //使用者權限
                //FCommon.list_Init(drpC_SN_AUTH, true);
                roleChange();
                //隸屬單位
                //FCommon.list_Init(drpC_SN_SUB, true);
                //tdTitleC_SN_SUB.Visible = false;
                //tdC_SN_SUB.Visible = false;
                subChange();
                //上傳功能
                FCommon.list_dataImportYN(drpIS_UPLOAD, false, false);
                #endregion
                //dataImport_Auth();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUSER_ID = txtUSER_ID.Text;
            string strPWD = txtPWD.Text;
            string strPWDConfirm = txtPWDConfirm.Text;
            string strUSER_NAME = txtUSER_NAME.Text;
            string strDEPT_SN = MainContent_txtOVC_DEPT_CDE.Text;
            string strIUSER_PHONE = txtIUSER_PHONE.Text;
            string strEMAIL_ACCOUNT = txtEMAIL_ACCOUNT.Text;
            string strPURCHASE_1 = txtPURCHASE_1.Text;

            #region 錯誤訊息
            if (strUSER_ID.Equals(string.Empty))
                strMessage += "<P> 請輸入 使用者帳號 </p>";
            else if (!FCommon.CheckPersonalID(strUSER_ID))
                strMessage += "<P> 使用者帳號 須為身份證字號 </p>";
            else
            {
                ACCOUNT account = GME.ACCOUNTs.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                if (account != null) strMessage += "<P> 使用者帳號 已存在 </p>";
            }
            if (!strPWD.Equals(strPWDConfirm))
                strMessage += "<P> 密碼設定 與 再次輸入密碼 不符 </p>";
            else if (strPWD.Length < 8)
                strMessage += "<P> 密碼 長度限制為至少8碼 </p>";
            else if (strPWD.Length > 20)
                strMessage += "<P> 密碼 長度限制為不能超過20碼 </p>";
            if (strUSER_NAME.Equals(string.Empty))
                strMessage += "<P> 請輸入 使用者姓名 </p>";
            if (strDEPT_SN.Equals(string.Empty))
                strMessage += "<P> 請帶入 單位代碼 </p>";
            if (strIUSER_PHONE.Length != 6)
                strMessage += "<P> 電話(軍線) 長度為6碼 </p>";
            if (strEMAIL_ACCOUNT.Equals(string.Empty))
                strMessage += "<P> 請輸入 E-mail </p>";
            if (strPURCHASE_1.Equals(string.Empty))
                strMessage += "<P> 請輸入 採購購案編號第一組代字 </p>";
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                ACCOUNT account = new ACCOUNT();
                account.AC_SN = Guid.NewGuid();
                account.AC_ID = strUSER_ID;
                account.USER_ID = strUSER_ID;
                account.PWD = strPWD;
                account.USER_NAME = strUSER_NAME;
                account.DEPT_SN = strDEPT_SN;
                account.IUSER_PHONE = strIUSER_PHONE;
                account.CHANGE_DATE = FCommon.getDateTime(DateTime.Now);
                account.EMAIL_ACCOUNT = strEMAIL_ACCOUNT;
                account.PURCHASE_1 = strPURCHASE_1;
                //account.LAST_LOGIN = DateTime.Now;
                account.ACCOUNT_STATUS = 1;
                account.ERROR_CNT = 0;
                GME.ACCOUNTs.Add(account);
                GME.SaveChanges();
                FCommon.syslog_add(strUSER_ID, Request.ServerVariables["REMOTE_ADDR"].ToString(), account.GetType().Name.ToString(), this, "新增");

                ViewState["USER_ID"] = strUSER_ID;
                dataImport_Auth();
                pnAuth.Visible = true;
                pnApply.Visible = true;
                FCommon.AlertShow(PnMessage_Account, "success", "系統訊息", "申請成功，請申請系統使用權限");
                FCommon.Controls_Attributes("readonly", "true", txtUSER_ID);
                floatmenu.InnerText = "確認成功";

                #region 全域系統一般人員權限申請
                ACCOUNT_AUTH auth_new = new ACCOUNT_AUTH
                {
                    USER_ID = strUSER_ID,
                    C_SN_ROLE = "21",
                    IS_ENABLE = "N",
                    IS_UPLOAD = "N",
                    IS_PRO = 0,
                    AA_SN = Guid.NewGuid(),
                    C_SN_SYS = "GM",
                    C_SN_AUTH = "2102"
                };
                GME.ACCOUNT_AUTH.Add(auth_new);
                GME.SaveChanges();
                dataImport_Auth();
                #endregion
            }
            else
                FCommon.AlertShow(PnMessage_Account, "danger", "系統訊息", strMessage);
        }
        protected void btnAuth_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUSER_ID = txtUSER_ID.Text;
            string strC_SN_SYS = drpC_SN_SYS.SelectedValue;
            string strC_SN_ROLE = drpC_SN_ROLE.SelectedValue;
            string strC_SN_AUTH = drpC_SN_AUTH.SelectedValue;
            string strC_SN_SUB = drpC_SN_SUB.SelectedValue;
            string strIS_UPLOAD = drpIS_UPLOAD.SelectedValue;
            string strIS_ENABLE = "N"; //預設開放使用為 不開放
            int intIS_PRO = 0; //預設處理狀態為 未處理

            #region 錯誤訊息
            if (strUSER_ID.Equals(string.Empty))
                strMessage += "<P> 請先申請 帳號 </p>";
            if (strC_SN_SYS.Equals(string.Empty))
                strMessage += "<P> 請選擇 系統別 </p>";
            if (strC_SN_ROLE.Equals(string.Empty))
                strMessage += "<P> 請選擇 使用者角色 </p>";
            if (strC_SN_AUTH.Equals(string.Empty))
                strMessage += "<P> 請選擇 使用者權限 </p>";
            if (tdC_SN_SUB.Visible && strC_SN_SUB.Equals(string.Empty))
                strMessage += "<P> 請選擇 隸屬單位 </p>";
            ACCOUNT_AUTH accountAuth = GME.ACCOUNT_AUTH
                .Where(table => table.USER_ID.Equals(strUSER_ID) && table.C_SN_AUTH.Equals(strC_SN_AUTH) && table.C_SN_SUB.Equals(strC_SN_SUB)).FirstOrDefault();
            if (accountAuth != null)
                strMessage += "<P> 此權限已申請過 </p>";
            #endregion

            //舊系統連審規則:計畫評核(購案審查綜辦)=>03 採購發包=>04 履約驗結=>05
            if (strC_SN_AUTH.IndexOf("32") > -1)
                strC_SN_SUB = "03";
            else if (strC_SN_AUTH.IndexOf("33") > -1)
                strC_SN_SUB = "04";
            else if (strC_SN_AUTH.IndexOf("34") > -1)
                strC_SN_SUB = "05";

            if (strMessage.Equals(string.Empty))
            {
                accountAuth = new ACCOUNT_AUTH();
                accountAuth.AA_SN = Guid.NewGuid();
                accountAuth.USER_ID = strUSER_ID;
                accountAuth.C_SN_SYS = strC_SN_SYS;
                accountAuth.C_SN_ROLE = strC_SN_ROLE;
                accountAuth.C_SN_AUTH = strC_SN_AUTH;
                accountAuth.C_SN_SUB = strC_SN_SUB;
                accountAuth.IS_UPLOAD = strIS_UPLOAD;
                accountAuth.IS_ENABLE = strIS_ENABLE;
                accountAuth.IS_PRO = intIS_PRO;
                GME.ACCOUNT_AUTH.Add(accountAuth);
                GME.SaveChanges();
                FCommon.syslog_add(strUSER_ID, Request.ServerVariables["REMOTE_ADDR"].ToString(), accountAuth.GetType().Name.ToString(), this, "新增");
                dataImport_Auth();
                FCommon.AlertShow(PnMessage_AccountAuth, "success", "系統訊息", "申請成功，請等候審核");
                floatmenu.InnerText = "權限申請成功";
            }
            else
                FCommon.AlertShow(PnMessage_AccountAuth, "danger", "系統訊息", strMessage);
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            FCommon.Controls_Clear(txtUSER_NAME, txtIUSER_PHONE, MainContent_txtOVC_DEPT_CDE, MainContent_txtOVC_ONNAME, txtPURCHASE_1, txtEMAIL_ACCOUNT, txtPWD, txtPWDConfirm);
            floatmenu.InnerText = "清除成功";
        }

        protected void drpC_SN_SYS_SelectedIndexChanged(object sender, EventArgs e)
        {
            //使用者角色
            var queryC_SN_ROLE = GME.TBM1407.Where(table => table.OVC_PHR_CATE == "S5").Where(table => table.OVC_PHR_PARENTS.Equals(drpC_SN_SYS.SelectedValue));
            DataTable dtC_SN_ROLE = CommonStatic.LinqQueryToDataTable(queryC_SN_ROLE);
            FCommon.list_dataImport(drpC_SN_ROLE, dtC_SN_ROLE, "OVC_PHR_DESC", "OVC_PHR_ID", true);

            roleChange();
            subChange();
        }
        protected void drpC_SN_ROLE_SelectedIndexChanged(object sender, EventArgs e)
        {
            roleChange();
            subChange();
        }
        protected void drpC_SN_AUTH_SelectedIndexChanged(object sender, EventArgs e)
        {
            subChange();
        }

        protected void GV_ACCOUNT_AUTH_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //try
            //{
            //    string strC_SN_SUB = e.Row.Cells[3].Text;

            //    if (e.Row.RowType == DataControlRowType.DataRow)
            //    {
            //        if (!strC_SN_SUB.Equals(string.Empty))
            //        {
            //            var query =
            //                from tableSub in GME.TBM1407
            //                where tableSub.OVC_PHR_CATE.Equals("K5")
            //                where tableSub.OVC_PHR_ID.Equals(strC_SN_SUB)
            //                select new { tableSub.OVC_PHR_DESC };
            //            var queryList = query.ToList();
            //            if (queryList.Count > 0) e.Row.Cells[3].Text = queryList.First().OVC_PHR_DESC.ToString();
            //        }
            //    }
            //}
            //catch { }
        }
        protected void GV_ACCOUNT_AUTH_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}