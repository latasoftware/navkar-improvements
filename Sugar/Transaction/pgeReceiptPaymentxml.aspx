﻿<%@ Page Title="Reciept Payment Xml" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeReceiptPaymentxml.aspx.cs" Inherits="Sugar_pgeReceiptPaymentxml" %>

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

    <script type="text/javascript" src="../JS/DateValidation.js">
    </script>
    <script type="text/javascript">
        function SB() {
            var billno = document.getElementById('<%=txtdoc_no.ClientID %>').value;
            var TranType = document.getElementById('<%=drpTrnType.ClientID %>').value;
            window.open('../Report/rptReceptPaymenPrintt.aspx?Doc_no=' + billno + '&TranType=' + TranType)
        }</script>
    <script type="text/javascript">
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
        function Back() {
            window.open('../Transaction/PgeReceiptPaymentUtility.aspx', '_self');
        }
    </script>
    <script type="text/javascript" language="javascript">
        var SelectedRow = null;
        var SelectedRowIndex = null;
        var UpperBound = null;
        var LowerBound = null;
        function SelectSibling(e) {
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

                if (hdnfClosePopupValue == "txtVoucherNo") {
                    debugger;
                    document.getElementById("<%=txtVoucherNo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtvoucherType.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[9].innerText;
                    document.getElementById("<%=hdnflblvoucher.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[10].innerText;
                    document.getElementById("<%=txtamount.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[8].innerText;
                    document.getElementById("<%=txtadAmount.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[7].innerText;

                    var drp = document.getElementById('<%=drpFilter.ClientID %>');
                    var val = drp.options[drp.selectedIndex].value;
                    document.getElementById("<%=txtamount.ClientID %>").focus();
                    //                    if (val == "T") {
                    //                        document.getElementById("<%= hdnfTransportBalance.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[8].innerText;
                    //                        document.getElementById("<%=txtVoucherNo.ClientID %>").focus();
                    //                    }
                    //                    if (val == "S" || val == "V") {
                    //                        document.getElementById("<%= hdnfTransportBalance.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[11].innerText;
                    //                        document.getElementById("<%=txtVoucherNo.ClientID %>").focus();
                    //                    }

                }
                if (hdnfClosePopupValue == "txtVoucherNo1") {
                    debugger;
                    document.getElementById("<%=txtVoucherNo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtvoucherType.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[9].innerText;
                    document.getElementById("<%=hdnflblvoucher.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[10].innerText;
                    document.getElementById("<%=txtamount.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[8].innerText;
                    document.getElementById("<%=txtadAmount.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[7].innerText;

                    var drp = document.getElementById('<%=drpFilter.ClientID %>');
                    var val = drp.options[drp.selectedIndex].value;
                    document.getElementById("<%=txtamount.ClientID %>").focus();
                    //                    if (val == "T") {
                    //                        document.getElementById("<%= hdnfTransportBalance.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[8].innerText;
                    //                        document.getElementById("<%=txtVoucherNo.ClientID %>").focus();
                    //                    }
                    //                    if (val == "S" || val == "V") {
                    //                        document.getElementById("<%= hdnfTransportBalance.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[11].innerText;
                    //                        document.getElementById("<%=txtVoucherNo.ClientID %>").focus();
                    //                    }

                }
                if (hdnfClosePopupValue == "txtCashBank") {
                    document.getElementById("<%=txtCashBank.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblCashBank.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtCashBank.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtACCode") {
                    document.getElementById("<%=txtACCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblACName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtACCode.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtUnit_Code") {
                    document.getElementById("<%=txtUnit_Code.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblUnitName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtUnit_Code.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtnarration") {
                    document.getElementById("<%=txtnarration.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtNarration2.ClientID %>").focus();
                    //document.getElementById("<%=btnAdddetails.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtdoc_no") {
                    //document.getElementById("<%=txtdoc_no.ClientID %>").disabled = false;
                    document.getElementById("<%=txtdoc_no.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtdoc_date.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtEditDoc_No") {
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").value = "";
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").focus();
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
                if (hdnfClosePopupValue == "txtACCode") {
                    document.getElementById("<%=txtACCode.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtUnit_Code") {
                    document.getElementById("<%=txtUnit_Code.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtCashBank") {
                    document.getElementById("<%=txtCashBank.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtVoucherNo") {
                    document.getElementById("<%=txtVoucherNo.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtVoucherNo1") {
                    document.getElementById("<%=txtVoucherNo.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtnarration") {
                    document.getElementById("<%=txtnarration.ClientID %>").focus();
                }
                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";
                document.getElementById("<%= txtSearchText.ClientID %>").value = "";

            }

        });
    </script>
    <script type="text/javascript">
        function cashbank(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtCashBank.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtCashBank.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtCashBank.ClientID %>").val(unit);
                __doPostBack("txtCashBank", "TextChanged");

            }
        }
        function accode(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtACCode.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtACCode.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtACCode.ClientID %>").val(unit);
                __doPostBack("txtACCode", "TextChanged");

            }
            if (e.keyCode == 13) {
                e.preventDefault();
                document.getElementById("<%=btnSave.ClientID %>").focus();

            }
        }

        function unit(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtUnitcode.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtUnit_Code.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtUnit_Code.ClientID %>").val(unit);
                __doPostBack("txtUnit_Code", "TextChanged");

            }
        }
        function voucherno(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtVoucherNo.ClientID %>").click();

            }
            if (e.keyCode == 113) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnsoudaall.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtVoucherNo.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtVoucherNo.ClientID %>").val(unit);
                __doPostBack("txtVoucherNo", "TextChanged");

            }
        }
        function vouchertype(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnsoudaall.ClientID %>").click();

            }
            //  if (e.keyCode == 9) {
            // e.preventDefault();
            // var unit = $("#<%=txtvoucherType.ClientID %>").val();

            // unit = "0" + unit;
            // $("#<%=txtvoucherType.ClientID %>").val(unit);
            //__doPostBack("txtvoucherType", "TextChanged");

            // }
        }
        function narration(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtnarration.ClientID %>").click();

            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%; margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="   Receipt/Payment   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdconfirm" runat="server" />
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfTran_type" runat="server" />
            <asp:HiddenField ID="hdnfSuffix" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <asp:HiddenField ID="hdnfTransportBalance" runat="server" />
            <asp:HiddenField ID="hdnflblvoucher" runat="server" />
            <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                <table width="80%" align="left">
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="90px"
                                TabIndex="16" ValidationGroup="save" OnClick="btnAdd_Click" Height="24px" />
                            &nbsp;
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnHelp" Width="90px"
                            TabIndex="17" ValidationGroup="add" OnClick="btnSave_Click" Height="24px" />
                            &nbsp;
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="90px"
                            TabIndex="18" ValidationGroup="save" OnClick="btnEdit_Click" Height="24px" />
                            &nbsp;
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="90px"
                            TabIndex="19" ValidationGroup="add" OnClick="btnDelete_Click" OnClientClick="Confirm()" Height="24px" />
                            &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="90px"
                            TabIndex="20" Height="24px" ValidationGroup="save" OnClick="btnCancel_Click" />
                            &nbsp;
                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btnHelp" Width="90px"
                            TabIndex="20" Height="24px" ValidationGroup="save" OnClientClick="Back()" />
                            <asp:Button runat="server" ID="btnPrintSaleBill" Text="Print" CssClass="btnHelp"
                                Width="80px" Height="24px" OnClientClick="SB();" />
                        </td>

                    </tr>
                </table>
                <br />
                <br />
                <table align="left" cellspacing="5">
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Transaction Type:
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="drpTrnType" runat="server" CssClass="ddl" Width="200px" Height="24px"
                                TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="drpTrnType_SelectedIndexChanged">
                                <asp:ListItem Text="Cash Payment" Value="CP"></asp:ListItem>
                                <asp:ListItem Text="Cash Receipt" Value="CR"></asp:ListItem>
                                <asp:ListItem Text="Bank Payment" Value="BP"></asp:ListItem>
                                <asp:ListItem Text="Bank Receipt" Value="BR"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="left">Change No:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtEditDoc_No" CssClass="txt" Width="100px" Height="24px"
                                TabIndex="0" AutoPostBack="true" OnTextChanged="txtEditDoc_No_TextChanged"></asp:TextBox>
                            &nbsp;<asp:Label ID="Label2" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                        <td align="left" style="width: 10%;">Entry No:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtdoc_no" runat="Server" CssClass="txt" TabIndex="0" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtdoc_no_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtdoc_no" runat="server" Text="..." Width="80px" OnClick="btntxtdoc_no_Click"
                                CssClass="btnHelp" Height="24px" />
                            <asp:Label ID="lblReceiptPayment_Id" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                            <ajax1:FilteredTextBoxExtender ID="Filteretxtdoc_no" runat="server" TargetControlID="txtdoc_no"
                                FilterType="Numbers">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                        <td align="right" style="width: 10%;">Date:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtdoc_date" runat="Server" CssClass="txt" TabIndex="2" Width="90px"
                                MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtdoc_date_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" />
                            <ajax1:CalendarExtender ID="calenderExtenderDate" runat="server" TargetControlID="txtdoc_date"
                                PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Cash/Bank:
                        </td>
                        <td align="left" colspan="5">
                            <asp:TextBox ID="txtCashBank" runat="server" CssClass="txt" Style="text-align: right;"
                                onkeydown="cashbank(event);" AutoPostBack="false" TabIndex="3" OnTextChanged="txtCashBank_TextChanged" Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtCashBank" runat="server" Text="..." OnClick="btntxtCashBank_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblCashBank" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>

                </table>
                <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%; margin-left: 30px; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px; height: 7px;">
                    <legend style="text-align: center;">
                        <h5 style="color: White;" font-names="verdana" font-size="Medium">Detail Entry</h5>
                    </legend>
                </fieldset>
                <table width="90%" align="center">

                    <tr>
                        <td align="right">
                            <asp:Label ID="lblNo" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblID" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">A/C Code:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtACCode" runat="Server" CssClass="txt" Width="80px" Style="text-align: right;"
                                onkeydown="accode(event);" TabIndex="4" AutoPostBack="false" OnTextChanged="txtACCode_TextChanged" Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtACCode" runat="server" Text="..." OnClick="btntxtACCode_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblACName" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Unit:
                        </td>
                        <td align="left" colspan="4">
                            <asp:TextBox ID="txtUnit_Code" runat="Server" CssClass="txt" TabIndex="5" Width="80px"
                                onkeydown="unit(event);" Style="text-align: right;" AutoPostBack="false" Height="24px" OnTextChanged="txtUnit_Code_TextChanged"></asp:TextBox>
                            <asp:Button ID="btntxtUnitcode" runat="server" Text="..." CssClass="btnHelp" Height="24px"
                                Width="20px" OnClick="btntxtUnitcode_Click" />
                            <asp:Label ID="lblUnitName" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Select:
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="drpFilter" runat="server" CssClass="ddl" Width="280px" Height="25px"
                                Visible="true" AutoPostBack="true" TabIndex="6" OnSelectedIndexChanged="drpFilter_SelectedIndexChanged"
                                OnTextChanged="drpFilter_SelectedIndexChanged">
                                <asp:ListItem Text="Againt Loading Voucher" Value="V"></asp:ListItem>
                                <asp:ListItem Text="Againt Sauda" Value="S"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label Text="Voucher Number:" runat="server" ID="lblHead"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtVoucherNo" Height="24px" runat="Server" CssClass="txt" Width="80px"
                                onkeydown="voucherno(event);" Style="text-align: right;" TabIndex="7" AutoPostBack="false" OnTextChanged="txtVoucherNo_TextChanged"></asp:TextBox>
                            <asp:Button ID="btntxtVoucherNo" runat="server" Text="..." OnClick="btntxtVoucherNo_Click"
                                TabIndex="8" CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:TextBox ID="txtvoucherType" Enabled="false" runat="Server" CssClass="txt" Width="20px"
                                onkeydown="vouchertype(event);" Style="text-align: right;" AutoPostBack="False" Height="24px"></asp:TextBox>
                            <asp:Label ID="lblVoucherBy" runat="server" CssClass="lblName"></asp:Label>
                            <asp:Button ID="btnsoudaall" runat="server" Text="..." OnClick="btnsoudaall_Click"
                                TabIndex="8" CssClass="btnHelp" Height="24px" Width="20px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Amount:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtamount" runat="Server" TabIndex="9" Height="24px" CssClass="txt"
                                Width="80px" Style="text-align: right;" AutoPostBack="true" OnTextChanged="txtamount_TextChanged"></asp:TextBox><asp:Label
                                    runat="server" ID="lblErrorAdvance" Text="" ForeColor="Red"></asp:Label>
                            <ajax1:FilteredTextBoxExtender ID="filterAmount" runat="server" TargetControlID="txtamount"
                                FilterType="Custom,Numbers" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp;&nbsp;&nbsp;
                            Adjusted Amount:
                            <asp:TextBox ID="txtadAmount" runat="Server" CssClass="txt" Height="24px" Width="80px"
                                Style="text-align: right;" AutoPostBack="false" TabIndex="10" OnTextChanged="txtadAmount_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FiltertxtadAmount" runat="server" TargetControlID="txtamount"
                                FilterType="Custom,Numbers" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                    </tr>
                    <%--<tr>
                        <td align="left" >
                            UTR Narration:
                        </td>
                        <td align="left" >
                            <asp:TextBox ID="txtUtrNarration" runat="Server" CssClass="txt" Height="24px" Width="80px"
                                Style="text-align: right;" AutoPostBack="false" TabIndex="10" OnTextChanged="txtadAmount_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtamount"
                                FilterType="Custom,Numbers" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>--%>
                    <tr>
                        <td align="left">Narration:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtnarration" runat="Server" CssClass="txt" Width="480px" Height="50px"
                                onkeydown="narration(event);" TextMode="MultiLine" Style="text-align: left;" AutoPostBack="True" TabIndex="11"
                                OnTextChanged="txtnarration_TextChanged"></asp:TextBox>
                            <asp:Button ID="btntxtnarration" runat="server" Text="..." OnClick="btntxtnarration_Click"
                                CssClass="btnHelp" Width="20px" Height="24px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Narration 2:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtNarration2" runat="Server" CssClass="txt" Width="480px" Height="24px"
                                Style="text-align: left;" AutoPostBack="True" TabIndex="12" OnTextChanged="txtNarration2_TextChanged"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2" align="left">
                            <asp:Button ID="btnAdddetails" runat="server" Text="ADD" CssClass="btnHelp" Width="80px"
                                Height="24px" OnClick="btnAdddetails_Click" TabIndex="14" />
                            <asp:Button ID="btnClosedetails" runat="server" Text="Close" CssClass="btnHelp"
                                Height="24px" Width="80px" OnClick="btnClosedetails_Click" TabIndex="15" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="width: 100%; position: relative;">
                <asp:UpdatePanel ID="upGrid" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlgrdDetail" runat="server" ScrollBars="Both" Height="250px" Width="1300px"
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
            <table width="70%" align="left">
                <tr>
                    <td align="right">
                        <asp:Label ID="lblTotal" runat="server" CssClass="lblName" Font-Bold="true" Text="Total:"></asp:Label>&nbsp;<asp:TextBox
                            ID="txtTotal" runat="server" ReadOnly="true" CssClass="txt" Width="100px" Height="25px" Style="text-align: right;"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />

            <asp:Panel ID="pnlPopup" onkeydown="closepopup(event);" runat="server" Width="80%"
                align="center" ScrollBars="None" BackColor="#FFFFE4" Direction="LeftToRight"
                Style="z-index: 5000; position: absolute; display: none; float: right; max-height: 500px; min-height: 500px; box-shadow: 1px 1px 8px 2px; background-position: center; left: 10%; top: 10%;">
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
                            <asp:TextBox ID="txtSearchText" onkeydown="SelectFirstRow(event);" runat="server"
                                Width="250px" Height="20px" AutoPostBack="true" OnTextChanged="txtSearchText_TextChanged"></asp:TextBox>
                            <asp:Button ID="btnSearch" onkeydown="SelectFirstRow(event);" runat="server" Text="Search"
                                CssClass="btnSubmit" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlInner" runat="server" Width="100%" Direction="LeftToRight" BackColor="#FFFFE4"
                                Style="z-index: 5000; float: right; overflow: auto; height: 400px">
                                <asp:GridView ID="grdPopup" Font-Bold="true" CssClass="select" runat="server" AutoGenerateColumns="true"
                                    EmptyDataText="No Records Found" HeaderStyle-BackColor="#6D8980" HeaderStyle-ForeColor="White"
                                    AllowPaging="true" PageSize="20" OnRowCreated="grdPopup_RowCreated" OnRowDataBound="grdPopup_RowDataBound"
                                    OnPageIndexChanging="grdPopup_PageIndexChanging" Style="table-layout: fixed;"
                                    OnSelectedIndexChanged="grdPopup_SelectedIndexChanged">
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
            <%-- <asp:Panel ID="pnlPopupDetails" runat="server" BackColor="GhostWhite" Width="955px"
                BorderColor="Teal" BorderWidth="1px" Height="400px" BorderStyle="Solid" Style="z-index: 4999; left: 20%; top: 20%; position: absolute; display: none; font-weight: bold; box-shadow: 1px 2px 10px 2px;">
            </asp:Panel>--%>
            <%--<ajax1:ModalPopupExtender runat="server" ID="popup1" CancelControlID="btnClosedetails"
                PopupControlID="pnlPopupDetails" TargetControlID="btnOpenDetailsPopup">
            </ajax1:ModalPopupExtender>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
