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
using TemplateEngine.Docx;
using System.IO;
using Microsoft.International.Formatters;
using System.Collections.Generic;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D19_1 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        public string sOVC_PURCH, sOVC_PURCH_5, sOVC_DOPEN, sONB_TIMES, sONB_GROUP;
        string strOVC_PURCH, strOVC_PUR_AGENCY, strOVC_PURCH_5, strOVC_DOPEN; 
        short numONB_TIMES, numONB_GROUP;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            {
                //設置readonly屬性
                FCommon.Controls_Attributes("readonly", "true", txtOVC_DAPPROVE, txtONB_GROUP_TBM1302, txtONB_GROUP_TBM1314
                , txtOVC_VEN_CST_TBM1314, txtOVC_VEN_TITLE_TBM1314, txtOVC_VEN_CST_TBM1302
                , txtOVC_VEN_TITLE_0, txtOVC_VEN_TITLE_3); //txtOVC_VENDORS_NAME, txtOVC_EFFECTS_NAME,
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);

                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_DOPEN"] == null || Request.QueryString["ONB_TIMES"] == null || Request.QueryString["ONB_GROUP"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    strOVC_PURCH = Request.QueryString["OVC_PURCH"].ToString();
                    strOVC_DOPEN = Request.QueryString["OVC_DOPEN"].ToString();
                    short.TryParse(Request.QueryString["ONB_TIMES"].ToString(), out numONB_TIMES);
                    short.TryParse(Request.QueryString["ONB_GROUP"].ToString(), out numONB_GROUP);

                    TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;

                    TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PUR_AGENCY = tbm1301 == null ? "" : tbm1301.OVC_PUR_AGENCY;

                    if (IsOVC_DO_NAME() && !IsPostBack)
                    {
                        DataImport();
                        DataImport_Gv();
                    }
                }
            }
        }


        protected void gvTBM1314_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strOVC_VEN_CST = ((Label)((Control)e.CommandSource).FindControl("lblOVC_VEN_CST")).Text;
            if (e.CommandName == "More")
            {
                //點擊 更多 按鈕
                string send_url = "~/pages/MPMS/D/MPMS_D19_2.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_DOPEN=" + strOVC_DOPEN
                                    + "&ONB_TIMES=" + numONB_TIMES + "&ONB_GROUP=" + numONB_GROUP + "&OVC_VEN_CST" + strOVC_VEN_CST;
                Response.Redirect(send_url);
            }
            else if (e.CommandName == "Chnage")
            {
                //點擊 異動 按鈕
                TBM1314 tbm1314_Edit = new TBM1314();
                tbm1314_Edit = mpms.TBM1314.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                               && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP
                               && tb.OVC_VEN_CST.Equals(strOVC_VEN_CST)).FirstOrDefault();
                if (tbm1314_Edit != null)
                {
                    txtONB_GROUP_TBM1314.Text = tbm1314_Edit.ONB_GROUP.ToString();
                    txtOVC_VEN_CST_TBM1314.Text = tbm1314_Edit.OVC_VEN_CST;
                    txtOVC_VEN_TITLE_TBM1314.Text = tbm1314_Edit.OVC_VEN_TITLE;
                    rdoOVC_MBID.SelectedValue = tbm1314_Edit.OVC_MBID;
                    rdoOVC_MINIS_LOWEST.SelectedValue = tbm1314_Edit.OVC_MINIS_LOWEST;
                    rdoOVC_MINIS_1.SelectedValue = tbm1314_Edit.OVC_MINIS_1;
                    rdoOVC_MINIS_2.SelectedValue = tbm1314_Edit.OVC_MINIS_2;
                    rdoOVC_MINIS_3.SelectedValue = tbm1314_Edit.OVC_MINIS_3;
                    txtONB_MBID.Text = tbm1314_Edit.ONB_MBID?.ToString();
                    txtONB_MINIS_LOWEST.Text = tbm1314_Edit.ONB_MINIS_LOWEST?.ToString();
                    txtONB_MINIS_1.Text = tbm1314_Edit.ONB_MINIS_1?.ToString();
                    txtONB_MINIS_2.Text = tbm1314_Edit.ONB_MINIS_2?.ToString();
                    txtONB_MINIS_3.Text = tbm1314_Edit.ONB_MINIS_3?.ToString();
                    rdoOVC_KMBID.SelectedValue = tbm1314_Edit.OVC_KMBID;
                    rdoOVC_KMINIS_LOWEST.SelectedValue = tbm1314_Edit.OVC_KMINIS_LOWEST;
                    rdoOVC_KMINIS_1.SelectedValue = tbm1314_Edit.OVC_KMINIS_1;
                    rdoOVC_KMINIS_2.SelectedValue = tbm1314_Edit.OVC_KMINIS_2;
                    rdoOVC_KMINIS_3.SelectedValue = tbm1314_Edit.OVC_KMINIS_3;
                }
            }
            else if (e.CommandName == "Del")
            {
                //點擊 刪除 按鈕
                TBM1314 tbm1314_Del = new TBM1314();
                tbm1314_Del = mpms.TBM1314.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                                    && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP
                                                    && tb.OVC_VEN_CST.Equals(strOVC_VEN_CST)).FirstOrDefault();
                mpms.Entry(tbm1314_Del).State = EntityState.Deleted;
                mpms.SaveChanges();
                DataImport_Gv();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1314_Del.GetType().Name.ToString(), this, "刪除"); 
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "投標廠商報價比價表 已刪除！");
            }
            
        }


        protected void gvTBM1302_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strOVC_VEN_CST = ((Label)((Control)e.CommandSource).FindControl("lblOVC_VEN_CST")).Text;
            if (e.CommandName == "Chnage")
            {
                //點擊 異動 按鈕
                txtONB_GROUP_TBM1302.Text = ((Label)((Control)e.CommandSource).FindControl("lblONB_GROUP")).Text;
                txtOVC_VEN_CST_TBM1302.Text = ((Label)((Control)e.CommandSource).FindControl("lblOVC_VEN_CST")).Text;
                txtOVC_VEN_TITLE_TBM1302.Text = ((Label)((Control)e.CommandSource).FindControl("lblOVC_VEN_TITLE")).Text;
                txtOVC_VEN_TEL.Text = ((Label)((Control)e.CommandSource).FindControl("lblOVC_VEN_TEL")).Text;
                txtOVC_PURCH_6.Text = ((Label)((Control)e.CommandSource).FindControl("lblOVC_PURCH_6")).Text;
            }
            else if (e.CommandName == "Del")
            {
                //點擊 刪除 按鈕
                TBM1302 tbm1302_Del = new TBM1302();
                tbm1302_Del = mpms.TBM1302.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                                    && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP 
                                                    && tb.OVC_VEN_CST.Equals(strOVC_VEN_CST)).FirstOrDefault();
                mpms.Entry(tbm1302_Del).State = EntityState.Deleted;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1302_Del.GetType().Name.ToString(), this, "刪除");
                DataImport_Gv();
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "得標廠商檔案 已刪除！");
            }
        }

        //開標結果
        protected void rdoOVC_RESULT_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if (rdo.ID == "rdoOVC_RESULT_0" && rdo.Checked==true)           //決標
                SetOVC_RESULT("0");
            else if(rdo.ID== "rdoOVC_RESULT_1" && rdo.Checked == true)      //流標
                SetOVC_RESULT("1");
            else if (rdo.ID == "rdoOVC_RESULT_2" && rdo.Checked == true)    //廢標
                SetOVC_RESULT("2");
            else if (rdo.ID == "rdoOVC_RESULT_3" && rdo.Checked == true)    //保留開標結果
                SetOVC_RESULT("3");
        }



        //(一)
        protected void rdoOVC_BID_METHOD_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if (rdo.ID == "rdoOVC_BID_METHOD_1" && rdo.Checked == true)
            {
                rdoOVC_BID_METHOD_2.Checked = false;
                rdoOVC_BID_METHOD_3.Checked = false;
                for (int i = 0; i < chkOVC_METHOD.Items.Count; i++)
                {
                    chkOVC_METHOD.Items[i].Selected = false;
                }
            }

            else if (rdo.ID == "rdoOVC_BID_METHOD_2" && rdo.Checked == true)
            {
                rdoOVC_BID_METHOD_1.Checked = false;
                rdoOVC_BID_METHOD_3.Checked = false;
            }
            else if (rdo.ID == "rdoOVC_BID_METHOD_3" && rdo.Checked == true)
            {
                rdoOVC_BID_METHOD_1.Checked = false;
                rdoOVC_BID_METHOD_2.Checked = false;
                for (int i = 0; i < chkOVC_METHOD.Items.Count; i++)
                {
                    chkOVC_METHOD.Items[i].Selected = false;
                }
            }
        }

        protected void chkOVC_METHOD_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isChecked = false;
            foreach (System.Web.UI.WebControls.ListItem item in chkOVC_METHOD.Items)
            {
                if (item.Selected)
                {
                    isChecked = true;
                }
            }
            if (isChecked)
            {
                rdoOVC_BID_METHOD_2.Checked = true;
                rdoOVC_BID_METHOD_1.Checked = false;
                rdoOVC_BID_METHOD_3.Checked = false;
            }
        }




        //(三)開、審標
        protected void drpOVC_VENDORS_NAME_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOVC_VENDORS_NAME.Text += txtOVC_VENDORS_NAME.Text == "" ? drpOVC_VENDORS_NAME.SelectedValue : " , " + drpOVC_VENDORS_NAME.SelectedValue;
        }

        protected void drpOVC_EFFECTS_NAME_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOVC_EFFECTS_NAME.Text += txtOVC_EFFECTS_NAME.Text == "" ? drpOVC_EFFECTS_NAME.SelectedValue : " , " + drpOVC_EFFECTS_NAME.SelectedValue;
        }



        //(五)決標
        protected void rdoOVC_FOLLOW_OK_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if (rdo.ID == "rdoOVC_FOLLOW_OK_Y")
                rdoOVC_FOLLOW_OK_N.Checked = false;
            else if (rdo.ID == "rdoOVC_FOLLOW_OK_N")
                rdoOVC_FOLLOW_OK_Y.Checked = false;
        }


        protected void drpOVC_VEN_CST_0_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(drpOVC_VEN_CST_0.SelectedItem.Text=="請選擇")
                txtOVC_VEN_TITLE_0.Text = "";
            else
            txtOVC_VEN_TITLE_0.Text = drpOVC_VEN_CST_0.SelectedItem.Text;
        }


        protected void rdoOVC_RESULT_REASON_0_CheckedChanged(object sender, EventArgs e)
        {
            //開標結果說明 rdoOVC_RESULT_REASON
            RadioButton rdo = (RadioButton)sender;
            switch (rdo.ID)
            {
                case "rdoOVC_RESULT_REASON_0_0":
                    rdoOVC_RESULT_REASON_0_1.Checked = false;
                    rdoOVC_RESULT_REASON_0_2.Checked = false;
                    rdoONB_COMMITTEE_BUDGET.Checked = false;
                    break;

                case "rdoOVC_RESULT_REASON_0_1":
                    rdoOVC_RESULT_REASON_0_0.Checked = false;
                    rdoOVC_RESULT_REASON_0_2.Checked = false;
                    rdoONB_COMMITTEE_BUDGET.Checked = false;
                    break;

                case "rdoOVC_RESULT_REASON_0_2":
                    rdoOVC_RESULT_REASON_0_0.Checked = false;
                    rdoOVC_RESULT_REASON_0_1.Checked = false;
                    rdoONB_COMMITTEE_BUDGET.Checked = false;
                    break;
                case "rdoONB_COMMITTEE_BUDGET":
                    rdoOVC_RESULT_REASON_0_0.Checked = false;
                    rdoOVC_RESULT_REASON_0_1.Checked = false;
                    rdoOVC_RESULT_REASON_0_2.Checked = false;
                    break;
                default:
                    break;
            }
        }



        //(六)保留開標結果
        protected void drpOVC_VEN_CST_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(drpOVC_VEN_CST_3.SelectedItem.Text == "請選擇")
                txtOVC_VEN_TITLE_3.Text = "";
            else
                txtOVC_VEN_TITLE_3.Text = drpOVC_VEN_CST_3.SelectedItem.Text;
        }



        //第二頁
        protected void drpOVC_VEN_TITLE_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drp = (DropDownList)sender;
            if (drp.SelectedValue != "" && drp.SelectedValue != null)
            {
                TBM1313 tbm1313 = new TBM1313();
                tbm1313 = mpms.TBM1313.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP
                            && tb.OVC_VEN_CST.Equals(drp.SelectedValue)).FirstOrDefault();
                if (tbm1313 != null)
                {
                    if (drp.ID == "drpOVC_VEN_TITLE_TBM1314")
                    {
                        txtOVC_VEN_CST_TBM1314.Text = tbm1313.OVC_VEN_CST;
                        txtOVC_VEN_TITLE_TBM1314.Text = tbm1313.OVC_VEN_TITLE;
                    }
                    else if (drp.ID == "drpOVC_VEN_TITLE_TBM1302")
                    {
                        txtOVC_VEN_CST_TBM1302.Text = tbm1313.OVC_VEN_CST;
                        txtOVC_VEN_TITLE_TBM1302.Text = tbm1313.OVC_VEN_TITLE;
                        txtOVC_VEN_TEL.Text = tbm1313.OVC_VEN_TEL;
                    }
                }
            }
        }



        #region Button Click
        protected void lbtnVendor_Click(object sender, EventArgs e)
        {
            //點擊 廠商編輯
            string send_url = "~/pages/MPMS/D/MPMS_D18.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_DOPEN=" + strOVC_DOPEN 
                            + "&ONB_TIMES=" + numONB_TIMES + "&ONB_GROUP=" + numONB_GROUP;
            Response.Redirect(send_url);
        }


        protected void btnSave_TBM1303_Click(object sender, EventArgs e)
        {
            //點擊 採購開標紀錄檔的 存檔
            TBM1303 tbm1303 = new TBM1303();
            tbm1303 = mpms.TBM1303.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_TIMES == numONB_TIMES
                            && tb.ONB_GROUP == numONB_GROUP).FirstOrDefault();
            if (tbm1303 == null)
            {
                //新增 TBM1303 (採購開標紀錄檔)
                TBM1303 tbm1303_new = new TBM1303();
                tbm1303_new.OVC_PURCH = strOVC_PURCH;
                tbm1303_new.OVC_PURCH_5 = strOVC_PURCH_5;
                tbm1303_new.OVC_DOPEN = strOVC_DOPEN;
                tbm1303_new.ONB_TIMES = numONB_TIMES;
                tbm1303_new.ONB_GROUP = numONB_GROUP;
                tbm1303_new.OVC_RESULT = GetOVC_RESULT();


                //(一)
                tbm1303_new.OVC_ROOM = txtOVC_ROOM.Text;
                tbm1303_new.OVC_CHAIRMAN = txtOVC_CHAIRMAN.Text;
                tbm1303_new.OVC_22_1_NO = txtOVC_22_1_NO.Text;
                tbm1303_new.OVC_OPEN_METHOD = rdoOVC_OPEN_METHOD.SelectedValue;
                tbm1303_new.OVC_BID_METHOD = GetOVC_BID_METHOD();
                if (rdoOVC_BID_METHOD_2.Checked == true)
                {
                    tbm1303_new.OVC_METHOD_1 = GetSaveValue(chkOVC_METHOD, 0);
                    tbm1303_new.OVC_METHOD_2 = GetSaveValue(chkOVC_METHOD, 1);
                    tbm1303_new.OVC_METHOD_3 = GetSaveValue(chkOVC_METHOD, 2);
                }


                //(二)
                tbm1303_new.OVC_RESTRICT_1 = GetSaveValue(chkOVC_RESTRICT, 0);
                tbm1303_new.OVC_RESTRICT_2 = GetSaveValue(chkOVC_RESTRICT, 1);





                //(三)
                tbm1303_new.OVC_CHECK_RESULT_1 = GetSaveValue(chkOVC_CHECK_RESULT, 0);
                tbm1303_new.OVC_CHECK_RESULT_2 = GetSaveValue(chkOVC_CHECK_RESULT, 1);

                tbm1303_new.ONB_OK_VENDORS = txtONB_OK_VENDORS.Text == "" ? (short?)null : short.Parse(txtONB_OK_VENDORS.Text);
                tbm1303_new.ONB_NOTOK_VENDORS = txtONB_NOTOK_VENDORS.Text == "" ? (short?)null : short.Parse(txtONB_NOTOK_VENDORS.Text);
                tbm1303_new.OVC_VENDORS_NAME = txtOVC_VENDORS_NAME.Text;
                tbm1303_new.OVC_AUDIT_SPEC = GetSaveValue(chkOVC_AUDIT,0);
                tbm1303_new.OVC_AUDIT_DOC = GetSaveValue(chkOVC_AUDIT, 1);
                tbm1303_new.OVC_AUDIT_OTHER = GetSaveValue(chkOVC_AUDIT, 2) == "Other" ? txtOVC_AUDIT_OTHER.Text : null;

                tbm1303_new.ONB_AUDIT_DOC = txtONB_AUDIT_DOC.Text != "" ? short.Parse(txtONB_AUDIT_DOC.Text) : (short?)null;
                tbm1303_new.OVC_CHECK_DOC = chkOVC_CHECK_DOC.Checked == true ? "Y" : "N";
                if (chkOVC_CHECK_DOC.Checked == true)
                {
                    if (chkONB_OK_EFFECT.Checked == true)
                        tbm1303_new.ONB_OK_EFFECT = txtONB_OK_EFFECT.Text != "" ? short.Parse(txtONB_OK_EFFECT.Text) : (short?)null;
                    else
                        tbm1303_new.ONB_OK_EFFECT = null;

                    if (chkONB_NOTOK_EFFECT.Checked == true)
                    {
                        tbm1303_new.ONB_NOTOK_EFFECT = txtONB_NOTOK_EFFECT.Text != "" ? short.Parse(txtONB_NOTOK_EFFECT.Text) : (short?)null;
                        tbm1303_new.OVC_EFFECTS_NAME = txtOVC_EFFECTS_NAME.Text;
                    }
                    else
                    {
                        tbm1303_new.ONB_NOTOK_EFFECT = null;
                        tbm1303_new.OVC_EFFECTS_NAME = null;
                    }
                }
                else
                {
                    tbm1303_new.ONB_OK_EFFECT = null;
                    tbm1303_new.ONB_NOTOK_EFFECT = null;
                    tbm1303_new.OVC_EFFECTS_NAME = null;
                }
                
                tbm1303_new.ONB_RESULT_OK = txtONB_RESULT_OK.Text != "" ? short.Parse(txtONB_RESULT_OK.Text) : (short?)null;
                tbm1303_new.ONB_RESULT_NOTOK = txtONB_RESULT_NOTOK.Text != "" ? short.Parse(txtONB_RESULT_NOTOK.Text) : (short?)null;


                //(四)流標
                if (rdoOVC_RESULT_1.Checked == true)
                {
                    tbm1303_new.OVC_RESULT_REASON = "投標未達法定家數";
                    tbm1303_new.ONB_BID_VENDOR_LAW = txtONB_BID_VENDOR_LAW.Text != "" ? short.Parse(txtONB_BID_VENDOR_LAW.Text) : (short?)null;
                    tbm1303_new.OVC_BACK = chkOVC_BACK.Checked == true ? "Y" : "N";
                }

                //(四)廢標
                else if (rdoOVC_RESULT_2.Checked == true)
                    tbm1303_new.OVC_RESULT_REASON = rdoOVC_RESULT_REASON_2.SelectedValue;
                

                //(五)決標
                else if (rdoOVC_RESULT_0.Checked == true)
                { 
                    tbm1303_new.OVC_VEN_CST = drpOVC_VEN_CST_0.SelectedValue;
                    tbm1303_new.OVC_VEN_TITLE = txtOVC_VEN_TITLE_0.Text;
                    if (rdoOVC_FOLLOW_OK_Y.Checked == true)
                        tbm1303_new.OVC_FOLLOW_OK = "Y";
                    else if (rdoOVC_FOLLOW_OK_N.Checked == true)
                    {
                        tbm1303_new.OVC_FOLLOW_OK = "N";
                        tbm1303_new.OVC_BID_CURRENT = drpOVC_BID_CURRENT_0.SelectedValue;
                        tbm1303_new.ONB_BID_MONEY = txtONB_BID_MONEY_0.Text == "" ? (decimal?)null : decimal.Parse(txtONB_BID_MONEY_0.Text);
                    }


                    if (rdoOVC_RESULT_REASON_0_0.Checked == true)
                    {
                        tbm1303_new.OVC_RESULT_REASON = "在底價內";
                        tbm1303_new.OVC_CURRENT = drpOVC_CURRENT.SelectedValue;
                        tbm1303_new.ONB_BID_BUDGET = txtONB_BID_BUDGET.Text == "" ? (decimal?)null : decimal.Parse(txtONB_BID_BUDGET.Text);
                    }

                    else if (rdoOVC_RESULT_REASON_0_1.Checked == true)
                        tbm1303_new.OVC_RESULT_REASON = "評審委員會認為廠商報價合理且在預算以內";

                    else if (rdoONB_COMMITTEE_BUDGET.Checked == true)
                    {
                        tbm1303_new.OVC_RESULT_REASON = "在評審委員會建議金額內";
                        tbm1303_new.OVC_COMMITTEE_CURRENT = drpOVC_COMMITTEE_CURRENT.SelectedValue;
                        tbm1303_new.ONB_COMMITTEE_BUDGET = txtONB_COMMITTEE_BUDGET.Text != "" ? decimal.Parse(txtONB_COMMITTEE_BUDGET.Text) : (decimal?)null;
                    }

                    else if (rdoOVC_RESULT_REASON_0_2.Checked == true)
                    {
                        tbm1303_new.OVC_RESULT_REASON = "評選委員會依規定評選為最有利標";
                        tbm1303_new.OVC_RESULT_DESC = txtOVC_RESULT_DESC.Text;
                        tbm1303_new.OVC_LAW_ITEM = txtOVC_LAW_ITEM.Text;
                        tbm1303_new.OVC_LAW_NO = txtOVC_LAW_NO.Text;
                    }
                }


                //(六)保留開標結果
                else if (rdoOVC_RESULT_3.Checked == true)
                {
                    tbm1303_new.OVC_VEN_CST = drpOVC_VEN_CST_3.SelectedValue;
                    tbm1303_new.OVC_VEN_TITLE = txtOVC_VEN_TITLE_3.Text;
                    tbm1303_new.OVC_BID_CURRENT = drpOVC_BID_CURRENT_3.SelectedValue;
                    tbm1303_new.ONB_BID_MONEY = txtONB_BID_MONEY_3.Text == "" ? (decimal?)null : decimal.Parse(txtONB_BID_MONEY_3.Text);
                }


                //(七)不予開標、決標
                else if (rdoOVC_RESULT_4.Checked == true)
                {
                    tbm1303_new.OVC_RESULT_REASON = rdoOVC_RESULT_REASON_4.SelectedValue;
                    tbm1303_new.OVC_NONE_BID_NO = txtOVC_NONE_BID_NO.Text;
                }


                tbm1303_new.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;



                //第二頁

                //第二頁下方
                tbm1303_new.OVC_RESULT_CURRENT = drpOVC_RESULT_CURRENT.SelectedValue;
                tbm1303_new.ONB_BID_RESULT = txtONB_BID_RESULT.Text == "" ? (decimal?)null : decimal.Parse(txtONB_BID_RESULT.Text);
                tbm1303_new.OVC_ADDITIONAL = txtOVC_ADDITIONAL.Text;
                tbm1303_new.OVC_FINISH_1 = GetSaveValue(chkOVC_FINISH, 0);
                tbm1303_new.OVC_FINISH_2 = GetSaveValue(chkOVC_FINISH, 1);


                tbm1303_new.OVC_ADVICE = txtOVC_ADVICE.Text;

                mpms.TBM1303.Add(tbm1303_new);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1303_new.GetType().Name.ToString(), this, "新增");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "採購開標紀錄檔 新增成功！");
            }


            else
            {
                //修改 TBM1303 (採購開標紀錄檔)
                tbm1303.OVC_PURCH = strOVC_PURCH;
                tbm1303.OVC_PURCH_5 = strOVC_PURCH_5;
                tbm1303.OVC_DOPEN = strOVC_DOPEN;
                tbm1303.ONB_TIMES = numONB_TIMES;
                tbm1303.ONB_GROUP = numONB_GROUP;
                tbm1303.OVC_RESULT = GetOVC_RESULT();


                //(一)
                tbm1303.OVC_ROOM = txtOVC_ROOM.Text;
                tbm1303.OVC_CHAIRMAN = txtOVC_CHAIRMAN.Text;
                tbm1303.OVC_22_1_NO = txtOVC_22_1_NO.Text;
                tbm1303.OVC_OPEN_METHOD = rdoOVC_OPEN_METHOD.SelectedValue;
                tbm1303.OVC_BID_METHOD = GetOVC_BID_METHOD();
                if (rdoOVC_BID_METHOD_2.Checked == true)
                {
                    tbm1303.OVC_METHOD_1 = GetSaveValue(chkOVC_METHOD, 0);
                    tbm1303.OVC_METHOD_2 = GetSaveValue(chkOVC_METHOD, 1);
                    tbm1303.OVC_METHOD_3 = GetSaveValue(chkOVC_METHOD, 2);
                }


                //(二)
                tbm1303.OVC_RESTRICT_1 = GetSaveValue(chkOVC_RESTRICT, 0);
                tbm1303.OVC_RESTRICT_2 = GetSaveValue(chkOVC_RESTRICT, 1);



                //(三)
                tbm1303.OVC_CHECK_RESULT_1 = GetSaveValue(chkOVC_CHECK_RESULT, 0);
                tbm1303.OVC_CHECK_RESULT_2 = GetSaveValue(chkOVC_CHECK_RESULT, 1);
                tbm1303.ONB_OK_VENDORS = txtONB_OK_VENDORS.Text == "" ? (short?)null : short.Parse(txtONB_OK_VENDORS.Text);
                tbm1303.ONB_NOTOK_VENDORS = txtONB_NOTOK_VENDORS.Text == "" ? (short?)null : short.Parse(txtONB_NOTOK_VENDORS.Text);
                tbm1303.OVC_VENDORS_NAME = txtOVC_VENDORS_NAME.Text;

                tbm1303.OVC_AUDIT_SPEC = GetSaveValue(chkOVC_AUDIT, 0);
                tbm1303.OVC_AUDIT_DOC = GetSaveValue(chkOVC_AUDIT, 1);
                tbm1303.OVC_AUDIT_OTHER = GetSaveValue(chkOVC_AUDIT, 2) == "Other" ? txtOVC_AUDIT_OTHER.Text : null;

                tbm1303.ONB_AUDIT_DOC = txtONB_AUDIT_DOC.Text != "" ? short.Parse(txtONB_AUDIT_DOC.Text) : (short?)null;
                tbm1303.OVC_CHECK_DOC = chkOVC_CHECK_DOC.Checked == true ? "Y" : "N";
                if (chkOVC_CHECK_DOC.Checked == true)
                {
                    if (chkONB_OK_EFFECT.Checked == true)
                        tbm1303.ONB_OK_EFFECT = txtONB_OK_EFFECT.Text != "" ? short.Parse(txtONB_OK_EFFECT.Text) : (short?)null;
                    else
                        tbm1303.ONB_OK_EFFECT = null;

                    if (chkONB_NOTOK_EFFECT.Checked == true)
                    {
                        tbm1303.ONB_NOTOK_EFFECT = txtONB_NOTOK_EFFECT.Text != "" ? short.Parse(txtONB_NOTOK_EFFECT.Text) : (short?)null;
                        tbm1303.OVC_EFFECTS_NAME = txtOVC_EFFECTS_NAME.Text;
                    }
                    else
                    {
                        tbm1303.ONB_NOTOK_EFFECT = null;
                        tbm1303.OVC_EFFECTS_NAME = null;
                    }
                }
                else
                {
                    tbm1303.ONB_OK_EFFECT = null;
                    tbm1303.ONB_NOTOK_EFFECT = null;
                    tbm1303.OVC_EFFECTS_NAME = null;
                }
                tbm1303.ONB_RESULT_OK = txtONB_RESULT_OK.Text != "" ? short.Parse(txtONB_RESULT_OK.Text) : (short?)null;
                tbm1303.ONB_RESULT_NOTOK = txtONB_RESULT_NOTOK.Text != "" ? short.Parse(txtONB_RESULT_NOTOK.Text) : (short?)null;


                //(四)流標
                if (rdoOVC_RESULT_1.Checked == true)
                {
                    tbm1303.OVC_RESULT_REASON = "投標未達法定家數";
                    if (txtONB_BID_VENDOR_LAW.Text != "")
                        tbm1303.ONB_BID_VENDOR_LAW = short.Parse(txtONB_BID_VENDOR_LAW.Text);
                    tbm1303.OVC_BACK = chkOVC_BACK.Checked == true ? "Y" : "N";
                }
                //(四)廢標
                else if (rdoOVC_RESULT_2.Checked == true)
                    tbm1303.OVC_RESULT_REASON = rdoOVC_RESULT_REASON_2.SelectedValue;


                //(五)決標
                else if (rdoOVC_RESULT_0.Checked == true)
                {
                    tbm1303.OVC_VEN_CST = drpOVC_VEN_CST_0.SelectedValue;
                    tbm1303.OVC_VEN_TITLE = txtOVC_VEN_TITLE_0.Text;
                    if (rdoOVC_FOLLOW_OK_Y.Checked == true)
                        tbm1303.OVC_FOLLOW_OK = "Y";
                    else if (rdoOVC_FOLLOW_OK_N.Checked == true)
                    {
                        tbm1303.OVC_FOLLOW_OK = "N";
                        tbm1303.OVC_BID_CURRENT = drpOVC_BID_CURRENT_0.SelectedValue;
                        tbm1303.ONB_BID_MONEY = txtONB_BID_MONEY_0.Text == "" ? (decimal ?)null : decimal.Parse(txtONB_BID_MONEY_0.Text);
                    }


                    if (rdoOVC_RESULT_REASON_0_0.Checked == true)
                    {
                        tbm1303.OVC_RESULT_REASON = "在底價內";
                        tbm1303.OVC_CURRENT = drpOVC_CURRENT.SelectedValue;
                        tbm1303.ONB_BID_BUDGET = txtONB_BID_BUDGET.Text == "" ? (decimal?)null : decimal.Parse(txtONB_BID_BUDGET.Text);
                    }

                    if (rdoOVC_RESULT_REASON_0_1.Checked == true)
                        tbm1303.OVC_RESULT_REASON = "評審委員會認為廠商報價合理且在預算以內";

                    if (rdoONB_COMMITTEE_BUDGET.Checked == true)
                    {
                        tbm1303.OVC_RESULT_REASON = "在評審委員會建議金額內";
                        tbm1303.OVC_COMMITTEE_CURRENT = drpOVC_COMMITTEE_CURRENT.SelectedValue;
                        tbm1303.ONB_COMMITTEE_BUDGET = txtONB_COMMITTEE_BUDGET.Text != "" ? decimal.Parse(txtONB_COMMITTEE_BUDGET.Text) : (decimal?)null;
                    }

                    if (rdoOVC_RESULT_REASON_0_2.Checked == true)
                    {
                        tbm1303.OVC_RESULT_REASON = "評選委員會依規定評選為最有利標";
                        tbm1303.OVC_RESULT_DESC = txtOVC_RESULT_DESC.Text;
                        tbm1303.OVC_LAW_ITEM = txtOVC_LAW_ITEM.Text;
                        tbm1303.OVC_LAW_NO = txtOVC_LAW_NO.Text;
                    }
                }


                //(六)保留開標結果
                else if (rdoOVC_RESULT_3.Checked == true)
                {
                    tbm1303.OVC_VEN_CST = drpOVC_VEN_CST_3.SelectedValue;
                    tbm1303.OVC_VEN_TITLE = txtOVC_VEN_TITLE_3.Text;
                    tbm1303.OVC_BID_CURRENT = drpOVC_BID_CURRENT_3.SelectedValue;
                    tbm1303.ONB_BID_MONEY = txtONB_BID_MONEY_3.Text == "" ? (decimal?)null : decimal.Parse(txtONB_BID_MONEY_3.Text);
                }


                //(七)不予開標、決標
                else if (rdoOVC_RESULT_4.Checked == true)
                {
                    tbm1303.OVC_RESULT_REASON = rdoOVC_RESULT_REASON_4.SelectedValue;
                    tbm1303.OVC_NONE_BID_NO = txtOVC_NONE_BID_NO.Text;
                }


                tbm1303.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;


                //第二頁


                //第二頁下方
                tbm1303.OVC_RESULT_CURRENT = drpOVC_RESULT_CURRENT.SelectedValue;
                if(txtONB_BID_RESULT.Text != "")
                    tbm1303.ONB_BID_RESULT = decimal.Parse(txtONB_BID_RESULT.Text);
                tbm1303.OVC_ADDITIONAL = txtOVC_ADDITIONAL.Text;
                tbm1303.OVC_FINISH_1 = GetSaveValue(chkOVC_FINISH, 0);
                tbm1303.OVC_FINISH_2 = GetSaveValue(chkOVC_FINISH, 1);
                tbm1303.OVC_ADVICE = txtOVC_ADVICE.Text;

                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1303.GetType().Name.ToString(), this, "修改");
            }
            //修改 OVC_STATUS="25" 開標通知的 階段結束日
            TBMSTATUS tbmSTATUS_25 = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                        && tb.ONB_TIMES == 1 && tb.OVC_STATUS.Equals("25")).FirstOrDefault();
            if (tbmSTATUS_25 != null)
            {
                tbmSTATUS_25.OVC_DEND = DateTime.Now.ToString("yyyy-MM-dd");
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmSTATUS_25.GetType().Name.ToString(), this, "修改");
            }

            //新增 OVC_STATUS="26" 開標結果的 階段開始日
            string strOVC_DO_NAME = "";
            TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbmRECEIVE_BID != null)
                strOVC_DO_NAME = tbmRECEIVE_BID.OVC_DO_NAME;

            TBMSTATUS tbmSTATUS_26 = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                        && tb.ONB_TIMES == 1 && tb.OVC_STATUS.Equals("26")).FirstOrDefault();
            if (tbmSTATUS_26 == null)
            {
                TBMSTATUS tbmSTATUS_New = new TBMSTATUS
                {
                    OVC_STATUS_SN = Guid.NewGuid(),
                    OVC_PURCH = strOVC_PURCH,
                    OVC_PURCH_5 = strOVC_PURCH_5,
                    ONB_TIMES = 1,
                    OVC_DO_NAME = strOVC_DO_NAME,
                    OVC_STATUS = "26",
                    OVC_DBEGIN = DateTime.Now.ToString("yyyy-MM-dd"),
                };
                mpms.TBMSTATUS.Add(tbmSTATUS_New);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmSTATUS_New.GetType().Name.ToString(), this, "新增");
            }

            FCommon.AlertShow(PnMessage, "success", "系統訊息", "開標記錄作業檔 存檔成功！");
        }


        protected void btnSave_TBM1314_Click(object sender, EventArgs e)
        {
            //點擊 投標廠商報價比價表的 存檔
            string strErrorMsg = "";
            if (txtONB_GROUP_TBM1314.Text == "")
                strErrorMsg += "<p>請輸入組別</p>";
            if (txtOVC_VEN_CST_TBM1314.Text == "")
                strErrorMsg += "<p>請輸入統一編號</p>";
            
            if (strErrorMsg == "")
            { 
                TBM1314 tbm1314 = new TBM1314();
                tbm1314 = mpms.TBM1314.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tb.ONB_GROUP == numONB_GROUP && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                            && tb.OVC_VEN_CST.Equals(txtOVC_VEN_CST_TBM1314.Text)).FirstOrDefault();
                if (tbm1314 == null)
                {
                    //新增 TBM1314 (採購投標廠商報價歷史檔/第二頁)
                    TBM1314 tbm1314_new = new TBM1314()
                    {
                        OVC_PURCH = strOVC_PURCH,
                        OVC_PURCH_5 = strOVC_PURCH_5,
                        OVC_DOPEN = strOVC_DOPEN,
                        OVC_VEN_CST = txtOVC_VEN_CST_TBM1314.Text,
                        OVC_VEN_TITLE = txtOVC_VEN_TITLE_TBM1314.Text,
                        ONB_GROUP = numONB_GROUP,
                        ONB_MBID = txtONB_MBID.Text=="" ? (decimal?)null : decimal.Parse(txtONB_MBID.Text),
                        ONB_MINIS_LOWEST = txtONB_MINIS_LOWEST.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_LOWEST.Text),
                        ONB_MINIS_1 = txtONB_MINIS_1.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_1.Text),
                        ONB_MINIS_2 = txtONB_MINIS_2.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_2.Text),
                        ONB_MINIS_3 = txtONB_MINIS_3.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_3.Text),
                        OVC_MBID = rdoOVC_MBID.SelectedValue,
                        OVC_MINIS_LOWEST = rdoOVC_MINIS_LOWEST.SelectedValue,
                        OVC_MINIS_1 = rdoOVC_MINIS_1.SelectedValue,
                        OVC_MINIS_2 = rdoOVC_MINIS_2.SelectedValue,
                        OVC_MINIS_3 = rdoOVC_MINIS_3.SelectedValue,
                        OVC_KMBID = rdoOVC_KMBID.SelectedValue,
                        OVC_KMINIS_LOWEST = rdoOVC_KMINIS_LOWEST.SelectedValue,
                        OVC_KMINIS_1 = rdoOVC_KMINIS_1.SelectedValue,
                        OVC_KMINIS_2 = rdoOVC_KMINIS_2.SelectedValue,
                        OVC_KMINIS_3 = rdoOVC_KMINIS_3.SelectedValue,
                    };
                    mpms.TBM1314.Add(tbm1314_new);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1314_new.GetType().Name.ToString(), this, "新增");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "投標廠商報價比價表 新增成功！");
                }
                else
                {
                    //修改 TBM1314 (採購投標廠商報價歷史檔/第二頁)
                    tbm1314.OVC_VEN_TITLE = txtOVC_VEN_TITLE_TBM1314.Text;
                    tbm1314.ONB_MBID = txtONB_MBID.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MBID.Text);
                    tbm1314.ONB_MINIS_LOWEST = txtONB_MINIS_LOWEST.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_LOWEST.Text);
                    tbm1314.ONB_MINIS_1 = txtONB_MINIS_1.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_1.Text);
                    tbm1314.ONB_MINIS_2 = txtONB_MINIS_2.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_2.Text);
                    tbm1314.ONB_MINIS_3 = txtONB_MINIS_3.Text == "" ? (decimal?)null : decimal.Parse(txtONB_MINIS_3.Text);
                    tbm1314.OVC_MBID = rdoOVC_MBID.SelectedValue;
                    tbm1314.OVC_MINIS_LOWEST = rdoOVC_MINIS_LOWEST.SelectedValue;
                    tbm1314.OVC_MINIS_1 = rdoOVC_MINIS_1.SelectedValue;
                    tbm1314.OVC_MINIS_2 = rdoOVC_MINIS_2.SelectedValue;
                    tbm1314.OVC_MINIS_3 = rdoOVC_MINIS_3.SelectedValue;
                    tbm1314.OVC_KMBID = rdoOVC_KMBID.SelectedValue;
                    tbm1314.OVC_KMINIS_LOWEST = rdoOVC_KMINIS_LOWEST.SelectedValue;
                    tbm1314.OVC_KMINIS_1 = rdoOVC_KMINIS_1.SelectedValue;
                    tbm1314.OVC_KMINIS_2 = rdoOVC_KMINIS_2.SelectedValue;
                    tbm1314.OVC_KMINIS_3 = rdoOVC_KMINIS_3.SelectedValue;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1314.GetType().Name.ToString(), this, "修改");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "投標廠商報價比價表 存檔成功！");
                }
                DataImport_Gv();
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErrorMsg);
            }
        }



        protected void btnSave_TBM1302_Click(object sender, EventArgs e)
        {
            //點擊 得標廠商的 存檔
            string strErrorMsg = "";
            if (txtONB_GROUP_TBM1302.Text == "")
                strErrorMsg += "<p>請輸入組別</p>";
            if (txtOVC_VEN_CST_TBM1302.Text == "")
                strErrorMsg += "<p>請輸入統一編號</p>";
            if (txtOVC_PURCH_6.Text == "")
                strErrorMsg += "<p>請輸契約號</p>";

            TBM1302 tbm1302_Exist = mpms.TBM1302.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                        && tb.OVC_PURCH_6.Equals(txtOVC_PURCH_6.Text) && tb.ONB_GROUP == numONB_GROUP).FirstOrDefault();
            if (tbm1302_Exist != null)
                strErrorMsg += "<p>已有得標廠商</p>";

            if (strErrorMsg == "")
            { 
                //short numONB_GROUP_TBM1302 = short.Parse(txtONB_GROUP_TBM1302.Text);
                TBM1302 tbm1302 = new TBM1302();
                tbm1302 = mpms.TBM1302.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tb.OVC_PURCH_6.Equals(txtOVC_PURCH_6.Text) && tb.ONB_GROUP == numONB_GROUP
                            && tb.OVC_VEN_CST.Equals(txtOVC_VEN_CST_TBM1302.Text)).FirstOrDefault();
                if (tbm1302 == null)
                {
                    //新增 TBM1302 (採購合約檔)
                    TBM1302 tbm1302_New = new TBM1302()
                    {
                        OVC_PURCH = strOVC_PURCH,
                        OVC_PURCH_5 = strOVC_PURCH_5,
                        ONB_GROUP = numONB_GROUP,
                        OVC_VEN_CST = txtOVC_VEN_CST_TBM1302.Text,
                        OVC_DOPEN = strOVC_DOPEN,
                        OVC_VEN_TITLE = txtOVC_VEN_TITLE_TBM1302.Text,
                        OVC_VEN_TEL = txtOVC_VEN_TEL.Text,
                        OVC_PURCH_6 = txtOVC_PURCH_6.Text,
                    };
                    mpms.TBM1302.Add(tbm1302_New);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1302_New.GetType().Name.ToString(), this, "新增");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "得標廠商檔案 新增成功！");
                }
                else
                {
                    //修改 TBM1302 (採購合約檔)
                    tbm1302.OVC_VEN_TITLE = txtOVC_VEN_TITLE_TBM1302.Text;
                    tbm1302.OVC_VEN_TEL = txtOVC_VEN_TEL.Text;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1302.GetType().Name.ToString(), this, "修改");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "得標廠商檔案 存檔成功！");
                }
                DataImport_Gv();
            }

            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErrorMsg);
            
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            //點擊 回上一頁
            string send_url = "~/pages/MPMS/D/MPMS_D19.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_PURCH_5=" + strOVC_PURCH_5;
            Response.Redirect(send_url);
        }

        protected void lbtnToWordD19_1_1_Click(object sender, EventArgs e)
        {
            GetWordD19_1_1();
        }

        protected void lbtnToWordD19_1_2_Click(object sender, EventArgs e)
        {
            GetWordD19_1_2();
        }

        protected void lbtnToWordD19_1_3_Click(object sender, EventArgs e)
        {
            GetWordD19_1_3();
        }

        protected void lbtnToWordD19_1_4_Click(object sender, EventArgs e)
        {
            GetWordD19_1_4();
        }

        protected void lbtnToWordD19_1_5_Click(object sender, EventArgs e)
        {
            GetWordD19_1_5();
        }

        protected void lbtnToWordD19_1_6_Click(object sender, EventArgs e)
        {
            GetWordD19_1_6();
        }



        protected void btnTBMBID_BACK_Click(object sender, EventArgs e)
        {
            //點擊 押標金投標文件退還作業
            string send_url = "~/pages/MPMS/D/MPMS_D1A.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_DOPEN=" + strOVC_DOPEN 
                            + "&ONB_TIMES=" + numONB_TIMES + "&ONB_GROUP=" + numONB_GROUP;
            Response.Redirect(send_url);
        }

        protected void btnTBMBID_DOC_LOG_Click(object sender, EventArgs e)
        {
            //點擊 開標紀錄分送作業
            string send_url = "~/pages/MPMS/D/MPMS_D19_3.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_DOPEN=" + strOVC_DOPEN 
                                + "&ONB_TIMES=" + numONB_TIMES + "&ONB_GROUP=" + numONB_GROUP;
            Response.Redirect(send_url);
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
            string OVC_DOPEN;
            int ONB_TIMES, ONB_GROUP;
            if (Request.QueryString["copyDOPEN"] == null || Request.QueryString["copyTIMES"] == null || Request.QueryString["copyGROUP"] == null)
            {
                OVC_DOPEN = strOVC_DOPEN;
                ONB_TIMES = numONB_TIMES;
                ONB_GROUP = numONB_GROUP;
            }
            else
            {
                OVC_DOPEN = Request.QueryString["copyDOPEN"]?.ToString();
                ONB_TIMES = short.Parse(Request.QueryString["copyTIMES"]?.ToString());
                ONB_GROUP = short.Parse(Request.QueryString["copyGROUP"]?.ToString());
            }

            string strOVC_PUR_IPURCH = "";
            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                //設定 廠商名稱下拉式選單
                SetDrpVENDOR(drpOVC_VENDORS_NAME, "Title");
                SetDrpVENDOR(drpOVC_EFFECTS_NAME, "Title");
                SetDrpVENDOR(drpOVC_VEN_CST_0, "ID");
                SetDrpVENDOR(drpOVC_VEN_CST_3, "ID");
                SetDrpVENDOR(drpOVC_VEN_TITLE_TBM1302, "ID");
                SetDrpVENDOR(drpOVC_VEN_TITLE_TBM1314, "ID");

                //設定 幣別下拉式選單
                SetTBM1407List("B0",drpOVC_BID_CURRENT_0);
                SetTBM1407List("B0",drpOVC_BID_CURRENT_3);
                SetTBM1407List("B0",drpOVC_CURRENT);
                SetTBM1407List("B0",drpOVC_COMMITTEE_CURRENT);
                SetTBM1407List("B0",drpOVC_RESULT_CURRENT);

                //設定 開標方式下拉式選單
                SetTBM1407List("R8",rdoOVC_OPEN_METHOD);


                TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
                tbmRECEIVE_ANNOUNCE =
                    mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                    && tb.OVC_DOPEN.Equals(OVC_DOPEN) && tb.ONB_TIMES == ONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_ANNOUNCE != null)
                {
                    lblOVC_DOPEN.Text = GetTaiwanDate(OVC_DOPEN) + " " + tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR + "時" + tbmRECEIVE_ANNOUNCE.OVC_OPEN_MIN + "分";
                    lblOVC_DANNOUNCE.Text = tbmRECEIVE_ANNOUNCE.OVC_DANNOUNCE;
                }



                TBM1303 tbm1303 = new TBM1303();
                tbm1303 = mpms.TBM1303.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                && tb.OVC_DOPEN.Equals(OVC_DOPEN) && tb.ONB_TIMES == ONB_TIMES
                                && tb.ONB_GROUP == ONB_GROUP).FirstOrDefault();
                if (tbm1303 != null)
                {
                    strOVC_PUR_IPURCH = "0";
                    TBM1201 tbm1201 = new TBM1201();
                    tbm1201 = mpms.TBM1201.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).OrderByDescending(tb => tb.ONB_POI_ICOUNT).FirstOrDefault();
                    if (tbm1201 != null)
                        strOVC_PUR_IPURCH = tbm1201.ONB_POI_ICOUNT.ToString();
                    txtOVC_ROOM.Text = tbm1303.OVC_ROOM;
                    txtOVC_CHAIRMAN.Text = tbm1303.OVC_CHAIRMAN;
                    lblOVC_PUR_IPURCH.Text = tbm1301.OVC_PUR_IPURCH + "等" + strOVC_PUR_IPURCH + "項";
                    lblOVC_PURCH_A_5.Text = tbm1303.OVC_PURCH + strOVC_PUR_AGENCY + strOVC_PURCH_5;
                    lblONB_GROUP.Text = numONB_GROUP.ToString();
                    txtONB_GROUP_TBM1302.Text = numONB_GROUP.ToString();
                    txtONB_GROUP_TBM1314.Text = numONB_GROUP.ToString();
                    lblOVC_PUR_APPROVE.Text = tbm1301.OVC_PUR_APPROVE;
                    SetOVC_RESULT(tbm1303.OVC_RESULT);

                    //(一)
                    string[] strOVC_METHODs = { tbm1303.OVC_METHOD_1, tbm1303.OVC_METHOD_2, tbm1303.OVC_METHOD_3 };
                    lblOVC_PUR_ASS_VEN_CODE.Text = GetTbm1407Desc("C7", tbm1301.OVC_PUR_ASS_VEN_CODE);
                    txtOVC_22_1_NO.Text = tbm1303.OVC_22_1_NO;
                    rdoOVC_OPEN_METHOD.SelectedValue = tbm1303.OVC_OPEN_METHOD;
                    lblONB_TIMES.Text = numONB_TIMES.ToString();
                    if (tbm1303.OVC_BID_METHOD != null)
                    {
                        RadioButton rdo = (RadioButton)this.Master.FindControl("MainContent").FindControl("rdoOVC_BID_METHOD_" + tbm1303.OVC_BID_METHOD);
                        rdo.Checked = true;
                    }
                    SetCheckeds(chkOVC_METHOD, strOVC_METHODs);


                    //(二)
                    string[] strOVC_RESTRICTs = { tbm1303.OVC_RESTRICT_1, tbm1303.OVC_RESTRICT_2 };
                    SetCheckeds(chkOVC_RESTRICT, strOVC_RESTRICTs);


                    //(三)開、審標

                    /*   ---- 審查結果：合格/不合格家數-----
                    var queryTBM1313 =
                        from tbm1313 in mpms.TBM1313
                        where tbm1313.OVC_PURCH.Equals(strOVC_PURCH) && tbm1313.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                && tbm1313.OVC_DOPEN.Equals(strOVC_DOPEN) && tbm1313.ONB_GROUP == numONB_GROUP
                        select tbm1313;

                    var queryOK_VENDORS = queryTBM1313.Where(tb => tb.OVC_RESULT == "Y").ToList();
                    txtONB_OK_VENDORS.Text = queryOK_VENDORS.Count().ToString();

                    var queryNOTOK_VENDORS = queryTBM1313.Where(tb => tb.OVC_RESULT == "Y").ToArray();
                    txtONB_NOTOK_VENDORS.Text = queryNOTOK_VENDORS.Count().ToString();
                    */

                    string[] strOVC_CHECK_RESULTs = { tbm1303.OVC_CHECK_RESULT_1, tbm1303.OVC_CHECK_RESULT_2 };
                    string[] strOVC_AUDITs = { tbm1303.OVC_AUDIT_SPEC, tbm1303.OVC_AUDIT_DOC };
                    lblONB_BID_VENDORS_Site.Text = GetVendors_Title("Table","1");
                    lblONB_BID_VENDORS_Comm.Text = GetVendors_Title("Table","2");
                    lblONB_BID_VENDORS_Elec.Text = GetVendors_Title("Table","3");
                    lblONB_BID_VENDORS.Text = GetVendors_Title("Table","All");
                    SetCheckeds(chkOVC_CHECK_RESULT, strOVC_CHECK_RESULTs);
                    txtONB_OK_VENDORS.Text = tbm1303.ONB_OK_VENDORS.ToString();
                    txtONB_NOTOK_VENDORS.Text = tbm1303.ONB_NOTOK_VENDORS.ToString();
                    txtOVC_VENDORS_NAME.Text = tbm1303.OVC_VENDORS_NAME;
                    SetCheckeds(chkOVC_AUDIT, strOVC_AUDITs);
                    SetChecked(chkOVC_AUDIT, tbm1303.OVC_AUDIT_OTHER == null ? null : "Other");
                    txtOVC_AUDIT_OTHER.Text = tbm1303.OVC_AUDIT_OTHER;
                    txtONB_AUDIT_DOC.Text = tbm1303.ONB_AUDIT_DOC.ToString();
                    chkOVC_CHECK_DOC.Checked = tbm1303.OVC_CHECK_DOC == "Y" ? true : false;
                    if (tbm1303.ONB_OK_EFFECT.ToString() != "")
                        chkONB_OK_EFFECT.Checked = true;
                    txtONB_OK_EFFECT.Text = tbm1303.ONB_OK_EFFECT.ToString();
                    if (tbm1303.ONB_NOTOK_EFFECT.ToString() != "")
                        chkONB_NOTOK_EFFECT.Checked = true;
                    txtONB_NOTOK_EFFECT.Text = tbm1303.ONB_NOTOK_EFFECT.ToString();
                    txtOVC_EFFECTS_NAME.Text = tbm1303.OVC_EFFECTS_NAME;
                    txtONB_RESULT_OK.Text = tbm1303.ONB_RESULT_OK.ToString();
                    txtONB_RESULT_NOTOK.Text = tbm1303.ONB_RESULT_NOTOK.ToString();


                    //(四)流、廢標
                    if (tbm1303.OVC_RESULT == "1")
                    { 
                        txtONB_BID_VENDOR_LAW.Text = tbm1303.ONB_BID_VENDOR_LAW.ToString();
                        SetCheckedOne_NotNull(chkOVC_BACK, tbm1303.OVC_BACK);
                    }
                    if (tbm1303.OVC_RESULT == "2")
                        SetChecked(rdoOVC_RESULT_REASON_2, tbm1303.OVC_RESULT_REASON);



                    //(五)決標
                    if (tbm1303.OVC_RESULT == "0")
                    {
                        drpOVC_VEN_CST_0.SelectedValue = tbm1303.OVC_VEN_CST;
                        txtOVC_VEN_TITLE_0.Text = tbm1303.OVC_VEN_TITLE;
                        if (tbm1303.OVC_FOLLOW_OK == "Y")
                            rdoOVC_FOLLOW_OK_Y.Checked = true;
                        else if (tbm1303.OVC_FOLLOW_OK == "N")
                        {
                            rdoOVC_FOLLOW_OK_N.Checked = true;
                            drpOVC_BID_CURRENT_0.SelectedValue = tbm1303.OVC_BID_CURRENT;
                            txtONB_BID_MONEY_0.Text = tbm1303.ONB_BID_MONEY.ToString();
                        }
                        switch (tbm1303.OVC_RESULT_REASON)
                        {
                            case ("在底價內"):
                                rdoOVC_RESULT_REASON_0_0.Checked = true;
                                drpOVC_CURRENT.SelectedValue = tbm1303.OVC_CURRENT;
                                txtONB_BID_BUDGET.Text = tbm1303.ONB_BID_BUDGET.ToString();
                                break;

                            case ("底價內"):
                                rdoOVC_RESULT_REASON_0_0.Checked = true;
                                drpOVC_CURRENT.SelectedValue = tbm1303.OVC_CURRENT;
                                txtONB_BID_BUDGET.Text = tbm1303.ONB_BID_BUDGET.ToString();
                                break;

                            case ("評審委員會認為廠商報價合理且在預算以內"):
                                rdoOVC_RESULT_REASON_0_1.Checked = true;
                                break;

                            case ("在評審委員會建議金額內"):
                                rdoONB_COMMITTEE_BUDGET.Checked = true;
                                drpOVC_COMMITTEE_CURRENT.SelectedValue = tbm1303.OVC_COMMITTEE_CURRENT;
                                SetCheckedOne(rdoOVC_RESULT_REASON_0_2, tbm1303.OVC_RESULT_REASON);
                                break;

                            case ("評選委員會依規定評選為最有利標"):
                                rdoOVC_RESULT_REASON_0_2.Checked = true;
                                txtOVC_RESULT_DESC.Text = tbm1303.OVC_RESULT_DESC;
                                txtOVC_LAW_ITEM.Text = tbm1303.OVC_LAW_ITEM;
                                txtOVC_LAW_NO.Text = tbm1303.OVC_LAW_NO;
                                break;
                        } 
                    }


                    //(六)保留開標結果
                    if (tbm1303.OVC_RESULT == "3")
                    {
                        rdoOVC_RESULT_3.Checked = true;
                        drpOVC_VEN_CST_3.SelectedValue = tbm1303.OVC_VEN_CST;
                        txtOVC_VEN_TITLE_3.Text = tbm1303.OVC_VEN_TITLE;
                        drpOVC_BID_CURRENT_3.SelectedValue = tbm1303.OVC_BID_CURRENT;
                        txtONB_BID_MONEY_3.Text = tbm1303.ONB_BID_MONEY.ToString();
                    }


                    //(七)不予開標、決標
                    if (tbm1303.OVC_RESULT == "4")
                        txtOVC_NONE_BID_NO.Text = tbm1303.OVC_NONE_BID_NO;
  
                    txtOVC_DAPPROVE.Text = tbm1303.OVC_DAPPROVE;



                    //第二頁
                    lblOVC_PURCH.Text = tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                    lblONB_GROUP_TBM1313.Text = numONB_GROUP.ToString();
                    lblOVC_LAW_ITEM.Text = tbm1303.OVC_LAW_ITEM;
                    lblOVC_LAW_NO.Text = tbm1303.OVC_LAW_NO;
                    drpOVC_RESULT_CURRENT.SelectedValue = tbm1303.OVC_RESULT_CURRENT;
                    txtONB_BID_RESULT.Text = tbm1303.ONB_BID_RESULT.ToString();
                    txtOVC_ADDITIONAL.Text = tbm1303.OVC_ADDITIONAL;
                    string[] strOVC_FINISH = { tbm1303.OVC_FINISH_1, tbm1303.OVC_FINISH_2 };
                    SetCheckeds(chkOVC_FINISH, strOVC_FINISH);
                    txtOVC_ADVICE.Text = tbm1303.OVC_ADVICE;
                }

                //設定 幣別下拉式選單預設為新台幣
                SetCURRENT(drpOVC_BID_CURRENT_0, drpOVC_BID_CURRENT_0.SelectedValue);
                SetCURRENT(drpOVC_BID_CURRENT_3, drpOVC_BID_CURRENT_3.SelectedValue);
                SetCURRENT(drpOVC_CURRENT, drpOVC_CURRENT.SelectedValue);
                SetCURRENT(drpOVC_COMMITTEE_CURRENT, drpOVC_COMMITTEE_CURRENT.SelectedValue);
                SetCURRENT(drpOVC_RESULT_CURRENT, drpOVC_RESULT_CURRENT.SelectedValue);

            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查無此購案");
        }

        private void DataImport_Gv()
        {
            //投標廠商報價比價表
            DataTable dtTBM1314;
            var queryTBM1314 =
                from tbm1314 in mpms.TBM1314
                where tbm1314.OVC_PURCH.Equals(strOVC_PURCH) && tbm1314.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                        && tbm1314.OVC_DOPEN.Equals(strOVC_DOPEN) && tbm1314.ONB_GROUP == numONB_GROUP
                orderby tbm1314.OVC_DOPEN, tbm1314.ONB_GROUP, tbm1314.OVC_VEN_CST
                select new
                {
                    ONB_GROUP = tbm1314.ONB_GROUP,
                    OVC_VEN_CST = tbm1314.OVC_VEN_CST,
                    OVC_VEN_TITLE = tbm1314.OVC_VEN_TITLE,
                    ONB_MBID = tbm1314.ONB_MBID,
                    ONB_MINIS_LOWEST = tbm1314.ONB_MINIS_LOWEST,
                    ONB_MINIS_1 = tbm1314.ONB_MINIS_1,
                    ONB_MINIS_2 = tbm1314.ONB_MINIS_2,
                    ONB_MINIS_3 = tbm1314.ONB_MINIS_3,
                    OVC_MBID = tbm1314.OVC_MBID,
                    OVC_MINIS_LOWEST = tbm1314.OVC_MINIS_LOWEST,
                    OVC_MINIS_1 = tbm1314.OVC_MINIS_1,
                    OVC_MINIS_2 = tbm1314.OVC_MINIS_2,
                    OVC_MINIS_3 = tbm1314.OVC_MINIS_3,
                    OVC_KMBID = tbm1314.OVC_KMBID,
                    OVC_KMINIS_LOWEST = tbm1314.OVC_KMINIS_LOWEST,
                    OVC_KMINIS_1 = tbm1314.OVC_KMINIS_1,
                    OVC_KMINIS_2 = tbm1314.OVC_KMINIS_2,
                    OVC_KMINIS_3 = tbm1314.OVC_KMINIS_3,
                };
            dtTBM1314 = CommonStatic.LinqQueryToDataTable(queryTBM1314);
            foreach (DataRow rows in dtTBM1314.Rows)
            {
                rows["OVC_MBID"] = GetTbm1407Desc("U1", rows["OVC_MBID"].ToString());
                rows["OVC_MINIS_LOWEST"] = GetTbm1407Desc("U1", rows["OVC_MINIS_LOWEST"].ToString());
                rows["OVC_MINIS_1"] = GetTbm1407Desc("U1", rows["OVC_MINIS_1"].ToString());
                rows["OVC_MINIS_2"] = GetTbm1407Desc("U1", rows["OVC_MINIS_2"].ToString());
                rows["OVC_MINIS_3"] = GetTbm1407Desc("U1", rows["OVC_MINIS_3"].ToString());
                rows["OVC_KMBID"] = GetOVC_KMBID(rows["OVC_KMBID"].ToString());
                rows["OVC_KMINIS_LOWEST"] = GetOVC_KMBID(rows["OVC_KMINIS_LOWEST"].ToString());
                rows["OVC_KMINIS_1"] = GetOVC_KMBID(rows["OVC_KMINIS_1"].ToString());
                rows["OVC_KMINIS_2"] = GetOVC_KMBID(rows["OVC_KMINIS_2"].ToString());
                rows["OVC_KMINIS_3"] = GetOVC_KMBID(rows["OVC_KMINIS_3"].ToString());
            }
            FCommon.GridView_dataImport(gvTBM1314, dtTBM1314);


            //廠商資料
            DataTable dtTBM1302;
            var queryTBM1302 =
                from tbm1302 in mpms.TBM1302
                where tbm1302.OVC_PURCH.Equals(strOVC_PURCH) && tbm1302.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                        && tbm1302.OVC_DOPEN.Equals(strOVC_DOPEN) && tbm1302.ONB_GROUP == numONB_GROUP
                orderby tbm1302.OVC_DOPEN, tbm1302.ONB_GROUP, tbm1302.OVC_VEN_CST
                select new
                {
                    ONB_GROUP = tbm1302.ONB_GROUP,
                    OVC_VEN_CST = tbm1302.OVC_VEN_CST,
                    OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE,
                    OVC_VEN_TEL = tbm1302.OVC_VEN_TEL,
                    OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                };

            dtTBM1302 = CommonStatic.LinqQueryToDataTable(queryTBM1302);
            FCommon.GridView_dataImport(gvTBM1302, dtTBM1302);
        }



        private void SetTBM1407List(string cateID,ListControl list)
        {
            DataTable dt;
            dt = CommonStatic.ListToDataTable(gm.TBM1407.Where(tb => tb.OVC_PHR_CATE.Equals(cateID)).ToList());
            if(cateID=="B0")
                FCommon.list_dataImportV(list, dt, "OVC_PHR_DESC", "OVC_PHR_ID", true);
            else
                FCommon.list_dataImport(list, dt, "OVC_PHR_DESC", "OVC_PHR_ID", false);
        }

        private void SetCURRENT(ListControl list, string strSelected)
        {
            if (strSelected == "" || strSelected == null)
                list.SelectedValue = "N";
        }



            private void SetDrpVENDOR(ListControl list,string valueType)
        {
            DataTable dtTBM1313;
            var queryTBM1313 =
                from tbm1313 in mpms.TBM1313
                where tbm1313.OVC_PURCH.Equals(strOVC_PURCH) && tbm1313.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                   && tbm1313.OVC_DOPEN.Equals(strOVC_DOPEN) && tbm1313.ONB_GROUP == numONB_GROUP
                orderby tbm1313.ONB_GROUP
                select new
                {
                    OVC_VEN_CST = tbm1313.OVC_VEN_CST,
                    OVC_VEN_TITLE = tbm1313.OVC_VEN_TITLE,
                };
            dtTBM1313 = CommonStatic.LinqQueryToDataTable(queryTBM1313);

            if (valueType =="ID")
                FCommon.list_dataImport(list, dtTBM1313, "OVC_VEN_TITLE", "OVC_VEN_CST", true);
            else if (valueType == "Title")
                FCommon.list_dataImport(list, dtTBM1313, "OVC_VEN_TITLE", "OVC_VEN_TITLE", true);

        }


        private void SetCheckeds(CheckBoxList list,string[] strCheckeds)
        {
            foreach (string strSelected in strCheckeds)
            {
                System.Web.UI.WebControls.ListItem newItme = (System.Web.UI.WebControls.ListItem)list.Items.FindByValue(strSelected);
                if (newItme != null)
                    newItme.Selected = true;
            }
        }

        private void SetChecked(ListControl list, string strChecked)
        {
            if (strChecked != "")
            {
                System.Web.UI.WebControls.ListItem newItme = (System.Web.UI.WebControls.ListItem)list.Items.FindByValue(strChecked);
                if (newItme != null)
                    newItme.Selected = true;
            }
        }


        private void SetCheckedOne_NotNull(CheckBox chk, string strChecked)
        {
            if (strChecked != "0" && strChecked != "" && strChecked != null && strChecked != "N")
                chk.Checked = true;
        }

        private void SetCheckedOne_NotNull(RadioButton rdo, string strChecked)
        {
            if (strChecked != "0" && strChecked != "" && strChecked != null && strChecked != "N")
                rdo.Checked = true;
        }

        private void SetCheckedOne(RadioButton rdo, string strChecked)
        {
            if (rdo.Text == strChecked)
                rdo.Checked = true;
        }

        private void SetCheckedOne(RadioButton rdo, string strChecked1, string strChecked2)
        {
            if ((strChecked1 != null && strChecked1 != "") || (strChecked2 != null && strChecked2 != ""))
                rdo.Checked = true;
        }

        private void SetOVC_RESULT(string strOVC_RESULT)
        {
            switch (strOVC_RESULT)
            {
                case "0":   //決標
                    rdoOVC_RESULT_0.Checked = true;
                    rdoOVC_RESULT_1.Checked = false;
                    rdoOVC_RESULT_2.Checked = false;
                    rdoOVC_RESULT_3.Checked = false;
                    rdoOVC_RESULT_4.Checked = false;
                    break;
                case "1":   //流標
                    rdoOVC_RESULT_1.Checked = true;
                    rdoOVC_RESULT_0.Checked = false;
                    rdoOVC_RESULT_2.Checked = false;
                    rdoOVC_RESULT_3.Checked = false;
                    rdoOVC_RESULT_4.Checked = false;
                    break;
                case "2":   //廢標
                    rdoOVC_RESULT_2.Checked = true;
                    rdoOVC_RESULT_0.Checked = false;
                    rdoOVC_RESULT_1.Checked = false;
                    rdoOVC_RESULT_3.Checked = false;
                    rdoOVC_RESULT_4.Checked = false;
                    break;
                case "3":   //保留開標結果
                    rdoOVC_RESULT_3.Checked = true;
                    rdoOVC_RESULT_0.Checked = false;
                    rdoOVC_RESULT_1.Checked = false;
                    rdoOVC_RESULT_2.Checked = false;
                    rdoOVC_RESULT_4.Checked = false;
                    break;
                case "4":   //保留開標結果
                    rdoOVC_RESULT_4.Checked = true;
                    rdoOVC_RESULT_0.Checked = false;
                    rdoOVC_RESULT_1.Checked = false;
                    rdoOVC_RESULT_2.Checked = false;
                    rdoOVC_RESULT_3.Checked = false;
                    break;
                default:
                    break;
                    
            }
        }
        

        private string GetOVC_BID_METHOD()
        {
            if (rdoOVC_BID_METHOD_1.Checked == true)
                return "1";
            if (rdoOVC_BID_METHOD_2.Checked == true)
                return "2";
            else if (rdoOVC_BID_METHOD_3.Checked == true)
                return "3";
            return "";
        }

        

        private string GetVendors_Title(string type,string strOVC_KIND)
        {
            //取得各類投標廠商總計
            string strTitle="", strVENDORS, strOVC_KIND_Name="";
            DataTable dt;
            switch (strOVC_KIND)
            {
                case "1":
                    strOVC_KIND_Name = "現場投標 ";
                    break;
                case "2":
                    strOVC_KIND_Name = "通信投標 ";
                    break;
                case "3":
                    strOVC_KIND_Name = "電子投標 ";
                    break;
                default:
                    break;
            }
            if (strOVC_KIND == "All")
            {
                var queryTBM1313 =
                    (from tbm1313 in mpms.TBM1313
                     where tbm1313.OVC_PURCH.Equals(strOVC_PURCH) && tbm1313.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                        && tbm1313.OVC_DOPEN.Equals(strOVC_DOPEN) && tbm1313.ONB_GROUP == numONB_GROUP
                     select tbm1313);
                return queryTBM1313.Count().ToString();
            }
            else if (strOVC_KIND == "1+2")
            {
                var queryTBM1313 =
                    from tbm1313 in mpms.TBM1313
                    where tbm1313.OVC_PURCH.Equals(strOVC_PURCH) && tbm1313.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                       && tbm1313.OVC_DOPEN.Equals(strOVC_DOPEN) && tbm1313.ONB_GROUP == numONB_GROUP
                       && (tbm1313.OVC_KIND.Equals("1") || tbm1313.OVC_KIND.Equals("2"))
                    select new
                    {
                        OVC_VEN_CST = tbm1313.OVC_VEN_CST,
                        OVC_VEN_TITLE = tbm1313.OVC_VEN_TITLE,
                    };
                dt = CommonStatic.LinqQueryToDataTable(queryTBM1313);

                if (queryTBM1313.Count() != 0 && type == "Word")
                {
                    strVENDORS = queryTBM1313.Count().ToString();
                }
                else
                    strVENDORS = "0";
                return strVENDORS;
            }


            else
            {
                var queryTBM1313 =
                    from tbm1313 in mpms.TBM1313
                    where tbm1313.OVC_PURCH.Equals(strOVC_PURCH) && tbm1313.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                       && tbm1313.OVC_DOPEN.Equals(strOVC_DOPEN) && tbm1313.ONB_GROUP == numONB_GROUP
                       && tbm1313.OVC_KIND.Equals(strOVC_KIND)
                    select new
                    {
                        OVC_VEN_CST = tbm1313.OVC_VEN_CST,
                        OVC_VEN_TITLE = tbm1313.OVC_VEN_TITLE,
                    };
                dt = CommonStatic.LinqQueryToDataTable(queryTBM1313);

                if (queryTBM1313.Count() != 0)
                {
                    foreach (DataRow rows in dt.Rows)
                    {
                        if (strTitle == "")
                            strTitle += rows["OVC_VEN_TITLE"].ToString();
                        else
                            strTitle += " , " + rows["OVC_VEN_TITLE"].ToString();
                    }
                    strVENDORS = queryTBM1313.Count().ToString();
                    if (type == "Table")
                        return strOVC_KIND_Name + strVENDORS + " 家，為 " + strTitle + "。<BR />";
                    else if (type == "Word")
                        return strVENDORS;
                }
            }
            return "";
        }

        
   
        
        private string[] GetSaveValue(ListControl list)
        {
            string[] strSaveValues = new string [list.Items.Count-1];
            for (int i = 0; i < list.Items.Count; i++)
            {
                if (list.Items[i].Selected)
                    strSaveValues[i] = list.Items[i].Value;
                else
                    strSaveValues[i] = "";
            }
            return strSaveValues;
        }

        private string GetSaveValue(ListControl list,int i)
        {
            if (list.Items[i].Selected)
                return list.Items[i].Value;
            else
                return "";
        }




        private string GetOVC_RESULT()
        {
            if (rdoOVC_RESULT_0.Checked == true)
                return "0";
            else if (rdoOVC_RESULT_1.Checked == true)
                return "1";
            else if (rdoOVC_RESULT_2.Checked == true)
                return "2";
            else if (rdoOVC_RESULT_3.Checked == true)
                return "3";
            else if (rdoOVC_RESULT_4.Checked == true)
                return "4";
            return "";
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


        private String GetTbm1407ID(string cateID, string codeDesc)
        {
            //將TBM1407片語ID代碼轉為Desc
            TBM1407 tbm1407 = new TBM1407();
            if (codeDesc != null && codeDesc != "")
            {
                tbm1407 = gm.TBM1407.Where(tb => tb.OVC_PHR_CATE.Equals(cateID) && tb.OVC_PHR_DESC.Equals(codeDesc)).OrderBy(tb => tb.OVC_PHR_ID).FirstOrDefault();
                if (tbm1407 != null)
                {
                    return tbm1407.OVC_PHR_ID.ToString();
                }
            }
            return codeDesc;
        }

        

        private String GetOVC_KMBID(string codeID)
        {
            string codeName="";
            switch (codeID)
            {
                case "1":
                    codeName = "(決)";
                    break;

                case "2":
                    codeName = "(廢)";
                    break;

                case "3":
                    codeName = "(保留)";
                    break;

                case "4":
                    codeName = "(並列)";
                    break;

                case "5":
                    codeName = "(無效標)";
                    break;

                case "6":
                    codeName = "(不合格標)";
                    break;

                case "7":
                    codeName = "(以上皆非)";
                    break;
            }
            if (codeName == "")
                return codeID;
            else
                return codeName;
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




        #region 輸出Word檔

        private void GetWordD19_1_1()
        {
            //開、決標、比議價紀錄.docx
            var targetPath = Server.MapPath("~/WordPDFprint/");
            string fileName = strOVC_PURCH + "-開、決標、比議價紀錄.docx";
            File.Copy(targetPath + "D19_1_1-開、決標、比議價紀錄.docx", targetPath + fileName);
            var valuesToFill = new TemplateEngine.Docx.Content();

            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;

                /*
                string strOVC_PUR_IPURCH = "0";
                TBM1201 tbm1201 = new TBM1201();
                tbm1201 = mpms.TBM1201.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).OrderByDescending(tb => tb.ONB_POI_ICOUNT).FirstOrDefault();
                if (tbm1201 != null)
                    strOVC_PUR_IPURCH = tbm1201.ONB_POI_ICOUNT.ToString();
                */
                valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IPURCH", tbm1301.OVC_PUR_IPURCH == null ? "" : tbm1301.OVC_PUR_IPURCH));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_ASS_VEN_CODE", GetTbm1407Desc("C7", tbm1301.OVC_PUR_ASS_VEN_CODE == null ? "" : tbm1301.OVC_PUR_ASS_VEN_CODE)));
                valuesToFill.Fields.Add(new FieldContent("ONB_TIMES", numONB_TIMES.ToString()));
                
                TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
                tbmRECEIVE_ANNOUNCE =
                    mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                    && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_ANNOUNCE != null)
                    valuesToFill.Fields.Add(new FieldContent("OVC_DANNOUNCE", tbmRECEIVE_ANNOUNCE.OVC_DANNOUNCE == null? "" : GetTaiwanDate(tbmRECEIVE_ANNOUNCE.OVC_DANNOUNCE)));

                var query1314 =
                    from tbm1314 in mpms.TBM1314
                    where tbm1314.OVC_PURCH.Equals(strOVC_PURCH) && tbm1314.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                         && tbm1314.OVC_DOPEN.Equals(strOVC_DOPEN) && tbm1314.ONB_GROUP == numONB_GROUP
                    select tbm1314;

                DataTable dt = CommonStatic.LinqQueryToDataTable(query1314);

                var tableContent = new TableContent("row");
                if (dt.Rows.Count != 0)
                {
                    string strOVC_VEN_TITLE, strONB_MBID, strONB_MINIS_LOWEST, strONB_MINIS_1, strONB_MINIS_2, strONB_MINIS_3;
                    foreach (DataRow rows in dt.Rows)
                    {
                        strOVC_VEN_TITLE = rows["OVC_VEN_TITLE"] == null ? "" : rows["OVC_VEN_TITLE"].ToString();
                        strONB_MBID = rows["ONB_MBID"] == null ? "" : rows["ONB_MBID"].ToString();
                        strONB_MINIS_LOWEST = rows["ONB_MINIS_LOWEST"] == null ? "" : rows["ONB_MINIS_LOWEST"].ToString();
                        strONB_MINIS_1 = rows["ONB_MINIS_1"] == null ? "" : rows["ONB_MINIS_1"].ToString();
                        strONB_MINIS_2 = rows["ONB_MINIS_2"] == null ? "" : rows["ONB_MINIS_2"].ToString();
                        strONB_MINIS_3 = rows["ONB_MINIS_3"] == null ? "" : rows["ONB_MINIS_3"].ToString();


                        tableContent.AddRow(
                            new FieldContent("OVC_VEN_TITLE", strOVC_VEN_TITLE),
                            new FieldContent("ONB_MBID", strONB_MBID),
                            new FieldContent("ONB_MINIS_LOWEST", strONB_MINIS_LOWEST),
                            new FieldContent("ONB_MINIS_1", strONB_MINIS_1),
                            new FieldContent("ONB_MINIS_2", strONB_MINIS_2),
                            new FieldContent("ONB_MINIS_3", strONB_MINIS_3)
                        );
                    }
                }
                else
                {
                    tableContent.AddRow(
                            new FieldContent("OVC_VEN_TITLE", ""),
                            new FieldContent("ONB_MBID", ""),
                            new FieldContent("ONB_MINIS_LOWEST", ""),
                            new FieldContent("ONB_MINIS_1", ""),
                            new FieldContent("ONB_MINIS_2", ""),
                            new FieldContent("ONB_MINIS_3", "")
                    );
                }
                valuesToFill.Tables.Add(tableContent);
            }

            valuesToFill.Fields.Add(new FieldContent("Total_SiteComm", GetVendors_Title("Word", "1+2")));
            valuesToFill.Fields.Add(new FieldContent("Total_Elec", GetVendors_Title("Word", "3")));
            valuesToFill.Fields.Add(new FieldContent("Total_VENDOR", GetVendors_Title("Word", "All")));
            //valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_Y", GetOVC_RESULT_Y()));


            TBM1303 tbm1303 = new TBM1303();
            tbm1303 = mpms.TBM1303.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_TIMES == numONB_TIMES
                            && tb.ONB_GROUP == numONB_GROUP).FirstOrDefault();
            if (tbm1303 != null)
            {
                valuesToFill.Fields.Add(new FieldContent("OVC_PLACE", tbm1303.OVC_PLACE == null ? "" : tbm1303.OVC_PLACE));
                valuesToFill.Fields.Add(new FieldContent("OVC_ROOM", tbm1303.OVC_ROOM == null ? "" : tbm1303.OVC_ROOM));                
                valuesToFill.Fields.Add(new FieldContent("GroupVendors", GetGroupVendors()));
                valuesToFill.Fields.Add(new FieldContent("ONB_RESULT_OK", tbm1303.ONB_RESULT_OK == null ? "" : tbm1303.ONB_RESULT_OK.ToString()));
                valuesToFill.Fields.Add(new FieldContent("ONB_RESULT_NOTOK", tbm1303.ONB_RESULT_NOTOK == null ? "" : tbm1303.ONB_RESULT_NOTOK.ToString()));
                //valuesToFill.Fields.Add(new FieldContent("OVC_VENDORS_NAME", tbm1303.OVC_VENDORS_NAME == null ? "" : tbm1303.OVC_VENDORS_NAME.ToString()));

                valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_CURRENT", tbm1303.OVC_RESULT_CURRENT == null ? "" : GetTbm1407Desc("B0", tbm1303.OVC_RESULT_CURRENT)));
                valuesToFill.Fields.Add(new FieldContent("ONB_BID_RESULT", tbm1303.ONB_BID_RESULT == null ? "" : GetTaiwanNumber(tbm1303.ONB_BID_RESULT.ToString())));
                valuesToFill.Fields.Add(new FieldContent("OVC_CHAIRMAN", tbm1303.OVC_CHAIRMAN == null ? "" : tbm1303.OVC_CHAIRMAN));

                if (tbm1303.OVC_RESULT != null)
                {
                    switch (tbm1303.OVC_RESULT)
                    {
                        case "0":   //一、決標
                            valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_0", "■"));
                            if (tbm1303.OVC_VEN_TITLE != null)
                                valuesToFill.Fields.Add(new FieldContent("chkOVC_VEN_TITLE_0", tbm1303.OVC_VEN_TITLE == null ? "□" : "■"));
                            valuesToFill.Fields.Add(new FieldContent("OVC_VEN_TITLE_0", tbm1303.OVC_VEN_TITLE == null ? "" : tbm1303.OVC_VEN_TITLE));
                            valuesToFill.Fields.Add(new FieldContent("OVC_FOLLOW_OK_Y", tbm1303.OVC_FOLLOW_OK == "Y" ? "■" : "□"));
                            valuesToFill.Fields.Add(new FieldContent("OVC_FOLLOW_OK_N", tbm1303.OVC_FOLLOW_OK == "N" ? "■" : "□"));
                            if (tbm1303.OVC_FOLLOW_OK == "N")
                            {
                                valuesToFill.Fields.Add(new FieldContent("OVC_BID_CURRENT_0", tbm1303.OVC_BID_CURRENT == null ? "" : GetTbm1407Desc("B0",tbm1303.OVC_BID_CURRENT)));
                                if ((tbm1303.ONB_BID_MONEY).HasValue)
                                    valuesToFill.Fields.Add(new FieldContent("ONB_BID_MONEY_0", tbm1303.ONB_BID_MONEY.ToString()));
                                else
                                    valuesToFill.Fields.Add(new FieldContent("ONB_BID_MONEY_0", ""));
                            }

                            if (tbm1303.OVC_RESULT_REASON == "在底價內")
                            {
                                valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_REASON_0_1", "■"));
                                valuesToFill.Fields.Add(new FieldContent("OVC_CURRENT", tbm1303.OVC_CURRENT == null ? "" : GetTbm1407Desc("B0", tbm1303.OVC_CURRENT)));
                                if ((tbm1303.ONB_BID_BUDGET).HasValue)
                                    valuesToFill.Fields.Add(new FieldContent("ONB_BID_BUDGET", tbm1303.ONB_BID_BUDGET.ToString()));
                                else
                                    valuesToFill.Fields.Add(new FieldContent("ONB_BID_BUDGET", ""));
                            }

                            else if (tbm1303.OVC_RESULT_REASON == "評審委員會認為廠商報價合理且在預算以內")
                            {
                                valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_REASON_0_2", "■"));
                                valuesToFill.Fields.Add(new FieldContent("OVC_COMMITTEE_CURRENT", tbm1303.OVC_COMMITTEE_CURRENT == null ? "" : GetTbm1407Desc("B0", tbm1303.OVC_COMMITTEE_CURRENT)));
                                if ((tbm1303.ONB_COMMITTEE_BUDGET).HasValue)
                                    valuesToFill.Fields.Add(new FieldContent("ONB_COMMITTEE_BUDGET", tbm1303.ONB_COMMITTEE_BUDGET.ToString()));
                                else
                                    valuesToFill.Fields.Add(new FieldContent("ONB_COMMITTEE_BUDGET", ""));
                            }
                            
                            else if (tbm1303.OVC_RESULT_REASON == "在評審委員會建議金額內")
                                valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_REASON_0_3", "■"));

                            else if (tbm1303.OVC_RESULT_REASON == "評選委員會依規定評選為最有利標")
                            {
                                valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_REASON_0_4", "■"));
                                valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_DESC", tbm1303.OVC_RESULT_DESC == null ? "" : tbm1303.OVC_RESULT_DESC));
                                valuesToFill.Fields.Add(new FieldContent("OVC_LAW_ITEM", tbm1303.OVC_LAW_ITEM == null ? "" : tbm1303.OVC_LAW_ITEM));
                                valuesToFill.Fields.Add(new FieldContent("OVC_LAW_NO", tbm1303.OVC_LAW_NO == null ? "" : tbm1303.OVC_LAW_NO));
                            }
                            break;


                        case "1":   //流標
                            valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_1", "■"));
                            if (tbm1303.ONB_BID_VENDOR_LAW != null)
                                valuesToFill.Fields.Add(new FieldContent("chkONB_BID_VENDOR_LAW", "■"));
                            valuesToFill.Fields.Add(new FieldContent("ONB_BID_VENDOR_LAW", tbm1303.ONB_BID_VENDOR_LAW == null ? "" : tbm1303.ONB_BID_VENDOR_LAW.ToString()));
                            if(tbm1303.OVC_BACK == "Y")
                                valuesToFill.Fields.Add(new FieldContent("OVC_BACK", "■"));
                            break;


                        case "2":   //廢標
                            valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_2", "■"));
                            if(tbm1303.OVC_RESULT_REASON== "最低報價超底價")
                                valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_REASON_2_1", "■"));
                            else if(tbm1303.OVC_RESULT_REASON == "最低報價逾評審委員會建議金額")
                                valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_REASON_2_2", "■"));
                            else if (tbm1303.OVC_RESULT_REASON == "經審標結果，無得為決標對象之廠商")
                                valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_REASON_2_3", "■"));
                            else if (tbm1303.OVC_RESULT_REASON == "不予開標或不予決標，致採購程序無法進行者，本案廢標")
                                valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_REASON_2_4", "■"));
                            break;


                        case "3":   //保留開標結果
                            valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_3", "■"));
                            if (tbm1303.OVC_VEN_TITLE != null && tbm1303.OVC_VEN_TITLE != "")
                            {
                                valuesToFill.Fields.Add(new FieldContent("chkOVC_VEN_TITLE_3", "■"));
                                valuesToFill.Fields.Add(new FieldContent("OVC_VEN_TITLE_3", tbm1303.OVC_VEN_TITLE == null ? "" : tbm1303.OVC_VEN_TITLE));
                            }
                            valuesToFill.Fields.Add(new FieldContent("OVC_BID_CURRENT_3", tbm1303.OVC_BID_CURRENT == null ? "" : GetTbm1407Desc("B0", tbm1303.OVC_BID_CURRENT)));
                            if ((tbm1303.ONB_BID_MONEY).HasValue)
                                valuesToFill.Fields.Add(new FieldContent("ONB_BID_MONEY_3", tbm1303.ONB_BID_MONEY.ToString()));
                            else
                                valuesToFill.Fields.Add(new FieldContent("ONB_BID_MONEY_3", ""));
                            break;

                        /*
                        case "4":   //不予開標、決標
                            valuesToFill.Fields.Add(new FieldContent("OVC_RESULT_4", "■"));
                            //OVC_RESULT_REASON
                            //OVC_NONE_BID_NO
                            break;
                        */

                        default:
                            break;

                    }
                }
                TBM1302 tbm1302 = mpms.TBM1302.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP).FirstOrDefault();
                if (tbm1302 != null)
                {
                    valuesToFill.Fields.Add(new FieldContent("TBM1302_OVC_VEN_TITLE", tbm1302.OVC_VEN_TITLE == null ? "" : tbm1302.OVC_VEN_TITLE));
                    //valuesToFill.Fields.Add(new FieldContent("TBM1302_OVC_CURRENT", tbm1302.OVC_CURRENT == null ? "" : GetTbm1407Desc("B0",tbm1302.OVC_CURRENT)));
                    //valuesToFill.Fields.Add(new FieldContent("TBM1302_ONB_MONEY", tbm1302.ONB_MONEY == null ? "" : GetTaiwanNumber(tbm1302.ONB_MONEY.ToString())));
                    valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_6", tbm1302.OVC_PURCH_6 == null ? "" : tbm1302.OVC_PURCH_6));
                }
            }


            using (var outputDocument = new TemplateProcessor(targetPath + fileName).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }
            FileInfo fileInfo = new FileInfo(targetPath + fileName);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            System.IO.File.Delete(targetPath + fileName);
            Response.End();
        }

        



        private void GetWordD19_1_2()
        {
            //廠商減價單(分組)
            var targetPath = Server.MapPath("~/WordPDFprint/");
            string fileName = strOVC_PURCH + "-廠商減價單(分組).docx";
            File.Copy(targetPath + "D19_1_2-廠商減價單(分組).docx", targetPath + fileName);

            string strOVC_PUR_IPURCH = "";
            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
                strOVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH;

            var queryGroupID =
                (from tbm1314Group in mpms.TBM1314
                 where tbm1314Group.OVC_PURCH.Equals(strOVC_PURCH) && tbm1314Group.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                         && tbm1314Group.OVC_DOPEN.Equals(strOVC_DOPEN)
                 orderby tbm1314Group.ONB_GROUP
                 select new
                 {
                     OVC_PURCH = tbm1314Group.OVC_PURCH,
                     ONB_GROUP = tbm1314Group.ONB_GROUP,
                 }).Distinct();

            var queryData =
                from tbm1314Data in mpms.TBM1314
                where tbm1314Data.OVC_PURCH.Equals(strOVC_PURCH) && tbm1314Data.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                        && tbm1314Data.OVC_DOPEN.Equals(strOVC_DOPEN)
                orderby tbm1314Data.ONB_GROUP
                select tbm1314Data;

            var queryTBM1314 =
                (from groupID in queryGroupID.AsEnumerable()
                 join tbm1314 in queryData on groupID.ONB_GROUP equals tbm1314.ONB_GROUP into tbGroup
                 select new
                 {
                     OVC_PURCH = groupID.OVC_PURCH,
                     ONB_GROUP = groupID.ONB_GROUP,
                     ONB_MINIS_LOWEST = tbGroup.Sum((tb => tb.ONB_MINIS_LOWEST)),
                     ONB_MINIS_1 = tbGroup.Sum((tb => tb.ONB_MINIS_1)),
                     ONB_MINIS_2 = tbGroup.Sum((tb => tb.ONB_MINIS_2)),
                     ONB_MINIS_3 = tbGroup.Sum((tb => tb.ONB_MINIS_3)),
                     ONB_MINIS_4 = tbGroup.Sum((tb => tb.ONB_MINIS_4)),
                     ONB_MINIS_5 = tbGroup.Sum((tb => tb.ONB_MINIS_5)),
                     ONB_MINIS_6 = tbGroup.Sum((tb => tb.ONB_MINIS_6)),
                     ONB_MINIS_7 = tbGroup.Sum((tb => tb.ONB_MINIS_7)),
                     ONB_MINIS_8 = tbGroup.Sum((tb => tb.ONB_MINIS_8)),
                     OVC_VEN_TITLE = string.Join(",", tbGroup.Select(tb => tb.OVC_VEN_TITLE)),
                 }).ToArray();

            DataTable dt = CommonStatic.LinqQueryToDataTable(queryTBM1314);

            
            var valuesToFill = new TemplateEngine.Docx.Content();
            var tableContent = new TableContent("table");
            
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow rows in dt.Rows)
                {
                    /* -------------Table Content----------------- */
                    tableContent.AddRow
                    (
                        new FieldContent("OVC_PUR_IPURCH", strOVC_PUR_IPURCH),
                        new FieldContent("OVC_PURCH_A_5", strOVC_PURCH + strOVC_PUR_AGENCY + strOVC_PURCH_5),
                        new FieldContent("ONB_GROUP", rows["ONB_GROUP"].ToString()),
                        new FieldContent("ONB_MINIS_LOWEST", rows["ONB_MINIS_LOWEST"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_LOWEST"].ToString())),
                        new FieldContent("ONB_MINIS_1", rows["ONB_MINIS_1"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_1"].ToString())),
                        new FieldContent("ONB_MINIS_2", rows["ONB_MINIS_2"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_2"].ToString())),
                        new FieldContent("ONB_MINIS_3", rows["ONB_MINIS_3"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_3"].ToString())),
                        new FieldContent("ONB_MINIS_4", rows["ONB_MINIS_4"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_4"].ToString())),
                        new FieldContent("ONB_MINIS_5", rows["ONB_MINIS_5"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_5"].ToString())),
                        new FieldContent("ONB_MINIS_6", rows["ONB_MINIS_6"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_6"].ToString())),
                        new FieldContent("ONB_MINIS_7", rows["ONB_MINIS_7"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_7"].ToString())),
                        new FieldContent("ONB_MINIS_8", rows["ONB_MINIS_8"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_8"].ToString())),

                        new FieldContent("OVC_VEN_TITLE", rows["OVC_VEN_TITLE"] == null ? "" : rows["OVC_VEN_TITLE"].ToString()),
                        new FieldContent("nowDate", "中華民國" + GetTaiwanDate(DateTime.Today.ToString("yyyy-MM-dd")))
                    );
                }
                valuesToFill.Tables.Add(tableContent);
                



                /* -------------  List Content  -----------------
                   listContent.AddItem
                       (
                           new FieldContent("Title", "廠  商  投  標  減  價  單"),
                           new FieldContent("OVC_PURCH_A_5", strOVC_PURCH + strOVC_PUR_AGENCY + strOVC_PURCH_5)
                       );

                   tableContent.AddRow
                       (
                           new FieldContent("ONB_GROUP", numONB_GROUP.ToString()),
                           new FieldContent("ONB_MINIS_LOWEST", strONB_MINIS_LOWEST),
                           new FieldContent("ONB_MINIS_1", strONB_MINIS_1),
                           new FieldContent("ONB_MINIS_2", rows["ONB_MINIS_2"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_2"].ToString())),
                           new FieldContent("ONB_MINIS_3", rows["ONB_MINIS_3"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_3"].ToString())),
                           new FieldContent("ONB_MINIS_4", rows["ONB_MINIS_4"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_4"].ToString())),
                           new FieldContent("ONB_MINIS_5", rows["ONB_MINIS_5"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_5"].ToString())),
                           new FieldContent("ONB_MINIS_6", rows["ONB_MINIS_6"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_6"].ToString())),
                           new FieldContent("ONB_MINIS_7", rows["ONB_MINIS_7"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_7"].ToString())),
                           new FieldContent("ONB_MINIS_8", rows["ONB_MINIS_8"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_8"].ToString()))
                       );
                   listContent.AddItem(tableContent);
                   }
                   valuesToFill.Lists.Add(listContent);
               */

            }

            using (var outputDocument = new TemplateProcessor(targetPath + fileName).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }
            FileInfo fileInfo = new FileInfo(targetPath + fileName);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            System.IO.File.Delete(targetPath + fileName);
            Response.End();
        }

        

        private void GetWordD19_1_3()
        {
            //廠商減價單(折扣)
            var targetPath = Server.MapPath("~/WordPDFprint/");
            string fileName = strOVC_PURCH + "-廠商減價單(折扣).docx";
            File.Copy(targetPath + "D19_1_3-廠商減價單(折扣).docx", targetPath + fileName);
            var valuesToFill = new TemplateEngine.Docx.Content();

            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IPURCH", tbm1301.OVC_PUR_IPURCH == null ? "" : tbm1301.OVC_PUR_IPURCH));
            }
            
            valuesToFill.Fields.Add(new FieldContent("nowDate", "中華民國" + GetTaiwanDate(DateTime.Today.ToString("yyyy-MM-dd"))));

            using (var outputDocument = new TemplateProcessor(targetPath + fileName).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }
            FileInfo fileInfo = new FileInfo(targetPath + fileName);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            System.IO.File.Delete(targetPath + fileName);
            Response.End();
        }


        private void GetWordD19_1_4()
        {
            var targetPath = Server.MapPath("~/WordPDFprint/");
            string fileName = strOVC_PURCH + "-廠商減價單(總價).docx";
            File.Copy(targetPath + "D19_1_4-廠商減價單(總價).docx", targetPath + fileName);
            var valuesToFill = new TemplateEngine.Docx.Content();

            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IPURCH", tbm1301.OVC_PUR_IPURCH == null ? "" : tbm1301.OVC_PUR_IPURCH));
            }

            var queryData = mpms.TBM1314.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                        && tb.OVC_DOPEN.Equals(strOVC_DOPEN));
            valuesToFill.Fields.Add(new FieldContent("ONB_MINIS_LOWEST", GetTaiwanNumber(queryData.Select(tb => tb.ONB_MINIS_LOWEST).Sum().ToString())));
            valuesToFill.Fields.Add(new FieldContent("ONB_MINIS_1", GetTaiwanNumber(queryData.Select(tb => tb.ONB_MINIS_1).Sum().ToString())));
            valuesToFill.Fields.Add(new FieldContent("ONB_MINIS_2", GetTaiwanNumber(queryData.Select(tb => tb.ONB_MINIS_2).Sum().ToString())));
            valuesToFill.Fields.Add(new FieldContent("ONB_MINIS_3", GetTaiwanNumber(queryData.Select(tb => tb.ONB_MINIS_3).Sum().ToString())));
            valuesToFill.Fields.Add(new FieldContent("ONB_MINIS_4", GetTaiwanNumber(queryData.Select(tb => tb.ONB_MINIS_4).Sum().ToString())));
            valuesToFill.Fields.Add(new FieldContent("ONB_MINIS_5", GetTaiwanNumber(queryData.Select(tb => tb.ONB_MINIS_5).Sum().ToString())));
            valuesToFill.Fields.Add(new FieldContent("ONB_MINIS_6", GetTaiwanNumber(queryData.Select(tb => tb.ONB_MINIS_6).Sum().ToString())));
            valuesToFill.Fields.Add(new FieldContent("ONB_MINIS_7", GetTaiwanNumber(queryData.Select(tb => tb.ONB_MINIS_7).Sum().ToString())));
            valuesToFill.Fields.Add(new FieldContent("ONB_MINIS_9", GetTaiwanNumber(queryData.Select(tb => tb.ONB_MINIS_9).Sum().ToString())));
            valuesToFill.Fields.Add(new FieldContent("ONB_MINIS_10", GetTaiwanNumber(queryData.Select(tb => tb.ONB_MINIS_10).Sum().ToString())));
            valuesToFill.Fields.Add(new FieldContent("ONB_MINIS_11", GetTaiwanNumber(queryData.Select(tb => tb.ONB_MINIS_11).Sum().ToString())));
            valuesToFill.Fields.Add(new FieldContent("ONB_MINIS_12", GetTaiwanNumber(queryData.Select(tb => tb.ONB_MINIS_12).Sum().ToString())));

            valuesToFill.Fields.Add(new FieldContent("nowDate", "中華民國" + GetTaiwanDate(DateTime.Today.ToString("yyyy-MM-dd"))));


            using (var outputDocument = new TemplateProcessor(targetPath + fileName).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }
            FileInfo fileInfo = new FileInfo(targetPath + fileName);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            System.IO.File.Delete(targetPath + fileName);
            Response.End();
        }


        private void GetWordD19_1_5()
        {
            var targetPath = Server.MapPath("~/WordPDFprint/");
            string fileName = strOVC_PURCH + "-減價單(單價).docx";
            File.Copy(targetPath + "D19_1_5-減價單(單價).docx", targetPath + fileName);

            string strOVC_PUR_IPURCH = "";
            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
                strOVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH;


            var queryTBM1314 =
                from tbm1314 in mpms.TBM1314
                where tbm1314.OVC_PURCH.Equals(strOVC_PURCH) && tbm1314.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                        && tbm1314.OVC_DOPEN.Equals(strOVC_DOPEN)
                orderby tbm1314.ONB_GROUP
                select tbm1314;
            DataTable dt = CommonStatic.LinqQueryToDataTable(queryTBM1314);

            var valuesToFill = new TemplateEngine.Docx.Content();
            var tableContent = new TableContent("table");
           
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow rows in dt.Rows)
                {
                    /* -------------Table Content----------------- */
                    tableContent.AddRow
                    (
                        new FieldContent("OVC_PUR_IPURCH", strOVC_PUR_IPURCH),
                        new FieldContent("OVC_PURCH_A_5", strOVC_PURCH + strOVC_PUR_AGENCY + strOVC_PURCH_5),
                        new FieldContent("ONB_GROUP", rows["ONB_GROUP"].ToString()),
                        new FieldContent("ONB_MINIS_LOWEST", rows["ONB_MINIS_LOWEST"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_LOWEST"].ToString())), 
                        new FieldContent("ONB_MINIS_1", rows["ONB_MINIS_1"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_1"].ToString())), 
                        new FieldContent("ONB_MINIS_2", rows["ONB_MINIS_2"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_2"].ToString())),
                        new FieldContent("ONB_MINIS_3", rows["ONB_MINIS_3"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_3"].ToString())),
                        new FieldContent("ONB_MINIS_4", rows["ONB_MINIS_4"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_4"].ToString())),
                        new FieldContent("ONB_MINIS_5", rows["ONB_MINIS_5"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_5"].ToString())),
                        new FieldContent("ONB_MINIS_6", rows["ONB_MINIS_6"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_6"].ToString())),
                        new FieldContent("ONB_MINIS_7", rows["ONB_MINIS_7"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_7"].ToString())),
                        new FieldContent("ONB_MINIS_8", rows["ONB_MINIS_8"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_8"].ToString())),
                        new FieldContent("OVC_VEN_TITLE", rows["OVC_VEN_TITLE"] == null ? "" : rows["OVC_VEN_TITLE"].ToString()),
                        new FieldContent("nowDate", "中華民國" + GetTaiwanDate(DateTime.Today.ToString("yyyy-MM-dd")))
                    );
                }
                valuesToFill.Tables.Add(tableContent);
            }

            using (var outputDocument = new TemplateProcessor(targetPath + fileName).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }
            FileInfo fileInfo = new FileInfo(targetPath + fileName);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            System.IO.File.Delete(targetPath + fileName);
            Response.End();
        }

        private void GetWordD19_1_6()
        {
            var targetPath = Server.MapPath("~/WordPDFprint/");
            string fileName = strOVC_PURCH + "-開標結果通知押標金投標文件退還紀錄表.docx";
            File.Copy(targetPath + "D19_1_6-開標結果通知押標金投標文件退還紀錄表.docx", targetPath + fileName);
            var valuesToFill = new TemplateEngine.Docx.Content();

            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5));
            }

            TBMBID_BACK tbmBID_BACK = mpms.TBMBID_BACK.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP).FirstOrDefault();
            if (tbmBID_BACK != null)
            {
                valuesToFill.Fields.Add(new FieldContent("OVC_DBACK", tbmBID_BACK.OVC_DBACK == null? "" : GetTaiwanDate(tbmBID_BACK.OVC_DBACK)));
                valuesToFill.Fields.Add(new FieldContent("OVC_BACK_PLACE", tbmBID_BACK.OVC_BACK_PLACE == null ? "" : tbmBID_BACK.OVC_BACK_PLACE));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_1", tbmBID_BACK.OVC_REASON_1 == null ? "□" : "■"));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_2", tbmBID_BACK.OVC_REASON_2 == null ? "□" : "■"));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_5", tbmBID_BACK.OVC_REASON_5 == null ? "□" : "■"));
                valuesToFill.Fields.Add(new FieldContent("chkOVC_MEMO", tbmBID_BACK.OVC_MEMO == null ? "□" : "■"));
                valuesToFill.Fields.Add(new FieldContent("OVC_MEMO", tbmBID_BACK.OVC_MEMO == null ? "" : tbmBID_BACK.OVC_MEMO));
                valuesToFill.Fields.Add(new FieldContent("OVC_REMARK", tbmBID_BACK.OVC_REMARK == null ? "" : tbmBID_BACK.OVC_REMARK));
            }

            using (var outputDocument = new TemplateProcessor(targetPath + fileName).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }
            FileInfo fileInfo = new FileInfo(targetPath + fileName);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            System.IO.File.Delete(targetPath + fileName);
            Response.End();
        }

        private string GetTaiwanNumber(string strNumber)
        {
            if (strNumber != "")
            {
                double num = double.Parse(strNumber);
                return EastAsiaNumericFormatter.FormatWithCulture("L", num, null, new CultureInfo("zh-tw")) + "元正。";
            }
                
            return "";
        }


        private string GetGroupVendors()
        {
            string strGroupVendors = "";
            bool isOnlyGroup0 = false;

            var queryTBM1313 =
                from tbm1313 in mpms.TBM1313
                where tbm1313.OVC_PURCH.Equals(strOVC_PURCH) && tbm1313.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                   && tbm1313.OVC_DOPEN.Equals(strOVC_DOPEN)
                select tbm1313;

            var queryTBM1303 =
                from tbm1303 in mpms.TBM1303
                where tbm1303.OVC_PURCH.Equals(strOVC_PURCH) && tbm1303.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                      && tbm1303.ONB_TIMES == numONB_TIMES && tbm1303.OVC_DOPEN.Equals(strOVC_DOPEN)
                join tbm1313 in queryTBM1313 on tbm1303.ONB_GROUP equals tbm1313.ONB_GROUP into tbGroup1
                select new
                {
                    ONB_GROUP = tbm1303.ONB_GROUP,
                    ToatalVendors = tbGroup1.Count(),
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(queryTBM1303);

            if (dt.Rows.Count != 0)
            {
                int rowsCount = dt.Rows.Count;
                foreach (DataRow rows in dt.Rows)
                {
                    if(rows["ONB_GROUP"].ToString() == "0" && rowsCount == 1)
                        isOnlyGroup0 = true;
                    strGroupVendors += strGroupVendors == "" ? "" : "、";
                    strGroupVendors += "第" + rows["ONB_GROUP"].ToString() + "組計有" + rows["ToatalVendors"].ToString() + "家廠商投標";
                }
            }
            if (!isOnlyGroup0 && strGroupVendors != "")
                return "(" + strGroupVendors + ")，";
            else
                return "";
        }


        private string GetOVC_RESULT_Y()
        {
            //取得投標廠商 審查結果="Y"的廠商數量
            string strTotalResult;
            var queryTBM1313 =
                (from tbm1313 in mpms.TBM1313
                where tbm1313.OVC_PURCH.Equals(strOVC_PURCH) && tbm1313.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                   && tbm1313.OVC_DOPEN.Equals(strOVC_DOPEN) && tbm1313.ONB_GROUP == numONB_GROUP && tbm1313.OVC_RESULT =="Y"
                select tbm1313).ToList();

            strTotalResult = queryTBM1313.Count() == 0 ? "" : queryTBM1313.Count().ToString();
            return strTotalResult;
        }


        private string GetTBM1314Sum()
        {
            var queryData = mpms.TBM1314.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                        && tb.OVC_DOPEN.Equals(strOVC_DOPEN)).Select(tb => tb.ONB_MINIS_1).Sum();
            return queryData.ToString();
        }




        /*   ---- 分組總價 -----
            var queryGroupID =
                (from tbm1314Group in mpms.TBM1314
                 where tbm1314Group.OVC_PURCH.Equals(strOVC_PURCH) && tbm1314Group.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                         && tbm1314Group.OVC_DOPEN.Equals(strOVC_DOPEN)
                 orderby tbm1314Group.ONB_GROUP
                 select new
                 {
                     OVC_PURCH = tbm1314Group.OVC_PURCH,
                     ONB_GROUP = tbm1314Group.ONB_GROUP,
                 }).Distinct();

            var queryData =
                from tbm1314Data in mpms.TBM1314
                 where tbm1314Data.OVC_PURCH.Equals(strOVC_PURCH) && tbm1314Data.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                         && tbm1314Data.OVC_DOPEN.Equals(strOVC_DOPEN)
                 orderby tbm1314Data.ONB_GROUP
                 select tbm1314Data;

            var queryTBM1314 =
                (from groupID in queryGroupID.AsEnumerable()
                join tbm1314 in queryData on groupID.ONB_GROUP equals tbm1314.ONB_GROUP into tbGroup
                select new
                {
                    OVC_PURCH = groupID.OVC_PURCH,
                    ONB_GROUP = groupID.ONB_GROUP,
                    ONB_MINIS_LOWEST = tbGroup.Sum((tb => tb.ONB_MINIS_LOWEST)),
                    ONB_MINIS_1 = tbGroup.Sum((tb => tb.ONB_MINIS_1)),
                    ONB_MINIS_2 = tbGroup.Sum((tb => tb.ONB_MINIS_2)),
                    ONB_MINIS_3 = tbGroup.Sum((tb => tb.ONB_MINIS_3)),
                    ONB_MINIS_4 = tbGroup.Sum((tb => tb.ONB_MINIS_4)),
                    ONB_MINIS_5 = tbGroup.Sum((tb => tb.ONB_MINIS_5)),
                    ONB_MINIS_6 = tbGroup.Sum((tb => tb.ONB_MINIS_6)),
                    ONB_MINIS_7 = tbGroup.Sum((tb => tb.ONB_MINIS_7)),
                    ONB_MINIS_8 = tbGroup.Sum((tb => tb.ONB_MINIS_8)),
                    OVC_VEN_TITLE = string.Join(",", tbGroup.Select(tb => tb.OVC_VEN_TITLE)),
                }).ToArray();

        */



        /*  ------- 廠商單價 ------
        var queryTBM1314 =
                from tbm1314 in mpms.TBM1314
                where tbm1314.OVC_PURCH.Equals(strOVC_PURCH) && tbm1314.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                        && tbm1314.OVC_DOPEN.Equals(strOVC_DOPEN) && tbm1314.ONB_GROUP == numONB_GROUP
                orderby tbm1314.ONB_GROUP
                select tbm1314;

            DataTable dt = CommonStatic.LinqQueryToDataTable(queryTBM1314);

            
            var valuesToFill = new TemplateEngine.Docx.Content();
            var listContent = new ListContent("list");
            var tableContent = new TableContent("table");
            
            
            string strONB_MINIS_LOWEST, strONB_MINIS_1;
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow rows in dt.Rows)
                {
                    
                    strONB_MINIS_LOWEST = rows["ONB_MINIS_LOWEST"] == null ? "" : EastAsiaNumericFormatter.FormatWithCulture("L", int.Parse(rows["ONB_MINIS_LOWEST"].ToString()), null, new CultureInfo("zh-tw"));
                    strONB_MINIS_1 = rows["ONB_MINIS_1"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_1"].ToString());

                    tableContent.AddRow
                    (
                        new FieldContent("Title0", " "),
                        new FieldContent("Title", "廠  商  投  標  減  價  單"),
                        new FieldContent("OVC_PURCH_A_5", strOVC_PURCH + strOVC_PUR_AGENCY + strOVC_PURCH_5),
                        new FieldContent("ONB_GROUP", rows["ONB_GROUP"].ToString()),
                        new FieldContent("ONB_MINIS_LOWEST", strONB_MINIS_LOWEST),
                        new FieldContent("ONB_MINIS_1", strONB_MINIS_1),
                        new FieldContent("ONB_MINIS_2", rows["ONB_MINIS_2"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_2"].ToString())),
                        new FieldContent("ONB_MINIS_3", rows["ONB_MINIS_3"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_3"].ToString())),
                        new FieldContent("ONB_MINIS_4", rows["ONB_MINIS_4"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_4"].ToString())),
                        new FieldContent("ONB_MINIS_5", rows["ONB_MINIS_5"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_5"].ToString())),
                        new FieldContent("ONB_MINIS_6", rows["ONB_MINIS_6"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_6"].ToString())),
                        new FieldContent("ONB_MINIS_7", rows["ONB_MINIS_7"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_7"].ToString())),
                        new FieldContent("ONB_MINIS_8", rows["ONB_MINIS_8"] == null ? "" : GetTaiwanNumber(rows["ONB_MINIS_8"].ToString())),

                        new FieldContent("r1", "一、本報價單之報價有效期至本日起30日內有效；如屬招標文件規定先行以保留決標辦理者，廠商報"),
                        new FieldContent("r1_1", "價有效期至決標簽約日止。"),
                        new FieldContent("r2", "二、投標標的原產地(敘明國家或地區)："),
                        new FieldContent("r3", "此    致"),
                        new FieldContent("r4", "國防部"),
                        new FieldContent("r5_1", "公司章戳"),
                        new FieldContent("r5_2", "負責人章戳"),
                        new FieldContent("r6_1", " "),
                        new FieldContent("r6_2", " "),
                        new FieldContent("OVC_VEN_TITLE", "廠商名稱：" +  rows["OVC_VEN_TITLE"] == null ? "" : rows["OVC_VEN_TITLE"].ToString()),
                        new FieldContent("r7", "中華民國" + GetTaiwanDate(DateTime.Today.ToString("yyyy-MM-dd"))),
                        new FieldContent("r8", " ")
                    );
                }
            valuesToFill.Tables.Add(tableContent);
        */












        #endregion


    }
}