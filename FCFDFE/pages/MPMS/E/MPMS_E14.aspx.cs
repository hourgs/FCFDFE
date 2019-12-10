using System;
using System.Linq;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Web;
using System.Net;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E14 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        string name, listYear, status;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            if (Session["name"] != null && Session["listYear"] != null && Session["status"] != null)
            {
                if (!IsPostBack)
                {
                    GV_dataImport();
                    Session.Contents.Remove("rowtext");
                    Session.Contents.Remove("E15");
                }
            }
            else
                FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
        }

        #region Click
        //選擇btn
        protected void BtnSelect_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_BuyCaseStage = (GridViewRow)BtnTakeOver.NamingContainer;
            Label OVC_PURCH = (Label)GV_BuyCaseStage.FindControl("labOVC_PURCH");
            Label labOVC_PURCH_6 = (Label)GV_BuyCaseStage.FindControl("labOVC_PURCH_6");
            Session["rowtext"] = OVC_PURCH.Text;
            Session["rowgroup"] = GV_BuyCaseStage.Cells[1].Text;
            Session["rowven"] = GV_BuyCaseStage.Cells[3].Text;
            Session["purch_6"] = labOVC_PURCH_6.Text;
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }
        //產生Excel
        protected void btn_Click(object sender, EventArgs e)
        {
            ExportToFile(GV_BuyCaseStage);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/CreatExcel_E14.xls");
            string filepath = Session["listYear"].ToString() + "年購案.xls";
            string fileName = HttpUtility.UrlEncode(filepath);
            WebClient wc = new WebClient(); //宣告並建立WebClient物件
            byte[] b = wc.DownloadData(path_temp); //載入要下載的檔案
            Response.Clear(); //清除Response內的HTML
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName); //設定標頭檔資訊 attachment 是本文章的關鍵字
            Response.BinaryWrite(b); //開始輸出讀取到的檔案
            File.Delete(path_temp);
            Response.End();
        }
        protected void btn_Click_2(object sender, EventArgs e)
        {
            ExportToFile(GV_BuyCaseStage);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/CreatExcel_E14.xls");
            string path_temp_ods = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/CreatExcel_E14.ods");
            File.Delete(path_temp_ods);
            string filepath = Session["listYear"].ToString() + "年購案.ods";
            string fileName = HttpUtility.UrlEncode(filepath);
            ExcelToODS(path_temp, path_temp_ods, fileName);
        }
        //回上一頁
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E11.aspx";
            Response.Redirect(send_url);
        }
        #endregion

        #region 副程式
        #region GridView資料帶入
        private void GV_dataImport()
        {
            DataTable dt = new DataTable();
            if (Session["name"] != null)
            {
                name = Session["name"].ToString();
                listYear = Session["listYear"].ToString();
                status = Session["status"].ToString();

                if (status.Equals("total"))
                {
                    var query =
                    from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                    join tbm1302 in mpms.TBM1302 on tbmreceive.OVC_PURCH equals tbm1302.OVC_PURCH
                    join tbm1301 in mpms.TBM1301_PLAN on tbmreceive.OVC_PURCH equals tbm1301.OVC_PURCH
                    where tbmreceive.OVC_PURCH_6.Equals(tbm1302.OVC_PURCH_6)
                    where tbmreceive.OVC_VEN_CST.Equals(tbm1302.OVC_VEN_CST)
                    where tbmreceive.ONB_GROUP.Equals(tbm1302.ONB_GROUP)
                    where tbm1302.OVC_DSEND != null
                    where tbm1302.OVC_DRECEIVE != null
                    where tbmreceive.OVC_DO_NAME.Equals(name)
                    where tbmreceive.OVC_PURCH.Substring(2, 2).Equals(listYear)
                    select new
                    {
                        OVC_PURCH = tbm1301.OVC_PURCH,
                        OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                        OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                        OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                        ONB_GROUP = tbmreceive.ONB_GROUP,
                        OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                        OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE
                    };
                    hasRows = true;
                    dt = CommonStatic.LinqQueryToDataTable(query);
                    ViewState["hasRows"] = FCommon.GridView_dataImport(GV_BuyCaseStage, dt);
                }
                else
                {
                    var query =
                    from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                    join tbm1302 in mpms.TBM1302 on tbmreceive.OVC_PURCH equals tbm1302.OVC_PURCH
                    join tbm1301 in mpms.TBM1301_PLAN on tbmreceive.OVC_PURCH equals tbm1301.OVC_PURCH
                    where tbmreceive.OVC_PURCH_6.Equals(tbm1302.OVC_PURCH_6)
                    where tbmreceive.OVC_VEN_CST.Equals(tbm1302.OVC_VEN_CST)
                    where tbmreceive.ONB_GROUP.Equals(tbm1302.ONB_GROUP)
                    where tbm1302.OVC_DSEND != null
                    where tbm1302.OVC_DRECEIVE != null
                    where tbmreceive.OVC_DO_NAME.Equals(name)
                    where tbmreceive.OVC_PURCH.Substring(2, 2).Equals(listYear)
                    where tbmreceive.OVC_DCLOSE.Equals(null)
                    select new
                    {
                        OVC_PURCH = tbmreceive.OVC_PURCH,
                        OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                        OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                        OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                        ONB_GROUP = tbmreceive.ONB_GROUP,
                        OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                        OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE
                    };
                    hasRows = true;
                    dt = CommonStatic.LinqQueryToDataTable(query);
                    ViewState["hasRows"] = FCommon.GridView_dataImport(GV_BuyCaseStage, dt);
                }
            }
        }
        #endregion

        #region GridView
        protected void GV_BuyCaseStage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string labOVC_PURCH;
                string labOVC_PUR_AGENCY;
                string labOVC_PURCH_5;
                string labOVC_PURCH_6;
                Label OVC_PURCH = (Label)e.Row.FindControl("labOVC_PURCH");
                Label OVC_PUR_AGENCY = (Label)e.Row.FindControl("labOVC_PUR_AGENCY");
                Label OVC_PURCH_5 = (Label)e.Row.FindControl("labOVC_PURCH_5");
                Label OVC_PURCH_6 = (Label)e.Row.FindControl("labOVC_PURCH_6");
                Label labONB_SHIP_TIMES = (Label)e.Row.FindControl("labONB_SHIP_TIMES");
                Label labOVC_DELIVERY_CONTRACT = (Label)e.Row.FindControl("labOVC_DELIVERY_CONTRACT");
                Label labOVC_DELIVERY = (Label)e.Row.FindControl("labOVC_DELIVERY");
                Label labTimeSpan = (Label)e.Row.FindControl("labTimeSpan");
                Label labMain = (Label)e.Row.FindControl("labMain");
                Label labOVC_DELAY_REASON = (Label)e.Row.FindControl("labOVC_DELAY_REASON");
                Label labMargin = (Label)e.Row.FindControl("labMargin");
                Label labCashT = (Label)e.Row.FindControl("labCashT");
                Label labCash = (Label)e.Row.FindControl("labCash");
                Label labPromT = (Label)e.Row.FindControl("labPromT");
                Label labProm = (Label)e.Row.FindControl("labProm");
                Label labPromT_2 = (Label)e.Row.FindControl("labPromT_2");
                Label labProm_2 = (Label)e.Row.FindControl("labProm_2");
                Label labPromT_3 = (Label)e.Row.FindControl("labPromT_3");
                Label labProm_3 = (Label)e.Row.FindControl("labProm_3");
                Label labStockT = (Label)e.Row.FindControl("labStockT");
                Label labStock = (Label)e.Row.FindControl("labStock");
                OVC_PURCH.Text = OVC_PURCH.Text + OVC_PUR_AGENCY.Text + OVC_PURCH_5.Text + OVC_PURCH_6.Text;
                var purch = e.Row.Cells[0].Text;
                var query =
                    from tbm1301 in mpms.TBM1301_PLAN
                    join tbm1302 in mpms.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                    where tbm1301.OVC_PURCH.Equals(OVC_PURCH.Text.Substring(0, 7))
                    where tbm1302.OVC_PURCH_6.Equals(OVC_PURCH_6.Text)
                    select new
                    {
                        OVC_PURCH = tbm1301.OVC_PURCH,
                        OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                        OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                        OVC_PURCH_6 = tbm1302.OVC_PURCH_6
                    };
                foreach (var q in query)
                {
                    labOVC_PURCH = q.OVC_PURCH;
                    labOVC_PUR_AGENCY = q.OVC_PUR_AGENCY;
                    labOVC_PURCH_5 = q.OVC_PURCH_5;
                    labOVC_PURCH_6 = q.OVC_PURCH_6;

                    #region 辦理狀況
                    name = Session["name"].ToString();
                    var queryStatus =
                        from tbmdelivery in mpms.TBMDELIVERY
                        where tbmdelivery.OVC_PURCH.Equals(labOVC_PURCH)
                        where tbmdelivery.OVC_PURCH_6.Equals(labOVC_PURCH_6)
                        select new
                        {
                            ONB_SHIP_TIMES = tbmdelivery.ONB_SHIP_TIMES,
                            OVC_DELIVERY_CONTRACT = tbmdelivery.OVC_DELIVERY_CONTRACT,
                            OVC_DELIVERY = tbmdelivery.OVC_DELIVERY,
                            OVC_DELAY_REASON = tbmdelivery.OVC_DELAY_REASON
                        };
                    foreach (var qu in queryStatus)
                    {
                        labONB_SHIP_TIMES.Text = qu.ONB_SHIP_TIMES.ToString();
                        labOVC_DELIVERY_CONTRACT.Text = qu.OVC_DELIVERY_CONTRACT;
                        labOVC_DELIVERY.Text = qu.OVC_DELIVERY;
                        labOVC_DELAY_REASON.Text = qu.OVC_DELAY_REASON;
                    }
                    #endregion
                    #region 超前or落後
                    if (labOVC_DELIVERY_CONTRACT.Text != null && labOVC_DELIVERY_CONTRACT.Text != "" && labOVC_DELIVERY.Text != null && labOVC_DELIVERY.Text != "")
                    {
                        DateTime dtOVC_DELIVERY_CONTRACT = DateTime.ParseExact(labOVC_DELIVERY_CONTRACT.Text, "yyyy-MM-dd", null);
                        DateTime dtOVC_DELIVERY = DateTime.ParseExact(labOVC_DELIVERY.Text, "yyyy-MM-dd", null);
                        TimeSpan ts = dtOVC_DELIVERY_CONTRACT - dtOVC_DELIVERY;
                        if (ts.Days < 0)
                        {
                            labTimeSpan.Text = "落後" + Math.Abs(ts.Days) + "天";
                            labTimeSpan.Attributes.Add("style", "color:red");
                        }
                        else
                        {
                            labTimeSpan.Text = "超前" + ts.Days + "天";
                            labTimeSpan.Attributes.Add("style", "color:green");
                        }
                    }
                    labOVC_DELIVERY_CONTRACT.Text = dateTW(labOVC_DELIVERY_CONTRACT.Text);
                    labOVC_DELIVERY.Text = dateTW(labOVC_DELIVERY.Text);
                    #endregion
                    #region 主要事項記載
                    if (labOVC_DELAY_REASON.Text == null || labOVC_DELAY_REASON.Text == "")
                    {
                        labMain.Visible = false;
                        labOVC_DELAY_REASON.Visible = false;
                    }
                    else
                    {
                        labMain.Visible = true;
                        labOVC_DELAY_REASON.Visible = true;
                        labMain.Attributes.Add("style", "color:blue");
                        labOVC_DELAY_REASON.Attributes.Add("style", "color:blue");
                    }
                    #endregion
                    #region 免收履約保證金
                    var queryMarginFirst =
                        from tbm1301 in mpms.TBM1301_PLAN
                        where tbm1301.OVC_PURCH.Equals(labOVC_PURCH)
                        select tbm1301.OVC_PUR_AGENCY;
                    var ovcikind = "";
                    foreach (var qu in queryMarginFirst)
                    {

                        if (qu.ToString() == "L" || qu.ToString() == "P" || qu.ToString() == "B")
                            ovcikind = "D55";
                        else if (qu.ToString() == "W" || qu.ToString() == "C" || qu.ToString() == "E" || qu.ToString() == "A")
                            ovcikind = "W55";
                    }
                    string norece = "";
                    bool had_receive = false;
                    var queryMargin =
                        from tbm1220 in mpms.TBM1220_1
                        where tbm1220.OVC_PURCH.Equals(labOVC_PURCH)
                        where tbm1220.OVC_IKIND.Equals(ovcikind)
                        select tbm1220.OVC_MEMO;
                    foreach (var qu in queryMargin)
                    {
                        if (qu.Equals("免收履約保證金"))
                        {
                            norece = "免收履約保證金";
                            had_receive = false;
                        }
                    }
                    #endregion
                    #region 代管現金
                    var queryCash =
                        from tbmmanage in mpms.TBMMANAGE_CASH
                        where tbmmanage.OVC_PURCH.Equals(labOVC_PURCH)
                        where tbmmanage.OVC_PURCH_5.Equals(labOVC_PURCH_5)
                        where tbmmanage.OVC_PURCH_6.Equals(labOVC_PURCH_6)
                        select new
                        {
                            OVC_DBACK = tbmmanage.OVC_DBACK,
                            OVC_KIND = tbmmanage.OVC_KIND,
                            ONB_MONEY_NT = tbmmanage.ONB_MONEY_NT_1 + tbmmanage.ONB_MONEY_NT_2 + tbmmanage.ONB_MONEY_NT_3
                        };
                    foreach (var qu in queryCash)
                    {
                        if (qu.OVC_DBACK == null)
                        {
                            if (qu.OVC_KIND == "1")
                            {
                                labCashT.Text = "履保代收(現金)：";
                                had_receive = true;
                            }
                            else
                            {
                                labCashT.Text = "保固代收(現金)：";
                            }
                        }
                        labCash.Text = "新台幣" + String.Format("{0:N}", qu.ONB_MONEY_NT) + "元";
                    }
                    #endregion
                    #region 代管文件
                    var queryProm =
                        from tbmmanage in mpms.TBMMANAGE_PROM
                        where tbmmanage.OVC_PURCH.Equals(labOVC_PURCH)
                        where tbmmanage.OVC_PURCH_5.Equals(labOVC_PURCH_5)
                        where tbmmanage.OVC_PURCH_6.Equals(labOVC_PURCH_6)
                        select new
                        {
                            OVC_DBACK = tbmmanage.OVC_DBACK,
                            OVC_KIND = tbmmanage.OVC_KIND,
                            OVC_NSTOCK_1 = tbmmanage.OVC_NSTOCK_1,
                            OVC_DEFFECT_1 = tbmmanage.OVC_DEFFECT_1,
                            OVC_NSTOCK_2 = tbmmanage.OVC_NSTOCK_2,
                            OVC_DEFFECT_2 = tbmmanage.OVC_DEFFECT_2,
                            OVC_NSTOCK_3 = tbmmanage.OVC_NSTOCK_3,
                            OVC_DEFFECT_3 = tbmmanage.OVC_DEFFECT_3
                        };
                    foreach (var qu in queryProm)
                    {
                        if (qu.OVC_DBACK == null)
                        {
                            if (qu.OVC_NSTOCK_1 != null)
                            {
                                if (qu.OVC_KIND == "1")
                                {
                                    labPromT.Text = "履保代收(";
                                    had_receive = true;
                                }
                                else
                                {
                                    labPromT.Text = "保固代收(";
                                }
                                labPromT.Text += qu.OVC_NSTOCK_1 + ")";

                                if (qu.OVC_DEFFECT_1 != null)
                                {
                                    labProm.Text = "有效期" + dateTW(qu.OVC_DEFFECT_1);
                                    labProm.Attributes.Add("style", "color:red");
                                }

                                if (qu.OVC_KIND == "2")
                                {
                                    if (qu.OVC_DEFFECT_1 != null)
                                    {
                                        DateTime dtOVC_DEFFECT_1 = DateTime.ParseExact(qu.OVC_DEFFECT_1, "yyyy-MM-dd", null);
                                        TimeSpan ts = dtOVC_DEFFECT_1 - DateTime.Today;
                                        if (ts.Days < 0)
                                        {
                                            labProm.Text = "(已經到期)";
                                        }
                                        else
                                        {
                                            labProm.Text = "(還有" + ts.Days + "天即將到期)";
                                        }
                                    }
                                }
                            }

                            if (qu.OVC_NSTOCK_2 != null)
                            {
                                if (qu.OVC_KIND == "1")
                                {
                                    labPromT_2.Text = "履保代收(";
                                    had_receive = true;
                                }
                                else
                                {
                                    labPromT_2.Text = "保固代收(";
                                }
                                labPromT_2.Text += qu.OVC_NSTOCK_2 + ")";

                                if (qu.OVC_DEFFECT_2 != null)
                                {
                                    labProm_2.Text = "有效期" + dateTW(qu.OVC_DEFFECT_2);
                                    labProm_2.Attributes.Add("style", "color:red");
                                }

                                if (qu.OVC_KIND == "2")
                                {
                                    if (qu.OVC_DEFFECT_2 != null)
                                    {
                                        DateTime dtOVC_DEFFECT_2 = DateTime.ParseExact(qu.OVC_DEFFECT_2, "yyyy-MM-dd", null);
                                        TimeSpan ts = dtOVC_DEFFECT_2 - DateTime.Today;
                                        if (ts.Days < 0)
                                        {
                                            labProm_2.Text = "(已經到期)";
                                        }
                                        else
                                        {
                                            labProm_2.Text = "(還有" + ts.Days + "天即將到期)";
                                        }
                                    }
                                }
                            }

                            if (qu.OVC_NSTOCK_3 != null)
                            {
                                if (qu.OVC_KIND == "1")
                                {
                                    labPromT_3.Text = "履保代收(";
                                    had_receive = true;
                                }
                                else
                                {
                                    labPromT_3.Text = "保固代收(";
                                }
                                labPromT_3.Text += qu.OVC_NSTOCK_3 + ")";

                                if (qu.OVC_DEFFECT_3 != null)
                                {
                                    labProm_3.Text = "有效期" + dateTW(qu.OVC_DEFFECT_3);
                                    labProm_3.Attributes.Add("style", "color:red");
                                }

                                if (qu.OVC_KIND == "2")
                                {
                                    if (qu.OVC_DEFFECT_3 != null)
                                    {
                                        DateTime dtOVC_DEFFECT_3 = DateTime.ParseExact(qu.OVC_DEFFECT_3, "yyyy-MM-dd", null);
                                        TimeSpan ts = dtOVC_DEFFECT_3 - DateTime.Today;
                                        if (ts.Days < 0)
                                        {
                                            labProm_3.Text = "(已經到期)";
                                        }
                                        else
                                        {
                                            labProm_3.Text = "(還有" + ts.Days + "天即將到期)";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region 有價證券
                    var queryStock =
                        from tbmmanage in mpms.TBMMANAGE_STOCK
                        where tbmmanage.OVC_PURCH.Equals(labOVC_PURCH)
                        where tbmmanage.OVC_PURCH_5.Equals(labOVC_PURCH_5)
                        where tbmmanage.OVC_PURCH_6.Equals(labOVC_PURCH_6)
                        select new
                        {
                            OVC_DBACK = tbmmanage.OVC_DBACK,
                            OVC_KIND = tbmmanage.OVC_KIND,
                            OVC_NSTOCK_1 = tbmmanage.OVC_NSTOCK_1,
                            ONB_MONEY_NT_1 = tbmmanage.ONB_MONEY_NT_1,
                            OVC_NSTOCK_2 = tbmmanage.OVC_NSTOCK_2,
                            ONB_MONEY_NT_2 = tbmmanage.ONB_MONEY_NT_2,
                            OVC_NSTOCK_3 = tbmmanage.OVC_NSTOCK_3,
                            ONB_MONEY_NT_3 = tbmmanage.ONB_MONEY_NT_3,
                            OVC_DGARRENT_END = tbmmanage.OVC_DGARRENT_END
                        };
                    foreach (var qu in queryStock)
                    {
                        if (qu.OVC_DBACK != null)
                        {
                            if (qu.OVC_KIND == "1")
                            {
                                labStockT.Text = "履保代收：";
                                had_receive = true;
                            }
                            else
                            {
                                labStockT.Text = "保固代收：";
                            }

                            if (qu.OVC_NSTOCK_1 != null)
                                labStock.Text += "(" + qu.OVC_NSTOCK_1 + ")" + String.Format("{0:N}", qu.ONB_MONEY_NT_1) + "元<br>";
                            if (qu.OVC_NSTOCK_2 != null)
                                labStock.Text += "(" + qu.OVC_NSTOCK_2 + ")" + String.Format("{0:N}", qu.ONB_MONEY_NT_2) + "元<br>";
                            if (qu.OVC_NSTOCK_3 != null)
                                labStock.Text += "(" + qu.OVC_NSTOCK_3 + ")" + String.Format("{0:N}", qu.ONB_MONEY_NT_3) + "元";

                            if (qu.OVC_KIND == "2")
                            {
                                if (qu.OVC_DGARRENT_END != null)
                                {
                                    string strOVC_DGARRENT_END = qu.OVC_DGARRENT_END.Replace("-", "");
                                    DateTime dtOVC_DGARRENT_END = DateTime.ParseExact(strOVC_DGARRENT_END, "yyyyMMdd", null);
                                    TimeSpan ts = dtOVC_DGARRENT_END - DateTime.Today;
                                    if (ts.Days < 0)
                                    {
                                        labStock.Text = "(已經到期)";
                                    }
                                    else
                                    {
                                        labStock.Text = "(還有" + ts.Days + "天即將到期)";
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region 免收履約保證金顯示
                    if (norece == "免收履約保證金")
                    {
                        if (had_receive)
                            labMargin.Text = "計畫清單免收履約保證金(但已經有收履保金)";
                        else
                            labMargin.Text = "免收履約保證金";
                        labMargin.Attributes.Add("style", "color:blue");
                    }
                    #endregion
                    string status = "<p>第" + labONB_SHIP_TIMES.Text + "批契約交貨日期：" + labOVC_DELIVERY_CONTRACT.Text + "</p><p>實際交貨日期：" + labOVC_DELIVERY.Text + "</p><p>" + labTimeSpan.Text + "</p><p>主要事項記載：" + labOVC_DELAY_REASON.Text + "</p><p>" + labCashT.Text + labCash.Text + "</p><p>" + labPromT.Text + "</p><p>" + labProm.Text + "</p><p>" + labPromT_2.Text + "</p><p>" + labProm_2.Text + "</p><p>" + labPromT_3.Text + "</p><p>" + labProm_3.Text + "</p><p>" + labStockT.Text + "</p><p>" + labStock.Text + "</p><p>" + labMargin.Text + "</p>";
                }
            }
        }
        protected void GV_BuyCaseStage_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Insert);
                TableCell tc1 = new TableCell();
                TableCell tc2 = new TableCell();
                Label lab1 = new Label();
                Label lab2 = new Label();
                Label lab3 = new Label();
                Button btn = new Button();
                Button btn_2 = new Button();
                lab1.Text = "合計";
                if (Session["status"].ToString() == "total")
                {
                    var query =
                    from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                    join tbm1302 in mpms.TBM1302
                        on tbmreceive.OVC_PURCH equals tbm1302.OVC_PURCH
                    join tbm1301 in mpms.TBM1301_PLAN
                        on tbmreceive.OVC_PURCH equals tbm1301.OVC_PURCH
                    where tbmreceive.OVC_PURCH_6.Equals(tbm1302.OVC_PURCH_6)
                    where tbmreceive.OVC_VEN_CST.Equals(tbm1302.OVC_VEN_CST)
                    where tbmreceive.ONB_GROUP.Equals(tbm1302.ONB_GROUP)
                    where tbm1302.OVC_DSEND != null
                    where tbm1302.OVC_DRECEIVE != null
                    where tbmreceive.OVC_DO_NAME.Equals(name)
                    where tbmreceive.OVC_PURCH.Substring(2, 2).Equals(listYear)
                    select tbmreceive;
                    lab2.Text = query.Count() + " 案\r\n";
                }
                else
                {
                    var query =
                    from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                    join tbm1302 in mpms.TBM1302
                        on tbmreceive.OVC_PURCH equals tbm1302.OVC_PURCH
                    join tbm1301 in mpms.TBM1301_PLAN
                        on tbmreceive.OVC_PURCH equals tbm1301.OVC_PURCH
                    where tbmreceive.OVC_PURCH_6.Equals(tbm1302.OVC_PURCH_6)
                    where tbmreceive.OVC_VEN_CST.Equals(tbm1302.OVC_VEN_CST)
                    where tbmreceive.ONB_GROUP.Equals(tbm1302.ONB_GROUP)
                    where tbm1302.OVC_DSEND != null
                    where tbm1302.OVC_DRECEIVE != null
                    where tbmreceive.OVC_DO_NAME.Equals(name)
                    where tbmreceive.OVC_PURCH.Substring(2, 2).Equals(listYear)
                    where tbmreceive.OVC_DCLOSE.Equals(null)
                    select tbmreceive;
                    lab2.Text = query.Count() + " 案\r\n";
                }
                lab3.Text = " ";
                btn.Text = "轉成Excel檔";
                btn.CssClass = "btn-success";
                btn.Click += new EventHandler(btn_Click); //事件
                btn_2.Text = "轉成ods檔";
                btn_2.CssClass = "btn-success";
                btn_2.Click += new EventHandler(btn_Click_2); //事件
                tc1.Controls.Add(lab1);
                tc1.ColumnSpan = 2;
                tc2.Controls.Add(lab2);
                tc2.Controls.Add(btn);
                tc2.Controls.Add(lab3);
                tc2.Controls.Add(btn_2);
                tc2.ColumnSpan = 4;
                gvRow.Cells.Add(tc1);
                gvRow.Cells.Add(tc2);
                GV_BuyCaseStage.Controls[0].Controls.Add(gvRow);
            }
        }
        protected void GV_BuyCaseStage_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GV_BuyCaseStage.UseAccessibleHeader = true;
                GV_BuyCaseStage.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        #endregion

        #region 產生Excel
        private void ExportToFile(GridView gv)
        {
            HSSFWorkbook wb = new HSSFWorkbook();
            ISheet ws;
            ws = wb.CreateSheet("統計表");
            MemoryStream ms = new MemoryStream();
            HSSFSheet ns = (HSSFSheet)wb.CreateSheet("mySheet");
            HSSFRow row = (HSSFRow)ns.CreateRow(0);
            row.CreateCell(0).SetCellValue("購案編號");
            row.CreateCell(1).SetCellValue("組別");
            row.CreateCell(2).SetCellValue("購案名稱");
            row.CreateCell(3).SetCellValue("得標商名稱");
            row.CreateCell(4).SetCellValue("辦理現況");
            ws.CreateRow(0);
            ws.GetRow(0).CreateCell(0).SetCellValue("序號");
            ws.GetRow(0).CreateCell(1).SetCellValue("購案編號");
            ws.GetRow(0).CreateCell(2).SetCellValue("組別");
            ws.GetRow(0).CreateCell(3).SetCellValue("購案名稱");
            ws.GetRow(0).CreateCell(4).SetCellValue("得標商名稱");
            ws.GetRow(0).CreateCell(5).SetCellValue("辦理狀況");
            for (int i = 1, iCount = gv.Rows.Count + 1; i < iCount; i++)
            {
                ws.CreateRow(i);
                ws.GetRow(i).CreateCell(0).SetCellValue(i.ToString());
                for (int j = 0, jCount = gv.HeaderRow.Cells.Count; j < jCount; j++)
                {
                    Label OVC_PURCH = (Label)gv.Rows[i - 1].Cells[0].FindControl("labOVC_PURCH");
                    Label labONB_SHIP_TIMES = (Label)gv.Rows[i - 1].Cells[4].FindControl("labONB_SHIP_TIMES");
                    Label labOVC_DELIVERY_CONTRACT = (Label)gv.Rows[i - 1].Cells[4].FindControl("labOVC_DELIVERY_CONTRACT");
                    Label labOVC_DELIVERY = (Label)gv.Rows[i - 1].Cells[4].FindControl("labOVC_DELIVERY");
                    Label labTimeSpan = (Label)gv.Rows[i - 1].Cells[4].FindControl("labTimeSpan");
                    Label labMain = (Label)gv.Rows[i - 1].Cells[4].FindControl("labMain");
                    Label labOVC_DELAY_REASON = (Label)gv.Rows[i - 1].Cells[4].FindControl("labOVC_DELAY_REASON");
                    Label labMargin = (Label)gv.Rows[i - 1].Cells[4].FindControl("labMargin");
                    Label labCashT = (Label)gv.Rows[i - 1].Cells[4].FindControl("labCashT");
                    Label labCash = (Label)gv.Rows[i - 1].Cells[4].FindControl("labCash");
                    Label labPromT = (Label)gv.Rows[i - 1].Cells[4].FindControl("labPromT");
                    Label labProm = (Label)gv.Rows[i - 1].Cells[4].FindControl("labProm");
                    Label labPromT_2 = (Label)gv.Rows[i - 1].Cells[4].FindControl("labPromT_2");
                    Label labProm_2 = (Label)gv.Rows[i - 1].Cells[4].FindControl("labProm_2");
                    Label labPromT_3 = (Label)gv.Rows[i - 1].Cells[4].FindControl("labPromT_3");
                    Label labProm_3 = (Label)gv.Rows[i - 1].Cells[4].FindControl("labProm_3");
                    Label labStockT = (Label)gv.Rows[i - 1].Cells[4].FindControl("labStockT");
                    Label labStock = (Label)gv.Rows[i - 1].Cells[4].FindControl("labStock");

                    if (j == 5)
                    {
                        string status = "第" + labONB_SHIP_TIMES.Text + "批契約交貨日期：" + labOVC_DELIVERY_CONTRACT.Text + "\n實際交貨日期：" + labOVC_DELIVERY.Text + "\n" + labTimeSpan.Text + "\n主要事項記載：" + labOVC_DELAY_REASON.Text + "\n" + labCashT.Text + labCash.Text + "\n" + labPromT.Text + "\n" + labProm.Text + "\n" + labPromT_2.Text + "\n" + labProm_2.Text + "\n" + labPromT_3.Text + "\n" + labProm_3.Text + "\n" + labStockT.Text + "\n" + labStock.Text + "\n" + labMargin.Text;
                        ws.GetRow(i).CreateCell(j).SetCellValue(status);
                    }
                    else if (j == 0)
                        ws.GetRow(i).CreateCell(j).SetCellValue(i.ToString());
                    else if (j == 1)
                        ws.GetRow(i).CreateCell(j).SetCellValue(OVC_PURCH.Text);
                    else
                        ws.GetRow(i).CreateCell(j).SetCellValue(gv.Rows[i - 1].Cells[j - 1].Text);
                }
                ws.AutoSizeColumn(i); //自動調整欄寬
            }
            ws.CreateRow(gv.Rows.Count + 1);
            ws.GetRow(gv.Rows.Count + 1).CreateCell(0).SetCellValue("合計");
            ws.GetRow(gv.Rows.Count + 1).CreateCell(1).SetCellValue(gv.Rows.Count + " 案");
            wb.Write(ms);

            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/CreatExcel_E14.xls");
            FileStream file = new FileStream(path_temp, FileMode.Create);//產生檔案
            wb.Write(file);
            file.Close();

            //string filepath = Session["listYear"].ToString() + "年購案.xls";
            //string fileName = HttpUtility.UrlEncode(filepath);
            //Response.AddHeader("Content-Disposition", String.Format("attachment;filename=" + fileName));
            //Response.BinaryWrite(ms.ToArray());
            //wb = null;
            //ms.Close();
            //ms.Dispose();
        }
        #endregion

        #endregion

        #region 民國年
        private string dateTW(string str)
        {
            DateTime dt;
            string strdt = "";
            if (DateTime.TryParse(str, out DateTime d))
            {
                dt = DateTime.Parse(str);
                strdt = (int.Parse(dt.Year.ToString()) - 1911).ToString() + "年" + dt.Month.ToString() + "月" + dt.Day.ToString() + "日";
                return strdt;
            }
            else
                return str;
        }
        #endregion

        #region Excel轉ods
        private void ExcelToODS(string FromPath, string TargetPath, string fileName)
        {
            var ExcelApp = new Microsoft.Office.Interop.Excel.Application();

            var workbooks = ExcelApp.Workbooks;
            var book = workbooks.Open(FromPath);
            // Microsoft.Office.Interop.Excel.Workbook book = ExcelApp.Workbooks.Open(FromPath);
            // Microsoft.Office.Interop.Excel.XlFileFormat xml = (Microsoft.Office.Interop.Excel.XlFileFormat)57;
            
            try
            {
                book.SaveAs(TargetPath, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenDocumentSpreadsheet);

                ExcelApp.Visible = false;
                book.Close();
                ExcelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ExcelApp);

                book = null;
                workbooks = null;
                ExcelApp = null;
                
                GC.Collect();
                GC.WaitForPendingFinalizers();

            }
            catch (Exception ex)
            {
                ExcelApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ExcelApp);

                book = null;
                workbooks = null;
                ExcelApp = null;
                
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            byte[] renderedBytes = null;
            var buffer = new byte[16 * 1024];
            using (var stream = new FileStream(TargetPath, FileMode.Open))
            {
                var memoryStream = new MemoryStream();
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memoryStream.Write(buffer, 0, read);
                        renderedBytes = memoryStream.ToArray();
                    }

                }

                Response.Clear();
                Response.ContentType = "Application/application/vnd.oasis.opendocument.text";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.BinaryWrite(renderedBytes);
                // memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.Close();
                Response.End();
            }
        }
        #endregion
    }
}