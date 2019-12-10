using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.pages.MPMS.A
{
    public partial class example_MPMS_A11 : Page
    {
        bool hasRows = false;
        Common FCommon = new Common();
        GMEntities GME = new GMEntities();
        MPMSEntities MPMSE = new MPMSEntities();
        public string strMenuName = "", strMenuNameItem = "";
        string strImagePagh = Content.Variable.strImagePath;

        #region 副程式
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            MaintainScrollPositionOnPostBack = true; //autopostback時不會在最上方，沒用

            if (!IsPostBack)
            {
                imgExanple.ImageUrl = strImagePagh + "CIMS/CIMS_C11_example.PNG";
                FCommon.Controls_Attributes("readonly", "true", TextBox1);
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("column1");
            dt.Columns.Add("column2");
            dt.Columns.Add("column3");
            for (int i = 0; i < 5; i++)
            {
                DataRow dr = dt.NewRow();
                dr["column1"] = "資料鏈結1-" + i.ToString();
                dr["column2"] = "資料鏈結2-" + i.ToString();
                dr["column3"] = "資料鏈結3-" + i.ToString();
                dt.Rows.Add(dr);
                hasRows = true;
            }
            GridView1.DataSource = dt.AsDataView();
            GridView1.DataBind();
            GridView2.DataSource = dt.AsDataView();
            GridView2.DataBind();

            #region 複雜版
            ////select 範例
            ////SELECT M.ovc_purch,M.C7,N.TG
            ////FROM(SELECT tbm1301.ovc_purch, tbm1301.OVC_PUR_ASS_VEN_CODE, TBM1407.OVC_PHR_DESC AS C7 FROM tbm1301 left JOIN(SELECT * FROM TBM1407 WHERE TBM1407.OVC_PHR_CATE = 'C7') TBM1407 ON tbm1301.OVC_PUR_ASS_VEN_CODE = TBM1407.OVC_PHR_ID) M INNER JOIN
            ////(SELECT tbm1301.ovc_purch, tbm1301.OVC_BID_TIMES, TBM1407.OVC_PHR_DESC AS TG FROM tbm1301 left JOIN(SELECT * FROM TBM1407 WHERE TBM1407.OVC_PHR_CATE = 'TG') TBM1407 ON tbm1301.OVC_BID_TIMES = TBM1407.OVC_PHR_ID) N ON M.ovc_purch = N.ovc_purch
            ////where m.ovc_purch = 'HJ96001'

            ////SELECT * FROM TBM1407 WHERE TBM1407.OVC_PHR_CATE = 'C7'
            //var queryTBM1407_C7 =
            //    from table1407 in GME.TBM1407 where table1407.OVC_PHR_CATE.Equals("C7") select table1407;

            ////M SELECT tbm1301.ovc_purch, tbm1301.OVC_PUR_ASS_VEN_CODE, TBM1407.OVC_PHR_DESC AS C7 FROM tbm1301 left JOIN(queryTBM1407_C7) TBM1407 ON tbm1301.OVC_PUR_ASS_VEN_CODE = TBM1407.OVC_PHR_ID
            //var queryTBM1301_C7 =
            //    from talbe1301 in GME.TBM1301
            //    join table1407 in queryTBM1407_C7 on talbe1301.OVC_PUR_ASS_VEN_CODE equals table1407.OVC_PHR_ID into temp1407
            //    from table1407 in temp1407.DefaultIfEmpty()
            //    select new
            //    {
            //        talbe1301.OVC_PURCH,
            //        talbe1301.OVC_PUR_ASS_VEN_CODE,
            //        C7 = table1407.OVC_PHR_DESC
            //    };

            ////SELECT * FROM TBM1407 WHERE TBM1407.OVC_PHR_CATE = 'TG'
            //var queryTBM1407_TG =
            //    from table1407 in GME.TBM1407 where table1407.OVC_PHR_CATE.Equals("TG") select table1407;

            ////N SELECT tbm1301.ovc_purch, tbm1301.OVC_BID_TIMES, TBM1407.OVC_PHR_DESC AS TG FROM tbm1301 left JOIN(queryTBM1407_TG) TBM1407 ON tbm1301.OVC_BID_TIMES = TBM1407.OVC_PHR_ID
            //var queryTBM1301_TG =
            //    from talbe1301 in GME.TBM1301
            //    join table1407 in queryTBM1407_TG on talbe1301.OVC_BID_TIMES equals table1407.OVC_PHR_ID into temp1407
            //    from table1407 in temp1407.DefaultIfEmpty()
            //    select new
            //    {
            //        talbe1301.OVC_PURCH,
            //        talbe1301.OVC_BID_TIMES,
            //        TG = table1407.OVC_PHR_DESC
            //    };

            //var queryTBM1301_Sum =
            //    from tableC7 in queryTBM1301_C7
            //    join tableTG in queryTBM1301_TG on tableC7.OVC_PURCH equals tableTG.OVC_PURCH
            //    where tableC7.OVC_PURCH.Equals("HJ96001")
            //    select new
            //    {
            //        tableC7.OVC_PURCH,
            //        tableC7.C7,
            //        tableTG.TG
            //    };
            #endregion



            //select AA.ovc_purch,B1.OVC_PHR_DESC as C7,B2.OVC_PHR_DESC as TG from tbm1301 AA
            //left join tbm1407 B1 on AA.OVC_PUR_ASS_VEN_CODE = B1.OVC_PHR_ID and B1.OVC_PHR_CATE = 'C7'
            //left join tbm1407 B2 on AA.OVC_BID_TIMES = B1.OVC_PHR_ID and B2.OVC_PHR_CATE = 'TG'
            //where AA.ovc_purch = 'HJ96003'

            var queryTBM1407_C7 =
                from table1407 in GME.TBM1407 where table1407.OVC_PHR_CATE.Equals("C7") select table1407;
            var queryTBM1407_TG =
                from table1407 in GME.TBM1407 where table1407.OVC_PHR_CATE.Equals("TG") select table1407;

            var query =
                from table1301 in GME.TBM1301
                join table1407_G7 in queryTBM1407_C7 on table1301.OVC_PUR_ASS_VEN_CODE equals table1407_G7.OVC_PHR_ID into tempG7
                from table1407_G7 in tempG7.DefaultIfEmpty()
                join table1407_TG in queryTBM1407_TG on table1301.OVC_BID_TIMES equals table1407_TG.OVC_PHR_ID into tempTG
                from table1407_TG in tempTG.DefaultIfEmpty()
                where table1301.OVC_PURCH.Equals("HJ96001")
                select new
                {
                    table1301.OVC_PURCH,
                    C7 = table1407_G7.OVC_PHR_DESC,
                    TG = table1407_TG.OVC_PHR_DESC
                };
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GridView1.UseAccessibleHeader = true;
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void BtnEdt_Click(object sender, EventArgs e)
        {
            FCommon.AlertShow(PnMessage, "danger", "系統訊息", "欄位不得為空值");
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            FCommon.AlertShow(PnMessage, "info", "系統訊息", "文字方塊改變");
        }

        protected void GridView2_PreRender(object sender, EventArgs e)
        {
            if (hasRows)
            {
                GridView2.UseAccessibleHeader = true;
                GridView2.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
    }
}