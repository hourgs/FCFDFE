<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SqlConnet.aspx.cs" Inherits="FCFDFE.pages.MPMS.F.SqlConnet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


<div>
<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
<asp:Button ID="Button1" runat="server" OnClick="btnQuery_Click" Text="Query" />
<%--<asp:Button ID="Button2" runat="server" OnClick="btnUpdate_Click" Text="Update" />--%>

</div>

</asp:Content>
