using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;
using System.Collections;
using FCFDFE.Content;

namespace FCFDFE
{
    
    public partial class SiteMaster : MasterPage
    {
        //public string theSystem;
        Common FCommon = new Common();
        private GMEntities GME = new GMEntities();
        public string strSYSTEM = "";


        #region 副程式
        private void importPanel(Panel thePanel, ref string str)
        {
            thePanel.Controls.Add(new LiteralControl(str));
            str = string.Empty;
        }

        private void importMenu() //匯入資料庫之選單
        {
            string strSystem = "";
            //strSystem += "<ul class='nav top-menu'>";
            //var allauthsys =
            //    from authaccount in GME.ACCOUNT_AUTH.DefaultIfEmpty().AsEnumerable().Where(Table => Table.USER_ID == Session["userid"].ToString())
            //    join menupage1 in GME.MENU_PAGES.DefaultIfEmpty().AsEnumerable() on authaccount.C_SN_SYS equals menupage1.M_SN
            //    orderby menupage1.M_ORD
            //    select new
            //    {
            //        SYSCODE = menupage1.M_SN,
            //        SYSNAME = menupage1.M_NAME
            //    };
            //
            //foreach (var itemSys in allauthsys.Distinct())
            //{
            //    string strSystemId = itemSys.SYSCODE;
            //    strSystem += "<li id='li_" + strSystemId + "' class='drop down'>";
            //    importPanel(pn_topMenu, ref strSystem);
            //
            //    //新增LinkButton
            //    LinkButton theButton = new LinkButton();
            //    theButton.ID = "btn" + strSystemId;
            //    theButton.Text = itemSys.SYSNAME;
            //    theButton.Click += new EventHandler(btnSys_Click);
            //    pn_topMenu.Controls.Add(theButton);
            //
            //    //取得資料庫新增左方選單
            //    Panel thePanel = new Panel();
            //    thePanel.ID = strSystemId;
            //    pn_leftMenu.Controls.Add(thePanel);
            //
            //    string strPages = "";
            //    strPages += "";
            //    strPages += "<div class='nav-collapse menu-bar sidebar' visible='false'>";
            //    strPages += "<ul class='sidebar-menu'>";
            //
            //    if (strSystemId != "GM")
            //    {
            //        //新增左方選單內容
            //        //讀取第一階層資料
            //        var allauthmod =
            //        from authaccount in GME.ACCOUNT_AUTH.DefaultIfEmpty().AsEnumerable().Where(Table => Table.USER_ID == Session["userid"].ToString())
            //        join authmenu in GME.AUTH_MENU.DefaultIfEmpty().AsEnumerable() on authaccount.C_SN_AUTH equals authmenu.GROUP_SN
            //        join menupage1 in GME.MENU_PAGES.DefaultIfEmpty().AsEnumerable() on authmenu.MENU_SN equals menupage1.M_SN
            //        join menupage2 in GME.MENU_PAGES.DefaultIfEmpty().AsEnumerable() on menupage1.M_SN_PAR equals menupage2.M_SN
            //        where menupage2.M_MODEL != "" & menupage2.M_SN_PAR.Equals(strSystemId)
            //        orderby menupage2.M_ORD
            //        select menupage2;
            //
            //
            //        foreach (var itemModel in allauthmod.Distinct())
            //        {
            //            string strModelName = itemModel.M_NAME;
            //            string strModelCode = itemModel.M_MODEL;
            //            //URL空的表示還有下一階層~
            //            //讀取第二階層資料
            //            string strModelId = itemModel.M_SN;
            //
            //            var allauthpage =
            //            from authaccount in GME.ACCOUNT_AUTH.DefaultIfEmpty().AsEnumerable().Where(Table => Table.USER_ID == Session["userid"].ToString())
            //            join authmenu in GME.AUTH_MENU.DefaultIfEmpty().AsEnumerable() on authaccount.C_SN_AUTH equals authmenu.GROUP_SN
            //            join menupage1 in GME.MENU_PAGES.DefaultIfEmpty().AsEnumerable() on authmenu.MENU_SN equals menupage1.M_SN
            //            where menupage1.M_SN_PAR.Equals(strModelId)
            //            where menupage1.M_VISIBLE.Equals("Y")
            //            orderby menupage1.M_ORD
            //            select menupage1;
            //
            //            strPages += "<li id='li_" + strModelCode + "' class='sub-menu' runat='server'>";
            //            strPages += "<a href='#'>";
            //            strPages += "<span>" + strModelName + "</span>";
            //            strPages += "<span class='arrow'></span>";
            //            strPages += "</a>";
            //            strPages += "<ul class='sub'>";
            //            foreach (var itemPage in allauthpage)
            //            {
            //                string strURL = itemPage.M_URL;
            //                string strField = strURL.Substring(strURL.IndexOf("_") + 1, 1);
            //                strPages += "<li id='li_" + strURL + "'><a  href='" + ResolveClientUrl("~/pages/" + strSystemId + "/" + strField + "/" + strURL) + "'>" + itemPage.M_NAME + "</a></li>";
            //            }
            //
            //            strPages += "</ul>";
            //            strPages += "</li>";
            //        }
            //
            //    }
            //    else
            //    {//GM
            //        var allauthmod =
            //         from authaccount in GME.ACCOUNT_AUTH.DefaultIfEmpty().AsEnumerable().Where(Table => Table.USER_ID == Session["userid"].ToString())
            //         join authmenu in GME.AUTH_MENU.DefaultIfEmpty().AsEnumerable() on authaccount.C_SN_AUTH equals authmenu.GROUP_SN
            //         join menupage1 in GME.MENU_PAGES.DefaultIfEmpty().AsEnumerable() on authmenu.MENU_SN equals menupage1.M_SN
            //         where menupage1.M_SN_PAR.Equals(strSystemId)
            //         orderby menupage1.M_ORD
            //         select menupage1;
            //
            //        foreach (var itemModel in allauthmod.Distinct())
            //        {
            //            string strModelName = itemModel.M_NAME;
            //            string strModelCode = itemModel.M_MODEL;
            //            //URL空的表示還有下一階層~
            //            //讀取第二階層資料
            //            string strModelId = itemModel.M_SN;
            //
            //            strPages += "<li id='li_" + strModelCode + "' class='sub-menu' runat='server'>";
            //
            //            strPages += "<a href='" + ResolveClientUrl("~/pages/" + strSystemId + "/" + itemModel.M_URL) + "'>";
            //            strPages += "<span>" + strModelName + "</span>";
            //            strPages += "</a>";
            //
            //            strPages += "</li>";
            //        }
            //    }
            //    strPages += "</ul>";
            //    strPages += "</div>";
            //    importPanel(thePanel, ref strPages);
            //
            //    strSystem += "</li>";
            //}
            //strSystem += "</ul>";
            //importPanel(pn_topMenu, ref strSystem);

            /////////////////////////////////////////////////////////////
            //舊語法
            //取得資料庫新增上方系統別按鈕
            //string strSystem = "";
            strSystem += "<ul class='nav top-menu'>";
            var querySys =
                from tableSys in GME.MENU_PAGES
                where tableSys.M_SN_PAR.Equals("SYSTEM")
                where tableSys.M_VISIBLE.Equals("Y")
                orderby tableSys.M_ORD
                select tableSys;
            foreach (var itemSys in querySys)
            {
                string strSystemId = itemSys.M_SN;
                strSystem += "<li class='drop down'>";
                importPanel(pn_topMenu, ref strSystem);

                //新增LinkButton
                LinkButton theButton = new LinkButton();
                theButton.ID = "btn" + strSystemId;
                theButton.Text = itemSys.M_NAME;
                theButton.Click += new EventHandler(btnSys_Click);
                theButton.CausesValidation = false;
                pn_topMenu.Controls.Add(theButton);

                //取得資料庫新增左方選單
                Panel thePanel = new Panel();
                thePanel.ID = strSystemId;
                pn_leftMenu.Controls.Add(thePanel);

                //新增左方選單內容
                //讀取第一階層資料
                var queryModel =
                    from tableModule in GME.MENU_PAGES
                    where tableModule.M_SN_PAR.Equals(strSystemId)
                    where tableModule.M_VISIBLE.Equals("Y")
                    orderby tableModule.M_ORD
                    select tableModule;

                string strPages = "";
                strPages += "";
                strPages += "<div class='nav-collapse menu-bar sidebar' visible='false'>";
                strPages += "<ul class='sidebar-menu'>";
                foreach (var itemModel in queryModel)
                {
                    string strModelName = itemModel.M_NAME;
                    string strModelCode = itemModel.M_MODEL;
                    if (itemModel.M_URL == null)
                    {//URL空的表示還有下一階層~
                     //讀取第二階層資料
                        string strModelId = itemModel.M_SN;
                        var queryPage =
                            from tablePage in GME.MENU_PAGES
                            where tablePage.M_SN_PAR.Equals(strModelId)
                            where tablePage.M_VISIBLE.Equals("Y")
                            orderby tablePage.M_ORD
                            select tablePage;
                        strPages += "<li id='li_" + strModelCode + "' class='sub-menu' runat='server'>";
                        strPages += "<a href='#'>";
                        strPages += "<span>" + strModelName + "</span>";
                        strPages += "<span class='arrow'></span>";
                        strPages += "</a>";
                        strPages += "<ul class='sub'>";
                        foreach (var itemPage in queryPage)
                        {
                            string strURL = itemPage.M_URL;
                            string strField = strURL.Substring(strURL.IndexOf("_") + 1, 1);
                            strPages += "<li id='li_" + strURL + "'><a  href='" + ResolveClientUrl("~/pages/" + strSystemId + "/" + strField + "/" + strURL) + "'>" + itemPage.M_NAME + "</a></li>";
                        }

                        strPages += "</ul>";
                        strPages += "</li>";
                    }
                    else
                    {
                        strPages += "<li id='li_" + strModelCode + "' class='sub-menu' runat='server'>";

                        strPages += "<a href='" + ResolveClientUrl("~/pages/" + strSystemId + "/" + itemModel.M_URL) + "'>";
                        strPages += "<span>" + strModelName + "</span>";
                        strPages += "</a>";

                        strPages += "</li>";
                    }
                }
                strPages += "</ul>";
                strPages += "</div>";
                importPanel(thePanel, ref strPages);

                strSystem += "</li>";
            }
            strSystem += "</ul>";
            importPanel(pn_topMenu, ref strSystem);
        }
        private void setSystemMenu(string theSystem) //設定被選取的選單顯示，其他選單隱藏
        {
            if (theSystem == null)
            {
                string[] strUrls = Request.FilePath.Split('/');
                int intCouunt = strUrls.Length;
                for (int i = 0; i < intCouunt; i++)
                {
                    string strUrl = strUrls[i];
                    if (strUrl.Equals("pages") && i + 1 < intCouunt)
                    {
                        theSystem = strUrls[i + 1];
                        break;
                    }
                }
            }
            if (theSystem == null) theSystem = "GM";
            strSYSTEM = theSystem;

            if (ViewState["sysmpkmscount"] == null)
                ViewState["sysmpkmscount"] = 0;

            if (strSYSTEM == "MPKMS")
            {
                ViewState["sysmpkmscount"] = Convert.ToInt16(ViewState["sysmpkmscount"].ToString()) + 1;
                if (Convert.ToInt16(ViewState["sysmpkmscount"].ToString()) % 2 == 1)
                    Response.Write("<script language='javascript'>window.open('https://mpkms.mil.tw');</script>");

            }
            else
            {
                foreach (Control item in pn_leftMenu.Controls)
                {
                    if (item.GetType() == typeof(Panel))
                        item.Visible = false;
                }
                Panel thePanel = (Panel)pn_leftMenu.FindControl(theSystem);
                if (thePanel != null)
                    thePanel.Visible = true;
            }
        }

