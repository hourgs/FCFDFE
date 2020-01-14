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
using Microsoft.International.Formatters;
using System.Globalization;
using System.Web;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D1D : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        public string sOVC_PURCH, sOVC_PURCH_5;
        bool hasRows = false, isUpload;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this, out isUpload))
            {
                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_PURCH_5"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    if (!IsPostBack && IsOVC_DO_NAME())
                    {
                        PnCompany.Visible = false;//異動合約商
                        PnContract.Visible = true;//簽約
                        PnEscrow.Visible = false;//保證金收繳
                        lblTitle.Text = "簽約";

                        #region drp
                        drpOVC_BUD_CURRENT.Items.Clear();
                        drpOVC_CURRENT.Items.Clear();
                        var queryCurrent =
                            from tbm1407 in mpms.TBM1407
                            where tbm1407.OVC_PHR_CATE.Equals("B0")
                            select new
                            {
                                tbm1407.OVC_PHR_ID,
                                tbm1407.OVC_PHR_DESC
                            };
                        foreach (var qu in queryCurrent)
                        {
                            drpOVC_BUD_CURRENT.Items.Add(new ListItem(qu.OVC_PHR_DESC, qu.OVC_PHR_ID));
                            drpOVC_CURRENT.Items.Add(new ListItem(qu.OVC_PHR_DESC, qu.OVC_PHR_ID));
                        }
                        FCommon.list_setValue(drpOVC_BUD_CURRENT, "N");
                        FCommon.list_setValue(drpOVC_CURRENT, "N");
                        #endregion

                        string strOVC_PURCH = Request.QueryString["OVC_PURCH"];
                        string strOVC_PURCH_5 = Request.QueryString["OVC_PURCH_5"];
                        panCashD.Visible = true;
                        panCash.Visible = false;
                        panDoc.Visible = false;
                        panDocD.Visible = true;
                        panSec.Visible = false;
                        panSecD.Visible = true;

                        ViewState["strOVC_PURCH"] = strOVC_PURCH;
                        ViewState["strOVC_PURCH_5"] = strOVC_PURCH_5;
                        string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH));
                        string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH_5));
                        sOVC_PURCH = strOVC_PURCH;
                        sOVC_PURCH_5 = strOVC_PURCH_5;
                        dataCImport(strOVC_PURCH, strOVC_PURCH_5);
                        dataDImport(strOVC_PURCH, strOVC_PURCH_5);
                        dataSImport(strOVC_PURCH, strOVC_PURCH_5);
                        dataImport(strOVC_PURCH, strOVC_PURCH_5);

                        FCommon.Controls_Attributes("readonly", "true", txtEscOVC_DRECEIVE, txtEscOVC_DCOMPTROLLER, txtEscOVC_DAPPROVE, txtDocExpDate_1, txtOVC_DOPEN
                            , txtDocExpDate_2, txtDocExpDate_3, txtDocOVC_DRECEIVE, txtDocOVC_DCOMPTROLLER, txtDocOVC_DAPPROVE, txtOVC_DAPPROVE, txtOVC_DCONTRACT,
                            txtSecOVC_DRECEIVE, txtSecOVC_DCOMPTROLLER, txtSecOVC_DAPPROVE, txtSecOVC_WORK_UNIT, txtDocOVC_WORK_UNIT, txtEscOVC_WORK_UNIT, txtOVC_DBID,
                            txtOVC_PURCH_6, txtONB_GROUP);

                        TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)).FirstOrDefault();
                        if (tbmRECEIVE_BID != null)
                            ViewState["OVC_DO_NAME"] = tbmRECEIVE_BID.OVC_DO_NAME;
                        
                        var query1407 =
                            (from tbm1407 in mpms.TBM1407
                             where tbm1407.OVC_PHR_CATE == "GB"
                             select tbm1407).ToList();
                        DataTable dt = CommonStatic.ListToDataTable(query1407);
                        FCommon.list_dataImport(drpEscOVC_ONNAME, dt, "OVC_PHR_DESC", "OVC_PHR_ID", true);
                        FCommon.list_dataImport(drpDocOVC_ONNAME, dt, "OVC_PHR_DESC", "OVC_PHR_ID", true);
                        FCommon.list_dataImport(drpSecOVC_ONNAME, dt, "OVC_PHR_DESC", "OVC_PHR_ID", true);

                        var query1407B0 =
                            (from tbm1407 in mpms.TBM1407
                             where tbm1407.OVC_PHR_CATE == "B0"
                             select tbm1407).ToList();
                        DataTable dtB0 = CommonStatic.ListToDataTable(query1407B0);
                        FCommon.list_dataImport(drpEscOVC_CURRENT_1, dtB0, "OVC_PHR_DESC", "OVC_PHR_ID", false);
                        FCommon.list_dataImport(drpEscOVC_CURRENT_2, dtB0, "OVC_PHR_DESC", "OVC_PHR_ID", false);
                        FCommon.list_dataImport(drpEscOVC_CURRENT_3, dtB0, "OVC_PHR_DESC", "OVC_PHR_ID", false);
                        drpEscOVC_CURRENT_1.SelectedValue = "N";
                        drpEscOVC_CURRENT_2.SelectedValue = "N";
                        drpEscOVC_CURRENT_3.SelectedValue = "N";
                        /*drpEscOVC_CURRENT_1.SelectedItem.Text = "新臺幣";
                        drpEscOVC_CURRENT_2.SelectedItem.Text = "新臺幣";
                        drpEscOVC_CURRENT_3.SelectedItem.Text = "新臺幣";*/

                        txtEscOVC_MARK.Text = "保證(固)期間自XX年XX月XX日至合約履行完為止。";

                        GvFilesImport();

                        var tbmstatus = mpms.TBMSTATUS
                        .Where(o => o.OVC_PURCH.Equals(strOVC_PURCH) && o.OVC_PURCH_5.Equals(strOVC_PURCH_5))
                        .Where(o => o.OVC_STATUS.Equals("27")).FirstOrDefault();
                        if (tbmstatus != null)
                            txtOVC_DAPPROVE.Text = tbmstatus.OVC_DEND != null ? tbmstatus.OVC_DEND : "";

                        var tbm1302 = mpms.TBM1302.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH) && o.OVC_PURCH_5.Equals(strOVC_PURCH_5)).FirstOrDefault();
                        if (tbm1302 != null)
                            txtOVC_DCONTRACT.Text = tbm1302.OVC_DCONTRACT;
                        dataImport();
                    }
                }
            }
        }

        #region gridview event
        protected void GV_TBMMANAGE_CASH_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void GV_TBMMANAGE_CASH_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBMMANAGE_CASH.DataKeys[gvrIndex].Value.ToString();
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            switch (e.CommandName)
            {
                case "btnwork":
                    panCashD.Visible = false;
                    panCash.Visible = true;
                    ViewState["type"] = "cmodify";
                    ViewState["strOVC_PURCH_6_C"] = id;
                    dataEditC(strOVC_PURCH, strOVC_PURCH_5, id,"");
                    break;

                case "btnDel":
                    var querycash =
                     (from tbmcash in mpms.TBMMANAGE_CASH
                      where tbmcash.OVC_PURCH == strOVC_PURCH
                      where tbmcash.OVC_PURCH_5 == strOVC_PURCH_5
                      where tbmcash.OVC_PURCH_6 == id
                      select tbmcash).FirstOrDefault();
                    mpms.Entry(querycash).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), querycash.GetType().Name.ToString(), this, "刪除");
                    FCommon.AlertShow(PnMessageC, "success", "系統訊息", "刪除成功");
                    dataCImport(strOVC_PURCH, strOVC_PURCH_5);
                    break;

                case "Copy":
                    string copyID = ((TextBox)GV_TBMMANAGE_CASH.Rows[gvrIndex].FindControl("txtCopy")).Text;
                    if (copyID == "")
                        FCommon.AlertShow(PnMessageC, "danger", "系統訊息", "請輸入項次數字");
                    else if (int.Parse(copyID) <= 0 || int.Parse(copyID) > GV_TBMMANAGE_CASH.Rows.Count)
                        FCommon.AlertShow(PnMessageC, "danger", "系統訊息", "無此項次");
                    else
                    {
                        panCashD.Visible = false;
                        panCash.Visible = true;
                        ViewState["type"] = "cmodify";
                        ViewState["strOVC_PURCH_6_C"] = id;
                        copyID = GV_TBMMANAGE_CASH.DataKeys[int.Parse(copyID) - 1].Value.ToString();
                        dataEditC(strOVC_PURCH, strOVC_PURCH_5, id, copyID);
                    }
                    break;
                default:
                    break;
            }
        }

        protected void GV_TBMMANAGE_PROM_PreRender(object sender, EventArgs e)
        {
            bool hasRows2 = false;
            if (ViewState["hasRows2"] != null)
                hasRows2 = Convert.ToBoolean(ViewState["hasRows2"]);
            FCommon.GridView_PreRenderInit(sender, hasRows2);
        }

        protected void GV_TBMMANAGE_PROM_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBMMANAGE_PROM.DataKeys[gvrIndex].Value.ToString();
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            switch (e.CommandName)
            {
                case "btnwork":
                    panDocD.Visible = false;
                    panDoc.Visible = true;
                    ViewState["type"] = "dmodify";
                    ViewState["strOVC_PURCH_6_D"] = id;
                    dataEditD(strOVC_PURCH, strOVC_PURCH_5, id,"");
                    break;

                case "btnDel":
                    var queryprom =
                     (from tbmprom in mpms.TBMMANAGE_PROM
                      where tbmprom.OVC_PURCH == strOVC_PURCH
                      where tbmprom.OVC_PURCH_5 == strOVC_PURCH_5
                      where tbmprom.OVC_PURCH_6 == id
                      select tbmprom).FirstOrDefault();
                    mpms.Entry(queryprom).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), queryprom.GetType().Name.ToString(), this, "刪除");
                    FCommon.AlertShow(PnMessageD, "success", "系統訊息", "刪除成功");
                    dataDImport(strOVC_PURCH, strOVC_PURCH_5);
                    break;

                case "Copy":
                    string copyID = ((TextBox)GV_TBMMANAGE_PROM.Rows[gvrIndex].FindControl("txtCopy")).Text;
                    if (copyID == "")
                        FCommon.AlertShow(PnMessageD, "danger", "系統訊息", "請輸入項次數字");
                    else if (int.Parse(copyID) <= 0 || int.Parse(copyID) > GV_TBMMANAGE_PROM.Rows.Count)
                        FCommon.AlertShow(PnMessageD, "danger", "系統訊息", "無此項次");
                    else
                    {
                        panDocD.Visible = false;
                        panDoc.Visible = true;
                        ViewState["type"] = "dmodify";
                        ViewState["strOVC_PURCH_6_D"] = id;
                        copyID = GV_TBMMANAGE_PROM.DataKeys[int.Parse(copyID) - 1].Value.ToString();
                        dataEditD(strOVC_PURCH, strOVC_PURCH_5, id, copyID);
                    }
                    break;

                default:
                    break;
            }
        }

        protected void GV_TBMMANAGE_STOCK_PreRender(object sender, EventArgs e)
        {
            bool hasRows3 = false;
            if (ViewState["hasRows3"] != null)
                hasRows3 = Convert.ToBoolean(ViewState["hasRows3"]);
            FCommon.GridView_PreRenderInit(sender, hasRows3);
        }

        protected void GV_TBMMANAGE_STOCK_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBMMANAGE_STOCK.DataKeys[gvrIndex].Value.ToString();
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            switch (e.CommandName)
            {
                case "btnwork":
                    panSecD.Visible = false;
                    panSec.Visible = true;
                    ViewState["type"] = "smodify";
                    ViewState["strOVC_PURCH_6_S"] = id;
                    dataEditS(strOVC_PURCH, strOVC_PURCH_5, id,"");
                    break;

                case "btnDel":
                    var querysec =
                     (from tbmsec in mpms.TBMMANAGE_STOCK
                      where tbmsec.OVC_PURCH == strOVC_PURCH
                      where tbmsec.OVC_PURCH_5 == strOVC_PURCH_5
                      where tbmsec.OVC_PURCH_6 == id
                      select tbmsec).FirstOrDefault();
                    mpms.Entry(querysec).State = EntityState.Deleted;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), querysec.GetType().Name.ToString(), this, "刪除");
                    FCommon.AlertShow(PnMessageS, "success", "系統訊息", "刪除成功");
                    dataSImport(strOVC_PURCH, strOVC_PURCH_5);
                    break;

                case "Copy":
                    string copyID = ((TextBox)GV_TBMMANAGE_STOCK.Rows[gvrIndex].FindControl("txtCopy")).Text;
                    if (copyID == "")
                        FCommon.AlertShow(PnMessageS, "danger", "系統訊息", "請輸入項次數字");
                    else if (int.Parse(copyID) <= 0 || int.Parse(copyID) > GV_TBMMANAGE_STOCK.Rows.Count)
                        FCommon.AlertShow(PnMessageS, "danger", "系統訊息", "無此項次");
                    else
                    {
                        panSecD.Visible = false;
                        panSec.Visible = true;
                        ViewState["type"] = "smodify";
                        ViewState["strOVC_PURCH_6_S"] = id;
                        copyID = GV_TBMMANAGE_STOCK.DataKeys[int.Parse(copyID) - 1].Value.ToString();
                        dataEditS(strOVC_PURCH, strOVC_PURCH_5, id,copyID);
                    }
                    break;

                default:
                    break;
            }
        }

        protected void GV_TBM1302_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows4"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows4"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void GV_TBM1302_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBM1302.DataKeys[gvrIndex].Value.ToString();
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strOVC_VEN_TITLE = gvr.Cells[3].Text;
            short shrONB_GROUP = short.Parse(gvr.Cells[2].Text);
            var purch = ViewState["strOVC_PURCH"].ToString();
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            string filepath, fileName;
            switch (e.CommandName)
            {
                case "btnComMod":
                    PnCompany.Visible = true;
                    lblOVC_PURCH.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5 + id;
                    lblOVC_VEN_TITLE.Text = strOVC_VEN_TITLE;
                    lblONB_GROUP.Text = shrONB_GROUP.ToString();
                    lblOVC_PURCH_1.Text = strOVC_PURCH;
                    txtOVC_PURCH_6.Text = id;
                    txtONB_GROUP.Text = shrONB_GROUP.ToString();
                    lblOVC_PUR_IPURCH.Text = query1301.OVC_PUR_IPURCH;
                    var query1302 =
                        (from tbm1302 in mpms.TBM1302
                         where tbm1302.OVC_PURCH.Equals(strOVC_PURCH)
                         where tbm1302.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                         where tbm1302.OVC_PURCH_6.Equals(id)
                         where tbm1302.OVC_VEN_TITLE.Equals(strOVC_VEN_TITLE)
                         select tbm1302).FirstOrDefault();
                    if (query1302 != null)
                    {
                        if (query1302.OVC_BUD_CURRENT != null) drpOVC_BUD_CURRENT.Text = query1302.OVC_BUD_CURRENT;
                        txtONB_BUD_MONEY.Text = query1302.ONB_BUD_MONEY == null ? "" : query1302.ONB_BUD_MONEY.ToString();
                        txtOVC_DBID.Text = query1302.OVC_DBID;
                        txtOVC_DCONTRACT.Text = query1302.OVC_DCONTRACT;
                        txtOVC_DOPEN.Text = query1302.OVC_DOPEN;
                        txtONB_MONEY_DISCOUNT.Text = query1302.ONB_MONEY_DISCOUNT == null ? "" : query1302.ONB_MONEY_DISCOUNT.ToString();
                        if (query1302.OVC_CURRENT != null) drpOVC_CURRENT.Text = query1302.OVC_CURRENT;
                        txtONB_MONEY.Text = query1302.ONB_MONEY == null ? "" : query1302.ONB_MONEY.ToString();
                        txtOVC_SHIP_TIMES.Text = query1302.OVC_SHIP_TIMES;
                        txtOVC_RECEIVE_PLACE.Text = query1302.OVC_RECEIVE_PLACE;
                        txtONB_DELIVERY_TIMES.Text = query1302.ONB_DELIVERY_TIMES == null ? "" : query1302.ONB_DELIVERY_TIMES.ToString();
                        txtOVC_PAYMENT.Text = query1302.OVC_PAYMENT;
                        txtOVC_VEN_TITLE.Text = query1302.OVC_VEN_TITLE;
                        txtOVC_VEN_CST.Text = query1302.OVC_VEN_CST;
                        txtOVC_VEN_EMAIL.Text = query1302.OVC_VEN_EMAIL;
                        txtOVC_VEN_FAX.Text = query1302.OVC_VEN_FAX;
                        txtOVC_VEN_TEL.Text = query1302.OVC_VEN_TEL;
                        txtOVC_VEN_BOSS.Text = query1302.OVC_VEN_BOSS;
                        txtOVC_VEN_CELLPHONE.Text = query1302.OVC_VEN_CELLPHONE;
                        txtOVC_VEN_NAME.Text = query1302.OVC_NAME;
                        txtOVC_VEN_ADDRESS.Text = query1302.OVC_VEN_ADDRESS;
                        txtOVC_CONTRACT_COMM.Text = query1302.OVC_CONTRACT_COMM;
                    }
                    break;
                case "lbtnOffice":
                    string path_temp = lbtnOffice(id, shrONB_GROUP, strOVC_VEN_TITLE);
                    FileInfo file = new FileInfo(path_temp);
                    filepath = purch + "發包處通知單.docx";
                    fileName = HttpUtility.UrlEncode(filepath);
                    Response.Clear();
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                    Response.ContentType = "application/octet-stream";
                    Response.WriteFile(file.FullName);
                    Response.OutputStream.Flush();
                    Response.OutputStream.Close();
                    Response.Flush();
                    File.Delete(path_temp);
                    Response.End();
                    break;
                case "lbtnOffice_odt":
                    string path_temp_odt = lbtnOffice(id, shrONB_GROUP, strOVC_VEN_TITLE);
                    filepath = Request.PhysicalApplicationPath + "WordPDFprint/lbtnOffice_odt.odt";
                    fileName = purch + "發包處通知單.odt";
                    FCommon.WordToOdt(this, path_temp_odt, filepath, fileName);
                    break;
                case "lbtnAttachment":
                    string path_temp2 = lbtnAttachment(id, shrONB_GROUP, strOVC_VEN_TITLE);
                    FileInfo file2 = new FileInfo(path_temp2);
                    filepath = purch + "契約及附件分配表.docx";
                    fileName = HttpUtility.UrlEncode(filepath);
                    Response.Clear();
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                    Response.ContentType = "application/octet-stream";
                    Response.WriteFile(file2.FullName);
                    Response.OutputStream.Flush();
                    Response.OutputStream.Close();
                    Response.Flush();
                    File.Delete(path_temp2);
                    Response.End();
                    break;
                case "lbtnAttachment_odt":
                    string path_temp_odt2 = lbtnAttachment(id, shrONB_GROUP, strOVC_VEN_TITLE);
                    filepath = Request.PhysicalApplicationPath + "WordPDFprint/lbtnAttachment_odt.odt";
                    fileName = purch + "契約及附件分配表.odt";
                    FCommon.WordToOdt(this, path_temp_odt2, filepath, fileName);
                    break;
                case "btnChk":
                    string strONB_GROUP = shrONB_GROUP.ToString();
                    string key = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH));
                    string key2 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strOVC_PURCH_5));
                    string key3 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(strONB_GROUP));
                    string key4 = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(id));
                    ViewState["strOVC_PURCH"] = key;
                    ViewState["strOVC_PURCH_5"] = key2;
                    ViewState["strONB_GROUP"] = key3;
                    ViewState["strOVC_PURCH_6"] = key4;
                    Response.Redirect("MPMS_D22.aspx?OVC_PURCH=" + ViewState["strOVC_PURCH"].ToString() + "&OVC_PURCH_5=" + ViewState["strOVC_PURCH_5"].ToString() + "&OVC_PURCH_6=" + ViewState["strOVC_PURCH_6"].ToString() + "&ONB_GROUP=" + ViewState["strONB_GROUP"].ToString());
                    break;
            }
        }
        #endregion

        #region click

        protected void btnContract_Click(object sender, EventArgs e)
        {
            PnContract.Visible = true;
            PnEscrow.Visible = false;
            lblTitle.Text = "簽約";
        }

        protected void btnEscrow_Click(object sender, EventArgs e)
        {
            PnContract.Visible = false;
            PnEscrow.Visible = true;
            lblTitle.Text = "保證金(函)收繳";
        }
        protected void btnNewC_Click(object sender, EventArgs e)
        {
            panCash.Visible = true;
            panCashD.Visible = false;
            ViewState["type"] = "cnew";
            txtEscOVC_PURCH_6.Visible = true;
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            lblEscOVC_PURCH_6.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
            string ThisYear = (int.Parse(DateTime.Now.Year.ToString()) - 1912).ToString().Substring(1);
            string y = ThisYear.CompareTo(strOVC_PURCH.Substring(2, 2)) >= 0 ? "1" + strOVC_PURCH.Substring(2, 2) : strOVC_PURCH.Substring(2, 2);
            string p = query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
            txtEscOVC_RECEIVE_NO.Text = "(" + y + ")履繳字第" + p + "號";
        }

        protected void btnSaveC_Click(object sender, EventArgs e)
        {
            string type = ViewState["type"].ToString();
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            if (type == "cmodify")
                dataModifyC(strOVC_PURCH, strOVC_PURCH_5);
            if (type == "cnew")
                dataNewC(strOVC_PURCH, strOVC_PURCH_5);
        }

        protected void btnToC_Click(object sender, EventArgs e)
        {
            foreach (Control item in Page.Form.FindControl("MainContent").FindControl("panCash").Controls)
            {
                if (item is TextBox)
                {
                    ((TextBox)item).Text = string.Empty;
                }
            }
            panCash.Visible = false;
            panCashD.Visible = true;
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            dataCImport(strOVC_PURCH, strOVC_PURCH_5);
            drpEscOVC_CURRENT_1.SelectedValue = "N";
            drpEscOVC_CURRENT_2.SelectedValue = "N";
            drpEscOVC_CURRENT_3.SelectedValue = "N";
            drpEscOVC_ONNAME.SelectedItem.Text = "請選擇";
            txtEscOVC_WORK_UNIT.Text = "採購發包處";
        }

        protected void btnSaveD_Click(object sender, EventArgs e)
        {
            string type = ViewState["type"].ToString();
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            if (type == "dmodify")
                dataModifyD(strOVC_PURCH, strOVC_PURCH_5);
            if (type == "dnew")
                dataNewD(strOVC_PURCH, strOVC_PURCH_5);
        }

        protected void btnToD_Click(object sender, EventArgs e)
        {
            foreach (Control item in Page.Form.FindControl("MainContent").FindControl("panDoc").Controls)
            {
                if (item is TextBox)
                {
                    ((TextBox)item).Text = string.Empty;
                }
            }
            panDoc.Visible = false;
            panDocD.Visible = true;
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            dataDImport(strOVC_PURCH, strOVC_PURCH_5);
            drpDocOVC_ONNAME.SelectedItem.Text = "請選擇";
            txtDocOVC_WORK_UNIT.Text = "採購發包處";
        }

        protected void btnNewD_Click(object sender, EventArgs e)
        {
            panDoc.Visible = true;
            panDocD.Visible = false;
            ViewState["type"] = "dnew";
            txtDocOVC_PURCH_6.Visible = true;
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            //string strOVC_PURCH_6 = ViewState["strOVC_PURCH_6_C"].ToString();
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            lblDocOVC_PURCH_6.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;

        }

        protected void btnNewS_Click(object sender, EventArgs e)
        {
            panSec.Visible = true;
            panSecD.Visible = false;
            ViewState["type"] = "snew";
            txtSecOVC_PURCH_6.Visible = true;
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            lblSecOVC_PURCH_6.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
            
        }

        protected void btnSaveS_Click(object sender, EventArgs e)
        {
            string type = ViewState["type"].ToString();
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            if (type == "smodify")
                dataModifyS(strOVC_PURCH, strOVC_PURCH_5);
            if (type == "snew")
                dataNewS(strOVC_PURCH, strOVC_PURCH_5);
        }

        protected void btnToS_Click(object sender, EventArgs e)
        {
            foreach (Control item in Page.Form.FindControl("MainContent").FindControl("panSec").Controls)
            {
                if (item is TextBox)
                {
                    ((TextBox)item).Text = string.Empty;
                }
            }
            panSec.Visible = false;
            panSecD.Visible = true;
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            dataSImport(strOVC_PURCH, strOVC_PURCH_5);
            drpSecOVC_ONNAME.SelectedItem.Text = "請選擇";
            txtSecOVC_WORK_UNIT.Text = "採購發包處";
        }

        protected void btnMain_Click(object sender, EventArgs e)
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            Response.Redirect("MPMS_D14_1.aspx?OVC_PURCH="+ strOVC_PURCH);
        }
        
        protected void lnkC_Click(object sender, EventArgs e)
        {
            var purch = lblEscOVC_PURCH_6.Text.Substring(0, 7);
            string path_d = Cash_Income_ExportToWord();
            FileInfo file = new FileInfo(path_d);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + purch + "收入通知單.docx");
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            Response.End();
        }
        protected void lnkC_odt_Click(object sender, EventArgs e)
        {
            var purch = lblEscOVC_PURCH_6.Text.Substring(0, 7);
            string path_d = Cash_Income_ExportToWord();
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/lnkC_odt.odt";
            string FileName = purch + "收入通知單.odt";
            FCommon.WordToOdt(this, path_d, filetemp, FileName);
        }

        protected void lnkD_Click(object sender, EventArgs e)
        {
            var purch = lblDocOVC_PURCH_6.Text.Substring(0, 7);
            string path_d = Prom_Income_ExportToWord();
            FileInfo file = new FileInfo(path_d);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + purch + "收入通知單.docx");
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            Response.End();
        }
        protected void lnkD_odt_Click(object sender, EventArgs e)
        {
            var purch = lblDocOVC_PURCH_6.Text.Substring(0, 7);
            string path_d = Prom_Income_ExportToWord();
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/lnkD_odt.odt";
            string FileName = purch + "收入通知單.odt";
            FCommon.WordToOdt(this, path_d, filetemp, FileName);
        }

        protected void lnkS_Click(object sender, EventArgs e)
        {
            var purch = lblSecOVC_PURCH_6.Text.Substring(0, 7);
            string path_d = Stock_Income_ExportToWord();
            FileInfo file = new FileInfo(path_d);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + purch + "收入通知單.docx");
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            Response.End();
        }
        protected void lnkS_odt_Click(object sender, EventArgs e)
        {
            var purch = lblSecOVC_PURCH_6.Text.Substring(0, 7);
            string path_d = Stock_Income_ExportToWord();
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/lnkS_odt.odt";
            string FileName = purch + "收入通知單.odt";
            FCommon.WordToOdt(this, path_d, filetemp, FileName);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            var queryPurch_6 = mpms.TBM1302.Where(t => t.OVC_PURCH.Equals(strOVC_PURCH) && t.OVC_PURCH_5.Equals(strOVC_PURCH_5)).Select(t => t.OVC_PURCH_6).FirstOrDefault();

            var querysta =
                (from tsta in mpms.TBMSTATUS
                 where tsta.OVC_PURCH == strOVC_PURCH
                 where tsta.OVC_PURCH_5 == strOVC_PURCH_5
                 where tsta.OVC_STATUS == "27"
                 select tsta).FirstOrDefault();
            if (!txtOVC_DAPPROVE.Text.Equals(string.Empty))
            {
                querysta.OVC_DEND = txtOVC_DAPPROVE.Text;
                mpms.SaveChanges();
            }

            var querystaa =
                (from tsta in mpms.TBMSTATUS
                 where tsta.OVC_PURCH == strOVC_PURCH
                 where tsta.OVC_PURCH_5 == strOVC_PURCH_5
                 where tsta.OVC_STATUS == "28"
                 select tsta).FirstOrDefault();
            if (querystaa == null && !txtOVC_DAPPROVE.Text.Equals(string.Empty))
            {
                TBMSTATUS sta = new TBMSTATUS();
                sta.OVC_PURCH = strOVC_PURCH;
                sta.OVC_PURCH_5 = strOVC_PURCH_5;
                sta.OVC_PURCH_6 = querysta.OVC_PURCH_6;
                sta.ONB_TIMES = 1;
                sta.OVC_DO_NAME = ViewState["OVC_DO_NAME"].ToString();
                sta.OVC_STATUS = "28";
                sta.OVC_DBEGIN = txtOVC_DAPPROVE.Text;
                sta.OVC_STATUS_SN = Guid.NewGuid();
                mpms.TBMSTATUS.Add(sta);
                mpms.SaveChanges();
            }
            else if (!txtOVC_DAPPROVE.Text.Equals(string.Empty))
            {
                querystaa.OVC_PURCH_6 = querysta.OVC_PURCH_6;
                querystaa.OVC_DBEGIN = txtOVC_DAPPROVE.Text;
                querystaa.OVC_DEND = txtOVC_DAPPROVE.Text;
                mpms.SaveChanges();
            }
            var query3 = mpms.TBMSTATUS.Where(t => t.OVC_PURCH.Equals(strOVC_PURCH) && t.OVC_PURCH_5.Equals(strOVC_PURCH_5) && t.OVC_STATUS.Equals("3")).FirstOrDefault();
            if (query3 == null)
            {
                TBMSTATUS status3 = new TBMSTATUS();
                status3.OVC_PURCH = strOVC_PURCH;
                status3.OVC_PURCH_5 = strOVC_PURCH_5;
                status3.OVC_PURCH_6 = querysta.OVC_PURCH_6 ?? queryPurch_6;
                status3.ONB_TIMES = 1;
                status3.OVC_DO_NAME = ViewState["OVC_DO_NAME"].ToString();
                status3.OVC_STATUS = "3";
                status3.OVC_DBEGIN = txtOVC_DAPPROVE.Text;
                status3.OVC_STATUS_SN = Guid.NewGuid();
                mpms.TBMSTATUS.Add(status3);
                mpms.SaveChanges();
            }
            else
            {
                query3.OVC_DBEGIN = txtOVC_DAPPROVE.Text;
                mpms.SaveChanges();
            }

            var query1302 =
                from t1302 in mpms.TBM1302
                where t1302.OVC_PURCH.Equals(strOVC_PURCH)
                where t1302.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                select t1302;
            foreach (var q in query1302)
            {
                TBM1302 tbm1302 = q;
                tbm1302.OVC_DSEND = txtOVC_DAPPROVE.Text;
                mpms.SaveChanges();
            }
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功！");
        }

        protected void btnSaveCONTRACT_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            string strCompany = lblOVC_VEN_TITLE.Text;
            string strOVC_BUD_CURRENT = drpOVC_BUD_CURRENT.SelectedValue;
            string strONB_BUD_MONEY = txtONB_BUD_MONEY.Text;
            string strOVC_DBID = txtOVC_DBID.Text;
            string strOVC_DCONTRACT = txtOVC_DCONTRACT.Text;
            string strOVC_DOPEN = txtOVC_DOPEN.Text;
            string strONB_MONEY_DISCOUNT = txtONB_MONEY_DISCOUNT.Text;
            string strOVC_CURRENT = drpOVC_CURRENT.SelectedValue;
            string strONB_MONEY = txtONB_MONEY.Text;
            string strOVC_SHIP_TIMES = txtOVC_SHIP_TIMES.Text;
            string strOVC_RECEIVE_PLACE = txtOVC_RECEIVE_PLACE.Text;
            string strONB_DELIVERY_TIMES = txtONB_DELIVERY_TIMES.Text;
            string strOVC_PAYMENT = txtOVC_PAYMENT.Text;
            string strOVC_VEN_TITLE = txtOVC_VEN_TITLE.Text;
            string strOVC_VEN_CST = txtOVC_VEN_CST.Text;
            string strOVC_VEN_EMAIL = txtOVC_VEN_EMAIL.Text;
            string strOVC_VEN_FAX = txtOVC_VEN_FAX.Text;
            string strOVC_VEN_TEL = txtOVC_VEN_TEL.Text;
            string strOVC_VEN_BOSS = txtOVC_VEN_BOSS.Text;
            string strOVC_VEN_CELLPHONE = txtOVC_VEN_CELLPHONE.Text;
            string strOVC_VEN_NAME = txtOVC_VEN_NAME.Text;
            string strOVC_VEN_ADDRESS = txtOVC_VEN_ADDRESS.Text;
            string strOVC_CONTRACT_COMM = txtOVC_CONTRACT_COMM.Text;

            if (string.Empty.Equals(strOVC_VEN_CST))
            {
                strMessage += "<p> 請輸入 得標商統一編號 </p>";
            }

            bool boolONB_BUD_MONEY = FCommon.checkDecimal(strONB_BUD_MONEY, "預算金額", ref strMessage, out decimal decONB_BUD_MONEY);
            bool boolONB_MONEY_DISCOUNT = FCommon.checkDecimal(strONB_MONEY_DISCOUNT, "折讓金額", ref strMessage, out decimal decONB_MONEY_DISCOUNT);
            bool boolONB_MONEY = FCommon.checkDecimal(strONB_MONEY, "契約金額", ref strMessage, out decimal decONB_MONEY);
            bool isShort = short.TryParse(strONB_DELIVERY_TIMES, out short shoONB_DELIVERY_TIMES);
            if (!string.IsNullOrEmpty(strONB_DELIVERY_TIMES) && !isShort)
                strMessage += "<p> 交貨批次 須為數字！ </p>";

            if (string.Empty.Equals(strMessage))
            {
                var query =
                    from t1302 in mpms.TBM1302
                    where t1302.OVC_PURCH.Equals(strOVC_PURCH)
                    where t1302.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                    select t1302.OVC_VEN_TITLE;
                foreach (var q in query)
                {
                    TBM1302 tbm1302 = mpms.TBM1302
                        .Where(o => o.OVC_PURCH.Equals(strOVC_PURCH) && o.OVC_PURCH_5.Equals(strOVC_PURCH_5))
                        .Where(o => o.OVC_VEN_TITLE.Equals(q)).FirstOrDefault();
                    if (tbm1302 != null)
                    {
                        tbm1302.OVC_BUD_CURRENT = strOVC_BUD_CURRENT;
                        if (boolONB_BUD_MONEY) tbm1302.ONB_BUD_MONEY = decONB_BUD_MONEY; else tbm1302.ONB_BUD_MONEY = null;
                        tbm1302.OVC_DBID = strOVC_DBID;
                        tbm1302.OVC_DCONTRACT = strOVC_DCONTRACT;
                        tbm1302.OVC_DOPEN = strOVC_DOPEN;
                        if (boolONB_MONEY_DISCOUNT) tbm1302.ONB_MONEY_DISCOUNT = decONB_MONEY_DISCOUNT; else tbm1302.ONB_MONEY_DISCOUNT = null;
                        tbm1302.OVC_CURRENT = strOVC_CURRENT;
                        if (boolONB_MONEY) tbm1302.ONB_MONEY = decONB_MONEY; else tbm1302.ONB_MONEY = null;
                        tbm1302.OVC_SHIP_TIMES = strOVC_SHIP_TIMES;
                        tbm1302.OVC_RECEIVE_PLACE = strOVC_RECEIVE_PLACE;
                        if (isShort) tbm1302.ONB_DELIVERY_TIMES = shoONB_DELIVERY_TIMES; else tbm1302.ONB_DELIVERY_TIMES = null;
                        tbm1302.OVC_PAYMENT = strOVC_PAYMENT;
                        if (tbm1302.OVC_VEN_TITLE == strCompany)
                        {
                            tbm1302.OVC_VEN_TITLE = strOVC_VEN_TITLE;
                            tbm1302.OVC_VEN_CST = strOVC_VEN_CST;
                            tbm1302.OVC_VEN_EMAIL = strOVC_VEN_EMAIL;
                            tbm1302.OVC_VEN_FAX = strOVC_VEN_FAX;
                            tbm1302.OVC_VEN_TEL = strOVC_VEN_TEL;
                            tbm1302.OVC_VEN_BOSS = strOVC_VEN_BOSS;
                            tbm1302.OVC_VEN_CELLPHONE = strOVC_VEN_CELLPHONE;
                            tbm1302.OVC_VEN_NAME = strOVC_VEN_NAME;
                            tbm1302.OVC_VEN_ADDRESS = strOVC_VEN_ADDRESS;
                            tbm1302.OVC_CONTRACT_COMM = strOVC_CONTRACT_COMM;
                        }
                        mpms.SaveChanges();
                    }
                }
                FCommon.AlertShow(PnMessageC, "success", "系統訊息", "合約儲存成功!!");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            //if (!query.Any())
            //    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無合約項目");
        }
        #endregion

        #region 副程式

        private void dataImport()
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
            //query1302 = 1301_plan join 1302 的ovc_purch
            var query1302 =
                (from tbm1301 in mpms.TBM1301_PLAN
                 join tbm1302 in mpms.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                 where tbm1301.OVC_PURCH.Equals(strOVC_PURCH)
                 where tbm1302.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                 select tbm1302).FirstOrDefault();
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();

            if (query1302 != null)
            {
                lblOVC_PURCH_1.Text = strOVC_PURCH;
                txtOVC_PURCH_6.Text = query1302.OVC_PURCH_6;
                txtONB_GROUP.Text = query1302.ONB_GROUP == null ? "" : query1302.ONB_GROUP.ToString();
                if (query1302.OVC_BUD_CURRENT != null)
                    drpOVC_BUD_CURRENT.Text = query1302.OVC_BUD_CURRENT;
                txtONB_BUD_MONEY.Text = query1302.ONB_BUD_MONEY == null ? "" : query1302.ONB_BUD_MONEY.ToString();
                txtOVC_DBID.Text = query1302.OVC_DBID;
                txtOVC_DCONTRACT.Text = query1302.OVC_DCONTRACT;
                txtOVC_DOPEN.Text = query1302.OVC_DOPEN;
                txtONB_MONEY_DISCOUNT.Text = query1302.ONB_MONEY_DISCOUNT == null ? "" : query1302.ONB_MONEY_DISCOUNT.ToString();
                if (query1302.OVC_CURRENT != null)
                    drpOVC_CURRENT.Text = query1302.OVC_CURRENT;
                txtONB_MONEY.Text = query1302.ONB_MONEY == null ? "" : query1302.ONB_MONEY.ToString();
                txtOVC_SHIP_TIMES.Text = query1302.OVC_SHIP_TIMES;
                txtOVC_RECEIVE_PLACE.Text = query1302.OVC_RECEIVE_PLACE;
                txtONB_DELIVERY_TIMES.Text = query1302.ONB_DELIVERY_TIMES == null ? "" : query1302.ONB_DELIVERY_TIMES.ToString();
                txtOVC_PAYMENT.Text = query1302.OVC_PAYMENT;
            }
            lblOVC_PUR_IPURCH.Text = query1301 == null ? "" : query1301.OVC_PUR_IPURCH;

        }

        private bool IsOVC_DO_NAME()
        {
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strUserName, strDept = "";
            string strOVC_PURCH = Request.QueryString["OVC_PURCH"];
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


        #region dataImport

        private void dataCImport(string strOVC_PURCH, string strOVC_PURCH_5)
        {

            DataTable dt = new DataTable();
            var query =
                  from tbmcash in mpms.TBMMANAGE_CASH
                  where tbmcash.OVC_PURCH == strOVC_PURCH
                  where tbmcash.OVC_PURCH_5 == strOVC_PURCH_5
                  join tbm1301 in mpms.TBM1301 on tbmcash.OVC_PURCH equals tbm1301.OVC_PURCH
                  select new
                  {
                      OVC_KIND = tbmcash.OVC_KIND,
                      OVC_PURCH = tbmcash.OVC_PURCH,
                      OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                      OVC_PURCH_5 = tbmcash.OVC_PURCH_5,
                      OVC_PURCH_6 = tbmcash.OVC_PURCH_6,
                      OVC_OWN_NAME = tbmcash.OVC_OWN_NAME,
                      ONB_ALL_MONEY = tbmcash.ONB_ALL_MONEY,
                      OVC_RECEIVE_NO = tbmcash.OVC_RECEIVE_NO,
                      OVC_DRECEIVE = tbmcash.OVC_DRECEIVE,
                      OVC_DAPPROVE = tbmcash.OVC_DAPPROVE
                  };
            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBMMANAGE_CASH, dt);
        }
        private void dataDImport(string strOVC_PURCH, string strOVC_PURCH_5)
        {
            DataTable dt = new DataTable();
            var query =
                  from tbmprom in mpms.TBMMANAGE_PROM
                  where tbmprom.OVC_PURCH == strOVC_PURCH
                  where tbmprom.OVC_PURCH_5 == strOVC_PURCH_5
                  join tbm1301 in mpms.TBM1301 on tbmprom.OVC_PURCH equals tbm1301.OVC_PURCH
                  select new
                  {
                      OVC_KIND = tbmprom.OVC_KIND,
                      OVC_PURCH = tbmprom.OVC_PURCH,
                      OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                      OVC_PURCH_5 = tbmprom.OVC_PURCH_5,
                      OVC_PURCH_6 = tbmprom.OVC_PURCH_6,
                      OVC_OWN_NAME = tbmprom.OVC_OWN_NAME,
                      ONB_ALL_MONEY = tbmprom.ONB_ALL_MONEY,
                      OVC_RECEIVE_NO = tbmprom.OVC_RECEIVE_NO,
                      OVC_DRECEIVE = tbmprom.OVC_DRECEIVE,
                      OVC_DAPPROVE = tbmprom.OVC_DAPPROVE

                  };
            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows2"] = FCommon.GridView_dataImport(GV_TBMMANAGE_PROM, dt);
        }
        private void dataSImport(string strOVC_PURCH, string strOVC_PURCH_5)
        {
            DataTable dt = new DataTable();
            var query =
                  from tbmstock in mpms.TBMMANAGE_STOCK
                  where tbmstock.OVC_PURCH == strOVC_PURCH
                  where tbmstock.OVC_PURCH_5 == strOVC_PURCH_5
                  join tbm1301 in mpms.TBM1301 on tbmstock.OVC_PURCH equals tbm1301.OVC_PURCH
                  select new
                  {
                      OVC_KIND = tbmstock.OVC_KIND,
                      OVC_PURCH = tbmstock.OVC_PURCH,
                      OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                      OVC_PURCH_5 = tbmstock.OVC_PURCH_5,
                      OVC_PURCH_6 = tbmstock.OVC_PURCH_6,
                      OVC_OWN_NAME = tbmstock.OVC_OWN_NAME,
                      ONB_ALL_MONEY = tbmstock.ONB_ALL_MONEY,
                      OVC_RECEIVE_NO = tbmstock.OVC_RECEIVE_NO,
                      OVC_DRECEIVE = tbmstock.OVC_DRECEIVE,
                      OVC_DAPPROVE = tbmstock.OVC_DAPPROVE
                  };
            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows3"] = FCommon.GridView_dataImport(GV_TBMMANAGE_STOCK, dt);
        }

        private void dataImport(string strOVC_PURCH, string strOVC_PURCH_5)
        {
            DataTable dt = new DataTable();

            var query =
                from tbm1301 in mpms.TBM1301_PLAN
                join tbm1302 in mpms.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                where tbm1301.OVC_PURCH.Equals(strOVC_PURCH)
                where tbm1302.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                select new
                {
                    OVC_PURCH = tbm1301.OVC_PURCH,
                    OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                    OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                    OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                    ONB_GROUP = tbm1302.ONB_GROUP,
                    OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE
                };
            dt = CommonStatic.LinqQueryToDataTable(query);
            ViewState["hasRows4"] = FCommon.GridView_dataImport(GV_TBM1302, dt);
        }
        #endregion

        #region dataEditC

        private void dataEditC(string strOVC_PURCH, string strOVC_PURCH_5, string strOVC_PURCH_6,string copyOVC_PURCH_6)
        {
            string OVC_PURCH_6 = copyOVC_PURCH_6 == "" ? strOVC_PURCH_6 : copyOVC_PURCH_6;
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            lblEscOVC_PURCH_6.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5 + strOVC_PURCH_6;
            txtEscOVC_PURCH_6.Visible = false;
            var querycash =
                (from tbmcash in mpms.TBMMANAGE_CASH
                 where tbmcash.OVC_PURCH == strOVC_PURCH
                 where tbmcash.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmcash.OVC_PURCH_6 == OVC_PURCH_6
                 select tbmcash).FirstOrDefault();
            if (querycash != null)
            {
                txtEscOVC_OWN_NAME.Text = querycash.OVC_OWN_NAME;
                txtEscOVC_OWN_NO.Text = querycash.OVC_OWN_NO;
                txtEscOVC_OWN_ADDRESS.Text = querycash.OVC_OWN_ADDRESS;
                txtEscOVC_OWN_TEL.Text = querycash.OVC_OWN_TEL;

                //代管項目
                if (querycash.ONB_ITEM_1 != null)
                {
                    string strB01 = querycash.OVC_CURRENT_1 == null ? "" : querycash.OVC_CURRENT_1 ;
                    txtEscOVC_REASON_1.Text = querycash.OVC_REASON_1;
                    //drpEscOVC_CURRENT_1.SelectedItem.Text = query1407B01.OVC_PHR_DESC;
                    drpEscOVC_CURRENT_1.SelectedValue = strB01;
                    txtEscONB_MONEY_1.Text = querycash.ONB_MONEY_1.ToString();
                    txtEscONB_MONEY_NT_1.Text = querycash.ONB_MONEY_NT_1.ToString();
                }
                if (querycash.ONB_ITEM_2 != null)
                {
                    string strB02 = querycash.OVC_CURRENT_2 == null ? "" : querycash.OVC_CURRENT_2;
                    txtEscOVC_REASON_2.Text = querycash.OVC_REASON_2;
                    //drpEscOVC_CURRENT_2.SelectedItem.Text = query1407B02.OVC_PHR_DESC;
                    drpEscOVC_CURRENT_2.SelectedValue = strB02;
                    txtEscONB_MONEY_2.Text = querycash.ONB_MONEY_2.ToString();
                    txtEscONB_MONEY_NT_2.Text = querycash.ONB_MONEY_NT_2.ToString();
                }
                if (querycash.ONB_ITEM_3 != null)
                {
                    string strB03 = querycash.OVC_CURRENT_3 == null ? "" : querycash.OVC_CURRENT_3;
                    txtEscOVC_REASON_3.Text = querycash.OVC_REASON_3;
                    //drpEscOVC_CURRENT_3.SelectedItem.Text = query1407B03.OVC_PHR_DESC;
                    drpEscOVC_CURRENT_3.SelectedValue = strB03;
                    txtEscONB_MONEY_3.Text = querycash.ONB_MONEY_3.ToString();
                    txtEscONB_MONEY_NT_3.Text = querycash.ONB_MONEY_NT_3.ToString();
                }

                //代管項目
                txtEscONB_ALL_MONEY.Text = querycash.ONB_ALL_MONEY.ToString();
                txtEscOVC_MARK.Text = querycash.OVC_MARK;

                txtEscOVC_WORK_NO.Text = querycash.OVC_WORK_NO;
                txtEscOVC_WORK_NAME.Text = querycash.OVC_WORK_NAME;
                txtEscOVC_WORK_UNIT.Text = "採購發包處";
                if (querycash.OVC_ONNAME != null)
                    drpEscOVC_ONNAME.SelectedItem.Text = querycash.OVC_ONNAME;
                txtEscOVC_ONNAME.Text = querycash.OVC_ONNAME;
                txtEscOVC_RECEIVE_NO.Text = querycash.OVC_RECEIVE_NO;
                txtEscOVC_DRECEIVE.Text = querycash.OVC_DRECEIVE;
                txtEscOVC_COMPTROLLER_NO.Text = querycash.OVC_COMPTROLLER_NO;
                txtEscOVC_DCOMPTROLLER.Text = querycash.OVC_DCOMPTROLLER;
                txtEscOVC_DAPPROVE.Text = querycash.OVC_DAPPROVE;
            }
        }

        private void dataNewC(string strOVC_PURCH, string strOVC_PURCH_5)
        {
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();

            string strOVC_REASON_1 = txtEscOVC_REASON_1.Text;
            string strONB_MONEY_1 = txtEscONB_MONEY_1.Text;
            string strONB_MONEY_NT_1 = txtEscONB_MONEY_NT_1.Text;
            string strOVC_REASON_2 = txtEscOVC_REASON_2.Text;
            string strONB_MONEY_2 = txtEscONB_MONEY_2.Text;
            string strONB_MONEY_NT_2 = txtEscONB_MONEY_NT_2.Text;
            string strOVC_REASON_3 = txtEscOVC_REASON_3.Text;
            string strONB_MONEY_3 = txtEscONB_MONEY_3.Text;
            string strONB_MONEY_NT_3 = txtEscONB_MONEY_NT_3.Text;
            string strOVC_OWN_NO = txtEscOVC_OWN_NO.Text;
            string strOVC_WORK_NO = txtEscOVC_WORK_NO.Text;
            string strOVC_ONNAME = drpEscOVC_ONNAME.SelectedItem.Text;
            string strMessage = "";
            int onball = 0;
            string strOVC_PURCH_6 = txtEscOVC_PURCH_6.Text;
            var querycashyesno =
                (from tbmcash in mpms.TBMMANAGE_CASH
                 where tbmcash.OVC_PURCH == strOVC_PURCH
                 where tbmcash.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmcash.OVC_PURCH_6 == strOVC_PURCH_6
                 select tbmcash).FirstOrDefault();
            if (querycashyesno != null)
                strMessage += "<p> 此購案合約編號已有資料 無法新增 </p>";

            if (strOVC_PURCH_6.Equals(string.Empty))
                strMessage += "<p> 分約號欄位不得為空白 </p>";
            if (strOVC_OWN_NO.Equals(string.Empty))
                strMessage += "<p> 款項所有權人統一編號欄位不得為空白 </p>";
            if (strOVC_WORK_NO.Equals(string.Empty))
                strMessage += "<p> 承辦單位統一編號欄位不得為空白 </p>";
            if (strOVC_ONNAME == "請選擇")
                strOVC_ONNAME = "";

            if (!strOVC_REASON_1.Equals(string.Empty))
            {
                if (!strONB_MONEY_NT_1.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_NT_1, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<p> 項次1新台幣入帳金額請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<p>  項次1新台幣入帳金額欄位不得為空白 </p>";
                if (strONB_MONEY_1 != "")
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_1, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<p> 項次1原幣金額請輸入數字 </p>";
                    }
                }
            }
            if (strOVC_REASON_2 != "")
            {
                if (strONB_MONEY_NT_2 != "")
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_NT_2, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<p> 項次2新台幣入帳金額請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<p>  項次2新台幣入帳金額欄位不得為空白 </p>";
                if (strONB_MONEY_2 != "")
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_2, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<p> 項次2原幣金額請輸入數字 </p>";
                    }
                }
            }
            if (strOVC_REASON_3 != "")
            {
                if (strONB_MONEY_NT_3 != "")
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_NT_3, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<p> 項次3新台幣入帳金額請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<p>  項次3新台幣入帳金額欄位不得為空白 </p>";
                if (strONB_MONEY_3 != "")
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_3, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<p> 項次3原幣金額請輸入數字 </p>";
                    }
                }
            }
            if (strMessage == "")
            {
                TBMMANAGE_CASH querycash = new TBMMANAGE_CASH();
                querycash.OVC_CASH_SN = Guid.NewGuid();
                querycash.OVC_PURCH = strOVC_PURCH;
                querycash.OVC_PURCH_5 = strOVC_PURCH_5;
                querycash.OVC_PURCH_6 = strOVC_PURCH_6;
                querycash.OVC_OWN_NAME = txtEscOVC_OWN_NAME.Text;
                querycash.OVC_OWN_NO = txtEscOVC_OWN_NO.Text;
                querycash.OVC_OWN_ADDRESS = txtEscOVC_OWN_ADDRESS.Text;
                querycash.OVC_OWN_TEL = txtEscOVC_OWN_TEL.Text;
                if (strOVC_REASON_1 != "")
                {
                    querycash.ONB_ITEM_1 = 1;
                    querycash.OVC_REASON_1 = strOVC_REASON_1;
                    querycash.OVC_CURRENT_1 = drpEscOVC_CURRENT_1.SelectedValue.ToString();
                    if (strONB_MONEY_1 != "")
                    {
                        int onbmoney1 = Convert.ToInt32(strONB_MONEY_1);
                        querycash.ONB_MONEY_1 = onbmoney1;
                    }
                    int onbmoneynt1 = Convert.ToInt32(strONB_MONEY_NT_1);
                    querycash.ONB_MONEY_NT_1 = onbmoneynt1;
                    onball = onball + onbmoneynt1;
                }
                if (strOVC_REASON_2 != "")
                {
                    querycash.ONB_ITEM_2 = 2;
                    querycash.OVC_REASON_2 = strOVC_REASON_2;
                    querycash.OVC_CURRENT_2 = drpEscOVC_CURRENT_2.SelectedValue.ToString();
                    if (strONB_MONEY_2 != "")
                    {
                        int onbmoney2 = Convert.ToInt32(strONB_MONEY_2);
                        querycash.ONB_MONEY_2 = onbmoney2;
                    }
                    int onbmoneynt2 = Convert.ToInt32(strONB_MONEY_NT_2);
                    querycash.ONB_MONEY_NT_2 = onbmoneynt2;
                    onball = onball + onbmoneynt2;
                }
                if (strOVC_REASON_3 != "")
                {
                    querycash.ONB_ITEM_3 = 3;
                    querycash.OVC_REASON_3 = strOVC_REASON_3;
                    querycash.OVC_CURRENT_3 = drpEscOVC_CURRENT_3.SelectedValue.ToString();
                    if (strONB_MONEY_3 != "")
                    {
                        int onbmoney3 = Convert.ToInt32(strONB_MONEY_3);
                        querycash.ONB_MONEY_3 = onbmoney3;
                    }
                    int onbmoneynt3 = Convert.ToInt32(strONB_MONEY_NT_3);
                    querycash.ONB_MONEY_NT_3 = onbmoneynt3;
                    onball = onball + onbmoneynt3;
                }
                querycash.ONB_ALL_MONEY = onball;
                txtEscONB_ALL_MONEY.Text = onball.ToString();
                querycash.OVC_MARK = txtEscOVC_MARK.Text;
                querycash.OVC_WORK_NO = txtEscOVC_WORK_NO.Text;
                querycash.OVC_WORK_NAME = txtEscOVC_WORK_NAME.Text;
                querycash.OVC_WORK_UNIT = txtEscOVC_WORK_UNIT.Text;
                querycash.OVC_ONNAME = txtEscOVC_ONNAME.Text;
                querycash.OVC_RECEIVE_NO = txtEscOVC_RECEIVE_NO.Text;
                querycash.OVC_DRECEIVE = txtEscOVC_DRECEIVE.Text;
                querycash.OVC_COMPTROLLER_NO = txtEscOVC_COMPTROLLER_NO.Text;
                querycash.OVC_DCOMPTROLLER = txtEscOVC_DCOMPTROLLER.Text;
                querycash.OVC_DAPPROVE = txtEscOVC_DAPPROVE.Text;
                //var querystaa =
                //        (from tsta in mpms.TBMSTATUS
                //         where tsta.OVC_PURCH == strOVC_PURCH
                //         where tsta.OVC_PURCH_5 == strOVC_PURCH_5
                //         where tsta.OVC_STATUS == "28"
                //         select tsta).FirstOrDefault();
                //if (querystaa == null && !txtEscOVC_DAPPROVE.Text.Equals(string.Empty))
                //{
                //    TBMSTATUS sta = new TBMSTATUS();
                //    sta.OVC_PURCH_5 = strOVC_PURCH_5;
                //    sta.OVC_PURCH = strOVC_PURCH;
                //    sta.ONB_TIMES = 1;
                //    sta.OVC_DO_NAME = ViewState["OVC_DO_NAME"].ToString();
                //    sta.OVC_STATUS = "28";
                //    sta.OVC_DBEGIN = DateTime.Now.ToString("yyyy-MM-dd");
                //    sta.OVC_STATUS_SN = Guid.NewGuid();
                //    mpms.TBMSTATUS.Add(sta);
                //
                //    var querysta =
                //        (from tsta in mpms.TBMSTATUS
                //         where tsta.OVC_PURCH == strOVC_PURCH
                //         where tsta.OVC_PURCH_5 == strOVC_PURCH_5
                //         where tsta.OVC_STATUS == "27"
                //         select tsta).FirstOrDefault();
                //    querysta.OVC_DEND = DateTime.Now.ToString("yyyy-MM-dd");
                //}
                //else if (querystaa != null)
                //{
                //    short onbtime = Convert.ToInt16(querystaa.ONB_TIMES + 1);
                //    querystaa.ONB_TIMES = onbtime;
                //}

                mpms.TBMMANAGE_CASH.Add(querycash);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), querycash.GetType().Name.ToString(), this, "新增");
                FCommon.AlertShow(PnMessageC, "success", "系統訊息", "購案合約編號" + strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5 + strOVC_PURCH_6 + "新增成功!!");
            }
            else
                FCommon.AlertShow(PnMessageC, "danger", "系統訊息", strMessage);
        }

        private void dataModifyC(string strOVC_PURCH, string strOVC_PURCH_5)
        {
            string strOVC_REASON_1 = txtEscOVC_REASON_1.Text;
            string strONB_MONEY_1 = txtEscONB_MONEY_1.Text;
            string strONB_MONEY_NT_1 = txtEscONB_MONEY_NT_1.Text;
            string strOVC_REASON_2 = txtEscOVC_REASON_2.Text;
            string strONB_MONEY_2 = txtEscONB_MONEY_2.Text;
            string strONB_MONEY_NT_2 = txtEscONB_MONEY_NT_2.Text;
            string strOVC_REASON_3 = txtEscOVC_REASON_3.Text;
            string strONB_MONEY_3 = txtEscONB_MONEY_3.Text;
            string strONB_MONEY_NT_3 = txtEscONB_MONEY_NT_3.Text;
            string strOVC_OWN_NO = txtEscOVC_OWN_NO.Text;
            string strOVC_WORK_NO = txtEscOVC_WORK_NO.Text;
            string strOVC_ONNAME = drpEscOVC_ONNAME.SelectedItem.Text;
            string strMessage = "";
            int onball = 0;
            string strOVC_PURCH_6 = ViewState["strOVC_PURCH_6_C"].ToString();
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            if (!strOVC_REASON_1.Equals(string.Empty))
            {
                if (!strONB_MONEY_NT_1.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_NT_1, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<p> 項次1新台幣入帳金額請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<p>  項次1新台幣入帳金額欄位不得為空白 </p>";
                if (!strONB_MONEY_1.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_1, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<p> 項次1原幣金額請輸入數字 </p>";
                    }
                }
            }
            if (!strOVC_REASON_2.Equals(string.Empty))
            {
                if (!strONB_MONEY_NT_2.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_NT_2, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<p> 項次2新台幣入帳金額請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<p>  項次2新台幣入帳金額欄位不得為空白 </p>";
                if (!strONB_MONEY_2.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_2, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<p> 項次2原幣金額請輸入數字 </p>";
                    }
                }
            }
            if (!strOVC_REASON_3.Equals(string.Empty))
            {
                if (!strONB_MONEY_NT_3.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_NT_3, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<p> 項次3新台幣入帳金額請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<p>  項次3新台幣入帳金額欄位不得為空白 </p>";
                if (!strONB_MONEY_3.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_3, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<p> 項次3原幣金額請輸入數字 </p>";
                    }
                }
            }
            if (strOVC_OWN_NO.Equals(string.Empty))
                strMessage += "<p> 款項所有權人統一編號欄位不得為空白 </p>";
            if (strOVC_WORK_NO.Equals(string.Empty))
                strMessage += "<p> 承辦單位統一編號欄位不得為空白 </p>";
            if (strOVC_ONNAME == "請選擇")
                strOVC_ONNAME = "";
            if (strMessage.Equals(string.Empty))
            {
                var querycash =
                (from tbmcash in mpms.TBMMANAGE_CASH
                 where tbmcash.OVC_PURCH == strOVC_PURCH
                 where tbmcash.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmcash.OVC_PURCH_6 == strOVC_PURCH_6
                 select tbmcash).FirstOrDefault();

                querycash.OVC_OWN_NAME = txtEscOVC_OWN_NAME.Text;
                querycash.OVC_OWN_NO = txtEscOVC_OWN_NO.Text;
                querycash.OVC_OWN_ADDRESS = txtEscOVC_OWN_ADDRESS.Text;
                querycash.OVC_OWN_TEL = txtEscOVC_OWN_TEL.Text;
                if (!strOVC_REASON_1.Equals(string.Empty))
                {
                    querycash.ONB_ITEM_1 = 1;
                    querycash.OVC_REASON_1 = strOVC_REASON_1;
                    querycash.OVC_CURRENT_1 = drpEscOVC_CURRENT_1.SelectedValue.ToString();
                    if (!strONB_MONEY_1.Equals(string.Empty))
                    {
                        int onbmoney1 = Convert.ToInt32(strONB_MONEY_1);
                        querycash.ONB_MONEY_1 = onbmoney1;
                    }
                    int onbmoneynt1 = Convert.ToInt32(strONB_MONEY_NT_1);
                    querycash.ONB_MONEY_NT_1 = onbmoneynt1;
                    onball = onball + onbmoneynt1;
                }
                if (!strOVC_REASON_2.Equals(string.Empty))
                {
                    querycash.ONB_ITEM_2 = 2;
                    querycash.OVC_REASON_2 = strOVC_REASON_2;
                    querycash.OVC_CURRENT_2 = drpEscOVC_CURRENT_2.SelectedValue.ToString();
                    if (!strONB_MONEY_2.Equals(string.Empty))
                    {
                        int onbmoney2 = Convert.ToInt32(strONB_MONEY_2);
                        querycash.ONB_MONEY_2 = onbmoney2;
                    }
                    int onbmoneynt2 = Convert.ToInt32(strONB_MONEY_NT_2);
                    querycash.ONB_MONEY_NT_2 = onbmoneynt2;
                    onball = onball + onbmoneynt2;
                }
                if (!strOVC_REASON_3.Equals(string.Empty))
                {
                    querycash.ONB_ITEM_3 = 3;
                    querycash.OVC_REASON_3 = strOVC_REASON_3;
                    querycash.OVC_CURRENT_3 = drpEscOVC_CURRENT_3.SelectedValue.ToString();
                    if (!strONB_MONEY_3.Equals(string.Empty))
                    {
                        int onbmoney3 = Convert.ToInt32(strONB_MONEY_3);
                        querycash.ONB_MONEY_3 = onbmoney3;
                    }
                    int onbmoneynt3 = Convert.ToInt32(strONB_MONEY_NT_3);
                    querycash.ONB_MONEY_NT_3 = onbmoneynt3;
                    onball = onball + onbmoneynt3;
                }
                querycash.ONB_ALL_MONEY = onball;
                txtEscONB_ALL_MONEY.Text = onball.ToString();
                querycash.OVC_MARK = txtEscOVC_MARK.Text;
                querycash.OVC_WORK_NO = txtEscOVC_WORK_NO.Text;
                querycash.OVC_WORK_NAME = txtEscOVC_WORK_NAME.Text;
                querycash.OVC_WORK_UNIT = txtEscOVC_WORK_UNIT.Text;
                querycash.OVC_ONNAME = txtEscOVC_ONNAME.Text;
                querycash.OVC_RECEIVE_NO = txtEscOVC_RECEIVE_NO.Text;
                querycash.OVC_DRECEIVE = txtEscOVC_DRECEIVE.Text;
                querycash.OVC_COMPTROLLER_NO = txtEscOVC_COMPTROLLER_NO.Text;
                querycash.OVC_DCOMPTROLLER = txtEscOVC_DCOMPTROLLER.Text;
                querycash.OVC_DAPPROVE = txtEscOVC_DAPPROVE.Text;
                //var querystaa =
                //    (from tsta in mpms.TBMSTATUS
                //     where tsta.OVC_PURCH == strOVC_PURCH
                //     where tsta.OVC_PURCH_5 == strOVC_PURCH_5
                //     where tsta.OVC_STATUS == "28"
                //     select tsta).FirstOrDefault();
                //if (querystaa == null && !txtEscOVC_DAPPROVE.Text.Equals(string.Empty))
                //{
                //    TBMSTATUS sta = new TBMSTATUS();
                //    sta.OVC_PURCH_5 = strOVC_PURCH_5;
                //    sta.OVC_PURCH = strOVC_PURCH;
                //    sta.ONB_TIMES = 1;
                //    sta.OVC_DO_NAME = ViewState["OVC_DO_NAME"].ToString();
                //    sta.OVC_STATUS = "28";
                //    sta.OVC_DBEGIN = DateTime.Now.ToString("yyyy-MM-dd");
                //    sta.OVC_STATUS_SN = Guid.NewGuid();
                //    mpms.TBMSTATUS.Add(sta);
                //
                //    var querysta =
                //        (from tsta in mpms.TBMSTATUS
                //         where tsta.OVC_PURCH == strOVC_PURCH
                //         where tsta.OVC_PURCH_5 == strOVC_PURCH_5
                //         where tsta.OVC_STATUS == "27"
                //         select tsta).FirstOrDefault();
                //    if(querysta != null)
                //        querysta.OVC_DEND = DateTime.Now.ToString("yyyy-MM-dd");
                //}
                //else
                //{
                //    if (querystaa != null)
                //    {
                //        short onbtime = Convert.ToInt16(querystaa.ONB_TIMES + 1);
                //        querystaa.ONB_TIMES = onbtime;
                //    }
                //}
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), querycash.GetType().Name.ToString(), this, "修改");
                FCommon.AlertShow(PnMessageC, "success", "系統訊息", "購案合約編號" + strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5 + strOVC_PURCH_6 + "存檔成功!!");
            }
            else
                FCommon.AlertShow(PnMessageC, "danger", "系統訊息", strMessage);
        }

        #endregion

        #region dataEditD

        private void dataEditD(string strOVC_PURCH, string strOVC_PURCH_5, string strOVC_PURCH_6,string copyOVC_PURCH_6)
        {
            string OVC_PURCH_6 = copyOVC_PURCH_6 == "" ? strOVC_PURCH_6 : copyOVC_PURCH_6;
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            lblDocOVC_PURCH_6.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5 + strOVC_PURCH_6;
            txtDocOVC_PURCH_6.Visible = false;
            var querydoc =
                (from tbmdoc in mpms.TBMMANAGE_PROM
                 where tbmdoc.OVC_PURCH == strOVC_PURCH
                 where tbmdoc.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmdoc.OVC_PURCH_6 == OVC_PURCH_6
                 select tbmdoc).FirstOrDefault();
            txtDocOVC_OWN_NAME.Text = querydoc.OVC_OWN_NAME;
            //txtDocOVC_OWN_NO.Text = querydoc.OVC_OWN_NO;
            txtDocOVC_OWN_ADDRESS.Text = querydoc.OVC_OWN_ADDRESS;
            txtDocOVC_OWNED_NAME.Text = querydoc.OVC_OWNED_NAME;
            txtDocOVC_OWNED_NO.Text = querydoc.OVC_OWNED_NO;
            txtDocOVC_OWNED_ADDRESS.Text = querydoc.OVC_OWNED_ADDRESS;
            //代管項目
            if (querydoc.ONB_ITEM_1 != null)
            {
                txtDocOVC_REASON_1.Text = querydoc.OVC_REASON_1;
                txtDocOVC_NSTOCK_1.Text = querydoc.OVC_NSTOCK_1;
                txtDocOVC_NUMBER_1.Text = querydoc.OVC_NUMBER_1;
                txtDocONB_MONEY_1.Text = querydoc.ONB_MONEY_1.ToString();
                txtDocExpDate_1.Text = querydoc.OVC_DEFFECT_1;
            }
            if (querydoc.ONB_ITEM_2 != null)
            {
                txtDocOVC_REASON_2.Text = querydoc.OVC_REASON_2;
                txtDocOVC_NSTOCK_2.Text = querydoc.OVC_NSTOCK_2;
                txtDocOVC_NUMBER_2.Text = querydoc.OVC_NUMBER_2;
                txtDocONB_MONEY_2.Text = querydoc.ONB_MONEY_2.ToString();
                txtDocExpDate_2.Text = querydoc.OVC_DEFFECT_2;
            }
            if (querydoc.ONB_ITEM_3 != null)
            {
                txtDocOVC_REASON_3.Text = querydoc.OVC_REASON_3;
                txtDocOVC_NSTOCK_3.Text = querydoc.OVC_NSTOCK_3;
                txtDocOVC_NUMBER_3.Text = querydoc.OVC_NUMBER_3;
                txtDocONB_MONEY_3.Text = querydoc.ONB_MONEY_3.ToString();
                txtDocExpDate_3.Text = querydoc.OVC_DEFFECT_3;
            }
            //代管項目

            txtDocOVC_MARK.Text = querydoc.OVC_MARK;

            txtDocOVC_WORK_NO.Text = querydoc.OVC_WORK_NO;
            txtDocOVC_WORK_NAME.Text = querydoc.OVC_WORK_NAME;
            txtDocOVC_WORK_UNIT.Text = "採購發包處";
            if (querydoc.OVC_ONNAME != null)
                drpDocOVC_ONNAME.SelectedItem.Text = querydoc.OVC_ONNAME;
            txtDocOVC_ONNAME.Text = querydoc.OVC_ONNAME;
            txtDocOVC_RECEIVE_NO.Text = querydoc.OVC_RECEIVE_NO;
            txtDocOVC_DRECEIVE.Text = querydoc.OVC_DRECEIVE;
            txtDocOVC_COMPTROLLER_NO.Text = querydoc.OVC_COMPTROLLER_NO;
            txtDocOVC_DCOMPTROLLER.Text = querydoc.OVC_DCOMPTROLLER;
            txtEscOVC_DAPPROVE.Text = querydoc.OVC_DAPPROVE;
        }

        private void dataNewD(string strOVC_PURCH, string strOVC_PURCH_5)
        {
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();

            string strOVC_REASON_1 = txtDocOVC_REASON_1.Text;
            string strONB_MONEY_1 = txtDocONB_MONEY_1.Text;

            string strOVC_REASON_2 = txtDocOVC_REASON_2.Text;
            string strONB_MONEY_2 = txtDocONB_MONEY_2.Text;

            string strOVC_REASON_3 = txtDocOVC_REASON_3.Text;
            string strONB_MONEY_3 = txtDocONB_MONEY_3.Text;
            string strOVC_WORK_NO = txtDocOVC_WORK_NO.Text;
            string strOVC_ONNAME = drpDocOVC_ONNAME.SelectedItem.Text;
            string strMessage = "";
            int onball = 0;
            string strOVC_PURCH_6 = txtDocOVC_PURCH_6.Text;
            var querydocyesno =
                (from tbmdoc in mpms.TBMMANAGE_PROM
                 where tbmdoc.OVC_PURCH == strOVC_PURCH
                 where tbmdoc.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmdoc.OVC_PURCH_6 == strOVC_PURCH_6
                 select tbmdoc).FirstOrDefault();
            if (querydocyesno != null)
                strMessage += "<p> 此購案合約編號已有資料 無法新增 </p>";

            if (strOVC_PURCH_6.Equals(string.Empty))
                strMessage += "<p> 分約號欄位不得為空白 </p>";
            if (strOVC_WORK_NO.Equals(string.Empty))
                strMessage += "<p> 承辦單位統一編號欄位不得為空白 </p>";
            if (strOVC_ONNAME == "請選擇")
                strOVC_ONNAME = "";

            if (!strOVC_REASON_1.Equals(string.Empty))
            {
                if (!strONB_MONEY_1.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_1, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<p> 項次1入帳金額請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<p>  項次1入帳金額欄位不得為空白 </p>";

            }
            if (!strOVC_REASON_2.Equals(string.Empty))
            {
                if (!strONB_MONEY_2.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_2, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<p> 項次2入帳金額請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<p>  項次2入帳金額欄位不得為空白 </p>";

            }
            if (!strOVC_REASON_3.Equals(string.Empty))
            {
                if (!strONB_MONEY_3.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_3, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<p> 項次3入帳金額請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<p>項次3入帳金額欄位不得為空白 </p>";
            }
            if (strMessage.Equals(string.Empty))
            {
                TBMMANAGE_PROM querydoc = new TBMMANAGE_PROM();
                querydoc.OVC_PROM_SN = Guid.NewGuid();
                querydoc.OVC_PURCH = strOVC_PURCH;
                querydoc.OVC_PURCH_5 = strOVC_PURCH_5;
                querydoc.OVC_PURCH_6 = strOVC_PURCH_6;
                querydoc.OVC_OWN_NO = DateTime.Now.ToShortDateString();
                querydoc.OVC_OWN_NAME = txtDocOVC_OWN_NAME.Text;
                querydoc.OVC_OWN_ADDRESS = txtDocOVC_OWN_ADDRESS.Text;
                querydoc.OVC_OWNED_NO = txtDocOVC_OWNED_NO.Text;
                querydoc.OVC_OWNED_NAME = txtDocOVC_OWNED_NAME.Text;
                querydoc.OVC_OWNED_ADDRESS = txtDocOVC_OWNED_ADDRESS.Text;

                if (!strOVC_REASON_1.Equals(string.Empty))
                {
                    querydoc.ONB_ITEM_1 = 1;
                    querydoc.OVC_REASON_1 = strOVC_REASON_1;
                    querydoc.OVC_NSTOCK_1 = txtDocOVC_NSTOCK_1.Text;
                    querydoc.OVC_DEFFECT_1 = txtDocExpDate_1.Text;
                    int onbmoney1 = Convert.ToInt32(strONB_MONEY_1);
                    querydoc.ONB_MONEY_1 = onbmoney1;


                    onball = onball + onbmoney1;
                }
                if (!strOVC_REASON_2.Equals(string.Empty))
                {
                    querydoc.ONB_ITEM_2 = 2;
                    querydoc.OVC_REASON_2 = strOVC_REASON_2;
                    querydoc.OVC_NSTOCK_2 = txtDocOVC_NSTOCK_2.Text;
                    querydoc.OVC_DEFFECT_2 = txtDocExpDate_2.Text;
                    int onbmoney2 = Convert.ToInt32(strONB_MONEY_2);
                    querydoc.ONB_MONEY_2 = onbmoney2;


                    onball = onball + onbmoney2;
                }
                if (!strOVC_REASON_3.Equals(string.Empty))
                {
                    querydoc.ONB_ITEM_3 = 3;
                    querydoc.OVC_REASON_3 = strOVC_REASON_3;
                    querydoc.OVC_NSTOCK_3 = txtDocOVC_NSTOCK_3.Text;
                    querydoc.OVC_DEFFECT_3 = txtDocExpDate_3.Text;
                    int onbmoney3 = Convert.ToInt32(strONB_MONEY_3);
                    querydoc.ONB_MONEY_3 = onbmoney3;


                    onball = onball + onbmoney3;
                }

                querydoc.ONB_ALL_MONEY = onball;
                txtDocONB_ALL_MONEY.Text = onball.ToString();
                querydoc.OVC_MARK = txtDocOVC_MARK.Text;
                querydoc.OVC_WORK_NO = txtDocOVC_WORK_NO.Text;
                querydoc.OVC_WORK_NAME = txtDocOVC_WORK_NAME.Text;
                querydoc.OVC_WORK_UNIT = txtDocOVC_WORK_UNIT.Text;
                querydoc.OVC_ONNAME = txtDocOVC_ONNAME.Text;
                querydoc.OVC_RECEIVE_NO = txtDocOVC_RECEIVE_NO.Text;
                querydoc.OVC_DRECEIVE = txtDocOVC_DRECEIVE.Text;
                querydoc.OVC_COMPTROLLER_NO = txtDocOVC_COMPTROLLER_NO.Text;
                querydoc.OVC_DCOMPTROLLER = txtDocOVC_DCOMPTROLLER.Text;
                querydoc.OVC_DAPPROVE = txtDocOVC_DAPPROVE.Text;
                mpms.TBMMANAGE_PROM.Add(querydoc);
                //var querystaa =
                //    (from tsta in mpms.TBMSTATUS
                //     where tsta.OVC_PURCH == strOVC_PURCH
                //     where tsta.OVC_PURCH_5 == strOVC_PURCH_5
                //     where tsta.OVC_STATUS == "28"
                //     select tsta).FirstOrDefault();
                //if (querystaa == null && !txtDocOVC_DAPPROVE.Text.Equals(string.Empty))
                //{
                //    TBMSTATUS sta = new TBMSTATUS();
                //    sta.OVC_PURCH_5 = strOVC_PURCH_5;
                //    sta.OVC_PURCH = strOVC_PURCH;
                //    sta.ONB_TIMES = 1;
                //    sta.OVC_DO_NAME = ViewState["OVC_DO_NAME"].ToString();
                //    sta.OVC_STATUS = "28";
                //    sta.OVC_DBEGIN = DateTime.Now.ToString("yyyy-MM-dd");
                //    sta.OVC_STATUS_SN = Guid.NewGuid();
                //    mpms.TBMSTATUS.Add(sta);
                //
                //    var querysta =
                //        (from tsta in mpms.TBMSTATUS
                //         where tsta.OVC_PURCH == strOVC_PURCH
                //         where tsta.OVC_PURCH_5 == strOVC_PURCH_5
                //         where tsta.OVC_STATUS == "27"
                //         select tsta).FirstOrDefault();
                //    querysta.OVC_DEND = DateTime.Now.ToString("yyyy-MM-dd");
                //}
                //else
                //{
                //    short onbtime = Convert.ToInt16(querystaa.ONB_TIMES + 1);
                //    querystaa.ONB_TIMES = onbtime;
                //}
                try
                {
                    // do something
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), querydoc.GetType().Name.ToString(), this, "新增");
                }
                catch (Exception ex)
                {
                    // do something
                    throw ex;
                }

                FCommon.AlertShow(PnMessageD, "success", "系統訊息", "購案合約編號" + strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5 + strOVC_PURCH_6 + "新增成功!!");
            }
            else
                FCommon.AlertShow(PnMessageD, "danger", "系統訊息", strMessage);
        }

        private void dataModifyD(string strOVC_PURCH, string strOVC_PURCH_5)
        {
            string strOVC_PURCH_6 = ViewState["strOVC_PURCH_6_D"].ToString();
            string strOVC_REASON_1 = txtDocOVC_REASON_1.Text;
            string strONB_MONEY_1 = txtDocONB_MONEY_1.Text;
            string strOVC_REASON_2 = txtDocOVC_REASON_2.Text;
            string strONB_MONEY_2 = txtDocONB_MONEY_2.Text;
            string strOVC_REASON_3 = txtDocOVC_REASON_3.Text;
            string strONB_MONEY_3 = txtDocONB_MONEY_3.Text;
            string strOVC_WORK_NO = txtDocOVC_WORK_NO.Text;
            string strOVC_ONNAME = drpDocOVC_ONNAME.SelectedItem.Text;
            string strMessage = "";
            int onball = 0;

            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            if (strOVC_PURCH.Equals(string.Empty))
                strMessage += "<P> 分約號欄位不得為空白 </p>";
            if (strOVC_WORK_NO.Equals(string.Empty))
                strMessage += "<P> 承辦單位統一編號欄位不得為空白 </p>";
            if (strOVC_ONNAME == "請選擇")
                strOVC_ONNAME = "";

            if (!strOVC_REASON_1.Equals(string.Empty))
            {
                if (!strONB_MONEY_1.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_1, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<P> 項次1入帳金額請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<P>  項次1入帳金額欄位不得為空白 </p>";

            }
            if (!strOVC_REASON_2.Equals(string.Empty))
            {
                if (!strONB_MONEY_2.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_2, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<P> 項次2入帳金額請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<P>  項次2入帳金額欄位不得為空白 </p>";

            }
            if (!strOVC_REASON_3.Equals(string.Empty))
            {
                if (!strONB_MONEY_3.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_3, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<P> 項次3入帳金額請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<P>  項次3入帳金額欄位不得為空白 </p>";
            }
            if (strMessage.Equals(string.Empty))
            {
                var querydoc =
                (from tbmdoc in mpms.TBMMANAGE_PROM
                 where tbmdoc.OVC_PURCH == strOVC_PURCH
                 where tbmdoc.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmdoc.OVC_PURCH_6 == strOVC_PURCH_6
                 select tbmdoc).FirstOrDefault();

                querydoc.OVC_OWN_NAME = txtDocOVC_OWN_NAME.Text;
                querydoc.OVC_OWN_ADDRESS = txtDocOVC_OWN_ADDRESS.Text;
                querydoc.OVC_OWNED_NO = txtDocOVC_OWNED_NO.Text;
                querydoc.OVC_OWNED_NAME = txtDocOVC_OWNED_NAME.Text;
                querydoc.OVC_OWNED_ADDRESS = txtDocOVC_OWNED_ADDRESS.Text;
                if (!strOVC_REASON_1.Equals(string.Empty))
                {
                    querydoc.ONB_ITEM_1 = 1;
                    querydoc.OVC_REASON_1 = strOVC_REASON_1;
                    int onbmoney1 = Convert.ToInt32(strONB_MONEY_1);
                    querydoc.ONB_MONEY_1 = onbmoney1;
                    querydoc.OVC_NSTOCK_1 = txtDocOVC_NSTOCK_1.Text;
                    querydoc.OVC_DEFFECT_1 = txtDocExpDate_1.Text;

                    onball = onball + onbmoney1;
                }
                if (!strOVC_REASON_2.Equals(string.Empty))
                {
                    querydoc.ONB_ITEM_2 = 2;
                    querydoc.OVC_REASON_2 = strOVC_REASON_2;
                    int onbmoney2 = Convert.ToInt32(strONB_MONEY_2);
                    querydoc.ONB_MONEY_2 = onbmoney2;
                    querydoc.OVC_NSTOCK_2 = txtDocOVC_NSTOCK_2.Text;
                    querydoc.OVC_DEFFECT_2 = txtDocExpDate_2.Text;

                    onball = onball + onbmoney2;
                }
                if (!strOVC_REASON_3.Equals(string.Empty))
                {
                    querydoc.ONB_ITEM_3 = 3;
                    querydoc.OVC_REASON_3 = strOVC_REASON_3;
                    int onbmoney3 = Convert.ToInt32(strONB_MONEY_3);
                    querydoc.ONB_MONEY_3 = onbmoney3;
                    querydoc.OVC_NSTOCK_3 = txtDocOVC_NSTOCK_3.Text;
                    querydoc.OVC_DEFFECT_3 = txtDocExpDate_3.Text;

                    onball = onball + onbmoney3;
                }

                querydoc.ONB_ALL_MONEY = onball;
                txtDocONB_ALL_MONEY.Text = onball.ToString();
                querydoc.OVC_MARK = txtDocOVC_MARK.Text;
                querydoc.OVC_WORK_NO = txtDocOVC_WORK_NO.Text;
                querydoc.OVC_WORK_NAME = txtDocOVC_WORK_NAME.Text;
                querydoc.OVC_WORK_UNIT = txtDocOVC_WORK_UNIT.Text;
                querydoc.OVC_ONNAME = txtDocOVC_ONNAME.Text;
                querydoc.OVC_RECEIVE_NO = txtDocOVC_RECEIVE_NO.Text;
                querydoc.OVC_DRECEIVE = txtDocOVC_DRECEIVE.Text;
                querydoc.OVC_COMPTROLLER_NO = txtDocOVC_COMPTROLLER_NO.Text;
                querydoc.OVC_DCOMPTROLLER = txtDocOVC_DCOMPTROLLER.Text;
                querydoc.OVC_DAPPROVE = txtDocOVC_DAPPROVE.Text;
                //var querystaa =
                //    (from tsta in mpms.TBMSTATUS
                //     where tsta.OVC_PURCH == strOVC_PURCH
                //     where tsta.OVC_PURCH_5 == strOVC_PURCH_5
                //     where tsta.OVC_STATUS == "28"
                //     select tsta).FirstOrDefault();
                //if (querystaa == null && !txtDocOVC_DAPPROVE.Text.Equals(string.Empty))
                //{
                //    TBMSTATUS sta = new TBMSTATUS();
                //    sta.OVC_PURCH_5 = strOVC_PURCH_5;
                //    sta.OVC_PURCH = strOVC_PURCH;
                //    sta.ONB_TIMES = 1;
                //    sta.OVC_DO_NAME = ViewState["OVC_DO_NAME"].ToString();
                //    sta.OVC_STATUS = "28";
                //    sta.OVC_DBEGIN = DateTime.Now.ToString("yyyy-MM-dd");
                //    sta.OVC_STATUS_SN = Guid.NewGuid();
                //    mpms.TBMSTATUS.Add(sta);
                //
                //    var querysta =
                //        (from tsta in mpms.TBMSTATUS
                //         where tsta.OVC_PURCH == strOVC_PURCH
                //         where tsta.OVC_PURCH_5 == strOVC_PURCH_5
                //         where tsta.OVC_STATUS == "27"
                //         select tsta).FirstOrDefault();
                //    querysta.OVC_DEND = DateTime.Now.ToString("yyyy-MM-dd");
                //}
                //else
                //{
                //    short onbtime = Convert.ToInt16(querystaa.ONB_TIMES + 1);
                //    querystaa.ONB_TIMES = onbtime;
                //}
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), querydoc.GetType().Name.ToString(), this, "修改");
                FCommon.AlertShow(PnMessageD, "success", "系統訊息", "購案合約編號" + strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5 + strOVC_PURCH_6 + "存檔成功!!");
            }
            else
                FCommon.AlertShow(PnMessageD, "danger", "系統訊息", strMessage);

        }

        #endregion

        #region dataEditS

        private void dataEditS(string strOVC_PURCH, string strOVC_PURCH_5, string strOVC_PURCH_6, string copyOVC_PURCH_6)
        {
            string OVC_PURCH_6 = copyOVC_PURCH_6 == "" ? strOVC_PURCH_6 : copyOVC_PURCH_6;
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            lblSecOVC_PURCH_6.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5 + strOVC_PURCH_6;
            txtSecOVC_PURCH_6.Visible = false;
            var querysec =
                (from tbmstock in mpms.TBMMANAGE_STOCK
                 where tbmstock.OVC_PURCH == strOVC_PURCH
                 where tbmstock.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmstock.OVC_PURCH_6 == OVC_PURCH_6
                 select tbmstock).FirstOrDefault();
            txtSecOVC_OWN_NAME.Text = querysec.OVC_OWN_NAME;
            txtSecOVC_OWN_NO.Text = querysec.OVC_OWN_NO;
            txtSecOVC_OWN_ADDRESS.Text = querysec.OVC_OWN_ADDRESS;
            txtSecOVC_OWN_TEL.Text = querysec.OVC_OWN_TEL;
            //代管項目
            if (querysec.ONB_ITEM_1 != null)
            {
                txtSecOVC_REASON_1.Text = querysec.OVC_REASON_1;
                txtSecOVC_NSTOCK_1.Text = querysec.OVC_NSTOCK_1;
                txtSecOVC_CURRENT_1.Text = querysec.OVC_CURRENT_1;
                txtSecONB_MONEY_1.Text = querysec.ONB_MONEY_1.ToString();
                txtSecONB_MONEY_NT_1.Text = querysec.ONB_MONEY_NT_1.ToString();
            }
            if (querysec.ONB_ITEM_2 != null)
            {
                txtSecOVC_REASON_2.Text = querysec.OVC_REASON_2;
                txtSecOVC_NSTOCK_2.Text = querysec.OVC_NSTOCK_2;
                txtSecOVC_CURRENT_2.Text = querysec.OVC_CURRENT_2;
                txtSecONB_MONEY_2.Text = querysec.ONB_MONEY_2.ToString();
                txtSecONB_MONEY_NT_2.Text = querysec.ONB_MONEY_NT_2.ToString();
            }
            if (querysec.ONB_ITEM_3 != null)
            {
                txtSecOVC_REASON_3.Text = querysec.OVC_REASON_3;
                txtSecOVC_NSTOCK_3.Text = querysec.OVC_NSTOCK_3;
                txtSecOVC_CURRENT_3.Text = querysec.OVC_CURRENT_3;
                txtSecONB_MONEY_3.Text = querysec.ONB_MONEY_3.ToString();
                txtSecONB_MONEY_NT_3.Text = querysec.ONB_MONEY_NT_3.ToString();
            }

            //代管項目
            txtSecONB_ALL_MONEY.Text = querysec.ONB_ALL_MONEY.ToString();
            txtSecOVC_MARK.Text = querysec.OVC_MARK;

            txtSecOVC_WORK_NO.Text = querysec.OVC_WORK_NO;
            txtSecOVC_WORK_NAME.Text = querysec.OVC_WORK_NAME;
            txtSecOVC_WORK_UNIT.Text = "採購發包處";
            if (querysec.OVC_ONNAME != null)
                drpSecOVC_ONNAME.SelectedItem.Text = querysec.OVC_ONNAME;
            txtSecOVC_ONNAME.Text = querysec.OVC_ONNAME;
            txtSecOVC_RECEIVE_NO.Text = querysec.OVC_RECEIVE_NO;
            txtSecOVC_DRECEIVE.Text = querysec.OVC_DRECEIVE;
            txtSecOVC_COMPTROLLER_NO.Text = querysec.OVC_COMPTROLLER_NO;
            txtSecOVC_DCOMPTROLLER.Text = querysec.OVC_DCOMPTROLLER;
            txtSecOVC_DAPPROVE.Text = querysec.OVC_DAPPROVE;
        }

        private void dataNewS(string strOVC_PURCH, string strOVC_PURCH_5)
        {
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();


            string strOVC_REASON_1 = txtSecOVC_REASON_1.Text;
            string strONB_MONEY_1 = txtSecONB_MONEY_1.Text;
            string strONB_MONEY_NT_1 = txtSecONB_MONEY_NT_1.Text;
            string strOVC_REASON_2 = txtSecOVC_REASON_2.Text;
            string strONB_MONEY_2 = txtSecONB_MONEY_2.Text;
            string strONB_MONEY_NT_2 = txtSecONB_MONEY_NT_2.Text;
            string strOVC_REASON_3 = txtSecOVC_REASON_3.Text;
            string strONB_MONEY_3 = txtSecONB_MONEY_3.Text;
            string strONB_MONEY_NT_3 = txtSecONB_MONEY_NT_3.Text;
            string strOVC_ONNAME = drpSecOVC_ONNAME.SelectedItem.Text;
            string strOVC_WORK_NO = txtSecOVC_OWN_NO.Text;
            string strMessage = "";
            int onball = 0;
            string strOVC_PURCH_6 = txtSecOVC_PURCH_6.Text;
            var querystockyesno =
                (from tbmstock in mpms.TBMMANAGE_STOCK
                 where tbmstock.OVC_PURCH == strOVC_PURCH
                 where tbmstock.OVC_PURCH_5 == strOVC_PURCH_5
                 where tbmstock.OVC_PURCH_6 == strOVC_PURCH_6
                 select tbmstock).FirstOrDefault();
            if (querystockyesno != null)
                strMessage += "<P> 此購案合約編號已有資料 無法新增 </p>";

            if (strOVC_PURCH_6.Equals(string.Empty))
                strMessage += "<P> 分約號欄位不得為空白 </p>";
            if (strOVC_WORK_NO.Equals(string.Empty))
                strMessage += "<P> 承辦單位統一編號欄位不得為空白 </p>";
            if (strOVC_ONNAME == "請選擇")
                strOVC_ONNAME = "";
            if (!strOVC_REASON_1.Equals(string.Empty))
            {
                if (!strONB_MONEY_NT_1.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_NT_1, out n))
                    {
                    }
                    else
                    {
                        strMessage += "<P> 項次1新台幣入帳金額請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<P>  項次1新台幣入帳金額欄位不得為空白 </p>";
                if (!strONB_MONEY_1.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_1, out n))
                    {
                    }
                    else
                    {
                        strMessage += "<P> 項次1有價證券面額數量請輸入數字 </p>";
                    }
                }
            }
            if (!strOVC_REASON_2.Equals(string.Empty))
            {
                if (!strONB_MONEY_NT_2.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_NT_2, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<P> 項次2新台幣入帳金額請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<P>  項次2新台幣入帳金額欄位不得為空白 </p>";
                if (!strONB_MONEY_2.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_2, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<P> 項次2有價證券面額數量請輸入數字 </p>";
                    }
                }
            }
            if (!strOVC_REASON_3.Equals(string.Empty))
            {
                if (!strONB_MONEY_NT_3.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_NT_3, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<P> 項次3新台幣入帳金額請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<P>  項次3新台幣入帳金額欄位不得為空白 </p>";
                if (!strONB_MONEY_3.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_3, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<P> 項次3有價證券面額數量請輸入數字 </p>";
                    }
                }
            }
            if (strMessage.Equals(string.Empty))
            {
                TBMMANAGE_STOCK querystock = new TBMMANAGE_STOCK();
                querystock.OVC_STOCK_SN = Guid.NewGuid();
                querystock.OVC_PURCH = strOVC_PURCH;
                querystock.OVC_PURCH_5 = strOVC_PURCH_5;
                querystock.OVC_PURCH_6 = strOVC_PURCH_6;
                querystock.OVC_OWN_NAME = txtSecOVC_OWN_NAME.Text;
                querystock.OVC_OWN_NO = txtSecOVC_OWN_NO.Text;
                querystock.OVC_OWN_ADDRESS = txtSecOVC_OWN_ADDRESS.Text;
                querystock.OVC_OWN_TEL = txtSecOVC_OWN_TEL.Text;
                if (!strOVC_REASON_1.Equals(string.Empty))
                {
                    querystock.ONB_ITEM_1 = 1;
                    querystock.OVC_REASON_1 = strOVC_REASON_1;
                    querystock.OVC_NSTOCK_1 = txtSecOVC_NSTOCK_1.Text;
                    querystock.OVC_CURRENT_1 = txtSecOVC_CURRENT_1.Text;
                    if (!strONB_MONEY_1.Equals(string.Empty))
                    {
                        int onbmoney1 = Convert.ToInt32(strONB_MONEY_1);
                        querystock.ONB_MONEY_1 = onbmoney1;
                    }
                    int onbmoneynt1 = Convert.ToInt32(strONB_MONEY_NT_1);
                    querystock.ONB_MONEY_NT_1 = onbmoneynt1;
                    onball = onball + onbmoneynt1;
                }
                if (!strOVC_REASON_2.Equals(string.Empty))
                {
                    querystock.ONB_ITEM_2 = 2;
                    querystock.OVC_REASON_2 = strOVC_REASON_2;
                    querystock.OVC_CURRENT_2 = txtSecOVC_CURRENT_2.Text;
                    querystock.OVC_NSTOCK_2 = txtSecOVC_NSTOCK_2.Text;
                    if (!strONB_MONEY_2.Equals(string.Empty))
                    {
                        int onbmoney2 = Convert.ToInt32(strONB_MONEY_2);
                        querystock.ONB_MONEY_2 = onbmoney2;
                    }
                    int onbmoneynt2 = Convert.ToInt32(strONB_MONEY_NT_2);
                    querystock.ONB_MONEY_NT_2 = onbmoneynt2;
                    onball = onball + onbmoneynt2;
                }
                if (!strOVC_REASON_3.Equals(string.Empty))
                {
                    querystock.ONB_ITEM_3 = 3;
                    querystock.OVC_REASON_3 = strOVC_REASON_3;
                    querystock.OVC_CURRENT_3 = txtSecOVC_CURRENT_3.Text;
                    querystock.OVC_NSTOCK_3 = txtSecOVC_NSTOCK_3.Text;
                    if (!strONB_MONEY_3.Equals(string.Empty))
                    {
                        int onbmoney3 = Convert.ToInt32(strONB_MONEY_3);
                        querystock.ONB_MONEY_3 = onbmoney3;
                    }
                    int onbmoneynt3 = Convert.ToInt32(strONB_MONEY_NT_3);
                    querystock.ONB_MONEY_NT_3 = onbmoneynt3;
                    onball = onball + onbmoneynt3;
                }
                querystock.ONB_ALL_MONEY = onball;
                txtSecONB_ALL_MONEY.Text = onball.ToString();
                querystock.OVC_MARK = txtSecOVC_MARK.Text;
                querystock.OVC_WORK_NO = txtSecOVC_WORK_NO.Text;
                querystock.OVC_ONNAME = txtSecOVC_ONNAME.Text;
                querystock.OVC_WORK_NAME = txtSecOVC_WORK_NAME.Text;
                querystock.OVC_WORK_UNIT = txtSecOVC_WORK_UNIT.Text;
                querystock.OVC_RECEIVE_NO = txtSecOVC_RECEIVE_NO.Text;
                querystock.OVC_DRECEIVE = txtSecOVC_DRECEIVE.Text;
                querystock.OVC_COMPTROLLER_NO = txtSecOVC_COMPTROLLER_NO.Text;
                querystock.OVC_DCOMPTROLLER = txtSecOVC_DCOMPTROLLER.Text;
                querystock.OVC_DAPPROVE = txtSecOVC_DAPPROVE.Text;
                //var querystaa =
                //    (from tsta in mpms.TBMSTATUS
                //     where tsta.OVC_PURCH == strOVC_PURCH
                //     where tsta.OVC_PURCH_5 == strOVC_PURCH_5
                //     where tsta.OVC_STATUS == "28"
                //     select tsta).FirstOrDefault();
                //if (querystaa == null && !txtSecOVC_DAPPROVE.Text.Equals(string.Empty))
                //{
                //    TBMSTATUS sta = new TBMSTATUS();
                //    sta.OVC_PURCH_5 = strOVC_PURCH_5;
                //    sta.OVC_PURCH = strOVC_PURCH;
                //    sta.ONB_TIMES = 1;
                //    sta.OVC_DO_NAME = ViewState["OVC_DO_NAME"].ToString();
                //    sta.OVC_STATUS = "28";
                //    sta.OVC_DBEGIN = DateTime.Now.ToString("yyyy-MM-dd");
                //    sta.OVC_STATUS_SN = Guid.NewGuid();
                //    mpms.TBMSTATUS.Add(sta);
                //
                //    var querysta =
                //        (from tsta in mpms.TBMSTATUS
                //         where tsta.OVC_PURCH == strOVC_PURCH
                //         where tsta.OVC_PURCH_5 == strOVC_PURCH_5
                //         where tsta.OVC_STATUS == "27"
                //         select tsta).FirstOrDefault();
                //    querysta.OVC_DEND = DateTime.Now.ToString("yyyy-MM-dd");
                //}
                //else
                //{
                //    short onbtime = Convert.ToInt16(querystaa.ONB_TIMES + 1);
                //    querystaa.ONB_TIMES = onbtime;
                //}
                mpms.TBMMANAGE_STOCK.Add(querystock);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), querystock.GetType().Name.ToString(), this, "新增");
                FCommon.AlertShow(PnMessageS, "success", "系統訊息", "購案合約編號" + strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5 + strOVC_PURCH_6 + "新增成功!!");
                ViewState["strOVC_PURCH_6_C"] = strOVC_PURCH_6;
            }
            else
                FCommon.AlertShow(PnMessageS, "danger", "系統訊息", strMessage);
        }

        private void dataModifyS(string strOVC_PURCH, string strOVC_PURCH_5)
        {
            string strOVC_REASON_1 = txtSecOVC_REASON_1.Text;
            string strONB_MONEY_1 = txtSecONB_MONEY_1.Text;
            string strONB_MONEY_NT_1 = txtSecONB_MONEY_NT_1.Text;
            string strOVC_REASON_2 = txtSecOVC_REASON_2.Text;
            string strONB_MONEY_2 = txtSecONB_MONEY_2.Text;
            string strONB_MONEY_NT_2 = txtSecONB_MONEY_NT_2.Text;
            string strOVC_REASON_3 = txtSecOVC_REASON_3.Text;
            string strONB_MONEY_3 = txtSecONB_MONEY_3.Text;
            string strONB_MONEY_NT_3 = txtSecONB_MONEY_NT_3.Text;
            string strOVC_ONNAME = drpSecOVC_ONNAME.SelectedItem.Text;

            string strMessage = "";
            int onball = 0;
            string strOVC_PURCH_6 = ViewState["strOVC_PURCH_6_S"].ToString();
            var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
            if (strOVC_ONNAME == "請選擇")
                strOVC_ONNAME = "";
            if (!strOVC_REASON_1.Equals(string.Empty))
            {
                if (!strONB_MONEY_NT_1.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_NT_1, out n))
                    {
                    }
                    else
                    {
                        strMessage += "<P> 項次1新台幣入帳金額請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<P>  項次1新台幣入帳金額欄位不得為空白 </p>";
                if (!strONB_MONEY_1.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_1, out n))
                    {
                    }
                    else
                    {
                        strMessage += "<P> 項次1有價證券面額數量請輸入數字 </p>";
                    }
                }
            }
            if (!strOVC_REASON_2.Equals(string.Empty))
            {
                if (!strONB_MONEY_NT_2.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_NT_2, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<P> 項次2新台幣入帳金額請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<P>  項次2新台幣入帳金額欄位不得為空白 </p>";
                if (!strONB_MONEY_2.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_2, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<P> 項次2有價證券面額數量請輸入數字 </p>";
                    }
                }
            }
            if (!strOVC_REASON_3.Equals(string.Empty))
            {
                if (!strONB_MONEY_NT_3.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_NT_3, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<P> 項次3新台幣入帳金額請輸入數字 </p>";
                    }
                }
                else
                    strMessage += "<P>  項次3新台幣入帳金額欄位不得為空白 </p>";
                if (!strONB_MONEY_3.Equals(string.Empty))
                {
                    int n;
                    if (int.TryParse(strONB_MONEY_3, out n))
                    {

                    }
                    else
                    {
                        strMessage += "<P> 項次3有價證券面額數量請輸入數字 </p>";
                    }
                }
            }
            if (strMessage.Equals(string.Empty))
            {
                var querystock =
               (from tbmstock in mpms.TBMMANAGE_STOCK
                where tbmstock.OVC_PURCH == strOVC_PURCH
                where tbmstock.OVC_PURCH_5 == strOVC_PURCH_5
                where tbmstock.OVC_PURCH_6 == strOVC_PURCH_6
                select tbmstock).FirstOrDefault();

                querystock.OVC_OWN_NAME = txtSecOVC_OWN_NAME.Text;
                querystock.OVC_OWN_NO = txtSecOVC_OWN_NO.Text;
                querystock.OVC_OWN_ADDRESS = txtSecOVC_OWN_ADDRESS.Text;
                querystock.OVC_OWN_TEL = txtSecOVC_OWN_TEL.Text;
                if (!strOVC_REASON_1.Equals(string.Empty))
                {
                    querystock.ONB_ITEM_1 = 1;
                    querystock.OVC_REASON_1 = strOVC_REASON_1;
                    querystock.OVC_NSTOCK_1 = txtSecOVC_NSTOCK_1.Text;
                    querystock.OVC_CURRENT_1 = txtSecOVC_CURRENT_1.Text;
                    if (!strONB_MONEY_1.Equals(string.Empty))
                    {
                        int onbmoney1 = Convert.ToInt32(strONB_MONEY_1);
                        querystock.ONB_MONEY_1 = onbmoney1;
                    }
                    int onbmoneynt1 = Convert.ToInt32(strONB_MONEY_NT_1);
                    querystock.ONB_MONEY_NT_1 = onbmoneynt1;
                    onball = onball + onbmoneynt1;
                }
                if (!strOVC_REASON_2.Equals(string.Empty))
                {
                    querystock.ONB_ITEM_2 = 2;
                    querystock.OVC_REASON_2 = strOVC_REASON_2;
                    querystock.OVC_CURRENT_2 = txtSecOVC_CURRENT_2.Text;
                    querystock.OVC_NSTOCK_2 = txtSecOVC_NSTOCK_2.Text;
                    if (!strONB_MONEY_2.Equals(string.Empty))
                    {
                        int onbmoney2 = Convert.ToInt32(strONB_MONEY_2);
                        querystock.ONB_MONEY_2 = onbmoney2;
                    }
                    int onbmoneynt2 = Convert.ToInt32(strONB_MONEY_NT_2);
                    querystock.ONB_MONEY_NT_2 = onbmoneynt2;
                    onball = onball + onbmoneynt2;
                }
                if (!strOVC_REASON_3.Equals(string.Empty))
                {
                    querystock.ONB_ITEM_3 = 3;
                    querystock.OVC_REASON_3 = strOVC_REASON_3;
                    querystock.OVC_CURRENT_3 = txtSecOVC_CURRENT_3.Text;
                    querystock.OVC_NSTOCK_3 = txtSecOVC_NSTOCK_3.Text;
                    if (!strONB_MONEY_3.Equals(string.Empty))
                    {
                        int onbmoney3 = Convert.ToInt32(strONB_MONEY_3);
                        querystock.ONB_MONEY_3 = onbmoney3;
                    }
                    int onbmoneynt3 = Convert.ToInt32(strONB_MONEY_NT_3);
                    querystock.ONB_MONEY_NT_3 = onbmoneynt3;
                    onball = onball + onbmoneynt3;
                }
                querystock.ONB_ALL_MONEY = onball;
                txtSecONB_ALL_MONEY.Text = onball.ToString();
                querystock.OVC_MARK = txtSecOVC_MARK.Text;
                querystock.OVC_WORK_NO = txtSecOVC_WORK_NO.Text;
                querystock.OVC_WORK_NAME = txtSecOVC_WORK_NAME.Text;
                querystock.OVC_WORK_UNIT = txtSecOVC_WORK_UNIT.Text;
                querystock.OVC_ONNAME = txtSecOVC_ONNAME.Text;
                querystock.OVC_RECEIVE_NO = txtSecOVC_RECEIVE_NO.Text;
                querystock.OVC_DRECEIVE = txtSecOVC_DRECEIVE.Text;
                querystock.OVC_COMPTROLLER_NO = txtSecOVC_COMPTROLLER_NO.Text;
                querystock.OVC_DCOMPTROLLER = txtSecOVC_DCOMPTROLLER.Text;
                querystock.OVC_DAPPROVE = txtSecOVC_DAPPROVE.Text;
                //var querystaa =
                //    (from tsta in mpms.TBMSTATUS
                //     where tsta.OVC_PURCH == strOVC_PURCH
                //     where tsta.OVC_PURCH_5 == strOVC_PURCH_5
                //     where tsta.OVC_STATUS == "28"
                //     select tsta).FirstOrDefault();
                //if (querystaa == null && !txtSecOVC_DAPPROVE.Text.Equals(string.Empty))
                //{
                //    TBMSTATUS sta = new TBMSTATUS();
                //    sta.OVC_PURCH_5 = strOVC_PURCH_5;
                //    sta.OVC_PURCH = strOVC_PURCH;
                //    sta.ONB_TIMES = 1;
                //    sta.OVC_DO_NAME = ViewState["OVC_DO_NAME"].ToString();
                //    sta.OVC_STATUS = "28";
                //    sta.OVC_DBEGIN = DateTime.Now.ToString("yyyy-MM-dd");
                //    sta.OVC_STATUS_SN = Guid.NewGuid();
                //    mpms.TBMSTATUS.Add(sta);
                //
                //    var querysta =
                //        (from tsta in mpms.TBMSTATUS
                //         where tsta.OVC_PURCH == strOVC_PURCH
                //         where tsta.OVC_PURCH_5 == strOVC_PURCH_5
                //         where tsta.OVC_STATUS == "27"
                //         select tsta).FirstOrDefault();
                //    querysta.OVC_DEND = DateTime.Now.ToString("yyyy-MM-dd");
                //}
                //else
                //{
                //    short onbtime = Convert.ToInt16(querystaa.ONB_TIMES + 1);
                //    querystaa.ONB_TIMES = onbtime;
                //}
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), querystock.GetType().Name.ToString(), this, "修改");
                FCommon.AlertShow(PnMessageS, "success", "系統訊息", "購案合約編號" + strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5 + strOVC_PURCH_6 + "存檔成功!!");
            }
            else
                FCommon.AlertShow(PnMessageS, "danger", "系統訊息", strMessage);
        }


        private String GetTbm1407Desc(string cateID, string codeID)
        {
            //將TBM1407片語ID代碼轉為Desc
            TBM1407 tbm1407 = new TBM1407();
            if (codeID != null && codeID != "")
            {
                tbm1407 = gm.TBM1407.Where(tb => tb.OVC_PHR_CATE.Equals(cateID) && tb.OVC_PHR_ID.Equals(codeID)).OrderBy(tb => tb.OVC_PHR_ID).FirstOrDefault();
                if (tbm1407 != null)
                {
                    return tbm1407.OVC_PHR_DESC.ToString();
                }
            }
            return codeID;
        }


        #endregion



        #endregion

        #region drpevent
        protected void drpDocOVC_ONNAME_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDocOVC_ONNAME.Text = drpDocOVC_ONNAME.SelectedItem.Text;
        }

        protected void drpSecOVC_ONNAME_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSecOVC_ONNAME.Text = drpSecOVC_ONNAME.SelectedItem.Text;
        }

        protected void drpEscOVC_ONNAME_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtEscOVC_ONNAME.Text = drpEscOVC_ONNAME.SelectedItem.Text;
        }
        #endregion

        #region 收入通知單預覽列印_Cash
        private string Cash_Income_ExportToWord()
        {
            decimal money = 0;
            string Money = "";
            string path = "";
            var purch = lblEscOVC_PURCH_6.Text.Substring(0, 7);
            path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/ReceivePurchaseServletE1D_1.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryCash =
                        from cash in mpms.TBMMANAGE_CASH
                        where cash.OVC_PURCH.Equals(purch)
                        where cash.OVC_OWN_NAME.Equals(txtEscOVC_OWN_NAME.Text)
                        select new
                        {
                            OVC_OWN_NAME = cash.OVC_OWN_NAME,
                            OVC_OWN_NO = cash.OVC_OWN_NO,
                            OVC_OWN_ADDRESS = cash.OVC_OWN_ADDRESS,
                            OVC_OWN_TEL = cash.OVC_OWN_TEL,
                            OVC_REASON_1 = cash.OVC_REASON_1,
                            OVC_CURRENT_1 = cash.OVC_CURRENT_1,
                            ONB_MONEY_1 = cash.ONB_MONEY_1,
                            ONB_MONEY_NT_1 = cash.ONB_MONEY_NT_1,
                            OVC_REASON_2 = cash.OVC_REASON_2,
                            OVC_CURRENT_2 = cash.OVC_CURRENT_2,
                            ONB_MONEY_2 = cash.ONB_MONEY_2,
                            ONB_MONEY_NT_2 = cash.ONB_MONEY_NT_2,
                            OVC_REASON_3 = cash.OVC_REASON_3,
                            OVC_CURRENT_3 = cash.OVC_CURRENT_3,
                            ONB_MONEY_3 = cash.ONB_MONEY_3,
                            ONB_MONEY_NT_3 = cash.ONB_MONEY_NT_3,
                            ONB_ALL_MONEY = cash.ONB_ALL_MONEY,
                            OVC_MARK = cash.OVC_MARK,
                            OVC_WORK_NO = cash.OVC_WORK_NO,
                            OVC_WORK_NAME = cash.OVC_WORK_NAME,
                            OVC_WORK_UNIT = cash.OVC_WORK_UNIT,
                            OVC_RECEIVE_NO = cash.OVC_RECEIVE_NO,
                            OVC_DRECEIVE = cash.OVC_DRECEIVE
                        };
                    foreach (var q in queryCash)
                    {
                        if (q.OVC_OWN_NAME != null)
                            doc.ReplaceText("[$OWN_NAME$]", q.OVC_OWN_NAME, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OWN_NAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_OWN_NO != null)
                            doc.ReplaceText("[$OWN_NO$]", q.OVC_OWN_NO, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OWN_NO$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_OWN_ADDRESS != null)
                            doc.ReplaceText("[$OWN_ADDRESS$]", q.OVC_OWN_ADDRESS, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OWN_ADDRESS$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_OWN_TEL != null)
                            doc.ReplaceText("[$OWN_TEL$]", q.OVC_OWN_TEL, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OWN_TEL$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_REASON_1 != null)
                            doc.ReplaceText("[$REASON_1$]", q.OVC_REASON_1, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$REASON_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_MONEY_1.ToString() != null)
                        {
                            doc.ReplaceText("[$MONEY_1$]", String.Format("{0:N}", q.ONB_MONEY_1), false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$MONEY_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_MONEY_NT_1.ToString() != null)
                        {
                            doc.ReplaceText("[$MONEY_NT_1$]", String.Format("{0:N}", q.ONB_MONEY_NT_1), false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$MONEY_NT_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_REASON_2 != null)
                            doc.ReplaceText("[$REASON_2$]", q.OVC_REASON_2, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$REASON_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_MONEY_2.ToString() != null)
                        {
                            doc.ReplaceText("[$MONEY_2$]", String.Format("{0:N}", q.ONB_MONEY_2), false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$MONEY_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_MONEY_NT_2.ToString() != null)
                        {
                            doc.ReplaceText("[$MONEY_NT_2$]", String.Format("{0:N}", q.ONB_MONEY_NT_2), false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$MONEY_NT_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_REASON_3 != null)
                            doc.ReplaceText("[$REASON_3$]", q.OVC_REASON_3, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$REASON_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_MONEY_3.ToString() != null)
                        {
                            doc.ReplaceText("[$MONEY_3$]", String.Format("{0:N}", q.ONB_MONEY_3), false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$MONEY_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_MONEY_NT_3.ToString() != null)
                        {
                            doc.ReplaceText("[$MONEY_NT_3$]", String.Format("{0:N}", q.ONB_MONEY_NT_3), false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$MONEY_NT_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        var query1407 =
                            from tbm1407 in mpms.TBM1407
                            where tbm1407.OVC_PHR_CATE.Equals("B0")
                            select new
                            {
                                OVC_PHR_ID = tbm1407.OVC_PHR_ID,
                                OVC_PHR_DESC = tbm1407.OVC_PHR_DESC
                            };
                        foreach (var qu in query1407)
                        {
                            if (q.OVC_CURRENT_1 == qu.OVC_PHR_ID)
                                doc.ReplaceText("[$CURRENT_1$]", qu.OVC_PHR_DESC, false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.OVC_CURRENT_2 == qu.OVC_PHR_ID)
                                doc.ReplaceText("[$CURRENT_2$]", qu.OVC_PHR_DESC, false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.OVC_CURRENT_3 == qu.OVC_PHR_ID)
                                doc.ReplaceText("[$CURRENT_3$]", qu.OVC_PHR_DESC, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$CURRENT_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CURRENT_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CURRENT_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ALL_MONEY != null)
                        {
                            money = decimal.Parse(q.ONB_ALL_MONEY.ToString());
                            Money = EastAsiaNumericFormatter.FormatWithCulture("Lc", money, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                            doc.ReplaceText("[$ALL_MONEY$]", Money, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$ALL_MONEY$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_MARK != null)
                            doc.ReplaceText("[$txtOVC_MARK$]", q.OVC_MARK, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$txtOVC_MARK$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_WORK_NO != null)
                            doc.ReplaceText("[$WORK_NO$]", q.OVC_WORK_NO, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$WORK_NO$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_WORK_NAME != null)
                            doc.ReplaceText("[$WORK_NAME$]", q.OVC_WORK_NAME, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$WORK_NAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_WORK_UNIT != null)
                            doc.ReplaceText("[$WORK_UNIT$]", q.OVC_WORK_UNIT, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$WORK_UNIT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_RECEIVE_NO != null)
                            doc.ReplaceText("[$RECEIVE_NO$]", q.OVC_RECEIVE_NO, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$RECEIVE_NO$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DRECEIVE != null)
                        {
                            int year = int.Parse(q.OVC_DRECEIVE.Substring(0, 4)) - 1911;
                            string date = year.ToString() + "年" + q.OVC_DRECEIVE.Substring(5, 2) + "月" + q.OVC_DRECEIVE.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DRECEIVE$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$OVC_DRECEIVE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }

                    doc.SaveAs(Request.PhysicalApplicationPath + "Tempprint/b.docx");
                }
                buffer = ms.ToArray();
            }
            string path_d = Path.Combine(Request.PhysicalApplicationPath, "Tempprint/b.docx");
            return path_d;
        }
        #endregion

        #region 收入通知單預覽列印_Prom
        private string Prom_Income_ExportToWord()
        {
            decimal money = 0;
            string Money = "";
            string path = "";
            var purch = lblDocOVC_PURCH_6.Text.Substring(0, 7);
            path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/ReceivePurchaseServletE1E_1.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryProm =
                        from prom in mpms.TBMMANAGE_PROM
                        where prom.OVC_PURCH.Equals(purch)
                        where prom.OVC_OWN_NAME.Equals(txtDocOVC_OWN_NAME.Text)
                        select new
                        {
                            OVC_OWN_NAME = prom.OVC_OWN_NAME,
                            OVC_OWNED_NAME = prom.OVC_OWNED_NAME,
                            OVC_OWNED_NO = prom.OVC_OWNED_NO,
                            OVC_OWN_NO = prom.OVC_OWN_NO,
                            OVC_OWN_ADDRESS = prom.OVC_OWN_ADDRESS,
                            OVC_OWN_TEL = prom.OVC_OWN_TEL,
                            OVC_REASON_1 = prom.OVC_REASON_1,
                            OVC_NSTOCK_1 = prom.OVC_NSTOCK_1,
                            OVC_NUMBER_1 = prom.OVC_NUMBER_1,
                            ONB_MONEY_1 = prom.ONB_MONEY_1,
                            OVC_REASON_2 = prom.OVC_REASON_2,
                            OVC_NSTOCK_2 = prom.OVC_NSTOCK_2,
                            OVC_NUMBER_2 = prom.OVC_NUMBER_2,
                            ONB_MONEY_2 = prom.ONB_MONEY_2,
                            OVC_REASON_3 = prom.OVC_REASON_3,
                            OVC_NSTOCK_3 = prom.OVC_NSTOCK_3,
                            OVC_NUMBER_3 = prom.OVC_NUMBER_3,
                            ONB_MONEY_3 = prom.ONB_MONEY_3,
                            ONB_ALL_MONEY = prom.ONB_ALL_MONEY,
                            OVC_MARK = prom.OVC_MARK,
                            OVC_WORK_NO = prom.OVC_WORK_NO,
                            OVC_WORK_NAME = prom.OVC_WORK_NAME,
                            OVC_WORK_UNIT = prom.OVC_WORK_UNIT,
                            OVC_RECEIVE_NO = prom.OVC_RECEIVE_NO,
                            OVC_DRECEIVE = prom.OVC_DRECEIVE,
                            OVC_CURRENT = prom.OVC_CURRENT
                        };
                    foreach (var q in queryProm)
                    {
                        if (q.OVC_OWN_NAME != null)
                            doc.ReplaceText("[$OWN_NAME$]", q.OVC_OWN_NAME, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OWN_NAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_OWNED_NAME != null)
                            doc.ReplaceText("[$OWNED_NAME$]", q.OVC_OWNED_NAME, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OWNED_NAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_OWNED_NO != null)
                            doc.ReplaceText("[$OWNED_NO$]", q.OVC_OWNED_NO, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OWNED_NO$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_OWN_ADDRESS != null)
                            doc.ReplaceText("[$OWN_ADDRESS$]", q.OVC_OWN_ADDRESS, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OWN_ADDRESS$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_OWN_ADDRESS != null)
                            doc.ReplaceText("[$OWNED_ADDRESS$]", q.OVC_OWN_ADDRESS, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OWNED_ADDRESS$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_REASON_1 != null)
                            doc.ReplaceText("[$REASON_1$]", q.OVC_REASON_1, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$REASON_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_NSTOCK_1 != null)
                            doc.ReplaceText("[$NSTOCK_1$]", q.OVC_NSTOCK_1, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$NSTOCK_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_NUMBER_1 != null)
                            doc.ReplaceText("[$NUMBER_1$]", q.OVC_NUMBER_1, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$NUMBER_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_MONEY_1.ToString() != null)
                        {
                            doc.ReplaceText("[$MONEY_1$]", String.Format("{0:N}", q.ONB_MONEY_1), false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$MONEY_1$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_REASON_2 != null)
                            doc.ReplaceText("[$REASON_2$]", q.OVC_REASON_2, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$REASON_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_NSTOCK_2 != null)
                            doc.ReplaceText("[$NSTOCK_2$]", q.OVC_NSTOCK_2, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$NSTOCK_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_NUMBER_2 != null)
                            doc.ReplaceText("[$NUMBER_2$]", q.OVC_NUMBER_2, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$NUMBER_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_MONEY_2 != null)
                        {
                            doc.ReplaceText("[$MONEY_2$]", String.Format("{0:N}", q.ONB_MONEY_2), false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$MONEY_2$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_REASON_3 != null)
                            doc.ReplaceText("[$REASON_3$]", q.OVC_REASON_3, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$REASON_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_NSTOCK_3 != null)
                            doc.ReplaceText("[$NSTOCK_3$]", q.OVC_NSTOCK_3, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$NSTOCK_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_NUMBER_3 != null)
                            doc.ReplaceText("[$NUMBER_3$]", q.OVC_NUMBER_3, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$NUMBER_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_MONEY_3 != null)
                        {
                            doc.ReplaceText("[$MONEY_3$]", String.Format("{0:N}", q.ONB_MONEY_3), false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$MONEY_3$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                        var query1407 =
                            from tbm1407 in mpms.TBM1407
                            where tbm1407.OVC_PHR_CATE.Equals("B0")
                            where tbm1407.OVC_PHR_ID.Equals(q.OVC_CURRENT)
                            select new
                            {
                                OVC_PHR_ID = tbm1407.OVC_PHR_ID,
                                OVC_PHR_DESC = tbm1407.OVC_PHR_DESC
                            };
                        foreach (var qu in query1407)
                        {
                            doc.ReplaceText("[$CURRENT$]", q.OVC_CURRENT, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$CURRENT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ALL_MONEY != null)
                        {
                            money = decimal.Parse(q.ONB_ALL_MONEY.ToString());
                            Money = EastAsiaNumericFormatter.FormatWithCulture("Lc", money, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                            doc.ReplaceText("[$ALL_MONEY$]", Money, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$ALL_MONEY$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_MARK != null)
                            doc.ReplaceText("[$OVC_MARK$]", q.OVC_MARK, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OVC_MARK$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_WORK_NO != null)
                            doc.ReplaceText("[$WORK_NO$]", q.OVC_WORK_NO, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$WORK_NO$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_WORK_NAME != null)
                            doc.ReplaceText("[$WORK_NAME$]", q.OVC_WORK_NAME, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$WORK_NAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_WORK_UNIT != null)
                            doc.ReplaceText("[$WORK_UNIT$]", q.OVC_WORK_UNIT, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$WORK_UNIT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_RECEIVE_NO != null)
                            doc.ReplaceText("[$RECEIVE_NO$]", q.OVC_RECEIVE_NO, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$RECEIVE_NO$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DRECEIVE != null)
                        {
                            int year = int.Parse(q.OVC_DRECEIVE.Substring(0, 4)) - 1911;
                            string date = year.ToString() + "年" + q.OVC_DRECEIVE.Substring(5, 2) + "月" + q.OVC_DRECEIVE.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DRECEIVE$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$OVC_DRECEIVE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.SaveAs(Request.PhysicalApplicationPath + "Tempprint/b.docx");
                }
                buffer = ms.ToArray();
            }
            string path_d = Path.Combine(Request.PhysicalApplicationPath, "Tempprint/b.docx");
            return path_d;
        }
        #endregion

        #region 收入通知單預覽列印_Stock
        private string Stock_Income_ExportToWord()
        {
            decimal money = 0;
            string Money = "";
            string path = "";
            var purch = lblSecOVC_PURCH_6.Text.Substring(0, 7);
            path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/ReceivePurchaseServletE20_1.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryStock =
                           from stock in mpms.TBMMANAGE_STOCK
                           where stock.OVC_PURCH.Equals(purch)
                           where stock.OVC_OWN_NAME.Equals(txtSecOVC_OWN_NAME.Text)
                           select new
                           {
                               OVC_OWN_NAME = stock.OVC_OWN_NAME,
                               OVC_OWN_NO = stock.OVC_OWN_NO,
                               OVC_OWN_ADDRESS = stock.OVC_OWN_ADDRESS,
                               OVC_OWN_TEL = stock.OVC_OWN_TEL,
                               OVC_REASON_1 = stock.OVC_REASON_1,
                               OVC_NSTOCK_1 = stock.OVC_NSTOCK_1,
                               OVC_CURRENT_1 = stock.OVC_CURRENT_1,
                               ONB_MONEY_1 = stock.ONB_MONEY_1,
                               ONB_MONEY_NT_1 = stock.ONB_MONEY_NT_1,
                               OVC_REASON_2 = stock.OVC_REASON_2,
                               OVC_NSTOCK_2 = stock.OVC_NSTOCK_2,
                               OVC_CURRENT_2 = stock.OVC_CURRENT_2,
                               ONB_MONEY_2 = stock.ONB_MONEY_2,
                               ONB_MONEY_NT_2 = stock.ONB_MONEY_NT_2,
                               OVC_REASON_3 = stock.OVC_REASON_3,
                               OVC_NSTOCK_3 = stock.OVC_NSTOCK_3,
                               OVC_CURRENT_3 = stock.OVC_CURRENT_3,
                               ONB_MONEY_3 = stock.ONB_MONEY_3,
                               ONB_MONEY_NT_3 = stock.ONB_MONEY_NT_3,
                               ONB_ALL_MONEY = stock.ONB_ALL_MONEY,
                               OVC_MARK = stock.OVC_MARK,
                               OVC_WORK_NO = stock.OVC_WORK_NO,
                               OVC_WORK_NAME = stock.OVC_WORK_NAME,
                               OVC_WORK_UNIT = stock.OVC_WORK_UNIT,
                               OVC_RECEIVE_NO = stock.OVC_RECEIVE_NO,
                               OVC_DRECEIVE = stock.OVC_DRECEIVE
                           };
                    foreach (var q in queryStock)
                    {
                        if (q.OVC_OWN_NAME != null)
                            doc.ReplaceText("[$OWN_NAME$]", q.OVC_OWN_NAME, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OWN_NAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_OWN_NO != null)
                            doc.ReplaceText("[$OWN_NO$]", q.OVC_OWN_NO, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OWN_NO$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_OWN_ADDRESS != null)
                            doc.ReplaceText("[$OWN_ADDRESS$]", q.OVC_OWN_ADDRESS, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OWN_ADDRESS$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_OWN_TEL != null)
                            doc.ReplaceText("[$OWN_TEL$]", q.OVC_OWN_TEL, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OWN_TEL$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_REASON_1 != null)
                            doc.ReplaceText("[$REASON_1$]", q.OVC_REASON_1, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$REASON_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_NSTOCK_1 != null)
                            doc.ReplaceText("[$NSTOCK_1$]", q.OVC_NSTOCK_1, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$NSTOCK_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_MONEY_1 != null)
                        {
                            money = decimal.Parse(q.ONB_MONEY_1.ToString());
                            doc.ReplaceText("[$MONEY_1$]", String.Format("{0:N}", money), false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$MONEY_1$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_MONEY_NT_1 != null)
                        {
                            money = decimal.Parse(q.ONB_MONEY_NT_1.ToString());
                            doc.ReplaceText("[$MONEY_NT_1$]", String.Format("{0:N}", money), false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$MONEY_NT_1$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_REASON_2 != null)
                            doc.ReplaceText("[$REASON_2$]", q.OVC_REASON_2, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$REASON_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_NSTOCK_2 != null)
                            doc.ReplaceText("[$NSTOCK_2$]", q.OVC_NSTOCK_2, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$NSTOCK_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_MONEY_2 != null)
                        {
                            money = decimal.Parse(q.ONB_MONEY_2.ToString());
                            doc.ReplaceText("[$MONEY_2$]", String.Format("{0:N}", money), false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$MONEY_2$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_MONEY_NT_2 != null)
                        {
                            money = decimal.Parse(q.ONB_MONEY_NT_2.ToString());
                            doc.ReplaceText("[$MONEY_NT_2$]", String.Format("{0:N}", money), false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$MONEY_NT_2$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_REASON_3 != null)
                            doc.ReplaceText("[$REASON_3$]", q.OVC_REASON_3, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$REASON_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_NSTOCK_3 != null)
                            doc.ReplaceText("[$NSTOCK_3$]", q.OVC_NSTOCK_3, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$NSTOCK_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_MONEY_3 != null)
                        {
                            money = decimal.Parse(q.ONB_MONEY_3.ToString());
                            doc.ReplaceText("[$MONEY_3$]", String.Format("{0:N}", money), false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$MONEY_3$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_MONEY_NT_3 != null)
                        {
                            money = decimal.Parse(q.ONB_MONEY_NT_3.ToString());
                            doc.ReplaceText("[$MONEY_NT_3$]", String.Format("{0:N}", money), false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$MONEY_NT_3$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                        var query1407 =
                            from tbm1407 in mpms.TBM1407
                            where tbm1407.OVC_PHR_CATE.Equals("B0")
                            select new
                            {
                                OVC_PHR_ID = tbm1407.OVC_PHR_ID,
                                OVC_PHR_DESC = tbm1407.OVC_PHR_DESC
                            };
                        foreach (var qu in query1407)
                        {
                            if (q.OVC_CURRENT_1 == qu.OVC_PHR_ID)
                                doc.ReplaceText("[$CURRENT_1$]", qu.OVC_PHR_DESC, false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.OVC_CURRENT_2 == qu.OVC_PHR_ID)
                                doc.ReplaceText("[$CURRENT_2$]", qu.OVC_PHR_DESC, false, System.Text.RegularExpressions.RegexOptions.None);
                            if (q.OVC_CURRENT_3 == qu.OVC_PHR_ID)
                                doc.ReplaceText("[$CURRENT_3$]", qu.OVC_PHR_DESC, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$CURRENT_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CURRENT_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CURRENT_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ALL_MONEY != null)
                        {
                            money = decimal.Parse(q.ONB_ALL_MONEY.ToString());
                            Money = EastAsiaNumericFormatter.FormatWithCulture("Lc", money, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                            doc.ReplaceText("[$ALL_MONEY$]", Money, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$ALL_MONEY$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_MARK != null)
                            doc.ReplaceText("[$OVC_MARK$]", q.OVC_MARK, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$OVC_MARK$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_WORK_NO != null)
                            doc.ReplaceText("[$WORK_NO$]", q.OVC_WORK_NO, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$WORK_NO$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_WORK_NAME != null)
                            doc.ReplaceText("[$WORK_NAME$]", q.OVC_WORK_NAME, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$WORK_NAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_WORK_UNIT != null)
                            doc.ReplaceText("[$WORK_UNIT$]", q.OVC_WORK_UNIT, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$WORK_UNIT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_RECEIVE_NO != null)
                            doc.ReplaceText("[$RECEIVE_NO$]", q.OVC_RECEIVE_NO, false, System.Text.RegularExpressions.RegexOptions.None);
                        else
                            doc.ReplaceText("[$RECEIVE_NO$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DRECEIVE != null)
                        {
                            int year = int.Parse(q.OVC_DRECEIVE.Substring(0, 4)) - 1911;
                            string date = year.ToString() + "年" + q.OVC_DRECEIVE.Substring(5, 2) + "月" + q.OVC_DRECEIVE.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DRECEIVE$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        else
                            doc.ReplaceText("[$OVC_DRECEIVE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.SaveAs(Request.PhysicalApplicationPath + "Tempprint/b.docx");
                }
                buffer = ms.ToArray();
            }
            string path_d = Path.Combine(Request.PhysicalApplicationPath, "Tempprint/b.docx");
            return path_d;
        }
        #endregion

        #region 合約
        protected void lbtnContract_Click(object sender, EventArgs e)
        {
            var purch = ViewState["strOVC_PURCH"].ToString();
            string path_temp = Contract_ExportToWord();
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "合約.docx";
            string fileName = HttpUtility.UrlEncode(filepath);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            File.Delete(path_temp);
            Response.End();
        }

        protected void lbtnContract_odt_Click(object sender, EventArgs e)
        {
            var purch = ViewState["strOVC_PURCH"].ToString();
            string path_temp = Contract_ExportToWord();
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/Contract_odt.odt";
            string FileName = purch + "合約.odt";
            FCommon.WordToOdt(this, path_temp, filetemp, FileName);
        }

        private string Contract_ExportToWord()
        {
            int year = 0;
            string date = "";
            var purch = ViewState["strOVC_PURCH"].ToString();
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/國防部採購契約D1D.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryNSECTION = mpms.TBM1301
                        .Where(o => o.OVC_PURCH.Equals(purch)).FirstOrDefault();
                    doc.ReplaceText("[$OVC_PUR_NSECTION$]", queryNSECTION != null ? queryNSECTION.OVC_PUR_NSECTION : "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryPurch =
                        from tbm1301_plan in mpms.TBM1301_PLAN
                        join tbm1301 in mpms.TBM1301 on tbm1301_plan.OVC_PURCH equals tbm1301.OVC_PURCH
                        join tbm1302 in mpms.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                        where tbm1301.OVC_PURCH.Equals(purch)
                        select new
                        {
                            OVC_PURCH = tbm1301.OVC_PURCH,
                            OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                            OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                            OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                            OVC_DBID = tbm1302.OVC_DBID,
                            OVC_DCONTRACT = tbm1302.OVC_DCONTRACT,
                            OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE,
                            OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                            ONB_MONEY = tbm1302.ONB_MONEY,
                            OVC_VEN_BOSS = tbm1302.OVC_VEN_BOSS,
                            OVC_VEN_NAME = tbm1302.OVC_VEN_NAME,
                            OVC_VEN_FAX = tbm1302.OVC_VEN_FAX,
                            OVC_VEN_CELLPHONE = tbm1302.OVC_VEN_CELLPHONE,
                            OVC_VEN_ADDRESS = tbm1302.OVC_VEN_ADDRESS,
                            OVC_PUR_IUSER_PHONE = tbm1301.OVC_PUR_IUSER_PHONE,
                            OVC_PUR_IUSER_FAX = tbm1301_plan.OVC_PUR_IUSER_FAX,
                            OVC_PUR_FEE_OK = tbm1301.OVC_PUR_FEE_OK,
                            OVC_PUR_GOOD_OK = tbm1301.OVC_PUR_GOOD_OK,
                            OVC_PUR_TAX_OK = tbm1301.OVC_PUR_TAX_OK
                        };
                    foreach (var q in queryPurch)
                    {
                        doc.ReplaceText("[$PURCH$]", q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$PURCH_6$]", q.OVC_PURCH_6, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DBID != null)
                        {
                            year = int.Parse(q.OVC_DBID.Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DBID.Substring(5, 2) + "月" + q.OVC_DBID.Substring(8, 2) + "日";
                            doc.ReplaceText("[$DBID$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$DBID$]", "   年  月  日", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DCONTRACT != null)
                        {
                            year = int.Parse(q.OVC_DCONTRACT.Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DCONTRACT.Substring(5, 2) + "月" + q.OVC_DCONTRACT.Substring(8, 2) + "日";
                            doc.ReplaceText("[$DCONTRACT$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$DCONTRACT$]", "   年  月  日", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$VEN_TITLE$]", q.OVC_VEN_TITLE != null ? q.OVC_VEN_TITLE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$NAME$]", q.OVC_PUR_IPURCH != null ? q.OVC_PUR_IPURCH : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$MONEY$]", q.ONB_MONEY != null ? q.ONB_MONEY.ToString() : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_MONEY != null)
                        {
                            string money = EastAsiaNumericFormatter.FormatWithCulture("Lc", q.ONB_MONEY, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                            doc.ReplaceText("[$CH_MONEY$]", money, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$CH_MONEY$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$PRINCIPAL$]", q.OVC_VEN_BOSS != null ? q.OVC_VEN_BOSS : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CONTACT$]", q.OVC_VEN_NAME != null ? q.OVC_VEN_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$FAX$]", q.OVC_VEN_FAX != null ? q.OVC_VEN_FAX : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CELLPHONE$]", q.OVC_VEN_CELLPHONE != null ? q.OVC_VEN_CELLPHONE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$ADDRESS$]", q.OVC_VEN_ADDRESS != null ? q.OVC_VEN_ADDRESS : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$IUSER_PHONE$]", q.OVC_PUR_IUSER_PHONE != null ? q.OVC_PUR_IUSER_PHONE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$IUSER_FAX$]", q.OVC_PUR_IUSER_FAX != null ? q.OVC_PUR_IUSER_FAX : "", false, System.Text.RegularExpressions.RegexOptions.None);

                        string tax = "";
                        if (q.OVC_PUR_GOOD_OK.Equals("N") && q.OVC_PUR_FEE_OK.Equals("N") && q.OVC_PUR_TAX_OK.Equals("N")) tax = "(含貨物稅、含關稅及進口營業稅)";
                        if (!q.OVC_PUR_GOOD_OK.Equals("N") && q.OVC_PUR_FEE_OK.Equals("N") && q.OVC_PUR_TAX_OK.Equals("N")) tax = "(不含貨物稅、含關稅及進口營業稅)";
                        if (q.OVC_PUR_GOOD_OK.Equals("N") && !q.OVC_PUR_FEE_OK.Equals("N") && q.OVC_PUR_TAX_OK.Equals("N")) tax = "(含貨物稅及進口營業稅、不含關稅)";
                        if (q.OVC_PUR_GOOD_OK.Equals("N") && q.OVC_PUR_FEE_OK.Equals("N") && !q.OVC_PUR_TAX_OK.Equals("N")) tax = "(含貨物稅及關稅、不含進口營業稅)";
                        if (!q.OVC_PUR_GOOD_OK.Equals("N") && !q.OVC_PUR_FEE_OK.Equals("N") && q.OVC_PUR_TAX_OK.Equals("N")) tax = "(不含貨物稅及關稅、含進口營業稅)";
                        if (!q.OVC_PUR_GOOD_OK.Equals("N") && q.OVC_PUR_FEE_OK.Equals("N") && !q.OVC_PUR_TAX_OK.Equals("N")) tax = "(不含貨物稅及進口營業稅、含關稅)";
                        if (q.OVC_PUR_GOOD_OK.Equals("N") && !q.OVC_PUR_FEE_OK.Equals("N") && !q.OVC_PUR_TAX_OK.Equals("N")) tax = "(含貨物稅、不含關稅及進口營業稅)";
                        if (!q.OVC_PUR_GOOD_OK.Equals("N") && !q.OVC_PUR_FEE_OK.Equals("N") && !q.OVC_PUR_TAX_OK.Equals("N")) tax = "(不含貨物稅、不含關稅及進口營業稅)";
                        doc.ReplaceText("[$TAX$]", tax, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    var queryCurrency =
                        from tbm1302 in mpms.TBM1302
                        join tbm1407 in mpms.TBM1407
                            on tbm1302.OVC_CURRENT equals tbm1407.OVC_PHR_ID
                        where tbm1302.OVC_PURCH.Equals(purch)
                        where tbm1302.OVC_PURCH_6.Equals(purch)
                        where tbm1407.OVC_PHR_CATE.Equals("B0")
                        select tbm1407.OVC_PHR_DESC;
                    if (queryCurrency.Count() > 0)
                    {
                        foreach (var q in queryCurrency)
                        {
                            doc.ReplaceText("[$Currency$]", q, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                    doc.ReplaceText("[$Currency$]", "新台幣", false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryTime =
                        from tbm1220_1 in mpms.TBM1220_1
                        where tbm1220_1.OVC_PURCH.Equals(purch)
                        where tbm1220_1.OVC_IKIND.Equals("D56")
                        select tbm1220_1.OVC_MEMO;
                    if (queryTime.Count() > 0)
                    {
                        foreach (var q in queryTime)
                        {
                            doc.ReplaceText("[$TIME$]", q, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                    doc.ReplaceText("[$TIME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryPlace =
                        from tbm1220_1 in mpms.TBM1220_1
                        where tbm1220_1.OVC_PURCH.Equals(purch)
                        where tbm1220_1.OVC_IKIND.Equals("D57")
                        select tbm1220_1.OVC_MEMO;
                    if (queryPlace.Count() > 0)
                    {
                        foreach (var q in queryPlace)
                        {
                            doc.ReplaceText("[$PLACE$]", q, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                    doc.ReplaceText("[$PLACE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryWay =
                        from tbm1220_1 in mpms.TBM1220_1
                        where tbm1220_1.OVC_PURCH.Equals(purch)
                        where tbm1220_1.OVC_IKIND.Equals("D5B")
                        select tbm1220_1.OVC_MEMO;
                    if (queryWay.Count() > 0)
                    {
                        foreach (var q in queryWay)
                        {
                            doc.ReplaceText("[$WAY$]", q, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                    #region 抓不到資料顯示空白
                    doc.ReplaceText("[$PURCH$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$PURCH_6$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$DBID$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$DCONTRACT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$VEN_TITLE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$NAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$MONEY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$TAX$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$CH_MONEY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$TAX$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$VEN_TITLE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$PRINCIPAL$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$CONTACT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$CELLPHONE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$FAX$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$ADDRESS$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$IUSER_PHONE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$IUSER_FAX$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$WAY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    #endregion
                    doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/Contract_Temp.docx");
                }
                buffer = ms.ToArray();
            }
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Contract_Temp.docx");
            return path_temp;
        }
        #endregion

        #region 印信申請表
        protected void lbtnLetterApplicationForm_Click(object sender, EventArgs e)
        {
            var purch = ViewState["strOVC_PURCH"].ToString();
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/印信申請表D1D.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryPurch =
                        from tbm1301 in mpms.TBM1301_PLAN
                        join tbm1302 in mpms.TBM1302
                            on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                        where tbm1301.OVC_PURCH.Equals(purch)
                        select new
                        {
                            OVC_PURCH = tbm1301.OVC_PURCH,
                            OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                            OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                            OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                            OVC_DBID = tbm1302.OVC_DBID,
                            OVC_DCONTRACT = tbm1302.OVC_DCONTRACT,
                            OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE,
                            OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                            ONB_MONEY = tbm1302.ONB_MONEY,
                            OVC_VEN_BOSS = tbm1302.OVC_VEN_BOSS,
                            OVC_VEN_NAME = tbm1302.OVC_VEN_NAME,
                            OVC_VEN_FAX = tbm1302.OVC_VEN_FAX,
                            OVC_VEN_CELLPHONE = tbm1302.OVC_VEN_CELLPHONE,
                            OVC_VEN_ADDRESS = tbm1302.OVC_VEN_ADDRESS,
                            OVC_PUR_IUSER_PHONE = tbm1301.OVC_PUR_IUSER_PHONE,
                            OVC_PUR_IUSER_FAX = tbm1301.OVC_PUR_IUSER_FAX,
                            OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION
                        };
                    foreach (var q in queryPurch)
                    {
                        doc.ReplaceText("[$OVC_PURCH$]", q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_VEN_TITLE$]", q.OVC_VEN_TITLE, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_IPURCH$]", q.OVC_PUR_IPURCH, false, System.Text.RegularExpressions.RegexOptions.None);
                    }

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/LAF_Temp.docx");
                }
                buffer = ms.ToArray();
            }
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/LAF_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "印信申請表.docx";
            string fileName = HttpUtility.UrlEncode(filepath);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            File.Delete(path_temp);
            Response.End();
        }

        protected void lbtnLetterApplicationForm_odt_Click(object sender, EventArgs e)
        {
            var purch = ViewState["strOVC_PURCH"].ToString();
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/印信申請表D1D.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryPurch =
                        from tbm1301 in mpms.TBM1301_PLAN
                        join tbm1302 in mpms.TBM1302
                            on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                        where tbm1301.OVC_PURCH.Equals(purch)
                        select new
                        {
                            OVC_PURCH = tbm1301.OVC_PURCH,
                            OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                            OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                            OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                            OVC_DBID = tbm1302.OVC_DBID,
                            OVC_DCONTRACT = tbm1302.OVC_DCONTRACT,
                            OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE,
                            OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                            ONB_MONEY = tbm1302.ONB_MONEY,
                            OVC_VEN_BOSS = tbm1302.OVC_VEN_BOSS,
                            OVC_VEN_NAME = tbm1302.OVC_VEN_NAME,
                            OVC_VEN_FAX = tbm1302.OVC_VEN_FAX,
                            OVC_VEN_CELLPHONE = tbm1302.OVC_VEN_CELLPHONE,
                            OVC_VEN_ADDRESS = tbm1302.OVC_VEN_ADDRESS,
                            OVC_PUR_IUSER_PHONE = tbm1301.OVC_PUR_IUSER_PHONE,
                            OVC_PUR_IUSER_FAX = tbm1301.OVC_PUR_IUSER_FAX,
                            OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION
                        };
                    foreach (var q in queryPurch)
                    {
                        doc.ReplaceText("[$OVC_PURCH$]", q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_VEN_TITLE$]", q.OVC_VEN_TITLE, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_IPURCH$]", q.OVC_PUR_IPURCH, false, System.Text.RegularExpressions.RegexOptions.None);
                    }

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/LAF_Temp.docx");
                }
                buffer = ms.ToArray();
            }
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/LAF_Temp.docx");
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/LAF_Temp.odt";
            string FileName = purch + "印信申請表.odt";
            FCommon.WordToOdt(this, path_temp, filetemp, FileName);
        }
        #endregion

        #region 連帶保證書
        protected void lbtnJointGuarantee_Click(object sender, EventArgs e)
        {
            var purch = ViewState["strOVC_PURCH"].ToString();
            var purch_6 = ViewState["strOVC_PURCH_6_D"].ToString();
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/連帶保證書D1D.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryProm =
                        from prom in mpms.TBMMANAGE_PROM
                        where prom.OVC_PURCH.Equals(purch)
                        where prom.OVC_PURCH_6.Equals(purch_6)
                        where prom.OVC_OWN_NAME.Equals(txtDocOVC_OWN_NAME.Text)
                        select new
                        {
                            OVC_OWN_NAME = prom.OVC_OWN_NAME,
                            OVC_OWNED_NAME = prom.OVC_OWNED_NAME,
                            OVC_OWNED_NO = prom.OVC_OWNED_NO,
                            OVC_OWN_NO = prom.OVC_OWN_NO,
                            OVC_OWN_ADDRESS = prom.OVC_OWN_ADDRESS,
                            OVC_OWNED_ADDRESS = prom.OVC_OWNED_ADDRESS,
                            OVC_OWN_TEL = prom.OVC_OWN_TEL,
                            OVC_REASON_1 = prom.OVC_REASON_1,
                            OVC_NSTOCK_1 = prom.OVC_NSTOCK_1,
                            OVC_NUMBER_1 = prom.OVC_NUMBER_1,
                            ONB_MONEY_1 = prom.ONB_MONEY_1,
                            OVC_REASON_2 = prom.OVC_REASON_2,
                            OVC_NSTOCK_2 = prom.OVC_NSTOCK_2,
                            OVC_NUMBER_2 = prom.OVC_NUMBER_2,
                            ONB_MONEY_2 = prom.ONB_MONEY_2,
                            OVC_REASON_3 = prom.OVC_REASON_3,
                            OVC_NSTOCK_3 = prom.OVC_NSTOCK_3,
                            OVC_NUMBER_3 = prom.OVC_NUMBER_3,
                            ONB_MONEY_3 = prom.ONB_MONEY_3,
                            ONB_ALL_MONEY = prom.ONB_ALL_MONEY,
                            OVC_MARK = prom.OVC_MARK,
                            OVC_WORK_NO = prom.OVC_WORK_NO,
                            OVC_WORK_NAME = prom.OVC_WORK_NAME,
                            OVC_WORK_UNIT = prom.OVC_WORK_UNIT,
                            OVC_RECEIVE_NO = prom.OVC_RECEIVE_NO,
                            OVC_DRECEIVE = prom.OVC_DRECEIVE,
                            OVC_CURRENT = prom.OVC_CURRENT
                        };
                    foreach (var q in queryProm)
                    {
                        doc.ReplaceText("[$OVC_OWN_NAME$]", q.OVC_OWN_NAME != null ? q.OVC_OWN_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_OWN_ADDRESS$]", q.OVC_OWN_ADDRESS != null ?  q.OVC_OWN_ADDRESS : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_OWNED_NAME$]", q.OVC_OWNED_NAME != null ? q.OVC_OWNED_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_OWNED_ADDRESS$]", q.OVC_OWNED_ADDRESS != null ? q.OVC_OWNED_ADDRESS : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ALL_MONEY != null)
                        {
                            string money = EastAsiaNumericFormatter.FormatWithCulture("Lc", q.ONB_ALL_MONEY, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                            doc.ReplaceText("[$ONB_ALL_MONEY$]", money, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        //doc.ReplaceText("[$ONB_ALL_MONEY$]", q.ONB_ALL_MONEY != null ? String.Format("{0:N}", q.ONB_ALL_MONEY) : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_MARK$]", q.OVC_MARK != null ? q.OVC_MARK : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_WORK_NO$]", q.OVC_WORK_NO != null ? q.OVC_WORK_NO : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_WORK_NAME$]", q.OVC_WORK_NAME != null ? q.OVC_WORK_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_WORK_UNIT$]", q.OVC_WORK_UNIT != null ? q.OVC_WORK_UNIT : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_RECEIVE_NO$]", q.OVC_RECEIVE_NO != null ? q.OVC_RECEIVE_NO : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_REASON_1$]", q.OVC_REASON_1 != null ? q.OVC_REASON_1 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_NSTOCK_1$]", q.OVC_NSTOCK_1 != null ? q.OVC_NSTOCK_1 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_NUMBER_1$]", q.OVC_NUMBER_1 != null ? q.OVC_NUMBER_1 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$ONB_MONEY_1$]", q.ONB_MONEY_1 != null ? String.Format("{0:N}", q.ONB_MONEY_1) : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_REASON_2$]", q.OVC_REASON_2 != null ? q.OVC_REASON_2 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_NSTOCK_2$]", q.OVC_NSTOCK_2 != null ? q.OVC_NSTOCK_2 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_NUMBER_2$]", q.OVC_NUMBER_2 != null ? q.OVC_NUMBER_2 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$ONB_MONEY_2$]", q.ONB_MONEY_2 != null ? String.Format("{0:N}", q.ONB_MONEY_2) : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_REASON_3$]", q.OVC_REASON_3 != null ? q.OVC_REASON_3 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_NSTOCK_3$]", q.OVC_NSTOCK_3 != null ? q.OVC_NSTOCK_3 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_NUMBER_3$]", q.OVC_NUMBER_3 != null ? q.OVC_NUMBER_3 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$ONB_MONEY_3$]", q.ONB_MONEY_3 != null ? String.Format("{0:N}", q.ONB_MONEY_3) : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);

                        var groceryListTable = doc.Tables.FirstOrDefault(table => table.TableCaption == "table_1");
                        if (groceryListTable != null)
                        {
                            var rowPattern_1 = groceryListTable.Rows[4];
                            var rowPattern_2 = groceryListTable.Rows[5];
                            var rowPattern_3 = groceryListTable.Rows[6];
                            if (q.OVC_REASON_2 == null && q.OVC_NSTOCK_2 == null && q.OVC_NUMBER_2 == null && q.ONB_MONEY_2 == null)
                                rowPattern_2.Remove();
                            if (q.OVC_REASON_3 == null && q.OVC_NSTOCK_3 == null && q.OVC_NUMBER_3 == null && q.ONB_MONEY_3 == null)
                                rowPattern_3.Remove();
                        }
                    }
                    doc.ReplaceText("[$OVC_OWN_NAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_OWN_ADDRESS$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_OWNED_NAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_OWNED_ADDRESS$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$ONB_ALL_MONEY$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_MARK$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_WORK_NO$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_WORK_NAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_WORK_UNIT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_RECEIVE_NO$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_REASON_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_NSTOCK_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_NUMBER_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$ONB_MONEY_1$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_REASON_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_NSTOCK_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_NUMBER_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$ONB_MONEY_2$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_REASON_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_NSTOCK_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_NUMBER_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$ONB_MONEY_3$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/JG_Temp.docx");
                }
                buffer = ms.ToArray();
            }
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/JG_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "連帶保證書.docx";
            string fileName = HttpUtility.UrlEncode(filepath);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            File.Delete(path_temp);
            Response.End();
        }

        protected void lbtnJointGuarantee_odt_Click(object sender, EventArgs e)
        {
            var purch = ViewState["strOVC_PURCH"].ToString();
            var purch_6 = ViewState["strOVC_PURCH_6_D"].ToString();
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/連帶保證書D1D.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryProm =
                        from prom in mpms.TBMMANAGE_PROM
                        where prom.OVC_PURCH.Equals(purch)
                        where prom.OVC_PURCH_6.Equals(purch_6)
                        where prom.OVC_OWN_NAME.Equals(txtDocOVC_OWN_NAME.Text)
                        select new
                        {
                            OVC_OWN_NAME = prom.OVC_OWN_NAME,
                            OVC_OWNED_NAME = prom.OVC_OWNED_NAME,
                            OVC_OWNED_NO = prom.OVC_OWNED_NO,
                            OVC_OWN_NO = prom.OVC_OWN_NO,
                            OVC_OWN_ADDRESS = prom.OVC_OWN_ADDRESS,
                            OVC_OWNED_ADDRESS = prom.OVC_OWNED_ADDRESS,
                            OVC_OWN_TEL = prom.OVC_OWN_TEL,
                            OVC_REASON_1 = prom.OVC_REASON_1,
                            OVC_NSTOCK_1 = prom.OVC_NSTOCK_1,
                            OVC_NUMBER_1 = prom.OVC_NUMBER_1,
                            ONB_MONEY_1 = prom.ONB_MONEY_1,
                            OVC_REASON_2 = prom.OVC_REASON_2,
                            OVC_NSTOCK_2 = prom.OVC_NSTOCK_2,
                            OVC_NUMBER_2 = prom.OVC_NUMBER_2,
                            ONB_MONEY_2 = prom.ONB_MONEY_2,
                            OVC_REASON_3 = prom.OVC_REASON_3,
                            OVC_NSTOCK_3 = prom.OVC_NSTOCK_3,
                            OVC_NUMBER_3 = prom.OVC_NUMBER_3,
                            ONB_MONEY_3 = prom.ONB_MONEY_3,
                            ONB_ALL_MONEY = prom.ONB_ALL_MONEY,
                            OVC_MARK = prom.OVC_MARK,
                            OVC_WORK_NO = prom.OVC_WORK_NO,
                            OVC_WORK_NAME = prom.OVC_WORK_NAME,
                            OVC_WORK_UNIT = prom.OVC_WORK_UNIT,
                            OVC_RECEIVE_NO = prom.OVC_RECEIVE_NO,
                            OVC_DRECEIVE = prom.OVC_DRECEIVE,
                            OVC_CURRENT = prom.OVC_CURRENT
                        };
                    foreach (var q in queryProm)
                    {
                        doc.ReplaceText("[$OVC_OWN_NAME$]", q.OVC_OWN_NAME != null ? q.OVC_OWN_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_OWN_ADDRESS$]", q.OVC_OWN_ADDRESS != null ? q.OVC_OWN_ADDRESS : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_OWNED_NAME$]", q.OVC_OWNED_NAME != null ? q.OVC_OWNED_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_OWNED_ADDRESS$]", q.OVC_OWNED_ADDRESS != null ? q.OVC_OWNED_ADDRESS : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.ONB_ALL_MONEY != null)
                        {
                            string money = EastAsiaNumericFormatter.FormatWithCulture("Lc", q.ONB_ALL_MONEY, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                            doc.ReplaceText("[$ONB_ALL_MONEY$]", money, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        //doc.ReplaceText("[$ONB_ALL_MONEY$]", q.ONB_ALL_MONEY != null ? String.Format("{0:N}", q.ONB_ALL_MONEY) : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_MARK$]", q.OVC_MARK != null ? q.OVC_MARK : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_WORK_NO$]", q.OVC_WORK_NO != null ? q.OVC_WORK_NO : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_WORK_NAME$]", q.OVC_WORK_NAME != null ? q.OVC_WORK_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_WORK_UNIT$]", q.OVC_WORK_UNIT != null ? q.OVC_WORK_UNIT : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_RECEIVE_NO$]", q.OVC_RECEIVE_NO != null ? q.OVC_RECEIVE_NO : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_REASON_1$]", q.OVC_REASON_1 != null ? q.OVC_REASON_1 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_NSTOCK_1$]", q.OVC_NSTOCK_1 != null ? q.OVC_NSTOCK_1 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_NUMBER_1$]", q.OVC_NUMBER_1 != null ? q.OVC_NUMBER_1 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$ONB_MONEY_1$]", q.ONB_MONEY_1 != null ? String.Format("{0:N}", q.ONB_MONEY_1) : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_REASON_2$]", q.OVC_REASON_2 != null ? q.OVC_REASON_2 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_NSTOCK_2$]", q.OVC_NSTOCK_2 != null ? q.OVC_NSTOCK_2 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_NUMBER_2$]", q.OVC_NUMBER_2 != null ? q.OVC_NUMBER_2 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$ONB_MONEY_2$]", q.ONB_MONEY_2 != null ? String.Format("{0:N}", q.ONB_MONEY_2) : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_REASON_3$]", q.OVC_REASON_3 != null ? q.OVC_REASON_3 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_NSTOCK_3$]", q.OVC_NSTOCK_3 != null ? q.OVC_NSTOCK_3 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_NUMBER_3$]", q.OVC_NUMBER_3 != null ? q.OVC_NUMBER_3 : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$ONB_MONEY_3$]", q.ONB_MONEY_3 != null ? String.Format("{0:N}", q.ONB_MONEY_3) : "0.00", false, System.Text.RegularExpressions.RegexOptions.None);

                        var groceryListTable = doc.Tables.FirstOrDefault(table => table.TableCaption == "table_1");
                        if (groceryListTable != null)
                        {
                            var rowPattern_1 = groceryListTable.Rows[4];
                            var rowPattern_2 = groceryListTable.Rows[5];
                            var rowPattern_3 = groceryListTable.Rows[6];
                            if (q.OVC_REASON_2 == null && q.OVC_NSTOCK_2 == null && q.OVC_NUMBER_2 == null && q.ONB_MONEY_2 == null)
                                rowPattern_2.Remove();
                            if (q.OVC_REASON_3 == null && q.OVC_NSTOCK_3 == null && q.OVC_NUMBER_3 == null && q.ONB_MONEY_3 == null)
                                rowPattern_3.Remove();
                        }
                    }
                    doc.ReplaceText("[$OVC_OWN_NAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_OWN_ADDRESS$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_OWNED_NAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_OWNED_ADDRESS$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$ONB_ALL_MONEY$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_MARK$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_WORK_NO$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_WORK_NAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_WORK_UNIT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_RECEIVE_NO$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_REASON_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_NSTOCK_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_NUMBER_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$ONB_MONEY_1$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_REASON_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_NSTOCK_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_NUMBER_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$ONB_MONEY_2$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_REASON_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_NSTOCK_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_NUMBER_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$ONB_MONEY_3$]", "0.00", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/JG_Temp.docx");
                }
                buffer = ms.ToArray();
            }
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/JG_Temp.docx");
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/JG_Temp.odt";
            string FileName = purch + "連帶保證書.odt";
            FCommon.WordToOdt(this, path_temp, filetemp, FileName);
        }
        #endregion

        #region 傳真
        protected void lbtnFax_Click(object sender, EventArgs e)
        {
            var purch = ViewState["strOVC_PURCH"].ToString();
            var purch_6 = ViewState["strOVC_PURCH_6_D"].ToString();
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/傳真機使用申請表D1D.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryPurch =
                        from tbm1301 in mpms.TBM1301_PLAN
                        join tbm1302 in mpms.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                        where tbm1301.OVC_PURCH.Equals(purch)
                        where tbm1302.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            OVC_PURCH = tbm1301.OVC_PURCH,
                            OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                            OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                            OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                            OVC_DBID = tbm1302.OVC_DBID,
                            OVC_DCONTRACT = tbm1302.OVC_DCONTRACT,
                            OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE,
                            OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                            ONB_MONEY = tbm1302.ONB_MONEY,
                            OVC_VEN_BOSS = tbm1302.OVC_VEN_BOSS,
                            OVC_VEN_NAME = tbm1302.OVC_VEN_NAME,
                            OVC_VEN_FAX = tbm1302.OVC_VEN_FAX,
                            OVC_VEN_CELLPHONE = tbm1302.OVC_VEN_CELLPHONE,
                            OVC_VEN_ADDRESS = tbm1302.OVC_VEN_ADDRESS,
                            OVC_PUR_IUSER_PHONE = tbm1301.OVC_PUR_IUSER_PHONE,
                            OVC_PUR_IUSER_FAX = tbm1301.OVC_PUR_IUSER_FAX,
                            OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION
                        };
                    foreach (var q in queryPurch)
                    {
                        doc.ReplaceText("[$OVC_PURCH$]", q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_IPURCH$]", q.OVC_PUR_IPURCH, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_PURCH$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_PUR_IPURCH$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    var queryProm =
                        from prom in mpms.TBMMANAGE_PROM
                        where prom.OVC_PURCH.Equals(purch)
                        where prom.OVC_PURCH_6.Equals(purch_6)
                        where prom.OVC_OWN_NAME.Equals(txtDocOVC_OWN_NAME.Text)
                        select new
                        {
                            OVC_OWN_NAME = prom.OVC_OWN_NAME
                        };
                    foreach (var q in queryProm)
                    {
                        doc.ReplaceText("[$OVC_OWN_NAME$]", q.OVC_OWN_NAME != null ? q.OVC_OWN_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_OWN_NAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/Fax_Temp.docx");
                }
                buffer = ms.ToArray();
            }
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Fax_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "傳真使用申請表.docx";
            string fileName = HttpUtility.UrlEncode(filepath);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            File.Delete(path_temp);
            Response.End();
        }

        protected void lbtnFax_odt_Click(object sender, EventArgs e)
        {
            var purch = ViewState["strOVC_PURCH"].ToString();
            var purch_6 = ViewState["strOVC_PURCH_6_D"].ToString();
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/傳真機使用申請表D1D.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryPurch =
                        from tbm1301 in mpms.TBM1301_PLAN
                        join tbm1302 in mpms.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                        where tbm1301.OVC_PURCH.Equals(purch)
                        where tbm1302.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            OVC_PURCH = tbm1301.OVC_PURCH,
                            OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                            OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                            OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                            OVC_DBID = tbm1302.OVC_DBID,
                            OVC_DCONTRACT = tbm1302.OVC_DCONTRACT,
                            OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE,
                            OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                            ONB_MONEY = tbm1302.ONB_MONEY,
                            OVC_VEN_BOSS = tbm1302.OVC_VEN_BOSS,
                            OVC_VEN_NAME = tbm1302.OVC_VEN_NAME,
                            OVC_VEN_FAX = tbm1302.OVC_VEN_FAX,
                            OVC_VEN_CELLPHONE = tbm1302.OVC_VEN_CELLPHONE,
                            OVC_VEN_ADDRESS = tbm1302.OVC_VEN_ADDRESS,
                            OVC_PUR_IUSER_PHONE = tbm1301.OVC_PUR_IUSER_PHONE,
                            OVC_PUR_IUSER_FAX = tbm1301.OVC_PUR_IUSER_FAX,
                            OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION
                        };
                    foreach (var q in queryPurch)
                    {
                        doc.ReplaceText("[$OVC_PURCH$]", q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_IPURCH$]", q.OVC_PUR_IPURCH, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_PURCH$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_PUR_IPURCH$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    var queryProm =
                        from prom in mpms.TBMMANAGE_PROM
                        where prom.OVC_PURCH.Equals(purch)
                        where prom.OVC_PURCH_6.Equals(purch_6)
                        where prom.OVC_OWN_NAME.Equals(txtDocOVC_OWN_NAME.Text)
                        select new
                        {
                            OVC_OWN_NAME = prom.OVC_OWN_NAME
                        };
                    foreach (var q in queryProm)
                    {
                        doc.ReplaceText("[$OVC_OWN_NAME$]", q.OVC_OWN_NAME != null ? q.OVC_OWN_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_OWN_NAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/Fax_Temp.docx");
                }
                buffer = ms.ToArray();
            }
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Fax_Temp.docx");
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/Fax_Temp.odt";
            string FileName = purch + "傳真使用申請表.odt";
            FCommon.WordToOdt(this, path_temp, filetemp, FileName);
        }
        #endregion

        #region 電話傳真
        protected void lbtnTelFax_Click(object sender, EventArgs e)
        {
            var purch = ViewState["strOVC_PURCH"].ToString();
            string path_temp = TelFax_Print(ViewState["strOVC_PURCH_6_D"].ToString());
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "電話傳真表.docx";
            string fileName = HttpUtility.UrlEncode(filepath);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            File.Delete(path_temp);
            Response.End();
        }

        protected void lbtnTelFax_odt_Click(object sender, EventArgs e)
        {
            var purch = ViewState["strOVC_PURCH"].ToString();
            string path_temp = TelFax_Print(ViewState["strOVC_PURCH_6_D"].ToString());
            string filetemp = Request.PhysicalApplicationPath + "WordPDFprint/Fax_odt.odt";
            string FileName = purch + "電話傳真表.odt";
            FCommon.WordToOdt(this, path_temp, filetemp, FileName);
        }

        private string TelFax_Print(string purch_6)
        {
            var purch = ViewState["strOVC_PURCH"].ToString();
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/電話傳真表D1D.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryPurch =
                        from tbm1301 in mpms.TBM1301_PLAN
                        join tbm1302 in mpms.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                        where tbm1301.OVC_PURCH.Equals(purch)
                        where tbm1302.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            OVC_PURCH = tbm1301.OVC_PURCH,
                            OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                            OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                            OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                            OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH
                        };
                    foreach (var q in queryPurch)
                    {
                        doc.ReplaceText("[$OVC_PURCH$]", q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_IPURCH$]", q.OVC_PUR_IPURCH, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_PURCH$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_PUR_IPURCH$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    var queryProm =
                        from prom in mpms.TBMMANAGE_PROM
                        where prom.OVC_PURCH.Equals(purch)
                        where prom.OVC_PURCH_6.Equals(purch_6)
                        where prom.OVC_OWN_NAME.Equals(txtDocOVC_OWN_NAME.Text)
                        select new
                        {
                            OVC_OWN_NAME = prom.OVC_OWN_NAME,
                            OVC_OWN_TEL = prom.OVC_OWN_TEL
                        };
                    foreach (var q in queryProm)
                    {
                        doc.ReplaceText("[$OVC_OWN_NAME$]", q.OVC_OWN_NAME != null ? q.OVC_OWN_NAME : "", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_OWN_TEL$]", q.OVC_OWN_TEL != null ? q.OVC_OWN_TEL : "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_OWN_NAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_OWN_TEL$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    string name = Session["username"].ToString();
                    doc.ReplaceText("[$USER_NAME$]", name, false, System.Text.RegularExpressions.RegexOptions.None);

                    //不確定欄位
                    doc.ReplaceText("[$OpenDate$]", "   年  月  日", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OptionDate1$]", "   年  月  日", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OptionDate2$]", "   年  月  日", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$DocumentNumber1$]", "       ", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$DocumentNumber2$]", "       ", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/TF_Temp.docx");
                }
                buffer = ms.ToArray();
            }
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/TF_Temp.docx");
            return path_temp;
        }
        #endregion

        #region 發包處
        private string lbtnOffice(string id, short g, string ven)
        {
            var purch = ViewState["strOVC_PURCH"].ToString();
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/發包處通知單D1D.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryPurch =
                        from tbm1301 in mpms.TBM1301
                        join tbm1302 in mpms.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                        where tbm1301.OVC_PURCH.Equals(purch)
                        where tbm1302.OVC_PURCH_6.Equals(id)
                        where tbm1302.ONB_GROUP.Equals(g)
                        where tbm1302.OVC_VEN_TITLE.Equals(ven)
                        select new
                        {
                            OVC_PURCH = tbm1301.OVC_PURCH,
                            OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                            OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                            OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                            OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH
                        };
                    foreach (var q in queryPurch)
                    {
                        string ThisYear = (int.Parse(DateTime.Now.Year.ToString()) - 1912).ToString().Substring(1);
                        doc.ReplaceText("[$YYY$]", ThisYear.CompareTo(q.OVC_PURCH.Substring(2, 2)) >= 0 ? "1" + q.OVC_PURCH.Substring(2, 2) : q.OVC_PURCH.Substring(2, 2), false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$PURCH$]", q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PURCH$]", q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_IPURCH$]", q.OVC_PUR_IPURCH, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$PURCH$]", "      ", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_PURCH$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_PUR_IPURCH$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    string name = Session["username"].ToString();
                    doc.ReplaceText("[$USER_NAME$]", name, false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryTel = mpms.ACCOUNT.Where(o => o.USER_NAME.Equals(name)).Select(o => o.IUSER_PHONE).FirstOrDefault();
                    doc.ReplaceText("[$IUSER_PHONE$]", queryTel != null ? queryTel : "", false, System.Text.RegularExpressions.RegexOptions.None);

                    //不確定欄位
                    string today = (int.Parse(DateTime.Now.Year.ToString()) - 1911).ToString() + DateTime.Now.ToString("MMdd");
                    doc.ReplaceText("[$DATE$]", today, false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/CO_Temp.docx");
                }
                buffer = ms.ToArray();
            }
            
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/CO_Temp.docx");
            return path_temp;
        }
        #endregion

        #region 契約及附件分配表
        private string lbtnAttachment(string id, short g, string ven)
        {
            var purch = ViewState["strOVC_PURCH"].ToString();
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/契約及附件分配表D1D.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryPurch =
                        from tbm1301 in mpms.TBM1301
                        join tbm1302 in mpms.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                        where tbm1301.OVC_PURCH.Equals(purch)
                        where tbm1302.OVC_PURCH_6.Equals(id)
                        where tbm1302.ONB_GROUP.Equals(g)
                        where tbm1302.OVC_VEN_TITLE.Equals(ven)
                        select new
                        {
                            OVC_PURCH = tbm1301.OVC_PURCH,
                            OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                            OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                            OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                            OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                            OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE
                        };
                    foreach (var q in queryPurch)
                    {
                        doc.ReplaceText("[$OVC_PURCH$]", q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_PUR_IPURCH$]", q.OVC_PUR_IPURCH, false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$OVC_OWN_NAME$]", q.OVC_VEN_TITLE != null ? q.OVC_VEN_TITLE : "", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_PURCH$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_PUR_IPURCH$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_OWN_NAME$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    
                    var queryNSECTION = mpms.TBM1301
                        .Where(o => o.OVC_PURCH.Equals(purch)).FirstOrDefault();
                    doc.ReplaceText("[$OVC_PUR_NSECTION$]", queryNSECTION != null ? queryNSECTION.OVC_PUR_NSECTION : "", false, System.Text.RegularExpressions.RegexOptions.None);
                    
                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/CA_Temp.docx");
                }
                buffer = ms.ToArray();
            }
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/CA_Temp.docx");
            return path_temp;
        }
        #endregion

        #region 檢查項目表
        protected void lbtnCheckItem_Click(object sender, EventArgs e)
        {

            var purch = ViewState["strOVC_PURCH"].ToString();
            string path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/檢查項目表D1D.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryPurch =
                        from tbm1301 in mpms.TBM1301_PLAN
                        join tbm1302 in mpms.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                        where tbm1301.OVC_PURCH.Equals(purch)
                        select new
                        {
                            OVC_PURCH = tbm1301.OVC_PURCH,
                            OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                            OVC_PURCH_5 = tbm1302.OVC_PURCH_5,
                            OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                            OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                            OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE
                        };
                    foreach (var q in queryPurch)
                    {
                        doc.ReplaceText("[$OVC_PURCH$]", q.OVC_PURCH + q.OVC_PUR_AGENCY + q.OVC_PURCH_5 + q.OVC_PURCH_6, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_PURCH$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/CI_Temp.docx");
                }
                buffer = ms.ToArray();
            }
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/CI_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "檢查項目表.docx";
            string fileName = HttpUtility.UrlEncode(filepath);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            File.Delete(path_temp);
            Response.End();
        }
        #endregion

        #region 複製
        protected void btnCashNo_Click(object sender, EventArgs e)
        {
            string message = "";
            if (!int.TryParse(txtCashNo.Text, out int i))
                message = "請輸入項次數字";
            else if (int.Parse(txtCashNo.Text) > GV_TBMMANAGE_CASH.Rows.Count || int.Parse(txtCashNo.Text) == 0)
                message = "無此項次";

            if (message == "")
            {
                panCash.Visible = true;
                panCashD.Visible = false;
                ViewState["type"] = "cnew";
                txtEscOVC_PURCH_6.Visible = true;
                string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
                string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
                var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
                lblEscOVC_PURCH_6.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                string copyID = GV_TBMMANAGE_CASH.DataKeys[int.Parse(txtCashNo.Text) - 1].Value.ToString();
                #region 如果有資料
                var tbmCash = mpms.TBMMANAGE_CASH.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH) && o.OVC_PURCH_6.Equals(copyID)).FirstOrDefault();
                if (tbmCash != null)
                {
                    txtEscOVC_PURCH_6.Text = tbmCash.OVC_PURCH_6; //分約號
                    txtEscOVC_OWN_NAME.Text = tbmCash.OVC_OWN_NAME; //保證人
                    txtEscOVC_OWN_ADDRESS.Text = tbmCash.OVC_OWN_ADDRESS; //保證人地址
                    txtEscOVC_REASON_1.Text = tbmCash.OVC_REASON_1; //代管事由說明1
                    txtEscOVC_REASON_2.Text = tbmCash.OVC_REASON_2; //代管事由說明2
                    txtEscOVC_REASON_3.Text = tbmCash.OVC_REASON_3; //代管事由說明3
                    txtEscONB_MONEY_NT_1.Text = tbmCash.ONB_MONEY_NT_1 != null ? tbmCash.ONB_MONEY_NT_1.ToString() : ""; //新台幣入帳金額1(必填)
                    txtEscONB_MONEY_NT_2.Text = tbmCash.ONB_MONEY_NT_2 != null ? tbmCash.ONB_MONEY_NT_2.ToString() : ""; //新台幣入帳金額2(必填)
                    txtEscONB_MONEY_NT_3.Text = tbmCash.ONB_MONEY_NT_3 != null ? tbmCash.ONB_MONEY_NT_3.ToString() : ""; //新台幣入帳金額3(必填)
                    txtEscONB_ALL_MONEY.Text = tbmCash.ONB_ALL_MONEY != null ? tbmCash.ONB_ALL_MONEY.ToString() : ""; //合計金額
                    txtEscOVC_MARK.Text = tbmCash.OVC_MARK; //附記
                    txtEscOVC_WORK_NO.Text = tbmCash.OVC_WORK_NO; //承辦單位統一編號
                    txtEscOVC_WORK_NAME.Text = tbmCash.OVC_WORK_NAME; //承辦單位全銜
                    drpEscOVC_ONNAME.SelectedItem.Text = tbmCash.OVC_ONNAME; //財務收繳單位
                    txtEscOVC_ONNAME.Text = tbmCash.OVC_ONNAME; //財務收繳單位
                    txtEscOVC_RECEIVE_NO.Text = tbmCash.OVC_RECEIVE_NO; //收入單通知編號
                    txtEscOVC_DRECEIVE.Text = tbmCash.OVC_DRECEIVE; //收入日期
                    txtEscOVC_COMPTROLLER_NO.Text = tbmCash.OVC_COMPTROLLER_NO; //財務單位編號
                    txtEscOVC_DCOMPTROLLER.Text = tbmCash.OVC_DCOMPTROLLER; //財務單位收訖日
                }
                #endregion
            }
            else
                FCommon.AlertShow(PnMessageC, "danger", "系統訊息", message);
        }

        protected void btnPromNo_Click(object sender, EventArgs e)
        {
            string message = "";
            if (!int.TryParse(txtPromNo.Text, out int i))
                message = "請輸入項次數字";
            else if (int.Parse(txtPromNo.Text) > GV_TBMMANAGE_PROM.Rows.Count || int.Parse(txtPromNo.Text) == 0)
                message = "無此項次";

            if (message == "")
            {
                panDoc.Visible = true;
                panDocD.Visible = false;
                ViewState["type"] = "dnew";
                txtDocOVC_PURCH_6.Visible = true;
                string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
                string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
                //string strOVC_PURCH_6 = ViewState["strOVC_PURCH_6_C"].ToString();
                var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
                lblDocOVC_PURCH_6.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                string copyID = GV_TBMMANAGE_PROM.DataKeys[int.Parse(txtPromNo.Text) - 1].Value.ToString();

                #region 如果有資料
                var tbmProm = mpms.TBMMANAGE_PROM.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH) && o.OVC_PURCH_6.Equals(copyID)).FirstOrDefault();
                if (tbmProm != null)
                {
                    txtDocOVC_PURCH_6.Text = tbmProm.OVC_PURCH_6; //分約號
                    txtDocOVC_OWN_NAME.Text = tbmProm.OVC_OWN_NAME; //保證人
                    txtDocOVC_OWN_ADDRESS.Text = tbmProm.OVC_OWN_ADDRESS; //保證人地址
                    txtDocOVC_OWNED_NAME.Text = tbmProm.OVC_OWNED_NAME; //被保證人
                    txtDocOVC_OWNED_NO.Text = tbmProm.OVC_OWNED_NO; //被保證人統一編號
                    txtDocOVC_OWNED_ADDRESS.Text = tbmProm.OVC_OWNED_ADDRESS; //被保證人地址
                    txtDocOVC_REASON_1.Text = tbmProm.OVC_REASON_1; //代管事由說明1
                    txtDocOVC_REASON_2.Text = tbmProm.OVC_REASON_2; //代管事由說明2
                    txtDocOVC_REASON_3.Text = tbmProm.OVC_REASON_3; //代管事由說明3
                    txtDocONB_MONEY_1.Text = tbmProm.ONB_MONEY_1 != null ? tbmProm.ONB_MONEY_1.ToString() : ""; //新台幣入帳金額1(必填)
                    txtDocONB_MONEY_2.Text = tbmProm.ONB_MONEY_2 != null ? tbmProm.ONB_MONEY_2.ToString() : ""; //新台幣入帳金額2(必填)
                    txtDocONB_MONEY_3.Text = tbmProm.ONB_MONEY_3 != null ? tbmProm.ONB_MONEY_3.ToString() : ""; //新台幣入帳金額3(必填)
                    txtDocONB_ALL_MONEY.Text = tbmProm.ONB_ALL_MONEY != null ? tbmProm.ONB_ALL_MONEY.ToString() : ""; //合計金額
                    txtDocOVC_MARK.Text = tbmProm.OVC_MARK; //附記
                    txtDocOVC_WORK_NO.Text = tbmProm.OVC_WORK_NO; //承辦單位統一編號
                    txtDocOVC_WORK_NAME.Text = tbmProm.OVC_WORK_NAME; //承辦單位全銜
                    drpDocOVC_ONNAME.SelectedItem.Text = tbmProm.OVC_ONNAME; //財務收繳單位
                    txtDocOVC_ONNAME.Text = tbmProm.OVC_ONNAME; //財務收繳單位
                    txtDocOVC_RECEIVE_NO.Text = tbmProm.OVC_RECEIVE_NO; //收入單通知編號
                    txtDocOVC_DRECEIVE.Text = tbmProm.OVC_DRECEIVE; //收入日期
                    txtDocOVC_COMPTROLLER_NO.Text = tbmProm.OVC_COMPTROLLER_NO; //財務單位編號
                    txtDocOVC_DCOMPTROLLER.Text = tbmProm.OVC_DCOMPTROLLER; //財務單位收訖日
                }
                #endregion
            }
            else
                FCommon.AlertShow(PnMessageD, "danger", "系統訊息", message);
        }

        protected void btnStockNo_Click(object sender, EventArgs e)
        {
            string message = "";
            if (!int.TryParse(txtStockNo.Text, out int i))
                message = "請輸入項次數字";
            else if (int.Parse(txtStockNo.Text) > GV_TBMMANAGE_STOCK.Rows.Count || int.Parse(txtStockNo.Text) == 0)
                message = "無此項次";

            if (message == "")
            {
                panSec.Visible = true;
                panSecD.Visible = false;
                ViewState["type"] = "snew";
                txtSecOVC_PURCH_6.Visible = true;
                string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
                string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
                var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
                lblSecOVC_PURCH_6.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                string copyID = GV_TBMMANAGE_STOCK.DataKeys[int.Parse(txtStockNo.Text) - 1].Value.ToString();

                #region 如果有資料
                var tbmStock = mpms.TBMMANAGE_STOCK.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH) && o.OVC_PURCH_6.Equals(copyID)).FirstOrDefault();
                if (tbmStock != null)
                {
                    txtSecOVC_PURCH_6.Text = tbmStock.OVC_PURCH_6; //分約號
                    txtSecOVC_OWN_NAME.Text = tbmStock.OVC_OWN_NAME; //保證人
                    txtSecOVC_OWN_ADDRESS.Text = tbmStock.OVC_OWN_ADDRESS; //保證人地址
                    txtSecOVC_REASON_1.Text = tbmStock.OVC_REASON_1; //代管事由說明1
                    txtSecOVC_REASON_2.Text = tbmStock.OVC_REASON_2; //代管事由說明2
                    txtSecOVC_REASON_3.Text = tbmStock.OVC_REASON_3; //代管事由說明3
                    txtSecONB_MONEY_NT_1.Text = tbmStock.ONB_MONEY_NT_1 != null ? tbmStock.ONB_MONEY_NT_1.ToString() : ""; //新台幣入帳金額1(必填)
                    txtSecONB_MONEY_NT_2.Text = tbmStock.ONB_MONEY_NT_2 != null ? tbmStock.ONB_MONEY_NT_2.ToString() : ""; //新台幣入帳金額2(必填)
                    txtSecONB_MONEY_NT_3.Text = tbmStock.ONB_MONEY_NT_3 != null ? tbmStock.ONB_MONEY_NT_3.ToString() : ""; //新台幣入帳金額3(必填)
                    txtSecONB_ALL_MONEY.Text = tbmStock.ONB_ALL_MONEY != null ? tbmStock.ONB_ALL_MONEY.ToString() : ""; //合計金額
                    txtSecOVC_MARK.Text = tbmStock.OVC_MARK; //附記
                    txtSecOVC_WORK_NO.Text = tbmStock.OVC_WORK_NO; //承辦單位統一編號
                    txtSecOVC_WORK_NAME.Text = tbmStock.OVC_WORK_NAME; //承辦單位全銜
                    drpSecOVC_ONNAME.SelectedItem.Text = tbmStock.OVC_ONNAME; //財務收繳單位
                    txtSecOVC_ONNAME.Text = tbmStock.OVC_ONNAME; //財務收繳單位
                    txtSecOVC_RECEIVE_NO.Text = tbmStock.OVC_RECEIVE_NO; //收入單通知編號
                    txtSecOVC_DRECEIVE.Text = tbmStock.OVC_DRECEIVE; //收入日期
                    txtSecOVC_COMPTROLLER_NO.Text = tbmStock.OVC_COMPTROLLER_NO; //財務單位編號
                    txtSecOVC_DCOMPTROLLER.Text = tbmStock.OVC_DCOMPTROLLER; //財務單位收訖日
                }
                #endregion
            }
            else
                FCommon.AlertShow(PnMessageS, "danger", "系統訊息", message);
        }
        #endregion

        #region 複製代管現金
        protected void btnCashNo_P_Click(object sender, EventArgs e)
        {
            string message = "";
            if (!int.TryParse(txtCashNo_P.Text, out int i))
                message = "請輸入項次數字";
            else if (int.Parse(txtCashNo_P.Text) > GV_TBMMANAGE_CASH.Rows.Count || int.Parse(txtCashNo_P.Text) == 0)
                message = "無此項次";

            if (message == "")
            {
                panDoc.Visible = true;
                panDocD.Visible = false;
                ViewState["type"] = "dnew";
                txtDocOVC_PURCH_6.Visible = true;
                string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
                string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
                //string strOVC_PURCH_6 = ViewState["strOVC_PURCH_6_C"].ToString();
                var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
                lblDocOVC_PURCH_6.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                string copyID = GV_TBMMANAGE_CASH.DataKeys[int.Parse(txtCashNo_P.Text) - 1].Value.ToString();

                #region 如果cash有資料
                var tbmCash = mpms.TBMMANAGE_CASH.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH) && o.OVC_PURCH_6.Equals(copyID)).FirstOrDefault();
                if (tbmCash != null)
                {
                    txtDocOVC_PURCH_6.Text = tbmCash.OVC_PURCH_6; //分約號
                    txtDocOVC_OWN_NAME.Text = tbmCash.OVC_OWN_NAME; //保證人
                    txtDocOVC_OWN_ADDRESS.Text = tbmCash.OVC_OWN_ADDRESS; //保證人地址
                    txtDocOVC_REASON_1.Text = tbmCash.OVC_REASON_1; //代管事由說明1
                    txtDocOVC_REASON_2.Text = tbmCash.OVC_REASON_2; //代管事由說明2
                    txtDocOVC_REASON_3.Text = tbmCash.OVC_REASON_3; //代管事由說明3
                    txtDocONB_MONEY_1.Text = tbmCash.ONB_MONEY_NT_1 != null ? tbmCash.ONB_MONEY_NT_1.ToString() : ""; //新台幣入帳金額1(必填)
                    txtDocONB_MONEY_2.Text = tbmCash.ONB_MONEY_NT_2 != null ? tbmCash.ONB_MONEY_NT_2.ToString() : ""; //新台幣入帳金額2(必填)
                    txtDocONB_MONEY_3.Text = tbmCash.ONB_MONEY_NT_3 != null ? tbmCash.ONB_MONEY_NT_3.ToString() : ""; //新台幣入帳金額3(必填)
                    txtDocONB_ALL_MONEY.Text = tbmCash.ONB_ALL_MONEY != null ? tbmCash.ONB_ALL_MONEY.ToString() : ""; //合計金額
                    txtDocOVC_MARK.Text = tbmCash.OVC_MARK; //附記
                    txtDocOVC_WORK_NO.Text = tbmCash.OVC_WORK_NO; //承辦單位統一編號
                    txtDocOVC_WORK_NAME.Text = tbmCash.OVC_WORK_NAME; //承辦單位全銜
                    drpDocOVC_ONNAME.SelectedItem.Text = tbmCash.OVC_ONNAME; //財務收繳單位
                    txtDocOVC_ONNAME.Text = tbmCash.OVC_ONNAME; //財務收繳單位
                    txtDocOVC_RECEIVE_NO.Text = tbmCash.OVC_RECEIVE_NO; //收入單通知編號
                    txtDocOVC_DRECEIVE.Text = tbmCash.OVC_DRECEIVE; //收入日期
                    txtDocOVC_COMPTROLLER_NO.Text = tbmCash.OVC_COMPTROLLER_NO; //財務單位編號
                    txtDocOVC_DCOMPTROLLER.Text = tbmCash.OVC_DCOMPTROLLER; //財務單位收訖日
                }
                #endregion
            }
            else
                FCommon.AlertShow(PnMessageD, "danger", "系統訊息", message);
        }

        protected void btnCashNo_S_Click(object sender, EventArgs e)
        {
            string message = "";
            if (!int.TryParse(txtCashNo_S.Text, out int i))
                message = "請輸入項次數字";
            else if (int.Parse(txtCashNo_S.Text) > GV_TBMMANAGE_CASH.Rows.Count || int.Parse(txtCashNo_S.Text) == 0)
                message = "無此項次";

            if (message == "")
            {
                panSec.Visible = true;
                panSecD.Visible = false;
                ViewState["type"] = "snew";
                txtSecOVC_PURCH_6.Visible = true;
                string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
                string strOVC_PURCH_5 = ViewState["strOVC_PURCH_5"].ToString();
                var query1301 = mpms.TBM1301.Where(table => table.OVC_PURCH == strOVC_PURCH).FirstOrDefault();
                lblSecOVC_PURCH_6.Text = strOVC_PURCH + query1301.OVC_PUR_AGENCY + strOVC_PURCH_5;
                string copyID = GV_TBMMANAGE_CASH.DataKeys[int.Parse(txtCashNo_S.Text) - 1].Value.ToString();

                #region 如果cash有資料
                var tbmCash = mpms.TBMMANAGE_CASH.Where(o => o.OVC_PURCH.Equals(strOVC_PURCH) && o.OVC_PURCH_6.Equals(copyID)).FirstOrDefault();
                if (tbmCash != null)
                {
                    txtSecOVC_PURCH_6.Text = tbmCash.OVC_PURCH_6; //分約號
                    txtSecOVC_OWN_NAME.Text = tbmCash.OVC_OWN_NAME; //保證人
                    txtSecOVC_OWN_ADDRESS.Text = tbmCash.OVC_OWN_ADDRESS; //保證人地址
                    txtSecOVC_REASON_1.Text = tbmCash.OVC_REASON_1; //代管事由說明1
                    txtSecOVC_REASON_2.Text = tbmCash.OVC_REASON_2; //代管事由說明2
                    txtSecOVC_REASON_3.Text = tbmCash.OVC_REASON_3; //代管事由說明3
                    txtSecONB_MONEY_NT_1.Text = tbmCash.ONB_MONEY_NT_1 != null ? tbmCash.ONB_MONEY_NT_1.ToString() : ""; //新台幣入帳金額1(必填)
                    txtSecONB_MONEY_NT_2.Text = tbmCash.ONB_MONEY_NT_2 != null ? tbmCash.ONB_MONEY_NT_2.ToString() : ""; //新台幣入帳金額2(必填)
                    txtSecONB_MONEY_NT_3.Text = tbmCash.ONB_MONEY_NT_3 != null ? tbmCash.ONB_MONEY_NT_3.ToString() : ""; //新台幣入帳金額3(必填)
                    txtSecONB_ALL_MONEY.Text = tbmCash.ONB_ALL_MONEY != null ? tbmCash.ONB_ALL_MONEY.ToString() : ""; //合計金額
                    txtSecOVC_MARK.Text = tbmCash.OVC_MARK; //附記
                    txtSecOVC_WORK_NO.Text = tbmCash.OVC_WORK_NO; //承辦單位統一編號
                    txtSecOVC_WORK_NAME.Text = tbmCash.OVC_WORK_NAME; //承辦單位全銜
                    drpSecOVC_ONNAME.SelectedItem.Text = tbmCash.OVC_ONNAME; //財務收繳單位
                    txtSecOVC_ONNAME.Text = tbmCash.OVC_ONNAME; //財務收繳單位
                    txtSecOVC_RECEIVE_NO.Text = tbmCash.OVC_RECEIVE_NO; //收入單通知編號
                    txtSecOVC_DRECEIVE.Text = tbmCash.OVC_DRECEIVE; //收入日期
                    txtSecOVC_COMPTROLLER_NO.Text = tbmCash.OVC_COMPTROLLER_NO; //財務單位編號
                    txtSecOVC_DCOMPTROLLER.Text = tbmCash.OVC_DCOMPTROLLER; //財務單位收訖日
                }
                #endregion
            }
            else
                FCommon.AlertShow(PnMessageS, "danger", "系統訊息", message);
        }
        #endregion

        #region 採購發包階段上傳檔案
        public void GvFilesImport()
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();

            //資料夾檔案內容寫入GV
            System.Data.DataTable dtFile = new System.Data.DataTable();
            dtFile.Columns.Add("FileName", typeof(System.String));
            //dtFile.Columns.Add("Time", typeof(System.DateTime));
            dtFile.Columns.Add("Time", typeof(System.String));
            dtFile.Columns.Add("FileSize", typeof(System.Int32));
            string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D12/" + strOVC_PURCH));
            if (!Directory.Exists(serverDir))
            {
                Directory.CreateDirectory(serverDir);  //新增資料夾
            }
            DirectoryInfo filePath = new DirectoryInfo(serverDir);
            FileInfo[] fileList = filePath.GetFiles();  //擷取目錄下所有檔案內容，並存到 FileInfo Array
            if (fileList.Length != 0)
            {
                foreach (FileInfo file in fileList)
                {
                    string strFilePath = String.Format("<a href='/Uploadfile/MPMS/D/D12/" + strOVC_PURCH + "/" + file.Name + "' target='_blank'>" + file.Name + "</a>");
                    dtFile.Rows.Add(strFilePath, GetTaiwanDate(file.CreationTime.ToString()) + " " + file.CreationTime.Hour + ":" + file.CreationTime.Minute
                                    , file.Length / 1024);  //file.Length/1024 = KB
                }
                gvFiles.DataSource = dtFile;
                gvFiles.DataBind();
            }

            if (dtFile == null || dtFile.Rows.Count <= 0)
            {
                string[] fileField = { "FileName", "Time", "FileSize" };
                FCommon.GridView_setEmpty(gvFiles, fileField);
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string strOVC_PURCH = ViewState["strOVC_PURCH"].ToString();
            //按下上傳 => 檔案上傳
            if (isUpload)
            {
                if (FileUpload.HasFile)
                {
                    string serverPath = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D12"));
                    string serverDir = Path.Combine(Server.MapPath("~/Uploadfile/MPMS/D/D12/" + strOVC_PURCH));
                    string fileName = FileUpload.FileName;
                    string serverFilePath = Path.Combine(serverDir, fileName);
                    //string strOVC_ATTACH_NAME = ViewState["OVC_ATTACH_NAME_UPLOAD"].ToString();
                    if (!Directory.Exists(serverPath))
                        Directory.CreateDirectory(serverPath);   //新增D12資料夾
                    if (!Directory.Exists(serverDir))
                        Directory.CreateDirectory(serverDir);   //新增購案編號資料夾
                    try
                    {
                        FileUpload.SaveAs(serverFilePath);
                        GvFilesImport();
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "檔案上傳成功");
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", error);
                    }
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先 選擇檔案");
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "無上傳權限");
        }

        public string GetTaiwanDate(string strDate)
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
        #endregion
    }
}