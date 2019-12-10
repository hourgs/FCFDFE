using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.B
{
    public partial class MPMS_B13_7 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strPurchNum = "AG96001";
        string[] strField = { "OVC_NO" };
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (Request.QueryString["PurchNum"] != null)
                {
                    strPurchNum = Request.QueryString["PurchNum"].ToString();
                    if (strPurchNum != null)
                    {
                        TBM1301 tb1301 = new TBM1301();
                        tb1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                        if (tb1301 != null)
                        {
                            if (tb1301.OVC_PUR_AGENCY.Equals("B") || tb1301.OVC_PUR_AGENCY.Equals("L") || tb1301.OVC_PUR_AGENCY.Equals("P"))
                            {
                                pn_Button.Controls.Clear();
                                pn_Button2.Controls.Clear();
                                Buttonevent("D5");
                                ViewState["Field"] = "D5";
                                ViewState["OVC_SYSTEM"] = 1;
                                ViewState["OVC_PURCH_KIND"] = 1;
                            }
                            else if (tb1301.OVC_PUR_AGENCY.Equals("M") || tb1301.OVC_PUR_AGENCY.Equals("S"))
                            {
                                pn_Button.Controls.Clear();
                                pn_Button2.Controls.Clear();
                                Buttonevent("F5");
                                ViewState["Field"] = "F5";
                                ViewState["OVC_SYSTEM"] = 1;
                                ViewState["OVC_PURCH_KIND"] = 2;
                            }
                            else
                            {
                                pn_Button.Controls.Clear();
                                pn_Button2.Controls.Clear();
                                Buttonevent("W5");
                                ViewState["Field"] = "W5";
                                ViewState["OVC_SYSTEM"] = 1;
                                ViewState["OVC_PURCH_KIND"] = 2;
                            }
                        }
                        if (!IsPostBack)
                        {
                            LoginScreen();
                        }
                    }
                }
                else
                {
                    Response.Redirect("MPMS_B11");
                }
            }
        }
        private void LoginScreen()
        {
            lblOVC_PURCH.Text = strPurchNum;
            lblbtnText.Visible = false;
        }

        

        protected void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Gridevent(btn.ID.ToString());
            string strOVC_IKIND = btn.ID.ToString();
            ViewState["BtnId"] = strOVC_IKIND;
            ViewState["isCreate"] = true;
            lblbtnText.Visible = true;
            lblbtnText.Text = btn.Text;
            GV_OVC_ISOURCE.Rows[0].Cells[0].Text = "目前編輯：" + btn.Text;
            txtNewOVC_MEMO.Text = "";
            GridviewRowSpan();
            MemoMainImport();
        }

        protected void btn_change_Click(object sender, EventArgs e)
        {
            //上方GV的異動按鈕
            Button btnThis = (Button)sender;
            int gvRowIndex = (btnThis.NamingContainer as GridViewRow).RowIndex;
            TextBox txtMemo = (TextBox)GV_EDITMEMO.Rows[gvRowIndex].Cells[1].FindControl("txtOVC_MEMO_Main");
            //hid裡放的是ONBNO因為有些值不是從1開始編
            HiddenField hidField = (HiddenField)GV_EDITMEMO.Rows[gvRowIndex].Cells[1].FindControl("hidONB_NO");
            ViewState["ONBNO"] = hidField.Value;
            txtNewOVC_MEMO.Text = txtMemo.Text;
            txtNewOVC_MEMO.Rows = (txtNewOVC_MEMO.Text.Length / 70) + 1;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('請在編輯區依實際情況修訂，修訂完畢請按「存檔」鍵。');", true);
            return;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //GV裡的直接存檔按鈕
            //找出按下的值
            Button btnThis = (Button)sender;
            int gvRowIndex = (btnThis.NamingContainer as GridViewRow).RowIndex;
            TextBox txtMemo = (TextBox)GV_OVC_ISOURCE.Rows[gvRowIndex].Cells[2].FindControl("txtOVC_MEMO");
            HiddenField hidCHECK = (HiddenField)GV_OVC_ISOURCE.Rows[gvRowIndex].Cells[2].FindControl("hidOVC_CHECK");
            //存檔至1220_1
            SaveMemo(txtMemo.Text, hidCHECK.Value);
            //刷新上方GV
            MemoMainImport();
            //改顏色
            ChangeColor();
        }

        protected void btnEdit_Save_Click(object sender, EventArgs e)
        {
            //下方編輯區的按鈕
            if (!txtNewOVC_MEMO.Text.Equals(string.Empty))
            {
                if(ViewState["BtnId"] != null)
                {
                    //編輯區的存檔按鈕
                    //存檔至1220_1
                    EditSave(txtNewOVC_MEMO.Text);
                    //刷新上方GV
                    MemoMainImport();
                    //改顏色
                    ChangeColor();
                    txtNewOVC_MEMO.Text = "";
                }
                else
                {
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先 選擇種類");
                }
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先 輸入內容");
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            Button btnThis = (Button)sender;
            int gvRowIndex = (btnThis.NamingContainer as GridViewRow).RowIndex;
            string btnid = ViewState["BtnId"].ToString();
            HiddenField hidField = (HiddenField)GV_EDITMEMO.Rows[gvRowIndex].Cells[1].FindControl("hidONB_NO");
            short index = short.Parse(hidField.Value);
            TBM1220_1 tbm1220_1 = new TBM1220_1();
            tbm1220_1 = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum)
                                                && o.OVC_IKIND.Equals(btnid) && o.ONB_NO == index).FirstOrDefault();
            mpms.Entry(tbm1220_1).State = EntityState.Deleted;
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , tbm1220_1.GetType().Name.ToString(), this, "刪除");
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
            //刷新
            MemoMainImport();
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
                   OVC_CHECK=tableTbm.OVC_CHECK,
                   tableTbm.OVC_NO,
               };
            DataTable dtSubItem = CommonStatic.LinqQueryToDataTable(query);
            //要先把<br>拿掉
            foreach (DataRow rows in dtSubItem.Rows)
            {
                string strTemp = rows["OVC_MEMO"].ToString();
                strTemp = strTemp.Replace("<br>", "");
                rows["OVC_MEMO"] = strTemp;
            }
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_OVC_ISOURCE, dtSubItem, strField);
        }
        
        private void GridviewRowSpan()
        {
           foreach(GridViewRow rows in GV_OVC_ISOURCE.Rows)
            {
                GV_OVC_ISOURCE.Rows[0].Cells[0].RowSpan += 1;
                if (rows.RowIndex != 0)
                    rows.Cells[0].Visible = false;
            }
        }

        private void MemoMainImport()
        {
            //上方GV資料載入
            string strOVC_IKIND = ViewState["BtnId"].ToString();
            string[] strItem = { "ONB_NO","OVC_MEMO" };
            var query =
                from table in mpms.TBM1220_1
                where table.OVC_PURCH.Equals(strPurchNum) && table.OVC_IKIND.Equals(strOVC_IKIND)
                select new
                {
                    table.ONB_NO,
                    table.OVC_MEMO
                };
            DataTable tbm1220_1 = CommonStatic.LinqQueryToDataTable(query);
            //要先把<br>拿掉
            foreach (DataRow rows in tbm1220_1.Rows)
            {
                string strTemp = rows["OVC_MEMO"].ToString();
                strTemp = strTemp.Replace("<br>", "");
                rows["OVC_MEMO"] = strTemp;
            }
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_EDITMEMO, tbm1220_1, strItem);
        }

       

        private void ChangeColor()
        {
            //存檔後更改按鈕顏色
            string btnid = ViewState["BtnId"].ToString();
            Button btn = (Button)this.Master.FindControl("MainContent").FindControl(btnid);
            btn.ForeColor = System.Drawing.Color.Red;
        }

        
        private void SaveMemo(string strMemo,string strCHECK)
        {
            string strOVC_IKIND = ViewState["BtnId"].ToString();
            var queryExist = mpms.TBM1220_1.Where(table => table.OVC_PURCH.Equals(strPurchNum)
                                && table.OVC_IKIND.Equals(strOVC_IKIND)).FirstOrDefault();
          
            if (queryExist == null)
            {
                //null表示沒資料要新增
                TBM1220_1 tbm1220_1 = new TBM1220_1();
                tbm1220_1.OVC_PURCH = strPurchNum;
                tbm1220_1.OVC_IKIND = strOVC_IKIND;
                tbm1220_1.ONB_NO = 1;
                tbm1220_1.OVC_CHECK = strCHECK;
                tbm1220_1.OVC_MEMO = strMemo;
                tbm1220_1.OVC_STANDARD = "Y";
                mpms.TBM1220_1.Add(tbm1220_1);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , tbm1220_1.GetType().Name.ToString(), this, "新增");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功");
            }
            else
            {
                //兩種情況普通的修改（只能有一筆）,特殊項目新增（可以有多筆）
                switch (strOVC_IKIND)
                {
                    case "D50":
                    case "D52":
                    case "D53":
                    case "D55":
                    case "D56":
                    case "D57":
                    case "D58":
                    case "D59":
                    case "D5B":
                    case "D5D":
                    case "D5E":
                    case "D5F":
                    case "D5G":
                    case "D5H":
                    case "D5I":
                    case "D5K":
                    case "D5L":
                    case "W50":
                    case "W5B":
                    case "W5G":
                    case "W5I":
                    case "W5J":
                    case "W5M":
                        int count = GV_EDITMEMO.Rows.Count + 1;
                        TBM1220_1 tbm1220_1 = new TBM1220_1();
                        tbm1220_1.OVC_PURCH = strPurchNum;
                        tbm1220_1.OVC_IKIND = strOVC_IKIND;
                        tbm1220_1.ONB_NO = (short)count;
                        tbm1220_1.OVC_CHECK = strCHECK;
                        tbm1220_1.OVC_MEMO = strMemo;
                        tbm1220_1.OVC_STANDARD = "Y";
                        mpms.TBM1220_1.Add(tbm1220_1);
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , tbm1220_1.GetType().Name.ToString(), this, "新增");
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功");
                        break;
                    default:
                        TBM1220_1 table1220_1 = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum)
                                                                   && o.OVC_IKIND.Equals(strOVC_IKIND)).FirstOrDefault();
                        table1220_1.OVC_CHECK = strCHECK;
                        table1220_1.OVC_MEMO = strMemo;
                        table1220_1.OVC_STANDARD = "Y";
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , table1220_1.GetType().Name.ToString(), this, "修改");
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                        break;
                }
            }
        }
        
        private void EditSave(string strMemo)
        {
            string strOVC_IKIND = ViewState["BtnId"].ToString();
            if ( ViewState["ONBNO"] == null)
            {
                //null表示沒資料要新增
                TBM1220_1 tbm1220_1 = new TBM1220_1();
                tbm1220_1.OVC_PURCH = strPurchNum;
                tbm1220_1.OVC_IKIND = strOVC_IKIND;
                tbm1220_1.ONB_NO = 1;
                tbm1220_1.OVC_CHECK = "ss";
                tbm1220_1.OVC_MEMO = strMemo;
                tbm1220_1.OVC_STANDARD = "N";
                mpms.TBM1220_1.Add(tbm1220_1);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                        , tbm1220_1.GetType().Name.ToString(), this, "新增");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功");
            }
            else
            {
                short index = short.Parse(ViewState["ONBNO"].ToString());
                TBM1220_1 table1220_1 = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum)
                                                                   && o.OVC_IKIND.Equals(strOVC_IKIND) && o.ONB_NO==index).FirstOrDefault();
                table1220_1.OVC_CHECK = "ss";
                table1220_1.OVC_MEMO = strMemo;
                table1220_1.OVC_STANDARD = "N";
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , table1220_1.GetType().Name.ToString(), this, "修改");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            //回物資申請書
            string send_urlUse;
            send_urlUse = "~/pages/MPMS/B/MPMS_B13.aspx?PurchNum=" + strPurchNum;
            Response.Redirect(send_urlUse);
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
                from tableSys in gm.TBM1220_2.AsEnumerable()
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
                    if (i <= 5)
                    {
                        pn_Button.Controls.Add(theButton);
                        pn_Button.Controls.Add(new LiteralControl(" "));
                    }
                    else
                    {
                        pn_Button2.Controls.Add(theButton);
                        pn_Button2.Controls.Add(new LiteralControl(" "));
                    }
                }
            }
        }
    }
}