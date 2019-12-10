using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.GM
{
    public partial class Standardlanguage : Page
    {       
        public string strMenuName = "", strMenuNameItem = "";
        GMEntities ae = new GMEntities();
        Common FCommon = new Common();

        #region 副程式
        private void Gridevent(string item)
        {
            var query =
               from tableSys in ae.TBM1220_2
               join tableTbm in ae.TBMSTANDARDITEMs on tableSys.OVC_IKIND equals tableTbm.OVC_IKIND
               where tableSys.OVC_IKIND.StartsWith(item)
               select new
               {
                   OVC_MEMO_NAME = tableSys.OVC_MEMO_NAME,
                   OVC_MEMO = tableTbm.OVC_MEMO,
                   OVC_PUR_AGENCY = tableTbm.OVC_PUR_AGENCY,
                   OVC_DESC = tableTbm.OVC_DESC,
                   tableTbm.OVC_NO,
               };
            DataTable dtSubItem = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBMStandardItem, dtSubItem);
        }

        private void Buttonevent(string field) {
            var query =
               from tableSys in ae.TBM1220_2
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
                from tableSys in ae.TBM1220_2
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

        private void Agency(String[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                CheckBox theCheckBox = new CheckBox();
                theCheckBox.Text = array[i].ToString();
                theCheckBox.ID = array[i].ToString();
                pn_CheckBox.Controls.Add(theCheckBox);
                pn_CheckBox.Controls.Add(new LiteralControl(" "));
            }
        }

        private void Controls_Attributes(string strName, string strValue, params Control[] controls)
        {
            foreach (Control theControl in controls)
            {
                if (theControl is Button)
                {
                    (theControl as Button).Attributes.Add(strName, strValue);
                }
            }
        }

        private void Attributes_Remove(string strName, params Control[] controls)
        {
            foreach (Control theControl in controls)
            {
                if (theControl is Button)
                {
                    (theControl as Button).Attributes.Remove(strName);
                }
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FCommon.GridView_setEmpty(GV_TBMStandardItem);
            }

            if (ViewState["Field"] != null)
            {
                Buttonevent(ViewState["Field"].ToString());
            }

            if(ViewState["Agency"] != null)
            {
                Agency((string[])ViewState["Agency"]);
            }
        }

        #region Button事件
        protected void btnM11_Click(object sender, EventArgs e)
        {
            pn_Button.Controls.Clear();
            Buttonevent("M11");
            ViewState["Field"] = "M11";
            string[] M11 = { "A","C","E","F","M","S","W" };
            ViewState["Agency"] = M11;
            ViewState["OVC_SYSTEM"] = 1;        
            ViewState["OVC_PURCH_KIND"] = 1;
            Controls_Attributes("disabled", "true", btnM21, btnM3all, btnF3all, btnW3all, btnM4all,
                btnF4all, btnW4all, btnD5all, btnF5all, btnW5all, btnA1all, btnA2all, btnA3all);
        }

        protected void btnM21_Click(object sender, EventArgs e)
        {
            pn_Button.Controls.Clear();
            Buttonevent("M21");
            string[] M21 = { "A", "C", "E", "F", "M", "S", "W" };
            ViewState["Agency"] = M21;
            ViewState["Field"] = "M21";
            ViewState["OVC_SYSTEM"] = 1;
            ViewState["OVC_PURCH_KIND"] = 1;
            Controls_Attributes("disabled", "true", btnM11, btnM3all, btnF3all, btnW3all, btnM4all,
                btnF4all, btnW4all, btnD5all, btnF5all, btnW5all, btnA1all, btnA2all, btnA3all);
        }

        protected void btnM3all_Click(object sender, EventArgs e)
        {
            pn_Button.Controls.Clear();
            pn_CheckBox.Controls.Clear();
            Buttonevent("M3");
            ViewState["Field"] = "M3";
            string[] M3= { "B", "L", "P" };
            Agency(M3);
            ViewState["Agency"] = M3;
            ViewState["OVC_SYSTEM"] = 1;
            ViewState["OVC_PURCH_KIND"] = 1;
            Controls_Attributes("disabled", "true", btnM11, btnM21, btnF3all, btnW3all, btnM4all,
                btnF4all, btnW4all, btnD5all, btnF5all, btnW5all, btnA1all, btnA2all, btnA3all);
        }

        protected void btnF3all_Click(object sender, EventArgs e)
        {
            pn_Button.Controls.Clear();
            pn_CheckBox.Controls.Clear();
            Buttonevent("F3");
            ViewState["Field"] = "F3";
            string[] F3 = { "M", "S" };
            Agency(F3);
            ViewState["Agency"] = F3;
            ViewState["OVC_SYSTEM"] = 1;
            ViewState["OVC_PURCH_KIND"] = 2;
            Controls_Attributes("disabled", "true", btnM11, btnM21, btnM3all, btnW3all, btnM4all,
                btnF4all, btnW4all, btnD5all, btnF5all, btnW5all, btnA1all, btnA2all, btnA3all);
        }

        protected void btnW3all_Click(object sender, EventArgs e)
        {
            pn_Button.Controls.Clear();
            pn_CheckBox.Controls.Clear();
            Buttonevent("W3");
            ViewState["Field"] = "W3";
            string[] W3 = { "A", "C", "E", "F", "W" };
            Agency(W3);
            ViewState["Agency"] = W3;
            ViewState["OVC_SYSTEM"] = 1;
            ViewState["OVC_PURCH_KIND"] = 2;
            Controls_Attributes("disabled", "true", btnM11, btnM21, btnM3all, btnF3all, btnM4all,
                btnF4all, btnW4all, btnD5all, btnF5all, btnW5all, btnA1all, btnA2all, btnA3all);
        }

        protected void btnM4all_Click(object sender, EventArgs e)
        {
            pn_Button.Controls.Clear();
            pn_CheckBox.Controls.Clear();
            Buttonevent("M4");
            ViewState["Field"] = "M4";
            string[] M4 = { "B", "L", "P" };
            Agency(M4);
            ViewState["Agency"] = M4;
            ViewState["OVC_SYSTEM"] = 1;
            ViewState["OVC_PURCH_KIND"] = 1;
            Controls_Attributes("disabled", "true", btnM11, btnM21, btnM3all, btnF3all, btnW3all,
                btnF4all, btnW4all, btnD5all, btnF5all, btnW5all, btnA1all, btnA2all, btnA3all);
        }

        protected void btnF4all_Click(object sender, EventArgs e)
        {
            pn_Button.Controls.Clear();
            pn_CheckBox.Controls.Clear();
            Buttonevent("F4");
            ViewState["Field"] = "F4";
            string[] F4 = { "M", "S" };
            Agency(F4);
            ViewState["Agency"] = F4;
            ViewState["OVC_SYSTEM"] = 1;
            ViewState["OVC_PURCH_KIND"] = 2;
            Controls_Attributes("disabled", "true", btnM11, btnM21, btnM3all, btnF3all, btnW3all,
                btnM4all, btnW4all, btnD5all, btnF5all, btnW5all, btnA1all, btnA2all, btnA3all);
        }

        protected void btnW4all_Click(object sender, EventArgs e)
        {
            pn_Button.Controls.Clear();
            pn_CheckBox.Controls.Clear();
            Buttonevent("W4");
            ViewState["Field"] = "W4";
            string[] W4 = { "A", "C", "E", "F", "W" };
            Agency(W4);
            ViewState["Agency"] = W4;
            ViewState["OVC_SYSTEM"] = 1;
            ViewState["OVC_PURCH_KIND"] = 2;
            Controls_Attributes("disabled", "true", btnM11, btnM21, btnM3all, btnF3all, btnW3all,
                btnM4all, btnF4all, btnD5all, btnF5all, btnW5all, btnA1all, btnA2all, btnA3all);
        }

        protected void btnD5all_Click(object sender, EventArgs e)
        {
            pn_Button.Controls.Clear();
            pn_CheckBox.Controls.Clear();
            Buttonevent("D5");
            ViewState["Field"] = "D5";
            string[] D5 = { "B", "L", "P" };
            Agency(D5);
            ViewState["Agency"] = D5;
            ViewState["OVC_SYSTEM"] = 1;
            ViewState["OVC_PURCH_KIND"] = 1;
            Controls_Attributes("disabled", "true", btnM11, btnM21, btnM3all, btnF3all, btnW3all,
                btnM4all, btnF4all, btnW4all, btnF5all, btnW5all, btnA1all, btnA2all, btnA3all);
        }

        protected void btnF5all_Click(object sender, EventArgs e)
        {
            pn_Button.Controls.Clear();
            pn_CheckBox.Controls.Clear();
            Buttonevent("F5");
            ViewState["Field"] = "F5";
            string[] F5 = { "M", "S" };
            Agency(F5);
            ViewState["Agency"] = F5;
            ViewState["OVC_SYSTEM"] = 1;
            ViewState["OVC_PURCH_KIND"] = 2;
            Controls_Attributes("disabled", "true", btnM11, btnM21, btnM3all, btnF3all, btnW3all,
                btnM4all, btnF4all, btnW4all, btnD5all, btnW5all, btnA1all, btnA2all, btnA3all);
        }

        protected void btnW5all_Click(object sender, EventArgs e)
        {
            pn_Button.Controls.Clear();
            pn_CheckBox.Controls.Clear();
            Buttonevent("W5");
            ViewState["Field"] = "W5";
            string[] W5 = { "A", "C", "E", "F", "W" };
            Agency(W5);
            ViewState["Agency"] = W5;
            ViewState["OVC_SYSTEM"] = 1;
            ViewState["OVC_PURCH_KIND"] = 2;
            Controls_Attributes("disabled", "true", btnM11, btnM21, btnM3all, btnF3all, btnW3all,
                btnM4all, btnF4all, btnW4all, btnD5all, btnF5all, btnA1all, btnA2all, btnA3all);
        }

        protected void btnA1all_Click(object sender, EventArgs e)
        {
            pn_Button.Controls.Clear();
            pn_CheckBox.Controls.Clear();
            Buttonevent("A1");
            ViewState["Field"] = "A1";
            string[] A1 = { "B", "L", "P" };
            Agency(A1);
            ViewState["Agency"] = A1;
            ViewState["OVC_SYSTEM"] = 1;
            ViewState["OVC_PURCH_KIND"] = 1;
            Controls_Attributes("disabled", "true", btnM11, btnM21, btnM3all, btnF3all, btnW3all,
                btnM4all, btnF4all, btnW4all, btnD5all, btnF5all, btnW5all, btnA2all, btnA3all);
        }

        protected void btnA2all_Click(object sender, EventArgs e)
        {
            pn_Button.Controls.Clear();
            pn_CheckBox.Controls.Clear();
            Buttonevent("A2");
            ViewState["Field"] = "A2";
            string[] A2 = { "A", "C", "E", "F", "W" };
            Agency(A2);
            ViewState["Agency"] = A2;
            ViewState["OVC_SYSTEM"] = 2;
            ViewState["OVC_PURCH_KIND"] = 2;
            Controls_Attributes("disabled", "true", btnM11, btnM21, btnM3all, btnF3all, btnW3all,
                btnM4all, btnF4all, btnW4all, btnD5all, btnF5all, btnW5all, btnA1all, btnA3all);
        }

        protected void btnA3all_Click(object sender, EventArgs e)
        {
            pn_Button.Controls.Clear();
            pn_CheckBox.Controls.Clear();
            Buttonevent("A3");
            ViewState["Field"] = "A3";
            string[] A3 = { "M" };
            Agency(A3);
            ViewState["Agency"] = A3;
            ViewState["OVC_SYSTEM"] = 2;
            ViewState["OVC_PURCH_KIND"] = 2;
            Controls_Attributes("disabled", "true", btnM11, btnM21, btnM3all, btnF3all, btnW3all,
                btnM4all, btnF4all, btnW4all, btnD5all, btnF5all, btnW5all, btnA1all, btnA2all);
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Gridevent(btn.ID.ToString());
            ViewState["BtnId"] = btn.ID.ToString();
            ViewState["isCreate"] = true;
        }

        protected void btnResetTop_Click(object sender, EventArgs e)
        {
            Attributes_Remove("disabled", btnM11, btnM21, btnM3all, btnF3all, btnW3all,
                btnM4all, btnF4all, btnW4all, btnD5all, btnF5all, btnW5all, btnA1all, btnA2all, btnA3all);
            pn_CheckBox.Controls.Clear();
            pn_Button.Controls.Clear();
            ViewState["hasRows"] = FCommon.GridView_setEmpty(GV_TBMStandardItem);
            ViewState.Remove("Field");
            ViewState.Remove("Agency");
            ViewState.Remove("OVC_SYSTEM");
            ViewState.Remove("OVC_PURCH_KIND");
            ViewState.Remove("BtnId");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ViewState["hasRows"] = FCommon.GridView_setEmpty(GV_TBMStandardItem);
            ViewState.Remove("BtnId");
        }
        #endregion

        protected void GV_TBMStandardItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBMStandardItem.DataKeys[gvrIndex].Value.ToString();
            string iKind = ViewState["BtnId"].ToString();
            switch (e.CommandName)
            {
                case "DataModify":
                    txtOVC_MEMO.Text = GV_TBMStandardItem.Rows[gvrIndex].Cells[2].Text;
                    if(GV_TBMStandardItem.Rows[gvrIndex].Cells[4].Text == null)
                        txtOVC_DESC.Text = " ";
                    else
                        txtOVC_DESC.Text = GV_TBMStandardItem.Rows[gvrIndex].Cells[4].Text;
                    if(GV_TBMStandardItem.Rows[gvrIndex].Cells[3].Text != null)
                    {
                        string str = GV_TBMStandardItem.Rows[gvrIndex].Cells[3].Text;
                        string[] arr = (string[])ViewState["Agency"];
                        for (int i = 0; i < arr.Length; i++)
                        {
                            CheckBox CB = (CheckBox)pn_CheckBox.FindControl(arr[i].ToString());
                            CB.Checked = false;
                        }

                        for (int i = 0 ; i < str.Length ; i++)
                        {
                            for(int j = 0 ; j < arr.Length ; j++)
                            {
                                if( arr[j].ToString() == str.Substring(i, 1))
                                {
                                    CheckBox CB = (CheckBox)pn_CheckBox.FindControl(arr[j].ToString());
                                    CB.Checked = true;
                                }
                            }
                        }
                    }
                    ViewState["OVC_NO"] = id;
                    ViewState["isCreate"] = false;
                    break;
                case "DataDel":
                    string strOVC_MEMO = GV_TBMStandardItem.Rows[gvrIndex].Cells[2].Text;
                    string strtxtOVC_DESC = GV_TBMStandardItem.Rows[gvrIndex].Cells[4].Text;
                    string strtxtOVC_AGENCY = GV_TBMStandardItem.Rows[gvrIndex].Cells[3].Text;
                    int OVC_NO =  Int32.Parse(id);
                    TBMSTANDARDITEM dStandrad = new TBMSTANDARDITEM();
                    dStandrad = ae.TBMSTANDARDITEMs.Where(table => table.OVC_IKIND.Equals(iKind) && table.OVC_NO==OVC_NO && table.OVC_PUR_AGENCY.Equals(strtxtOVC_AGENCY)).FirstOrDefault();
                    ae.Entry(dStandrad).State = EntityState.Deleted;
                    ae.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), dStandrad.GetType().Name.ToString(), this, "刪除");
                    FCommon.AlertShow(PnMessage_Save, "success", "系統訊息", "刪除成功");
                    Gridevent(iKind);
                    break;
                default:
                    break;
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strOVC_MEMO = txtOVC_MEMO.Text;
            string strOVC_DESC = txtOVC_DESC.Text;
            string arrOVC_PUR_AGENCY = "";
            string[] arr = (string[])ViewState["Agency"];
            string strOVC_IKIND = "";
            byte numOVC_SYSTEM = 0;
            byte numOVC_PURCH_KIND = 0;
            if (ViewState["BtnId"] == null)
                FCommon.AlertShow(PnMessage_Save, "danger", "系統訊息", "請先 選擇欲修改的種類");
            else
            {
                strOVC_IKIND = ViewState["BtnId"].ToString();
                numOVC_SYSTEM = byte.Parse(ViewState["OVC_SYSTEM"].ToString());
                numOVC_PURCH_KIND = byte.Parse(ViewState["OVC_PURCH_KIND"].ToString());
                var queryLastRecord =
                    (from tableSys in ae.TBMSTANDARDITEMs
                     where tableSys.OVC_IKIND == strOVC_IKIND
                     where tableSys.OVC_SYSTEM == numOVC_SYSTEM
                     where tableSys.OVC_PURCH_KIND == numOVC_PURCH_KIND
                     orderby tableSys.OVC_NO descending
                     select new
                     {
                         LastRecord = tableSys.OVC_NO,
                     }).ToList().FirstOrDefault();
                int intOVC_NO = queryLastRecord.LastRecord;

                if (strOVC_MEMO.Equals(string.Empty))
                    strMessage += "<P> 請輸入 標準用語 </p>";

                if (strMessage.Equals(string.Empty))
                {
                    if ((bool)ViewState["isCreate"])
                    {   //新增
                        TBMSTANDARDITEM codeTable = new TBMSTANDARDITEM();
                        codeTable.OVC_MEMO = strOVC_MEMO;
                        codeTable.OVC_DESC = strOVC_DESC;
                        for (int i = 0; i < arr.Length; i++)
                        {
                            CheckBox CB = (CheckBox)pn_CheckBox.FindControl(arr[i].ToString());
                            if (CB.Checked == true)
                                arrOVC_PUR_AGENCY += arr[i].ToString();
                        }
                        codeTable.OVC_PUR_AGENCY = arrOVC_PUR_AGENCY;
                        codeTable.OVC_IKIND = strOVC_IKIND;
                        codeTable.OVC_SYSTEM = numOVC_SYSTEM;
                        codeTable.OVC_PURCH_KIND = numOVC_PURCH_KIND;
                        intOVC_NO = intOVC_NO + 1;
                        codeTable.OVC_NO = (short)intOVC_NO;

                        ae.TBMSTANDARDITEMs.Add(codeTable);
                        ae.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), codeTable.GetType().Name.ToString(), this, "新增");

                        FCommon.AlertShow(PnMessage_Save, "success", "系統訊息", "新增成功");
                        FCommon.Controls_Clear(txtOVC_MEMO, txtOVC_DESC);
                        Gridevent(strOVC_IKIND);
                    }
                    else
                    {   //修改
                        int OVC_NO = Int32.Parse(ViewState["OVC_NO"].ToString());
                        TBMSTANDARDITEM codeTable = new TBMSTANDARDITEM();
                        codeTable = ae.TBMSTANDARDITEMs
                            .Where(table => table.OVC_IKIND == strOVC_IKIND)
                            .Where(table => table.OVC_SYSTEM == numOVC_SYSTEM)
                            .Where(table => table.OVC_PURCH_KIND == numOVC_PURCH_KIND)
                            .Where(table => table.OVC_NO == OVC_NO)
                            .FirstOrDefault();
                        if (codeTable != null)
                        {
                            codeTable.OVC_MEMO = strOVC_MEMO;
                            codeTable.OVC_DESC = strOVC_DESC;
                            for (int i = 0; i < arr.Length; i++)
                            {
                                CheckBox CB = (CheckBox)pn_CheckBox.FindControl(arr[i].ToString());
                                if (CB.Checked == true)
                                    arrOVC_PUR_AGENCY += arr[i].ToString();
                            }
                            codeTable.OVC_PUR_AGENCY = arrOVC_PUR_AGENCY;

                            ae.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), codeTable.GetType().Name.ToString(), this, "修改");

                            FCommon.AlertShow(PnMessage_Save, "success", "系統訊息", "修改成功");
                            Gridevent(strOVC_IKIND);
                            FCommon.Controls_Clear(txtOVC_MEMO, txtOVC_DESC);
                            ViewState.Remove("OVC_NO");
                            ViewState["isCreate"] = true;
                        }
                        else
                            FCommon.AlertShow(PnMessage_Save, "danger", "系統訊息", "標準用語 不存在");
                    }
                }
                else
                    FCommon.AlertShow(PnMessage_Save, "danger", "系統訊息", strMessage);
            }
        }

        protected void GV_TBMStandardItem_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);

            int i = 1;
            if (GV_TBMStandardItem.Rows.Count > 0)
            {
                foreach (GridViewRow gvItem in GV_TBMStandardItem.Rows)
                {
                    if (gvItem.RowIndex != 0)
                    {
                        //比對如果名稱如果相同就合併(RowSpan+1)
                        if (gvItem.Cells[0].Text.Trim() == GV_TBMStandardItem.Rows[(gvItem.RowIndex - i)].Cells[0].Text.Trim())
                        {
                            GV_TBMStandardItem.Rows[(gvItem.RowIndex - i)].Cells[0].RowSpan += 1;
                            gvItem.Cells[0].Visible = false;
                            i = i + 1;
                        }
                        else
                        {
                            GV_TBMStandardItem.Rows[(gvItem.RowIndex)].Cells[0].RowSpan += 1;
                            i = 1;
                        }
                    }
                    else
                    {
                        gvItem.Cells[0].RowSpan = 1;
                    }
                }
            }
            
        }
    }
}