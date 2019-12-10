using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.IO;
using System.Data.Entity;

namespace FCFDFE.pages.MTS.E
{
    public partial class MTS_E14_2 : System.Web.UI.Page
    {
        bool hasRows = false;
        bool hasRows2 = false;
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        TBGMT_ICS ics = new TBGMT_ICS();
        TBGMT_BLD bld = new TBGMT_BLD();
        TBGMT_CINF cinf = new TBGMT_CINF();
        Common FCommon = new Common();
        string id;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getQueryString(this, "id", out id, true);
                if (!IsPostBack)
                {
                    string ovc_inf_no = id;
                    lblOvcInfNo.Text = ovc_inf_no;
                    var query = from cinf in MTSE.TBGMT_CINF
                                where cinf.OVC_INF_NO.Equals(ovc_inf_no)
                                select new
                                {
                                    OVC_INF_NO = ovc_inf_no,
                                    OVC_GIST = cinf.OVC_GIST,
                                    OVC_BUDGET = cinf.OVC_BUDGET,
                                    OVC_SEA_OR_AIR = cinf.OVC_SEA_OR_AIR,
                                    OVC_IMP_OR_EXP = cinf.OVC_IMP_OR_EXP,
                                    OVC_PURPOSE_TYPE = cinf.OVC_PURPOSE_TYPE,
                                    ODT_APPLY_DATE = cinf.ODT_APPLY_DATE,
                                    ONB_AMOUNT = cinf.ONB_AMOUNT,
                                    OVC_BUDGET_INF_NO = cinf.OVC_BUDGET_INF_NO,
                                    OVC_INV_NO = cinf.OVC_INV_NO,
                                    ODT_INV_DATE = cinf.ODT_INV_DATE,
                                    OVC_NOTE = cinf.OVC_NOTE,
                                    OVC_PLN_CONTENT = cinf.OVC_PLN_CONTENT,
                                };
                    DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                    var querydrpOvcGist = from cinf in MTSE.TBGMT_CINF
                                          select new
                                          {
                                              OVC_GIST = cinf.OVC_GIST,
                                          };
                    DataTable dtdrpOvcGist = CommonStatic.ListToDataTable(MTSE.TBGMT_DEPT_CLASS.ToList());
                    FCommon.list_dataImport(drpOvcGist, dtdrpOvcGist, "OVC_CLASS_NAME", "OVC_CLASS_NAME", false);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < drpOvcGist.Items.Count; i++)
                        {
                            if (dt.Rows[0][1].ToString().Contains(drpOvcGist.Items[i].Value))
                            {
                                drpOvcGist.Items[i].Selected = true;
                            }
                        }
                        for (int i = 0; i < drpOvcBudgetInfNo.Items.Count; i++)
                        {
                            if (dt.Rows[0][2].ToString().Contains(drpOvcBudgetInfNo.Items[i].Value))
                            {
                                drpOvcBudgetInfNo.Items[i].Selected = true;
                            }
                        }
                        for (int i = 0; i < drpOvcSeaOrAir.Items.Count; i++)
                        {
                            if (dt.Rows[0][2].ToString().Contains(drpOvcSeaOrAir.Items[i].Value))
                            {
                                drpOvcSeaOrAir.Items[i].Selected = true;
                            }
                        }
                        for (int i = 0; i < drpOvcImpOrExp.Items.Count; i++)
                        {
                            if (dt.Rows[0][2].ToString().Contains(drpOvcImpOrExp.Items[i].Value))
                            {
                                drpOvcImpOrExp.Items[i].Selected = true;
                            }
                        }



                        txtOvcGist.Text = dt.Rows[0][1].ToString().Substring(3);
                        OvcPurposeType.Text = dt.Rows[0][5].ToString();
                        if (dt.Rows[0][6].ToString() != string.Empty)
                            txtOdtApplyDate.Text = Convert.ToDateTime(dt.Rows[0][6]).ToString("yyyy/MM/dd");
                        txtOnbAmount.Text = dt.Rows[0][7].ToString();
                        txtOvcBudgetInfNo.Text = dt.Rows[0][8].ToString();
                        txtOvcInvNo.Text = dt.Rows[0][9].ToString();
                        if (dt.Rows[0][10].ToString() != string.Empty)
                            txtOvcInvDate.Text = Convert.ToDateTime(dt.Rows[0][10]).ToString("yyyy/MM/dd");

