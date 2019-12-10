using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;
using System.Globalization;
using Xceed.Words.NET;
using TemplateEngine.Docx;
using System.IO;


namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D17_2 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string strOVC_PURCH, strOVC_PURCH_5, strOVC_DOPEN;
        short numONB_TIMES, numONB_GROUP;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.Controls_Attributes("readonly", "true", txtOVC_DAPPROVE, txtOVC_DAPPROVE); //設置readonly屬性
                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_DOPEN"] == null || Request.QueryString["ONB_TIMES"] == null || Request.QueryString["ONB_GROUP"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    strOVC_PURCH = Request.QueryString["OVC_PURCH"] == null ? "" : Request.QueryString["OVC_PURCH"].ToString();
                    TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                    strOVC_DOPEN = Request.QueryString["OVC_DOPEN"] == null ? "" : Request.QueryString["OVC_DOPEN"].ToString();
                    short.TryParse(Request.QueryString["ONB_TIMES"].ToString(), out numONB_TIMES);
                    short.TryParse(Request.QueryString["ONB_GROUP"].ToString(), out numONB_GROUP);
                    if (IsOVC_DO_NAME() && !IsPostBack)
                    {
                        DataImport();
                        DataImport_gvBID_VENDOR();
                        DataImport_tbBID_DOC_ITEM();
                    }
                }
            }
        }


        protected void gvBID_VENDOR_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string strOVC_KIND = rdoOVC_KIND.SelectedValue;
            string strOVC_VEN_CST = ((Label)gvBID_VENDOR.Rows[gvrIndex].FindControl("lblOVC_VEN_CST")).Text;
            string strOVC_VEN_TITLE = ((Label)gvBID_VENDOR.Rows[gvrIndex].FindControl("lblOVC_VEN_TITLE")).Text;
            string strOVC_VEN_ADDRESS = ((Label)gvBID_VENDOR.Rows[gvrIndex].FindControl("lblOVC_VEN_ADDRESS")).Text;

            //刪除
            if (e.CommandName == "Del")
            {
                TBMBID_VENDOR tbmBID_VENDOR = new TBMBID_VENDOR();
                tbmBID_VENDOR = mpms.TBMBID_VENDOR.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                        && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.OVC_KIND.Equals(strOVC_KIND) && tb.ONB_GROUP == numONB_GROUP
                        && tb.OVC_VEN_CST.Equals(strOVC_VEN_CST)).FirstOrDefault();
                mpms.Entry(tbmBID_VENDOR).State = EntityState.Deleted;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmBID_VENDOR.GetType().Name.ToString(), this, "刪除");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功！");
                DataImport_gvBID_VENDOR();
            }

            //異動
            if (e.CommandName == "Modify")
            {
                TBMBID_VENDOR tbmBID_VENDOR = mpms.TBMBID_VENDOR.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                        && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.OVC_KIND.Equals(strOVC_KIND) && tb.OVC_VEN_CST.Equals(strOVC_VEN_CST)
                        && tb.ONB_GROUP == numONB_GROUP).FirstOrDefault();
                if (tbmBID_VENDOR != null)
                {
                    txtOVC_VEN_CST.Text = tbmBID_VENDOR.OVC_VEN_CST;
                    txtOVC_VEN_TITLE.Text = tbmBID_VENDOR.OVC_VEN_TITLE;
                    txtOVC_VEN_ADDRESS.Text = tbmBID_VENDOR.OVC_VEN_ADDRESS;
                    txtOVC_VEN_TEL.Text = tbmBID_VENDOR.OVC_VEN_TEL;
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查無此廠商資料");
            }

            //列印
            if (e.CommandName == "Print")
            {
                
            }
        }

        protected void rdoOVC_BID_OPEN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoOVC_BID_OPEN.SelectedValue == "N")
            {
                divVendor.Visible = true;
                rdoOVC_KIND.Visible = true;
            }
            else if (rdoOVC_BID_OPEN.SelectedValue == "Y")
            {
                divVendor.Visible = false;
                rdoOVC_KIND.Visible = false;
            }
        }

        protected void drpQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strItem = "";
            DropDownList drp = (DropDownList)sender;
            switch (drp.ID)
            {
                case "drpQ12":
                    strItem = "12";
                    break;
                case "drpQ14":
                    strItem = "14";
                    break;
                case "drpQ15":
                    strItem = "15";
                    break;
                default:
                    break;
            }
            TextBox txtOVC_ITEM_OPTION = (TextBox)this.Master.FindControl("MainContent").FindControl("txtQ" + strItem);
            if (txtOVC_ITEM_OPTION.Text == "")
                txtOVC_ITEM_OPTION.Text = drp.SelectedValue;
            else
                txtOVC_ITEM_OPTION.Text += "," + drp.SelectedValue;
        }



        protected void rdoOVC_KIND_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataImport_gvBID_VENDOR();
        }



        #region Button Click
        protected void btnSaveVendor_Click(object sender, EventArgs e)
        {
            //點擊 存檔：新增招標廠商存入TBMBID_VENDOR
            SaveData("TBMBID_VENDOR");
        }


        protected void btnSaveAndInsert_Click(object sender, EventArgs e)
        {
            //點擊 存檔並寫入廠商檔：新增招標廠商存入TBMBID_VENDOR、TBM1203
            SaveData("TBMBID_VENDOR_And_TBM1203");
        }


        protected void btnReset_Click(object sender, EventArgs e)
        {
            //點擊 清除
            txtOVC_VEN_CST.Text = "";
            txtOVC_VEN_TITLE.Text = "";
            txtOVC_VEN_ADDRESS.Text = "";
            txtOVC_VEN_TEL.Text = "";
        }

        protected void btnVendorQuery_Click(object sender, EventArgs e)
        {
            //點擊 廠商查詢
            TBM1203 tbm1203 = new TBM1203();
            tbm1203 = mpms.TBM1203.Where(tb => tb.OVC_VEN_CST.Equals(txtOVC_VEN_CST.Text)).FirstOrDefault();
            if(tbm1203 != null)
            {
                txtOVC_VEN_CST.Text = tbm1203.OVC_VEN_CST;
                txtOVC_VEN_TITLE.Text = tbm1203.OVC_VEN_TITLE;
                txtOVC_VEN_ADDRESS.Text = tbm1203.OVC_VEN_ADDRESS;
                txtOVC_VEN_TEL.Text = tbm1203.OVC_VEN_ITEL;
            }
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('統一編號查無廠商資訊');", true);
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            //點擊 存檔
            if (IsSaveTBMBID_DOC_ITEM())
            {
                string strONB_ITEM;
                int i;
                for (i = 1; i < 22; i++)
                {
                    strONB_ITEM = ((Label)this.Master.FindControl("MainContent").FindControl("lblONB_ITEM" + i)).Text;
                    short numONB_ITEM = short.Parse(strONB_ITEM);
                    TBMBID_DOC_ITEM tbmBID_DOC_ITEM = new TBMBID_DOC_ITEM();
                    tbmBID_DOC_ITEM = mpms.TBMBID_DOC_ITEM.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                        && tb.ONB_ITEM == numONB_ITEM).FirstOrDefault();
                    if (tbmBID_DOC_ITEM != null)
                    {
                        //修改 TBMBID_DOC_ITEM 招標文件檢查項目
                        tbmBID_DOC_ITEM.OVC_RESULT = ((RadioButtonList)this.Master.FindControl("MainContent").FindControl("rdoCheck" + strONB_ITEM)).SelectedValue;
                        if (strONB_ITEM == "12" || strONB_ITEM == "14" || strONB_ITEM == "15")
                        {
                            TextBox txtOVC_ITEM_OPTION = (TextBox)this.Master.FindControl("MainContent").FindControl("txtQ" + strONB_ITEM);
                            tbmBID_DOC_ITEM.OVC_ITEM_OPTION = txtOVC_ITEM_OPTION.Text;
                        }
                        else if(strONB_ITEM == "21")
                            tbmBID_DOC_ITEM.OVC_ITEM_NAME = txtOthers.Text;
                        mpms.SaveChanges();
                    }
                    else
                    {
                        //新增 TBMBID_DOC_ITEM 招標文件檢查項目
                        TBMBID_DOC_ITEM tbmBID_DOC_ITEM_New = new TBMBID_DOC_ITEM();
                        tbmBID_DOC_ITEM_New.OVC_PURCH = strOVC_PURCH;
                        tbmBID_DOC_ITEM_New.OVC_PURCH_5 = strOVC_PURCH_5;
                        tbmBID_DOC_ITEM_New.OVC_DOPEN = strOVC_DOPEN;
                        tbmBID_DOC_ITEM_New.ONB_ITEM = numONB_ITEM;
                        RadioButtonList rdoCheck = ((RadioButtonList)this.Master.FindControl("MainContent").FindControl("rdoCheck" + strONB_ITEM));
                        tbmBID_DOC_ITEM_New.OVC_RESULT = rdoCheck.SelectedValue;
                        if (strONB_ITEM == "12" || strONB_ITEM == "14" || strONB_ITEM == "15")
                        {
                            TextBox txtOVC_ITEM_OPTION = (TextBox)this.Master.FindControl("MainContent").FindControl("txtQ" + strONB_ITEM);
                            tbmBID_DOC_ITEM_New.OVC_ITEM_OPTION = txtOVC_ITEM_OPTION.Text;
                        }
                        else if (strONB_ITEM == "21")
                            tbmBID_DOC_ITEM_New.OVC_ITEM_NAME = txtOthers.Text;
                        mpms.TBMBID_DOC_ITEM.Add(tbmBID_DOC_ITEM_New);
                        mpms.SaveChanges();
                    }
                }
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), "TBMBID_DOC_ITEM", this, "更新");


                TBMBID_DOC tbmBID_DOC = new TBMBID_DOC();
                tbmBID_DOC = mpms.TBMBID_DOC.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tb.OVC_DOPEN.Equals(strOVC_DOPEN)).FirstOrDefault();
                if (tbmBID_DOC == null)
                {
                    //新增 TBMBID_DOC
                    TBMBID_DOC tbmBID_DOC_New = new TBMBID_DOC
                    {
                        OVC_PURCH = strOVC_PURCH,
                        OVC_PURCH_5 = strOVC_PURCH_5,
                        OVC_DOPEN = strOVC_DOPEN,
                        OVC_BID_OPEN = rdoOVC_BID_OPEN.SelectedValue,
                        OVC_KIND = rdoOVC_BID_OPEN.SelectedValue == "N" ? rdoOVC_KIND.SelectedValue : null,
                        OVC_DAPPROVE = txtOVC_DAPPROVE.Text,
                    };
                    mpms.TBMBID_DOC.Add(tbmBID_DOC_New);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmBID_DOC_New.GetType().Name.ToString(), this, "新增");
                }
                else
                {
                    //修改 TBMBID_DOC
                    tbmBID_DOC.OVC_BID_OPEN = rdoOVC_BID_OPEN.SelectedValue;
                    tbmBID_DOC.OVC_KIND = rdoOVC_BID_OPEN.SelectedValue == "N" ? rdoOVC_KIND.SelectedValue : null;
                    tbmBID_DOC.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmBID_DOC.GetType().Name.ToString(), this, "修改");
                }


                //修改 OVC_STATUS="23" 開標通知的 階段結束日
                TBMSTATUS tbmSTATUS_23 = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                            && tb.ONB_TIMES == 1 && tb.OVC_STATUS.Equals("23")).FirstOrDefault();
                if (tbmSTATUS_23 != null)
                {
                    tbmSTATUS_23.OVC_DEND = DateTime.Now.ToString("yyyy-MM-dd");
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmSTATUS_23.GetType().Name.ToString(), this, "修改");
                }

                string strOVC_DO_NAME = "";
                TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (tbmRECEIVE_BID != null)
                    strOVC_DO_NAME = tbmRECEIVE_BID.OVC_DO_NAME;

                //新增 OVC_STATUS="24" 截標審查的 階段開始日
                TBMSTATUS tbmSTATUS_24 = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                            && tb.ONB_TIMES == 1 && tb.OVC_STATUS.Equals("24")).FirstOrDefault();
                if (tbmSTATUS_24 == null)
                {
                    TBMSTATUS tbmSTATUS_New = new TBMSTATUS
                    {
                        OVC_STATUS_SN = Guid.NewGuid(),
                        OVC_PURCH = strOVC_PURCH,
                        OVC_PURCH_5 = strOVC_PURCH_5,
                        ONB_TIMES = 1,
                        OVC_DO_NAME = strOVC_DO_NAME,
                        OVC_STATUS = "24",
                        OVC_DBEGIN = DateTime.Now.ToString("yyyy-MM-dd"),
                    };
                    mpms.TBMSTATUS.Add(tbmSTATUS_New);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmSTATUS_New.GetType().Name.ToString(), this, "新增");
                }

                //新增 OVC_STATUS="25" 開標紀錄的 階段開始日
                TBMSTATUS tbmSTATUS_25 = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                            && tb.ONB_TIMES == 1 && tb.OVC_STATUS.Equals("25")).FirstOrDefault();
                if (tbmSTATUS_25 == null)
                {
                    TBMSTATUS tbmSTATUS_New = new TBMSTATUS
                    {
                        OVC_STATUS_SN = Guid.NewGuid(),
                        OVC_PURCH = strOVC_PURCH,
                        OVC_PURCH_5 = strOVC_PURCH_5,
                        ONB_TIMES = 1,
                        OVC_DO_NAME = strOVC_DO_NAME,
                        OVC_STATUS = "25",
                        OVC_DBEGIN = DateTime.Now.ToString("yyyy-MM-dd"),
                    };
                    mpms.TBMSTATUS.Add(tbmSTATUS_New);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmSTATUS_New.GetType().Name.ToString(), this, "新增");
                }

                FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功！");
            }
        }

        #endregion




        #region Button Click 副程式
        private bool IsOVC_DO_NAME()
        {
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strUserName, strDept = "";
            DataTable dt = new DataTable();
            if (strUSER_ID.Length > 0)
            {
                ACCOUNT ac = new ACCOUNT();
                ac = gm.ACCOUNTs.Where(tb => tb.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                if (ac != null)
                {
                    strUserName = ac.USER_NAME.ToString();
                    strDept = ac.DEPT_SN;
                    TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    if (strOVC_PURCH == "")
                        strErrorMsg = "請選擇購案";
                    else if (tbm1301 == null)
                        strErrorMsg = "查無此購案編號";
                    else
                    {
                        TBM1301_PLAN plan1301 =
                            gm.TBM1301_PLAN.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCHASE_UNIT.Equals(strDept)).FirstOrDefault();

                        TBMRECEIVE_BID tbmRECEIVE_BID =
                            mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_DO_NAME.Equals(strUserName)).FirstOrDefault();
                        if (tbmRECEIVE_BID == null || plan1301 == null)
                            strErrorMsg = "非此購案的承辦人";
                    }

                    if (strErrorMsg != "")
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErrorMsg);
                    else
                    {
                        divForm.Visible = true;
                        return true;
                    }
                }
            }
            divForm.Visible = false;
            return false;
        }



        private bool IsSaveTBMBID_DOC_ITEM()
        {
            string strErrorMsg="";
            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 == null)
                strErrorMsg = "<p>查無此購案</p>";
            if (txtQ12.Text == "")
                strErrorMsg += "<p>檢查項目第12項尚未輸入!</p>";
            if (txtQ14.Text == "")
                strErrorMsg += "<p>檢查項目第14項尚未輸入!</p>";
            if (txtQ15.Text == "")
                strErrorMsg += "<p>檢查項目第15項尚未輸入!</p>";

            if (strErrorMsg == "")
                return true;
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErrorMsg);
            return false;
        }


        private bool IsSaveTBMBID_VENDOR()
        {
            string strErrorMsg = "";
            string strOVC_KIND = rdoOVC_KIND.SelectedValue;
            if (strOVC_KIND == null || strOVC_KIND == "")
                strErrorMsg += "<p>請選擇類別(選擇性/限制性)</p>";
            if (txtOVC_VEN_CST.Text == "")
                strErrorMsg += "<p>請填入統一編號</p>";

            if (strErrorMsg == "")
                return true; 
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErrorMsg);
            return false;
        }



        private void SaveData(string saveType)
        {
            string strMsg = "";
            string strOVC_KIND = rdoOVC_KIND.SelectedValue;
            if (IsSaveTBMBID_VENDOR())
            {
                if (saveType == "TBMBID_VENDOR" || saveType== "TBMBID_VENDOR_And_TBM1203")
                {
                    TBMBID_VENDOR tbmBID_VENDOR = new TBMBID_VENDOR();
                    tbmBID_VENDOR = mpms.TBMBID_VENDOR.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.OVC_KIND.Equals(strOVC_KIND) && tb.ONB_GROUP == numONB_GROUP
                            && tb.OVC_VEN_CST.Equals(txtOVC_VEN_CST.Text)).FirstOrDefault();

                    if (tbmBID_VENDOR == null)
                    {
                        //新增 TBMBID_VENDOR
                        TBMBID_VENDOR tbmBID_VENDOR_New = new TBMBID_VENDOR
                        {
                            OVC_VEN_SN = Guid.NewGuid(),
                            OVC_PURCH = strOVC_PURCH,
                            OVC_PURCH_5 = strOVC_PURCH_5,
                            OVC_DOPEN = strOVC_DOPEN,
                            OVC_KIND = strOVC_KIND,
                            OVC_VEN_CST = txtOVC_VEN_CST.Text,
                            OVC_VEN_TITLE = txtOVC_VEN_TITLE.Text,
                            OVC_VEN_ADDRESS = txtOVC_VEN_ADDRESS.Text,
                            OVC_VEN_TEL = txtOVC_VEN_TEL.Text,
                            ONB_GROUP = numONB_GROUP,
                        };
                        mpms.TBMBID_VENDOR.Add(tbmBID_VENDOR_New);
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmBID_VENDOR_New.GetType().Name.ToString(), this, "新增");
                    }
                    else
                    {
                        //修改 TBMBID_VENDOR
                        tbmBID_VENDOR.OVC_VEN_TITLE = txtOVC_VEN_TITLE.Text;
                        tbmBID_VENDOR.OVC_VEN_ADDRESS = txtOVC_VEN_ADDRESS.Text;
                        tbmBID_VENDOR.OVC_VEN_TEL = txtOVC_VEN_TEL.Text;
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmBID_VENDOR.GetType().Name.ToString(), this, "修改");
                    }
                    strMsg = "邀商明細檔 存檔成功！";
                }


                if (saveType == "TBM1203" || saveType== "TBMBID_VENDOR_And_TBM1203")
                {
                    TBM1203 tbm1203 = new TBM1203();
                    tbm1203 = mpms.TBM1203.Where(tb => tb.OVC_VEN_CST.Equals(txtOVC_VEN_CST.Text)).FirstOrDefault();
                    if (tbm1203 == null)
                    {
                        //新增 TBM1203
                        TBM1203 tbm1203_New = new TBM1203
                        {
                            OVC_VEN_SN = Guid.NewGuid(),
                            OVC_VEN_CST = txtOVC_VEN_CST.Text,
                            OVC_VEN_TITLE = txtOVC_VEN_TITLE.Text,
                            OVC_VEN_ADDRESS = txtOVC_VEN_ADDRESS.Text,
                            OVC_VEN_ITEL = txtOVC_VEN_TEL.Text,
                        };
                        mpms.TBM1203.Add(tbm1203_New);
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1203_New.GetType().Name.ToString(), this, "新增");
                    }
                    else
                    {
                        //修改 TBM1203
                        tbm1203.OVC_VEN_TITLE = txtOVC_VEN_TITLE.Text;
                        tbm1203.OVC_VEN_ADDRESS = txtOVC_VEN_ADDRESS.Text;
                        tbm1203.OVC_VEN_ITEL = txtOVC_VEN_TEL.Text;
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1203.GetType().Name.ToString(), this, "修改");

                    }
                    strMsg += strMsg=="" ? "廠商資料 存檔成功！" : "<BR>廠商資料 存檔成功！";
                }

                FCommon.AlertShow(PnMessage, "success", "系統訊息", strMsg);
                DataImport_gvBID_VENDOR();
                txtOVC_VEN_CST.Text = "";
                txtOVC_VEN_TITLE.Text = "";
                txtOVC_VEN_ADDRESS.Text = "";
                txtOVC_VEN_TEL.Text = "";
            }
        }

        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            //點擊 回主流程畫面
            string send_url = "~/pages/MPMS/D/MPMS_D14_1.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_PURCH_5=" + strOVC_PURCH_5;
            Response.Redirect(send_url);
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            //點擊 回截標審查選擇畫面
            string send_url = "~/pages/MPMS/D/MPMS_D17.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_PURCH_5=" + strOVC_PURCH_5;
            Response.Redirect(send_url);
        }
        #endregion




        #region 副程式
        private void DataImport()
        {
            //將資料庫資料帶出至畫面
            
            //開標資料
            lblOVC_DOPEN.Text = GetTaiwanDate(strOVC_DOPEN);
            lblONB_TIMES.Text = numONB_TIMES.ToString();
            lblONB_GROUP.Text = numONB_GROUP.ToString();

            //TBM1301(購案主檔)
            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                //購案資料
                lblOVC_PURCH_A_5.Text = tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                lblOVC_PURCH.Text = tbm1301.OVC_PURCH;
                lblOVC_PUR_AGENCY.Text = tbm1301.OVC_PUR_AGENCY;
                lblOVC_PUR_IPURCH.Text = tbm1301.OVC_PUR_IPURCH;
                lblOVC_PURCH_5.Text = strOVC_PURCH_5;
                lblOVC_BID_TIMES.Text = GetTbm1407Desc("TG", tbm1301.OVC_BID_TIMES);
                lblOVC_PUR_ASS_VEN_CODE.Text = GetTbm1407Desc("C7", tbm1301.OVC_PUR_ASS_VEN_CODE);


                TBMRECEIVE_WORK tbmRECEIVE_WORK = new TBMRECEIVE_WORK();
                tbmRECEIVE_WORK = mpms.TBMRECEIVE_WORK.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                        && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_WORK != null)
                {
                    lblOVC_PUR_ASS_VEN_CODE_Sign.Text = GetTbm1407Desc("C7", tbmRECEIVE_WORK.OVC_PUR_ASS_VEN_CODE);
                }


                TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
                tbmRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                        && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_ANNOUNCE != null)
                {
                    lblOVC_OPEN_HOUR.Text = tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR;
                    lblOVC_OPEN_MIN.Text = tbmRECEIVE_ANNOUNCE.OVC_OPEN_MIN;
                    //開標資料
                    lblOVC_BID_METHOD_1.Text = tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_1;
                    lblOVC_BID_METHOD_2.Text = tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_2;
                    lblOVC_BID_METHOD_3.Text = tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_3;
                }

                
                TBMBID_DOC tbmBID_DOC = new TBMBID_DOC();
                tbmBID_DOC = mpms.TBMBID_DOC.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tb.OVC_DOPEN.Equals(strOVC_DOPEN)).FirstOrDefault();
                if (tbmBID_DOC != null)
                {
                    rdoOVC_BID_OPEN.SelectedValue = tbmBID_DOC.OVC_BID_OPEN;
                    rdoOVC_KIND.SelectedValue = tbmBID_DOC.OVC_KIND;
                    txtOVC_DAPPROVE.Text = tbmBID_DOC.OVC_DAPPROVE;
                }


                if (rdoOVC_BID_OPEN.SelectedValue == "N")
                {
                    divVendor.Visible = true;
                    rdoOVC_KIND.Visible = true;
                }
                else if (rdoOVC_BID_OPEN.SelectedValue == "Y")
                {
                    divVendor.Visible = false;
                    rdoOVC_KIND.Visible = false;
                }
            }
        }


        private void DataImport_gvBID_VENDOR()
        {
            string strOVC_KIND = rdoOVC_KIND.SelectedValue;
            DataTable dt;
            var queryBID_VENDOR =
                from tbBID_VENDOR in mpms.TBMBID_VENDOR
                where tbBID_VENDOR.OVC_PURCH.Equals(strOVC_PURCH) && tbBID_VENDOR.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                    && tbBID_VENDOR.OVC_DOPEN.Equals(strOVC_DOPEN) && tbBID_VENDOR.OVC_KIND.Equals(strOVC_KIND)
                    && tbBID_VENDOR.ONB_GROUP == numONB_GROUP
                select new
                {
                    OVC_VEN_CST = tbBID_VENDOR.OVC_VEN_CST,
                    OVC_VEN_TITLE = tbBID_VENDOR.OVC_VEN_TITLE,
                    OVC_VEN_ADDRESS = tbBID_VENDOR.OVC_VEN_ADDRESS,
                    OVC_VEN_TEL = tbBID_VENDOR.OVC_VEN_TEL,
                };
            dt = CommonStatic.LinqQueryToDataTable(queryBID_VENDOR);
            FCommon.GridView_dataImport(gvBID_VENDOR, dt);
        }


        private void DataImport_tbBID_DOC_ITEM()
        {
            DataTable dt;
            var queryDOC_ITEM =
                from tbBID_DOC_ITEM in mpms.TBMBID_DOC_ITEM
                where tbBID_DOC_ITEM.OVC_PURCH.Equals(strOVC_PURCH) && tbBID_DOC_ITEM.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                    && tbBID_DOC_ITEM.OVC_DOPEN.Equals(strOVC_DOPEN)
                orderby tbBID_DOC_ITEM.ONB_ITEM
                select new
                {
                    OVC_PURCH = tbBID_DOC_ITEM.OVC_PURCH,
                    ONB_ITEM = tbBID_DOC_ITEM.ONB_ITEM,
                    OVC_ITEM_NAME = tbBID_DOC_ITEM.OVC_ITEM_NAME,
                    OVC_RESULT = tbBID_DOC_ITEM == null ? string.Empty : tbBID_DOC_ITEM.OVC_RESULT,
                    OVC_ITEM_OPTION = tbBID_DOC_ITEM == null ? string.Empty : tbBID_DOC_ITEM.OVC_ITEM_OPTION,
                };
            dt = CommonStatic.LinqQueryToDataTable(queryDOC_ITEM);
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow rows in dt.Rows)
                {
                    string strONB_ITEM = rows["ONB_ITEM"].ToString();
                    RadioButtonList rdoOVC_RESULT = (RadioButtonList)this.Master.FindControl("MainContent").FindControl("rdoCheck" + strONB_ITEM);
                    rdoOVC_RESULT.SelectedValue = rows["OVC_RESULT"].ToString();
                    if (strONB_ITEM == "12" || strONB_ITEM == "14" || strONB_ITEM == "15")
                    {
                        TextBox txtOVC_ITEM_OPTION = (TextBox)this.Master.FindControl("MainContent").FindControl("txtQ" + strONB_ITEM);
                        txtOVC_ITEM_OPTION.Text = rows["OVC_ITEM_OPTION"].ToString();
                    }
                    else if (strONB_ITEM == "21")
                    {
                        txtOthers.Text = rows["OVC_ITEM_NAME"].ToString();
                    }
                }
            }
        }




        private String GetTbm1407Desc(string cateID, string codeID)
        {
            //將TBM1407片語ID代碼轉為Desc
            TBM1407 tbm1407 = new TBM1407();
            if (codeID != null && codeID != "")
            {
                tbm1407 = gm.TBM1407.Where(tb => tb.OVC_PHR_CATE.Equals(cateID) && tb.OVC_PHR_ID.Equals(codeID)).OrderBy(tb => tb.OVC_PHR_ID).FirstOrDefault();
                if (tbm1407 != null)
                {
                    return tbm1407.OVC_PHR_DESC.ToString();
                }
            }
            return codeID;
        }



        public string GetTaiwanDate(string strDate)
        {
            //西元年轉民國年
            if (strDate != null && strDate != "")
            {
                DateTime datetime = Convert.ToDateTime(strDate);
                CultureInfo info = new CultureInfo("zh-TW");
                TaiwanCalendar twC = new TaiwanCalendar();
                info.DateTimeFormat.Calendar = twC;
                return datetime.ToString("yyy年MM月dd日", info);
            }
            return string.Empty;
        }


        



        #endregion

    }
}