using System;
using System.Linq;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.Web.UI;

namespace FCFDFE.pages.MTS.B
{
    public partial class MTS_B23 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();

        #region 副程式
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtODT_APPLY_DATE, txtODT_INV_DATE);
                    DateTime dateNow = DateTime.Now;
                    #region 匯入下拉式選單
                    //DataTable dtOVC_GIST = CommonStatic.ListToDataTable(MTSE.TBGMT_DEPT_CLASS.OrderBy(c => c.OVC_CLASS_NAME).ToList());
                    //FCommon.list_dataImport(drpOVC_GIST, dtOVC_GIST, "OVC_CLASS_NAME", "OVC_CLASS", true);
                    CommonMTS.list_dataImport_DEPT_CLASS(drpOVC_MILITARY_TYPE, true); //軍種
                    //var query =
                    //    from c in MTSE.TBGMT_COMPANY
                    //    join i in MTSE.TBGMT_INSRATE on c.OVC_COMPANY equals i.OVC_INSCOMPNAY
                    //    where c.OVC_CO_TYPE == "1"
                    //    select c;
                    //DataTable dtOVC_COMPANY = CommonStatic.ListToDataTable(query.ToList());
                    //FCommon.list_dataImport(drpOVC_COMPANY, dtOVC_COMPANY, "OVC_COMPANY", "CO_SN", true);
                    //CommonMTS.list_dataImport_COMPANY(drpOVC_COMPANY, CommonMTS.COMPANY_TYPE.InsuranceCompany, true); //保險公司
                    CommonMTS.list_dataImport_COMPANY(drpCO_SN, CommonMTS.COMPANY_TYPE.InsuranceCompany, true); //保險公司
                    #endregion

                    txtODT_APPLY_DATE.Text = FCommon.getDateTime(dateNow);
                    txtOVC_PLN_CONTENT.Text = "擬請准予結報";
                }
            }
        }

        #region ~Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            //string strOVC_INF_NO = ViewState["OVC_INF_NO"].ToString();
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedValue;
            string strOVC_MILITARY = drpOVC_MILITARY_TYPE.SelectedItem != null ? drpOVC_MILITARY_TYPE.SelectedItem.Text : "";
            string strOVC_GIST = txtOVC_GIST.Text;
            string strOVC_PURPOSE_TYPE = txtOVC_PURPOSE_TYPE.Text;
            string strODT_APPLY_DATE = txtODT_APPLY_DATE.Text;
            string strOVC_INV_NO = txtOVC_INV_NO.Text;
            string strODT_INV_DATE = txtODT_INV_DATE.Text;
            string strOVC_NOTE = txtOVC_NOTE.Text;
            string strCO_SN = drpCO_SN.SelectedValue;
            string strOVC_PLN_CONTENT = txtOVC_PLN_CONTENT.Text;
            DateTime dateNow = DateTime.Now;

            #region 結報申請表編號
            int yyy = FCommon.getTaiwanYear(dateNow);
            string strOVC_INF_NO = "EINF" + yyy.ToString("000");
            TBGMT_EINF einf = MTSE.TBGMT_EINF.Where(table => table.OVC_INF_NO.StartsWith(strOVC_INF_NO))
                    .OrderByDescending(table => table.OVC_INF_NO).FirstOrDefault();
            int einf_no_num = 0;
            if (einf != null)
                int.TryParse(einf.OVC_INF_NO.Substring(strOVC_INF_NO.Length, 4), out einf_no_num);
            einf_no_num++;
            strOVC_INF_NO = strOVC_INF_NO + einf_no_num.ToString("0000");
            #endregion

            #region 錯誤訊息
            einf = MTSE.TBGMT_EINF.Where(table => table.OVC_INF_NO.Equals(strOVC_INF_NO)).FirstOrDefault();
            if (einf != null)
                strMessage += "<P> 結報申請表編號 已存在，請重新試過！ </p>";

            if (strOVC_MILITARY_TYPE.Equals(string.Empty) || strOVC_GIST.Equals(string.Empty))
                strMessage += "<P> 請輸入 案由 </p>";
            else
                strOVC_GIST = $"{ strOVC_MILITARY } { strOVC_GIST }";
            bool boolODT_APPLY_DATE = FCommon.checkDateTime(strODT_APPLY_DATE, "結報申請日期", ref strMessage, out DateTime dateODT_APPLY_DATE);
            bool boolODT_INV_DATE = FCommon.checkDateTime(strODT_INV_DATE, "收據日期", ref strMessage, out DateTime dateODT_INV_DATE);
            bool boolCO_SN = Guid.TryParse(strCO_SN, out Guid guidCO_SN);
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                einf = new TBGMT_EINF();
                einf.EINF_SN = Guid.NewGuid();
                einf.OVC_INF_NO = strOVC_INF_NO;
                einf.OVC_GIST = strOVC_GIST;
                einf.OVC_PURPOSE_TYPE = strOVC_PURPOSE_TYPE;
                if (boolODT_APPLY_DATE) einf.ODT_APPLY_DATE = dateODT_APPLY_DATE; else einf.ODT_APPLY_DATE = null;
                einf.OVC_INV_NO = strOVC_INV_NO;
                if (boolODT_INV_DATE) einf.ODT_INV_DATE = dateODT_INV_DATE; else einf.ODT_INV_DATE = null;
                einf.OVC_NOTE = strOVC_NOTE;
                if (boolCO_SN) einf.CO_SN = guidCO_SN; else einf.CO_SN = null;
                einf.OVC_PLN_CONTENT = strOVC_PLN_CONTENT;
                einf.OVC_IS_PAID = "未付款";
                einf.ODT_CREATE_DATE = dateNow;
                einf.OVC_CREATE_LOGIN_ID = strUserId;
                einf.ODT_MODIFY_DATE = dateNow;
                einf.OVC_MODIFY_LOGIN_ID = strUserId;
                MTSE.TBGMT_EINF.Add(einf);
                MTSE.SaveChanges();

                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), einf.GetType().Name.ToString(), this, "新增");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增結報申請表成功，結報申請表編號為" + strOVC_INF_NO);
                FCommon.Controls_Clear(drpOVC_MILITARY_TYPE, txtOVC_GIST, txtOVC_PURPOSE_TYPE, txtOVC_INV_NO, txtODT_INV_DATE, txtOVC_NOTE, drpCO_SN, txtOVC_PLN_CONTENT);

                FCommon.DialogBoxShow(this, "繼續新增結報申請表", "MTS_B23", false);
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        #endregion
    }
}