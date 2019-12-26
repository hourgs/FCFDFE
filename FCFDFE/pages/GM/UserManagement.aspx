<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="FCFDFE.pages.GM.UserManagement" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function(){
            //$("#a_eqpt").addClass("in");
            //$("#ul_eqpt").addClass("in");
            $("#li_MPMS_A").addClass("active");
            $("#li_MPMS_A12").addClass("active");
        });
    </script>
    <div class="row">
        <div <%--class="col-lg-offset-2 col-lg-8"--%> style="width: 1000px; margin:auto;">
            <section class="panel">
                <header class="panel-heading" style="text-align:center; font-size:20px; font-weight: bold; color: #000;">
                    採購預劃購案編輯
                </header>
                <asp:Label ID="firstname" hidden="true" runat="server"></asp:Label>
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <div class="form-group">
                                <asp:Label AssociatedControlID="lblPrePurNum" CssClass="control-label col-lg-2" runat="server">預劃購案編號：</asp:Label>
                                <div class="col-lg-4">
                                    <asp:Label ID="lblPrePurNum" CssClass="control-label col-lg-12" runat="server">123</asp:Label>
                                </div>
                                <asp:Label AssociatedControlID="drp__" CssClass="control-label col-lg-2" runat="server">採購單位地區及方式：</asp:Label>
                                <div class="col-lg-4">
                                    <asp:DropDownList ID="drp__" CssClass="form-control" runat="server">
                                        <asp:ListItem>內容1</asp:ListItem>
                                        <asp:ListItem>內容2</asp:ListItem>
                                        <asp:ListItem>內容3</asp:ListItem>
                                        <asp:ListItem>內容4</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group ">
                                <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-2" runat="server">美軍編號：</asp:Label>
                                <div class="col-lg-4">

                                </div>
                                <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-2" runat="server">軍售案類別：</asp:Label>
                                <div class="col-lg-4">

                                </div>
                            </div>
                            <div class="form-group ">
                                <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-1" runat="server">購案名稱：</asp:Label>
                                <div class="col-lg-5">
                                    <asp:TextBox ID="TextBox3" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-1" runat="server">招標方式：</asp:Label>
                                <div class="col-lg-3">
                                    <asp:DropDownList ID="DropDownList1" CssClass="form-control" runat="server">
                                        <asp:ListItem>內容1</asp:ListItem>
                                        <asp:ListItem>內容2</asp:ListItem>
                                        <asp:ListItem>內容3</asp:ListItem>
                                        <asp:ListItem>內容4</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group ">
                                <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-2" runat="server">專案代號(適用裝備)：</asp:Label>
                                <div class="col-lg-1">
                                    <asp:TextBox ID="TextBox2" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-1" runat="server">採購屬性：</asp:Label>
                                <div class="col-lg-2">
                                    <asp:DropDownList ID="DropDownList10" CssClass="form-control" runat="server"></asp:DropDownList>
                                </div>
                                <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-1" runat="server">計畫性質：</asp:Label>
                                <div class="col-lg-3">
                                    <asp:DropDownList ID="DropDownList12" CssClass="form-control" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group ">
                                <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-1" runat="server">核定權責：</asp:Label>
                                <div class="col-lg-3">
                                    <asp:DropDownList ID="DropDownList2" CssClass="form-control" runat="server"></asp:DropDownList>
                                </div>
                                <div class="col-lg-2">
                                    <asp:Button ID="Button1" CssClass="btn btn-info" runat="server" Text="核定權責說明" />
                                </div>
                                <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-3" runat="server">是否為1月1日須執行之購案：</asp:Label>
                                <div class="col-lg-3">
                                    <asp:DropDownList ID="DropDownList4" CssClass="form-control" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group ">
                                <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-2" runat="server">承辦人姓名：</asp:Label>
                                <div class="col-lg-2">
                                    <asp:TextBox ID="TextBox5" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-lg-8">
                                    <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-1" runat="server">電話</asp:Label>
                                    <div class="col-lg-4">
                                        <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-4" runat="server">軍線1：</asp:Label>
                                        <div class="col-lg-8">
                                            <asp:TextBox ID="TextBox6" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-4">
                                        <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-4" runat="server">軍線2：</asp:Label>
                                        <div class="col-lg-8">
                                            <asp:TextBox ID="TextBox1" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-3">
                                        <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-5" runat="server">自動：</asp:Label>
                                        <div class="col-lg-7">
                                            <asp:TextBox ID="TextBox8" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group ">
                                <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-2" runat="server">承辦人手機：</asp:Label>
                                <div class="col-lg-4">
                                    <asp:TextBox ID="TextBox4" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-2" runat="server">承辦人E-MAIL：</asp:Label>
                                <div class="col-lg-4">
                                    <asp:TextBox ID="TextBox7" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-3" runat="server">本案委託購案，委託單位：</asp:Label>
                                <div class="col-lg-2">
                                    <asp:TextBox ID="TextBox10" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-lg-4 text-center">
                                    <asp:Button ID="Button9" CssClass="btn btn-info" runat="server" Text="單位查詢" />
                                    <asp:Button ID="Button10" CssClass="btn btn-info" runat="server" Text="委託單位說明" />
                                </div>
                                <div class="col-lg-3">
                                    <asp:TextBox ID="TextBox9" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-group-inner">
                                    <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-3" ForeColor="Red" runat="server">最後計畫評核(審查)單位：</asp:Label>
                                    <div class="col-lg-2">
                                        <asp:TextBox ID="TextBox24" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-lg-3">
                                        <asp:TextBox ID="TextBox11" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-lg-4">
                                        <asp:Button ID="Button11" CssClass="btn btn-info" runat="server" Text="單位查詢" />
                                    </div>
                                </div>
                                <div class="form-group-inner">
                                    <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-2" runat="server">採購發包單位：</asp:Label>
                                    <div class="col-lg-2">
                                        <asp:TextBox ID="TextBox12" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-lg-3">
                                        <asp:TextBox ID="TextBox25" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-lg-5">
                                        <asp:Button ID="Button2" CssClass="btn btn-info" runat="server" Text="單位查詢" />
                                    </div>
                                </div>
                                <div class="form-group-inner">
                                    <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-2" runat="server">履約驗結單位：</asp:Label>
                                    <div class="col-lg-2">
                                        <asp:TextBox ID="TextBox26" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-lg-3">
                                        <asp:TextBox ID="TextBox27" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-lg-5">
                                        <asp:Button ID="Button3" CssClass="btn btn-info" runat="server" Text="單位查詢" />
                                    </div>
                                </div>
                                <asp:Label ID="Label3" CssClass="control-label col-lg-12" runat="server" Text="(若仍由採購室下授委辦單位履約者，仍請選定採購中心為履約驗結單位)" ForeColor="Red"></asp:Label>
                            </div>
                            <div class="form-group">
                                <div class="form-group-inner">
                                    <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-2" runat="server">預計申辦日期</asp:Label>
                                    <div class="col-lg-2">
                                        <input id="dp1" type="text" value="12-02-2012" size="16" class="form-control">
                                    </div>
                                    <asp:Label CssClass="control-label col-lg-8" runat="server">(西元年，例如:2000-01-01)</asp:Label>
                                </div>
                                <div class="form-group-inner">
                                    <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-1" runat="server">審核天數：</asp:Label>
                                    <div class="col-lg-1">
                                        <asp:TextBox ID="TextBox30" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <asp:Label CssClass="control-label col-lg-10" runat="server">(日曆天)</asp:Label>
                                </div>
                                <div class="form-group-inner">
                                    <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-2" runat="server">預計核定日：</asp:Label>
                                    <div class="col-lg-2">
                                        <asp:TextBox ID="TextBox13" CssClass="form-control" disabled runat="server"></asp:TextBox>
                                    </div>
                                    <asp:Label CssClass="control-label col-lg-8" runat="server">(=預計呈報日期+審核天數，由系統幫您算出)</asp:Label>
                                </div>
                                <div class="form-group-inner">
                                    <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-1" runat="server">招標天數：</asp:Label>
                                    <div class="col-lg-4">
                                        <asp:DropDownList ID="DropDownList3" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                    <asp:Label CssClass="control-label col-lg-7" runat="server">(日曆天)</asp:Label>
                                </div>
                                <div class="form-group-inner">
                                    <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-2" runat="server">預計簽約日：</asp:Label>
                                    <div class="col-lg-2">
                                        <asp:TextBox ID="TextBox14" CssClass="form-control" disabled runat="server"></asp:TextBox>
                                    </div>
                                    <asp:Label CssClass="control-label col-lg-8" runat="server">(=預計呈報日期+審核天數，由系統幫您算出)</asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-12" ForeColor="Red" runat="server">交貨天數</asp:Label>
                                <div class="form-group-inner">
                                    <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-1" runat="server">交貨天數：</asp:Label>
                                    <div class="col-lg-1">
                                        <asp:TextBox ID="TextBox17" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-2" runat="server">或限定交貨日期：</asp:Label>
                                    <div class="col-lg-2">
                                        <input id="dp1" type="text" value="12-02-2012" size="16" class="form-control">
                                    </div>
                                    <asp:Label CssClass="control-label col-lg-6" runat="server">(二擇一來做你的設定)</asp:Label>
                                </div>
                                <div class="form-group-inner">
                                    <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-1" runat="server">驗結天數：</asp:Label>
                                    <div class="col-lg-4">
                                        <asp:DropDownList ID="DropDownList5" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                    <asp:Label CssClass="control-label col-lg-6" runat="server">(日曆天)</asp:Label>
                                </div>
                                <div class="form-group-inner">
                                    <asp:Label AssociatedControlID="firstname" CssClass="control-label col-lg-2" runat="server">預計結案日：</asp:Label>
                                    <div class="col-lg-2">
                                        <asp:TextBox ID="TextBox15" CssClass="form-control" disabled runat="server"></asp:TextBox>
                                    </div>
                                    <asp:Label CssClass="control-label col-lg-6" runat="server">(=預計簽約日期+交貨天數+驗結天數，由系統幫您算出)</asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <button class="btn btn-danger" type="submit">Save</button>
                    <button class="btn btn-default" type="button">Cancel</button>
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
