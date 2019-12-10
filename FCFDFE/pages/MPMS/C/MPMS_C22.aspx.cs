using System;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Entity.GMModel;
using System.Linq;
using FCFDFE.Content;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.C
{
    public partial class MPMS_C22 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        MPMSEntities mpms = new MPMSEntities();
        GMEntities gm = new GMEntities();
        Common FCommon = new Common();
        string strDRecive = "";
        string strPurchNum = "";
        byte checkTimes = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!string.IsNullOrEmpty(Request.QueryString["PurchNum"])
                    && !string.IsNullOrEmpty(Request.QueryString["CheckTimes"]) &&
                    !string.IsNullOrEmpty(Request.QueryString["DRecive"]))
                {
                    strPurchNum = Request.QueryString["PurchNum"];
                    strDRecive = Request.QueryString["DRecive"];
                    checkTimes = Convert.ToByte(Request.QueryString["CheckTimes"]);
                    if (!IsPostBack)
                    {
                        GetUserInfo();
                        DataImport();
                    }
                }
                else
                {
                    Response.Redirect("MPMS_C14");
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
        
        private void DataImport()
        {
            string[] field = { "ONB_NO","OVC_ORG", "OVC_MEMO", "OVC_PERFORM"};
            var query = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.OVC_PUR_AGENCY).FirstOrDefault();
            lblPurchNum.Text = strPurchNum + query;
            lblCheckTimes.Text = checkTimes.ToString();
            var query1202_8 =
                from t in mpms.TBM1202_8
                where t.OVC_PURCH.Equals(strPurchNum) && t.ONB_CHECK_TIMES == checkTimes
                orderby t.ONB_NO
                select new
                {
                    t.ONB_NO,
                    t.OVC_ORG,
                    t.OVC_MEMO,
                    t.OVC_PERFORM
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query1202_8);
            FCommon.GridView_dataImport(GV_OVC, dt, field);
        }

        private void DataSave()
        {
            TBM1202_8 tbm1202_8 = new TBM1202_8();
            //異動=先刪除再新增
            byte onb = 1;
            if (ViewState["Change"] != null || ViewState["ONB_NO"] != null)
            {
                if (ViewState["Change"].ToString().Equals("True"))
                {
                    byte ONB_NO = Convert.ToByte(ViewState["ONB_NO"].ToString());
                    tbm1202_8 =
                    mpms.TBM1202_8.Where(o => o.OVC_PURCH.Equals(strPurchNum)
                                            && o.ONB_CHECK_TIMES == checkTimes
                                            && o.ONB_NO == ONB_NO)
                                  .FirstOrDefault();
                    mpms.Entry(tbm1202_8).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1202_8.GetType().Name.ToString(), this, "刪除");
                    onb = ONB_NO;
                    ViewState.Remove("Change");
                    ViewState.Remove("ONB_NO");
                }
            }
            else
            {
                var query1202_8 =
                    mpms.TBM1202_8.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.ONB_CHECK_TIMES == checkTimes)
                                  .Select(o=>o.ONB_NO);
               
                if (query1202_8.Any())
                {
                    onb = query1202_8.Max();
                    onb++;
                }
            }
            tbm1202_8.OVC_PURCH = strPurchNum;
            tbm1202_8.OVC_CHECK_UNIT = ViewState["DEPT_SN"].ToString();
            tbm1202_8.ONB_CHECK_TIMES = checkTimes;
            tbm1202_8.OVC_ORG = txtunit.Text;
            tbm1202_8.OVC_MEMO = txtMes.Text;
            tbm1202_8.OVC_PERFORM = txtdo.Text;
            tbm1202_8.OVC_IKIND = "ss";
            tbm1202_8.OVC_CHECK = "ss";
            tbm1202_8.ONB_NO = onb;
            tbm1202_8.OVC_STANDARD = "N";
            mpms.TBM1202_8.Add(tbm1202_8);
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1202_8.GetType().Name.ToString(), this, "新增");
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
            txtunit.Text = "";
            txtMes.Text = "";
            txtdo.Text = "";
            DataImport();
        }

        protected void GV_OVC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Button BTN = (Button)e.CommandSource;
            GridViewRow myRow = (GridViewRow)BTN.NamingContainer;
            HiddenField hidONB_NO = (HiddenField)GV_OVC.Rows[myRow.DataItemIndex].FindControl("hidONB_NO");
            byte onbNO = Convert.ToByte(hidONB_NO.Value);
            TBM1202_8 tbm1202_8 = new TBM1202_8();
            tbm1202_8 =
                mpms.TBM1202_8.Where(o => o.OVC_PURCH.Equals(strPurchNum)
                                        && o.ONB_CHECK_TIMES == checkTimes
                                        && o.ONB_NO == onbNO)
                              .FirstOrDefault();
            if (e.CommandName.Equals("btnM"))
            {
                txtunit.Text = tbm1202_8.OVC_ORG;
                txtMes.Text = tbm1202_8.OVC_MEMO;
                txtdo.Text = tbm1202_8.OVC_PERFORM;
                ViewState["Change"] = "True";
                ViewState["ONB_NO"] = onbNO;
            }
            else if (e.CommandName.Equals("btnDel"))
            {
                mpms.Entry(tbm1202_8).State = EntityState.Deleted;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1202_8.GetType().Name.ToString(), this, "刪除");
                DataImport();
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string url = "~/pages/MPMS/C/MPMS_C18.aspx?PurchNum=" + strPurchNum + "&numCheckTimes=" + checkTimes + "&DRecive=" + strDRecive;
            Response.Redirect(url);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtunit.Text) && string.IsNullOrEmpty(txtMes.Text) && string.IsNullOrEmpty(txtdo.Text))
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先 輸入內容");
            }
            else
            {
                DataSave();
            }
        }

    }
}