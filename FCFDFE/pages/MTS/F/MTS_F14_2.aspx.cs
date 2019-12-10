using System;
using System.Linq;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System.Data;
using System.Data.Entity;
using System.Web.UI;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F14_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        string id;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getQueryString(this, "id", out id, true);
                if (!IsPostBack)
                {
                    var query = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(id)).FirstOrDefault();
                    if (query != null)
                    {
                        lblPortCdeSn.Text = query.OVC_PORT_CDE.ToString();
                        txtOvcPortChiName.Text = query.OVC_PORT_CHI_NAME.ToString();
                        txtOvcPortEngName.Text = query.OVC_PORT_ENG_NAME.ToString();

                        if (query.OVC_PORT_TYPE.ToString() == "海港")
                        {
                            drpOvcPortType.Items[0].Selected = true;
                        }
                        else
                        {
                            drpOvcPortType.Items[1].Selected = true;
                        }
                        for (int i = 0; i < drpOvcIsAbroad.Items.Count; i++)
                        {
                            if (query.OVC_IS_ABROAD.ToString() == drpOvcIsAbroad.Items[i].ToString())
                            {
                                drpOvcIsAbroad.Items[i].Selected = true;
                            }
                        }
                        txtOVC_ROUTE.Text = query.OVC_ROUTE;
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "此筆資料不存在");
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";

            string strCN = txtOvcPortChiName.Text;
            string strEN = txtOvcPortEngName.Text;
            string strOVC_ROUTE = txtOVC_ROUTE.Text;

            if (strCN.Equals(string.Empty))
            {
                strMessage += "<p> 請輸入 中文名稱！ </p>";
            }
            if (strEN.Equals(string.Empty))
            {
                strMessage += "<p> 請輸入 英文名稱！ </p>";
            }

            if (strMessage.Equals(string.Empty))
            {
                var query = MTSE.TBGMT_PORTS.Where(t => t.OVC_PORT_CDE != id);
                var query_chi = query.Where(table => table.OVC_PORT_CHI_NAME == txtOvcPortChiName.Text);
                var query_eng = query.Where(table => table.OVC_PORT_ENG_NAME == txtOvcPortEngName.Text);
                //if ( ifexist_chi == 0 && ifexist_eng == 0)
                if (!query_chi.Any() && !query_eng.Any())
                {
                    try
                    {
                        TBGMT_PORTS port = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(id)).FirstOrDefault();
                        if (port != null)
                        {
                            port.OVC_PORT_CHI_NAME = txtOvcPortChiName.Text;
                            port.OVC_PORT_ENG_NAME = txtOvcPortEngName.Text;
                            port.OVC_PORT_TYPE = drpOvcPortType.SelectedValue;
                            port.OVC_IS_ABROAD = drpOvcIsAbroad.SelectedValue;
                            port.OVC_ROUTE = strOVC_ROUTE;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), port.GetType().Name.ToString(), this, "修改");
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改機場港口資料成功！");
                        }
                    }
                    catch (Exception ex)
                    {
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "修改機場港口資料失敗！");
                    }
                }
                else
                {
                    if (query_chi.Any())
                    {
                        strMessage += "<p> 中文名稱已重複！</p>";
                    }
                    if (query_eng.Any())
                    {
                        strMessage += "<p> 英文名稱已重複！</p>";
                    }
                    if (strMessage != "")
                    {
                        FCommon.AlertShow(PnMessage, "warning", "系統訊息", strMessage);
                    }
                }
            }
            else
            {
                FCommon.AlertShow(PnMessage, "warning", "系統訊息", strMessage);
            }
        }
        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                TBGMT_PORTS port = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(id)).FirstOrDefault();
                if (port != null)
                {
                    MTSE.Entry(port).State = EntityState.Deleted;
                    MTSE.SaveChanges();


                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), port.GetType().Name.ToString(), this, "刪除");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除機場港口資料成功！");
                    Response.AddHeader("REFRESH", "2;URL=MTS_F14_1.aspx");
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"編號：{ id } 已被刪除，不存在！");
            }
            catch(Exception ex)
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "刪除機場港口資料失敗！");
            }
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F14_1{ getQueryString() }");
        }
    }
}