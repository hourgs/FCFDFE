<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_C23.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.MPMS_C23" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        th {
            background-color: blue;
            color: aliceblue;
            text-align: center;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <asp:Label ID ="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>購案審查簽辦表附件分發單位
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-left">
                                <tr>
                                    <th rowspan="10" style="width: 5%">
                                        <asp:Label CssClass="control-label" runat="server">附件分發單位</asp:Label></th>
                                    <th style="width: 25%">
                                        <asp:Label CssClass="control-label" runat="server">分發單位</asp:Label></th>
                                    <th style="width: 70%">
                                        <asp:Label CssClass="control-label" runat="server">附件名稱及份數</asp:Label></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkAdmin" runat="server" Text="行政院金融監督管理委員會銀行局" /></td>
                                    <td>
                                        <asp:DropDownList ID="drpAdmin" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="0">外匯申請書</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtAdmin" CssClass="tb tb-xs" runat="server">1</asp:TextBox>份
                                        <asp:Button ID="btnAddAdmin" CssClass="btn-warning btnw2" OnClick="btnAddAdmin_Click" runat="server" Text="加入" />
                                        <asp:TextBox ID="txtAdminMes" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chk0A100" runat="server" Text="採購發包單位" /></td>
                                    <td>
                                        <asp:DropDownList ID="drp0A100" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="0">核定書</asp:ListItem>
                                            <asp:ListItem Value="1">計畫清單</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txt0A100" CssClass="tb tb-xs" runat="server">1</asp:TextBox>份
                                        <asp:Button ID="btnAdd0A100" CssClass="btn-warning btnw2" OnClick="btnAdd0A100_Click" runat="server" Text="加入" />
                                        <asp:TextBox ID="txt0A100Mes" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkNSECTION" runat="server" Text="原申購單位" /></td>
                                    <td>
                                        <asp:DropDownList ID="drpNSECTION" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="0">核定書</asp:ListItem>
                                            <asp:ListItem Value="1">計畫清單</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtNSECTION" CssClass="tb tb-xs" runat="server">1</asp:TextBox>份
                                        <asp:Button ID="btnAddNSECTION" CssClass="btn-warning btnw2" OnClick="btnAddNSECTION_Click" runat="server" Text="加入" />
                                        <asp:TextBox ID="txtNSECTIONMes" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkSec" runat="server" Text="審計部（第二廳）" /></td>
                                    <td>
                                        <asp:DropDownList ID="drpSec" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="0">核定書</asp:ListItem>
                                            <asp:ListItem Value="1">計畫清單</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtSec" CssClass="tb tb-xs" runat="server">1</asp:TextBox>份
                                        <asp:Button ID="btnAddSec" CssClass="btn-warning btnw2" OnClick="btnAddSec_Click" runat="server" Text="加入" />
                                        <asp:TextBox ID="txtSecMes" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkKeelung" runat="server" Text ="財政部基隆關稅局" /></td>
                                    <td>
                                        <asp:DropDownList ID="drpKeelung" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="0">核定書</asp:ListItem>
                                            <asp:ListItem Value="1">計畫清單</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtKeelung" CssClass="tb tb-xs" runat="server">1</asp:TextBox>份
                                        <asp:Button ID="btnAddKeelung" CssClass="btn-warning btnw2" OnClick="btnAddKeelung_Click" runat="server" Text="加入" />
                                        <asp:TextBox ID="txtKeelungMes" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkTaipei" runat="server" Text="財政部台北關稅局" /></td>
                                    <td>
                                        <asp:DropDownList ID="drpTaipei" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="0">核定書</asp:ListItem>
                                            <asp:ListItem Value="1">計畫清單</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtTaipei" CssClass="tb tb-xs" runat="server">1</asp:TextBox>份
                                        <asp:Button ID="btnAddTaipei" CssClass="btn-warning btnw2" OnClick="btnAddTaipei_Click" runat="server" Text="加入" />
                                        <asp:TextBox ID="txtTaipeiMes" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkTaichung" runat="server" Text="財政部台中關稅局" /></td>
                                    <td>
                                        <asp:DropDownList ID="drpTaichung" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="0">核定書</asp:ListItem>
                                            <asp:ListItem Value="1">計畫清單</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtTaichung" CssClass="tb tb-xs" runat="server">1</asp:TextBox>份
                                        <asp:Button ID="btnAddTaichung" CssClass="btn-warning btnw2" OnClick="btnAddTaichung_Click" runat="server" Text="加入" />
                                        <asp:TextBox ID="txtTaichungMes" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkKao" runat="server" Text="財政部高雄關稅局" /></td>
                                    <td>
                                        <asp:DropDownList ID="drpKao" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="0">核定書</asp:ListItem>
                                            <asp:ListItem Value="1">計畫清單</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtKao" CssClass="tb tb-xs" runat="server">1</asp:TextBox>份
                                        <asp:Button ID="btnAddKao" CssClass="btn-warning btnw2" OnClick="btnAddKao_Click" runat="server" Text="加入" />
                                        <asp:TextBox ID="txtKaoMes" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkPURCHASE_UNIT" runat="server" Text="採購發包單位" /></td>
                                    <td>
                                        <asp:DropDownList ID="drpPURCHASE_UNIT" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="0">核定書</asp:ListItem>
                                            <asp:ListItem Value="1">計畫清單</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtPURCHASE_UNIT" CssClass="tb tb-xs" runat="server">1</asp:TextBox>份
                                        <asp:Button ID="btnAddPURCHASE_UNIT" CssClass="btn-warning btnw2" OnClick="btnAddPURCHASE_UNIT_Click" runat="server" Text="加入" />
                                        <asp:TextBox ID="txtPURCHASE_UNIT2" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>

                            </table>
                            <div style="margin:5% auto" class="text-center">
                                <asp:Button ID="btnSave" CssClass="btn-warning btnw4" OnClick="btnSave_Click" runat="server" Text="存檔" />
                                <asp:Button ID="btnReturn" CssClass="btn-warning btnw4" runat="server" OnClick="btnReturn_Click" Text="回上一頁" />
                            </div>
                            <asp:GridView ID="GV_OVC" CssClass=" table data-table table-striped border-top " AutoGenerateColumns="false" OnPreRender="GV_OVC_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="分發單位" DataField="OVC_UNIT" />
                                    <asp:BoundField HeaderText="附件名稱及份數" DataField="OVC_ATTACH_NAME" />
                	            </Columns>
	   		               </asp:GridView>
                            
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
