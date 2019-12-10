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


namespace FCFDFE.pages.MPMS.B
{
    public partial class MPMS_B13_8 : System.Web.UI.Page
    {

        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        string strPurchNum ="";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (Request.QueryString["PurchNum"] != null)
                {
                    strPurchNum = Request.QueryString["PurchNum"].ToString();
                    lblOVC_PURCH.Text = strPurchNum;
                    if (!IsPostBack)
                    {
                        LoginScreen();
                    }
                }
                else
                {
                    Response.Redirect("MPMS_B11");
                }
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            //新增
            string strTYPENO = "";
            string strDOC_NO = "";
            Button btnThis = (Button)sender;
            int gvRowIndex = (btnThis.NamingContainer as GridViewRow).RowIndex;
            Button btn_modify = (Button)GV_DOCTYPE.Rows[gvRowIndex].Cells[3].FindControl("btn_modify");
            Button btn_delete = (Button)GV_DOCTYPE.Rows[gvRowIndex].Cells[3].FindControl("btn_delete");
            HiddenField hidTYPENO = (HiddenField)GV_DOCTYPE.Rows[gvRowIndex].Cells[3].FindControl("hidDOC_TYPENO");
            btnThis.CssClass += " disabled";
            btn_modify.CssClass= "btn-success btnw4";
            btn_delete.CssClass= "btn-danger btnw4";
            strTYPENO = hidTYPENO.Value;
            //查編號然後加一當作這筆編號 利用字串長度先排再用編號排即可得到正確排序
            strDOC_NO = mpms.TBMDOCPURCH.Where(o => o.DOC_TYPENO.Equals(strTYPENO))
                                                .OrderByDescending(o => o.DOC_NO.Length).ThenByDescending(o=>o.DOC_NO)
                                                .Select(o => o.DOC_NO).FirstOrDefault();
            /*if (query.Any())
                strDOC_NO = query;*/

