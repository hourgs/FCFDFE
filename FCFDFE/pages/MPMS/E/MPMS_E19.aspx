<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E19.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E19" %>
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
                <header  class="title text-red">
                    <!--標題-->檢驗申請
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td  style="background-color:red;color:yellow;"><asp:Label CssClass="control-label" runat="server" Text="請注意！未輸入資料或存檔無法列印各項報表！！" Font-Size="Large"></asp:Label></td>
                                </tr>
                            </table>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="購案編號"></asp:Label></td>
                                    <td class="text-left"><asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="批次"></asp:Label></td>
                                    <td class="text-left"><asp:Label ID="lblONB_TIMES" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="複驗次數"></asp:Label></td>
                                    <td class="text-left"><asp:Label ID="lblONB_INSPECT_TIMES" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="再驗次數"></asp:Label></td>
                                    <td class="text-left"><asp:Label ID="lblONB_RE_INSPECT_TIMES" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="受文單位"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_INSPECT_UNIT" CssClass="tb tb-m" OnSelectedIndexChanged="drpOVC_INSPECT_UNIT_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                        <asp:TextBox ID="txtOVC_INSPECT_UNIT" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="申請日期"></asp:Label></td>
                                    <td class="text-left">
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DAPPLY" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="取樣者"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_TAKER" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="樣品中文名稱"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_INSPECTED_STUFF" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="剩餘樣品退還"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:RadioButtonList ID="RadioButtonList1" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td><asp:Label ID="Label11" CssClass="control-label" runat="server" Text="樣品英文名稱"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_INSPECTED_STUFF_ENG" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="件數"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtONB_QUALITY" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="規格號碼"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_INSPECT_DESC" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="檢附規格"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:RadioButtonList ID="RadioButtonList2" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>有</asp:ListItem>
                                            <asp:ListItem>無</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="申請號碼"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_IAPPLY" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="申請內容"></asp:Label></td>
                                    <td colspan="3" class="text-left"><asp:TextBox ID="txtOVC_APPLY_DESC" CssClass="tb tb-full" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="標誌"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_MARK" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="申請檢驗"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtONB_CASE_QUALITY" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server" Text="項"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="需要報告書正本"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtONB_REPORT_ORG" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server" Text="份"></asp:Label>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="副本"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtONB_REPORT_COPY" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server" Text="份"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="報告書中文份數"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtONB_REPORT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server" Text="份"></asp:Label>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="報告書英文份數"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtONB_REPORT_ENG" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server" Text="份"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnReturn" CssClass="btn-default btnw4" OnClick="btnReturn_Click" runat="server" Text="回上一頁" />
                                <asp:Button ID="btnReturnM" CssClass="btn-default btnw4" OnClick="btnReturnM_Click" runat="server" Text="回主流程" />
                                <asp:Button ID="btnSave" CssClass="btn-warning btnw2" OnClick="btnSave_Click" runat="server" Text="存檔" />
                            </div>
                            <br />
                            <div class="text-center">
                                <asp:LinkButton ID="LinkButton1" OnClick="LinkButton1_Click" CssClass="text-red" runat="server">列印檢驗申請單.doc</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton2" OnClick="LinkButton2_Click" CssClass="text-red" runat="server">列印檢驗申請單.pdf</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton3" OnClick="LinkButton3_Click" CssClass="text-red" runat="server">列印檢驗申請單.odt</asp:LinkButton>
                                <asp:HyperLink ID="InkPreview" Visible="false" CssClass="text-red" runat="server">列印檢驗申請單</asp:HyperLink>
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
