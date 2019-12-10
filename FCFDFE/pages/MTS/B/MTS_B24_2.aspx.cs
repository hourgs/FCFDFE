using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.MTS.B
{
    public partial class MTS_B24_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        Guid guidEINF_SN;

        #region 副程式
        private void dataImport()
        {
            TBGMT_EINF einf = MTSE.TBGMT_EINF.Where(table => table.EINF_SN.Equals(guidEINF_SN)).FirstOrDefault();
            if (einf != null)
            {
                string strOVC_INF_NO = einf.OVC_INF_NO;
                #region 頁面上半部初始值載入
                lblOVC_INF_NO.Text = strOVC_INF_NO;
                int index = einf.OVC_GIST.IndexOf(" "); //取得空白值在第幾位
                string strOVC_MILITARY_TYPE = einf.OVC_GIST.Substring(0, index); //取得軍種
                ListItem item = drpOVC_MILITARY_TYPE.Items.FindByText(strOVC_MILITARY_TYPE);
                if (item != null) item.Selected = true;
                txtOVC_GIST.Text = einf.OVC_GIST.Substring(index);
                txtOVC_PURPOSE_TYPE.Text = einf.OVC_PURPOSE_TYPE;
                txtODT_APPLY_DATE.Text = FCommon.getDateTime(einf.ODT_APPLY_DATE);
                lblOVC_BUDGET_INF_NO.Text = einf.OVC_BUDGET_INF_NO;
                lblONB_AMOUNT.Text = einf.ONB_AMOUNT.ToString();
                txtOVC_INV_NO.Text = einf.OVC_INV_NO;
                txtODT_INV_DATE.Text = FCommon.getDateTime(einf.ODT_INV_DATE);
                txtOVC_NOTE.Text = einf.OVC_NOTE;
                FCommon.list_setValue(drpCO_SN, einf.CO_SN.ToString());
                txtOVC_PLN_CONTENT.Text = einf.OVC_PLN_CONTENT;
                #endregion

                #region 頁面下半部pn_INF初始資料載入
                if (int.TryParse(strOVC_INF_NO.Substring(4, 3), out int yyy))
                    FCommon.list_setValue(drpOVC_INF_DATE, yyy.ToString());
                #endregion

                AddCheckboxlist(false); //Checkboxlist
                dataImport_GV_TBGMT_EINN(); //GridView資料載入
            }
            else
                FCommon.AlertShow(pnMessageEINF, "danger", "系統訊息", "結報申請表 不存在！");
        }
        private void dataImport_GV_TBGMT_EINN()
        {
            string strOVC_INF_NO = lblOVC_INF_NO.Text;
            var query =
                from edf in MTSE.TBGMT_EDF.AsEnumerable()
                join einn in MTSE.TBGMT_EINN on edf.OVC_EDF_NO equals einn.OVC_EDF_NO
                join bld in MTSE.TBGMT_BLD on edf.OVC_BLD_NO equals bld.OVC_BLD_NO
                join portSta in MTSE.TBGMT_PORTS on edf.OVC_START_PORT equals portSta.OVC_PORT_CDE
                join portArr in MTSE.TBGMT_PORTS on edf.OVC_ARRIVE_PORT equals portArr.OVC_PORT_CDE
                join detail in MTSE.TBGMT_EDF_DETAIL.GroupBy(d => d.OVC_EDF_NO) on edf.OVC_EDF_NO equals detail.Key into tempDetail
                from detail in tempDetail.DefaultIfEmpty()
                where einn.OVC_INF_NO != null && einn.OVC_INF_NO.Equals(strOVC_INF_NO)
                select new
                {
                    einn.EINN_SN,
                    edf.OVC_EDF_NO,
                    edf.OVC_PURCH_NO,
                    einn.ONB_ITEM_VALUE,
                    OVC_FINAL_INS_AMOUNT = einn.OVC_FINAL_INS_AMOUNT ?? 0,
                    bld.OVC_SHIP_COMPANY,
                    OVC_START_PORT = portSta.OVC_PORT_CHI_NAME,
                    OVC_ARRIVE_PORT = portArr.OVC_PORT_CHI_NAME,
                    //COUNT = "",
                    //OVC_CHI_NAME = ""
                    COUNT = detail != null ? detail.Count() : 0,
                    OVC_CHI_NAME = detail != null && detail.Any() ? detail.FirstOrDefault().OVC_CHI_NAME : ""
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);

            int count = 0;
            foreach (DataRow dr in dt.Rows)
            {
                //string strOVC_EDF_NO = dr["OVC_EDF_NO"].ToString();
                //var queryDetail =
                //    from detail in MTSE.TBGMT_EDF_DETAIL
                //    where detail.OVC_EDF_NO.Equals(strOVC_EDF_NO)
                //    select detail;
                //dr["COUNT"] = queryDetail.Count();
                //TBGMT_EDF_DETAIL edf_detail = queryDetail.FirstOrDefault();
                //if (edf_detail != null)
                //    dr["OVC_CHI_NAME"] = edf_detail.OVC_CHI_NAME;
                //計算金額
                if (int.TryParse(dr["OVC_FINAL_INS_AMOUNT"].ToString(), out int intOVC_FINAL_INS_AMOUNT))
                    count += intOVC_FINAL_INS_AMOUNT;
            }
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_EINN, dt);
            txtONB_AMOUNT.Text = count.ToString();
        }
        private void AddCheckboxlist(bool isQuery)
        {
            string strDrpYear = drpOVC_INF_DATE.SelectedValue;
            string strOVC_EINN_NO = txtOVC_EINN_NO.Text;
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;
            if (!isQuery || (!strDrpYear.Equals(string.Empty) || !strOVC_EINN_NO.Equals(string.Empty) || !strOVC_BLD_NO.Equals(string.Empty)))
            {
                var query =
                    from edf in MTSE.TBGMT_EDF
                    join einn in MTSE.TBGMT_EINN on edf.OVC_EDF_NO equals einn.OVC_EDF_NO
                    join bld in MTSE.TBGMT_BLD on edf.OVC_BLD_NO equals bld.OVC_BLD_NO
                    where einn.OVC_INF_NO == null
                    select new
                    {
                        OVC_EINN_NO = einn.OVC_EINN_NO,
                        OVC_BLD_NO = edf.OVC_BLD_NO
                    };
                if (!strDrpYear.Equals(string.Empty))
                {
                    string yyy = "E" + strDrpYear.PadLeft(3, '0');
                    query = query.Where(table => table.OVC_EINN_NO.StartsWith(yyy));
                }
                if (!strOVC_EINN_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_EINN_NO.Contains(strOVC_EINN_NO));
                if (!strOVC_BLD_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_BLD_NO.Contains(strOVC_BLD_NO));

                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                string strFieldName = "fieldText";
                dt.Columns.Add(strFieldName);
                foreach (DataRow dr in dt.Rows)
                {
                    string strText = "<p> 投保通知書編號 " + dr["OVC_EINN_NO"].ToString() + "</p>";
                    strText += "<p> ( 提單編號 : " + dr["OVC_BLD_NO"].ToString() + " ) </p>";
                    dr[strFieldName] = strText;
                }
                FCommon.list_dataImport(chkOVC_EINN_NO, dt, strFieldName, "OVC_EINN_NO", false);
                //FCommon.list_dataImport(chkOVC_EINN_NO, dt, "Text", "Value", false);
            }
            else
                FCommon.AlertShow(PnMessageQuery, "danger", "系統訊息", "請輸入至少一個條件");
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "ODT_APPLY_DATE", Request.QueryString["ODT_APPLY_DATE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_INF_NO", Request.QueryString["OVC_INF_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_MILITARY_TYPE", Request.QueryString["OVC_MILITARY_TYPE"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_CREATE_DATE", Request.QueryString["ODT_CREATE_DATE"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_MODIFY_DATE_S", Request.QueryString["ODT_MODIFY_DATE_S"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_MODIFY_DATE_E", Request.QueryString["ODT_MODIFY_DATE_E"], false);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (FCommon.getQueryString(this, "id", out string id, true) && Guid.TryParse(id, out guidEINF_SN))
                {
                    if (!IsPostBack)
                    {
                        FCommon.Controls_Attributes("readonly", "true", txtODT_APPLY_DATE, txtODT_INV_DATE);
                        DateTime dateNow = DateTime.Now;
                        #region 匯入下拉式選單
                        string strFirstText = "不限定", strFirstValue = "";
                        CommonMTS.list_dataImport_DEPT_CLASS(drpOVC_MILITARY_TYPE, true); //軍種=
                        CommonMTS.list_dataImport_COMPANY(drpCO_SN, CommonMTS.COMPANY_TYPE.InsuranceCompany, true); //保險公司

                        int theYear = FCommon.getTaiwanYear(dateNow);
                        int yearMax = theYear + 0, yearMin = theYear - 15;
                        FCommon.list_dataImportNumber(drpOVC_INF_DATE, 2, yearMax, yearMin, strFirstText, strFirstValue, true);
                        FCommon.list_setValue(drpOVC_INF_DATE, theYear.ToString());
                        #endregion
                        dataImport();
                    }
                }
                else
                    FCommon.MessageBoxShow(this, "結報申請表 編號錯誤！", $"MTS_B24_1{ getQueryString() }", false);
            }
        }

        #region ~Click
        protected void btnModify_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedValue;
            string strOVC_MILITARY = drpOVC_MILITARY_TYPE.SelectedItem != null ? drpOVC_MILITARY_TYPE.SelectedItem.Text : "";
            string strOVC_GIST = txtOVC_GIST.Text;
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
            TBGMT_EINF einf = MTSE.TBGMT_EINF.Where(table => table.EINF_SN.Equals(guidEINF_SN)).FirstOrDefault();
            if (einf == null)
                strMessage += "<P> 結報申請表 不存在！ </p>";
            if (strOVC_MILITARY_TYPE.Equals(string.Empty) || strOVC_GIST.Equals(string.Empty))
                strMessage += "<P> 請輸入 案由 </p>";
            else
                strOVC_GIST = $"{ strOVC_MILITARY } { strOVC_GIST }";

            bool boolODT_APPLY_DATE = FCommon.checkDateTime(strODT_APPLY_DATE, "結報申請日期", ref strMessage, out DateTime dateODT_APPLY_DATE);
            bool boolONB_AMOUNT = FCommon.checkDecimal(strONB_AMOUNT, "金額", ref strMessage, out decimal decONB_AMOUNT);
            bool boolODT_INV_DATE = FCommon.checkDateTime(strODT_INV_DATE, "收據日期", ref strMessage, out DateTime dateODT_INV_DATE);
            bool boolCO_SN = Guid.TryParse(strCO_SN, out Guid guidCO_SN);
            #endregion

            #region 舊程式有但文件上沒有寫到
            //if ((!lblOVC_BUDGET_INF_NO.Text.Equals("") && txtOVC_BUDGET_INF_NO.Text.Trim().Equals("")) | (lblONB_AMOUNT.Text.Trim() != txtONB_AMOUNT.Text.Trim()))
            //{
            //    if (query.SIGNNO != "")
            //    {
            //        txtOVC_BUDGET_INF_NO.Text = lblOVC_BUDGET_INF_NO.Text.Trim();
            //        txtONB_AMOUNT.Text = lblONB_AMOUNT.Text.Trim();
            //        strMessage += "<P> 預算單中本結算表尚有簽證編號,無法變更或刪除 </p>";
            //        return;
            //    }
            //}
            //if ((!txtOVC_BUDGET_INF_NO.Text.Trim().Equals("")) &&(!lblOVC_BUDGET_INF_NO.Text.Trim().Equals(txtOVC_BUDGET_INF_NO.Text.Trim())))
            //{
            //    var queryB = MTSE.TBGMT_BUDGET.Where(table => table.OVC_BUD_NO == txtOVC_BUDGET_INF_NO.Text);
            //    if (queryB == null)
            //    {
            //        txtOVC_BUDGET_INF_NO.Text = lblOVC_BUDGET_INF_NO.Text.Trim();
            //        strMessage += "<P> 無此簽證編號,請確認 </p>";
            //        return;
            //    }
            //    else
            //    {
            //        if (!lblOVC_BUDGET_INF_NO.Text.Trim().Equals(""))
            //        {
            //            txtOVC_BUDGET_INF_NO.Text = lblOVC_BUDGET_INF_NO.Text.Trim();
            //            strMessage += "<P> 無法直接換預算單編號,請先清空預算單號再指定 </p>";
            //            return;
            //        }
            //    }
            //}
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                string strOVC_INF_NO = einf.OVC_INF_NO;
                einf.OVC_GIST = strOVC_GIST;
                einf.OVC_PURPOSE_TYPE = strOVC_PURPOSE_TYPE;
                if (boolODT_APPLY_DATE) einf.ODT_APPLY_DATE = dateODT_APPLY_DATE; else einf.ODT_APPLY_DATE = null;
                if (boolONB_AMOUNT) einf.ONB_AMOUNT = decONB_AMOUNT; else einf.ONB_AMOUNT = null;
                einf.OVC_INV_NO = strOVC_INV_NO;
                if (boolODT_INV_DATE) einf.ODT_INV_DATE = dateODT_INV_DATE; else einf.ODT_INV_DATE = null;
                einf.OVC_NOTE = strOVC_NOTE;
                if (boolCO_SN) einf.CO_SN = guidCO_SN; else einf.CO_SN = null;
                einf.OVC_PLN_CONTENT = strOVC_PLN_CONTENT;
                einf.ODT_MODIFY_DATE = dateNow;
                einf.OVC_MODIFY_LOGIN_ID = strUserId;
                MTSE.SaveChanges();
                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), einf.GetType().Name.ToString(), this, "修改");
                FCommon.AlertShow(pnMessageEINF, "success", "系統訊息", $"編號：{ strOVC_INF_NO } 之結報申請表 修改成功");
            }
            else
                FCommon.AlertShow(pnMessageEINF, "danger", "系統訊息", strMessage);
        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            AddCheckboxlist(true);
        }
        protected void btnCheckBox_Click(object sender, EventArgs e)
        {
            Button theButton = (Button)sender;
            if (bool.TryParse(theButton.CommandArgument, out bool isSelected))
                foreach (ListItem item in chkOVC_EINN_NO.Items)
                    item.Selected = isSelected;
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            TBGMT_EINF einf = MTSE.TBGMT_EINF.Where(table => table.EINF_SN.Equals(guidEINF_SN)).FirstOrDefault();
            if (einf != null)
            {
                string strSuccess = "", strExistError = "";
                string strUserId = Session["userid"].ToString();
                string strOVC_INF_NO = einf.OVC_INF_NO;
                decimal.TryParse(einf.ONB_AMOUNT.ToString(), out decimal decONB_AMOUNT);
                DateTime dateNow = DateTime.Now;
                bool isHasData = false;

                foreach (ListItem item in chkOVC_EINN_NO.Items)
                {
                    if (item.Selected)
                    {
                        string strOVC_EINN_NO = item.Value;
                        TBGMT_EINN einn = MTSE.TBGMT_EINN.Where(table => table.OVC_EINN_NO.Equals(strOVC_EINN_NO)).FirstOrDefault();
                        if (einn != null)
                        {
                            decimal.TryParse(einn.OVC_FINAL_INS_AMOUNT.ToString(), out decimal decOVC_FINAL_INS_AMOUNT);
                            decONB_AMOUNT += decOVC_FINAL_INS_AMOUNT;

                            einn.OVC_INF_NO = strOVC_INF_NO;
                            einn.OVC_MODIFY_LOGIN_ID = strUserId;
                            einn.ODT_MODIFY_DATE = dateNow;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), einn.GetType().Name.ToString(), this, "修改");

                            strSuccess += strSuccess.Equals(string.Empty) ? "" : ", ";
                            strSuccess += strOVC_EINN_NO;
                            isHasData = true;
                        }
                        else
                        {
                            strExistError += strExistError.Equals(string.Empty) ? "" : ", ";
                            strExistError += strOVC_EINN_NO;
                        }
                    }
                }
                if (!strExistError.Equals(string.Empty))
                    FCommon.AlertShow(PnMessageQuery, "danger", "系統訊息", $"<p> 編號：{ strExistError } 之投保通知書 不存在！</p>");
                if (isHasData)
                {
                    einf.ONB_AMOUNT = decONB_AMOUNT;
                    einf.ODT_MODIFY_DATE = dateNow;
                    einf.OVC_MODIFY_LOGIN_ID = strUserId;
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), einf.GetType().Name.ToString(), this, "修改");

                    FCommon.AlertShow(PnMessageQuery, "success", "系統訊息", $"編號：{ strSuccess } 之投保通知書 新增成功。");
                    AddCheckboxlist(false);
                    dataImport_GV_TBGMT_EINN();
                }
            }
            else
                FCommon.AlertShow(PnMessageQuery, "danger", "系統訊息", "結報申請表 不存在！");

            #region 舊寫法－改寫
            //ArrayList listOVC_BLD_NO = new ArrayList();
            //foreach (ListItem item in chkOVC_EINN_NO.Items)
            //    if (item.Selected)
            //        listOVC_BLD_NO.Add(item.Value);
            ////blds += "'" + item.Value + "',";
            //if (listOVC_BLD_NO.Count > 0)
            //{
            //    //更新TBGMT_EINN
            //    string[] queryEdf = MTSE.TBGMT_EDF.Where(table => listOVC_BLD_NO.Contains(table.OVC_BLD_NO)).Select(table => table.OVC_EDF_NO).ToArray();
            //    var queryEinn = MTSE.TBGMT_EINN.Where(table => queryEdf.Contains(table.OVC_EDF_NO));
            //    if (queryEinn.Any())
            //    {
            //        string strUserId = Session["userid"].ToString();
            //        string strOVC_INF_NO = lblOVC_INF_NO.Text;
            //        foreach (TBGMT_EINN einn in queryEinn)
            //        {
            //            einn.OVC_INF_NO = strOVC_INF_NO;
            //            MTSE.SaveChanges();
            //            FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), einn.GetType().Name.ToString(), this, "修改");
            //        }
            //        string edfno = "";
            //        foreach (string str in queryEdf)
            //        {
            //            edfno += str + "、";
            //        }
            //        if (edfno != "")
            //            edfno = edfno.Substring(0, edfno.Length - 1);
            //        FCommon.AlertShow(PnMessageQuery, "success", "系統訊息", "新增投保通知書" + edfno + "成功");
            //        AddCheckboxlist(false);
            //        dataImport_GV_TBGMT_EINN();
            //    }
            //}
            //else
            //    FCommon.AlertShow(PnMessageQuery, "danger", "系統訊息", "請選擇 投保通知書！");
            #endregion
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_B24_1{ getQueryString() }");
        }
        #endregion

        #region GridView
        protected void GV_TBGMT_EINN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            Guid guidEINN_SN = new Guid(GV_TBGMT_EINN.DataKeys[gvrIndex].Value.ToString());

            switch (e.CommandName)
            {
                case "dataDel":
                    TBGMT_EINF einf = MTSE.TBGMT_EINF.Where(table => table.EINF_SN.Equals(guidEINF_SN)).FirstOrDefault();
                    if (einf != null)
                    {
                        string strMessage = "";
                        string strUserId = Session["userid"].ToString();
                        DateTime dateNow = DateTime.Now;
                        TBGMT_EINN einn = MTSE.TBGMT_EINN.Where(table => table.EINN_SN.Equals(guidEINN_SN)).FirstOrDefault();
                        if (einn == null)
                            strMessage += $"<p> 投保通知書 不存在！ </p>";
                        if (strMessage.Equals(string.Empty))
                        {
                            string strOVC_EINN_NO = einn.OVC_EINN_NO;
                            decimal.TryParse(einn.OVC_FINAL_INS_AMOUNT.ToString(), out decimal decOVC_FINAL_INS_AMOUNT);
                            einn.OVC_INF_NO = null;
                            einn.ODT_MODIFY_DATE = dateNow;
                            einn.OVC_MODIFY_LOGIN_ID = strUserId;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), einn.GetType().Name.ToString(), this, "修改");

                            decimal.TryParse(einf.ONB_AMOUNT.ToString(), out decimal decONB_AMOUNT);
                            decONB_AMOUNT = decONB_AMOUNT - decOVC_FINAL_INS_AMOUNT; //扣掉投保申請書之 保費
                            einf.ONB_AMOUNT = decONB_AMOUNT;
                            einf.OVC_MODIFY_LOGIN_ID = strUserId;
                            einf.ODT_MODIFY_DATE = dateNow;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), einf.GetType().Name.ToString(), this, "修改");

                            FCommon.AlertShow(PnMessageEINN, "success", "系統訊息", $"編號：{ strOVC_EINN_NO } 之投保通知書 移除成功。");
                        }
                        else
                            FCommon.AlertShow(PnMessageEINN, "danger", "系統訊息", strMessage);
                    }
                    else
                        FCommon.AlertShow(PnMessageEINN, "danger", "系統訊息", "結報申請表 不存在！");
                    AddCheckboxlist(false);
                    dataImport_GV_TBGMT_EINN();
                    break;
                default:
                    break;
            }
        }
        protected void GV_TBGMT_EINN_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        #endregion

    }
}