using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using System.Web.UI;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.GM
{
    public partial class PermissionView : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities GME = new GMEntities();

        #region 副程式
        private void roleChange()
        {
            string strC_SN_ROLE = drpC_SN_ROLE.SelectedValue;
            if (strC_SN_ROLE.Equals(string.Empty)) strC_SN_ROLE = "無";
            //使用者權限
            var queryC_SN_AUTH = GME.TBM1407.Where(table => table.OVC_PHR_CATE == "S6").Where(table => table.OVC_PHR_PARENTS.Equals(strC_SN_ROLE)).ToList();
            DataTable dtC_SN_AUTH = CommonStatic.ListToDataTable(queryC_SN_AUTH);
            FCommon.list_dataImport(drpC_SN_AUTH, dtC_SN_AUTH, "OVC_PHR_DESC", "OVC_PHR_ID", true);
        }

        private void lstbox_dataImport(ListControl list, DataTable dt, string textField, string valueField)
        {   //新增listbox選項
            //先將選單清空
            list.Items.Clear();
            list.DataSource = dt;
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataBind();
        }

        private void dataImport()
        {
            lstAdd.Items.Clear();
            lstDel.Items.Clear();
            lstSetFunction.Items.Clear();
            lstUseFunction.Items.Clear();

            string strMessage = "";
            //string strC_SN_SYS = drpC_SN_SYS.SelectedValue;
            //string strC_SN_ROLE = drpC_SN_ROLE.SelectedValue;
            string strC_SN_AUTH = drpC_SN_AUTH.SelectedValue;
            string strM_System = drpM_System.SelectedValue;
            string strM_Model = drpM_Model.SelectedValue;
            bool isHasModel = drpM_Model.Items.Count > 0;

            //if (strC_SN_SYS.Equals(string.Empty))
            //    strMessage += "<P> 請選擇 權限角色系統別 </p>";
            //if (strC_SN_ROLE.Equals(string.Empty))
            //    strMessage += "<P> 請選擇 使用者角色 </p>";
            if (strC_SN_AUTH.Equals(string.Empty))
                strMessage += "<P> 請選擇 使用者權限 </p>";
            if (strM_System.Equals(string.Empty))
                strMessage += "<P> 請選擇 畫面模組系統別 </p>";
            else if (isHasModel && strM_Model.Equals(string.Empty))
                strMessage += "<P> 請選擇 模組名稱 </p>";

            if (strMessage.Equals(string.Empty))
            {
                //DataTable dtSet = new DataTable();
                //DataRow dsr;
                //dtSet.Columns.Add(new DataColumn("M_NAME", typeof(string)));
                //dtSet.Columns.Add(new DataColumn("M_SN", typeof(string)));
                string strParent = !strM_Model.Equals(string.Empty) ? strM_Model : strM_System;
                //取得模組/系統之所有頁面
                var queryPage =
                     (from table in GME.MENU_PAGES
                      where table.M_SN_PAR.Equals(strParent)
                      select new
                      {
                          M_NAME = table.M_NAME,
                          M_SN = table.M_SN
                      }).ToList();
                //取得該權限角色已設定之頁面
                var queryAuth =
                    (from table in GME.MENU_PAGES
                     join tableauth in GME.AUTH_MENU
                     on table.M_SN equals tableauth.MENU_SN
                     where tableauth.GROUP_SN == strC_SN_AUTH
                     where table.M_SN_PAR.Equals(strParent)
                     select new
                     {
                         M_NAME = table.M_NAME,
                         M_SN = table.M_SN
                     }).ToList();
                //取得可設定之頁面
                var querySet = queryPage.Except(queryAuth);
                ////取得已設定之頁面
                //var queryUse = queryAuth.Concat(querySet);

                DataTable dtSet = CommonStatic.LinqQueryToDataTable(querySet);
                DataTable dtUse = CommonStatic.LinqQueryToDataTable(queryAuth);
                lstbox_dataImport(lstSetFunction, dtSet, "M_NAME", "M_SN");
                lstbox_dataImport(lstUseFunction, dtUse, "M_NAME", "M_SN");

                #region 原始清單－設定功能項
                //if (drpM_System.SelectedValue == "GM")
                //{
                //    var queryGM = GME.MENU_PAGES.Where(table => table.M_SN_PAR.Equals(drpM_System.SelectedValue)).ToList();
                //    DataTable dtGM = CommonStatic.ListToDataTable(queryGM);
                //    lstbox_dataImport(lstSetFunction, dtGM, "M_NAME", "M_SN");
                //}          

                //DataTable dtSet = new DataTable();
                //DataRow dsr;
                //dtSet.Columns.Add(new DataColumn("M_NAME", typeof(string)));
                //dtSet.Columns.Add(new DataColumn("M_SN", typeof(string)));
                //var querySet =
                //    (from table in GME.MENU_PAGES
                //     join tableauth in GME.AUTH_MENU
                //     on table.M_SN equals tableauth.MENU_SN
                //     where tableauth.GROUP_SN == strC_SN_AUTH
                //     select new
                //     {
                //         M_NAME = table.M_NAME,
                //         M_SN = table.M_SN
                //     }).ToList();
                //var querypage =
                //     (from table in GME.MENU_PAGES
                //      where table.M_SN_PAR.Equals(strM_SN_PAR)
                //      select new
                //      {
                //          M_NAME = table.M_NAME,
                //          M_SN = table.M_SN
                //      }).ToList();
                //var exceptResult = querypage.Except(querySet);
                //foreach (var aa in exceptResult)
                //{
                //    dsr = dtSet.NewRow();
                //    dsr[0] = aa.M_NAME;
                //    dsr[1] = aa.M_SN;
                //    dtSet.Rows.Add(dsr);
                //}
                //lstbox_dataImport(lstSetFunction, dtSet, "M_NAME", "M_SN");

                //DataTable dtUse = new DataTable();
                //DataRow dr;
                //dtUse.Columns.Add(new DataColumn("M_NAME", typeof(string)));
                //dtUse.Columns.Add(new DataColumn("M_SN", typeof(string)));

                //var queryUse =
                //    (from table in GME.MENU_PAGES
                //     join tableauth in GME.AUTH_MENU
                //     on table.M_SN equals tableauth.MENU_SN
                //     where tableauth.GROUP_SN == strC_SN_AUTH
                //     select new
                //     {
                //         M_NAME = table.M_NAME,
                //         M_SN = table.M_SN
                //     }).ToList();
                //foreach(var aa in queryUse)
                //{
                //    dr = dtUse.NewRow();
                //    dr[0] = aa.M_NAME;
                //    dr[1] = aa.M_SN;
                //    dtUse.Rows.Add(dr);
                //}
                //lstbox_dataImport(lstUseFunction, dtUse, "M_NAME", "M_SN");

                ////if (drpM_System.SelectedValue == "GM")
                ////{
                ////    var queryGM = GME.MENU_PAGES.Where(table => table.M_SN_PAR.Equals(drpM_System.SelectedValue)).ToList();
                ////    DataTable dtGM = CommonStatic.ListToDataTable(queryGM);
                ////    lstbox_dataImport(lstSetFunction, dtGM, "M_NAME", "M_SN");
                ////}          
                #endregion
            }
            else
                FCommon.AlertShow(PnMessage_Permission, "danger", "系統訊息", strMessage);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region 下拉式選單匯入資料
                //權限角色系統別
                DataTable dtC_SN_SYS = CommonStatic.ListToDataTable(GME.TBM1407.Where(table => table.OVC_PHR_CATE == "SA").ToList());
                FCommon.list_dataImport(drpC_SN_SYS, dtC_SN_SYS, "OVC_PHR_DESC", "OVC_PHR_ID", true);
                //使用者角色
                FCommon.list_Init(drpC_SN_ROLE, true);
                //使用者權限
                FCommon.list_Init(drpC_SN_AUTH, true);

                //畫面模組系統別
                DataTable dtM_SN_PAR = CommonStatic.LinqQueryToDataTable(GME.MENU_PAGES.Where(table => table.M_SN_PAR == "SYSTEM").ToList());
                FCommon.list_dataImport(drpM_System, dtM_SN_PAR, "M_NAME", "M_SN", true);
                //模組名稱
                FCommon.list_Init(drpM_Model, false);
                pnModel.Visible = false;
                #endregion
            }
        }

        protected void drpC_SN_SYS_SelectedIndexChanged(object sender, EventArgs e)
        {
            //使用者角色
            var queryC_SN_ROLE = GME.TBM1407.Where(table => table.OVC_PHR_CATE == "S5").Where(table => table.OVC_PHR_PARENTS.Equals(drpC_SN_SYS.SelectedValue)).ToList();
            DataTable dtC_SN_ROLE = CommonStatic.ListToDataTable(queryC_SN_ROLE);
            FCommon.list_dataImport(drpC_SN_ROLE, dtC_SN_ROLE, "OVC_PHR_DESC", "OVC_PHR_ID", true);

            roleChange();
        }
        protected void drpC_SN_ROLE_SelectedIndexChanged(object sender, EventArgs e)
        {
            roleChange();
        }

        protected void drpM_System_SelectedIndexChanged(object sender, EventArgs e)
        {
            //模組名稱
            var queryM_Model = GME.MENU_PAGES.Where(table => table.M_SN_PAR.Equals(drpM_System.SelectedValue) && table.M_URL == null).ToList();
            bool isHasData = queryM_Model.Any();
            if (isHasData)
            {
                DataTable dtM_Model = CommonStatic.ListToDataTable(queryM_Model);
                FCommon.list_dataImport(drpM_Model, dtM_Model, "M_NAME", "M_SN", true);
            }
            else
                FCommon.list_Init(drpM_Model, false);
            pnModel.Visible = isHasData;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }

        protected void lstSetFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem selectedItem = lstSetFunction.SelectedItem;
            selectedItem.Selected = false;

            lstUseFunction.Items.Add(selectedItem);
            lstSetFunction.Items.Remove(selectedItem);

            //增加
            if (lstDel.Items.Contains(selectedItem)) //刪除清單中有項目的話，則清除清單中的項目
                lstDel.Items.Remove(selectedItem);
            else
                lstAdd.Items.Add(selectedItem); //加入新增清單中的項目
        }

        protected void lstUseFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem selectedItem = lstUseFunction.SelectedItem;
            selectedItem.Selected = false;

            lstSetFunction.Items.Add(selectedItem);
            lstUseFunction.Items.Remove(selectedItem);

            //刪除
            if (lstAdd.Items.Contains(selectedItem)) //新增清單中有項目的話，則清除清單中的項目
                lstAdd.Items.Remove(selectedItem);
            else
                lstDel.Items.Add(selectedItem); //加入刪除清單中的項目
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strC_SN_AUTH = drpC_SN_AUTH.SelectedValue;
            //刪除項目
            var query = GME.AUTH_MENU.Where(id => id.GROUP_SN.Equals(strC_SN_AUTH)).ToList();
            foreach (ListItem theItem in lstDel.Items)
            {
                string strDel = theItem.Value;
                AUTH_MENU authMenu = query.Where(table => table.MENU_SN.Equals(strDel)).FirstOrDefault();
                if (authMenu != null)
                {
                    GME.AUTH_MENU.Remove(authMenu);
                }
            }

            foreach (ListItem theItem in lstAdd.Items)
            {
                string strAdd = theItem.Value;

                AUTH_MENU authMenu = new AUTH_MENU();
                authMenu.AM_SN = Guid.NewGuid();
                authMenu.GROUP_SN = strC_SN_AUTH;
                authMenu.MENU_SN = strAdd;
                GME.AUTH_MENU.Add(authMenu);
                GME.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), authMenu.GetType().Name.ToString(), this, "新增");
            }

            dataImport();
            FCommon.AlertShow(PnMessage_Permission, "success", "系統訊息", "修改成功");
        }
    }
}