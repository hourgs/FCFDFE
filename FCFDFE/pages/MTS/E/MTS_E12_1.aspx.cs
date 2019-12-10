using FCFDFE.Content;
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
    public partial class MTS_E12_1 : System.Web.UI.Page
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
                    DataTable DEPT_CLASS = CommonStatic.ListToDataTable(MTS.TBGMT_DEPT_CLASS.ToList());
                    FCommon.list_dataImport(drpOvcMilitaryType, DEPT_CLASS, "OVC_CLASS_NAME", "OVC_CLASS", true);
                    FCommon.Controls_Attributes("readonly", "true", txtOdtInvDate1, txtOdtInvDate2);

                    bool boolImport = false;
                    string strOVC_BLD_NO, strOVC_SEA_OR_AIR, strOVC_SHIP_NAME, strOVC_VOYAGE, strOVC_MILITARY_TYPE, isBring, isDefine, strODT_START_DATE, strODT_LAST_DATE;
                    if (FCommon.getQueryString(this, "OVC_BLD_NO", out strOVC_BLD_NO, true))
                    {
                        txtOvcBldNo.Text = strOVC_BLD_NO;
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_SEA_OR_AIR", out strOVC_SEA_OR_AIR, false))
                    {
                        FCommon.list_setValue(drpOvcSeaOrAir, strOVC_SEA_OR_AIR);
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_SHIP_NAME", out strOVC_SHIP_NAME, true))
                    {
                        txtOvcShipName.Text = strOVC_SHIP_NAME;
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_VOYAGE", out strOVC_VOYAGE, true))
                    {
                        txtOvcVoyage.Text = strOVC_VOYAGE;
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OVC_MILITARY_TYPE", out strOVC_MILITARY_TYPE, true))
                    {
                        FCommon.list_setValue(drpOvcMilitaryType, strOVC_MILITARY_TYPE);
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "isBring", out isBring, true))
                    {
                        FCommon.list_setValue(drpIsBring, isBring);
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "isDefine", out isDefine, true))
                    {
                        FCommon.list_setValue(rdoOvcEinnNo1, isDefine);
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "ODT_START_DATE", out strODT_START_DATE, true))
                    {
                        txtOdtInvDate1.Text = strODT_START_DATE;
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "ODT_LAST_DATE", out strODT_LAST_DATE, true))
                    {
                        txtOdtInvDate2.Text = strODT_LAST_DATE;
                        boolImport = true;
                    }
                    if (boolImport)
                    {
                        dataimport();
                    }
                }
            }
        }
        protected void GV_TBGMT_CINF_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        public void btnQuery_onclick(object sender, EventArgs e)
        {
            dataimport();
        }

        private string getQueryString()
        {

            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", ViewState["OVC_BLD_NO"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_SEA_OR_AIR", ViewState["OVC_SEA_OR_AIR"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_SHIP_NAME", ViewState["OVC_SHIP_NAME"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_VOYAGE", ViewState["OVC_VOYAGE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_MILITARY_TYPE", ViewState["OVC_MILITARY_TYPE"], true);
            FCommon.setQueryString(ref strQueryString, "isBring", ViewState["isBring"], true);
            FCommon.setQueryString(ref strQueryString, "isDefine", ViewState["isDefine"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_START_DATE", ViewState["ODT_START_DATE"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_LAST_DATE", ViewState["ODT_LAST_DATE"], true);
            return strQueryString;
        }

        protected void dataimport()
        {
            DataTable dt = new DataTable();
            string strOVC_BLD_NO = txtOvcBldNo.Text;
            string strOVC_SEA_OR_AIR = drpOvcSeaOrAir.SelectedItem.Text;
            string strOVC_SHIP_NAME = txtOvcShipName.Text;
            string strOVC_VOYAGE = txtOvcVoyage.Text;
            string strOVC_MILITARY_TYPE = drpOvcMilitaryType.SelectedItem.Value;
            string isBring = drpIsBring.SelectedItem.Text;
            string isDefine = rdoOvcEinnNo1.SelectedItem.Text;
            string strODT_START_DATE = txtOdtInvDate1.Text;
            string strODT_LAST_DATE = txtOdtInvDate2.Text;

            ViewState["OVC_BLD_NO"] = strOVC_BLD_NO;
            ViewState["OVC_SEA_OR_AIR"] = strOVC_SEA_OR_AIR;
            ViewState["OVC_SHIP_NAME"] = strOVC_SHIP_NAME;
            ViewState["OVC_VOYAGE"] = strOVC_VOYAGE;
            ViewState["OVC_MILITARY_TYPE"] = strOVC_MILITARY_TYPE;
            ViewState["isBring"] = isBring;
            ViewState["isDefine"] = isDefine;
            ViewState["ODT_START_DATE"] = strODT_START_DATE;
            ViewState["ODT_LAST_DATE"] = strODT_LAST_DATE;

            if (strOVC_BLD_NO == string.Empty && strOVC_SEA_OR_AIR == "不限定" && strOVC_SHIP_NAME == string.Empty && strOVC_VOYAGE == string.Empty && strOVC_MILITARY_TYPE == string.Empty && isBring == "不限定" && isDefine == "不限定" && strODT_START_DATE == string.Empty && strODT_LAST_DATE == string.Empty)
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "至少輸入一項條件!!");
            }
            else
            {
                var query =
                from ics in MTS.TBGMT_ICS
                join bld in MTS.TBGMT_BLD on ics.OVC_BLD_NO equals bld.OVC_BLD_NO into p1
                from bld in p1.DefaultIfEmpty()
                select new
                {
                    OVC_ICS_NO = ics.OVC_ICS_NO,
                    OVC_BLD_NO = ics.OVC_BLD_NO,
                    OVC_INLAND_CARRIAGE = ics.OVC_INLAND_CARRIAGE,
                    OVC_INF_NO = ics.OVC_INF_NO,
                    OVC_SEA_OR_AIR = bld.OVC_SEA_OR_AIR,
                    OVC_SHIP_NAME = bld.OVC_SHIP_NAME,
                    OVC_VOYAGE = bld.OVC_VOYAGE,
                    OVC_MILITARY_TYPE = bld.OVC_MILITARY_TYPE,
                    ODT_START_DATE = bld.ODT_START_DATE,
                    ODT_PLN_ARRIVE_DATE = bld.ODT_PLN_ARRIVE_DATE,

                };
            
                if (!strOVC_BLD_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));
                if (!strOVC_SEA_OR_AIR.Equals("不限定"))
                    query = query.Where(table => table.OVC_SEA_OR_AIR.Equals(strOVC_SEA_OR_AIR));
                if (!strOVC_SHIP_NAME.Equals(string.Empty))
                    query = query.Where(table => table.OVC_SHIP_NAME.Contains(strOVC_SHIP_NAME));
                if (!strOVC_VOYAGE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_VOYAGE.Equals(strOVC_VOYAGE));
                if (!strOVC_MILITARY_TYPE.Equals(string.Empty))
                    query = query.Where(table => table.OVC_MILITARY_TYPE.Equals(strOVC_MILITARY_TYPE));
                if (isBring.Equals("是"))
                    query = query.Where(table => !table.OVC_INF_NO.Equals(string.Empty));
                if (isBring.Equals("否"))
                    query = query.Where(table => table.OVC_INF_NO.Equals(string.Empty));
                if (!isDefine.Equals("不限定") && !txtOdtInvDate1.Text.Equals(string.Empty) && !txtOdtInvDate2.Text.Equals(string.Empty))
                {
                    DateTime strat = Convert.ToDateTime(strODT_START_DATE);
                    DateTime end = Convert.ToDateTime(strODT_LAST_DATE);

                    if (strat > end)
                    {
                        query = query.Where(table => table.ODT_START_DATE <= strat);
                        query = query.Where(table => table.ODT_PLN_ARRIVE_DATE <= end);
                    }
                    else
                    {
                        query = query.Where(table => table.ODT_START_DATE >= strat);
                        query = query.Where(table => table.ODT_PLN_ARRIVE_DATE >= end);
                    }

                }
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_CINF, dt);
            }

            //把搜尋結果Bld儲存至清單
            if (dt.Rows.Count > 0)
            {
                string txtBld_NO = dt.Rows[0][1].ToString();
                for (var i = 1; i < dt.Rows.Count; i++)
                {
                    txtBld_NO += "," + dt.Rows[i][1].ToString();
                }
                Session["MTS_E12"] = txtBld_NO;
            }
        }

        public void btnNew_onclick(object sender,EventArgs e)
        {
            string strQueryString = getQueryString();
            Response.Redirect($"MTS_E12_3{strQueryString}");
        }

        protected void GV_TBGMT_CINF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBGMT_CINF.DataKeys[gvrIndex].Value.ToString();
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);


            switch (e.CommandName)
            {
                case "btnOther":
                    Response.Redirect($"MTS_E12_2{strQueryString}");
                    break;
                default:
                    break;
            }
        }
        protected void txtOvcBldNo_TextChanged(object sender, EventArgs e)
        {
            txtOvcBldNo.Text = txtOvcBldNo.Text.ToUpper();   //轉大寫
        }
    }
}