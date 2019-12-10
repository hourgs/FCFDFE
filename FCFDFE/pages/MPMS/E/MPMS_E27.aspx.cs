using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using Microsoft.International.Formatters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xceed.Words.NET;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E27 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        int count = 0, count_2 = 0;
        public string strMenuName = "", strMenuNameItem = "";
                                               
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            if (Session["rowtext"] != null && Session["shiptime"] != null)
            {
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtONB_SHIP_TIMES, txtOVC_PURCH, txtOVC_VEN_TITLE, txtOVC_DCONTRACT, txtOVC_DBID, txtOVC_DEPT_CDE, txtOVC_ONNAME,
                        txtOVC_DAUDIT, txtOVC_DELIVERY, txtOVC_DAIRSHIP, txtOVC_DAIRSHIP_PLAN, txtOVC_DINFORM, txtOVC_DINFORM_PLAN, txtOVC_DJOINCHECK, txtOVC_DJOINCHECK_PLAN,
                        txtOVC_DSHIPMENT, txtOVC_DSHIPMENT_PLAN, txtOVC_DINVENTORY, txtOVC_DINVENTORY_PLAN, txtOVC_DINSPECT, txtOVC_DINSPECT_PLAN, txtOVC_DPAY, txtOVC_DPAY_PLAN, txtOVC_DACCORDING);
                    
                    TB_dataImport();
                    GV_dataImport();

                    #region 超前or落後天數
                    dtDifference_days(txtOVC_DAIRSHIP, txtOVC_DAIRSHIP_PLAN, txt);
                    dtDifference_days(txtOVC_DINFORM, txtOVC_DINFORM_PLAN, txtONB_NO_PUNISH_DAYS_OVC_DINFORM);
                    dtDifference_days(txtOVC_DJOINCHECK, txtOVC_DJOINCHECK_PLAN, txtONB_NO_PUNISH_DAYS_OVC_DJOINCHECK);
                    dtDifference_days(txtOVC_DSHIPMENT, txtOVC_DSHIPMENT_PLAN, txtONB_NO_PUNISH_DAYS_OVC_DSHIPMENT);
                    dtDifference_days(txtOVC_DINVENTORY, txtOVC_DINVENTORY_PLAN, txtONB_NO_PUNISH_DAYS_OVC_DINVENTORY);
                    dtDifference_days(txtOVC_DINSPECT, txtOVC_DINSPECT_PLAN, txtONB_NO_PUNISH_DAYS_OVC_DINSPECT);
                    dtDifference_days(txtOVC_DPAY, txtOVC_DPAY_PLAN, txtONB_NO_PUNISH_DAYS_OVC_DPAY);
                    #endregion

                    Session.Contents.Remove("INSPECT_TIMES");
                    Session.Contents.Remove("RE_INSPECT_TIMES");
                    Session.Contents.Remove("isModify_2");
                    var query = gm.TBM1407;
                    DataTable dtONB_RECEIVE_DAYS = CommonStatic.ListToDataTable(query.Where(table => table.OVC_PHR_CATE == "M1").ToList());
                    FCommon.list_dataImportV(drpONB_DINSPECT_SOP, dtONB_RECEIVE_DAYS, "OVC_PHR_DESC", "OVC_USR_ID", ":", true);
                }
            }
            else
                FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
        }

        #region Click

        //輸入交貨項目btn
        protected void btnIp_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E32.aspx";
            Response.Redirect(send_url);
        }

        #region 回上一頁
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E17.aspx";
            Response.Redirect(send_url);
        }
        protected void btnR_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E17.aspx";
            Response.Redirect(send_url);
        }
        #endregion

        #region 回主流程
        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }
        protected void btnRM_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E28.aspx";
            Response.Redirect(send_url);
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E29.aspx";
            Response.Redirect(send_url);
        }
        #endregion

        #region 清除日期
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtOVC_DAUDIT.Text = "";
        }
        protected void btnC_OVC_DELIVERY_Click(object sender, EventArgs e)
        {
            txtOVC_DELIVERY.Text = "";
        }
        protected void btnClear_OVC_DAIRSHIP_Click(object sender, EventArgs e)
        {
            txtOVC_DAIRSHIP.Text = "";
            txt.Text = "";
        }
        protected void btnClear_OVC_DAIRSHIP_PLAN_Click(object sender, EventArgs e)
        {
            txtOVC_DAIRSHIP_PLAN.Text = "";
            txt.Text = "";
        }
        protected void btnClear_OVC_DINFORM_Click(object sender, EventArgs e)
        {
            txtOVC_DINFORM.Text = "";
            txtONB_NO_PUNISH_DAYS_OVC_DINFORM.Text = "";
        }
        protected void btnClear_OVC_DINFORM_PLAN_Click(object sender, EventArgs e)
        {
            txtOVC_DINFORM_PLAN.Text = "";
            txtONB_NO_PUNISH_DAYS_OVC_DINFORM.Text = "";
        }
        protected void btnCLear__Click(object sender, EventArgs e)
        {
            txtOVC_DJOINCHECK.Text = "";
            txtONB_NO_PUNISH_DAYS_OVC_DJOINCHECK.Text = "";
        }
        protected void btnClear_OVC_DJOINCHECK_PLAN_Click(object sender, EventArgs e)
        {
            txtOVC_DJOINCHECK_PLAN.Text = "";
            txtONB_NO_PUNISH_DAYS_OVC_DJOINCHECK.Text = "";
        }
        protected void btnClear_OVC_DSHIPMENT_Click(object sender, EventArgs e)
        {
            txtOVC_DSHIPMENT.Text = "";
            txtONB_NO_PUNISH_DAYS_OVC_DSHIPMENT.Text = "";
        }
        protected void btnClear_OVC_DSHIPMENT_PLAN_Click(object sender, EventArgs e)
        {
            txtOVC_DSHIPMENT_PLAN.Text = "";
            txtONB_NO_PUNISH_DAYS_OVC_DSHIPMENT.Text = "";
        }
        protected void btnClear_OVC_DINVENTORY_Click(object sender, EventArgs e)
        {
            txtOVC_DINVENTORY.Text = "";
            txtONB_NO_PUNISH_DAYS_OVC_DINVENTORY.Text = "";
        }
        protected void btnClear_OVC_DINVENTORY_PLAN_Click(object sender, EventArgs e)
        {
            txtOVC_DINVENTORY_PLAN.Text = "";
            txtONB_NO_PUNISH_DAYS_OVC_DINVENTORY.Text = "";
        }
        protected void btnClear_OVC_DINSPECT_Click(object sender, EventArgs e)
        {
            txtOVC_DINSPECT.Text = "";
            txtONB_NO_PUNISH_DAYS_OVC_DINSPECT.Text = "";
        }
        protected void btnClear_OVC_DINSPECT_PLAN_Click(object sender, EventArgs e)
        {
            txtOVC_DINSPECT_PLAN.Text = "";
            txtONB_NO_PUNISH_DAYS_OVC_DINSPECT.Text = "";
        }
        protected void btnClear_OVC_DPAY_Click(object sender, EventArgs e)
        {
            txtOVC_DPAY.Text = "";
            txtONB_NO_PUNISH_DAYS_OVC_DPAY.Text = "";
        }
        protected void btnClear_OVC_DPAY_PLAN_Click(object sender, EventArgs e)
        {
            txtOVC_DPAY_PLAN.Text = "";
            txtONB_NO_PUNISH_DAYS_OVC_DPAY.Text = "";
        }
        protected void btnClear_OVC_DACCORDING_Click(object sender, EventArgs e)
        {
            txtOVC_DACCORDING.Text = "";
        }
        #endregion

        #region 修改btn
        protected void btnCha_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            Session["INSPECT_TIMES"] = GV_TakeOver.Cells[1].Text;
            Session["RE_INSPECT_TIMES"] = GV_TakeOver.Cells[2].Text;
            Session["isModify_2"] = "1";
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E28.aspx";
            Response.Redirect(send_url);
        }
        protected void btnCha_Click1(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            Session["INSPECT_TIMES"] = GV_TakeOver.Cells[1].Text;
            Session["RE_INSPECT_TIMES"] = GV_TakeOver.Cells[2].Text;
            Session["isModify_2"] = "1";
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E29.aspx";
            Response.Redirect(send_url);
        }
        #endregion

        #region 刪除btn
        protected void btnDel_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            string purch = Session["rowtext"].ToString().Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            var query =
                from report in mpms.TBMINSPECT_REPORT
                join report_item in mpms.TBMINSPECT_REPORT_ITEM on new { report.OVC_PURCH, report.ONB_TIMES, report.ONB_INSPECT_TIMES, report.ONB_RE_INSPECT_TIMES } equals new { report_item.OVC_PURCH ,report_item.ONB_TIMES, report_item.ONB_INSPECT_TIMES, report_item.ONB_RE_INSPECT_TIMES } into i
                from report_item in i.DefaultIfEmpty()
                where report.OVC_PURCH.Equals(purch)
                where report.OVC_PURCH_6.Equals(purch_6)
                select new
                {
                    OVC_PURCH_6 = report.OVC_PURCH_6,
                    OVC_VEN_CST = report.OVC_VEN_CST,
                    ONB_TIMES = report.ONB_TIMES,
                    ONB_INSPECT_TIMES = report.ONB_INSPECT_TIMES,
                    ONB_RE_INSPECT_TIMES = report.ONB_RE_INSPECT_TIMES,
                    OVC_REPORT_DESC = report.OVC_REPORT_DESC,
                    OVC_DINSPECT = report.OVC_DINSPECT
                };
            foreach (var q in query)
            {
                if (q.ONB_TIMES.ToString() == txtONB_SHIP_TIMES.Text && q.ONB_INSPECT_TIMES.ToString() == GV_TakeOver.Cells[1].Text && q.ONB_RE_INSPECT_TIMES.ToString() == GV_TakeOver.Cells[2].Text)
                {
                    TBMINSPECT_REPORT tbminspect_report = new TBMINSPECT_REPORT();
                    tbminspect_report = mpms.TBMINSPECT_REPORT
                        .Where(table => table.OVC_PURCH.Equals(purch) && table.OVC_PURCH_6.Equals(purch_6) && table.ONB_TIMES.Equals(q.ONB_TIMES) && table.ONB_INSPECT_TIMES.Equals(q.ONB_INSPECT_TIMES) && table.ONB_RE_INSPECT_TIMES.Equals(q.ONB_RE_INSPECT_TIMES)).FirstOrDefault();
                    if (tbminspect_report != null)
                    {
                        mpms.Entry(tbminspect_report).State = EntityState.Deleted;
                        mpms.SaveChanges();
                    }
                    for (int i = 1; i < 13; i++)
                    {
                        short sh = short.Parse(i.ToString());
                        TBMINSPECT_REPORT_ITEM tbminspect_report_item = new TBMINSPECT_REPORT_ITEM();
                        tbminspect_report_item = mpms.TBMINSPECT_REPORT_ITEM
                            .Where(table => table.OVC_PURCH.Equals(purch) && table.OVC_PURCH_6.Equals(purch_6) && table.ONB_ITEM.Equals(sh) && table.ONB_TIMES.Equals(q.ONB_TIMES) && table.ONB_INSPECT_TIMES.Equals(q.ONB_INSPECT_TIMES) && table.ONB_RE_INSPECT_TIMES.Equals(q.ONB_RE_INSPECT_TIMES)).FirstOrDefault();
                        if (tbminspect_report_item != null)
                        {
                            mpms.Entry(tbminspect_report_item).State = EntityState.Deleted;
                            mpms.SaveChanges();
                        }
                    }
                }
            }
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
            GV_dataImport();
        }
        protected void btnDel_1_Click(object sender, EventArgs e)
        {
            Button BtnTakeOver = (Button)sender;
            GridViewRow GV_TakeOver = (GridViewRow)BtnTakeOver.NamingContainer;
            int rowindex = GV_TakeOver.RowIndex;
            string purch = Session["rowtext"].ToString().Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            var query =
                from record in mpms.TBMINSPECT_RECORD
                where record.OVC_PURCH.Equals(purch)
                where record.OVC_PURCH_6.Equals(purch_6)
                select new
                {
                    OVC_PURCH_6 = record.OVC_PURCH_6,
                    OVC_VEN_CST = record.OVC_VEN_CST,
                    ONB_TIMES = record.ONB_TIMES,
                    ONB_INSPECT_TIMES = record.ONB_INSPECT_TIMES,
                    ONB_RE_INSPECT_TIMES = record.ONB_RE_INSPECT_TIMES,
                    OVC_DELIVERY_CONTRACT = record.OVC_DELIVERY_CONTRACT,
                    OVC_DELIVERY_PLACE = record.OVC_DELIVERY_PLACE
                };
            foreach (var q in query)
            {
                if (q.ONB_TIMES.ToString() == txtONB_SHIP_TIMES.Text && q.ONB_INSPECT_TIMES.ToString() == GV_TakeOver.Cells[1].Text && q.ONB_RE_INSPECT_TIMES.ToString() == GV_TakeOver.Cells[2].Text)
                {
                    TBMINSPECT_RECORD tbminspect_record = new TBMINSPECT_RECORD();
                    tbminspect_record = mpms.TBMINSPECT_RECORD
                        .Where(table => table.OVC_PURCH.Equals(purch) && table.OVC_PURCH_6.Equals(purch_6) && table.ONB_TIMES.Equals(q.ONB_TIMES) && table.ONB_INSPECT_TIMES.Equals(q.ONB_INSPECT_TIMES) && table.ONB_RE_INSPECT_TIMES.Equals(q.ONB_RE_INSPECT_TIMES)).FirstOrDefault();
                    if (tbminspect_record != null)
                    {
                        mpms.Entry(tbminspect_record).State = EntityState.Deleted;
                        mpms.SaveChanges();
                    }
                }
            }
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功");
            GV_dataImport();
        }
        #endregion

        #region 存檔
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string purch = lblOVC_PURCH.Text.Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            TBMRECEIVE_CONTRACT con = mpms.TBMRECEIVE_CONTRACT.Where(t => t.OVC_PURCH.Equals(purch) && t.OVC_PURCH_6.Equals(purch_6)).FirstOrDefault();
            if (con != null)
            {
                con.OVC_PUR_NSECTION = txtOVC_ONNAME.Text;
                con.OVC_DCONTRACT = txtOVC_DCONTRACT.Text;
            }
            TBM1302 t1302 = mpms.TBM1302.Where(t => t.OVC_PURCH.Equals(purch) && t.OVC_PURCH_6.Equals(purch_6)).FirstOrDefault();
            if (t1302 != null)
            {
                t1302.OVC_DCONTRACT = txtOVC_DCONTRACT.Text;
            }
            TBM1301 t1301 = mpms.TBM1301.Where(t => t.OVC_PURCH.Equals(purch)).FirstOrDefault();
            if (t1301 != null)
            {
                t1301.OVC_PUR_SECTION = txtOVC_DEPT_CDE.Text;
                t1301.OVC_PUR_NSECTION = txtOVC_ONNAME.Text;
            }
            TBM1301_PLAN t1301_p = mpms.TBM1301_PLAN.Where(t => t.OVC_PURCH.Equals(purch)).FirstOrDefault();
            if (t1301_p != null)
            {
                t1301_p.OVC_PUR_SECTION = txtOVC_DEPT_CDE.Text;
                t1301_p.OVC_PUR_NSECTION = txtOVC_ONNAME.Text;
            }
            mpms.SaveChanges();
            if (Session["isModify"] != null)
            {
                var query =
                    from delivery in mpms.TBMDELIVERY
                    where delivery.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    where delivery.OVC_PURCH_6.Equals(purch_6)
                    select delivery.ONB_SHIP_TIMES;
                foreach (var q in query)
                {
                    if (q.ToString() == txtONB_SHIP_TIMES.Text)
                    {
                        TBMDELIVERY tbmdelivery = new TBMDELIVERY();
                        tbmdelivery = mpms.TBMDELIVERY
                            .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7)) && table.OVC_PURCH_6.Equals(purch_6) && table.ONB_SHIP_TIMES.Equals(q)).FirstOrDefault();
                        if (tbmdelivery != null)
                        {
                            tbmdelivery.OVC_DELIVERY_CONTRACT = txtOVC_DAUDIT.Text;
                            tbmdelivery.OVC_DELIVERY_PERIOD = txtOVC_DAUDIT_Input.Text;
                            if (txtONB_DAYS_CONTRACT.Text != "")
                            {
                                if (short.TryParse(txtONB_DAYS_CONTRACT.Text, out short s))
                                    tbmdelivery.ONB_DAYS_CONTRACT = short.Parse(txtONB_DAYS_CONTRACT.Text);
                                else
                                    strMessage += "<p>契約交貨天數 須為數字</p>";
                            }
                            tbmdelivery.OVC_DELIVERY = txtOVC_DELIVERY.Text;
                            if (txtONB_MCONTRACT.Text != "")
                            {
                                if (decimal.TryParse(txtONB_MCONTRACT.Text, out decimal d))
                                    tbmdelivery.ONB_MCONTRACT = decimal.Parse(txtONB_MCONTRACT.Text);
                                else
                                    strMessage += "<p>契約金額 須為數字</p>";
                            }
                            if (txtONB_MDELIVERY.Text != "")
                            {
                                if (decimal.TryParse(txtONB_MDELIVERY.Text, out decimal d))
                                    tbmdelivery.ONB_MDELIVERY = decimal.Parse(txtONB_MDELIVERY.Text);
                                else
                                    strMessage += "<p>本次交貨金額 須為數字</p>";
                            }
                            tbmdelivery.OVC_DELIVERY_PLACE = txtOVC_DELIVERY_PLACE.Text;
                            tbmdelivery.OVC_DAIRSHIP = txtOVC_DAIRSHIP.Text;
                            tbmdelivery.OVC_DAIRSHIP_PLAN = txtOVC_DAIRSHIP_PLAN.Text;
                            tbmdelivery.OVC_DINFORM = txtOVC_DINFORM.Text;
                            tbmdelivery.OVC_DINFORM_PLAN = txtOVC_DINFORM_PLAN.Text;
                            tbmdelivery.OVC_DJOINCHECK = txtOVC_DJOINCHECK.Text;
                            tbmdelivery.OVC_DJOINCHECK_PLAN = txtOVC_DJOINCHECK_PLAN.Text;
                            tbmdelivery.OVC_DSHIPMENT = txtOVC_DSHIPMENT.Text;
                            tbmdelivery.OVC_DSHIPMENT_PLAN = txtOVC_DSHIPMENT_PLAN.Text;
                            tbmdelivery.OVC_DINVENTORY = txtOVC_DINVENTORY.Text;
                            tbmdelivery.OVC_DINVENTORY_PLAN = txtOVC_DINVENTORY_PLAN.Text;
                            tbmdelivery.OVC_DINSPECT = txtOVC_DINSPECT.Text;
                            tbmdelivery.OVC_DINSPECT_PLAN = txtOVC_DINSPECT_PLAN.Text;
                            tbmdelivery.OVC_DPAY = txtOVC_DPAY.Text;
                            tbmdelivery.OVC_DPAY_PLAN = txtOVC_DPAY_PLAN.Text;
                            if (strMessage == "")
                                mpms.SaveChanges();
                            else
                                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                        }
                    }
                }
                if (strMessage == "")
                {
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                    TB_dataImport();
                    GV_dataImport();
                }
            }
            else
            {
                TBMDELIVERY tbmdelivery_new = new TBMDELIVERY();
                var queryNew =
                    from tbm1302 in mpms.TBM1302
                    where tbm1302.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    where tbm1302.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        OVC_PURCH = tbm1302.OVC_PURCH,
                        OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                        OVC_VEN_CST = tbm1302.OVC_VEN_CST
                    };
                foreach (var q in queryNew)
                {
                    tbmdelivery_new.OVC_PURCH = q.OVC_PURCH;
                    tbmdelivery_new.OVC_PURCH_6 = q.OVC_PURCH_6;
                    tbmdelivery_new.OVC_VEN_CST = q.OVC_VEN_CST;
                }
                if (txtONB_SHIP_TIMES.Text != "")
                {
                    if (short.TryParse(txtONB_SHIP_TIMES.Text, out short s))
                        tbmdelivery_new.ONB_SHIP_TIMES = short.Parse(txtONB_SHIP_TIMES.Text);
                    else
                        strMessage += "<p>交貨批次 須為數字</p>";
                }
                tbmdelivery_new.OVC_DELIVERY_CONTRACT = txtOVC_DAUDIT.Text;
                tbmdelivery_new.OVC_DELIVERY_PERIOD = txtOVC_DAUDIT_Input.Text;
                if (txtONB_DAYS_CONTRACT.Text != "")
                {
                    if (short.TryParse(txtONB_DAYS_CONTRACT.Text, out short s))
                        tbmdelivery_new.ONB_DAYS_CONTRACT = short.Parse(txtONB_DAYS_CONTRACT.Text);
                    else
                        strMessage += "<p>契約交貨天數 須為數字</p>";
                }
                tbmdelivery_new.OVC_DELIVERY = txtOVC_DELIVERY.Text;
                if (txtONB_MCONTRACT.Text != "")
                {
                    if (decimal.TryParse(txtONB_MCONTRACT.Text, out decimal d))
                        tbmdelivery_new.ONB_MCONTRACT = decimal.Parse(txtONB_MCONTRACT.Text);
                    else
                        strMessage += "<p>契約金額 須為數字</p>";
                }
                if (txtONB_MDELIVERY.Text != "")
                {
                    if (decimal.TryParse(txtONB_MDELIVERY.Text, out decimal d))
                        tbmdelivery_new.ONB_MDELIVERY = decimal.Parse(txtONB_MDELIVERY.Text);
                    else
                        strMessage += "<p>本次交貨金額 須為數字</p>";
                }
                tbmdelivery_new.OVC_DELIVERY_PLACE = txtOVC_DELIVERY_PLACE.Text;
                tbmdelivery_new.OVC_DAIRSHIP = txtOVC_DAIRSHIP.Text;
                tbmdelivery_new.OVC_DAIRSHIP_PLAN = txtOVC_DAIRSHIP_PLAN.Text;
                tbmdelivery_new.OVC_DINFORM = txtOVC_DINFORM.Text;
                tbmdelivery_new.OVC_DINFORM_PLAN = txtOVC_DINFORM_PLAN.Text;
                tbmdelivery_new.OVC_DJOINCHECK = txtOVC_DJOINCHECK.Text;
                tbmdelivery_new.OVC_DJOINCHECK_PLAN = txtOVC_DJOINCHECK_PLAN.Text;
                tbmdelivery_new.OVC_DSHIPMENT = txtOVC_DSHIPMENT.Text;
                tbmdelivery_new.OVC_DSHIPMENT_PLAN = txtOVC_DSHIPMENT_PLAN.Text;
                tbmdelivery_new.OVC_DINVENTORY = txtOVC_DINVENTORY.Text;
                tbmdelivery_new.OVC_DINVENTORY_PLAN = txtOVC_DINVENTORY_PLAN.Text;
                tbmdelivery_new.OVC_DINSPECT = txtOVC_DINSPECT.Text;
                tbmdelivery_new.OVC_DINSPECT_PLAN = txtOVC_DINSPECT_PLAN.Text;
                tbmdelivery_new.OVC_DPAY = txtOVC_DPAY.Text;
                tbmdelivery_new.OVC_DPAY_PLAN = txtOVC_DPAY_PLAN.Text;
                var querydoname =
                    from tbmreceive in mpms.TBMRECEIVE_CONTRACT
                    where tbmreceive.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    where tbmreceive.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        OVC_DO_NAME = tbmreceive.OVC_DO_NAME
                    };
                foreach (var q in querydoname)
                    tbmdelivery_new.OVC_DO_NAME = q.OVC_DO_NAME;
                if (strMessage == "")
                {
                    mpms.TBMDELIVERY.Add(tbmdelivery_new);
                    mpms.SaveChanges();
                    Session["isModify"] = "1";
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
        }

        protected void btnChange_OVC_DELIVERY_Click(object sender, EventArgs e)
        {
            if (Session["isModify"] != null)
            {
                var query =
                    from delivery in mpms.TBMDELIVERY
                    where delivery.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    select delivery.ONB_SHIP_TIMES;
                foreach (var q in query)
                {
                    if (q.ToString() == txtONB_SHIP_TIMES.Text)
                    {
                        TBMDELIVERY tbmdelivery = new TBMDELIVERY();
                        tbmdelivery = mpms.TBMDELIVERY
                            .Where(table => table.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7)) && table.ONB_SHIP_TIMES.Equals(q)).FirstOrDefault();
                        if (tbmdelivery != null)
                        {
                            tbmdelivery.OVC_DELIVERY = txtOVC_DELIVERY.Text;
                            mpms.SaveChanges();
                        }
                    }
                }
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
            }
        }
        #endregion

        #region 列印軍品交貨暨驗收配合注意事項
        protected void btnPrint_Detail_Click(object sender, EventArgs e)
        {
            PrinterServlet_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Precautions_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "軍品交貨暨驗收配合注意事項.docx";
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
        protected void btnPrint_Detail_pdf_Click(object sender, EventArgs e)
        {
            PrinterServlet_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Precautions_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/Precautions_Temp.pdf";
            string fileName = purch + "軍品交貨暨驗收配合注意事項.pdf";
            FCommon.WordToPDF(this, path_temp, wordfilepath, fileName);
        }
        protected void btnPrint_Detail_odt_Click(object sender, EventArgs e)
        {
            PrinterServlet_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Precautions_Temp.docx");
            string wordfilepath = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Precautions_Temp.odt");
            string fileName = purch + "軍品交貨暨驗收配合注意事項.odt";
            FCommon.WordToOdt(this, path_temp, wordfilepath, fileName);
        }

        protected void btnPrint_Result_Click(object sender, EventArgs e)
        {
            PrinterServlet2_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Report_Card_Temp.docx");
            FileInfo file = new FileInfo(path_temp);
            string filepath = purch + "採購接收暨會驗結果報告單.docx";
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
        protected void btnPrint_Result_pdf_Click(object sender, EventArgs e)
        {
            PrinterServlet2_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Report_Card_Temp.docx");
            string wordfilepath = Request.PhysicalApplicationPath + "WordPDFprint/Report_Card_Temp.pdf";
            string fileName = purch + "採購接收暨會驗結果報告單.pdf";
            FCommon.WordToPDF(this, path_temp, wordfilepath, fileName);
        }
        protected void btnPrint_Result_odt_Click(object sender, EventArgs e)
        {
            PrinterServlet2_ExportToWord();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            string path_temp = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Report_Card_Temp.docx");
            string wordfilepath = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/Report_Card_Temp.odt");
            string fileName = purch + "採購接收暨會驗結果報告單.odt";
            FCommon.WordToOdt(this, path_temp, wordfilepath, fileName);
        }
        #endregion

        #endregion

        #region 副程式

        #region Table資料帶入
        private void TB_dataImport()
        {
            if (Session["rowtext"] != null)
            {
                lblOVC_PURCH.Text = Session["rowtext"].ToString();
                txtONB_SHIP_TIMES.Text = Session["shiptime"].ToString();
                var purch = Session["rowtext"].ToString().Substring(0, 7);
                string purch_6 = Session["purch_6"].ToString();
                var queryFirst =
                    from tbmreceive_contract in mpms.TBMRECEIVE_CONTRACT
                    join tbm1301 in mpms.TBM1301_PLAN on tbmreceive_contract.OVC_PURCH equals tbm1301.OVC_PURCH
                    join tbm1302 in mpms.TBM1302 on tbmreceive_contract.OVC_PURCH equals tbm1302.OVC_PURCH
                    where tbmreceive_contract.OVC_PURCH.Equals(purch)
                    where tbmreceive_contract.OVC_PURCH_6.Equals(purch_6)
                    where tbmreceive_contract.OVC_PURCH_6.Equals(tbm1302.OVC_PURCH_6)
                    select new
                    {
                        OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                        OVC_PUR_SECTION = tbm1301.OVC_PUR_SECTION,
                        OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                        OVC_VEN_TITLE = tbmreceive_contract.OVC_VEN_TITLE,
                        OVC_DCONTRACT = tbm1302.OVC_DCONTRACT,
                        OVC_DBID = tbm1302.OVC_DBID
                    };
                foreach (var q in queryFirst)
                {
                    txtOVC_PURCH.Text = q.OVC_PUR_IPURCH;
                    txtOVC_DEPT_CDE.Text = q.OVC_PUR_SECTION;
                    txtOVC_ONNAME.Text = q.OVC_PUR_NSECTION;
                    txtOVC_VEN_TITLE.Text = q.OVC_VEN_TITLE;
                    txtOVC_DCONTRACT.Text = q.OVC_DCONTRACT; // dateTW(q.OVC_DCONTRACT);
                    txtOVC_DBID.Text = dateTW(q.OVC_DBID);
                }

                var query =
                    from tbmdelivery in mpms.TBMDELIVERY
                    where tbmdelivery.OVC_PURCH.Equals(purch)
                    where tbmdelivery.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        ONB_SHIP_TIMES = tbmdelivery.ONB_SHIP_TIMES,
                        OVC_DELIVERY_CONTRACT = tbmdelivery.OVC_DELIVERY_CONTRACT,
                        OVC_DELIVERY_PERIOD = tbmdelivery.OVC_DELIVERY_PERIOD,
                        ONB_DAYS_CONTRACT = tbmdelivery.ONB_DAYS_CONTRACT,
                        OVC_DELIVERY = tbmdelivery.OVC_DELIVERY,
                        ONB_MCONTRACT = tbmdelivery.ONB_MCONTRACT,
                        ONB_MDELIVERY = tbmdelivery.ONB_MDELIVERY,
                        OVC_DELIVERY_PLACE = tbmdelivery.OVC_DELIVERY_PLACE,
                        ONB_DINSPECT_SOP = tbmdelivery.ONB_DINSPECT_SOP,
                        OVC_DAIRSHIP = tbmdelivery.OVC_DAIRSHIP,
                        OVC_DAIRSHIP_PLAN = tbmdelivery.OVC_DAIRSHIP_PLAN,
                        OVC_DINFORM = tbmdelivery.OVC_DINFORM,
                        OVC_DINFORM_PLAN = tbmdelivery.OVC_DINFORM_PLAN,
                        OVC_DJOINCHECK = tbmdelivery.OVC_DJOINCHECK,
                        OVC_DJOINCHECK_PLAN = tbmdelivery.OVC_DJOINCHECK_PLAN,
                        OVC_DSHIPMENT = tbmdelivery.OVC_DSHIPMENT,
                        OVC_DSHIPMENT_PLAN = tbmdelivery.OVC_DSHIPMENT_PLAN,
                        OVC_DINVENTORY = tbmdelivery.OVC_DINVENTORY,
                        OVC_DINVENTORY_PLAN = tbmdelivery.OVC_DINVENTORY_PLAN,
                        OVC_DINSPECT = tbmdelivery.OVC_DINSPECT,
                        OVC_DINSPECT_PLAN = tbmdelivery.OVC_DINSPECT_PLAN,
                        OVC_DPAY = tbmdelivery.OVC_DPAY,
                        OVC_DPAY_PLAN = tbmdelivery.OVC_DPAY_PLAN,
                        OVC_ACCORDING = tbmdelivery.OVC_ACCORDING,
                        OVC_DACCORDING = tbmdelivery.OVC_DACCORDING,
                        OVC_ACCORDING_PLACE = tbmdelivery.OVC_ACCORDING_PLACE
                    };
                foreach (var q in query)
                {
                    if (txtONB_SHIP_TIMES.Text.Equals(q.ONB_SHIP_TIMES.ToString()))
                    {
                        txtOVC_DAUDIT.Text = q.OVC_DELIVERY_CONTRACT;
                        txtOVC_DAUDIT_Input.Text = q.OVC_DELIVERY_PERIOD;
                        txtONB_DAYS_CONTRACT.Text = q.ONB_DAYS_CONTRACT.ToString();
                        txtOVC_DELIVERY.Text = q.OVC_DELIVERY;
                        txtONB_MCONTRACT.Text = q.ONB_MCONTRACT.ToString();
                        txtONB_MDELIVERY.Text = q.ONB_MDELIVERY.ToString();
                        txtOVC_DELIVERY_PLACE.Text = q.OVC_DELIVERY_PLACE;
                        drpONB_DINSPECT_SOP.Text = q.ONB_DINSPECT_SOP.ToString();
                        txtOVC_DAIRSHIP.Text = q.OVC_DAIRSHIP;
                        txtOVC_DAIRSHIP_PLAN.Text = q.OVC_DAIRSHIP_PLAN;
                        txtOVC_DINFORM.Text = q.OVC_DINFORM;
                        txtOVC_DINFORM_PLAN.Text = q.OVC_DINFORM_PLAN;
                        txtOVC_DJOINCHECK.Text = q.OVC_DJOINCHECK;
                        txtOVC_DJOINCHECK_PLAN.Text = q.OVC_DJOINCHECK_PLAN;
                        txtOVC_DSHIPMENT.Text = q.OVC_DSHIPMENT;
                        txtOVC_DSHIPMENT_PLAN.Text = q.OVC_DSHIPMENT_PLAN;
                        txtOVC_DINVENTORY.Text = q.OVC_DINVENTORY;
                        txtOVC_DINVENTORY_PLAN.Text = q.OVC_DINVENTORY_PLAN;
                        txtOVC_DINSPECT.Text = q.OVC_DINSPECT;
                        txtOVC_DINSPECT_PLAN.Text = q.OVC_DINSPECT_PLAN;
                        txtOVC_DPAY.Text = q.OVC_DPAY;
                        txtOVC_DPAY_PLAN.Text = q.OVC_DPAY_PLAN;
                        txtOVC_ACCORDING.Text = q.OVC_ACCORDING;
                        txtOVC_DACCORDING.Text = q.OVC_DACCORDING;
                        txtOVC_ACCORDING_PLACE.Text = q.OVC_ACCORDING_PLACE;
                    }
                }
            }
        }
        private void dtDifference_days(TextBox actual, TextBox preplanning, TextBox result)
        {
            if (actual.Text != "" && preplanning.Text != "")
            {
                DateTime dtActual = DateTime.ParseExact(actual.Text, "yyyy-MM-dd", null);
                DateTime dtPreplanning = DateTime.ParseExact(preplanning.Text, "yyyy-MM-dd", null);
                TimeSpan ts = dtPreplanning - dtActual;
                result.Text = ts.Days.ToString();
                if (int.Parse(result.Text) < 0)
                    result.Text = "落後" + result.Text.Substring(1) + "天";
                else
                    result.Text = "超前" + result.Text + "天";
            }
        }
        #endregion

        #region GridView資料帶入
        private void GV_dataImport()
        {
            int count = 0;
            int count2 = 0;
            short time = 0;
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            if (Session["rowtext"] != null)
            {
                var purch = Session["rowtext"].ToString().Substring(0, 7);
                string purch_6 = Session["purch_6"].ToString();
                var query =
                    from tbminspect_report in mpms.TBMINSPECT_REPORT
                    where tbminspect_report.OVC_PURCH.Equals(purch)
                    where tbminspect_report.OVC_PURCH_6.Equals(purch_6)
                    select tbminspect_report.ONB_TIMES;
                foreach (var q in query)
                {
                    if (q.ToString() == txtONB_SHIP_TIMES.Text)
                    {
                        count++;
                        time = q;
                    }
                }
                var querySituation =
                    from tbminspect_report in mpms.TBMINSPECT_REPORT
                    where tbminspect_report.OVC_PURCH.Equals(purch)
                    where tbminspect_report.OVC_PURCH_6.Equals(purch_6)
                    where tbminspect_report.ONB_TIMES.Equals(time)
                    select new
                    {
                        ONB_INSPECT_TIMES = tbminspect_report.ONB_INSPECT_TIMES,
                        ONB_RE_INSPECT_TIMES = tbminspect_report.ONB_RE_INSPECT_TIMES,
                        OVC_REPORT_DESC = tbminspect_report.OVC_REPORT_DESC,
                        OVC_DINSPECT = tbminspect_report.OVC_DINSPECT,
                        OVC_RESULT_5 = tbminspect_report.OVC_RESULT_5
                    };
                dt = CommonStatic.LinqQueryToDataTable(querySituation);
                FCommon.GridView_dataImport(GV_Situation, dt);

                #region 如果無資料，新增一筆空資料，才會顯示新增
                if (count == 0)
                {                                 
                    dt.Columns.Add(new DataColumn("ONB_INSPECT_TIMES", typeof(string)));
                    dt.Columns.Add(new DataColumn("ONB_RE_INSPECT_TIMES", typeof(string)));
                    dt.Columns.Add(new DataColumn("OVC_REPORT_DESC", typeof(string)));
                    dt.Columns.Add(new DataColumn("OVC_DINSPECT", typeof(string)));
                    dt.Columns.Add(new DataColumn("OVC_RESULT_5", typeof(string)));
                    DataRow dr = null;
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    GV_Situation.DataSource = dt;
                    GV_Situation.DataBind();
                }
                #endregion

                var query2 =
                    from tbminspect_record in mpms.TBMINSPECT_RECORD
                    where tbminspect_record.OVC_PURCH.Equals(purch)
                    where tbminspect_record.OVC_PURCH_6.Equals(purch_6)
                    select tbminspect_record.ONB_TIMES;
                foreach (var q in query2)
                {
                    if (q.ToString() == txtONB_SHIP_TIMES.Text)
                    {
                        count2++;
                        time = q;
                    }
                }
                var queryAcceptance =
                    from tbminspect_record in mpms.TBMINSPECT_RECORD
                    where tbminspect_record.OVC_PURCH.Equals(purch)
                    where tbminspect_record.OVC_PURCH_6.Equals(purch_6)
                    where tbminspect_record.ONB_TIMES.Equals(time)
                    select new
                    {
                        ONB_INSPECT_TIMES = tbminspect_record.ONB_INSPECT_TIMES,
                        ONB_RE_INSPECT_TIMES = tbminspect_record.ONB_RE_INSPECT_TIMES,
                        OVC_DELIVERY_CONTRACT = tbminspect_record.OVC_DELIVERY_CONTRACT,
                        OVC_DELIVERY_PLACE = tbminspect_record.OVC_DELIVERY_PLACE
                    };
                dt2 = CommonStatic.LinqQueryToDataTable(queryAcceptance);
                FCommon.GridView_dataImport(GV_Acceptance, dt2);

                #region 如果無資料，新增一筆空資料，才會顯示新增
                if (count2 == 0)
                {
                    dt2.Columns.Add(new DataColumn("ONB_INSPECT_TIMES", typeof(string)));
                    dt2.Columns.Add(new DataColumn("ONB_RE_INSPECT_TIMES", typeof(string)));
                    dt2.Columns.Add(new DataColumn("OVC_DELIVERY_CONTRACT", typeof(string)));
                    dt2.Columns.Add(new DataColumn("OVC_DELIVERY_PLACE", typeof(string)));
                    DataRow dr2 = null;
                    dr2 = dt2.NewRow();
                    dt2.Rows.Add(dr2);
                    GV_Acceptance.DataSource = dt2;
                    GV_Acceptance.DataBind();
                }
                #endregion
            }
        }
        #endregion
        
        #region GridView_RowDataBound
        protected void GV_Situation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[4].Text = dateTW(e.Row.Cells[4].Text);
                string purch_6 = Session["purch_6"].ToString();
                Label labRESULT1_1 = (Label)e.Row.FindControl("labRESULT1_1");
                Label labRESULT2_1 = (Label)e.Row.FindControl("labRESULT2_1");
                Label labRESULT3_1 = (Label)e.Row.FindControl("labRESULT3_1");
                Label labRESULT4_1 = (Label)e.Row.FindControl("labRESULT4_1");
                if (e.Row.Cells[1].Text == "&nbsp;" && e.Row.Cells[2].Text == "&nbsp;")
                    e.Row.Visible = false;
                var query =
                    from tbminspect_report in mpms.TBMINSPECT_REPORT
                    where tbminspect_report.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    where tbminspect_report.OVC_PURCH_6.Equals(purch_6)
                    select new
                    {
                        ONB_INSPECT_TIMES = tbminspect_report.ONB_INSPECT_TIMES,
                        ONB_RE_INSPECT_TIMES = tbminspect_report.ONB_RE_INSPECT_TIMES,

                        OVC_RESULT_1 = tbminspect_report.OVC_RESULT_1,
                        OVC_RESULT_2 = tbminspect_report.OVC_RESULT_2,
                        OVC_RESULT_2_PERCENT = tbminspect_report.OVC_RESULT_2_PERCENT,
                        OVC_RESULT_3 = tbminspect_report.OVC_RESULT_3,
                        OVC_RESULT_4 = tbminspect_report.OVC_RESULT_4,
                        OVC_RESULT_5 = tbminspect_report.OVC_RESULT_5
                    };
                foreach (var q in query)
                {
                    if (q.ONB_INSPECT_TIMES.ToString() == e.Row.Cells[1].Text && q.ONB_RE_INSPECT_TIMES.ToString() == e.Row.Cells[2].Text)
                    {
                        if (q.OVC_RESULT_1 == "Y")
                            labRESULT1_1.Text = "一、交驗軍品數量會同清點大數相符";
                        else
                            labRESULT1_1.Text = "一、交驗軍品數量與合約規定不符";
                        labRESULT2_1.Text = "二、抽驗" + q.OVC_RESULT_2_PERCENT + "%目視情形";
                        if (q.OVC_RESULT_2 == "Y")
                            labRESULT2_1.Text = labRESULT2_1.Text + "與合約規定相符";
                        else
                            labRESULT2_1.Text = labRESULT2_1.Text + "與合約規定不符";
                        if (q.OVC_RESULT_3 == "Y")
                            labRESULT3_1.Text = "三、包裝情形與合約規定相符";
                        else
                            labRESULT3_1.Text = "三、包裝情形與合約規定不符";
                        if (q.OVC_RESULT_4 == "Y")
                            labRESULT4_1.Text = "四、交貨時間未逾期";
                        else
                            labRESULT4_1.Text = "四、交貨時間逾期";
                        if (q.OVC_RESULT_1 == "Y" && q.OVC_RESULT_2 == "Y" && q.OVC_RESULT_3 == "Y")
                            e.Row.Cells[5].Text = "驗收合格";
                        else
                            e.Row.Cells[5].Text = "驗收不合格";
                    }
                }
            }
        }

        protected void GV_Acceptance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Text = dateTW(e.Row.Cells[3].Text);
                Label labRESULT_1 = (Label)e.Row.FindControl("labRESULT_1");
                Label labRESULT_2 = (Label)e.Row.FindControl("labRESULT_2");
                Label labRESULT_3 = (Label)e.Row.FindControl("labRESULT_3");
                Label labRESULT_4 = (Label)e.Row.FindControl("labRESULT_4");
                Label labRESULT_5 = (Label)e.Row.FindControl("labRESULT_5");
                Label labRESULT_6 = (Label)e.Row.FindControl("labRESULT_6");
                if (e.Row.Cells[1].Text == "&nbsp;" && e.Row.Cells[2].Text == "&nbsp;")
                    e.Row.Visible = false;
                var query =
                    from record in mpms.TBMINSPECT_RECORD
                    where record.OVC_PURCH.Equals(lblOVC_PURCH.Text.Substring(0, 7))
                    select new
                    {
                        ONB_INSPECT_TIMES = record.ONB_INSPECT_TIMES,
                        ONB_RE_INSPECT_TIMES = record.ONB_RE_INSPECT_TIMES,

                        OVC_DELIVERY_CONTRACT = record.OVC_DELIVERY_CONTRACT,
                        OVC_DELIVERY_PLACE = record.OVC_DELIVERY_PLACE,

                        OVC_RESULT_1_1 = record.OVC_RESULT_1_1,
                        OVC_RESULT_2 = record.OVC_RESULT_2,
                        OVC_RESULT_3 = record.OVC_RESULT_3,
                        OVC_RESULT_4 = record.OVC_RESULT_4,
                        OVC_RESULT_5 = record.OVC_RESULT_5,
                        OVC_RESULT_6 = record.OVC_RESULT_6
                    };
                foreach (var q in query)
                {
                    if (q.ONB_INSPECT_TIMES.ToString() == e.Row.Cells[1].Text && q.ONB_RE_INSPECT_TIMES.ToString() == e.Row.Cells[2].Text)
                    {
                        labRESULT_1.Text = q.OVC_RESULT_1_1;
                        labRESULT_2.Text = q.OVC_RESULT_2;
                        labRESULT_3.Text = q.OVC_RESULT_3;
                        labRESULT_4.Text = q.OVC_RESULT_4;
                        labRESULT_5.Text = q.OVC_RESULT_5;
                        labRESULT_6.Text = q.OVC_RESULT_6;
                    }
                }
            }
        }
        #endregion

        #region GridView_RowCreated
        protected void GV_Situation_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //TableCellCollection oldCell = e.Row.Cells;
                //oldCell.Clear();

                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                Label lab = new Label();
                lab.Text = "驗&nbsp;&nbsp;&nbsp;&nbsp;收&nbsp;&nbsp;&nbsp;&nbsp;情&nbsp;&nbsp;&nbsp;&nbsp;形";
                lab.CssClass = "control-label";
                lab.Font.Size = 18;
                lab.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 7;
                gvRow.Cells.Add(tc);
                GV_Situation.Controls[0].Controls.Add(gvRow);
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                GridViewRow Tgvr = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Insert);
                TableCell cell1 = new TableCell();
                TableCell cell2 = new TableCell();
                TableCell cell3 = new TableCell();
                TableCell cell4 = new TableCell();
                TableCell cell5 = new TableCell();
                TableCell cell6 = new TableCell();
                TableCell cell7 = new TableCell();
                Button btn = new Button();
                btn.Text = "新增";
                btn.CssClass = "btn-success";
                btn.PostBackUrl = "~/pages/MPMS/E/MPMS_E28.aspx";
                cell1.Controls.Add(btn);
                Tgvr.Controls.Add(cell1);
                Tgvr.Controls.Add(cell2);
                Tgvr.Controls.Add(cell3);
                Tgvr.Controls.Add(cell4);
                Tgvr.Controls.Add(cell5);
                Tgvr.Controls.Add(cell6);
                Tgvr.Controls.Add(cell7);
                GV_Situation.Controls[0].Controls.Add(Tgvr);
            }
        }

        protected void GV_Acceptance_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc = new TableCell();
                Label lab = new Label();
                lab.Text = "驗&nbsp;&nbsp;&nbsp;&nbsp;收&nbsp;&nbsp;&nbsp;&nbsp;紀&nbsp;&nbsp;&nbsp;&nbsp;錄";
                lab.CssClass = "control-label";
                lab.Font.Size = 18;
                lab.Font.Bold = true;
                tc.Controls.Add(lab);
                tc.ColumnSpan = 6;
                gvRow.Cells.Add(tc);
                GV_Acceptance.Controls[0].Controls.Add(gvRow);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                GridViewRow Tgvr = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Insert);
                TableCell cell1 = new TableCell();
                TableCell cell2 = new TableCell();
                TableCell cell3 = new TableCell();
                TableCell cell4 = new TableCell();
                TableCell cell5 = new TableCell();
                TableCell cell6 = new TableCell();
                Button btn = new Button();
                btn.Text = "新增";
                btn.CssClass = "btn-success";
                btn.PostBackUrl = "~/pages/MPMS/E/MPMS_E29.aspx";
                cell1.Controls.Add(btn);
                Tgvr.Controls.Add(cell1);
                Tgvr.Controls.Add(cell2);
                Tgvr.Controls.Add(cell3);
                Tgvr.Controls.Add(cell4);
                Tgvr.Controls.Add(cell5);
                Tgvr.Controls.Add(cell6);
                GV_Acceptance.Controls[0].Controls.Add(Tgvr);
            }
        }
        #endregion

        #region TextChanged
        protected void txtOVC_DAIRSHIP_TextChanged(object sender, EventArgs e)
        {
            GV_dataImport();
            dtDifference_days(txtOVC_DAIRSHIP, txtOVC_DAIRSHIP_PLAN, txt);
        }
        protected void txtOVC_DAIRSHIP_PLAN_TextChanged(object sender, EventArgs e)
        {
            GV_dataImport();
            dtDifference_days(txtOVC_DAIRSHIP, txtOVC_DAIRSHIP_PLAN, txt);
        }
        protected void txtOVC_DINFORM_TextChanged(object sender, EventArgs e)
        {
            GV_dataImport();
            dtDifference_days(txtOVC_DINFORM, txtOVC_DINFORM_PLAN, txtONB_NO_PUNISH_DAYS_OVC_DINFORM);
        }
        protected void txtOVC_DINFORM_PLAN_TextChanged(object sender, EventArgs e)
        {
            GV_dataImport();
            dtDifference_days(txtOVC_DINFORM, txtOVC_DINFORM_PLAN, txtONB_NO_PUNISH_DAYS_OVC_DINFORM);
        }
        protected void txtOVC_DJOINCHECK_TextChanged(object sender, EventArgs e)
        {
            GV_dataImport();
            dtDifference_days(txtOVC_DJOINCHECK, txtOVC_DJOINCHECK_PLAN, txtONB_NO_PUNISH_DAYS_OVC_DJOINCHECK);
        }
        protected void txtOVC_DJOINCHECK_PLAN_TextChanged(object sender, EventArgs e)
        {
            GV_dataImport();
            dtDifference_days(txtOVC_DJOINCHECK, txtOVC_DJOINCHECK_PLAN, txtONB_NO_PUNISH_DAYS_OVC_DJOINCHECK);
        }
        protected void txtOVC_DSHIPMENT_TextChanged(object sender, EventArgs e)
        {
            GV_dataImport();
            dtDifference_days(txtOVC_DSHIPMENT, txtOVC_DSHIPMENT_PLAN, txtONB_NO_PUNISH_DAYS_OVC_DSHIPMENT);
        }
        protected void txtOVC_DSHIPMENT_PLAN_TextChanged(object sender, EventArgs e)
        {
            GV_dataImport();
            dtDifference_days(txtOVC_DSHIPMENT, txtOVC_DSHIPMENT_PLAN, txtONB_NO_PUNISH_DAYS_OVC_DSHIPMENT);
        }
        protected void txtOVC_DINVENTORY_TextChanged(object sender, EventArgs e)
        {
            GV_dataImport();
            dtDifference_days(txtOVC_DINVENTORY, txtOVC_DINVENTORY_PLAN, txtONB_NO_PUNISH_DAYS_OVC_DINVENTORY);
        }
        protected void txtOVC_DINVENTORY_PLAN_TextChanged(object sender, EventArgs e)
        {
            GV_dataImport();
            dtDifference_days(txtOVC_DINVENTORY, txtOVC_DINVENTORY_PLAN, txtONB_NO_PUNISH_DAYS_OVC_DINVENTORY);
        }
        protected void txtOVC_DINSPECT_TextChanged(object sender, EventArgs e)
        {
            GV_dataImport();
            dtDifference_days(txtOVC_DINSPECT, txtOVC_DINSPECT_PLAN, txtONB_NO_PUNISH_DAYS_OVC_DINSPECT);
        }
        protected void txtOVC_DINSPECT_PLAN_TextChanged(object sender, EventArgs e)
        {
            GV_dataImport();
            dtDifference_days(txtOVC_DINSPECT, txtOVC_DINSPECT_PLAN, txtONB_NO_PUNISH_DAYS_OVC_DINSPECT);
        }
        protected void txtOVC_DPAY_TextChanged(object sender, EventArgs e)
        {
            GV_dataImport();
            dtDifference_days(txtOVC_DPAY, txtOVC_DPAY_PLAN, txtONB_NO_PUNISH_DAYS_OVC_DPAY);
        }
        protected void txtOVC_DPAY_PLAN_TextChanged(object sender, EventArgs e)
        {
            GV_dataImport();
            dtDifference_days(txtOVC_DPAY, txtOVC_DPAY_PLAN, txtONB_NO_PUNISH_DAYS_OVC_DPAY);
        }
        #endregion

        #region 列印軍品交貨暨驗收配合注意事項
        void PrinterServlet_ExportToWord()
        {
            int year = 0;
            string date = "";
            string path = "";
            var userunit = Session["userunit"].ToString();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            var rowven = Session["rowven"].ToString();
            short ship = short.Parse(Session["shiptime"].ToString());
            string purch_6 = Session["purch_6"].ToString();
            path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/PrinterServletE27_1.docx");
            byte[] buffer = null;
            if (Session["isModify"] == null)
                return;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryNSECTION = mpms.TBM1301.Where(o => o.OVC_PURCH.Equals(purch)).FirstOrDefault();
                    doc.ReplaceText("[$OVC_PUR_NSECTION$]", queryNSECTION != null ? queryNSECTION.OVC_PUR_NSECTION : "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_PURCH_KIND$]", queryNSECTION.OVC_PURCH_KIND == "1" ? "內" : "外", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$AGENCY$]", txtOVC_ONNAME.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$VEN_TITLE$]", txtOVC_VEN_TITLE.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$PURCH$]", lblOVC_PURCH.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryPay =
                        from pay in mpms.TBMPAY_MONEY
                        where pay.OVC_PURCH.Equals(purch)
                        where pay.OVC_PURCH_6.Equals(purch_6)
                        where pay.ONB_TIMES.Equals(ship)
                        select new
                        {
                            OVC_PERFORMANCE_LIMIT = pay.OVC_PERFORMANCE_LIMIT,
                            OVC_DFINISH = pay.OVC_DFINISH
                        };
                    foreach (var q in queryPay)
                    {
                        if (q.OVC_PERFORMANCE_LIMIT != null)
                        {
                            year = int.Parse(q.OVC_PERFORMANCE_LIMIT.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_PERFORMANCE_LIMIT.Substring(5, 2) + "月" + q.OVC_PERFORMANCE_LIMIT.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_PERFORMANCE_LIMIT$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        if (q.OVC_DFINISH != null)
                        {
                            year = int.Parse(q.OVC_DFINISH.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DFINISH.Substring(5, 2) + "月" + q.OVC_DFINISH.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DFINISH$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                    doc.ReplaceText("[$OVC_PERFORMANCE_LIMIT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_DFINISH$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryLab =
                        from tbm1301 in mpms.TBM1301
                        join tbmreceive in mpms.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmreceive.OVC_PURCH
                        where tbm1301.OVC_PURCH.Equals(purch)
                        where tbmreceive.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            OVC_LAB = tbm1301.OVC_LAB,
                            ONB_MONEY = tbmreceive.ONB_MONEY
                        };
                    foreach (var q in queryLab)
                    {
                        if (q.OVC_LAB == "1")
                        {
                            if (q.ONB_MONEY < 1000000)
                            {
                                doc.ReplaceText("[$CHK1$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK2$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK3$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK4$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            else if (q.ONB_MONEY >= 1000000 && q.ONB_MONEY < 10000000)
                            {
                                doc.ReplaceText("[$CHK1$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK2$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK3$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK4$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            else if (q.ONB_MONEY >= 10000000 && q.ONB_MONEY < 20000000)
                            {
                                doc.ReplaceText("[$CHK1$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK2$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK3$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK4$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            else if (q.ONB_MONEY >= 20000000)
                            {
                                doc.ReplaceText("[$CHK1$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK2$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK3$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK4$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                        }
                        else
                        {
                            if (q.ONB_MONEY < 1000000)
                            {
                                doc.ReplaceText("[$CHK1$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK2$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK3$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK4$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            else if (q.ONB_MONEY >= 1000000 && q.ONB_MONEY < 50000000)
                            {
                                doc.ReplaceText("[$CHK1$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK2$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK3$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK4$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            else if (q.ONB_MONEY >= 50000000 && q.ONB_MONEY < 100000000)
                            {
                                doc.ReplaceText("[$CHK1$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK2$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK3$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK4$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            else if (q.ONB_MONEY >= 100000000)
                            {
                                doc.ReplaceText("[$CHK1$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK2$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK3$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK4$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                        }
                        if (q.ONB_MONEY != null)
                        {
                            string money = EastAsiaNumericFormatter.FormatWithCulture("Lc", q.ONB_MONEY, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                            doc.ReplaceText("[$MCONTRACT$]", money, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$MCONTRACT$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    {
                        doc.ReplaceText("[$CHK1$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CHK2$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CHK3$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CHK4$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$MCONTRACT$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$DELIVERY_PLACE$]", txtOVC_DELIVERY_PLACE.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    var groceryListTable = doc.Tables.FirstOrDefault(table => table.TableCaption == "Amount_Used");
                    int count = 0;
                    if (groceryListTable != null)
                    {
                        var rowPattern = groceryListTable.Rows[groceryListTable.Rows.Count - 5];
                        rowPattern.Remove();
                        var queryItem =
                            from tbmItem in mpms.TBMDELIVERY_ITEM
                            join tbm1201 in mpms.TBM1201
                                on tbmItem.OVC_PURCH equals tbm1201.OVC_PURCH
                            where tbmItem.ONB_ICOUNT.Equals(tbm1201.ONB_POI_ICOUNT)
                            where tbmItem.OVC_PURCH.Equals(purch)
                            where tbmItem.OVC_PURCH_6.Equals(purch_6)
                            where tbmItem.ONB_SHIP_TIMES.Equals(ship)
                            orderby tbmItem.ONB_ICOUNT
                            select new
                            {
                                ONB_ICOUNT = tbmItem.ONB_ICOUNT,
                                OVC_POI_NSTUFF_CHN = tbm1201.OVC_POI_NSTUFF_CHN,
                                OVC_POI_IUNIT = tbm1201.OVC_POI_IUNIT,
                                ONB_QDELIVERY = tbmItem.ONB_QDELIVERY,
                                ONB_MDELIVERY = tbmItem.ONB_MDELIVERY,
                                ONB_SHIP_TIMES = tbmItem.ONB_SHIP_TIMES
                            };
                        foreach (var q in queryItem)
                        {
                            var newItem = groceryListTable.InsertRow(rowPattern, groceryListTable.RowCount - 4);
                            newItem.ReplaceText("[$ONB_ICOUNT$]", q.ONB_ICOUNT.ToString());
                            newItem.ReplaceText("[$OVC_POI_NSTUFF_CHN$]", q.OVC_POI_NSTUFF_CHN);
                            newItem.ReplaceText("[$OVC_POI_IUNIT$]", q.OVC_POI_IUNIT);
                            newItem.ReplaceText("[$ONB_QDELIVERY$]", q.ONB_QDELIVERY.ToString());
                            newItem.ReplaceText("[$ONB_MDELIVERY$]", q.ONB_MDELIVERY.ToString());
                            newItem.ReplaceText("[$ONB_TIMES$]", q.ONB_SHIP_TIMES.ToString());
                            newItem.ReplaceText("[$TOTLE$]", (q.ONB_QDELIVERY * q.ONB_MDELIVERY).ToString());
                            count = count + int.Parse((q.ONB_QDELIVERY * q.ONB_MDELIVERY).ToString());
                        }
                        string money = EastAsiaNumericFormatter.FormatWithCulture("Lc", count, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                        doc.ReplaceText("[$ALL_MONEY$]", money, false, System.Text.RegularExpressions.RegexOptions.None);
                    }

                    doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/Precautions_Temp.docx");
                }
                buffer = ms.ToArray();
            }
        }
        #endregion

        #region 列印採購接收暨會驗結果報告單
        void PrinterServlet2_ExportToWord()
        {
            int year = 0;
            string date = "";
            string path = "";
            var userunit = Session["userunit"].ToString();
            string purch_6 = Session["purch_6"].ToString();
            var purch = lblOVC_PURCH.Text.Substring(0, 7);
            var rowven = Session["rowven"].ToString();
            short ship = short.Parse(Session["shiptime"].ToString());
            if (Session["isModify"] == null)
                return;
            path = Path.Combine(Request.PhysicalApplicationPath, "WordPDFprint/會驗結果報告單E27.docx");
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DocX doc = DocX.Load(path))
                {
                    var queryNSECTION = mpms.TBM1301.Where(o => o.OVC_PURCH.Equals(purch)).FirstOrDefault();
                    doc.ReplaceText("[$OVC_PUR_NSECTION$]", queryNSECTION != null ? queryNSECTION.OVC_PUR_NSECTION : "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_PURCH_KIND$]", queryNSECTION.OVC_PURCH_KIND == "1" ? "內" : "外", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$AGENCY$]", txtOVC_ONNAME.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$VEN_TITLE$]", txtOVC_VEN_TITLE.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$PURCH$]", lblOVC_PURCH.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryPay =
                        from pay in mpms.TBMPAY_MONEY
                        where pay.OVC_PURCH.Equals(purch)
                        where pay.OVC_PURCH_6.Equals(purch_6)
                        where pay.ONB_TIMES.Equals(ship)
                        select new
                        {
                            OVC_PERFORMANCE_LIMIT = pay.OVC_PERFORMANCE_LIMIT,
                            OVC_DFINISH = pay.OVC_DFINISH
                        };
                    foreach (var q in queryPay)
                    {
                        if (q.OVC_PERFORMANCE_LIMIT != null)
                        {
                            year = int.Parse(q.OVC_PERFORMANCE_LIMIT.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_PERFORMANCE_LIMIT.Substring(5, 2) + "月" + q.OVC_PERFORMANCE_LIMIT.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_PERFORMANCE_LIMIT$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        if (q.OVC_DFINISH != null)
                        {
                            year = int.Parse(q.OVC_DFINISH.ToString().Substring(0, 4)) - 1911;
                            date = year.ToString() + "年" + q.OVC_DFINISH.Substring(5, 2) + "月" + q.OVC_DFINISH.Substring(8, 2) + "日";
                            doc.ReplaceText("[$OVC_DFINISH$]", date, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                    }
                    doc.ReplaceText("[$OVC_PERFORMANCE_LIMIT$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_DFINISH$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    var queryLab =
                        from tbm1301 in mpms.TBM1301
                        join tbmreceive in mpms.TBMRECEIVE_CONTRACT on tbm1301.OVC_PURCH equals tbmreceive.OVC_PURCH
                        where tbm1301.OVC_PURCH.Equals(purch)
                        where tbmreceive.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            OVC_LAB = tbm1301.OVC_LAB,
                            ONB_MONEY = tbmreceive.ONB_MONEY
                        };
                    foreach (var q in queryLab)
                    {
                        if (q.OVC_LAB == "1")
                        {
                            if (q.ONB_MONEY < 1000000)
                            {
                                doc.ReplaceText("[$CHK1$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK2$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK3$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK4$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            else if (q.ONB_MONEY >= 1000000 && q.ONB_MONEY < 10000000)
                            {
                                doc.ReplaceText("[$CHK1$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK2$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK3$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK4$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            else if (q.ONB_MONEY >= 10000000 && q.ONB_MONEY < 20000000)
                            {
                                doc.ReplaceText("[$CHK1$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK2$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK3$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK4$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            else
                            {
                                doc.ReplaceText("[$CHK1$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK2$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK3$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK4$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                        }
                        else
                        {
                            if (q.ONB_MONEY < 1000000)
                            {
                                doc.ReplaceText("[$CHK1$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK2$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK3$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK4$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            else if (q.ONB_MONEY >= 1000000 && q.ONB_MONEY < 50000000)
                            {
                                doc.ReplaceText("[$CHK1$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK2$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK3$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK4$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            else if (q.ONB_MONEY >= 50000000 && q.ONB_MONEY < 100000000)
                            {
                                doc.ReplaceText("[$CHK1$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK2$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK3$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK4$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                            else
                            {
                                doc.ReplaceText("[$CHK1$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK2$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK3$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                                doc.ReplaceText("[$CHK4$]", "■", false, System.Text.RegularExpressions.RegexOptions.None);
                            }
                        }
                        if (q.ONB_MONEY != null)
                        {
                            string money = EastAsiaNumericFormatter.FormatWithCulture("Lc", q.ONB_MONEY, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                            doc.ReplaceText("[$MCONTRACT$]", money, false, System.Text.RegularExpressions.RegexOptions.None);
                        }
                        doc.ReplaceText("[$MCONTRACT$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    {
                        doc.ReplaceText("[$CHK1$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CHK2$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CHK3$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$CHK4$]", "□", false, System.Text.RegularExpressions.RegexOptions.None);
                        doc.ReplaceText("[$MCONTRACT$]", "零", false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$DELIVERY_PLACE$]", txtOVC_DELIVERY_PLACE.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    var groceryListTable = doc.Tables.FirstOrDefault(table => table.TableCaption == "Amount_Used");
                    var groceryListTable_2 = doc.Tables.FirstOrDefault(table => table.TableCaption == "Amount_Used_2");
                    int count = 0;
                    if (groceryListTable != null)
                    {
                        var rowPattern = groceryListTable.Rows[6];
                        rowPattern.Remove();
                        var queryItem =
                            from tbmItem in mpms.TBMDELIVERY_ITEM
                            join tbm1201 in mpms.TBM1201 on tbmItem.OVC_PURCH equals tbm1201.OVC_PURCH
                            where tbmItem.ONB_ICOUNT.Equals(tbm1201.ONB_POI_ICOUNT)
                            where tbmItem.OVC_PURCH.Equals(purch)
                            where tbmItem.OVC_PURCH_6.Equals(purch_6)
                            where tbmItem.ONB_SHIP_TIMES.Equals(ship)
                            orderby tbmItem.ONB_ICOUNT descending
                            select new
                            {
                                ONB_ICOUNT = tbmItem.ONB_ICOUNT,
                                OVC_POI_NSTUFF_CHN = tbm1201.OVC_POI_NSTUFF_CHN,
                                OVC_POI_IUNIT = tbm1201.OVC_POI_IUNIT,
                                ONB_QDELIVERY = tbmItem.ONB_QDELIVERY,
                                ONB_MDELIVERY = tbmItem.ONB_MDELIVERY,
                                ONB_SHIP_TIMES = tbmItem.ONB_SHIP_TIMES
                            };
                        foreach (var q in queryItem)
                        {
                            var newItem = groceryListTable.InsertRow(rowPattern, 6);
                            newItem.ReplaceText("[$ONB_ICOUNT$]", q.ONB_ICOUNT.ToString());
                            newItem.ReplaceText("[$OVC_POI_NSTUFF_CHN$]", q.OVC_POI_NSTUFF_CHN);
                            newItem.ReplaceText("[$OVC_POI_IUNIT$]", q.OVC_POI_IUNIT);
                            newItem.ReplaceText("[$ONB_QDELIVERY$]", q.ONB_QDELIVERY.ToString());
                            newItem.ReplaceText("[$ONB_MDELIVERY$]", q.ONB_MDELIVERY.ToString());
                            newItem.ReplaceText("[$ONB_TIMES$]", q.ONB_SHIP_TIMES.ToString());
                            newItem.ReplaceText("[$TOTLE$]", (q.ONB_QDELIVERY * q.ONB_MDELIVERY).ToString());
                            count = count + int.Parse((q.ONB_QDELIVERY * q.ONB_MDELIVERY).ToString());
                        }
                        string money = EastAsiaNumericFormatter.FormatWithCulture("Lc", count, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                        doc.ReplaceText("[$ALL_MONEY$]", money, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    if (groceryListTable_2 != null)
                    {
                        var rowPattern = groceryListTable_2.Rows[6];
                        rowPattern.Remove();
                        var queryItem =
                            from tbmItem in mpms.TBMDELIVERY_ITEM
                            join tbm1201 in mpms.TBM1201 on tbmItem.OVC_PURCH equals tbm1201.OVC_PURCH
                            where tbmItem.ONB_ICOUNT.Equals(tbm1201.ONB_POI_ICOUNT)
                            where tbmItem.OVC_PURCH.Equals(purch)
                            where tbmItem.OVC_PURCH_6.Equals(purch_6)
                            where tbmItem.ONB_SHIP_TIMES.Equals(ship)
                            orderby tbmItem.ONB_ICOUNT descending
                            select new
                            {
                                ONB_ICOUNT = tbmItem.ONB_ICOUNT,
                                OVC_POI_NSTUFF_CHN = tbm1201.OVC_POI_NSTUFF_CHN,
                                OVC_POI_IUNIT = tbm1201.OVC_POI_IUNIT,
                                ONB_QDELIVERY = tbmItem.ONB_QDELIVERY,
                                ONB_MDELIVERY = tbmItem.ONB_MDELIVERY,
                                ONB_SHIP_TIMES = tbmItem.ONB_SHIP_TIMES
                            };
                        foreach (var q in queryItem)
                        {
                            var newItem = groceryListTable_2.InsertRow(rowPattern, 6);
                            newItem.ReplaceText("[$ONB_ICOUNT$]", q.ONB_ICOUNT.ToString());
                            newItem.ReplaceText("[$OVC_POI_NSTUFF_CHN$]", q.OVC_POI_NSTUFF_CHN);
                            newItem.ReplaceText("[$OVC_POI_IUNIT$]", q.OVC_POI_IUNIT);
                            newItem.ReplaceText("[$ONB_QDELIVERY$]", q.ONB_QDELIVERY.ToString());
                            newItem.ReplaceText("[$ONB_MDELIVERY$]", q.ONB_MDELIVERY.ToString());
                            newItem.ReplaceText("[$ONB_TIMES$]", q.ONB_SHIP_TIMES.ToString());
                            newItem.ReplaceText("[$TOTLE$]", (q.ONB_QDELIVERY * q.ONB_MDELIVERY).ToString());
                            count = count + int.Parse((q.ONB_QDELIVERY * q.ONB_MDELIVERY).ToString());
                        }
                        string money = EastAsiaNumericFormatter.FormatWithCulture("Lc", count, null, new CultureInfo("zh-tw"));//使用 Microsoft Visual Studio International Feature Pack 2.0 轉換數字
                        doc.ReplaceText("[$ALL_MONEY$]", money, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    var queryDel =
                        from tbmdel in mpms.TBMDELIVERY
                        where tbmdel.OVC_PURCH.Equals(purch)
                        where tbmdel.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            OVC_ACCORDING = tbmdel.OVC_ACCORDING,
                            OVC_DACCORDING = tbmdel.OVC_DACCORDING,
                            OVC_ACCORDING_PLACE = tbmdel.OVC_ACCORDING_PLACE
                        };
                    foreach (var q in queryDel)
                    {
                        if (q.OVC_ACCORDING != null)
                            doc.ReplaceText("[$OVC_ACCORDING$]", q.OVC_ACCORDING, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_DACCORDING != null)
                            doc.ReplaceText("[$OVC_DACCORDING$]", dateTW(q.OVC_DACCORDING), false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_ACCORDING_PLACE != null)
                            doc.ReplaceText("[$OVC_ACCORDING_PLACE$]", q.OVC_ACCORDING_PLACE, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_ACCORDING$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_DACCORDING$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_ACCORDING_PLACE$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    var queryIns =
                        from tbmins in mpms.TBMINSPECT_RECORD
                        where tbmins.OVC_PURCH.Equals(purch)
                        where tbmins.OVC_PURCH_6.Equals(purch_6)
                        select new
                        {
                            OVC_RESULT_1_1 = tbmins.OVC_RESULT_1_1,
                            OVC_RESULT_2 = tbmins.OVC_RESULT_2,
                            OVC_RESULT_3 = tbmins.OVC_RESULT_3,
                            OVC_RESULT_4 = tbmins.OVC_RESULT_4,
                            OVC_RESULT_5 = tbmins.OVC_RESULT_5,
                            OVC_RESULT_6 = tbmins.OVC_RESULT_6,
                            OVC_RESULT_SUMMARY = tbmins.OVC_RESULT_SUMMARY
                        };
                    foreach (var q in queryIns)
                    {
                        if (q.OVC_RESULT_1_1 != null)
                            doc.ReplaceText("[$OVC_RESULT_1_1$]", q.OVC_RESULT_1_1, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_RESULT_2 != null)
                            doc.ReplaceText("[$OVC_RESULT_2$]", q.OVC_RESULT_2, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_RESULT_3 != null)
                            doc.ReplaceText("[$OVC_RESULT_3$]", q.OVC_RESULT_3, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_RESULT_4 != null)
                            doc.ReplaceText("[$OVC_RESULT_4$]", q.OVC_RESULT_4, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_RESULT_5 != null)
                            doc.ReplaceText("[$OVC_RESULT_5$]", q.OVC_RESULT_5, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_RESULT_6 != null)
                            doc.ReplaceText("[$OVC_RESULT_6$]", q.OVC_RESULT_6, false, System.Text.RegularExpressions.RegexOptions.None);
                        if (q.OVC_RESULT_SUMMARY != null)
                            doc.ReplaceText("[$OVC_RESULT_SUMMARY$]", q.OVC_RESULT_SUMMARY, false, System.Text.RegularExpressions.RegexOptions.None);
                    }
                    doc.ReplaceText("[$OVC_RESULT_1_1$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_RESULT_2$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_RESULT_3$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_RESULT_4$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_RESULT_5$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_RESULT_6$]", "", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("[$OVC_RESULT_SUMMARY$]", "", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.ReplaceText("軍備局採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("軍備局", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);
                    doc.ReplaceText("採購中心", "國防採購室", false, System.Text.RegularExpressions.RegexOptions.None);

                    doc.SaveAs(Request.PhysicalApplicationPath + "WordPDFprint/Report_Card_Temp.docx");
                }
                buffer = ms.ToArray();
            }
        }
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
        #endregion
    }
}