using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.E
{
    public partial class MTS_E12_2 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        private MTSEntities MTS = new MTSEntities();
        Common FCommon = new Common();
        TBGMT_ICS ics = new TBGMT_ICS();
        string id;
        string[] filed;
        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getQueryString(this, "id", out id, true);
            ViewState["bld_no"] = id;

            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (Session["MTS_E12"] != null)
                    filed = Session["MTS_E12"].ToString().Split(',');
                if (Session["MTS_E12_PAGE"] != null)
                {
                    lblnow.Text = (Convert.ToInt16(Session["MTS_E12_PAGE"]) + 1).ToString();
                    lblall.Text = filed.Length.ToString();
                }

                if (!IsPostBack)
                {
                    if (id != "")
                        dataImport(id);
                }
            }
        }

        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", Request.QueryString["OVC_BLD_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_SEA_OR_AIR", Request.QueryString["OVC_SEA_OR_AIR"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_SHIP_NAME", Request.QueryString["OVC_SHIP_NAME"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_VOYAGE", Request.QueryString["OVC_VOYAGE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_MILITARY_TYPE", Request.QueryString["OVC_MILITARY_TYPE"], false);
            FCommon.setQueryString(ref strQueryString, "isBring", Request.QueryString["isBring"], false);
            FCommon.setQueryString(ref strQueryString, "isDefine", Request.QueryString["isDefine"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_START_DATE", Request.QueryString["ODT_START_DATE"], false);
            FCommon.setQueryString(ref strQueryString, "ODT_LAST_DATE", Request.QueryString["ODT_LAST_DATE"], false);
            return strQueryString;
        }

        protected void btnlast_Click(object sender, EventArgs e)
        {
            string[] filed = Session["MTS_E12"].ToString().Split(',');
            int strnum = (Convert.ToInt16(Session["MTS_E12_PAGE"]) - 1);
            Session["MTS_E12_PAGE"] = strnum;
            string bld_no = filed[strnum].ToString();
            dataImport(bld_no);
        }
        protected void btnnext_Click(object sender, EventArgs e)
        {
            string[] filed = Session["MTS_E12"].ToString().Split(',');
            int strnum = (Convert.ToInt16(Session["MTS_E12_PAGE"]) + 1);
            Session["MTS_E12_PAGE"] = strnum;
            string bld_no = filed[strnum].ToString();
            dataImport(bld_no);

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string txtOVC_BLD_NO = txtBldSn.Text;
            string txtOVC_INLAND_CARRIAGE = txtOnbCarriage.Text;
            string txtOVC_INF_NO = txtInfSn.Text;
            DateTime txtODT_MODIFY_DATE = DateTime.Now;


           

            if (txtOVC_BLD_NO.Equals(string.Empty))
                strMessage += "<P> 請填入 提單編號 </p>";
            if (txtOVC_INLAND_CARRIAGE.Equals(string.Empty))
                strMessage += "<P> 請填入 海空運費 </p>";
            if (txtOVC_INF_NO.Equals(string.Empty))
                strMessage += "<P> 請填入 結報申請表編號 </p>";

            Decimal decOVC_INLAND_CARRIAGE;
            bool boolOVC_INLAND_CARRIAGE = FCommon.checkDecimal(txtOVC_INLAND_CARRIAGE, "海空運費", ref strMessage, out decOVC_INLAND_CARRIAGE);


            ics = MTS.TBGMT_ICS.Where(table => table.OVC_ICS_NO == lblOvcIcsNo.Text).FirstOrDefault();
            if(strMessage.Equals(string.Empty))
            {
                try
                {
                    ics.OVC_BLD_NO = txtOVC_BLD_NO;
                    if (boolOVC_INLAND_CARRIAGE) ics.OVC_INLAND_CARRIAGE = decOVC_INLAND_CARRIAGE; else ics.OVC_INLAND_CARRIAGE = null;
                    ics.OVC_INF_NO = txtOVC_INF_NO;
                    MTS.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), ics.GetType().Name.ToString(), this, "修改");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功!!");
                }
                catch (Exception ex)
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "修改失敗");
                }

            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);

        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                ics = MTS.TBGMT_ICS.Where(table => table.OVC_ICS_NO == lblOvcIcsNo.Text).FirstOrDefault();
                MTS.Entry(ics).State = EntityState.Deleted;
                MTS.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), ics.GetType().Name.ToString(), this, "刪除");


                //刪除後清除文字內容
                lblOvcIcsNo.Text = "";
                txtBldSn.Text = "";
                txtOnbCarriage.Text = "";
                txtInfSn.Text = "";
                OdtModifyDate.Text = "";
                lblOvcCreateId.Text = "";

                FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功!!");
            }
            catch (Exception ex)
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "刪除失敗");
            }
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_E12_1{getQueryString()}");
        }

        public void dataImport(string bld_no)
        {
            DataTable dt = new DataTable();
            var query =
                from ics in MTS.TBGMT_ICS
                where ics.OVC_BLD_NO.Equals(bld_no)
                select ics;
            dt = CommonStatic.LinqQueryToDataTable(query);
            lblOvcIcsNo.Text = dt.Rows[0]["OVC_ICS_NO"].ToString();
            txtBldSn.Text = dt.Rows[0]["OVC_BLD_NO"].ToString();
            txtOnbCarriage.Text= dt.Rows[0]["OVC_INLAND_CARRIAGE"].ToString();
            txtInfSn.Text = dt.Rows[0]["OVC_INF_NO"].ToString();
            OdtModifyDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            lblOvcCreateId.Text= Session["userid"].ToString();
            lblnow.Text = (Convert.ToInt16(Session["MTS_E12_PAGE"]) + 1).ToString();

            //如果總筆數最高時隱藏下一頁按鈕
            string[] filed = Session["MTS_E12"].ToString().Split(',');
            if (lblnow.Text == filed.Length.ToString())
                btnnext.Visible = false;
            else
                btnnext.Visible = true;

            //如果總筆數為1時隱藏上一頁按鈕
            if (lblnow.Text == "1")
                btnlast.Visible = false;
            else
                btnlast.Visible = true;
        }
    }
}