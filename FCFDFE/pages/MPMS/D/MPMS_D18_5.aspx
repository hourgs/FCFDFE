<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D18_5.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D18_5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    購案開標紀錄分送作業
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;" id="divForm" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table no-bordered-seesaw text-center">
                                <tr class="no-bordered-seesaw">
                                    <td class="no-bordered">
                                        <asp:Label CssClass="control-label" runat="server">購案編號：</asp:Label>
                                        <asp:Label ID="lblOVC_PURCH_A_5" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server"> (</asp:Label>
                                        <asp:Label CssClass="control-label text-red" runat="server">第</asp:Label>
                                        <asp:Label ID="lblONB_GROUP" CssClass="control-label text-red" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label text-red" runat="server">組</asp:Label>
                                        <asp:Label CssClass="control-label" runat="server"> )</asp:Label>
                                    </td>
                                    <td class="no-bordered">
                                        <asp:Label CssClass="control-label" runat="server">開標日期：</asp:Label>
                                        <asp:Label ID="lblOVC_DOPEN" CssClass="control-label" runat="server"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="lblOVC_OPEN_HOUR" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">時</asp:Label>
                                        <asp:Label ID="lblOVC_OPEN_MIN" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">分</asp:Label>
                                    </td>
                                    <td class="no-bordered">
                                        <asp:Label CssClass="control-label" runat="server">開標地點：</asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">第</asp:Label>
                                        <asp:Label ID="lblOVC_ROOM" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">開標室</asp:Label>
                                    </td>
                                    <td class="no-bordered">
                                        <asp:Label CssClass="control-label" runat="server">主標人：</asp:Label>
                                        <asp:Label ID="lblOVC_CHAIRMAN" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                </table>

                            <%--<div class="title text-center text-red">
                                <asp:Label CssClass="control-label" runat="server">先選擇欲通知之參標廠商</asp:Label>
                            </div>
                            <div class=" text-center">
                                <asp:Label CssClass="control-label text-red" runat="server">(讀取投標廠商報價比較資料)</asp:Label>
                            </div>

                            <table class="table table-bordered text-left">
                                <tr>
                                    <th>
                                        <asp:Label CssClass="control-label" runat="server">選擇</asp:Label></th>
                                    <th>
                                        <asp:Label CssClass="control-label text-red" runat="server">投標廠商名稱</asp:Label></th>
                                    <th>
                                        <asp:Label CssClass="control-label" runat="server">廠商地址</asp:Label></th>
                                    <th>
                                        <asp:Label CssClass="control-label" runat="server">最後通知時間</asp:Label></th>
                                    <th>
                                        <asp:Label CssClass="control-label" runat="server">承辦人</asp:Label></th>
                                </tr>
                            </table>--%>
                            <asp:GridView ID="gvTBMBID_DOC_LOG" CssClass="table data-table table-striped border-top text-center" AutoGenerateColumns="false" OnPreRender="gvTBMBID_DOC_LOG_PreRender" OnRowCommand="gvTBMBID_DOC_LOG_RowCommand" DataKeyNames="OVC_PURCH" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="作業">
                                        <ItemTemplate>
                   		                    <asp:Button ID="btnChange" CssClass="btn-default btnw2" Text="異動" CommandName="Change" runat="server" />
                                        </ItemTemplate>
                                        <ItemTemplate>
                   		                    <asp:Button ID="btnDel" CssClass="btn-warning btnw2" Text="刪除" CommandName="Del" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="編號" DataField="ONB_NO" />
                                    <asp:TemplateField HeaderText="單位(名稱)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_RECEIVE_UNIT" Text='<%# Eval("OVC_RECEIVE_UNIT") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="級職" DataField="OVC_TITLE" />
                                    <asp:BoundField HeaderText="姓名" DataField="OVC_NAME" />
                                    <asp:BoundField HeaderText="簽收時間" DataField="OVC_TIME" />
                                    <asp:BoundField HeaderText="備考" DataField="OVC_REMARK" />
                                </Columns>
                            </asp:GridView>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">動作</asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server">自動編號</asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server">單位</asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server">級職</asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server">姓名</asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server">簽收時間</asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server">備考</asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width:30%"><asp:Button ID="btnSave" CssClass="btn-default btnw2" Text="儲存" OnClick="btnSave_Click" runat="server" /><!--黃色-->
                   		                <asp:Button ID="btnClear" CssClass="btn-default btnw2" OnClick="btnClear_Click" Text="清除" runat="server" />
                    	    	    </td>
                                    <td style="width:10%"><asp:Label CssClass="control-label" runat="server">自動編號</asp:Label></td>
                                    <td style="text-align:left;width:15%">
                                        <asp:DropDownList ID="drpOVC_RECEIVE_UNIT" CssClass="tb tb-m" OnSelectedIndexChanged="drpOVC_RECEIVE_UNIT_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList><br />
                                        <asp:TextBox ID="txtOVC_RECEIVE_UNIT" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width:10%"><asp:TextBox ID="txtOVC_TITLE" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                    <td style="width:10%"><asp:TextBox ID="txtOVC_NAME" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                    <td style="width:15%">
                                        <div class="input-append datepicker position-left" style="border-left-style:none;border-left:none">
                                            <asp:TextBox ID="txtOVC_TIME" CssClass="tb tb-s position-left text-change" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                    <td style="width:10%"><asp:TextBox ID="txtOVC_REMARK" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                </tr>
                            </table>
                            <div class="text-left">
                                <asp:Label CssClass="control-label" runat="server">註：</asp:Label><br />
                                <asp:Label CssClass="control-label" runat="server">一、以空心章蓋於紀錄文件正中央，以利識別。</asp:Label><br />
                                <asp:Label CssClass="control-label" runat="server">二、如得標廠商有一家以上情形者，本表可自行延伸級增列編號。</asp:Label>
                            </div>
                            <br />
                            <div class="text-center">
                                <asp:Button ID="btnReturn" CssClass="btn-default" OnClick="btnReturn_Click" Text="回開標紀錄作業編輯" runat="server" />
                                <asp:Button ID="btnReturnR" CssClass="btn-default" OnClick="btnReturnR_Click" Text="回開標紀錄選擇畫面" runat="server" />
                                <asp:Button ID="btnReturnM" CssClass="btn-default" OnClick="btnReturnM_Click" Text="回主流程畫面" runat="server" />
                                <asp:LinkButton id="lbtnToWordD18_5" OnClick="lbtnToWordD18_5_Click" runat="server">紀錄發送清冊匯出</asp:LinkButton>
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



