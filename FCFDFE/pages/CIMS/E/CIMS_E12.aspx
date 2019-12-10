<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_E12.aspx.cs" Inherits="FCFDFE.pages.CIMS.E.CIMS_E12" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<script>
		$(document).ready(function () {
			$("<%=strMenuName%>").addClass("active");
			$("<%=strMenuNameItem%>").addClass("active");
		});
	</script>

	<div class="row">
		<div style="width: 900px; margin:auto;">
			<section class="panel">
				<header  class="title">
					<!--標題-->
					<div>常用網址</div>
				</header>
				<asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
				<div class="panel-body" style=" border: solid 2px;">
					<div class="form" style="border: 5px;">
						<div class="cmxform form-horizontal tasi-form">
							<!--網頁內容-->
							<div class="container">
                                <div class="col-md-6 text-right">
                                    <asp:Button ID="btnArmy" CssClass="btn-success btnw4" runat="server" Text="軍網類" Onclick="btnArmy_Click"/>
                                </div>
                                <div class="col-md-6">
                                    <asp:Button ID="btnCivi" CssClass="btn-success btnw4" runat="server" Text="民網類" Onclick="btnCivi_Click"/>
                                </div>
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