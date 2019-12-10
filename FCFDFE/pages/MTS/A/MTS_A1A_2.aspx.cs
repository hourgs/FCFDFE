using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A1A_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        Common FCommon = new Common();
        string strOVC_IHO_NO;

        #region 副程式
        private void dateImport()
        {
            TBGMT_IHO IHO = MTSE.TBGMT_IHO.Where(table => table.OVC_IHO_NO.Equals(strOVC_IHO_NO)).FirstOrDefault();
            if (IHO != null)
            {
                lblOVC_IHO_NO.Text = IHO.OVC_IHO_NO;
                FCommon.list_setValue(drpOVC_TRANS_TYPE, IHO.OVC_INLAND_TRANS_TYPE);
                txtOVC_START_PLACE.Text = IHO.OVC_START_PLACE;
                txtOVC_ARRIVE_PLACE.Text = IHO.OVC_ARRIVE_PLACE;
                txtODT_START_DATE.Text = FCommon.getDateTime(IHO.ODT_START_DATE);
                lblODT_ARRIVE_DATE.Text = FCommon.getDateTime(IHO.ODT_ARRIVE_DATE);
                string strOVC_RECEIVE_DEPT_CDE = IHO.OVC_RECEIVE_DEPT_CDE;
                txtOVC_RECEIVE_DEPT_CDE.Value = strOVC_RECEIVE_DEPT_CDE;
                TBMDEPT tbmdept = GME.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(strOVC_RECEIVE_DEPT_CDE)).FirstOrDefault();
                if (tbmdept != null)
                    lblOVC_RECEIVE_DEPT_CDE.Text = tbmdept.OVC_ONNAME;

                lblOVC_TRANSER_DEPT_CDE.Text = IHO.OVC_TRANSER_DEPT_CDE;
                txtONB_TOTAL_QUANITY.Text = IHO.ONB_QUANITY.ToString();
                string strOVC_QUANITY_UNIT = IHO.OVC_QUANITY_UNIT;
                FCommon.list_setValue(drpOVC_QUANITY_UNIT, strOVC_QUANITY_UNIT);
                bool isOther = !drpOVC_QUANITY_UNIT.SelectedValue.Equals(strOVC_QUANITY_UNIT);
                if (isOther)
                {
                    FCommon.list_setValue(drpOVC_QUANITY_UNIT, "其他");
                    txtOVC_QUANITY_UNIT.Text = strOVC_QUANITY_UNIT;
                }
                txtOVC_QUANITY_UNIT.Visible = isOther;
                txtONB_TOTAL_VOLUME.Text = IHO.ONB_VOLUME.ToString();
                FCommon.list_setValue(drpOVC_VOLUME_UNIT, IHO.OVC_VOLUME_UNIT);
                txtONB_TOTAL_WEIGHT.Text = IHO.ONB_WEIGHT.ToString();
                FCommon.list_setValue(drpOVC_WEIGHT_UNIT, IHO.OVC_WEIGHT_UNIT);
                txtOVC_SHIP_NAME.Text = IHO.OVC_SHIP_NAME;
                txtOVC_VOYAGE.Text = IHO.OVC_VOYAGE;
                txtONB_OVERFLOW.Text = IHO.ONB_OVERFLOW.ToString();
                txtONB_LESS.Text = IHO.ONB_LESS.ToString();
                txtONB_BROKEN.Text = IHO.ONB_BROKEN.ToString();
                txtONB_ACTUAL_RECEIVE.Text = IHO.ONB_ACTUAL_RECEIVE.ToString();
                txtOVC_NOTE.Text = IHO.OVC_NOTE;
            }
        }
        private void dataImport_GVTBGMT_IRD_DETAIL_1()
        {
            var query =
                from irdtail in MTSE.TBGMT_IRD_DETAIL.AsEnumerable()
                join bld in MTSE.TBGMT_BLD.AsEnumerable() on irdtail.OVC_BLD_NO equals bld.OVC_BLD_NO
                join icr in MTSE.TBGMT_ICR.AsEnumerable() on irdtail.OVC_BLD_NO equals icr.OVC_BLD_NO
                join arrport in MTSE.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals arrport.OVC_PORT_CDE
                where arrport.OVC_IS_ABROAD.Equals("國內")
                where irdtail.OVC_IHO_NO != null && irdtail.OVC_IHO_NO.Equals(strOVC_IHO_NO)
                select new
                {
                    OVC_BLD_NO = irdtail.OVC_BLD_NO,
                    OVC_PURCH_NO = irdtail.OVC_PURCH_NO,
                    //OVC_ITEM_TYPE = bld.OVC_ITEM_TYPE,
                    icr.OVC_CHI_NAME,
                    OVC_BOX_NO = irdtail.OVC_BOX_NO,
                    ONB_ACTUAL_RECEIVE = irdtail.ONB_ACTUAL_RECEIVE,
                    OVC_IRDDETAIL_SN = irdtail.OVC_IRDDETAIL_SN
                };

            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows1"] = FCommon.GridView_dataImport(GVTBGMT_IRD_DETAIL_1, dt);
        }
        private void dataImport_GVTBGMT_IRD_DETAIL_2()
        {
            string strMessage = "";
            string strOVC_BLD_NO = txtOVC_BLD_NO.Text;

            if (strOVC_BLD_NO.Equals(string.Empty))
                strMessage += "<P> 請輸入 提單號碼 </p>";

            if (strMessage.Equals(string.Empty))
            {
                //查詢尚未新增IHO之
                var query =
                    from bld in MTSE.TBGMT_BLD.AsEnumerable()
                    where bld.OVC_BLD_NO.Contains(strOVC_BLD_NO)
                    join irdtail in MTSE.TBGMT_IRD_DETAIL.AsEnumerable() on bld.OVC_BLD_NO equals irdtail.OVC_BLD_NO
                    join icr in MTSE.TBGMT_ICR.AsEnumerable() on irdtail.OVC_BLD_NO equals icr.OVC_BLD_NO
                    where irdtail.OVC_IHO_NO == null
                    select new
                    {
                        OVC_IRDDETAIL_SN = irdtail.OVC_IRDDETAIL_SN,
                        OVC_BLD_NO = bld.OVC_BLD_NO,
                        OVC_PURCH_NO = irdtail.OVC_PURCH_NO,
                        //OVC_ITEM_TYPE = bld.OVC_ITEM_TYPE,
                        icr.OVC_CHI_NAME,
                        OVC_DEPT_CDE = irdtail.OVC_DEPT_CDE,
                        OVC_BOX_NO = irdtail.OVC_BOX_NO,
                        ONB_ACTUAL_RECEIVE = irdtail.ONB_ACTUAL_RECEIVE
                    };
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows2"] = FCommon.GridView_dataImport(GVTBGMT_IRD_DETAIL_2, dt);
            }
            else
                FCommon.AlertShow(PnUpdate, "danger", "系統訊息", strMessage);
        }

        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_IHO_NO", Request.QueryString["OVC_IHO_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", Request.QueryString["OVC_BLD_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", Request.QueryString["OVC_TRANSER_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_DEPT_CDE", Request.QueryString["OVC_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_START_DATE_S", Request.QueryString["ODT_START_DATE_S"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_START_DATE_E", Request.QueryString["ODT_START_DATE_E"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_ARRIVE_DATE_S", Request.QueryString["ODT_ARRIVE_DATE_S"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_ARRIVE_DATE_E", Request.QueryString["ODT_ARRIVE_DATE_E"], false);
            return strQueryString;
        }

        #endregion
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getQueryString(this, "id", out strOVC_IHO_NO, true);

                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtODT_START_DATE);
                    #region 下拉式選單匯入資料
                    CommonMTS.list_dataImport_TRANS_TYPE(drpOVC_TRANS_TYPE, false); //清運方式
                    CommonMTS.list_dataImport_QUANITY_UNIT(drpOVC_QUANITY_UNIT, true); //件數單位
                    CommonMTS.list_dataImport_VOLUME_UNIT(drpOVC_VOLUME_UNIT, true); //體積單位
                    CommonMTS.list_dataImport_WEIGHT_UNIT2(drpOVC_WEIGHT_UNIT, true); //重量單位
                    #endregion

                    int count = 0;
                    ViewState["count"] = count; //紀錄顯示/隱藏明細選單次數
                    PnTable.Visible = false;
                    btnOther.Text = "顯示新增明細選單";

                    dateImport();
                    dataImport_GVTBGMT_IRD_DETAIL_1();
                }
            }
        }

        #region OnClick~
        protected void btnEdit_IHO_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strOVC_TRANS_TYPE = drpOVC_TRANS_TYPE.SelectedValue;
            string strOVC_START_PLACE = txtOVC_START_PLACE.Text;
            string strOVC_ARRIVE_PLACE = txtOVC_ARRIVE_PLACE.Text;
            string strODT_START_DATE = txtODT_START_DATE.Text;
            string strODT_ARRIVE_DATE = lblODT_ARRIVE_DATE.Text;
            string strONB_TOTAL_QUANITY = txtONB_TOTAL_QUANITY.Text;
            string strOVC_QUANITY_UNIT = drpOVC_QUANITY_UNIT.SelectedValue;
            if (strOVC_QUANITY_UNIT.Equals("其他")) strOVC_QUANITY_UNIT = txtOVC_QUANITY_UNIT.Text; //若選擇其他，則取得輸入之資料
            string strONB_TOTAL_VOLUME = txtONB_TOTAL_VOLUME.Text;
            string strOVC_VOLUME_UNIT = drpOVC_VOLUME_UNIT.SelectedValue;
            string strONB_TOTAL_WEIGHT = txtONB_TOTAL_WEIGHT.Text;
            string strOVC_WEIGHT_UNIT = drpOVC_WEIGHT_UNIT.SelectedValue;
            string strOVC_SHIP_NAME = txtOVC_SHIP_NAME.Text;
            string strOVC_VOYAGE = txtOVC_VOYAGE.Text;
            string strONB_OVERFLOW = txtONB_OVERFLOW.Text;
            string strONB_LESS = txtONB_LESS.Text;
            string strONB_BROKEN = txtONB_BROKEN.Text;
            string strONB_ACTUAL_RECEIVE = txtONB_ACTUAL_RECEIVE.Text;
            string strOVC_NOTE = txtOVC_NOTE.Text;

            DateTime dateODT_START_DATE, dateODT_ARRIVE_DATE, dateNow=DateTime.Now;
            decimal decONB_TOTAL_QUANITY, decONB_OVERFLOW, decONB_TOTAL_VOLUME, decONB_TOTAL_WEIGHT, decONB_LESS, decONB_BROKEN, decONB_ACTUAL_RECEIVE;

            TBGMT_IHO IHO = MTSE.TBGMT_IHO.Where(table => table.OVC_IHO_NO.Equals(strOVC_IHO_NO)).FirstOrDefault();

            #region 錯誤訊息
            if(IHO==null)
                strMessage += $"<P> 交接單號碼：{ strOVC_IHO_NO } 之運輸交接單 不存在！ </p>";
            if (strOVC_TRANS_TYPE.Equals(string.Empty))
                strMessage += "<P> 請輸入 運輸方法 </p>";
            if (strOVC_START_PLACE.Equals(string.Empty))
                strMessage += "<P> 請輸入 啟運地點 </p>";
            if (strOVC_ARRIVE_PLACE.Equals(string.Empty))
                strMessage += "<P> 請輸入 運達地點 </p>";
            if (strODT_START_DATE.Equals(string.Empty))
                strMessage += "<P> 請輸入 起運時間 </p>";
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

            bool boolODT_START_DATE = FCommon.checkDateTime(strODT_START_DATE, "起運時間", ref strMessage, out dateODT_START_DATE);
            bool boolODT_ARRIVE_DATE = FCommon.checkDateTime(strODT_ARRIVE_DATE, "抵運時間", ref strMessage, out dateODT_ARRIVE_DATE);
            bool boolONB_TOTAL_QUANITY = FCommon.checkDecimal(strONB_TOTAL_QUANITY, "總件數", ref strMessage, out decONB_TOTAL_QUANITY);
            bool boolONB_TOTAL_VOLUME = FCommon.checkDecimal(strONB_TOTAL_VOLUME, "總體積", ref strMessage, out decONB_TOTAL_VOLUME);
            bool boolONB_TOTAL_WEIGHT = FCommon.checkDecimal(strONB_TOTAL_WEIGHT, "總重量", ref strMessage, out decONB_TOTAL_WEIGHT);
            bool boolONB_OVERFLOW = FCommon.checkDecimal(strONB_OVERFLOW, "超出", ref strMessage, out decONB_OVERFLOW); //未輸入，補0
            bool boolONB_LESS = FCommon.checkDecimal(strONB_LESS, "短少", ref strMessage, out decONB_LESS); //未輸入，補0
            bool boolONB_BROKEN = FCommon.checkDecimal(strONB_BROKEN, "破損", ref strMessage, out decONB_BROKEN); //未輸入，補0
            bool boolONB_ACTUAL_RECEIVE = FCommon.checkDecimal(strONB_ACTUAL_RECEIVE, "實收", ref strMessage, out decONB_ACTUAL_RECEIVE); //未輸入，補0
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                try
                {
                    string strOVC_IHO_NO = lblOVC_IHO_NO.Text;
                    #region TBGMT_IHO 修改
                    IHO.OVC_INLAND_TRANS_TYPE = strOVC_TRANS_TYPE;
                    IHO.OVC_START_PLACE = strOVC_START_PLACE;
                    IHO.OVC_ARRIVE_PLACE = strOVC_ARRIVE_PLACE;
                    IHO.ODT_START_DATE = dateODT_START_DATE;

                    if (boolODT_ARRIVE_DATE) IHO.ODT_ARRIVE_DATE = dateODT_ARRIVE_DATE; else IHO.ODT_ARRIVE_DATE = null;
                    IHO.OVC_RECEIVE_DEPT_CDE = txtOVC_RECEIVE_DEPT_CDE.Value;
                    IHO.OVC_TRANSER_DEPT_CDE = txtOVC_TRANSER_DEPT_CDE.Value;
                    IHO.ONB_QUANITY = decONB_TOTAL_QUANITY;
                    IHO.OVC_QUANITY_UNIT = strOVC_QUANITY_UNIT;
                    IHO.ONB_VOLUME = decONB_TOTAL_VOLUME;
                    IHO.OVC_VOLUME_UNIT = strOVC_VOLUME_UNIT;
                    IHO.ONB_WEIGHT = decONB_TOTAL_WEIGHT;
                    IHO.OVC_WEIGHT_UNIT = strOVC_WEIGHT_UNIT;
                    IHO.OVC_SHIP_NAME = strOVC_SHIP_NAME;
                    IHO.OVC_VOYAGE = strOVC_VOYAGE;
                    IHO.ONB_OVERFLOW = decONB_OVERFLOW;
                    IHO.ONB_LESS = decONB_LESS;
                    IHO.ONB_BROKEN = decONB_BROKEN;
                    IHO.ONB_ACTUAL_RECEIVE = decONB_ACTUAL_RECEIVE;
                    IHO.OVC_NOTE = strOVC_NOTE;
                    IHO.ODT_MODIFY_DATE = dateNow;

                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), IHO.GetType().Name.ToString(), this, "修改");
                    #endregion

                    //ArrayList aryTBM_IRD = new ArrayList(); // 紀錄曾經修改過的IRD-OVC_BLD_NO
                    //#region TBGMT_IRD 修改
                    //for (int i = 0; i < GVTBGMT_IRD_DETAIL_1.Rows.Count; i++)
                    //{
                    //    string bld_no = GVTBGMT_IRD_DETAIL_1.Rows[i].Cells[1].Text;
                    //    if (!aryTBM_IRD.Contains(bld_no)) // 不曾修改過之IRD，再行修改動作。
                    //    {
                    //        TBGMT_IRD IRD = MTSE.TBGMT_IRD.Where(table => table.OVC_BLD_NO == bld_no).FirstOrDefault();

                    //        //IRD.OVC_BLD_NO = bld_no;
                    //        IRD.ONB_OVERFLOW = Convert.ToDecimal(strONB_OVERFLOW);
                    //        IRD.ONB_LESS = Convert.ToDecimal(strONB_LESS);
                    //        IRD.ONB_BROKEN = Convert.ToDecimal(strONB_BROKEN);
                    //        IRD.ONB_ACTUAL_RECEIVE = Convert.ToDecimal(strONB_ACTUAL_RECEIVE);
                    //        IRD.OVC_NOTE = strOVC_NOTE;
                    //        IRD.OVC_CREATE_LOGIN_ID = IRD.OVC_CREATE_LOGIN_ID;
                    //        IRD.ODT_MODIFY_DATE = IRD.ODT_MODIFY_DATE;
                    //        IRD.OVC_IRD_SN = IRD.OVC_IRD_SN;

                    //        MTSE.SaveChanges();
                    //        aryTBM_IRD.Add(bld_no);
                    //    }
                    //}
                    //#endregion

                    //#region TBGMT_IRD_DETAIL 修改
                    //for (int i = 0; i < GVTBGMT_IRD_DETAIL_1.Rows.Count; i++)
                    //{
                    //    string bld_no = GVTBGMT_IRD_DETAIL_1.Rows[i].Cells[1].Text;
                    //    TBGMT_IRD_DETAIL IRD_DETAIL = MTSE.TBGMT_IRD_DETAIL.Where(table => table.OVC_BLD_NO.Equals(bld_no)).FirstOrDefault();

                    //    IRD_DETAIL.OVC_BLD_NO = GVTBGMT_IRD_DETAIL_1.Rows[i].Cells[1].Text;
                    //    IRD_DETAIL.OVC_DEPT_CDE = txtOVC_TRANSER_DEPT_CDE.Value;
                    //    IRD_DETAIL.OVC_BOX_NO = GVTBGMT_IRD_DETAIL_1.Rows[i].Cells[4].Text;
                    //    IRD_DETAIL.OVC_IHO_NO = lblOVC_IHO_NO.Text;
                    //    IRD_DETAIL.ONB_ACTUAL_RECEIVE = Convert.ToDecimal(GVTBGMT_IRD_DETAIL_1.Rows[i].Cells[5].Text);
                    //    IRD_DETAIL.ONB_OVERFLOW = Convert.ToDecimal(strONB_OVERFLOW);
                    //    IRD_DETAIL.ONB_LESS = Convert.ToDecimal(strONB_LESS);
                    //    IRD_DETAIL.ONB_BROKEN = Convert.ToDecimal(strONB_BROKEN);
                    //    IRD_DETAIL.ONB_ACTUAL_RECEIVE = Convert.ToDecimal(strONB_ACTUAL_RECEIVE);
                    //    IRD_DETAIL.OVC_IRDDETAIL_SN = IRD_DETAIL.OVC_IRDDETAIL_SN;

                    //    MTSE.SaveChanges();
                    //}
                    //#endregion

                    #region TBGMT_IRD_MRPLOG 新增LOG
                    for (int i = 0; i < GVTBGMT_IRD_DETAIL_1.Rows.Count; i++)
                    {
                        GridViewRow grv = GVTBGMT_IRD_DETAIL_1.Rows[i];
                        HyperLink hlOVC_BLD_NO = (HyperLink)grv.FindControl("hlkOVC_BLD_NO");
                        string strRow = grv.Cells[5].Text; //實收件數
                        bool boolstrRow = FCommon.checkDecimal(strRow, "實收件數", ref strMessage, out decimal decRow);

                        TBGMT_IRD_MRPLOG IRD_MRPLOG = new TBGMT_IRD_MRPLOG();
                        IRD_MRPLOG.LOG_LOGIN_ID = strUserId;
                        IRD_MRPLOG.LOG_TIME = dateNow;
                        IRD_MRPLOG.LOG_EVENT = "UPDATE";
                        IRD_MRPLOG.OVC_BLD_NO = hlOVC_BLD_NO.Text;
                        if (boolstrRow)
                            IRD_MRPLOG.ONB_ACTUAL_RECEIVE = decRow;
                        IRD_MRPLOG.ONB_OVERFLOW = decONB_OVERFLOW;
                        IRD_MRPLOG.ONB_LESS = decONB_LESS;
                        IRD_MRPLOG.ONB_BROKEN = decONB_BROKEN;
                        IRD_MRPLOG.ONB_ACTUAL_RECEIVE = decONB_ACTUAL_RECEIVE;
                        IRD_MRPLOG.OVC_NOTE = strOVC_NOTE;
                        IRD_MRPLOG.OVC_CREATE_LOGIN_ID = strUserId;
                        IRD_MRPLOG.ODT_MODIFY_DATE = dateNow;
                        IRD_MRPLOG.OVC_IRDMRPLOG_SN = Guid.NewGuid();
                        MTSE.TBGMT_IRD_MRPLOG.Add(IRD_MRPLOG);
                        MTSE.SaveChanges();
                        FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), IRD_MRPLOG.GetType().Name.ToString(), this, "修改");
                    }
                    #endregion
                    
                    FCommon.AlertShow(PnUpdate, "success", "系統訊息", $"交接單號碼：{ strOVC_IHO_NO } 更新成功!!");
                }
                catch
                {
                    FCommon.AlertShow(PnUpdate, "danger", "系統訊息", "更新失敗，請聯絡工程師。");
                }
            }
            else
                FCommon.AlertShow(PnUpdate, "danger", "系統訊息", strMessage);
        }
        protected void btnOther_Click(object sender, EventArgs e)
        {//顯示&隱藏新增明細選單
            if (ViewState["count"] != null)
            {
                if(int.TryParse(ViewState["count"].ToString(), out int num))
                {
                    num++;
                    if ((num % 2) != 0)
                    {
                        PnTable.Visible = true;
                        btnOther.Text = "隱藏新增明細選單";
                    }
                    else
                    {
                        PnTable.Visible = false;
                        btnOther.Text = "顯示新增明細選單";
                    }
                    ViewState["count"] = num;
                }
            }
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {//下面GridView的查詢
            dataImport_GVTBGMT_IRD_DETAIL_2();
        }
        protected void btnNew_Detail_Click(object sender, EventArgs e)
        {//將選取之Detail新增至IHO
            try
            {
                string strErrorNo = "", strMessageError = ""; ;
                bool isHasData = false;
                #region TBGMT_IRD_DETAIL 修改資料
                for (int i = 0; i < GVTBGMT_IRD_DETAIL_2.Rows.Count; i++)
                {
                    GridViewRow gvr = GVTBGMT_IRD_DETAIL_2.Rows[i];
                    int gvrIndex = gvr.RowIndex;
                    CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSelect");
                    if (FCommon.Controls_isExist(chkSelect) && chkSelect.Checked)
                    { //將被選取之分運明細，加進IHO中
                        bool isExise = false;
                        string id = GVTBGMT_IRD_DETAIL_2.DataKeys[gvrIndex].Value.ToString();
                        if (Guid.TryParse(id, out Guid guidOVC_IRDDETAIL_SN))
                        {
                            TBGMT_IRD_DETAIL ird_detail = MTSE.TBGMT_IRD_DETAIL.Where(table => table.OVC_IRDDETAIL_SN.Equals(guidOVC_IRDDETAIL_SN)).FirstOrDefault();
                            if (ird_detail != null)
                            {
                                string strOVC_BLD_NO = ird_detail.OVC_BLD_NO;
                                string strOVC_BOX_NO = ird_detail.OVC_BOX_NO;
                                if (ird_detail.OVC_IHO_NO == null)
                                {
                                    ird_detail.OVC_IHO_NO = strOVC_IHO_NO;
                                    MTSE.SaveChanges();
                                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), ird_detail.GetType().Name.ToString(), this, "修改");
                                    isHasData = true;
                                }
                                else
                                    strMessageError += $"<p> 提單編號：{ strOVC_BLD_NO } 箱號：{ strOVC_BOX_NO } 之分運明細，已建立交接單！ </p>";
                                isExise = true;
                            }
                        }
                        if (!isExise)
                        {
                            strErrorNo += strErrorNo.Equals(string.Empty) ? "" : ", ";
                            strErrorNo += (i + 1).ToString();
                        }
                    }
                }
                #endregion

                if (isHasData)
                {
                    dataImport_GVTBGMT_IRD_DETAIL_1();
                    dataImport_GVTBGMT_IRD_DETAIL_2();

                    FCommon.AlertShow(PnUpdate, "success", "系統訊息", "加入軍品運輸交接單成功");
                    if (!strErrorNo.Equals(string.Empty))
                        strMessageError += $"<p> 第 {  strErrorNo } 筆 分運明細，不存在！ </p>";
                    if (!strMessageError.Equals(string.Empty))
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessageError);
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "所有明細皆已建立交接單，請重新選擇明細。");
            }
            catch
            {
                FCommon.AlertShow(PnUpdate, "danger", "系統訊息", "加入失敗，請聯絡工程師。");
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_A1A_1{ getQueryString() }");
        }
        #endregion

        protected void txtOVC_BLD_NO_TextChanged(object sender, EventArgs e)
        {
            txtOVC_BLD_NO.Text = txtOVC_BLD_NO.Text.ToUpper();   //轉大寫
        }
        protected void drpOVC_QUANITY_UNIT_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isShow = drpOVC_QUANITY_UNIT.SelectedValue.Equals("其他");
            txtOVC_QUANITY_UNIT.Visible = isShow;
        }

        protected void GVTBGMT_IRD_DETAIL_1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = theGridView.DataKeys[gvrIndex].Value.ToString();
            Guid.TryParse(id, out Guid guidOVC_IRDDETAIL_SN);

            switch (e.CommandName)
            {
                case "btnModify":
                    theGridView.EditIndex = gvrIndex;
                    dataImport_GVTBGMT_IRD_DETAIL_1();
                    break;
                case "btnDel":
                    TBGMT_IRD_DETAIL ird_detail = MTSE.TBGMT_IRD_DETAIL.Where(t => t.OVC_IRDDETAIL_SN.Equals(guidOVC_IRDDETAIL_SN)).FirstOrDefault();
                    if (ird_detail != null)
                    {
                        string strOVC_BLD_NO = ird_detail.OVC_BLD_NO;
                        //MTSE.Entry(ird_datil).State = EntityState.Deleted;
                        ird_detail.OVC_IHO_NO = null;
                        MTSE.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), ird_detail.GetType().Name.ToString(), this, "修改");
                        FCommon.AlertShow(PnUpdate, "success", "系統訊息", $"提單號碼：{ strOVC_BLD_NO } 之分運明細 移除成功。");
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "分運明細 不存在！");
                    theGridView.EditIndex = -1;
                    dataImport_GVTBGMT_IRD_DETAIL_1();
                    break;
                case "btnUpdate":
                    TextBox txtOVC_PURCH_NO = (TextBox)gvr.FindControl("txtOVC_PURCH_NO");
                    if (FCommon.Controls_isExist(txtOVC_PURCH_NO) && !string.IsNullOrEmpty(txtOVC_PURCH_NO.Text))
                    {
                        ird_detail = MTSE.TBGMT_IRD_DETAIL.Where(table => table.OVC_IRDDETAIL_SN.Equals(guidOVC_IRDDETAIL_SN)).FirstOrDefault();
                        if (ird_detail != null)
                        {
                            string strOVC_BLD_NO = ird_detail.OVC_BLD_NO;
                            ird_detail.OVC_PURCH_NO = txtOVC_PURCH_NO.Text;
                            MTSE.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), ird_detail.GetType().Name.ToString(), this, "修改");
                            FCommon.AlertShow(PnUpdate, "success", "系統訊息", $"提單號碼：{ strOVC_BLD_NO } 之購案號 修改成功。");
                        }
                        else
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "分運明細 不存在！");
                        theGridView.EditIndex = -1;
                        dataImport_GVTBGMT_IRD_DETAIL_1();
                    }
                    else
                        FCommon.AlertShow(PnUpdate, "danger", "系統訊息", "請輸入 購案號！");
                    break;
                case "btnCancel":
                    theGridView.EditIndex = -1;
                    dataImport_GVTBGMT_IRD_DETAIL_1();
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
        protected void GVTBGMT_IRD_DETAIL_1_PreRender(object sender, EventArgs e)
        {
            bool hasRows1 = false;
            if (ViewState["hasRows1"] != null)
                hasRows1 = Convert.ToBoolean(ViewState["hasRows1"]);
            FCommon.GridView_PreRenderInit(sender, hasRows1);
        }
        protected void GVTBGMT_IRD_DETAIL_2_PreRender(object sender, EventArgs e)
        {
            bool hasRows2 = false;
            if (ViewState["hasRows2"] != null)
                hasRows2 = Convert.ToBoolean(ViewState["hasRows2"]);
            FCommon.GridView_PreRenderInit(sender, hasRows2);
        }
    }
}