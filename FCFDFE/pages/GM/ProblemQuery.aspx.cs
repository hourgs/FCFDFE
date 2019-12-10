using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using System.Web.UI;
using System.IO;
using Word = Microsoft.Office.Interop.Word;
using FCFDFE.Entity.GMModel;
using NPOI.XWPF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;
using System.Web;
using System.Collections;

namespace FCFDFE.pages.GM
{
    public partial class ProblemQuery : Page
    {
        DateTime time = new DateTime();
        Common FCommon = new Common();
        GMEntities de = new GMEntities();
        TBM1407 tableSys = new TBM1407();
        PROBLEM_DATA pbdt = new PROBLEM_DATA();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ArrayList yearList = new ArrayList();
                DataTable dtC_SN_SYS = CommonStatic.ListToDataTable(de.TBM1407.Where(table => table.OVC_PHR_CATE == "SA").ToList());
                FCommon.list_dataImport(drpC_SN_SYS, dtC_SN_SYS, "OVC_PHR_DESC", "OVC_PHR_ID", true);
                for(int i = 91; i <= DateTime.Now.Year-1911; i++)
                {
                    yearList.Add(i.ToString());
                }
                foreach (object List in yearList)
                {
                    YEARLIST.Items.Add(new ListItem(List.ToString(), List.ToString()));
                }
                
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string strMessage = "";
            string strODT_CREATE_YEAR = YEARLIST.SelectedItem.ToString();
            string strOVC_CLAIM_NO = txtOVC_CLAIM_NO.Text;
            string strC_SN_SYS = drpC_SN_SYS.SelectedValue;

            if (strODT_CREATE_YEAR.Equals(string.Empty) && strOVC_CLAIM_NO.Equals(string.Empty) && strC_SN_SYS.Equals(string.Empty))
                strMessage += "<P> 至少填入一個選項 </p>";

            if (strMessage.Equals(string.Empty))
            {
                var query =
                    from PROBLEM_DATA in de.PROBLEM_DATA.AsEnumerable()
                    join USER in de.ACCOUNTs.AsEnumerable() on PROBLEM_DATA.OVC_CREATE_ID equals USER.USER_ID 
                    join t1407 in de.TBM1407.AsEnumerable() on PROBLEM_DATA.PROSYS_SN equals t1407.OVC_PHR_ID
                    where t1407.OVC_PHR_CATE == "SA"
                    select new
                    {
                        PRO_SN = PROBLEM_DATA.PRO_SN.ToString(),
                        PRO_ID = PROBLEM_DATA.PRO_ID,
                        PROSYS_CH = t1407.OVC_PHR_DESC,
                        USER_NAME = USER.USER_NAME,
                        ODT_CREATE_DATE = PROBLEM_DATA.ODT_CREATE_DATE,
                        PROSYS_SN = PROBLEM_DATA.PROSYS_SN
                    };
                if (!strODT_CREATE_YEAR.Equals(string.Empty))
                    query = query.Where(table => Convert.ToInt32(Convert.ToDateTime(table.ODT_CREATE_DATE).ToString("yyyy")) - 1911 == Convert.ToInt32(strODT_CREATE_YEAR));
                if (!strOVC_CLAIM_NO.Equals(string.Empty))
                    query = query.Where(table => table.PRO_ID.Equals(strOVC_CLAIM_NO));
                if (!strC_SN_SYS.Equals(string.Empty))
                    query = query.Where(table => table.PROSYS_SN.Equals(strC_SN_SYS));

                dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GVPRO_DATA, dt);

                ViewState["strODT_CREATE_YEAR"] = strODT_CREATE_YEAR;
                ViewState["strOVC_CLAIM_NO"] = strOVC_CLAIM_NO;
                ViewState["strC_SN_SYS"] = strC_SN_SYS;
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
        }

        protected void GVPRO_DATA_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string proid = GVPRO_DATA.DataKeys[gvrIndex].Value.ToString();

            switch (e.CommandName)
            {
                case "btnDetail":
                    Response.Redirect("ProblemDetail.aspx?PRO_SN=" + proid);
                    break;
                default:
                    break;
            }
        }

        protected void GVPRO_DATA_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}