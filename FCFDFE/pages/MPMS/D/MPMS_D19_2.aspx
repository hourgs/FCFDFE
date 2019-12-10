<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D19_2.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D19_2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    開標記錄作業編輯(超過3次之投標廠商報價)
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;" id="divForm" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <table class="table table-bordered text-center">
                            <tr>
                                <td class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server" >第四次比減價</asp:Label>  
                                </td>
                                <td class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server" >第五次比減價</asp:Label>  
                                </td>
                                <td class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server" >第六次比減價</asp:Label>  
                                </td>
                            </tr>
                            <tr>
                                <td class="text-left">
                                    <asp:Label CssClass="control-label text-red" runat="server">標價類別(擇一)</asp:Label><br>
                                    <asp:RadioButtonList ID="rdoOVC_MINIS_4" CssClass="radioButton text-red" RepeatLayout="UnorderedList" runat="server" >
                                        <asp:ListItem Text="一般(單價)" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="一般(總價)" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="折扣率" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="不再減價" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="按底價承製" Value="4"></asp:ListItem>
                                    </asp:RadioButtonList><br><br>
                                    <asp:Label CssClass="control-label text-red" runat="server" >金額或折扣率</asp:Label><br>
                                    <asp:TextBox ID="txtONB_MINIS_4" CssClass="tb tb-s" runat="server"></asp:TextBox><br><br>
                                    <asp:Label CssClass="control-label" runat="server" >標價區分(擇一)</asp:Label><br>
                                    <asp:RadioButtonList ID="rdoOVC_KMINIS_4" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                        <asp:ListItem Text="決" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="廢" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="保留" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="並列" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="無效標" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="不合格標" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="以上皆非" Value="6"></asp:ListItem>
                                    </asp:RadioButtonList><br><br>
                                </td>
                                <td class="text-left">
                                    <asp:Label CssClass="control-label text-red" runat="server">標價類別(擇一)</asp:Label><br>
                                    <asp:RadioButtonList ID="rdoOVC_MINIS_5" CssClass="radioButton text-red" RepeatLayout="UnorderedList" runat="server" >
                                        <asp:ListItem Text="一般(單價)" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="一般(總價)" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="折扣率" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="不再減價" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="按底價承製" Value="4"></asp:ListItem>
                                    </asp:RadioButtonList><br><br>
                                    <asp:Label CssClass="control-label text-red" runat="server" >金額或折扣率</asp:Label><br>
                                    <asp:TextBox ID="txtONB_MINIS_5" CssClass="tb tb-s" runat="server"></asp:TextBox><br><br>
                                    <asp:Label CssClass="control-label" runat="server" >標價區分(擇一)</asp:Label><br>
                                    <asp:RadioButtonList ID="rdoOVC_KMINIS_5" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                        <asp:ListItem Text="決" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="廢" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="保留" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="並列" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="無效標" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="不合格標" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="以上皆非" Value="6"></asp:ListItem>
                                    </asp:RadioButtonList><br><br>
                                </td>
                                <td class="text-left">
                                    <asp:Label CssClass="control-label text-red" runat="server">標價類別(擇一)</asp:Label><br>
                                    <asp:RadioButtonList ID="rdoOVC_MINIS_6" CssClass="radioButton text-red" RepeatLayout="UnorderedList" runat="server" >
                                        <asp:ListItem Text="一般(單價)" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="一般(總價)" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="折扣率" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="不再減價" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="按底價承製" Value="4"></asp:ListItem>
                                    </asp:RadioButtonList><br><br>
                                    <asp:Label CssClass="control-label text-red" runat="server" >金額或折扣率</asp:Label><br>
                                    <asp:TextBox ID="txtONB_MINIS_6" CssClass="tb tb-s" runat="server"></asp:TextBox><br><br>
                                    <asp:Label CssClass="control-label" runat="server" >標價區分(擇一)</asp:Label><br>
                                    <asp:RadioButtonList ID="rdoOVC_KMINIS_6" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                        <asp:ListItem Text="決" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="廢" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="保留" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="並列" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="無效標" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="不合格標" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="以上皆非" Value="6"></asp:ListItem>
                                    </asp:RadioButtonList><br><br>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server" >第七次比減價</asp:Label>  
                                </td>
                                <td class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server" >第八次比減價</asp:Label>  
                                </td>
                                <td class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server" >第九次比減價</asp:Label>  
                                </td>
                            </tr>
                            <tr>
                                <td class="text-left">
                                    <asp:Label CssClass="control-label text-red" runat="server">標價類別(擇一)</asp:Label><br>
                                    <asp:RadioButtonList ID="rdoOVC_MINIS_7" CssClass="radioButton text-red" RepeatLayout="UnorderedList" runat="server" >
                                        <asp:ListItem Text="一般(單價)" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="一般(總價)" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="折扣率" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="不再減價" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="按底價承製" Value="4"></asp:ListItem>
                                    </asp:RadioButtonList><br><br>
                                    <asp:Label CssClass="control-label text-red" runat="server" >金額或折扣率</asp:Label><br>
                                    <asp:TextBox ID="txtONB_MINIS_7" CssClass="tb tb-s" runat="server"></asp:TextBox><br><br>
                                    <asp:Label CssClass="control-label" runat="server" >標價區分(擇一)</asp:Label><br>
                                    <asp:RadioButtonList ID="rdoOVC_KMINIS_7" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                        <asp:ListItem Text="決" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="廢" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="保留" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="並列" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="無效標" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="不合格標" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="以上皆非" Value="6"></asp:ListItem>
                                    </asp:RadioButtonList><br><br>
                                </td>
                                <td class="text-left">
                                    <asp:Label CssClass="control-label text-red" runat="server">標價類別(擇一)</asp:Label><br>
                                    <asp:RadioButtonList ID="rdoOVC_MINIS_8" CssClass="radioButton text-red" RepeatLayout="UnorderedList" runat="server" >
                                        <asp:ListItem Text="一般(單價)" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="一般(總價)" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="折扣率" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="不再減價" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="按底價承製" Value="4"></asp:ListItem>
                                    </asp:RadioButtonList><br><br>
                                    <asp:Label CssClass="control-label text-red" runat="server">金額或折扣率</asp:Label><br>
                                    <asp:TextBox ID="txtONB_MINIS_8" CssClass="tb tb-s" runat="server"></asp:TextBox><br><br>
                                    <asp:Label CssClass="control-label" runat="server" >標價區分(擇一)</asp:Label><br>
                                    <asp:RadioButtonList ID="rdoOVC_KMINIS_8" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                        <asp:ListItem Text="決" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="廢" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="保留" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="並列" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="無效標" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="不合格標" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="以上皆非" Value="6"></asp:ListItem>
                                    </asp:RadioButtonList><br><br>
                                </td>
                                <td class="text-left">
                                    <asp:Label CssClass="control-label text-red" runat="server">標價類別(擇一)</asp:Label><br>
                                    <asp:RadioButtonList ID="rdoOVC_MINIS_9" CssClass="radioButton text-red" RepeatLayout="UnorderedList" runat="server" >
                                        <asp:ListItem Text="一般(單價)" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="一般(總價)" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="折扣率" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="不再減價" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="按底價承製" Value="4"></asp:ListItem>
                                    </asp:RadioButtonList><br><br>
                                    <asp:Label CssClass="control-label text-red" runat="server" >金額或折扣率</asp:Label><br>
                                    <asp:TextBox ID="txtONB_MINIS_9" CssClass="tb tb-s" runat="server"></asp:TextBox><br><br>
                                    <asp:Label CssClass="control-label" runat="server" >標價區分(擇一)</asp:Label><br>
                                    <asp:RadioButtonList ID="rdoOVC_KMINIS_9" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                        <asp:ListItem Text="決" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="廢" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="保留" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="並列" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="無效標" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="不合格標" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="以上皆非" Value="6"></asp:ListItem>
                                    </asp:RadioButtonList><br><br>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server" >第十次比減價</asp:Label>  
                                </td>
                                <td class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server" >第11次比減價</asp:Label>  
                                </td>
                                <td class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server" >第12次比減價</asp:Label>  
                                </td>
                            </tr>
                            <tr>
                                <td class="text-left">
                                    <asp:Label CssClass="control-label text-red" runat="server">標價類別(擇一)</asp:Label><br>
                                    <asp:RadioButtonList ID="rdoOVC_MINIS_10" CssClass="radioButton text-red" RepeatLayout="UnorderedList" runat="server" >
                                        <asp:ListItem Text="一般(單價)" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="一般(總價)" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="折扣率" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="不再減價" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="按底價承製" Value="4"></asp:ListItem>
                                    </asp:RadioButtonList><br><br>
                                    <asp:Label CssClass="control-label text-red" runat="server" >金額或折扣率</asp:Label><br>
                                    <asp:TextBox ID="txtONB_MINIS_10" CssClass="tb tb-s" runat="server"></asp:TextBox><br><br>
                                    <asp:Label CssClass="control-label" runat="server" >標價區分(擇一)</asp:Label><br>
                                    <asp:RadioButtonList ID="rdoOVC_KMINIS_10" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                        <asp:ListItem Text="決" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="廢" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="保留" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="並列" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="無效標" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="不合格標" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="以上皆非" Value="6"></asp:ListItem>
                                    </asp:RadioButtonList><br><br>
                                </td>
                                <td class="text-left">
                                    <asp:Label CssClass="control-label text-red" runat="server">標價類別(擇一)</asp:Label><br>
                                    <asp:RadioButtonList ID="rdoOVC_MINIS_11" CssClass="radioButton text-red" RepeatLayout="UnorderedList" runat="server" >
                                        <asp:ListItem Text="一般(單價)" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="一般(總價)" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="折扣率" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="不再減價" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="按底價承製" Value="4"></asp:ListItem>
                                    </asp:RadioButtonList><br><br>
                                    <asp:Label CssClass="control-label text-red" runat="server" >金額或折扣率</asp:Label><br>
                                    <asp:TextBox ID="txtONB_MINIS_11" CssClass="tb tb-s" runat="server"></asp:TextBox><br><br>
                                    <asp:Label CssClass="control-label" runat="server" >標價區分(擇一)</asp:Label><br>
                                    <asp:RadioButtonList ID="rdoOVC_KMINIS_11" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                        <asp:ListItem Text="決" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="廢" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="保留" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="並列" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="無效標" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="不合格標" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="以上皆非" Value="6"></asp:ListItem>
                                    </asp:RadioButtonList><br><br>
                                </td>
                                <td class="text-left">
                                    <asp:Label CssClass="control-label text-red" runat="server">標價類別(擇一)</asp:Label><br>
                                    <asp:RadioButtonList ID="rdoOVC_MINIS_12" CssClass="radioButton text-red" RepeatLayout="UnorderedList" runat="server" >
                                        <asp:ListItem Text="一般(單價)" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="一般(總價)" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="折扣率" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="不再減價" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="按底價承製" Value="4"></asp:ListItem>
                                    </asp:RadioButtonList><br><br>
                                    <asp:Label CssClass="control-label text-red" runat="server" >金額或折扣率</asp:Label><br>
                                    <asp:TextBox ID="txtONB_MINIS_12" CssClass="tb tb-s" runat="server"></asp:TextBox><br><br>
                                    <asp:Label CssClass="control-label" runat="server" >標價區分(擇一)</asp:Label><br>
                                    <asp:RadioButtonList ID="rdoOVC_KMINIS_12" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                        <asp:ListItem Text="決" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="廢" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="保留" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="並列" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="無效標" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="不合格標" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="以上皆非" Value="6"></asp:ListItem>
                                    </asp:RadioButtonList><br><br>
                                </td>
                            </tr>
                        </table>
                    </div><br />
                    <div>
                        <asp:Button ID="btnReturn" CssClass="btn-default" Text="回上一頁" OnClick="btnReturn_Click" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnSave" CssClass="btn-default" Text="存檔" OnClick="btnSave_Click" runat="server" />
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                </footer>
            </section>
        </div>
    </div>
</asp:Content>

