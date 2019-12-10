using System;
using System.Linq;
using System.Web.UI;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;
using System.Data;

namespace FCFDFE.pages.MTS.B
{
    public partial class MTS_B13 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        Common FCommon = new Common();

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
                    #region 匯入下拉式選單
                    CommonMTS.list_dataImport_DEPT_CLASS(drpOVC_MILITARY_TYPE, true); //軍種
                    //CommonMTS.list_dataImport_COMPANY(drpCO_SN, CommonMTS.COMPANY_TYPE.InsuranceCompany, true); //保險公司
                    var query =
                        from company in MTSE.TBGMT_COMPANY
                        where company.OVC_CO_TYPE.Equals("1")
                        select company;
                    DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                    FCommon.list_dataImport(drpCO_SN, dt, "OVC_COMPANY", "CO_SN", true);
                    #endregion

                    txtOVC_PLN_CONTENT.Text = "擬請准予結報"; //預設文字
                    #region 結報申請表編號
                    string yyy = FCommon.getTaiwanYear(DateTime.Now).ToString("000");
                    string strOVC_IINF_NO = "IINF" + yyy;
                    TBGMT_IINF TBGMTIINF = MTSE.TBGMT_IINF.Where(table => table.OVC_INF_NO.StartsWith(strOVC_IINF_NO)).OrderByDescending(table => table.OVC_INF_NO).FirstOrDefault();

                    int iinfNum = 0;
                    if (TBGMTIINF != null && TBGMTIINF.OVC_INF_NO.Length == 11)
                            int.TryParse(TBGMTIINF.OVC_INF_NO.Substring(7, 4), out iinfNum);
                    iinfNum++;
                    strOVC_IINF_NO += iinfNum.ToString("0000");
                    lblOVC_INF_NO.Text = strOVC_IINF_NO;
                    ViewState["OVC_IINF_NO"] = strOVC_IINF_NO;
                    #endregion
                }
            }
        }

        #region ~Click
        protected void btnNew_Click(object sender, EventArgs e)
        {
            #region  取值
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strOVC_INF_NO = lblOVC_INF_NO.Text;
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedItem != null ? drpOVC_MILITARY_TYPE.SelectedItem.Text : "";
            string strOVC_PURCH_NO = txtOVC_PURCH_NO.Text;
            string strISSU_NO = txtISSU_NO.Text;
            string strOVC_PURPOSE_TYPE = txtOVC_PURPOSE_TYPE.Text;
            string strODT_APPLY_DATE = txtODT_APPLY_DATE.Text;
            string strOVC_INV_NO = txtOVC_INV_NO.Text;
            string strODT_INV_DATE = txtODT_INV_DATE.Text;
            string strOVC_NOTE = txtOVC_NOTE.Text;
            string strCO_SN = drpCO_SN.SelectedValue;
            string strOVC_PLN_CONTENT = txtOVC_PLN_CONTENT.Text;
            DateTime dateNow = DateTime.Now;
            #endregion

            #region 錯誤訊息
            TBGMT_IINF iinf = MTSE.TBGMT_IINF.Where(table=> table.OVC_INF_NO.Equals(strOVC_INF_NO)).FirstOrDefault();
            if(iinf!=null)
                strMessage += $"<P> 編號：{ strOVC_INF_NO } 之結報申請表 已存在！ </p>";
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
            if (strOVC_INV_NO.Equals(string.Empty))
                strMessage += "<P> 請輸入 收據號碼 </p>";
            if (strODT_INV_DATE.Equals(string.Empty))
                strMessage += "<P> 請選擇 收據日期 </p>";
            if (strCO_SN.Equals(string.Empty))
                strMessage += "<P> 請選擇 保險公司</p>";
            if (strOVC_PLN_CONTENT.Equals(string.Empty))
                strMessage += "<P> 請輸入 擬辦</p>";
            
            bool boolODT_APPLY_DATE = FCommon.checkDateTime(strODT_APPLY_DATE, "結報申請日期", ref strMessage, out DateTime dateODT_APPLY_DATE);
            bool boolODT_INV_DATE = FCommon.checkDateTime(strODT_INV_DATE, "收據日期", ref strMessage, out DateTime dateODT_INV_DATE);
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                try
                {
                    iinf = new TBGMT_IINF();
                    iinf.OVC_INF_NO = strOVC_INF_NO;
                    iinf.OVC_GIST = strOVC_MILITARY_TYPE + " " + strOVC_PURCH_NO;
                    iinf.ISSU_NO = strISSU_NO;
                    iinf.OVC_PURPOSE_TYPE = strOVC_PURPOSE_TYPE;
                    iinf.ODT_APPLY_DATE = dateODT_APPLY_DATE;
                    iinf.OVC_INV_NO = strOVC_INV_NO;
                    iinf.ODT_INV_DATE = dateODT_INV_DATE;
                    iinf.OVC_NOTE = strOVC_NOTE;
                    iinf.CO_SN = new Guid(strCO_SN);
                    iinf.OVC_PLN_CONTENT = strOVC_PLN_CONTENT;
                    iinf.ODT_CREATE_DATE = dateNow;
                    iinf.OVC_CREATE_LOGIN_ID = strUserId;
                    iinf.ODT_MODIFY_DATE = dateNow;
                    iinf.OVC_MODIFY_LOGIN_ID = strUserId;
                    iinf.IINF_SN = Guid.NewGuid();
                    iinf.OVC_IS_PAID = "未付款";
                    MTSE.TBGMT_IINF.Add(iinf);

                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), iinf.GetType().Name.ToString(), this, "新增");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", $"編號：{ strOVC_INF_NO } 之結報申請表 新增成功。");
                }
                catch
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "新增失敗，請聯絡工程師！");
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        #endregion
    }
}