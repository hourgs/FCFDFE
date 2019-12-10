using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;

namespace FCFDFE.pages.MPMS.B
{
    public partial class MPMS_B18 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
        }

        #region onClick
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                //查詢按鈕功能
                string strMessage = "";
                string txtPurch = txtOVC_PURCH.Text;

                var query = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(txtPurch));
                if (query.Any())
                {
                    LoginScreen(txtPurch);
                    trDel.Style.Remove("display");
                }
                else
                {
                    strMessage += "<p>本購案編號：" + txtPurch + " 不存在，請重新輸入！</p>";
                    ClearControl();
                    trDel.Style.Add("display", "none");
                    FCommon.AlertShow(PnMessage, "danger", "注意事項", strMessage);
                }
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            //清除按鈕功能
            ClearControl();
        }
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            //回主畫面按鈕功能
            Response.Redirect("MPMS_B11");
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            
            string userName = Session["username"].ToString();
            string strPurchNum = txtOVC_PURCH.Text;
            var query1301 = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum));
            
            var item = query1301.FirstOrDefault();
            if (string.IsNullOrEmpty(item.OVC_PERMISSION_UPDATE) || item.Equals("Y"))
            {
                TBM1301D tbm1301D = new TBM1301D();
                tbm1301D.OVC_PURCH = strPurchNum;
                tbm1301D.OVC_KIND = "D";
                tbm1301D.OVC_USER = userName;
                tbm1301D.OVC_DDATE = DateTime.Now.ToString();
                tbm1301D.OVC_D_SN = Guid.NewGuid();
                mpms.TBM1301D.Add(tbm1301D);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , tbm1301D.GetType().Name.ToString(), this, "新增");

                gm.TBM1301.RemoveRange(query1301);
                gm.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , query1301.GetType().Name.ToString(), this, "刪除");

                var query1231 = mpms.TBM1231.Where(o => o.OVC_PURCH.Equals(strPurchNum));
                if (query1231.Any())
                {
                    mpms.TBM1231.RemoveRange(query1231);
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , query1231.GetType().Name.ToString(), this, "刪除");
                }


                var query1118 = mpms.TBM1118.Where(o => o.OVC_PURCH.Equals(strPurchNum));
                if (query1118.Any())
                {
                    mpms.TBM1118.RemoveRange(query1118);
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                       , query1118.GetType().Name.ToString(), this, "刪除");
                }


                var query1118_1 = mpms.TBM1118_1.Where(o => o.OVC_PURCH.Equals(strPurchNum));
                if (query1118_1.Any())
                {
                    mpms.TBM1118_1.RemoveRange(query1118_1);
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                       , query1118_1.GetType().Name.ToString(), this, "刪除");
                }

                var query1118_2 = mpms.TBM1118_2.Where(o => o.OVC_PURCH.Equals(strPurchNum));
                if (query1118_2.Any())
                {
                    mpms.TBM1118_2.RemoveRange(query1118_2);
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                       , query1118_2.GetType().Name.ToString(), this, "刪除");
                }
                    

                var query1201 = mpms.TBM1201.Where(o => o.OVC_PURCH.Equals(strPurchNum));
                if (query1201.Any())
                {
                    mpms.TBM1201.RemoveRange(query1201);
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                       , query1201.GetType().Name.ToString(), this, "刪除");
                }
                    

                var query1233 = mpms.TBM1233.Where(o => o.OVC_PURCH.Equals(strPurchNum));
                if (query1233.Any())
                {
                    mpms.TBM1233.RemoveRange(query1233);
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                       , query1233.GetType().Name.ToString(), this, "刪除");
                }
                    

                var query1220_1 = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum));
                if (query1220_1.Any())
                {
                    mpms.TBM1220_1.RemoveRange(query1220_1);
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                       , query1220_1.GetType().Name.ToString(), this, "刪除");
                }

                var query1119 = mpms.TBM1119.Where(o => o.OVC_PURCH.Equals(strPurchNum));
                if (query1119.Any())
                {
                    mpms.TBM1119.RemoveRange(query1119);
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                       , query1119.GetType().Name.ToString(), this, "刪除");
                }
                    

                var queryPURCH_EXT = mpms.TBMPURCH_EXT.Where(o => o.OVC_PURCH.Equals(strPurchNum));
                if (queryPURCH_EXT.Any())
                {
                    mpms.TBMPURCH_EXT.RemoveRange(queryPURCH_EXT);
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                       , queryPURCH_EXT.GetType().Name.ToString(), this, "刪除");
                }
                    
                

                mpms.SaveChanges();
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "本案由其他單位管制中不能刪除購案");
            }
            
        }
        #endregion

        #region 副程式
        private void LoginScreen(string PurchNum)
        {
            //資料載入
            var query =
                (from t in gm.TBM1301
                 join t2 in gm.TBM1407 on t.OVC_PUR_AGENCY equals t2.OVC_PHR_ID
                 where t.OVC_PURCH.Equals(PurchNum) && t2.OVC_PHR_CATE.Equals("C2")
                 select new
                 {
                     t.OVC_PUR_AGENCY,
                     t2.OVC_PHR_DESC,
                     t.OVC_PUR_IPURCH,
                     t.OVC_PUR_NSECTION,
                     t.OVC_PUR_SECTION,
                     t.OVC_PUR_USER,
                     t.OVC_PUR_IUSER_PHONE_EXT
                 }).FirstOrDefault();
            lblOVC_PUR_AGENCY.Text = query.OVC_PUR_AGENCY + query.OVC_PHR_DESC;
            lblOVC_PUR_IPURCH.Text = query.OVC_PUR_IPURCH;
            lblOVC_PUR_NSECTION.Text = query.OVC_PUR_NSECTION;
            lblOVC_PUR_SECTION.Text = query.OVC_PUR_SECTION;
            lblOVC_PUR_USER.Text = query.OVC_PUR_USER;
            lblOVC_PUR_IUSER_PHONE_EXT.Text = query.OVC_PUR_IUSER_PHONE_EXT;
        }
        
        private void ClearControl()
        {
            FCommon.Controls_Clear(txtOVC_PURCH, lblOVC_PUR_AGENCY, lblOVC_PUR_IPURCH
                , lblOVC_PUR_NSECTION, lblOVC_PUR_USER, lblOVC_PUR_SECTION, lblOVC_PUR_IUSER_PHONE_EXT);
        }
        #endregion
    }
}