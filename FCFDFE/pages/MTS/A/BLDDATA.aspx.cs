using System;
using System.Linq;
using System.Web.UI;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.MTS.A
{
    public partial class BLDDATA : Page
    {        
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!FCommon.getQueryString(this, "id", out string id, true))
                FCommon.getQueryString(this, "OVC_BLD_NO", out id, false); //若取不到id變數，則用原始OVC_BLD_NO變數
            if (!IsPostBack)
            {
                TBGMT_BLD codetable = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO == id).FirstOrDefault();
                if (codetable != null)
                {
                    lblOVC_BLD_NO.Text = codetable.OVC_BLD_NO;
                    lblOVC_SHIP_COMPANY.Text = codetable.OVC_SHIP_COMPANY;
                    lblOVC_SEA_OR_AIR.Text = codetable.OVC_SEA_OR_AIR;
                    lblOVC_SHIP_NAME.Text = codetable.OVC_SHIP_NAME;
                    lblOVC_VOYAGE.Text = codetable.OVC_VOYAGE;
                    lblODT_START_DATE.Text = FCommon.getDateTime(codetable.ODT_START_DATE);
                    string strOVC_START_PORT = codetable.OVC_START_PORT;
                    TBGMT_PORTS startPort = MTSE.TBGMT_PORTS.Where(o => o.OVC_PORT_CDE.Equals(strOVC_START_PORT)).FirstOrDefault();
                    lblOVC_START_PORT.Text = startPort != null ? startPort.OVC_PORT_CHI_NAME : strOVC_START_PORT;
                    lblODT_PLN_ARRIVE_DATE.Text = FCommon.getDateTime(codetable.ODT_PLN_ARRIVE_DATE);
                    lblODT_ACT_ARRIVE_DATE.Text = FCommon.getDateTime(codetable.ODT_ACT_ARRIVE_DATE);
                    string strOVC_ARRIVE_PORT = codetable.OVC_ARRIVE_PORT;
                    TBGMT_PORTS arrivePort = MTSE.TBGMT_PORTS.Where(o => o.OVC_PORT_CDE.Equals(strOVC_ARRIVE_PORT)).FirstOrDefault();
                    lblOVC_ARRIVE_PORT.Text = arrivePort != null ? arrivePort.OVC_PORT_CHI_NAME : strOVC_ARRIVE_PORT;
                    lblONB_QUANITY.Text = codetable.ONB_QUANITY.ToString();
                    lblOVC_QUANITY_UNIT.Text = codetable.OVC_QUANITY_UNIT;
                    lblONB_VOLUME.Text = codetable.ONB_VOLUME.ToString();
                    lblONB_ON_SHIP_VOLUME.Text = codetable.ONB_ON_SHIP_VOLUME.ToString();
                    lblOVC_VOLUME_UNIT.Text = codetable.OVC_VOLUME_UNIT;
                    lblONB_WEIGHT.Text = codetable.ONB_WEIGHT.ToString();
                    lblOVC_WEIGHT_UNIT.Text = codetable.OVC_WEIGHT_UNIT;
                    lblONB_ITEM_VALUE.Text = codetable.ONB_ITEM_VALUE.ToString();
                    string strONB_CARRIAGE_CURRENCY_I = codetable.ONB_CARRIAGE_CURRENCY_I;
                    TBGMT_CURRENCY q = MTSE.TBGMT_CURRENCY.Where(o => o.OVC_CURRENCY_CODE.Equals(strONB_CARRIAGE_CURRENCY_I)).FirstOrDefault();
                    lblONB_CARRIAGE_CURRENCY_I.Text = q != null ? q.OVC_CURRENCY_NAME : strONB_CARRIAGE_CURRENCY_I;
                    lblONB_CARRIAGE.Text = codetable.ONB_CARRIAGE.ToString();
                    string strONB_CARRIAGE_CURRENCY = codetable.ONB_CARRIAGE_CURRENCY;
                    TBGMT_CURRENCY q2 = MTSE.TBGMT_CURRENCY.Where(o => o.OVC_CURRENCY_CODE.Equals(strONB_CARRIAGE_CURRENCY)).FirstOrDefault();
                    lblONB_CARRIAGE_CURRENCY.Text = q2 != null ? q2.OVC_CURRENCY_NAME : strONB_CARRIAGE_CURRENCY;
                    string strOVC_MILITARY_TYPE = codetable.OVC_MILITARY_TYPE;
                    TBGMT_DEPT_CLASS MILITARY = MTSE.TBGMT_DEPT_CLASS.Where(table => table.OVC_CLASS.Equals(strOVC_MILITARY_TYPE)).FirstOrDefault();
                    lblOVC_MILITARY_TYPE.Text = MILITARY != null ? MILITARY.OVC_CLASS_NAME : strOVC_MILITARY_TYPE;
                    lblODT_CREATE_DATE.Text = FCommon.getDateTime(codetable.ODT_CREATE_DATE);
                    lblODT_MODIFY_DATE.Text = FCommon.getDateTime(codetable.ODT_MODIFY_DATE);
                    lblOVC_CREATE_LOGIN_ID.Text = FCommon.getUserName(codetable.OVC_CREATE_LOGIN_ID);
                }
            }
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            Response.Write("<script language='javascript'>window.close();</" + "script>");
        }
    }
}