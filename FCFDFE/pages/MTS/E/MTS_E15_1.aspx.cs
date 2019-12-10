using FCFDFE.Content;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;

namespace FCFDFE.pages.MTS.E
{
    public partial class MTS_E15_1 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    var query = (from dept in MTSE.TBGMT_DEPT_CLASS
                                 orderby dept.ONB_SORT
                                 select dept).ToList();

                    DataTable dtdrpOvcMilitaryType = CommonStatic.ListToDataTable(query);
                    FCommon.list_dataImport(drpOvcMilitaryType, dtdrpOvcMilitaryType, "OVC_CLASS_NAME", "OVC_CLASS_NAME", false);
                    drpOvcMilitaryType.Items.Add("不限定");
                    for (int i = 0; i < drpOvcMilitaryType.Items.Count; i++)
                    {
                        if (drpOvcMilitaryType.Items[i].Value == "不限定")
                        {
                            drpOvcMilitaryType.Items[i].Selected = true;
                        }
                    }
                    FCommon.Controls_Attributes("readonly", "true", txtOdtApplyDate);
                    if (Session["gv"] != null && Request.QueryString["return"] != null)
                    {
                        FCommon.GridView_dataImport(GV_TBGMT_CINF, (DataTable)Session["gv"]);
                        //Session["gv"] = null;
                    }

                    bool boolImport = false;
                    string strOdtApplyDate, strOvcMilitaryType, strOvcCompany, strOvcIsPaid, strOvcInfNo, strOvcImpOrExp;
                    if (FCommon.getQueryString(this, "OdtApplyDate", out strOdtApplyDate, true))
                    {
                        txtOdtApplyDate.Text = strOdtApplyDate;
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OvcMilitaryType", out strOvcMilitaryType, false))
                    {
                        FCommon.list_setValue(drpOvcMilitaryType, strOvcMilitaryType);
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OvcCompany", out strOvcCompany, true))
                    {
                        FCommon.list_setValue(drpOvcCompany, strOvcCompany);
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OvcIsPaid", out strOvcIsPaid, true))
                    {
                        FCommon.list_setValue(drpOvcIsPaid, strOvcIsPaid);
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OvcInfNo", out strOvcInfNo, true))
                    {
                        txtOvcInfNo.Text = strOvcInfNo;
                        boolImport = true;
                    }
                    if (FCommon.getQueryString(this, "OvcImpOrExp", out strOvcImpOrExp, true))
                    {
                        FCommon.list_setValue(drpOvcImpOrExp, strOvcImpOrExp);
                        boolImport = true;
                    }
                    if (boolImport)
                    {
                        dataimport(strOvcInfNo);
                    }

                }

            }
            
        }

        private string getQueryString()
        {

            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OdtApplyDate", ViewState["OdtApplyDate"], true);
            FCommon.setQueryString(ref strQueryString, "OvcMilitaryType", ViewState["OvcMilitaryType"], false);
            FCommon.setQueryString(ref strQueryString, "OvcCompany", ViewState["OvcCompany"], true);
            FCommon.setQueryString(ref strQueryString, "OvcIsPaid", ViewState["OvcIsPaid"], true);
            FCommon.setQueryString(ref strQueryString, "OvcInfNo", ViewState["OvcInfNo"], true);
            FCommon.setQueryString(ref strQueryString, "OvcImpOrExp", ViewState["OvcImpOrExp"], true);
            return strQueryString;
        }
        protected void dataimport(string ovc_inf_no)
        {
            if (txtOvcInfNo.Equals(string.Empty))
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請輸入 結報申請表編號");
            else
            {
                var query2 = from cinf in MTSE.TBGMT_CINF
                             select new
                             {
                                 OVC_INF_NO = cinf.OVC_INF_NO,
                                 OVC_GIST = cinf.OVC_GIST,
                                 OVC_BUDGET = cinf.OVC_BUDGET,
                                 OVC_PURPOSE_TYPE = cinf.OVC_PURPOSE_TYPE,
                                 ONB_AMOUNT = cinf.ONB_AMOUNT,
                                 OVC_BUDGET_INF_NO = cinf.OVC_BUDGET_INF_NO,
                                 OVC_INV_NO = cinf.OVC_INV_NO,
                                 OVC_IS_PAID = cinf.OVC_IS_PAID,
                                 OVC_NOTE = cinf.OVC_NOTE,
                                 OVC_IMP_OR_EXP = cinf.OVC_IMP_OR_EXP,
                                 ODT_APPLY_DATE = cinf.ODT_APPLY_DATE

                             };
                
                query2 = query2.Where(table => table.OVC_INF_NO.Contains(txtOvcInfNo.Text));
                
                string strOdtApplyDate = txtOdtApplyDate.Text;
                if (!strOdtApplyDate.Equals(String.Empty))
                {
                    DateTime OdtApplyDate = Convert.ToDateTime(strOdtApplyDate);
                    query2 = query2.Where(table => table.ODT_APPLY_DATE == OdtApplyDate);

                }
                if (!drpOvcMilitaryType.SelectedValue.Equals("不限定"))
                {
                    query2 = query2.Where(table => table.OVC_GIST.Contains(drpOvcMilitaryType.SelectedValue));

                }
                if (!drpOvcCompany.SelectedValue.Equals("不限定"))
                {
                    query2 = query2.Where(table => table.OVC_NOTE.Contains(drpOvcCompany.SelectedValue));
                }
                if (!drpOvcIsPaid.SelectedValue.Equals("不限定"))
                {
                    query2 = query2.Where(table => table.OVC_IS_PAID.Contains(drpOvcIsPaid.SelectedValue));
                    Session["drpOvcIsPaid"] = drpOvcIsPaid.SelectedValue;
                }
                if (!drpOvcImpOrExp.SelectedValue.Equals("不限定"))
                {
                    query2 = query2.Where(table => table.OVC_IMP_OR_EXP.Contains(drpOvcImpOrExp.SelectedValue));
                    Session["drpOvcImpOrExp"] = drpOvcImpOrExp.SelectedValue;
                }

                ViewState["OdtApplyDate"] = txtOdtApplyDate.Text;
                ViewState["OvcMilitaryType"] = drpOvcMilitaryType.SelectedValue;
                ViewState["OvcCompany"] = drpOvcCompany.SelectedValue;
                ViewState["OvcIsPaid"] = drpOvcIsPaid.SelectedValue;
                ViewState["OvcInfNo"] = txtOvcInfNo.Text;
                ViewState["OvcImpOrExp"] = drpOvcImpOrExp.SelectedValue;


                if (query2.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query2 = query2.Take(1000);
                DataTable dt = CommonStatic.LinqQueryToDataTable(query2);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_CINF, dt);
                Session["gv"] = dt;
            }
        }
        


        protected void GV_TBGMT_CINF_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {

                GV_TBGMT_CINF.UseAccessibleHeader = true;
                GV_TBGMT_CINF.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataimport(txtOvcInfNo.Text);
        }

        protected void GV_TBGMT_CINF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string ovc_inf_no = GV_TBGMT_CINF.Rows[gvrIndex].Cells[0].Text;
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", ovc_inf_no, true);
            switch (e.CommandName)
            {
                case "btnModify":
                    Response.Redirect($"MTS_E15_2{strQueryString}");
                    break;
                default:
                    break;
            }

        }
    }
}