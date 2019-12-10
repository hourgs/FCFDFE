using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;
using FCFDFE.Content;
using System.Data;

namespace FCFDFE.pages.GM
{
    public partial class OnlineUserNumber : Page
    {
        Common FCommon = new Common();
        GMEntities gme = new GMEntities();
        public string strMenuName = "", strMenuNameItem = "";

        protected void Page_Load(object sender, EventArgs e)
        {


            string Name = Session["username"].ToString();
            string id = Session["userid"].ToString();
            string logid = Session["logid"].ToString();
            Guid g = new Guid(logid);
            var getip = gme.USER_LOGIN.Where(ip => ip.AC_SN == g).Single();
            string logip = getip.IP_ADDRESS;
            string logtime = getip.LOGIN_TIME.ToString();
            ShowOnLineUserList_Fn(id, logip);


            //GetData();
        }

        public void ShowOnLineUserList_Fn(string strUserName_Val, string strIP)
        {
            //宣告一維陣列清單會依照增加項目的多少而動態調整大小。
            ArrayList arrltTemp = new ArrayList();
            ArrayList arrltIp = new ArrayList();
            //宣告字串變數。(用戶識別碼)
            string strUserID = null;
            string strUserIP = null;
            //宣告字串變數。(取得進入頁面後系統給予的 Session 唯一識別碼)
            string strUserSessionID = strUserName_Val;
            string ip = strIP;
            int onlinenum = Convert.ToInt32(Session["OnlinNumber"]);
            //宣告整數變數。(人數計數器)
            int intNum = 0;
            //宣告整數變數。(迴圈計數器)
            int i = 0;
            //宣告整數變數。(距離多少時間(秒)的「用戶最後存取時間」才算是離開)
            int intCheckTickSeconds = 30;
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
                            //當「舊有存在用戶」不等於「新加入用戶」陣列名單時。
                            if (strUserID != strUserSessionID.ToString())
                            {
                                //將「舊有存在用戶」放入陣列項目裡面。
                                arrltTemp.Add(strUserID);
                                arrltIp.Add(strUserIP);
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
            }

            //指派目前進入用戶的「Session 個人變數 ID 和最後存取時間」Application 名稱全域變數值。(目前存取時間)
            Application.Set(strUserSessionID + "LastAccessTime_A", DateTime.Now.ToString());

            ArrayList arrltOnlineUserT = (ArrayList)Application["OnlineUser_A"];
            ArrayList arrltOnlineIp = (ArrayList)Application["OnlineIp"];
            intNum = 0;

            DateTime dtLastAccessTime = new DateTime();

            //讀取「線上用戶名單」陣列項目。

            for (i = 0; i < arrltOnlineUserT.Count; i++)
            {
                //當 Application 全域變數「用戶最後存取時間」不是無值時。
                if (Application[arrltOnlineUserT[i].ToString() + "LastAccessTime_A"] != null )
                {
                    //取得 Application 全域變數的「用戶最後存取時間」。
                    dtLastAccessTime = DateTime.Parse(Application[arrltOnlineUserT[i].ToString() + "LastAccessTime_A"].ToString());
                    //宣告刻度間隔物件操作案例。(取得「目前時間」與「用戶最後存取時間」相差距的秒數)
                    TimeSpan tsTicks = new TimeSpan(dtDateTime.Ticks - dtLastAccessTime.Ticks);

                    if (Convert.ToInt32(tsTicks.TotalSeconds) < intCheckTickSeconds)
                    {
                        intNum += 1;
                    }
                    else //當相差距的秒數大於 30 秒時。
                    {
                        Application.Set(arrltOnlineUserT[i].ToString() + "LastAccessTime_A", null);
                        Application.Set(arrltOnlineUserT[i].ToString(), null);
                        Application.Set(arrltOnlineIp[i].ToString(), null);
                        Application.Remove(arrltOnlineUserT[i].ToString() + "LastAccessTime_A");
                        Application.Remove(arrltOnlineUserT[i].ToString());
                        Application.Remove(arrltOnlineIp[i].ToString());
                        arrltOnlineIp.Remove(arrltOnlineIp[i]);
                        arrltOnlineUserT.Remove(arrltOnlineUserT[i]);
                    }
                }
            }
            DataTable dt = new DataTable();
            
            dt.Columns.Add(new DataColumn("ip", typeof(String)));
            dt.Columns.Add(new DataColumn("user", typeof(String)));
            dt.Columns.Add(new DataColumn("time", typeof(String)));
            for (i = 0; i<arrltOnlineUserT.Count; i++)
            {
                dt.Rows.Add();
                dt.Rows[i]["ip"] = arrltOnlineIp[i].ToString();
                dt.Rows[i]["user"] = arrltOnlineUserT[i].ToString();
                dt.Rows[i]["time"] = Application[arrltOnlineUserT[i].ToString() + "LastAccessTime_A"];

                
                ViewState["hasRows"] = FCommon.GridView_dataImport(GV_ONLINE, dt);
            }

            if (intNum != (int)Application["TotalUsers_A"])
            {
                //將陣列項目值「線上用戶名單」放入 Application["OnlineUser_A"] 全域陣列值裡面。
                Application.Set("OnlineUser_A", arrltOnlineUserT);
                Application.Set("OnlineIp", arrltOnlineIp);
                Application.Set("TotalUsers_A", intNum);
            }
            //不鎖定全域變數存取。
            Application.UnLock();

        
        }
        protected void GV_ONLINE_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
            {
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
                FCommon.GridView_PreRenderInit(sender, hasRows);
            }

        }
    }
}