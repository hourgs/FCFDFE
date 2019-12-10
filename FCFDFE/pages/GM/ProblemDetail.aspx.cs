using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using System.Web.UI;
using System.IO;
using Word = Microsoft.Office.Interop.Word;
using FCFDFE.Entity.GMModel;
using NPOI.XWPF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;
using System.Web;
using System.Collections;

namespace FCFDFE.pages.GM
{
    public partial class ProblemDetail : Page
    {
        Common FCommon = new Common();
        GMEntities de = new GMEntities();
        TBM1407 tableSys = new TBM1407();
        PROBLEM_DATA pbdt = new PROBLEM_DATA();
        ACCOUNT AC = new ACCOUNT();
        TBMDEPT DEPT = new TBMDEPT();
        TBM1407 SYS = new TBM1407();
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            Guid claim_SN = new Guid(Request.QueryString["PRO_SN"].ToString());
            if (!IsPostBack)
            {
                if (claim_SN != null)
                {
                    pbdt = de.PROBLEM_DATA.Where(table => table.PRO_SN == claim_SN).FirstOrDefault();
                    AC = de.ACCOUNTs.Where(table => table.USER_ID == pbdt.OVC_CREATE_ID).FirstOrDefault();
                    DEPT = de.TBMDEPTs.Where(table => table.OVC_DEPT_CDE == AC.DEPT_SN).FirstOrDefault();
                    SYS = de.TBM1407.Where(table => table.OVC_PHR_CATE == "SA").Where(table => table.OVC_PHR_ID == pbdt.PROSYS_SN).FirstOrDefault();
                }
                if(pbdt != null)
                {
                    labC_SN_SYS.Text = SYS.OVC_PHR_DESC;
                    labDate.Text = pbdt.ODT_CREATE_DATE.ToString();
                    labOVC_ONNAME.Text = DEPT.OVC_ONNAME;
                    labPerson.Text = AC.USER_NAME;
                    labPhone.Text = AC.IUSER_PHONE;
                    labPRO_DESC.Text = pbdt.PRO_DESC;
                    labODT_GET_DATE.Text = pbdt.ODT_GET_DATE.ToString();
                    labODT_PLAN_DATE.Text = pbdt.ODT_PLAN_DATE.ToString();
                    labQuestionProcess.Text = "";

                    ViewState["PRO_ID"] = pbdt.PRO_ID;
                    ViewState["OVC_PHR_DESC"] = SYS.OVC_PHR_DESC;//子系統名稱
                    ViewState["ODT_CREATE_DATE"] = pbdt.ODT_CREATE_DATE.ToString();//建檔日期
                    ViewState["OVC_ONNAME"] = DEPT.OVC_ONNAME;//業管單位
                    ViewState["USER_NAME"] = AC.USER_NAME;
                    ViewState["IUSER_PHONE"] = AC.IUSER_PHONE;
                    ViewState["PRO_DESC"] = pbdt.PRO_DESC;
                    ViewState["ODT_GET_DATE"] = pbdt.ODT_GET_DATE.ToString();
                    ViewState["ODT_PLAN_DATE"] = pbdt.ODT_PLAN_DATE.ToString();

                }
            }

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            createwordfile();
        }
        #region word製作報表-問題單

