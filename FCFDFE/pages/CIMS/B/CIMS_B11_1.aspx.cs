using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.CIMSModel;
using FCFDFE.Entity.GMModel;
using System.Data.Entity;
using System.IO;

namespace FCFDFE.pages.CIMS.B
{
    public partial class CIMS_B11_1 : System.Web.UI.Page
    {
        string[] strField = { "RANK", "OVC_PURCH", "OVC_ATTACH_NAME", "OVC_ITEM", "OVC_TIMES", "OVC_SUB", "OVC_FILE_NAME" };
        public string strMenuName = "", strMenuNameItem = "";
        string strImagePagh = Content.Variable.strImagePath;
        Common FCommon = new Common();
        CIMSEntities CIMS = new CIMSEntities();
        GMEntities GM = new GMEntities();



        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                ViewState["Del"] = false;
                Button1.Visible = false;
                if (!IsPostBack)
                {
                    if (Request.QueryString["id"] != null)
                    {
                        string id = "";
                        id = Request.QueryString["id"].ToString();
                        txtPURCHquery.Text = id;
                    }
                }
            }

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {

            DijaUpload.Visible = true;
            PURCH_query.Visible = false;
            QueryResult.Visible = false;
            Detail.Visible = true;
            showDijaData();
            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = GM.TBM1301.Where(table => table.OVC_PURCH.Equals(txtPURCHquery.Text)).FirstOrDefault();
            if (tbm1301 != null)
            {
                uploadPURCH.Text = tbm1301.OVC_PURCH;
                uploadOVC_PUR_IPURCH.Text = tbm1301.OVC_PUR_IPURCH;
            }

        }

