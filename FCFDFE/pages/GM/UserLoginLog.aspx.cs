using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.GM
{
    public partial class UserLoginLog : Page
    {
        bool hasRows = false;
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        GMEntities GME = new GMEntities();
        string[] field = { "USER_NAME", "LOGIN_TIME", "LOGOUT_TIME", "IP_ADDRESS" }; 
        protected void Page_Load(object sender, EventArgs e)
        {
            //string Name = Session["USER_NAME"].ToString();
            //ShowOnLineUserList_Fn(Name);
            FCommon.Controls_Attributes("readonly", "true", txtDateStart, txtDateEnd);
            if (!IsPostBack)
            {
                LoginQuery();
            }
        }

        private void LoginQuery()
        {
            DateTime dtNow = DateTime.Now;
            txtDateStart.Text = FCommon.getDateTime(dtNow);
            txtDateEnd.Text = FCommon.getDateTime(dtNow);
            DateTime dateStart = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 00, 00, 00);
            DateTime dateEnd = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 23, 59, 59);
            var query =
                from t in GME.USER_LOGIN
                join t2 in GME.ACCOUNTs on t.AC_ID equals t2.USER_ID
                where t.LOGIN_TIME >= dateStart && t.LOGIN_TIME <= dateEnd
                select new
                {
                    t2.USER_NAME,
                    t.LOGIN_TIME,
                    t.LOGOUT_TIME,
                    t.IP_ADDRESS
                };
            DataTable dt = CommonStatic.LinqQueryToDataTable(query);
            hasRows = FCommon.GridView_dataImport(GV_ACCOUNT, dt, field);
        }

        public void ShowOnLineUserList_Fn(string strUserName_Val)
        {
            //宣告一維陣列清單會依照增加項目的多少而動態調整大小。
            ArrayList arrltTemp = new ArrayList();
            //宣告字串變數。(用戶識別碼)
            string strUserID = null;
            //宣告字串變數。(取得進入頁面後系統給予的 Session 唯一識別碼)
            string strUserSessionID = strUserName_Val;
            //宣告整數變數。(人數計數器)
            int intNum = 0;
            //宣告整數變數。(迴圈計數器)
            int i = 0;

            //新增此 Session 變數以使 Session 物件能正常運作。
            Session["Flag"] = true;
            //宣告日期物件變數。(取得目前時間)
            DateTime dtDateTime = DateTime.Now;

            //清空變數值。(測試用)
            //Application.Clear();

            //鎖定全域變數存取。
            Application.Lock();

            //@操作說明：清點所有連線到此網頁的用戶名單，並判斷是否為「新加入用戶」，
            //如果是就將目前的「新加入用戶」名稱放入「舊有存在用戶」陣列的最後一筆。

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
                            //當「舊有存在用戶」不等於「新加入用戶」陣列名單時。
                            if (strUserID != strUserSessionID.ToString())
                            {
                                //將「舊有存在用戶」放入陣列項目裡面。
                                arrltTemp.Add(strUserID);
                                //累加計數值。(統計人數)
                                intNum += 1;
                            }
                        }
                    }
                }

                //將「新加入的用戶」放入「舊有存在用戶」陣列項目裡面。(此陣列位置為最後一筆)
                arrltTemp.Add(strUserSessionID);
                //將「線上所有人數」累加 1。
                Application.Set("TotalUsers_A", intNum + 1);
                //將之前清查的陣列項目值「目前用戶名單」放入 Application「線上用戶名單」值裡面。
                Application.Set("OnlineUser_A", arrltTemp);
            }

            //指派目前進入用戶的「Session 個人變數 ID 和最後存取時間」Application 名稱全域變數值。(目前存取時間)
            Application.Set(strUserSessionID + "LastAccessTime_A", DateTime.Now.ToString());

            //== 檢查所有連線到此網頁之瀏覽器的最近存取時間，重新計算線上人數名單。

            //使用動態陣列讀取。(取得 Application 的「線上用戶名單」陣列)
            ArrayList arrltOnlineUserT = (ArrayList)Application["OnlineUser_A"];
            //指派累加值為0。
            intNum = 0;

            //宣告日期物件操作案例。
            DateTime dtLastAccessTime = new DateTime();

            // //////////////////////////////////////////////////////////////////////////////////////
            //清除下拉式選單。                                                                     //
            //this.DropDownList_UserName.Items.Clear();                                            //
            //將目前線上人數數量，新增到下拉式選單的項目。                                         //
            //this.DropDownList_UserName.Items.Add("線上人數:" + Application["TotalUsers_A"]);     //
            /////////////////////////////////////////////////////////////////////////////////////////

            //Response.Write("<div class='row'><div class='col-md-12'><table style='border: 3px #cccccc solid;' cellpadding='10' border='1' ><tr><td>在線用戶</td><td>存取時間</td></tr>");
            //讀取「線上用戶名單」陣列項目。
            for (i = 0; i < arrltOnlineUserT.Count; i++)
            {
                //當 Application 全域變數「用戶最後存取時間」不是無值時。
                if (Application[arrltOnlineUserT[i].ToString() + "LastAccessTime_A"] != null)
                {
                    //取得 Application 全域變數的「用戶最後存取時間」。
                    dtLastAccessTime = DateTime.Parse(Application[arrltOnlineUserT[i].ToString() + "LastAccessTime_A"].ToString());

                    //////////////////////////////////////////////////////////////////////////////////
                    //宣告刻度間隔物件操作案例。(取得「目前時間」與「用戶最後存取時間」相差距的秒數)//
                    //TimeSpan tsTicks = new TimeSpan(dtDateTime.Ticks - dtLastAccessTime.Ticks);   //
                    //////////////////////////////////////////////////////////////////////////////////

                    //當使用者有登入的時候。
                    if (Session["IsLogined"].ToString() == "Y")
                    {
                        //顯示目前「線上用戶名單」與「用戶最後存取時間」與「目前時間」與「用戶最後存取時間」相差距的秒數。

                        //Response.Write("<tr><td>" + arrltOnlineUserT[i].ToString() + " </td><td>" + Application[arrltOnlineUserT[i].ToString() + "LastAccessTime_A"] + "</td></tr>");
                        //<td> " + Convert.ToInt32(tsTicks.TotalSeconds).ToString() + "</td>

                        //將目前線上人數名單，新增到下拉式選單的項目。
                        //this.DropDownList_UserName.Items.Add(arrltOnlineUserT[i].ToString());

                        //累加計數值。(統計人數)
                        intNum += 1;
                    }
                    else //當使用者登出的時候。
                    {
                        //設定目前用戶的「用戶最後存取時間」Application 變數為無值。(清除 Application)
                        Application.Set(arrltOnlineUserT[i].ToString() + "LastAccessTime_A", null);
                        Application.Set(arrltOnlineUserT[i].ToString(), null);
                        Application.Remove(arrltOnlineUserT[i].ToString() + "LastAccessTime_A");
                        Application.Remove(arrltOnlineUserT[i].ToString());
                    }
                }
            }
            //Response.Write("</table></div></div>");
            //當上面所清查完成的「目前線上人數」與 Application「線上所有人數」不同就表示中間有人斷線。
            if (intNum != (int)Application["TotalUsers_A"])
            {
                //將陣列項目值「線上用戶名單」放入 Application["OnlineUser_A"] 全域陣列值裡面。
                Application.Set("OnlineUser_A", arrltOnlineUserT);
                //將上面所清查的「線上人數」放入 Application["TotalUsers_A"] 全域變數裡面。
                Application.Set("TotalUsers_A", intNum);
            }

            //不鎖定全域變數存取。
            Application.UnLock();
        }

        protected void btnLoginDate_Click(object sender, EventArgs e)
        {
            
            DateTime dateStart;
            DateTime dateEnd;
            if (string.IsNullOrEmpty(txtDateStart.Text) || string.IsNullOrEmpty(txtDateEnd.Text))
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先 選擇日期");
            else
            {
                dateStart = Convert.ToDateTime(txtDateStart.Text);
                dateEnd = Convert.ToDateTime(txtDateEnd.Text);
                if (dateStart > dateEnd)
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "結束日期大於開始日期");
                else
                {
                    dateStart = Convert.ToDateTime(txtDateStart.Text + " 00:00:00");
                    dateEnd = Convert.ToDateTime(txtDateEnd.Text + " 23:59:59");
                    var query =
                        from t in GME.USER_LOGIN
                        join t2 in GME.ACCOUNTs on t.AC_ID equals t2.USER_ID
                        where t.LOGIN_TIME >= dateStart && t.LOGIN_TIME <= dateEnd
                        select new
                        {
                            t2.USER_NAME,
                            t.LOGIN_TIME,
                            t.LOGOUT_TIME,
                            t.IP_ADDRESS
                        };
                    DataTable dt = CommonStatic.LinqQueryToDataTable(query);
                    hasRows = FCommon.GridView_dataImport(GV_ACCOUNT, dt, field);
                }
            }
        }

        protected void GV_ACCOUNT_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {

                GV_ACCOUNT.UseAccessibleHeader = true;
                GV_ACCOUNT.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
    }
}