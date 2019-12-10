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
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace FCFDFE.pages.CIMS.B
{
    public partial class CIMS_B13 : System.Web.UI.Page
    {
        string[] strField = { "RANK", "OVC_PURCH", "OVC_PUR_AGENCY", "OVC_PUR_IPURCH", "OVC_ITEM", "OVC_TIMES", "OVC_SUB", "OVC_FILE_NAME" ,"Undertaker", "OVC_UP_USER" };
        public string strMenuName = "", strMenuNameItem = "";
        string strImagePagh = Content.Variable.strImagePath;
        Common FCommon = new Common();
        CIMSEntities CIMS = new CIMSEntities();
        GMEntities GM = new GMEntities();

        protected void GV_TBM1301_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
        }

        protected void btnquery_Click(object sender, EventArgs e)
        {
            if (year.Text == "")
            {

                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請輸入年度後兩碼，例如:欲搜尋101年度的資料，請輸入01");
            }
            else
            {
                showquery();
                export.Visible = true;
            }

        }

        protected void export_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable ();
            dt = (DataTable)ViewState["querydt"];

            //private void DataTableToExcelFile(DataTable dt, string strPath, string strFileName, string SheetName)
            //{
            //建立Excel 2003檔案
            IWorkbook wb = new HSSFWorkbook();
            ISheet ws;
            string SheetName = "底價表上傳明細";
            ws = wb.CreateSheet(SheetName);
            MemoryStream ms = new MemoryStream();
            ws.CreateRow(0).CreateCell(0).SetCellValue("底價表上傳查詢明細");//第一欄標題
            ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 9));
            string timenow = "";
            timenow = DateTime.Now.ToString("yyyy-MM-dd");
            ws.CreateRow(1).CreateCell(0).SetCellValue("印表日期:"+ timenow);//第二欄資料
            ws.AddMergedRegion(new CellRangeAddress(1, 1, 0, 9));
            ws.CreateRow(2);//第三行為欄位名稱
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ws.GetRow(2).CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
            }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ws.CreateRow(i + 3);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ws.GetRow(i + 3).CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                    if(i==0)
                        ws.AutoSizeColumn(j);

                }
                }

            wb.Write(ms);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=底價表上傳查詢.xls"));
            Response.BinaryWrite(ms.ToArray());
            wb = null;
            ms.Close();
            ms.Dispose();

        }

        private void showquery()
        {
            DataTable dt = new DataTable();

            string txtyear = year.Text;
            var query =
                from TBM1301 in GM.TBM1301.DefaultIfEmpty().AsEnumerable()
                join TBM_FILE in CIMS.TBM_FILE.DefaultIfEmpty().AsEnumerable() on TBM1301.OVC_PURCH equals TBM_FILE.OVC_PURCH
                where TBM1301.OVC_PURCH.Substring(2,2).Equals(txtyear)
                select new
                {
                    OVC_PURCH =TBM1301.OVC_PURCH,
                    OVC_PUR_AGENCY=TBM1301.OVC_PUR_AGENCY==null?"": TBM1301.OVC_PUR_AGENCY,
                    OVC_PUR_IPURCH=TBM1301==null?"":TBM1301.OVC_PUR_IPURCH,
                    OVC_ITEM = TBM_FILE.OVC_ITEM == null ? "" : TBM_FILE.OVC_ITEM,
                    OVC_TIMES = TBM_FILE.OVC_TIMES == null ? "" : TBM_FILE.OVC_TIMES,
                    OVC_SUB = TBM_FILE.OVC_SUB == null ? "" : TBM_FILE.OVC_SUB,
                    OVC_FILE_NAME = TBM_FILE.OVC_FILE_NAME == null ? "" : TBM_FILE.OVC_FILE_NAME,
                    Undertaker="",
                    OVC_UP_USER=TBM_FILE.OVC_UP_USER==null?"":TBM_FILE.OVC_UP_USER
                };
            dt = CommonStatic.LinqQueryToDataTable(query);

            
            DataColumn column = new DataColumn();
            column.ColumnName = "RANK";
            column.DataType = System.Type.GetType("System.Int32");
            dt.Columns.Add(column);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Rank"] = i + 1;

            }
            dt.Columns["RANK"].SetOrdinal(0);//定義欄位順序
            




            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBM1301, dt, strField);
            //下面修改dt內容用以excel匯出
            dt.Columns["RANK"].ColumnName = "項次";
            dt.Columns["OVC_PURCH"].ColumnName = "購案案號";
            dt.Columns["OVC_PUR_AGENCY"].ColumnName = "採購單位";
            dt.Columns["OVC_PUR_IPURCH"].ColumnName = "購案名稱";
            dt.Columns["OVC_ITEM"].ColumnName = "購案分組";
            dt.Columns["OVC_TIMES"].ColumnName = "次";
            dt.Columns["OVC_SUB"].ColumnName = "附件序號";
            dt.Columns["OVC_FILE_NAME"].ColumnName = "檔案名稱";
            dt.Columns["Undertaker"].ColumnName = "商情承辦人";
            dt.Columns["OVC_UP_USER"].ColumnName = "上傳者";
            ViewState["querydt"] = dt;

        }
    }
}