        private void createwordfile()
        {
            string strproid = ViewState["PRO_ID"].ToString();
            string OVC_PHR_DESC = ViewState["OVC_PHR_DESC"].ToString();
            string ODT_CREATE_DATE = ViewState["ODT_CREATE_DATE"].ToString();
            string OVC_ONNAME = ViewState["OVC_ONNAME"].ToString();
            string USER_NAME = ViewState["USER_NAME"].ToString();
            string IUSER_PHONE = ViewState["IUSER_PHONE"].ToString();
            string PRO_DESC = ViewState["PRO_DESC"].ToString();
            string ODT_GET_DATE = ViewState["ODT_GET_DATE"].ToString();
            string ODT_PLAN_DATE = ViewState["ODT_PLAN_DATE"].ToString();

            XWPFDocument doc = new XWPFDocument();      //创建新的word文档
            CT_Fonts ct = new CT_Fonts
            {
                eastAsia = "標楷體",
            };
            XWPFStyles sty = doc.CreateStyles();
            sty.SetDefaultFonts(ct);

            #region 標題
            //製作標題
            XWPFParagraph p1 = doc.CreateParagraph();   //向新文档中添加段落
            p1.Alignment = ParagraphAlignment.CENTER; //段落对其方式为居中
            XWPFRun r1 = p1.CreateRun();                //向该段落中添加文字
            r1.SetText("問題(保修)申請單");
            r1.FontFamily = "標楷體";
            r1.IsBold = true;
            r1.FontSize = 16;

            XWPFParagraph p2 = doc.CreateParagraph();   //向新文档中添加段落
            p2.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r2 = p2.CreateRun();                //向该段落中添加文字
            r2.SetText("編號:" + strproid + "                                                     " + "第1頁 共1頁");
            r2.FontFamily = "標楷體";
            r2.FontSize = 10;

            #endregion 標題

            #region 上表格

            XWPFTable table = doc.CreateTable(1, 2);//创建1行1列表
            CT_Tbl m_CTTbl = doc.Document.body.GetTblArray()[0];//获得文档第一张表
            m_CTTbl.AddNewTblPr().AddNewTblW().w = "8200";//表宽度
            m_CTTbl.AddNewTblPr().AddNewTblW().type = ST_TblWidth.dxa;


            table.RemoveRow(0);//去掉第一行空白的
            CT_Row nr = new CT_Row();
            XWPFTableRow mr = new XWPFTableRow(nr, table);
            mr.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)350;//设置行高
            nr.AddNewTrPr().AddNewTrHeight().val = (ulong)350;//设置行高（这两行都得有）
            table.AddRow(mr);
            mr.CreateCell().SetText("1.子系統名稱：" + OVC_PHR_DESC);
            mr.GetCell(0).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);
            mr.CreateCell().SetText("7.審查確認：");
            XWPFParagraph pIOm1 = table.GetRow(0).GetCell(1).AddParagraph();
            XWPFRun rIOm1 = pIOm1.CreateRun();
            rIOm1.SetText("□修訂完畢，同意結案。");
            XWPFParagraph pIOm2 = table.GetRow(0).GetCell(1).AddParagraph();
            XWPFRun rIOm2 = pIOm2.CreateRun();
            rIOm2.SetText("(由業管單位核章確認)");


            mr.GetCell(1).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);
            CT_Row cr1 = new CT_Row();
            XWPFTableRow tr1 = new XWPFTableRow(cr1, table);
            tr1.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            cr1.AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            table.AddRow(tr1);
            tr1.CreateCell().SetText("2.日期：" + ODT_CREATE_DATE);
            tr1.GetCell(0).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);
            tr1.CreateCell();
            tr1.GetCell(1).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

            CT_Row cr2 = new CT_Row();
            XWPFTableRow tr2 = new XWPFTableRow(cr2, table);
            tr2.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            cr2.AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            table.AddRow(tr2);
            tr2.CreateCell().SetText("3.業管單位及人員、電話：" + OVC_ONNAME + USER_NAME + IUSER_PHONE);
            tr2.GetCell(0).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);
            tr2.CreateCell();
            tr2.GetCell(1).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

            CT_Row cr3 = new CT_Row();
            XWPFTableRow tr3 = new XWPFTableRow(cr3, table);
            tr3.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            cr3.AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            table.AddRow(tr3);
            tr3.CreateCell().SetText("4.問題描述、影響與需求規格：");
            tr3.GetCell(0).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);
            XWPFParagraph pIOb21 = table.GetRow(3).GetCell(0).AddParagraph();
            XWPFRun rIOb21 = pIOb21.CreateRun();
            rIOb21.SetText(PRO_DESC);
            XWPFParagraph pIOb22 = table.GetRow(3).GetCell(0).AddParagraph();
            XWPFRun rIOb22 = pIOb22.CreateRun();
            rIOb22.SetText("");
            XWPFParagraph pIOb23 = table.GetRow(3).GetCell(0).AddParagraph();
            XWPFRun rIOb23 = pIOb23.CreateRun();
            rIOb23.SetText("");
            tr3.CreateCell().SetText("8.補註說明：\n");
            tr3.GetCell(1).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

            CT_Row cr4 = new CT_Row();
            XWPFTableRow tr4 = new XWPFTableRow(cr4, table);
            tr4.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            cr4.AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            table.AddRow(tr4);
            tr4.CreateCell();
            tr4.GetCell(0).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);
            tr4.CreateCell();
            tr4.GetCell(1).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

            CT_Row cr5 = new CT_Row();
            XWPFTableRow tr5 = new XWPFTableRow(cr5, table);
            tr5.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            cr5.AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            table.AddRow(tr5);
            tr5.CreateCell();
            tr5.GetCell(0).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);
            tr5.CreateCell();
            tr5.GetCell(1).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

            CT_Row cr6 = new CT_Row();
            XWPFTableRow tr6 = new XWPFTableRow(cr6, table);
            tr6.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            cr6.AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            table.AddRow(tr4);
            tr6.CreateCell();
            tr6.GetCell(0).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);
            tr6.CreateCell();
            tr6.GetCell(1).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

            CT_Row cr7 = new CT_Row();
            XWPFTableRow tr7 = new XWPFTableRow(cr7, table);
            tr7.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            cr7.AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            table.AddRow(tr7);
            tr7.CreateCell();
            tr7.GetCell(0).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);
            tr7.CreateCell();
            tr7.GetCell(1).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

            CT_TcPr m_Pr = table.GetRow(0).GetCell(0).GetCTTc().AddNewTcPr();
            m_Pr.tcW = new CT_TblWidth();
            m_Pr.tcW.w = "5400";//单元格宽
            m_Pr.tcW.type = ST_TblWidth.dxa;

            m_Pr = table.GetRow(0).GetCell(1).GetCTTc().AddNewTcPr();
            m_Pr.tcW = new CT_TblWidth();
            m_Pr.tcW.w = "2800";//单元格宽
            m_Pr.tcW.type = ST_TblWidth.dxa;
            m_Pr.AddNewVMerge().val = ST_Merge.restart;
            m_Pr.AddNewVAlign().val = ST_VerticalJc.center;

            m_Pr = table.GetRow(1).GetCell(1).GetCTTc().AddNewTcPr();
            m_Pr.AddNewVMerge().val = ST_Merge.@continue;
            m_Pr = table.GetRow(2).GetCell(1).GetCTTc().AddNewTcPr();
            m_Pr.AddNewVMerge().val = ST_Merge.@continue;

            m_Pr = table.GetRow(3).GetCell(0).GetCTTc().AddNewTcPr();
            m_Pr.AddNewVMerge().val = ST_Merge.restart;
            m_Pr = table.GetRow(4).GetCell(0).GetCTTc().AddNewTcPr();
            m_Pr.AddNewVMerge().val = ST_Merge.@continue;
            m_Pr = table.GetRow(5).GetCell(0).GetCTTc().AddNewTcPr();
            m_Pr.AddNewVMerge().val = ST_Merge.@continue;
            m_Pr = table.GetRow(6).GetCell(0).GetCTTc().AddNewTcPr();
            m_Pr.AddNewVMerge().val = ST_Merge.@continue;
            m_Pr = table.GetRow(7).GetCell(0).GetCTTc().AddNewTcPr();
            m_Pr.AddNewVMerge().val = ST_Merge.@continue;

            m_Pr = table.GetRow(3).GetCell(1).GetCTTc().AddNewTcPr();
            m_Pr.AddNewVMerge().val = ST_Merge.restart;
            m_Pr = table.GetRow(4).GetCell(1).GetCTTc().AddNewTcPr();
            m_Pr.AddNewVMerge().val = ST_Merge.@continue;
            m_Pr = table.GetRow(5).GetCell(1).GetCTTc().AddNewTcPr();
            m_Pr.AddNewVMerge().val = ST_Merge.@continue;
            m_Pr = table.GetRow(6).GetCell(1).GetCTTc().AddNewTcPr();
            m_Pr.AddNewVMerge().val = ST_Merge.@continue;
            m_Pr = table.GetRow(7).GetCell(1).GetCTTc().AddNewTcPr();
            m_Pr.AddNewVMerge().val = ST_Merge.@continue;

            table.SetLeftBorder(XWPFTable.XWPFBorderType.DOUBLE, 0, 0, "");
            table.SetRightBorder(XWPFTable.XWPFBorderType.DOUBLE, 0, 0, "");
            table.SetBottomBorder(XWPFTable.XWPFBorderType.DOUBLE, 0, 0, "");
            table.SetTopBorder(XWPFTable.XWPFBorderType.DOUBLE, 0, 0, "");

            #endregion

            #region 中表格
            XWPFParagraph p12 = doc.CreateParagraph();   //向新文档中添加段落
            p12.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r12 = p12.CreateRun();                //向该段落中添加文字 
            r12.SetText("相關佐證資料與系統畫面請於第2頁後填寫                              (7~8由系統業管承參填寫確認，印");
            r12.FontFamily = "標楷體";
            r12.FontSize = 8;

            XWPFParagraph p13 = doc.CreateParagraph();   //向新文档中添加段落
            p13.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r13 = p13.CreateRun();                //向该段落中添加文字
            r13.SetText("(1~4由使用承參提出時填寫，可由電腦繕打)                            出後交管制單位結業審核)");
            r13.FontFamily = "標楷體";
            r13.FontSize = 8;

            XWPFTable bottomtable = doc.CreateTable(1, 1);//创建1行1列表
            CT_Tbl m_CTTblbottom = doc.Document.body.GetTblArray()[1];//获得文档第一张表
            m_CTTblbottom.AddNewTblPr().AddNewTblW().w = "8200";//表宽度
            m_CTTblbottom.AddNewTblPr().AddNewTblW().type = ST_TblWidth.dxa;

            bottomtable.RemoveRow(0);//去掉第一行空白的
            CT_Row nrbottom = new CT_Row();
            XWPFTableRow mrbottom = new XWPFTableRow(nrbottom, bottomtable);
            mrbottom.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)550;//设置行高
            nrbottom.AddNewTrPr().AddNewTrHeight().val = (ulong)550;//设置行高（这两行都得有）
            bottomtable.AddRow(mrbottom);
            mrbottom.CreateCell().SetText("5.問題審查與工作指派：");
            mrbottom.GetCell(0).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);
            XWPFParagraph pIOb1 = bottomtable.GetRow(0).GetCell(0).AddParagraph();
            XWPFRun rIOb1 = pIOb1.CreateRun();
            rIOb1.SetText("（1）問題種類：□程式錯誤□系統功能調整□轉為精進項目□硬體故障□資料修訂或其他");
            XWPFParagraph pIOb2 = bottomtable.GetRow(0).GetCell(0).AddParagraph();
            XWPFRun rIOb2 = pIOb2.CreateRun();
            rIOb2.SetText("（2）接獲通知日期：" + ODT_GET_DATE);
            XWPFParagraph pIOb3 = bottomtable.GetRow(0).GetCell(0).AddParagraph();
            XWPFRun rIOb3 = pIOb3.CreateRun();
            rIOb3.SetText("（3）預計完成日期：" + ODT_PLAN_DATE);


            CT_Row crb1 = new CT_Row();
            XWPFTableRow trb1 = new XWPFTableRow(crb1, bottomtable);
            //trb1.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            //crb1.AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            bottomtable.AddRow(trb1);
            trb1.CreateCell();

            CT_Row crb2 = new CT_Row();
            XWPFTableRow trb2 = new XWPFTableRow(crb2, bottomtable);
            //trb2.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            //crb2.AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            bottomtable.AddRow(trb2);
            trb2.CreateCell();

            CT_Row crb3 = new CT_Row();
            XWPFTableRow trb3 = new XWPFTableRow(crb3, bottomtable);
            //trb3.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            //crb3.AddNewTrPr().AddNewTrHeight().val = (ulong)350;
            bottomtable.AddRow(trb3);
            trb3.CreateCell();

            CT_Row nrbottom2 = new CT_Row();
            XWPFTableRow mrbottom2 = new XWPFTableRow(nrbottom2, bottomtable);
            mrbottom2.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)350;//设置行高
            nrbottom2.AddNewTrPr().AddNewTrHeight().val = (ulong)350;//设置行高（这两行都得有）
            bottomtable.AddRow(mrbottom2);
            mrbottom2.CreateCell().SetText("6.問題處理情形：（欄位不足請自行延伸，或以附件說明）");
            mrbottom2.GetCell(0).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);
            XWPFParagraph pIOb31 = bottomtable.GetRow(4).GetCell(0).AddParagraph();
            XWPFRun rIOb31 = pIOb31.CreateRun();
            rIOb31.SetText("");
            XWPFParagraph pIOb32 = bottomtable.GetRow(4).GetCell(0).AddParagraph();
            XWPFRun rIOb32 = pIOb32.CreateRun();
            rIOb32.SetText("");
            XWPFParagraph pIOb33 = bottomtable.GetRow(4).GetCell(0).AddParagraph();
            XWPFRun rIOb33 = pIOb33.CreateRun();
            rIOb33.SetText("");
            XWPFParagraph pIOb35 = bottomtable.GetRow(4).GetCell(0).AddParagraph();
            XWPFRun rIOb35 = pIOb35.CreateRun();
            rIOb35.SetText("");
            XWPFParagraph pIOb36 = bottomtable.GetRow(4).GetCell(0).AddParagraph();
            XWPFRun rIOb36 = pIOb33.CreateRun();
            rIOb36.SetText("");
            XWPFParagraph pIOb37 = bottomtable.GetRow(4).GetCell(0).AddParagraph();
            XWPFRun rIOb37 = pIOb37.CreateRun();
            rIOb37.SetText("");
            XWPFParagraph pIOb34 = bottomtable.GetRow(4).GetCell(0).AddParagraph();
            XWPFRun rIOb34 = pIOb34.CreateRun();
            rIOb34.SetText("維護案負責人：　　　　　　　　　　                    　實際完成日期：　　年　　月　　日");

            CT_Row crb21 = new CT_Row();
            XWPFTableRow trb21 = new XWPFTableRow(crb21, bottomtable);
            //trb21.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)550;
            //crb21.AddNewTrPr().AddNewTrHeight().val = (ulong)550;
            bottomtable.AddRow(trb21);
            trb21.CreateCell();

            CT_Row crb22 = new CT_Row();
            XWPFTableRow trb22 = new XWPFTableRow(crb22, bottomtable);
            //trb22.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)550;
            //crb22.AddNewTrPr().AddNewTrHeight().val = (ulong)550;
            bottomtable.AddRow(trb22);
            trb22.CreateCell();

            CT_Row crb23 = new CT_Row();
            XWPFTableRow trb23 = new XWPFTableRow(crb23, bottomtable);
            //trb23.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)550;
            //crb23.AddNewTrPr().AddNewTrHeight().val = (ulong)550;
            bottomtable.AddRow(trb23);
            trb23.CreateCell();

            CT_Row crb24 = new CT_Row();
            XWPFTableRow trb24 = new XWPFTableRow(crb24, bottomtable);
            bottomtable.AddRow(trb24);
            trb24.CreateCell();

            CT_Row crb25 = new CT_Row();    
            XWPFTableRow trb25 = new XWPFTableRow(crb25, bottomtable);
            bottomtable.AddRow(trb25);
            trb25.CreateCell();

            CT_Row crb26 = new CT_Row();
            XWPFTableRow trb26 = new XWPFTableRow(crb26, bottomtable);
            bottomtable.AddRow(trb26);
            trb26.CreateCell();

            CT_Row crb27 = new CT_Row();
            XWPFTableRow trb27 = new XWPFTableRow(crb27, bottomtable);
            bottomtable.AddRow(trb27);
            trb27.CreateCell();

            CT_TcPr m_Prb = bottomtable.GetRow(0).GetCell(0).GetCTTc().AddNewTcPr();
            m_Prb.tcW = new CT_TblWidth();
            m_Prb.tcW.w = "8200";//单元格宽
            m_Prb.tcW.type = ST_TblWidth.dxa;
            m_Prb.AddNewVMerge().val = ST_Merge.restart;
            m_Prb = bottomtable.GetRow(1).GetCell(0).GetCTTc().AddNewTcPr();
            m_Prb.AddNewVMerge().val = ST_Merge.@continue;
            m_Prb = bottomtable.GetRow(2).GetCell(0).GetCTTc().AddNewTcPr();
            m_Prb.AddNewVMerge().val = ST_Merge.@continue;
            m_Prb = bottomtable.GetRow(3).GetCell(0).GetCTTc().AddNewTcPr();
            m_Prb.AddNewVMerge().val = ST_Merge.@continue;

            m_Prb = bottomtable.GetRow(4).GetCell(0).GetCTTc().AddNewTcPr();
            m_Prb.AddNewVMerge().val = ST_Merge.restart;
            m_Prb = bottomtable.GetRow(5).GetCell(0).GetCTTc().AddNewTcPr();
            m_Prb.AddNewVMerge().val = ST_Merge.@continue;
            m_Prb = bottomtable.GetRow(6).GetCell(0).GetCTTc().AddNewTcPr();
            m_Prb.AddNewVMerge().val = ST_Merge.@continue;
            m_Prb = bottomtable.GetRow(7).GetCell(0).GetCTTc().AddNewTcPr();
            m_Prb.AddNewVMerge().val = ST_Merge.@continue;
            m_Prb = bottomtable.GetRow(8).GetCell(0).GetCTTc().AddNewTcPr();
            m_Prb.AddNewVMerge().val = ST_Merge.@continue;
            m_Prb = bottomtable.GetRow(9).GetCell(0).GetCTTc().AddNewTcPr();
            m_Prb.AddNewVMerge().val = ST_Merge.@continue;
            m_Prb = bottomtable.GetRow(10).GetCell(0).GetCTTc().AddNewTcPr();
            m_Prb.AddNewVMerge().val = ST_Merge.@continue;
            m_Prb = bottomtable.GetRow(11).GetCell(0).GetCTTc().AddNewTcPr();
            m_Prb.AddNewVMerge().val = ST_Merge.@continue;

            bottomtable.SetLeftBorder(XWPFTable.XWPFBorderType.SINGLE, 15, 0, "");
            bottomtable.SetRightBorder(XWPFTable.XWPFBorderType.SINGLE, 15, 0, "");
            bottomtable.SetBottomBorder(XWPFTable.XWPFBorderType.SINGLE, 15, 0, "");
            bottomtable.SetTopBorder(XWPFTable.XWPFBorderType.SINGLE, 15, 0, "");

            XWPFParagraph p124 = doc.CreateParagraph();   //向新文档中添加段落
            XWPFRun r124 = p124.CreateRun();                //向该段落中添加文字
            r124.SetText("(5~6由維護承商填寫，可用電腦繕打)");
            r124.FontFamily = "標楷體";
            r124.FontSize = 8;
            #endregion

            #region 下表格
            XWPFTable endtable = doc.CreateTable(1, 1);//创建1行1列表
            CT_Tbl m_CTTblend = doc.Document.body.GetTblArray()[2];//获得文档第一张表
            m_CTTblend.AddNewTblPr().AddNewTblW().w = "8200";//表宽度
            m_CTTblend.AddNewTblPr().AddNewTblW().type = ST_TblWidth.dxa;

            endtable.RemoveRow(0);//去掉第一行空白的
            CT_Row nrend = new CT_Row();
            XWPFTableRow mrend = new XWPFTableRow(nrend, endtable);
            mrend.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)350;//设置行高
            nrend.AddNewTrPr().AddNewTrHeight().val = (ulong)350;//设置行高（这两行都得有）
            endtable.AddRow(mrend);
            mrend.CreateCell().SetText("9.管制確認：□准予結案");
            mrend.GetCell(0).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

            CT_Row nrend2 = new CT_Row();
            XWPFTableRow mrend2 = new XWPFTableRow(nrend2, endtable);
            mrend2.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)350;//设置行高
            nrend2.AddNewTrPr().AddNewTrHeight().val = (ulong)350;//设置行高（这两行都得有）
            endtable.AddRow(mrend2);
            mrend2.CreateCell().SetText("管制單位核章確認：");
            mrend2.GetCell(0).SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);
            XWPFParagraph pIOe21 = endtable.GetRow(1).GetCell(0).AddParagraph();
            XWPFRun rIOe21 = pIOe21.CreateRun();
            rIOe21.SetText("");
            XWPFParagraph pIOe22 = endtable.GetRow(1).GetCell(0).AddParagraph();
            XWPFRun rIOe22 = pIOe22.CreateRun();
            rIOe22.SetText("");
            XWPFParagraph pIOe23 = endtable.GetRow(1).GetCell(0).AddParagraph();
            XWPFRun rIOe23 = pIOe23.CreateRun();
            rIOe23.SetText("");
            XWPFParagraph pIOe24 = endtable.GetRow(1).GetCell(0).AddParagraph();
            XWPFRun rIOe24 = pIOe24.CreateRun();
            rIOe24.SetText("");

            CT_Row cre1 = new CT_Row();
            XWPFTableRow tre1 = new XWPFTableRow(cre1, endtable);
            endtable.AddRow(tre1);
            tre1.CreateCell();

            CT_Row cre2 = new CT_Row();
            XWPFTableRow tre2 = new XWPFTableRow(cre2, endtable);
            endtable.AddRow(tre2);
            tre2.CreateCell();

            CT_TcPr m_Pre = endtable.GetRow(0).GetCell(0).GetCTTc().AddNewTcPr();
            m_Pre.tcW = new CT_TblWidth();
            m_Pre.tcW.w = "8200";//单元格宽
            m_Pre.tcW.type = ST_TblWidth.dxa;
            m_Pre = endtable.GetRow(1).GetCell(0).GetCTTc().AddNewTcPr();
            m_Pre.AddNewVMerge().val = ST_Merge.restart;
            m_Pre = endtable.GetRow(2).GetCell(0).GetCTTc().AddNewTcPr();
            m_Pre.AddNewVMerge().val = ST_Merge.@continue;
            m_Pre = endtable.GetRow(3).GetCell(0).GetCTTc().AddNewTcPr();
            m_Pre.AddNewVMerge().val = ST_Merge.@continue;

            endtable.SetLeftBorder(XWPFTable.XWPFBorderType.SINGLE, 15, 0, "");
            endtable.SetRightBorder(XWPFTable.XWPFBorderType.SINGLE, 15, 0, "");
            endtable.SetBottomBorder(XWPFTable.XWPFBorderType.SINGLE, 15, 0, "");
            endtable.SetTopBorder(XWPFTable.XWPFBorderType.SINGLE, 15, 0, "");

            XWPFParagraph p125 = doc.CreateParagraph();   //向新文档中添加段落
            XWPFRun r125 = p125.CreateRun();                //向该段落中添加文字
            r125.SetText("(9由管制單位－資訊室核章)");
            r125.FontFamily = "標楷體";
            r125.FontSize = 8;
            #endregion

            #region savefile

            FileStream sw = File.Create("../../../NPOITEST/cutput2.docx"); //...
            doc.Write(sw);                              //...
            sw.Close();                                 //在服务端生成文件

            FileInfo file = new FileInfo("../../../NPOITEST/cutput2.docx");//文件保存路径及名称  
                                                                           //注意: 文件保存的父文件夹需添加Everyone用户，并给予其完全控制权限
            Response.Clear();
            Response.ClearHeaders();
            Response.Buffer = false;
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode("問題(保修)申請單.docx", System.Text.Encoding.UTF8));
            Response.AppendHeader("Content-Length", file.Length.ToString());
            Response.WriteFile(file.FullName);
            Response.Flush();                           //以上将生成的word文件发送至用户浏览器

            File.Delete("../../../NPOITEST/cutput2.docx");
            //清除服务端生成的word文件

            #endregion
        }
        #endregion
    }
}