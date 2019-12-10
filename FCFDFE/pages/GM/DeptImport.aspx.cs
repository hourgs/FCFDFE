using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using System;
using System.Data;
using System.Web.UI;
using System.IO;
using System.Linq;

namespace FCFDFE.pages.GM
{
    public partial class DeptImport : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities GME = new GMEntities();
        bool isUpload;

        #region 副程式
        private void XlsImport()
        {
            //檔案上傳
            if (FileUploadxls.HasFile)
            {
                string strFileName = FileUploadxls.FileName;
                string strFileExtension = Path.GetExtension(strFileName);

                if (strFileExtension.Equals(".xlsx") || strFileExtension.Equals(".xls"))
                {
                    DataTable dt = ExcelNPOI.RenderStreamToDataTable(FileUploadxls.FileContent, strFileName, 1); //讀取Excel
                    string strSuccess = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        string strOVC_DEPT_CDE = dr[0].ToString();
                        if(DataSave(dr,strOVC_DEPT_CDE,out string strMessage))
                        {
                            strSuccess += strSuccess.Equals(string.Empty) ? "" : ", ";
                            strSuccess += strOVC_DEPT_CDE;
                        }
                        else
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", $"<p> 單位代碼：{ strOVC_DEPT_CDE } 之單位檔資料 新增失敗！ </p>"+ strMessage);
                    }
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", $"單位代碼：{ strSuccess } 之單位檔資料 新增成功。");
                    //FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增單位檔資料成功!!");
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請確認 選擇的檔案類型！");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請選擇 檔案！");
        }
        private bool DataSave(DataRow dr, string strOVC_DEPT_CDE, out string strMessage)
        {
            strMessage = "";
            bool isSuccess = false;
            try
            {
                string strOVC_ONNAME = dr[1].ToString();
                string strOVC_DEPT_FAX = dr[2].ToString();
                string strOVC_DEPT_MAILADDR = dr[3].ToString();
                string strOVC_DEPT_DOC_NAME = ""; //發文字號
                string strOVC_MANAGER = dr[4].ToString();
                string strOVC_PURCHASE_DEPT = dr[5].ToString();
                string strOVC_MANAGER_TITLE = dr[6].ToString();
                string strOVC_EMAILADDRESS = dr[7].ToString();
                string strOVC_DEPT_SEC_NAME = dr[8].ToString();
                string strOVC_TOP_DEPT = dr[9].ToString();
                string strOVC_TOP_DEPT2 = dr[10].ToString();
                string strOVC_CLASS = dr[11].ToString();
                string strOVC_CLASS2 = dr[12].ToString();
                string strOVC_PURCHASE_OK = dr[13].ToString();
                string strOVC_DEPT_PHONE = dr[14].ToString();
                string strOVC_ENABLE = dr[15].ToString();

                TBMDEPT dept = GME.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(strOVC_DEPT_CDE)).FirstOrDefault();
                #region 錯誤訊息
                if (string.IsNullOrEmpty(strOVC_DEPT_CDE))
                    strMessage += "<p> 請輸入 單位代碼！ </p>";
                else if (dept != null)
                    strMessage += "<p> 單位代碼 已存在！ </p>";
                if (string.IsNullOrEmpty(strOVC_ONNAME))
                    strMessage += "<p> 請輸入 單位名稱！ </p>";
                #endregion
                if (string.IsNullOrEmpty(strMessage))
                {
                    dept = new TBMDEPT();
                    dept.OVC_DEPT_CDE = strOVC_DEPT_CDE;
                    dept.OVC_ONNAME = strOVC_ONNAME;
                    dept.OVC_DEPT_FAX = strOVC_DEPT_FAX;
                    dept.OVC_DEPT_MAILADDR = strOVC_DEPT_MAILADDR;
                    dept.OVC_DEPT_DOC_NAME = strOVC_DEPT_DOC_NAME;
                    dept.OVC_MANAGER = strOVC_MANAGER;
                    dept.OVC_PURCHASE_DEPT = strOVC_PURCHASE_DEPT;
                    dept.OVC_MANAGER_TITLE = strOVC_MANAGER_TITLE;
                    dept.OVC_EMAILADDRESS = strOVC_EMAILADDRESS;
                    dept.OVC_DEPT_SEC_NAME = strOVC_DEPT_SEC_NAME;
                    dept.OVC_TOP_DEPT = strOVC_TOP_DEPT;
                    dept.OVC_CLASS = strOVC_CLASS;
                    dept.OVC_PURCHASE_OK = strOVC_PURCHASE_OK;
                    dept.OVC_DEPT_PHONE = strOVC_DEPT_PHONE;
                    dept.OVC_ENABLE = strOVC_ENABLE;
                    dept.OVC_CLASS2 = strOVC_CLASS2;
                    dept.OVC_TOP_DEPT2 = strOVC_TOP_DEPT2;
                    GME.TBMDEPTs.Add(dept);
                    GME.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), dept.GetType().Name.ToString(), this, "新增");
                    isSuccess = true;
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
            catch
            {
                strMessage = "單位 新增失敗，請聯絡工程師！";
            }
            return isSuccess;
        }
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_DEPT_CDE", Request.QueryString["OVC_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_ONNAME", Request.QueryString["OVC_ONNAME"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_ENABLE", Request.QueryString["OVC_ENABLE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_CLASS", Request.QueryString["OVC_CLASS"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_PURCHASE_DEPT", Request.QueryString["OVC_PURCHASE_DEPT"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_CLASS2", Request.QueryString["OVC_CLASS2"], false);
            return strQueryString;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this, isUpload))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {

                }
            }
        }
        
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"DeptQuery{ getQueryString() }");
        }
        protected void btnLoadunit_Click(object sender, EventArgs e)
        {
            if (isUpload)
                XlsImport();
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無上傳權限！");
        }
    }
}