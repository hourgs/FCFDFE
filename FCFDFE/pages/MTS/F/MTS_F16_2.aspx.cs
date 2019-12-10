using System;
using System.Linq;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Content;
using System.Data;

namespace FCFDFE.pages.MTS.F
{
    public partial class MTS_F16_2 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        MTSEntities MTSE = new MTSEntities();
        #region 副程式
        private string getQueryString()
        {
            string strQueryString = "";
            FCommon.setQueryString(ref strQueryString, "OVC_CLASS", Request.QueryString["OVC_CLASS"], false);
            return strQueryString;
            //在接收頁面加入此副程式
        }
        #endregion
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string err = "";
            decimal decSort;
            bool boolOnbCoSort = FCommon.checkDecimal(txtSort.Text, "排序", ref err, out decSort);
            if (txtOvcClass.Text != "" && txtOvcClassName.Text != "" && txtSort.Text != "")
            {
                try
                {
                    int sort = Convert.ToInt32(txtSort.Text);

                    int ifexist_class = MTSE.TBGMT_DEPT_CLASS.Where(table => table.OVC_CLASS == txtOvcClass.Text).Count();
                    int ifexist_name = MTSE.TBGMT_DEPT_CLASS.Where(table => table.OVC_CLASS_NAME == txtOvcClassName.Text).Count();
                    int ifexist_sort = MTSE.TBGMT_DEPT_CLASS.Where(table => table.ONB_SORT == sort).Count();
                    if (ifexist_sort == 0 && ifexist_name == 0 && ifexist_sort == 0 && decSort > 0)
                    {
                        try
                        {
                            TBGMT_DEPT_CLASS dc = new TBGMT_DEPT_CLASS();
                            dc.OVC_CLASS = txtOvcClass.Text;
                            dc.OVC_CLASS_NAME = txtOvcClassName.Text;
                            dc.ONB_SORT = sort;
                            dc.OVC_DEPTCLA_SN = Guid.NewGuid();

                            MTSE.TBGMT_DEPT_CLASS.Add(dc);
                            MTSE.SaveChanges();
                            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), dc.GetType().Name.ToString(), this, "新增");
                            FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增接轉單位種類資料成功！");
                        }
                        catch (Exception ex)
                        {
                            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "新增接轉單位種類資料失敗！");
                        }
                    }
                    else
                    {
                        if (ifexist_class > 0)
                        {
                            err += "<p> 類別代碼已重複！</p>";
                        }
                        if (ifexist_name > 0)
                        {
                            err += "<p> 類別名稱已重複！</p>";
                        }
                        if (ifexist_sort > 0)
                        {
                            err += "<p> 排序已重複！</p>";
                        }
                        if(decSort <= 0)
                        {
                            err += "<p> 排序請輸入正數！</p>";
                        }
                    }
                }
                catch
                {
                    err += "<p> 排序請輸入正數！</p>";
                }

                   
            }
            else
            {
                 if (txtOvcClass.Text == "")
                 {
                    err += "<p> 請輸入類別代碼！</p>";
                 }
                 if (txtOvcClassName.Text == "")
                 {
                    err += "<p> 請輸類別名稱！</p>";
                 }
                 if (txtSort.Text == "")
                 {
                    err += "請輸入排序！<br>";
                 }
             }
            if (err != "")
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", err);
            }
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect($"MTS_F16{ getQueryString() }");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            //{
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            //}
        }
    }
}