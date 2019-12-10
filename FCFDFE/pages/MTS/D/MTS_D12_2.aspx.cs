using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;

namespace FCFDFE.pages.MTS.D
{
    public partial class MTS_D12_2 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        private MTSEntities mtse = new MTSEntities();
        Common FCommon = new Common();
        string id;

        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "ODT_APPLY_DATE", Request.QueryString["ODT_APPLY_DATE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_INF_NO", Request.QueryString["OVC_INF_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_IS_PAID", Request.QueryString["OVC_IS_PAID"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_CLASS_NAME", Request.QueryString["OVC_CLASS_NAME"], false);
            return strQueryString;
            //紀錄查詢數值
        }
        #endregion
        protected void btnSave_Click(object sender, EventArgs e)
        {
            FCommon.getQueryString(this, "id", out id, true);
            string strOVC_IS_PAID = rdoOvcIsPaid.SelectedItem.ToString();

            var query = mtse.TBGMT_EINF.Where(t => t.OVC_INF_NO.Equals(id)).FirstOrDefault();
            query.OVC_IS_PAID = strOVC_IS_PAID;
            //對資料庫做異動的指令，帶入下方欄位(註一)
            mtse.SaveChanges();


            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), query.GetType().Name.ToString(), this, "修改");
            // FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), (註一).GetType().Name.ToString(), this, "(這邊改成你的動作：新增、修改、刪除)");
            
            if (query != null)
            {
                query.OVC_IS_PAID = strOVC_IS_PAID;
                mtse.SaveChanges();

                FCommon.AlertShow(PnMessage, "success", "系統訊息", $"{ id} 該筆資料更新成功!");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"{ id }，該筆資料不存在！");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_D12_1{ getQueryString() }");
            //在下方加入返回上頁程式，請去前端確認是否有返回上頁之按鈕
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getQueryString(this,"id",out id,true);
                if (!IsPostBack)
                {
                    lblOvcInfNo.Text = id;
                    var query = mtse.TBGMT_EINF.Where(t => t.OVC_INF_NO.Equals(id)).FirstOrDefault();
                    rdoOvcIsPaid.Text = query.OVC_IS_PAID;
                }
            }
        }
    }
}