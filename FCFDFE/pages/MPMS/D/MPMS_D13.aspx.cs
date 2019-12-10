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
using Xceed.Words.NET;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D13 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string strUserName,strDept;
        string strOVC_PURCH;
        string strOVC_PURCH_5;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if ((string)(Session["XSSRequest"]) == "danger")
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "輸入錯誤，請重新輸入！");
                    Session["XSSRequest"] = null;
                }
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                //設置readonly屬性
                FCommon.Controls_Attributes("readonly", "true", txtOVC_DAPPROVE, txtOVC_DRECEIVE);

                if (FCommon.getQueryString(this, "OVC_PURCH", out strOVC_PURCH, false))
                {
                    FCommon.getQueryString(this, "OVC_PURCH_5", out strOVC_PURCH_5, false);
                    if (isPURCHASE_UNIT() && !IsPostBack)
                        DataImport();
                }
                else
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D11.aspx", false);      
            }  
        }

        protected void drpOVC_DO_NAME_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOVC_DO_NAME.Text = drpOVC_DO_NAME.SelectedValue;
            lblChangeOVC_DO_NAME.Text = drpOVC_DO_NAME.SelectedValue;
        }



        #region Button OnClick
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //存檔
            string strOVC_PURCH_5 = txtOVC_PURCH_5.Text;
            string strNew_STATUS = "";
            short[] numONB_ITEMs = {1,2,3,4 };

            if (isSave())
            {
                TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault(); 
                if (tbmRECEIVE_BID == null)
                {
                    TBMRECEIVE_BID tbmRECEIVE_BID_New = new TBMRECEIVE_BID();
                    //新增 採購收辦主檔
                    tbmRECEIVE_BID_New.OVC_PURCH = strOVC_PURCH;
                    tbmRECEIVE_BID_New.OVC_PURCH_5 = strOVC_PURCH_5;
                    tbmRECEIVE_BID_New.OVC_DRECEIVE = txtOVC_DRECEIVE.Text;
                    tbmRECEIVE_BID_New.OVC_NAME = strUserName;
                    tbmRECEIVE_BID_New.OVC_DO_NAME = lblChangeOVC_DO_NAME.Text;
                    tbmRECEIVE_BID_New.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;
                    if (rdoOVC_NAME_RESULT.SelectedValue != "")
                        tbmRECEIVE_BID_New.OVC_NAME_RESULT = rdoOVC_NAME_RESULT.SelectedValue;
                    mpms.TBMRECEIVE_BID.Add(tbmRECEIVE_BID_New);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmRECEIVE_BID_New.GetType().Name.ToString(), this, "新增");
                }
                else
                {
                    //修改 採購收辦主檔
                    tbmRECEIVE_BID.OVC_PURCH_5 = strOVC_PURCH_5;
                    tbmRECEIVE_BID.OVC_DRECEIVE = txtOVC_DRECEIVE.Text;
                    tbmRECEIVE_BID.OVC_NAME = strUserName;
                    tbmRECEIVE_BID.OVC_DO_NAME = lblChangeOVC_DO_NAME.Text;
                    tbmRECEIVE_BID.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;
                    if (rdoOVC_NAME_RESULT.SelectedValue != "")
                        tbmRECEIVE_BID.OVC_NAME_RESULT = rdoOVC_NAME_RESULT.SelectedValue;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmRECEIVE_BID.GetType().Name.ToString(), this, "修改");
                }



                foreach (short numONB_ITEM in numONB_ITEMs)
                {
                    string strOVC_ITEM_NAME, strOVC_RESULT;
                    if (numONB_ITEM == 4)
                        strOVC_ITEM_NAME = txtOVC_ITEM_NAME_4.Text;
                    
                    else
                    {
                        Label lblOVC_ITEM_NAME = (Label)this.Master.FindControl("MainContent").FindControl("lblOVC_ITEM_NAME_" + numONB_ITEM);
                        strOVC_ITEM_NAME = lblOVC_ITEM_NAME.Text;
                    }
                    RadioButtonList rdoOVC_RESULT = (RadioButtonList)this.Master.FindControl("MainContent").FindControl("rdoOVC_RESULT_" + numONB_ITEM);
                    strOVC_RESULT = rdoOVC_RESULT.SelectedValue;

                    TBMRECEIVE_BID_ITEM tbmRECEIVE_BID_ITEM =
                            mpms.TBMRECEIVE_BID_ITEM.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tb.OVC_KIND.Equals("1") && tb.ONB_ITEM == numONB_ITEM).FirstOrDefault();
                    if (tbmRECEIVE_BID_ITEM != null)
                    {
                        //修改 登管人員檢查項目
                        tbmRECEIVE_BID_ITEM.OVC_ITEM_NAME = strOVC_ITEM_NAME;
                        tbmRECEIVE_BID_ITEM.OVC_RESULT = strOVC_RESULT;
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmRECEIVE_BID_ITEM.GetType().Name.ToString(), this, "修改");
                    }
                    else
                    {
                        //新增 登管人員檢查項目
                        TBMRECEIVE_BID_ITEM tbmRECEIVE_BID_ITEM_New = new TBMRECEIVE_BID_ITEM()
                        {
                            OVC_PURCH = strOVC_PURCH,
                            OVC_PURCH_5 = strOVC_PURCH_5,
                            OVC_KIND = "1",
                            ONB_ITEM = numONB_ITEM,
                            OVC_ITEM_NAME = strOVC_ITEM_NAME,
                            OVC_RESULT = strOVC_RESULT,
                        };
                        mpms.TBMRECEIVE_BID_ITEM.Add(tbmRECEIVE_BID_ITEM_New);
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmRECEIVE_BID_ITEM_New.GetType().Name.ToString(), this, "新增");
                    }
                }

                

                //新增 購案階段紀錄檔
                //總辦意見欄SelectValue="1"：TBMSTATUS多一筆STATUS="21"  /   綜辦意見欄 SelectValue="2"：TBMSTATUS多一筆STATUS="19"
                if (rdoOVC_NAME_RESULT.SelectedValue == "1")  //SelectedValue = "1" ：可接受
                    strNew_STATUS = "21";
                else if (rdoOVC_NAME_RESULT.SelectedValue == "2")  //SelectedValue = "2" ：需澄覆
                    strNew_STATUS = "19";

                if (strNew_STATUS != "")
                {              
                    TBMSTATUS tbmSTATUS = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                        && tb.OVC_STATUS.Equals(strNew_STATUS)).FirstOrDefault();
                   
                    if (tbmSTATUS == null)
                    {
                        //新增 TBMSTATUS (購案階段紀錄檔)
                        TBMSTATUS tbmSTATUS_New = new TBMSTATUS
                        {
                            OVC_STATUS_SN = Guid.NewGuid(),
                            OVC_PURCH = strOVC_PURCH,
                            OVC_PURCH_5 = txtOVC_PURCH_5.Text,
                            ONB_TIMES = 1,
                            OVC_DO_NAME = lblChangeOVC_DO_NAME.Text,
                            OVC_STATUS = strNew_STATUS,
                            OVC_DBEGIN = DateTime.Now.ToString("yyyy-MM-dd"),
                        };
                        mpms.TBMSTATUS.Add(tbmSTATUS_New);
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmSTATUS_New.GetType().Name.ToString(), this, "新增");
                    }
                    else
                    {
                        //修改 TBMSTATUS (購案階段紀錄檔)
                        tbmSTATUS.OVC_PURCH_5 = txtOVC_PURCH_5.Text;
                        tbmSTATUS.ONB_TIMES = 1;
                        tbmSTATUS.OVC_DO_NAME = lblChangeOVC_DO_NAME.Text;
                        tbmSTATUS.OVC_STATUS = strNew_STATUS;
                        tbmSTATUS.OVC_DBEGIN = DateTime.Now.ToString("yyyy-MM-dd");
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmSTATUS.GetType().Name.ToString(), this, "修改");
                    }
                }

                if (lblChangeOVC_DO_NAME.Text != lblOVC_FROM_NAME.Text)
                {
                    //新增 承辦人異動歷史檔
                    TBMRECEIVE_LOG tbmRECEIVE_LOG = new TBMRECEIVE_LOG
                    {
                        OVC_PURCH = strOVC_PURCH,
                        OVC_STEP = "2",   //購案階段(1->計評  2->採購  3->履驗)
                        OVC_DUPDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        OVC_DO_NAME = strUserName,
                        OVC_FROM_NAME = lblOVC_FROM_NAME.Text,
                        OVC_TO_NAME = lblChangeOVC_DO_NAME.Text,
                        OVC_MEMO = "採購發包階段更換承辦人",
                    };
                    mpms.TBMRECEIVE_LOG.Add(tbmRECEIVE_LOG);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmRECEIVE_LOG.GetType().Name.ToString(), this, "新增");
                }

                //修改 OVC_STATUS="20" 收案(採購)的 階段結束日
                TBMSTATUS tbmSTATUS_20 = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                            && tb.ONB_TIMES == 1 && tb.OVC_STATUS.Equals("20")).FirstOrDefault();
                if (tbmSTATUS_20 != null)
                {
                    tbmSTATUS_20.OVC_DEND = DateTime.Now.ToString("yyyy-MM-dd");
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmSTATUS_20.GetType().Name.ToString(), this, "修改");
                }

                FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功！");
                DataImport();
            }
        }


        protected void btnBack_Click(object sender, EventArgs e)
        {
            //點擊 回上一頁
            string send_url = "~/pages/MPMS/D/MPMS_D11.aspx";
            Response.Redirect(send_url);
        }



        protected void lbtnVCover_Click(object sender, EventArgs e)
        {
            //點擊LinkButton 直式卷宗封面
            string templatePath = Server.MapPath("~/WordPDFprint/D13_5.docx");
            DocX document = DocX.Load(templatePath);

            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                document.ReplaceText("OVC_PURCH_A", strOVC_PURCH + tbm1301.OVC_PUR_AGENCY);
                document.ReplaceText("OVC_PURCH_5", txtOVC_PURCH_5.Text);
                document.ReplaceText("OVC_PUR_IPURCH", tbm1301.OVC_PUR_IPURCH);
            }
        }


        protected void lbtnHCover_Click(object sender, EventArgs e)
        {

        }


        #endregion



        #region 副程式
        private bool isPURCHASE_UNIT()
        {
            //檢查使用者是否為該購案採購發包的部門
            string strErrorMsg = "";
            DataTable dt = new DataTable();
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
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
                        strErrorMsg = "請輸入購案編號";
                    else if (tbm1301 == null)
                        strErrorMsg = "查無此購案編號";

                    else
                    {
                        var queryOvcPurch =
                            (from tbm1301Plan in gm.TBM1301_PLAN
                             where tbm1301Plan.OVC_PURCHASE_UNIT.Equals(strDept)
                                 && tbm1301Plan.OVC_PURCH.Equals(strOVC_PURCH)
                             join tbm1301_1 in gm.TBM1301 on tbm1301Plan.OVC_PURCH equals tbm1301_1.OVC_PURCH
                             select tbm1301Plan).ToList();

                        if (queryOvcPurch.Count == 0)
                            strErrorMsg = "非此購案的採購發包部門";
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
            //將資料庫資料帶出至畫面

            //設定 綜辦意見 RadioButtonList選項
            RdoImport(rdoOVC_NAME_RESULT, "RA");

            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                //購案編號
                lblOVC_PURCH_A.Text = tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY;
                //購案名稱(中文)
                lblOVC_PUR_IPURCH.Text = tbm1301.OVC_PUR_IPURCH;
                //計畫申購單位
                lblOVC_PUR_NSECTION.Text = tbm1301.OVC_PUR_NSECTION;
                //計畫申購單位代碼
                lblOVC_PUR_SECTION.Text = tbm1301.OVC_PUR_SECTION;
                //申購人
                lblOVC_PUR_USER.Text = tbm1301.OVC_PUR_USER;
                //電話
                lblOVC_PUR_IUSER_PHONE.Text = tbm1301.OVC_PUR_IUSER_PHONE;
                //軍線
                lblOVC_PUR_IUSER_PHONE_EXT.Text = tbm1301.OVC_PUR_IUSER_PHONE_EXT;
                //採購屬性(代碼GN)
                lblOVC_LAB.Text = GetTbm1407Desc("GN", tbm1301.OVC_LAB);
                //招標方式(代碼C7)
                lblOVC_PUR_ASS_VEN_CODE.Text = GetTbm1407Desc("C7", tbm1301.OVC_PUR_ASS_VEN_CODE);
                //投標段次(代碼TG)
                lblOVC_BID_TIMES.Text = GetTbm1407Desc("TG", tbm1301.OVC_BID_TIMES);
                //決標原則(代碼M3)
                lblOVC_BID.Text = GetTbm1407Desc("M3", tbm1301.OVC_BID);

                txtOVC_PURCH_5.Text = strOVC_PURCH_5;

                //設定 指派承辦人 下拉式選單
                Assigned(drpOVC_DO_NAME);

                TBMRECEIVE_BID tbmRECEIVE_BID = new TBMRECEIVE_BID();
                tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
                if (tbmRECEIVE_BID != null)
                {
                    //採購號碼
                    //txtOVC_PURCH_5.Text = tbmRECEIVE_BID.OVC_PURCH_5;
                    //承辦人若為空值：欄位名稱=指派承辦人 / 若不為空，欄位名稱=重新指派承辦人
                    lblOVC_DO_NAME.Text = tbmRECEIVE_BID.OVC_DO_NAME == "" ? "指派承辦人" : "重新指派承辦人";
                    //承辦人
                    txtOVC_DO_NAME.Text = tbmRECEIVE_BID.OVC_DO_NAME;
                    lblChangeOVC_DO_NAME.Text = tbmRECEIVE_BID.OVC_DO_NAME;
                    lblOVC_FROM_NAME.Text = tbmRECEIVE_BID.OVC_DO_NAME;
                    if (tbmRECEIVE_BID.OVC_DO_NAME != null && drpOVC_DO_NAME.Items.FindByValue(tbmRECEIVE_BID.OVC_DO_NAME) != null )
                        drpOVC_DO_NAME.SelectedValue = tbmRECEIVE_BID.OVC_DO_NAME;

                    //收辦日
                    txtOVC_DRECEIVE.Text = tbmRECEIVE_BID.OVC_DRECEIVE;
                    //綜辦意見
                    if (tbmRECEIVE_BID.OVC_NAME_RESULT != "")
                        rdoOVC_NAME_RESULT.SelectedValue = tbmRECEIVE_BID.OVC_NAME_RESULT;
                    //完成審查日
                    txtOVC_DAPPROVE.Text = tbmRECEIVE_BID.OVC_DAPPROVE;
                }
                else
                {
                    TBMPURCH_EXT tbmPURCH_EXT = mpms.TBMPURCH_EXT.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    //if (tbmPURCH_EXT != null)
                    //  txtOVC_PURCH_5.Text = tbmPURCH_EXT.OVC_PURCH_5;
                }

                TBMSTATUS tbmSTATUS = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH))
                    .OrderByDescending(tb => tb.OVC_STATUS == "" ? "0".Length : tb.OVC_STATUS == "3" ? "30".Length : tb.OVC_STATUS.Length)
                    .ThenByDescending(tb => tb.OVC_STATUS).FirstOrDefault();
                if (tbmSTATUS != null)
                {
                    lblSTATUS.Text = tbmSTATUS.OVC_STATUS;
                    //核評移案日
                    lblOVC_DBEGIN.Text = GetTaiwanDate(tbmSTATUS.OVC_DBEGIN);
                }


                int[] numONB_ITEMs = { 1, 2, 3, 4 };
                foreach (int numONB_ITEM in numONB_ITEMs)
                {
                    TBMRECEIVE_BID_ITEM tbmRECEIVE_BID_ITEM =
                        mpms.TBMRECEIVE_BID_ITEM.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                        && tb.OVC_KIND.Equals("1") && tb.ONB_ITEM == numONB_ITEM).FirstOrDefault();
                    if (tbmRECEIVE_BID_ITEM != null)
                    {
                        if (numONB_ITEM == 4)
                            txtOVC_ITEM_NAME_4.Text = tbmRECEIVE_BID_ITEM.OVC_ITEM_NAME;
                        RadioButtonList rdoOVC_RESULT = (RadioButtonList)this.Master.FindControl("MainContent").FindControl("rdoOVC_RESULT_" + numONB_ITEM);
                        rdoOVC_RESULT.SelectedValue = tbmRECEIVE_BID_ITEM.OVC_RESULT;
                    }
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無此購案編號");
        }


        private void RdoImport(RadioButtonList rdo,string cateID)
        {
            //設定 綜辦意見 RadioButtonList選項
            rdo.Items.Clear();
            DataTable dt;
            var query1407 = from tb1407 in gm.TBM1407 where tb1407.OVC_PHR_CATE.Equals(cateID) select tb1407;
            dt = CommonStatic.LinqQueryToDataTable(query1407);
            if (dt != null && dt.Rows.Count > 0)
            {
                rdo.DataSource = dt;
                rdo.DataValueField = "OVC_PHR_ID";
                rdo.DataTextField = "OVC_PHR_DESC";
                rdo.DataBind();
            }
        }


        private void Assigned(ListControl list)
        {
            //設定 指派承辦人 下拉式選單
            //條件：該綜辦單位(與該名分案人同單位) 且account_auth -> c_sn_auth = '3303'
            //or 該綜辦單位(與該名分案人同單位) 底下 TBM5200_1 C_SN_ROLE=32, OVC_PRIV_LEVEL=7  
            //var queryUserList =
            //    (from tbACCOUNT_AUTH in gm.ACCOUNT_AUTH.AsEnumerable()
            //     where tbACCOUNT_AUTH.C_SN_AUTH.Equals("3303")
            //     join tb5200_PPP in mpms.TBM5200_PPP on tbACCOUNT_AUTH.USER_ID equals tb5200_PPP.USER_ID
            //     where tb5200_PPP.OVC_PUR_SECTION.Equals(strDept)
            //     select tb5200_PPP.USER_ID)
            //    .Union
            //    (from tb5200_1 in mpms.TBM5200_1
            //     join tb5200_PPP in mpms.TBM5200_PPP on tb5200_1.USER_ID equals tb5200_PPP.USER_ID
            //     where tb5200_PPP.OVC_PUR_SECTION.Equals(strDept) && tb5200_1.C_SN_ROLE.Equals("32")
            //        && tb5200_1.OVC_PRIV_LEVEL.Equals("7")
            //     select tb5200_PPP.USER_NAME
            //     );
            var queryUserList =
                (from tbACCOUNT_AUTH in gm.ACCOUNT_AUTH.AsEnumerable()
                 where tbACCOUNT_AUTH.C_SN_AUTH != null
                 where tbACCOUNT_AUTH.C_SN_AUTH.Equals("3303")
                 join acc in mpms.ACCOUNT on tbACCOUNT_AUTH.USER_ID equals acc.USER_ID
                 where acc.DEPT_SN.Equals(strDept)
                 select acc.USER_NAME)
                .Union
                (from tb5200_1 in mpms.TBM5200_1
                 join tb5200_PPP in mpms.TBM5200_PPP on tb5200_1.USER_ID equals tb5200_PPP.USER_ID
                 where tb5200_PPP.OVC_PUR_SECTION.Equals(strDept) && tb5200_1.C_SN_ROLE.Equals("32")
                    && tb5200_1.OVC_PRIV_LEVEL.Equals("7")
                 select tb5200_PPP.USER_NAME
                 );
            list.Items.Clear();
            list.Items.Add(new System.Web.UI.WebControls.ListItem("請選擇", ""));
            foreach (var item in queryUserList)
            {
                list.Items.Add(item);
            }
        }

        private bool isSave()
        {
            string strErrorMsg = "";
            string strOVC_RESULT_N = "";
            string[] strONB_ITEMs = {"1","2","3","4" };
            string strOVC_NAME_RESULT = rdoOVC_NAME_RESULT.SelectedValue;
            if (txtOVC_PURCH_5.Text == "")
                strErrorMsg += "<p> 請輸入 採購號碼3碼 </p>";
            if (strOVC_NAME_RESULT == "")
                strErrorMsg += "<p> 請輸入 綜辦意見 </p>";

            //檢查項目表有勾選"否"之項目，綜辦意見欄不得為"可接受" (rdoOVC_NAME_RESULT.SelectedValue=1)
            if (rdoOVC_NAME_RESULT.SelectedValue == "1")   //SelectedValue = "1" ：可接受
            {
                foreach (string strONB_ITEM in strONB_ITEMs)
                {
                    if (strOVC_RESULT_N == "")
                    {
                        RadioButtonList rdoOVC_RESULT = (RadioButtonList)this.Master.FindControl("MainContent").FindControl("rdoOVC_RESULT_" + strONB_ITEM);
                        if (rdoOVC_RESULT.SelectedValue == "否")
                        {
                            strErrorMsg += "<p>檢查項目表有勾選\"否\"之項目，綜辦意見欄不得為\"可接受\"</p>";
                            strOVC_RESULT_N = "N";
                        }
                    }
                }
            }
            

            //收辦日不可空白
            if (txtOVC_DRECEIVE.Text == "")
            {
                strErrorMsg += "<p>請選擇收辦日</p>";
            }

            
            if (strErrorMsg == "")
                return true;
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErrorMsg);
                return false;
            }
        }

        

        private String GetTbm1407Desc(string cateID, string codeID)
        {
            //將TBM1407片語ID代碼轉為Desc
            TBM1407 tbm1407 = new TBM1407();
            if (codeID != null && codeID != "")
            {
                tbm1407 = gm.TBM1407.Where(table => table.OVC_PHR_CATE.Equals(cateID) && table.OVC_PHR_ID.Equals(codeID)).FirstOrDefault();
                if (tbm1407 != null)
                {
                    return tbm1407.OVC_PHR_DESC.ToString();
                }
            }
            return "";
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
            else
                return strDate;
        }


    }

        #endregion

}