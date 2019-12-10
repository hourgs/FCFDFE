using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MPMS.B
{

    public partial class MPMS_B11 : System.Web.UI.Page
    {  
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string[] strField = { "OVC_PURCH", "OVC_PUR_AGENCY", "OVC_PUR_IPURCH" , "OVC_DAPPLY" , "OVC_DPROPOSE" , "OVC_DRECEIVE_PAPER", "OVC_PURCH_OK" };
        public string strMenuName = "", strMenuNameItem = "";
        string year = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (Session["B11_YEAR"] != null)
                {
                    year = Session["B11_YEAR"].ToString();
                    dataImport(year);
                    if (!IsPostBack)
                    {
                        list_dataImport(drpOVC_BUDGET_YEAR);
                        drpOVC_BUDGET_YEAR.Items.FindByValue(year).Selected = true;
                    }
                    Session.Remove("B11_YEAR");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        list_dataImport(drpOVC_BUDGET_YEAR);
                        FCommon.GridView_setEmpty(GV_OVC_BUDGET, strField);
                    }
                }
            }
            
        }

        

        #region ~OnClick
        protected void btnQuery_OVC_BUDGET_Click(object sender, EventArgs e)
        {
            //上方年度查詢按鈕
            dataImport(drpOVC_BUDGET_YEAR.SelectedItem.ToString());
        }
        #endregion

        #region 副程式
        private void list_dataImport(ListControl list)
        {
            //帶入計畫年度下拉選單的值
            //先將下拉式選單清空
            list.Items.Clear();

            //取得台灣年月日
            DateTime datetime = DateTime.Now;
            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            int CalDateYear  = Convert.ToInt16(datetime.ToString("yyy", culture));
            int num = CalDateYear;
            for (int i = 0; i < 15; i++)
            {
                list.Items.Add(Convert.ToString(num));
                num = num - 1;
            }
        }

        

        protected void GV_OVC_BUDGET_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void GV_OVC_BUDGET_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnNew = (Button)e.Row.FindControl("btnNew");
                Button btnModify = (Button)e.Row.FindControl("btnModify");
                Button btnApplication = (Button)e.Row.FindControl("btnApplication");
                Button btnClosed = (Button)e.Row.FindControl("btnClosed");
                TBM1301_PLAN table1301p = new TBM1301_PLAN();
                TBM1301 table1301 = new TBM1301();
                Label lblPurch = (Label)e.Row.FindControl("lblPurch");
                string PurchNum = lblPurch.Text;

                table1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(PurchNum)).FirstOrDefault();
                var query1114 = mpms.TBM1114
                    .Where(t => t.OVC_PURCH.Equals(PurchNum))
                    .OrderByDescending(t => t.OVC_DATE).FirstOrDefault();
                if (table1301 == null)
                {
                    btnModify.CssClass += " disabled";
                    btnApplication.CssClass += " disabled";
                    btnClosed.CssClass += " disabled";
                    //FCommon.Controls_Attributes("readonly", "true", btnNew); // 新增控制項屬性
                    //FCommon.Controls_Attributes("readonly", btnModify, btnApplication, btnClosed); // 移除控制項屬性
                    //btnNew.Enabled = true;
                    //btnModify.Enabled = false;
                    //btnApplication.Enabled = false;
                    //btnClosed.Enabled = false;
                }
                else if (query1114 == null)
                {
                    btnNew.CssClass += " disabled";
                    //FCommon.Controls_Attributes("readonly", "true", btnModify, btnApplication, btnClosed); // 新增控制項屬性
                    //FCommon.Controls_Attributes("readonly", btnNew); // 移除控制項屬性
                    //btnNew.Enabled = false;
                    //btnModify.Enabled = true;
                    //btnApplication.Enabled = true;
                    //btnClosed.Enabled = true;
                }
                else if (query1114.OVC_REMARK == null || query1114.OVC_REMARK.Contains("將購案退回申購單位澄覆或修訂"))
                {
                    btnNew.CssClass += " disabled";
                }
                else
                {
                    btnNew.CssClass += " disabled";
                    btnModify.CssClass += " disabled";
                    btnApplication.CssClass += " disabled";
                    btnClosed.CssClass += " disabled";
                }
            }
        }
        protected void GV_OVC_BUDGET_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_OVC_BUDGET.DataKeys[gvrIndex].Value.ToString(); //OVC_PURCH

            switch (e.CommandName)
            {
                case "DataNew":
                    if (hidYESNO.Value.Equals("ok"))
                    {
                        string send_urlDataNew;
                        send_urlDataNew = "~/pages/MPMS/B/MPMS_B12.aspx?strToPurch=" + id;
                        Response.Redirect(send_urlDataNew);
                    }
                    else
                    {
                        string send_urlDataNew;
                        send_urlDataNew = "~/pages/MPMS/B/MPMS_B13.aspx?PurchNum=" + id;//原本是PurchNum 改成id
                        Session["B11_YEAR"] = drpOVC_BUDGET_YEAR.SelectedItem.ToString();
                        Response.Redirect(send_urlDataNew);
                    }
                    break;
                case "DataModify":
                    string send_urlModify;
                    send_urlModify = "~/pages/MPMS/B/MPMS_B13.aspx?PurchNum=" + id;//原本是PurchNum 改成id
                    Session["B11_YEAR"] = drpOVC_BUDGET_YEAR.SelectedItem.ToString();
                    Response.Redirect(send_urlModify);
                    break;
                case "DataApplication":
                    string strMessage="";
                    strMessage += CheckData(id);
                    strMessage += RemarkCheck(id);
                    if(strMessage.Equals(string.Empty))
                    {
                        string send_urlApp;
                        send_urlApp = "~/pages/MPMS/B/MPMS_B15.aspx?PurchNum=" + id;//原本是PurchNum 改成id
                        Session["B11_YEAR"] = drpOVC_BUDGET_YEAR.SelectedItem.ToString();
                        Response.Redirect(send_urlApp);
                    }
                    else
                    {
                        Session["strMessage"] = strMessage;
                        string send_urlApp;
                        send_urlApp = "~/pages/MPMS/B/MPMS_B15_1.aspx?PurchNum=" + id;//原本是PurchNum 改成id
                        Session["B11_YEAR"] = drpOVC_BUDGET_YEAR.SelectedItem.ToString();
                        Response.Redirect(send_urlApp);
                    }
                    break;
                case "DataClosed":
                    string send_urlDataClosed;
                    send_urlDataClosed = "~/pages/MPMS/B/MPMS_B14.aspx?PurchNum=" + id;//原本是PurchNum 改成id
                    Session["B11_YEAR"] = drpOVC_BUDGET_YEAR.SelectedItem.ToString();
                    Response.Redirect(send_urlDataClosed);
                    break;
                default:
                    break;
            }
        }

        protected void GV_OVC_BUDGET_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
            {
                //要隱藏的欄位    
                e.Row.Cells[7].Visible = false;
            }
        }

        private void dataImport(string strOVC_BUDGET_YEAR) {
            if (Session["userid"] != null)
            {
                string strUSER_ID = Session["userid"].ToString();
                DataTable dt = new DataTable();
                if (strUSER_ID.Length > 0)
                {
                    ACCOUNT ac = new ACCOUNT();
                    string userName = "";
                    string userdept = "";
                    ac = gm.ACCOUNTs.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                    if (ac != null)
                    {
                        userName = ac.USER_NAME;
                        userdept = ac.DEPT_SN;
                    }
                    string[] strParameterName = { ":qry_YY", ":USER_DEPT_SN", ":USER_Name" };
                    ArrayList aryData = new ArrayList();
                    
                    string Compare = strOVC_BUDGET_YEAR.Substring((strOVC_BUDGET_YEAR.Length - 2), 2);
                    aryData.Add(Compare);
                    aryData.Add(userdept);
                    aryData.Add(userName);
                    //linq 效能問題
                    ////已申請的案子
                    //var queryapply =
                    //    from tbTBM1301plan in gm.TBM1301_PLAN
                    //    join tbTBM1301 in gm.TBM1301 on tbTBM1301plan.OVC_PURCH equals tbTBM1301.OVC_PURCH into ps
                    //    from tbTBM1301 in ps.DefaultIfEmpty()
                    //    where tbTBM1301.OVC_PURCH.Substring(2, 2).Equals(Compare) && tbTBM1301.OVC_PUR_SECTION == userdept
                    //    && (tbTBM1301.OVC_PUR_USER == userName || tbTBM1301.OVC_KEYIN == userName) && tbTBM1301.OVC_PUR_DCANPO.Equals(null)
                    //    select new
                    //    {
                    //        OVC_PURCH = tbTBM1301plan.OVC_PURCH,
                    //        OVC_PUR_IPURCH = tbTBM1301plan.OVC_PUR_IPURCH,
                    //        OVC_DAPPLY = tbTBM1301plan.OVC_DAPPLY,
                    //        OVC_DPROPOSE = tbTBM1301.OVC_DPROPOSE,
                    //        OVC_PURCH_OK = tbTBM1301plan.OVC_PURCH_OK,
                    //    };

                    //var apply =
                    //    from t1301 in queryapply.AsEnumerable()
                    //    join t1202 in mpms.TBM1202 on t1301.OVC_PURCH equals t1202.OVC_PURCH
                    //    select new
                    //    {
                    //        OVC_PURCH = t1301.OVC_PURCH,
                    //        OVC_PUR_IPURCH = t1301.OVC_PUR_IPURCH,
                    //        OVC_DAPPLY = t1301.OVC_DAPPLY,
                    //        OVC_DPROPOSE = t1301.OVC_DPROPOSE,
                    //        OVC_PURCH_OK = t1301.OVC_PURCH_OK,
                    //        OVC_DRECEIVE_PAPER = t1202.OVC_DRECEIVE_PAPER
                    //    };


                    ////尚未申請的案子
                    //var queryunapply =
                    //    from tbm1301plan in gm.TBM1301_PLAN.AsEnumerable()
                    //    where (string.IsNullOrEmpty(tbm1301plan.OVC_PURCH_OK) || tbm1301plan.OVC_PURCH_OK.Equals("N")) 
                    //    where tbm1301plan.OVC_PURCH.Substring(2, 2).Equals(Compare)
                    //     && tbm1301plan.OVC_PUR_SECTION == userdept
                    //    where (tbm1301plan.OVC_PUR_USER == userName || tbm1301plan.OVC_KEYIN == userName)
                    //    select new
                    //    {
                    //        OVC_PURCH = tbm1301plan.OVC_PURCH,
                    //        OVC_PUR_IPURCH = tbm1301plan.OVC_PUR_IPURCH,
                    //        OVC_DAPPLY = tbm1301plan.OVC_DAPPLY,
                    //        OVC_DPROPOSE = "",
                    //        OVC_PURCH_OK = tbm1301plan.OVC_PURCH_OK,
                    //        OVC_DRECEIVE_PAPER =""
                    //    };

                    //var unionquery =
                    //    apply.Union(queryunapply);
                    
                    string strSQL = "";
                    strSQL += $@"Select A.Ovc_Purch , A.Ovc_Pur_Agency , A.Ovc_Pur_Ipurch , A.Ovc_Dapply , B.Ovc_Dpropose , A.Ovc_Purch_Ok, C.Ovc_Dreceive_Paper
                                 From Tbm1301_Plan A ,Tbm1301 B
                                 ,(Select Ovc_Purch, Max(Ovc_Dreceive_Paper) as Ovc_Dreceive_Paper  From Tbm1202
                                 Group By Ovc_Purch) C 
                                 Where A.Ovc_Purch =B.Ovc_Purch (+)
                                 and  A.Ovc_Purch =C.Ovc_Purch (+)
                                 And B.Ovc_Pur_Dcanpo Is Null 
                                 And Substr(B.Ovc_Purch,3,2)={ strParameterName[0] }
                                 And B.Ovc_Pur_Section = { strParameterName[1] }
                                 And ( B.Ovc_Pur_User = { strParameterName[2] } Or B.Ovc_Keyin = { strParameterName[2] }) 
                                 union
                                 Select b.Ovc_Purch , Ovc_Pur_Agency , b.Ovc_Pur_Ipurch , b.Ovc_Dapply , '' as Ovc_Dpropose , b.Ovc_Purch_Ok, '' as Ovc_Dreceive_Paper
                                 from tbm1301_plan  b  where nvl(b.ovc_purch_ok,'N') <> 'Y' 
                                 And Substr(Ovc_Purch,3,2)={ strParameterName[0] }
                                 And B.Ovc_Pur_Section = { strParameterName[1] }
                                 and ( b.OVC_PUR_USER = { strParameterName[2] } or OVC_KEYIN = { strParameterName[2] })";

                    dt = FCommon.getDataTableFromSelect(strSQL, strParameterName, aryData);
                    //dt = CommonStatic.LinqQueryToDataTable(unionquery);
                }
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_OVC_BUDGET, dt, strField);
            }
        }

        private string CheckData(string strPurchNum)
        {
            string strMessage = "";

            //承辦單位？
            TBM1301 table1301 = new TBM1301();
            table1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            if (table1301 != null)
            {
                //上呈權限
                if (table1301.OVC_PERMISSION_UPDATE != null && table1301.OVC_PERMISSION_UPDATE.Equals("N"))
                    strMessage += "<p>沒有上呈權限</p>";
                //撤案
                if (table1301.OVC_PUR_DCANPO != null)
                    strMessage += "<p>此購案已經撤案</p>";
                //分案TBM1202
                var query1202 = mpms.TBM1201.Where(o => o.OVC_PURCH.Equals(strPurchNum));
                if (query1202.Any())
                {
                    //澄覆是否完成
                    var querycomment = mpms.TBM1202_COMMENT.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.OVC_RESPONSE).Count();
                    var querisNullComment = mpms.TBM1202_COMMENT.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_DRESPONSE != null
                                                                        && o.OVC_RESPONSE != null && o.OVC_RESPONSER != null)
                                                                .Select(o => o.OVC_RESPONSE).Count();
                    if (querycomment != querisNullComment)
                    {
                        strMessage += "<p>澄覆尚未完成</p>";
                    }
                }


                //複數決標分組
                if (table1301.IS_PLURAL_BASIS != null)
                {
                    if(table1301.IS_PLURAL_BASIS == "Y")
                    {
                        var query1201Count = mpms.TBM1201.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.ONB_POI_ICOUNT).Count();
                        var query1118COunt = mpms.TBM1118_2.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.ONB_POI_ICOUNT).Count();
                        if (query1201Count != query1118COunt)
                        {
                            strMessage += "<p>複數決標要先分組 ! 尚有採購明細未分組 !</p>";
                        }
                    }
                }
                

                //採購明細
                var query = mpms.TBM1201.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.ONB_POI_ICOUNT).Count();
                if (query <= 0)
                {
                    strMessage = "<p>無輸入採購明細（採購項次總數為 0）！</p>";
                }

                //檢查預算(1301)與明細總價(1201)  S、M案不檢查
                if (table1301.OVC_PUR_AGENCY == null || !(table1301.OVC_PUR_AGENCY.Equals("S") || table1301.OVC_PUR_AGENCY.Equals("M")))
                {
                    decimal numBudget = 0, numMoney = 0;
                    var queryMoney =
                    (from t in mpms.TBM1201
                     where t.OVC_PURCH.Equals(strPurchNum)
                     select new
                     {
                         money = t.ONB_POI_QORDER_PLAN * t.ONB_POI_MPRICE_PLAN
                     }).Sum(o => o.money);

                    if(queryMoney != null)
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
            string strMessage="";
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
                        strMessage += "<p>【物資申請書之備註欄中】(8)政府協購協定項目不得空白！</p>";
                    if (queryM4Aee == null && queryM4Aff == null && queryM4Agg == null)
                        strMessage += "<p>【物資申請書之備註欄中】(11)其他 第1、2、3項大陸地區相關項目不得空白！</p>";
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
                            where  t.OVC_IKIND.Equals(keyWord) && t.OVC_PURCH.Equals(strPurchNum)
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