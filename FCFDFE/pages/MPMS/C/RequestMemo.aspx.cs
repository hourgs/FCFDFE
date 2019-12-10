using FCFDFE.Content;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;

namespace FCFDFE.pages.MPMS.C
{
    public partial class RequestMemo : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strPurNum = "JC96001";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["OVC_PURCH"]))
            {
                strPurNum = Request.QueryString["OVC_PURCH"];
                DataImport();
            }
            
        }

        private void DataImport()
        {
            var queryKind = gm.TBM1301.Where(o => o.OVC_PURCH.Equals(strPurNum)).Select(o => o.OVC_PUR_AGENCY).FirstOrDefault();
            //請求事項
            lblTitleUnit.Text = strPurNum + queryKind;
            string ovcIkindR = "";
            string ovcIkinMark = "";
            switch (queryKind)
            {
                case "B":
                case "L":
                case "P":
                    ovcIkindR = "M3";
                    break;
                case "M":
                case "S":
                    ovcIkindR = "F3";
                    break;
                default:
                    ovcIkindR = "W3";
                    break;
            }
            txtBoxInIt(txtRequestMemo, ovcIkindR);

            //備考
            switch (queryKind)
            {
                case "B":
                case "L":
                case "P":
                    ovcIkinMark = "M4";
                    break;
                case "M":
                case "S":
                    ovcIkinMark = "F4";
                    break;
                default:
                    ovcIkinMark = "W4";
                    break;
            }
            txtBoxInIt(txtMarkMemo, ovcIkinMark);
        }

        private void txtBoxInIt(TextBox textBox, string ikind)
        {
            //放請求事項跟備考
            List<string> HasMemo = new List<string>();
            List<string> NotMemo = new List<string>();
            var queryMemo =
                from tMemo in mpms.TBM1220_1
                where tMemo.OVC_PURCH.Equals(strPurNum) && tMemo.OVC_IKIND.StartsWith(ikind)
                select tMemo;

            var query =
                from tName in mpms.TBM1220_2
                join tMemo in queryMemo on tName.OVC_IKIND equals tMemo.OVC_IKIND into ps
                from tMemo in ps.DefaultIfEmpty()
                where tName.OVC_IKIND.StartsWith(ikind)
                orderby tName.OVC_IKIND
                select new
                {
                    tName.OVC_MEMO_NAME,
                    OVC_MEMO = tMemo != null ? tMemo.OVC_MEMO : ""
                };
            
            foreach(var item in query)
            {
                if (string.IsNullOrEmpty(item.OVC_MEMO))
                {
                    string text = item.OVC_MEMO_NAME + "：（空白）";
                    NotMemo.Add(text);
                }
                else
                {
                    string text = item.OVC_MEMO_NAME + "：" + item.OVC_MEMO.Replace("<br>","");
                    HasMemo.Add(text);
                }
            }

           
            foreach (string has in HasMemo)
                textBox.Text += has + "\r\n\r\n";
            if (!textBox.Text.Equals(string.Empty))
                textBox.Text += "===============================================\r\n\r\n";
            foreach (string not in NotMemo)
                textBox.Text += not + "\r\n\r\n";
        }
    }
}