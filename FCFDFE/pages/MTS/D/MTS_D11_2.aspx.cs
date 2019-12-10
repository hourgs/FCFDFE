using System;
using System.Linq;
using System.Web.UI;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;

namespace FCFDFE.pages.MTS.D
{
    public partial class MTS_D11_2 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities mtse = new MTSEntities();
        Common FCommon = new Common();
        string id; //修改此表之主鍵編號，全域變數。到註二地方繼續
        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "ODT_APPLY_DATE", Request.QueryString["ODT_APPLY_DATE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_INF_NO", Request.QueryString["OVC_INF_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_IS_PAID", Request.QueryString["OVC_IS_PAID"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_CLASS_NAME", Request.QueryString["OVC_CLASS_NAME"], false);
            return strQueryString;
            //在接收頁面加入此副程式
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getQueryString(this, "id", out id, true); 
                //註二：須於Page_Load下IsPostBack外部取值(id)，否則postBack時未定義。

                if (!IsPostBack)
                {
                    lblOvcInfNo.Text = id;
                    var query = mtse.TBGMT_IINF.Where(t => t.OVC_INF_NO.Equals(id)).FirstOrDefault();   
                    rdoOvcIsPaid.Text = query.OVC_IS_PAID;
                }
            }
            
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strOVC_IS_PAID = rdoOvcIsPaid.SelectedItem.ToString();

            var query = mtse.TBGMT_IINF.Where(t => t.OVC_INF_NO.Equals(id)).FirstOrDefault();
            if (query != null)
            {
                query.OVC_IS_PAID = strOVC_IS_PAID;
                mtse.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), query.GetType().Name.ToString(), this, "修改");

                FCommon.AlertShow(PnMessage, "success", "系統訊息", id + "，該筆資料更新成功!");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", $"{ id }，該筆資料不存在！");
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_D11_1{ getQueryString() }");
            //在下方加入返回上頁程式，請去前端確認是否有返回上頁之按鈕
        }
    }
}