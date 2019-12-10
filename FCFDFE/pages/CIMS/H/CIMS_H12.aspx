<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_H12.aspx.cs" Inherits="FCFDFE.pages.CIMS.H.CIMS_H12" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        ul li {
            margin-top: 5px;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <div>稅率稅則查詢</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="container">
                                <div class="row">
                                    <div class="col-md-12">
                                        <ul class="col-md-6">
                                            <li><a href="tax_f1.htm">適用國定稅率第一欄稅率之國家或地區名單</a></li>
                                            
                                            <li><a href="tax_input_rule.htm">經濟部國際貿易局貨品輸入規定</a></li>

                                            <li><a href="CIMS_H12_1.aspx">機動稅率查詢</a></li>

                                        </ul>
                                        <ul class="col-md-6">
                                            <li><a href="tax_f2.htm">適用國定稅率第二欄稅率之國家或地區名單</a></li>
                                            <li><a href="tax_output_rule.htm">經濟部國際貿易局貨品輸出規定</a></li>
                                            <li><a href="tax_spec.htm">稽徵特別規定說明</a></li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-12">
                                        <table class="table table-bordered control-label" style="text-align: center">
                                            <tr>
                                                <th colspan="3">操作(輸入)說明</th>
                                            </tr>
                                            <tr style="background-color: lightseagreen">
                                                <th>查詢欄位</th>
                                                <th>範例(關鍵字)</th>
                                                <th>範例結果</th>
                                            </tr>
                                            <tr>
                                                <th>稅則號別</th>
                                                <td>0101</td>
                                                <td><span class="text-red">0101</span>1000104、02012<span class="text-red">0101</span>09、0202209<span class="text-red">0101</span></td>

                                            </tr>
                                            <tr>
                                                <th>中文貨名、英文貨名</th>
                                                <td>電</td>
                                                <td><span class="text-red">電</span>吉他、其他無線<span class="text-red">電</span>遙控玩具</td>
                                            </tr>
                                            <tr>
                                                <th>輸出規定、輸入規定</th>
                                                <td>442</td>
                                                <td>請查閱經濟部國際貿易局貨品輸出規定內容說明</td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div style="margin-top: 30px;" class="row">
                                    <table class="table nobordered control-label">
                                        <tr>
                                            <td class="text-right">
                                                <asp:Label CssClass="control-label" runat="server">稅則號別：</asp:Label></td>
                                            <td>
                                                <asp:TextBox ID="txtTax" CssClass="tb tb-m" AutoPostBack="true" runat="server"></asp:TextBox></td>
                                            <td class="text-right">
                                                <asp:Label CssClass="control-label" runat="server">中文貨名：</asp:Label></td>
                                            <td>
                                                <asp:TextBox ID="txtCHI_NAME" CssClass="tb tb-m" AutoPostBack="true" runat="server"></asp:TextBox></td>
                                        </tr>

                                        <tr>
                                            <td class="text-right">
                                                <asp:Label CssClass="control-label" runat="server">英文貨名：</asp:Label></td>
                                            <td>
                                                <asp:TextBox ID="txtENG_NAME" CssClass="tb tb-m" AutoPostBack="true" runat="server"></asp:TextBox></td>
                                            <td class="text-right">
                                                <asp:Label CssClass="control-label" runat="server">輸出規定：</asp:Label></td>
                                            <td>
                                                <asp:TextBox ID="txtOP" CssClass="tb tb-m" AutoPostBack="true" runat="server"></asp:TextBox></td>
                                        </tr>

                                        <tr>
                                            <td class="text-right">
                                                <asp:Label CssClass="control-label" runat="server">輸入規定：</asp:Label></td>
                                            <td>
                                                <asp:TextBox ID="txtIP" CssClass="tb tb-m" AutoPostBack="true" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" class="text-center">
                                                <asp:Label CssClass="control-label text-red" runat="server">以上五個欄位至少需選一輸入</asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="text-center">
                                        <asp:Button ID="btnQuery" CssClass="btn-success btnw2" runat="server" Text="查詢" />
                                        <asp:Button ID="btnReset" CssClass="btn-default btnw2" runat="server" Text="清除" />
                                        <asp:Button ID="btnReturn" CssClass="btn-default btnw2" runat="server" Text="返回" />
                                    </div>
                                </div>
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
