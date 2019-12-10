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

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D18_2 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string strOVC_PURCH, strOVC_PURCH_5, strOVC_DOPEN, strOVC_VEN_TITLE, strOVC_KIND;
        short numONB_TIMES, numONB_GROUP;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            if (Request.QueryString["OVC_PURCH"] != null)
            {
                strOVC_PURCH = Request.QueryString["OVC_PURCH"].ToString();
                strOVC_PURCH_5 = Request.QueryString["OVC_PURCH_5"]?.ToString();
                strOVC_DOPEN = Request.QueryString["OVC_DOPEN"]?.ToString();
                strOVC_VEN_TITLE = Request.QueryString["OVC_VEN_TITLE"]?.ToString();
                strOVC_KIND = Request.QueryString["OVC_KIND"]?.ToString();
                numONB_TIMES = short.Parse(Request.QueryString["ONB_TIMES"]?.ToString());
                numONB_GROUP = short.Parse(Request.QueryString["ONB_GROUP"]?.ToString());
                if (!IsPostBack)
                {
                    DataImport();
                }
            }
        }



        #region Button Click

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            //點擊 複製審查資料
            string send_url = "~/pages/MPMS/D/MPMS_D18_3.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_PURCH_5=" + strOVC_PURCH_5
                                    + "&OVC_DOPEN=" + strOVC_DOPEN + "&ONB_TIMES=" + numONB_TIMES + "&ONB_GROUP=" + numONB_GROUP
                                    + "&OVC_KIND=" + strOVC_KIND;
            Response.Redirect(send_url);
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
            "var retValue=window.confirm('"+strMsg+"');" +
            "if(retValue){document.getElementById('MainContent_btnSave_OK').click();} else { } ", true);
        }

        protected void btnSave_OK_Click(object sender, EventArgs e)
        {
            //點擊彈跳視窗的"確認"後，進行存檔
            short[] numONB_ITEMs = { 10, 11, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            TBM1313_ITEM tbm1313_ITEM = new TBM1313_ITEM();
            tbm1313_ITEM = mpms.TBM1313_ITEM.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.OVC_KIND.Equals(strOVC_KIND) && tb.OVC_VEN_TITLE.Equals(strOVC_VEN_TITLE)
                            && tb.ONB_GROUP == numONB_GROUP).OrderBy(tb => tb.ONB_ITEM).FirstOrDefault();
            if (tbm1313_ITEM == null)
            {
                foreach (short i in numONB_ITEMs)
                {
                    SaveTBM1313_ITEM(i);
                }
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功！");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "該筆資料已經存在！");
        }



        #endregion



        #region 副程式
        private void DataImport()
        {
            short[] numONB_ITEMs = { 10, 11, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                lblOVC_PURCH_A_5.Text = strOVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                lblOVC_PUR_IPURCH.Text = tbm1301.OVC_PUR_IPURCH;
                lblOVC_VEN_TITLE.Text = strOVC_VEN_TITLE;
                /*
                TBM1303 tbm1303 = new TBM1303();
                tbm1303 = mpms.TBM1303.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP).FirstOrDefault();
                if (tbm1303 != null)
                {
                    lblOVC_DOPEN.Text = strOVC_DOPEN;
                    lblOVC_OPEN_HOUR.Text = tbm1303.OVC_OPEN_HOUR;
                    lblOVC_OPEN_MIN.Text = tbm1303.OVC_OPEN_MIN;
                    lblONB_TIMES.Text = numONB_TIMES.ToString();
                    lblONB_GROUP.Text = numONB_GROUP.ToString();
                }
                */

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

                foreach (short numONB_ITEM in numONB_ITEMs)
                {
                    DataImport_TBM1313_ITEM(numONB_ITEM);
                }
            }
        }


        private void DataImport_TBM1313_ITEM(short numONB_ITEM)
        {
            TBM1313_ITEM tbm1313_ITEM = new TBM1313_ITEM();
            tbm1313_ITEM = mpms.TBM1313_ITEM.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.OVC_KIND.Equals(strOVC_KIND) && tb.OVC_VEN_TITLE.Equals(strOVC_VEN_TITLE)
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
                        ListItem newItme = (ListItem)chkOVC_ITEM_NAME.Items.FindByValue(item);
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
                        ListItem newItme = (ListItem)chkOVC_ITEM_NAME.Items.FindByValue(i);
                        if (newItme != null)
                            newItme.Selected = true;
                    }
                    if (tbm1313_ITEM.OVC_ITEM_NAME_3 != null && tbm1313_ITEM.OVC_ITEM_NAME_3 != "")
                    {
                        ListItem newItme = (ListItem)chkOVC_ITEM_NAME.Items.FindByValue("建議書");
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
                        ListItem newItme = (ListItem)chkOVC_ITEM_NAME.Items.FindByValue(i);
                        if (newItme != null)
                            newItme.Selected = true;
                    }
                    txtOther.Text = tbm1313_ITEM.OVC_ITEM_NAME_7;
                    foreach (string i in Selected_REJECT)
                    {
                        ListItem newItme = (ListItem)chkREJECT.Items.FindByValue(i);
                        if (newItme != null)
                            newItme.Selected = true;
                    }
                    txtREJECT.Text = tbm1313_ITEM.OVC_REJECT_REASON_4;
                }

                else if (numONB_ITEM == 90)
                {
                    TextBox txtOther = (TextBox)this.Master.FindControl("MainContent").FindControl("txtOther_" +numONB_ITEM);
                    txtOther.Text = tbm1313_ITEM.OVC_ITEM_NAME_1;
                }
            }
        }



        private void SaveTBM1313_ITEM(short numONB_ITEM)
        {
            //新增 TBM1313_ITEM
            TBM1313_ITEM tbm1313_ITEM_New = new TBM1313_ITEM();
            DropDownList drpOVC_RESULT = (DropDownList)this.Master.FindControl("MainContent").FindControl("drpOVC_RESULT_" + numONB_ITEM);

            tbm1313_ITEM_New.OVC_PURCH = strOVC_PURCH;
            tbm1313_ITEM_New.OVC_PURCH_5 = strOVC_PURCH_5;
            tbm1313_ITEM_New.OVC_DOPEN = strOVC_DOPEN;
            tbm1313_ITEM_New.OVC_KIND = strOVC_KIND;
            tbm1313_ITEM_New.OVC_VEN_TITLE = strOVC_VEN_TITLE;
            tbm1313_ITEM_New.ONB_ITEM = numONB_ITEM;
            tbm1313_ITEM_New.ONB_GROUP = numONB_GROUP;
            tbm1313_ITEM_New.OVC_RESULT = drpOVC_RESULT.SelectedValue;


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
                TextBox txtREJECT = (TextBox)this.Master.FindControl("MainContent").FindControl("txtREJECT_" + numONB_ITEM);
                tbm1313_ITEM_New.OVC_REJECT_REASON_1 = txtREJECT.Text;
            }

            else if (numONB_ITEM == 40)
            {
                CheckBoxList chkOVC_ITEM_NAME = (CheckBoxList)this.Master.FindControl("MainContent").FindControl("chkOVC_ITEM_NAME_" + numONB_ITEM);
                TextBox txtOVC_ITEM_NAME = (TextBox)this.Master.FindControl("MainContent").FindControl("txtOVC_ITEM_NAME_" + numONB_ITEM);
                TextBox txtREJECT = (TextBox)this.Master.FindControl("MainContent").FindControl("txtREJECT_" + numONB_ITEM);

                for (int i = 0; i < chkOVC_ITEM_NAME.Items.Count; i++)
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
                            default:
                                break;
                        }
                    }
                }

                if (txtOVC_ITEM_NAME.Text != "")
                {
                    ListItem newItme = (ListItem)chkOVC_ITEM_NAME.Items.FindByValue("建議書");
                    newItme.Selected = true;
                }
                tbm1313_ITEM_New.OVC_ITEM_NAME_3 = txtOVC_ITEM_NAME.Text;
                tbm1313_ITEM_New.OVC_REJECT_REASON_1 = txtREJECT.Text;
            }

            else if (numONB_ITEM == 80)
            {
                TextBox txtOVC_ITEM_NAME = (TextBox)this.Master.FindControl("MainContent").FindControl("txtMoney");
                CheckBoxList chkOVC_ITEM_NAME = (CheckBoxList)this.Master.FindControl("MainContent").FindControl("chkOVC_ITEM_NAME_" + numONB_ITEM);
                TextBox txtOther = (TextBox)this.Master.FindControl("MainContent").FindControl("txtOther_" + numONB_ITEM);
                CheckBoxList chkREJECT = (CheckBoxList)this.Master.FindControl("MainContent").FindControl("chkREJECT_" + numONB_ITEM);
                TextBox txtREJECT = (TextBox)this.Master.FindControl("MainContent").FindControl("txtREJECT_" + numONB_ITEM);
                txtOVC_ITEM_NAME.Text = tbm1313_ITEM_New.OVC_ITEM_NAME;

                for (int i = 0; i < 6; i++)
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
                            case 3:
                                tbm1313_ITEM_New.OVC_ITEM_NAME_4 = chkOVC_ITEM_NAME.Items[i].Value;
                                break;
                            case 4:
                                tbm1313_ITEM_New.OVC_ITEM_NAME_5 = chkOVC_ITEM_NAME.Items[i].Value;
                                break;
                            case 5:
                                tbm1313_ITEM_New.OVC_ITEM_NAME_6 = chkOVC_ITEM_NAME.Items[i].Value;
                                break;
                            default:
                                break;
                        }
                    }
                }
                tbm1313_ITEM_New.OVC_ITEM_NAME_7 = txtOther.Text;
                for (int i = 0; i < 3; i++)
                {
                    if (chkREJECT.Items[i].Selected == true)
                    {
                        switch (i)
                        {
                            case 0:
                                tbm1313_ITEM_New.OVC_REJECT_REASON_1 = chkREJECT.Items[i].Value;
                                break;
                            case 1:
                                tbm1313_ITEM_New.OVC_REJECT_REASON_2 = chkREJECT.Items[i].Value;
                                break;
                            case 2:
                                tbm1313_ITEM_New.OVC_REJECT_REASON_3 = chkREJECT.Items[i].Value;
                                break;
                            default:
                                break;
                        }
                    }
                }
                tbm1313_ITEM_New.OVC_REJECT_REASON_4 = txtREJECT.Text;
            }

            else if (numONB_ITEM == 90)
            {
                TextBox txtOther = (TextBox)this.Master.FindControl("MainContent").FindControl("txtOther_" + numONB_ITEM);
                tbm1313_ITEM_New.OVC_ITEM_NAME_1 = txtOther.Text;
            }
            mpms.TBM1313_ITEM.Add(tbm1313_ITEM_New);
            //mpms.SaveChanges();
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