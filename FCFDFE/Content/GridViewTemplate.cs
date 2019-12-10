using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FCFDFE.Content
{
    public class GridViewTemplate : ITemplate
    { //半成品
        private DataControlRowType templateType; //定義 DataControlRowType 變數
        private string columnName; //定義 columnName 變數
        private object control; //定義 控制項
        private EventHandler eventClick; //定義 Click事件

        ///<summary>建構函式</summary>
        ///<param name="InType">資料列型態。</param>
        ///<param name="InColName">欄位名稱</param>
        public GridViewTemplate(DataControlRowType InType, string InColName, object InCtrl = null, EventHandler InEventClick = null)
        {
            templateType = InType;
            columnName = InColName;
            control = InCtrl;
            eventClick = InEventClick;
        }

        #region ITemplate 成員
        public void InstantiateIn(Control container)
        {
            //若為標頭資料列
            switch (templateType)
            {
                case DataControlRowType.Header:
                    if (control == null) //如果沒有控制項，加入粗體文字
                    {
                        Literal oLiteral = new Literal();
                        oLiteral.Text = $"<B>{ columnName }</B>";
                        container.Controls.Add(oLiteral);
                    }
                    else //否則加入控制項
                        container.Controls.Add(control as Control);
                    break;
                case DataControlRowType.DataRow:
                    if (control == null) //如果沒有控制項，加入文字
                    {
                        Label oLabel = new Label();
                        oLabel.DataBinding += new EventHandler(data_DataBinding);
                        container.Controls.Add(oLabel);
                    }
                    else
                    {
                        if(control is LinkButton)
                        {
                            LinkButton oldLinkButton = control as LinkButton;
                            LinkButton newLinkButton = new LinkButton(); //複製一個新的Control
                            //newLinkButton = control as LinkButton;
                            newLinkButton.ID = oldLinkButton.ID;
                            newLinkButton.CommandName = oldLinkButton.CommandName;
                            //if (eventClick != null)
                                newLinkButton.Click += eventClick;
                            newLinkButton.DataBinding += new EventHandler(data_DataBinding);
                            container.Controls.Add(newLinkButton); //加入新的控制項，否則只有最後一行有，因為Control為Reference型別
                        }
                    }
                    break;
            }
        }
        private void data_DataBinding(object sender, EventArgs e)
        {
            if(sender is Label)
            {
                Label oLabel = sender as Label;
                GridViewRow oGridViewRow = (GridViewRow)oLabel.NamingContainer;
                if (oGridViewRow == null || oGridViewRow.DataItem == null)
                    oLabel.Text = "繫結不到";
                else
                    oLabel.Text = DataBinder.Eval(oGridViewRow.DataItem, columnName).ToString();
            }
            else if(sender is LinkButton)
            {
                LinkButton oLinkButton = sender as LinkButton;
                GridViewRow oGridViewRow = (GridViewRow)oLinkButton.NamingContainer;
                if (oGridViewRow == null || oGridViewRow.DataItem == null)
                    oLinkButton.Text = "繫結不到";
                else
                    oLinkButton.Text = DataBinder.Eval(oGridViewRow.DataItem, columnName).ToString();
            }
        }
        #endregion
    }
}