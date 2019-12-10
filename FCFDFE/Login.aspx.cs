using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Entity.GMModel;
using System.Net;
using System.Net.Mail;
using FCFDFE.Content;


namespace FCFDFE
{
    public partial class login : System.Web.UI.Page
    {
        //private static readonly ILog TxtLog = LogManager.GetLogger(typeof(_Default));//取得logger物件
        private GMEntities gm = new GMEntities();
        Common FCommon = new Common();
        ACCOUNT account = new ACCOUNT();
        //寄信之該信箱需設定stmp協定，寄信功能才可使用
        protected void Page_Load(object sender, EventArgs e)
        {
            MSG.Text = "";
            //XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("log4net.config")));
            string IP= Request.ServerVariables["REMOTE_ADDR"].ToString();
            //檢查是否為黑名單IP
            //if (BlackList_Check(IP) == false)
            //    MSG.Text = "<script>alert('您的IP位置被拒絕造訪本系統，若有疑問請聯繫採購室資訊官！');location.href='https://tw.yahoo.com/';</script>";
            //else if(TryLogin_Check(IP) == false)
            //    MSG.Text = "<script>alert('系統偵測出您的IP位置嘗試進行暴力登入，故拒絕該IP造訪本系統。若有疑問請聯繫採購室資訊官！');location.href='https://tw.yahoo.com/';</script>";
            //檢查是否有短時間內多次登入次數
        }
        protected void btnlogin_Click1(object sender, EventArgs e)
        {
            string ID = LoginName.Text;
            #region mpcmsystem帳號驗證

            if (ID == "mpcmsystem")
            {
                var queryPW =
                    from tbm1407 in gm.TBM1407
                    where tbm1407.OVC_PHR_CATE == "SP"
                    where tbm1407.OVC_PHR_ID == "03"
                    where tbm1407.OVC_PHR_DESC == LoginPass.Text
                    select tbm1407;
                if (queryPW.Any())
                {
                    Session["Superadmin"] = "Y";
                    Response.Redirect("~/pages/Sup/Super_User_Track");
                }
                else
                    Response.Write("<script>alert('密碼錯誤！')</script>");
            }
            #endregion

            var getaccount = gm.ACCOUNTs.Where(id => id.USER_ID == ID).FirstOrDefault();

            if (getaccount != null)
            {
                var getpwd = gm.ACCOUNTs.Where(id => id.USER_ID == ID).Single();
                var getlog = gm.USER_LOGIN.Where(sn => sn.AC_ID == ID).FirstOrDefault();
                if (LoginPass.Text == getpwd.PWD && getpwd.ACCOUNT_STATUS == 1)
                {
                    USER_LOGIN log = new USER_LOGIN();
                    log.AC_ID = getpwd.USER_ID;
                    log.AC_SN = Guid.NewGuid();
                    log.LOGIN_TIME = DateTime.Now;
                    log.FAILURE_TIMES = 0;
                    log.IS_SUCCESS = "Y";
                    log.IP_ADDRESS = Request.ServerVariables["REMOTE_ADDR"].ToString();
                    gm.USER_LOGIN.Add(log);
                    gm.SaveChanges();


                    Session["IsLogined"] = "Y";
                    Session["username"] = getpwd.USER_NAME;
                    Session["userid"] = getpwd.USER_ID;
                    Session["logid"] = log.AC_SN;
                    //20181108_新增紀錄單位代號By敬國
                    Session["dept"] = getaccount.DEPT_SN;
                    getpwd.LAST_LOGIN = DateTime.Now;
                    getpwd.ERROR_CNT = 0;
                    gm.SaveChanges();
                    Response.Redirect("~/pages/GM/BulletinBoard");
                }
                else
                {
                    getpwd.ERROR_CNT += 1;
                    USER_LOGIN log = new USER_LOGIN();
                    if (getpwd.ERROR_CNT >= 3)
                    {
                        getpwd.ACCOUNT_STATUS = 0;
                    }
                    log.AC_ID = getpwd.USER_ID;
                    log.AC_SN = Guid.NewGuid();
                    log.LOGIN_TIME = DateTime.Now;
                    log.FAILURE_TIMES = getpwd.ERROR_CNT;
                    log.IS_SUCCESS = "N";
                    log.IP_ADDRESS = Request.ServerVariables["REMOTE_ADDR"].ToString();
                    gm.USER_LOGIN.Add(log);
                    gm.SaveChanges();

                    if (getpwd.ERROR_CNT >= 3)
                    {
                        string erroraccount = getpwd.USER_ID;
                        string errorcnt = getpwd.ERROR_CNT.ToString();
                        string errormessage = "帳號" + erroraccount + "已登入失敗" + errorcnt + "次! ip位置:" + Request.ServerVariables["REMOTE_ADDR"].ToString();
                        Response.Write("<script>alert('您的帳號登入失敗已超過3次！')</script>");

                    }
                    else if (getpwd.ACCOUNT_STATUS != 1)
                    {
                        Response.Write("<script>alert('您的帳號目前沒有權限！')</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('帳號或密碼錯誤！')</script>");
                    }

                    Session["IsLogined"] = "N";
                }
            }
            else
            {
                USER_LOGIN log = new USER_LOGIN();
                log.AC_ID = ID;
                log.AC_SN = Guid.NewGuid();
                log.LOGIN_TIME = DateTime.Now;
                log.FAILURE_TIMES = 1;
                log.IS_SUCCESS = "N";
                log.IP_ADDRESS = Request.ServerVariables["REMOTE_ADDR"].ToString();
                gm.USER_LOGIN.Add(log);
                gm.SaveChanges();

                Response.Write("<script>alert('帳號或密碼錯誤！')</script>");
                Session["IsLogined"] = "N";
            }
                
        }


