using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.IO;
using System.Data.Entity;

namespace FCFDFE.pages.MTS.E
{
    public partial class MTS_E14_1 : System.Web.UI.Page
    {
        bool hasRows = false;
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        public string strMenuName = "", strMenuNameItem = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", drpOvcInfNo);
                    if (Session["gv"] != null && Request.QueryString["return"] != null)
                    {
                        FCommon.GridView_dataImport(GV_TBGMT_CINF, (DataTable)Session["gv"]);

                    }
                    int now = DateTime.Now.AddYears(-1911).Year;
                    for(var i = now; i >= 93; i--)
                    {
                        drpOvcInfNo.Items.Add(i.ToString());
                    }

                    bool boolimport = false;

                    string strtxtovcinfno, strtxtovcbldno, strdrpovcinfno;
                    if (FCommon.getQueryString(this, "strtxtovcinfno", out strtxtovcinfno, true))
                    {
                        boolimport = false;
                        txtOvcInfNo.Text = strtxtovcinfno;
                    }
                    if (FCommon.getQueryString(this, "strtxtovcbldno", out strtxtovcbldno, true))
                        txtOvcBldNo.Text = strtxtovcbldno;
                    if (FCommon.getQueryString(this, "strdrpovcinfno", out strdrpovcinfno, true))
                        FCommon.list_setValue(drpOvcInfNo, strdrpovcinfno);
                    if (boolimport)
                        dataimport();
                }
            }
        }
        protected void GV_TBGMT_CINF_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataimport();
            
        }
        
        protected void dataimport()
        {
            string strtxtovcinfno = txtOvcInfNo.Text;
            string strtxtovcbldno = txtOvcBldNo.Text;
            string strdrpovcinfno = drpOvcInfNo.SelectedValue;

            ViewState["strtxtovcinfno"] = strtxtovcinfno;
            ViewState["strtxtovcbldno"] = strtxtovcbldno;
            ViewState["strdrpovcinfno"] = strdrpovcinfno;

            var query = (from cinf in MTSE.TBGMT_CINF
                         join ics in MTSE.TBGMT_ICS on cinf.OVC_INF_NO equals ics.OVC_INF_NO
                         join bld in MTSE.TBGMT_BLD on ics.OVC_BLD_NO equals bld.OVC_BLD_NO
                         where bld.OVC_BLD_NO.Contains(strtxtovcbldno)
                         select new
                         {
                             OVC_INF_NO = cinf.OVC_INF_NO,
                             OVC_GIST = cinf.OVC_GIST,
                             OVC_BUDGET = cinf.OVC_BUDGET,
                             OVC_PURPOSE_TYPE = cinf.OVC_PURPOSE_TYPE,
                             ONB_AMOUNT = cinf.ONB_AMOUNT,
                             OVC_BUDGET_INF_NO = cinf.OVC_BUDGET_INF_NO,
                             OVC_NOTE = cinf.OVC_NOTE,
                             OVC_PLN_CONTENT = cinf.OVC_PLN_CONTENT,

                         }).Distinct();
            query = query.Where(table => table.OVC_INF_NO.Substring(4, 3).Equals(strdrpovcinfno));
            if (!strtxtovcinfno.Equals(string.Empty))
                    query = query.Where(table => table.OVC_INF_NO.Contains(strtxtovcinfno));


            if (query.Count() > 1000)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "由於資料龐大，只顯示前1000筆");
            query = query.Take(1000);
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBGMT_CINF, dt);
            Session["gv"] = dt;
            
        }
        protected string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "strtxtovcinfno", ViewState["strtxtovcinfno"], true);
            FCommon.setQueryString(ref strQueryString, "strtxtovcbldno", ViewState["strtxtovcbldno"], true);
            FCommon.setQueryString(ref strQueryString, "strdrpovcinfno", ViewState["strdrpovcinfno"], true);
            return strQueryString;

        }
        protected void print(string ovc_inf_no,string ovc_bld_no)
        {
            var query = from cinf in MTSE.TBGMT_CINF
                        join ics in MTSE.TBGMT_ICS on cinf.OVC_INF_NO equals ics.OVC_INF_NO into ics
                        from ics2 in ics.DefaultIfEmpty()
                        join bld in MTSE.TBGMT_BLD on ics2.OVC_BLD_NO equals bld.OVC_BLD_NO into bld
                        from bld2 in bld.DefaultIfEmpty()
                        join port_start in MTSE.TBGMT_PORTS on bld2.OVC_START_PORT equals port_start.OVC_PORT_CDE into port_start
                        from port_start2 in port_start.DefaultIfEmpty()
                        join port_arrive in MTSE.TBGMT_PORTS on bld2.OVC_ARRIVE_PORT equals port_arrive.OVC_PORT_CDE into port_arrive
                        from port_arrive2 in port_arrive.DefaultIfEmpty()

                        select new
                        {
                            OVC_INF_NO = cinf.OVC_INF_NO,
                            OVC_BLD_NO = ics2.OVC_BLD_NO,
                            OVC_INLAND_CARRIAGE = ics2.OVC_INLAND_CARRIAGE,
                            OVC_SEA_OR_AIR = bld2.OVC_SEA_OR_AIR,
                            OVC_START_PORT = port_start2.OVC_PORT_CHI_NAME,
                            OVC_ARRIVE_PORT = port_arrive2.OVC_PORT_CHI_NAME
                        };

            query = query.Where(table => table.OVC_INF_NO.Contains(ovc_inf_no));
            query = query.Where(table => table.OVC_BLD_NO.Contains(ovc_bld_no));
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);



            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\FONTS\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//設定字型
            Font ChFont = new Font(bfChinese, 12f, Font.BOLD);
            Font Title_ChFont = new Font(bfChinese, 18f, Font.BOLD);
            MemoryStream Memory = new MemoryStream();
            var doc1 = new Document(PageSize.A4, 50, 50, 80, 50);
            PdfWriter writer = PdfWriter.GetInstance(doc1, Memory);//檔案 下載
            DateTime PrintTime = DateTime.Now;
            doc1.Open();

            PdfPTable Firsttable = new PdfPTable(5);
            Firsttable.SetWidths(new float[] { 3, 3, 2, 3, 3 });

            Firsttable.TotalWidth = 500F;
            Firsttable.LockedWidth = true;
            Firsttable.DefaultCell.FixedHeight = 40f;
            Firsttable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Firsttable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            Firsttable.AddCell(new Phrase("提單編號", ChFont));
            Firsttable.AddCell(new Phrase("海空運費", ChFont));
            Firsttable.AddCell(new Phrase("運輸方法", ChFont));
            Firsttable.AddCell(new Phrase("起運港口", ChFont));
            Firsttable.AddCell(new Phrase("目的港口", ChFont));
            int total = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                for(int x=1;x<6;x++)
                {
                    Firsttable.AddCell(new Phrase(dt.Rows[i][x].ToString(), ChFont));
                    if(x==2)
                    {
                        try
                        {
                            total += Convert.ToInt32(dt.Rows[i][x]);
                        }
                        catch { }
                       
                    }
                }
            }
            if(dt.Rows.Count<15)
            {
                for (var i = 0; i < (15 - dt.Rows.Count); i++)
                {
                    for (int x = 1; x < 6; x++)
                    {
                        Firsttable.AddCell(new Phrase("", ChFont));
                    }
                }
            }
            else
            {
                for (var i = 0; i < (15 - (dt.Rows.Count-15)); i++)
                {
                    for (int x = 1; x < 6; x++)
                    {
                        Firsttable.AddCell(new Phrase("", ChFont));
                    }
                }
            }
            PdfContentByte sum = writer.DirectContent;
            Rectangle pagesize = doc1.PageSize;
            sum.BeginText();
            sum.SetFontAndSize(bfChinese, 12f);
            sum.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "合計:" + total.ToString("##,#") + "元", pagesize.GetLeft
                (50), pagesize.GetBottom(50),0);
            sum.EndText();
            doc1.Add(Firsttable);
            doc1.Close();


            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=國防部國防採購室採購及外購軍品作業費結報申請表.pdf");
            Response.OutputStream.Write(Memory.GetBuffer(), 0, Memory.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            Response.End();
        }

        protected void GV_TBGMT_CINF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBGMT_CINF.DataKeys[gvrIndex].Value.ToString();
            string strQueryString = getQueryString();
            FCommon.setQueryString(ref strQueryString, "id", id, true);

            switch (e.CommandName)
            {
                case "btnSave":
                    Response.Redirect($"MTS_E14_2.aspx{strQueryString}");
                    break;
                case "btnDel":
                    try
                    {
                        var cinfModel = new TBGMT_CINF();
                        cinfModel = MTSE.TBGMT_CINF.Where(table => table.OVC_INF_NO == id).FirstOrDefault();
                        MTSE.Entry(cinfModel).State = EntityState.Deleted;
                        TBGMT_ICS ics = new TBGMT_ICS();
                       
                        
                            var q = MTSE.TBGMT_ICS.Where(table => table.OVC_INF_NO == id);
                        foreach(var item in q)
                        {
                            item.OVC_INF_NO = null;
                        }

                        MTSE.SaveChanges();
                        dataimport();
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除投保通知書" + id + "成功");
                    }
                    catch (Exception ex)
                    {
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", "刪除投保通知書" + id + "失敗");
                    }
                    break;
                case "btnPrint":
                    print(id, e.CommandArgument.ToString());
                    break;
                default:
                    break;
            }
        }
    }
}