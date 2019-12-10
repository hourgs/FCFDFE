<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CIMS_C12_2.aspx.cs" Inherits="FCFDFE.pages.CIMS.C.CIMS_C12_2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self"/>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <meta name="description" content=""/>
    <meta name="author" content="Mosaddek"/>
    <meta name="keyword" content="FlatLab, Dashboard, Bootstrap, Admin, Template, Theme, Responsive, Fluid, Retina"/>
    <%--<link rel="shortcut icon" href="img/favicon.html">--%>

    <!-- Bootstrap core CSS -->
    <link href="~/assets/css/bootstrap.css" rel="stylesheet"/>
    <link href="~/assets/css/bootstrap-reset.css" rel="stylesheet"/>
    <!--external css-->
    <link href="~/assets/assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="~/assets/assets/jquery-easy-pie-chart/jquery.easy-pie-chart.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="~/assets/css/owl.carousel.css" rel="stylesheet" type="text/css"/>
    <!--picker-->
    <link rel="stylesheet" type="text/css" href="~/assets/assets/bootstrap-datepicker/css/datepicker.css" />
    <link rel="stylesheet" type="text/css" href="~/assets/assets/bootstrap-datetimepicker/css/bootstrap-datetimepicker.css" />
    <link rel="stylesheet" type="text/css" href="~/assets/assets/bootstrap-colorpicker/css/colorpicker.css" />
    <link rel="stylesheet" type="text/css" href="~/assets/assets/bootstrap-daterangepicker/daterangepicker.css" />
    <!-- Custom styles for this template -->
    <link href="~/assets/css/style.css" rel="stylesheet"/>
    <link href="~/assets/css/style-responsive.css" rel="stylesheet" />
    
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    
</head>
<body>
    <script>
        function close() {
            window.close();
        }
    </script>
    <form id="form1" runat="server" >
        <div class="row">
            <div style="width: 800px; margin:auto;">
                <section class="panel">                          
                    <table class="table table-bordered text-center">                             
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">A.廠商名稱</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_A" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">B.是否為得標廠商</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_B" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">C.廠商英文名稱</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_C" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">D.廠商地址(縣市)</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_D" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                           <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">E.廠商地址(市政鄉區)</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_E" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">F.廠商地址(其他地址資料)  </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_F" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                           <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">G.廠商郵遞區號</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_G" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">H.廠商地址(英文) </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_H" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                           <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">I.廠商電話國碼</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_I" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">J.廠商電話區碼</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_J" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">K.廠商電話號碼</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_K" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">L.廠商電話分機</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_L" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">M.決標金額</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_M" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">N.履約起始日期</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_N" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">O.履約終止日期</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_O" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">P.是否為中小企業</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_P" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                           <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">Q.預估分包予中小企業之金額</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_Q" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">R.採購法第103條第2項 上級機關核准文號</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_R" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">S.僱用員工總人數是否超過100人</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_S" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">T.僱用員工總人數 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_T" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">U.已僱用原住民人數</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_U" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">V.已僱用身心障礙者人數 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_V" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">W.標案名稱</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_W" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">X.標案案號 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_X" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">Y.招標單位</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_Y" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">Z.聯絡電話 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_Z" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AA.聯絡人</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AA" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AB.機關名稱 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AB" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AC.附加說明</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AC" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AD.標比(％) </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_AD" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AE.契約編號</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AF.是否刊登公報 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AF" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AG.底價金額</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_AG" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AH.底價金額是否公開  </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AH" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AI.總決標金額</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_AI" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AJ.總決標金額是否公開 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AJ" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AK.契約是否訂有依物價指數調整價金規定</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AK" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AL.履約執行機關 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AL" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AM.委員</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AM" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AN.職稱 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AN" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AO.決標序號</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_AO" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AP.是否刊登英文公告 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AP" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AQ.決標日期</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AQ" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AR.決標公告日期 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AR" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AS.是否受機關補助</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AS" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AT.標的分類 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AT" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AU.是否屬優先採購身心障礙褔利機構產品或勞務</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AU" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AV.投標家數 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_AV" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AW.招標方式</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AW" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AX.決標品項名稱 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AX" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AY.單位</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AY" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">AZ.是否相同底價 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_AZ" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">BA.決標品項數</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_BA" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">BB.是否屬共同供應契約採購 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_BB" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">BC.決標方式</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_BC" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">BD.是否複數決標 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_BD" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">BE.是否屬公共工程實施技師簽證範圍</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_BE" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">BF.採購金額 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_BF" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">BG.開標時間</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_BG" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">BH.是否訂有底價 </asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_BH" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">BI.預算金額</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="ONB_BI" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                             <td style="width:12.5%"><asp:Label CssClass="control-label" runat="server">KEY：建檔編號</asp:Label></td>
                            <td class="text-left" style="width:37.5%">
                                <asp:Label ID="OVC_KEY" CssClass="control-label" runat="server"></asp:Label>
                            </td>
                        </tr>                        
                    </table>                       
                    <footer class="panel-footer" style="text-align: center;">
                        <asp:Button ID="btnclose" CssClass="btn-default btnw4" OnClientClick="close()" OnClick="btnclose_Click" Text="關閉視窗" runat="server"/>
                    </footer>
                </section>
            </div>
        </div>
    </form>
</body>
</html>
