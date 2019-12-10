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

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D19_2 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string strOVC_PURCH, strOVC_PUR_AGENCY, strOVC_PURCH_5, strOVC_DOPEN, strOVC_VEN_CST;

        short numONB_TIMES, numONB_GROUP;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);

                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_DOPEN"] == null || Request.QueryString["ONB_TIMES"] == null || Request.QueryString["ONB_GROUP"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    strOVC_PURCH = Request.QueryString["OVC_PURCH"].ToString();
                    strOVC_DOPEN = Request.QueryString["OVC_DOPEN"].ToString();
                    strOVC_VEN_CST = Request.QueryString["OVC_VEN_CST"] == null ? "" : Request.QueryString["OVC_VEN_CST"].ToString();
                    short.TryParse(Request.QueryString["ONB_TIMES"].ToString(), out numONB_TIMES);
                    short.TryParse(Request.QueryString["ONB_GROUP"].ToString(), out numONB_GROUP);
                    TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;

                    TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY;

                    if (IsOVC_DO_NAME() && !IsPostBack)
                    {
                        DataImport();
                    }
                }
            }

        }


        #region Button Click

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //點擊 存檔
            SaveTbm1314();
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            //點擊 回上一頁
            string send_url = "~/pages/MPMS/D/MPMS_D19_1.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_DOPEN=" + strOVC_DOPEN +
                                    "&ONB_TIMES=" + numONB_TIMES + "&ONB_GROUP=" + numONB_GROUP;
            Response.Redirect(send_url);
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
            TBM1314 tbm1314 = new TBM1314();
            tbm1314 = mpms.TBM1314.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP
                                && tbm1314.OVC_VEN_CST == strOVC_VEN_CST).FirstOrDefault();
            if (tbm1314 != null)
            {
                rdoOVC_MINIS_4.SelectedValue = tbm1314.OVC_MINIS_4;
                txtONB_MINIS_4.Text = tbm1314.ONB_MINIS_4.ToString();
                rdoOVC_KMINIS_4.SelectedValue = tbm1314.OVC_KMINIS_4;
                rdoOVC_MINIS_5.SelectedValue = tbm1314.OVC_MINIS_5;
                txtONB_MINIS_5.Text = tbm1314.ONB_MINIS_5.ToString();
                rdoOVC_KMINIS_5.SelectedValue = tbm1314.OVC_KMINIS_5;
                rdoOVC_MINIS_6.SelectedValue = tbm1314.OVC_MINIS_6;
                txtONB_MINIS_6.Text = tbm1314.ONB_MINIS_6.ToString();
                rdoOVC_KMINIS_6.SelectedValue = tbm1314.OVC_KMINIS_6;
                rdoOVC_MINIS_7.SelectedValue = tbm1314.OVC_MINIS_7;
                txtONB_MINIS_7.Text = tbm1314.ONB_MINIS_7.ToString();
                rdoOVC_KMINIS_7.SelectedValue = tbm1314.OVC_KMINIS_7;
                rdoOVC_MINIS_8.SelectedValue = tbm1314.OVC_MINIS_8;
                txtONB_MINIS_8.Text = tbm1314.ONB_MINIS_8.ToString();
                rdoOVC_KMINIS_8.SelectedValue = tbm1314.OVC_KMINIS_8;
                rdoOVC_MINIS_9.SelectedValue = tbm1314.OVC_MINIS_9;
                txtONB_MINIS_9.Text = tbm1314.ONB_MINIS_9.ToString();
                rdoOVC_KMINIS_9.SelectedValue = tbm1314.OVC_KMINIS_9;
                rdoOVC_MINIS_10.SelectedValue = tbm1314.OVC_MINIS_10;
                txtONB_MINIS_10.Text = tbm1314.ONB_MINIS_10.ToString();
                rdoOVC_KMINIS_10.SelectedValue = tbm1314.OVC_KMINIS_10;
                rdoOVC_MINIS_11.SelectedValue = tbm1314.OVC_MINIS_11;
                txtONB_MINIS_11.Text = tbm1314.ONB_MINIS_11.ToString();
                rdoOVC_KMINIS_11.SelectedValue = tbm1314.OVC_KMINIS_11;
                rdoOVC_MINIS_12.SelectedValue = tbm1314.OVC_MINIS_12;
                txtONB_MINIS_12.Text = tbm1314.ONB_MINIS_12.ToString();
                rdoOVC_KMINIS_12.SelectedValue = tbm1314.OVC_KMINIS_12;
            }

        }


        private void SaveTbm1314()
        {
            TBM1314 tbm1314 = new TBM1314();
            tbm1314 = mpms.TBM1314.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP
                                && tbm1314.OVC_VEN_CST == strOVC_VEN_CST).FirstOrDefault();
            if (tbm1314 != null)
            {
                //修改
                tbm1314.OVC_MINIS_4 = rdoOVC_MINIS_4.SelectedValue;
                tbm1314.ONB_MINIS_4 = txtONB_MINIS_4.Text =="" ? (decimal?)null : decimal.Parse(txtONB_MINIS_4.Text);
                tbm1314.OVC_KMINIS_4 = rdoOVC_KMINIS_4.SelectedValue;
                tbm1314.OVC_MINIS_5 = rdoOVC_MINIS_5.SelectedValue;
                tbm1314.ONB_MINIS_5 = txtONB_MINIS_5.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_5.Text);
                tbm1314.OVC_KMINIS_5 = rdoOVC_KMINIS_5.SelectedValue;
                tbm1314.OVC_MINIS_6 = rdoOVC_MINIS_6.SelectedValue;
                tbm1314.ONB_MINIS_6 = txtONB_MINIS_6.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_6.Text);
                tbm1314.OVC_KMINIS_6 = rdoOVC_KMINIS_6.SelectedValue;
                tbm1314.OVC_MINIS_7 = rdoOVC_MINIS_7.SelectedValue;
                tbm1314.ONB_MINIS_7 = txtONB_MINIS_7.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_7.Text);
                tbm1314.OVC_KMINIS_7 = rdoOVC_KMINIS_7.SelectedValue;
                tbm1314.OVC_MINIS_8 = rdoOVC_MINIS_8.SelectedValue;
                tbm1314.ONB_MINIS_8 = txtONB_MINIS_8.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_8.Text);
                tbm1314.OVC_KMINIS_8 = rdoOVC_KMINIS_8.SelectedValue;
                tbm1314.OVC_MINIS_9 = rdoOVC_MINIS_9.SelectedValue;
                tbm1314.ONB_MINIS_9 = txtONB_MINIS_9.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_9.Text);
                tbm1314.OVC_KMINIS_9 = rdoOVC_KMINIS_9.SelectedValue;
                tbm1314.OVC_MINIS_10 = rdoOVC_MINIS_10.SelectedValue;
                tbm1314.ONB_MINIS_10 = txtONB_MINIS_10.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_10.Text);
                tbm1314.OVC_KMINIS_10 = rdoOVC_KMINIS_10.SelectedValue;
                tbm1314.OVC_MINIS_11 = rdoOVC_MINIS_11.SelectedValue;
                tbm1314.ONB_MINIS_11 = txtONB_MINIS_11.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_11.Text);
                tbm1314.OVC_KMINIS_11 = rdoOVC_KMINIS_11.SelectedValue;
                tbm1314.OVC_MINIS_12 = rdoOVC_MINIS_12.SelectedValue;
                tbm1314.ONB_MINIS_12 = txtONB_MINIS_12.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_12.Text);
                tbm1314.OVC_KMINIS_12 = rdoOVC_KMINIS_12.SelectedValue;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1314.GetType().Name.ToString(), this, "修改");
            }

            else
            {
                //新增
                TBM1314 tbm1314_new = new TBM1314();
                tbm1314_new.OVC_PURCH = strOVC_PURCH;
                tbm1314_new.OVC_PURCH_5 = strOVC_PURCH_5;
                tbm1314_new.OVC_DOPEN = strOVC_DOPEN;
                tbm1314.ONB_GROUP = numONB_GROUP;
                tbm1314_new.OVC_VEN_CST = strOVC_VEN_CST;
                tbm1314_new.OVC_MINIS_4 = rdoOVC_MINIS_4.SelectedValue;
                tbm1314_new.ONB_MINIS_4 = txtONB_MINIS_4.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_4.Text);
                tbm1314_new.OVC_KMINIS_4 = rdoOVC_KMINIS_4.SelectedValue;
                tbm1314_new.OVC_MINIS_5 = rdoOVC_MINIS_5.SelectedValue;
                tbm1314_new.ONB_MINIS_5 = txtONB_MINIS_5.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_5.Text);
                tbm1314_new.OVC_KMINIS_5 = rdoOVC_KMINIS_5.SelectedValue;
                tbm1314_new.OVC_MINIS_6 = rdoOVC_MINIS_6.SelectedValue;
                tbm1314_new.ONB_MINIS_6 = txtONB_MINIS_6.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_6.Text);
                tbm1314_new.OVC_KMINIS_6 = rdoOVC_KMINIS_6.SelectedValue;
                tbm1314_new.OVC_MINIS_7 = rdoOVC_MINIS_7.SelectedValue;
                tbm1314_new.ONB_MINIS_7 = txtONB_MINIS_7.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_7.Text);
                tbm1314_new.OVC_KMINIS_7 = rdoOVC_KMINIS_7.SelectedValue;
                tbm1314_new.OVC_MINIS_8 = rdoOVC_MINIS_8.SelectedValue;
                tbm1314_new.ONB_MINIS_8 = txtONB_MINIS_8.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_8.Text);
                tbm1314_new.OVC_KMINIS_8 = rdoOVC_KMINIS_8.SelectedValue;
                tbm1314_new.OVC_MINIS_9 = rdoOVC_MINIS_9.SelectedValue;
                tbm1314_new.ONB_MINIS_9 = txtONB_MINIS_9.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_9.Text);
                tbm1314_new.OVC_KMINIS_9 = rdoOVC_KMINIS_9.SelectedValue;
                tbm1314_new.OVC_MINIS_10 = rdoOVC_MINIS_10.SelectedValue;
                tbm1314_new.ONB_MINIS_10 = txtONB_MINIS_10.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_10.Text);
                tbm1314_new.OVC_KMINIS_10 = rdoOVC_KMINIS_10.SelectedValue;
                tbm1314_new.OVC_MINIS_11 = rdoOVC_MINIS_11.SelectedValue;
                tbm1314_new.ONB_MINIS_11 = txtONB_MINIS_11.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_11.Text);
                tbm1314_new.OVC_KMINIS_11 = rdoOVC_KMINIS_11.SelectedValue;
                tbm1314_new.OVC_MINIS_12 = rdoOVC_MINIS_12.SelectedValue;
                tbm1314_new.ONB_MINIS_12 = txtONB_MINIS_12.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_12.Text);
                tbm1314_new.OVC_KMINIS_12 = rdoOVC_KMINIS_12.SelectedValue;
                mpms.TBM1314.Add(tbm1314_new);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1314_new.GetType().Name.ToString(), this, "新增");
            }
        }


        #endregion
    }
}