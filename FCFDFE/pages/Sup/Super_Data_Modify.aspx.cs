using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using System.Web.UI;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.Sup
{
    public partial class Super_Data_Modify : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities GME = new GMEntities();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                var TBM1407_01 = GME.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("SP") && table.OVC_PHR_ID.Equals("01")).FirstOrDefault();

                if (TBM1407_01 != null)
                {
                    Loginmax_Text.Text = TBM1407_01.OVC_PHR_DESC.ToString();

                }

                var TBM1407_02 = GME.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("SP") && table.OVC_PHR_ID.Equals("03")).FirstOrDefault();

                if (TBM1407_02 != null)
                {
                    Password_Text.Text = TBM1407_02.OVC_PHR_DESC.ToString();
                }

                var TBM1407_03 = GME.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("SP") && table.OVC_PHR_ID.Equals("02")).FirstOrDefault();
                if (TBM1407_03 != null)
                {
                    SeverPwd_Text.Text = TBM1407_03.OVC_PHR_DESC.ToString();
                }
            }
        }

        protected void Loginmax_Save(object sender, EventArgs e)
        {
            string strLoginmax_Text = Loginmax_Text.Text;

            TBM1407 tbm1407 = GME.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("SP") && table.OVC_PHR_ID.Equals("01")).FirstOrDefault();
            if (tbm1407 != null)
            {
                tbm1407.OVC_PHR_DESC = strLoginmax_Text;
                GME.SaveChanges();
                FCommon.AlertShow(PnMessage_Account, "success", "系統訊息", "修改成功");
            }
        }

        protected void Pwd_change(object sender, EventArgs e)

        {

            Pwd_modify.Visible = true;
        }

        protected void SerPwd_change(object sender, EventArgs e)
        {


            SerPwd_modify.Visible = true;
        }

        protected void Pwd_Save(object sender, EventArgs e)
        {
            string strMessage = "";

            string strNewPwd = NewPwd_Text.Text;
            string strNewPwdCon = NewPwdCon_Text.Text;

            if (!strNewPwd.Equals(strNewPwdCon))
            {
                strMessage += "<P> 密碼設定 與 再次輸入密碼 不符 </p>";
                FCommon.AlertShow(PnMessage_Account, "danger", "系統訊息", strMessage);
            }
            else
            {
                TBM1407 tbm1407_1 = GME.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("SP") && table.OVC_PHR_ID.Equals("03")).FirstOrDefault();
                if (tbm1407_1 != null)
                {
                    tbm1407_1.OVC_PHR_DESC = strNewPwd;
                    GME.SaveChanges();
                    FCommon.AlertShow(PnMessage_Account, "success", "系統訊息", "存檔成功");
                    Pwd_modify.Visible = false;
                }

            }


        }

        protected void SerPwd_Save(object sender, EventArgs e)
        {
            string strMessage = "";
            string strNewSerPwd = NewSerPwd_Text.Text;
            string strNewSerPwdCon = NewSerPwdCon_Text.Text;

            if (!strNewSerPwd.Equals(strNewSerPwdCon))
            {
                strMessage += "<P> 密碼設定 與 再次輸入密碼 不符 </p>";
                FCommon.AlertShow(PnMessage_Account, "danger", "系統訊息", strMessage);
            }
            else
            {
                TBM1407 tbm1407_02 = GME.TBM1407.Where(table => table.OVC_PHR_CATE.Equals("SP") && table.OVC_PHR_ID.Equals("02")).FirstOrDefault();
                if (tbm1407_02 != null)
                {
                    tbm1407_02.OVC_PHR_DESC = strNewSerPwd;
                    GME.SaveChanges();
                    FCommon.AlertShow(PnMessage_Account, "success", "系統訊息", "存檔成功");
                    SerPwd_modify.Visible = false;
                }
            }
        }
    }
}