using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;
using System.Globalization;
using TemplateEngine.Docx;
using System.IO;
using FCFDFE.Content;

namespace FCFDFE.pages.MPMS.D
{
    /// <summary>
    /// MPMS_D18_3_Word 的摘要描述
    /// </summary>
    public class MPMS_D18_3_Word : IHttpHandler
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["CommandName"] != null && context.Request.QueryString["OVC_PURCH"] != null && context.Request.QueryString["OVC_DOPEN"] != null && context.Request.QueryString["OVC_VEN_TITLE"] != null)
            {
                string strCommandName = context.Request.QueryString["CommandName"];
                string strOVC_PURCH = context.Request.QueryString["OVC_PURCH"];
                TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                string strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                string strOVC_DOPEN = context.Request.QueryString["OVC_DOPEN"];
                string strOVC_VEN_TITLE = context.Request.QueryString["OVC_VEN_TITLE"];

                var targetPath = context.Server.MapPath("~/WordPDFprint/");
                string fileName = "";


                if (strCommandName == "WordD18_3_1" || strCommandName == "WordD18_3_1_odt")
                {
                    //公開招標一次投、不分段開標審查表.docx
                    fileName = strOVC_PURCH + "-" + strOVC_VEN_TITLE + "-公開招標一次投、不分段開標審查表.docx";
                    File.Delete(targetPath + fileName);
                    File.Copy(targetPath + "D18_3_1-公開招標一次投、不分段開標審查表.docx", targetPath + fileName);
                }

                else if (strCommandName == "WordD18_3_2" || strCommandName == "WordD18_3_2_odt")
                {
                    //公開一次投分段開標審查表 
                    fileName = strOVC_PURCH + "-" + strOVC_VEN_TITLE + "-公開一次投分段開標審查表.docx";
                    File.Delete(targetPath + fileName);
                    File.Copy(targetPath + "D18_3_2-公開一次投分段開標審查表.docx", targetPath + fileName);
                }

                else if (strCommandName == "WordD18_3_3" || strCommandName == "WordD18_3_3_odt")
                {
                    //公開分段投標審查
                    fileName = strOVC_PURCH + "-" + strOVC_VEN_TITLE + "-公開分段投標審查表.docx";
                    File.Delete(targetPath + fileName);
                    File.Copy(targetPath + "D18_3_3-公開分段投標審查表.docx", targetPath + fileName);
                }

                else if (strCommandName == "WordD18_3_4" || strCommandName == "WordD18_3_4_odt")
                {
                    //選擇性招標投標審查表
                    fileName = strOVC_PURCH + "-" + strOVC_VEN_TITLE + "-選擇性招標投標審查表.docx";
                    File.Delete(targetPath + fileName);
                    File.Copy(targetPath + "D18_3_4-選擇性招標投標審查表.docx", targetPath + fileName);
                }

                var valuesToFill = new TemplateEngine.Docx.Content();

                TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                if (tbm1301 != null)
                {
                    valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5));
                    valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IPURCH", tbm1301.OVC_PUR_IPURCH == null ? "" : tbm1301.OVC_PUR_IPURCH));
                    valuesToFill.Fields.Add(new FieldContent("OVC_DOPEN", strOVC_DOPEN == "" ? "" : GetTaiwanDate(strOVC_DOPEN)));
                    valuesToFill.Fields.Add(new FieldContent("OVC_VEN_TITLE", strOVC_VEN_TITLE));
                }

                using (var outputDocument = new TemplateProcessor(targetPath + fileName).SetRemoveContentControls(true))
                {
                    outputDocument.FillContent(valuesToFill);
                    outputDocument.SaveChanges();
                }

                if (strCommandName.Contains("odt"))
                {
                    string filetemp = context.Request.PhysicalApplicationPath + "WordPDFprint/D18_3_odt.odt";
                    string filename_fin = fileName.Replace(".docx", ".odt");
                    FCommon.WordToOdt(context, targetPath + fileName, filetemp, filename_fin);
                }
                else
                {
                    FileInfo fileInfo = new FileInfo(targetPath + fileName);
                    context.Response.Clear();
                    context.Response.ClearContent();
                    context.Response.ClearHeaders();
                    context.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                    context.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                    context.Response.AddHeader("Content-Transfer-Encoding", "binary");
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                    context.Response.WriteFile(fileInfo.FullName);
                    context.Response.Flush();
                    System.IO.File.Delete(targetPath + fileName);
                    context.Response.End();
                }
            }
        }

        private string GetTaiwanDate(string strDate)
        {
            //西元年轉民國年
            if (strDate != null && strDate != "")
            {
                DateTime datetime = Convert.ToDateTime(strDate);
                CultureInfo info = new CultureInfo("zh-TW");
                TaiwanCalendar twC = new TaiwanCalendar();
                info.DateTimeFormat.Calendar = twC;
                return datetime.ToString("yyy年MM月dd日", info);
            }
            return strDate;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}