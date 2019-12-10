using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.MTS.E
{
    public partial class BLDDATA : System.Web.UI.Page
    {        
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        GMEntities GME = new GMEntities();
        TBGMT_BLD codetable = new TBGMT_BLD();
        TBGMT_PORTS port = new TBGMT_PORTS();
        TBGMT_DEPT_CLASS MILITARY = new TBGMT_DEPT_CLASS();
        ACCOUNT account = new ACCOUNT();

        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["OVC_BLD_NO"];
            string user;
            if (!IsPostBack)
            {
                codetable = MTSE.TBGMT_BLD.Where(table => table.OVC_BLD_NO == id).FirstOrDefault();
                port=MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE == codetable.OVC_START_PORT).FirstOrDefault();
                MILITARY = MTSE.TBGMT_DEPT_CLASS.Where(table => table.OVC_CLASS == codetable.OVC_MILITARY_TYPE).FirstOrDefault();
                account = GME.ACCOUNTs.Where(table => table.USER_ID == codetable.OVC_CREATE_LOGIN_ID).FirstOrDefault();
                if (account != null)
                    user = account.USER_NAME;
                else
                    user = codetable.OVC_CREATE_LOGIN_ID;
                if (codetable != null)
                {
                    lblOVC_BLD_NO.Text = codetable.OVC_BLD_NO;
                    lblOVC_SHIP_COMPANY.Text = codetable.OVC_SHIP_COMPANY;
                    lblOVC_SEA_OR_AIR.Text = codetable.OVC_SEA_OR_AIR;
                    lblOVC_SHIP_NAME.Text = codetable.OVC_SHIP_NAME;
                    lblOVC_VOYAGE.Text = codetable.OVC_VOYAGE;
                    lblODT_START_DATE.Text = Convert.ToDateTime(codetable.ODT_START_DATE).ToString(Variable.strDateFormat);
                    lblOVC_START_PORT.Text = port.OVC_PORT_CHI_NAME;
                    lblODT_PLN_ARRIVE_DATE.Text = Convert.ToDateTime(codetable.ODT_PLN_ARRIVE_DATE).ToString(Variable.strDateFormat);
                    lblODT_ACT_ARRIVE_DATE.Text = Convert.ToDateTime(codetable.ODT_ACT_ARRIVE_DATE).ToString(Variable.strDateFormat);

                    port = MTSE.TBGMT_PORTS.Where(table => table.OVC_PORT_CDE == codetable.OVC_ARRIVE_PORT).FirstOrDefault();

                    lblOVC_ARRIVE_PORT.Text = port.OVC_PORT_CHI_NAME;
                    lblONB_QUANITY.Text = codetable.ONB_QUANITY.ToString();
                    lblOVC_QUANITY_UNIT.Text = codetable.OVC_QUANITY_UNIT;
                    lblONB_VOLUME.Text = codetable.ONB_VOLUME.ToString();
                    lblONB_ON_SHIP_VOLUME.Text = codetable.ONB_ON_SHIP_VOLUME.ToString();
                    lblOVC_VOLUME_UNIT.Text = codetable.OVC_VOLUME_UNIT;
                    lblONB_WEIGHT.Text = codetable.ONB_WEIGHT.ToString();
                    lblOVC_WEIGHT_UNIT.Text = codetable.OVC_WEIGHT_UNIT;
                    lblONB_CARRIAGE.Text = codetable.ONB_CARRIAGE.ToString();
                    lblONB_CARRIAGE_CURRENCY.Text = codetable.ONB_CARRIAGE_CURRENCY;
                    if(codetable.OVC_MILITARY_TYPE != null)
                        lblOVC_MILITARY_TYPE.Text = MILITARY.OVC_CLASS_NAME;
                    lblODT_CREATE_DATE.Text = codetable.ODT_CREATE_DATE.ToString();
                    lblODT_MODIFY_DATE.Text = codetable.ODT_MODIFY_DATE.ToString();
                    lblOVC_CREATE_LOGIN_ID.Text = user;
                }
            }
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            Response.Write("<script language='javascript'>window.close();</" + "script>");
        }
    }
}