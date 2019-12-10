<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_C19.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.MPMS_C19" %>

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
                    登錄購案核批輸入
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="width: 40%">
                                        <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">購案名稱</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server">AA09871L </asp:Label><asp:TextBox ID="txtOVC_PURCHEXT" CssClass="tb tb-s" runat="server"></asp:TextBox><br />
                                        <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server">購案測試01</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">申購單位(代碼)-申購人(電話)</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PUR_USER" CssClass="control-label" runat="server">國防部政務辦公室(00200)-陳XX(11 軍線:265780)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-red position-left" runat="server">主管批核日：</asp:Label>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtSUPERVISOR" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label text-red" runat="server">是否納入參考範例：</asp:Label>
                                        <asp:DropDownList ID="drpOEM_EXAMPLE_SUPPORT" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>Y:是</asp:ListItem>
                                            <asp:ListItem>N:否</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-red position-left" runat="server">核定(發文)日期：</asp:Label>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOVC_PUR_DAPPROVE" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label text-red" runat="server">核定(發文)文號：</asp:Label>
                                        <asp:TextBox ID="txtOVC_PUR_APPROVE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <asp:Repeater ID="rpt_ISOURCE" runat="server" OnItemDataBound="rpt_ISOURCE_ItemDataBound">
                                    <ItemTemplate>
                                       <tr>
                                            <td>
                                                <asp:Label ID="lblOVC_ISOURCE" CssClass="control-label text-red position-left" runat="server" Text='<%#Bind("OVC_ISOURCE") %>'></asp:Label><br />
                                            </td>
                                            <td>
                                                <asp:Label CssClass="control-label text-red position-left" runat="server">預算奉准日期：</asp:Label>
                                                <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                                    <asp:TextBox ID="txtOVC_PUR_DAPPR_PLAN" CssClass="tb tb-s position-left" runat="server" Text='<%#Bind("OVC_PUR_DAPPR_PLAN") %>' ></asp:TextBox>
                                                    <div class="add-on"><i class="icon-calendar"></i></div>
                                                </div>                                 
                                                <asp:Label CssClass="control-label text-red position-left" runat="server">奉准文號：</asp:Label>
                                                <asp:TextBox ID="txtOVC_PUR_APPR_PLAN" CssClass="tb tb-m position-left" runat="server" Text ='<%#Bind("OVC_PUR_APPR_PLAN") %>' ></asp:TextBox>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                
                                <tr>
                                    <td class="text-right">
                                        <asp:Label CssClass="control-label text-red" runat="server">決標原則</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_BID" CssClass="control-label text-red" runat="server">定底價，最低標</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-right">
                                        <asp:Label CssClass="control-label text-red" runat="server">報價及決標方式</asp:Label><br />
                                        <asp:Label CssClass="control-label text-red" runat="server">投標次數</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_QUOTE" CssClass="control-label text-red" runat="server">本業採總價決標</asp:Label><br />
                                        <asp:Label ID="lblOVC_BID_TIMES" CssClass="control-label text-red" runat="server">不分段開標</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-right">
                                        <asp:Label CssClass="control-label text-red" runat="server">採購屬性(採購性質)</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_LAB_GN" CssClass="control-label text-red" runat="server">勞務採購</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-right">
                                        <asp:Label CssClass="control-label text-red" runat="server">招標方式</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PUR_ASS_VEN_CODE" CssClass="control-label text-red" runat="server">公開招標</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-right">
                                        <asp:Label CssClass="control-label text-red" runat="server">保留增購金額</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblONB_RESERVE_AMOUNT" CssClass="control-label text-red" runat="server">無</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-right">
                                        <asp:Label CssClass="control-label text-red" runat="server">押標金額度</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_BID_MONEY" CssClass="control-label text-red" runat="server">不少於投標文件標價百分之OO</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-right">
                                        <asp:Label CssClass="control-label text-red" runat="server">特殊採購(臨時、緊急、臨時緊急)</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_SPECIAL" CssClass="control-label text-red" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-right">
                                        <asp:Label CssClass="control-label text-red" runat="server">協商措施</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_NEGOTIATION" CssClass="control-label text-red" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-right">
                                        <asp:Label CssClass="control-label text-red" runat="server">是否需辦理FAC資審</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_FAC" CssClass="control-label text-blue" runat="server">是</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-right">
                                        <asp:Label CssClass="control-label text-red" runat="server">是否已完成FAC資審</asp:Label><br />
                                        <asp:Label CssClass="control-label text-red" runat="server">計劃評核(審查)單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drp_OVC_FAC" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>請選擇</asp:ListItem>
                                            <asp:ListItem>已完成</asp:ListItem>
                                            <asp:ListItem>尚未完成</asp:ListItem>
                                            <asp:ListItem>無須資審</asp:ListItem>
                                        </asp:DropDownList><br />
                                        <asp:Label ID="lblOVC_AUDIT_UNIT" CssClass="control-label text-red" runat="server">00N00(國防部國防採購室)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-right">
                                        <asp:Label CssClass="control-label text-red" runat="server">採購發包單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_PURCHASE_UNIT" CssClass="tb tb-s" runat="server">00N00</asp:TextBox>
                                        <asp:Button ID="btnQuery_PURCHASE" CssClass="btn-success btnw4" runat="server" OnClientClick="OpenWindow('txtOVC_PURCHASE_UNIT', 'txtOVC_PURCHASE_UNIT_1')" OnClick="btnQuery_PURCHASE_Click" Text="單位查詢" /><!--綠色-->
                                        <asp:TextBox ID="txtOVC_PURCHASE_UNIT_1" CssClass="tb tb-m" runat="server">國防部國防採購室</asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-right">
                                        <asp:Label CssClass="control-label text-red" runat="server">履約檢核單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_CONTRACT_UNIT" CssClass="tb tb-s" runat="server">00N00</asp:TextBox>
                                        <asp:Button ID="btnQuery_OVC_CONTRACT_UNIT" CssClass="btn-success btnw4" OnClientClick="OpenWindow('txtOVC_CONTRACT_UNIT', 'txtOVC_CONTRACT_UNIT_1')" OnClick="btnQuery_OVC_CONTRACT_UNIT_Click" runat="server" Text="單位查詢" /><!--綠色-->
                                        <asp:TextBox ID="txtOVC_CONTRACT_UNIT_1" CssClass="tb tb-m" runat="server">國防部國防採購室</asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center" style="margin-bottom: 15px;">
                                <asp:Button ID="btnSave" CssClass="btn-success btnw4" OnClick="btnSave_Click" runat="server" Text="確認" /><!--黃色-->
                            </div>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="width: 35%;">
                                        <asp:Label CssClass="control-label" runat="server">評核單位紙本收案日：</asp:Label>
                                    </td>
                                    <td style="width: 15%;">
                                        <asp:Label ID="labOVC_PAPER_DRECEIVE" CssClass="control-label text-blue" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 13%;">
                                        <asp:Label CssClass="control-label text-red" runat="server">評核總天數：</asp:Label>
                                    </td>
                                    <td style="width: 2%;">
                                        <asp:TextBox ID="txtOVC_AUDIT_DAY" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width: 35%;">
                                        <asp:Label CssClass="control-label" runat="server">(主官核批日-- 評核單位紙本收案日)</asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center" style="margin-bottom: 15px;">
                                <asp:Button ID="btnGOOD_APPLICATION_PRINT" CssClass="btn-success" CommandName="PrintSupPDF" OnCommand="btn_PRINT_Command" runat="server" Text="物資申請書列印" /><!--黃色-->
                                <asp:Button ID="btnOLD_GOOD_APPLICATION_PRINT" CssClass="btn-success" CommandName="PrintNewPDF" OnCommand="btn_PRINT_Command" runat="server" Text="新物資申請書列印" /><!--黃色-->
                                <asp:Button ID="btnBUDGET_DETAIL" CssClass="btn-success" runat="server" CommandName="btnBUDGET_DETAIL" OnCommand="btn_PRINT_Command" Text="預算年度分配明細表" /><!--黃色-->
                                <asp:Button ID="btnOVCLIST_PRINT" CssClass="btn-success" runat="server" CommandName="btnOVCLIST_PRINT" OnCommand="btn_PRINT_Command" Text="採購計劃清單列印.pdf" /><!--黃色-->
                                <asp:Button ID="btnOVCLIST_PRINT_WORD" CssClass="btn-success" CommandName="btnOVCLIST_PRINT_WORD" OnCommand="btn_PRINT_Command" runat="server" Text=".doc" /><!--黃色-->
                                <asp:Button ID="btnOVCLIST_PRINT_ODT" CssClass="btn-success" CommandName="btnOVCLIST_PRINT_ODT" OnCommand="btn_PRINT_Command" runat="server" Text=".odt" /><!--黃色-->
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
