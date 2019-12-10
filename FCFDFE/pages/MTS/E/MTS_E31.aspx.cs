using System;
using FCFDFE.Content;
using FCFDFE.Entity.MTSModel;
using FCFDFE.Entity.GMModel;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace FCFDFE.pages.MTS.E
{
    public partial class MTS_E31 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        private GMEntities gme = new GMEntities();
        private MTSEntities mtse = new MTSEntities();
        Common FCommon = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userid"] != null)
                {
                    string strUser = Session["userid"].ToString();
                    var query = gme.ACCOUNTs.Where(id => id.USER_ID == strUser).FirstOrDefault();
                    if (query.DEPT_SN != null)
                    {
                        string strdept = query.DEPT_SN;
                        var querydept = gme.TBMDEPTs.Where(id => id.OVC_DEPT_CDE == strdept).FirstOrDefault();
                        lblDept.Text = querydept.OVC_ONNAME;
                        txtOvcPlnContent.Text = "呈核後，送中心主計室結報。";
                        DataTable dt = CommonStatic.ListToDataTable(mtse.TBGMT_DEPT_CLASS.OrderBy(table=>table.ONB_SORT).Select(x => x).ToList());
                        list_dataImport(drpOvcMilitaryType, dt, "OVC_CLASS_NAME", "OVC_CLASS");
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strOvcMilitaryType = drpOvcMilitaryType.SelectedItem.Text;
            string strDept = drpOvcMilitaryType.SelectedValue.ToString();
            string strOVcIEType = drpOvcIeType.SelectedItem.Text;
            string strOvcPurposeType = drpOvcPurposeType.SelectedValue.ToString();
            string strOvcAbstract = txtOvcAbstract.Text;
            string strOnbAmount = txtOnbAmount.Text;
            string strOvcNote = txtOvcNote.Text;
            string strOvcPlnContent = txtOvcPlnContent.Text;
            string strMessage = "";
            int CalDateYear = Convert.ToInt16(DateTime.Now.ToString("yyyy")) - 1911;
            string tofsn = CalDateYear + strDept;
            string UserDept = "";

            if (lblDept.Text.Contains("基隆地區"))
                UserDept = "基隆接轉組";
            if (lblDept.Text.Contains("桃園地區"))
                UserDept = "桃園接轉組";
            if (lblDept.Text.Contains("高雄分遣組"))
                UserDept = "高雄接轉組";
            if(UserDept.Equals(string.Empty))
                strMessage += "<P> 非各地區接轉運費結報承辦人，無法新增結報申請表！！ </p>";
            if (strOvcAbstract.Equals(string.Empty))
                strMessage += "<P> 摘要欄位不得為空白 </p>";
            if (strOnbAmount.Equals(string.Empty))
                strMessage += "<P> 金額欄位不得為空白</p>";
            if (!strOnbAmount.Equals(string.Empty))
            {
                int n;
                if(int.TryParse(strOnbAmount, out n))
                {

                }
                else
                {
                    strMessage += "<P> 金額請輸入數字 </p>";
                }
            }
            if (strMessage.Equals(string.Empty))
            {
                int onbamonunt = Convert.ToInt32(strOnbAmount);
                string strUser = Session["userid"].ToString();

                var querytof = mtse.TBGMT_TOF.Where(table => table.OVC_TOF_NO.StartsWith(tofsn)).OrderByDescending(table => table.OVC_TOF_NO).FirstOrDefault();
                int num = 1;
                if (querytof != null)
                //num = Convert.ToInt16(querytof.OVC_TOF_NO.Substring(4, 4)) + 1;
                {
                    int OVC_TOF_NO_LENGTH = querytof.OVC_TOF_NO.ToString().Length;
                    if(OVC_TOF_NO_LENGTH==8)
                        num = Convert.ToInt16(querytof.OVC_TOF_NO.Substring(4, 4)) + 1;
                    else
                        num = Convert.ToInt16(querytof.OVC_TOF_NO.Substring(6, 4)) + 1;


                }
                TBGMT_TOF tof = new TBGMT_TOF();
                tof.OVC_TOF_NO = tofsn + num.ToString("0000");
                tof.OVC_MILITARY_TYPE = strOvcMilitaryType;
                tof.OVC_IE_TYPE = strOVcIEType;
                tof.OVC_PURPOSE_TYPE = strOvcPurposeType;
                tof.OVC_ABSTRACT = strOvcAbstract;
                tof.ONB_AMOUNT = onbamonunt;
                tof.OVC_NOTE = strOvcNote;
                tof.OVC_PLN_CONTENT = strOvcPlnContent;
                tof.OVC_SECTION = "";
                tof.OVC_APPLY_ID = strUser;
                tof.ODT_APPLY_DATE = DateTime.Now;
                tof.ODT_MODIFY_DATE = DateTime.Now;
                tof.OVC_IS_PAID = "未付款";
                tof.OVC_SECTION = UserDept;
                mtse.TBGMT_TOF.Add(tof);
                mtse.SaveChanges();
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "新增接轉作業費結報申請表成功，申請表編號:"+tof.OVC_TOF_NO);                
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
        }

        public void list_dataImport(ListControl list, DataTable dt, string textField, string valueField)
        {
            //先將下拉式選單清空
            list.Items.Clear();
            list.AppendDataBoundItems = true;
            list.DataSource = dt;
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataBind();
        }
    }
}