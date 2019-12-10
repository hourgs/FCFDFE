using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.Web.UI;

namespace FCFDFE.pages.MTS.B
{
    public partial class MTS_B22_3 : Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        TBGMT_EDF edf = new TBGMT_EDF();
        GMEntities GME = new GMEntities();
        DateTime dateNow = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

        #region 副程式
        public decimal getRate(string currency)
        {
            if (currency == "NTD")
                return 1;
            else
            {
                DateTime dateNowM = dateNow.AddMonths(-1);
                var query = MTSE.TBGMT_CURRENCY_RATE
                    .Where(c => c.OVC_CURRENCY_CODE.Equals(currency))
                    .Where(c => c.ODT_DATE <= dateNow && c.ODT_DATE >= dateNowM)
                    .OrderByDescending(c => c.ODT_DATE).FirstOrDefault();
                if (query != null)
                    return query.ONB_RATE;
                else //找不到近一個月內的匯率
                    return -1;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            FCommon.Controls_Attributes("readonly", "true", txtOVC_EINN_NO, txtODT_INS_DATE);
            if (!IsPostBack)
            {
                string enkey = Request.QueryString["OVC_EDF_NO"];
                string edf_no = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(enkey));
                //軍種下拉選單
                DataTable dtMILITARY_TYPE = CommonStatic.ListToDataTable(MTSE.TBGMT_DEPT_CLASS.OrderBy(c => c.OVC_CLASS_NAME).ToList());
                FCommon.list_dataImport(drpOVC_MILITARY_TYPE, dtMILITARY_TYPE, "OVC_CLASS_NAME", "OVC_CLASS", true);

                #region 初始資料載入
                lbOVC_EDF_NO.Text = edf.ToString();
                var query = MTSE.TBGMT_EDF.Join(MTSE.TBGMT_EINN, f => f.OVC_EDF_NO, n => n.OVC_EDF_NO, (f, n) => new { f, n })
                    .Where(table => table.f.OVC_EDF_NO.Equals(edf_no)).FirstOrDefault();
                if (query != null)
                {
                    var query_detail = MTSE.TBGMT_EDF_DETAIL.Where(table => table.OVC_EDF_NO.Equals(edf_no));
                    var ports =
                        from ed in MTSE.TBGMT_EDF
                        join p in MTSE.TBGMT_PORTS on ed.OVC_START_PORT equals p.OVC_PORT_CDE
                        join p1 in MTSE.TBGMT_PORTS on ed.OVC_ARRIVE_PORT equals p1.OVC_PORT_CDE
                        select new
                        {
                            OVC_START_PORT = p.OVC_PORT_CHI_NAME,
                            OVC_ARRIVE_PORT = p1.OVC_PORT_CHI_NAME
                        };
                    string moneycount = query_detail.Sum(table => table.ONB_MONEY).ToString();
                    string moneycurrency = query_detail.Join(MTSE.TBGMT_CURRENCY, d => d.OVC_CURRENCY, c => c.OVC_CURRENCY_CODE, (d, c) => new { d, c })
                        .FirstOrDefault().c.OVC_CURRENCY_NAME;

                    txtOVC_EINN_NO.Text = query.n.OVC_EINN_NO;
                    lbltitle.Text = query.n.OVC_EINN_NO;
                    lbOVC_EDF_NO.Text = query.f.OVC_EDF_NO;
                    lbOVC_PURCH_NO.Text = query.f.OVC_PURCH_NO;
                    lbOVC_CHI_NAME.Text = query_detail.FirstOrDefault().OVC_CHI_NAME;
                    lbOVC_ITEM_COUNT.Text = query_detail.Count().ToString();
                    lbONB_ITEM_VALUE.Text = moneycount;
                    lbONB_CARRIAGE_CURRENCY.Text = moneycurrency;
                    lbONB_INS_AMOUNT.Text = moneycount;
                    lbONB_CARRIAGE_CURRENCY2.Text = moneycurrency;
                    txtODT_INS_DATE.Text = query.n.ODT_INS_DATE.ToString();
                    if (query.n.OVC_INS_CONDITION.Contains("全險"))
                        chkOVC_INS_CONDITION.Items[0].Selected = true;
                    if (query.n.OVC_INS_CONDITION.Contains("在台內陸險"))
                        chkOVC_INS_CONDITION.Items[1].Selected = true;
                    if (query.n.OVC_INS_CONDITION.Contains("在外內陸險"))
                        chkOVC_INS_CONDITION.Items[2].Selected = true;
                    if (query.n.OVC_INS_CONDITION.Contains("兵險"))
                        chkOVC_INS_CONDITION.Items[3].Selected = true;
                    txtONB_INS_RATE.Text = query.n.ONB_INS_RATE.ToString();
                    lbOVC_START_PORT.Text = ports.FirstOrDefault().OVC_START_PORT;
                    lbOVC_ARRIVE_PORT.Text = ports.FirstOrDefault().OVC_ARRIVE_PORT;
                    drpOVC_MILITARY_TYPE.SelectedValue = query.n.OVC_MILITARY_TYPE;
                    //保費計算
                    lbOVC_FINAL_INS_AMOUNT.Text = query.n.OVC_FINAL_INS_AMOUNT.ToString();
                    if (lbOVC_FINAL_INS_AMOUNT.Text == "" || lbOVC_FINAL_INS_AMOUNT.Text == "0")
                    {
                        decimal rate = getRate(query_detail.FirstOrDefault().OVC_CURRENCY);
                        decimal total = 0;
                        if (rate != -1)
                        {
                            lbOVC_FINAL_INS_AMOUNT.Text = moneycount + "*" + txtONB_INS_RATE.Text + "% *" + rate + "=";
                            total = Convert.ToDecimal(moneycount) * Convert.ToDecimal(txtONB_INS_RATE.Text) * rate;
                            if (total < 1)
                            {
                                lbOVC_FINAL_INS_AMOUNT.Text += "1 (保費不足一元，以一元計價)";
                                ViewState["OVC_FINAL_INS_AMOUNT"] = 1;
                            }
                            else
                            {
                                lbOVC_FINAL_INS_AMOUNT.Text += total.ToString();
                                ViewState["OVC_FINAL_INS_AMOUNT"] = Math.Round(total);
                            }
                        }
                        else
                        {
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "找不到當天匯率，請先至「F15幣別幣值維護」加入" + dateNow.ToShortDateString() + "之匯率");
                        }
                    }
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "找不到該筆投保通知書");
                #endregion
            }
        }

        #region ~Click
        protected void btnDel_Click(object sender, EventArgs e)
        {

            //    try
            //    {
            //        string einn_sn = txtOVC_EINN_NO.ToString();
            //        einn_SN = new Guid(einn_sn);
            //        TBGMT_EINN einnModel = new TBGMT_EINN { EINN_SN = einn_SN };
            //        MTSE.Entry(einnModel).State = EntityState.Deleted;
            //        MTSE.SaveChanges();
            //        FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除投保通知書" + einn_SN + "成功");
            //    }
            //            catch (Exception ex)
            //    {
            //        throw ex;
            //    }
        }
        #endregion

        protected void chkOVC_INS_CONDITION_SelectedIndexChanged(object sender, EventArgs e)
        {
            var query = MTSE.TBGMT_INSRATE;
            decimal rate = 0;
            if (chkOVC_INS_CONDITION.Items[0].Selected)
                rate += query.Where(r => r.OVC_INS_NAME.Equals("全險")).FirstOrDefault().OVC_INS_RATE;
            if (chkOVC_INS_CONDITION.Items[1].Selected)
                rate += query.Where(r => r.OVC_INS_NAME.Equals("在台內陸險")).FirstOrDefault().OVC_INS_RATE;
            if (chkOVC_INS_CONDITION.Items[2].Selected)
                rate += query.Where(r => r.OVC_INS_NAME.Equals("在外內陸險")).FirstOrDefault().OVC_INS_RATE;
            if (chkOVC_INS_CONDITION.Items[3].Selected)
                rate += query.Where(r => r.OVC_INS_NAME.Equals("兵險")).FirstOrDefault().OVC_INS_RATE;
            txtONB_INS_RATE.Text = rate.ToString();
        }
    }
}