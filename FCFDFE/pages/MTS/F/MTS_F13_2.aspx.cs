using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F13_2 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTS = new MTSEntities();
        Guid id;
        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "ODT_START_DATE", Request.QueryString["ODT_START_DATE"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_END_DATE", Request.QueryString["ODT_END_DATE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_INS_TYPE", Request.QueryString["OVC_INS_TYPE"], false);
            return strQueryString;
            //在接收頁面加入此副程式
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (FCommon.getQueryString(this, "id", out string strID, true))
                {
                    id = new Guid(strID);
                    if (!IsPostBack)
                    {
                        FCommon.Controls_Attributes("readonly", "true", txtOdtStartDate, txtOdtEndDate, txtEX_WORK, txtFBO_FCA_CPT_CFR, txtOTHER);
                        list_import();
                        dataImport();
                        Insurance();
                    }
                }
                else
                    FCommon.MessageBoxShow(this, "編號錯誤！", $"MTS_F13_1{ getQueryString() }", false);
            }
        }
        protected void dataImport()
        {
            var query2 =
                from tbgmt_insrate in MTS.TBGMT_INSRATE
                where tbgmt_insrate.INSRATE_SN == id
                select new
                {
                    tbgmt_insrate.OVC_INS_NAME,
                    tbgmt_insrate.OVC_INS_RATE,
                    tbgmt_insrate.ODT_START_DATE,
                    tbgmt_insrate.ODT_END_DATE,
                    tbgmt_insrate.ONB_SORT,
                    tbgmt_insrate.ODT_CREATE_DATE,
                    tbgmt_insrate.OVC_CREATE_ID,
                    tbgmt_insrate.ODT_MODIFY_DATE,
                    tbgmt_insrate.OVC_MODIFY_LOGIN_ID,
                    OVC_INSCOMPNAY = tbgmt_insrate.OVC_INSCOMPNAY,
                    tbgmt_insrate.OVC_INS_NAME_1,
                    tbgmt_insrate.OVC_INS_RATE_1,
                    tbgmt_insrate.OVC_INS_NAME_2,
                    tbgmt_insrate.OVC_INS_RATE_2,
                    tbgmt_insrate.OVC_INS_NAME_3,
                    tbgmt_insrate.OVC_INS_RATE_3,
                    tbgmt_insrate.OVC_INS_NAME_4,
                    tbgmt_insrate.OVC_INS_RATE_4
                };
            DataTable dt2 = CommonStatic.LinqQueryToDataTable(query2);
            if (dt2.Rows.Count > 0)
            {
                DataRow dr = dt2.Rows[0];
                string strOVC_INSCOMPNAY = dr["OVC_INSCOMPNAY"].ToString();
                drpOVC_INSCOMPNAY.Text = strOVC_INSCOMPNAY;
                txtOvcInsType.Text = dr["OVC_INS_NAME"].ToString();
                txtOvcInsRate.Text = dr["OVC_INS_RATE"] != null ? string.Format("{0:0.########}", decimal.Parse(dr["OVC_INS_RATE"].ToString())) : "";
                txtOdtStartDate.Text = dr["ODT_START_DATE"].ToString();
                txtOdtEndDate.Text = dr["ODT_END_DATE"].ToString();
                txtOnbIrSort.Text = dr["ONB_SORT"].ToString();
                lblOdtCreateDate.Text = dr["ODT_CREATE_DATE"].ToString();
                lblOvcCreateId.Text = dr["OVC_CREATE_ID"].ToString();
                lblOdtModifyDate.Text = dr["ODT_MODIFY_DATE"].ToString();
                lblModifyLoginId.Text = dr["OVC_MODIFY_LOGIN_ID"].ToString();
                //初始值
                ViewState["name"] = dr["OVC_INS_NAME"].ToString();
                ViewState["sort"] = dr["ONB_SORT"].ToString();
                txtOdtStartDate.Text = FCommon.getDateTime(dr["ODT_START_DATE"]);
                txtOdtEndDate.Text = FCommon.getDateTime(dr["ODT_END_DATE"]);
                chkFULL_INSURANCE.Checked = dr["OVC_INS_NAME_1"].ToString() == "Y" ? true : false;
                txtFULL_INSURANCE.Text = dr["OVC_INS_RATE_1"] != null ? string.Format("{0:0.########}", decimal.Parse(dr["OVC_INS_RATE_1"].ToString())) : "";
                chkINSIDE_LAND_INSURANCE.Checked = dr["OVC_INS_NAME_2"].ToString() == "Y" ? true : false;
                txtINSIDE_LAND_INSURANCE.Text = dr["OVC_INS_RATE_2"] != null ? string.Format("{0:0.########}", decimal.Parse(dr["OVC_INS_RATE_2"].ToString())) : "";
                chkOUTSIDE_LAND_INSURANCE.Checked = dr["OVC_INS_NAME_3"].ToString() == "Y" ? true : false;
                txtOUTSIDE_LAND_INSURANCE.Text = dr["OVC_INS_RATE_3"] != null ? string.Format("{0:0.########}", decimal.Parse(dr["OVC_INS_RATE_3"].ToString())) : "";
                chkMILITARY_INSURANCE.Checked = dr["OVC_INS_NAME_4"].ToString() == "Y" ? true : false;
                txtMILITARY_INSURANCE.Text = dr["OVC_INS_RATE_4"] != null ? string.Format("{0:0.########}", decimal.Parse(dr["OVC_INS_RATE_4"].ToString())) : "";
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            //string strOvcInsType = txtOvcInsType.Text;
            //string strOvcInsRate = txtOvcInsRate.Text;
            string strINS_NAME = "";
            string strOdtStartDay = txtOdtStartDate.Text;
            string strOdtEndDay = txtOdtEndDate.Text;
            string strOnbIrSort = txtOnbIrSort.Text;
            string strOVC_INSCOMPNAY = drpOVC_INSCOMPNAY.Text;
            string strFULL_INSURANCE = txtFULL_INSURANCE.Text;
            string strINSIDE_LAND_INSURANCE = txtINSIDE_LAND_INSURANCE.Text;
            string strOUTSIDE_LAND_INSURANCE = txtOUTSIDE_LAND_INSURANCE.Text;
            string strMILITARY_INSURANCE = txtMILITARY_INSURANCE.Text;
            DateTime dateSD, dateED;
            decimal Decsort, DecRate, decFULL_INSURANCE = 0, decINSIDE_LAND_INSURANCE = 0, decOUTSIDE_LAND_INSURANCE = 0, decMILITARY_INSURANCE = 0;
            bool boolSD = FCommon.checkDateTime(strOdtStartDay, "保險開始日", ref strMessage, out dateSD);
            bool boolED = FCommon.checkDateTime(strOdtEndDay, "保險結束日", ref strMessage, out dateED);
            bool boolDS = FCommon.checkDecimal(strOnbIrSort, "排序", ref strMessage, out Decsort);
            //bool boolRA = FCommon.checkDecimal(strOvcInsRate, "排序", ref strMessage, out DecRate);
            bool boolFULL_INSURANCE = strFULL_INSURANCE.Equals(string.Empty) ? true : FCommon.checkDecimal(strFULL_INSURANCE, "全險", ref strMessage, out decFULL_INSURANCE);
            bool boolINSIDE_LAND_INSURANCE = strINSIDE_LAND_INSURANCE.Equals(string.Empty) ? true : FCommon.checkDecimal(strINSIDE_LAND_INSURANCE, "在台內陸險", ref strMessage, out decINSIDE_LAND_INSURANCE);
            bool boolOUTSIDE_LAND_INSURANCE = strOUTSIDE_LAND_INSURANCE.Equals(string.Empty) ? true : FCommon.checkDecimal(strOUTSIDE_LAND_INSURANCE, "在外內陸險", ref strMessage, out decOUTSIDE_LAND_INSURANCE);
            bool boolMILITARY_INSURANCE = strMILITARY_INSURANCE.Equals(string.Empty) ? true : FCommon.checkDecimal(strMILITARY_INSURANCE, "兵險及罷工險", ref strMessage, out decMILITARY_INSURANCE);
            DecRate = decFULL_INSURANCE + decINSIDE_LAND_INSURANCE + decOUTSIDE_LAND_INSURANCE + decMILITARY_INSURANCE; //費率加總

            int if_exist_sort = MTS.TBGMT_INSRATE.Where(table => table.ONB_SORT == Decsort).Count();
            //int chaname = MTS.TBGMT_INSRATE.Where(table => table.OVC_INS_NAME.Equals(strOvcInsType)).Count();

            if (chkFULL_INSURANCE.Checked == true)
            {
                strINS_NAME += strINS_NAME.Equals(string.Empty) ? "全險" : "、全險";
            }
            if (chkINSIDE_LAND_INSURANCE.Checked == true)
            {
                strINS_NAME += strINS_NAME.Equals(string.Empty) ? "在台內陸險" : "、在台內陸險";
            }
            if (chkOUTSIDE_LAND_INSURANCE.Checked == true)
            {
                strINS_NAME += strINS_NAME.Equals(string.Empty) ? "在外內陸險" : "、在外內陸險";
            }
            if (chkMILITARY_INSURANCE.Checked == true)
            {
                strINS_NAME += strINS_NAME.Equals(string.Empty) ? "兵險及罷工險" : "、兵險及罷工險";
            }
            if (strOVC_INSCOMPNAY.Equals(string.Empty))
            {
                strMessage += "<p>請輸入 保險公司！</p>";
            }
            //if (strOvcInsType.Equals(string.Empty))
            //{
            //    strMessage += "<p>請輸入 保險種類！</p>";
            //}
            if (strFULL_INSURANCE.Equals(string.Empty) && strINSIDE_LAND_INSURANCE.Equals(string.Empty) && strOUTSIDE_LAND_INSURANCE.Equals(string.Empty) && strMILITARY_INSURANCE.Equals(string.Empty)
                && chkFULL_INSURANCE.Checked == false && chkINSIDE_LAND_INSURANCE.Checked == false && chkOUTSIDE_LAND_INSURANCE.Checked == false && chkMILITARY_INSURANCE.Checked == false)
            {
                strMessage += "<p>請輸入 保險種類！</p>";
            }
            //if (strOvcInsRate.Equals(string.Empty))
            //{
            //    strMessage += "<p>請輸入 保險費率百分比！</p>";
            //}
            if (strOdtStartDay.Equals(string.Empty))
            {
                strMessage += "<p>請輸入 保險開始日！</p>";
            }
            if (strOdtEndDay.Equals(string.Empty))
            {
                strMessage += "<p>請輸入 保險結束日！</p>";
            }
            if (strOnbIrSort.Equals(string.Empty))
            {
                strMessage += "<p>請輸入  排序！</p>";
            }
            if (Decsort < 0)
            {
                strMessage += "<p>排序請輸入正數！</p>";
            }
            //if (DecRate < 0)
            //{
            //    strMessage += "<p>保險費率百分比請輸入正數！</p>";
            //}
            if (decFULL_INSURANCE < 0 || decINSIDE_LAND_INSURANCE < 0 || decOUTSIDE_LAND_INSURANCE < 0 || decMILITARY_INSURANCE < 0)
            {
                strMessage += "<p>保險費率百分比請輸入正數！</p>";
            }
            //if (if_exist_sort > 0)
            //{
            //    if (ViewState["name"].ToString().Equals(strOvcInsType))
            //    {
            //
            //    }
            //    else
            //        strMessage += "<p>保險費種類不可重複！</p>";
            //}
            //if (chaname > 0)
            //{
            //    if (ViewState["sort"].ToString().Equals(strOnbIrSort))
            //    {
            //
            //    }
            //    else
            //        strMessage += "<p>排序不可重複！</p>";
            //}
            var query_sort = strMessage == "" ? MTS.TBGMT_INSRATE
                .Where(t => t.INSRATE_SN != id)
                .Where(t => t.ONB_SORT != null ? t.ONB_SORT == Decsort : false).FirstOrDefault() : null;
            if (query_sort != null)
            {
                strMessage += "<p>排序不可重複！</p>";
            }
            if (dateSD > dateED)
            {
                strMessage += "<p>保險開始日期需早於保險結束日期</p>";
            }
            var query_start_day = strMessage == "" ? MTS.TBGMT_INSRATE.AsEnumerable()
                .Where(t => t.INSRATE_SN != id)
                //.Where(t => t.OVC_INSCOMPNAY.Equals(strOVC_INSCOMPNAY))
                .Where(t => DateTime.Compare(Convert.ToDateTime(t.ODT_START_DATE), dateSD) <= 0)
                .Where(t => DateTime.Compare(Convert.ToDateTime(t.ODT_END_DATE), dateSD) >= 0).FirstOrDefault() : null;
            var query_end_day = strMessage == "" ? MTS.TBGMT_INSRATE.AsEnumerable()
                .Where(t => t.INSRATE_SN != id)
                //.Where(t => t.OVC_INSCOMPNAY.Equals(strOVC_INSCOMPNAY))
                .Where(t => DateTime.Compare(Convert.ToDateTime(t.ODT_START_DATE), dateED) <= 0)
                .Where(t => DateTime.Compare(Convert.ToDateTime(t.ODT_END_DATE), dateED) >= 0).FirstOrDefault() : null;
            if (query_start_day != null || query_end_day != null)
            {
                strMessage += "<p>保險公司的保險期間不能重疊</p>";
            }
            if (strMessage.Equals(string.Empty))
            {
                TBGMT_INSRATE insrate2 = MTS.TBGMT_INSRATE.Where(ot => ot.INSRATE_SN == id).FirstOrDefault();
                if (insrate2 != null)
                {
                    insrate2.OVC_INSCOMPNAY = strOVC_INSCOMPNAY;
                    insrate2.OVC_INS_NAME = strINS_NAME;
                    insrate2.OVC_INS_RATE = DecRate;
                    insrate2.ODT_START_DATE = dateSD;
                    insrate2.ODT_END_DATE = dateED;
                    insrate2.ONB_SORT = Decsort;
                    insrate2.ODT_MODIFY_DATE = DateTime.Now;
                    insrate2.OVC_MODIFY_LOGIN_ID = Session["userid"].ToString();
                    insrate2.OVC_INS_NAME_1 = chkFULL_INSURANCE.Checked == true ? "Y" : "N";
                    insrate2.OVC_INS_NAME_2 = chkINSIDE_LAND_INSURANCE.Checked == true ? "Y" : "N";
                    insrate2.OVC_INS_NAME_3 = chkOUTSIDE_LAND_INSURANCE.Checked == true ? "Y" : "N";
                    insrate2.OVC_INS_NAME_4 = chkMILITARY_INSURANCE.Checked == true ? "Y" : "N";
                    insrate2.OVC_INS_RATE_1 = decFULL_INSURANCE;
                    insrate2.OVC_INS_RATE_2 = decINSIDE_LAND_INSURANCE;
                    insrate2.OVC_INS_RATE_3 = decOUTSIDE_LAND_INSURANCE;
                    insrate2.OVC_INS_RATE_4 = decMILITARY_INSURANCE;
                    MTS.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), insrate2.GetType().Name.ToString(), this, "修改");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "更新成功");
                    dataImport();
                }
                else FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無符合資料!!");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void btnDel_Click(object sender, EventArgs e)
        {
            TBGMT_INSRATE insrate = MTS.TBGMT_INSRATE.Where(ot => ot.INSRATE_SN == id).FirstOrDefault();
            if (insrate != null)
            {
                MTS.Entry(insrate).State = EntityState.Deleted;
                MTS.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), insrate.GetType().Name.ToString(), this, "刪除");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無符合資料!!");
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F13_1{ getQueryString() }");
        }
        private void list_import()
        {
            var queryCompany =
                from com in MTS.TBGMT_COMPANY
                where com.OVC_CO_TYPE.Equals("1")
                select com;
            foreach (var q in queryCompany)
            {
                drpOVC_INSCOMPNAY.Items.Add(q.OVC_COMPANY);
            }
        }
        #region 保險費率
        protected void INSURANCE_CheckedChanged(object sender, EventArgs e)
        {
            Insurance();
        }

        private void Insurance()
        {
            decimal dec, decEX_WORK = 0, decFBO_FCA_CPT_CFR = 0, decOTHER = 0;
            if (decimal.TryParse(txtFULL_INSURANCE.Text, out dec))
            {
                decEX_WORK = decEX_WORK + dec;
                decFBO_FCA_CPT_CFR = decFBO_FCA_CPT_CFR + dec;
                if (chkFULL_INSURANCE.Checked == true)
                    decOTHER = decOTHER + dec;
            }
            if (decimal.TryParse(txtINSIDE_LAND_INSURANCE.Text, out dec))
            {
                decEX_WORK = decEX_WORK + dec;
                decFBO_FCA_CPT_CFR = decFBO_FCA_CPT_CFR + dec;
                if (chkINSIDE_LAND_INSURANCE.Checked == true)
                    decOTHER = decOTHER + dec;
            }
            if (decimal.TryParse(txtOUTSIDE_LAND_INSURANCE.Text, out dec))
            {
                decEX_WORK = decEX_WORK + dec;
                if (chkOUTSIDE_LAND_INSURANCE.Checked == true)
                    decOTHER = decOTHER + dec;
            }
            if (decimal.TryParse(txtMILITARY_INSURANCE.Text, out dec))
            {
                decEX_WORK = decEX_WORK + dec;
                decFBO_FCA_CPT_CFR = decFBO_FCA_CPT_CFR + dec;
                if (chkMILITARY_INSURANCE.Checked == true)
                    decOTHER = decOTHER + dec;
            }
            txtEX_WORK.Text = decEX_WORK.ToString();
            txtFBO_FCA_CPT_CFR.Text = decFBO_FCA_CPT_CFR.ToString();
            txtOTHER.Text = decOTHER.ToString();
        }
        #endregion
    }
}