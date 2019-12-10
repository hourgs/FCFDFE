using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using System.Linq;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.MPMS.A
{
    public partial class MPMS_A11 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();

        protected void btnNew_Click(object sender, EventArgs e)
        {
            
            if (txtPURCHASE_1.Text.Length!=2) {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('單位代字僅可為2碼英文');", true);
                return;
            }
            if (txtPlanNum.Text.Length != 3) {
                txtPlanNum.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('計畫編號僅可為3碼數字');", true);
                return;
            }
            
            string strOVC_PURCH = lblOVC_PURCH.Text;
            TBM1301_PLAN plan1301 = new TBM1301_PLAN();
            plan1301 = gm.TBM1301_PLAN.Where(table => table.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (plan1301 != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('此預劃購案編號已存在，請變更末三碼計畫編號');", true);
                return;
            }
            else {
                string send_url;
                send_url = "~/pages/MPMS/A/MPMS_A12.aspx?PurchNum_11=" + lblOVC_PURCH.Text.ToString();
                Response.Redirect(send_url);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtPlanNum.Text = "";
            LoginScreen();
        }

        protected void btnPurch_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strPurch = txtPurch.Text;

            if (strPurch.Equals(string.Empty))
            {
                strMessage += "<p> 請輸入 單位代字及年度 </p>";
            }
            else if (strPurch.Length != 4)
            {
                strMessage += "<p> 單位代字及年度 須為4碼 </p>";
            }

            if (strMessage.Equals(string.Empty))
            {
                var query =
                    from tbm1301_plan in gm.TBM1301_PLAN
                    where tbm1301_plan.OVC_PURCH.Contains(strPurch)
                    select new
                    {
                        OVC_PURCH = tbm1301_plan.OVC_PURCH
                    };

                DataTable dt = CommonStatic.LinqQueryToDataTable(query);

                int intRun = (dt.Rows.Count % 5) > 0 ? dt.Rows.Count / 5 + 1 : dt.Rows.Count / 5;
                for (int i = 0; i < intRun; i++)
                {
                    System.Web.UI.HtmlControls.HtmlTableRow tr = new System.Web.UI.HtmlControls.HtmlTableRow();
                    for (int j = 0; j < 5; j++)
                    {
                        System.Web.UI.HtmlControls.HtmlTableCell tc = new System.Web.UI.HtmlControls.HtmlTableCell();
                        int intRow = (i * 5 + j);
                        if (dt.Rows.Count > intRow)
                        {
                            tc.InnerText = dt.Rows[intRow]["OVC_PURCH"].ToString();
                            tr.Cells.Add(tc);
                        }
                    }
                    QueryTable.Rows.Add(tr);
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }

        private void LoginScreen()
        {
            if (Session["userid"] != null)
            {
                string strUSER_ID = Session["userid"].ToString();
                ACCOUNT ac = new ACCOUNT();
                ac = gm.ACCOUNTs.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                if (ac != null)
                {
                    drpPURCHASE_1.Items.Clear();
                    string UnitCode = ac.PURCHASE_1 != null ? ac.PURCHASE_1.ToString() : "";
                    Char delimiter = ',';
                    string[] SepUnitcode = UnitCode.Split(delimiter);
                    for (int i = 0; i <= SepUnitcode.Length - 1; i++)
                    {
                        drpPURCHASE_1.Items.Insert(0,"請選擇");
                        drpPURCHASE_1.Items.Insert(i+1, SepUnitcode[i]);
                    }
                }
                list_dataImport(drpSysYear);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            if (!IsPostBack)
            {
                LoginScreen();
                list_dataImport(drpSysYear);
            }
        }

        protected void drpPURCHASE_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPURCHASE_1.Text = drpPURCHASE_1.SelectedItem.Text;
            lblPur();
        }
        #region 副程式
        private void list_dataImport(ListControl list)
        {
            //先將下拉式選單清空
            list.Items.Clear();

            int CalDateYear = Convert.ToInt16(DateTime.Now.ToString("yyyy")) - 1911;
            int num = CalDateYear;
            for (int i = 0; i < 15; i++)
            {
                list.Items.Add(Convert.ToString(num));
                num = num - 1;
            }
        }

        protected void txtPlanNum_TextChanged(object sender, EventArgs e)
        {
            lblPur();
        }

        private void lblPur() {
            lblOVC_PURCH.Text = "";
            int CalDateYear = Convert.ToInt16(DateTime.Now.ToString("yyyy")) - 1911;
            string strSysYear = drpSysYear.SelectedItem.Text.ToString();
            if (txtPURCHASE_1.Text != null && strSysYear != null && txtPlanNum.Text.ToString().Length == 3)
            {
                string strPurchase = txtPURCHASE_1.Text;
                string NewstrPurchase = "";
                for (int i = 0; i < strPurchase.Length; i++)
                    NewstrPurchase += Char.ToUpper(strPurchase[i]);
                txtPURCHASE_1.Text = NewstrPurchase;
                string PurNum = txtPURCHASE_1.Text + strSysYear.Substring((strSysYear.Length - 2), 2) + txtPlanNum.Text.ToString();
                lblOVC_PURCH.Text = PurNum;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('請檢查是否有未填欄位);", true);
                return;
            }
        }
        #endregion
    }
}
