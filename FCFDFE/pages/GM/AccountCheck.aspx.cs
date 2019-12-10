using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.GM
{
    public partial class AccountCheck : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities GME = new GMEntities();

        #region 副程式
        private void dataImport_GV()
        {
            string strDEPT_SN = txtOVC_DEPT_CDE.Text;
            string strUSER_NAME = txtUSER_NAME.Text;
            decimal decACCOUNT_STATUS = decimal.Parse(drpACCOUNT_STATUS.SelectedValue);

            var query = 
                from acc in GME.ACCOUNTs
                join dept in GME.TBMDEPTs on acc.DEPT_SN equals dept.OVC_DEPT_CDE
                where acc.ACCOUNT_STATUS != null
                select new
                {
                    AC_SN = acc.AC_SN,
                    OVC_DEPT_CDE = dept.OVC_DEPT_CDE,
                    DEPT_SN = dept.OVC_ONNAME,
                    USER_ID = acc.USER_ID,
                    USER_NAME = acc.USER_NAME,
                    IUSER_PHONE = acc.IUSER_PHONE,
                    ACCOUNT_STATUS = acc.ACCOUNT_STATUS
                };
            
            if (!strDEPT_SN.Equals(string.Empty))
                query = query.Where(table => table.OVC_DEPT_CDE.Equals(strDEPT_SN));
            if (!strUSER_NAME.Equals(string.Empty))
                query = query.Where(table => table.USER_NAME.Contains(strUSER_NAME));
            if (decACCOUNT_STATUS != 2)
                query = query.Where(table => table.ACCOUNT_STATUS == decACCOUNT_STATUS);

            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_AccountCheck, dt);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_DEPT_CDE, txtOVC_ONNAME);
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport_GV();
        }

        protected void GV_AccountCheck_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView theGridView = (GridView)sender;
            GridViewRow gvr = e.Row;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblACCOUNT_STATUS = (Label)gvr.FindControl("lblACCOUNT_STATUS");
                RadioButtonList rdoACCOUNT_STATUS = (RadioButtonList)gvr.FindControl("rdoACCOUNT_STATUS");
                if (FCommon.Controls_isExist(lblACCOUNT_STATUS, rdoACCOUNT_STATUS))
                {
                    FCommon.list_setValue(rdoACCOUNT_STATUS, lblACCOUNT_STATUS.Text);
                }
            }
        }

        protected void GV_AccountCheck_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            Guid id = new Guid(GV_AccountCheck.DataKeys[gvr.RowIndex].Value.ToString());

            switch (e.CommandName)
            {
                case "DataSave":
                    RadioButtonList rdoACCOUNT_STATUS = (RadioButtonList)gvr.FindControl("rdoACCOUNT_STATUS");
                    if (FCommon.Controls_isExist(rdoACCOUNT_STATUS))
                    {
                        decimal decrdoACCOUNT_STATUS = decimal.Parse(rdoACCOUNT_STATUS.SelectedValue);

                        ACCOUNT account = GME.ACCOUNTs.Where(table => table.AC_SN.Equals(id)).FirstOrDefault();
                        if (account != null)
                        {
                            account.ACCOUNT_STATUS = decrdoACCOUNT_STATUS;
                            if (decrdoACCOUNT_STATUS == 1)
                                account.ERROR_CNT = 0;
                            GME.SaveChanges();
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "帳號修改成功！");
                        }
                        else
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查無此帳號！");
                    }
                    break;
            }
        }

        protected void GV_AccountCheck_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}