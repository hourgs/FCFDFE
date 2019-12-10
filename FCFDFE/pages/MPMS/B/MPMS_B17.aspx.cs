using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.B
{
    public partial class MPMS_B17 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);

                if (Session["userid"] != null)
                {
                    string strUSER_ID = Session["userid"].ToString();
                    ACCOUNT ac = new ACCOUNT();
                    ac = gm.ACCOUNTs.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                    if (ac != null)
                    {
                        ViewState["UserName"] = ac.USER_NAME;
                        ViewState["UserDeptSN"] = ac.DEPT_SN;
                    }
                }
            }
        }

        #region onClick
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            //查詢按鈕功能

            string strMessage = "";
            string txtPurch = txtOVC_PURCH.Text;

            var query = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(txtPurch));
            if (query.Any())
            {
                DataImport(txtPurch);
                drpRow.Visible = true;
            }
            else
            {
                 strMessage += "<p>本購案編號：" + txtPurch + " 不存在，請重新輸入！</p>";
                ClearControl();
                drpRow.Visible = false;
                FCommon.AlertShow(PnMessage, "danger", "注意事項", strMessage);
            }
            

        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            //清除按鈕功能
            ClearControl();
            drpRow.Visible = false;
        }
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            //回主畫面按鈕功能
            Response.Redirect("MPMS_B11");
        }
        protected void btnModify_Click(object sender, EventArgs e)
        {
            
            ChangePurchNumber();
        }
        #endregion

        #region 副程式
        private void DataImport(string PurchNum)
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
            //可變更的預劃購案
            drpTO_PHRID.Items.Clear();
            string userName = ViewState["UserName"].ToString();
            var query1301P =
                (from t2 in gm.TBM1301_PLAN where t2.OVC_PUR_USER.Equals(userName) select t2.OVC_PURCH)
                .Except(from t in gm.TBM1301 where t.OVC_PUR_USER.Equals(userName) select t.OVC_PURCH);

            foreach (var id in query1301P)
            {
                drpTO_PHRID.Items.Add(id);
            }

        }


        private void ChangePurchNumber()
        {
            string userName = ViewState["UserName"].ToString();
            //userName=測試123
            System.Data.Entity.Core.Objects.ObjectParameter rTN_MSG =
                new System.Data.Entity.Core.Objects.ObjectParameter("rTN_MSG", typeof(char));
            //try {

            mpms.PG_CHANGE_PURCH_ID(txtOVC_PURCH.Text, drpTO_PHRID.SelectedItem.Text, userName, rTN_MSG);
                string myKey = rTN_MSG.Value.ToString();
                if (myKey.Equals("SUCCESS"))
                {
                    ChangeOtherTable();
                    InsertLog();
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "更改成功");
                }
                else
                    FCommon.AlertShow(PnMessage, "fail", "系統訊息", "更改失敗");
                //ClearControl();
                //drpRow.Visible = false;
            /*}
            catch
            {
                FCommon.AlertShow(PnMessage,"Alert", userName, "新購案編號無新增選項!");
            }*/
        }
        
        private void ChangeOtherTable()
        {
            //先刪除再新增
            //TBM1118_1
            var query1118_1 =
                from t in mpms.TBM1118_1
                where t.OVC_PURCH.Equals(txtOVC_PURCH)
                select t;
               
            if (query1118_1.Any())
            {
                foreach (var item in query1118_1)
                {
                    mpms.Entry(item).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , item.GetType().Name.ToString(), this, "刪除");
                    item.OVC_PURCH = drpTO_PHRID.SelectedItem.Text;
                    mpms.TBM1118_1.Add(item);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , item.GetType().Name.ToString(), this, "新增");
                }
            }

            //TBM1118_2
            var query1118_2 =
                from t in mpms.TBM1118_2
                where t.OVC_PURCH.Equals(txtOVC_PURCH)
                select t;

            if (query1118_2.Any())
            {
                foreach (var item in query1118_2)
                {
                    mpms.Entry(item).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , item.GetType().Name.ToString(), this, "刪除");
                    item.OVC_PURCH = drpTO_PHRID.SelectedItem.Text;
                    mpms.TBM1118_2.Add(item);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , item.GetType().Name.ToString(), this, "新增");
                }
            }

            //TBM1118_3
            var query1118_3 =
                from t in mpms.TBM1118_3
                where t.OVC_PURCH.Equals(txtOVC_PURCH)
                select t;

            if (query1118_3.Any())
            {
                foreach (var item in query1118_3)
                {
                    mpms.Entry(item).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , item.GetType().Name.ToString(), this, "刪除");
                    item.OVC_PURCH = drpTO_PHRID.SelectedItem.Text;
                    mpms.TBM1118_3.Add(item);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , item.GetType().Name.ToString(), this, "新增");
                }
            }

            //TBMOVERSEE_LOG
            var queryOVERSEE_LOG =
                from t in mpms.TBMOVERSEE_LOG
                where t.OVC_PURCH.Equals(txtOVC_PURCH)
                select t;

            if (queryOVERSEE_LOG.Any())
            {
                foreach (var item in queryOVERSEE_LOG)
                {
                    mpms.Entry(item).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , item.GetType().Name.ToString(), this, "刪除");
                    item.OVC_PURCH = drpTO_PHRID.SelectedItem.Text;
                    mpms.TBMOVERSEE_LOG.Add(item);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , item.GetType().Name.ToString(), this, "新增");
                }
            }
            

            //TBM1202
            var query1202 =
                from t in mpms.TBM1202
                where t.OVC_PURCH.Equals(txtOVC_PURCH)
                select t;

            if (query1202.Any())
            {
                foreach (var item in query1202)
                {
                    mpms.Entry(item).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , item.GetType().Name.ToString(), this, "刪除");
                    item.OVC_PURCH = drpTO_PHRID.SelectedItem.Text;
                    mpms.TBM1202.Add(item);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , item.GetType().Name.ToString(), this, "新增");
                }
            }

            //TBM1202_COMMENT
            var query1202_COMMENT =
                from t in mpms.TBM1202_COMMENT
                where t.OVC_PURCH.Equals(txtOVC_PURCH)
                select t;

            if (query1202_COMMENT.Any())
            {
                foreach (var item in query1202_COMMENT)
                {
                    mpms.Entry(item).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , item.GetType().Name.ToString(), this, "刪除");
                    item.OVC_PURCH = drpTO_PHRID.SelectedItem.Text;
                    mpms.TBM1202_COMMENT.Add(item);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , item.GetType().Name.ToString(), this, "新增");
                }
            }

            //TBM1202_ADVICE
            var query1202_ADVICE =
                from t in mpms.TBM1202_ADVICE
                where t.OVC_PURCH.Equals(txtOVC_PURCH)
                select t;

            if (query1202_ADVICE.Any())
            {
                foreach (var item in query1202_ADVICE)
                {
                    mpms.Entry(item).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , item.GetType().Name.ToString(), this, "刪除");
                    item.OVC_PURCH = drpTO_PHRID.SelectedItem.Text;
                    mpms.TBM1202_ADVICE.Add(item);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , item.GetType().Name.ToString(), this, "新增");
                }
            }

            //TBM1202_1
            var query1202_1 =
                from t in mpms.TBM1202_1
                where t.OVC_PURCH.Equals(txtOVC_PURCH)
                select t;

            if (query1202_1.Any())
            {
                foreach (var item in query1202_1)
                {
                    mpms.Entry(item).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , item.GetType().Name.ToString(), this, "刪除");
                    item.OVC_PURCH = drpTO_PHRID.SelectedItem.Text;
                    mpms.TBM1202_1.Add(item);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , item.GetType().Name.ToString(), this, "新增");
                }
            }

            //TBM1202_2
            var query1202_2 =
                from t in mpms.TBM1202_2
                where t.OVC_PURCH.Equals(txtOVC_PURCH)
                select t;

            if (query1202_2.Any())
            {
                foreach (var item in query1202_2)
                {
                    mpms.Entry(item).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , item.GetType().Name.ToString(), this, "刪除");
                    item.OVC_PURCH = drpTO_PHRID.SelectedItem.Text;
                    mpms.TBM1202_2.Add(item);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , item.GetType().Name.ToString(), this, "新增");
                }
            }
        }

        private void InsertLog()
        {
            //TBM1114新增變更紀錄
            string userName = ViewState["UserName"].ToString();
            string deptSN = ViewState["UserDeptSN"].ToString();
            var querydeptName = gm.TBMDEPTs.Where(o => o.OVC_DEPT_CDE.Equals(deptSN)).Select(o=>o.OVC_ONNAME).FirstOrDefault();
            TBM1114 tbm1114 = new TBM1114();
            tbm1114.OVC_PURCH = drpTO_PHRID.SelectedItem.Text;
            tbm1114.OVC_USER = userName;
            tbm1114.OVC_FROM_UNIT_NAME = querydeptName;
            tbm1114.OVC_TO_UNIT_NAME = querydeptName;
            tbm1114.OVC_REMARK = "變更購案編號：由" + txtOVC_PURCH.Text + "變更為：" + drpTO_PHRID.SelectedItem.Text;
            mpms.TBM1114.Add(tbm1114);
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1114.GetType().Name.ToString(), this, "新增");
        }
        private void ClearControl()
        {
            FCommon.Controls_Clear(txtOVC_PURCH, lblOVC_PUR_AGENCY, lblOVC_PUR_IPURCH
                , lblOVC_PUR_NSECTION, lblOVC_PUR_USER, lblOVC_PUR_SECTION, lblOVC_PUR_IUSER_PHONE_EXT,drpTO_PHRID);
        }
        #endregion
    }
}