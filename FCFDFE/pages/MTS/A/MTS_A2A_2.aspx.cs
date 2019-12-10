using System;
using System.Linq;
using System.Web.UI;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System.IO;

namespace FCFDFE.pages.MTS.A
{
    public partial class MTS_A2A_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        Common FCommon = new Common();
        string id;
        bool isUpload = false;

        #region 副程式
        private void dataImport()
        {
            TBGMT_ETR ETR = MTSE.TBGMT_ETR.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
            if (ETR != null)
            {
                txtODT_REQUIRE_DATE.Text = FCommon.getDateTime(ETR.ODT_REQUIRE_DATE);
                txtOVC_REQUIRE_MSG_NO.Text = ETR.OVC_REQUIRE_MSG_NO;
                txtODT_RECEIVE_DATE.Text = FCommon.getDateTime(ETR.ODT_RECEIVE_DATE);
                txtOVC_RECEIVE_MSG_NO.Text = ETR.OVC_RECEIVE_MSG_NO;
                txtODT_PROCESS_DATE.Text = FCommon.getDateTime(ETR.ODT_PROCESS_DATE);
                txtOVC_PROCESS_MSG_NO.Text = ETR.OVC_PROCESS_MSG_NO;
                txtODT_STRATEGY_PROCESS_DATE.Text = FCommon.getDateTime(ETR.ODT_STRATEGY_PROCESS_DATE);
                txtODT_TEL_DATE.Text = FCommon.getDateTime(ETR.ODT_TEL_DATE);
                txtODT_PASS_DATE.Text = FCommon.getDateTime(ETR.ODT_PASS_DATE);
                txtODT_SHIP_START_DATE.Text = FCommon.getDateTime(ETR.ODT_SHIP_START_DATE);
                txtODT_RETURN_DATE.Text = FCommon.getDateTime(ETR.ODT_RETURN_DATE);
                txtOVC_DELAY_DESCRIPTION.Text = ETR.OVC_DELAY_DESCRIPTION;
            }

            TBGMT_EDF EDF = MTSE.TBGMT_EDF.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
            if (EDF != null)
            {
                lblODT_VALIDITY_DATE.Text = FCommon.getDateTime(EDF.ODT_VALIDITY_DATE);
                txtODT_VALIDITY_DATE.Text = FCommon.getDateTime(EDF.ODT_VALIDITY_DATE);
            }

            TBGMT_ESO ESO = MTSE.TBGMT_ESO.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
            if (ESO != null)
                lblODT_STORED_DATE.Text = FCommon.getDateTime(ESO.ODT_STORED_DATE);

            TBGMT_ICR ICR = MTSE.TBGMT_ICR.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
            if (ICR != null)
            {
                lblODT_CUSTOM_DATE.Text = FCommon.getDateTime(ICR.ODT_CUSTOM_DATE);
                txtODT_CUSTOM_DATE.Text = FCommon.getDateTime(ICR.ODT_CUSTOM_DATE);
            }

            TBGMT_ECL ECL = MTSE.TBGMT_ECL.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
            if (ECL != null)
            {
                lblODT_CUSTOM_DATE.Text = FCommon.getDateTime(ECL.ODT_EXP_DATE);
                txtODT_CUSTOM_DATE.Text = FCommon.getDateTime(ECL.ODT_EXP_DATE);
            }
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
            if (FCommon.getAuth(this, out isUpload))
            {
                FCommon.getQueryString(this, "id", out id, true);
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    lblOVC_BLD_NO.Text = id;
                    FCommon.Controls_Attributes("readonly", "true", txtODT_REQUIRE_DATE, txtODT_RECEIVE_DATE, txtODT_PROCESS_DATE, txtODT_STRATEGY_PROCESS_DATE,
                        txtODT_TEL_DATE, txtODT_PASS_DATE, txtODT_SHIP_START_DATE, txtODT_RETURN_DATE, txtODT_VALIDITY_DATE, txtODT_CUSTOM_DATE);
                    dataImport();
                }
            }
        }

        protected void btnModify_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strODT_REQUIRE_DATE = txtODT_REQUIRE_DATE.Text;
            string strOVC_REQUIRE_MSG_NO = txtOVC_REQUIRE_MSG_NO.Text;
            string strODT_RECEIVE_DATE = txtODT_RECEIVE_DATE.Text;
            string strOVC_RECEIVE_MSG_NO = txtOVC_RECEIVE_MSG_NO.Text;
            string strODT_PROCESS_DATE = txtODT_PROCESS_DATE.Text;
            string strOVC_PROCESS_MSG_NO = txtOVC_PROCESS_MSG_NO.Text;
            string strOVC_BLD_NO = lblOVC_BLD_NO.Text;
            string strODT_STRATEGY_PROCESS_DATE = txtODT_STRATEGY_PROCESS_DATE.Text;
            string strODT_VALIDITY_DATE = txtODT_VALIDITY_DATE.Text;
            string strODT_TEL_DATE = txtODT_TEL_DATE.Text;
            string strODT_STORED_DATE = lblODT_STORED_DATE.Text;
            string strODT_PASS_DATE = txtODT_PASS_DATE.Text;
            string strODT_SHIP_START_DATE = txtODT_SHIP_START_DATE.Text;
            string strODT_RETURN_DATE = txtODT_RETURN_DATE.Text;
            string strOVC_DELAY_DESCRIPTION = txtOVC_DELAY_DESCRIPTION.Text;
            string strODT_CUSTOM_DATE = txtODT_CUSTOM_DATE.Text;

            DateTime dateODT_REQUIRE_DATE, dateODT_RECEIVE_DATE, dateODT_PROCESS_DATE, dateODT_STRATEGY_PROCESS_DATE,
                 dateODT_TEL_DATE, dateODT_PASS_DATE, dateODT_SHIP_START_DATE, dateODT_RETURN_DATE, dateNow = DateTime.Now;
            DateTime dateODT_VALIDITY_DATE, dateODT_CUSTOM_DATE;
            FCommon.checkDateTime(strODT_VALIDITY_DATE, "有效期限", ref strMessage, out dateODT_VALIDITY_DATE);
            FCommon.checkDateTime(strODT_CUSTOM_DATE, "報關日期", ref strMessage, out dateODT_CUSTOM_DATE);
            #region 錯誤訊息
            if (strODT_REQUIRE_DATE.Equals(string.Empty))
                strMessage += "<P> 請填入 委運出口單位函文申請時間 </p>";
            if (strOVC_REQUIRE_MSG_NO.Equals(string.Empty))
                strMessage += "<P> 請填入 委運出口單位文號 </p>";
            if (strODT_RECEIVE_DATE.Equals(string.Empty))
                strMessage += "<P> 請填入 接轉組收文時間 </p>";
            if (strOVC_RECEIVE_MSG_NO.Equals(string.Empty))
                strMessage += "<P> 請填入 接轉組收文文號 </p>";
            if (strODT_PROCESS_DATE.Equals(string.Empty))
                strMessage += "<P> 請填入 中心函文關稅局辦理免稅時間 </p>";
            if (strOVC_PROCESS_MSG_NO.Equals(string.Empty))
                strMessage += "<P> 請填入 中心函文文號 </p>";
            if (strODT_STRATEGY_PROCESS_DATE.Equals(string.Empty))
                strMessage += "<P> 請填入 「戰略性高科技貨口輸入許可證」收辦時間 </p>";
            if (strODT_TEL_DATE.Equals(string.Empty))
                strMessage += "<P> 請填入 電請委運單位配合辦理進倉時間 </p>";
            if (strODT_PASS_DATE.Equals(string.Empty))
                strMessage += "<P> 請填入 通關時間 </p>";
            if (strODT_SHIP_START_DATE.Equals(string.Empty))
                strMessage += "<P> 請填入 航輪(貨機)啟航時間 </p>";
            if (strODT_RETURN_DATE.Equals(string.Empty))
                strMessage += "<P> 請填入 出口案正本提單函復時間 </p>";

            //確認輸入型態
            bool boolODT_REQUIRE_DATE = FCommon.checkDateTime(strODT_REQUIRE_DATE, "委運出口單位函文申請時間", ref strMessage, out dateODT_REQUIRE_DATE);
            bool boolODT_RECEIVE_DATE = FCommon.checkDateTime(strODT_RECEIVE_DATE, "接轉組收文時間", ref strMessage, out dateODT_RECEIVE_DATE);
            bool boolODT_PROCESS_DATE = FCommon.checkDateTime(strODT_PROCESS_DATE, "中心函文關稅局辦理免稅時間", ref strMessage, out dateODT_PROCESS_DATE);
            bool boolODT_STRATEGY_PROCESS_DATE = FCommon.checkDateTime(strODT_STRATEGY_PROCESS_DATE, "「戰略性高科技貨口輸入許可證」收辦時間", ref strMessage, out dateODT_STRATEGY_PROCESS_DATE);
            bool boolODT_TEL_DATE = FCommon.checkDateTime(strODT_TEL_DATE, "電請委運單位配合辦理進倉時間", ref strMessage, out dateODT_TEL_DATE);
            bool boolODT_PASS_DATE = FCommon.checkDateTime(strODT_PASS_DATE, "通關時間", ref strMessage, out dateODT_PASS_DATE);
            bool boolODT_SHIP_START_DATE = FCommon.checkDateTime(strODT_SHIP_START_DATE, "航輪(貨機)啟航時間", ref strMessage, out dateODT_SHIP_START_DATE);
            bool boolODT_RETURN_DATE = FCommon.checkDateTime(strODT_RETURN_DATE, "出口案正本提單函復時間", ref strMessage, out dateODT_RETURN_DATE);
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                try
                {
                    TBGMT_EDF EDF = MTSE.TBGMT_EDF.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
                    if (EDF != null && !strODT_VALIDITY_DATE.Equals(string.Empty))
                    {
                        EDF.ODT_VALIDITY_DATE = dateODT_VALIDITY_DATE;
                        MTSE.SaveChanges();
                        FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), EDF.GetType().Name, this, "修改");
                    }
                    TBGMT_ICR ICR = MTSE.TBGMT_ICR.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
                    if (ICR != null && !strODT_CUSTOM_DATE.Equals(string.Empty))
                    {
                        ICR.ODT_CUSTOM_DATE = dateODT_CUSTOM_DATE;
                        MTSE.SaveChanges();
                        FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), ICR.GetType().Name.ToString(), this, "修改");
                    }
                    TBGMT_ECL ECL = MTSE.TBGMT_ECL.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
                    if (ECL != null)
                    {
                        ECL.ODT_EXP_DATE = dateODT_CUSTOM_DATE;
                        MTSE.SaveChanges();
                        FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), ECL.GetType().Name.ToString(), this, "修改");
                    }
                    TBGMT_ETR etr = MTSE.TBGMT_ETR.Where(table => table.OVC_BLD_NO.Equals(id)).FirstOrDefault();
                    if (etr != null)
                    {
                        etr.ODT_REQUIRE_DATE = dateODT_REQUIRE_DATE;
                        etr.OVC_REQUIRE_MSG_NO = strOVC_REQUIRE_MSG_NO;
                        etr.ODT_RECEIVE_DATE = dateODT_RECEIVE_DATE;
                        etr.OVC_RECEIVE_MSG_NO = strOVC_RECEIVE_MSG_NO;
                        etr.ODT_PROCESS_DATE = dateODT_PROCESS_DATE;
                        etr.OVC_PROCESS_MSG_NO = strOVC_PROCESS_MSG_NO;
                        etr.ODT_STRATEGY_PROCESS_DATE = dateODT_STRATEGY_PROCESS_DATE;
                        etr.ODT_TEL_DATE = dateODT_TEL_DATE;
                        etr.ODT_PASS_DATE = dateODT_PASS_DATE;
                        etr.ODT_SHIP_START_DATE = dateODT_SHIP_START_DATE;
                        etr.ODT_RETURN_DATE = dateODT_RETURN_DATE;
                        etr.OVC_DELAY_DESCRIPTION = strOVC_DELAY_DESCRIPTION;
                        etr.ODT_MODIFY_DATE = dateNow;
                    }

                    MTSE.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), etr.GetType().Name, this, "修改");
                    FCommon.AlertShow(PnWarning, "success", "系統訊息", $"提單編號：{ id } 之時程管制表 修改成功");
                }
                catch
                {
                    FCommon.AlertShow(PnWarning, "danger", "系統訊息", "修改失敗，請聯絡工程師。");
                }
            }
            else
                FCommon.AlertShow(PnWarning, "danger", "系統訊息", strMessage);
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_A2A_1{ getQueryString() }");
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (isUpload)
            {
                if (FileUpload.HasFile)
                {
                    string serverPath = Path.Combine(Server.MapPath("~/Uploadfile/MTS/A"));
                    string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MTS/A/" + id));
                    string fileName = FileUpload.FileName;
                    switch (fileName.Split('.')[1])
                    {
                        case "doc":
                            fileName = id + ".doc";
                            break;
                        case "docx":
                            fileName = id + ".docx";
                            break;
                        case "pdf":
                            fileName = id + ".pdf";
                            break;
                        default:
                            fileName = "";
                            break;
                    }
                    if (fileName != "")
                    {
                        string serverFilePath = Path.Combine(serverDir, fileName);
                        if (!Directory.Exists(serverPath))
                            Directory.CreateDirectory(serverPath);   //新增A29資料夾
                        if (!Directory.Exists(serverDir))
                            Directory.CreateDirectory(serverDir);   //新增購案編號資料夾
                        else
                        {
                            string[] files = Directory.GetFiles(serverDir);
                            foreach (string file in files)
                            {
                                File.Delete(file);
                            }
                        }
                        try
                        {
                            FileUpload.SaveAs(serverFilePath);
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "檔案上傳成功");
                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message;
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", error);
                        }
                    }
                    else
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請上傳word或pdf檔案！");
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先 選擇檔案");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無上傳權限");
        }
    }
}