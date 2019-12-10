using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;
using System.IO;
using Xceed.Words.NET;
using System.Web;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D22 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string strUserName;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_PURCH_5"] == null || Request.QueryString["OVC_PURCH_6"] == null || Request.QueryString["ONB_GROUP"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    if (!IsPostBack && IsOVC_DO_NAME())
                    {
                        FCommon.Controls_Attributes("readonly", "true", txtOVC_DAPPROVE);
                        string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
                        string strOVC_PURCH_5_url = Request.QueryString["OVC_PURCH_5"];
                        string strOVC_PURCH_6_url = Request.QueryString["OVC_PURCH_6"];
                        string strONB_GROUP_url = Request.QueryString["ONB_GROUP"];
                        string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
                        string strOVC_PURCH_5 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_5_url));
                        string strOVC_PURCH_6 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_6_url));
                        string strONB_GROUP = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strONB_GROUP_url));
                        //string strOVC_PURCH_6;
                        int onbgroup = Convert.ToInt32(strONB_GROUP);
                        ViewState["strOVC_PURCH"] = strOVC_PURCH;
                        ViewState["strOVC_PURCH_5"] = strOVC_PURCH_5;
                        ViewState["strOVC_PURCH_6"] = strOVC_PURCH_6;
                        ViewState["strONB_GROUP"] = strONB_GROUP;
                        var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
                        var query1302 =
                        (from tbm1302 in mpms.TBM1302
                         where tbm1302.OVC_PURCH == strOVC_PURCH
                         where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
                         where tbm1302.OVC_PURCH_6 == strOVC_PURCH_6
                         where tbm1302.ONB_GROUP == onbgroup
                         select tbm1302).FirstOrDefault();
                        //strOVC_PURCH_6 = query1302 == null ? "" : query1302.OVC_PURCH_6;
                        //ViewState["strOVC_PURCH_6"] = strOVC_PURCH_6;
                        lblOVC_PURCH.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5 + strOVC_PURCH_6;
                        lblOVC_VEN_TITLE.Text = query1302 == null ? "" : query1302.OVC_VEN_TITLE;
                        lblONB_GROUP.Text = strONB_GROUP;
                        
                        dataImport(strOVC_PURCH, strOVC_PURCH_5, strOVC_PURCH_6, onbgroup);
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

        private void dataImport(string strOVC_PURCH, string strOVC_PURCH_5, string strOVC_PURCH_6, int onbgroup)
        {
            var queryMAKE_ITEM_1 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 1
                 select tbmmake).FirstOrDefault();
            var queryMAKE_ITEM_2 =
                (from tbmmake2 in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake2.OVC_PURCH == strOVC_PURCH
                 where tbmmake2.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake2.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake2.ONB_GROUP == onbgroup
                 where tbmmake2.ONB_ITEM == 2
                 select tbmmake2).FirstOrDefault();
            var queryMAKE_ITEM_3 =
                (from tbmmake3 in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake3.OVC_PURCH == strOVC_PURCH
                 where tbmmake3.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake3.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake3.ONB_GROUP == onbgroup
                 where tbmmake3.ONB_ITEM == 3
                 select tbmmake3).FirstOrDefault();
            var queryMAKE_ITEM_4 =
                (from tbmmake4 in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake4.OVC_PURCH == strOVC_PURCH
                 where tbmmake4.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake4.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake4.ONB_GROUP == onbgroup
                 where tbmmake4.ONB_ITEM == 4
                 select tbmmake4).FirstOrDefault();
            var queryMAKE_ITEM_5 =
                (from tbmmake5 in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake5.OVC_PURCH == strOVC_PURCH
                 where tbmmake5.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake5.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake5.ONB_GROUP == onbgroup
                 where tbmmake5.ONB_ITEM == 5
                 select tbmmake5).FirstOrDefault();
            var queryMAKE_ITEM_6 =
                (from tbmmake6 in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake6.OVC_PURCH == strOVC_PURCH
                 where tbmmake6.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake6.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake6.ONB_GROUP == onbgroup
                 where tbmmake6.ONB_ITEM == 6
                 select tbmmake6).FirstOrDefault();
            var queryMAKE_ITEM_7 =
                (from tbmmake7 in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake7.OVC_PURCH == strOVC_PURCH
                 where tbmmake7.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake7.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake7.ONB_GROUP == onbgroup
                 where tbmmake7.ONB_ITEM == 7
                 select tbmmake7).FirstOrDefault();
            var queryMAKE_ITEM_8 =
                (from tbmmake8 in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake8.OVC_PURCH == strOVC_PURCH
                 where tbmmake8.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake8.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake8.ONB_GROUP == onbgroup
                 where tbmmake8.ONB_ITEM == 8
                 select tbmmake8).FirstOrDefault();
            var queryMAKE_ITEM_9 =
                (from tbmmake9 in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake9.OVC_PURCH == strOVC_PURCH
                 where tbmmake9.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake9.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake9.ONB_GROUP == onbgroup
                 where tbmmake9.ONB_ITEM == 9
                 select tbmmake9).FirstOrDefault();
            var queryMAKE_ITEM_10 =
                (from tbmmake10 in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake10.OVC_PURCH == strOVC_PURCH
                 where tbmmake10.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake10.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake10.ONB_GROUP == onbgroup
                 where tbmmake10.ONB_ITEM == 10
                 select tbmmake10).FirstOrDefault();
            var queryMAKE_ITEM_11 =
                (from tbmmake11 in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake11.OVC_PURCH == strOVC_PURCH
                 where tbmmake11.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake11.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake11.ONB_GROUP == onbgroup
                 where tbmmake11.ONB_ITEM == 11
                 select tbmmake11).FirstOrDefault();
            var queryMAKE_ITEM_12 =
                (from tbmmake12 in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake12.OVC_PURCH == strOVC_PURCH
                 where tbmmake12.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake12.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake12.ONB_GROUP == onbgroup
                 where tbmmake12.ONB_ITEM == 12
                 select tbmmake12).FirstOrDefault();
            var queryMAKE_ITEM_13 =
                (from tbmmake13 in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake13.OVC_PURCH == strOVC_PURCH
                 where tbmmake13.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake13.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake13.ONB_GROUP == onbgroup
                 where tbmmake13.ONB_ITEM == 13
                 select tbmmake13).FirstOrDefault();
            var queryMAKE_ITEM_14 =
                (from tbmmake14 in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake14.OVC_PURCH == strOVC_PURCH
                 where tbmmake14.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake14.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake14.ONB_GROUP == onbgroup
                 where tbmmake14.ONB_ITEM == 14
                 select tbmmake14).FirstOrDefault();
            var queryMAKE_ITEM_15 =
                (from tbmmake15 in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake15.OVC_PURCH == strOVC_PURCH
                 where tbmmake15.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake15.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake15.ONB_GROUP == onbgroup
                 where tbmmake15.ONB_ITEM == 15
                 select tbmmake15).FirstOrDefault();
            var queryMAKE_ITEM_16 =
                (from tbmmake16 in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake16.OVC_PURCH == strOVC_PURCH
                 where tbmmake16.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake16.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake16.ONB_GROUP == onbgroup
                 where tbmmake16.ONB_ITEM == 16
                 select tbmmake16).FirstOrDefault();
            var queryMAKE_ITEM_17 =
                (from tbmmake17 in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake17.OVC_PURCH == strOVC_PURCH
                 where tbmmake17.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake17.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake17.ONB_GROUP == onbgroup
                 where tbmmake17.ONB_ITEM == 17
                 select tbmmake17).FirstOrDefault();
            var queryMAKE_ITEM_18 =
                (from tbmmake18 in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake18.OVC_PURCH == strOVC_PURCH
                 where tbmmake18.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake18.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake18.ONB_GROUP == onbgroup
                 where tbmmake18.ONB_ITEM == 18
                 select tbmmake18).FirstOrDefault();


            if (queryMAKE_ITEM_1 != null)
                rdoCheck1.SelectedValue = queryMAKE_ITEM_1.OVC_RESULT;
            if (queryMAKE_ITEM_2 != null)
                rdoCheck2.SelectedValue = queryMAKE_ITEM_2.OVC_RESULT;
            if (queryMAKE_ITEM_3 != null)
                rdoCheck3.SelectedValue = queryMAKE_ITEM_3.OVC_RESULT;
            if (queryMAKE_ITEM_4 != null)
            {
                rdoCheck4.SelectedValue = queryMAKE_ITEM_4.OVC_RESULT;
                txtQ4.Text = queryMAKE_ITEM_4.OVC_ITEM_NAME;
            }
            if (queryMAKE_ITEM_5 != null)
                rdoCheck5.SelectedValue = queryMAKE_ITEM_5.OVC_RESULT;
            if (queryMAKE_ITEM_6 != null)
                rdoCheck6.SelectedValue = queryMAKE_ITEM_6.OVC_RESULT;
            if (queryMAKE_ITEM_7 != null)
                rdoCheck7.SelectedValue = queryMAKE_ITEM_7.OVC_RESULT;
            if (queryMAKE_ITEM_8 != null)
                rdoCheck8.SelectedValue = queryMAKE_ITEM_8.OVC_RESULT;
            if (queryMAKE_ITEM_9 != null)
                rdoCheck9.SelectedValue = queryMAKE_ITEM_9.OVC_RESULT;
            if (queryMAKE_ITEM_10 != null)
                rdoCheck10.SelectedValue = queryMAKE_ITEM_10.OVC_RESULT;
            if (queryMAKE_ITEM_11 != null)
                rdoCheck11.SelectedValue = queryMAKE_ITEM_11.OVC_RESULT;
            if (queryMAKE_ITEM_12 != null)
                rdoCheck12.SelectedValue = queryMAKE_ITEM_12.OVC_RESULT;
            if (queryMAKE_ITEM_13 != null)
            {
                rdoCheck13.SelectedValue = queryMAKE_ITEM_13.OVC_RESULT;
                txtQ13.Text = queryMAKE_ITEM_13.OVC_ITEM_NAME;
            }
            if (queryMAKE_ITEM_14 != null)
                rdoCheck14.SelectedValue = queryMAKE_ITEM_14.OVC_RESULT;
            if (queryMAKE_ITEM_15 != null)
                rdoCheck15.SelectedValue = queryMAKE_ITEM_15.OVC_RESULT;
            if (queryMAKE_ITEM_16 != null)
                rdoCheck16.SelectedValue = queryMAKE_ITEM_16.OVC_RESULT;
            if (queryMAKE_ITEM_17 != null)
                rdoCheck17.SelectedValue = queryMAKE_ITEM_17.OVC_RESULT;
            if (queryMAKE_ITEM_18 != null)
                rdoCheck18.SelectedValue = queryMAKE_ITEM_18.OVC_RESULT;

            var query =
                (from tbmcm in mpms.TBMCONTRACT_MAKE
                 where tbmcm.OVC_PURCH == strOVC_PURCH
                 where tbmcm.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmcm.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmcm.ONB_GROUP == onbgroup
                 select tbmcm).FirstOrDefault();
            if (query != null)
            {
                 rdoOVC_DO_NAME_RESULT.SelectedValue = query.OVC_RESULT;
                string strtype = query.OVC_RESULT;
                if (strtype == "其他")
                    TextBox1.Text = query.OVC_RESULT_OTHER;
                txtOVC_DAPPROVE.Text = query.OVC_DAPPROVE;
               
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            dataSave();
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_D1D.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"] + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"]);
        }

        protected void drpQ13_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtQ13.Text = drpQ13.SelectedItem.Text;
        }

        protected void drpQ4_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtQ4.Text = drpQ4.SelectedItem.Text;
        }

        protected void cblQ4_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtQ4.Text = "";
            for (int i = 0; i < 4; i++)
            {
                if (cblQ4.Items[i].Selected == true)
                {
                    if (txtQ4.Text != "")
                        txtQ4.Text += "、";
                    txtQ4.Text += cblQ4.Items[i].Text;
                }
            }
        }

        protected void cblQ13_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtQ13.Text = "";
            for (int i = 0; i < 4; i++)
            {
                if (cblQ13.Items[i].Selected == true)
                {
                    if (txtQ13.Text != "")
                        txtQ13.Text += "、";
                    txtQ13.Text += cblQ13.Items[i].Text;
                }
            }
        }

        private void dataSave()
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strOVC_PURCH_6 = ViewState["strOVC_PURCH_6"].ToString();
            string strONB_GROUP = ViewState["strONB_GROUP"].ToString();
            short onbgroup = Convert.ToInt16(strONB_GROUP);

            if (rdoCheck1.SelectedIndex != -1)
            {
                var queryMAKE_ITEM_1 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 1
                 select tbmmake).FirstOrDefault();
                if (queryMAKE_ITEM_1 == null)
                {
                    TBMCONTRACT_MAKE_ITEM make = new TBMCONTRACT_MAKE_ITEM();
                    make.OVC_PURCH = strOVC_PURCH;
                    make.OVC_PURCH_5 = strOVC_PURCH_5;
                    make.OVC_PURCH_6 = strOVC_PURCH_6;
                    make.ONB_GROUP = onbgroup;

                    make.ONB_ITEM = 1;
                    make.OVC_ITEM_NAME = lblQ1.Text;
                    make.OVC_RESULT = rdoCheck1.SelectedItem.Text;
                    mpms.TBMCONTRACT_MAKE_ITEM.Add(make);
                    mpms.SaveChanges();
                }
                else
                {
                    if (queryMAKE_ITEM_1.OVC_RESULT != rdoCheck1.SelectedItem.Text)
                    {
                        queryMAKE_ITEM_1.OVC_RESULT = rdoCheck1.SelectedItem.Text;
                        mpms.SaveChanges();
                    }
                }
            }
            if (rdoCheck2.SelectedIndex != -1)
            {
                var queryMAKE_ITEM_2 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 2
                 select tbmmake).FirstOrDefault();
                if (queryMAKE_ITEM_2 == null)
                {
                    TBMCONTRACT_MAKE_ITEM make = new TBMCONTRACT_MAKE_ITEM();
                    make.OVC_PURCH = strOVC_PURCH;
                    make.OVC_PURCH_5 = strOVC_PURCH_5;
                    make.OVC_PURCH_6 = strOVC_PURCH_6;
                    make.ONB_GROUP = onbgroup;

                    make.ONB_ITEM = 2;
                    make.OVC_ITEM_NAME = lblQ2.Text;
                    make.OVC_RESULT = rdoCheck2.SelectedItem.Text;
                    mpms.TBMCONTRACT_MAKE_ITEM.Add(make);
                    mpms.SaveChanges();
                }
                else
                {
                    if (queryMAKE_ITEM_2.OVC_RESULT != rdoCheck2.SelectedItem.Text)
                    {
                        queryMAKE_ITEM_2.OVC_RESULT = rdoCheck2.SelectedItem.Text;
                        mpms.SaveChanges();
                    }
                }
            }
            if (rdoCheck3.SelectedIndex != -1)
            {
                var queryMAKE_ITEM_3 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 3
                 select tbmmake).FirstOrDefault();
                if (queryMAKE_ITEM_3 == null)
                {
                    TBMCONTRACT_MAKE_ITEM make = new TBMCONTRACT_MAKE_ITEM();
                    make.OVC_PURCH = strOVC_PURCH;
                    make.OVC_PURCH_5 = strOVC_PURCH_5;
                    make.OVC_PURCH_6 = strOVC_PURCH_6;
                    make.ONB_GROUP = onbgroup;

                    make.ONB_ITEM = 3;
                    make.OVC_ITEM_NAME = lblQ1.Text;
                    make.OVC_RESULT = rdoCheck3.SelectedItem.Text;
                    mpms.TBMCONTRACT_MAKE_ITEM.Add(make);
                    mpms.SaveChanges();
                }
                else
                {
                    if (queryMAKE_ITEM_3.OVC_RESULT != rdoCheck3.SelectedItem.Text)
                    {
                        queryMAKE_ITEM_3.OVC_RESULT = rdoCheck3.SelectedItem.Text;
                        mpms.SaveChanges();
                    }
                }
            }
            if (rdoCheck4.SelectedIndex != -1)
            {
                var queryMAKE_ITEM_4 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 4
                 select tbmmake).FirstOrDefault();
                if (queryMAKE_ITEM_4 == null)
                {
                    TBMCONTRACT_MAKE_ITEM make = new TBMCONTRACT_MAKE_ITEM();
                    make.OVC_PURCH = strOVC_PURCH;
                    make.OVC_PURCH_5 = strOVC_PURCH_5;
                    make.OVC_PURCH_6 = strOVC_PURCH_6;
                    make.ONB_GROUP = onbgroup;

                    make.ONB_ITEM = 4;
                    make.OVC_ITEM_NAME = txtQ4.Text;
                    make.OVC_RESULT = rdoCheck4.SelectedItem.Text;
                    mpms.TBMCONTRACT_MAKE_ITEM.Add(make);
                    mpms.SaveChanges();
                }
                else
                {
                    if (queryMAKE_ITEM_4.OVC_RESULT != rdoCheck4.SelectedItem.Text)
                    {
                        queryMAKE_ITEM_4.OVC_ITEM_NAME = txtQ4.Text;
                        queryMAKE_ITEM_4.OVC_RESULT = rdoCheck4.SelectedItem.Text;
                        mpms.SaveChanges();
                    }
                }
            }
            if (rdoCheck1.SelectedIndex != -1)
            {
                var queryMAKE_ITEM_5 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 5
                 select tbmmake).FirstOrDefault();
                if (queryMAKE_ITEM_5 == null)
                {
                    TBMCONTRACT_MAKE_ITEM make = new TBMCONTRACT_MAKE_ITEM();
                    make.OVC_PURCH = strOVC_PURCH;
                    make.OVC_PURCH_5 = strOVC_PURCH_5;
                    make.OVC_PURCH_6 = strOVC_PURCH_6;
                    make.ONB_GROUP = onbgroup;

                    make.ONB_ITEM = 5;
                    make.OVC_ITEM_NAME = lblQ5.Text;
                    make.OVC_RESULT = rdoCheck5.SelectedItem.Text;
                    mpms.TBMCONTRACT_MAKE_ITEM.Add(make);
                    mpms.SaveChanges();
                }
                else
                {
                    if (queryMAKE_ITEM_5.OVC_RESULT != rdoCheck5.SelectedItem.Text)
                    {
                        queryMAKE_ITEM_5.OVC_RESULT = rdoCheck5.SelectedItem.Text;
                        mpms.SaveChanges();
                    }
                }
            }
            if (rdoCheck6.SelectedIndex != -1)
            {
                var queryMAKE_ITEM_6 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 6
                 select tbmmake).FirstOrDefault();
                if (queryMAKE_ITEM_6 == null)
                {
                    TBMCONTRACT_MAKE_ITEM make = new TBMCONTRACT_MAKE_ITEM();
                    make.OVC_PURCH = strOVC_PURCH;
                    make.OVC_PURCH_5 = strOVC_PURCH_5;
                    make.OVC_PURCH_6 = strOVC_PURCH_6;
                    make.ONB_GROUP = onbgroup;

                    make.ONB_ITEM = 6;
                    make.OVC_ITEM_NAME = lblQ6.Text;
                    make.OVC_RESULT = rdoCheck6.SelectedItem.Text;
                    mpms.TBMCONTRACT_MAKE_ITEM.Add(make);
                    mpms.SaveChanges();
                }
                else
                {
                    if (queryMAKE_ITEM_6.OVC_RESULT != rdoCheck6.SelectedItem.Text)
                    {
                        queryMAKE_ITEM_6.OVC_RESULT = rdoCheck6.SelectedItem.Text;
                        mpms.SaveChanges();
                    }
                }
            }
            if (rdoCheck7.SelectedIndex != -1)
            {
                var queryMAKE_ITEM_7 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 7
                 select tbmmake).FirstOrDefault();
                if (queryMAKE_ITEM_7 == null)
                {
                    TBMCONTRACT_MAKE_ITEM make = new TBMCONTRACT_MAKE_ITEM();
                    make.OVC_PURCH = strOVC_PURCH;
                    make.OVC_PURCH_5 = strOVC_PURCH_5;
                    make.OVC_PURCH_6 = strOVC_PURCH_6;
                    make.ONB_GROUP = onbgroup;

                    make.ONB_ITEM = 7;
                    make.OVC_ITEM_NAME = lblQ7.Text;
                    make.OVC_RESULT = rdoCheck7.SelectedItem.Text;
                    mpms.TBMCONTRACT_MAKE_ITEM.Add(make);
                    mpms.SaveChanges();
                }
                else
                {
                    if (queryMAKE_ITEM_7.OVC_RESULT != rdoCheck7.SelectedItem.Text)
                    {
                        queryMAKE_ITEM_7.OVC_RESULT = rdoCheck7.SelectedItem.Text;
                        mpms.SaveChanges();
                    }
                }
            }
            if (rdoCheck8.SelectedIndex != -1)
            {
                var queryMAKE_ITEM_8 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 8
                 select tbmmake).FirstOrDefault();
                if (queryMAKE_ITEM_8 == null)
                {
                    TBMCONTRACT_MAKE_ITEM make = new TBMCONTRACT_MAKE_ITEM();
                    make.OVC_PURCH = strOVC_PURCH;
                    make.OVC_PURCH_5 = strOVC_PURCH_5;
                    make.OVC_PURCH_6 = strOVC_PURCH_6;
                    make.ONB_GROUP = onbgroup;

                    make.ONB_ITEM = 8;
                    make.OVC_ITEM_NAME = lblQ8.Text;
                    make.OVC_RESULT = rdoCheck8.SelectedItem.Text;
                    mpms.TBMCONTRACT_MAKE_ITEM.Add(make);
                    mpms.SaveChanges();
                }
                else
                {
                    if (queryMAKE_ITEM_8.OVC_RESULT != rdoCheck8.SelectedItem.Text)
                    {
                        queryMAKE_ITEM_8.OVC_RESULT = rdoCheck8.SelectedItem.Text;
                        mpms.SaveChanges();
                    }
                }
            }
            if (rdoCheck9.SelectedIndex != -1)
            {
                var queryMAKE_ITEM_9 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 9
                 select tbmmake).FirstOrDefault();
                if (queryMAKE_ITEM_9 == null)
                {
                    TBMCONTRACT_MAKE_ITEM make = new TBMCONTRACT_MAKE_ITEM();
                    make.OVC_PURCH = strOVC_PURCH;
                    make.OVC_PURCH_5 = strOVC_PURCH_5;
                    make.OVC_PURCH_6 = strOVC_PURCH_6;
                    make.ONB_GROUP = onbgroup;

                    make.ONB_ITEM = 9;
                    make.OVC_ITEM_NAME = lblQ9.Text;
                    make.OVC_RESULT = rdoCheck9.SelectedItem.Text;
                    mpms.TBMCONTRACT_MAKE_ITEM.Add(make);
                    mpms.SaveChanges();
                }
                else
                {
                    if (queryMAKE_ITEM_9.OVC_RESULT != rdoCheck9.SelectedItem.Text)
                    {
                        queryMAKE_ITEM_9.OVC_RESULT = rdoCheck9.SelectedItem.Text;
                        mpms.SaveChanges();
                    }
                }
            }
            if (rdoCheck10.SelectedIndex != -1)
            {
                var queryMAKE_ITEM_10 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 10
                 select tbmmake).FirstOrDefault();
                if (queryMAKE_ITEM_10 == null)
                {
                    TBMCONTRACT_MAKE_ITEM make = new TBMCONTRACT_MAKE_ITEM();
                    make.OVC_PURCH = strOVC_PURCH;
                    make.OVC_PURCH_5 = strOVC_PURCH_5;
                    make.OVC_PURCH_6 = strOVC_PURCH_6;
                    make.ONB_GROUP = onbgroup;

                    make.ONB_ITEM = 10;
                    make.OVC_ITEM_NAME = lblQ10.Text;
                    make.OVC_RESULT = rdoCheck10.SelectedItem.Text;
                    mpms.TBMCONTRACT_MAKE_ITEM.Add(make);
                    mpms.SaveChanges();
                }
                else
                {
                    if (queryMAKE_ITEM_10.OVC_RESULT != rdoCheck10.SelectedItem.Text)
                    {
                        queryMAKE_ITEM_10.OVC_RESULT = rdoCheck10.SelectedItem.Text;
                        mpms.SaveChanges();
                    }
                }
            }
            if (rdoCheck11.SelectedIndex != -1)
            {
                var queryMAKE_ITEM_11 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 11
                 select tbmmake).FirstOrDefault();
                if (queryMAKE_ITEM_11 == null)
                {
                    TBMCONTRACT_MAKE_ITEM make = new TBMCONTRACT_MAKE_ITEM();
                    make.OVC_PURCH = strOVC_PURCH;
                    make.OVC_PURCH_5 = strOVC_PURCH_5;
                    make.OVC_PURCH_6 = strOVC_PURCH_6;
                    make.ONB_GROUP = onbgroup;

                    make.ONB_ITEM = 11;
                    make.OVC_ITEM_NAME = lblQ11.Text;
                    make.OVC_RESULT = rdoCheck11.SelectedItem.Text;
                    mpms.TBMCONTRACT_MAKE_ITEM.Add(make);
                    mpms.SaveChanges();
                }
                else
                {
                    if (queryMAKE_ITEM_11.OVC_RESULT != rdoCheck11.SelectedItem.Text)
                    {
                        queryMAKE_ITEM_11.OVC_RESULT = rdoCheck11.SelectedItem.Text;
                        mpms.SaveChanges();
                    }
                }
            }
            if (rdoCheck1.SelectedIndex != -1)
            {
                var queryMAKE_ITEM_12 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 12
                 select tbmmake).FirstOrDefault();
                if (queryMAKE_ITEM_12 == null)
                {
                    TBMCONTRACT_MAKE_ITEM make = new TBMCONTRACT_MAKE_ITEM();
                    make.OVC_PURCH = strOVC_PURCH;
                    make.OVC_PURCH_5 = strOVC_PURCH_5;
                    make.OVC_PURCH_6 = strOVC_PURCH_6;
                    make.ONB_GROUP = onbgroup;

                    make.ONB_ITEM = 12;
                    make.OVC_ITEM_NAME = lblQ12.Text;
                    make.OVC_RESULT = rdoCheck1.SelectedItem.Text;
                    mpms.TBMCONTRACT_MAKE_ITEM.Add(make);
                    mpms.SaveChanges();
                }
                else
                {
                    if (queryMAKE_ITEM_12.OVC_RESULT != rdoCheck12.SelectedItem.Text)
                    {
                        queryMAKE_ITEM_12.OVC_RESULT = rdoCheck12.SelectedItem.Text;
                        mpms.SaveChanges();
                    }
                }
            }
            if (rdoCheck1.SelectedIndex != -1)
            {
                var queryMAKE_ITEM_13 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 13
                 select tbmmake).FirstOrDefault();
                if (queryMAKE_ITEM_13 == null)
                {
                    TBMCONTRACT_MAKE_ITEM make = new TBMCONTRACT_MAKE_ITEM();
                    make.OVC_PURCH = strOVC_PURCH;
                    make.OVC_PURCH_5 = strOVC_PURCH_5;
                    make.OVC_PURCH_6 = strOVC_PURCH_6;
                    make.ONB_GROUP = onbgroup;

                    make.ONB_ITEM = 13;
                    make.OVC_ITEM_NAME = txtQ13.Text;
                    make.OVC_RESULT = rdoCheck13.SelectedItem.Text;
                    mpms.TBMCONTRACT_MAKE_ITEM.Add(make);
                    mpms.SaveChanges();
                }
                else
                {
                    if (queryMAKE_ITEM_13.OVC_RESULT != rdoCheck13.SelectedItem.Text)
                    {
                        queryMAKE_ITEM_13.OVC_ITEM_NAME = txtQ13.Text;
                        queryMAKE_ITEM_13.OVC_RESULT = rdoCheck13.SelectedItem.Text;
                        mpms.SaveChanges();
                    }
                }
            }
            if (rdoCheck14.SelectedIndex != -1)
            {
                var queryMAKE_ITEM_14 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 14
                 select tbmmake).FirstOrDefault();
                if (queryMAKE_ITEM_14 == null)
                {
                    TBMCONTRACT_MAKE_ITEM make = new TBMCONTRACT_MAKE_ITEM();
                    make.OVC_PURCH = strOVC_PURCH;
                    make.OVC_PURCH_5 = strOVC_PURCH_5;
                    make.OVC_PURCH_6 = strOVC_PURCH_6;
                    make.ONB_GROUP = onbgroup;

                    make.ONB_ITEM = 14;
                    make.OVC_ITEM_NAME = lblQ14.Text;
                    make.OVC_RESULT = rdoCheck14.SelectedItem.Text;
                    mpms.TBMCONTRACT_MAKE_ITEM.Add(make);
                    mpms.SaveChanges();
                }
                else
                {
                    if (queryMAKE_ITEM_14.OVC_RESULT != rdoCheck14.SelectedItem.Text)
                    {
                        queryMAKE_ITEM_14.OVC_RESULT = rdoCheck14.SelectedItem.Text;
                        mpms.SaveChanges();
                    }
                }
            }
            if (rdoCheck15.SelectedIndex != -1)
            {
                var queryMAKE_ITEM_15 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 15
                 select tbmmake).FirstOrDefault();
                if (queryMAKE_ITEM_15 == null)
                {
                    TBMCONTRACT_MAKE_ITEM make = new TBMCONTRACT_MAKE_ITEM();
                    make.OVC_PURCH = strOVC_PURCH;
                    make.OVC_PURCH_5 = strOVC_PURCH_5;
                    make.OVC_PURCH_6 = strOVC_PURCH_6;
                    make.ONB_GROUP = onbgroup;

                    make.ONB_ITEM = 15;
                    make.OVC_ITEM_NAME = lblQ15.Text;
                    make.OVC_RESULT = rdoCheck15.SelectedItem.Text;
                    mpms.TBMCONTRACT_MAKE_ITEM.Add(make);
                    mpms.SaveChanges();
                }
                else
                {
                    if (queryMAKE_ITEM_15.OVC_RESULT != rdoCheck15.SelectedItem.Text)
                    {
                        queryMAKE_ITEM_15.OVC_RESULT = rdoCheck15.SelectedItem.Text;
                        mpms.SaveChanges();
                    }
                }
            }
            if (rdoCheck16.SelectedIndex != -1)
            {
                var queryMAKE_ITEM_16 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 16
                 select tbmmake).FirstOrDefault();
                if (queryMAKE_ITEM_16 == null)
                {
                    TBMCONTRACT_MAKE_ITEM make = new TBMCONTRACT_MAKE_ITEM();
                    make.OVC_PURCH = strOVC_PURCH;
                    make.OVC_PURCH_5 = strOVC_PURCH_5;
                    make.OVC_PURCH_6 = strOVC_PURCH_6;
                    make.ONB_GROUP = onbgroup;

                    make.ONB_ITEM = 16;
                    make.OVC_ITEM_NAME = lblQ16.Text;
                    make.OVC_RESULT = rdoCheck16.SelectedItem.Text;
                    mpms.TBMCONTRACT_MAKE_ITEM.Add(make);
                    mpms.SaveChanges();
                }
                else
                {
                    if (queryMAKE_ITEM_16.OVC_RESULT != rdoCheck16.SelectedItem.Text)
                    {
                        queryMAKE_ITEM_16.OVC_RESULT = rdoCheck16.SelectedItem.Text;
                        mpms.SaveChanges();
                    }
                }
            }
            if (rdoCheck17.SelectedIndex != -1)
            {
                var queryMAKE_ITEM_17 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 17
                 select tbmmake).FirstOrDefault();
                if (queryMAKE_ITEM_17 == null)
                {
                    TBMCONTRACT_MAKE_ITEM make = new TBMCONTRACT_MAKE_ITEM();
                    make.OVC_PURCH = strOVC_PURCH;
                    make.OVC_PURCH_5 = strOVC_PURCH_5;
                    make.OVC_PURCH_6 = strOVC_PURCH_6;
                    make.ONB_GROUP = onbgroup;

                    make.ONB_ITEM = 17;
                    make.OVC_ITEM_NAME = lblQ17.Text;
                    make.OVC_RESULT = rdoCheck17.SelectedItem.Text;
                    mpms.TBMCONTRACT_MAKE_ITEM.Add(make);
                    mpms.SaveChanges();
                }
                else
                {
                    if (queryMAKE_ITEM_17.OVC_RESULT != rdoCheck17.SelectedItem.Text)
                    {
                        queryMAKE_ITEM_17.OVC_RESULT = rdoCheck17.SelectedItem.Text;
                        mpms.SaveChanges();
                    }
                }
            }
            if (rdoCheck18.SelectedIndex != -1)
            {
                var queryMAKE_ITEM_18 =
                (from tbmmake in mpms.TBMCONTRACT_MAKE_ITEM
                 where tbmmake.OVC_PURCH == strOVC_PURCH
                 where tbmmake.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmmake.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmmake.ONB_GROUP == onbgroup
                 where tbmmake.ONB_ITEM == 18
                 select tbmmake).FirstOrDefault();
                if (queryMAKE_ITEM_18 == null)
                {
                    TBMCONTRACT_MAKE_ITEM make = new TBMCONTRACT_MAKE_ITEM();
                    make.OVC_PURCH = strOVC_PURCH;
                    make.OVC_PURCH_5 = strOVC_PURCH_5;
                    make.OVC_PURCH_6 = strOVC_PURCH_6;
                    make.ONB_GROUP = onbgroup;

                    make.ONB_ITEM = 18;
                    make.OVC_ITEM_NAME = lblQ18.Text;
                    make.OVC_RESULT = rdoCheck18.SelectedItem.Text;
                    mpms.TBMCONTRACT_MAKE_ITEM.Add(make);
                    mpms.SaveChanges();
                }
                else
                {
                    if (queryMAKE_ITEM_18.OVC_RESULT != rdoCheck18.SelectedItem.Text)
                    {
                        queryMAKE_ITEM_18.OVC_RESULT = rdoCheck18.SelectedItem.Text;
                        mpms.SaveChanges();
                    }
                }
            }
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), "TBMCONTRACT_MAKE_ITEM", this, "更新");

            //var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            var query =
                (from tbmcm in mpms.TBMCONTRACT_MAKE
                 where tbmcm.OVC_PURCH == strOVC_PURCH
                 where tbmcm.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmcm.OVC_PURCH_6 == strOVC_PURCH_6
                 where tbmcm.ONB_GROUP == onbgroup
                 select tbmcm).FirstOrDefault();
            if(query == null)
            {
                TBMCONTRACT_MAKE make = new TBMCONTRACT_MAKE();
                make.OVC_PURCH = strOVC_PURCH;
                make.OVC_PURCH_5 = strOVC_PURCH_5;
                make.OVC_PURCH_6 = strOVC_PURCH_6;
                //make.OVC_PURCH_6 = query1301.OVC_PUR_USER;
                if (rdoOVC_DO_NAME_RESULT.SelectedIndex != -1)
                {
                    make.OVC_RESULT = rdoOVC_DO_NAME_RESULT.SelectedItem.Text;
                    string strtype = rdoOVC_DO_NAME_RESULT.SelectedItem.Text;
                    make.OVC_RESULT_OTHER = null;
                    if (strtype == "其他")
                        make.OVC_RESULT_OTHER = TextBox1.Text;
                }
                make.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;
                make.ONB_GROUP = onbgroup;
                mpms.TBMCONTRACT_MAKE.Add(make);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), make.GetType().Name.ToString(), this, "新增");
            }
            else
            {
                if (rdoOVC_DO_NAME_RESULT.SelectedIndex != -1)
                {
                    query.OVC_RESULT = rdoOVC_DO_NAME_RESULT.SelectedItem.Text;
                    string strtype = rdoOVC_DO_NAME_RESULT.SelectedItem.Text;
                    query.OVC_RESULT_OTHER = null;
                    if (strtype == "其他")
                        query.OVC_RESULT_OTHER = TextBox1.Text;
                }
                query.OVC_DAPPROVE = txtOVC_DAPPROVE.Text;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), query.GetType().Name.ToString(), this, "修改");
            }
        }


        #region 檢查項目表
        protected void lbtnCheck_Click(object sender, EventArgs e)
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strOVC_PURCH_6 = ViewState["strOVC_PURCH_6"].ToString();
            string strONB_GROUP = ViewState["strONB_GROUP"].ToString();
            short shortONB_GROUP = short.Parse(strONB_GROUP);
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/契約檢查項目表D22.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    doc.ReplaceText("[$OVC_PURCH$]", lblOVC_PURCH.Text, false, System.Text.RegularExpressions.RegexOptions.None);

                    var groceryListTable = doc.Tables.FirstOrDefault(table => table.TableCaption == "CheckItem");
                    if (groceryListTable != null)
                    {
                        var rowPattern_1 = groceryListTable.Rows[1];
                        var rowPattern_2 = groceryListTable.Rows[2];

                        var queryMakeItem =
                            from item in mpms.TBMCONTRACT_MAKE_ITEM.DefaultIfEmpty()
                            where item.OVC_PURCH.Equals(strOVC_PURCH)
                            where item.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            where item.OVC_PURCH_6.Equals(strOVC_PURCH_6)
                            where item.ONB_GROUP.Equals(shortONB_GROUP)
                            orderby item.ONB_ITEM descending
                            select new
                            {
                                ONB_ITEM = item.ONB_ITEM,
                                OVC_ITEM_NAME = item.OVC_ITEM_NAME??"",
                                OVC_RESULT = item.OVC_RESULT,
                            };
                        foreach (var q in queryMakeItem)
                        {
                            if (q.OVC_RESULT == "免審")
                            {
                                var newItem = groceryListTable.InsertRow(rowPattern_2, 1);
                                newItem.ReplaceText("[$NO$]", q.ONB_ITEM.ToString());
                                newItem.ReplaceText("[$ITEM$]", q.OVC_ITEM_NAME.ToString());
                                newItem.ReplaceText("[$Y$]", "");
                                newItem.ReplaceText("[$N$]", "");
                            }
                            else if (q.OVC_RESULT == "是")
                            {
                                var newItem = groceryListTable.InsertRow(rowPattern_1, 1);
                                newItem.ReplaceText("[$NO$]", q.ONB_ITEM.ToString());
                                newItem.ReplaceText("[$ITEM$]", q.OVC_ITEM_NAME.ToString());
                                newItem.ReplaceText("[$Y$]", "V");
                                newItem.ReplaceText("[$N$]", "");
                            }
                            else
                            {
                                var newItem = groceryListTable.InsertRow(rowPattern_1, 1);
                                newItem.ReplaceText("[$NO$]", q.ONB_ITEM.ToString());
                                newItem.ReplaceText("[$ITEM$]", q.OVC_ITEM_NAME.ToString());
                                newItem.ReplaceText("[$Y$]", "");
                                newItem.ReplaceText("[$N$]", "V");
                            }
                        }
                        rowPattern_1.Remove();
                        rowPattern_2.Remove();
                    }

                    var queryMake =
                        (from make in mpms.TBMCONTRACT_MAKE
                         where make.OVC_PURCH.Equals(strOVC_PURCH)
                         where make.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                         where make.OVC_PURCH_6.Equals(strOVC_PURCH_6)
                         where make.ONB_GROUP.Equals(shortONB_GROUP)
                         select new
                         {
                             OVC_RESULT = make.OVC_RESULT,
                             OVC_RESULT_OTHER = make.OVC_RESULT_OTHER
                         }).FirstOrDefault();
                    if (queryMake != null)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            string chk = "[$CHK" + i + "$]";
                            if (queryMake.OVC_RESULT == rdoOVC_DO_NAME_RESULT.Items[i].Text)
                                doc.ReplaceText(chk, "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            else
                                doc.ReplaceText(chk, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OTHER$]", queryMake.OVC_RESULT_OTHER != null ? queryMake.OVC_RESULT_OTHER : "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        string chk = "[$CHK" + i + "$]";
                        doc.ReplaceText(chk, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OTHER$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/CI_Temp.docx");
                }
                buffer = ms.ToArray();
            }
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/CI_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = strOVC_PURCH + "檢查項目表.docx";
            string fileName = HttpUtility.UrlEncode(filepath);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            File.Delete(path_temp);
            Response.End();
        }

        protected void lbtnCheck_odt_Click(object sender, EventArgs e)
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strOVC_PURCH_6 = ViewState["strOVC_PURCH_6"].ToString();
            string strONB_GROUP = ViewState["strONB_GROUP"].ToString();
            short shortONB_GROUP = short.Parse(strONB_GROUP);
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/契約檢查項目表D22.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    doc.ReplaceText("[$OVC_PURCH$]", lblOVC_PURCH.Text, false, System.Text.RegularExpressions.RegexOptions.None);

                    var groceryListTable = doc.Tables.FirstOrDefault(table => table.TableCaption == "CheckItem");
                    if (groceryListTable != null)
                    {
                        var rowPattern_1 = groceryListTable.Rows[1];
                        var rowPattern_2 = groceryListTable.Rows[2];

                        var queryMakeItem =
                            from item in mpms.TBMCONTRACT_MAKE_ITEM
                            where item.OVC_PURCH.Equals(strOVC_PURCH)
                            where item.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            where item.OVC_PURCH_6.Equals(strOVC_PURCH_6)
                            where item.ONB_GROUP.Equals(shortONB_GROUP)
                            orderby item.ONB_ITEM descending
                            select new
                            {
                                ONB_ITEM = item.ONB_ITEM,
                                OVC_ITEM_NAME = item.OVC_ITEM_NAME,
                                OVC_RESULT = item.OVC_RESULT,
                            };
                        foreach (var q in queryMakeItem)
                        {
                            if (q.OVC_RESULT == "免審")
                            {
                                var newItem = groceryListTable.InsertRow(rowPattern_2, 1);
                                newItem.ReplaceText("[$NO$]", q.ONB_ITEM.ToString());
                                newItem.ReplaceText("[$ITEM$]", q.OVC_ITEM_NAME.ToString());
                                newItem.ReplaceText("[$Y$]", "");
                                newItem.ReplaceText("[$N$]", "");
                            }
                            else if (q.OVC_RESULT == "是")
                            {
                                var newItem = groceryListTable.InsertRow(rowPattern_1, 1);
                                newItem.ReplaceText("[$NO$]", q.ONB_ITEM.ToString());
                                newItem.ReplaceText("[$ITEM$]", q.OVC_ITEM_NAME.ToString());
                                newItem.ReplaceText("[$Y$]", "V");
                                newItem.ReplaceText("[$N$]", "");
                            }
                            else
                            {
                                var newItem = groceryListTable.InsertRow(rowPattern_1, 1);
                                newItem.ReplaceText("[$NO$]", q.ONB_ITEM.ToString());
                                newItem.ReplaceText("[$ITEM$]", q.OVC_ITEM_NAME.ToString());
                                newItem.ReplaceText("[$Y$]", "");
                                newItem.ReplaceText("[$N$]", "V");
                            }
                        }
                        rowPattern_1.Remove();
                        rowPattern_2.Remove();
                    }

                    var queryMake =
                        (from make in mpms.TBMCONTRACT_MAKE
                         where make.OVC_PURCH.Equals(strOVC_PURCH)
                         where make.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                         where make.OVC_PURCH_6.Equals(strOVC_PURCH_6)
                         where make.ONB_GROUP.Equals(shortONB_GROUP)
                         select new
                         {
                             OVC_RESULT = make.OVC_RESULT,
                             OVC_RESULT_OTHER = make.OVC_RESULT_OTHER
                         }).FirstOrDefault();
                    if (queryMake != null)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            string chk = "[$CHK" + i + "$]";
                            if (queryMake.OVC_RESULT == rdoOVC_DO_NAME_RESULT.Items[i].Text)
                                doc.ReplaceText(chk, "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            else
                                doc.ReplaceText(chk, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OTHER$]", queryMake.OVC_RESULT_OTHER != null ? queryMake.OVC_RESULT_OTHER : "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        string chk = "[$CHK" + i + "$]";
                        doc.ReplaceText(chk, "□", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OTHER$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/CI_Temp.docx");
                }
                buffer = ms.ToArray();
            }
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/CI_Temp.docx");
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/CI_Temp.odt";
            string FileName = strOVC_PURCH + "檢查項目表.odt";
            FCommon.WordToOdt(this, path_temp, filetemp, FileName);
        }
        #endregion
    }
}