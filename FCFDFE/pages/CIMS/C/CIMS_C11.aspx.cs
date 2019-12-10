using FCFDFE.Content;
using FCFDFE.Entity.CIMSModel;
using OfficeOpenXml;
using System;
using System.Data;
using System.Web.UI;
using System.Linq;
using System.IO;
using System.Web;

namespace FCFDFE.pages.CIMS.C
{
    public partial class CIMS_C11 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        string strImagePagh = Content.Variable.strImagePath;
        Common FCommon = new Common();
        CIMSEntities CIMS = new CIMSEntities();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    imgExample.ImageUrl = strImagePagh + "CIMS/CIMS_C11_example.PNG";
                }
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {

            string appPath = Request.PhysicalApplicationPath;
            string location = appPath + "\\pages\\CIMS\\2016FD轉檔程式.xlsm";
            System.Net.WebClient wc = new System.Net.WebClient();
            byte[] xfile = null;
            xfile = wc.DownloadData(location);
            string xfileName = System.IO.Path.GetFileName(location);
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpContext.Current.Server.UrlEncode("2016FD轉檔程式.xlsm"));
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.BinaryWrite(xfile);
            HttpContext.Current.Response.End();
        }

        public void btnNew_OnClick(object sender, EventArgs e)
        {
            String fileName = ful.FileName.ToString();
            TBM_PUBLIC_BID_99 codetable = new TBM_PUBLIC_BID_99();
            var query =
                 from cims in CIMS.TBM_PUBLIC_BID_99 
                 select new
                 {
                     cims.OVC_KEY
                 };
            var dt = new DataTable();
            dt = CommonStatic.LinqQueryToDataTable(query);
            var finalkey = dt.Rows[dt.Rows.Count-1][0].ToString();
            int finalkey_num= Convert.ToInt32(finalkey.Substring(finalkey.Length - 7));
            if (ful.HasFile)
            {
                if (Path.GetExtension(ful.FileName) != ".xlsx") //副檔名必須要.xls
                    FCommon.AlertShow(PnWarning, "danger", "系統訊息", "副檔名必須為.xls");
                else
                {
                    using (var excel = new ExcelPackage(ful.PostedFile.InputStream))
                    {
                        var tbl = new DataTable();  //新增DataTable存放excel
                        var ws = excel.Workbook.Worksheets.First();
                        var hasHeader = true;
                        foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                            tbl.Columns.Add(hasHeader ? firstRowCell.Text
                                : String.Format("Column {0}", firstRowCell.Start.Column));

                        // add excel data  to DataTable
                        int startRow = hasHeader ? 2 : 1;
                        for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                        {
                            var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                            DataRow row = tbl.NewRow();
                            foreach (var cell in wsRow)
                                row[cell.Start.Column - 1] = cell.Text;
                            tbl.Rows.Add(row);
                        }
                       
                        try
                        {
                            #region TBM_PUBLIC_BID_99 新增excel資料
                            for(var i = 0; i < tbl.Rows.Count; i++)
                            {
                                finalkey_num++;
                                codetable.OVC_KEY = "B" + String.Format("{0:0000000}", finalkey_num);
                                codetable.OVC_A = tbl.Rows[i][0].ToString() ?? string.Empty; //新增第i列第0行資料 判斷是否空值 是insert null
                                codetable.OVC_B = tbl.Rows[i][1].ToString() ?? string.Empty;
                                codetable.OVC_C = tbl.Rows[i][2].ToString() ?? string.Empty;
                                codetable.OVC_D = tbl.Rows[i][3].ToString() ?? string.Empty;
                                codetable.OVC_E = tbl.Rows[i][4].ToString() ?? string.Empty;
                                codetable.OVC_F = tbl.Rows[i][5].ToString() ?? string.Empty;
                                codetable.OVC_G = tbl.Rows[i][6].ToString() ?? string.Empty;
                                codetable.OVC_H = tbl.Rows[i][7].ToString() ?? string.Empty;
                                codetable.OVC_I = tbl.Rows[i][8].ToString() ?? string.Empty;
                                codetable.OVC_J = tbl.Rows[i][9].ToString() ?? string.Empty;
                                codetable.OVC_K = tbl.Rows[i][10].ToString() ?? string.Empty;
                                codetable.OVC_L = tbl.Rows[i][11].ToString() ?? string.Empty;
                                if(tbl.Rows[i][12].ToString() != "")
                                {
                                    codetable.ONB_M = Convert.ToInt64(tbl.Rows[i][12]);
                                }
                                else
                                {
                                    codetable.ONB_M = null;
                                }
                                codetable.OVC_N = tbl.Rows[i][13].ToString() ?? string.Empty;
                                codetable.OVC_O = tbl.Rows[i][14].ToString() ?? string.Empty;
                                codetable.OVC_P = tbl.Rows[i][15].ToString() ?? string.Empty;
                                if (tbl.Rows[i][16].ToString() != "")
                                {
                                    codetable.ONB_Q = Convert.ToInt64(tbl.Rows[i][16]);
                                }
                                else
                                {
                                    codetable.ONB_Q = null;
                                }
                                codetable.OVC_R = tbl.Rows[i][17].ToString() ?? string.Empty;
                                codetable.OVC_S = tbl.Rows[i][18].ToString() ?? string.Empty;
                                if (tbl.Rows[i][19].ToString() != "")
                                {
                                    codetable.ONB_T = Convert.ToInt32(tbl.Rows[i][19]);
                                }
                                else
                                {
                                    codetable.ONB_T = null;
                                }
                                if (tbl.Rows[i][20].ToString() != "")
                                {
                                    codetable.ONB_U = Convert.ToInt32(tbl.Rows[i][20]);
                                }
                                else
                                {
                                    codetable.ONB_U = null;
                                }
                                if (tbl.Rows[i][21].ToString() != "")
                                {
                                    codetable.ONB_V = Convert.ToInt32(tbl.Rows[i][21]);
                                }
                                else
                                {
                                    codetable.ONB_V = null;
                                }
                                codetable.OVC_W = tbl.Rows[i][22].ToString() ?? string.Empty;
                                codetable.OVC_X = tbl.Rows[i][23].ToString() ?? string.Empty;
                                codetable.OVC_Y = tbl.Rows[i][24].ToString() ?? string.Empty;
                                codetable.OVC_Z = tbl.Rows[i][25].ToString() ?? string.Empty;
                                codetable.OVC_AA = tbl.Rows[i][26].ToString() ?? string.Empty;
                                codetable.OVC_AB = tbl.Rows[i][27].ToString() ?? string.Empty;
                                codetable.OVC_AC = tbl.Rows[i][28].ToString() ?? string.Empty;
                                if (tbl.Rows[i][29].ToString() != "")
                                {
                                    var a = tbl.Rows[i][29].ToString().Replace("底價為", "");
                                    codetable.ONB_AD = Convert.ToInt64(a);
                                }
                                else
                                {
                                    codetable.ONB_AD = null;
                                }
                                codetable.OVC_AE = tbl.Rows[i][30].ToString() ?? string.Empty;
                                codetable.OVC_AF = tbl.Rows[i][31].ToString() ?? string.Empty;
                                if (tbl.Rows[i][32].ToString() != "")
                                {
                                    codetable.ONB_AG = Convert.ToInt64(tbl.Rows[i][32]);
                                }
                                else
                                {
                                    codetable.ONB_AG = null;
                                }
                                codetable.OVC_AH = tbl.Rows[i][33].ToString() ?? string.Empty;
                                if (tbl.Rows[i][34].ToString() != "")
                                {
                                    codetable.ONB_AI = Convert.ToInt64(tbl.Rows[i][34]);
                                }
                                else
                                {
                                    codetable.ONB_AI = null;
                                }
                                codetable.OVC_AJ = tbl.Rows[i][35].ToString() ?? string.Empty;
                                codetable.OVC_AK = tbl.Rows[i][36].ToString() ?? string.Empty;
                                codetable.OVC_AL = tbl.Rows[i][37].ToString() ?? string.Empty;
                                codetable.OVC_AM = tbl.Rows[i][38].ToString() ?? string.Empty;
                                codetable.OVC_AN = tbl.Rows[i][39].ToString() ?? string.Empty;
                                if (tbl.Rows[i][40].ToString() != "")
                                {
                                    codetable.ONB_AO = Convert.ToInt16(tbl.Rows[i][40]);
                                }
                                else
                                {
                                    codetable.ONB_AO = null;
                                }
                                codetable.OVC_AP = tbl.Rows[i][41].ToString() ?? string.Empty;
                                codetable.OVC_AQ = tbl.Rows[i][42].ToString() ?? string.Empty;
                                codetable.OVC_AR = tbl.Rows[i][43].ToString() ?? string.Empty;
                                codetable.OVC_AS = tbl.Rows[i][44].ToString() ?? string.Empty;
                                codetable.OVC_AT = tbl.Rows[i][45].ToString() ?? string.Empty;
                                codetable.OVC_AU = tbl.Rows[i][46].ToString() ?? string.Empty;
                                if (tbl.Rows[i][47].ToString() != "")
                                {
                                    codetable.ONB_AV = Convert.ToInt16(tbl.Rows[i][47]);
                                }
                                else
                                {
                                    codetable.ONB_AV = null;
                                }
                                codetable.OVC_AW = tbl.Rows[i][48].ToString() ?? string.Empty;
                                codetable.OVC_AX = tbl.Rows[i][49].ToString() ?? string.Empty;
                                codetable.OVC_AY = tbl.Rows[i][50].ToString() ?? string.Empty;
                                codetable.OVC_AZ = tbl.Rows[i][51].ToString() ?? string.Empty;
                                if (tbl.Rows[i][52].ToString() != "")
                                {
                                    codetable.ONB_BA = Convert.ToInt16(tbl.Rows[i][52]);
                                }
                                else
                                {
                                    codetable.ONB_BA = null;
                                }
                                codetable.OVC_BB = tbl.Rows[i][53].ToString() ?? string.Empty;
                                codetable.OVC_BC = tbl.Rows[i][54].ToString() ?? string.Empty;
                                codetable.OVC_BD = tbl.Rows[i][55].ToString() ?? string.Empty;
                                codetable.OVC_BE = tbl.Rows[i][56].ToString() ?? string.Empty;
                                if (tbl.Rows[i][57].ToString() != "")
                                {
                                    codetable.ONB_BF = Convert.ToInt64(tbl.Rows[i][57]);
                                }
                                else
                                {
                                    codetable.ONB_BF = null;
                                }
                               
                                codetable.OVC_BG = Convert.ToDateTime(tbl.Rows[i][58]).ToString("yyyy/M/d") ?? string.Empty;
                                codetable.OVC_BH = tbl.Rows[i][59].ToString() ?? string.Empty;
                                if (tbl.Rows[i][60].ToString() != "")
                                {
                                    codetable.ONB_BI = Convert.ToInt64(tbl.Rows[i][60]);
                                }
                                else
                                {
                                    codetable.ONB_BI = null;
                                }
                                CIMS.TBM_PUBLIC_BID_99.Add(codetable);
                                CIMS.SaveChanges();
                                codetable = new TBM_PUBLIC_BID_99();
                            }
                            
                            #endregion
                            FCommon.AlertShow(PnWarning, "success", "系統訊息", "新增成功");
                        }
                        catch (Exception ex)
                        {
                            FCommon.AlertShow(PnWarning, "danger", "系統訊息", ex.ToString());
                        }
                    }
                }
            }
            else
            {
                FCommon.AlertShow(PnWarning, "danger", "系統訊息", "請上傳檔案");
            }
        }


    }
}