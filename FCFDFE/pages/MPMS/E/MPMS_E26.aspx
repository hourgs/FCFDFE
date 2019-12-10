<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E26.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E26" %>

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
                <header class="title">
                    <!--標題-->
                    履約督導紀錄
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-left">
                                <tr>
                                    <th colspan="2" style="background: red;">
                                        <asp:Label ForeColor="#ffff37" Font-Size="X-Large" CssClass="control-label" runat="server">請注意！未輸入資料或存檔無法列印各項報表！！</asp:Label></th>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">購案編號：</asp:Label><asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server">GH0613L008PE</asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server">交貨批次：</asp:Label><asp:TextBox ID="txtONB_SHIP_TIMES" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Label CssClass="control-label" runat="server">履約督導次數：</asp:Label><asp:TextBox ID="TextBox1" CssClass="tb tb-m" runat="server">1</asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">計畫申購單位：</asp:Label><asp:TextBox ID="txtOVC_PUR_AGENCY" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">購案名稱：</asp:Label><asp:TextBox ID="txtOVC_PURCH" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">契約金額：</asp:Label><asp:TextBox ID="txtONB_MCONTRACT" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">簽約日期：</asp:Label><asp:TextBox ID="txtOVC_DCONTRACT" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">決標日期：</asp:Label><asp:TextBox ID="txtOVC_DBID" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">履約期限：</asp:Label><asp:TextBox ID="txtOVC_PERFORMANCE_LIMIT" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">廠商地址：</asp:Label><asp:TextBox ID="txtOVC_VEN_ADDRESS" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label position-left" runat="server">履約督導日期：</asp:Label><!--前方標籤文字，跟日期同一行需使用"position-left"之class-->
                                        <!--↓日期套件↓-->
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <div class="input-append datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                    <asp:TextBox ID="txtOVC_DAUDIT" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                    <div class="add-on"><i class="icon-calendar"></i></div>
                                                </div>
                                                <asp:Button ID="btnClear" CssClass="btn-default btnw4" OnClick="btnClear_Click" runat="server" Text="清除日期" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">廠商履約地址：</asp:Label><asp:TextBox ID="txtOVC_PERFORMANCE_PLACE" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label position-left" runat="server">依據：</asp:Label><!--前方標籤文字，跟日期同一行需使用"position-left"之class-->
                                        <!--↓日期套件↓-->
                                        <asp:UpdatePanel UpdateMode="Conditional" style="display:inline;" runat="server">
                                            <ContentTemplate>
                                                <div class="input-append datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                    <asp:TextBox ID="txtOVC_DNOTICE" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                    <div class="add-on"><i class="icon-calendar"></i></div>
                                                </div>
                                                <asp:Button ID="Button1" CssClass="btn-default btnw4" OnClick="Button1_Click" runat="server" Text="清除日期" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:TextBox ID="txtOVC_INOTICE" CssClass="tb tb-l" runat="server"></asp:TextBox><asp:Label CssClass="control-label" runat="server">號通知單</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-blue" runat="server">所見情形：</asp:Label><asp:TextBox ID="txtOVC_DESC" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            
                            <div class="text-center">
                                <asp:Button  ID="btnReturn" CssClass="btn-default btnw4" OnClick="btnReturn_Click" runat="server" Text="回上一頁" />
                                <asp:Button ID="btnReturnM" CssClass="btn-default btnw4" OnClick="btnReturnM_Click" runat="server" Text="回主流程" />
                                <asp:Button ID="btnSave" CssClass="btn-default btnw2" OnClick="btnSave_Click" runat="server" Text="存檔" />
                                <br /><br />
                                <asp:LinkButton ID="LinkButton1" OnClick="LinkButton1_Click" runat="server">列印履約督導紀錄表.doc</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton2" OnClick="LinkButton2_Click" runat="server">列印履約督導紀錄表.pdf</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton3" OnClick="LinkButton3_Click" runat="server">列印履約督導紀錄表.odt</asp:LinkButton>
                            </div>
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
