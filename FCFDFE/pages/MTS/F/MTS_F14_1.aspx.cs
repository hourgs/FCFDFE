using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F14_1 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();

        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_PORT_TYPE", ViewState["OVC_PORT_TYPE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_ROUTE", ViewState["OVC_ROUTE"], true);
            return strQueryString;
            //紀錄查詢數值
        }
        public void dataImport()
        {
            string strOvcPortType = drpOvcPortType.SelectedItem.ToString();
            string strOVC_ROUTE = drpOVC_ROUTE.SelectedValue;

            ViewState["OVC_PORT_TYPE"] = strOvcPortType;
            ViewState["OVC_ROUTE"] = strOVC_ROUTE;

            var query = from port in MTSE.TBGMT_PORTS
                        where port.OVC_PORT_TYPE.Equals(strOvcPortType)
                        select new
                        {
                            OVC_PORT_CDE = port.OVC_PORT_CDE,
                            OVC_PORT_CHI_NAME = port.OVC_PORT_CHI_NAME,
                            OVC_PORT_ENG_NAME = port.OVC_PORT_ENG_NAME,
                            OVC_PORT_TYPE = port.OVC_PORT_TYPE,
                            OVC_IS_ABROAD = port.OVC_IS_ABROAD,
                            OVC_ROUTE = port.OVC_ROUTE
                        };
            if (drpOVC_ROUTE.SelectedIndex != 0)
                query = query.Where(t => t.OVC_ROUTE.Equals(strOVC_ROUTE));
            if (query.Count() > 1000)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
            query = query.Take(1000);
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_PORTS, dt);
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    CommonMTS.list_dataImport_ROUTE(drpOVC_ROUTE, true);
                    bool boolImport = false;
                    string strOvcPortType, strOvcRoute;
                    bool boolOvcPortType = FCommon.getQueryString(this, "OVC_PORT_TYPE", out strOvcPortType, true);
                    bool boolOvcRoute = FCommon.getQueryString(this, "OVC_ROUTE", out strOvcRoute, true);
                    if (boolOvcPortType || boolOvcRoute)
                    {
                        FCommon.list_setValue(drpOvcPortType, strOvcPortType);
                        FCommon.list_setValue(drpOVC_ROUTE, strOvcRoute);
                        boolImport = true;
                    }
                    if (boolImport)
                        dataImport();
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "type", drpOvcPortType.SelectedValue, true);
            FCommon.setQueryString(ref strQueryString, "route", drpOVC_ROUTE.SelectedValue, true);
            Response.Redirect($"MTS_F14_3{ strQueryString }");
        }

        protected void GV_TBGMT_PORTS_PreRender(object sender, EventArgs e)
        {
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
                FCommon.GridView_PreRenderInit(sender, hasRows);
            if (hasRows)
            {
                GV_TBGMT_PORTS.UseAccessibleHeader = true;
                GV_TBGMT_PORTS.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void drpOvcPortType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["hasRows"] != null)
            {
                dataImport();
            }
        }

        protected void btnRoute_Click(object sender, EventArgs e)
        {
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "type", drpOvcPortType.SelectedValue, true);
            FCommon.setQueryString(ref strQueryString, "route", drpOVC_ROUTE.SelectedValue, true);
            Response.Redirect($"MTS_F14_4{strQueryString}");
        }

        protected void GV_TBGMT_PORTS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBGMT_PORTS.DataKeys[gvrIndex].Value.ToString();

            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);
            //string cde = GV_TBGMT_PORTS.Rows[gvrIndex].Cells[0].Text;
            switch (e.CommandName)
            {
                case "btnManage":
                    //string str_url_Modify;
                    //str_url_Modify = "MTS_F14_2.aspx?cde="+cde;
                    //Response.Redirect(str_url_Modify);
                    Response.Redirect($"MTS_F14_2{strQueryString}");
                    break;
                default:
                    break;
            }
        }

    }
}