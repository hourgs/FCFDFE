using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Word = Microsoft.Office.Interop.Word;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E1E_Word : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
        #region 預覽word報表
            object oMissing = System.Reflection.Missing.Value;
            object oEndOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */
            MemoryStream ms = new MemoryStream();

            //Start Word and create a new document.
            Word._Application oWord;
            Word._Document oDoc;
            oWord = new Word.Application();
            //控制是否開啟word
            oWord.Visible = true;
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            //製作申請單編號與頁數
            Word.Table oTable;
            Word.Range wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
            oTable = oDoc.Tables.Add(wrdRng, 1, 11, ref oMissing, ref oMissing);
            oTable.Range.Font.Bold = 0;
            oTable.Range.Font.Name = "標楷體";
            oTable.Range.Font.Size = 12;
            oTable.Cell(1, 1).Range.Text = "類別：" + "歲出"; //類別之後改系統自帶
            oTable.Cell(1, 1).Merge(oTable.Cell(1, 3));
            oTable.Cell(1, 2).Range.Text = "代管保證文件收入通知單";
            oTable.Cell(1, 2).Merge(oTable.Cell(1, 9));
            oTable.Cell(1, 2).Range.Underline = Word.WdUnderline.wdUnderlineSingle;
            oTable.Cell(1, 2).Range.Bold = 1;

            //通知單正文
            Word.Paragraph oPara2;
            object oRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
            oRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
            oPara2 = oDoc.Content.Paragraphs.Add(ref oRng);
            oPara2.Range.Font.Size = 1;
            oPara2.Range.ParagraphFormat.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceExactly; //設定為固定行高
            oPara2.Range.ParagraphFormat.LineSpacing = 1; //設定行高為1

            wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
            oTable = oDoc.Tables.Add(wrdRng, 17, 11, ref oMissing, ref oMissing);
            oTable.Range.Font.Bold = 0;
            oTable.Range.Font.Name = "標楷體";
            oTable.Range.Font.Size = 12;
            oTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter; //置中對齊
            oTable.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphDistribute; //分散對齊

            //統一設定欄位寬度
            oTable.Columns[1].Width = oWord.CentimetersToPoints(1);
            oTable.Columns[2].Width = oWord.CentimetersToPoints(1);
            oTable.Columns[3].Width = oWord.CentimetersToPoints(2);
            oTable.Columns[4].Width = oWord.CentimetersToPoints(1);
            oTable.Columns[5].Width = oWord.CentimetersToPoints(2);
            oTable.Columns[6].Width = oWord.CentimetersToPoints(1);
            oTable.Columns[7].Width = oWord.CentimetersToPoints(2);
            oTable.Columns[8].Width = oWord.CentimetersToPoints(1);
            oTable.Columns[9].Width = oWord.CentimetersToPoints(2);
            oTable.Columns[10].Width = oWord.CentimetersToPoints(1);
            oTable.Columns[11].Width = oWord.CentimetersToPoints(1);

            oTable.Cell(1, 1).Range.Text = "甲、承辦單位填註事項";
            oTable.Cell(1, 1).Merge(oTable.Cell(13, 1));
            oTable.Cell(1, 2).Range.Text = "保證人\n（全名）";
            oTable.Cell(1, 2).Merge(oTable.Cell(1, 3));
            oTable.Cell(1, 3).Range.Text = "";  //保證人姓名
            oTable.Cell(1, 3).Merge(oTable.Cell(1, 4));
            oTable.Cell(1, 4).Range.Text = "被保證人\n統一編號";
            oTable.Cell(1, 4).Merge(oTable.Cell(1, 5));
            oTable.Cell(1, 5).Range.Text = "";  //被保證人+統一編號
            oTable.Cell(1, 5).Merge(oTable.Cell(1, 6));
            oTable.Cell(1, 6).Range.Text = "第\n一\n聯\n承\n辦\n單\n位\n業\n務\n部\n門\n存\n查";
            oTable.Cell(1, 6).Merge(oTable.Cell(11, 11));
            oTable.Cell(1, 6).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;

            oTable.Cell(2, 2).Range.Text = "保證人\n住址";
            oTable.Cell(2, 2).Merge(oTable.Cell(2, 3));
            oTable.Cell(2, 3).Range.Text = "";  //保證人住址
            oTable.Cell(2, 3).Merge(oTable.Cell(2, 4));
            oTable.Cell(2, 4).Range.Text = "被保證人\n住址";
            oTable.Cell(2, 4).Merge(oTable.Cell(2, 5));
            oTable.Cell(2, 5).Range.Text = "";  //被保證人住址
            oTable.Cell(2, 5).Merge(oTable.Cell(2, 6));

            oTable.Cell(3, 2).Range.Text = "保證事由說明保證文件";
            oTable.Cell(3, 2).Merge(oTable.Cell(3, 3));
            oTable.Cell(3, 3).Range.Text = "名稱\n保證文件";
            oTable.Cell(3, 3).Merge(oTable.Cell(3, 4));
            oTable.Cell(3, 4).Range.Text = "份數";
            oTable.Cell(3, 4).Merge(oTable.Cell(3, 5));
            oTable.Cell(3, 5).Range.Text = "保證文件保證金額";
            oTable.Cell(3, 5).Merge(oTable.Cell(3, 6));

            oTable.Cell(4, 2).Range.Text = "";  //保證事由說明保證文件_1
            oTable.Cell(4, 2).Merge(oTable.Cell(4, 3));
            oTable.Cell(4, 3).Range.Text = "";  //名稱保證文件_1
            oTable.Cell(4, 3).Merge(oTable.Cell(4, 4));
            oTable.Cell(4, 4).Range.Text = "";  //份數_1
            oTable.Cell(4, 4).Merge(oTable.Cell(4, 5));
            oTable.Cell(4, 5).Range.Text = "";  //保證文件保證金額_1
            oTable.Cell(4, 5).Merge(oTable.Cell(4, 6));

            oTable.Cell(5, 2).Range.Text = "";  //保證事由說明保證文件_2
            oTable.Cell(5, 2).Merge(oTable.Cell(5, 3));
            oTable.Cell(5, 3).Range.Text = "";  //名稱保證文件_2
            oTable.Cell(5, 3).Merge(oTable.Cell(5, 4));
            oTable.Cell(5, 4).Range.Text = "";  //份數_2
            oTable.Cell(5, 4).Merge(oTable.Cell(5, 5));
            oTable.Cell(5, 5).Range.Text = "";  //保證文件保證金額_2
            oTable.Cell(5, 5).Merge(oTable.Cell(5, 6));

            oTable.Cell(6, 2).Range.Text = "";  //保證事由說明保證文件_3
            oTable.Cell(6, 2).Merge(oTable.Cell(6, 3));
            oTable.Cell(6, 3).Range.Text = "";  //名稱保證文件_3
            oTable.Cell(6, 3).Merge(oTable.Cell(6, 4));
            oTable.Cell(6, 4).Range.Text = "";  //份數_3   
            oTable.Cell(6, 4).Merge(oTable.Cell(6, 5));
            oTable.Cell(6, 5).Range.Text = "";  //保證文件保證金額_3
            oTable.Cell(6, 5).Merge(oTable.Cell(6, 6));

            oTable.Cell(7, 2).Range.Text = "合計保證文件（保證金額為新台幣（大寫））伍拾陸萬柒仟柒佰捌拾捌元整";
            oTable.Cell(7, 2).Merge(oTable.Cell(7, 9));
            oTable.Cell(7, 2).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;

            oTable.Cell(8, 2).Range.Text = "附記";
            oTable.Cell(8, 3).Range.Text = "一、保證事項無法計價者每項以壹元為保證文件保證金額（象徵）。"
                                         + "\n二、保證期間自 XX 年 XX 月 XX 日至合約履行完成止。";
            oTable.Cell(8, 3).Merge(oTable.Cell(8, 9));
            oTable.Cell(8, 3).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;

            oTable.Cell(9, 2).Range.Text = "承辦單位";
            oTable.Cell(9, 2).Merge(oTable.Cell(13, 2));
            oTable.Cell(9, 3).Range.Text = "承辦單位主管";
            oTable.Cell(9, 3).Merge(oTable.Cell(11, 3));
            oTable.Cell(9, 4).Range.Text = "承辦單位業務部門主管";
            oTable.Cell(9, 4).Merge(oTable.Cell(11, 4));
            oTable.Cell(9, 5).Range.Text = "承辦單位業務部門承辦人";
            oTable.Cell(9, 5).Merge(oTable.Cell(11, 5));
            oTable.Cell(9, 6).Range.Text = "承辦單位";
            oTable.Cell(9, 6).Merge(oTable.Cell(10, 6));
            oTable.Cell(9, 7).Range.Text = "統一編號";
            oTable.Cell(9, 8).Range.Text = "";  //統一編號
            oTable.Cell(9, 8).Merge(oTable.Cell(9, 9));

            oTable.Cell(10, 7).Range.Text = "全銜";
            oTable.Cell(10, 8).Range.Text = ""; //全銜
            oTable.Cell(10, 8).Merge(oTable.Cell(10, 9));

            oTable.Cell(11, 6).Range.Text = "業務部門名稱";
            oTable.Cell(11, 6).Merge(oTable.Cell(11, 7));
            oTable.Cell(11, 7).Range.Text = ""; //業務部門名稱
            oTable.Cell(11, 7).Merge(oTable.Cell(11, 8));

            oTable.Cell(12, 3).Range.Text = ""; //承辦單位主管
            oTable.Cell(12, 3).Merge(oTable.Cell(13, 3));
            oTable.Cell(12, 4).Range.Text = ""; //承辦單位業務部門主管
            oTable.Cell(12, 4).Merge(oTable.Cell(13, 4));
            oTable.Cell(12, 5).Range.Text = ""; //承辦單位業務部門承辦人
            oTable.Cell(12, 5).Merge(oTable.Cell(13, 5));
            oTable.Cell(12, 6).Range.Text = "收入通知單編號";
            oTable.Cell(12, 6).Width = oWord.CentimetersToPoints(2);
            oTable.Cell(12, 7).Range.Text = ""; //收入通知單編號
            oTable.Cell(12, 7).Merge(oTable.Cell(12, 9));
            oTable.Cell(12, 7).Width = oWord.CentimetersToPoints(4);
            oTable.Cell(12, 8).Range.Text = "軍\n官\n簽\n證\n：";
            oTable.Cell(12, 8).Merge(oTable.Cell(17, 10));
            oTable.Cell(12, 8).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
            oTable.Cell(12, 9).Range.Text = "主\n辦\n主\n計\n：";
            oTable.Cell(12, 9).Merge(oTable.Cell(17, 11));
            oTable.Cell(12, 9).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;

            oTable.Cell(13, 6).Range.Text = "日期";
            oTable.Cell(13, 6).Width = oWord.CentimetersToPoints(2);
            oTable.Cell(13, 7).Range.Text = ""; //日期
            oTable.Cell(13, 7).Merge(oTable.Cell(13, 9));
            oTable.Cell(13, 7).Width = oWord.CentimetersToPoints(4);

            oTable.Cell(14, 1).Range.Text = "乙、保證文件保管部門填註事項";
            oTable.Cell(14, 1).Merge(oTable.Cell(17, 1));
            oTable.Cell(14, 2).Range.Text = "附記";
            oTable.Cell(14, 3).Range.Text = ""; //附記
            oTable.Cell(14, 3).Merge(oTable.Cell(14, 9));

            oTable.Cell(15, 2).Range.Text = "繳存國庫日期：　年　月　日";
            oTable.Cell(15, 2).Merge(oTable.Cell(15, 5));
            oTable.Cell(15, 3).Range.Text = "國庫存證字號：";
            oTable.Cell(15, 3).Merge(oTable.Cell(15, 6));
            oTable.Cell(15, 3).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;

            oTable.Cell(16, 2).Range.Text = "保證文件保管單位";
            oTable.Cell(16, 2).Merge(oTable.Cell(17, 2));
            oTable.Cell(16, 3).Range.Text = "保證文件保管單位主管";
            oTable.Cell(16, 4).Range.Text = "保證文件保管單位之保證文件保管部門";
            oTable.Cell(16, 4).Width = oWord.CentimetersToPoints(3);
            oTable.Cell(16, 5).Range.Text = "保證文件保管部門保管人點收簽名及點收日期時分\n（第一聯免）";
            oTable.Cell(16, 5).Merge(oTable.Cell(16, 6));
            oTable.Cell(16, 5).Width = oWord.CentimetersToPoints(3);
            oTable.Cell(16, 6).Range.Text = "交件人密封\n簽章樣式";
            oTable.Cell(16, 6).Merge(oTable.Cell(16, 8));
            oTable.Cell(16, 6).Width = oWord.CentimetersToPoints(3);

            oTable.Cell(17, 3).Range.Text = "\n\n\n\n"; //保證文件保管單位主管           
            oTable.Cell(17, 4).Range.Text = ""; //保證文件保管單位之保證文件保管部門   
            oTable.Cell(17, 4).Width = oWord.CentimetersToPoints(3);
            oTable.Cell(17, 5).Range.Text = ""; //保證文件保管部門保管人點收簽名及點收日期時分（第一聯免）
            oTable.Cell(17, 5).Merge(oTable.Cell(17, 6));
            oTable.Cell(17, 5).Width = oWord.CentimetersToPoints(3);
            oTable.Cell(17, 6).Range.Text = ""; //交件人密封簽章樣式
            oTable.Cell(17, 6).Merge(oTable.Cell(17, 8));
            oTable.Cell(17, 6).Width = oWord.CentimetersToPoints(3);

            oTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            oTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            oTable.Borders.OutsideLineWidth = Word.WdLineWidth.wdLineWidth225pt; //設定外框粗體  
#endregion
        }
    }
}