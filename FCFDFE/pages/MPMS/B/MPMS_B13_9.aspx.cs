using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using System.Data;
using FCFDFE.Entity.MPMSModel;
using FCFDFE.Entity.GMModel;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.B
{
    public partial class MPMS_B13_9 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        GMEntities gm = new GMEntities();
        string strPurchNum="";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (Request.QueryString["PurchNum"] != null)
                {
                    strPurchNum = Request.QueryString["PurchNum"].ToString();
                    lblOVC_PURCH.Text = strPurchNum;
                    if (!IsPostBack)
                    {
                        LoginScreen();
                    }
                }
                else
                {
                    Response.Redirect("MPMS_B11");
                }
            }
        }

        protected void btn_Save_1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAgency_1.Text))
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先 輸入內容");
            }
            else
            {
                string memo = "本案" + drpSuit_1.SelectedValue + "「" + txtAgency_1.Text +
                                            "內購財務，勞務、資訊服務採購契約通用條款。」";
                SaveMemo(memo, "Memo", "Y");
            }
        }

        protected void btn_Save_2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPage_1.Text))
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先 輸入內容");
            }
            else
            {
                string memo = "隨案檢附" + drpContract_1.SelectedValue + "乙份（共" + txtPage_1.Text + " 頁。）";
                SaveMemo(memo, "Memo", "Y");
            }
        }

        protected void btn_Save_3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAgency_2.Text)|| string.IsNullOrEmpty(txtPage_2.Text))
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先 輸入內容");
            }
            else
            {
                string memo = "本案" + drpSuit_2.SelectedValue + "「" + txtAgency_2.Text +
                                            "內購財務，勞務、資訊服務採購契約通用條款，並檢附" + drpContract_2.SelectedValue +
                                            "乙份（共" + txtPage_2.Text + "頁。）";
                SaveMemo(memo, "Memo", "Y");
            }
        }

        protected void btn_Del_Click(object sender, EventArgs e)
        {
            TBM1220_1 tbm1220_1 = new TBM1220_1();
            tbm1220_1 = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_IKIND.Equals("D20")).FirstOrDefault();
            mpms.Entry(tbm1220_1).State = EntityState.Deleted;
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                    , tbm1220_1.GetType().Name.ToString(), this, "刪除");
            MemoInIt();
        }

        protected void btnEdit_Save_Click(object sender, EventArgs e)
        {
            string memo = txtNewOVC_MEMO.Text;
            SaveMemo(memo, "ss", "N");
            txtNewOVC_MEMO.Text = "";
        }

        private void SaveMemo(string memo,string check, string standard)
        {
            if (string.IsNullOrEmpty(memo))
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先 輸入內容");
            }
            else
            {
                TBM1220_1 tbm1220_1 = new TBM1220_1();
                tbm1220_1 = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_IKIND.Equals("D20")).FirstOrDefault();
                if (tbm1220_1 == null)
                {
                    //新增
                    TBM1220_1 newtbm1220_1 = new TBM1220_1();
                    newtbm1220_1.OVC_PURCH = strPurchNum;
                    newtbm1220_1.OVC_IKIND = "D20";
                    newtbm1220_1.ONB_NO = 1;
                    newtbm1220_1.OVC_CHECK = check;
                    newtbm1220_1.OVC_MEMO = memo;
                    newtbm1220_1.OVC_STANDARD = standard;
                    mpms.TBM1220_1.Add(newtbm1220_1);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                    , newtbm1220_1.GetType().Name.ToString(), this, "新增");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功");
                }
                else
                {
                    //修改
                    tbm1220_1.OVC_CHECK = check;
                    tbm1220_1.OVC_MEMO = memo;
                    tbm1220_1.OVC_STANDARD = standard;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                    , tbm1220_1.GetType().Name.ToString(), this, "修改");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                }
                MemoInIt();
            }
            
        }
        
        private void LoginScreen()
        {
            MemoInIt();
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_urlUse;
            send_urlUse = "~/pages/MPMS/B/MPMS_B13.aspx?PurchNum=" + Request.QueryString["PurchNum"];
            Response.Redirect(send_urlUse);
        }

        private void MemoInIt()
        {
            var query = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_IKIND.Equals("D20")).FirstOrDefault();
            if (query != null)
            {
                txtMemo.Text = query.OVC_MEMO;
            }
            else
            {
                txtMemo.Text = "";
            }
        }


    }
}