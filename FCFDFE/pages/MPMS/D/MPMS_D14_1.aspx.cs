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
using TemplateEngine.Docx;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D14_1 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strUserName = "", strOVC_PURCH, strOVC_PUR_AGENCY, strOVC_PURCH_5;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            {
                if (FCommon.getQueryString(this, "OVC_PURCH", out strOVC_PURCH, false))
                {
                    TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PUR_AGENCY = tbm1301 == null? "" : tbm1301.OVC_PUR_AGENCY;
                    TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PURCH_5 = tbmRECEIVE_BID == null? "" : tbmRECEIVE_BID.OVC_PURCH_5;

                    if (IsOVC_DO_NAME() && !IsPostBack)
                        DataImport();
                }
                else
                {
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                }
            }
        }


        protected void gvSTATUS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string strOVC_DO_NAME = gvSTATUS.Rows[gvrIndex].Cells[5].Text;
            string strOVC_STATUS = ((Label)gvSTATUS.Rows[gvrIndex].FindControl("lblOVC_STATUS")).Text;
            string strONB_TIMES = ((Label)gvSTATUS.Rows[gvrIndex].FindControl("lblONB_TIMES")).Text;

            string send_url;
            if (e.CommandName == "Change")
            {
                switch (strOVC_STATUS)
                {
                    case "21":        //核定 = 收辦(採購)
                        send_url = "~/pages/MPMS/D/MPMS_D15.aspx?OVC_PURCH=" + strOVC_PURCH + "&ONB_TIMES=" + strONB_TIMES;
                        Response.Redirect(send_url);
                        break;

                    case "23":        //招標 = 開標通知
                        send_url = "~/pages/MPMS/D/MPMS_D16.aspx?OVC_PURCH=" + strOVC_PURCH;
                        Response.Redirect(send_url);
                        break;

                    case "24":        //疑義、異議
                        send_url = "~/pages/MPMS/D/MPMS_D17.aspx?OVC_PURCH=" + strOVC_PURCH;
                        Response.Redirect(send_url);
                        break;

                    case "25":        //開標 = 開標紀錄
                        send_url = "~/pages/MPMS/D/MPMS_D18.aspx?OVC_PURCH=" + strOVC_PURCH;
                        Response.Redirect(send_url);
                        break;

                    case "27":        //簽約 = 保證金(函)收繳
                        send_url = "~/pages/MPMS/D/MPMS_D1D.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_PURCH_5=" + strOVC_PURCH_5;
                        Response.Redirect(send_url);
                        break;
                }
            }
        }

        protected void gvSTATUS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            { /*
                Label lblOVC_STATUS = (Label)e.Row.FindControl("lblOVC_STATUS");
                
                int rowsCount = gvSTATUS.Rows.Count;
                e.Row.Cells.Clear();
                TableCell tbCell = new TableCell();
                TableCell tbCell_2 = new TableCell();
                TableCell tbCell_3 = new TableCell();
                tbCell.ColumnSpan = 2;
                tbCell.Text = "合計";
                tbCell_2.ColumnSpan = 5;
                tbCell_2.Text = rowsCount.ToString();
                tbCell_3.Text = "";
                e.Row.Cells.Add(tbCell);
                e.Row.Cells.Add(tbCell_2);
                e.Row.Cells.Add(tbCell_3);
                */
            }
        }


        protected void btnBack_Click(object sender, EventArgs e)
        {
            //按下 回上一頁
            string strDateYear = "1" + strOVC_PURCH.Substring(2, 2);
            string send_url = "~/pages/MPMS/D/MPMS_D14.aspx?DateYear=" + strDateYear;
            Response.Redirect(send_url);
        }



        #region 副程式
        private bool IsOVC_DO_NAME()
        {
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strDept = "";
            DataTable dt = new DataTable();
            if (strUSER_ID.Length > 0)
            {
                ACCOUNT ac = new ACCOUNT();
                ac = gm.ACCOUNTs.Where(tb => tb.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                if (ac != null)
                {
                    strUserName = ac.USER_NAME.ToString();
                    lblOVC_DO_NAME.Text = strUserName;
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
            DataTable dt = new DataTable();

            //先檢查最該購案的TBMSTATUS最新一筆的STATUS是否為22或24，若是則於TBMSTATUS往下新增一筆
            //STATUS檢查開始
            var queryMaxStatus =
                    (from tbmStatus in mpms.TBMSTATUS
                     where tbmStatus.OVC_PURCH.Equals(strOVC_PURCH)
                     orderby tbmStatus.OVC_STATUS descending
                     select tbmStatus).FirstOrDefault();
            if (queryMaxStatus.OVC_STATUS == "22" || queryMaxStatus.OVC_STATUS == "24")
            {
                TBMSTATUS tbmSTATUS_New = new TBMSTATUS
                {
                    OVC_STATUS_SN = Guid.NewGuid(),
                    OVC_PURCH = queryMaxStatus.OVC_PURCH,
                    OVC_PURCH_5 = queryMaxStatus.OVC_PURCH_5,
                    ONB_TIMES = 1,
                    OVC_DO_NAME = queryMaxStatus.OVC_DO_NAME,
                    OVC_STATUS = queryMaxStatus.OVC_STATUS == "22" ? "23" : "25",
                    OVC_DBEGIN = queryMaxStatus.OVC_DBEGIN,
                };
                mpms.TBMSTATUS.Add(tbmSTATUS_New);
                mpms.SaveChanges();
            }
            //STATUS檢查結束


            var query =
                from tbSTATUS in mpms.TBMSTATUS
                where tbSTATUS.OVC_PURCH.Equals(strOVC_PURCH) && tbSTATUS.OVC_STATUS != "22" && tbSTATUS.OVC_STATUS != "24"
                      && tbSTATUS.OVC_STATUS != "26" && tbSTATUS.OVC_STATUS != "28" && tbSTATUS.OVC_STATUS != "29" && tbSTATUS.OVC_STATUS != "3"
                where tbSTATUS.OVC_DO_NAME.Equals(strUserName)
                orderby tbSTATUS.OVC_STATUS == "3" ? "30".Length : tbSTATUS.OVC_STATUS.Length, tbSTATUS.OVC_STATUS
                select new
                {
                    OVC_PURCH = tbSTATUS.OVC_PURCH,
                    OVC_PURCH_A_5 = "",
                    ONB_TIMES = tbSTATUS.ONB_TIMES,
                    OVC_DBEGIN = tbSTATUS.OVC_DBEGIN,
                    OVC_DEND = tbSTATUS.OVC_DEND,
                    OVC_DO_NAME = tbSTATUS.OVC_DO_NAME,
                    OVC_STATUS = tbSTATUS.OVC_STATUS,
                    OVC_STATUS_Desc = "",
                    WorkDays = "",
                    Date_Flag = "",
                };

            dt = CommonStatic.LinqQueryToDataTable(query);

            DataTable newTable = new DataTable();
            newTable = dt.Clone();
            foreach (DataRow rows in dt.Rows)
            {
                string strOVC_STATUS_Desc = "";
                rows["OVC_PURCH_A_5"] = rows["OVC_PURCH"].ToString() + strOVC_PUR_AGENCY + strOVC_PURCH_5;
                rows["WorkDays"] = GetDaySpan(rows["OVC_DBEGIN"].ToString(), rows["OVC_DEND"].ToString());
                rows["Date_Flag"] = GetDate_Flag(rows["OVC_DBEGIN"].ToString(), rows["OVC_DEND"].ToString());
                //rows["OVC_STATUS_Desc"] = GetTbm1407Desc("Q9", rows["OVC_STATUS"].ToString());
                switch (rows["OVC_STATUS"].ToString())
                {
                    case "19":
                        strOVC_STATUS_Desc = "退評核澄清";
                        break;
                    case "20":
                        strOVC_STATUS_Desc = "收案(採購)";
                        break;
                    case "21":
                        strOVC_STATUS_Desc = "核定";
                        break;
                    case "23":
                        strOVC_STATUS_Desc = "招標";
                        break;
                    case "23-1":
                        strOVC_STATUS_Desc = "疑義、異議";
                        break;
                    case "25":
                        strOVC_STATUS_Desc = "開標";
                        break;
                    case "27":
                        strOVC_STATUS_Desc = "簽約";
                        break;
                }
                rows["OVC_STATUS_Desc"] = strOVC_STATUS_Desc;
                if (rows["OVC_DBEGIN"] != null && rows["OVC_DBEGIN"].ToString() != "")
                    rows["OVC_DBEGIN"] = GetTaiwanDate(rows["OVC_DBEGIN"].ToString());
                if (rows["OVC_DEND"] != null && rows["OVC_DEND"].ToString() != "")
                    rows["OVC_DEND"] = GetTaiwanDate(rows["OVC_DEND"].ToString());

                //疑義、異議檔案數量
                DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D17/Doubt/" + strOVC_PURCH)));
                string NumberOfFiles = dirInfo.Exists == false ? "0" : dirInfo.GetFiles().Length.ToString();
                DirectoryInfo dirInfo_2 = new DirectoryInfo(Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D17/Objection/" + strOVC_PURCH)));
                string NumberOfFiles_2 = dirInfo_2.Exists == false ? "0" : dirInfo_2.GetFiles().Length.ToString();

                //newTable.Rows.Add(rows.ItemArray);
                newTable.ImportRow(rows);
                if (rows["OVC_STATUS"].ToString() == "23")
                {
                    newTable.Rows.Add(rows["OVC_PURCH"].ToString(), rows["OVC_PURCH_A_5"].ToString(), rows["ONB_TIMES"].ToString()
                                , rows["OVC_DBEGIN"].ToString(), rows["OVC_DEND"].ToString(), rows["OVC_DO_NAME"].ToString()
                                , "24", "疑義(" + NumberOfFiles + ")、異議(" + NumberOfFiles_2 + ")", rows["WorkDays"].ToString(), rows["Date_Flag"].ToString());
                }

            }
            ViewState["hasRows"] = FCommon.GridView_dataImport(gvSTATUS, newTable);

            ViewState["dt"] = newTable; //列印使用
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


        public string GetDaySpan(string startDate, string endDate)
        {
            TimeSpan ts;
            if (startDate != "" && startDate != null)
            {
                if (endDate != "" && endDate != null)
                    ts = DateTime.Parse(endDate) - DateTime.Parse(startDate);

                else
                    ts = DateTime.Now - DateTime.Parse(startDate);

                return ts.Days.ToString();
            }
            else
                return string.Empty;
        }


        #region Word輸出
        protected void Btn_P1Click(object sender, EventArgs e)
        {
            if (ViewState["dt"] != null)
            {
                DataTable dt = (DataTable)ViewState["dt"];

                string outPutfilePath = "";
                string fileName = "D14_1-疑義電傳.docx";
                string TempName = "";
                string filePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + fileName));

                TempName = strOVC_PURCH + "-疑義電傳.docx";
                outPutfilePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + TempName));
                File.Copy(filePath, outPutfilePath);
                var valuesToFill = new TemplateEngine.Docx.Content();

                string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
                TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (tbm1301 != null)
                {
                    TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                    if (tbmRECEIVE_BID != null)
                    {
                        //valuesToFill.Fields.Add(new FieldContent("OVC_NAME", tbmRECEIVE_BID.OVC_NAME == null ? "" : tbmRECEIVE_BID.OVC_NAME));
                    }
                    valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5));
                    valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IPURCH", tbm1301.OVC_PUR_IPURCH == null ? "" : tbm1301.OVC_PUR_IPURCH));
                }
                /*
                TBM5200_PPP tbm5200_ppp = mpms.TBM5200_PPP.Where(tb => tb.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                if (tbm5200_ppp != null)
                {
                    valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IUSER_PHONE", tbm5200_ppp.OVC_PUR_IUSER_PHONE == null ? "" : tbm5200_ppp.OVC_PUR_IUSER_PHONE));
                }
                */

                using (TemplateProcessor outputDocument = new TemplateProcessor(outPutfilePath).SetRemoveContentControls(true))
                {
                    outputDocument.FillContent(valuesToFill);
                    outputDocument.SaveChanges();
                }

                FileInfo file = new FileInfo(outPutfilePath);
                string wordPath = outPutfilePath;
                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + TempName);
                Response.ContentType = "application/octet-stream";
                //Response.BinaryWrite(buffer);
                Response.WriteFile(wordPath);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.Flush();
                File.Delete(outPutfilePath);
                File.Delete(wordPath);
                Response.End();
            }
        }

        protected void Btn_P2Click(object sender, EventArgs e)
        {
            if (ViewState["dt"] != null)
            {
                DataTable dt = (DataTable)ViewState["dt"];

                string outPutfilePath = "";
                string fileName = "D14_1-異議電傳.docx";
                string TempName = "";
                string filePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + fileName));

                TempName = strOVC_PURCH + "-異議電傳.docx";
                outPutfilePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + TempName));
                File.Copy(filePath, outPutfilePath);
                var valuesToFill = new TemplateEngine.Docx.Content();

                TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (tbm1301 != null)
                {
                    TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                    valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5));
                    valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IPURCH", tbm1301.OVC_PUR_IPURCH == null ? "" : tbm1301.OVC_PUR_IPURCH));
                }

                using (TemplateProcessor outputDocument = new TemplateProcessor(outPutfilePath).SetRemoveContentControls(true))
                {
                    outputDocument.FillContent(valuesToFill);
                    outputDocument.SaveChanges();
                }


                FileInfo file = new FileInfo(outPutfilePath);
                string wordPath = outPutfilePath;
                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + TempName);
                Response.ContentType = "application/octet-stream";
                //Response.BinaryWrite(buffer);
                Response.WriteFile(wordPath);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.Flush();
                File.Delete(outPutfilePath);
                File.Delete(wordPath);
                Response.End();
            }
        }
        protected void Btn_P3Click(object sender, EventArgs e)
        {
            string outPutfilePath = "";
            string fileName = "D14_1-傳真通知書.docx";
            string TempName = "";
            string filePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + fileName));
            TempName = strOVC_PURCH + "-傳真通知書.docx";
            outPutfilePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + TempName));
            File.Copy(filePath, outPutfilePath);
            var valuesToFill = new TemplateEngine.Docx.Content();

            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IPURCH", tbm1301.OVC_PUR_IPURCH == null ? "" : tbm1301.OVC_PUR_IPURCH));
                valuesToFill.Fields.Add(new FieldContent("OVC_RECEIVE_NSECTION", tbm1301.OVC_RECEIVE_NSECTION == null ? "" : tbm1301.OVC_RECEIVE_NSECTION));
            }

            using (TemplateProcessor outputDocument = new TemplateProcessor(outPutfilePath).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }

            FileInfo file = new FileInfo(outPutfilePath);
            string wordPath = outPutfilePath;
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + TempName);
            Response.ContentType = "application/octet-stream";
            //Response.BinaryWrite(buffer);
            Response.WriteFile(wordPath);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            File.Delete(outPutfilePath);
            //File.Delete(wordPath);
            Response.End();
            
        }
        #endregion


        

        public string GetDate_Flag(string startDate, string endDate)
        {
            string StrSaySpan = GetDaySpan(startDate, endDate);
            if (StrSaySpan != null)
            {
                if (int.Parse(StrSaySpan) > 30)
                    return "1";
            }
            return "0";
        }



        public string FlagVisible(string strOVC_DBEGIN)
        {
            //跟現在日期差30天以上 return "1"=>顯示 /  "0"=>隱藏
            if (strOVC_DBEGIN != null)
            {
                if ((Convert.ToDateTime(strOVC_DBEGIN) <= DateTime.Now.AddDays(-30)))
                {
                    return "1";
                }
            }
            return "0";
        }


        private String GetTbm1407Desc(string cateID, string codeID)
        {
            //將TBM1407片語ID代碼轉為Desc
            TBM1407 tbm1407 = new TBM1407();
            if (codeID != null && codeID != "")
            {
                tbm1407 = gm.TBM1407.Where(tb => tb.OVC_PHR_CATE.Equals(cateID) && tb.OVC_PHR_ID.Equals(codeID)).OrderBy(tb => tb.OVC_PHR_ID).FirstOrDefault();
                if (tbm1407 != null)
                {
                    return tbm1407.OVC_PHR_DESC.ToString();
                }
            }
            return codeID;
        }

        #endregion

    }
}