using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MPMS.B
{
    public partial class TRANSFORM : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string[] strField = { "OVC_PURCH", "OVC_PURCH_6", "OVC_PUR_IPURCH", /**/"OVC_VEN_TITLE", "OVC_DATE", "OVC_USER", "OVC_FROM_UNIT_NAME", "OVC_TO_UNIT_NAME", "OVC_REMARK" };
        public string strMenuName = "", strMenuNameItem = "";
        string PurchNum;
        protected void Page_Load(object sender, EventArgs e)
        {
            //FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            if (!IsPostBack)
            {
                ViewState["OVC_PURCH"] = Request.QueryString["OVC_PURCH"];
                OVC_Purch.Text = ViewState["OVC_PURCH"].ToString();
            }


            dataImport();
        }
        #region 副程式


        protected void GV_OVC_BUDGET_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void GV_OVC_BUDGET_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
            {
                //要隱藏的欄位    
                //e.Row.Cells[1].Visible = false;
            }
        }

        private void dataImport()
        {
            //Nullable<int> NUL = new int32();
            string purch = ViewState["OVC_PURCH"].ToString();
            //var query1114 = mpms.TBM1114.Where(o => o.OVC_PURCH.Equals(purch)).ToArray();
            //var query =
            //    from tb1114 in query1114
            //    join tb1301 in gm.TBM1301 on tb1114.OVC_PURCH equals tb1301.OVC_PURCH into lj
            //    from tb1301 in lj.DefaultIfEmpty()
            //    join tb1302 in mpms.TBM1302 on tb1114.OVC_PURCH equals tb1302.OVC_PURCH into lj1
            //    from tb1302 in lj1.DefaultIfEmpty()
            //    join tb1313 in mpms.TBM1313 on tb1114.OVC_PURCH equals tb1313.OVC_PURCH into lj2
            //    from tb1313 in lj2.DefaultIfEmpty()
            //    where tb1114.OVC_PURCH.Equals(purch)
            //    select new
            //    {
            //        OVC_PURCH = tb1114.OVC_PURCH,
            //        OVC_PURCH_6 = (tb1302 == null) ? "" : tb1302.OVC_PURCH_6,
            //        OVC_PUR_IPURCH = tb1301.OVC_PUR_IPURCH,
            //        OVC_VEN_TITLE = (tb1313 == null) ? null : tb1313.OVC_VEN_TITLE,
            //        OVC_DATE = tb1114.OVC_DATE,
            //        OVC_USER = tb1114.OVC_USER,
            //        OVC_FROM_UNIT_NAME = tb1114.OVC_FROM_UNIT_NAME,
            //        OVC_TO_UNIT_NAME = tb1114.OVC_TO_UNIT_NAME,
            //        OVC_REMARK = tb1114.OVC_REMARK
            //    }
            //    ;
            //DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            string[] strParameterName = { ":Ovc_Purch"};
            ArrayList aryData = new ArrayList();
            aryData.Add(purch);
            string strSQL = "";
            strSQL += $@"Select a.OVC_PURCH, c.OVC_PURCH_6 ,b.OVC_PUR_IPURCH ,d.OVC_VEN_TITLE,a.OVC_DATE,a.OVC_USER,a.OVC_FROM_UNIT_NAME,a.OVC_TO_UNIT_NAME,a.OVC_REMARK
                        From Tbm1114 A, Tbm1301 B, Tbm1302 C ,Tbm1313 D
                        Where A.Ovc_Purch =B.Ovc_Purch(+)
                        And A.Ovc_Purch = C.Ovc_Purch(+)
                        And A.Ovc_Purch =D.Ovc_Purch(+)
                        And a.Ovc_Purch ={strParameterName[0]}";

            DataTable dt = FCommon.getDataTableFromSelect(strSQL, strParameterName, aryData);

            ViewState["hasRows"] = FCommon.GridView_dataImport(TBM1114, dt, strField);
        }
        #endregion

        //protected void btnclose_Click()
        //{

        //}
    }
}