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
    public partial class MPMS_D16_4 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string strOVC_PURCH, strOVC_PURCH_5, strOVC_DOPEN, strOVC_DANNOUNCE;
        short numONB_TIMES;


        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            if (Request.QueryString["OVC_PURCH"] != null)
            {
                strOVC_PURCH = Request.QueryString["OVC_PURCH"].ToString();
                strOVC_PURCH_5 = Request.QueryString["OVC_PURCH_5"]?.ToString();
                strOVC_DOPEN = Request.QueryString["OVC_DOPEN"]?.ToString();
                if (Request.QueryString["ONB_TIMES"] != null)
                    numONB_TIMES = short.Parse(Request.QueryString["ONB_TIMES"].ToString());
                strOVC_DANNOUNCE = Request.QueryString["OVC_DANNOUNCE"]?.ToString();
                if (!IsPostBack)
                {
                    DataImport();
                }
            }
        }




        #region Button OnClick
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //存檔
            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
                tbmRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                        && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_ANNOUNCE != null)
                {
                    tbmRECEIVE_ANNOUNCE.OVC_DANNOUNCE = txtOVC_DANNOUNCE.Text;
                    tbmRECEIVE_ANNOUNCE.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;



                    TBMANNOUNCE_MODIFY tbmANNOUNCE_MODIFY = new TBMANNOUNCE_MODIFY();
                    tbmANNOUNCE_MODIFY = mpms.TBMANNOUNCE_MODIFY.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                            && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DANNOUNCE.Equals(strOVC_DANNOUNCE)).FirstOrDefault();
                    if (tbmANNOUNCE_MODIFY != null)
                    {
                        //修改
                        tbmANNOUNCE_MODIFY.OVC_DSEND = txtOVC_DSEND.Text;
                        tbmANNOUNCE_MODIFY.OVC_AGNT_IN = txtOVC_AGNT_IN.Text;
                        tbmANNOUNCE_MODIFY.OVC_NAME = txtOVC_NAME.Text;
                        tbmANNOUNCE_MODIFY.OVC_TELEPHONE = txtOVC_TELEPHONE.Text;
                        tbmANNOUNCE_MODIFY.OVC_DESC_ORG = txtOVC_DESC_ORG.Text;
                        tbmANNOUNCE_MODIFY.OVC_DESC_MODIFY = txtOVC_DESC_MODIFY.Text;
                        tbmANNOUNCE_MODIFY.OVC_DOPEN_N = txtOVC_DOPEN_N.Text;
                        tbmANNOUNCE_MODIFY.OVC_OPEN_HOUR_N = txtOVC_OPEN_HOUR_N.Text;
                        tbmANNOUNCE_MODIFY.OVC_OPEN_MIN_N = txtOVC_OPEN_MIN_N.Text;
                        tbmANNOUNCE_MODIFY.OVC_DESC = txtOVC_DESC.Text;
                        tbmANNOUNCE_MODIFY.OVC_MEMO = txtOVC_MEMO.Text;
                        mpms.SaveChanges();
                    }
                    else
                    {
                        //新增
                        TBMANNOUNCE_MODIFY tbmANNOUNCE_MODIFY_New = new TBMANNOUNCE_MODIFY
                        {
                            OVC_PURCH = strOVC_PURCH,
                            OVC_PURCH_5 = strOVC_PURCH_5,
                            OVC_DWORK = DateTime.Now.ToShortDateString().ToString(),
                            OVC_DANNOUNCE = strOVC_DANNOUNCE,
                            OVC_DSEND = txtOVC_DSEND.Text,
                            OVC_AGNT_IN = txtOVC_AGNT_IN.Text,
                            OVC_NAME = txtOVC_NAME.Text,
                            OVC_TELEPHONE = txtOVC_TELEPHONE.Text,
                            OVC_DESC_ORG = txtOVC_DESC_ORG.Text,
                            OVC_DESC_MODIFY = txtOVC_DESC_MODIFY.Text,
                            OVC_DOPEN_N = txtOVC_DOPEN_N.Text,
                            OVC_OPEN_HOUR_N = txtOVC_OPEN_HOUR_N.Text,
                            OVC_OPEN_MIN_N = txtOVC_OPEN_MIN_N.Text,
                            OVC_DESC = txtOVC_DESC.Text,
                            OVC_MEMO = txtOVC_MEMO.Text,
                        };
                        mpms.TBMANNOUNCE_MODIFY.Add(tbmANNOUNCE_MODIFY_New);
                        mpms.SaveChanges();
                    }
                }
                else
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查無此公告");
                }
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查無此購案");
            }
        }

        protected void btnReturnC_Click(object sender, EventArgs e)
        {
            //回修正公告選擇畫面

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
            string strOVC_PUR_IPURCH = "0";
            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                TBM1201 tbm1201 = new TBM1201();
                tbm1201 = mpms.TBM1201.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).OrderBy(tb => tb.ONB_POI_ICOUNT).FirstOrDefault();
                if (tbm1201 != null)
                    strOVC_PUR_IPURCH = tbm1201.ONB_POI_ICOUNT.ToString();
                lblOVC_PURCH_A_5.Text = tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                lblOVC_PURCH.Text = tbm1301.OVC_PURCH;
                lblOVC_PUR_AGENCY.Text = tbm1301.OVC_PUR_AGENCY;
                lblOVC_PURCH_5.Text = strOVC_PURCH_5;
                lblOVC_PUR_IPURCH.Text = tbm1301.OVC_PUR_IPURCH + "等計" + strOVC_PUR_IPURCH + "項";
                lblOVC_BID_TIMES.Text = GetTbm1407Desc("TG", tbm1301.OVC_BID_TIMES);



                TBMRECEIVE_ANNOUNCE tbmRECEIVE_ANNOUNCE = new TBMRECEIVE_ANNOUNCE();
                tbmRECEIVE_ANNOUNCE = mpms.TBMRECEIVE_ANNOUNCE.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                        && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DOPEN.Equals(strOVC_DOPEN)
                                        && tb.ONB_TIMES == numONB_TIMES).FirstOrDefault();
                if (tbmRECEIVE_ANNOUNCE != null)
                {
                    txtOVC_DANNOUNCE.Text = tbmRECEIVE_ANNOUNCE.OVC_DANNOUNCE;
                    lblOVC_DRAFT_COMM.Text = tbmRECEIVE_ANNOUNCE.OVC_DRAFT_COMM;
                    txtOVC_DAPPROVE.Text = tbmRECEIVE_ANNOUNCE.OVC_DAPPROVE;



                    TBMANNOUNCE_MODIFY tbmANNOUNCE_MODIFY = new TBMANNOUNCE_MODIFY();
                    tbmANNOUNCE_MODIFY = mpms.TBMANNOUNCE_MODIFY.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)
                                            && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5) && tb.OVC_DANNOUNCE.Equals(strOVC_DANNOUNCE)).FirstOrDefault();
                    if (tbmANNOUNCE_MODIFY != null)
                    {
                        txtOVC_DSEND.Text = tbmANNOUNCE_MODIFY.OVC_DSEND;
                        txtOVC_AGNT_IN.Text = tbmANNOUNCE_MODIFY.OVC_AGNT_IN;
                        txtOVC_NAME.Text = tbmANNOUNCE_MODIFY.OVC_NAME;
                        txtOVC_TELEPHONE.Text = tbmANNOUNCE_MODIFY.OVC_TELEPHONE;
                        txtOVC_DESC_ORG.Text = tbmANNOUNCE_MODIFY.OVC_DESC_ORG;
                        txtOVC_DESC_MODIFY.Text = tbmANNOUNCE_MODIFY.OVC_DESC_MODIFY;
                        rdoOVC_DOPEN_MODIFY.SelectedValue = tbmANNOUNCE_MODIFY.OVC_DOPEN_MODIFY;
                        lblOVC_DOPEN.Text = GetTaiwanDate(tbmANNOUNCE_MODIFY.OVC_DOPEN);
                        lblOVC_OPEN_HOUR.Text = tbmANNOUNCE_MODIFY.OVC_OPEN_HOUR;
                        lblOVC_OPEN_MIN.Text = tbmANNOUNCE_MODIFY.OVC_OPEN_MIN;
                        lblOVC_DOPEN_input.Text = GetTaiwanDate(tbmANNOUNCE_MODIFY.OVC_DOPEN) + " " + 
                                                    tbmANNOUNCE_MODIFY.OVC_OPEN_HOUR + "時" + tbmANNOUNCE_MODIFY.OVC_OPEN_MIN + "分";
                        txtOVC_DOPEN_N.Text = tbmANNOUNCE_MODIFY.OVC_DOPEN_N;
                        txtOVC_OPEN_HOUR_N.Text = tbmANNOUNCE_MODIFY.OVC_OPEN_HOUR_N;
                        txtOVC_OPEN_MIN_N.Text = tbmANNOUNCE_MODIFY.OVC_OPEN_MIN_N;
                        txtOVC_DESC.Text = tbmANNOUNCE_MODIFY.OVC_DESC;
                        txtOVC_MEMO.Text = tbmANNOUNCE_MODIFY.OVC_MEMO;
                    }
                }
                else
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查無此公告");
                }
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查無此購案");
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
                return string.Empty;
        }


        #endregion

    }
}