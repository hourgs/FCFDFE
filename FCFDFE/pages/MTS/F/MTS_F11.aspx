<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_F11.aspx.cs" Inherits="FCFDFE.pages.MTS.F.MTS_F11" %>
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
                    <asp:Label CssClass="control-label" runat="server"  ForeColor="Red">外運資料表作業載入步驟分為二 :</asp:Label>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2pxd;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                   <td colspan="2">
                                       <asp:Label CssClass="control-label" runat="server"  ForeColor="Red">1.外運資料表主檔載入作業說明</asp:Label><br />
                                       <asp:Label CssClass="control-label" runat="server">( 一 ) 注意：務必優先讀取 <a href="外運資料表主檔載入作業說明.docx">【外運資料表主檔】載入作業說明 </a>，以便順利作業。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 二 ) 下載  <a href="外運資料表主檔範例.xlsx">外運資料表主檔Excel 檔範例</a>。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 三 ) 按照載入作業說明規定格式編輯Excel檔。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 四 ) 按【瀏覽】選擇要載入的Excel檔。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 五 ) 按 <a href="javascript:var win=window.open('MTS_F11_2.aspx',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=1000,height=700,left=0,top=0');" >
                                                           【查詢機場或港口代碼】</a> 後檢查載入的資料是否正確。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 六 ) 按<a href="javascript:var win=window.open('<%=ResolveClientUrl("~/Content/unitQuery.aspx") %>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=600,height=400,left=0,top=0');" >
                                            【查詢單位代碼】 </a>後檢查載入資料是否正確。</asp:Label><br /> 
                                        <asp:Label CssClass="control-label" runat="server">( 七 ) 按【讀取Excel檔內容】後檢查顯示的資料內容是否正確。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 八 ) 如果資料正確無誤請按【確認轉入外運資料表主檔資料】完成仔入作業。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 九 ) 完成載入外運資料表主檔資料請繼續【2.外運資料表料件明細檔載入】作業。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 十 ) 或直接點取選<a href="javascript:var win=window.open('../A/MTS_A22_1.aspx','_blank');" >
                                            【外運資料管理功能】</a>繼續外運資料管理作業。</asp:Label><br />
                                   </td>
                                </tr>
                                <tr>
                                    <td style="width:400px;" class="text-center">
                                         <asp:Button ID="btnUpload" cssclass="btn-success" runat="server" Text="讀取外運資料表主檔內容" OnClick="btnUpload_Click"  />
                                        <asp:Label CssClass="control-label" runat="server">檔案(*.xlsx)</asp:Label>
                                    </td>
                                    <td style="width:600px;">
                                        <asp:FileUpload ID="browse" title="瀏覽" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </section>  
            <section class="panel">
                  <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                   <td colspan="2">
                                       <asp:Label CssClass="control-label" runat="server"  ForeColor="Red">2.外運資料表料件明細檔載入作業說明</asp:Label><br />
                                       <asp:Label CssClass="control-label" runat="server">( 一 ) 注意：務必優先讀取 <a href="外運資料表料件明細檔載入作業說明.docx">【外運資料表料件明細檔】載入作業說明</a>，以便順利作業。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 二 ) 下載  <a href="外運資料表料件明細檔範例.xlsx">外運資料表料件明細檔Excel檔範例</a>。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 三 ) 按照載入作業說明規定格式編輯Excel檔。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 四 ) 按【瀏覽】選擇要載入的Excel檔。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 五 ) 按【讀取Excel檔內容】後檢查顯示的資料內容是否正確。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 六 ) 如果資料正確無誤請按【確認轉入外運資料表料件明細檔資料】完成載入作業。</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">( 七 ) 完成載入外運資料表料件明細檔資料請點選<a href="javascript:var win=window.open('../A/MTS_A22_1.aspx','_blank');" >
                                            【外運資料管理功能】</a>繼續外運資料管理作業。</asp:Label><br />
                                   </td>
                                </tr>
                                <tr>
                                    <td style="width:400px;" class="text-center">
                                         <asp:Button ID="btnUpload2" cssclass="btn-success" runat="server" Text="讀取外運資料表料件明細檔內容" OnClick="btnUpload2_Click"  />
                                        <asp:Label CssClass="control-label" runat="server">檔案(*.xlsx)</asp:Label>
                                    </td>
                                    <td style="width:600px;">
                                        <asp:FileUpload ID="browse2" title="瀏覽" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
             </section>
        </div>
    </div>
</asp:Content>
