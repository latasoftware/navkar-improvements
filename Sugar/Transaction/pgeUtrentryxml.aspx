<%@ Page Title="UTR Entry" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeUtrentryxml.aspx.cs" Inherits="pgeUtrentryxml" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../CSS/NewModalPopup.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Script/jquery-1.4.2.js"></script>

    <link href="../../CSS/cssCommon.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../JQuery/jquery-1.4.1.js" type="text/javascript"></script>
    <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <script src="../../JQuery/jquery.keynavigation.js" type="text/javascript"></script>
    <link type="text/css" href="../../menu/menu.css" rel="stylesheet" />
    <script type="text/javascript" src="../../menu/jquery.js"></script>
    <script type="text/javascript" src="../../menu/menu.js"></script>
    <script type="text/javascript" src="../JS/DateValidation.js"> </script>

    <script src="../../JS/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="../../Scripts/selectfirstrow.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to delete data?")) {
                confirm_value.value = "Yes";
                document.getElementById("<%= hdconfirm.ClientID %>").value = "Yes";
            }
            else {
                confirm_value.value = "No";
                document.getElementById("<%= hdconfirm.ClientID %>").value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
        function UTRReport(docno) {
           
            window.open('../Report/rptUtrReport_Print.aspx?docno=' + docno);
        }
        function Back() {
            window.open('../Transaction/PgeUTRHeadUtility.aspx', '_self');
        }
    </script>


    <script type="text/javascript" language="javascript">
        debugger;
        document.addEventListener('keyup', function (event) {
            if (event.defaultPrevented) {
                return;
            }

            var key = event.key || event.keyCode;

            if (key === 'Escape' || key === 'Esc' || key === 27) {
                //                doWhateverYouWantNowThatYourKeyWasHit();
                debugger;
                document.getElementById("<%=pnlPopup.ClientID %>").style.display = "none";
                var hdnfClosePopupValue = document.getElementById("<%= hdnfClosePopup.ClientID %>").value;
                if (hdnfClosePopupValue == "txtbank_ac") {
                    document.getElementById("<%=txtbank_ac.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtmill_code") {
                    document.getElementById("<%=txtmill_code.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtlotno_Detail") {
                    document.getElementById("<%=txtlotno_Detail.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtgrade_no") {
                    document.getElementById("<%=txtgrade_no.ClientID %>").focus();
                }
                document.getElementById("<%=txtSearchText.ClientID %>").value = "";
                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";
            }

        });
    </script>

    <script type="text/javascript" language="javascript">
        var SelectedRow = null;
        var SelectedRowIndex = null;
        var UpperBound = null;
        var LowerBound = null;
        function SelectSibling(e) {
            debugger;
            var e = e ? e : window.event;
            var KeyCode = e.which ? e.which : e.keyCode;
            if (KeyCode == 40) {
                SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
            }
            else if (KeyCode == 38) {
                SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);
            }
            else if (KeyCode == 13) {
                document.getElementById("<%=pnlPopup.ClientID %>").style.display = "none";
                document.getElementById("<%=txtSearchText.ClientID %>").value = "";
                var hdnfClosePopupValue = document.getElementById("<%= hdnfClosePopup.ClientID %>").value;
                var grid = document.getElementById("<%= grdPopup.ClientID %>");
                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";
                var pageCount = document.getElementById("<%= hdHelpPageCount.ClientID %>").value;
                pageCount = parseInt(pageCount);
                if (pageCount > 1) {
                    SelectedRowIndex = SelectedRowIndex + 1;
                }
                if (hdnfClosePopupValue == "txtEditDoc_No") {
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").value = "";
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtbank_ac") {
                    document.getElementById("<%= txtbank_ac.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%= lblbank_Name.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtbank_ac.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtmill_code") {
                    document.getElementById("<%= txtmill_code.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%= lblmill_name.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtmill_code.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtlotno_Detail") {

                    document.getElementById("<%= txtlotno_Detail.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%= lblyear_Code_Detail.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[7].innerText;
                    document.getElementById("<%= hdyearcodedetail.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[7].innerText;
                    document.getElementById("<%= txtgrade_no.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[8].innerText;
                    document.getElementById("<%= hdnfpodetailid.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[6].innerText;
                    document.getElementById("<%=txtlotno_Detail.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtgrade_no") {
                    document.getElementById("<%= txtgrade_no.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;


                    document.getElementById("<%=txtgrade_no.ClientID %>").focus();
                }
            }
}
function SelectRow(CurrentRow, RowIndex) {
    UpperBound = parseInt('<%= this.grdPopup.Rows.Count %>') - 1;
    LowerBound = 0;
    if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound)
        if (SelectedRow != null) {
            SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
            SelectedRow.style.color = SelectedRow.originalForeColor;
        }
    if (CurrentRow != null) {
        CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
        CurrentRow.originalForeColor = CurrentRow.style.color;
        CurrentRow.style.backgroundColor = '#DCFC5C';
        CurrentRow.style.color = 'Black';
    }
    SelectedRow = CurrentRow;
    SelectedRowIndex = RowIndex;
    setTimeout("SelectedRow.focus();", 0);
}
    </script>
    <script type="text/javascript">
        function enable(invoker) {
            var textbox = document.getElementById("txtMillMobile");
            document.getElementById('<%= txtMillMobile.ClientID%>').disabled = invoker.checked ? false : true;
            if (!document.getElementById('<%= txtMillMobile.ClientID%>').disabled) {
                textbox.focus();
            }
        }
    </script>
    <script type="text/javascript">
        function text() {
            var txtmobile = document.getElementById('<%= txtMillMobile.ClientID%>').value;
        }

    </script>
    <script type="text/javascript" src="../JS/DateValidation.js">
    </script>
    <script type="text/javascript">
        function chanegno(e) {
            if (e.keyCode == 112) {
                e.preventDefault();
                var edi = "txtEditDoc_No";
                ("#<%=hdnfClosePopup.ClientID %> ").val(edi);
                ("#<%=btnSearch.ClientID %> ").click();
            }
            if (e.keyCode == 9) {
                e.preventDefault();
                __doPostBack("txtEditDoc_No", "TextChanged");
            }
        }
        function bank_ac(e) {
            debugger;
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtbank_ac.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtbank_ac.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtbank_ac.ClientID %>").val(unit);
                __doPostBack("txtbank_ac", "TextChanged");

            }
        }
        function MillCode(e) {
            debugger;
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtmill_code.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtmill_code.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtmill_code.ClientID %>").val(unit);
                __doPostBack("txtmill_code", "TextChanged");

            }

        }


        function grade_no(e) {
            if (e.keyCode == 112) {
                e.preventDefault();
                $("#<%=btntxtgrade_no.ClientID %> ").click();
            }
            
        }

        function Focusbtn(e) {
            debugger;
            if (e.keyCode == 112) {
                e.preventDefault();
                $("#<%=btntxtlot_no_Detail.ClientID %> ").click();
            }
            if (e.keyCode == 9) {
                e.preventDefault();
                __doPostBack("txtlotno_Detail", "TextChanged");
            }
            if (e.keyCode == 13) {
                e.preventDefault();
                $("#<%=btnSave.ClientID %>").focus();
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%; margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="UTR Entry" Font-Names="verdana" ForeColor="White"
                Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfSuffix" runat="server" />
            <asp:HiddenField ID="hdconfirm" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <asp:HiddenField ID="hdyearcode" runat="server" />
            <asp:HiddenField ID="hdyearcodedetail" runat="server" />
            <asp:HiddenField ID="hdnfpodetailid" runat="server" />
            <table width="80%" align="left">
                <tr>
                    <td align="center">
                        <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" OnClick="btnAdd_Click" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnHelp" Width="90px"
                            ValidationGroup="add" OnClick="btnSave_Click" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" OnClick="btnEdit_Click" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="90px"
                            ValidationGroup="add" OnClick="btnDelete_Click" OnClientClick="Confirm()" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" OnClick="btnCancel_Click" Height="24px" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" Height="24px" TabIndex="24" OnClientClick="Back();" />
                        &nbsp;<asp:Button runat="server" ID="btnUtrReport" Text="Print" Width="80px" Height="24px"
                           OnClick="btnUtrReport_Click" CssClass="btnHelp"  />
                    </td>
                    <%-- <td align="center">
                        <asp:Button ID="btnFirst" runat="server" Text="<<" ToolTip="First" CssClass="btnHelp"
                            OnClick="btnFirst_Click" Width="90px" Height="24px" />
                        <asp:Button ID="btnPrevious" runat="server" Text="<" ToolTip="First" CssClass="btnHelp"
                            OnClick="btnPrevious_Click" Width="90px" Height="24px" />
                        <asp:Button ID="btnNext" runat="server" Text=">" ToolTip="First" CssClass="btnHelp"
                            OnClick="btnNext_Click" Width="90px" Height="24px" />
                        <asp:Button ID="btnLast" runat="server" Text=">>" ToolTip="First" CssClass="btnHelp"
                            OnClick="btnLast_Click" Width="90px" Height="24px" />
                    </td>--%>
                </tr>
            </table>
            <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                <table width="100%" align="center">
                    <tr>
                        <td align="right">Mill Mobile:<asp:TextBox runat="server" ID="txtMillMobile" Enabled="true" CssClass="txt"
                            MaxLength="10" Width="200px" Height="24px"></asp:TextBox>
                            &nbsp;<asp:CheckBox runat="server" ID="rbEditMobile" Text="Edit" OnClick="enable(this);" />&nbsp;<asp:Button
                                runat="server" ID="btnSendSMS" Text="Send Sms" CssClass="btnHelp" Height="24px"
                                OnClick="btnSendSMS_Click" Width="100px" />
                        </td>
                    </tr>
                </table>
                <table style="width: 60%;" align="Left" cellpadding="4" cellspacing="4">
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                        <tr>
                            <td align="left">Change No
                            </td>
                            <td align="left">
                                <asp:TextBox Height="24px" ID="txtEditDoc_No" runat="Server" CssClass="txt" TabIndex="1"
                                    Width="90px" Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtEditDoc_No_TextChanged"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">Entry No
                            </td>
                            <td align="left">
                                <asp:TextBox Height="24px" ID="txtdoc_no" runat="Server" CssClass="txt" TabIndex="2"
                                    Width="90px" Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtdoc_no_TextChanged"></asp:TextBox>
                                <asp:Button ID="btntxtdoc_no" runat="server" Text="..." Width="80px" OnClick="btntxtdoc_no_Click"
                                    CssClass="btnHelp" Height="24px" />
                                <asp:Label ID="lblUtr_Id" runat="server" CssClass="lblName" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">Entry Date
                            </td>
                            <td align="left">
                                <asp:TextBox Height="24px" ID="txtdoc_date" runat="Server" CssClass="txt" TabIndex="3"
                                    Width="90px" Style="text-align: left;" AutoPostBack="true" OnTextChanged="txtdoc_date_TextChanged"
                                    onkeyup="ValiddateDate(this,event.keyCode)" onkeydown="retun DateFormat(this,event.keyCode)"></asp:TextBox>
                                <asp:Image ID="imgcalendertxtdoc_date" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                    Width="25px" Height="15px" /><ajax1:CalendarExtender ID="CalendarExtenderDatetxtdoc_date"
                                        runat="server" TargetControlID="txtdoc_date" PopupButtonID="imgcalendertxtdoc_date"
                                        Format="dd/MM/yyyy">
                                    </ajax1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">Bank Code
                            </td>
                            <td align="left">
                                <asp:TextBox Height="24px" ID="txtbank_ac" runat="Server" CssClass="txt" TabIndex="4"
                                    Width="90px" Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtbank_ac_TextChanged" onkeydown="bank_ac(event);"></asp:TextBox>
                                <asp:Button Width="20px" Height="24px" ID="btntxtbank_ac" runat="server" Text="..."
                                    OnClick="btntxtbank_ac_Click" CssClass="btnHelp" />
                                <asp:Label ID="lblbank_Name" runat="server" CssClass="lblName"></asp:Label>
                                <asp:Label ID="lblbank_id" runat="server" CssClass="lblName" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">Mill Code
                            </td>
                            <td align="left">
                                <asp:TextBox Height="24px" ID="txtmill_code" runat="Server" CssClass="txt" TabIndex="5"
                                    Width="90px" Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtmill_code_TextChanged" onkeydown="MillCode(event);"></asp:TextBox>
                                <asp:Button Width="20px" Height="24px" ID="btntxtmill_code" runat="server" Text="..."
                                    OnClick="btntxtmill_code_Click" CssClass="btnHelp" />
                                <asp:Label ID="lblmill_name" runat="server" CssClass="lblName"></asp:Label>
                                <asp:Label ID="lblmill_id" runat="server" CssClass="lblName" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <%--<tr>
                            <td align="left">
                                Lot No
                            </td>
                            <td align="left">
                                <asp:TextBox Height="24px" ID="txtlot_no" runat="Server" CssClass="txt" TabIndex="6"
                                    Width="90px" Style="text-align: left;" AutoPostBack="true" OnTextChanged="txtlot_no_TextChanged"></asp:TextBox>
                                <asp:Button Width="20px" Height="24px" ID="btntxtlot_no" runat="server" Text="..."
                                    OnClick="btntxtlot_no_Click" CssClass="btnHelp" />
                                <asp:Label ID="lblyear_code" runat="server" CssClass="lblName"></asp:Label>
                                <asp:Label ID="lblCompnycode" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>--%>
                        <tr>
                            <td align="left">Amount
                            </td>
                            <td align="left">
                                <asp:TextBox Height="24px" ID="txtamount" runat="Server" CssClass="txt" TabIndex="7"
                                    Width="90px" Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtamount_TextChanged"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtamount">
                                </ajax1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">Utr No
                            </td>
                            <td align="left">
                                <asp:TextBox Height="24px" ID="txtunt_no" runat="Server" CssClass="txt" TabIndex="8"
                                    Width="300px" Style="text-align: left;" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">Narration Header
                            </td>
                            <td align="left">
                                <asp:TextBox Height="24px" ID="txtnarration_header" runat="Server" CssClass="txt"
                                    TabIndex="9" Width="300px" Style="text-align: left;" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">Narration Footer
                            </td>
                            <td align="left">
                                <asp:TextBox Height="24px" ID="txtnarration_footer" runat="Server" CssClass="txt"
                                    TabIndex="10" Width="300px" Style="text-align: left;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">Is Save
                            </td>
                            <td align="left">
                                <asp:CheckBox runat="server" ID="chkIsSave" Width="25px" Height="20px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <asp:Button ID="btnOpenDetailsPopup" runat="server" Text="ADD" CssClass="btnHelp"
                                    Width="80px" Height="25px" OnClick="btnOpenDetailsPopup_Click" TabIndex="28"
                                    Visible="false" />
                            </td>
                        </tr>
                </table>
                <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%; margin-left: 30px; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px; height: 7px;">
                    <legend style="text-align: center;">
                        <h5 style="color: White;" font-names="verdana" font-size="Medium">Detail Entry</h5>
                    </legend>
                </fieldset>
                <table width="80%" align="Left">

                    <tr>
                        <td align="left">
                            <asp:Label ID="lblID" runat="server" CssClass="lblName" Font-Names="verdana" Text=""
                                Visible="false"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblNo" runat="server" CssClass="lblName" Font-Names="verdana" Text=""
                                Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Lot No
                            <asp:TextBox ID="txtlotno_Detail" runat="Server" CssClass="txt" TabIndex="11" Width="90px"
                                Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtlotno_Detail_TextChanged"
                                Height="24px" onKeyDown="Focusbtn(event);"></asp:TextBox>
                            <asp:Button ID="btntxtlot_no_Detail" runat="server" Text="..." OnClick="btntxtlot_no_Detail_Click"
                                CssClass="btnHelp" />
                            <asp:Label ID="lblyear_Code_Detail" runat="server" CssClass="lblName"></asp:Label>
                            <asp:Label ID="lblCompnycode_Detail" runat="server" CssClass="lblName"></asp:Label>
                            Grade
                            <asp:TextBox ID="txtgrade_no" runat="Server" CssClass="txt" TabIndex="12" Width="90px"
                                Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtgrade_no_TextChanged"
                                Height="24px" onKeyDown="grade_no(event);" ></asp:TextBox>
                            <asp:Button ID="btntxtgrade_no" runat="server" Text="..." OnClick="btntxtgrade_no_Click"
                                CssClass="btnHelp" />
                            <%-- <asp:Label ID="lblgrade_name" runat="server" CssClass="lblName"></asp:Label>--%>
                            Amount
                            <asp:TextBox ID="txtamount_Detail" runat="Server" CssClass="txt" TabIndex="13" Width="90px"
                                Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtamount_Detail_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers,Custom"
                                ValidChars="." TargetControlID="txtamount_Detail">
                            </ajax1:FilteredTextBoxExtender>
                            Adjusted Amount
                            <asp:TextBox ID="txtAdjusted_Amt" runat="Server" CssClass="txt" TabIndex="13" Width="90px"
                                Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtAdjusted_Amt_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers,Custom"
                                ValidChars="." TargetControlID="txtAdjusted_Amt">
                            </ajax1:FilteredTextBoxExtender>
                            <asp:Button ID="btnAdddetails" runat="server" Text="ADD" CssClass="btnHelp" Width="80px"
                                Height="25px" OnClick="btnAdddetails_Click" TabIndex="14" />
                            <asp:Button ID="btnClosedetails" runat="server" Text="Close" CssClass="btnHelp" Width="80px"
                                Height="25px" OnClick="btnClosedetails_Click" TabIndex="28" />
                        </td>


                    </tr>
                </table>

            </asp:Panel>


            <div style="width: 100%; position: relative;">

                <asp:UpdatePanel ID="upGrid" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlgrdDetail" runat="server" ScrollBars="Both" Height="200px" Width="1000px"
                            BorderColor="Maroon" BorderWidth="1px" BorderStyle="Solid" Font-Bold="true" Font-Names="Verdana"
                            Font-Size="11px" BackColor="SeaShell" Style="margin-left: 30px; float: left;">
                            <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="true" HeaderStyle-BackColor="#397CBB"
                                HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" GridLines="Both" Width="100%"
                                OnRowCommand="grdDetail_RowCommand" CellPadding="5" CellSpacing="5" OnRowDataBound="grdDetail_RowDataBound"
                                Style="table-layout: fixed;">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditRecord" Text="Edit"
                                                CommandArgument="lnk"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteRecord" Text="Delete"
                                                CommandArgument="lnk"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle Height="25px" Wrap="false" ForeColor="Black" />
                            </asp:GridView>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <table width="80%" align="left">
                <caption>

                    <b>Total Amount:</b>
                    <asp:Label ID="lblGridTotal" runat="server" CssClass="lblName" Font-Bold="true" Font-Size="Large" ForeColor="Black"></asp:Label>
                    </tr>
                     <b>Difference:</b>
                    <asp:Label ID="lblDiff" runat="server" CssClass="lblName" Font-Bold="true" Font-Size="Large" ForeColor="Black"></asp:Label>
                </caption>



            </table>

            <asp:Panel onkeydown="closepopup(event);" ID="pnlPopup" runat="server" Width="70%"
                align="center" ScrollBars="None" BackColor="#FFFFE4" Direction="LeftToRight"
                Style="z-index: 5000; position: absolute; display: none; float: right; max-height: 1000px; min-height: 700px; box-shadow: 1px 1px 8px 2px; background-position: center; left: 10%; top: 10%;">
                <asp:ImageButton ID="imgBtnClose" runat="server" ImageUrl="~/Images/closebtn.jpg"
                    Width="20px" Height="20px" Style="float: right; vertical-align: top;" OnClick="imgBtnClose_Click"
                    ToolTip="Close" />
                <table width="95%">
                    <tr>
                        <td align="center" style="background-color: #F5B540; width: 100%;">
                            <asp:Label ID="lblPopupHead" runat="server" Font-Size="Medium" Font-Names="verdana"
                                Font-Bold="true" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Search Text:
                            <asp:TextBox onkeydown="SelectFirstRow(event);" ID="txtSearchText" runat="server"
                                Width="250px" Height="20px" AutoPostBack="false" OnTextChanged="txtSearchText_TextChanged"></asp:TextBox>
                            <asp:Button onkeydown="closepopup(event);" ID="btnSearch" runat="server" Text="Search"
                                CssClass="btnSubmit" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlInner" runat="server" Width="100%" Direction="LeftToRight" BackColor="#FFFFE4"
                                Style="z-index: 5000; float: right; overflow: auto; height: 680px">
                                <asp:GridView Font-Bold="true" CssClass="select" AllowPaging="true" PageSize="25"
                                    OnPageIndexChanging="grdPopup_PageIndexChanging" ID="grdPopup" runat="server"
                                    AutoGenerateColumns="true" EmptyDataText="No Records Found" HeaderStyle-BackColor="#6D8980"
                                    HeaderStyle-ForeColor="White" OnRowCreated="grdPopup_RowCreated" OnRowDataBound="grdPopup_RowDataBound"
                                    Style="table-layout: fixed;">
                                    <HeaderStyle Height="30px" ForeColor="White" BackColor="#6D8980" />
                                    <RowStyle Height="25px" ForeColor="Black" Wrap="false" />
                                    <PagerStyle BackColor="Tomato" ForeColor="White" Width="100%" Font-Bold="true" />
                                    <PagerSettings Position="TopAndBottom" />
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>


            <asp:Panel ID="pnlPopupDetails" runat="server" BackColor="GhostWhite" Width="955px"
                BorderColor="Teal" BorderWidth="1px" Height="300px" BorderStyle="Solid" Style="z-index: 4999; left: 20%; top: 20%; position: absolute; display: none; font-weight: bold; box-shadow: 1px 2px 10px 2px;">
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
