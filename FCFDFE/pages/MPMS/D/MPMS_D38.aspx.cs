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
    public partial class MPMS_D38 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_PURCH_5"] == null || Request.QueryString["ONB_GROUP"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    if (!IsPostBack && IsOVC_DO_NAME())
                    {
                        string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
                        string strOVC_PURCH_5_url = Request.QueryString["OVC_PURCH_5"];
                        string strONB_GROUP_url = Request.QueryString["ONB_GROUP"];
                        string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
                        string strOVC_PURCH_5 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_5_url));
                        string strONB_GROUP = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strONB_GROUP_url));

                        ViewState["strOVC_PURCH"] = strOVC_PURCH;
                        ViewState["strOVC_PURCH_5"] = strOVC_PURCH_5;
                        ViewState["strONB_GROUP"] = strONB_GROUP;
                        short onbgroup = Convert.ToInt16(strONB_GROUP);
                        var query1302 =
                        (from tbm1302 in mpms.TBM1302
                         where tbm1302.OVC_PURCH == strOVC_PURCH
                         where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
                         where tbm1302.ONB_GROUP == onbgroup
                         select tbm1302).FirstOrDefault();

                        var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
                        lblOVC_VEN_TITLE.Text = query1302.OVC_VEN_TITLE;
                        lblOVC_PURCH.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                        lblOVC_PURCH_6.Text = query1302.OVC_PURCH_6;
                        string strCST = query1302.OVC_VEN_CST;
                        ViewState["strOVC_VEN_CST"] = strCST;
                        ViewState["strOVC_PURCH_6"] = lblOVC_PURCH_6.Text;
                        lblONB_GROUP.Text = strONB_GROUP;
                        dataImport(strOVC_PURCH, strOVC_PURCH_5, onbgroup);
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

        private void dataImport(string strOVC_PURCH, string strOVC_PURCH_5, short onbgroup)
        {
            DataTable dt = new DataTable();
            var query =
            from tbm1302 in mpms.TBM1302
            where tbm1302.OVC_PURCH == strOVC_PURCH
            where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
            where tbm1302.ONB_GROUP == onbgroup
            join tbm1201 in mpms.TBM1201 on tbm1302.OVC_PURCH equals tbm1201.OVC_PURCH
            join tbm1407 in mpms.TBM1407 on tbm1201.OVC_POI_IUNIT equals tbm1407.OVC_PHR_ID
            where tbm1407.OVC_PHR_CATE == "J1"
            join tbm1302item in mpms.TBM1302_ITEM on new {tbm1302.OVC_PURCH,tbm1302.OVC_PURCH_5,tbm1302.OVC_PURCH_6}equals new {tbm1302item.OVC_PURCH,tbm1302item.OVC_PURCH_5,tbm1302item.OVC_PURCH_6}
            into tb
            let tbm1302item = tb.FirstOrDefault()
            select new
            {
                OVC_PURCH = tbm1302.OVC_PURCH,
                OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                ONB_GROUP = tbm1302.ONB_GROUP,
                OVC_VEN_CST = tbm1302.OVC_VEN_CST,
                ONB_POI_ICOUNT = tbm1201.ONB_POI_ICOUNT,
                ONB_ICOUNT = tbm1302item.ONB_ICOUNT, //合約項次
                OVC_PUR_IPURCH = tbm1201.OVC_POI_NSTUFF_CHN,
                NSN = tbm1201.NSN,
                OVC_POI_IUNIT = tbm1407.OVC_PHR_DESC,
                ONB_POI_QORDER_PLAN = tbm1201.ONB_POI_QORDER_PLAN
            };

            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["Status"] = FCommon.GridView_dataImport(GV_info, dt);
        }

        protected void btnSAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < GV_info.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)GV_info.Rows[i].FindControl("CheckBox1");
                cb.Checked = true;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < GV_info.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)GV_info.Rows[i].FindControl("CheckBox1");
                cb.Checked = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            short onbgroup = Convert.ToInt16((ViewState["strONB_GROUP"].ToString()));
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strOVC_PURCH_6 = ViewState["strOVC_PURCH_6"].ToString();
            string strOVC_VEN_CST = ViewState["strOVC_VEN_CST"].ToString();
            var queryitem =
                (from tbm1302item in mpms.TBM1302_ITEM.OrderByDescending(o => o.ONB_ICOUNT)
                 where tbm1302item.OVC_PURCH == strOVC_PURCH
                 where tbm1302item.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbm1302item.OVC_PURCH_6 == strOVC_PURCH_6
                 select tbm1302item).FirstOrDefault();
            short n;
            if (queryitem == null)
            {
                n = 1;
            }
            else
            {
                string stronbicount = queryitem.ONB_ICOUNT.ToString();
                n = Convert.ToInt16(stronbicount);
            }
                
                
            for (int i = 0; i < GV_info.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)GV_info.Rows[i].FindControl("CheckBox1");
                if (cb.Checked == true)
                {
                    short onbpoiicount = Convert.ToInt16((GV_info.Rows[i].Cells[4].Text));
                    
                    var query =
                        (from tbm1302 in mpms.TBM1302_ITEM
                         where tbm1302.OVC_PURCH == strOVC_PURCH
                         where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
                         where tbm1302.OVC_PURCH_6 == strOVC_PURCH_6
                         where tbm1302.ONB_GROUP == onbgroup
                         where tbm1302.ONB_POI_ICOUNT == onbpoiicount
                         where tbm1302.OVC_VEN_CST == strOVC_VEN_CST
                         select tbm1302).FirstOrDefault();
                    if(query == null)
                    {
                        TBM1302_ITEM t1302 = new TBM1302_ITEM();
                        t1302.OVC_PURCH = strOVC_PURCH;
                        t1302.OVC_PURCH_5 = strOVC_PURCH_5;
                        t1302.OVC_PURCH_6 = strOVC_PURCH_6;
                        t1302.OVC_VEN_CST = strOVC_VEN_CST;
                        t1302.ONB_GROUP = onbgroup;
                        t1302.ONB_ICOUNT = n;
                        t1302.ONB_POI_ICOUNT = onbpoiicount;

                        mpms.TBM1302_ITEM.Add(t1302);
                        mpms.SaveChanges();
                        n++;
                        strMessage += "<P> 本草約之原採購項次"+ onbpoiicount.ToString()+ "明細存檔成功</p>";
                    }
                    else
                        strMessage += "<P> 本草約之原採購項次" + onbpoiicount.ToString() + "明細已經存在</p>";
                }
            }
            if(strMessage.Contains("成功"))
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), queryitem.GetType().Name.ToString(), this, "新增");
            FCommon.AlertShow(PnMessage, "success", "系統訊息", strMessage);
        }

        protected void btnPrice_Click(object sender, EventArgs e)
        {
            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH"].ToString()));
            string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH_5"].ToString()));
            string key3 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["ONB_GROUP"].ToString()));
            Response.Redirect("MPMS_D39.aspx?OVC_PURCH=" + key + "&OVC_PURCH_5=" + key2 + "&ONB_GROUP=" + key3);
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH"].ToString()));
            string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH_5"].ToString()));
            string key3 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["ONB_GROUP"].ToString()));
            Response.Redirect("MPMS_D37.aspx?OVC_PURCH=" + key + "&OVC_PURCH_5=" + key2 + "&ONB_GROUP=" + key3);
        }

        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_D14_1.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString());
        }

        protected void GV_info_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
            {
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            }
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}