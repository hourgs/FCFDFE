using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using Microsoft.International.Formatters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xceed.Words.NET;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E1E : System.Web.UI.Page
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
                        FCommon.Controls_Attributes("readonly", "true", txtExpDate_1, txtExpDate_2, txtExpDate_3, txtOVC_DRECEIVE, txtOVC_DCOMPTROLLER, txtOVC_DBACK, txtOVC_DAPPROVE);
                        list_dataImport(drpOVC_BACK_REASON, "HA");
                        list_dataImport(drpOVC_BACK_MARK, "HB");
                        TB_dataImport();
                        list_dataImport(drpOVC_CURRENT);
                    }
                }
                else
                    FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
            }
        }

        #region Click
        //存檔
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strmeg = "";
            if (txtOVC_OWN_NO.Text == "")
                strmeg += "<p>請填選 保證人及保證人統一編號</p>";
            if (txtOVC_WORK_NO.Text == "")
                strmeg += "<p>請填選 承辦單位及承辦單位統一編號</p>";
            bool boolONB_MONEY_1 = FCommon.checkDecimal(txtONB_MONEY_1.Text, "入帳金額", ref strmeg, out decimal d);
            bool boolONB_MONEY_2 = FCommon.checkDecimal(txtONB_MONEY_2.Text, "入帳金額", ref strmeg, out d);
            bool boolONB_MONEY_3 = FCommon.checkDecimal(txtONB_MONEY_3.Text, "入帳金額", ref strmeg, out d);
            bool boolONB_ALL_MONEY = FCommon.checkDecimal(txtONB_ALL_MONEY.Text, "合計金額", ref strmeg, out d);

            if (Session["sn"] != null)
            {
                Guid guid = Guid.Parse(Session["sn"].ToString());
                TBMMANAGE_PROM tbmmanage_prom = new TBMMANAGE_PROM();
                tbmmanage_prom = mpms.TBMMANAGE_PROM
                    .Where(table => table.OVC_PROM_SN.Equals(guid))
                    .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH_6.Text.Substring(0, 7))).FirstOrDefault();
                if (tbmmanage_prom != null)
                {
                    tbmmanage_prom.OVC_OWN_NO = txtOVC_OWN_NO.Text;
                    tbmmanage_prom.OVC_OWN_NAME = txtOVC_OWN_NAME.Text;
                    tbmmanage_prom.OVC_OWN_ADDRESS = txtOVC_OWN_ADDRESS.Text;
                    tbmmanage_prom.OVC_OWNED_NAME = txtOVC_OWNED_NAME.Text;
                    tbmmanage_prom.OVC_OWNED_NO = txtOVC_OWNED_NO.Text;
                    tbmmanage_prom.OVC_OWNED_ADDRESS = txtOVC_OWNED_ADDRESS.Text;
                    tbmmanage_prom.OVC_REASON_1 = txtOVC_REASON_1.Text;
                    tbmmanage_prom.OVC_REASON_2 = txtOVC_REASON_2.Text;
                    tbmmanage_prom.OVC_REASON_3 = txtOVC_REASON_3.Text;
                    tbmmanage_prom.OVC_NUMBER_1 = txtOVC_NUMBER_1.Text;
                    tbmmanage_prom.OVC_NUMBER_2 = txtOVC_NUMBER_2.Text;
                    tbmmanage_prom.OVC_NUMBER_3 = txtOVC_NUMBER_3.Text;
                    tbmmanage_prom.OVC_NSTOCK_1 = txtOVC_NSTOCK_1.Text;
                    tbmmanage_prom.OVC_NSTOCK_2 = txtOVC_NSTOCK_2.Text;
                    tbmmanage_prom.OVC_NSTOCK_3 = txtOVC_NSTOCK_3.Text;
                    if (boolONB_MONEY_1) tbmmanage_prom.ONB_MONEY_1 = decimal.Parse(txtONB_MONEY_1.Text); else tbmmanage_prom.ONB_MONEY_1 = null;
                    if (boolONB_MONEY_2) tbmmanage_prom.ONB_MONEY_2 = decimal.Parse(txtONB_MONEY_2.Text); else tbmmanage_prom.ONB_MONEY_2 = null;
                    if (boolONB_MONEY_3) tbmmanage_prom.ONB_MONEY_3 = decimal.Parse(txtONB_MONEY_3.Text); else tbmmanage_prom.ONB_MONEY_3 = null;
                    tbmmanage_prom.OVC_DEFFECT_1 = txtExpDate_1.Text;
                    tbmmanage_prom.OVC_DEFFECT_2 = txtExpDate_2.Text;
                    tbmmanage_prom.OVC_DEFFECT_3 = txtExpDate_3.Text;
                    if (boolONB_ALL_MONEY) tbmmanage_prom.ONB_ALL_MONEY = decimal.Parse(txtONB_ALL_MONEY.Text); else tbmmanage_prom.ONB_ALL_MONEY = null;
                    var query =
                        from tbm1407 in mpms.TBM1407
                        where tbm1407.OVC_PHR_CATE.Equals("B0")
                        select new
                        {
                            OVC_PHR_ID = tbm1407.OVC_PHR_ID,
                            OVC_PHR_DESC = tbm1407.OVC_PHR_DESC
                        };
                    foreach (var q in query)
                        if (drpOVC_CURRENT.Text == q.OVC_PHR_DESC) tbmmanage_prom.OVC_CURRENT = q.OVC_PHR_ID;
                    if (rdoOVC_KIND.SelectedIndex == 0)
                        tbmmanage_prom.OVC_KIND = "1";
                    else if (rdoOVC_KIND.SelectedIndex == 1)
                        tbmmanage_prom.OVC_KIND = "2";
                    tbmmanage_prom.OVC_MARK = txtOVC_MARK.Text;
                    tbmmanage_prom.OVC_WORK_NO = txtOVC_WORK_NO.Text;
                    tbmmanage_prom.OVC_WORK_NAME = txtOVC_WORK_NAME.Text;
                    tbmmanage_prom.OVC_WORK_UNIT = txtOVC_WORK_UNIT.Text;
                    tbmmanage_prom.OVC_RECEIVE_NO = txtOVC_RECEIVE_NO.Text;
                    tbmmanage_prom.OVC_DRECEIVE = txtOVC_DRECEIVE.Text;
                    //if (drpDocOVC_COMPTROLLER_NO.Text == "請選擇")
                    //    tbmmanage_prom.OVC_ONNAME = txtOVC_COMPTROLLER_NO.Text;
                    //else
                    //    tbmmanage_prom.OVC_ONNAME = drpDocOVC_COMPTROLLER_NO.Text;
                    tbmmanage_prom.OVC_ONNAME = txtOVC_COMPTROLLER_NO.Text;
                    tbmmanage_prom.OVC_COMPTROLLER_NO = txtOVC_COMPTROLLER_NO_1.Text;
                    tbmmanage_prom.OVC_DCOMPTROLLER = txtOVC_DCOMPTROLLER.Text;
                    tbmmanage_prom.OVC_BACK_NO = txtOVC_BACK_NO.Text;
                    tbmmanage_prom.OVC_DBACK = txtOVC_DBACK.Text;
                    tbmmanage_prom.OVC_BACK_REASON = txtOVC_BACK_REASON.Text;
                    tbmmanage_prom.OVC_BACK_MARK = txtOVC_BACK_MARK.Text;
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
                var check = mpms.TBMMANAGE_PROM.Where(o => o.OVC_WORK_NO.Equals(txtOVC_WORK_NO.Text)).FirstOrDefault();
                if (check != null)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "承辦單位不可重複");
                else
                {
                    TBMMANAGE_PROM tbmmanage_prom_new = new TBMMANAGE_PROM();
                    var queryNew =
                        from tbm1302 in mpms.TBM1302
                        where tbm1302.OVC_PURCH.Equals(lblOVC_PURCH_6.Text.Substring(0, 7))
                        select new
                        {
                            OVC_PURCH = tbm1302.OVC_PURCH,
                            OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                            OVC_PURCH_5 = tbm1302.OVC_PURCH_5
                        };
                    foreach (var q in queryNew)
                    {
                        tbmmanage_prom_new.OVC_PURCH = q.OVC_PURCH;
                        tbmmanage_prom_new.OVC_PURCH_6 = q.OVC_PURCH_6;
                        tbmmanage_prom_new.OVC_PURCH_5 = q.OVC_PURCH_5;
                    }
                    tbmmanage_prom_new.OVC_OWN_NO = txtOVC_OWN_NO.Text;
                    tbmmanage_prom_new.OVC_OWN_NAME = txtOVC_OWN_NAME.Text;
                    tbmmanage_prom_new.OVC_OWN_ADDRESS = txtOVC_OWN_ADDRESS.Text;
                    tbmmanage_prom_new.OVC_OWNED_NAME = txtOVC_OWNED_NAME.Text;
                    tbmmanage_prom_new.OVC_OWNED_NO = txtOVC_OWNED_NO.Text;
                    tbmmanage_prom_new.OVC_OWNED_ADDRESS = txtOVC_OWNED_ADDRESS.Text;
                    tbmmanage_prom_new.OVC_REASON_1 = txtOVC_REASON_1.Text;
                    tbmmanage_prom_new.OVC_REASON_2 = txtOVC_REASON_2.Text;
                    tbmmanage_prom_new.OVC_REASON_3 = txtOVC_REASON_3.Text;
                    tbmmanage_prom_new.OVC_NUMBER_1 = txtOVC_NUMBER_1.Text;
                    tbmmanage_prom_new.OVC_NUMBER_2 = txtOVC_NUMBER_2.Text;
                    tbmmanage_prom_new.OVC_NUMBER_3 = txtOVC_NUMBER_3.Text;
                    tbmmanage_prom_new.OVC_NSTOCK_1 = txtOVC_NSTOCK_1.Text;
                    tbmmanage_prom_new.OVC_NSTOCK_2 = txtOVC_NSTOCK_2.Text;
                    tbmmanage_prom_new.OVC_NSTOCK_3 = txtOVC_NSTOCK_3.Text;
                    if (boolONB_MONEY_1) tbmmanage_prom_new.ONB_MONEY_1 = decimal.Parse(txtONB_MONEY_1.Text); else tbmmanage_prom_new.ONB_MONEY_1 = null;
                    if (boolONB_MONEY_2) tbmmanage_prom_new.ONB_MONEY_2 = decimal.Parse(txtONB_MONEY_2.Text); else tbmmanage_prom_new.ONB_MONEY_2 = null;
                    if (boolONB_MONEY_3) tbmmanage_prom_new.ONB_MONEY_3 = decimal.Parse(txtONB_MONEY_3.Text); else tbmmanage_prom_new.ONB_MONEY_3 = null;
                    tbmmanage_prom_new.OVC_DEFFECT_1 = txtExpDate_1.Text;
                    tbmmanage_prom_new.OVC_DEFFECT_2 = txtExpDate_2.Text;
                    tbmmanage_prom_new.OVC_DEFFECT_3 = txtExpDate_3.Text;
                    if (boolONB_ALL_MONEY) tbmmanage_prom_new.ONB_ALL_MONEY = decimal.Parse(txtONB_ALL_MONEY.Text); else tbmmanage_prom_new.ONB_ALL_MONEY = null;
                    var query =
                        from tbm1407 in mpms.TBM1407
                        where tbm1407.OVC_PHR_CATE.Equals("B0")
                        select new
                        {
                            OVC_PHR_ID = tbm1407.OVC_PHR_ID,
                            OVC_PHR_DESC = tbm1407.OVC_PHR_DESC
                        };
                    foreach (var q in query)
                        if (drpOVC_CURRENT.Text == q.OVC_PHR_DESC) tbmmanage_prom_new.OVC_CURRENT = q.OVC_PHR_ID;
                    if (rdoOVC_KIND.SelectedIndex == 0)
                        tbmmanage_prom_new.OVC_KIND = "1";
                    else if (rdoOVC_KIND.SelectedIndex == 1)
                        tbmmanage_prom_new.OVC_KIND = "2";
                    tbmmanage_prom_new.OVC_MARK = txtOVC_MARK.Text;
                    tbmmanage_prom_new.OVC_WORK_NO = txtOVC_WORK_NO.Text;
                    tbmmanage_prom_new.OVC_WORK_NAME = txtOVC_WORK_NAME.Text;
                    tbmmanage_prom_new.OVC_WORK_UNIT = txtOVC_WORK_UNIT.Text;
                    tbmmanage_prom_new.OVC_RECEIVE_NO = txtOVC_RECEIVE_NO.Text;
                    tbmmanage_prom_new.OVC_DRECEIVE = txtOVC_DRECEIVE.Text;
                    //if (drpDocOVC_COMPTROLLER_NO.Text == "請選擇")
                    //    tbmmanage_prom_new.OVC_ONNAME = txtOVC_COMPTROLLER_NO.Text;
                    //else
                    //    tbmmanage_prom_new.OVC_ONNAME = drpDocOVC_COMPTROLLER_NO.Text;
                    tbmmanage_prom_new.OVC_ONNAME = txtOVC_COMPTROLLER_NO.Text;
                    tbmmanage_prom_new.OVC_COMPTROLLER_NO = txtOVC_COMPTROLLER_NO_1.Text;
                    tbmmanage_prom_new.OVC_DCOMPTROLLER = txtOVC_DCOMPTROLLER.Text;
                    tbmmanage_prom_new.OVC_BACK_NO = txtOVC_BACK_NO.Text;
                    tbmmanage_prom_new.OVC_DBACK = txtOVC_DBACK.Text;
                    tbmmanage_prom_new.OVC_BACK_REASON = txtOVC_BACK_REASON.Text;
                    tbmmanage_prom_new.OVC_BACK_MARK = txtOVC_BACK_MARK.Text;
                    tbmmanage_prom_new.OVC_PROM_SN = Guid.NewGuid();
                    if (strmeg == "")
                    {
                        mpms.TBMMANAGE_PROM.Add(tbmmanage_prom_new);
                        mpms.SaveChanges();
                        Session["sn"] = tbmmanage_prom_new.OVC_PROM_SN.ToString();
                        Session["isModify"] = "1";
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strmeg);
                }
            }
        }
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

        #region 列印
        //收入
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            ExpenseCertificate_ExportToWord();
            var purch = lblOVC_PURCH_6.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Prom_Revenue_Temp.docx");
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
            ExpenseCertificate_ExportToWord();
            var purch = lblOVC_PURCH_6.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Prom_Revenue_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/Prom_Revenue_Temp.pdf";
            string fileName = purch + "收入通知單.pdf";
            FCommon.WordToPDF(this, path_temp, wordfilepath, fileName);
        }
        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            ExpenseCertificate_ExportToWord();
            var purch = lblOVC_PURCH_6.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Prom_Revenue_Temp.docx");
            string wordfilepath = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Prom_Revenue_Temp.odt");
            string fileName = purch + "收入通知單.odt";
            FCommon.WordToOdt(this, path_temp, wordfilepath, fileName);
        }
        //退還
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            RefundNotice_ExportToWord();
            var purch = lblOVC_PURCH_6.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Prom_Return_Temp.docx");
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
            var purch = lblOVC_PURCH_6.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Prom_Return_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/Prom_Return_Temp.pdf";
            string fileName = purch + "退還通知單.pdf";
            FCommon.WordToPDF(this, path_temp, wordfilepath, fileName);
        }
        protected void LinkButton6_Click(object sender, EventArgs e)
        {
            RefundNotice_ExportToWord();
            var purch = lblOVC_PURCH_6.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Prom_Return_Temp.docx");
            string wordfilepath = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Prom_Return_Temp.odt");
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
                lblOVC_PURCH_6.Text = Session["rowtext"].ToString();
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
                    from tbmmanage_prom in mpms.TBMMANAGE_PROM
                    where tbmmanage_prom.OVC_PURCH.Equals(purch)
                    select new
                    {
                        OVC_OWN_NAME = tbmmanage_prom.OVC_OWN_NAME,
                        OVC_OWN_ADDRESS = tbmmanage_prom.OVC_OWN_ADDRESS,
                        OVC_OWN_NO = tbmmanage_prom.OVC_OWN_NO,
                        OVC_OWNED_NAME = tbmmanage_prom.OVC_OWNED_NAME,
                        OVC_OWNED_NO = tbmmanage_prom.OVC_OWNED_NO,
                        OVC_OWNED_ADDRESS = tbmmanage_prom.OVC_OWNED_ADDRESS,
                        OVC_REASON_1 = tbmmanage_prom.OVC_REASON_1,
                        OVC_NSTOCK_1 = tbmmanage_prom.OVC_NSTOCK_1,
                        OVC_NUMBER_1 = tbmmanage_prom.OVC_NUMBER_1,
                        ONB_MONEY_1 = tbmmanage_prom.ONB_MONEY_1,
                        OVC_DEFFECT_1 = tbmmanage_prom.OVC_DEFFECT_1,
                        OVC_REASON_2 = tbmmanage_prom.OVC_REASON_2,
                        OVC_NSTOCK_2 = tbmmanage_prom.OVC_NSTOCK_2,
                        OVC_NUMBER_2 = tbmmanage_prom.OVC_NUMBER_2,
                        ONB_MONEY_2 = tbmmanage_prom.ONB_MONEY_2,
                        OVC_DEFFECT_2 = tbmmanage_prom.OVC_DEFFECT_2,
                        OVC_REASON_3 = tbmmanage_prom.OVC_REASON_3,
                        OVC_NSTOCK_3 = tbmmanage_prom.OVC_NSTOCK_3,
                        OVC_NUMBER_3 = tbmmanage_prom.OVC_NUMBER_3,
                        ONB_MONEY_3 = tbmmanage_prom.ONB_MONEY_3,
                        OVC_DEFFECT_3 = tbmmanage_prom.OVC_DEFFECT_3,
                        ONB_ALL_MONEY = tbmmanage_prom.ONB_ALL_MONEY,
                        OVC_CURRENT = tbmmanage_prom.OVC_CURRENT,
                        OVC_KIND = tbmmanage_prom.OVC_KIND,
                        OVC_MARK = tbmmanage_prom.OVC_MARK,
                        OVC_WORK_NO = tbmmanage_prom.OVC_WORK_NO,
                        OVC_WORK_NAME = tbmmanage_prom.OVC_WORK_NAME,
                        OVC_WORK_UNIT = tbmmanage_prom.OVC_WORK_UNIT,
                        OVC_RECEIVE_NO = tbmmanage_prom.OVC_RECEIVE_NO,
                        OVC_DRECEIVE = tbmmanage_prom.OVC_DRECEIVE,
                        OVC_ONNAME = tbmmanage_prom.OVC_ONNAME,
                        OVC_COMPTROLLER_NO = tbmmanage_prom.OVC_COMPTROLLER_NO,
                        OVC_DCOMPTROLLER = tbmmanage_prom.OVC_DCOMPTROLLER,
                        OVC_BACK_NO = tbmmanage_prom.OVC_BACK_NO,
                        OVC_DBACK = tbmmanage_prom.OVC_DBACK,
                        OVC_BACK_REASON = tbmmanage_prom.OVC_BACK_REASON,
                        OVC_BACK_MARK = tbmmanage_prom.OVC_BACK_MARK,
                        OVC_DAPPROVE = tbmmanage_prom.OVC_DAPPROVE,
                        OVC_PROM_SN = tbmmanage_prom.OVC_PROM_SN
                    };
                foreach (var q in queryPurch)
                {
                    txtOVC_OWNED_NAME.Text = q.OVC_VEN_TITLE;
                    txtOVC_OWNED_NO.Text = q.OVC_VEN_CST;
                    txtOVC_OWNED_ADDRESS.Text = q.OVC_VEN_ADDRESS;
                    lblOVC_PUR_USER.Text = q.OVC_PUR_USER;
                    lblOVC_PUR_NSECTION.Text = "";//職級
                    lblOVC_PUR_IUSER_PHONE.Text = q.OVC_PUR_IUSER_PHONE;
                    lblOVC_PUR_IUSER_PHONE_EXT.Text = q.OVC_PUR_IUSER_PHONE_EXT;
                }
                if (Session["sn"] != null)
                {
                    foreach (var q in query)
                    {
                        if (q.OVC_PROM_SN.ToString().Equals(Session["sn"].ToString()))
                        {
                            txtOVC_OWN_NO.Text = q.OVC_OWN_NO;
                            txtOVC_OWN_NAME.Text = q.OVC_OWN_NAME;
                            txtOVC_OWN_ADDRESS.Text = q.OVC_OWN_ADDRESS;
                            txtOVC_REASON_1.Text = q.OVC_REASON_1;
                            txtOVC_NSTOCK_1.Text = q.OVC_NSTOCK_1;
                            txtOVC_NUMBER_1.Text = q.OVC_NUMBER_1;
                            txtONB_MONEY_1.Text = String.Format("{0:N}", q.ONB_MONEY_1 ?? 0);
                            txtExpDate_1.Text = q.OVC_DEFFECT_1;
                            txtOVC_REASON_2.Text = q.OVC_REASON_2;
                            txtOVC_NSTOCK_2.Text = q.OVC_NSTOCK_2;
                            txtOVC_NUMBER_2.Text = q.OVC_NUMBER_2;
                            txtONB_MONEY_2.Text = String.Format("{0:N}", q.ONB_MONEY_2 ?? 0);
                            txtExpDate_2.Text = q.OVC_DEFFECT_2;
                            txtOVC_REASON_3.Text = q.OVC_REASON_3;
                            txtOVC_NSTOCK_3.Text = q.OVC_NSTOCK_3;
                            txtOVC_NUMBER_3.Text = q.OVC_NUMBER_3;
                            txtONB_MONEY_3.Text = String.Format("{0:N}", q.ONB_MONEY_3 ?? 0);
                            txtExpDate_3.Text = q.OVC_DEFFECT_3;
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
                            //drpDocOVC_COMPTROLLER_NO.Text = q.OVC_ONNAME;
                            //if (drpDocOVC_COMPTROLLER_NO.Text == "請選擇")
                            //    txtOVC_COMPTROLLER_NO.Text = q.OVC_ONNAME;
                            txtOVC_COMPTROLLER_NO.Text = q.OVC_ONNAME;
                            txtOVC_COMPTROLLER_NO_1.Text = q.OVC_COMPTROLLER_NO;
                            txtOVC_BACK_NO.Text = q.OVC_BACK_NO;
                            txtOVC_DBACK.Text = q.OVC_DBACK;
                            txtOVC_BACK_REASON.Text = q.OVC_BACK_REASON;
                            txtOVC_BACK_MARK.Text = q.OVC_BACK_MARK;
                            txtOVC_DAPPROVE.Text = q.OVC_DAPPROVE;
                            txtOVC_DCOMPTROLLER.Text = q.OVC_DCOMPTROLLER;
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
        private void list_dataImport(ListControl list)
        {
            list.Items.Clear();
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
                list.Items.Add(qu.OVC_PHR_DESC);
                var queryCurrency =
                    from prom in mpms.TBMMANAGE_PROM
                    where prom.OVC_PURCH.Equals(lblOVC_PURCH_6.Text.Substring(0, 7))
                    select new
                    {
                        prom.OVC_CURRENT,
                    };
                foreach (var q in queryCurrency)
                {
                    if (q.OVC_CURRENT == qu.OVC_PHR_ID)
                        list.Text = qu.OVC_PHR_DESC;
                    else if (q.OVC_CURRENT == null)
                        list.Text = "新臺幣";
                }
            }
        }
        #endregion

        #region 收入通知單預覽列印
        void ExpenseCertificate_ExportToWord()
        {
            decimal money = 0;
            string Money = "";
            string path = "";
            var purch = lblOVC_PURCH_6.Text.Substring(0, 7);
            path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/ReceivePurchaseServletE1E_1.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    if (Session["sn"] != null)
                    {
                        Guid guid = Guid.Parse(Session["sn"].ToString());
                        var queryProm =
                            from prom in mpms.TBMMANAGE_PROM
                            where prom.OVC_PURCH.Equals(purch)
                            where prom.OVC_PROM_SN.Equals(guid)
                            select new
                            {
                                OVC_OWN_NAME = prom.OVC_OWN_NAME,
                                OVC_OWNED_NAME = prom.OVC_OWNED_NAME,
                                OVC_OWNED_NO = prom.OVC_OWNED_NO,
                                OVC_OWN_NO = prom.OVC_OWN_NO,
                                OVC_OWN_ADDRESS = prom.OVC_OWN_ADDRESS,
                                OVC_OWN_TEL = prom.OVC_OWN_TEL,
                                OVC_REASON_1 = prom.OVC_REASON_1,
                                OVC_NSTOCK_1 = prom.OVC_NSTOCK_1,
                                OVC_NUMBER_1 = prom.OVC_NUMBER_1,
                                ONB_MONEY_1 = prom.ONB_MONEY_1,
                                OVC_REASON_2 = prom.OVC_REASON_2,
                                OVC_NSTOCK_2 = prom.OVC_NSTOCK_2,
                                OVC_NUMBER_2 = prom.OVC_NUMBER_2,
                                ONB_MONEY_2 = prom.ONB_MONEY_2,
                                OVC_REASON_3 = prom.OVC_REASON_3,
                                OVC_NSTOCK_3 = prom.OVC_NSTOCK_3,
                                OVC_NUMBER_3 = prom.OVC_NUMBER_3,
                                ONB_MONEY_3 = prom.ONB_MONEY_3,
                                ONB_ALL_MONEY = prom.ONB_ALL_MONEY,
                                OVC_MARK = prom.OVC_MARK,
                                OVC_WORK_NO = prom.OVC_WORK_NO,
                                OVC_WORK_NAME = prom.OVC_WORK_NAME,
                                OVC_WORK_UNIT = prom.OVC_WORK_UNIT,
                                OVC_RECEIVE_NO = prom.OVC_RECEIVE_NO,
                                OVC_DRECEIVE = prom.OVC_DRECEIVE,
                                OVC_CURRENT = prom.OVC_CURRENT
                            };
                        if (queryProm.Count() < 1)
                            return;
                        foreach (var q in queryProm)
                        {
                            doc.ReplaceText("[$OWN_NAME$]", q.OVC_OWN_NAME != null ? q.OVC_OWN_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OWNED_NAME$]", q.OVC_OWNED_NAME != null ? q.OVC_OWNED_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OWNED_NO$]", q.OVC_OWNED_NO != null ? q.OVC_OWNED_NO : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OWN_ADDRESS$]", q.OVC_OWN_ADDRESS != null ? q.OVC_OWN_ADDRESS : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OWNED_ADDRESS$]", q.OVC_OWN_ADDRESS != null ? q.OVC_OWN_ADDRESS : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$REASON_1$]", q.OVC_REASON_1 != null ? q.OVC_REASON_1 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$NSTOCK_1$]", q.OVC_NSTOCK_1 != null ? q.OVC_NSTOCK_1 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$NUMBER_1$]", q.OVC_NUMBER_1 != null ? q.OVC_NUMBER_1 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY_1$]", q.ONB_MONEY_1 != null ? String.Format("{0:N}", q.ONB_MONEY_1) : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$REASON_2$]", q.OVC_REASON_2 != null ? q.OVC_REASON_2 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$NSTOCK_2$]", q.OVC_NSTOCK_2 != null ? q.OVC_NSTOCK_2 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$NUMBER_2$]", q.OVC_NUMBER_2 != null ? q.OVC_NUMBER_2 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY_2$]", q.ONB_MONEY_2 != null ? String.Format("{0:N}", q.ONB_MONEY_2) : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$REASON_3$]", q.OVC_REASON_3 != null ? q.OVC_REASON_3 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$NSTOCK_3$]", q.OVC_NSTOCK_3 != null ? q.OVC_NSTOCK_3 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$NUMBER_3$]", q.OVC_NUMBER_3 != null ? q.OVC_NUMBER_3 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY_3$]", q.ONB_MONEY_3 != null ? String.Format("{0:N}", q.ONB_MONEY_3) : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                            var query1407 =
                                from tbm1407 in mpms.TBM1407
                                where tbm1407.OVC_PHR_CATE.Equals("B0")
                                where tbm1407.OVC_PHR_ID.Equals(q.OVC_CURRENT)
                                select new
                                {
                                    OVC_PHR_ID = tbm1407.OVC_PHR_ID,
                                    OVC_PHR_DESC = tbm1407.OVC_PHR_DESC
                                };
                            foreach (var qu in query1407)
                                doc.ReplaceText("[$CURRENT$]", q.OVC_CURRENT, false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$CURRENT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ALL_MONEY != null)
                            {
                                money = decimal.Parse(q.ONB_ALL_MONEY.ToString());
                                Money = EastAsiaNumericFormatter.FormatWithCulture("Lc", money, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                                doc.ReplaceText("[$ALL_MONEY$]", Money, false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            doc.ReplaceText("[$ALL_MONEY$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OVC_MARK$]", q.OVC_MARK != null ? q.OVC_MARK : "", false, System.Text.RegularExpressions.RegexOptions.None);
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

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/Prom_Revenue_Temp.docx");
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
            var purch = lblOVC_PURCH_6.Text.Substring(0, 7);
            path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/ReceivePurchaseServletE1E_2.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    if (Session["sn"] != null)
                    {
                        Guid guid = Guid.Parse(Session["sn"].ToString());
                        var queryProm =
                        from prom in mpms.TBMMANAGE_PROM
                        where prom.OVC_PURCH.Equals(purch)
                        where prom.OVC_PROM_SN.Equals(guid)
                        select new
                        {
                            OVC_WORK_NAME = prom.OVC_WORK_NAME,
                            OVC_BACK_NO = prom.OVC_BACK_NO,
                            OVC_WORK_NO = prom.OVC_WORK_NO,
                            OVC_DRECEIVE = prom.OVC_DRECEIVE,
                            OVC_RECEIVE_NO = prom.OVC_RECEIVE_NO,
                            OVC_OWN_NAME = prom.OVC_OWN_NAME,
                            OVC_OWN_NO = prom.OVC_OWN_NO,
                            OVC_OWNED_NAME = prom.OVC_OWNED_NAME,
                            OVC_OWNED_NO = prom.OVC_OWNED_NO,
                            OVC_REASON_1 = prom.OVC_REASON_1,
                            OVC_REASON_2 = prom.OVC_REASON_2,
                            OVC_REASON_3 = prom.OVC_REASON_3,
                            OVC_DCOMPTROLLER = prom.OVC_DCOMPTROLLER,
                            OVC_NSTOCK_1 = prom.OVC_NSTOCK_1,
                            OVC_NSTOCK_2 = prom.OVC_NSTOCK_2,
                            OVC_NSTOCK_3 = prom.OVC_NSTOCK_3,
                            OVC_NUMBER_1 = prom.OVC_NUMBER_1,
                            OVC_NUMBER_2 = prom.OVC_NUMBER_2,
                            OVC_NUMBER_3 = prom.OVC_NUMBER_3,
                            ONB_MONEY_1 = prom.ONB_MONEY_1,
                            ONB_MONEY_2 = prom.ONB_MONEY_2,
                            ONB_MONEY_3 = prom.ONB_MONEY_3,
                            ONB_ALL_MONEY = prom.ONB_ALL_MONEY,
                            OVC_BACK_REASON = prom.OVC_BACK_REASON,
                            OVC_BACK_MARK = prom.OVC_BACK_MARK
                        };
                        if (queryProm.Count() < 1)
                            return;
                        foreach (var q in queryProm)
                        {
                            doc.ReplaceText("[$OWN_NO$]", q.OVC_OWN_NO != null ? q.OVC_OWN_NO : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OWN_NAME$]", q.OVC_OWN_NAME != null ? q.OVC_OWN_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OWNED_NAME$]", q.OVC_OWNED_NAME != null ? q.OVC_OWNED_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$OWNED_NO$]", q.OVC_OWNED_NO != null ? q.OVC_OWNED_NO : "", false, System.Text.RegularExpressions.RegexOptions.None);
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
                            doc.ReplaceText("[$NSTOCK_1$]", q.OVC_NSTOCK_1 != null ? q.OVC_NSTOCK_1 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$NSTOCK_2$]", q.OVC_NSTOCK_2 != null ? q.OVC_NSTOCK_2 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$NSTOCK_3$]", q.OVC_NSTOCK_3 != null ? q.OVC_NSTOCK_3 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY_1$]", q.ONB_MONEY_1 != null ? String.Format("{0:N}", q.ONB_MONEY_1) : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY_2$]", q.ONB_MONEY_2 != null ? String.Format("{0:N}", q.ONB_MONEY_2) : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$MONEY_3$]", q.ONB_MONEY_3 != null ? String.Format("{0:N}", q.ONB_MONEY_3) : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$NUMBER_1$]", q.OVC_NUMBER_1 != null ? q.OVC_NUMBER_1 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$NUMBER_2$]", q.OVC_NUMBER_2 != null ? q.OVC_NUMBER_2 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$NUMBER_3$]", q.OVC_NUMBER_3 != null ? q.OVC_NUMBER_3 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.ONB_ALL_MONEY != null)
                            {
                                money = decimal.Parse(q.ONB_ALL_MONEY.ToString());
                                Money = EastAsiaNumericFormatter.FormatWithCulture("Lc", money, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                                doc.ReplaceText("[$ALL_MONEY$]", Money, false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            doc.ReplaceText("[$ALL_MONEY$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$BACK_REASON$]", q.OVC_BACK_REASON != null ? q.OVC_BACK_REASON.ToString() : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("[$BACK_MARK$]", q.OVC_BACK_MARK != null ? q.OVC_BACK_MARK.ToString() : "", false, System.Text.RegularExpressions.RegexOptions.None);
                            
                            doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                            doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                            doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/Prom_Return_Temp.docx");
                        }
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
                      from prom in mpms.TBMMANAGE_PROM
                      where prom.OVC_PURCH.Equals(purch)
                      select prom.OVC_ONNAME;
                //foreach (var e in query)
                //    drpDocOVC_COMPTROLLER_NO.Items.Add(e);
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

        protected void txtONB_MONEY_TextChanged(object sender, EventArgs e)
        {
            decimal decNT1 = 0, decNT2 = 0, decNT3 = 0;
            bool boolNT1 = string.Empty.Equals(txtONB_MONEY_1.Text) ? true : decimal.TryParse(txtONB_MONEY_1.Text, out decNT1);
            bool boolNT2 = string.Empty.Equals(txtONB_MONEY_2.Text) ? true : decimal.TryParse(txtONB_MONEY_2.Text, out decNT2);
            bool boolNT3 = string.Empty.Equals(txtONB_MONEY_3.Text) ? true : decimal.TryParse(txtONB_MONEY_3.Text, out decNT3);
            if (boolNT1 && boolNT2 && boolNT3)
            {
                txtONB_ALL_MONEY.Text = (decNT1 + decNT2 + decNT3).ToString();
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", " 入帳金額 須為數字！");
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