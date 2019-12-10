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
    public partial class MTS_F13_1 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTS = new MTSEntities();

        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "ODT_START_DATE", ViewState["ODT_START_DATE"], true);
            FCommon.setQueryString(ref strQueryString, "ODT_END_DATE", ViewState["ODT_END_DATE"], true);
            FCommon.setQueryString(ref strQueryString, "OVC_INS_TYPE", ViewState["OVC_INS_TYPE"], false);
            return strQueryString;
        }
        public void dataImport()
        {
            string strOdtStartDate = txtOdtStartDate.Text;
            string strOdtEndDate = txtOdtEndDate.Text;
            string strOvcInsType = drpOvcInsType.SelectedValue;
            #region 如果沒有輸入期限，自動勾選不限定日期
            if (strOdtEndDate.Equals(string.Empty) && strOdtStartDate.Equals(string.Empty))
            {
                chkOdtApplyDate.Items[0].Selected = true;
            }
            #endregion
            //存取變數值
            ViewState["ODT_START_DATE"] = strOdtStartDate;
            ViewState["ODT_END_DATE"] = strOdtEndDate;
            ViewState["OVC_INS_TYPE"] = strOvcInsType;
            if (strOvcInsType.Equals("保險費率"))
            {
                var query =
                from insrate in MTS.TBGMT_INSRATE
                join company in MTS.TBGMT_COMPANY on insrate.OVC_INSCOMPNAY equals company.OVC_COMPANY into p1
                from company in p1.DefaultIfEmpty()
                where company.OVC_CO_TYPE.Equals("1")
                orderby insrate.ONB_SORT
                select new
                {
                    insrate.INSRATE_SN,
                    OVC_INSCOMPNAY = insrate.OVC_INSCOMPNAY,
                    Effective_period = "",
                    OVC_INS_NAME = insrate.OVC_INS_NAME,
                    OVC_INS_RATE = insrate.OVC_INS_RATE,
                    ODT_START_DATE = insrate.ODT_START_DATE,
                    ODT_END_DATE = insrate.ODT_END_DATE,
                    ONB_SORT = insrate.ONB_SORT,
                    ODT_CREATE_DATE = insrate.ODT_CREATE_DATE,
                    OVC_CREATE_ID = insrate.OVC_CREATE_ID,
                    ODT_MODIFY_DATE = insrate.ODT_MODIFY_DATE,
                    OVC_MODIFY_LOGIN_ID = insrate.OVC_MODIFY_LOGIN_ID,
                    OVC_INS_NAME_1 = insrate.OVC_INS_NAME_1,
                    OVC_INS_NAME_2 = insrate.OVC_INS_NAME_2,
                    OVC_INS_NAME_3 = insrate.OVC_INS_NAME_3,
                    OVC_INS_NAME_4 = insrate.OVC_INS_NAME_4,
                    OVC_INS_RATE_1 = insrate.OVC_INS_RATE_1,
                    OVC_INS_RATE_2 = insrate.OVC_INS_RATE_2,
                    OVC_INS_RATE_3 = insrate.OVC_INS_RATE_3,
                    OVC_INS_RATE_4 = insrate.OVC_INS_RATE_4,
                    EX_WORK = (decimal)0,
                    FOB = (decimal)0,
                };
                if (!chkOdtApplyDate.SelectedValue.Equals("不限定日期"))
                {
                    if (strOdtStartDate != string.Empty)
                    {
                        DateTime start = Convert.ToDateTime(strOdtStartDate);
                        query = query.Where(ot => ot.ODT_START_DATE >= start);
                    }
                    if (strOdtEndDate != string.Empty)
                    {
                        DateTime end = Convert.ToDateTime(strOdtEndDate);
                        query = query.Where(ot => ot.ODT_END_DATE <= end);
                    }
                    if (strOdtStartDate != string.Empty && strOdtEndDate.Equals(string.Empty))
                    {
                        DateTime end = DateTime.Now;
                        query = query.Where(ot => ot.ODT_END_DATE <= end);
                    }
                }
                if (chkOdtApplyDate.SelectedValue.Equals("不限定日期"))
                {
                    //選取不限定時清空原本資料
                    txtOdtStartDate.Text = "";
                    txtOdtEndDate.Text = "";
                }
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                //判斷DT是否友值 如果有 合併開始時間 + " - " + 結束時間 =有效期間
                if (dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Effective_period"] = FCommon.getDateTime(dt.Rows[i]["ODT_START_DATE"]) + " － " + FCommon.getDateTime(dt.Rows[i]["ODT_END_DATE"]);
                        if (dt.Rows[i]["OVC_INS_NAME_1"].ToString() == "Y")
                        {
                            dt.Rows[i]["EX_WORK"] = decimal.Parse(dt.Rows[i]["EX_WORK"].ToString()) + decimal.Parse(dt.Rows[i]["OVC_INS_RATE_1"].ToString());
                            dt.Rows[i]["FOB"] = decimal.Parse(dt.Rows[i]["FOB"].ToString()) + decimal.Parse(dt.Rows[i]["OVC_INS_RATE_1"].ToString());
                        }
                        if (dt.Rows[i]["OVC_INS_NAME_2"].ToString() == "Y")
                        {
                            dt.Rows[i]["EX_WORK"] = decimal.Parse(dt.Rows[i]["EX_WORK"].ToString()) + decimal.Parse(dt.Rows[i]["OVC_INS_RATE_2"].ToString());
                            dt.Rows[i]["FOB"] = decimal.Parse(dt.Rows[i]["FOB"].ToString()) + decimal.Parse(dt.Rows[i]["OVC_INS_RATE_2"].ToString());
                        }
                        if (dt.Rows[i]["OVC_INS_NAME_3"].ToString() == "Y")
                        {
                            dt.Rows[i]["EX_WORK"] = decimal.Parse(dt.Rows[i]["EX_WORK"].ToString()) + decimal.Parse(dt.Rows[i]["OVC_INS_RATE_3"].ToString());
                            //dt.Rows[i]["FOB"] = decimal.Parse(dt.Rows[i]["FOB"].ToString()) + decimal.Parse(dt.Rows[i]["OVC_INS_RATE_1"].ToString());
                        }
                        if (dt.Rows[i]["OVC_INS_NAME_4"].ToString() == "Y")
                        {
                            dt.Rows[i]["EX_WORK"] = decimal.Parse(dt.Rows[i]["EX_WORK"].ToString()) + decimal.Parse(dt.Rows[i]["OVC_INS_RATE_4"].ToString());
                            dt.Rows[i]["FOB"] = decimal.Parse(dt.Rows[i]["FOB"].ToString()) + decimal.Parse(dt.Rows[i]["OVC_INS_RATE_4"].ToString());
                        }
                    }
                }
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_INSRATE, dt);
            }
            else if (strOvcInsType.Equals("空運運費"))
            {
                DataTable dt = new DataTable();
                var query =
                    from air in MTS.TBGMT_AIR_TRANSPORT
                    join com in MTS.TBGMT_COMPANY on air.CO_SN equals com.CO_SN
                    join cur in MTS.TBGMT_CURRENCY on air.ONB_CARRIAGE_CURRENCY equals cur.OVC_CURRENCY_CODE
                    join port in MTS.TBGMT_PORTS on air.OVC_START_PORT equals port.OVC_PORT_CDE
                    select new
                    {
                        AT_SN = air.AT_SN,
                        ODT_START_DATE = air.ODT_START_DATE,
                        ODT_END_DATE = air.ODT_END_DATE,
                        OVC_COMPANY = com.OVC_COMPANY,
                        OVC_START_PORT = port.OVC_PORT_CHI_NAME,
                        OVC_CURRENCY_NAME = cur.OVC_CURRENCY_NAME,
                        ONB_DISCOUNT_1 = air.ONB_DISCOUNT_1
                    };
                if (!chkOdtApplyDate.SelectedValue.Equals("不限定日期"))
                {

                    if (strOdtStartDate != string.Empty)
                    {
                        DateTime start = Convert.ToDateTime(strOdtStartDate);
                        query = query.Where(ot => ot.ODT_START_DATE >= start);
                    }
                    if (strOdtEndDate != string.Empty)
                    {
                        DateTime end = Convert.ToDateTime(strOdtEndDate);
                        query = query.Where(ot => ot.ODT_END_DATE <= end);
                    }
                    if (strOdtStartDate != string.Empty && strOdtEndDate.Equals(string.Empty))
                    {
                        DateTime end = DateTime.Now;
                        query = query.Where(ot => ot.ODT_END_DATE <= end);
                    }
                }
                if (chkOdtApplyDate.SelectedValue.Equals("不限定日期"))
                {
                    //選取不限定時清空原本資料
                    txtOdtStartDate.Text = "";
                    txtOdtEndDate.Text = "";
                }
                if (query.Count() > 1000)
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                    query = query.Take(1000);
                }
                dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows2"] = FCommon.GridView_dataImport(GV_TBGMT_AIR_TRANSPORT, dt);
            }
            else
            {
                DataTable dt = new DataTable();
                var query =
                    from sea in MTS.TBGMT_SEA_TRANSPORT.AsEnumerable()
                    join com in MTS.TBGMT_COMPANY on sea.CO_SN equals com.CO_SN
                    join cur in MTS.TBGMT_CURRENCY on sea.ONB_CARRIAGE_CURRENCY equals cur.OVC_CURRENCY_CODE
                    join port in MTS.TBGMT_PORTS on sea.OVC_START_PORT equals port.OVC_PORT_CDE
                    select new
                    {
                        ST_SN = sea.ST_SN,
                        OVC_COMPANY = com.OVC_COMPANY,
                        OVC_DATE = Convert.ToString(sea.ODT_START_DATE) + " － " + Convert.ToString(sea.ODT_END_DATE),
                        ODT_START_DATE = sea.ODT_START_DATE,
                        ODT_END_DATE = sea.ODT_END_DATE,
                        OVC_START_PORT = port.OVC_PORT_CHI_NAME,
                        OVC_IMPORT_EXPORT_1 = sea.OVC_IMPORT_EXPORT_1,
                        ONB_DISCOUNT_1 = sea.ONB_DISCOUNT_1,
                        OVC_ITEM_CATEGORY_1 = sea.OVC_ITEM_CATEGORY_1,
                        OVC_ITEM_CHI_NAME_1 = sea.OVC_ITEM_CHI_NAME_1,
                        OVC_CURRENCY_NAME = cur.OVC_CURRENCY_NAME,
                    };
                if (!chkOdtApplyDate.SelectedValue.Equals("不限定日期"))
                {

                    if (strOdtStartDate != string.Empty)
                    {
                        DateTime start = Convert.ToDateTime(strOdtStartDate);
                        query = query.Where(ot => ot.ODT_START_DATE >= start);
                    }
                    if (strOdtEndDate != string.Empty)
                    {
                        DateTime end = Convert.ToDateTime(strOdtEndDate);
                        query = query.Where(ot => ot.ODT_END_DATE <= end);
                    }
                    if (strOdtStartDate != string.Empty && strOdtEndDate.Equals(string.Empty))
                    {
                        DateTime end = DateTime.Now;
                        query = query.Where(ot => ot.ODT_END_DATE <= end);
                    }
                }
                if (chkOdtApplyDate.SelectedValue.Equals("不限定日期"))
                {
                    //選取不限定時清空原本資料
                    txtOdtStartDate.Text = "";
                    txtOdtEndDate.Text = "";
                }
                if (query.Count() > 1000)
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                    query = query.Take(1000);
                }
                dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows3"] = FCommon.GridView_dataImport(GV_TBGMT_SEA_TRANSPORT, dt);
            }
        }
        private void setTYPE_Change()
        {
            string strOvcInsType = drpOvcInsType.SelectedValue;
            ViewState["OVC_INS_TYPE"] = strOvcInsType;
            if (strOvcInsType.Equals("保險費率"))
            {
                lblTITLE.Text = "保險費率資料維護";
                btnSave.Visible = true;
                btnSave3.Visible = true;
                btnQuery.Visible = true;
                GV_TBGMT_INSRATE.Visible = true;
                GV_TBGMT_AIR_TRANSPORT.Visible = false;
                GV_TBGMT_SEA_TRANSPORT.Visible = false;
                btnQuery2.Visible = false;
                btnSave2.Visible = false;
                DateClumn.Visible = true;
                lblDate.Text = "保險期間";
            }
            else if (strOvcInsType.Equals("空運運費"))
            {
                lblTITLE.Text = "空運運費資料維護";
                btnSave.Visible = false;
                btnSave3.Visible = false;
                btnQuery.Visible = false;
                GV_TBGMT_INSRATE.Visible = false;
                GV_TBGMT_AIR_TRANSPORT.Visible = true;
                GV_TBGMT_SEA_TRANSPORT.Visible = false;
                btnQuery2.Visible = true;
                btnSave2.Visible = true;
                DateClumn.Visible = true;
                lblDate.Text = "合約期間";
            }
            else
            {
                lblTITLE.Text = "海運運費資料維護";
                btnSave.Visible = false;
                btnSave2.Visible = false;
                btnSave3.Visible = false;
                btnSave4.Visible = true;
                btnQuery.Visible = false;
                btnQuery2.Visible = false;
                btnQuery3.Visible = true;
                GV_TBGMT_INSRATE.Visible = false;
                GV_TBGMT_AIR_TRANSPORT.Visible = false;
                GV_TBGMT_SEA_TRANSPORT.Visible = true;
                lblDate.Text = "合約期間";
            }
        }
#endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);

                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtOdtStartDate, txtOdtEndDate);

                    bool boolImport = false;
                    string strOdtStartDate, strOdtEndDate, strOvcInsType;
                    if (FCommon.getQueryString(this, "OVC_INS_TYPE", out strOvcInsType, false))
                    {
                        FCommon.list_setValue(drpOvcInsType, strOvcInsType);
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "ODT_START_DATE", out strOdtStartDate, true))
                        txtOdtStartDate.Text = strOdtStartDate;
                    if (FCommon.getQueryString(this, "ODT_END_DATE", out strOdtEndDate, true))
                        txtOdtEndDate.Text = strOdtEndDate;
                    setTYPE_Change();
                    if (boolImport) dataImport();
                }
            }
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }
        protected void GV_TBGMT_INSRATE_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            Guid id = new Guid(theGridView.DataKeys[gvrIndex].Value.ToString());

            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);

            switch (e.CommandName)
            {
                case "btnManagement":
                    Response.Redirect($"MTS_F13_2{ strQueryString }");
                    break;
            }
        }

        protected void GV_TBGMT_AIR_TRANSPORT_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            Guid id = new Guid(theGridView.DataKeys[gvrIndex].Value.ToString());
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);
            switch (e.CommandName)
            {
                case "btnManagement":
                    Response.Redirect($"MTS_F13_7{ strQueryString }");
                    break;
            }
        }

        protected void GV_TBGMT_SEA_TRANSPORT_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            Guid id = new Guid(theGridView.DataKeys[gvrIndex].Value.ToString());
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);
            switch (e.CommandName)
            {
                case "btnManagement":
                    Response.Redirect($"MTS_F13_8{ strQueryString }");
                    break;
            }
        }

        protected void btnSave3_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F13_5{ getQueryString() }");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F13_3{ getQueryString() }");
        }
        protected void btnSave4_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F13_8{ getQueryString() }");
        }
        protected void GV_TBGMT_INSRATE_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        protected void btnQuery2_Click(object sender, EventArgs e)
        {
            dataImport();
        }
        protected void btnQuery3_Click(object sender, EventArgs e)
        {
            dataImport();
        }

        protected void btnSave2_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F13_4{ getQueryString() }");
        }

        protected void drpOvcInsType_SelectedIndexChanged(object sender, EventArgs e)
        {
            setTYPE_Change();
        }

        protected void GV_TBGMT_AIR_TRANSPORT_PreRender(object sender, EventArgs e)
        {
            bool hasRows2 = false;
            if (ViewState["hasRows2"] != null)
                hasRows2 = Convert.ToBoolean(ViewState["hasRows2"]);
            FCommon.GridView_PreRenderInit(sender, hasRows2);
        }

        protected void GV_TBGMT_SEA_TRANSPORT_PreRender(object sender, EventArgs e)
        {
            bool hasRows3 = false;
            if (ViewState["hasRows3"] != null)
                hasRows3 = Convert.ToBoolean(ViewState["hasRows3"]);
            FCommon.GridView_PreRenderInit(sender, hasRows3);
        }
    }
}