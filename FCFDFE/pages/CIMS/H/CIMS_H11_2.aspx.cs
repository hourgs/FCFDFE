﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.CIMS.H
{
    public partial class CIMS_H12_2 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_long.Text = Request.QueryString["text_long"];
        }
    }
}