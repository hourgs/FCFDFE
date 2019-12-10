using System;
using FCFDFE.Content;
using System.Data;
using System.Linq;
using System.Web.UI;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data.Entity;

namespace FCFDFE.pages.GM
{
    public partial class BulletinBoard : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        Common FCommon = new Common();
        ArrayList list = new ArrayList();

        #region 副程式
        private void GetData()
        {
            if (Session["userid"] != null)
            {
                DateTime dateToday = DateTime.Today;
                string strUserId = Session["userid"].ToString();
                var val =
                    from accountAuth in gm.ACCOUNT_AUTH.AsEnumerable()
                    where accountAuth.USER_ID.Equals(strUserId) && accountAuth.IS_ENABLE.Equals("Y")
                    where accountAuth.END_DATE == null || accountAuth.END_DATE >= dateToday
                    join billg in gm.BILLBOARD_GROUP.AsEnumerable() on accountAuth.C_SN_AUTH equals billg.GROUP_ID
                    join bill in gm.BILLBOARDs.AsEnumerable() on billg.BB_SN equals bill.BB_SN
                    where bill.END_DATE == null || bill.END_DATE >= dateToday
                    join account in gm.ACCOUNTs on bill.AUTHOR_ID equals account.USER_ID into accountTemp
                    from account in accountTemp.DefaultIfEmpty()
                    //where DateTime.Compare(Convert.ToDateTime(bill.END_DATE), dateToday) >= 0
                    orderby bill.START_DATE descending
                    select new
                    {
                        BB_SN = bill.BB_SN,
                        TITLE = bill.TITLE,
                        AUTHOR_ID = account != null ? account.USER_NAME : "",
                        STATUS = bill.STATUS,
                        START_DATE = FCommon.getDateTime(bill.START_DATE, "yyyy-MM-dd HH:mm"),
                        END_DATE = bill.END_DATE == null ? "永久公告" : FCommon.getDateTime(bill.END_DATE)
                    };
                DataTable dt = CommonStatic.LinqQueryToDataTable(val);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_BULLENTIN, dt);
            }
            //GV_TBGMT_BULLENTIN.DataSource = valtable.Select
            //    (billboard => new
            //    {
            //        TITLE = billboard.TITLE,
            //        AUTHOR_ID = billboard.AUTHOR_ID,
            //        STATUS = billboard.STATUS,
            //        START_DATE = billboard.START_DATE,
            //        END_DATE = billboard.END_DATE,
            //    }).ToList();
            //GV_TBGMT_BULLENTIN.DataBind();
            //gm.BILLBOARDs.Add(bill);
        }

        private void AddWindowOpen(string strBB_SN, LinkButton btn)
        {
            string strURL = "";
            string strWinTitle = "";
            string strWinProperty = "";

            strURL = "\\BulletinBoardMemo.aspx?BB_SN=" + strBB_SN;
            strWinTitle = "null";
            strWinProperty = "toolbar=0,location=0,status=0,menubar=0,scrollbars=yes,width=700,height=500,left=200,top=80";
            btn.Attributes.Add("onClick", "javascript:window.open('" + strURL + "','" + strWinTitle + "','" + strWinProperty + "');return false;");
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                list.Clear();
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                    GetData();
            }
        }

        protected void GV_TBGMT_BULLENTIN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            #region 隱藏刪除
            string strUserId = Session["userid"].ToString();
            var queryAuth =
                from auth in gm.ACCOUNT_AUTH
                where auth.USER_ID.Equals(strUserId)
                where auth.C_SN_AUTH.Equals("2101")
                select auth;
            if (!queryAuth.Any())
                GV_TBGMT_BULLENTIN.Columns[4].Visible = false;
            #endregion
            GridView theGridView = (GridView)sender;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int gvrIndex = e.Row.RowIndex;
                string id = theGridView.DataKeys[gvrIndex].Value.ToString();
                LinkButton btn_Print = (LinkButton)e.Row.FindControl("btn_Print");
                AddWindowOpen(id, btn_Print);

                #region 重複公告隱藏
                if (!list.Contains(id))
                    list.Add(id);
                else
                    e.Row.Visible = false;
                #endregion
            }
        }

        protected void GV_TBGMT_BULLENTIN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            Guid id = new Guid(GV_TBGMT_BULLENTIN.DataKeys[gvr.RowIndex].Value.ToString());

            switch (e.CommandName)
            {
                case "DataDel":
                    BILLBOARD billboard = gm.BILLBOARDs.Where(t => t.BB_SN.Equals(id)).FirstOrDefault();
                    gm.Entry(billboard).State = EntityState.Deleted;
                    var querybill = gm.BILLBOARD_GROUP.Where(t => t.BB_SN.Equals(id));
                    foreach (var q in querybill)
                    {
                        BILLBOARD_GROUP billboard_group = q;
                        gm.Entry(billboard_group).State = EntityState.Deleted;
                    }
                    gm.SaveChanges();
                    GetData();
                    break;
            }
        }

        protected void GV_TBGMT_BULLENTIN_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}