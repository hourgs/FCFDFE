using System;
using System.Linq;
using System.Web.UI;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A27_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        Common FCommon = new Common();
        string id;

        #region 副程式
        private void dataImport()
        {
            var query =
                from edf in MTSE.TBGMT_EDF
                join edf_detail in MTSE.TBGMT_EDF_DETAIL on edf.OVC_EDF_NO equals edf_detail.OVC_EDF_NO
                //from edf in MTSE.TBGMT_EDF.AsEnumerable()
                //join edf_detail in MTSE.TBGMT_EDF_DETAIL.AsEnumerable() on edf.OVC_EDF_NO equals edf_detail.OVC_EDF_NO
                where edf.OVC_BLD_NO.Equals(id)
                join currency in MTSE.TBGMT_CURRENCY on edf_detail.OVC_CURRENCY equals currency.OVC_CURRENCY_CODE into currencyTemp
                from currency in currencyTemp.DefaultIfEmpty()
                select new
                {
                    OVC_ENG_NAME = edf_detail.OVC_ENG_NAME,
                    OVC_CHI_NAME = edf_detail.OVC_CHI_NAME,
                    OVC_ITEM_NO = edf_detail.OVC_ITEM_NO,
                    OVC_ITEM_NO2 = edf_detail.OVC_ITEM_NO2,
                    OVC_ITEM_NO3 = edf_detail.OVC_ITEM_NO3,
                    ONB_ITEM_COUNT = edf_detail.ONB_ITEM_COUNT,
                    OVC_ITEM_COUNT_UNIT = edf_detail.OVC_ITEM_COUNT_UNIT,
                    ONB_WEIGHT = edf_detail.ONB_WEIGHT,
                    OVC_WEIGHT_UNIT = edf_detail.OVC_WEIGHT_UNIT,
                    ONB_VOLUME = edf_detail.ONB_VOLUME,
                    OVC_VOLUME_UNIT = edf_detail.OVC_VOLUME_UNIT,
                    ONB_BULK = (edf_detail.ONB_LENGTH) * (edf_detail.ONB_WIDTH) * (edf_detail.ONB_HEIGHT),
                    ONB_MONEY = edf_detail.ONB_MONEY,
                    OVC_CURRENCY = currency != null ? currency.OVC_CURRENCY_NAME : ""
                };

            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_EDF_DETAIL, dt);
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
                    FCommon.Controls_Attributes("readonly", "true", txtODT_EXP_DATE);
                    dataImport();
                }
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strOVC_CLASS_CDE = txtOVC_CLASS_CDE.Text;
            string strOVC_CLASS_NAME = txtOVC_CLASS_NAME.Text;
            string strOVC_ECL_NO = txtOVC_ECL_NO.Text;
            string strOVC_EXP_TYPE = txtOVC_EXP_TYPE.Text;
            string strOVC_SHIP_CDE = txtOVC_SHIP_CDE.Text;
            string strOVC_PACK_NO = txtOVC_PACK_NO.Text;
            string strODT_EXP_DATE = txtODT_EXP_DATE.Text;
            string strOVC_STORED_PLACE = txtOVC_STORED_PLACE.Text;

            DateTime dateODT_EXP_DATE, dateNow = DateTime.Now;

            #region 錯誤訊息 
            if (strOVC_CLASS_CDE.Equals(string.Empty))
                strMessage += "<P> 請輸入 類別代號 </p>";
            if (strOVC_CLASS_NAME.Equals(string.Empty))
                strMessage += "<P> 請輸入 類別名稱 </p>";
            if (strOVC_ECL_NO.Equals(string.Empty))
                strMessage += "<P> 請輸入 報單號碼 </p>";
            if (strOVC_EXP_TYPE.Equals(string.Empty))
                strMessage += "<P> 請輸入 出口關別 </p>";
            if (strOVC_SHIP_CDE.Equals(string.Empty))
                strMessage += "<P> 請輸入 船或關代號 </p>";
            if (strOVC_PACK_NO.Equals(string.Empty))
                strMessage += "<P> 請輸入 裝貨單或收序號 </p>";
            if (strODT_EXP_DATE.Equals(string.Empty))
                strMessage += "<P> 請輸入 報關日期 </p>";
            if (strOVC_STORED_PLACE.Equals(string.Empty))
                strMessage += "<P> 請輸入 貨物存放處所 </p>";

            //確認輸入型態
            bool boolODT_EXP_DATE = FCommon.checkDateTime(strODT_EXP_DATE, "報關日期", ref strMessage, out dateODT_EXP_DATE);
            #endregion

            TBGMT_ECL ecl = MTSE.TBGMT_ECL.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
            if (ecl != null)
                FCommon.MessageBoxShow(this, $"提單編號：{ id } 之出口報單 已存在！", $"MTS_A27_1{ getQueryString() }", false);
            else if (strMessage.Equals(string.Empty))
            {
                try
                {
                    ecl = new TBGMT_ECL();
                    ecl.OVC_BLD_NO = id;
                    ecl.OVC_CLASS_CDE = strOVC_CLASS_CDE;
                    ecl.OVC_CLASS_NAME = strOVC_CLASS_NAME;
                    ecl.OVC_ECL_NO = strOVC_ECL_NO;
                    ecl.OVC_EXP_TYPE = strOVC_EXP_TYPE;
                    ecl.OVC_SHIP_CDE = strOVC_SHIP_CDE;
                    ecl.OVC_PACK_NO = strOVC_PACK_NO;
                    ecl.ODT_EXP_DATE = dateODT_EXP_DATE;
                    ecl.OVC_STORED_PLACE = strOVC_STORED_PLACE;
                    ecl.ODT_MODIFY_DATE = dateNow;
                    ecl.OVC_CREATE_LOGIN_ID = strUserId;
                    ecl.ECL_SN = Guid.NewGuid();

                    MTSE.TBGMT_ECL.Add(ecl);
                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), ecl.GetType().Name, this, "新增");

                    FCommon.AlertShow(PnWarning, "success", "系統訊息", $"提單編號：{ id } 之出口報單 新增成功");
                    btnBack.Visible = true;
                }
                catch
                {
                    FCommon.AlertShow(PnWarning, "danger", "系統訊息", "新增失敗，請聯絡工程師。");
                }
            }
            else
                FCommon.AlertShow(PnWarning, "danger", "系統訊息", strMessage);
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_A27_1{ getQueryString() }");
        }

        protected void GVTBGMT_EDF_DETAIL_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            decimal? decITEM_COUNT = 0, decWEIGHT = 0, decVOLUME = 0, decBULK = 0, decMONEY = 0;
            var query =
                from edf in MTSE.TBGMT_EDF
                join edf_detail in MTSE.TBGMT_EDF_DETAIL on edf.OVC_EDF_NO equals edf_detail.OVC_EDF_NO
                //from edf in MTSE.TBGMT_EDF.AsEnumerable()
                //join edf_detail in MTSE.TBGMT_EDF_DETAIL.AsEnumerable() on edf.OVC_EDF_NO equals edf_detail.OVC_EDF_NO
                where edf.OVC_BLD_NO.Equals(id)
                join currency in MTSE.TBGMT_CURRENCY on edf_detail.OVC_CURRENCY equals currency.OVC_CURRENCY_CODE into currencyTemp
                from currency in currencyTemp.DefaultIfEmpty()
                select new
                {
                    ONB_ITEM_COUNT = edf_detail.ONB_ITEM_COUNT,
                    ONB_WEIGHT = edf_detail.ONB_WEIGHT,
                    ONB_VOLUME = edf_detail.ONB_VOLUME,
                    ONB_BULK = (edf_detail.ONB_LENGTH) * (edf_detail.ONB_WIDTH) * (edf_detail.ONB_HEIGHT),
                    ONB_MONEY = edf_detail.ONB_MONEY,
                };
            foreach (var q in query)
            {
                decITEM_COUNT += q.ONB_ITEM_COUNT;
                decWEIGHT += q.ONB_WEIGHT;
                decVOLUME += q.ONB_VOLUME;
                decBULK += q.ONB_BULK;
                decMONEY += q.ONB_MONEY;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
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

                cell1.Text = "總計";
                cell2.Text = theGridView.Rows.Count + "項";
                cell7.Text = decITEM_COUNT.ToString();
                cell9.Text = String.Format("{0:N}", decWEIGHT);
                cell11.Text = String.Format("{0:N}", decVOLUME);
                cell13.Text = decBULK.ToString();
                cell14.Text = String.Format("{0:N}", decMONEY);

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