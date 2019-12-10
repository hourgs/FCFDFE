using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.Entity;

namespace FCFDFE.pages.Sup
{
    public partial class Super_IP_BLACKLIST : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string[] strField = { "OVC_PURCH", "OVC_PUR_IPURCH", "OVC_AGENT_UNIT", "OVC_PUR_USER", "OVC_DAPPLY", "OVC_DAUDIT", "OVC_PURCH_OK" };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                DataImport();
        }

        protected void GV_BlackList_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void BL_ADD_Click(object sender, EventArgs e)
        {
            BlackList_Query.Visible = false;
            BlackList_Add.Visible = true;
        }

        protected void GV_BlackList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            Guid id = new Guid(GV_BlackList.DataKeys[gvrIndex].Value.ToString());

            switch (e.CommandName)
            {
                case "DataDel":
                    BLACKLIST bl = new BLACKLIST();
                    bl = gm.BLACKLISTs.Where(table => table.BL_SN == id).FirstOrDefault();
                    gm.Entry(bl).State = EntityState.Deleted;
                    gm.SaveChanges();
                    DataImport();
                    break;
                default:
                    break;
            }
        }

        protected void query_Click(object sender, EventArgs e)
        {
            DataImport();
        }

        protected void add_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strIP = txtIP_add.Text.ToString();
            string strREASON = txtREASON.Text.ToString();
            if (strIP == "")
                strMessage += "<p> 請輸入 欲封鎖IP或是網段 </p>";
            if (strREASON == "")
                strMessage += "<p> 請輸入 封鎖理由 </p>";

            if (strMessage.Equals(string.Empty))
            {
                BLACKLIST bLACKLIST = new BLACKLIST();
                bLACKLIST.BL_SN= Guid.NewGuid();
                bLACKLIST.BL_IP = strIP;
                bLACKLIST.BL_CREATEDATE = DateTime.Now;
                bLACKLIST.BL_REASON = strREASON;
                bLACKLIST.BL_STATUS = 1;
                gm.BLACKLISTs.Add(bLACKLIST);
                gm.SaveChanges();
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功!");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }

        protected void return_Click(object sender, EventArgs e)
        {
            BlackList_Query.Visible = true;
            BlackList_Add.Visible = false;
        }

        private void DataImport()
        {
            string ip = txtIP.Text.ToString();
            string sdate = txt_SDATE.Text.ToString();
            string edate = txt_EDATE.Text.ToString();
            var query =
                from BL in gm.BLACKLISTs
                orderby BL.BL_CREATEDATE descending
                select new
                {
                    SN = BL.BL_SN,
                    IP = BL.BL_IP,
                    DATE = BL.BL_CREATEDATE,
                    REASON = BL.BL_REASON,
                };
            if (ip != "")
                query = query.Where(x => x.IP.Contains(ip));
            if (sdate != "")
            {
                DateTime Date1 = Convert.ToDateTime(sdate);
                query = query.Where(table => table.DATE >= Date1);
            }
            if (edate != "")
            {
                DateTime Date2 = Convert.ToDateTime(edate);
                query = query.Where(table => table.DATE <= Date2);
            }
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_BlackList, dt);
            GV_BlackList.Visible = true;
        }

    }
}