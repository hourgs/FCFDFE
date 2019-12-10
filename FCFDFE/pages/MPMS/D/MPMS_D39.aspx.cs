using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;
using System.IO;
using Xceed.Words.NET;
using Microsoft.Office.Interop.Word;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D39 : System.Web.UI.Page
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
                        short onbgroup = Convert.ToInt16(strONB_GROUP);
                        dataGV(strOVC_PURCH, strOVC_PURCH_5, onbgroup);
                        ViewState["strOVC_PURCH"] = strOVC_PURCH;
                        ViewState["strOVC_PURCH_5"] = strOVC_PURCH_5;
                        ViewState["strONB_GROUP"] = strONB_GROUP;

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
            System.Data.DataTable dt = new System.Data.DataTable();
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

        private void dataGV(string strOVC_PURCH,string strOVC_PURCH_5,int onbgroup)
        {
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            var query1302 =
                (from tbm1302 in mpms.TBM1302
                 where tbm1302.OVC_PURCH == strOVC_PURCH
                 where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbm1302.ONB_GROUP == onbgroup
                 select tbm1302).FirstOrDefault();
            string strOVC_PURCH_6 = query1302 == null ? "" : query1302.OVC_PURCH_6;
            lblOVC_PURCH.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5+ strOVC_PURCH_6;
            lblOVC_VEN_TITLE.Text = query1302 == null ? "" : query1302.OVC_VEN_TITLE;
            lblONB_GROUP.Text = onbgroup.ToString();
            txtResult.Text = query1302 == null ? "" : query1302.ONB_MONEY.ToString();
            txtONB_MONEY_DISCOUNT.Text = query1302 == null ? "" : query1302.ONB_MONEY_DISCOUNT.ToString();
            System.Data.DataTable dt = new System.Data.DataTable();
            var query1302item =
                from t1302 in mpms.TBM1302_ITEM
                where t1302.OVC_PURCH == strOVC_PURCH
                where t1302.OVC_PURCH_5 == strOVC_PURCH_5
                where t1302.OVC_PURCH_6 == strOVC_PURCH_6
                join t1201 in mpms.TBM1201 on t1302.OVC_PURCH equals t1201.OVC_PURCH
                join tbm1407 in mpms.TBM1407 on t1201.OVC_POI_IUNIT equals tbm1407.OVC_PHR_ID
                where tbm1407.OVC_PHR_CATE == "J1"
                select new
                {
                    OVC_PURCH = t1302.OVC_PURCH,
                    OVC_PURCH_5 = t1302.OVC_PURCH_5,
                    OVC_PURCH_6 = t1302.OVC_PURCH_6,
                    ONB_ICOUNT = t1302.ONB_ICOUNT,
                    OVC_PUR_IPURCH = t1201.OVC_POI_NSTUFF_CHN,
                    NSN = t1201.NSN,
                    OVC_POI_IUNIT = tbm1407.OVC_PHR_DESC,
                    ONB_POI_QORDER_CONT = t1201.ONB_POI_QORDER_CONT,
                    ONB_POI_MPRICE_PLAN = t1201.ONB_POI_MPRICE_PLAN,
                    ONB_POI_MPRICE_CONT = t1201.ONB_POI_MPRICE_CONT,
                };
            dt = CommonStatic.LinqQueryToDataTable(query1302item);
            ViewState["Status"] = FCommon.GridView_dataImport(GV_info, dt);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            short onbgroup = Convert.ToInt16((ViewState["strONB_GROUP"].ToString()));
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strResult = txtResult.Text;
            string strONB_MONEY_DISCOUNT = txtONB_MONEY_DISCOUNT.Text;
            string strMessage = "";
            if (!strResult.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strResult, out n))
                {

                }
                else
                {
                    strMessage += "<P> 試算結果請輸入數字 </p>";
                }
            }
            if (!strONB_MONEY_DISCOUNT.Equals(string.Empty))
            {
                int n;
                if (int.TryParse(strONB_MONEY_DISCOUNT, out n))
                {

                }
                else
                {
                    strMessage += "<P> 折讓請輸入數字 </p>";
                }
            }
            if (strMessage.Equals(string.Empty))
            {
                int result = Convert.ToInt32(strResult);
                int onbdiscount = Convert.ToInt32(strONB_MONEY_DISCOUNT);
                var query1302 =
                (from tbm1302 in mpms.TBM1302
                 where tbm1302.OVC_PURCH == strOVC_PURCH
                 where tbm1302.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbm1302.ONB_GROUP == onbgroup
                 select tbm1302).FirstOrDefault();
                query1302.ONB_MONEY = result;
                query1302.ONB_MONEY_DISCOUNT = onbdiscount;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), query1302.GetType().Name.ToString(), this, "修改");
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功!!");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }

        protected void btnDo_Click(object sender, EventArgs e)
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strONB_GROUP = ViewState["ONB_GROUP"].ToString();
            short onbgroup = Convert.ToInt16(strONB_GROUP);
            var tbm1302 =
                (from t1302 in mpms.TBM1302
                 where t1302.OVC_PURCH == strOVC_PURCH
                 where t1302.OVC_PURCH_5 == strOVC_PURCH_5
                 where t1302.ONB_GROUP == onbgroup
                 select t1302).FirstOrDefault();
            if (tbm1302 != null)
            {
                string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH"].ToString()));
                string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["strOVC_PURCH_5"].ToString()));
                string key3 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ViewState["ONB_GROUP"].ToString()));
                Response.Redirect("MPMS_D38.aspx?OVC_PURCH=" + key + "&OVC_PURCH_5=" + key2 + "&ONB_GROUP=" + key3);
            }
            else
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先製作契約草稿");
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_D37.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString() + "&ONB_GROUP=" + ViewState["strONB_GROUP"].ToString());
        }
        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            Response.Redirect("MPMS_D14_1.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString());
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Draft_Contract_Details("PDF");
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Draft_Contract_Details("Word");
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

        void Draft_Contract_Details(string WordOrPDF)
        {
            string ovcIkind = "";
            int first = 0;
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string wordfilepath = "";
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/DraftContractDetails_D39.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    doc.ReplaceText("[$PURCH$]", purch, false, System.Text.RegularExpressions.RegexOptions.None);
                    var query1301 =
                        from tbm1301 in mpms.TBM1301
                        where tbm1301.OVC_PURCH.Equals(purch)
                        select tbm1301.OVC_PUR_AGENCY;
                    foreach (var q in query1301)
                    {
                        switch (q)
                        {
                            case "B":
                            case "L":
                            case "P":
                                ovcIkind = "D5";
                                break;
                            case "M":
                            case "S":
                                ovcIkind = "F5";
                                break;
                            default:
                                ovcIkind = "W5";
                                break;
                        }
                    }
                    wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/DraftContractDetails_D39.pdf";
                    #region 品名料號及規格詳細資料
                    var groceryListTable = doc.Tables.FirstOrDefault(table => table.TableCaption == "Table_1");
                    if (groceryListTable != null)
                    {
                        var rowPattern_1 = groceryListTable.Rows[1];
                        var rowPattern_2 = groceryListTable.Rows[2];
                        var rowPattern_3 = groceryListTable.Rows[3];
                        rowPattern_1.Remove();
                        rowPattern_2.Remove();
                        rowPattern_3.Remove();
                        var query1201 =
                            from tbm1201 in mpms.TBM1201
                            where tbm1201.OVC_PURCH.Equals(purch)
                            orderby tbm1201.ONB_POI_ICOUNT descending
                            select new
                            {
                                ONB_POI_ICOUNT = tbm1201.ONB_POI_ICOUNT,
                                OVC_POI_NSTUFF_CHN = tbm1201.OVC_POI_NSTUFF_CHN,
                                OVC_POI_NSTUFF_ENG = tbm1201.OVC_POI_NSTUFF_ENG,
                                NSN = tbm1201.NSN,
                                OVC_BRAND = tbm1201.OVC_BRAND,
                                OVC_MODEL = tbm1201.OVC_MODEL,
                                OVC_POI_IREF = tbm1201.OVC_POI_IREF,
                                OVC_FCODE = tbm1201.OVC_FCODE,
                                OVC_POI_IUNIT = tbm1201.OVC_POI_IUNIT,
                                ONB_POI_QORDER_PLAN = tbm1201.ONB_POI_QORDER_PLAN,
                                ONB_POI_MPRICE_PLAN = tbm1201.ONB_POI_MPRICE_PLAN,
                                OVC_POI_NDESC = tbm1201.OVC_POI_NDESC,
                            };
                        foreach (var q in query1201)
                        {
                            var item = rowPattern_2;
                            if (first == 0)
                                item = rowPattern_3;
                            else
                                item = rowPattern_2;
                            first++;
                            var newItem_2 = groceryListTable.InsertRow(item, 1);
                            var newItem_1 = groceryListTable.InsertRow(rowPattern_1, 1);
                            newItem_1.ReplaceText("[$ONB_POI_ICOUNT$]", q.ONB_POI_ICOUNT.ToString());
                            if (q.OVC_POI_NSTUFF_CHN != null)
                                newItem_1.ReplaceText("[$OVC_POI_NSTUFF_CHN$]", q.OVC_POI_NSTUFF_CHN);
                            else
                                newItem_1.ReplaceText("[$OVC_POI_NSTUFF_CHN$]", "");
                            var query1407 =
                                from tbm1407 in mpms.TBM1407
                                where tbm1407.OVC_PHR_CATE.Equals("J1")
                                where tbm1407.OVC_PHR_ID.Equals(q.OVC_POI_IUNIT)
                                select new
                                {
                                    OVC_PHR_DESC = tbm1407.OVC_PHR_DESC,
                                    OVC_PHR_REMARK = tbm1407.OVC_PHR_REMARK
                                };
                            foreach (var qu in query1407)
                            {
                                if (qu.OVC_PHR_DESC != null)
                                    newItem_1.ReplaceText("[$OVC_POI_IUNIT$]", qu.OVC_PHR_DESC ?? "");
                            }
                            if (q.ONB_POI_QORDER_PLAN != null)
                                newItem_1.ReplaceText("[$ONB_POI_QORDER_PLAN$]", String.Format("{0:N}", q.ONB_POI_QORDER_PLAN));
                            else
                                newItem_1.ReplaceText("[$ONB_POI_QORDER_PLAN$]", "0");
                            if (q.ONB_POI_MPRICE_PLAN != null)
                                newItem_1.ReplaceText("[$ONB_POI_MPRICE_PLAN$]", String.Format("{0:N}", q.ONB_POI_MPRICE_PLAN));
                            else
                                newItem_1.ReplaceText("[$ONB_POI_MPRICE_PLAN$]", "0.00");
                            if (q.ONB_POI_QORDER_PLAN != null && q.ONB_POI_MPRICE_PLAN != null)
                                newItem_1.ReplaceText("[$TOTAL$]", String.Format("{0:N}", String.Format("{0:N}", q.ONB_POI_MPRICE_PLAN * q.ONB_POI_QORDER_PLAN)));
                            else
                                newItem_1.ReplaceText("[$TOTAL$]", "0.00");
                            if (q.OVC_POI_NDESC != null)
                                newItem_1.ReplaceText("[$OVC_POI_NDESC$]", q.OVC_POI_NDESC);
                            else
                                newItem_1.ReplaceText("[$OVC_POI_NDESC$]", "");
                            if (q.OVC_POI_NSTUFF_ENG != null)
                                newItem_2.ReplaceText("[$OVC_POI_NSTUFF_ENG$]", q.OVC_POI_NSTUFF_ENG);
                            else
                                newItem_2.ReplaceText("[$OVC_POI_NSTUFF_ENG$]", "(空白)");
                            if (q.NSN != null)
                                newItem_2.ReplaceText("[$NSN$]", q.NSN);
                            else
                                newItem_2.ReplaceText("[$NSN$]", "(空白)");
                            if (q.OVC_BRAND != null)
                                newItem_2.ReplaceText("[$OVC_BRAND$]", q.OVC_BRAND);
                            else
                                newItem_2.ReplaceText("[$OVC_BRAND$]", "(空白)");
                            if (q.OVC_MODEL != null)
                                newItem_2.ReplaceText("[$OVC_MODEL$]", q.OVC_MODEL);
                            else
                                newItem_2.ReplaceText("[$OVC_MODEL$]", "(空白)");
                            if (q.OVC_POI_IREF != null)
                                newItem_2.ReplaceText("[$OVC_POI_IREF$]", q.OVC_POI_IREF);
                            else
                                newItem_2.ReplaceText("[$OVC_POI_IREF$]", "(空白)");
                            if (q.OVC_FCODE != null)
                                newItem_2.ReplaceText("[$OVC_FCODE$]", q.OVC_FCODE);
                            else
                                newItem_2.ReplaceText("[$OVC_FCODE$]", "(空白)");
                            var query1233 =
                                from tbm1233 in mpms.TBM1233
                                where tbm1233.OVC_PURCH.Equals(purch)
                                where tbm1233.ONB_POI_ICOUNT.Equals(q.ONB_POI_ICOUNT)
                                orderby tbm1233.ONB_POI_ICOUNT
                                select tbm1233.OVC_POI_NDESC;
                            foreach (var qu in query1233)
                            {
                                newItem_2.ReplaceText("[$POI_NDESC$]", qu);
                            }
                        }
                    }
                    #endregion
                    #region 備註
                    var groceryListTable_2 = doc.Tables.FirstOrDefault(table => table.TableCaption == "Table_2");
                    if (groceryListTable_2 != null)
                    {
                        var rowPattern = groceryListTable_2.Rows[1];
                        rowPattern.Remove();
                        var query1220 =
                            from tbm1220_1 in mpms.TBM1220_1
                            join tbm1220_2 in mpms.TBM1220_2
                                on tbm1220_1.OVC_IKIND equals tbm1220_2.OVC_IKIND
                            where tbm1220_1.OVC_PURCH.Equals(purch)
                            where tbm1220_1.OVC_IKIND.Contains(ovcIkind)
                            select new
                            {
                                OVC_IKIND = tbm1220_1.OVC_IKIND,
                                OVC_MEMO = tbm1220_1.OVC_MEMO,
                                OVC_STANDARD = tbm1220_1.OVC_STANDARD,
                                OVC_MEMO_NAME = tbm1220_2.OVC_MEMO_NAME
                            };
                        foreach (var q in query1220)
                        {
                            var newItem = groceryListTable_2.InsertRow(rowPattern, groceryListTable_2.RowCount - 1);
                            newItem.ReplaceText("[$MEMO$]", q.OVC_MEMO_NAME + "：\r" + q.OVC_MEMO);
                        }
                    }
                    #endregion
                    var queryD20 =
                            from tbm1220_1 in mpms.TBM1220_1
                            where tbm1220_1.OVC_PURCH.Equals(purch)
                            where tbm1220_1.OVC_IKIND.Equals("D20")
                            select tbm1220_1.OVC_MEMO;
                    foreach (var q in queryD20)
                    {
                        doc.ReplaceText("[$D20$]", q, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$D20$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_ATTACH_NAME$]", GetAttached("D"), false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.SaveAs(Request.PhysicalApplicationPath + "Tempprint/b.docx");
                }
                buffer = ms.ToArray();
            }
            string path_d = Path.Combine(Request.PhysicalApplicationPath, "Tempprint/b.docx");
            if (WordOrPDF == "PDF")
            {
                WordcvDdf(path_d, wordfilepath);
                FileInfo file = new FileInfo(wordfilepath);
                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + DateTime.Now + ".pdf");
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(file.FullName);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.Flush();
                Response.End();
            }
            else
            {
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
        private string GetAttached(string kind)
        {
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            //附件內容
            var queryFile =
                mpms.TBM1119.AsEnumerable()
                .Where(o => o.OVC_PURCH.Equals(purch) && o.OVC_IKIND.Equals(kind))
                .OrderByDescending(o => o.OVC_ATTACH_NAME.Split('.')[0].Length)
                .OrderBy(o => o.OVC_ATTACH_NAME.Split('.')[0]);
            string[] arrFileName = new string[queryFile.Count()];
            int counter = 0;
            foreach (var row in queryFile)
            {
                if (row.ONB_PAGES == 0)
                    arrFileName[counter] = row.OVC_ATTACH_NAME + row.ONB_PAGES + "份";
                else
                        arrFileName[counter] = row.OVC_ATTACH_NAME + row.ONB_PAGES + "份(" + row.ONB_PAGES.ToString() + "頁)";
                counter++;
            }
            string strFileName = string.Join("、", arrFileName);
            return strFileName;
        }

        
        

        #region WordToPDF
        static void WordcvDdf(string args, string wordfilepath)
        {
            // word 檔案位置
            string sourcedocx = args;
            // PDF 儲存位置
            // string targetpdf =  @"C:\Users\linon\Downloads\ddd.pdf";

            //建立 word application instance
            Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
            //開啟 word 檔案
            var wordDocument = appWord.Documents.Open(sourcedocx);

            //匯出為 pdf
            wordDocument.ExportAsFixedFormat(wordfilepath, WdExportFormat.wdExportFormatPDF);

            //關閉 word 檔
            wordDocument.Close();
            //結束 word
            appWord.Quit();
        }
        #endregion
    }
}