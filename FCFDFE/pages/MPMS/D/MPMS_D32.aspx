<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D32.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D32" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title text-blue">
                    <!--標題-->
                    開標結果（<asp:Label ID="lblType" runat="server" ></asp:Label>
                    ）通知作業               
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" id="divForm" style="border: solid 2px;" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <table class="table table-bordered text-center tr1">
                            <tr>
                                <td style="width: 15%">
                                    <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td style="width: 15%">
                                    <asp:Label CssClass="control-label" runat="server">購案名稱</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    <asp:Label CssClass="control-label" runat="server">開標時間</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_DOPEN_tw" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_DOPEN" CssClass="control-label" Visible="false" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">投標段次</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_BID_TIMES" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    <asp:Label CssClass="control-label" runat="server">決標原則</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_BID" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">招標方式</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_PUR_ASS_VEN_CODE" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    <asp:Label CssClass="control-label" runat="server">開標結果</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_RESULT" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">得標廠商</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_VEN_TITLE" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>

                        <div class="title text-center text-red">
                            <asp:Label CssClass="control-label" runat="server">先選擇欲通知之參標廠商</asp:Label>
                        </div>
                        <div class=" text-center">
                            <asp:Label CssClass="control-label text-red" runat="server">(讀取投標廠商報價比較資料)</asp:Label>
                        </div>

                        <%--<table class="table table-bordered text-left">
                            <tr>
                                <th>
                                    <asp:Label CssClass="control-label" runat="server">選擇</asp:Label></th>
                                <th>
                                    <asp:Label CssClass="control-label text-red" runat="server">投標廠商名稱</asp:Label></th>
                                <th>
                                    <asp:Label ID="Label3" CssClass="control-label" runat="server">廠商地址</asp:Label></th>
                                <th>
                                    <asp:Label ID="Label1" CssClass="control-label" runat="server">最後通知時間</asp:Label></th>
                                <th>
                                    <asp:Label ID="Label2" CssClass="control-label" runat="server">承辦人</asp:Label></th>
                            </tr>
                        </table>--%>
                        <asp:GridView ID="GV_info" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_info_PreRender" DataKeyNames="OVC_PURCH" runat="server">
                        <Columns>
                            
                            <asp:TemplateField HeaderText="選擇">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" CssClass="radioButton" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="廠商編號" DataField="OVC_VEN_CST" />
                            <asp:BoundField HeaderText="開標時間" DataField="OVC_DOPEN"/>
                            <asp:BoundField HeaderText="投標廠商名稱" DataField="OVC_VEN_TITLE" />
                            <asp:TemplateField HeaderText="場商地址">
                                <ItemTemplate>
                                     <asp:TextBox id="txtaddress" CssClass="tb tb-l" Text='<%# Eval("OVC_VEN_ADDRESS") %>'  runat="server"></asp:TextBox></td>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="最後通知時間" DataField="OVC_NOTICE_TIME" />
                            <asp:BoundField HeaderText="承辦人" DataField="OVC_NAME" />
                        </Columns>
                    </asp:GridView>

                        <div class="text-center" style="margin-bottom: 10px;">
                            <asp:Button ID="btnSAll" OnClick="btnSAll_Click" CssClass="btn-default btnw6" runat="server" Text="選擇全部" />
                            <asp:Button ID="btnClear" OnClick="btnClear_Click" CssClass="btn-default btnw6" runat="server" Text="清除全部" />
                        </div>
                        <div class="text-center">
                            <p>↓</p>
                        </div>
                        <div class="text-center" style="margin-bottom: 10px;">
                            <asp:Button ID="btnPrint" OnClick="btnPrint_Click" CssClass="btn-default" runat="server" Text="開標結果通知函(稿)預覽列印.pdf" />
                            <asp:Button ID="btnPrint_odt" OnClick="btnPrint_odt_Click" CssClass="btn-default" runat="server" Text=".odt" />
                        </div>

                        <div class="text-center">
                            <asp:Button ID="btnReturnMod" OnClick="btnReturnMod_Click" CssClass="btn-default " runat="server" Text="回採購開標結果作業編輯畫面" />
                           <%-- <asp:Button ID="btnReturnS" OnClick="btnReturnS_Click" CssClass="btn-default " runat="server" Text="回採購開標結果選擇畫面" />
                            <asp:Button ID="btnReturnM" OnClick="btnReturnM_Click" CssClass="btn-default " runat="server" Text="回主流程畫面" />--%>
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
