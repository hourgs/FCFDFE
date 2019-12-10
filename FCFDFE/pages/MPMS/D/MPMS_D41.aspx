<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D41.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D41" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        .subtitle1{
            font-size:18px;
            text-align:center;
            padding-bottom:10px;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title text-blue">
                    <!--標題-->
                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>購案契約製作
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;" id="divForm" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <div class="subtitle1">委辦單位：<asp:Label ID="lblOVC_AGENT_UNIT" CssClass="control-label" runat="server"></asp:Label>　購案名稱：<asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label>

                        </div>
                       <div class="text-center">
                        <img alt="↓" src="../../../images/MPMS/arrow.png" width="60"/>
                        </div>
                        <div class="text-center">
                            <asp:Button ID="btnMake" OnClick="btnMake_Click" CssClass="btn-default  " runat="server" Text="契約草稿製作" />
                        </div>
                       <div class="text-center">
                        <img alt="↓" src="../../../images/MPMS/arrow.png" width="60"/>
                        </div>
                        <div class="text-center">
                            <asp:Button ID="btnCkeck" OnClick="btnCkeck_Click" CssClass="btn-default" runat="server" Text="契約製作檢查項目" />
                        </div>
                       <div class="text-center">
                        <img alt="↓" src="../../../images/MPMS/arrow.png" width="60"/>
                        </div>
                        <div class="text-center">
                            <asp:Button ID="btnDistribution" OnClick="btnDistribution_Click" CssClass="btn-default" runat="server" Text="契約及附件分配" />
                        </div>
                       <div class="text-center">
                        <img alt="↓" src="../../../images/MPMS/arrow.png" width="60"/>
                        </div>
                        <div class="text-center">
                            <asp:Button ID="btnMove" OnClick="btnMove_Click" CssClass="btn-default btn2" runat="server" Text="移履約" />
                        </div>
                        <br />

                        <div class="subtitle">採購發包階段上傳檔案 </div>
                        <asp:GridView ID="gvFiles" CssClass="table data-table table-striped border-top text-center" Width="100%" AutoGenerateColumns="False" OnRowCommand="gvFiles_RowCommand" runat="server" >
 		     		        <Columns>
                                <asp:TemplateField HeaderText="選擇">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelect" cssclass="btn-warning btnw2" Text="刪除" CommandName="DeleteFile" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server"/>
                                        <asp:Label ID="lblFileName" Text='<%# Eval("FileName") %>' Style="display:none" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:HyperLinkField DataTextField="LinkFileName" HeaderText="上傳之檔案名稱" />
                                <asp:BoundField DataField="Time" HeaderText="時間" />     
                                  <asp:TemplateField HeaderText="檔案大小">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("FileSize") %>' runat="server"></asp:Label> KB
                                    </ItemTemplate>
                                </asp:TemplateField>
                	        </Columns>
	   		            </asp:GridView><br />

                        <table class="table table-bordered text-center">
                            <tr>
                                <td><asp:Button ID="btnUpload" CssClass="btn-default btnw2" Text="上傳" OnClick="btnUpload_Click" runat="server"/></td>
                                <td style="width:80%">
                                    <asp:FileUpload ID="FileUpload" title="瀏覽..." runat="server" /></td>
                            </tr>
                        </table>
                    </div>

                    <div class="text-center">
                        <asp:Button ID="btnReturnS" OnClick="btnReturnS_Click" CssClass="btn-default" runat="server" Text="回契約製作選擇畫面" />
                        <asp:Button ID="btnReturnM" OnClick="btnReturnM_Click" CssClass="btn-default" runat="server" Text="回主流程畫面" />
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
