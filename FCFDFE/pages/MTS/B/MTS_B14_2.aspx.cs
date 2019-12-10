using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;

namespace FCFDFE.pages.MTS.B
{
    public partial class MTS_B14_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        Common FCommon = new Common();
        //string id;
        Guid guidIINF_SN;

        #region 副程式
        private void dataImport()
        {
            TBGMT_IINF iinf = MTSE.TBGMT_IINF.Where(table => table.IINF_SN.Equals(guidIINF_SN)).FirstOrDefault();
            if (iinf != null)
            {
                string strOVC_INF_NO = iinf.OVC_INF_NO;
                lblOVC_INF_NO.Text = strOVC_INF_NO;
                int index = iinf.OVC_GIST.IndexOf(" ");//取得空白值
                if (iinf.OVC_GIST != null)
                {
                    string strOVC_MILITARY_TYPE = iinf.OVC_GIST.Substring(0, index);//取得軍種
                    ListItem mitem = drpOVC_MILITARY_TYPE.Items.FindByText(strOVC_MILITARY_TYPE);
                    if (mitem != null) mitem.Selected = true;
                }
                txtOVC_PURCH_NO.Text = iinf.OVC_GIST.Substring(index + 1); //不取空白值
                txtOVC_PURPOSE_TYPE.Text = iinf.OVC_PURPOSE_TYPE;
                txtODT_APPLY_DATE.Text = FCommon.getDateTime(iinf.ODT_APPLY_DATE);
                lblOVC_BUDGET_INF_NO.Text = iinf.OVC_BUDGET_INF_NO;
                lblONB_AMOUNT.Text = iinf.ONB_AMOUNT.ToString();
                //txtONB_AMOUNT.Text = iinf.ONB_AMOUNT.ToString();
                txtOVC_INV_NO.Text = iinf.OVC_INV_NO;
                txtODT_INV_DATE.Text = FCommon.getDateTime(iinf.ODT_INV_DATE);
                txtOVC_NOTE.Text = iinf.OVC_NOTE;
                txtISSU_NO.Text = iinf.ISSU_NO;
                FCommon.list_setValue(drpCO_SN, iinf.CO_SN.ToString());
                //if (iinf.CO_SN != null)
                //{
                //    string com = iinf.CO_SN.ToString();
                //    Guid guidcom = new Guid(com);
                //    var querycompany = MTSE.TBGMT_COMPANY.Where(id => id.CO_SN.Equals(guidcom)).FirstOrDefault();
                //    string company = querycompany.OVC_COMPANY;
                //    ListItem citem = drpCO_SN.Items.FindByText(company);
                //    if (citem != null)
                //        citem.Selected = true;
                //}
                txtOVC_PLN_CONTENT.Text = iinf.OVC_PLN_CONTENT;

                AddCheckboxlist(false); //checkboxlist
                dataImport_GV_TBGMT_IINN();
            }
            else
                FCommon.AlertShow(PnMessageIINF, "danger", "系統訊息", "結報申請表 不存在！");
        }
        private void dataImport_GV_TBGMT_IINN()
        {
            string strOVC_INF_NO = lblOVC_INF_NO.Text;
            var query =
                from iinn in MTSE.TBGMT_IINN
                join dept_class in MTSE.TBGMT_DEPT_CLASS on iinn.OVC_MILITARY_TYPE equals dept_class.OVC_CLASS into classTemp
                from dept_class in classTemp
                where iinn.OVC_INF_NO.Equals(strOVC_INF_NO)
                select new
                {
                    iinn.OVC_IINN_NO,
                    iinn.OVC_BLD_NO,
                    iinn.OVC_PURCH_NO,
                    iinn.ONB_INS_AMOUNT,
                    iinn.OVC_FINAL_INS_AMOUNT,
                    OVC_CLASS_NAME = dept_class != null ? dept_class.OVC_CLASS_NAME : "",
                    iinn.OVC_DELIVERY_CONDITION,
                    iinn.OVC_PURCHASE_TYPE
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query.ToList());
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_IINN, dt);

