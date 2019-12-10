using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Entity.GMModel;
using System.Data;
using System.Web.UI.HtmlControls;

namespace FCFDFE.pages.MPMS.C
{
    public partial class ReviewCOMMENT : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        GMEntities gm = new GMEntities();
        string[] field = { "OVC_ITEM", "OVC_ITEM_ADVICE" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["OVC_PURCH"] = Request.QueryString["OVC_PURCH"];
                ViewState["ONB_CHECK_TIMES"] = Request.QueryString["ONB_CHECK_TIMES"];
                ViewState["IS_RESPONSE"] = Request.QueryString["IS_RESPONSE"];
                GetUserInfo();
                DataImport();
                RepeaterHeaderImport();

            }
        }

        protected void GV_OVC_BUDGET_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        private void DataImport()
        {
            string strPurNum = ViewState["OVC_PURCH"].ToString();
            byte numCheckTimes = Convert.ToByte(ViewState["ONB_CHECK_TIMES"]);
            string deptSn = ViewState["DEPT_SN"].ToString();
            string checkStatus;
            var query1202 =
                (from t1202 in mpms.TBM1202
                 join dept in mpms.TBMDEPTs on t1202.OVC_CHECK_UNIT equals dept.OVC_DEPT_CDE
                 where t1202.OVC_PURCH.Equals(strPurNum) && t1202.ONB_CHECK_TIMES == numCheckTimes
                    && t1202.OVC_CHECK_UNIT.Equals(deptSn)
                 select new
                 {
                     OVC_CHECK_OK = t1202.OVC_CHECK_OK ?? "",
                     t1202.OVC_DRECEIVE,
                     t1202.OVC_DRESULT,
                     t1202.OVC_CHECKER,
                     dept.OVC_ONNAME

                 }).FirstOrDefault();
            var query1301 = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurNum)).FirstOrDefault();
            lblOVC_PURCH.Text = strPurNum + query1301.OVC_PUR_AGENCY;
            if (!string.IsNullOrEmpty(query1202.OVC_CHECK_OK) && query1202.OVC_CHECK_OK.Equals("Y"))
            {
                checkStatus = "(確認審)";
            }
            else if (numCheckTimes == 1)
            {
                checkStatus = "(初審)";
            }
            else
            {
                checkStatus = "(複審)";
            }
            lblCHECK_TIMES.Text = numCheckTimes.ToString();
            lblcheckStatus.Text = checkStatus;
            lblDRECIVE.Text = query1202.OVC_DRECEIVE;
            lblCHECK_OK.Text = query1202.OVC_CHECK_OK;
            lblDRESULT.Text = query1202.OVC_DRESULT;
            lblCHECK_UNIT.Text = query1202.OVC_ONNAME;
            lblOVC_CHECKER.Text = query1202.OVC_CHECKER;

            var queryADVICE = mpms.TBM1202_ADVICE.Where(o => o.OVC_PURCH.Equals(strPurNum) && o.OVC_DRECEIVE.Equals(query1202.OVC_DRECEIVE));
            DataTable dt = CommonStatic.LinqQueryToDataTable(queryADVICE);
            ViewState["hasRow"] = FCommon.GridView_dataImport(GV_ADVICE, dt);

            var query1202_7 =
                from t in mpms.TBM1202_7
                where t.OVC_PURCH.Equals(strPurNum) && t.ONB_CHECK_TIMES == numCheckTimes 
                orderby t.OVC_IKIND,t.ONB_NO
                select new
                {
                    t.OVC_MEMO
                };
            int iCount = 1;
            foreach(var item in query1202_7)
            {
                lblAllComment.Text += iCount.ToString() + "." + item.OVC_MEMO + "<br>";
                iCount++;
            }

            var query1202_6 =
                from t in mpms.TBM1202_6
                where t.OVC_PURCH.Equals(strPurNum) && t.ONB_CHECK_TIMES == numCheckTimes
                orderby t.OVC_IKIND, t.ONB_NO
                select new
                {
                    t.OVC_MEMO
                };
            int iCount_2 = 1;
            foreach (var item in query1202_6)
            {
                lblApproveComent.Text += iCount_2.ToString() + "." + item.OVC_MEMO + "<br>";
                iCount_2++;
            }

            var queryAttach = mpms.TBM1119.Where(o => o.OVC_PURCH.Equals(strPurNum));

            DataTable dtAttach = CommonStatic.LinqQueryToDataTable(queryAttach);
            foreach(DataRow row in dtAttach.Rows)
            {
                if (row["OVC_IKIND"].ToString().Equals("D"))
                {
                    row["OVC_IKIND"] = "採購計畫清單";
                }
                else
                {
                    row["OVC_IKIND"] = "物資申請書";
                }
            }
            FCommon.GridView_dataImport(GV_ATTACH, dtAttach);



        }

        private void RepeaterHeaderImport()
        {
            string strPurNum = ViewState["OVC_PURCH"].ToString();
            string deptSn = ViewState["DEPT_SN"].ToString();
            byte numCheckTimes = Convert.ToByte(ViewState["ONB_CHECK_TIMES"]);
            //第一層repeater
            var query =
                (from t12021 in mpms.TBM1202_1
                 join t1407 in mpms.TBM1407 on t12021.OVC_AUDIT_UNIT equals t1407.OVC_PHR_ID
                 join tAccount in mpms.ACCOUNT
                 on new { name = t12021.OVC_AUDITOR, unit = t12021.OVC_CHECK_UNIT }
                 equals new { name = tAccount.USER_NAME, unit = tAccount.DEPT_SN }
                 where t12021.OVC_PURCH.Equals(strPurNum) && t12021.ONB_CHECK_TIMES == numCheckTimes
                         && t12021.OVC_CHECK_UNIT.Equals(deptSn)
                         && t1407.OVC_PHR_CATE.Equals("K5") && tAccount.IUSER_PHONE != null
                 orderby t1407.OVC_PHR_ID
                 select new
                 {
                     t12021.OVC_AUDIT_UNIT,
                     t1407.OVC_USR_ID,
                     t12021.OVC_AUDITOR,
                     tAccount.IUSER_PHONE,
                     t12021.OVC_DAUDIT_ASSIGN,
                     t12021.OVC_DAUDIT
                 }).ToArray();
            var queryFinal =
                from t in query
                select new
                {
                    t.OVC_AUDIT_UNIT,
                    t.OVC_USR_ID,
                    t.OVC_AUDITOR,
                    t.IUSER_PHONE,
                    PROCESS = DateDiff(t.OVC_DAUDIT_ASSIGN, t.OVC_DAUDIT)
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(queryFinal);
            Repeater_Header.DataSource = dt;
            Repeater_Header.DataBind();
        }

        private string DateDiff(string strdate1, string strdate2)
        {
            if (string.IsNullOrEmpty(strdate1) || string.IsNullOrEmpty(strdate2))
            {
                return "";
            }
            else
            {
                string dateDiff = null;
                DateTime DateTime1 = Convert.ToDateTime(strdate1);
                DateTime DateTime2 = Convert.ToDateTime(strdate2);
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                dateDiff = ts.Days.ToString();
                return dateDiff;
            }

        }

        private void GetUserInfo()
        {
            if (Session["userid"] != null)
            {
                if (Session["userid"] != null)
                {
                    string userID = Session["userid"].ToString();
                    var userInfo = gm.ACCOUNTs.Where(o => o.USER_ID.Equals(userID)).FirstOrDefault();
                    ViewState["USER_NAME"] = userInfo.DEPT_SN;
                    ViewState["DEPT_SN"] = userInfo.DEPT_SN;
                }
            }

        }

        private DataTable CommentImport(string auditUnit)
        {
            string strPurNum = ViewState["OVC_PURCH"].ToString();
            string deptSn = ViewState["DEPT_SN"].ToString();
            byte numCheckTimes = Convert.ToByte(ViewState["ONB_CHECK_TIMES"]);
            var query =
                from t1202C in mpms.TBM1202_COMMENT
                join tDETAIL in mpms.TBMOPINION_DETAIL
                on new { t1202C.OVC_AUDIT_UNIT, t1202C.OVC_TITLE, t1202C.OVC_TITLE_ITEM, t1202C.OVC_TITLE_DETAIL }
                equals new { tDETAIL.OVC_AUDIT_UNIT, tDETAIL.OVC_TITLE, tDETAIL.OVC_TITLE_ITEM, tDETAIL.OVC_TITLE_DETAIL }
                where t1202C.OVC_PURCH.Equals(strPurNum) && t1202C.ONB_CHECK_TIMES == numCheckTimes
                        && t1202C.OVC_CHECK_UNIT.Equals(deptSn) 
                        && t1202C.OVC_AUDIT_UNIT.Equals(auditUnit)
                orderby t1202C.ONB_NO
                select new
                {
                    t1202C.ONB_NO,
                    tDETAIL.OVC_CONTENT,
                    t1202C.OVC_CHECK_REASON,
                    t1202C.OVC_RESPONSE
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            foreach (DataRow rows in dt.Rows)
            {
                rows["OVC_RESPONSE"] = rows["OVC_RESPONSE"].ToString().Replace("<br>", "");
            }
            return dt;
        }

        protected void Repeater_Header_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                HiddenField hidOVC_AUDIT_UNIT = (HiddenField)e.Item.FindControl("hidOVC_AUDIT_UNIT");
                string strUnit = hidOVC_AUDIT_UNIT.Value;
                Repeater childRepeater = (Repeater)e.Item.FindControl("Repeater_Content");//找到要繫結資料的childRepeater
                if (childRepeater != null)
                {
                    DataTable dt = new DataTable();
                    dt = CommentImport(strUnit);
                    childRepeater.DataSource = dt;
                    childRepeater.DataBind();
                }
            }
        }

        protected void Repeater_Content_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string isRespose = ViewState["IS_RESPONSE"].ToString();
                if (isRespose.Equals("false"))
                {
                    HtmlTableCell cellTitle = e.Item.FindControl("cellTitle") as HtmlTableCell;
                    HtmlTableCell cellContent = e.Item.FindControl("cellContent") as HtmlTableCell;
                    cellTitle.Attributes.Add("style", "display: none;");
                    cellContent.Attributes.Add("style", "display: none;");
                }
            }
        }
    }
}