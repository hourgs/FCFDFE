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
using System.Globalization;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Model;

namespace FCFDFE.pages.CIMS.C
{
    public partial class CIMS_C12 : System.Web.UI.Page
    {

        public string strMenuName = "", strMenuNameItem = "";
        string strImagePagh = Content.Variable.strImagePath;
        Common FCommon = new Common();
        CIMSEntities CIMS = new CIMSEntities();
        GMEntities GM = new GMEntities();

        string data_year = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtovc_aq_s, txtovc_aq_e);
                }
            }
        }




        protected void GV_A_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void Button98_Click(object sender, EventArgs e)
        {
            Panel.Visible = false;
            Panel1.Visible = true;
            Session["data_year"] = "98";
        }

        protected void Button99_Click(object sender, EventArgs e)
        {
            Panel.Visible = false;
            Panel1.Visible = true;
            Session["data_year"] = "99";
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            
            string strMessage = "";
            string strtxtovc_x = txtovc_x.Text;
            string strtxtovc_w = txtovc_w.Text;
            string strtxtovc_aq_s = txtovc_aq_s.Text;
            string strtxtovc_aq_e = txtovc_aq_e.Text;
            string strtxtovc_a = txtovc_a.Text;
            string strtxtovc_aw = txtovc_aw.SelectedValue;
            string strtxtovc_bc = txtovc_bc.SelectedValue;
            string strtxtovc_y = txtovc_y.SelectedValue;
            string strtxtovc_at = txtovc_at.SelectedValue;

            if (strtxtovc_x.Equals(string.Empty) && strtxtovc_w.Equals(string.Empty) && strtxtovc_aq_s.Equals(string.Empty) && strtxtovc_aq_e.Equals(string.Empty) && strtxtovc_a.Equals(string.Empty)
                && strtxtovc_aw.Equals(string.Empty) && strtxtovc_bc.Equals(string.Empty) && strtxtovc_y.Equals(string.Empty) && strtxtovc_at.Equals(string.Empty))
                strMessage += "<P> 至少填入一個選項 </p>";


            if (strMessage.Equals(string.Empty))
            {
                if (Session["data_year"].ToString().Equals("99"))
                {
                    
                    var query = from bid in CIMS.TBM_PUBLIC_BID_99.ToList()
                                select new
                                {
                                    OVC_KEY = bid.OVC_KEY,
                                    OVC_W = bid.OVC_W,
                                    OVC_AQ = bid.OVC_AQ,
                                    OVC_A = bid.OVC_A,
                                    OVC_AW = bid.OVC_AW,
                                    OVC_BC = bid.OVC_BC,
                                    OVC_AT = bid.OVC_AT,
                                    OVC_Y = bid.OVC_Y
                                };


                    if (!strtxtovc_x.Equals(string.Empty))
                    {
                        query = query.Where(table => table.OVC_KEY!=null);
                        query = query.Where(table => table.OVC_KEY.Contains(strtxtovc_x));
                    }

                    if (!strtxtovc_w.Equals(string.Empty))

                    {
                        query = query.Where(table => table.OVC_W!=null);
                        query = query.Where(table => table.OVC_W.Contains(strtxtovc_w));
                    }
                    if (!strtxtovc_a.Equals(string.Empty))
                    {
                        query = query.Where(table => table.OVC_A!=null);
                        query = query.Where(table => table.OVC_A.Contains(strtxtovc_a));
                    }

                    if (!strtxtovc_aw.Equals(string.Empty))
                    {
                        query = query.Where(table => table.OVC_AW!=null);
                        query = query.Where(table => table.OVC_AW.Contains(strtxtovc_aw));
                    }
                    if (!strtxtovc_bc.Equals(string.Empty))

                    {
                        query = query.Where(table => table.OVC_BC!=null);
                        query = query.Where(table => table.OVC_BC.Contains(strtxtovc_bc));
                    }
                    if (!strtxtovc_y.Equals(string.Empty))

                    {
                        query = query.Where(table => table.OVC_Y!=null);
                        query = query.Where(table => table.OVC_Y.Contains(strtxtovc_y));
                    }
                    if (!strtxtovc_at.Equals(string.Empty))

                    {
                        query = query.Where(table => table.OVC_AT!=null);
                        query = query.Where(table => table.OVC_AT.Contains(strtxtovc_at));
                    }
                    if (!strtxtovc_aq_s.Equals(string.Empty) && !strtxtovc_aq_e.Equals(string.Empty))
                    {
                        query = query.Where(table => table.OVC_AQ != null);
                        DateTime Date1 = Convert.ToDateTime(strtxtovc_aq_s);
                        DateTime Date2 = Convert.ToDateTime(strtxtovc_aq_e);
                        query = query.Where(table => Convert.ToDateTime(table.OVC_AQ) >= Date1 && Convert.ToDateTime(table.OVC_AQ) <= Date2);
                    }

                    DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                    string[] strField = { "OVC_KEY", "OVC_W", "OVC_AQ", "OVC_A", "OVC_AW", "OVC_BC", "OVC_AT" };
                    ViewState["hasRows"] = FCommon.GridView_dataImport(GV_A, dt, strField);
                    if (dt.Rows.Count > 0)
                    {
                        btnPrint.Visible = true;
                    }
                    else
                    {
                        btnPrint.Visible = false;
                    }
                }
                if (Session["data_year"].ToString().Equals("98"))
                {
                    var query =
                   from bid in CIMS.TBM_PUBLIC_BID.ToList()

                   select new
                   {
                       OVC_KEY = bid.OVC_KEY,
                       OVC_W = bid.OVC_NPURCH,
                       OVC_AQ=bid.OVC_DBID_DATE,
                       OVC_A= bid.OVC_VEN_NAME,
                       OVC_AW=bid.OVC_BID_KIND,
                       OVC_BC=bid.OVC_DBID_KIND,
                       OVC_AT=bid.OVC_PURCH_KIND,
                       OVC_Y = bid.OVC_BID_SEC

                   };
                    if (!strtxtovc_x.Equals(string.Empty))
                        query = query.Where(table => table.OVC_KEY.Contains(strtxtovc_x));
                    if (!strtxtovc_w.Equals(string.Empty))
                        query = query.Where(table => table.OVC_W.Contains(strtxtovc_w));
                    if (!strtxtovc_a.Equals(string.Empty))
                        query = query.Where(table => table.OVC_A.Contains(strtxtovc_a));
                    if (!strtxtovc_aw.Equals(string.Empty))
                        query = query.Where(table => table.OVC_AW.Contains(strtxtovc_aw));
                    if (!strtxtovc_bc.Equals(string.Empty))
                        query = query.Where(table => table.OVC_BC.Contains(strtxtovc_bc));
                    if (!strtxtovc_y.Equals(string.Empty))
                        query = query.Where(table => table.OVC_AT.Contains(strtxtovc_y));
                    if (!strtxtovc_at.Equals(string.Empty))
                        query = query.Where(table => table.OVC_Y.Contains(strtxtovc_at));
                    if (!strtxtovc_aq_s.Equals(string.Empty) && !strtxtovc_aq_e.Equals(string.Empty))
                    {
                        DateTime Date1 = Convert.ToDateTime(strtxtovc_aq_s);
                        DateTime Date2 = Convert.ToDateTime(strtxtovc_aq_e);
                        query = query.Where(table => Convert.ToDateTime(table.OVC_AQ) >= Date1 && Convert.ToDateTime(table.OVC_AQ) <= Date2);

                    }


                    DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                    string[] strField = { "OVC_KEY", "OVC_W", "OVC_AQ", "OVC_A", "OVC_AW", "OVC_BC", "OVC_AT" };
                    ViewState["hasRows"] = FCommon.GridView_dataImport(GV_A, dt, strField);
                    if (dt.Rows.Count > 0)
                    {
                        btnPrint.Visible = true;
                    }
                    else
                    {
                        btnPrint.Visible = false;
                    }
                }
                
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }


        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtovc_x.Text = string.Empty;
            txtovc_w.Text = string.Empty;
            txtovc_aq_s.Text = string.Empty;
            txtovc_aq_e.Text = string.Empty;
            txtovc_a.Text = string.Empty;

            txtovc_aw.SelectedValue = "";
                txtovc_bc.SelectedValue = "";
            txtovc_y.SelectedValue = "";
            txtovc_at.SelectedValue = "";

        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            ////建立Excel 2007檔案
            IWorkbook wb = new XSSFWorkbook();
            ISheet ws = wb.CreateSheet("ReportHandlerServlet");
            ws.DisplayGridlines = false;

            ICellStyle style = wb.CreateCellStyle();//藍色style          
            

            style.VerticalAlignment = VerticalAlignment.Center;
            style.Alignment = HorizontalAlignment.Center;
            style.WrapText = true;
            style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;

            IFont fontstyle = wb.CreateFont();
            fontstyle.FontName = "標楷體";
            fontstyle.FontHeightInPoints = 12;

            ws.CreateRow(0).Height = 32 * 40;
            for(int i=0;i<=6;i++)
            {
                ws.SetColumnWidth(i, 32 * 200);

            }
            ws.GetRow(0).CreateCell(0).SetCellValue("項次");
            ws.GetRow(0).CreateCell(1).SetCellValue("採購案號");
            ws.GetRow(0).CreateCell(2).SetCellValue("標的名稱");
            ws.GetRow(0).CreateCell(3).SetCellValue("決標日期");
            ws.GetRow(0).CreateCell(4).SetCellValue("招標方式");
            ws.GetRow(0).CreateCell(5).SetCellValue("決標方式");
            ws.GetRow(0).CreateCell(6).SetCellValue("標的分類");
            for (int i = 0; i < GV_A.Rows.Count; i++)
            {
                ws.CreateRow(i+1).Height = 32 * 40;
                ws.GetRow(i+1).CreateCell(0).SetCellValue(i+1);
                ws.GetRow(i+1).CreateCell(1).SetCellValue(GV_A.Rows[i].Cells[1].Text);
                ws.GetRow(i+1).CreateCell(2).SetCellValue(GV_A.Rows[i].Cells[2].Text);
                ws.GetRow(i+1).CreateCell(3).SetCellValue(GV_A.Rows[i].Cells[3].Text);
                ws.GetRow(i+1).CreateCell(4).SetCellValue(GV_A.Rows[i].Cells[4].Text);
                ws.GetRow(i+1).CreateCell(5).SetCellValue(GV_A.Rows[i].Cells[5].Text);
                ws.GetRow(i + 1).CreateCell(6).SetCellValue(GV_A.Rows[i].Cells[6].Text);
            }
            for(int i=0; i <=GV_A.Rows.Count; i++)
            {
                for(int x=0;x<7;x++)
                {
                    ws.GetRow(i).GetCell(x).CellStyle = style;
                }
            }
           

            wb.Write(ms);
            wb = null;
            ms.Close();
            ms.Dispose();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + HttpUtility.UrlEncode("工程會決標購案查詢", System.Text.Encoding.UTF8) + ".xlsx\"");
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        }
}