using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.CIMSModel;

namespace FCFDFE.pages.CIMS.C
{
    public partial class CIMS_C12_3 : System.Web.UI.Page
    {        
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        CIMSEntities CIMS = new CIMSEntities();
        TBM_PUBLIC_BID codetable = new TBM_PUBLIC_BID();
        ACCOUNT account = new ACCOUNT();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["OVC_KEY"] == null)
            {
                Response.Write("<script>alert('系統檢測到您未依照正確方式進入此頁面，將導至登入畫面!'); location.href='../../../logout.aspx'; </script>");
                return;
            }
            string id = Request.QueryString["OVC_KEY"];
            string user;
            if (!IsPostBack)
            {
                codetable = CIMS.TBM_PUBLIC_BID.Where(table => table.OVC_KEY == id).FirstOrDefault();
                
                if (codetable != null)
                {
                    OVC_A.Text = codetable.OVC_PUB_KIND;
                    OVC_B.Text = codetable.OVC_BID_SEC;
                    OVC_C.Text = codetable.OVC_PURCH;
                    OVC_D.Text = codetable.OVC_NPURCH;
                    OVC_E.Text = codetable.OVC_PURCH_KIND;
                    OVC_F.Text = codetable.ONB_BUDGET_MONEY.ToString();
                    OVC_G.Text = codetable.OVC_BUDGET_RANGE;
                    OVC_H.Text = codetable.OVC_BID_KIND;
                    OVC_I.Text = codetable.OVC_DBID_KIND;
                    OVC_J.Text = codetable.ONB_UNDER_MONEY.ToString();
                    OVC_K.Text = codetable.OVC_OPEN_UNDER;
                    OVC_L.Text = codetable.ONB_DBID_MONEY.ToString();
                    ONB_M.Text = codetable.OVC_OPEN_DBID;
                    OVC_N.Text = codetable.ONB_VEN_NUM.ToString();
                    OVC_O.Text = codetable.OVC_VEN_NAME;
                    OVC_P.Text = codetable.OVC_VEN_CST;
                    ONB_Q.Text = codetable.OVC_LAW58;
                    OVC_R.Text = codetable.OVC_LIMIT_RULE;
                    if(codetable.OVC_PUBLISH_DATE!=null)
                        OVC_S.Text = Convert.ToDateTime(codetable.OVC_PUBLISH_DATE).ToString(Variable.strDateFormat); 
                    if(codetable.OVC_DBID_DATE!=null)
                        ONB_T.Text = Convert.ToDateTime(codetable.OVC_DBID_DATE).ToString(Variable.strDateFormat); 
                    ONB_U.Text = codetable.ONB_UNDER_PLUS_BUDGET.ToString();
                    ONB_V.Text = codetable.ONB_DBID_PLUS_UNDER.ToString();
                    OVC_W.Text = codetable.ONB_DBID_PLUS_BUDGET.ToString();
                    if(codetable.OVC_DBID_DATE_IN!=null)
                        OVC_X.Text = Convert.ToDateTime(codetable.OVC_DBID_DATE_IN).ToString(Variable.strDateFormat); 
                    OVC_Y.Text = codetable.ONB_JOIN_VEN.ToString();
                    OVC_Z.Text = codetable.ONB_SERIAL.ToString();
                    OVC_AA.Text = codetable.OVC_NOGET_VEN;
                    OVC_AB.Text = codetable.ONB_VEN_BID_MONEY.ToString();
                    if(codetable.OVC_OPEN_DATE!=null)
                        ONB_AD.Text = Convert.ToDateTime(codetable.OVC_OPEN_DATE.ToString()).ToString(Variable.strDateFormat); ;
                    
                }
            }
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            Response.Write("<script language='javascript'>window.close();</" + "script>");
        }
    }
}