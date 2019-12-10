<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E23.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E23" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <style>
        tr td:nth-child(2n) {
            text-align: left;
        }
    </style>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    免稅紀錄
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <th colspan="8" style="background:red;"><asp:Label ForeColor="#ffff37" Font-Size="X-Large" CssClass="control-label" runat="server">請注意！未輸入資料或存檔無法列印各項報表</asp:Label></th>
                                </tr>
                                <tr>
                                    <th><asp:Label CssClass="control-label" runat="server">購案編號</asp:Label></th>
                                    <th><asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label></th>
                                    <th><asp:Label CssClass="control-label" runat="server">採購單位</asp:Label></th>
                                    <th><asp:Label ID="lblOVC_PUR_AGENCY" CssClass="control-label" runat="server"></asp:Label></th>
                                    <th><asp:Label CssClass="control-label" runat="server">交貨批次</asp:Label></th>
                                    <th><asp:Label ID="lblONB_SHIP_TIMES" CssClass="control-label" runat="server">1</asp:Label></th>
                                    <th><asp:Label CssClass="control-label" runat="server">申請次數</asp:Label></th>
                                    <th><asp:TextBox ID="txtONB_NO" CssClass="tb tb-s" runat="server">1</asp:TextBox></th>
                                </tr>
                                <tr>
                                    <td colspan="8" style="background:red;"><asp:Label ForeColor="#ffff37" Font-Size="X-Large" CssClass="control-label" runat="server">免徵關稅、進口營業稅、暨進口貨物稅</asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <asp:Button ID="btnCD" CssClass="btn-default" OnClick="btnCD_Click" runat="server" Text="請選擇合約免稅明細" />
                                        <asp:Label CssClass="control-label text-red" runat="server">(轉錄前請先存檔)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">出口廠商</asp:Label></td>
                                    <td colspan="3"><asp:TextBox ID="txtOVC_EXPORT_VENDOR" CssClass="tb tb-full" runat="server"></asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server">出口國別</asp:Label></td>
                                    <td colspan="3"><asp:TextBox ID="txtOVC_TAX_COUNTRY" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">貨物中英文名稱</asp:Label></td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtOVC_TAX_STUFF" CssClass="tb tb-full" runat="server">台中至烏坵人員委商往返包船</asp:TextBox>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">貨物數量</asp:Label></td>
                                    <td colspan="3"><asp:TextBox ID="txtOVC_TAX_QUALITY" CssClass="tb tb-s" runat="server">詳如附件貨物進口明細表</asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">貨物單位</asp:Label></td>
                                    <td colspan="3"><asp:TextBox ID="txtOVC_TAX_UNIT" CssClass="tb tb-full" runat="server">詳如附件貨物進口明細表</asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server">貨物總單位</asp:Label></td>
                                    <td colspan="3"><asp:TextBox ID="txtOVC_TAX_UNIT_SUM" CssClass="tb tb-full" runat="server">詳如附件貨物進口明細表</asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">貨物型號</asp:Label></td>
                                    <td colspan="3"><asp:TextBox ID="txtOVC_TAX_MODEL" CssClass="tb tb-full" runat="server">詳如附件貨物進口明細表</asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server">貨物規格</asp:Label></td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtOVC_TAX_DESC" CssClass="tb tb-full" runat="server">詳如附件貨物進口明細表</asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">貨物單價</asp:Label></td>
                                    <td colspan="3"><asp:TextBox ID="txtOVC_UNIT_PRICE" CssClass="tb tb-m" runat="server">詳如附件貨物進口明細表</asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server">貨物用途</asp:Label></td>
                                    <td colspan="3"><asp:TextBox ID="txtOVC_USE" CssClass="tb tb-m" runat="server">詳如附件貨物進口明細表</asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">貨物總價</asp:Label></td>
                                    <td colspan="7"><asp:TextBox ID="txtOVC_GOOD_TOTAL" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">進口日期</asp:Label></td>
                                    <td colspan="3">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" >
                                                    <asp:TextBox ID="txtOVC_TAX_DIMPORT" CssClass='tb tb-m position-left' runat="server"></asp:TextBox>
                                                    <span class='add-on'><i class="icon-calendar"></i></span>
                                                </div>
                                                <asp:Button  ID="btnClear" CssClass="btn-default btw4" OnClick="btnClear_Click" runat="server" Text="清除日期" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">進口地點</asp:Label></td>
                                    <td colspan="3"><asp:TextBox ID="txtOVC_TAX_PLACE" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">進口廠商</asp:Label></td>
                                    <td colspan="7">
                                        <asp:TextBox ID="txtOVC_TAX_VENDOR" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                             <div class="text-center">
                                 <asp:Button  ID="btnReturn" CssClass="btn-default btw4" OnClick="btnReturn_Click" runat="server" Text="回上一頁" />
                                 <asp:Button  ID="btnReturnM" CssClass="btn-default btw4" OnClick="btnReturnM_Click" runat="server" Text="回主流程" />
                                 <asp:Button  ID="btnDel" CssClass="btn-default btw2" OnClick="btnDel_Click" OnClientClick="if (confirm('確定要刪除資料?') == false) return false;" runat="server" Text="刪除" />
                                 <asp:Button  ID="btnSave" CssClass="btn-default btw2" OnClick="btnSave_Click" runat="server" Text="存檔" />
                                 <br /><br />
                                 <asp:LinkButton ID="LinkButton1" OnClick="LinkButton1_Click" runat="server">列印軍品採購免進口稅貨物稅表單.doc</asp:LinkButton>
                                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                 <asp:LinkButton ID="LinkButton3" OnClick="LinkButton3_Click" runat="server">列印軍品採購免進口稅貨物稅表單.pdf</asp:LinkButton>
                                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                 <asp:LinkButton ID="LinkButton4" OnClick="LinkButton4_Click" runat="server">列印軍品採購免進口稅貨物稅表單.odt</asp:LinkButton>

                                 <asp:LinkButton ID="LinkButton2" CssClass="text-red title" Visible="false" runat="server">列印貨物進口明細表</asp:LinkButton>
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
