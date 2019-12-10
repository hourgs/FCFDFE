using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MPMS.B
{
    public partial class MPMS_B12_1 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strPurchNum = "";
        string strToPurch = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!string.IsNullOrEmpty(Request.QueryString["OVC_PURCH"]))
                {
                    strPurchNum = Request.QueryString["OVC_PURCH"].ToString();
                    if (!strPurchNum.Equals(string.Empty))
                    {
                        if (!IsPostBack)
                        {
                            if (string.IsNullOrEmpty(txtOVC_PURCH.Text))
                            {
                                txtOVC_PURCH.Text = strPurchNum;
                                LoginScreen(strPurchNum);

                            }

                        }
                    }
                }
                if (!string.IsNullOrEmpty(Request.QueryString["strToPurch"]))
                {
                    strToPurch = Request.QueryString["strToPurch"].ToString();
                    txtNewOVC_PURCH.Text = strToPurch;
                }
            }
        }

        #region OnClick
        protected void btnReset_Click(object sender, EventArgs e)
        {
            //清除按鈕功能
            ClearAll();
        }

        protected void btnReQuery_Click(object sender, EventArgs e)
        {
            //重新尋找按鈕功能
            Response.Redirect("~/pages/MPMS/B/MPMS_B12");
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            //確認複製按鈕功能
            //複製原購案編號資料至新購案編號
            string strMessage = "";
            if (string.IsNullOrEmpty(txtOVC_PURCH.Text))
                strMessage += "<p>未輸入購案編號<p>";
            if (string.IsNullOrEmpty(txtNewOVC_PURCH.Text))
                strMessage += "<p>未輸入新購案編號<p>";
           
            if (string.IsNullOrEmpty(strMessage))
            {
                TBM1301 table1301 = new TBM1301();
                table1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                var table1301_PLAN = mpms.TBM1301_PLAN
                                    .Where(table => table.OVC_PURCH.Equals(txtNewOVC_PURCH.Text))
                                    .FirstOrDefault();
                if (table1301 != null)
                {
                    TBM1301 Newtable1301 = new TBM1301();
                    Newtable1301.OVC_PURCH = txtNewOVC_PURCH.Text;
                    Newtable1301.OVC_PLAN_PURCH = table1301.OVC_PLAN_PURCH;
                    Newtable1301.OVC_PUR_AGENCY = table1301_PLAN.OVC_PUR_AGENCY;
                    Newtable1301.OVC_PURCH_KIND = table1301.OVC_PURCH_KIND;
                    Newtable1301.OVC_LAB = table1301.OVC_LAB;
                    Newtable1301.OVC_PUR_ASS_VEN_CODE = table1301.OVC_PUR_ASS_VEN_CODE;
                    Newtable1301.OVC_BID_TIMES = table1301.OVC_BID_TIMES;
                    Newtable1301.OVC_BID = table1301.OVC_BID;
                    Newtable1301.OVC_PUR_NSECTION = table1301.OVC_PUR_NSECTION;
                    Newtable1301.OVC_PUR_SECTION = table1301.OVC_PUR_SECTION;
                    gm.TBM1301.Add(Newtable1301);
                    gm.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(),Request.ServerVariables["REMOTE_ADDR"].ToString(), Newtable1301.GetType().Name.ToString(),this,"新增");

                }

                var table1119 = mpms.TBM1119.Where(table => table.OVC_PURCH.Equals(strPurchNum));
                foreach (var item in table1119)
                {
                    TBM1119 Newtable1119 = new TBM1119();
                    Newtable1119.OVC_PURCH = txtNewOVC_PURCH.Text;
                    Newtable1119.OVC_IKIND = item.OVC_IKIND;
                    Newtable1119.OVC_ATTACH_NAME = item.OVC_ATTACH_NAME;
                    Newtable1119.ONB_QTY = item.ONB_QTY;
                    Newtable1119.ONB_PAGES = item.ONB_PAGES;
                    mpms.TBM1119.Add(Newtable1119);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), Newtable1119.GetType().Name.ToString(), this, "新增");
                }

                var table1201 = mpms.TBM1201.Where(table => table.OVC_PURCH.Equals(strPurchNum));
                foreach (var item in table1201)
                {
                    TBM1201 Newtable1201 = new TBM1201();
                    Newtable1201.OVC_PURCH = txtNewOVC_PURCH.Text;
                    Newtable1201.ONB_POI_ICOUNT = item.ONB_POI_ICOUNT;
                    Newtable1201.OVC_POI_NSTUFF_CHN = item.OVC_POI_NSTUFF_CHN;
                    Newtable1201.OVC_POI_NSTUFF_ENG = item.OVC_POI_NSTUFF_ENG;
                    Newtable1201.NSN_KIND = item.NSN_KIND;
                    Newtable1201.NSN = item.NSN;
                    Newtable1201.OVC_BRAND = item.OVC_BRAND;
                    Newtable1201.OVC_MODEL = item.OVC_MODEL;
                    Newtable1201.OVC_POI_IREF = item.OVC_POI_IREF;
                    Newtable1201.OVC_FCODE = item.OVC_FCODE;
                    Newtable1201.OVC_POI_IUNIT = item.OVC_POI_IUNIT;
                    Newtable1201.ONB_POI_QORDER_PLAN = item.ONB_POI_QORDER_PLAN;
                    Newtable1201.OVC_FIRST_BUY = item.OVC_FIRST_BUY;
                    Newtable1201.OVC_POI_IPURCH_BEF = item.OVC_POI_IPURCH_BEF;
                    Newtable1201.OVC_CURR_MPRICE_BEF = item.OVC_CURR_MPRICE_BEF;
                    Newtable1201.ONB_POI_QORDER_BEF = item.ONB_POI_QORDER_BEF;
                    Newtable1201.ONB_POI_MPRICE_BEF = item.ONB_POI_MPRICE_BEF;
                    Newtable1201.OVC_POI_NDESC = item.OVC_POI_NDESC;
                    Newtable1201.OVC_SAME_QUALITY = item.OVC_SAME_QUALITY;

                    mpms.TBM1201.Add(Newtable1201);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), Newtable1201.GetType().Name.ToString(), this, "新增");
                }

                var table1220_1 = mpms.TBM1220_1.Where(table => table.OVC_PURCH.Equals(strPurchNum));
                foreach (var item in table1220_1)
                {
                    TBM1220_1 Newtable1220_1 = new TBM1220_1();
                    Newtable1220_1.OVC_PURCH = txtNewOVC_PURCH.Text;
                    Newtable1220_1.OVC_IKIND = item.OVC_IKIND;
                    Newtable1220_1.ONB_NO = item.ONB_NO;
                    Newtable1220_1.OVC_CHECK = item.OVC_CHECK;
                    Newtable1220_1.OVC_MEMO = item.OVC_MEMO;
                    Newtable1220_1.OVC_STANDARD = item.OVC_STANDARD;

                    mpms.TBM1220_1.Add(Newtable1220_1);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), Newtable1220_1.GetType().Name.ToString(), this, "新增");
                }
                
                var table1233 = mpms.TBM1233.Where(table => table.OVC_PURCH.Equals(strPurchNum));
                foreach (var item in table1233)
                {
                    TBM1233 Newtable1233 = new TBM1233();
                    Newtable1233.OVC_PURCH = txtNewOVC_PURCH.Text;
                    Newtable1233.ONB_POI_ICOUNT = item.ONB_POI_ICOUNT;
                    Newtable1233.OVC_POI_NDESC = item.OVC_POI_NDESC;

                    mpms.TBM1233.Add(Newtable1233);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), Newtable1233.GetType().Name.ToString(), this, "新增");

                }

                TBMPURCH_EXT tabletbmpurch = new TBMPURCH_EXT();
                tabletbmpurch = mpms.TBMPURCH_EXT.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                if (tabletbmpurch != null)
                {
                    TBMPURCH_EXT Newtabletbmpurch = new TBMPURCH_EXT();
                    Newtabletbmpurch.OVC_PURCH = txtNewOVC_PURCH.Text;
                    Newtabletbmpurch.OVC_PUR_NSECTION_2 = tabletbmpurch.OVC_PUR_NSECTION_2;
                    Newtabletbmpurch.IS_OEM_CONTRACT = tabletbmpurch.IS_OEM_CONTRACT;
                    Newtabletbmpurch.OEM_EXAMPLE_SUPPORT = tabletbmpurch.OEM_EXAMPLE_SUPPORT;
                    Newtabletbmpurch.OVC_TARGET_KIND = tabletbmpurch.OVC_TARGET_KIND;
                    Newtabletbmpurch.OVC_PERFORMANCE_PLACE = tabletbmpurch.OVC_PERFORMANCE_PLACE;
                    Newtabletbmpurch.OVC_PERFORMANCE_LIMIT = tabletbmpurch.OVC_PERFORMANCE_LIMIT;
                    Newtabletbmpurch.OVC_VENDOR_DESC = tabletbmpurch.OVC_VENDOR_DESC;
                    mpms.TBMPURCH_EXT.Add(Newtabletbmpurch);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), Newtabletbmpurch.GetType().Name.ToString(), this, "新增");
                }
                FCommon.AlertShow(PnMessage, "success", "注意事項", "從：" + txtOVC_PURCH.Text + "到：" + txtNewOVC_PURCH.Text + "複製成功");
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            //查詢按鈕功能
            ClearAll();
            txtNewOVC_PURCH.Visible = true;
            btnConfirm.Visible = true;
            lblDescription.Visible = true;
            if (string.IsNullOrEmpty(txtOVC_PURCH.Text))
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "未輸入購案案號！");
            }
            else
            {

                LoginScreen(txtOVC_PURCH.Text);
            }
        }
        #endregion

        #region 副程式
        private void ClearAll()
        {
            FCommon.Controls_Clear(txtOVC_PURCH);
            lblOVC_PUR_AGENCY.Text = "";
            lblOVC_PUR_IPURCH.Text = "";
            lblOVC_PUR_NSECTION.Text = "";
            lblOVC_PUR_SECTION.Text = "";
            lblBrackets_1.Visible = false;
            lblBrackets_2.Visible = false;
            lblBrackets_3.Visible = false;
            lblBrackets_4.Visible = false;
            lblConnect.Visible = false;
            lblOVC_PUR_USER.Text = "";
            lblOVC_PUR_IUSER_PHONE_EXT.Text = "";
            txtNewOVC_PURCH.Visible = false;
            btnConfirm.Visible = false;
            lblDescription.Visible = false;
        }

        private void LoginScreen(string strPurchNum)
        {
            
            //初始頁面
            TBM1301 tb1301 = new TBM1301();
            tb1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            TBM1407 tb1407 = new TBM1407();
            tb1407 = gm.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("C2")).Where(table => table.OVC_PHR_ID.Equals(tb1301.OVC_PUR_AGENCY)).FirstOrDefault();
            if (tb1407 != null && tb1301 != null){
                lblOVC_PUR_AGENCY.Text = tb1407.OVC_PHR_DESC;
                lblOVC_PUR_IPURCH.Text = tb1301.OVC_PUR_IPURCH;
                lblOVC_PUR_NSECTION.Text = tb1301.OVC_PUR_NSECTION;
                lblOVC_PUR_SECTION.Text = tb1301.OVC_PUR_SECTION;
                lblOVC_PUR_USER.Text = tb1301.OVC_PUR_USER;
                lblOVC_PUR_IUSER_PHONE_EXT.Text = tb1301.OVC_PUR_IUSER_PHONE_EXT;
            }

            //賦予新購案編號
            //DateTime datetime = DateTime.Now;
            //CultureInfo culture = new CultureInfo("zh-TW");
            //culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            //int ThisYear = Convert.ToInt16(datetime.ToString("yyy", culture));
            //string YearPart = ThisYear.ToString();
            //string LastPurch = strPurchNum.Substring(0, 2) + YearPart.Substring(YearPart.Length - 2, 2);
            //string NewPurch = LastPurch + "001";
            //TBM1301 tb1301New = new TBM1301();
            //tb1301New = gm.TBM1301.Where(table => table.OVC_PURCH.StartsWith(LastPurch)).OrderByDescending(table => table.OVC_PURCH).FirstOrDefault();
            //if (tb1301New != null)
            //{
            //    int plus = int.Parse(tb1301New.OVC_PURCH.Substring(4,3)) + 1;
            //    txtNewOVC_PURCH.Text = LastPurch + plus.ToString("000");
            //}
            //else
            //{
            //    txtNewOVC_PURCH.Text = NewPurch;
            //}
        }
        #endregion
    }
}