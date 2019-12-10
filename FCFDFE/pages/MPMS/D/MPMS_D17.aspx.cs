using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using TemplateEngine.Docx;
using System.Windows;
using System.Web.UI;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D17 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strOVC_PURCH, strOVC_PURCH_5;
        bool isUpload = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this, out isUpload))
            {
                if (FCommon.getQueryString(this, "OVC_PURCH", out strOVC_PURCH, false))
                {
                    TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                    if (IsOVC_DO_NAME() && !IsPostBack)
                    {
                        DataImport();
                        GvFilesImport();
                        GvFilesImport_2();
                    } 
                }
                else
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
            }
        }


        protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            short numONB_TIMES;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            ViewState["OVC_DOPEN"] = ((Label)gv.Rows[gvrIndex].FindControl("lblOVC_DOPEN")).Text;
            ViewState["ONB_TIMES"] = ((Label)gv.Rows[gvrIndex].FindControl("lblONB_TIMES")).Text;
            short.TryParse(ViewState["ONB_TIMES"].ToString(), out numONB_TIMES);

            #region 疑義電傳
            if (e.CommandName == "WordD17_1")
            {
                string filepath = OutputWordD17_1();
                string TempName = strOVC_PURCH + "-疑義電傳.docx";
                FileInfo file = new FileInfo(filepath);
                string wordPath = filepath;
                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + TempName);
                Response.ContentType = "application/octet-stream";
                //Response.BinaryWrite(buffer);
                Response.WriteFile(wordPath);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.Flush();
                File.Delete(filepath);
                File.Delete(wordPath);
                Response.End();
            }
            else if (e.CommandName == "WordD17_1_pdf")
            {
                string filepath = OutputWordD17_1();
                string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/WordD17_1_pdf.pdf";
                string FileName = strOVC_PURCH + "-疑義電傳.pdf";
                FCommon.WordToPDF(this, filepath, filetemp, FileName);
            }
            else if (e.CommandName == "WordD17_1_odt")
            {
                string filepath = OutputWordD17_1();
                string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/WordD17_1_odt.odt";
                string FileName = strOVC_PURCH + "-疑義電傳.odt";
                FCommon.WordToOdt(this, filepath, filetemp, FileName);
            }
            #endregion
            #region 異議電傳
            else if (e.CommandName == "WordD17_2")
            {
                string filepath = OutputWordD17_2();
                string TempName = strOVC_PURCH + "-異議電傳.docx";
                FileInfo file = new FileInfo(filepath);
                string wordPath = filepath;
                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + TempName);
                Response.ContentType = "application/octet-stream";
                //Response.BinaryWrite(buffer);
                Response.WriteFile(wordPath);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.Flush();
                File.Delete(filepath);
                File.Delete(wordPath);
                Response.End();
            }
            else if (e.CommandName == "WordD17_2_pdf")
            {
                string filepath = OutputWordD17_2();
                string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/WordD17_2_pdf.pdf";
                string FileName = strOVC_PURCH + "-異議電傳.pdf";
                FCommon.WordToPDF(this, filepath, filetemp, FileName);
            }
            else if (e.CommandName == "WordD17_2_odt")
            {
                string filepath = OutputWordD17_2();
                string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/WordD17_2_odt.odt";
                string FileName = strOVC_PURCH + "-異議電傳.odt";
                FCommon.WordToOdt(this, filepath, filetemp, FileName);
            }
            #endregion
            #region 傳真通知書
            else if (e.CommandName == "WordD17_3")
            {
                string filepath = OutputWordD17_3();
                string TempName = strOVC_PURCH + "-傳真通知書.docx";
                FileInfo file = new FileInfo(filepath);
                string wordPath = filepath;
                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + TempName);
                Response.ContentType = "application/octet-stream";
                //Response.BinaryWrite(buffer);
                Response.WriteFile(wordPath);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.Flush();
                File.Delete(filepath);
                //File.Delete(wordPath);
                Response.End();
            }
            else if (e.CommandName == "WordD17_3_pdf")
            {
                string filepath = OutputWordD17_3();
                string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/WordD17_3_pdf.pdf";
                string FileName = strOVC_PURCH + "-傳真通知書.pdf";
                FCommon.WordToPDF(this, filepath, filetemp, FileName);
            }
            else if (e.CommandName == "WordD17_3_odt")
            {
                string filepath = OutputWordD17_3();
                string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/WordD17_3_odt.odt";
                string FileName = strOVC_PURCH + "-傳真通知書.odt";
                FCommon.WordToOdt(this, filepath, filetemp, FileName);
            }
            #endregion
        }


        protected void gvFiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteFile")
            {
                //按下 刪除
                GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
                int gvrIndex = gvr.RowIndex;
                string fileName = ((Label)gvFiles.Rows[gvrIndex].FindControl("lblFileName")).Text;
                string filePath = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D17/Doubt/" + strOVC_PURCH + "/" + fileName));
                if (File.Exists(filePath))
                    File.Delete(filePath);
                GvFilesImport();
            }
        }
        protected void gvFiles_2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteFile")
            {
                //按下 刪除
                GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
                int gvrIndex = gvr.RowIndex;
                string fileName = ((Label)gvFiles_2.Rows[gvrIndex].FindControl("lblFileName")).Text;
                string filePath = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D17/Objection/" + strOVC_PURCH + "/" + fileName));
                if (File.Exists(filePath))
                    File.Delete(filePath);
                GvFilesImport_2();
            }
        }

        #region Button OnClick

        protected void btnBack_Click(object sender, EventArgs e)
        {
            //點擊 回上一頁
            string send_url = "~/pages/MPMS/D/MPMS_D14_1.aspx?OVC_PURCH=" + strOVC_PURCH;
            Response.Redirect(send_url);
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //按下上傳 => 檔案上傳
            if (isUpload)
            {
                if (FileUpload.HasFile)
                {
                    string serverPath = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D17/Doubt"));
                    string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D17/Doubt/" + strOVC_PURCH));
                    string fileName = FileUpload.FileName;
                    string serverFilePath = Path.Combine(serverDir, fileName);
                    if (!Directory.Exists(serverPath))
                        Directory.CreateDirectory(serverPath);   //新增D17資料夾
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

        protected void btnUpload_2_Click(object sender, EventArgs e)
        {
            //按下上傳 => 檔案上傳
            if (isUpload)
            {
                if (FileUpload_2.HasFile)
                {
                    string serverPath = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D17/Objection"));
                    string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D17/Objection/" + strOVC_PURCH));
                    string fileName = FileUpload_2.FileName;
                    string serverFilePath = Path.Combine(serverDir, fileName);
                    if (!Directory.Exists(serverPath))
                        Directory.CreateDirectory(serverPath);   //新增D17資料夾
                    if (!Directory.Exists(serverDir))
                        Directory.CreateDirectory(serverDir);   //新增購案編號資料夾
                    try
                    {
                        FileUpload_2.SaveAs(serverFilePath);
                        GvFilesImport_2();
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
                lblOVC_PURCH_A_5.Text = "購案編號：" + strOVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5;

                var queryANNOUNCE =
                    from tbRECEIVE_ANNOUNCE in mpms.TBMRECEIVE_ANNOUNCE
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
                    rows["OVC_DSEND"] = GetTaiwanDate(rows["OVC_DSEND"].ToString());
                    rows["OVC_BUY_RANK"] = GetTbm1407Desc("MA", rows["OVC_BUY_RANK"].ToString());
                    rows["OVC_PUR_ASS_VEN_CODE"] = GetTbm1407Desc("C7", tbm1301.OVC_PUR_ASS_VEN_CODE);

                    TBM1303 tbm1303 = new TBM1303();
                    tbm1303 = mpms.TBM1303.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                    if (tbm1303 != null)
                        rows["OVC_RESULT"] = GetTbm1407Desc("A8", tbm1303.OVC_RESULT);
                }
                FCommon.GridView_dataImport(gv, dt);
                FCommon.GridView_dataImport(gv2, dt);
                FCommon.GridView_dataImport(gv3, dt);
            }
        }

        //疑義
        public void GvFilesImport()
        {
            //資料夾檔案內容寫入GV
            DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D17/Doubt/" + strOVC_PURCH)));
            lblTotalFile.Text = dirInfo.Exists == false ? "0" : dirInfo.GetFiles().Length.ToString();
            //string strFileDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D17/" + strOVC_PURCH));
            //DirectoryInfo dirInfo = new DirectoryInfo(strFileDir);
            //lblTotalFile.Text = dirInfo.GetFiles().Length.ToString();

            System.Data.DataTable dtFile = new System.Data.DataTable();
            dtFile.Columns.Add("FileName", typeof(System.String));
            dtFile.Columns.Add("LinkFileName", typeof(System.String));
            dtFile.Columns.Add("Time", typeof(System.String));
            dtFile.Columns.Add("FileSize", typeof(System.Int32));
            string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D17/Doubt/" + strOVC_PURCH));
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
                    string strFilePath = String.Format("<a href='/Uploadfile/MPMS/D/D17/Doubt/" + strOVC_PURCH + "/" + file.Name + "' target='_blank'>" + file.Name + "</a>");
                    dtFile.Rows.Add(file.Name, strFilePath, GetTaiwanDate(file.CreationTime.ToString()) + " " + file.CreationTime.Hour + ":" + file.CreationTime.Minute
                                    , file.Length / 1024);  //file.Length/1024 = KB
                }
                gvFiles.DataSource = dtFile;
                gvFiles.DataBind();
            }

            if (dtFile == null || dtFile.Rows.Count <= 0)
            {
                string[] fileField = { "FileName", "LinkFileName", "Time", "FileSize" };
                FCommon.GridView_setEmpty(gvFiles, fileField);
            }
        }

        //異議
        public void GvFilesImport_2()
        {
            //資料夾檔案內容寫入GV
            DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D17/Objection/" + strOVC_PURCH)));
            lblTotalFile_2.Text = dirInfo.Exists == false ? "0" : dirInfo.GetFiles().Length.ToString();
            //string strFileDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D17/" + strOVC_PURCH));
            //DirectoryInfo dirInfo = new DirectoryInfo(strFileDir);
            //lblTotalFile.Text = dirInfo.GetFiles().Length.ToString();

            System.Data.DataTable dtFile = new System.Data.DataTable();
            dtFile.Columns.Add("FileName", typeof(System.String));
            dtFile.Columns.Add("LinkFileName", typeof(System.String));
            dtFile.Columns.Add("Time", typeof(System.String));
            dtFile.Columns.Add("FileSize", typeof(System.Int32));
            string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D17/Objection/" + strOVC_PURCH));
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
                    string strFilePath = String.Format("<a href='/Uploadfile/MPMS/D/D17/Objection/" + strOVC_PURCH + "/" + file.Name + "' target='_blank'>" + file.Name + "</a>");
                    dtFile.Rows.Add(file.Name, strFilePath, GetTaiwanDate(file.CreationTime.ToString()) + " " + file.CreationTime.Hour + ":" + file.CreationTime.Minute
                                    , file.Length / 1024);  //file.Length/1024 = KB
                }
                gvFiles_2.DataSource = dtFile;
                gvFiles_2.DataBind();
            }

            if (dtFile == null || dtFile.Rows.Count <= 0)
            {
                string[] fileField = { "FileName", "LinkFileName", "Time", "FileSize" };
                FCommon.GridView_setEmpty(gvFiles_2, fileField);
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

        #region 輸出報表
        public string OutputWordD17_1()
        {
            string strOVC_DOPEN = ViewState["OVC_DOPEN"] == null ? "" : ViewState["OVC_DOPEN"].ToString();
            string strOVC_DOPEN_tw = strOVC_DOPEN == "" ? "" : GetTaiwanDate(strOVC_DOPEN);
            string strONB_TIMES = ViewState["ONB_TIMES"] == null ? "" : ViewState["ONB_TIMES"].ToString();
            short.TryParse(strONB_TIMES, out short numONB_TIMES);
            string outPutfilePath = "";
            string fileName = "D17_1-疑義電傳.docx";
            string TempName = "";
            string filePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + fileName));

            TempName = strOVC_PURCH + "-疑義電傳.docx";
            outPutfilePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + TempName));
            File.Delete(outPutfilePath);
            File.Copy(filePath, outPutfilePath);
            var valuesToFill = new TemplateEngine.Docx.Content();

            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IPURCH", tbm1301.OVC_PUR_IPURCH == null ? "" : tbm1301.OVC_PUR_IPURCH));
                valuesToFill.Fields.Add(new FieldContent("OVC_DOPEN", strOVC_DOPEN_tw));
                valuesToFill.Fields.Add(new FieldContent("ONB_TIMES", strONB_TIMES));
                if (page1_Select1.SelectedValue != "" && page1_Select1.SelectedValue != null)
                    valuesToFill.Fields.Add(new FieldContent("page1_Select1", page1_Select1.SelectedValue + "，"));
                if (page1_Select2.Checked == true)
                    valuesToFill.Fields.Add(new FieldContent("page1_Select2", page1_Select2.Text));
                if (page1_Select3.Checked == true)
                    valuesToFill.Fields.Add(new FieldContent("page1_Select3", page1_Select3.Text));


                TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
                tbmRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                        && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_ANNOUNCE != null)
                    valuesToFill.Fields.Add(new FieldContent("OVC_OPEN_HOUR", tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR == null ? "" : tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR));

                TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (tbmRECEIVE_BID != null)
                    valuesToFill.Fields.Add(new FieldContent("OVC_DO_NAME", tbmRECEIVE_BID.OVC_DO_NAME == null? "" : tbmRECEIVE_BID.OVC_DO_NAME));
            }

            using (TemplateProcessor outputDocument = new TemplateProcessor(outPutfilePath).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }
            string filepath = outPutfilePath;
            return filepath;
        }

        #endregion

        public string OutputWordD17_2()
        {
            string strOVC_DOPEN = ViewState["OVC_DOPEN"] == null ? "" : ViewState["OVC_DOPEN"].ToString();
            string strOVC_DOPEN_tw = strOVC_DOPEN == "" ? "" : GetTaiwanDate(strOVC_DOPEN);
            string strONB_TIMES = ViewState["ONB_TIMES"] == null ? "" : ViewState["ONB_TIMES"].ToString();
            short.TryParse(strONB_TIMES, out short numONB_TIMES);
            string outPutfilePath = "";
            string fileName = "D17_2-異議電傳.docx";
            string TempName = "";
            string filePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + fileName));

            TempName = strOVC_PURCH + "-異議電傳.docx";
            outPutfilePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + TempName));
            File.Delete(outPutfilePath);
            File.Copy(filePath, outPutfilePath);
            var valuesToFill = new TemplateEngine.Docx.Content();

            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IPURCH", tbm1301.OVC_PUR_IPURCH == null ? "" : tbm1301.OVC_PUR_IPURCH));
                valuesToFill.Fields.Add(new FieldContent("OVC_DOPEN", strOVC_DOPEN_tw));
                valuesToFill.Fields.Add(new FieldContent("ONB_TIMES", strONB_TIMES));
                if (page2_Select1.SelectedValue != "" && page1_Select1.SelectedValue != null)
                    valuesToFill.Fields.Add(new FieldContent("page2_Select1", page2_Select1.SelectedValue + "，"));
                if (page2_Select2.Checked == true)
                    valuesToFill.Fields.Add(new FieldContent("page2_Select2", page2_Select2.Text));
                if (page2_Select3.Checked == true)
                    valuesToFill.Fields.Add(new FieldContent("page2_Select3", page2_Select3.Text));



                TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
                tbmRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                        && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_ANNOUNCE != null)
                    valuesToFill.Fields.Add(new FieldContent("OVC_OPEN_HOUR", tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR == null ? "" : tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR));

                TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (tbmRECEIVE_BID != null)
                    valuesToFill.Fields.Add(new FieldContent("OVC_DO_NAME", tbmRECEIVE_BID.OVC_DO_NAME == null ? "" : tbmRECEIVE_BID.OVC_DO_NAME));

            }

            using (TemplateProcessor outputDocument = new TemplateProcessor(outPutfilePath).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }

            string filepath = outPutfilePath;
            return filepath;
        }

        public string OutputWordD17_3()
        {
            string strOVC_DOPEN = ViewState["OVC_DOPEN"] == null ? "" : ViewState["OVC_DOPEN"].ToString();
            string strOVC_DOPEN_tw = strOVC_DOPEN == "" ? "" : GetTaiwanDate(strOVC_DOPEN);
            string strONB_TIMES = ViewState["ONB_TIMES"] == null ? "" : ViewState["ONB_TIMES"].ToString();
            short.TryParse(strONB_TIMES, out short numONB_TIMES);

            string outPutfilePath = "";
            string fileName = "D17_3-傳真通知書.docx";
            string TempName = "";
            string filePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + fileName));
            TempName = strOVC_PURCH + "-傳真通知書.docx";
            outPutfilePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + TempName));
            File.Delete(outPutfilePath);
            File.Copy(filePath, outPutfilePath);
            var valuesToFill = new TemplateEngine.Docx.Content();

            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IPURCH", tbm1301.OVC_PUR_IPURCH == null ? "" : tbm1301.OVC_PUR_IPURCH));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_NSECTION", tbm1301.OVC_PUR_NSECTION == null ? "" : tbm1301.OVC_PUR_NSECTION));

                TBM1301_PLAN tbm1301Plan = gm.TBM1301_PLAN.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (tbm1301Plan != null)
                {
                    valuesToFill.Fields.Add(new FieldContent("OVC_PUR_USER", tbm1301Plan.OVC_PUR_USER == null ? "" : tbm1301Plan.OVC_PUR_USER));
                    valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IUSER_FAX", tbm1301Plan.OVC_PUR_IUSER_FAX == null ? "" : tbm1301Plan.OVC_PUR_IUSER_FAX));
                }

            }

            using (TemplateProcessor outputDocument = new TemplateProcessor(outPutfilePath).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }

            string filepath = outPutfilePath;
            return filepath;
        }
    }
}