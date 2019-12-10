using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System.Data;

namespace FCFDFE.pages.MPMS.B
{
    public partial class MPMS_B13_8_2 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        string strDocTypeNO = "";
        string strDocNo = "";
        string strLawNo = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (string.IsNullOrEmpty(Request.QueryString["DocTypeNO"]) ||
                    string.IsNullOrEmpty(Request.QueryString["DocNO"]) ||
                    string.IsNullOrEmpty(Request.QueryString["LawNo"]) ||
                    string.IsNullOrEmpty(Request.QueryString["PurchNum"]))
                {

                }
                else
                {
                    strDocTypeNO = Request.QueryString["DocTypeNO"].ToString();
                    strDocNo = Request.QueryString["DocNO"].ToString();
                    strLawNo = Request.QueryString["LawNo"].ToString();
                    if (!IsPostBack)
                    {
                        LoginScreen();
                        DataImport();
                    }
                }
            }
        }

        protected void btn_CommitSave_Click(object sender, EventArgs e)
        {
            SaveContent(sender, "1");
            DataImport();
        }

        protected void btn_TempSave_Click(object sender, EventArgs e)
        {
            SaveContent(sender, "0");
            DataImport();
        }

        protected void btn_Return_Click(object sender, EventArgs e)
        {
            string url = "~/pages/MPMS/B/MPMS_B13_8_1.aspx?PurchNum=" + Request.QueryString["PurchNum"] + "&DocTypeNO=" 
                + Request.QueryString["DocTypeNO"] + "&DocNO=" + Request.QueryString["DocNO"];
            Response.Redirect(url);
        }


        private void SaveContent(object sender,string commit)
        {
            //存檔
            Button btnThis = (Button)sender;
            int gvRowIndex = (btnThis.NamingContainer as GridViewRow).RowIndex;
            TextBox content = (TextBox)GV_DOCCONTENT.Rows[gvRowIndex].Cells[3].FindControl("txtDocContent");
            string conNo = GV_DOCCONTENT.Rows[gvRowIndex].Cells[0].Text;
            if (string.IsNullOrEmpty(content.Text))
            {
                TBMDOCCONTENT tbmDOCCONTENT = new TBMDOCCONTENT();
                tbmDOCCONTENT = mpms.TBMDOCCONTENT.Where(o => o.DOC_TYPENO.Equals(strDocTypeNO) && o.DOC_NO.Equals(strDocNo)
                                                                && o.DOC_LAW_NO.Equals(strLawNo) && o.DOC_CON_NO.Equals(conNo)).FirstOrDefault();
                tbmDOCCONTENT.DOC_CON = content.Text;
                tbmDOCCONTENT.DOC_COMMIT = commit;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                    , tbmDOCCONTENT.GetType().Name.ToString(), this, "修改");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
            }
            else
            {
                FCommon.AlertShow(PnMessage, "dnager", "系統訊息", "請先 輸入內容");
            }
            
        }

        private void LoginScreen()
        {
            string strLawName = mpms.TBMDOCLAW.Where(o => o.DOC_TYPENO.Equals(strDocTypeNO) && o.DOC_NO.Equals(strDocNo) && o.DOC_LAW_NO.Equals(strLawNo))
                                              .Select(o => o.DOC_LAW_NAME).First();
            lblDOCLAWNAME.Text = strLawName;
        }

       
        private void DataImport()
        {
            //資料載入 排序先用字串長度再用內容排 可以正確排序
            string[] strField = { "DOC_CON_NO", "DOC_COMMIT", "DOC_CON", "DOC_CON_DESC" }; 
            DataTable dt = new DataTable();
            var query =
                from table in mpms.TBMDOCCONTENT
                where table.DOC_TYPENO.Equals(strDocTypeNO) && table.DOC_NO.Equals(strDocNo) && table.DOC_LAW_NO.Equals(strLawNo)
                orderby table.DOC_CON_NO.Length, table.DOC_CON_NO
                select new
                {
                    table.DOC_CON_NO,
                    DOC_COMMIT = table.DOC_COMMIT=="1" ? "已確認" : "未完成",
                    table.DOC_CON,
                    table.DOC_CON_DESC
                };
            dt = CommonStatic.LinqQueryToDataTable(query);
           
            FCommon.GridView_dataImport(GV_DOCCONTENT, dt, strField);
        }
    }
}