        public void MessageBoxShow(string strMessage, string strUrl, bool isRootDirectory)
        {
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Message Box", "<script language = 'javascript'>alert('" + msg + "');location.href='../../../../login.aspx';</script>");

            if (isRootDirectory)
                strUrl = Page.ResolveClientUrl("~/" + strUrl);
            string strScript =
                "<script language='javascript'>" +
                    "alert('" + strMessage + "');" +
                    "location.href='" + strUrl + "';" +
                "</script>";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "MessageBox", strScript);
        }

        public void ShowOnLineUserList_Fn(string strUserName_Val, string strIP, string strName, string strTime)
        {
            //宣告一維陣列清單會依照增加項目的多少而動態調整大小。
            ArrayList arrltTemp = new ArrayList();
            ArrayList arrltIp = new ArrayList();
            //ArrayList arrltName = new ArrayList();
            //ArrayList arrltTime = new ArrayList();
            //宣告字串變數。(用戶識別碼)
            string strUserID = null;
            string strUserIP = null;
            //string strUserName = null;
            //string strUserTime = null;
            //宣告字串變數。(取得進入頁面後系統給予的 Session 唯一識別碼)
            string strUserSessionID = strUserName_Val;
            string ip = strIP;
            //string name = strName;
            //string time = strTime;

            //宣告整數變數。(人數計數器)
            int intNum = 0;
            //宣告整數變數。(迴圈計數器)
            int i = 0;
            //宣告整數變數。(距離多少時間(秒)的「用戶最後存取時間」才算是離開)
            int intCheckTickSeconds = 3000;
            //新增此 Session 變數以使 Session 物件能正常運作。
            Session["Flag"] = true;
            //宣告日期物件變數。(取得目前時間)
            DateTime dtDateTime = DateTime.Now;


            //鎖定全域變數存取。
            Application.Lock();


            //當目前進入用戶的「UserSessionID 和最後存取時間」的
            //Application 名稱全域變數為空值時。(代表該用戶為第一次進入)
            if (Application[strUserSessionID + "LastAccessTime_A"] == null)
            {
                //當 Application 「所有人數數量」為無值時，先設定預設值。(不然存取會有錯誤)
                if (Application["TotalUsers_A"] == null)
                {
                    //指派變數名稱值為 0。(所有線上用戶數量)
                    Application.Set("TotalUsers_A", 0);
                }

                //宣告一維陣列清單會依照增加項目的多少而動態調整大小。(取得 Application 的「線上用戶名單」陣列)
                ArrayList arrltOnlineUserC = (ArrayList)Application["OnlineUser_A"];
                ArrayList arrltOnlineUserIp = (ArrayList)Application["OnlineIp"];
                //ArrayList arrltOnlineUserName = (ArrayList)Application["OnlineName"];
                //ArrayList arrltOnlineUserTime = (ArrayList)Application["OnlineTime"];
                //設定累計數值值為 0。
                intNum = 0;

                //如果「總人數計數」大於0時。(代表線上已經有人進入，不是第一次進來的狀況)
                if ((int)Application["TotalUsers_A"] > 0)
                {
                    //當陣列內容不是無值時。(線上用戶名單)
                    if (arrltOnlineUserC != null)
                    {
                        //讀取「線上用戶名單」陣列範圍，將「新加入用戶」與 
                        //「舊有存在用戶」陣列內容做比較看是否以經存在。

                        //利用「元素長度」取得一維陣列內容值。(線上用戶名單)
                        for (i = 0; i < arrltOnlineUserC.Count; i++)
                        {
                            //取出存放在「舊有存在用戶」陣列裡面的用戶名單。
                            strUserID = arrltOnlineUserC[i].ToString();
                            strUserIP = arrltOnlineUserIp[i].ToString();
                            //strUserName = arrltOnlineUserName[i].ToString();
                            //strUserTime = arrltOnlineUserTime[i].ToString();
                            //當「舊有存在用戶」不等於「新加入用戶」陣列名單時。
                            if (strUserID != strUserSessionID.ToString())
                            {
                                //將「舊有存在用戶」放入陣列項目裡面。
                                arrltTemp.Add(strUserID);
                                arrltIp.Add(strUserIP);
                                //arrltName.Add(strUserName);
                                //arrltTime.Add(strUserTime);
                                //累加計數值。(統計人數)
                                intNum += 1;
                            }
                        }
                    }
                }

                //將「新加入的用戶」放入「舊有存在用戶」陣列項目裡面。(此陣列位置為最後一筆)
                arrltTemp.Add(strUserSessionID);
                arrltIp.Add(ip);
                //將「線上所有人數」累加 1。
                Application.Set("TotalUsers_A", intNum + 1);
                //將之前清查的陣列項目值「目前用戶名單」放入 Application「線上用戶名單」值裡面。
                Application.Set("OnlineUser_A", arrltTemp);
                Application.Set("OnlineIp", arrltIp);
                //Application.Set("OnlineName", arrltName);
                //Application.Set("OnlineTime", arrltTime);
            }

            //指派目前進入用戶的「Session 個人變數 ID 和最後存取時間」Application 名稱全域變數值。(目前存取時間)
            Application.Set(strUserSessionID + "LastAccessTime_A", DateTime.Now.ToString());

            //== 檢查所有連線到此網頁之瀏覽器的最近存取時間，重新計算線上人數名單。

            //使用動態陣列讀取。(取得 Application 的「線上用戶名單」陣列)
            ArrayList arrltOnlineUserT = (ArrayList)Application["OnlineUser_A"];
            ArrayList arrltOnlineIp = (ArrayList)Application["OnlineIp"];
            //ArrayList arrltOnlineName = (ArrayList)Application["OnlineName"];
            //ArrayList arrltOnlineTime = (ArrayList)Application["OnlineTime"];
            //指派累加值為0。
            //宣告日期物件操作案例。
            intNum = 0;

            DateTime dtLastAccessTime = new DateTime();

            // //////////////////////////////////////////////////////////////////////////////////////
            //清除下拉式選單。                                                                     //
            //this.DropDownList_UserName.Items.Clear();                                            //
            //將目前線上人數數量，新增到下拉式選單的項目。                                         //
            //this.DropDownList_UserName.Items.Add("線上人數:" + Application["TotalUsers_A"]);     //
            /////////////////////////////////////////////////////////////////////////////////////////
            //DataTable dt = new DataTable();
            //dt.Columns.Add("ip");
            //dt.Columns.Add("user");
            //dt.Columns.Add("username");
            //Response.Write("<div class='row'><div class='col-md-12'><table style='border: 3px #cccccc solid;' cellpadding='10' border='1' ><tr><td>在線用戶</td><td>存取時間</td></tr>");
            //讀取「線上用戶名單」陣列項目。
            for (i = 0; i < arrltOnlineUserT.Count; i++)
            {

                //當 Application 全域變數「用戶最後存取時間」不是無值時。
                if (Application[arrltOnlineUserT[i].ToString() + "LastAccessTime_A"] != null)
                {
                    //取得 Application 全域變數的「用戶最後存取時間」。
                    dtLastAccessTime = DateTime.Parse(Application[arrltOnlineUserT[i].ToString() + "LastAccessTime_A"].ToString());
                    //宣告刻度間隔物件操作案例。(取得「目前時間」與「用戶最後存取時間」相差距的秒數)
                    TimeSpan tsTicks = new TimeSpan(dtDateTime.Ticks - dtLastAccessTime.Ticks);
                    //當相差距的秒數小於 30 秒時。

                    if (Convert.ToInt32(tsTicks.TotalSeconds) < intCheckTickSeconds)
                    {
                        //string ID = Session["userid"].ToString();
                        //顯示目前「線上用戶名單」與「用戶最後存取時間」與「目前時間」與「用戶最後存取時間」相差距的秒數。
                        //Response.Write("使用者帳號：" + arrltOnlineUserT[i].ToString() + "ip" + arrltOnlineIp[i].ToString());
                        ////將目前線上人數名單，新增到下拉式選單的項目。
                        //dt.Rows.Add();
                        //dt.Rows[i]["ip"] = arrltOnlineIp[i].ToString();
                        //dt.Rows[i]["user"] = arrltOnlineUserT[i].ToString();
                        //dt.Rows[i]["username"] = Application[arrltOnlineUserT[i].ToString() + "LastAccessTime_A"];

                        //string[] strField = { "ip", "user", "username" };
                        //ViewState["hasRows"] = FCommon.GridView_dataImport(GV_ONLINE, dt, strField);
                        //累加計數值。(統計人數)
                        intNum += 1;

                    }
                    else //當相差距的秒數大於 30 秒時。
                    {
                        //設定目前用戶的「用戶最後存取時間」Application 變數為無值。(清除 Application)
                        //Application.Set(arrltOnlineUserT[i].ToString() + "LastAccessTime_A", null);
                        //Application.Set(arrltOnlineUserT[i].ToString(), null);
                        //Application.Set(arrltOnlineIp[i].ToString(), null);
                        //Application.Remove(arrltOnlineUserT[i].ToString() + "LastAccessTime_A");
                        //Application.Remove(arrltOnlineUserT[i].ToString());
                        //Application.Remove(arrltOnlineIp[i].ToString());
                    }
                }
            }

            //String[] myIp = (String[])arrltOnlineIp.ToArray(typeof(string));
            //String[] myArr = (String[])arrltOnlineUserT.ToArray(typeof(string));

            ////String[] myArr = (String[])arrltOnlineUserT.ToArray(typeof(string));
            //var getdata =
            //    from table in myIp
            //    join tab in myArr
            //    on table.Count() equals tab.Count()
            //    select new
            //    {
            //        ip = table,
            //        userid = tab,
            //        username = tab
            //    };
            //DataTable datatable = CommonStatic.LinqQueryToDataTable(getdata);
            //string[] strField = { "ip", "userid", "username" };
            //ViewState["hasRows"] = FCommon.GridView_dataImport(GV_ONLINE, datatable, strField);
            //當上面所清查完成的「目前線上人數」與 Application「線上所有人數」不同就表示中間有人斷線。
            if (intNum != (int)Application["TotalUsers_A"])
            {
                //將陣列項目值「線上用戶名單」放入 Application["OnlineUser_A"] 全域陣列值裡面。
                Application.Set("OnlineUser_A", arrltOnlineUserT);
                Application.Set("OnlineIp", arrltOnlineIp);
                //Application.Set("OnlineName", arrltOnlineName);
                //Application.Set("OnlineTime", arrltOnlineTime);
                //將上面所清查的「線上人數」放入 Application["TotalUsers_A"] 全域變數裡面。
                Application.Set("TotalUsers_A", intNum);
            }

            //不鎖定全域變數存取。
            Application.UnLock();
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 登入session驗證
            if (Session["IsLogined"] != null)
            {
                if (Session["IsLogined"].ToString() == "Y")
                {  //之後會帶值'使用者'名稱
                    username.InnerText = Session["username"].ToString();

                }
                else if (Session["IsLogined"].ToString() == "Apply")
                {
                    Session["IsLogined"] = "N";
                }
                else
                {
                    Response.Redirect("~/login");
                }
            }
            else
            {
                Response.Redirect("~/login");
            }
            #endregion

            //if (Session["username"] != null)
            //{
            //    string Name = Session["logid"].ToString();
            //    ShowOnLineUserList_Fn(Name);
            //}
            if(Session["username"]!=null && Session["userid"] != null && Session["logid"] != null)
            {
                string Name = Session["username"].ToString();
                string id = Session["userid"].ToString();
                string logid = Session["logid"].ToString();
                Guid g = new Guid(logid);
                var getip = GME.USER_LOGIN.Where(ip => ip.AC_SN == g).Single();
                string logip = getip.IP_ADDRESS;
                string logtime = getip.LOGIN_TIME.ToString();
                ShowOnLineUserList_Fn(id, logip, Name, logtime);
                importMenu();
                setSystemMenu(null);
            }
        }

