<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_C14.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.MPMS_C14" %>
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
                    <!--標題-->計劃評核－審查資料輸入
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label><br />
                                        <asp:Label CssClass="control-label" runat="server">購案名稱</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label text-left" runat="server">AA09871L</asp:Label><br />
                                        <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label text-left" runat="server">購案測試01</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">申購單位(代碼)-申購人(電話)</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_PUR_USER" CssClass="control-label text-left" runat="server">國防部政務辦公室(00200)-黃XX(11,軍線:23456，手機:11)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">是否需辦理FAC資審</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:RadioButtonList ID="rdoFAC" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1">是</asp:ListItem>
                                            <asp:ListItem Value="2">否</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">是否需辦理公開閱覽</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:RadioButtonList ID="rdo_OVC_READ_BID" CssClass="radioButton" OnSelectedIndexChanged="rdo_OVC_READ_BID_SelectedIndexChanged"
                                                                RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true" runat="server">
                                            <asp:ListItem Value="1">是</asp:ListItem>
                                            <asp:ListItem Value="2">否</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <div id="divDateTime" visible="false" runat="server">
                                            <asp:Label runat="server">公告日期</asp:Label>
                                             <!--↓日期套件↓-->
                                                <div class="input-append datepicker" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                                    <asp:TextBox ID="txtOVC_DANNOUNCE" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                    <div class="add-on"><i class="icon-calendar"></i></div>
                                                </div>
                                            <!--↑日期套件↑-->
                                            <br />
                                                <asp:Label runat="server">閱覽時間</asp:Label>
                                                <asp:TextBox ID="txtOVC_OPEN_PERIOD" CssClass="tb tb-m" runat="server" ></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnSave" CssClass="btn-success" runat="server" OnClick="btnSave_Click" Text="FAC資審與公開閱覽存檔" /><!--黃色-->
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">資料版本</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_VERSION" CssClass="control-label text-blue" runat="server">上一版本:2017-03-03 14:09:55 下午</asp:Label> <=> <asp:Label CssClass="control-label text-red" ID="lblOVC_VERSION_NOW" runat="server">目前版本: 2017-04-03 15:10:45 下午</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-red" runat="server">注意事項：若上一版本未顯示，或上一級版本與目前版本資訊相同，表示委購單位尚未完成資料重新轉呈採購中心或委辦單位毋需澄覆</asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="GV_Query_PLAN" CssClass=" table data-table table-striped border-top " 
                                AutoGenerateColumns="false" OnPreRender="GV_Query_PLAN_PreRender"
                                  OnRowDataBound="GV_Query_PLAN_RowDataBound" OnRowCommand="GV_Query_PLAN_RowCommand" runat="server">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="10%" HeaderText="審查次數" >
                                        <ItemTemplate>
                                            <asp:Label CssClass="control-label" ID="lblONB_CHECK_TIMES" Text='<%#Eval("ONB_CHECK_TIMES")%>' runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label text-red" ID="lblAUDITSTATUS" Text='<%#Eval("OVC_CHECK_OK")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:BoundField HeaderText="分派日" DataField="OVC_DRECEIVE" />
                                    <asp:TemplateField HeaderText="聯審單位" >
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-success btnw4" ID="btn_Change" Text="異動" CommandName="btn_Change"
                                                CommandArgument='<%--#Eval("傳變數")--%>' runat="server"/>
                                            <asp:Label CssClass="control-label" ID="lblUnit" Text='<%#Eval("ISAUDIT")%>' runat="server"></asp:Label>
                                            <asp:LinkButton ID="btn_PrintComment" Text ="聯審意見" runat="server" Visible="false"></asp:LinkButton>
                                            <asp:LinkButton ID="btn_CommentWithResponse" Text ="聯審意見(含澄覆意見)" runat="server" Visible="false"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="綜審簽呈" >
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-success btnw4" ID="btn_Signature" Text="綜簽" CommandName="btn_Signature"
                                                CommandArgument='<%--#Eval("傳變數")--%>' Visible="false" runat="server"/>
                                            <asp:Label CssClass="control-label" ID="lblOVC_DRESULT" Text='<%#Eval("OVC_DRESULT")%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="text-center" HeaderText="申購單位逾初審7日、複審5日時限" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblSignDate" runat="server" Visible="false">
                                                <span class="glyphicon glyphicon-warning-sign" style="color:red"></span>
                                            </asp:Label>
                                            <asp:HiddenField ID="hidOVC_CHECK_OK" Value='<%#Bind("OVC_CHECK_OK")%>' runat="server" />
                                            <asp:HiddenField ID="hidOVC_PERMISSION_UPDATE" Value='<%#Bind("OVC_PERMISSION_UPDATE")%>' runat="server" />
                                            <asp:HiddenField ID="hidOVC_DREJECT" Value='<%#Bind("OVC_DREJECT")%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="功能" >
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-success btnw2" ID="btn_Review" Text="審查" CommandName="btn_Review"
                                                CommandArgument='<%#Eval("ONB_CHECK_TIMES")%>' runat="server"/>
                                            <asp:LinkButton ID="btn_PrintREASON" Text ="列印審查意見" runat="server" Visible="false"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                	            </Columns>
	   		               </asp:GridView>
                           
                            <div class="text-center" style="margin-bottom:15px;">
                                <asp:Button ID="btnGOOD_APPLICATION_PRINT" CssClass="btn-success" CommandName="PrintSupPDF" OnCommand="btn_PRINT_Command" runat="server" Text="物資申請書列印" /><!--黃色-->
                                <asp:Button ID="btnOLD_GOOD_APPLICATION_PRINT" CssClass="btn-success" CommandName="PrintNewPDF" OnCommand="btn_PRINT_Command" runat="server" Text="新物資申請書列印" /><!--黃色-->
                                <asp:Button ID="btnBUDGET_DETAIL" CssClass="btn-success" runat="server" CommandName="btnBUDGET_DETAIL" OnCommand="btn_PRINT_Command" Text="預算年度分配明細表" /><!--黃色-->
                                <asp:Button ID="btnOVCLIST_PRINT" CssClass="btn-success" runat="server"  CommandName="btnOVCLIST_PRINT" OnCommand="btn_PRINT_Command" Text="採購計劃清單列印.pdf" /><!--黃色-->
                                <asp:Button ID="btnOVCLIST_PRINT_WORD" CssClass="btn-success" runat="server"  CommandName="btnOVCLIST_PRINT_WORD" OnCommand="btn_PRINT_Command"  Text=".doc" /><!--黃色-->
                                <asp:Button ID="btnOVCLIST_PRINT_ODT" CssClass="btn-success" runat="server"  CommandName="btnOVCLIST_PRINT_ODT" OnCommand="btn_PRINT_Command"  Text=".odt" /><!--黃色-->
                                <asp:Button ID="btnOVC_RESPONSER" CssClass="btn-success" runat="server" Text="澄覆後資料比較表" /><!--黃色-->
                            </div>
                            
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                     
                                        <asp:Button ID="btnNew" CssClass="btn-success btnw6" OnClick="btnNew_Click" runat="server" Text="新增審查紀錄" /><!--黃色-->
                                         
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label position-left" runat="server">審查次數：</asp:Label>
                                        <asp:TextBox ID="txtONB_CHECK_TIMES" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>

                                        <asp:Label CssClass="control-label position-left" runat="server">分派日：</asp:Label><!--前方標籤文字，跟日期同一行需使用"position-left"之class-->
                                        <!--↓日期套件↓-->
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtOVC_DRECEIVE" CssClass="tb tb-s position-left" runat="server" ></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <!--↑日期套件↑-->
                                    </td>
                                </tr>
                            </table>
                            <div class="subtitle">委案單位已上傳檔案</div>
                            <asp:GridView ID="GV_C_Alreadyupdate" CssClass=" table data-table table-striped border-top " AutoGenerateColumns="false" OnPreRender="GV_C_Alreadyupdate_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="主件名稱" DataField="OVC_IKIND" />
                                    <asp:BoundField HeaderText="附件名稱" DataField="OVC_ATTACH_NAME" />
                                    <asp:BoundField HeaderText="份數" DataField="ONB_QTY" />
                                    <asp:BoundField HeaderText="相對之上傳檔案" DataField="OVC_FILE_NAME" />
                                    <asp:BoundField HeaderText="頁數" DataField="ONB_PAGES" />
                	            </Columns>
	   		               </asp:GridView>
                            <div class="subtitle">購案審查上傳檔案</div>
                            <asp:GridView ID="GV_OVC_Alreadyupdate" CssClass=" table data-table table-striped border-top " AutoGenerateColumns="false" OnPreRender="GV_OVC_Alreadyupdate_PreRender" OnRowCommand="GV_OVC_Alreadyupdate_RowCommand" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="選擇" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-success btnw4" ID="btn_Change" Text="下載" CommandName="btn_Download" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="選擇" DataField="" />--%>
                                    <asp:BoundField HeaderText="上傳之檔案名稱" DataField="FileName" />
                                    <asp:BoundField HeaderText="時間" DataField="Time" />
                                    <asp:BoundField HeaderText="檔案大小" DataField="Size" />
                	            </Columns>
	   		               </asp:GridView>
                             <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnUpload" CssClass="btn-success btnw6" OnClick="btnUpload_Click" runat="server" Text="上傳" /><!--黃色-->
                                    </td>
                                    <td class="text-left">
                                        <asp:FileUpload ID="ful" title="瀏覽" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <div class="subtitle">採購計劃核定歷史紀錄</div>
                            <asp:GridView ID="GV_History" CssClass=" table data-table table-striped border-top " AutoGenerateColumns="false" OnPreRender="GV_History_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="確認審" DataField="" />
                                    <asp:BoundField HeaderText="歷程檔案" DataField="" />
                                    <asp:BoundField HeaderText="歷程日期" DataField="" />
                                    <asp:BoundField HeaderText="核定審備註" DataField="" />
                	            </Columns>
	   		               </asp:GridView>
                    </div>
                </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
