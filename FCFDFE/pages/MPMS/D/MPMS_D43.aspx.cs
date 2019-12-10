using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D43 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            if (!IsPostBack)
            {

                if (Request.QueryString["PurchNum"] != null)
                {

                    string purchNum  = Request.QueryString["PurchNum"];
                    lblPurchNum.Text = purchNum;
                    ViewState["OVC_PURCH"] = purchNum;
                    QueryPlanDataImport();
                }
                else
                {
                    Response.Redirect("MPMS_D42.aspx");
                }
            }
        }

        protected void GV_Query_PLAN_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
            {
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            }
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void GV_Query_PLAN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hidOVC_CHECK_OK = (HiddenField)e.Row.FindControl("hidOVC_CHECK_OK");
                HiddenField hidOVC_PERMISSION_UPDATE = (HiddenField)e.Row.FindControl("hidOVC_PERMISSION_UPDATE");
                HiddenField hidOVC_DREJECT = (HiddenField)e.Row.FindControl("hidOVC_DREJECT");
                Label lblONB_CHECK_TIMES = (Label)e.Row.FindControl("lblONB_CHECK_TIMES");
                Label lblAUDITSTATUS = (Label)e.Row.FindControl("lblAUDITSTATUS");
                Label lblOVC_DRESULT = (Label)e.Row.FindControl("lblOVC_DRESULT");
                Label lblSignDate = (Label)e.Row.FindControl("lblSignDate");
                Label lblUnit = (Label)e.Row.FindControl("lblUnit");
                LinkButton btn_PrintComment = (LinkButton)e.Row.FindControl("btn_PrintComment");
                LinkButton btn_CommentWithResponse = (LinkButton)e.Row.FindControl("btn_CommentWithResponse");
                DateTime dateReject;
                DateTime now = DateTime.Now;
                TimeSpan span;
                Boolean isTimeData_Reject = DateTime.TryParse(hidOVC_DREJECT.Value, out dateReject);

                if (lblAUDITSTATUS.Text.Equals("Y"))
                {
                    lblAUDITSTATUS.Text = "(確認審)";
                }
                else
                {
                    if (lblONB_CHECK_TIMES.Text.Equals("1"))
                    {
                        lblAUDITSTATUS.Text = "(初審)";
                    }
                    else
                        lblAUDITSTATUS.Text = "(複審)";
                }

                
                if (lblUnit.Text.Equals("【尚未回覆】"))
                {
                    lblUnit.Text = "";
                }
                else
                {
                    btn_PrintComment.Visible = true;
                    btn_CommentWithResponse.Visible = true;
                    AddWindowOpen(lblONB_CHECK_TIMES.Text, "false", btn_PrintComment);
                    AddWindowOpen(lblONB_CHECK_TIMES.Text, "true", btn_CommentWithResponse);
                }
                if (e.Row.RowIndex == GV_Query_PLAN.Rows.Count)
                {
                    //逾時五日、七日
                    if (isTimeData_Reject)
                    {
                        span = now.Subtract(dateReject);
                        int diff = Convert.ToInt16(span.Days);
                        if (hidOVC_CHECK_OK.Value != "Y" && hidOVC_PERMISSION_UPDATE.Value.Equals("Y"))
                        {
                            if (lblONB_CHECK_TIMES.Text.Equals("1") && diff >= 7)
                            {
                                lblSignDate.Visible = true;
                            }
                            else if (lblONB_CHECK_TIMES.Text != "1" && diff >= 5)
                            {
                                lblSignDate.Visible = true;
                            }
                        }
                    }
                }
            }
        }

        private void AddWindowOpen(string numCheckTimes, string IS_RESPONSE, LinkButton btn)
        {
            string strURL = "";
            string strWinTitle = "";
            string strWinProperty = "";
            string strPurchNum = ViewState["OVC_PURCH"].ToString();
            strURL = "\\Comment_D.aspx?OVC_PURCH=" + strPurchNum + "&ONB_CHECK_TIMES=" + numCheckTimes + "&IS_RESPONSE=" + IS_RESPONSE;
            strWinTitle = "null";
            strWinProperty = "toolbar=0,location=0,status=0,menubar=0,scrollbars=yes,width=700,height=500,left=200,top=80";
            btn.Attributes.Add("onClick", "javascript:window.open('" + strURL + "','" + strWinTitle + "','" + strWinProperty + "');return false;");
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_D42.aspx");
        }

        private void QueryPlanDataImport()
        {
            string strPurchNum = ViewState["OVC_PURCH"].ToString();
           
            var detp = mpms.TBM1407.Where(o => o.OVC_PHR_CATE.Equals("K5"));
            var query =
                from t in mpms.TBM1202
                join t1202_1 in mpms.TBM1202_1 on new { t.OVC_PURCH, t.ONB_CHECK_TIMES } equals new { t1202_1.OVC_PURCH, t1202_1.ONB_CHECK_TIMES } into ps
                from t1202_1 in ps.DefaultIfEmpty()
                join t1407 in detp on t1202_1.OVC_AUDIT_UNIT equals t1407.OVC_PHR_ID into gp
                from t1407 in gp.DefaultIfEmpty()
                join t1301 in mpms.TBM1301 on t.OVC_PURCH equals t1301.OVC_PURCH
                where t.OVC_PURCH.Equals(strPurchNum)
                select new
                {
                    t.OVC_PURCH,
                    t.ONB_CHECK_TIMES,
                    t.OVC_CHECK_OK,
                    t.OVC_DRECEIVE,
                    t.OVC_DRESULT,
                    t.OVC_DREJECT,
                    OVC_DAUDIT = t1202_1 != null ? t1202_1.OVC_DAUDIT : "",
                    OVC_USR_ID = t1407 != null ? t1407.OVC_USR_ID : "",
                    t1301.OVC_PERMISSION_UPDATE
                };
            var queryCombine =
                from t in query.AsEnumerable()
                select new
                {
                    t.OVC_PURCH,
                    t.ONB_CHECK_TIMES,
                    t.OVC_CHECK_OK,
                    t.OVC_DRECEIVE,
                    t.OVC_DRESULT,
                    t.OVC_DREJECT,
                    t.OVC_DAUDIT,
                    t.OVC_PERMISSION_UPDATE,
                    ISAUDIT = t.OVC_USR_ID + (t.OVC_DAUDIT == null ? "【尚未回覆】" : "【於" + FCommon.getTaiwanDate(t.OVC_DAUDIT, "{0}.{1}.{2}") + "回覆】")
                };
            var groupAll =
                queryCombine.AsEnumerable()
                .OrderBy(o => o.ONB_CHECK_TIMES)
                .GroupBy(o =>
                new
                {
                    o.OVC_PURCH,
                    o.OVC_DRESULT,
                    o.ONB_CHECK_TIMES,
                    o.OVC_DRECEIVE,
                    o.OVC_CHECK_OK,
                    o.OVC_DREJECT,
                    o.OVC_PERMISSION_UPDATE,
                }).Select(g =>
                new
                {
                    g.Key.OVC_PURCH,
                    g.Key.OVC_DRESULT,
                    g.Key.ONB_CHECK_TIMES,
                    g.Key.OVC_DRECEIVE,
                    g.Key.OVC_CHECK_OK,
                    g.Key.OVC_DREJECT,
                    g.Key.OVC_PERMISSION_UPDATE,
                    ISAUDIT = string.Join("；", g.Select(ee => ee.ISAUDIT).ToList())
                });


            DataTable dt = CommonStatic.LinqQueryToDataTable(groupAll);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_Query_PLAN, dt);
        }
    }
}