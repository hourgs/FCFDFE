using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using FCFDFE.Content;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A22_3 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        Guid guidEDF_SN;

        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_REQ_DEPT_CDE", Request.QueryString["OVC_REQ_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_EDF_NO", Request.QueryString["OVC_EDF_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_START_PORT", Request.QueryString["OVC_START_PORT"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_ARRIVE_PORT", Request.QueryString["OVC_ARRIVE_PORT"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", Request.QueryString["OVC_TRANSER_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_IS_STRATEGY", Request.QueryString["OVC_IS_STRATEGY"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_REVIEW_STATUS", Request.QueryString["OVC_REVIEW_STATUS"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_RECEIVE_DATE", Request.QueryString["ODT_RECEIVE_DATE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_PURCH_NO", Request.QueryString["OVC_PURCH_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_ITEM_NO2", Request.QueryString["OVC_ITEM_NO2"], false);
            return strQueryString;
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (FCommon.getQueryString(this, "id", out string id, true) && Guid.TryParse(id, out guidEDF_SN))
                {
                    if (!IsPostBack)
                    {
                        #region 初始值載入
                        TBGMT_EDF edf = MTSE.TBGMT_EDF.Where(table => table.EDF_SN.Equals(guidEDF_SN)).FirstOrDefault();
                        if (edf != null)
                        {
                            string strOVC_EDF_NO = edf.OVC_EDF_NO;
                            string strOVC_REVIEW_LOGIN_Name = FCommon.getUserName(edf.OVC_REVIEW_LOGIN_ID); //取得審核者名字

                            lblOVC_EDF_NO.Text = strOVC_EDF_NO;
                            string strODT_REVIEW_DATE = FCommon.getDateTime(edf.ODT_REVIEW_DATE);
                            if (strODT_REVIEW_DATE.Equals("")) strODT_REVIEW_DATE = " - ";
                            if (edf.OVC_REVIEW_STATUS != null)
                                lbREVIEW.Text = "已於" + strODT_REVIEW_DATE + " 經 " + strOVC_REVIEW_LOGIN_Name + edf.OVC_REVIEW_STATUS;

                            lbOVC_PURCH_NO.Text = edf.OVC_PURCH_NO;
                            string strOVC_START_PORT = edf.OVC_START_PORT ?? "";
                            TBGMT_PORTS startPort = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(strOVC_START_PORT)).FirstOrDefault();
                            if (startPort != null) lbOVC_START_PORT.Text = startPort.OVC_PORT_CHI_NAME;
                            string strOVC_ARRIVE_PORT = edf.OVC_ARRIVE_PORT ?? "";
                            TBGMT_PORTS arrivePort = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE.Equals(strOVC_ARRIVE_PORT)).FirstOrDefault();
                            if (arrivePort != null) lbOVC_ARRIVE_PORT.Text = arrivePort.OVC_PORT_CHI_NAME;
                            txtOVC_SHIP_FROM.Text = edf.OVC_SHIP_FROM;
                            string strOVC_CON_ENG_ADDRESS = edf.OVC_CON_ENG_ADDRESS ?? "";
                            lbOVC_CON_ENG_ADDRESS.Text = strOVC_CON_ENG_ADDRESS.Replace("\r\n", "<br>");
                            lbOVC_CON_TEL.Text = edf.OVC_CON_TEL;
                            lbOVC_CON_FAX.Text = edf.OVC_CON_FAX;
                            string strOVC_NP_ENG_ADDRESS = edf.OVC_NP_ENG_ADDRESS ?? "";
                            lbOVC_NP_ENG_ADDRESS.Text = strOVC_NP_ENG_ADDRESS.Replace("\r\n", "<br>");
                            lbOVC_NP_TEL.Text = edf.OVC_NP_TEL;
                            lbOVC_NP_FAX.Text = edf.OVC_NP_FAX;
                            string strOVC_ANP_ENG_ADDRESS = edf.OVC_ANP_ENG_ADDRESS ?? "";
                            lbOVC_ANP_ENG_ADDRESS.Text = strOVC_ANP_ENG_ADDRESS.Replace("\r\n", "<br>");
                            lbOVC_ANP_TEL.Text = edf.OVC_ANP_TEL;
                            lbOVC_ANP_FAX.Text = edf.OVC_ANP_FAX;
                            string strOVC_ANP_ENG_ADDRESS2 = edf.OVC_ANP_ENG_ADDRESS2 ?? "";
                            lbOVC_ANP_ENG_ADDRESS2.Text = strOVC_ANP_ENG_ADDRESS2.Replace("\r\n", "<br>");
                            lbOVC_ANP_TEL2.Text = edf.OVC_ANP_TEL2;
                            lbOVC_ANP_FAX2.Text = edf.OVC_ANP_FAX2;
                            string strOVC_PAYMENT_TYPE_Text = "";
                            string strOVC_PAYMENT_TYPE = edf.OVC_PAYMENT_TYPE != null ? edf.OVC_PAYMENT_TYPE : "";
                            string strOVC_IS_PAY = edf.OVC_IS_PAY != null ? edf.OVC_IS_PAY : "";
                            if (!strOVC_PAYMENT_TYPE.Equals(string.Empty))
                                if (strOVC_PAYMENT_TYPE.Equals("預付"))
                                    strOVC_PAYMENT_TYPE_Text = "PREPAID 預付 (軍種年度運保費項下支付)";
                                else
                                {
                                    strOVC_PAYMENT_TYPE_Text = "COLLECT 到付 (收貨人支付) " + strOVC_PAYMENT_TYPE;
                                    if (!strOVC_IS_PAY.Equals(string.Empty))
                                        strOVC_PAYMENT_TYPE_Text += strOVC_IS_PAY.Equals("1") ? " 投保" : " 不投保";
                                }
                            lbOVC_PAYMENT_TYPE.Text = strOVC_PAYMENT_TYPE_Text;
                            string strOVC_NOTE = edf.OVC_NOTE ?? "";
                            lbOVC_NOTE.Text = strOVC_NOTE.Replace("\r\n", "<br>");
                            txtOVC_DELIVER_NAME.Text = edf.OVC_DELIVER_NAME;
                            txtOVC_DELIVER_MOBILE.Text = edf.OVC_DELIVER_MOBILE;
                            txtOVC_DELIVER_MILITARY_LINE.Text = edf.OVC_DELIVER_MILITARY_LINE;
                            bool boolOVC_IS_STRATEGY = (edf.OVC_IS_STRATEGY ?? "").Equals("是");
                            bool boolOVC_IS_RISK = (edf.OVC_IS_RISK ?? "").Equals("是");
                            bool boolOVC_IS_ALERTNESS = (edf.OVC_IS_ALERTNESS ?? "").Equals("是");
                            if (boolOVC_IS_STRATEGY)
                            {
                                lbODT_VALIDITY_DATE.Text = FCommon.getDateTime(edf.ODT_VALIDITY_DATE);
                                lbOVC_LICENSE_NO.Text = edf.OVC_LICENSE_NO;
                            }
                            chkOVC_IS_STRATEGY.Checked = boolOVC_IS_STRATEGY;
                            chkOVC_IS_RISK.Checked = boolOVC_IS_RISK;
                            chkOVC_IS_ALERTNESS.Checked = boolOVC_IS_ALERTNESS;
                            pnStrategy.Visible = boolOVC_IS_STRATEGY;
                            lbOVC_NOTE.Text = edf.OVC_NOTE;

                            var queryDetail =
                            from de in MTSE.TBGMT_EDF_DETAIL
                            join c in MTSE.TBGMT_CURRENCY on de.OVC_CURRENCY equals c.OVC_CURRENCY_CODE
                            where de.OVC_EDF_NO.Equals(strOVC_EDF_NO)
                            orderby de.OVC_EDF_ITEM_NO
                            select new
                            {
                                de.EDF_DET_SN,
                                OVC_BOX_NO = de.OVC_BOX_NO,
                                OVC_ENG_NAME = de.OVC_ENG_NAME,
                                OVC_CHI_NAME = de.OVC_CHI_NAME,
                                OVC_ITEM_NO = de.OVC_ITEM_NO,
                                OVC_ITEM_NO2 = de.OVC_ITEM_NO2,
                                OVC_ITEM_NO3 = de.OVC_ITEM_NO3,
                                ONB_ITEM_COUNT = de.ONB_ITEM_COUNT,
                                OVC_ITEM_COUNT_UNIT = de.OVC_ITEM_COUNT_UNIT,
                                ONB_WEIGHT = de.ONB_WEIGHT,
                                OVC_WEIGHT_UNIT = de.OVC_WEIGHT_UNIT,
                                ONB_VOLUME = de.ONB_VOLUME,
                                OVC_VOLUME_UNIT = de.OVC_VOLUME_UNIT,
                                ONB_LENGTH = de.ONB_LENGTH,
                                ONB_WIDTH = de.ONB_WIDTH,
                                ONB_HEIGHT = de.ONB_HEIGHT,
                                ONB_MONEY = de.ONB_MONEY,
                                OVC_CURRENCY_NAME = c.OVC_CURRENCY_NAME,
                                de.OVC_CURRENCY
                            };
                            DataTable dt = CommonStatic.LinqQueryToDataTable(queryDetail);
                            FCommon.GridView_dataImport(GVTBGMT_EDF_DETAIL, dt);
                        }
                        #endregion

                        chkOVC_IS_STRATEGY.Enabled = false;
                        chkOVC_IS_RISK.Enabled = false;
                        chkOVC_IS_ALERTNESS.Enabled = false;
                        //如果已審核過則關閉編輯
                        if (lbREVIEW.Text != "")
                        {
                            //btnPass.Visible = false;
                            //btnFail.Visible = false;
                            //txtOVC_SHIP_FROM.Enabled = false;
                            //txtOVC_DELIVER_NAME.Enabled = false;
                            //txtOVC_DELIVER_MOBILE.Enabled = false;
                            //txtOVC_DELIVER_MILITARY_LINE.Enabled = false;
                        }
                    }
                }
                else
                    FCommon.MessageBoxShow(this, "編號 錯誤！", $"MTS_A22_1{ getQueryString() }", false);
            }
        }

        protected void btnPass_Click(object sender, EventArgs e)
        {
            TBGMT_EDF edf = MTSE.TBGMT_EDF.Where(table => table.EDF_SN.Equals(guidEDF_SN)).FirstOrDefault();
            if (edf != null)
            {
                string strMessage = "";
                string strUserId = Session["userid"].ToString();
                string strOVC_EDF_NO = edf.OVC_EDF_NO;
                string strOVC_PURCH_NO = edf.OVC_PURCH_NO;
                string strOVC_START_PORT = edf.OVC_START_PORT;
                string strOVC_ARRIVE_PORT = edf.OVC_ARRIVE_PORT;
                string strOVC_CON_ENG_ADDRESS = edf.OVC_CON_ENG_ADDRESS;
                string strOVC_CON_TEL = edf.OVC_CON_TEL;
                string strOVC_CON_FAX = edf.OVC_CON_FAX;
                string strOVC_PAYMENT_TYPE = edf.OVC_PAYMENT_TYPE;
                string strOVC_IS_STRATEGY = edf.OVC_PAYMENT_TYPE ?? "否";
                bool boolOVC_IS_STRATEGY = strOVC_IS_STRATEGY.Equals("是");
                string strODT_VALIDITY_DATE = FCommon.getDateTime(edf.ODT_VALIDITY_DATE);
                string strOVC_LICENSE_NO = edf.OVC_LICENSE_NO;

                string strOVC_SHIP_FROM = txtOVC_SHIP_FROM.Text;
                string strOVC_DELIVER_NAME = txtOVC_DELIVER_NAME.Text;
                string strOVC_DELIVER_MOBILE = txtOVC_DELIVER_MOBILE.Text;
                string strOVC_DELIVER_MILITARY_LINE = txtOVC_DELIVER_MILITARY_LINE.Text;
                
                DateTime dateNow = DateTime.Now;
                TBGMT_EDF_DETAIL edf_detail = MTSE.TBGMT_EDF_DETAIL.Where(table => table.OVC_EDF_NO.Equals(strOVC_EDF_NO)).FirstOrDefault();
                #region 必填項目
                if (strOVC_SHIP_FROM.Equals(string.Empty))
                    strMessage += "<P> 請輸入 發貨單位！ </p>";
                if (strOVC_DELIVER_NAME.Equals(string.Empty))
                    strMessage += "<P> 請輸入 發貨人資訊 名字！ </p>";
                if (strOVC_DELIVER_MOBILE.Equals(string.Empty))
                    strMessage += "<P> 請輸入 發貨人資訊 手機！ </p>";
                if (strOVC_DELIVER_MILITARY_LINE.Equals(string.Empty))
                    strMessage += "<P> 請輸入 發貨人資訊 軍線！ </p>";

                if (string.IsNullOrEmpty( strOVC_PURCH_NO))
                    strMessage += "<P> 案號 不得為空！ </p>";
                if(string.IsNullOrEmpty(strOVC_START_PORT))
                    strMessage += "<P> 啟運港(機場) 不得為空！ </p>";
                if(string.IsNullOrEmpty(strOVC_ARRIVE_PORT))
                    strMessage += "<P> 目的港(機場) 不得為空！ </p>";
                if(string.IsNullOrEmpty(strOVC_CON_ENG_ADDRESS))
                    strMessage += "<P> CONSIGNEE 地址 不得為空！ </p>";
                if(string.IsNullOrEmpty(strOVC_CON_TEL))
                    strMessage += "<P> CONSIGNEE 電話 不得為空！ </p>";
                if(string.IsNullOrEmpty(strOVC_CON_FAX))
                    strMessage += "<P> CONSIGNEE 傳真 不得為空！ </p>";
                if(string.IsNullOrEmpty(strOVC_DELIVER_NAME))
                    strMessage += "<P> 發貨人資訊 名字 不得為空！ </p>";
                if(string.IsNullOrEmpty(strOVC_DELIVER_MOBILE))
                    strMessage += "<P> 發貨人資訊 手機 不得為空！ </p>";
                if(string.IsNullOrEmpty(strOVC_DELIVER_MILITARY_LINE))
                    strMessage += "<P> 發貨人資訊 軍線 不得為空！ </p>";
                if (string.IsNullOrEmpty(strOVC_PAYMENT_TYPE))
                    strMessage += "<P> 付款方式 不得為空！ </p>";
                if (boolOVC_IS_STRATEGY)
                {
                    if (string.IsNullOrEmpty(strODT_VALIDITY_DATE))
                        strMessage += "<P> 有效期限 不得為空！ </p>";
                    if (string.IsNullOrEmpty(strOVC_LICENSE_NO))
                        strMessage += "<P> 輸出許可證號碼 不得為空！ </p>";
                }
                if (edf_detail == null)
                    strMessage += "<P> 料件 不得為空！ </p>";
                #endregion

                if (strMessage.Equals(string.Empty))
                {
                    edf.OVC_SHIP_FROM = strOVC_SHIP_FROM;
                    edf.OVC_DELIVER_NAME = strOVC_DELIVER_NAME;
                    edf.OVC_DELIVER_MOBILE = strOVC_DELIVER_MOBILE;
                    edf.OVC_DELIVER_MILITARY_LINE = strOVC_DELIVER_MILITARY_LINE;
                    edf.OVC_REVIEW_STATUS = "通過";
                    edf.OVC_REVIEW_LOGIN_ID = strUserId;
                    edf.ODT_REVIEW_DATE = dateNow;

                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), edf.GetType().Name, this, "修改");
                    strMessage = $"編號： { lblOVC_EDF_NO.Text } 之外運資料表 審核通過成功。";
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", strMessage);
                    FCommon.DialogBoxShow(this, lblOVC_EDF_NO.Text + $"{ strMessage }，繼續管理外運資料表？", $"MTS_A22_1{ getQueryString() }", false);
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "外運資料表 不存在！");
        }
        protected void btnFail_Click(object sender, EventArgs e)
        {
            TBGMT_EDF edf = MTSE.TBGMT_EDF.Where(table => table.EDF_SN.Equals(guidEDF_SN)).FirstOrDefault();
            if (edf != null)
            {
                string strMessage = "";
                string strUserId = Session["userid"].ToString();
                string strOVC_SHIP_FROM = txtOVC_SHIP_FROM.Text;
                string strOVC_DELIVER_NAME = txtOVC_DELIVER_NAME.Text;
                string strOVC_DELIVER_MOBILE = txtOVC_DELIVER_MOBILE.Text;
                string strOVC_DELIVER_MILITARY_LINE = txtOVC_DELIVER_MILITARY_LINE.Text;

                DateTime dateNow = DateTime.Now;
                #region 必填項目
                if (strOVC_SHIP_FROM.Equals(string.Empty))
                    strMessage += "<P> 請輸入 發貨單位！ </p>";
                if (strOVC_DELIVER_NAME.Equals(string.Empty))
                    strMessage += "<P> 請輸入 發貨人資訊 名字！ </p>";
                if (strOVC_DELIVER_MOBILE.Equals(string.Empty))
                    strMessage += "<P> 請輸入 發貨人資訊 手機！ </p>";
                if (strOVC_DELIVER_MILITARY_LINE.Equals(string.Empty))
                    strMessage += "<P> 請輸入 發貨人資訊 軍線！ </p>";
                #endregion

                if (strMessage.Equals(string.Empty))
                {
                    edf.OVC_SHIP_FROM = strOVC_SHIP_FROM;
                    edf.OVC_DELIVER_NAME = strOVC_DELIVER_NAME;
                    edf.OVC_DELIVER_MOBILE = strOVC_DELIVER_MOBILE;
                    edf.OVC_DELIVER_MILITARY_LINE = strOVC_DELIVER_MILITARY_LINE;
                    edf.OVC_REVIEW_STATUS = "剔退";
                    edf.OVC_REVIEW_LOGIN_ID = strUserId;
                    edf.ODT_REVIEW_DATE = dateNow;
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), edf.GetType().Name, this, "修改");

                    strMessage = $"編號： { lblOVC_EDF_NO.Text } 之外運資料表 審核剔退成功。";
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", strMessage);
                    FCommon.DialogBoxShow(this, lblOVC_EDF_NO.Text + $"{ strMessage }，繼續管理外運資料表？", $"MTS_A22_1{ getQueryString() }", false);
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "外運資料表 不存在！");
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_A22_1{ getQueryString() }");
        }

        protected void GVTBGMT_EDF_DETAIL_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void GVTBGMT_EDF_DETAIL_RowCreated(object sender, GridViewRowEventArgs e)
        {
            decimal Quantity = 0, Weight = 0, Volume = 0, Amount = 0;
            string strFormat_Money = "#,0";
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                GridView theGridView = (GridView)sender;
                for (int i = 0; i < theGridView.Rows.Count; i++)
                {
                    GridViewRow gvr = theGridView.Rows[i];
                    Quantity += decimal.TryParse(gvr.Cells[6].Text, out decimal decQuantity) ? decQuantity : 0;
                    Weight += decimal.TryParse(gvr.Cells[8].Text,out decimal decWeight) ? decWeight : 0;
                    Volume += decimal.TryParse(gvr.Cells[10].Text, out decimal decVolume) ? decVolume : 0;
                    Amount += decimal.TryParse(gvr.Cells[13].Text, out decimal decAmount) ? decAmount : 0;
                }

                GridViewRow Tgvr = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Insert);
                TableCell cell1 = new TableCell();
                TableCell cell2 = new TableCell();
                TableCell cell3 = new TableCell();
                TableCell cell4 = new TableCell();
                TableCell cell5 = new TableCell();
                TableCell cell6 = new TableCell();
                TableCell cell7 = new TableCell();
                TableCell cell8 = new TableCell();
                TableCell cell9 = new TableCell();
                TableCell cell10 = new TableCell();
                TableCell cell11 = new TableCell();
                TableCell cell12 = new TableCell();
                TableCell cell13 = new TableCell();
                TableCell cell14 = new TableCell();
                TableCell cell15 = new TableCell();
                cell1.Text = theGridView.Rows.Count.ToString() + "項";
                cell7.Text = Quantity.ToString();
                cell9.Text = Weight.ToString(strFormat_Money);
                cell11.Text = Volume.ToString(strFormat_Money);
                cell14.Text = Amount.ToString(strFormat_Money);
                Tgvr.Controls.Add(cell1);
                Tgvr.Controls.Add(cell2);
                Tgvr.Controls.Add(cell3);
                Tgvr.Controls.Add(cell4);
                Tgvr.Controls.Add(cell5);
                Tgvr.Controls.Add(cell6);
                Tgvr.Controls.Add(cell7);
                Tgvr.Controls.Add(cell8);
                Tgvr.Controls.Add(cell9);
                Tgvr.Controls.Add(cell10);
                Tgvr.Controls.Add(cell11);
                Tgvr.Controls.Add(cell12);
                Tgvr.Controls.Add(cell13);
                Tgvr.Controls.Add(cell14);
                Tgvr.Controls.Add(cell15);

                GVTBGMT_EDF_DETAIL.Controls[0].Controls.Add(Tgvr);
            }
        }

        protected void GVTBGMT_EDF_DETAIL_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}