<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_B13_3.aspx.cs" Inherits="FCFDFE.pages.MPMS.B.MPMS_B13_3" %>
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
                    <!--標題-->附件及檔案上傳作業
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="text-center">
                                <asp:Label CssClass="control-label" runat="server" Text="購案編號："></asp:Label>
                                <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                            </div>
                            <!--附件明細資料-->
                            <div id="divUploadDetail" runat="server">
                                <div class="text-center">
                                <asp:Button ID="btnReturn" CssClass="btn-warning" runat="server" Text="回物資申請書編制作業" OnClick="btnReturn_Click"/><br><br>
                                <asp:Label ID="lblTitle" CssClass="control-label text-red" runat="server" Text=""></asp:Label>
                                    <asp:Label CssClass="control-label text-red" runat="server" Text="附件明細"></asp:Label>
                            </div>
                            <asp:GridView ID="GV_OVC_ISOURCE" CssClass="table table-striped border-top text-center" OnPreRender="GV_OVC_ISOURCE_PreRender" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="作業">
                                        <ItemTemplate>
                                            <asp:Button ID="btn_change" CssClass="btn-success btnw2" Text="異動" CommandName="按鈕屬性" OnClick="btn_change_Click" runat="server" />
                                            <asp:Button ID="btnDel" CssClass="btn-danger btnw2" Text="刪除" CommandName="按鈕屬性" OnClick="btnDel_Click" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="主件名稱" DataField="OVC_IKINDTEXT" />
                                    <asp:BoundField HeaderText="附件名稱" DataField="OVC_ATTACH_NAME" />
                                    <asp:BoundField HeaderText="份數" DataField="ONB_QTY" />
                                    <asp:TemplateField HeaderText="相對之上傳檔案">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hidOVC_FILE_NAME" Value='<%# Bind("OVC_FILE_NAME") %>' runat ="server" />
                                            <asp:Button ID="btnToUpload" CssClass="btn-success btnw2" Text="上傳" OnClick="btnToUpload_Click" CommandName="按鈕屬性" runat="server" />
                                            <asp:LinkButton ID="btn_downloadFile" Text ='<%# Bind("OVC_FILE_NAME") %>' OnClick="btn_downloadFile_Click" runat="server"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="頁數" DataField="ONB_PAGES" />
                                </Columns>
                            </asp:GridView>
                            <div class="text-center">
                                <asp:Label CssClass="control-label text-red" runat="server" Text="請於附件名稱最前面，自行加入&quot;項次&quot;，以便依序排列列印。(如：1.採購計畫清單)..."></asp:Label>
                            </div>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td><asp:Button ID="btnNew" CssClass="btn-success btnw2" OnClick="btnNew_Click"  runat="server" Text="新增" /></td>
                                    <td>
                                        <asp:Label ID="lblOVC_IKIND_NAME" CssClass="control-label text-blue" runat="server" Text=""></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server" Text="附件名稱："></asp:Label>
                                        <asp:DropDownList ID="drpOVC_ATTACH_NAME" CssClass="tb tb-m" OnSelectedIndexChanged="drpOVC_ATTACH_NAME_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                        <asp:TextBox ID="txtOVC_ATTACH_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <p></p>
                                        <asp:Label CssClass="control-label" runat="server" Text="份數："></asp:Label>
                                        <asp:TextBox ID="txtONB_QTY" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server" Text="頁數："></asp:Label>
                                        <asp:TextBox ID="txtONB_PAGES" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label text-red" runat="server" Text="頁數為0則不印"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table id="tbModify" class="table table-bordered text-center"  visible="false" runat="server">
                                <tr>
                                    <th colspan="2">
                                        <asp:Label ID="lblHeader" Font-Size="Large" ForeColor="Red" runat="server" Text="資　料　異　動"></asp:Label> 
                                    </th>
                                </tr>
                                    <tr>
                                     <td><asp:Button ID="btnModify" CssClass="btn-success btnw2" OnClick="btnModify_Click" runat="server" Text="存檔" /></td>
                                     <td>
                                         <asp:Label ID="lblOVC_IKIND_NAME_Modify" CssClass="control-label text-blue" runat="server" Text=""></asp:Label>
                                         <asp:Label CssClass="control-label" runat="server" Text="附件名稱："></asp:Label>
                                         <asp:DropDownList ID="drpOVC_ATTACH_NAME_Modify" CssClass="tb tb-m" OnSelectedIndexChanged="drpOVC_ATTACH_NAME_Modify_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                         <asp:TextBox ID="txtOVC_ATTACH_NAME_Modify" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                         <p></p>
                                         <asp:Label CssClass="control-label" runat="server" Text="份數："></asp:Label>
                                         <asp:TextBox ID="txtONB_QTY_Modify" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                         <asp:Label CssClass="control-label" runat="server" Text="頁數："></asp:Label>
                                         <asp:TextBox ID="txtONB_PAGES_Modify" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                         <asp:Label CssClass="control-label text-red" runat="server" Text="頁數為0則不印"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Label CssClass="control-label text-red" runat="server" Text="內購採購計畫清單第一聯固定為一份、第二聯為六份"></asp:Label><br><br>
                                <asp:Label CssClass="control-label text-red" runat="server" Text="外購採購輸出入計畫清單第一聯固定為一份、第二聯為九份"></asp:Label><br><br>
                                <asp:Label CssClass="control-label text-red" runat="server" Text="商情分析表及商情檢視表等電子檔，請勿上傳"></asp:Label>
                            </div>
                            </div>
                            <!--上傳作業-->
                            <div id="divFileUpload" visible="false" runat="server">
                                <div class="text-center">
                                    <asp:Button ID="btnBackDetail" CssClass="btn-warning" runat="server" OnClick="btnBackDetail_Click" Text="回上一頁"/><br><br>
                                    <asp:Label CssClass="control-label text-red" Font-Size="Large" runat="server" Text="注意：「採購計畫清單」、「商情分析表」及「商情蒐集檢視表」請勿上傳"></asp:Label>
                                </div>
                                <table class="table table-bordered">
                                    <tr>
                                        <td class="text-center">
                                            <asp:Button ID="btnUpload" CssClass="btn-success btnw2" OnClick="btnUpload_Click" runat="server" Text="上傳"/>
                                            
                                        </td>
                                         <td style="width:80%">
                                             <asp:FileUpload ID="FileUpload" title="選擇" runat="server" />
                                         </td>
                                    </tr>
                                </table>
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
