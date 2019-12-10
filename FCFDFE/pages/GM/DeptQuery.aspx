<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeptQuery.aspx.cs" Inherits="FCFDFE.pages.GM.DeptQuery" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        th{
            background-color: turquoise;
            color: white;
            font-size: 18px;
            font-weight: bold;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    全域系統單位維護
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered text-center">
                                <tr>
                                    <th colspan="6">
                                        <asp:Label CssClass="th_label" runat="server">全域組織單位維護</asp:Label>
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">單位代碼：</asp:Label>
                                        <input type="button" value="代碼查詢" class="btn-success" onclick="OpenWindow('txtOVC_DEPT_CDE', 'txtOVC_ONNAME')" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_DEPT_CDE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">單位名稱：</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">單位狀況：</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_ENABLE" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="0">0：停用</asp:ListItem>
                                            <asp:ListItem Value="1">1：現用</asp:ListItem>
                                            <asp:ListItem Value="2">2：戰時</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">採購單位類別：</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_CLASS" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">所屬之採購單位代碼：</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_PURCHASE_DEPT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">接轉單位類別：</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_CLASS2" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Button CssClass="btn-success" OnClick="btnQuery_Click" Text="查詢" runat="server" />
                                        <asp:Button CssClass="btn-success" OnClick="btnNew_Click" Text="新增組織單位資料" runat="server" />
                                        <asp:Button CssClass="btn-success" OnClick="btnUpload_Click" Text="上傳組織單位資料" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <!-- table-striped border-top-->
                            <asp:GridView ID="GV_TBMDEPT" DataKeyNames="OVC_DEPT_CDE" CssClass="table data-table table-bordered text-center" AutoGenerateColumns="false" OnPreRender="GV_TBMDEPT_PreRender" OnRowCommand="GV_TBMDEPT_RowCommand" OnRowDataBound="GV_TBMDEPT_RowDataBound" runat="server">
                                <Columns>
                                    <%--<th runat="server"><asp:Label CssClass="th_label" runat="server">維護</asp:Label></th>
                                    <th runat="server"><asp:Label CssClass="th_label" runat="server"></asp:Label></th>
                                    <th runat="server"><asp:Label CssClass="th_label" runat="server"></asp:Label></th>
                                    <th runat="server"><asp:Label CssClass="th_label" runat="server"></asp:Label></th>
                                    <th runat="server"><asp:Label CssClass="th_label" runat="server"></asp:Label></th>
                                    <th runat="server"><asp:Label CssClass="th_label" runat="server"></asp:Label></th>
                                    <th runat="server"><asp:Label CssClass="th_label" runat="server"></asp:Label></th>--%>
                                    <asp:TemplateField HeaderText="維護">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-success wbtn2" style="margin-bottom: 3px;" CommandName="dataModify" Text="異動" runat="server" />
                                            <asp:Button CssClass="btn-danger wbtn2" OnClientClick="return confirm('確認刪除？');" CommandName="dataDel" Text="刪除" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="單位代碼" DataField="" />--%>
                                    <asp:TemplateField HeaderText="單位代碼">
                                        <ItemTemplate>
                                            <a href="javascript:var win=window.open('DeptSubUnit?topDept=<%# Eval("OVC_DEPT_CDE")%>',null,'toolbar=0,location=0,status=0,menubar=0,width=850,height=500,left=200,top=80');">
                                                <%# Eval("OVC_DEPT_CDE")%>
                                            </a>
                                            <%--<asp:LinkButton ID="btnToSubUnit" CommandName="ToSubUnit" CommandArgument='<%#Eval("OVC_DEPT_CDE")%>' Text='<%#Eval("OVC_DEPT_CDE")%>' runat="server"></asp:LinkButton>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="單位名稱" DataField="OVC_ONNAME" />
                                    <%--<asp:BoundField HeaderText="採購單位" DataField="" />--%>
                                    <asp:TemplateField HeaderText="採購單位">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_PURCHASE_OK" CssClass="control-label" Text='<%#Eval("OVC_PURCHASE_OK_Value")%>' Visible="false" runat="server"></asp:Label>
                                            <asp:Label CssClass="control-label" Text='<%#Eval("OVC_PURCHASE_OK")%>' runat="server"></asp:Label><br />
                                            <asp:LinkButton ID="btnOVC_PURCHASE_OK" ForeColor="Red" CommandName="ToSubUnit" CommandArgument='<%#Eval("OVC_DEPT_CDE")%>' Visible="false" runat="server">下轄單位</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="採購上級單位代碼（名稱）<br>所屬之採購單位代碼（名稱）" DataField="" />--%>
                                    <asp:TemplateField HeaderText="採購上級單位代碼（名稱）<br>所屬之採購單位代碼（名稱）">
                                        <ItemTemplate>
                                            <asp:Label CssClass="control-label" Text='<%#Eval("OVC_TOP_DEPT")%>' runat="server"></asp:Label>（
                                            <asp:Label CssClass="control-label" ForeColor="Red" Text='<%#Eval("OVC_TOP_DEPT_NAME")%>' runat="server"></asp:Label>）
                                            </br>
                                            <asp:Label CssClass="control-label" Text='<%#Eval("OVC_PURCHASE_DEPT")%>' runat="server"></asp:Label>（
                                            <asp:Label CssClass="control-label" ForeColor="Red" Text='<%#Eval("OVC_PURCHASE_DEPT_NAME")%>' runat="server"></asp:Label>）
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="採購單位類別<br>接轉單位類別" DataField="" />--%>
                                    <asp:TemplateField HeaderText="採購單位類別<br>接轉單位類別">
                                        <ItemTemplate>
                                            <asp:Label CssClass="control-label" Text='<%#Eval("OVC_CLASS_NAME")%>' runat="server"></asp:Label>
                                            </br>
                                            <asp:Label CssClass="control-label" Text='<%#Eval("OVC_CLASS2_NAME")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="單位狀況" DataField="OVC_ENABLE"/>
                                </Columns>
                            </asp:GridView>
                            <%--<div>
                                <asp:ListView ID="Querylist" OnItemDataBound="Querylist_ItemDataBound" OnItemCommand="Querylist_ItemCommand" OnPagePropertiesChanging="Querylist_PagePropertiesChanging" runat="server">
                                    <layouttemplate>
                                        <table class="table table-bordered text-center">
                                            <tr runat="server">
                                                <th runat="server"><asp:Label CssClass="th_label" runat="server">維護</asp:Label></th>
                                                <th runat="server"><asp:Label CssClass="th_label" runat="server">單位代碼</asp:Label></th>
                                                <th runat="server"><asp:Label CssClass="th_label" runat="server">單位名稱</asp:Label></th>
                                                <th runat="server"><asp:Label CssClass="th_label" runat="server">採購單位</asp:Label></th>
                                                <th runat="server"><asp:Label CssClass="th_label" runat="server">採購上級單位代碼(名稱)<br>所屬之採購單位代碼(名稱)</asp:Label></th>
                                                <th runat="server"><asp:Label CssClass="th_label" runat="server">採購單位類別<br>接轉單位類別</asp:Label></th>
                                                <th runat="server"><asp:Label CssClass="th_label" runat="server">單位狀況</asp:Label></th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server"></tr>
                                        </table>
                                    </layouttemplate>
                                    <ItemTemplate>
                                        <tr>
                                        <td>
                                            <asp:Button ID="btnModify" CssClass="btn-success" CommandName="btnModify" CommandArgument='<%#Eval("OVC_DEPT_CDE")%>' Text="異動" runat="server" />
                                            <br/>
                                            <br/>
                                            <asp:Button ID="btnDel" CssClass="btn-success" OnClientClick="return confirm('確認刪除？');" CommandName="btnDel" CommandArgument='<%#Eval("OVC_DEPT_CDE")%> Text="刪除"' runat="server" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="btnToSubUnit" CommandName="ToSubUnit" CommandArgument='<%#Eval("OVC_DEPT_CDE")%>' Text='<%#Eval("OVC_DEPT_CDE")%>' runat="server"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:Label CssClass="control-label" Text='<%#Eval("OVC_ONNAME")%>' runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPURCHASE_OK" CssClass="control-label" Text='<%#Eval("OVC_PURCHASE_OK")%>' runat="server"></asp:Label>
                                            </br>
                                            <asp:LinkButton ID="linkBtnPURCHASE_OK" ForeColor="Red" CommandName="ToSubUnit" CommandArgument='<%#Eval("OVC_DEPT_CDE")%>' Visible="false" runat="server">下轄單位</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:Label CssClass="control-label" Text='<%#Eval("OVC_TOP_DEPT")%>' runat="server"></asp:Label>(
                                            <asp:Label CssClass="control-label" ForeColor="Red" Text='<%#Eval("OVC_TOP_DEPT_NAME")%>' runat="server"></asp:Label>)
                                            </br>
                                            <asp:Label CssClass="control-label" Text='<%#Eval("OVC_PURCHASE_DEPT")%>' runat="server"></asp:Label>(
                                            <asp:Label CssClass="control-label" ForeColor="Red" Text='<%#Eval("OVC_PURCHASE_DEPT_NAME")%>' runat="server"></asp:Label>)
                                        </td>
                                        <td>
                                            <asp:Label CssClass="control-label" Text='<%#Eval("OVC_CLASS_NAME")%>' runat="server"></asp:Label>
                                            </br>
                                            <asp:Label CssClass="control-label" Text='<%#Eval("OVC_CLASS2_NAME")%>' runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblOVC_ENABLE" CssClass="control-label" Text='<%#Eval("OVC_ENABLE")%>' runat="server"></asp:Label>
                                        </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                                <div class="text-center">
                                    <asp:DataPager PagedControlID="Querylist" ID="ContactsDataPager" PageSize="10" runat="server">
                                        <Fields>
                                            <asp:NextPreviousPagerField ButtonCssClass="btn-success" ButtonType="Button" ShowFirstPageButton="false"  PreviousPageText="上一頁" />
                                            <asp:NumericPagerField />
                                            <asp:NextPreviousPagerField ButtonCssClass="btn-success" ButtonType="Button" ShowFirstPageButton="false"  NextPageText="下一頁" />
                                        </Fields>
                                    </asp:DataPager>
                                </div>
                            </div>--%>
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
