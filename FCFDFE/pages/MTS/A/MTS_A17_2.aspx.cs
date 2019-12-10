using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System.Data.Entity;
using System.Web.UI;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A17_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        DataTable dt_ONB_SIZE;
        //string id;

        #region 副程式
        private void dataImport(string strOVC_BLD_NO) //載入本頁基本資料
        {
            //資料匯入
            TBGMT_BLD bldTable = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
            TBGMT_ICR icrTable = MTSE.TBGMT_ICR.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
            lblOVC_BLD_NO.Text = strOVC_BLD_NO;
            if (bldTable != null)
            {
                lblONB_QUANITY.Text = bldTable.ONB_QUANITY.ToString();
                lblOVC_QUANITY_UNIT.Text = bldTable.OVC_QUANITY_UNIT;
                lblOVC_SHIP_NAME.Text = bldTable.OVC_SHIP_NAME;
                lblONB_WEIGHT.Text = bldTable.ONB_WEIGHT.ToString();
                lblOVC_WEIGHT_UNIT.Text = bldTable.OVC_WEIGHT_UNIT;
                lblOVC_VOYAGE.Text = bldTable.OVC_VOYAGE;
                lblONB_VOLUME.Text = bldTable.ONB_VOLUME.ToString();
                lblOVC_VOLUME_UNIT.Text = bldTable.OVC_VOLUME_UNIT;
            }
            if (icrTable != null)
            {
                lblOVC_PURCH_NO.Text = icrTable.OVC_PURCH_NO;
                lblODT_ARRIVE_PORT_DATE.Text = FCommon.getDateTime(icrTable.ODT_IMPORT_DATE);
                txtODT_CLEAR_DATE.Text = FCommon.getDateTime(icrTable.ODT_UNPACKING_DATE);
            }
        }
        private void dataImport_CTN(bool isCreate) //載入貨櫃資料表GV
        {
            //下拉式選單
            string[] strContainerSizeText = VariableMTS.strContainerSizeText;
            string[] strContainerSizeValue = VariableMTS.strContainerSizeValue;
            FCommon.getDataTable(ref dt_ONB_SIZE, strContainerSizeText, strContainerSizeValue);

            //資料匯入
            string strOVC_BLD_NO = lblOVC_BLD_NO.Text;
            if (!strOVC_BLD_NO.Equals(string.Empty))
            {
                var query =
                from IrdCtnTable in MTSE.TBGMT_IRD_CTN
                where IrdCtnTable.OVC_BLD_NO.Equals(strOVC_BLD_NO)
                select IrdCtnTable;

                DataTable dt = query.ListToDataTable();
                if (isCreate) //如果是新增，則新增一筆空資料，並設定為Editing
                {
                    string[] strField = { "OVC_CONTAINER_NO", "ONB_SIZE" };
                    if (dt.Columns.Count == 0)
                        foreach (string value in strField)
                        {
                            dt.Columns.Add(value);
                        }
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    GVTBGMT_CTN.EditIndex = dt.Rows.Count - 1;
                }
                ViewState["hasRows_CTN"] = FCommon.GridView_dataImport(GVTBGMT_CTN, dt);
            }
        }
        private void dataImport_DETAIL(bool isCreate) //載入分運明細資料表GV
        {
            //資料匯入
            string strOVC_BLD_NO = lblOVC_BLD_NO.Text;
            if (!strOVC_BLD_NO.Equals(string.Empty))
            {
                var query =
                    from IrdDetailTable in MTSE.TBGMT_IRD_DETAIL
                    join dept in MTSE.TBMDEPTs on IrdDetailTable.OVC_DEPT_CDE equals dept.OVC_DEPT_CDE into deptTemp
                    from dept in deptTemp.DefaultIfEmpty()
                    where IrdDetailTable.OVC_BLD_NO.Equals(strOVC_BLD_NO)
                    select new
                    {
                        IrdDetailTable.OVC_IRDDETAIL_SN,
                        IrdDetailTable.OVC_DEPT_CDE,
                        OVC_ONNAME = dept != null ? dept.OVC_ONNAME : "",
                        IrdDetailTable.OVC_BOX_NO,
                        IrdDetailTable.ONB_ACTUAL_RECEIVE,
                        IrdDetailTable.ONB_OVERFLOW,
                        IrdDetailTable.ONB_LESS,
                        IrdDetailTable.ONB_BROKEN
                    };
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);

                //計算清驗情形
                decimal decONB_ACTUAL_RECEIVE_T = 0, decONB_OVERFLOW_T = 0, decONB_LESS_T = 0, decONB_BROKEN_T = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    decimal.TryParse(dr["ONB_ACTUAL_RECEIVE"].ToString(), out decimal decONB_ACTUAL_RECEIVE);
                    decimal.TryParse(dr["ONB_OVERFLOW"].ToString(), out decimal decONB_OVERFLOW);
                    decimal.TryParse(dr["ONB_LESS"].ToString(), out decimal decONB_LESS);
                    decimal.TryParse(dr["ONB_BROKEN"].ToString(), out decimal decONB_BROKEN);
                    decONB_ACTUAL_RECEIVE_T += decONB_ACTUAL_RECEIVE;
                    decONB_OVERFLOW_T += decONB_OVERFLOW;
                    decONB_LESS_T += decONB_LESS;
                    decONB_BROKEN_T += decONB_BROKEN;
                }
                txtONB_ACTUAL_RECEIVE.Text = decONB_ACTUAL_RECEIVE_T.ToString();
                txtONB_OVERFLOW.Text = decONB_OVERFLOW_T.ToString();
                txtONB_LESS.Text = decONB_LESS_T.ToString();
                txtONB_BROKEN.Text = decONB_BROKEN_T.ToString();

                if (isCreate) //如果是新增，則新增一筆空資料，並設定為Editing
                {
                    string[] strField = { "OVC_IRDDETAIL_SN", "OVC_DEPT_CDE", "OVC_ONNAME", "OVC_BOX_NO", "ONB_ACTUAL_RECEIVE", "ONB_OVERFLOW", "ONB_LESS", "ONB_BROKEN" };
                    //Ctable.Visible = true;
                    if (dt.Columns.Count == 0)
                        foreach (string value in strField)
                        {
                            dt.Columns.Add(value);
                        }
                    DataRow dr = dt.NewRow();
                    dr["OVC_IRDDETAIL_SN"] = Guid.NewGuid(); //新增Guid型態之Key值
                    dt.Rows.Add(dr);
                    GVTBGMT_DETAIL.EditIndex = dt.Rows.Count - 1;
                }
                ViewState["hasRows_DETAIL"] = FCommon.GridView_dataImport(GVTBGMT_DETAIL, dt);
            }
        }

        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", Request.QueryString["OVC_BLD_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", Request.QueryString["OVC_TRANSER_DEPT_CDE"], false);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtODT_CLEAR_DATE); //設定唯讀屬性
                    //FCommon.Controls_Attributes("readonly", "true", txtONB_ACTUAL_RECEIVE, txtONB_OVERFLOW, txtONB_LESS, txtONB_BROKEN); //設定唯讀屬性 清驗情形
                    if (FCommon.getQueryString(this, "id", out string id, true))
                    {
                        dataImport(id);
                        dataImport_CTN(false);
                        dataImport_DETAIL(false);
                    }
                    else
                        FCommon.MessageBoxShow(this, "提單編號錯誤！", $"MTS_A17_1{ getQueryString() }", false);
                    //Response.Redirect("MTS_A17_1"); //讀取不到提單號碼則跳回查詢頁面
                }
            }
        }

        #region ~Click
        //protected void btnUnit_Click(object sender, EventArgs e)
        //{
        //    Session["unitquery"] = "query1";
        //}
        protected void btnCreate_CTN_Click(object sender, EventArgs e) //新增貨櫃
        {
            ViewState["isCreate_CTN"] = true;
            dataImport_CTN(true);
        }
        protected void btnCreate_DETAIL_Click(object sender, EventArgs e) //新增分運明細
        {
            ViewState["isCreate_DETAIL"] = true;
            dataImport_DETAIL(true);
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            string strOVC_BLD_NO = lblOVC_BLD_NO.Text;
            if (!strOVC_BLD_NO.Equals(string.Empty))
            {
                string strMessage = "", strUserId = "";
                string strONB_QUANITY = lblONB_QUANITY.Text; //應收件數
                string strONB_WEIGHT = lblONB_WEIGHT.Text; //重量
                string strONB_VOLUME = lblONB_VOLUME.Text; //體積
                string strODT_ARRIVE_PORT_DATE = lblODT_ARRIVE_PORT_DATE.Text; //進口日期
                string strODT_CLEAR_DATE = txtODT_CLEAR_DATE.Text; //提單/拆櫃日期
                string strONB_ACTUAL_RECEIVE = txtONB_ACTUAL_RECEIVE.Text; //實收
                string strONB_OVERFLOW = txtONB_OVERFLOW.Text; //溢卸
                string strONB_LESS = txtONB_LESS.Text; //短少
                string strONB_BROKEN = txtONB_BROKEN.Text; //破損
                string strOVC_NOTE = txtOVC_NOTE.Text; //備考

                DateTime dateODT_ARRIVE_PORT_DATE, dateODT_CLEAR_DATE, dateNow = DateTime.Now;
                decimal decONB_QUANITY, decONB_WEIGHT, decONB_VOLUME, decONB_ACTUAL_RECEIVE, decONB_OVERFLOW, decONB_LESS, decONB_BROKEN;

                TBGMT_BLD bldTable = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                if (bldTable == null) strMessage += "<p> 請先新增 提單資料檔！ </p>";
                TBGMT_ICR icrTable = MTSE.TBGMT_ICR.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                if (icrTable == null) strMessage += "<p> 請先新增 時程管制簿！ </p>";
                TBGMT_IRD irdTable = MTSE.TBGMT_IRD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                if (irdTable != null) strMessage += "<p> 此筆 接配紀錄表 已存在！ </p>";
                #region 檢查欄位輸入
                if (Session["userid"] != null)
                    strUserId = Session["userid"].ToString();
                else
                    strMessage += "<p> 請重新登入！ </p>";
                if (strONB_QUANITY.Equals(string.Empty))
                    strMessage += "<p> 應收件數 不得為空！ </p>";
                if (strONB_WEIGHT.Equals(string.Empty))
                    strMessage += "<p> 重量 不得為空！ </p>";
                if (strONB_VOLUME.Equals(string.Empty))
                    strMessage += "<p> 體積 不得為空！ </p>";
                if (strODT_ARRIVE_PORT_DATE.Equals(string.Empty))
                    strMessage += "<p> 進口日期 不得為空！ </p>";
                if (strODT_CLEAR_DATE.Equals(string.Empty))
                    strMessage += "<p> 請選擇 提單/拆櫃日期！ </p>";
                //if (strONB_ACTUAL_RECEIVE.Equals(string.Empty))
                //    strMessage += "<p> 請輸入 實收！ </p>";
                //if (strONB_OVERFLOW.Equals(string.Empty))
                //    strMessage += "<p> 請輸入 溢卸！ </p>";
                //if (strONB_LESS.Equals(string.Empty))
                //    strMessage += "<p> 請輸入 短少！ </p>";
                //if (strONB_BROKEN.Equals(string.Empty))
                //    strMessage += "<p> 請輸入 破損！ </p>";

                bool boolODT_ARRIVE_PORT_DATE = FCommon.checkDateTime(strODT_ARRIVE_PORT_DATE, "進口日期", ref strMessage, out dateODT_ARRIVE_PORT_DATE);
                bool boolODT_CLEAR_DATE = FCommon.checkDateTime(strODT_CLEAR_DATE, "提單/拆櫃日期", ref strMessage, out dateODT_CLEAR_DATE);
                bool boolONB_QUANITY = FCommon.checkDecimal(strONB_QUANITY, "應收數量", ref strMessage, out decONB_QUANITY);
                bool boolONB_WEIGHT = FCommon.checkDecimal(strONB_WEIGHT, "重量", ref strMessage, out decONB_WEIGHT);
                bool boolONB_VOLUME = FCommon.checkDecimal(strONB_VOLUME, "體積", ref strMessage, out decONB_VOLUME);
                bool boolONB_ACTUAL_RECEIVE = FCommon.checkDecimal(strONB_ACTUAL_RECEIVE, "實收", ref strMessage, out decONB_ACTUAL_RECEIVE);
                bool boolONB_OVERFLOW = FCommon.checkDecimal(strONB_OVERFLOW, "溢卸", ref strMessage, out decONB_OVERFLOW);
                bool boolONB_LESS = FCommon.checkDecimal(strONB_LESS, "短少", ref strMessage, out decONB_LESS);
                bool boolONB_BROKEN = FCommon.checkDecimal(strONB_BROKEN, "破損", ref strMessage, out decONB_BROKEN);

                if (boolONB_QUANITY && boolONB_ACTUAL_RECEIVE && boolONB_OVERFLOW && boolONB_LESS && boolONB_BROKEN)
                {
                    decimal decTotal = decONB_ACTUAL_RECEIVE + decONB_OVERFLOW + decONB_LESS + decONB_BROKEN;
                    if(decONB_QUANITY!= decTotal)
                        strMessage += "<p> 清驗情形 與 應收件數 不符！ </p>";
                }
                if (boolONB_QUANITY)
                {
                    var query =
                        from IrdDetailTable in MTSE.TBGMT_IRD_DETAIL
                        where IrdDetailTable.OVC_BLD_NO.Equals(strOVC_BLD_NO)
                        select new
                        {
                            IrdDetailTable.ONB_ACTUAL_RECEIVE,
                            IrdDetailTable.ONB_OVERFLOW,
                            IrdDetailTable.ONB_LESS,
                            IrdDetailTable.ONB_BROKEN
                        };
                    DataTable dt = CommonStatic.LinqQueryToDataTable(query); //取得分運明細

                    //計算清驗情形
                    decimal decONB_ACTUAL_RECEIVE_T = 0, decONB_OVERFLOW_T = 0, decONB_LESS_T = 0, decONB_BROKEN_T = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        decimal.TryParse(dr["ONB_ACTUAL_RECEIVE"].ToString(), out decimal theONB_ACTUAL_RECEIVE);
                        decimal.TryParse(dr["ONB_OVERFLOW"].ToString(), out decimal theONB_OVERFLOW);
                        decimal.TryParse(dr["ONB_LESS"].ToString(), out decimal theONB_LESS);
                        decimal.TryParse(dr["ONB_BROKEN"].ToString(), out decimal theONB_BROKEN);
                        decONB_ACTUAL_RECEIVE_T += theONB_ACTUAL_RECEIVE;
                        decONB_OVERFLOW_T += theONB_OVERFLOW;
                        decONB_LESS_T += theONB_LESS;
                        decONB_BROKEN_T += theONB_BROKEN;
                    }
                    decimal decTotal = decONB_ACTUAL_RECEIVE_T + decONB_OVERFLOW_T + decONB_LESS_T + decONB_BROKEN_T;
                    if (decONB_QUANITY != decTotal)
                        strMessage += "<p> 分運明細加總 與 應收件數 不符！ </p>";
                }
                #endregion

                if (strMessage.Equals(string.Empty))
                {
                    //#region 修改 BLD
                    //bldTable.ONB_QUANITY = decONB_QUANITY;
                    //bldTable.ONB_WEIGHT = decONB_WEIGHT;
                    //bldTable.ONB_VOLUME = decONB_VOLUME;
                    //MTSE.SaveChanges(); //儲存
                    //FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), bldTable.GetType().Name.ToString(), this, "修改");
                    //#endregion

                    #region 新增 IRD
                    irdTable = new TBGMT_IRD();
                    irdTable.OVC_BLD_NO = strOVC_BLD_NO;
                    irdTable.ODT_ARRIVE_PORT_DATE = dateODT_ARRIVE_PORT_DATE; //到港日期 => 進口日期
                    irdTable.ODT_CLEAR_DATE = dateODT_CLEAR_DATE; //清檢日期 => 提單/拆櫃日期
                    irdTable.ONB_ACTUAL_RECEIVE = decONB_ACTUAL_RECEIVE; //實收 若未輸入，自動補0
                    irdTable.ONB_OVERFLOW = decONB_OVERFLOW; //溢卸 若未輸入，自動補0
                    irdTable.ONB_LESS = decONB_LESS; //短少 若未輸入，自動補0
                    irdTable.ONB_BROKEN = decONB_BROKEN; //破損 若未輸入，自動補0
                    irdTable.OVC_NOTE = strOVC_NOTE; //備考
                    irdTable.OVC_CREATE_LOGIN_ID = strUserId; //資料建立人員
                    irdTable.ODT_MODIFY_DATE = dateNow; //資料修改日期
                    irdTable.OVC_IRD_SN = Guid.NewGuid();
                    MTSE.TBGMT_IRD.Add(irdTable);
                    MTSE.SaveChanges(); //儲存
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), irdTable.GetType().Name.ToString(), this, "新增");
                    #endregion

                    FCommon.AlertShow(PnMessage, "success", "系統訊息", $"提單編號：{ strOVC_BLD_NO } 新增接配紀錄表成功");
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_A17_1{ getQueryString() }");
        }
        #endregion

        #region 貨櫃GV事件
        protected void GVTBGMT_CTN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = theGridView.DataKeys[gvrIndex].Value.ToString();
            string strOVC_BLD_NO = lblOVC_BLD_NO.Text;

            switch (e.CommandName)
            {
                case "DataEdit": //編輯
                    ViewState["isCreate_CTN"] = false;
                    theGridView.EditIndex = gvrIndex;
                    dataImport_CTN(false);
                    break;
                case "DataSave": //儲存
                    TextBox txtOVC_CONTAINER_NO = (TextBox)gvr.FindControl("txtOVC_CONTAINER_NO");
                    DropDownList drpONB_SIZE = (DropDownList)gvr.FindControl("drpONB_SIZE");
                    if (ViewState["isCreate_CTN"] != null && !strOVC_BLD_NO.Equals(string.Empty) && FCommon.Controls_isExist(txtOVC_CONTAINER_NO, drpONB_SIZE))
                    {
                        string strMessage = "";
                        bool isCreate = Convert.ToBoolean(ViewState["isCreate_CTN"]);
                        string strOVC_CONTAINER_NO = txtOVC_CONTAINER_NO.Text;
                        string strONB_SIZE = drpONB_SIZE.SelectedValue;
                        decimal decONB_SIZE;
                        TBGMT_IRD_CTN IrdCtnTable = MTSE.TBGMT_IRD_CTN
                            .Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO) && table.OVC_CONTAINER_NO.Equals(strOVC_CONTAINER_NO)).FirstOrDefault();
                        #region 錯誤驗證
                        if (isCreate&& IrdCtnTable != null)
                            strMessage += $"<p> 貨櫃號碼：{ strOVC_CONTAINER_NO } 已存在，不得新增！ </p>";
                        if (!isCreate && IrdCtnTable == null)
                            strMessage += $"<p> 貨櫃號碼：{ strOVC_CONTAINER_NO } 不存在，無法修改！ </p>";
                        if (strOVC_CONTAINER_NO.Equals(string.Empty))
                            strMessage += "<p> 請輸入 貨櫃號碼！ </p>";
                        if (strONB_SIZE.Equals(string.Empty))
                            strMessage += "<p> 請選擇 尺寸！ </p>";
                        bool boolONB_SIZE = FCommon.checkDecimal(strONB_SIZE, "尺寸", ref strMessage, out decONB_SIZE);
                        #endregion
                        if (strMessage.Equals(string.Empty))
                        {
                            if (isCreate)
                            { //新增
                                IrdCtnTable = new TBGMT_IRD_CTN();
                                IrdCtnTable.OVC_BLD_NO = strOVC_BLD_NO;
                                IrdCtnTable.OVC_CONTAINER_NO = txtOVC_CONTAINER_NO.Text;
                                IrdCtnTable.ONB_SIZE = decONB_SIZE;
                                MTSE.TBGMT_IRD_CTN.Add(IrdCtnTable);
                            }
                            else
                            { //修改
                                IrdCtnTable.ONB_SIZE = decONB_SIZE;
                            }
                            MTSE.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), IrdCtnTable.GetType().Name.ToString(), this, isCreate ? "新增" : "修改");
                            theGridView.EditIndex = -1;
                            dataImport_CTN(false);
                            FCommon.AlertShow(PnMessage_CTN, "success", "系統訊息", $"貨櫃號碼：{ strOVC_CONTAINER_NO } " + (isCreate ? "新增" : "修改") + "成功。");
                        }
                        else
                            FCommon.AlertShow(PnMessage_CTN, "danger", "系統訊息", strMessage);
                    }
                    break;
                case "DataDelete": //刪除
                    if (!strOVC_BLD_NO.Equals(string.Empty))
                    {
                        TBGMT_IRD_CTN IrdCtnTable = MTSE.TBGMT_IRD_CTN
                            .Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO) && table.OVC_CONTAINER_NO.Equals(id)).FirstOrDefault();
                        if (IrdCtnTable != null)
                        {
                            MTSE.Entry(IrdCtnTable).State = EntityState.Deleted;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), IrdCtnTable.GetType().Name.ToString(), this, "刪除");
                        }
                        else
                            FCommon.AlertShow(PnMessage_CTN, "danger", "系統訊息", $"貨櫃號碼：{ id } 不存在，無法刪除！");
                    }
                    theGridView.EditIndex = -1; //若原本有設為Edit，也改為-1
                    dataImport_CTN(false);
                    break;
                case "DataCancel": //取消
                    theGridView.EditIndex = -1;
                    dataImport_CTN(false);
                    break;
            }
        }
        protected void GVTBGMT_CTN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList drpONB_SIZE = (DropDownList)e.Row.FindControl("drpONB_SIZE");
                TextBox txtOVC_CONTAINER_NO = (TextBox)e.Row.FindControl("txtOVC_CONTAINER_NO");
                Label lblONB_SIZE = (Label)e.Row.FindControl("lblONB_SIZE");
                if (dt_ONB_SIZE != null && FCommon.Controls_isExist(drpONB_SIZE, txtOVC_CONTAINER_NO, lblONB_SIZE))
                {
                    FCommon.list_dataImport(drpONB_SIZE, dt_ONB_SIZE, "Text", "Value", true);
                    string strONB_SIZE = lblONB_SIZE.Text;
                    FCommon.list_setValue(drpONB_SIZE, strONB_SIZE);
                }
                if (txtOVC_CONTAINER_NO != null)
                {
                    string strOVC_CONTAINER_NO = txtOVC_CONTAINER_NO.Text;
                    if (strOVC_CONTAINER_NO.Equals(string.Empty))
                        FCommon.Controls_Attributes("readonly", txtOVC_CONTAINER_NO);
                    else
                        FCommon.Controls_Attributes("readonly", "true", txtOVC_CONTAINER_NO);
                }
            }
        }
        protected void GVTBGMT_CTN_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows_CTN"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        #endregion

        #region 明細GV事件
        protected void GVTBGMT_DETAIL_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            Guid id = new Guid(theGridView.DataKeys[gvrIndex].Value.ToString());
            string strOVC_BLD_NO = lblOVC_BLD_NO.Text;

            switch (e.CommandName)
            {
                case "DataEdit": //編輯
                    ViewState["isCreate_DETAIL"] = false;
                    theGridView.EditIndex = gvrIndex;
                    dataImport_DETAIL(false);
                    break;
                case "DataSave": //儲存
                    TextBox txtOVC_DEPT_CDE = (TextBox)gvr.FindControl("txtOVC_DEPT_CDE");
                    TextBox txtOVC_BOX_NO = (TextBox)gvr.FindControl("txtOVC_BOX_NO");
                    TextBox txtONB_ACTUAL_RECEIVE = (TextBox)gvr.FindControl("txtONB_ACTUAL_RECEIVE");
                    TextBox txtONB_OVERFLOW = (TextBox)gvr.FindControl("txtONB_OVERFLOW");
                    TextBox txtONB_LESS = (TextBox)gvr.FindControl("txtONB_LESS");
                    TextBox txtONB_BROKEN = (TextBox)gvr.FindControl("txtONB_BROKEN");
                    if (ViewState["isCreate_DETAIL"] != null && FCommon.Controls_isExist(txtOVC_DEPT_CDE, txtOVC_BOX_NO, txtONB_ACTUAL_RECEIVE, txtONB_OVERFLOW, txtONB_LESS, txtONB_BROKEN))
                    {
                        string strMessage = "";
                        bool isCreate = Convert.ToBoolean(ViewState["isCreate_DETAIL"]);
                        string strOVC_DEPT_CDE = txtOVC_DEPT_CDE.Text;
                        string strOVC_BOX_NO = txtOVC_BOX_NO.Text; //取得箱號清單 修改使用
                        string[] strOVC_BOX_NOs = strOVC_BOX_NO.Split(','); //將箱號轉為字串陣列 新增使用
                        string strONB_ACTUAL_RECEIVE = txtONB_ACTUAL_RECEIVE.Text;
                        string strONB_OVERFLOW = txtONB_OVERFLOW.Text;
                        string strONB_LESS = txtONB_LESS.Text;
                        string strONB_BROKEN = txtONB_BROKEN.Text;

                        decimal decONB_ACTUAL_RECEIVE, decONB_OVERFLOW, decONB_LESS, decONB_BROKEN;
                        var query =
                            from IrdDetailT in MTSE.TBGMT_IRD_DETAIL
                            where IrdDetailT.OVC_BLD_NO.Equals(strOVC_BLD_NO)
                            select IrdDetailT;
                        TBGMT_IRD_DETAIL IrdDetailTable = query.Where(table => table.OVC_IRDDETAIL_SN.Equals(id)).FirstOrDefault();
                        #region 錯誤訊息
                        if (isCreate && IrdDetailTable != null)
                            strMessage += "<p> 分運明細 已存在！ </p>";
                        if (!isCreate && IrdDetailTable == null)
                            strMessage += "<p> 分運明細 不存在！ </p>";
                        if (strOVC_DEPT_CDE.Equals(string.Empty))
                            strMessage += "<p> 請選擇 分運單位！ </p>";
                        if (strOVC_BOX_NO.Equals(string.Empty))
                            strMessage += "<p> 請輸入 箱號！ </p>";
                        else
                        {
                            if (isCreate)
                            { //新增，判斷多筆箱號是否已存在
                                string strBoxExist = "";
                                foreach (string strData in strOVC_BOX_NOs)
                                {
                                    string theOVC_BOX_NO = strData.Trim();
                                    TBGMT_IRD_DETAIL query_box = query.Where(table => table.OVC_BOX_NO.Equals(theOVC_BOX_NO)).FirstOrDefault();
                                    if (query_box != null)
                                    {
                                        strBoxExist += strBoxExist.Equals(string.Empty) ? "" : ", ";
                                        strBoxExist += theOVC_BOX_NO;
                                    }
                                }
                                if (!strBoxExist.Equals(string.Empty))
                                    strMessage += $"<p> 箱號：{ strBoxExist } 已存在！ </p>";
                            }
                            else
                            {
                                TBGMT_IRD_DETAIL query_box = query.Where(table => table.OVC_BOX_NO.Equals(strOVC_BOX_NO) && !table.OVC_IRDDETAIL_SN.Equals(id)).FirstOrDefault();
                                if (query_box != null)
                                    strMessage += $"<p> 箱號：{ strOVC_BOX_NO } 已存在！ </p>";
                            }
                        }
                        //if (strONB_ACTUAL_RECEIVE.Equals(string.Empty))
                        //    strMessage += "<p> 請輸入 實收！ </p>";
                        //if (strONB_OVERFLOW.Equals(string.Empty))
                        //    strMessage += "<p> 請輸入 溢卸！ </p>";
                        //if (strONB_LESS.Equals(string.Empty))
                        //    strMessage += "<p> 請輸入 短少！ </p>";
                        //if (strONB_BROKEN.Equals(string.Empty))
                        //    strMessage += "<p> 請輸入 破損！ </p>";
                        bool boolONB_ACTUAL_RECEIVE = FCommon.checkDecimal(strONB_ACTUAL_RECEIVE, "實收", ref strMessage, out decONB_ACTUAL_RECEIVE);
                        bool boolONB_OVERFLOW = FCommon.checkDecimal(strONB_OVERFLOW, "實收", ref strMessage, out decONB_OVERFLOW);
                        bool boolONB_LESS = FCommon.checkDecimal(strONB_LESS, "實收", ref strMessage, out decONB_LESS);
                        bool boolONB_BROKEN = FCommon.checkDecimal(strONB_BROKEN, "實收", ref strMessage, out decONB_BROKEN);
                        #endregion

                        if (strMessage.Equals(string.Empty))
                        {
                            if (isCreate)
                            {
                                string strBoxSuccess = "";
                                foreach (string strData in strOVC_BOX_NOs)
                                {
                                    string theOVC_BOX_NO = strData.Trim();
                                    IrdDetailTable = new TBGMT_IRD_DETAIL();
                                    IrdDetailTable.OVC_BLD_NO = strOVC_BLD_NO;
                                    IrdDetailTable.OVC_DEPT_CDE = strOVC_DEPT_CDE;
                                    IrdDetailTable.OVC_BOX_NO = theOVC_BOX_NO;
                                    IrdDetailTable.ONB_ACTUAL_RECEIVE = decONB_ACTUAL_RECEIVE;
                                    IrdDetailTable.ONB_OVERFLOW = decONB_OVERFLOW;
                                    IrdDetailTable.ONB_LESS = decONB_LESS;
                                    IrdDetailTable.ONB_BROKEN = decONB_BROKEN;
                                    IrdDetailTable.OVC_IRDDETAIL_SN = Guid.NewGuid(); //產生新的 Guid
                                    MTSE.TBGMT_IRD_DETAIL.Add(IrdDetailTable);
                                    MTSE.SaveChanges();
                                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), IrdDetailTable.GetType().Name.ToString(), this, "新增");

                                    strBoxSuccess += strBoxSuccess.Equals(string.Empty) ? "" : ", ";
                                    strBoxSuccess += theOVC_BOX_NO;
                                }
                                FCommon.AlertShow(PnMessage_DETAIL, "success", "系統訊息", $"箱號：{ strBoxSuccess } 新增成功。");
                            }
                            else
                            {
                                IrdDetailTable.OVC_BLD_NO = strOVC_BLD_NO;
                                IrdDetailTable.OVC_DEPT_CDE = strOVC_DEPT_CDE;
                                IrdDetailTable.OVC_BOX_NO = strOVC_BOX_NO;
                                IrdDetailTable.ONB_ACTUAL_RECEIVE = decONB_ACTUAL_RECEIVE;
                                IrdDetailTable.ONB_OVERFLOW = decONB_OVERFLOW;
                                IrdDetailTable.ONB_LESS = decONB_LESS;
                                IrdDetailTable.ONB_BROKEN = decONB_BROKEN;
                                MTSE.SaveChanges();
                                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), IrdDetailTable.GetType().Name.ToString(), this, "修改");
                                FCommon.AlertShow(PnMessage_DETAIL, "success", "系統訊息", $"箱號：{ strOVC_BOX_NO } 修改成功。");
                            }
                            theGridView.EditIndex = -1;
                            dataImport_DETAIL(false);
                        }
                        else
                            FCommon.AlertShow(PnMessage_DETAIL, "danger", "系統訊息", strMessage);
                    }
                    break;
                case "DataDelete": //刪除
                    if (!strOVC_BLD_NO.Equals(string.Empty))
                    {
                        TBGMT_IRD_DETAIL IrdDetailTable = MTSE.TBGMT_IRD_DETAIL.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO) && table.OVC_IRDDETAIL_SN.Equals(id)).FirstOrDefault();
                        if (IrdDetailTable != null)
                        {
                            string strOVC_BOX_NO = IrdDetailTable.OVC_BOX_NO;
                            MTSE.Entry(IrdDetailTable).State = EntityState.Deleted;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), IrdDetailTable.GetType().Name.ToString(), this, "刪除");
                            FCommon.AlertShow(PnMessage_DETAIL, "success", "系統訊息", $"箱號：{ strOVC_BOX_NO } 刪除成功。");
                        }
                        else
                            FCommon.AlertShow(PnMessage_DETAIL, "danger", "系統訊息", "分運明細 不存在");
                    }
                    theGridView.EditIndex = -1; //若原本有設為Edit，也改為-1
                    dataImport_DETAIL(false);
                    break;
                case "DataCancel": //取消
                    theGridView.EditIndex = -1;
                    dataImport_DETAIL(false);
                    break;
            }
        }
        protected void GVTBGMT_DETAIL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                int index = gvr.RowIndex;
                if(theGridView.EditIndex == index) //修改行
                {
                    TextBox txtOVC_DEPT_CDE = (TextBox)gvr.FindControl("txtOVC_DEPT_CDE");
                    TextBox txtOVC_ONNAME = (TextBox)gvr.FindControl("txtOVC_ONNAME");
                    Button btnQueryOVC_REQ_DEPT_CDE = (Button)gvr.FindControl("btnQueryOVC_REQ_DEPT_CDE");
                    if (FCommon.Controls_isExist(txtOVC_ONNAME))
                    {
                        FCommon.Controls_Attributes("readonly", "true", txtOVC_ONNAME);
                    }
                    if (FCommon.Controls_isExist(txtOVC_DEPT_CDE, txtOVC_ONNAME,btnQueryOVC_REQ_DEPT_CDE))
                    {
                        string strOVC_DEPT_CDE = txtOVC_DEPT_CDE.ClientID.Replace("MainContent_", "");
                        string strOVC_ONNAME = txtOVC_ONNAME.ClientID.Replace("MainContent_", "");
                        btnQueryOVC_REQ_DEPT_CDE.OnClientClick = $"OpenWindow('{ strOVC_DEPT_CDE }','{ strOVC_ONNAME }')";
                    }
                }
                //DropDownList drpONB_SIZE = (DropDownList)e.Row.FindControl("drpONB_SIZE");
                //TextBox txtOVC_CONTAINER_NO = (TextBox)e.Row.FindControl("txtOVC_CONTAINER_NO");
                //Label lblONB_SIZE = (Label)e.Row.FindControl("lblONB_SIZE");
                //if (dt_ONB_SIZE != null && drpONB_SIZE != null && lblONB_SIZE != null)
                //{
                //    FCommon.list_dataImport(drpONB_SIZE, dt_ONB_SIZE, "Text", "Value", true);
                //    string strONB_SIZE = lblONB_SIZE.Text;
                //    drpONB_SIZE.SelectedValue = strONB_SIZE;
                //}
                //if (txtOVC_CONTAINER_NO != null)
                //{
                //    string strOVC_CONTAINER_NO = txtOVC_CONTAINER_NO.Text;
                //    if (strOVC_CONTAINER_NO.Equals(string.Empty))
                //        FCommon.Controls_Attributes("readonly", txtOVC_CONTAINER_NO);
                //    else
                //        FCommon.Controls_Attributes("readonly", "true", txtOVC_CONTAINER_NO);
                //}
            }
        }

        protected void GVTBGMT_DETAIL_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows_DETAIL"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        #endregion

        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    Guid id = Guid.NewGuid();
        //    string strOVC_BLD_NO = lblOVC_BLD_NO.Text;
        //    string strMessage = "";
        //    string strOVC_DEPT_CDE = txtOVC_DEPT_CDE.Text;
        //    string strOVC_BOX_NO = txtOVC_BOX_NO.Text;
        //    string strONB_ACTUAL_RECEIVE = txtONB_ACTUAL_RECEIVE.Text;
        //    string strONB_OVERFLOW = txtONB_OVERFLOW.Text;
        //    string strONB_LESS = txtONB_LESS.Text;
        //    string strONB_BROKEN = txtONB_BROKEN.Text;

        //    decimal decONB_ACTUAL_RECEIVE;
        //    decimal decONB_OVERFLOW;
        //    decimal decONB_LESS;
        //    decimal decONB_BROKEN;

        //    bool boolONB_ACTUAL_RECEIVE = FCommon.checkDecimal(strONB_ACTUAL_RECEIVE, "實收", ref strMessage, out decONB_ACTUAL_RECEIVE);
        //    bool boolONB_OVERFLOW = FCommon.checkDecimal(strONB_OVERFLOW, "實收", ref strMessage, out decONB_OVERFLOW);
        //    bool boolONB_LESS = FCommon.checkDecimal(strONB_LESS, "實收", ref strMessage, out decONB_LESS);
        //    bool boolONB_BROKEN = FCommon.checkDecimal(strONB_BROKEN, "實收", ref strMessage, out decONB_BROKEN);

        //    if (strOVC_DEPT_CDE.Equals(string.Empty))
        //        strMessage += "<p> 請輸入 分運單位！ </p>";
        //    if (strOVC_BOX_NO.Equals(string.Empty))
        //        strMessage += "<p> 請輸入 箱號！ </p>";
        //    if (strONB_ACTUAL_RECEIVE.Equals(string.Empty))
        //        strMessage += "<p> 請輸入 實收！ </p>";
        //    if (strONB_OVERFLOW.Equals(string.Empty))
        //        strMessage += "<p> 請輸入 溢卸！ </p>";
        //    if (strONB_LESS.Equals(string.Empty))
        //        strMessage += "<p> 請輸入 短少！ </p>";
        //    if (strONB_BROKEN.Equals(string.Empty))
        //        strMessage += "<p> 請輸入 破損！ </p>";

        //    if (strMessage.Equals(string.Empty))
        //    {
        //        if (ViewState["isCreate_DETAIL"] != null)
        //        {
        //            TBGMT_IRD_DETAIL IrdDetailTable = new TBGMT_IRD_DETAIL();
        //            bool isCreate = Convert.ToBoolean(ViewState["isCreate_DETAIL"]);
        //            var query =
        //            from IrdDetailT in MTSE.TBGMT_IRD_DETAIL
        //            where IrdDetailT.OVC_BLD_NO.Equals(strOVC_BLD_NO)
        //            select IrdDetailT;

        //            var query_exist = query
        //                .Where(table => table.OVC_BOX_NO.Equals(strOVC_BOX_NO))
        //                .Where(table => !table.OVC_IRDDETAIL_SN.Equals(id));
        //            if (query_exist.FirstOrDefault() != null)
        //                strMessage += "<p> 箱號 已存在！ </p>";
        //            if (!isCreate)
        //            { //修改
        //                var query_edit = query.Where(table => table.OVC_IRDDETAIL_SN.Equals(id));
        //                IrdDetailTable = query_edit.FirstOrDefault();
        //                if (IrdDetailTable == null)
        //                    strMessage += "<p> 修改錯誤！ </p>";
        //            }
        //            if (strMessage.Equals(string.Empty))
        //            {
        //                IrdDetailTable.OVC_BLD_NO = strOVC_BLD_NO;
        //                IrdDetailTable.OVC_DEPT_CDE = strOVC_DEPT_CDE;
        //                IrdDetailTable.OVC_BOX_NO = strOVC_BOX_NO;
        //                if (boolONB_ACTUAL_RECEIVE)
        //                    IrdDetailTable.ONB_ACTUAL_RECEIVE = decONB_ACTUAL_RECEIVE;
        //                if (boolONB_OVERFLOW)
        //                    IrdDetailTable.ONB_OVERFLOW = decONB_OVERFLOW;
        //                if (boolONB_LESS)
        //                    IrdDetailTable.ONB_LESS = decONB_LESS;
        //                if (boolONB_BROKEN)
        //                    IrdDetailTable.ONB_BROKEN = decONB_BROKEN;

        //                if (isCreate)
        //                {
        //                    IrdDetailTable.OVC_IRDDETAIL_SN = id;
        //                    MTSE.TBGMT_IRD_DETAIL.Add(IrdDetailTable);
        //                }
        //                try
        //                {
        //                    MTSE.SaveChanges();
        //                }
        //                catch
        //                {
        //FCommon.AlertShow(PnMessage, "danger", "系統訊息", "儲存 失敗，請聯絡工程師！");
        //                }

        //                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), IrdDetailTable.GetType().Name.ToString(), this, "修改");

        //                GVTBGMT_DETAIL.EditIndex = -1;
        //                dataImport_DETAIL(false);
        //                Ctable.Visible = false;
        //            }
        //            else
        //                FCommon.AlertShow(PnMessage_DETAIL, "danger", "系統訊息", strMessage);
        //        }
        //    }
        //    else
        //        FCommon.AlertShow(PnMessage_DETAIL, "danger", "系統訊息", strMessage);
        //}

        //protected void btnCancel_Click(object sender, EventArgs e)
        //{
        //    txtOVC_DEPT_CDE.Text = "";
        //    txtOVC_BOX_NO.Text = "";
        //    txtONB_ACTUAL_RECEIVE.Text = "";
        //    txtONB_OVERFLOW.Text = "";
        //    txtONB_LESS.Text = "";
        //    txtONB_BROKEN.Text = "";
        //    Ctable.Visible = false;
        //}
    }
}