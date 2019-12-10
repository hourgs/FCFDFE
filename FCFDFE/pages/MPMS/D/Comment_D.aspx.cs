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

namespace FCFDFE.pages.MPMS.D
{
    public partial class Comment_D : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        GMEntities gm = new GMEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["OVC_PURCH"] = Request.QueryString["OVC_PURCH"];
                ViewState["ONB_CHECK_TIMES"] = Request.QueryString["ONB_CHECK_TIMES"];
                ViewState["IS_RESPONSE"] = Request.QueryString["IS_RESPONSE"];
                RepeaterHeaderImport();

            }
        }

        private void RepeaterHeaderImport()
        {
            string strPurNum = ViewState["OVC_PURCH"].ToString();
            int ONB_CHECK_TIMES = Convert.ToInt32(ViewState["ONB_CHECK_TIMES"]);
            var query =
               (from t12021 in mpms.TBM1202_1
                join t1407 in mpms.TBM1407 on t12021.OVC_AUDIT_UNIT equals t1407.OVC_PHR_ID
                join tAccount in mpms.ACCOUNT
                on new { name = t12021.OVC_AUDITOR, unit = t12021.OVC_CHECK_UNIT }
                equals new { name = tAccount.USER_NAME, unit = tAccount.DEPT_SN }
                where t12021.OVC_PURCH.Equals(strPurNum) && t12021.ONB_CHECK_TIMES == ONB_CHECK_TIMES
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

        private DataTable CommentImport(string auditUnit)
        {
           
            string strPurNum = ViewState["OVC_PURCH"].ToString();
            byte numCheckTimes = Convert.ToByte(ViewState["ONB_CHECK_TIMES"]);
            var query =
                from t1202C in mpms.TBM1202_COMMENT
                join tDETAIL in mpms.TBMOPINION_DETAIL
                on new { t1202C.OVC_AUDIT_UNIT, t1202C.OVC_TITLE, t1202C.OVC_TITLE_ITEM, t1202C.OVC_TITLE_DETAIL }
                equals new { tDETAIL.OVC_AUDIT_UNIT, tDETAIL.OVC_TITLE, tDETAIL.OVC_TITLE_ITEM, tDETAIL.OVC_TITLE_DETAIL }
                where t1202C.OVC_PURCH.Equals(strPurNum) && t1202C.ONB_CHECK_TIMES == numCheckTimes
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
    }
}