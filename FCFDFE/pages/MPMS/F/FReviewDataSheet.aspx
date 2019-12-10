<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FReviewDataSheet.aspx.cs" Inherits="FCFDFE.pages.MPMS.F.FReviewDataSheet" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        .lbl-title {
            display: block;
            font-size: 32px;
        }
        .lbl-subtitle {
            text-align: center;
            font-size: 20px;
        }
    </style>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <asp:Label CssClass="lbl-title" runat="server">採購計畫審查主畫面</asp:Label>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered table-striped text-center" style="margin-bottom: 0px;">
                                <tr>
                                    <th>
                                        <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label CssClass="control-label" runat="server">審查次數</asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label CssClass="control-label" runat="server">分派日</asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label CssClass="control-label" runat="server">確認審</asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label CssClass="control-label" runat="server">審查綜簽日</asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label CssClass="control-label" runat="server">主辦單位</asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label CssClass="control-label" runat="server">主辦人</asp:Label>
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblONB_CHECK_TIMES" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label ID="lblONB_CHECK_STATUS" CssClass="control-label text-red" runat="server"></asp:Label>
                                        <%--<asp:Label CssClass="control-label" runat="server">1(確認審)</asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_DAUDIT_ASSIGN" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_CHECK_OK" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_DRESULT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_ONNAME" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_CHECKER" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnADVICE" Visible="false" runat="server">
                            <table class="table table-bordered" style="margin-bottom: 0px;">
                                <tr>
                                    <td style="width: 100px;">
                   		                <asp:Label CssClass="control-label" Text="擬辦事項" runat="server"></asp:Label>
                                    </td>
                                    <td class="td-inner-table">
                                        <asp:GridView ID="GV_ADVICE" CssClass="table table-bordered table-inner" OnPreRender="GV_ADVICE_PreRender" AutoGenerateColumns="false" runat="server">
                                            <Columns>
                                                <asp:TemplateField HeaderText="擬辦項目" >
                                                    <ItemTemplate>
                   		                               <asp:Label ID="lblOVC_ITEM" CssClass="control-label" Text='<%# Bind("OVC_ITEM")%>' runat="server"></asp:Label>
                                                       </br>
                                                       <asp:Label ID="lblOVC_ITEM_ADVICE" CssClass="control-label" Text='<%# Bind("OVC_ITEM_ADVICE")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                <asp:BoundField HeaderText="說明" DataField="OVC_ITEM_DESC" />
                	                        </Columns>
	   		                            </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                            </asp:Panel>
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width: 100px;">
                   		                <asp:Label CssClass="control-label" Text="綜審意見" runat="server"></asp:Label>
                                    </td>
                                    <td>
                   		                <asp:Label ID="lblOpinion_MEMO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                   		                <asp:Label CssClass="control-label" Text="核定事項" runat="server"></asp:Label>
                                    </td>
                                    <td>
                   		                <asp:Label ID="lblApproved_MEMO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <div class="title lbl-subtitle text-red">委購單位已上傳檔案</div>
                            <asp:GridView ID="GV_C_Alreadyupdate" CssClass=" table data-table table-striped border-top " AutoGenerateColumns="false" OnPreRender="GV_C_Alreadyupdate_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="主件名稱" DataField="OVC_IKIND" />
                                    <asp:BoundField HeaderText="附件名稱" DataField="OVC_ATTACH_NAME" />
                                    <asp:BoundField HeaderText="份數" DataField="ONB_QTY" />
                                    <asp:BoundField HeaderText="相對之上傳檔案" DataField="OVC_FILE_NAME" />
                                    <asp:BoundField HeaderText="頁數" DataField="ONB_PAGES" />
                	            </Columns>
	   		               </asp:GridView>
                            
                            <asp:Repeater ID="Repeater_Header"  OnItemDataBound="Repeater_Header_ItemDataBound" runat="server">
                                <HeaderTemplate>
                                    <table class="table table-bordered">
                                </HeaderTemplate>
                                <ItemTemplate>
                                        <tr>
                                            <td colspan="4" style="text-align:center">
                                                <asp:HiddenField id ="hidOVC_AUDIT_UNIT" Value='<%# Bind("OVC_AUDIT_UNIT")%>' runat="server" />
                                                <div class="lbl-subtitle text-blue">
                                                    <asp:Label Text="審查單位--聯審小組（" runat="server"></asp:Label>
                                                    <asp:Label ID="lblUnitName" Text='<%# Bind("OVC_USR_ID")%>' runat="server"></asp:Label>
                                                    <asp:Label Text="）" runat="server"></asp:Label>
                                                </div>
                                                <div class="text-blue" style="text-align: center;">
                                                    <asp:Label CssClass="control-label" Text="（審查者： " runat="server"></asp:Label>
                                                    <asp:Label ID="lblOVC_AUDITOR" CssClass="control-label" Text='<%# Bind("OVC_AUDITOR")%>' runat="server"></asp:Label>
                                                    <asp:Label CssClass="control-label" Text="--電話：" runat="server"></asp:Label>
                                                    <asp:Label ID="lblIUSER_PHONE" CssClass="control-label" Text='<%# Bind("IUSER_PHONE") %>' runat="server"></asp:Label>
                                                    <asp:Label CssClass="control-label" style="margin-left: 8px;" Text="回覆日：" runat="server"></asp:Label>
                                                    <asp:Label ID="lblOVC_DAUDIT" CssClass="control-label" Text='<%# Bind("OVC_DAUDIT") %>' runat="server"></asp:Label>
                                                    <asp:Label CssClass="control-label" Text="）" runat="server"></asp:Label>
                                                </div>
                                             </td>
                                         </tr>
                                      <asp:Repeater ID="Repeater_Content"  runat="server">
                                       <HeaderTemplate>
                                       </HeaderTemplate>
                                        <ItemTemplate>
                                                 <tr>
                                                   <td rowspan="2" style="width:12%">
                                                       <asp:Label CssClass="control-label" Text="審查意見(" runat="server"></asp:Label>
                                                       <asp:Label ID ="lblNO" CssClass="control-label" Text='<%# Bind("ONB_NO") %>' runat="server"></asp:Label>
                                                       <asp:Label CssClass="control-label" Text=")" runat="server"></asp:Label>
                                                   </td>
                                                   <td style="width:38%">
                                                       <asp:Label ID="lblOVC_CONTENT" CssClass="control-label text-red" Text='<%# Bind("OVC_CONTENT") %>' runat="server"></asp:Label>
                                                   </td>
                                                   <td rowspan="2" style="width:10%">
                                                       <asp:Label CssClass="control-label" Text="澄覆意見：" runat="server"></asp:Label>
                                                   </td>
                                                   <td rowspan="2" style="width:40%">
                                                       <asp:Label ID="OVC_CHECK_REASON" CssClass="control-label" Text='<%# Bind("OVC_RESPONSE") %>' runat="server"></asp:Label>
                                                   </td>
                                                </tr>
                                                <tr>
                                                   <td style="width:38%">
                                                       <asp:Label ID="lblOVC_RESPONSE" CssClass="control-label" Text='<%# Bind("OVC_CHECK_REASON") %>' runat="server"></asp:Label>
                                                   </td>
                                                </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                      </asp:Repeater>
                                    </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
