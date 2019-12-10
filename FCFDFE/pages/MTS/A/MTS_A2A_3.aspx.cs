using System;
using System.Linq;
using System.Web.UI;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A2A_3 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        Common FCommon = new Common();
        string id;

        #region 副程式
        private void dataImport()
        {
            TBGMT_ETR ETR = MTSE.TBGMT_ETR.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
            if (ETR != null)
            {
                lblODT_REQUIRE_DATE.Text = FCommon.getDateTime(ETR.ODT_REQUIRE_DATE);
                lblOVC_REQUIRE_MSG_NO.Text = ETR.OVC_REQUIRE_MSG_NO;
                lblODT_RECEIVE_DATE.Text = FCommon.getDateTime(ETR.ODT_RECEIVE_DATE);
                lblOVC_RECEIVE_MSG_NO.Text = ETR.OVC_RECEIVE_MSG_NO;
                lblODT_PROCESS_DATE.Text = FCommon.getDateTime(ETR.ODT_PROCESS_DATE);
                lblOVC_PROCESS_MSG_NO.Text = ETR.OVC_PROCESS_MSG_NO;
                lblODT_STRATEGY_PROCESS_DATE.Text = FCommon.getDateTime(ETR.ODT_STRATEGY_PROCESS_DATE);
                lblODT_TEL_DATE.Text = FCommon.getDateTime(ETR.ODT_TEL_DATE);
                lblODT_PASS_DATE.Text = FCommon.getDateTime(ETR.ODT_PASS_DATE);
                lblODT_SHIP_START_DATE.Text = FCommon.getDateTime(ETR.ODT_SHIP_START_DATE);
                lblODT_RETURN_DATE.Text = FCommon.getDateTime(ETR.ODT_RETURN_DATE);
                string strOVC_DELAY_DESCRIPTION = ETR.OVC_DELAY_DESCRIPTION ?? "";
                strOVC_DELAY_DESCRIPTION = strOVC_DELAY_DESCRIPTION.Replace("\r\n", "<br>");
                lblOVC_DELAY_DESCRIPTION.Text = strOVC_DELAY_DESCRIPTION;
            }

            TBGMT_EDF EDF = MTSE.TBGMT_EDF.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
            if (EDF != null)
                lblODT_VALIDITY_DATE.Text = FCommon.getDateTime(EDF.ODT_VALIDITY_DATE);

            TBGMT_ESO ESO = MTSE.TBGMT_ESO.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
            if (ESO != null)
                lblODT_STORED_DATE.Text = FCommon.getDateTime(ESO.ODT_STORED_DATE);

            TBGMT_ICR ICR = MTSE.TBGMT_ICR.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
            if (ICR != null)
                lblODT_CUSTOM_DATE.Text = FCommon.getDateTime(ICR.ODT_CUSTOM_DATE);
        }

        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", Request.QueryString["OVC_BLD_NO"], false);
            return strQueryString;
        }
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getQueryString(this, "id", out id, true);
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    lblOVC_BLD_NO.Text = id;
                    dataImport();
                }
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                string strUserId = Session["userid"].ToString();
                TBGMT_ETR etr = MTSE.TBGMT_ETR.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
                if (etr != null)
                {
                    MTSE.Entry(etr).State = EntityState.Deleted;
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), etr.GetType().Name, this, "刪除");

                    FCommon.AlertShow(PnDelete, "success", "系統訊息", $"提單編號：{ id } 之時程管制表 刪除成功");
                }
                else
                    FCommon.AlertShow(PnDelete, "danger", "系統訊息", $"提單編號：{ id } 之時程管制表 已被刪除，不存在！");
            }
            catch
            {
                FCommon.AlertShow(PnDelete, "danger", "系統訊息", "刪除失敗，請聯絡工程師。");
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_A2A_1{ getQueryString() }");
        }
    }
}