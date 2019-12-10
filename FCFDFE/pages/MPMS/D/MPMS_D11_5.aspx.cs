using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.Entity;
using System.Collections.Generic;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D11_5 : System.Web.UI.Page
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
                divOVC_PURCH.Visible = true;
            }
                
        }

        protected void drpGROUP_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strGroupNum = drpGROUP.SelectedValue;
            tableGroupImport(short.Parse(strGroupNum));
            lblONB_GROUP_PRE.Text = strGroupNum;
            lblONB_GROUP_PRE_2.Text = strGroupNum;
            lblDrpSelect.Text = strGroupNum;
        }


        #region Button OnClick

        protected void btnQuery_OVC_PURCH_Click(object sender, EventArgs e)
        {
            //點擊按鈕 選擇購案
            ViewState["strOVC_PURCH"] = txtOVC_PURCH.Text;
            string strOVC_PURCH = Convert.ToString(ViewState["strOVC_PURCH"]);
            if(isPURCHASE_UNIT() )
            { 
                DataImport();
                DrpGroupImport();
                TBM1301 tb1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (tb1301 != null)
                {
                    tableGroupImport(1);
                    tableGroupImport_ICout();
                    if (tb1301.IS_PLURAL_BASIS == "Y")
                    {
                        chkIS_PLURAL_BASIS.Checked = true;
                        divContent.Visible = true;
                        lblIS_PLURAL_BASIS.Visible = false;
                    }
                    else
                    {
                        chkIS_PLURAL_BASIS.Checked = false;
                        divContent.Visible = false;
                        lblIS_PLURAL_BASIS.Visible = true;
                    }
                    chkIS_OPEN_CONTRACT.Checked = tb1301.IS_OPEN_CONTRACT == "Y" ? true : false;
                    chkIS_JUXTAPOSED_MANUFACTURER.Checked = tb1301.IS_JUXTAPOSED_MANUFACTURER == "Y" ? true : false;
                }
            }
        }


        protected void btnBack_Click(object sender, EventArgs e)
        {
            //點擊按鈕 回主畫面
            Response.Redirect("~/pages/MPMS/D/MPMS_D11.aspx");
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            //點擊按鈕 存檔
            if ( lblOVC_PURCH.Text != "")
            {
                string strOVC_PURCH = lblOVC_PURCH.Text;
                TBM1301 tb1301 = new TBM1301();
                tb1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                tb1301.OVC_PURCH = strOVC_PURCH;
                tb1301.IS_PLURAL_BASIS = chkIS_PLURAL_BASIS.Checked ? "Y" : "N";
                tb1301.IS_OPEN_CONTRACT = chkIS_OPEN_CONTRACT.Checked ? "Y" : "N";
                tb1301.IS_JUXTAPOSED_MANUFACTURER = chkIS_JUXTAPOSED_MANUFACTURER.Checked ? "Y" : "N";
                gm.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tb1301.GetType().Name.ToString(), this, "修改");
                if (tb1301.IS_PLURAL_BASIS == "Y")
                {
                    divContent.Visible = true;
                    lblIS_PLURAL_BASIS.Visible = false;
                }
                else
                {
                    divContent.Visible = false;
                    lblIS_PLURAL_BASIS.Visible = true;
                }
            }
        }



        protected void btnCancel_LEFT_Click(object sender, EventArgs e)
        {
            //分組刪除按鈕 左邊
            Button btn = (Button)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            string strOVC_PURCH = lblOVC_PURCH.Text;
            short numONB_POI_COUNT = short.Parse(gvr.Cells[1].Text);
            short numONB_GROUP_PRE = short.Parse(lblONB_GROUP_PRE.Text);
            TBM1118_2 tbm1118_2 = new TBM1118_2();
            tbm1118_2 = mpms.TBM1118_2.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH)
                                                && o.ONB_GROUP_PRE == numONB_GROUP_PRE && o.ONB_POI_ICOUNT == numONB_POI_COUNT).FirstOrDefault();
            mpms.Entry(tbm1118_2).State = EntityState.Deleted;
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1118_2.GetType().Name.ToString(), this, "刪除");
            dataGroupBudget();
            tableGroupImport(numONB_GROUP_PRE);
            tableGroupImport_ICout();
            DrpGroupImport();
            drpGROUP.SelectedValue = lblONB_GROUP_PRE.Text;
        }

        protected void btnCancel_Right_Click(object sender, EventArgs e)
        {
            //分組刪除按鈕 右邊
            Button btn = (Button)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            string strOVC_PURCH = lblOVC_PURCH.Text;
            short numONB_POI_COUNT = short.Parse(gvr.Cells[1].Text);
            short numONB_GROUP_PRE = short.Parse(lblONB_GROUP_PRE.Text);
            TBM1118_2 tbm1118_2 = new TBM1118_2();
            tbm1118_2 = mpms.TBM1118_2.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH)
                                                && o.ONB_GROUP_PRE == numONB_GROUP_PRE && o.ONB_POI_ICOUNT == numONB_POI_COUNT).FirstOrDefault();
            mpms.Entry(tbm1118_2).State = EntityState.Deleted;
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1118_2.GetType().Name.ToString(), this, "刪除");
            dataGroupBudget();
            tableGroupImport(numONB_GROUP_PRE);
            tableGroupImport_ICout();
            DrpGroupImport();
            drpGROUP.SelectedValue = lblONB_GROUP_PRE.Text;
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            //分類存檔案按鈕
            dataSaveGroup(gvONB_POI_ICOUNT_LEFT, "cbIsGroupLeft");
            dataSaveGroup(gvONB_POI_ICOUNT_Right, "cbIsGroupRight");
            //計算分組預算
            dataGroupBudget();
            tableGroupImport(short.Parse(lblONB_GROUP_PRE.Text));
            tableGroupImport_ICout();
            DrpGroupImport();
            drpGROUP.SelectedValue = lblONB_GROUP_PRE.Text;
        }


        protected void btnReset_Click(object sender, EventArgs e)
        {
            //取消全選按鈕
            ResetAll(gvONB_POI_ICOUNT_LEFT, "cbIsGroupLeft");
            ResetAll(gvONB_POI_ICOUNT_Right, "cbIsGroupRight");
        }

        private void ResetAll(GridView gv, string id)
        {
            foreach (GridViewRow rows in gv.Rows)
            {
                CheckBox cb = (CheckBox)rows.FindControl(id);
                cb.Checked = false;
            }
        }


        protected void btnSelectAll_Click(object sender, EventArgs e)
        {
            //全選按鈕
            SelectAll(gvONB_POI_ICOUNT_LEFT, "cbIsGroupLeft");
            SelectAll(gvONB_POI_ICOUNT_Right, "cbIsGroupRight");
        }

        private void SelectAll(GridView gv, string id)
        {
            foreach (GridViewRow rows in gv.Rows)
            {
                if (rows.Cells[2].Text.Equals("&nbsp;"))
                {
                    CheckBox cb = (CheckBox)rows.FindControl(id);
                    cb.Checked = true;
                }
            }
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
                        divForm.Visible = true;
                        return true;
                    }
                }
            }
            divForm.Visible = false;
            return false;
        }




        private void DataImport()
        {
            //購案資料載入  
            DataTable dt = new DataTable();

            if (isPURCHASE_UNIT())
            {
                string strOVC_PURCH = Convert.ToString(ViewState["strOVC_PURCH"]);
                TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (tbm1301 != null)
                {
                    lblOVC_PURCH.Text = tbm1301.OVC_PURCH;
                    lblOVC_PUR_IPURCH.Text = tbm1301.OVC_PUR_IPURCH;
                    lblOVC_PUR_USER.Text = tbm1301.OVC_PUR_USER;
                    lblOVC_PUR_IUSER_PHONE_EXT.Text = tbm1301.OVC_PUR_IUSER_PHONE_EXT;
                }
            }
        }



        private void tableGroupImport(short numGroup)
        {
            //項目資料載入
            DataTable tableGroup = new DataTable();
            DataTable tableLeft = new DataTable();
            DataTable tableRight = new DataTable();
            string strOVC_PURCH = Convert.ToString(ViewState["strOVC_PURCH"]);
            var query =
                from tb1118_2 in mpms.TBM1118_2
                join tb1201 in mpms.TBM1201 on new { tb1118_2.OVC_PURCH, tb1118_2.ONB_POI_ICOUNT }
                equals new { tb1201.OVC_PURCH, tb1201.ONB_POI_ICOUNT }
                where tb1118_2.OVC_PURCH.Equals(strOVC_PURCH)
                        && tb1118_2.ONB_GROUP_PRE == numGroup
                orderby tb1118_2.ONB_POI_ICOUNT
                select new
                {
                    ONB_POI_ICOUNT = tb1118_2.ONB_POI_ICOUNT,
                    OVC_POI_NSTUFF_CHN = tb1201.OVC_POI_NSTUFF_CHN,
                    OVC_BRAND = tb1201.OVC_BRAND
                };
            
            tableGroup = CommonStatic.LinqQueryToDataTable(query);
            tableLeft = tableGroup.Clone();
            tableRight = tableGroup.Clone();
            int flag = 1;
            //FCommon.GridView_dataImport(gvGroupLeft, tableGroup, poi);
            foreach (DataRow rows in tableGroup.Rows)
            {
                if (flag == 1)
                {
                    tableLeft.ImportRow(rows);
                    flag = 2;
                }
                else
                {
                    tableRight.ImportRow(rows);
                    flag = 1;
                }

            }
            var queryBudge = mpms.TBM1118_1.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH)
                                                         && o.ONB_GROUP_PRE == numGroup).ToList();
            if (queryBudge.Count > 0)
            {
                foreach (var rows in queryBudge)
                {
                    lblGRUOP_BUDGE.Text = rows.ONB_GROUP_BUDGET.ToString();
                }
            }
            else
                lblGRUOP_BUDGE.Text = "0";

            ViewState["hasRows_odd"] = FCommon.GridView_dataImport(gvGroupLeft, tableLeft);
            ViewState["hasRows_even"] = FCommon.GridView_dataImport(gvGroupRight, tableRight);
        }



        private void tableGroupImport_ICout()
        {
            DataTable dtMain = new DataTable();
            DataTable dtLeft = new DataTable();
            DataTable dtRight = new DataTable();
            string strOVC_PURCH = Convert.ToString(ViewState["strOVC_PURCH"]);
            
            var query =
                from c in mpms.TBM1201
                join o in mpms.TBM1118_2 on new { c.OVC_PURCH, c.ONB_POI_ICOUNT } equals new { o.OVC_PURCH, o.ONB_POI_ICOUNT }
                into g
                where c.OVC_PURCH.Equals(strOVC_PURCH)
                from o in g.DefaultIfEmpty()
                orderby c.ONB_POI_ICOUNT
                select new
                {
                    ONB_POI_ICOUNT = c.ONB_POI_ICOUNT,
                    ONB_GROUP_PRE = (int?)o.ONB_GROUP_PRE,
                    OVC_POI_NSTUFF_CHN = c.OVC_POI_NSTUFF_CHN,
                    OVC_BRAND = c.OVC_BRAND
                };
            dtMain = CommonStatic.LinqQueryToDataTable(query);
            dtLeft = dtMain.Clone();
            dtRight = dtMain.Clone();
            int flag = 1;
            foreach (DataRow rows in dtMain.Rows)
            {
                if (flag == 1)
                {
                    dtLeft.ImportRow(rows);
                    flag = 2;
                }
                else
                {
                    dtRight.ImportRow(rows);
                    flag = 1;
                }
            }
            FCommon.GridView_dataImport(gvONB_POI_ICOUNT_LEFT, dtLeft);
            FCommon.GridView_dataImport(gvONB_POI_ICOUNT_Right, dtRight);

        }



        private void DrpGroupImport()
        {
            string strOVC_PURCH = Convert.ToString(ViewState["strOVC_PURCH"]);
            //下拉式選單
            drpGROUP.Items.Clear();
            var querymax = mpms.TBM1118_2.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH)).Any();
            if (querymax)
            {
                var max = mpms.TBM1118_2.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH)).Max(o => o.ONB_POI_ICOUNT);
                for (int i = 1; i <= max + 1; i++)
                {
                    drpGROUP.Items.Add(i.ToString());
                }
            }
            else
            {
                drpGROUP.Items.Add("1");
            }

        }



        private void dataSaveGroup(GridView gv, string id)
        {
            string strOVC_PURCH = lblOVC_PURCH.Text;
            //存分組資料到資料庫
            foreach (GridViewRow rows in gv.Rows)
            {
                CheckBox cb = (CheckBox)rows.FindControl(id);
                if (cb.Checked)
                {
                    TBM1118_2 tbm1118_2 = new TBM1118_2();
                    tbm1118_2.OVC_PURCH = strOVC_PURCH;
                    tbm1118_2.ONB_GROUP_PRE = short.Parse(lblONB_GROUP_PRE.Text);
                    tbm1118_2.ONB_POI_ICOUNT = short.Parse(rows.Cells[1].Text);
                    mpms.TBM1118_2.Add(tbm1118_2);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1118_2.GetType().Name.ToString(), this, "新增");
                }
            }
        }


        private void dataGroupBudget()
        {
            //計算並儲存(刪除)分組預算 tbm1118_1
            string strOVC_PURCH = lblOVC_PURCH.Text;
            short numGroup = short.Parse(lblONB_GROUP_PRE.Text);
            decimal sum = 0;
            var query =
                from table in mpms.TBM1118_2
                join table2 in mpms.TBM1201
                on new { table.OVC_PURCH, table.ONB_POI_ICOUNT } equals new { table2.OVC_PURCH, table2.ONB_POI_ICOUNT }
                where table.OVC_PURCH.Equals(strOVC_PURCH) && table.ONB_GROUP_PRE == numGroup
                select new
                {
                    ONB_POI_MPRICE_PLAN = table2.ONB_POI_MPRICE_PLAN,
                    ONB_POI_QORDER_PLAN = table2.ONB_POI_QORDER_PLAN
                };
            foreach (var rows in query)
            {
                decimal money = rows.ONB_POI_MPRICE_PLAN == null? 0 : (decimal)rows.ONB_POI_MPRICE_PLAN;
                decimal q = rows.ONB_POI_QORDER_PLAN == null? 0 : (decimal)rows.ONB_POI_QORDER_PLAN;
                sum += money * q;
            }
            if (sum == 0)
            {
                //合計等於零表示要刪除
                TBM1118_1 tbm1118_1 = new TBM1118_1();
                tbm1118_1 = mpms.TBM1118_1.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH) && o.ONB_GROUP_PRE == numGroup).FirstOrDefault();
                mpms.Entry(tbm1118_1).State = EntityState.Deleted;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1118_1.GetType().Name.ToString(), this, "刪除");
            }
            else
            {
                //合計不等於零表示要新增或修改
                var isExist = mpms.TBM1118_1.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH) && o.ONB_GROUP_PRE == numGroup).ToList();
                if (isExist.Count == 0)
                {
                    //新增
                    TBM1118_1 tbm1118_1 = new TBM1118_1();
                    tbm1118_1.OVC_PURCH = strOVC_PURCH;
                    tbm1118_1.ONB_GROUP_PRE = numGroup;
                    tbm1118_1.ONB_GROUP_BUDGET = sum;
                    mpms.TBM1118_1.Add(tbm1118_1);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1118_1.GetType().Name.ToString(), this, "新增");
                }
                else
                {
                    //修改
                    TBM1118_1 tbm1118_1 = new TBM1118_1();
                    tbm1118_1 = mpms.TBM1118_1.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH)
                                                       && o.ONB_GROUP_PRE == numGroup).FirstOrDefault();
                    tbm1118_1.OVC_PURCH = strOVC_PURCH;
                    tbm1118_1.ONB_GROUP_PRE = numGroup;
                    tbm1118_1.ONB_GROUP_BUDGET = sum;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1118_1.GetType().Name.ToString(), this, "修改");
                }
            }
        }
        #endregion

    }

}