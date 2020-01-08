using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Linq;



namespace FCFDFE.pages.MPMS.B
{
    public partial class MPMS_B15_1 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strPurchNum = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (Request.QueryString["PurchNum"] != null)
                {
                    strPurchNum = Request.QueryString["PurchNum"];
                    if (!IsPostBack)
                    {

                        txtOVC_PURCH.Text = strPurchNum;
                        LoginScreen(strPurchNum);
                        if (Session["strMessage"] != null)
                        {
                            string strMessage = Session["strMessage"].ToString();
                            FCommon.AlertShow(PnMessage, "danger", "注意事項", strMessage);
                        }

                    }
                }
            }
        }
        #region onClick
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            //查詢按鈕功能
           
            string strMessage = "";
            string txtPurch = txtOVC_PURCH.Text;
            
            var query = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(txtPurch));
            if (query.Any())
            {
                strMessage += CheckData(txtOVC_PURCH.Text);
                strMessage += RemarkCheck(txtPurch);
                LoginScreen(txtPurch);
            }
            else
            {
                strMessage += "<p>本購案編號：" + txtPurch + " 不存在，請重新輸入！</p>";
                ClearControl();
            }
            FCommon.AlertShow(PnMessage, "danger", "注意事項", strMessage);

        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            //清除按鈕功能
            ClearControl();
        }
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            //回主畫面按鈕功能
            Response.Redirect("MPMS_B11");
        }
        #endregion

        #region 副程式
        private void LoginScreen(string PurchNum)
        {
            //資料載入
            var query =
                (from t in gm.TBM1301
                 join t2 in gm.TBM1407 on t.OVC_PUR_AGENCY equals t2.OVC_PHR_ID
                 where t.OVC_PURCH.Equals(PurchNum) && t2.OVC_PHR_CATE.Equals("C2")
                 select new
                 {
                     t.OVC_PUR_AGENCY,
                     t2.OVC_PHR_DESC,
                     t.OVC_PUR_IPURCH,
                     t.OVC_PUR_NSECTION,
                     t.OVC_PUR_SECTION,
                     t.OVC_PUR_USER,
                     t.OVC_PUR_IUSER_PHONE_EXT
                 }).FirstOrDefault();
            lblOVC_PUR_AGENCY.Text = query.OVC_PUR_AGENCY + query.OVC_PHR_DESC;
            lblOVC_PUR_IPURCH.Text = query.OVC_PUR_IPURCH;
            lblOVC_PUR_NSECTION.Text = query.OVC_PUR_NSECTION;
            lblOVC_PUR_SECTION.Text = query.OVC_PUR_SECTION;
            lblOVC_PUR_USER.Text = query.OVC_PUR_USER;
            lblOVC_PUR_IUSER_PHONE_EXT.Text = query.OVC_PUR_IUSER_PHONE_EXT;
        }

        private void ClearControl()
        {
            FCommon.Controls_Clear(txtOVC_PURCH, lblOVC_PUR_AGENCY, lblOVC_PUR_IPURCH
                , lblOVC_PUR_NSECTION, lblOVC_PUR_USER, lblOVC_PUR_SECTION, lblOVC_PUR_IUSER_PHONE_EXT);
        }
        
        private string CheckData(string purchNum)
        {
            string strMessage = "";

            //承辦單位？

            var table1301 =
                (from t in gm.TBM1301
                where t.OVC_PURCH.Equals(purchNum)
                select new
                {
                    OVC_PERMISSION_UPDATE = t.OVC_PERMISSION_UPDATE ?? "",
                    OVC_PUR_DCANPO = t.OVC_PUR_DCANPO,
                    IS_PLURAL_BASIS = t.IS_PLURAL_BASIS ?? "N/A",
                    OVC_PUR_AGENCY = t.OVC_PUR_AGENCY ?? "",
                    t.ONB_PUR_BUDGET
                }).FirstOrDefault();
            if (table1301 != null)
            {
                //上呈權限
                //20181108_舊條件會有null問題
                //if (table1301.OVC_PERMISSION_UPDATE.Equals("N"))
                //    strMessage += "<p>沒有上呈權限！</p>";
                if (table1301.OVC_PERMISSION_UPDATE != null && table1301.OVC_PERMISSION_UPDATE.Equals("N"))
                    strMessage += "<p>沒有上呈權限</p>";
                //撤案
                if (table1301.OVC_PUR_DCANPO != null)
                    strMessage += "<p>此購案已經撤案！</p>";
                //分案TBM1202
                var query1202 = mpms.TBM1201.Where(o => o.OVC_PURCH.Equals(purchNum));
                if (query1202.Any())
                {
                    //澄覆是否完成
                    var querycomment = mpms.TBM1202_COMMENT.Where(o => o.OVC_PURCH.Equals(purchNum)).Select(o => o.OVC_RESPONSE).Count();
                    var querisNullComment = mpms.TBM1202_COMMENT.Where(o => o.OVC_PURCH.Equals(purchNum) && o.OVC_DRESPONSE != null
                                                                        && o.OVC_RESPONSE != null && o.OVC_RESPONSER != null)
                                                                .Select(o => o.OVC_RESPONSE).Count();
                    if (querycomment != querisNullComment)
                    {
                        strMessage += "<p>澄覆尚未完成！</p>";
                    }
                }


                //複數決標分組
                if (table1301.IS_PLURAL_BASIS.Equals("Y"))
                {
                    var query1201Count = mpms.TBM1201.Where(o => o.OVC_PURCH.Equals(purchNum)).Select(o => o.ONB_POI_ICOUNT).Count();
                    var query1118COunt = mpms.TBM1118_2.Where(o => o.OVC_PURCH.Equals(purchNum)).Select(o => o.ONB_POI_ICOUNT).Count();
                    if (query1201Count != query1118COunt)
                    {
                        strMessage += "<p>複數決標要先分組 ! 尚有採購明細未分組 !</p>";
                    }
                }

                //採購明細
                var query = mpms.TBM1201.Where(o => o.OVC_PURCH.Equals(purchNum)).Select(o => o.ONB_POI_ICOUNT).Count();
                if (query <= 0)
                {
                    strMessage = "<p>無輸入採購明細（採購項次總數為 0）！</p>";
                }

                //檢查預算(1301)與明細總價(1201)  S、M案不檢查
                if (!(table1301.OVC_PUR_AGENCY.Equals("S") || table1301.OVC_PUR_AGENCY.Equals("M")))
                {
                    decimal numBudget = 0, numMoney = 0;
                    var queryMoney =
                    (from t in mpms.TBM1201
                     where t.OVC_PURCH.Equals(strPurchNum)
                     select new
                     {
                         money = t.ONB_POI_QORDER_PLAN * t.ONB_POI_MPRICE_PLAN
                     }).Sum(o => o.money);

                    if (queryMoney != null)
                        numMoney = (decimal)queryMoney;
                    if (table1301.ONB_PUR_BUDGET != null)
                        numBudget = (decimal)table1301.ONB_PUR_BUDGET;
                    if (queryMoney == 0 || table1301.ONB_PUR_BUDGET == 0)
                    {
                        strMessage += "<p>預算總價與明細金額不得為0！</p>";
                        strMessage += "<p>預算金額為" + numBudget.ToString("0.00") + "</p>";
                        strMessage += "<p>明細總價為" + numMoney.ToString("0.00") + "</p>";
                    }
                    else
                    {
                        if (queryMoney != table1301.ONB_PUR_BUDGET)
                        {
                            strMessage += "<p>預算總價與明細金額不相符！</p>";
                            strMessage += "<p>預算金額為" + numBudget.ToString("0.00") + "</p>";
                            strMessage += "<p>明細總價為" + numMoney.ToString("0.00") + "</p>";
                        }
                    }

                }
            }
            return strMessage;
        }

        private string RemarkCheck(string strPurchNum)
        {
            //備註檢查 L案另外檢查M47、M4A
            string strMessage = "";
            string field = "";
            string[] memoNames = { "投標廠商資格", "投標及開標方式", "報價及決標方式", "決標原則", "押標金", "履約保證金" };
            TBM1301 tb1301 = new TBM1301();
            tb1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            if (tb1301 != null)
            {
                string item = tb1301.OVC_PUR_AGENCY;
                if (item.Equals("L"))
                {
                    var queryM47 = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_IKIND.Equals("M47")).FirstOrDefault();
                    var queryM4Aee = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_IKIND.Equals("M4A") && o.OVC_CHECK.Equals("ee")).FirstOrDefault();
                    var queryM4Aff = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_IKIND.Equals("M4A") && o.OVC_CHECK.Equals("ff")).FirstOrDefault();
                    var queryM4Agg = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_IKIND.Equals("M4A") && o.OVC_CHECK.Equals("gg")).FirstOrDefault();
                    if (queryM47 == null)
                        strMessage += "<p>【物資申請書之備考欄中】(8.1)政府協購協定項目不得空白！</p>";
                    if (queryM4Aee == null && queryM4Aff == null && queryM4Agg == null)
                        strMessage += "<p>【物資申請書之備考欄中】(11)其他 第1、2、3項大陸地區相關項目不得空白！</p>";

                }
                switch (item)
                {
                    case "B":
                    case "L":
                    case "P":
                        field = "D5";
                        break;
                    case "A":
                    case "C":
                    case "E":
                    case "F":
                    case "W":
                        field = "W5";
                        break;
                }
                if (!field.Equals(string.Empty))
                {
                    for (int i = 0; i <= 5; i++)
                    {
                        string keyWord = field + i.ToString();
                        var query =
                            from t in mpms.TBM1220_1
                            where t.OVC_IKIND.Equals(keyWord) && t.OVC_PURCH.Equals(strPurchNum)
                            select t;
                        if (!query.Any())
                        {
                            strMessage += "<p>【計畫清單之備註欄中】" + memoNames[i] + "不得空白！</p>";
                        }
                    }
                }


            }
            return strMessage;


        }
        #endregion
    }
}