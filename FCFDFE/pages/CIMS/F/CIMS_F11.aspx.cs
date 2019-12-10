using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.CIMSModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;

namespace FCFDFE.pages.CIMS.F
{
    public partial class CIMS_F11 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        CIMSEntities CIMS = new CIMSEntities();
        MPMSEntities MPMS = new MPMSEntities();
        string[] strField = { "RANK", "OVC_VEN_CST", "OVC_NVEN", "OVC_VEN_TITLE", "PERFORM_NAME", "OVC_VEN_ITEL", "OVC_VEN_ADDRESS" };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);

        }

        protected void GV_TBM1203_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }

        protected void GV_TBM1203_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string id = GV_TBM1203.DataKeys[gvrIndex].Value.ToString();
            switch (e.CommandName)
            {
                case "OVC_VEN_CST":
                    cleantxt();
                    dataimport(id);
                    Detail.Visible = true;
                    search.Visible = false;
                    Addok.Visible = false;
                    Editok.Visible = true;
                    addOVC_VEN_CST.Visible = false;
                    editcontrol.Visible=true;
                    editcontorl1.Visible = true;
                    addcontrol1.Visible = false;
                    tbEIN_change.Visible = true;

                    return;
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            cleantxt();
            Detail.Visible = true;
            search.Visible = false;
            Addok.Visible = true;
            Editok.Visible = false;
            tbEIN_change.Visible = false;
            addOVC_VEN_CST.Visible = true;
            editcontrol.Visible = false;
            editcontorl1.Visible = false;
            addcontrol1.Visible = true;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            bool flag = true;//查詢所有資料
            showGV_TBM1203(flag);
            GV_TBM1203.Visible = true;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            FCommon.Controls_Clear(txtOVC_VEN_TITLE_query, txtOVC_NVEN_query, txtOVC_VEN_CST_query);
        }

        private void showGV_TBM1203(bool flag)
        {
            DataTable dt = new DataTable();
            var query =
                from TBM1203 in MPMS.TBM1203.DefaultIfEmpty().AsEnumerable()
                orderby TBM1203.OVC_VEN_CST ascending
                select new
                {
                    OVC_VEN_CST = TBM1203.OVC_VEN_CST ?? "",
                    OVC_NVEN = TBM1203.OVC_NVEN ?? "",
                    OVC_VEN_TITLE = TBM1203.OVC_VEN_TITLE ?? "",
                    PERFORM_NAME = TBM1203.PERFORM_NAME ?? "",
                    OVC_VEN_ITEL = TBM1203.OVC_VEN_ITEL ?? "",
                    OVC_VEN_ADDRESS = TBM1203.OVC_VEN_ADDRESS ?? ""
                };
            if (flag != true)
                query = query.Where(table => table.OVC_VEN_CST.Contains("BP"));
            else
            {
                if (txtOVC_VEN_TITLE_query.Text != "")
                    query = query.Where(table => table.OVC_VEN_TITLE.Contains(txtOVC_VEN_TITLE_query.Text));
                if (txtOVC_NVEN_query.Text != "")
                    query = query.Where(table => table.OVC_NVEN.Contains(txtOVC_NVEN_query.Text));
                if (txtOVC_VEN_CST_query.Text != "")
                    query = query.Where(table => table.OVC_VEN_CST.Contains(txtOVC_VEN_CST_query.Text));
            }
            
            
                

            dt = CommonStatic.LinqQueryToDataTable(query);
            DataColumn column = new DataColumn();
            column.ColumnName = "RANK";
            column.DataType = System.Type.GetType("System.Int32");
            dt.Columns.Add(column);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Rank"] = i + 1;
            }
            ViewState["hasRows"] = FCommon.GridView_dataImport(GV_TBM1203, dt, strField);
        }

        protected void Return_Click(object sender, EventArgs e)
        {
            Detail.Visible = false;
            search.Visible = true;
            
        }

        protected void btnQuery_temporary_Click(object sender, EventArgs e)
        {
            bool flag = false;//查詢暫編資料
            showGV_TBM1203(flag);
            GV_TBM1203.Visible = true;
        }

        protected void EIN_Change_Click(object sender, EventArgs e)
        {
            string EINafter = EIN_after.Text;
            string EINbefor = EIN_before.Text;
            if (EINafter == "")
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "請先輸入統一編號");
            }
            else
            {
                TBM1203 tbm1203 = new TBM1203();
                tbm1203 = MPMS.TBM1203.Where(table => table.OVC_VEN_CST.Equals(EINafter)).FirstOrDefault();
                if (tbm1203 == null)
                {
                    //PK無法修改，把資料換新PK後儲存，再將舊資料刪除
                    TBM1203 tbm1203_change = new TBM1203();
                    tbm1203_change = MPMS.TBM1203.Where(table => table.OVC_VEN_CST.Equals(EINbefor)).FirstOrDefault();

                    TBM1203 tbm1203_add = new TBM1203();

                    tbm1203_add.OVC_VEN_CST = EINafter;
                    tbm1203_add.OVC_VEN_ADDRESS = tbm1203_change.OVC_VEN_ADDRESS;
                    tbm1203_add.OVC_VEN_ADDRESS_1 = tbm1203_change.OVC_VEN_ADDRESS_1;
                    tbm1203_add.OVC_PUR_CREATE = tbm1203_change.OVC_PUR_CREATE;
                    tbm1203_add.OVC_DRECOVERY = tbm1203_change.OVC_DRECOVERY;
                    tbm1203_add.OVC_PUR_TUPDATE = tbm1203_change.OVC_PUR_TUPDATE;
                    tbm1203_add.OVC_IBUILD_UNIT = tbm1203_change.OVC_IBUILD_UNIT;
                    tbm1203_add.OVC_ICOUNTRY = tbm1203_change.OVC_ICOUNTRY;
                    tbm1203_add.OVC_VEN_TITLE = tbm1203_change.OVC_VEN_TITLE;
                    tbm1203_add.CAGE = tbm1203_change.CAGE;
                    tbm1203_add.OVC_FAX_NO = tbm1203_change.OVC_FAX_NO;
                    tbm1203_add.OVC_VEN_ITEL = tbm1203_change.OVC_VEN_ITEL;
                    tbm1203_add.OVC_IPURCHASE = tbm1203_change.OVC_IPURCHASE;
                    tbm1203_add.ONB_QSUBTRACT = tbm1203_change.ONB_QSUBTRACT;
                    tbm1203_add.OVC_BOSS = tbm1203_change.OVC_BOSS;
                    tbm1203_add.USERID = Session["username"].ToString();
                    tbm1203_add.CHECKED = tbm1203_change.CHECKED;
                    tbm1203_add.INPUT_USER = tbm1203_change.INPUT_USER;
                    tbm1203_add.OVC_MAIN_PRODUCT = tbm1203_change.OVC_MAIN_PRODUCT;
                    tbm1203_add.CSIST_VEN_CST = tbm1203_change.CSIST_VEN_CST;
                    tbm1203_add.OVC_STATUS = tbm1203_change.OVC_STATUS;
                    tbm1203_add.GINGE_VEN_CST = tbm1203_change.GINGE_VEN_CST;
                    tbm1203_add.MIXED_CAGE = tbm1203_change.MIXED_CAGE;
                    tbm1203_add.MIXED_VEN_CST = tbm1203_change.MIXED_VEN_CST;
                    tbm1203_add.OVC_UPDATED = tbm1203_change.OVC_UPDATED;
                    tbm1203_add.KEY = tbm1203_change.KEY;
                    tbm1203_add.BID_PURCH = tbm1203_change.BID_PURCH;
                    tbm1203_add.CAGE_DATE = tbm1203_change.CAGE_DATE;
                    tbm1203_add.PERFORM_NAME = tbm1203_change.PERFORM_NAME;
                    tbm1203_add.OVC_MANAGE = tbm1203_change.OVC_MANAGE;
                    tbm1203_add.OVC_DMANAGE_BEGIN = tbm1203_change.OVC_DMANAGE_BEGIN;
                    tbm1203_add.OVC_DMANAGE_END = tbm1203_change.OVC_DMANAGE_END;

                    MPMS.TBM1203.Add(tbm1203_add);
                    MPMS.SaveChanges();

                    MPMS.Entry(tbm1203_change).State = EntityState.Deleted;
                    MPMS.SaveChanges();
                    FCommon.AlertShow(PnMessage, "success", "系統訊息", "統一編號轉換成功");
                    dataimport(EINafter);
                }
                else
                    FCommon.AlertShow(PnMessage, "danger", "系統訊息", "此組統一編號已被使用");
            }
            
        }

        protected void Editok_Click(object sender, EventArgs e)
        {
            string EIN_id = txtOVC_VEN_CST.Text;
            TBM1203 tbm1203 = new TBM1203();
            tbm1203 = MPMS.TBM1203.Where(table => table.OVC_VEN_CST.Equals(EIN_id)).FirstOrDefault();
            if (tbm1203 != null)
            {
                tbm1203.OVC_VEN_TITLE = txtOVC_VEN_TITLE.Text;
                tbm1203.OVC_VEN_ADDRESS = txtOVC_VEN_ADDRESS.Text;
                tbm1203.OVC_NVEN = txtOVC_NVEN.Text;
                tbm1203.OVC_VEN_ADDRESS_1 = txtOVC_VEN_ADDRESS_1.Text;
                tbm1203.OVC_VEN_ITEL = txtOVC_VEN_ITEL.Text;
                tbm1203.OVC_FAX_NO = txtOVC_FAX_NO.Text;
                tbm1203.OVC_BOSS = txtOVC_BOSS.Text;
                tbm1203.PERFORM_NAME = txtPERFORM_NAME.Text;
                tbm1203.GINGE_VEN_CST = txtGINGE_VEN_CST.Text;
                tbm1203.CAGE = txtCAGE.Text;
                tbm1203.CAGE_DATE = txtCAGE_DATE.Text;
                tbm1203.OVC_MAIN_PRODUCT = txtOVC_MAIN_PRODUCT.Text;
                MPMS.SaveChanges();
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "廠商資料修改成功");
            }
        }

        private void dataimport(string id)
        {
            var query =
                (from TBM1203 in MPMS.TBM1203
                 where TBM1203.OVC_VEN_CST.Equals(id)
                 select TBM1203).FirstOrDefault();
            if (query != null)
            {
                EIN_before.Text= query.OVC_VEN_CST;
                txtOVC_VEN_CST.Text = query.OVC_VEN_CST;
                txtOVC_VEN_TITLE.Text = query.OVC_VEN_TITLE;
                txtOVC_NVEN.Text = query.OVC_NVEN;
                txtOVC_VEN_ADDRESS.Text = query.OVC_VEN_ADDRESS;
                txtOVC_VEN_ADDRESS_1.Text = query.OVC_VEN_ADDRESS_1;
                txtOVC_PUR_CREATE.Text = query.OVC_PUR_CREATE;
                txtOVC_VEN_ITEL.Text = query.OVC_VEN_ITEL;
                txtOVC_FAX_NO.Text = query.OVC_FAX_NO;
                txtOVC_BOSS.Text = query.OVC_BOSS;
                txtPERFORM_NAME.Text= query.PERFORM_NAME;
                txtGINGE_VEN_CST.Text = query.GINGE_VEN_CST;
                txtCAGE.Text = query.CAGE;
                txtCAGE_DATE.Text = query.CAGE_DATE;
                txtOVC_MAIN_PRODUCT.Text = query.OVC_MAIN_PRODUCT;
            }
        }

        protected void Addok_Click(object sender, EventArgs e)
        {
            int num1;
            if (addOVC_VEN_CST.Text.Length != 8 || int.TryParse(addOVC_VEN_CST.Text, out num1) == false)
            {
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "欄位[統一編號]所輸入的資料不正確，統一編號為8碼數字");
                return;
            }

            TBM1203 querytbm1203 = new TBM1203();
            querytbm1203 = MPMS.TBM1203.Where(table => table.OVC_VEN_CST.Equals(addOVC_VEN_CST.Text)).FirstOrDefault();
            if (querytbm1203 != null)
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", "統一編號已被使用過!");
            else
            {
                TBM1203 addtbm1203 = new TBM1203();
                addtbm1203.OVC_VEN_CST = addOVC_VEN_CST.Text;
                addtbm1203.OVC_VEN_TITLE = txtOVC_VEN_TITLE.Text;
                addtbm1203.OVC_VEN_ADDRESS = txtOVC_VEN_ADDRESS.Text;
                addtbm1203.OVC_NVEN = txtOVC_NVEN.Text;
                addtbm1203.OVC_VEN_ADDRESS_1 = txtOVC_VEN_ADDRESS_1.Text;
                addtbm1203.OVC_VEN_ITEL = txtOVC_VEN_ITEL.Text;
                addtbm1203.OVC_FAX_NO = txtOVC_FAX_NO.Text;
                addtbm1203.OVC_BOSS = txtOVC_BOSS.Text;
                addtbm1203.PERFORM_NAME = txtPERFORM_NAME.Text;
                addtbm1203.GINGE_VEN_CST = txtGINGE_VEN_CST.Text;
                addtbm1203.CAGE = txtCAGE.Text;
                addtbm1203.CAGE_DATE = txtCAGE_DATE.Text;
                addtbm1203.OVC_MAIN_PRODUCT = txtOVC_MAIN_PRODUCT.Text;
                addtbm1203.OVC_PUR_CREATE= DateTime.Now.ToString("yyyy-MM-dd");
                addtbm1203.INPUT_USER= Session["username"].ToString();
                addtbm1203.USERID= Session["username"].ToString();
                addtbm1203.OVC_VEN_SN = Guid.NewGuid();
                MPMS.TBM1203.Add(addtbm1203);
                MPMS.SaveChanges();
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "廠商資料新增成功");
                cleantxt();
            }
        }



        private void cleantxt()
        {
            FCommon.Controls_Clear(EIN_before, EIN_after, txtOVC_VEN_CST, txtOVC_VEN_TITLE, txtOVC_VEN_ADDRESS, txtOVC_NVEN);
            FCommon.Controls_Clear(txtOVC_VEN_ADDRESS_1, txtOVC_PUR_CREATE, txtOVC_VEN_ITEL, txtOVC_FAX_NO, txtOVC_BOSS, txtPERFORM_NAME);
            FCommon.Controls_Clear(txtGINGE_VEN_CST, txtCAGE, txtCAGE_DATE, txtOVC_MAIN_PRODUCT, addOVC_VEN_CST);
        }


    }
}