        #region 忘記密碼
        protected void btnforgotpass_Click(object sender, EventArgs e)
        {
            string userid = LoginName.Text;
            var queryAcc = gm.ACCOUNTs
                .Where(o => o.USER_ID.Equals(userid)).FirstOrDefault();
            if (queryAcc != null)
            {
                if (queryAcc.EMAIL_ACCOUNT != null)
                    try
                    {
                        FCommon.contactMail(queryAcc.EMAIL_ACCOUNT, queryAcc.USER_NAME, "國軍採購資訊系統(忘記密碼)", "帳號：" + queryAcc.USER_ID + "  密碼：" + queryAcc.PWD);
                    }
                    catch
                    {
                        Response.Write("<script>alert('信件寄出失敗！')</script>");
                        throw;
                    }
                    finally
                    {
                        Response.Write("<script>alert('密碼已寄發至申請之信箱！')</script>");
                    }
                else
                    Response.Write("<script>alert('沒有信箱地址無法寄發帳號密碼！')</script>");
            }
            else
            {
                Response.Write("<script>alert('輸入錯誤的帳號！')</script>");
            }
        }
        //private void contactMail(string xFrom, string xName, string xSubject, string xMessage)
        //{
        //    MailMessage xmail = new MailMessage();  //信件本體宣告
        //    xFrom = Server.HtmlEncode(xFrom);  // 對使用者輸入都先做 HtmlEncode，避免隱碼攻擊
        //    xName = Server.HtmlEncode(xName); 
        //    xSubject = Server.HtmlEncode(xSubject);
        //    xmail.From = new MailAddress("寄信人ADDRESS", "寄信人NAME");
        //    xmail.To.Add(new MailAddress(xFrom, xName));
        //    //xmail.CC.Add(new MailAddress("", ""));  //其它收件人
        //    //xmail.Bcc.Add(new MailAddress("", "")); //密本收件人
        //    xmail.Priority = MailPriority.Normal;  //優先等級
        //    xmail.Subject = xSubject;  //主旨
        //    xmail.Body = Server.HtmlEncode(xMessage);  //Email 內容
        //    xmail.IsBodyHtml = true;  // 設定Email 內容為HTML格式
        //    //xmail.BodyEncoding = System.Text.Encoding.GetEncoding(950);     
        //    //設定編碼為BIG 5
        //    xmail.BodyEncoding = System.Text.Encoding.UTF8;
        //    //xmail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");   
        //    //設定編碼為 utf-8
        //    // 設定SMTP伺服器
        //    SmtpClient smtpServer = new SmtpClient();
        //    smtpServer.Credentials = new System.Net.NetworkCredential("寄信人ADDRESS", "寄信人PASSWORD");
        //    smtpServer.Port = 25;  // smtpServer port = 25
        //    smtpServer.Host = "smtp.gmail.com";
        //    smtpServer.EnableSsl = true;  // SSL 要看你的 mail server，像 gmail 就必須啟用
        //    try
        //    {
        //        smtpServer.Send(xmail);
        //    }
        //    catch (Exception)
        //    {
        //        Response.Write("<script>alert('信件寄出失敗！')</script>");
        //        throw;
        //    }
        //    finally
        //    {
        //        Response.Write("<script>alert('帳號密碼已寄發至申請之信箱！')</script>");
        //    }
        //}
        #endregion

