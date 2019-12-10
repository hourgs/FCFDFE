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

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F13_6 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTS = new MTSEntities();
        Guid id;
        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_INS_TYPE", Request.QueryString["OVC_INS_TYPE"], false);
            return strQueryString;
            //在接收頁面加入此副程式
        }
        protected void dataImport()
        {
            DataTable dt = new DataTable();
            var query =
                from discount in MTS.TBGMT_CARRIAGE_DISCOUNT
                where discount.CARRD_SN == id
                select new
                {
                    discount.OVC_CARR_NAME,
                    discount.OVC_CARR_RATE,
                    discount.OVC_CARR_TYPE,
                    discount.ONB_CARRD_SORT,
                };
            dt = CommonStatic.LinqQueryToDataTable(query);

            DataRow dr = dt.Rows[0];
            txtOvcCarrName.Text = dr["OVC_CARR_NAME"].ToString();
            txtOvcCarrRate.Text = dr["OVC_CARR_RATE"].ToString();
            drpOvcCarrType.SelectedValue = dr["OVC_CARR_TYPE"].ToString();
            txtOvcCarrdSort.Text = dr["ONB_CARRD_SORT"].ToString();

            ViewState["Name"] = dr["OVC_CARR_NAME"].ToString();
            ViewState["Sort"] = dr["ONB_CARRD_SORT"].ToString();
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (FCommon.getQueryString(this, "id", out string strID, true))
                {
                    id = new Guid(strID);
                    if (!IsPostBack)
                    {
                        dataImport();
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strOvcCarrName = txtOvcCarrName.Text;
            string strOvcCarrRate = txtOvcCarrRate.Text;
            string strOvcCarrSort = txtOvcCarrdSort.Text;
            string strOvcCarrType = drpOvcCarrType.SelectedValue;
            Decimal Rate, Sort, Type;
            bool boolRate = FCommon.checkDecimal(strOvcCarrRate, "運費百分比", ref strMessage, out Rate);
            bool boolSort = FCommon.checkDecimal(strOvcCarrSort, "排序", ref strMessage, out Sort);
            bool boolType = FCommon.checkDecimal(strOvcCarrType, "運費型態", ref strMessage, out Type);
            int if_exist_sort = MTS.TBGMT_CARRIAGE_DISCOUNT.Where(table => table.ONB_CARRD_SORT == Sort).Count();
            int if_name = MTS.TBGMT_CARRIAGE_DISCOUNT.Where(table => table.OVC_CARR_NAME.Equals(strOvcCarrName)).Count();
            #region
            if (strOvcCarrName.Equals(string.Empty)) strMessage += "<p>請輸入 運費名稱！</p>";
            if (if_name > 0)
            {
                if (ViewState["Name"].ToString().Equals(strOvcCarrName))
                {

                }
                else strMessage += "<p>運費名稱 已重複</p>";
            }
            if (strOvcCarrRate.Equals(string.Empty)) strMessage += "<p>請輸入 運費百分比！</p>";
            if (Rate < 0) strMessage += "<p>運費百分比請輸入正數</p>";
            if (strOvcCarrSort.Equals(string.Empty)) strMessage += "<p>請輸入 排序！</p>";
            if (Sort < 0) strMessage += "<p>排序請輸入正數</p>";
            if (if_exist_sort > 0)
            {
                if (ViewState["Sort"].ToString().Equals(strOvcCarrSort)) {
                }
                else strMessage += "<p>排序 已重複！</p>";
            }
            if (strOvcCarrType.Equals(string.Empty)) strMessage += "<p>請選擇 運費型態！</p>";
#endregion
            if (strMessage.Equals(string.Empty))
            {
                TBGMT_CARRIAGE_DISCOUNT carriage_discount = MTS.TBGMT_CARRIAGE_DISCOUNT.Where(ot => ot.CARRD_SN == id).FirstOrDefault();
                if (carriage_discount != null)
                {
                    carriage_discount.OVC_CARR_NAME = strOvcCarrName;
                    carriage_discount.OVC_CARR_RATE = strOvcCarrRate;
                    carriage_discount.OVC_CARR_TYPE = Type;
                    carriage_discount.ONB_CARRD_SORT = Sort;
                    carriage_discount.ODT_MODIFY_DATE = DateTime.Now;
                    carriage_discount.OVC_MODIFY_LOGIN_ID = Session["userid"].ToString();
                    MTS.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), carriage_discount.GetType().Name.ToString(), this, "修改");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "更新成功");
                    dataImport();
                }
                else FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無符合資料!!");
            }
            else FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        protected void btnDel_Click(object sender, EventArgs e)
        {
            TBGMT_CARRIAGE_DISCOUNT carriage_discount = MTS.TBGMT_CARRIAGE_DISCOUNT.Where(ot => ot.CARRD_SN == id).FirstOrDefault();
            if (carriage_discount != null)
            {
                carriage_discount = MTS.TBGMT_CARRIAGE_DISCOUNT.Where(ot => ot.CARRD_SN == id).FirstOrDefault();
                MTS.Entry(carriage_discount).State = EntityState.Deleted;
                MTS.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), carriage_discount.GetType().Name.ToString(), this, "刪除");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
            }
            else FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無符合資料!!");
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F13_1{ getQueryString() }");
        }
    }
}