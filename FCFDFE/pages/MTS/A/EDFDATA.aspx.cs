using System;
using System.Linq;
using System.Web.UI;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.A
{
    public partial class EDFDATA : Page
    {        
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                if (FCommon.getQueryString(this, "id", out string strEDF_SN, true) && Guid.TryParse(strEDF_SN, out Guid guidEDF_SN))
                {
                    #region 初始值載入
                    TBGMT_EDF edf = MTSE.TBGMT_EDF.Where(table => table.EDF_SN.Equals(guidEDF_SN)).FirstOrDefault();
                    if (edf != null)
                    {
                        string strOVC_EDF_NO = edf.OVC_EDF_NO;

                        lblOVC_EDF_NO.Text = strOVC_EDF_NO;
                        lblOVC_PURCH_NO.Text = edf.OVC_PURCH_NO;
                        string strOVC_START_PORT = edf.OVC_START_PORT ?? "";
                        TBGMT_PORTS startPort = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(strOVC_START_PORT)).FirstOrDefault();
                        if (startPort != null) lblOVC_START_PORT.Text = startPort.OVC_PORT_CHI_NAME;
                        string strOVC_ARRIVE_PORT = edf.OVC_ARRIVE_PORT ?? "";
                        TBGMT_PORTS arrivePort = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(strOVC_ARRIVE_PORT)).FirstOrDefault();
                        if (arrivePort != null) lblOVC_ARRIVE_PORT.Text = arrivePort.OVC_PORT_CHI_NAME;
                        string strOVC_SHIP_FROM = edf.OVC_SHIP_FROM ?? "";
                        lblOVC_SHIP_FROM.Text = strOVC_SHIP_FROM.Replace("\r\n", "<br>");
                        string strOVC_CON_ENG_ADDRESS = edf.OVC_CON_ENG_ADDRESS ?? "";
                        lblOVC_CON_ENG_ADDRESS.Text = strOVC_CON_ENG_ADDRESS.Replace("\r\n", "<br>");
                        lblOVC_CON_TEL.Text = edf.OVC_CON_TEL;
                        lblOVC_CON_FAX.Text = edf.OVC_CON_FAX;
                        string strOVC_NP_ENG_ADDRESS = edf.OVC_NP_ENG_ADDRESS ?? "";
                        lblOVC_NP_ENG_ADDRESS.Text = strOVC_NP_ENG_ADDRESS.Replace("\r\n", "<br>");
                        lblOVC_NP_TEL.Text = edf.OVC_NP_TEL;
                        lblOVC_NP_FAX.Text = edf.OVC_NP_FAX;
                        string strOVC_ANP_ENG_ADDRESS = edf.OVC_ANP_ENG_ADDRESS ?? "";
                        lblOVC_ANP_ENG_ADDRESS.Text = strOVC_ANP_ENG_ADDRESS.Replace("\r\n", "<br>");
                        lblOVC_ANP_TEL.Text = edf.OVC_ANP_TEL;
                        lblOVC_ANP_FAX.Text = edf.OVC_ANP_FAX;
                        string strOVC_ANP_ENG_ADDRESS2 = edf.OVC_ANP_ENG_ADDRESS2 ?? "";
                        lblOVC_ANP_ENG_ADDRESS2.Text = strOVC_ANP_ENG_ADDRESS2.Replace("\r\n", "<br>");
                        lblOVC_ANP_TEL2.Text = edf.OVC_ANP_TEL2;
                        lblOVC_ANP_FAX2.Text = edf.OVC_ANP_FAX2;
                        string strOVC_PAYMENT_TYPE = edf.OVC_PAYMENT_TYPE != null ? edf.OVC_PAYMENT_TYPE : "";
                        string strOVC_IS_PAY = edf.OVC_IS_PAY != null ? edf.OVC_IS_PAY : "";
                        string strOVC_PAYMENT_TYPE_Text = "";
                        if (!strOVC_PAYMENT_TYPE.Equals(string.Empty))
                            if (strOVC_PAYMENT_TYPE.Equals("預付"))
                                strOVC_PAYMENT_TYPE_Text = "PREPAID 預付 (軍種年度運保費項下支付)";
                            else
                            {
                                strOVC_PAYMENT_TYPE_Text = "COLLECT 到付 (收貨人支付) " + strOVC_PAYMENT_TYPE;
                                if (!strOVC_IS_PAY.Equals(string.Empty))
                                    strOVC_PAYMENT_TYPE_Text += strOVC_IS_PAY.Equals("1") ? " 投保" : " 不投保";
                            }
                        lblOVC_PAYMENT_TYPE.Text = strOVC_PAYMENT_TYPE_Text;
                        string strOVC_NOTE = edf.OVC_NOTE ?? "";
                        lblOVC_NOTE.Text = strOVC_NOTE.Replace("\r\n", "<br>");
                        lblOVC_DELIVER_NAME.Text = edf.OVC_DELIVER_NAME;
                        lblOVC_DELIVER_MOBILE.Text = edf.OVC_DELIVER_MOBILE;
                        lblOVC_DELIVER_MILITARY_LINE.Text = edf.OVC_DELIVER_MILITARY_LINE;
                        bool boolOVC_IS_STRATEGY = (edf.OVC_IS_STRATEGY ?? "").Equals("是");
                        if (boolOVC_IS_STRATEGY)
                        {
                            lblODT_VALIDITY_DATE.Text = FCommon.getDateTime(edf.ODT_VALIDITY_DATE);
                            lblOVC_LICENSE_NO.Text = edf.OVC_LICENSE_NO;
                        }
                        chkOVC_IS_STRATEGY.Checked = boolOVC_IS_STRATEGY;
                        pnStrategy.Visible = boolOVC_IS_STRATEGY;
                        lblOVC_NOTE.Text = edf.OVC_NOTE;

                        var queryDetail =
                        from detail in MTSE.TBGMT_EDF_DETAIL
                        where detail.OVC_EDF_NO.Equals(strOVC_EDF_NO)
                        join currency in MTSE.TBGMT_CURRENCY on detail.OVC_CURRENCY equals currency.OVC_CURRENCY_CODE into currencyTemp
                        from currency in currencyTemp.DefaultIfEmpty()
                        orderby detail.OVC_EDF_ITEM_NO
                        select new
                        {
                            detail.EDF_DET_SN,
                            detail.OVC_BOX_NO,
                            detail.OVC_ENG_NAME,
                            detail.OVC_CHI_NAME,
                            detail.OVC_ITEM_NO,
                            detail.OVC_ITEM_NO2,
                            detail.OVC_ITEM_NO3,
                            detail.ONB_ITEM_COUNT,
                            detail.OVC_ITEM_COUNT_UNIT,
                            detail.ONB_WEIGHT,
                            detail.OVC_WEIGHT_UNIT,
                            detail.ONB_VOLUME,
                            detail.OVC_VOLUME_UNIT,
                            detail.ONB_LENGTH,
                            detail.ONB_WIDTH,
                            detail.ONB_HEIGHT,
                            detail.ONB_MONEY,
                            OVC_CURRENCY_NAME = currency != null ? currency.OVC_CURRENCY_NAME : "",
                            detail.OVC_CURRENCY
                        };
                        DataTable dt = CommonStatic.LinqQueryToDataTable(queryDetail);
                        FCommon.GridView_dataImport(GV_TBGMT_EDF_DETAIL, dt);
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "此 外運資料表 不存在！");
                    #endregion
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "編號 錯誤！");
        }
        
        //protected void btnclose_Click(object sender, EventArgs e)
        //{
        //    Response.Write("<script language='javascript'>window.close();</" + "script>");
        //}

        protected void GV_TBGMT_EDF_DETAIL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = e.Row;
            switch (gvr.RowType)
            {
                case DataControlRowType.DataRow:
                    break;
                case DataControlRowType.Footer:
                    gvr.Cells[0].Text = "總計";
                    gvr.Cells[1].Text = "共" + theGridView.Rows.Count + "項";
                    int sumONB_ITEM_COUNT = 0;
                    decimal sumONB_WEIGHT = 0, sumONB_VOLUME = 0, sumONB_MONEY = 0;
                    if (theGridView.Rows.Count != 0)
                    {
                        for (int i = 0; i < theGridView.Rows.Count; i++)
                        {
                            GridViewRow theRow = theGridView.Rows[i];
                            string strONB_ITEM_COUNT = theRow.Cells[6].Text;
                            int intTemp;
                            if (int.TryParse(strONB_ITEM_COUNT, out intTemp))
                                sumONB_ITEM_COUNT += intTemp;
                            string strONB_WEIGHT = theRow.Cells[8].Text;
                            string strONB_VOLUME = theRow.Cells[10].Text;
                            string strONB_MONEY = theRow.Cells[13].Text;
                            decimal decTemp;
                            if (decimal.TryParse(strONB_WEIGHT, out decTemp))
                                sumONB_WEIGHT += decTemp;
                            if (decimal.TryParse(strONB_VOLUME, out decTemp))
                                sumONB_VOLUME += decTemp;
                            if (decimal.TryParse(strONB_MONEY, out decTemp))
                                sumONB_MONEY += decTemp;
                        }
                    }
                    gvr.Cells[6].Text = sumONB_ITEM_COUNT.ToString();
                    gvr.Cells[8].Text = sumONB_WEIGHT.ToString();
                    gvr.Cells[10].Text = sumONB_VOLUME.ToString();
                    gvr.Cells[13].Text = sumONB_MONEY.ToString();

                    GridViewRow firstRow = theGridView.Rows[0];
                    gvr.Cells[7].Text = firstRow.Cells[7].Text;
                    gvr.Cells[9].Text = firstRow.Cells[9].Text;
                    gvr.Cells[11].Text = firstRow.Cells[11].Text;
                    gvr.Cells[14].Text = firstRow.Cells[14].Text;
                    break;
                default:
                    break;
            }
        }
        protected void GV_TBGMT_EDF_DETAIL_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            Response.Write("<script language='javascript'>window.close();</" + "script>");
        }
    }
}