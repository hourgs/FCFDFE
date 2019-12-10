using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.E
{
    public partial class MTS_E12_3 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        private MTSEntities MTS = new MTSEntities();
        Common FCommon = new Common();
        TBGMT_ICS ics = new TBGMT_ICS();
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_E12_1{getQueryString()}");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string txtOVC_ICS_NO = "";
            string txtOVC_BLD_NO = txtOvcBldNo.Text;
            string txtOVC_INLAND_CARRIAGE = txtOnbCarriage.Text;
            string txtOVC_INF_NO = txtBldSn.Text;
            DateTime txtODT_MODIFY_DATE = DateTime.Now;
            string txtOVC_CREATE_LOGIN_ID = Session["userid"].ToString();
            Guid GuidOVC_ICS_SN = Guid.NewGuid();
            Guid GuidOVC_BLD_SN = Guid.NewGuid();
            Guid GuidOVC_INF_SN = Guid.NewGuid();
            DateTime txtODT_CREATE_DATE = DateTime.Now;
            string txtOVC_CREATE_ID = Session["logid"].ToString(); ;
            string txtOVC_MODIFY_LOGIN_ID = Session["logid"].ToString(); ;

            //判斷ICR_NO ICS+年分+4位數字
            DataTable dt = new DataTable();
            var icrYear = DateTime.Now.AddYears(-1911).ToString("yyy");
            var query =
               from ics in MTS.TBGMT_ICS
               select new
               {
                   OVC_ICS_NO = ics.OVC_ICS_NO,
               };
            query = query.Where(table => table.OVC_ICS_NO.Contains("ICS" + icrYear));
            dt = CommonStatic.LinqQueryToDataTable(query);
            if (dt.Rows.Count > 0)
            {
                string temp = dt.Rows[dt.Rows.Count - 1][0].ToString().Replace("ICS", "");
                txtOVC_ICS_NO = "ICS" + (Convert.ToInt64(temp) + 1).ToString();
            }
            else
            {
                //如果!hasrow 代表是新的一筆
                txtOVC_ICS_NO = "ICS" + icrYear + "0001";  
            }
            string strMessage = "";
            if (txtOVC_BLD_NO.Equals(string.Empty))
                strMessage += "<P> 請填入 提單編號 </p>";
            if (txtOVC_INLAND_CARRIAGE.Equals(string.Empty))
                strMessage += "<P> 請填入 海空運費 </p>";
            if (txtOVC_INF_NO.Equals(string.Empty))
                strMessage += "<P> 請填入 結報申請表編號 </p>";

            Decimal decOVC_INLAND_CARRIAGE;
            bool boolOVC_INLAND_CARRIAGE = FCommon.checkDecimal(txtOVC_INLAND_CARRIAGE, "海空運費", ref strMessage, out decOVC_INLAND_CARRIAGE);
            
            if(strMessage.Equals(string.Empty))
            {
                //新增資料庫
                try
                {
                    ics.OVC_ICS_NO = txtOVC_ICS_NO;
                    ics.OVC_BLD_NO = txtOVC_BLD_NO;
                    if (boolOVC_INLAND_CARRIAGE) ics.OVC_INLAND_CARRIAGE = decOVC_INLAND_CARRIAGE; else ics.OVC_INLAND_CARRIAGE = null;
                    ics.OVC_INF_NO = txtOVC_INF_NO;
                    ics.ODT_MODIFY_DATE = txtODT_MODIFY_DATE;
                    ics.OVC_CREATE_LOGIN_ID = txtOVC_CREATE_LOGIN_ID;
                    ics.OVC_ICS_SN = GuidOVC_ICS_SN;
                    ics.OVC_BLD_SN = GuidOVC_BLD_SN;
                    ics.OVC_INF_SN = GuidOVC_INF_SN;
                    ics.ODT_CREATE_DATE = txtODT_CREATE_DATE;
                    //ics.OVC_CREATE_ID = txtOVC_CREATE_ID;
                    //ics.OVC_MODIFY_LOGIN_ID = txtOVC_MODIFY_LOGIN_ID;
                    MTS.TBGMT_ICS.Add(ics);
                    MTS.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), ics.GetType().Name.ToString(), this, "新增");
                    FCommon.AlertShow(msg, "success", "系統訊息", "新增成功!");
                }
                catch (Exception ex)
                {
                    FCommon.AlertShow(msg, "danger", "系統訊息", "新增失敗");
                }
            }
            else
                FCommon.AlertShow(msg, "danger", "系統訊息", strMessage);



        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {

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

    }
}