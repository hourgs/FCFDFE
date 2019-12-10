using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;
using System.IO;
using System.Globalization;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D14_2 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strOVC_PURCH, strOVC_PURCH_5;
        bool isUpload=true;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))//, out isUpload))
            {
                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_PURCH_5"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    strOVC_PURCH = Request.QueryString["OVC_PURCH"].ToString();
                    strOVC_PURCH_5 = Request.QueryString["OVC_PURCH_5"].ToString();
                    if (IsOVC_DO_NAME() && !IsPostBack)
                        DataImport();
                }
            }
        }

       


        #region Button OnClick

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //按下 存檔按鈕
            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                TBMRECEIVE_BID_LOG tbRECEIVE_BID_LOG = new TBMRECEIVE_BID_LOG();
                var isExist = mpms.TBMRECEIVE_BID_LOG.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).ToList();
                if (isExist.Count == 0)
                {
                    //新增 TBMRECEIVE_BID_LOG
                    tbRECEIVE_BID_LOG.OVC_PURCH = strOVC_PURCH;
                    tbRECEIVE_BID_LOG.OVC_PURCH_5 = strOVC_PURCH_5;
                    //最後異動日
                    tbRECEIVE_BID_LOG.OVC_DMODIFIED = DateTime.Now.ToString("yyyy-MM-dd");
                    //重要事項
                    tbRECEIVE_BID_LOG.OVC_COMM = txtOVC_COMM.Text;
                    //檢討與因應措施
                    tbRECEIVE_BID_LOG.OVC_COMM_REASON = txtOVC_COMM_REASON.Text;
                    mpms.TBMRECEIVE_BID_LOG.Add(tbRECEIVE_BID_LOG);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbRECEIVE_BID_LOG.GetType().Name.ToString(), this, "新增");
                }
                else
                {
                    //修改 TBMRECEIVE_BID_LOG
                    tbRECEIVE_BID_LOG = mpms.TBMRECEIVE_BID_LOG.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    tbRECEIVE_BID_LOG.OVC_PURCH = strOVC_PURCH;
                    tbRECEIVE_BID_LOG.OVC_PURCH_5 = strOVC_PURCH_5;
                    //最後異動日
                    tbRECEIVE_BID_LOG.OVC_DMODIFIED = DateTime.Now.ToString("yyyy-MM-dd");
                    //重要事項
                    tbRECEIVE_BID_LOG.OVC_COMM = txtOVC_COMM.Text;
                    //檢討與因應措施
                    tbRECEIVE_BID_LOG.OVC_COMM_REASON = txtOVC_COMM_REASON.Text;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbRECEIVE_BID_LOG.GetType().Name.ToString(), this, "修改");
                }
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功！");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無此購案編號");
        }


        protected void btnBack_Click(object sender, EventArgs e)
        {
            //點擊Button 回上一頁
            string strDateYear = "1" + strOVC_PURCH.Substring(2, 2);
            string send_url = "~/pages/MPMS/D/MPMS_D14.aspx?DateYear=" + strDateYear;
            Response.Redirect(send_url);
        }


        protected void btnQueryOVC_CONTRACT_UNIT_Click(object sender, EventArgs e)
        {
            //點擊Button 單位查詢 履約驗結單位
            Session["unitquery"] = "query4";
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //按下上傳 => 檔案上傳
            if (isUpload)
            {
                if (FileUpload.HasFile)
                {
                    string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/" + strOVC_PURCH));
                    string fileName = FileUpload.FileName;
                    string serverFilePath = Path.Combine(serverDir, fileName);
                    //string strOVC_ATTACH_NAME = ViewState["OVC_ATTACH_NAME_UPLOAD"].ToString();
                    if (!Directory.Exists(serverDir))
                        Directory.CreateDirectory(serverDir);   //新增資料夾
                    try
                    {
                        FileUpload.SaveAs(serverFilePath);
                        GvFilesImport();
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "檔案上傳成功");
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", error);
                    }
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先 選擇檔案");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無上傳權限");
        }


        protected void lbtnPrint_Click(object sender, EventArgs e)
        {
            //按下 列印
            string send_url = "MPMS_D11_6.aspx?OVC_PURCH=" + strOVC_PURCH;
            Response.Write("<script>window.open('" + send_url + "','_blank');</script>");
        }

        #endregion





        #region 副程式
        private bool IsOVC_DO_NAME()
        {
            //判斷是否為購案承辦人
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strUserName, strDept = "";
            DataTable dt = new DataTable();
            if (strUSER_ID.Length > 0)
            {
                ACCOUNT ac = new ACCOUNT();
                ac = gm.ACCOUNTs.Where(tb => tb.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                if (ac != null)
                {
                    strUserName = ac.USER_NAME.ToString();
                    strDept = ac.DEPT_SN;
                    TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    if (strOVC_PURCH == "")
                        strErrorMsg = "請選擇購案";
                    else if (tbm1301 == null)
                        strErrorMsg = "查無此購案編號";
                    else
                    {
                        TBM1301_PLAN plan1301 =
                            gm.TBM1301_PLAN.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCHASE_UNIT.Equals(strDept)).FirstOrDefault();

                        TBMRECEIVE_BID tbmRECEIVE_BID =
                            mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_DO_NAME.Equals(strUserName)).FirstOrDefault();
                        if (tbmRECEIVE_BID == null || plan1301 == null)
                            strErrorMsg = "非此購案的承辦人";
                    }

                    if (strErrorMsg != "")
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErrorMsg);
                    else
                    {
                        divForm.Visible = true;
                        return true;
                    }
                }
            }
            divForm.Visible = false;
            return false;
        }



        private void DataImport()
        {
            //將資料庫資料帶出至畫面
            var query = gm.TBM1407;
            TBM1301 tbm1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                //案號
                lblOVC_PURCH_A_5.Text = tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                //購案名稱(中文)
                lblOVC_PUR_IPURCH.Text = tbm1301.OVC_PUR_IPURCH;
                //計畫申購單位
                lblOVC_PUR_NSECTION.Text = tbm1301.OVC_PUR_NSECTION;
                //計畫性質(1->計劃2->非計劃)
                lblOVC_PLAN_PURCH.Text = GetTbm1407Desc("TD", tbm1301.OVC_PLAN_PURCH?.ToString());
                //招標方式(代碼C7
                lblOVC_PUR_ASS_VEN_CODE.Text = GetTbm1407Desc("C7", tbm1301.OVC_PUR_ASS_VEN_CODE?.ToString());
                //採購屬性(財物勞務代碼GN)
                lblOVC_LAB.Text = GetTbm1407Desc("GN", tbm1301.OVC_LAB?.ToString());
                //預算金額
                if (tbm1301.OVC_PURCH_KIND == "1")
                    lblONB_PUR_BUDGET.Text = "新台幣" + string.Format("{0:N}", tbm1301.ONB_PUR_BUDGET) + "元整";
                else if (tbm1301.OVC_PURCH_KIND == "2")
                    lblONB_PUR_BUDGET.Text = GetTbm1407Desc("B0", tbm1301.OVC_PUR_CURRENT) + string.Format("{0:N}", tbm1301.ONB_PUR_BUDGET);
                lblONB_PUR_BUDGET.Text = string.Format("{0:N}", tbm1301.ONB_PUR_BUDGET);
                //核定日期 OVC_PUR_DAPPROVE
                lblOVC_PUR_DAPPROVE.Text = tbm1301.OVC_PUR_DAPPROVE == null ? "" : GetTaiwanDate(tbm1301.OVC_PUR_DAPPROVE);


                TBM1301_PLAN plan1301 = new TBM1301_PLAN();
                plan1301 = gm.TBM1301_PLAN.Where(table => table.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                //核定權責(代碼E8)
                lblOVC_PUR_APPROVE_DEP.Text = GetTbm1407Desc("E8", plan1301.OVC_PUR_APPROVE_DEP?.ToString());

                //履約驗結單位名稱
                string strOVC_CONTRACT_UNIT = "";
                if (plan1301.OVC_CONTRACT_UNIT != null)
                {
                    string strOVC_CONTRACT_UNIT_ID = plan1301.OVC_CONTRACT_UNIT;
                    TBMDEPT tbmDEPT = gm.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(strOVC_CONTRACT_UNIT_ID)).FirstOrDefault();
                    if(tbmDEPT != null)
                        strOVC_CONTRACT_UNIT = tbmDEPT.OVC_ONNAME;
                }
                lblOVC_CONTRACT_UNIT.Text = strOVC_CONTRACT_UNIT;
                //交貨天數
                lblONB_DELIVER_DAYS.Text = plan1301.ONB_DELIVER_DAYS.ToString();

                TBM1231 tbm1231 = mpms.TBM1231.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (tbm1231 != null)
                {
                    //預算年度
                    lblOVC_ISOURCE.Text = tbm1231.OVC_ISOURCE;
                }

                TBMSTATUS　tbSTATUS = mpms.TBMSTATUS.Where(table => table.OVC_PURCH.Equals(strOVC_PURCH) && table.OVC_STATUS.Equals("21")).FirstOrDefault();
                if (tbSTATUS != null)
                {
                    //收辦日期
                    lblOVC_DRECEIVE.Text = GetTaiwanDate(tbSTATUS.OVC_DBEGIN);
                }

                TBMRECEIVE_BID_LOG tbECEIVE_BID_LOG = new TBMRECEIVE_BID_LOG();
                tbECEIVE_BID_LOG = mpms.TBMRECEIVE_BID_LOG.Where(table => table.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (tbECEIVE_BID_LOG != null)
                {
                    //重要事項
                    txtOVC_COMM.Text = tbECEIVE_BID_LOG.OVC_COMM;
                    //檢討與因應措施
                    txtOVC_COMM_REASON.Text = tbECEIVE_BID_LOG.OVC_COMM_REASON;
                }
                GvFilesImport();
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無此購案編號");
        }


        public void GvFilesImport()
        {
            //資料夾檔案內容寫入GV
            DataTable dtFile = new DataTable();
            dtFile.Columns.Add("FileName", typeof(System.String));
            dtFile.Columns.Add("Time", typeof(System.DateTime));
            dtFile.Columns.Add("FileSize", typeof(System.Int32));
            string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/" + strOVC_PURCH));
            if (!Directory.Exists(serverDir))
            {
                Directory.CreateDirectory(serverDir);  //新增資料夾
            }
            DirectoryInfo filePath = new DirectoryInfo(serverDir);
            FileInfo[] fileList = filePath.GetFiles();  //擷取目錄下所有檔案內容，並存到 FileInfo Array
            if (fileList.Length != 0)
            {
                foreach (FileInfo file in fileList)
                {
                    string strFilePath = String.Format("<a href='/Uploadfile/MPMS/D/" + strOVC_PURCH + "/" + file.Name + "' target='_blank'>" + file.Name + "</a>");
                    dtFile.Rows.Add(strFilePath, file.CreationTime, file.Length / 1024);  //file.Length/1024 = KB
                }
                gvFiles.DataSource = dtFile;
                gvFiles.DataBind();
            }

            if (dtFile == null || dtFile.Rows.Count <= 0)
            {
                string[] fileField = { "FileName", "Time", "FileSize" };
                FCommon.GridView_setEmpty(gvFiles, fileField);
            }
        }


        private String GetTbm1407Desc(string cateID, string codeID)
        {
            //將TBM1407片語ID代碼轉為Desc
            TBM1407 tbm1407 = new TBM1407();
            if (codeID != null && codeID != "")
            {
                tbm1407 = gm.TBM1407.Where(tb => tb.OVC_PHR_CATE.Equals(cateID) && tb.OVC_PHR_ID.Equals(codeID)).FirstOrDefault();
                if (tbm1407 != null)
                {
                    return tbm1407.OVC_PHR_DESC.ToString();
                }
            }
            return "";
        }


        private String DeptChange(string cateID, string codeID)
        {
            //將DEPT ID單位代碼轉為單位名稱
            TBM1301_PLAN plan1301 = new TBM1301_PLAN();
            TBMDEPT tbmDept = new TBMDEPT();
            string strOVC_CONTRACT_UNIT;
            if (plan1301.OVC_CONTRACT_UNIT != null)
            {
                strOVC_CONTRACT_UNIT = plan1301.OVC_CONTRACT_UNIT;
                tbmDept = gm.TBMDEPTs.Where(table => table.OVC_DEPT_CDE.Equals(strOVC_CONTRACT_UNIT)).FirstOrDefault();
                return tbmDept.OVC_ONNAME;
            }
            else
            {
                return "";
            }
        }


        public string GetTaiwanDate(string strDate)
        {
            //西元年轉民國年
            if (strDate != null && strDate != "")
            {
                DateTime datetime = Convert.ToDateTime(strDate);
                CultureInfo info = new CultureInfo("zh-TW");
                TaiwanCalendar twC = new TaiwanCalendar();
                info.DateTimeFormat.Calendar = twC;
                return datetime.ToString("yyy年MM月dd日", info);
            }
            else
                return string.Empty;
        }


        #endregion

    }
}