<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_C18.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.MPMS_C18" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <script type="text/javascript">
        function setHeight(txtdesc) {
            txtdesc.style.height = txtdesc.scrollHeight + "px";
        }
    </script>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    計畫評核－綜簽
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server">AA0980l</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">採購號</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_PURCH_NO" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">購案名稱</asp:Label>
                                    </td>
                                    <td colspan="3"  class="text-left">
                                        <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server">購案測試01</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">委購單位</asp:Label>
                                    </td>
                                    <td colspan="7" class="text-left">
                                        <asp:Label ID="lblOVC_AGENT_UNIT" CssClass="control-label" runat="server">國防部政務辦公室-陳XX(11 軍線:237543)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">審查次數</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblONB_CHECK_TIMES" CssClass="control-label" runat="server">1</asp:Label>
                                        <asp:Label ID="lblONB_CHECK_STATUS" CssClass="control-label text-red" runat="server">(初審)</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">分派日</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_DAUDIT_ASSIGN" CssClass="control-label" runat="server">106年04月03日</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">綜辦單位</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label ID="lblOVC_CHECK_UNIT" CssClass="control-label" runat="server">國防部國防採購室(簡XX)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">確認審</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_CHECK_OK" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value ="N">否</asp:ListItem>
                                            <asp:ListItem Value ="Y">是</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">紙本收文日</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOVC_DRECEIVE_PAPER" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">綜審綜簽日</asp:Label>
                                    </td>
                                    <td>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOVC_DRESULT" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_DRESULT_PRINT" CssClass="control-label" runat="server">綜審綜簽簽呈列印</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8" class="text-center">
                                        <asp:Label CssClass="control-label position-left text-red" runat="server">退委購單位澄清日：</asp:Label><!--前方標籤文字，跟日期同一行需使用"position-left"之class-->
                                        <!--↓日期套件↓-->
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOVC_DREJECTs" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">核定審備註</asp:Label>
                                    </td>
                                    <td colspan="7" class="text-left">
                                        <asp:TextBox ID="txtOVC_INTEGRATED_REASON" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table id="tbADVICE" class="table table-bordered" visible="false" runat="server">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnToADVICE" cssclass="btn-success btnw"  CommandName="btnToADVICE" OnCommand="btnTo_Command" runat="server" Text="擬辦事項編輯" />
                                    </td>
                                    <td>
                                        <asp:GridView ID="GV_ADVICE" CssClass="table table-bordered" OnPreRender="GV_ADVICE_PreRender" AutoGenerateColumns="false" runat="server">
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
                                <tr>
                                    <td>
                                        <asp:Button ID="btnToAllMemo" cssclass="btn-success btnw" CommandName="btnToAllMemo" OnCommand="btnTo_Command" runat="server" Text="綜審意見編輯" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnToAppMemo" cssclass="btn-success btnw" CommandName="btnToAppMemo" OnCommand="btnTo_Command" runat="server" Text="會辦意見編輯" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Button ID="btnToOKmemo" cssclass="btn-success btnw" CommandName="btnToOKmemo" OnCommand="btnTo_Command" runat="server" Text="核定事項編輯" />
                                    </td>
                                    <td class="text-center">
                                        <asp:Label ID="Label1" CssClass="control-label text-red" runat="server">
                                            注意：當確認審為「是」時，本核定事項將列印於物資申請書之「審查意見」欄。
                                        </asp:Label>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                   		                <asp:TextBox ID="txtApproved_MEMO" CssClass="tb tb-full" TextMode="MultiLine" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnToAttch" cssclass="btn-success btnw" CommandName="btnToAttch" OnCommand="btnTo_Command" runat="server" Text="附件分發單位編輯" />
                                    </td>
                                     <td>
                                        <asp:GridView ID="GV_Attached" CssClass="table table-bordered" AutoGenerateColumns="false" runat="server">
                                            <Columns>
                                                 <asp:TemplateField HeaderText="附件分發單位說明" >
                                                    <ItemTemplate>
                   		                               <asp:Label ID="lblTARGET_UNIT" CssClass="control-label" Text='<%# Bind("TARGET_UNIT")%>' runat="server"></asp:Label>
                                                        <asp:Label ID="Label2" CssClass="control-label" runat="server">　　</asp:Label>
                                                       <asp:Label ID="lblOVC_ATTACH_NAME" CssClass="control-label" Text='<%# Bind("OVC_ATTACH_NAME")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                	                        </Columns>
	   		                            </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                 <asp:Button ID="btnSave" cssclass="btn-success btnw" OnClick="btnSave_Click" runat="server" Text="存檔"/>
                                <asp:Button ID="btnReturn" cssclass="btn-success btnw" OnClick="btnReturn_Click" runat="server" Text="回上頁" />
                            </div>
                            <br/>
                            <br/>
                            <div id="divPrint" runat="server" visible="false" class="text-center">
                                <asp:Button ID="btnPrintInfoPDF" cssclass="btn-success btnw" runat="server" OnCommand="btnPrint_Command" CommandName="btnPrintInfoPDF" Text="列印基本資料" />
                                <asp:Button ID="Button5" cssclass="btn-success btnw" runat="server" Text="列印審查意見" />
                                 <asp:Button ID="Button6" cssclass="btn-success btnw" runat="server" Text="列印會辦意見" />
                                <asp:Button ID="btnPrintAppMemo" cssclass="btn-success btnw" CommandName="btnPrintAppMemo" OnCommand="btnPrint_Command" runat="server" Text="列印核定事項" />
                                <br/>
                                <br/>
                                <asp:Button ID="btnPrintCheckOKPDF" cssclass="btn-success btnw" runat="server" OnCommand="btnPrint_Command" CommandName="btnPrintCheckOKPDF" Text="列印確認審會辦單" />
                                <asp:Button ID="btnPrintMaterial" cssclass="btn-success btnw" runat="server"  OnCommand="btnPrint_Command" CommandName="btnPrintMaterial" Text="列印物資申請書" />
                            </div>
                            <div class=" text-center subtitle text-red"><h3>聯審小組審查意見</h3>
                                <asp:Label ID="lblUNIT" CssClass="control-label" runat="server"></asp:Label>
                            </div>
                            <asp:Repeater ID="Repeater_Header"  OnItemDataBound="Repeater_Header_ItemDataBound" runat="server">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table class="table table-bordered table-striped">
                                        <tr>
                                            <td colspan="4" style="text-align:center">
                                                <asp:HiddenField id ="hidOVC_AUDIT_UNIT" Value='<%# Bind("OVC_AUDIT_UNIT")%>' runat="server" />
                                                <asp:Label CssClass="control-label" Text="審查單位--" runat="server"></asp:Label>
                                                <asp:Label ID="lblUnitName" CssClass="control-label" Text='<%# Bind("OVC_USR_ID")%>' runat="server"></asp:Label>
                                                <asp:Label CssClass="control-label" Text="(審查者： " runat="server"></asp:Label>
                                                <asp:Label ID="lblName" CssClass="control-label" Text='<%# Bind("OVC_AUDITOR")%>' runat="server"></asp:Label>
                                                <asp:Label CssClass="control-label" Text=") -- 電話：" runat="server"></asp:Label>
                                                <asp:Label ID="lblPhone" CssClass="control-label" Text='<%# Bind("IUSER_PHONE") %>' runat="server"></asp:Label>
                                                <asp:Label CssClass="control-label" Text="作業天數：" runat="server"></asp:Label>
                                                <asp:Label ID="lblPROCESSDATE" CssClass="control-label" Text='<%# Bind("PROCESS") %>' runat="server"></asp:Label>
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
                                                       <asp:Label ID="lblOVC_CONTENT" CssClass="control-label" Text='<%# Bind("OVC_CONTENT") %>' runat="server"></asp:Label>
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
