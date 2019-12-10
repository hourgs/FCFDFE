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

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D18_3 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strOVC_PURCH, strOVC_PUR_AGENCY, strOVC_PURCH_5, strOVC_DOPEN;
        short numONB_TIMES, numONB_GROUP;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            {
                //設置readonly屬性
                FCommon.Controls_Attributes("readonly", "true", txtOVC_DBID_Site, txtOVC_DBID_Comm, txtOVC_DBID_Elec, txtOVC_DBID_Site);

                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_DOPEN"] == null || Request.QueryString["ONB_TIMES"] == null || Request.QueryString["ONB_GROUP"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    strOVC_PURCH = Request.QueryString["OVC_PURCH"].ToString();
                    TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                    strOVC_DOPEN = Request.QueryString["OVC_DOPEN"].ToString();
                    short.TryParse(Request.QueryString["ONB_TIMES"].ToString(), out numONB_TIMES);
                    short.TryParse(Request.QueryString["ONB_GROUP"].ToString(), out numONB_GROUP);
                    if (IsOVC_DO_NAME() && !IsPostBack)
                        DataImport();
                }


            }
        }

        protected void gvTBM1313_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView gv = (GridView)sender;
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string strOVC_KIND = "", strOVC_KIND_Name = "";
            string strOVC_VEN_CST = ((Label)gv.Rows[gvrIndex].FindControl("lblOVC_VEN_CST")).Text;
            string strOVC_VEN_TITLE = ((Label)gv.Rows[gvrIndex].FindControl("lblOVC_VEN_TITLE")).Text;
            ViewState["OVC_VEN_TITLE"] = strOVC_VEN_TITLE;
            if (e.CommandName == "Delete_Site" || e.CommandName == "Modify_Site" || e.CommandName == "Edit_Site")
            {
                strOVC_KIND = "1";
                strOVC_KIND_Name = "Site";
            }
            else if (e.CommandName == "Delete_Comm" || e.CommandName == "Modify_Comm" || e.CommandName == "Edit_Comm")
            {
                strOVC_KIND = "2";
                strOVC_KIND_Name = "Comm";
            }


            else if (e.CommandName == "Delete_Elec" || e.CommandName == "Modify_Elec" || e.CommandName == "Edit_Elec")
            {
                strOVC_KIND = "3";
                strOVC_KIND_Name = "Elec";
            }



            //刪除
            if (e.CommandName == "Delete_Site" || e.CommandName == "Delete_Comm" || e.CommandName == "Delete_Elec")
            {
                TBM1313 tbm1313_Del = new TBM1313();
                tbm1313_Del = mpms.TBM1313.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                                    && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.OVC_KIND.Equals(strOVC_KIND)
                                                    && tb.ONB_GROUP == numONB_GROUP && tb.OVC_VEN_TITLE.Equals(strOVC_VEN_TITLE)).FirstOrDefault();
                mpms.Entry(tbm1313_Del).State = EntityState.Deleted;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1313_Del.GetType().Name.ToString(), this, "刪除");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功！");
                SetGvTBM1313(strOVC_KIND, strOVC_KIND_Name);
            }

            //異動
            if (e.CommandName == "Modify_Site" || e.CommandName == "Modify_Comm" || e.CommandName == "Modify_Elec")
            {
                GetOVC_KIND_Action("Modify", e.CommandName, strOVC_VEN_CST, strOVC_VEN_TITLE);
            }

            //審查編輯
            if (e.CommandName == "Edit_Site" || e.CommandName == "Edit_Comm" || e.CommandName == "Edit_Elec")
            {
                strOVC_VEN_CST = ((Label)gv.Rows[gvrIndex].FindControl("lblOVC_VEN_CST")).Text;
                string send_url = "~/pages/MPMS/D/MPMS_D18_4.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_DOPEN=" + strOVC_DOPEN + "&ONB_TIMES=" + numONB_TIMES + "&ONB_GROUP=" + numONB_GROUP
                                    + "&OVC_KIND=" + strOVC_KIND + "&OVC_VEN_TITLE=" + strOVC_VEN_TITLE;
                Response.Redirect(send_url);
            }

            //審查表輸出
            if (e.CommandName == "WordD18_3_1" || e.CommandName == "WordD18_3_1_odt" || e.CommandName == "WordD18_3_2" || e.CommandName == "WordD18_3_2_odt" || e.CommandName == "WordD18_3_3" || e.CommandName == "WordD18_3_3_odt" || e.CommandName == "WordD18_3_4" || e.CommandName == "WordD18_3_4_odt")
            {
                string strUrl = "MPMS_D18_3_Word.ashx?CommandName=" + e.CommandName + "&OVC_PURCH=" + strOVC_PURCH + "&OVC_DOPEN=" + strOVC_DOPEN + "&OVC_VEN_TITLE=" + strOVC_VEN_TITLE;
                this.Response.Redirect(strUrl);
            }
        }

        protected void ddlOVC_VEN_CST_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GetOVC_KIND_Action("DdlImport", ddl.ID, ddl.SelectedValue, "");
        }




        #region Button Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            //點擊 清除
            Button btn = (Button)sender;
            GetOVC_KIND_Action("Clear", btn.ID, "", "");
        }

        protected void btnSaveVendor_Click(object sender, EventArgs e)
        {
            //點擊 投標廠商編輯裡的存檔 TBMBID_END
            Button btn = (Button)sender;
            GetOVC_KIND_Action("Save", btn.ID, "", "");
        }

        protected void btnSaveAndInsert_Click(object sender, EventArgs e)
        {
            //點擊 投標廠商編輯裡的存檔並寫入廠商檔 TBMBID_END + TBM1313
            Button btn = (Button)sender;
            GetOVC_KIND_Action("SaveAndInsert", btn.ID, "", "");
        }

        protected void btnOVC_VEN_CST_Click(object sender, EventArgs e)
        {
            //點擊 查詢廠商
            Button btn = (Button)sender;
            GetOVC_KIND_Action("Search", btn.ID, "", "");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //點擊 存檔 TBMBID_END
            SaveData("TBMBID_END", "Site", "1");
        }


        #endregion



        #region Button Click 副程式

        private void GetOVC_KIND_Action(string actionKind, string actionID, string strOVC_VEN_CST, string strOVC_VEN_TITLE)
        {
            string strOVC_KIND_Name = "";
            string strOVC_KIND = "";
            if (actionID == "btnSave_Site" || actionID == "btnClear_Site" || actionID == "btnSaveAndInsert_Site"
                || actionID == "btnSearch_Site" || actionID == "Modify_Site" || actionID == "ddlOVC_VEN_CST_Site")
            {
                strOVC_KIND_Name = "Site";
                strOVC_KIND = "1";
            }

            else if (actionID == "btnSave_Comm" || actionID == "btnClear_Comm" || actionID == "btnSaveAndInsert_Comm"
                || actionID == "btnSearch_Comm" || actionID == "Modify_Comm" || actionID == "ddlOVC_VEN_CST_Comm")
            {
                strOVC_KIND_Name = "Comm";
                strOVC_KIND = "2";
            }

            else if (actionID == "btnSave_Elec" || actionID == "btnClear_Elec" || actionID == "btnSaveAndInsert_Elec"
                || actionID == "btnSearch_Elec" || actionID == "Modify_Elec" || actionID == "ddlOVC_VEN_CST_Elec")
            {
                strOVC_KIND_Name = "Elec";
                strOVC_KIND = "3";
            }


            if (strOVC_KIND_Name != "")
            {
                DropDownList ddlOVC_VEN_CST = (DropDownList)this.Master.FindControl("MainContent").FindControl("ddlOVC_VEN_CST_" + strOVC_KIND_Name);
                TextBox txtOVC_VEN_CST = (TextBox)this.Master.FindControl("MainContent").FindControl("txtOVC_VEN_CST_" + strOVC_KIND_Name);
                TextBox txtOVC_VEN_TITLE = (TextBox)this.Master.FindControl("MainContent").FindControl("txtOVC_VEN_TITLE_" + strOVC_KIND_Name);
                TextBox txtOVC_VEN_TEL = (TextBox)this.Master.FindControl("MainContent").FindControl("txtOVC_VEN_TEL_" + strOVC_KIND_Name);
                TextBox txtOVC_DBID = (TextBox)this.Master.FindControl("MainContent").FindControl("txtOVC_DBID_" + strOVC_KIND_Name);
                TextBox txtOVC_BID_HOUR = (TextBox)this.Master.FindControl("MainContent").FindControl("txtOVC_BID_HOUR_" + strOVC_KIND_Name);
                TextBox txtOVC_BID_MIN = (TextBox)this.Master.FindControl("MainContent").FindControl("txtOVC_BID_MIN_" + strOVC_KIND_Name);

                //投標廠商編輯裡的 存檔
                if (actionKind == "Save")
                    SaveData("TBM1313", strOVC_KIND_Name, strOVC_KIND);


                //投標廠商編輯裡的 存檔並寫入廠商檔 TBMBID_END + TBM1313 
                else if (actionKind == "SaveAndInsert")
                {
                    SaveData("TBM1313_And_TBM1203", strOVC_KIND_Name, strOVC_KIND);
                }


                //清除
                else if (actionKind == "Clear")
                {
                    ddlOVC_VEN_CST.SelectedValue = null;
                    txtOVC_VEN_CST.Text = "";
                    txtOVC_VEN_TITLE.Text = "";
                    txtOVC_VEN_TEL.Text = "";
                    txtOVC_DBID.Text = "";
                    txtOVC_BID_HOUR.Text = "";
                    txtOVC_BID_MIN.Text = "";
                }


                //廠商查詢
                else if (actionKind == "Search" && txtOVC_VEN_CST.Text != "")
                {
                    /*
                    TBM1313 tbm1313 = new TBM1313();
                    tbm1313 = mpms.TBM1313.Where(tb => tb.OVC_VEN_CST.Equals(txtOVC_VEN_CST.Text)
                                && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)).FirstOrDefault();
                    if (tbm1313 != null)
                    {
                        txtOVC_VEN_TITLE.Text = tbm1313.OVC_VEN_TITLE;
                        txtOVC_VEN_TEL.Text = tbm1313.OVC_VEN_TEL;
                        txtOVC_DBID.Text = tbm1313.OVC_DBID;
                        txtOVC_BID_HOUR.Text = tbm1313.OVC_BID_HOUR;
                        txtOVC_BID_MIN.Text = tbm1313.OVC_BID_MIN;
                    }
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('統一編號查無廠商資訊');", true);
                    */
                    TBMBID_VENDOR tbmBID_VENDOR = new TBMBID_VENDOR();
                    tbmBID_VENDOR = mpms.TBMBID_VENDOR.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                            && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP
                            && tb.OVC_VEN_CST.Equals(strOVC_VEN_CST)).FirstOrDefault();
                    if (tbmBID_VENDOR != null)
                    {
                        txtOVC_VEN_CST.Text = tbmBID_VENDOR.OVC_VEN_CST;
                        txtOVC_VEN_TITLE.Text = tbmBID_VENDOR.OVC_VEN_TITLE;
                        txtOVC_VEN_TEL.Text = tbmBID_VENDOR.OVC_VEN_TEL;
                        txtOVC_DBID.Text = "";
                        txtOVC_BID_HOUR.Text = "";
                        txtOVC_BID_MIN.Text = "";
                    }
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('統一編號查無廠商資訊');", true);
                }


                //異動
                else if (actionKind == "Modify")
                {
                    TBM1313 tbm1313 = new TBM1313();
                    tbm1313 = mpms.TBM1313.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                                        && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.OVC_KIND.Equals(strOVC_KIND)
                                                        && tb.ONB_GROUP == numONB_GROUP && tb.OVC_VEN_CST.Equals(strOVC_VEN_CST)
                                                        && tb.OVC_VEN_TITLE.Equals(strOVC_VEN_TITLE)).FirstOrDefault();
                    txtOVC_VEN_CST.Text = tbm1313.OVC_VEN_CST;
                    txtOVC_VEN_TITLE.Text = tbm1313.OVC_VEN_TITLE;
                    txtOVC_VEN_TEL.Text = tbm1313.OVC_VEN_TEL;
                    txtOVC_DBID.Text = tbm1313.OVC_DBID;
                    txtOVC_BID_HOUR.Text = tbm1313.OVC_BID_HOUR;
                    txtOVC_BID_MIN.Text = tbm1313.OVC_BID_MIN;
                }

                //帶入DDL選擇的廠商資料
                else if (actionKind == "DdlImport")
                {
                    /*
                    var queryTBM1313 =
                        (from tb1313 in mpms.TBM1313
                            where tb1313.OVC_PURCH.Equals(strOVC_PURCH) && tb1313.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tb1313.OVC_DOPEN.Equals(strOVC_DOPEN) && tb1313.ONB_GROUP == numONB_GROUP && tb1313.OVC_KIND.Equals(strOVC_KIND)
                            && tb1313.OVC_VEN_CST.Equals(strOVC_VEN_CST)
                            select tb1313).FirstOrDefault();
                    if (queryTBM1313 != null)
                    {
                        txtOVC_VEN_CST.Text = queryTBM1313.OVC_VEN_CST;
                        txtOVC_VEN_TITLE.Text = queryTBM1313.OVC_VEN_TITLE;
                        txtOVC_VEN_TEL.Text = queryTBM1313.OVC_VEN_TEL;
                        txtOVC_DBID.Text = queryTBM1313.OVC_DBID;
                        txtOVC_BID_HOUR.Text = queryTBM1313.OVC_BID_HOUR;
                        txtOVC_BID_MIN.Text = queryTBM1313.OVC_BID_MIN;
                    }
                    */

                    TBMBID_VENDOR tbmBID_VENDOR = new TBMBID_VENDOR();
                    tbmBID_VENDOR = mpms.TBMBID_VENDOR.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                            && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP
                            && tb.OVC_VEN_CST.Equals(strOVC_VEN_CST)).FirstOrDefault();
                    txtOVC_VEN_CST.Text = tbmBID_VENDOR.OVC_VEN_CST;
                    txtOVC_VEN_TITLE.Text = tbmBID_VENDOR.OVC_VEN_TITLE;
                    txtOVC_VEN_TEL.Text = tbmBID_VENDOR.OVC_VEN_TEL;
                    txtOVC_DBID.Text = "";
                    txtOVC_BID_HOUR.Text = "";
                    txtOVC_BID_MIN.Text = "";
                }
            }
        }



        private void SaveData(string saveType, string strOVC_KIND_Name, string strOVC_KIND)
        {
            string strMsg = "", strErroMsg = "";
            string strOVC_VEN_CST = ((TextBox)this.Master.FindControl("MainContent").FindControl("txtOVC_VEN_CST_" + strOVC_KIND_Name)).Text;
            string strOVC_VEN_TITLE = ((TextBox)this.Master.FindControl("MainContent").FindControl("txtOVC_VEN_TITLE_" + strOVC_KIND_Name)).Text;
            string strOVC_VEN_TEL = ((TextBox)this.Master.FindControl("MainContent").FindControl("txtOVC_VEN_TEL_" + strOVC_KIND_Name)).Text;
            string strOVC_DBID = ((TextBox)this.Master.FindControl("MainContent").FindControl("txtOVC_DBID_" + strOVC_KIND_Name)).Text;
            string strOVC_BID_HOUR = ((TextBox)this.Master.FindControl("MainContent").FindControl("txtOVC_BID_HOUR_" + strOVC_KIND_Name)).Text;
            string strOVC_BID_MIN = ((TextBox)this.Master.FindControl("MainContent").FindControl("txtOVC_BID_MIN_" + strOVC_KIND_Name)).Text;

            if (saveType == "TBM1313_And_TBM1203")
            {
                if (strOVC_VEN_CST == "")
                    strErroMsg = "請輸入廠商統一編號";
                else
                {
                    TBM1203 tbm1203 = mpms.TBM1203.Where(tb => tb.OVC_VEN_CST.Equals(strOVC_VEN_CST)).FirstOrDefault();
                    if (tbm1203 == null)
                    {
                        //新增 TBM1203 廠商資料
                        TBM1203 tbm1203_New = new TBM1203
                        {
                            OVC_VEN_SN = Guid.NewGuid(),
                            OVC_VEN_CST = strOVC_VEN_CST,
                            OVC_VEN_TITLE = strOVC_VEN_TITLE,
                            OVC_VEN_ITEL = strOVC_VEN_TEL,
                        };
                        mpms.TBM1203.Add(tbm1203_New);
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1203_New.GetType().Name.ToString(), this, "新增");
                    }
                    else
                    {
                        //修改 TBM1203 廠商資料
                        tbm1203.OVC_VEN_TITLE = strOVC_VEN_TITLE;
                        tbm1203.OVC_VEN_ITEL = strOVC_VEN_TEL;
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1203.GetType().Name.ToString(), this, "修改");
                    }
                    strMsg += strMsg == "" ? "廠商資料 存檔成功！" : "<BR>廠商資料 存檔成功！";
                }
            }

            if (saveType == "TBM1313" || saveType == "TBM1313_And_TBM1203")
            {
                if (strOVC_VEN_TITLE == "")
                    strErroMsg += strErroMsg == "" ? "請輸入廠商名稱" : "<BR>請輸入廠商名稱";
                else
                {
                    TBM1313 tbm1313 = new TBM1313();
                    tbm1313 = mpms.TBM1313.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                                        && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.OVC_KIND.Equals(strOVC_KIND)
                                                        && tb.ONB_GROUP == numONB_GROUP && tb.OVC_VEN_TITLE.Equals(strOVC_VEN_TITLE)).FirstOrDefault();
                    if (tbm1313 == null)
                    {
                        //新增 TBM1313
                        TBM1313 tbm1313_New = new TBM1313
                        {
                            OVC_SN = Guid.NewGuid(),
                            OVC_PURCH = strOVC_PURCH,
                            OVC_PURCH_5 = strOVC_PURCH_5,
                            OVC_DOPEN = strOVC_DOPEN,
                            OVC_KIND = strOVC_KIND,
                            ONB_GROUP = numONB_GROUP,
                            OVC_VEN_CST = strOVC_VEN_CST,
                            OVC_VEN_TITLE = strOVC_VEN_TITLE,
                            OVC_VEN_TEL = strOVC_VEN_TEL,
                            OVC_DBID = strOVC_DBID,
                            OVC_BID_HOUR = strOVC_BID_HOUR,
                            OVC_BID_MIN = strOVC_BID_MIN,
                        };
                        mpms.TBM1313.Add(tbm1313_New);
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1313_New.GetType().Name.ToString(), this, "新增");
                    }
                    else
                    {
                        //修改 TBM1313
                        tbm1313.OVC_VEN_CST = strOVC_VEN_CST;
                        tbm1313.OVC_VEN_TEL = strOVC_VEN_TEL;
                        tbm1313.OVC_DBID = strOVC_DBID;
                        tbm1313.OVC_BID_HOUR = strOVC_BID_HOUR;
                        tbm1313.OVC_BID_MIN = strOVC_BID_MIN;
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1313.GetType().Name.ToString(), this, "修改");
                    }
                    GetOVC_KIND_Action("Clear", "btnClear_" + strOVC_KIND_Name, "", "");
                    strMsg += strMsg == "" ? "投標廠商主檔 存檔成功！" : "<BR>投標廠商主檔 存檔成功！";
                    DataImport();
                }
            }

            if (saveType == "TBMBID_END")
            {
                TBMBID_END tbmBID_END = new TBMBID_END();
                tbmBID_END = mpms.TBMBID_END.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                                    && tb.OVC_DOPEN.Equals(strOVC_DOPEN)).FirstOrDefault();
                if (tbmBID_END == null)
                {
                    //新增 TBMBID_END
                    string strOVC_DO_NAME = "";
                    TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    if (tbmRECEIVE_BID != null)
                        strOVC_DO_NAME = tbmRECEIVE_BID.OVC_DO_NAME;

                    TBMBID_END tbmBID_END_New = new TBMBID_END
                    {
                        OVC_SN = Guid.NewGuid(),
                        OVC_PURCH = strOVC_PURCH,
                        OVC_PURCH_5 = strOVC_PURCH_5,
                        OVC_DOPEN = strOVC_DOPEN,
                        OVC_NAME = strOVC_DO_NAME,
                        OVC_DAPPROVE = txtOVC_DAPPROVE.Text,
                        ONB_GROUP = numONB_GROUP,
                    };
                    mpms.TBMBID_END.Add(tbmBID_END_New);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmBID_END_New.GetType().Name.ToString(), this, "新增");
                }
                else
                {
                    //修改 TBMBID_END
                    tbmBID_END.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmBID_END.GetType().Name.ToString(), this, "修改");
                }
                strMsg += strMsg == "" ? "截標審查紀錄主檔 存檔成功！" : "<BR>截標審查紀錄主檔 存檔成功！";
            }



            if (strErroMsg == "")
            {
                //修改 OVC_STATUS="24" 截標審查的 階段結束日
                string strOVC_DO_NAME = "";
                TBMSTATUS tbmSTATUS_24 = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                            && tb.ONB_TIMES == 1 && tb.OVC_STATUS.Equals("24")).FirstOrDefault();
                if (tbmSTATUS_24 != null)
                {
                    tbmSTATUS_24.OVC_DEND = DateTime.Now.ToString("yyyy-MM-dd");
                    strOVC_DO_NAME = tbmSTATUS_24.OVC_DO_NAME;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmSTATUS_24.GetType().Name.ToString(), this, "修改");
                }
                FCommon.AlertShow(PnMessage, "success", "系統訊息", strMsg);
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErroMsg);
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            //點擊 回開標紀錄作業編輯
            string send_url = "~/pages/MPMS/D/MPMS_D18_1.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_DOPEN=" + strOVC_DOPEN +
                              "&ONB_TIMES=" + numONB_TIMES + "&ONB_GROUP=" + numONB_GROUP;
            Response.Redirect(send_url);
        }

        protected void btnReturnR_Click(object sender, EventArgs e)
        {
            //點擊 回開標紀錄選擇畫面
            string send_url = "~/pages/MPMS/D/MPMS_D18.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_PUR_AGENCY=" + strOVC_PUR_AGENCY
                            + "&OVC_PURCH_5=" + strOVC_PURCH_5;
            Response.Redirect(send_url);
        }

        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            //點擊 回主流程畫面
            string send_url = "~/pages/MPMS/D/MPMS_D14_1.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_PURCH_5=" + strOVC_PURCH_5;
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
            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                lblOVC_PURCH_A_5.Text = tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                strOVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY;
                lblOVC_PURCH.Text = strOVC_PURCH;
                lblOVC_PUR_AGENCY.Text = strOVC_PUR_AGENCY;
                lblOVC_PUR_IPURCH.Text = tbm1301.OVC_PUR_IPURCH;
                lblOVC_BID_TIMES.Text = GetTbm1407Desc("TG", tbm1301.OVC_BID_TIMES);
                lblOVC_PUR_ASS_VEN_CODE.Text = GetTbm1407Desc("C7", tbm1301.OVC_PUR_ASS_VEN_CODE);



                TBMRECEIVE_BID tbmRECEIVE_BID = new TBMRECEIVE_BID();
                tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                strOVC_PURCH_5 = tbmRECEIVE_BID?.OVC_PURCH_5;
                lblOVC_PURCH_5.Text = strOVC_PURCH_5;



                TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
                tbmRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                        && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_ANNOUNCE != null)
                {
                    lblOVC_BID_METHOD_1.Text = tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_1;
                    lblOVC_BID_METHOD_2.Text = tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_2;
                    lblOVC_BID_METHOD_3.Text = tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_3;
                    lblOVC_DANNOUNCE.Text = GetTaiwanDate(tbmRECEIVE_ANNOUNCE.OVC_DANNOUNCE);
                    lblOVC_DBID_LIMIT_H_M.Text = GetTaiwanDate(tbmRECEIVE_ANNOUNCE.OVC_DBID_LIMIT) + " " + tbmRECEIVE_ANNOUNCE.OVC_LIMIT_HOUR + "時" + tbmRECEIVE_ANNOUNCE.OVC_LIMIT_MIN + "分";
                    txtOVC_DAPPROVE.Text = tbmRECEIVE_ANNOUNCE.OVC_DAPPROVE;
                }



                TBMRECEIVE_WORK tbmRECEIVE_WORK = new TBMRECEIVE_WORK();
                tbmRECEIVE_WORK = mpms.TBMRECEIVE_WORK.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                        && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_WORK != null)
                {
                    lblOVC_PUR_ASS_VEN_CODE_Sign.Text = GetTbm1407Desc("C7", tbmRECEIVE_WORK.OVC_PUR_ASS_VEN_CODE);
                }



                TBM1303 tbm1303 = new TBM1303();
                tbm1303 = mpms.TBM1303.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                        && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_TIMES == numONB_TIMES
                                        && tb.ONB_GROUP == numONB_GROUP).FirstOrDefault();
                if (tbm1303 != null)
                {
                    lblOVC_DOPEN.Text = GetTaiwanDate(tbm1303.OVC_DOPEN);
                    lblOVC_OPEN_HOUR.Text = tbm1303.OVC_OPEN_HOUR;
                    lblOVC_OPEN_MIN.Text = tbm1303.OVC_OPEN_MIN;
                    lblONB_TIMES.Text = tbm1303.ONB_TIMES.ToString();
                    lblONB_GROUP.Text = tbm1303.ONB_GROUP.ToString();
                }
                SetGvTBM1313("1", "Site");
                SetGvTBM1313("2", "Comm");
                SetGvTBM1313("3", "Elec");
            }
        }


        private void SetGvTBM1313(string strOVC_KIND, string strOVC_KIND_Name)
        {
            DataTable dt, dtVENDOR;
            GridView gv = ((GridView)this.Master.FindControl("MainContent").FindControl("gvTBM1313_" + strOVC_KIND_Name));
            DropDownList ddl = ((DropDownList)this.Master.FindControl("MainContent").FindControl("ddlOVC_VEN_CST_" + strOVC_KIND_Name));


            var queryTBM1313 =
                from tb1313 in mpms.TBM1313
                where tb1313.OVC_PURCH.Equals(strOVC_PURCH) && tb1313.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                        && tb1313.OVC_DOPEN.Equals(strOVC_DOPEN) && tb1313.ONB_GROUP == numONB_GROUP && tb1313.OVC_KIND.Equals(strOVC_KIND)
                select new
                {
                    OVC_SN = new Guid(),
                    OVC_VEN_CST = tb1313.OVC_VEN_CST,
                    OVC_VEN_TITLE = tb1313.OVC_VEN_TITLE,
                    OVC_VEN_TEL = tb1313.OVC_VEN_TEL,
                    OVC_DBID = tb1313.OVC_DBID,
                    OVC_BID_HOUR = tb1313.OVC_BID_HOUR,
                    OVC_BID_MIN = tb1313.OVC_BID_MIN,
                };
            dt = CommonStatic.LinqQueryToDataTable(queryTBM1313);

            foreach (DataRow rows in dt.Rows)
            {
                rows["OVC_DBID"] = GetTaiwanDate(rows["OVC_DBID"].ToString());
            }
            FCommon.GridView_dataImport(gv, dt);
            //FCommon.list_dataImport(ddl, dt, "OVC_VEN_TITLE", "OVC_VEN_CST", true);


            var queryBID_VENDOR =
                from tbBID_VENDOR in mpms.TBMBID_VENDOR
                where tbBID_VENDOR.OVC_PURCH.Equals(strOVC_PURCH) && tbBID_VENDOR.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                    && tbBID_VENDOR.OVC_DOPEN.Equals(strOVC_DOPEN) && tbBID_VENDOR.ONB_GROUP == numONB_GROUP
                select new
                {
                    OVC_VEN_CST = tbBID_VENDOR.OVC_VEN_CST,
                    OVC_VEN_TITLE = tbBID_VENDOR.OVC_VEN_TITLE,
                };
            dtVENDOR = CommonStatic.LinqQueryToDataTable(queryBID_VENDOR);
            FCommon.list_dataImport(ddl, dtVENDOR, "OVC_VEN_TITLE", "OVC_VEN_CST", true);

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


        #endregion
        
        
    }
}