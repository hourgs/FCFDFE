using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.IO;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D41 : System.Web.UI.Page
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
                        data();
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

        protected void btnMake_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_D37.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString() + "&ONB_GROUP=" + ViewState["strONB_GROUP"].ToString());
        }

        protected void btnCkeck_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_D22.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString() + "&ONB_GROUP=" + ViewState["strONB_GROUP"].ToString());
        }

        protected void btnDistribution_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_D40.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString() + "&ONB_GROUP=" + ViewState["strONB_GROUP"].ToString());
        }

        protected void btnMove_Click(object sender, EventArgs e)
        {
            string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(ViewState["strOVC_PURCH"].ToString()));
            string strOVC_PURCH_5 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(ViewState["strOVC_PURCH_5"].ToString()));

            Response.Redirect("MPMS_D36.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_PURCH_5=" + strOVC_PURCH_5);
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //按下上傳 => 檔案上傳
            if (FileUpload.HasFile)
            {
                string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/Contract/" + ViewState["strOVC_PURCH"].ToString()));
                string fileName = FileUpload.FileName;
                string serverFilePath = Path.Combine(serverDir, fileName);
                if (!Directory.Exists(serverDir))
                    Directory.CreateDirectory(serverDir);   //新增資料夾
                try
                {
                    FileUpload.SaveAs(serverFilePath);
                    GvFilesImport();
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

        protected void btnReturnS_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_D36.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString());
        }

        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_D14_1.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString());
        }

        protected void gvFiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteFile")
            {
                //按下 刪除
                GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
                int gvrIndex = gvr.RowIndex;
                string fileName = ((Label)gvFiles.Rows[gvrIndex].FindControl("lblFileName")).Text;  //((HyperLink)gvFiles.Rows[gvrIndex].Cells[1].Controls[0]).Text;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('確定刪除 " + fileName + " ?');", true);
                string filePath = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/Contract/" + ViewState["strOVC_PURCH"].ToString() + "/" + fileName));
                
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                GvFilesImport();
            }
        }

        public void GvFilesImport()
        {
            //資料夾檔案內容寫入GV
            DataTable dtFile = new DataTable();
            dtFile.Columns.Add("FileName", typeof(System.String));
            dtFile.Columns.Add("LinkFileName", typeof(System.String));
            dtFile.Columns.Add("Time", typeof(System.DateTime));
            dtFile.Columns.Add("FileSize", typeof(System.Int32));

            string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/Contract/" + ViewState["strOVC_PURCH"].ToString()));
            if (!Directory.Exists(serverDir))
            {
                Directory.CreateDirectory(serverDir);  //新增資料夾
            }
            DirectoryInfo filePath = new DirectoryInfo(serverDir);
            FileInfo[] fileList = filePath.GetFiles();  //擷取目錄下所有檔案內容，並存到 FileInfo Array
            foreach (FileInfo file in fileList)
            {
                string strFilePath = String.Format("<a href='/Uploadfile/MPMS/D/" + ViewState["strOVC_PURCH"].ToString() + "/" + file.Name + "' target='_blank'>" + file.Name + "</a>");
                dtFile.Rows.Add(file.Name, strFilePath, file.CreationTime, file.Length / 1024);  //file.Length/1024 = KB
            }
            gvFiles.DataSource = dtFile;
            gvFiles.DataBind();

            if (dtFile == null || dtFile.Rows.Count <= 0)
            {
                string[] fileField = { "FileName", "LinkFileName", "Time", "FileSize", "isOVC_NAME" };
                FCommon.GridView_setEmpty(gvFiles, fileField);
            }
        }




        private void data()
        {
            string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
            string strOVC_PURCH_5_url = Request.QueryString["OVC_PURCH_5"];
            string strONB_GROUP_url = Request.QueryString["ONB_GROUP"];
            string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
            string strOVC_PURCH_5 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_5_url));
            string strONB_GROUP = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strONB_GROUP_url));

            int onbgroup = Convert.ToInt32(strONB_GROUP);
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            var query1302 =
            (from tbm1302 in mpms.TBM1302
             where tbm1302.OVC_PURCH == strOVC_PURCH
             where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
             where tbm1302.ONB_GROUP == onbgroup
             select tbm1302).FirstOrDefault();
            string strOVC_PURCH_6 = query1302 == null ? "" : query1302.OVC_PURCH_6;
            lblOVC_PURCH.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5 + strOVC_PURCH_6;
            string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH));
            string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH_5));
            string key3 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strONB_GROUP));
            ViewState["strOVC_PURCH"] = key;
            ViewState["strOVC_PURCH_5"] = key2;
            ViewState["strONB_GROUP"] = key3;
        }
    }
}