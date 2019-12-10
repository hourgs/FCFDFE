<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EDFQUERY.aspx.cs" Inherits="FCFDFE.pages.MTS.A.EDFQUERY" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!-- Bootstrap core CSS -->
    <link href="~/assets/css/bootstrap.css" rel="stylesheet"/>
    <link href="~/assets/css/bootstrap-reset.css" rel="stylesheet"/>
<script>
    $(document).ready(function () {
        $("<%=strMenuName%>").addClass("active");
        $("<%=strMenuNameItem%>").addClass("active");
    });
</script>
</head>
<body runat="server">
    <div class="row">
        <div style="width: 610px; margin:auto;">
            <section class="panel">                          
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <form runat="server">
                                <asp:Panel ID="pnTop" runat="server">
                                    <%--[案號]、[發貨單位]、[備考]GV--%>
                                    <asp:GridView ID="gvOneCol" CssClass="table table-striped border-top text-center table-inner" AutoGenerateColumns="false" runat="server" >
                                        <Columns>
                                            <asp:BoundField />
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                                <asp:Panel ID="pnBottom" runat="server">
                                    <%--[CONSIGNEE]、[NOTIFY PARTY]、[NOTIFY PARTY] 、[ALSO NOTIFY PARTY2]查詢GV--%>
                                    <table class="table table-bordered text-center">
                                        <tr>
                                            <td colspan="3" style="text-align:center"><h4>查詢</h4></td>
                                        </tr>
                                        <tr>
                                            <td style="width:25%;text-align:center">查詢條件</td>
								            <td colspan="2" class="text-left">
								                地址 &nbsp<asp:TextBox ID="txtAddress" CssClass="tb tb-l" runat="server"></asp:TextBox>
								            </td>
								        </tr>
                                        <tr>
                                            <td colspan="3" >
                                                <div style="text-align:center">
								                    <asp:Button ID="btnQueryA" cssclass="btn-success btnw2" OnClick="btnQuery_Click" Text="查詢" runat="server" />
							                    </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="width:100%"><br />
                                                <asp:GridView ID="gvCONSIGNEE" CssClass="table table-striped border-top text-center table-inner" AutoGenerateColumns="false" runat="server">
							                        <Columns>
							                            <asp:BoundField HeaderText="地址" DataField="OVC_CON_ENG_ADDRESS" />
							                            <asp:BoundField HeaderText="電話" DataField="OVC_CON_TEL" />
							                            <asp:BoundField HeaderText="傳真" DataField="OVC_CON_FAX" />
							                        </Columns>
							                    </asp:GridView>
                                                <asp:GridView ID="gvNOTIFYPARTY" CssClass="table table-striped border-top text-center table-inner" AutoGenerateColumns="false" runat="server">
							                        <Columns>
							                            <asp:BoundField HeaderText="地址" DataField="OVC_NP_ENG_ADDRESS" />
							                            <asp:BoundField HeaderText="電話" DataField="OVC_NP_TEL" />
							                            <asp:BoundField HeaderText="傳真" DataField="OVC_NP_FAX" />
							                        </Columns>
							                    </asp:GridView>
                                                <asp:GridView ID="gvALSONOTIFYPARTY1" CssClass="table table-striped border-top text-center table-inner" AutoGenerateColumns="false" runat="server">
							                        <Columns>
							                            <asp:BoundField HeaderText="地址" DataField="OVC_ANP_ENG_ADDRESS" />
							                            <asp:BoundField HeaderText="電話" DataField="OVC_ANP_TEL" />
							                            <asp:BoundField HeaderText="傳真" DataField="OVC_ANP_FAX" />
							                        </Columns>
							                    </asp:GridView>
                                                <asp:GridView ID="gvALSONOTIFYPARTY2" CssClass="table table-striped border-top text-center table-inner" AutoGenerateColumns="false" runat="server">
							                        <Columns>
							                            <asp:BoundField HeaderText="地址" DataField="OVC_ANP_ENG_ADDRESS2" />
							                            <asp:BoundField HeaderText="電話" DataField="OVC_ANP_TEL2" />
							                            <asp:BoundField HeaderText="傳真" DataField="OVC_ANP_FAX2" />
							                        </Columns>
							                    </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </form>
                        </div>
                    </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                </footer>
                </div>
            </section>
        </div>
    </div>
</body>
</html>
