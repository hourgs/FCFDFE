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
using Microsoft.International.Formatters;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D16_2 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strOVC_PURCH, strOVC_PURCH_5, strOVC_DOPEN;
        short numONB_TIMES;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            {
                //設置readonly屬性
                FCommon.Controls_Attributes("readonly", "true", txtOVC_DSEND, txtOVC_DOPEN_N, txtOVC_DANNOUNCE, txtOVC_DAPPROVE, txtOVC_AGNT_IN, txtOVC_PUR_USER, txtOVC_USER_CELLPHONE, txtOVC_MEMO);

                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_DOPEN"] == null || Request.QueryString["ONB_TIMES"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    strOVC_PURCH = Request.QueryString["OVC_PURCH"].ToString();
                    TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                    strOVC_DOPEN = Request.QueryString["OVC_DOPEN"].ToString();
                    short.TryParse(Request.QueryString["ONB_TIMES"].ToString(), out numONB_TIMES);
                    if (IsOVC_DO_NAME() && !IsPostBack)
                        DataImport();
                }
            }
        }

        protected void txtOVC_DANNOUNCE_TextChanged(object sender, EventArgs e)
        {
            txtOVC_MEMO.Text = "一、本案已於" + GetTaiwanDate(txtOVC_DANNOUNCE.Text) + "刊登政府採購公報。\r\n"
                             + "二、本案標單所附清單交貨時間無誤，已購買標單廠商，原公告及招標單首頁交貨時間，於本次公告後自行更正，不另更換。";
        }

        protected void drpOVC_ITEM_NAME_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOVC_DESC_MODIFY.Text += txtOVC_DESC_MODIFY.Text == "" ? "" : "\r\n";
            txtOVC_DESC_MODIFY.Text += drpOVC_ITEM_NAME.SelectedValue;
        }

        protected void drpOVC_DRAFT_COMM_SelectedIndexChanged(object sender, EventArgs e)
        {
            string saveOVC_DRAFT_COMM = "";
            string nowOVC_DRAFT_COMM = txtOVC_DRAFT_COMM.Text;
            string strSelect1 = "本案招標文件更正事宜，依招標期限標準第七條規定，於等標期截止前變更或補充招標文件內容者，應視需要延長等標期。";
            string strSelect2 = "本案招標單更正事宜，依招標期限標準第七條第2項規定，本變更非屬重大改變，於原定截止日前五日公告補充招標文件內容，免延長等標期。";
            if (nowOVC_DRAFT_COMM == "")
                saveOVC_DRAFT_COMM = drpOVC_DRAFT_COMM.SelectedValue;

            else if (!nowOVC_DRAFT_COMM.Contains(drpOVC_DRAFT_COMM.SelectedValue))
            {
                if (drpOVC_DRAFT_COMM.SelectedValue == strSelect1)
                {
                    if (nowOVC_DRAFT_COMM.Contains(strSelect2))
                    {
                        saveOVC_DRAFT_COMM = nowOVC_DRAFT_COMM.Insert(nowOVC_DRAFT_COMM.IndexOf(strSelect2), "\r\n" + strSelect1);
                    }
                    else
                        saveOVC_DRAFT_COMM = strSelect1 + "\r\n" + nowOVC_DRAFT_COMM;
                }
                else if (drpOVC_DRAFT_COMM.SelectedValue == strSelect2)
                {
                    if (nowOVC_DRAFT_COMM.Contains(strSelect2))
                    {
                        saveOVC_DRAFT_COMM = nowOVC_DRAFT_COMM.Insert(nowOVC_DRAFT_COMM.IndexOf(strSelect1), "\r\n" + strSelect2);
                    }
                    else
                        saveOVC_DRAFT_COMM = strSelect2 + "\r\n" + nowOVC_DRAFT_COMM;
                }
            }
            else if (nowOVC_DRAFT_COMM.Contains(drpOVC_DRAFT_COMM.SelectedValue))
            {
                saveOVC_DRAFT_COMM = nowOVC_DRAFT_COMM;
            }

            if (!nowOVC_DRAFT_COMM.Contains("依修正內容於"))
                saveOVC_DRAFT_COMM += "\r\n依修正內容於○○○年○月○日下午○○○○時前辦理前辦理公告更正作業，電子檔一併修正之。";

            if (!nowOVC_DRAFT_COMM.Contains("借用軍網、民網隨身碟各乙枚，辦理公告上傳事宜。"))
                saveOVC_DRAFT_COMM += "\r\n借用軍網、民網隨身碟各乙枚，辦理公告上傳事宜。";

            txtOVC_DRAFT_COMM.Text = saveOVC_DRAFT_COMM;
        }


        #region Button OnClick
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //存檔
            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                if (isSave())
                {
                    //OVC_DRAFT_COMM
                    TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
                    tbmRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                            && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                            && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                    if (tbmRECEIVE_ANNOUNCE != null)
                    {
                        tbmRECEIVE_ANNOUNCE.OVC_DRAFT_COMM = txtOVC_DRAFT_COMM.Text;
                    }


                    TBMANNOUNCE_MODIFY tbmANNOUNCE_MODIFY = new TBMANNOUNCE_MODIFY();
                    tbmANNOUNCE_MODIFY = mpms.TBMANNOUNCE_MODIFY.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                            && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)).FirstOrDefault();
                    if (tbmANNOUNCE_MODIFY != null && lblOVC_DWORK.Text == DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        //修改日期相同 =>直接修改
                        tbmANNOUNCE_MODIFY.OVC_DSEND = txtOVC_DSEND.Text;
                        tbmANNOUNCE_MODIFY.OVC_AGNT_IN = txtOVC_AGNT_IN_ID.Text;
                        tbmANNOUNCE_MODIFY.OVC_NAME = txtOVC_PUR_USER.Text;
                        tbmANNOUNCE_MODIFY.OVC_TELEPHONE = txtOVC_USER_CELLPHONE.Text;
                        tbmANNOUNCE_MODIFY.OVC_DESC_ORG = txtOVC_DESC_ORG.Text;
                        tbmANNOUNCE_MODIFY.OVC_DESC_MODIFY = txtOVC_DESC_MODIFY.Text;
                        tbmANNOUNCE_MODIFY.OVC_DOPEN_MODIFY = rdoOVC_DOPEN_MODIFY.SelectedValue;
                        tbmANNOUNCE_MODIFY.OVC_DOPEN = strOVC_DOPEN;
                        tbmANNOUNCE_MODIFY.OVC_OPEN_HOUR = lblOVC_OPEN_HOUR.Text;
                        tbmANNOUNCE_MODIFY.OVC_OPEN_MIN = lblOVC_OPEN_MIN.Text;
                        if (rdoOVC_DOPEN_MODIFY.SelectedValue == "Y")
                        {
                            tbmANNOUNCE_MODIFY.OVC_DOPEN_N = txtOVC_DOPEN_N.Text;
                            tbmANNOUNCE_MODIFY.OVC_OPEN_HOUR_N = txtOVC_OPEN_HOUR_N.Text;
                            tbmANNOUNCE_MODIFY.OVC_OPEN_MIN_N = txtOVC_OPEN_MIN_N.Text;
                        }
                        tbmANNOUNCE_MODIFY.OVC_DESC = txtOVC_DESC.Text;
                        tbmANNOUNCE_MODIFY.OVC_MEMO = txtOVC_MEMO.Text;
                        tbmANNOUNCE_MODIFY.OVC_DANNOUNCE = txtOVC_DANNOUNCE.Text;
                        tbmANNOUNCE_MODIFY.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;
                        tbmANNOUNCE_MODIFY.OVC_DWORK = DateTime.Now.ToString("yyyy-MM-dd");
                        mpms.SaveChanges();
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "採購公告修正紀錄檔 修改成功！");
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmANNOUNCE_MODIFY.GetType().Name.ToString(), this, "修改");

                    }

                    else
                    {
                        if (tbmANNOUNCE_MODIFY != null && lblOVC_DWORK.Text != DateTime.Now.ToString("yyyy-MM-dd"))
                        {
                            //修改日期不同 => 先刪除資料後新增一筆
                            mpms.Entry(tbmANNOUNCE_MODIFY).State = EntityState.Deleted;
                            mpms.SaveChanges();
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "採購公告修正紀錄檔 修改成功！");
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmANNOUNCE_MODIFY.GetType().Name.ToString(), this, "修改");
                        }
                        else
                        {
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "採購公告修正紀錄檔 新增成功！");
                            //FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmANNOUNCE_MODIFY.GetType().Name.ToString(), this, "新增"); //有問題
                        }
                        //新增
                        TBMANNOUNCE_MODIFY tbmANNOUNCE_MODIFY_New = new TBMANNOUNCE_MODIFY();
                        tbmANNOUNCE_MODIFY_New.OVC_PURCH = strOVC_PURCH;
                        tbmANNOUNCE_MODIFY_New.OVC_PURCH_5 = strOVC_PURCH_5;
                        tbmANNOUNCE_MODIFY_New.OVC_DWORK = DateTime.Now.ToString("yyyy-MM-dd");
                        tbmANNOUNCE_MODIFY_New.OVC_DSEND = txtOVC_DSEND.Text;
                        tbmANNOUNCE_MODIFY_New.OVC_AGNT_IN = txtOVC_AGNT_IN_ID.Text;
                        tbmANNOUNCE_MODIFY_New.OVC_NAME = txtOVC_PUR_USER.Text;
                        tbmANNOUNCE_MODIFY_New.OVC_TELEPHONE = txtOVC_USER_CELLPHONE.Text;
                        tbmANNOUNCE_MODIFY_New.OVC_DESC_ORG = txtOVC_DESC_ORG.Text;
                        tbmANNOUNCE_MODIFY_New.OVC_DESC_MODIFY = txtOVC_DESC_MODIFY.Text;
                        tbmANNOUNCE_MODIFY_New.OVC_DOPEN_MODIFY = rdoOVC_DOPEN_MODIFY.SelectedValue;
                        tbmANNOUNCE_MODIFY_New.OVC_DOPEN = strOVC_DOPEN;
                        tbmANNOUNCE_MODIFY_New.OVC_OPEN_HOUR = lblOVC_OPEN_HOUR.Text;
                        tbmANNOUNCE_MODIFY_New.OVC_OPEN_MIN = lblOVC_OPEN_MIN.Text;
                        if (rdoOVC_DOPEN_MODIFY.SelectedValue == "Y")
                        {
                            tbmANNOUNCE_MODIFY_New.OVC_DOPEN_N = txtOVC_DOPEN_N.Text;
                            tbmANNOUNCE_MODIFY_New.OVC_OPEN_HOUR_N = txtOVC_OPEN_HOUR_N.Text;
                            tbmANNOUNCE_MODIFY_New.OVC_OPEN_MIN_N = txtOVC_OPEN_MIN_N.Text;
                        }
                        tbmANNOUNCE_MODIFY_New.OVC_DESC = txtOVC_DESC.Text;
                        tbmANNOUNCE_MODIFY_New.OVC_MEMO = txtOVC_MEMO.Text;
                        tbmANNOUNCE_MODIFY_New.OVC_DANNOUNCE = txtOVC_DANNOUNCE.Text;
                        tbmANNOUNCE_MODIFY_New.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;

                        mpms.TBMANNOUNCE_MODIFY.Add(tbmANNOUNCE_MODIFY_New);
                        mpms.SaveChanges();
                    }
                }
            }
        }
        protected void btnReturnC_Click(object sender, EventArgs e)
        {
            //回招標選擇畫面
            string send_url = "~/pages/MPMS/D/MPMS_D16.aspx?OVC_PURCH=" + strOVC_PURCH;
            Response.Redirect(send_url);
        }

        protected void btnReturnP_Click(object sender, EventArgs e)
        {
            //回招標作業編輯
            string send_url = "~/pages/MPMS/D/MPMS_D16_1.aspx?OVC_PURCH=" + strOVC_PURCH
                            + "&OVC_DOPEN=" + strOVC_DOPEN + "&ONB_TIMES=" + numONB_TIMES;
            Response.Redirect(send_url);
        }

        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            //回主流程畫面
            string send_url = "~/pages/MPMS/D/MPMS_D14_1.aspx?OVC_PURCH=" + strOVC_PURCH;
            Response.Redirect(send_url);
        }

        #region 產出修正公告稿
        protected void lbtnToWordD16_2_Click(object sender, EventArgs e)
        {
            string filepath = GetWordD16_2();
            string TempName = strOVC_PURCH + "-修正公告稿.docx";
            FileInfo file = new FileInfo(filepath);
            string wordPath = filepath;
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + TempName);
            Response.ContentType = "application/octet-stream";
            //Response.BinaryWrite(buffer);
            Response.WriteFile(filepath);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            File.Delete(filepath);
            File.Delete(wordPath);
            Response.End();
        }
        protected void lbtnToWordD16_2_pdf_Click(object sender, EventArgs e)
        {
            string filepath = GetWordD16_2();
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/WordD16_2_pdf.pdf";
            string FileName = strOVC_PURCH + "-修正公告稿.pdf";
            FCommon.WordToPDF(this, filepath, filetemp, FileName);
        }
        protected void lbtnToWordD16_2_odt_Click(object sender, EventArgs e)
        {
            string filepath = GetWordD16_2();
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/WordD16_2_odt.odt";
            string FileName = strOVC_PURCH + "-修正公告稿.odt";
            FCommon.WordToOdt(this, filepath, filetemp, FileName);
        }
        #endregion
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
            //帶入資料
            string strOVC_PUR_IPURCH = "0";
            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                TBM1201 tbm1201 = new TBM1201();
                tbm1201 = mpms.TBM1201.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).OrderByDescending(tb => tb.ONB_POI_ICOUNT).FirstOrDefault();
                if (tbm1201 != null)
                    strOVC_PUR_IPURCH = tbm1201.ONB_POI_ICOUNT.ToString();
                lblOVC_PURCH_A_5.Text = tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                lblOVC_PURCH.Text = tbm1301.OVC_PURCH;
                lblOVC_PUR_AGENCY.Text = tbm1301.OVC_PUR_AGENCY;
                lblOVC_PURCH_5.Text = strOVC_PURCH_5;
                lblOVC_PUR_IPURCH.Text = tbm1301.OVC_PUR_IPURCH + "等計" + strOVC_PUR_IPURCH + "項";
                lblOVC_BID_TIMES.Text = GetTbm1407Desc("TG", tbm1301.OVC_BID_TIMES);
                txtOVC_AGNT_IN_ID.Text = tbm1301.OVC_AGNT_IN;
                txtOVC_AGNT_IN.Text = GetTbm1407Desc("GK", tbm1301.OVC_AGNT_IN);
                txtOVC_PUR_USER.Text = tbm1301.OVC_PUR_USER;
                txtOVC_USER_CELLPHONE.Text = tbm1301.OVC_USER_CELLPHONE;



                TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
                tbmRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                        && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_ANNOUNCE != null)
                {
                    lblOVC_DOPEN.Text = GetTaiwanDate(tbmRECEIVE_ANNOUNCE.OVC_DOPEN);
                    lblOVC_OPEN_HOUR.Text = tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR;
                    lblOVC_OPEN_MIN.Text = tbmRECEIVE_ANNOUNCE.OVC_OPEN_MIN;
                    lblOVC_DOPEN_input.Text = GetTaiwanDate(tbmRECEIVE_ANNOUNCE.OVC_DOPEN) + " " +
                                                tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR + "時" + tbmRECEIVE_ANNOUNCE.OVC_OPEN_MIN + "分";
                    //txtOVC_DRAFT_COMM.Text = tbmRECEIVE_ANNOUNCE.OVC_DRAFT_COMM;


                    TBMANNOUNCE_MODIFY tbmANNOUNCE_MODIFY = new TBMANNOUNCE_MODIFY();
                    tbmANNOUNCE_MODIFY = mpms.TBMANNOUNCE_MODIFY.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                            && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)).FirstOrDefault();
                    if (tbmANNOUNCE_MODIFY != null)
                    {
                        lblOVC_DWORK.Text = tbmANNOUNCE_MODIFY.OVC_DWORK;
                        txtOVC_DSEND.Text = tbmANNOUNCE_MODIFY.OVC_DSEND;
                        txtOVC_DESC_ORG.Text = tbmANNOUNCE_MODIFY.OVC_DESC_ORG;
                        txtOVC_DESC_MODIFY.Text = tbmANNOUNCE_MODIFY.OVC_DESC_MODIFY;
                        rdoOVC_DOPEN_MODIFY.SelectedValue = tbmANNOUNCE_MODIFY.OVC_DOPEN_MODIFY;
                        if (tbmANNOUNCE_MODIFY.OVC_DOPEN_MODIFY == "Y")
                        {
                            txtOVC_DOPEN_N.Text = tbmANNOUNCE_MODIFY.OVC_DOPEN_N;
                            txtOVC_OPEN_HOUR_N.Text = tbmANNOUNCE_MODIFY.OVC_OPEN_HOUR_N;
                            txtOVC_OPEN_MIN_N.Text = tbmANNOUNCE_MODIFY.OVC_OPEN_MIN_N;
                        }
                        txtOVC_DESC.Text = tbmANNOUNCE_MODIFY.OVC_DESC;
                        txtOVC_MEMO.Text = tbmANNOUNCE_MODIFY.OVC_MEMO;
                        txtOVC_DANNOUNCE.Text = tbmANNOUNCE_MODIFY.OVC_DANNOUNCE;
                        txtOVC_DAPPROVE.Text = tbmANNOUNCE_MODIFY.OVC_DAPPROVE;
                    }
                }
                /*
                string strItem1 = "原截標時間(○○○年○月○日○○○○時)延至○○○年○月○日○○○○時，原開標時間(" 
                                  + lblOVC_DOPEN.Text + lblOVC_OPEN_HOUR.Text + lblOVC_OPEN_MIN.Text + "時)延至" 
                                  + GetTaiwanDate(txtOVC_DOPEN_N.Text) + txtOVC_OPEN_HOUR_N.Text + txtOVC_OPEN_MIN_N + "時。";
                string strItem2 = "對招標文件內容有疑義之日期，自○○○年○月○日前，延至○○○年○月○日前以書面向本單位提出；" +
                                  "對招標文件內容有異議之日期，自○○○年○月○日前，延至○○○年○月○日前以書面向本單位提出。";
                System.Web.UI.WebControls.ListItem item1 = new System.Web.UI.WebControls.ListItem(strItem1, strItem1, true);
                System.Web.UI.WebControls.ListItem item2 = new System.Web.UI.WebControls.ListItem(strItem2, strItem2, true);
                drpOVC_ITEM_NAME.Items.Add(item1);
                drpOVC_ITEM_NAME.Items.Add(item2);
                */
            }
        }



        private bool isSave()
        {
            string strErrorMsg = "";

            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 == null)
                strErrorMsg += "<p>查無此購案</p>";

            TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
            tbmRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                    && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                    && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
            if (tbmRECEIVE_ANNOUNCE == null)
                strErrorMsg += "<p>查無此公告</p>";

            //公告日期不可空白
            if (txtOVC_DANNOUNCE.Text == "")
                strErrorMsg += "<p>請輸入公告日期</p>";

            //主官核批日不可空白
            if (txtOVC_DAPPROVE.Text == "")
                strErrorMsg += "<p>請輸入主官核批日</p>";


            if (strErrorMsg == "")
                return true;
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErrorMsg);
                return false;
            }
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


        #region 輸出Word
        private string GetWordD16_2()
        {
            DataTable dt = (DataTable)ViewState["dt"];
            //這邊要在修改成中文日期
            string today = DateTime.Now.ToString();
            string strDateFormatTaiwan = "{0} 年 {1} 月 {2} 日";
            string strday = FCommon.getTaiwanDate_Assign((today), strDateFormatTaiwan, 2);
            string outPutfilePath = "";
            string fileName = "D16_2-修正公告稿.docx";
            string TempName = "";
            string filePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + fileName));

            TempName = "15-2." + strOVC_PURCH + "-修正公告稿.docx";
            outPutfilePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + TempName));
            File.Delete(outPutfilePath);
            File.Copy(filePath, outPutfilePath);
            var valuesToFill = new TemplateEngine.Docx.Content();

            valuesToFill.Fields.Add(new FieldContent("today", strday));
            valuesToFill.Fields.Add(new FieldContent("pno", lblOVC_PURCH_A_5.Text));
            valuesToFill.Fields.Add(new FieldContent("lblOVC_PUR_IPURCH", lblOVC_PUR_IPURCH.Text));
            valuesToFill.Fields.Add(new FieldContent("lblOVC_BID_TIMES", lblOVC_BID_TIMES.Text));
            //valuesToFill.Fields.Add(new FieldContent("txtOVC_PUR_USER", txtOVC_PUR_USER.Text));
            //valuesToFill.Fields.Add(new FieldContent("txtOVC_USER_CELLPHONE", txtOVC_USER_CELLPHONE.Text));
            valuesToFill.Fields.Add(new FieldContent("lblOVC_DO_NAME", txtOVC_PUR_USER.Text));
            valuesToFill.Fields.Add(new FieldContent("lblIUSER_PHONE", txtOVC_USER_CELLPHONE.Text));

            TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
            tbmRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                    && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                    && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
            if (tbmRECEIVE_ANNOUNCE.OVC_DRAFT_COMM != null)
            {
                if ((tbmRECEIVE_ANNOUNCE.OVC_DRAFT_COMM).Trim().Length != 0)
                {
                    string strOVC_DRAFT_COMM = "";
                    int item = 1;
                    string[] stringSeparators = new string[] { "\r\n" };
                    string[] strOVC_DRAFT_COMMs = (tbmRECEIVE_ANNOUNCE.OVC_DRAFT_COMM).Split(stringSeparators, StringSplitOptions.None);
                    foreach (string str in strOVC_DRAFT_COMMs)
                    {
                        string strItem = EastAsiaNumericFormatter.FormatWithCulture("Ln", item, null, new CultureInfo("zh-tw"));
                        strOVC_DRAFT_COMM += (strItem + "、" + str + "\r\n");
                        item++;
                    }
                    valuesToFill.Fields.Add(new FieldContent("OVC_DRAFT_COMM", strOVC_DRAFT_COMM));
                }
            }

            TBMANNOUNCE_MODIFY tbmANNOUNCE_MODIFY = new TBMANNOUNCE_MODIFY();
            tbmANNOUNCE_MODIFY = mpms.TBMANNOUNCE_MODIFY.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                    && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)).FirstOrDefault();
            if (tbmANNOUNCE_MODIFY != null)
            {
                if (tbmANNOUNCE_MODIFY.OVC_DESC_MODIFY != null)
                {
                    string strOVC_DESC_MODIFY = "";
                    int item = 1;
                    string[] stringSeparators = new string[] { "\r\n" };
                    string[] strOVC_DESC_MODIFYs = (tbmANNOUNCE_MODIFY.OVC_DESC_MODIFY).Split(stringSeparators, StringSplitOptions.None);
                    foreach (string str in strOVC_DESC_MODIFYs)
                    {
                        strOVC_DESC_MODIFY += (item + "." + str + "\r\n");
                        item++;
                    }
                    valuesToFill.Fields.Add(new FieldContent("OVC_DESC_MODIFY", strOVC_DESC_MODIFY));
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

        #endregion

    }
}