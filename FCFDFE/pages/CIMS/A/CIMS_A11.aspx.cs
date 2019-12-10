using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.CIMSModel;
using System.Data.Entity;


namespace FCFDFE.pages.CIMS.A
{
    public partial class CIMS_A11 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        CIMSEntities CIMS = new CIMSEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
            {
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
                if (!IsPostBack)
                {
                    ShowRENGO();
                    FCommon.Controls_Attributes("readonly", "true", txtAUTH_DATE_S, txtAUTH_DATE_E, txtAPPR_DATE_S, txtAPPR_DATE_E);
                }
                if (Manual.Checked == true)
                {
                    txtREGNO_Manual.Visible = true;
                    txtREGNO.Visible = false;
                }
                else
                {
                    txtREGNO_Manual.Visible = false;
                    txtREGNO.Visible = true;
                }
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            string strMessage = "";
            if (Manual.Checked==true)
            {
                if (txtREGNO_Manual.Text == "")
                {
                    strMessage = "<P> 您已勾選手動輸入，請輸入申請序號/編號重複或是取消勾選 </p>";
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                    return;
                }
                VENAGENT VENAGENTquery = new VENAGENT();
                VENAGENTquery = CIMS.VENAGENT.Where(table => table.REGNO.Equals(txtREGNO_Manual.Text)).FirstOrDefault();
                if (VENAGENTquery != null)
                {
                    strMessage = "<P> 手動輸入之申請序號/編號重複 </p>";
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                    return;
                }
                int num1;
                if (txtREGNO_Manual.Text.Length != 5 || txtREGNO_Manual.Text.Substring(0, 1) != "R" || int.TryParse(txtREGNO_Manual.Text.Substring(1), out num1) == false)
                {
                    strMessage = "<P> 手動輸入之申請序號/編號格式錯誤。格式為R加上4位數字，例:R0023 </p>";
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", strMessage);
                    return;
                }

                
                
            }
            VENAGENT VENAGENTadd = new VENAGENT();
            if (Manual.Checked == true)
                VENAGENTadd.REGNO = txtREGNO_Manual.Text;
            else
                VENAGENTadd.REGNO = txtREGNO.Text;
            VENAGENTadd.VEN_NAME = txtVEN_NAME.Text;
            VENAGENTadd.VEN_DEPT = txtVEN_DEPT.Text;
            VENAGENTadd.VEN_CODE = txtVEN_CODE.Text;
            VENAGENTadd.VEN_ADDR = txtVEN_ADDR.Text;
            VENAGENTadd.VEN_TEL = txtVEN_TEL.Text;
            VENAGENTadd.VEN_FAX = txtVEN_FAX.Text;
            VENAGENTadd.VEN_BOSS = txtVEN_BOSS.Text;
            VENAGENTadd.VEN_NAME_T = txtVEN_NAME_T.Text;
            VENAGENTadd.VEN_ENAME_T = txtVEN_ENAME_T.Text;
            VENAGENTadd.VEN_CODE_T = txtVEN_CODE_T.Text;
            VENAGENTadd.VEN_ADDR_T = txtVEN_ADDR_T.Text;
            VENAGENTadd.VEN_TEL_T = txtVEN_TEL_T.Text;
            VENAGENTadd.VEN_FAX_T = txtVEN_FAX_T.Text;
            VENAGENTadd.VEN_BOSS_T = txtVEN_BOSS_T.Text;
            if(txtAUTH_DATE_S.Text!="")
                VENAGENTadd.AUTH_DATE_S = Convert.ToDateTime(txtAUTH_DATE_S.Text);
            VENAGENTadd.AUTH_DATE_E = txtAUTH_DATE_E.Text;
            VENAGENTadd.AUTH_RANGE = txtAUTH_RANGE.Text;
            VENAGENTadd.AGENT_ITEM = txtAGENT_ITEM.Text;
            VENAGENTadd.APPR_DATE_S = txtAPPR_DATE_S.Text;
            VENAGENTadd.APPR_DATE_E = txtAPPR_DATE_E.Text;
            VENAGENTadd.OVC_MEMO = txtOVC_MEMO.Text;
            VENAGENTadd.OVC_CREATE = DateTime.Now.ToString("yyyy-MM-dd");
            VENAGENTadd.KEYIN = Session["username"].ToString();
            CIMS.VENAGENT.Add(VENAGENTadd);
            CIMS.SaveChanges();
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "代理商新增成功");
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), VENAGENTadd.GetType().Name.ToString(), this, "代理商新增");
            ClearData();
        }

        protected void ShowRENGO()
        {
            DataTable dt = new DataTable();
            var query =
                from VENAGENT in CIMS.VENAGENT.DefaultIfEmpty().AsEnumerable()
                orderby VENAGENT.REGNO descending
                select new
                {
                    REGNO = VENAGENT.REGNO
                };
            dt = CommonStatic.LinqQueryToDataTable(query);
            string RENGO_Last = dt.Rows[0][0].ToString();
            int D = Convert.ToInt16(RENGO_Last.Substring(1)) + 1;
            string RENGO = "R" + D.ToString();

            //Boolean flag  = false;
            //int i = 0;
            //string RENGO="";
            //while (flag == false)
            //{
            //    if (i == dt.Rows.Count - 2)
            //    {
            //        RENGO = "R" + (Convert.ToInt16(dt.Rows[i][0].ToString().Substring(1))+1).ToString();
            //        flag = true;
            //    }
            //    else
            //    {
            //        int D1 = Convert.ToInt16(dt.Rows[i][0].ToString().Substring(1));
            //        int D2 = Convert.ToInt16(dt.Rows[i + 1][0].ToString().Substring(1));
            //        if (D2 - D1 != 1)
            //        {
            //            switch ((D1 + 1).ToString().Length)
            //            {
            //                case 1:
            //                    RENGO = "R000" + (D1 + 1);
            //                    break;
            //                case 2:
            //                    RENGO = "R00" + (D1 + 1);
            //                    break;
            //                case 3:
            //                    RENGO = "R0" + (D1 + 1);
            //                    break;
            //                default:
            //                    RENGO = "R" + (D1 + 1);
            //                    break;

            //            }
            //            flag = true;
            //        }
            //        else
            //        {
            //            i += 1;
            //        }

            //    }
            //}
            txtREGNO.Text = RENGO;
        }

        protected void ClearData()
        {
            FCommon.Controls_Clear(txtREGNO_Manual, txtREGNO, txtVEN_NAME, txtVEN_DEPT, txtVEN_CODE, txtVEN_ADDR, txtVEN_TEL, txtVEN_FAX, txtVEN_BOSS, txtVEN_NAME_T, txtVEN_ENAME_T,
                txtVEN_CODE_T, txtVEN_ADDR_T, txtVEN_TEL_T, txtVEN_FAX_T, txtVEN_BOSS_T, txtAUTH_DATE_S, txtAUTH_DATE_E, txtAUTH_RANGE, txtAGENT_ITEM, txtAPPR_DATE_S, txtAPPR_DATE_E, txtOVC_MEMO);
            Manual.Checked = false;

        }
    }
}