using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F11 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (browse.HasFile && Path.GetExtension(browse.FileName) == ".xlsx")
            {
                using (var excel = new ExcelPackage(browse.PostedFile.InputStream))
                {
                    var tbl = new DataTable();
                    var ws = excel.Workbook.Worksheets.First();
                    var hasHeader = true;  // adjust accordingly
                                           // add DataColumns to DataTable
                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                        tbl.Columns.Add(hasHeader ? firstRowCell.Text
                            : String.Format("Column {0}", firstRowCell.Start.Column));

                    // add DataRows to DataTable
                    int startRow = hasHeader ? 2 : 1;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        DataRow row = tbl.NewRow();
                        foreach (var cell in wsRow)
                            row[cell.Start.Column - 1] = cell.Text;
                        tbl.Rows.Add(row);
                    }
                    for (var i = 0; i < tbl.Rows.Count; i++) 
                    {
                        try
                        {
                            #region 判斷是否有重複EDF_NO
                            var query =
                            from tbgmt_edf in MTSE.TBGMT_EDF
                            orderby tbgmt_edf.OVC_EDF_NO
                            select new
                            {
                                OVC_EDF_NO = tbgmt_edf.OVC_EDF_NO,
                            };
                            string temp = "EDF" + DateTime.Now.AddYears(-1911).ToString("yyy") + tbl.Rows[0][4];
                            query = query.Where(table => table.OVC_EDF_NO.Contains(temp));
                            DataTable dt = new DataTable();
                            dt = CommonStatic.LinqQueryToDataTable(query);
                            #endregion
                            #region 新增進EDF 資料庫
                            TBGMT_EDF edf = new TBGMT_EDF();
                            if (dt.Rows.Count > 0)
                            {
                                string number = dt.Rows[dt.Rows.Count - 1][0].ToString().Substring(11, 4);
                                edf.OVC_EDF_NO = "EDF" + DateTime.Now.AddYears(-1911).ToString("yyy") + tbl.Rows[0][4] + (Convert.ToInt16(number) + 1).ToString("0000");
                            }
                            else
                            {
                                edf.OVC_EDF_NO = "EDF" + DateTime.Now.AddYears(-1911).ToString("yyy") + tbl.Rows[0][4] + "0001";
                            }
                            edf.OVC_PURCH_NO = tbl.Rows[i][0].ToString();
                            edf.OVC_START_PORT = tbl.Rows[i][1].ToString();
                            edf.OVC_ARRIVE_PORT = tbl.Rows[i][2].ToString();
                            edf.OVC_DEPT_CDE = tbl.Rows[i][3].ToString();
                            edf.OVC_REQ_DEPT_CDE = tbl.Rows[i][4].ToString();
                            edf.OVC_CON_CHI_ADDRESS = tbl.Rows[i][5].ToString();
                            edf.OVC_CON_ENG_ADDRESS = tbl.Rows[i][6].ToString();
                            edf.OVC_CON_TEL = tbl.Rows[i][7].ToString();
                            edf.OVC_CON_FAX = tbl.Rows[i][8].ToString();
                            edf.OVC_NP_CHI_ADDRESS = tbl.Rows[i][9].ToString();
                            edf.OVC_NP_ENG_ADDRESS = tbl.Rows[i][10].ToString();
                            edf.OVC_NP_TEL = tbl.Rows[i][11].ToString();
                            edf.OVC_NP_FAX = tbl.Rows[i][12].ToString();
                            edf.OVC_ANP_CHI_ADDRESS = tbl.Rows[i][13].ToString();
                            edf.OVC_ANP_ENG_ADDRESS = tbl.Rows[i][14].ToString();
                            edf.OVC_ANP_TEL = tbl.Rows[i][15].ToString();
                            edf.OVC_ANP_FAX = tbl.Rows[i][16].ToString();
                            edf.OVC_PAYMENT_TYPE = tbl.Rows[i][17].ToString();
                            edf.OVC_NOTE = tbl.Rows[i][18].ToString();
                            edf.OVC_IS_STRATEGY = tbl.Rows[i][19].ToString();
                            edf.OVC_SHIP_FROM = tbl.Rows[i][20].ToString();
                            edf.ODT_MODIFY_DATE = DateTime.Now;
                            edf.OVC_CREATE_LOGIN_ID = Session["userid"].ToString();
                            edf.OVC_ANP_ENG_ADDRESS2 = tbl.Rows[i][21].ToString();
                            edf.OVC_ANP_TEL2 = tbl.Rows[i][22].ToString();
                            edf.OVC_ANP_FAX2 = tbl.Rows[i][23].ToString();
                            edf.OVC_IS_PAY = tbl.Rows[i][24].ToString();
                            edf.OVC_DELIVER_NAME = tbl.Rows[i][25].ToString();
                            edf.OVC_DELIVER_MILITARY_LINE = tbl.Rows[i][26].ToString();
                            edf.OVC_DELIVER_MOBILE = tbl.Rows[i][27].ToString();
                            edf.EDF_SN = Guid.NewGuid();
                            edf.OVC_MODIFY_LOGIN_ID = Session["userid"].ToString();
                            MTSE.TBGMT_EDF.Add(edf);

                            MTSE.SaveChanges();
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            msg = "Excel第" + (i + 1) + "行上傳失敗，請檢察資料是否正確!!";
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", msg);
                        }

                    }

                    if (msg == "")
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "上傳成功!!");
                }
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "上傳失敗，檔名不正確!!");
            }
        }

        protected void btnUpload2_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (browse2.HasFile && Path.GetExtension(browse2.FileName) == ".xlsx")
            {
                using (var excel = new ExcelPackage(browse2.PostedFile.InputStream))
                {
                    var tbl = new DataTable();
                    var ws = excel.Workbook.Worksheets.First();
                    var hasHeader = true;  // adjust accordingly
                                           // add DataColumns to DataTable
                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                        tbl.Columns.Add(hasHeader ? firstRowCell.Text
                            : String.Format("Column {0}", firstRowCell.Start.Column));

                    // add DataRows to DataTable
                    int startRow = hasHeader ? 2 : 1;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        DataRow row = tbl.NewRow();
                        foreach (var cell in wsRow)
                            row[cell.Start.Column - 1] = cell.Text;
                        tbl.Rows.Add(row);
                    }
                    for (var i = 0; i < tbl.Rows.Count; i++)
                    {
                        try
                        {
                            DataTable dt = new DataTable();
                            string strEdfNo = tbl.Rows[i][0].ToString();
                            string strEdfItemNo = tbl.Rows[i][5].ToString();
                            var query =
                                from detail in MTSE.TBGMT_EDF_DETAIL
                                select new
                                {
                                    detail.OVC_EDF_NO,
                                    detail.OVC_ITEM_NO,
                                };
                            query = query.Where(ot => ot.OVC_EDF_NO.Equals(strEdfNo));
                            query = query.Where(ot => ot.OVC_ITEM_NO.Equals(strEdfItemNo));
                            dt = CommonStatic.LinqQueryToDataTable(query);
                            if (dt.Rows.Count > 0)
                            {
                                msg = "Excel第" + (i + 1) + "行上傳失敗，資料已經存在!!";
                                FCommon.AlertShow(PnMessage, "danger", "系統訊息", msg);
                            }
                            else
                            {
                                #region 新增edf detail
                                TBGMT_EDF_DETAIL edf_detail = new TBGMT_EDF_DETAIL();
                                edf_detail.OVC_EDF_NO = tbl.Rows[i][0].ToString();
                                edf_detail.OVC_CHI_NAME = tbl.Rows[i][2].ToString();
                                edf_detail.OVC_ENG_NAME = tbl.Rows[i][3].ToString();
                                edf_detail.OVC_BOX_NO = tbl.Rows[i][4].ToString();
                                edf_detail.OVC_ITEM_NO = tbl.Rows[i][5].ToString();
                                edf_detail.OVC_ITEM_NO2 = tbl.Rows[i][6].ToString();
                                edf_detail.OVC_ITEM_NO3 = tbl.Rows[i][7].ToString();
                                edf_detail.OVC_ITEM_COUNT_UNIT = tbl.Rows[i][9].ToString();
                                edf_detail.OVC_WEIGHT_UNIT = tbl.Rows[i][11].ToString();
                                edf_detail.OVC_VOLUME_UNIT = tbl.Rows[i][13].ToString();
                                edf_detail.OVC_EDF_ITEM_NO = Convert.ToDecimal(tbl.Rows[i][1]);
                                edf_detail.ONB_ITEM_COUNT = Convert.ToDecimal(tbl.Rows[i][8]);
                                edf_detail.ONB_WEIGHT = Convert.ToDecimal(tbl.Rows[i][10]);
                                edf_detail.ONB_VOLUME = Convert.ToDecimal(tbl.Rows[i][12]);
                                edf_detail.ONB_LENGTH = Convert.ToDecimal(tbl.Rows[i][14]);
                                edf_detail.ONB_WIDTH = Convert.ToDecimal(tbl.Rows[i][15]);
                                edf_detail.ONB_HEIGHT = Convert.ToDecimal(tbl.Rows[i][16]);
                                edf_detail.ONB_MONEY = Convert.ToDecimal(tbl.Rows[i][17]);
                                edf_detail.OVC_CURRENCY = tbl.Rows[i][18].ToString();
                                edf_detail.ODT_MODIFY_DATE = DateTime.Now;
                                edf_detail.OVC_CREATE_LOGIN_ID = Session["userid"].ToString();
                                edf_detail.EDF_DET_SN = Guid.NewGuid();
                                MTSE.TBGMT_EDF_DETAIL.Add(edf_detail);
                                MTSE.SaveChanges();
                                #endregion
                            }
                        }
                        catch(Exception ex)
                        {
                            msg = "Excel第"+(i+1)+"行上傳失敗，請檢察資料是否正確!!";
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", msg);
                        }

                    }
                    if (msg == "")
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "上傳成功!!");
                }
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "上傳失敗，檔名不正確!!");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            bool isUpload;
            if (FCommon.getAuth(this, out isUpload))
            {

            }
        }
    }
}