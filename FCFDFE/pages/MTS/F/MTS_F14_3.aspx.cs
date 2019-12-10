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
    public partial class MTS_F14_3 : System.Web.UI.Page
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
        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string mess_isrepeat = "";
            string mess_isnull = "";

            string strMessage = "";
            string strPortCdeSn = txtPortCdeSn.Text;
            string strOvcPortChiName = txtOvcPortChiName.Text;
            string strOvcPortEngName = txtOvcPortEngName.Text;
            string strOVC_ROUTE = txtOVC_ROUTE.Text;

            if (strPortCdeSn.Equals(string.Empty))
            {
                strMessage += "<p> 請輸入 代碼！ </p>";
            }
            if (strOvcPortChiName.Equals(string.Empty))
            {
                strMessage += "<p> 請輸入 中文名稱！ </p>";
            }
            if (strOvcPortEngName.Equals(string.Empty))
            {
                strMessage += "<p> 請輸入 英文名稱！ </p>";
            }
            if (strOVC_ROUTE.Equals(string.Empty))
            {
                strMessage += "<p> 請輸入 航線！ </p>";
            }

            if (strMessage.Equals(string.Empty))
            {
                var query_cde = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE == txtPortCdeSn.Text);
                var query_chi = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CHI_NAME == txtOvcPortChiName.Text);
                var query_eng = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_ENG_NAME == txtOvcPortEngName.Text);
                if (!query_cde.Any() && !query_chi.Any() && !query_eng.Any())
                {
                    try
                    {
                        TBGMT_PORTS port = new TBGMT_PORTS();
                        port.OVC_PORT_CDE = txtPortCdeSn.Text;
                        port.OVC_PORT_CHI_NAME = txtOvcPortChiName.Text;
                        port.OVC_PORT_ENG_NAME = txtOvcPortEngName.Text;
                        port.OVC_PORT_TYPE = drpOvcPortType.SelectedValue;
                        port.OVC_IS_ABROAD = drpOvcIsAbroad.SelectedValue;
                        MTSE.TBGMT_PORTS.Add(port);
                        MTSE.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), port.GetType().Name.ToString(), this, "新增");
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增機場港口資料成功！");
                    }
                    catch (Exception ex)
                    {
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "新增機場港口資料失敗！");
                    }
                }
                else
                {
                    if (query_cde.Any())
                    {
                        mess_isrepeat += "<p> 代碼已重複！</p>";
                    }
                    if (query_chi.Any())
                    {
                        mess_isrepeat += "<p> 中文名稱已重複！</p>";
                    }
                    if (query_eng.Any())
                    {
                        mess_isrepeat += "<p> 英文名稱已重複！</p>";
                    }
                    if (mess_isrepeat != "")
                    {
                        FCommon.AlertShow(PnMessage, "warning", "系統訊息", mess_isrepeat);
                    }
                }
            }
            else
            {
                if (txtPortCdeSn.Text == "")
                {
                    mess_isnull += "請輸入代碼！<br>";
                }
                if (txtOvcPortChiName.Text == "")
                {
                    mess_isnull += "請輸入中文名稱！<br>";
                }
                if (txtOvcPortEngName.Text == "")
                {
                    mess_isnull += "請輸入英文名稱！<br>";
                }
                if (mess_isnull != "")
                {
                    FCommon.AlertShow(PnMessage, "warning", "系統訊息", mess_isnull);
                }
            }
        }


        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F14_1{ getQueryString() }");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {

            }
        }
    }
}