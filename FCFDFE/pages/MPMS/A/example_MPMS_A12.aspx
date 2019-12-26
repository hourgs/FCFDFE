<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="example_MPMS_A12.aspx.cs" Inherits="FCFDFE.pages.MPMS.A.example_MPMS_A12" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function(){
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header class="title">
                    採購預劃購案編輯
                </header>

                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body">
                    <div class="form">
                        <div class="cmxform form-horizontal tasi-form">
                            <div class="form-group">
                                <div class="col-lg-6">
                                    <asp:Label AssociatedControlID="lblPrePurNum" CssClass="control-label" runat="server">預劃購案編號：</asp:Label>
                                    <asp:Label ID="lblPrePurNum" CssClass="" runat="server">123</asp:Label>
                                </div>
                                <div class="col-lg-6">
                                    <asp:Label AssociatedControlID="drpPurAreaMethod" CssClass="control-label" runat="server">採購單位地區及方式：</asp:Label>
                                    <asp:DropDownList ID="drpPurAreaMethod" CssClass="" runat="server">
                                        <asp:ListItem>內容1</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-lg-6">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">美軍編號：</asp:Label>
                                </div>
                                <div class="col-lg-6">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">軍售案類別：</asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-lg-6">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">購案名稱：</asp:Label>
                                    <asp:TextBox ID="TextBox3" CssClass="" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-lg-6">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">招標方式：</asp:Label>
                                    <asp:DropDownList ID="DropDownList1" CssClass="" runat="server">
                                        <asp:ListItem>內容1</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-lg-6">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">專案代號(適用裝備)：</asp:Label>
                                        <asp:TextBox ID="TextBox2" CssClass="" runat="server"></asp:TextBox>
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">採購屬性：</asp:Label>
                                    <asp:DropDownList ID="DropDownList10" CssClass="" runat="server">
                                        <asp:ListItem>內容1</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-lg-6">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">計畫性質：</asp:Label>
                                    <asp:DropDownList ID="DropDownList12" CssClass="" runat="server">
                                        <asp:ListItem>內容1</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-lg-6">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">核定權責：</asp:Label>
                                    <asp:DropDownList ID="DropDownList2" CssClass="" runat="server"></asp:DropDownList>
                                    <asp:Button ID="Button1" CssClass="btn-info btnw6" runat="server" Text="核定權責說明" />
                                </div>
                                <div class="col-lg-6">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">是否為1月1日須執行之購案：</asp:Label>
                                    <asp:DropDownList ID="DropDownList4" CssClass="" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-lg-3">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">承辦人姓名：</asp:Label>
                                    <asp:TextBox ID="TextBox5" CssClass="" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-lg-9">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">電話</asp:Label>
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">軍線1：</asp:Label>
                                    <asp:TextBox ID="TextBox6" CssClass="" runat="server"></asp:TextBox>
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">軍線2：</asp:Label>
                                    <asp:TextBox ID="TextBox1" CssClass="" runat="server"></asp:TextBox>
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">自動：</asp:Label>
                                    <asp:TextBox ID="TextBox8" CssClass="" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-lg-12">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">承辦人手機：</asp:Label>
                                    <asp:TextBox ID="TextBox4" CssClass="" runat="server"></asp:TextBox>
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">承辦人E-MAIL：</asp:Label>
                                    <asp:TextBox ID="TextBox7" CssClass="" runat="server"></asp:TextBox>
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">傳真號碼：</asp:Label>
                                    <asp:TextBox ID="TextBox16" CssClass="" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-lg-12">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">本案委託購案，委託單位：</asp:Label>
                                    <asp:TextBox ID="TextBox10" CssClass="" runat="server"></asp:TextBox>
                                    <asp:Button ID="Button9" CssClass="btn-info btnw4" runat="server" Text="單位查詢" />
                                    <asp:Button ID="Button10" CssClass="btn-info btnw6" runat="server" Text="委託單位說明" />
                                    <asp:TextBox ID="TextBox9" CssClass="" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">

                            </div>
                            <div class="form-group">
                                <div class="col-lg-12">

                                </div>
                                <div class="col-lg-12">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" ForeColor="Red" runat="server">最後計畫評核(審查)單位：</asp:Label>
                                    <asp:TextBox ID="TextBox24" CssClass="" runat="server"></asp:TextBox>
                                    <asp:TextBox ID="TextBox11" CssClass="" runat="server"></asp:TextBox>
                                    <asp:Button ID="Button11" CssClass="btn-info btnw4" runat="server" Text="單位查詢" />
                                </div>
                                <div class="col-lg-12">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">採購發包單位：</asp:Label>
                                    <asp:TextBox ID="TextBox12" CssClass="" runat="server"></asp:TextBox>
                                    <asp:TextBox ID="TextBox25" CssClass="" runat="server"></asp:TextBox>
                                    <asp:Button ID="Button2" CssClass="btn-info btnw4" runat="server" Text="單位查詢" />
                                </div>
                                <div class="col-lg-12">

                                </div>
                                <div class="col-lg-12">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">履約驗結單位：</asp:Label>
                                    <asp:TextBox ID="TextBox26" CssClass="" runat="server"></asp:TextBox>
                                    <asp:TextBox ID="TextBox27" CssClass="" runat="server"></asp:TextBox>
                                    <asp:Button ID="Button3" CssClass="btn-info btnw4" runat="server" Text="單位查詢" />
                                </div>
                                <div class="col-lg-12">
                                    <asp:Label ID="Label3" CssClass="control-label" runat="server" Text="(若仍由採購室下授委辦單位履約者，仍請選定採購室為履約驗結單位)" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">

                            </div>
                            <div class="form-group">
                                <div class="col-lg-12 form-group-inner">
                                    <asp:Label AssociatedControlID="" CssClass="control-label position-left" runat="server">預計申辦日期：</asp:Label>
                                    <div class="input-append date position-left" id="dpYears" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                        <asp:TextBox ID="TextBox20" CssClass="tb tb-s position-left" runat="server" readonly="true" ></asp:TextBox>
                                        <div class="add-on"><i class="icon-calendar"></i></div>
                                    </div>
                                    <asp:Label CssClass="control-label"  runat="server">(西元年，例如:2000-01-01)</asp:Label>
                                </div>
                                <div class="col-lg-12">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">審核天數：</asp:Label>
                                    <asp:TextBox ID="TextBox30" CssClass="" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label" runat="server">(日曆天)</asp:Label>
                                </div>
                                <div class="col-lg-12">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">預計核定日：</asp:Label>
                                    <asp:TextBox ID="TextBox13" CssClass="" readonly="true" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label" runat="server">(=預計呈報日期+審核天數，由系統幫您算出)</asp:Label>
                                </div>
                                <div class="col-lg-12">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">招標天數：</asp:Label>
                                    <asp:DropDownList ID="DropDownList3" CssClass="" runat="server"></asp:DropDownList>
                                    <asp:Label CssClass="control-label" runat="server">(日曆天)</asp:Label>
                                </div>
                                <div class="col-lg-12">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">預計簽約日：</asp:Label>
                                    <asp:TextBox ID="TextBox14" CssClass="tb tb-s" readonly="true" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label" runat="server">(=預計呈報日期+審核天數，由系統幫您算出)</asp:Label>
                                </div>
                            </div>
                            <asp:Label AssociatedControlID="" CssClass="control-label" style="float: left; width: 20px;" ForeColor="Red" runat="server">交貨天數</asp:Label>
                            <div class="form-group col-lg-5" style="float: left;border-left:1px solid;">
                                <div class="col-lg-6" style="border-bottom:1px solid;">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">交貨天數：</asp:Label>
                                    <asp:TextBox ID="TextBox17" CssClass="" runat="server"></asp:TextBox>
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">或 限定交貨日期：</asp:Label>
                                    <input id="dp1" type="text" value="12-02-2012" size="16" class="">
                                    <asp:Label CssClass="control-label" runat="server">(二擇一來做你的設定)</asp:Label>
                                </div>
                                <div class="col-lg-12" style="border-bottom:1px solid;">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">驗結天數：</asp:Label>
                                    <asp:DropDownList ID="DropDownList5" CssClass="" runat="server"></asp:DropDownList>
                                    <asp:Label CssClass="control-label" runat="server">(日曆天)</asp:Label>
                                </div>
                                <div class="col-lg-12" style="border-bottom:1px solid;">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">預計結案日：</asp:Label>
                                    <asp:TextBox ID="TextBox15" CssClass="" readonly="true" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label" runat="server">(=預計簽約日期+交貨天數+驗結天數，由系統幫您算出)</asp:Label>
                                </div>
                                <div class="col-lg-12" style="border-bottom:1px solid;">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">預計結案日：</asp:Label>
                                    <asp:TextBox ID="TextBox18" CssClass="" readonly="true" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label" runat="server">(=預計簽約日期+交貨天數+驗結天數，由系統幫您算出)</asp:Label>
                                </div>
                                <div class="col-lg-12">
                                    <asp:Label AssociatedControlID="" CssClass="control-label" runat="server">預計結案日：</asp:Label>
                                    <asp:TextBox ID="TextBox19" CssClass="" readonly="true" runat="server"></asp:TextBox>
                                    <asp:Label CssClass="control-label" runat="server">(=預計簽約日期+交貨天數+驗結天數，由系統幫您算出)</asp:Label>
                                </div>

                            </div>
                       
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                   		<asp:Button ID="btnSave" CssClass="btn-warning btnw2" runat="server" Text="儲存" /><!--黃色-->
                   		<asp:Button ID="btnDel" CssClass="btn-danger btnw2" runat="server" Text="刪除" /><!--紅色-->
                    	<asp:Button ID="btnOther" CssClass="btn-success btnw4" runat="server" Text="查詢" /><!--綠色-->
                   		<asp:Button ID="btnReset" CssClass="btn-default btnw4" runat="server" Text="清除" /><!--灰色-->
                </footer>
            </section>

            <section class="panel">
                <header  class="title">
                    字串合併
                </header>
                <asp:Panel ID="Panel1" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">字串一</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtString1"  CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        <asp:Label CssClass="control-label" runat="server">字串二</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtString2" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="text-align:center;vertical-align:middle;">
                                        
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" ID="lblString" text="lblString" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnMerge" cssclass="btn-warning btnw2" OnClick="btnMerge_Click" runat="server" Text="合併" /><br />
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                </footer>
            </section>
            <section class="panel">
                <header  class="title">
                    字串合併
                </header>
                <asp:Panel ID="Panel2" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            
                            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataSourceID="SqlDataSource2">
                                <Columns>
                                    <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                                    <asp:BoundField DataField="C_SN_ROLE" HeaderText="C_SN_ROLE" SortExpression="C_SN_ROLE" />
                                    <asp:BoundField DataField="IS_ENABLE" HeaderText="IS_ENABLE" SortExpression="IS_ENABLE" />
                                    <asp:BoundField DataField="IS_UPLOAD" HeaderText="IS_UPLOAD" SortExpression="IS_UPLOAD" />
                                    <asp:BoundField DataField="IS_PRO" HeaderText="IS_PRO" SortExpression="IS_PRO" />
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" DeleteCommand="DELETE FROM &quot;ACCOUNT_AUTH&quot; WHERE &quot;USER_ID&quot; = ? AND &quot;C_SN_ROLE&quot; = ? AND ((&quot;IS_ENABLE&quot; = ?) OR (&quot;IS_ENABLE&quot; IS NULL AND ? IS NULL)) AND ((&quot;IS_UPLOAD&quot; = ?) OR (&quot;IS_UPLOAD&quot; IS NULL AND ? IS NULL)) AND ((&quot;IS_PRO&quot; = ?) OR (&quot;IS_PRO&quot; IS NULL AND ? IS NULL))" InsertCommand="INSERT INTO &quot;ACCOUNT_AUTH&quot; (&quot;USER_ID&quot;, &quot;C_SN_ROLE&quot;, &quot;IS_ENABLE&quot;, &quot;IS_UPLOAD&quot;, &quot;IS_PRO&quot;) VALUES (?, ?, ?, ?, ?)" OldValuesParameterFormatString="original_{0}" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT * FROM &quot;ACCOUNT_AUTH&quot;" UpdateCommand="UPDATE &quot;ACCOUNT_AUTH&quot; SET &quot;C_SN_ROLE&quot; = ?, &quot;IS_ENABLE&quot; = ?, &quot;IS_UPLOAD&quot; = ?, &quot;IS_PRO&quot; = ? WHERE &quot;USER_ID&quot; = ? AND &quot;C_SN_ROLE&quot; = ? AND ((&quot;IS_ENABLE&quot; = ?) OR (&quot;IS_ENABLE&quot; IS NULL AND ? IS NULL)) AND ((&quot;IS_UPLOAD&quot; = ?) OR (&quot;IS_UPLOAD&quot; IS NULL AND ? IS NULL)) AND ((&quot;IS_PRO&quot; = ?) OR (&quot;IS_PRO&quot; IS NULL AND ? IS NULL))">
                                <DeleteParameters>
                                    <asp:Parameter Name="original_USER_ID" Type="Object" />
                                    <asp:Parameter Name="original_C_SN_ROLE" Type="String" />
                                    <asp:Parameter Name="original_IS_ENABLE" Type="String" />
                                    <asp:Parameter Name="original_IS_ENABLE" Type="String" />
                                    <asp:Parameter Name="original_IS_UPLOAD" Type="String" />
                                    <asp:Parameter Name="original_IS_UPLOAD" Type="String" />
                                    <asp:Parameter Name="original_IS_PRO" Type="Decimal" />
                                    <asp:Parameter Name="original_IS_PRO" Type="Decimal" />
                                </DeleteParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="USER_ID" Type="Object" />
                                    <asp:Parameter Name="C_SN_ROLE" Type="String" />
                                    <asp:Parameter Name="IS_ENABLE" Type="String" />
                                    <asp:Parameter Name="IS_UPLOAD" Type="String" />
                                    <asp:Parameter Name="IS_PRO" Type="Decimal" />
                                </InsertParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="C_SN_ROLE" Type="String" />
                                    <asp:Parameter Name="IS_ENABLE" Type="String" />
                                    <asp:Parameter Name="IS_UPLOAD" Type="String" />
                                    <asp:Parameter Name="IS_PRO" Type="Decimal" />
                                    <asp:Parameter Name="original_USER_ID" Type="Object" />
                                    <asp:Parameter Name="original_C_SN_ROLE" Type="String" />
                                    <asp:Parameter Name="original_IS_ENABLE" Type="String" />
                                    <asp:Parameter Name="original_IS_ENABLE" Type="String" />
                                    <asp:Parameter Name="original_IS_UPLOAD" Type="String" />
                                    <asp:Parameter Name="original_IS_UPLOAD" Type="String" />
                                    <asp:Parameter Name="original_IS_PRO" Type="Decimal" />
                                    <asp:Parameter Name="original_IS_PRO" Type="Decimal" />
                                </UpdateParameters>
                            </asp:SqlDataSource>
                            
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