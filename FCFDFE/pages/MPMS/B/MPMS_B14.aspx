<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_B14.aspx.cs" Inherits="FCFDFE.pages.MPMS.B.MPMS_B141" %>

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
                    <!--標題-->購案審查澄覆作業(購案編號：
                    <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                    )
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div id="divMain" runat="server" style="text-align:center">
                                <asp:GridView ID="GV_Clarification" CssClass="table data-table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:TemplateField HeaderText="作業" >
                                            <ItemTemplate>
                   		                        <asp:Button ID="btnQuery" CssClass="btn-success btnw2" Text="查詢" OnClick="btnQuery_Click" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                        <asp:BoundField HeaderText="收案次數" DataField="ONB_CHECK_TIMES" />
                                        <asp:BoundField HeaderText="收案日" DataField="OVC_DRECEIVE_PAPER" />
                                        <asp:BoundField HeaderText="聯審結果" DataField="OVC_CHECK_OK" />
                                        <asp:BoundField HeaderText="主辦單位" DataField="OVC_CHECK_UNIT" />
                                        <asp:BoundField HeaderText="聯審單位" DataField="OVC_PHR_DESC" />
                	                </Columns>
	   		                    </asp:GridView>
                                <asp:Button ID="btnReturn" OnClick="btnReturn_Click" CssClass="btn-success btnw4" runat="server" Text="回主畫面" /><!--綠色-->
                            </div>
                            <div id="divContent" visible="false" runat="server">
                                <asp:GridView ID="GV_ClarDetail" CssClass="table data-table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                                        <asp:BoundField HeaderText="審查次數" DataField="ONB_CHECK_TIMES" />
                                        <asp:BoundField HeaderText="分派日" DataField="OVC_DRECEIVE" />
                                        <asp:BoundField HeaderText="確認審" DataField="OVC_CHECK_OK" />
                                        <asp:BoundField HeaderText="審查綜簽日" DataField="OVC_DRESULT" />
                                        <asp:BoundField HeaderText="主辦單位" DataField="OVC_CHECK_UNIT" />
                                        <asp:BoundField HeaderText="主辦人" DataField="OVC_CHECKER" />
                	                </Columns>
	   		                    </asp:GridView>
                                <!--巢狀Repeater(2層)--->
                                 <asp:Repeater ID="Repeater_Header" OnItemCommand="Repeater_Header_ItemCommand" OnItemDataBound="Repeater_Header_ItemDataBound" runat="server">
                                      <HeaderTemplate>
                                      </HeaderTemplate>
                                      <ItemTemplate>
                                           <table class="table table-bordered table-striped text-center">
                                                      <tr>
                                                          <td>
                                                              <asp:HiddenField ID="hidOVC_AUDIT_UNIT" Value='<%# Bind("OVC_AUDIT_UNIT")%>' runat="server" />
                                                              <asp:Label CssClass="control-label" Text="審查單位--" runat="server"></asp:Label>
                                                              <asp:Label ID="lblUnitName" CssClass="control-label" Text='<%# Bind("UnitName")%>' runat="server"></asp:Label>
                                                              <asp:Label CssClass="control-label" Text="(審查者： " runat="server"></asp:Label>
                                                              <asp:Label ID="lblName" CssClass="control-label" Text='<%# Bind("OVC_AUDITOR")%>' runat="server"></asp:Label>
                                                              <asp:Label CssClass="control-label" Text=") -- 電話：" runat="server"></asp:Label>
                                                              <asp:Label ID="lblPhone" CssClass="control-label" Text='<%# Bind("OVC_PUR_IUSER_PHONE") %>' runat="server"></asp:Label>
                                                          </td>
                                                      </tr>
                                              <asp:Repeater ID="Repeater_Content" OnPreRender="Repeater_Content_PreRender" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-bordered table-striped">
                                                        <tr>
                                                            <th style="width:5%">
                                                                項次
                                                            </th>
                                                            <th style="width:10%">
                                                                審查類別
                                                            </th>
                                                            <th style="width:10%">
                                                                審查項目
                                                            </th>
                                                            <th style="width:25%">
                                                                審查標題
                                                            </th>
                                                            <th style="width:50%">
                                                                澄覆意見
                                                            </th>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                         <tr>
                                                           <td><asp:Label ID="lblONB_NO" CssClass="control-label" Text=<%# Bind("ONB_NO")%> runat="server"></asp:Label>
                                                               <asp:HiddenField ID="hidONB_NO" Value='<%# Bind("ONB_NO")%>' runat="server" />
                                                           </td>
                                                             <td><%# Eval("OVC_TITLE_NAME")%>
                                                                 <asp:HiddenField ID="hidOVC_TITLE" Value='<%# Bind("OVC_TITLE")%>' runat="server" />
                                                             </td>
                                                             <td><%# Eval("OVC_TITLE_ITEM_NAME")%>
                                                                 <asp:HiddenField ID="hidOVC_TITLE_ITEM" Value='<%# Bind("OVC_TITLE_ITEM")%>' runat="server" />
                                                             </td>
                                                             <td><%# Eval("OVC_TITLE_DETAIL_NAME")%>
                                                                 <asp:HiddenField ID="hidOVC_TITLE_DETAIL" Value='<%# Bind("OVC_TITLE_DETAIL")%>' runat="server" />
                                                             </td>
                                                             <td rowspan="2"><asp:TextBox ID="txtOVC_RESPONSE" CssClass="tb tb-full" TextMode="MultiLine" Height="100%" Rows=<%# Eval("OVC_RESPONSE").ToString().Length/30 +1 %> Text=<%# Bind("OVC_RESPONSE")%> runat="server"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>審查<br/>意見</td>
                                                            <td colspan="3"><%# Eval("OVC_CHECK_REASON")%></td>
                                                        </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                   </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                               <div style="text-align:center"><asp:Button ID="btnSave" CommandName="Save"  runat="server" Visible="false" CssClass="btn-success" Text="存檔" /></div>
                                               <p></p>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                         </table>
                                     </FooterTemplate>
                                 </asp:Repeater>
                                <div style="text-align:center">
                                    <asp:Button ID="btnPresented" CssClass="btn-success" Text="澄清無誤轉呈採購中心" OnClick="btnPresented_Click" runat="server"/>
                                    <asp:Button ID="btnToOtherPurch" OnClick="btnToOtherPurch_Click" CssClass="btn-success" Text="處理其他購案" runat="server"/>
                                    <asp:Button ID="btnShowMain" CssClass="btn-success" OnClick="btnShowMain_Click"  Text="回上一頁" runat="server"/>
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
