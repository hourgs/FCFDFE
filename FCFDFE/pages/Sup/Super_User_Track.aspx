<%@ Page Title="" Language="C#" MasterPageFile="~/Super.Master" AutoEventWireup="true" CodeBehind="Super_User_Track.aspx.cs" Inherits="FCFDFE.pages.Sup.Super_User_Track" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" style="background-color:red;">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->使用者探查
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered" style="width: 700px;">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">使用者帳號</asp:Label>
                                                                                </td>
                                    <td>
                                        <asp:TextBox ID="txtUSER_ID" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">使用者IP</asp:Label>
                                        </td>
                                    <td>
                                        <asp:TextBox ID="txtUSER_IP" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">日期區間</asp:Label>
                                    </td>
                                    <td>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txt_SDATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Label CssClass="control-label" runat="server">至</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txt_EDATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>

                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="query" CssClass="btn-success" OnClick="query_Click" runat="server" Text="查詢" /><br />
                            </div>
                                
                            <div class="subtitle">查詢結果</div>
                            <asp:GridView ID="GV_UserTrack" CssClass=" table data-table table-striped border-top table-bordered" DataKeyNames="SN" AutoGenerateColumns="false" OnPreRender="GV_UserTrack_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="使用者帳號" DataField="USER_ID" />
                                    <asp:BoundField HeaderText="名稱" DataField="USER_NAME" />
                                    <asp:BoundField HeaderText="操作日期" DataField="DATE" />
                                    <asp:BoundField HeaderText="使用者IP" DataField="IP" />
                                    <asp:BoundField HeaderText="使用功能" DataField="PAGE" />
                                    <asp:BoundField HeaderText="異動資料表名稱" DataField="TB" />
                                    <asp:BoundField HeaderText="動作" DataField="ACT" />
                	            </Columns>
	   		               </asp:GridView>
                    </div>
                </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
