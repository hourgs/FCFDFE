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
    public partial class CIMS_C12_2 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        CIMSEntities CIMS = new CIMSEntities();
        TBM_PUBLIC_BID_99 codetable = new TBM_PUBLIC_BID_99();
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
                codetable = CIMS.TBM_PUBLIC_BID_99.Where(table => table.OVC_KEY == id).FirstOrDefault();

                if (codetable != null)
                {
                    OVC_A.Text = codetable.OVC_A;
                    OVC_B.Text = codetable.OVC_B;
                    OVC_C.Text = codetable.OVC_C;
                    OVC_D.Text = codetable.OVC_D;
                    OVC_E.Text = codetable.OVC_E;
                    OVC_F.Text = codetable.OVC_F;
                    OVC_G.Text = codetable.OVC_G;
                    OVC_H.Text = codetable.OVC_H;
                    OVC_I.Text = codetable.OVC_I;
                    OVC_J.Text = codetable.OVC_J;
                    OVC_K.Text = codetable.OVC_K;
                    OVC_L.Text = codetable.OVC_L;
                    ONB_M.Text = codetable.ONB_M.ToString();
                    if (codetable.OVC_N != null)
                        OVC_N.Text = Convert.ToDateTime(codetable.OVC_N).ToString(Variable.strDateFormat);
                    if (codetable.OVC_O != null)
                        OVC_O.Text = Convert.ToDateTime(codetable.OVC_O).ToString(Variable.strDateFormat);
                    OVC_P.Text = codetable.OVC_P;
                    ONB_Q.Text = codetable.ONB_Q.ToString();
                    OVC_R.Text = codetable.OVC_R;
                    OVC_S.Text = codetable.OVC_S;
                    ONB_T.Text = codetable.ONB_T.ToString();
                    ONB_U.Text = codetable.ONB_U.ToString();
                    ONB_V.Text = codetable.ONB_V.ToString();
                    OVC_W.Text = codetable.OVC_W;
                    OVC_X.Text = codetable.OVC_X;
                    OVC_Y.Text = codetable.OVC_Y;
                    OVC_Z.Text = codetable.OVC_Z;
                    OVC_AA.Text = codetable.OVC_AA;
                    OVC_AB.Text = codetable.OVC_AB;
                    OVC_AC.Text = codetable.OVC_AC;
                    ONB_AD.Text = codetable.ONB_AD.ToString();
                    OVC_AE.Text = codetable.OVC_AE;
                    OVC_AF.Text = codetable.OVC_AF;
                    ONB_AG.Text = codetable.ONB_AG.ToString();
                    OVC_AH.Text = codetable.OVC_AH;
                    ONB_AI.Text = codetable.ONB_AI.ToString();
                    OVC_AJ.Text = codetable.OVC_AJ;
                    OVC_AK.Text = codetable.OVC_AK;
                    OVC_AL.Text = codetable.OVC_AL;
                    OVC_AM.Text = codetable.OVC_AM;
                    OVC_AN.Text = codetable.OVC_AN;
                    ONB_AO.Text = codetable.ONB_AO.ToString();
                    OVC_AP.Text = codetable.OVC_AP;
                    if (codetable.OVC_AQ != null)
                        OVC_AQ.Text = Convert.ToDateTime(codetable.OVC_AQ).ToString(Variable.strDateFormat);
                    if (codetable.OVC_AR != null)
                        OVC_AR.Text = Convert.ToDateTime(codetable.OVC_AR).ToString(Variable.strDateFormat);
                    OVC_AS.Text = codetable.OVC_AS;
                    OVC_AT.Text = codetable.OVC_AT;
                    OVC_AU.Text = codetable.OVC_AU;
                    ONB_AV.Text = codetable.ONB_AV.ToString();
                    OVC_AW.Text = codetable.OVC_AW;
                    OVC_AX.Text = codetable.OVC_AX;
                    OVC_AY.Text = codetable.OVC_AY;
                    OVC_AZ.Text = codetable.OVC_AZ;
                    ONB_BA.Text = codetable.ONB_BA.ToString();
                    OVC_BB.Text = codetable.OVC_BB;
                    OVC_BC.Text = codetable.OVC_BC;
                    OVC_BD.Text = codetable.OVC_BD;
                    OVC_BE.Text = codetable.OVC_BE;
                    ONB_BF.Text = codetable.ONB_BF.ToString();
                    OVC_BG.Text = codetable.OVC_BG;
                    OVC_BH.Text = codetable.OVC_BH;
                    ONB_BI.Text = codetable.ONB_BI.ToString();
                    OVC_KEY.Text = codetable.OVC_KEY;
                }
            }
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            Response.Write("<script language='javascript'>window.close();</" + "script>");
        }
    }
}