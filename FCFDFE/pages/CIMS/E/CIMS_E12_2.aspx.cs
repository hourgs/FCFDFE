using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using FCFDFE.Content;
using FCFDFE.Entity.CIMSModel;
using FCFDFE.Entity.GMModel;

namespace FCFDFE.pages.CIMS.E
{
    public partial class CIMS_E12_2 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        private CIMSEntities CIMS = new CIMSEntities();
        private GMEntities GM = new GMEntities();
        Common FCommon = new Common();
        String strs;
        protected void Page_Load(object sender, EventArgs e)
        {
            FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
            DataTable dr = new DataTable();
           
            var queryr =
                from com in CIMS.COMMONLINK.AsEnumerable()
                from tbm in GM.TBM1407.AsEnumerable()
                where com.CL_LAN == "公" || com.CL_LAN == "民"
                where tbm.OVC_PHR_CATE=="CL"
                where com.CL_KIND == tbm.OVC_PHR_ID
                select new
                {
                    com.CL_ORD,         //排序'
                    com.CL_LAN,         //網域'
                    com.CL_TITLE,       //連結名稱'
                    com.CL_LINK,        //超連結'
                    com.CL_DESC,        //內容概述'
                    com.CL_UPLOADLINK,  //網站說明'
                    tbm.OVC_PHR_DESC,   //類別
                };
            dr = CommonStatic.LinqQueryToDataTable(queryr);


            for (int i = 0; i < dr.Rows.Count; i++)
            {
                String ifone = dr.Rows[i][0].ToString().Trim();
                if (ifone == "1")
                {
                    strs += "<tr><th colspan='5'>" + dr.Rows[i][6]+"</th></tr>";
                    strs += "<tr>";
                    for (int j = 0; j <= 5; j++)
                    {
                        if (j != 2 && j != 3)
                            strs += "<td>" + dr.Rows[i][j] + "</td>";
                        else if (j == 2)
                            strs += "<td><a href='http://" + dr.Rows[i][j+1]+"'>" + dr.Rows[i][j] + "</a>" + "<br />【" + dr.Rows[i][j + 1] + "】" + "</td>";

                    }
                    strs += "</tr>";
                }
                else
                {
                    strs += "<tr>";
                    for (int j = 0; j <= 5; j++)
                    {
                        if( j!=2 && j !=3)
                        strs += "<td>" + dr.Rows[i][j] + "</td>";
                        else if(j==2)
                        strs += "<td><a href='http://" + dr.Rows[i][j+1]+"'>"+ dr.Rows[i][j]+"</a>"+ "<br />【" + dr.Rows[i][j+1]+ "】" + "</td>";
                      
                    }
                    strs += "</tr>";
                }
            }
            Literal1.Text = strs;

        }
    }
}