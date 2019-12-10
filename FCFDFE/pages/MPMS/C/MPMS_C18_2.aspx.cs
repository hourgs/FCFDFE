using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Linq;
using System.Data;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.C
{
    public partial class MPMS_C18_2 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strPurchNum = "";
        int ONB_CHECK_TIMES = 0;
        string strDRecive = "";
        string[] strField = { "OVC_NO" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!string.IsNullOrEmpty(Request.QueryString["PurchNum"]) &&
                    !string.IsNullOrEmpty(Request.QueryString["CheckTimes"]) &&
                    !string.IsNullOrEmpty(Request.QueryString["DRecive"]))
                {
                    strPurchNum = Request.QueryString["PurchNum"].ToString();
                    strDRecive = Request.QueryString["DRecive"].ToString();
                    ONB_CHECK_TIMES = Convert.ToInt32(Request.QueryString["CheckTimes"]);
                    TBM1301 tb1301 = new TBM1301();
                    tb1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                    if (tb1301 != null)
                    {
                        if (tb1301.OVC_PUR_AGENCY.Equals("B") || tb1301.OVC_PUR_AGENCY.Equals("L") || tb1301.OVC_PUR_AGENCY.Equals("P"))
                        {
                            ViewState["Field"] = "M3";
                        }
                        else if (tb1301.OVC_PUR_AGENCY.Equals("M") || tb1301.OVC_PUR_AGENCY.Equals("S"))
                        {
                            ViewState["Field"] = "F3";
                        }
                        else
                        {
                            ViewState["Field"] = "W3";
                        }

                        if (tb1301.OVC_PUR_AGENCY.Equals("B") || tb1301.OVC_PUR_AGENCY.Equals("L") || tb1301.OVC_PUR_AGENCY.Equals("P"))
                        {
                            ViewState["BtnField"] = "A1";
                        }
                        else if (tb1301.OVC_PUR_AGENCY.Equals("M"))
                        {
                            ViewState["BtnField"] = "A3";
                        }
                        else
                        {
                            ViewState["BtnField"] = "A2";
                        }
                        Buttonevent(ViewState["BtnField"].ToString());
                        ViewState["BtnText"] = "採購單位";
                        if (!IsPostBack)
                        {
                            RequestImport();
                            GetUserInfo();
                        }

                    }

                }
                else
                {
                    Response.Redirect("MPMS_C14");
                }
            }
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Gridevent(btn.ID.ToString());
            ViewState["BtnId"] = btn.ID;
            ViewState["BtnText"] = btn.Text;
            GridviewRowSpan();
            RequestImport();
            GV_OVC_MEMO.Rows[0].Cells[0].Text = "目前編輯：" + btn.Text;
            MemoMainImport();
        }

        private void MemoMainImport()
        {
            //上方GV資料載入
            if (ViewState["BtnId"] != null)
            {
                string strOVC_IKIND = ViewState["BtnId"].ToString();
                string[] strItem = { "ONB_NO", "OVC_MEMO" };
                var query =
                    from table in mpms.TBM1202_7
                    where table.OVC_PURCH.Equals(strPurchNum) && table.OVC_IKIND.Equals(strOVC_IKIND)
                            && table.ONB_CHECK_TIMES == ONB_CHECK_TIMES
                    select new
                    {
                        table.ONB_NO,
                        table.OVC_MEMO
                    };
                DataTable tbm1220_1 = CommonStatic.LinqQueryToDataTable(query);
                //要先把<br>拿掉
                //foreach (DataRow rows in tbm1220_1.Rows)
                //{
                //    string strTemp = rows["OVC_MEMO"].ToString();
                //    strTemp = strTemp.Replace("<br>", "");
                //    rows["OVC_MEMO"] = strTemp;
                //}
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_EDITMEMO, tbm1220_1, strItem);
            }
           
        }

        protected void GV_EDITMEMO_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string detp = ViewState["DEPT_SN"].ToString();
            Button BTN = (Button)e.CommandSource;
            GridViewRow myRow = (GridViewRow)BTN.NamingContainer;
            TextBox txtOVC_MEMO_Main = (TextBox)GV_EDITMEMO.Rows[myRow.DataItemIndex].FindControl("txtOVC_MEMO_Main");
            HiddenField hidONB_NO = (HiddenField)GV_EDITMEMO.Rows[myRow.DataItemIndex].FindControl("hidONB_NO");
            ViewState["ONBNO"] = hidONB_NO.Value;
            if (e.CommandName.Equals("btn_change"))
            {
                txtNewOVC_MEMO.Text = txtOVC_MEMO_Main.Text;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('請在編輯區依實際情況修訂，修訂完畢請按「存檔」鍵。');", true);
            }
            else if (e.CommandName.Equals("btnDel"))
            {
                int number = Convert.ToInt32(hidONB_NO.Value);
                TBM1202_7 tbm1202_7 = new TBM1202_7();
                tbm1202_7 =
                    mpms.TBM1202_7.Where(o => o.OVC_PURCH.Equals(strPurchNum)
                                         && o.ONB_CHECK_TIMES == ONB_CHECK_TIMES
                                         && o.OVC_CHECK_UNIT == detp
                                         && o.ONB_NO== number)
                                  .FirstOrDefault();
                mpms.Entry(tbm1202_7).State = EntityState.Deleted;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1202_7.GetType().Name.ToString(), this, "刪除");
                MemoMainImport();
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
            }
        }

       

        protected void GV_OVC_MEMO_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Button BTN = (Button)e.CommandSource;
            GridViewRow myRow = (GridViewRow)BTN.NamingContainer;
            TextBox txtOVC_MEMO = (TextBox)GV_OVC_MEMO.Rows[myRow.DataItemIndex].FindControl("txtOVC_MEMO");
            if (e.CommandName.Equals("btn_save"))
            {
                SaveMemo(txtOVC_MEMO.Text, "Memo", "Y");
            }
        }

        protected void btn_EditSave_Click(object sender, EventArgs e)
        {
            EditMemo();
        }

        private void EditMemo()
        {
            if (ViewState["BtnId"] != null)
            {
                string detp = ViewState["DEPT_SN"].ToString();
                string ikind = ViewState["BtnId"].ToString();
                string strOVC_IKIND = ViewState["BtnId"].ToString();
                if (ViewState["ONBNO"] == null)
                {
                    //null表示沒資料要新增
                    TBM1202_7 tbm1202_7 = new TBM1202_7();
                    tbm1202_7.OVC_PURCH = strPurchNum;
                    tbm1202_7.ONB_CHECK_TIMES = (byte)ONB_CHECK_TIMES;
                    tbm1202_7.OVC_CHECK_UNIT = detp;
                    tbm1202_7.OVC_IKIND = ikind;
                    tbm1202_7.ONB_NO = 1;
                    tbm1202_7.OVC_MEMO = txtNewOVC_MEMO.Text;
                    tbm1202_7.OVC_CHECK = "ss";
                    tbm1202_7.OVC_STANDARD = "N";
                    mpms.TBM1202_7.Add(tbm1202_7);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1202_7.GetType().Name.ToString(), this, "新增");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功");
                }
                else
                {
                    short index = short.Parse(ViewState["ONBNO"].ToString());
                    TBM1202_7 table1202_7 = mpms.TBM1202_7.Where(o => o.OVC_PURCH.Equals(strPurchNum)
                                                                       && o.OVC_IKIND.Equals(strOVC_IKIND) 
                                                                       && o.OVC_CHECK_UNIT.Equals(detp)
                                                                       && o.ONB_NO == index)
                                                          .FirstOrDefault();
                    table1202_7.OVC_CHECK = "ss";
                    table1202_7.OVC_MEMO = txtNewOVC_MEMO.Text;
                    table1202_7.OVC_STANDARD = "N";
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , table1202_7.GetType().Name.ToString(), this, "修改");
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                }
                //刷新上方GV
                MemoMainImport();
                txtNewOVC_MEMO.Text = "";
                ViewState["ONBNO"] = null;
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先 選擇種類");
            }
            ViewState["BtnId"]=null;
        }

        private void SaveMemo(string memo,string strCheck,string standard)
        {
            string detp = ViewState["DEPT_SN"].ToString();
            string ikind = ViewState["BtnId"].ToString();
            int count = GV_EDITMEMO.Rows.Count + 1;
            
            TBM1202_7 tbm1202_7 = new TBM1202_7();
            tbm1202_7.OVC_PURCH = strPurchNum;
            tbm1202_7.ONB_CHECK_TIMES = (byte)ONB_CHECK_TIMES;
            tbm1202_7.OVC_CHECK_UNIT = detp;
            tbm1202_7.OVC_IKIND = ikind;
            tbm1202_7.ONB_NO = (byte)count;
            tbm1202_7.OVC_MEMO = memo;
            tbm1202_7.OVC_CHECK = strCheck;
            tbm1202_7.OVC_STANDARD = standard;
            mpms.TBM1202_7.Add(tbm1202_7);
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                           , tbm1202_7.GetType().Name.ToString(), this, "新增");
            MemoMainImport();
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功");
        }

        private string IkindName()
        {
            string MemoName = "";
            string btnText = ViewState["BtnText"].ToString();
            if (btnText.Contains("注意事項"))
            {
                MemoName = "其他";
            }
            else if(btnText.Contains("其他"))
            {
                MemoName = "empty";
            }
            else
            {
                int startInt = btnText.IndexOf(")");
                MemoName = btnText.Substring(startInt + 1);
            }
            return MemoName;
        }

        private void RequestImport()
        {
            string strIkind = ViewState["Field"].ToString();
            string MemoName = IkindName();
           
            var query =
                from t1220_1 in mpms.TBM1220_1
                join t1220_2 in mpms.TBM1220_2 on t1220_1.OVC_IKIND equals t1220_2.OVC_IKIND
                where t1220_1.OVC_PURCH.Equals(strPurchNum) && t1220_1.OVC_IKIND.StartsWith(strIkind)
                        && t1220_2.OVC_MEMO_NAME.Contains(MemoName)
                select new { t1220_1.OVC_MEMO };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            rpt_Request.DataSource = dt;
            rpt_Request.DataBind();

        }

        private void Buttonevent(string field)
        {
            var query =
               from tableSys in gm.TBM1220_2
               where tableSys.OVC_IKIND.StartsWith(field)
               select new
               {
                   Value = tableSys.OVC_IKIND,
                   Field = tableSys.OVC_MEMO_NAME
               };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string strSysId = dt.Rows[i]["Value"].ToString();
                string strSysName = dt.Rows[i]["Field"].ToString();
                var queryInner =
                from tableSys in gm.TBM1220_2
                where tableSys.OVC_IKIND.StartsWith(field)
                select new
                {
                    Value = tableSys.OVC_IKIND,
                    Field = tableSys.OVC_MEMO_NAME
                };
                DataTable chkItems = CommonStatic.LinqQueryToDataTable(queryInner);
                if (chkItems.Rows.Count > 0)
                {
                    Button theButton = new Button();
                    theButton.ID = dt.Rows[i]["Value"].ToString();
                    theButton.Text = dt.Rows[i]["Field"].ToString();
                    theButton.CssClass = "btn-default";
                    theButton.Click += new EventHandler(btn_Click);
                    pn_Button.Controls.Add(theButton);
                    pn_Button.Controls.Add(new LiteralControl(" "));
                }
            }
        }

        private void GridviewRowSpan()
        {
            //合併下方GV第一欄
            foreach (GridViewRow rows in GV_OVC_MEMO.Rows)
            {
                GV_OVC_MEMO.Rows[0].Cells[0].RowSpan += 1;
                if (rows.RowIndex != 0)
                    rows.Cells[0].Visible = false;
            }
        }

       
        private void Gridevent(string item)
        {
            var tb1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
            var standardItem = gm.TBMSTANDARDITEMs.Where(o => o.OVC_PUR_AGENCY != null);
            var query =
                from tableSys in gm.TBM1220_2
                join tableTbm in standardItem on tableSys.OVC_IKIND equals tableTbm.OVC_IKIND
                where tableSys.OVC_IKIND.StartsWith(item) && tableTbm.OVC_PUR_AGENCY.Contains(tb1301.OVC_PUR_AGENCY)
                select new
                {
                    OVC_MEMO_NAME = tableSys.OVC_MEMO_NAME,
                    OVC_MEMO = tableTbm.OVC_MEMO,
                    OVC_PUR_AGENCY = tableTbm.OVC_PUR_AGENCY,
                    OVC_DESC = tableTbm.OVC_DESC,
                    tableTbm.OVC_NO,
                };
            DataTable dtSubItem = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_OVC_MEMO, dtSubItem, strField);
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string url = "~/pages/MPMS/C/MPMS_C18.aspx?PurchNum=" + strPurchNum + "&numCheckTimes=" + ONB_CHECK_TIMES + "&DRecive=" + strDRecive;
            Response.Redirect(url);
        }

        private void GetUserInfo()
        {
            if (Session["userid"] != null)
            {
                string strUSER_ID = Session["userid"].ToString();
                var query =
                    (from tAccount in gm.ACCOUNTs
                     where tAccount.USER_ID.Equals(strUSER_ID)
                     select new
                     {
                         tAccount.DEPT_SN,
                     }).FirstOrDefault();

                ViewState["DEPT_SN"] = query.DEPT_SN;
            }
        }
    }
}