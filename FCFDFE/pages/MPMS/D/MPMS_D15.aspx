<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D15.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D15" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .rdoOVC_RESULT_Item1 input[type=radio]:checked + label {
            color:blue;
        }
        .rdoOVC_RESULT_Item2 input[type=radio]:checked + label {
            color:red;
        }
        .rdoOVC_RESULT_Item3 input[type=radio]:checked + label {
            color:green;
        }
    </style>

    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->計畫清單移轉檢查項目表編輯
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;" id="divForm" visible="false" runat="server">
                    <div class="form" style="border: 5px;" >
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="Width:40%"><asp:Label CssClass="control-label" Text="購案編號" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <asp:Label ID="lblOVC_PURCH_A_5" CssClass="control-label" runat="server"></asp:Label>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" Text="購案名稱" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" Text="申購單位(代碼)-申購人(電話)" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <asp:Label ID="lblOVC_PUR_NSECTION" CssClass="control-label" runat="server"></asp:Label>(
                                        <asp:Label ID="lblOVC_PUR_SECTION" CssClass="control-label" runat="server"></asp:Label>)-
                                        <asp:Label ID="lblOVC_PUR_USER" CssClass="control-label" runat="server"></asp:Label>(
                                        <asp:Label ID="lblOVC_PUR_IUSER_PHONE" CssClass="control-label" runat="server"></asp:Label>&nbsp;&nbsp;
                                        軍線：<asp:Label ID="lblOVC_PUR_IUSER_PHONE_EXT" CssClass="control-label" runat="server"></asp:Label>)</td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" Text="採購屬性" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <asp:Label ID="lblOVC_LAB" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" Text="招標方式" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <asp:Label ID="lblOVC_PUR_ASS_VEN_CODE" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" Text="投標段次" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <asp:Label ID="lblOVC_BID_TIMES" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" Text="決標原則" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <asp:Label ID="lblOVC_BID" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" Text="指派承辦人"  runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <asp:Label ID="lblOVC_DO_NAME" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" Text="核評移案日" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <asp:Label ID="lblOVC_DBEGIN" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" Text="收辦日" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <asp:Label ID="lblOVC_DRECEIVE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>



                            <table class="table table-bordered text-center">
                                <tr>
                                    <td rowspan="5" style="Width:5%" class="td-vertical"><asp:Label CssClass="control-label text-vertical-m" style="height: 280px;" runat="server">登管人員檢查項目</asp:Label></td>
                                    <td style="Width:10%"><asp:Label CssClass="control-label" style="Width:50px" runat="server">項次</asp:Label></td>
                                    <td style="Width:60%"><asp:Label CssClass="control-label" runat="server"></asp:Label>登管人員檢查項目</td>
                                    <td style="Width:25%"><asp:Label CssClass="control-label" runat="server">是否</asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="Width:10%">
                                        <asp:Label ID="lblONB_ITEM_1" CssClass="control-label" runat="server">1</asp:Label></td>
                                    <td class="text-left"  style="Width:60%">
                                        <asp:Label ID="lblOVC_ITEM_NAME_1" CssClass="control-label" runat="server">核對購案號是否正確?</asp:Label></td>
                                    <td  style="Width:25%">
                                        <asp:Label ID="lblOVC_RESULT_1" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:DropDownList ID="drpOVC_RESULT_1" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                 <tr>
                                    <td><asp:Label ID="lblONB_ITEM_2" CssClass="control-label" runat="server">2</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_ITEM_NAME_2" CssClass="control-label" runat="server">核定書及清單、品名、金額是否相符?</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblOVC_RESULT_2" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:DropDownList ID="drpOVC_RESULT_2" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label ID="lblONB_ITEM_3" CssClass="control-label" runat="server" Text="3"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_ITEM_NAME_3" CssClass="control-label" runat="server">是否已登入本中心購案時程管制資訊系統?</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblOVC_RESULT_3" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:DropDownList ID="drpOVC_RESULT_3" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label ID="lblONB_ITEM_4" CssClass="control-label" runat="server">4</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label" runat="server">其他：</asp:Label>
                                        <asp:Label ID="lblOVC_ITEM_NAME_4" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtOVC_ITEM_NAME_4" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOVC_RESULT_4" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:DropDownList ID="drpOVC_RESULT_4" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Label CssClass="control-label text-red" runat="server">綜辦<br />意見</asp:Label></td>
                                    <td colspan="2" class="text-left"><asp:Label ID="lblOVC_NAME_RESULT" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="4"><asp:Label CssClass="control-label text-red" runat="server">主官批核日</asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="lblOVC_DAPPROVE" CssClass="control-label text-red" runat="server"></asp:Label></td>
                                </tr>
                            </table><br />



                            <table class="table table-bordered text-center">
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">購案承辦人檢查項目</asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_ITEM_NAME" CssClass="tb tb-full" OnSelectedIndexChanged="drpOVC_ITEM_NAME_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                            <asp:ListItem Value="" Text="請選擇"></asp:ListItem>
                                            <asp:ListItem>案屬時效急迫性購案是否需優先辦理採購?</asp:ListItem>
                                            <asp:ListItem>年度預算是否奉核?如未奉核，請求先行開標條文是否明確?</asp:ListItem>
                                            <asp:ListItem>核定書及清單單價、總價是否明確?</asp:ListItem>
                                            <asp:ListItem>附件、附表或藍圖、樣品、契約草案是否移交?</asp:ListItem>
                                            <asp:ListItem>核定採購之招標方式是否違法?</asp:ListItem>
                                            <asp:ListItem>核定清單內容與電子檔是否相符?</asp:ListItem>
                                            <asp:ListItem>廠商資格訂定是否有不當限制牴觸法令之虞?</asp:ListItem>
                                            <asp:ListItem>清單備註欄一般要求事項是否有語意不詳或謄寫錯誤情形?</asp:ListItem>
                                            <asp:ListItem>同等品審查標準詳實訂定?</asp:ListItem>
                                            <asp:ListItem>最有利標決標之審查標準是否詳實訂定?</asp:ListItem>
                                            <asp:ListItem>免繳押標金及履保金條件是否合法?</asp:ListItem>
                                            <asp:ListItem>核定採購之決標方式是否合宜?</asp:ListItem>
                                            <asp:ListItem>交貨暨招標作業所需三次時間年度內是否能完成訂約?</asp:ListItem>
                                            <asp:ListItem>後續擴充購案是否敘明項目、內容、金額、數量或期間上限?</asp:ListItem>
                                            <asp:ListItem>其他：</asp:ListItem>
                                        </asp:DropDownList><br /><br />
                                        <asp:TextBox ID="txtOVC_ITEM_NAME" CssClass="textarea tb-full" TextMode="MultiLine" Rows="6" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="Width:7%">
                                        <asp:Label CssClass="control-label text-red" Text="綜辦" runat="server"></asp:Label><br>
                                        <asp:Label CssClass="control-label text-red" Text="意見" runat="server"></asp:Label>
                                    </td>
                                    <td class="text-left" style="Width:93%">
                                        <asp:RadioButtonList ID="rdoOVC_DO_NAME_RESULT" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server"></asp:RadioButtonList></td>
                                </tr>
                            </table>

                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:50%;border-right:none" class="text-right">
                                        <asp:Label CssClass="control-label text-red" runat="server">收辦主官核批日：</asp:Label></td>
                                    <td style="border-left: none;">
                                        <div class="input-append datepicker position-left" style="border-left-style:none;border-left:none"">
                                            <asp:TextBox ID="txtOVC_DO_DAPPROVE" CssClass="tb tb-s position-left text-change" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div></td>
                                </tr>
                            </table>


                            <div style="text-align:center"> 
                                <asp:Button ID="btnSave" cssclass="btn-default btnw2" Text="存檔" OnClick="btnSave_Click" runat="server" />&nbsp;
                               <asp:Button ID="btnWork" cssclass="btn-default" Text="招標作業" Visible="false" OnClick="btnWork_Click" runat="server" />&nbsp;
                                <asp:Button ID="btnBack" CssClass="btn-default" OnClick="btnBack_Click" Text="回上一頁" runat="server"/>
                            </div>

                            <div class="subtitle">委案單位已上傳檔案</div>
                            <asp:GridView ID="GV_C_Alreadyupdate" CssClass=" table data-table table-striped border-top " OnRowCommand="GV_C_Alreadyupdate_RowCommand" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="主件名稱" DataField="OVC_IKIND" />
                                    <asp:BoundField HeaderText="附件名稱" DataField="OVC_ATTACH_NAME" />
                                    <asp:BoundField HeaderText="份數" DataField="ONB_QTY" />
                                    <%--<asp:BoundField HeaderText="相對之上傳檔案" DataField="OVC_FILE_NAME" />--%>
                                    <asp:TemplateField HeaderText="相對之上傳檔案">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnDownloadFile" Text='<%# Eval("OVC_FILE_NAME")%>' CommandName="DownloadFile" runat="server"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="頁數" DataField="ONB_PAGES" />
                	            </Columns>
	   		               </asp:GridView>
                            <div class="subtitle">購案審查上傳檔案</div>
                            <asp:GridView ID="GV_OVC_Alreadyupdate" CssClass=" table data-table table-striped border-top " AutoGenerateColumns="false"  runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="選擇" DataField="" />
                                    <asp:BoundField HeaderText="上傳之檔案名稱" DataField="FileName" />
                                    <asp:BoundField HeaderText="時間" DataField="Time" />
                                    <asp:BoundField HeaderText="檔案大小" DataField="Size" />
                	            </Columns>
	   		               </asp:GridView>

                            
                            <div class="subtitle" visible="false" runat="server">採購發包階段上傳檔案 </div>
                            <asp:GridView ID="gvFiles" Visible="false" CssClass="table data-table table-striped border-top text-center" Width="100%" AutoGenerateColumns="False" runat="server" >
 		     		            <Columns>
                                    <asp:HyperLinkField DataTextField="FileName" HeaderText="上傳之檔案名稱" />
                                    <asp:BoundField DataField="Time" HeaderText="時間" />     
                                      <asp:TemplateField HeaderText="檔案大小">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("FileSize") %>' runat="server"></asp:Label> KB
                                        </ItemTemplate>
                                    </asp:TemplateField>
                	            </Columns>
	   		                </asp:GridView><br />

                            <table class="table table-bordered text-center" visible="false" runat="server">
                                <tr>
                                    <td><asp:Button ID="btnUpload" CssClass="btn-default btnw2" Text="上傳" OnClick="btnUpload_Click" runat="server"/></td>
                                    <td style="width:80%">
                                        <asp:FileUpload ID="FileUpload" title="瀏覽..." runat="server" /></td>
                                </tr>
                            </table>

                            <div>
                                <div class="subtitle"> 預覽列印 </div>
                                <div style="text-align:center"> 
                                    <asp:Button ID="btnBook" Visible="false" CssClass="btn-default" Text="物資核定書PDF" OnClick="btnBook_Click" runat="server"/>
                                    <asp:Button ID="btnWordP" Visible="false" CssClass="btn-default" Text="標單(WORD有價款)" OnClick="btnWordP_Click" runat="server"/>
                                    <asp:Button ID="btnWord" Visible="false" CssClass="btn-default" Text="標單(WORD無價款)" OnClick="btnWord_Click" runat="server"/>
                                    <asp:Button ID="btnPdfP" Visible="false" CssClass="btn-default" Text="標單(pdf有價款)" OnClick="btnPdfP_Click" runat="server"/>
                                    <asp:Button ID="btnPdf" Visible="false" CssClass="btn-default" Text="標單(pdf無價款)" OnClick="btnPdf_Click" runat="server"/>
                                    <asp:Button ID="btnCheck" Visible="false" CssClass="btn-default" Text="檢核表" OnClick="btnCheck_Click" runat="server"/>

                                    <table>
                                        <tr>
                                            <td style="width:150px"><asp:LinkButton ID="lbtnBook" OnClick="btnBook_Click" runat="server">物資核定書.doc</asp:LinkButton></td>
                                            <td style="width:150px"><asp:LinkButton ID="lbtnWordP" OnClick="btnWordP_Click" runat="server">標單(有價款).doc</asp:LinkButton></td>
                                            <td style="width:150px"><asp:LinkButton ID="lbtnWord" OnClick="btnWord_Click" runat="server">標單(無價款).doc</asp:LinkButton></td>
                                            <td style="width:150px"><asp:LinkButton ID="lbtnCheck" OnClick="btnCheck_Click" runat="server">檢核表.doc</asp:LinkButton></td>
                                        </tr>
                                        <tr>
                                            <td style="width:150px"><asp:LinkButton ID="lbtnBook_pdf" OnClick="lbtnBook_pdf_Click" runat="server">物資核定書.pdf</asp:LinkButton></td>
                                            <td style="width:150px"><asp:LinkButton ID="lbtnWordP_pdf" OnClick="lbtnWordP_pdf_Click" runat="server">標單(有價款).pdf</asp:LinkButton></td>
                                            <td style="width:150px"><asp:LinkButton ID="lbtnWord_pdf" OnClick="lbtnWord_pdf_Click" runat="server">標單(無價款).pdf</asp:LinkButton></td>
                                            <td style="width:150px"><asp:LinkButton ID="lbtnCheck_pdf" OnClick="lbtnCheck_pdf_Click" runat="server">檢核表.pdf</asp:LinkButton></td>
                                        </tr>
                                        <tr>
                                            <td style="width:150px"><asp:LinkButton ID="lbtnBook_odt" OnClick="lbtnBook_odt_Click" runat="server">物資核定書.odt</asp:LinkButton></td>
                                            <td style="width:150px"><asp:LinkButton ID="lbtnWordP_odt" OnClick="lbtnWordP_odt_Click" runat="server">標單(有價款).odt</asp:LinkButton></td>
                                            <td style="width:150px"><asp:LinkButton ID="lbtnWord_odt" OnClick="lbtnWord_odt_Click" runat="server">標單(無價款).odt</asp:LinkButton></td>
                                            <td style="width:150px"><asp:LinkButton ID="lbtnCheck_odt" OnClick="lbtnCheck_odt_Click" runat="server">檢核表.odt</asp:LinkButton></td>
                                        </tr>
                                    </table>
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
