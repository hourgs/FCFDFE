using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using System.Data.Entity;
using FCFDFE.Entity.MTSModel;


namespace FCFDFE.pages.MTS.A
{
    public partial class EDFQUERY : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        string strImagePagh = Content.Variable.strImagePath;
        MTSEntities MTSE = new MTSEntities();
        Common FCommon = new Common();
        //string[] strFieldCon = { "OVC_CON_ENG_ADDRESS", "OVC_CON_TEL", "OVC_CON_FAX" };
        //string[] strFieldNP = { "OVC_NP_ENG_ADDRESS", "OVC_NP_TEL", "OVC_NP_TEL" };
        //string[] strFieldANP = { "OVC_ANP_ENG_ADDRESS", "OVC_ANP_TEL", "OVC_ANP_FAX" };
        //string[] strFieldANP2 = { "OVC_ANP_ENG_ADDRESS2", "OVC_ANP_TEL2", "OVC_ANP_FAX2" };

        protected void Page_Load(object sender, EventArgs e)
        {
            string dept = Request.QueryString["OVC_REQ_DEPT_CDE"];

            string Type = Request.QueryString["historyType"];
            switch (Type)
            {
                case "OVC_PURCH_NO":
                    queryOVC_PURCH_NO();
                    break;
                case "OVC_SHIP_FROM":
                    queryOVC_SHIP_FROM();
                    break;
                case "CONSIGNEE":
                    queryCONSIGNEE();
                    break;
                case "NOTIFYPARTY":
                    queryNOTIFYPARTY();
                    break;
                case "ALSONOTIFYPARTY1":
                    queryALSONOTIFYPARTY1();
                    break;
                case "ALSONOTIFYPARTY2":
                    queryALSONOTIFYPARTY2();
                    break;
                case "OVC_NOTE":
                    queryOVC_NOTE();
                    break;
                default:
                    break;
            }
        }

        #region 副程式
        protected void dataImport(GridView gridView, DataTable dt, string str)
        {
            bool hasRows = dt.Rows.Count > 0;
            if (hasRows)
            {
                gridView.DataSource = dt;
                gridView.DataBind();
            }
            else
            {
                dt.Columns.Add(str);
                gridView.ShowHeaderWhenEmpty = true;
                gridView.EmptyDataText = "無資料";
                gridView.EmptyDataRowStyle.HorizontalAlign = HorizontalAlign.Center;
                gridView.EmptyDataRowStyle.VerticalAlign = VerticalAlign.Middle;
                gridView.DataSource = dt;
                gridView.DataBind();
            }
        }
        //按下查詢按鈕的查詢
        protected void btn_query(GridView gv, string dept)
        {
            string strAddress = txtAddress.Text;
            var query =
                from edf in MTSE.TBGMT_EDF
                where edf.OVC_REQ_DEPT_CDE == dept
                where edf.OVC_CON_ENG_ADDRESS.Contains(strAddress)
                select edf;
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            FCommon.GridView_dataImport(gv, dt);
        }
        
        protected void history_query(GridView gv,string datafield)
        {
            string dept = Request.QueryString["OVC_REQ_DEPT_CDE"];
            var query =
                from edf in MTSE.TBGMT_EDF
                where edf.OVC_REQ_DEPT_CDE == dept
                select edf;
            DataTable dtHistory = CommonStatic.LinqQueryToDataTable(query);
            dataImport(gv, dtHistory, datafield);
            pnBottom.Visible = false;
        }

        protected void history_query(GridView gv)
        {
            string dept = Request.QueryString["OVC_REQ_DEPT_CDE"];
            var query =
                from edf in MTSE.TBGMT_EDF
                where edf.OVC_REQ_DEPT_CDE == dept
                select edf;
            DataTable dtHistory = CommonStatic.LinqQueryToDataTable(query);
            FCommon.GridView_dataImport(gv, dtHistory);
            pnTop.Enabled = false;
        }        
        #endregion

        #region 歷史資料查詢
        protected void queryOVC_PURCH_NO()
        {
            //更改DataField
            BoundField field = (BoundField)this.gvOneCol.Columns[0];
            field.DataField = "OVC_PURCH_NO";
            history_query(gvOneCol, "OVC_PURCH_NO");
            //更改HeaderText
            gvOneCol.HeaderRow.Cells[0].Text = "案號";
        }

        protected void queryOVC_SHIP_FROM()
        {
            BoundField field = (BoundField)this.gvOneCol.Columns[0];
            field.DataField = "OVC_SHIP_FROM";
            history_query(gvOneCol, "OVC_SHIP_FROM");
            gvOneCol.HeaderRow.Cells[0].Text = "發貨單位";
        }

        protected void queryCONSIGNEE()
        {
            //history_query(gvCONSIGNEE, strFieldCon);
            history_query(gvCONSIGNEE);
        }

        protected void queryNOTIFYPARTY()
        {
            //history_query(gvNOTIFYPARTY, strFieldNP);
            history_query(gvNOTIFYPARTY);
        }

        protected void queryALSONOTIFYPARTY1()
        {
            //history_query(gvALSONOTIFYPARTY1, strFieldANP);
            history_query(gvALSONOTIFYPARTY1);
        }

        protected void queryALSONOTIFYPARTY2()
        {
            //history_query(gvALSONOTIFYPARTY2, strFieldANP2);
            history_query(gvALSONOTIFYPARTY2);
        }

        protected void queryOVC_NOTE()
        {
            BoundField field = (BoundField)this.gvOneCol.Columns[0];
            field.DataField = "OVC_NOTE";
            history_query(gvOneCol, "OVC_NOTE");
            gvOneCol.HeaderRow.Cells[0].Text = "備考";
        }
        #endregion

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string dept = Request.QueryString["OVC_REQ_DEPT_CDE"];
            string Type = Request.QueryString["historyType"];

            switch (Type)
            {
                case "CONSIGNEE":
                    //btn_query(strFieldCon, gvCONSIGNEE, dept);
                    btn_query(gvCONSIGNEE, dept);
                    break;
                case "NOTIFYPARTY":
                    //btn_query(strFieldNP,gvNOTIFYPARTY, dept);
                    btn_query(gvNOTIFYPARTY, dept);
                    break;
                case "ALSONOTIFYPARTY1":
                    //btn_query(strFieldANP,gvALSONOTIFYPARTY1, dept);
                    btn_query(gvALSONOTIFYPARTY1, dept);
                    break;
                case "ALSONOTIFYPARTY2":
                    //btn_query(strFieldANP2,gvALSONOTIFYPARTY2, dept);
                    btn_query(gvALSONOTIFYPARTY2, dept);
                    break;
                default:
                    break;
            }
        }
    }
}