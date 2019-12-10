using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.GM
{
    public partial class Phrases : Page
    {
        bool isCreate = true;
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities GME = new GMEntities();

        #region 副程式
        private void dataImport()
        {
            if (ViewState["Query"] != null)
            {
                string strQuery = ViewState["Query"].ToString();
                switch (strQuery)
                {
                    case "Query":
                        Query();
                        break;
                    case "PartQuery":
                        PartQuery();
                        break;
                }
            }
        }
        private void Query()
        {
            string strMainItem = drpMainItem.SelectedValue;
            DataTable dtSubItem = CommonStatic.ListToDataTable(GME.TBM1407.Where(table => table.OVC_PHR_CATE.Equals(strMainItem)).Where(table => table.OVC_PHR_CATE != "SP").ToList());
            ViewState["hasRows"] = FCommon.GridView_dataImport(GridView_SubItem, dtSubItem);
        }
        private void PartQuery()
        {
            string strOVC_PHR_DESC = txtOVC_PHR_DESC_Query.Text;
            string[] strOVC_PHR_DESCs = strOVC_PHR_DESC.Split(' ');

            var query =
                from codeTable in GME.TBM1407
                select codeTable;

            foreach (string value in strOVC_PHR_DESCs)
            {
                query = query.Where(table => table.OVC_PHR_DESC.Contains(value)).Where(table=>table.OVC_PHR_CATE!="SP");
            }
            DataTable dtSubItem = CommonStatic.ListToDataTable(query.ToList());

            ViewState["hasRows"] = FCommon.GridView_dataImport(GridView_SubItem, dtSubItem);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            if (!IsPostBack)
            {
                #region 下拉式選單匯入資料
                //系統別
                DataTable dtMainItem = CommonStatic.ListToDataTable(GME.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("00")).ToList());
                FCommon.list_dataImportV(drpMainItem, dtMainItem, "OVC_PHR_DESC", "OVC_PHR_ID", true);
                #endregion
                FCommon.GridView_setEmpty(GridView_SubItem);
            }

            if (ViewState["isCreate"] != null)
            {
                isCreate = Convert.ToBoolean(ViewState["isCreate"]);
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            Query();
            ViewState["Query"] = "Query";
            ViewState["isCreate"] = true;
            FCommon.Controls_Attributes("readonly", txtOVC_PHR_CATE, txtOVC_PHR_ID);
            FCommon.Controls_Clear(txtOVC_PHR_CATE, txtOVC_PHR_ID, txtOVC_PHR_DESC, txtOVC_PHR_REMARK, txtOVC_USR_ID);
        }

        protected void btnPartQuery_Click(object sender, EventArgs e)
        {
            PartQuery();
            ViewState["Query"] = "PartQuery";
            ViewState["isCreate"] = true;
            FCommon.Controls_Attributes("readonly", txtOVC_PHR_CATE, txtOVC_PHR_ID);
            FCommon.Controls_Clear(txtOVC_PHR_CATE, txtOVC_PHR_ID, txtOVC_PHR_DESC, txtOVC_PHR_REMARK, txtOVC_USR_ID);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strOVC_PHR_CATE = txtOVC_PHR_CATE.Text;
            string strOVC_PHR_ID = txtOVC_PHR_ID.Text;
            string strOVC_PHR_DESC = txtOVC_PHR_DESC.Text;
            string strOVC_PHR_REMARK = txtOVC_PHR_REMARK.Text;
            string strOVC_USR_ID = txtOVC_USR_ID.Text;

            #region 資料錯誤判斷
            var query =
                from codeTable in GME.TBM1407
                where codeTable.OVC_PHR_CATE.Equals(strOVC_PHR_CATE)
                where codeTable.OVC_PHR_ID.Equals(strOVC_PHR_ID)
                select codeTable;
            bool isExist = query.ToList().Count > 0;

            if (strOVC_PHR_CATE.Equals(string.Empty))
                strMessage += "<P> 請輸入 片語類別 </p>";
            if (strOVC_PHR_ID.Equals(string.Empty))
                strMessage += "<P> 請輸入 片語代碼 </p>";
            if (strOVC_PHR_DESC.Equals(string.Empty))
                strMessage += "<P> 請輸入 片語內容 </p>";
            if (isCreate && isExist)
                strMessage += "<P> 片語類別及代碼 已經存在 </p>";
            if (!isCreate && !isExist)
                strMessage += "<P> 片語類別及代碼 不存在 </p>";
            #endregion

            if (strMessage.Equals(string.Empty))
            {
                if (isCreate)
                {   //新增
                    TBM1407 codeTable = new TBM1407();
                    codeTable.OVC_PHR_CATE = strOVC_PHR_CATE;
                    codeTable.OVC_PHR_ID = strOVC_PHR_ID;
                    codeTable.OVC_PHR_DESC = strOVC_PHR_DESC;
                    codeTable.OVC_PHR_REMARK = strOVC_PHR_REMARK;
                    codeTable.OVC_USR_ID = strOVC_USR_ID;

                    GME.TBM1407.Add(codeTable);
                    GME.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), codeTable.GetType().Name.ToString(), this, "新增");

                    FCommon.AlertShow(PnMessage_Save, "success", "系統訊息", "新增成功");
                    dataImport();
                }
                else
                {   //修改
                    TBM1407 codeTable = new TBM1407();
                    codeTable = GME.TBM1407
                        .Where(table => table.OVC_PHR_CATE.Equals(strOVC_PHR_CATE))
                        .Where(table => table.OVC_PHR_ID.Equals(strOVC_PHR_ID))
                        .FirstOrDefault();
                    if (codeTable != null)
                    {
                        codeTable.OVC_PHR_DESC = strOVC_PHR_DESC;
                        codeTable.OVC_PHR_REMARK = strOVC_PHR_REMARK;
                        codeTable.OVC_USR_ID = strOVC_USR_ID;

                        GME.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), codeTable.GetType().Name.ToString(), this, "修改");

                        FCommon.AlertShow(PnMessage_Save, "success", "系統訊息", "修改成功");
                        dataImport();
                    }
                    else
                        FCommon.AlertShow(PnMessage_Save, "danger", "系統訊息", "片語類別及代碼 不存在");
                }
            }
            else
                FCommon.AlertShow(PnMessage_Save, "danger", "系統訊息", strMessage);
        }

        protected void GridView_SubItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GridView_SubItem.DataKeys[gvrIndex].Value.ToString();

            switch (e.CommandName)
            {
                case "DataModify":
                    txtOVC_PHR_CATE.Text = GridView_SubItem.Rows[gvrIndex].Cells[0].Text;
                    txtOVC_PHR_ID.Text = GridView_SubItem.Rows[gvrIndex].Cells[1].Text;
                    txtOVC_PHR_DESC.Text = GridView_SubItem.Rows[gvrIndex].Cells[2].Text;
                    txtOVC_PHR_REMARK.Text = GridView_SubItem.Rows[gvrIndex].Cells[3].Text;
                    txtOVC_USR_ID.Text = GridView_SubItem.Rows[gvrIndex].Cells[4].Text;

                    ViewState["isCreate"] = false;
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_PHR_CATE, txtOVC_PHR_ID);
                    break;
                case "DataDel":
                    string strOVC_PHR_CATE = GridView_SubItem.Rows[gvrIndex].Cells[0].Text;
                    string strOVC_PHR_ID = GridView_SubItem.Rows[gvrIndex].Cells[1].Text;
                    var model = new TBM1407 { OVC_PHR_CATE = strOVC_PHR_CATE, OVC_PHR_ID = strOVC_PHR_ID };
                    GME.Entry(model).State = EntityState.Deleted;
                    GME.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), model.GetType().Name.ToString(), this, "刪除");
                    dataImport();
                    break;
                default:
                    break;
            }
        }

        protected void GridView_SubItem_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
    }
}