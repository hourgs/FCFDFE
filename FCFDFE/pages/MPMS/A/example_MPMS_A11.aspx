<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="example_MPMS_A11.aspx.cs" Inherits="FCFDFE.pages.MPMS.A.example_MPMS_A11" MaintainScrollPositionOnPostback="true"%>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        //$(document).ready(function () {
            //$("<%=strMenuName%>").addClass("active");
            //$("<%=strMenuNameItem%>").addClass("active");
        //});
    </script>
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->採購預劃
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body">
                    <div class="form">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="subtitle"> 預劃購案編號賦予 </div>
                                
                            <!--頁籤－開始-->
                            <header class="panel-heading">
                                <ul class="nav nav-tabs">
                                    <li class="active">
                                        <a data-toggle="tab" href="#home">Home</a>
                                    </li>
                                    <li class="">
                                        <a data-toggle="tab" href="#about">About</a>
                                    </li>
                                    <li class="">
                                        <a data-toggle="tab" href="#profile">Profile</a>
                                    </li>
                                    <li class="">
                                        <a data-toggle="tab" href="#contact">Contact</a>
                                    </li>
                                </ul>
                            </header>
                            <div class="panel-body tab-body">
                                <div class="tab-content">
                                    <div id="home" class="tab-pane active">
                                        Home
                                    </div>
                                    <div id="about" class="tab-pane">About</div>
                                    <div id="profile" class="tab-pane">Profile</div>
                                    <div id="contact" class="tab-pane">Contact</div>
                                </div>
                            </div>
                            <!--頁籤－結束-->

                            <table class="table table-bordered text-center">
                                <tr>
                                    <td rowspan="10" class="td-vertical">
                                        <asp:Label CssClass="control-label text-vertical-l" style="height: 190px;" runat="server">標題名稱</asp:Label>
                                    </td>
                                </tr>
                                <tr class="screentone-gray">
                                    <td style="width: 17%;"><asp:Label CssClass="control-label text-star" runat="server">單位代號(第一組)</asp:Label></td>
                                    <td style="width: 17%;"><asp:Label CssClass="control-label text-star" runat="server">計畫年度(第二組)</asp:Label></td>
                                    <td style="width: 17%;"><asp:Label CssClass="control-label text-star" runat="server">計畫編號(第三組)</asp:Label></td>
                                    <td style="width: 49%;"><asp:Label CssClass="control-label text-star" runat="server">預劃購案編號</asp:Label></td>
                                </tr>
                                <tr class="text-right">
                                    <td class="td-noBorder">
                                        <asp:DropDownList ID="drpPurAreaMethod" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>單位代字</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="text-center"><asp:DropDownList ID="DropDownList1" CssClass="tb tb-s" runat="server">
                                        <asp:ListItem>計畫年度</asp:ListItem>
                                    </asp:DropDownList></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="DropDownList2" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>計畫編號</asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                    <td class="text-left">
                                        <asp:Label  CssClass="control-label" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtVEN_NAME_T" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" class="text-right">
                                        <asp:Label CssClass="control-label position-left" runat="server">日期範例(舊)：</asp:Label>
                                        <div class="input-append date" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="TextBox2" CssClass="tb tb-s position-left" readonly="true" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <br /><br />
                                        <asp:Label CssClass="control-label position-left" runat="server">日期範例(新)：</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="TextBox1" CssClass="tb tb-s position-left text-change" OnTextChanged="TextBox1_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <br /><br />
                                        <asp:Label CssClass="control-label position-left" runat="server">日期範例(左)無效：</asp:Label>
                                        <div class="input-append datepicker-left">
                                            <asp:TextBox ID="TextBox3" CssClass="tb tb-s position-left text-change" readonly="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                    <td rowspan="5" class="td-inner-table">
                                        <asp:GridView ID="GridView2" CssClass="table table-striped border-top table-inner" AutoGenerateColumns="false" OnPreRender="GridView2_PreRender" runat="server">
                                            <Columns>
                                                <asp:BoundField HeaderText="欄位名稱1" DataField="column1" ItemStyle-CssClass="text-center" />
                                                <asp:BoundField HeaderText="欄位名稱3" DataField="column3" ItemStyle-CssClass="text-right" />
                                                <asp:BoundField HeaderText="欄位名稱2" DataField="column2" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" class="text-right">
                                        <asp:Label CssClass="control-label position-left" runat="server">日期+時間範例：</asp:Label>   
                                        <div class='input-append datetimepicker'>
                                            <asp:TextBox CssClass='tb tb-m position-left' readonly="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label position-left text-red" runat="server">備註紅文字</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" class="text-right">
                                        <asp:Label CssClass="control-label position-left" runat="server">時間範例：</asp:Label>   
                                        <div class='input-append timepicker'>
                                            <asp:TextBox CssClass='tb tb-s position-left' readonly="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-time"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label position-left text-red" runat="server">備註紅文字</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" class="text-left">
                                        <asp:Label CssClass="control-label" runat="server">複選項水平範例：</asp:Label>
                                        <asp:CheckBoxList ID="CheckBoxList1" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1">複選項1</asp:ListItem>
                                            <asp:ListItem Value="2">複選項2</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" class="text-left">
                                        <asp:Label CssClass="control-label" runat="server">單選項水平範例：</asp:Label>
                                        <asp:RadioButtonList ID="RadioButtonList1" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1">單選項1</asp:ListItem>
                                            <asp:ListItem Value="2">單選項2</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr class="text-top">
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server">複選項垂直範例：</asp:Label>
                                        <asp:CheckBoxList ID="CheckBoxList2" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                            <asp:ListItem Value="1">複選項1</asp:ListItem>
                                            <asp:ListItem Value="2">複選項2</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server">單選項垂直範例：</asp:Label>
                                        <asp:RadioButtonList ID="RadioButtonList2" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                            <asp:ListItem Value="1">單選項1</asp:ListItem>
                                            <asp:ListItem Value="2">單選項2</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server">複選項垂直範例：</asp:Label><br />
                                        <asp:CheckBoxList ID="CheckBoxList3" CssClass="radioButton rb-s" RepeatLayout="UnorderedList" runat="server">
                                            <asp:ListItem Value="1">複選複選項複選項1</asp:ListItem>
                                            <asp:ListItem Value="2">複選複選項複選項2</asp:ListItem>
                                            <asp:ListItem Value="2">複選複選項複選項3</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr class="no-bordered">
                                    <td>
                                        <asp:Label  CssClass="control-label" runat="server">第一欄</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label  CssClass="control-label text-star" runat="server">第二欄</asp:Label>
                                    </td>
                                    <td class="no-bordered-seesaw">
                                        <asp:Label  CssClass="control-label" runat="server">第三欄</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server">垂直水平複合範例：</asp:Label>
                                        <asp:CheckBoxList ID="CheckBoxList4" CssClass="radioButton rb-complex" RepeatLayout="UnorderedList" runat="server">
                                            <asp:ListItem Value="1">複選項1</asp:ListItem>
                                            <asp:ListItem Value="2">複選項2</asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:CheckBox ID="CheckBox3" CssClass="radioButton rb-complex" Text="複選項3" runat="server" />
                                        <asp:CheckBox ID="CheckBox4" CssClass="radioButton rb-complex" Text="複選項4" runat="server" />
                                        <asp:RadioButtonList GroupName="gropu1" ID="RadioButtonList3" CssClass="radioButton rb-complex" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1">單選項1</asp:ListItem>
                                            <asp:ListItem Value="2">單選項2</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:RadioButton GroupName="gropu1" ID="RadioButton1" CssClass="radioButton rb-complex" Text="單選項1" runat="server" />
                                        <asp:RadioButton GroupName="gropu1" ID="RadioButton2" CssClass="radioButton rb-complex" Text="單選項2" runat="server" />
                                        <asp:RadioButton GroupName="gropu1" ID="RadioButton3" CssClass="radioButton rb-complex" Text="單選項3" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="no-bordered">
                                        <asp:Label  CssClass="control-label" runat="server">第一欄</asp:Label>
                                    </td>
                                    <td class="no-bordered">
                                        <asp:Label  CssClass="control-label" runat="server">第二欄</asp:Label>
                                    </td>
                                    <td class="no-bordered-seesaw">
                                        <asp:Label  CssClass="control-label" runat="server">第三欄</asp:Label>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="text-left">
                                        <asp:FileUpload ID="ful" title="選擇檔案" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5"><asp:Image ID="imgExanple" class="img img-full" runat="server" /></td>
                                </tr>
                            </table>

                            <div class="text-center">
                                <asp:Button ID="BtnEdt" cssclass="btn-success btnw4" runat="server" Text="內容編輯" OnClick="BtnEdt_Click" />
                                <asp:Button ID="BtnCancel" cssclass="btn-default btnw2" runat="server" Text="取消" />
                            </div>
                            
                            <asp:GridView ID="GridView1" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GridView1_PreRender" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="欄位名稱1" DataField="column1" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="欄位名稱3" DataField="column3" ItemStyle-CssClass="text-right" />
                                    <asp:BoundField HeaderText="欄位名稱2" DataField="column2" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                </footer>
            </section>
        </div>
    </div>
            <a class="btn-success" data-toggle="modal" href="#myModal">Dialog</a>

            <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                <div class="modal-dialog" style="width: 800px;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title">Modal Tittle</h4>
                        </div>
                        <div class="modal-body">

                            Body goes here...

                        </div>
                        <div class="modal-footer">
                            <button data-dismiss="modal" class="btn-default" type="button">Close</button>
                            <button class="btn-success" type="button">Save changes</button>
                        </div>
                    </div>
                </div>
            </div>
</asp:Content>
