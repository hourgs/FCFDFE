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
namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D17_1 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strOVC_PURCH, strOVC_PUR_AGENCY, strOVC_PURCH_5;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (FCommon.getQueryString(this, "OVC_PURCH", out strOVC_PURCH, false))
                {
                    TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PUR_AGENCY = tbm1301?.OVC_PUR_AGENCY;

                    TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PURCH_5 = tbmRECEIVE_BID?.OVC_PURCH_5;

                    if (IsOVC_DO_NAME() && !IsPostBack)
                        DataImport();
                }
                else
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
            }
        }







        #region Button Click

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            //點擊 回上一頁
            string send_url = "~/pages/MPMS/D/MPMS_D14_1.aspx?OVC_PURCH=" + strOVC_PURCH;
            Response.Redirect(send_url);
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

        protected void btnD17_1_Click(object sender, EventArgs e)
        {

        }

        protected void btnD17_2_Click(object sender, EventArgs e)
        {

        }

        protected void btnD17_3_Click(object sender, EventArgs e)
        {

        }

        private void DataImport()
        {


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
            return string.Empty;
        }



        #endregion


        #region 輸出報表

        public void OutputWordD17_1()
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

        #endregion

    }

}
