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
using System.IO;
using TemplateEngine.Docx;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D18_4 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strOVC_PURCH, strOVC_PURCH_5, strOVC_DOPEN, strOVC_KIND;
        short numONB_TIMES, numONB_GROUP;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            {
                short[] numONB_ITEMs = { 10, 11, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_DOPEN"] == null || Request.QueryString["ONB_TIMES"] == null || Request.QueryString["ONB_GROUP"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    strOVC_PURCH = Request.QueryString["OVC_PURCH"].ToString();
                    TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                    strOVC_DOPEN = Request.QueryString["OVC_DOPEN"].ToString();
                    lblOVC_VEN_TITLE.Text = Request.QueryString["OVC_VEN_TITLE"] == null ? "" : Request.QueryString["OVC_VEN_TITLE"].ToString();
                    strOVC_KIND = Request.QueryString["OVC_KIND"] == null ? "" : Request.QueryString["OVC_KIND"].ToString();
                    short.TryParse(Request.QueryString["ONB_TIMES"].ToString(), out numONB_TIMES);
                    short.TryParse(Request.QueryString["ONB_GROUP"].ToString(), out numONB_GROUP);
                    if (IsOVC_DO_NAME() && !IsPostBack)
                    {
                        DataImport();
                        foreach (short numONB_ITEM in numONB_ITEMs)
                        {
                            DataImport_TBM1313_ITEM(numONB_ITEM, lblOVC_VEN_TITLE.Text);
                        }
                    }
                }
            }
        }

        protected void gvTBM1313_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView gv = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            if (e.CommandName == "Copy")
            {
                string strCopyOVC_VEN_TITLE = ((Label)gv.Rows[gvrIndex].FindControl("lblOVC_VEN_TITLE")).Text;
                short[] numONB_ITEMs = { 10, 11, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
                foreach (short numONB_ITEM in numONB_ITEMs)
                {
                    DataImport_TBM1313_ITEM(numONB_ITEM, strCopyOVC_VEN_TITLE);
                }
                divTBM1313.Visible = false;
                divTBM1313_ITEM.Visible = true;
            }
        }


        #region Button Click

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            //點擊 複製審查資料
            divTBM1313_ITEM.Visible = false;
            divTBM1313.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //點擊 存檔 =>判斷審查結果 並顯示彈跳視窗
            string strMsg = "";
            if (rdoOVC_RESULT.SelectedValue == "N")
                strMsg = "本廠商審查結果為：不合格！ \\n是否確定？";
            else if (rdoOVC_RESULT.SelectedValue == "Y")
                strMsg = "本廠商審查結果為：合格！ \\n是否確定？";
            else if (rdoOVC_RESULT.SelectedValue == null || rdoOVC_RESULT.SelectedValue == "")
                strMsg = "本廠商審查結果尚未選擇 \\n是否確定？";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Confirm01",
            "var retValue=window.confirm('" + strMsg + "');" +
            "if(retValue){document.getElementById('MainContent_btnSave_OK').click();} else { } ", true);
        }

        protected void btnSave_OK_Click(object sender, EventArgs e)
        {
            //點擊彈跳視窗的"確認"後，進行存檔
            short[] numONB_ITEMs = { 10, 11, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            foreach (short i in numONB_ITEMs)
            {
                SaveTBM1313_ITEM(i);
            }
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), "TBM1313_ITEM", this, "更新");
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功！");
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            //回廠商編輯
            string send_url = "~/pages/MPMS/D/MPMS_D18_3.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_DOPEN=" + strOVC_DOPEN
                            + "&ONB_TIMES=" + numONB_TIMES + "&ONB_GROUP=" + numONB_GROUP;
            Response.Redirect(send_url);
        }

        protected void btnReturnR_Click(object sender, EventArgs e)
        {
            //點擊 回開標紀錄選擇畫面
            string send_url = "~/pages/MPMS/D/MPMS_D18.aspx?OVC_PURCH=" + strOVC_PURCH;
            Response.Redirect(send_url);
        }

        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            //回主流程畫面
            string send_url = "~/pages/MPMS/D/MPMS_D14_1.aspx?OVC_PURCH=" + strOVC_PURCH;
            Response.Redirect(send_url);
        }

        protected void btnShowDivTBM1313_ITEM_Click(object sender, EventArgs e)
        {
            //回上一頁
            divTBM1313_ITEM.Visible = true;
            divTBM1313.Visible = false;
        }







        #endregion

        #region 副程式
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



        private void DataImport()
        {
            DataTable dt;
            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                lblOVC_PURCH_A_5.Text = strOVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                lblOVC_PUR_IPURCH.Text = tbm1301.OVC_PUR_IPURCH;


                TBM1313 tbm1313 = new TBM1313();
                tbm1313 = mpms.TBM1313.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP
                                && tb.OVC_KIND.Equals(strOVC_KIND)).FirstOrDefault();
                if (tbm1313 != null)
                {
                    lblOVC_DOPEN.Text = GetTaiwanDate(strOVC_DOPEN);
                    lblOVC_OPEN_HOUR.Text = tbm1313.OVC_BID_HOUR;
                    lblOVC_OPEN_MIN.Text = tbm1313.OVC_BID_MIN;
                    lblONB_TIMES.Text = numONB_TIMES.ToString();
                    lblONB_GROUP.Text = numONB_GROUP.ToString();
                    rdoOVC_RESULT.SelectedValue = tbm1313.OVC_RESULT;
                }


                var queryTBM1313 =
                    from tb1313 in mpms.TBM1313
                    where tb1313.OVC_PURCH.Equals(strOVC_PURCH) && tb1313.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                       && tb1313.OVC_DOPEN.Equals(strOVC_DOPEN) && tb1313.ONB_GROUP == numONB_GROUP
                    select new
                    {
                        OVC_VEN_CST = tb1313.OVC_VEN_CST,
                        OVC_VEN_TITLE = tb1313.OVC_VEN_TITLE,
                        OVC_VEN_TEL = tb1313.OVC_VEN_TEL,
                        OVC_DBID = tb1313.OVC_DBID,
                        OVC_DBID_tw = "",
                        OVC_BID_HOUR = tb1313.OVC_BID_HOUR,
                        OVC_BID_MIN = tb1313.OVC_BID_MIN,
                        OVC_RESULT = tb1313.OVC_RESULT,
                    };
                dt = CommonStatic.LinqQueryToDataTable(queryTBM1313);

                foreach (DataRow rows in dt.Rows)
                {
                    rows["OVC_DBID_tw"] = GetTaiwanDate(rows["OVC_DBID"].ToString());
                    if (rows["OVC_RESULT"].ToString() == "Y")
                        rows["OVC_RESULT"] = "合格";
                    else if (rows["OVC_RESULT"].ToString() == "N")
                        rows["OVC_RESULT"] = "不合格";
                }
                FCommon.GridView_dataImport(gvTBM1313, dt);
            }
        }


        private void DataImport_TBM1313_ITEM(short numONB_ITEM, string strImportOVC_VEN_TITLE)
        {
            ClearChecked();
            TBM1313_ITEM tbm1313_ITEM = new TBM1313_ITEM();
            tbm1313_ITEM = mpms.TBM1313_ITEM.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.OVC_VEN_TITLE.Equals(strImportOVC_VEN_TITLE) //&& tb.OVC_KIND.Equals(strOVC_KIND)
                            && tb.ONB_ITEM == numONB_ITEM && tb.ONB_GROUP == numONB_GROUP).OrderBy(tb => tb.ONB_ITEM).FirstOrDefault();

            if (tbm1313_ITEM != null)
            {
                DropDownList drpOVC_RESULT = (DropDownList)this.Master.FindControl("MainContent").FindControl("drpOVC_RESULT_" + numONB_ITEM);
                drpOVC_RESULT.SelectedValue = tbm1313_ITEM.OVC_RESULT;

                if (numONB_ITEM == 10 || numONB_ITEM == 11 || numONB_ITEM == 20)
                {
                    string[] Selected_ITEM_NAME = { tbm1313_ITEM.OVC_ITEM_NAME_1, tbm1313_ITEM.OVC_ITEM_NAME_2, tbm1313_ITEM.OVC_ITEM_NAME_3 };
                    CheckBoxList chkOVC_ITEM_NAME = (CheckBoxList)this.Master.FindControl("MainContent").FindControl("chkOVC_ITEM_NAME_" + numONB_ITEM);
                    TextBox txtOther = (TextBox)this.Master.FindControl("MainContent").FindControl("txtOther_" + numONB_ITEM);
                    TextBox txtREJECT = (TextBox)this.Master.FindControl("MainContent").FindControl("txtREJECT_" + numONB_ITEM);
                    foreach (string item in Selected_ITEM_NAME)
                    {
                        System.Web.UI.WebControls.ListItem newItme = (System.Web.UI.WebControls.ListItem)chkOVC_ITEM_NAME.Items.FindByValue(item);
                        if (newItme != null)
                            newItme.Selected = true;
                    }
                    txtOther.Text = tbm1313_ITEM.OVC_ITEM_NAME_4;
                    txtREJECT.Text = tbm1313_ITEM.OVC_REJECT_REASON_1;
                }

                else if (numONB_ITEM == 30 || numONB_ITEM == 50 || numONB_ITEM == 60 || numONB_ITEM == 70)
                {
                    TextBox txtREJECT = (TextBox)this.Master.FindControl("MainContent").FindControl("txtREJECT_" + numONB_ITEM);
                    txtREJECT.Text = tbm1313_ITEM.OVC_REJECT_REASON_1;
                }

                else if (numONB_ITEM == 40)
                {
                    string[] Selected_ITEM_NAME = { tbm1313_ITEM.OVC_ITEM_NAME_1, tbm1313_ITEM.OVC_ITEM_NAME_2 };
                    CheckBoxList chkOVC_ITEM_NAME = (CheckBoxList)this.Master.FindControl("MainContent").FindControl("chkOVC_ITEM_NAME_" + numONB_ITEM);
                    TextBox txtOVC_ITEM_NAME = (TextBox)this.Master.FindControl("MainContent").FindControl("txtOVC_ITEM_NAME_" + numONB_ITEM);
                    TextBox txtREJECT = (TextBox)this.Master.FindControl("MainContent").FindControl("txtREJECT_" + numONB_ITEM);
                    foreach (string i in Selected_ITEM_NAME)
                    {
                        System.Web.UI.WebControls.ListItem newItme = (System.Web.UI.WebControls.ListItem)chkOVC_ITEM_NAME.Items.FindByValue(i);
                        if (newItme != null)
                            newItme.Selected = true;
                    }
                    if (tbm1313_ITEM.OVC_ITEM_NAME_3 != null && tbm1313_ITEM.OVC_ITEM_NAME_3 != "")
                    {
                        System.Web.UI.WebControls.ListItem newItme = (System.Web.UI.WebControls.ListItem)chkOVC_ITEM_NAME.Items.FindByValue("建議書");
                        newItme.Selected = true;
                    }
                    txtOVC_ITEM_NAME.Text = tbm1313_ITEM.OVC_ITEM_NAME_3;
                    txtREJECT.Text = tbm1313_ITEM.OVC_REJECT_REASON_1;
                }

                else if (numONB_ITEM == 80)
                {
                    string[] Selected_ITEM_NAME = { tbm1313_ITEM.OVC_ITEM_NAME_1, tbm1313_ITEM.OVC_ITEM_NAME_2, tbm1313_ITEM.OVC_ITEM_NAME_3,
                                                   tbm1313_ITEM.OVC_ITEM_NAME_4, tbm1313_ITEM.OVC_ITEM_NAME_5, tbm1313_ITEM.OVC_ITEM_NAME_6 };
                    string[] Selected_REJECT = { tbm1313_ITEM.OVC_REJECT_REASON_1, tbm1313_ITEM.OVC_REJECT_REASON_2, tbm1313_ITEM.OVC_REJECT_REASON_3 };
                    TextBox txtOVC_ITEM_NAME = (TextBox)this.Master.FindControl("MainContent").FindControl("txtMoney");
                    CheckBoxList chkOVC_ITEM_NAME = (CheckBoxList)this.Master.FindControl("MainContent").FindControl("chkOVC_ITEM_NAME_" + numONB_ITEM);
                    TextBox txtOther = (TextBox)this.Master.FindControl("MainContent").FindControl("txtOther_" + numONB_ITEM);
                    CheckBoxList chkREJECT = (CheckBoxList)this.Master.FindControl("MainContent").FindControl("chkREJECT_" + numONB_ITEM);
                    TextBox txtREJECT = (TextBox)this.Master.FindControl("MainContent").FindControl("txtREJECT_" + numONB_ITEM);
                    txtOVC_ITEM_NAME.Text = tbm1313_ITEM.OVC_ITEM_NAME;
                    foreach (string i in Selected_ITEM_NAME)
                    {
                        System.Web.UI.WebControls.ListItem newItme = (System.Web.UI.WebControls.ListItem)chkOVC_ITEM_NAME.Items.FindByValue(i);
                        if (newItme != null)
                            newItme.Selected = true;
                    }
                    txtOther.Text = tbm1313_ITEM.OVC_ITEM_NAME_7;
                    foreach (string i in Selected_REJECT)
                    {
                        System.Web.UI.WebControls.ListItem newItme = (System.Web.UI.WebControls.ListItem)chkREJECT.Items.FindByValue(i);
                        if (newItme != null)
                            newItme.Selected = true;
                    }
                    txtREJECT.Text = tbm1313_ITEM.OVC_REJECT_REASON_4;
                }

                else if (numONB_ITEM == 90)
                {
                    TextBox txtOther = (TextBox)this.Master.FindControl("MainContent").FindControl("txtOther_" + numONB_ITEM);
                    txtOther.Text = tbm1313_ITEM.OVC_ITEM_NAME_1;
                }
            }
        }


        private void ClearChecked(params Control[] controls)
        {
            //建議書XX份 chkOVC_ITEM_NAME_40
            string[] strClears = { "chkOVC_ITEM_NAME_10", "chkOVC_ITEM_NAME_11", "chkOVC_ITEM_NAME_20", "chkOVC_ITEM_NAME_40", "chkOVC_ITEM_NAME_80", "chkREJECT_80" };
            foreach (string strClear in strClears)
            {
                CheckBoxList drp = (CheckBoxList)this.Master.FindControl("MainContent").FindControl(strClear);
                foreach (System.Web.UI.WebControls.ListItem item in drp.Items)
                {
                    if (item.Selected)
                        item.Selected = false;
                }
            }
            rdoOVC_RESULT.SelectedValue = null;
        }


        private void SaveTBM1313_ITEM(short numONB_ITEM)
        {
            TBM1313_ITEM tbm1313_ITEM = mpms.TBM1313_ITEM.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.OVC_KIND.Equals(strOVC_KIND) && tb.OVC_VEN_TITLE.Equals(lblOVC_VEN_TITLE.Text)
                && tb.ONB_GROUP == numONB_GROUP).OrderBy(tb => tb.ONB_ITEM).FirstOrDefault();
            if (tbm1313_ITEM == null)
            {
                //新增 TBM1313_ITEM
                TBM1313_ITEM tbm1313_ITEM_New = new TBM1313_ITEM();
                DropDownList drpOVC_RESULT = (DropDownList)this.Master.FindControl("MainContent").FindControl("drpOVC_RESULT_" + numONB_ITEM);

                tbm1313_ITEM_New.OVC_SN = Guid.NewGuid();
                tbm1313_ITEM_New.OVC_PURCH = strOVC_PURCH;
                tbm1313_ITEM_New.OVC_PURCH_5 = strOVC_PURCH_5;
                tbm1313_ITEM_New.OVC_DOPEN = strOVC_DOPEN;
                tbm1313_ITEM_New.OVC_KIND = strOVC_KIND;
                tbm1313_ITEM_New.OVC_VEN_TITLE = lblOVC_VEN_TITLE.Text;
                tbm1313_ITEM_New.ONB_ITEM = numONB_ITEM;
                tbm1313_ITEM_New.ONB_GROUP = numONB_GROUP;
                tbm1313_ITEM_New.OVC_RESULT = drpOVC_RESULT.SelectedValue;


                if (numONB_ITEM == 10 || numONB_ITEM == 11 || numONB_ITEM == 20)
                {
                    CheckBoxList chkOVC_ITEM_NAME = (CheckBoxList)this.Master.FindControl("MainContent").FindControl("chkOVC_ITEM_NAME_" + numONB_ITEM);
                    TextBox txtOther = (TextBox)this.Master.FindControl("MainContent").FindControl("txtOther_" + numONB_ITEM);
                    TextBox txtREJECT = (TextBox)this.Master.FindControl("MainContent").FindControl("txtREJECT_" + numONB_ITEM);

                    if (numONB_ITEM == 10)
                        tbm1313_ITEM_New.OVC_ITEM_NAME = "廠商資格(公司文件)";
                    else if (numONB_ITEM == 11)
                        tbm1313_ITEM_New.OVC_ITEM_NAME = "廠商資格(證明文件)";
                    else if (numONB_ITEM == 20)
                        tbm1313_ITEM_New.OVC_ITEM_NAME = "納稅證明";

                    for (int i = 0; i < 2; i++)
                    {
                        if (chkOVC_ITEM_NAME.Items[i].Selected == true)
                        {
                            switch (i)
                            {
                                case 0:
                                    tbm1313_ITEM_New.OVC_ITEM_NAME_1 = chkOVC_ITEM_NAME.Items[i].Value;
                                    break;
                                case 1:
                                    tbm1313_ITEM_New.OVC_ITEM_NAME_2 = chkOVC_ITEM_NAME.Items[i].Value;
                                    break;
                                case 2:
                                    tbm1313_ITEM_New.OVC_ITEM_NAME_3 = chkOVC_ITEM_NAME.Items[i].Value;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    tbm1313_ITEM_New.OVC_ITEM_NAME_4 = txtOther.Text;
                    tbm1313_ITEM_New.OVC_REJECT_REASON_1 = txtREJECT.Text;
                }

                else if (numONB_ITEM == 30 || numONB_ITEM == 50 || numONB_ITEM == 60 || numONB_ITEM == 70)
                {
                    if (numONB_ITEM == 30)
                        tbm1313_ITEM_New.OVC_ITEM_NAME = "投標廠商聲明書";
                    else if (numONB_ITEM == 50)
                        tbm1313_ITEM_New.OVC_ITEM_NAME = "投標報價單(附件清單)單、總價";
                    else if (numONB_ITEM == 60)
                        tbm1313_ITEM_New.OVC_ITEM_NAME = "投標報價單內 廠商簽章";
                    else if (numONB_ITEM == 70)
                        tbm1313_ITEM_New.OVC_ITEM_NAME = "清單內容";

                    TextBox txtREJECT = (TextBox)this.Master.FindControl("MainContent").FindControl("txtREJECT_" + numONB_ITEM);
                    tbm1313_ITEM_New.OVC_REJECT_REASON_1 = txtREJECT.Text;
                }

                else if (numONB_ITEM == 40)
                {
                    tbm1313_ITEM_New.OVC_ITEM_NAME = "規格文件或同等品審查";
                    for (int i = 0; i < chkOVC_ITEM_NAME_40.Items.Count; i++)
                    {
                        if (chkOVC_ITEM_NAME_40.Items[i].Selected == true)
                        {
                            switch (i)
                            {
                                case 0:
                                    tbm1313_ITEM_New.OVC_ITEM_NAME_1 = chkOVC_ITEM_NAME_40.Items[i].Value;
                                    break;
                                case 1:
                                    tbm1313_ITEM_New.OVC_ITEM_NAME_2 = chkOVC_ITEM_NAME_40.Items[i].Value;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    if (txtOVC_ITEM_NAME_40.Text != "")
                    {
                        System.Web.UI.WebControls.ListItem selectItme = (System.Web.UI.WebControls.ListItem)chkOVC_ITEM_NAME_40.Items.FindByValue("建議書");
                        selectItme.Selected = true;
                    }
                    tbm1313_ITEM_New.OVC_ITEM_NAME_3 = txtOVC_ITEM_NAME_40.Text;
                    tbm1313_ITEM_New.OVC_REJECT_REASON_1 = txtREJECT_40.Text;
                }

                else if (numONB_ITEM == 80)
                {
                    tbm1313_ITEM_New.OVC_ITEM_NAME = txtMoney.Text;

                    for (int i = 0; i < 6; i++)
                    {
                        if (chkOVC_ITEM_NAME_80.Items[i].Selected == true)
                        {
                            switch (i)
                            {
                                case 0:
                                    tbm1313_ITEM_New.OVC_ITEM_NAME_1 = chkOVC_ITEM_NAME_80.Items[i].Value;
                                    break;
                                case 1:
                                    tbm1313_ITEM_New.OVC_ITEM_NAME_2 = chkOVC_ITEM_NAME_80.Items[i].Value;
                                    break;
                                case 2:
                                    tbm1313_ITEM_New.OVC_ITEM_NAME_3 = chkOVC_ITEM_NAME_80.Items[i].Value;
                                    break;
                                case 3:
                                    tbm1313_ITEM_New.OVC_ITEM_NAME_4 = chkOVC_ITEM_NAME_80.Items[i].Value;
                                    break;
                                case 4:
                                    tbm1313_ITEM_New.OVC_ITEM_NAME_5 = chkOVC_ITEM_NAME_80.Items[i].Value;
                                    break;
                                case 5:
                                    tbm1313_ITEM_New.OVC_ITEM_NAME_6 = chkOVC_ITEM_NAME_80.Items[i].Value;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    tbm1313_ITEM_New.OVC_ITEM_NAME_7 = txtOther_80.Text;
                    for (int i = 0; i < 3; i++)
                    {
                        if (chkREJECT_80.Items[i].Selected == true)
                        {
                            switch (i)
                            {
                                case 0:
                                    tbm1313_ITEM_New.OVC_REJECT_REASON_1 = chkREJECT_80.Items[i].Value;
                                    break;
                                case 1:
                                    tbm1313_ITEM_New.OVC_REJECT_REASON_2 = chkREJECT_80.Items[i].Value;
                                    break;
                                case 2:
                                    tbm1313_ITEM_New.OVC_REJECT_REASON_3 = chkREJECT_80.Items[i].Value;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    tbm1313_ITEM_New.OVC_REJECT_REASON_4 = txtREJECT_80.Text;
                }

                else if (numONB_ITEM == 90)
                {
                    tbm1313_ITEM_New.OVC_ITEM_NAME = "其他";
                    tbm1313_ITEM_New.OVC_ITEM_NAME_1 = txtOther_90.Text;
                }

                else if (numONB_ITEM == 100)
                    tbm1313_ITEM_New.OVC_ITEM_NAME = "拒絕往來廠商";


                mpms.TBM1313_ITEM.Add(tbm1313_ITEM_New);
                mpms.SaveChanges();
            }



            else
            {
                //修改 TBM1313_ITEM
                DropDownList drpOVC_RESULT = (DropDownList)this.Master.FindControl("MainContent").FindControl("drpOVC_RESULT_" + numONB_ITEM);

                tbm1313_ITEM.ONB_ITEM = numONB_ITEM;
                tbm1313_ITEM.ONB_GROUP = numONB_GROUP;
                tbm1313_ITEM.OVC_RESULT = drpOVC_RESULT.SelectedValue;


                if (numONB_ITEM == 10 || numONB_ITEM == 11 || numONB_ITEM == 20)
                {
                    CheckBoxList chkOVC_ITEM_NAME = (CheckBoxList)this.Master.FindControl("MainContent").FindControl("chkOVC_ITEM_NAME_" + numONB_ITEM);
                    TextBox txtOther = (TextBox)this.Master.FindControl("MainContent").FindControl("txtOther_" + numONB_ITEM);
                    TextBox txtREJECT = (TextBox)this.Master.FindControl("MainContent").FindControl("txtREJECT_" + numONB_ITEM);

                    for (int i = 0; i < 2; i++)
                    {
                        if (chkOVC_ITEM_NAME.Items[i].Selected == true)
                        {
                            switch (i)
                            {
                                case 0:
                                    tbm1313_ITEM.OVC_ITEM_NAME_1 = chkOVC_ITEM_NAME.Items[i].Value;
                                    break;
                                case 1:
                                    tbm1313_ITEM.OVC_ITEM_NAME_2 = chkOVC_ITEM_NAME.Items[i].Value;
                                    break;
                                case 2:
                                    tbm1313_ITEM.OVC_ITEM_NAME_3 = chkOVC_ITEM_NAME.Items[i].Value;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    tbm1313_ITEM.OVC_ITEM_NAME_4 = txtOther.Text;
                    tbm1313_ITEM.OVC_REJECT_REASON_1 = txtREJECT.Text;
                }

                else if (numONB_ITEM == 30 || numONB_ITEM == 50 || numONB_ITEM == 60 || numONB_ITEM == 70)
                {
                    TextBox txtREJECT = (TextBox)this.Master.FindControl("MainContent").FindControl("txtREJECT_" + numONB_ITEM);
                    tbm1313_ITEM.OVC_REJECT_REASON_1 = txtREJECT.Text;
                }

                else if (numONB_ITEM == 40)
                {
                    for (int i = 0; i < chkOVC_ITEM_NAME_40.Items.Count; i++)
                    {
                        if (chkOVC_ITEM_NAME_40.Items[i].Selected == true)
                        {
                            switch (i)
                            {
                                case 0:
                                    tbm1313_ITEM.OVC_ITEM_NAME_1 = chkOVC_ITEM_NAME_40.Items[i].Value;
                                    break;
                                case 1:
                                    tbm1313_ITEM.OVC_ITEM_NAME_2 = chkOVC_ITEM_NAME_40.Items[i].Value;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    if (txtOVC_ITEM_NAME_40.Text != "")
                    {
                        System.Web.UI.WebControls.ListItem newItme = (System.Web.UI.WebControls.ListItem)chkOVC_ITEM_NAME_40.Items.FindByValue("建議書");
                        newItme.Selected = true;
                    }
                    tbm1313_ITEM.OVC_ITEM_NAME_3 = txtOVC_ITEM_NAME_40.Text;
                    tbm1313_ITEM.OVC_REJECT_REASON_1 = txtREJECT_40.Text;
                }

                else if (numONB_ITEM == 80)
                {
                    txtMoney.Text = tbm1313_ITEM.OVC_ITEM_NAME;

                    for (int i = 0; i < 6; i++)
                    {
                        if (chkOVC_ITEM_NAME_80.Items[i].Selected == true)
                        {
                            switch (i)
                            {
                                case 0:
                                    tbm1313_ITEM.OVC_ITEM_NAME_1 = chkOVC_ITEM_NAME_80.Items[i].Value;
                                    break;
                                case 1:
                                    tbm1313_ITEM.OVC_ITEM_NAME_2 = chkOVC_ITEM_NAME_80.Items[i].Value;
                                    break;
                                case 2:
                                    tbm1313_ITEM.OVC_ITEM_NAME_3 = chkOVC_ITEM_NAME_80.Items[i].Value;
                                    break;
                                case 3:
                                    tbm1313_ITEM.OVC_ITEM_NAME_4 = chkOVC_ITEM_NAME_80.Items[i].Value;
                                    break;
                                case 4:
                                    tbm1313_ITEM.OVC_ITEM_NAME_5 = chkOVC_ITEM_NAME_80.Items[i].Value;
                                    break;
                                case 5:
                                    tbm1313_ITEM.OVC_ITEM_NAME_6 = chkOVC_ITEM_NAME_80.Items[i].Value;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    tbm1313_ITEM.OVC_ITEM_NAME_7 = txtOther_80.Text;
                    for (int i = 0; i < 3; i++)
                    {
                        if (chkREJECT_80.Items[i].Selected == true)
                        {
                            switch (i)
                            {
                                case 0:
                                    tbm1313_ITEM.OVC_REJECT_REASON_1 = chkREJECT_80.Items[i].Value;
                                    break;
                                case 1:
                                    tbm1313_ITEM.OVC_REJECT_REASON_2 = chkREJECT_80.Items[i].Value;
                                    break;
                                case 2:
                                    tbm1313_ITEM.OVC_REJECT_REASON_3 = chkREJECT_80.Items[i].Value;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    tbm1313_ITEM.OVC_REJECT_REASON_4 = txtREJECT_80.Text;
                }

                else if (numONB_ITEM == 90)
                    tbm1313_ITEM.OVC_ITEM_NAME_1 = txtOther_90.Text;


                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1313_ITEM.GetType().Name.ToString(), this, "更新");
            }
        }



        private string GetTaiwanDate(string strDate)
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
            return strDate;
        }


        #endregion
    }
}