using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D16_3 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string strOVC_PURCH, strOVC_PURCH_5, strOVC_DOPEN;
        short numONB_TIMES;

        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);

            if (Request.QueryString["OVC_PURCH"] != null)
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                strOVC_PURCH = Request.QueryString["OVC_PURCH"].ToString();
                strOVC_PURCH_5 = Request.QueryString["OVC_PURCH_5"]?.ToString();
                strOVC_DOPEN = Request.QueryString["OVC_DOPEN"]?.ToString();
                if (Request.QueryString["ONB_TIMES"] != null)
                    numONB_TIMES = short.Parse(Request.QueryString["ONB_TIMES"].ToString());
                if (!IsPostBack)
                {
                    DataImport();
                }
            }
        }

        protected void rdoOVC_CHAIRMAN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(rdoOVC_CHAIRMAN.SelectedValue != "" && rdoOVC_CHAIRMAN.SelectedValue != null)
                rdoOVC_CHAIRMAN_Other.Checked = false;
        }

        protected void rdoOVC_CHAIRMAN_Other_CheckedChanged(object sender, EventArgs e)
        {
            if(rdoOVC_CHAIRMAN_Other.Checked == true)
                rdoOVC_CHAIRMAN.SelectedValue = null;
        }


        #region Button OnClick
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //存檔
            var query = gm.TBM1407;
            bool isRESTRICT;
            string strErrorMsg="";

            if (txtONB_TIMES.Text == "")
                strErrorMsg = "請輸入開標次數";

            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null & strErrorMsg=="")
            {
                //招標方式=="C" =>限制性招標(未經公開評選或公告徵求)
                isRESTRICT = tbm1301.OVC_PUR_ASS_VEN_CODE == "C" ? true : false;
                tbm1301.OVC_PUR_ASS_VEN_CODE = ddlOVC_PUR_ASS_VEN_CODE.SelectedValue;
                tbm1301.OVC_BID_MONEY = txtOVC_BID_MONEY.Text;
                gm.SaveChanges();


                TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
                tbmRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                        && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_ANNOUNCE != null)
                {
                    //修改 TBMRECEIVE_ANNOUNCE (採購公告紀錄檔)
                    if (!isRESTRICT)
                    {
                        //非限制招標則儲存公告日期 (限制性招標免填)
                        tbmRECEIVE_ANNOUNCE.OVC_DANNOUNCE = txtOVC_DANNOUNCE.Text;
                    }
                    tbmRECEIVE_ANNOUNCE.OVC_DOPEN = txtOVC_DOPEN.Text;
                    tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR = txtOVC_OPEN_HOUR.Text;
                    tbmRECEIVE_ANNOUNCE.OVC_OPEN_MIN = txtOVC_OPEN_MIN.Text;
                    tbmRECEIVE_ANNOUNCE.ONB_TIMES = short.Parse(txtONB_TIMES.Text);
                    mpms.SaveChanges();
                }
                else
                {
                    //新增 TBMRECEIVE_ANNOUNCE (採購公告紀錄檔)
                    TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE_new = new TBMRECEIVE_ANNOUNCE();
                    tbmRECEIVE_ANNOUNCE_new.OVC_PURCH = strOVC_PURCH;
                    tbmRECEIVE_ANNOUNCE_new.OVC_PURCH_5 = strOVC_PURCH_5;
                    tbmRECEIVE_ANNOUNCE_new.OVC_DOPEN = strOVC_DOPEN;
                    tbmRECEIVE_ANNOUNCE_new.ONB_TIMES = numONB_TIMES;
                    tbmRECEIVE_ANNOUNCE_new.OVC_DOPEN = txtOVC_DOPEN.Text;
                    tbmRECEIVE_ANNOUNCE_new.OVC_OPEN_HOUR = txtOVC_OPEN_HOUR.Text;
                    tbmRECEIVE_ANNOUNCE_new.OVC_OPEN_MIN = txtOVC_OPEN_MIN.Text;
                    tbmRECEIVE_ANNOUNCE_new.ONB_TIMES = short.Parse(txtONB_TIMES.Text);
                    mpms.TBMRECEIVE_ANNOUNCE.Add(tbmRECEIVE_ANNOUNCE_new);
                    mpms.SaveChanges();
                }



                TBMRECEIVE_WORK tbmRECEIVE_WORK = new TBMRECEIVE_WORK();
                tbmRECEIVE_WORK = mpms.TBMRECEIVE_WORK.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                        && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_WORK != null)
                {
                    //修改 TBMRECEIVE_WORK (採購簽辦檔)
                    if (!isRESTRICT)
                    {
                        //非限制招標則儲存發售標單日期 (限制招標免填)
                        tbmRECEIVE_WORK.OVC_DSELL = txtOVC_DSELL.Text;
                        tbmRECEIVE_WORK.OVC_SELL_HOUR = txtOVC_SELL_HOUR.Text;
                        tbmRECEIVE_WORK.OVC_SELL_MIN = txtOVC_SELL_MIN.Text;
                    }
                    if (rdoOVC_CHAIRMAN.SelectedValue == "處長" || rdoOVC_CHAIRMAN.SelectedValue == "副處長")
                        tbmRECEIVE_WORK.OVC_CHAIRMAN = rdoOVC_CHAIRMAN.SelectedValue;
                    else if (rdoOVC_CHAIRMAN_Other.Checked == true)
                        tbmRECEIVE_WORK.OVC_CHAIRMAN = txtOVC_CHAIRMAN_Other.Text;
                    tbmRECEIVE_WORK.OVC_DWAIT = txtOVC_DWAIT.Text;
                    tbmRECEIVE_WORK.OVC_DESC = txtOVC_DESC.Text;
                    tbmRECEIVE_WORK.OVC_MEETING = txtOVC_MEETING.Text;
                    tbmRECEIVE_WORK.OVC_ADVICE = txtOVC_ADVICE.Text;
                    tbmRECEIVE_WORK.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;
                    mpms.SaveChanges();
                }
                else
                {
                    //新增 TBMRECEIVE_WORK (採購簽辦檔)
                    TBMRECEIVE_WORK tbmRECEIVE_WORK_new = new TBMRECEIVE_WORK();
                    if (!isRESTRICT)
                    {
                        //非限制招標則儲存發售標單日期 (限制招標免填)
                        tbmRECEIVE_WORK_new.OVC_DSELL = txtOVC_DSELL.Text;
                        tbmRECEIVE_WORK_new.OVC_SELL_HOUR = txtOVC_SELL_HOUR.Text;
                        tbmRECEIVE_WORK_new.OVC_SELL_MIN = txtOVC_SELL_MIN.Text;
                    }
                    if (rdoOVC_CHAIRMAN.SelectedValue == "處長" || rdoOVC_CHAIRMAN.SelectedValue == "副處長")
                        tbmRECEIVE_WORK_new.OVC_CHAIRMAN = rdoOVC_CHAIRMAN.SelectedValue;
                    else if (rdoOVC_CHAIRMAN_Other.Checked == true)
                        tbmRECEIVE_WORK_new.OVC_CHAIRMAN = txtOVC_CHAIRMAN_Other.Text;
                    tbmRECEIVE_WORK_new.OVC_DWAIT = txtOVC_DWAIT.Text;
                    tbmRECEIVE_WORK_new.OVC_DESC = txtOVC_DESC.Text;
                    tbmRECEIVE_WORK_new.OVC_MEETING = txtOVC_MEETING.Text;
                    tbmRECEIVE_WORK_new.OVC_ADVICE = txtOVC_ADVICE.Text;
                    tbmRECEIVE_WORK_new.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;
                    mpms.TBMRECEIVE_WORK.Add(tbmRECEIVE_WORK_new);
                    mpms.SaveChanges();
                }
            }
            if (strErrorMsg == "")
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "存檔成功！");
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErrorMsg);
        }

        protected void btnReturnP_Click(object sender, EventArgs e)
        {
            //回公告畫面
            string send_url = "~/pages/MPMS/D/MPMS_D16_1.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_PURCH_5=" + strOVC_PURCH_5
                            + "&OVC_DOPEN=" + strOVC_DOPEN + "&ONB_TIMES=" + numONB_TIMES;
            Response.Redirect(send_url);
        }

        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            //回主流程畫面
            string send_url = "~/pages/MPMS/D/MPMS_D14_1.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_PURCH_5=" + strOVC_PURCH_5;
            Response.Redirect(send_url);
        }

        #endregion




        #region 副程式
        private void DataImport()
        {
            //帶入資料
            var query = gm.TBM1407;
            bool isRESTRICT;
            
            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                //招標方式=="C" =>限制性招標(未經公開評選或公告徵求)
                isRESTRICT = tbm1301.OVC_PUR_ASS_VEN_CODE == "C" ? true : false;
                lblOVC_PURCH_A_5.Text = tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                lblOVC_PURCH.Text = tbm1301.OVC_PURCH;
                lblOVC_PUR_AGENCY.Text = tbm1301.OVC_PUR_AGENCY;
                lblOVC_PURCH_5.Text = strOVC_PURCH_5;
                txtONB_TIMES.Text = numONB_TIMES.ToString();

                lblOVC_PUR_IPURCH.Text = tbm1301.OVC_PUR_IPURCH;
                lblOVC_PUR_NSECTION.Text = tbm1301.OVC_PUR_NSECTION;
                SetDrp(ddlOVC_PUR_ASS_VEN_CODE, "C7");
                ddlOVC_PUR_ASS_VEN_CODE.SelectedValue = tbm1301.OVC_PUR_ASS_VEN_CODE;
                lblOVC_BUDGET_BUY.Text = string.Format("{0:N}", tbm1301.ONB_PUR_BUDGET);
                if (tbm1301.OVC_PURCH_KIND == "1")
                {
                    lblONB_PUR_BUDGET.Text = "新台幣 " + string.Format("{0:N}", tbm1301.ONB_PUR_BUDGET) + "元整";
                }
                else if (tbm1301.OVC_PURCH_KIND == "2")
                {
                    lblONB_PUR_BUDGET.Text = GetTbm1407Desc("B0", tbm1301.OVC_PUR_CURRENT) + " " + string.Format("{0:N}", tbm1301.ONB_PUR_BUDGET);
                    lblONB_PUR_RATE.Visible = true;
                    lblONB_PUR_RATE.Text += " (匯率：" + tbm1301.ONB_PUR_RATE + ")";
                }
                txtOVC_BID_MONEY.Text = tbm1301.OVC_BID_MONEY;
                lblOVC_BID_TIMES.Text = "投、開標方式：" + GetTbm1407Desc("TG", tbm1301.OVC_BID_TIMES);





                TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
                tbmRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                        && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_ANNOUNCE != null)
                {
                    lblOVC_BID_METHOD_1.Text = tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_1;
                    lblOVC_BID_METHOD_2.Text = tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_2;
                    lblOVC_BID_METHOD_3.Text = tbmRECEIVE_ANNOUNCE.OVC_BID_METHOD_3;
                    if (isRESTRICT)
                    {
                        lblOVC_DANNOUNCE.Visible = true;
                    }
                    else
                    {
                        divOVC_DANNOUNCE.Visible = true;
                        txtOVC_DANNOUNCE.Text = tbmRECEIVE_ANNOUNCE.OVC_DANNOUNCE;
                    }
                    lblOVC_PERFORMANCE_LIMIT.Text = tbmRECEIVE_ANNOUNCE.OVC_PERFORMANCE_LIMIT;
                    txtOVC_DOPEN.Text = tbmRECEIVE_ANNOUNCE.OVC_DOPEN;
                    txtOVC_OPEN_HOUR.Text = tbmRECEIVE_ANNOUNCE.OVC_OPEN_HOUR;
                    txtOVC_OPEN_MIN.Text = tbmRECEIVE_ANNOUNCE.OVC_OPEN_MIN;
                }



                TBMRECEIVE_WORK tbmRECEIVE_WORK = new TBMRECEIVE_WORK();
                tbmRECEIVE_WORK = mpms.TBMRECEIVE_WORK.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                        && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_WORK != null)
                {
                    if (!isRESTRICT)
                    {
                        txtOVC_DSELL.Text = tbmRECEIVE_WORK.OVC_DSELL;
                        txtOVC_SELL_HOUR.Text = tbmRECEIVE_WORK.OVC_SELL_HOUR;
                        txtOVC_SELL_MIN.Text = tbmRECEIVE_WORK.OVC_SELL_MIN;
                    }
                    if (tbmRECEIVE_WORK.OVC_CHAIRMAN == "處長" || tbmRECEIVE_WORK.OVC_CHAIRMAN == "副處長")
                        rdoOVC_CHAIRMAN.SelectedValue = tbmRECEIVE_WORK.OVC_CHAIRMAN;
                    else if (tbmRECEIVE_WORK.OVC_CHAIRMAN != null && tbmRECEIVE_WORK.OVC_CHAIRMAN != "")
                    {
                        txtOVC_CHAIRMAN_Other.Text = tbmRECEIVE_WORK.OVC_CHAIRMAN;
                        rdoOVC_CHAIRMAN_Other.Checked = true;
                    }
                    txtOVC_DWAIT.Text = tbmRECEIVE_WORK.OVC_DWAIT;
                    txtOVC_DESC.Text = tbmRECEIVE_WORK.OVC_DESC;
                    txtOVC_MEETING.Text = tbmRECEIVE_WORK.OVC_MEETING;
                    txtOVC_ADVICE.Text = tbmRECEIVE_WORK.OVC_ADVICE;
                    txtOVC_DAPPROVE.Text = tbmRECEIVE_WORK.OVC_DAPPROVE;
                }

                if (isRESTRICT)
                {
                    lblOVC_DSELL.Visible = true;
                    lblOVC_DWAIT.Text = "限制性招標本欄位請自行填入";
                    lblDwaitTip_Restrict.Visible = true;
                }
                else
                {
                    divOVC_DSELL.Visible = true;
                    lblOVC_DWAIT.Text = "系統自動計算：發售標單截止日期 - 公告日期(亦可自行修訂)";
                    lblDwaitTip.Visible = true;
                    lblOVC_BID_TIMES.Visible = true;
                }
            }
        }

        

        private void SetDrp(ListControl list, string cateID)
        {
            //設定DropDownList選項
            DataTable dt;
            dt = CommonStatic.ListToDataTable(gm.TBM1407.Where(tb => tb.OVC_PHR_CATE.Equals(cateID)).ToList());
            FCommon.list_dataImport(list, dt, "OVC_PHR_DESC", "OVC_PHR_ID", true);
        }

        private void SetRdoOVC_CHAIRMAN(string strOVC_CHAIRMAN)
        {
            

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
            return "";
        }



        #endregion


    }
}