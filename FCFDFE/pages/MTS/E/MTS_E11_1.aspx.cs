using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MTSModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.E
{
    public partial class MTS_E11_1 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        private MTSEntities MTS = new MTSEntities();
        Common FCommon = new Common();
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    bool boolimport = false;
                    string strOVC_BLD_NO, strOVC_TRANSER_DEPT_CDE, strOVC_IS_CHARGE, strOVC_SHIP_NAME, strOVC_VOYAGE, strOVC_BLD;

                    if (FCommon.getQueryString(this, "OVC_BLD_NO", out strOVC_BLD_NO, true))
                    {
                        txtOvcBldNo.Text = strOVC_BLD_NO;
                        boolimport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_TRANSER_DEPT_CDE", out strOVC_TRANSER_DEPT_CDE, true))
                        FCommon.list_setValue(drpOvcTranserDeptCde, strOVC_TRANSER_DEPT_CDE);
                    if (FCommon.getQueryString(this, "OVC_IS_CHARGE", out strOVC_IS_CHARGE, true))
                        FCommon.list_setValue(drpOvcIsCharge, strOVC_IS_CHARGE);
                    if (FCommon.getQueryString(this, "OVC_SHIP_NAME", out strOVC_SHIP_NAME, true))
                        txtOvcShipName.Text = strOVC_SHIP_NAME;
                    if (FCommon.getQueryString(this, "OVC_VOYAGE", out strOVC_VOYAGE, true))
                        txtOvcVoyage.Text = strOVC_VOYAGE;
                    if (FCommon.getQueryString(this, "OVC_BLD", out strOVC_BLD, false))
                        FCommon.list_setValue(rdoOvcBld, strOVC_BLD);
                    if (boolimport)
                        dataimport();
                }

            }  
        }
        protected void GV_TBGMT_BLD_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataimport();
        }

        protected void dataimport()
        {
            DataTable dt = new DataTable();

            string strOVC_BLD_NO = txtOvcBldNo.Text;
            string strOVC_TRANSER_DEPT_CDE = drpOvcTranserDeptCde.SelectedValue;
            string strOVC_IS_CHARGE = drpOvcIsCharge.SelectedValue;
            string strOVC_SHIP_NAME = txtOvcShipName.Text;
            string strOVC_VOYAGE = txtOvcVoyage.Text;
            string strOVC_BLD = rdoOvcBld.SelectedValue;

            ViewState["OVC_BLD_NO"] = strOVC_BLD_NO;
            ViewState["OVC_TRANSER_DEPT_CDE"] = strOVC_TRANSER_DEPT_CDE;
            ViewState["OVC_IS_CHARGE"] = strOVC_IS_CHARGE;
            ViewState["OVC_SHIP_NAME"] = strOVC_SHIP_NAME;
            ViewState["OVC_VOYAGE"] = strOVC_VOYAGE;
            ViewState["OVC_BLD"] = strOVC_BLD;

            var queryBLD =
                from bld in MTS.TBGMT_BLD
                select bld;

            var queryICS =
                from bld in MTS.TBGMT_BLD
                join ics in MTS.TBGMT_ICS on bld.OVC_BLD_NO equals ics.OVC_BLD_NO
                select bld;

            queryBLD = queryBLD.Except(queryICS);

            var query =
                from bld in queryBLD
                join edf in MTS.TBGMT_EDF on bld.OVC_BLD_NO equals edf.OVC_BLD_NO into p1
                from edf in p1.DefaultIfEmpty()
                join port in MTS.TBGMT_PORTS on bld.OVC_START_PORT equals port.OVC_PORT_CDE into p2
                from port in p2.DefaultIfEmpty()
                join port2 in MTS.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals port2.OVC_PORT_CDE into p3
                from port2 in p3.DefaultIfEmpty()
                join currtency in MTS.TBGMT_CURRENCY on bld.ONB_CARRIAGE_CURRENCY equals currtency.OVC_CURRENCY_CODE into p4
                from currtency in p4.DefaultIfEmpty()
                select new
                {
                    OVC_BLD_NO = bld.OVC_BLD_NO,
                    OVC_DEPT_CDE = edf.OVC_DEPT_CDE,
                    OVC_IS_CHARGE = bld.OVC_IS_CHARGE == null ? "是" : bld.OVC_IS_CHARGE,
                    OVC_SHIP_NAME = bld.OVC_SHIP_NAME,
                    OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY,
                    OVC_REVIEW_STATUS = edf.OVC_REVIEW_STATUS,
                    OVC_PAYMENT_TYPE = bld.OVC_PAYMENT_TYPE,
                    OVC_SEA_OR_AIR = bld.OVC_SEA_OR_AIR,
                    OVC_VOYAGE = bld.OVC_VOYAGE,
                    ODT_START_DATE = bld.ODT_START_DATE,
                    OVC_START_PORT = port.OVC_PORT_CHI_NAME,
                    ODT_ACT_ARRIVE_DATE = bld.ODT_ACT_ARRIVE_DATE,
                    OVC_ARRIVE_PORT = port2.OVC_PORT_CHI_NAME,
                    ONB_CARRIAGE = bld.ONB_CARRIAGE,
                    ONB_CARRIAGE_CURRENCY = currtency.OVC_CURRENCY_NAME,
                    OVC_STATUS = bld.OVC_STATUS,
                };

            if (!strOVC_BLD_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
            if (!strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                query = query.Where(table => table.OVC_DEPT_CDE == strOVC_TRANSER_DEPT_CDE);
            if (!strOVC_IS_CHARGE.Equals(string.Empty))
                query = query.Where(table => table.OVC_IS_CHARGE == strOVC_IS_CHARGE);
            if (!strOVC_SHIP_NAME.Equals(string.Empty))
                query = query.Where(table => table.OVC_SHIP_NAME.Contains(strOVC_SHIP_NAME));
            if (!strOVC_VOYAGE.Equals(string.Empty))
                query = query.Where(table => table.OVC_VOYAGE.Contains(strOVC_VOYAGE));
            if (!strOVC_BLD.Equals(string.Empty))
            {
                if (strOVC_BLD.Equals("預付"))
                {
                    query = query.Where(table => table.OVC_PAYMENT_TYPE == strOVC_BLD);
                }
                else
                {
                    query = query.Where(table => table.OVC_PAYMENT_TYPE != strOVC_BLD);
                }
            }
            query = query.Where(table => table.OVC_STATUS == "B");
            query = query.Where(table => table.OVC_REVIEW_STATUS == "通過");

            if (query.Count() > 1000)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
            query = query.Take(1000);
            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_BLD, dt);
            if (dt.Rows.Count > 0)
            {
                string text = dt.Rows[0][0].ToString();
                for (var i = 1; i < dt.Rows.Count; i++)
                {
                    text += "," + dt.Rows[i][0].ToString();
                }
                Session["MTS_E11"] = text;
            }
        }
        protected string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", ViewState["OVC_BLD_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", ViewState["OVC_TRANSER_DEPT_CDE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_IS_CHARGE", ViewState["OVC_IS_CHARGE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_SHIP_NAME", ViewState["OVC_SHIP_NAME"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_VOYAGE", ViewState["OVC_VOYAGE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_BLD", ViewState["OVC_BLD"], false);
            return strQueryString;

        }
        protected void txtOvcBldNo_TextChanged(object sender, EventArgs e)
        {
            txtOvcBldNo.Text = txtOvcBldNo.Text.ToUpper();   //轉大寫
        }

        protected void GV_TBGMT_BLD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBGMT_BLD.DataKeys[gvrIndex].Value.ToString();
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);
            switch (e.CommandName)
            {
                case "btnOther":
                    Session["MTS_E11_PAGE"] = gvrIndex;
                    Response.Redirect($"MTS_E11_2.aspx{strQueryString}");
                    break;
                default:
                    break;
            }
        }

    }
}