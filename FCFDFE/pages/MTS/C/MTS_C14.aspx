<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_C14.aspx.cs" Inherits="FCFDFE.pages.MTS.C.MTS_C14" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
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
                    國防部國防採購室外購案軍品保留索賠權-建立
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:Panel ID="pnMessageNew" runat="server"></asp:Panel>
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center" style="width: 20%;">
                                        <asp:Label CssClass="control-label" runat="server">保留索賠權編號</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblOVC_RECLAIM_NO" CssClass="control-label" runat="server"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="lblOVC_CREATE_LOGIN_ID" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label text-star" runat="server">申請單位</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:HiddenField ID="txtOVC_DEPT_CDE" runat="server" />
                                        <asp:TextBox ID="txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Button CssClass="btn-success btnw4" OnClientClick="OpenWindow('txtOVC_DEPT_CDE', 'txtOVC_ONNAME')" Text="申請單位" runat="server" />&nbsp;&nbsp;
                                        <asp:Button CssClass="btn-default btnw4" OnClick="btnClearDept_Click" Text="資料清空" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td  class="text-center">
                                       <asp:Label CssClass="control-label text-star" runat="server">申請日期</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_APPLY_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label text-star" runat="server">提單編號</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m text-toUpper" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Button CssClass="btn-success btnw6" OnClick="btnLoad_Click" Text="載入提單資料" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">實際抵達日期</asp:Label>
                                    </td>
                                    <td>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_IMPORT_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                    <td class="text-center" style="width: 20%;">
                                        <asp:Label CssClass="control-label text-star" runat="server">投保通知書編號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_INN_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label text-star" runat="server">軍種保留索賠權來文文號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_CLAIM_MSG_NO" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                    <td  class="text-center">
                                        <asp:Label CssClass="control-label text-star" runat="server">軍種保留索賠權來文日期</asp:Label>
                                    </td>
                                    <td>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_CLAIM_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label text-star" runat="server">軍品名稱</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Textbox ID="txtOVC_CLAIM_ITEM" CssClass="textarea tb-full" TextMode="MultiLine" Rows="2" runat="server"></asp:Textbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">損失原因</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:RadioButtonList ID="rdoOVC_CLAIM_REASON" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="rdoOVC_CLAIM_REASON_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:RadioButtonList>
                                        <asp:Textbox ID="txtOVC_CLAIM_REASON_NOTE" CssClass="tb tb-l" runat="server"></asp:Textbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">應收件數</asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="labelA" CssClass="text-red" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_RECEIVE" CssClass="tb tb-s" OnTextChanged="txtONB_RECEIVE_TextChanged" AutoPostBack="true" OnKeyPress="txtKeyNumber()" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">件</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">實收件數</asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="labelB" CssClass="text-red" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_ACTUAL_RECEIVE" CssClass="tb tb-s" OnTextChanged="txtONB_RECEIVE_TextChanged" AutoPostBack="true" OnKeyPress="txtKeyNumber()" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">件</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">破損件數</asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="labelC" CssClass="text-red" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_CLAIM_BREAK" CssClass="tb tb-s" OnTextChanged="txtONB_RECEIVE_TextChanged" AutoPostBack="true" OnKeyPress="txtKeyNumber()" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">件</asp:Label>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">索賠件數</asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="labelD" CssClass="text-red" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_CLAIM_NUMBER" CssClass="tb tb-s" OnTextChanged="txtONB_RECEIVE_TextChanged" AutoPostBack="true" OnKeyPress="txtKeyNumber()" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">件</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">索賠金額</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtONB_CLAIM_AMOUNT" CssClass="tb tb-s" OnKeyPress="txtKeyNumber()" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">元</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="drpOVC_CLAIM_CURRENCY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <%--<td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">作業進度</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpProgressRate" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>--%>
                                    <td class="text-center">
                                        <%--<asp:Label CssClass="control-label text-star" runat="server">實際理賠金額</asp:Label>--%>
                                        <asp:Label CssClass="control-label" runat="server">作業進度</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <%--<asp:TextBox ID="txtONB_COMPENSATION_AMOUNT_NTD" CssClass="tb tb-s" OnKeyPress="txtKeyNumber()" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">元</asp:Label>--%>
                                        <%--<asp:TextBox ID="TextBox1" CssClass="tb tb-s" OnKeyPress="txtKeyNumber()" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">元</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="drpOVC_COMPENSATION_CURRENCY" CssClass="tb tb-s" runat="server"></asp:DropDownList>--%>
                                        <asp:DropDownList ID="drpOVC_APPROVE_STATUS" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label text-star" runat="server">結案日期</asp:Label>
                                    </td>
                                    <td>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_APPROVE_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">支票銀行</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_CHEQUE_BANK" CssClass="tb tb-m" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">支票號碼</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_CHEQUE_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">支票抬頭</asp:Label>
                                        
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_CHEQUE_TITLE" CssClass="tb tb-m" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">支票日期</asp:Label>
                                    </td>
                                    <td>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_CHEQUE_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">備註</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtOVC_NOTE" CssClass="textarea tb-full" TextMode="MultiLine" Rows="2" runat="server"></asp:TextBox>
                                    </td>
                                </tr>--%>
                            </table>
                            <div class="text-center">
                                <asp:Button CssClass="btn-warning btnw2" OnClick="btnSave_Click" Text="送出" runat="server" />
                                <asp:Button CssClass="btn-default btnw2" OnClick="btnCancel_Click" Text="取消" runat="server" /><br/><br />
                                <%--<asp:LinkButton ID="LinkNew" ForeColor="#3366FF" runat="server">繼續新增保留索賠權</asp:LinkButton><br />--%>
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                </footer>
            </section>
        </div>
    </div>
</asp:Content>

