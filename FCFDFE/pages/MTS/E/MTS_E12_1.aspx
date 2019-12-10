<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E12_1.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E12_1" %>
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
                    運費資料管理
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" OnTextChanged="txtOvcBldNo_TextChanged" runat="server">提單編號</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtOvcBldNo" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">海空運別</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="drpOvcSeaOrAir" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>不限定</asp:ListItem>
                                            <asp:ListItem>海運</asp:ListItem>
                                            <asp:ListItem>空運</asp:ListItem>
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
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="drpOvcMilitaryType" CssClass="tb tb-s" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">是否帶入結報申請表</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="drpIsBring" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>不限定</asp:ListItem>
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">進出口日期</asp:Label>
                                    </td>
                                    <td colspan="7">
                                        <asp:RadioButtonList ID="rdoOvcEinnNo1" CssClass="radioButton position-left" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Selected="True">不限定</asp:ListItem>
                                            <asp:ListItem></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <div class="input-append datepicker position-left" >
                                            <asp:TextBox ID="txtOdtInvDate1" CssClass="tb tb-s position-left text-change" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server">&nbsp;&nbsp;至&nbsp;&nbsp;</asp:Label>
                                        <div class="input-append datepicker date position-left  text-change" >
                                            <asp:TextBox ID="txtOdtInvDate2" CssClass="tb tb-s position-left" runat="server" ></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnQuery" cssclass="btn-success btnw2" OnClick="btnQuery_onclick" runat="server" Text="查詢" />
                                <asp:Button ID="btnNew" cssclass="btn-success" runat="server" OnClick="btnNew_onclick" Text="直接新增運費資料" /><br /><br />
                            </div>
                            <asp:GridView ID="GV_TBGMT_CINF" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_CINF_PreRender"  OnRowCommand="GV_TBGMT_CINF_RowCommand" runat="server" DataKeyNames="OVC_BLD_NO">
                                <Columns>
                                    <asp:BoundField DataField="OVC_ICS_NO" HeaderText="運費編號" />
                                    <asp:BoundField DataField="OVC_BLD_NO" HeaderText="提單編號" />
                                    <asp:BoundField DataField="OVC_INLAND_CARRIAGE" DataFormatString="{0:#,##0}"  HeaderText="海空運費" />
                                    <asp:BoundField DataField="OVC_INF_NO" HeaderText="結報申請表編號" />
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnOther" CssClass="btn-success" Text="管理" CommandName="btnOther"   CommandArgument='<%#Eval("OVC_BLD_NO") %>'
                                                 runat="server"/>
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
