using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.MPMS.C
{
    public partial class COMMENT : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        GMEntities gm = new GMEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["OVC_PURCH"]) 
                 ||string.IsNullOrEmpty(Request.QueryString["numCheckTimes"]))
            {
               
            }
            else
            {
                ViewState["OVC_PURCH"] = Request.QueryString["OVC_PURCH"];
                ViewState["CheckTimes"] = Request.QueryString["numCheckTimes"];
                ViewState["AuditUnit"] = Request.QueryString["Unit"];
                if (!IsPostBack)
                {
                    UserInfoImport();
                    DataImport();
                }
                
            }

        }
        private void UserInfoImport()
        {
            if (Session["userid"] != null)
            {
                string unit = ViewState["AuditUnit"].ToString();
                string userID = Session["userid"].ToString();
                var userInfo =
                    (from tAccount in gm.ACCOUNTs.AsEnumerable()
                     join t52001 in mpms.TBM5200_1 on tAccount.USER_ID equals t52001.USER_ID
                     join t1407 in mpms.TBM1407 on t52001.OVC_AUDIT_UNIT equals t1407.OVC_PHR_ID
                     where tAccount.USER_ID.Equals(userID) && t1407.OVC_PHR_CATE.Equals("K5") && t52001.OVC_AUDIT_UNIT.Equals(unit)
                     select new
                     {
                         tAccount.USER_ID,
                         tAccount.USER_NAME,
                         tAccount.DEPT_SN,
                         OVC_AUDIT_UNIT = t52001.OVC_AUDIT_UNIT,
                         UNIT_USR_ID = t1407.OVC_USR_ID,
                         UNIT_NAME = t1407.OVC_PHR_DESC,
                     }).FirstOrDefault();
                lblTitleUnit.Text = userInfo.UNIT_USR_ID;
                lblUserName.Text = userInfo.USER_NAME;
                ViewState["OVC_AUDIT_UNIT"] = userInfo.OVC_AUDIT_UNIT;
                ViewState["DEPT_SN"] = userInfo.DEPT_SN;
            }
        }
        private void DataImport()
        {
            string strUnit = ViewState["OVC_AUDIT_UNIT"].ToString();
            string purch = ViewState["OVC_PURCH"].ToString();
            string checkUnit = ViewState["DEPT_SN"].ToString();
            int numCheckTimes = Convert.ToInt32(ViewState["CheckTimes"].ToString());
            var queryAgency = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(purch)).Select(o => o.OVC_PUR_AGENCY).FirstOrDefault();
            lbl_OVC_PURCH.Text = purch+ queryAgency;
            lblCheckTimes.Text = ViewState["CheckTimes"].ToString();
            var query1202_1 =
                (from t in mpms.TBM1202_1
                where t.OVC_PURCH.Equals(purch) && t.ONB_CHECK_TIMES == numCheckTimes
                        && t.OVC_AUDIT_UNIT.Equals(strUnit) && t.OVC_CHECK_UNIT.Equals(checkUnit)
                select t.OVC_DAUDIT).FirstOrDefault();
            lblOVC_DAUDIT.Text = query1202_1;
            var query =
                from t1 in mpms.TBM1202_COMMENT
                join t2 in mpms.TBMOPINION on new { t1.OVC_AUDIT_UNIT, t1.OVC_TITLE } equals new { t2.OVC_AUDIT_UNIT, t2.OVC_TITLE }
                join t3 in mpms.TBMOPINION_ITEM
                on new { t1.OVC_AUDIT_UNIT, t1.OVC_TITLE, t1.OVC_TITLE_ITEM }
                equals new { t3.OVC_AUDIT_UNIT, t3.OVC_TITLE, t3.OVC_TITLE_ITEM }
                join t4 in mpms.TBMOPINION_DETAIL
                on new { t1.OVC_AUDIT_UNIT, t1.OVC_TITLE, t1.OVC_TITLE_ITEM, t1.OVC_TITLE_DETAIL }
                equals new { t4.OVC_AUDIT_UNIT, t4.OVC_TITLE, t4.OVC_TITLE_ITEM, t4.OVC_TITLE_DETAIL }
                where t1.OVC_PURCH.Equals(purch) && t1.ONB_CHECK_TIMES == numCheckTimes && t1.OVC_AUDIT_UNIT.Equals(strUnit)
                orderby t1.ONB_NO, t1.OVC_TITLE
                select new
                {
                    t1.ONB_NO,
                    OVC_TITLE = t1.OVC_TITLE,
                    OVC_TITLE_ITEM = t1.OVC_TITLE_ITEM,
                    OVC_TITLE_DETAIL = t1.OVC_TITLE_DETAIL,
                    OVC_TITLE_NAME = t2.OVC_CONTENT,
                    OVC_TITLE_ITEM_NAME = t3.OVC_CONTENT,
                    OVC_TITLE_DETAIL_NAME = t4.OVC_CONTENT,
                    t1.OVC_CHECK_REASON
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            //要先把<br> 拿掉
            foreach (DataRow rows in dt.Rows)
            {
                rows["OVC_CHECK_REASON"] = rows["OVC_CHECK_REASON"].ToString().Replace("<br>", "");
            }
            RPT_COMMENT.DataSource = dt;
            RPT_COMMENT.DataBind();
        }
    }
}