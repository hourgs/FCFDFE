<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D11.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D11" %>

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
                    <!--標題-->
                    採購室採購發包-分案作業
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="subtitle"> 查詢購案 </div><br />
                            <div class="text-center">
                            <asp:Label CssClass="control-label" Text="請輸入購案編號(第一組至第三組，共七碼)：" runat="server"></asp:Label>
                            <asp:TextBox ID="txtOVC_PURCH" CssClass="tb tb-s" runat="server"></asp:TextBox>&nbsp;
                            <asp:Button ID="btnSearchData" cssclass="btn-default" Text="查詢購案移辦資料" OnClick="btnSearchData_Click" runat="server"  />&nbsp;&nbsp;
                            <asp:Button ID="btnSearchStatus" cssclass="btn-default" Text="查詢購案目前狀態" OnClick="btnSearchStatus_Click" runat="server" />&nbsp;&nbsp;
                            <asp:Button ID="btnAllRevoked" cssclass="btn-default" Text="查詢所有撤案資料" OnClick="btnAllRevoked_Click" runat="server" />
                            </div><br /><br />

                            <div class="subtitle"> 其他作業 </div><br />
                            <p  style="text-align:center">
                                <asp:Button ID="btnPreview" cssclass="btn-default" Text="預覽分組勾稽作業" OnClick="btnPreview_Click" ForeColor="Blue" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCalculate" cssclass="btn-default" Text="計畫清單明細分組作業" OnClick="btnCalculate_Click" ForeColor="Blue" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnPhreseReload" cssclass="btn-default" Text="片語重新載入" Visible="false" runat="server" />
                            </p><br />
                            
                            <div class="subtitle" style="text-align:center;color:blue">分案明細表(分案人:<asp:Label ID="lblUserName" runat="server"></asp:Label>）</div>
                            <p class="text-center">
                                <asp:RadioButtonList ID="rdoFilter" CssClass="radioButton" ForeColor="Red" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server" >
                                    <asp:ListItem Value="1" >已移履約驗結單位之購案不顯示</asp:ListItem>
                                    <asp:ListItem Value="2" >顯示全部購案</asp:ListItem>
                                </asp:RadioButtonList><br /><br />
                                <asp:Label CssClass="control-label" Text="計畫年度(第二組)：" runat="server"></asp:Label>
                                <asp:DropDownList ID="drpOVC_BUDGET_YEAR" CssClass="tb tb-s" runat="server"></asp:DropDownList>&nbsp;&nbsp;
                                <asp:Button ID="btnSearchByOVC_PURCH" cssclass="btn-default" Text="依購案編號查詢" OnClick="btnSearchByOVC_PURCH_Click" runat="server"/>&nbsp;&nbsp;
                                <asp:Button ID="btnSearchByNo" cssclass="btn-default" Text="依採購號碼查詢" OnClick="btnSearchByNo_Click" runat="server"/>&nbsp;&nbsp;
                                <asp:Button ID="btnSearchByContractor" cssclass="btn-default" Text="依承辦人查詢" OnClick="btnSearchByContractor_Click" runat="server"/><br /><br />
                            </p>

                            <asp:GridView ID="gvSTATUS" CssClass="table data-table table-striped border-top text-center" AutoGenerateColumns="false"  OnRowCommand="gvSTATUS_RowCommand" OnPreRender="gvSTATUS_PreRender" OnRowDataBound="gvSTATUS_RowDataBound" runat="server">
 		     		            <Columns>
                                    <asp:TemplateField HeaderText="購案編號">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnOVC_PURCH_A" CommandName="DataDetail" Text='<%# Eval("OVC_PURCH_A") %>' runat="server" ></asp:LinkButton>
                                            <asp:Label ID="lblOVC_PURCH" Text='<%# Eval("OVC_PURCH") %>' Style="display:none" runat="server" ></asp:Label>
                                            <asp:Label ID="lblOVC_PUR_AGENCY" Text='<%# Eval("OVC_PUR_AGENCY") %>' Style="display:none" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="採購號碼">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_PURCH_5" Text='<%# Eval("OVC_PURCH_5") %>' CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Button ID="btnInsert" CommandName="Insert" cssclass="btn-default" Text="輸入" Visible='<%# Convert.ToString(Eval("OVC_PURCH_5"))=="" ? false : true %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="組別">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGroup" Text="0" runat="server" ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="階段開始日" DataField="OVC_DBEGIN" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField HeaderText="承辦人" DataField="OVC_DO_NAME" ItemStyle-CssClass="text-center" />
                                    <asp:TemplateField HeaderText="目前階段--00N00--">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_STATUS" Text='<%# Eval("OVC_STATUS") %>' Style="display:none" runat="server"></asp:Label>
                                            <asp:Label ID="lblOVC_STATUS_Desc" Text='<%# Eval("OVC_STATUS_Desc") %>' CssClass="control-label text-red" runat="server"></asp:Label>&nbsp;
                                            <asp:Label ID="lblDate_Flag" Text=" ╳" ForeColor="#ff00ff" Font-Bold="True" Visible='<%# Convert.ToString(Eval("Date_Flag"))=="1" ? true:false %>' runat="server"></asp:Label>
                                            <asp:Label ID="lblOVC_REMARK" Text='<%# Eval("OVC_REMARK") %>' ForeColor="Blue" Font-Bold="true" runat="server" ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="功能">
                                        <ItemTemplate>
                                            <asp:Button ID="btnChange" CommandName="Change" cssclass="btn-default" Text="異動" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
</asp:Content>
