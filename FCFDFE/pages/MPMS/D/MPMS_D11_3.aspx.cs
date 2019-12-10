using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D11_3 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                if (!IsPostBack)
                    List_DataImport(drpOVC_BUDGET_YEAR);
            }
        }

        protected void btnQuery_OVC_BUDGET_YEAR_Click(object sender, EventArgs e)
        {
            //按下 查詢按鈕
            DataImport();
        }


        #region 副程式
        private void DataImport()
        {
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strUserName="",strDept="";
            DataTable dt = new DataTable();
            if (strUSER_ID.Length > 0)
            {
                ACCOUNT ac = new ACCOUNT();
                ac = gm.ACCOUNTs.Where(table => table.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                if (ac != null)
                {
                    strUserName = ac.USER_NAME.ToString();
                    strDept = ac.DEPT_SN;
                }
                string strOVC_BUDGET_YEAR = drpOVC_BUDGET_YEAR.SelectedItem.ToString();
                string Compare = strOVC_BUDGET_YEAR.Substring((strOVC_BUDGET_YEAR.Length - 2), 2);

                /*
                 var queryOvcPurch =
                            (from tbm1301Plan in gm.TBM1301_PLAN
                             where tbm1301Plan.OVC_PURCHASE_UNIT.Equals(strDept)
                                 && tbm1301Plan.OVC_PURCH.Equals(strOVC_PURCH)
                             join tbm1301_1 in gm.TBM1301 on tbm1301Plan.OVC_PURCH equals tbm1301_1.OVC_PURCH
                             select tbm1301Plan
                    

                    
                */
                var query =
                    from tbm1301Plan in gm.TBM1301_PLAN
                    where tbm1301Plan.OVC_PURCHASE_UNIT.Equals(strDept)
                          && tbm1301Plan.OVC_PURCH.Substring(2, 2).Equals(Compare)
                    join tbm1301 in gm.TBM1301 on tbm1301Plan.OVC_PURCH equals tbm1301.OVC_PURCH
                    where tbm1301.OVC_PUR_DCANPO != null
                    orderby tbm1301Plan.OVC_PURCH
                    select new
                    {
                        OVC_PURCH = tbm1301Plan.OVC_PURCH,
                        OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                        OVC_PURCH_A = "",
                        OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                        OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                        OVC_PUR_DCANPO = tbm1301.OVC_PUR_DCANPO,
                        OVC_PUR_DCANRE = tbm1301.OVC_PUR_DCANRE,
                        //ONB_CHECK_TIMES = ONB_CHECK_TIMES 最大值
                        ONB_CHECK_TIMES = "",
                        //OVC_ASSIGNER = OVC_CHECK_OK ="N" & OVC_DRECEIVE 最大日期
                        OVC_ASSIGNER = "",
                        //OVC_STATUS = OVC_STATUS 最大值
                        OVC_STATUS_Desc = "",
                        OVC_STATUS = "",
                        OVC_PERMISSION_UPDATE = tbm1301.OVC_PERMISSION_UPDATE,
                        OVC_REMARK = "",
                    };

                /*
                var query =
                    from tbm1301 in gm.TBM1301
                    where tbm1301.OVC_PURCH.Substring(2, 2).Equals(Compare) && tbm1301.OVC_PUR_DCANPO != null
                    orderby tbm1301.OVC_PURCH
                    select new
                    {
                        OVC_PURCH = tbm1301.OVC_PURCH,
                        OVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY,
                        OVC_PURCH_A = "",
                        OVC_PUR_IPURCH = tbm1301.OVC_PUR_IPURCH,
                        OVC_PUR_NSECTION = tbm1301.OVC_PUR_NSECTION,
                        OVC_PUR_DCANPO = tbm1301.OVC_PUR_DCANPO,
                        OVC_PUR_DCANRE = tbm1301.OVC_PUR_DCANRE,
                        //ONB_CHECK_TIMES = ONB_CHECK_TIMES 最大值
                        ONB_CHECK_TIMES = "",
                        //OVC_ASSIGNER = OVC_CHECK_OK ="N" & OVC_DRECEIVE 最大日期
                        OVC_ASSIGNER = "",
                        //OVC_STATUS = OVC_STATUS 最大值
                        OVC_STATUS_Desc = "",
                        OVC_STATUS = "",
                        OVC_PERMISSION_UPDATE = tbm1301.OVC_PERMISSION_UPDATE,
                        OVC_REMARK = "",
                    };
                */
                dt = CommonStatic.LinqQueryToDataTable(query);

                foreach (DataRow rows in dt.Rows)
                {
                    string status_OVC_PURCH = rows["OVC_PURCH"].ToString();
                    rows["OVC_PURCH_A"] = status_OVC_PURCH + rows["OVC_PUR_AGENCY"].ToString();

                    //ONB_CHECK_TIMES = ONB_CHECK_TIMES 最大值
                    TBM1202 tbm1202 = mpms.TBM1202.Where(tb => tb.OVC_PURCH.Equals(status_OVC_PURCH))
                                    .OrderByDescending(tb => tb.ONB_CHECK_TIMES).FirstOrDefault();
                    if (tbm1202 != null)
                        rows["ONB_CHECK_TIMES"] = tbm1202.ONB_CHECK_TIMES;

                    //OVC_ASSIGNER = OVC_CHECK_OK ="N" & OVC_DRECEIVE 最大日期
                    TBM1202 tbm1202_2 = mpms.TBM1202.Where(tb => tb.OVC_PURCH.Equals(status_OVC_PURCH)
                                        && tb.OVC_CHECK_OK.Equals("N")).OrderByDescending(tb => tb.OVC_DRECEIVE).FirstOrDefault();
                    if (tbm1202_2 != null)
                        rows["OVC_ASSIGNER"] = tbm1202_2.OVC_ASSIGNER;

                    //OVC_STATUS = OVC_STATUS 最大值
                    TBMSTATUS tbmSTATUS = mpms.TBMSTATUS.Where(tb => tb.OVC_PURCH.Equals(status_OVC_PURCH))
                                .OrderByDescending(tb => tb.OVC_STATUS == null ? "0".Length : tb.OVC_STATUS =="3" ? "30".Length : tb.OVC_STATUS.Length)
                                .ThenByDescending(tb => tb.OVC_STATUS).FirstOrDefault();
                    string maxStatus = "";
                    if (tbmSTATUS != null)
                    {
                        maxStatus = tbmSTATUS.OVC_STATUS;
                        rows["OVC_STATUS"] = maxStatus;
                        rows["OVC_STATUS_Desc"] = GetTbm1407Desc("Q9", maxStatus);
                    }
                    string strOVC_REMARK = (rows["OVC_STATUS"].ToString() != "" && rows["OVC_PERMISSION_UPDATE"].ToString() == "Y") ? "<br/>" : string.Empty;
                    strOVC_REMARK += rows["OVC_PERMISSION_UPDATE"].ToString() == "Y" ? "退委辦(申購)單位澄覆或修訂中" : string.Empty;
                    rows["OVC_REMARK"] = strOVC_REMARK;
                }

                FCommon.GridView_dataImport(gvRevoked, dt);
            }
        }

       


        private void List_DataImport(ListControl list)
        {
            //設定DropDownList選項
            list.Items.Clear();

            int CalDateYear = Convert.ToInt16(DateTime.Now.ToString("yyyy")) - 1911;
            int num = CalDateYear;
            for (int i = 0; i < 15; i++)
            {
                list.Items.Add(Convert.ToString(num));
                num = num - 1;
            }
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

        }

}