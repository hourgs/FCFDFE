<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E24_1.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E24_1" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <script>
         $(document).ready(function () {
             $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
         });
    </script>
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    運費 結報申請表-管理
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:200px;" class="text-center"><asp:Label CssClass="control-label" runat="server">結報申請表編號</asp:Label>
                                    </td>
                                    <td style="width:400px;">
                                        <asp:DropDownList ID="drpOvcInfNo" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">年</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtOvcInfNo" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width:150px;" class="text-center"><asp:Label CssClass="control-label" runat="server">提單編號</asp:Label>
                                    </td>
                                    <td style="width:250px;">
                                        <asp:TextBox ID="txtOvcBldNo" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnQuery" cssclass="btn-success" runat="server" Text="查詢" /> <br /><br />
                            </div>
                            <asp:GridView ID="GV_TBGMT_CINF" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_CINF_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="" HeaderText="結報申請表編號" />
                                    <asp:BoundField DataField="" HeaderText="案由" />
                                    <asp:BoundField DataField="" HeaderText="預算科目及編號" />
                                    <asp:BoundField DataField="" HeaderText="用途別" />
                                    <asp:BoundField DataField="" HeaderText="金額" />
                                    <asp:BoundField DataField="" HeaderText="預算通知單編號" />
                                    <asp:BoundField DataField="" HeaderText="備考" />
                                    <asp:BoundField DataField="" HeaderText="擬辦" />
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnSave" CssClass="btn-warning" Text="修改" CommandName="按鈕屬性"
                                                 runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnDel" CssClass="btn-danger" Text="刪除" CommandName="按鈕屬性"
                                                 runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnOther" CssClass="btn-success" Text="列印" CommandName="按鈕屬性"
                                                 runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->   
                </footer>           
            </section>
        </div>
    </div>
</asp:Content>