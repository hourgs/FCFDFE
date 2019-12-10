<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E27.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E27" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    交貨暨驗收情形
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-left">
                                <tr>
                                    <th colspan="2" style="background: red;">
                                        <asp:Label ForeColor="#ffff37" Font-Size="X-Large" CssClass="control-label" runat="server">請注意！未輸入資料或存檔無法列印各項報表！！</asp:Label></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">購案編號：</asp:Label><asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">交貨批次：</asp:Label><asp:TextBox ID="txtONB_SHIP_TIMES" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">購案名稱：</asp:Label><asp:TextBox ID="txtOVC_PURCH" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">申購單位：</asp:Label>
                                        <%--<asp:TextBox ID="txtOVC_PUR_AGENCY" CssClass="tb tb-l" runat="server"></asp:TextBox>--%>
                                        <asp:TextBox id="txtOVC_DEPT_CDE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <input type="button" value="單位查詢" class="btn-success" onclick="OpenWindow('txtOVC_DEPT_CDE', 'txtOVC_ONNAME')" />
                                        <asp:TextBox id="txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">廠商名稱：</asp:Label><asp:TextBox ID="txtOVC_VEN_TITLE" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label position-left" runat="server">簽約日期：</asp:Label>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                        <asp:TextBox ID="txtOVC_DCONTRACT" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Button ID="Button2" CssClass="btn-default btnw4" runat="server" Text="清除日期" />
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">決標日期：</asp:Label><asp:TextBox ID="txtOVC_DBID" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">清單所列交貨日期：</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">詳如契約附加條款第一條：</asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label position-left" runat="server">契約交貨日期：</asp:Label><!--前方標籤文字，跟日期同一行需使用"position-left"之class-->
                                                <!--↓日期套件↓-->
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                    <asp:TextBox ID="txtOVC_DAUDIT" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                    <div class="add-on"><i class="icon-calendar"></i></div>
                                                </div>
                                                <asp:Button ID="btnClear" CssClass="btn-default btnw4" OnClick="btnClear_Click" runat="server" Text="清除日期" />
                                                <asp:Label CssClass="control-label" runat="server">或(自行輸入)</asp:Label>
                                                <asp:TextBox ID="txtOVC_DAUDIT_Input" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">契約交貨天數(自簽約次日起計算)：</asp:Label>
                                        <asp:TextBox ID="txtONB_DAYS_CONTRACT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">天</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel UpdateMode="Conditional" style="display:inline" runat="server">
                                            <ContentTemplate>
                                                <asp:Label CssClass="control-label position-left" runat="server">實際交貨日期：</asp:Label><!--前方標籤文字，跟日期同一行需使用"position-left"之class-->
                                                <!--↓日期套件↓-->
                                                <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                    <asp:TextBox ID="txtOVC_DELIVERY" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                    <div class="add-on"><i class="icon-calendar"></i></div>
                                                </div>
                                                <asp:Button ID="btnC_OVC_DELIVERY" CssClass="btn-default btnw4" OnClick="btnC_OVC_DELIVERY_Click" runat="server" Text="清除日期" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <%--<asp:Button ID="btnChange_OVC_DELIVERY" CssClass="btn-default" OnClick="btnChange_OVC_DELIVERY_Click" runat="server" Text="日期變更後請按我" />--%>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">契約金額：</asp:Label><asp:TextBox ID="txtONB_MCONTRACT" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">本次交貨金額：</asp:Label><asp:TextBox ID="txtONB_MDELIVERY" CssClass="tb tb-l" runat="server"></asp:TextBox><br />
                                        <asp:Label CssClass="control-label text-red" runat="server">除了交貨項目為【詳如契約清單】外系統會自動由輸入交貨項目帶入：</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">交貨地點：</asp:Label><asp:TextBox ID="txtOVC_DELIVERY_PLACE" TextMode="MultiLine" Rows="3" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">標準驗收天數：</asp:Label><asp:DropDownList ID="drpONB_DINSPECT_SOP" CssClass="tb tb-l" runat="server">
                                            
                                        </asp:DropDownList>
                                        <asp:Label CssClass="control-label text-red" runat="server">請先輸入實際交貨日期<br />(有實際交貨日期及標準驗收天數後才有預劃時程)</asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <th style="width: 15%">
                                        <asp:Label CssClass="control-label" runat="server"></asp:Label></th>
                                    <th style="width: 30%">
                                        <asp:Label CssClass="control-label" runat="server">實際</asp:Label></th>
                                    <th style="width: 30%">
                                        <asp:Label CssClass="control-label" runat="server">預劃</asp:Label></th>
                                    <th style="width: 25%">
                                        <asp:Label CssClass="control-label" runat="server">超前或落後天數</asp:Label></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">海(空)運日期</asp:Label>
                                    </td>
                                    <td>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DAIRSHIP" CssClass="tb tb-s position-left" OnTextChanged="txtOVC_DAIRSHIP_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Button ID="btnClear_OVC_DAIRSHIP" CssClass="btn-default btnw4" OnClick="btnClear_OVC_DAIRSHIP_Click" runat="server" Text="清除日期" />
                                    </td>
                                    <td>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DAIRSHIP_PLAN" CssClass="tb tb-s position-left" OnTextChanged="txtOVC_DAIRSHIP_PLAN_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Button ID="btnClear_OVC_DAIRSHIP_PLAN" CssClass="btn-default btnw4" OnClick="btnClear_OVC_DAIRSHIP_PLAN_Click" runat="server" Text="清除日期" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">報驗日期</asp:Label></td>
                                    <td>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DINFORM" CssClass="tb tb-s position-left" OnTextChanged="txtOVC_DINFORM_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Button ID="btnClear_OVC_DINFORM" CssClass="btn-default btnw4" OnClick="btnClear_OVC_DINFORM_Click" runat="server" Text="清除日期" />
                                    </td>
                                    <td>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DINFORM_PLAN" CssClass="tb tb-s position-left" OnTextChanged="txtOVC_DINFORM_PLAN_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Button ID="btnClear_OVC_DINFORM_PLAN" CssClass="btn-default btnw4" OnClick="btnClear_OVC_DINFORM_PLAN_Click" runat="server" Text="清除日期" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_NO_PUNISH_DAYS_OVC_DINFORM" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">會驗日期</asp:Label></td>
                                    <td>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DJOINCHECK" CssClass="tb tb-s position-left" OnTextChanged="txtOVC_DJOINCHECK_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Button ID="btnCLear_" CssClass="btn-default btnw4" OnClick="btnCLear__Click" runat="server" Text="清除日期" />
                                    </td>
                                    <td>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DJOINCHECK_PLAN" CssClass="tb tb-s position-left" OnTextChanged="txtOVC_DJOINCHECK_PLAN_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Button ID="btnClear_OVC_DJOINCHECK_PLAN" CssClass="btn-default btnw4" OnClick="btnClear_OVC_DJOINCHECK_PLAN_Click" runat="server" Text="清除日期" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_NO_PUNISH_DAYS_OVC_DJOINCHECK" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">安裝交運日期</asp:Label></td>
                                    <td>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DSHIPMENT" CssClass="tb tb-s position-left" OnTextChanged="txtOVC_DSHIPMENT_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Button ID="btnClear_OVC_DSHIPMENT" CssClass="btn-default btnw4" OnClick="btnClear_OVC_DSHIPMENT_Click" runat="server" Text="清除日期" />
                                    </td>
                                    <td>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DSHIPMENT_PLAN" CssClass="tb tb-s position-left" OnTextChanged="txtOVC_DSHIPMENT_PLAN_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Button ID="btnClear_OVC_DSHIPMENT_PLAN" CssClass="btn-default btnw4" OnClick="btnClear_OVC_DSHIPMENT_PLAN_Click" runat="server" Text="清除日期" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_NO_PUNISH_DAYS_OVC_DSHIPMENT" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">細數清點日期</asp:Label></td>
                                    <td>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DINVENTORY" CssClass="tb tb-s position-left" OnTextChanged="txtOVC_DINVENTORY_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Button ID="btnClear_OVC_DINVENTORY" CssClass="btn-default btnw4" OnClick="btnClear_OVC_DINVENTORY_Click" runat="server" Text="清除日期" />
                                    </td>
                                    <td>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DINVENTORY_PLAN" CssClass="tb tb-s position-left" OnTextChanged="txtOVC_DINVENTORY_PLAN_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Button ID="btnClear_OVC_DINVENTORY_PLAN" CssClass="btn-default btnw4" OnClick="btnClear_OVC_DINVENTORY_PLAN_Click" runat="server" Text="清除日期" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_NO_PUNISH_DAYS_OVC_DINVENTORY" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">檢測日期</asp:Label></td>
                                    <td>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DINSPECT" CssClass="tb tb-s position-left" OnTextChanged="txtOVC_DINSPECT_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Button ID="btnClear_OVC_DINSPECT" CssClass="btn-default btnw4" OnClick="btnClear_OVC_DINSPECT_Click" runat="server" Text="清除日期" />
                                    </td>
                                    <td>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DINSPECT_PLAN" CssClass="tb tb-s position-left" OnTextChanged="txtOVC_DINSPECT_PLAN_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Button ID="btnClear_OVC_DINSPECT_PLAN" CssClass="btn-default btnw4" OnClick="btnClear_OVC_DINSPECT_PLAN_Click" runat="server" Text="清除日期" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_NO_PUNISH_DAYS_OVC_DINSPECT" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">結報日期</asp:Label></td>
                                    <td>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DPAY" CssClass="tb tb-s position-left" OnTextChanged="txtOVC_DPAY_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Button ID="btnClear_OVC_DPAY" CssClass="btn-default btnw4" OnClick="btnClear_OVC_DPAY_Click" runat="server" Text="清除日期" />
                                    </td>
                                    <td>
                                        <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                            <asp:TextBox ID="txtOVC_DPAY_PLAN" CssClass="tb tb-s position-left" OnTextChanged="txtOVC_DPAY_PLAN_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <asp:Button ID="btnClear_OVC_DPAY_PLAN" CssClass="btn-default btnw4" OnClick="btnClear_OVC_DPAY_PLAN_Click" runat="server" Text="清除日期" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_NO_PUNISH_DAYS_OVC_DPAY" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">主要事項記載</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label CssClass="control-label text-red" runat="server">本記載將顯示於「交貨驗收情形」及「辦理狀況」</asp:Label><br />
                                        <asp:TextBox ID="txtOVC_RECEIVE_COMM" TextMode="MultiLine" Rows="3" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">會同履驗依據</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_ACCORDING" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">會同履驗時間</asp:Label></td>
                                    <td>
                                        <asp:UpdatePanel UpdateMode="Conditional" style="display:inline" runat="server">
                                            <ContentTemplate>
                                                <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                    <asp:TextBox ID="txtOVC_DACCORDING" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                    <div class="add-on"><i class="icon-calendar"></i></div>
                                                </div>
                                                <asp:Button ID="btnClear_OVC_DACCORDING" CssClass="btn-default btnw4" OnClick="btnClear_OVC_DACCORDING_Click" runat="server" Text="清除日期" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">會同履驗地點</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_ACCORDING_PLACE" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="btnReturn" CssClass="btn-default btnw4" OnClick="btnReturn_Click" runat="server" Text="回上一頁" />
                                                <asp:Button ID="btnReturnM" CssClass="btn-default btnw4" OnClick="btnReturnM_Click" runat="server" Text="回主流程" />
                                                <asp:Button ID="btnSave" CssClass="btn-default btnw2" OnClick="btnSave_Click" runat="server" Text="存檔" /></td>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnSave" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        <asp:Button ID="btnIp" CssClass="btn-default" OnClick="btnIp_Click" runat="server" Text="輸入交貨項目" /></td>
                                    <td colspan="3">
                                        <asp:Button ID="btnPrint_Detail" CssClass="btn-default" OnClick="btnPrint_Detail_Click" runat="server" Text="列印軍品交貨暨驗收配合注意事項.doc" />
                                        <asp:Button ID="btnPrint_Detail_pdf" CssClass="btn-default" OnClick="btnPrint_Detail_pdf_Click" runat="server" Text=".pdf" />
                                        <asp:Button ID="btnPrint_Detail_odt" CssClass="btn-default" OnClick="btnPrint_Detail_odt_Click" runat="server" Text=".odt" />
                                        <br /><br />
                                        <asp:Button ID="btnPrint_Result" CssClass="btn-default" OnClick="btnPrint_Result_Click" runat="server" Text="列印採購接收暨會驗結果報告單.doc" />
                                        <asp:Button ID="btnPrint_Result_pdf" CssClass="btn-default" OnClick="btnPrint_Result_pdf_Click" runat="server" Text=".pdf" />
                                        <asp:Button ID="btnPrint_Result_odt" CssClass="btn-default" OnClick="btnPrint_Result_odt_Click" runat="server" Text=".odt" />
                                    </td>
                                </tr>
                            </table>
                                    <asp:GridView ID="GV_Situation" CssClass=" table data-table table-striped border-top text-center" OnRowCreated="GV_Situation_RowCreated" OnRowDataBound="GV_Situation_RowDataBound" runat="server" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField HeaderText="動作" >
                                                <ItemTemplate>
                                                    <div class="control-group">
                                                        <asp:Button ID="btnCha" CssClass="btn-success" Text="修改" OnClick="btnCha_Click" Visible="true" runat="server" />
                                                    </div>
                                                    <div class="control-group">
                                                        <asp:Button ID="btnDel" CssClass="btn-danger" Text="刪除" OnClick="btnDel_Click" OnClientClick="if (confirm('確定要刪除資料?') == false) return false;" Visible="true" runat="server" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="複驗次數" DataField="ONB_INSPECT_TIMES" />
                                            <asp:BoundField HeaderText="再驗次數" DataField="ONB_RE_INSPECT_TIMES" />
                                            <asp:BoundField HeaderText="軍品名稱及數量" DataField="OVC_REPORT_DESC" />
                                            <asp:BoundField HeaderText="驗收日期" DataField="OVC_DINSPECT" />
                                            <asp:BoundField HeaderText="驗收情形摘要" DataField="OVC_RESULT_5" />
                                            <asp:TemplateField HeaderText="擬辦" >
                                                <ItemTemplate>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labRESULT1_1" Text="一、交驗軍品數量" runat="server"></asp:Label>
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labRESULT2_1" Text="二、抽驗" runat="server"></asp:Label>
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labRESULT3_1" Text="三、包裝情形" runat="server"></asp:Label>
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labRESULT4_1" Text="四、交貨時間" runat="server"></asp:Label>
                                                    </p>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                	                    </Columns>
	   		                        </asp:GridView>
                            <table id="tbSituation" class="table table-bordered text-center" visible="false" runat="server">
                                <tr>
                                    <th style="letter-spacing: 20px;" colspan="7"><asp:Label CssClass="control-label" Font-Size="X-Large" Font-Bold="true" runat="server">驗收情形</asp:Label></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">動作</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">複驗次數</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">再驗次數</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">軍品名稱及數量</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">驗收日期</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">驗收情形摘要</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">擬辦</asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnNew" CssClass="btnw2 text-red" OnClick="btnNew_Click" runat="server" Text="新增" /></td>
                                    <td>
                                        <asp:Label ID="lblONB_INSPECT_TIMES" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblONB_RE_INSPECT_TIMES" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblOVC_REPORT_DESC" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblOVC_DINSPECT" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblONB_ITEM" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblOVC_ITEM_NAME" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                            
                                    <asp:GridView ID="GV_Acceptance" CssClass=" table data-table table-striped border-top text-center" OnRowDataBound="GV_Acceptance_RowDataBound" OnRowCreated="GV_Acceptance_RowCreated" runat="server" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField HeaderText="動作" >
                                                <ItemTemplate>
                                                    <div class="control-group">
                                                        <asp:Button ID="btnCha" CssClass="btn-success" Text="修改" OnClick="btnCha_Click1" Visible="true" runat="server" />
                                                    </div>
                                                    <div class="control-group">
                                                        <asp:Button ID="btnDel_1" CssClass="btn-danger" Text="刪除" OnClick="btnDel_1_Click" OnClientClick="if (confirm('確定要刪除資料?') == false) return false;" Visible="true" runat="server" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="複驗次數" DataField="ONB_INSPECT_TIMES" />
                                            <asp:BoundField HeaderText="再驗次數" DataField="ONB_RE_INSPECT_TIMES" />
                                            <asp:BoundField HeaderText="交貨日期" DataField="OVC_DELIVERY_CONTRACT" />
                                            <asp:BoundField HeaderText="交貨地點" DataField="OVC_DELIVERY_PLACE" />
                                            <asp:TemplateField HeaderText="會驗結果" ControlStyle-Width="400px" >
                                                <ItemTemplate>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labRESULT_T1" Text="一、品名數量：" runat="server"></asp:Label>
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labRESULT_1" runat="server"></asp:Label>
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labRESULT_T2" Text="二、抽驗情形：" runat="server"></asp:Label>
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labRESULT_2" runat="server"></asp:Label>
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labRESULT_T3" Text="三、包裝情形：" runat="server"></asp:Label>
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labRESULT_3" runat="server"></asp:Label>
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labRESULT_T4" Text="四、逾期天數及罰款：" runat="server"></asp:Label>
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labRESULT_4" runat="server"></asp:Label>
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labRESULT_T5" Text="五、封樣及檢驗單位：" runat="server"></asp:Label>
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labRESULT_5" runat="server"></asp:Label>
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labRESULT_T6" Text="六、其他：" runat="server"></asp:Label>
                                                    </p>
                                                    <p style="TEXT-ALIGN:left;" >
                                                        <asp:Label ID="labRESULT_6" runat="server"></asp:Label>
                                                    </p>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                	                    </Columns>
	   		                        </asp:GridView>
                             <table id="tbAcceptance" class="table table-bordered text-center" visible="false" runat="server">
                                <tr>
                                    <th style="letter-spacing: 20px;" colspan="6"><asp:Label CssClass="control-label" Font-Size="X-Large" Font-Bold="true" runat="server">驗收紀錄</asp:Label></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">動作</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">複驗次數</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">再驗次數</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">交貨日期</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">交貨地點</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">會驗結果</asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="Button1" CssClass=" btnw2 text-red" OnClick="Button1_Click" runat="server" Text="新增" /></td>
                                    <td>
                                        <asp:Label ID="lblRONB_INSPECT_TIMES" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblRONB_RE_INSPECT_TIMES" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblOVC_DELIVERY_CONTRACT" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblOVC_DELIVERY_PLACE" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblOVC_ADVICE" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                            </table>

                            <div class="text-center">
                                <asp:Button ID="btnR" CssClass="btn-default btnw4" OnClick="btnR_Click" runat="server" Text="回上一頁" />
                                <asp:Button ID="btnRM" CssClass="btn-default btnw4" OnClick="btnRM_Click" runat="server" Text="回主流程" />
                            </div>
                            <footer class="panel-footer" style="text-align: center;">
                                <!--網頁尾-->
                            </footer>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
