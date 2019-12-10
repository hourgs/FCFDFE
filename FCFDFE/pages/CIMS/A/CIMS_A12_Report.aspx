<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CIMS_A12_Report.aspx.cs" Inherits="FCFDFE.pages.CIMS.A.CIMS_A12_Report" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>國外廠商在台代理╱代表登記表</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="row">
            <div style="width: 800px; margin: auto;">
                <div style="text-align: center;">
                    <font size="5">國外廠商在台代理╱代表登記表</font>
                    <br />
                </div>
                <div style="text-align: right;">
                    編號:<asp:Label ID="txtREGNO" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                </div>
                <div style="text-align: center;">
                    <asp:Table ID="Table1" Style="width: 100%;" border="1" cellspacing="0" runat="server">
                        <asp:TableRow>
                            <asp:TableCell Style="text-align: left;" colspan="2" Height="30px"><font size="4" style="background-color:#D3D3D3">國外廠商資料及代號</font>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Style="text-align: left;" colspan="2">
                                <span style='line-height: 30px;'><%--font-size:15px;'--%>
                                公司名稱：<asp:Label ID="txtVEN_NAME" runat="server" ForeColor="Blue"></asp:Label><br/>
                                部&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;門：<asp:Label ID="txtVEN_DEPT" runat="server" ForeColor="Blue"></asp:Label><br/>
                                代&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;號：<asp:Label ID="txtVEN_CODE" runat="server" ForeColor="Blue"></asp:Label><br/>
                                地&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;址：<asp:Label ID="txtVEN_ADDR" runat="server" ForeColor="Blue"></asp:Label><br/>
                                電&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;話：<asp:Label ID="txtVEN_TEL" runat="server" ForeColor="Blue"></asp:Label><br/>
                                電&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;傳：<asp:Label ID="txtVEN_FAX" runat="server" ForeColor="Blue"></asp:Label><br/>
                                負&nbsp;&nbsp;責&nbsp;&nbsp;人：<asp:Label ID="txtVEN_BOSS" runat="server" ForeColor="Blue"></asp:Label><br/>
                                </span>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Style="text-align: left;" colspan="2" Height="30px"><font size="4" style="background-color:#D3D3D3">在台代理╱代表商資料及代號</font>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Style="text-align: left;"  colspan="2">
                                <span style='line-height: 30px;'><%--font-size:15px;'--%>
                                公司中文名稱：<asp:Label ID="txtVEN_NAME_T" runat="server" ForeColor="Blue"></asp:Label><br/>
                                公司英文名稱：<asp:Label ID="txtVEN_ENAME_T" runat="server" ForeColor="Blue"></asp:Label><br/>
                                代&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;號：<asp:Label ID="txtVEN_CODE_T" runat="server" ForeColor="Blue"></asp:Label><br/>
                                營業地址：<asp:Label ID="txtVEN_ADDR_T" runat="server" ForeColor="Blue"></asp:Label><br/>
                                電&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;話：<asp:Label ID="txtVEN_TEL_T" runat="server" ForeColor="Blue"></asp:Label><br/>
                                電&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;傳：<asp:Label ID="txtVEN_FAX_T" runat="server" ForeColor="Blue"></asp:Label><br/>
                                負&nbsp;&nbsp;責&nbsp;&nbsp;人：<asp:Label ID="txtVEN_BOSS_T" runat="server" ForeColor="Blue"></asp:Label><br/>
                                </span>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Style="text-align: left;" colspan="2" Height="30px"><font size="4" style="background-color:#D3D3D3">國外廠商授權在台代理╱代表效期：</font>
                                <asp:Label ID="txtAUTH_DATE_S" runat="server" ForeColor="Blue"></asp:Label>
                                <font color="blue">至</font>
                                <asp:Label ID="txtAUTH_DATE_E" runat="server" ForeColor="Blue"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Style="text-align: left;" colspan="2" Height="30px"><font size="4" style="background-color:#D3D3D3">授權範圍〈權限〉：</font>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Style="text-align: left;" colspan="2" Height="100px">
                                <asp:Label ID="txtAUTH_RANGE" runat="server" ForeColor="Blue"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Style="text-align: left;" colspan="2" Height="30px"><font size="4" style="background-color:#D3D3D3">代理項目〈產品〉：</font><asp:Label ID="Label21" runat="server" ForeColor="Blue"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Style="text-align: left;" colspan="2" Height="160px">
                                <asp:Label ID="txtAGENT_ITEM" runat="server" ForeColor="Blue"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Style="text-align: left;" colspan="2" Height="30px"><font size="4" style="background-color:#D3D3D3">核定在台代理╱代表效期：</font>
                                <asp:Label ID="txtAPPR_DATE_S" runat="server" ForeColor="Blue"></asp:Label>
                                <font color="blue">至</font>
                                <asp:Label ID="txtAPPR_DATE_E" runat="server" ForeColor="Blue"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Style="text-align: left;" Width="50%" Height="140px" valign="top">備考
                            </asp:TableCell>
                            <asp:TableCell Style="text-align: left;" Width="50%" Height="140px">
                                核<br/><br/>
                                定<br/><br/>
                                章<br/><br/>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
