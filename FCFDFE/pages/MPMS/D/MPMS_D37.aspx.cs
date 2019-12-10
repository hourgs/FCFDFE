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
    public partial class MPMS_D37 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        public string sOVC_PURCH, sONB_GROUP;
        public string ssOVC_PURCH, ssOVC_PURCH_5;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.Controls_Attributes("readonly", "true", txtOVC_DBID, txtOVC_DOPEN, txtOVC_DCONTRACT);
                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_PURCH_5"] == null || Request.QueryString["ONB_GROUP"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    if (!IsPostBack && IsOVC_DO_NAME())
                    {
                        var query1407B0 =
                            (from tbm1407 in mpms.TBM1407
                             where tbm1407.OVC_PHR_CATE == "B0"
                             select tbm1407).ToList();
                        DataTable dtB0 = CommonStatic.ListToDataTable(query1407B0);
                        FCommon.list_dataImport(drpONB_GROUP_BUDGET, dtB0, "OVC_PHR_DESC", "OVC_PHR_ID", false);
                        FCommon.list_dataImport(drpONB_MCONTRACT, dtB0, "OVC_PHR_DESC", "OVC_PHR_ID", false);
                        drpONB_GROUP_BUDGET.SelectedItem.Text = "新臺幣";
                        drpONB_MCONTRACT.SelectedItem.Text = "新臺幣";
                        dataDefault();
                    }
                }
            }
        }

        private bool IsOVC_DO_NAME()
        {
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strUserName, strDept = "";
            string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
            string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
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

        private void dataDefault()
        {
            string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
            string strOVC_PURCH_5_url = Request.QueryString["OVC_PURCH_5"];
            string strONB_GROUP_url = Request.QueryString["ONB_GROUP"];
            string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
            string strOVC_PURCH_5 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_5_url));
            string strONB_GROUP = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strONB_GROUP_url));
            ViewState["strOVC_PURCH"] = strOVC_PURCH;
            ViewState["strOVC_PURCH_5"] = strOVC_PURCH_5;
            ViewState["ONB_GROUP"] = strONB_GROUP;
            int onbgroup = Convert.ToInt32(strONB_GROUP);
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            var query1302 =
                (from tbm1302 in mpms.TBM1302
                 where tbm1302.OVC_PURCH == strOVC_PURCH
                 where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbm1302.ONB_GROUP == onbgroup
                 select tbm1302).FirstOrDefault();
            if (query1302 == null)
            {
                foreach (Control item in Page.Form.FindControl("MainContent").Controls)
                {
                    if (item is TextBox)
                    {
                        ((TextBox)item).Text = string.Empty;
                    }
                }
                ViewState["type"] = "new";

            }
            lblInfo_PurchNo.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
            //合約商
            lblInfo_Group.Text = strONB_GROUP;
            lblOVC_PURCH.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
            lblOVC_PUR_IPURCH.Text = query1301.OVC_PUR_IPURCH;
            txtONB_GROUP.Text = strONB_GROUP;
            string str1301current = query1301.OVC_PUR_CURRENT;
            var query1407B01 =
                (from tbm1407 in mpms.TBM1407
                 where tbm1407.OVC_PHR_CATE == "B0"
                 where tbm1407.OVC_PHR_ID == str1301current
                 select tbm1407).FirstOrDefault();
            drpONB_GROUP_BUDGET.SelectedItem.Text = query1407B01.OVC_PHR_DESC;
            txtONB_PUR_BUDGET.Text = query1301.ONB_PUR_BUDGET.ToString();
            var query1303 =
                (from tbm1303 in mpms.TBM1303
                 where tbm1303.OVC_PURCH == strOVC_PURCH
                 where tbm1303.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbm1303.ONB_GROUP == onbgroup
                 select tbm1303).FirstOrDefault();
            txtOVC_DOPEN.Text = query1303.OVC_DOPEN;
            txtOVC_RECEIVE_PLACE.Text = "詳如清單備註8";
            txtOVC_SHIP_TIMES.Text = "詳如清單備註7";
            txtOVC_PAYMENT.Text = "詳如清單備註12";
            txtONB_DELIVERY_TIMES.Text = "1";
            if (query1302 != null)
            {
                dataImport(strOVC_PURCH, strOVC_PURCH_5, onbgroup);
                ViewState["type"] = "modify";
            }

            string strkey = strOVC_PURCH ;
            string strkey3 = strONB_GROUP + strOVC_PURCH_5;
            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strkey));
            string key3 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strkey3));

            sOVC_PURCH = key;
            sONB_GROUP = key3;
            string key1 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH));
            string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH_5));

            ssOVC_PURCH = key1;
            ssOVC_PURCH_5 = key2;

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strType = ViewState["type"].ToString();
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strONB_GROUP = ViewState["ONB_GROUP"].ToString();
            int onbgroup = Convert.ToInt32(strONB_GROUP);
            if (strType == "new")
            {
                newdata(strOVC_PURCH, strOVC_PURCH_5, onbgroup);
            }
            if (strType == "modify")
            {
                modifydata(strOVC_PURCH, strOVC_PURCH_5, onbgroup);
            }
        }
        private void newdata(string strOVC_PURCH, string strOVC_PURCH_5, int onbgroup)
        {
            string strOVC_PURCH_6 = txtOVC_PURCH_6.Text;
            string strONB_BUD_MOENY = txtONB_PUR_BUDGET.Text;
            string strONB_MONEY = txtONB_MCONTRACT.Text;
            string strONB_MONEY_DISCOUNT = txtONB_MONEY_DISCOUNT.Text;
            string strONB_DELIVERY_TIMES = txtONB_DELIVERY_TIMES.Text;
            string strONB_GROUP = txtONB_GROUP.Text;
            string strOVC_VEN_CST = txtOVC_VEN_CST.Text;
            string strMessage = "";
            #region
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            var queryyesno =
                (from tbm1302 in mpms.TBM1302
                 where tbm1302.OVC_PURCH == strOVC_PURCH
                 where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbm1302.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbm1302.ONB_GROUP == onbgroup
                 where tbm1302.OVC_VEN_CST == strOVC_VEN_CST
                 select tbm1302).FirstOrDefault();
            if (queryyesno != null)
                strMessage += "<P> 此購案契約已有資料 無法新增 </p>";
            if (strOVC_PURCH_6.Equals(string.Empty))
                strMessage += "<P> 契約編號欄位不得為空白 </p>";
            if (!strONB_GROUP.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_GROUP, out n))
                {

                }
                else
                {
                    strMessage += "<P> 組別請輸入數字 </p>";
                }
            }
            else
                strMessage += "<P>  組別欄位不得為空白 </p>";
            if (!strONB_BUD_MOENY.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_BUD_MOENY, out n))
                {

                }
                else
                {
                    strMessage += "<P> 預算金額請輸入數字 </p>";
                }
            }
            else
                strMessage += "<P>  預算金額欄位不得為空白 </p>";
            if (!strONB_MONEY.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_MONEY, out n))
                {

                }
                else
                {
                    strMessage += "<P> 契約金額請輸入數字 </p>";
                }
            }
            else
                strMessage += "<P>  契約金額欄位不得為空白 </p>";
            if (!strONB_MONEY_DISCOUNT.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_MONEY_DISCOUNT, out n))
                {

                }
                else
                {
                    strMessage += "<P> 折讓金額請輸入數字 </p>";
                }
            }
            if (!strONB_DELIVERY_TIMES.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_DELIVERY_TIMES, out n))
                {

                }
                else
                {
                    strMessage += "<P> 交貨批次請輸入數字 </p>";
                }
            }
            else
                strMessage += "<P>  交貨批次欄位不得為空白 </p>";
            #endregion
            if (strMessage.Equals(string.Empty))
            {
                short onb_group = Convert.ToInt16(strONB_GROUP);
                int onb_bud_money = Convert.ToInt32(strONB_BUD_MOENY);
                int onb_money = Convert.ToInt32(strONB_MONEY);
                short onb_delivery_times = Convert.ToInt16(strONB_DELIVERY_TIMES);

                TBM1302 tbm1302 = new TBM1302();
                tbm1302.OVC_PURCH = strOVC_PURCH;
                tbm1302.OVC_PURCH_5 = strOVC_PURCH_5;

                tbm1302.OVC_PURCH_6 = strOVC_PURCH_6;
                tbm1302.ONB_GROUP = onb_group;
                tbm1302.OVC_BUD_CURRENT = drpONB_GROUP_BUDGET.SelectedValue.ToString();
                tbm1302.ONB_BUD_MONEY = onb_bud_money;
                tbm1302.OVC_DBID = txtOVC_DBID.Text;
                tbm1302.OVC_DOPEN = txtOVC_DOPEN.Text;
                tbm1302.OVC_DCONTRACT = txtOVC_DCONTRACT.Text;
                tbm1302.OVC_CURRENT = drpONB_MCONTRACT.SelectedValue.ToString();
                tbm1302.ONB_MONEY = onb_money;
                if (strONB_MONEY_DISCOUNT != null)
                {
                    int onb_money_disocunt = Convert.ToInt32(strONB_MONEY_DISCOUNT);
                    tbm1302.ONB_MONEY_DISCOUNT = onb_money_disocunt;
                }
                tbm1302.OVC_RECEIVE_PLACE = txtOVC_RECEIVE_PLACE.Text;
                tbm1302.OVC_SHIP_TIMES = txtOVC_SHIP_TIMES.Text;
                tbm1302.OVC_PAYMENT = txtOVC_PAYMENT.Text;
                tbm1302.ONB_DELIVERY_TIMES = onb_delivery_times;
                tbm1302.OVC_VEN_CST = strOVC_VEN_CST;
                tbm1302.OVC_VEN_TITLE = txtOVC_VEN_TITLE.Text;
                tbm1302.OVC_VEN_FAX = txtOVC_VEN_FAX.Text;
                tbm1302.OVC_VEN_EMAIL = txtOVC_VEN_EMAIL.Text;
                tbm1302.OVC_VEN_BOSS = txtOVC_VEN_BOSS.Text;
                tbm1302.OVC_VEN_TEL = txtOVC_VEN_TEL.Text;
                tbm1302.OVC_VEN_NAME = txtOVC_VEN_NAME.Text;
                tbm1302.OVC_VEN_CELLPHONE = txtOVC_VEN_CELLPHONE.Text;
                tbm1302.OVC_VEN_ADDRESS = txtOVC_VEN_ADDRESS.Text;
                tbm1302.OVC_CONTRACT_COMM = txtOVC_CONTRACT_COMM.Text;
                tbm1302.OVC_NAME = Session["username"].ToString();

                mpms.TBM1302.Add(tbm1302);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1302.GetType().Name.ToString(), this, "新增");

                FCommon.AlertShow(PnMessage, "success", "系統訊息", "購案合約編號" + strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5 + strOVC_PURCH_6 + "新增成功!!");
            }
            FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }

        private void modifydata(string strOVC_PURCH, string strOVC_PURCH_5, int onbgroup)
        {
            string strOVC_PURCH_6 = txtOVC_PURCH_6.Text;
            string strONB_BUD_MOENY = txtONB_PUR_BUDGET.Text;
            string strONB_MONEY = txtONB_MCONTRACT.Text;
            string strONB_MONEY_DISCOUNT = txtONB_MONEY_DISCOUNT.Text;
            string strONB_DELIVERY_TIMES = txtONB_DELIVERY_TIMES.Text;
            string strOVC_VEN_CST = txtOVC_VEN_CST.Text;
            string strONB_GROUP = txtONB_GROUP.Text;
            string stronbgroup = onbgroup.ToString();
            string strMessage = "";

            #region
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            string strOVC_PURCH_6_O = ViewState["strOVC_PURCH_6"].ToString();
            string strOVC_VEN_CST_O = ViewState["strOVC_VEN_CST"].ToString();
            if (strOVC_VEN_CST != strOVC_VEN_CST_O || strOVC_PURCH_6 != strOVC_PURCH_6_O || stronbgroup != strONB_GROUP)
            {
                var queryyesno =
               (from tbm1302 in mpms.TBM1302
                where tbm1302.OVC_PURCH == strOVC_PURCH
                where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
                where tbm1302.OVC_PURCH_6 == strOVC_PURCH_6
                where tbm1302.OVC_VEN_CST == strOVC_VEN_CST
                where tbm1302.ONB_GROUP == onbgroup
                select tbm1302).FirstOrDefault();
                if (queryyesno != null)
                    strMessage += "<P> 此購案契約已有資料 無法修改 </p>";
            }

            if (strOVC_PURCH_6.Equals(string.Empty))
                strMessage += "<P> 契約編號欄位不得為空白 </p>";

            if (strOVC_PURCH_6.Equals(string.Empty))
                strMessage += "<P> 契約編號欄位不得為空白 </p>";
            if (!strONB_GROUP.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_GROUP, out n))
                {

                }
                else
                {
                    strMessage += "<P> 組別請輸入數字 </p>";
                }
            }
            else
                strMessage += "<P>  組別欄位不得為空白 </p>";
            if (!strONB_BUD_MOENY.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_BUD_MOENY, out n))
                {

                }
                else
                {
                    strMessage += "<P> 預算金額請輸入數字 </p>";
                }
            }
            else
                strMessage += "<P>  預算金額欄位不得為空白 </p>";
            if (!strONB_MONEY.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_MONEY, out n))
                {

                }
                else
                {
                    strMessage += "<P> 契約金額請輸入數字 </p>";
                }
            }
            else
                strMessage += "<P>  契約金額欄位不得為空白 </p>";
            if (!strONB_MONEY_DISCOUNT.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_MONEY_DISCOUNT, out n))
                {

                }
                else
                {
                    strMessage += "<P> 折讓金額請輸入數字 </p>";
                }
            }
            if (!strONB_DELIVERY_TIMES.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_DELIVERY_TIMES, out n))
                {

                }
                else
                {
                    strMessage += "<P> 交貨批次請輸入數字 </p>";
                }
            }
            else
                strMessage += "<P>  交貨批次欄位不得為空白 </p>";
            #endregion
            if (strMessage.Equals(string.Empty))
            {
                short onb_group = Convert.ToInt16(strONB_GROUP);
                int onb_bud_money = Convert.ToInt32(strONB_BUD_MOENY);
                int onb_money = Convert.ToInt32(strONB_MONEY);
                short onb_delivery_times = Convert.ToInt16(strONB_DELIVERY_TIMES);

                var tbm1302 =
                (from t1302 in mpms.TBM1302
                 where t1302.OVC_PURCH == strOVC_PURCH
                 where t1302.OVC_PURCH_5 == strOVC_PURCH_5
                 where t1302.OVC_PURCH_6 == strOVC_PURCH_6_O
                 where t1302.ONB_GROUP == onbgroup
                 where t1302.OVC_VEN_CST == strOVC_VEN_CST_O
                 select t1302).FirstOrDefault();

                tbm1302.OVC_PURCH_6 = strOVC_PURCH_6;
                tbm1302.ONB_GROUP = onb_group;
                tbm1302.OVC_BUD_CURRENT = drpONB_GROUP_BUDGET.SelectedValue.ToString();
                tbm1302.ONB_BUD_MONEY = onb_bud_money;
                tbm1302.OVC_DBID = txtOVC_DBID.Text;
                tbm1302.OVC_DOPEN = txtOVC_DOPEN.Text;
                tbm1302.OVC_DCONTRACT = txtOVC_DCONTRACT.Text;
                tbm1302.OVC_CURRENT = drpONB_MCONTRACT.SelectedValue.ToString();
                tbm1302.ONB_MONEY = onb_money;
                if (strONB_MONEY_DISCOUNT != null)
                {
                    int onb_money_disocunt = Convert.ToInt32(strONB_MONEY_DISCOUNT);
                    tbm1302.ONB_MONEY_DISCOUNT = onb_money_disocunt;
                }
                tbm1302.OVC_RECEIVE_PLACE = txtOVC_RECEIVE_PLACE.Text;
                tbm1302.OVC_SHIP_TIMES = txtOVC_SHIP_TIMES.Text;
                tbm1302.OVC_PAYMENT = txtOVC_PAYMENT.Text;
                tbm1302.ONB_DELIVERY_TIMES = onb_delivery_times;
                tbm1302.OVC_VEN_CST = strOVC_VEN_CST;
                tbm1302.OVC_VEN_TITLE = txtOVC_VEN_TITLE.Text;
                tbm1302.OVC_VEN_FAX = txtOVC_VEN_FAX.Text;
                tbm1302.OVC_VEN_EMAIL = txtOVC_VEN_EMAIL.Text;
                tbm1302.OVC_VEN_BOSS = txtOVC_VEN_BOSS.Text;
                tbm1302.OVC_VEN_TEL = txtOVC_VEN_TEL.Text;
                tbm1302.OVC_VEN_NAME = txtOVC_VEN_NAME.Text;
                tbm1302.OVC_VEN_CELLPHONE = txtOVC_VEN_CELLPHONE.Text;
                tbm1302.OVC_VEN_ADDRESS = txtOVC_VEN_ADDRESS.Text;
                tbm1302.OVC_CONTRACT_COMM = txtOVC_CONTRACT_COMM.Text;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm1302.GetType().Name.ToString(), this, "修改");

                FCommon.AlertShow(PnMessage, "success", "系統訊息", "購案合約編號" + strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5 + strOVC_PURCH_6 + "修改成功!!");
            }
            FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH"].ToString()));
            string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH_5"].ToString()));
            string key3 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["ONB_GROUP"].ToString()));
            Response.Redirect("MPMS_D37.aspx?OVC_PURCH=" + key + "&OVC_PURCH_5=" + key2 + "&ONB_GROUP=" + key3);
        }

        protected void btnDetail_Click(object sender, EventArgs e)
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strONB_GROUP = ViewState["ONB_GROUP"].ToString();
            short onbgroup = Convert.ToInt16(strONB_GROUP);
            var tbm1302 =
                (from t1302 in mpms.TBM1302
                 where t1302.OVC_PURCH == strOVC_PURCH
                 where t1302.OVC_PURCH_5 == strOVC_PURCH_5
                 where t1302.ONB_GROUP == onbgroup
                 select t1302).FirstOrDefault();
            if (tbm1302 != null)
            {
                string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH"].ToString()));
                string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH_5"].ToString()));
                string key3 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["ONB_GROUP"].ToString()));
                Response.Redirect("MPMS_D38.aspx?OVC_PURCH=" + key + "&OVC_PURCH_5=" + key2 + "&ONB_GROUP=" + key3);
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先製作契約草稿");
            }
            
        }

        protected void btnPrice_Click(object sender, EventArgs e)
        {
            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH"].ToString()));
            string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH_5"].ToString()));
            string key3 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["ONB_GROUP"].ToString()));
            Response.Redirect("MPMS_D39.aspx?OVC_PURCH=" + key + "&OVC_PURCH_5=" + key2 + "&ONB_GROUP=" + key3);
        }

        protected void btnReturnS_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_D36.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString());
        }

        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_D14_1.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString());
        }

        private void dataImport(string strOVC_PURCH, string strOVC_PURCH_5, int strONB_GROUP)
        {
            FCommon.Controls_Attributes("readonly", "true", txtOVC_PURCH_6,txtONB_GROUP,txtOVC_VEN_CST);
            var query1302 =
                (from tbm1302 in mpms.TBM1302
                 where tbm1302.OVC_PURCH == strOVC_PURCH
                 where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbm1302.ONB_GROUP == strONB_GROUP
                 select tbm1302).FirstOrDefault();
            string str1302current="", strcurrent="";
            if (query1302 != null)
            {
                lblInfo_Contractor.Text = query1302.OVC_VEN_TITLE;
                txtOVC_PURCH_6.Text = query1302.OVC_PURCH_6;
                ViewState["strOVC_PURCH_6"] = query1302.OVC_PURCH_6;

                txtONB_GROUP.Text = query1302.ONB_GROUP.ToString();
                str1302current = query1302.OVC_BUD_CURRENT;

                txtONB_PUR_BUDGET.Text = query1302.ONB_BUD_MONEY.ToString();
                txtOVC_DBID.Text = query1302.OVC_DBID;
                txtOVC_DOPEN.Text = query1302.OVC_DOPEN;
                txtOVC_DCONTRACT.Text = query1302.OVC_DCONTRACT;
                strcurrent = query1302.OVC_CURRENT;

                txtONB_MCONTRACT.Text = query1302.ONB_MONEY.ToString();
                txtONB_MONEY_DISCOUNT.Text = query1302.ONB_MONEY_DISCOUNT.ToString();
                txtOVC_RECEIVE_PLACE.Text = query1302.OVC_RECEIVE_PLACE;
                txtOVC_SHIP_TIMES.Text = query1302.OVC_SHIP_TIMES;
                txtOVC_PAYMENT.Text = query1302.OVC_PAYMENT;
                txtONB_DELIVERY_TIMES.Text = query1302.ONB_DELIVERY_TIMES.ToString();
                txtOVC_VEN_CST.Text = query1302.OVC_VEN_CST;
                ViewState["strOVC_VEN_CST"] = query1302.OVC_VEN_CST;
                txtOVC_VEN_TITLE.Text = query1302.OVC_VEN_TITLE;
                txtOVC_VEN_FAX.Text = query1302.OVC_VEN_FAX;
                txtOVC_VEN_EMAIL.Text = query1302.OVC_VEN_EMAIL;
                txtOVC_VEN_BOSS.Text = query1302.OVC_VEN_BOSS;
                txtOVC_VEN_TEL.Text = query1302.OVC_VEN_TEL;
                txtOVC_VEN_NAME.Text = query1302.OVC_VEN_NAME;
                txtOVC_VEN_CELLPHONE.Text = query1302.OVC_VEN_CELLPHONE;
                txtOVC_VEN_ADDRESS.Text = query1302.OVC_VEN_ADDRESS;
                txtOVC_CONTRACT_COMM.Text = query1302.OVC_CONTRACT_COMM;
            }

            var query1407B01 =
                (from tbm1407 in mpms.TBM1407
                 where tbm1407.OVC_PHR_CATE == "B0"
                 where tbm1407.OVC_PHR_ID == str1302current
                 select tbm1407).FirstOrDefault();
            drpONB_GROUP_BUDGET.SelectedItem.Text = query1407B01 == null ? "" : query1407B01.OVC_PHR_DESC;
            var query1407B012 =
                (from tbm1407 in mpms.TBM1407
                 where tbm1407.OVC_PHR_CATE == "B0"
                 where tbm1407.OVC_PHR_ID == strcurrent
                 select tbm1407).FirstOrDefault();
            drpONB_MCONTRACT.SelectedItem.Text = query1407B012 == null ? "" : query1407B012.OVC_PHR_DESC;           
        }
    }
}