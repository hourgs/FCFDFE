using System;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;
using System.Web.UI;
using Microsoft.International.Formatters;
using System.Globalization;

namespace FCFDFE.pages.MPMS.E
{
    public partial class MPMS_E34 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["rowtext"] != null && Session["time"] != null)
            {
                if (!IsPostBack)
                {
                    FCommon.Controls_Attributes("readonly", "true", txtOVC_DMODIFY);
                    dataImport();
                }
            }
            else
                FCommon.MessageBoxShow(this, "查無購案！", $"MPMS_E11", false);
        }

        #region Click
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E15.aspx";
            Response.Redirect(send_url);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            string send_url;
            send_url = "~/pages/MPMS/E/MPMS_E33.aspx";
            Response.Redirect(send_url);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtOVC_DMODIFY.Text = "";
        }

        #region 存檔
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            string purch = Session["rowtext"].ToString().Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            short time = short.Parse(Session["time"].ToString());
            var tbmCONTRACT_MODIFY = mpms.TBMCONTRACT_MODIFY.Where(o => o.OVC_PURCH.Equals(purch) && o.OVC_PURCH_6.Equals(purch_6) && o.ONB_TIMES.Equals(time)).FirstOrDefault();

            decimal decONB_MODIFIED;
            bool boolONB_MODIFIED = FCommon.checkDecimal(txtONB_MODIFIED.Text, "金額", ref strMessage, out decONB_MODIFIED);

            if (tbmCONTRACT_MODIFY != null)
            {
                tbmCONTRACT_MODIFY.OVC_DMODIFY = txtOVC_DMODIFY.Text;
                for (int i = 1; i < 9; i++)
                {
                    RadioButton rb = (RadioButton)tb.FindControl("rb0" + i);
                    if (rb.Checked == true)
                    {
                        tbmCONTRACT_MODIFY.OVC_MODIFY_KIND = "0" + i.ToString();
                        if (i == 8) tbmCONTRACT_MODIFY.OVC_REASON_MODIFY = txtOVC_REASON_MODIFY.Text; else tbmCONTRACT_MODIFY.OVC_REASON_MODIFY = null;
                    }
                }
                tbmCONTRACT_MODIFY.OVC_KIND = rdOVC_KIND.SelectedIndex != -1 ? rdOVC_KIND.SelectedValue : null;
                tbmCONTRACT_MODIFY.ONB_MODIFIED = boolONB_MODIFIED ? decONB_MODIFIED : 0;
                tbmCONTRACT_MODIFY.OVC_MAIN_SUB = rbOVC_MAIN_SUB.SelectedIndex != -1 ? rbOVC_MAIN_SUB.SelectedValue : null;
                tbmCONTRACT_MODIFY.OVC_LAW_NO = drpOVC_LAW_NO.SelectedValue;
                if (strMessage == "")
                {
                    mpms.SaveChanges();
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "修改成功");
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
            else
            {
                TBMCONTRACT_MODIFY tbmCONTRACT_MODIFY_new = new TBMCONTRACT_MODIFY();
                tbmCONTRACT_MODIFY_new.OVC_PURCH = purch;
                tbmCONTRACT_MODIFY_new.OVC_PURCH_6 = purch_6;
                tbmCONTRACT_MODIFY_new.OVC_VEN_CST = mpms.TBM1302.Where(o => o.OVC_PURCH.Equals(purch) && o.OVC_PURCH_6.Equals(purch_6)).FirstOrDefault().OVC_VEN_CST;
                tbmCONTRACT_MODIFY_new.ONB_TIMES = short.Parse(labONB_TIMES.Text);
                tbmCONTRACT_MODIFY_new.OVC_DMODIFY = txtOVC_DMODIFY.Text;
                tbmCONTRACT_MODIFY_new.OVC_DO_NAME = Session["username"].ToString();
                for (int i = 1; i < 9; i++)
                {
                    RadioButton rb = (RadioButton)tb.FindControl("rb0" + i);
                    if (rb.Checked == true)
                    {
                        tbmCONTRACT_MODIFY_new.OVC_MODIFY_KIND = "0" + i.ToString();
                        if (i == 8) tbmCONTRACT_MODIFY_new.OVC_REASON_MODIFY = txtOVC_REASON_MODIFY.Text; else tbmCONTRACT_MODIFY_new.OVC_REASON_MODIFY = null;
                    }
                }
                tbmCONTRACT_MODIFY_new.OVC_KIND = rdOVC_KIND.SelectedIndex != -1 ? rdOVC_KIND.SelectedValue : null;
                tbmCONTRACT_MODIFY_new.ONB_MODIFIED = boolONB_MODIFIED ? decONB_MODIFIED : 0;
                tbmCONTRACT_MODIFY_new.OVC_MAIN_SUB = rbOVC_MAIN_SUB.SelectedIndex != -1 ? rbOVC_MAIN_SUB.SelectedValue : null;
                tbmCONTRACT_MODIFY_new.OVC_LAW_NO = drpOVC_LAW_NO.SelectedValue;
                if (strMessage == "")
                {
                    mpms.TBMCONTRACT_MODIFY.Add(tbmCONTRACT_MODIFY_new);
                    mpms.SaveChanges();
                    Session["time"] = labONB_TIMES.Text;
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功");
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
            }
        }
        #endregion
        #endregion

        #region rb_CheckChanged
        protected void rb01_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 1; i < 9; i++)
            {
                RadioButton rb = (RadioButton)tb.FindControl("rb0" + i);
                rb.Checked = i == 1 ? true : false;
            }
        }

        protected void rb02_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 1; i < 9; i++)
            {
                RadioButton rb = (RadioButton)tb.FindControl("rb0" + i);
                rb.Checked = i == 2 ? true : false;
            }
        }

        protected void rb03_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 1; i < 9; i++)
            {
                RadioButton rb = (RadioButton)tb.FindControl("rb0" + i);
                rb.Checked = i == 3 ? true : false;
            }
        }

        protected void rb04_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 1; i < 9; i++)
            {
                RadioButton rb = (RadioButton)tb.FindControl("rb0" + i);
                rb.Checked = i == 4 ? true : false;
            }
        }

        protected void rb05_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 1; i < 9; i++)
            {
                RadioButton rb = (RadioButton)tb.FindControl("rb0" + i);
                rb.Checked = i == 5 ? true : false;
            }
        }

        protected void rb06_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 1; i < 9; i++)
            {
                RadioButton rb = (RadioButton)tb.FindControl("rb0" + i);
                rb.Checked = i == 6 ? true : false;
            }
        }

        protected void rb07_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 1; i < 9; i++)
            {
                RadioButton rb = (RadioButton)tb.FindControl("rb0" + i);
                rb.Checked = i == 7 ? true : false;
            }
        }

        protected void rb08_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 1; i < 9; i++)
            {
                RadioButton rb = (RadioButton)tb.FindControl("rb0" + i);
                rb.Checked = i == 8 ? true : false;
            }
        }
        #endregion

        #region DataImport
        private void dataImport()
        {
            string purch = Session["rowtext"].ToString().Substring(0, 7);
            string purch_6 = Session["purch_6"].ToString();
            short time = short.Parse(Session["time"].ToString());
            var tbmCONTRACT_MODIFY = mpms.TBMCONTRACT_MODIFY.Where(o => o.OVC_PURCH.Equals(purch) && o.OVC_PURCH_6.Equals(purch_6) && o.ONB_TIMES.Equals(time)).FirstOrDefault();
            var tbm1302 = mpms.TBM1302.Where(o => o.OVC_PURCH.Equals(purch) && o.OVC_PURCH_6.Equals(purch_6)).FirstOrDefault();
            var tbm1301 = mpms.TBM1301.Where(o => o.OVC_PURCH.Equals(purch)).FirstOrDefault();

            labOVC_PURCH.Text = Session["rowtext"].ToString();
            if (tbm1301 != null)
            {
                labOVC_PUR_NSECTION.Text = !tbm1301.OVC_PUR_NSECTION.Equals(null) ? tbm1301.OVC_PUR_NSECTION : "";
                labOVC_PUR_IPURCH.Text = !tbm1301.OVC_PUR_IPURCH.Equals(null) ? tbm1301.OVC_PUR_IPURCH : "";
            }
            if (tbm1302 != null)
            {
                labONB_MONEY.Text = !tbm1302.ONB_MONEY.Equals(null) ? EastAsiaNumericFormatter.FormatWithCulture("Lc", tbm1302.ONB_MONEY, null, new CultureInfo("zh-tw")) + "元整" : "零元整";
                labOVC_DBID.Text = tbm1302.OVC_DBID==null? "" : dateTW(tbm1302.OVC_DBID);
                labOVC_VEN_TITLE.Text = !tbm1302.OVC_VEN_TITLE.Equals(null) ? tbm1302.OVC_VEN_TITLE : "";
                labOVC_DCONTRACT.Text = !tbm1302.OVC_DCONTRACT.Equals(null) ? dateTW(tbm1302.OVC_DCONTRACT) : "";
            }
            if (tbmCONTRACT_MODIFY != null)
            {
                labONB_TIMES.Text = tbmCONTRACT_MODIFY.ONB_TIMES.ToString();
                txtOVC_DMODIFY.Text = tbmCONTRACT_MODIFY.OVC_DMODIFY;
                txtOVC_REASON_MODIFY.Text = "";
                switch (tbmCONTRACT_MODIFY.OVC_MODIFY_KIND)
                {
                    case "01":
                        rb01.Checked = true;
                        break;
                    case "02":
                        rb02.Checked = true;
                        break;
                    case "03":
                        rb03.Checked = true;
                        break;
                    case "04":
                        rb04.Checked = true;
                        break;
                    case "05":
                        rb05.Checked = true;
                        break;
                    case "06":
                        rb06.Checked = true;
                        break;
                    case "07":
                        rb07.Checked = true;
                        break;
                    case "08":
                        rb08.Checked = true;
                        txtOVC_REASON_MODIFY.Text = tbmCONTRACT_MODIFY.OVC_REASON_MODIFY;
                        break;
                    default:
                        break;
                }
                rdOVC_KIND.SelectedValue = tbmCONTRACT_MODIFY.OVC_KIND;
                txtONB_MODIFIED.Text = tbmCONTRACT_MODIFY.ONB_MODIFIED != null ? String.Format("{0:N}", tbmCONTRACT_MODIFY.ONB_MODIFIED ?? 0) : "";
                rbOVC_MAIN_SUB.SelectedValue = tbmCONTRACT_MODIFY.OVC_MAIN_SUB;
                drpOVC_LAW_NO.SelectedValue = tbmCONTRACT_MODIFY.OVC_LAW_NO;
            }
            else
            {
                var queryTime =
                    from con in mpms.TBMCONTRACT_MODIFY
                    where con.OVC_PURCH.Equals(purch)
                    where con.OVC_PURCH_6.Equals(purch_6)
                    select con.ONB_TIMES;
                foreach (var q in queryTime)
                    if (time <= q) time = q;
                labONB_TIMES.Text = (time + 1).ToString();
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
    }
}