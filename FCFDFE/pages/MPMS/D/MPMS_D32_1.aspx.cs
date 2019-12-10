using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using System.Web.UI;
using System.IO;
//reference add Microsoft Office 16.0 Object Library 在 com 裡面可以找到
//reference add Microsoft Word 16.0 Object Library 在 com 裡面可以找到
using Word = Microsoft.Office.Interop.Word;
using FCFDFE.Entity.GMModel;
using NPOI.XWPF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;
using System.Web;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D32_1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            createwordfile();
        }
        private void createwordfile()
        {
            
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
            p1.Alignment = ParagraphAlignment.RIGHT; //段落对其方式为居中
            XWPFRun r1 = p1.CreateRun();                //向该段落中添加文字
            r1.SetText("會辦字號：");
            r1.FontFamily = "標楷體";
            r1.FontSize = 12;

            XWPFParagraph p2 = doc.CreateParagraph();   //向新文档中添加段落
            p2.Alignment = ParagraphAlignment.CENTER; //段落对其方式为居中
            XWPFRun r2 = p2.CreateRun();                //向该段落中添加文字
            r2.SetText("國防採購室  會辦單");
            r2.FontFamily = "標楷體";
            r1.IsBold = true;
            r2.FontSize = 16;

            XWPFParagraph p3 = doc.CreateParagraph();   //向新文档中添加段落
            p3.Alignment = ParagraphAlignment.RIGHT; //段落对其方式为居中
            XWPFRun r3 = p3.CreateRun();                //向该段落中添加文字
            r3.SetText("密等及解密條件：");
            r3.FontFamily = "標楷體";
            r3.FontSize = 14;

            XWPFParagraph p4 = doc.CreateParagraph();   //向新文档中添加段落
            p4.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r4 = p4.CreateRun();                //向该段落中添加文字
            r4.SetText("來文字號：");
            r4.FontFamily = "標楷體";
            r4.FontSize = 14;

            XWPFParagraph p5 = doc.CreateParagraph();   //向新文档中添加段落
            p5.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r5 = p5.CreateRun();                //向该段落中添加文字
            r5.SetText("案情摘要：");
            r5.FontFamily = "標楷體";
    
            r5.FontSize = 14;

            XWPFParagraph p6 = doc.CreateParagraph();   //向新文档中添加段落
            p6.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r6 = p6.CreateRun();                //向该段落中添加文字
            r6.SetText("附件：契約及附件分配表");
            r6.FontFamily = "標楷體";

            r6.FontSize = 14;

            XWPFParagraph p7 = doc.CreateParagraph();   //向新文档中添加段落
            p7.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r7 = p7.CreateRun();                //向该段落中添加文字
            r7.SetText("第一受會單位：");
            r7.FontFamily = "標楷體";
     
            r7.FontSize = 14;

            XWPFParagraph p8 = doc.CreateParagraph();   //向新文档中添加段落
            p8.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r8 = p8.CreateRun();                //向该段落中添加文字
            r8.SetText("送會目的：");
            r8.FontFamily = "標楷體";
    
            r8.FontSize = 14;

            XWPFParagraph p9 = doc.CreateParagraph();   //向新文档中添加段落
            p9.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r9 = p9.CreateRun();                //向该段落中添加文字
            r9.SetText("收會時間：              退轉時間：               承辦人簽章：");
            r9.FontFamily = "標楷體";
      
            r9.FontSize = 12;

            XWPFParagraph p10 = doc.CreateParagraph();   //向新文档中添加段落
            p10.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r10 = p10.CreateRun();                //向该段落中添加文字
            r10.SetText("");
            r10.FontFamily = "標楷體";
       
            r10.FontSize = 12;

            XWPFParagraph p11 = doc.CreateParagraph();   //向新文档中添加段落
            p11.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r11 = p11.CreateRun();                //向该段落中添加文字
            r11.SetText("");
            r11.FontFamily = "標楷體";
     
            r11.FontSize = 12;

            XWPFParagraph p12 = doc.CreateParagraph();   //向新文档中添加段落
            p12.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r12 = p12.CreateRun();                //向该段落中添加文字
            r12.SetText("會畢時間：");
            r12.FontFamily = "標楷體";
   
            r12.FontSize = 12;

            XWPFParagraph p13 = doc.CreateParagraph();   //向新文档中添加段落
            p13.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r13 = p13.CreateRun();                //向该段落中添加文字
            r13.SetText("正本：");
            r13.FontFamily = "標楷體";
 
            r13.FontSize = 12;

            XWPFParagraph p14 = doc.CreateParagraph();   //向新文档中添加段落
            p14.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r14 = p14.CreateRun();                //向该段落中添加文字
            r14.SetText("主辦單位電話：");
            r14.FontFamily = "標楷體";
           r14.FontSize = 12;

            XWPFParagraph p141 = doc.CreateParagraph();   //向新文档中添加段落
            p141.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r141 = p141.CreateRun();                //向该段落中添加文字
            r141.SetText("主辦單位簽章：");
            r141.FontFamily = "標楷體";
        
            r141.FontSize = 12;

            XWPFParagraph p142 = doc.CreateParagraph();   //向新文档中添加段落
            p142.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r142 = p142.CreateRun();                //向该段落中添加文字
            r142.SetText("");
            r142.FontFamily = "標楷體";

            r142.FontSize = 12;

            XWPFParagraph p15 = doc.CreateParagraph();   //向新文档中添加段落
            p15.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r15 = p15.CreateRun();                //向该段落中添加文字
            r15.SetText("");
            r15.FontFamily = "標楷體";
         
            r15.FontSize = 12;


            XWPFParagraph p1412 = doc.CreateParagraph();   //向新文档中添加段落
            p1412.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r1412 = p1412.CreateRun();                //向该段落中添加文字
            r1412.SetText("會辦意見");
            r1412.FontFamily = "標楷體";
         
            r1412.FontSize = 16;

            XWPFParagraph p1422 = doc.CreateParagraph();   //向新文档中添加段落
            p1422.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r1422 = p1422.CreateRun();                //向该段落中添加文字
            r1422.SetText("單位：");
            r1422.FontFamily = "標楷體";
        
            r1422.FontSize = 14;

            XWPFParagraph p152 = doc.CreateParagraph();   //向新文档中添加段落
            p152.Alignment = ParagraphAlignment.LEFT; //段落对其方式为居中
            XWPFRun r152 = p152.CreateRun();                //向该段落中添加文字
            r152.SetText("意見：");
            r152.FontFamily = "標楷體";
     
            r152.FontSize = 14;
            #endregion 標題

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
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode("契約分送會辦單.docx", System.Text.Encoding.UTF8));
            Response.AppendHeader("Content-Length", file.Length.ToString());
            Response.WriteFile(file.FullName);
            Response.Flush();                           //以上将生成的word文件发送至用户浏览器

            File.Delete("../../../NPOITEST/cutput2.docx");
            //清除服务端生成的word文件

            #endregion
        }
    }
}