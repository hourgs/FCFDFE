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

namespace FCFDFE.pages.GM
{
    public partial class BulletinBoardMemo : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strBB_SN;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["BB_SN"]))
            {
                FCommon.Controls_Attributes("readonly", "true", txtCONTEXT);
                strBB_SN = Request.QueryString["BB_SN"];
                DataImport();
            }
            
        }

        private void DataImport()
        {
            Guid guid = new Guid(strBB_SN);
            var query = gm.BILLBOARDs.Where(t => t.BB_SN.Equals(guid)).FirstOrDefault();
            if (query != null)
            {
                lblTITLE.Text = query.TITLE;
                lblAUTHOR_ID.Text = query.AUTHOR_ID;
                lblSTART_DATE.Text = query.START_DATE == null ? "" : query.START_DATE.ToString();
                lblEND_DATE.Text = query.END_DATE == null ? "" : query.END_DATE.ToString();
                txtCONTEXT.Text = query.CONTEXT;
            }
        }
    }
}