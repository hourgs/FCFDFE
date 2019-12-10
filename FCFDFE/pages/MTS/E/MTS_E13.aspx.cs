using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using System.Data;
using FCFDFE.Content;

namespace FCFDFE.pages.MTS.E
{
    public partial class MTS_E13 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        TBGMT_CINF cinf = new TBGMT_CINF();

        protected void btnSave_Click(object sender, EventArgs e)
        {
            String strMessage = "";
            string inf_no = "";
            if(txtOvcGist.Text.Equals(string.Empty))
            {
                strMessage += "請輸入案由<br>";
            }
            if (txtOvcPurposeType.Text.Equals(string.Empty))
            {
                strMessage += "請輸入用途別<br>";
            }
            if (txtOvcBudgetInfNo.Text.Equals(string.Empty))
            {
                strMessage += "請輸入預算通知單編號<br>";
            }
            if (txtOvcInvNo.Text.Equals(string.Empty))
            {
                strMessage += "請輸入收據號碼<br>";
            }
            if (txtOvcInvDate.Text.Equals(string.Empty))
            {
                strMessage += "請輸入收據日期<br>";
            }
            DateTime OvcInvDate;
            if (!DateTime.TryParse(txtOvcInvDate.Text, out OvcInvDate)&& !txtOvcInvDate.Text.Equals(string.Empty))
            {
                strMessage += "收據日期格式不正確<br>";
            }
            if (OvcNote.Text.Equals(string.Empty))
            {
                strMessage += "請輸入備考<br>";
            }
            
            if(strMessage.Equals(""))
            {
                
                
        
                cinf.OVC_GIST = drpOvcGist.SelectedValue.ToString() + txtOvcGist.Text;
                if (!drpOvcBudgetInfNo.SelectedValue.Equals("未輸入")) 
                {
                    cinf.OVC_BUDGET = drpOvcBudgetInfNo.SelectedValue.ToString();
                }
                cinf.OVC_SEA_OR_AIR = drpOvcSeaOrAir.SelectedValue.ToString();
                cinf.OVC_IMP_OR_EXP = drpOvcImpOrExp.SelectedValue.ToString();
                cinf.OVC_PURPOSE_TYPE = txtOvcPurposeType.Text;
                cinf.ODT_APPLY_DATE = Convert.ToDateTime(txtOvcApplyDate.Text) ;
                cinf.OVC_BUDGET_INF_NO = txtOvcBudgetInfNo.Text;
                cinf.OVC_INV_NO = txtOvcInvNo.Text;
                cinf.ODT_INV_DATE = Convert.ToDateTime(txtOvcInvDate.Text);
                cinf.OVC_NOTE = OvcNote.Text;
                cinf.OVC_PLN_CONTENT = OvcPlnContent.Text;
               
                string last_cinf_year = (DateTime.Now.Year - 1911).ToString();
                var query = from Tcinf in MTSE.TBGMT_CINF.DefaultIfEmpty()
                             where Tcinf.OVC_INF_NO.Contains("CINF" + last_cinf_year)
                             select new
                             {
                                ovc_inf_no= Tcinf.OVC_INF_NO
                             };
                DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                if(dt.Rows.Count>0)
                {
                    string temp = dt.Rows[dt.Rows.Count-1][0].ToString();
                    int last_inf_no = Convert.ToInt32(temp.Substring(temp.Length - 4, 4));
                    inf_no= "CINF" + (DateTime.Now.Year - 1911).ToString() + (last_inf_no + 1).ToString().PadLeft(4, '0');
                    cinf.OVC_INF_NO = inf_no;

                }
                else
                {
                    inf_no= "CINF" + (DateTime.Now.Year - 1911).ToString() + "0001";
                    cinf.OVC_INF_NO = inf_no;
                }





                cinf.OVC_CREATE_LOGIN_ID = Session["userid"].ToString();
                cinf.OVC_CREATE_ID= Session["userid"].ToString();
                cinf.OVC_MODIFY_LOGIN_ID= Session["userid"].ToString();
                cinf.OVC_IS_PAID = "未付款";
                cinf.OVC_INF_SN = System.Guid.NewGuid();
                cinf.ODT_MODIFY_DATE = Convert.ToDateTime(DateTime.Now);
                cinf.ODT_CREATE_DATE = Convert.ToDateTime(DateTime.Now);



                MTSE.TBGMT_CINF.Add(cinf);
                MTSE.SaveChanges();
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功，結報申請表編號為:"+inf_no);
                FCommon.syslog_add(Session["userid"].ToString(),Request.ServerVariables["REMOTE_ADDR"].ToString(), cinf.GetType().Name.ToString(), this, "新增");
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
        }

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
                    FCommon.list_dataImport(drpOvcGist, dtdrpOvcMilitaryType, "OVC_CLASS_NAME", "OVC_CLASS_NAME", false);
                    for (int i = 0; i < drpOvcGist.Items.Count; i++)
                    {
                        if (drpOvcGist.Items[i].ToString().Equals("中央"))
                        {
                            drpOvcGist.Items[i].Selected = true;
                        }
                    }

                    txtOvcApplyDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                }
            }

        }
    }
}