using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.CIMSModel;

namespace FCFDFE.pages.CIMS.E
{
    public partial class CIMS_E12_1 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        private CIMSEntities CIMS = new CIMSEntities();
        Common FCommon = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            DataTable dt = new DataTable();
            var query =
                from com in CIMS.COMMONLINK.AsEnumerable()
                where com.CL_LAN == "軍"
                select new
                {
                    com.CL_SN,          //GUID碼’
                    com.CL_LAN,         //網域'
                    com.CL_TITLE,       //連結名稱'
                    com.CL_LINK,        //超連結'
                    com.CL_DESC,        //內容概述'
                    com.CL_UPLOADLINK,  //網站說明'
                    com.CL_KIND,        //種類'(對應TBM1407 ovc_phr_cate='CL')
                    com.CL_ORD,         //排序'
                };
            dt = CommonStatic.LinqQueryToDataTable(query);

            DataList1.DataSource = dt;
            DataList1.DataBind();

        }
    }
}