        /*protected void Page_Unload(object sender, EventArgs e)
        {
            string ID = Session["logid"].ToString();
            Guid g = new Guid(ID);
            var getlog = ae.USER_LOGIN.Where(sn => sn.AC_SN == g).FirstOrDefault();

            getlog.LOGOUT_TIME = DateTime.Now;
            ae.SaveChanges();

            Session.Clear();
        }*/

        protected void btnSys_Click(object sender, EventArgs e) //選單按鈕事件
        {
            LinkButton theButton = (LinkButton)sender;
            //Session["theSystem"] = theButton.ID.Substring(3);
            string strSystem = theButton.ID.Substring(3);
            setSystemMenu(strSystem);
        }

        protected void SiteMapPath1_ItemCreated(object sender, SiteMapNodeItemEventArgs e)
        {
            if (e.Item.ItemType == SiteMapNodeItemType.Root ||
                e.Item.ItemType == SiteMapNodeItemType.PathSeparator && e.Item.ItemIndex == 1)
            {
                e.Item.Visible = false;
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            if(Session["logid"]!= null)
            {
                string ID = Session["logid"].ToString();
                Guid g = new Guid(ID);
                var getlog = GME.USER_LOGIN.Where(sn => sn.AC_SN == g).FirstOrDefault();

                getlog.LOGOUT_TIME = DateTime.Now;
                GME.SaveChanges();
            }

            Session.Clear();
            MessageBoxShow("登入逾時，請重新登入", "login", true);
        }
    }
}