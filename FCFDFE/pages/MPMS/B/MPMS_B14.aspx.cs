using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Linq;
using System.Globalization;

namespace FCFDFE.pages.MPMS.B
{
    public partial class MPMS_B141 : System.Web.UI.Page
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
                        lblOVC_PURCH.Text = strPurchNum;
                        GVImport();
                        CheckResponse();
                    }
                }
                else
                {
                    Response.Redirect("MPMS_B11");
                }
            }

        }
        #region onClick
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            Button btnThis = (Button)sender;
            int gvRowIndex = (btnThis.NamingContainer as GridViewRow).RowIndex;
            ViewState["ONB_CHECK_TIMES"] = gvRowIndex + 1;
            string strONB_CHECK_TIMES = GV_Clarification.Rows[gvRowIndex].Cells[1].Text;
            Import_GV_ClarDetail(strONB_CHECK_TIMES);
            RepeaterHeaderImport(gvRowIndex);
            divMain.Visible = false;
            divContent.Visible = true;
        }

        protected void btnShowMain_Click(object sender, EventArgs e)
        {
            divMain.Visible = true;
            divContent.Visible = false;
        }
        protected void btnPresented_Click(object sender, EventArgs e)
        {
            var querycomment = mpms.TBM1202_COMMENT.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.OVC_RESPONSE).Count();
            var querisNullComment = mpms.TBM1202_COMMENT.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_DRESPONSE != null
                                                                && o.OVC_RESPONSE != null && o.OVC_RESPONSER != null).Count();
            if (querycomment != querisNullComment)
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "尚未完成澄覆");
            }
            else
            {
                string strMessage = "";
                strMessage += CheckData(strPurchNum);
                strMessage += RemarkCheck(strPurchNum);
                if (strMessage.Equals(string.Empty))
                {
                    string send_urlApp;
                    send_urlApp = "~/pages/MPMS/B/MPMS_B15.aspx?PurchNum=" + strPurchNum;//原本是PurchNum 改成id
                    Response.Redirect(send_urlApp);
                }
                else
                {
                    Session["strMessage"] = strMessage;
                    string send_urlApp;
                    send_urlApp = "~/pages/MPMS/B/MPMS_B15_1.aspx?PurchNum=" + strPurchNum;//原本是PurchNum 改成id
                    Response.Redirect(send_urlApp);
                }
            }
        }

        protected void Repeater_Header_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //存檔
            if (e.CommandName.Equals("Save"))
            {
                Repeater childRepeater = (Repeater)e.Item.FindControl("Repeater_Content");//找到要繫結資料的childRepeater
                HiddenField hidOVC_AUDIT_UNIT = (HiddenField)e.Item.FindControl("hidOVC_AUDIT_UNIT");

                foreach (RepeaterItem rt in childRepeater.Items)
                {
                    HiddenField hidOVC_TITLE = (HiddenField)rt.FindControl("hidOVC_TITLE");
                    HiddenField hidOVC_TITLE_ITEM = (HiddenField)rt.FindControl("hidOVC_TITLE_ITEM");
                    HiddenField hidOVC_TITLE_DETAIL = (HiddenField)rt.FindControl("hidOVC_TITLE_DETAIL");
                    HiddenField hidONB_NO = (HiddenField)rt.FindControl("hidONB_NO");
                    TextBox txtRESPONSE = (TextBox)rt.FindControl("txtOVC_RESPONSE");

                    int item = int.Parse(ViewState["ONB_CHECK_TIMES"].ToString());
                    int onbNo = int.Parse(hidONB_NO.Value);
                    string strOVC_AUDIT_UNIT = hidOVC_AUDIT_UNIT.Value;
                    string strOVC_TITLE = hidOVC_TITLE.Value;
                    string strOVC_TITLE_ITEM = hidOVC_TITLE_ITEM.Value;
                    string strOVC_TITLE_DETAIL = hidOVC_TITLE_DETAIL.Value;

                    TBM1202_COMMENT tbm1202_COMMENT = new TBM1202_COMMENT();
                    tbm1202_COMMENT = 
                        mpms.TBM1202_COMMENT.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.ONB_CHECK_TIMES == item
                                                     && o.OVC_AUDIT_UNIT.Equals(strOVC_AUDIT_UNIT) && o.OVC_TITLE.Equals(strOVC_TITLE)
                                                     && o.OVC_TITLE_ITEM.Equals(strOVC_TITLE_ITEM) && o.OVC_TITLE_DETAIL.Equals(strOVC_TITLE_DETAIL)
                                                     && o.ONB_NO == onbNo).FirstOrDefault();

                    if (tbm1202_COMMENT.OVC_RESPONSE == null || !tbm1202_COMMENT.OVC_RESPONSE.Equals(txtRESPONSE.Text))
                    {
                        //有改變內容才存 因為日期會蓋掉
                        tbm1202_COMMENT.OVC_DRESPONSE = DateTime.Now.ToString("yyyy-MM-dd");
                        tbm1202_COMMENT.OVC_RESPONSE = txtRESPONSE.Text;
                        tbm1202_COMMENT.OVC_RESPONSER = Session["username"].ToString();
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , tbm1202_COMMENT.GetType().Name.ToString(), this, "修改");
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
                    }
                }
                
            }
        }
        #endregion



        #region 副程式
        private void GVImport()
        {
            //澄覆查詢作業
            //先把主表查出來 
            string[] strField = { "ONB_CHECK_TIMES", "OVC_DRECEIVE_PAPER", "OVC_CHECK_OK", "OVC_CHECK_UNIT", "OVC_PHR_DESC" };
            var query =
               (from t1 in mpms.TBM1202
               join t2 in mpms.TBM1202_1 on new { t1.OVC_PURCH, t1.ONB_CHECK_TIMES } equals new { t2.OVC_PURCH, t2.ONB_CHECK_TIMES }
               where t1.OVC_PURCH.Equals(strPurchNum)
               orderby t1.ONB_CHECK_TIMES,t2.OVC_AUDIT_UNIT
               select new
               {
                   t1.ONB_CHECK_TIMES,
                   t1.OVC_DRECEIVE_PAPER,
                   t1.OVC_CHECK_OK,
                   t1.OVC_CHECK_UNIT,
                   t2.OVC_AUDIT_UNIT
               }).ToArray();
            var query2 =
               from t1 in query
               join t2 in gm.TBM1407 on t1.OVC_AUDIT_UNIT equals t2.OVC_PHR_ID
               join t3 in gm.TBMDEPTs on t1.OVC_CHECK_UNIT equals t3.OVC_DEPT_CDE
               where t2.OVC_PHR_CATE.Equals("K5")
               select new
               {
                   t1.ONB_CHECK_TIMES,
                   t1.OVC_DRECEIVE_PAPER,
                   t1.OVC_CHECK_OK,
                   OVC_CHECK_UNIT = t3.OVC_ONNAME,
                   t2.OVC_PHR_DESC
               };

            //合併欄位
            var result =
                query2.GroupBy(cc =>
                    new
                    {
                        cc.ONB_CHECK_TIMES,
                        cc.OVC_DRECEIVE_PAPER,
                        cc.OVC_CHECK_OK,
                        cc.OVC_CHECK_UNIT
                    }).Select(dd =>
                    new
                    {
                        dd.Key.ONB_CHECK_TIMES,
                        dd.Key.OVC_DRECEIVE_PAPER,
                        dd.Key.OVC_CHECK_OK,
                        dd.Key.OVC_CHECK_UNIT,
                        OVC_PHR_DESC = string.Join(";", dd.Select(ee => ee.OVC_PHR_DESC).ToList())
                    });
            DataTable dt = CommonStatic.LinqQueryToDataTable(result);
            ViewState["hasRow"] = FCommon.GridView_dataImport(GV_Clarification, dt, strField);

        }

        private void CheckResponse()
        {
            bool flag = false;
            string id = Session["userid"].ToString();
            string queryName = gm.ACCOUNTs.Where(o => o.USER_ID.Equals(id)).Select(o => o.USER_NAME).FirstOrDefault();
            var querycomment = mpms.TBM1202_COMMENT.Where(o => o.OVC_PURCH.Equals(strPurchNum)).Select(o => o.OVC_RESPONSE).Count();
            var querisNullComment = mpms.TBM1202_COMMENT.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_DRESPONSE != null
                                                                && o.OVC_RESPONSE != null && o.OVC_RESPONSER != null)
                                                        .Select(o => o.OVC_RESPONSE).Count();
            if (querycomment != querisNullComment)
            {
                flag = false;
            }
            else
            {
                var query1114 = mpms.TBM1114.Where(o => o.OVC_PURCH.Equals(strPurchNum)).OrderByDescending(o => o.OVC_DATE);

                if (query1114.Any())
                {
                    var item = query1114.FirstOrDefault();
                    if (!item.OVC_USER.Equals(queryName))
                    {
                        flag = true;
                    }
                }
                else
                {
                    flag = true;
                }
            }
            btnPresented.Visible = flag;
        }

        private void Import_GV_ClarDetail(string strONB_CHECK_TIMES)
        {
            byte checkItems = byte.Parse(strONB_CHECK_TIMES);
            string[] strField = { "OVC_PURCH", "ONB_CHECK_TIMES", "OVC_DRECEIVE", "OVC_CHECK_OK",
                                    "OVC_DRESULT","OVC_CHECK_UNIT","OVC_CHECKER"};
            var query =
                from table in mpms.TBM1202
                where table.OVC_PURCH.Equals(strPurchNum) && table.ONB_CHECK_TIMES == checkItems
                select new
                {
                    table.OVC_PURCH,
                    table.ONB_CHECK_TIMES,
                    table.OVC_DRECEIVE,
                    table.OVC_CHECK_OK,
                    table.OVC_DRESULT,
                    table.OVC_CHECK_UNIT,
                    table.OVC_CHECKER
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            dt.Rows[0]["OVC_DRECEIVE"] = getTaiwanDate(dt.Rows[0]["OVC_DRECEIVE"].ToString());
            dt.Rows[0]["OVC_CHECK_UNIT"] = getUnitName(dt.Rows[0]["OVC_CHECK_UNIT"].ToString());
            ViewState["hasRow"] = FCommon.GridView_dataImport(GV_ClarDetail, dt, strField);
            
        }

        private void RepeaterHeaderImport(int item)
        {
            //第一層Repeater
            //項次從1開始
            item += 1;
            var query =
               (from t1 in mpms.TBM1202_1
                join t2 in mpms.ACCOUNT on t1.OVC_AUDITOR equals t2.USER_NAME
                 into subg
                from t2 in subg.DefaultIfEmpty()
                where t1.OVC_PURCH.Equals(strPurchNum) && t1.ONB_CHECK_TIMES == item
                select new
                {
                    OVC_AUDIT_UNIT = t1.OVC_AUDIT_UNIT,
                    OVC_AUDITOR = t1.OVC_AUDITOR,
                    OVC_PUR_IUSER_PHONE = t2.IUSER_PHONE,
                }).Select(dd => new { dd.OVC_AUDIT_UNIT, dd.OVC_AUDITOR, dd.OVC_PUR_IUSER_PHONE })
                  .OrderBy(dd => dd.OVC_AUDIT_UNIT).Distinct().ToArray();
            var query2 =
                from t1 in query
                join t2 in gm.TBM1407 on t1.OVC_AUDIT_UNIT equals t2.OVC_PHR_ID
                where t2.OVC_PHR_CATE.Equals("K5")
                orderby t1.OVC_AUDIT_UNIT
                select new
                {
                    OVC_AUDIT_UNIT = t1.OVC_AUDIT_UNIT,
                    UnitName = t2.OVC_PHR_DESC,
                    OVC_AUDITOR = t1.OVC_AUDITOR,
                    OVC_PUR_IUSER_PHONE = t1.OVC_PUR_IUSER_PHONE,
                };

            DataTable dt = CommonStatic.LinqQueryToDataTable(query2);
            
            Repeater_Header.DataSource = dt;
            Repeater_Header.DataBind();

        }

        private DataTable RepeaterContentImport(int item, string dept)
        {
            //第二層Repeater
            //TBMOPINION 類別 TBMOPINION_ITEM項目 TBMOPINION_DETAIL標題 
            var query =
                from t1 in mpms.TBM1202_COMMENT
                join t2 in mpms.TBMOPINION on new {t1.OVC_AUDIT_UNIT, t1.OVC_TITLE} equals new {t2.OVC_AUDIT_UNIT,t2.OVC_TITLE}
                join t3 in mpms.TBMOPINION_ITEM 
                on new { t1.OVC_AUDIT_UNIT, t1.OVC_TITLE,t1.OVC_TITLE_ITEM } 
                equals new { t3.OVC_AUDIT_UNIT, t3.OVC_TITLE,t3.OVC_TITLE_ITEM }
                join t4 in mpms.TBMOPINION_DETAIL 
                on new { t1.OVC_AUDIT_UNIT, t1.OVC_TITLE, t1.OVC_TITLE_ITEM,t1.OVC_TITLE_DETAIL } 
                equals new { t4.OVC_AUDIT_UNIT, t4.OVC_TITLE, t4.OVC_TITLE_ITEM, t4.OVC_TITLE_DETAIL }
                where t1.OVC_PURCH.Equals(strPurchNum) && t1.ONB_CHECK_TIMES == item && t1.OVC_AUDIT_UNIT.Equals(dept)
                orderby t1.ONB_NO, t1.OVC_TITLE
                select new
                {
                    t1.ONB_NO,
                    OVC_TITLE=t1.OVC_TITLE,
                    OVC_TITLE_ITEM=t1.OVC_TITLE_ITEM,
                    OVC_TITLE_DETAIL=t1.OVC_TITLE_DETAIL,
                    OVC_TITLE_NAME = t2.OVC_CONTENT,
                    OVC_TITLE_ITEM_NAME = t3.OVC_CONTENT,
                    OVC_TITLE_DETAIL_NAME = t4.OVC_CONTENT,
                    t1.OVC_CHECK_REASON,
                    t1.OVC_RESPONSE
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            //要先把<br>拿掉
            foreach (DataRow rows in dt.Rows)
            {
                rows["OVC_RESPONSE"] = rows["OVC_RESPONSE"].ToString().Replace("<br>", "");
            }
            return dt;
        }
        

        protected void Repeater_Header_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item|| e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField lblDept = (HiddenField)e.Item.FindControl("hidOVC_AUDIT_UNIT");
                string dept = lblDept.Value;
                int item = int.Parse(ViewState["ONB_CHECK_TIMES"].ToString());
                Repeater childRepeater = (Repeater)e.Item.FindControl("Repeater_Content");//找到要繫結資料的childRepeater
                if (childRepeater != null)
                {
                    DataTable dt = new DataTable();
                    dt = RepeaterContentImport(item, dept);
                    childRepeater.DataSource = dt;
                    childRepeater.DataBind();
                    if (dt.Rows.Count > 0)
                    {
                        Button btnthis = (Button)e.Item.FindControl("btnSave");
                        btnthis.Visible = true;
                    }
                        
                }
            }
        }
        

        private string getTaiwanDate(string strDate)
        {
            if (DateTime.TryParse(strDate, out DateTime datetime))
            {
                CultureInfo culture = new CultureInfo("zh-TW");
                culture.DateTimeFormat.Calendar = new TaiwanCalendar();
                return datetime.ToString("yyy年MM月dd日", culture);
            }
            else
            {

                return "";
            }
        }

        protected void Repeater_Content_PreRender(object sender, EventArgs e)
        {
            //沒資料把表頭藏起來
            if (((Repeater)sender).Items.Count == 0)
            {
                ((Repeater)sender).Visible = false;
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_B11");
        }

        protected void btnToOtherPurch_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_B11");
        }

        

        private string getUnitName(string strUnit)
        {
            string query = gm.TBMDEPTs.Where(o => o.OVC_DEPT_CDE.Equals(strUnit)).Select(o => o.OVC_ONNAME).FirstOrDefault();
            return query;
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
                if (table1301.OVC_PERMISSION_UPDATE.Equals("N"))
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
                    if (table1301.IS_PLURAL_BASIS == "Y")
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
                if (!(table1301.OVC_PUR_AGENCY.Equals("S") || table1301.OVC_PUR_AGENCY.Equals("M")))
                {
                    var queryMoney =
                    (from t in mpms.TBM1201
                     where t.OVC_PURCH.Equals(strPurchNum)
                     select new
                     {
                         money = t.ONB_POI_QORDER_PLAN * t.ONB_POI_MPRICE_PLAN
                     }).Sum(o => o.money);

                    if (queryMoney == 0 || table1301.ONB_PUR_BUDGET == 0)
                    {
                        decimal numBudget = (decimal)table1301.ONB_PUR_BUDGET;
                        decimal numMoney = (decimal)queryMoney;
                        strMessage += "<p>預算總價與明細金額不得為0！</p>";
                        strMessage += "<p>預算金額為" + numBudget.ToString("#.00") + "</p>";
                        strMessage += "<p>明細總價為" + numMoney.ToString("#.00") + "</p>";
                    }
                    else
                    {
                        if (queryMoney != table1301.ONB_PUR_BUDGET)
                        {
                            decimal numBudget = (decimal)table1301.ONB_PUR_BUDGET;
                            decimal numMoney = (decimal)queryMoney;
                            strMessage += "<p>預算總價與明細金額不相符！</p>";
                            strMessage += "<p>預算金額為" + numBudget.ToString("#.00") + "</p>";
                            strMessage += "<p>明細總價為" + numMoney.ToString("#.00") + "</p>";
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
                        strMessage += "<p>【物資申請書之備註欄中】(8)政府協購協定項目不得空白！</p>";
                    if (queryM4Aee == null || queryM4Aff == null || queryM4Agg == null)
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