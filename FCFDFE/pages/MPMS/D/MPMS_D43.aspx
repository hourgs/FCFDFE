<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D43.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D43" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                    <!--標題--><asp:Label ID="lblPurchNum" runat="server"></asp:Label>審查紀錄查詢
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:GridView ID="GV_Query_PLAN" CssClass=" table data-table table-striped border-top " 
                                AutoGenerateColumns="false" OnPreRender="GV_Query_PLAN_PreRender"
                                  OnRowDataBound="GV_Query_PLAN_RowDataBound" runat="server">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="10%" HeaderText="審查次數" >
                                        <ItemTemplate>
                                            <asp:Label CssClass="control-label" ID="lblONB_CHECK_TIMES" Text='<%#Eval("ONB_CHECK_TIMES")%>' runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label text-red" ID="lblAUDITSTATUS" Text='<%#Eval("OVC_CHECK_OK")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:BoundField HeaderText="分派日" DataField="OVC_DRECEIVE" />
                                    <asp:TemplateField HeaderText="聯審單位" >
                                        <ItemTemplate>
                                            <asp:Label CssClass="control-label" ID="lblUnit" Text='<%#Eval("ISAUDIT")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="text-center" HeaderText="申購單位逾初審7日、複審5日時限" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblSignDate" runat="server" Visible="false">
                                                <span class="glyphicon glyphicon-warning-sign" style="color:red"></span>
                                            </asp:Label>
                                            <asp:HiddenField ID="hidOVC_CHECK_OK" Value='<%#Bind("OVC_CHECK_OK")%>' runat="server" />
                                            <asp:HiddenField ID="hidOVC_PERMISSION_UPDATE" Value='<%#Bind("OVC_PERMISSION_UPDATE")%>' runat="server" />
                                            <asp:HiddenField ID="hidOVC_DREJECT" Value='<%#Bind("OVC_DREJECT")%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="審查意見" >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btn_PrintComment" Text ="聯審意見" runat="server" Visible="false"></asp:LinkButton>
                                            <asp:LinkButton ID="btn_CommentWithResponse" Text ="聯審意見(含澄覆意見)" runat="server" Visible="false"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                	            </Columns>
	   		               </asp:GridView>
                            <div class="text-center">
                            <asp:Button ID="btnReturn" OnClick="btnReturn_Click" CssClass="btn-warning btnw4" runat="server" Text="回上一頁" />
                            </div>
                    </div>
                </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
