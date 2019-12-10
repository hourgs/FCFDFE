using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.B
{
    public partial class MPMS_B13_6 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strPurchNum;
        string[] strField = { "OVC_NO" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (Request.QueryString["PurchNum"] != null)
                {
                    strPurchNum = Request.QueryString["PurchNum"];
                    TBM1301 tb1301 = new TBM1301();
                    tb1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strPurchNum)).FirstOrDefault();
                    if (tb1301 != null)
                    {
                        pn_Button.Controls.Clear();
                        Buttonevent("M21");
                        ViewState["Field"] = "M21";
                        ViewState["OVC_SYSTEM"] = 1;
                        ViewState["OVC_PURCH_KIND"] = 1;
                    }
                    lblPURCHNUM.Text = strPurchNum;
                }
                else
                {
                    Response.Redirect("MPMS_B11");
                }
            }
        }
        protected void GV_OutPurEditing_PreRender(object sender, EventArgs e)
        {

        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            if (!txtNewMemo.Text.Equals(string.Empty))
            {
                if (ViewState["BtnId"] != null)
                {
                    SaveMemo(txtNewMemo.Text, "ss", "N");
                    ChangeColor();
                    MemoMainImport();
                    txtNewMemo.Text = "";
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

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            Button btnThis = (Button)sender;
            int gvRowIndex = (btnThis.NamingContainer as GridViewRow).RowIndex;
            TextBox txtMemo = (TextBox)GV_OutPurEditing.Rows[gvRowIndex].Cells[2].FindControl("txtSTANDARD");
            SaveMemo(txtMemo.Text, "Memo", "Y");
            ChangeColor();
            MemoMainImport();
        }
        
        protected void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Gridevent(btn.ID.ToString());
            ViewState["BtnId"] = btn.ID;
            GridviewRowSpan();
            GV_OutPurEditing.Rows[0].Cells[0].Text = "目前編輯：" + btn.Text;
            trMemo.Visible = true;
            MemoMainImport();
        }

        protected void Btndel_Click(object sender, EventArgs e)
        {
            //刪除按鈕
            TBM1220_1 tbm1220_1 = new TBM1220_1();
            tbm1220_1 = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_IKIND.Equals("M21")).FirstOrDefault();
            if (tbm1220_1 != null)
            {
                mpms.Entry(tbm1220_1).State = EntityState.Deleted;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , tbm1220_1.GetType().Name.ToString(), this, "刪除");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
            }
            MemoMainImport();
        }
        protected void BtnChange_Click(object sender, EventArgs e)
        {
            txtNewMemo.Text = "";
            txtNewMemo.Text = txtOVC_MEMO.Text;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('請在編輯區依實際情況修訂，修訂完畢請按「存檔」鍵。');", true);
            return;
        }

        private void SaveMemo(string strMemo, string strCheck, string strStandard)
        {
            //存檔
            var queryExist = mpms.TBM1220_1.Where(table => table.OVC_PURCH.Equals(strPurchNum)
                                && table.OVC_IKIND.Equals("M21")).FirstOrDefault();
            if (queryExist == null)
            {
                //null表示沒資料要新增
                TBM1220_1 tbm1220_1 = new TBM1220_1();
                tbm1220_1.OVC_PURCH = strPurchNum;
                tbm1220_1.OVC_IKIND = "M21";
                tbm1220_1.ONB_NO = 1;
                tbm1220_1.OVC_CHECK = strCheck;
                tbm1220_1.OVC_MEMO = strMemo;
                tbm1220_1.OVC_STANDARD = strStandard;
                mpms.TBM1220_1.Add(tbm1220_1);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , tbm1220_1.GetType().Name.ToString(), this, "新增");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功");
            }
            else
            {
                TBM1220_1 table1220_1 = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum)
                                                                   && o.OVC_IKIND.Equals("M21")).FirstOrDefault();
                table1220_1.OVC_CHECK = strCheck;
                table1220_1.OVC_MEMO = strMemo;
                table1220_1.OVC_STANDARD = strStandard;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , table1220_1.GetType().Name.ToString(), this, "修改");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
            }
        }

        private void GridviewRowSpan()
        {
            //合併下方GV第一欄
            foreach (GridViewRow rows in GV_OutPurEditing.Rows)
            {
                GV_OutPurEditing.Rows[0].Cells[0].RowSpan += 1;
                if (rows.RowIndex != 0)
                    rows.Cells[0].Visible = false;
            }
        }

        private void Buttonevent(string field)
        {
            var query =
               from tableSys in gm.TBM1220_2.AsEnumerable()
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
                    pn_Button.Controls.Add(theButton);
                    pn_Button.Controls.Add(new LiteralControl(" "));
                }
            }
        }

        private void ChangeColor()
        {
            //存檔後更改按鈕顏色
            string btnid = ViewState["BtnId"].ToString();
            Button btn = (Button)this.Master.FindControl("MainContent").FindControl(btnid);
            btn.ForeColor = System.Drawing.Color.Red;
        }

        private void MemoMainImport()
        {
            txtOVC_MEMO.Text = "";
            var query = mpms.TBM1220_1.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_IKIND.Equals("M21")).FirstOrDefault();
            if (query != null)
            {
                string strTemp;
                strTemp = query.OVC_MEMO.Replace("<br>", "");
                txtOVC_MEMO.Text = strTemp;
            }

        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_urlUse;
            send_urlUse = "~/pages/MPMS/B/MPMS_B13.aspx?PurchNum=" + strPurchNum;
            Response.Redirect(send_urlUse);
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
            //要先把<br>拿掉
            foreach (DataRow rows in dtSubItem.Rows)
            {
                string strTemp = rows["OVC_MEMO"].ToString();
                strTemp = strTemp.Replace("<br>", "");
                rows["OVC_MEMO"] = strTemp;
            }
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_OutPurEditing, dtSubItem, strField);
            
        }
    }
}