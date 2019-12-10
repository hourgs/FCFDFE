using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;
using System.Globalization;
using System.IO;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D16 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strOVC_PURCH, strOVC_PURCH_5;
        bool hasRows = false, isUpload;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this, out isUpload))
            {
                if (FCommon.getQueryString(this, "OVC_PURCH", out strOVC_PURCH, false))
                {
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_DAPPROVE);
                    TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PURCH_5 = tbmRECEIVE_BID == null? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                    if (IsOVC_DO_NAME() && !IsPostBack)
                        DataImport();
                    GvFilesImport();

                }
                else
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
            }
        }


        protected void gvANNOUNCE_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string send_url, strOVC_DOPEN, strONB_TIMES;
            short numONB_TIMES;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            strOVC_DOPEN = ((Label)gvANNOUNCE.Rows[gvrIndex].FindControl("lblOVC_DOPEN")).Text;
            strONB_TIMES = ((Label)gvANNOUNCE.Rows[gvrIndex].FindControl("lblONB_TIMES")).Text;
            short.TryParse(strONB_TIMES, out numONB_TIMES);

            if (e.CommandName == "Change")
            {
                send_url = "~/pages/MPMS/D/MPMS_D16_1.aspx?OVC_PURCH=" + strOVC_PURCH
                            + "&OVC_DOPEN=" + strOVC_DOPEN + "&ONB_TIMES=" + strONB_TIMES;
                Response.Redirect(send_url);
            }

            else if (e.CommandName == "DeleteRow")
            {
                TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE
                {
                    OVC_PURCH = strOVC_PURCH,
                    OVC_PURCH_5 = strOVC_PURCH_5,
                    OVC_DOPEN = strOVC_DOPEN,
                    ONB_TIMES = numONB_TIMES,
                };
                mpms.Entry(tbmRECEIVE_ANNOUNCE).State = EntityState.Deleted;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmRECEIVE_ANNOUNCE.GetType().Name.ToString(), this, "刪除");

                TBMRECEIVE_WORK tbmRECEIVE_WORK = new TBMRECEIVE_WORK();
                tbmRECEIVE_WORK = mpms.TBMRECEIVE_WORK.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                        && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_WORK != null)
                {
                    mpms.Entry(tbmRECEIVE_WORK).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmRECEIVE_WORK.GetType().Name.ToString(), this, "刪除");

                }
                
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功！");
                DataImport();
            }
            else if (e.CommandName == "ToD16_2")
            {
                send_url = "~/pages/MPMS/D/MPMS_D16_2.aspx?OVC_PURCH=" + strOVC_PURCH 
                            + "&OVC_DOPEN=" + strOVC_DOPEN + "&ONB_TIMES=" + strONB_TIMES;
                Response.Redirect(send_url);
            }
        }


        #region Button OnClick

        protected void btnBack_Click(object sender, EventArgs e)
        {
            //點擊 回上一頁
            string send_url = "~/pages/MPMS/D/MPMS_D14_1.aspx?OVC_PURCH=" + strOVC_PURCH;
            Response.Redirect(send_url);
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            //點擊 新增公告資料
            string send_url = "~/pages/MPMS/D/MPMS_D16_1.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_PURCH_5=" + strOVC_PURCH_5;
            Response.Redirect(send_url);
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            //政賢看這邊
            if (txtOVC_DAPPROVE.Text != "")
            {
                //修改 OVC_STATUS="23" 開標通知的 階段結束日=主官核批日
                TBMSTATUS tbmSTATUS_23 = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                            && tb.ONB_TIMES == 1 && tb.OVC_STATUS.Equals("23")).FirstOrDefault();
                if (tbmSTATUS_23 != null)
                {
                    tbmSTATUS_23.OVC_DEND = txtOVC_DAPPROVE.Text;
                    mpms.SaveChanges();
                }

                //新增 OVC_STATUS="25" 開標紀錄的 階段開始日
                string strOVC_DO_NAME = "";
                TBMSTATUS tbmSTATUS_25 = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                            && tb.ONB_TIMES == 1 && tb.OVC_STATUS.Equals("25")).FirstOrDefault();
                if (tbmSTATUS_25 == null)
                {
                    //新增 TBMSTATUS (購案階段紀錄檔)
                    TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    if (tbmRECEIVE_BID != null)
                        strOVC_DO_NAME = tbmRECEIVE_BID.OVC_DO_NAME;

                    TBMSTATUS tbmSTATUS_New = new TBMSTATUS
                    {
                        OVC_STATUS_SN = Guid.NewGuid(),
                        OVC_PURCH = strOVC_PURCH,
                        OVC_PURCH_5 = strOVC_PURCH_5,
                        ONB_TIMES = 1,
                        OVC_DO_NAME = strOVC_DO_NAME,
                        OVC_STATUS = "25",
                        OVC_DBEGIN = DateTime.Now.ToString("yyyy-MM-dd"),
                    };
                    mpms.TBMSTATUS.Add(tbmSTATUS_New);
                    mpms.SaveChanges();
                }
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
            }
        }

        #endregion


        #region 副程式
        private bool IsOVC_DO_NAME()
        {
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
            DataTable dt;
            string strOVC_DOPEN;
            short numONB_TIMES;
            
            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                var queryANNOUNCE =
                from tbRECEIVE_ANNOUNCE in mpms.TBMRECEIVE_ANNOUNCE
                join modify in mpms.TBMANNOUNCE_MODIFY on new { tbRECEIVE_ANNOUNCE.OVC_PURCH, tbRECEIVE_ANNOUNCE.OVC_PURCH_5, tbRECEIVE_ANNOUNCE.OVC_DOPEN, tbRECEIVE_ANNOUNCE.OVC_OPEN_HOUR, tbRECEIVE_ANNOUNCE.OVC_OPEN_MIN } equals new { modify.OVC_PURCH, modify.OVC_PURCH_5, modify.OVC_DOPEN, modify.OVC_OPEN_HOUR, modify.OVC_OPEN_MIN } into mod
                from modify in mod.DefaultIfEmpty()
                where tbRECEIVE_ANNOUNCE.OVC_PURCH.Equals(strOVC_PURCH) && tbRECEIVE_ANNOUNCE.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                orderby tbRECEIVE_ANNOUNCE.OVC_DOPEN, tbRECEIVE_ANNOUNCE.ONB_TIMES
                select new
                {

                    OVC_PURCH_A_5 = tbRECEIVE_ANNOUNCE.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + tbRECEIVE_ANNOUNCE.OVC_PURCH_5,
                    OVC_PURCH = tbRECEIVE_ANNOUNCE.OVC_PURCH,
                    OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                    OVC_PURCH_5 = tbRECEIVE_ANNOUNCE.OVC_PURCH_5,
                    OVC_DANNOUNCE = tbRECEIVE_ANNOUNCE.OVC_DANNOUNCE,
                    OVC_DOPEN_H_M = "",
                    OVC_DOPEN = tbRECEIVE_ANNOUNCE.OVC_DOPEN,
                    OVC_OPEN_HOUR = tbRECEIVE_ANNOUNCE.OVC_OPEN_HOUR,
                    OVC_OPEN_MIN = tbRECEIVE_ANNOUNCE.OVC_OPEN_MIN,
                    OVC_DOPEN_H_M_MOD = "",
                    OVC_DOPEN_N = !string.IsNullOrEmpty(modify.OVC_DOPEN_N) ? modify.OVC_DOPEN_N : "",
                    OVC_OPEN_HOUR_N = !string.IsNullOrEmpty(modify.OVC_OPEN_HOUR_N) ? modify.OVC_OPEN_HOUR_N : "",
                    OVC_OPEN_MIN_N = !string.IsNullOrEmpty(modify.OVC_OPEN_MIN_N) ? modify.OVC_OPEN_MIN_N : "",
                    OVC_DSEND = tbRECEIVE_ANNOUNCE.OVC_DSEND,
                    OVC_BUY_RANK = tbRECEIVE_ANNOUNCE.OVC_BUY_RANK,
                    OVC_PUR_ASS_VEN_CODE = "",
                    ONB_TIMES = tbRECEIVE_ANNOUNCE.ONB_TIMES,
                    //開標結果(代碼A8)
                    OVC_RESULT = "",
                };

                dt = CommonStatic.LinqQueryToDataTable(queryANNOUNCE);
                foreach (DataRow rows in dt.Rows)
                {
                    strOVC_DOPEN = rows["OVC_DOPEN"].ToString();
                    numONB_TIMES = short.Parse(rows["ONB_TIMES"].ToString());
                    rows["OVC_DANNOUNCE"] = GetTaiwanDate(rows["OVC_DANNOUNCE"].ToString());
                    rows["OVC_DOPEN_H_M"] = GetTaiwanDate(strOVC_DOPEN) + " " + rows["OVC_OPEN_HOUR"].ToString()
                                                    + "時 " + rows["OVC_OPEN_MIN"].ToString() + "分";
                    
                    strOVC_DOPEN = rows["OVC_DOPEN_N"].ToString();
                    if (strOVC_DOPEN != "")
                        rows["OVC_DOPEN_H_M_MOD"] = GetTaiwanDate(strOVC_DOPEN) + " " + rows["OVC_OPEN_HOUR_N"].ToString()
                                                    + "時 " + rows["OVC_OPEN_MIN_N"].ToString() + "分";

                    rows["OVC_DSEND"] = GetTaiwanDate(rows["OVC_DSEND"].ToString());
                    rows["OVC_BUY_RANK"] = GetTbm1407Desc("MA", rows["OVC_BUY_RANK"].ToString());
                    rows["OVC_PUR_ASS_VEN_CODE"] = GetTbm1407Desc("C7", tbm1301.OVC_PUR_ASS_VEN_CODE);

                    TBM1303 tbm1303 = new TBM1303();
                    tbm1303 = mpms.TBM1303.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                    if (tbm1303 != null)
                        rows["OVC_RESULT"] = GetTbm1407Desc("A8", tbm1303.OVC_RESULT);
                }
                FCommon.GridView_dataImport(gvANNOUNCE, dt);
            }

            var tbmstatus = mpms.TBMSTATUS
                .Where(o => o.OVC_PURCH.Equals(strOVC_PURCH) && o.OVC_PURCH_5.Equals(strOVC_PURCH_5))
                .Where(o => o.OVC_STATUS.Equals("23")).FirstOrDefault();
            if (tbmstatus != null)
                txtOVC_DAPPROVE.Text = tbmstatus.OVC_DEND != null ? tbmstatus.OVC_DEND : "";
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
            return strDate;
        }

        

        private String GetTbm1407Desc(string cateID, string codeID)
        {
            //將TBM1407片語ID代碼轉為Desc
            TBM1407 tbm1407 = new TBM1407();
            if (codeID != null && codeID != "")
            {
                tbm1407 = gm.TBM1407.Where(table => table.OVC_PHR_CATE.Equals(cateID) && table.OVC_PHR_ID.Equals(codeID)).FirstOrDefault();
                if (tbm1407 != null)
                {
                    return tbm1407.OVC_PHR_DESC.ToString();
                }
            }
            return codeID;
        }


        #endregion

        #region 採購發包階段上傳檔案
        public void GvFilesImport()
        {
            //資料夾檔案內容寫入GV
            System.Data.DataTable dtFile = new System.Data.DataTable();
            dtFile.Columns.Add("FileName", typeof(System.String));
            //dtFile.Columns.Add("Time", typeof(System.DateTime));
            dtFile.Columns.Add("Time", typeof(System.String));
            dtFile.Columns.Add("FileSize", typeof(System.Int32));
            string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D12/" + strOVC_PURCH));
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
                    string strFilePath = String.Format("<a href='/Uploadfile/MPMS/D/D12/" + strOVC_PURCH + "/" + file.Name + "' target='_blank'>" + file.Name + "</a>");
                    dtFile.Rows.Add(strFilePath, GetTaiwanDate(file.CreationTime.ToString()) + " " + file.CreationTime.Hour + ":" + file.CreationTime.Minute
                                    , file.Length / 1024);  //file.Length/1024 = KB
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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //按下上傳 => 檔案上傳
            if (isUpload)
            {
                if (FileUpload.HasFile)
                {
                    string serverPath = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D12"));
                    string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D12/" + strOVC_PURCH));
                    string fileName = FileUpload.FileName;
                    string serverFilePath = Path.Combine(serverDir, fileName);
                    //string strOVC_ATTACH_NAME = ViewState["OVC_ATTACH_NAME_UPLOAD"].ToString();
                    if (!Directory.Exists(serverPath))
                        Directory.CreateDirectory(serverPath);   //新增D12資料夾
                    if (!Directory.Exists(serverDir))
                        Directory.CreateDirectory(serverDir);   //新增購案編號資料夾
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
        #endregion
    }
}