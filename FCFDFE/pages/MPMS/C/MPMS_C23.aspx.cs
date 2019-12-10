using System;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Entity.GMModel;
using System.Linq;
using FCFDFE.Content;

namespace FCFDFE.pages.MPMS.C
{
    public partial class MPMS_C23 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        MPMSEntities mpms = new MPMSEntities();
        GMEntities gm = new GMEntities();
        Common FCommon = new Common();
        string strPurchNum = "";
        string dateRecive = "";
        string strCheckTimes = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!string.IsNullOrEmpty(Request.QueryString["PurchNum"])
                    && !string.IsNullOrEmpty(Request.QueryString["CheckTimes"])
                    && !string.IsNullOrEmpty(Request.QueryString["DRecive"]))
                {
                    strPurchNum = Request.QueryString["PurchNum"];
                    strCheckTimes = Request.QueryString["CheckTimes"];
                    dateRecive = Request.QueryString["DRecive"];
                    if (!IsPostBack)
                    {
                        DataImport();
                        GetUserInfo();
                    }
                }
            }
        } 

        protected void GV_OVC_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GV_OVC.UseAccessibleHeader = true;
                GV_OVC.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        #region onClick
        protected void btnAddAdmin_Click(object sender, EventArgs e)
        {
            AddText(drpAdmin.SelectedItem.Text, txtAdmin, txtAdminMes);

        }

        protected void btnAdd0A100_Click(object sender, EventArgs e)
        {
            AddText(drp0A100.SelectedItem.Text, txt0A100, txt0A100Mes);
        }

        protected void btnAddNSECTION_Click(object sender, EventArgs e)
        {
            AddText(drpNSECTION.SelectedItem.Text, txtNSECTION, txtNSECTIONMes);
        }

        protected void btnAddSec_Click(object sender, EventArgs e)
        {
            AddText(drpSec.SelectedItem.Text, txtSec, txtSecMes);
        }

        protected void btnAddKeelung_Click(object sender, EventArgs e)
        {
            AddText(drpKeelung.SelectedItem.Text, txtKeelung, txtKeelungMes);
        }

        protected void btnAddTaipei_Click(object sender, EventArgs e)
        {
            AddText(drpTaipei.SelectedItem.Text, txtTaipei, txtTaipeiMes);
        }

        protected void btnAddTaichung_Click(object sender, EventArgs e)
        {
            AddText(drpTaichung.SelectedItem.Text, txtTaichung, txtTaichungMes);
        }

        protected void btnAddKao_Click(object sender, EventArgs e)
        {
            AddText(drpKao.SelectedItem.Text, txtKao, txtKaoMes);
        }

        protected void btnAddPURCHASE_UNIT_Click(object sender, EventArgs e)
        {
            AddText(drpPURCHASE_UNIT.SelectedItem.Text, txtPURCHASE_UNIT, txtPURCHASE_UNIT2);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int count = 0;
            if (chkAdmin.Checked)
            {
                SaveData(chkAdmin, txtAdminMes);
                count++;
            }

            if (chk0A100.Checked)
            {
                SaveData(chk0A100, txt0A100Mes);
                count++;
            }

            if (chkNSECTION.Checked)
            {
                SaveData(chkNSECTION, txtNSECTIONMes);
                count++;
            }
                

            if (chkSec.Checked)
            {
                SaveData(chkSec, txtSecMes);
                count++;
            }
                

            if (chkKeelung.Checked)
            {
                SaveData(chkKeelung, txtKeelungMes);
                count++;
            }
               
            if (chkTaipei.Checked)
            {
                SaveData(chkTaipei, txtTaipeiMes);
                count++;
            }
               

            if (chkTaichung.Checked)
            {
                SaveData(chkTaichung, txtTaichungMes);
                count++;
            }

            if (chkKao.Checked)
            {
                SaveData(chkKao, txtKaoMes);
                count++;
            }
                

            if (chkPURCHASE_UNIT.Checked)
            {
                SaveData(chkPURCHASE_UNIT, txtPURCHASE_UNIT2);
                count++;
            }
            if (count == 0)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "沒有勾選任何資料");
            else
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
            DataImport();
        }
        #endregion

        #region 副程式
        private void AddText(string text , TextBox paper, TextBox attTextBox)
        {
            if (string.IsNullOrEmpty(attTextBox.Text))
            {
                attTextBox.Text = text + paper.Text + "份";
            }
            else
            {
                attTextBox.Text += "，" + text + paper.Text + "份";
            }
            
        }

        private void SaveData(CheckBox cbUnit, TextBox txtMemo)
        {
            TBM1119_1 tbm1119_1 = new TBM1119_1();
            string dept = ViewState["DEPT_SN"].ToString();
            var queryUnitCode =
               gm.TBM1407.Where(o => o.OVC_PHR_CATE.Equals("Q6") && o.OVC_PHR_DESC.Equals(cbUnit.Text))
                         .Select(o => o.OVC_PHR_ID).FirstOrDefault();

            var queryisHasfile =
                mpms.TBM1119_1.Where(o => o.OVC_PURCH.Equals(strPurchNum) 
                                && o.OVC_DRECEIVE.Equals(dateRecive)
                                 && o.OVC_TARGET_UNIT.Equals(queryUnitCode));
            
            if (queryisHasfile.Any())
            {
                tbm1119_1 = queryisHasfile.FirstOrDefault();
                tbm1119_1.OVC_ATTACH_NAME = txtMemo.Text;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1119_1.GetType().Name.ToString(), this, "修改");
            }
            else
            {
                tbm1119_1.OVC_PURCH = strPurchNum;
                tbm1119_1.OVC_DRECEIVE = dateRecive;
                tbm1119_1.OVC_TARGET_UNIT = queryUnitCode;
                tbm1119_1.OVC_CHECK_UNIT = dept;
                tbm1119_1.OVC_ATTACH_NAME = txtMemo.Text;
                mpms.TBM1119_1.Add(tbm1119_1);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1119_1.GetType().Name.ToString(), this, "新增");
            }
           
        }

        private void DataImport()
        {
            string[] field = { "OVC_UNIT", "OVC_ATTACH_NAME" };
            var query =
                from t in mpms.TBM1119_1
                join t1407 in mpms.TBM1407 on t.OVC_TARGET_UNIT equals t1407.OVC_PHR_ID
                where t.OVC_PURCH.Equals(strPurchNum) && t.OVC_DRECEIVE.Equals(dateRecive)
                        && t1407.OVC_PHR_CATE.Equals("Q6")
                select new
                {
                    OVC_UNIT = t1407.OVC_PHR_DESC,
                    t.OVC_ATTACH_NAME
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            hasRows = FCommon.GridView_dataImport(GV_OVC, dt, field);
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string url = "~/pages/MPMS/C/MPMS_C18.aspx?PurchNum=" + strPurchNum + "&numCheckTimes=" + strCheckTimes + "&DRecive=" + dateRecive;
            Response.Redirect(url);
        }

        private void GetUserInfo()
        {
            if (Session["userid"] != null)
            {
                if (Session["userid"] != null)
                {
                    string userID = Session["userid"].ToString();
                    var userInfo = gm.ACCOUNTs.Where(o => o.USER_ID.Equals(userID)).FirstOrDefault();
                    ViewState["DEPT_SN"] = userInfo.DEPT_SN;
                }
            }

        }
        #endregion
    }
}