        protected void GV_TBM1301_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void GV_TBM1301_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if(Convert.ToBoolean(ViewState["Del"]) == true)
                    GV_TBM1301.Columns[7].Visible = true;
                else
                    GV_TBM1301.Columns[7].Visible = false;

            }
                
        }

        protected void GV_TBM1301_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBM1301.DataKeys[gvrIndex].Values[0].ToString();
            string ITEM = GV_TBM1301.DataKeys[gvrIndex].Values[1].ToString();
            string TIME = GV_TBM1301.DataKeys[gvrIndex].Values[2].ToString();
            string SUB  = GV_TBM1301.DataKeys[gvrIndex].Values[3].ToString();
            switch (e.CommandName)
            {
                case "downloadfile":
                    TBM_FILE tbm_file_download  = new TBM_FILE();
                    tbm_file_download = CIMS.TBM_FILE.Where(table => table.OVC_PURCH.Equals(id) && table.OVC_ITEM.Equals(ITEM) && table.OVC_TIMES.Equals(TIME) && table.OVC_SUB.Equals(SUB)).FirstOrDefault();
                    if (tbm_file_download != null)
                    {
                        string path = tbm_file_download.OVC_FILE_NAME;

                        string appPath = Request.PhysicalApplicationPath;
                        string location = appPath + "\\Upload\\CIMS\\";
                        string download = location + path;
                        System.Net.WebClient wc = new System.Net.WebClient();
                        byte[] xfile = null;
                        xfile = wc.DownloadData(download);
                        string xfileName = System.IO.Path.GetFileName(download);
                        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpContext.Current.Server.UrlEncode(xfileName));
                        HttpContext.Current.Response.ContentType = "application/octet-stream";
                        HttpContext.Current.Response.BinaryWrite(xfile);
                        HttpContext.Current.Response.End();
                    }
                    
                    break;
                case "Del":
                    TBM_FILE tbm_file_del = new TBM_FILE();
                    tbm_file_del=CIMS.TBM_FILE.Where(table=>table.OVC_PURCH.Equals(id)&&table.OVC_ITEM.Equals(ITEM)&&table.OVC_TIMES.Equals(TIME)&&table.OVC_SUB.Equals(SUB)).FirstOrDefault();
                    CIMS.Entry(tbm_file_del).State = EntityState.Deleted;
                    CIMS.SaveChanges();
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm_file_del.GetType().Name.ToString(), this, "代理商修改");
                    showDijaData();
                    break;

                default:
                    break;
            }
        }

        protected void Dijiquery_Click(object sender, EventArgs e)
        {
            
            Detail.Visible = true;
            QueryResult.Visible = false;
            PURCH_query.Visible = false;
            Button1.Visible = true;
            showDijaData();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = GM.TBM1301.Where(table => table.OVC_PURCH.Equals(txtPURCHquery.Text)).FirstOrDefault();
            if (tbm1301 != null)
            {
                txtOVC_PUR_IPURCH.Text = tbm1301.OVC_PUR_IPURCH;
                txtOVC_PUR_USER.Text = tbm1301.OVC_PUR_USER;
                txt_OVC_DPROPOSE.Text = tbm1301.OVC_DPROPOSE;
                txtOVC_PUR_SECTION.Text = tbm1301.OVC_PUR_SECTION;
                txtOVC_PUR_NSECTION .Text= tbm1301.OVC_PUR_NSECTION;
                QueryResult.Visible = true;
                upload.Visible = true;
                TBM_FILE tbm_file = new TBM_FILE();
                tbm_file=CIMS.TBM_FILE.Where(table=>table.OVC_PURCH.Equals(txtPURCHquery.Text)).FirstOrDefault();
                if (tbm_file != null)
                {
                    Dijiquery.Visible = true;
                    Dijintdata.Visible = false;
                }
                else
                {
                    Dijiquery.Visible = false;
                    Dijintdata.Visible = true;
                }
            }
            else
            {
                QueryResult.Visible = false;
                upload.Visible = false;
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查無資料");
            }
        }

        protected void uploadConfirm_Click(object sender, EventArgs e)
        {
            


            string appPath, Path, saveResult,location, fileName,saveName,id = "";
            id = uploadPURCH.Text;
            //檢查資料
            string strMessage = "";
            int num1;

            if(uploadOVC_ITEM.Text=="")
                strMessage += "<P> 組 為必填欄位 </p>";
            if (int.TryParse(uploadOVC_ITEM.Text, out num1) == false)
                strMessage += "<P> 組 須為數字 </p>";
            if (uploadOVC_TIMES.Text == "")
                strMessage += "<P> 次 為必填欄位 </p>";
            if (int.TryParse(uploadOVC_TIMES.Text, out num1) == false)
                strMessage += "<P> 次 須為數字 </p>";
            if (uploadOVC_SUB.Text == "")
                strMessage += "<P> 附件序號 為必填欄位 </p>";
            if (int.TryParse(uploadOVC_SUB.Text, out num1) == false)
                strMessage += "<P> 附件序號 須為數字 </p>";
            if (!FileUpload1.HasFile)
                strMessage += "<P> 請選擇上傳檔案 </p>";



            if (strMessage.Equals(string.Empty))
            {
                TBM_FILE tbm_file = new TBM_FILE();
                tbm_file=CIMS.TBM_FILE.Where(table=>table.OVC_PURCH.Equals(id)&&table.OVC_ITEM.Equals(uploadOVC_ITEM.Text)&& table.OVC_TIMES.Equals(uploadOVC_TIMES.Text)&& table.OVC_SUB.Equals(uploadOVC_SUB.Text)).FirstOrDefault();
                if (tbm_file != null)
                {
                    strMessage += "<P> 已有重複資料，請修改欄位值 </p>";
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                }
                else
                {

                    appPath = Request.PhysicalApplicationPath;
                    location = appPath + "\\Upload\\CIMS\\";
                    Path = location + id + "\\";

                    if (!Directory.Exists(Path))
                    {
                        Directory.CreateDirectory(Path);
                    }

                    fileName = FileUpload1.FileName;
                    saveName = id + "\\" + fileName;
                    saveResult = Path + fileName;
                    FileUpload1.SaveAs(saveResult);

                    TBM_FILE tbm_file_add = new TBM_FILE();
                    tbm_file_add.OVC_PURCH = uploadPURCH.Text;
                    tbm_file_add.OVC_ATTACH_NAME = uploadOVC_ATTACH_NAME.SelectedValue;
                    tbm_file_add.OVC_KIND = "T";
                    tbm_file_add.OVC_ITEM = uploadOVC_ITEM.Text;
                    tbm_file_add.OVC_TIMES = uploadOVC_TIMES.Text;
                    tbm_file_add.OVC_SUB = uploadOVC_SUB.Text;
                    tbm_file_add.OVC_FILE_NAME = saveName;
                    tbm_file_add.OVC_UP_USER = Session["username"].ToString();
                    CIMS.TBM_FILE.Add(tbm_file_add);
                    CIMS.SaveChanges();
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "底價表新增成功");
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbm_file_add.GetType().Name.ToString(), this, "新增底價表");
                    showDijaData();
                }
                    

            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);




        }

        protected void uploadCancel_Click(object sender, EventArgs e)
        {
            DijaUpload.Visible = false;
            PURCH_query.Visible = true;
            QueryResult.Visible = true;
            Detail.Visible = false;
        }

        protected void showDijaData()
        {
            DataTable dt = new DataTable();

            string strPURCH = txtPURCHquery.Text;
            var query =
                from TBM1301 in GM.TBM1301.DefaultIfEmpty().AsEnumerable()
                join TBM_FILE in CIMS.TBM_FILE.DefaultIfEmpty().AsEnumerable() on TBM1301.OVC_PURCH equals TBM_FILE.OVC_PURCH
                where TBM_FILE.OVC_PURCH.Equals(strPURCH)
                select new
                {
                    OVC_PURCH = strPURCH,
                    OVC_ATTACH_NAME = TBM_FILE.OVC_ATTACH_NAME == null ? "" : TBM_FILE.OVC_ATTACH_NAME,
                    OVC_ITEM = TBM_FILE.OVC_ITEM == null ? "" : TBM_FILE.OVC_ITEM,
                    OVC_TIMES = TBM_FILE.OVC_TIMES == null ? "" : TBM_FILE.OVC_TIMES,
                    OVC_SUB = TBM_FILE.OVC_SUB == null ? "" : TBM_FILE.OVC_SUB,
                    OVC_FILE_NAME = TBM_FILE.OVC_FILE_NAME == null ? "" : TBM_FILE.OVC_FILE_NAME
                };
            dt = CommonStatic.LinqQueryToDataTable(query);

            //加上流水序號&字串切割(OVC_FILE_NAME資料欄，舊資料與新資料存放規則不同，新資料會多上一個目錄，但顯示時要將目錄去除)
            DataColumn column = new DataColumn();
            column.ColumnName = "RANK";
            column.DataType = System.Type.GetType("System.Int32");
            dt.Columns.Add(column);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Rank"] = i + 1;

                string[] strs = dt.Rows[i]["OVC_FILE_NAME"].ToString().Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                if (strs.Length>1)
                {
                    dt.Rows[i]["OVC_FILE_NAME"] = strs[1].ToString();
                }


            }


            if (DijaUpload.Visible == true && Detail.Visible == true)
            {
                ViewState["Del"] = true;
            }




            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBM1301, dt, strField);
        }

    }
}