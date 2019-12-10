using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E29 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            if (Session["rowtext"] != null && Session["shiptime"] != null)
            {
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtONB_SHIP_TIMES, txtOVC_DAUDIT, txtOVC_DELIVERY);
                    TB_dataImport();
                }
            }
            else
                FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
        }

        #region Click

        //清除日期btn
        protected void btnC_OVC_DELIVERY_Click(object sender, EventArgs e)
        {
            txtOVC_DELIVERY.Text = "";
        }
        // 回上一頁
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E27.aspx";
            Response.Redirect(send_url);
        }
        //回主流程
        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }
        //存檔
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtONB_MDELIVERY.Text, out int i))
            {
                if (Session["rowtext"] != null)
                {
                    string purch_6 = Session["purch_6"].ToString();
                    string purch = Session["rowtext"].ToString();
                    short shiptime = short.Parse(Session["shiptime"].ToString());
                    if (Session["isModify_2"] != null)//修改
                    {
                        short INSPECT_TIMES = short.Parse(Session["INSPECT_TIMES"].ToString());
                        short RE_INSPECT_TIMES = short.Parse(Session["RE_INSPECT_TIMES"].ToString());

                        TBMINSPECT_RECORD tbminspect_record = new TBMINSPECT_RECORD();
                        tbminspect_record = mpms.TBMINSPECT_RECORD
                            .Where(table => purch.Contains(table.OVC_PURCH) && table.OVC_PURCH_6.Equals(purch_6))
                            .Where(table => table.ONB_TIMES.Equals(shiptime) && table.ONB_INSPECT_TIMES.Equals(INSPECT_TIMES) && table.ONB_RE_INSPECT_TIMES.Equals(RE_INSPECT_TIMES)).FirstOrDefault();
                        if (tbminspect_record != null)
                        {
                            //tbminspect_record.ONB_INSPECT_TIMES = short.Parse(drpONB_INSPECT_TIMES.Text);
                            //tbminspect_record.ONB_RE_INSPECT_TIMES = short.Parse(drpONB_RE_INSPECT_TIMES.Text);
                            tbminspect_record.OVC_DELIVERY_CONTRACT = txtOVC_DAUDIT.Text;
                            tbminspect_record.OVC_DELIVERY = txtOVC_DELIVERY.Text;
                            tbminspect_record.OVC_DELIVERY_PLACE = txtOVC_DELIVERY_PLACE.Text;
                            tbminspect_record.ONB_MDELIVERY = decimal.Parse(txtONB_MDELIVERY.Text);
                            tbminspect_record.OVC_RESULT_1_1 = txtOVC_RESULT_1_1.Text;
                            tbminspect_record.OVC_RESULT_2 = txtOVC_RESULT_2.Text;
                            tbminspect_record.OVC_RESULT_3 = txtOVC_RESULT_3.Text;
                            tbminspect_record.OVC_RESULT_4 = txtOVC_RESULT_4.Text;
                            tbminspect_record.OVC_RESULT_5 = TextBox4.Text;
                            tbminspect_record.OVC_RESULT_6 = txtOVC_RESULT_6.Text;
                            tbminspect_record.OVC_RESULT_SUMMARY = txtOVC_RESULT_SUMMARY.Text;
                            mpms.SaveChanges();
                        }
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                    }
                    else
                    {
                        short INSPECT_TIMES = short.Parse(drpONB_INSPECT_TIMES.Text);
                        short RE_INSPECT_TIMES = short.Parse(drpONB_RE_INSPECT_TIMES.Text);
                        var queryPurch =
                            (from tbm1301 in mpms.TBM1301
                             join tbm1302 in mpms.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                             where purch.Contains(tbm1301.OVC_PURCH)
                             where tbm1302.OVC_PURCH_6.Equals(purch_6)
                             select new
                             {
                                 OVC_PURCH = tbm1301.OVC_PURCH,
                                 OVC_PURCH_KIND = tbm1301.OVC_PURCH_KIND,
                                 OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                                 OVC_VEN_CST = tbm1302.OVC_VEN_CST
                             }).FirstOrDefault();
                        var queryIns = mpms.TBMINSPECT_RECORD
                            .Where(o => o.OVC_PURCH.Equals(queryPurch.OVC_PURCH) && o.OVC_PURCH_6.Equals(queryPurch.OVC_PURCH_6))
                            .Where(o => o.ONB_TIMES.Equals(shiptime) && o.ONB_INSPECT_TIMES.Equals(INSPECT_TIMES) && o.ONB_RE_INSPECT_TIMES.Equals(RE_INSPECT_TIMES)).FirstOrDefault();
                        if (queryIns != null)
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "副驗次數及再驗次數重複");
                        else
                        {
                            TBMINSPECT_RECORD tbminspect_record_new = new TBMINSPECT_RECORD();
                            tbminspect_record_new.OVC_PURCH = queryPurch.OVC_PURCH;
                            tbminspect_record_new.OVC_PURCH_6 = queryPurch.OVC_PURCH_6;
                            tbminspect_record_new.OVC_VEN_CST = queryPurch.OVC_VEN_CST;
                            tbminspect_record_new.ONB_TIMES = shiptime;
                            tbminspect_record_new.ONB_INSPECT_TIMES = short.Parse(drpONB_INSPECT_TIMES.Text);
                            tbminspect_record_new.ONB_RE_INSPECT_TIMES = short.Parse(drpONB_RE_INSPECT_TIMES.Text);
                            tbminspect_record_new.OVC_DELIVERY_CONTRACT = txtOVC_DAUDIT.Text;
                            tbminspect_record_new.OVC_DELIVERY = txtOVC_DELIVERY.Text;
                            tbminspect_record_new.OVC_DELIVERY_PLACE = txtOVC_DELIVERY_PLACE.Text;
                            if (decimal.TryParse(txtONB_MDELIVERY.Text, out decimal n))
                                tbminspect_record_new.ONB_MDELIVERY = decimal.Parse(txtONB_MDELIVERY.Text);
                            var queryDoName = mpms.TBMRECEIVE_CONTRACT
                                .Where(o => o.OVC_PURCH.Equals(queryPurch.OVC_PURCH))
                                .Where(o => o.OVC_PURCH_6.Equals(queryPurch.OVC_PURCH_6)).FirstOrDefault();
                            tbminspect_record_new.OVC_DO_NAME = queryDoName.OVC_DO_NAME;
                            tbminspect_record_new.OVC_RESULT_1_1 = txtOVC_RESULT_1_1.Text;
                            tbminspect_record_new.OVC_RESULT_2 = txtOVC_RESULT_2.Text;
                            tbminspect_record_new.OVC_RESULT_3 = txtOVC_RESULT_3.Text;
                            tbminspect_record_new.OVC_RESULT_4 = txtOVC_RESULT_4.Text;
                            tbminspect_record_new.OVC_RESULT_5 = TextBox4.Text;
                            tbminspect_record_new.OVC_RESULT_6 = txtOVC_RESULT_6.Text;
                            tbminspect_record_new.OVC_RESULT_SUMMARY = txtOVC_RESULT_SUMMARY.Text;
                            mpms.TBMINSPECT_RECORD.Add(tbminspect_record_new);
                            mpms.SaveChanges();
                        }
                        Session["isModify_2"] = "1";
                        Session["INSPECT_TIMES"] = INSPECT_TIMES;
                        Session["RE_INSPECT_TIMES"] = RE_INSPECT_TIMES;
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
                    }
                }
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "本次交貨金額 須為數字");
        }

        #endregion

        #region 副程式

        #region Table資料帶入
        private void TB_dataImport()
        {
            if (Session["rowtext"] != null)
            {
                string purch_6 = Session["purch_6"].ToString();
                lblOVC_PURCH.Text = Session["rowtext"].ToString();
                txtONB_SHIP_TIMES.Text = Session["shiptime"].ToString();
                var purch = Session["rowtext"].ToString().Substring(0, 7);
                string Purch = Session["rowtext"].ToString();
                if (Session["shiptime"] != null)
                    txtONB_SHIP_TIMES.Text = Session["shiptime"].ToString();
                if (Session["INSPECT_TIMES"] == null)
                {
                    drpONB_INSPECT_TIMES.Text = "0";
                    drpONB_RE_INSPECT_TIMES.Text = "0";
                }
                else
                {
                    drpONB_INSPECT_TIMES.Text = Session["INSPECT_TIMES"].ToString();
                    drpONB_RE_INSPECT_TIMES.Text = Session["RE_INSPECT_TIMES"].ToString();
                }
                if (Session["isModify_2"] != null)
                {
                    var query =
                        from record in mpms.TBMINSPECT_RECORD
                        join tbmreceive in mpms.TBMRECEIVE_CONTRACT on record.OVC_PURCH equals tbmreceive.OVC_PURCH
                        where record.OVC_PURCH.Equals(purch)
                        where tbmreceive.OVC_PURCH_6.Equals(purch_6)
                        where record.OVC_PURCH_6.Equals(tbmreceive.OVC_PURCH_6)
                        select new
                        {
                            ONB_TIMES = record.ONB_TIMES,
                            ONB_INSPECT_TIMES = record.ONB_INSPECT_TIMES,
                            ONB_RE_INSPECT_TIMES = record.ONB_RE_INSPECT_TIMES,
                            OVC_PUR_NSECTION = tbmreceive.OVC_PUR_NSECTION,
                            OVC_VEN_TITLE = tbmreceive.OVC_VEN_TITLE,
                            OVC_DELIVERY_CONTRACT = record.OVC_DELIVERY_CONTRACT,
                            OVC_DELIVERY = record.OVC_DELIVERY,
                            OVC_DELIVERY_PLACE = record.OVC_DELIVERY_PLACE,
                            ONB_MDELIVERY = record.ONB_MDELIVERY,

                            OVC_RESULT_1_1 = record.OVC_RESULT_1_1,
                            OVC_RESULT_2 = record.OVC_RESULT_2,
                            OVC_RESULT_3 = record.OVC_RESULT_3,
                            OVC_RESULT_4 = record.OVC_RESULT_4,
                            OVC_RESULT_5 = record.OVC_RESULT_5,
                            OVC_RESULT_6 = record.OVC_RESULT_6,
                            OVC_RESULT_SUMMARY = record.OVC_RESULT_SUMMARY
                        };
                    foreach (var q in query)
                    {
                        txtOVC_PUR_AGENCY.Text = q.OVC_PUR_NSECTION;
                        txtOVC_VEN_TITLE.Text = q.OVC_VEN_TITLE;
                        if (q.ONB_TIMES.ToString() == Session["shiptime"].ToString() && q.ONB_INSPECT_TIMES.ToString() == drpONB_INSPECT_TIMES.Text && q.ONB_RE_INSPECT_TIMES.ToString() == drpONB_RE_INSPECT_TIMES.Text)
                        {
                            drpONB_INSPECT_TIMES.Text = q.ONB_INSPECT_TIMES.ToString();
                            drpONB_RE_INSPECT_TIMES.Text = q.ONB_RE_INSPECT_TIMES.ToString();
                            txtOVC_DAUDIT.Text = q.OVC_DELIVERY_CONTRACT;
                            txtOVC_DELIVERY.Text = q.OVC_DELIVERY;
                            txtOVC_DELIVERY_PLACE.Text = q.OVC_DELIVERY_PLACE;
                            txtONB_MDELIVERY.Text = q.ONB_MDELIVERY.ToString();
                        }
                        if (Session["isModify_2"] != null && q.ONB_INSPECT_TIMES.ToString() == drpONB_INSPECT_TIMES.Text && q.ONB_RE_INSPECT_TIMES.ToString() == drpONB_RE_INSPECT_TIMES.Text)
                        {
                            txtOVC_RESULT_1_1.Text = q.OVC_RESULT_1_1;
                            txtOVC_RESULT_2.Text = q.OVC_RESULT_2;
                            txtOVC_RESULT_3.Text = q.OVC_RESULT_3;
                            txtOVC_RESULT_4.Text = q.OVC_RESULT_4;
                            TextBox4.Text = q.OVC_RESULT_5;
                            txtOVC_RESULT_6.Text = q.OVC_RESULT_6;
                            txtOVC_RESULT_SUMMARY.Text = q.OVC_RESULT_SUMMARY;
                        }
                    }
                }
                else
                {
                    var queryPurch =
                        (from tbm1301 in mpms.TBM1301
                         join tbm1302 in mpms.TBM1302 on tbm1301.OVC_PURCH equals tbm1302.OVC_PURCH
                         where Purch.Contains(tbm1301.OVC_PURCH)
                         where tbm1302.OVC_PURCH_6.Equals(purch_6)
                         select new
                         {
                             OVC_PURCH = tbm1301.OVC_PURCH,
                             OVC_PURCH_KIND = tbm1301.OVC_PURCH_KIND,
                             OVC_PURCH_6 = tbm1302.OVC_PURCH_6,
                             OVC_VEN_CST = tbm1302.OVC_VEN_CST,
                             OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                             OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE
                         }).FirstOrDefault();
                    txtOVC_PUR_AGENCY.Text = queryPurch.OVC_PUR_NSECTION;
                    txtOVC_VEN_TITLE.Text = queryPurch.OVC_VEN_TITLE;
                }
            }
        }
        #endregion

        #region SelectedIndexChanged
        protected void chkOVC_RESULT_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOVC_RESULT_2.Text = "";
            if (chkOVC_RESULT_2.Items[0].Selected == true)
                txtOVC_RESULT_2.Text += chkOVC_RESULT_2.Items[0].Text + "   ";
            if (chkOVC_RESULT_2.Items[1].Selected == true)
                txtOVC_RESULT_2.Text += chkOVC_RESULT_2.Items[1].Text + "   ";
        }
        protected void chkOVC_RESULT_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOVC_RESULT_3.Text = "";
            if (chkOVC_RESULT_3.Items[0].Selected == true)
                txtOVC_RESULT_3.Text += chkOVC_RESULT_3.Items[0].Text + "   ";
            if (chkOVC_RESULT_3.Items[1].Selected == true)
                txtOVC_RESULT_3.Text += chkOVC_RESULT_3.Items[1].Text + "   ";
        }
        protected void chkOVC_RESULT_4_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOVC_RESULT_4.Text = "";
            if (chkOVC_RESULT_4.Items[0].Selected == true)
                txtOVC_RESULT_4.Text += chkOVC_RESULT_4.Items[0].Text + "   ";
            if (chkOVC_RESULT_4.Items[1].Selected == true)
                txtOVC_RESULT_4.Text += chkOVC_RESULT_4.Items[1].Text + "   ";
        }
        protected void chkOVC_RESULT_5_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox4.Text = "";
            if (chkOVC_RESULT_5.Items[0].Selected == true)
                TextBox4.Text += chkOVC_RESULT_5.Items[0].Text + "   ";
            if (chkOVC_RESULT_5.Items[1].Selected == true)
                TextBox4.Text += chkOVC_RESULT_5.Items[1].Text + "   ";
        }
        protected void chkOVC_RESULT_6_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOVC_RESULT_6.Text = "";
            if (chkOVC_RESULT_6.Items[0].Selected == true)
                txtOVC_RESULT_6.Text += chkOVC_RESULT_6.Items[0].Text + "   ";
            if (chkOVC_RESULT_6.Items[1].Selected == true)
                txtOVC_RESULT_6.Text += chkOVC_RESULT_6.Items[1].Text + "   ";
            if (chkOVC_RESULT_6.Items[2].Selected == true)
                txtOVC_RESULT_6.Text += chkOVC_RESULT_6.Items[2].Text + "   ";
            if (chkOVC_RESULT_6.Items[3].Selected == true)
                txtOVC_RESULT_6.Text += chkOVC_RESULT_6.Items[3].Text + "   ";
            if (chkOVC_RESULT_6.Items[4].Selected == true)
                txtOVC_RESULT_6.Text += chkOVC_RESULT_6.Items[4].Text + "   ";
            if (chkOVC_RESULT_6.Items[5].Selected == true)
                txtOVC_RESULT_6.Text += chkOVC_RESULT_6.Items[5].Text + "   ";
        }

        protected void chkOVC_RESULT_1_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOVC_RESULT_1_1.Text = "";
            if (chkOVC_RESULT_1_1.Items[0].Selected == true)
                txtOVC_RESULT_1_1.Text += chkOVC_RESULT_1_1.Items[0].Text + "   ";
        }
        #endregion

        #endregion
    }
}