using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;
using FCFDFE.Entity.GMModel;
using FCFDFE.Entity.MPMSModel;
using System.Data.Entity;
using System.Globalization;
using TemplateEngine.Docx;
using System.IO;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D18_6 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strOVC_PURCH, strOVC_PUR_AGENCY, strOVC_PURCH_5, strOVC_DOPEN;
        short numONB_TIMES, numONB_GROUP;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            {
                //設置readonly屬性
                FCommon.Controls_Attributes("readonly", "true", txtOVC_DBACK);

                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_DOPEN"] == null || Request.QueryString["ONB_TIMES"] == null || Request.QueryString["ONB_GROUP"] == null)
                    FCommon.MessageBoxShow(this, "錯誤訊息：請先選擇購案", "MPMS_D14.aspx", false);
                else
                {
                    strOVC_PURCH = Request.QueryString["OVC_PURCH"].ToString();
                    strOVC_DOPEN = Request.QueryString["OVC_DOPEN"].ToString();
                    short.TryParse(Request.QueryString["ONB_TIMES"].ToString(), out numONB_TIMES);
                    short.TryParse(Request.QueryString["ONB_GROUP"].ToString(), out numONB_GROUP);

                    TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;

                    TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    strOVC_PUR_AGENCY = tbm1301 == null ? "" : tbm1301.OVC_PUR_AGENCY;

                    if (IsOVC_DO_NAME() && !IsPostBack)
                        DataImport();   //資料帶入頁面
                }
            }
        }

        protected void btnTBMBID_END_Click(object sender, EventArgs e)
        {
            //按下按鈕 截標審查(廠商編輯)作業
            string send_url = "~/pages/MPMS/D/MPMS_D18_3.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_DOPEN=" + strOVC_DOPEN
                            + "&ONB_TIMES=" + numONB_TIMES + "&ONB_GROUP=" + numONB_GROUP;
            Response.Redirect(send_url);
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            //點擊 存檔
            lblOVC_PURCH_A_5.Text = strOVC_PURCH + strOVC_PUR_AGENCY + strOVC_PURCH_5;
            lblOVC_DOPEN.Text = GetTaiwanDate(strOVC_DOPEN);
            lblONB_GROUP.Text = numONB_GROUP.ToString();

            string reason1 = "", reason2 = "", reason3 = "", reason4 = "", reason5 = "";
            if (chkOVC_REASON_1.Checked == true)
                reason1 = "本次開標未達法定家數，依規定原封退回貴廠商押標金及投標文件。";
            if (chkOVC_REASON_2.Checked == true)
            {
                reason2 = "本次依政府採購法第四十八條第一項第一款規定不予";
                if (chkOVC_REASON_2_1.Items[0].Selected == true)
                    reason2 += "開標";
                if (chkOVC_REASON_2_1.Items[1].Selected == true)
                    reason2 += "決標";
                reason2 += "，依規定原封退還貴廠商";
                if (chkOVC_REASON_2_2.Items[0].Selected == true)
                    reason2 += "押標金";
                if (chkOVC_REASON_2_2.Items[1].Selected == true)
                    reason2 += "投標文件";
                reason2 += "。";
            }
            if (chkOVC_REASON_4.Checked == true)
            {
                reason4 = "貴廠商經評選結果非為最優勝廠商，依規定退回貴廠商";
                if (chkOVC_REASON_4_1.Items[0].Selected == true)
                    reason4 += "押標金";
                if (chkOVC_REASON_4_1.Items[1].Selected == true)
                    reason4 += "企劃書乙式份";
                if (chkOVC_REASON_4_1.Items[2].Selected == true)
                    reason4 += "建議書乙式份";
                reason4 += "。";
            }
            if (chkOVC_REASON_5.Checked == true)
                reason5 += "貴廠商於得標後已依規定繳足額履約保證金，依規定退回貴廠商押標金。";
            if (chkOVC_REASON_6.Checked == true)
                reason5 += "本案開標結果通知單。";

            TBMBID_BACK tbmBID_BACK = new TBMBID_BACK();
            tbmBID_BACK = mpms.TBMBID_BACK.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP).FirstOrDefault();

            if (tbmBID_BACK != null)
            {
                //修改 TBMBID_BACK (採購開標押標金/投標文件退還紀錄主檔)
                tbmBID_BACK.OVC_DBACK = txtOVC_DBACK.Text;
                tbmBID_BACK.OVC_BACK_PLACE = rdoOVC_BACK_PLACE.SelectedValue;
                tbmBID_BACK.OVC_REASON_1 = reason1.Equals(string.Empty) ? null : reason1;
                tbmBID_BACK.OVC_REASON_2 = reason2.Equals(string.Empty) ? null : reason2;
                tbmBID_BACK.OVC_REASON_3 = reason4.Equals(string.Empty) ? null : reason4;
                tbmBID_BACK.OVC_REASON_5 = reason5.Equals(string.Empty) ? null : reason5;

                if (chkOVC_REASON_3.Checked == true)
                {
                    tbmBID_BACK.OVC_REASON_41 = GetSaveValue(chkOVC_REASON_3_1, 0);
                    tbmBID_BACK.OVC_REASON_42 = GetSaveValue(chkOVC_REASON_3_1, 1);
                    tbmBID_BACK.OVC_REASON_43 = GetSaveValue(chkOVC_REASON_3_1, 2);
                    tbmBID_BACK.OVC_REASON_44 = GetSaveValue(chkOVC_REASON_3_1, 3);
                    tbmBID_BACK.OVC_REASON_45 = GetSaveValue(chkOVC_REASON_3_1, 4);
                }
                else
                {
                    tbmBID_BACK.OVC_REASON_41 = null;
                    tbmBID_BACK.OVC_REASON_42 = null;
                    tbmBID_BACK.OVC_REASON_43 = null;
                    tbmBID_BACK.OVC_REASON_44 = null;
                    tbmBID_BACK.OVC_REASON_45 = null;
                }
                tbmBID_BACK.OVC_MEMO = txtOVC_MEMO.Text;
                tbmBID_BACK.OVC_REMARK = txtOVC_REMARK.Text;
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmBID_BACK.GetType().Name.ToString(), this, "修改");
            }

            else
            {
                //新增 TBMBID_BACK (採購開標押標金/投標文件退還紀錄主檔)
                TBMBID_BACK tbmBID_BACK_new = new TBMBID_BACK();
                tbmBID_BACK_new.OVC_SN = Guid.NewGuid();
                tbmBID_BACK_new.OVC_PURCH = strOVC_PURCH;
                tbmBID_BACK_new.OVC_PURCH_5 = strOVC_PURCH_5;
                tbmBID_BACK_new.OVC_DOPEN = strOVC_DOPEN;
                tbmBID_BACK_new.ONB_GROUP = numONB_GROUP;
                tbmBID_BACK_new.OVC_DBACK = txtOVC_DBACK.Text;
                tbmBID_BACK_new.OVC_BACK_PLACE = rdoOVC_BACK_PLACE.SelectedValue;
                tbmBID_BACK_new.OVC_REASON_1 = reason1.Equals(string.Empty) ? null : reason1;
                tbmBID_BACK_new.OVC_REASON_2 = reason2.Equals(string.Empty) ? null : reason2;
                tbmBID_BACK_new.OVC_REASON_3 = reason4.Equals(string.Empty) ? null : reason4;
                tbmBID_BACK_new.OVC_REASON_5 = reason5.Equals(string.Empty) ? null : reason5;
                if (chkOVC_REASON_3.Checked == true)
                {
                    tbmBID_BACK_new.OVC_REASON_41 = GetSaveValue(chkOVC_REASON_3_1, 0);
                    tbmBID_BACK_new.OVC_REASON_42 = GetSaveValue(chkOVC_REASON_3_1, 1);
                    tbmBID_BACK_new.OVC_REASON_43 = GetSaveValue(chkOVC_REASON_3_1, 2);
                    tbmBID_BACK_new.OVC_REASON_44 = GetSaveValue(chkOVC_REASON_3_1, 3);
                    tbmBID_BACK_new.OVC_REASON_45 = GetSaveValue(chkOVC_REASON_3_1, 4);
                }
                else
                {
                    tbmBID_BACK_new.OVC_REASON_41 = null;
                    tbmBID_BACK_new.OVC_REASON_42 = null;
                    tbmBID_BACK_new.OVC_REASON_43 = null;
                    tbmBID_BACK_new.OVC_REASON_44 = null;
                    tbmBID_BACK_new.OVC_REASON_45 = null;
                }
                tbmBID_BACK_new.OVC_MEMO = txtOVC_MEMO.Text;
                tbmBID_BACK_new.OVC_REMARK = txtOVC_REMARK.Text;
                mpms.TBMBID_BACK.Add(tbmBID_BACK_new);
                mpms.SaveChanges();
                FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmBID_BACK_new.GetType().Name.ToString(), this, "新增");
            }

            //刪除 TBMBID_BACK_ITEM
            /*
            var queryBID_BACK_ITEM_Del =
                from tbmBID_BACK_ITEM_Del in mpms.TBMBID_BACK_ITEM
                where tbmBID_BACK_ITEM_Del.OVC_PURCH.Equals(strOVC_PURCH) && tbmBID_BACK_ITEM_Del.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tbmBID_BACK_ITEM_Del.OVC_DOPEN.Equals(strOVC_DOPEN) && tbmBID_BACK_ITEM_Del.ONB_GROUP == numONB_GROUP
                select tbmBID_BACK_ITEM_Del;
            if (queryBID_BACK_ITEM_Del != null)
            {
                mpms.Entry(queryBID_BACK_ITEM_Del).State = EntityState.Deleted;
                mpms.SaveChanges();    
            }
            */

            if (gvTBM1313.Rows.Count != 0)
            {
                foreach (GridViewRow row in gvTBM1313.Rows)
                {
                    string strOVC_VEN_TITLE, strOVC_VEN_CST;
                    CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                    strOVC_VEN_TITLE = ((Label)row.FindControl("lblOVC_VEN_TITLE")).Text;
                    strOVC_VEN_CST = ((Label)row.FindControl("lblOVC_VEN_CST")).Text;
                    TBMBID_BACK_ITEM tbmBID_BACK_ITEM = mpms.TBMBID_BACK_ITEM.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP && tb.OVC_VEN_TITLE.Equals(strOVC_VEN_TITLE)).FirstOrDefault();
                    if (chk.Checked == true)
                    {
                        if (tbmBID_BACK_ITEM == null)
                        {
                            //新增 TBMBID_BACK_ITEM
                            TBMBID_BACK_ITEM tbmBID_BACK_ITEM_new = new TBMBID_BACK_ITEM
                            {
                                OVC_SN = Guid.NewGuid(),
                                OVC_PURCH = strOVC_PURCH,
                                OVC_PURCH_5 = strOVC_PURCH_5,
                                OVC_DOPEN = strOVC_DOPEN,
                                ONB_GROUP = numONB_GROUP,
                                OVC_VEN_TITLE = strOVC_VEN_TITLE,
                                OVC_VEN_CST = strOVC_VEN_CST,
                            };
                            mpms.TBMBID_BACK_ITEM.Add(tbmBID_BACK_ITEM_new);
                            mpms.SaveChanges();
                        }
                    }
                    if (chk.Checked == false)
                    {
                        if (tbmBID_BACK_ITEM != null)
                        {
                            mpms.Entry(tbmBID_BACK_ITEM).State = EntityState.Deleted;
                            mpms.SaveChanges();
                        }
                    }
                }
            }
            FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), "TBMBID_BACK_ITEM", this, "更新");
            FCommon.AlertShow(PnMessage, "success", "系統訊息", "存檔成功！");
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            //點擊 回開標紀錄作業編輯
            string send_url = "~/pages/MPMS/D/MPMS_D18_1.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_DOPEN=" + strOVC_DOPEN +
                              "&ONB_TIMES=" + numONB_TIMES + "&ONB_GROUP=" + numONB_GROUP;
            Response.Redirect(send_url);
        }

        protected void btnReturnR_Click(object sender, EventArgs e)
        {
            //點擊 回開標紀錄選擇畫面
            string send_url = "~/pages/MPMS/D/MPMS_D18.aspx?OVC_PURCH=" + strOVC_PURCH;
            Response.Redirect(send_url);
        }


        protected void btnReturnM_Click(object sender, EventArgs e)
        {
            //點擊 回主流程畫面
            string send_url = "~/pages/MPMS/D/MPMS_D14_1.aspx?OVC_PURCH=" + strOVC_PURCH;
            Response.Redirect(send_url);
        }

        protected void lbtnToWordD18_6_Click(object sender, EventArgs e)
        {
            //開標結果通知押標金投標文件退還紀錄表
            string path = OutputWordD18_6();
            string fileName = strOVC_PURCH + "-開標結果通知押標金投標文件退還紀錄表.docx";
            FileInfo fileInfo = new FileInfo(path);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            System.IO.File.Delete(path);
            Response.End();
        }

        protected void lbtnToWordD18_6_odt_Click(object sender, EventArgs e)
        {
            string path = OutputWordD18_6();
            string file_temp = OutputWordD18_6().Replace(".docx", ".odt");
            string fileName = strOVC_PURCH + "-開標結果通知押標金投標文件退還紀錄表.odt";
            FCommon.WordToOdt(this, path, file_temp, fileName);
        }



        #region 副程式
        private bool IsOVC_DO_NAME()
        {
            string strErrorMsg = "";
            string strUSER_ID = Session["userid"] == null ? "" : Session["userid"].ToString();
            string strUserName, strDept = "";
            DataTable dt = new DataTable();
            if (strUSER_ID.Length > 0)
            {
                ACCOUNT ac = new ACCOUNT();
                ac = gm.ACCOUNTs.Where(tb => tb.USER_ID.Equals(strUSER_ID)).FirstOrDefault();
                if (ac != null)
                {
                    strUserName = ac.USER_NAME.ToString();
                    strDept = ac.DEPT_SN;
                    TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                    if (strOVC_PURCH == "")
                        strErrorMsg = "請選擇購案";
                    else if (tbm1301 == null)
                        strErrorMsg = "查無此購案編號";
                    else
                    {
                        TBM1301_PLAN plan1301 =
                            gm.TBM1301_PLAN.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCHASE_UNIT.Equals(strDept)).FirstOrDefault();

                        TBMRECEIVE_BID tbmRECEIVE_BID =
                            mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_DO_NAME.Equals(strUserName)).FirstOrDefault();
                        if (tbmRECEIVE_BID == null || plan1301 == null)
                            strErrorMsg = "非此購案的承辦人";
                    }

                    if (strErrorMsg != "")
                        FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErrorMsg);
                    else
                    {
                        divForm.Visible = true;
                        return true;
                    }
                }
            }
            divForm.Visible = false;
            return false;
        }



        private void DataImport()
        {
            //將資料庫資料帶出至畫面
            string[] strField = { "OVC_PURCH", "OVC_VEN_CST", "OVC_VEN_TITLE" };
            DataTable dt;
            lblOVC_PURCH_A_5.Text = strOVC_PURCH + strOVC_PUR_AGENCY + strOVC_PURCH_5;
            lblOVC_DOPEN.Text = GetTaiwanDate(strOVC_DOPEN);
            lblONB_GROUP.Text = numONB_GROUP.ToString();

            TBM1303 tbm1303 = new TBM1303();
            tbm1303 = mpms.TBM1303.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP).FirstOrDefault();
            if (tbm1303 != null)
            {
                lblOVC_OPEN_HOUR.Text = tbm1303.OVC_OPEN_HOUR;
                lblOVC_OPEN_MIN.Text = tbm1303.OVC_OPEN_MIN;
                lblOVC_ROOM.Text = tbm1303.OVC_ROOM;
                rdoOVC_BACK_PLACE.Items[0].Text = "第" + lblOVC_ROOM.Text + "開標室";
                rdoOVC_BACK_PLACE.Items[0].Value = "第" + lblOVC_ROOM.Text + "開標室";
                lblOVC_CHAIRMAN.Text = tbm1303.OVC_CHAIRMAN;
            }
            TBM1313 tbm1313_Exist = new TBM1313();
            tbm1313_Exist = mpms.TBM1313.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                        && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP).FirstOrDefault();
            /*TBM1313_ITEM tbm1313_ITEM_Exist = new TBM1313_ITEM();
            tbm1313_ITEM_Exist = mpms.TBM1313_ITEM.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP).FirstOrDefault();
            */
            if (tbm1313_Exist != null) //&& tbm1313_ITEM_Exist != null)
            {
                string strOVC_VEN_TITLE = "";

                var queryTbm1313 =
                (from tbm1313 in mpms.TBM1313
                 where tbm1313.OVC_PURCH.Equals(strOVC_PURCH) && tbm1313.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                             && tbm1313.OVC_DOPEN.Equals(strOVC_DOPEN) && tbm1313.ONB_GROUP == numONB_GROUP
                 orderby tbm1313.ONB_GROUP
                 select new
                 {
                     OVC_PURCH = tbm1313.OVC_PURCH,
                     OVC_VEN_CST = tbm1313.OVC_VEN_CST,
                     OVC_VEN_TITLE = tbm1313.OVC_VEN_TITLE,
                     IsCheck = "",
                 }).Distinct();

                dt = CommonStatic.LinqQueryToDataTable(queryTbm1313);
                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        strOVC_VEN_TITLE = row["OVC_VEN_TITLE"].ToString();
                        TBMBID_BACK_ITEM tbmBID_BACK_ITEM = mpms.TBMBID_BACK_ITEM.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP && tb.OVC_VEN_TITLE.Equals(strOVC_VEN_TITLE)).FirstOrDefault();
                        if (tbmBID_BACK_ITEM != null)
                        {
                            row["IsCheck"] = "true";
                        }
                    }
                }
                FCommon.GridView_dataImport(gvTBM1313, dt);
            }
            else
            {
                DataTable dt_strField = new DataTable();
                foreach (string value in strField)
                {
                    dt_strField.Columns.Add(value);
                }
                gvTBM1313.ShowHeaderWhenEmpty = true;
                gvTBM1313.EmptyDataText = "截標審查作業未完成，無廠商資料！";
                gvTBM1313.EmptyDataRowStyle.HorizontalAlign = HorizontalAlign.Center;
                gvTBM1313.EmptyDataRowStyle.VerticalAlign = VerticalAlign.Middle;
                gvTBM1313.DataSource = dt_strField;
                gvTBM1313.DataBind();
            }


            TBMBID_BACK tbmBID_BACK = new TBMBID_BACK();
            tbmBID_BACK = mpms.TBMBID_BACK.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP).FirstOrDefault();

            if (tbmBID_BACK != null)
            {
                string[] strOVC_REASON_4_1 = { tbmBID_BACK.OVC_REASON_41, tbmBID_BACK.OVC_REASON_42, tbmBID_BACK.OVC_REASON_43 };
                string[] strOVC_REASON_4_2 = { tbmBID_BACK.OVC_REASON_44, tbmBID_BACK.OVC_REASON_45 };
                txtOVC_DBACK.Text = tbmBID_BACK.OVC_DBACK;
                rdoOVC_BACK_PLACE.SelectedValue = tbmBID_BACK.OVC_BACK_PLACE;
                chkOVC_REASON_1.Checked = tbmBID_BACK.OVC_REASON_1 != null ? true : false;
                chkOVC_REASON_2.Checked = tbmBID_BACK.OVC_REASON_2 != null ? true : false;
                if (tbmBID_BACK.OVC_REASON_2 != null)
                {
                    if (tbmBID_BACK.OVC_REASON_2.Contains("開標")) chkOVC_REASON_2_1.Items[0].Selected = true;
                    if (tbmBID_BACK.OVC_REASON_2.Contains("決標")) chkOVC_REASON_2_1.Items[1].Selected = true;
                    if (tbmBID_BACK.OVC_REASON_2.Contains("押標金")) chkOVC_REASON_2_2.Items[0].Selected = true;
                    if (tbmBID_BACK.OVC_REASON_2.Contains("投標文件")) chkOVC_REASON_2_2.Items[1].Selected = true;
                }
                if (tbmBID_BACK.OVC_REASON_41 != null || tbmBID_BACK.OVC_REASON_42 != null || tbmBID_BACK.OVC_REASON_43 != null || tbmBID_BACK.OVC_REASON_44 != null || tbmBID_BACK.OVC_REASON_45 != null)
                    chkOVC_REASON_3.Checked = true;
                if (tbmBID_BACK.OVC_REASON_41 != null) chkOVC_REASON_3_1.Items[0].Selected = true;
                if (tbmBID_BACK.OVC_REASON_42 != null) chkOVC_REASON_3_1.Items[1].Selected = true;
                if (tbmBID_BACK.OVC_REASON_43 != null) chkOVC_REASON_3_1.Items[2].Selected = true;
                if (tbmBID_BACK.OVC_REASON_44 != null) chkOVC_REASON_3_1.Items[3].Selected = true;
                if (tbmBID_BACK.OVC_REASON_45 != null) chkOVC_REASON_3_1.Items[4].Selected = true;
                chkOVC_REASON_4.Checked = tbmBID_BACK.OVC_REASON_3 != null ? true : false;
                if (tbmBID_BACK.OVC_REASON_3 != null)
                {
                    if (tbmBID_BACK.OVC_REASON_3.Contains("押標金")) chkOVC_REASON_4_1.Items[0].Selected = true;
                    if (tbmBID_BACK.OVC_REASON_3.Contains("企劃書乙式份")) chkOVC_REASON_4_1.Items[1].Selected = true;
                    if (tbmBID_BACK.OVC_REASON_3.Contains("建議書乙式份")) chkOVC_REASON_4_1.Items[2].Selected = true;
                }
                if (tbmBID_BACK.OVC_REASON_5 != null)
                {
                    chkOVC_REASON_5.Checked = tbmBID_BACK.OVC_REASON_5.Contains("貴廠商於得標後已依規定繳足額履約保證金，依規定退回貴廠商押標金。") ? true : false;
                    chkOVC_REASON_6.Checked = tbmBID_BACK.OVC_REASON_5.Contains("本案開標結果通知單。") ? true : false;
                }
                txtOVC_MEMO.Text = tbmBID_BACK.OVC_MEMO;
                txtOVC_REMARK.Text = tbmBID_BACK.OVC_REMARK;
            }
        }

        private void SetCheckeds(CheckBoxList list, string[] strCheckeds)
        {
            foreach (string strSelected in strCheckeds)
            {
                System.Web.UI.WebControls.ListItem newItme = (System.Web.UI.WebControls.ListItem)list.Items.FindByValue(strSelected);
                if (newItme != null)
                    newItme.Selected = true;
            }
        }



        private string GetSaveValue(ListControl list, int i)
        {
            if (list.Items[i].Selected)
                return list.Items[i].Value;
            else
                return "";
        }




        private string GetTaiwanDate(string strDate)
        {
            //西元年轉民國年
            if (strDate != null && strDate != "")
            {
                DateTime datetime = Convert.ToDateTime(strDate);
                CultureInfo info = new CultureInfo("zh-TW");
                TaiwanCalendar twC = new TaiwanCalendar();
                info.DateTimeFormat.Calendar = twC;
                return datetime.ToString("yyy年MM月dd日", info);
            }
            return strDate;
        }

        #endregion


        #region 輸出Word檔
        private string OutputWordD18_6()
        {
            var targetPath = Server.MapPath("~/WordPDFprint/");
            string fileName = strOVC_PURCH + "-開標結果通知押標金投標文件退還紀錄表.docx";
            File.Delete(targetPath + fileName);
            File.Copy(targetPath + "D18_6-開標結果通知押標金投標文件退還紀錄表.docx", targetPath + fileName);
            var valuesToFill = new TemplateEngine.Docx.Content();

            TBM1301 tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                TBMRECEIVE_BID tbmRECEIVE_BID = mpms.TBMRECEIVE_BID.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
                strOVC_PURCH_5 = tbmRECEIVE_BID == null ? "" : tbmRECEIVE_BID.OVC_PURCH_5;
                valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", tbm1301.OVC_PURCH + tbm1301.OVC_PUR_AGENCY + strOVC_PURCH_5));
            }

            TBMBID_BACK tbmBID_BACK = mpms.TBMBID_BACK.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP).FirstOrDefault();
            if (tbmBID_BACK != null)
            {
                valuesToFill.Fields.Add(new FieldContent("OVC_DBACK", tbmBID_BACK.OVC_DBACK == null ? "" : GetTaiwanDate(tbmBID_BACK.OVC_DBACK)));
                valuesToFill.Fields.Add(new FieldContent("OVC_BACK_PLACE", tbmBID_BACK.OVC_BACK_PLACE == null ? "" : tbmBID_BACK.OVC_BACK_PLACE));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_1", tbmBID_BACK.OVC_REASON_1 == null ? "□" : "■"));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_2", tbmBID_BACK.OVC_REASON_2 == null ? "□" : "■"));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_2_11", tbmBID_BACK.OVC_REASON_2 == null ? "□" : (tbmBID_BACK.OVC_REASON_2.Contains("開標") ? "■" : "□")));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_2_12", tbmBID_BACK.OVC_REASON_2 == null ? "□" : (tbmBID_BACK.OVC_REASON_2.Contains("決標") ? "■" : "□")));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_2_21", tbmBID_BACK.OVC_REASON_2 == null ? "□" : (tbmBID_BACK.OVC_REASON_2.Contains("押標金") ? "■" : "□")));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_2_22", tbmBID_BACK.OVC_REASON_2 == null ? "□" : (tbmBID_BACK.OVC_REASON_2.Contains("投標文件") ? "■" : "□")));
                if (tbmBID_BACK.OVC_REASON_41 != null || tbmBID_BACK.OVC_REASON_42 != null || tbmBID_BACK.OVC_REASON_43 != null || tbmBID_BACK.OVC_REASON_44 != null || tbmBID_BACK.OVC_REASON_45 != null)
                    valuesToFill.Fields.Add(new FieldContent("OVC_REASON_4", "■"));
                else
                    valuesToFill.Fields.Add(new FieldContent("OVC_REASON_4", "□"));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_4_1", tbmBID_BACK.OVC_REASON_41 == null ? "□" : "■"));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_4_2", tbmBID_BACK.OVC_REASON_42 == null ? "□" : "■"));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_4_3", tbmBID_BACK.OVC_REASON_43 == null ? "□" : "■"));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_4_4", tbmBID_BACK.OVC_REASON_44 == null ? "□" : "■"));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_4_5", tbmBID_BACK.OVC_REASON_45 == null ? "□" : "■"));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_3", tbmBID_BACK.OVC_REASON_3 == null ? "□" : "■"));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_3_1", tbmBID_BACK.OVC_REASON_3 == null ? "□" : (tbmBID_BACK.OVC_REASON_3.Contains("押標金") ? "■" : "□")));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_3_2", tbmBID_BACK.OVC_REASON_3 == null ? "□" : (tbmBID_BACK.OVC_REASON_3.Contains("企劃書乙式份") ? "■" : "□")));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_3_3", tbmBID_BACK.OVC_REASON_3 == null ? "□" : (tbmBID_BACK.OVC_REASON_3.Contains("建議書乙式份") ? "■" : "□")));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_5", tbmBID_BACK.OVC_REASON_5 == null ? "□" : (tbmBID_BACK.OVC_REASON_5.Contains("貴廠商於得標後已依規定繳足額履約保證金，依規定退回貴廠商押標金。") ? "■" : "□")));
                valuesToFill.Fields.Add(new FieldContent("OVC_REASON_6", tbmBID_BACK.OVC_REASON_5 == null ? "□" : (tbmBID_BACK.OVC_REASON_5.Contains("本案開標結果通知單。") ? "■" : "□")));
                valuesToFill.Fields.Add(new FieldContent("chkOVC_MEMO", tbmBID_BACK.OVC_MEMO == null ? "□" : "■"));
                valuesToFill.Fields.Add(new FieldContent("OVC_MEMO", tbmBID_BACK.OVC_MEMO == null ? "" : tbmBID_BACK.OVC_MEMO));
                valuesToFill.Fields.Add(new FieldContent("OVC_REMARK", tbmBID_BACK.OVC_REMARK == null ? "" : tbmBID_BACK.OVC_REMARK));
            }


            using (var outputDocument = new TemplateProcessor(targetPath + fileName).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }
            string path = targetPath + fileName;
            return path;
        }

        #endregion


    }
}