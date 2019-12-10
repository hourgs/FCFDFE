using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;
using System.Globalization;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D11_1 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strOVC_PURCH, strAGENCY="";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (FCommon.getQueryString(this, "OVC_PURCH", out strOVC_PURCH, false))
                {
                    TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strAGENCY = tbm1301 == null? "" : tbm1301.OVC_PUR_AGENCY;
                    lblOVC_PURCH_A.Text = strOVC_PURCH + strAGENCY;

                    if (!IsPostBack)
                        DataImport();   //將資料帶入畫面
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('錯誤訊息：未輸入購案編號');", true);
            }
        }


        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Write("<script language='javascript'>window.close();</script>");
        }


        #region 副程式
        private void DataImport()
        {
            if (isPURCHASE_UNIT())
            { 
                DataTable dt = new DataTable();
                var query =
                    from tb1114 in mpms.TBM1114
                    where tb1114.OVC_PURCH.Equals(strOVC_PURCH)
                    orderby tb1114.OVC_DATE
                    select new
                    {
                        OVC_PURCH_A = strOVC_PURCH + strAGENCY,
                        OVC_DATE = tb1114.OVC_DATE,
                        OVC_USER = tb1114.OVC_USER,
                        OVC_FROM_UNIT_NAME = tb1114.OVC_FROM_UNIT_NAME,
                        OVC_TO_UNIT_NAME = tb1114.OVC_TO_UNIT_NAME,
                        OVC_REMARK = tb1114.OVC_REMARK
                    };
                dt = CommonStatic.LinqQueryToDataTable(query);
                ViewState["hasRows"] = FCommon.GridView_dataImport(gvShiftDo, dt);
            }
        }

        private bool isPURCHASE_UNIT()
        {
            //檢查使用者是否為該購案採購發包的部門
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strUserName, strDept = "";
            DataTable dt = new DataTable();
            if (strUSER_ID.Length > 0)
            {
                ACCOUNT ac = new ACCOUNT();
                ac = gm.ACCOUNTs.Where(tb => tb.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                if (ac != null)
                {
                    strUserName = ac.USER_NAME.ToString();
                    strDept = ac.DEPT_SN;
                    TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();

                    if (strOVC_PURCH=="")
                        strErrorMsg = "請輸入購案編號";
                    else if (tbm1301 == null)
                        strErrorMsg = "查無此購案編號";
                    else
                    {
                        var queryOvcPurch =
                            (from tbm1301Plan in gm.TBM1301_PLAN
                             where tbm1301Plan.OVC_PURCHASE_UNIT.Equals(strDept)
                                 && tbm1301Plan.OVC_PURCH.Equals(strOVC_PURCH)
                             join tbm1301_1 in gm.TBM1301 on tbm1301Plan.OVC_PURCH equals tbm1301_1.OVC_PURCH
                             select tbm1301Plan).ToList();

                        if (queryOvcPurch.Count == 0)
                            strErrorMsg = "非此購案的採購發包部門";
                    }

                    if (strErrorMsg != "")
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErrorMsg);

                    else
                    {
                        gvShiftDo.Visible = true;
                        return true;
                    }
                }
            }
            gvShiftDo.Visible = false;
            return false;
        }



        #endregion

    }
}