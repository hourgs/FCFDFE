<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D14_2.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D14_2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->
                    採購重要事項編輯
                </header>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <div class="panel-body" style="border: solid 2px;" id="divForm" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                               <caption style="text-align:center">
                                   <asp:Label CssClass="control-label" Text="案號：" Font-Size="Large" runat="server"></asp:Label>
                                   <asp:Label ID="lblOVC_PURCH_A_5" CssClass="control-label" Text="" Font-Size="Large" runat="server"></asp:Label>
                               </caption>
                               <tbody>
                                    <tr>
                                        <td rowspan="10" class="td-vertical">
                                            <asp:Label CssClass="control-label text-vertical-m text-green" style="height: 190px;" Width="20px" Font-Bold="True" runat="server">基本資料</asp:Label></td>
                                        <td><asp:Label CssClass="control-label" Text="計畫申請單位" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td colspan="3" class="text-left">
                                            <asp:Label ID="lblOVC_PUR_NSECTION" CssClass="control-label" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" Text="購案名稱" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td colspan="3" class="text-left">
                                            <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" Text="計畫性質" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td colspan="3" class="text-left"><asp:Label ID="lblOVC_PLAN_PURCH" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label text-red" runat="server">（如有錯誤請分案人修正）</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" Text="招標方式" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td colspan="3" class="text-left"><asp:Label ID="lblOVC_PUR_ASS_VEN_CODE" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label text-red" runat="server">（如有錯誤請分案人修正）</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" Text="採購屬性" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td colspan="3" class="text-left"><asp:Label ID="lblOVC_LAB" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label text-red" runat="server">（如有錯誤請分案人修正）</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" Text="核定權責" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td colspan="3" class="text-left"><asp:Label ID="lblOVC_PUR_APPROVE_DEP" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label text-red" runat="server">（如有錯誤請分案人修正）</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" Text="履約驗結單位" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td colspan="3" class="text-left"><asp:Label ID="lblOVC_CONTRACT_UNIT" runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label text-red" runat="server">（如有錯誤請分案人修正）</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" Text="預算年度" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td colspan="3" class="text-left"><asp:Label CssClass="control-label" ID="lblOVC_ISOURCE" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" Text="預算金額" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblONB_PUR_BUDGET" runat="server"></asp:Label>&nbsp;&nbsp;
                                            <asp:Label CssClass="control-label" ID="lblOVC_PUR_CURRENT" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" Text="交貨天數" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblONB_DELIVER_DAYS" runat="server"></asp:Label> (日/曆天)</td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" Text="核定日期" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_PUR_DAPPROVE" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" Text="收辦日期" Font-Bold="True" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" ID="lblOVC_DRECEIVE" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr >
                                        <td colspan=5><asp:Label CssClass="control-label" Text="重要事項" Font-Bold="True" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan=5><asp:TextBox CssClass="textarea tb-full" ID="txtOVC_COMM" Rows="5" TextMode="MultiLine" runat="server" ></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td colspan=5><asp:Label CssClass="control-label" Text="檢討與因應措施" Font-Bold="True" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan=5><asp:TextBox CssClass="textarea tb-full" ID="txtOVC_COMM_REASON" Rows="5" TextMode="MultiLine" runat="server"></asp:TextBox></td>
                                    </tr>
                               </tbody>
                            </table>
                            <div style="text-align:center"> 
                                <asp:Button CssClass="btn-default btnw2" ID="btnSave" Text="存檔"  OnClick="btnSave_Click" runat="server"/>&nbsp;
                                <asp:LinkButton ID="lbtnPrint" OnClick="lbtnPrint_Click" runat="server">列印時程管制表</asp:LinkButton>
                                <asp:Button ID="btnBack" CssClass="btn-default" Text="回上一頁" OnClick="btnBack_Click" runat="server"/><br/><br/>
                            </div>


                            <div class="subtitle">採購發包階段上傳檔案 </div>
                            <asp:GridView ID="gvFiles" CssClass="table data-table table-striped border-top text-center" Width="100%" AutoGenerateColumns="False" runat="server" >
 		     		            <Columns>
                                    <asp:HyperLinkField DataTextField="FileName" HeaderText="上傳之檔案名稱" />
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

                            <p style="text-align:center">
                                <asp:Label Text="註:檔案上傳後，承辦人只能瀏覽資訊，唯採購發包分案者具刪除已上傳檔案之權限!" runat="server"></asp:Label>
                            </p>
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
