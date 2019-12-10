<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E31.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E31" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        tr td:nth-child(2n) {
            text-align: left;
        }
    </style>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    免稅紀錄
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->

                            <table class="table table-bordered text-center" style="margin-bottom: 0px;">
                                <tr>
                                    <th colspan="8" style="background: red;">
                                        <asp:Label ForeColor="#ffff37" Font-Size="X-Large" CssClass="control-label" runat="server">請注意！未輸入資料或存檔無法列印各項報表！！</asp:Label></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server">GH0613L008PE</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">採購單位</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblOVC_AGNT_IN" CssClass="control-label" runat="server">GH0613L008PE</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">交貨批次</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblONB_DELIVERY_TIMES" CssClass="control-label" runat="server">GH0613L008PE</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">申請次數</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_NO" CssClass="tb text-red tb-s" runat="server">1</asp:TextBox></td>
                                </tr>
                            </table>
                            <table class="table table-bordered text-center" style="margin-top: 0px;">
                                <tr>
                                    <th colspan="4">
                                        <asp:Label ForeColor="red" Font-Size="X-Large" CssClass="control-label" runat="server">免貨物稅</asp:Label></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">申請書</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_APPLY" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">核定機關</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_GOOD_NAPPROVE" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">貨品名稱及其規格</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_GOOD_DESC" TextMode="MultiLine" Rows="3" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">核定意見</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_MEMO" TextMode="MultiLine" Rows="1" CssClass="textarea tb-full" runat="server"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">購買數量</asp:Label>
                                        
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox3" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                       
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">發文日期文號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox4" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">經費來源</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_GOOD_BUDGET" CssClass="tb tb-l" runat="server"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">稽徵機關</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_GOOD_AUDIT" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">貨品用途</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_GOOD_USE" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                    <td rowspan="2">
                                        <asp:Label CssClass="control-label" runat="server">核准意見</asp:Label>
                                    </td>
                                    <td rowspan="2">
                                        <asp:TextBox ID="txtOVC_APPROVE_DESC" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">供應廠商</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_EXPORT_VENDOR" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">使用單位</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_GOOD_NSECTION" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">發文日期文號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_DAPPLY" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnReturn" CssClass="btn-default btnw4" OnClick="btnReturn_Click" runat="server" Text="回上一頁" />
                                <asp:Button ID="btnReturnM" CssClass="btn-default btnw4" OnClick="btnReturnM_Click" runat="server" Text="回主流程" />
                                <asp:Button  ID="btnDel" CssClass="btn-default btw2" OnClick="btnDel_Click" OnClientClick="if (confirm('確定要刪除資料?') == false) return false;" runat="server" Text="刪除" />
                                <asp:Button ID="btnSave" CssClass="btn-default btnw2" OnClick="btnSave_Click" runat="server" Text="存檔" />
                                <br /><br />
                                <asp:LinkButton ID="LinkButton1" OnClick="LinkButton1_Click" runat="server">列印申請免稅購買軍用貨品核定單.doc</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton2" OnClick="LinkButton2_Click" runat="server">列印申請免稅購買軍用貨品核定單.pdf</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton3" OnClick="LinkButton3_Click" runat="server">列印申請免稅購買軍用貨品核定單.odt</asp:LinkButton>
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
