using System;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Entity.GMModel;
using System.Linq;
using FCFDFE.Content;
namespace FCFDFE.pages.MPMS.C
{
    public partial class MPMS_C21 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        MPMSEntities mpms = new MPMSEntities();
        GMEntities gm = new GMEntities();
        Common FCommon = new Common();
        string strPurchNum = "";
        string dateRecive = "";
        byte checkTimes = 1;
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
                    checkTimes = Convert.ToByte(Request.QueryString["CheckTimes"]);
                    dateRecive = Request.QueryString["DRecive"];
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

        private void DataSave()
        {
            string dept = ViewState["DEPT_SN"].ToString();
            TBM1202_ADVICE tbm1202A = new TBM1202_ADVICE();
            tbm1202A = mpms.TBM1202_ADVICE.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_DRECEIVE.Equals(dateRecive)
                                            && o.OVC_CHECK_UNIT.Equals(dept) && o.OVC_ITEM.Equals("1")).FirstOrDefault();
            if(tbm1202A != null)
            {
                if(rdo1.SelectedIndex!=-1)
                    tbm1202A.OVC_ITEM_ADVICE = rdo1.SelectedItem.Text;
                tbm1202A.OVC_ITEM_DESC = txt1.Text;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1202A.GetType().Name.ToString(), this, "修改");
            }
            else
            {
                TBM1202_ADVICE New_tbm1202A = new TBM1202_ADVICE();
                New_tbm1202A.OVC_PURCH = strPurchNum;
                New_tbm1202A.OVC_DRECEIVE = dateRecive;
                New_tbm1202A.OVC_CHECK_UNIT = dept;
                New_tbm1202A.OVC_ITEM = "1";
                if (rdo1.SelectedIndex != -1)
                    New_tbm1202A.OVC_ITEM_ADVICE = rdo1.SelectedItem.Text;
                New_tbm1202A.OVC_ITEM_DESC = txt1.Text;
                mpms.TBM1202_ADVICE.Add(New_tbm1202A);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , New_tbm1202A.GetType().Name.ToString(), this, "新增");
            }

            TBM1202_ADVICE tbm1202A_2 = new TBM1202_ADVICE();
            tbm1202A_2 = mpms.TBM1202_ADVICE.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_DRECEIVE.Equals(dateRecive)
                                            && o.OVC_CHECK_UNIT.Equals(dept) && o.OVC_ITEM.Equals("2")).FirstOrDefault();
            if(tbm1202A_2 != null)
            {
                if (rdo2.SelectedIndex != -1)
                {
                    if (rdo2.SelectedValue.Equals("4"))
                    {
                        tbm1202A_2.OVC_ITEM_ADVICE = txtrdo2_4.Text;
                    }
                    else
                    {
                        tbm1202A_2.OVC_ITEM_ADVICE = rdo2.SelectedItem.Text;
                    }
                }
                tbm1202A_2.OVC_ITEM_DESC = txt2.Text;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1202A_2.GetType().Name.ToString(), this, "修改");
            }
            else
            {
                TBM1202_ADVICE New_tbm1202A = new TBM1202_ADVICE();
                New_tbm1202A.OVC_PURCH = strPurchNum;
                New_tbm1202A.OVC_DRECEIVE = dateRecive;
                New_tbm1202A.OVC_CHECK_UNIT = dept;
                New_tbm1202A.OVC_ITEM = "2";
                if (rdo2.SelectedIndex != -1)
                {
                    if (rdo2.SelectedValue.Equals("4"))
                    {
                        New_tbm1202A.OVC_ITEM_ADVICE = txtrdo2_4.Text;
                    }
                    else
                    {
                        New_tbm1202A.OVC_ITEM_ADVICE = rdo2.SelectedItem.Text;
                    }
                }
                New_tbm1202A.OVC_ITEM_DESC = txt2.Text;
                mpms.TBM1202_ADVICE.Add(New_tbm1202A);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , New_tbm1202A.GetType().Name.ToString(), this, "新增");
            }
            TBM1202_ADVICE tbm1202A_3 = new TBM1202_ADVICE();
            tbm1202A_3 = mpms.TBM1202_ADVICE.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_DRECEIVE.Equals(dateRecive)
                                            && o.OVC_CHECK_UNIT.Equals(dept) && o.OVC_ITEM.Equals("3")).FirstOrDefault();
            if (tbm1202A_3 != null)
            {
                if (rdo3.SelectedIndex != -1)
                    tbm1202A_3.OVC_ITEM_ADVICE = rdo3.SelectedItem.Text;
                tbm1202A_3.OVC_ITEM_DESC = txt3.Text;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1202A_3.GetType().Name.ToString(), this, "修改");
            }
            else
            {
                TBM1202_ADVICE New_tbm1202A = new TBM1202_ADVICE();
                New_tbm1202A.OVC_PURCH = strPurchNum;
                New_tbm1202A.OVC_DRECEIVE = dateRecive;
                New_tbm1202A.OVC_CHECK_UNIT = dept;
                New_tbm1202A.OVC_ITEM = "3";
                if (rdo3.SelectedIndex != -1)
                    New_tbm1202A.OVC_ITEM_ADVICE = rdo3.SelectedItem.Text;
                New_tbm1202A.OVC_ITEM_DESC = txt3.Text;
                mpms.TBM1202_ADVICE.Add(New_tbm1202A);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , New_tbm1202A.GetType().Name.ToString(), this, "新增");
            }
            TBM1202_ADVICE tbm1202A_4 = new TBM1202_ADVICE();
            tbm1202A_4 = mpms.TBM1202_ADVICE.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_DRECEIVE.Equals(dateRecive)
                                            && o.OVC_CHECK_UNIT.Equals(dept) && o.OVC_ITEM.Equals("4")).FirstOrDefault();
            if(tbm1202A_4 != null)
            {
                if (rdo4.SelectedIndex != -1)
                    tbm1202A_4.OVC_ITEM_ADVICE = rdo4.SelectedItem.Text;
                tbm1202A_4.OVC_ITEM_DESC = txt4.Text;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1202A_4.GetType().Name.ToString(), this, "修改");
            }
            else
            {
                TBM1202_ADVICE New_tbm1202A = new TBM1202_ADVICE();
                New_tbm1202A.OVC_PURCH = strPurchNum;
                New_tbm1202A.OVC_DRECEIVE = dateRecive;
                New_tbm1202A.OVC_CHECK_UNIT = dept;
                New_tbm1202A.OVC_ITEM = "4";
                if (rdo4.SelectedIndex != -1)
                    New_tbm1202A.OVC_ITEM_ADVICE = rdo4.SelectedItem.Text;
                New_tbm1202A.OVC_ITEM_DESC = txt4.Text;
                mpms.TBM1202_ADVICE.Add(New_tbm1202A);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , New_tbm1202A.GetType().Name.ToString(), this, "新增");
            }
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "儲存成功");
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
            var query = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.OVC_PUR_AGENCY).FirstOrDefault();
            lblPurchNum.Text = strPurchNum + query;
            var query1202A = 
                mpms.TBM1202_ADVICE.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_DRECEIVE.Equals(dateRecive))
                              .OrderBy(o=>o.OVC_ITEM);
            if (query1202A.Any())
            {
                foreach(var item in query1202A)
                {
                    string txtID = "txt" + item.OVC_ITEM;
                    TextBox textBox =  this.Master.FindControl("MainContent").FindControl(txtID) as TextBox;
                    textBox.Text = item.OVC_ITEM_DESC;
                }
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string url = "~/pages/MPMS/C/MPMS_C18.aspx?PurchNum=" + strPurchNum + "&numCheckTimes=" + checkTimes + "&DRecive=" + dateRecive;
            Response.Redirect(url);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataSave();
        }

    }
}