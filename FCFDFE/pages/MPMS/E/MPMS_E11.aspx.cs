using System;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System.Web.UI;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E11 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        string username;
        string listYear;
        int TAcquire = 0, TUnsettled = 0;
        int Year = 99;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (!IsPostBack)
                {
                    if ((string)(Session["XSSRequest"]) == "danger")
                    {
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "輸入錯誤，請重新輸入！");
                        Session["XSSRequest"] = null;
                    }

                    rdoSearchBy.SelectedIndex = 0;
                    FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                    FCommon.Controls_Attributes("readonly", "true", txtBuyCaseFrom, txtBuyCaseTo);
                    list_dataImport(drpSelectPlanYear);
                    GV_dataImport();
                    GV_tatle_dataImport();
                    GV_total.Visible = false;

                    #region SessionRemove
                    Session.Contents.Remove("CaseFrom");
                    Session.Contents.Remove("CaseTo");
                    Session.Contents.Remove("return");
                    Session.Contents.Remove("status");
                    Session.Contents.Remove("name");
                    Session.Contents.Remove("rowtext");
                    Session.Contents.Remove("E15");
                    #endregion
                }
            }
        }

        #region Click

        //統計查詢btn
        protected void btnCaculate_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_ESearch.aspx";
            Response.Redirect(send_url);
        }

        //產出購案報表btn
        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            if (txtBuyCaseFrom.Text != "" && txtBuyCaseTo.Text != "")
            {
                Session["CaseFrom"] = txtBuyCaseFrom.Text;
                Session["CaseTo"] = txtBuyCaseTo.Text;
                var dtCaseFrom = Session["CaseFrom"].ToString();
                var dtCaseTo = Session["CaseTo"].ToString();

                #region 當日期dtCaseFrom比dtCaseTo晚時，兩邊交換值
                if (dtCaseFrom.CompareTo(dtCaseTo) > 0)
                {
                    dtCaseFrom = Session["CaseTo"].ToString();
                    dtCaseTo = Session["CaseFrom"].ToString();
                }
                #endregion

                var query =
                    from tbm1301 in mpms.TBM1301
                    join tbmreceive in mpms.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmreceive.OVC_PURCH
                    where tbmreceive.OVC_STATUS.Equals("已結案")
                    where !tbmreceive.OVC_DCLOSE.Equals(null)
                    where tbmreceive.OVC_DCLOSE.CompareTo(dtCaseFrom) >= 0 && tbmreceive.OVC_DCLOSE.CompareTo(dtCaseTo) <= 0
                    orderby tbmreceive.OVC_DCLOSE descending
                    select tbm1301;

                if (query.Any())
                {
                    string send_url;
                    send_url = "~/pages/MPMS/E/MPMS_E11_1.aspx";
                    Response.Redirect(send_url);
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "找不到購案資料");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請填選 查詢條件");
        }

        //查詢購案移辦資料btn
        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            #region SessionRemove
            Session.Contents.Remove("Purchase_number");
            Session.Contents.Remove("Purchase_name");
            Session.Contents.Remove("Contract_manufacturer");
            #endregion

            if (txtSearchCondition.Text != "" && rdoSearchBy.SelectedIndex != -1)
            {
                if (rdoSearchBy.SelectedIndex.Equals(0)) Session["Purchase_number"] = txtSearchCondition.Text;
                else if (rdoSearchBy.SelectedIndex.Equals(1)) Session["Purchase_name"] = txtSearchCondition.Text;
                else if (rdoSearchBy.SelectedIndex.Equals(2)) Session["Contract_manufacturer"] = txtSearchCondition.Text;

                var query =
                        from tbm1114 in mpms.TBM1114
                        join tbm1301 in mpms.TBM1301_PLAN.AsEnumerable() on tbm1114.OVC_PURCH equals tbm1301.OVC_PURCH
                        join tbm1302 in mpms.TBM1302.AsEnumerable() on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                        where rdoSearchBy.SelectedIndex.Equals(0) ? (tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + tbm1302.OVC_PURCH_5 + tbm1302.OVC_PURCH_6).Contains(txtSearchCondition.Text) : true
                        where rdoSearchBy.SelectedIndex.Equals(1) ? tbm1301.OVC_PUR_IPURCH.Contains(txtSearchCondition.Text) : true
                        where rdoSearchBy.SelectedIndex.Equals(2) ? tbm1302.OVC_VEN_TITLE.Contains(txtSearchCondition.Text) : true
                        select tbm1114;

                if (query.Any())
                {
                    string send_url;
                    send_url = "~/pages/MPMS/E/MPMS_E11_2.aspx";
                    Response.Redirect(send_url);
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "找不到購案資料");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請填選 查詢條件");
        }

        //查詢採購目前資料btn
        protected void btnCurrently_Click(object sender, EventArgs e)
        {
            #region SessionRemove
            Session["Purchase_number"] = null;
            Session["Purchase_name"] = null;
            Session["Contract_manufacturer"] = null;
            #endregion

            if (txtSearchCondition.Text != "" && rdoSearchBy.SelectedIndex != -1)
            {
                if (rdoSearchBy.SelectedIndex.Equals(0)) Session["Purchase_number"] = txtSearchCondition.Text;
                else if (rdoSearchBy.SelectedIndex.Equals(1)) Session["Purchase_name"] = txtSearchCondition.Text;
                else if (rdoSearchBy.SelectedIndex.Equals(2)) Session["Contract_manufacturer"] = txtSearchCondition.Text;

                var query =
                        from tbmstatus in mpms.TBMSTATUS
                        join tbm1301 in mpms.TBM1301_PLAN.AsEnumerable() on tbmstatus.OVC_PURCH equals tbm1301.OVC_PURCH
                        join tbm1302 in mpms.TBM1302.AsEnumerable() on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                        where rdoSearchBy.SelectedIndex.Equals(0) ? (tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + tbm1302.OVC_PURCH_5 + tbm1302.OVC_PURCH_6).Contains(txtSearchCondition.Text) : true
                        where rdoSearchBy.SelectedIndex.Equals(1) ? tbm1301.OVC_PUR_IPURCH.Contains(txtSearchCondition.Text) : true
                        where rdoSearchBy.SelectedIndex.Equals(2) ? tbm1302.OVC_VEN_TITLE.Contains(txtSearchCondition.Text) : true
                        where tbmstatus.OVC_STATUS.Equals("3") || tbmstatus.OVC_STATUS.Equals("31") || tbmstatus.OVC_STATUS.Equals("32") || tbmstatus.OVC_STATUS.Equals("33") || tbmstatus.OVC_STATUS.Equals("34") || tbmstatus.OVC_STATUS.Equals("35") || tbmstatus.OVC_STATUS.Equals("36") || tbmstatus.OVC_STATUS.Equals("37")
                        orderby tbmstatus.OVC_STATUS
                        select tbmstatus;

                if (query.Any())
                {
                    string send_url;
                    send_url = "~/pages/MPMS/E/MPMS_E11_3.aspx";
                    Response.Redirect(send_url);
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "找不到購案資料");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請填選 查詢條件");
        }

        //收辦案btn
        protected void btnOVC_PURCH_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            Session["name"] = GV_TakeOver.Cells[0].Text;
            Session["status"] = "total";
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E14.aspx";
            Response.Redirect(send_url);
        }

        //未結案btn
        protected void btnOVC_STATUS_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            int rowindex = GV_TakeOver.RowIndex;
            Session["name"] = GV_TakeOver.Cells[0].Text;
            Session["status"] = "notyet";
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E14.aspx";
            Response.Redirect(send_url);
        }

        //年度案件總表btn
        protected void btnAll_Click(object sender, EventArgs e)
        {
            if (GV_TBMCONTRACT_MODIFY.Visible == true)
            {
                GV_TBMCONTRACT_MODIFY.Visible = false;
                GV_total.Visible = true;
            }
            else if (GV_TBMCONTRACT_MODIFY.Visible == false)
            {
                GV_TBMCONTRACT_MODIFY.Visible = true;
                GV_total.Visible = false;
            }
            GV_dataImport();
        }

        #endregion




        #region 副程式

        #region 選擇年度下拉式選單
        private void list_dataImport(ListControl list)
        {
            list.Items.Clear();
            int CalDateYear = Convert.ToInt16(DateTime.Now.Year) - 1911;
            int num = CalDateYear;
            for (int i = 0; i < 15; i++)
            {
                list.Items.Add(Convert.ToString(num));
                num = num - 1;
            }
        }
        #endregion

        #region GridView資料帶入
        private void GV_dataImport()
        {
            DataTable dt = new DataTable();
            //listYear = (int.Parse(drpSelectPlanYear.Text) + 1911).ToString();
            if (int.Parse(drpSelectPlanYear.Text) < 100)
                listYear = drpSelectPlanYear.Text;
            else
                listYear = drpSelectPlanYear.Text.Substring(1, 2);
            Session["listYear"] = listYear;

            if (Session["userid"] != null)
            {
                var id = Session["userid"].ToString();
                var queryunit = mpms.ACCOUNT.Where(o => o.USER_ID.Equals(id)).FirstOrDefault();
                Session["userunit"] = queryunit.DEPT_SN;
                var userunit = Session["userunit"].ToString();

                var queryNC =
                    from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                    join tbm1302 in mpms.TBM1302 on tbmreceive.OVC_PURCH equals tbm1302.OVC_PURCH
                    where tbmreceive.OVC_PURCH.Substring(2, 2).Equals(listYear)
                    where tbmreceive.OVC_PURCH_6.Equals(tbm1302.OVC_PURCH_6)
                    where tbmreceive.OVC_VEN_CST.Equals(tbm1302.OVC_VEN_CST)
                    where tbmreceive.ONB_GROUP.Equals(tbm1302.ONB_GROUP)
                    where tbm1302.OVC_DSEND != null
                    where tbm1302.OVC_DRECEIVE != null
                    group tbmreceive by tbmreceive.OVC_DO_NAME into g
                    select new
                    {
                        g.Key,
                        count = g.Count()
                    };
                var queryDC =
                    from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                    join tbm1302 in mpms.TBM1302 on tbmreceive.OVC_PURCH equals tbm1302.OVC_PURCH
                    where tbmreceive.OVC_PURCH.Substring(2, 2).Equals(listYear)
                    where tbmreceive.OVC_PURCH_6.Equals(tbm1302.OVC_PURCH_6)
                    where tbmreceive.OVC_VEN_CST.Equals(tbm1302.OVC_VEN_CST)
                    where tbmreceive.ONB_GROUP.Equals(tbm1302.ONB_GROUP)
                    where tbm1302.OVC_DSEND != null
                    where tbm1302.OVC_DRECEIVE != null
                    where tbmreceive.OVC_DCLOSE.Equals(null)
                    group tbmreceive by tbmreceive.OVC_DO_NAME into g
                    select new
                    {
                        g.Key,
                        count = g.Count()
                    };
                //var querytbm5200_1 =
                //    from account in mpms.ACCOUNT
                //    join tbm5200_1 in mpms.TBM5200_1 on account.USER_ID equals tbm5200_1.USER_ID
                //    join tbmreceive in mpms.TBMRECEIVE_CONTRACT on account.USER_NAME equals tbmreceive.OVC_DO_NAME
                //    join tbm1302 in mpms.TBM1302 on tbmreceive.OVC_PURCH equals tbm1302.OVC_PURCH
                //    where userunit == "0A100" || userunit == "00N00" ? account.DEPT_SN.Equals("0A100") || account.DEPT_SN.Equals("00N00") : account.DEPT_SN.Equals(userunit)
                //    where tbm5200_1.C_SN_ROLE.Equals("33")
                //    where tbm5200_1.OVC_PRIV_LEVEL.Equals("7")
                //    where tbm5200_1.OVC_ENABLE.Equals("Y")
                //    group account by account.USER_NAME into grp
                //    select grp.Key;
                //var querytbm5200_ppp =
                //    from account in mpms.ACCOUNT
                //    join TBM5200_PPP in mpms.TBM5200_PPP on account.USER_ID equals TBM5200_PPP.USER_ID
                //    where userunit == "0A100" || userunit == "00N00" ? account.DEPT_SN.Equals("0A100") || account.DEPT_SN.Equals("00N00") : account.DEPT_SN.Equals(userunit)
                //    where TBM5200_PPP.ROLE_IKIND.Equals("33")
                //    where TBM5200_PPP.OVC_PRIV_LEVEL.Equals("7")
                //    where TBM5200_PPP.OVC_ENABLE.Equals("Y")
                //    group account by account.USER_NAME into grp
                //    select grp.Key;
                var queryaccount_auth =
                    from account in mpms.ACCOUNT
                    join account_auth in mpms.ACCOUNT_AUTH on account.USER_ID equals account_auth.USER_ID
                    where userunit == "0A100" || userunit == "00N00" ? account.DEPT_SN.Equals("0A100") || account.DEPT_SN.Equals("00N00") : account.DEPT_SN.Equals(userunit)
                    where account_auth.C_SN_ROLE.Equals("34")
                    where account_auth.C_SN_AUTH.Equals("3403")
                    where account_auth.IS_ENABLE.Equals("Y")
                    group account by account.USER_NAME into grp
                    select grp.Key;
                //var queryDoName = querytbm5200_1.Union(querytbm5200_ppp).Union(queryaccount_auth);
                var query =
                    from doname in queryaccount_auth
                    join nc in queryNC on doname equals nc.Key into NC
                    from nc in NC.DefaultIfEmpty()
                    join dc in queryDC on doname equals dc.Key into DC
                    from dc in DC.DefaultIfEmpty()
                    select new
                    {
                        OVC_DO_NAME = doname,
                        OVC_PURCH = (int?)nc.count ?? 0,
                        OVC_DCLOSE = (int?)dc.count ?? 0
                    };

                dt = CommonStatic.LinqQueryToDataTable(query);
                FCommon.GridView_dataImport(GV_TBMCONTRACT_MODIFY, dt);
            }
        }
        private void GV_tatle_dataImport()
        {
            string ThisYear = DateTime.Now.ToString("yyyy");
            int TotalYear = (int.Parse(ThisYear) - 1911) - 98;
            DataTable dt = new DataTable();

            for (int i = 0; i < TotalYear; i++)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            GV_total.DataSource = dt.AsDataView();
            GV_total.DataBind();
        }
        #endregion

        #region DropDownList、GridView改變
        protected void drpSelectPlanYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            GV_TBMCONTRACT_MODIFY.Visible = true;
            GV_total.Visible = false;
            Session.Contents.Remove("return");
            GV_dataImport();
        }
        #endregion

        #region GridView
        protected void GV_TBMCONTRACT_MODIFY_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.Footer)
            {
                GridViewRow Tgvr = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Insert);
                TableCell cell1 = new TableCell();
                TableCell cell2 = new TableCell();
                TableCell cell3 = new TableCell();
                cell1.Text = "合計";
                cell2.Text = TAcquire.ToString();
                cell3.Text = TUnsettled.ToString();
                Tgvr.Controls.Add(cell1);
                Tgvr.Controls.Add(cell2);
                Tgvr.Controls.Add(cell3);

                var userunit = Session["userunit"].ToString();
                var query =
                    from tbm1302 in mpms.TBM1302
                    join tbm1301 in mpms.TBM1301_PLAN on tbm1302.OVC_PURCH equals tbm1301.OVC_PURCH
                    where tbm1302.OVC_DSEND != null && tbm1302.OVC_DSEND != " "
                    where tbm1302.OVC_DRECEIVE.Equals(null) || tbm1302.OVC_DRECEIVE.Equals(" ")
                    where userunit == "0A100" || userunit == "00N00" ? tbm1301.OVC_CONTRACT_UNIT.Equals("0A100") || tbm1301.OVC_CONTRACT_UNIT.Equals("00N00") : tbm1301.OVC_CONTRACT_UNIT.Equals(userunit)
                    select tbm1302;

                GridViewRow Ugvr = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Insert);
                TableCell cell4 = new TableCell();
                TableCell cell5 = new TableCell();
                Label llab = new Label();
                Button lbtn = new Button();
                llab.ID = "llab";
                llab.Text = query.Count() + " 案";
                lbtn.ID = "lbtn";
                lbtn.Text = query.Count() + " 案";
                if (query.Count() == 0)
                {
                    llab.Visible = true;
                    lbtn.Visible = false;
                }
                else
                {
                    llab.Visible = false;
                    lbtn.Visible = true;
                }
                lbtn.CssClass = "btn-success";
                lbtn.PostBackUrl = "~/pages/MPMS/E/MPMS_E12.aspx";

                cell4.Text = "尚未分案";
                cell5.Controls.Add(llab);
                cell5.Controls.Add(lbtn);
                cell5.ColumnSpan = 2;
                Ugvr.Controls.Add(cell4);
                Ugvr.Controls.Add(cell5);

                GV_TBMCONTRACT_MODIFY.Controls[0].Controls.Add(Tgvr);
                GV_TBMCONTRACT_MODIFY.Controls[0].Controls.Add(Ugvr);
            }
        }
        protected void GV_TBMCONTRACT_MODIFY_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label labOVC_PURCH = (Label)e.Row.FindControl("labOVC_PURCH");
                Button btnOVC_PURCH = (Button)e.Row.FindControl("btnOVC_PURCH");
                Label labOVC_STATUS = (Label)e.Row.FindControl("labOVC_STATUS");
                Button btnOVC_STATUS = (Button)e.Row.FindControl("btnOVC_STATUS");
                TAcquire += int.Parse(labOVC_PURCH.Text);
                TUnsettled += int.Parse(labOVC_STATUS.Text);
                //username = e.Row.Cells[0].Text;
                if (Session["userid"] != null && Session["return"] == null)
                {
                    username = Session["username"].ToString();
                    var quserid = Session["userid"].ToString();
                    var queryanydata =
                        from tbm5200 in mpms.TBM5200_1
                        join account in mpms.ACCOUNT on tbm5200.USER_ID equals account.USER_ID
                        where tbm5200.USER_ID.Equals(quserid)
                        where tbm5200.C_SN_ROLE.Equals("33")
                        where tbm5200.OVC_PRIV_LEVEL.Equals("3") || tbm5200.OVC_PRIV_LEVEL.Equals("8")
                        select account.USER_NAME;
            
                    if (queryanydata.Count() > 0)
                    {
                        if (labOVC_PURCH.Text != "0")
                        {
                            labOVC_PURCH.Visible = false;
                            btnOVC_PURCH.Visible = true;
                            labOVC_STATUS.Visible = false;
                            btnOVC_STATUS.Visible = true;
                            btnOVC_PURCH.Text = btnOVC_PURCH.Text + " 案";
                            btnOVC_STATUS.Text = btnOVC_STATUS.Text + " 案";
                        }
                    }
                    else if (username == e.Row.Cells[0].Text)
                    {
                        if (labOVC_PURCH.Text != "0")
                        {
                            labOVC_PURCH.Visible = false;
                            btnOVC_PURCH.Visible = true;
                            labOVC_STATUS.Visible = false;
                            btnOVC_STATUS.Visible = true;
                            btnOVC_PURCH.Text = btnOVC_PURCH.Text + " 案";
                            btnOVC_STATUS.Text = btnOVC_STATUS.Text + " 案";
                        }
                    }
                }
            }
        }

        protected void GV_total_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && Session["username"] != null)
            {
                string purchYear = Year.ToString();
                if (Year > 99)
                    purchYear = Year.ToString().Substring(1, 2);
                e.Row.Cells[0].Text = Year.ToString();
                var username = Session["username"].ToString();
                var query_1 =
                    (from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                     join tbm1302 in mpms.TBM1302 on tbmreceive.OVC_PURCH equals tbm1302.OVC_PURCH
                     where tbmreceive.OVC_PURCH.Substring(2, 2).Equals(purchYear)
                     where tbmreceive.OVC_PURCH_6.Equals(tbm1302.OVC_PURCH_6)
                     where tbmreceive.OVC_VEN_CST.Equals(tbm1302.OVC_VEN_CST)
                     where tbmreceive.ONB_GROUP.Equals(tbm1302.ONB_GROUP)
                     where tbm1302.OVC_DSEND != null
                     where tbm1302.OVC_DRECEIVE != null
                     where tbmreceive.OVC_DO_NAME.Equals(username)
                     select tbmreceive.OVC_DO_NAME).Count();
                var query_2 =
                    (from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                     join tbm1302 in mpms.TBM1302 on tbmreceive.OVC_PURCH equals tbm1302.OVC_PURCH
                     where tbmreceive.OVC_PURCH.Substring(2, 2).Equals(purchYear)
                     where tbmreceive.OVC_PURCH_6.Equals(tbm1302.OVC_PURCH_6)
                     where tbmreceive.OVC_VEN_CST.Equals(tbm1302.OVC_VEN_CST)
                     where tbmreceive.ONB_GROUP.Equals(tbm1302.ONB_GROUP)
                     where tbm1302.OVC_DSEND != null
                     where tbm1302.OVC_DRECEIVE != null
                     where tbmreceive.OVC_DO_NAME.Equals(username)
                     where tbmreceive.OVC_DCLOSE.Equals(null)
                     select tbmreceive.OVC_DO_NAME).Count();
                e.Row.Cells[1].Text = query_1.ToString();
                e.Row.Cells[2].Text = query_2.ToString();
                Year++;
            }
        }

        protected void GV_TBMCONTRACT_MODIFY_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GV_TBMCONTRACT_MODIFY.UseAccessibleHeader = true;
                GV_TBMCONTRACT_MODIFY.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void GV_total_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GV_total.UseAccessibleHeader = true;
                GV_total.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        #endregion

        #endregion
    }
}