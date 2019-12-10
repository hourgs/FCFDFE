using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.CIMSModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;

namespace FCFDFE.pages.CIMS.F
{
    public partial class CIMS_F12 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        CIMSEntities CIMS = new CIMSEntities();
        MPMSEntities MPMS = new MPMSEntities();
        string[] strField = { "RANK" , "OVC_VEN_CST" , "OVC_NVEN" , "OVC_VEN_TITLE" , "PERFORM_NAME" , "OVC_VEN_ITEL" , "OVC_VEN_ADDRESS" };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
        }

        protected void GV_TBM1203_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void GV_TBM1203_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBM1203.DataKeys[gvrIndex].Value.ToString();
            switch (e.CommandName)
            {
                case "OVC_VEN_CST":
                    dataimport(id);
                    Detail.Visible = true;
                    search.Visible = false;
                    return;
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            showGV_TBM1203();
            GV_TBM1203.Visible = true;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

        }

        private void showGV_TBM1203()
        {
            DataTable dt = new DataTable();
            var query =
                from TBM1203 in MPMS.TBM1203.DefaultIfEmpty().AsEnumerable()
                select new
                {
                    OVC_VEN_CST= TBM1203.OVC_VEN_CST??"",
                    OVC_NVEN=TBM1203.OVC_NVEN??"",
                    OVC_VEN_TITLE=TBM1203.OVC_VEN_TITLE??"",
                    PERFORM_NAME=TBM1203.PERFORM_NAME??"",
                    OVC_VEN_ITEL=TBM1203.OVC_VEN_ITEL??"",
                    OVC_VEN_ADDRESS=TBM1203.OVC_VEN_ADDRESS??""
                };
            if (txtOVC_VEN_TITLE_query.Text != "")
                query = query.Where(table => table.OVC_VEN_TITLE.Contains(txtOVC_VEN_TITLE_query.Text));
            if(txtOVC_NVEN_query.Text != "")
                query = query.Where(table => table.OVC_NVEN.Contains(txtOVC_NVEN_query.Text));
            if(txtOVC_VEN_CST_query.Text != "")
                query = query.Where(table => table.OVC_VEN_CST.Contains(txtOVC_VEN_CST_query.Text));

            dt = CommonStatic.LinqQueryToDataTable(query);
            DataColumn column = new DataColumn();
            column.ColumnName = "RANK";
            column.DataType = System.Type.GetType("System.Int32");
            dt.Columns.Add(column);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Rank"] = i + 1;
            }
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBM1203, dt, strField);
        }

        protected void Return_Click(object sender, EventArgs e)
        {
            Detail.Visible = false;
            search.Visible = true;
        }

        private void dataimport(string id)
        {
            var query =
                (from TBM1203 in MPMS.TBM1203
                 where TBM1203.OVC_VEN_CST.Equals(id)
                 select TBM1203).FirstOrDefault();
            if (query != null)
            {
                txtOVC_VEN_CST.Text = query.OVC_VEN_CST;
                txtOVC_VEN_TITLE.Text = query.OVC_VEN_TITLE;
                txtOVC_NVEN.Text = query.OVC_NVEN;
                txtOVC_VEN_ADDRESS.Text = query.OVC_VEN_ADDRESS;
                txtOVC_VEN_ADDRESS_1.Text = query.OVC_VEN_ADDRESS_1;
                txtOVC_PUR_CREATE.Text = query.OVC_PUR_CREATE;
                txtOVC_VEN_ITEL.Text = query.OVC_VEN_ITEL;
                txtOVC_FAX_NO.Text = query.OVC_FAX_NO;
                txtOVC_BOSS.Text = query.OVC_BOSS+"/"+query.PERFORM_NAME;
                txtGINGE_VEN_CST.Text = query.GINGE_VEN_CST;
                txtCAGE.Text = query.CAGE;
                txtCAGE_DATE.Text = query.CAGE_DATE;
                txtOVC_DMANAGE_BEGIN.Text = query.OVC_DMANAGE_BEGIN;
                txtOVC_DMANAGE_END.Text = query.OVC_DMANAGE_END;
                txtOVC_DRECOVERY.Text = query.OVC_DRECOVERY;
                txtOVC_MAIN_PRODUCT.Text = query.OVC_MAIN_PRODUCT;
            }
        }


    }
}