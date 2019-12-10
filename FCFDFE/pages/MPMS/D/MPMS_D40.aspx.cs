using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;
using NPOI.XWPF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;
using System.Web;
using System.IO;
using Xceed.Words.NET;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D40 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_PURCH_5"] == null || Request.QueryString["ONB_GROUP"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    if (!IsPostBack && IsOVC_DO_NAME())
                    {
                        string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
                        string strOVC_PURCH_5_url = Request.QueryString["OVC_PURCH_5"];
                        string strONB_GROUP_url = Request.QueryString["ONB_GROUP"];
                        string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
                        string strOVC_PURCH_5 = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_5_url));
                        string strONB_GROUP = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strONB_GROUP_url));
                        ViewState["strOVC_NOTICE_TITLE"] = "none";
                        ViewState["strOVC_PURCH"] = strOVC_PURCH;
                        ViewState["strOVC_PURCH_5"] = strOVC_PURCH_5;
                        ViewState["strONB_GROUP"] = strONB_GROUP;
                        short onbgroup = Convert.ToInt16(strONB_GROUP);
                        var query1302 =
                        (from tbm1302 in mpms.TBM1302
                         where tbm1302.OVC_PURCH == strOVC_PURCH
                         where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
                         where tbm1302.ONB_GROUP == onbgroup
                         select tbm1302).FirstOrDefault();
                        string strOVC_PURCH_6 = query1302 == null ? "" : query1302.OVC_PURCH_6;
                        string strOVC_VEN_TITLE = query1302 == null ? "" : query1302.OVC_VEN_TITLE;
                        ViewState["strOVC_VEN_TITLE"] = strOVC_VEN_TITLE;
                        ViewState["strOVC_PURCH_6"] = strOVC_PURCH_6;
                        var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
                        lblOVC_PURCH.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5 + strOVC_PURCH_6;
                        dataGV(strOVC_PURCH, strOVC_PURCH_5, strOVC_PURCH_6);
                        var query1407S3 =
                            (from tbm1407 in mpms.TBM1407
                             where tbm1407.OVC_PHR_CATE == "S3"
                             select tbm1407).ToList();
                        DataTable dtS3 = CommonStatic.ListToDataTable(query1407S3);
                        FCommon.list_dataImport(drpOVC_NOTICE_TITLE, dtS3, "OVC_PHR_DESC", "OVC_PHR_ID", true);
                        drpOVC_NOTICE_TITLE.Items.Add(strOVC_VEN_TITLE);
                        drpOVC_NOTICE_TITLE.Items.Add("國防部政治作戰局");
                        var query1407S4 =
                            (from tbm1407 in mpms.TBM1407
                             where tbm1407.OVC_PHR_CATE == "S4"
                             select tbm1407).ToList();
                        DataTable dtB0 = CommonStatic.ListToDataTable(query1407S4);
                        FCommon.list_dataImport(drpOVC_ATTACH_NAME, dtB0, "OVC_PHR_DESC", "OVC_PHR_ID", true);
                    }
                }
            }
        }

        private bool IsOVC_DO_NAME()
        {
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strUserName, strDept = "";
            string strOVC_PURCH_url = Request.QueryString["OVC_PURCH"];
            string strOVC_PURCH = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strOVC_PURCH_url));
            DataTable dt = new DataTable();
            if (strUSER_ID.Length > 0)
            {
                ACCOUNT ac = new ACCOUNT();
                ac = gm.ACCOUNTs.Where(tb => tb.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                if (ac != null)
                {
                    strUserName = ac.USER_NAME.ToString();
                    strDept = ac.DEPT_SN;
                    TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    if (strOVC_PURCH == "")
                        strErrorMsg = "請選擇購案";
                    else if (tbm1301 == null)
                        strErrorMsg = "查無此購案編號";
                    else
                    {
                        TBM1301_PLAN plan1301 =
                            gm.TBM1301_PLAN.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCHASE_UNIT.Equals(strDept)).FirstOrDefault();

                        TBMRECEIVE_BID tbmRECEIVE_BID =
                            mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_DO_NAME.Equals(strUserName)).FirstOrDefault();
                        if (tbmRECEIVE_BID == null || plan1301 == null)
                            strErrorMsg = "非此購案的承辦人";
                    }

                    if (strErrorMsg != "")
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErrorMsg);
                    else
                    {
                        divForm.Visible = true;
                        return true;
                    }
                }
            }
            divForm.Visible = false;
            return false;
        }

        private void dataGV(string strOVC_PURCH,string strOVC_PURCH_5,string strOVC_PURCH_6)
        {
            DataTable dt = new DataTable();
            var query =
                from tbmnotice in mpms.TBMCONTRACT_NOTICE
                where tbmnotice.OVC_PURCH == strOVC_PURCH
                where tbmnotice.OVC_PURCH_5 == strOVC_PURCH_5
                where tbmnotice.OVC_PURCH_6 == strOVC_PURCH_6
                select new
                {
                    OVC_PURCH = tbmnotice.OVC_PURCH,
                    OVC_PURCH_5 = tbmnotice.OVC_PURCH_5,
                    OVC_PURCH_6 = tbmnotice.OVC_PURCH_6,
                    OVC_NOTICE_TITLE = tbmnotice.OVC_NOTICE_TITLE,
                    OVC_ATTACH_NAME = tbmnotice.OVC_ATTACH_NAME,
                    OVC_MEMO = tbmnotice.OVC_MEMO
                };
            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["Status"] = FCommon.GridView_dataImport(GV_info, dt);
        }
        protected void GV_info_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_info.DataKeys[gvrIndex].Value.ToString(); //OVC_PURCH
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strOVC_PURCH_6 = ViewState["strOVC_PURCH_6"].ToString();
            switch (e.CommandName)
            {
                case "btnMove":
                    txtOVC_NOTICE_TITLE.Text = id;
                    var query =
                        (from tbm in mpms.TBMCONTRACT_NOTICE
                         where tbm.OVC_PURCH == strOVC_PURCH
                         where tbm.OVC_PURCH_5 == strOVC_PURCH_5
                         where tbm.OVC_PURCH_6 == strOVC_PURCH_6
                         where tbm.OVC_NOTICE_TITLE == id
                         select tbm).ToList();
                    ViewState["strOVC_NOTICE_TITLE"] = id;
                    //string strOVC_ATTACH_NAME = query.OVC_ATTACH_NAME;
                    FCommon.list_dataImport(lst, query, "OVC_ATTACH_NAME", "OVC_PURCH", false);
                    break;
                case "btnDel":
                    break;
                default:
                    break;
            }
        }
        public void list_dataImportL(ListControl list, object query, string textField, string valueField, string strOVC_ATTACH_NAME)
        {
            //先將下拉式選單清空
            list.Items.Clear();

            list.AppendDataBoundItems = true;
  
            list.DataSource = query;
            list.Items.Add(new ListItem(strOVC_ATTACH_NAME, ""));
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataBind();
        }
        protected void drpOVC_NOTICE_TITLE_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOVC_NOTICE_TITLE.Text = drpOVC_NOTICE_TITLE.SelectedItem.Text;
            txtNum.Text = "";
            txtOVC_MEMO.Text = "";
            lst.Items.Clear();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strOVC_PURCH_6 = ViewState["strOVC_PURCH_6"].ToString();
            string strOVC_NOTICE_TITLE = txtOVC_NOTICE_TITLE.Text;
            string strITEM = "";
            string strOVC_VEN_TITLE = ViewState["strOVC_VEN_TITLE"].ToString();
            string strOVC_NOTICE_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt");
            var query =
                        (from tbm in mpms.TBMCONTRACT_NOTICE
                         where tbm.OVC_PURCH == strOVC_PURCH
                         where tbm.OVC_PURCH_5 == strOVC_PURCH_5
                         where tbm.OVC_PURCH_6 == strOVC_PURCH_6
                         where tbm.OVC_NOTICE_TITLE == strOVC_NOTICE_TITLE
                         select tbm).FirstOrDefault();
            if (query != null)
            {
                int num = lst.Items.Count-1;
                if(num < 1)
                {
                    strITEM = lst.Items[0].ToString();
                }
                else
                {
                    for (int i = 0; i < lst.Items.Count-1; i++)
                    {
                        strITEM += lst.Items[i].ToString()+"、";
                    }
                    strITEM += lst.Items[num].ToString();
                }
                query.OVC_ATTACH_NAME = strITEM;
                query.OVC_NOTICE_TIME = strOVC_NOTICE_TIME;
                string strTitle = query.OVC_NOTICE_TITLE;
                if (strTitle == strOVC_VEN_TITLE)
                {
                    query.OVC_NOTICE_KIND = "Y";
                }
                else
                    query.OVC_NOTICE_KIND = "N";
                query.OVC_MEMO = txtOVC_MEMO.Text;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), query.GetType().Name.ToString(), this, "修改");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
            }
            else
            {
                TBMCONTRACT_NOTICE notice = new TBMCONTRACT_NOTICE();
                notice.OVC_PURCH = strOVC_PURCH;
                notice.OVC_PURCH_5 = strOVC_PURCH_5;
                notice.OVC_PURCH_6 = strOVC_PURCH_6;
                notice.OVC_NOTICE_TITLE = strOVC_NOTICE_TITLE;
                notice.OVC_NOTICE_TIME = strOVC_NOTICE_TIME;
                int num = lst.Items.Count;
                if (num < 1)
                {
                    strITEM = lst.Items[0].ToString();
                }
                else
                {
                    for (int i = 0; i < lst.Items.Count-1; i++)
                    {
                        strITEM += lst.Items[i].ToString() + "、";
                    }
                    strITEM += lst.Items[num-1].ToString();
                }
                notice.OVC_ATTACH_NAME = strITEM;
                if (strOVC_NOTICE_TITLE == strOVC_VEN_TITLE)
                {
                    notice.OVC_NOTICE_KIND = "Y";
                }
                else
                    notice.OVC_NOTICE_KIND = "N";
                notice.OVC_MEMO = txtOVC_MEMO.Text;
                mpms.TBMCONTRACT_NOTICE.Add(notice);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), notice.GetType().Name.ToString(), this, "新增");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增成功");
            }
            dataGV(strOVC_PURCH, strOVC_PURCH_5, strOVC_PURCH_6);
            txtOVC_NOTICE_TITLE.Text = "";
            txtNum.Text = "";
            txtOVC_MEMO.Text = "";
            lst.Items.Clear();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string strOVC_ATTACH_NAME = drpOVC_ATTACH_NAME.SelectedItem.Text + txtNum.Text + "份";

            //string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            //string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            //string strOVC_PURCH_6 = ViewState["strOVC_PURCH_6"].ToString();
            //string id = ViewState["strOVC_NOTICE_TITLE"].ToString();
            //if (id != "none")
            //{
            //    var query =
            //            (from tbm in mpms.TBMCONTRACT_NOTICE
            //             where tbm.OVC_PURCH == strOVC_PURCH
            //             where tbm.OVC_PURCH_5 == strOVC_PURCH_5
            //             where tbm.OVC_PURCH_6 == strOVC_PURCH_6
            //             where tbm.OVC_NOTICE_TITLE == id
            //             select tbm).ToList();

            //    //string strOVC_ATTACH_NAME = query.OVC_ATTACH_NAME;
            //    list_dataImportL(lst, query, "OVC_ATTACH_NAME", "OVC_PURCH", strOVC_ATTACH_NAME);
            //}
            lst.Items.Add(strOVC_ATTACH_NAME);
        }

        protected void GV_info_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
            {
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            }
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void btnReturnS_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_D36.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString());
        }

        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_D14_1.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString());
        }


        protected void LinkButton1_Click(object sender, EventArgs e)
        {

        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {

        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            createwordfile();
        }

        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            Contract_Distribution_Notice();
        }



        private void createwordfile()
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strOVC_PURCH_6 = ViewState["strOVC_PURCH_6"].ToString();
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
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
            r5.SetText("案情摘要：檢送「"+query1301.OVC_PUR_IPURCH+"("+strOVC_PURCH+query1301.OVC_PUR_AGENCY+strOVC_PURCH_5+strOVC_PURCH_6+ ")」案契約正、副本。");
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

        

        void Contract_Distribution_Notice()
        {
            int y = 0;
            string date = "";
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Contract_Distribution_Notice_D40.docx");
            byte[] buffer = null;
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var query =
                        from tbm1301 in mpms.TBM1301
                        join tbm1302 in mpms.TBM1302
                            on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                        where tbm1301.OVC_PURCH.Equals(purch)
                        select new
                        {
                            OVC_PURCH = tbm1301.OVC_PURCH ?? "",
                            OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY ?? "",
                            OVC_PURCH_5 = tbm1302.OVC_PURCH_5 ?? "",
                            OVC_PURCH_6 = tbm1302.OVC_PURCH_6 ?? "",
                            OVC_DSEND = tbm1302.OVC_DSEND,
                            OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH ?? "",
                            OVC_NAME = tbm1302.OVC_NAME ?? "",
                        };
                    foreach (var q in query)
                    {
                        if (q.OVC_DSEND != null)
                        {
                            y = int.Parse(q.OVC_DSEND.ToString().Substring(0, 4)) - 1911;
                            date = y.ToString() + "年" + q.OVC_DSEND.Substring(5, 2) + "月" + q.OVC_DSEND.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DSEND$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$OVC_DSEND$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        string year = q.OVC_PURCH.Substring(2, 2);
                        y = int.Parse(DateTime.Now.Year.ToString()) - 1911;
                        if (year.CompareTo(y.ToString()) <= 0)
                            doc.ReplaceText("[$YEAR$]", "1" + year, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$YEAR$]", year, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$PURCH_56$]", q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_IPURCH$]", q.OVC_PUR_IPURCH, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$PURCH$]", q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_NAME$]", q.OVC_NAME, false, System.Text.RegularExpressions.RegexOptions.None);
                        var queryAcc = mpms.ACCOUNT.Where(table => table.USER_NAME.Equals(q.OVC_NAME)).FirstOrDefault();
                        doc.ReplaceText("[$IUSER_PHONE$]", queryAcc.IUSER_PHONE ?? "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.SaveAs(Request.PhysicalApplicationPath + "Tempprint/b.docx");
                }
                buffer = ms.ToArray();
            }
            string path_d = Path.Combine(Request.PhysicalApplicationPath, "Tempprint/b.docx");
            FileInfo file = new FileInfo(path_d);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + DateTime.Now + ".docx");
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            Response.End();
        }
    }
}