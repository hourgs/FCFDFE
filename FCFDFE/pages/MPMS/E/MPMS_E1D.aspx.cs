using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using Microsoft.International.Formatters;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xceed.Words.NET;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E1D : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (Session["rowtext"] != null)
                {
                    if (!IsPostBack)
                    {
                        //drp_dataImport();
                        FCommon.Controls_Attributes("readonly", "true", txtOVC_DRECEIVE, txtOVC_DCOMPTROLLER, txtOVC_DBACK, txtOVC_DAPPROVE);
                        list_dataImport(drpOVC_BACK_REASON, "HA");
                        list_dataImport(drpOVC_BACK_MARK, "HB");
                        TB_dataImport();
                        list_dataImport(drpOVC_CURRENT_1, drpOVC_CURRENT_2, drpOVC_CURRENT_3);
                    }
                }
                else
                    FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
                if (Session["sn"] != null)
                    print.Visible = true;
                else
                    print.Visible = false;
            }
        }
        #region Click
        //回上一頁
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E1C.aspx";
            Response.Redirect(send_url);
        }
        //回主流程
        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }
        //存檔
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strmeg = "";
            if (txtOVC_OWN_NO.Text == "")
                strmeg += "<p>請填選 款項所有權人及款項所有權人統一編號</p>";
            if (txtOVC_WORK_NO.Text == "")
                strmeg += "<p>請填選 承辦單位及承辦單位統一編號</p>";

            bool boolONB_MONEY_1 = FCommon.checkDecimal(txtONB_MONEY_1.Text, "原幣金額", ref strmeg, out decimal d);
            bool boolONB_MONEY_2 = FCommon.checkDecimal(txtONB_MONEY_2.Text, "原幣金額", ref strmeg, out d);
            bool boolONB_MONEY_3 = FCommon.checkDecimal(txtONB_MONEY_3.Text, "原幣金額", ref strmeg, out d);
            bool boolONB_MONEY_NT_1 = FCommon.checkDecimal(txtONB_MONEY_NT_1.Text, "新台幣入帳金額", ref strmeg, out d);
            bool boolONB_MONEY_NT_2 = FCommon.checkDecimal(txtONB_MONEY_NT_2.Text, "新台幣入帳金額", ref strmeg, out d);
            bool boolONB_MONEY_NT_3 = FCommon.checkDecimal(txtONB_MONEY_NT_3.Text, "新台幣入帳金額", ref strmeg, out d);
            bool boolONB_ALL_MONEY = FCommon.checkDecimal(txtONB_ALL_MONEY.Text, "合計金額", ref strmeg, out d);

            if (Session["sn"] != null)
            {
                Guid guid = Guid.Parse(Session["sn"].ToString());
                TBMMANAGE_CASH tbmmanage_cash = new TBMMANAGE_CASH();
                tbmmanage_cash = mpms.TBMMANAGE_CASH
                    .Where(table => table.OVC_CASH_SN.Equals(guid))
                    .Where(table => table.OVC_PURCH.Equals(lblEscOVC_PURCH_6.Text.Substring(0, 7))).FirstOrDefault();
                if (tbmmanage_cash != null)
                {
                    tbmmanage_cash.OVC_OWN_NAME = txtOVC_OWN_NAME.Text;
                    tbmmanage_cash.OVC_OWN_NO = txtOVC_OWN_NO.Text;
                    tbmmanage_cash.OVC_OWN_ADDRESS = txtOVC_OWN_ADDRESS.Text;
                    tbmmanage_cash.OVC_OWN_TEL = txtOVC_OWN_TEL.Text;
                    tbmmanage_cash.OVC_REASON_1 = txtOVC_REASON_1.Text;
                    tbmmanage_cash.OVC_REASON_2 = txtOVC_REASON_2.Text;
                    tbmmanage_cash.OVC_REASON_3 = txtOVC_REASON_3.Text;
                    if (boolONB_MONEY_1) tbmmanage_cash.ONB_MONEY_1 = decimal.Parse(txtONB_MONEY_1.Text); else tbmmanage_cash.ONB_MONEY_1 = null;
                    if (boolONB_MONEY_2) tbmmanage_cash.ONB_MONEY_2 = decimal.Parse(txtONB_MONEY_2.Text); else tbmmanage_cash.ONB_MONEY_2 = null;
                    if (boolONB_MONEY_3) tbmmanage_cash.ONB_MONEY_3 = decimal.Parse(txtONB_MONEY_3.Text); else tbmmanage_cash.ONB_MONEY_3 = null;
                    if (boolONB_MONEY_NT_1) tbmmanage_cash.ONB_MONEY_NT_1 = decimal.Parse(txtONB_MONEY_NT_1.Text); else tbmmanage_cash.ONB_MONEY_NT_1 = null;
                    if (boolONB_MONEY_NT_2) tbmmanage_cash.ONB_MONEY_NT_2 = decimal.Parse(txtONB_MONEY_NT_2.Text); else tbmmanage_cash.ONB_MONEY_NT_2 = null;
                    if (boolONB_MONEY_NT_3) tbmmanage_cash.ONB_MONEY_NT_3 = decimal.Parse(txtONB_MONEY_NT_3.Text); else tbmmanage_cash.ONB_MONEY_NT_3 = null;
                    var query =
                        from tbm1407 in mpms.TBM1407
                        where tbm1407.OVC_PHR_CATE.Equals("B0")
                        select new
                        {
                            OVC_PHR_ID = tbm1407.OVC_PHR_ID,
                            OVC_PHR_DESC = tbm1407.OVC_PHR_DESC
                        };
                    foreach (var q in query)
                    {
                        if (drpOVC_CURRENT_1.Text == q.OVC_PHR_DESC)
                            tbmmanage_cash.OVC_CURRENT_1 = q.OVC_PHR_ID;
                        if (drpOVC_CURRENT_2.Text == q.OVC_PHR_DESC)
                            tbmmanage_cash.OVC_CURRENT_2 = q.OVC_PHR_ID;
                        if (drpOVC_CURRENT_3.Text == q.OVC_PHR_DESC)
                            tbmmanage_cash.OVC_CURRENT_3 = q.OVC_PHR_ID;
                    }
                    if (boolONB_ALL_MONEY) tbmmanage_cash.ONB_ALL_MONEY = decimal.Parse(txtONB_ALL_MONEY.Text); else tbmmanage_cash.ONB_ALL_MONEY = null;
                    if (rdoOVC_KIND.SelectedIndex == 0)
                        tbmmanage_cash.OVC_KIND = "1";
                    else if (rdoOVC_KIND.SelectedIndex == 1)
                        tbmmanage_cash.OVC_KIND = "2";
                    tbmmanage_cash.OVC_MARK = txtOVC_MARK.Text;
                    tbmmanage_cash.OVC_WORK_NO = txtOVC_WORK_NO.Text;
                    tbmmanage_cash.OVC_WORK_NAME = txtOVC_WORK_NAME.Text;
                    tbmmanage_cash.OVC_WORK_UNIT = txtOVC_WORK_UNIT.Text;
                    tbmmanage_cash.OVC_RECEIVE_NO = txtOVC_RECEIVE_NO.Text;
                    tbmmanage_cash.OVC_COMPTROLLER_NO = txtOVC_COMPTROLLER_NO_1.Text;
                    //if (drpOVC_COMPTROLLER_NO.Text != "請選擇")
                    //    tbmmanage_cash.OVC_ONNAME = drpOVC_COMPTROLLER_NO.Text;
                    //else
                    //    tbmmanage_cash.OVC_ONNAME = txtOVC_COMPTROLLER_NO.Text;
                    tbmmanage_cash.OVC_ONNAME = txtOVC_COMPTROLLER_NO.Text;
                    tbmmanage_cash.OVC_DCOMPTROLLER = txtOVC_DCOMPTROLLER.Text;
                    tbmmanage_cash.OVC_BACK_NO = txtOVC_BACK_NO.Text;
                    tbmmanage_cash.OVC_DRECEIVE = txtOVC_DRECEIVE.Text;
                    tbmmanage_cash.OVC_DBACK = txtOVC_DBACK.Text;
                    tbmmanage_cash.OVC_BACK_REASON = txtOVC_BACK_REASON.Text;
                    tbmmanage_cash.OVC_BACK_MARK = txtOVC_BACK_MARK.Text;
                    tbmmanage_cash.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;
                }
                if (strmeg == "")
                {
                    mpms.SaveChanges();
                    TB_dataImport();
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strmeg);
            }
            else
            {
                var check = mpms.TBMMANAGE_CASH
                    .Where(o => o.OVC_WORK_NO.Equals(txtOVC_WORK_NO.Text)).FirstOrDefault();
                if (check != null)
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "承辦單位不可重複");
                }
                else
                {
                    TBMMANAGE_CASH tbmmanage_cash_new = new TBMMANAGE_CASH();
                    var queryNew =
                        from tbm1302 in mpms.TBM1302
                        where tbm1302.OVC_PURCH.Equals(lblEscOVC_PURCH_6.Text.Substring(0, 7))
                        select new
                        {
                            OVC_PURCH = tbm1302.OVC_PURCH,
                            OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                            OVC_PURCH_5 = tbm1302.OVC_PURCH_5
                        };
                    foreach (var q in queryNew)
                    {
                        tbmmanage_cash_new.OVC_PURCH = q.OVC_PURCH;
                        tbmmanage_cash_new.OVC_PURCH_6 = q.OVC_PURCH_6;
                        tbmmanage_cash_new.OVC_PURCH_5 = q.OVC_PURCH_5;
                    }
                    tbmmanage_cash_new.OVC_OWN_NAME = txtOVC_OWN_NAME.Text;
                    tbmmanage_cash_new.OVC_OWN_NO = txtOVC_OWN_NO.Text;
                    tbmmanage_cash_new.OVC_OWN_ADDRESS = txtOVC_OWN_ADDRESS.Text;
                    tbmmanage_cash_new.OVC_OWN_TEL = txtOVC_OWN_TEL.Text;
                    tbmmanage_cash_new.OVC_REASON_1 = txtOVC_REASON_1.Text;
                    tbmmanage_cash_new.OVC_REASON_2 = txtOVC_REASON_2.Text;
                    tbmmanage_cash_new.OVC_REASON_3 = txtOVC_REASON_3.Text;
                    if (boolONB_MONEY_1) tbmmanage_cash_new.ONB_MONEY_1 = decimal.Parse(txtONB_MONEY_1.Text); else tbmmanage_cash_new.ONB_MONEY_1 = null;
                    if (boolONB_MONEY_2) tbmmanage_cash_new.ONB_MONEY_2 = decimal.Parse(txtONB_MONEY_2.Text); else tbmmanage_cash_new.ONB_MONEY_2 = null;
                    if (boolONB_MONEY_3) tbmmanage_cash_new.ONB_MONEY_3 = decimal.Parse(txtONB_MONEY_3.Text); else tbmmanage_cash_new.ONB_MONEY_3 = null;
                    if (boolONB_MONEY_NT_1) tbmmanage_cash_new.ONB_MONEY_NT_1 = decimal.Parse(txtONB_MONEY_NT_1.Text); else tbmmanage_cash_new.ONB_MONEY_NT_1 = null;
                    if (boolONB_MONEY_NT_2) tbmmanage_cash_new.ONB_MONEY_NT_2 = decimal.Parse(txtONB_MONEY_NT_2.Text); else tbmmanage_cash_new.ONB_MONEY_NT_2 = null;
                    if (boolONB_MONEY_NT_3) tbmmanage_cash_new.ONB_MONEY_NT_3 = decimal.Parse(txtONB_MONEY_NT_3.Text); else tbmmanage_cash_new.ONB_MONEY_NT_3 = null;
                    var query =
                        from tbm1407 in mpms.TBM1407
                        where tbm1407.OVC_PHR_CATE.Equals("B0")
                        select new
                        {
                            OVC_PHR_ID = tbm1407.OVC_PHR_ID,
                            OVC_PHR_DESC = tbm1407.OVC_PHR_DESC
                        };
                    foreach (var q in query)
                    {
                        if (drpOVC_CURRENT_1.Text == q.OVC_PHR_DESC)
                            tbmmanage_cash_new.OVC_CURRENT_1 = q.OVC_PHR_ID;
                        if (drpOVC_CURRENT_2.Text == q.OVC_PHR_DESC)
                            tbmmanage_cash_new.OVC_CURRENT_2 = q.OVC_PHR_ID;
                        if (drpOVC_CURRENT_3.Text == q.OVC_PHR_DESC)
                            tbmmanage_cash_new.OVC_CURRENT_3 = q.OVC_PHR_ID;
                    }
                    if (boolONB_ALL_MONEY) tbmmanage_cash_new.ONB_ALL_MONEY = decimal.Parse(txtONB_ALL_MONEY.Text); else tbmmanage_cash_new.ONB_ALL_MONEY = null;
                    if (rdoOVC_KIND.SelectedIndex == 0)
                        tbmmanage_cash_new.OVC_KIND = "1";
                    else if (rdoOVC_KIND.SelectedIndex == 1)
                        tbmmanage_cash_new.OVC_KIND = "2";
                    tbmmanage_cash_new.OVC_MARK = txtOVC_MARK.Text;
                    tbmmanage_cash_new.OVC_WORK_NO = txtOVC_WORK_NO.Text;
                    tbmmanage_cash_new.OVC_WORK_NAME = txtOVC_WORK_NAME.Text;
                    tbmmanage_cash_new.OVC_WORK_UNIT = txtOVC_WORK_UNIT.Text;
                    tbmmanage_cash_new.OVC_RECEIVE_NO = txtOVC_RECEIVE_NO.Text;
                    tbmmanage_cash_new.OVC_COMPTROLLER_NO = txtOVC_COMPTROLLER_NO_1.Text;
                    //if (drpOVC_COMPTROLLER_NO.Text != "請選擇")
                    //    tbmmanage_cash_new.OVC_ONNAME = drpOVC_COMPTROLLER_NO.Text;
                    //else
                    //    tbmmanage_cash_new.OVC_ONNAME = txtOVC_COMPTROLLER_NO.Text
                    tbmmanage_cash_new.OVC_ONNAME = txtOVC_COMPTROLLER_NO.Text;
                    tbmmanage_cash_new.OVC_DCOMPTROLLER = txtOVC_DCOMPTROLLER.Text;
                    tbmmanage_cash_new.OVC_BACK_NO = txtOVC_BACK_NO.Text;
                    tbmmanage_cash_new.OVC_DRECEIVE = txtOVC_DRECEIVE.Text;
                    tbmmanage_cash_new.OVC_DBACK = txtOVC_DBACK.Text;
                    tbmmanage_cash_new.OVC_BACK_REASON = txtOVC_BACK_REASON.Text;
                    tbmmanage_cash_new.OVC_BACK_MARK = txtOVC_BACK_MARK.Text;
                    tbmmanage_cash_new.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;
                    tbmmanage_cash_new.OVC_CASH_SN = Guid.NewGuid();
                    if (strmeg == "")
                    {
                        mpms.TBMMANAGE_CASH.Add(tbmmanage_cash_new);
                        mpms.SaveChanges();
                        Session["sn"] = tbmmanage_cash_new.OVC_CASH_SN.ToString();
                        Session["isModify"] = "1";
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");

                        print.Visible = true;
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strmeg);
                }
            }
        }

        #region 列印
        //收入
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            IncomeNotice_ExportToWord();
            var purch = lblEscOVC_PURCH_6.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Cash_Revenue_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "收入通知單.docx";
            string fileName = HttpUtility.UrlEncode(filepath);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            File.Delete(path_temp);
            Response.End();
        }
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            IncomeNotice_ExportToWord();
            var purch = lblEscOVC_PURCH_6.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Cash_Revenue_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/Cash_Revenue_Temp.pdf";
            string fileName = purch + "收入通知單.pdf";
            FCommon.WordToPDF(this, path_temp, wordfilepath, fileName);
        }
        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            IncomeNotice_ExportToWord();
            var purch = lblEscOVC_PURCH_6.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Cash_Revenue_Temp.docx");
            string wordfilepath = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Cash_Revenue_Temp.odt");
            string fileName = purch + "收入通知單.odt";
            FCommon.WordToOdt(this, path_temp, wordfilepath, fileName);
        }
        //退還
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            RefundNotice_ExportToWord();
            var purch = lblEscOVC_PURCH_6.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Cash_Return_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "退還通知單.docx";
            string fileName = HttpUtility.UrlEncode(filepath);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            File.Delete(path_temp);
            Response.End();
        }
        protected void LinkButton5_Click(object sender, EventArgs e)
        {
            RefundNotice_ExportToWord();
            var purch = lblEscOVC_PURCH_6.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Cash_Return_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/Cash_Return_Temp.pdf";
            string fileName = purch + "退還通知單.pdf";
            FCommon.WordToPDF(this, path_temp, wordfilepath, fileName);
        }
        protected void LinkButton6_Click(object sender, EventArgs e)
        {
            RefundNotice_ExportToWord();
            var purch = lblEscOVC_PURCH_6.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Cash_Return_Temp.docx");
            string wordfilepath = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Cash_Return_Temp.odt");
            string fileName = purch + "退還通知單.odt";
            FCommon.WordToOdt(this, path_temp, wordfilepath, fileName);
        }
        #endregion

        #endregion

        #region 副程式

        #region Table資料帶入
        private void TB_dataImport()
        {
            if (Session["rowtext"] != null)
            {
                lblEscOVC_PURCH_6.Text = Session["rowtext"].ToString();
                var purch = Session["rowtext"].ToString().Substring(0, 7);
                var queryPurch =
                    from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                    where tbmreceive.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_PUR_USER = tbmreceive.OVC_PUR_USER,
                        OVC_PUR_NSECTION = tbmreceive.OVC_PUR_NSECTION,
                        OVC_PUR_IUSER_PHONE = tbmreceive.OVC_PUR_IUSER_PHONE,
                        OVC_PUR_IUSER_PHONE_EXT = tbmreceive.OVC_PUR_IUSER_PHONE_EXT,
                        OVC_VEN_TITLE = tbmreceive.OVC_VEN_TITLE,
                        OVC_VEN_CST = tbmreceive.OVC_VEN_CST,
                        OVC_VEN_ADDRESS = tbmreceive.OVC_VEN_ADDRESS,
                        OVC_VEN_TEL = tbmreceive.OVC_VEN_TEL
                    };
                var query =
                    from tbmmanage_cash in mpms.TBMMANAGE_CASH
                    where tbmmanage_cash.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_OWN_NO = tbmmanage_cash.OVC_OWN_NO,
                        OVC_OWN_ADDRESS = tbmmanage_cash.OVC_OWN_ADDRESS,
                        OVC_OWN_TEL = tbmmanage_cash.OVC_OWN_TEL,
                        OVC_REASON_1 = tbmmanage_cash.OVC_REASON_1,
                        OVC_CURRENT_1 = tbmmanage_cash.OVC_CURRENT_1,
                        ONB_MONEY_1 = tbmmanage_cash.ONB_MONEY_1,
                        ONB_MONEY_NT_1 = tbmmanage_cash.ONB_MONEY_NT_1,
                        OVC_REASON_2 = tbmmanage_cash.OVC_REASON_2,
                        OVC_CURRENT_2 = tbmmanage_cash.OVC_CURRENT_2,
                        ONB_MONEY_2 = tbmmanage_cash.ONB_MONEY_2,
                        ONB_MONEY_NT_2 = tbmmanage_cash.ONB_MONEY_NT_2,
                        OVC_REASON_3 = tbmmanage_cash.OVC_REASON_3,
                        OVC_CURRENT_3 = tbmmanage_cash.OVC_CURRENT_3,
                        ONB_MONEY_3 = tbmmanage_cash.ONB_MONEY_3,
                        ONB_MONEY_NT_3 = tbmmanage_cash.ONB_MONEY_NT_3,
                        ONB_ALL_MONEY = tbmmanage_cash.ONB_ALL_MONEY,
                        OVC_KIND = tbmmanage_cash.OVC_KIND,
                        OVC_MARK = tbmmanage_cash.OVC_MARK,
                        OVC_WORK_NO = tbmmanage_cash.OVC_WORK_NO,
                        OVC_WORK_NAME = tbmmanage_cash.OVC_WORK_NAME,
                        OVC_WORK_UNIT = tbmmanage_cash.OVC_WORK_UNIT,
                        OVC_RECEIVE_NO = tbmmanage_cash.OVC_RECEIVE_NO,
                        OVC_DRECEIVE = tbmmanage_cash.OVC_DRECEIVE,
                        OVC_COMPTROLLER_NO = tbmmanage_cash.OVC_COMPTROLLER_NO,
                        OVC_DCOMPTROLLER = tbmmanage_cash.OVC_DCOMPTROLLER,
                        OVC_ONNAME = tbmmanage_cash.OVC_ONNAME,
                        OVC_BACK_NO = tbmmanage_cash.OVC_BACK_NO,
                        OVC_DBACK = tbmmanage_cash.OVC_DBACK,
                        OVC_BACK_REASON = tbmmanage_cash.OVC_BACK_REASON,
                        OVC_BACK_MARK = tbmmanage_cash.OVC_BACK_MARK,
                        OVC_DAPPROVE = tbmmanage_cash.OVC_DAPPROVE,
                        OVC_CASH_SN = tbmmanage_cash.OVC_CASH_SN
                    };
                foreach (var q in queryPurch)
                {
                    txtOVC_OWN_NAME.Text = q.OVC_VEN_TITLE;
                    txtOVC_OWN_NO.Text = q.OVC_VEN_CST;
                    txtOVC_OWN_ADDRESS.Text = q.OVC_VEN_ADDRESS;
                    txtOVC_OWN_TEL.Text = q.OVC_VEN_TEL;
                    lblOVC_PUR_USER.Text = q.OVC_PUR_USER;
                    lblOVC_PUR_NSECTION.Text = "";//職級
                    lblOVC_PUR_IUSER_PHONE.Text = q.OVC_PUR_IUSER_PHONE;
                    lblOVC_PUR_IUSER_PHONE_EXT.Text = q.OVC_PUR_IUSER_PHONE_EXT;
                }
                if (Session["sn"] != null)
                {
                    foreach (var q in query)
                    {
                        if (q.OVC_CASH_SN.ToString().Equals(Session["sn"].ToString()))
                        {
                            txtOVC_REASON_1.Text = q.OVC_REASON_1;
                            txtONB_MONEY_1.Text = String.Format("{0:N}", q.ONB_MONEY_1 ?? 0);
                            txtONB_MONEY_NT_1.Text = String.Format("{0:N}", q.ONB_MONEY_NT_1 ?? 0);
                            txtOVC_REASON_2.Text = q.OVC_REASON_2;
                            txtONB_MONEY_2.Text = String.Format("{0:N}", q.ONB_MONEY_2 ?? 0);
                            txtONB_MONEY_NT_2.Text = String.Format("{0:N}", q.ONB_MONEY_NT_2 ?? 0);
                            txtOVC_REASON_3.Text = q.OVC_REASON_3;
                            txtONB_MONEY_3.Text = String.Format("{0:N}", q.ONB_MONEY_3 ?? 0);
                            txtONB_MONEY_NT_3.Text = String.Format("{0:N}", q.ONB_MONEY_NT_3 ?? 0);
                            txtONB_ALL_MONEY.Text = String.Format("{0:N}", q.ONB_ALL_MONEY ?? 0);
                            if (q.OVC_KIND == "1")
                                rdoOVC_KIND.SelectedIndex = 0;
                            else if (q.OVC_KIND == "2")
                                rdoOVC_KIND.SelectedIndex = 1;
                            txtOVC_MARK.Text = q.OVC_MARK;
                            txtOVC_WORK_NO.Text = q.OVC_WORK_NO;
                            txtOVC_WORK_NAME.Text = q.OVC_WORK_NAME;
                            txtOVC_WORK_UNIT.Text = q.OVC_WORK_UNIT;
                            txtOVC_RECEIVE_NO.Text = q.OVC_RECEIVE_NO;
                            txtOVC_DRECEIVE.Text = q.OVC_DRECEIVE;
                            //drpOVC_COMPTROLLER_NO.Text = q.OVC_ONNAME;
                            //if (drpOVC_COMPTROLLER_NO.Text == "請選擇")
                            //    txtOVC_COMPTROLLER_NO.Text = q.OVC_ONNAME;
                            txtOVC_COMPTROLLER_NO.Text = q.OVC_ONNAME;
                            txtOVC_COMPTROLLER_NO_1.Text = q.OVC_COMPTROLLER_NO;
                            txtOVC_DCOMPTROLLER.Text = q.OVC_DCOMPTROLLER;
                            txtOVC_BACK_NO.Text = q.OVC_BACK_NO;
                            txtOVC_DBACK.Text = q.OVC_DBACK;
                            txtOVC_DAPPROVE.Text = q.OVC_DAPPROVE;
                            txtOVC_BACK_REASON.Text = q.OVC_BACK_REASON;
                            txtOVC_BACK_MARK.Text = q.OVC_BACK_MARK;
                        }
                    }
                }
            }
        }
        #endregion

        #region 標準片語
        private void list_dataImport(ListControl list, string cate)
        {
            list.Items.Clear();
            list.Items.Add("請選擇");
            var query =
                from tbm1407 in mpms.TBM1407
                where tbm1407.OVC_PHR_CATE.Equals(cate)
                select tbm1407.OVC_PHR_DESC;
            foreach (var q in query)
                list.Items.Add(q);
        }
        #endregion

        #region 幣別
        private void list_dataImport(ListControl list1, ListControl list2, ListControl list3)
        {
            list1.Items.Clear();
            list2.Items.Clear();
            list3.Items.Clear();
            var query1407 =
                from tbm1407 in mpms.TBM1407
                where tbm1407.OVC_PHR_CATE.Equals("B0")
                select new
                {
                    tbm1407.OVC_PHR_ID,
                    tbm1407.OVC_PHR_DESC
                };
            foreach (var qu in query1407)
            {
                list1.Items.Add(qu.OVC_PHR_DESC);
                list2.Items.Add(qu.OVC_PHR_DESC);
                list3.Items.Add(qu.OVC_PHR_DESC);
                var queryCurrency =
                    from cash in mpms.TBMMANAGE_CASH
                    where cash.OVC_PURCH.Equals(lblEscOVC_PURCH_6.Text.Substring(0, 7))
                    select new
                    {
                        cash.OVC_CURRENT_1,
                        cash.OVC_CURRENT_2,
                        cash.OVC_CURRENT_3
                    };
                foreach (var q in queryCurrency)
                {
                    if (q.OVC_CURRENT_1 == qu.OVC_PHR_ID)
                        list1.Text = qu.OVC_PHR_DESC;
                    else if (q.OVC_CURRENT_1 == null)
                        list1.Text = "新臺幣";
                    if (q.OVC_CURRENT_2 == qu.OVC_PHR_ID)
                        list2.Text = qu.OVC_PHR_DESC;
                    else if (q.OVC_CURRENT_2 == null)
                        list2.Text = "新臺幣";
                    if (q.OVC_CURRENT_3 == qu.OVC_PHR_ID)
                        list3.Text = qu.OVC_PHR_DESC;
                    else if (q.OVC_CURRENT_3 == null)
                        list3.Text = "新臺幣";
                }
            }
        }
        #endregion

        #region 收入通知單預覽列印
        void IncomeNotice_ExportToWord()
        {
            decimal money = 0;
            string Money = "";
            string path = "";
            var purch = lblEscOVC_PURCH_6.Text.Substring(0, 7);
            path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/ReceivePurchaseServletE1D_1.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    if (Session["sn"] != null)
                    {
                        Guid guid = Guid.Parse(Session["sn"].ToString());
                        var queryCash =
                            from cash in mpms.TBMMANAGE_CASH
                            where cash.OVC_PURCH.Equals(purch)
                            where cash.OVC_CASH_SN.Equals(guid)
                            select new
                            {
                                OVC_OWN_NAME = cash.OVC_OWN_NAME,
                                OVC_OWN_NO = cash.OVC_OWN_NO,
                                OVC_OWN_ADDRESS = cash.OVC_OWN_ADDRESS,
                                OVC_OWN_TEL = cash.OVC_OWN_TEL,
                                OVC_REASON_1 = cash.OVC_REASON_1,
                                OVC_CURRENT_1 = cash.OVC_CURRENT_1,
                                ONB_MONEY_1 = cash.ONB_MONEY_1,
                                ONB_MONEY_NT_1 = cash.ONB_MONEY_NT_1,
                                OVC_REASON_2 = cash.OVC_REASON_2,
                                OVC_CURRENT_2 = cash.OVC_CURRENT_2,
                                ONB_MONEY_2 = cash.ONB_MONEY_2,
                                ONB_MONEY_NT_2 = cash.ONB_MONEY_NT_2,
                                OVC_REASON_3 = cash.OVC_REASON_3,
                                OVC_CURRENT_3 = cash.OVC_CURRENT_3,
                                ONB_MONEY_3 = cash.ONB_MONEY_3,
                                ONB_MONEY_NT_3 = cash.ONB_MONEY_NT_3,
                                ONB_ALL_MONEY = cash.ONB_ALL_MONEY,
                                OVC_MARK = cash.OVC_MARK,
                                OVC_WORK_NO = cash.OVC_WORK_NO,
                                OVC_WORK_NAME = cash.OVC_WORK_NAME,
                                OVC_WORK_UNIT = cash.OVC_WORK_UNIT,
                                OVC_RECEIVE_NO = cash.OVC_RECEIVE_NO,
                                OVC_DRECEIVE = cash.OVC_DRECEIVE
                            };
                        if (queryCash.Count() < 1)
                            return;
                        foreach (var q in queryCash)
                        {
                            doc.ReplaceText("[$OWN_NAME$]", q.OVC_OWN_NAME != null ? q.OVC_OWN_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OWN_NO$]", q.OVC_OWN_NO != null ? q.OVC_OWN_NO : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OWN_ADDRESS$]", q.OVC_OWN_ADDRESS != null ? q.OVC_OWN_ADDRESS : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OWN_TEL$]", q.OVC_OWN_TEL != null ? q.OVC_OWN_TEL : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$REASON_1$]", q.OVC_REASON_1 != null ? q.OVC_REASON_1 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY_1$]", q.ONB_MONEY_1 != null ? String.Format("{0:N}", q.ONB_MONEY_1) : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY_NT_1$]", q.ONB_MONEY_NT_1 != null ? String.Format("{0:N}", q.ONB_MONEY_NT_1) : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$REASON_2$]", q.OVC_REASON_2 != null ?q.OVC_REASON_2 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY_2$]", q.ONB_MONEY_2 != null ? String.Format("{0:N}", q.ONB_MONEY_2) : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY_NT_2$]", q.ONB_MONEY_NT_2 != null ? String.Format("{0:N}", q.ONB_MONEY_NT_2) : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$REASON_3$]", q.OVC_REASON_3 != null ? q.OVC_REASON_3 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY_3$]", q.ONB_MONEY_3 != null ? String.Format("{0:N}", q.ONB_MONEY_3) : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY_NT_3$]", q.ONB_MONEY_NT_3 != null ? String.Format("{0:N}", q.ONB_MONEY_NT_3) : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            var query1407 =
                                from tbm1407 in mpms.TBM1407
                                where tbm1407.OVC_PHR_CATE.Equals("B0")
                                select new
                                {
                                    OVC_PHR_ID = tbm1407.OVC_PHR_ID,
                                    OVC_PHR_DESC = tbm1407.OVC_PHR_DESC
                                };
                            foreach (var qu in query1407)
                            {
                                if (q.OVC_CURRENT_1 == qu.OVC_PHR_ID)
                                    doc.ReplaceText("[$CURRENT_1$]", qu.OVC_PHR_DESC, false, System.Text.RegularExpressions.RegexOptions.None);
                                if (q.OVC_CURRENT_2 == qu.OVC_PHR_ID)
                                    doc.ReplaceText("[$CURRENT_2$]", qu.OVC_PHR_DESC, false, System.Text.RegularExpressions.RegexOptions.None);
                                if (q.OVC_CURRENT_3 == qu.OVC_PHR_ID)
                                    doc.ReplaceText("[$CURRENT_3$]", qu.OVC_PHR_DESC, false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            doc.ReplaceText("[$CURRENT_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CURRENT_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CURRENT_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ALL_MONEY != null)
                            {
                                money = decimal.Parse(q.ONB_ALL_MONEY.ToString());
                                Money = EastAsiaNumericFormatter.FormatWithCulture("Lc", money, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                                doc.ReplaceText("[$ALL_MONEY$]", Money, false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            doc.ReplaceText("[$ALL_MONEY$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$txtOVC_MARK$]", q.OVC_MARK != null ? q.OVC_MARK : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$WORK_NO$]", q.OVC_WORK_NO != null ? q.OVC_WORK_NO : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$WORK_NAME$]", q.OVC_WORK_NAME != null ? q.OVC_WORK_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$WORK_UNIT$]", q.OVC_WORK_UNIT != null ? q.OVC_WORK_UNIT : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$RECEIVE_NO$]", q.OVC_RECEIVE_NO != null ? q.OVC_RECEIVE_NO : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.OVC_DRECEIVE != null)
                            {
                                int year = int.Parse(q.OVC_DRECEIVE.Substring(0, 4)) - 1911;
                                string date = year.ToString() + "年" + q.OVC_DRECEIVE.Substring(5, 2) + "月" + q.OVC_DRECEIVE.Substring(8, 2) + "日";
                                doc.ReplaceText("[$OVC_DRECEIVE$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            doc.ReplaceText("[$OVC_DRECEIVE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                    else
                        return;

                    doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/Cash_Revenue_Temp.docx");
                }
                buffer = ms.ToArray();
            }
        }
        #endregion

        #region 退還通知單預覽列印
        void RefundNotice_ExportToWord()
        {
            decimal money = 0;
            string Money = "";
            string path = "";
            var purch = lblEscOVC_PURCH_6.Text.Substring(0, 7);
            path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/ReceivePurchaseServletE1D_2.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    if (Session["sn"] != null)
                    {
                        Guid guid = Guid.Parse(Session["sn"].ToString());
                        var queryCash =
                            from cash in mpms.TBMMANAGE_CASH
                            where cash.OVC_PURCH.Equals(purch)
                            where cash.OVC_CASH_SN.Equals(guid)
                            select new
                            {
                                OVC_WORK_NAME = cash.OVC_WORK_NAME,
                                OVC_BACK_NO = cash.OVC_BACK_NO,
                                OVC_WORK_NO = cash.OVC_WORK_NO,
                                OVC_DRECEIVE = cash.OVC_DRECEIVE,
                                OVC_RECEIVE_NO = cash.OVC_RECEIVE_NO,
                                OVC_OWN_NAME = cash.OVC_OWN_NAME,
                                OVC_REASON_1 = cash.OVC_REASON_1,
                                OVC_REASON_2 = cash.OVC_REASON_2,
                                OVC_REASON_3 = cash.OVC_REASON_3,
                                OVC_DCOMPTROLLER = cash.OVC_DCOMPTROLLER,
                                OVC_CURRENT_1 = cash.OVC_CURRENT_1,
                                OVC_CURRENT_2 = cash.OVC_CURRENT_2,
                                OVC_CURRENT_3 = cash.OVC_CURRENT_3,
                                ONB_MONEY_1 = cash.ONB_MONEY_1,
                                ONB_MONEY_2 = cash.ONB_MONEY_2,
                                ONB_MONEY_3 = cash.ONB_MONEY_3,
                                ONB_MONEY_NT_1 = cash.ONB_MONEY_NT_1,
                                ONB_MONEY_NT_2 = cash.ONB_MONEY_NT_2,
                                ONB_MONEY_NT_3 = cash.ONB_MONEY_NT_3,
                                ONB_ALL_MONEY = cash.ONB_ALL_MONEY,
                                OVC_BACK_REASON = cash.OVC_BACK_REASON,
                                OVC_BACK_MARK = cash.OVC_BACK_MARK
                            };
                        if (queryCash.Count() < 1)
                            return;
                        foreach (var q in queryCash)
                        {
                            doc.ReplaceText("[$OVC_WORK_NAME$]", q.OVC_WORK_NAME != null ? q.OVC_WORK_NAME : "國防部國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_BACK_NO$]", q.OVC_BACK_NO != null ? q.OVC_BACK_NO : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$WORK_NO$]", q.OVC_WORK_NO != null ? q.OVC_WORK_NO : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            string d = DateTime.Now.ToString("yyyy-MM-dd");
                            int year = int.Parse(d.Substring(0, 4)) - 1911;
                            string date = year.ToString() + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day + "日";
                            doc.ReplaceText("[$TODAY$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$DRECEIVE$]", getTaiwanDate(q.OVC_DRECEIVE), false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$RECEIVE_NO$]", q.OVC_RECEIVE_NO != null ? q.OVC_RECEIVE_NO : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OWN_NAME$]", q.OVC_OWN_NAME != null ? q.OVC_OWN_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$REASON_1$]", q.OVC_REASON_1 != null ? q.OVC_REASON_1 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$REASON_2$]", q.OVC_REASON_2 != null ? q.OVC_REASON_2 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$REASON_3$]", q.OVC_REASON_3 != null ? q.OVC_REASON_3 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.OVC_DCOMPTROLLER != null)
                            {
                                if (q.OVC_REASON_1 != null)
                                {
                                    year = int.Parse(q.OVC_DCOMPTROLLER.Substring(0, 4)) - 1911;
                                    date = year.ToString();
                                    doc.ReplaceText("[$YYY1$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                                    date = q.OVC_DCOMPTROLLER.Substring(5, 2);
                                    doc.ReplaceText("[$MM1$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                                    date = q.OVC_DCOMPTROLLER.Substring(8, 2);
                                    doc.ReplaceText("[$DD1$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                                }
                                else
                                {
                                    doc.ReplaceText("[$YYY1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                    doc.ReplaceText("[$MM1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                    doc.ReplaceText("[$DD1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                }
                                if (q.OVC_REASON_2 != null)
                                {
                                    year = int.Parse(q.OVC_DCOMPTROLLER.Substring(0, 4)) - 1911;
                                    date = year.ToString();
                                    doc.ReplaceText("[$YYY2$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                                    date = q.OVC_DCOMPTROLLER.Substring(5, 2);
                                    doc.ReplaceText("[$MM2$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                                    date = q.OVC_DCOMPTROLLER.Substring(8, 2);
                                    doc.ReplaceText("[$DD2$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                                }
                                else
                                {
                                    doc.ReplaceText("[$YYY2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                    doc.ReplaceText("[$MM2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                    doc.ReplaceText("[$DD2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                }
                                if (q.OVC_REASON_3 != null)
                                {
                                    year = int.Parse(q.OVC_DCOMPTROLLER.Substring(0, 4)) - 1911;
                                    date = year.ToString();
                                    doc.ReplaceText("[$YYY3$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                                    date = q.OVC_DCOMPTROLLER.Substring(5, 2);
                                    doc.ReplaceText("[$MM3$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                                    date = q.OVC_DCOMPTROLLER.Substring(8, 2);
                                    doc.ReplaceText("[$DD3$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                                }
                                else
                                {
                                    doc.ReplaceText("[$YYY3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                    doc.ReplaceText("[$MM3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                    doc.ReplaceText("[$DD3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                }
                            }
                            else
                            {
                                doc.ReplaceText("[$YYY1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$YYY2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$YYY3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$MM1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$MM2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$MM3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$DD1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$DD2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$DD3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            var queryCurrency =
                                from tbm1407 in mpms.TBM1407
                                where tbm1407.OVC_PHR_CATE.Equals("B0")
                                where tbm1407.OVC_PHR_ID.Equals(q.OVC_CURRENT_1)
                                select tbm1407.OVC_PHR_DESC;
                            foreach (var qu in queryCurrency)
                                if (q.OVC_CURRENT_1 != null) doc.ReplaceText("[$CURRENT_1$]", qu, false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CURRENT_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            var queryCurrency2 =
                                from tbm1407 in mpms.TBM1407
                                where tbm1407.OVC_PHR_CATE.Equals("B0")
                                where tbm1407.OVC_PHR_ID.Equals(q.OVC_CURRENT_2)
                                select tbm1407.OVC_PHR_DESC;
                            foreach (var qu in queryCurrency)
                                if (q.OVC_CURRENT_2 != null) doc.ReplaceText("[$CURRENT_2$]", qu, false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CURRENT_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            var queryCurrency3 =
                                from tbm1407 in mpms.TBM1407
                                where tbm1407.OVC_PHR_CATE.Equals("B0")
                                where tbm1407.OVC_PHR_ID.Equals(q.OVC_CURRENT_3)
                                select tbm1407.OVC_PHR_DESC;
                            foreach (var qu in queryCurrency)
                                if (q.OVC_CURRENT_3 != null) doc.ReplaceText("[$CURRENT_3$]", qu, false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CURRENT_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY_1$]", q.ONB_MONEY_1 != null ? String.Format("{0:N}", q.ONB_MONEY_1) : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY_2$]", q.ONB_MONEY_2 != null ? String.Format("{0:N}", q.ONB_MONEY_2) : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY_3$]", q.ONB_MONEY_3 != null ? String.Format("{0:N}", q.ONB_MONEY_3) : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY_NT_1$]", q.ONB_MONEY_NT_1 != null ? String.Format("{0:N}", q.ONB_MONEY_NT_1) : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY_NT_2$]", q.ONB_MONEY_NT_2 != null ? String.Format("{0:N}", q.ONB_MONEY_NT_2) : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY_NT_3$]", q.ONB_MONEY_NT_3 != null ? String.Format("{0:N}", q.ONB_MONEY_NT_3) : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ALL_MONEY != null)
                            {
                                money = decimal.Parse(q.ONB_ALL_MONEY.ToString());
                                Money = EastAsiaNumericFormatter.FormatWithCulture("Lc", money, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                                doc.ReplaceText("[$ALL_MONEY$]", Money, false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            doc.ReplaceText("[$ALL_MONEY$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$BACK_REASON$]", q.OVC_BACK_REASON != null ? q.OVC_BACK_REASON.ToString() : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$BACK_MARK$]", q.OVC_BACK_MARK != null ? q.OVC_BACK_MARK.ToString() : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_BACK_NO$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                        doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/Cash_Return_Temp.docx");
                    }
                    else
                        return;
                    buffer = ms.ToArray();
                }
            }
        }
        #endregion

        #region drp資料帶入
        private void drp_dataImport()
        {
            if (Session["rowtext"] != null)
            {
                var purch = Session["rowtext"].ToString().Substring(0, 7);
                var query =
                      from cash in mpms.TBMMANAGE_CASH
                      where cash.OVC_PURCH.Equals(purch)
                      select cash.OVC_ONNAME;
                //foreach (var e in query)
                //    drpOVC_COMPTROLLER_NO.Items.Add(e);
            }
        }
        #endregion

        protected void drpOVC_BACK_REASON_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpOVC_BACK_REASON.Text != "請選擇")
                txtOVC_BACK_REASON.Text = drpOVC_BACK_REASON.Text;
        }

        protected void drpOVC_BACK_MARK_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpOVC_BACK_MARK.Text != "請選擇")
                txtOVC_BACK_MARK.Text = drpOVC_BACK_MARK.Text;
        }

        protected void txtONB_MONEY_NT_TextChanged(object sender, EventArgs e)
        {
            decimal decNT1 = 0, decNT2 = 0, decNT3 = 0;
            bool boolNT1 = string.Empty.Equals(txtONB_MONEY_NT_1.Text) ? true : decimal.TryParse(txtONB_MONEY_NT_1.Text, out decNT1);
            bool boolNT2 = string.Empty.Equals(txtONB_MONEY_NT_2.Text) ? true : decimal.TryParse(txtONB_MONEY_NT_2.Text, out decNT2);
            bool boolNT3 = string.Empty.Equals(txtONB_MONEY_NT_3.Text) ? true : decimal.TryParse(txtONB_MONEY_NT_3.Text, out decNT3);
            if (boolNT1 && boolNT2 && boolNT3)
            {
                txtONB_ALL_MONEY.Text = (decNT1 + decNT2 + decNT3).ToString();
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", " 新台幣入帳金額 須為數字！");
        }

        #region getTaiwanDate
        private string getTaiwanDate(string strDate)
        {
            if (DateTime.TryParse(strDate, out DateTime datetime))
            {
                CultureInfo culture = new CultureInfo("zh-TW");
                culture.DateTimeFormat.Calendar = new TaiwanCalendar();
                return datetime.ToString("yyy年MM月dd日", culture);
            }
            else
                return "";
        }
        #endregion
        #endregion
    }
}