                        FCommon.getQueryString(this, "id", out id, true);
                        dataimport(id);

                        var query3 = from ics in MTSE.TBGMT_ICS
                                     where ics.OVC_INF_NO == null
                                     select new
                                     {
                                         OVC_BLD_NO = ics.OVC_BLD_NO,
                                     };
                        DataTable dt3 = CommonStatic.LinqQueryToDataTable(query3);

                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            if (!dt3.Rows[i][0].ToString().Equals(string.Empty))
                            {
                                chkOvcInfNo.Items.Add(dt3.Rows[i][0].ToString());
                            }
                        }
                    }

                }
            }
            
        }

        protected void dataimport(string ovc_inf_no)
        {
            var query = from ics in MTSE.TBGMT_ICS
                         where ics.OVC_INF_NO.Equals(ovc_inf_no)
                         select new
                         {
                             OVC_ICS_NO = ics.OVC_ICS_NO,
                             OVC_BLD_NO = ics.OVC_BLD_NO,
                             OVC_INLAND_CARRIAGE = ics.OVC_INLAND_CARRIAGE,
                             OVC_INF_NO = ics.OVC_INF_NO
                         };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["dt"] = dt;
            ViewState["hasRows"] = FCommon.GridView_dataImport(GridViewTBGMT_CINF, dt);
        }

        protected void linkAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkOvcInfNo.Items.Count; i++)
            {

                chkOvcInfNo.Items[i].Selected = true;
            }
        }

        protected void linkCancel_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkOvcInfNo.Items.Count; i++)
            {

                chkOvcInfNo.Items[i].Selected = false;
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {

                int txtdrpOvcInfNo = Convert.ToInt32(drpOvcBldNo.SelectedValue);
                string txtOvcInfNo2 = txBldfNo.Text;
                var query3 = from ics in MTSE.TBGMT_ICS
                             where ics.OVC_INF_NO == null
                             where ics.ODT_MODIFY_DATE.Year.Equals(txtdrpOvcInfNo + 1911)
                             select new
                             {
                                 ics.OVC_BLD_NO

                             };


                DataTable dt3 = CommonStatic.LinqQueryToDataTable(query3);

                chkOvcInfNo.Items.Clear();
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    if(!dt3.Rows[i][0].ToString().Equals(string.Empty))
                    {
                        chkOvcInfNo.Items.Add(dt3.Rows[i][0].ToString());
                    }
                }
            }
            catch 
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "過濾失敗!!");
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string temp = "";
                for (int i = 0; i < chkOvcInfNo.Items.Count; i++)
                {
                    temp = chkOvcInfNo.Items[i].Value;
                    if (chkOvcInfNo.Items[i].Selected == true)
                    {
                        cinf = MTSE.TBGMT_CINF.Where(table => table.OVC_INF_NO.Equals(lblOvcInfNo.Text)).FirstOrDefault();
                        ics = MTSE.TBGMT_ICS.Where(table => table.OVC_BLD_NO.Equals(temp)).FirstOrDefault();
                        ics.OVC_INF_NO = lblOvcInfNo.Text;

                        MTSE.SaveChanges();
                        var query2 = from ics in MTSE.TBGMT_ICS
                                     where ics.OVC_BLD_NO.Equals(temp)
                                     select new
                                     {
                                         OVC_ICS_NO = ics.OVC_ICS_NO,
                                         OVC_BLD_NO = ics.OVC_BLD_NO,
                                         OVC_INLAND_CARRIAGE = ics.OVC_INLAND_CARRIAGE,
                                         OVC_INF_NO = ics.OVC_INF_NO
                                     };
                        DataTable dt2 = CommonStatic.LinqQueryToDataTable(query2);
                        DataTable newdt = (DataTable)ViewState["dt"];
                        DataRow row = newdt.NewRow();
                        row[0] = dt2.Rows[0][0].ToString();
                        row[1] = dt2.Rows[0][1].ToString();
                        row[2] = dt2.Rows[0][2].ToString();
                        row[3] = dt2.Rows[0][3].ToString();
                        newdt.Rows.Add(row);
                        ViewState["hasRows"] = FCommon.GridView_dataImport(GridViewTBGMT_CINF, newdt);
                        chkOvcInfNo.Items.RemoveAt(i);
                        txtOnbAmount.Text = (Convert.ToInt32(txtOnbAmount.Text) + Convert.ToInt32(dt2.Rows[0][2])).ToString();
                        try
                        {
                            cinf.ONB_AMOUNT = Convert.ToDecimal(txtOnbAmount.Text);
                        }
                        catch
                        {
                            cinf.ONB_AMOUNT = Convert.ToDecimal(null);
                        }
                        
                        MTSE.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(),Request.ServerVariables["REMOTE_ADDR"].ToString(), cinf.GetType().Name.ToString(), this, "新增");

                    }

                }
            }
            catch{
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "新增失敗!");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string Massage = "";
            string strODT_APPLY_DATE = txtOdtApplyDate.Text;
            string strOnbAmount = txtOnbAmount.Text;
            string strOvcInvDate = txtOvcInvDate.Text;
            DateTime dateODT_APPLY_DATE, dateOvcInvDate;
            decimal decOnbAmount;
            bool boolOvcInvDate = FCommon.checkDateTime(strODT_APPLY_DATE, "結報申請日期", ref Massage, out dateOvcInvDate);
            bool boolOnbAmount = FCommon.checkDecimal(strOnbAmount, "結報申請日期", ref Massage, out decOnbAmount);
            bool boolODT_APPLY_DATE = FCommon.checkDateTime(strOvcInvDate, "結報申請日期", ref Massage, out dateODT_APPLY_DATE);
            try
            {
                cinf = MTSE.TBGMT_CINF.Where(table => table.OVC_INF_NO.Equals(lblOvcInfNo.Text)).FirstOrDefault();
                cinf.OVC_GIST = drpOvcGist.SelectedValue.ToString()+" "+ txtOvcGist.Text;
                cinf.OVC_BUDGET = drpOvcBudgetInfNo.SelectedValue.ToString();
                cinf.OVC_SEA_OR_AIR = drpOvcSeaOrAir.SelectedValue.ToString();
                cinf.OVC_IMP_OR_EXP= drpOvcImpOrExp.SelectedValue.ToString();
                cinf.OVC_PURPOSE_TYPE = OvcPurposeType.Text;
                cinf.ODT_APPLY_DATE = dateODT_APPLY_DATE;
                cinf.ONB_AMOUNT = decOnbAmount;
                cinf.OVC_BUDGET_INF_NO = txtOvcBudgetInfNo.Text;
                cinf.OVC_INV_NO = txtOvcInvNo.Text;
                cinf.ODT_INV_DATE = dateOvcInvDate;
                cinf.OVC_NOTE = lstOvcNote.Text;
                MTSE.SaveChanges();
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), cinf.GetType().Name.ToString(), this, "修改");
            }
            catch 
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", Massage);
            }
        }

        protected string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "strtxtovcinfno", Request.QueryString["strtxtovcinfno"], false);
            FCommon.setQueryString(ref strQueryString, "strtxtovcbldno", Request.QueryString["strtxtovcbldno"], false);
            FCommon.setQueryString(ref strQueryString, "strdrpovcinfno", Request.QueryString["strdrpovcinfno"], false);
            return strQueryString;

        }
        protected void GridViewTBGMT_CINF_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {

                GridViewTBGMT_CINF.UseAccessibleHeader = true;
                GridViewTBGMT_CINF.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        protected void GV_PreRender(object sender, EventArgs e)
        {
            if (hasRows2)
            {

                GV.UseAccessibleHeader = true;
                GV.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }


        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_E14_1.aspx{getQueryString()}");
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            if (txtOnbAmount.Text != "")
            {
                Panel_main.Visible = false;
                Panel_BudgetInfNo.Visible = true;
                var query = from tdc in MTSE.TBGMT_DEPT_CLASS
                            select new
                            {
                                OVC_CLASS_NAME = tdc.OVC_CLASS_NAME
                            };
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                FCommon.list_dataImport(Mil_type, dt, "OVC_CLASS_NAME", "OVC_CLASS_NAME", true);
                int y = DateTime.Now.Year - 1911 + 10;
                for (int i = 93; i <= y; i++)
                {
                    Year.Items.Add(i.ToString());
                }
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "金額欄位不可為空!");
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            int y = Convert.ToInt32(Year.SelectedValue)+1911;
            var query = from budget in MTSE.TBGMT_BUDGET
                        where budget.OVC_MILITARY_TYPE.Contains(Mil_type.SelectedValue)
                        where budget.OVC_EXPENSE_TYPE.Equals(qExpense_TYPE.SelectedValue)
                        where budget.PRE_REMAIN > 0
                        where budget.ODT_BUD_DATE.Value.Year == y
                        select new
                        {
                            OVC_BUD_NO = budget.OVC_BUD_NO,
                            OVC_MILITARY_TYPE = budget.OVC_MILITARY_TYPE,
                            OVC_EXPENSE_TYPE = budget.OVC_EXPENSE_TYPE,
                            OVC_BUDGET_TYPE = budget.OVC_BUDGET_TYPE,
                            OVC_PURPOSE_TYPE = budget.OVC_PURPOSE_TYPE,
                            ONB_BUD_AMOUNT = budget.ONB_BUD_AMOUNT,
                            ODT_BUD_DATE = budget.ODT_BUD_DATE,
                            REMAIN = budget.REMAIN,
                            PRE_REMAIN = budget.PRE_REMAIN,
                        };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows2"] = FCommon.GridView_dataImport(GV, dt);

        }

        protected void GridViewTBGMT_CINF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            //Guid cinf = (Guid)GV_TBGMT_CINF.DataKeys[gvrIndex].Value;
            string ovc_bld_no = GridViewTBGMT_CINF.DataKeys[gvrIndex].Value.ToString();
            int onbamount= Convert.ToInt32(GridViewTBGMT_CINF.Rows[gvrIndex].Cells[2].Text);
            string ovc_inf_no = GridViewTBGMT_CINF.Rows[gvrIndex].Cells[3].Text;
            switch (e.CommandName)
            {
                case "btnDel":
                    try
                    {
                        TBGMT_ICS ics = new TBGMT_ICS();
                        ics = MTSE.TBGMT_ICS.Where(table => table.OVC_BLD_NO == ovc_bld_no).FirstOrDefault();
                        MTSE.Entry(ics).State = EntityState.Deleted;
                        MTSE.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(),Request.ServerVariables["REMOTE_ADDR"].ToString(), ics.GetType().Name.ToString(), this, "刪除");


                        txtOnbAmount.Text = (Convert.ToInt32(txtOnbAmount.Text) - onbamount).ToString();
                        TBGMT_CINF cinf = new TBGMT_CINF();
                        cinf = MTSE.TBGMT_CINF.Where(table => table.OVC_INF_NO == ovc_inf_no).FirstOrDefault();
                        cinf.ONB_AMOUNT = Convert.ToDecimal(txtOnbAmount.Text);
                        MTSE.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), cinf.GetType().Name.ToString(), this, "修改");


                    }
                    catch 
                    {
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "刪除失敗!!");
                    }
                    break;
                default:
                    break;
            }

        }
        protected void GV_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string bud_no= GridViewTBGMT_CINF.Rows[gvrIndex].Cells[0].Text;
            switch (e.CommandName)
            {
                case "btnChoose":
                    Panel_main.Visible = true;
                    txtOvcBudgetInfNo.Text = bud_no;
                    Panel_BudgetInfNo.Visible = false;
                    break;
                default:
                    break;
            }
        }
    }
}