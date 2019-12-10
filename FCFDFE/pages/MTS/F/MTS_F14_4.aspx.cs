using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F14_4 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_PORT_TYPE", Request.QueryString["OVC_PORT_TYPE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_ROUTE", Request.QueryString["OVC_ROUTE"], false);
            return strQueryString;
            //在接收頁面加入此副程式
        }

        private void dataImport()
        {
            lstSetFunction.Items.Clear();
            lstAdd.Items.Clear();
            lstDel.Items.Clear();
            lstUseFunction.Items.Clear();

            string strMessage = "";
            string strOVC_ROUTE = drpOVC_ROUTE.SelectedValue;
            string strPortChiName = txtPortChiName.Text;
            ViewState["OVC_ROUTE"] = strOVC_ROUTE;

            if (strPortChiName.Equals(string.Empty))
            {
                chkPort.Checked = true;
            }
            if (drpOVC_ROUTE.SelectedIndex == 0)
            {
                strMessage += "<p> 請選擇 航線！ </p>";
            }

            if (strMessage.Equals(string.Empty))
            {
                IQueryable<TBGMT_PORTS> query_all = MTSE.TBGMT_PORTS;
                if (!chkPort.Checked == true)
                {
                    query_all = query_all.Where(t => t.OVC_PORT_CHI_NAME.Contains(strPortChiName));
                }
                DataTable dt_all = CommonStatic.LinqQueryToDataTable(query_all);
                lstbox_dataImport(lstSetFunction, dt_all, "OVC_PORT_CHI_NAME", "OVC_PORT_CDE");

                var query = MTSE.TBGMT_PORTS.Where(t => t.OVC_ROUTE.Equals(strOVC_ROUTE));
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                lstbox_dataImport(lstUseFunction, dt, "OVC_PORT_CHI_NAME", "OVC_PORT_CDE");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (!IsPostBack)
                {
                    CommonMTS.list_dataImport_ROUTE(drpOVC_ROUTE, true);
                }
            }
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F14_1{ getQueryString() }");
        }

        protected void lstSetFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem selectedItem = lstSetFunction.SelectedItem;
            selectedItem.Selected = false;

            if (!lstUseFunction.Items.Contains(selectedItem))
                lstUseFunction.Items.Add(selectedItem);

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

            lstUseFunction.Items.Remove(selectedItem);

            //刪除
            if (lstAdd.Items.Contains(selectedItem)) //新增清單中有項目的話，則清除清單中的項目
                lstAdd.Items.Remove(selectedItem);
            else
                lstDel.Items.Add(selectedItem); //加入刪除清單中的項目
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            dataImport();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ViewState["OVC_ROUTE"] != null)
            {
                string strOVC_ROUTE = ViewState["OVC_ROUTE"].ToString();

                foreach (ListItem add in lstAdd.Items)
                {
                    string strAdd = add.Value;
                    TBGMT_PORTS port = MTSE.TBGMT_PORTS.Where(t => t.OVC_PORT_CDE.Equals(strAdd)).FirstOrDefault();
                    port.OVC_ROUTE = strOVC_ROUTE;
                }

                foreach (ListItem del in lstDel.Items)
                {
                    string strDel = del.Value;
                    TBGMT_PORTS port = MTSE.TBGMT_PORTS.Where(t => t.OVC_PORT_CDE.Equals(strDel)).FirstOrDefault();
                    port.OVC_ROUTE = null;
                }

                MTSE.SaveChanges();
                dataImport();
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請選擇 航線！");
        }
    }
}