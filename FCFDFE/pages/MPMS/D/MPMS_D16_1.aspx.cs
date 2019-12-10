using System;
using System.Data;
using System.Linq;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using TemplateEngine.Docx;
using System.IO;
using System.Text.RegularExpressions;



namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D16_1 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strOVC_PURCH, strOVC_PUR_AGENCY, strOVC_PURCH_5, strOVC_DOPEN;
        short numONB_TIMES;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            {
                FCommon.Controls_Attributes("readonly", "true", txtOVC_DBID_LIMIT, txtOVC_DOPEN, txtOVC_DANNOUNCE, txtOVC_DAPPROVE);

                if (FCommon.getQueryString(this, "OVC_PURCH", out strOVC_PURCH, false))
                {
                    TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PUR_AGENCY = tbm1301 == null? "" : tbm1301.OVC_PUR_AGENCY;
                    TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                    strOVC_DOPEN = Request.QueryString["OVC_DOPEN"] == null ? "" : Request.QueryString["OVC_DOPEN"].ToString();
                    if (Request.QueryString["ONB_TIMES"] != null)
                        short.TryParse(Request.QueryString["ONB_TIMES"].ToString(), out numONB_TIMES);
                    if (IsOVC_DO_NAME() && !IsPostBack)
                        DataImport();
                }
                else
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
            }
        }




        #region Text / Selected Changed
        protected void rdoOVC_CHAIRMAN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoOVC_CHAIRMAN.SelectedValue != "" && rdoOVC_CHAIRMAN.SelectedValue != null)
                rdoOVC_CHAIRMAN_Other.Checked = false;
        }

        protected void rdoOVC_CHAIRMAN_Other_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOVC_CHAIRMAN_Other.Checked == true)
                rdoOVC_CHAIRMAN.SelectedValue = null;
        }

        protected void drpOVC_PUR_ASS_VEN_CODE_SelectedIndexChanged(object sender, EventArgs e)
        {
            //招標方式變動
            SetOVC_DWAIT();
        }

        protected void txtOVC_DOPEN_TextChanged(object sender, EventArgs e)
        {
            //開標日期變動
            SetOVC_DWAIT();
            SetOVC_DRAFT_COMM();
        }

        protected void txtOVC_OPEN_HOUR_TextChanged(object sender, EventArgs e)
        {
            //開標時間變動
            SetOVC_DRAFT_COMM();
        }

        protected void txtOVC_DANNOUNCE_TextChanged(object sender, EventArgs e)
        {
            //公告日期變動
            SetOVC_DWAIT();
        }

        protected void drpOVC_BID_MONEY_SelectedIndexChanged(object sender, EventArgs e)
        {
            //押標金額度
            txtOVC_BID_MONEY.Text = "";
            SetOVC_BID_MONEY();
        }

        protected void drpOVC_DESC_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //說明 一、
            if (drpOVC_DESC_1.SelectedValue != "")
                txtOVC_DESC_1.Text = "一、" + drpOVC_DESC_1.SelectedValue;
        }

        protected void drpOVC_DESC_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //說明  二、
            if (drpOVC_DESC_2.SelectedValue != "")
                txtOVC_DESC_2.Text = "二、" + drpOVC_DESC_2.SelectedValue;
        }

        protected void drpOVC_DESC_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //說明 三、
            if (drpOVC_DESC_3.SelectedValue != "")
            {
                if (!txtOVC_DESC_3.Text.Contains("三、其他："))
                    txtOVC_DESC_3.Text += txtOVC_DESC_3.Text == "" ? "三、其他：" : "\r\n三、其他：";
                txtOVC_DESC_3.Text += "\r\n　　■" + drpOVC_DESC_3.SelectedValue;
            }
        }

        #endregion




        #region Button Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //點擊 存檔
            if (IsSave())
            {
                TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (tbm1301 != null)
                {
                    if (drpOVC_BID_MONEY.SelectedItem.Text == "按廠商報價") tbm1301.OVC_BID_MONEY = "按廠商報價，百分之" + txtOVC_BID_MONEY.Text;
                    else if (drpOVC_BID_MONEY.SelectedItem.Text == "定額") tbm1301.OVC_BID_MONEY = "定額，新台幣" + txtOVC_BID_MONEY.Text + "元整";
                    else tbm1301.OVC_BID_MONEY = "免繳";
                    gm.SaveChanges();
                    
                    if (strOVC_DOPEN == "")
                    {
                        //新增
                        CreateData( short.Parse(lblONB_TIMES.Text));
                    }
                    else
                    {
                        //修改
                        string strOVC_DESC_1, strOVC_DESC_2, strOVC_DESC_3;
                        strOVC_DESC_1 = txtOVC_DESC_1.Text == "" ? "" : txtOVC_DESC_1.Text + "\r\n";
                        strOVC_DESC_2 = txtOVC_DESC_2.Text == "" ? "" : txtOVC_DESC_2.Text + "\r\n";
                        strOVC_DESC_3 = txtOVC_DESC_3.Text == "" ? "" : txtOVC_DESC_3.Text + "\r\n";

                        TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                                && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                                && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                        if (tbmRECEIVE_ANNOUNCE != null)
                        {
                            tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_1 = chkOVC_BID_METHOD.Items[0].Selected == true ? chkOVC_BID_METHOD.Items[0].Value : string.Empty;
                            tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_2 = chkOVC_BID_METHOD.Items[1].Selected == true ? chkOVC_BID_METHOD.Items[1].Value : string.Empty;
                            tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_3 = chkOVC_BID_METHOD.Items[2].Selected == true ? chkOVC_BID_METHOD.Items[2].Value : string.Empty;
                            tbmRECEIVE_ANNOUNCE.OVC_BUDGET_BUY = txtOVC_BUDGET_BUY.Text == "" ? "同上": txtOVC_BUDGET_BUY.Text;
                            tbmRECEIVE_ANNOUNCE.OVC_DBID_LIMIT = txtOVC_DBID_LIMIT.Text;
                            tbmRECEIVE_ANNOUNCE.OVC_LIMIT_HOUR = txtOVC_LIMIT_HOUR.Text;
                            tbmRECEIVE_ANNOUNCE.OVC_LIMIT_MIN = txtOVC_LIMIT_MIN.Text;
                            tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR = txtOVC_OPEN_HOUR.Text;
                            tbmRECEIVE_ANNOUNCE.OVC_OPEN_MIN = txtOVC_OPEN_MIN.Text;
                            tbmRECEIVE_ANNOUNCE.OVC_PLACE = txtOVC_PLACE.Text;
                            tbmRECEIVE_ANNOUNCE.OVC_DANNOUNCE = txtOVC_DANNOUNCE.Text;
                            tbmRECEIVE_ANNOUNCE.OVC_DESC = strOVC_DESC_1+ strOVC_DESC_2+ strOVC_DESC_3;
                            tbmRECEIVE_ANNOUNCE.OVC_DRAFT_COMM = txtOVC_DRAFT_COMM.Text;
                            tbmRECEIVE_ANNOUNCE.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;

                            mpms.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmRECEIVE_ANNOUNCE.GetType().Name.ToString(), this, "修改");


                            TBMRECEIVE_WORK tbmRECEIVE_WORK = new TBMRECEIVE_WORK();
                            tbmRECEIVE_WORK = mpms.TBMRECEIVE_WORK.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                                    && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                                    && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                            if (tbmRECEIVE_WORK != null)
                            {
                                if (rdoOVC_CHAIRMAN.SelectedValue == "處長" || rdoOVC_CHAIRMAN.SelectedValue == "副處長")
                                    tbmRECEIVE_WORK.OVC_CHAIRMAN = rdoOVC_CHAIRMAN.SelectedValue;
                                else if (rdoOVC_CHAIRMAN_Other.Checked == true)
                                    tbmRECEIVE_WORK.OVC_CHAIRMAN = txtOVC_CHAIRMAN_Other.Text;
                                tbmRECEIVE_WORK.OVC_OPEN_HOUR = txtOVC_OPEN_HOUR.Text;
                                tbmRECEIVE_WORK.OVC_OPEN_MIN = txtOVC_OPEN_MIN.Text;
                                tbmRECEIVE_WORK.OVC_BID_MONEY = txtOVC_BID_MONEY.Text;
                                tbmRECEIVE_WORK.OVC_MEETING = txtOVC_MEETING.Text;
                                tbmRECEIVE_WORK.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;
                                tbmRECEIVE_WORK.OVC_DESC = strOVC_DESC_1 + strOVC_DESC_2 + strOVC_DESC_3;
                                tbmRECEIVE_WORK.OVC_DWAIT = txtOVC_DWAIT.Text;
                                mpms.SaveChanges();
                            }
                        }
                    }

                    //政賢看這邊
                    //if (txtOVC_DAPPROVE.Text != "")
                    //{
                    //    //修改 OVC_STATUS="23" 開標通知的 階段結束日=主官核批日
                    //    TBMSTATUS tbmSTATUS_23 = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                    //                && tb.ONB_TIMES == 1 && tb.OVC_STATUS.Equals("23")).FirstOrDefault();
                    //    if (tbmSTATUS_23 != null)
                    //    {
                    //        tbmSTATUS_23.OVC_DEND = txtOVC_DAPPROVE.Text;
                    //        mpms.SaveChanges();
                    //    }
                    //
                    //    //新增 OVC_STATUS="25" 開標紀錄的 階段開始日
                    //    string strOVC_DO_NAME = "";
                    //    TBMSTATUS tbmSTATUS_25 = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                    //                && tb.ONB_TIMES == 1 && tb.OVC_STATUS.Equals("25")).FirstOrDefault();
                    //    if (tbmSTATUS_25 == null)
                    //    {
                    //        //新增 TBMSTATUS (購案階段紀錄檔)
                    //        TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    //        if (tbmRECEIVE_BID != null)
                    //            strOVC_DO_NAME = tbmRECEIVE_BID.OVC_DO_NAME;
                    //
                    //        TBMSTATUS tbmSTATUS_New = new TBMSTATUS
                    //        {
                    //            OVC_STATUS_SN = Guid.NewGuid(),
                    //            OVC_PURCH = strOVC_PURCH,
                    //            OVC_PURCH_5 = strOVC_PURCH_5,
                    //            ONB_TIMES = 1,
                    //            OVC_DO_NAME = strOVC_DO_NAME,
                    //            OVC_STATUS = "25",
                    //            OVC_DBEGIN = DateTime.Now.ToString("yyyy-MM-dd"),
                    //        };
                    //        mpms.TBMSTATUS.Add(tbmSTATUS_New);
                    //        mpms.SaveChanges();
                    //    }
                    //}

                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功！");
                    divBtn.Visible = true;
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            //點擊 回上一頁
            string send_url = "~/pages/MPMS/D/MPMS_D16.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_PURCH_5=" + strOVC_PURCH_5;
            Response.Redirect(send_url);
        }

        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            //點擊 回主流程畫面
            string send_url = "~/pages/MPMS/D/MPMS_D14_1.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_PURCH_5=" + strOVC_PURCH_5;
            Response.Redirect(send_url);
        }
 
        protected void btnFixNotice_Click(object sender, EventArgs e)
        {
            //點擊 修正公告稿作業
            string strDate, strTimes;
            strDate = strOVC_DOPEN == "" ? Convert.ToString(ViewState["SavedOVC_DOPEN"]) : strOVC_DOPEN;
            strTimes = numONB_TIMES == 0 ? Convert.ToString(ViewState["SavedONB_TIMES"]) : numONB_TIMES.ToString();
            string send_url = "~/pages/MPMS/D/MPMS_D16_2.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_DOPEN=" + strDate + "&ONB_TIMES=" + strTimes;
            Response.Redirect(send_url);
        }

        #region 簽辦單
        protected void lbtnToWordD16_1_1_Click(object sender, EventArgs e)
        {
            //簽辦表(Word格式)
            string filepath = OutputWordD16_1_1();
            string fileName = strOVC_PURCH + "-簽辦單.docx";
            FileInfo fileInfo = new FileInfo(filepath);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            System.IO.File.Delete(filepath);
            Response.End();
        }
        protected void lbtnToWordD16_1_1_pdf_Click(object sender, EventArgs e)
        {
            string filepath = OutputWordD16_1_1();
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/WordD16_1_1_pdf.pdf";
            string fileName = strOVC_PURCH + "-簽辦單.pdf";
            FCommon.WordToPDF(this, filepath, filetemp, fileName);
        }
        protected void lbtnToWordD16_1_1_odt_Click(object sender, EventArgs e)
        {
            string filepath = OutputWordD16_1_1();
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/WordD16_1_1_odt.odt";
            string FileName = strOVC_PURCH + "-簽辦單.odt";
            FCommon.WordToOdt(this, filepath, filetemp, FileName);
        }
        #endregion

        #region 開標通知單(上呈版)
        protected void lbtnToWordD16_1_2_Click(object sender, EventArgs e)
        {
            //開標通知單(上呈版)
            string filepath = OutputWordD16_1_2();
            if (filepath != "")
            {
                string fileName = strOVC_PURCH + "-開標通知單(上呈版).docx";
                FileInfo fileInfo = new FileInfo(filepath);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                Response.WriteFile(fileInfo.FullName);
                Response.Flush();
                System.IO.File.Delete(filepath);
                Response.End();
            }
        }
        protected void lbtnToWordD16_1_2_pdf_Click(object sender, EventArgs e)
        {
            string filepath = OutputWordD16_1_2();
            if (filepath != "")
            {
                string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/WordD16_1_2_pdf.pdf";
                string fileName = strOVC_PURCH + "-開標通知單(上呈版).pdf";
                FCommon.WordToPDF(this, filepath, filetemp, fileName);
            }
        }
        protected void lbtnToWordD16_1_2_odt_Click(object sender, EventArgs e)
        {
            string filepath = OutputWordD16_1_2();
            if (filepath != "")
            {
                string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/WordD16_1_2_odt.odt";
                string FileName = strOVC_PURCH + "-開標通知單(上呈版).odt";
                FCommon.WordToOdt(this, filepath, filetemp, FileName);
            }
        }
        #endregion

        #region 開標通知單(正式版)
        protected void lbtnToWordD16_1_3_Click(object sender, EventArgs e)
        {
            //開標通知單(正式版)
            string filepath = OutputWordD16_1_3();
            if (filepath != "")
            {
                string fileName = strOVC_PURCH + "-開標通知單(正式版).docx";
                FileInfo fileInfo = new FileInfo(filepath);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                Response.WriteFile(fileInfo.FullName);
                Response.Flush();
                System.IO.File.Delete(filepath);
                Response.End();
            }
        }
        protected void lbtnToWordD16_1_3_pdf_Click(object sender, EventArgs e)
        {
            string filepath = OutputWordD16_1_3();
            if (filepath != "")
            {
                string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/WordD16_1_3_pdf.pdf";
                string fileName = strOVC_PURCH + "-開標通知單(正式版).pdf";
                FCommon.WordToPDF(this, filepath, filetemp, fileName);
            }
        }
        protected void lbtnToWordD16_1_3_odt_Click(object sender, EventArgs e)
        {
            string filepath = OutputWordD16_1_3();
            if (filepath != "")
            {
                string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/WordD16_1_3_odt.odt";
                string FileName = strOVC_PURCH + "-開標通知單(正式版).odt";
                FCommon.WordToOdt(this, filepath, filetemp, FileName);
            }
        }
        #endregion

        #region 招標文件封面
        protected void lbtnToWordD16_1_4_Click(object sender, EventArgs e)
        {
            //招標文件封面
            string filepath = OutputWordD16_1_4();
            if (filepath != "")
            {
                string fileName = strOVC_PURCH + "-招標文件封面.docx";
                FileInfo fileInfo = new FileInfo(filepath);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                Response.WriteFile(fileInfo.FullName);
                Response.Flush();
                System.IO.File.Delete(filepath);
                Response.End();
            }
        }
        protected void lbtnToWordD16_1_4_pdf_Click(object sender, EventArgs e)
        {
            string filepath = OutputWordD16_1_4();
            if (filepath != "")
            {
                string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/WordD16_1_4_pdf.pdf";
                string fileName = strOVC_PURCH + "-招標文件封面.pdf";
                FCommon.WordToPDF(this, filepath, filetemp, fileName);
            }
        }
        protected void lbtnToWordD16_1_4_odt_Click(object sender, EventArgs e)
        {
            string filepath = OutputWordD16_1_4();
            if (filepath != "")
            {
                string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/WordD16_1_4_odt.odt";
                string FileName = strOVC_PURCH + "-招標文件封面.odt";
                FCommon.WordToOdt(this, filepath, filetemp, FileName);
            }
        }
        #endregion


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
                        strErrorMsg = "<p>請選擇購案</p>";
                    else if (tbm1301 == null)
                        strErrorMsg = "<p>查無此購案編號</p>";
                    else
                    {
                        TBM1301_PLAN plan1301 =
                            gm.TBM1301_PLAN.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCHASE_UNIT.Equals(strDept)).FirstOrDefault();

                        TBMRECEIVE_BID tbmRECEIVE_BID =
                            mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_DO_NAME.Equals(strUserName)).FirstOrDefault();
                        if (tbmRECEIVE_BID == null || plan1301 == null)
                            strErrorMsg = "<p>非此購案的承辦人</p>";
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
            //表單帶入資料
            bool isRESTRICT;
            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                //招標方式
                string strOVC_PUR_ASS_VEN_CODE = GetTbm1407Desc("C7", tbm1301.OVC_PUR_ASS_VEN_CODE == null ? "" : tbm1301.OVC_PUR_ASS_VEN_CODE);
                isRESTRICT = strOVC_PUR_ASS_VEN_CODE.Contains("限制性招標");

                lblOVC_PURCH_A_5.Text = tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                lblOVC_PUR_APPROVE.Text = tbm1301.OVC_PUR_APPROVE;
                lblOVC_PUR_IPURCH.Text = tbm1301.OVC_PUR_IPURCH;
                lblOVC_PUR_NSECTION.Text = tbm1301.OVC_PUR_NSECTION;
                SetDrp(drpOVC_PUR_ASS_VEN_CODE, "C7");
                drpOVC_PUR_ASS_VEN_CODE.SelectedValue = tbm1301.OVC_PUR_ASS_VEN_CODE;
                if (tbm1301.OVC_PURCH_KIND == "1")
                {
                    lblONB_PUR_BUDGET.Text = "新台幣 " + string.Format("{0:N}", tbm1301.ONB_PUR_BUDGET) + "元整";
                }
                else if (tbm1301.OVC_PURCH_KIND == "2")
                {
                    lblONB_PUR_BUDGET.Text = GetTbm1407Desc("B0", tbm1301.OVC_PUR_CURRENT) + " " + string.Format("{0:N}", tbm1301.ONB_PUR_BUDGET);
                    lblONB_PUR_RATE.Visible = true;
                    lblONB_PUR_RATE.Text += " (匯率：" + tbm1301.ONB_PUR_RATE + ")";
                }
                //押標金額度
                if (tbm1301.OVC_BID_MONEY != null)
                {
                    for (int i = 1; i < drpOVC_BID_MONEY.Items.Count; i++)
                    {
                        if (tbm1301.OVC_BID_MONEY.Contains(drpOVC_BID_MONEY.Items[i].ToString()))
                        {
                            drpOVC_BID_MONEY.Items[i].Selected = true;
                        }
                    }
                    string numOVC_BID_MONEY = Regex.Replace(tbm1301.OVC_BID_MONEY, @"[^\d.\d]", "");
                    txtOVC_BID_MONEY.Text = numOVC_BID_MONEY;
                }
                SetOVC_BID_MONEY();

                lblOVC_BID_TIMES.Text = "投、開標方式：" + GetTbm1407Desc("TG", tbm1301.OVC_BID_TIMES);




                if (strOVC_DOPEN != "")
                {
                    //修改
                    lblONB_TIMES.Text = numONB_TIMES.ToString();
                    lblOVC_DOPEN.Visible = true;
                    divOVC_DOPEN.Visible = false;

                    TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
                    tbmRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                            && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                            && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                    if (tbmRECEIVE_ANNOUNCE != null)
                    {
                        divBtn.Visible = true;
                        if (chkOVC_BID_METHOD.Items[0].Value == tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_1)
                            chkOVC_BID_METHOD.Items[0].Selected = true;
                        if (chkOVC_BID_METHOD.Items[1].Value == tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_2)
                            chkOVC_BID_METHOD.Items[1].Selected = true;
                        if (chkOVC_BID_METHOD.Items[2].Value == tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_3)
                            chkOVC_BID_METHOD.Items[2].Selected = true;
                        txtOVC_DBID_LIMIT.Text = tbmRECEIVE_ANNOUNCE.OVC_DBID_LIMIT;
                        txtOVC_LIMIT_HOUR.Text = tbmRECEIVE_ANNOUNCE.OVC_LIMIT_HOUR;
                        txtOVC_LIMIT_MIN.Text = tbmRECEIVE_ANNOUNCE.OVC_LIMIT_MIN;
                        lblOVC_DOPEN.Text = GetTaiwanDate(tbmRECEIVE_ANNOUNCE.OVC_DOPEN,false) + " " + tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR + "時" + tbmRECEIVE_ANNOUNCE.OVC_OPEN_MIN + "分";
                        txtOVC_DOPEN.Text = tbmRECEIVE_ANNOUNCE.OVC_DOPEN;
                        txtOVC_OPEN_HOUR.Text = tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR;
                        txtOVC_OPEN_MIN.Text = tbmRECEIVE_ANNOUNCE.OVC_OPEN_MIN;
                        txtOVC_BUDGET_BUY.Text = tbmRECEIVE_ANNOUNCE.OVC_BUDGET_BUY;
                        txtOVC_PLACE.Text = tbmRECEIVE_ANNOUNCE.OVC_PLACE == null ? "國防部武德樓1樓開標室" : tbmRECEIVE_ANNOUNCE.OVC_PLACE;

                        txtOVC_DESC_3.Text = tbmRECEIVE_ANNOUNCE.OVC_DESC;
                        txtOVC_DANNOUNCE.Text = tbmRECEIVE_ANNOUNCE.OVC_DANNOUNCE;
                        string strOVC_DOPEN_tw = txtOVC_DOPEN.Text == "" ? "" : GetTaiwanDate(txtOVC_DOPEN.Text,false);
                        string strWeek = txtOVC_DOPEN.Text == "" ? "" : (DateTime.Parse(txtOVC_DOPEN.Text)).ToString("dddd", new System.Globalization.CultureInfo("zh-cn"));
                        if (tbmRECEIVE_ANNOUNCE.OVC_DRAFT_COMM != null)
                            txtOVC_DRAFT_COMM.Text = tbmRECEIVE_ANNOUNCE.OVC_DRAFT_COMM;
                        else
                            SetOVC_DRAFT_COMM();
                        txtOVC_DAPPROVE.Text = tbmRECEIVE_ANNOUNCE.OVC_DAPPROVE;
                    }

                    TBMRECEIVE_WORK tbmRECEIVE_WORK = new TBMRECEIVE_WORK();
                    tbmRECEIVE_WORK = mpms.TBMRECEIVE_WORK.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                            && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                            && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                    if (tbmRECEIVE_WORK != null)
                    {
                        if (tbmRECEIVE_WORK.OVC_CHAIRMAN == "處長" || tbmRECEIVE_WORK.OVC_CHAIRMAN == "副處長")
                            rdoOVC_CHAIRMAN.SelectedValue = tbmRECEIVE_WORK.OVC_CHAIRMAN;
                        else if (tbmRECEIVE_WORK.OVC_CHAIRMAN != null && tbmRECEIVE_WORK.OVC_CHAIRMAN != "")
                        {
                            txtOVC_CHAIRMAN_Other.Text = tbmRECEIVE_WORK.OVC_CHAIRMAN;
                            rdoOVC_CHAIRMAN_Other.Checked = true;
                        }
                        txtOVC_DWAIT.Text = tbmRECEIVE_WORK.OVC_DWAIT;
                        txtOVC_MEETING.Text = tbmRECEIVE_WORK.OVC_MEETING;
                        txtOVC_DAPPROVE.Text = tbmRECEIVE_WORK.OVC_DAPPROVE;
                    }
                    else
                        SetOVC_DWAIT();
                }

                else
                {
                    //新增
                    short newONB_TIMES = 1;
                    TBMRECEIVE_ANNOUNCE maxONB_TIMES = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)).OrderByDescending(tb => tb.ONB_TIMES).FirstOrDefault();
                    if (maxONB_TIMES != null)
                        newONB_TIMES = short.Parse((maxONB_TIMES.ONB_TIMES + 1).ToString());
                    lblONB_TIMES.Text = newONB_TIMES.ToString();
                    txtOVC_PLACE.Text = "國防部武德樓1樓開標室";
                    divBtn.Visible = false;
                    lblOVC_DOPEN.Visible = false;
                    divOVC_DOPEN.Visible = true;
                }
                SetOVC_DANNOUNCE_Visible();
            }
        }


        private bool IsSave()
        {
            string strErrorMsg = "";
            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 == null)
                strErrorMsg = "<p>查無此購案編號</p>";
            else
            {
                if (txtOVC_DOPEN.Text == "")
                {
                    strErrorMsg = "<p>請輸入開標時間(日期)</p>";
                    errorOVC_DOPEN.Visible = true;
                }
                else
                    errorOVC_DOPEN.Visible = false;

                if (txtOVC_OPEN_HOUR.Text == "")
                {
                    strErrorMsg += "<p>請輸入開標時間(時)</p>";
                    errorOVC_OPEN_HOUR.Visible = true;
                }
                else
                    errorOVC_OPEN_HOUR.Visible = false;

                if (txtOVC_OPEN_MIN.Text == "")
                {
                    strErrorMsg += "<p>請輸入開標時間(分)</p>";
                    errorOVC_OPEN_MIN.Visible = true;
                }
                else
                    errorOVC_OPEN_MIN.Visible = false;

                if (strOVC_DOPEN == "")
                {
                    if (isExist(txtOVC_DOPEN.Text))
                        strErrorMsg += "<p>此購案編號的開標日期已存在，請選擇其他開標日期</p>";
                }
            }

            if (strErrorMsg != "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErrorMsg);
            else
                return true;

            return false;
        }


        private bool isExist(string strDate)
        {
            var queryRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                                   && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)).ToArray();
            if (strDate == "")
            {
                if (queryRECEIVE_ANNOUNCE.Count() != 0)
                    return true;
            }
            else
            {
                var queryDOPEN =  queryRECEIVE_ANNOUNCE.Where(tb => tb.OVC_DOPEN.Equals(strDate)).FirstOrDefault();
                if(queryDOPEN != null)
                    return true;
            }
            return false;
        }




        private void CreateData(short newONB_TIMES)
        {
            //新增 TBMRECEIVE_ANNOUNCE (採購公告紀錄檔)
            string strOVC_DESC_1, strOVC_DESC_2, strOVC_DESC_3;
            strOVC_DESC_1 = txtOVC_DESC_1.Text == "" ? "" : txtOVC_DESC_1.Text + "\r\n";
            strOVC_DESC_2 = txtOVC_DESC_2.Text == "" ? "" : txtOVC_DESC_2.Text + "\r\n";
            strOVC_DESC_3 = txtOVC_DESC_3.Text == "" ? "" : txtOVC_DESC_3.Text + "\r\n";

            TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE_new = new TBMRECEIVE_ANNOUNCE();
            tbmRECEIVE_ANNOUNCE_new.OVC_PURCH = strOVC_PURCH;
            tbmRECEIVE_ANNOUNCE_new.OVC_PURCH_5 = strOVC_PURCH_5;
            tbmRECEIVE_ANNOUNCE_new.ONB_TIMES = newONB_TIMES;

            //領投標資料
            tbmRECEIVE_ANNOUNCE_new.OVC_BUDGET_BUY = txtOVC_BUDGET_BUY.Text == "" ? "同上" : txtOVC_BUDGET_BUY.Text;
            tbmRECEIVE_ANNOUNCE_new.OVC_DBID_LIMIT = txtOVC_DBID_LIMIT.Text;
            tbmRECEIVE_ANNOUNCE_new.OVC_LIMIT_HOUR = txtOVC_LIMIT_HOUR.Text;
            tbmRECEIVE_ANNOUNCE_new.OVC_LIMIT_MIN = txtOVC_LIMIT_MIN.Text;
            tbmRECEIVE_ANNOUNCE_new.OVC_DOPEN = txtOVC_DOPEN.Text;
            tbmRECEIVE_ANNOUNCE_new.OVC_OPEN_HOUR = txtOVC_OPEN_HOUR.Text;
            tbmRECEIVE_ANNOUNCE_new.OVC_OPEN_MIN = txtOVC_OPEN_MIN.Text;
            tbmRECEIVE_ANNOUNCE_new.OVC_PLACE = txtOVC_PLACE.Text;
            tbmRECEIVE_ANNOUNCE_new.OVC_DANNOUNCE = txtOVC_DANNOUNCE.Text;
            tbmRECEIVE_ANNOUNCE_new.OVC_DESC = strOVC_DESC_1 + strOVC_DESC_2 + strOVC_DESC_3;
            tbmRECEIVE_ANNOUNCE_new.OVC_DRAFT_COMM = txtOVC_DRAFT_COMM.Text;
            tbmRECEIVE_ANNOUNCE_new.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;

            mpms.TBMRECEIVE_ANNOUNCE.Add(tbmRECEIVE_ANNOUNCE_new);
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmRECEIVE_ANNOUNCE_new.GetType().Name.ToString(), this, "新增");


            //新增 TBMRECEIVE_WORK (採購簽辦檔)
            TBMRECEIVE_WORK tbmRECEIVE_WORK_new = new TBMRECEIVE_WORK();
            tbmRECEIVE_WORK_new.OVC_PURCH = strOVC_PURCH;
            tbmRECEIVE_WORK_new.OVC_PURCH_5 = strOVC_PURCH_5;
            tbmRECEIVE_WORK_new.OVC_DOPEN = txtOVC_DOPEN.Text;
            tbmRECEIVE_WORK_new.OVC_OPEN_HOUR = txtOVC_OPEN_HOUR.Text;
            tbmRECEIVE_WORK_new.OVC_OPEN_MIN = txtOVC_OPEN_MIN.Text;
            tbmRECEIVE_WORK_new.ONB_TIMES = numONB_TIMES;   //short.Parse(lblONB_TIMES.Text)
            tbmRECEIVE_WORK_new.OVC_DWAIT = txtOVC_DWAIT.Text;

            if (rdoOVC_CHAIRMAN.SelectedValue == "處長" || rdoOVC_CHAIRMAN.SelectedValue == "副處長")
                tbmRECEIVE_WORK_new.OVC_CHAIRMAN = rdoOVC_CHAIRMAN.SelectedValue;
            else if (rdoOVC_CHAIRMAN_Other.Checked == true)
                tbmRECEIVE_WORK_new.OVC_CHAIRMAN = txtOVC_CHAIRMAN_Other.Text;
            tbmRECEIVE_WORK_new.OVC_DESC = strOVC_DESC_1 + strOVC_DESC_2 + strOVC_DESC_3;
            tbmRECEIVE_WORK_new.OVC_MEETING = txtOVC_MEETING.Text;
            tbmRECEIVE_WORK_new.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;
            mpms.TBMRECEIVE_WORK.Add(tbmRECEIVE_WORK_new);
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmRECEIVE_WORK_new.GetType().Name.ToString(), this, "新增");


            ViewState["SavedOVC_DOPEN"] = txtOVC_DOPEN.Text;
            ViewState["SavedONB_TIMES"] = newONB_TIMES;
        }

        private String GetTbm1407Desc(string cateID, string codeID)
        {
            //將TBM1407片語ID代碼轉為Desc
            TBM1407 tbm1407 = new TBM1407();
            if (codeID != null && codeID != "")
            {
                tbm1407 = gm.TBM1407.Where(tb => tb.OVC_PHR_CATE.Equals(cateID) && tb.OVC_PHR_ID.Equals(codeID)).OrderBy(tb=>tb.OVC_PHR_ID).FirstOrDefault();
                if (tbm1407 != null)
                {
                    return tbm1407.OVC_PHR_DESC.ToString();
                }
                else
                {
                    return codeID;
                }
            }
            return "";
        }

        public string GetTaiwanDate(string strDate, bool hasWeek)
        {
            //西元年轉民國年
            if (strDate != null && strDate != "")
            {
                DateTime datetime = Convert.ToDateTime(strDate);
                CultureInfo info = new CultureInfo("zh-TW");
                TaiwanCalendar twC = new TaiwanCalendar();
                info.DateTimeFormat.Calendar = twC;
                if(hasWeek)
                    return datetime.ToString("yyy年MM月dd日", info) + "(" + datetime.ToString("dddd", new System.Globalization.CultureInfo("zh-cn")) + ")";
                else
                    return datetime.ToString("yyy年MM月dd日", info);
            }
            return strDate;
        }


        #region Set
        private void SetDrp(ListControl list, string cateID)
        {
            //設定DropDownList選項
            DataTable dt;
            dt = CommonStatic.ListToDataTable(gm.TBM1407.Where(tb => tb.OVC_PHR_CATE.Equals(cateID)).ToList());
            FCommon.list_dataImport(list, dt, "OVC_PHR_DESC", "OVC_PHR_ID", true);
        }

        private void SetOVC_DRAFT_COMM()
        {
            string strOVC_DOPEN_tw = txtOVC_DOPEN.Text == "" ? "" : GetTaiwanDate(txtOVC_DOPEN.Text,true);
            txtOVC_DRAFT_COMM.Text = "奉核後，訂於" + strOVC_DOPEN_tw + txtOVC_OPEN_HOUR.Text + "時辦理本案第" + lblONB_TIMES.Text + "次開標，並附呈〈標單稿(財勞務)招標須知稿(工程類)〉；另借用軍網、民網USB隨身碟各乙枚，辦理公告上傳事宜。";
        }

        private void SetOVC_DWAIT()
        {
            if (drpOVC_PUR_ASS_VEN_CODE.SelectedValue != null && drpOVC_PUR_ASS_VEN_CODE.SelectedValue != "")
            {
                string strOVC_PUR_ASS_VEN_CODE = drpOVC_PUR_ASS_VEN_CODE.SelectedItem.Text;
                bool isRESTRICT = strOVC_PUR_ASS_VEN_CODE.Contains("限制性招標");
                if (!isRESTRICT)
                {
                    //等標期 = 系統自動計算：開標日前一日 - 公告日期 (亦可自行修訂)
                    if (txtOVC_DOPEN.Text != "" && txtOVC_DANNOUNCE.Text != "")
                    {
                        DateTime dateOVC_DOPEN = DateTime.Parse(txtOVC_DOPEN.Text).AddDays(-1);
                        DateTime dateOVC_DANNOUNCE = DateTime.Parse(txtOVC_DANNOUNCE.Text);
                        TimeSpan tsOVC_DWAIT = dateOVC_DOPEN - dateOVC_DANNOUNCE;
                        txtOVC_DWAIT.Text = tsOVC_DWAIT.Days.ToString();
                    }
                }
                SetOVC_DANNOUNCE_Visible();
            }
        }

        private void SetOVC_DANNOUNCE_Visible()
        {
            if (drpOVC_PUR_ASS_VEN_CODE.SelectedValue != null && drpOVC_PUR_ASS_VEN_CODE.SelectedValue != "")
            {
                string strOVC_PUR_ASS_VEN_CODE = drpOVC_PUR_ASS_VEN_CODE.SelectedItem.Text;
                bool isRESTRICT = strOVC_PUR_ASS_VEN_CODE.Contains("限制性招標");
                if (!isRESTRICT)
                {
                    lblOVC_DWAIT.Text = "系統自動計算：開標日前一日 - 公告日期(亦可自行修訂)";
                    lblOVC_BID_TIMES.Visible = true;
                    divOVC_DANNOUNCE.Visible = true;
                    lblOVC_DANNOUNCE.Visible = false;
                }
                else
                {
                    lblOVC_DWAIT.Text = "限制性招標本欄位請自行填入";
                    lblOVC_BID_TIMES.Visible = false;
                    divOVC_DANNOUNCE.Visible = false;
                    lblOVC_DANNOUNCE.Visible = true;
                }
            }
        }

        private void SetOVC_BID_MONEY()
        {
            string strOVC_BID_MONEY = drpOVC_BID_MONEY.SelectedValue;
            if (strOVC_BID_MONEY == "按廠商報價")
            {
                lblOVC_BID_MONEY_1.Text = "";
                txtOVC_BID_MONEY.Visible = true;
                lblOVC_BID_MONEY_2.Text = "%";
            }
            else if (strOVC_BID_MONEY == "定額")
            {
                lblOVC_BID_MONEY_1.Text = "：新台幣";
                txtOVC_BID_MONEY.Visible = true;
                lblOVC_BID_MONEY_2.Text = "元整";
            }
            else
            {
                lblOVC_BID_MONEY_1.Text = "";
                txtOVC_BID_MONEY.Visible = false;
                txtOVC_BID_MONEY.Text = "";
                lblOVC_BID_MONEY_2.Text = "";
            }
        }
        
        #endregion

        #endregion




        #region 輸出Word
        private string OutputWordD16_1_1()
        {
            //簽辦單
            var targetPath = Server.MapPath("~/WordPDFprint/");
            string fileName = strOVC_PURCH + "-簽辦單.docx";
            File.Delete(targetPath + fileName);
            File.Copy(targetPath + "D16_1_1-簽辦單.docx", targetPath + fileName);
            var valuesToFill = new TemplateEngine.Docx.Content();
            bool isRESTRICT;

            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                //招標方式==限制性招標?
                string strOVC_PUR_ASS_VEN_CODE = GetTbm1407Desc("C7", tbm1301.OVC_PUR_ASS_VEN_CODE == null ? "" : tbm1301.OVC_PUR_ASS_VEN_CODE);
                isRESTRICT = strOVC_PUR_ASS_VEN_CODE.Contains("限制性招標");

                TBMRECEIVE_BID tbmRECEIVE_BID = tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (tbmRECEIVE_BID != null)
                    strOVC_PURCH_5 = tbmRECEIVE_BID.OVC_PURCH_5;
                else
                    strOVC_PURCH_5 = "";
                valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IPURCH", tbm1301.OVC_PUR_IPURCH == null ? "" : tbm1301.OVC_PUR_IPURCH));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_APPROVE", tbm1301.OVC_PUR_APPROVE == null ? "" : tbm1301.OVC_PUR_APPROVE));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_ASS_VEN_CODE", GetTbm1407Desc("C7", tbm1301.OVC_PUR_ASS_VEN_CODE == null ? "" : tbm1301.OVC_PUR_ASS_VEN_CODE)));
                valuesToFill.Fields.Add(new FieldContent("ONB_PUR_BUDGET", tbm1301.ONB_PUR_BUDGET == null ? "" : tbm1301.ONB_PUR_BUDGET.ToString()));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_NSECTION", tbm1301.OVC_PUR_NSECTION == null ? "" : tbm1301.OVC_PUR_NSECTION));
                string strMoney, numMoney, strOVC_BID_MONEY = "";
                if (tbm1301.OVC_BID_MONEY != null)
                {
                    if (tbm1301.OVC_BID_MONEY.Contains("定額"))
                    {
                        numMoney = Regex.Replace(tbm1301.OVC_BID_MONEY, @"[^\d.\d]", "");
                        if (int.TryParse(numMoney, out int intMoney))
                            strMoney = String.Format("{0:n0}", intMoney);
                        else
                            strMoney = "";
                        strOVC_BID_MONEY = "定額：新台幣" + strMoney + "元整";
                    }
                    else
                        strOVC_BID_MONEY = tbm1301.OVC_BID_MONEY;
                }
                valuesToFill.Fields.Add(new FieldContent("OVC_BID_MONEY", strOVC_BID_MONEY));

                string strONB_PUR_BUDGET = tbm1301.ONB_PUR_BUDGET == null ? "" : string.Format("{0:N}", tbm1301.ONB_PUR_BUDGET);
                if (tbm1301.OVC_PURCH_KIND == "1")
                {
                    valuesToFill.Fields.Add(new FieldContent("ONB_PUR_BUDGET", "新台幣 " + strONB_PUR_BUDGET + "元整"));
                }
                else if (tbm1301.OVC_PURCH_KIND == "2")
                {
                    string strONB_PUR_BUDGET2 = GetTbm1407Desc("B0", tbm1301.OVC_PUR_CURRENT == null ? "" : tbm1301.OVC_PUR_CURRENT)
                        + " " + strONB_PUR_BUDGET;
                    valuesToFill.Fields.Add(new FieldContent("ONB_PUR_BUDGET", strONB_PUR_BUDGET2));
                }
                else
                {
                    valuesToFill.Fields.Add(new FieldContent("ONB_PUR_BUDGET", ""));
                }
                //if (int.TryParse(strONB_PUR_BUDGET, out n))
                TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
                tbmRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                        && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_ANNOUNCE != null)
                {
                    valuesToFill.Fields.Add(new FieldContent("OVC_BID_METHOD_1", tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_1 == null ? "" : tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_1));
                    valuesToFill.Fields.Add(new FieldContent("OVC_BID_METHOD_2", tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_2 == null ? "" : "," + tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_2));
                    valuesToFill.Fields.Add(new FieldContent("OVC_BID_METHOD_3", tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_3 == null ? "" : "," + tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_3));
                    if (int.TryParse(tbmRECEIVE_ANNOUNCE.OVC_BUDGET_BUY.ToString(), out int numONB_PUR_BUDGET) && tbmRECEIVE_ANNOUNCE.OVC_BUDGET_BUY != null)
                        valuesToFill.Fields.Add(new FieldContent("OVC_BUDGET_BUY", string.Format("{0:N}", numONB_PUR_BUDGET)));
                    else
                        valuesToFill.Fields.Add(new FieldContent("OVC_BUDGET_BUY", tbmRECEIVE_ANNOUNCE.OVC_BUDGET_BUY == null ? "" : tbmRECEIVE_ANNOUNCE.OVC_BUDGET_BUY));
                    valuesToFill.Fields.Add(new FieldContent("OVC_DANNOUNCE", tbmRECEIVE_ANNOUNCE.OVC_DANNOUNCE == null ? "" : GetTaiwanDate(tbmRECEIVE_ANNOUNCE.OVC_DANNOUNCE,false)));
                    valuesToFill.Fields.Add(new FieldContent("OVC_DOPEN", tbmRECEIVE_ANNOUNCE.OVC_DOPEN == null ? "" : GetTaiwanDate(tbmRECEIVE_ANNOUNCE.OVC_DOPEN, false)));
                    valuesToFill.Fields.Add(new FieldContent("OVC_OPEN_HOUR", tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR == null ? "" : tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR));
                    valuesToFill.Fields.Add(new FieldContent("OVC_DBID_LIMIT", tbmRECEIVE_ANNOUNCE.OVC_DBID_LIMIT == null ? "" : GetTaiwanDate(tbmRECEIVE_ANNOUNCE.OVC_DBID_LIMIT, false)));
                    valuesToFill.Fields.Add(new FieldContent("OVC_DRAFT_COMM", tbmRECEIVE_ANNOUNCE.OVC_DRAFT_COMM == null ? "" : tbmRECEIVE_ANNOUNCE.OVC_DRAFT_COMM.Replace("\r\n", "\n"))); //換行有問題
                    valuesToFill.Fields.Add(new FieldContent("OVC_PLACE", tbmRECEIVE_ANNOUNCE.OVC_PLACE == null ? "" : tbmRECEIVE_ANNOUNCE.OVC_PLACE));
                }


                TBMRECEIVE_WORK tbmRECEIVE_WORK = new TBMRECEIVE_WORK();
                tbmRECEIVE_WORK = mpms.TBMRECEIVE_WORK.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                        && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_WORK != null)
                {
                    string strOVC_CHAIRMAN = tbmRECEIVE_WORK.OVC_CHAIRMAN == null ? "" : tbmRECEIVE_WORK.OVC_CHAIRMAN;
                    if (strOVC_CHAIRMAN == "處長" || strOVC_CHAIRMAN == "副處長")
                        strOVC_CHAIRMAN += "(若不克主持，由OO代理)";
                    valuesToFill.Fields.Add(new FieldContent("OVC_CHAIRMAN", strOVC_CHAIRMAN));
                    valuesToFill.Fields.Add(new FieldContent("OVC_DWAIT", tbmRECEIVE_WORK.OVC_DWAIT == null ? "" : tbmRECEIVE_WORK.OVC_DWAIT));
                    valuesToFill.Fields.Add(new FieldContent("OVC_DESC", tbmRECEIVE_WORK.OVC_DESC == null ? "" : tbmRECEIVE_WORK.OVC_DESC));
                }
            }

            using (var outputDocument = new TemplateProcessor(targetPath + fileName).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }
            string filepath = targetPath + fileName;
            return filepath;
        }

        public string OutputWordD16_1_2()
        {
            //開標通知單(上呈版)
            var targetPath = Server.MapPath("~/WordPDFprint/");
            string fileName = strOVC_PURCH + "-開標通知單(上呈版).docx";
            File.Delete(targetPath + fileName);
            File.Copy(targetPath + "D16_1_2-開標通知單(上呈版).docx", targetPath + fileName);
            string strOVC_DO_NAME = "", strUserDept = "", strIUSER_PHONE = "";
            var valuesToFill = new TemplateEngine.Docx.Content();

            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                TBMRECEIVE_BID tbmRECEIVE_BID = tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (tbmRECEIVE_BID != null)
                {
                    strOVC_PURCH_5 = tbmRECEIVE_BID.OVC_PURCH_5;
                    strOVC_DO_NAME = tbmRECEIVE_BID.OVC_DO_NAME;
                }

                TBM1301_PLAN plan1301 = gm.TBM1301_PLAN.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (plan1301 != null)
                    strUserDept = plan1301.OVC_PURCHASE_UNIT;

                ACCOUNT ac = gm.ACCOUNTs.Where(tb => tb.DEPT_SN.Equals(strUserDept) && tb.USER_NAME.Equals(strOVC_DO_NAME)).FirstOrDefault();
                if (ac != null)
                    strIUSER_PHONE = ac.IUSER_PHONE;

                valuesToFill.Fields.Add(new FieldContent("OVC_DO_NAME", strOVC_DO_NAME));
                valuesToFill.Fields.Add(new FieldContent("IUSER_PHONE", strIUSER_PHONE));
                valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IPURCH", tbm1301.OVC_PUR_IPURCH == null ? "" : tbm1301.OVC_PUR_IPURCH));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_ASS_VEN_CODE", GetTbm1407Desc("C7", tbm1301.OVC_PUR_ASS_VEN_CODE == null ? "" : tbm1301.OVC_PUR_ASS_VEN_CODE)));
                valuesToFill.Fields.Add(new FieldContent("ONB_TIMES", numONB_TIMES.ToString()));

                if (tbm1301.OVC_PURCH_KIND == "1")
                {
                    valuesToFill.Fields.Add(new FieldContent("OVC_PUR_CURRENT", "新台幣"));
                    valuesToFill.Fields.Add(new FieldContent("ONB_PUR_BUDGET", tbm1301.ONB_PUR_BUDGET == null ? "" : string.Format("{0:N}", tbm1301.ONB_PUR_BUDGET)));
                }
                else if (tbm1301.OVC_PURCH_KIND == "2")
                {
                    valuesToFill.Fields.Add(new FieldContent("OVC_PUR_CURRENT", tbm1301.OVC_PUR_CURRENT == null ? "" : GetTbm1407Desc("B0", tbm1301.OVC_PUR_CURRENT)));
                    valuesToFill.Fields.Add(new FieldContent("ONB_PUR_BUDGET", tbm1301.ONB_PUR_BUDGET == null ? "" : string.Format("{0:N}", tbm1301.ONB_PUR_BUDGET)));
                }
            }

            int numOVC_OPEN_HOUR;
            TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
            tbmRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                    && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                    && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
            if (tbmRECEIVE_ANNOUNCE != null)
            {
                valuesToFill.Fields.Add(new FieldContent("OVC_DOPEN", GetTaiwanDate(strOVC_DOPEN, false)));
                if (tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR != null)
                {
                    numOVC_OPEN_HOUR = int.Parse(tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR);
                    if (numOVC_OPEN_HOUR >= 12)
                    {
                        valuesToFill.Fields.Add(new FieldContent("OVC_OPEN_HOUR_Type", "下"));
                        valuesToFill.Fields.Add(new FieldContent("OVC_OPEN_HOUR", (numOVC_OPEN_HOUR - 12).ToString()));
                    }
                    else if (numOVC_OPEN_HOUR < 12)
                    {
                        valuesToFill.Fields.Add(new FieldContent("OVC_OPEN_HOUR_Type", "上"));
                        valuesToFill.Fields.Add(new FieldContent("OVC_OPEN_HOUR", numOVC_OPEN_HOUR.ToString()));
                    }
                }
                else
                {
                    valuesToFill.Fields.Add(new FieldContent("OVC_OPEN_HOUR_Type", ""));
                    valuesToFill.Fields.Add(new FieldContent("OVC_OPEN_HOUR", ""));
                }
                valuesToFill.Fields.Add(new FieldContent("OVC_PLACE", tbmRECEIVE_ANNOUNCE.OVC_PLACE == null ? "" : tbmRECEIVE_ANNOUNCE.OVC_PLACE));

                using (var outputDocument = new TemplateProcessor(targetPath + fileName).SetRemoveContentControls(true))
                {
                    outputDocument.FillContent(valuesToFill);
                    outputDocument.SaveChanges();
                }
                string filepath = targetPath + fileName;
                return filepath;
            }
            else
                return "";
        }


        public string OutputWordD16_1_3()
        {
            //開標通知單(正式版)
            var targetPath = Server.MapPath("~/WordPDFprint/");
            string fileName = strOVC_PURCH + "-開標通知單(正式版).docx";
            File.Delete(targetPath + fileName);
            File.Copy(targetPath + "D16_1_3-開標通知單(正式版).docx", targetPath + fileName);
            var valuesToFill = new TemplateEngine.Docx.Content();
            string strOVC_DO_NAME = "", strUserDept = "", strIUSER_PHONE = "";

            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                TBMRECEIVE_BID tbmRECEIVE_BID = tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (tbmRECEIVE_BID != null)
                {
                    strOVC_PURCH_5 = tbmRECEIVE_BID.OVC_PURCH_5;
                    strOVC_DO_NAME = tbmRECEIVE_BID.OVC_DO_NAME;
                }
                TBM1301_PLAN plan1301 = gm.TBM1301_PLAN.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (plan1301 != null)
                    strUserDept = plan1301.OVC_PURCHASE_UNIT;

                ACCOUNT ac = gm.ACCOUNTs.Where(tb => tb.DEPT_SN.Equals(strUserDept) && tb.USER_NAME.Equals(strOVC_DO_NAME)).FirstOrDefault();
                if (ac != null)
                    strIUSER_PHONE = ac.IUSER_PHONE;

                valuesToFill.Fields.Add(new FieldContent("OVC_DO_NAME", strOVC_DO_NAME));
                valuesToFill.Fields.Add(new FieldContent("IUSER_PHONE", strIUSER_PHONE));
                valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IPURCH", tbm1301.OVC_PUR_IPURCH == null ? "" : tbm1301.OVC_PUR_IPURCH));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_ASS_VEN_CODE", GetTbm1407Desc("C7", tbm1301.OVC_PUR_ASS_VEN_CODE == null ? "" : tbm1301.OVC_PUR_ASS_VEN_CODE)));
                valuesToFill.Fields.Add(new FieldContent("ONB_TIMES", numONB_TIMES.ToString()));

                if (tbm1301.OVC_PURCH_KIND == "1")
                {
                    valuesToFill.Fields.Add(new FieldContent("OVC_PUR_CURRENT", "新台幣"));
                    valuesToFill.Fields.Add(new FieldContent("ONB_PUR_BUDGET", tbm1301.ONB_PUR_BUDGET == null ? "" : string.Format("{0:N}", tbm1301.ONB_PUR_BUDGET)));
                }
                else if (tbm1301.OVC_PURCH_KIND == "2")
                {
                    valuesToFill.Fields.Add(new FieldContent("OVC_PUR_CURRENT", tbm1301.OVC_PUR_CURRENT == null ? "" : GetTbm1407Desc("B0", tbm1301.OVC_PUR_CURRENT)));
                    valuesToFill.Fields.Add(new FieldContent("ONB_PUR_BUDGET", tbm1301.ONB_PUR_BUDGET == null ? "" : string.Format("{0:N}", tbm1301.ONB_PUR_BUDGET)));
                }


                int numOVC_OPEN_HOUR;
                TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
                tbmRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                        && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_ANNOUNCE != null)
                {
                    valuesToFill.Fields.Add(new FieldContent("OVC_DOPEN", GetTaiwanDate(strOVC_DOPEN, false)));
                    if (tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR != null)
                    {
                        numOVC_OPEN_HOUR = int.Parse(tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR);
                        if (numOVC_OPEN_HOUR >= 12)
                        {
                            valuesToFill.Fields.Add(new FieldContent("OVC_OPEN_HOUR_Type", "下"));
                            valuesToFill.Fields.Add(new FieldContent("OVC_OPEN_HOUR", (numOVC_OPEN_HOUR - 12).ToString()));
                        }
                        else if (numOVC_OPEN_HOUR < 12)
                        {
                            valuesToFill.Fields.Add(new FieldContent("OVC_OPEN_HOUR_Type", "上"));
                            valuesToFill.Fields.Add(new FieldContent("OVC_OPEN_HOUR", numOVC_OPEN_HOUR.ToString()));
                        }
                    }
                    else
                    {
                        valuesToFill.Fields.Add(new FieldContent("OVC_OPEN_HOUR_Type", ""));
                        valuesToFill.Fields.Add(new FieldContent("OVC_OPEN_HOUR", ""));
                    }
                    valuesToFill.Fields.Add(new FieldContent("OVC_PLACE", tbmRECEIVE_ANNOUNCE.OVC_PLACE == null ? "" : tbmRECEIVE_ANNOUNCE.OVC_PLACE));



                    using (var outputDocument = new TemplateProcessor(targetPath + fileName).SetRemoveContentControls(true))
                    {
                        outputDocument.FillContent(valuesToFill);
                        outputDocument.SaveChanges();
                    }
                    string filepath = targetPath + fileName;
                    return filepath;

                }
            }
            return "";
        }


        public string OutputWordD16_1_4()
        {
            //招標文件封面
            var targetPath = Server.MapPath("~/WordPDFprint/");
            string fileName = strOVC_PURCH + "-招標文件封面.docx";
            File.Delete(targetPath + fileName);
            File.Copy(targetPath + "D16_1_4-招標文件封面.docx", targetPath + fileName);
            var valuesToFill = new TemplateEngine.Docx.Content();

            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IPURCH", tbm1301.OVC_PUR_IPURCH == null ? "" : tbm1301.OVC_PUR_IPURCH));

                using (var outputDocument = new TemplateProcessor(targetPath + fileName).SetRemoveContentControls(true))
                {
                    outputDocument.FillContent(valuesToFill);
                    outputDocument.SaveChanges();
                }
                string filepath = targetPath + fileName;
                return filepath;
            }
            return "";
        }


        #endregion



    }
}
 