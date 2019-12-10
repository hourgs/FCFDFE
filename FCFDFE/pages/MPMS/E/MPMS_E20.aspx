<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E20.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E20" %>
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
                    <!--標題-->代管有價證券
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
                            <table class="table table-bordered">
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label" runat="server" Text="購案合約編號："></asp:Label>
                                                        <asp:Label ID="lblOVC_PURCH_6" CssClass="control-label" runat="server"></asp:Label>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:HyperLink ID="InkContractorData" Visible="false" runat="server">查詢本合約之合約商資料</asp:HyperLink>
                                                    </td>
                                                </tr>
                                                <tr class="no-bordered-seesaw">
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label" runat="server" Text="有價證券所有權人："></asp:Label>
                                                        <asp:TextBox ID="txtOVC_OWN_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        <asp:Label CssClass="control-label" runat="server" Text="有價證券所有權人統一編號："></asp:Label>
                                                        <asp:TextBox ID="txtOVC_OWN_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="no-bordered-seesaw">
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label" runat="server" Text="有價證券所有權人地址："></asp:Label>
                                                        <asp:TextBox ID="txtOVC_OWN_ADDRESS" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="no-bordered-seesaw">
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label" runat="server" Text="有價證券所有權人電話："></asp:Label>
                                                        <asp:TextBox ID="txtOVC_OWN_TEL" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="no-bordered-seesaw">
                                                    <td class="no-bordered">
                                                        <asp:Label CssClass="control-label" runat="server" Text="申購人："></asp:Label>
                                                        <asp:Label ID="lblOVC_PUR_USER" CssClass="control-label text-green" runat="server"></asp:Label>
                                                    </td>
                                                    <td class="no-bordered">
                                                        <%--<asp:Label CssClass="control-label" runat="server" Text="職級："></asp:Label>--%>
                                                        <asp:Label ID="lblOVC_PUR_NSECTION" CssClass="control-label text-green" runat="server"></asp:Label>
                                                    </td>
                                                    <td class="no-bordered">
                                                        <asp:Label CssClass="control-label" runat="server" Text="電話(自動)："></asp:Label>
                                                        <asp:Label ID="lblOVC_PUR_IUSER_PHONE" CssClass="control-label text-green" runat="server"></asp:Label>
                                                    </td>
                                                    <td class="no-bordered">
                                                        <asp:Label CssClass="control-label" runat="server" Text="電話(軍線)："></asp:Label>
                                                        <asp:Label ID="lblOVC_PUR_IUSER_PHONE_EXT" CssClass="control-label text-green" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="td-inner-table" colspan="4">
                                                        <asp:Label CssClass="control-label text-blue" runat="server" Text="代管項目"></asp:Label>
                                                        <asp:Label CssClass="control-label text-red" runat="server" Text="(最多三筆)"></asp:Label>
                                                        <table class="table table-inner table-bordered text-center">
                                                            <tr>
                                                                <td><asp:Label CssClass="control-label text-green" runat="server" Text="項次"></asp:Label></td>
                                                                <td><asp:Label CssClass="control-label text-green" runat="server" Text="代管事由說明"></asp:Label></td>
                                                                <td><asp:Label CssClass="control-label text-green" runat="server" Text="有價證券名稱"></asp:Label></td>
                                                                <td><asp:Label CssClass="control-label text-green" runat="server" Text="有價證券面額衡量單位"></asp:Label></td>
                                                                <td><asp:Label CssClass="control-label text-green" runat="server" Text="有價證券面額數量"></asp:Label></td>
                                                                <td><asp:Label CssClass="control-label text-green" runat="server" Text="新台幣入帳金額(必填)"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td><asp:Label ID="lblONB_ITEM_1" CssClass="control-label text-green" runat="server" Text="1"></asp:Label></td>
                                                                <td><asp:TextBox ID="txtOVC_REASON_1" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                <td><asp:TextBox ID="txtOVC_NSTOCK_1" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                <td><asp:TextBox ID="txtOVC_CURRENT_1" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                <td><asp:TextBox ID="txtONB_MONEY_1" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                <td><asp:TextBox ID="txtONB_MONEY_NT_1" CssClass="tb tb-s" OnTextChanged="txtONB_MONEY_NT_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td><asp:Label ID="lblONB_ITEM_2" CssClass="control-label text-green" runat="server" Text="2"></asp:Label></td>
                                                                <td><asp:TextBox ID="txtOVC_REASON_2" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                <td><asp:TextBox ID="txtOVC_NSTOCK_2" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                <td><asp:TextBox ID="txtOVC_CURRENT_2" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                <td><asp:TextBox ID="txtONB_MONEY_2" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                <td><asp:TextBox ID="txtONB_MONEY_NT_2" CssClass="tb tb-s" OnTextChanged="txtONB_MONEY_NT_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td><asp:Label ID="lblONB_ITEM_3" CssClass="control-label text-green" runat="server" Text="3"></asp:Label></td>
                                                                <td><asp:TextBox ID="txtOVC_REASON_3" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                <td><asp:TextBox ID="txtOVC_NSTOCK_3" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                <td><asp:TextBox ID="txtOVC_CURRENT_3" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                <td><asp:TextBox ID="txtONB_MONEY_3" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                <td><asp:TextBox ID="txtONB_MONEY_NT_3" CssClass="tb tb-s" OnTextChanged="txtONB_MONEY_NT_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr class="no-bordered-seesaw">
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label text-red" runat="server" Text="合計有價證券金額："></asp:Label>
                                                        <asp:TextBox ID="txtONB_ALL_MONEY" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="no-bordered-seesaw">
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label" runat="server" Text="類別："></asp:Label>
                                                        <asp:RadioButtonList ID="rdoOVC_KIND" CssClass="radioButton text-red" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                            <asp:ListItem>履保</asp:ListItem>
                                                            <asp:ListItem>保固</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr class="no-bordered-seesaw">
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label text-blue" runat="server" Text="附記："></asp:Label>
                                                        <asp:TextBox ID="txtOVC_MARK" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="no-bordered-seesaw">
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label" runat="server" Text="承辦單位統一編號："></asp:Label>
                                                        <asp:TextBox ID="txtOVC_WORK_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        <asp:Label CssClass="control-label" runat="server" Text="全銜"></asp:Label>
                                                        <asp:TextBox ID="txtOVC_WORK_NAME" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="no-bordered-seesaw">
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label" runat="server" Text="業務部門名稱："></asp:Label>
                                                        <asp:TextBox ID="txtOVC_WORK_UNIT" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="no-bordered-seesaw">
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label" runat="server" Text="業務部門名稱："></asp:Label>
                                                        <asp:TextBox ID="TextBox1" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="no-bordered-seesaw">
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label position-left" runat="server" Text="收入單通知編號："></asp:Label>
                                                        <asp:TextBox ID="txtOVC_RECEIVE_NO" CssClass="position-left tb tb-l" runat="server"></asp:TextBox>
                                                        <asp:Label CssClass="control-label position-left" runat="server">日期：</asp:Label>
                                                        <div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                        <asp:TextBox ID="txtOVC_DRECEIVE" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                        <div class="add-on"><i class="icon-calendar"></i></div>
                                                            </div>
                                                    </td>
                                                </tr>
                                                <tr class="no-bordered-seesaw">
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label" runat="server" Text="財務收繳單位："></asp:Label>
                                                        <asp:TextBox ID="txtSecOVC_COMPTROLLER_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        <%--<asp:DropDownList ID="drpDocOVC_COMPTROLLER_NO" CssClass="tb tb-m" runat="server">
                                                            <asp:ListItem>請選擇</asp:ListItem>
                                                        </asp:DropDownList>--%>
                                                    </td>
                                                </tr>
                                                <tr class="no-bordered-seesaw">
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label position-left" runat="server" Text="財務單位編號："></asp:Label>
                                                        <asp:TextBox ID="txtSecOVC_COMPTROLLER_NO_1" CssClass="position-left tb tb-l" runat="server"></asp:TextBox>
                                                        <asp:Label CssClass="control-label position-left" runat="server">收訖日期：</asp:Label>
                                                        <div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" >
                                                        <asp:TextBox ID="txtOVC_DCOMPTROLLER" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                        <div class="add-on"><i class="icon-calendar"></i></div>
                                                            </div>
                                                    </td>
                                                </tr>
                                                <tr class="no-bordered-seesaw">
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label position-left" runat="server" Text="退還單通知編號："></asp:Label>
                                                        <asp:TextBox ID="txtOVC_BACK_NO" CssClass="position-left tb tb-l" runat="server"></asp:TextBox>
                                                        <asp:Label CssClass="control-label position-left" runat="server">日期：</asp:Label>
                                                        <div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" >
                                                        <asp:TextBox ID="txtOVC_DBACK" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                        <div class="add-on"><i class="icon-calendar"></i></div>
                                                            </div>
                                                    </td>
                                                </tr>
                                                <tr class="no-bordered-seesaw">
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label text-blue" runat="server" Text="退還理由："></asp:Label>
                                                        <asp:Label CssClass="control-label text-red" runat="server" Text="請選擇標準片語-->"></asp:Label>
                                                        <asp:DropDownList ID="drpOVC_BACK_REASON" CssClass="tb tb-l" OnSelectedIndexChanged="drpOVC_BACK_REASON_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList><br>
                                                        <asp:TextBox ID="txtOVC_BACK_REASON" CssClass="tb tb-full" runat="server" TextMode="MultiLine" Height="100px" Width="700px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="no-bordered-seesaw">
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label text-blue" runat="server" Text="退還附記："></asp:Label>
                                                        <asp:Label CssClass="control-label text-red" runat="server" Text="請選擇標準片語-->"></asp:Label>
                                                        <asp:DropDownList ID="drpOVC_BACK_MARK" CssClass="tb tb-l" OnSelectedIndexChanged="drpOVC_BACK_MARK_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList><br>
                                                        <asp:TextBox ID="txtOVC_BACK_MARK" CssClass="tb tb-full" runat="server" TextMode="MultiLine" Height="100px" Width="700px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            

                                    
                            <table class="table no-bordered-seesaw text-center">
                                <tr class="no-bordered-seesaw">
                                    <td style="width:50%" class="text-right no-bordered"><asp:Label CssClass="control-label text-red" runat="server">主官核批日：</asp:Label></td>
                                    <td class="no-bordered"><div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" >
                                    <asp:TextBox ID="txtOVC_DAPPROVE" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                    <div class="add-on"><i class="icon-calendar"></i></div>
                                    </div></td>
                               </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnReturn" CssClass="btn-default btnw4" OnClick="btnReturn_Click" runat="server" Text="回上一頁" />
                                <asp:Button ID="btnReturnM" CssClass="btn-default btnw4" OnClick="btnReturnM_Click" runat="server" Text="回主流程" />
                                <asp:Button ID="btnSave" CssClass="btn-warning btnw2" OnClick="btnSave_Click" runat="server" Text="存檔"/>
                            </div>
                            <br />
                            <div class="text-center">
                                <asp:LinkButton ID="LinkButton1" CssClass="text-red" OnClick="LinkButton1_Click" runat="server">收入通知單預覽列印.doc</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton3" CssClass="text-red" OnClick="LinkButton3_Click" runat="server">收入通知單預覽列印.pdf</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton4" CssClass="text-red" OnClick="LinkButton4_Click" runat="server">收入通知單預覽列印.odt</asp:LinkButton>
                                <asp:HyperLink ID="InkReceivePreview" CssClass="text-red" Visible="false" runat="server">收入通知單預覽列印</asp:HyperLink>
                                <br />
                                <asp:LinkButton ID="LinkButton2" CssClass="text-red" OnClick="LinkButton2_Click" runat="server">退還通知單預覽列印.doc</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton5" CssClass="text-red" OnClick="LinkButton5_Click" runat="server">退還通知單預覽列印.pdf</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton6" CssClass="text-red" OnClick="LinkButton6_Click" runat="server">退還通知單預覽列印.odt</asp:LinkButton>
                                <asp:HyperLink ID="InkReturnPreview" CssClass="text-red" Visible="false" runat="server">退還通知單預覽列印</asp:HyperLink>
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
