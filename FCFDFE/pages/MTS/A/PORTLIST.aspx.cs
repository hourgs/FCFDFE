using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;
using System.Data;

namespace FCFDFE.pages.MTS.A
{
    public partial class PORTLIST : Page
    {
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();

        #region 副程式
        private void dataImport() //是否匯入中文選單、是否匯入英文選單
        {
            var queryTemp = from tPort in MTSE.TBGMT_PORTS select tPort;
            string strQueryCHI = txtQueryCHI.Text.Trim();
            if (!string.IsNullOrEmpty(strQueryCHI))
                queryTemp = queryTemp.Where(table => table.OVC_PORT_CHI_NAME.Contains(strQueryCHI));
            string strQueryENG = txtQueryENG.Text.Trim();
            if (!string.IsNullOrEmpty(strQueryENG))
                queryTemp = queryTemp.Where(table => table.OVC_PORT_ENG_NAME.Contains(strQueryENG));

            if (FCommon.getQueryString(this, "OVC_SEA_OR_AIR", out string port))
            {
                string strPort = "";
                if (port == "空運") strPort = "機場";
                else if (port == "海運") strPort = "海港";

                if (!string.IsNullOrEmpty(strPort))
                    queryTemp = queryTemp.Where(table => table.OVC_PORT_TYPE.Equals(strPort));
            }
            var query =
                queryTemp.Select(cus => new
                {
                    OVC_PORT_CHI_NAME = cus.OVC_PORT_CHI_NAME,
                    OVC_PORT_ENG_NAME = cus.OVC_PORT_ENG_NAME,
                    OVC_PORT_CDE = cus.OVC_PORT_CDE
                }).ToList();

            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            FCommon.list_dataImport(drpQueryCHI, dt, "OVC_PORT_CHI_NAME", "OVC_PORT_CDE", false);
            FCommon.list_dataImport(drpQueryENG, dt, "OVC_PORT_ENG_NAME", "OVC_PORT_CDE", false);
            string strEmpty = "", strText, strValue;
            if (drpQueryCHI.SelectedItem != null)
            {
                strText = drpQueryCHI.SelectedItem.Text;
                strValue = drpQueryCHI.SelectedValue;
            }
            else
            {
                strText = strEmpty;
                strValue = strEmpty;
            }
            drpQueryText.Text = strText;
            drpQueryValue.Text = strValue;
        }
        //private void dataImport() //是否匯入中文選單、是否匯入英文選單
        //{
        //    string port, txtport = "";
        //    var queryTemp = from tPort in MTSE.TBGMT_PORTS select tPort;
        //    if (isCHI)
        //    {
        //        string strQuery = txtQuery.Text.Trim();
        //        if (!string.IsNullOrEmpty(strQuery))
        //            queryTemp = queryTemp.Where(table => table.OVC_PORT_CHI_NAME.Contains(strQuery));
        //    }
        //    else if (isENG)
        //    {
        //        string strQuery = txtQuery2.Text.Trim();
        //        if (!string.IsNullOrEmpty(strQuery))
        //            queryTemp = queryTemp.Where(table => table.OVC_PORT_ENG_NAME.Contains(strQuery));
        //    }

        //    if (FCommon.getQueryString(this, "OVC_SEA_OR_AIR", out port))
        //    {
        //        if (port == "空運") txtport = "機場";
        //        else if (port == "海運") txtport = "海港";

        //        queryTemp = queryTemp.Where(table => table.OVC_PORT_TYPE == txtport);
        //    }
        //    var query =
        //        queryTemp.Select(cus => new
        //        {
        //            OVC_PORT_CHI_NAME = cus.OVC_PORT_CHI_NAME,
        //            OVC_PORT_ENG_NAME = cus.OVC_PORT_ENG_NAME,
        //            OVC_PORT_CDE = cus.OVC_PORT_CDE
        //        }).ToList();

        //    DataTable dt = CommonStatic.LinqQueryToDataTable(query);
        //    if (isCHI) FCommon.list_dataImport(drpQuery, dt, "OVC_PORT_CHI_NAME", "OVC_PORT_CDE", false);
        //    if (isENG) FCommon.list_dataImport(drpQuery2, dt, "OVC_PORT_ENG_NAME", "OVC_PORT_CDE", false);
        //    string strEmpty = "";
        //    if (isCHI && drpQuery.SelectedItem != null)
        //    {

        //        drpQueryText.Text = drpQuery.SelectedItem.Text;
        //        drpQueryValue.Text = drpQuery.SelectedItem.Value;
        //    }
        //    else
        //    {
        //        drpQueryText.Text = strEmpty;
        //        drpQueryValue.Text = strEmpty;
        //    }
        //    if (isENG && drpQuery2.SelectedItem != null)
        //    {

        //        drpQueryText2.Text = drpQuery2.SelectedItem.Text;
        //        drpQueryValue2.Text = drpQuery2.SelectedItem.Value;
        //    }
        //    else
        //    {
        //        drpQueryText2.Text = strEmpty;
        //        drpQueryValue2.Text = strEmpty;
        //    }
        //}
        //private void setSelect()
        //{

        //}
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dataImport();
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dataImport();
        }
        //protected void btnQuery2_Click(object sender, EventArgs e)
        //{
        //    dataImport(false, true);
        //}

        protected void drpQuery_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList theDropDownList = (DropDownList)sender;
            string strValue = theDropDownList.SelectedValue;
            FCommon.list_setValue(drpQueryCHI, strValue);
            FCommon.list_setValue(drpQueryENG, strValue);
            if (drpQueryCHI.SelectedItem != null)
            {
                string strText = drpQueryCHI.SelectedItem.Text;
                strValue = drpQueryCHI.SelectedValue;
                drpQueryText.Text = strText;
                drpQueryValue.Text = strValue;
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "選取錯誤！");
        }
        //protected void drpQuery2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    drpQueryText2.Text = drpQuery2.SelectedItem.Text;
        //    drpQueryValue2.Text = drpQuery2.SelectedItem.Value;
        //}
    }
}