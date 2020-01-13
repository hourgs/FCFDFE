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
using System.IO;

namespace FCFDFE.pages.MPMS.B
{
    public partial class MPMS_B13_3 : System.Web.UI.Page
    {
        bool isUpload;
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strPurchNum = "";
        string strOVC_IKIND = "";
        string[] strField = { "OVC_IKINDTEXT", "OVC_ATTACH_NAME", "OVC_FILE_NAME", "ONB_QTY", "ONB_PAGES" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this,out isUpload))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (Request.QueryString["PurchNum"] != null)
                {
                    strPurchNum = Request.QueryString["PurchNum"].ToString();
                    strOVC_IKIND = Request.QueryString["IKIND"].ToString();
                    if (strPurchNum != null)
                    {
                        if (!IsPostBack)
                        {
                            LoginScreen();
                            GVImport();
                        }
                    }
                }
                else
                {
                    Response.Redirect("MPMS_B11");
                }
            }
        }

        
        protected void GV_OVC_ISOURCE_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        #region onCllick
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            //回物資申請書編制作業按鈕功能
            string url = "~/pages/MPMS/B/MPMS_B13.aspx?PurchNum=" + Request.QueryString["PurchNum"].ToString();
            Response.Redirect(url);
        }
        
        protected void btn_change_Click(object sender, EventArgs e)
        {
            //異動按鈕
            Button btnThis = (Button)sender;
            var query = gm.TBM1407;
            //主件名稱
            string strOVC_IKIND_NAME = query.Where(table => table.OVC_PHR_CATE == "J8" && table.OVC_PHR_ID.Equals(strOVC_IKIND)).Select(table => table.OVC_PHR_DESC).FirstOrDefault();
            lblOVC_IKIND_NAME_Modify.Text = strOVC_IKIND_NAME;
            //附件名稱
            DataTable dtOVC_ATTACH_NAME = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "K3").ToList());
            FCommon.list_dataImportV(drpOVC_ATTACH_NAME_Modify, dtOVC_ATTACH_NAME, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            int gvRowIndex = (btnThis.NamingContainer as GridViewRow).RowIndex;
            //找出GV的各項值
            string strOVC_ATTACH_NAME = GV_OVC_ISOURCE.Rows[gvRowIndex].Cells[2].Text;
            string strONB_QTY = GV_OVC_ISOURCE.Rows[gvRowIndex].Cells[3].Text;
            string strONB_Pages = GV_OVC_ISOURCE.Rows[gvRowIndex].Cells[5].Text;
            
            ViewState["OVC_ATTACH_NAME"] = strOVC_ATTACH_NAME;
           
            string queryATTACHNAME = dtOVC_ATTACH_NAME.AsEnumerable().Where(o => o.Field<string>("OVC_PHR_DESC").Equals(strOVC_ATTACH_NAME)).Select(o=>o.Field<string>("OVC_PHR_ID")).FirstOrDefault();

            if (queryATTACHNAME != null)
                drpOVC_ATTACH_NAME_Modify.SelectedValue = queryATTACHNAME;
            txtOVC_ATTACH_NAME_Modify.Text = strOVC_ATTACH_NAME;

            txtONB_QTY_Modify.Text= strONB_QTY;
            txtONB_PAGES_Modify.Text = strONB_Pages;

            tbModify.Visible = true;
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            //刪除
            Button btnThis = (Button)sender;
            int gvRowIndex = (btnThis.NamingContainer as GridViewRow).RowIndex;
            HiddenField hidThis = (HiddenField)GV_OVC_ISOURCE.Rows[gvRowIndex].Cells[4].FindControl("hidOVC_FILE_NAME");
            string strOVC_FILE_NAME = hidThis.Value;
            string strOVC_ATTACH_NAME = GV_OVC_ISOURCE.Rows[gvRowIndex].Cells[2].Text;
            TBM1119 tbm1119 = new TBM1119();//購案附件檔
            tbm1119 = mpms.TBM1119.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_IKIND.Equals(strOVC_IKIND) 
                                            && o.OVC_ATTACH_NAME.Equals(strOVC_ATTACH_NAME)).FirstOrDefault();
            if (tbm1119 != null)
            {
                mpms.Entry(tbm1119).State = EntityState.Deleted;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , tbm1119.GetType().Name.ToString(), this, "刪除");
                if (!strOVC_FILE_NAME.Equals(string.Empty))
                {
                    string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/B/" + strPurchNum));
                    string fileName = strOVC_FILE_NAME;
                    string serverFilePath = Path.Combine(serverDir, fileName);
                    File.Delete(serverFilePath);
                }
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
                
            }
            GVImport();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            //新增
            string strMessage = "";
            if (txtOVC_ATTACH_NAME.Text.Equals(string.Empty))
                strMessage += "<p> 請先 選擇或輸入附件名稱 </p>";
            if (string.IsNullOrEmpty(txtONB_QTY.Text))
                strMessage += "<p> 請先 輸入份數 </p>";
            if (string.IsNullOrEmpty(txtONB_PAGES.Text))
                strMessage += "<p> 請先 輸入頁數 </p>";

            if (strMessage.Equals(string.Empty))
            {
                TBM1119 tbm1119 = new TBM1119();
                tbm1119.OVC_PURCH = strPurchNum;
                tbm1119.OVC_IKIND = strOVC_IKIND;
                tbm1119.OVC_ATTACH_NAME = txtOVC_ATTACH_NAME.Text;
                tbm1119.ONB_QTY = byte.Parse(txtONB_QTY.Text);
                tbm1119.ONB_PAGES = byte.Parse(txtONB_PAGES.Text);
                mpms.TBM1119.Add(tbm1119);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , tbm1119.GetType().Name.ToString(), this, "新增");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功");
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
            GVImport();
        }

        protected void btnModify_Click(object sender, EventArgs e)
        {
            //修改存檔按鈕 因為會動到主key 不能直接update 要先刪再新增
            string strMessage = "";

            if (txtOVC_ATTACH_NAME_Modify.Text.Equals(string.Empty))
                strMessage += "<p> 請先 選擇或輸入附件名稱 </p>";
            if (string.IsNullOrEmpty(txtONB_QTY.Text))
                strMessage += "<p> 請先 輸入份數 </p>";
            if (string.IsNullOrEmpty(txtONB_PAGES.Text))
                strMessage += "<p> 請先 輸入頁數 </p>";

            string strOVC_ATTACH_NAME = ViewState["OVC_ATTACH_NAME"].ToString();

            if (strMessage.Equals(string.Empty))
            {
                //刪除
                string strOVC_ATTACH_NAME_Modify = txtOVC_ATTACH_NAME_Modify.Text;
                TBM1119 tbm1119 = new TBM1119();
                tbm1119 = mpms.TBM1119.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_IKIND.Equals(strOVC_IKIND)
                                              && o.OVC_ATTACH_NAME.Equals(strOVC_ATTACH_NAME)).FirstOrDefault();
                mpms.Entry(tbm1119).State = EntityState.Deleted;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , tbm1119.GetType().Name.ToString(), this, "刪除");
                //新增
                TBM1119 modTbm1119 = new TBM1119();
                modTbm1119.OVC_PURCH = strPurchNum;
                modTbm1119.OVC_IKIND = strOVC_IKIND;
                modTbm1119.OVC_ATTACH_NAME = strOVC_ATTACH_NAME;
                modTbm1119.ONB_QTY = byte.Parse(txtONB_QTY_Modify.Text);
                modTbm1119.ONB_PAGES = byte.Parse(txtONB_PAGES_Modify.Text);
                mpms.TBM1119.Add(modTbm1119);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
               , modTbm1119.GetType().Name.ToString(), this, "新增");

                FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                tbModify.Visible = false;
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
            GVImport();

        }

        protected void btnToUpload_Click(object sender, EventArgs e)
        {
            Button btnThis = (Button)sender;
            int gvRowIndex = (btnThis.NamingContainer as GridViewRow).RowIndex;
            //找出GV的各項值
            string strOVC_ATTACH_NAME = GV_OVC_ISOURCE.Rows[gvRowIndex].Cells[2].Text;

            ViewState["OVC_ATTACH_NAME_UPLOAD"] = strOVC_ATTACH_NAME;
            divUploadDetail.Visible = false;
            divFileUpload.Visible = true;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //檔案上傳
            if (isUpload)
            {
                if (FileUpload.HasFile)
                {
                    string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/B/" + strPurchNum));
                    if (!Directory.Exists(serverDir))
                    {
                        Directory.CreateDirectory(serverDir);
                    }
                    string fileName = FileUpload.FileName;
                    string serverFilePath = Path.Combine(serverDir, fileName);
                    string strOVC_ATTACH_NAME = ViewState["OVC_ATTACH_NAME_UPLOAD"].ToString();
                    try
                    {
                        FileUpload.SaveAs(serverFilePath);
                        TBM1119 tbm1119 = new TBM1119();
                        tbm1119 = mpms.TBM1119.Where(o => o.OVC_PURCH.Equals(strPurchNum) && o.OVC_IKIND.Equals(strOVC_IKIND)
                                                 && o.OVC_ATTACH_NAME.Equals(strOVC_ATTACH_NAME)).FirstOrDefault();
                        tbm1119.OVC_FILE_NAME = fileName;
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString()
                            , tbm1119.GetType().Name.ToString(), this, "修改");
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "檔案上傳成功");
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", error);
                    }
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先 選擇檔案");
            }
        }

        protected void btnBackDetail_Click(object sender, EventArgs e)
        {
            divFileUpload.Visible = false;
            divUploadDetail.Visible = true;
            GVImport();
        }

        protected void btn_downloadFile_Click(object sender, EventArgs e)
        {
            string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/B/" + strPurchNum));
            LinkButton btnThis = (LinkButton)sender;
            string serverFilePath = Path.Combine(serverDir, btnThis.Text);
            if (File.Exists(serverFilePath))
            {
                FileInfo file = new FileInfo(serverFilePath);
                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.ContentType = "application/octet-stream";
                //Response.BinaryWrite(buffer);
                Response.WriteFile(file.FullName);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.Flush();
                Response.End();
            }
            else
            {
                FCommon.AlertShow(PnMessage,"danger", "系統訊息", "檔案不存在！");
            }
        }

        #endregion

        #region onSelectIndexChange
        protected void drpOVC_ATTACH_NAME_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpOVC_ATTACH_NAME.SelectedItem.Text.Equals("請選擇"))
            {
                txtOVC_ATTACH_NAME.Text = "";
            }
            else
            {
                txtOVC_ATTACH_NAME.Text = drpOVC_ATTACH_NAME.SelectedItem.Text.Split(':')[1];
            }
            
        }

        protected void drpOVC_ATTACH_NAME_Modify_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOVC_ATTACH_NAME_Modify.Text = drpOVC_ATTACH_NAME_Modify.SelectedItem.Text.Split(':')[1];
        }


        #endregion

        #region 副程式
        private void LoginScreen()
        {
            var query = gm.TBM1407;
            lblOVC_PURCH.Text = strPurchNum;
            //主件名稱
            string strOVC_IKIND_NAME = query.Where(table => table.OVC_PHR_CATE == "J8" && table.OVC_PHR_ID.Equals(strOVC_IKIND)).Select(table => table.OVC_PHR_DESC).FirstOrDefault();
            lblOVC_IKIND_NAME.Text = strOVC_IKIND_NAME;
            lblTitle.Text = strOVC_IKIND_NAME;
            //附件名稱
            DataTable dtOVC_ATTACH_NAME = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "K3").ToList());
            FCommon.list_dataImportV(drpOVC_ATTACH_NAME, dtOVC_ATTACH_NAME, "OVC_PHR_DESC", "OVC_PHR_ID", ":", true);
            //份數
            txtONB_QTY.Text = "1";
            //頁數
            txtONB_PAGES.Text = "1";
            if (strOVC_IKIND.Equals("D"))
                btnReturn.Text = "回採購計畫清單編制作業";

        }

        
        private void GVImport(){
            //附件明細
            DataTable dt = new DataTable();
            var GVContent =
                from table1119 in mpms.TBM1119
                where table1119.OVC_PURCH.Equals(strPurchNum) && table1119.OVC_IKIND.Equals(strOVC_IKIND)
                select new
                {
                    OVC_PURCH = table1119.OVC_PURCH,
                    OVC_IKIND = table1119.OVC_IKIND,
                    OVC_IKINDTEXT = table1119.OVC_IKIND,
                    OVC_ATTACH_NAME = table1119.OVC_ATTACH_NAME,
                    OVC_FILE_NAME = table1119.OVC_FILE_NAME,
                    ONB_QTY = table1119.ONB_QTY,
                    ONB_PAGES = table1119.ONB_PAGES
                };
            
            var GVContentNew = 
                from t in GVContent.AsEnumerable()
                orderby t.OVC_ATTACH_NAME.Split('.')[0].Length, t.OVC_ATTACH_NAME
                select new
                {
                    OVC_PURCH = t.OVC_PURCH,
                    OVC_IKINDTEXT = t.OVC_IKIND == "D" ? "採購計畫清單" : "物資申請書",
                    OVC_ATTACH_NAME = t.OVC_ATTACH_NAME,
                    OVC_FILE_NAME = t.OVC_FILE_NAME,
                    ONB_QTY = t.ONB_QTY,
                    ONB_PAGES = t.ONB_PAGES
                };
            dt = CommonStatic.LinqQueryToDataTable(GVContentNew);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_OVC_ISOURCE, dt, strField);

        }
        #endregion
    }
}