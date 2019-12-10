using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D11_4 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if ((string)(Session["XSSRequest"]) == "danger")
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "輸入錯誤，請重新輸入！");
                    Session["XSSRequest"] = null;
                }

                divForm.Visible = true;
            }
                
        }



        protected void gvTBM1118_1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Enter")
            {
                //資料儲存 TBM1118_1 (購案分組預算檔)
                GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
                int gvrIndex = gvr.RowIndex;
                string strOVC_PURCH = lblOVC_PURCH.Text;
                short numONB_GROUP_PRE = short.Parse(gvTBM1118_1.Rows[gvrIndex].Cells[2].Text);
                DropDownList drpONB_GROUP = (DropDownList)gvTBM1118_1.Rows[gvrIndex].FindControl("drpONB_GROUP");
                
                TBM1118_1 tb1118_1 = new TBM1118_1();
                tb1118_1 = mpms.TBM1118_1.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.ONB_GROUP_PRE == numONB_GROUP_PRE).FirstOrDefault();
                tb1118_1.OVC_PURCH = strOVC_PURCH;
                tb1118_1.ONB_GROUP_PRE = numONB_GROUP_PRE;
                if (drpONB_GROUP.SelectedValue == "")
                    tb1118_1.ONB_GROUP = null;

                else
                    tb1118_1.ONB_GROUP = short.Parse(drpONB_GROUP.SelectedValue);

                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tb1118_1.GetType().Name.ToString(), this, "修改");


                Label lblONB_GROUP = (Label)gvTBM1118_1.Rows[gvrIndex].FindControl("lblONB_GROUP");
                Label lblSaved = (Label)gvTBM1118_1.Rows[gvrIndex].FindControl("lblSaved");
                lblONB_GROUP.Text = drpONB_GROUP.SelectedValue;
                lblSaved.Text = "已儲存：";
                lblONB_GROUP.Visible = true;
                lblSaved.Visible = true;
            }
        }



        #region Button OnClick
        protected void btnQuery_OVC_PURCH_Click(object sender, EventArgs e)
        {
            //點擊 選擇購案
            ViewState["strOVC_PURCH"] = txtOVC_PURCH.Text;
            DataImport();          
        }


        protected void btnBack_Click(object sender, EventArgs e)
        {
            //點擊 回主畫面
            Response.Redirect("~/pages/MPMS/D/MPMS_D11.aspx");
        }
        #endregion



        #region 副程式
        private bool isPURCHASE_UNIT()
        {
            //檢查使用者是否為該購案採購發包的部門
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strOVC_PURCH = Convert.ToString(ViewState["strOVC_PURCH"]);
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

                    if (strOVC_PURCH == "")
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
                        divContent.Visible = true;
                        return true;
                    }
                }
            }
            divContent.Visible = false;
            return false;
        }


        private void DataImport()
        {
            string strOVC_PURCH = Convert.ToString(ViewState["strOVC_PURCH"]);
            DataTable dt = new DataTable();

            if (isPURCHASE_UNIT())
            {
                TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (tbm1301 != null)
                {
                    //divContent.Visible = true;
                    lblOVC_PURCH.Text = tbm1301.OVC_PURCH;
                    lblOVC_PUR_IPURCH.Text = tbm1301.OVC_PUR_IPURCH;
                    lblOVC_PUR_USER.Text = tbm1301.OVC_PUR_USER;
                    lblOVC_PUR_IUSER_PHONE_EXT.Text = tbm1301.OVC_PUR_IUSER_PHONE_EXT;

                    if (tbm1301.IS_PLURAL_BASIS == "Y")
                    {
                        //複數決標
                        lblSubtitle.Text = "請選擇跟計劃編制對應之採包分組代號";
                    }
                    else if (tbm1301.IS_PLURAL_BASIS == "N")
                    {
                        //非複數決標
                        lblSubtitle.Text = "本案非複數決標案";
                    }

                    var queryGvTBM1118_1 =
                        from tb1118_1 in mpms.TBM1118_1.DefaultIfEmpty().AsEnumerable()
                        where tb1118_1.OVC_PURCH.Equals(strOVC_PURCH)
                        orderby tb1118_1.ONB_GROUP_PRE
                        select new
                        {
                            OVC_PURCH = tb1118_1.OVC_PURCH,
                            ONB_GROUP = tb1118_1.ONB_GROUP,
                            ONB_GROUP_PRE = tb1118_1.ONB_GROUP_PRE,
                            ONB_GROUP_BUDGET = tb1118_1.ONB_GROUP_BUDGET,
                        };
                    dt = CommonStatic.LinqQueryToDataTable(queryGvTBM1118_1);
                    ViewState["hasRows_tb1118_1"] = FCommon.GridView_dataImport(gvTBM1118_1, dt);

                    var queryGvTBM1201 =
                        from tb1201 in mpms.TBM1201.DefaultIfEmpty()
                        where tb1201.OVC_PURCH.Equals(strOVC_PURCH)
                        join tb1118_2 in mpms.TBM1118_2 on new { tb1201.OVC_PURCH, tb1201.ONB_POI_ICOUNT } equals new { tb1118_2.OVC_PURCH, tb1118_2.ONB_POI_ICOUNT } into tbGroup1
                        from tb1118_2 in tbGroup1.DefaultIfEmpty()
                        orderby tb1201.ONB_POI_ICOUNT
                        select new
                        {
                            OVC_PURCH = tb1201.OVC_PURCH,
                            ONB_POI_ICOUNT = tb1201.ONB_POI_ICOUNT,
                            ONB_GROUP_PRE = (int?)tb1118_2.ONB_GROUP_PRE,
                            OVC_POI_NSTUFF_CHN = tb1201.OVC_POI_NSTUFF_CHN,
                            OVC_BRAND = tb1201.OVC_BRAND,
                            OVC_MODEL = tb1201.OVC_MODEL,
                        };
                    var oddGvTBM1201 = queryGvTBM1201.ToList().Where((c, i) => i % 2 == 0);
                    var evenGvTBM1201 = queryGvTBM1201.ToList().Where((c, i) => i % 2 != 0);
                    dt = CommonStatic.LinqQueryToDataTable(oddGvTBM1201);
                    FCommon.GridView_dataImport(gvTBM1201_odd, dt);
                    dt = CommonStatic.LinqQueryToDataTable(evenGvTBM1201);
                    FCommon.GridView_dataImport(gvTBM1201_even, dt);


                    //SetDrpONB_GROUP
                    var query = mpms.TBM1118_1;
                    DataTable dtONB_GROUP = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PURCH.Equals(strOVC_PURCH)));
                    foreach (GridViewRow rows in gvTBM1118_1.Rows)
                    {
                        Label lblONB_GROUP = (Label)rows.FindControl("lblONB_GROUP");
                        DropDownList drpONB_GROUP = (DropDownList)rows.FindControl("drpONB_GROUP");
                        FCommon.list_dataImport(drpONB_GROUP, dtONB_GROUP, "ONB_GROUP_PRE", "ONB_GROUP_PRE", true);
                        drpONB_GROUP.SelectedValue = lblONB_GROUP.Text;
                    }
                }
            }
        }
        
        #endregion
    }
}