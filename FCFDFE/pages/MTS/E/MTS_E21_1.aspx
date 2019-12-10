<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E21_1.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E21_1" %>
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
                    提單運費審核與修訂作業
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">提單編號</asp:Label>
                                    </td>
                                    <td colspan="7">
                                        <asp:TextBox ID="txtOvcBldNo" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server" Text="(本作業僅顯示「已審核」並「未帶入運費檔」之提單)" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">接轉單位</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="drpOvcTranserDeptCde" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>&nbsp;&nbsp;
                                        <asp:Label ID="lblOvcTranserDeptCde" CssClass="control-label" runat="server" Text="dispatchLabel" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">是否計費</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="drpOvcIsCharge" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">船機名稱</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtOvcShipName" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">航次</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtOvcVoyage" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">出口提單</asp:Label>
                                    </td>
                                    <td colspan="7">
                                        <asp:RadioButtonList ID="rdoOvcBld" CssClass="radioButton position-left" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem>所有提單</asp:ListItem>
                                            <asp:ListItem>非Collect提單</asp:ListItem>
                                            <asp:ListItem>Collect提單</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnQuery" cssclass="btn-success btnw2" runat="server" Text="查詢" /><br /><br />
                                <asp:Label ID="lblGryMessage" CssClass="control-label text-red" text="grymessageLabel" runat="server"></asp:Label><br />
                            </div>
                            <asp:GridView ID="GV_TBGMT_BLD" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_BLD_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="" HeaderText="海運提單編號" />
                                    <asp:BoundField DataField="" HeaderText="承運航商" />
                                    <asp:BoundField DataField="" HeaderText="海運" />
                                    <asp:BoundField DataField="" HeaderText="船機名稱" />
                                    <asp:BoundField DataField="" HeaderText="船機班次" />
                                    <asp:BoundField DataField="" HeaderText="啟運日期" />
                                    <asp:BoundField DataField="" HeaderText="啟運港埠" />
                                    <asp:BoundField DataField="" HeaderText="實際抵運日期" />
                                    <asp:BoundField DataField="" HeaderText="抵運港埠" />
                                    <asp:BoundField DataField="" HeaderText="運費" />
                                    <asp:BoundField DataField="" HeaderText="運費幣別" />
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnOther" CssClass="btn-success" Text="管理" CommandName="按鈕屬性" runat="server"/>
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
