<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_C13_1.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.MPMS_C13_1" %>
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
                    <!--標題-->複數決標案--查核預算分組作業
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="subtitle">購案編號：</div>
                            <table  style="text-align:center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server" Font-Size="X-Large" >購案編號：</asp:Label>
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label" Font-Size="X-Large" runat="server" ></asp:Label>
                                        <br/>
                                        <br/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblICheck" CssClass="control-label" runat="server" Font-Size="X-Large" >本案尚未查核</asp:Label>
                                        <asp:Label ID="lblCheckDate" CssClass="control-label" runat="server" Font-Size="X-Large" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br/>
                                        <br/>
                                        <asp:Button ID="btnCheck" CssClass="btn-success" OnClick="btnCheck_Click" runat="server" Text="確認查核無誤" /><!--綠色-->
                                        <asp:Button ID="btnReturn" CssClass="btn-success" OnClick="btnReturn_Click" runat="server" Text="回上一頁" /><!--綠色-->
                                        <asp:Button ID="btnBackMain" CssClass="btn-success btnw4" OnClick="btnBackMain_Click" runat="server" Text="回主畫面" /><!--綠色-->
                                    </td>
                                 </tr>
                                 <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server" Font-Size="Medium" >本案共分</asp:Label>
                                        <asp:Label ID="lblGroupNum" CssClass="control-label" runat="server" Font-Size="Medium" ></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server" Font-Size="Medium" >組</asp:Label>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td>
                                      <asp:DropDownList ID="drpGroupNum" OnSelectedIndexChanged="drpGroupNum_SelectedIndexChanged" AutoPostBack="true" CssClass="tb tb-s" runat="server"></asp:DropDownList> 
                                    </td>
                                </tr>
                            </table>
                             <table class="table table-bordered text-center">
                                 <tr>
                                     <th colspan="3">
                                     <div class="text-center">
                                        <asp:Label CssClass="control-label text-blue" runat="server">組別：</asp:Label>
                                        <asp:Label ID="lbldrpselect" CssClass="control-label text-blue" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label text-blue" runat="server">所選的項目</asp:Label>
                                     </div>
                                     </th>
                                 </tr>
                                 <tr>
                                     <td style="width:49%">
                                         <asp:GridView ID="gvGroupLeft" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                             <Columns>
                                                 <asp:BoundField HeaderText="項次" DataField="ONB_POI_ICOUNT" />
                                                 <asp:BoundField HeaderText="品名" DataField="OVC_POI_NSTUFF_CHN" />
                                                 <asp:BoundField HeaderText="廠牌/規格" DataField="OVC_BRAND" />
                                             </Columns>
                                         </asp:GridView>
                                     </td>
                                     <td style="width:2%">

                                     </td>
                                     <td style="width:49%">
                                           <asp:GridView ID="gvGroupRight" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                 <Columns>
                                     <asp:BoundField HeaderText="項次" DataField="ONB_POI_ICOUNT" />
                                     <asp:BoundField HeaderText="品名" DataField="OVC_POI_NSTUFF_CHN" />
                                     <asp:BoundField HeaderText="廠牌/規格" DataField="OVC_BRAND" />
                                 </Columns>
                            </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <table class="table table-bordered text-center">
                             <tr>
                            <td style="width:49%">
                                <asp:GridView ID="gvONB_POI_ICOUNT_LEFT" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:BoundField HeaderText="項次" DataField="ONB_POI_ICOUNT" />
                                        <asp:BoundField HeaderText="組別" DataField="ONB_GROUP_PRE" />
                                        <asp:BoundField HeaderText="品名" DataField="OVC_POI_NSTUFF_CHN" />
                                        <asp:BoundField HeaderText="廠牌/規格" DataField="OVC_BRAND" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                            <td style="width:2%">
                            </td>
                            <td style="width:49%">
                                <asp:GridView ID="gvONB_POI_ICOUNT_Right" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:BoundField HeaderText="項次" DataField="ONB_POI_ICOUNT" />
                                        <asp:BoundField HeaderText="組別" DataField="ONB_GROUP_PRE" />
                                        <asp:BoundField HeaderText="品名" DataField="OVC_POI_NSTUFF_CHN" />
                                        <asp:BoundField HeaderText="廠牌/規格" DataField="OVC_BRAND" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                           </tr>
                       </table>
                    </div>
                </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
