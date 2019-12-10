<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_ESearch.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_ESearch" EnableEventValidation = "false" MaintainScrollPositionOnPostback="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
       

    </script>
    <style>
        tr td:nth-child(2) {
            text-align: left;
        }

        span {
            margin: 0 3px;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title text-red">
                    <!--標題-->
                    統計查詢主畫面
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->

                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnTC" CssClass="btn-default btnw2" runat="server" Text="查詢" OnClick="btnTC_Click"/>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpTimeControl" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="06">106</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年度 <span class="text-green">履驗</span><span class="text-red">時程管制</span>總表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnStatics" CssClass="btn-default btnw2" runat="server" Text="查詢" Onclick="btnStatics_Click"/>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpStatics" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="06">106</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年度 <span class="text-green">未結案</span><span class="text-red">統計</span>表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btn_nondelivery" CssClass="btn-default btnw2" runat="server" Text="查詢" OnClick="btn_nondelivery_Click" />
                                    </td>
                                    <td>
                                        <asp:Label Style="margin: 0 5px;" CssClass="control-label position-left" runat="server">於</asp:Label>
                                        <div class="input-append datepicker" style="width:auto; float:left;">
                                            <asp:TextBox ID="txt_nondelivery" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server"><span class="text-red">前應交貨而未交貨</span>個案統計表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAfter_day" CssClass="btn-default btnw2" runat="server" Text="查詢" OnClick="btnAfter_day_Click" />
                                    </td>
                                    <td>
                                        <asp:Label Style="margin: 0 5px;" CssClass="control-label position-left" runat="server">於</asp:Label>
                                        <div class="input-append datepicker" style="width:auto; float:left;">
                                            <asp:TextBox ID="txtAfter_day" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server"><span class="text-green">收辦後</span><span class="text-red">預劃結案日未能於年度結案</span>個案統計表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnexceed_standard" CssClass="btn-default btnw2" runat="server" Text="查詢" onclick="btnexceed_standard_Click"/>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpexceed_standard" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="06">106</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年度<span class="text-red">結案天數超過標準作業天數</span>個案統計表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAfter_year" CssClass="btn-default btnw2" runat="server" Text="查詢" onclick="btnAfter_year_Click" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpAfter_year" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="106">106</asp:ListItem>
                                            <asp:ListItem Value="105">105</asp:ListItem>
                                            <asp:ListItem Value="104">104</asp:ListItem>
                                            <asp:ListItem Value="103">103</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年度購案<span class="text-green">收辦後</span><span class="text-red">預劃結案日未能於年度結案</span>個案統計表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnShouldhavef" CssClass="btn-default btnw2" runat="server" Text="查詢" onclick="btnShouldhavef_Click" />
                                    </td>
                                    <td>
                                        <asp:Label Style="margin: 0 5px;" CssClass="control-label position-left" runat="server">購案<span class="text-red">預劃結案應於</span></asp:Label>
                                        <div class="input-append datepicker" style="width:auto; float:left;">
                                            <asp:TextBox ID="txtShouldhavef" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server"><span class="text-red">前應結案而未結案</span>個案統計表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnWithoutwarrantymoney" CssClass="btn-default btnw2" runat="server" Text="查詢" OnClick="btnWithoutwarrantymoney_Click" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpWithoutwarrantymoney" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="06">106</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年度<span class="text-red">保證金不含保固金</span>管制明細表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnpguaranteenotrefund" CssClass="btn-default btnw2" runat="server" Text="查詢" OnClick="btnpguaranteenotrefund_Click" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpguaranteenotrefund" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="06" Text="106"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年度 <span class="text-red">尚未發還保證金</span>管制明細表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnWarrantyGold" CssClass="btn-default btnw2" runat="server" Text="查詢" OnClick="btnWarrantyGold_Click" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpWarrantyGold" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="06">106</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年度 <span class="text-red">保固金</span>管制明細表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnWarrantyGoldnotRe" CssClass="btn-default btnw2" runat="server" Text="查詢" onclick="btnWarrantyGoldnotRe_Click" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpWarrantyGoldnotRe" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="06">106</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年度<span class="text-red">尚未發還保固金</span>管制明細表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnReceivePurchase" CssClass="btn-default btnw2" runat="server" Text="查詢" OnClick="btnReceivePurchase_Click" />
                                    </td>
                                    <td>
                                        <asp:Label Style="margin: 0 5px;" CssClass="control-label position-left" runat="server">自</asp:Label>
                                        <div class="input-append datepicker" style="width:auto; float:left;">
                                            <asp:TextBox ID="txtRePurD1" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server">至&emsp;</asp:Label>
                                        <div class="input-append datepicker" style="width:auto; float:left;">
                                            <asp:TextBox ID="txtRePurD2" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server"><span class="text-red">收辦購案</span>統計表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnClosing" CssClass="btn-default btnw2" runat="server" Text="查詢" OnClick="btnClosing_Click" />
                                    </td>
                                    <td>
                                        <asp:Label Style="margin: 0 5px;" CssClass="control-label position-left" runat="server">自</asp:Label>
                                        <div class="input-append datepicker" style="width:auto; float:left;">
                                            <asp:TextBox ID="txtClosingD1" CssClass="tb tb-s position-left"  runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server">至&emsp;</asp:Label>
                                        <div class="input-append datepicker" style="width:auto; float:left;">
                                            <asp:TextBox ID="txtClosingD2" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server"><span class="text-red">結案購案</span>統計表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnDJOINCHECK" CssClass="btn-default btnw2" runat="server" Text="查詢" onclick="btnDJOINCHECK_Click" />
                                    </td>
                                    <td>
                                        <asp:Label Style="margin: 0 5px;" CssClass="control-label position-left" runat="server">自</asp:Label>
                                        <div class="input-append datepicker" style="width:auto; float:left;">
                                            <asp:TextBox ID="txtDJOINCHECK" CssClass="tb tb-s position-left"  runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server">至&emsp;</asp:Label>
                                        <div class="input-append datepicker" style="width:auto; float:left;">
                                            <asp:TextBox ID="txtDJOINCHECK2" CssClass="tb tb-s position-left"    runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server"><span class="text-red">會驗購案</span>統計表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnPenaltyfuse" CssClass="btn-default btnw2" runat="server" Text="查詢" onclick="btnPenaltyfuse_Click" />
                                    </td>
                                    <td>
                                        <asp:Label Style="margin: 0 5px;" CssClass="control-label position-left" runat="server">自</asp:Label>
                                        <div class="input-append datepicker" style="width:auto; float:left;">
                                            <asp:TextBox ID="txtPenaltyfuse" CssClass="tb tb-s position-left"  runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server">至&emsp;</asp:Label>
                                        <div class="input-append datepicker" style="width:auto; float:left;">
                                            <asp:TextBox ID="txtPenaltyfuse2" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server"><span class="text-red">支用逾罰購案</span>統計表<span class="text-red">(查詢日期以支用日期計算)</span></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAnnualBatchC" CssClass="btn-default btnw2" runat="server" Text="查詢" onclick="btnAnnualBatchC_Click" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpAnnualBatchC" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="06">106</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年度 <span class="text-red">履驗分年分批時程管制</span>總表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnNSecuritydeposit" CssClass="btn-default btnw2" runat="server" Text="查詢" OnClick="btnNSecuritydeposit_Click" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpNSecuritydeposit" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="06">106</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server">年度<span class="text-red">新式保證金</span>管制明細表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnMarginExpiration" CssClass="btn-default btnw2" runat="server" Text="查詢" onclick="btnMarginExpiration_Click" />
                                    </td>
                                    <td>
                                        <asp:Label Style="margin: 0 5px;" CssClass="control-label position-left" runat="server">自</asp:Label>
                                        <div class="input-append datepicker" style="width:auto; float:left;">
                                            <asp:TextBox ID="txtMarginExpiration" CssClass="tb tb-s position-left"  runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server">至&emsp;</asp:Label>
                                        <div class="input-append datepicker" style="width:auto; float:left;">
                                            <asp:TextBox ID="txtMarginExpiration2" CssClass="tb tb-s position-left"  runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server"><span class="text-red">保證金到期</span>管制明細表</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSix" CssClass="btn-default btnw2" runat="server" Text="查詢" OnClick="btnSix_Click" />
                                    </td>
                                    <td>
                                        <div class="container">
                                            <div class="row" style="margin-bottom:10px;">
                                            <div class="col-md-3">
                                                <asp:Label CssClass="control-label" runat="server">年度</asp:Label>
                                                <asp:DropDownList ID="drpSixY" CssClass="tb tb-s" runat="server">
                                                    <asp:ListItem Value="05">105</asp:ListItem>
                                                    <asp:ListItem Value="06">106</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-5">
                                                <asp:CheckBoxList ID="chkOVC_PUR_NSECTION" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                                    <asp:ListItem Value="值" Text=""></asp:ListItem>
                                                </asp:CheckBoxList>
                                                <asp:Label CssClass="control-label" runat="server">申購單位</asp:Label>
                                                <asp:TextBox ID="txtOVC_ONNAME" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                <asp:Button ID="btnQueryOVC_DEPT_CDE" OnClientClick="OpenWindow('txtOVC_DEPT_CDE', 'txtOVC_ONNAME')" CssClass="btn-success" Text="單位查詢" runat="server" />
                                                <asp:HiddenField ID="txtOVC_DEPT_CDE" runat="server" />
                                                
                                            </div>
                                            <div class="col-md-4">
                                                <asp:CheckBoxList ID="CheckBoxList2" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                                    <asp:ListItem Value="值" Text=""></asp:ListItem>
                                                </asp:CheckBoxList>
                                                <asp:Label CssClass="control-label" runat="server">採購方式</asp:Label>
                                                <asp:DropDownList ID="DropDownList11" CssClass="tb tb-s" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                                </div>
                                            <div class="row">
                                            <div class="col-md-4">
                                                <asp:CheckBoxList ID="CheckBoxList1" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                                    <asp:ListItem Value="值" Text=""></asp:ListItem>
                                                </asp:CheckBoxList>
                                                <asp:Label CssClass="control-label" runat="server">適用性質</asp:Label>
                                                <asp:TextBox ID="TextBox14" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:CheckBoxList ID="CheckBoxList3" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                                    <asp:ListItem Value="值" Text=""></asp:ListItem>
                                                </asp:CheckBoxList>
                                                <asp:Label CssClass="control-label" runat="server">購案類型</asp:Label>
                                                <asp:DropDownList ID="DropDownList12" CssClass="tb tb-s" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:CheckBoxList ID="CheckBoxList4" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                                    <asp:ListItem Value="值" Text=""></asp:ListItem>
                                                </asp:CheckBoxList>
                                                <asp:Label CssClass="control-label" runat="server">履約進度</asp:Label>
                                                <asp:DropDownList ID="DropDownList13" CssClass="tb tb-s" runat="server">
                                                    <asp:ListItem Value="3">履驗單位待收辦</asp:ListItem>
                                                    <asp:ListItem Value="31">契約接管</asp:ListItem>
                                                    <asp:ListItem Value="32">契約管理</asp:ListItem>
                                                    <asp:ListItem Value="33">待交貨</asp:ListItem>
                                                    <asp:ListItem Value="34">待會驗</asp:ListItem>
                                                    <asp:ListItem Value="35">待驗收</asp:ListItem>
                                                    <asp:ListItem Value="36">待結報</asp:ListItem>
                                                    <asp:ListItem Value="37">保證金管制</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                                </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            
                            <div class="text-center">
                                <asp:Button ID="btnReturn" CssClass="btn-default btnw4" OnClick="btnReturn_Click" runat="server" Text="回上一頁" />
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                    <asp:GridView ID="GV_Q2" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Q2_RowCreated" OnRowDataBound="GV_Q2_RowDataBound" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="項次" DataField="" />
                            <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                            <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                            <asp:BoundField HeaderText="得標商" DataField="OVC_VEN_TITLE" />
                            <asp:BoundField HeaderText="交貨日期" DataField="OVC_DELIVERY" />
                            <asp:BoundField HeaderText="標準驗收天數" DataField="ONB_DINSPECT_SOP" />
                            <asp:BoundField HeaderText="目前採購天數" DataField="ONB_DAYS_CONTRACT" />
                            <asp:BoundField HeaderText="承辦人" DataField="OVC_DO_NAME" />
                	    </Columns>
	   		        </asp:GridView>
                    <asp:GridView ID="GV_Q3" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Q3_RowCreated" OnRowDataBound="GV_Q3_RowDataBound" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="項次" DataField="" />
                            <asp:TemplateField HeaderText="購案資訊" >
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" Text='<%# Bind("OVC_PURCH") %>' runat="server" /><br />
                                            <asp:Label ID="Label2" Text='<%# Bind("OVC_PUR_IPURCH") %>' runat="server" /><br />
                                            <asp:Label ID="Label3" Text='<%# Bind("OVC_PUR_NSECTION") %>' runat="server" /><br />
                                            <asp:Label ID="Label4" Text='<%# Bind("ONB_MCONTRACT") %>' runat="server" /><br />
                                            <asp:Label ID="Label5" Text='<%# Bind("OVC_DELIVERY_CONTRACT") %>' runat="server" /><br />
                                            <asp:Label ID="Label6" Text='<%# Bind("OVC_VEN_TITLE") %>' runat="server" /><br />
                                            <asp:Label ID="Label7" Text='<%# Bind("OVC_DO_NAME") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                            <asp:BoundField HeaderText="重要資訊" DataField="OVC_RECEIVE_COMM" />
                	    </Columns>
	   		        </asp:GridView>
                    <asp:GridView ID="GV_Q4" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Q4_RowCreated" OnRowDataBound="GV_Q4_RowDataBound" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="項次" DataField="" />
                            <asp:TemplateField HeaderText="購案資訊" >
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" Text='<%# Bind("OVC_PURCH") %>' runat="server" /><br />
                                            <asp:Label ID="Label2" Text='<%# Bind("OVC_PUR_IPURCH") %>' runat="server" /><br />
                                            <asp:Label ID="Label3" Text='<%# Bind("OVC_PUR_NSECTION") %>' runat="server" /><br />
                                            <asp:Label ID="Label4" Text='<%# Bind("ONB_MCONTRACT") %>' runat="server" /><br />
                                            <asp:Label ID="Label5" Text='<%# Bind("OVC_DJOINCHECK") %>' runat="server" /><br />
                                            <asp:Label ID="Label8" Text='<%# Bind("OVC_DPAY_PLAN") %>' runat="server" /><br />
                                            <asp:Label ID="Label9" Text='<%# Bind("OVC_DELIVERY") %>' runat="server" /><br />
                                            <asp:Label ID="Label6" Text='<%# Bind("OVC_VEN_TITLE") %>' runat="server" /><br />
                                            <asp:Label ID="Label7" Text='<%# Bind("OVC_PUR_USER") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                            <asp:BoundField HeaderText="重要資訊" DataField="OVC_RECEIVE_COMM" />
                	    </Columns>
	   		        </asp:GridView>
                    <asp:GridView ID="GV_Q5" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Q5_RowCreated" OnRowDataBound="GV_Q5_RowDataBound" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="項次" DataField="" />
                            <asp:BoundField HeaderText="購案編號" DataField="購案編號" />
                            <asp:BoundField HeaderText="購案名稱" DataField="購案名稱" />
                            <asp:BoundField HeaderText="申購單位" DataField="申購單位" />
                            <asp:BoundField HeaderText="合約金額" DataField="合約金額" />
                            <asp:BoundField HeaderText="實際交貨日期" DataField="實際交貨日期" />
                            <asp:BoundField HeaderText="會驗日期" DataField="會驗日期" />
                            <asp:BoundField HeaderText="結案日期" DataField="結案日期" />
                            <asp:BoundField HeaderText="使用天數" DataField="使用天數" />
                            <asp:BoundField HeaderText="標準天數" DataField="標準天數" />
                            <asp:BoundField HeaderText="延誤天數" DataField="延誤天數" />
                            <asp:BoundField HeaderText="重要事項" DataField="重要事項" />
                            <asp:BoundField HeaderText="承辦人" DataField="承辦人" />
                	    </Columns>
	   		        </asp:GridView>
                    <asp:GridView ID="GV_Q6" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Q6_RowCreated" OnRowDataBound="GV_Q6_RowDataBound" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="項次" DataField="" />
                            <asp:BoundField HeaderText="購案編號" DataField="購案編號" />
                            <asp:BoundField HeaderText="購案名稱" DataField="購案名稱" />
                            <asp:BoundField HeaderText="申購單位" DataField="申購單位" />
                            <asp:BoundField HeaderText="預算年度" DataField="預算年度" />
                            <asp:BoundField HeaderText="簽約日期" DataField="簽約日期" />
                            <asp:BoundField HeaderText="收辦日期" DataField="收辦日期" />
                            <asp:BoundField HeaderText="交貨天數" DataField="交貨天數" />
                            <asp:BoundField HeaderText="驗收天數" DataField="驗收天數" />
                            <asp:BoundField HeaderText="合約金額" DataField="合約金額" />
                            <asp:BoundField HeaderText="支用金額" DataField="支用金額" />
                            <asp:BoundField HeaderText="預劃結案日" DataField="預劃結案日" />
                            <asp:BoundField HeaderText="重要事項" DataField="重要事項" />
                            <asp:BoundField HeaderText="承辦人" DataField="承辦人" />
                	    </Columns>
	   		        </asp:GridView>
                    <asp:GridView ID="GV_Q7" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Q7_RowCreated" OnRowDataBound="GV_Q7_RowDataBound" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="項次" DataField="" />
                            <asp:BoundField HeaderText="購案編號" DataField="購案編號" />
                            <asp:BoundField HeaderText="購案名稱" DataField="購案名稱" />
                            <asp:BoundField HeaderText="申購單位" DataField="申購單位" />
                            <asp:BoundField HeaderText="合約金額" DataField="合約金額" />
                            <asp:BoundField HeaderText="支用金額" DataField="支用金額" />
                            <asp:BoundField HeaderText="預劃結案日" DataField="預劃結案日" />
                            <asp:BoundField HeaderText="重要事項" DataField="重要事項" />
                            <asp:BoundField HeaderText="承辦人" DataField="承辦人" />
                	    </Columns>
	   		        </asp:GridView>
                    <asp:GridView ID="GV_Q8" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Q8_RowCreated" OnRowDataBound="GV_Q8_RowDataBound" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="項次" DataField="" />
                            <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                            <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                            <asp:BoundField HeaderText="合約金額" DataField="ONB_MONEY" />
                            <asp:BoundField HeaderText="履保金種類" DataField="PRO" />
                            <asp:BoundField HeaderText="履保金額" DataField="ONB_ALL_MONEY" />
                            <asp:BoundField HeaderText="廠商名稱" DataField="OVC_OWN_NAME" />
                            <asp:BoundField HeaderText="單證編號" DataField="OVC_COMPTROLLER_NO" />
                            <asp:BoundField HeaderText="收繳單位" DataField="OVC_ONNAME" />
                            <asp:BoundField HeaderText="退還日期" DataField="OVC_DBACK" />
                            <asp:BoundField HeaderText="承辦人" DataField="OVC_DO_NAME" />
                	    </Columns>
	   		        </asp:GridView>
                    <asp:GridView ID="GV_Q9" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Q9_RowCreated" OnRowDataBound="GV_Q9_RowDataBound" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="項次" DataField="" />
                            <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                            <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                            <asp:BoundField HeaderText="種類" DataField="KIND" />
                            <asp:BoundField HeaderText="性質" DataField="PRO" />
                            <asp:BoundField HeaderText="金額" DataField="ONB_ALL_MONEY" />
                            <asp:BoundField HeaderText="廠商名稱" DataField="OVC_OWN_NAME" />
                            <asp:BoundField HeaderText="收繳單位" DataField="OVC_ONNAME" />
                            <asp:BoundField HeaderText="單證編號" DataField="OVC_COMPTROLLER_NO" />
                            <asp:BoundField HeaderText="起始日期" DataField="OVC_DGARRENT_START" />
                            <asp:BoundField HeaderText="文件有效日期" DataField="OVC_DEFFECT_1" />
                            <asp:BoundField HeaderText="到期日期" DataField="OVC_DGARRENT_END" />
                            <asp:BoundField HeaderText="退還日" DataField="OVC_DBACK" />
                            <asp:BoundField HeaderText="承辦人" DataField="OVC_DO_NAME" />
                	    </Columns>
	   		        </asp:GridView>
                    <asp:GridView ID="GV_Q10" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Q10_RowCreated" OnRowDataBound="GV_Q10_RowDataBound" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="項次" DataField="" />
                            <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                            <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                            <asp:BoundField HeaderText="合約金額" DataField="ONB_MONEY" />
                            <asp:BoundField HeaderText="保固金種類" DataField="PRO" />
                            <asp:BoundField HeaderText="保固金額" DataField="ONB_ALL_MONEY" />
                            <asp:BoundField HeaderText="廠商名稱" DataField="OVC_OWN_NAME" />
                            <asp:BoundField HeaderText="單證編號" DataField="OVC_COMPTROLLER_NO" />
                            <asp:BoundField HeaderText="收繳單位" DataField="OVC_ONNAME" />
                            <asp:BoundField HeaderText="保固金到期日" DataField="OVC_MARK" />
                            <asp:BoundField HeaderText="文件有效日" DataField="OVC_DEFFECT_1" />
                            <asp:BoundField HeaderText="退還日期" DataField="OVC_DBACK" />
                            <asp:BoundField HeaderText="承辦人" DataField="OVC_DO_NAME" />
                	    </Columns>
	   		        </asp:GridView>
                    <asp:GridView ID="GV_Q11" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Q11_RowCreated" OnRowDataBound="GV_Q11_RowDataBound" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="項次" DataField="" />
                            <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                            <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                            <asp:BoundField HeaderText="種類" DataField="KIND" />
                            <asp:BoundField HeaderText="性質" DataField="PRO" />
                            <asp:BoundField HeaderText="金額" DataField="ONB_ALL_MONEY" />
                            <asp:BoundField HeaderText="廠商名稱" DataField="OVC_OWN_NAME" />
                            <asp:BoundField HeaderText="收繳單位" DataField="OVC_ONNAME" />
                            <asp:BoundField HeaderText="單證編號" DataField="OVC_COMPTROLLER_NO" />
                            <asp:BoundField HeaderText="起始日期" DataField="OVC_DGARRENT_START" />
                            <asp:BoundField HeaderText="文件有效日期" DataField="OVC_DEFFECT_1" />
                            <asp:BoundField HeaderText="到期日期" DataField="OVC_DGARRENT_END" />
                            <asp:BoundField HeaderText="退還日" DataField="OVC_DBACK" />
                            <asp:BoundField HeaderText="承辦人" DataField="OVC_DO_NAME" />
                	    </Columns>
	   		        </asp:GridView>
                    <asp:GridView ID="GV_Q12" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Q12_RowCreated" OnRowDataBound="GV_Q12_RowDataBound" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="項次" DataField="" />
                            <asp:BoundField HeaderText="購案編號" DataField="purch" />
                            <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                            <asp:BoundField HeaderText="申購單位" DataField="OVC_PUR_NSECTION" />
                            <asp:BoundField HeaderText="預算年度" DataField="OVC_YY" />
                            <asp:BoundField HeaderText="預算科目" DataField="OVC_POI_IBDG" />                   
                            <asp:BoundField HeaderText="預算科目名稱" DataField="OVC_PJNAME" />          
                            <asp:BoundField HeaderText="合約金額" DataField="ONB_MCONTRACT" />           
                            <asp:BoundField HeaderText="支用金額" DataField="ONB_PAY_MONEY" />           
                            <asp:BoundField HeaderText="簽約日期" DataField="OVC_DCONTRACT" />           
                            <asp:BoundField HeaderText="收辦日期" DataField="OVC_DRECEIVE" />            
                            <asp:BoundField HeaderText="契約交貨日期" DataField="OVC_DELIVERY_CONTRACT" /> 
                            <asp:BoundField HeaderText="實際交貨日期" DataField="OVC_DELIVERY" />        
                            <asp:BoundField HeaderText="實際交貨天數" DataField="ONB_DAYS_CONTRACT" />    
                            <asp:BoundField HeaderText="會驗日期" DataField="OVC_DJOINCHECK" />          
                            <asp:BoundField HeaderText="結案日期" DataField="OVC_DPAY" />                
                            <asp:BoundField HeaderText="預估結案日期" DataField="預估結案日" />          
                            <asp:BoundField HeaderText="使用天數" DataField="use_day" />                 
                            <asp:BoundField HeaderText="標準天數" DataField="ONB_DINSPECT_SOP" />        
                            <asp:BoundField HeaderText="延誤天數" DataField="ONB_DELAY_DAYS" />          
                            <asp:BoundField HeaderText="重要事項" DataField="OVC_RECEIVE_COMM" />        
                            <asp:BoundField HeaderText="逾期計罰" DataField="OnB_DELAY_DAYS" />          
                            <asp:BoundField HeaderText="減價收受" DataField="onb_mins_money_1" />        
                            <asp:BoundField HeaderText="下授" DataField="OVC_GRANT_TO" />                
                            <asp:BoundField HeaderText="承辦人" DataField="OVC_DO_NAME" />               
                            <asp:BoundField HeaderText="廠商名稱" DataField="OVC_VEN_TITLE" />           
                            <asp:BoundField HeaderText="是否歸檔" DataField="OVC_CLOSE" />               
                	    </Columns>
	   		        </asp:GridView>
                    <asp:GridView ID="GV_Q13" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Q13_RowCreated" OnRowDataBound="GV_Q13_RowDataBound" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="項次" DataField="" />
                            <asp:BoundField HeaderText="購案編號" DataField="purch" />
                            <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                            <asp:BoundField HeaderText="申購單位" DataField="OVC_PUR_NSECTION" />
                            <asp:BoundField HeaderText="預算年度" DataField="OVC_YY" />
                            <asp:BoundField HeaderText="預算科目" DataField="OVC_POI_IBDG" />                   
                            <asp:BoundField HeaderText="預算科目名稱" DataField="OVC_PJNAME" />          
                            <asp:BoundField HeaderText="合約金額" DataField="ONB_MCONTRACT" />           
                            <asp:BoundField HeaderText="支用金額" DataField="ONB_PAY_MONEY" />           
                            <asp:BoundField HeaderText="簽約日期" DataField="OVC_DCONTRACT" />           
                            <asp:BoundField HeaderText="收辦日期" DataField="OVC_DRECEIVE" />            
                            <asp:BoundField HeaderText="契約交貨日期" DataField="OVC_DELIVERY_CONTRACT" /> 
                            <asp:BoundField HeaderText="實際交貨日期" DataField="OVC_DELIVERY" />        
                            <asp:BoundField HeaderText="實際交貨天數" DataField="ONB_DAYS_CONTRACT" />    
                            <asp:BoundField HeaderText="會驗日期" DataField="OVC_DJOINCHECK" />          
                            <asp:BoundField HeaderText="結案日期" DataField="OVC_DPAY" />                
                            <asp:BoundField HeaderText="預估結案日期" DataField="預估結案日" />          
                            <asp:BoundField HeaderText="使用天數" DataField="use_day" />                 
                            <asp:BoundField HeaderText="標準天數" DataField="ONB_DINSPECT_SOP" />        
                            <asp:BoundField HeaderText="延誤天數" DataField="ONB_DELAY_DAYS" />          
                            <asp:BoundField HeaderText="重要事項" DataField="OVC_RECEIVE_COMM" />        
                            <asp:BoundField HeaderText="逾期計罰" DataField="OnB_DELAY_DAYS" />          
                            <asp:BoundField HeaderText="減價收受" DataField="onb_mins_money_1" />        
                            <asp:BoundField HeaderText="下授" DataField="OVC_GRANT_TO" />                
                            <asp:BoundField HeaderText="承辦人" DataField="OVC_DO_NAME" />               
                            <asp:BoundField HeaderText="廠商名稱" DataField="OVC_VEN_TITLE" />           
                            <asp:BoundField HeaderText="是否歸檔" DataField="OVC_CLOSE" />               
                	    </Columns>
	   		        </asp:GridView>
                    <asp:GridView ID="GV_Q14" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Q14_RowCreated" OnRowDataBound="GV_Q14_RowDataBound" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="項次" DataField="" />
                            <asp:BoundField HeaderText="購案編號" DataField="purch" />
                            <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                            <asp:BoundField HeaderText="申購單位" DataField="OVC_PUR_NSECTION" />
                            <asp:BoundField HeaderText="預算年度" DataField="OVC_YY" />
                            <asp:BoundField HeaderText="預算科目" DataField="OVC_POI_IBDG" />                   
                            <asp:BoundField HeaderText="預算科目名稱" DataField="OVC_PJNAME" />          
                            <asp:BoundField HeaderText="合約金額" DataField="ONB_MCONTRACT" />           
                            <asp:BoundField HeaderText="支用金額" DataField="ONB_PAY_MONEY" />           
                            <asp:BoundField HeaderText="簽約日期" DataField="OVC_DCONTRACT" />           
                            <asp:BoundField HeaderText="收辦日期" DataField="OVC_DRECEIVE" />            
                            <asp:BoundField HeaderText="契約交貨日期" DataField="OVC_DELIVERY_CONTRACT" /> 
                            <asp:BoundField HeaderText="實際交貨日期" DataField="OVC_DELIVERY" />        
                            <asp:BoundField HeaderText="實際交貨天數" DataField="ONB_DAYS_CONTRACT" />    
                            <asp:BoundField HeaderText="會驗日期" DataField="OVC_DJOINCHECK" />          
                            <asp:BoundField HeaderText="結案日期" DataField="OVC_DPAY" />                
                            <asp:BoundField HeaderText="預估結案日期" DataField="預估結案日" />          
                            <asp:BoundField HeaderText="使用天數" DataField="use_day" />                 
                            <asp:BoundField HeaderText="標準天數" DataField="ONB_DINSPECT_SOP" />        
                            <asp:BoundField HeaderText="延誤天數" DataField="ONB_DELAY_DAYS" />          
                            <asp:BoundField HeaderText="重要事項" DataField="OVC_RECEIVE_COMM" />        
                            <asp:BoundField HeaderText="逾期計罰" DataField="OnB_DELAY_DAYS" />          
                            <asp:BoundField HeaderText="減價收受" DataField="onb_mins_money_1" />        
                            <asp:BoundField HeaderText="下授" DataField="OVC_GRANT_TO" />                
                            <asp:BoundField HeaderText="承辦人" DataField="OVC_DO_NAME" />               
                            <asp:BoundField HeaderText="廠商名稱" DataField="OVC_VEN_TITLE" />           
                            <asp:BoundField HeaderText="是否歸檔" DataField="OVC_CLOSE" />               
                	    </Columns>
	   		        </asp:GridView>
                    <asp:GridView ID="GV_Q15" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Q15_RowCreated" OnRowDataBound="GV_Q15_RowDataBound" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="項次" DataField="" />
                            <asp:BoundField HeaderText="購案編號" DataField="purch" />
                            <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                            <asp:BoundField HeaderText="申購單位" DataField="OVC_PUR_NSECTION" />
                            <asp:BoundField HeaderText="合約金額" DataField="ONB_MONEY" />
                            <asp:BoundField HeaderText="批次" DataField="ONB_TIMES" />                   
                            <asp:BoundField HeaderText="付款日期" DataField="OVC_DPAY" />          
                            <asp:BoundField HeaderText="驗收金額" DataField="ONB_INSPECT_MONEY" />           
                            <asp:BoundField HeaderText="付款金額" DataField="ONB_ONB_PAY_MONEY" />           
                            <asp:BoundField HeaderText="逾罰金額" DataField="OnB_DELAY_MONEY" />           
                            <asp:BoundField HeaderText="減價金額" DataField="onb_mins_money_1" />            
                            <asp:BoundField HeaderText="承辦人" DataField="OVC_DO_NAME" /> 
                	    </Columns>
	   		        </asp:GridView>
                    <asp:GridView ID="GV_Q16" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Q16_RowCreated" OnRowDataBound="GV_Q16_RowDataBound" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="項次" DataField="" />
                            <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                            <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                            <asp:BoundField HeaderText="申購單位" DataField="OVC_PUR_NSECTION" />
                            <asp:BoundField HeaderText="預算年度" DataField="OVC_YY" />
                            <asp:BoundField HeaderText="預算科目" DataField="OVC_POI_IBDG" />                   
                            <asp:BoundField HeaderText="預算科目名稱" DataField="OVC_PJNAME" />       
                            <asp:BoundField HeaderText="採購屬性" DataField="OVC_LAB" />    
                            <asp:BoundField HeaderText="合約總金額" DataField="ONB_MCONTRACT" />           
                            <asp:BoundField HeaderText="支用總金額" DataField="ONB_PAY_MONEY" />           
                            <asp:BoundField HeaderText="簽約日期" DataField="OVC_DCONTRACT" />
                            <asp:BoundField HeaderText="第一年合約金額" DataField="ONB_MCONTRACT" />
                            <asp:BoundField HeaderText="第二年合約金額" DataField="" />
                            <asp:BoundField HeaderText="第三年合約金額" DataField="" />
                            <asp:BoundField HeaderText="第四年合約金額" DataField="" />
                            <asp:BoundField HeaderText="第五年合約金額" DataField="" />            
                            <asp:BoundField HeaderText="契約最後交貨日期" DataField="OVC_DELIVERY_CONTRACT" /> 
                            <asp:BoundField HeaderText="實際最後交貨日期" DataField="OVC_DELIVERY" />
                            <asp:BoundField HeaderText="第一批契約交貨日期" DataField="OVC_DELIVERY_CONTRACT" />
                            <asp:BoundField HeaderText="第一批實際交貨日期" DataField="OVC_DELIVERY" />
                            <asp:BoundField HeaderText="第一批會驗日期" DataField="OVC_DJOINCHECK" />
                            <asp:BoundField HeaderText="第一批支用金額" DataField="ONB_PAY_MONEY" />
                            <asp:BoundField HeaderText="第一批結報日期" DataField="OVC_DPAY" />
                            <asp:BoundField HeaderText="第二批契約交貨日期" DataField="" />
                            <asp:BoundField HeaderText="第二批實際交貨日期" DataField="" />
                            <asp:BoundField HeaderText="第二批會驗日期" DataField="" />
                            <asp:BoundField HeaderText="第二批支用金額" DataField="" />
                            <asp:BoundField HeaderText="第二批結報日期" DataField="" />
                            <asp:BoundField HeaderText="第三批契約交貨日期" DataField="" />
                            <asp:BoundField HeaderText="第三批實際交貨日期" DataField="" />
                            <asp:BoundField HeaderText="第三批會驗日期" DataField="" />
                            <asp:BoundField HeaderText="第三批支用金額" DataField="" />
                            <asp:BoundField HeaderText="第三批結報日期" DataField="" />
                            <asp:BoundField HeaderText="第四批契約交貨日期" DataField="" />
                            <asp:BoundField HeaderText="第四批實際交貨日期" DataField="" />
                            <asp:BoundField HeaderText="第四批會驗日期" DataField="" />
                            <asp:BoundField HeaderText="第四批支用金額" DataField="" />
                            <asp:BoundField HeaderText="第四批結報日期" DataField="" />
                            <asp:BoundField HeaderText="第五批契約交貨日期" DataField="" />
                            <asp:BoundField HeaderText="第五批實際交貨日期" DataField="" />
                            <asp:BoundField HeaderText="第五批會驗日期" DataField="" />
                            <asp:BoundField HeaderText="第五批支用金額" DataField="" />
                            <asp:BoundField HeaderText="第五批結報日期" DataField="" />
                            <asp:BoundField HeaderText="第六批契約交貨日期" DataField="" />
                            <asp:BoundField HeaderText="第六批實際交貨日期" DataField="" />
                            <asp:BoundField HeaderText="第六批會驗日期" DataField="" />
                            <asp:BoundField HeaderText="第六批支用金額" DataField="" />
                            <asp:BoundField HeaderText="第六批結報日期" DataField="" />
                            <asp:BoundField HeaderText="第七批契約交貨日期" DataField="" />
                            <asp:BoundField HeaderText="第七批實際交貨日期" DataField="" />
                            <asp:BoundField HeaderText="第七批會驗日期" DataField="" />
                            <asp:BoundField HeaderText="第七批支用金額" DataField="" />
                            <asp:BoundField HeaderText="第七批結報日期" DataField="" />
                            <asp:BoundField HeaderText="第八批契約交貨日期" DataField="" />
                            <asp:BoundField HeaderText="第八批實際交貨日期" DataField="" />
                            <asp:BoundField HeaderText="第八批會驗日期" DataField="" />
                            <asp:BoundField HeaderText="第八批支用金額" DataField="" />
                            <asp:BoundField HeaderText="第八批結報日期" DataField="" />                
                            <asp:BoundField HeaderText="預估結案日期" DataField="OVC_DAPPLY" />           
                            <asp:BoundField HeaderText="結案日期" DataField="OVC_DCLOSE" />          
                            <asp:BoundField HeaderText="履驗狀態" DataField="OVC_STATUS" />          
                            <asp:BoundField HeaderText="天數" DataField="" />
                            <asp:BoundField HeaderText="使用天數" DataField="use_day" />
                            <asp:BoundField HeaderText="標準天數" DataField="ONB_DINSPECT_SOP" />
                            <asp:BoundField HeaderText="延誤天數" DataField="ONB_DELAY_DAYS" />
                            <asp:BoundField HeaderText="重要事項" DataField="OVC_RECEIVE_COMM" />
                            <asp:BoundField HeaderText="逾期計罰" DataField="ONB_DELAY_MONEY" /> 
                            <asp:BoundField HeaderText="減價收受" DataField="ONB_MINS_MONEY" />
                            <asp:BoundField HeaderText="下授" DataField="OVC_GRANT_TO" />                
                            <asp:BoundField HeaderText="承辦人" DataField="OVC_DO_NAME" />               
                            <asp:BoundField HeaderText="廠商名稱" DataField="OVC_VEN_TITLE" />           
                            <asp:BoundField HeaderText="是否歸檔" DataField="OVC_CLOSE" />
                	    </Columns>
	   		        </asp:GridView>
                    <asp:GridView ID="GV_Q17" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Q17_RowCreated" OnRowDataBound="GV_Q17_RowDataBound" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="項次" DataField="" />
                            <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                            <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                            <asp:BoundField HeaderText="申購單位" DataField="unit" />
                            <asp:BoundField HeaderText="合約金額" DataField="money" />
                            <asp:BoundField HeaderText="種類" DataField="KIND" />
                            <asp:BoundField HeaderText="性質" DataField="PRO" />
                            <asp:BoundField HeaderText="單證編號" DataField="OVC_COMPTROLLER_NO" />
                            <asp:BoundField HeaderText="金額" DataField="ONB_ALL_MONEY" />
                            <asp:BoundField HeaderText="幣別" DataField="cur" />
                            <asp:BoundField HeaderText="廠商名稱" DataField="OVC_OWN_NAME" />
                            <asp:BoundField HeaderText="收繳單位" DataField="OVC_ONNAME" />
                            <asp:BoundField HeaderText="起始日" DataField="OVC_DGARRENT_START" />
                            <asp:BoundField HeaderText="到期日" DataField="OVC_DGARRENT_END" />
                            <asp:BoundField HeaderText="文件有效日" DataField="OVC_DEFFECT_1" />
                            <asp:BoundField HeaderText="退還日期" DataField="OVC_DBACK" />
                            <asp:BoundField HeaderText="承辦人" DataField="OVC_DO_NAME" />
                	    </Columns>
	   		        </asp:GridView>
                    <asp:GridView ID="GV_Q18" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Q18_RowCreated" OnRowDataBound="GV_Q18_RowDataBound" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="項次" DataField="" />
                            <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                            <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                            <asp:BoundField HeaderText="申購單位" DataField="OVC_PUR_NSECTION" /> 
                            <asp:BoundField HeaderText="合約金額" DataField="ONB_MONEY" />    
                            <asp:BoundField HeaderText="種類" DataField="KIND" />
                            <asp:BoundField HeaderText="性質" DataField="PRO" />
                            <asp:BoundField HeaderText="單證編號" DataField="OVC_COMPTROLLER_NO" />
                            <asp:BoundField HeaderText="金額" DataField="ONB_ALL_MONEY" />
                            <asp:BoundField HeaderText="幣別" DataField="cur" />
                            <asp:BoundField HeaderText="廠商名稱" DataField="OVC_OWN_NAME" />
                            <asp:BoundField HeaderText="收繳單位" DataField="OVC_ONNAME" />
                            <asp:BoundField HeaderText="起始日" DataField="OVC_DGARRENT_START" />
                            <asp:BoundField HeaderText="到期日" DataField="OVC_DGARRENT_END" />
                            <asp:BoundField HeaderText="文件有效日" DataField="OVC_DEFFECT_1" />
                            <asp:BoundField HeaderText="退還日" DataField="OVC_DBACK" />
                            <asp:BoundField HeaderText="承辦人" DataField="OVC_DO_NAME" />
                	    </Columns>
	   		        </asp:GridView>
                    <asp:GridView ID="GV_Q19" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Q19_RowCreated" OnRowDataBound="GV_Q19_RowDataBound" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="項次" DataField="" />
                            <asp:BoundField HeaderText="購案編號" DataField="OVC_PURCH" />
                            <asp:BoundField HeaderText="組別" DataField="ONB_GROUP" />
                            <asp:BoundField HeaderText="購案名稱" DataField="OVC_PUR_IPURCH" />
                            <asp:BoundField HeaderText="申購單位" DataField="OVC_PUR_NSECTION" />
                            <asp:BoundField HeaderText="預算年度" DataField="OVC_YY" />
                            <asp:BoundField HeaderText="預算科目" DataField="OVC_POI_IBDG" />
                            <asp:BoundField HeaderText="預算科目名稱" DataField="OVC_PJNAME" />
                            <asp:BoundField HeaderText="採購屬性" DataField="OVC_LAB" />
                            <asp:BoundField HeaderText="合約金額" DataField="ONB_MONEY" />
                            <asp:BoundField HeaderText="支用金額" DataField="ONB_PAY_MONEY" />
                            <asp:BoundField HeaderText="簽約日期" DataField="OVC_DCONTRACT" />
                            <asp:BoundField HeaderText="契約交貨日期" DataField="OVC_DELIVERY_CONTRACT" />
                            <asp:BoundField HeaderText="實際交貨日期" DataField="OVC_DELIVERY" />
                            <asp:BoundField HeaderText="實際交貨天數" DataField="ONB_DAYS_CONTRACT" />
                            <asp:BoundField HeaderText="會驗日期" DataField="OVC_DJOINCHECK" />
                            <asp:BoundField HeaderText="結案日期" DataField="OVC_DCLOSE" />
                            <asp:BoundField HeaderText="履驗狀態" DataField="OVC_STATUS" />
                            <asp:BoundField HeaderText="預估結案日期" DataField="OVC_DAPPLY" />
                            <asp:BoundField HeaderText="重要事項" DataField="OVC_RECEIVE_COMM" />
                            <asp:BoundField HeaderText="逾期計罰" DataField="ONB_DELAY_MONEY" />
                            <asp:BoundField HeaderText="減價收受" DataField="ONB_MINS_MONEY" />
                            <asp:BoundField HeaderText="下授" DataField="OVC_GRANT_TO" />
                            <asp:BoundField HeaderText="承辦人" DataField="OVC_DO_NAME" />
                            <asp:BoundField HeaderText="廠商名稱" DataField="OVC_VEN_TITLE" />
                            <asp:BoundField HeaderText="是否歸檔" DataField="OVC_CLOSE" />
                	    </Columns>
	   		        </asp:GridView>
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
