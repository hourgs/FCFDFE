<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_G11.aspx.cs" Inherits="FCFDFE.pages.CIMS.G.CIMS_G11" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <div class="row">
        <div style="width: 800px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->
                    <div>案數統計查詢</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="subtitle">請選定時間條件，按下[匯出檔案]即可匯出</div>
                            <table class="table table-bordered" style="text-align:center">
                                <tr>
                                    <td style="width:90%;">  
                                        <asp:Label CssClass="control-label position-left" runat="server">核定日期自&ensp;</asp:Label>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtQuery1_s" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                <span class="add-on"><i class="icon-calendar"></i></span>
                                                <asp:Label CssClass="control-label position-left" runat="server">&emsp;至&emsp;</asp:Label>
                                            </div>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtQuery1_e" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                <span class="add-on"><i class="icon-calendar"></i></span>
                                                <asp:Label CssClass="control-label position-left" runat="server">止 排標統計表&ensp;</asp:Label>
                                                
                                            </div>
                                    </td>
                                    <td><asp:Button ID="btnQuery1" cssclass="btn-success btnw4" runat="server" Text="匯出檔案" OnClick="btnQuery1_Click"/></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label position-left" runat="server">公告日期自&ensp;</asp:Label>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtQuery2_s" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                <span class="add-on"><i class="icon-calendar"></i></span>
                                                <asp:Label CssClass="control-label position-left" runat="server">&emsp;至&emsp;</asp:Label>
                                            </div>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtQuery2_e" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                <span class="add-on"><i class="icon-calendar"></i></span>
                                                <asp:Label CssClass="control-label position-left" runat="server">止 排標統計表&ensp;</asp:Label>
                                            </div>
                                    </td>
                                    <td><asp:Button ID="btnQuery2" cssclass="btn-success btnw4" runat="server" Text="匯出檔案" OnClick="btnQuery2_Click"/></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label position-left" runat="server">開標日期自&ensp;</asp:Label>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtQuery3_s" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                <span class="add-on"><i class="icon-calendar"></i></span>
                                                <asp:Label CssClass="control-label position-left" runat="server">&emsp;至&emsp;</asp:Label>
                                            </div>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtQuery3_e" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                <span class="add-on"><i class="icon-calendar"></i></span>
                                                <asp:Label CssClass="control-label position-left" runat="server">止 排標統計表&ensp;</asp:Label>
                                            </div>
                                    </td>
                                    <td><asp:Button ID="btnQuery3" cssclass="btn-success btnw4" runat="server" Text="匯出檔案" OnClick="btnQuery3_Click" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label position-left" runat="server">開標日期自&ensp;</asp:Label>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtQuery4_s" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                <span class="add-on"><i class="icon-calendar"></i></span>
                                                <asp:Label CssClass="control-label position-left" runat="server">&emsp;至&emsp;</asp:Label>
                                            </div>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtQuery4_e" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                <span class="add-on"><i class="icon-calendar"></i></span>
                                                <asp:Label CssClass="control-label position-left" runat="server">止 開標預劃期程表&ensp;</asp:Label>
                                                
                                            </div>
                                    </td>
                                    <td><asp:Button ID="btnQuery4" cssclass="btn-success btnw4" runat="server" Text="匯出檔案" OnClick="btnQuery4_Click" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label position-left" runat="server">開標日期自&ensp;</asp:Label>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtQuery5_s" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                <span class="add-on"><i class="icon-calendar"></i></span>
                                                <asp:Label CssClass="control-label position-left" runat="server">&emsp;至&emsp;</asp:Label>
                                            </div>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtQuery5_e" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                <span class="add-on"><i class="icon-calendar"></i></span>
                                                <asp:Label CssClass="control-label position-left" runat="server">止 決標購案統計表&ensp;</asp:Label>
                                                
                                            </div>
                                    </td>
                                    <td><asp:Button ID="btnQuery5" cssclass="btn-success btnw4" runat="server" Text="匯出檔案" OnClick="btnQuery5_Click" /></td>
                                </tr>
                            </table>
                        </div>
                        <div class="cmxform form-horizontal tasi-form">
                            <div class="subtitle">查詢條件</div>
                            <div>
                                【報表名稱】排標統計表<br/>
                                1.資料來源：編製『購案主檔』、購訂『收辦檔』『簽辦檔』。<br/>
                                2.限制條件：列計已經採購中心『購訂承辦人』收辦之案件。 <br/><br/>
                                【報表名稱】*開標預劃期程表<br/>
                                1.資料來源：編製『購案主檔』、購訂『收辦檔』『簽辦檔』、鑑價系統『分組檔』。
                                2.限制條件：列計已經採購中心『購訂承辦人』收辦之『L、W』案。<br/><br/>
                                【報表名稱】*決標購案統計表<br/>
                                1.資料來源：預劃『購案預劃主檔』、編製『購案主檔』、購訂『簽辦檔』『開標紀錄檔』『開標結果檔』『合約檔』。<br/>
                                2.限制條件：列計購訂單位為『00N00』、核定權責『A、B、C、X』、『L、W』案；且『開標紀錄檔』(分組)已列『決標』之購案；有關金額皆來自『開標結果檔』。 

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