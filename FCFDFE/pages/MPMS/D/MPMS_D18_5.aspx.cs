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
using System.Collections.Generic;
using System.IO;
using TemplateEngine.Docx;

namespace FCFDFE.pages.MPMS.D
{
    public partial class MPMS_D18_5 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        GMEntities gm = new GMEntities();
        MPMSEntities mpms = new MPMSEntities();
        string strOVC_PURCH, strOVC_PURCH_5, strOVC_PUR_AGENCY, strOVC_DOPEN;
        short numONB_TIMES, numONB_GROUP;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (FCommon.getAuth(this))
            {
                //設置readonly屬性
                FCommon.Controls_Attributes("readonly", "true", txtOVC_TIME);

                if (Request.QueryString["OVC_PURCH"] == null || Request.QueryString["OVC_DOPEN"] == null || Request.QueryString["ONB_GROUP"] == null)
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
                    strOVC_PUR_AGENCY = tbm1301.OVC_PUR_AGENCY;

                    if (IsOVC_DO_NAME() && !IsPostBack)
                        DataImport();
                }
            }
        }


        protected void gvTBMBID_DOC_LOG_PreRender(object sender, EventArgs e)
        {
            bool hasRows = false;
            if (ViewState["hasRows"] != null)
            {
                hasRows = Convert.ToBoolean(ViewState["hasRows"]);
            }
            FCommon.GridView_PreRenderInit(sender, hasRows);
        }


        protected void gvTBMBID_DOC_LOG_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((WebControl)e.CommandSource).NamingContainer;
            int gvrIndex = gvr.RowIndex;
            string strOVC_RECEIVE_UNIT = ((Label)gvTBMBID_DOC_LOG.Rows[gvrIndex].FindControl("lblOVC_RECEIVE_UNIT")).Text;

            switch (e.CommandName)
            {
                case "Change":
                    TBMBID_DOC_LOG tbmBID_DOC_LOG =
                        mpms.TBMBID_DOC_LOG.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                    && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP && tb.OVC_RECEIVE_UNIT.Equals(strOVC_RECEIVE_UNIT)).FirstOrDefault();
                    if (tbmBID_DOC_LOG != null)
                    {
                        txtOVC_RECEIVE_UNIT.Text = tbmBID_DOC_LOG.OVC_RECEIVE_UNIT;
                        txtOVC_TITLE.Text = tbmBID_DOC_LOG.OVC_TITLE;
                        txtOVC_NAME.Text = tbmBID_DOC_LOG.OVC_NAME;
                        txtOVC_TIME.Text = tbmBID_DOC_LOG.OVC_TIME;
                        txtOVC_REMARK.Text = tbmBID_DOC_LOG.OVC_REMARK;
                    }
                    break;

                case "Del":
                    TBMBID_DOC_LOG tbmBID_DOC_LOG_Del =
                        mpms.TBMBID_DOC_LOG.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                    && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP && tb.OVC_RECEIVE_UNIT.Equals(strOVC_RECEIVE_UNIT)).FirstOrDefault();
                    if (tbmBID_DOC_LOG_Del != null)
                    {
                        mpms.Entry(tbmBID_DOC_LOG_Del).State = EntityState.Deleted;
                        mpms.SaveChanges();
                        FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmBID_DOC_LOG_Del.GetType().Name.ToString(), this, "刪除");
                        FCommon.AlertShow(PnMessage, "success", "系統訊息", "刪除成功！");
                        DataImport();
                    }
                    break;

                default:
                    break;
            }
        }

        protected void drpOVC_RECEIVE_UNIT_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOVC_RECEIVE_UNIT.Text = drpOVC_RECEIVE_UNIT.SelectedValue;
        }




        #region Button Click

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strErrorMsg = "";
            short? numMaxONB_NO = 1;
            TBMBID_DOC_LOG MaxONB_NO =
                mpms.TBMBID_DOC_LOG.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP)
                                .OrderByDescending(tb => tb.ONB_NO).Take(1).FirstOrDefault();
            if (MaxONB_NO != null)
                numMaxONB_NO = (short)(MaxONB_NO.ONB_NO + 1);


            if (txtOVC_RECEIVE_UNIT.Text != "")
            {
                TBMBID_DOC_LOG tbmBID_DOC_LOG = new TBMBID_DOC_LOG();
                tbmBID_DOC_LOG = mpms.TBMBID_DOC_LOG.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                                && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP
                                && tb.OVC_RECEIVE_UNIT.Equals(txtOVC_RECEIVE_UNIT.Text)).FirstOrDefault();
                if (tbmBID_DOC_LOG == null)
                {
                    //新增
                    TBMBID_DOC_LOG tbmBID_DOC_LOG_new = new TBMBID_DOC_LOG
                    {
                        OVC_DOCLOG_SN = Guid.NewGuid(),
                        ONB_NO = numMaxONB_NO,
                        OVC_PURCH = strOVC_PURCH,
                        OVC_PURCH_5 = strOVC_PURCH_5,
                        OVC_DOPEN = strOVC_DOPEN,
                        ONB_GROUP = numONB_GROUP,
                        OVC_RECEIVE_UNIT = txtOVC_RECEIVE_UNIT.Text,
                        OVC_TITLE = txtOVC_TITLE.Text,
                        OVC_NAME = txtOVC_NAME.Text,
                        OVC_TIME = txtOVC_TIME.Text,
                        OVC_REMARK = txtOVC_REMARK.Text,
                    };
                    mpms.TBMBID_DOC_LOG.Add(tbmBID_DOC_LOG_new);
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmBID_DOC_LOG_new.GetType().Name.ToString(), this, "新增");
                    DataImport();
                }
                else
                {
                    //修改
                    tbmBID_DOC_LOG.OVC_TITLE = txtOVC_TITLE.Text;
                    tbmBID_DOC_LOG.OVC_NAME = txtOVC_NAME.Text;
                    tbmBID_DOC_LOG.OVC_TIME = txtOVC_TIME.Text;
                    tbmBID_DOC_LOG.OVC_REMARK = txtOVC_REMARK.Text;
                    mpms.SaveChanges();
                    FCommon.syslog_add(Session["userid"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString(), tbmBID_DOC_LOG.GetType().Name.ToString(), this, "修改");
                    DataImport();
                }
            }
            else
                strErrorMsg = "請輸入單位(名稱)";

            if (strErrorMsg == "")
            {
                FCommon.AlertShow(PnMessage, "success", "系統訊息", "購案開標紀錄分送檔 存檔成功！");
                ClearText();
            }
            else
                FCommon.AlertShow(PnMessage, "danger", "系統訊息", strErrorMsg);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            //點擊 清除
            ClearText();
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
            string send_url = "~/pages/MPMS/D/MPMS_D14_1.aspx?OVC_PURCH=" + strOVC_PURCH + "&OVC_PURCH_5=" + strOVC_PURCH_5;
            Response.Redirect(send_url);
        }

        protected void lbtnToWordD18_5_Click(object sender, EventArgs e)
        {
            //點擊 紀錄發送清冊
            TBM1301 tbm1301 = gm.TBM1301.Where(table => table.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();
            if (tbm1301 != null)
            {
                string outPutfilePath = "";
                string fileName = "D18_5-紀錄發送清冊.docx";
                string TempName = strOVC_PURCH + "-紀錄發送清冊.docx";
                string filePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + fileName));

                //TempName = strOVC_PURCH + "-紀錄發送清冊.docx";
                outPutfilePath = Path.Combine(Server.MapPath("~/WordPDFprint/" + TempName));
                File.Delete(outPutfilePath);
                File.Copy(filePath, outPutfilePath);
                var valuesToFill = new TemplateEngine.Docx.Content();

                valuesToFill.Fields.Add(new FieldContent("OVC_PURCH_A_5", lblOVC_PURCH_A_5.Text));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_IPURCH", tbm1301.OVC_PUR_IPURCH == null ? "" : tbm1301.OVC_PUR_IPURCH));
                valuesToFill.Fields.Add(new FieldContent("OVC_PUR_NSECTION", tbm1301.OVC_PUR_NSECTION == null ? "" : tbm1301.OVC_PUR_NSECTION));

                var tableContent = new TableContent("table");
                var query1302 =
                from tbm1302 in mpms.TBM1302
                where tbm1302.OVC_PURCH.Equals(strOVC_PURCH) && tbm1302.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tbm1302.ONB_GROUP == numONB_GROUP
                select new
                {
                    OVC_PURCH = tbm1302.OVC_PURCH,
                    OVC_VEN_CST = tbm1302.OVC_VEN_CST,
                    OVC_VEN_TITLE = tbm1302.OVC_VEN_TITLE,
                };
                DataTable dt = CommonStatic.LinqQueryToDataTable(query1302);
                if (dt.Rows.Count != 0)
                {
                    int numONB_NO = 5;
                    foreach (DataRow rows in dt.Rows)
                    {
                        tableContent.AddRow(
                            new FieldContent("ONB_NO", numONB_NO.ToString()),//編號
                            new FieldContent("OVC_VEN_TITLE", rows["OVC_VEN_TITLE"] == null ? "" : rows["OVC_VEN_TITLE"].ToString())//得標商
                        );
                        numONB_NO++;
                    }
                }

                valuesToFill.Tables.Add(tableContent);
                using (TemplateProcessor outputDocument = new TemplateProcessor(outPutfilePath).SetRemoveContentControls(true))
                {
                    outputDocument.FillContent(valuesToFill);
                    outputDocument.SaveChanges();
                }


                FileInfo file = new FileInfo(outPutfilePath);
                string wordPath = outPutfilePath;
                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + TempName);
                Response.ContentType = "application/octet-stream";
                //Response.BinaryWrite(buffer);
                Response.WriteFile(wordPath);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.Flush();
                File.Delete(outPutfilePath);
                File.Delete(wordPath);
                Response.End();
            }
        }

        #endregion

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
            DataTable dt;
            lblOVC_PURCH_A_5.Text = strOVC_PURCH + strOVC_PUR_AGENCY + strOVC_PURCH_5;
            lblOVC_DOPEN.Text = GetTaiwanDate(strOVC_DOPEN);
            lblONB_GROUP.Text = numONB_GROUP.ToString();
            SetDrpOVC_RECEIVE_UNIT();

            TBM1303 tbm1303 = new TBM1303();
            tbm1303 = mpms.TBM1303.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH) && tb.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tb.OVC_DOPEN.Equals(strOVC_DOPEN) && tb.ONB_GROUP == numONB_GROUP).FirstOrDefault();
            if (tbm1303 != null)
            {
                lblOVC_OPEN_HOUR.Text = tbm1303.OVC_OPEN_HOUR;
                lblOVC_OPEN_MIN.Text = tbm1303.OVC_OPEN_MIN;
                lblOVC_ROOM.Text = tbm1303.OVC_ROOM;
                lblOVC_CHAIRMAN.Text = tbm1303.OVC_CHAIRMAN;
            }

            var queryBID_DOC_LOG =
                from tbmBID_DOC_LOG in mpms.TBMBID_DOC_LOG
                where tbmBID_DOC_LOG.OVC_PURCH.Equals(strOVC_PURCH) && tbmBID_DOC_LOG.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                            && tbmBID_DOC_LOG.OVC_DOPEN.Equals(strOVC_DOPEN) && tbmBID_DOC_LOG.ONB_GROUP == numONB_GROUP
                orderby tbmBID_DOC_LOG.ONB_GROUP, tbmBID_DOC_LOG.ONB_NO
                select new
                {
                    OVC_PURCH = tbmBID_DOC_LOG.OVC_PURCH,
                    ONB_NO = tbmBID_DOC_LOG.ONB_NO,
                    OVC_RECEIVE_UNIT = tbmBID_DOC_LOG.OVC_RECEIVE_UNIT,
                    OVC_TITLE = tbmBID_DOC_LOG.OVC_TITLE,
                    OVC_NAME = tbmBID_DOC_LOG.OVC_NAME,
                    OVC_TIME = tbmBID_DOC_LOG.OVC_TIME,
                    OVC_REMARK = tbmBID_DOC_LOG.OVC_REMARK,
                };
            dt = CommonStatic.LinqQueryToDataTable(queryBID_DOC_LOG);
            FCommon.GridView_dataImport(gvTBMBID_DOC_LOG, dt);
        }



        private void SetDrpOVC_RECEIVE_UNIT()
        {
            DataTable dt;
            dt = CommonStatic.ListToDataTable(gm.TBM1407.Where(tb => tb.OVC_PHR_CATE.Equals("K5") &&
                tb.OVC_PHR_ID.StartsWith("0")).OrderBy(tb => tb.OVC_PHR_ID).ToList());
            FCommon.list_dataImport(drpOVC_RECEIVE_UNIT, dt, "OVC_PHR_DESC", "OVC_PHR_DESC", true);

            TBM1301 tbm1301 = new TBM1301();
            tbm1301 = gm.TBM1301.Where(tb => tb.OVC_PURCH.Equals(strOVC_PURCH)).FirstOrDefault();

            var queryTBM1313 =
                from tbm1313 in mpms.TBM1313
                where tbm1313.OVC_PURCH.Equals(strOVC_PURCH) && tbm1313.OVC_PURCH_5.Equals(strOVC_PURCH_5)
                   && tbm1313.OVC_DOPEN.Equals(strOVC_DOPEN) && tbm1313.ONB_GROUP == numONB_GROUP
                select new
                {
                    OVC_VEN_CST = tbm1313.OVC_VEN_CST,
                    OVC_VEN_TITLE = tbm1313.OVC_VEN_TITLE,
                };
            DataTable dtTBM1313 = CommonStatic.LinqQueryToDataTable(queryTBM1313);

            foreach (DataRow rows in dtTBM1313.Rows)
            {
                //Dropdownlist項目 加入廠商、申購單位
                drpOVC_RECEIVE_UNIT.Items.Add(rows["OVC_VEN_TITLE"].ToString());
                if (tbm1301 != null)
                    drpOVC_RECEIVE_UNIT.Items.Add(tbm1301.OVC_PUR_NSECTION);
            }
        }




        private string GetOVC_PHR_ID(string strOVC_PHR_ID)
        {
            if (strOVC_PHR_ID != null && strOVC_PHR_ID != "")
            {
                int numOVC_PHR_ID = int.Parse(strOVC_PHR_ID);
                if (numOVC_PHR_ID >= 0 && numOVC_PHR_ID < 9)
                    return strOVC_PHR_ID;
            }
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


        private void ClearText()
        {
            drpOVC_RECEIVE_UNIT.SelectedValue = "";
            txtOVC_RECEIVE_UNIT.Text = "";
            txtOVC_TITLE.Text = "";
            txtOVC_NAME.Text = "";
            txtOVC_TIME.Text = "";
            txtOVC_REMARK.Text = "";
        }

        #endregion

    }
}