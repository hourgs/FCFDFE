using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Content;
using System.Globalization;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;

namespace FCFDFE.pages.CIMS.E
{
    public partial class CIMS_E11 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        private GMEntities gme = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        Common FCommon = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtovc_aq_s, txtovc_aq_e);
                    pnQuery.Visible = false;
                }
            }
                
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            
            string strMessage = "";
            string strOVC_POI_NSTUFF_CHN = txtOVC_POI_NSTUFF_CHN.Text;
            string strNSN = txtNSN.Text;
            string strOVC_POI_IREF = txtOVC_POI_IREF.Text;
            string strOVC_VEN_TITLE = txtOVC_VEN_TITLE.Text;
            string strOVC_PUR_IPURCH = txtOVC_PUR_IPURCH.Text;
            string strOVC_PURCH = txtOVC_PURCH.Text;
            string strSTART = txtovc_aq_s.Text;
            string strEND = txtovc_aq_e.Text;

            if (strOVC_POI_NSTUFF_CHN.Equals(string.Empty)
                && strNSN.Equals(string.Empty)
                && strOVC_POI_IREF.Equals(string.Empty)
                && strOVC_VEN_TITLE.Equals(string.Empty)
                && strOVC_PUR_IPURCH.Equals(string.Empty)
                && strOVC_PURCH.Equals(string.Empty)
                && strSTART.Equals(string.Empty)
                && strEND.Equals(string.Empty))
            {
                strMessage += "<P> 請至少輸入一項條件 </p>";
            }
            if (strMessage.Equals(string.Empty))
            {
                pnQuery.Visible = true;
                OnStartData();
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }
        #region 副程式
        ICollection CreateDataSource()
        {
            string strOVC_POI_NSTUFF_CHN = txtOVC_POI_NSTUFF_CHN.Text;
            string strNSN = txtNSN.Text;
            string strOVC_POI_IREF = txtOVC_POI_IREF.Text;
            string strOVC_VEN_TITLE = txtOVC_VEN_TITLE.Text;
            string strOVC_PUR_IPURCH = txtOVC_PUR_IPURCH.Text;
            string strOVC_PURCH = txtOVC_PURCH.Text;
            string strSTART = txtovc_aq_s.Text;
            string strEND = txtovc_aq_e.Text;
            int i = 1;

            // Create sample data for the DataList control.
            DataTable dt = new DataTable();
            DataRow dr;

            // Define the columns of the table.
            dt.Columns.Add(new DataColumn("OVC_PURCH", typeof(String)));
            dt.Columns.Add(new DataColumn("OVC_PUR_AGENCY", typeof(String)));
            dt.Columns.Add(new DataColumn("NSN", typeof(String)));
            dt.Columns.Add(new DataColumn("OVC_POI_IREF", typeof(String)));
            dt.Columns.Add(new DataColumn("ONB_POI_QORDER_CONT", typeof(String)));
            dt.Columns.Add(new DataColumn("OVC_POI_IUNIT", typeof(String)));
            dt.Columns.Add(new DataColumn("ONB_POI_MPRICE_CONT", typeof(String)));
            dt.Columns.Add(new DataColumn("OVC_VEN_CST", typeof(String)));
            dt.Columns.Add(new DataColumn("OVC_PURCH_6", typeof(String)));
            dt.Columns.Add(new DataColumn("ONB_POI_ICOUNT", typeof(String)));
            dt.Columns.Add(new DataColumn("OVC_POI_NSTUFF_CHN", typeof(String)));
            dt.Columns.Add(new DataColumn("OVC_PUR_IPURCH", typeof(String)));
            dt.Columns.Add(new DataColumn("OVC_DBID", typeof(String)));
            dt.Columns.Add(new DataColumn("OVC_VEN_TITLE", typeof(String)));
            dt.Columns.Add(new DataColumn("OVC_NUMBER", typeof(String)));
            dt.Columns.Add(new DataColumn("OVC_PURCH_URL", typeof(String)));
            dt.Columns.Add(new DataColumn("OVC_VEN_CST_URL", typeof(String)));
            dt.Columns.Add(new DataColumn("OVC_PURCH_6_URL", typeof(String)));
            dt.Columns.Add(new DataColumn("ONB_POI_ICOUNT_URL", typeof(String)));
            dt.Columns.Add(new DataColumn("QUOTE_PRICE", typeof(String))); //報價折幅
            dt.Columns.Add(new DataColumn("MARKED_PRICE", typeof(String))); //標價折幅

            ////ESQL
            //using (MPMSEntities mpms = new MPMSEntities())
            //{
            //    string entitySQL = "SELECT Value a FROM ";
            //}
            var querySys =
               from tb1201 in mpms.TBM1201
               join tb1301 in mpms.TBM1301 on tb1201.OVC_PURCH equals tb1301.OVC_PURCH
               //join tb1301 in gme.TBM1301 on tb1201.OVC_PURCH equals tb1301.OVC_PURCH
               join tb1302 in mpms.TBM1302 on tb1201.OVC_PURCH equals tb1302.OVC_PURCH
               select new
               {
                   OVC_PURCH = tb1201.OVC_PURCH,
                   OVC_PUR_AGENCY = tb1301.OVC_PUR_AGENCY,
                   //OVC_PUR_AGENCY = tb1201.ONB_POI_ICOUNT,
                   NSN = tb1201.NSN,
                   OVC_POI_IREF = tb1201.OVC_POI_IREF,
                   ONB_POI_QORDER_CONT = tb1201.ONB_POI_QORDER_CONT,
                   OVC_POI_IUNIT = tb1201.OVC_POI_IUNIT,
                   ONB_POI_MPRICE_CONT = tb1201.ONB_POI_MPRICE_CONT,
                   OVC_VEN_CST = tb1302.OVC_VEN_CST,
                   OVC_PURCH_6 = tb1302.OVC_PURCH_6,
                   ONB_POI_ICOUNT = tb1201.ONB_POI_ICOUNT,
                   OVC_POI_NSTUFF_CHN = tb1201.OVC_POI_NSTUFF_CHN,
                   OVC_PUR_IPURCH = tb1301.OVC_PUR_IPURCH,
                   //OVC_PUR_IPURCH = tb1201.OVC_POI_NSTUFF_CHN,
                   OVC_DBID =tb1302.OVC_DBID,
                   OVC_VEN_TITLE = tb1302.OVC_VEN_TITLE
                   //ODT_DATE = tb1302.OVC_DBID,
               };
            if (!strOVC_POI_NSTUFF_CHN.Equals(string.Empty))
                querySys = querySys.Where(table => table.OVC_POI_NSTUFF_CHN.Contains(strOVC_POI_NSTUFF_CHN));
            if (!strNSN.Equals(string.Empty))
                querySys = querySys.Where(table => table.NSN.Contains(strNSN));
            if (!strOVC_POI_IREF.Equals(string.Empty))
                querySys = querySys.Where(table => table.OVC_POI_IREF.Contains(strOVC_POI_IREF));
            if (!strOVC_VEN_TITLE.Equals(string.Empty))
                querySys = querySys.Where(table => table.OVC_VEN_TITLE.Contains(strOVC_VEN_TITLE));
            if (!strOVC_PUR_IPURCH.Equals(string.Empty))
                querySys = querySys.Where(table => table.OVC_PUR_IPURCH.Contains(strOVC_PUR_IPURCH));
            if (!strOVC_PURCH.Equals(string.Empty))
                querySys = querySys.Where(table => table.OVC_PURCH.Contains(strOVC_PURCH));
            if (!strSTART.Equals(string.Empty))
                querySys = querySys.Where(table => String.Compare(table.OVC_DBID, strSTART) >= 0);
            if (!strEND.Equals(string.Empty))
                querySys = querySys.Where(table => String.Compare(table.OVC_DBID, strEND) <= 0);
            // Populate the table with sample values.
            foreach (var aa in querySys)
            {
                dr = dt.NewRow();
                dr[0] = aa.OVC_PURCH;
                dr[1] = aa.OVC_PUR_AGENCY;
                dr[2] = aa.NSN;
                dr[3] = aa.OVC_POI_IREF;
                dr[4] = aa.ONB_POI_QORDER_CONT;
                dr[5] = aa.OVC_POI_IUNIT;
                dr[6] = aa.ONB_POI_MPRICE_CONT;
                dr[7] = aa.OVC_VEN_CST;
                dr[8] = aa.OVC_PURCH_6;
                dr[9] = aa.ONB_POI_ICOUNT;
                dr[10] = aa.OVC_POI_NSTUFF_CHN;
                dr[11] = aa.OVC_PUR_IPURCH;
                dr[12] = Convert.ToDateTime(aa.OVC_DBID).ToString(Variable.strDateFormat); //
                dr[13] = aa.OVC_VEN_TITLE;
                dr[14] = i.ToString();
                string ovcpurch = aa.OVC_PURCH;
                string ovcpurchkey = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ovcpurch));
                string ovcvencst = aa.OVC_VEN_CST;
                string ovcvencstkey = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ovcvencst));
                string ovcpurch6 = aa.OVC_PURCH_6;
                string ovcpurch6key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ovcpurch6));
                string onbpoiicount = aa.ONB_POI_ICOUNT.ToString();
                string onbpoiicountkey = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(onbpoiicount));
                dr[15] = "CIMS_E11_1.aspx?OVC_PURCH=" + ovcpurchkey;
                if (ovcpurch == "NODATA")
                    dr[16] = "";
                else
                    dr[16] = "CIMS_E11_2.aspx?OVC_VEN_CST=" + ovcvencst;
                dr[17] = "CIMS_E11_3.aspx?OVC_PURCH=" + ovcpurchkey + "&OVC_PURCH_6=" + ovcpurch6key;
                dr[18] = "CIMS_E11_4.aspx?OVC_PURCH=" + ovcpurchkey + "&ONB_POI_ICOUNT=" + onbpoiicountkey;
                dr[19] = "0%";//報價折幅
                dr[20] = "0%";//標價折幅
                dt.Rows.Add(dr);
                i++;
            }
            DataView dv = new DataView(dt);
            return dv;
        }

        public void OnStartData()
        {
            // Manually register the event-handling method for the 
            // ItemCommand event.
            ItemsList.ItemCreated +=
                new DataListItemEventHandler(this.Item_Created);
            // Load sample data only once, when the page is first loaded.
            ItemsList.DataSource = CreateDataSource();
            ItemsList.DataBind();
        }



        void Item_Created(Object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
            }
        }
        #endregion
        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtNSN.Text = string.Empty;
            txtovc_aq_e.Text = string.Empty;
            txtovc_aq_s.Text = string.Empty;
            txtOVC_POI_IREF.Text = string.Empty;
            txtOVC_POI_NSTUFF_CHN.Text = string.Empty;
            txtOVC_PURCH.Text = string.Empty;
            txtOVC_PUR_IPURCH.Text = string.Empty;
            txtOVC_VEN_TITLE.Text = string.Empty;
        }
    }
}