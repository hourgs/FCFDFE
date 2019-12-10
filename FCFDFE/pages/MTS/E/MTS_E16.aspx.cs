using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Globalization;

namespace FCFDFE.pages.MTS.E
{
    public partial class MTS_E16 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        private MTSEntities mtse = new MTSEntities();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (!IsPostBack)
                {
                    list_dataImport(drpOdtInvDate);
                    DataTable dt = CommonStatic.ListToDataTable(mtse.TBGMT_DEPT_CLASS.Select(x => x).ToList());
                    list_dataImport2(drpOvcMilitaryType, dt, "OVC_CLASS_NAME", "OVC_CLASS");
                    panD.Visible = false;
                    panS.Visible = true;
                    btnPrint.Visible = false;
                    FCommon.Controls_Attributes("readonly", "true", txtOdtInvDate1, txtOdtInvDate2);
                }
            }

        }
        #region 副程式
        public void list_dataImport(ListControl list)
        {
            //先將下拉式選單清空
            list.Items.Clear();

            int CalDateYear = Convert.ToInt16(DateTime.Now.ToString("yyyy")) - 1911;
            int num = CalDateYear;
            for (int i = num; i > 93; i--)
            {
                list.Items.Add(Convert.ToString(i));
            }
        }
        public void list_dataImport2(ListControl list, DataTable dt, string textField, string valueField)
        {
            //先將下拉式選單清空
            list.Items.Clear();
            list.AppendDataBoundItems = true;
            list.Items.Add("不限定");
            list.DataSource = dt;
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataBind();
        }
        private void datas()
        {
            string strODT_APPLY_DATE = drpOdtInvDate.SelectedItem.ToString();
            if (strODT_APPLY_DATE.Length != 3)
            {
                if (strODT_APPLY_DATE.Length == 2)
                    strODT_APPLY_DATE = "0" + strODT_APPLY_DATE;
                else if (strODT_APPLY_DATE.Length == 1)
                    strODT_APPLY_DATE = "00" + strODT_APPLY_DATE;
                else
                    strODT_APPLY_DATE = "000";
            }
            var query =
                from dept in mtse.TBGMT_DEPT_CLASS
                join bld in mtse.TBGMT_BLD on dept.OVC_CLASS equals bld.OVC_MILITARY_TYPE
                join ics in mtse.TBGMT_ICS on bld.OVC_BLD_NO equals ics.OVC_BLD_NO
                join cinf in mtse.TBGMT_CINF on ics.OVC_INF_NO equals cinf.OVC_INF_NO
                where ics.OVC_ICS_NO.StartsWith("ICS" + strODT_APPLY_DATE)
                group ics by dept.OVC_CLASS into g
                select new
                {
                    OVC_CLASS = g.Key,
                    ODT_SHOULD = g.Sum(p => p.OVC_INLAND_CARRIAGE)
                };
            DataTable dtics = new DataTable();
            DataRow drics;
            dtics.Columns.Add(new DataColumn("OVC_CLASS", typeof(String)));
            dtics.Columns.Add(new DataColumn("OVC_SHOULD", typeof(Decimal)));
            foreach (var aa in query)
            {
                drics = dtics.NewRow();
                drics[0] = aa.OVC_CLASS;
                if (aa.ODT_SHOULD == null)
                {
                    drics[1] = 0;
                }
                else
                {
                    drics[1] = aa.ODT_SHOULD;
                }

                dtics.Rows.Add(drics);
            }

            var queryyes =
                from dept in mtse.TBGMT_DEPT_CLASS
                join bld in mtse.TBGMT_BLD on dept.OVC_CLASS equals bld.OVC_MILITARY_TYPE
                join ics in mtse.TBGMT_ICS on bld.OVC_BLD_NO equals ics.OVC_BLD_NO
                join cinf in mtse.TBGMT_CINF on ics.OVC_INF_NO equals cinf.OVC_INF_NO
                where cinf.OVC_IS_PAID == "已付款"
                where ics.OVC_ICS_NO.StartsWith("ICS" + strODT_APPLY_DATE)
                group ics by dept.OVC_CLASS into g
                select new
                {
                    OVC_CLASS = g.Key,
                    ODT_SHOULD = g.Sum(p => p.OVC_INLAND_CARRIAGE)
                };
            DataTable dticsyes = new DataTable();
            DataRow dricsyes;
            dticsyes.Columns.Add(new DataColumn("OVC_CLASS", typeof(String)));
            dticsyes.Columns.Add(new DataColumn("OVC_SHOULD", typeof(Decimal)));
            foreach (var aa in queryyes)
            {
                dricsyes = dticsyes.NewRow();
                dricsyes[0] = aa.OVC_CLASS;
                if (aa.ODT_SHOULD == null)
                {
                    dricsyes[1] = 0;
                }
                else
                {
                    dricsyes[1] = aa.ODT_SHOULD;
                }

                dticsyes.Rows.Add(dricsyes);
            }

            var queryno =
               from dept in mtse.TBGMT_DEPT_CLASS
               join bld in mtse.TBGMT_BLD on dept.OVC_CLASS equals bld.OVC_MILITARY_TYPE
               join ics in mtse.TBGMT_ICS on bld.OVC_BLD_NO equals ics.OVC_BLD_NO
               join cinf in mtse.TBGMT_CINF on ics.OVC_INF_NO equals cinf.OVC_INF_NO
               where cinf.OVC_IS_PAID == "未付款"
               where ics.OVC_ICS_NO.StartsWith("ICS" + strODT_APPLY_DATE)
               group ics by dept.OVC_CLASS into g
               select new
               {
                   OVC_CLASS = g.Key,
                   ODT_SHOULD = g.Sum(p => p.OVC_INLAND_CARRIAGE)
               };
            DataTable dticsno = new DataTable();
            DataRow dricsno;
            dticsno.Columns.Add(new DataColumn("OVC_CLASS", typeof(String)));
            dticsno.Columns.Add(new DataColumn("OVC_SHOULD", typeof(Decimal)));
            foreach (var aa in queryno)
            {
                dricsno = dticsno.NewRow();
                dricsno[0] = aa.OVC_CLASS;
                if (aa.ODT_SHOULD == null)
                {
                    dricsno[1] = 0;
                }
                else
                {
                    dricsno[1] = aa.ODT_SHOULD;
                }

                dticsno.Rows.Add(dricsno);
            }

            var queryall = from dept in mtse.TBGMT_DEPT_CLASS.AsEnumerable()
                           join cinf in dtics.AsEnumerable()
                           on dept.OVC_CLASS equals cinf.Field<string>("OVC_CLASS")
                           into tmpcinf
                           from cinf in tmpcinf.DefaultIfEmpty()
                           join cinfyes in dticsyes.AsEnumerable()
                           on dept.OVC_CLASS equals cinfyes.Field<string>("OVC_CLASS")
                           into tmpcinfyes
                           from cinfyes in tmpcinfyes.DefaultIfEmpty()
                           join cinfno in dticsno.AsEnumerable()
                           on dept.OVC_CLASS equals cinfno.Field<string>("OVC_CLASS")
                           into tmpcinfno
                           from cinfno in tmpcinfno.DefaultIfEmpty()
                           select new
                           {
                               OVC_CLASS_NAME = dept.OVC_CLASS_NAME,
                               ODT_SHOULD = cinf?.Field<decimal>("OVC_SHOULD") ?? Decimal.Zero,
                               ODT_YES = cinfyes?.Field<decimal>("OVC_SHOULD") ?? Decimal.Zero,
                               ODT_NO = cinfno?.Field<decimal>("OVC_SHOULD") ?? Decimal.Zero,
                           };
            if (queryall.Count() > 1000)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
            queryall = queryall.Take(1000);
            DataTable dt = new DataTable();
            dt = CommonStatic.LinqQueryToDataTable(queryall);

            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_CINF, dt );
        }
        private void datad()
        {
            string strODTINVDATE1 = txtOdtInvDate1.Text;
            string strODTINVDATE2 = txtOdtInvDate2.Text;
            string strDEPT = drpOvcMilitaryType.SelectedItem.Text;
            string strOVCTYPE = drpOvcType.SelectedItem.Text;
            string strCOMPANY = drpOvcShipCompany.SelectedItem.Text;

            ViewState["txtOdtInvDate1"] = txtOdtInvDate1.Text;
            ViewState["txtOdtInvDate2"] = txtOdtInvDate2.Text;
            ViewState["drpOvcMilitaryType"] = drpOvcMilitaryType.SelectedItem.Text;
            ViewState["drpOvcType"] = drpOvcType.SelectedItem.Text;
            ViewState["drpOvcShipCompany"] = drpOvcShipCompany.SelectedItem.Text;

            if (strODTINVDATE1.Equals(string.Empty) && strODTINVDATE2.Equals(string.Empty) && strDEPT.Equals("不限定") && strOVCTYPE.Equals("不限定") && strCOMPANY.Equals("不限定"))
            {
                FCommon.AlertShow(Panel1, "danger", "系統訊息", "請至少填寫一項條件");
            }
            else
            {
                var query =
                from ics in mtse.TBGMT_ICS
                join bld in mtse.TBGMT_BLD on ics.OVC_BLD_NO equals bld.OVC_BLD_NO
                join cinf in mtse.TBGMT_CINF on ics.OVC_INF_NO equals cinf.OVC_INF_NO
                join dept in mtse.TBGMT_DEPT_CLASS on bld.OVC_MILITARY_TYPE equals dept.OVC_CLASS
                select new
                {
                    OVC_BLD_NO = ics.OVC_BLD_NO,
                    OVC_INF_NO = ics.OVC_INF_NO,
                    OVC_ICS_NO = ics.OVC_ICS_NO,
                    OVC_INLAND_CARRIAGE = ics.OVC_INLAND_CARRIAGE,
                    OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY,
                    OVC_CLASS_NAME = dept.OVC_CLASS_NAME,
                    OVC_IS_PAID = cinf.OVC_IS_PAID,
                    ODT_DATE = ics.ODT_MODIFY_DATE
                };
                if (strDEPT != "不限定")
                    query = query.Where(table => table.OVC_CLASS_NAME == strDEPT);
                if (strOVCTYPE == "未申請")
                    query = query.Where(table => table.OVC_IS_PAID == null);
                if (strOVCTYPE == "未付款")
                    query = query.Where(table => table.OVC_IS_PAID == "未付款");
                if (strOVCTYPE == "已付款")
                    query = query.Where(table => table.OVC_IS_PAID == "已付款");
                if (strCOMPANY != "不限定")
                    query = query.Where(table => table.OVC_SHIP_COMPANY == strCOMPANY);
                if (!strODTINVDATE1.Equals(string.Empty))
                {
                    DateTime d1 = Convert.ToDateTime(strODTINVDATE1);
                    query = query.Where(table => table.ODT_DATE >= d1);
                }
                if (!strODTINVDATE2.Equals(string.Empty))
                {
                    DateTime d2 = Convert.ToDateTime(strODTINVDATE2);
                    query = query.Where(table => table.ODT_DATE <= d2);
                }
                if (query.Count() > 1000)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
                query = query.Take(1000);
                DataTable dt = new DataTable();
                dt = CommonStatic.LinqQueryToDataTable(query);

                ViewState["hasRows2"] = FCommon.GridView_dataImport(GV_TBGMT_DETAIL_CINF, dt);
                btnPrint.Visible = true;
            }

        }
        private void print()
        {
            #region
            string strODTINVDATE1 = ViewState["txtOdtInvDate1"].ToString();
            string strODTINVDATE2 = ViewState["txtOdtInvDate2"].ToString();
            string strDEPT = ViewState["drpOvcMilitaryType"].ToString();
            string strOVCTYPE = ViewState["drpOvcType"].ToString();
            string strCOMPANY = ViewState["drpOvcShipCompany"].ToString();

            var query =
               from ics in mtse.TBGMT_ICS
               join bld in mtse.TBGMT_BLD on ics.OVC_BLD_NO equals bld.OVC_BLD_NO
               join cinf in mtse.TBGMT_CINF on ics.OVC_INF_NO equals cinf.OVC_INF_NO
               join dept in mtse.TBGMT_DEPT_CLASS on bld.OVC_MILITARY_TYPE equals dept.OVC_CLASS
               select new
               {
                   OVC_BLD_NO = ics.OVC_BLD_NO,
                   OVC_INF_NO = ics.OVC_INF_NO,
                   OVC_ICS_NO = ics.OVC_ICS_NO,
                   OVC_INLAND_CARRIAGE = ics.OVC_INLAND_CARRIAGE,
                   OVC_SHIP_COMPANY = bld.OVC_SHIP_COMPANY,
                   OVC_CLASS_NAME = dept.OVC_CLASS_NAME,
                   OVC_IS_PAID = cinf.OVC_IS_PAID,
                   ODT_DATE = ics.ODT_MODIFY_DATE
               };
            if (strDEPT != "不限定")
                query = query.Where(table => table.OVC_CLASS_NAME == strDEPT);
            if (strOVCTYPE == "未申請")
                query = query.Where(table => table.OVC_IS_PAID == null);
            if (strOVCTYPE == "未付款")
                query = query.Where(table => table.OVC_IS_PAID == "未付款");
            if (strOVCTYPE == "已付款")
                query = query.Where(table => table.OVC_IS_PAID == "已付款");
            if (strCOMPANY != "不限定")
                query = query.Where(table => table.OVC_SHIP_COMPANY == strCOMPANY);
            if (!strODTINVDATE1.Equals(string.Empty))
            {
                DateTime d1 = Convert.ToDateTime(strODTINVDATE1);
                query = query.Where(table => table.ODT_DATE >= d1);
            }
            if (!strODTINVDATE2.Equals(string.Empty))
            {
                DateTime d2 = Convert.ToDateTime(strODTINVDATE2);
                query = query.Where(table => table.ODT_DATE <= d2);
            }
            #endregion

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime create =DateTime.Now;
            var datetime = string.Format("中華民國{0}年{1}月{2}日", taiwanCalendar.GetYear(create), create.Month, create.Day);

            string path = Request.PhysicalApplicationPath;//取得檔案絕對路徑
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChTitle = new Font(bfChinese, 16, Font.BOLD);
            Font ChTabTitle = new Font(bfChinese, 12);
            Font ChFont = new Font(bfChinese, 11);
            var doc1 = new Document(PageSize.A4, 0, 0, 50, 80);
            MemoryStream Memory = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            doc1.Open();

            PdfPTable table1 = new PdfPTable(6);
            table1.TotalWidth = 1200F;
            table1.SetWidths(new float[] {4,3,3,3,2,2});
            table1.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table1.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell title = new PdfPCell(new Phrase("國軍外購軍品運費統計表\n\n", ChTitle));
            title.VerticalAlignment = Element.ALIGN_MIDDLE;
            title.HorizontalAlignment = Element.ALIGN_CENTER;
            title.Border = Rectangle.NO_BORDER;
            title.Colspan = 6;
            table1.AddCell(title);
            PdfPCell top = new PdfPCell(new Phrase("製表人:\n\n", ChFont));
            top.Border = Rectangle.NO_BORDER;
            top.VerticalAlignment = Element.ALIGN_MIDDLE;
            top.HorizontalAlignment = Element.ALIGN_LEFT;
            table1.AddCell(top);
            PdfPCell top2 = new PdfPCell(new Phrase("聯絡電話:\n\n", ChFont));
            top2.Border = Rectangle.NO_BORDER;
            top2.VerticalAlignment = Element.ALIGN_MIDDLE;
            top2.HorizontalAlignment = Element.ALIGN_LEFT;
            top2.Colspan = 2;
            table1.AddCell(top2);
            PdfPCell top3 = new PdfPCell(new Phrase(datetime+"\n\n", ChFont));
            top3.Border = Rectangle.NO_BORDER;
            top3.VerticalAlignment = Element.ALIGN_MIDDLE;
            top3.HorizontalAlignment = Element.ALIGN_RIGHT;
            top3.Colspan = 3;
            table1.AddCell(top3);

            table1.AddCell(new Phrase("提單號碼", ChTabTitle));
            table1.AddCell(new Phrase("運費編號", ChTabTitle));
            table1.AddCell(new Phrase("運費", ChTabTitle));
            table1.AddCell(new Phrase("承運航商", ChTabTitle));
            table1.AddCell(new Phrase("軍種", ChTabTitle));
            table1.AddCell(new Phrase("分類", ChTabTitle));

            foreach (var t in query)
            {
                decimal i = Convert.ToDecimal(t.OVC_INLAND_CARRIAGE);
                string a;
                a = i.ToString("#,0");
                table1.AddCell(new Phrase(t.OVC_BLD_NO, ChFont));
                table1.AddCell(new Phrase(t.OVC_ICS_NO, ChFont));
                PdfPCell cell = new PdfPCell(new Phrase(a, ChFont));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table1.AddCell(cell);
                table1.AddCell(new Phrase(t.OVC_SHIP_COMPANY, ChFont));
                table1.AddCell(new Phrase(t.OVC_CLASS_NAME, ChFont));
                table1.AddCell(new Phrase(t.OVC_IS_PAID, ChFont));
            }
            
            doc1.Add(table1);
            doc1.Close();
            Response.Clear();//瀏覽器上顯示
            Response.AddHeader("Content-disposition", "attachment;filename=國君外購軍品運費統計表.pdf");
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.Flush();
            Response.End();
        }
        #endregion
        protected void btnToD_Click(object sender, EventArgs e)
        {
            panD.Visible = true;
            panS.Visible = false;
        }
        protected void btnToS_Click(object sender, EventArgs e)
        {
            panS.Visible = true;
            panD.Visible = false;
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            datas();
        }
        protected void btnQuery1_Click(object sender, EventArgs e)
        {
            datad();
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            print();
        }
        protected void GV_TBGMT_CINF_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }
        protected void GV_TBGMT_DETAIL_CINF_PreRender(object sender, EventArgs e)
        {
            bool hasRows2 = false;
            if (ViewState["hasRows2"] != null)
                hasRows2 = Convert.ToBoolean(ViewState["hasRows2"]);
            FCommon.GridView_PreRenderInit(sender, hasRows2);
        }
    }
}