<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D18_6.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D18_6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->購案押標金/投標文件退還作業
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;" id="divForm" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table no-bordered-seesaw text-center">
                                <tr class="no-bordered-seesaw">
                                    <td class="no-bordered">
                                        <asp:Label CssClass="control-label" runat="server" >購案編號：</asp:Label>
                                        <asp:Label ID="lblOVC_PURCH_A_5" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server"> (</asp:Label>
                                        <asp:Label CssClass="control-label text-red" runat="server">第</asp:Label>
                                        <asp:Label ID="lblONB_GROUP" CssClass="control-label text-red" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label text-red" runat="server">組</asp:Label>
                                        <asp:Label CssClass="control-label" runat="server"> )</asp:Label>
                                    </td>
                                    <td class="no-bordered">
                                        <asp:Label CssClass="control-label" runat="server" >開標日期：</asp:Label>
                                        <asp:Label ID="lblOVC_DOPEN" CssClass="control-label" runat="server"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="lblOVC_OPEN_HOUR" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">時</asp:Label>
                                        <asp:Label ID="lblOVC_OPEN_MIN" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">分</asp:Label>
                                    </td>
                                    <td class="no-bordered">
                                        <asp:Label CssClass="control-label" runat="server" >開標地點：第 </asp:Label>
                                        <asp:Label ID="lblOVC_ROOM" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server" > 開標室</asp:Label>
                                    </td>
                                    <td class="no-bordered">
                                        <asp:Label CssClass="control-label" runat="server">主持人：</asp:Label>
                                        <asp:Label ID="lblOVC_CHAIRMAN" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table><br><br>

                            <div class="subtitle text-red"><asp:Label CssClass="control-label" runat="server">請先選擇欲退還 押標金/投標文件 之廠商：</asp:Label></div>
                            <asp:GridView ID="gvTBM1313" CssClass="table data-table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="請選擇">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" CssClass="radioButton" Checked='<%# Eval("IsCheck").ToString()=="true" ? true : false %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="統一編號">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_VEN_CST" Text='<%# Eval("OVC_VEN_CST") %>' CssClass="control-label" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="廠商名稱">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_VEN_TITLE" Text='<%# Bind("OVC_VEN_TITLE") %>' CssClass="control-label" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                            <div style="text-align:center">
                                <asp:Button ID="btnTBMBID_END" CssClass="btn-default" Text="截標審查(廠商編輯)作業" OnClick="btnTBMBID_END_Click" runat="server" />
                            </div>


                                <div class="subtitle text-red">
                                    <asp:Label CssClass="control-label" runat="server" >再輸入退還 押標金/投標文件 之內容：</asp:Label>
                                </div>
                                <table class="table table-bordered">
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" >一、退還日期與地點</asp:Label><br>
                                            <div class="input-append datepicker position-left">
                                                <asp:TextBox ID="txtOVC_DBACK" CssClass="tb tb-s position-left"  runat="server"></asp:TextBox>
                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                            </div>
                                            <asp:RadioButtonList ID="rdoOVC_BACK_PLACE" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem>本單位服務台</asp:ListItem>
                                                <asp:ListItem>本單位會客室</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">二、事由</asp:Label><br>
                                            <asp:CheckBox ID="chkOVC_REASON_1" CssClass="radioButton" Text="本次開標未達法定家數，依規定原封退回貴廠商押標金及投標文件。" runat="server"/>
                                            <br>
                                            <asp:CheckBox ID="chkOVC_REASON_2" CssClass="radioButton" Text="本次依政府採購法第四十八條第一項第一款規定不予" runat="server"/>
                                            <asp:CheckBoxList ID="chkOVC_REASON_2_1" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server" >
                                                <asp:ListItem>開標</asp:ListItem>
                                                <asp:ListItem>決標，依規定原封退還貴廠商</asp:ListItem>
                                            </asp:CheckBoxList>
                                            <asp:CheckBoxList ID="chkOVC_REASON_2_2" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server" >
                                                <asp:ListItem>押標金</asp:ListItem>
                                                <asp:ListItem>投標文件。</asp:ListItem>
                                            </asp:CheckBoxList>
                                            <br>
                                            <asp:CheckBox ID="chkOVC_REASON_3" CssClass="radioButton" Text="貴廠商" runat="server"/>
                                            <asp:CheckBoxList ID="chkOVC_REASON_3_1" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server" >
                                                <asp:ListItem>為不予決標對象</asp:ListItem>
                                                <asp:ListItem>為非得標廠商，依規定退回貴廠商</asp:ListItem>
                                                <asp:ListItem>押標金</asp:ListItem>
                                                <asp:ListItem>未開標之規格文件</asp:ListItem>
                                                <asp:ListItem>價格文件。</asp:ListItem>
                                            </asp:CheckBoxList>
                                            <br>
                                            <asp:CheckBox ID="chkOVC_REASON_4" CssClass="radioButton" Text="貴廠商經評選結果非為最優勝廠商，依規定退回貴廠商" runat="server"/>
                                            <asp:CheckBoxList ID="chkOVC_REASON_4_1" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                                <asp:ListItem>押標金</asp:ListItem>
                                                <asp:ListItem>企劃書乙式份</asp:ListItem>
                                                <asp:ListItem>建議書乙式份。</asp:ListItem>
                                            </asp:CheckBoxList><br>
                                            <asp:CheckBox ID="chkOVC_REASON_5" CssClass="radioButton" Text="貴廠商於得標後已依規定繳足額履約保證金，依規定退回貴廠商押標金。" runat="server"/><br>
                                            <asp:CheckBox ID="chkOVC_REASON_6" CssClass="radioButton" Text="本案開標結果通知單。" runat="server"/><br>
                                            <asp:Label CssClass="control-label" runat="server" >其他：</asp:Label><br>
                                            <asp:TextBox ID="txtOVC_MEMO" CssClass="tb tb-full" runat="server"></asp:TextBox><br>
                                            <asp:Label CssClass="control-label" runat="server" >備考：</asp:Label><br>
                                            <asp:TextBox ID="txtOVC_REMARK" CssClass="tb tb-full" runat="server"></asp:TextBox><br>
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <table class="table no-bordered-seesaw text-center">
                                        <tr class="no-bordered-seesaw">
                                            <td class="no-bordered">
                                                <asp:Button ID="btnSave" CssClass="btn-default btnw2" Text="存檔" OnClick="btnSave_Click" runat="server" />
                                                <asp:Button ID="btnReturn" CssClass="btn-default" OnClick="btnReturn_Click" Text="回開標紀錄作業編輯" runat="server" />
                                                <asp:Button ID="btnReturnR" CssClass="btn-default" OnClick="btnReturnR_Click" Text="回開標紀錄選擇畫面" runat="server" />
                                                <asp:Button ID="btnReturnM" CssClass="btn-default" OnClick="btnReturnM_Click" Text="回主流程畫面" runat="server" />
                                                <asp:LinkButton id="lbtnToWordD18_6" CssClass="btn-default" OnClick="lbtnToWordD18_6_Click" runat="server">退還紀錄表匯出.doc</asp:LinkButton>
                                                <asp:LinkButton id="lbtnToWordD18_6_odt" CssClass="btn-default" OnClick="lbtnToWordD18_6_odt_Click" runat="server">.odt</asp:LinkButton>
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

