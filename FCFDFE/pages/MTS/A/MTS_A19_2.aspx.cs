using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.Collections;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A19_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        Common FCommon = new Common();

        #region 副程式
        private void dataImport(string strOVC_BLD_NO)
        {
            if (!strOVC_BLD_NO.Equals(string.Empty))
            {
                TBGMT_BLD bld = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                if (bld != null)
                {

                    var table =
                        from t1407 in GME.TBM1407
                        where t1407.OVC_PHR_CATE.Equals("TR")
                        where t1407.OVC_PHR_ID.Equals(bld.OVC_ARRIVE_PORT)
                        select t1407;
                    var tableProt = table.FirstOrDefault();
                    if (tableProt != null)
                        FCommon.list_setValue(drpOVC_TRANSER_DEPT_CDE, tableProt.OVC_PHR_PARENTS);
                    if (drpOVC_TRANSER_DEPT_CDE.SelectedValue.Contains("高雄"))
                        txtOVC_START_PLACE.Text = "高雄";
                    if (drpOVC_TRANSER_DEPT_CDE.SelectedValue.Contains("基隆"))
                        txtOVC_START_PLACE.Text = "基隆";
                    if (drpOVC_TRANSER_DEPT_CDE.SelectedValue.Contains("桃園"))
                        txtOVC_START_PLACE.Text = "桃園";
                    txtOVC_SHIP_NAME.Text = bld.OVC_SHIP_NAME;
                    txtOVC_VOYAGE.Text = bld.OVC_VOYAGE;

                    string strOVC_QUANITY_UNIT = bld.OVC_QUANITY_UNIT;
                    FCommon.list_setValue(drpOVC_QUANITY_UNIT, strOVC_QUANITY_UNIT);
                    bool isOther = !drpOVC_QUANITY_UNIT.SelectedValue.Equals(strOVC_QUANITY_UNIT);
                    if (isOther)
                    {
                        FCommon.list_setValue(drpOVC_QUANITY_UNIT, "其他");
                        txtOVC_QUANITY_UNIT.Text = strOVC_QUANITY_UNIT;
                    }
                    txtOVC_QUANITY_UNIT.Visible = isOther;
                    FCommon.list_setValue(drpOVC_VOLUME_UNIT, bld.OVC_VOLUME_UNIT);
                    if (bld.OVC_WEIGHT_UNIT == "KG")
                        FCommon.list_setValue(drpOVC_WEIGHT_UNIT, "公斤");
                }
                TBGMT_ICR icr = MTSE.TBGMT_ICR.Where(table => table.OVC_BLD_NO.Equals(strOVC_BLD_NO)).FirstOrDefault();
                if (icr != null)
                {
                    FCommon.list_setValue(drpOVC_IMPORT_TRANS_TYPE, icr.OVC_TRANS_TYPE);
                    string strOVC_DEPT_CDE = icr.OVC_RECEIVE_DEPT_CODE;
                    txtOVC_RECEIVE_DEPT_CDE.Value = strOVC_DEPT_CDE;
                    TBMDEPT tbmdept = GME.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(strOVC_DEPT_CDE)).FirstOrDefault();
                    if (tbmdept != null) txtOVC_ONNAME.Text = tbmdept.OVC_ONNAME;
                    txtODT_START_DATE.Text = FCommon.getDateTime(icr.ODT_TRANSFER_DATE); //啟運時間：清運日期
                    txtODT_ARRIVE_DATE.Text = FCommon.getDateTime(icr.ODT_RECEIVE_DATE); //抵運時間：接收日期
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "提單編號錯誤！");
        }
        private void dataImport_GVTBGMT_IRD_DETAIL(bool boolImport)
        {
            string[] checkID = (string[])ViewState["checkID"];
            var query =
                from bld in MTSE.TBGMT_BLD.AsEnumerable()
                join irdtail in MTSE.TBGMT_IRD_DETAIL.AsEnumerable() on bld.OVC_BLD_NO equals irdtail.OVC_BLD_NO
                //join arrport in MTSE.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals arrport.OVC_PORT_CDE
                //where arrport.OVC_IS_ABROAD.Equals("國內")
                where checkID.Contains(irdtail.OVC_IRDDETAIL_SN.ToString())
                select new
                {
                    OVC_IRDDETAIL_SN = irdtail.OVC_IRDDETAIL_SN,
                    OVC_BLD_NO = bld.OVC_BLD_NO,
                    OVC_PURCH_NO = irdtail.OVC_PURCH_NO,
                    OVC_ITEM_TYPE = bld.OVC_ITEM_TYPE,
                    OVC_BOX_NO = irdtail.OVC_BOX_NO,
                    ONB_ACTUAL_RECEIVE = irdtail.ONB_ACTUAL_RECEIVE,

                    bld.ONB_QUANITY,
                    bld.ONB_VOLUME,
                    bld.ONB_WEIGHT
                };

            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_IRD_DETAIL, dt);

            //取得提單資料，帶入上方為預設
            if (!IsPostBack) //判斷為第一次進入
            {
                //取得第一筆之提單編號，帶入資料
                if (boolImport && dt.Rows.Count > 0)
                {
                    string strBLD_ID = dt.Rows[0]["OVC_BLD_NO"].ToString();
                    dataImport(strBLD_ID);
                }

                decimal decONB_TOTAL_QUANITY = 0, decONB_TOTAL_VOLUME = 0, decONB_TOTAL_WEIGHT = 0;
                ArrayList aryBLD_ID = new ArrayList();
                //計算累計 件數、體積、重量 (重複提單不做重複計算)
                foreach (DataRow dr in dt.Rows)
                {
                    string strBLD_ID = dr["OVC_BLD_NO"].ToString();
                    if (!aryBLD_ID.Contains(strBLD_ID)) //判斷該提單是否已加入
                    {
                        aryBLD_ID.Add(strBLD_ID); //加入以計算之提單編號
                        decimal.TryParse(dr["ONB_QUANITY"].ToString(), out decimal decONB_QUANITY);
                        decimal.TryParse(dr["ONB_VOLUME"].ToString(), out decimal decONB_VOLUME);
                        decimal.TryParse(dr["ONB_WEIGHT"].ToString(), out decimal decONB_WEIGHT);
                        decONB_TOTAL_QUANITY += decONB_QUANITY;
                        decONB_TOTAL_VOLUME += decONB_VOLUME;
                        decONB_TOTAL_WEIGHT += decONB_WEIGHT;
                    }
                }
                txtONB_TOTAL_QUANITY.Text = decONB_TOTAL_QUANITY.ToString();
                txtONB_TOTAL_VOLUME.Text = decONB_TOTAL_VOLUME.ToString();
                txtONB_TOTAL_WEIGHT.Text = decONB_TOTAL_WEIGHT.ToString();
            }
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", Request.QueryString["OVC_BLD_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_DEPT_CDE", Request.QueryString["OVC_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", Request.QueryString["OVC_TRANSER_DEPT_CDE"], false);
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
                    FCommon.Controls_Attributes("readonly", "true", txtODT_START_DATE, txtODT_ARRIVE_DATE, txtOVC_ONNAME);
                    #region 匯入下拉式選單
                    CommonMTS.list_dataImport_TRANS_TYPE(drpOVC_IMPORT_TRANS_TYPE, false); //清運方式
                    CommonMTS.list_dataImport_TransferArea(drpOVC_TRANSER_DEPT_CDE, true); //接轉地區
                    CommonMTS.list_dataImport_TransferArea(drpOVC_START_PLACE, true); //啟運地點
                    CommonMTS.list_dataImport_QUANITY_UNIT(drpOVC_QUANITY_UNIT, true); //件數單位
                    CommonMTS.list_dataImport_VOLUME_UNIT(drpOVC_VOLUME_UNIT, true); //體積單位
                    CommonMTS.list_dataImport_WEIGHT_UNIT2(drpOVC_WEIGHT_UNIT, true); //重量單位
                    #endregion

                    FCommon.getQueryString(this, "id", out string id, true);
                    string[] checkID = id.Split(' ');
                    ViewState["checkID"] = checkID; //最後一筆是空值，不須新增
                    //string bld_id = checkID[0].ToString();
                    //string arrcde = Request.QueryString["arrcde"].ToString();
                    //string[] cdeArray = arrcde.Split(' ');
                    //ViewState["cdeArray"] = cdeArray;
                    dataImport_GVTBGMT_IRD_DETAIL(true);
                }
            }
        }

        protected void btnResetOVC_DEPT_CDE_CODE_Click(object sender, EventArgs e)
        {
            txtOVC_RECEIVE_DEPT_CDE.Value = string.Empty;
            txtOVC_ONNAME.Text = string.Empty;
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strUserDept = FCommon.getAccountDEPT(this); ;
            string strOVC_IMPORT_TRANS_TYPE = drpOVC_IMPORT_TRANS_TYPE.SelectedValue;
            string strOVC_START_PLACE = txtOVC_START_PLACE.Text; //txtOVC_START_PLACE.Text;
            string strOVC_ARRIVE_PLACE = txtOVC_ARRIVE_PLACE.Text;
            string strODT_START_DATE = txtODT_START_DATE.Text;
            string strODT_ARRIVE_DATE = txtODT_ARRIVE_DATE.Text;
            string strOVC_RECEIVE_DEPT_CDE = txtOVC_RECEIVE_DEPT_CDE.Value;
            string strOVC_TRANSER_DEPT_CDE = drpOVC_TRANSER_DEPT_CDE.SelectedValue;
            string strONB_TOTAL_QUANITY = txtONB_TOTAL_QUANITY.Text;
            string strOVC_QUANITY_UNIT = drpOVC_QUANITY_UNIT.SelectedValue;
            if (strOVC_QUANITY_UNIT.Equals("其他")) strOVC_QUANITY_UNIT = txtOVC_QUANITY_UNIT.Text; //若選擇其他，則取得輸入之資料
            string strONB_TOTAL_VOLUME = txtONB_TOTAL_VOLUME.Text;
            string strOVC_VOLUME_UNIT = drpOVC_VOLUME_UNIT.SelectedValue;
            string strONB_TOTAL_WEIGHT = txtONB_TOTAL_WEIGHT.Text;
            string strOVC_WEIGHT_UNIT = drpOVC_WEIGHT_UNIT.SelectedValue;
            string strOVC_SHIP_NAME = txtOVC_SHIP_NAME.Text;
            string strOVC_VOYAGE = txtOVC_VOYAGE.Text;
            string strOVC_NOTE = txtOVC_NOTE.Text;

            DateTime dateNow = DateTime.Now;

            #region 錯誤訊息            
            if (GVTBGMT_IRD_DETAIL.EditIndex != -1)
                strMessage += "<P> 請先更新或取消 購案號 之編輯 </p>";
            if (strOVC_IMPORT_TRANS_TYPE.Equals(string.Empty))
                strMessage += "<P> 請輸入 運輸方法 </p>";
            if (strOVC_START_PLACE.Equals(string.Empty))
                strMessage += "<P> 請輸入 啟運地點 </p>";
            if (strOVC_ARRIVE_PLACE.Equals(string.Empty))
                strMessage += "<P> 請輸入 運達地點 </p>";
            if (strODT_START_DATE.Equals(string.Empty))
                strMessage += "<P> 請輸入 起運時間 </p>";
            if (strODT_ARRIVE_DATE.Equals(string.Empty))
                strMessage += "<P> 請輸入 抵運時間 </p>";
            if (strOVC_RECEIVE_DEPT_CDE.Equals(string.Empty))
                strMessage += "<P> 請輸入 接收單位 </p>";
            if (strOVC_TRANSER_DEPT_CDE.Equals(string.Empty))
                strMessage += "<P> 請輸入 接轉地區 </p>";
            if (strONB_TOTAL_QUANITY.Equals(string.Empty))
                strMessage += "<P> 請輸入 總件數 </p>";
            if (strOVC_QUANITY_UNIT.Equals(string.Empty))
                strMessage += "<P> 請輸入 件數計量單位 </p>";
            if (strONB_TOTAL_VOLUME.Equals(string.Empty))
                strMessage += "<P> 請輸入 總體積 </p>";
            if (strOVC_VOLUME_UNIT.Equals(string.Empty))
                strMessage += "<P> 請輸入 體積計量單位 </p>";
            if (strONB_TOTAL_WEIGHT.Equals(string.Empty))
                strMessage += "<P> 請輸入 總重量 </p>";
            if (strOVC_WEIGHT_UNIT.Equals(string.Empty))
                strMessage += "<P> 請輸入 重量計量單位 </p>";
            if (strOVC_SHIP_NAME.Equals(string.Empty))
                strMessage += "<P> 請輸入 船機名稱 </p>";
            if (strOVC_VOYAGE.Equals(string.Empty))
                strMessage += "<P> 請輸入 船機航次 </p>";

            bool boolODT_START_DATE = FCommon.checkDateTime(strODT_START_DATE, "啟運地點", ref strMessage, out DateTime dateODT_START_DATE);
            bool boolODT_ARRIVE_DATE = FCommon.checkDateTime(strODT_ARRIVE_DATE, "運達地點", ref strMessage, out DateTime dateODT_ARRIVE_DATE);
            bool boolONB_TOTAL_QUANITY = FCommon.checkDecimal(strONB_TOTAL_QUANITY, "總件數", ref strMessage, out decimal decONB_TOTAL_QUANITY);
            bool boolONB_TOTAL_VOLUME = FCommon.checkDecimal(strONB_TOTAL_VOLUME, "總體積", ref strMessage, out decimal decONB_TOTAL_VOLUME);
            bool boolONB_TOTAL_WEIGHT = FCommon.checkDecimal(strONB_TOTAL_WEIGHT, "總重量", ref strMessage, out decimal decONB_TOTAL_WEIGHT);
            #endregion

            string[] checkID = (string[])ViewState["checkID"];
            int iho_no_num = 0;
            string year = FCommon.getTaiwanDate(DateTime.Now, "{0}").PadLeft(3, '0');
            string judge_iho = "IHO" + year + strUserDept;
            TBGMT_IHO iho_no = MTSE.TBGMT_IHO.Where(table => table.OVC_IHO_NO.StartsWith(judge_iho)).OrderByDescending(table => table.OVC_IHO_NO).FirstOrDefault();
            if (iho_no != null)
                int.TryParse(iho_no.OVC_IHO_NO.Substring(11, 4), out iho_no_num);
            iho_no_num++;
            string strOVC_IHO_NO = judge_iho + iho_no_num.ToString("0000");

            if (strMessage.Equals(string.Empty))
            {
                string strErrorNo = "", strMessageError = ""; ;
                bool isHasData = false;
                //先修改TBGMT_IRD_DETAIL，並判斷是否所有DETAIL無IHO才需設定
                #region TBGMT_IRD_DETAIL 修改資料
                for (int i = 0; i < checkID.Length; i++)
                {
                    bool isExise = false;
                    string id = checkID[i];
                    if (Guid.TryParse(id, out Guid guidOVC_IRDDETAIL_SN))
                    {
                        //TBGMT_IRD_DETAIL ird_detail = new TBGMT_IRD_DETAIL();
                        TBGMT_IRD_DETAIL ird_detail = MTSE.TBGMT_IRD_DETAIL.Where(table => table.OVC_IRDDETAIL_SN.Equals(guidOVC_IRDDETAIL_SN)).FirstOrDefault();
                        if (ird_detail != null)
                        {
                            string strOVC_BLD_NO = ird_detail.OVC_BLD_NO;
                            string strOVC_BOX_NO = ird_detail.OVC_BOX_NO;
                            if (ird_detail.OVC_IHO_NO == null)
                            {
                                //ird_detail.OVC_DEPT_CDE = strOVC_DEPT_CDE;
                                //ird_detail.OVC_BOX_NO = GVTBGMT_IRD_DETAIL.Rows[i].Cells[4].Text;
                                ird_detail.OVC_IHO_NO = strOVC_IHO_NO;
                                //ird_detail.OVC_PURCH_NO = ((Label)GVTBGMT_IRD_DETAIL.Rows[i].FindControl("lblOVC_PURCH_NO")).Text;

                                //MTSE.TBGMT_IRD_DETAIL.Add(ird_detail);
                                MTSE.SaveChanges();
                                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), ird_detail.GetType().Name.ToString(), this, "修改");
                                isHasData = true;
                            }
                            else
                                strMessageError += $"<p> 提單編號：{ strOVC_BLD_NO } 箱號：{ strOVC_BOX_NO } 之分運明細，已建立交接單！ </p>";
                            isExise = true;
                            //FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"提單編號：{ strOVC_BLD_NO } 箱號：{ strOVC_BOX_NO } 之分運明細，已建立交接單！");
                        }
                    }
                    if (!isExise)
                    {
                        strErrorNo += strErrorNo.Equals(string.Empty) ? "" : ", ";
                        strErrorNo += (i + 1).ToString();
                    }
                }
                #endregion

                #region TBGMT_IHO 新增資料
                if (isHasData)
                {
                    TBGMT_IHO iho = new TBGMT_IHO();
                    iho.OVC_IHO_SN = Guid.NewGuid();
                    iho.OVC_IHO_NO = strOVC_IHO_NO;

                    iho.OVC_INLAND_TRANS_TYPE = strOVC_IMPORT_TRANS_TYPE;
                    iho.OVC_START_PLACE = strOVC_START_PLACE;
                    iho.OVC_ARRIVE_PLACE = strOVC_ARRIVE_PLACE;
                    iho.ODT_START_DATE = dateODT_START_DATE;
                    iho.ODT_ARRIVE_DATE = dateODT_ARRIVE_DATE;
                    iho.OVC_RECEIVE_DEPT_CDE = strOVC_RECEIVE_DEPT_CDE;
                    iho.OVC_TRANSER_DEPT_CDE = strOVC_TRANSER_DEPT_CDE;
                    iho.ONB_QUANITY = decONB_TOTAL_QUANITY;
                    iho.OVC_QUANITY_UNIT = strOVC_QUANITY_UNIT;
                    iho.ONB_VOLUME = decONB_TOTAL_VOLUME;
                    iho.OVC_VOLUME_UNIT = strOVC_VOLUME_UNIT;
                    iho.ONB_WEIGHT = decONB_TOTAL_WEIGHT;
                    iho.OVC_WEIGHT_UNIT = strOVC_WEIGHT_UNIT;
                    iho.OVC_SHIP_NAME = strOVC_SHIP_NAME;
                    iho.OVC_VOYAGE = strOVC_VOYAGE;
                    iho.OVC_NOTE = strOVC_NOTE;
                    iho.OVC_CREATE_LOGIN_ID = strUserId;
                    iho.ODT_MODIFY_DATE = dateNow;

                    MTSE.TBGMT_IHO.Add(iho);
                    MTSE.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), iho.GetType().Name.ToString(), this, "新增");

                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "交接單 新增成功，交接單單號：" + strOVC_IHO_NO);
                    if (!strErrorNo.Equals(string.Empty))
                        strMessageError += $"<p> 第 {  strErrorNo } 筆 分運明細，不存在！ </p>";
                    if (!strMessageError.Equals(string.Empty))
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessageError);
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "所有明細皆已建立交接單，請重新選擇明細。");
                #endregion


                //#region TBGMT_IRD 新增資料
                //TBGMT_IRD ird = new TBGMT_IRD();
                //ird.OVC_IRD_SN = Guid.NewGuid();
                //ird.OVC_BLD_NO = arrno[0];
                //ird.OVC_CREATE_LOGIN_ID = Session["userid"].ToString();
                //ird.ODT_MODIFY_DATE = System.DateTime.Now;

                //MTSE.TBGMT_IRD.Add(ird);
                //MTSE.SaveChanges();
                //#endregion

                //#region TBGMT_IRD_MRPLOG 新增資料
                //for (int i = 0; i < GVTBGMT_IHO.Rows.Count; i++)
                //{
                //    TBGMT_IRD_MRPLOG ird_log = new TBGMT_IRD_MRPLOG();
                //    ird_log.LOG_LOGIN_ID = Session["userid"].ToString();
                //    ird_log.LOG_TIME = System.DateTime.Now;
                //    ird_log.LOG_EVENT = "INSERT";
                //    ird_log.OVC_BLD_NO = arrno[i];
                //    ird_log.OVC_CREATE_LOGIN_ID = Session["userid"].ToString();
                //    ird_log.ODT_MODIFY_DATE = System.DateTime.Now;
                //    ird_log.OVC_IRDMRPLOG_SN = Guid.NewGuid();

                //    MTSE.TBGMT_IRD_MRPLOG.Add(ird_log);
                //    MTSE.SaveChanges();
                //}
                //#endregion
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {

            Response.Redirect($"MTS_A19_1{ getQueryString() }");
        }

        protected void drpOVC_QUANITY_UNIT_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isShow = drpOVC_QUANITY_UNIT.SelectedValue.Equals("其他");
            txtOVC_QUANITY_UNIT.Visible = isShow;
        }

        protected void GVTBGMT_IRD_DETAIL_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            Guid id = new Guid(theGridView.DataKeys[gvrIndex].Value.ToString());

            switch (e.CommandName)
            {
                case "btnModify":
                    theGridView.EditIndex = gvrIndex;
                    dataImport_GVTBGMT_IRD_DETAIL(false);
                    break;
                case "btnSave":
                    TextBox txtOVC_PURCH_NO = (TextBox)gvr.FindControl("txtOVC_PURCH_NO");
                    if (FCommon.Controls_isExist(txtOVC_PURCH_NO) && !string.IsNullOrEmpty(txtOVC_PURCH_NO.Text))
                    {
                        TBGMT_IRD_DETAIL IRD_DETAIL = MTSE.TBGMT_IRD_DETAIL.Where(table => table.OVC_IRDDETAIL_SN.Equals(id)).FirstOrDefault();
                        if (IRD_DETAIL != null)
                        {
                            IRD_DETAIL.OVC_PURCH_NO = txtOVC_PURCH_NO.Text;
                            MTSE.SaveChanges();
                            FCommon.AlertShow(pnIRD_DETAIL, "success", "系統訊息", "購案號 修改成功");
                        }
                        else
                            FCommon.AlertShow(pnIRD_DETAIL, "danger", "系統訊息", "此 分運明細 不存在！");
                        theGridView.EditIndex = -1;
                        dataImport_GVTBGMT_IRD_DETAIL(false);
                    }
                    else
                        FCommon.AlertShow(pnIRD_DETAIL, "danger", "系統訊息", "<p> 請輸入 購案號！ </p>");
                    break;
                case "btnCancel":
                    theGridView.EditIndex = -1;
                    dataImport_GVTBGMT_IRD_DETAIL(false);
                    break;
                default:
                    break;
            }
        }
        protected void GVTBGMT_IRD_DETAIL_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void GVTBGMT_IRD_DETAIL_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}