        #region 黑名單檢測
        //private bool BlackList_Check(string IP)
        //{
        //    bool result = true;
        //    string userIP = IP;
        //    var query =
        //        from BL in gm.BLACKLISTs
        //        select new
        //        {
        //            IP=BL.BL_IP
        //        };
        //   foreach (var item in query)
        //    {
        //        if (userIP.Contains(item.IP))
        //        {
        //            result = false;
        //            break;
        //        }
        //    }                
        //    return result;
        //}

        #endregion

        #region 暴力登入次數檢測
        //private bool TryLogin_Check(string IP)
        //{
        //    bool result = true;
        //    string UserIP = IP;
        //    DateTime strdate = Convert.ToDateTime(DateTime.Now.AddSeconds(-1));
        //    DateTime enddate = Convert.ToDateTime(DateTime.Now);
        //    var queryErr =
        //        from tbm1407 in gm.TBM1407
        //        where tbm1407.OVC_PHR_CATE == "SP"
        //        where tbm1407.OVC_PHR_ID == "01"
        //        select new
        //        {
        //            DESC = tbm1407.OVC_PHR_DESC
        //        };
        //    int MaxError = Convert.ToInt16(queryErr.FirstOrDefault().DESC);     
        //    var query =
        //        from UL in gm.USER_LOGIN.AsEnumerable()
        //        where UL.IS_SUCCESS=="N"
        //        where UL.IP_ADDRESS==UserIP
        //        select new
        //        {
        //            IP=UL.IP_ADDRESS,
        //            TIME=UL.LOGIN_TIME==null?"": UL.LOGIN_TIME.ToString()
        //        };
        //    DateTime dateDATE;
        //    query = query.Where(table => DateTime.TryParse(table.TIME, out dateDATE) &&
        //                   DateTime.Compare(dateDATE, strdate) >= 0 &&
        //                   DateTime.Compare(dateDATE, enddate) <= 0);
        //    if (query.Any())
        //        if (query.Count() > MaxError)
        //        {
        //            BLACKLIST bLACKLIST = new BLACKLIST();
        //            bLACKLIST.BL_SN = Guid.NewGuid();
        //            bLACKLIST.BL_IP = UserIP;
        //            bLACKLIST.BL_CREATEDATE = DateTime.Now;
        //            bLACKLIST.BL_REASON = "錯誤登入次數超過1秒"+MaxError+"次，故將IP封鎖";
        //            bLACKLIST.BL_STATUS = 1;
        //            gm.BLACKLISTs.Add(bLACKLIST);

        //            USER_LOGIN_ERRTRY ULE = new USER_LOGIN_ERRTRY();
        //            ULE.ULE_SN= Guid.NewGuid();
        //            ULE.ULE_IP = UserIP;
        //            ULE.ULE_CREATEDATE = DateTime.Now;
        //            ULE.ULE_REASON= "錯誤登入次數超過1秒" + MaxError + "次";
        //            gm.USER_LOGIN_ERRTRY.Add(ULE);
        //            gm.SaveChanges();
        //            result = false;
        //        }
        //    return result;
        //}

        #endregion
    }
}
