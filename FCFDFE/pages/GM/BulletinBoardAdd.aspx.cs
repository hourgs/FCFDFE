using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using System.Collections;

namespace FCFDFE.pages.GM
{
    public partial class BulletinBoardAdd : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities GME = new GMEntities();
        string strCheckBoxID = "chk";

        #region 副程式
        private void AddControls()
        {
            var query =
                from tableSys in GME.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("SA"))
                select new
                {
                    Value = tableSys.OVC_PHR_ID,
                    Field = tableSys.OVC_PHR_DESC
                };
            DataTable dt_System = CommonStatic.LinqQueryToDataTable(query);

            for (int i = 0; i < dt_System.Rows.Count; i++)
            {
                DataRow dr_System = dt_System.Rows[i];
                string strSysId = dr_System["Value"].ToString();
                string strSysName = dr_System["Field"].ToString();

                var queryItem =
                    from tableRole in GME.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("S5")).AsEnumerable()
                    where tableRole.OVC_PHR_PARENTS.Equals(strSysId)
                    join tableAuth in GME.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("S6")).AsEnumerable() on tableRole.OVC_PHR_ID equals tableAuth.OVC_PHR_PARENTS
                    select new
                    {
                        //System = tableSys.OVC_PHR_DESC,
                        //Role = tableRole.OVC_PHR_DESC,
                        //Auth = tableAuth.OVC_PHR_DESC,
                        Value = tableAuth.OVC_PHR_ID,
                        Field = tableRole.OVC_PHR_DESC + "&nbsp;&nbsp;" + tableAuth.OVC_PHR_DESC,
                    };
                DataTable dt_Item = CommonStatic.LinqQueryToDataTable(queryItem);
                if (dt_Item.Rows.Count > 0)
                {
                    Label lblSystem = new Label();
                    lblSystem.CssClass = "lbl-system";
                    lblSystem.Text = strSysName;

                    pn_CheckBox.Controls.Add(lblSystem);

                    CheckBoxList theCheckBox = new CheckBoxList();
                    FCommon.list_dataImport(theCheckBox, dt_Item, "Field", "Value", false);
                    theCheckBox.ID = strCheckBoxID + dr_System["Value"].ToString();
                    theCheckBox.CssClass = "radioButton chk-item";
                    theCheckBox.RepeatColumns = 1;
                    //水平
                    theCheckBox.RepeatLayout = RepeatLayout.Flow;
                    theCheckBox.RepeatDirection = RepeatDirection.Horizontal;
                    pn_CheckBox.Controls.Add(theCheckBox);
                }
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtEND_DATE);
                }
                AddControls();
            }
        }

        protected void chk_allcheck_CheckedChanged(object sender, EventArgs e)
        {
            bool boolChkAll = chk_allcheck.Checked;
            var query =
            from tableSys in GME.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("SA"))
            select new
            {
                Value = tableSys.OVC_PHR_ID,
                Field = tableSys.OVC_PHR_DESC
            };
            DataTable dt_System = CommonStatic.LinqQueryToDataTable(query);

            for (int i = 0; i < dt_System.Rows.Count; i++)
            {
                DataRow dr_System = dt_System.Rows[i];
                string strSysId = dr_System["Value"].ToString();
                string strSysName = dr_System["Field"].ToString();
                string strChkId = strCheckBoxID + dr_System["Value"].ToString();
                CheckBoxList theCheckBox = (CheckBoxList)pn_CheckBox.FindControl(strChkId);
                for (int j = 0; j < theCheckBox.Items.Count; j++)
                {
                    theCheckBox.Items[j].Selected = boolChkAll == true ? true : false;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strUserId = Session["userid"].ToString();
            string strONB_NO = "001"; //預設編號為001開始
            string strSTATUS = drpSTATUS.SelectedValue;
            string strTITLE = txtTITLE.Text;
            string strCONTEXT = txtCONTEXT.Text;
            string strEND_DATE = txtEND_DATE.Text;
            ArrayList aryGROUP_ID = new ArrayList(); //已選取清單

            DateTime dateNow = DateTime.Now;
            #region 產生編號
            string strDateNow = dateNow.ToString("yyMMdd");
            BILLBOARD billLast = GME.BILLBOARDs.Where(table => table.ONB_NO.Substring(0, 6).Equals(strDateNow))
                .OrderByDescending(table => table.ONB_NO).FirstOrDefault(); //抓取今日最後一筆 ONB_NO
            if (billLast != null)
            {
                string strONB_NO_Last = billLast.ONB_NO; //180720001
                if (strONB_NO_Last.Length == 9)
                {
                    strONB_NO_Last = strONB_NO_Last.Substring(6, 3);
                    int.TryParse(strONB_NO_Last, out int intONB_NO);
                    strONB_NO = (++intONB_NO).ToString().PadLeft(3, '0');
                }
            }
            strONB_NO = strDateNow + strONB_NO;
            #endregion
            #region 取得已選取群組
            var querySystem = from tableSys in GME.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("SA")) select tableSys; //OVC_PHR_ID
            DataTable dt_System = CommonStatic.LinqQueryToDataTable(querySystem);
            int intCount = dt_System.Rows.Count;
            for (int i = 0; i < intCount; i++)
            {
                DataRow dr_System = dt_System.Rows[i];
                CheckBoxList theCheckBox = (CheckBoxList)pn_CheckBox.FindControl(strCheckBoxID + dr_System["OVC_PHR_ID"].ToString());
                if (FCommon.Controls_isExist(theCheckBox))
                {
                    foreach (ListItem item in theCheckBox.Items)
                        if (item.Selected) aryGROUP_ID.Add(item.Value);
                }
            }
            #endregion
            BILLBOARD bill = GME.BILLBOARDs.Where(table => table.ONB_NO.Equals(strONB_NO)).FirstOrDefault();
            #region 錯誤訊息
            if (strONB_NO.Equals(string.Empty) || bill != null)
                strMessage += "<p> 編號 錯誤！ </p>";
            if (strSTATUS.Equals(string.Empty))
                strMessage += "<p> 請選擇 狀態！ </p>";
            //if (strEND_DATE.Equals(string.Empty))
            //    strMessage += "<p> 請選擇 結束日期！ </p>";
            if (strTITLE.Equals(string.Empty))
                strMessage += "<p> 請輸入 標題！ </p>";
            if (strCONTEXT.Equals(string.Empty))
                strMessage += "<p> 請輸入 內容！ </p>";
            if (aryGROUP_ID.Count == 0)
                strMessage += "<p> 請選取 閱讀群組！ </p>";
            bool boolEND_DATE = FCommon.checkDateTime(strEND_DATE, "", ref strMessage, out DateTime dateEND_DATE);

            //確認是否有選取群組
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                Guid guidBB_SN = Guid.NewGuid();
                #region 新增 BILLBOARD
                bill = new BILLBOARD();
                bill.ONB_NO = strONB_NO;
                bill.BB_SN = guidBB_SN;
                bill.TITLE = strTITLE;
                bill.AUTHOR_ID = strUserId; //發表者
                bill.STATUS = strSTATUS;
                bill.CONTEXT = strCONTEXT;
                bill.START_DATE = dateNow;
                if (boolEND_DATE) bill.END_DATE = dateEND_DATE; else bill.END_DATE = null;
                GME.BILLBOARDs.Add(bill);
                GME.SaveChanges();
                FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), bill.GetType().Name.ToString(), this, "新增");
                #endregion

                #region 新增 BILLBOARD_GROUP
                foreach (string strGROUP_ID in aryGROUP_ID)
                {
                    BILLBOARD_GROUP bill_group = new BILLBOARD_GROUP();
                    bill_group.BB_GRP_SN = Guid.NewGuid();
                    bill_group.GROUP_ID = strGROUP_ID;
                    bill_group.BB_SN = guidBB_SN;
                    GME.BILLBOARD_GROUP.Add(bill_group);
                    GME.SaveChanges();
                    FCommon.syslog_add(strUserId, Request.ServerVariables["REMOTE_ADDR"].ToString(), bill_group.GetType().Name.ToString(), this, "新增");
                }
                #endregion
                FCommon.AlertShow(PnMessage_Insert, "success", "系統訊息", $"標題：{ strTITLE } 之公告 新增成功。");
            }
            else
                FCommon.AlertShow(PnMessage_Insert, "danger", "系統訊息", strMessage);
        }
    }
}