using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace FCFDFE.pages.MTS.G
{
    public class Header2 : PdfPageEventHelper
    {
        PdfContentByte cb;
        PdfTemplate template;
        BaseFont bf = null;
        DateTime PrintTime = DateTime.Now.AddYears(-1911);
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H
          , BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                template = cb.CreateTemplate(80, 80);

            }
            catch
            {

            }
        }
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            int pageN = writer.PageNumber;
            String text = "第 " + pageN + " 頁/ ";
            float len = bf.GetWidthPoint(text, 8);
            Rectangle pageSize = document.PageSize;
            cb.BeginText();
            cb.SetFontAndSize(bf, 12);
            cb.SetTextMatrix(pageSize.GetRight(110), pageSize.GetTop(70));
            cb.ShowText(text);
            cb.EndText();
            cb.AddTemplate(template, pageSize.GetRight(100) + len, pageSize.GetTop(70));

            cb.BeginText();
            cb.SetFontAndSize(bf, 14);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "國防部國防採購室運案表", pageSize.GetLeft(290), pageSize.GetTop(40), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 11);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "製表日期：" + PrintTime.ToString("yy/MM/dd"), pageSize.GetRight(10), pageSize.GetTop(50), 0);
            cb.EndText();
            
        }
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            template.BeginText();
            template.SetFontAndSize(bf, 12);
            template.SetTextMatrix(0, 0);
            template.ShowText("共 " + (writer.PageNumber) + " 頁");
            template.EndText();
        }
    }
    public partial class MTS_G12 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        string strImagePagh = Content.Variable.strImagePath;
        private MTSEntities MTS = new MTSEntities();
        Common FCommon = new Common();
        string[] strField = { "OVC_BLD_NO", "OVC_PURCH_NO", "OVC_SHIP_COMPANY", "ONB_QUANITY", "ONB_WEIGHT", "ONB_VOLUME", "ONB_CARRIAGE","import_or_export", "OVC_SEA_OR_AIR", "OVC_CLASS_NAME" , "OVC_RECEIVE_DEPT_CODE", "ODT_IMPORT_DATE" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (!IsPostBack)
                {
                    FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                    FCommon.Controls_Attributes("readonly", "true", txtODT_ACT_ARRIVE_DATE, txtODT_START_DATE);
                    DataTable DEPT_CLASS = CommonStatic.ListToDataTable(MTS.TBGMT_DEPT_CLASS.ToList());
                    FCommon.list_dataImport(drpOVC_MILITARY_TYPE, DEPT_CLASS, "OVC_CLASS_NAME", "OVC_CLASS", true);
                    txtODT_ACT_ARRIVE_DATE.Text = DateTime.Now.ToString("yyyy/MM/dd");
                    txtODT_START_DATE.Text = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");
                }
            }
        }

        protected void GVTBGMT_BLD_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
            hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
            
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (drpOVC_IMP_OR_EXP.SelectedItem.Text.Equals("出口"))
            {
                ImportQuery();
            }
            if (drpOVC_IMP_OR_EXP.SelectedItem.Text.Equals("進口"))
            {
                 ExportQuery();
            }
            if (drpOVC_IMP_OR_EXP.SelectedItem.Text.Equals("不限定"))
            {
                UnlimitedQuery();
            }
        }
        protected void UnlimitedQuery()
        {
            #region 進口
            DataTable dt = new DataTable();

            string strImportOrExport = drpOVC_IMP_OR_EXP.SelectedItem.Text;
            string strOVC_SEA_OR_AIR = drpOVC_SEA_OR_AIR.SelectedItem.Text;
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedItem.Value;
            string strOVC_RECEIVE_DEPT_CODE = drpOVC_TRANSER_DEPT_CDE.SelectedItem.Text;
            string strOVC_SHIP_COMPANY = drpOVC_SHIP_COMPANY.SelectedItem.Text;
            string strOVC_PURCH_NO = txtOVC_PURCH_NO.Text;
            string strODT_IMPORT_DATE1 = txtODT_ACT_ARRIVE_DATE.Text;
            string strODT_IMPORT_DATE2 = txtODT_START_DATE.Text;
            int strOVC_IS_SECURITY = Convert.ToInt16(DropSmartUint.SelectedItem.Value);
            var query =
                from bld in MTS.TBGMT_BLD
                join edf in MTS.TBGMT_EDF on bld.OVC_BLD_NO equals edf.OVC_BLD_NO into p1
                from edf in p1.DefaultIfEmpty()
                join port in MTS.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals port.OVC_PORT_CDE into p2
                from port in p2.DefaultIfEmpty()
                join port2 in MTS.TBGMT_PORTS on bld.OVC_START_PORT equals port2.OVC_PORT_CDE into p3
                from port2 in p3.DefaultIfEmpty()
                join dept in MTS.TBGMT_DEPT_CLASS on bld.OVC_MILITARY_TYPE equals dept.OVC_CLASS into p4
                from dept in p4.DefaultIfEmpty()
                select new
                {
                    OVC_SEA_OR_AIR = bld.OVC_SEA_OR_AIR,
                    OVC_MILITARY_TYPE = bld.OVC_MILITARY_TYPE,
                    OVC_START_PORT = port.OVC_PORT_CHI_NAME,
                    OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY,//
                    OVC_PURCH_NO = edf.OVC_PURCH_NO,
                    ODT_IMPORT_DATE = bld.ODT_START_DATE,
                    OVC_IS_SECURITY = bld.OVC_IS_SECURITY,
                    OVC_BLD_NO = bld.OVC_BLD_NO,//
                    ONB_QUANITY = bld.ONB_QUANITY,//
                    ONB_WEIGHT = bld.ONB_WEIGHT,//
                    ONB_VOLUME = bld.ONB_VOLUME,//
                    bld.OVC_VOLUME_UNIT,
                    ONB_CARRIAGE = bld.ONB_CARRIAGE,
                    OVC_CLASS_NAME = dept.OVC_CLASS_NAME,
                    OVC_IS_ABROAD = port2.OVC_IS_ABROAD,
                    import_or_export = "出口",
                    OVC_RECEIVE_DEPT_CODE = port2.OVC_PORT_CHI_NAME,
                    OVC_PORT_CDE = port2.OVC_PORT_CDE,
                };
            if (!strOVC_SEA_OR_AIR.Equals("不限定"))
                query = query.Where(table => table.OVC_SEA_OR_AIR.Equals(strOVC_SEA_OR_AIR));

            if (!strOVC_MILITARY_TYPE.Equals(string.Empty))
                query = query.Where(table => table.OVC_MILITARY_TYPE.Equals(strOVC_MILITARY_TYPE));

            if (!strOVC_RECEIVE_DEPT_CODE.Equals("不限定"))
            {
                switch (strOVC_RECEIVE_DEPT_CODE)
                {
                    case "基隆地區":
                        query = query.Where(table => table.OVC_PORT_CDE.Equals("TWKEL"));
                        break;
                    case "桃園地區":
                        query = query.Where(table => table.OVC_PORT_CDE.Equals("TPE") || table.OVC_PORT_CDE.Equals("TTY"));
                        break;
                    case "高雄分遣組":
                        query = query.Where(table => table.OVC_PORT_CDE.Equals("TWKHH") || table.OVC_PORT_CDE.Equals("KHH"));
                        break;
                    default:
                        break;
                }
            }

            if (!strOVC_SHIP_COMPANY.Equals("不限定"))
                query = query.Where(table => table.OVC_SHIP_COMPANY.Equals(strOVC_SHIP_COMPANY));

            if (!strOVC_PURCH_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_PURCH_NO.Contains(strOVC_PURCH_NO));

            if (!rdoIm_ExportDate.SelectedItem.Text.Equals("當日作業"))
            {
                DateTime start = Convert.ToDateTime(strODT_IMPORT_DATE1);
                DateTime end = Convert.ToDateTime(strODT_IMPORT_DATE2);
                query = query.Where(table => table.ODT_IMPORT_DATE >= start && table.ODT_IMPORT_DATE < end);
            }
            else
            {
                DateTime now = DateTime.Now;
                query = query.Where(table => table.ODT_IMPORT_DATE == now);
            }
            if (strOVC_IS_SECURITY == 1)
            {
                query = query.Where(table => table.OVC_IS_SECURITY == strOVC_IS_SECURITY);
            }
            else
            {
                query = query.Where(table => table.OVC_IS_SECURITY == strOVC_IS_SECURITY || table.OVC_IS_SECURITY ==null);
            }
         
            query = query.Where(table => table.OVC_IS_ABROAD.Equals("國內"));
            query = query.Take(1000);
            dt = CommonStatic.LinqQueryToDataTable(query);

            if (dt.Rows.Count > 0)
            {
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        //CF 轉換 CBM
                        dt.Rows[i][10] = Math.Round(Convert.ToDouble(dt.Rows[i][10]) / Convert.ToDouble(dt.Rows[i][11].ToString().Replace("CF", "35.315")), 3);
                    }
                    catch
                    {

                    }
                    try
                    {
                        //地方轉換區域
                        dt.Rows[i][16] = dt.Rows[i][16].ToString().Replace("基隆港", "基隆地區").Replace("中正國際機場", "桃園地區").Replace("台灣桃園國際機場", "桃園地區").Replace("高雄港", "高雄分遣組").Replace("高雄小港機場", "高雄分遣組");
                    }
                    catch
                    {

                    }
                }
            }
            #endregion
            #region 出口
            DataTable dt2 = new DataTable();
            var query2 =
                from bld in MTS.TBGMT_BLD
                join icr in MTS.TBGMT_ICR on bld.OVC_BLD_NO equals icr.OVC_BLD_NO into p1
                from icr in p1.DefaultIfEmpty()
                join port in MTS.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals port.OVC_PORT_CDE into p2
                from port in p2.DefaultIfEmpty()
                join port2 in MTS.TBGMT_PORTS on bld.OVC_START_PORT equals port2.OVC_PORT_CDE into p3
                from port2 in p3.DefaultIfEmpty()
                join dept in MTS.TBGMT_DEPT_CLASS on bld.OVC_MILITARY_TYPE equals dept.OVC_CLASS into p4
                from dept in p4.DefaultIfEmpty()
                select new
                {
                    OVC_SEA_OR_AIR = bld.OVC_SEA_OR_AIR,
                    OVC_MILITARY_TYPE = bld.OVC_MILITARY_TYPE,
                    OVC_START_PORT = port2.OVC_PORT_CHI_NAME,
                    OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY,
                    OVC_PURCH_NO = icr.OVC_PURCH_NO,
                    ODT_IMPORT_DATE = icr.ODT_IMPORT_DATE,
                    OVC_IS_SECURITY = bld.OVC_IS_SECURITY,
                    OVC_BLD_NO = bld.OVC_BLD_NO,
                    ONB_QUANITY = bld.ONB_QUANITY,
                    ONB_WEIGHT = bld.ONB_WEIGHT,
                    ONB_VOLUME = bld.ONB_VOLUME,
                    bld.OVC_VOLUME_UNIT,
                    ONB_CARRIAGE = bld.ONB_CARRIAGE,
                    OVC_CLASS_NAME = dept.OVC_CLASS_NAME,
                    OVC_IS_ABROAD = port2.OVC_IS_ABROAD,
                    import_or_export = "進口",
                    OVC_RECEIVE_DEPT_CODE = port.OVC_PORT_CHI_NAME,
                    OVC_PORT_CDE = port.OVC_PORT_CDE,
                };

            if (!strOVC_SEA_OR_AIR.Equals("不限定"))
                query2 = query2.Where(table => table.OVC_SEA_OR_AIR.Equals(strOVC_SEA_OR_AIR));

            if (!strOVC_MILITARY_TYPE.Equals(string.Empty))
                query2 = query2.Where(table => table.OVC_MILITARY_TYPE.Equals(strOVC_MILITARY_TYPE));

            if (!strOVC_RECEIVE_DEPT_CODE.Equals("不限定"))
            {
                switch (strOVC_RECEIVE_DEPT_CODE)
                {
                    case "基隆地區":
                        query2 = query2.Where(table => table.OVC_PORT_CDE.Equals("TWKEL"));
                        break;
                    case "桃園地區":
                        query2 = query2.Where(table => table.OVC_PORT_CDE.Equals("TPE") || table.OVC_PORT_CDE.Equals("TTY"));
                        break;
                    case "高雄分遣組":
                        query2 = query2.Where(table => table.OVC_PORT_CDE.Equals("TWKHH") || table.OVC_PORT_CDE.Equals("KHH"));
                        break;
                    default:
                        break;
                }

            }

            if (!strOVC_SHIP_COMPANY.Equals("不限定"))
                query2 = query2.Where(table => table.OVC_SHIP_COMPANY.Equals(strOVC_SHIP_COMPANY));

            if (!strOVC_PURCH_NO.Equals(string.Empty))
                query2 = query2.Where(table => table.OVC_PURCH_NO.Contains(strOVC_PURCH_NO));

            if (!rdoIm_ExportDate.SelectedItem.Text.Equals("當日作業"))
            {
                DateTime start = Convert.ToDateTime(strODT_IMPORT_DATE1);
                DateTime end = Convert.ToDateTime(strODT_IMPORT_DATE2);
                query2 = query2.Where(table => table.ODT_IMPORT_DATE >= start && table.ODT_IMPORT_DATE < end);
            }
            else
            {
                DateTime now = DateTime.Now;
                query2 = query2.Where(table => table.ODT_IMPORT_DATE == now);
            }
            if (strOVC_IS_SECURITY == 1)
            {
                query2 = query2.Where(table => table.OVC_IS_SECURITY == strOVC_IS_SECURITY);
            }
            else
            {
                query2= query2.Where(table => table.OVC_IS_SECURITY == strOVC_IS_SECURITY || table.OVC_IS_SECURITY == null);
            }
            query2 = query2.Where(table =>! table.OVC_IS_ABROAD.Equals("國內"));
            query = query.Take(1000);
            dt2 = CommonStatic.LinqQueryToDataTable(query2);
            if (dt2.Rows.Count > 0)
            {
                for (var i = 0; i < dt2.Rows.Count; i++)
                {
                    try
                    {
                        //CF 轉換 CBM
                        dt2.Rows[i][10] = Math.Round(Convert.ToDouble(dt2.Rows[i][10]) / Convert.ToDouble(dt2.Rows[i][11].ToString().Replace("CF", "35.315")), 3);
                    }
                    catch
                    {

                    }
                    try
                    {
                        //地方轉換區域
                        dt.Rows[i][16] = dt.Rows[i][16].ToString().Replace("基隆港", "基隆地區").Replace("中正國際機場", "桃園地區").Replace("台灣桃園國際機場", "桃園地區").Replace("高雄港", "高雄分遣組").Replace("高雄小港機場", "高雄分遣組");
                    }
                    catch
                    {

                    }
                }
            }
            #endregion
            // dt+dt2
            dt.Merge(dt2);
            if(dt.Rows.Count !=0)
                dt.DefaultView.Sort = "ODT_IMPORT_DATE DESC";//排序
            ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_BLD, dt, strField);
            if (dt.Rows.Count > 0)
            {
                string bld_no_list = dt.Rows[0][7].ToString();
                for (var a = 1; a < dt.Rows.Count; a++)
                {
                    bld_no_list += "," + dt.Rows[a][7].ToString();
                    Session["bld_no_list"] = bld_no_list;
                }
            }
            if (GVTBGMT_BLD.Rows.Count > 0)
            {
                btnPrint.Visible = true;
            }
            else
            {
                btnPrint.Visible = false;
            }
        }
        protected void ImportQuery()
        {
            DataTable dt = new DataTable();

            string strImportOrExport = drpOVC_IMP_OR_EXP.SelectedItem.Text;
            string strOVC_SEA_OR_AIR = drpOVC_SEA_OR_AIR.SelectedItem.Text;
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedItem.Value;
            string strOVC_RECEIVE_DEPT_CODE = drpOVC_TRANSER_DEPT_CDE.SelectedItem.Text;
            string strOVC_SHIP_COMPANY = drpOVC_SHIP_COMPANY.SelectedItem.Text;
            string strOVC_PURCH_NO = txtOVC_PURCH_NO.Text;
            string strODT_IMPORT_DATE1 = txtODT_ACT_ARRIVE_DATE.Text;
            string strODT_IMPORT_DATE2 = txtODT_START_DATE.Text;
            int strOVC_IS_SECURITY = Convert.ToInt16(DropSmartUint.SelectedItem.Value);
            var query =
                from bld in MTS.TBGMT_BLD
                join edf in MTS.TBGMT_EDF on bld.OVC_BLD_NO equals edf.OVC_BLD_NO into p1
                from edf in p1.DefaultIfEmpty()
                join port in MTS.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals port.OVC_PORT_CDE into p2
                from port in p2.DefaultIfEmpty()
                join port2 in MTS.TBGMT_PORTS on bld.OVC_START_PORT equals port2.OVC_PORT_CDE into p3
                from port2 in p3.DefaultIfEmpty()
                join dept in MTS.TBGMT_DEPT_CLASS on bld.OVC_MILITARY_TYPE equals dept.OVC_CLASS into p4
                from dept in p4.DefaultIfEmpty()
                select new
                {
                    OVC_SEA_OR_AIR = bld.OVC_SEA_OR_AIR,
                    OVC_MILITARY_TYPE = bld.OVC_MILITARY_TYPE,
                    OVC_START_PORT = port.OVC_PORT_CHI_NAME,
                    OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY,//
                    OVC_PURCH_NO = edf.OVC_PURCH_NO,
                    ODT_IMPORT_DATE = bld.ODT_START_DATE,
                    OVC_IS_SECURITY = bld.OVC_IS_SECURITY,
                    OVC_BLD_NO = bld.OVC_BLD_NO,//
                    ONB_QUANITY = bld.ONB_QUANITY,//
                    ONB_WEIGHT = bld.ONB_WEIGHT,//
                    ONB_VOLUME = bld.ONB_VOLUME,//
                    bld.OVC_VOLUME_UNIT,
                    ONB_CARRIAGE = bld.ONB_CARRIAGE,
                    OVC_CLASS_NAME = dept.OVC_CLASS_NAME,
                    OVC_IS_ABROAD = port2.OVC_IS_ABROAD,
                    import_or_export = "出口",
                    OVC_RECEIVE_DEPT_CODE = port2.OVC_PORT_CHI_NAME,
                    OVC_PORT_CDE = port2.OVC_PORT_CDE,
                };
            if (!strOVC_SEA_OR_AIR.Equals("不限定"))
                query = query.Where(table => table.OVC_SEA_OR_AIR.Equals(strOVC_SEA_OR_AIR));

            if (!strOVC_MILITARY_TYPE.Equals(string.Empty))
                query = query.Where(table => table.OVC_MILITARY_TYPE.Equals(strOVC_MILITARY_TYPE));

            if (!strOVC_RECEIVE_DEPT_CODE.Equals("不限定"))
            {
                switch (strOVC_RECEIVE_DEPT_CODE)
                {
                    case "基隆地區":
                        query = query.Where(table => table.OVC_PORT_CDE.Equals("TWKEL"));
                        break;
                    case "桃園地區":
                        query = query.Where(table => table.OVC_PORT_CDE.Equals("TPE") || table.OVC_PORT_CDE.Equals("TTY"));
                        break;
                    case "高雄分遣組":
                        query = query.Where(table => table.OVC_PORT_CDE.Equals("TWKHH") || table.OVC_PORT_CDE.Equals("KHH"));
                        break;
                    default:
                        break;
                }
            }

            if (!strOVC_SHIP_COMPANY.Equals("不限定"))
                query = query.Where(table => table.OVC_SHIP_COMPANY.Equals(strOVC_SHIP_COMPANY));

            if (!strOVC_PURCH_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_PURCH_NO.Contains(strOVC_PURCH_NO));

            if (!rdoIm_ExportDate.SelectedItem.Text.Equals("當日作業"))
            {
                DateTime start = Convert.ToDateTime(strODT_IMPORT_DATE1);
                DateTime end = Convert.ToDateTime(strODT_IMPORT_DATE2);
                query = query.Where(table => table.ODT_IMPORT_DATE >= start && table.ODT_IMPORT_DATE < end);
            }
            else
            {
                DateTime now = DateTime.Now;
                query = query.Where(table => table.ODT_IMPORT_DATE == now);
            }
            if (strOVC_IS_SECURITY == 1)
            {
                query = query.Where(table => table.OVC_IS_SECURITY == strOVC_IS_SECURITY);
            }
            else
            {
                query = query.Where(table => table.OVC_IS_SECURITY == strOVC_IS_SECURITY || table.OVC_IS_SECURITY == null);
            }

            query = query.Where(table => table.OVC_IS_ABROAD.Equals("國內"));
            if (query.Count() > 1000)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
            query = query.Take(1000);
            query = query.OrderByDescending(t => t.ODT_IMPORT_DATE);
            dt = CommonStatic.LinqQueryToDataTable(query);
           
            if (dt.Rows.Count > 0) 
            {
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        //CF 轉換 CBM
                        dt.Rows[i][10] = Math.Round(Convert.ToDouble(dt.Rows[i][10]) / Convert.ToDouble(dt.Rows[i][11].ToString().Replace("CF", "35.315")), 3);
                    }
                    catch
                    {

                    }
                    try
                    {
                        //地方轉換區域
                        dt.Rows[i][16] = dt.Rows[i][16].ToString().Replace("基隆港", "基隆地區").Replace("中正國際機場", "桃園地區").Replace("台灣桃園國際機場", "桃園地區").Replace("高雄港", "高雄分遣組").Replace("高雄小港機場", "高雄分遣組");
                    }
                    catch
                    {

                    }
                }
            }
            
            ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_BLD, dt, strField);
            //儲存BLD
            if (dt.Rows.Count > 0)
            {
                string bld_no_list = dt.Rows[0][7].ToString();
                for (var a = 1; a < dt.Rows.Count; a++)
                {
                    bld_no_list += "," + dt.Rows[a][7].ToString();
                    Session["bld_no_list"] = bld_no_list;
                }
            }
            if (GVTBGMT_BLD.Rows.Count > 0)
            {
                btnPrint.Visible = true;
            }
            else
            {
                btnPrint.Visible = false;
            }
        }
        protected void ExportQuery()
        {
            DataTable dt = new DataTable();

            string strImportOrExport = drpOVC_IMP_OR_EXP.SelectedItem.Text;
            string strOVC_SEA_OR_AIR = drpOVC_SEA_OR_AIR.SelectedItem.Text;
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedItem.Value;
            string strOVC_RECEIVE_DEPT_CODE = drpOVC_TRANSER_DEPT_CDE.SelectedItem.Text;
            string strOVC_SHIP_COMPANY = drpOVC_SHIP_COMPANY.SelectedItem.Text;
            string strOVC_PURCH_NO = txtOVC_PURCH_NO.Text;
            string strODT_IMPORT_DATE1 = txtODT_ACT_ARRIVE_DATE.Text;
            string strODT_IMPORT_DATE2 = txtODT_START_DATE.Text;
            int strOVC_IS_SECURITY = Convert.ToInt16(DropSmartUint.SelectedItem.Value);
            var query =
                from bld in MTS.TBGMT_BLD
                join icr in MTS.TBGMT_ICR on bld.OVC_BLD_NO equals icr.OVC_BLD_NO into p1
                from icr in p1.DefaultIfEmpty()
                join port in MTS.TBGMT_PORTS on bld.OVC_ARRIVE_PORT equals port.OVC_PORT_CDE into p2
                from port in p2.DefaultIfEmpty()
                join port2 in MTS.TBGMT_PORTS on bld.OVC_START_PORT equals port2.OVC_PORT_CDE into p3
                from port2 in p3.DefaultIfEmpty()
                join dept in MTS.TBGMT_DEPT_CLASS on bld.OVC_MILITARY_TYPE equals dept.OVC_CLASS into p4
                from dept in p4.DefaultIfEmpty()
                select new
                {
                    OVC_SEA_OR_AIR = bld.OVC_SEA_OR_AIR,
                    OVC_MILITARY_TYPE = bld.OVC_MILITARY_TYPE,
                    OVC_START_PORT = port2.OVC_PORT_CHI_NAME,
                    OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY,
                    OVC_PURCH_NO = icr.OVC_PURCH_NO,
                    ODT_IMPORT_DATE = icr.ODT_IMPORT_DATE,
                    OVC_IS_SECURITY = bld.OVC_IS_SECURITY,
                    OVC_BLD_NO = bld.OVC_BLD_NO,
                    ONB_QUANITY = bld.ONB_QUANITY,
                    ONB_WEIGHT = bld.ONB_WEIGHT,
                    ONB_VOLUME = bld.ONB_VOLUME,
                    bld.OVC_VOLUME_UNIT,
                    ONB_CARRIAGE = bld.ONB_CARRIAGE,
                    OVC_CLASS_NAME = dept.OVC_CLASS_NAME,
                    OVC_IS_ABROAD = port2.OVC_IS_ABROAD,
                    import_or_export = "進口",
                    OVC_RECEIVE_DEPT_CODE = port.OVC_PORT_CHI_NAME,
                    OVC_PORT_CDE = port.OVC_PORT_CDE,
                };

            if (!strOVC_SEA_OR_AIR.Equals("不限定"))
                query = query.Where(table => table.OVC_SEA_OR_AIR.Equals(strOVC_SEA_OR_AIR));

            if (!strOVC_MILITARY_TYPE.Equals(string.Empty))
                query = query.Where(table => table.OVC_MILITARY_TYPE.Equals(strOVC_MILITARY_TYPE));

            if (!strOVC_RECEIVE_DEPT_CODE.Equals("不限定"))
            {
                switch (strOVC_RECEIVE_DEPT_CODE)
                {
                    case "基隆地區":
                        query = query.Where(table => table.OVC_PORT_CDE.Equals("TWKEL"));
                        break;
                    case "桃園地區":
                        query = query.Where(table => table.OVC_PORT_CDE.Equals("TPE") || table.OVC_PORT_CDE.Equals("TTY"));
                        break;
                    case "高雄分遣組":
                        query = query.Where(table => table.OVC_PORT_CDE.Equals("TWKHH") || table.OVC_PORT_CDE.Equals("KHH"));
                        break;
                    default:
                        break;
                }
 
            }

            if (!strOVC_SHIP_COMPANY.Equals("不限定"))
                query = query.Where(table => table.OVC_SHIP_COMPANY.Equals(strOVC_SHIP_COMPANY));

            if (!strOVC_PURCH_NO.Equals(string.Empty))
                query = query.Where(table => table.OVC_PURCH_NO.Contains(strOVC_PURCH_NO));

            if (!rdoIm_ExportDate.SelectedItem.Text.Equals("當日作業"))
            {
                DateTime start = Convert.ToDateTime(strODT_IMPORT_DATE1);
                DateTime end = Convert.ToDateTime(strODT_IMPORT_DATE2);
                query = query.Where(table => table.ODT_IMPORT_DATE >= start && table.ODT_IMPORT_DATE < end);
            }
            else
            {
                DateTime now = DateTime.Now;
                query = query.Where(table => table.ODT_IMPORT_DATE == now);
            }
            if (strOVC_IS_SECURITY == 1)
            {
                query = query.Where(table => table.OVC_IS_SECURITY == strOVC_IS_SECURITY);
            }
            else
            {
                query = query.Where(table => table.OVC_IS_SECURITY == strOVC_IS_SECURITY || table.OVC_IS_SECURITY==null);
            }
           
            query = query.Where(table => !table.OVC_IS_ABROAD.Equals("國內"));
            if (query.Count() > 1000)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
            query = query.Take(1000);
            query = query.OrderByDescending(t => t.ODT_IMPORT_DATE);
            dt = CommonStatic.LinqQueryToDataTable(query);
            if (dt.Rows.Count > 0)
            {
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        //CF 轉換 CBM
                        dt.Rows[i][10] = Math.Round(Convert.ToDouble(dt.Rows[i][10]) / Convert.ToDouble(dt.Rows[i][11].ToString().Replace("CF", "35.315")), 3);
                    }
                    catch
                    {

                    }
                    try
                    {
                        //地方轉換區域
                        dt.Rows[i][16] = dt.Rows[i][16].ToString().Replace("基隆港", "基隆地區").Replace("中正國際機場", "桃園地區").Replace("台灣桃園國際機場", "桃園地區").Replace("高雄港", "高雄分遣組").Replace("高雄小港機場", "高雄分遣組");
                    }
                    catch
                    {

                    }
                }
            }
            ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_BLD, dt, strField);
            //儲存BLD
            if (dt.Rows.Count > 0)
            {
                string bld_no_list = dt.Rows[0][7].ToString();
                for (var a = 1; a < dt.Rows.Count; a++)
                {
                    bld_no_list += "," + dt.Rows[a][7].ToString();
                    Session["bld_no_list"] = bld_no_list;
                }
            }
            if (GVTBGMT_BLD.Rows.Count > 0)
            {
                btnPrint.Visible = true;
            }
            else
            {
                btnPrint.Visible = false;
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChFont = new Font(bfChinese, 10f, Font.BOLD);
            Font Title_ChFont = new Font(bfChinese, 18f, Font.BOLD);
            MemoryStream Memory = new MemoryStream();
            var doc1 = new Document(PageSize.A4, 50, 50, 80, 50);  //la long
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            DateTime PrintTime = DateTime.Now;
            writer.PageEvent = new Header2();
            doc1.Open();
            PdfPTable Firsttable = new PdfPTable(12);
            Firsttable.SetWidths(new float[] { 2, 2, 2, 1,2, 2, 2, 1, 1, 1, 2, 2 });
            Firsttable.TotalWidth = 580F;
            Firsttable.LockedWidth = true;
            Firsttable.DefaultCell.FixedHeight = 40f;
            Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

            #region 日期區間
            string date = rdoIm_ExportDate.SelectedValue.Equals(true) ? 
                "民國" + (int.Parse(DateTime.Now.Year.ToString()) - 1911).ToString() + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day.ToString() + "日" :
                "民國" + (int.Parse(DateTime.Parse(txtODT_ACT_ARRIVE_DATE.Text).Year.ToString()) - 1911).ToString() + "年" + DateTime.Parse(txtODT_ACT_ARRIVE_DATE.Text).Month.ToString() + "月" + DateTime.Parse(txtODT_ACT_ARRIVE_DATE.Text).Day.ToString() + "日 至 民國"
                 + (int.Parse(DateTime.Parse(txtODT_START_DATE.Text).Year.ToString()) - 1911).ToString() + "年" + DateTime.Parse(txtODT_START_DATE.Text).Month.ToString() + "月" + DateTime.Parse(txtODT_START_DATE.Text).Day.ToString() + "日";
            PdfContentByte cb;
            BaseFont bf = null;
            Rectangle pageSize = doc1.PageSize;
            bf = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            cb = writer.DirectContent;
            cb.BeginText();
            cb.SetFontAndSize(bf, 12);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "日期：" + date, pageSize.GetLeft(10), pageSize.GetTop(70), 0);
            cb.EndText();
            #endregion
            double total = 0, kg = 0, cbm = 0, money = 0;
            string[] strTitle = { "提單編號", "案號", "承運航商", "件數", "重量(KG)", "體積(CBM)", "運費", "進出口", "海空運", "軍種", "接轉單位", "日期" };
            for (var i = 0; i < strTitle.Length; i++)
            {
                Firsttable.AddCell(new Phrase(strTitle[i], ChFont));
            }

            string[] list = Session["bld_no_list"].ToString().Split(',');
            if (GVTBGMT_BLD.Rows.Count > 0)
            {
                for (var i = 0; i < GVTBGMT_BLD.Rows.Count; i++)
                {
                    for (var x = 0; x < strTitle.Length; x++)
                    {
                        if (x == 0)
                        {
                            Firsttable.AddCell(new Phrase(list[i], ChFont));
                        }
                        else
                        {
                            Firsttable.AddCell(new Phrase(GVTBGMT_BLD.Rows[i].Cells[x].Text.Replace("&nbsp;", ""), ChFont));
                            #region 總計
                            if (x.Equals(3) && double.TryParse(GVTBGMT_BLD.Rows[i].Cells[x].Text.Replace("&nbsp;", ""), out double n))
                                total += double.Parse(GVTBGMT_BLD.Rows[i].Cells[x].Text.Replace("&nbsp;", ""));
                            if (x.Equals(4) && double.TryParse(GVTBGMT_BLD.Rows[i].Cells[x].Text.Replace("&nbsp;", ""), out n))
                                kg += double.Parse(GVTBGMT_BLD.Rows[i].Cells[x].Text.Replace("&nbsp;", ""));
                            if (x.Equals(5) && double.TryParse(GVTBGMT_BLD.Rows[i].Cells[x].Text.Replace("&nbsp;", ""), out n))
                                cbm += double.Parse(GVTBGMT_BLD.Rows[i].Cells[x].Text.Replace("&nbsp;", ""));
                            if (x.Equals(6) && double.TryParse(GVTBGMT_BLD.Rows[i].Cells[x].Text.Replace("&nbsp;", ""), out n))
                                money += double.Parse(GVTBGMT_BLD.Rows[i].Cells[x].Text.Replace("&nbsp;", ""));
                            #endregion
                        }
                    }
                }
            }
            PdfPCell foot = new PdfPCell(new Phrase("總共 " + GVTBGMT_BLD.Rows.Count + " 筆資料，總件數 " + total + " ，總重量 " + kg + " KG，總體積 " + cbm + " CBM，總運費 " + money + " USD", ChFont));
            foot.Colspan = 12;
            foot.Border = Rectangle.NO_BORDER;
            Firsttable.AddCell(foot);
            doc1.Add(Firsttable);
            doc1.Close();
            Response.Clear();//瀏覽器上顯示
            Response.AddHeader("Content-disposition", "attachment;filename=運量查詢.pdf");
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.Flush();
            Response.End();

        }

        protected void txtOVC_PURCH_NO_TextChanged(object sender, EventArgs e)
        {
           
        }
    }
}