            strDOC_NO = (int.Parse(strDOC_NO) + 1).ToString();
            //TBMDOCPURC新增
            TBMDOCPURCH tbmDOCPURCH = new TBMDOCPURCH();
            tbmDOCPURCH.DOC_TYPENO = strTYPENO;
            tbmDOCPURCH.DOC_NO = strDOC_NO;
            tbmDOCPURCH.PURCH_NO = strPurchNum;
            mpms.TBMDOCPURCH.Add(tbmDOCPURCH);
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , tbmDOCPURCH.GetType().Name.ToString(), this, "新增");
            //DOCLAW新增
            DataTable dtDOCLAW = new DataTable();
            var queryLAW = mpms.TBMDOCLAW.Where(o => o.DOC_TYPENO.Equals(strTYPENO) && o.DOC_NO.Equals("0")).DefaultIfEmpty();
            dtDOCLAW = CommonStatic.LinqQueryToDataTable(queryLAW);
            foreach(DataRow rows in dtDOCLAW.Rows)
            {
                TBMDOCLAW tbmDOCLAW = new TBMDOCLAW();
                tbmDOCLAW.DOC_TYPENO = rows["DOC_TYPENO"].ToString();
                tbmDOCLAW.DOC_NO = strDOC_NO;
                tbmDOCLAW.DOC_LAW_NO = rows["DOC_LAW_NO"].ToString();
                tbmDOCLAW.DOC_LAW_NAME = rows["DOC_LAW_NAME"].ToString();
                mpms.TBMDOCLAW.Add(tbmDOCLAW);
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , tbmDOCLAW.GetType().Name.ToString(), this, "新增");
            }
            mpms.SaveChanges();
            
            //DOCCONTENT新增
            DataTable dtDOCCONTENT = new DataTable();
            var queryCONTENT = mpms.TBMDOCCONTENT.Where(o => o.DOC_TYPENO.Equals(strTYPENO) && o.DOC_NO.Equals("0")).DefaultIfEmpty();
            dtDOCCONTENT = CommonStatic.LinqQueryToDataTable(queryCONTENT);
            foreach (DataRow rows in dtDOCCONTENT.Rows)
            {
                TBMDOCCONTENT tbmDOCCONTENT = new TBMDOCCONTENT();
                tbmDOCCONTENT.DOC_TYPENO = rows["DOC_TYPENO"].ToString();
                tbmDOCCONTENT.DOC_NO = strDOC_NO;
                tbmDOCCONTENT.DOC_LAW_NO = rows["DOC_LAW_NO"].ToString();
                tbmDOCCONTENT.DOC_CON_NO = rows["DOC_CON_NO"].ToString();
                tbmDOCCONTENT.DOC_CON = rows["DOC_CON"].ToString();
                tbmDOCCONTENT.DOC_CON_DESC = rows["DOC_CON_DESC"].ToString();
                tbmDOCCONTENT.DOC_CHECK = "0";
                tbmDOCCONTENT.DOC_COMMIT = "0";
                mpms.TBMDOCCONTENT.Add(tbmDOCCONTENT);
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , tbmDOCCONTENT.GetType().Name.ToString(), this, "新增");
            }
            mpms.SaveChanges();
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功");
            LoginScreen();
        }

        protected void btn_modify_Click(object sender, EventArgs e)
        {
            string strTYPENO = "";
            Button btnThis = (Button)sender;
            int gvRowIndex = (btnThis.NamingContainer as GridViewRow).RowIndex;
            HiddenField hidTYPENO = (HiddenField)GV_DOCTYPE.Rows[gvRowIndex].Cells[3].FindControl("hidDOC_TYPENO");
            strTYPENO = hidTYPENO.Value;
            var query = mpms.TBMDOCPURCH.Where(o => o.PURCH_NO.Equals(strPurchNum) && o.DOC_TYPENO.Equals(strTYPENO)).FirstOrDefault();
            string strDOCNO = query.DOC_NO;
            string url = "~/pages/MPMS/B/MPMS_B13_8_1.aspx?PurchNum=" + strPurchNum + "&DocTypeNO=" + strTYPENO + "&DocNO=" + strDOCNO;
            Response.Redirect(url);
        }

        protected void btn_delete_Click(object sender, EventArgs e)
        {
            //刪除
            string strTYPENO = "";
            Button btnThis = (Button)sender;
            int gvRowIndex = (btnThis.NamingContainer as GridViewRow).RowIndex;
            HiddenField hidTYPENO = (HiddenField)GV_DOCTYPE.Rows[gvRowIndex].Cells[3].FindControl("hidDOC_TYPENO");
            strTYPENO = hidTYPENO.Value;
            //查本筆DOC編號
            var query = mpms.TBMDOCPURCH.Where(o => o.PURCH_NO.Equals(strPurchNum) && o.DOC_TYPENO.Equals(strTYPENO)).FirstOrDefault();
            string strDOCNO = query.DOC_NO;
            //刪除DOCCONTENT
            mpms.TBMDOCCONTENT.RemoveRange(mpms.TBMDOCCONTENT.Where(o => o.DOC_TYPENO.Equals(strTYPENO) && o.DOC_NO.Equals(strDOCNO)));
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , "TBMDOCCONTENT", this, "刪除");
            //刪除DOCLAW
            mpms.TBMDOCLAW.RemoveRange(mpms.TBMDOCLAW.Where(o => o.DOC_TYPENO.Equals(strTYPENO) && o.DOC_NO.Equals(strDOCNO)));
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , "TBMDOCLAW", this, "刪除");
            //刪除DOCPURCH
            var queryDOCPURCH = mpms.TBMDOCPURCH.Where(o => o.PURCH_NO.Equals(strPurchNum)
                                                        && o.DOC_TYPENO.Equals(strTYPENO) && o.DOC_NO.Equals(strDOCNO));
            mpms.TBMDOCPURCH.RemoveRange(queryDOCPURCH);
            mpms.SaveChanges();
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , queryDOCPURCH.GetType().Name.ToString(), this, "刪除");
            Button btn_modify = (Button)GV_DOCTYPE.Rows[gvRowIndex].Cells[3].FindControl("btn_modify");
            Button btn_save = (Button)GV_DOCTYPE.Rows[gvRowIndex].Cells[3].FindControl("btn_save");
            btn_save.Enabled = true;
            btnThis.Enabled = false;
            btn_modify.Enabled = false;
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
            LoginScreen();
        }

        protected void btn_print_Click(object sender, EventArgs e)
        {

        }

        protected void btn_Return_Click(object sender, EventArgs e)
        {
            //回計畫清單
            string url = "~/pages/MPMS/B/MPMS_B13.aspx?PurchNum=" + Request.QueryString["PurchNum"];
            Response.Redirect(url);
        }

        private void LoginScreen()
        {
            //GV資料載入
            string[] strfield = { "DOC_NAME" , "PURCH_NO" , "DOC_TYPENO" };
            DataTable dt = new DataTable();
            var query =
                from table in mpms.TBMDOCTYPE
                join table2 in mpms.TBMDOCPURCH.Where(o=>o.PURCH_NO.Equals(strPurchNum)) 
                on table.DOC_TYPENO equals table2.DOC_TYPENO into table3
                from table2 in table3.DefaultIfEmpty()
                where table.DOC_LIVE.Equals("1")
                orderby table.DOC_TYPENO
                select new
                {
                    table.DOC_TYPENO,
                    table.DOC_NAME,
                    PURCH_NO = table2.PURCH_NO == strPurchNum ? "已建立": "未建立",
                    table2.DOC_NO
                };
            dt = CommonStatic.LinqQueryToDataTable(query);
            foreach(DataRow rows in dt.Rows)
            {
                var queryCOMMIT =
                        from table in mpms.TBMDOCCONTENT.AsEnumerable()
                        where table.DOC_TYPENO.Equals(rows["DOC_TYPENO"]) && table.DOC_NO.Equals(rows["DOC_NO"]) && table.DOC_COMMIT == "0"
                        select table;
                var queryIsExist = mpms.TBMDOCPURCH.AsEnumerable().Where(o => o.PURCH_NO.Equals(strPurchNum) && o.DOC_TYPENO.Equals(rows["DOC_TYPENO"]));

                if (queryIsExist.Any())
                {
                    if (queryCOMMIT.Any())
                        rows["DOC_NO"] = "未確認";
                    else
                        rows["DOC_NO"] = "已確認";
                }
                else
                {
                    rows["DOC_NO"] = "未確認";
                }
                
            }
            

            FCommon.GridView_dataImport(GV_DOCTYPE, dt, strfield);
            foreach(GridViewRow gvRows in  GV_DOCTYPE.Rows)
            {
                if (gvRows.Cells[1].Text.Equals("已建立"))
                {
                    Button btn_new = (Button)gvRows.FindControl("btn_save");
                    Button btn_modify = (Button)gvRows.FindControl("btn_modify");
                    Button btn_delete = (Button)gvRows.FindControl("btn_delete");
                    btn_new.CssClass += " disabled";
                    btn_modify.CssClass = "btn-success btnw4";
                    btn_delete.CssClass = "btn-danger btnw4";
                }
            }

        }
    }
}