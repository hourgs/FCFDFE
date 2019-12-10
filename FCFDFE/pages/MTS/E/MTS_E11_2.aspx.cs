using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MTS.E
{
    public partial class MTS_E11_2 : System.Web.UI.Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        private MTSEntities MTS = new MTSEntities();
        Common FCommon = new Common();
        TBGMT_BLD bld = new TBGMT_BLD();
        TBGMT_ICS ics = new TBGMT_ICS();
        string id;
        string[] filed;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                FCommon.getQueryString(this, "id",out id, true);
                FCommon.Controls_Attributes("readonly", "true", lblOvcBldNo, txtOvcShipCompany, txtOvcSeaOrAir, txtOvcShipName, txtOvcVoyage, txtOdtStartDate, txtOvcStartPort, txtOdtPlnArriveDate, txtOdtActArriveDate, txtOvcArrivePort, txtOvcMilitaryType, txtOnbQuanity, txtOvcQuanityUnit, txtOnbVolume, txtOvcVolumeUnit, txtOnbWeight, txtOvcWeightUnit, txtOnbCarriage, txtOnbCarriageCurrency, txtOnbRealWeight, txtOnbRealVolume, txtOnbRealQuanity, TextFinalDate, txtTotalNT, txtShipDiscount);
                if (Session["MTS_E11"] != null)
                     filed = Session["MTS_E11"].ToString().Split(',');
                if (Session["MTS_E11_PAGE"] != null)
                {
                    lblnow.Text = (Convert.ToInt16(Session["MTS_E11_PAGE"]) + 1).ToString();
                    lblall.Text = filed.Length.ToString();
                }
                if (!IsPostBack)
                {
                    DataTable dtITEM = CommonStatic.ListToDataTable(MTS.TBGMT_TRANSFER_SEA_PRICE.Where(table => table.OVC_IS_ABROAD.Equals("美加西岸")).ToList());
                    FCommon.list_dataImport(DropOvcChiName, dtITEM, "OVC_CHI_NAME", "ONB_WEIGHT_CARRIAGE", true);
                    FCommon.list_dataImport(DropOvcChiName2, dtITEM, "OVC_CHI_NAME", "ONB_WEIGHT_CARRIAGE", true);
                    FCommon.list_dataImport(DropOvcChiName3, dtITEM, "OVC_CHI_NAME", "ONB_WEIGHT_CARRIAGE", true);
                    FCommon.list_dataImport(DropOvcChiName4, dtITEM, "OVC_CHI_NAME", "ONB_WEIGHT_CARRIAGE", true);
                    DataTable dtFINAL_PORT = CommonStatic.ListToDataTable(MTS.TBGMT_PORTS.Where(table => table.OVC_PORT_TYPE.Equals("海港")).ToList());
                    FCommon.list_dataImport(DropOvcPortChiName, dtFINAL_PORT, "OVC_PORT_CHI_NAME", "OVC_PORT_CDE", true);
                    DataTable dtflyITEM = CommonStatic.ListToDataTable(MTS.TBGMT_CURRENCY.ToList());
                    FCommon.list_dataImport(DropOvcCurrencyCode, dtflyITEM, "OVC_CURRENCY_NAME", "OVC_CURRENCY_CODE", true);
                    if (id != "")
                        dataImport(id);
                }
            }



        }

        public void dataImport(string bld_no)
        {
            DataTable dt2 = new DataTable();
            lblOvcBldNo.Text = bld_no.ToString();

            var query2 =
                from bld in MTS.TBGMT_BLD
                join edf in MTS.TBGMT_EDF on bld.OVC_BLD_NO equals edf.OVC_BLD_NO into p1
                from edf in p1.DefaultIfEmpty()
                join dept_calss in MTS.TBGMT_DEPT_CLASS on bld.OVC_MILITARY_TYPE equals dept_calss.OVC_CLASS into p2
                from dept_calss in p2.DefaultIfEmpty()
                join ports in MTS.TBGMT_PORTS.Distinct() on edf.OVC_ARRIVE_PORT equals ports.OVC_PORT_CDE into p3
                from ports in p3.DefaultIfEmpty()
                join ports2 in MTS.TBGMT_PORTS.Distinct() on edf.OVC_START_PORT equals ports2.OVC_PORT_CDE into p4
                from ports2 in p4.DefaultIfEmpty()
                select new
                {
                    bld.OVC_SHIP_COMPANY,
                    bld.OVC_SEA_OR_AIR,
                    bld.OVC_SHIP_NAME,//船機名稱
                    bld.OVC_VOYAGE,//航次
                    bld.ODT_START_DATE,//日期
                    ports2.OVC_PORT_CHI_NAME,//啟運港
                    bld.ODT_PLN_ARRIVE_DATE,
                    bld.ODT_ACT_ARRIVE_DATE,
                    text = ports.OVC_PORT_CHI_NAME,//底運
                    dept_calss.OVC_CLASS_NAME,//軍種
                    bld.ONB_QUANITY,
                    bld.ONB_OLD_QUANITY,//實際體積找不到
                    bld.OVC_QUANITY_UNIT,
                    bld.ONB_VOLUME,
                    bld.REAL_VOLUME,
                    bld.OVC_VOLUME_UNIT,
                    bld.ONB_WEIGHT,
                    bld.REAL_WEIGHT,
                    bld.OVC_WEIGHT_UNIT,
                    bld.ONB_CARRIAGE,
                    bld.ONB_CARRIAGE_CURRENCY,
                    OVC_BLD_NO = bld.OVC_BLD_NO,
                    bld.ODT_LAST_START_DATE,
                    OVC_CONTRACT_AIRLINE=bld.OVC_CONTRACT_AIRLINE,
                    OVC_PASS_TPEKHH=bld.OVC_PASS_TPEKHH,
                };
            query2 = query2.Where(table => table.OVC_BLD_NO.Equals(bld_no));
            dt2 = CommonStatic.LinqQueryToDataTable(query2);

            txtOvcShipCompany.Text = dt2.Rows[0][0].ToString();
            txtOvcSeaOrAir.Text = dt2.Rows[0][1].ToString();
            txtOvcShipName.Text = dt2.Rows[0][2].ToString();
            txtOvcVoyage.Text = dt2.Rows[0][3].ToString();
            if (dt2.Rows[0][4].ToString() != string.Empty)
            {
                txtOdtStartDate.Text = Convert.ToDateTime(dt2.Rows[0][4]).ToString("yyyy-MM-dd");
            }

         
            txtOvcStartPort.Text = dt2.Rows[0][5].ToString();
            if (dt2.Rows[0][6].ToString() != string.Empty)
                txtOdtPlnArriveDate.Text = Convert.ToDateTime(dt2.Rows[0][6]).ToString("yyyy-MM-dd");
            if (dt2.Rows[0][7].ToString() != string.Empty)
                txtOdtActArriveDate.Text = Convert.ToDateTime(dt2.Rows[0][7]).ToString("yyyy-MM-dd");
            txtOvcArrivePort.Text = dt2.Rows[0][8].ToString();
            DropOvcPortChiName.SelectedItem.Equals( dt2.Rows[0][8].ToString());
            txtOvcMilitaryType.Text = dt2.Rows[0][9].ToString();
            txtOnbQuanity.Text = dt2.Rows[0][10].ToString();
            txtOnbRealQuanity.Text = dt2.Rows[0][11].ToString();
            txtOvcQuanityUnit.Text = dt2.Rows[0][12].ToString();
            txtOnbVolume.Text = dt2.Rows[0][13].ToString();
            txtOnbRealVolume.Text = dt2.Rows[0][14].ToString();
            txtOvcVolumeUnit.Text = dt2.Rows[0][15].ToString();
            txtOnbWeight.Text = dt2.Rows[0][16].ToString();
            txtOnbRealWeight.Text = dt2.Rows[0][17].ToString();
            txtOvcWeightUnit.Text = dt2.Rows[0][18].ToString();
            txtOnbCarriage.Text = dt2.Rows[0][19].ToString();
            txtOnbCarriageCurrency.Text = dt2.Rows[0][20].ToString();
            if (txtOvcSeaOrAir.Text == "海運" && dt2.Rows[0][22].ToString() != string.Empty)
                TextFinalDate.Text = Convert.ToDateTime(dt2.Rows[0][22]).ToString("yyyy-MM-dd");
            lblnow.Text = (Convert.ToInt16(Session["MTS_E11_PAGE"]) + 1).ToString();
            DropOvcShipComapany.SelectedIndex = dt2.Rows[0][23].ToString() == "合約航線" ? 1 : 0;
            DropPassedBy.SelectedIndex = dt2.Rows[0][24].ToString() == "是" ? 0 : 1;
            Viseble_button();

            #region 危險品處理費
            var query_DangerPorCost =
                (from air in MTS.TBGMT_AIR_TRANSPORT
                 join com in MTS.TBGMT_COMPANY on air.CO_SN equals com.CO_SN
                 join port in MTS.TBGMT_PORTS on air.OVC_START_PORT equals port.OVC_PORT_CDE
                 where port.OVC_PORT_CHI_NAME.Equals(txtOvcStartPort.Text)
                 where com.OVC_COMPANY.Equals(txtOvcShipCompany.Text)
                 select air).FirstOrDefault();
            if (query_DangerPorCost != null)
            {
                FCommon.list_setValue(DropOvcCurrencyCode, query_DangerPorCost.ONB_CARRIAGE_CURRENCY);
                var query_port = MTS.TBGMT_PORTS.Where(t => t.OVC_PORT_CHI_NAME.Equals(txtOvcArrivePort.Text)).FirstOrDefault();
                if (query_port != null)
                {
                    try
                    {
                        DangerousFee.Text = query_port.OVC_IS_ABROAD.Contains("歐洲") ? query_DangerPorCost.ONB_EUR_FIN_DANGER_PRO_COST_1.ToString() : 
                            (query_port.OVC_IS_ABROAD.Contains("美加") ? query_DangerPorCost.ONB_USA_FIN_DANGER_PRO_COST_1.ToString() : "");
                        txtFlyDiscount.Text = query_DangerPorCost.ONB_DISCOUNT_1 != null ? query_DangerPorCost.ONB_DISCOUNT_1.ToString() : "";
                    }
                    catch
                    {

                    }
                }
            }
            #endregion
        }
        protected string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_BLD_NO", Request.QueryString["OVC_BLD_NO"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_TRANSER_DEPT_CDE", Request.QueryString["OVC_TRANSER_DEPT_CDE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_IS_CHARGE", Request.QueryString["OVC_IS_CHARGE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_SHIP_NAME", Request.QueryString["OVC_SHIP_NAME"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_VOYAGE", Request.QueryString["OVC_VOYAGE"], false);
            FCommon.setQueryString(ref strQueryString, "OVC_BLD", Request.QueryString["OVC_BLD"], false);
            return strQueryString;

        }
        public void Viseble_button()
        {
            if ((Convert.ToInt16(Session["MTS_E11_PAGE"]) + 1).ToString() == lblall.Text)
                btnnext.Visible = false;
            else
                btnnext.Visible = true;

            if ((Convert.ToInt16(Session["MTS_E11_PAGE"]) + 1).ToString() == "1")
                btnlast.Visible = false;
            else
                btnlast.Visible = true;
            if (txtOvcSeaOrAir.Text == "海運")
            {
                Sea_Panel.Style["display"] = "block";
            }
            if (txtOvcSeaOrAir.Text == "空運")
            {
                Port_Panel.Style["display"] = "block";
            }
                
        }
        public void btnlast_onclick(object sender, EventArgs e)
        {
            string[] filed = Session["MTS_E11"].ToString().Split(',');
            int strnum = (Convert.ToInt16(Session["MTS_E11_PAGE"]) - 1);
            Session["MTS_E11_PAGE"] = strnum;
            string bld_no = filed[strnum].ToString();
            dataImport(bld_no);
        }
        public void btnnext_onclick(object sender, EventArgs e)
        {
            string[] filed = Session["MTS_E11"].ToString().Split(',');
            int strnum = (Convert.ToInt16(Session["MTS_E11_PAGE"]) + 1);
            Session["MTS_E11_PAGE"] = strnum;
            string bld_no = filed[strnum].ToString();
            dataImport(bld_no);
        }

        public void SeaDropList()
        {
            DataTable dt = new DataTable();
            var query =
                from sea_price in MTS.TBGMT_TRANSFER_SEA_PRICE
                select new
                {
                    sea_price.OVC_CHI_NAME,
                    OVC_IS_ABROAD = sea_price.OVC_IS_ABROAD,
                };
            query = query.Where(table => table.OVC_IS_ABROAD.Equals("美加西岸"));
            dt = CommonStatic.LinqQueryToDataTable(query.Distinct());

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                DropOvcChiName.Items.Add(dt.Rows[i][0].ToString());
                DropOvcChiName2.Items.Add(dt.Rows[i][0].ToString());
                DropOvcChiName3.Items.Add(dt.Rows[i][0].ToString());
                DropOvcChiName4.Items.Add(dt.Rows[i][0].ToString());
            }
            DataTable dt2 = new DataTable();

            var query2 =
                from ports in MTS.TBGMT_PORTS
                select new
                {
                    ports.OVC_PORT_CHI_NAME,
                    OVC_PORT_TYPE = ports.OVC_PORT_TYPE,
                };
            query2 = query2.Where(table => table.OVC_PORT_TYPE.Equals("海港"));
            dt2 = CommonStatic.LinqQueryToDataTable(query2.Distinct());

            for (var i = 0; i < dt2.Rows.Count; i++)
            {
                DropOvcPortChiName.Items.Add(dt2.Rows[i][0].ToString());
            }
        }


        public void btnFreight_onclick(object sender, EventArgs e)
        {
            string success = "";
            string error = "";
            string code = DropOvcCurrencyCode.SelectedItem.Value;
            DateTime getDate = Convert.ToDateTime(txtOdtActArriveDate.Text);
            DateTime getLastMonthDate = getDate.AddMonths(-1);
            DataTable dt = new DataTable();
            var query =
                from air_price in MTS.TBGMT_TRANSFER_AIR_PRICE
                select new
                {
                    air_price.ONB_WEIGHT,
                    air_price.ONB_CARRIAGE,
                    air_price.ONB_CARRIAGE_CURRENCY,
                    OVC_START_PORT = air_price.OVC_START_PORT,
                    OVC_ARRIVE_PORT = air_price.OVC_ARRIVE_PORT,
                    OVC_PORT_TYPE = air_price.OVC_PORT_TYPE,
                };
            query = query.Where(table => table.OVC_PORT_TYPE.Equals("進口"));
            query = query.Where(table => table.OVC_START_PORT.Equals("AMS"));
            query = query.Where(table => table.OVC_ARRIVE_PORT.Equals("KHH"));
            dt = CommonStatic.LinqQueryToDataTable(query.Distinct());

            DataTable dt2 = new DataTable();
            var query2 =
                from currency_rate in MTS.TBGMT_CURRENCY_RATE
                select new
                {
                    OVC_CURRENCY_CODE=currency_rate.OVC_CURRENCY_CODE,
                    currency_rate.ONB_RATE,
                    ODT_DATE = currency_rate.ODT_DATE,
                };
            query2 = query2.Where(table => table.OVC_CURRENCY_CODE == "EUR");
            query2 = query2.Where(table => table.ODT_DATE >= getLastMonthDate && table.ODT_DATE <= getDate);
            dt2 = CommonStatic.LinqQueryToDataTable(query2);

            double curreny_Min_Charge = 0;
            if (dt2.Rows.Count > 0)
                curreny_Min_Charge = Convert.ToDouble(dt2.Rows[dt2.Rows.Count - 1][1]) * 68.07;
            else
                error += "找不到一個月內歐元之匯率，故無法計算最低運費 <br>";
            DataTable dt3 = new DataTable();
            var query3 =
                from currency_rate in MTS.TBGMT_CURRENCY_RATE
                select new
                {
                    OVC_CURRENCY_CODE = currency_rate.OVC_CURRENCY_CODE,
                    currency_rate.ONB_RATE,
                    ODT_DATE = currency_rate.ODT_DATE,
                };
            query3 = query3.Where(table => table.OVC_CURRENCY_CODE == code);
            query3 = query3.Where(table => table.ODT_DATE >= getLastMonthDate && table.ODT_DATE <= getDate);
            dt3 = CommonStatic.LinqQueryToDataTable(query3);


            DataTable dt4 = new DataTable();
            var query4 =
                from tbgmt_carriage_discont in MTS.TBGMT_CARRIAGE_DISCOUNT
                select new
                {
                    tbgmt_carriage_discont.OVC_CARR_RATE,
                    OVC_CARR_NAME = tbgmt_carriage_discont.OVC_CARR_NAME,
                };
            query4 = query4.Where(table => table.OVC_CARR_NAME == "空運");
            dt4 = CommonStatic.LinqQueryToDataTable(query4);

            if (dt4.Rows.Count>0)
                txtFlyDiscount.Text = dt4.Rows[0][0].ToString();

            double KGweight = 0;
            decimal decOnbRealVolume;
            bool boolOnbRealVolume = FCommon.checkDecimal(txtOnbRealVolume.Text, "體積/實際體積", ref error, out decOnbRealVolume);

            if (txtOvcVolumeUnit.Text == "CF" && boolOnbRealVolume)
                KGweight = Convert.ToDouble(txtOnbRealVolume.Text) * 4.72;
            if (txtOvcVolumeUnit.Text == "CBM" && boolOnbRealVolume)
                KGweight = Convert.ToDouble(txtOnbRealVolume.Text) * 4.72 * 35.3142;
            if (txtOvcVolumeUnit.Text == "KG" && boolOnbRealVolume)
                KGweight = Convert.ToDouble(txtOnbRealVolume.Text);
            
            int level = 0;
            int temp = 0;
            for (var i =0; i< dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    if (KGweight <= Convert.ToInt64(dt.Rows[i][0]))
                    {
                        level = i;
                    }
                }
                else
                {
                    if(KGweight <= Convert.ToInt64(dt.Rows[i][0]) && KGweight >= Convert.ToInt64(dt.Rows[i - 1][0]))
                    {
                        level = i;
                    }
                }
            }
            KGweight = KGweight * Convert.ToDouble(dt.Rows[level][0]);
            KGweight = Math.Round(KGweight, 0);
            #region alert msg
            if (int.TryParse(DangerousFee.Text, out temp) && dt2.Rows.Count>0 && dt3.Rows.Count>0)
            {
                totalNT.Text = (Convert.ToInt32(DangerousFee.Text) * Convert.ToInt32(dt3.Rows[dt3.Rows.Count - 1][1]) + KGweight).ToString();
                success = "計算成功，規則為：單價(" + dt.Rows[level][1] + ")重量等級(" + dt.Rows[level][0] + "，取" + KGweight.ToString() + "KG)、匯率(" + dt2.Rows[dt2.Rows.Count - 1][1].ToString() + ")、幣別(" + dt.Rows[level][2] + ")<br>";
                if (KGweight >= 2729.607)
                {
                    success += "已超過最低運費：(台幣)"+ curreny_Min_Charge.ToString() + "，"  + dt.Rows[level][2] + dt.Rows[level][1] + "<br>";
                }
                success += "危險品運費：(台幣)" + (Convert.ToInt32(DangerousFee.Text) * Convert.ToInt32(dt2.Rows[dt2.Rows.Count - 1][1])).ToString() + "元";
            }

            if (dt2.Rows.Count == 0 )
                error += "找不到一個月內之匯率";
            if (dt3.Rows.Count == 0 && DropOvcCurrencyCode.SelectedItem.Text != "請選擇" && int.TryParse(DangerousFee.Text, out temp))
                error += "找不到一個月內"+ DropOvcCurrencyCode.SelectedItem.Text + "之匯率";

            if (DropOvcCurrencyCode.SelectedItem.Text == "請選擇" && DangerousFee.Text=="" && dt2.Rows.Count>0)
            {
                totalNT.Text =  KGweight.ToString();
                success = "計算成功，規則為：單價(" + dt.Rows[level][1] + ")重量等級(" + dt.Rows[level][0] + "，取" + KGweight.ToString() + "KG)、匯率(" + dt2.Rows[dt2.Rows.Count - 1][1].ToString() + ")、幣別(" + dt.Rows[level][2] + ")<br>";
                if (KGweight >= 2729.607)
                {
                    success += "已超過最低運費：(台幣)" + curreny_Min_Charge.ToString() + "，" + dt.Rows[level][2] + dt.Rows[level][1] + "<br>";
                }
            }
            if(!int.TryParse(DangerousFee.Text, out temp) && DangerousFee.Text!="")
            {
                error += "危險品處理費格式不正確";
            }
            if (DropOvcCurrencyCode.SelectedItem.Text == "請選擇" && int.TryParse(DangerousFee.Text, out temp) && dt2.Rows.Count > 0)
            {
                totalNT.Text = KGweight.ToString();
                success = "計算成功，規則為：單價(" + dt.Rows[level][1] + ")重量等級(" + dt.Rows[level][0] + "，取" + KGweight.ToString() + "KG)、匯率(" + dt2.Rows[dt2.Rows.Count - 1][1].ToString() + ")、幣別(" + dt.Rows[level][2] + ")<br>";
                if (KGweight >= 2729.607)
                {
                    success += "已超過最低運費：(台幣)" + curreny_Min_Charge.ToString() + "，" + dt.Rows[level][2] + dt.Rows[level][1] + "<br>";
                }
            }
            if (error == "" && success == "")
            {
                FCommon.AlertShow(flymassage, "danger", "系統訊息", "請輸入危險品處理費!!");
            }
            else
            {
                if (error == "")
                    FCommon.AlertShow(flymassage, "success", "系統訊息", success.ToString());
                else
                    FCommon.AlertShow(flymassage, "danger", "系統訊息", error.ToString());
            }

            #endregion

        }
        public void btnShipping_onclick(object sender, EventArgs e)
        {
            string altertxt = "";
            string success = "";
            DataTable dt = new DataTable();
            DateTime getDate = DateTime.Now.AddMonths(-1);
            DateTime nowDate = DateTime.Now;
            if (TextFinalDate.Text != "")
            {
                getDate = Convert.ToDateTime(TextFinalDate.Text);
                nowDate = Convert.ToDateTime(TextFinalDate.Text).AddDays(1);
                getDate = getDate.AddMonths(-1);
            }

            var query =
                from CURRENCY_RATE in MTS.TBGMT_CURRENCY_RATE
                select new
                {
                    OVC_CURRENCY_CODE=CURRENCY_RATE.OVC_CURRENCY_CODE,
                    CURRENCY_RATE.ONB_RATE,
                    ODT_DATE=CURRENCY_RATE.ODT_DATE,
                };
            query = query.Where(table => table.OVC_CURRENCY_CODE.Equals(txtOnbCarriageCurrency.Text));
            query = query.Where(table => table.ODT_DATE <= nowDate && table.ODT_DATE >= getDate);
            dt = CommonStatic.LinqQueryToDataTable(query);

            DataTable dt2 = new DataTable();
            var query2 =
                from discount in MTS.TBGMT_CARRIAGE_DISCOUNT
                where discount.OVC_CARR_NAME.Equals(txtOvcSeaOrAir.Text.ToString())
                select new
                {
                    discount.OVC_CARR_RATE,
                    OVC_CARR_NAME = discount.OVC_CARR_NAME,
                };
            query2 = query2.Where(table => table.OVC_CARR_NAME == "海運");
            dt2 = CommonStatic.LinqQueryToDataTable(query2);
            if (dt2.Rows.Count > 0)
            {
                txtShipDiscount.Text = dt2.Rows[0][0].ToString();
            }

            long allM = 0;

            if (dt.Rows.Count == 0)
            {
                altertxt += "第一細項計算失敗，原因為：找不到一個月內之匯率<br>";
                altertxt += "第二細項計算失敗，原因為：找不到一個月內之匯率<br>";
                altertxt += "第三細項計算失敗，原因為：找不到一個月內之匯率<br>";
                altertxt += "第四細項計算失敗，原因為：找不到一個月內之匯率<br>";
                altertxt += "→佔艙費計算失敗，原因為：找不到一個月內之匯率<br>";
            }
            int textnum = 0;
            if (altertxt == "")
            {
                if (Convert.ToInt16(dt.Rows[dt.Rows.Count - 1][1]) < 0)
                {
                    dt = null;
                    textnum = textnum + 1;
                }
            }
            #region alert msg
            if (DropOvcChiName.SelectedItem.Text != "請選擇" && dt.Rows.Count > 0 && int.TryParse(Volume1.Text,out textnum) && int.TryParse(Weight1.Text, out textnum))
            {
                var total = (Convert.ToInt64(DropOvcChiName.SelectedItem.Value) * Convert.ToInt64(Volume1.Text) * Convert.ToInt64(Weight1.Text));
                Freight1.Text = total * Convert.ToInt64(dt.Rows[dt.Rows.Count - 1][1]) + "元  (" + total.ToString() + "美元)";
                success += "第一細項計算成功，規則為：(美加西岸)、單價(" + DropOvcChiName.SelectedItem.Value + ")、匯率(" + dt.Rows[dt.Rows.Count - 1][1].ToString() + ")、取體積(" + Volume1.Text + ")<br>";
                allM += total;
            }
            else if(DropOvcChiName.SelectedItem.Text != "請選擇" && dt.Rows.Count == 0 && int.TryParse(Volume1.Text, out textnum) && int.TryParse(Weight1.Text, out textnum))
            {
                var total = (Convert.ToInt64(DropOvcChiName.SelectedItem.Value) * Convert.ToInt64(Volume1.Text) * Convert.ToInt64(Weight1.Text));
                Freight1.Text =  "元  (" + total.ToString() + "美元)";
                allM += total;
            }

                

            if (DropOvcChiName2.SelectedItem.Text != "請選擇" && dt.Rows.Count > 0 && int.TryParse(Volume2.Text, out textnum) && int.TryParse(Weight2.Text, out textnum))
            {
                var total = (Convert.ToInt64(DropOvcChiName2.SelectedItem.Value) * Convert.ToInt64(Volume2.Text) * Convert.ToInt64(Weight2.Text));
                Freight2.Text = total * Convert.ToInt64(dt.Rows[dt.Rows.Count - 1][1]) + "元  (" + total.ToString() + "美元)";
                success += "第二細項計算成功，規則為：(美加西岸)、單價(" + DropOvcChiName2.SelectedItem.Value + ")、匯率(" + dt.Rows[dt.Rows.Count - 1][1].ToString() + ")、取體積(" + Volume2.Text + ")<br>";
                allM += total;
            }
            else if (DropOvcChiName2.SelectedItem.Text != "請選擇" && dt.Rows.Count == 0 && int.TryParse(Volume2.Text, out textnum) && int.TryParse(Weight2.Text, out textnum))
            {
                var total = (Convert.ToInt64(DropOvcChiName2.SelectedItem.Value) * Convert.ToInt64(Volume2.Text) * Convert.ToInt64(Weight2.Text));
                Freight2.Text = "元  (" + total.ToString() + "美元)";
                allM += total;
            }


            if (DropOvcChiName3.SelectedItem.Text != "請選擇" && dt.Rows.Count > 0 && int.TryParse(Volume3.Text, out textnum) && int.TryParse(Weight3.Text, out textnum))
            {
                var total = (Convert.ToInt64(DropOvcChiName3.SelectedItem.Value) * Convert.ToInt64(Volume3.Text) * Convert.ToInt64(Weight3.Text));
                Freight3.Text = total * Convert.ToInt64(dt.Rows[dt.Rows.Count - 1][1]) + "元  (" + total.ToString() + "美元)";
                success += "第三細項計算成功，規則為：(美加西岸)、單價(" + DropOvcChiName3.SelectedItem.Value + ")、匯率(" + dt.Rows[dt.Rows.Count - 1][1].ToString() + ")、取體積(" + Volume3.Text + ")<br>";
                allM += total;
            }
            else if (DropOvcChiName3.SelectedItem.Text != "請選擇" && dt.Rows.Count == 0 && int.TryParse(Volume3.Text, out textnum) && int.TryParse(Weight3.Text, out textnum))
            {
                var total = (Convert.ToInt64(DropOvcChiName3.SelectedItem.Value) * Convert.ToInt64(Volume3.Text) * Convert.ToInt64(Weight3.Text));
                Freight3.Text =  "元  (" + total.ToString() + "美元)";
                allM += total;
            }


            if (DropOvcChiName4.SelectedItem.Text != "請選擇" && dt.Rows.Count > 0 && int.TryParse(Volume4.Text, out textnum) && int.TryParse(Weight4.Text, out textnum))
            {
                var total = (Convert.ToInt64(DropOvcChiName4.SelectedItem.Value) * Convert.ToInt64(Volume4.Text) * Convert.ToInt64(Weight4.Text));
                Freight4.Text = total * Convert.ToInt64(dt.Rows[dt.Rows.Count - 1][1]) + "元  (" + total.ToString() + "美元)";
                success += "第四細項計算成功，規則為：(美加西岸)、單價(" + DropOvcChiName4.SelectedItem.Value + ")、匯率(" + dt.Rows[dt.Rows.Count - 1][1].ToString() + ")、取體積(" + Volume4.Text + ")<br>";
                allM += total;
            }
            else if(DropOvcChiName4.SelectedItem.Text != "請選擇" && dt.Rows.Count == 0 && int.TryParse(Volume4.Text, out textnum) && int.TryParse(Weight4.Text, out textnum))
            {
                var total = (Convert.ToInt64(DropOvcChiName4.SelectedItem.Value) * Convert.ToInt64(Volume4.Text) * Convert.ToInt64(Weight4.Text));
                Freight4.Text =  "元  (" + total.ToString() + "美元)";
                allM += total;
            }


            if(int.TryParse(textONB_ON_SHIP_QUANTITY.Text, out textnum) && int.TryParse(textONB_ON_SHIP_COST.Text, out textnum) && dt.Rows.Count > 0)
            {
                var total = (Convert.ToInt64(textONB_ON_SHIP_QUANTITY.Text) * Convert.ToInt64(textONB_ON_SHIP_COST.Text));
                success += "→佔艙費計算成功，規則為：匯率(" + dt.Rows[dt.Rows.Count - 1][1].ToString() + ")" + "<br>";
                txtONB_ON_SHIP_TOTAL.Text = total * Convert.ToInt64(dt.Rows[dt.Rows.Count - 1][1]) + "元  (" + total.ToString() + "美元)";
            }
            else if(int.TryParse(textONB_ON_SHIP_QUANTITY.Text, out textnum) && int.TryParse(textONB_ON_SHIP_COST.Text, out textnum) && dt.Rows.Count == 0)
            {
                var total = (Convert.ToInt64(textONB_ON_SHIP_QUANTITY.Text) * Convert.ToInt64(textONB_ON_SHIP_COST.Text));
                txtONB_ON_SHIP_TOTAL.Text =  "元  (" + total.ToString() + "美元)";
            }
            else
            {
                altertxt += "→佔艙費計算失敗，原因為：格式不正確<br>";
            }
            txtTotalNT.Text = allM.ToString();


            if(altertxt !="")
                FCommon.AlertShow(massage, "danger", "系統訊息", altertxt.ToString());
            else
                FCommon.AlertShow(massage, "success", "系統訊息", success.ToString());
            #endregion
        }

        public void btnNoneed_onclick(object sender, EventArgs e)
        {
            try
            {
                bld = MTS.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(lblOvcBldNo.Text)).FirstOrDefault();
                bld.OVC_IS_CHARGE = "否";
                MTS.SaveChanges();
                FCommon.AlertShow(msg, "success", "系統訊息", "無須計算運費成功!");
                FCommon.syslog_add(Session["userid"].ToString(),Request.ServerVariables["REMOTE_ADDR"].ToString(), bld.GetType().Name.ToString(), this, "修改");

            }
            catch
            {
                FCommon.AlertShow(msg, "danger", "系統訊息", "無須計算運費失敗!");
            }

        }
        public void btnBring_onclick(object sender, EventArgs e)
        {
            var SeaOrPort = txtOvcSeaOrAir.Text;
            var icrYear = Convert.ToDateTime(txtOdtActArriveDate.Text).AddYears(-1911).ToString("yyy");

            var strOVC_ICS_NO = "";
            var strOVC_BLD_NO = lblOvcBldNo.Text;
            var strOVC_INLAND_CARRIAGE = "";
            if (SeaOrPort == "海運")
                strOVC_INLAND_CARRIAGE = txtTotalNT.Text;
            if (SeaOrPort == "空運")
                strOVC_INLAND_CARRIAGE = totalNT.Text;

            var strOVC_INF_NO = "";
            DateTime strODT_MODIFY_DATE = DateTime.Now;
            Guid GOVC_ICS_SN;
            GOVC_ICS_SN = Guid.NewGuid();
            Guid GOVC_BLD_SN;
            GOVC_BLD_SN = Guid.NewGuid();
            Guid GOVC_INF_SN;
            GOVC_INF_SN = Guid.NewGuid();
            DataTable dt = new DataTable();
            var query =
                from ics in MTS.TBGMT_ICS
                select new
                {
                    OVC_ICS_NO=ics.OVC_ICS_NO,
                };
            query = query.Where(table => table.OVC_ICS_NO.Contains("ICS"+icrYear));
            dt = CommonStatic.LinqQueryToDataTable(query);
            if (dt.Rows.Count > 0)
            {
                string temp = dt.Rows[dt.Rows.Count - 1][0].ToString().Replace("ICS", "");
                strOVC_ICS_NO = "ICS" + (Convert.ToInt64(temp) + 1).ToString();
            }
            else
            {
                strOVC_ICS_NO = "ICS" + icrYear + "0001";
            }


         
            try
            {
                
                ics.OVC_ICS_NO = strOVC_ICS_NO;
                ics.OVC_BLD_NO = strOVC_BLD_NO;
                ics.OVC_INLAND_CARRIAGE = Convert.ToInt32(strOVC_INLAND_CARRIAGE);
                ics.OVC_INF_NO = strOVC_INF_NO;
                ics.ODT_MODIFY_DATE = strODT_MODIFY_DATE;
                ics.OVC_CREATE_LOGIN_ID = Session["userid"].ToString(); 
                ics.OVC_ICS_SN = GOVC_ICS_SN;
                ics.OVC_BLD_SN = GOVC_BLD_SN;
                ics.OVC_INF_SN = GOVC_INF_SN;
                ics.ODT_CREATE_DATE = null;
                ics.OVC_CREATE_ID = null;
                ics.OVC_MODIFY_LOGIN_ID = null;
                MTS.TBGMT_ICS.Add(ics);
                MTS.SaveChanges();
                FCommon.AlertShow(msg, "success", "系統訊息", "帶入運費檔成功!");
                FCommon.syslog_add(Session["userid"].ToString(),Request.ServerVariables["REMOTE_ADDR"].ToString(), ics.GetType().Name.ToString(), this, "新增");
            }
            catch 
            {
                FCommon.AlertShow(msg, "danger", "系統訊息", "帶入運費檔失敗!");
            }
        }
        public void btnSave_onclick(object sender, EventArgs e)
        {
            string Message = "";
            string SeaOrPort = txtOvcSeaOrAir.Text;
            string strOVC_PASS_TPEKHH = DropPassedBy.SelectedItem.Text;
            string strOVC_CONTRACT_AIRLINE = DropOvcShipComapany.SelectedItem.Text;
            string strOVC_LAST_START_PORT = DropOvcPortChiName.SelectedItem.Value;
            string strODT_LAST_START_DATE = TextFinalDate.Text;

            DateTime dateODT_LAST_START_DATE;

            bool boolODT_LAST_START_DATE = FCommon.checkDateTime(strODT_LAST_START_DATE, "最後離境日期/結關前一日", ref Message, out dateODT_LAST_START_DATE);

            try
            {
                if(SeaOrPort== "海運")
                {
                    bld = MTS.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(lblOvcBldNo.Text)).FirstOrDefault();
                    bld.OVC_LAST_START_PORT = strOVC_LAST_START_PORT;
                    if (boolODT_LAST_START_DATE)
                        bld.ODT_LAST_START_DATE = Convert.ToDateTime(strODT_LAST_START_DATE);
                    MTS.SaveChanges();
                    FCommon.AlertShow(msg,"success", "系統訊息", "修改運費檔成功!!");
                    FCommon.syslog_add(Session["userid"].ToString(),Request.ServerVariables["REMOTE_ADDR"].ToString(), bld.GetType().Name.ToString(), this, "修改");
                }
                if (SeaOrPort == "空運")
                {
                    bld = MTS.TBGMT_BLD.Where(table => table.OVC_BLD_NO.Equals(lblOvcBldNo.Text)).FirstOrDefault();
                    bld.OVC_PASS_TPEKHH = strOVC_PASS_TPEKHH;
                    bld.OVC_CONTRACT_AIRLINE = strOVC_CONTRACT_AIRLINE;
                    MTS.SaveChanges();
                    FCommon.AlertShow(msg, "success", "系統訊息", "修改運費檔成功!!");
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), bld.GetType().Name.ToString(), this, "修改");
                }

            }
            catch
            {
                FCommon.AlertShow(msg, "danger", "系統訊息", "修改運費檔失敗!");
            }
        }

        public void btnHone_onclick(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_E11_1.aspx{getQueryString()}");
        }
                   

    }
}