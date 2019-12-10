using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;

namespace FCFDFE.pages.MTS.E
{
    public partial class MTS_E15_2 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        MTSEntities MTSE = new MTSEntities();
        TBGMT_CINF cinf = new TBGMT_CINF();
        Common FCommon = new Common();
        string id;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getQueryString(this, "id", out id, true);
            ViewState["inf_no"] = id;
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    dataimport(id);
                }
            }

        }

        private string getQueryString()
        {

            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OdtApplyDate", Request.QueryString["OdtApplyDate"], false);
            FCommon.setQueryString(ref strQueryString, "OvcMilitaryType", Request.QueryString["OvcMilitaryType"], false);
            FCommon.setQueryString(ref strQueryString, "OvcCompany", Request.QueryString["OvcCompany"], false);
            FCommon.setQueryString(ref strQueryString, "OvcIsPaid", Request.QueryString["OvcIsPaid"], false);
            FCommon.setQueryString(ref strQueryString, "OvcInfNo", Request.QueryString["OvcInfNo"], false);
            FCommon.setQueryString(ref strQueryString, "OvcImpOrExp", Request.QueryString["OvcImpOrExp"], false);
            return strQueryString;
        }

        protected void dataimport(string ovc_inf_no)
        {
            lblOvcInfNo.Text = ovc_inf_no;
            var query = (from cinf in MTSE.TBGMT_CINF
                         select cinf).DefaultIfEmpty();
            query = query.Where(table => table.OVC_INF_NO.Equals(ovc_inf_no));
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["OVC_IS_PAID"].ToString().Equals("未付款"))
                {
                    rdoOvcIsPaid.Items[0].Selected = true;
                }
                else
                {
                    rdoOvcIsPaid.Items[1].Selected = true;
                }
                if (dt.Rows[0]["ODT_PAID_DATE"] != DBNull.Value)
                {
                    txtOdtPaidDate.Text = Convert.ToDateTime(dt.Rows[0]["ODT_PAID_DATE"]).ToString("yyyy/MM/dd");
                }
                lblOvcGist.Text = dt.Rows[0]["OVC_GIST"].ToString();
                lblOvcBudgetInfNo.Text = dt.Rows[0]["OVC_BUDGET"].ToString();
                lblOvcSeaOrAir.Text = dt.Rows[0]["OVC_SEA_OR_AIR"].ToString();
                lblOvcPurposeType.Text = dt.Rows[0]["OVC_PURPOSE_TYPE"].ToString();
                txtOdtApplyDate.Text = Convert.ToDateTime(dt.Rows[0]["ODT_APPLY_DATE"]).ToString("yyyy/MM/dd");
                txtOdtApplyDate.ReadOnly = true;
                lblOnbAmount1.Text = dt.Rows[0]["ONB_AMOUNT"].ToString();
                lblOvcBudgetInfNo1.Text = dt.Rows[0]["OVC_BUDGET_INF_NO"].ToString();
                lblOvcInvNo.Text = dt.Rows[0]["OVC_INV_NO"].ToString();
                txtOdtInvDate.Text = Convert.ToDateTime(dt.Rows[0]["ODT_INV_DATE"]).ToString("yyyy/MM/dd");
                txtOdtInvDate.ReadOnly = true;
                lblOvcNote.Text = dt.Rows[0]["OVC_NOTE"].ToString();
                lblOvcPlnContent.Text = dt.Rows[0]["OVC_PLN_CONTENT"].ToString();

                var query2 = from ics in MTSE.TBGMT_ICS
                             select ics;
                query2 = query2.Where(table => table.OVC_INF_NO.Equals(ovc_inf_no));
                DataTable dt2 = CommonStatic.LinqQueryToDataTable(query2);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_CINF, dt2);
            }
            
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strOvcIsPaid = rdoOvcIsPaid.SelectedValue;
            string strOdtPaidDate = txtOdtPaidDate.Text;

            if (strOdtPaidDate.Equals(string.Empty))
                strMessage += "<P> 請填入 付款日期 </p>";

            DateTime dateOdtPaidDate;
            bool boolOdtPaidDat = FCommon.checkDateTime(strOdtPaidDate, "付款日期", ref strMessage, out dateOdtPaidDate);

            if(strMessage.Equals(string.Empty))
            {
                try
                {
                    cinf = MTSE.TBGMT_CINF.Where(table => table.OVC_INF_NO.Equals(lblOvcInfNo.Text)).FirstOrDefault();
                    cinf.OVC_IS_PAID = rdoOvcIsPaid.SelectedValue.ToString();
                    if (boolOdtPaidDat) cinf.ODT_PAID_DATE = dateOdtPaidDate; else cinf.ODT_PAID_DATE = null;
                    MTSE.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), cinf.GetType().Name.ToString(), this, "修改");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                }
                catch (Exception ex)
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "修改失敗");
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);


        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_E15_1{getQueryString()}");
        }

        protected void GV_TBGMT_CINF_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {

                GV_TBGMT_CINF.UseAccessibleHeader = true;
                GV_TBGMT_CINF.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
    }
}