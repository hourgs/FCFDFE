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
    public partial class MTS_G13 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        string strImagePagh = Content.Variable.strImagePath;
        private MTSEntities MTS = new MTSEntities();
        Common FCommon = new Common();
        string[] strField = { "OVC_INF_NO", "OVC_CLASS_NAME", "OVC_IE_TYPE", "Insurance_Premium", "OVC_BUDGET", "ONB_AMOUNT", "OVC_IS_PAID", "OVC_SHIP_COMPANY", "ODT_APPLY_DATE" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (!IsPostBack)
                {
                    FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                    FCommon.Controls_Attributes("readonly", "true", txtODT_APPLY_DATE_S, txtODT_APPLY_DATE_E);
                    //新增軍種dropdownlist item
                    DataTable DEPT_CLASS = CommonStatic.ListToDataTable(MTS.TBGMT_DEPT_CLASS.ToList());
                    FCommon.list_dataImport(drpOVC_MILITARY_TYPE, DEPT_CLASS, "OVC_CLASS_NAME", "OVC_CLASS", true);

                    txtODT_APPLY_DATE_S.Text = DateTime.Now.ToString("yyyy/MM/dd");
                    txtODT_APPLY_DATE_E.Text = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");
                    //判斷如果是海運費、空運費 則新增選項
                }
                if (drpONB_CARRIAGE.SelectedItem.Text == "海運費")
                {
                    drpOVC_FINAL_INS_AMOUNT.Visible = true;
                    drpOVC_FINAL_INS_AMOUNT2.Visible = false;
                }
                else if (drpONB_CARRIAGE.SelectedItem.Text == "空運費")
                {
                    drpOVC_FINAL_INS_AMOUNT.Visible = false;
                    drpOVC_FINAL_INS_AMOUNT2.Visible = true;
                }
                else
                {
                    drpOVC_FINAL_INS_AMOUNT.Visible = false;
                    drpOVC_FINAL_INS_AMOUNT2.Visible = false;
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            
            string strOVC_TOF_NO = txtOVC_TOF_NO.Text;
            string strOVC_MILITARY_TYPE = drpOVC_MILITARY_TYPE.SelectedItem.Text;
            string strOVC_IMP_OR_EXP = drpOVC_IMP_OR_EXP.SelectedItem.Text;
            string strONB_CARRIAGE = drpONB_CARRIAGE.SelectedItem.Text;
            string strOVC_FINAL_INS_AMOUNT = drpOVC_FINAL_INS_AMOUNT.SelectedItem.Text;
            string strOVC_BUDGET = drpOVC_BUDGET.SelectedItem.Text;
            string strOVC_PAYMENT_TYPE = drpOVC_PAYMENT_TYPE.SelectedItem.Text;
            string strODT_APPLY_DATE_S = txtODT_APPLY_DATE_S.Text;
            string strODT_APPLY_DATE_E = txtODT_APPLY_DATE_E.Text;
            #region bld ics cinf 
                DataTable dt = new DataTable();
                var query =
                    from bld in MTS.TBGMT_BLD
                    join ics in MTS.TBGMT_ICS on bld.OVC_BLD_NO equals ics.OVC_BLD_NO into p1
                    from ics in p1.DefaultIfEmpty()
                    join cinf in MTS.TBGMT_CINF on ics.OVC_INF_NO equals cinf.OVC_INF_NO into p2
                    from cinf in p2.DefaultIfEmpty()
                    join dept_class in MTS.TBGMT_DEPT_CLASS on bld.OVC_MILITARY_TYPE equals dept_class.OVC_CLASS into p3
                    from dept_class in p3.DefaultIfEmpty()
                    select new
                    {
                        OVC_INF_NO = cinf.OVC_INF_NO,
                        OVC_CLASS_NAME = dept_class.OVC_CLASS_NAME,
                        OVC_IE_TYPE = cinf.OVC_IMP_OR_EXP,
                        Insurance_Premium = bld.OVC_SEA_OR_AIR,
                        OVC_BUDGET = cinf.OVC_BUDGET,
                        ONB_AMOUNT =cinf.ONB_AMOUNT,
                        OVC_IS_PAID = cinf.OVC_IS_PAID,
                        OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY,
                        ODT_APPLY_DATE = cinf.ODT_APPLY_DATE,
                    };

                if (!strOVC_TOF_NO.Equals(string.Empty))
                    query = query.Where(table => table.OVC_INF_NO.Contains(strOVC_TOF_NO));

                if (!strOVC_MILITARY_TYPE.Equals("請選擇"))
                    query = query.Where(table => table.OVC_CLASS_NAME.Equals(strOVC_MILITARY_TYPE));

                if (!strOVC_IMP_OR_EXP.Equals("不限定"))
                    query = query.Where(table => table.OVC_IE_TYPE.Equals(strOVC_IMP_OR_EXP));

                if (!strOVC_BUDGET.Equals("不限定"))
                    query = query.Where(table => table.OVC_BUDGET.Equals(strOVC_BUDGET));

                if (!strOVC_PAYMENT_TYPE.Equals("不限定"))
                    query = query.Where(table => table.OVC_IS_PAID.Equals(strOVC_PAYMENT_TYPE));

                if (rdoODT_APPLY_DATE.SelectedItem.Text == "當日作業")
                {
                    DateTime now = DateTime.Now;
                    query = query.Where(table => table.ODT_APPLY_DATE == now);
                }
                else
                {
                    DateTime start = Convert.ToDateTime(strODT_APPLY_DATE_S);
                    DateTime end = Convert.ToDateTime(strODT_APPLY_DATE_E);
                    query = query.Where(table => table.ODT_APPLY_DATE >= start && table.ODT_APPLY_DATE < end);
                }

                if(drpONB_CARRIAGE.SelectedItem.Text == "海運費" )
                {
                    if (drpOVC_FINAL_INS_AMOUNT.SelectedItem.Text != "不限定")
                        query = query.Where(table => table.OVC_SHIP_COMPANY.Equals(drpOVC_FINAL_INS_AMOUNT.SelectedItem.Text));
                }
                if (drpONB_CARRIAGE.SelectedItem.Text == "空運費")
                {
                    if (drpOVC_FINAL_INS_AMOUNT2.SelectedItem.Text != "不限定")
                        query = query.Where(table => table.OVC_SHIP_COMPANY.Equals(drpOVC_FINAL_INS_AMOUNT2.SelectedItem.Text));
                }
            if (strONB_CARRIAGE == "海運費")
                query = query.Where(table => table.Insurance_Premium.Equals("海運"));
            if (strONB_CARRIAGE == "空運費")
                query = query.Where(table => table.Insurance_Premium.Equals("空運"));
            
            query = query.Take(1000);
            dt = CommonStatic.LinqQueryToDataTable(query.Distinct());
            if (dt.Rows.Count > 0)
            {
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i][3] = dt.Rows[i][3] + "費";
                }
            }
            #endregion
            //出口
            #region einf einn
            DataTable dt2 = new DataTable();

                var query2 =
                    from einf in MTS.TBGMT_EINF
                    join einn in MTS.TBGMT_EINN on einf.OVC_INF_NO equals einn.OVC_INF_NO into p1
                    from einn in p1.DefaultIfEmpty()
                    join dept_class in MTS.TBGMT_DEPT_CLASS on einn.OVC_MILITARY_TYPE equals dept_class.OVC_CLASS into p2
                    from dept_class in p2.DefaultIfEmpty()
                    select new
                    {
                        OVC_INF_NO = einf.OVC_INF_NO,
                        OVC_CLASS_NAME = dept_class.OVC_CLASS_NAME,
                        OVC_IE_TYPE = "出口",
                        Insurance_Premium =" 保險費",
                        OVC_BUDGET= einf.OVC_BUDGET,
                        ONB_AMOUNT = einf.ONB_AMOUNT,
                        OVC_IS_PAID = einf.OVC_IS_PAID,
                        OVC_SHIP_COMPANY = einf.OVC_NOTE,
                        ODT_APPLY_DATE = einf.ODT_APPLY_DATE,
                    };
                if (!strOVC_TOF_NO.Equals(string.Empty))
                    query2 = query2.Where(table => table.OVC_INF_NO.Contains(strOVC_TOF_NO));

                if (!strOVC_MILITARY_TYPE.Equals("請選擇"))
                    query2 = query2.Where(table => table.OVC_CLASS_NAME.Equals(strOVC_MILITARY_TYPE));

                if (!strOVC_IMP_OR_EXP.Equals("不限定"))
                    query2 = query2.Where(table => table.OVC_IE_TYPE.Equals(strOVC_IMP_OR_EXP));

                if (!strOVC_BUDGET.Equals("不限定"))
                    query2 = query2.Where(table => table.OVC_BUDGET.Equals(strOVC_BUDGET));

                if (!strOVC_PAYMENT_TYPE.Equals("不限定"))
                    query2 = query2.Where(table => table.OVC_IS_PAID.Equals(strOVC_PAYMENT_TYPE));

                if (rdoODT_APPLY_DATE.SelectedItem.Text == "當日作業")
                {
                    DateTime now = DateTime.Now;
                    query2 = query2.Where(table => table.ODT_APPLY_DATE == now);
                }
                else
                {
                    DateTime start = Convert.ToDateTime(strODT_APPLY_DATE_S);
                    DateTime end = Convert.ToDateTime(strODT_APPLY_DATE_E);
                    query2 = query2.Where(table => table.ODT_APPLY_DATE >= start && table.ODT_APPLY_DATE < end);
                }
                dt2 = CommonStatic.LinqQueryToDataTable(query2.Distinct());
                #endregion
            //進口
            #region iinf  iinn
                DataTable dt3 = new DataTable();
                var query3 =
                    from iinf in MTS.TBGMT_IINF
                    join iinn in MTS.TBGMT_IINN on iinf.OVC_INF_NO equals iinn.OVC_INF_NO into p1
                    from iinn in p1.DefaultIfEmpty()
                    join dept_class in MTS.TBGMT_DEPT_CLASS on iinn.OVC_MILITARY_TYPE equals dept_class.OVC_CLASS into p2
                    from dept_class in p2.DefaultIfEmpty()
                    select new
                    {
                        OVC_INF_NO = iinf.OVC_INF_NO,
                        OVC_CLASS_NAME = dept_class.OVC_CLASS_NAME,
                        OVC_IE_TYPE = "進口",
                        Insurance_Premium = "保險費",
                        OVC_BUDGET = iinf.OVC_BUDGET,
                        ONB_AMOUNT = iinf.ONB_AMOUNT,
                        OVC_IS_PAID = iinf.OVC_IS_PAID,
                        OVC_SHIP_COMPANY = iinf.OVC_NOTE,
                        ODT_APPLY_DATE = iinf.ODT_APPLY_DATE,
                    };
                if (!strOVC_TOF_NO.Equals(string.Empty))
                    query3 = query3.Where(table => table.OVC_INF_NO.Contains(strOVC_TOF_NO));

                if (!strOVC_MILITARY_TYPE.Equals("請選擇"))
                    query3 = query3.Where(table => table.OVC_CLASS_NAME.Equals(strOVC_MILITARY_TYPE));

                if (!strOVC_IMP_OR_EXP.Equals("不限定"))
                    query3 = query3.Where(table => table.OVC_IE_TYPE.Equals(strOVC_IMP_OR_EXP));

                if (!strOVC_BUDGET.Equals("不限定"))
                    query3 = query3.Where(table => table.OVC_BUDGET.Equals(strOVC_BUDGET));

                if (!strOVC_PAYMENT_TYPE.Equals("不限定"))
                    query3 = query3.Where(table => table.OVC_IS_PAID.Equals(strOVC_PAYMENT_TYPE));

                if (rdoODT_APPLY_DATE.SelectedItem.Text == "當日作業")
                {
                    DateTime now = DateTime.Now;
                    query3 = query3.Where(table => table.ODT_APPLY_DATE == now);
                }
                else
                {
                    DateTime start = Convert.ToDateTime(strODT_APPLY_DATE_S);
                    DateTime end = Convert.ToDateTime(strODT_APPLY_DATE_E);
                    query3 = query3.Where(table => table.ODT_APPLY_DATE >= start && table.ODT_APPLY_DATE < end);
                }
                dt3 = CommonStatic.LinqQueryToDataTable(query3.Distinct());
                #endregion
            #region tof
                DataTable dt4 = new DataTable();
                var query4 =
                    from tof in MTS.TBGMT_TOF
                    select new
                    {
                        OVC_INF_NO = tof.OVC_TOF_NO,
                        OVC_CLASS_NAME = tof.OVC_MILITARY_TYPE,
                        OVC_IE_TYPE = tof.OVC_IE_TYPE,
                        Insurance_Premium = "作業費",
                        OVC_BUDGET = tof.OVC_BUDGET,
                        ONB_AMOUNT = tof.ONB_AMOUNT,
                        OVC_IS_PAID = tof.OVC_IS_PAID,
                        OVC_SHIP_COMPANY = tof.OVC_NOTE,
                        ODT_APPLY_DATE = tof.ODT_APPLY_DATE,
                    };
                if (!strOVC_TOF_NO.Equals(string.Empty))
                    query4 = query4.Where(table => table.OVC_INF_NO.Contains(strOVC_TOF_NO));

                if (!strOVC_MILITARY_TYPE.Equals("請選擇"))
                    query4 = query4.Where(table => table.OVC_CLASS_NAME.Equals(strOVC_MILITARY_TYPE));

                if (!strOVC_IMP_OR_EXP.Equals("不限定"))
                    query4 = query4.Where(table => table.OVC_IE_TYPE.Equals(strOVC_IMP_OR_EXP));

                if (!strOVC_BUDGET.Equals("不限定"))
                    query4 = query4.Where(table => table.OVC_BUDGET.Equals(strOVC_BUDGET));

                if (!strOVC_PAYMENT_TYPE.Equals("不限定"))
                    query4 = query4.Where(table => table.OVC_IS_PAID.Equals(strOVC_PAYMENT_TYPE));

                if (rdoODT_APPLY_DATE.SelectedItem.Text == "當日作業")
                {
                    DateTime now = DateTime.Now;
                    query4 = query4.Where(table => table.ODT_APPLY_DATE == now);
                }
                else
                {
                    DateTime start = Convert.ToDateTime(strODT_APPLY_DATE_S);
                    DateTime end = Convert.ToDateTime(strODT_APPLY_DATE_E);
                    query4 = query4.Where(table => table.ODT_APPLY_DATE >= start && table.ODT_APPLY_DATE < end);
                }
                dt4 = CommonStatic.LinqQueryToDataTable(query4.Distinct());
            #endregion
            if(drpONB_CARRIAGE.SelectedItem.Text == "不限定")
            {
                //判斷進口/出口 對應新增dt
                if (strOVC_IMP_OR_EXP == "不限定")
                {
                    dt.Merge(dt2);
                    dt.Merge(dt3);
                }
                else if (strOVC_IMP_OR_EXP == "進口")
                {
                    dt.Merge(dt3);
                }
                else
                {
                    dt.Merge(dt2);
                }

                dt.Merge(dt4);
                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_TOF, dt, strField);
            }
            else if(drpONB_CARRIAGE.SelectedItem.Text == "海運費" || drpONB_CARRIAGE.SelectedItem.Text == "空運費")
            {
                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_TOF, dt, strField);
            }
            else if(drpONB_CARRIAGE.SelectedItem.Text == "保險費")
            {
                if (strOVC_IMP_OR_EXP == "不限定")
                {
                    dt2.Merge(dt3);
                    ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_TOF, dt2, strField);
                }
                else if(strOVC_IMP_OR_EXP == "進口")
                {
                    ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_TOF, dt3, strField);
                }
                else
                {
                    ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_TOF, dt2 , strField);
                }
            }
            else
            {
                ViewState["hasRows"] = FCommon.GridView_dataImport(GVTBGMT_TOF, dt4, strField);
            }

            if (GVTBGMT_TOF.Rows.Count > 0)
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
            if (GVTBGMT_TOF.Rows.Count > 0)
            {
                BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
                Font ChFont = new Font(bfChinese, 12f, Font.BOLD);
                Font SmallChFont = new Font(bfChinese, 10f, Font.BOLD);
                Font Title_ChFont = new Font(bfChinese, 18f, Font.BOLD);
                MemoryStream Memory = new MemoryStream();
                var doc1 = new Document(PageSize.A4, 80, 80, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
                doc1.Open();
                PdfPTable Firsttable = new PdfPTable(9);
                Firsttable.SetWidths(new float[] { 3, 1, 2, 2, 2, 3, 2, 2, 2 });
                Firsttable.TotalWidth = 580F;
                Firsttable.LockedWidth = true;
                Firsttable.DefaultCell.FixedHeight = 40f;
                Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell Title = new PdfPCell(new Phrase("軍品運雜費統計", Title_ChFont));
                Title.HorizontalAlignment = Element.ALIGN_CENTER;
                Title.VerticalAlignment = Element.ALIGN_MIDDLE;
                Title.Colspan = 9;
                Title.Border = Rectangle.NO_BORDER;
                Firsttable.AddCell(Title);
                PdfPCell PrintTime = new PdfPCell(new Phrase("列印日期：" + DateTime.Now.AddYears(-1911).ToString("yy/MM/dd"), SmallChFont));
                PrintTime.HorizontalAlignment = Element.ALIGN_RIGHT;
                PrintTime.VerticalAlignment = Element.ALIGN_MIDDLE;
                PrintTime.Colspan = 9;
                PrintTime.Border = Rectangle.NO_BORDER;
                Firsttable.AddCell(PrintTime);
                string[] strTitle = { "結報申請表編號", "軍種", "進出口別", "運保費別", "預算科目", "金額(新台幣)", "付款區分", "備考", "結報日期" };

                for (var i = 0; i < strTitle.Length; i++)
                {
                    Firsttable.AddCell(new Phrase(strTitle[i], ChFont));
                }
                for(var i = 0; i < GVTBGMT_TOF.Rows.Count; i++)
                {
                    Firsttable.AddCell(new Phrase(GVTBGMT_TOF.Rows[i].Cells[0].Text.Replace("&nbsp;", ""), ChFont));
                    Firsttable.AddCell(new Phrase(GVTBGMT_TOF.Rows[i].Cells[1].Text.Replace("&nbsp;", ""), ChFont));
                    Firsttable.AddCell(new Phrase(GVTBGMT_TOF.Rows[i].Cells[2].Text.Replace("&nbsp;", ""), ChFont));
                    Firsttable.AddCell(new Phrase(GVTBGMT_TOF.Rows[i].Cells[3].Text.Replace("&nbsp;", ""), ChFont));
                    Firsttable.AddCell(new Phrase(GVTBGMT_TOF.Rows[i].Cells[4].Text.Replace("&nbsp;", ""), ChFont));
                    Firsttable.AddCell(new Phrase(GVTBGMT_TOF.Rows[i].Cells[5].Text.Replace("&nbsp;", ""), ChFont));
                    Firsttable.AddCell(new Phrase(GVTBGMT_TOF.Rows[i].Cells[6].Text.Replace("&nbsp;", ""), ChFont));
                    Firsttable.AddCell(new Phrase(GVTBGMT_TOF.Rows[i].Cells[7].Text.Replace("&nbsp;", ""), ChFont));
                    Firsttable.AddCell(new Phrase(GVTBGMT_TOF.Rows[i].Cells[8].Text.Replace("&nbsp;", ""), ChFont));
                }
                string fileName = HttpUtility.UrlEncode("軍品運雜費統計報表.pdf");

                doc1.Add(Firsttable);
                doc1.Close();
                Response.Clear();//瀏覽器上顯示
                Response.AddHeader("Content-disposition", "attachment;filename=" + fileName);
                Response.ContentType = "application/octet-stream";
                Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.Flush();
                Response.End();
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "查詢資料訊息");
            }
        }
        protected void GVTBGMT_TOF_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        protected void txtOVC_TOF_NO_TextChanged(object sender, EventArgs e)
        {
            txtOVC_TOF_NO.Text = txtOVC_TOF_NO.Text.ToUpper();   //轉大寫
        }
    }
}