            //計算保費加總
            decimal decONB_AMOUNT = MTSE.TBGMT_IINN.Where(table => table.OVC_INF_NO.Equals(strOVC_INF_NO)).Select(table => table.OVC_FINAL_INS_AMOUNT).Sum() ?? 0;
            txtONB_AMOUNT.Text = decONB_AMOUNT.ToString();
            //TBGMT_IINF iinf = MTSE.TBGMT_IINF.Where(table => table.OVC_INF_NO.Equals(strOVC_INF_NO)).FirstOrDefault();
            //if (iinf != null)
            //{
            //    iinf.ONB_AMOUNT = decONB_AMOUNT;
            //    MTSE.SaveChanges();
            //    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), iinf.GetType().Name.ToString(), this, "修改");
            //}
        }
        private void AddCheckboxlist(bool isQuery)
        {
            string strDrpYear = drpODT_INS_DATE.SelectedValue;
            string strOVC_IINN_NO = txtOVC_IINN_NO.Text;
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            if (!isQuery || (!strDrpYear.Equals(string.Empty) || !strOVC_BLD_NO.Equals(string.Empty) || !strOVC_IINN_NO.Equals(string.Empty)))
            {
                var query =
                from iinn in MTSE.TBGMT_IINN
                where iinn.OVC_INF_NO == null
                select new
                {
                    OVC_IINN_NO = iinn.OVC_IINN_NO,
                    OVC_BLD_NO = iinn.OVC_BLD_NO
                };
                if (!strDrpYear.Equals(string.Empty))
                {
                    string yyy = "I" + strDrpYear.PadLeft(3, '0');
                    query = query.Where(table => table.OVC_IINN_NO.StartsWith(yyy));
                }
                if (!strOVC_IINN_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_IINN_NO.Contains(strOVC_IINN_NO));
                if (!strOVC_BLD_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));

                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                string strFieldName = "fieldText";
                dt.Columns.Add(strFieldName);
                foreach (DataRow dr in dt.Rows)
                {
                    string strText = "<p> 投保通知書編號 " + dr["OVC_IINN_NO"].ToString() + "</p>";
                    strText += "<p> ( 提單編號 : " + dr["OVC_BLD_NO"].ToString() + " ) </p>";
                    dr[strFieldName] = strText;
                }
                FCommon.list_dataImport(chkOVC_IINN_NO, dt, strFieldName, "OVC_IINN_NO", false);
                //list_dataImportV(cbOVC_IINN_NO, dt, "OVC_IINN_NO", "OVC_BLD_NO");
            }
            else
                FCommon.AlertShow(PnMessageQuery, "danger", "系統訊息", "請輸入至少一個條件");
        }
        //private void list_dataImportV(ListControl list, DataTable dt, string textField, string valueField)
        //{
        //    string strFieldName = valueField + textField;
        //    dt.Columns.Add(strFieldName);
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        dr[strFieldName] = "<p> 投保通知書編號 " + dr[textField].ToString() + "</p><p> ( 提單編號 : " + dr[valueField].ToString() + " ) </p>";
        //    }
        //    list.DataSource = dt;
        //    list.DataTextField = strFieldName;
        //    list.DataValueField = textField;
        //    list.DataBind();
        //}
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "ODT_APPLY_DATE", Request.QueryString["ODT_APPLY_DATE"], false);
            FCommon.setQueryString(ref strQueryString, "ISSU_NO", Request.QueryString["ISSU_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_INF_NO", Request.QueryString["OVC_INF_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_INV_NO", Request.QueryString["OVC_INV_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_MILITARY_TYPE", Request.QueryString["OVC_MILITARY_TYPE"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_CREATE_DATE_S", Request.QueryString["ODT_CREATE_DATE_S"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_CREATE_DATE_E", Request.QueryString["ODT_CREATE_DATE_E"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_CREATE_DATE", Request.QueryString["ODT_CREATE_DATE"], false);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (FCommon.getQueryString(this, "id", out string id, true) && Guid.TryParse(id, out guidIINF_SN))
                {
                    if (!IsPostBack)
                    {
                        FCommon.Controls_Attributes("readonly", "true", txtODT_APPLY_DATE, txtODT_INV_DATE);
                        DateTime dateNow = DateTime.Now;
                        #region 匯入下拉式選單
                        string strFirstText = "不限定", strFirstValue= "";
                        CommonMTS.list_dataImport_DEPT_CLASS(drpOVC_MILITARY_TYPE, true); //軍種
                        CommonMTS.list_dataImport_COMPANY(drpCO_SN, CommonMTS.COMPANY_TYPE.InsuranceCompany, true); //保險公司

                        int theYear = FCommon.getTaiwanYear(dateNow);
                        int yearMax = theYear + 0, yearMin = theYear - 15;
                        FCommon.list_dataImportNumber(drpODT_INS_DATE, 2, yearMax, yearMin, strFirstText, strFirstValue, true);
                        //FCommon.list_setValue(drpODT_INS_DATE, theYear.ToString());
                        FCommon.list_setValue(drpODT_INS_DATE, "");
                        #endregion

                        dataImport();
                    }
                }
                else
                    FCommon.MessageBoxShow(this, "結報申請表 編號錯誤！", $"MTS_B14_1{ getQueryString() }", false);
            }
        }

        #region ~Click
        protected void btnModify_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedItem != null ? drpOVC_MILITARY_TYPE.SelectedItem.Text : "";
            string strOVC_PURCH_NO = txtOVC_PURCH_NO.Text;
            string strISSU_NO = txtISSU_NO.Text;
            string strOVC_PURPOSE_TYPE = txtOVC_PURPOSE_TYPE.Text;
            string strODT_APPLY_DATE = txtODT_APPLY_DATE.Text;
            string strONB_AMOUNT = txtONB_AMOUNT.Text;
            string strOVC_INV_NO = txtOVC_INV_NO.Text;
            string strODT_INV_DATE = txtODT_INV_DATE.Text;
            string strOVC_NOTE = txtOVC_NOTE.Text;
            string strCO_SN = drpCO_SN.SelectedValue;
            string strOVC_PLN_CONTENT = txtOVC_PLN_CONTENT.Text;
            DateTime dateNow = DateTime.Now;

            #region 錯誤訊息
            TBGMT_IINF iinf = MTSE.TBGMT_IINF.Where(table => table.IINF_SN.Equals(guidIINF_SN)).FirstOrDefault();
            if (iinf == null)
                strMessage += $"<P> 結報申請表 不存在！ </p>";
            if (strOVC_MILITARY_TYPE.Equals(string.Empty))
                strMessage += "<P> 請選擇 軍種</p>";
            if (strOVC_PURCH_NO.Equals(string.Empty))
                strMessage += "<P> 請輸入 案號 </p>";
            if (strISSU_NO.Equals(string.Empty))
                strMessage += "<P> 請輸入 發文字號 </p>";
            if (strOVC_PURPOSE_TYPE.Equals(string.Empty))
                strMessage += "<P> 請輸入 用途別 </p>";
            if (strODT_APPLY_DATE.Equals(string.Empty))
                strMessage += "<P> 請選擇 結報申請日期 </p>";
            if (strONB_AMOUNT.Equals(string.Empty))
                strMessage += "<P> 請輸入 金額 </p>";
            if (strOVC_INV_NO.Equals(string.Empty))
                strMessage += "<P> 請輸入 收據號碼 </p>";
            if (strODT_INV_DATE.Equals(string.Empty))
                strMessage += "<P> 請選擇 收據日期 </p>";
            if (strCO_SN.Equals(string.Empty))
                strMessage += "<P> 請選擇 保險公司</p>";
            if (strOVC_PLN_CONTENT.Equals(string.Empty))
                strMessage += "<P> 請輸入 擬辦</p>";

            bool boolODT_APPLY_DATE = FCommon.checkDateTime(strODT_APPLY_DATE, "結報申請日期", ref strMessage, out DateTime dateODT_APPLY_DATE);
            bool boolONB_AMOUNT = FCommon.checkDecimal(strONB_AMOUNT, "金額", ref strMessage, out decimal decONB_AMOUNT);
            bool boolODT_INV_DATE = FCommon.checkDateTime(strODT_INV_DATE, "收據日期", ref strMessage, out DateTime dateODT_INV_DATE);
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                string strOVC_INF_NO = iinf.OVC_INF_NO;
                iinf.OVC_GIST = strOVC_MILITARY_TYPE + " " + strOVC_PURCH_NO;
                iinf.ISSU_NO = strISSU_NO;
                iinf.OVC_PURPOSE_TYPE = strOVC_PURPOSE_TYPE;
                iinf.ODT_APPLY_DATE = dateODT_APPLY_DATE;
                iinf.ONB_AMOUNT = decONB_AMOUNT;
                iinf.OVC_INV_NO = strOVC_INV_NO;
                iinf.ODT_INV_DATE = dateODT_INV_DATE;
                iinf.OVC_NOTE = strOVC_NOTE;
                iinf.CO_SN = new Guid(strCO_SN);
                iinf.OVC_PLN_CONTENT = strOVC_PLN_CONTENT;
                iinf.ODT_MODIFY_DATE = dateNow;
                iinf.OVC_MODIFY_LOGIN_ID = strUserId;
                MTSE.SaveChanges();
                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), iinf.GetType().Name.ToString(), this, "修改");
                FCommon.AlertShow(PnMessageIINF, "success", "系統訊息", $"編號：{ strOVC_INF_NO } 之結報申請表 更新成功。");
            }
            else
                FCommon.AlertShow(PnMessageIINF, "danger", "系統訊息", strMessage);
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_B14_1{ getQueryString() }");
        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            AddCheckboxlist(true);
        }
        protected void btnCheckBox_Click(object sender, EventArgs e)
        {
            Button theButton = (Button)sender;
            if (bool.TryParse(theButton.CommandArgument, out bool isSelected))
                foreach (ListItem item in chkOVC_IINN_NO.Items)
                    item.Selected = isSelected;
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            TBGMT_IINF iinf = MTSE.TBGMT_IINF.Where(table => table.IINF_SN.Equals(guidIINF_SN)).FirstOrDefault();
            if (iinf != null)
            {
                string strSuccess = "", strExistError = "";
                string strUserId = Session["userid"].ToString();
                string strOVC_INF_NO = iinf.OVC_INF_NO;
                decimal.TryParse(iinf.ONB_AMOUNT.ToString(), out decimal decONB_AMOUNT);
                DateTime dateNow = DateTime.Now;
                bool isHasData = false;

                foreach (ListItem item in chkOVC_IINN_NO.Items)
                {
                    if (item.Selected)
                    {
                        string strOVC_IINN_NO = item.Value;
                        //strBLD += "'" + li.Value + "',";
                        TBGMT_IINN iinn = MTSE.TBGMT_IINN.Where(table => table.OVC_IINN_NO.Equals(strOVC_IINN_NO)).FirstOrDefault();
                        if (iinn != null)
                        {
                            decimal.TryParse(iinn.OVC_FINAL_INS_AMOUNT.ToString(), out decimal decOVC_FINAL_INS_AMOUNT);
                            decONB_AMOUNT += decOVC_FINAL_INS_AMOUNT;

                            iinn.OVC_INF_NO = strOVC_INF_NO;
                            iinn.OVC_MODIFY_LOGIN_ID = strUserId;
                            iinn.ODT_MODIFY_DATE = dateNow;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), iinn.GetType().Name.ToString(), this, "修改");

                            strSuccess += strSuccess.Equals(string.Empty) ? "" : ", ";
                            strSuccess += strOVC_IINN_NO;
                            isHasData = true;
                        }
                        else
                        {
                            strExistError += strExistError.Equals(string.Empty) ? "" : ", ";
                            strExistError += strOVC_IINN_NO;
                            //strExistError += "<p> 投保通知書 不存在！ </p>";
                        }
                    }
                }
                if (!strExistError.Equals(string.Empty))
                    FCommon.AlertShow(PnMessageQuery, "danger", "系統訊息", $"<p> 編號：{ strExistError } 之投保通知書 不存在！</p>");
                if (isHasData)
                {
                    iinf.ONB_AMOUNT = decONB_AMOUNT;
                    iinf.ODT_MODIFY_DATE = dateNow;
                    iinf.OVC_MODIFY_LOGIN_ID = strUserId;
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), iinf.GetType().Name.ToString(), this, "修改");

                    FCommon.AlertShow(PnMessageQuery, "success", "系統訊息", $"編號：{ strSuccess } 之投保通知書 新增成功。");
                    AddCheckboxlist(false);
                    dataImport_GV_TBGMT_IINN();
                }
            }
            else
                FCommon.AlertShow(PnMessageQuery, "danger", "系統訊息", "結報申請表 不存在！");
        }
        #endregion

        #region GridView
        protected void GV_TBGMT_IINN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string strOVC_IINN_NO = GV_TBGMT_IINN.DataKeys[gvrIndex].Value.ToString();

            switch (e.CommandName)
            {
                case "dataDel":
                    TBGMT_IINF iinf = MTSE.TBGMT_IINF.Where(table => table.IINF_SN.Equals(guidIINF_SN)).FirstOrDefault();
                    if (iinf != null)
                    {
                        string strMessage = "";
                        string strUserId = Session["userid"].ToString();
                        DateTime dateNow = DateTime.Now;
                        TBGMT_IINN iinn = MTSE.TBGMT_IINN.Where(table => table.OVC_IINN_NO.Equals(strOVC_IINN_NO)).FirstOrDefault();
                        if (iinn == null)
                            strMessage += $"<p> 編號：{ strOVC_IINN_NO } 之投保通知書 不存在！ </p>";
                        if (strMessage.Equals(string.Empty))
                        {
                            decimal.TryParse(iinn.OVC_FINAL_INS_AMOUNT.ToString(), out decimal decOVC_FINAL_INS_AMOUNT);
                            iinn.OVC_INF_NO = null;
                            iinn.ODT_MODIFY_DATE = dateNow;
                            iinn.OVC_MODIFY_LOGIN_ID = strUserId;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), iinn.GetType().Name.ToString(), this, "修改");

                            decimal.TryParse(iinf.ONB_AMOUNT.ToString(), out decimal decONB_AMOUNT);
                            decONB_AMOUNT = decONB_AMOUNT - decOVC_FINAL_INS_AMOUNT; //扣掉投保申請書之 保費
                            iinf.ONB_AMOUNT = decONB_AMOUNT;
                            iinf.OVC_MODIFY_LOGIN_ID = strUserId;
                            iinf.ODT_MODIFY_DATE = dateNow;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), iinf.GetType().Name.ToString(), this, "修改");

                            FCommon.AlertShow(PnMessageIINN, "success", "系統訊息", strOVC_IINN_NO + "該筆資料刪除成功。");
                        }
                        else
                            FCommon.AlertShow(PnMessageIINN, "danger", "系統訊息", strMessage);
                    }
                    else
                        FCommon.AlertShow(PnMessageIINF, "danger", "系統訊息", "結報申請表 不存在！");
                    AddCheckboxlist(false);
                    dataImport_GV_TBGMT_IINN();
                    break;
            }
        }
        protected void GV_TBGMT_IINN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                GridView theGridView = (GridView)sender;
                int index = gvr.RowIndex;
                HyperLink hlkOVC_BLD_NO = (HyperLink)gvr.FindControl("hlkOVC_BLD_NO");
                if (FCommon.Controls_isExist(hlkOVC_BLD_NO))
                {
                    string strOVC_BLD_NO = hlkOVC_BLD_NO.Text;
                    hlkOVC_BLD_NO.NavigateUrl = $"javascript: OpenWindow_BLDDATA('{ FCommon.getEncryption(strOVC_BLD_NO) }');";
                }
            }
        }
        protected void GV_TBGMT_IINN_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        #endregion
    }
}