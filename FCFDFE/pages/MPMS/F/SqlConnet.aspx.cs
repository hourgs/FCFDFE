using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Configuration;
using System.Data.SqlClient;

namespace FCFDFE.pages.MPMS.F
{
    public partial class SqlConnet : Page
    {
        string connnn = ConfigurationManager.ConnectionStrings["SQLConnetString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (user.getRole_Ikind  ().equals("31"))
            //{ //角色為 計評主辦單位

            //    if ((qry_Purch != null) && (!qry_Purch.equals("")))
            //    { //有輸入購案編號
            //        sql = " select a.OVC_PUR_AGENCY, a.OVC_PUR_IPURCH,a.OVC_PUR_NSECTION,a.OVC_PUR_USER," +
            //            " a.OVC_PUR_IUSER_PHONE_EXT, a.OVC_DPROPOSE,a.OVC_PROPOSE, a.OVC_PUR_APPROVE," +
            //            " a.OVC_PUR_DAPPROVE,a.ovc_Pur_Dcanpo,b.OVC_PURCH, b.OVC_DRECEIVE, b.OVC_CHECK_UNIT, " +
            //            " b.ONB_CHECK_TIMES,b.OVC_DAUDIT_ASSIGN, b.OVC_AUDIT_UNIT,b.OVC_AUDIT_ASSIGNER,b.OVC_DAUDIT," +
            //            " b.OVC_CHECKER OVC_AUDITOR, b.OVC_DRESULT  ";
            //        sql = sql + " from TBM1301 a," +
            //               " (select a.OVC_PURCH,a.OVC_CHECK_UNIT,a.ONB_CHECK_TIMES,b.OVC_DRECEIVE," +
            //               " b.OVC_DAUDIT_ASSIGN,b.OVC_AUDIT_UNIT,b.OVC_AUDIT_ASSIGNER," +
            //               " b.OVC_DAUDIT,a.OVC_CHECKER,a.OVC_DRESULT  " +
            //               " from TBM1202 a ,tbm1202_1 b where a.ovc_purch = b.ovc_purch(+) " +
            //               " and a.OVC_DRECEIVE = b.OVC_DRECEIVE(+) " +
            //               " and a.OVC_PURCH like '" + modovc_purch + "%'" + //限制案號
            //               " and a.OVC_CHECK_UNIT = b.OVC_CHECK_UNIT(+) " +
            //               " and a.OVC_CHECKER = b.OVC_AUDITOR(+) ";
            //        if (user.getUser_Id().equals("31ASSIGNER"))
            //        {
            //            sql = sql + " and (a.OVC_CHECK_UNIT = '00N00' or a.OVC_CHECK_UNIT = '0A100' ) ";  //採購室也可查到採購中心案
            //        }
            //        else
            //        {
            //            if (user.getOvc_Pur_Section().equals("00N00") || user.getOvc_Pur_Section().equals("0A100"))
            //            {
            //                sql = sql + " and (a.OVC_CHECK_UNIT = '00N00' or a.OVC_CHECK_UNIT = '0A100' ) ";  //採購室也可查到採購中心案 
            //            }
            //            else
            //            {
            //                sql = sql + " and a.OVC_CHECK_UNIT = '" + user.getOvc_Pur_Section() + "' ";  //主辦單位與登入者單位相同
            //            }
            //        }

            //        sql = sql + " and a.ONB_CHECK_TIMES = b.ONB_CHECK_TIMES(+)   ) b, " +
            //               " TBM1301_PLAN d ";
            //        sql = sql + " where a.OVC_PURCH = b.OVC_PURCH " +
            //               "   and a.OVC_PURCH = d.OVC_PURCH ";
            //        if (user.getUser_Id().equals("31ASSIGNER"))
            //        {
            //            sql = sql + "    and (d.OVC_AUDIT_UNIT = '0A100' or d.OVC_AUDIT_UNIT = '00N00' " + //計評單位=登錄者單位
            //                "         or d.OVC_PUR_APPROVE_DEP = 'A') "; //讀取採購中心權責購案(部核案)
            //        }
            //        else
            //        {
            //            if (user.getOvc_Pur_Section().equals("00N00") || user.getOvc_Pur_Section().equals("0A100"))
            //            {
            //                sql = sql + "    and (d.OVC_AUDIT_UNIT = '0A100' or d.OVC_AUDIT_UNIT = '00N00' " + //計評單位=登錄者單位
            //                      "         or d.OVC_PUR_APPROVE_DEP = 'A') "; //讀取採購中心權責購案(部核案)
            //            }
            //            else
            //            {
            //                sql = sql + "    and d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' ";  //計評單位=登錄者單位
            //            }
            //        }
            //        sql = sql + "   order by b.OVC_PURCH, b.ONB_CHECK_TIMES ";
            //    }
            //    else
            //    {//未輸入購案編號
            //        sql = " select a.OVC_PUR_AGENCY, a.OVC_PUR_IPURCH,a.OVC_PUR_NSECTION,a.OVC_PUR_USER," +
            //            " a.OVC_PUR_IUSER_PHONE_EXT, a.OVC_DPROPOSE,a.OVC_PROPOSE, a.OVC_PUR_APPROVE," +
            //            " a.OVC_PUR_DAPPROVE,a.ovc_Pur_Dcanpo,b.OVC_PURCH, b.OVC_DRECEIVE, b.OVC_CHECK_UNIT, " +
            //            " b.ONB_CHECK_TIMES,b.OVC_DAUDIT_ASSIGN, b.OVC_AUDIT_UNIT,b.OVC_AUDIT_ASSIGNER,b.OVC_DAUDIT," +
            //            " b.OVC_CHECKER OVC_AUDITOR, b.OVC_DRESULT  " +
            //               " from TBM1301 a," +
            //               " (select a.OVC_PURCH,a.OVC_CHECK_UNIT,a.ONB_CHECK_TIMES,b.OVC_DRECEIVE," +
            //               " b.OVC_DAUDIT_ASSIGN,b.OVC_AUDIT_UNIT,b.OVC_AUDIT_ASSIGNER," +
            //               " b.OVC_DAUDIT,a.OVC_CHECKER,a.OVC_DRESULT  " +
            //               " from TBM1202 a ,tbm1202_1 b where a.ovc_purch = b.ovc_purch(+) " +
            //               " and a.OVC_DRECEIVE = b.OVC_DRECEIVE(+) " +
            //               ((ovc_Name != null) && (!ovc_Name.equals("none")) ? (ovc_Name.equals("All") ? "" : "and a.OVC_CHECKER = '" + ovc_Name + "' ") : " and (a.OVC_CHECKER is null or a.OVC_CHECKER = ' ') ") +//限制某承辦人                   
            //               " and a.OVC_CHECK_UNIT = b.OVC_CHECK_UNIT(+) " +
            //               " and a.OVC_CHECKER = b.OVC_AUDITOR(+) ";
            //        if (!qry_YY.equals("all"))
            //        {
            //            sql = sql + " and substr(a.OVC_PURCH,3,2) = '" + qry_YY + "' ";  //限制年度
            //        }
            //        if (user.getUser_Id().equals("31ASSIGNER"))
            //        {
            //            sql = sql + " and (a.OVC_CHECK_UNIT = '00N00' or a.OVC_CHECK_UNIT = '0A100') ";  //採購室也可查到採購中心
            //        }
            //        else
            //        {
            //            if (user.getOvc_Pur_Section().equals("00N00"))
            //            {
            //                sql = sql + " and (a.OVC_CHECK_UNIT = '00N00' or a.OVC_CHECK_UNIT = '0A100') ";  //採購室也可查到採購中心
            //            }
            //            else
            //            {
            //                sql = sql + " and a.OVC_CHECK_UNIT = '" + user.getOvc_Pur_Section() + "' ";  //主辦單位與登入者單位相同 
            //            }
            //        }
            //        sql = sql + " and a.ONB_CHECK_TIMES = b.ONB_CHECK_TIMES(+)   ) b, " +
            //               " TBM1301_PLAN d " +
            //               " where a.OVC_PURCH = b.OVC_PURCH " +
            //               "   and a.OVC_PURCH = d.OVC_PURCH ";
            //        if (user.getUser_Id().equals("31ASSIGNER"))
            //        {
            //            sql = sql + "    and ( d.OVC_AUDIT_UNIT = '00N00' or d.OVC_AUDIT_UNIT = '0A100' " + //計評單位=登錄者單位
            //                  "         or d.OVC_PUR_APPROVE_DEP = 'A') "; //讀取採購中心權責購案(部核案)
            //        }
            //        else
            //        {
            //            if (user.getOvc_Pur_Section().equals("00N00"))
            //            {
            //                sql = sql + "    and ( d.OVC_AUDIT_UNIT = '00N00' or d.OVC_AUDIT_UNIT = '0A100' " + //計評單位=登錄者單位
            //                    "         or d.OVC_PUR_APPROVE_DEP = 'A') "; //讀取採購中心權責購案(部核案)
            //            }
            //            else
            //            {
            //                sql = sql + "    and d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' ";  //計評單位=登錄者單位
            //            }
            //        }
            //        //承辦人 權限
            //        if ((qry_Purch != null) && (!qry_Purch.equals("")))
            //        {
            //            //指定某購案        
            //            sql = sql + " and a.OVC_PURCH like '" + modovc_purch + "%'";
            //        }
            //        else
            //        {
            //            if ((qry_YY != null) && (!qry_YY.equals("")) && (!qry_YY.equals("all")))
            //            {
            //                //限制年度        
            //                sql = sql + " and substr(a.OVC_PURCH,3,2) = '" + qry_YY + "'";
            //            }
            //        }
            //        if (vcase != null && vcase.equals("notApprove"))
            //        {
            //            if (user.getOvc_Pur_Section().equals("0A100"))
            //            {//採購中心核定文號為【備採綜 ...】  
            //                sql = sql + " and (a.ovc_Pur_Dapprove is null " + //尚未核定
            //                          " or (a.ovc_Pur_Dapprove is not null and substr(trim(nvl(a.ovc_pur_approve,' ')),1,2) not in ('備採','昇軸') )) " + //有核定日期但非採購中心核定
            //                          " and ( d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' " + //計評單位=登錄者單位      
            //                    "         or d.OVC_PUR_APPROVE_DEP = 'A') "; //讀取採購中心權責購案(部核案)
            //            }
            //            else
            //            {
            //                sql = sql + " and a.ovc_Pur_Dapprove is null " + //尚未核定
            //                          " and d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' ";  //計評單位=登錄者單位 
            //            }
            //            sql = sql + " and (a.OVC_PUR_DCANPO is null or a.OVC_PUR_DCANPO = '') " +  //未撤案
            //                      " and (d.OVC_DCANCEL is null or d.OVC_DCANCEL = '') ";
            //        }
            //        else if (vcase != null && vcase.equals("Approve"))
            //        {
            //            if (user.getOvc_Pur_Section().equals("0A100"))
            //            { //採購中心核定文號為【備採綜 ...】 
            //                sql = sql + " and (a.ovc_Pur_Dapprove is not null and substr(trim(nvl(a.ovc_pur_approve,' ')),1,2) in ('備採','昇軸') ) " + //採購中心核定
            //                          " and ( d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' " + //計評單位=登錄者單位      
            //                          " or d.OVC_PUR_APPROVE_DEP = 'A') "; //讀取採購中心權責購案(部核案)
            //            }
            //            else
            //            {
            //                sql = sql + " and a.ovc_Pur_Dapprove is not null " + //已核定
            //                          "    and d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' ";  //計評單位=登錄者單位 
            //            }
            //            sql = sql + " and (a.OVC_PUR_DCANPO is null or a.OVC_PUR_DCANPO = '') " +//未撤案
            //                      " and (d.OVC_DCANCEL is null or d.OVC_DCANCEL = '') ";
            //        }
            //        else if (vcase != null && vcase.equals("Cancel"))
            //        {
            //            sql = sql + " and ((a.OVC_PUR_DCANPO is not null) " + //已撤案;
            //                      " or (d.OVC_DCANCEL is not null)) ";
            //        }
            //        sql = sql + "   order by b.OVC_PURCH, b.ONB_CHECK_TIMES ";
            //    }
            //}
            //else
            //{ // !user.getRole_Ikind().equals("31") //非計評主辦單位
            //    sql = "select a.OVC_PUR_AGENCY, a.OVC_PUR_IPURCH, a.OVC_PUR_NSECTION, " +
            //             "       a.OVC_PUR_USER, a.OVC_PUR_IUSER_PHONE_EXT, a.OVC_DPROPOSE, " +
            //             "       a.OVC_PROPOSE, a.OVC_PUR_APPROVE, a.OVC_PUR_DAPPROVE,a.ovc_Pur_Dcanpo, " +
            //             "       b.OVC_PURCH, b.OVC_DRECEIVE, b.OVC_CHECK_UNIT, b.ONB_CHECK_TIMES, " +
            //             "       b.OVC_DAUDIT_ASSIGN, b.OVC_AUDIT_UNIT,b.OVC_AUDIT_ASSIGNER," +
            //             "       b.OVC_DAUDIT, b.OVC_AUDITOR, c.OVC_DRESULT " +
            //             " from TBM1301 a, TBM1202_1 b, TBM1202 c, TBM1301_PLAN d " +
            //             " where a.OVC_PURCH = b.OVC_PURCH " +
            //             "   and b.OVC_AUDIT_UNIT = '" + userapprovedept.getOvc_Audit_Unit() + "' " + //同聯審小組(代碼K5)
            //             "   and a.OVC_PURCH = d.OVC_PURCH ";
            //    if (user.getUser_Id().equals("31ASSIGNER"))
            //    {
            //        sql = sql + "    and ( d.OVC_AUDIT_UNIT = '00N00' or d.OVC_AUDIT_UNIT = '0A100' " + //計評單位=登錄者單位
            //                "         or d.OVC_PUR_APPROVE_DEP = 'A') "; //讀取採購中心權責購案(部核案)
            //    }
            //    else
            //    {
            //        if (user.getOvc_Pur_Section().equals("00N00") || user.getOvc_Pur_Section().equals("0A100"))
            //        {
            //            sql = sql + "    and ( d.OVC_AUDIT_UNIT = '00N00' or d.OVC_AUDIT_UNIT = '0A100' " + //計評單位=登錄者單位
            //                  "         or d.OVC_PUR_APPROVE_DEP = 'A') "; //讀取採購中心權責購案(部核案)
            //        }
            //        else
            //        {
            //            sql = sql + "    and d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' ";  //計評單位=登錄者單位             
            //        }
            //    }

            //    //主官(管) 權限 or 承辦首席
            //    if (((user.getOvc_Priv_Level() != null) && (user.getOvc_Priv_Level().equals("5"))) ||
            //      ((user.getOvc_Priv_Level() != null) && (user.getOvc_Priv_Level().equals("8"))))
            //    {
            //        if ((qry_YY != null) && (!qry_YY.equals("")) && (!qry_YY.equals("all")))
            //        {
            //            sql = sql + " and substr(a.OVC_PURCH,3,2) = '" + qry_YY + "'";//限制年度 
            //        }
            //        if ((ovc_Name != null) && (!ovc_Name.equals("none")))
            //        {
            //            sql = sql + "   and b.OVC_AUDITOR = '" + ovc_Name + "'"; //限制某承辦人
            //        }
            //        else
            //        {
            //            sql = sql + "   and (b.OVC_AUDITOR is null or b.OVC_AUDITOR = ' ') ";
            //        }
            //    }
            //    else
            //    {
            //        //承辦人 權限
            //        if ((qry_Purch != null) && (!qry_Purch.equals("")))
            //        {
            //            //指定某購案        
            //            sql = sql + " and a.OVC_PURCH like '" + modovc_purch + "%'";
            //        }
            //        else
            //        {
            //            if ((qry_YY != null) && (!qry_YY.equals("")) && (!qry_YY.equals("all")))
            //            {
            //                //限制年度        
            //                sql = sql + " and substr(a.OVC_PURCH,3,2) = '" + qry_YY + "'";
            //            }
            //            //指定某承辦人      
            //            if ((ovc_Name != null) && (!ovc_Name.equals("")))
            //            {
            //                sql = sql + "   and b.OVC_AUDITOR = '" + ovc_Name + "'";
            //            }
            //        }
            //    }
            //    if (vcase != null && vcase.equals("notApprove"))
            //    {
            //        if (user.getOvc_Pur_Section().equals("0A100") || user.getOvc_Pur_Section().equals("00N00"))
            //        {//採購中心核定文號為【備採綜 ...】  
            //            sql = sql + " and (a.ovc_Pur_Dapprove is null " + //尚未核定
            //                        " or (a.ovc_Pur_Dapprove is not null and substr(trim(nvl(a.ovc_pur_approve,' ')),1,2) not in ('備採','昇軸') )) " + //有核定日期但非採購中心核定
            //                        " and ( d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' or d.OVC_AUDIT_UNIT = '00N00'   " + //計評單位=登錄者單位      
            //                  "         or d.OVC_PUR_APPROVE_DEP = 'A') "; //讀取採購中心權責購案(部核案)
            //        }
            //        else
            //        {
            //            sql = sql + " and a.ovc_Pur_Dapprove is null " + //尚未核定
            //                        " and d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' ";  //計評單位=登錄者單位 
            //        }
            //        sql = sql + " and (a.OVC_PUR_DCANPO is null or a.OVC_PUR_DCANPO = '') " +  //未撤案
            //                    " and (d.OVC_DCANCEL is null or d.OVC_DCANCEL = '') ";
            //    }
            //    else if (vcase != null && vcase.equals("Approve"))
            //    {
            //        if (user.getOvc_Pur_Section().equals("0A100") || user.getOvc_Pur_Section().equals("00N00"))
            //        { //採購中心核定文號為【備採綜 ...】 
            //            sql = sql + " and (a.ovc_Pur_Dapprove is not null and substr(trim(nvl(a.ovc_pur_approve,' ')),1,2) in ('備採','昇軸') ) " + //採購中心核定
            //                        " and ( d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' or d.OVC_AUDIT_UNIT = '00N00'   " + //計評單位=登錄者單位      
            //                        " or d.OVC_PUR_APPROVE_DEP = 'A') "; //讀取採購中心權責購案(部核案)
            //        }
            //        else
            //        {
            //            sql = sql + " and a.ovc_Pur_Dapprove is not null " + //已核定
            //                        "    and d.OVC_AUDIT_UNIT = '" + user.getOvc_Pur_Section() + "' ";  //計評單位=登錄者單位 
            //        }
            //        sql = sql + " and (a.OVC_PUR_DCANPO is null or a.OVC_PUR_DCANPO = '') " +//未撤案
            //                    " and (d.OVC_DCANCEL is null or d.OVC_DCANCEL = '') ";
            //    }
            //    else if (vcase != null && vcase.equals("Cancel"))
            //    {
            //        sql = sql + " and ((a.OVC_PUR_DCANPO is not null) " + //已撤案;
            //                    " or (d.OVC_DCANCEL is not null)) ";
            //    }
            //    sql = sql + "  and b.OVC_PURCH = c.OVC_PURCH " +
            //            "  and b.OVC_DRECEIVE = c.OVC_DRECEIVE " +
            //            "  and b.OVC_CHECK_UNIT = c.OVC_CHECK_UNIT " +
            //            "  and b.ONB_CHECK_TIMES = c.ONB_CHECK_TIMES ";
            //    if (user.getOvc_Pur_Section().equals("0A100") || user.getOvc_Pur_Section().equals("00N00"))
            //    {
            //        sql = sql + "  and (b.OVC_CHECK_UNIT = '" + user.getOvc_Pur_Section() + "' or b.OVC_CHECK_UNIT = '00N00') "; //主辦單位與登入者單位相同
            //    }
            //    else
            //    {
            //        sql = sql + "  and b.OVC_CHECK_UNIT = '" + user.getOvc_Pur_Section() + "' "; //主辦單位與登入者單位相同
            //    }
            //    sql = sql + "   order by b.OVC_PURCH, b.ONB_CHECK_TIMES ";
            //}
        }
        

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connnn))
            {
                conn.Open();
                string sql = @"select user_name from Account where user_id = 'ADMIN32' ";
                OracleCommand cmd = new OracleCommand(sql, conn);
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    this.TextBox1.Text = dr["user_name"].ToString();
                }
                conn.Close();
            }
        }
        
    }
}