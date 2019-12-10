using System;
using System.Linq;
using System.Web.UI;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A24_3 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        Common FCommon = new Common();

        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_EDF_NO", Request.QueryString["OVC_EDF_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", Request.QueryString["OVC_TRANSER_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_START_PORT", Request.QueryString["OVC_START_PORT"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_ARRIVE_PORT", Request.QueryString["OVC_ARRIVE_PORT"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_SHIP_COMPANY", Request.QueryString["OVC_SHIP_COMPANY"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_SO_NO", Request.QueryString["OVC_SO_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_STORED_COMPANY", Request.QueryString["OVC_STORED_COMPANY"], false);
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
                    if (FCommon.getQueryString(this, "id", out string strEDF_SN, true) && Guid.TryParse(strEDF_SN, out Guid guidEDF_SN))
                    {
                        TBGMT_EDF edf = MTSE.TBGMT_EDF.Where(table => table.EDF_SN.Equals(guidEDF_SN)).FirstOrDefault();
                        if (edf != null)
                        {
                            string strOVC_EDF_NO = edf.OVC_EDF_NO;
                            lblOVC_EDF_NO.Text = strOVC_EDF_NO;
                            lblOVC_PURCH_NO.Text = edf.OVC_PURCH_NO;

                            TBGMT_PORTS port_start = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(edf.OVC_START_PORT)).FirstOrDefault();
                            if (port_start != null)
                                lblOVC_START_PORT.Text = port_start.OVC_PORT_CHI_NAME;

                            TBGMT_PORTS port_arr = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(edf.OVC_ARRIVE_PORT)).FirstOrDefault();
                            if (port_arr != null)
                                lblOVC_ARRIVE_PORT.Text = port_arr.OVC_PORT_CHI_NAME;

                            TBGMT_ESO eso = MTSE.TBGMT_ESO.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();
                            if (eso != null)
                            {
                                lblOVC_PURCH_MSG_NO1.Text = eso.OVC_PURCH_MSG_NO1;
                                lblOVC_PURCH_MSG_NO2.Text = eso.OVC_PURCH_MSG_NO2;
                                lblOVC_PURCH_MSG_NO3.Text = eso.OVC_PURCH_MSG_NO3;
                                lblOVC_PROCESS_NO1.Text = eso.OVC_PROCESS_NO1 ?? "O";
                                lblOVC_PROCESS_NO2.Text = eso.OVC_PROCESS_NO2 ?? "O";
                                lblOVC_PROCESS_NO3.Text = eso.OVC_PROCESS_NO3 ?? "O";
                                lblOVC_SHIP_COMPANY.Text = eso.OVC_SHIP_COMPANY;
                                lblOVC_SO_NO.Text = eso.OVC_SO_NO;
                                lblOVC_BLD_NO.Text = eso.OVC_BLD_NO;
                                lblOVC_SHIP_NAME.Text = eso.OVC_SHIP_NAME;
                                lblOVC_VOYAGE.Text = eso.OVC_VOYAGE;
                                lblOVC_CONTAINER_TYPE.Text = eso.OVC_CONTAINER_TYPE;
                                lblONB_20_COUNT.Text = eso.ONB_20_COUNT.ToString();
                                lblONB_40_COUNT.Text = eso.ONB_40_COUNT.ToString();
                                lblONB_45_COUNT.Text = eso.ONB_45_COUNT.ToString();
                                lblODT_STORED_DATE.Text = FCommon.getDateTime(eso.ODT_STORED_DATE);
                                lblODT_START_DATE.Text = FCommon.getDateTime(eso.ODT_START_DATE);
                                lblODT_ACT_ARRIVE_DATE.Text = FCommon.getDateTime(eso.ODT_ACT_ARRIVE_DATE);
                                lblOVC_STORED_PLACE.Text = eso.OVC_CUSTOM_CLR_PLACE;
                                lblODT_CUSTOM_CLR_DATE.Text = FCommon.getDateTime(eso.ODT_CUSTOM_CLR_DATE);
                                lblOVC_STORED_COMPANY.Text = eso.OVC_STORED_COMPANY;
                                ViewState["ESO_SN"] = eso.ESO_SN;
                            }
                        }
                    }
                    else
                        FCommon.MessageBoxShow(this, "外運資料表編號 錯誤！", $"MTS_A24_1{ getQueryString() }", false);
                }
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            Guid eso_SN = (Guid)ViewState["ESO_SN"];
            try
            {
                string strUserId = Session["userid"].ToString();
                TBGMT_ESO esoDel = MTSE.TBGMT_ESO.Where(table => table.ESO_SN == eso_SN).Where(table => table.OVC_EDF_NO == lblOVC_EDF_NO.Text).FirstOrDefault();
                if (esoDel != null)
                {
                    string strOVC_EDF_NO = esoDel.OVC_EDF_NO;
                    MTSE.Entry(esoDel).State = EntityState.Deleted;
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), esoDel.GetType().Name, this, "刪除");

                    FCommon.AlertShow(PnDelete, "success", "系統訊息", $"編號：{ strOVC_EDF_NO } 訂艙單 刪除成功。");
                }
                else
                    FCommon.AlertShow(PnDelete, "danger", "系統訊息", "訂艙單 不存在，已被刪除！");
            }
            catch
            {
                FCommon.AlertShow(PnDelete, "danger", "系統訊息", "刪除失敗，請聯絡工程師。");
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_A24_1{ getQueryString() }");
        }
    }
}